Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Bson
Imports Newtonsoft.Json.Serialization
Imports Newtonsoft.Json.Schema
Imports Newtonsoft.Json.Converters
Imports System.Data.SqlClient
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Net
Imports System.Text
Imports System.Environment
Imports System.IO
Imports System.Data.SqlServerCe
Imports System.Configuration
Imports System.Security.Cryptography
Imports Masterchest.mlib
Imports Org.BouncyCastle.Math.EC
Public Class sellcancelfrm


    Private Sub Form1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RectangleShape1.MouseDown

    End Sub

    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        Me.Close()
    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Close()
    End Sub

    Private Sub sellcancelfrm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

    End Sub

    Private Sub bsell_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsell.Click
        If Form1.workthread.IsBusy = True Then
            MsgBox("The background processing thread is currently modifying the state.  Please send your transaction when processing has finished.")
            Exit Sub
        End If
        If Len(lselladdress.Text) > 26 And Len(lselladdress.Text) < 35 Then 'sanity check
            Try
                Dim tmpcur
                If dexcur = "MSC" Then
                    tmpcur = 1
                End If
                If dexcur = "TMSC" Then
                    tmpcur = 2
                End If
                sentfrm.lsent.Text = "transaction failed"
                bsell.Enabled = False
                'get wallet passphrase
                passfrm.ShowDialog()
                If btcpass = "" Then Exit Sub 'cancelled
                Dim mlib As New Masterchest.mlib
                Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, lselladdress.Text, 0, 0))
                If validater.result.isvalid = True Then 'address is valid
                    txsummary = "Address is valid."
                    'push out to masterchest lib to encode the tx
                    Dim rawtx As String = mlib.encodeselltx(bitcoin_con, lselladdress.Text, tmpcur, 1, 1, 1, 1, 3)
                    'is rawtx empty
                    If rawtx = "" Then
                        txsummary = txsummary & vbCrLf & "Raw transaction is empty - stopping."
                        Exit Sub
                    End If
                    'decode the tx in the viewer
                    txsummary = txsummary & vbCrLf & "Raw transaction hex:" & vbCrLf & rawtx & vbCrLf & "Raw transaction decode:" & vbCrLf & mlib.rpccall(bitcoin_con, "decoderawtransaction", 1, rawtx, 0, 0)
                    'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
                    Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
                    Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(btcpass.ToString), 15, 0)
                    btcpass = ""
                    'try and sign transaction
                    Try
                        Dim signedtxn As signedtx = JsonConvert.DeserializeObject(Of signedtx)(mlib.rpccall(bitcoin_con, "signrawtransaction", 1, rawtx, 0, 0))
                        If signedtxn.result.complete = True Then
                            txsummary = txsummary & vbCrLf & "Signing appears successful."
                            Dim broadcasttx As broadcasttx = JsonConvert.DeserializeObject(Of broadcasttx)(mlib.rpccall(bitcoin_con, "sendrawtransaction", 1, signedtxn.result.hex, 0, 0))
                            If broadcasttx.result <> "" Then
                                txsummary = txsummary & vbCrLf & "Transaction sent, ID: " & broadcasttx.result.ToString
                                sentfrm.lsent.Text = "transaction sent"
                                senttxid = broadcasttx.result.ToString
                                Application.DoEvents()
                                If Form1.workthread.IsBusy <> True Then
                                    Form1.UIrefresh.Enabled = False
                                    Form1.syncicon.Visible = True
                                    Form1.lsyncing.Visible = True
                                    Form1.poversync.Image = My.Resources.gif
                                    Form1.loversync.Text = "Synchronizing..."
                                    Form1.lsyncing.Text = "Synchronizing..."
                                    ' Start the workthread for the blockchain scanner
                                    Form1.workthread.RunWorkerAsync()
                                End If
                                Application.DoEvents()
                                sentfrm.ShowDialog()
                                bsell.Enabled = True
                                Me.Close()
                                Exit Sub
                            Else
                                txsummary = txsummary & vbCrLf & "Error sending transaction."
                                sentfrm.lsent.Text = "transaction failed"
                                sentfrm.ShowDialog()
                                Exit Sub
                            End If
                        Else
                            txsummary = txsummary & vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct."
                            sentfrm.lsent.Text = "transaction failed"
                            sentfrm.ShowDialog()
                            Exit Sub
                        End If
                    Catch ex As Exception
                        txsummary = txsummary & vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct.  " & ex.Message
                        sentfrm.lsent.Text = "transaction failed"
                        sentfrm.ShowDialog()
                        Exit Sub
                    End Try
                Else
                    txsummary = "Build transaction failed.  Recipient address is not valid."
                    sentfrm.lsent.Text = "transaction failed"
                    sentfrm.ShowDialog()
                    Exit Sub
                End If
                sentfrm.ShowDialog()
            Catch ex As Exception
                MsgBox("Exeption thrown : " & ex.Message)
                sentfrm.ShowDialog()
            End Try
        End If
    End Sub
End Class