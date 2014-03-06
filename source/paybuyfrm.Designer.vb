<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class paybuyfrm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.boverview = New System.Windows.Forms.Label()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.bsell = New System.Windows.Forms.Button()
        Me.lselladdress = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbtc = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lcur = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ltimeleft = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbuyaddress = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lunitprice = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lcurtype = New System.Windows.Forms.Label()
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.bclose = New System.Windows.Forms.PictureBox()
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'boverview
        '
        Me.boverview.AutoSize = True
        Me.boverview.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boverview.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.boverview.Location = New System.Drawing.Point(24, 18)
        Me.boverview.Name = "boverview"
        Me.boverview.Size = New System.Drawing.Size(144, 30)
        Me.boverview.TabIndex = 57
        Me.boverview.Text = "send payment"
        '
        'bcancel
        '
        Me.bcancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.bcancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bcancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bcancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.bcancel.Location = New System.Drawing.Point(539, 214)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(58, 23)
        Me.bcancel.TabIndex = 73
        Me.bcancel.Text = "No"
        Me.bcancel.UseVisualStyleBackColor = False
        '
        'bsell
        '
        Me.bsell.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.bsell.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bsell.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bsell.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bsell.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.bsell.Location = New System.Drawing.Point(603, 214)
        Me.bsell.Name = "bsell"
        Me.bsell.Size = New System.Drawing.Size(58, 23)
        Me.bsell.TabIndex = 72
        Me.bsell.Text = "Yes"
        Me.bsell.UseVisualStyleBackColor = False
        '
        'lselladdress
        '
        Me.lselladdress.AutoSize = True
        Me.lselladdress.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lselladdress.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lselladdress.Location = New System.Drawing.Point(114, 71)
        Me.lselladdress.Name = "lselladdress"
        Me.lselladdress.Size = New System.Drawing.Size(78, 13)
        Me.lselladdress.TabIndex = 71
        Me.lselladdress.Text = "SELLERERROR"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(368, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(299, 18)
        Me.Label1.TabIndex = 69
        Me.Label1.Text = "Are you sure you wish to send the following payment:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(44, 71)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 74
        Me.Label4.Text = "SELLER:"
        '
        'lbtc
        '
        Me.lbtc.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbtc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lbtc.Location = New System.Drawing.Point(429, 93)
        Me.lbtc.Name = "lbtc"
        Me.lbtc.Size = New System.Drawing.Size(254, 32)
        Me.lbtc.TabIndex = 76
        Me.lbtc.Text = "0.00 BTC"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(368, 134)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(299, 18)
        Me.Label5.TabIndex = 77
        Me.Label5.Text = "To purchase the following currency:"
        '
        'lcur
        '
        Me.lcur.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lcur.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lcur.Location = New System.Drawing.Point(429, 158)
        Me.lcur.Name = "lcur"
        Me.lcur.Size = New System.Drawing.Size(254, 32)
        Me.lcur.TabIndex = 78
        Me.lcur.Text = "0.00 TMSC"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(44, 171)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 80
        Me.Label2.Text = "TIME LEFT:"
        '
        'ltimeleft
        '
        Me.ltimeleft.AutoSize = True
        Me.ltimeleft.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltimeleft.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.ltimeleft.Location = New System.Drawing.Point(114, 171)
        Me.ltimeleft.Name = "ltimeleft"
        Me.ltimeleft.Size = New System.Drawing.Size(89, 13)
        Me.ltimeleft.TabIndex = 79
        Me.ltimeleft.Text = "TIMELEFTERROR"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(44, 95)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 82
        Me.Label3.Text = "BUYER:"
        '
        'lbuyaddress
        '
        Me.lbuyaddress.AutoSize = True
        Me.lbuyaddress.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbuyaddress.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lbuyaddress.Location = New System.Drawing.Point(114, 95)
        Me.lbuyaddress.Name = "lbuyaddress"
        Me.lbuyaddress.Size = New System.Drawing.Size(76, 13)
        Me.lbuyaddress.TabIndex = 81
        Me.lbuyaddress.Text = "BUYERERROR"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(44, 145)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 13)
        Me.Label8.TabIndex = 84
        Me.Label8.Text = "UNIT PRICE:"
        '
        'lunitprice
        '
        Me.lunitprice.AutoSize = True
        Me.lunitprice.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lunitprice.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lunitprice.Location = New System.Drawing.Point(114, 145)
        Me.lunitprice.Name = "lunitprice"
        Me.lunitprice.Size = New System.Drawing.Size(67, 13)
        Me.lunitprice.TabIndex = 83
        Me.lunitprice.Text = "UNITERROR"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(45, 120)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 13)
        Me.Label10.TabIndex = 86
        Me.Label10.Text = "CURRENCY:"
        '
        'lcurtype
        '
        Me.lcurtype.AutoSize = True
        Me.lcurtype.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lcurtype.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lcurtype.Location = New System.Drawing.Point(114, 120)
        Me.lcurtype.Name = "lcurtype"
        Me.lcurtype.Size = New System.Drawing.Size(65, 13)
        Me.lcurtype.TabIndex = 85
        Me.lcurtype.Text = "CURERROR"
        '
        'RectangleShape1
        '
        Me.RectangleShape1.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.Location = New System.Drawing.Point(0, 0)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(694, 264)
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(695, 265)
        Me.ShapeContainer1.TabIndex = 87
        Me.ShapeContainer1.TabStop = False
        '
        'bclose
        '
        Me.bclose.Image = Global.Masterchest_Wallet.My.Resources.Resources.closeicon
        Me.bclose.Location = New System.Drawing.Point(671, 12)
        Me.bclose.Name = "bclose"
        Me.bclose.Size = New System.Drawing.Size(12, 12)
        Me.bclose.TabIndex = 88
        Me.bclose.TabStop = False
        '
        'paybuyfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(695, 265)
        Me.Controls.Add(Me.bclose)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.lcurtype)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lunitprice)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lbuyaddress)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ltimeleft)
        Me.Controls.Add(Me.lcur)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lbtc)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bsell)
        Me.Controls.Add(Me.lselladdress)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.boverview)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "paybuyfrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "paybuyfrm"
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents boverview As System.Windows.Forms.Label
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents bsell As System.Windows.Forms.Button
    Friend WithEvents lselladdress As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lbtc As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lcur As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ltimeleft As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbuyaddress As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lunitprice As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lcurtype As System.Windows.Forms.Label
    Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents bclose As System.Windows.Forms.PictureBox
End Class
