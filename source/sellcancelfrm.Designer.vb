<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class sellcancelfrm
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
        Me.boverview = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lselladdress = New System.Windows.Forms.Label()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.bsell = New System.Windows.Forms.Button()
        Me.bclose = New System.Windows.Forms.PictureBox()
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RectangleShape1
        '
        Me.RectangleShape1.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RectangleShape1.Location = New System.Drawing.Point(0, 0)
        Me.RectangleShape1.Name = "RectangleShape1"
        Me.RectangleShape1.Size = New System.Drawing.Size(431, 214)
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(432, 215)
        Me.ShapeContainer1.TabIndex = 0
        Me.ShapeContainer1.TabStop = False
        '
        'boverview
        '
        Me.boverview.AutoSize = True
        Me.boverview.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boverview.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.boverview.Location = New System.Drawing.Point(29, 21)
        Me.boverview.Name = "boverview"
        Me.boverview.Size = New System.Drawing.Size(158, 30)
        Me.boverview.TabIndex = 57
        Me.boverview.Text = "cancel sell offer"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(54, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(363, 45)
        Me.Label1.TabIndex = 58
        Me.Label1.Text = "Are you sure you wish to cancel your sell offer at the address below?  A sell can" & _
            "cel transaction will be broadcast."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer), CType(CType(81, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(54, 118)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 59
        Me.Label2.Text = "ADDRESS:"
        '
        'lselladdress
        '
        Me.lselladdress.AutoSize = True
        Me.lselladdress.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.lselladdress.Location = New System.Drawing.Point(116, 118)
        Me.lselladdress.Name = "lselladdress"
        Me.lselladdress.Size = New System.Drawing.Size(287, 13)
        Me.lselladdress.TabIndex = 60
        Me.lselladdress.Text = "IF YOU ARE SEEING THIS AN ERROR HAS OCCURRED"
        '
        'bcancel
        '
        Me.bcancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.bcancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bcancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bcancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.bcancel.Location = New System.Drawing.Point(266, 162)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(58, 23)
        Me.bcancel.TabIndex = 68
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
        Me.bsell.Location = New System.Drawing.Point(330, 162)
        Me.bsell.Name = "bsell"
        Me.bsell.Size = New System.Drawing.Size(58, 23)
        Me.bsell.TabIndex = 67
        Me.bsell.Text = "Yes"
        Me.bsell.UseVisualStyleBackColor = False
        '
        'bclose
        '
        Me.bclose.Image = Global.Masterchest_Wallet.My.Resources.Resources.closeicon
        Me.bclose.Location = New System.Drawing.Point(408, 13)
        Me.bclose.Name = "bclose"
        Me.bclose.Size = New System.Drawing.Size(12, 12)
        Me.bclose.TabIndex = 70
        Me.bclose.TabStop = False
        '
        'sellcancelfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(432, 215)
        Me.Controls.Add(Me.bclose)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bsell)
        Me.Controls.Add(Me.lselladdress)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.boverview)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "sellcancelfrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "sellcancelfrm"
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents boverview As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lselladdress As System.Windows.Forms.Label
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents bsell As System.Windows.Forms.Button
    Friend WithEvents bclose As System.Windows.Forms.PictureBox
End Class
