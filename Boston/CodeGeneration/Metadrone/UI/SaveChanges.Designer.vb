Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SaveChanges
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SaveChanges))
            Me.Label1 = New System.Windows.Forms.Label
            Me.lstItems = New System.Windows.Forms.ListBox
            Me.btnYes = New System.Windows.Forms.Button
            Me.btnNo = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.Panel1.SuspendLayout()
            Me.Panel2.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(17, 24)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(223, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Do you want to save changes to these items?"
            '
            'lstItems
            '
            Me.lstItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lstItems.FormattingEnabled = True
            Me.lstItems.Location = New System.Drawing.Point(20, 51)
            Me.lstItems.Name = "lstItems"
            Me.lstItems.SelectionMode = System.Windows.Forms.SelectionMode.None
            Me.lstItems.Size = New System.Drawing.Size(358, 238)
            Me.lstItems.TabIndex = 1
            '
            'btnYes
            '
            Me.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes
            Me.btnYes.Location = New System.Drawing.Point(13, 6)
            Me.btnYes.Name = "btnYes"
            Me.btnYes.Size = New System.Drawing.Size(75, 23)
            Me.btnYes.TabIndex = 0
            Me.btnYes.Text = "Yes"
            Me.btnYes.UseVisualStyleBackColor = True
            '
            'btnNo
            '
            Me.btnNo.DialogResult = System.Windows.Forms.DialogResult.No
            Me.btnNo.Location = New System.Drawing.Point(94, 6)
            Me.btnNo.Name = "btnNo"
            Me.btnNo.Size = New System.Drawing.Size(75, 23)
            Me.btnNo.TabIndex = 1
            Me.btnNo.Text = "No"
            Me.btnNo.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(175, 6)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 2
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.Panel2)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.Panel1.Location = New System.Drawing.Point(0, 301)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(399, 43)
            Me.Panel1.TabIndex = 2
            '
            'Panel2
            '
            Me.Panel2.Controls.Add(Me.btnYes)
            Me.Panel2.Controls.Add(Me.btnCancel)
            Me.Panel2.Controls.Add(Me.btnNo)
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Right
            Me.Panel2.Location = New System.Drawing.Point(130, 0)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(269, 43)
            Me.Panel2.TabIndex = 0
            '
            'SaveChanges
            '
            Me.AcceptButton = Me.btnYes
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(399, 344)
            Me.Controls.Add(Me.Panel1)
            Me.Controls.Add(Me.lstItems)
            Me.Controls.Add(Me.Label1)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "SaveChanges"
            Me.Opacity = 0.95
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Save Changes"
            Me.Panel1.ResumeLayout(False)
            Me.Panel2.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lstItems As System.Windows.Forms.ListBox
        Friend WithEvents btnYes As System.Windows.Forms.Button
        Friend WithEvents btnNo As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
    End Class

End Namespace