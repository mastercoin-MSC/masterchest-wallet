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
Imports System.IO
Imports System.Data.SqlServerCe
Imports System.Configuration
Imports System.Security.Cryptography
Imports Masterchest.mlib
Imports Org.BouncyCastle.Math.EC

Public Class Form1
    Public bitcoin_con As New bitcoinrpcconnection
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
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown, RectangleShape1.MouseDown, psetup.MouseDown, pwelcome.MouseDown, paddresses.MouseDown, pdebug.MouseDown, poverview.MouseDown, psend.MouseDown, psettings.MouseDown
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
        'disclaimer
        MsgBox("DISCLAIMER: " & vbCrLf & vbCrLf & "This software is pre-release software for testing only." & vbCrLf & vbCrLf & "The protocol and transaction processing rules for Mastercoin are still under active development and are subject to change in future." & vbCrLf & vbCrLf & "DO NOT USE IT WITH A LARGE AMOUNT OF MASTERCOINS AND/OR BITCOINS.  YOU MAY LOSE ALL YOUR COINS." & vbCrLf & vbCrLf & "A fraction of a bitcoin and a few mastercoins are the suggested testing amounts.")
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
            Dim FINAME As String = Application.StartupPath & "\wallet.cfg"
            If System.IO.File.Exists(FINAME) = True Then
                Dim objreader As New System.IO.StreamReader(FINAME)
                Dim line As String
                Do
                    line = objreader.ReadLine()
                    If Len(line) > 14 Then
                        Select Case line.ToLower.Substring(0, 15)
                            Case "bitcoinrpcserv="
                                bitcoin_con.bitcoinrpcserver = line.ToLower.Substring(15, Len(line) - 15)
                                txtrpcserver.Text = bitcoin_con.bitcoinrpcserver
                            Case "bitcoinrpcport="
                                bitcoin_con.bitcoinrpcport = Val(line.ToLower.Substring(15, Len(line) - 15))
                                txtrpcport.Text = bitcoin_con.bitcoinrpcport.ToString
                            Case "bitcoinrpcuser="
                                bitcoin_con.bitcoinrpcuser = line.ToLower.Substring(15, Len(line) - 15)
                                txtrpcuser.Text = bitcoin_con.bitcoinrpcuser
                            Case "bitcoinrpcpass="
                                bitcoin_con.bitcoinrpcpassword = line.ToLower.Substring(15, Len(line) - 15)
                                txtrpcpassword.Text = "********************"
                            Case "gettingstarted#"
                                'gettingstardscreen
                                psetup.Visible = True
                                Exit Sub
                        End Select
                    End If
                Loop Until line Is Nothing
                objreader.Close()
            End If
        Catch ex As Exception
            MsgBox("Exception reading configuration : " & ex.Message)
        End Try
        'handle send currency buttons
        rsendmsc.Checked = True
        rsendtmsc.Checked = False
        'show welcome panel
        pwelcome.Visible = True
    End Sub
    Private Sub updateui()
        Me.Refresh()
        Application.DoEvents()
    End Sub

    Private Sub teststartup()
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

        'enumarate bitcoin addresses
        If debuglevel > 0 Then workthread.ReportProgress(0, "DEBUG: Enumerating addresses...")
        Try
            Dim addresses As List(Of btcaddressbal) = mlib.getaddresses(bitcoin_con)
            For Each address In addresses
                addresslist.Rows.Add(address.address, address.amount, 0, 0)
            Next

            'Dim balbitcoin As getbal = JsonConvert.DeserializeObject(Of getbal)(rpccall("getbalance", 0, 0, True))
            'balbtc = balbitcoin.result
        Catch ex As Exception
            MsgBox(ex.Message)
            workthread.ReportProgress(0, "ERROR: Enumerating addresses did not complete properly." & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
            Exit Sub
        End Try
        startup = False
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
    Private Sub bcurrencies_Click(sender As System.Object, e As System.EventArgs) Handles bcurrencies.Click
        If curscreen <> "2" Then
            activatecurrencies()
            lastscreen = lastscreen & "2"
        End If
    End Sub

    Private Sub bsend_Click(sender As System.Object, e As System.EventArgs) Handles bsend.Click
        If curscreen <> "3" Then
            activatesend()
            lastscreen = lastscreen & "3"
        End If
    End Sub
    Private Sub baddresses_Click(sender As System.Object, e As System.EventArgs) Handles baddresses.Click
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
    Private Sub bsettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsettings.Click
        If curscreen <> "6" Then
            activatesettings()
            lastscreen = lastscreen & "6"
        End If
    End Sub
    Private Sub bdebug_Click(sender As System.Object, e As System.EventArgs) Handles bdebug.Click
        If curscreen <> "7" Then
            activatedebug()
            lastscreen = lastscreen & "7"
        End If
    End Sub
    Private Sub checkdebugscroll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles checkdebugscroll.CheckedChanged
        If checkdebugscroll.Checked = True Then txtdebug.ScrollBars = ScrollBars.Vertical
        If checkdebugscroll.Checked = False Then txtdebug.ScrollBars = ScrollBars.None
    End Sub
    Private Sub lnkdebug_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkdebug.LinkClicked
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
    Private Sub workthread_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles workthread.ProgressChanged
        Me.txtdebug.AppendText(vbCrLf & e.UserState.ToString)
    End Sub

    Private Sub workthread_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles workthread.DoWork
        If startup = True Then
            workthread.ReportProgress(0, "DEBUG: Thread 'startup' starting...")
            teststartup()
            Exit Sub
        End If
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
        If dbposition > 249497 Then dbposition = dbposition - 1 Else dbposition = 249497 'roll database back one block for safety
        'delete transactions after dbposition block
        Dim txdeletedcount = SQLGetSingleVal("DELETE FROM transactions WHERE BLOCKNUM > " & dbposition - 1)
        Dim blockdeletedcount = SQLGetSingleVal("DELETE FROM processedblocks WHERE BLOCKNUM > " & dbposition - 1)
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
                    If mlib.ismastercointx(bitcoin_con, txarray(j)) = True Then
                        workthread.ReportProgress(0, "BLOCKSCAN: Found MSC transaction: " & txarray(j))
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
                        If Not IsNothing(txdetails) Then Dim dbwrite2 As Integer = SQLGetSingleVal("INSERT INTO transactions (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) VALUES ('" & txdetails.txid & "','" & txdetails.fromadd & "','" & txdetails.toadd & "'," & txdetails.value & ",'" & txdetails.type & "'," & txdetails.blocktime & "," & blocknum & "," & txdetails.valid & "," & txdetails.curtype & ")")
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
                If mlib.ismastercointx(bitcoin_con, intermedarray(j).hash) = True Then
                    workthread.ReportProgress(0, "BLOCKSCAN: Found MSC transaction: " & intermedarray(j).hash)
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
        Try
            'do all generate transactions and calculate initial balances
            Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
            Dim cmd As New SqlCeCommand()
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "delete from transactions_processed"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from balances"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "insert into transactions_processed (TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE) SELECT TXID,FROMADD,TOADD,VALUE,TYPE,BLOCKTIME,BLOCKNUM,VALID,CURTYPE from transactions where type='generate'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "insert into balances (address, cbalance, cbalancet) SELECT TOADD,SUM(VALUE),SUM(VALUE) from transactions_processed where curtype = 1 group by toadd"
            cmd.ExecuteNonQuery()

            'go through simple transactions, check validity and apply to balances
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
                            cmd.CommandText = sqlquery
                            returnval = cmd.ExecuteScalar
                            'check if transaction amount is over senders balance
                            If returnval > txamount Then 'ok
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
                    End With
                Next
            End With

            'update address balances
            'enumarate bitcoin addresses
            If debuglevel > 0 Then workthread.ReportProgress(0, "DEBUG: Enumerating addresses...")
            Try
                Dim addresses As List(Of btcaddressbal) = mlib.getaddresses(bitcoin_con)
                taddresslist.Clear()
                For Each address In addresses
                    taddresslist.Rows.Add(address.address, address.amount, 0, 0)
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

                If hrtbal <> row.Item(2) Then row.Item(2) = hrtbal
                balbtc = balbtc + row.item(1)
            Next

            'update history - use temp table to reduce ui lag while updating
            thistorylist.Clear()
            For Each row In taddresslist.Rows
                sqlquery = "SELECT txid,valid,fromadd,toadd,curtype,value,blocktime FROM transactions_processed where FROMADD='" & row.Item(0) & "' or TOADD='" & row.item(0) & "'"
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
            'done
            
            con.Close()

        Catch ex As Exception
            workthread.ReportProgress(0, "ERROR: Processing transactions threw an exception of: " & ex.Message.ToString & " - Exiting thread...")
            Exit Sub
        End Try
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
            Dim con As New SqlCeConnection("data source=" & Application.StartupPath & "\wallet.sdf; password=" & walpass)
            Dim cmd As New SqlCeCommand()
            Dim returnval
            If debuglevel > 1 Then workthread.ReportProgress(0, "DEBUG: SQL " & sqlquery)
            cmd.Connection = con
            con.Open()
            cmd.CommandText = sqlquery
            returnval = cmd.ExecuteScalar
            If Not IsDBNull(returnval) Then Return returnval
            con.Close()
        Catch e As Exception
            'exception thrown connecting
            workthread.ReportProgress(0, "ERROR: Connection to database threw an exception of: " & vbCrLf & e.Message.ToString & vbCrLf & "STATUS: UI thread will remain but blockchain scanning thread will now exit.")
            Return 99
        End Try
    End Function

    Private Sub workthread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles workthread.RunWorkerCompleted
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
        For Each row In addresslist.Rows
            If Not comsendaddress.Items.Contains(row.item(0).ToString) Then
                comsendaddress.Items.Add(row.item(0).ToString)
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
            dgvcolumn.Width = 322
            dgvcolumn = dgvaddresses.Columns(1)
            dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn.Width = 90
            dgvcolumn = dgvaddresses.Columns(2)
            dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn.Width = 90
            dgvcolumn = dgvaddresses.Columns(3)
            dgvcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvcolumn.DefaultCellStyle.Format = "########0.00######"
            dgvcolumn.Width = 90
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
            currencylist.Rows.Add("Bitcoin", balbtc.ToString, 0)
            dgvcurrencies.DataSource = currencylist
            Dim dgvcolumn As New DataGridViewColumn
            dgvcolumn = dgvcurrencies.Columns(0)
            dgvcolumn.Width = 295
            dgvcolumn = dgvcurrencies.Columns(1)
            dgvcolumn.Width = 162
            dgvcolumn.DefaultCellStyle.Format = "#0.00000000"
            dgvcolumn = dgvcurrencies.Columns(2)
            dgvcolumn.DefaultCellStyle.Format = "#0.00000000"
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
            dgvcolumn.Width = 114
            dgvcolumn = dgvhistory.Columns(3)
            dgvcolumn.Width = 90
            dgvcolumn = dgvhistory.Columns(4)
            dgvcolumn.Width = 230
            dgvcolumn = dgvhistory.Columns(5)
            dgvcolumn.Width = 94
            dgvcolumn = dgvhistory.Columns(6)
            dgvcolumn.DefaultCellStyle.Format = "#0.00000000"
            dgvcolumn.Width = 94
            If lnkhistorysort.Text = "Highest Value" Then dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Descending)
            If lnkhistorysort.Text = "Lowest Value" Then dgvhistory.Sort(dgvhistory.Columns(6), System.ComponentModel.ListSortDirection.Ascending)
            If lnkhistorysort.Text = "Recent First" Then dgvhistory.Sort(dgvhistory.Columns(2), System.ComponentModel.ListSortDirection.Descending)
        Catch ex As Exception
            MsgBox("historylist exception" & vbCrLf & ex.Message)
        End Try

        updateui()
        Application.DoEvents()
        If asyncjump = True Then
            asyncjump = False
            poversync.Image = My.Resources.sync
            loversync.Text = "Syncronizing..."
            Application.DoEvents()
            If workthread.IsBusy <> True Then
                ' Start the workthread for the blockchain scanner
                workthread.RunWorkerAsync()
            End If
        End If
    End Sub


    Private Sub dgvaddresses_CellMouseDown(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvaddresses.CellMouseDown
        If e.Button = MouseButtons.Right Then
            mrow = e.RowIndex
            mcol = e.ColumnIndex
        End If
    End Sub

    Private Sub UIrefresh_Tick(sender As System.Object, e As System.EventArgs) Handles UIrefresh.Tick
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

    Private Sub bupdatesettings_Click(sender As System.Object, e As System.EventArgs) Handles bupdatesettings.Click
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

    Private Sub txtrpcserver_Enter(sender As Object, e As System.EventArgs) Handles txtrpcserver.Enter
        txtrpcserver.BorderStyle = BorderStyle.FixedSingle
        If txtrpcserver.Text = "Not configured." Then txtrpcserver.Text = ""
    End Sub
    Private Sub txtrpcserver_Leave(sender As Object, e As System.EventArgs) Handles txtrpcserver.Leave
        txtrpcserver.BorderStyle = BorderStyle.None
        If txtrpcserver.Text = "" Then txtrpcserver.Text = "Not configured."
    End Sub
    Private Sub txtrpcport_Enter(sender As Object, e As System.EventArgs) Handles txtrpcport.Enter
        txtrpcport.BorderStyle = BorderStyle.FixedSingle
        If txtrpcport.Text = "Not configured." Then txtrpcport.Text = ""
    End Sub
    Private Sub txtrpcport_Leave(sender As Object, e As System.EventArgs) Handles txtrpcport.Leave
        txtrpcport.BorderStyle = BorderStyle.None
        If txtrpcport.Text = "" Then txtrpcport.Text = "Not configured."
    End Sub
    Private Sub txtrpcuser_Enter(sender As Object, e As System.EventArgs) Handles txtrpcuser.Enter
        txtrpcuser.BorderStyle = BorderStyle.FixedSingle
        If txtrpcuser.Text = "Not configured." Then txtrpcuser.Text = ""
    End Sub
    Private Sub txtrpcuser_Leave(sender As Object, e As System.EventArgs) Handles txtrpcuser.Leave
        txtrpcuser.BorderStyle = BorderStyle.None
        If txtrpcuser.Text = "" Then txtrpcuser.Text = "Not configured."
    End Sub
    Private Sub txtrpcpassword_Enter(sender As Object, e As System.EventArgs) Handles txtrpcpassword.Enter
        txtrpcpassword.BorderStyle = BorderStyle.FixedSingle
        If txtrpcpassword.Text = "Not configured." Then txtrpcpassword.Text = ""
    End Sub
    Private Sub txtrpcpassword_Leave(sender As Object, e As System.EventArgs) Handles txtrpcpassword.Leave
        txtrpcpassword.BorderStyle = BorderStyle.None
        If txtrpcpassword.Text = "" Then txtrpcpassword.Text = "Not configured."
    End Sub


    Private Sub lnkaddsort_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkaddsort.LinkClicked
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

    Private Sub lnkaddfilter_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkaddfilter.LinkClicked
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

    Private Sub lnkwcont_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkwcont.LinkClicked
        'check passphrase against db
        walpass = txtwalpass.Text
        'test connection to database
        Dim testval As Integer
        testval = SQLGetSingleVal("SELECT count(*) FROM information_schema.columns WHERE table_name = 'processedblocks'")
        If testval = 2 Then 'sanity check ok
        Else
            'something has gone wrong
            lwelpass.Visible = True
            Exit Sub
        End If
        lwelpass.Visible = False
        Application.DoEvents()

        'do some initial setup
        showlabels()
        bback.Visible = True
        txtdebug.Text = "MASTERCHEST WALLET v0.1a"
        activateoverview()
        lastscreen = "1"
        updateui()

        'kick off the background worker thread
        If workthread.IsBusy <> True Then
            workthread.RunWorkerAsync()
        End If
    End Sub

    Private Sub txtwalpass_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtwalpass.TextChanged
        lwelpass.Visible = False
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
            bitcoin_con.bitcoinrpcserver = txtstartsrv.Text
            bitcoin_con.bitcoinrpcport = Val(txtstartport.Text)
            bitcoin_con.bitcoinrpcuser = txtstartuser.Text
            bitcoin_con.bitcoinrpcpassword = txtstartpass.Text
            'test connection to bitcoind
            Try
                Dim checkhash As blockhash = mlib.getblockhash(bitcoin_con, 2)
                If checkhash.result.ToString = "000000006a625f06636b8bb6ac7b960a8d03705d1ace08b1a19da3fdcc99ddbd" Then 'we've got a correct response
                    
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
                        objWriter.Close()
                        'Application.Restart()
                    Else
                        MsgBox("Configuration file error")
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                ltestinfo.Text = "Failed to connect to Bitcoin."
                ltestinfo.ForeColor = Color.Red
                Exit Sub
            End Try
        Else
            ltestinfo.Text = "Please complete all fields"
        End If
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

    Private Sub lnkshowvwr_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkshowvwr.LinkClicked
        If lnkshowvwr.Text = "Show Transaction Viewer" Then
            txtviewer.Visible = True
            lnkshowvwr.Text = "Hide Transaction Viewer"
            Exit Sub
        End If
        If lnkshowvwr.Text = "Hide Transaction Viewer" Then
            txtviewer.Visible = False
            lnkshowvwr.Text = "Show Transaction Viewer"
            Exit Sub
        End If
    End Sub

    Private Sub rsendmsc_CheckedClicked(sender As System.Object, e As System.EventArgs) Handles rsendmsc.Click
        rsendmsc.Checked = True
        rsendtmsc.Checked = False
        updateavail()
    End Sub

    Private Sub rsendtmsc_CheckedClicked(sender As System.Object, e As System.EventArgs) Handles rsendtmsc.Click
        rsendmsc.Checked = False
        rsendtmsc.Checked = True
        updateavail()
    End Sub
    Private Sub updateavail()
        Dim denom As String = ""
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
        If denom = "" Then lsendavail.Text = "Select a sending address"
        lsendavail.Text = "Available: " & avail.ToString & denom
    End Sub
    Private Sub comsendaddress_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles comsendaddress.SelectedIndexChanged
        updateavail()
    End Sub

    Private Sub bdebugsend_Click(sender As System.Object, e As System.EventArgs) Handles bdebugsend.Click
        'first validate recipient address
        If txtsenddest.Text <> "" Then
            Dim fromadd As String = comsendaddress.SelectedItem.ToString
            Dim toadd As String = txtsenddest.Text
            Dim curtype As Integer
            If rsendmsc.Checked = True Then curtype = 1
            If rsendtmsc.Checked = True Then curtype = 2
            Dim amount As Double = Val(txtsendamount.Text)
            Dim amountlong As Long = amount * 100000000
            Try
                Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, txtsenddest.Text, 0, 0))
                If validater.result.isvalid = True Then 'address is valid
                    txtviewer.Text = "Recipient address is valid."
                    'push out to masterchest lib to encode the tx
                    Dim rawtx As String = mlib.encodetx(bitcoin_con, fromadd, toadd, curtype, amountlong)
                    'decode the tx in the viewer
                    txtviewer.AppendText(vbCrLf & "Raw transaction hex:" & vbCrLf & rawtx & vbCrLf & "Raw transaction decode:" & vbCrLf & mlib.rpccall(bitcoin_con, "decoderawtransaction", 1, rawtx, 0, 0))
                Else
                    txtviewer.Text = "Build transaction failed.  Recipient address is not valid."
                End If
            Catch ex As Exception
                MsgBox("Exeption thrown : " & ex.Message)
            End Try
        End If
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
        'first validate recipient address
        If txtsenddest.Text <> "" Then
            Dim fromadd As String = comsendaddress.SelectedItem.ToString
            Dim toadd As String = txtsenddest.Text
            Dim curtype As Integer
            If rsendmsc.Checked = True Then curtype = 1
            If rsendtmsc.Checked = True Then curtype = 2
            Dim amount As Double = Val(txtsendamount.Text)
            Dim amountlong As Long = amount * 100000000
            Try
                Dim validater As validate = JsonConvert.DeserializeObject(Of validate)(mlib.rpccall(bitcoin_con, "validateaddress", 1, txtsenddest.Text, 0, 0))
                If validater.result.isvalid = True Then 'address is valid
                    txtviewer.Text = "Recipient address is valid."
                    'push out to masterchest lib to encode the tx
                    Dim rawtx As String = mlib.encodetx(bitcoin_con, fromadd, toadd, curtype, amountlong)
                    'is rawtx empty
                    If rawtx = "" Then
                        txtviewer.AppendText(vbCrLf & "Raw transaction is empty - stopping.")
                        Exit Sub
                    End If
                    'decode the tx in the viewer
                    txtviewer.AppendText(vbCrLf & "Raw transaction hex:" & vbCrLf & rawtx & vbCrLf & "Raw transaction decode:" & vbCrLf & mlib.rpccall(bitcoin_con, "decoderawtransaction", 1, rawtx, 0, 0))
                    'attempt to unlock wallet, if it's not locked these will error out but we'll pick up the error on signing instead
                    Dim dontcareresponse = mlib.rpccall(bitcoin_con, "walletlock", 0, 0, 0, 0)
                    Dim dontcareresponse2 = mlib.rpccall(bitcoin_con, "walletpassphrase", 2, Trim(txtbtcpass.Text.ToString), 15, 0)
                    txtbtcpass.Text = ""
                    'try and sign transaction
                    Try
                        Dim signedtxn As signedtx = JsonConvert.DeserializeObject(Of signedtx)(mlib.rpccall(bitcoin_con, "signrawtransaction", 1, rawtx, 0, 0))
                        If signedtxn.result.complete = True Then
                            txtviewer.AppendText(vbCrLf & "Signing appears successful.")
                            Dim broadcasttx As broadcasttx = JsonConvert.DeserializeObject(Of broadcasttx)(mlib.rpccall(bitcoin_con, "sendrawtransaction", 1, signedtxn.result.hex, 0, 0))
                            If broadcasttx.result <> "" Then
                                txtviewer.AppendText(vbCrLf & "Transaction sent, ID: " & broadcasttx.result.ToString)
                                lsendtxinfo.Text = "Transaction sent, check viewer for TXID."
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
                                txtviewer.AppendText(vbCrLf & "Error sending transaction.")
                                lsendtxinfo.Text = "Error sending transaction."
                                lsendtxinfo.ForeColor = Color.FromArgb(255, 192, 128)
                                Exit Sub
                            End If
                        Else
                            txtviewer.AppendText(vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct.")
                            Exit Sub
                        End If
                    Catch ex As Exception
                        txtviewer.AppendText(vbCrLf & "Failed to sign transaction.  Ensure wallet passphrase is correct.  " & ex.Message)
                        Exit Sub
                    End Try
                Else
                    txtviewer.Text = "Build transaction failed.  Recipient address is not valid."
                End If
            Catch ex As Exception
                MsgBox("Exeption thrown : " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub bsendnew_Click(sender As System.Object, e As System.EventArgs) Handles bsendnew.Click
        txtviewer.Text = ""
        txtsendamount.Text = ""
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
End Class

