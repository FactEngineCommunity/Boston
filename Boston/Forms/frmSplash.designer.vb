<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSplash
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSplash))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LabelSoftwareCategory = New System.Windows.Forms.Label()
        Me.PictureboxSplash = New System.Windows.Forms.PictureBox()
        Me.label_serial_number = New System.Windows.Forms.Label()
        Me.label_splash = New System.Windows.Forms.Label()
        Me.labelprompt_rosters = New System.Windows.Forms.Label()
        Me.time_close_form = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.PictureboxSplash, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.LabelSoftwareCategory)
        Me.Panel1.Controls.Add(Me.PictureboxSplash)
        Me.Panel1.Controls.Add(Me.label_serial_number)
        Me.Panel1.Controls.Add(Me.label_splash)
        Me.Panel1.Controls.Add(Me.labelprompt_rosters)
        Me.Panel1.Location = New System.Drawing.Point(10, 10)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(415, 369)
        Me.Panel1.TabIndex = 0
        '
        'LabelSoftwareCategory
        '
        Me.LabelSoftwareCategory.AutoSize = True
        Me.LabelSoftwareCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelSoftwareCategory.ForeColor = System.Drawing.Color.Gray
        Me.LabelSoftwareCategory.Location = New System.Drawing.Point(150, 28)
        Me.LabelSoftwareCategory.Name = "LabelSoftwareCategory"
        Me.LabelSoftwareCategory.Size = New System.Drawing.Size(95, 16)
        Me.LabelSoftwareCategory.TabIndex = 7
        Me.LabelSoftwareCategory.Text = "Professional"
        '
        'PictureboxSplash
        '
        Me.PictureboxSplash.Location = New System.Drawing.Point(21, 54)
        Me.PictureboxSplash.Name = "PictureboxSplash"
        Me.PictureboxSplash.Size = New System.Drawing.Size(371, 256)
        Me.PictureboxSplash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureboxSplash.TabIndex = 6
        Me.PictureboxSplash.TabStop = False
        '
        'label_serial_number
        '
        Me.label_serial_number.Location = New System.Drawing.Point(93, 534)
        Me.label_serial_number.Name = "label_serial_number"
        Me.label_serial_number.Size = New System.Drawing.Size(302, 25)
        Me.label_serial_number.TabIndex = 5
        '
        'label_splash
        '
        Me.label_splash.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_splash.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.label_splash.Location = New System.Drawing.Point(18, 313)
        Me.label_splash.Name = "label_splash"
        Me.label_splash.Size = New System.Drawing.Size(273, 33)
        Me.label_splash.TabIndex = 4
        Me.label_splash.Text = "label_splash"
        '
        'labelprompt_rosters
        '
        Me.labelprompt_rosters.Font = New System.Drawing.Font("Arial", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelprompt_rosters.ForeColor = System.Drawing.Color.DimGray
        Me.labelprompt_rosters.Location = New System.Drawing.Point(14, 9)
        Me.labelprompt_rosters.Name = "labelprompt_rosters"
        Me.labelprompt_rosters.Size = New System.Drawing.Size(142, 42)
        Me.labelprompt_rosters.TabIndex = 2
        Me.labelprompt_rosters.Text = "Boston"
        '
        'time_close_form
        '
        Me.time_close_form.Enabled = True
        Me.time_close_form.Interval = 5000
        '
        'frmSplash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.ClientSize = New System.Drawing.Size(437, 390)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSplash"
        Me.Text = "Boston"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureboxSplash, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents labelprompt_rosters As System.Windows.Forms.Label
    Friend WithEvents label_splash As System.Windows.Forms.Label
    Friend WithEvents label_serial_number As System.Windows.Forms.Label
    Friend WithEvents time_close_form As System.Windows.Forms.Timer
    Friend WithEvents PictureboxSplash As System.Windows.Forms.PictureBox
    Friend WithEvents LabelSoftwareCategory As System.Windows.Forms.Label
End Class
