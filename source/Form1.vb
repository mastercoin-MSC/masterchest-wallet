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
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown, RectangleShape1.MouseDown, psetup.MouseDown, pwelcome.MouseDown, pcurrencies.MouseDown, paddresses.MouseDown, pdebug.MouseDown, poverview.MouseDown, psend.MouseDown, psettings.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If

    End Sub
    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        Application.Exit()
    End Sub
    Private Sub bmin_Click(sender As System.Object, e As System.EventArgs) Handles bmin.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub


    '//////////////
    '////INITIALIZE
    '//////////////
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'disable dex
        'Button1.Visible = False
        'DataGridView1.Visible = False
        'Label60.Visible = False
        'Label61.Visible = False
        'lnkpricehistory.Visible = False
        'Label49.Text = "            Distributed Exchange is disabled in this build."

        'disclaimer
        MsgBox("DISCLAIMER: " & vbCrLf & vbCrLf & "This software is pre-release software for testing only." & vbCrLf & vbCrLf & "The protocol and transaction processing rules for Mastercoin are still under active development and are subject to change in future." & vbCrLf & vbCrLf & "DO NOT USE IT WITH A LARGE AMOUNT OF MASTERCOINS AND/OR BITCOINS.  IT IS ENTIRELY POSSIBLE YOU MAY LOSE ALL YOUR COINS.  INFORMATION DISPLAYED MAY BE INCORRECT.  MASTERCHEST OFFERS ABSOLUTELY NO GUARANTEES OF ANY KIND." & vbCrLf & vbCrLf & "A fraction of a bitcoin and a fraction of a mastercoin are the suggested testing amounts.  Preferably use a fresh bitcoin wallet.dat." & vbCrLf & vbCrLf & "This software is provided open-source at no cost.  You are responsible for knowing the law in your country and determining if your use of this software contravenes any local laws.")

        poversync.Image = My.Resources.sync
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

        'handle send currency buttons
        rsendmsc.Checked = True
        rsendtmsc.Checked = False
        rsendbtc.Checked = False
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
        'check we have configuration info
        If bitcoin_con.bitcoinrpcserver = "" Or bitcoin_con.bitcoinrpcport = 0 Or bitcoin_con.bitcoinrpcuser = "" Or bitcoin_con.bitcoinrpcpassword = "" Then
            MsgBox("There was a problem configuring your connection to bitcoind/qt.  Both auto detection and manual configuration appear to have failed." & vbCrLf & vbCrLf & "Will now exit.")
            Application.Exit()
        End If

        'test connection to bitcoind
        lwelstartup.Text &= vbCrLf & "Startup: Testing bitcoin connection..."
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
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
            MsgBox("Exception testing connection to bitcoin via RPC : " & ex2.Message)
            Exit Sub
        End Try
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        Application.DoEvents()

        'test connection to database
        lwelstartup.Text &= vbCrLf & "Startup: Testing database connection..."
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        Application.DoEvents()

        Dim testval As Integer
        testval = SQLGetSingleVal("SELECT count(*) FROM information_schema.columns WHERE table_name = 'processedblocks'")
        If testval = 99 Then Exit Sub
        If testval = 2 Then 'sanity check ok
            lwelstartup.Text &= vbCrLf & "Startup: Connection to database established and sanity check OK."
        Else
            'something has gone wrong
            lwelstartup.Text &= vbCrLf & "Startup ERROR: Connection to database seems to be established but responses are not as expected.  Aborting startup."
            Exit Sub
        End If
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        Application.DoEvents()


        '### we have confirmed our connections to resources external to the program ###

        'enumarate bitcoin addresses
        lwelstartup.Text &= vbCrLf & "Startup: Enumerating wallet addresses..."
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
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
        System.Threading.Thread.Sleep(5000)
        Application.DoEvents()
        'do some initial setup
        showlabels()
        bback.Visible = True
        txtdebug.Text = "MASTERCHEST WALLET v0.1a"
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
            If lastval = "6" Then activatesettings()
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
    Private Sub bcurrencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcurrencies.Click
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
    Private Sub bcontracts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcontracts.Click
        'If curscreen <> "6" Then
        'activatesettings()
        'lastscreen = lastscreen & "6"
        'End If
    End Sub
    Private Sub bdebug_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdebug.Click
        If curscreen <> "7" Then
            activatedebug()
            lastscreen = lastscreen & "7"
        End If
    End Sub
    Private Sub bexchange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bexchange.Click
        'If curscreen <> "8" Then
        'activateexchange()
        'lastscreen = lastscreen & "8"
        'End If
    End Sub
    Private Sub checkdebugscroll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkdebugscroll.CheckedChanged
        If checkdebugscroll.Checked = True Then txtdebug.ScrollBars = ScrollBars.Vertical
        If checkdebugscroll.Checked = False Then txtdebug.ScrollBars = ScrollBars.None
    End Sub
    Private Sub lnkdebug_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkdebug.LinkClicked
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
        Me.txtdebug.AppendText(vbCrLf & e.UserState.ToString)
        If e.UserState.ToString.Substring(0, 27) = "DEBUG: Block Analysis for: " Then loversync.Text = "Syncronizing...  Current Block: " & e.UserState.ToString.Substring(27, 6) & "..."
    End Sub

    Private Sub workthread_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles workthread.DoWork
        varsyncronized = False
        workthread.ReportProgress(0, "DEBUG: Thread 'workthread' starting...")
        'test connection to bitcoind
        Try
            Dim checkhash As blockhash = mlib.getblockhash(bitcoin_con, 2)
            If checkhash.result.ToString = "000000006a625f06636b8bb6ac7b960a8d03705d1ace08b1a19da3fdcc99ddbd" Then 'we've got a correct response
                workthread.ReportProgress(0, "STATUS: Connection to bitcoin RPC established & sanity check OK.")
            Else
                'something has gone wrong
                workthread.ReportProgress(0, "ERROR: Connection to bitcoin RPC seems to be established but responses are not as expected." & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
                Exit Sub
            End If
        Catch
            Exit Sub
        End Try

        'test connection to database
        Dim testval As Integer
        testval = SQLGetSingleVal("SELECT count(*) FROM information_schema.columns WHERE table_name = 'processedblocks'")
        If testval = 99 Then Exit Sub
        If testval = 2 Then 'sanity check ok
            workthread.ReportProgress(0, "STATUS: Connection to database established & sanity check OK.")
        Else
            'something has gone wrong
            workthread.ReportProgress(0, "ERROR: Connection to database seems to be established but responses are not as expected." & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
            Exit Sub
        End If
        Application.DoEvents()
        '### we have confirmed our connections to resources external to the program ###

        'check transaction list for last database block and update from there - always delete current last block transactions and go back one ensuring we don't miss transactions if code bombs while processing a block
        Dim dbposition As Integer
        dbposition = SQLGetSingleVal("SELECT MAX(BLOCKNUM) FROM processedblocks")
        If dbposition > 249497 Then dbposition = dbposition - 5 Else dbposition = 249497 'roll database back five blocks every scan for sanity/orphans etc
        'dbposition = 249497
        'delete transactions after dbposition block
        Dim txdeletedcount = SQLGetSingleVal("DELETE FROM transactions WHERE BLOCKNUM > " & dbposition - 1)
        Dim blockdeletedcount = SQLGetSingleVal("DELETE FROM processedblocks WHERE BLOCKNUM > " & dbposition - 1)
        Dim exodeletedcount = SQLGetSingleVal("DELETE FROM exotransactions WHERE BLOCKNUM > " & dbposition - 1)
        workthread.ReportProgress(0, "STATUS: Database starting at block " & dbposition.ToString)
        'System.Threading.Thread.Sleep(10000)
        'check bitcoin RPC for latest block
        Dim rpcblock As Integer
        Dim blockcount As blockcount = mlib.getblockcount(bitcoin_con)
        rpcblock = blockcount.result
        workthread.ReportProgress(0, "STATUS: Network is at block " & rpcblock.ToString)
        'if db block is newer than bitcoin rpc (eg new bitcoin install with preseed db)
        If rpcblock < dbposition Then
            workthread.ReportProgress(0, "ERROR: Database block appears newer than bitcoinrpc blocks - is bitcoinrpc up to date? Exiting thread.")
            Exit Sub
        End If

        'calculate catchup
        Dim catchup As Integer
        catchup = rpcblock - dbposition
        workthread.ReportProgress(0, "STATUS: " & catchup.ToString & " blocks to catch up")

        '### loop through blocks since dbposition and add any transactions detected as mastercoin to the transactions table
        Dim msctranscount As Integer
        msctranscount = 0
        Dim msctrans(100000) As String
        For x = dbposition To rpcblock
            Dim blocknum As Integer = x
            If debuglevel > 0 Then workthread.ReportProgress(0, "DEBUG: Block Analysis for: " & blocknum.ToString)
            Dim blockhash As blockhash = mlib.getblockhash(bitcoin_con, blocknum)
            Dim block As Block = mlib.getblock(bitcoin_con, blockhash.result.ToString)
            Dim txarray() As String = block.result.tx.ToArray

            For j = 1 To UBound(txarray) 'skip tx0 which should be coinbase
                Try
                    Dim workingtxtype As String = mlib.ismastercointx(bitcoin_con, txarray(j))
                    'simple send
                    If workingtxtype = "simple" Then
                        workthread.ReportProgress(0, "BLOCKSCAN: Found MSC transaction (simple send): " & txarray(j))
                        Dim results As txn = mlib.gettransaction(bitcoin_con, txarray(j))
                        'handle generate
                        If results.result.blocktime < 1377993875 Then 'before exodus cutofff
                            Dim mastercointxinfo As mastercointx = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "generate")
                            If mastercointxinfo.type = "generate" And mastercointxinfo.curtype = 0 Then
                                Dim dbwritemsc As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & mastercointxinfo.txid & "','" & mastercointxinfo.fromadd & "','" & mastercointxinfo.toadd & "'," & mastercointxinfo.value & ",'" & mastercointxinfo.type & "'," & mastercointxinfo.blocktime & "," & blocknum & "," & mastercointxinfo.valid & ",1)")
                                Dim dbwritetmsc As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & mastercointxinfo.txid & "','" & mastercointxinfo.fromadd & "','" & mastercointxinfo.toadd & "'," & mastercointxinfo.value & ",'" & mastercointxinfo.type & "'," & mastercointxinfo.blocktime & "," & blocknum & "," & mastercointxinfo.valid & ",2)")
                            End If
                        End If
                        'decode mastercoin transaction
                        Dim txdetails As mastercointx = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "send")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then
                            Dim dbwrite9 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.value & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & ")")
                        Else
                            'String vouts together
                            Dim voutstring As String = ""
                            For Each Vout In results.result.vout
                                For Each address As String In Vout.scriptPubKey.addresses
                                    voutstring = voutstring & "-" & address
                                Next
                            Next

                            Dim dbwrite2 As Integer = SQLGetSingleVal("INSERT INTO exotransactions (TXID,BLOCKTIME,BLOCKNUM,VOUTS) VALUES ('" & results.result.txid & "'," & results.result.blocktime & "," & blocknum & ",'" & voutstring & "')")
                        End If
                    End If

                    'sell offer
                    If workingtxtype = "selloffer" Then
                        workthread.ReportProgress(0, "BLOCKSCAN: Found MSC transaction (sell offer): " & txarray(j))
                        Dim txdetails As mastercointx_selloffer = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "selloffer")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "'," & txdetails.saleamount & "," & txdetails.offeramount & "," & txdetails.minfee & "," & txdetails.timelimit & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & ")")
                    End If

                    'accept offer
                    If workingtxtype = "acceptoffer" Then
                        workthread.ReportProgress(0, "BLOCKSCAN: Found MSC transaction (accept offer): " & txarray(j))
                        Dim txdetails As mastercointx_acceptoffer = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "acceptoffer")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.purchaseamount & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & ")")
                    End If

                Catch exx As Exception
                    Console.WriteLine("ERROR: Exception occured." & vbCrLf & exx.Message & vbCrLf & "Exiting...")
                End Try
            Next
            'only here do we write that the block has been processed to database
            Dim dbwrite3 As Integer = SQLGetSingleVal("INSERT INTO processedblocks VALUES (" & blocknum & "," & block.result.time & ")")
        Next

        'handle unconfirmed transactions
        If debuglevel > 0 Then workthread.ReportProgress(0, "DEBUG: Block Analysis for pending transactions")
        Dim btemplate As blocktemplate = mlib.getblocktemplate(bitcoin_con)
        Dim intermedarray As bttx() = btemplate.result.transactions.ToArray
        For j = 1 To UBound(intermedarray) 'skip tx0 which should be coinbase
            Try
                If mlib.ismastercointx(bitcoin_con, intermedarray(j).hash) = "simple" Then
                    workthread.ReportProgress(0, "BLOCKSCAN: Found MSC transaction (simple send): " & intermedarray(j).hash)
                    Dim results As txn = mlib.gettransaction(bitcoin_con, intermedarray(j).hash)
                    'decode mastercoin transaction
                    Dim txdetails As mastercointx = mlib.getmastercointransaction(bitcoin_con, intermedarray(j).hash.ToString, "send")
                    'see if we have a transaction back and if so write it to database
                    If Not IsNothing(txdetails) Then Dim dbwrite2 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.value & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & 999999 & "," & txdetails.valid & "," & txdetails.curtype & ")")
                End If
            Catch exx As Exception
                Console.WriteLine("ERROR: Exception occured looking at unconfirmed transactions." & vbCrLf & exx.Message & vbCrLf & "Exiting...")
            End Try
        Next

        '///process transactions
        workthread.ReportProgress(0, "BLOCKSCAN: Transaction processing starting... ")
        ' Try
        'dev sends temp
        Dim maxtime As Long = SQLGetSingleVal("SELECT MAX(BLOCKTIME) FROM processedblocks")
        Dim devmsc As Double = Math.Round(((1 - (0.5 ^ ((maxtime - 1377993874) / 31556926))) * 56316.23576222), 8)
        'do all generate transactions and calculate initial balances
        Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        Dim cmd As New SqlCeCommand()
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "delete from transactions_processed"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from balances"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from exchange"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into transactions_processed (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) SELECT TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE from transactions where type='generate'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into balances (address, cbalance, cbalancet) SELECT TOADD,SUM(VALUE),SUM(VALUE) from transactions_processed where curtype = 1 group by toadd"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into balances (address, cbalance, cbalancet) VALUES ('1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P', " & (devmsc * 100000000) & ",0)"
        cmd.ExecuteNonQuery()
        'go through transactions, check validity, process by type and apply to balances
        Dim sqlquery
        Dim returnval
        sqlquery = "SELECT * FROM transactions order by ID"
        cmd.CommandText = sqlquery
        Dim adptSQL As New SqlCeDataAdapter(cmd)
        Dim ds1 As New DataSet()
        adptSQL.Fill(ds1)

        With ds1.Tables(0)
            For rowNumber As Integer = 0 To .Rows.Count - 1
                With .Rows(rowNumber)
                    If .Item(4) = "simple" Then
                        'get currency type
                        Dim curtype As Integer = .Item(8)
                        'get transaction amount
                        Dim txamount As Long = .Item(3)
                        'check senders input balance
                        If curtype = 1 Then sqlquery = "SELECT CBALANCE FROM balances where ADDRESS='" & .Item(1).ToString & "'"
                        If curtype = 2 Then sqlquery = "SELECT CBALANCET FROM balances where ADDRESS='" & .Item(1).ToString & "'"
                        If curtype > 0 And curtype < 3 Then
                            cmd.CommandText = sqlquery
                            returnval = cmd.ExecuteScalar
                            'check if transaction amount is over senders balance
                            If returnval >= txamount Then 'ok
                                cmd.CommandText = "INSERT INTO transactions_processed (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0).ToString & "','" & .Item(1).ToString & "','" & .Item(2).ToString & "'," & .Item(3).ToString & ",'" & .Item(4).ToString & "'," & .Item(5).ToString & "," & .Item(6).ToString & ",1," & .Item(8).ToString & ")"
                                returnval = cmd.ExecuteScalar
                                'subtract balances accordingly
                                If curtype = 1 Then cmd.CommandText = "UPDATE balances SET CBALANCE=CBALANCE-" & txamount & " where ADDRESS='" & .Item(1).ToString & "'"
                                If curtype = 2 Then cmd.CommandText = "UPDATE balances SET CBALANCET=CBALANCET-" & txamount & " where ADDRESS='" & .Item(1).ToString & "'"
                                returnval = cmd.ExecuteScalar
                                'add balances accordingly
                                'does address already exist in db?
                                sqlquery = "SELECT ADDRESS FROM balances where ADDRESS='" & .Item(2).ToString & "'"
                                cmd.CommandText = sqlquery
                                returnval = cmd.ExecuteScalar
                                If .Item(6) < 999998 Then
                                    If returnval = .Item(2).ToString Then
                                        If curtype = 1 Then cmd.CommandText = "UPDATE balances SET CBALANCE=CBALANCE+" & txamount & " where ADDRESS='" & .Item(2).ToString & "'"
                                        If curtype = 2 Then cmd.CommandText = "UPDATE balances SET CBALANCET=CBALANCET+" & txamount & " where ADDRESS='" & .Item(2).ToString & "'"
                                        returnval = cmd.ExecuteScalar
                                    Else
                                        If curtype = 1 Then cmd.CommandText = "INSERT INTO balances (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(2).ToString & "'," & txamount & ",0,0,0)"
                                        If curtype = 2 Then cmd.CommandText = "INSERT INTO balances (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(2).ToString & "',0," & txamount & ",0,0)"
                                        returnval = cmd.ExecuteScalar
                                    End If
                                Else
                                    If returnval = .Item(2).ToString Then
                                        If curtype = 1 Then cmd.CommandText = "UPDATE balances SET UBALANCE=UBALANCE+" & txamount & " where ADDRESS='" & .Item(2).ToString & "'"
                                        If curtype = 2 Then cmd.CommandText = "UPDATE balances SET UBALANCET=UBALANCET+" & txamount & " where ADDRESS='" & .Item(2).ToString & "'"
                                        returnval = cmd.ExecuteScalar
                                    Else
                                        If curtype = 1 Then cmd.CommandText = "INSERT INTO balances (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(2).ToString & "',0,0," & txamount & ",0)"
                                        If curtype = 2 Then cmd.CommandText = "INSERT INTO balances (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(2).ToString & "',0,0,0," & txamount & ")"
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If

                            Else 'transaction not valid
                                cmd.CommandText = "INSERT INTO transactions_processed (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0).ToString & "','" & .Item(1).ToString & "','" & .Item(2).ToString & "'," & .Item(3).ToString & ",'" & .Item(4).ToString & "'," & .Item(5).ToString & "," & .Item(6).ToString & ",0," & .Item(8).ToString & ")"
                                returnval = cmd.ExecuteScalar
                            End If
                        End If
                    End If


                End With
            Next
        End With

        'update address balances
        'enumarate bitcoin addresses
        If debuglevel > 0 Then workthread.ReportProgress(0, "DEBUG: Enumerating addresses...")
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
            workthread.ReportProgress(0, "ERROR: Enumerating addresses did not complete properly." & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
            Exit Sub
        End Try

        balmsc = 0
        baltmsc = 0
        balbtc = 0
        balumsc = 0
        balutmsc = 0
        For Each row In taddresslist.Rows
            sqlquery = "SELECT CBALANCE FROM balances where ADDRESS='" & row.Item(0) & "'"
            Dim addbal As Long = SQLGetSingleVal(sqlquery)
            Dim hrbal As Double = addbal / 100000000
            balmsc = balmsc + addbal
            If hrbal <> row.Item(3) Then row.Item(3) = hrbal

            sqlquery = "SELECT CBALANCET FROM balances where ADDRESS='" & row.Item(0) & "'"
            Dim addtbal As Long = SQLGetSingleVal(sqlquery)
            Dim hrtbal As Double = addtbal / 100000000
            baltmsc = baltmsc + addtbal
            If hrtbal <> row.Item(2) Then row.Item(2) = hrtbal

            sqlquery = "SELECT UBALANCE FROM balances where ADDRESS='" & row.Item(0) & "'"
            Dim addubal As Long = SQLGetSingleVal(sqlquery)
            balumsc = balumsc + addubal

            sqlquery = "SELECT UBALANCET FROM balances where ADDRESS='" & row.Item(0) & "'"
            Dim addutbal As Long = SQLGetSingleVal(sqlquery)
            balutmsc = balutmsc + addutbal

            balbtc = balbtc + row.item(1)
        Next

        'update history - use temp table to reduce ui lag while updating
        thistorylist.Clear()
        For Each row In taddresslist.Rows
            sqlquery = "SELECT txid,valid,fromadd,toadd,curtype,value,blocktime FROM transactions_processed where type='simple' and (FROMADD='" & row.Item(0) & "' or TOADD='" & row.item(0) & "') AND (FROMADD<>'1zAtHRASgdHvZDfHs6xJquMghga4eG7gy' AND TOADD<>'1zAtHRASgdHvZDfHs6xJquMghga4eG7gy')"
            cmd.CommandText = sqlquery
            Dim adptSQL2 As New SqlCeDataAdapter(cmd)
            Dim ds2 As New DataSet()
            adptSQL2.Fill(ds2)
            Dim cur As String
            Dim valimg, dirimg As System.Drawing.Bitmap
            Dim txexists As Boolean = False

            With ds2.Tables(0)
                For rowNumber As Integer = 0 To .Rows.Count - 1
                    With .Rows(rowNumber)
                        txexists = False
                        If .Item(4) = 1 Then cur = "Mastercoin"
                        If .Item(4) = 2 Then cur = "Test Mastercoin"
                        If .Item(1) = False Then valimg = My.Resources.invalid
                        If .Item(1) = True Then valimg = My.Resources.valid
                        If .Item(6) = 0 Then valimg = My.Resources.uncof 'unconfirmed tx
                        If row.item(0) = .Item(2) Then dirimg = My.Resources.out1
                        If row.item(0) = .Item(3) Then dirimg = My.Resources.in1
                        Dim dtdatetime As System.DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
                        If .Item(6) = 0 Then dtdatetime = New DateTime(9999, 12, 30, 0, 0, 0, 0) 'unconfirmed tx
                        dtdatetime = dtdatetime.AddSeconds(.Item(6)).ToLocalTime()
                        thistorylist.Rows.Add(valimg, dirimg, dtdatetime, .Item(2), .Item(3), cur, .Item(5) / 100000000)
                    End With
                Next

            End With
        Next

        'perform bug check
        workthread.ReportProgress(0, "DEBUG: Sanity checking balances table...")
        sqlquery = "select count(address) from balances group by ADDRESS HAVING COUNT(*) > 1"
        cmd.CommandText = sqlquery
        Dim adptSQL3 As New SqlCeDataAdapter(cmd)
        Dim ds3 As New DataSet()
        adptSQL3.Fill(ds3)
        If ds3.Tables(0).Rows.Count > 0 Then
            MsgBox("ERROR:" & vbCrLf & vbCrLf & "Sanity checking has detected addresses with multiple balance entries in the database.  It is not safe to continue.  The wallet will now exit.")
            Application.Exit()
        End If
        'done

        con.Close()

        'Catch ex As Exception
        ' workthread.ReportProgress(0, "ERROR: Processing transactions threw an exception of: " & ex.Message.ToString & " - Exiting thread...")
        ' Exit Sub
        'End Try

        varsyncronized = True
        varsyncblock = rpcblock
        workthread.ReportProgress(0, "BLOCKSCAN: Finished, sleeping. ")

    End Sub

    '//////////////////////////////
    '///// FUNCTIONS
    '//////////////////////////////
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
            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL " & sqlquery)
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
            txtdebug.AppendText(vbCrLf & "DEBUG: Thread exited with error condition.")
            poversync.Image = My.Resources.redcross
            loversync.Text = "Not Syncronized."
            Exit Sub
        End If

        txtdebug.AppendText(vbCrLf & "DEBUG: Thread exited.")
        UIrefresh.Enabled = True
        If varsyncronized = True Then
            poversync.Image = My.Resources.green_tick
            loversync.Text = "Syncronized.  Last block scanned was block " & varsyncblock.ToString & "."
        Else
            poversync.Image = My.Resources.redcross
            loversync.Text = "Not Syncronized."
        End If

        'update addresses
        For Each row In taddresslist.Rows
            If Not comsendaddress.Items.Contains(row.item(0).ToString) Then
                If row.item(1) = 0 And row.item(2) = 0 And row.item(3) = 0 Then
                    'ignore empty address
                Else
                    comsendaddress.Items.Add(row.item(0).ToString)
                End If
            End If


        Next
        Dim hrbalmsc As Double = balmsc / 100000000
        Dim hrbaltmsc As Double = baltmsc / 100000000
        Dim hrbalumsc As Double = balumsc / 100000000
        Dim hrbalutmsc As Double = balutmsc / 100000000

        loverviewmscbal.Text = (hrbalmsc + hrbalumsc).ToString("#0.00000000") & " MSC"
        loverviewsmallmscbal.Text = hrbalmsc.ToString("#0.00000000") & " MSC"
        loverviewsmallunconfmsc.Text = hrbalumsc.ToString("#0.00000000") & " MSC"

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
            currencylist.Rows.Add("Mastercoin", hrbalmsc.ToString, hrbalumsc.ToString)
            currencylist.Rows.Add("Test Mastercoin", hrbaltmsc.ToString, hrbalutmsc.ToString)
            currencylist.Rows.Add("Bitcoin", balbtc.ToString, balubtc.ToString)
            dgvcurrencies.DataSource = currencylist
            Dim dgvcolumn As New DataGridViewColumn
            dgvcolumn = dgvcurrencies.Columns(0)
            dgvcolumn.Width = 398
            dgvcolumn = dgvcurrencies.Columns(1)
            dgvcolumn.Width = 162
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn = dgvcurrencies.Columns(2)
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
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
                historylist.Rows.Add(row.item(0), row.item(1), row.item(2), row.item(3), row.item(4), row.item(5), row.item(6))
            Next
            dgvhistory.DataSource = historylist
            Dim dgvcolumn As New DataGridViewColumn
            dgvcolumn = dgvhistory.Columns(0)
            dgvcolumn.Width = 13
            dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            dgvcolumn = dgvhistory.Columns(1)
            dgvcolumn.Width = 16
            dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            dgvcolumn = dgvhistory.Columns(2)
            dgvcolumn.Width = 115
            dgvcolumn = dgvhistory.Columns(3)
            dgvcolumn.Width = 233
            dgvcolumn = dgvhistory.Columns(4)
            dgvcolumn.Width = 233
            dgvcolumn = dgvhistory.Columns(5)
            dgvcolumn.Width = 94
            dgvcolumn = dgvhistory.Columns(6)
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn.Width = 120
            If lnkhistorysort.Text = "Highest Value" Then dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Descending)
            If lnkhistorysort.Text = "Lowest Value" Then dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Ascending)
            If lnkhistorysort.Text = "Recent First" Then dgvhistory.Sort(dgvhistory.Columns(2), System.ComponentModel.ListSortDirection.Descending)
        Catch ex As Exception
            MsgBox("historylist exception" & vbCrLf & ex.Message)
        End Try
        'DataGridView1.DataSource = openorders
        'DataGridView1.CurrentCell = Nothing
        'DataGridView2.DataSource = selloffers
        'DataGridView2.CurrentCell = Nothing
        updateui()
        Application.DoEvents()
      
    End Sub


    Private Sub dgvaddresses_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvaddresses.CellMouseDown
        If e.Button = MouseButtons.Right Then
            mrow = e.RowIndex
            mcol = e.ColumnIndex
        End If
    End Sub

    Private Sub UIrefresh_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UIrefresh.Tick
        UIrefresh.Enabled = False
        poversync.Image = My.Resources.sync
        loversync.Text = "Syncronizing..."
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
        txtdebug.Text = "MASTERCHEST WALLET v0.1a"
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

    Private Sub btest_Click(sender As System.Object, e As System.EventArgs) Handles btest.Click
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

    Private Sub bfinish_Click(sender As System.Object, e As System.EventArgs) Handles bfinish.Click
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

    Private Sub txtstartwalpass_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtstartwalpass.TextChanged
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
    Private Sub rsendmsc_CheckedClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rsendmsc.Click
        rsendmsc.Checked = True
        rsendtmsc.Checked = False
        rsendbtc.Checked = False
        comsendaddress.Enabled = True
        updateavail()
    End Sub

    Private Sub rsendtmsc_CheckedClicked(sender As System.Object, e As System.EventArgs) Handles rsendtmsc.Click
        rsendmsc.Checked = False
        rsendtmsc.Checked = True
        rsendbtc.Checked = False
        comsendaddress.Enabled = True
        updateavail()
    End Sub

    Private Sub rsendbtc_CheckedClicked(sender As System.Object, e As System.EventArgs) Handles rsendbtc.Click
        rsendmsc.Checked = False
        rsendtmsc.Checked = False
        rsendbtc.Checked = True
        comsendaddress.Enabled = False
        updateavail()
    End Sub
    Private Sub updateavail()
        Dim denom As String = ""
        avail = 0
        If rsendmsc.Checked = True Then
            denom = " MSC"
            For Each row In addresslist.Rows
                If row.item(0) = comsendaddress.SelectedItem Then avail = row.item(3)
            Next
        End If
        If rsendtmsc.Checked = True Then
            denom = " TEST MSC"
            For Each row In addresslist.Rows
                If row.item(0) = comsendaddress.SelectedItem Then avail = row.item(2)
            Next
        End If
        If rsendbtc.Checked = True Then
            denom = " BTC (TOTAL)"
            avail = balbtc
        End If
        If denom = "" Then lsendavail.Text = "Select a sending address"
        lsendavail.Text = "Available: " & avail.ToString & denom
    End Sub
    Private Sub comsendaddress_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles comsendaddress.SelectedIndexChanged
        updateavail()
    End Sub

    Private Sub txtsendamount_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtsendamount.TextChanged
        If txtsendamount.Text <> "" Then
            If Val(txtsendamount.Text) > 0 And Val(txtsendamount.Text) <= avail Then
                lsendamver.Text = ""
            End If
            If Val(txtsendamount.Text) > 0 And Val(txtsendamount.Text) > avail Then
                lsendamver.Text = "Insufficient Funds"
                lsendamver.ForeColor = Color.FromArgb(255, 192, 128)
            End If
        End If

    End Sub

    Private Sub bback_Click(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles bback.MouseUp

    End Sub

    Private Sub lnkhistorysort_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkhistorysort.LinkClicked
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

    Private Sub lnkhistoryfilter_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkhistoryfilter.LinkClicked
        If lnkhistoryfilter.Text = "No Filter Active" Then
            lnkhistoryfilter.Text = "Mastercoin Only"
            Exit Sub
        End If
        If lnkhistoryfilter.Text = "Mastercoin Only" Then
            lnkhistoryfilter.Text = "No Filter Active"
            Exit Sub
        End If
    End Sub

    Private Sub bsignsend_Click(sender As System.Object, e As System.EventArgs) Handles bsignsend.Click
        If Val(txtsendamount.Text) > 0 And Val(txtsendamount.Text) > avail Then
            lsendamver.Text = "Insufficient Funds"
            lsendamver.ForeColor = Color.FromArgb(255, 192, 128)
            Exit Sub
        End If
        If Val(txtsendamount.Text) = 0 Then
            'nothing to send
            Exit Sub
        End If
        txsummary = ""
        senttxid = "Transaction not sent"
        'first validate recipient address
        If txtsenddest.Text <> "" Then
            'get wallet passphrase
            passfrm.ShowDialog()
            If btcpass = "" Then Exit Sub 'cancelled
            Dim fromadd As String
            If rsendbtc.Checked = False Then fromadd = Trim(comsendaddress.SelectedItem.ToString)
            Dim toadd As String = Trim(txtsenddest.Text)
            Dim curtype As Integer
            If rsendmsc.Checked = True Then curtype = 1
            If rsendtmsc.Checked = True Then curtype = 2
            Dim amount As Double = Val(txtsendamount.Text)
            Dim amountlong As Long = amount * 100000000

            'handle bitcoin sends - disabled while we move to building transaction manually so we have control over sending address
            If rsendbtc.Checked = True Then
                MsgBox("Bitcoin sends are temporarily disabled in this build.")
                Exit Sub
                Try
                    Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, txtsenddest.Text, 0, 0))
                    If validater.result.isvalid = True Then 'address is valid
                        txsummary = "Recipient address is valid."
                        'push out to bitcoin rpc to send the tx since we can use sendtoaddress for simple bitcoin tx
                        'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
                        Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
                        Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(btcpass.ToString), 15, 0)
                        btcpass = "" 'dispose of wallet passphrase
                        Dim txref As broadcasttx = JsonConvert.DeserializeObject(Of broadcasttx)(mlib.rpccall(bitcoin_con, "sendtoaddress", 2, txtsenddest.Text, amount, 0))
                        'check txref is not empty and display txref
                        If txref.result <> "" Then
                            txsummary = txsummary & vbCrLf & "Transaction sent, ID: " & txref.result
                            'lsendtxinfo.Text = "Transaction sent, check viewer for TXID."
                            lsendtxinfo.ForeColor = Color.Lime
                            bsignsend.Enabled = False
                            Application.DoEvents()

                            If workthread.IsBusy <> True Then
                                UIrefresh.Enabled = False
                                poversync.Image = My.Resources.sync
                                loversync.Text = "Syncronizing..."
                                ' Start the workthread for the blockchain scanner
                                workthread.RunWorkerAsync()
                            End If
                            Exit Sub
                        Else
                            txsummary = txsummary & vbCrLf & "Failed to send transaction."
                        End If
                    Else
                        txsummary = txsummary & vbCrLf & "Build transaction failed.  Recipient address is not valid."
                    End If
                Catch ex As Exception
                    MsgBox("Exeption thrown : " & ex.Message)
                    Exit Sub
                End Try
                Exit Sub
            End If

            If rsendbtc.Checked = False Then
                Try
                    Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, txtsenddest.Text, 0, 0))
                    If validater.result.isvalid = True Then 'address is valid
                        txsummary = "Recipient address is valid."
                        'push out to masterchest lib to encode the tx
                        Dim rawtx As String = mlib.encodetx(bitcoin_con, fromadd, toadd, curtype, amountlong)
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
                                    'lsendtxinfo.Text = "Transaction sent, check viewer for TXID."
                                    'lsendtxinfo.ForeColor = Color.Lime
                                    bsignsend.Enabled = False
                                    Application.DoEvents()
                                    sentfrm.ShowDialog()
                                    If workthread.IsBusy <> True Then
                                        UIrefresh.Enabled = False
                                        poversync.Image = My.Resources.sync
                                        loversync.Text = "Syncronizing..."
                                        ' Start the workthread for the blockchain scanner
                                        workthread.RunWorkerAsync()
                                    End If
                                    Exit Sub
                                Else
                                    txsummary = txsummary & vbCrLf & "Error sending transaction."
                                    sentfrm.ShowDialog()
                                    lsendtxinfo.Text = "Error sending transaction."
                                    lsendtxinfo.ForeColor = Color.FromArgb(255, 192, 128)
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

        End If
    End Sub

    Private Sub bsendnew_Click(sender As System.Object, e As System.EventArgs) Handles bsendnew.Click
        txtsendamount.Text = "0.00000000"
        sentfrm.txtviewer.Text = ""
        lsendamver.Visible = False
        txtsenddest.Text = ""
        lsendtxinfo.Text = ""
        bsignsend.Enabled = True
    End Sub

    Private Sub dgvhistory_DataError(sender As Object, e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvhistory.DataError
        'trap error when we clear list bound to dgv temporarily
    End Sub

    Private Sub dgvaddresses_DataError(sender As Object, e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvaddresses.DataError
        'trap error when we clear list bound to dgv temporarily
    End Sub

    Private Sub Form1_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        rsendmsc.Checked = True
        rsendtmsc.Checked = False
        rsendbtc.Checked = False
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
End Class

