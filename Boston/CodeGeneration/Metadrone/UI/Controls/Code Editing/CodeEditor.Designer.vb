Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class CodeEditor
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CodeEditor))
            Me.tsMain = New System.Windows.Forms.ToolStrip
            Me.btnPreview = New System.Windows.Forms.ToolStripButton
            Me.lblDescription = New System.Windows.Forms.ToolStripLabel
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.pnlMain = New System.Windows.Forms.Panel
            Me.tsMain.SuspendLayout()
            Me.SuspendLayout()
            '
            'tsMain
            '
            'Me.tsMain.BackgroundImage = Global.Metadrone.My.Resources.Resources.MainToolbarBG
            Me.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnPreview, Me.lblDescription})
            Me.tsMain.Location = New System.Drawing.Point(0, 0)
            Me.tsMain.Name = "tsMain"
            Me.tsMain.Size = New System.Drawing.Size(524, 25)
            Me.tsMain.TabIndex = 1
            '
            'btnPreview
            '
            Me.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnPreview.Image = CType(resources.GetObject("btnPreview.Image"), System.Drawing.Image)
            Me.btnPreview.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnPreview.Name = "btnPreview"
            Me.btnPreview.Size = New System.Drawing.Size(23, 22)
            Me.btnPreview.Text = "Preview Output"
            Me.btnPreview.ToolTipText = "Preview Output (F6)"
            '
            'lblDescription
            '
            Me.lblDescription.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.lblDescription.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDescription.ForeColor = System.Drawing.SystemColors.GrayText
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(0, 22)
            Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
            '
            'Panel1
            '
            Me.Panel1.BackColor = System.Drawing.SystemColors.WindowText
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel1.Location = New System.Drawing.Point(0, 25)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(524, 1)
            Me.Panel1.TabIndex = 1
            '
            'pnlMain
            '
            Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlMain.Location = New System.Drawing.Point(0, 26)
            Me.pnlMain.Name = "pnlMain"
            Me.pnlMain.Size = New System.Drawing.Size(524, 397)
            Me.pnlMain.TabIndex = 2
            '
            'CodeEditor
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Window
            Me.Controls.Add(Me.pnlMain)
            Me.Controls.Add(Me.Panel1)
            Me.Controls.Add(Me.tsMain)
            Me.Name = "CodeEditor"
            Me.Size = New System.Drawing.Size(524, 423)
            Me.tsMain.ResumeLayout(False)
            Me.tsMain.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
        Friend WithEvents btnPreview As System.Windows.Forms.ToolStripButton
        Friend WithEvents lblDescription As System.Windows.Forms.ToolStripLabel
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents pnlMain As System.Windows.Forms.Panel

    End Class

End Namespace