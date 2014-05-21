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
Imports System.Diagnostics
Imports System.Data.SqlServerCe
Imports System.Drawing.Drawing2D
Imports System.Configuration
Imports System.Security.Cryptography
Imports Masterchest.mlib
Imports Org.BouncyCastle.Math.EC
Imports System.Globalization

Public Class Form1


    Public startup As Boolean = True
    Public asyncjump As Boolean = True
    Const WM_NCLBUTTONDOWN As Integer = &HA1
    Const HT_CAPTION As Integer = &H2
    Public mlib As New Masterchest.mlib

    '////////////////////////
    '///HANDLE FORM FUNCTIONS
    '////////////////////////
    <DllImportAttribute("user32.dll")> Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function
    <DllImportAttribute("user32.dll")> Public Shared Function ReleaseCapture() As Boolean
    End Function

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown, RectangleShape1.MouseDown, psetup.MouseDown, pwelcome.MouseDown, pbalances.MouseDown, paddresses.MouseDown, pdebug.MouseDown, poverview.MouseDown, psend.MouseDown, psettings.MouseDown, pproperties.MouseDown
        If e.Button = MouseButtons.Left Then
            tryunfocusinputs()
            dgvaddresses.CurrentCell = Nothing
            dgvaddresses.ClearSelection()
            dgvhistory.CurrentCell = Nothing
            dgvhistory.ClearSelection()
            dgvselloffer.CurrentCell = Nothing
            dgvselloffer.ClearSelection()
            dgvopenorders.CurrentCell = Nothing
            dgvopenorders.ClearSelection()
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If

    End Sub
    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
         Me.WindowState = FormWindowState.Minimized
        Me.Visible = False
        nfi.Visible = True
        nfi.BalloonTipTitle = "Masterchest Wallet Background Mode"
        nfi.BalloonTipText = "To return to the wallet, simply double-click the icon in your system tray.  If you wish to completely exit the wallet, please right click on the system tray icon and choose 'Exit'."
        nfi.BalloonTipIcon = ToolTipIcon.Info
        nfi.ShowBalloonTip(30000)
    End Sub
    Private Sub bmin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bmin.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub mnurestore_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        nfi.Visible = False
        Me.Visible = True
        Me.WindowState = FormWindowState.Normal
    End Sub
    Private Sub mnuexit_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        nfi.Visible = False
        Application.Exit()
    End Sub

    '//////////////
    '////INITIALIZE
    '//////////////
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'context icon
        Dim icnmnu As New ContextMenuStrip
        Dim mnurestore As New ToolStripMenuItem("&Restore")
        AddHandler mnurestore.Click, AddressOf mnurestore_click
        icnmnu.Items.AddRange(New ToolStripItem() {mnurestore})
        Dim mnuexit As New ToolStripMenuItem("E&xit")
        AddHandler mnuexit.Click, AddressOf mnuexit_click
        icnmnu.Items.AddRange(New ToolStripItem() {mnuexit})
        nfi.ContextMenuStrip = icnmnu

        'declare globalization to make sure we use a . for decimal only
        Dim customCulture As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture.Clone()
        customCulture.NumberFormat.NumberDecimalSeparator = "."
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture
        System.Threading.Thread.CurrentThread.CurrentUICulture = customCulture

        'declare current currency for exchange
        dexcur = "MSC"
        lnkdexcurrency.Text = "Mastercoin"

        'disclaimer
        MsgBox("DISCLAIMER - PLEASE READ - TEST ONLY: " & vbCrLf & "============================" & vbCrLf & vbCrLf & "This software is EXPERIMENTAL software for TESTING only." & vbCrLf & vbCrLf & "ALL USE OF THIS SOFTWARE IS ENTIRELY AT YOUR OWN RISK." & vbCrLf & vbCrLf & "The protocol and transaction processing rules for Mastercoin are still under active development and are subject to change in future." & vbCrLf & vbCrLf & "DO NOT USE IT WITH A LARGE AMOUNT OF MASTERCOINS AND/OR BITCOINS.  IT IS ENTIRELY POSSIBLE YOU MAY LOSE ALL YOUR COINS.  INFORMATION DISPLAYED MAY BE INCORRECT.  MASTERCHEST OFFERS ABSOLUTELY NO GUARANTEES OF ANY KIND." & vbCrLf & vbCrLf & "A fraction of a bitcoin and a fraction of a mastercoin are the suggested testing amounts.  Preferably use a fresh bitcoin wallet.dat." & vbCrLf & vbCrLf & "DO *NOT* USE THIS SOFTWARE WITH WALLETS CONTAINING, OR TRANSACT WITH, SIGNIFICANT AMOUNTS - IT IS FOR TESTING ONLY." & vbCrLf & vbCrLf & "This software is provided open-source at no cost.  You are responsible for knowing the law in your country and determining if your use of this software contravenes any local laws.")
        poversync.Image = My.Resources.gif
        loversync.Text = "Syncronizing..."
        bback.Visible = False
        hidelabels()
        initialize()
        'are we configured?
        'setup bitcoin connection
        txtrpcserver.Text = "Not configured."
        txtrpcport.Text = "Not configured."
        txtrpcuser.Text = "Not configured."
        txtrpcpassword.Text = "Not configured."
        Try
            Dim btcconf As String = GetFolderPath(SpecialFolder.ApplicationData)
            btcconf = btcconf & "\Bitcoin\bitcoin.conf"
            If System.IO.File.Exists(btcconf) = True Then
                Dim objreader As New System.IO.StreamReader(btcconf)
                Dim line As String
                'set defaults
                bitcoin_con.bitcoinrpcserver = "127.0.0.1"
                bitcoin_con.bitcoinrpcport = 8332
                Dim txenabled As Boolean = False
                Dim rpcenabled As Boolean = False
                Do
                    line = objreader.ReadLine()
                    If Len(line) > 7 Then
                        Select Case line.ToLower.Substring(0, 7)
                            Case "rpcport"
                                bitcoin_con.bitcoinrpcport = Val(line.Substring(8, Len(line) - 8))
                                txtrpcport.Text = bitcoin_con.bitcoinrpcport.ToString
                            Case "rpcuser"
                                bitcoin_con.bitcoinrpcuser = line.Substring(8, Len(line) - 8)
                                txtrpcuser.Text = bitcoin_con.bitcoinrpcuser
                            Case "rpcpass"
                                bitcoin_con.bitcoinrpcpassword = line.Substring(12, Len(line) - 12)
                                txtrpcpassword.Text = "********************"
                            Case "txindex"
                                If line.ToLower.Substring(0, 9) = "txindex=1" Then txenabled = True
                            Case "server="
                                If line.ToLower.Substring(0, 8) = "server=1" Then rpcenabled = True
                            Case "gettingstarted#"
                                'gettingstardscreen
                                psetup.Visible = True
                                Exit Sub
                        End Select
                    End If
                Loop Until line Is Nothing
                objreader.Close()
                If rpcenabled = False Then
                    MsgBox("BITCOIN AUTO CONFIGURATION" & vbCrLf & vbCrLf & "Auto-detection has determined your bitcoind/qt configuration does not have the RPC server enabled." & vbCrLf & vbCrLf & "Please add server=1 to bitcoin.conf and restart bitcoind/qt." & vbCrLf & vbCrLf & "Will now exit.")
                    Application.Exit()
                End If
                If txenabled = False Then
                    MsgBox("BITCOIN AUTO CONFIGURATION" & vbCrLf & vbCrLf & "Auto-detection has determined your bitcoind/qt configuration does not have the transaction index enabled." & vbCrLf & vbCrLf & "Please add txindex=1 to bitcoin.conf and restart bitcoind/qt with the -reindex flag to enable the transaction index." & vbCrLf & vbCrLf & "Will now exit.")
                    Application.Exit()
                End If
            Else
                'couldn't auto-detect bitcoin settings, looking at manual config
                Try
                    Dim FINAME As String = Application.StartupPath & "\wallet.cfg"
                    If System.IO.File.Exists(FINAME) = True Then
                        Dim objreader As New System.IO.StreamReader(FINAME)
                        Dim line As String
                        Do
                            line = objreader.ReadLine()
                            If Len(line) > 14 Then
                                Select Case line.ToLower.Substring(0, 15)
                                    Case "bitcoinrpcserv="
                                        bitcoin_con.bitcoinrpcserver = line.Substring(15, Len(line) - 15)
                                        txtrpcserver.Text = bitcoin_con.bitcoinrpcserver
                                    Case "bitcoinrpcport="
                                        bitcoin_con.bitcoinrpcport = Val(line.Substring(15, Len(line) - 15))
                                        txtrpcport.Text = bitcoin_con.bitcoinrpcport.ToString
                                    Case "bitcoinrpcuser="
                                        bitcoin_con.bitcoinrpcuser = line.Substring(15, Len(line) - 15)
                                        txtrpcuser.Text = bitcoin_con.bitcoinrpcuser
                                    Case "bitcoinrpcpass="
                                        bitcoin_con.bitcoinrpcpassword = line.Substring(15, Len(line) - 15)
                                        txtrpcpassword.Text = "********************"
                                End Select
                            End If
                        Loop Until line Is Nothing
                        objreader.Close()
                    End If
                Catch ex As Exception
                    MsgBox("Exception reading configuration : " & ex.Message)
                    Application.Exit()
                End Try
            End If

        Catch ex As Exception
            MsgBox("Exception during configuration : " & ex.Message)
            Application.Exit()
        End Try

     
        'show welcome panel
        pwelcome.Visible = True
        'Me.ActiveControl = txtwalpass
        lwelstartup.Text = "Startup: Initializing..."
    End Sub
    Private Sub updateui()
        Me.Refresh()
        Application.DoEvents()
    End Sub

    Private Sub teststartup()
        'build 0021 drastically shrunk most of the startup delays, we no longer need to wait for anything to catch up etc
        'check we have configuration info
        If bitcoin_con.bitcoinrpcserver = "" Or bitcoin_con.bitcoinrpcport = 0 Or bitcoin_con.bitcoinrpcuser = "" Or bitcoin_con.bitcoinrpcpassword = "" Then
            MsgBox("There was a problem configuring your connection to bitcoind/qt.  Both auto detection and manual configuration appear to have failed." & vbCrLf & vbCrLf & "Will now exit.")
            Application.Exit()
        End If

        'test connection to bitcoind
        lwelstartup.Text &= vbCrLf & "Startup: Testing bitcoin connection..."
        Application.DoEvents()
        System.Threading.Thread.Sleep(100)
        Application.DoEvents()

        Try
            Dim checkhash As blockhash = mlib.getblockhash(bitcoin_con, 2)
            If checkhash.result.ToString = "000000006a625f06636b8bb6ac7b960a8d03705d1ace08b1a19da3fdcc99ddbd" Then 'we've got a correct response
                lwelstartup.Text &= vbCrLf & "Startup: Connection to bitcoin via RPC established and sanity check OK."
            Else
                'something has gone wrong
                lwelstartup.Text &= vbCrLf & "Startup ERROR: Connection to bitcoin RPC seems to be established but responses are not as expected.  Aborting startup."
                Exit Sub
            End If
        Catch ex2 As Exception
            lwelstartup.Text &= vbCrLf & "Startup ERROR: Exception testing connection to bitcoin via RPC. Aborting startup. E2: " & ex2.Message
            MsgBox("ERROR: An exception was raised testing your connection to bitcoin via RPC.  Please ensure bitcoind/qt is running.  The wallet will now exit.")
            Application.Exit()
        End Try
        Application.DoEvents()
        System.Threading.Thread.Sleep(100)
        Application.DoEvents()

        'test connection to database
        lwelstartup.Text &= vbCrLf & "Startup: Testing database connection..."
        Application.DoEvents()
        System.Threading.Thread.Sleep(100)
        Application.DoEvents()

        Dim testval As Integer
        testval = SQLGetSingleVal("SELECT count(*) FROM information_schema.columns WHERE table_name = 'processedblocks'")
        If testval = 99 Then Application.Exit() 'something went wrong
        If testval = 3 Then 'sanity check ok
            lwelstartup.Text &= vbCrLf & "Startup: Connection to database established and sanity check OK."
        Else
            'something has gone wrong
            lwelstartup.Text &= vbCrLf & "Startup ERROR: Connection to database seems to be established but responses are not as expected.  Aborting startup."
            Exit Sub
        End If
        Application.DoEvents()
        System.Threading.Thread.Sleep(100)
        Application.DoEvents()


        '### we have confirmed our connections to resources external to the program ###

        'enumarate bitcoin addresses
        lwelstartup.Text &= vbCrLf & "Startup: Enumerating wallet addresses..."
        Application.DoEvents()
        System.Threading.Thread.Sleep(100)
        Application.DoEvents()
        balubtc = 0
        Try
            Dim addresses As List(Of btcaddressbal) = mlib.getaddresses(bitcoin_con)
            taddresslist.Clear()
            For Each address In addresses
                taddresslist.Rows.Add(address.address, address.amount, 0, 0)
                balubtc = balubtc + address.uamount
            Next

            dgvaddresses.DataSource = Nothing
            dgvaddresses.Refresh()
            'load addresslist with taddresslist
            addresslist.Clear()
            For Each row In taddresslist.Rows
                addresslist.Rows.Add(row.item(0), row.item(1), row.item(2), row.item(3))
            Next
            dgvaddresses.DataSource = addresslist
            Dim dgvcolumn As New DataGridViewColumn
            dgvcolumn = dgvaddresses.Columns(0)
            dgvcolumn.Width = 370
            dgvcolumn = dgvaddresses.Columns(1)
            'dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn.Width = 130
            dgvcolumn = dgvaddresses.Columns(2)
            'dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn.Width = 130
            dgvcolumn = dgvaddresses.Columns(3)
            'dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######" '"########0.00######"
            dgvcolumn.Width = 130
            If lnkaddsort.Text = "Address Alpha" Then dgvaddresses.Sort(dgvaddresses.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            If lnkaddsort.Text = "BTC Balance" Then dgvaddresses.Sort(dgvaddresses.Columns(1), System.ComponentModel.ListSortDirection.Descending)
            If lnkaddsort.Text = "MSC Balance" Then dgvaddresses.Sort(dgvaddresses.Columns(3), System.ComponentModel.ListSortDirection.Descending)
            If lnkaddfilter.Text = "No Filter Active" Then addresslist.DefaultView.RowFilter = ""
            If lnkaddfilter.Text = "Empty Balances" Then addresslist.DefaultView.RowFilter = "btcamount > 0 or mscamount > 0 or tmscamount > 0"

        Catch ex As Exception
            MsgBox(ex.Message)
            lwelstartup.Text &= vbCrLf & "Startup ERROR: Enumerating addresses did not complete properly.  Aborting startup."
            Exit Sub
        End Try
        balbtc = 0
        For Each row In taddresslist.Rows
            balbtc = balbtc + row.item(1)
        Next

        startup = False
        lwelstartup.Text &= vbCrLf & "Startup: Initialization Complete."
        Application.DoEvents()
        System.Threading.Thread.Sleep(1000)
        Application.DoEvents()
        'do some initial setup
        showlabels()
        bback.Visible = True
        txtdebug.Text = "MASTERCHEST WALLET v0.4a"
        activateoverview()
        lastscreen = "1"
        updateui()
        Me.Refresh()
        Application.DoEvents()
        'kick off the background worker thread
        If workthread.IsBusy <> True Then
            syncicon.Image = My.Resources.gif
            syncicon.Visible = True
            lsyncing.Visible = True
            lsyncing.Text = "Synchronizing..."
            workthread.RunWorkerAsync()
        End If

    End Sub
    Private Sub txtsendamount_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtsendamount.Enter '## credit dexX7
        If lcurdiv.Text = "Divisible currency" Then
            If txtsendamount.Text = "0.00000000" Then
                txtsendamount.Text = ""
            End If
        Else
            If txtsendamount.Text = "0" Then
                txtsendamount.Text = ""
            End If
        End If
    End Sub

    Private Sub txtsendamount_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtsendamount.Leave '## credit dexX7
        If lcurdiv.Text = "Divisible currency" Then
            If txtsendamount.Text = "" Then
                txtsendamount.Text = "0.00000000"
            Else
                txtsendamount.Text = Val(txtsendamount.Text).ToString("##########0.00000000")
            End If
        Else
            If txtsendamount.Text = "" Then
                txtsendamount.Text = "0"
            Else
                txtsendamount.Text = Math.Truncate(Val(txtsendamount.Text)).ToString("############0")
            End If
        End If
    End Sub


    Private Sub bback_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bback.MouseUp
        Dim lastval
        If Len(lastscreen) > 1 Then
            lastscreen = lastscreen.Substring(0, Len(lastscreen) - 1)
            lastval = lastscreen(lastscreen.Length - 1)
            If lastval = "1" Then activateoverview()
            If lastval = "2" Then activatecurrencies()
            If lastval = "3" Then activatesend()
            If lastval = "4" Then activateaddresses()
            If lastval = "5" Then activatehistory()
            If lastval = "6" Then activateproperties()
            If lastval = "7" Then activatedebug()
        End If
    End Sub


    '////////////
    '/////BUTTONS
    '////////////
    Private Sub boverview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boverview.Click
        If curscreen <> "1" Then
            activateoverview()
            lastscreen = lastscreen & "1"
        End If
    End Sub
    Private Sub bcurrencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bbalances.Click
        If curscreen <> "2" Then
            activatecurrencies()
            lastscreen = lastscreen & "2"
        End If
    End Sub

    Private Sub bsend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsend.Click
        If curscreen <> "3" Then
            activatesend()
            lastscreen = lastscreen & "3"
        End If
    End Sub
    Private Sub baddresses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles baddresses.Click
        If curscreen <> "4" Then
            activateaddresses()
            lastscreen = lastscreen & "4"
        End If
    End Sub
    Private Sub bhistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bhistory.Click
        If curscreen <> "5" Then
            activatehistory()
            lastscreen = lastscreen & "5"
        End If
    End Sub
    Private Sub bproperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bproperties.Click
        If curscreen <> "6" Then
            activateproperties()
            lastscreen = lastscreen & "6"
        End If
    End Sub
    Private Sub bdebug_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdebug.Click
        If curscreen <> "7" Then
            activatedebug()
            lastscreen = lastscreen & "7"
        End If
    End Sub
    Private Sub bexchange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bexchange.Click
        If curscreen <> "8" Then
            activateexchange()
            lastscreen = lastscreen & "8"
        End If
    End Sub
    Private Sub checkdebugscroll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkdebugscroll.CheckedChanged
        If checkdebugscroll.Checked = True Then txtdebug.ScrollBars = ScrollBars.Vertical
        If checkdebugscroll.Checked = False Then txtdebug.ScrollBars = ScrollBars.None
    End Sub

    Private Sub lnkdebug_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkdebug.LinkClicked
        'disabled for now
        Exit Sub

        If debuglevel = 1 Then
            debuglevel = 2
            lnkdebug.Text = "MED"
            Exit Sub
        End If
        If debuglevel = 2 Then
            debuglevel = 3
            lnkdebug.Text = "HIGH"
            Exit Sub
        End If
        If debuglevel = 3 Then
            debuglevel = 1
            lnkdebug.Text = "LOW"
            Exit Sub
        End If
    End Sub

    '//////////////////////////////
    '/// BACKGROUND WORKER
    '//////////////////////////////
    Private Sub workthread_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles workthread.ProgressChanged
        Dim bluepen As New Pen(Color.FromArgb(51, 153, 255), 2)
        Dim greypen As New Pen(Color.FromArgb(65, 65, 65), 2)
        Dim greenpen As New Pen(Color.FromArgb(51, 255, 153), 2)
        Dim fig As Integer
        If InStr(e.UserState.ToString, "%") Then
            fig = Val(e.UserState.ToString.Substring(0, Len(e.UserState.ToString) - 1))
            lsyncing.Text = "Processing (" & fig & "%)..."
            If fig Mod 2 = 0 Then
                Dim blueline As System.Drawing.Graphics = CreateGraphics()
                blueline.DrawLine(bluepen, Convert.ToInt32(556), Convert.ToInt32(32), Convert.ToInt32(fig + 556), Convert.ToInt32(32))
                blueline.DrawLine(greypen, Convert.ToInt32(fig + 556), Convert.ToInt32(32), Convert.ToInt32(656), Convert.ToInt32(32))
            End If
            Application.DoEvents()
        Else
            If InStr(e.UserState.ToString, "#") Then
                fig = Val(e.UserState.ToString.Substring(0, Len(e.UserState.ToString) - 1))
                lsyncing.Text = "Synchronizing (" & fig & "%)..."
                Dim greenline As System.Drawing.Graphics = CreateGraphics()
                greenline.DrawLine(greenpen, Convert.ToInt32(556), Convert.ToInt32(32), Convert.ToInt32(fig + 556), Convert.ToInt32(32))
                greenline.DrawLine(greypen, Convert.ToInt32(fig + 556), Convert.ToInt32(32), Convert.ToInt32(656), Convert.ToInt32(32))
                Application.DoEvents()
            Else
                Me.txtdebug.AppendText(vbCrLf & e.UserState.ToString)
                If InStr(e.UserState.ToString, "Transaction processing") Then
                    loversync.Text = "Processing..."
                    Me.Refresh()
                End If

                If InStr(e.UserState.ToString, "DEBUG: Block Analysis for: ") Then loversync.Text = "Synchronizing...  Current Block: " & e.UserState.ToString.Substring((Len(e.UserState.ToString) - 6), 6) & "..."
            End If
        End If
    End Sub

    Private Sub workthread_DoWork(ByVal senderobj As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles workthread.DoWork
        'declare globalization to make sure we use a . for decimal only
        Dim customCulture As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture.Clone()
        customCulture.NumberFormat.NumberDecimalSeparator = "."
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture
        System.Threading.Thread.CurrentThread.CurrentUICulture = customCulture

        varsyncronized = False
        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: Thread 'workthread' starting...")
        'test connection to bitcoind
        Try
            Dim checkhash As blockhash = mlib.getblockhash(bitcoin_con, 2)
            If checkhash.result.ToString = "000000006a625f06636b8bb6ac7b960a8d03705d1ace08b1a19da3fdcc99ddbd" Then 'we've got a correct response
                workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] STATUS: Connection to bitcoin RPC established & sanity check OK.")
            Else
                'something has gone wrong
                workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] ERROR: Connection to bitcoin RPC seems to be established but responses are not as expected." & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
                Exit Sub
            End If
        Catch
            Exit Sub
        End Try

        'test connection to database
        Dim testval As Integer
        testval = SQLGetSingleVal("SELECT count(*) FROM information_schema.columns WHERE table_name = 'processedblocks'")
        If testval = 99 Then Exit Sub
        If testval = 3 Then 'sanity check ok
            workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] STATUS: Connection to database established & sanity check OK.")
        Else
            'something has gone wrong
            workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] ERROR: Connection to database seems to be established but responses are not as expected." & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
            Exit Sub
        End If
        Application.DoEvents()
        '### we have confirmed our connections to resources external to the program ###

        'check transaction list for last database block and update from there - always delete current last block transactions and go back one ensuring we don't miss transactions if code bombs while processing a block
        Dim dbposition As Integer
        Dim dbposhash, rpcposhash As String
        dbposition = SQLGetSingleVal("SELECT MAX(BLOCKNUM) FROM processedblocks")
        If dbposition > 249497 Then dbposition = dbposition - 1 Else dbposition = 249497 'roll database back a block as long as hash matches, otherwise roll back 50 blocks
        'dbposition = 249497 'force roll back to first MP tx
        'dbposition = 292635 'force roll back to first SP tx
        'dbposition block hash check
        dbposhash = SQLGetSingleVal("SELECT blockhash FROM processedblocks where blocknum=" & dbposition)
        Dim tmpblockhash As blockhash = mlib.getblockhash(bitcoin_con, dbposition)
        rpcposhash = tmpblockhash.result.ToString
        If rpcposhash <> dbposhash Then
            workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] WARNING: Reorg protection initiated, rolling back 50 blocks for safety")
            dbposition = dbposition - 50
        End If

        'delete transactions after dbposition block
        Dim txdeletedcount = SQLGetSingleVal("DELETE FROM transactions WHERE BLOCKNUM > " & dbposition - 1)
        Dim blockdeletedcount = SQLGetSingleVal("DELETE FROM processedblocks WHERE BLOCKNUM > " & dbposition - 1)
        Dim exodeletedcount = SQLGetSingleVal("DELETE FROM exotransactions WHERE BLOCKNUM > " & dbposition - 1)
        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] STATUS: Database starting at block " & dbposition.ToString)
        'System.Threading.Thread.Sleep(10000)
        'check bitcoin RPC for latest block
        Dim rpcblock As Integer
        Dim blockcount As blockcount = mlib.getblockcount(bitcoin_con)
        rpcblock = blockcount.result
        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] STATUS: Network is at block " & rpcblock.ToString)
        'if db block is newer than bitcoin rpc (eg new bitcoin install with preseed db)
        If rpcblock < dbposition Then
            workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] ERROR: Database block appears newer than bitcoinrpc blocks - is bitcoinrpc up to date? Exiting thread.")
            Exit Sub
        End If

        'calculate catchup
        Dim catchup As Integer
        catchup = rpcblock - dbposition
        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] STATUS: " & (catchup + 1).ToString & " blocks to catch up")

        '### loop through blocks since dbposition and add any transactions detected as mastercoin to the transactions table
        Dim msctranscount As Integer
        msctranscount = 0
        Dim msctrans(100000) As String
        For x = dbposition To rpcblock
            Dim blocknum As Integer = x
            If debuglevel > 0 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: Block Analysis for: " & blocknum.ToString)
            Dim perccomp As Integer = ((x - dbposition) / (catchup + 1)) * 100
            workthread.ReportProgress(0, perccomp.ToString & "#")
            Dim blockhash As blockhash = mlib.getblockhash(bitcoin_con, blocknum)
            Dim block As Block = mlib.getblock(bitcoin_con, blockhash.result.ToString)
            Dim txarray() As String = block.result.tx.ToArray

            For j = 1 To UBound(txarray) 'skip tx0 which should be coinbase
                Try
                    Dim workingtxtype As String = mlib.ismastercointx(bitcoin_con, txarray(j))
                    'simple send
                    If workingtxtype = "simple" Then
                        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found MSC transaction (simple send): " & txarray(j))
                        Dim results As txn = mlib.gettransaction(bitcoin_con, txarray(j))
                        'handle generate
                        If results.result.blocktime < 1377993875 Then 'before exodus cutofff
                            Dim mastercointxinfo As mastercointx = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "generate")
                            If mastercointxinfo.type = "generate" And mastercointxinfo.curtype = 0 Then
                                Dim dbwritemsc As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION) VALUES ('" & mastercointxinfo.txid & "','" & mastercointxinfo.fromadd & "','" & mastercointxinfo.toadd & "'," & mastercointxinfo.value & ",'" & mastercointxinfo.type & "'," & mastercointxinfo.blocktime & "," & blocknum & "," & mastercointxinfo.valid & ",1,0)")
                                Dim dbwritetmsc As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION) VALUES ('" & mastercointxinfo.txid & "','" & mastercointxinfo.fromadd & "','" & mastercointxinfo.toadd & "'," & mastercointxinfo.value & ",'" & mastercointxinfo.type & "'," & mastercointxinfo.blocktime & "," & blocknum & "," & mastercointxinfo.valid & ",2,0)")
                            End If
                        End If
                        'decode mastercoin transaction
                        Dim txdetails As mastercointx = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "send")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then
                            Dim dbwrite9 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.value & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & ",0)")
                        Else
                            If results.result.blocktime > 1377993875 Then 'after exodus cutofff
                                Dim btctxdetails As mastercointx_btcpayment = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "btcpayment")
                                If Not IsNothing(btctxdetails) Then Dim dbwrite2 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TYPE,BLOCKTIME,BLOCKNUM,CURTYPE,VOUTS,VALID) VALUES ('" & btctxdetails.txid & "','" & btctxdetails.fromadd & "','btcpayment'," & btctxdetails.blocktime & "," & blocknum & ",0,'" & btctxdetails.vouts & "',1)")
                            End If
                        End If
                    End If

                    '===DEx messages===
                    'sell offer
                    If workingtxtype = "selloffer" Then
                        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found MSC transaction (sell offer): " & txarray(j))
                        Dim txdetails As mastercointx_selloffer = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "selloffer")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION,ACTION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "'," & txdetails.saleamount & "," & txdetails.offeramount & "," & txdetails.minfee & "," & txdetails.timelimit & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & "," & txdetails.version & "," & txdetails.action & ")")
                    End If

                    'accept offer
                    If workingtxtype = "acceptoffer" Then
                        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found MSC transaction (accept offer): " & txarray(j))
                        Dim txdetails As mastercointx_acceptoffer = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "acceptoffer")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION,FEE) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.purchaseamount & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & ",0," & txdetails.fee & ")")
                    End If

                    '===SP messages===
                    'created SP - fixed
                    If workingtxtype = "spcreatefixed" Then
                        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found MSC transaction (create SP fixed): " & txarray(j))
                        Dim txdetails As mastercointx_spfixed = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "spcreatefixed")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,ECOSYSTEM,PROPTYPE,PREVID,STRCAT,STRSUBCAT,STRNAME,STRURL,STRDATA,SALEAMOUNT,CURTYPE,VERSION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.ecosystem & "," & txdetails.propertytype & "," & txdetails.previousid & ",'" & txdetails.category & "','" & txdetails.subcategory & "','" & txdetails.name & "','" & txdetails.url & "','" & txdetails.data & "'," & txdetails.numberproperties & ",9999999999,0)")
                    End If
                    'create SP - variable (fundraiser)
                    If workingtxtype = "spcreatevar" Then
                        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found MSC transaction (create SP crowdsale): " & txarray(j))
                        Dim txdetails As mastercointx_spvar = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "spcreatevar")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,ECOSYSTEM,PROPTYPE,PREVID,STRCAT,STRSUBCAT,STRNAME,STRURL,STRDATA,CURTYPE,SALEAMOUNT,DEADLINE,EARLYBONUS,PERCENTISSUER,VERSION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.ecosystem & "," & txdetails.propertytype & "," & txdetails.previousid & ",'" & txdetails.category & "','" & txdetails.subcategory & "','" & txdetails.name & "','" & txdetails.url & "','" & txdetails.data & "'," & txdetails.currencydesired & "," & txdetails.numberpropertiesperunit & "," & txdetails.deadline & "," & txdetails.earlybonus & "," & txdetails.percentforissuer & ",0)")
                    End If
                    'create SP - variable (fundraiser)
                    If workingtxtype = "spcancelvar" Then
                        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found MSC transaction (cancel SP crowdsale): " & txarray(j))
                        Dim txdetails As mastercointx_spcancelvar = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "spcancelvar")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.propid & ",0)")
                    End If

                Catch exx As Exception
                    Console.WriteLine("ERROR: Exception occured." & vbCrLf & exx.Message & vbCrLf & "Exiting...")
                End Try
            Next
            'only here do we write that the block has been processed to database
            Dim dbwrite3 As Integer = SQLGetSingleVal("INSERT INTO processedblocks VALUES (" & blocknum & "," & block.result.time & ",'" & block.result.hash & "')")
        Next
        workthread.ReportProgress(0, "100#")
        'handle unconfirmed transactions
        If debuglevel > 0 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: Block Analysis for pending transactions")
        Dim btemplate As blocktemplate = mlib.getblocktemplate(bitcoin_con)
        Dim intermedarray As bttx() = btemplate.result.transactions.ToArray
        For j = 1 To UBound(intermedarray) 'skip tx0 which should be coinbase
            Try
                Dim workingtxtype As String = mlib.ismastercointx(bitcoin_con, intermedarray(j).hash)
                'simple send
                If workingtxtype = "simple" Then
                    workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found pending MSC transaction (simple send): " & intermedarray(j).hash)
                    'decode mastercoin transaction
                    Dim txdetails As mastercointx = mlib.getmastercointransaction(bitcoin_con, intermedarray(j).hash.ToString, "send")
                    If Not IsNothing(txdetails) Then
                        Dim dbwrite9 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.value & ",'" & txdetails.type & "'," & txdetails.blocktime & ",999999," & txdetails.valid & "," & txdetails.curtype & ",0)")
                    Else
                        Dim btctxdetails As mastercointx_btcpayment = mlib.getmastercointransaction(bitcoin_con, intermedarray(j).hash.ToString, "btcpayment")
                        If Not IsNothing(btctxdetails) Then Dim dbwrite2 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TYPE,BLOCKTIME,BLOCKNUM,CURTYPE,VOUTS,VALID) VALUES ('" & btctxdetails.txid & "','" & btctxdetails.fromadd & "','btcpayment'," & btctxdetails.blocktime & ",999999,0,'" & btctxdetails.vouts & "',1)")
                    End If
                End If
                If workingtxtype = "selloffer" Then
                    workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found pending MSC transaction (sell offer): " & intermedarray(j).hash)
                    'decode mastercoin transaction
                    Dim txdetails As mastercointx_selloffer = mlib.getmastercointransaction(bitcoin_con, intermedarray(j).hash.ToString, "selloffer")
                    'see if we have a transaction back and if so write it to database
                    If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION,ACTION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "'," & txdetails.saleamount & "," & txdetails.offeramount & "," & txdetails.minfee & "," & txdetails.timelimit & ",'" & txdetails.type & "'," & txdetails.blocktime & ",999999," & txdetails.valid & "," & txdetails.curtype & "," & txdetails.version & "," & txdetails.action & ")")
                End If
                If workingtxtype = "acceptoffer" Then
                    workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Found pending MSC transaction (accept offer): " & intermedarray(j).hash)
                    Dim txdetails As mastercointx_acceptoffer = mlib.getmastercointransaction(bitcoin_con, intermedarray(j).hash.ToString, "acceptoffer")
                    'see if we have a transaction back and if so write it to database
                    If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION,FEE) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.purchaseamount & ",'" & txdetails.type & "'," & txdetails.blocktime & ",999999," & txdetails.valid & "," & txdetails.curtype & ",0," & txdetails.fee & ")")
                End If
            Catch exx As Exception
                Console.WriteLine("ERROR: Exception occured looking at unconfirmed transactions." & vbCrLf & exx.Message & vbCrLf & "Exiting...")
            End Try
        Next

        '///process transactions
        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Transaction processing starting... ")
        ' Try
        Dim nextfrexpiry As Long = 9999999999
        'dev sends temp
        Dim maxtime As Long = SQLGetSingleVal("SELECT MAX(BLOCKTIME) FROM processedblocks")
        Dim devmsc As Double = Math.Round(((1 - (0.5 ^ ((maxtime - 1377993874) / 31556926))) * 56316.23576222), 8)
        'do all generate transactions and calculate initial balances
        Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf")
        Dim cmd As New SqlCeCommand()
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "delete from transactions_processed_temp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from balances_temp2"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from exchange_temp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from properties_temp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from fundraisers_temp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into properties_temp values ('1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P',1,'Mastercoin','N/A','N/A','N/A','N/A',1)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into properties_temp values ('1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P',2,'Test Mastercoin','N/A','N/A','N/A','N/A',1)"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into transactions_processed_temp (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) SELECT TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE from transactions where type='generate'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into balances_temp2 (address, curtype, cbalance, ubalance) SELECT TOADD,1,SUM(VALUE),0 from transactions_processed_temp where curtype = 1 group by toadd"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into balances_temp2 (address, curtype, cbalance, ubalance) SELECT TOADD,2,SUM(VALUE),0 from transactions_processed_temp where curtype = 2 group by toadd"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into balances_temp2 (address, curtype, cbalance, ubalance) VALUES ('1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P',1," & (devmsc * 100000000) & ",0)"
        cmd.ExecuteNonQuery()
        'try and speed things up a little - drop some indexes into the right places
        'cmd.CommandText = "create unique index balindex on balances_temp2(address)"
        'cmd.ExecuteNonQuery()
        'cmd.CommandText = "create unique index dexindex on exchange_temp(fromadd)"
        'cmd.ExecuteNonQuery()
        'go through transactions, check validity, process by type and apply to balances
        Dim sqlquery
        Dim pendinglist As New List(Of String)
        Dim tmpblknum As Integer = 0
        Dim lasttmpblknum As Integer = 0
        Dim returnval
        sqlquery = "SELECT * FROM transactions order by ID ASC"
        cmd.CommandText = sqlquery
        Dim adptSQL As New SqlCeDataAdapter(cmd)
        Dim ds1 As New DataSet()
        adptSQL.Fill(ds1)
        'get total number of transactions to process
        Dim totaltxcount As Integer = ds1.Tables(0).Rows.Count
        With ds1.Tables(0)
            For rowNumber As Integer = 0 To .Rows.Count - 1
                With .Rows(rowNumber)
                    tmpblknum = .Item(6)
                    'update progress (don't go overboard - just every ten txs)
                    If (rowNumber Mod 80) = 0 Then
                        Dim percentdone As Integer = (rowNumber / totaltxcount) * 100
                        workthread.ReportProgress(0, percentdone.ToString & "%")
                    End If
                    '#####################################
                    'START PRE-TRANSACTION HANDLING CHECKS
                    '#####################################
                    '============================
                    'START CROWDSALE EXPIRY CHECK
                    '============================
                    If nextfrexpiry < .Item(5) Then
                        'a fundraiser has expired - what if multiple? loop through active fundraisers instead of blanket update - done
                        sqlquery = "SELECT * FROM fundraisers_temp where ACTIVE=1 AND deadline<" & .Item(5).ToString
                        cmd.CommandText = sqlquery
                        Dim adptSQLcrowd As New SqlCeDataAdapter(cmd)
                        Dim dscrowd As New DataSet()
                        adptSQLcrowd.Fill(dscrowd)
                        With dscrowd.Tables(0)
                            For rowcrowd As Integer = 0 To .Rows.Count - 1
                                With .Rows(rowcrowd)
                                    'get crowdsale txid, address & start/end times
                                    Dim cstxid = .Item(0)
                                    Dim csadd = .Item(1)
                                    Dim csstart = .Item(5)
                                    Dim csend = .Item(6)
                                    Dim icurdesired = .Item(2)
                                    Dim icurtype = .Item(3)
                                    Dim iamountperunit = .Item(4)
                                    Dim iearlybird = .Item(7)
                                    Dim iissuerpercent = .Item(8)
                                    'was there an issuer cut? if so go through and add missing fractions
                                    If iissuerpercent > 0 Then '###START ISSUER FRACTION CATCH UP
                                        Dim totalaward As ULong = 0
                                        Dim totalraised As ULong = 0
                                        Dim totalcut As ULong = 0
                                        sqlquery = "SELECT * FROM transactions_processed where type='investment' AND toadd='" + csadd + "' AND blocktime>" + csstart.ToString + " AND blocktime<=" + csend.ToString
                                        cmd.CommandText = sqlquery
                                        Dim adptSQLfund As New SqlCeDataAdapter(cmd)
                                        Dim dsfund As New DataSet()
                                        adptSQLfund.Fill(dsfund)
                                        With dsfund.Tables(0)
                                            For rowfund As Integer = 0 To .Rows.Count - 1
                                                With .Rows(rowfund)
                                                    'get amount awarded and add to totalawarded
                                                    Dim txamount = .Item(3)
                                                    Dim txtime = .Item(5)
                                                    Dim curtype = .Item(8)
                                                    'calculate investment
                                                    Dim srcamount As ULong = txamount
                                                    Dim investunits As Double = 0
                                                    Dim perunit As ULong = 0
                                                    Dim investreturn As Double = 0
                                                    'are funds being invested divisble?
                                                    cmd.CommandText = "select divisible from properties_temp where curtype=" & curtype
                                                    Dim srcdivisible As Boolean = cmd.ExecuteScalar
                                                    If srcdivisible = True Then
                                                        investunits = srcamount / 100000000
                                                    Else
                                                        investunits = srcamount
                                                    End If
                                                    'are tokens being returned divisible?
                                                    cmd.CommandText = "select divisible from properties_temp where curtype=" & icurtype
                                                    Dim dstdivisible As Boolean = cmd.ExecuteScalar
                                                    perunit = iamountperunit
                                                    'investment return
                                                    investreturn = investunits * perunit
                                                    'calculate bonus
                                                    Dim dif As Long = csend - txtime
                                                    Dim bonus As Double = (iearlybird / 100) * (dif / 604800)
                                                    If bonus < 0 Then bonus = 0 'avoid negative bonus
                                                    'apply bonus to investment return
                                                    investreturn = investreturn * (1 + bonus)
                                                    'truncate and shift to long ready to write to db
                                                    investreturn = Math.Truncate(investreturn)
                                                    Dim investreturnlong As ULong = investreturn
                                                    'calculate % for issuer
                                                    Dim issuercutlong As ULong
                                                    If iissuerpercent > 0 Then
                                                        Dim issuercut As Double = investreturn * (iissuerpercent / 100)
                                                        'truncate and shift to long ready to write to db
                                                        issuercut = Math.Truncate(issuercut)
                                                        issuercutlong = issuercut
                                                    End If
                                                    'add to totals
                                                    totalraised = totalraised + investreturnlong
                                                    totalaward = totalaward + issuercutlong
                                                End With
                                            Next
                                        End With
                                        'get amount that should have been awarded
                                        Dim totalcutdub As Double = totalraised * (iissuerpercent / 100)
                                        totalcut = Math.Truncate(totalcutdub)
                                        'if short, credit extra fractions
                                        If totalcut > totalaward Then
                                            'grant to issuer
                                            cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & (totalcut - totalaward) & " where CURTYPE=" & icurtype & " AND ADDRESS='" & csadd & "'"
                                            returnval = cmd.ExecuteNonQuery 'if 0 rows affected, address/curtype combo doesn't exist so insert it
                                            If returnval = 0 Then
                                                cmd.CommandText = "INSERT INTO balances_temp2 (ADDRESS,CURTYPE,CBALANCE,UBALANCE) VALUES ('" & csadd & "'," & icurtype & "," & (totalcut - totalaward) & ",0)"
                                                returnval = cmd.ExecuteScalar
                                            End If
                                        End If
                                    End If '###END ISSUER FRACTION CATCHUP
                                    'deactivate it
                                    cmd.CommandText = "update fundraisers_temp set active=0 where txid='" & cstxid & "'"
                                    returnval = cmd.ExecuteScalar
                                    'update next deadline
                                    cmd.CommandText = "select min(deadline) from fundraisers_temp"
                                    returnval = cmd.ExecuteScalar
                                    If Not IsDBNull(returnval) Then
                                        If Not returnval > 0 Then
                                            nextfrexpiry = 9999999999
                                        Else
                                            nextfrexpiry = returnval
                                        End If
                                    Else
                                        nextfrexpiry = 9999999999
                                    End If
                                End With
                            Next
                        End With
                    End If
                    '==========================
                    'END CROWDSALE EXPIRY CHECK
                    '==========================

                    '================================
                    'START PENDING OFFER EXPIRY CHECK
                    '================================
                    If pendinglist.Count > 0 And tmpblknum > 0 And tmpblknum <> lasttmpblknum Then '
                        Dim startpen As Long = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                        lasttmpblknum = tmpblknum
                        'blocktime has changed - get new blocknum
                        'optimization no need to go out to sqlce every block here, big waste of time - already have current blocknum in .item(6) - switch away from blocktime to blocknum
                        'get any expired pendingoffers
                        sqlquery = "select txid,matchingtx,purchaseamount,curtype,toadd,fromadd from transactions_processed_temp where type='pendingoffer' and expiry<" & tmpblknum
                        cmd.CommandText = sqlquery
                        Dim refundtxid As String = ""
                        Dim selltype As String = ""
                        Dim adptSQLexp As New SqlCeDataAdapter(cmd)
                        Dim dsexp As New DataSet()
                        adptSQLexp.Fill(dsexp)
                        With dsexp.Tables(0)
                            For rownum As Integer = 0 To .Rows.Count - 1
                                With .Rows(rownum)
                                    Dim tmpcur2 As Integer = .Item(3)
                                    'flip pending to expired
                                    sqlquery = "update transactions_processed_temp SET type='expiredoffer' where txid='" & .Item(0) & "'"
                                    cmd.CommandText = sqlquery
                                    returnval = cmd.ExecuteScalar
                                    'remove from pendinglist
                                    pendinglist.Remove(.Item(0))
                                    'return reserved funds
                                    sqlquery = "SELECT txid FROM exchange_temp where fromadd='" & .Item(4).ToString & "' and curtype=" & .Item(3).ToString
                                    cmd.CommandText = sqlquery
                                    refundtxid = cmd.ExecuteScalar
                                    If refundtxid <> "" Then
                                        sqlquery = "SELECT type FROM exchange_temp where txid='" & refundtxid & "'"
                                        cmd.CommandText = sqlquery
                                        selltype = cmd.ExecuteScalar
                                        sqlquery = "update exchange_temp SET reserved=reserved-" & .Item(2).ToString & " where txid='" & refundtxid & "'"
                                        cmd.CommandText = sqlquery
                                        returnval = cmd.ExecuteScalar
                                        sqlquery = "update exchange_temp SET saleamount=saleamount+" & .Item(2).ToString & " where txid='" & refundtxid & "'"
                                        cmd.CommandText = sqlquery
                                        returnval = cmd.ExecuteScalar

                                        If selltype = "sellpendingcancel" Then
                                            'sell has been cancelled
                                            'attempt cancellation again - credit back remaining saleamount
                                            'get sell details from exchange table
                                            Dim tmpreserved As Long
                                            Dim tmpsaleamount As Long
                                            sqlquery = "SELECT RESERVED FROM exchange_temp where txid='" & refundtxid & "'"
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                            If IsDBNull(returnval) Then
                                                tmpreserved = 0
                                            Else
                                                tmpreserved = returnval
                                            End If
                                            sqlquery = "SELECT SALEAMOUNT FROM exchange_temp where txid='" & refundtxid & "'"
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                            If IsDBNull(returnval) Then
                                                tmpsaleamount = 0
                                            Else
                                                tmpsaleamount = returnval
                                            End If
                                            cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & tmpsaleamount & " where CURTYPE=" & .Item(3).ToString & " AND ADDRESS='" & .Item(4).ToString & "'"
                                            returnval = cmd.ExecuteScalar
                                            If tmpreserved = 0 Then 'nothing reserved, cancel whole sell
                                                'delete it from exchange table
                                                sqlquery = "DELETE FROM exchange_temp where txid='" & refundtxid & "'"
                                                cmd.CommandText = sqlquery
                                                returnval = cmd.ExecuteScalar
                                            Else 'funds reserved, zero saleamount but do not remove sell as still active
                                                cmd.CommandText = "UPDATE exchange_temp SET SALEAMOUNT=0 where txid='" & refundtxid & "'"
                                                returnval = cmd.ExecuteScalar
                                            End If
                                        End If
                                    End If
                                End With
                            Next
                        End With
                    End If
                    '================================
                    'END PENDING OFFER EXPIRY CHECK
                    '================================
                    '#####################################
                    'END PRE-TRANSACTION HANDLING CHECKS
                    '#####################################

                    '###################################
                    'START TRANSACTION HANDLING ROUTINES
                    '###################################
                    '=================================
                    'START SIMPLE SEND PROCESSING CODE
                    '=================================
                    If .Item(4) = "simple" Then
                        'get vars for transaction
                        Dim curtype As UInteger = .Item(8)
                        Dim txamount As Long = .Item(3)
                        Dim sender As String = .Item(1)
                        Dim recip As String = .Item(2)
                        Dim sendtxtype As String = "simple"
                        'check if send is going to an open crowdsale - should be 1 or 0 crowdsales, flip type to investment if there is active crowdsale on this address/curtype
                        cmd.CommandText = "SELECT count(address) from fundraisers_temp where ADDRESS='" & recip & "' and CURDESIRED=" & curtype & "AND active=1"
                        returnval = cmd.ExecuteScalar
                        If returnval = 1 Then sendtxtype = "investment"
                        If curtype > 0 Then 'sanity check not CurID:0
                            'check if transaction amount is over senders balance
                            cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE-" & txamount & " where CURTYPE=" & curtype & " AND ADDRESS='" & sender & "' AND CBALANCE>=" & txamount
                            returnval = cmd.ExecuteNonQuery
                            If returnval = 1 Then 'ok, balance update was ok and sender had enough balance - proceed
                                'handle additional investment steps
                                If sendtxtype = "investment" Then
                                    'get crowdsale info
                                    Dim txtime As ULong = .Item(5)
                                    Dim icurdesired, icurtype, iamountperunit, ideadline As ULong
                                    Dim iearlybird, iissuerpercent, istarttime As Integer
                                    cmd.CommandText = "select * from fundraisers_temp where ADDRESS='" & recip & "' and CURDESIRED=" & curtype & " AND ACTIVE=1"
                                    Dim adptSQLfr As New SqlCeDataAdapter(cmd)
                                    Dim dsfr As New DataSet()
                                    adptSQLfr.Fill(dsfr)
                                    If dsfr.Tables(0).Rows.Count = 1 Then 'should only ever be one row
                                        With dsfr.Tables(0).Rows(0)
                                            icurdesired = .Item(2)
                                            icurtype = .Item(3)
                                            iamountperunit = .Item(4)
                                            istarttime = .Item(5)
                                            ideadline = .Item(6)
                                            iearlybird = .Item(7)
                                            iissuerpercent = .Item(8)
                                        End With
                                        'calculate investment
                                        Dim srcamount As ULong = txamount
                                        Dim investunits As Double = 0
                                        Dim perunit As ULong = 0
                                        Dim investreturn As Double = 0
                                        'are funds being invested divisble?
                                        cmd.CommandText = "select divisible from properties_temp where curtype=" & curtype
                                        Dim srcdivisible As Boolean = cmd.ExecuteScalar
                                        If srcdivisible = True Then
                                            investunits = srcamount / 100000000
                                        Else
                                            investunits = srcamount
                                        End If
                                        'are tokens being returned divisible?
                                        cmd.CommandText = "select divisible from properties_temp where curtype=" & icurtype
                                        Dim dstdivisible As Boolean = cmd.ExecuteScalar
                                        perunit = iamountperunit
                                        'investment return
                                        investreturn = investunits * perunit
                                        'calculate bonus
                                        Dim dif As Long = ideadline - txtime
                                        Dim bonus As Double = (iearlybird / 100) * (dif / 604800)
                                        If bonus < 0 Then bonus = 0 'avoid negative bonus
                                        'apply bonus to investment return
                                        investreturn = investreturn * (1 + bonus)
                                        'truncate and shift to long ready to write to db
                                        investreturn = Math.Truncate(investreturn)
                                        Dim investreturnlong As ULong = investreturn
                                        'credit SP tokens to sender
                                        cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & investreturnlong & " where CURTYPE=" & icurtype & " AND ADDRESS='" & sender & "'"
                                        returnval = cmd.ExecuteNonQuery 'if 0 rows affected, address/curtype combo doesn't exist so insert it
                                        If returnval = 0 Then
                                            cmd.CommandText = "INSERT INTO balances_temp2 (ADDRESS,CURTYPE,CBALANCE,UBALANCE) VALUES ('" & sender & "'," & icurtype & "," & investreturnlong & ",0)"
                                            returnval = cmd.ExecuteScalar
                                        End If
                                        'calculate % for issuer
                                        If iissuerpercent > 0 Then
                                            Dim issuercut As Double = investreturn * (iissuerpercent / 100)
                                            'truncate and shift to long ready to write to db
                                            issuercut = Math.Truncate(issuercut)
                                            Dim issuercutlong As ULong = issuercut
                                            'credit SP tokens to issuer
                                            cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & issuercut & " where CURTYPE=" & icurtype & " AND ADDRESS='" & recip & "'"
                                            returnval = cmd.ExecuteNonQuery 'if 0 rows affected, address/curtype combo doesn't exist so insert it
                                            If returnval = 0 Then
                                                cmd.CommandText = "INSERT INTO balances_temp2 (ADDRESS,CURTYPE,CBALANCE,UBALANCE) VALUES ('" & recip & "'," & icurtype & "," & issuercut & ",0)"
                                                returnval = cmd.ExecuteScalar
                                            End If
                                        End If
                                    End If
                                End If 'end handling additional investment tasks

                                'credit recipient funds
                                If .Item(6) < 999998 Then
                                    cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & txamount & " where CURTYPE=" & curtype & " AND ADDRESS='" & recip & "'"
                                    returnval = cmd.ExecuteNonQuery 'if 0 rows affected, address/curtype combo doesn't exist so insert it
                                    If returnval = 0 Then
                                        cmd.CommandText = "INSERT INTO balances_temp2 (ADDRESS,CURTYPE,CBALANCE,UBALANCE) VALUES ('" & recip & "'," & curtype & "," & txamount & ",0)"
                                        returnval = cmd.ExecuteScalar
                                    End If
                                Else
                                    cmd.CommandText = "UPDATE balances_temp2 SET UBALANCE=UBALANCE+" & txamount & " where CURTYPE=" & curtype & " AND ADDRESS='" & recip & "'"
                                    returnval = cmd.ExecuteNonQuery 'if 0 rows affected, address/curtype combo doesn't exist so insert it
                                    If returnval = 0 Then
                                        cmd.CommandText = "INSERT INTO balances_temp2 (ADDRESS,CURTYPE,CBALANCE,UBALANCE) VALUES ('" & recip & "'," & curtype & ",0," & txamount & ")"
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If
                                'write transaction to DB
                                cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & sender & "','" & recip & "'," & .Item(3) & ",'" & sendtxtype & "'," & .Item(5) & "," & .Item(6) & ",1," & .Item(8) & ")"
                                returnval = cmd.ExecuteScalar
                            Else 'transaction not valid, insufficient funds, write transaction to DB
                                cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & sender & "','" & recip & "'," & .Item(3) & ",'" & sendtxtype & "'," & .Item(5) & "," & .Item(6) & ",0," & .Item(8) & ")"
                                returnval = cmd.ExecuteScalar
                            End If
                        End If
                    End If
                    '==============================
                    'END IMPLE SEND PROCESSING CODE
                    '==============================

                    '================================
                    'START SELL OFFER PROCESSING CODE
                    '================================
                    If .Item(4) = "selloffer" Then
                        'only include unconfirmed if it's ours else ignore
                        Dim ismine As Boolean = False
                        If .Item(6) > 9999998 Then
                            For Each address In addresslist.Rows
                                If .Item(0) = .Item(1).ToString Then
                                    ismine = True
                                End If
                            Next
                        End If
                        If .Item(6) < 9999998 Or ismine = True Then
                            'get details
                            Dim curtype As Integer = .Item(8)
                            Dim sellaction As Integer = 0
                            Dim sellexists As Integer = 0
                            Dim saleamount As Long = .Item(10)
                            Dim fromadd As String = .Item(1)
                            'check if sell already exists
                            sqlquery = "SELECT COUNT(fromadd) FROM exchange_temp where FROMADD='" & .Item(1).ToString & "' and curtype=" & curtype
                            cmd.CommandText = sqlquery
                            returnval = cmd.ExecuteScalar
                            sellexists = returnval
                            'check version/action
                            If .Item(16) = 1 Then 'version 1 transaction, pull action explicitly
                                If Not IsDBNull(.Item(15)) Then sellaction = .Item(15)
                            End If
                            If .Item(16) = 1 And Not IsDBNull(.Item(15)) And Not IsDBNull(.Item(10)) Then
                                If .Item(15) = 2 And .Item(10) = 0 Then sellaction = 3 'handle edge case action=update,amount=0
                            End If
                            If .Item(16) = 0 Then 'version 0 transaction, infer action from exchange state
                                If sellexists = 1 Then
                                    sellaction = 2 'update				
                                Else
                                    sellaction = 1 'new
                                End If
                                If .Item(10) = 0 Then
                                    sellaction = 3 'cancel
                                End If
                            End If
                            'drop transaction by invalidating curtype if sell is MSC before live block of 290630
                            If .Item(6) < 290630 And curtype = 1 Then curtype = -1
                            'validate curtype (Meta DEx not yet supported, only MSC/TMSC) and ensure we have a valid action
                            If curtype > 0 And curtype < 3 And sellaction > 0 And sellaction < 4 Then
                                'sanity check exchange table to ensure not more than one sell for address
                                If sellexists > 1 Then
                                    MsgBox("Sanity check has failed.  More than one sell for an address exists in the exchange table.  It is not safe to continue.  Exiting...")
                                    End
                                End If
                                'get sell details from exchange table
                                Dim tmpreserved As Long
                                Dim tmpsaleamount As Long
                                sqlquery = "SELECT RESERVED FROM exchange_temp where FROMADD='" & .Item(1).ToString & "' and curtype=" & curtype
                                cmd.CommandText = sqlquery
                                returnval = cmd.ExecuteScalar
                                If IsDBNull(returnval) Then
                                    tmpreserved = 0
                                Else
                                    tmpreserved = returnval
                                End If
                                sqlquery = "SELECT SALEAMOUNT FROM exchange_temp where FROMADD='" & .Item(1).ToString & "' and curtype=" & curtype
                                cmd.CommandText = sqlquery
                                returnval = cmd.ExecuteScalar
                                If IsDBNull(returnval) Then
                                    tmpsaleamount = 0
                                Else
                                    tmpsaleamount = returnval
                                End If

                                'ACTION: CANCEL
                                If sellaction = 3 Then
                                    If .Item(6) > 999998 Then
                                        'set the sell to be pending
                                        cmd.CommandText = "UPDATE exchange_temp SET BLOCKNUM=999999 where FROMADD='" & .Item(1).ToString & "' AND CURTYPE=" & curtype
                                        returnval = cmd.ExecuteScalar
                                    Else
                                        If sellexists = 1 Then
                                            'credit back remaining saleamount
                                            cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & tmpsaleamount & " where CURTYPE=" & curtype & " AND ADDRESS='" & .Item(1).ToString & "'"
                                            returnval = cmd.ExecuteScalar
                                            'are there funds reserved?
                                            If tmpreserved = 0 Then 'nothing reserved, cancel whole sell
                                                'delete it from exchange table
                                                sqlquery = "DELETE FROM exchange_temp where FROMADD='" & .Item(1).ToString & "' and curtype=" & curtype
                                                cmd.CommandText = sqlquery
                                                returnval = cmd.ExecuteScalar
                                            Else 'funds reserved, zero saleamount but do not remove sell as still some funds reserved
                                                cmd.CommandText = "UPDATE exchange_temp SET SALEAMOUNT=0 where FROMADD='" & .Item(1).ToString & "' and curtype=" & curtype
                                                returnval = cmd.ExecuteScalar
                                                cmd.CommandText = "UPDATE exchange_temp SET TYPE='sellpendingcancel' where FROMADD='" & .Item(1).ToString & "' and curtype=" & curtype
                                                returnval = cmd.ExecuteScalar
                                            End If
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & tmpsaleamount & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "cancelsell" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & ")"
                                            returnval = cmd.ExecuteScalar
                                        Else
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & .Item(10) & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "cancelsell" & "'," & .Item(5) & "," & .Item(6) & ",0," & curtype & ")"
                                            returnval = cmd.ExecuteScalar
                                        End If
                                    End If
                                End If

                                'ACTION: UPDATE
                                If sellaction = 2 And .Item(10) > 0 And .Item(11) > 0 And .Item(13) > 0 Then 'sanity check we have all the necessary details
                                    'check previous sell to update
                                    If sellexists = 1 Then
                                        'calculate update difference
                                        Dim tmpsaleam As Long = 0
                                        Dim tmpvalid As Boolean = False
                                        Dim tmpdiff As Long = saleamount - tmpsaleamount
                                        Dim tmpunitprice As Long
                                        tmpunitprice = (.Item(11) / (saleamount / 100000000))
                                        If tmpdiff > 0 Then 'new sell is higher than existing sell
                                            'get seller balance
                                            cmd.CommandText = "SELECT CBALANCE FROM balances_temp2 where CURTYPE=" & curtype & " AND ADDRESS='" & .Item(1).ToString & "'"
                                            returnval = cmd.ExecuteScalar
                                            'check if transaction amount is over senders balance, if not diff amount = senders balance
                                            If returnval < tmpdiff Then
                                                tmpdiff = returnval
                                            End If
                                            tmpsaleam = tmpdiff + tmpsaleamount
                                            'reduce seller balance accordingly
                                            cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE-" & tmpdiff & " where CURTYPE=" & curtype & " AND ADDRESS='" & .Item(1).ToString & "'"
                                            returnval = cmd.ExecuteScalar
                                            tmpvalid = True
                                        Else 'new sell is lower than or equal to existing sell
                                            'return difference to seller balance
                                            If tmpsaleamount - saleamount > 0 Then
                                                cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & (tmpsaleamount - saleamount) & " where CURTYPE=" & curtype & " AND ADDRESS='" & .Item(1).ToString & "'"
                                                returnval = cmd.ExecuteScalar
                                            End If
                                            tmpsaleam = saleamount
                                            tmpvalid = True
                                        End If
                                        If tmpvalid = True Then
                                            'recalculate offeramount
                                            Dim tmpofferamount As Long = tmpsaleam * (tmpunitprice / 100000000)
                                            'put in exchange & processed tables
                                            cmd.CommandText = "UPDATE exchange_temp SET TXID='" & .Item(0) & "',SALEAMOUNT=" & tmpsaleam & ",OFFERAMOUNT=" & tmpofferamount.ToString & ",MINFEE=" & .Item(12) & ",TIMELIMIT=" & .Item(13) & ",BLOCKTIME=" & .Item(5) & ",BLOCKNUM=" & .Item(6) & ",UNITPRICE=" & tmpunitprice & " WHERE FROMADD='" & .Item(1) & "' and curtype=" & curtype
                                            returnval = cmd.ExecuteScalar
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & tmpsaleam & "," & tmpofferamount.ToString & "," & .Item(12) & "," & .Item(13) & ",'" & "updatesell" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & ")"
                                            returnval = cmd.ExecuteScalar
                                        Else
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & tmpsaleam & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "updatesell" & "'," & .Item(5) & "," & .Item(6) & ",0," & curtype & ")"
                                            returnval = cmd.ExecuteScalar
                                        End If
                                    Else
                                        cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & .Item(10) & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "updatesell" & "'," & .Item(5) & "," & .Item(6) & ",0," & curtype & ")"
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If

                                'ACTION: NEW
                                If sellaction = 1 And .Item(10) > 0 And .Item(11) > 0 And .Item(13) > 0 Then 'sanity check we have all the necessary details
                                    'check there is not already an existing sell
                                    Dim tmpofferamount As Long
                                    If sellexists = 0 Then
                                        'there is no existing sell, we can go ahead - first check sellers balance
                                        cmd.CommandText = "SELECT CBALANCE FROM balances_temp2 where CURTYPE=" & curtype & " AND ADDRESS='" & .Item(1).ToString & "'"
                                        returnval = cmd.ExecuteScalar
                                        Dim tmpunitprice As Long
                                        tmpunitprice = (.Item(11) / (saleamount / 100000000))
                                        'check if transaction amount is over senders balance, if not sell amount = senders balance
                                        tmpofferamount = .Item(11)
                                        If returnval > 0 And returnval < saleamount Then
                                            'reduce saleamount to balance
                                            saleamount = returnval
                                            'recalculate offeramount
                                            tmpofferamount = (saleamount * tmpunitprice) / 100000000
                                        End If
                                        If returnval >= saleamount Then 'ok
                                            'reduce seller balance
                                            cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE-" & saleamount & " where CURTYPE=" & curtype & " AND ADDRESS='" & .Item(1).ToString & "'"
                                            returnval = cmd.ExecuteScalar
                                            'put in exchange & processed tables
                                            cmd.CommandText = "INSERT INTO exchange_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,UNITPRICE,RESERVED) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & saleamount & "," & tmpofferamount & "," & .Item(12) & "," & .Item(13) & ",'" & "selloffer" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & "," & tmpunitprice & ",0)"
                                            returnval = cmd.ExecuteScalar
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & saleamount & "," & tmpofferamount & "," & .Item(12) & "," & .Item(13) & ",'" & "selloffer" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & ")"
                                            returnval = cmd.ExecuteScalar
                                        Else
                                            'insufficient balance
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & saleamount & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "selloffer" & "'," & .Item(5) & "," & .Item(6) & ",0," & curtype & ")"
                                            returnval = cmd.ExecuteScalar
                                        End If
                                    Else
                                        'there is an existing sell, we can't create a new one - invalidate sell offer
                                        cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & saleamount & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "selloffer" & "'," & .Item(5) & "," & .Item(6) & ",0," & curtype & ")"
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If
                            End If
                        End If
                    End If
                    '==============================
                    'END SELL OFFER PROCESSING CODE
                    '==============================

                    '==================================
                    'START ACCEPT OFFER PROCESSING CODE
                    '==================================
                    If .Item(4) = "acceptoffer" Then
                        'get currency type
                        Dim curtype As Integer = .Item(8)
                        'validate curid
                        If curtype > 0 And curtype < 3 Then
                            Dim matchedunitprice As Long
                            'get transaction amount
                            Dim purchaseamount As Long = .Item(14)
                            Dim paymentpaidlong As Long
                            Dim toadd As String = .Item(2)
                            Dim matchedsaleamount, matchedofferamount, matchedminfee As Long
                            Dim matchedtimelimit As Integer = 0
                            Dim expiry As Integer = 0
                            Dim txid As String
                            Dim matchingtx As String = ""
                            'look for a sell at the reference address and verify there is only one sell (sanity check)
                            sqlquery = "SELECT COUNT(fromadd) FROM exchange_temp where FROMADD='" & .Item(2).ToString & "' and curtype=" & curtype
                            cmd.CommandText = sqlquery
                            returnval = cmd.ExecuteScalar
                            If returnval > 1 Then
                                MsgBox("Sanity check has failed.  More than one sell for an address exists in the exchange table.  It is not safe to continue.  Exiting...")
                                End
                            End If
                            If returnval = 1 Then
                                'look for a pending accept from this address and make sure there isn't one
                                sqlquery = "SELECT COUNT(txid) FROM transactions_processed_temp where FROMADD='" & .Item(1).ToString & "' and TOADD='" & .Item(2).ToString & "' and TYPE='pendingoffer' and curtype=" & curtype
                                cmd.CommandText = sqlquery
                                returnval = cmd.ExecuteScalar
                                If returnval = 0 Then
                                    'matched a sell offer - get details
                                    sqlquery = "SELECT txid,saleamount,timelimit,unitprice,minfee,offeramount FROM exchange_temp where FROMADD='" & .Item(2).ToString & "' and curtype=" & curtype
                                    cmd.CommandText = sqlquery
                                    Dim adptSQLselldetails As New SqlCeDataAdapter(cmd)
                                    Dim dsselldetails As New DataSet()
                                    adptSQLselldetails.Fill(dsselldetails)
                                    If dsselldetails.Tables(0).Rows.Count = 1 Then
                                        matchingtx = dsselldetails.Tables(0).Rows(0).Item(0)
                                        matchedsaleamount = dsselldetails.Tables(0).Rows(0).Item(1)
                                        matchedtimelimit = dsselldetails.Tables(0).Rows(0).Item(2)
                                        matchedunitprice = dsselldetails.Tables(0).Rows(0).Item(3)
                                        matchedminfee = dsselldetails.Tables(0).Rows(0).Item(4)
                                        matchedofferamount = dsselldetails.Tables(0).Rows(0).Item(5)
                                        'check if transaction amount is over saleamount, if not set purchaseamount = saleamount
                                        If purchaseamount > matchedsaleamount Then purchaseamount = matchedsaleamount
                                        'set expiry
                                        expiry = .Item(6) + matchedtimelimit
                                        'check saleamount is not zero, if it is reject offer
                                        If matchedsaleamount > 0 Then
                                            'check accept transaction paid over minfee
                                            Dim paidfee As Long = 0
                                            If Not IsDBNull(.Item(17)) Then
                                                paidfee = .Item(17)
                                            Else
                                                paidfee = 0
                                            End If
                                            If paidfee >= matchedminfee Then
                                                sqlquery = "UPDATE exchange_temp SET saleamount=saleamount-" & purchaseamount & " where FROMADD='" & .Item(2).ToString & "' and curtype=" & curtype
                                                cmd.CommandText = sqlquery
                                                returnval = cmd.ExecuteScalar
                                                sqlquery = "UPDATE exchange_temp SET reserved=reserved+" & purchaseamount & " where FROMADD='" & .Item(2).ToString & "' and curtype=" & curtype
                                                cmd.CommandText = sqlquery
                                                returnval = cmd.ExecuteScalar
                                                'add to transactions_processed - is it our unconfirmed accept?
                                                If .Item(6) > 999998 Then
                                                    sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX,EXPIRY) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & purchaseamount & ",'pendingoffer'," & .Item(5) & ",999999,1," & .Item(8) & ",'" & matchingtx & "'," & expiry & ")"
                                                    cmd.CommandText = sqlquery
                                                    returnval = cmd.ExecuteScalar
                                                Else
                                                    sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX,EXPIRY) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & purchaseamount & ",'pendingoffer'," & .Item(5) & "," & .Item(6) & ",1," & .Item(8) & ",'" & matchingtx & "'," & expiry & ")"
                                                    cmd.CommandText = sqlquery
                                                    returnval = cmd.ExecuteScalar
                                                    'add to pendinglist
                                                    pendinglist.Add(.Item(0))
                                                End If
                                            Else
                                                'minfee not sufficient, reject accept
                                                sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'rejectedoffer'," & .Item(5) & "," & .Item(6) & ",0," & .Item(8) & ",'" & matchingtx & "')"
                                                cmd.CommandText = sqlquery
                                                returnval = cmd.ExecuteScalar
                                            End If
                                        Else
                                            'no MSC available in matched sell offer, reject accept
                                            sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'rejectedoffer'," & .Item(5) & "," & .Item(6) & ",0," & .Item(8) & ",'" & matchingtx & "')"
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                        End If
                                    Else
                                        'error matching, reject
                                        sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'rejectedoffer'," & .Item(5) & "," & .Item(6) & ",0," & .Item(8) & ",'" & matchingtx & "')"
                                        cmd.CommandText = sqlquery
                                        returnval = cmd.ExecuteScalar
                                    End If
                                Else
                                    'already an accept for this sell, reject
                                    sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'rejectedoffer'," & .Item(5) & "," & .Item(6) & ",0," & .Item(8) & ",'" & matchingtx & "')"
                                    cmd.CommandText = sqlquery
                                    returnval = cmd.ExecuteScalar
                                End If
                            Else
                                'no matching sell, reject accept
                                sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'rejectedoffer'," & .Item(5) & "," & .Item(6) & ",0," & .Item(8) & ",'" & matchingtx & "')"
                                cmd.CommandText = sqlquery
                                returnval = cmd.ExecuteScalar
                            End If
                        End If
                    End If
                    '================================
                    'END ACCEPT OFFER PROCESSING CODE
                    '================================

                    '=================================
                    'START BTC PAYMENT PROCESSING CODE
                    '=================================
                    If .Item(4) = "btcpayment" And .Item(6) > 265984 Then 'quickly invalidate spam payments from buymastercoins in the early days, since no sell offers before 265985 there can be no valid bitcoin payment dex messages before thenThen
                        'get a list of the pending accepts this from address has open
                        sqlquery = "select txid,toadd,matchingtx,purchaseamount from transactions_processed_temp where fromadd='" & .Item(1) & "' and type='pendingoffer'"
                        cmd.CommandText = sqlquery
                        Dim adptSQLbtc As New SqlCeDataAdapter(cmd)
                        Dim dsbtc As New DataSet()
                        adptSQLbtc.Fill(dsbtc)
                        Dim vouts As String = .Item(18)
                        Dim paymentpaidlong As Long = 0
                        Dim matched As Boolean = False
                        Dim matchedtxid As String = ""
                        Dim buyer As String = ""
                        Dim seller As String = ""
                        Dim matchedcurtype As Integer = 0
                        Dim matchedsaleamount, matchedofferamount, matchedminfee As Long
                        Dim matchedtimelimit As Integer
                        Dim matchedreserved As Long
                        Dim matchedunitprice As Long
                        Dim matchingtx As String
                        Dim purchaseamount As Long = 0
                        buyer = .Item(1).ToString
                        With dsbtc.Tables(0)
                            For rownum As Integer = 0 To .Rows.Count - 1
                                With .Rows(rownum)
                                    'match payment to accept offer - paying multiple accepts to different sellers in the same bitcoin payment currently unsupported
                                    If InStr(vouts, .Item(1)) Then 'matched output with accept
                                        matched = True
                                        matchedtxid = .Item(0)
                                        seller = .Item(1).ToString
                                        purchaseamount = .Item(3)
                                        matchingtx = .Item(2)
                                        Exit For
                                    End If
                                End With
                            Next
                        End With

                        If matched = True And seller <> "" Then
                            'is it unconfirmed?  if so flip the pendingoffer to unconfirmed temporarily
                            If .Item(6) > 999998 Then
                                Dim unconfirmedpaymentcount = SQLGetSingleVal("update transactions_processed_temp set blocknum=999999 where txid='" & matchedtxid & "'")
                            Else
                                'matched a sell offer - get details
                                sqlquery = "SELECT txid,saleamount,timelimit,minfee,offeramount,curtype FROM transactions_processed_temp where txid='" & matchingtx & "'"
                                cmd.CommandText = sqlquery
                                Dim adptSQLselldetails As New SqlCeDataAdapter(cmd)
                                Dim dsselldetails As New DataSet()
                                adptSQLselldetails.Fill(dsselldetails)
                                If dsselldetails.Tables(0).Rows.Count = 1 Then
                                    matchingtx = dsselldetails.Tables(0).Rows(0).Item(0)
                                    matchedsaleamount = dsselldetails.Tables(0).Rows(0).Item(1)
                                    matchedtimelimit = dsselldetails.Tables(0).Rows(0).Item(2)
                                    matchedminfee = dsselldetails.Tables(0).Rows(0).Item(3)
                                    matchedofferamount = dsselldetails.Tables(0).Rows(0).Item(4)
                                    matchedcurtype = dsselldetails.Tables(0).Rows(0).Item(5)
                                    Dim paymentpaid As Double = 0
                                    'loop through all outputs in transaction and total amount sent to reference address
                                    Dim txn As txn = mlib.gettransaction(bitcoin_con, .Item(0))
                                    If txn IsNot Nothing Then
                                        For Each output As Vout In txn.result.vout
                                            If output.scriptPubKey.type.ToString.ToLower = "pubkeyhash" And output.scriptPubKey.addresses(0) = seller Then
                                                paymentpaid = paymentpaid + output.value
                                            End If
                                        Next
                                    End If
                                    paymentpaidlong = paymentpaid * 100000000
                                    matchedunitprice = (matchedofferamount / (matchedsaleamount / 100000000))
                                    'work out total amount of units covered by payment
                                    Dim unitspurchased As Long
                                    If paymentpaid = 0 Then
                                        unitspurchased = 0
                                    Else
                                        'unitspurchased = (paymentpaidlong / matchedunitprice) * 100000000
                                        Dim perc As Double = paymentpaidlong / matchedofferamount
                                        unitspurchased = matchedsaleamount * perc
                                    End If

                                    If unitspurchased > 0 Then
                                        'are there more than were accepted? if so set as total accepted
                                        If unitspurchased > purchaseamount Then unitspurchased = purchaseamount
                                        'catch tiny fraction unit prices (eg 0.0000001)  with tiny saleamounts
                                        If paymentpaidlong >= matchedofferamount Then unitspurchased = purchaseamount
                                        'credit tokens
                                        cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & unitspurchased & " where CURTYPE=" & matchedcurtype & " AND ADDRESS='" & buyer & "'"
                                        returnval = cmd.ExecuteNonQuery 'if 0 rows affected, address/curtype combo doesn't exist so insert it
                                        If returnval = 0 Then
                                            cmd.CommandText = "INSERT INTO balances_temp2 (ADDRESS,CURTYPE,CBALANCE,UBALANCE) VALUES ('" & buyer & "'," & matchedcurtype & "," & unitspurchased & ",0)"
                                            returnval = cmd.ExecuteScalar
                                        End If
                                        'reduce/delete sell - is it fully paid?
                                        Dim tmpsaleamount, tmpreserved
                                        sqlquery = "SELECT saleamount FROM exchange_temp where FROMADD='" & seller & "' and curtype=" & matchedcurtype
                                        cmd.CommandText = sqlquery
                                        tmpsaleamount = cmd.ExecuteScalar
                                        sqlquery = "SELECT reserved FROM exchange_temp where FROMADD='" & seller & "' and curtype=" & matchedcurtype
                                        cmd.CommandText = sqlquery
                                        tmpreserved = cmd.ExecuteScalar
                                        If tmpsaleamount = 0 And tmpreserved = unitspurchased Then 'delete sell & flip offer from pending to accept
                                            sqlquery = "DELETE FROM exchange_temp where fromadd='" & seller & "' and curtype=" & matchedcurtype
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                            sqlquery = "update transactions_processed_temp SET type='acceptoffer' where txid='" & matchedtxid & "'"
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                        Else
                                            sqlquery = "UPDATE exchange_temp SET reserved=reserved-" & unitspurchased & " where fromadd='" & seller & "' and curtype=" & matchedcurtype
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                        End If
                                        'is accept fully paid
                                        If unitspurchased = purchaseamount Then 'flip pending to accept, otherwise leave pending and adjust purchaseamount on the accept/reserved on sell accordingly
                                            sqlquery = "update transactions_processed_temp SET type='acceptoffer' where txid='" & matchedtxid & "'"
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                            pendinglist.Remove(matchedtxid)
                                        Else
                                            sqlquery = "update transactions_processed_temp SET purchaseamount=purchaseamount-" & unitspurchased & " where txid='" & matchedtxid & "'"
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                            sqlquery = "update exchange_temp SET reserved=reserved-" & unitspurchased & " where txid='" & matchedtxid & "'"
                                            cmd.CommandText = sqlquery
                                            returnval = cmd.ExecuteScalar
                                        End If
                                        'write transaction
                                        sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,OFFERAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & seller & "','" & buyer & "'," & unitspurchased & "," & paymentpaidlong & ",'" & "purchase" & "'," & .Item(5) & "," & .Item(6).ToString & ",1," & matchedcurtype & ",'" & matchedtxid & "')"
                                        cmd.CommandText = sqlquery
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If
                            End If
                        Else
                            'failed - unmatched
                            sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,OFFERAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & buyer & "','UNMATCHED',0," & paymentpaidlong & ",'" & "payment" & "'," & .Item(5) & "," & .Item(6).ToString & ",0," & matchedcurtype & ",'N/A')"
                            cmd.CommandText = sqlquery
                            returnval = cmd.ExecuteScalar
                        End If
                    End If
                    '=================================
                    'END BTC PAYMENT PROCESSING CODE
                    '=================================

                    '=====================================
                    'START SP FIXED CREATE PROCESSING CODE
                    '=====================================
                    If .Item(4) = "spcreatefixed" Then
                        'read attributes
                        Dim speco As Integer = .Item(19)
                        Dim proptype As Integer = .Item(20)
                        Dim previd As UInteger = .Item(21)
                        Dim strcat As String = .Item(22)
                        Dim strsubcat As String = .Item(23)
                        Dim strname As String = .Item(24)
                        Dim strurl As String = .Item(25)
                        Dim strdata As String = .Item(26)
                        Dim address As String = .Item(1)
                        Dim divisible As Integer = 0
                        Dim amountoftokens As ULong = .Item(10)
                        'get next property ID
                        If speco = 2 Or (speco = 1 And .Item(6) > 297110) Then 'block 297110 live SP
                            Dim curtype As UInteger
                            If speco = 2 Then
                                cmd.CommandText = "SELECT MAX(CURTYPE) FROM PROPERTIES_TEMP"
                                returnval = cmd.ExecuteScalar
                                curtype = returnval + 1
                                If curtype < 2147483649 Then curtype = 2147483651
                            End If
                            If speco = 1 Then
                                cmd.CommandText = "SELECT MAX(CURTYPE) FROM PROPERTIES_TEMP WHERE CURTYPE<2147483648"
                                returnval = cmd.ExecuteScalar
                                curtype = returnval + 1
                            End If

                            If proptype > 0 And proptype < 3 Then
                                If proptype = 1 Then divisible = 0
                                If proptype = 2 Then divisible = 1
                                'note - what other validation can occur here?
                                'write to database - credit address with new tokens, add property to properties table and add to tx processed
                                cmd.CommandText = "INSERT INTO BALANCES_TEMP2 VALUES ('" & address & "'," & curtype & "," & amountoftokens & ",0)"
                                returnval = cmd.ExecuteScalar
                                cmd.CommandText = "INSERT INTO PROPERTIES_TEMP VALUES ('" & address & "'," & curtype & ",'" & strname & "','" & strcat & "','" & strsubcat & "','" & strdata & "','" & strurl & "'," & divisible & ")"
                                returnval = cmd.ExecuteScalar
                                cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & address & "','N/A','spcreatefixed" & "'," & .Item(5) & "," & .Item(6).ToString & ",1," & curtype & ")"
                                returnval = cmd.ExecuteScalar
                            Else
                                cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & address & "','N/A','spcreatefixed" & "'," & .Item(5) & "," & .Item(6).ToString & ",0,9999999999)"
                                returnval = cmd.ExecuteScalar
                            End If
                        Else
                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & address & "','N/A','spcreatefixed" & "'," & .Item(5) & "," & .Item(6).ToString & ",0,9999999999)"
                            returnval = cmd.ExecuteScalar
                        End If
                    End If
                    '===================================
                    'END SP CREATE FIXED PROCESSING CODE
                    '===================================

                    '=========================================
                    'START SP CREATE CROWDSALE PROCESSING CODE
                    '=========================================
                    If .Item(4) = "spcreatevar" Then
                        'read attributes
                        Dim speco As Integer = .Item(19)
                        Dim proptype As Integer = .Item(20)
                        Dim previd As UInteger = .Item(21)
                        Dim strcat As String = .Item(22)
                        Dim strsubcat As String = .Item(23)
                        Dim strname As String = .Item(24)
                        Dim strurl As String = .Item(25)
                        Dim strdata As String = .Item(26)
                        Dim deadline As Long = .Item(27)
                        Dim earlybird As Integer = .Item(28)
                        Dim issuerpercent As Integer = .Item(29)
                        Dim address As String = .Item(1)
                        Dim curdesired As UInteger = .Item(8)
                        Dim divisible As Integer = 0
                        Dim amountoftokens As ULong = .Item(10)
                        Dim maxcurexist As UInteger = 0
                        'ensure deadline not in past
                        If deadline > .Item(5) Then
                            'get next property ID
                            If speco = 2 Or (speco = 1 And .Item(6) > 297110) Then 'block 297110 live SP
                                Dim curtype As UInteger
                                If speco = 2 Then
                                    cmd.CommandText = "SELECT MAX(CURTYPE) FROM PROPERTIES_TEMP"
                                    returnval = cmd.ExecuteScalar
                                    curtype = returnval + 1
                                    If curtype < 2147483649 Then curtype = 2147483651
                                End If
                                If speco = 1 Then
                                    cmd.CommandText = "SELECT MAX(CURTYPE) FROM PROPERTIES_TEMP WHERE CURTYPE<2147483648 "
                                    returnval = cmd.ExecuteScalar
                                    curtype = returnval + 1
                                End If

                                If proptype > 0 And proptype < 3 Then
                                    If proptype = 1 Then divisible = 0
                                    If proptype = 2 Then divisible = 1
                                    'ensure not already an open fundraiser
                                    cmd.CommandText = "SELECT COUNT(ADDRESS) FROM FUNDRAISERS_TEMP WHERE ADDRESS='" & address & "' AND ACTIVE=1"
                                    returnval = cmd.ExecuteScalar
                                    If returnval = 0 Then
                                        'further validation here?
                                        'is curdesired valid?
                                        Dim maxcurexist1, maxcurexist2
                                        cmd.CommandText = "SELECT MAX(CURTYPE) FROM PROPERTIES_TEMP WHERE CURTYPE<2147483648"
                                        maxcurexist1 = cmd.ExecuteScalar
                                        cmd.CommandText = "SELECT MAX(CURTYPE) FROM PROPERTIES_TEMP"
                                        maxcurexist2 = cmd.ExecuteScalar
                                        If _
                                            (speco = 2 And ((curdesired > 2147483650 And curdesired <= maxcurexist2) Or curdesired = 2)) _
                                            Or _
                                            (speco = 1 And (curdesired > 0 And curdesired <= maxcurexist1)) Then
                                            'write to database - open fundraiser position, add property to properties table and add to tx processed
                                            cmd.CommandText = "INSERT INTO FUNDRAISERS_TEMP VALUES ('" & .Item(0) & "','" & address & "'," & curdesired & "," & curtype & "," & amountoftokens & "," & .Item(5) & "," & deadline & "," & earlybird & "," & issuerpercent & ",1)"
                                            returnval = cmd.ExecuteScalar
                                            cmd.CommandText = "INSERT INTO PROPERTIES_TEMP VALUES ('" & address & "'," & curtype & ",'" & strname & "','" & strcat & "','" & strsubcat & "','" & strdata & "','" & strurl & "'," & divisible & ")"
                                            returnval = cmd.ExecuteScalar
                                            cmd.CommandText = "INSERT INTO BALANCES_TEMP2 VALUES ('" & address & "'," & curtype & ",0,0)"
                                            returnval = cmd.ExecuteScalar
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & address & "','N/A','spcreatevar" & "'," & .Item(5) & "," & .Item(6).ToString & ",1," & curtype & ")"
                                            returnval = cmd.ExecuteScalar
                                            'set next fundraiser expiry
                                            cmd.CommandText = "SELECT MIN(DEADLINE) FROM FUNDRAISERS_TEMP"
                                            nextfrexpiry = cmd.ExecuteScalar
                                        Else
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & address & "','N/A','spcreatevar" & "'," & .Item(5) & "," & .Item(6).ToString & ",0,9999999999)"
                                            returnval = cmd.ExecuteScalar
                                        End If
                                    Else
                                        cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & address & "','N/A','spcreatevar" & "'," & .Item(5) & "," & .Item(6).ToString & ",0,9999999999)"
                                        returnval = cmd.ExecuteScalar
                                    End If
                                Else
                                    cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & address & "','N/A','spcreatevar" & "'," & .Item(5) & "," & .Item(6).ToString & ",0,9999999999)"
                                    returnval = cmd.ExecuteScalar
                                End If
                            End If
                        End If
                    End If
                    '=======================================
                    'END SP CREATE CROWDSALE PROCESSING CODE
                    '=======================================

                    '=========================================
                    'START SP CROWDSALE CANCEL PROCESSING CODE
                    '=========================================
                    If .Item(4) = "spcancelvar" Then
                        'a fundraiser has been closed early
                        'get prop id
                        Dim propid As UInteger = .Item(8)
                        Dim fromadd As String = .Item(1)
                        sqlquery = "SELECT * FROM fundraisers_temp where ACTIVE=1 AND CURTYPE=" & propid & " AND ADDRESS='" & fromadd & "'"
                        cmd.CommandText = sqlquery
                        Dim adptSQLcrowd As New SqlCeDataAdapter(cmd)
                        Dim dscrowdc As New DataSet()
                        adptSQLcrowd.Fill(dscrowdc)
                        If dscrowdc.Tables(0).Rows.Count = 1 Then
                            With dscrowdc.Tables(0).Rows(0)
                                'get crowdsale txid, address & start/end times
                                Dim cstxid = .Item(0)
                                Dim csadd = .Item(1)
                                Dim csstart = .Item(5)
                                Dim csend = .Item(6)
                                Dim icurdesired = .Item(2)
                                Dim icurtype = .Item(3)
                                Dim iamountperunit = .Item(4)
                                Dim iearlybird = .Item(7)
                                Dim iissuerpercent = .Item(8)
                                'was there an issuer cut? if so go through and add missing fractions
                                If iissuerpercent > 0 Then '###START ISSUER FRACTION CATCH UP
                                    Dim totalaward As ULong = 0
                                    Dim totalraised As ULong = 0
                                    Dim totalcut As ULong = 0
                                    sqlquery = "SELECT * FROM transactions_processed where type='investment' AND toadd='" + csadd + "' AND blocktime>" + csstart.ToString + " AND blocktime<=" + csend.ToString
                                    cmd.CommandText = sqlquery
                                    Dim adptSQLfund As New SqlCeDataAdapter(cmd)
                                    Dim dsfund As New DataSet()
                                    adptSQLfund.Fill(dsfund)
                                    With dsfund.Tables(0)
                                        For rowfund As Integer = 0 To .Rows.Count - 1
                                            With .Rows(rowfund)
                                                'get amount awarded and add to totalawarded
                                                Dim txamount = .Item(3)
                                                Dim txtime = .Item(5)
                                                Dim curtype = .Item(8)
                                                'calculate investment
                                                Dim srcamount As ULong = txamount
                                                Dim investunits As Double = 0
                                                Dim perunit As ULong = 0
                                                Dim investreturn As Double = 0
                                                'are funds being invested divisble?
                                                cmd.CommandText = "select divisible from properties_temp where curtype=" & curtype
                                                Dim srcdivisible As Boolean = cmd.ExecuteScalar
                                                If srcdivisible = True Then
                                                    investunits = srcamount / 100000000
                                                Else
                                                    investunits = srcamount
                                                End If
                                                'are tokens being returned divisible?
                                                cmd.CommandText = "select divisible from properties_temp where curtype=" & icurtype
                                                Dim dstdivisible As Boolean = cmd.ExecuteScalar
                                                perunit = iamountperunit
                                                'investment return
                                                investreturn = investunits * perunit
                                                'calculate bonus
                                                Dim dif As Long = csend - txtime
                                                Dim bonus As Double = (iearlybird / 100) * (dif / 604800)
                                                If bonus < 0 Then bonus = 0 'avoid negative bonus
                                                'apply bonus to investment return
                                                investreturn = investreturn * (1 + bonus)
                                                'truncate and shift to long ready to write to db
                                                investreturn = Math.Truncate(investreturn)
                                                Dim investreturnlong As ULong = investreturn
                                                'calculate % for issuer
                                                Dim issuercutlong As ULong
                                                If iissuerpercent > 0 Then
                                                    Dim issuercut As Double = investreturn * (iissuerpercent / 100)
                                                    'truncate and shift to long ready to write to db
                                                    issuercut = Math.Truncate(issuercut)
                                                    issuercutlong = issuercut
                                                End If
                                                'add to totals
                                                totalraised = totalraised + investreturnlong
                                                totalaward = totalaward + issuercutlong
                                            End With
                                        Next
                                    End With
                                    'get amount that should have been awarded
                                    Dim totalcutdub As Double = totalraised * (iissuerpercent / 100)
                                    totalcut = Math.Truncate(totalcutdub)
                                    'if short, credit extra fractions
                                    If totalcut > totalaward Then
                                        'grant to issuer
                                        cmd.CommandText = "UPDATE balances_temp2 SET CBALANCE=CBALANCE+" & (totalcut - totalaward) & " where CURTYPE=" & icurtype & " AND ADDRESS='" & csadd & "'"
                                        returnval = cmd.ExecuteNonQuery 'if 0 rows affected, address/curtype combo doesn't exist so insert it
                                        If returnval = 0 Then
                                            cmd.CommandText = "INSERT INTO balances_temp2 (ADDRESS,CURTYPE,CBALANCE,UBALANCE) VALUES ('" & csadd & "'," & icurtype & "," & (totalcut - totalaward) & ",0)"
                                            returnval = cmd.ExecuteScalar
                                        End If
                                    End If
                                End If '###END ISSUER FRACTION CATCHUP
                                'deactivate it
                                cmd.CommandText = "update fundraisers_temp set active=0 where txid='" & cstxid & "'"
                                returnval = cmd.ExecuteScalar
                                'update next deadline
                                cmd.CommandText = "select min(deadline) from fundraisers_temp"
                                returnval = cmd.ExecuteScalar
                                If Not IsDBNull(returnval) Then
                                    If Not returnval > 0 Then
                                        nextfrexpiry = 9999999999
                                    Else
                                        nextfrexpiry = returnval
                                    End If
                                Else
                                    nextfrexpiry = 9999999999
                                End If
                            End With
                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "','SPCANCELVAR'," & .Item(5) & "," & .Item(6) & ",1," & .Item(8) & ")"
                            returnval = cmd.ExecuteScalar
                        Else
                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "','SPCANCELVAR'," & .Item(5) & "," & .Item(6) & ",0," & .Item(8) & ")"
                            returnval = cmd.ExecuteScalar
                        End If
                    End If
                    '=======================================
                    'END SP CREATE CROWDSALE PROCESSING CODE
                    '=======================================
                    '#################################
                    'END TRANSACTION HANDLING ROUTINES
                    '#################################
                End With
            Next
        End With

        'processing
        Dim tmpcur
        If dexcur = "MSC" Then tmpcur = 1
        If dexcur = "TMSC" Then tmpcur = 2
        'drop real tables and copy from temp
        cmd.CommandText = "drop table exchange"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "create table [exchange] ([txid] [nvarchar](100) not null, [fromadd] [nvarchar](100) not null, [type] [nvarchar](100) not null, [blocktime] [bigint] not null, [blocknum] [int] not null, [valid] [bit] not null, [curtype] [int] not null, [saleamount] [bigint] null, [offeramount] [bigint] null, [minfee] [bigint] null, [timelimit] [int] null, [purchaseamount] [bigint] null, [unitprice] [bigint] not null, [reserved] [bigint] null)"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "INSERT INTO exchange select * FROM exchange_temp"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "drop table balances"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "create table balances (ADDRESS nvarchar(100) not null, CURTYPE bigint not null, CBALANCE bigint not null default 0, UBALANCE bigint not null default 0)"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "INSERT INTO balances select * from balances_temp2"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "drop table transactions_processed"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "create table transactions_processed (txid nvarchar(100) not null, fromadd nvarchar(100) not null, toadd nvarchar(100) null, value bigint null, type nvarchar(100) not null, blocktime bigint not null, blocknum int not null, valid bit not null, curtype bigint not null, ID int IDENTITY(1,1), saleamount bigint null, offeramount bigint null, minfee bigint null, timelimit int null, purchaseamount bigint null, matchingtx nvarchar(100) null, expiry bigint null)"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "INSERT INTO transactions_processed (txid,fromadd,toadd,value,type,blocktime,blocknum,valid,curtype,saleamount,offeramount,minfee,timelimit,purchaseamount,matchingtx,expiry) select txid,fromadd,toadd,value,type,blocktime,blocknum,valid,curtype,saleamount,offeramount,minfee,timelimit,purchaseamount,matchingtx,expiry from transactions_processed_temp order by id asc"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "drop table properties"
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "CREATE TABLE properties (ADDRESS nvarchar(100) NOT NULL, CURTYPE bigint NOT NULL, NAME ntext NOT NULL, CATEGORY ntext NOT NULL, SUBCATEGORY ntext NOT NULL, DATA ntext NOT NULL, URL ntext NOT NULL, DIVISIBLE bit NOT NULL)"
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "INSERT INTO properties (address,curtype,name,category,subcategory,data,url,divisible) select address,curtype,name,category,subcategory,data,url,divisible from properties_temp"
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "drop table fundraisers"
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "CREATE TABLE fundraisers (TXID nvarchar(100) NULL, ADDRESS nvarchar(100) NULL, CURDESIRED bigint NULL, CURTYPE bigint NULL, AMOUNTPERUNIT bigint NULL, STARTTIME bigint NULL, DEADLINE bigint NULL, EARLYBIRD int NULL, ISSUERPERCENT int NULL, active bit NULL)"
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "INSERT INTO fundraisers (txid, address, curdesired, curtype, amountperunit, starttime, deadline, earlybird,issuerpercent,active) select txid, address, curdesired, curtype, amountperunit, starttime, deadline, earlybird,issuerpercent,active from fundraisers_temp"
        returnval = cmd.ExecuteNonQuery



        'update address balances
        'enumarate bitcoin addresses
        If debuglevel > 0 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: Enumerating addresses...")
        balubtc = 0
        Try
            Dim addresses As List(Of btcaddressbal) = mlib.getaddresses(bitcoin_con)
            taddresslist.Clear()
            For Each address In addresses
                taddresslist.Rows.Add(address.address, address.amount, 0, 0)
                balubtc = balubtc + address.uamount
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
            workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] ERROR: Enumerating addresses did not complete properly." & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
            Exit Sub
        End Try

        balmsc = 0
        baltmsc = 0
        balbtc = 0
        balumsc = 0
        balutmsc = 0
        balrestmsc = 0
        balresmsc = 0

        'update address list balances
        For Each row In taddresslist.Rows
            sqlquery = "SELECT CBALANCE FROM balances where ADDRESS='" & row.Item(0) & "' AND CURTYPE=1"
            Dim addbal As Long = SQLGetSingleVal(sqlquery)
            Dim hrbal As Double = addbal / 100000000
            balmsc = balmsc + addbal
            If hrbal <> row.Item(3) Then row.Item(3) = hrbal

            sqlquery = "SELECT CBALANCE FROM balances where ADDRESS='" & row.Item(0) & "' AND CURTYPE=2"
            Dim addtbal As Long = SQLGetSingleVal(sqlquery)
            Dim hrtbal As Double = addtbal / 100000000
            baltmsc = baltmsc + addtbal
            If hrtbal <> row.Item(2) Then row.Item(2) = hrtbal

            sqlquery = "SELECT UBALANCE FROM balances where ADDRESS='" & row.Item(0) & "' AND CURTYPE=1"
            Dim addubal As Long = SQLGetSingleVal(sqlquery)
            balumsc = balumsc + addubal

            sqlquery = "SELECT UBALANCE FROM balances where ADDRESS='" & row.Item(0) & "' AND CURTYPE=2"
            Dim addutbal As Long = SQLGetSingleVal(sqlquery)
            balutmsc = balutmsc + addutbal

            sqlquery = "SELECT RESERVED FROM exchange where FROMADD='" & row.Item(0) & "' and curtype=1"
            Dim addresbal As Long = SQLGetSingleVal(sqlquery)
            balresmsc = balresmsc + addresbal
            sqlquery = "SELECT SALEAMOUNT FROM exchange where FROMADD='" & row.Item(0) & "' and curtype=1"
            Dim addresbal2 As Long = SQLGetSingleVal(sqlquery)
            balresmsc = balresmsc + addresbal2

            sqlquery = "SELECT RESERVED FROM exchange where FROMADD='" & row.Item(0) & "' and curtype=2"
            Dim addresbal3 As Long = SQLGetSingleVal(sqlquery)
            balrestmsc = balrestmsc + addresbal3
            sqlquery = "SELECT SALEAMOUNT FROM exchange where FROMADD='" & row.Item(0) & "' and curtype=2"
            Dim addresbal4 As Long = SQLGetSingleVal(sqlquery)
            balrestmsc = balrestmsc + addresbal4

            balbtc = balbtc + row.item(1)

        Next
        'update history - use temp table to reduce ui lag while updating
        thistorylist.Clear()
        For Each row In taddresslist.Rows
            sqlquery = "SELECT txid,valid,fromadd,toadd,curtype,value,blocknum,type,saleamount,purchaseamount,blocktime FROM transactions_processed where (FROMADD='" & row.Item(0) & "' or TOADD='" & row.item(0) & "')"
            cmd.CommandText = sqlquery
            If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd.CommandText)
            Dim adptSQL2 As New SqlCeDataAdapter(cmd)
            Dim ds2 As New DataSet()
            adptSQL2.Fill(ds2)
            Dim cur, txid As String
            Dim valimg, dirimg As System.Drawing.Bitmap
            Dim txexists As Boolean = False

            With ds2.Tables(0)
                For rowNumber As Integer = 0 To .Rows.Count - 1
                    With .Rows(rowNumber)
                        txexists = False
                        txid = .Item(0)
                        If .Item(4) = 1 Then cur = "Mastercoin"
                        If .Item(4) = 2 Then cur = "Test Mastercoin"
                        If .Item(4) > 2 Then cur = "SPT #" + .Item(4).ToString
                        If .Item(1) = False Then valimg = My.Resources.invalid
                        If .Item(1) = True Then valimg = My.Resources.valid
                        If .Item(6) = 999999 Then valimg = My.Resources.uncof 'unconfirmed tx
                        cmd.CommandText = "SELECT DIVISIBLE FROM PROPERTIES WHERE CURTYPE=" & .Item(4)
                        Dim divis = cmd.ExecuteScalar
                        If Not IsDBNull(.Item(2)) Then
                            If row.item(0) = .Item(2) Then dirimg = My.Resources.out1
                        End If
                        If Not IsDBNull(.Item(3)) Then
                            If row.item(0) = .Item(3) Then dirimg = My.Resources.in1
                        End If
                        Dim dtdatetime As System.DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
                        If .Item(6) = 999999 Then
                            dtdatetime = New DateTime(9999, 12, 30, 0, 0, 0, 0) 'unconfirmed tx
                        Else
                            dtdatetime = dtdatetime.AddSeconds(.Item(10)).ToLocalTime()
                        End If
                        Dim txtype As String
                        Dim amo As Double
                        Dim amol As ULong
                        If .Item(7) = "simple" Then
                            txtype = "Send"
                            amol = .Item(5)
                        End If
                        If .Item(7) = "pendingoffer" Then
                            txtype = "Buy (Pending)"
                            amol = .Item(9)
                        End If
                        If .Item(7) = "expiredoffer" Then
                            txtype = "Buy (Expired)"
                            amol = .Item(9)
                        End If
                        If .Item(7) = "rejectedoffer" Then
                            txtype = "Buy (Rejected)"
                            amol = .Item(9)
                        End If
                        If .Item(7) = "selloffer" Then
                            txtype = "Sell (Open)"
                            amol = .Item(8)
                        End If
                        If .Item(7) = "updatesell" Then
                            txtype = "Sell (Update)"
                            amol = .Item(8)
                        End If
                        If .Item(7) = "cancelsell" Then
                            txtype = "Sell (Cancel)"
                            amol = 0
                        End If
                        If .Item(7) = "purchase" Then
                            'am i seller of this purchase?
                            Dim ismine As Boolean = False
                            For Each row2 In taddresslist.Rows
                                If row2.item(0) = .Item(2) Then
                                    ismine = True
                                End If
                            Next
                            If ismine = True Then
                                txtype = "Sell"
                            Else
                                txtype = "Buy"
                            End If
                            amol = .Item(9)
                        End If
                        If divis = True Then
                            amo = amol / 100000000
                        Else
                            amo = amol
                        End If

                        Dim fadd, tadd
                        If Not IsDBNull(.Item(2)) Then
                            fadd = .Item(2)
                        Else
                            fadd = "N/A"
                        End If
                        If Not IsDBNull(.Item(3)) Then
                            tadd = .Item(3)
                        Else
                            tadd = "N/A"
                        End If
                        thistorylist.Rows.Add(valimg, dirimg, dtdatetime, fadd, tadd, txtype, cur, amo, txid)
                    End With
                Next

            End With
        Next

        'done
        refreshdex()
        Application.DoEvents()
        con.Close()

        varsyncronized = True
        varsyncblock = rpcblock
        workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] BLOCKSCAN: Finished, sleeping. ")

    End Sub

    '//////////////////////////////
    '///// FUNCTIONS
    '//////////////////////////////
    Public Sub refreshdex()
        'exchange scan
        Dim econ As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        Dim cmd2 As New SqlCeCommand()
        cmd2.Connection = econ
        econ.Open()
        'calculate values for chart
        'get lastblock unix time
        cmd2.CommandText = "select MAX(blocktime) from processedblocks"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd2.CommandText)
        Dim curtime = cmd2.ExecuteScalar
        Dim hval(62) As Double
        Dim lval(62) As Double
        Dim cval(62) As Double
        Dim low(31) As Double
        Dim high(31) As Double
        Dim open(31) As Double
        Dim close(31) As Double
        Dim dt(31) As String
        Dim tmpcur
        If dexcur = "MSC" Then tmpcur = 1
        If dexcur = "TMSC" Then tmpcur = 2
        'loop through last 60 days
        For i = 60 To 2 Step -2
            Dim starttime = curtime - (86400 * (i))
            Dim endtime = curtime - (86400 * (i - 2))
            'get days high
            cmd2.CommandText = "select purchaseamount,offeramount,blocktime from transactions_processed where type='purchase' and purchaseamount>10000000 and curtype=" & tmpcur.ToString & " AND blocktime>" & starttime.ToString & " AND blocktime<" & endtime.ToString
            If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd2.CommandText)
            Dim adptSQLhigh As New SqlCeDataAdapter(cmd2)
            Dim dshigh As New DataSet()
            adptSQLhigh.Fill(dshigh)
            With dshigh.Tables(0)
                lval(i) = 99999999
                Dim closetime
                For rowNumber As Integer = 0 To .Rows.Count - 1
                    With .Rows(rowNumber)
                        Dim chkprice As Double = Math.Round(.Item(1) / (.Item(0) / 100000000) / 100000000, 8)
                        If chkprice >= hval(i) Then hval(i) = chkprice
                        If chkprice <= lval(i) Then lval(i) = chkprice
                        If .Item(2) > closetime Then cval(i) = chkprice
                        closetime = .Item(2)
                    End With
                Next
            End With
            Try
                If hval(i) = 0 Then hval(i) = hval(i + 2)
                If lval(i) = 99999999 Or lval(i) = 0 Then lval(i) = hval(i + 2)
                If hval(i) = 0 Then hval(i) = 0 ' 0.0001
                If lval(i) = 99999999 Or lval(i) = 0 Then lval(i) = 0 ' 0.0001
                If cval(i) = 0 Then cval(i) = close((i / 2) + 1)
            Catch ex As Exception
            End Try
            Dim stime = DateAdd(DateInterval.Second, starttime, New DateTime(1970, 1, 1, 0, 0, 0))
            Dim etime = DateAdd(DateInterval.Second, endtime, New DateTime(1970, 1, 1, 0, 0, 0))
            dt(i / 2) = etime.ToString("dd/MM") '& "-" & etime.ToString("dd/MM"))
            low(i / 2) = lval(i)
            open(i / 2) = close((i / 2) + 1)
            close(i / 2) = cval(i)
            high(i / 2) = hval(i)
        Next
        Dim chxl As String = "chxl=0:|"
        For i = 31 To 1 Step -3
            If i = 31 Then
                chxl = chxl & dt(30) & "|"
            Else
                chxl = chxl & dt(i) & "|"
            End If
        Next
        chxl = chxl.Substring(0, Len(chxl) - 1)
        Dim chd As String = "chd=t0:"
        For i = 30 To 1 Step -1
            chd = chd & low(i) & ","
        Next
        chd = chd.Substring(0, Len(chd) - 1)
        chd = chd & "|"
        For i = 30 To 1 Step -1
            chd = chd & open(i) & ","
        Next
        chd = chd.Substring(0, Len(chd) - 1)
        chd = chd & "|"
        For i = 30 To 1 Step -1
            chd = chd & close(i) & ","
        Next
        chd = chd.Substring(0, Len(chd) - 1)
        chd = chd & "|"
        For i = 30 To 1 Step -1
            chd = chd & high(i) & ","
        Next
        chd = chd.Substring(0, Len(chd) - 1)
        Dim highrg As Double = 0
        Dim lowrg As Double = 999999999
        For i = 1 To 30
            If high(i) > highrg Then highrg = high(i)
            If low(i) < lowrg Then lowrg = low(i)
        Next
        If lowrg < 0.001 Then lowrg = 0
        highrg = (highrg / 10) + highrg
        Dim gcurl = "http://chart.googleapis.com/chart?cht=lc&" & chd & "&chm=F,,0,-1,10&chs=426x205&chf=bg,s,252526&chxt=x,y&chds=" & lowrg & "," & highrg & "&chxr=1," & lowrg & "," & highrg & "&chxs=1N*F4*,D1D1D1,11,0,lt|0,D1D1D1,11,0,lt&" & chxl
        'Dim ignore = InputBox("URL", "URL", gcurl)
        Dim gcimg As Bitmap = New System.Drawing.Bitmap(New IO.MemoryStream(New System.Net.WebClient().DownloadData(gcurl)))
        'flip black pixels to white to make up for charts api colour control
        For y As Integer = 0 To gcimg.Height - 1
            For x As Integer = 0 To gcimg.Width - 1
                Dim clr As Color = gcimg.GetPixel(x, y)
                If (CInt(clr.R) + clr.G + clr.B) < 50 Then
                    gcimg.SetPixel(x, y, Color.White)
                End If
                'If x > 1 And x < 21 And y > 174 And y < 189 Then
                'gcimg.SetPixel(x, y, Color.FromArgb(37, 37, 38))
                'End If
            Next x
        Next y
        'gcimg.SetPixel(21, 183, Color.FromArgb(81, 81, 81))
        'gcimg.SetPixel(21, 184, Color.FromArgb(81, 81, 81))
        'gcimg.SetPixel(21, 185, Color.FromArgb(81, 81, 81))
        picpricehistory.Image = gcimg

        'open orders
        'sells first
        cmd2.CommandText = "select txid,fromadd,saleamount,offeramount,purchaseamount,curtype,type,reserved,blocknum,unitprice from exchange where type='selloffer' and curtype=" & tmpcur.ToString
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd2.CommandText)
        Dim adptSQLopen As New SqlCeDataAdapter(cmd2)
        Dim dsopen As New DataSet()
        adptSQLopen.Fill(dsopen)
        If Not IsNothing(openorders) Then openorders.Clear()
        For i = 0 To dsopen.Tables(0).Rows.Count - 1
            Dim isordermine As Boolean = False
            With dsopen.Tables(0).Rows(i)
                For Each addressrow In addresslist.Rows
                    If addressrow.item(0).ToString = .Item(1).ToString Then
                        isordermine = True
                    End If
                    'isordermine = True 'debugging - show all open orders for everyone
                Next
                Dim newrow2 = openorders.NewRow
                If isordermine = True Then
                    newrow2("TXID") = .Item(0).ToString
                    newrow2("Seller") = .Item(1).ToString
                    newrow2("Buyer") = "N/A"
                    Dim tmpavail As Double = (.Item(2) / 100000000)
                    newrow2("Available") = tmpavail.ToString("######0.00######")
                    If IsDBNull(.Item(7)) Then
                        newrow2("Reserved") = "0.00"
                    Else
                        Dim tmpres As Double = (.Item(7) / 100000000)
                        newrow2("Reserved") = tmpres.ToString("######0.00######")
                    End If
                    Dim tmpunit As Double = (.Item(9) / 100000000)
                    newrow2("Unit Price") = tmpunit.ToString("######0.00######")
                    Dim tmptotal As Double = (.Item(3) / 100000000)
                    newrow2("Total Price") = tmptotal.ToString("######0.00######")
                    newrow2("Type") = "Sell"
                    Dim purch As Double = SQLGetSingleVal("select sum(purchaseamount) from transactions_processed where type='purchase' and matchingtx='" & .Item(0).ToString & "'")
                    If IsDBNull(purch) Then newrow2("Purchased") = "0.00"
                    If Not IsDBNull(purch) Then
                        Dim tmppurch As Double = (purch / 100000000)
                        newrow2("Purchased") = tmppurch.ToString("######0.00######")
                    End If
                    If .Item(8) > 999998 Then
                        newrow2("Status") = "Pending"
                    Else
                        newrow2("Status") = "Open"
                    End If
                    openorders.Rows.Add(newrow2)
                End If
            End With
        Next
        'accepts next
        cmd2.CommandText = "select txid,fromadd,toadd,purchaseamount,curtype,type,blocknum,matchingtx from transactions_processed where type='pendingoffer' and curtype=" & tmpcur.ToString
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd2.CommandText)
        Dim adptSQLopen2 As New SqlCeDataAdapter(cmd2)
        Dim dsopen2 As New DataSet()
        adptSQLopen2.Fill(dsopen2)
        For i = 0 To dsopen2.Tables(0).Rows.Count - 1
            Dim isordermine As Boolean = False
            With dsopen2.Tables(0).Rows(i)
                For Each addressrow In addresslist.Rows
                    If addressrow.item(0).ToString = .Item(1).ToString Then
                        isordermine = True
                    End If
                Next
                Dim newrow2 = openorders.NewRow
                If isordermine = True Then
                    newrow2("TXID") = .Item(0).ToString
                    newrow2("Seller") = .Item(2).ToString
                    newrow2("Buyer") = .Item(1).ToString
                    newrow2("Available") = "N/A"
                    If IsDBNull(.Item(3)) Then
                        newrow2("Reserved") = "0.00"
                    Else
                        Dim tmpres As Double = (.Item(3) / 100000000)
                        newrow2("Reserved") = tmpres.ToString("######0.00######")
                    End If
                    Dim total As Double = SQLGetSingleVal("select offeramount from transactions_processed where txid='" & .Item(7).ToString & "'")
                    Dim sale As Double = SQLGetSingleVal("select saleamount from transactions_processed where txid='" & .Item(7).ToString & "'")
                    Dim tmpunit As Double = (total / sale)
                    newrow2("Unit Price") = tmpunit.ToString("######0.00######")
                    Dim tmptotal As Double = ((.Item(3) / 100000000) * tmpunit)
                    newrow2("Total Price") = tmptotal.ToString("######0.00######")
                    newrow2("Type") = "Buy"
                    newrow2("Purchased") = "0.00"
                    If .Item(6) > 999998 Then
                        newrow2("Status") = "Pending"
                    Else
                        newrow2("Status") = "Unpaid"
                    End If
                    openorders.Rows.Add(newrow2)
                End If
            End With
        Next
        'empty open orders?
        If openorders.Rows.Count < 1 Then
            Dim newrow2 = openorders.NewRow
            newrow2("Seller") = "N/A"
            openorders.Rows.Add(newrow2)
        End If
        'sell table
        cmd2.CommandText = "select txid,fromadd,saleamount,offeramount,curtype,unitprice from exchange where type='selloffer' and blocknum>240000 and blocknum<999999 and curtype=" & tmpcur.ToString & " order by unitprice ASC"
        If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL: " & cmd2.CommandText)
        Dim adptSQLsell As New SqlCeDataAdapter(cmd2)
        Dim dssell As New DataSet()
        adptSQLsell.Fill(dssell)
        If Not IsNothing(selloffers) Then selloffers.Clear()
        For i = 0 To dssell.Tables(0).Rows.Count - 1
            With dssell.Tables(0).Rows(i)
                Dim newrow = selloffers.NewRow()
                newrow("seller") = .Item(1).ToString
                Dim am As Double = Math.Round(.Item(2) / 100000000, 8)
                newrow("Amount") = am.ToString("######0.00######")
                Dim un As Double = Math.Round(.Item(5) / 100000000, 8)
                newrow("Unit Price") = un.ToString("######0.00######")
                Dim tot As Double = Math.Round(.Item(3) / 100000000, 8)
                newrow("Total Price") = tot.ToString("######0.00######")
                selloffers.Rows.Add(newrow)
            End With
        Next
    End Sub

    Public Sub Upgrade()
        'Usage
        Dim instance As New SqlCeEngine("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        instance.Upgrade()
    End Sub
    Public Function SQLGetSingleVal(ByVal sqlquery)
        Try
            'Upgrade()
            Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf")
            Dim cmd As New SqlCeCommand()
            Dim returnval
            If debuglevel > 1 Then workthread.ReportProgress(0, "[" & DateTime.Now.ToString("s") & "] DEBUG: SQL " & sqlquery)
            cmd.Connection = con
            con.Open()
            cmd.CommandText = sqlquery
            returnval = cmd.ExecuteScalar
            con.Close()
            con.Dispose()
            cmd.Dispose()
            If Not IsDBNull(returnval) Then Return returnval
        Catch e As Exception
            'exception thrown connecting
            MsgBox(e.Message.ToString)
            workthread.ReportProgress(0, "ERROR: Connection to database threw an exception of: " & vbCrLf & e.Message.ToString & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
            Return 99
        End Try
    End Function

    Private Sub workthread_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles workthread.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            txtdebug.AppendText(vbCrLf & "ERROR: Blockchain scanning thread threw exception : " & e.Error.Message)
            txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Thread exited with error condition.")
            syncicon.Visible = True
            lsyncing.Visible = True
            lsyncing.Text = "Not Synchronized"
            syncicon.Image = My.Resources.redcross
            poversync.Image = My.Resources.redcross
            loversync.Text = "Not Synchronized."
            Exit Sub
        End If

        txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Thread exited.")
        UIrefresh.Enabled = True
        If varsyncronized = True Then
            syncicon.Image = My.Resources.gif
            syncicon.Visible = False
            lsyncing.Visible = True
            lsyncing.Text = "Refreshing UI..."
            poversync.Image = My.Resources.green_tick
            loversync.Text = "Synchronized.  Last block scanned was block " & varsyncblock.ToString & "."
        Else
            syncicon.Visible = True
            syncicon.Image = My.Resources.redcross
            lsyncing.Visible = True
            poversync.Image = My.Resources.redcross
            loversync.Text = "Not Synchronized."
            lsyncing.Text = "Not Synchronized."
        End If
        Application.DoEvents()

        'update addresses
        updateaddresses()

        Dim hrbalmsc As Double = balmsc / 100000000
        Dim hrbaltmsc As Double = baltmsc / 100000000
        Dim hrbalumsc As Double = balumsc / 100000000
        Dim hrbalutmsc As Double = balutmsc / 100000000
        Dim hrbalresmsc As Double = balresmsc / 100000000
        loverviewmscbal.Text = (hrbalmsc + hrbalumsc + hrbalresmsc).ToString("#0.00000000") & " MSC"
        loverviewsmallmscbal.Text = hrbalmsc.ToString("#0.00000000") & " MSC"
        loverviewsmallunconfmsc.Text = hrbalumsc.ToString("#0.00000000") & " MSC"
        loverviewres.Text = (hrbalresmsc).ToString("#0.00000000") & " MSC"

        Try
            dgvaddresses.DataSource = Nothing
            dgvaddresses.Refresh()
            'load addresslist with taddresslist
            addresslist.Clear()
            For Each row In taddresslist.Rows
                addresslist.Rows.Add(row.item(0), row.item(1), row.item(2), row.item(3))
            Next
            dgvaddresses.DataSource = addresslist
            Dim dgvcolumn As New DataGridViewColumn
            dgvcolumn = dgvaddresses.Columns(0)
            dgvcolumn.Width = 370
            dgvcolumn = dgvaddresses.Columns(1)
            'dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn.Width = 130
            dgvcolumn = dgvaddresses.Columns(2)
            'dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn.Width = 130
            dgvcolumn = dgvaddresses.Columns(3)
            'dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######" '"########0.00######"
            dgvcolumn.Width = 130
            If lnkaddsort.Text = "Address Alpha" Then dgvaddresses.Sort(dgvaddresses.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            If lnkaddsort.Text = "BTC Balance" Then dgvaddresses.Sort(dgvaddresses.Columns(1), System.ComponentModel.ListSortDirection.Descending)
            If lnkaddsort.Text = "MSC Balance" Then dgvaddresses.Sort(dgvaddresses.Columns(3), System.ComponentModel.ListSortDirection.Descending)
            If lnkaddfilter.Text = "No Filter Active" Then addresslist.DefaultView.RowFilter = ""
            If lnkaddfilter.Text = "Empty Balances" Then addresslist.DefaultView.RowFilter = "btcamount > 0 or mscamount > 0 or tmscamount > 0"

        Catch ex As Exception
            MsgBox("Addresslist exception" & vbCrLf & ex.Message)
        End Try
        Try
            dgvcurrencies.DataSource = Nothing
            dgvcurrencies.Refresh()
            currencylist.Clear()
            'make address search string
            Dim searchstr As String = ""
            For Each addressrow In addresslist.Rows
                searchstr = searchstr & " OR ADDRESS='" & addressrow(0) & "'"
            Next
            Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf")
            Dim cmd As New SqlCeCommand()
            Dim returnval
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "SELECT MAX(CURTYPE) FROM PROPERTIES WHERE CURTYPE<2147483649"
            Dim maxeco1 = cmd.ExecuteScalar
            cmd.CommandText = "SELECT MAX(CURTYPE) FROM PROPERTIES"
            Dim maxeco2 = cmd.ExecuteScalar
            Dim i As UInteger
            For i = 1 To maxeco1
                cmd.CommandText = "SELECT SUM(CBALANCE) FROM BALANCES WHERE CURTYPE=" & i.ToString & " AND (ADDRESS='FAKE'" & searchstr & ")"
                Dim tempval = cmd.ExecuteScalar
                cmd.CommandText = "SELECT SUM(UBALANCE) FROM BALANCES WHERE CURTYPE=" & i.ToString & " AND (ADDRESS='FAKE'" & searchstr & ")"
                Dim utempval = cmd.ExecuteScalar
                If Not IsDBNull(tempval) Or Not IsDBNull(utempval) Then
                    'get property details
                    cmd.CommandText = "SELECT NAME FROM PROPERTIES WHERE CURTYPE=" & i.ToString
                    Dim propname = cmd.ExecuteScalar
                    cmd.CommandText = "SELECT DIVISIBLE FROM PROPERTIES WHERE CURTYPE=" & i.ToString
                    Dim propdiv = cmd.ExecuteScalar
                    If propdiv = True Then
                        Dim divtempval As Double = tempval / 100000000
                        Dim divutempval As Double = utempval / 100000000
                        currencylist.Rows.Add(propname & " (#" & i.ToString & ")", divtempval.ToString("########0.########"), divutempval.ToString("########0.########"))
                    Else
                        currencylist.Rows.Add(propname & " (#" & i.ToString & ")", tempval.ToString, utempval.ToString)
                    End If
                    If Len(propname) > 20 Then
                        If Not comcurtype.Items.Contains(propname.ToString.Substring(0, 20) & "... (#" & i.ToString & ")") Then comcurtype.Items.Add(propname.ToString.Substring(0, 20) & "... (#" & i.ToString & ")")
                    Else
                        If Not comcurtype.Items.Contains(propname & " (#" & i.ToString & ")") Then comcurtype.Items.Add(propname & " (#" & i.ToString & ")")
                    End If
                Else
                    'no history of transactions in this currency, leave out of balances list
                End If
            Next
            For i = 2147483651 To maxeco2
                cmd.CommandText = "SELECT SUM(CBALANCE) FROM BALANCES WHERE CURTYPE=" & i.ToString & " AND (ADDRESS='FAKE'" & searchstr & ")"
                Dim tempval = cmd.ExecuteScalar
                cmd.CommandText = "SELECT SUM(UBALANCE) FROM BALANCES WHERE CURTYPE=" & i.ToString & " AND (ADDRESS='FAKE'" & searchstr & ")"
                Dim utempval = cmd.ExecuteScalar
                If Not IsDBNull(tempval) Or Not IsDBNull(utempval) Then
                    'get property details
                    cmd.CommandText = "SELECT NAME FROM PROPERTIES WHERE CURTYPE=" & i.ToString
                    Dim propname = cmd.ExecuteScalar
                    cmd.CommandText = "SELECT DIVISIBLE FROM PROPERTIES WHERE CURTYPE=" & i.ToString
                    Dim propdiv = cmd.ExecuteScalar
                    If propdiv = True Then
                        Dim divtempval As Double = tempval / 100000000
                        Dim divutempval As Double = utempval / 100000000
                        currencylist.Rows.Add(propname & " (#" & i.ToString & ")", divtempval.ToString("########0.########"), divutempval.ToString("########0.########"))
                    Else
                        currencylist.Rows.Add(propname & " (#" & i.ToString & ")", tempval.ToString, utempval.ToString)
                    End If
                    If Len(propname) > 20 Then
                        If Not comcurtype.Items.Contains(propname.ToString.Substring(0, 20) & "... (#" & i.ToString & ")") Then comcurtype.Items.Add(propname.ToString.Substring(0, 20) & "... (#" & i.ToString & ")")
                    Else
                        If Not comcurtype.Items.Contains(propname & " (#" & i.ToString & ")") Then comcurtype.Items.Add(propname & " (#" & i.ToString & ")")
                    End If
                Else
                    'no history of transactions in this currency, leave out of balances list
                End If
            Next
            currencylist.Rows.Add("Bitcoin", balbtc.ToString, balubtc.ToString)
            dgvcurrencies.DataSource = currencylist
            Dim dgvcolumn As New DataGridViewColumn
            dgvcolumn = dgvcurrencies.Columns(0)
            dgvcolumn.Width = 398
            dgvcolumn = dgvcurrencies.Columns(1)
            dgvcolumn.Width = 162
            dgvcolumn.DefaultCellStyle.Format = "########0.########"
            dgvcolumn = dgvcurrencies.Columns(2)
            dgvcolumn.DefaultCellStyle.Format = "########0.########"
            dgvcolumn.Width = 130
        Catch ex As Exception
            MsgBox("Currencylist exception" & vbCrLf & ex.Message)
        End Try
        Try
            dgvhistory.DataSource = Nothing
            dgvhistory.Refresh()
            'load historylist with thistorylist
            historylist.Clear()
            For Each row In thistorylist.Rows
                historylist.Rows.Add(row.item(0), row.item(1), row.item(2), row.item(3), row.item(4), row.item(5), row.item(6), row.item(7), row.item(8))
            Next
            dgvhistory.DataSource = historylist
            Dim dgvcolumn As New DataGridViewColumn
            dgvcolumn = dgvhistory.Columns(0)
            dgvcolumn.Width = 14
            dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvcolumn = dgvhistory.Columns(1)
            dgvcolumn.Width = 16
            dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvcolumn = dgvhistory.Columns(2)
            dgvcolumn.Width = 115
            dgvcolumn = dgvhistory.Columns(3)
            dgvcolumn.Width = 185
            dgvcolumn = dgvhistory.Columns(4)
            dgvcolumn.Width = 185
            dgvcolumn = dgvhistory.Columns(5)
            dgvcolumn.Width = 75
            dgvcolumn = dgvhistory.Columns(6)
            dgvcolumn.Width = 94
            dgvcolumn = dgvhistory.Columns(7)
            dgvcolumn.DefaultCellStyle.Format = "########0.########"
            dgvcolumn.Width = 140
            dgvcolumn = dgvhistory.Columns(8)
            dgvcolumn.Width = 0
            If lnkhistorysort.Text = "Highest Value" Then dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Descending)
            If lnkhistorysort.Text = "Lowest Value" Then dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Ascending)
            If lnkhistorysort.Text = "Recent First" Then dgvhistory.Sort(dgvhistory.Columns(2), System.ComponentModel.ListSortDirection.Descending)
        Catch ex As Exception
            MsgBox("historylist exception" & vbCrLf & ex.Message)
        End Try

        dgvopenorders.DataSource = Nothing
        dgvopenorders.Refresh()
        dgvopenorders.DataSource = openorders
        Dim dgvcolumnopen As New DataGridViewColumn
        dgvcolumnopen = dgvopenorders.Columns(0)
        dgvcolumnopen.Width = 0
        dgvcolumnopen.Visible = False
        dgvcolumnopen = dgvopenorders.Columns(1)
        dgvcolumnopen.Width = 58
        dgvcolumnopen = dgvopenorders.Columns(2)
        dgvcolumnopen.Width = 58
        dgvcolumnopen = dgvopenorders.Columns(3)
        dgvcolumnopen.Width = 82
        dgvcolumnopen = dgvopenorders.Columns(4)
        dgvcolumnopen.Width = 82
        dgvcolumnopen = dgvopenorders.Columns(5)
        dgvcolumnopen.Width = 82
        dgvcolumnopen = dgvopenorders.Columns(6)
        dgvcolumnopen.Width = 71
        dgvcolumnopen = dgvopenorders.Columns(7)
        dgvcolumnopen.Width = 82
        dgvcolumnopen = dgvopenorders.Columns(8)
        dgvcolumnopen.Width = 36
        dgvcolumnopen = dgvopenorders.Columns(9)
        dgvcolumnopen.Width = 59
        dgvopenorders.CurrentCell = Nothing
        dgvopenorders.ClearSelection()

        dgvselloffer.DataSource = Nothing
        dgvselloffer.Refresh()
        dgvselloffer.DataSource = selloffers
        Dim dgvcolumnsell As New DataGridViewColumn
        dgvcolumnsell = dgvselloffer.Columns(0)
        dgvcolumnsell.Width = 0
        dgvcolumnsell.Visible = False
        dgvcolumnsell = dgvselloffer.Columns(1)
        dgvcolumnsell.Width = 98
        dgvcolumnsell = dgvselloffer.Columns(2)
        dgvcolumnsell.Width = 98
        dgvcolumnsell = dgvselloffer.Columns(3)
        dgvcolumnsell.Width = 98
        If dexcur = "TMSC" Then
            lbldextotalcur.Text = (baltmsc / 100000000).ToString("######0.00######") & " " & dexcur
            lbldexrescur.Text = (balrestmsc / 100000000).ToString("######0.00######") & " " & dexcur
        End If
        If dexcur = "MSC" Then
            lbldextotalcur.Text = (balmsc / 100000000).ToString("######0.00######") & " " & dexcur
            lbldexrescur.Text = (balresmsc / 100000000).ToString("######0.00######") & " " & dexcur
        End If

        lbldextotalbtc.Text = balbtc.ToString & " BTC"
        dgvselloffer.CurrentCell = Nothing
        dgvselloffer.ClearSelection()
        If dgvhistory.Rows.Count > 0 Then
            dgvhistory.FirstDisplayedScrollingRowIndex = 0
        End If
        dgvhistory.CurrentCell = Nothing
        dgvhistory.ClearSelection()
        If dgvaddresses.Rows.Count > 0 Then
            dgvaddresses.FirstDisplayedScrollingRowIndex = 0
        End If
        dgvaddresses.CurrentCell = Nothing
        dgvaddresses.ClearSelection()
        lnknofocus.Focus()
        updateui()
        lnkdexcurrency.Visible = True
        If varsyncronized = True Then
            lsyncing.Visible = False
            lsyncing.Text = "Synchronizing..."
        End If
        Application.DoEvents()

    End Sub
    Private Sub dgvhistory_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvhistory.CellMouseDown
        If e.Button = MouseButtons.Right Then
            hrow = e.RowIndex
            hcol = e.ColumnIndex
            If e.RowIndex > 0 Then dgvhistory.Rows(e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub dgvaddresses_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvaddresses.CellMouseDown
        If e.Button = MouseButtons.Right Then
            mrow = e.RowIndex
            mcol = e.ColumnIndex
            If e.RowIndex > 0 Then dgvaddresses.Rows(e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub UIrefresh_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UIrefresh.Tick
        UIrefresh.Enabled = False
        poversync.Image = My.Resources.gif
        loversync.Text = "Synchronizing..."
        lsyncing.Visible = True
        lsyncing.Text = "Synchronizing..."
        syncicon.Visible = True
        lsyncing.Visible = True
        Application.DoEvents()
        If workthread.IsBusy <> True Then
            ' Start the workthread for the blockchain scanner
            workthread.RunWorkerAsync()
        End If
        Application.DoEvents()
    End Sub

    Private Sub bupdatesettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bupdatesettings.Click
        If txtrpcserver.Text <> "" And txtrpcport.Text <> "" And txtrpcuser.Text <> "" And txtrpcpassword.Text <> "" And txtrpcserver.Text <> "Not configured." And txtrpcport.Text <> "Not configured." And txtrpcuser.Text <> "Not configured." And txtrpcpassword.Text <> "Not configured." And txtrpcpassword.Text <> "********************" Then
            Dim FINAME As String = Application.StartupPath & "\wallet.cfg"
            If System.IO.File.Exists(FINAME) = True Then
                Dim objWriter As New System.IO.StreamWriter(FINAME)
                objWriter.WriteLine("bitcoinrpcserv=" & txtrpcserver.Text)
                objWriter.WriteLine("bitcoinrpcport=" & txtrpcport.Text)
                objWriter.WriteLine("bitcoinrpcuser=" & txtrpcuser.Text)
                objWriter.WriteLine("bitcoinrpcpass=" & txtrpcpassword.Text)
                objWriter.Close()
                Application.Restart()
            Else
                MsgBox("Configuration file error")
            End If
        Else
            MsgBox("Please complete all fields.")
        End If
    End Sub

    Private Sub txtrpcserver_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcserver.Enter
        txtrpcserver.BorderStyle = BorderStyle.FixedSingle
        If txtrpcserver.Text = "Not configured." Then txtrpcserver.Text = ""
    End Sub
    Private Sub txtrpcserver_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcserver.Leave
        txtrpcserver.BorderStyle = BorderStyle.None
        If txtrpcserver.Text = "" Then txtrpcserver.Text = "Not configured."
    End Sub
    Private Sub txtrpcport_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcport.Enter
        txtrpcport.BorderStyle = BorderStyle.FixedSingle
        If txtrpcport.Text = "Not configured." Then txtrpcport.Text = ""
    End Sub
    Private Sub txtrpcport_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcport.Leave
        txtrpcport.BorderStyle = BorderStyle.None
        If txtrpcport.Text = "" Then txtrpcport.Text = "Not configured."
    End Sub
    Private Sub txtrpcuser_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcuser.Enter
        txtrpcuser.BorderStyle = BorderStyle.FixedSingle
        If txtrpcuser.Text = "Not configured." Then txtrpcuser.Text = ""
    End Sub
    Private Sub txtrpcuser_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcuser.Leave
        txtrpcuser.BorderStyle = BorderStyle.None
        If txtrpcuser.Text = "" Then txtrpcuser.Text = "Not configured."
    End Sub
    Private Sub txtrpcpassword_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcpassword.Enter
        txtrpcpassword.BorderStyle = BorderStyle.FixedSingle
        If txtrpcpassword.Text = "Not configured." Then txtrpcpassword.Text = ""
    End Sub
    Private Sub txtrpcpassword_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcpassword.Leave
        txtrpcpassword.BorderStyle = BorderStyle.None
        If txtrpcpassword.Text = "" Then txtrpcpassword.Text = "Not configured."
    End Sub


    Private Sub lnkaddsort_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkaddsort.LinkClicked
        If lnkaddsort.Text = "Address Alpha" Then
            lnkaddsort.Text = "BTC Balance"
            dgvaddresses.Sort(dgvaddresses.Columns(1), System.ComponentModel.ListSortDirection.Descending)
            Exit Sub
        End If
        If lnkaddsort.Text = "BTC Balance" Then
            lnkaddsort.Text = "MSC Balance"
            dgvaddresses.Sort(dgvaddresses.Columns(3), System.ComponentModel.ListSortDirection.Descending)
            Exit Sub
        End If
        If lnkaddsort.Text = "MSC Balance" Then
            lnkaddsort.Text = "Address Alpha"
            dgvaddresses.Sort(dgvaddresses.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            Exit Sub
        End If
    End Sub

    Private Sub lnkaddfilter_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkaddfilter.LinkClicked
        If lnkaddfilter.Text = "No Filter Active" Then
            lnkaddfilter.Text = "Empty Balances"
            addresslist.DefaultView.RowFilter = "btcamount > 0 or mscamount > 0 or tmscamount > 0"
            Exit Sub
        End If
        If lnkaddfilter.Text = "Empty Balances" Then
            lnkaddfilter.Text = "No Filter Active"
            addresslist.DefaultView.RowFilter = ""
            Exit Sub
        End If
    End Sub

    Public Sub lcontinue()
        'check passphrase against db
        'walpass = txtwalpass.Text
        'test connection to database
        Dim testval As Integer
        testval = SQLGetSingleVal("SELECT count(*) FROM information_schema.columns WHERE table_name = 'processedblocks'")
        If testval = 2 Then 'sanity check ok
        Else
            'something has gone wrong
            lwelstartup.Visible = True
            Exit Sub
        End If
        lwelstartup.Visible = False

        'do some initial setup
        showlabels()
        bback.Visible = True
        txtdebug.Text = "MASTERCHEST WALLET v0.4a"
        activateoverview()
        lastscreen = "1"
        updateui()
        Me.Refresh()
        Application.DoEvents()
        'kick off the background worker thread
        If workthread.IsBusy <> True Then
            workthread.RunWorkerAsync()
        End If
    End Sub

    Private Sub txtwalpass_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            lcontinue()
        End If
    End Sub

    Private Sub txtwalpass_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        lwelstartup.Visible = False
    End Sub

    Private Sub btest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btest.Click
        Try
            If txtstartsrv.Text <> "" And txtstartport.Text <> "" And txtstartuser.Text <> "" And txtstartpass.Text <> "" Then
                bitcoin_con.bitcoinrpcserver = txtstartsrv.Text
                bitcoin_con.bitcoinrpcport = Val(txtstartport.Text)
                bitcoin_con.bitcoinrpcuser = txtstartuser.Text
                bitcoin_con.bitcoinrpcpassword = txtstartpass.Text
                'test connection to bitcoind
                Try
                    Dim checkhash As blockhash = mlib.getblockhash(bitcoin_con, 2)
                    If checkhash.result.ToString = "000000006a625f06636b8bb6ac7b960a8d03705d1ace08b1a19da3fdcc99ddbd" Then 'we've got a correct response
                        'also check for txindex
                        Try
                            Dim testtxn As txn = mlib.gettransaction(bitcoin_con, "4aa9f31f798ab1bde53b232b1039ee512f241dc24946ce990272d85fbd765b64")
                            If testtxn.result.txid <> "4aa9f31f798ab1bde53b232b1039ee512f241dc24946ce990272d85fbd765b64" Then
                                ltestinfo.Text = "Bitcoin connection OK but transaction index appears disabled."
                                ltestinfo.ForeColor = Color.Red
                                Exit Sub
                            End If
                        Catch
                            ltestinfo.Text = "Bitcoin connection OK but transaction index appears disabled."
                            ltestinfo.ForeColor = Color.Red
                            Exit Sub
                        End Try
                        ltestinfo.Text = "Bitcoin connection OK and transaction index appears enabled."
                        ltestinfo.ForeColor = Color.Lime
                    Else
                        'something has gone wrong
                        ltestinfo.Text = "ERROR: Connection to bitcoin RPC seems to be established but responses are not as expected."
                        Exit Sub
                    End If
                Catch
                    Exit Sub
                End Try
            Else
                ltestinfo.Text = "Please complete all fields"
            End If
        Catch ex As Exception
            MsgBox("Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub bfinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bfinish.Click
        If txtstartsrv.Text <> "" And txtstartport.Text <> "" And txtstartuser.Text <> "" And txtstartpass.Text <> "" And (Len(txtstartwalpass.Text) > 11) Then
            bfinish.Enabled = False
            btest.Enabled = False
            bitcoin_con.bitcoinrpcserver = txtstartsrv.Text
            bitcoin_con.bitcoinrpcport = Val(txtstartport.Text)
            bitcoin_con.bitcoinrpcuser = txtstartuser.Text
            bitcoin_con.bitcoinrpcpassword = txtstartpass.Text
            'test connection to bitcoind
            Try
                Dim checkhash As blockhash = mlib.getblockhash(bitcoin_con, 2)
                If checkhash.result.ToString = "000000006a625f06636b8bb6ac7b960a8d03705d1ace08b1a19da3fdcc99ddbd" Then 'we've got a correct response
                    'also check for txindex
                    Try
                        Dim testtxn As txn = mlib.gettransaction(bitcoin_con, "4aa9f31f798ab1bde53b232b1039ee512f241dc24946ce990272d85fbd765b64")
                        If testtxn.result.txid <> "4aa9f31f798ab1bde53b232b1039ee512f241dc24946ce990272d85fbd765b64" Then
                            ltestinfo.Text = "Bitcoin connection OK but transaction index appears disabled."
                            ltestinfo.ForeColor = Color.Red
                            Exit Sub
                        End If
                    Catch
                        ltestinfo.Text = "Bitcoin connection OK but transaction index appears disabled."
                        ltestinfo.ForeColor = Color.Red
                        Exit Sub
                    End Try
                    ltestinfo.Text = "Bitcoin connection OK and transaction index is enabled."
                    ltestinfo.ForeColor = Color.Lime
                    walpass = txtstartwalpass.Text
                    'see if wallet.sdf exists, if not create database (either blank or preseed)
                    Dim constr As String = "datasource=" & Application.StartupPath & "\wallet.sdf; password=" & walpass
                    Dim con As New SqlCeConnection(constr)
                    If System.IO.File.Exists(con.Database) Then
                    Else
                        If chkpreseed.Checked = True Then
                            My.Computer.FileSystem.RenameFile(Application.StartupPath & "\preseed_1.sdf", "wallet.sdf")
                        Else
                            My.Computer.FileSystem.RenameFile(Application.StartupPath & "\blank_1.sdf", "wallet.sdf")
                        End If
                    End If
                    'add password
                    Dim engine As New SqlCeEngine("data source=" & Application.StartupPath & "\wallet.sdf")
                    engine.Compact("data source=; password=" & walpass)
                    'write config
                    Dim FINAME As String = Application.StartupPath & "\wallet.cfg"
                    If System.IO.File.Exists(FINAME) = True Then
                        Dim objWriter As New System.IO.StreamWriter(FINAME)
                        objWriter.WriteLine("bitcoinrpcserv=" & txtstartsrv.Text)
                        objWriter.WriteLine("bitcoinrpcport=" & txtstartport.Text)
                        objWriter.WriteLine("bitcoinrpcuser=" & txtstartuser.Text)
                        objWriter.WriteLine("bitcoinrpcpass=" & txtstartpass.Text)
                        txtrpcserver.Text = bitcoin_con.bitcoinrpcserver
                        txtrpcport.Text = bitcoin_con.bitcoinrpcport.ToString
                        txtrpcuser.Text = bitcoin_con.bitcoinrpcuser
                        txtrpcpassword.Text = "********************"

                        objWriter.Close()
                        'Application.Restart()
                    Else
                        MsgBox("Configuration file error")
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                ltestinfo.Text = "Failed to connect to Bitcoin or could not locate seed wallet."
                ltestinfo.ForeColor = Color.Red
                bfinish.Enabled = True
                btest.Enabled = True
                MsgBox(ex.Message)
                Exit Sub
            End Try
        Else
            ltestinfo.Text = "Please complete all fields"
            bfinish.Enabled = True
            btest.Enabled = True
            Exit Sub
        End If
        bfinish.Enabled = True
        btest.Enabled = True
        hidepanels()
        pwelcome.Visible = True
    End Sub

    Private Sub txtstartwalpass_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtstartwalpass.TextChanged
        If Len(txtstartwalpass.Text) < 6 Then
            lwalinfo.Text = "More please... "
            lwalinfo.ForeColor = Color.FromArgb(255, 192, 128)
            Exit Sub
        End If
        If Len(txtstartwalpass.Text) < 12 Then
            lwalinfo.Text = "More please... More please..."
            lwalinfo.ForeColor = Color.FromArgb(255, 192, 128)
            Exit Sub
        End If
        If Len(txtstartwalpass.Text) > 11 Then
            lwalinfo.Text = "Great, thanks"
            lwalinfo.ForeColor = Color.Lime
            Exit Sub
        End If
    End Sub

    Private Sub updateaddresses()
        'update addresses
        Dim baltype As Integer = 0
        Dim balstr As String = ""
        If comcurtype.Text <> "" Then
            Dim loc As Integer = InStr(comcurtype.Text, "#")
            Dim curtype As UInteger = Val(comcurtype.Text.Substring(loc, Len(comcurtype.Text) - (loc + 1)))
            comsendaddress.Items.Clear()
            comsendaddress.Text = ""
            Dim searchstr As String = ""
            For Each addressrow In addresslist.Rows
                searchstr = searchstr & " OR ADDRESS='" & addressrow(0) & "'"
            Next
            Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf")
            Dim cmd As New SqlCeCommand()
            Dim returnval
            cmd.Connection = con
            con.Open()
            If curtype = 1 Then balstr = "MSC"
            If curtype = 2 Then balstr = "TMSC"
            If curtype > 2 Then balstr = "SPT"
            cmd.CommandText = "SELECT DIVISIBLE FROM PROPERTIES WHERE CURTYPE=" & curtype.ToString
            Dim propdiv = cmd.ExecuteScalar
            If propdiv = True Then lcurdiv.Text = "Divisible currency"
            If propdiv = False Then lcurdiv.Text = "Indivisible tokens"
            Dim dummy As New EventArgs
            txtsendamount_Leave(vbNull, dummy)
            cmd.CommandText = "SELECT address,cbalance FROM balances where curtype=" & curtype.ToString & " AND (ADDRESS='FAKEADD'" & searchstr & ")"
            Dim adptSQL As New SqlCeDataAdapter(cmd)
            Dim ds1 As New DataSet()
            adptSQL.Fill(ds1)
            With ds1.Tables(0)
                For rowNumber As Integer = 0 To .Rows.Count - 1
                    With .Rows(rowNumber)
                        If Not comsendaddress.Items.Contains(.Item(0).ToString) Then
                            If .Item(1) = 0 Then
                                'ignore empty address
                            Else
                                If propdiv = True Then
                                    Dim divtempval As Double = .Item(1) / 100000000
                                    comsendaddress.Items.Add(.Item(0).ToString & "     (" & divtempval.ToString("######0.00######") & " " & balstr & ")")
                                Else
                                    comsendaddress.Items.Add(.Item(0).ToString & "     (" & .Item(1).ToString & " " & balstr & ")")
                                End If
                            End If
                        End If
                    End With
                Next
            End With
        End If
    End Sub



    Private Sub bback_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bback.MouseUp

    End Sub

    Private Sub lnkhistorysort_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkhistorysort.LinkClicked
        If lnkhistorysort.Text = "Recent First" Then
            lnkhistorysort.Text = "Highest Value"
            dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Descending)
            Exit Sub
        End If
        If lnkhistorysort.Text = "Highest Value" Then
            lnkhistorysort.Text = "Lowest Value"
            dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Ascending)
            Exit Sub
        End If
        If lnkhistorysort.Text = "Lowest Value" Then
            lnkhistorysort.Text = "Recent First"
            dgvhistory.Sort(dgvhistory.Columns(2), System.ComponentModel.ListSortDirection.Descending)
            Exit Sub
        End If
    End Sub

    Private Sub lnkhistoryfilter_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkhistoryfilter.LinkClicked
        If lnkhistoryfilter.Text = "No Filter Active" Then
            lnkhistoryfilter.Text = "Mastercoin Only"
            Exit Sub
        End If
        If lnkhistoryfilter.Text = "Mastercoin Only" Then
            lnkhistoryfilter.Text = "No Filter Active"
            Exit Sub
        End If
    End Sub

    Private Sub bsignsend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsignsend.Click
        If workthread.IsBusy = True Then
            MsgBox("The background processing thread is currently modifying the state.  Please send your transaction when processing has finished.")
            Exit Sub
        End If
        If Val(txtsendamount.Text) > 0 And Val(txtsendamount.Text) > avail Then
            lsendamver.Text = "Insufficient Funds"
            lsendamver.ForeColor = Color.FromArgb(255, 192, 128)
            Exit Sub
        End If
        If Val(txtsendamount.Text) = 0 Then
            'nothing to send
            Exit Sub
        End If
        txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Beginning simple send transaction")
        txtdebug.AppendText(vbCrLf & "===================================================================================")
        txsummary = ""
        senttxid = "Transaction not sent"
        'first validate recipient address
        If txtsenddest.Text <> "" Then
            txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Requesting passphrase")
            'get wallet passphrase
            passfrm.ShowDialog()
            'get addresses
            Dim fromadd As String
            fromadd = comsendaddress.Text.Substring(0, comsendaddress.Text.IndexOf(" "))
            Dim toadd As String = Trim(txtsenddest.Text)
            'get currency
            Dim loc As Integer = InStr(comcurtype.Text, "#")
            Dim curtype As UInteger = Val(comcurtype.Text.Substring(loc, Len(comcurtype.Text) - (loc + 1)))
            'get divisibility
            Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf")
            Dim cmd As New SqlCeCommand()
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "SELECT DIVISIBLE FROM PROPERTIES WHERE CURTYPE=" & curtype.ToString
            Dim propdiv = cmd.ExecuteScalar
            'get amount
            Dim amount As Double = Val(txtsendamount.Text)
            Dim amountlong As ULong = 0
            If propdiv = True Then
                amountlong = amount * 100000000
            Else
                amountlong = amount
            End If

            Try
                txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Validating recipient address")
                Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, txtsenddest.Text, 0, 0))
                If validater.result.isvalid = True Then 'address is valid
                    txsummary = "Recipient address is valid."
                    'push out to masterchest lib to encode the tx
                    txtdebug.AppendText(vbCrLf & DateTime.Now.ToString("s") & "] DEBUG: Calling library: mlib.encodetx, bitcoin_con, " & fromadd & ", " & toadd & ", " & curtype & ", " & amountlong.ToString)
                    Dim rawtx As String = mlib.encodetx(bitcoin_con, fromadd, toadd, curtype, amountlong)
                    'is rawtx empty
                    If rawtx = "" Then
                        txtdebug.AppendText(vbCrLf & "ERROR: Raw transaction is empty - stopping")
                        txtdebug.AppendText(vbCrLf & "===================================================================================")
                        txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending send payment transaction")
                        Exit Sub
                    End If
                    'decode the tx in the viewer
                    txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Raw transaction hex: " & vbCrLf & rawtx)
                    txsummary = txsummary & vbCrLf & "Raw transaction hex:" & vbCrLf & rawtx & vbCrLf & "Raw transaction decode:" & vbCrLf & mlib.rpccall(bitcoin_con, "decoderawtransaction", 1, rawtx, 0, 0)
                    'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
                    txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Unlocking wallet")
                    If btcpass = "" Then 'skip unlocking wallet
                        txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: No passphrase specified, skipping unlocking wallet")
                    Else
                        Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
                        Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(btcpass.ToString), 15, 0)
                    End If
                    btcpass = ""
                    'try and sign/broadcast transaction
                    txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Attempting signing")
                    Dim signedtxn As signedtx = JsonConvert.DeserializeObject(Of signedtx)(mlib.rpccall(bitcoin_con, "signrawtransaction", 1, rawtx, 0, 0))
                    If signedtxn.result.complete = True Then
                        txsummary = txsummary & vbCrLf & "Signing appears successful."
                        txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Attempting broadcast")
                        Dim broadcasttx As broadcasttx = JsonConvert.DeserializeObject(Of broadcasttx)(mlib.rpccall(bitcoin_con, "sendrawtransaction", 1, signedtxn.result.hex, 0, 0))
                        If broadcasttx.result <> "" Then
                            txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Transaction sent - " & broadcasttx.result.ToString)
                            txtdebug.AppendText(vbCrLf & "===================================================================================")
                            txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending simple send transaction")
                            txsummary = txsummary & vbCrLf & "Transaction sent, ID: " & broadcasttx.result.ToString
                            sentfrm.lsent.Text = "transaction sent"
                            senttxid = broadcasttx.result.ToString
                            If lcurdiv.Text = "Divisible currency" Then
                                txtsendamount.Text = "0.00000000"
                            Else
                                txtsendamount.Text = "0"
                            End If
                            sentfrm.txtviewer.Text = ""
                            lsendamver.Visible = False
                            txtsenddest.Text = ""
                            lsendtxinfo.Text = ""
                            bsignsend.Enabled = True
                            Application.DoEvents()
                            If workthread.IsBusy <> True Then
                                UIrefresh.Enabled = False
                                syncicon.Visible = True
                                comsendaddress.Text = ""
                                lsyncing.Visible = True
                                poversync.Image = My.Resources.gif
                                loversync.Text = "Synchronizing..."
                                lsyncing.Text = "Synchronizing..."
                                ' Start the workthread for the blockchain scanner
                                workthread.RunWorkerAsync()
                            End If
                            Application.DoEvents()
                            sentfrm.ShowDialog()
                            Exit Sub
                        Else
                            txsummary = txsummary & vbCrLf & "Error sending transaction."
                            sentfrm.ShowDialog()
                            lsendtxinfo.Text = "Error sending transaction."
                            lsendtxinfo.ForeColor = Color.FromArgb(255, 192, 128)
                            txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] ERROR: Unknown error sending transaction")
                            txtdebug.AppendText(vbCrLf & "===================================================================================")
                            txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending simple send transaction")
                            Exit Sub
                        End If
                    Else
                        txsummary = txsummary & vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct."
                        txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] ERROR: Failed to sign transaction.  Ensure wallet passphrase is correct")
                        txtdebug.AppendText(vbCrLf & "===================================================================================")
                        txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending simple send transaction")
                        sentfrm.lsent.Text = "transaction failed"
                        sentfrm.ShowDialog()
                        Exit Sub
                    End If
                Else
                    txsummary = "Build transaction failed.  Recipient address is not valid."
                    txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] ERROR: Build transaction failed.  Recipient address is not valid")
                    txtdebug.AppendText(vbCrLf & "===================================================================================")
                    txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending simple send transaction")
                    sentfrm.lsent.Text = "transaction failed"
                    sentfrm.ShowDialog()
                    Exit Sub
                End If
                sentfrm.ShowDialog()
            Catch ex As Exception
                MsgBox("Exeption thrown : " & ex.Message)
                txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] ERROR: Exception thrown: " & ex.Message)
                txtdebug.AppendText(vbCrLf & "===================================================================================")
                txtdebug.AppendText(vbCrLf & "[" & DateTime.Now.ToString("s") & "] DEBUG: Ending simple send transaction")
                sentfrm.ShowDialog()
            End Try
            'End If

        End If
    End Sub

    Private Sub dgvhistory_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvhistory.DataError
        'trap error when we clear list bound to dgv temporarily
    End Sub

    Private Sub dgvaddresses_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvaddresses.DataError
        'trap error when we clear list bound to dgv temporarily
    End Sub

    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        teststartup()

    End Sub




    Private Sub txtbtcpass_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub



    Private Sub DataGridView2_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub

    Private Sub DataGridView2_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        'DataGridView2.Refresh()
    End Sub

    Private Sub DataGridView2_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'DataGridView2.Refresh()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        sentfrm.ShowDialog()
    End Sub

    Private Sub bbuy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bbuy.Click
        sellrefadd = ""
        Try
            'get currently highlighted sell
            Dim selected = dgvselloffer.SelectedRows
            For Each row In selected 'should only ever be one
                sellrefadd = row.cells(0).value.ToString
            Next
            If sellrefadd <> "" Then
                buyfrm.buyfrminit()
                buyfrm.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("Exception caught attempting to show buy dialog")
        End Try
    End Sub

    Private Sub bsell_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsell.Click
        sellfrm.sellfrminit()
        sellfrm.ShowDialog()
    End Sub

    Private Sub dgvselloffer_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvselloffer.DataError
        'trap error when we clear list bound to dgv temporarily
    End Sub

    Private Sub dgvselloffer_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvselloffer.SelectionChanged
        'get currently highlighted sell
        Dim selected = dgvselloffer.SelectedRows
        If selected.Count = 1 Then
            bbuy.ForeColor = Color.FromArgb(209, 209, 209)
        Else
            bbuy.ForeColor = Color.FromArgb(100, 100, 100)
        End If
    End Sub
    Private Sub dgvopenorders_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvopenorders.CellMouseClick

    End Sub

    Private Sub dgvopenorders_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvopenorders.CellMouseDown

    End Sub



    Private Sub pexchange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pexchange.Click
        dgvselloffer.ClearSelection()
        dgvopenorders.ClearSelection()
    End Sub

    Private Sub sendpay_click(ByVal sender As Object, ByVal e As EventArgs)
        paybuytxid = ""
        paybuytxid = dgvopenorders.SelectedRows.Item(0).Cells(0).Value.ToString
        paybuyfrm.paybuyfrminit()
        paybuyfrm.ShowDialog()
    End Sub

    Private Sub cancel_click(ByVal sender As Object, ByVal e As EventArgs)
        sellcancelfrm.lselladdress.Text = dgvopenorders.SelectedRows.Item(0).Cells(1).Value.ToString
        sellcancelfrm.ShowDialog()
    End Sub

    Private Sub dgvopenorders_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvopenorders.DataError
        'trap error when we clear list bound to dgv temporarily
    End Sub


    Private Sub dgvopenorders_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvopenorders.MouseDown
        If e.Button = MouseButtons.Right Then
            Try
                Dim ht As DataGridView.HitTestInfo
                ht = Me.dgvopenorders.HitTest(e.X, e.Y)
                If ht.Type = DataGridViewHitTestType.Cell Then
                    If Me.dgvopenorders.Rows(0).Cells(1).Value.ToString = "N/A" Then
                        dgvopenorders.ClearSelection()
                        Exit Sub
                    End If
                    'MsgBox(ht.RowIndex)
                    Me.dgvopenorders.Rows(ht.RowIndex).Selected = True
                    Dim mnusell As New ContextMenuStrip
                    Dim mnusendpay As New ToolStripMenuItem("Send Payment")
                    Dim mnucancel As New ToolStripMenuItem("Cancel")
                    AddHandler mnusendpay.Click, AddressOf sendpay_click
                    AddHandler mnucancel.Click, AddressOf cancel_click
                    mnusell.Items.AddRange(New ToolStripItem() {mnusendpay})
                    mnusell.Items.AddRange(New ToolStripItem() {mnucancel})
                    Dim tmptype As String = dgvopenorders.Rows(ht.RowIndex).Cells(8).Value.ToString
                    If tmptype = "Sell" Then
                        mnusell.Items(0).Enabled = False
                        mnusell.Items(1).Enabled = True
                    End If
                    If tmptype = "Buy" Then
                        mnusell.Items(0).Enabled = True
                        mnusell.Items(1).Enabled = False
                    End If
                    Dim tmppending As String = dgvopenorders.Rows(ht.RowIndex).Cells(9).Value.ToString
                    If tmppending = "Pending" Then
                        mnusell.Items(0).Enabled = False
                        mnusell.Items(1).Enabled = False
                    End If
                    dgvopenorders.ContextMenuStrip = mnusell
                Else
                    dgvopenorders.ClearSelection()
                    dgvopenorders.ContextMenuStrip = Nothing
                End If
            Catch ex As Exception
                MsgBox("DGV Open Orders Exception")
            End Try
        End If
        If e.Button = MouseButtons.Left Then
            Try
                Dim ht As DataGridView.HitTestInfo
                ht = Me.dgvopenorders.HitTest(e.X, e.Y)
                If ht.Type = DataGridViewHitTestType.Cell Then
                    If Me.dgvopenorders.Rows(0).Cells(1).Value.ToString = "N/A" Then
                        dgvopenorders.ClearSelection()
                        Exit Sub
                    End If
                    Me.dgvopenorders.Rows(ht.RowIndex).Selected = True
                Else
                    Me.dgvopenorders.ClearSelection()
                End If
            Catch exc As Exception
                MsgBox("DGV Open Orders Exception")
            End Try
        End If
    End Sub

    Private Sub lnkdexcurrency_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkdexcurrency.LinkClicked
        If lnkdexcurrency.Text = "Test Mastercoin" Then
            lnkdexcurrency.Text = "Mastercoin"
            dexcur = "MSC"
        Else
            lnkdexcurrency.Text = "Test Mastercoin"
            dexcur = "TMSC"
        End If
        'reinit based on currency change
        refreshdex()
        If dexcur = "TMSC" Then
            lbldextotalcur.Text = (baltmsc / 100000000).ToString("######0.00######") & " " & dexcur
            lbldexrescur.Text = (balrestmsc / 100000000).ToString("######0.00######") & " " & dexcur
        End If
        If dexcur = "MSC" Then
            lbldextotalcur.Text = (balmsc / 100000000).ToString("######0.00######") & " " & dexcur
            lbldexrescur.Text = (balresmsc / 100000000).ToString("######0.00######") & " " & dexcur
        End If
        dgvselloffer.ClearSelection()
        dgvopenorders.ClearSelection()
        Application.DoEvents()
    End Sub

    Private Sub lnksyncnow_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnksyncnow.LinkClicked
        If workthread.IsBusy <> True Then
            poversync.Image = My.Resources.gif
            syncicon.Image = My.Resources.gif
            syncicon.Visible = True
            lsyncing.Visible = True
            lsyncing.Text = "Synchronizing..."
            workthread.RunWorkerAsync()
        End If
    End Sub

    Private Sub lnksup_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnksup.LinkClicked
        Dim sinfo As New ProcessStartInfo("https://masterchest.info/support")
        Process.Start(sinfo)
    End Sub

    Private Sub RectangleShape1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangleShape1.Click

    End Sub

    Private Sub nfi_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles nfi.DoubleClick
        nfi.Visible = False
        Me.Visible = True
        Me.WindowState = FormWindowState.Normal
    End Sub


    Private Sub pproperties_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pproperties.Paint

    End Sub

    Private Sub bspsearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bspsearch.Click
        Dim curtype As Long = 0
        Try
            curtype = Val(txtsp.Text)
        Catch ex As Exception
            Exit Sub
        End Try
        If curtype = 0 Or curtype > 5000000000 Then Exit Sub
        Dim spcon As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        Dim cmd As New SqlCeCommand()
        cmd.Connection = spcon
        spcon.Open()
        cmd.CommandText = "SELECT * FROM PROPERTIES WHERE CURTYPE=" & curtype
        Dim adptSQLsp As New SqlCeDataAdapter(cmd)
        Dim dssp As New DataSet()
        adptSQLsp.Fill(dssp)
        If dssp.Tables(0).Rows.Count = 1 Then 'should only ever be one or zero rows
            With dssp.Tables(0).Rows(0)
                Dim propname = .Item(2)
                Dim propcat = .Item(3) + " > " + .Item(4)
                Dim propdata = .Item(5)
                Dim propurl = .Item(6)
                Dim propdiv = .Item(7)

                lpropnamed.Text = propname + " (#" + curtype.ToString + ")"
                lpropcatd.Text = propcat
                lpropdatad.Text = propdata
                lnkpropurld.Text = propurl
                If propdiv = True Then
                    lproptyped.Text = "Divisible currency"
                Else
                    lproptyped.Text = "Indivisible tokens"
                End If
                cmd.CommandText = "Select sum(cbalance) from balances where curtype=" & curtype
                Dim returnval = cmd.ExecuteScalar()
                If Not IsDBNull(returnval) Then
                    Dim lamount As String
                    Try
                        If propdiv = False Then
                            lamount = returnval.ToString
                        Else
                            Dim balance0 As Double
                            balance0 = returnval / 100000000
                            lamount = balance0.ToString("######0.########")
                        End If
                        If curtype = 1 Then lamount = lamount & " MSC"
                        If curtype = 2 Then lamount = lamount & " TMSC"
                        If curtype > 2 Then lamount = lamount & " SPT"
                        lpropsupplyd.Text = lamount
                    Catch ex As Exception
                        lpropsupplyd.Text = "Exception"
                    End Try
                Else
                    lpropsupplyd.Text = "0 SPT"
                End If
                cmd.CommandText = "Select blocktime from transactions_processed where (type='spcreatevar' or type='spcreatefixed') and curtype=" & curtype
                returnval = cmd.ExecuteScalar()
                If curtype = 1 Then returnval = 1375332164
                If curtype = 2 Then returnval = 1375332164
                Dim btdt2 As DateTime = DateAdd(DateInterval.Second, Val(returnval), New DateTime(1970, 1, 1, 0, 0, 0))
                lpropcreatedd.Text = btdt2.ToString

                cmd.CommandText = "select deadline from fundraisers where curtype=" & curtype
                returnval = cmd.ExecuteScalar()
                Dim btdt5l As String
                If returnval > 2470000000 Then
                    btdt5l = "post 2038"
                Else
                    Dim btdt5 As DateTime = DateAdd(DateInterval.Second, returnval, New DateTime(1970, 1, 1, 0, 0, 0))
                    btdt5l = btdt5.ToString
                End If
                cmd.CommandText = "select blocktime from transactions_processed where type='spcancelvar' and valid=1 and curtype=" & curtype.ToString
                returnval = cmd.ExecuteScalar()
                Dim forceclosed As Boolean = False
                If returnval > 0 Then
                    Dim btdt6 As DateTime = DateAdd(DateInterval.Second, returnval, New DateTime(1970, 1, 1, 0, 0, 0))
                    btdt5l = btdt6.ToString
                    forceclosed = True
                End If

                cmd.CommandText = "Select txid from fundraisers where curtype=" & curtype.ToString
                returnval = cmd.ExecuteScalar()
                If returnval <> "" Then
                    cmd.CommandText = "Select active from fundraisers where curtype=" & curtype.ToString
                    returnval = cmd.ExecuteScalar()
                    If returnval = True Then
                        lpropcrowdsaled.Text = "Active (expires " + btdt5l + ")"
                    Else
                        lpropcrowdsaled.Text = "Ended (expired " + btdt5l + ")"
                    End If
                    If forceclosed = True Then lpropcrowdsaled.Text = "Ended (closed early " + btdt5l + ")"
                    cmd.CommandText = "Select amountperunit from fundraisers where curtype=" & curtype.ToString
                    returnval = cmd.ExecuteScalar()
                    Dim amperunit = returnval
                    Dim pamount As String
                    If propdiv = False Then
                        pamount = amperunit.ToString
                    Else
                        Dim balance0g As Double
                        balance0g = amperunit / 100000000
                        pamount = balance0g.ToString("######0.########")
                    End If
                    lpropcrowdtermsd.Text = pamount.ToString + " tokens per unit invested"
                    cmd.CommandText = "select earlybird from fundraisers where curtype=" & curtype.ToString
                    returnval = cmd.ExecuteScalar()
                    lpropcrowdearlyd.Text = returnval.ToString + "% per week"
                    cmd.CommandText = "select issuerpercent from fundraisers where curtype=" & curtype.ToString
                    returnval = cmd.ExecuteScalar()
                    lpropcrowdissuerd.Text = returnval.ToString + "% to issuer"
                    cmd.CommandText = "select curdesired from fundraisers where curtype=" & curtype.ToString
                    returnval = cmd.ExecuteScalar()
                    Dim curdesnum = returnval
                    cmd.CommandText = "select name from properties where curtype=" & curdesnum.ToString
                    returnval = cmd.ExecuteScalar()
                    lpropcrowdcurd.Text = returnval + " (#" + curdesnum.ToString + ")"
                    lpropcrowdcur.Visible = True
                    lpropcrowdearly.Visible = True
                    lpropcrowdissuer.Visible = True
                    lpropcrowdterms.Visible = True
                    lpropcrowdcurd.Visible = True
                    lpropcrowdearlyd.Visible = True
                    lpropcrowdissuerd.Visible = True
                    lpropcrowdtermsd.Visible = True
                Else
                    lpropcrowdsaled.Visible = False
                    lpropcrowdcur.Visible = False
                    lpropcrowdearly.Visible = False
                    lpropcrowdissuer.Visible = False
                    lpropcrowdterms.Visible = False
                    lpropcrowdcurd.Visible = False
                    lpropcrowdearlyd.Visible = False
                    lpropcrowdissuerd.Visible = False
                    lpropcrowdtermsd.Visible = False
                    lpropcrowdsaled.Text = "None (fixed issuance)"
                End If
                lpropname.Visible = True
                lcat.Visible = True
                lpropdata.Visible = True
                lpropurl.Visible = True
                lproptype.Visible = True
                lpropsupply.Visible = True
                lpropcreated.Visible = True
                lpropcrowdsale.Visible = True
                lpropnamed.Visible = True
                lpropcatd.Visible = True
                lpropdatad.Visible = True
                lnkpropurld.Visible = True
                lproptyped.Visible = True
                lpropsupplyd.Visible = True
                lpropcreatedd.Visible = True
                lpropcrowdsaled.Visible = True
                lnoprop.Visible = False
            End With
        Else
            lpropname.Visible = False
            lcat.Visible = False
            lpropdata.Visible = False
            lpropurl.Visible = False
            lproptype.Visible = False
            lpropsupply.Visible = False
            lpropcreated.Visible = False
            lpropcrowdsale.Visible = False
            lpropnamed.Visible = False
            lpropcatd.Visible = False
            lpropdatad.Visible = False
            lnkpropurld.Visible = False
            lproptyped.Visible = False
            lpropsupplyd.Visible = False
            lpropcreatedd.Visible = False
            lpropcrowdsaled.Visible = False
            lpropcrowdcur.Visible = False
            lpropcrowdearly.Visible = False
            lpropcrowdissuer.Visible = False
            lpropcrowdterms.Visible = False
            lpropcrowdcurd.Visible = False
            lpropcrowdearlyd.Visible = False
            lpropcrowdissuerd.Visible = False
            lpropcrowdtermsd.Visible = False
            lnoprop.Visible = True


        End If
    End Sub


    Private Sub comcurtype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comcurtype.SelectedIndexChanged
        updateaddresses()
        If Val(txtsendamount.Text) > avail Then
            lsendamver.Visible = True
        Else
            lsendamver.Visible = False
        End If
    End Sub

    Private Sub comsendaddress_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comsendaddress.SelectedIndexChanged
        If comsendaddress.Text <> "" Then
            'check funds
            Dim loc As Integer = InStr(comsendaddress.Text, "(")
            Dim am As String = comsendaddress.Text.Substring(loc, (Len(comsendaddress.Text) - (loc + 5)))
            avail = Val(am)
            If Val(txtsendamount.Text) > avail Then
                lsendamver.Visible = True
            Else
                lsendamver.Visible = False
            End If
            'check fees
            Dim loc2 As Integer = InStr(comsendaddress.Text, " ")
            Dim add As String = comsendaddress.Text.Substring(0, loc2 - 1)
            For i = 0 To addresslist.Rows.Count - 1
                If addresslist.Rows(i).Item(0) = add Then 'found address in list, check enough BTC for fees
                    If addresslist.Rows(i).Item(1) < 0.0004 Then
                        lnofees.Visible = True
                    Else
                        lnofees.Visible = False
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub txtsendamount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtsendamount.TextChanged
        Try
            If Val(txtsendamount.Text) > avail Then
                lsendamver.Visible = True
            Else
                lsendamver.Visible = False
            End If
        Catch ex As Exception
            MsgBox("exception")
            lsendamver.Visible = True
        End Try
    End Sub

    Private Sub lnkpropurld_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkpropurld.LinkClicked
        Dim useresponse = MsgBox("You are about to visit an external website (the following URL will be automatically opened in your browser: http://" + lnkpropurld.Text + ")" + vbCrLf + vbCrLf + "Do you wish to continue?", MsgBoxStyle.YesNo)
        If useresponse = vbYes Then
            Dim sinfo As New ProcessStartInfo("http://" + lnkpropurld.Text)
            Process.Start(sinfo)
        End If
    End Sub
End Class

