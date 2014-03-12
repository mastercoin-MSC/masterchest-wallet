Imports System.Runtime.InteropServices
Imports System.Data.SqlServerCe
Imports Masterchest.mlib
Imports Masterchest
Imports Newtonsoft.Json

Public Class buyfrm

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Close()
    End Sub

    '////////////////////////
    '///HANDLE FORM FUNCTIONS
    '////////////////////////
    <DllImportAttribute("user32.dll")> Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function
    <DllImportAttribute("user32.dll")> Public Shared Function ReleaseCapture() As Boolean
    End Function
    Const WM_NCLBUTTONDOWN As Integer = &HA1
    Const HT_CAPTION As Integer = &H2

    Private Sub buyfrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub
    Public Sub buyfrminit()
        buyfrm_lsendavail.Text = "Select a buying address"
        txtsendamount.Text = "0.00"
        ltotalbtc.Text = "0.00"
        lunit.Text = "0.00"
        combuyaddress.Items.Clear()
        combuyaddress.Text = ""
        For Each row In addresslist.Rows
            If Not combuyaddress.Items.Contains(row.item(0).ToString) Then
                If row.item(1) > 0 Then combuyaddress.Items.Add(row.item(0).ToString)
            End If
        Next
        Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        Dim cmd2 As New SqlCeCommand()
        cmd2.Connection = con
        con.Open()
        'open orders
        cmd2.CommandText = "select saleamount from exchange where fromadd='" & sellrefadd & "'"
        Dim saleamount As Double = cmd2.ExecuteScalar
        cmd2.CommandText = "select offeramount from exchange where fromadd='" & sellrefadd & "'"
        Dim offeramount As Double = cmd2.ExecuteScalar
        cmd2.CommandText = "select unitprice from exchange where fromadd='" & sellrefadd & "'"
        Dim unitprice As Double = cmd2.ExecuteScalar
        cmd2.CommandText = "select timelimit from exchange where fromadd='" & sellrefadd & "'"
        Dim timelimit As Integer = cmd2.ExecuteScalar
        cmd2.CommandText = "select minfee from exchange where fromadd='" & sellrefadd & "'"
        Dim minfee As Double = cmd2.ExecuteScalar
        unitprice = Math.Round(unitprice / 100000000, 8)
        offeramount = Math.Round(offeramount / 100000000, 8)
        saleamount = Math.Round(saleamount / 100000000, 8)
        minfee = Math.Round(minfee / 100000000, 8)
        lnkavail.Text = saleamount.ToString("######0.00######")
        ltotal.Text = offeramount.ToString("######0.00######")
        lunit.Text = unitprice.ToString("######0.00######")
        ltimelimitn.Text = timelimit.ToString & " blocks"
        lminfee.Text = minfee.ToString("######0.00######")
        con.Close()
    End Sub
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown, RectangleShape1.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If

    End Sub
    Private Sub updateavail()
        Dim denom As String = ""
        avail = 0
        For Each row In addresslist.Rows
            If row.item(0) = combuyaddress.SelectedItem Then avail = row.item(1)
        Next
        buyfrm_lsendavail.Text = "Available: " & avail.ToString("######0.00######") & " BTC"
    End Sub

    Private Sub txtsendamount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtsendamount.LostFocus
        Try
            txtsendamount.Text = Math.Round(Val(txtsendamount.Text), 8).ToString
        Catch ex As Exception
            txtsendamount.Text = "0.00"
        End Try
    End Sub

    Private Sub txtsendamount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtsendamount.TextChanged

        Dim totalbtc As Double = Val(txtsendamount.Text) * Val(lunit.Text)
        ltotalbtc.Text = totalbtc.ToString("######0.00######")
    End Sub

    Private Sub combuyaddress_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles combuyaddress.SelectedIndexChanged
        updateavail()
    End Sub

    Private Sub bbuy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bbuy.Click
        txsummary = ""
        senttxid = "Transaction not sent"
        'validate amounts
        Try
            If Val(txtsendamount.Text) > Val(lnkavail.Text) Then
                MsgBox("There are not that many available to buy.")
                Exit Sub
            End If
            If Val(txtsendamount.Text) = 0 Or Val(ltotalbtc.Text) = 0 Then
                Exit Sub
            End If
            If Val(ltotalbtc.Text) > avail Then
                MsgBox("The selected buy address does not have enough BTC to fund this buy.")
                Exit Sub
            End If
            If combuyaddress.Text.Length < 26 Or combuyaddress.Text.Length > 35 Then Exit Sub
        Catch ex As Exception
            'validation fail
            Exit Sub
        End Try
        Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Beginning accept (buy) transaction")
        Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")

        Dim mlib As New Masterchest.mlib

        'first validate recipient address
        If combuyaddress.Text <> "" Then
            'get wallet passphrase
            Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Requesting passphrase")
            passfrm.ShowDialog()
            If btcpass = "" Then Exit Sub 'cancelled
            Dim fromadd As String
            fromadd = combuyaddress.SelectedItem.ToString

            Dim curtype As Integer = 2
            Dim amount As Double = Val(txtsendamount.Text)
            Dim amountlong As Long = amount * 100000000

            Try
                Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Validating recipient address")
                Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, sellrefadd, 0, 0))
                If validater.result.isvalid = True Then 'address is valid
                    'push out to masterchest lib to encode the tx
                    Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Calling library: mlib.encodeaccepttx, bitcoin_con, " & fromadd & ", " & sellrefadd & ", " & curtype.ToString & ", " & amountlong.ToString)
                    Dim rawtx As String = mlib.encodeaccepttx(bitcoin_con, fromadd, sellrefadd, curtype, amountlong)
                    'is rawtx empty
                    If rawtx = "" Then
                        Form1.txtdebug.AppendText(vbCrLf & "ERROR: Raw transaction is empty - stopping")
                        Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                        Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending accept (buy) transaction")
                        Exit Sub
                    End If
                    'decode the tx in the viewer
                    txsummary = txsummary & vbCrLf & "Raw transaction hex:" & vbCrLf & rawtx & vbCrLf & "Raw transaction decode:" & vbCrLf & mlib.rpccall(bitcoin_con, "decoderawtransaction", 1, rawtx, 0, 0)
                    Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Raw transaction hex: " & vbCrLf & rawtx)
                    'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
                    Form1.txtdebug.AppendText(vbCrLf & "DEUBG: Unlocking wallet")
                    Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
                    Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(btcpass.ToString), 15, 0)
                    btcpass = ""
                    'try and sign transaction
                    Form1.txtdebug.AppendText(vbCrLf & "DEUBG: Attempting signing")
                    Dim signedtxn As signedtx = JsonConvert.DeserializeObject(Of signedtx)(mlib.rpccall(bitcoin_con, "signrawtransaction", 1, rawtx, 0, 0))
                    If signedtxn.result.complete = True Then
                        txsummary = txsummary & vbCrLf & "Signing appears successful."
                        Form1.txtdebug.AppendText(vbCrLf & "DEUBG: Attempting broadcast")
                        Dim broadcasttx As broadcasttx = JsonConvert.DeserializeObject(Of broadcasttx)(mlib.rpccall(bitcoin_con, "sendrawtransaction", 1, signedtxn.result.hex, 0, 0))
                        If broadcasttx.result <> "" Then
                            txsummary = txsummary & vbCrLf & "Transaction sent, ID: " & broadcasttx.result.ToString
                            Form1.txtdebug.AppendText(vbCrLf & "DEUBG: Transaction sent - " & broadcasttx.result.ToString)
                            Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                            Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending accept (buy) transaction")
                            senttxid = broadcasttx.result.ToString
                            Application.DoEvents()
                            If Form1.workthread.IsBusy <> True Then
                                Form1.UIrefresh.Enabled = False
                                Form1.syncicon.Visible = True
                                Form1.lsyncing.Visible = True
                                Form1.poversync.Image = My.Resources.sync
                                Form1.loversync.Text = "Synchronizing..."
                                Form1.lsyncing.Text = "Synchronizing..."
                                ' Start the workthread for the blockchain scanner
                                Form1.workthread.RunWorkerAsync()
                            End If
                            Application.DoEvents()
                            sentfrm.ShowDialog()
                            Form1.dgvselloffer.ClearSelection()
                            Me.Close()
                            Exit Sub
                        Else
                            txsummary = txsummary & vbCrLf & "Error sending transaction."
                            sentfrm.lsent.Text = "transaction failed"
                            Form1.txtdebug.AppendText(vbCrLf & "ERROR: Unknown error sending transaction")
                            Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                            Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending accept (buy) transaction")
                            sentfrm.ShowDialog()
                            Exit Sub
                        End If
                    Else
                        txsummary = txsummary & vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct."
                        Form1.txtdebug.AppendText(vbCrLf & "ERROR: Failed to sign transaction.  Ensure wallet passphrase is correct")
                        Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                        Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending accept (buy) transaction")
                        sentfrm.lsent.Text = "transaction failed"
                        sentfrm.ShowDialog()
                        Exit Sub
                    End If
                Else
                    txsummary = "Build transaction failed.  Recipient address is not valid."
                    Form1.txtdebug.AppendText(vbCrLf & "ERROR: Build transaction failed.  Recipient address is not valid")
                    Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                    Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending accept (buy) transaction")
                    sentfrm.lsent.Text = "transaction failed"
                    sentfrm.ShowDialog()
                    Exit Sub
                End If
            Catch ex As Exception
                MsgBox("Exeption thrown : " & ex.Message)
                Form1.txtdebug.AppendText(vbCrLf & "ERROR: Exception thrown: " & ex.Message)
                Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending accept (buy) transaction")
                sentfrm.ShowDialog()
            End Try
        End If
    End Sub

    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        Me.Close()

    End Sub

    Private Sub lnkavail_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkavail.LinkClicked
        txtsendamount.Text = lnkavail.Text
    End Sub
End Class