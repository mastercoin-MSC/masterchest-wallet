Imports System.Data
Imports System.Data.Sql
Imports System.Web
Imports System.Net
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Bson
Imports Newtonsoft.Json.Serialization
Imports Newtonsoft.Json.Schema
Imports Newtonsoft.Json.Converters
Imports System.Linq
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Microsoft.Win32
Imports System.Configuration
Imports Masterchest.mlib

Module Module1
    Public Class broadcasttx
        Public result As String
        Public err As Object
        Public id As String
    End Class
    Public Class result_signedtx
        Public hex As String
        Public complete As Boolean
    End Class
    Public Class signedtx
        Public result As result_signedtx
        Public err As Object
        Public id As String
    End Class
    Public bitcoin_con As New bitcoinrpcconnection
    Public dexcur As String
    Public varsyncronized As Boolean
    Public varsyncblock As Integer
    Public debuglevel As Integer = 1
    Public avail As Double
    Public configured As Boolean = False
    Public lastscreen, curscreen As String
    Public localhostname As String
    Public mbkpsource, mbkpdest, sqluser, sqlpass As String
    Public mvkprunning As Boolean
    Public multisig As Boolean
    Public pubkeyhex As String
    Public walpass As String
    Public addresslist, taddresslist As New DataTable
    Public currencylist As New DataTable
    Public historylist, thistorylist As New DataTable
    Public mcol As Integer = -1
    Public mrow As Integer = -1
    Public balmsc As Double
    Public balumsc As Double
    Public balbtc As Double
    Public balrestmsc, balresmsc As Double
    Public balubtc As Double
    Public baltmsc As Double
    Public balutmsc As Double
    Public openorders, selloffers As New DataTable
    Public rpcuser, rpcpassword, rpcport, rpcserver As String
    Public btcpass, txsummary, senttxid, sellrefadd, paybuytxid As String
    Public errorcnt As Integer = 0
    '///////////////////////////
    '///// INIT & CLEAR ROUTINES
    '///////////////////////////
    Public Sub initialize()
        Form1.lnknofocus.Location = New Point(1000, 1000) 'hide off canvas
        hidepanels()
        Form1.poverview.Location = New Point(27, 125)
        Form1.pdebug.Location = New Point(27, 125)
        Form1.psend.Location = New Point(27, 125)
        Form1.paddresses.Location = New Point(27, 125)
        Form1.pcurrencies.Location = New Point(27, 125)
        Form1.psettings.Location = New Point(27, 125)
        Form1.phistory.Location = New Point(27, 125)
        Form1.pwelcome.Location = New Point(27, 125)
        Form1.pexchange.Location = New Point(27, 125)

        Form1.psetup.Location = New Point(27, 55)
        addresslist.Columns.Add("Address", GetType(String))
        addresslist.Columns.Add("btcamount", GetType(Double))
        addresslist.Columns.Add("tmscamount", GetType(Double))
        addresslist.Columns.Add("mscamount", GetType(Double))
        taddresslist.Columns.Add("Address", GetType(String))
        taddresslist.Columns.Add("btcamount", GetType(Double))
        taddresslist.Columns.Add("tmscamount", GetType(Double))
        taddresslist.Columns.Add("mscamount", GetType(Double))
        Form1.dgvaddresses.RowTemplate.Height = 18
        currencylist.Columns.Add("Currency", GetType(String))
        currencylist.Columns.Add("cbalance", GetType(Double))
        currencylist.Columns.Add("ubalance", GetType(Double))
        Form1.dgvcurrencies.RowTemplate.Height = 18
        historylist.Columns.Add("valid", GetType(Image))
        historylist.Columns.Add("direction", GetType(Image))
        historylist.Columns.Add("blocktime", GetType(Date))
        historylist.Columns.Add("fromadd", GetType(String))
        historylist.Columns.Add("toadd", GetType(String))
        historylist.Columns.Add("type", GetType(String))
        historylist.Columns.Add("currency", GetType(String))
        historylist.Columns.Add("amount", GetType(Double))
        thistorylist.Columns.Add("valid", GetType(Image))
        thistorylist.Columns.Add("direction", GetType(Image))
        thistorylist.Columns.Add("blocktime", GetType(Date))
        thistorylist.Columns.Add("fromadd", GetType(String))
        thistorylist.Columns.Add("toadd", GetType(String))
        thistorylist.Columns.Add("type", GetType(String))
        thistorylist.Columns.Add("currency", GetType(String))
        thistorylist.Columns.Add("amount", GetType(Double))
        Form1.dgvhistory.RowTemplate.Height = 18
        Form1.dgvselloffer.RowTemplate.Height = 18
        Form1.dgvopenorders.RowTemplate.Height = 18
        selloffers.Columns.Add("Seller")
        selloffers.Columns.Add("Amount")
        selloffers.Columns.Add("Unit Price")
        selloffers.Columns.Add("Total Price")
        openorders.Columns.Add("TXID")
        openorders.Columns.Add("Seller")
        openorders.Columns.Add("Buyer")
        openorders.Columns.Add("Available")
        openorders.Columns.Add("Reserved")
        openorders.Columns.Add("Purchased")
        openorders.Columns.Add("Unit Price")
        openorders.Columns.Add("Total Price")
        openorders.Columns.Add("Type")
        openorders.Columns.Add("Status")
        Form1.dgvopenorders.RowTemplate.DefaultCellStyle.Padding = New Padding(0)
        Dim mnu As New ContextMenuStrip
        Dim mnucopy As New ToolStripMenuItem("Copy")
        AddHandler mnucopy.Click, AddressOf mnucopy_click
        mnu.Items.AddRange(New ToolStripItem() {mnucopy})
        Form1.dgvaddresses.ContextMenuStrip = mnu
    End Sub
    Public Sub mnucopy_click()
        If mrow >= 0 And mcol >= 0 Then
            Try
                Clipboard.SetData(DataFormats.Text, Form1.dgvaddresses.Rows(mrow).Cells(mcol).Value.ToString)
            Catch e As Exception
            End Try

        End If
    End Sub
    Public Sub hidepanels()
        Form1.poverview.Visible = False
        Form1.pdebug.Visible = False
        Form1.psend.Visible = False
        Form1.paddresses.Visible = False
        Form1.pcurrencies.Visible = False
        Form1.psettings.Visible = False
        Form1.phistory.Visible = False
        Form1.psetup.Visible = False
        Form1.pwelcome.Visible = False
        Form1.pexchange.Visible = False
    End Sub
    Public Sub hidelabels()
        Form1.boverview.Visible = False
        Form1.bcurrencies.Visible = False
        Form1.bsend.Visible = False
        Form1.baddresses.Visible = False
        Form1.bhistory.Visible = False
        Form1.bcontracts.Visible = False
        Form1.bdebug.Visible = False
        Form1.bexchange.Visible = False
    End Sub
    Public Sub showlabels()
        Form1.boverview.Visible = True
        Form1.bcurrencies.Visible = True
        Form1.bsend.Visible = True
        Form1.baddresses.Visible = True
        Form1.bhistory.Visible = True
        Form1.bcontracts.Visible = True
        Form1.bdebug.Visible = True
        Form1.bexchange.Visible = True
    End Sub
    Public Sub deselectlabels()
        Form1.boverview.ForeColor = Color.FromArgb(100, 100, 100)
        Form1.bcurrencies.ForeColor = Color.FromArgb(100, 100, 100)
        Form1.bsend.ForeColor = Color.FromArgb(100, 100, 100)
        Form1.baddresses.ForeColor = Color.FromArgb(100, 100, 100)
        Form1.bhistory.ForeColor = Color.FromArgb(100, 100, 100)
        Form1.bcontracts.ForeColor = Color.FromArgb(65, 65, 65)
        Form1.bdebug.ForeColor = Color.FromArgb(100, 100, 100)
        Form1.bexchange.ForeColor = Color.FromArgb(100, 100, 100)
    End Sub

    '////////////////////
    '///PANEL ACTIVATIONS
    '////////////////////
    Public Sub activateoverview()
        deselectlabels()
        Form1.boverview.ForeColor = Color.FromArgb(209, 209, 209)
        hidepanels()
        Form1.poverview.Visible = True
        curscreen = "1"
    End Sub
    Public Sub activatecurrencies()
        deselectlabels()
        Form1.bcurrencies.ForeColor = Color.FromArgb(209, 209, 209)
        hidepanels()
        Form1.pcurrencies.Visible = True
        curscreen = "2"
    End Sub
    Public Sub activatesend()
        deselectlabels()
        Form1.bsend.ForeColor = Color.FromArgb(209, 209, 209)
        hidepanels()
        Form1.psend.Visible = True
        curscreen = "3"
    End Sub
    Public Sub activateaddresses()
        deselectlabels()
        Form1.baddresses.ForeColor = Color.FromArgb(209, 209, 209)
        hidepanels()
        Form1.paddresses.Visible = True
        curscreen = "4"
    End Sub
    Public Sub activatehistory()
        deselectlabels()
        Form1.bhistory.ForeColor = Color.FromArgb(209, 209, 209)
        hidepanels()
        Form1.phistory.Visible = True
        curscreen = "5"
    End Sub
    Public Sub activatesettings()
        deselectlabels()
        Form1.bcontracts.ForeColor = Color.FromArgb(209, 209, 209)
        hidepanels()
        Form1.psettings.Visible = True
        curscreen = "6"
    End Sub
    Public Sub activateexchange()
        deselectlabels()
        Form1.bexchange.ForeColor = Color.FromArgb(209, 209, 209)
        hidepanels()
        Form1.pexchange.Visible = True
        curscreen = "8"
        Form1.dgvselloffer.CurrentCell = Nothing
        Form1.dgvopenorders.CurrentCell = Nothing
        Form1.bbuy.ForeColor = Color.FromArgb(100, 100, 100)
    End Sub
    Public Sub activatedebug()
        deselectlabels()
        Form1.bdebug.ForeColor = Color.FromArgb(209, 209, 209)
        hidepanels()
        Form1.pdebug.Visible = True
        curscreen = "7"
        'handle text selection
        Form1.txtdebug.Focus()
        Form1.txtdebug.SelectionStart = Form1.txtdebug.Text.Length
        Form1.txtdebug.ScrollToCaret()
        Application.DoEvents()
        Form1.lnknofocus.Focus()
    End Sub
End Module

