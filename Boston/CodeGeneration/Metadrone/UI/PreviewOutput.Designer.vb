Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class PreviewOutput
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PreviewOutput))
            Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
            Me.grdPaths = New System.Windows.Forms.DataGridView
            Me.Path = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.Panel4 = New System.Windows.Forms.Panel
            Me.lblPaths = New System.Windows.Forms.Label
            Me.txtOut = New System.Windows.Forms.RichTextBox
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.lblOut = New System.Windows.Forms.Label
            Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
            Me.tsMain = New System.Windows.Forms.ToolStrip
            Me.btnSave = New System.Windows.Forms.ToolStripButton
            Me.btnSaveAll = New System.Windows.Forms.ToolStripButton
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.txtOutput = New System.Windows.Forms.TextBox
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.Label2 = New System.Windows.Forms.Label
            Me.SplitContainer1.Panel1.SuspendLayout()
            Me.SplitContainer1.Panel2.SuspendLayout()
            Me.SplitContainer1.SuspendLayout()
            CType(Me.grdPaths, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.Panel4.SuspendLayout()
            Me.Panel2.SuspendLayout()
            Me.SplitContainer2.Panel1.SuspendLayout()
            Me.SplitContainer2.Panel2.SuspendLayout()
            Me.SplitContainer2.SuspendLayout()
            Me.tsMain.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.Panel3.SuspendLayout()
            Me.SuspendLayout()
            '
            'SplitContainer1
            '
            Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SplitContainer1.Location = New System.Drawing.Point(0, 25)
            Me.SplitContainer1.Name = "SplitContainer1"
            '
            'SplitContainer1.Panel1
            '
            Me.SplitContainer1.Panel1.Controls.Add(Me.grdPaths)
            Me.SplitContainer1.Panel1.Controls.Add(Me.Panel4)
            '
            'SplitContainer1.Panel2
            '
            Me.SplitContainer1.Panel2.Controls.Add(Me.txtOut)
            Me.SplitContainer1.Panel2.Controls.Add(Me.Panel2)
            Me.SplitContainer1.Size = New System.Drawing.Size(984, 553)
            Me.SplitContainer1.SplitterDistance = 188
            Me.SplitContainer1.TabIndex = 0
            '
            'grdPaths
            '
            Me.grdPaths.AllowUserToAddRows = False
            Me.grdPaths.AllowUserToDeleteRows = False
            Me.grdPaths.AllowUserToResizeColumns = False
            Me.grdPaths.AllowUserToResizeRows = False
            Me.grdPaths.BackgroundColor = System.Drawing.SystemColors.Window
            Me.grdPaths.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.grdPaths.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.grdPaths.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Path})
            Me.grdPaths.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grdPaths.GridColor = System.Drawing.SystemColors.Window
            Me.grdPaths.Location = New System.Drawing.Point(0, 18)
            Me.grdPaths.MultiSelect = False
            Me.grdPaths.Name = "grdPaths"
            Me.grdPaths.ReadOnly = True
            Me.grdPaths.RowHeadersVisible = False
            Me.grdPaths.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.grdPaths.Size = New System.Drawing.Size(188, 535)
            Me.grdPaths.TabIndex = 0
            '
            'Path
            '
            Me.Path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.Path.HeaderText = "Path"
            Me.Path.Name = "Path"
            Me.Path.ReadOnly = True
            '
            'Panel4
            '
            Me.Panel4.BackgroundImage = Global.Boston.My.Resources.CodeGeneration.BarTile
            Me.Panel4.Controls.Add(Me.lblPaths)
            Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel4.Location = New System.Drawing.Point(0, 0)
            Me.Panel4.Name = "Panel4"
            Me.Panel4.Size = New System.Drawing.Size(188, 18)
            Me.Panel4.TabIndex = 0
            '
            'lblPaths
            '
            Me.lblPaths.BackColor = System.Drawing.Color.Transparent
            Me.lblPaths.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblPaths.Location = New System.Drawing.Point(0, 0)
            Me.lblPaths.Name = "lblPaths"
            Me.lblPaths.Size = New System.Drawing.Size(188, 18)
            Me.lblPaths.TabIndex = 1
            Me.lblPaths.Text = "Output Paths"
            Me.lblPaths.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtOut
            '
            Me.txtOut.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtOut.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtOut.Location = New System.Drawing.Point(0, 18)
            Me.txtOut.Name = "txtOut"
            Me.txtOut.Size = New System.Drawing.Size(792, 535)
            Me.txtOut.TabIndex = 1
            Me.txtOut.Text = ""
            '
            'Panel2
            '
            Me.Panel2.BackgroundImage = Global.Boston.My.Resources.CodeGeneration.BarTile
            Me.Panel2.Controls.Add(Me.lblOut)
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel2.Location = New System.Drawing.Point(0, 0)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(792, 18)
            Me.Panel2.TabIndex = 0
            '
            'lblOut
            '
            Me.lblOut.BackColor = System.Drawing.Color.Transparent
            Me.lblOut.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblOut.Location = New System.Drawing.Point(0, 0)
            Me.lblOut.Name = "lblOut"
            Me.lblOut.Size = New System.Drawing.Size(792, 18)
            Me.lblOut.TabIndex = 1
            Me.lblOut.Text = "Output"
            Me.lblOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'SplitContainer2
            '
            Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
            Me.SplitContainer2.Name = "SplitContainer2"
            Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'SplitContainer2.Panel1
            '
            Me.SplitContainer2.Panel1.Controls.Add(Me.SplitContainer1)
            Me.SplitContainer2.Panel1.Controls.Add(Me.tsMain)
            '
            'SplitContainer2.Panel2
            '
            Me.SplitContainer2.Panel2.Controls.Add(Me.Panel1)
            Me.SplitContainer2.Size = New System.Drawing.Size(984, 664)
            Me.SplitContainer2.SplitterDistance = 578
            Me.SplitContainer2.TabIndex = 0
            '
            'tsMain
            '
            Me.tsMain.BackColor = System.Drawing.Color.Transparent
            Me.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnSave, Me.btnSaveAll})
            Me.tsMain.Location = New System.Drawing.Point(0, 0)
            Me.tsMain.Name = "tsMain"
            Me.tsMain.Size = New System.Drawing.Size(984, 25)
            Me.tsMain.TabIndex = 2
            Me.tsMain.Text = "ToolStrip2"
            '
            'btnSave
            '
            Me.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnSave.Enabled = False
            Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
            Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(23, 22)
            Me.btnSave.Text = "Save Output"
            '
            'btnSaveAll
            '
            Me.btnSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnSaveAll.Enabled = False
            Me.btnSaveAll.Image = CType(resources.GetObject("btnSaveAll.Image"), System.Drawing.Image)
            Me.btnSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnSaveAll.Name = "btnSaveAll"
            Me.btnSaveAll.Size = New System.Drawing.Size(23, 22)
            Me.btnSaveAll.Text = "Save All Outputs"
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.txtOutput)
            Me.Panel1.Controls.Add(Me.Panel3)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel1.Location = New System.Drawing.Point(0, 0)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(984, 82)
            Me.Panel1.TabIndex = 0
            '
            'txtOutput
            '
            Me.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtOutput.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.txtOutput.Location = New System.Drawing.Point(0, 18)
            Me.txtOutput.Multiline = True
            Me.txtOutput.Name = "txtOutput"
            Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.txtOutput.Size = New System.Drawing.Size(984, 64)
            Me.txtOutput.TabIndex = 0
            '
            'Panel3
            '
            Me.Panel3.BackgroundImage = Global.Boston.My.Resources.CodeGeneration.BarTile
            Me.Panel3.Controls.Add(Me.Label2)
            Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel3.Location = New System.Drawing.Point(0, 0)
            Me.Panel3.Name = "Panel3"
            Me.Panel3.Size = New System.Drawing.Size(984, 18)
            Me.Panel3.TabIndex = 3
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.Transparent
            Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Label2.Location = New System.Drawing.Point(0, 0)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(984, 18)
            Me.Label2.TabIndex = 1
            Me.Label2.Text = "Result"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'PreviewOutput
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.LightSteelBlue
            Me.ClientSize = New System.Drawing.Size(984, 664)
            Me.Controls.Add(Me.SplitContainer2)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MinimizeBox = False
            Me.Name = "PreviewOutput"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Preview Output"
            Me.SplitContainer1.Panel1.ResumeLayout(False)
            Me.SplitContainer1.Panel2.ResumeLayout(False)
            Me.SplitContainer1.ResumeLayout(False)
            CType(Me.grdPaths, System.ComponentModel.ISupportInitialize).EndInit()
            Me.Panel4.ResumeLayout(False)
            Me.Panel2.ResumeLayout(False)
            Me.SplitContainer2.Panel1.ResumeLayout(False)
            Me.SplitContainer2.Panel1.PerformLayout()
            Me.SplitContainer2.Panel2.ResumeLayout(False)
            Me.SplitContainer2.ResumeLayout(False)
            Me.tsMain.ResumeLayout(False)
            Me.tsMain.PerformLayout()
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            Me.Panel3.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
        Friend WithEvents grdPaths As System.Windows.Forms.DataGridView
        Friend WithEvents txtOut As System.Windows.Forms.RichTextBox
        Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Path As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents lblPaths As System.Windows.Forms.Label
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents lblOut As System.Windows.Forms.Label
        Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
        Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
        Friend WithEvents btnSaveAll As System.Windows.Forms.ToolStripButton
        Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    End Class

End Namespace