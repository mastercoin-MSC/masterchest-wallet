<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class buyfrm
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
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.boverview = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ltotall = New System.Windows.Forms.Label()
        Me.combuyaddress = New System.Windows.Forms.ComboBox()
        Me.lsendavail = New System.Windows.Forms.Label()
        Me.bbuy = New System.Windows.Forms.Button()
        Me.txtsendamount = New System.Windows.Forms.TextBox()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.ltotal = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ltotalbtc = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ltimelimit = New System.Windows.Forms.Label()
        Me.bclose = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lminfee = New System.Windows.Forms.Label()
        Me.lunit = New System.Windows.Forms.Label()
        Me.lnkavail = New System.Windows.Forms.LinkLabel()
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RectangleShape1
        '
        Me.RectangleShape1.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.Location = New System.Drawing.Point(0, 0)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(518, 290)
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(519, 291)
        Me.ShapeContainer1.TabIndex = 0
        Me.ShapeContainer1.TabStop = False
        '
        'Label75
        '
        Me.Label75.AutoSize = True
        Me.Label75.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label75.Location = New System.Drawing.Point(54, 67)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(184, 13)
        Me.Label75.TabIndex = 54
        Me.Label75.Text = "SELECT YOUR BUYING ADDRESS:"
        '
        'boverview
        '
        Me.boverview.AutoSize = True
        Me.boverview.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boverview.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.boverview.Location = New System.Drawing.Point(30, 22)
        Me.boverview.Name = "boverview"
        Me.boverview.Size = New System.Drawing.Size(140, 30)
        Me.boverview.TabIndex = 56
        Me.boverview.Text = "buy 'test msc'"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(55, 173)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(137, 13)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "AMOUNT TO PURCHASE:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(208, 173)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 13)
        Me.Label2.TabIndex = 58
        Me.Label2.Text = "UNIT PRICE (BTC):"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(263, 120)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 59
        Me.Label3.Text = "AVAILABLE:"
        '
        'ltotall
        '
        Me.ltotall.AutoSize = True
        Me.ltotall.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.ltotall.Location = New System.Drawing.Point(262, 140)
        Me.ltotall.Name = "ltotall"
        Me.ltotall.Size = New System.Drawing.Size(110, 13)
        Me.ltotall.TabIndex = 60
        Me.ltotall.Text = "TOTAL PRICE (BTC):"
        '
        'combuyaddress
        '
        Me.combuyaddress.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.combuyaddress.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.combuyaddress.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.combuyaddress.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.combuyaddress.FormattingEnabled = True
        Me.combuyaddress.Location = New System.Drawing.Point(58, 86)
        Me.combuyaddress.Name = "combuyaddress"
        Me.combuyaddress.Size = New System.Drawing.Size(406, 21)
        Me.combuyaddress.TabIndex = 62
        '
        'lsendavail
        '
        Me.lsendavail.Font = New System.Drawing.Font("Arial Narrow", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsendavail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lsendavail.Location = New System.Drawing.Point(317, 67)
        Me.lsendavail.Name = "lsendavail"
        Me.lsendavail.Size = New System.Drawing.Size(151, 15)
        Me.lsendavail.TabIndex = 63
        Me.lsendavail.Text = "Select a buying address"
        Me.lsendavail.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'bbuy
        '
        Me.bbuy.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.bbuy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bbuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bbuy.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bbuy.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.bbuy.Location = New System.Drawing.Point(432, 236)
        Me.bbuy.Name = "bbuy"
        Me.bbuy.Size = New System.Drawing.Size(58, 23)
        Me.bbuy.TabIndex = 64
        Me.bbuy.Text = "Buy"
        Me.bbuy.UseVisualStyleBackColor = False
        '
        'txtsendamount
        '
        Me.txtsendamount.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.txtsendamount.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtsendamount.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsendamount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.txtsendamount.Location = New System.Drawing.Point(57, 184)
        Me.txtsendamount.Name = "txtsendamount"
        Me.txtsendamount.Size = New System.Drawing.Size(150, 28)
        Me.txtsendamount.TabIndex = 65
        Me.txtsendamount.Tag = LocaleTag.Numeric
        Me.txtsendamount.Text = "0.00"
        '
        'bcancel
        '
        Me.bcancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.bcancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bcancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bcancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.bcancel.Location = New System.Drawing.Point(368, 236)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(58, 23)
        Me.bcancel.TabIndex = 66
        Me.bcancel.Text = "Cancel"
        Me.bcancel.UseVisualStyleBackColor = False
        '
        'ltotal
        '
        Me.ltotal.AutoSize = True
        Me.ltotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.ltotal.Location = New System.Drawing.Point(375, 138)
        Me.ltotal.Name = "ltotal"
        Me.ltotal.Size = New System.Drawing.Size(38, 15)
        Me.ltotal.TabIndex = 69
        Me.ltotal.Tag = LocaleTag.Numeric
        Me.ltotal.Text = "0.000"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(360, 173)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 13)
        Me.Label4.TabIndex = 71
        Me.Label4.Text = "TOTAL COST (BTC)"
        '
        'ltotalbtc
        '
        Me.ltotalbtc.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltotalbtc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.ltotalbtc.Location = New System.Drawing.Point(357, 183)
        Me.ltotalbtc.Name = "ltotalbtc"
        Me.ltotalbtc.Size = New System.Drawing.Size(151, 31)
        Me.ltotalbtc.TabIndex = 72
        Me.ltotalbtc.Tag = LocaleTag.Numeric
        Me.ltotalbtc.Text = "0.00"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(55, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 13)
        Me.Label5.TabIndex = 73
        Me.Label5.Text = "TIME LIMIT:"
        '
        'ltimelimit
        '
        Me.ltimelimit.AutoSize = True
        Me.ltimelimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltimelimit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.ltimelimit.Location = New System.Drawing.Point(143, 118)
        Me.ltimelimit.Name = "ltimelimit"
        Me.ltimelimit.Size = New System.Drawing.Size(14, 15)
        Me.ltimelimit.TabIndex = 74
        Me.ltimelimit.Tag = LocaleTag.Numeric
        Me.ltimelimit.Text = "0"
        '
        'bclose
        '
        Me.bclose.Image = Global.Masterchest_Wallet.My.Resources.Resources.closeicon
        Me.bclose.Location = New System.Drawing.Point(494, 12)
        Me.bclose.Name = "bclose"
        Me.bclose.Size = New System.Drawing.Size(12, 12)
        Me.bclose.TabIndex = 70
        Me.bclose.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(55, 140)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 13)
        Me.Label6.TabIndex = 75
        Me.Label6.Text = "MINIMUM FEE:"
        '
        'lminfee
        '
        Me.lminfee.AutoSize = True
        Me.lminfee.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lminfee.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lminfee.Location = New System.Drawing.Point(143, 138)
        Me.lminfee.Name = "lminfee"
        Me.lminfee.Size = New System.Drawing.Size(14, 15)
        Me.lminfee.TabIndex = 76
        Me.lminfee.Tag = LocaleTag.Numeric
        Me.lminfee.Text = "0"
        '
        'lunit
        '
        Me.lunit.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lunit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lunit.Location = New System.Drawing.Point(205, 183)
        Me.lunit.Name = "lunit"
        Me.lunit.Size = New System.Drawing.Size(155, 32)
        Me.lunit.TabIndex = 67
        Me.lunit.Tag = LocaleTag.Numeric
        Me.lunit.Text = "0.00"
        '
        'lnkavail
        '
        Me.lnkavail.ActiveLinkColor = System.Drawing.Color.PaleTurquoise
        Me.lnkavail.AutoSize = True
        Me.lnkavail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkavail.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkavail.LinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkavail.Location = New System.Drawing.Point(375, 120)
        Me.lnkavail.Name = "lnkavail"
        Me.lnkavail.Size = New System.Drawing.Size(13, 13)
        Me.lnkavail.TabIndex = 81
        Me.lnkavail.TabStop = True
        Me.lnkavail.Tag = LocaleTag.Numeric
        Me.lnkavail.Text = "0"
        Me.lnkavail.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'buyfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(519, 291)
        Me.Controls.Add(Me.lnkavail)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lminfee)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ltimelimit)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ltotalbtc)
        Me.Controls.Add(Me.bclose)
        Me.Controls.Add(Me.ltotal)
        Me.Controls.Add(Me.lunit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.txtsendamount)
        Me.Controls.Add(Me.bbuy)
        Me.Controls.Add(Me.lsendavail)
        Me.Controls.Add(Me.combuyaddress)
        Me.Controls.Add(Me.ltotall)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.boverview)
        Me.Controls.Add(Me.Label75)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "buyfrm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "buyfrm"
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents boverview As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ltotall As System.Windows.Forms.Label
    Friend WithEvents combuyaddress As System.Windows.Forms.ComboBox
    Friend WithEvents lsendavail As System.Windows.Forms.Label
    Friend WithEvents bbuy As System.Windows.Forms.Button
    Friend WithEvents txtsendamount As System.Windows.Forms.TextBox
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents ltotal As System.Windows.Forms.Label
    Friend WithEvents bclose As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ltotalbtc As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ltimelimit As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lminfee As System.Windows.Forms.Label
    Friend WithEvents lunit As System.Windows.Forms.Label
    Friend WithEvents lnkavail As System.Windows.Forms.LinkLabel
End Class
