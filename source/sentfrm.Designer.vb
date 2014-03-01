<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class sentfrm
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
        Me.txtviewer = New System.Windows.Forms.TextBox()
        Me.lsent = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ltxid = New System.Windows.Forms.Label()
        Me.lnkcopy = New System.Windows.Forms.LinkLabel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.bclose = New System.Windows.Forms.PictureBox()
        Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtviewer
        '
        Me.txtviewer.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.txtviewer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtviewer.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtviewer.ForeColor = System.Drawing.Color.White
        Me.txtviewer.Location = New System.Drawing.Point(59, 114)
        Me.txtviewer.Multiline = True
        Me.txtviewer.Name = "txtviewer"
        Me.txtviewer.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtviewer.Size = New System.Drawing.Size(583, 144)
        Me.txtviewer.TabIndex = 38
        Me.txtviewer.TabStop = False
        '
        'lsent
        '
        Me.lsent.AutoSize = True
        Me.lsent.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsent.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lsent.Location = New System.Drawing.Point(34, 22)
        Me.lsent.Name = "lsent"
        Me.lsent.Size = New System.Drawing.Size(161, 30)
        Me.lsent.TabIndex = 57
        Me.lsent.Tag = "localizabletext"
        Me.lsent.Text = My.Resources.transactionsent
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(56, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 13)
        Me.Label2.TabIndex = 59
        Me.Label2.Tag = "localizabletext"
        Me.Label2.Text = My.Resources.transactionid
        '
        'ltxid
        '
        Me.ltxid.AutoSize = True
        Me.ltxid.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.ltxid.Location = New System.Drawing.Point(157, 70)
        Me.ltxid.Name = "ltxid"
        Me.ltxid.Size = New System.Drawing.Size(376, 13)
        Me.ltxid.TabIndex = 60
        Me.ltxid.Text = "8dc3bc439443a391e151fcc587f64bcf72feeb7d2dc592c884efe7378493057b"
        '
        'lnkcopy
        '
        Me.lnkcopy.AutoSize = True
        Me.lnkcopy.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkcopy.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkcopy.LinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lnkcopy.Location = New System.Drawing.Point(544, 70)
        Me.lnkcopy.Name = "lnkcopy"
        Me.lnkcopy.Size = New System.Drawing.Size(36, 13)
        Me.lnkcopy.TabIndex = 61
        Me.lnkcopy.TabStop = True
        Me.lnkcopy.Tag = "localizedtext"
        Me.lnkcopy.Text = My.Resources.copypar
        Me.lnkcopy.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(56, 98)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(153, 13)
        Me.Label3.TabIndex = 62
        Me.Label3.Tag = "localizedtext"
        Me.Label3.Text = My.Resources.additionalinformation
        '
        'bcancel
        '
        Me.bcancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.bcancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bcancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bcancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.bcancel.Location = New System.Drawing.Point(602, 277)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(58, 23)
        Me.bcancel.TabIndex = 68
        Me.bcancel.Tag = "localizedtext"
        Me.bcancel.Text = My.Resources.ok
        Me.bcancel.UseVisualStyleBackColor = False
        '
        'bclose
        '
        Me.bclose.Image = Global.Masterchest_Wallet.My.Resources.Resources.closeicon
        Me.bclose.Location = New System.Drawing.Point(677, 12)
        Me.bclose.Name = "bclose"
        Me.bclose.Size = New System.Drawing.Size(12, 12)
        Me.bclose.TabIndex = 70
        Me.bclose.TabStop = False
        '
        'RectangleShape1
        '
        Me.RectangleShape1.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.Location = New System.Drawing.Point(0, 0)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(700, 333)
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(701, 334)
        Me.ShapeContainer1.TabIndex = 71
        Me.ShapeContainer1.TabStop = False
        '
        'sentfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(701, 334)
        Me.Controls.Add(Me.bclose)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lnkcopy)
        Me.Controls.Add(Me.ltxid)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lsent)
        Me.Controls.Add(Me.txtviewer)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "sentfrm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "sentfrm"
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtviewer As System.Windows.Forms.TextBox
    Friend WithEvents lsent As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ltxid As System.Windows.Forms.Label
    Friend WithEvents lnkcopy As System.Windows.Forms.LinkLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents bclose As System.Windows.Forms.PictureBox
    Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
End Class
