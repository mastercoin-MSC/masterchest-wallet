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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(buyfrm))
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.buyfrm_Label75 = New System.Windows.Forms.Label()
        Me.buyfrm_boverview = New System.Windows.Forms.Label()
        Me.buyfrm_Label1 = New System.Windows.Forms.Label()
        Me.lunitpricebtc = New System.Windows.Forms.Label()
        Me.buyfrm_Label3 = New System.Windows.Forms.Label()
        Me.ltotalpricebtc = New System.Windows.Forms.Label()
        Me.combuyaddress = New System.Windows.Forms.ComboBox()
        Me.buyfrm_lsendavail = New System.Windows.Forms.Label()
        Me.bbuy = New System.Windows.Forms.Button()
        Me.txtsendamount = New System.Windows.Forms.TextBox()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.ltotal = New System.Windows.Forms.Label()
        Me.ltotalcostbtc = New System.Windows.Forms.Label()
        Me.ltotalbtc = New System.Windows.Forms.Label()
        Me.ltimelimit = New System.Windows.Forms.Label()
        Me.ltimelimitn = New System.Windows.Forms.Label()
        Me.bclose = New System.Windows.Forms.PictureBox()
        Me.lminimumfee = New System.Windows.Forms.Label()
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
        Me.buyfrm_Label75.AutoSize = True
        Me.buyfrm_Label75.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.buyfrm_Label75.Location = New System.Drawing.Point(54, 67)
        Me.buyfrm_Label75.Name = "Label75"
        Me.buyfrm_Label75.Size = New System.Drawing.Size(184, 13)
        Me.buyfrm_Label75.TabIndex = 54
        Me.buyfrm_Label75.Tag = LocaleTag.Text
        Me.buyfrm_Label75.Text = "SELECT YOUR BUYING ADDRESS:"
        '
        'buyfrm_boverview
        '
        Me.buyfrm_boverview.AutoSize = True
        Me.buyfrm_boverview.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.buyfrm_boverview.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.buyfrm_boverview.Location = New System.Drawing.Point(30, 22)
        Me.buyfrm_boverview.Name = "boverview"
        Me.buyfrm_boverview.Size = New System.Drawing.Size(140, 30)
        Me.buyfrm_boverview.TabIndex = 56
        Me.buyfrm_boverview.Tag = LocaleTag.Text
        Me.buyfrm_boverview.Text = "buy 'test msc'"
        '
        'Label1
        '
        Me.buyfrm_Label1.AutoSize = True
        Me.buyfrm_Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.buyfrm_Label1.Location = New System.Drawing.Point(55, 173)
        Me.buyfrm_Label1.Name = "Label1"
        Me.buyfrm_Label1.Size = New System.Drawing.Size(137, 13)
        Me.buyfrm_Label1.TabIndex = 57
        Me.buyfrm_Label1.Tag = LocaleTag.Text
        Me.buyfrm_Label1.Text = "AMOUNT TO PURCHASE:"
        '
        'lunitpricebtc
        '
        Me.lunitpricebtc.AutoSize = True
        Me.lunitpricebtc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.lunitpricebtc.Location = New System.Drawing.Point(208, 173)
        Me.lunitpricebtc.Name = "lunitpricebtc"
        Me.lunitpricebtc.Size = New System.Drawing.Size(101, 13)
        Me.lunitpricebtc.TabIndex = 58
        Me.lunitpricebtc.Tag = LocaleTag.Text
        Me.lunitpricebtc.Text = "UNIT PRICE (BTC):"
        '
        'buyfrm_Label3
        '
        Me.buyfrm_Label3.AutoSize = True
        Me.buyfrm_Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.buyfrm_Label3.Location = New System.Drawing.Point(263, 120)
        Me.buyfrm_Label3.Name = "buyfrm_Label3"
        Me.buyfrm_Label3.Size = New System.Drawing.Size(67, 13)
        Me.buyfrm_Label3.TabIndex = 59
        Me.buyfrm_Label3.Tag = LocaleTag.Text
        Me.buyfrm_Label3.Text = "AVAILABLE:"
        '
        'ltotalpricebtc
        '
        Me.ltotalpricebtc.AutoSize = True
        Me.ltotalpricebtc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.ltotalpricebtc.Location = New System.Drawing.Point(262, 140)
        Me.ltotalpricebtc.Name = "ltotalpricebtc"
        Me.ltotalpricebtc.Size = New System.Drawing.Size(110, 13)
        Me.ltotalpricebtc.TabIndex = 60
        Me.ltotalpricebtc.Tag = LocaleTag.Text
        Me.ltotalpricebtc.Text = "TOTAL PRICE (BTC):"
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
        'buyfrm_lsendavail
        '
        Me.buyfrm_lsendavail.Font = New System.Drawing.Font("Arial Narrow", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.buyfrm_lsendavail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.buyfrm_lsendavail.Location = New System.Drawing.Point(317, 67)
        Me.buyfrm_lsendavail.Name = "buyfrm_lsendavail"
        Me.buyfrm_lsendavail.Size = New System.Drawing.Size(151, 15)
        Me.buyfrm_lsendavail.TabIndex = 63
        Me.buyfrm_lsendavail.Tag = LocaleTag.Text
        Me.buyfrm_lsendavail.Text = "Select a buying address"
        Me.buyfrm_lsendavail.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.bbuy.Tag = LocaleTag.Text
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
        Me.bcancel.Tag = LocaleTag.Text
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
        'ltotalcostbtc
        '
        Me.ltotalcostbtc.AutoSize = True
        Me.ltotalcostbtc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.ltotalcostbtc.Location = New System.Drawing.Point(360, 173)
        Me.ltotalcostbtc.Name = "ltotalcostbtc"
        Me.ltotalcostbtc.Size = New System.Drawing.Size(104, 13)
        Me.ltotalcostbtc.TabIndex = 71
        Me.ltotalcostbtc.Tag = LocaleTag.Text
        Me.ltotalcostbtc.Text = "TOTAL COST (BTC)"
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
        'ltimelimit
        '
        Me.ltimelimit.AutoSize = True
        Me.ltimelimit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.ltimelimit.Location = New System.Drawing.Point(55, 120)
        Me.ltimelimit.Name = "ltimelimit"
        Me.ltimelimit.Size = New System.Drawing.Size(67, 13)
        Me.ltimelimit.TabIndex = 73
        Me.ltimelimit.Tag = LocaleTag.Text
        Me.ltimelimit.Text = "TIME LIMIT:"
        '
        'ltimelimitn
        '
        Me.ltimelimitn.AutoSize = True
        Me.ltimelimitn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltimelimitn.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.ltimelimitn.Location = New System.Drawing.Point(143, 118)
        Me.ltimelimitn.Name = "ltimelimit"
        Me.ltimelimitn.Size = New System.Drawing.Size(14, 15)
        Me.ltimelimitn.TabIndex = 74
        Me.ltimelimitn.Tag = LocaleTag.Numeric
        Me.ltimelimitn.Text = "0"
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
        'lminimumfee
        '
        Me.lminimumfee.AutoSize = True
        Me.lminimumfee.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.lminimumfee.Location = New System.Drawing.Point(55, 140)
        Me.lminimumfee.Name = "lminimumfee"
        Me.lminimumfee.Size = New System.Drawing.Size(82, 13)
        Me.lminimumfee.TabIndex = 75
        Me.lminimumfee.Tag = LocaleTag.Text
        Me.lminimumfee.Text = "MINIMUM FEE:"
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
        Me.Controls.Add(Me.lunitpricebtc)
        Me.Controls.Add(Me.lminfee)
        Me.Controls.Add(Me.lminimumfee)
        Me.Controls.Add(Me.ltimelimitn)
        Me.Controls.Add(Me.ltimelimit)
        Me.Controls.Add(Me.ltotalcostbtc)
        Me.Controls.Add(Me.ltotalbtc)
        Me.Controls.Add(Me.bclose)
        Me.Controls.Add(Me.ltotal)
        Me.Controls.Add(Me.lunit)
        Me.Controls.Add(Me.buyfrm_Label1)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.txtsendamount)
        Me.Controls.Add(Me.bbuy)
        Me.Controls.Add(Me.buyfrm_lsendavail)
        Me.Controls.Add(Me.combuyaddress)
        Me.Controls.Add(Me.ltotalpricebtc)
        Me.Controls.Add(Me.buyfrm_Label3)
        Me.Controls.Add(Me.buyfrm_boverview)
        Me.Controls.Add(Me.buyfrm_Label75)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
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
    Friend WithEvents buyfrm_Label75 As System.Windows.Forms.Label
    Friend WithEvents buyfrm_boverview As System.Windows.Forms.Label
    Friend WithEvents buyfrm_Label1 As System.Windows.Forms.Label
    Friend WithEvents lunitpricebtc As System.Windows.Forms.Label
    Friend WithEvents buyfrm_Label3 As System.Windows.Forms.Label
    Friend WithEvents ltotalpricebtc As System.Windows.Forms.Label
    Friend WithEvents combuyaddress As System.Windows.Forms.ComboBox
    Friend WithEvents buyfrm_lsendavail As System.Windows.Forms.Label
    Friend WithEvents bbuy As System.Windows.Forms.Button
    Friend WithEvents txtsendamount As System.Windows.Forms.TextBox
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents ltotal As System.Windows.Forms.Label
    Friend WithEvents bclose As System.Windows.Forms.PictureBox
    Friend WithEvents ltotalcostbtc As System.Windows.Forms.Label
    Friend WithEvents ltotalbtc As System.Windows.Forms.Label
    Friend WithEvents ltimelimit As System.Windows.Forms.Label
    Friend WithEvents ltimelimitn As System.Windows.Forms.Label
    Friend WithEvents lminimumfee As System.Windows.Forms.Label
    Friend WithEvents lminfee As System.Windows.Forms.Label
    Friend WithEvents lunit As System.Windows.Forms.Label
    Friend WithEvents lnkavail As System.Windows.Forms.LinkLabel
End Class
