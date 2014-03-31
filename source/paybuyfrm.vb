Imports System.Runtime.InteropServices
Imports System.Data.SqlServerCe
Imports Masterchest.mlib
Imports Masterchest
Imports Newtonsoft.Json
Public Class paybuyfrm
    Public buybtcamount As Long

    Private Sub paybuyfrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub

    Public Sub paybuyfrminit()
        Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        Dim cmd2 As New SqlCeCommand()
        cmd2.Connection = con
        con.Open()
        'get matching sell
        cmd2.CommandText = "select matchingtx from transactions_processed where txid='" & paybuytxid & "'"
        Dim matchingtx As String = cmd2.ExecuteScalar
        cmd2.CommandText = "select blocknum from transactions_processed where txid='" & paybuytxid & "'"
        Dim acceptblock As Integer = cmd2.ExecuteScalar
        cmd2.CommandText = "select fromadd from transactions_processed where txid='" & paybuytxid & "'"
        Dim buyer As String = cmd2.ExecuteScalar
        cmd2.CommandText = "select purchaseamount from transactions_processed where txid='" & paybuytxid & "'"
        Dim purchaseamount As Long = cmd2.ExecuteScalar
        cmd2.CommandText = "select toadd from transactions_processed where txid='" & paybuytxid & "'"
        Dim seller As String = cmd2.ExecuteScalar
        cmd2.CommandText = "select curtype from transactions_processed where txid='" & matchingtx & "'"
        Dim curtype As Integer = cmd2.ExecuteScalar
        cmd2.CommandText = "select saleamount from transactions_processed where txid='" & matchingtx & "'"
        Dim saleamount As Long = cmd2.ExecuteScalar
        cmd2.CommandText = "select offeramount from transactions_processed where txid='" & matchingtx & "'"
        Dim offeramount As Long = cmd2.ExecuteScalar
        Dim unitprice As Double = offeramount / saleamount
        cmd2.CommandText = "select timelimit from transactions_processed where txid='" & matchingtx & "'"
        Dim timelimit As Integer = cmd2.ExecuteScalar
        cmd2.CommandText = "select max(blocknum) from processedblocks"
        Dim curblock As Integer = cmd2.ExecuteScalar
        Dim timeneeded As Integer = (acceptblock + timelimit) - 1
        Dim timeleft As Integer = timeneeded - curblock
        Dim cur As Double = purchaseamount / 100000000
        Dim total As Double = Math.Round((unitprice * cur), 8)
        buybtcamount = 0
        buybtcamount = total * 100000000
        'populate details
        lselladdress.Text = seller
        lbuyaddress.Text = buyer
        lunitprice.Text = unitprice.ToString("######0.00######")
        If curtype = 2 Then lcurtype.Text = "Test Mastercoin"
        If curtype = 1 Then lcurtype.Text = "Mastercoin"
        lbtc.Text = total.ToString("######0.00######") & " BTC"
        lcur.Text = cur.ToString("######0.00######") & " " & dexcur
        If timeleft > 1 Then ltimeleft.Text = timeleft.ToString & " blocks"
        If timeleft = 1 Then ltimeleft.Text = "1 block (HIGH RISK!)"
        If timeleft < 1 Then ltimeleft.Text = "Expired"
    End Sub

    Private Sub Form1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RectangleShape1.MouseDown

    End Sub

    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        Me.Close()
    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Close()
    End Sub

    Private Sub bsendpay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsendpay.Click
        Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Beginning send payment transaction")
        Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
        txsummary = ""
        senttxid = "Transaction not sent"
        'validate amounts
        Try
            If buybtcamount = 0 Then
                MsgBox("ERROR: BTC amount is zero.")
                Form1.txtdebug.AppendText(vbCrLf & "ERROR: BTC amount is zero")
                Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                Exit Sub
            End If
            If lselladdress.Text.Length < 27 Or lselladdress.Text.Length > 35 Or lbuyaddress.Text.Length < 27 Or lbuyaddress.Text.Length > 35 Then
                MsgBox("ERROR: Sanity check failed on addresses.")
                Form1.txtdebug.AppendText(vbCrLf & "ERROR: Sanity checks failed on addresses")
                Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                Exit Sub
            End If
        Catch ex As Exception
            'validation fail
            Exit Sub
        End Try

        Dim mlib As New Masterchest.mlib
        'first validate recipient address
        If lselladdress.Text <> "" Then
            'get wallet passphrase
            Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Requesting passphrase")
            passfrm.ShowDialog()
            Dim fromadd As String
            Dim selladd As String = lselladdress.Text
            fromadd = lbuyaddress.Text
            Dim paymentamountlong As Long = buybtcamount

            Try
                Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Validating recipient address")
                Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, selladd, 0, 0))
                If validater.result.isvalid = True Then 'address is valid
                    Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Calling library: mlib.encodepaytx, bitcoin_con, " & fromadd & ", " & selladd & ", " & paymentamountlong.ToString)
                    'push out to masterchest lib to encode the tx
                    Dim rawtx As String = mlib.encodepaytx(bitcoin_con, fromadd, selladd, paymentamountlong)
                    'is rawtx empty
                    If rawtx = "" Then
                        Form1.txtdebug.AppendText(vbCrLf & "ERROR: Raw transaction is empty - stopping")
                        Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                        Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                        Exit Sub
                    End If
                    'decode the tx in the viewer
                    Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Raw transaction hex: " & vbCrLf & rawtx)
                    txsummary = txsummary & vbCrLf & "Raw transaction hex:" & vbCrLf & rawtx & vbCrLf & "Raw transaction decode:" & vbCrLf & mlib.rpccall(bitcoin_con, "decoderawtransaction", 1, rawtx, 0, 0)
                    'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
                    Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Unlocking wallet")
                    If btcpass = "" Then 'skip unlocking wallet
                        Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: No passphrase specified, skipping unlocking wallet")
                    Else
                        Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
                        Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(btcpass.ToString), 15, 0)
                    End If
                    btcpass = ""
                    'try and sign transaction
                    Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Attempting signing")
                    Dim signedtxn As signedtx = JsonConvert.DeserializeObject(Of signedtx)(mlib.rpccall(bitcoin_con, "signrawtransaction", 1, rawtx, 0, 0))
                    If signedtxn.result.complete = True Then
                        txsummary = txsummary & vbCrLf & "Signing appears successful."
                        Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Attempting broadcast")
                        Dim broadcasttx As broadcasttx = JsonConvert.DeserializeObject(Of broadcasttx)(mlib.rpccall(bitcoin_con, "sendrawtransaction", 1, signedtxn.result.hex, 0, 0))
                        If broadcasttx.result <> "" Then
                            Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Transaction sent - " & broadcasttx.result.ToString)
                            Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                            Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                            txsummary = txsummary & vbCrLf & "Transaction sent, ID: " & broadcasttx.result.ToString
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
                            Form1.dgvopenorders.ClearSelection()
                            Me.Close()
                            Exit Sub
                        Else
                            txsummary = txsummary & vbCrLf & "Error sending transaction"
                            sentfrm.lsent.Text = "transaction failed"
                            Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] ERROR: Unknown error sending transaction")
                            Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                            Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                            sentfrm.ShowDialog()
                            Exit Sub
                        End If
                    Else
                        txsummary = txsummary & vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct"
                        sentfrm.lsent.Text = "transaction failed"
                        Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] ERROR: Failed to sign transaction.  Ensure wallet passphrase is correct")
                        Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                        Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                        sentfrm.ShowDialog()
                        Exit Sub
                    End If
                Else
                    txsummary = "Build transaction failed.  Recipient address is not valid"
                    sentfrm.lsent.Text = "transaction failed"
                    Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] ERROR: Build transaction failed.  Recipient address is not valid")
                    Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                    Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                    sentfrm.ShowDialog()
                    Exit Sub
                End If
            Catch ex As Exception
                MsgBox("Exeption thrown : " & ex.Message)
                Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] ERROR: Exception thrown: " & ex.Message)
                Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                Form1.txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                sentfrm.ShowDialog()
            End Try
        End If
    End Sub
End Class