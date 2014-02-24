Imports System.Runtime.InteropServices

Public Class sentfrm

    '////////////////////////
    '///HANDLE FORM FUNCTIONS
    '////////////////////////
    <DllImportAttribute("user32.dll")> Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function
    <DllImportAttribute("user32.dll")> Public Shared Function ReleaseCapture() As Boolean
    End Function
    Const WM_NCLBUTTONDOWN As Integer = &HA1
    Const HT_CAPTION As Integer = &H2

    Private Sub sentfrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtviewer.Text = txsummary
        ltxid.Text = senttxid

    End Sub
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown, RectangleShape1.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If

    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Close()
    End Sub

    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RectangleShape1.Click

    End Sub

    Private Sub lnkcopy_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkcopy.LinkClicked
        Try
            Clipboard.SetData(DataFormats.Text, ltxid.Text)
        Catch ex As Exception
        End Try
    End Sub
End Class