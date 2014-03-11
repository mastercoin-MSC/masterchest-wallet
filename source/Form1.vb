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
Imports System.Threading
Imports System.Environment
Imports System.IO
Imports System.Data.SqlServerCe
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

    Dim activeCulture As CultureInfo = Thread.CurrentThread.CurrentCulture.Clone()
    Dim locales As New Dictionary(Of String, String)

    '////////////////////////
    '///HANDLE FORM FUNCTIONS
    '////////////////////////
    <DllImportAttribute("user32.dll")> Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function
    <DllImportAttribute("user32.dll")> Public Shared Function ReleaseCapture() As Boolean
    End Function
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown, psetup.MouseDown, pwelcome.MouseDown, pcurrencies.MouseDown, paddresses.MouseDown, pdebug.MouseDown, poverview.MouseDown, psend.MouseDown, psettings.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If

    End Sub
    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        Application.Exit()
    End Sub
    Private Sub bmin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bmin.Click
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

        'declare globalization to make sure we use a . for decimal only
        activeCulture.NumberFormat.NumberDecimalSeparator = "."
        Thread.CurrentThread.CurrentCulture = activeCulture
        Thread.CurrentThread.CurrentUICulture = activeCulture

        ' Fallback Locale/CulutureInfo (embedded in main assembly)
        locales.Add("en-US", "English (US)")


        ' More culutures can be added here when translations are available
        ' Culture name and language  matrix: http://msdn.microsoft.com/en-us/goglobal/bb896001.aspx
        ' To list cultures of host OS use: System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
        locales.Add("sv-SE", "Svenska")
        locales.Add("fr-FR", "Français")

        Dim value As String = Nothing
        locales.TryGetValue(My.Settings.culture, value)

        For Each locale In locales
            cbLocale.Items.Add(locale.Value)
            If value Is Nothing And locale.Key = Thread.CurrentThread.CurrentUICulture.Name Then
                cbLocale.SelectedItem = locale.Value
            End If
        Next

        If value IsNot Nothing Then
            cbLocale.SelectedItem = value
        End If

        MsgBox(My.Resources.disclaimer1 & " " & vbCrLf & vbCrLf & My.Resources.disclaimer2 & vbCrLf & vbCrLf & My.Resources.disclaimer3 & vbCrLf & vbCrLf & My.Resources.disclaimer4 & vbCrLf & vbCrLf & My.Resources.disclaimer5 & vbCrLf & vbCrLf & My.Resources.disclaimer6)

        poversync.Image = My.Resources.sync
        loversync.Text = My.Resources.synchronizing
        bback.Visible = False
        hidelabels()
        initialize()
        'are we configured?
        'setup bitcoin connection
        txtrpcserver.Text = My.Resources.notconfigured
        txtrpcport.Text = My.Resources.notconfigured
        txtrpcuser.Text = My.Resources.notconfigured
        txtrpcpassword.Text = My.Resources.notconfigured
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
                    MsgBox(My.Resources.messageheader1 & vbCrLf & vbCrLf & My.Resources.messageinfo1 & vbCrLf & vbCrLf & My.Resources.messagerequest1 & vbCrLf & vbCrLf & My.Resources.willnowexit)
                    Application.Exit()
                End If
                If txenabled = False Then
                    MsgBox(My.Resources.messageheader1 & vbCrLf & vbCrLf & My.Resources.messageinfo2 & vbCrLf & vbCrLf & My.Resources.messagerequest2 & vbCrLf & vbCrLf & My.Resources.willnowexit)
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
                    MsgBox(My.Resources.messageexception & ex.Message)
                    Application.Exit()
                End Try
            End If

        Catch ex As Exception
            MsgBox(My.Resources.messageexception & ex.Message)
            Application.Exit()
        End Try

        'handle send currency buttons
        rsendmsc.Checked = True
        rsendtmsc.Checked = False
        rsendbtc.Checked = False
        'show welcome panel
        pwelcome.Visible = True
        'Me.ActiveControl = txtwalpass
        lwelstartup.Text = My.Resources.startupinitializing
    End Sub
    Private Sub updateui()
        Me.Refresh()
        Application.DoEvents()
    End Sub

    Private Sub teststartup()
        'check we have configuration info
        If bitcoin_con.bitcoinrpcserver = "" Or bitcoin_con.bitcoinrpcport = 0 Or bitcoin_con.bitcoinrpcuser = "" Or bitcoin_con.bitcoinrpcpassword = "" Then
            MsgBox(My.Resources.messagestartinfo1 & vbCrLf & vbCrLf & My.Resources.willnowexit)
            Application.Exit()
        End If

        'test connection to bitcoind
        lwelstartup.Text &= vbCrLf & My.Resources.startinfo1
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        Application.DoEvents()

        Try
            Dim checkhash As blockhash = mlib.getblockhash(bitcoin_con, 2)
            If checkhash.result.ToString = "000000006a625f06636b8bb6ac7b960a8d03705d1ace08b1a19da3fdcc99ddbd" Then 'we've got a correct response
                lwelstartup.Text &= vbCrLf & My.Resources.startinfo2
            Else
                'something has gone wrong
                lwelstartup.Text &= vbCrLf & My.Resources.starterror1
                Exit Sub
            End If
        Catch ex2 As Exception
            lwelstartup.Text &= vbCrLf & My.Resources.starterror2 & " " & ex2.Message
            MsgBox(My.Resources.messagesstartexception1 & " " & ex2.Message)
            Exit Sub
        End Try
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        Application.DoEvents()

        'test connection to database
        lwelstartup.Text &= vbCrLf & My.Resources.startinfo3
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        Application.DoEvents()

        Dim testval As Integer
        testval = SQLGetSingleVal("SELECT count(*) FROM information_schema.columns WHERE table_name = 'processedblocks'")
        If testval = 99 Then Exit Sub
        If testval = 2 Then 'sanity check ok
            lwelstartup.Text &= vbCrLf & My.Resources.startinfo4
        Else
            'something has gone wrong
            lwelstartup.Text &= vbCrLf & My.Resources.starterror3
            Exit Sub
        End If
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        Application.DoEvents()


        '### we have confirmed our connections to resources external to the program ###

        'enumarate bitcoin addresses
        lwelstartup.Text &= vbCrLf & My.Resources.startinfo5
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

            If lnkaddsort.Text = My.Resources.addressalpha Then dgvaddresses.Sort(dgvaddresses.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            If lnkaddsort.Text = My.Resources.balancebtc Then dgvaddresses.Sort(dgvaddresses.Columns(1), System.ComponentModel.ListSortDirection.Descending)
            If lnkaddsort.Text = My.Resources.balancemsc Then dgvaddresses.Sort(dgvaddresses.Columns(3), System.ComponentModel.ListSortDirection.Descending)
            If lnkaddfilter.Text = My.Resources.nofilteractive Then addresslist.DefaultView.RowFilter = ""
            If lnkaddfilter.Text = My.Resources.emptybalances Then addresslist.DefaultView.RowFilter = "btcamount > 0 or mscamount > 0 or tmscamount > 0"

            Catch ex As Exception
                MsgBox(ex.Message)
            lwelstartup.Text &= vbCrLf & My.Resources.starterror4
                Exit Sub

        End Try
        balbtc = 0
        For Each row In taddresslist.Rows
            balbtc = balbtc + row.item(1)
        Next

        startup = False
        lwelstartup.Text &= vbCrLf & My.Resources.startupinitializingcomplete
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
            syncicon.Image = My.Resources.sync
            syncicon.Visible = True
            lsyncing.Visible = True
            lsyncing.Text = "Synchronizing..."
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
        If debuglevel = 1 Then
            debuglevel = 2
            lnkdebug.Text = My.Resources.med
            Exit Sub
        End If
        If debuglevel = 2 Then
            debuglevel = 3
            lnkdebug.Text = My.Resources.high
            Exit Sub
        End If
        If debuglevel = 3 Then
            debuglevel = 1
            lnkdebug.Text = My.Resources.low
            Exit Sub
        End If
    End Sub

    '//////////////////////////////
    '/// BACKGROUND WORKER
    '//////////////////////////////
    Private Sub workthread_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles workthread.ProgressChanged
        Me.txtdebug.AppendText(vbCrLf & e.UserState.ToString)
        If e.UserState.ToString.Substring(0, 27) = My.Resources.debugblockanalysis & " " Then loversync.Text = My.Resources.synchronizing & " " & My.Resources.currentblock & " " & e.UserState.ToString.Substring(27, 6) & "..."
    End Sub

    Private Sub workthread_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles workthread.DoWork
        'declare globalization to make sure we use a . for decimal only
        ' Dim customCulture As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture.Clone()
        'customCulture.NumberFormat.NumberDecimalSeparator = "."
        System.Threading.Thread.CurrentThread.CurrentCulture = activeCulture
        System.Threading.Thread.CurrentThread.CurrentUICulture = activeCulture

        varsyncronized = False
        workthread.ReportProgress(0, My.Resources.workerdebug1)
        'test connection to bitcoind
        Try
            Dim checkhash As blockhash = mlib.getblockhash(bitcoin_con, 2)
            If checkhash.result.ToString = "000000006a625f06636b8bb6ac7b960a8d03705d1ace08b1a19da3fdcc99ddbd" Then 'we've got a correct response
                workthread.ReportProgress(0, My.Resources.workerstatus1)
            Else
                'something has gone wrong
                workthread.ReportProgress(0, My.Resources.workererror1 & vbCrLf & My.Resources.workerstatus2)
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
            workthread.ReportProgress(0, My.Resources.workerstatus3)
        Else
            'something has gone wrong
            workthread.ReportProgress(0, My.Resources.workererror2 & vbCrLf & My.Resources.workerstatus2)
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
        workthread.ReportProgress(0, My.Resources.workerstatus4 & " " & dbposition.ToString)
        'System.Threading.Thread.Sleep(10000)
        'check bitcoin RPC for latest block
        Dim rpcblock As Integer
        Dim blockcount As blockcount = mlib.getblockcount(bitcoin_con)
        rpcblock = blockcount.result
        workthread.ReportProgress(0, My.Resources.workerstatus5 & " " & rpcblock.ToString)
        'if db block is newer than bitcoin rpc (eg new bitcoin install with preseed db)
        If rpcblock < dbposition Then
            workthread.ReportProgress(0, My.Resources.workererror3)
            Exit Sub
        End If

        'calculate catchup
        Dim catchup As Integer
        catchup = rpcblock - dbposition
        workthread.ReportProgress(0, My.Resources.status & " " & catchup.ToString & " " & My.Resources.blockscatchup)

        '### loop through blocks since dbposition and add any transactions detected as mastercoin to the transactions table
        Dim msctranscount As Integer
        msctranscount = 0
        Dim msctrans(100000) As String
        For x = dbposition To rpcblock
            Dim blocknum As Integer = x
            If debuglevel > 0 Then workthread.ReportProgress(0, My.Resources.workerdebug2 & " " & blocknum.ToString)
            Dim blockhash As blockhash = mlib.getblockhash(bitcoin_con, blocknum)
            Dim block As Block = mlib.getblock(bitcoin_con, blockhash.result.ToString)
            Dim txarray() As String = block.result.tx.ToArray

            For j = 1 To UBound(txarray) 'skip tx0 which should be coinbase
                Try
                    Dim workingtxtype As String = mlib.ismastercointx(bitcoin_con, txarray(j))
                    'simple send
                    If workingtxtype = "simple" Then
                        workthread.ReportProgress(0, My.Resources.workerblockscan1 & " " & txarray(j))
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
                        workthread.ReportProgress(0, My.Resources.workerblockscan2 & " " & txarray(j))
                        Dim txdetails As mastercointx_selloffer = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "selloffer")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION,ACTION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "'," & txdetails.saleamount & "," & txdetails.offeramount & "," & txdetails.minfee & "," & txdetails.timelimit & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & "," & txdetails.version & "," & txdetails.action & ")")
                    End If

                    'accept offer
                    If workingtxtype = "acceptoffer" Then
                        workthread.ReportProgress(0, My.Resources.workerblockscan3 & " " & txarray(j))
                        Dim txdetails As mastercointx_acceptoffer = mlib.getmastercointransaction(bitcoin_con, txarray(j).ToString, "acceptoffer")
                        'see if we have a transaction back and if so write it to database
                        If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.purchaseamount & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & ",0)")
                    End If

                Catch exx As Exception
                    Console.WriteLine("ERROR: Exception occured." & vbCrLf & exx.Message & vbCrLf & "Exiting...")
                End Try
            Next
            'only here do we write that the block has been processed to database
            Dim dbwrite3 As Integer = SQLGetSingleVal("INSERT INTO processedblocks VALUES (" & blocknum & "," & block.result.time & ")")
        Next

        'handle unconfirmed transactions
        If debuglevel > 0 Then workthread.ReportProgress(0, My.Resources.workerdebug3)
        Dim btemplate As blocktemplate = mlib.getblocktemplate(bitcoin_con)
        Dim intermedarray As bttx() = btemplate.result.transactions.ToArray
        For j = 1 To UBound(intermedarray) 'skip tx0 which should be coinbase
            Try
                Dim workingtxtype As String = mlib.ismastercointx(bitcoin_con, intermedarray(j).hash)
                'simple send
                If workingtxtype = "simple" Then
                    workthread.ReportProgress(0, My.Resources.workerblockscan7 & " " & intermedarray(j).hash)
                    'decode mastercoin transaction
                    Dim txdetails As mastercointx = mlib.getmastercointransaction(bitcoin_con, intermedarray(j).hash.ToString, "send")
                    'see if we have a transaction back and if so write it to database
                    If Not IsNothing(txdetails) Then
                        Dim dbwrite2 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.value & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & 999999 & "," & txdetails.valid & "," & txdetails.curtype & ")")
                    Else
                        Dim results As txn = mlib.gettransaction(bitcoin_con, intermedarray(j).hash)
                        'String vouts together
                        Dim voutstring As String = ""
                        For Each Vout In results.result.vout
                            For Each address As String In Vout.scriptPubKey.addresses
                                voutstring = voutstring & "-" & address
                            Next
                        Next
                        Dim dbwrite2 As Integer = SQLGetSingleVal("INSERT INTO exotransactions (TXID,BLOCKTIME,BLOCKNUM,VOUTS) VALUES ('" & results.result.txid & "'," & results.result.blocktime & ",999999,'" & voutstring & "')")

                    End If
                End If
                If workingtxtype = "selloffer" Then
                    workthread.ReportProgress(0, My.Resources.workerblockscan8 & " " & intermedarray(j).hash)
                    'decode mastercoin transaction
                    Dim txdetails As mastercointx_selloffer = mlib.getmastercointransaction(bitcoin_con, intermedarray(j).hash.ToString, "selloffer")
                    'see if we have a transaction back and if so write it to database
                    If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION,ACTION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "'," & txdetails.saleamount & "," & txdetails.offeramount & "," & txdetails.minfee & "," & txdetails.timelimit & ",'" & txdetails.type & "'," & txdetails.blocktime & ",999999," & txdetails.valid & "," & txdetails.curtype & "," & txdetails.version & "," & txdetails.action & ")")
                End If
                If workingtxtype = "acceptoffer" Then
                    workthread.ReportProgress(0, "BLOCKSCAN: Found pending MSC transaction (accept offer): " & intermedarray(j).hash)
                    Dim txdetails As mastercointx_acceptoffer = mlib.getmastercointransaction(bitcoin_con, intermedarray(j).hash.ToString, "acceptoffer")
                    'see if we have a transaction back and if so write it to database
                    If Not IsNothing(txdetails) Then Dim dbwrite4 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,VERSION) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.purchaseamount & ",'" & txdetails.type & "'," & txdetails.blocktime & ",999999," & txdetails.valid & "," & txdetails.curtype & ",0)")
                End If
            Catch exx As Exception
                Console.WriteLine("ERROR: Exception occured looking at unconfirmed transactions." & vbCrLf & exx.Message & vbCrLf & "Exiting...")
            End Try
        Next

        '///process transactions
        workthread.ReportProgress(0, My.Resources.workerblockscan5)
        ' Try
        'dev sends temp
        Dim maxtime As Long = SQLGetSingleVal("SELECT MAX(BLOCKTIME) FROM processedblocks")
        Dim devmsc As Double = Math.Round(((1 - (0.5 ^ ((maxtime - 1377993874) / 31556926))) * 56316.23576222), 8)
        'do all generate transactions and calculate initial balances
        Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        Dim cmd As New SqlCeCommand()
        cmd.Connection = con
        con.Open()
        cmd.CommandText = "delete from transactions_processed_temp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from balances_temp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "delete from exchange_temp"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into transactions_processed_temp (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) SELECT TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE from transactions where type='generate'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into balances_temp (address, cbalance, cbalancet) SELECT TOADD,SUM(VALUE),SUM(VALUE) from transactions_processed_temp where curtype = 1 group by toadd"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "insert into balances_temp (address, cbalance, cbalancet) VALUES ('1EXoDusjGwvnjZUyKkxZ4UHEf77z6A5S4P', " & (devmsc * 100000000) & ",0)"
        cmd.ExecuteNonQuery()
        'go through transactions, check validity, process by type and apply to balances
        Dim sqlquery
        Dim returnval
        sqlquery = "SELECT * FROM transactions order by ID ASC"
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
                        If curtype = 1 Then sqlquery = "SELECT CBALANCE FROM balances_temp where ADDRESS='" & .Item(1).ToString & "'"
                        If curtype = 2 Then sqlquery = "SELECT CBALANCET FROM balances_temp where ADDRESS='" & .Item(1).ToString & "'"
                        If curtype > 0 And curtype < 3 Then
                            cmd.CommandText = sqlquery
                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                            returnval = cmd.ExecuteScalar
                            'check if transaction amount is over senders balance
                            If returnval >= txamount Then 'ok
                                cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0).ToString & "','" & .Item(1).ToString & "','" & .Item(2).ToString & "'," & .Item(3).ToString & ",'" & .Item(4).ToString & "'," & .Item(5).ToString & "," & .Item(6).ToString & ",1," & .Item(8).ToString & ")"
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                returnval = cmd.ExecuteScalar
                                'subtract balances accordingly
                                If curtype = 1 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCE=CBALANCE-" & txamount & " where ADDRESS='" & .Item(1).ToString & "'"
                                If curtype = 2 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCET=CBALANCET-" & txamount & " where ADDRESS='" & .Item(1).ToString & "'"
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                returnval = cmd.ExecuteScalar
                                'add balances accordingly
                                'does address already exist in db?
                                sqlquery = "SELECT ADDRESS FROM balances_temp where ADDRESS='" & .Item(2).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                returnval = cmd.ExecuteScalar
                                If .Item(6) < 999998 Then
                                    If returnval = .Item(2).ToString Then
                                        If curtype = 1 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCE=CBALANCE+" & txamount & " where ADDRESS='" & .Item(2).ToString & "'"
                                        If curtype = 2 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCET=CBALANCET+" & txamount & " where ADDRESS='" & .Item(2).ToString & "'"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    Else
                                        If curtype = 1 Then cmd.CommandText = "INSERT INTO balances_temp (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(2).ToString & "'," & txamount & ",0,0,0)"
                                        If curtype = 2 Then cmd.CommandText = "INSERT INTO balances_temp (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(2).ToString & "',0," & txamount & ",0,0)"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    End If
                                Else
                                    If returnval = .Item(2).ToString Then
                                        If curtype = 1 Then cmd.CommandText = "UPDATE balances_temp SET UBALANCE=UBALANCE+" & txamount & " where ADDRESS='" & .Item(2).ToString & "'"
                                        If curtype = 2 Then cmd.CommandText = "UPDATE balances_temp SET UBALANCET=UBALANCET+" & txamount & " where ADDRESS='" & .Item(2).ToString & "'"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    Else
                                        If curtype = 1 Then cmd.CommandText = "INSERT INTO balances_temp (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(2).ToString & "',0,0," & txamount & ",0)"
                                        If curtype = 2 Then cmd.CommandText = "INSERT INTO balances_temp (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(2).ToString & "',0,0,0," & txamount & ")"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If
                            Else 'transaction not valid
                                cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0).ToString & "','" & .Item(1).ToString & "','" & .Item(2).ToString & "'," & .Item(3).ToString & ",'" & .Item(4).ToString & "'," & .Item(5).ToString & "," & .Item(6).ToString & ",0," & .Item(8).ToString & ")"
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                returnval = cmd.ExecuteScalar
                            End If
                        End If
                    End If
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
                            'get currency type
                            Dim curtype As Integer = .Item(8)
                            Dim sellaction As Integer = 0
                            'check version/action
                            If .Item(16) = 1 Then 'version 1 transaction, pull action explicitly
                                If Not IsDBNull(.Item(15)) Then sellaction = .Item(15)
                            End If
                            If .Item(16) = 1 And Not IsDBNull(.Item(15)) And Not IsDBNull(.Item(10)) Then
                                If .Item(15) = 2 And .Item(10) = 0 Then sellaction = 3 'handle edge case action=update,amount=0
                            End If
                            If .Item(16) = 1 And Not IsDBNull(.Item(15)) Then
                                If .Item(15) = 2 Then
                                    'check there is actually an existing sell to update, otherwise switch to new sell
                                    sqlquery = "SELECT COUNT(fromadd) FROM exchange_temp where FROMADD='" & .Item(1).ToString & "'"
                                    cmd.CommandText = sqlquery
                                    If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                    returnval = cmd.ExecuteScalar
                                    If returnval = 0 Then
                                        sellaction = 1 'new
                                    End If
                                End If
                            End If
                            If .Item(16) = 0 Then 'version 0 transaction, infer action from exchange state
                                'check for existing sell (therefore update) otherwise new sell
                                sqlquery = "SELECT COUNT(fromadd) FROM exchange_temp where FROMADD='" & .Item(1).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                returnval = cmd.ExecuteScalar
                                If returnval = 1 Then
                                    sellaction = 2 'update				
                                Else
                                    sellaction = 1 'new
                                End If
                                'check for saleamount = zero (therefore cancel)
                                If .Item(10) = 0 Then sellaction = 3
                            End If

                            'force test MSC and ensure we have an action
                            If curtype = 2 And sellaction > 0 And sellaction < 4 Then
                                'get transaction amount
                                Dim saleamount As Long = .Item(10)
                                'debug - fromadd
                                Dim fromadd As String = .Item(1).ToString

                                'sanity check exchange table to ensure not more than one sell for address
                                sqlquery = "SELECT COUNT(fromadd) FROM exchange_temp where FROMADD='" & .Item(1).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                returnval = cmd.ExecuteScalar
                                If returnval > 1 Then
                                    MsgBox("Sanity check has failed.  More than one sell for an address exists in the exchange table.  It is not safe to continue.  Exiting...")
                                    End
                                End If

                                'get sell details from exchange table
                                Dim tmpreserved As Long
                                Dim tmpsaleamount As Long
                                sqlquery = "SELECT RESERVED FROM exchange_temp where FROMADD='" & .Item(1).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                returnval = cmd.ExecuteScalar
                                If IsDBNull(returnval) Then
                                    tmpreserved = 0
                                Else
                                    tmpreserved = returnval
                                End If
                                sqlquery = "SELECT SALEAMOUNT FROM exchange_temp where FROMADD='" & .Item(1).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                returnval = cmd.ExecuteScalar
                                If IsDBNull(returnval) Then
                                    tmpsaleamount = 0
                                Else
                                    tmpsaleamount = returnval
                                End If

                                'cancel
                                If sellaction = 3 Then
                                    If .Item(6) > 999998 Then
                                        'set the sell to be pending
                                        cmd.CommandText = "UPDATE exchange_temp SET BLOCKNUM=999999 where FROMADD='" & .Item(1).ToString & "'"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    Else
                                        'credit back remaining saleamount
                                        If curtype = 1 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCE=CBALANCE+" & tmpsaleamount & " where ADDRESS='" & .Item(1).ToString & "'"
                                        If curtype = 2 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCET=CBALANCET+" & tmpsaleamount & " where ADDRESS='" & .Item(1).ToString & "'"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar

                                        If tmpreserved = 0 Then 'nothing reserved, cancel whole sell
                                            'delete it from exchange table
                                            sqlquery = "DELETE FROM exchange_temp where FROMADD='" & .Item(1).ToString & "'"
                                            cmd.CommandText = sqlquery
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                        Else 'funds reserved, zero saleamount but do not remove sell as still active
                                            cmd.CommandText = "UPDATE exchange_temp SET SALEAMOUNT=0 where FROMADD='" & .Item(1).ToString & "'"
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                        End If
                                        cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & .Item(10) & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "cancelsell" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & ")"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If

                                'update
                                If sellaction = 2 And .Item(10) > 0 And .Item(11) > 0 And .Item(13) > 0 Then 'sanity check we have all the necessary details
                                    'calculate update difference
                                    Dim tmpsaleam As Long = 0
                                    Dim tmpvalid As Boolean = False
                                    Dim tmpdiff As Long = saleamount - tmpsaleamount
                                    If tmpdiff > 0 Then 'new sell is higher than existing sell
                                        If curtype = 1 Then sqlquery = "SELECT CBALANCE FROM balances_temp where ADDRESS='" & .Item(1).ToString & "'"
                                        If curtype = 2 Then sqlquery = "SELECT CBALANCET FROM balances_temp where ADDRESS='" & .Item(1).ToString & "'"
                                        cmd.CommandText = sqlquery
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                        'check if transaction amount is over senders balance, if not diff amount = senders balance
                                        If returnval < tmpdiff Then
                                            tmpdiff = returnval
                                        End If
                                        tmpsaleam = tmpdiff + tmpsaleamount
                                        If returnval >= tmpdiff Then 'ok
                                            'reduce seller balance
                                            If curtype = 1 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCE=CBALANCE-" & tmpdiff & " where ADDRESS='" & .Item(1).ToString & "'"
                                            If curtype = 2 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCET=CBALANCET-" & tmpdiff & " where ADDRESS='" & .Item(1).ToString & "'"
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                            tmpvalid = True
                                        Else
                                            'insufficient balance
                                            tmpvalid = False
                                        End If
                                    Else 'new sell is lower than or equal to existing sell
                                        'return difference to seller balance
                                        If tmpsaleamount - saleamount > 0 Then
                                            If curtype = 1 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCE=CBALANCE+" & (tmpsaleamount - saleamount) & " where ADDRESS='" & .Item(1).ToString & "'"
                                            If curtype = 2 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCET=CBALANCET+" & (tmpsaleamount - saleamount) & " where ADDRESS='" & .Item(1).ToString & "'"
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                        End If
                                        tmpsaleam = saleamount
                                        tmpvalid = True
                                    End If
                                    If tmpvalid = True Then
                                        'put in exchange & processed tables
                                        Dim tmpunitprice As Long
                                        tmpunitprice = (.Item(11) / (tmpsaleam / 100000000))
                                        cmd.CommandText = "UPDATE exchange_temp SET TXID='" & .Item(0) & "',SALEAMOUNT=" & tmpsaleam & ",OFFERAMOUNT=" & .Item(11) & ",MINFEE=" & .Item(12) & ",TIMELIMIT=" & .Item(13) & ",BLOCKTIME=" & .Item(5) & ",BLOCKNUM=" & .Item(6) & ",UNITPRICE=" & tmpunitprice & " WHERE FROMADD='" & .Item(1) & "'"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                        cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & tmpsaleam & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "updatesell" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & ")"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    Else
                                        cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & tmpsaleam & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "updatesell" & "'," & .Item(5) & "," & .Item(6) & ",0," & curtype & ")"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If

                                'new
                                If sellaction = 1 And .Item(10) > 0 And .Item(11) > 0 And .Item(13) > 0 Then 'sanity check we have all the necessary details
                                    'check there is not already an existing sell
                                    sqlquery = "SELECT COUNT(fromadd) FROM exchange_temp where FROMADD='" & .Item(1).ToString & "'"
                                    cmd.CommandText = sqlquery
                                    If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                    returnval = cmd.ExecuteScalar
                                    If returnval = 0 Then
                                        'there is no existing sell, we can go ahead - first check sellers balance
                                        If curtype = 1 Then sqlquery = "SELECT CBALANCE FROM balances_temp where ADDRESS='" & .Item(1).ToString & "'"
                                        If curtype = 2 Then sqlquery = "SELECT CBALANCET FROM balances_temp where ADDRESS='" & .Item(1).ToString & "'"
                                        cmd.CommandText = sqlquery
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                        'check if transaction amount is over senders balance, if not sell amount = senders balance
                                        If returnval > 0 And returnval < saleamount Then saleamount = returnval
                                        If returnval >= saleamount Then 'ok
                                            'reduce seller balance
                                            If curtype = 1 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCE=CBALANCE-" & saleamount & " where ADDRESS='" & .Item(1).ToString & "'"
                                            If curtype = 2 Then cmd.CommandText = "UPDATE balances_temp SET CBALANCET=CBALANCET-" & saleamount & " where ADDRESS='" & .Item(1).ToString & "'"
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                            'put in exchange & processed tables
                                            Dim tmpunitprice As Long
                                            tmpunitprice = (.Item(11) / (saleamount / 100000000))
                                            cmd.CommandText = "INSERT INTO exchange_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,UNITPRICE,RESERVED) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & .Item(10) & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "selloffer" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & "," & tmpunitprice & ",0)"
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & .Item(10) & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "selloffer" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & ")"
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                        Else
                                            'insufficient balance
                                            cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & .Item(10) & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "selloffer" & "'," & .Item(5) & "," & .Item(6) & ",0," & curtype & ")"
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                        End If
                                    Else
                                        'there is an existing sell, we can't create a new one - invalidate sell offer
                                        cmd.CommandText = "INSERT INTO transactions_processed_temp (TXID,FROMADD,SALEAMOUNT,OFFERAMOUNT,MINFEE,TIMELIMIT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & .Item(0) & "','" & .Item(1) & "'," & .Item(10) & "," & .Item(11) & "," & .Item(12) & "," & .Item(13) & ",'" & "selloffer" & "'," & .Item(5) & "," & .Item(6) & ",0," & curtype & ")"
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If .Item(4) = "acceptoffer" Then
                        'get currency type
                        Dim curtype As Integer = .Item(8)
                        'testing - force msc
                        If curtype = 2 Then
                            Dim matchedunitprice As Long
                            'get transaction amount
                            Dim purchaseamount As Long = .Item(14)
                            Dim toadd As String = .Item(2)
                            Dim matchedsaleamount, matchedofferamount, matchedminfee As Long
                            Dim matchedtimelimit As Integer
                            Dim txid As String
                            Dim matchingtx As String
                            'look for a sell at the reference address and verify there is only one sell (sanity check)
                            sqlquery = "SELECT COUNT(fromadd) FROM exchange_temp where FROMADD='" & .Item(2).ToString & "'"
                            cmd.CommandText = sqlquery
                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                            returnval = cmd.ExecuteScalar
                            If returnval > 1 Then
                                MsgBox("Sanity check has failed.  More than one sell for an address exists in the exchange table.  It is not safe to continue.  Exiting...")
                                End
                            End If
                            If returnval = 1 Then
                                'matched a sell offer - get details
                                sqlquery = "SELECT txid FROM exchange_temp where FROMADD='" & .Item(2).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                matchingtx = cmd.ExecuteScalar
                                sqlquery = "SELECT saleamount FROM exchange_temp where FROMADD='" & .Item(2).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                matchedsaleamount = cmd.ExecuteScalar
                                sqlquery = "SELECT timelimit FROM exchange_temp where FROMADD='" & .Item(2).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                matchedtimelimit = cmd.ExecuteScalar
                                sqlquery = "SELECT unitprice FROM exchange_temp where FROMADD='" & .Item(2).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                matchedunitprice = cmd.ExecuteScalar
                                sqlquery = "SELECT minfee FROM exchange_temp where FROMADD='" & .Item(2).ToString & "'"
                                cmd.CommandText = sqlquery
                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                matchedminfee = cmd.ExecuteScalar

                                'check if transaction amount is over saleamount, if not set purchaseamount = saleamount
                                'If purchaseamount > matchedsaleamount Then purchaseamount = matchedsaleamount

                                'check saleamount is not zero, if it is reject offer
                                If matchedsaleamount > 0 Then
                                    'is it our unconfirmed accept?
                                    If .Item(6) > 999998 Then
                                        sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'pendingoffer'," & .Item(5) & ",999999,1," & .Item(8) & ",'" & matchingtx & "')"
                                        cmd.CommandText = sqlquery
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                        sqlquery = "UPDATE exchange_temp SET saleamount=saleamount-" & purchaseamount & " where FROMADD='" & .Item(2).ToString & "'"
                                        cmd.CommandText = sqlquery
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                        sqlquery = "UPDATE exchange_temp SET reserved=reserved+" & purchaseamount & " where FROMADD='" & .Item(2).ToString & "'"
                                        cmd.CommandText = sqlquery
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        returnval = cmd.ExecuteScalar
                                    Else
                                        'look for payment
                                        Dim paymentamount As Double = (purchaseamount / 100000000) * (matchedunitprice / 100000000)
                                        Dim paymentpaid As Double = 0
                                        sqlquery = "select txid from exotransactions where blocknum > " & .Item(6) & " and blocknum <= " & (.Item(6) + matchedtimelimit) & " and vouts like '%" & .Item(2).ToString & "%'"
                                        cmd.CommandText = sqlquery
                                        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                        Dim adptSQLpaymenttxs As New SqlCeDataAdapter(cmd)
                                        Dim dspaymenttxs As New DataSet()
                                        adptSQLpaymenttxs.Fill(dspaymenttxs)
                                        With dspaymenttxs.Tables(0)
                                            For rowNum As Integer = 0 To .Rows.Count - 1
                                                With .Rows(rowNum)
                                                    'loop through all outputs in each transaction and total amount sent to reference address
                                                    Dim txn As txn = mlib.gettransaction(bitcoin_con, .Item(0))
                                                    If txn IsNot Nothing Then
                                                        For Each output As Vout In txn.result.vout
                                                            If output.scriptPubKey.type.ToString.ToLower = "pubkeyhash" And output.scriptPubKey.addresses(0) = toadd Then
                                                                paymentpaid = paymentpaid + output.value
                                                            End If
                                                        Next
                                                    End If
                                                End With
                                            Next
                                        End With
                                        'work out total amount of units covered by payment
                                        Dim unitspurchased As Long
                                        If paymentpaid = 0 Then
                                            unitspurchased = 0
                                        Else
                                            unitspurchased = ((paymentpaid * 100000000) / matchedunitprice) * 100000000
                                        End If
                                        If unitspurchased > 0 Then
                                            'are there enough available? 
                                            If unitspurchased > purchaseamount Then unitspurchased = purchaseamount
                                            'credit mastercoins
                                            sqlquery = "SELECT ADDRESS FROM balances_temp where ADDRESS='" & .Item(1).ToString & "'"
                                            cmd.CommandText = sqlquery
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                            If returnval = .Item(1).ToString Then
                                                If curtype = 1 Then sqlquery = "UPDATE balances_temp SET CBALANCE=CBALANCE+" & unitspurchased & " where ADDRESS='" & .Item(1).ToString & "'"
                                                If curtype = 2 Then sqlquery = "UPDATE balances_temp SET CBALANCET=CBALANCET+" & unitspurchased & " where ADDRESS='" & .Item(1).ToString & "'"
                                            Else
                                                If curtype = 1 Then sqlquery = "INSERT INTO balances_temp (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(1).ToString & "'," & unitspurchased & ",0,0,0)"
                                                If curtype = 2 Then sqlquery = "INSERT INTO balances_temp (ADDRESS,CBALANCE,CBALANCET, UBALANCE,UBALANCET) VALUES ('" & .Item(1).ToString & "',0," & unitspurchased & ",0,0)"
                                            End If
                                            cmd.CommandText = sqlquery
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar

                                            'reduce/delete sell 
                                            sqlquery = "SELECT RESERVED FROM exchange_temp where FROMADD='" & .Item(2).ToString & "'"
                                            cmd.CommandText = sqlquery
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar
                                            If IsDBNull(returnval) Then returnval = 0
                                            If returnval = 0 And unitspurchased = matchedsaleamount Then
                                                sqlquery = "DELETE FROM exchange_temp where FROMADD='" & .Item(2).ToString & "'"
                                                cmd.CommandText = sqlquery
                                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                returnval = cmd.ExecuteScalar
                                            Else
                                                sqlquery = "UPDATE exchange_temp SET saleamount=saleamount-" & unitspurchased & " where FROMADD='" & .Item(2).ToString & "'"
                                                cmd.CommandText = sqlquery
                                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                returnval = cmd.ExecuteScalar
                                            End If
                                            If unitspurchased < 10 Then
                                                '  MsgBox("break")
                                            End If
                                            'write transaction
                                            sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,OFFERAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(2) & "','" & .Item(1) & "'," & unitspurchased & "," & paymentpaid * 100000000 & ",'" & "purchase" & "'," & .Item(5) & "," & .Item(6) & ",1," & curtype & ",'" & matchingtx & "')"
                                            cmd.CommandText = sqlquery
                                            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                            returnval = cmd.ExecuteScalar

                                            'fully paid? if not still want to reserve some
                                            If unitspurchased < purchaseamount Then
                                                sqlquery = "UPDATE exchange_temp SET saleamount=saleamount-" & purchaseamount - unitspurchased & " where FROMADD='" & .Item(2).ToString & "'"
                                                cmd.CommandText = sqlquery
                                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                returnval = cmd.ExecuteScalar
                                                sqlquery = "UPDATE exchange_temp SET reserved=reserved+" & purchaseamount - unitspurchased & " where FROMADD='" & .Item(2).ToString & "'"
                                                cmd.CommandText = sqlquery
                                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                returnval = cmd.ExecuteScalar
                                                'add to transactions_processed 
                                                sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'pendingoffer'," & .Item(5) & "," & .Item(6) & ",1," & .Item(8) & ",'" & matchingtx & "')"
                                                cmd.CommandText = sqlquery
                                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                returnval = cmd.ExecuteScalar
                                            End If

                                        Else 'nothing purchased
                                            'get last blocknum - already expired?
                                            Dim dbposition2 As Integer
                                            dbposition2 = SQLGetSingleVal("SELECT MAX(BLOCKNUM) FROM processedblocks")
                                            If .Item(6) + matchedtimelimit < dbposition2 Then
                                                'expired 
                                                sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'expiredoffer'," & .Item(5) & "," & .Item(6) & ",1," & .Item(8) & ",'" & matchingtx & "')"
                                                cmd.CommandText = sqlquery
                                                If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                returnval = cmd.ExecuteScalar
                                            Else
                                                'look for unconfirmed payment
                                                Dim unconfirmedpaymentcount = SQLGetSingleVal("select count(txid) from exotransactions where blocknum > 999998 and vouts like '%" & .Item(2).ToString & "%'")
                                                If unconfirmedpaymentcount > 0 Then
                                                    sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'pendingoffer'," & .Item(5) & ",999999,1," & .Item(8) & ",'" & matchingtx & "')"
                                                    cmd.CommandText = sqlquery
                                                    If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                    returnval = cmd.ExecuteScalar
                                                Else
                                                    sqlquery = "UPDATE exchange_temp SET saleamount=saleamount-" & purchaseamount & " where FROMADD='" & .Item(2).ToString & "'"
                                                    cmd.CommandText = sqlquery
                                                    If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                    returnval = cmd.ExecuteScalar
                                                    sqlquery = "UPDATE exchange_temp SET reserved=reserved+" & purchaseamount & " where FROMADD='" & .Item(2).ToString & "'"
                                                    cmd.CommandText = sqlquery
                                                    If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                    returnval = cmd.ExecuteScalar
                                                    'add to transactions_processed 
                                                    sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'pendingoffer'," & .Item(5) & "," & .Item(6) & ",1," & .Item(8) & ",'" & matchingtx & "')"
                                                    cmd.CommandText = sqlquery
                                                    If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                                    returnval = cmd.ExecuteScalar
                                                End If
                                            End If
                                        End If
                                    End If
                                Else
                                    'no MSC available in matched sell offer, reject accept
                                    sqlquery = "INSERT INTO transactions_processed_temp (TXID,FROMADD,TOADD,PURCHASEAMOUNT,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE,MATCHINGTX) VALUES ('" & .Item(0) & "','" & .Item(1) & "','" & .Item(2) & "'," & .Item(14) & ",'rejectedoffer'," & .Item(5) & "," & .Item(6) & ",1," & .Item(8) & ",'" & matchingtx & "')"
                                    cmd.CommandText = sqlquery
                                    If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
                                    returnval = cmd.ExecuteScalar
                                End If
                            End If
                        End If
                    End If
                End With
            Next
        End With
        'drop real tables and copy from temp
        cmd.CommandText = "drop table exchange"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "create table [exchange] ([txid] [nvarchar](100) not null, [fromadd] [nvarchar](100) not null, [type] [nvarchar](100) not null, [blocktime] [bigint] not null, [blocknum] [int] not null, [valid] [bit] not null, [curtype] [int] not null, [saleamount] [bigint] null, [offeramount] [bigint] null, [minfee] [bigint] null, [timelimit] [int] null, [purchaseamount] [bigint] null, [unitprice] [bigint] not null, [reserved] [bigint] null)"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "INSERT INTO exchange select * FROM exchange_temp"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "drop table balances"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "create table balances (ADDRESS nvarchar(100) not null, CBALANCE bigint not null default 0, CBALANCET bigint not null default 0, UBALANCE bigint not null default 0, UBALANCET bigint not null default 0)"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "INSERT INTO balances select * from balances_temp"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "drop table transactions_processed"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "create table transactions_processed (txid nvarchar(100) not null, fromadd nvarchar(100) not null, toadd nvarchar(100) null, value bigint null, type nvarchar(100) not null, blocktime bigint not null, blocknum int not null, valid bit not null, curtype int not null, ID int IDENTITY(1,1), saleamount bigint null, offeramount bigint null, minfee bigint null, timelimit int null, purchaseamount bigint null, matchingtx nvarchar(100) null)"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery
        cmd.CommandText = "INSERT INTO transactions_processed (txid,fromadd,toadd,value,type,blocktime,blocknum,valid,curtype,saleamount,offeramount,minfee,timelimit,purchaseamount,matchingtx) select txid,fromadd,toadd,value,type,blocktime,blocknum,valid,curtype,saleamount,offeramount,minfee,timelimit,purchaseamount,matchingtx from transactions_processed_temp order by id asc"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        returnval = cmd.ExecuteNonQuery

        'update address balances
        'enumarate bitcoin addresses
        If debuglevel > 0 Then workthread.ReportProgress(0, My.Resources.workerdebug4)
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
            workthread.ReportProgress(0, My.Resources.workererror4 & vbCrLf & My.Resources.workerstatus2)
            Exit Sub
        End Try

        balmsc = 0
        baltmsc = 0
        balbtc = 0
        balumsc = 0
        balutmsc = 0
        balrestmsc = 0

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

            sqlquery = "SELECT RESERVED FROM exchange where FROMADD='" & row.Item(0) & "'"
            Dim addresbal As Long = SQLGetSingleVal(sqlquery)
            balrestmsc = balrestmsc + addresbal
            sqlquery = "SELECT SALEAMOUNT FROM exchange where FROMADD='" & row.Item(0) & "'"
            Dim addresbal2 As Long = SQLGetSingleVal(sqlquery)
            balrestmsc = balrestmsc + addresbal2

            balbtc = balbtc + row.item(1)

        Next
        'update history - use temp table to reduce ui lag while updating
        thistorylist.Clear()
        For Each row In taddresslist.Rows
            sqlquery = "SELECT txid,valid,fromadd,toadd,curtype,value,blocknum,type,saleamount,purchaseamount,blocktime FROM transactions_processed where (FROMADD='" & row.Item(0) & "' or TOADD='" & row.item(0) & "')"
            cmd.CommandText = sqlquery
            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
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
                        If .Item(6) = 999999 Then valimg = My.Resources.uncof 'unconfirmed tx
                        If Not IsDBNull(.Item(2)) Then
                            If row.item(0) = .Item(2) Then dirimg = My.Resources.out1
                        End If
                        If Not IsDBNull(.Item(3)) Then
                            If row.item(0) = .Item(3) Then dirimg = My.Resources.in1
                        End If
                        Dim dtdatetime As System.DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
                        If .Item(6) = 999999 Then dtdatetime = New DateTime(9999, 12, 30, 0, 0, 0, 0) 'unconfirmed tx
                        dtdatetime = dtdatetime.AddSeconds(.Item(10)).ToLocalTime()
                        Dim txtype As String
                        Dim amo As Double
                        If .Item(7) = "simple" Then
                            txtype = "Send"
                            amo = .Item(5) / 100000000
                        End If
                        If .Item(7) = "pendingoffer" Then
                            txtype = "Buy (Pending)"
                            amo = .Item(9) / 100000000
                        End If
                        If .Item(7) = "expiredoffer" Then
                            txtype = "Buy (Expired)"
                            amo = .Item(9) / 100000000
                        End If
                        If .Item(7) = "rejectedoffer" Then
                            txtype = "Buy (Rejected)"
                            amo = .Item(9) / 100000000
                        End If
                        If .Item(7) = "selloffer" Then
                            txtype = "Sell (Open)"
                            amo = .Item(8) / 100000000
                        End If
                        If .Item(7) = "updatesell" Then
                            txtype = "Sell (Update)"
                            amo = .Item(8) / 100000000
                        End If
                        If .Item(7) = "cancelsell" Then
                            txtype = "Sell (Cancel)"
                            amo = 0
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
                            amo = .Item(9) / 100000000
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
                        thistorylist.Rows.Add(valimg, dirimg, dtdatetime, fadd, tadd, txtype, cur, amo)
                    End With
                Next

            End With
        Next

        'perform bug check
        workthread.ReportProgress(0, My.Resources.workerdebug4)
        sqlquery = "select count(address) from balances group by ADDRESS HAVING COUNT(*) > 1"
        cmd.CommandText = sqlquery
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        Dim adptSQL3 As New SqlCeDataAdapter(cmd)
        Dim ds3 As New DataSet()
        adptSQL3.Fill(ds3)
        If ds3.Tables(0).Rows.Count > 0 Then
            MsgBox(My.Resources.cerror & vbCrLf & vbCrLf & My.Resources.messageinfo3)
            Application.Exit()
        End If
        'done

        con.Close()



        'exchange scan
        Dim econ As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
        Dim cmd2 As New SqlCeCommand()
        cmd2.Connection = econ
        econ.Open()
        'calculate values for chart
        'get lastblock unix time
        cmd2.CommandText = "select MAX(blocktime) from processedblocks"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
        Dim curtime = cmd2.ExecuteScalar
        Dim hval(62) As Double
        Dim lval(62) As Double
        Dim cval(62) As Double
        Dim low(31) As Double
        Dim high(31) As Double
        Dim open(31) As Double
        Dim close(31) As Double
        Dim dt(31) As String
        'loop through last 60 days
        For i = 60 To 2 Step -2
            Dim starttime = curtime - (86400 * (i))
            Dim endtime = curtime - (86400 * (i - 2))
            'get days high
            cmd2.CommandText = "select purchaseamount,offeramount,blocktime from transactions_processed where type='purchase' AND blocktime>" & starttime.ToString & " AND blocktime<" & endtime.ToString
            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
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
                If hval(i) = 0 Then hval(i) = 0.0001
                If lval(i) = 99999999 Or lval(i) = 0 Then lval(i) = 0.0001
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
                If x > 1 And x < 21 And y > 174 And y < 189 Then
                    gcimg.SetPixel(x, y, Color.FromArgb(37, 37, 38))
                End If
            Next x
        Next y
        gcimg.SetPixel(21, 183, Color.FromArgb(81, 81, 81))
        gcimg.SetPixel(21, 184, Color.FromArgb(81, 81, 81))
        gcimg.SetPixel(21, 185, Color.FromArgb(81, 81, 81))
        picpricehistory.Image = gcimg

        'open orders
        'sells first
        cmd2.CommandText = "select txid,fromadd,saleamount,offeramount,purchaseamount,curtype,type,reserved,blocknum from exchange where type='selloffer'"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
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
                    Dim tmpunit As Double = (.Item(3) / .Item(2))
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
        cmd2.CommandText = "select txid,fromadd,toadd,purchaseamount,curtype,type,blocknum,matchingtx from transactions_processed where type='pendingoffer'"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
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
        cmd2.CommandText = "select txid,fromadd,saleamount,offeramount,curtype,unitprice from exchange where type='selloffer' order by unitprice ASC"
        If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL: " & cmd.CommandText)
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

        varsyncronized = True
        varsyncblock = rpcblock
        workthread.ReportProgress(0, My.Resources.workerblockscan6)

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
            If debuglevel > 1 Then workthread.ReportProgress(0, My.Resources.workerdebugSQL & " " & sqlquery)
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
            workthread.ReportProgress(0, My.Resources.workererror5 & " " & vbCrLf & e.Message.ToString & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
            Return 99
        End Try
    End Function

    Private Sub workthread_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles workthread.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            txtdebug.AppendText(vbCrLf & My.Resources.workererror6 & " " & e.Error.Message)
            txtdebug.AppendText(vbCrLf & My.Resources.workerdebug6)
            syncicon.Visible = True
            lsyncing.Visible = True
            lsyncing.Text = My.Resources.notsynchronized
            syncicon.Image = My.Resources.redcross
            poversync.Image = My.Resources.redcross
            loversync.Text = My.Resources.notsynchronized
            Exit Sub
        End If

        txtdebug.AppendText(vbCrLf & My.Resources.txtdebugexit)
        UIrefresh.Enabled = True
        If varsyncronized = True Then
            syncicon.Image = My.Resources.sync
            syncicon.Visible = False
            lsyncing.Visible = False
            lsyncing.Text = My.Resources.synchronizing
            poversync.Image = My.Resources.green_tick
            loversync.Text = My.Resources.synchronizedlastblock & " " & varsyncblock.ToString & "."
        Else
            syncicon.Visible = True
            syncicon.Image = My.Resources.redcross
            lsyncing.Visible = True
            poversync.Image = My.Resources.redcross
            loversync.Text = My.Resources.notsynchronized
            lsyncing.Text = My.Resources.notsynchronized
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
            If lnkaddsort.Text = My.Resources.addressalpha Then dgvaddresses.Sort(dgvaddresses.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            If lnkaddsort.Text = My.Resources.balancebtc Then dgvaddresses.Sort(dgvaddresses.Columns(1), System.ComponentModel.ListSortDirection.Descending)
            If lnkaddsort.Text = My.Resources.balancemsc Then dgvaddresses.Sort(dgvaddresses.Columns(3), System.ComponentModel.ListSortDirection.Descending)
            If lnkaddfilter.Text = My.Resources.nofilteractive Then addresslist.DefaultView.RowFilter = ""
            If lnkaddfilter.Text = My.Resources.emptybalances Then addresslist.DefaultView.RowFilter = "btcamount > 0 or mscamount > 0 or tmscamount > 0"

        Catch ex As Exception
            MsgBox(My.Resources.addresslistexception & vbCrLf & ex.Message)
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
            MsgBox(My.Resources.currencylistexception & vbCrLf & ex.Message)
        End Try
        Try
            dgvhistory.DataSource = Nothing
            dgvhistory.Refresh()
            'load historylist with thistorylist
            historylist.Clear()
            For Each row In thistorylist.Rows
                historylist.Rows.Add(row.item(0), row.item(1), row.item(2), row.item(3), row.item(4), row.item(5), row.item(6), row.item(7))
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
            dgvcolumn.Width = 185
            dgvcolumn = dgvhistory.Columns(4)
            dgvcolumn.Width = 185
            dgvcolumn = dgvhistory.Columns(5)
            dgvcolumn.Width = 75
            dgvcolumn = dgvhistory.Columns(6)
            dgvcolumn.Width = 94
            dgvcolumn = dgvhistory.Columns(7)
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"

            If lnkhistorysort.Text = My.Resources.highestvalue Then dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Descending)
            If lnkhistorysort.Text = My.Resources.lowestvalue Then dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Ascending)
            If lnkhistorysort.Text = My.Resources.recentfirst Then dgvhistory.Sort(dgvhistory.Columns(2), System.ComponentModel.ListSortDirection.Descending)
            dgvcolumn.Width = 140
        Catch ex As Exception
            MsgBox(My.Resources.historylistexception & vbCrLf & ex.Message)
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
        lbldextotalbtc.Text = balbtc.ToString & " BTC"
        lbldextotalcur.Text = (baltmsc / 100000000).ToString("######0.00######") & " TMSC"
        lbldexrescur.Text = (balrestmsc / 100000000).ToString("######0.00######") & " TMSC"
        dgvselloffer.CurrentCell = Nothing
        dgvselloffer.ClearSelection()
        lnknofocus.Focus()
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
        loversync.Text = My.Resources.synchronizing
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
                MsgBox(My.Resources.configFileError)
            End If
        Else
            MsgBox(My.Resources.completeFields)
        End If
    End Sub

    Private Sub txtrpcserver_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcserver.Enter
        txtrpcserver.BorderStyle = BorderStyle.FixedSingle
        If txtrpcserver.Text = My.Resources.notconfigured Then txtrpcserver.Text = ""
    End Sub
    Private Sub txtrpcserver_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcserver.Leave
        txtrpcserver.BorderStyle = BorderStyle.None
        If txtrpcserver.Text = "" Then txtrpcserver.Text = My.Resources.notconfigured
    End Sub
    Private Sub txtrpcport_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcport.Enter
        txtrpcport.BorderStyle = BorderStyle.FixedSingle
        If txtrpcport.Text = My.Resources.notconfigured Then txtrpcport.Text = ""
    End Sub
    Private Sub txtrpcport_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcport.Leave
        txtrpcport.BorderStyle = BorderStyle.None
        If txtrpcport.Text = "" Then txtrpcport.Text = My.Resources.notconfigured
    End Sub
    Private Sub txtrpcuser_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcuser.Enter
        txtrpcuser.BorderStyle = BorderStyle.FixedSingle
        If txtrpcuser.Text = My.Resources.notconfigured Then txtrpcuser.Text = ""
    End Sub
    Private Sub txtrpcuser_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcuser.Leave
        txtrpcuser.BorderStyle = BorderStyle.None
        If txtrpcuser.Text = "" Then txtrpcuser.Text = My.Resources.notconfigured
    End Sub
    Private Sub txtrpcpassword_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcpassword.Enter
        txtrpcpassword.BorderStyle = BorderStyle.FixedSingle
        If txtrpcpassword.Text = My.Resources.notconfigured Then txtrpcpassword.Text = ""
    End Sub
    Private Sub txtrpcpassword_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtrpcpassword.Leave
        txtrpcpassword.BorderStyle = BorderStyle.None
        If txtrpcpassword.Text = "" Then txtrpcpassword.Text = My.Resources.notconfigured
    End Sub


    Private Sub lnkaddsort_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkaddsort.LinkClicked
        If lnkaddsort.Text = My.Resources.addressalpha Then
            lnkaddsort.Text = My.Resources.balancebtc
            dgvaddresses.Sort(dgvaddresses.Columns(1), System.ComponentModel.ListSortDirection.Descending)
            Exit Sub
        End If
        If lnkaddsort.Text = My.Resources.balancebtc Then
            lnkaddsort.Text = My.Resources.balancemsc
            dgvaddresses.Sort(dgvaddresses.Columns(3), System.ComponentModel.ListSortDirection.Descending)
            Exit Sub
        End If
        If lnkaddsort.Text = My.Resources.addressalpha Then
            lnkaddsort.Text = My.Resources.addressalpha
            dgvaddresses.Sort(dgvaddresses.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
            Exit Sub
        End If
    End Sub

    Private Sub lnkaddfilter_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkaddfilter.LinkClicked
        If lnkaddfilter.Text = My.Resources.nofilteractive Then
            lnkaddfilter.Text = My.Resources.emptybalances
            addresslist.DefaultView.RowFilter = "btcamount > 0 or mscamount > 0 or tmscamount > 0"
            Exit Sub
        End If
        If lnkaddfilter.Text = My.Resources.emptybalances Then
            lnkaddfilter.Text = My.Resources.nofilteractive
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
                                ltestinfo.Text = My.Resources.testInfo1
                                ltestinfo.ForeColor = Color.Red
                                Exit Sub
                            End If
                        Catch
                            ltestinfo.Text = My.Resources.testInfo1
                            ltestinfo.ForeColor = Color.Red
                            Exit Sub
                        End Try
                        ltestinfo.Text = My.Resources.testInfo2
                        ltestinfo.ForeColor = Color.Lime
                    Else
                        'something has gone wrong
                        ltestinfo.Text = My.Resources.testError1
                        Exit Sub
                    End If
                Catch
                    Exit Sub
                End Try
            Else
                ltestinfo.Text = My.Resources.completeFields
            End If
        Catch ex As Exception
            MsgBox(My.Resources.exception & " " & ex.Message)
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
                            ltestinfo.Text = My.Resources.testInfo1
                            ltestinfo.ForeColor = Color.Red
                            Exit Sub
                        End If
                    Catch
                        ltestinfo.Text = My.Resources.testInfo1
                        ltestinfo.ForeColor = Color.Red
                        Exit Sub
                    End Try
                    ltestinfo.Text = My.Resources.testInfo2
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
                        MsgBox(My.Resources.configFileError)
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                ltestinfo.Text = My.Resources.testInfo3
                ltestinfo.ForeColor = Color.Red
                bfinish.Enabled = True
                btest.Enabled = True
                MsgBox(ex.Message)
                Exit Sub
            End Try
        Else
            ltestinfo.Text = My.Resources.completeFields
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
            lwalinfo.Text = My.Resources.more
            lwalinfo.ForeColor = Color.FromArgb(255, 192, 128)
            Exit Sub
        End If
        If Len(txtstartwalpass.Text) < 12 Then
            lwalinfo.Text = My.Resources.more & " " & My.Resources.more
            lwalinfo.ForeColor = Color.FromArgb(255, 192, 128)
            Exit Sub
        End If
        If Len(txtstartwalpass.Text) > 11 Then
            lwalinfo.Text = My.Resources.greatthanks
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

    Private Sub rsendtmsc_CheckedClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rsendtmsc.Click
        rsendmsc.Checked = False
        rsendtmsc.Checked = True
        rsendbtc.Checked = False
        comsendaddress.Enabled = True
        updateavail()
    End Sub

    Private Sub rsendbtc_CheckedClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rsendbtc.Click
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
        If denom = "" Then lsendavail.Text = My.Resources.selectsendaddress
        lsendavail.Text = My.Resources.available & " " & avail.ToString & denom
    End Sub
    Private Sub comsendaddress_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comsendaddress.SelectedIndexChanged
        updateavail()
    End Sub

    Private Sub txtsendamount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtsendamount.TextChanged
        If txtsendamount.Text <> "" Then
            If Val(txtsendamount.Text) > 0 And Val(txtsendamount.Text) <= avail Then
                lsendamver.Text = ""
            End If
            If Val(txtsendamount.Text) > 0 And Val(txtsendamount.Text) > avail Then
                lsendamver.Text = My.Resources.insufficientfunds
                lsendamver.ForeColor = Color.FromArgb(255, 192, 128)
            End If
        End If

    End Sub

    Private Sub bback_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bback.MouseUp

    End Sub

    Private Sub lnkhistorysort_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkhistorysort.LinkClicked
        If lnkhistorysort.Text = My.Resources.recentfirst Then
            lnkhistorysort.Text = My.Resources.highestvalue
            dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Descending)
            Exit Sub
        End If
        If lnkhistorysort.Text = My.Resources.highestvalue Then
            lnkhistorysort.Text = My.Resources.lowestvalue
            dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Ascending)
            Exit Sub
        End If
        If lnkhistorysort.Text = My.Resources.lowestvalue Then
            lnkhistorysort.Text = My.Resources.recentfirst
            dgvhistory.Sort(dgvhistory.Columns(2), System.ComponentModel.ListSortDirection.Descending)
            Exit Sub
        End If
    End Sub

    Private Sub lnkhistoryfilter_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkhistoryfilter.LinkClicked
        If lnkhistoryfilter.Text = My.Resources.nofilteractive Then
            lnkhistoryfilter.Text = My.Resources.onlymastercoin
            Exit Sub
        End If
        If lnkhistoryfilter.Text = My.Resources.onlymastercoin Then
            lnkhistoryfilter.Text = My.Resources.nofilteractive
            Exit Sub
        End If
    End Sub

    Private Sub bsignsend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsignsend.Click
        If Val(txtsendamount.Text) > 0 And Val(txtsendamount.Text) > avail Then
            lsendamver.Text = My.Resources.insufficientfunds
            lsendamver.ForeColor = Color.FromArgb(255, 192, 128)
            Exit Sub
        End If
        If Val(txtsendamount.Text) = 0 Then
            'nothing to send
            Exit Sub
        End If
        txtdebug.AppendText(vbCrLf & "DEBUG: Beginning simple send transaction")
        txtdebug.AppendText(vbCrLf & "===================================================================================")
        txsummary = ""
        senttxid = My.Resources.transactionnotsent
        'first validate recipient address
        If txtsenddest.Text <> "" Then
            txtdebug.AppendText(vbCrLf & "DEBUG: Requesting passphrase")
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
                MsgBox(My.Resources.btcsenddisabled)
                Exit Sub
                Try
                    Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, txtsenddest.Text, 0, 0))
                    If validater.result.isvalid = True Then 'address is valid
                        txsummary = My.Resources.invalidrecipient
                        'push out to bitcoin rpc to send the tx since we can use sendtoaddress for simple bitcoin tx
                        'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
                        Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
                        Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(btcpass.ToString), 15, 0)
                        btcpass = "" 'dispose of wallet passphrase
                        Dim txref As broadcasttx = JsonConvert.DeserializeObject(Of broadcasttx)(mlib.rpccall(bitcoin_con, "sendtoaddress", 2, txtsenddest.Text, amount, 0))
                        'check txref is not empty and display txref
                        If txref.result <> "" Then
                            txsummary = txsummary & vbCrLf & My.Resources.transactionsentid & " " & txref.result
                            'lsendtxinfo.Text = "Transaction sent, check viewer for TXID."
                            lsendtxinfo.ForeColor = Color.Lime
                            bsignsend.Enabled = False
                            Application.DoEvents()

                            If workthread.IsBusy <> True Then
                                UIrefresh.Enabled = False
                                syncicon.Visible = True
                                lsyncing.Text = "Synchronizing..."
                                lsyncing.Visible = True
                                poversync.Image = My.Resources.sync
                                loversync.Text = My.Resources.synchronizing

                                ' Start the workthread for the blockchain scanner
                                workthread.RunWorkerAsync()
                            End If
                            Exit Sub
                        Else
                            txsummary = txsummary & vbCrLf & My.Resources.failedsendtx
                        End If
                    Else
                        txsummary = txsummary & vbCrLf & My.Resources.failedbuildtx
                    End If
                Catch ex As Exception
                    MsgBox(My.Resources.exceptionthrown & " " & ex.Message)
                    Exit Sub
                End Try
                Exit Sub
            End If

            If rsendbtc.Checked = False Then
                Try
                    txtdebug.AppendText(vbCrLf & "DEBUG: Validating recipient address")
                    Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, txtsenddest.Text, 0, 0))
                    If validater.result.isvalid = True Then 'address is valid
                        txsummary = My.Resources.validrecipient
                        'push out to masterchest lib to encode the tx
                        txtdebug.AppendText(vbCrLf & "DEBUG: Calling library: mlib.encodetx, bitcoin_con, " & fromadd & ", " & toadd & ", " & curtype & ", " & amountlong.ToString)
                        Dim rawtx As String = mlib.encodetx(bitcoin_con, fromadd, toadd, curtype, amountlong)
                        'is rawtx empty
                        If rawtx = "" Then
                            txtdebug.AppendText(vbCrLf & "ERROR: Raw transaction is empty - stopping")
                            txtdebug.AppendText(vbCrLf & "===================================================================================")
                            txtdebug.AppendText(vbCrLf & "DEBUG: Ending send payment transaction")
                            Exit Sub
                        End If
                        'decode the tx in the viewer
                        txtdebug.AppendText(vbCrLf & "DEBUG: Raw transaction hex: " & vbCrLf & rawtx)
                        txsummary = txsummary & vbCrLf & "Raw transaction hex:" & vbCrLf & rawtx & vbCrLf & "Raw transaction decode:" & vbCrLf & mlib.rpccall(bitcoin_con, "decoderawtransaction", 1, rawtx, 0, 0)
                        'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
                        txtdebug.AppendText(vbCrLf & "DEUBG: Unlocking wallet")
                        Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
                        Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(btcpass.ToString), 15, 0)
                        btcpass = ""
                        'try and sign transaction
                        txtdebug.AppendText(vbCrLf & "DEUBG: Attempting signing")
                        Dim signedtxn As signedtx = JsonConvert.DeserializeObject(Of signedtx)(mlib.rpccall(bitcoin_con, "signrawtransaction", 1, rawtx, 0, 0))
                        If signedtxn.result.complete = True Then
                            txsummary = txsummary & vbCrLf & "Signing appears successful."
                            txtdebug.AppendText(vbCrLf & "DEUBG: Attempting broadcast")
                            Dim broadcasttx As broadcasttx = JsonConvert.DeserializeObject(Of broadcasttx)(mlib.rpccall(bitcoin_con, "sendrawtransaction", 1, signedtxn.result.hex, 0, 0))
                            If broadcasttx.result <> "" Then
                                txtdebug.AppendText(vbCrLf & "DEUBG: Transaction sent - " & broadcasttx.result.ToString)
                                txtdebug.AppendText(vbCrLf & "===================================================================================")
                                txtdebug.AppendText(vbCrLf & "DEBUG: Ending simple send transaction")
                                txsummary = txsummary & vbCrLf & "Transaction sent, ID: " & broadcasttx.result.ToString
                                sentfrm.lsent.Text = "transaction sent"
                                senttxid = broadcasttx.result.ToString
                                'lsendtxinfo.Text = "Transaction sent, check viewer for TXID."
                                'lsendtxinfo.ForeColor = Color.Lime
                                bsignsend.Enabled = False
                                Application.DoEvents()
                                If workthread.IsBusy <> True Then
                                    UIrefresh.Enabled = False
                                    syncicon.Visible = True
                                    lsyncing.Visible = True
                                    poversync.Image = My.Resources.sync
                                    loversync.Text = My.Resources.synchronizing
                                    lsyncing.Text = My.Resources.synchronizing
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
                                txtdebug.AppendText(vbCrLf & "ERROR: Unknown error sending transaction")
                                txtdebug.AppendText(vbCrLf & "===================================================================================")
                                txtdebug.AppendText(vbCrLf & "DEBUG: Ending simple send transaction")
                                Exit Sub
                            End If

                        Else
                            txsummary = txsummary & vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct."
                            txtdebug.AppendText(vbCrLf & "ERROR: Failed to sign transaction.  Ensure wallet passphrase is correct")
                            txtdebug.AppendText(vbCrLf & "===================================================================================")
                            txtdebug.AppendText(vbCrLf & "DEBUG: Ending simple send transaction")
                            sentfrm.lsent.Text = "transaction failed"
                            sentfrm.ShowDialog()
                            Exit Sub
                        End If
                    Else
                        txsummary = "Build transaction failed.  Recipient address is not valid."
                        txtdebug.AppendText(vbCrLf & "ERROR: Build transaction failed.  Recipient address is not valid")
                        txtdebug.AppendText(vbCrLf & "===================================================================================")
                        txtdebug.AppendText(vbCrLf & "DEBUG: Ending simple send transaction")
                        sentfrm.lsent.Text = "transaction failed"
                        sentfrm.ShowDialog()
                        Exit Sub
                    End If
                    sentfrm.ShowDialog()
                Catch ex As Exception
                    MsgBox(My.Resources.exceptionthrown & " " & ex.Message)
                    txtdebug.AppendText(vbCrLf & "ERROR: Exception thrown: " & ex.Message)
                    txtdebug.AppendText(vbCrLf & "===================================================================================")
                    txtdebug.AppendText(vbCrLf & "DEBUG: Ending simple send transaction")
                    sentfrm.ShowDialog()
                End Try
            End If

        End If
    End Sub

    Private Sub bsendnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsendnew.Click
        txtsendamount.Text = "0.00000000"
        sentfrm.txtviewer.Text = ""
        lsendamver.Visible = False
        txtsenddest.Text = ""
        lsendtxinfo.Text = ""
        bsignsend.Enabled = True
    End Sub

    Private Sub dgvhistory_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvhistory.DataError
        'trap error when we clear list bound to dgv temporarily
    End Sub

    Private Sub dgvaddresses_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvaddresses.DataError
        'trap error when we clear list bound to dgv temporarily
    End Sub

    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
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

    Private Sub cbLocale_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbLocale.SelectedIndexChanged
        For Each kv In locales
            If Thread.CurrentThread.CurrentUICulture.Name <> kv.Key And kv.Value = cbLocale.SelectedItem Then
                activeCulture = New CultureInfo(kv.Key)
                Dim style = NumberStyles.AllowDecimalPoint Or NumberStyles.AllowThousands

                LocalizeNumericControls(Me, style, activeCulture)

                ' Set culture for thread after numerics were converted
                Thread.CurrentThread.CurrentCulture = activeCulture
                Thread.CurrentThread.CurrentUICulture = activeCulture

                'Full namespace required, type of this
                Dim rm As New System.Resources.ResourceManager("Masterchest_Wallet.Resources", GetType(Form1).Assembly)
                LocalizeTextControls(Me, rm)
                LocalizeTextControls(passfrm, rm)
                LocalizeTextControls(sentfrm, rm)

                ' Update number formatting in grids for new culture
                dgvaddresses.Refresh()
                dgvcurrencies.Refresh()
                dgvhistory.Refresh()
                dgvopenorders.Refresh()
                dgvselloffer.Refresh()

                My.Settings.culture = kv.Key
                My.Settings.Save()
            End If
        Next
    End Sub

    Private Sub LocalizeTextControls(ByVal control As Control, ByVal rm As System.Resources.ResourceManager)
        If control.Tag IsNot Nothing And TypeOf control.Tag Is LocaleTag Then
            If control.Tag = LocaleTag.Text Then
                control.Text = rm.GetString(control.Name)
            End If
        End If
        For Each child As Control In control.Controls
            LocalizeTextControls(child, rm)
        Next
    End Sub

    Private Sub LocalizeNumericControls(ByVal control As Control, ByVal style As NumberStyles, ByVal newCulture As CultureInfo)
        If control.Tag IsNot Nothing And TypeOf control.Tag Is LocaleTag Then
            If control.Tag = LocaleTag.Numeric Then
                Dim amount As Decimal
                Dim vals = control.Text.Trim.Split(" ")
                If Decimal.TryParse(vals(0), style, Thread.CurrentThread.CurrentUICulture, amount) Then
                    Dim decimals = vals(0).Length - control.Text.LastIndexOf(Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator) - 1
                    Dim unit = Nothing
                    If vals.Length = 2 Then
                        unit = " " + vals(1)
                    End If
                    control.Text = amount.ToString("f" + decimals.ToString(), newCulture) + unit
                End If
            End If
        End If
        For Each child In control.Controls
            LocalizeNumericControls(child, style, newCulture)
        Next
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
End Class

