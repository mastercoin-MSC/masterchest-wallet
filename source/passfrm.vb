Public Class passfrm

    Private Sub Form1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RectangleShape1.MouseDown

    End Sub

    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        btcpass = ""
        Me.Close()
    End Sub

    Private Sub bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click
        btcpass = ""
        btcpass = Trim(TextBox1.Text)
        Me.Close()
    End Sub

    Private Sub passfrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btcpass = ""
        TextBox1.Text = ""
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If (e.KeyCode = Keys.Return) Then
            e.SuppressKeyPress = True
            btcpass = ""
            btcpass = Trim(TextBox1.Text)
            Me.Close()
        End If
    End Sub
End Class