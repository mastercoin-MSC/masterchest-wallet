<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class sellfrm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(sellfrm))
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.boverview = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.comselladdress = New System.Windows.Forms.ComboBox()
        Me.lsendavail = New System.Windows.Forms.Label()
        Me.bsell = New System.Windows.Forms.Button()
        Me.txtsendamount = New System.Windows.Forms.TextBox()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.lunit = New System.Windows.Forms.Label()
        Me.bclose = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ltotalbtc = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtunit = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lnktimelimit = New System.Windows.Forms.LinkLabel()
        Me.lnkminfee = New System.Windows.Forms.LinkLabel()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RectangleShape1
        '
        Me.RectangleShape1.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.Location = New System.Drawing.Point(0, 0)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(500, 296)
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(501, 297)
        Me.ShapeContainer1.TabIndex = 0
        Me.ShapeContainer1.TabStop = False
        '
        'Label75
        '
        Me.Label75.AutoSize = True
        Me.Label75.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label75.Location = New System.Drawing.Point(45, 67)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(188, 13)
        Me.Label75.TabIndex = 54
        Me.Label75.Text = "SELECT YOUR SELLING ADDRESS:"
        '
        'boverview
        '
        Me.boverview.AutoSize = True
        Me.boverview.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boverview.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.boverview.Location = New System.Drawing.Point(29, 22)
        Me.boverview.Name = "boverview"
        Me.boverview.Size = New System.Drawing.Size(136, 30)
        Me.boverview.TabIndex = 56
        Me.boverview.Text = "sell 'test msc'"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(46, 178)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 13)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "AMOUNT TO SELL:"
        '
        'comselladdress
        '
        Me.comselladdress.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.comselladdress.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.comselladdress.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comselladdress.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.comselladdress.FormattingEnabled = True
        Me.comselladdress.Location = New System.Drawing.Point(49, 86)
        Me.comselladdress.Name = "comselladdress"
        Me.comselladdress.Size = New System.Drawing.Size(390, 21)
        Me.comselladdress.TabIndex = 62
        '
        'lsendavail
        '
        Me.lsendavail.Font = New System.Drawing.Font("Arial Narrow", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsendavail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lsendavail.Location = New System.Drawing.Point(305, 67)
        Me.lsendavail.Name = "lsendavail"
        Me.lsendavail.Size = New System.Drawing.Size(138, 13)
        Me.lsendavail.TabIndex = 63
        Me.lsendavail.Text = "Select a selling address"
        Me.lsendavail.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'bsell
        '
        Me.bsell.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.bsell.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bsell.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bsell.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bsell.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.bsell.Location = New System.Drawing.Point(406, 247)
        Me.bsell.Name = "bsell"
        Me.bsell.Size = New System.Drawing.Size(58, 23)
        Me.bsell.TabIndex = 64
        Me.bsell.Text = "Sell"
        Me.bsell.UseVisualStyleBackColor = False
        '
        'txtsendamount
        '
        Me.txtsendamount.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.txtsendamount.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtsendamount.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsendamount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.txtsendamount.Location = New System.Drawing.Point(48, 189)
        Me.txtsendamount.Name = "txtsendamount"
        Me.txtsendamount.Size = New System.Drawing.Size(148, 28)
        Me.txtsendamount.TabIndex = 65
        Me.txtsendamount.Text = "0.00"
        '
        'bcancel
        '
        Me.bcancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.bcancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bcancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bcancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.bcancel.Location = New System.Drawing.Point(342, 247)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(58, 23)
        Me.bcancel.TabIndex = 66
        Me.bcancel.Text = "Cancel"
        Me.bcancel.UseVisualStyleBackColor = False
        '
        'lunit
        '
        Me.lunit.AutoSize = True
        Me.lunit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lunit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lunit.Location = New System.Drawing.Point(-68, 253)
        Me.lunit.Name = "lunit"
        Me.lunit.Size = New System.Drawing.Size(38, 15)
        Me.lunit.TabIndex = 67
        Me.lunit.Text = "0.000"
        '
        'bclose
        '
        Me.bclose.Image = Global.Masterchest_Wallet.My.Resources.Resources.closeicon
        Me.bclose.Location = New System.Drawing.Point(479, 12)
        Me.bclose.Name = "bclose"
        Me.bclose.Size = New System.Drawing.Size(12, 12)
        Me.bclose.TabIndex = 70
        Me.bclose.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(349, 178)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 13)
        Me.Label4.TabIndex = 71
        Me.Label4.Text = "TOTAL (BTC):"
        '
        'ltotalbtc
        '
        Me.ltotalbtc.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltotalbtc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.ltotalbtc.Location = New System.Drawing.Point(346, 189)
        Me.ltotalbtc.Name = "ltotalbtc"
        Me.ltotalbtc.Size = New System.Drawing.Size(152, 31)
        Me.ltotalbtc.TabIndex = 72
        Me.ltotalbtc.Text = "0.00"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(198, 178)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 75
        Me.Label3.Text = "UNIT PRICE:"
        '
        'txtunit
        '
        Me.txtunit.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.txtunit.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtunit.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtunit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.txtunit.Location = New System.Drawing.Point(200, 189)
        Me.txtunit.Name = "txtunit"
        Me.txtunit.Size = New System.Drawing.Size(145, 28)
        Me.txtunit.TabIndex = 76
        Me.txtunit.Text = "0.00"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(45, 124)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 77
        Me.Label6.Text = "TIME LIMIT:"
        '
        'lnktimelimit
        '
        Me.lnktimelimit.ActiveLinkColor = System.Drawing.Color.PaleTurquoise
        Me.lnktimelimit.AutoSize = True
        Me.lnktimelimit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnktimelimit.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnktimelimit.LinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnktimelimit.Location = New System.Drawing.Point(132, 124)
        Me.lnktimelimit.Name = "lnktimelimit"
        Me.lnktimelimit.Size = New System.Drawing.Size(47, 13)
        Me.lnktimelimit.TabIndex = 79
        Me.lnktimelimit.TabStop = True
        Me.lnktimelimit.Text = "6 blocks"
        Me.lnktimelimit.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'lnkminfee
        '
        Me.lnkminfee.ActiveLinkColor = System.Drawing.Color.PaleTurquoise
        Me.lnkminfee.AutoSize = True
        Me.lnkminfee.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkminfee.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkminfee.LinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkminfee.Location = New System.Drawing.Point(132, 147)
        Me.lnkminfee.Name = "lnkminfee"
        Me.lnkminfee.Size = New System.Drawing.Size(64, 13)
        Me.lnkminfee.TabIndex = 80
        Me.lnkminfee.TabStop = True
        Me.lnkminfee.Text = "0.0001 BTC"
        Me.lnkminfee.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(45, 147)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 81
        Me.Label2.Text = "MINIMUM FEE:"
        '
        'sellfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(501, 297)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lnkminfee)
        Me.Controls.Add(Me.lnktimelimit)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ltotalbtc)
        Me.Controls.Add(Me.bclose)
        Me.Controls.Add(Me.lunit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bsell)
        Me.Controls.Add(Me.lsendavail)
        Me.Controls.Add(Me.comselladdress)
        Me.Controls.Add(Me.boverview)
        Me.Controls.Add(Me.Label75)
        Me.Controls.Add(Me.txtunit)
        Me.Controls.Add(Me.txtsendamount)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "sellfrm"
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
    Friend WithEvents comselladdress As System.Windows.Forms.ComboBox
    Friend WithEvents lsendavail As System.Windows.Forms.Label
    Friend WithEvents bsell As System.Windows.Forms.Button
    Friend WithEvents txtsendamount As System.Windows.Forms.TextBox
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents lunit As System.Windows.Forms.Label
    Friend WithEvents bclose As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ltotalbtc As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtunit As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lnktimelimit As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkminfee As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
