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
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.sellfrm_Label75 = New System.Windows.Forms.Label()
        Me.sellfrm_boverview = New System.Windows.Forms.Label()
        Me.sellfrm_Label1 = New System.Windows.Forms.Label()
        Me.comselladdress = New System.Windows.Forms.ComboBox()
        Me.sellfrm_lsendavail = New System.Windows.Forms.Label()
        Me.bsell = New System.Windows.Forms.Button()
        Me.txtsendamount = New System.Windows.Forms.TextBox()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.lunit = New System.Windows.Forms.Label()
        Me.bclose = New System.Windows.Forms.PictureBox()
        Me.ltotalcostbtc = New System.Windows.Forms.Label()
        Me.ltotal = New System.Windows.Forms.Label()
        Me.lunitpricebtc = New System.Windows.Forms.Label()
        Me.txtunit = New System.Windows.Forms.TextBox()
        Me.ltimelimit = New System.Windows.Forms.Label()
        Me.lnktimelimit = New System.Windows.Forms.LinkLabel()
        Me.lnkminfee = New System.Windows.Forms.LinkLabel()
        Me.lminimumfee = New System.Windows.Forms.Label()
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
        'sellfrm_Label75
        '
        Me.sellfrm_Label75.AutoSize = True
        Me.sellfrm_Label75.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.sellfrm_Label75.Location = New System.Drawing.Point(45, 67)
        Me.sellfrm_Label75.Name = "sellfrm_Label75"
        Me.sellfrm_Label75.Size = New System.Drawing.Size(188, 13)
        Me.sellfrm_Label75.TabIndex = 54
        Me.sellfrm_Label75.Tag = LocaleTag.Text
        Me.sellfrm_Label75.Text = "SELECT YOUR SELLING ADDRESS:"
        '
        'sellfrm_boverview
        '
        Me.sellfrm_boverview.AutoSize = True
        Me.sellfrm_boverview.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sellfrm_boverview.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.sellfrm_boverview.Location = New System.Drawing.Point(29, 22)
        Me.sellfrm_boverview.Name = "sellfrm_boverview"
        Me.sellfrm_boverview.Size = New System.Drawing.Size(136, 30)
        Me.sellfrm_boverview.TabIndex = 56
        Me.sellfrm_boverview.Tag = LocaleTag.Text
        Me.sellfrm_boverview.Text = "sell 'test msc'"
        '
        'sellfrm_Label1
        '
        Me.sellfrm_Label1.AutoSize = True
        Me.sellfrm_Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.sellfrm_Label1.Location = New System.Drawing.Point(46, 178)
        Me.sellfrm_Label1.Name = "sellfrm_Label1"
        Me.sellfrm_Label1.Size = New System.Drawing.Size(104, 13)
        Me.sellfrm_Label1.TabIndex = 57
        Me.sellfrm_Label1.Tag = LocaleTag.Text
        Me.sellfrm_Label1.Text = "AMOUNT TO SELL:"
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
        'sellfrm_lsendavail
        '
        Me.sellfrm_lsendavail.Font = New System.Drawing.Font("Arial Narrow", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sellfrm_lsendavail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.sellfrm_lsendavail.Location = New System.Drawing.Point(305, 67)
        Me.sellfrm_lsendavail.Name = "sellfrm_lsendavail"
        Me.sellfrm_lsendavail.Size = New System.Drawing.Size(138, 13)
        Me.sellfrm_lsendavail.TabIndex = 63
        Me.sellfrm_lsendavail.Tag = LocaleTag.Text
        Me.sellfrm_lsendavail.Text = "Select a selling address"
        Me.sellfrm_lsendavail.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.bsell.Tag = LocaleTag.Text
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
        Me.bcancel.Location = New System.Drawing.Point(342, 247)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(58, 23)
        Me.bcancel.TabIndex = 66
        Me.bcancel.Tag = LocaleTag.Text
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
        Me.lunit.Tag = LocaleTag.Numeric
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
        'ltotalcostbtc
        '
        Me.ltotalcostbtc.AutoSize = True
        Me.ltotalcostbtc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.ltotalcostbtc.Location = New System.Drawing.Point(349, 178)
        Me.ltotalcostbtc.Name = "ltotalcostbtc"
        Me.ltotalcostbtc.Size = New System.Drawing.Size(75, 13)
        Me.ltotalcostbtc.TabIndex = 71
        Me.ltotalcostbtc.Tag = LocaleTag.Text
        Me.ltotalcostbtc.Text = "TOTAL (BTC):"
        '
        'ltotal
        '
        Me.ltotal.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.ltotal.Location = New System.Drawing.Point(346, 189)
        Me.ltotal.Name = "ltotal"
        Me.ltotal.Size = New System.Drawing.Size(152, 31)
        Me.ltotal.TabIndex = 72
        Me.ltotal.Tag = LocaleTag.Numeric
        Me.ltotal.Text = "0.00"
        '
        'lunitpricebtc
        '
        Me.lunitpricebtc.AutoSize = True
        Me.lunitpricebtc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.lunitpricebtc.Location = New System.Drawing.Point(198, 178)
        Me.lunitpricebtc.Name = "lunitpricebtc"
        Me.lunitpricebtc.Size = New System.Drawing.Size(71, 13)
        Me.lunitpricebtc.TabIndex = 75
        Me.ltotalcostbtc.Tag = LocaleTag.Text
        Me.lunitpricebtc.Text = "UNIT PRICE:"
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
        Me.txtunit.Tag = LocaleTag.Numeric
        Me.txtunit.Text = "0.00"
        '
        'ltimelimit
        '
        Me.ltimelimit.AutoSize = True
        Me.ltimelimit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.ltimelimit.Location = New System.Drawing.Point(45, 124)
        Me.ltimelimit.Name = "ltimelimit"
        Me.ltimelimit.Size = New System.Drawing.Size(67, 13)
        Me.ltimelimit.TabIndex = 77
        Me.ltimelimit.Tag = LocaleTag.Text
        Me.ltimelimit.Text = "TIME LIMIT:"
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
        Me.lnkminfee.Name = "lnkminfee"
        Me.lnkminfee.Tag = LocaleTag.Numeric
        Me.lnkminfee.Text = "0.0001 BTC"
        Me.lnkminfee.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'lminimumfee
        '
        Me.lminimumfee.AutoSize = True
        Me.lminimumfee.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.lminimumfee.Location = New System.Drawing.Point(45, 147)
        Me.lminimumfee.Name = "lminimumfee"
        Me.lminimumfee.Size = New System.Drawing.Size(82, 13)
        Me.lminimumfee.TabIndex = 81
        Me.lminimumfee.Tag = LocaleTag.Text
        Me.lminimumfee.Text = "MINIMUM FEE:"
        '
        'sellfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(501, 297)
        Me.Controls.Add(Me.lminimumfee)
        Me.Controls.Add(Me.lnkminfee)
        Me.Controls.Add(Me.lnktimelimit)
        Me.Controls.Add(Me.ltimelimit)
        Me.Controls.Add(Me.lunitpricebtc)
        Me.Controls.Add(Me.ltotalcostbtc)
        Me.Controls.Add(Me.ltotal)
        Me.Controls.Add(Me.bclose)
        Me.Controls.Add(Me.lunit)
        Me.Controls.Add(Me.sellfrm_Label1)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bsell)
        Me.Controls.Add(Me.sellfrm_lsendavail)
        Me.Controls.Add(Me.comselladdress)
        Me.Controls.Add(Me.sellfrm_boverview)
        Me.Controls.Add(Me.sellfrm_Label75)
        Me.Controls.Add(Me.txtunit)
        Me.Controls.Add(Me.txtsendamount)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
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
    Friend WithEvents sellfrm_Label75 As System.Windows.Forms.Label
    Friend WithEvents sellfrm_boverview As System.Windows.Forms.Label
    Friend WithEvents sellfrm_Label1 As System.Windows.Forms.Label
    Friend WithEvents comselladdress As System.Windows.Forms.ComboBox
    Friend WithEvents sellfrm_lsendavail As System.Windows.Forms.Label
    Friend WithEvents bsell As System.Windows.Forms.Button
    Friend WithEvents txtsendamount As System.Windows.Forms.TextBox
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents lunit As System.Windows.Forms.Label
    Friend WithEvents bclose As System.Windows.Forms.PictureBox
    Friend WithEvents ltotalcostbtc As System.Windows.Forms.Label
    Friend WithEvents ltotal As System.Windows.Forms.Label
    Friend WithEvents lunitpricebtc As System.Windows.Forms.Label
    Friend WithEvents txtunit As System.Windows.Forms.TextBox
    Friend WithEvents ltimelimit As System.Windows.Forms.Label
    Friend WithEvents lnktimelimit As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkminfee As System.Windows.Forms.LinkLabel
    Friend WithEvents lminimumfee As System.Windows.Forms.Label
End Class
