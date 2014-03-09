Imports System.Runtime.InteropServices
Imports System.Data.SqlServerCe
Imports Masterchest.mlib
Imports Masterchest
Imports Newtonsoft.Json

Public Class sellfrm

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Close()
        lnkminfee.Text = "0.0001 BTC"
        lnktimelimit.Text = "6 blocks"
        txtsendamount.Text = "0.00"
        txtunit.Text = "0.00"
        ltotalbtc.Text = "0.00"
        comselladdress.Text = ""
        comselladdress.SelectedItem = Nothing
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

    Public Sub sellfrminit()
        lsendavail.Text = "Select a selling address"
        txtsendamount.Text = "0.00"
        txtunit.Text = "0.00"
        ltotalbtc.Text = "0.00"
        lnktimelimit.Text = "6 blocks"
        lnkminfee.Text = "0.0001 BTC"
        comselladdress.Items.Clear()
        comselladdress.Text = ""
        For Each row In addresslist.Rows
            If Not comselladdress.Items.Contains(row.item(0).ToString) Then
                If row.item(2) > 0 Then comselladdress.Items.Add(row.item(0).ToString)
            End If
        Next
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
            If row.item(0) = comselladdress.SelectedItem Then avail = row.item(2)
        Next
        lsendavail.Text = "Available: " & avail.ToString("######0.00######") & " TMSC"
    End Sub

    Private Sub txtsendamount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtsendamount.TextChanged
        Dim tot As Double = Val(txtunit.Text) * Val(txtsendamount.Text)
        ltotalbtc.Text = tot.ToString("######0.00######")
    End Sub

    Private Sub combuyaddress_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comselladdress.SelectedIndexChanged
        updateavail()
    End Sub

    Private Sub bsell_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsell.Click
        txsummary = ""
        senttxid = "Transaction not sent"
        'validate amounts
        If Val(txtsendamount.Text) > avail Or Not Val(txtsendamount.Text) > 0 Or Not Val(ltotalbtc.Text) > 0 Or Not Val(txtunit.Text) > 0 Or String.IsNullOrEmpty(comselladdress.Text) Then
            Exit Sub
        End If
        'sanity check that not already an existing sell
        Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        Dim cmd As New SqlCeCommand()
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "SELECT COUNT(fromadd) FROM exchange_temp where FROMADD='" & comselladdress.SelectedItem.ToString & "'"
        Dim returnval = cmd.ExecuteScalar
        If returnval > 0 Then
            MsgBox("ERROR: " & vbCrLf & vbCrLf & "Sell offer already exists at this address.")
            Exit Sub
        End If
        con.Close()
        Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Beginning sell transaction")
        Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")

        Dim mlib As New Masterchest.mlib
        'get wallet passphrase
        Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Requesting passphrase")
        passfrm.ShowDialog()
        If btcpass = "" Then Exit Sub 'cancelled
        Dim fromadd As String
        fromadd = comselladdress.SelectedItem.ToString
        Dim curtype As Integer = 2
        Dim amount As Double = Val(txtsendamount.Text)
        Dim amountlong As Long = amount * 100000000
        Dim offer As Double = Val(ltotalbtc.Text)
        Dim offerlong As Long = offer * 100000000
        Dim timelimit As Integer = 6
        Dim minfee As Long = 10000
        Dim action As Integer = 1 'new sell
        Try
            'choose blocktime and timelimit
            If lnkminfee.Text = "0.0005 BTC" Then minfee = 50000
            If lnkminfee.Text = "0.0003 BTC" Then minfee = 30000
            If lnkminfee.Text = "0.00025 BTC" Then minfee = 25000
            If lnkminfee.Text = "0.0002 BTC" Then minfee = 20000
            If lnkminfee.Text = "0.00015 BTC" Then minfee = 15000
            If lnkminfee.Text = "0.00014 BTC" Then minfee = 14000
            If lnkminfee.Text = "0.00013 BTC" Then minfee = 13000
            If lnkminfee.Text = "0.00012 BTC" Then minfee = 12000
            If lnkminfee.Text = "0.00011 BTC" Then minfee = 11000
            If lnkminfee.Text = "0.0001 BTC" Then minfee = 10000
            If lnktimelimit.Text = "250 blocks" Then timelimit = 250
            If lnktimelimit.Text = "200 blocks" Then timelimit = 200
            If lnktimelimit.Text = "150 blocks" Then timelimit = 150
            If lnktimelimit.Text = "100 blocks" Then timelimit = 100
            If lnktimelimit.Text = "75 blocks" Then timelimit = 75
            If lnktimelimit.Text = "50 blocks" Then timelimit = 50
            If lnktimelimit.Text = "25 blocks" Then timelimit = 25
            If lnktimelimit.Text = "20 blocks" Then timelimit = 20
            If lnktimelimit.Text = "15 blocks" Then timelimit = 15
            If lnktimelimit.Text = "10 blocks" Then timelimit = 10
            If lnktimelimit.Text = "6 blocks" Then timelimit = 6
            If lnktimelimit.Text = "5 blocks" Then timelimit = 5
            If lnktimelimit.Text = "4 blocks" Then timelimit = 4
            If lnktimelimit.Text = "3 blocks" Then timelimit = 3
            If lnktimelimit.Text = "2 blocks" Then timelimit = 2
            If lnktimelimit.Text = "1 block" Then timelimit = 1
            'push out to masterchest lib to encode the tx
            Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Calling library: mlib.encodeselltx, bitcoin_con, " & fromadd & ", " & curtype.ToString & ", " & amountlong.ToString & ", " & offerlong.ToString & ", " & minfee.ToString & ", " & timelimit.ToString & ", " & timelimit.ToString & ", " & action.ToString)
            Dim rawtx As String = mlib.encodeselltx(bitcoin_con, fromadd, curtype, amountlong, offerlong, minfee, timelimit, action)
            'is rawtx empty
            If rawtx = "" Then
                MsgBox("Sanity Check Failed" & vbCrLf & vbCrLf & "Raw transaction is empty - stopping.")
                Form1.txtdebug.AppendText(vbCrLf & "ERROR: Raw transaction is empty - stopping")
                Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending sell transaction")
                Exit Sub
            End If
            'decode the tx in the viewer
            Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Raw transaction hex: " & vbCrLf & rawtx)
            txsummary = txsummary & vbCrLf & "Raw transaction hex:" & vbCrLf & rawtx & vbCrLf & "Raw transaction decode:" & vbCrLf & mlib.rpccall(bitcoin_con, "decoderawtransaction", 1, rawtx, 0, 0)
            'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
            Form1.txtdebug.AppendText(vbCrLf & "DEUBG: Unlocking wallet")
            Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
            Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(btcpass.ToString), 15, 0)
            btcpass = ""
            'try and sign transaction
            Try
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
                        Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending sell transaction")
                        senttxid = broadcasttx.result.ToString
                        sentfrm.lsent.Text = "transaction sent"
                        Application.DoEvents()
                        If Form1.workthread.IsBusy <> True Then
                            Form1.UIrefresh.Enabled = False
                            Form1.lsyncing.Visible = True
                            Form1.lsyncing.Text = "Synchronizing..."
                            Form1.syncicon.Image = My.Resources.sync
                            Form1.syncicon.Visible = True
                            Form1.poversync.Image = My.Resources.sync
                            Form1.loversync.Text = "Synchronizing..."
                            'Start the workthread for the blockchain scanner
                            Form1.workthread.RunWorkerAsync()
                        End If
                        Application.DoEvents()
                        sentfrm.ShowDialog()
                        Me.Close()
                        Exit Sub
                    Else
                        sentfrm.lsent.Text = "transaction failed"
                        txsummary = txsummary & vbCrLf & "Error sending transaction."
                        Form1.txtdebug.AppendText(vbCrLf & "ERROR: Unknown error sending transaction")
                        Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                        Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending sell transaction")
                        sentfrm.ShowDialog()
                        Exit Sub
                    End If
                Else
                    txsummary = txsummary & vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct."
                    sentfrm.lsent.Text = "transaction failed"
                    Form1.txtdebug.AppendText(vbCrLf & "ERROR: Failed to sign transaction.  Ensure wallet passphrase is correct")
                    Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                    Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending sell transaction")
                    sentfrm.ShowDialog()
                    Exit Sub
                End If
            Catch ex As Exception
                txsummary = txsummary & vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct.  " & ex.Message
                Form1.txtdebug.AppendText(vbCrLf & "ERROR: Build transaction failed.  Recipient address is not valid")
                Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
                Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending sell transaction")
                sentfrm.lsent.Text = "transaction failed"
                sentfrm.ShowDialog()
                Exit Sub
            End Try
            sentfrm.ShowDialog()
        Catch ex As Exception
            MsgBox("Exeption thrown : " & ex.Message)
            Form1.txtdebug.AppendText(vbCrLf & "ERROR: Exception thrown: " & ex.Message)
            Form1.txtdebug.AppendText(vbCrLf & "===================================================================================")
            Form1.txtdebug.AppendText(vbCrLf & "DEBUG: Ending sell transaction")
            sentfrm.ShowDialog()
        End Try


    
    End Sub

    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        Me.Close()
        lnkminfee.Text = "0.0001 BTC"
        lnktimelimit.Text = "6 blocks"
        txtsendamount.Text = "0.00"
        txtunit.Text = "0.00"
        ltotalbtc.Text = "0.00"
        comselladdress.Text = ""
        comselladdress.SelectedItem = Nothing
    End Sub

    Private Sub txtunit_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtunit.TextChanged
        Dim tot As Double = Val(txtunit.Text) * Val(txtsendamount.Text)
        ltotalbtc.Text = tot.ToString("######0.00######")
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

  
    Private Sub lnktimelimit_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnktimelimit.LinkClicked
        If lnktimelimit.Text = "250 blocks" Then
            lnktimelimit.Text = "1 block"
            Exit Sub
        End If
        If lnktimelimit.Text = "200 blocks" Then lnktimelimit.Text = "250 blocks"
        If lnktimelimit.Text = "150 blocks" Then lnktimelimit.Text = "200 blocks"
        If lnktimelimit.Text = "100 blocks" Then lnktimelimit.Text = "150 blocks"
        If lnktimelimit.Text = "75 blocks" Then lnktimelimit.Text = "100 blocks"
        If lnktimelimit.Text = "50 blocks" Then lnktimelimit.Text = "75 blocks"
        If lnktimelimit.Text = "25 blocks" Then lnktimelimit.Text = "50 blocks"
        If lnktimelimit.Text = "20 blocks" Then lnktimelimit.Text = "25 blocks"
        If lnktimelimit.Text = "15 blocks" Then lnktimelimit.Text = "20 blocks"
        If lnktimelimit.Text = "10 blocks" Then lnktimelimit.Text = "15 blocks"
        If lnktimelimit.Text = "6 blocks" Then lnktimelimit.Text = "10 blocks"
        If lnktimelimit.Text = "5 blocks" Then lnktimelimit.Text = "6 blocks"
        If lnktimelimit.Text = "4 blocks" Then lnktimelimit.Text = "5 blocks"
        If lnktimelimit.Text = "3 blocks" Then lnktimelimit.Text = "4 blocks"
        If lnktimelimit.Text = "2 blocks" Then lnktimelimit.Text = "3 blocks"
        If lnktimelimit.Text = "1 block" Then lnktimelimit.Text = "2 blocks"
    End Sub

    Private Sub lnkminfee_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkminfee.LinkClicked
        If lnkminfee.Text = "0.0005 BTC" Then
            lnkminfee.Text = "0.0001 BTC"
            Exit Sub
        End If
        If lnkminfee.Text = "0.0003 BTC" Then lnkminfee.Text = "0.0005 BTC"
        If lnkminfee.Text = "0.00025 BTC" Then lnkminfee.Text = "0.0003 BTC"
        If lnkminfee.Text = "0.0002 BTC" Then lnkminfee.Text = "0.00025 BTC"
        If lnkminfee.Text = "0.00015 BTC" Then lnkminfee.Text = "0.0002 BTC"
        If lnkminfee.Text = "0.00014 BTC" Then lnkminfee.Text = "0.00015 BTC"
        If lnkminfee.Text = "0.00013 BTC" Then lnkminfee.Text = "0.00014 BTC"
        If lnkminfee.Text = "0.00012 BTC" Then lnkminfee.Text = "0.00013 BTC"
        If lnkminfee.Text = "0.00011 BTC" Then lnkminfee.Text = "0.00012 BTC"
        If lnkminfee.Text = "0.0001 BTC" Then lnkminfee.Text = "0.00011 BTC"
    End Sub
End Class