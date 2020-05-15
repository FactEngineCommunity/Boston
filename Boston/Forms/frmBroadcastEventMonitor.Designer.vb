<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBroadcastEventMonitor
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

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
        Me.ListBoxBroadcastEvents = New System.Windows.Forms.ListBox()
        Me.LabelPromptBroadcastEvents = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ListBoxBroadcastEvents
        '
        Me.ListBoxBroadcastEvents.FormattingEnabled = True
        Me.ListBoxBroadcastEvents.Location = New System.Drawing.Point(12, 25)
        Me.ListBoxBroadcastEvents.Name = "ListBoxBroadcastEvents"
        Me.ListBoxBroadcastEvents.Size = New System.Drawing.Size(615, 238)
        Me.ListBoxBroadcastEvents.TabIndex = 0
        '
        'LabelPromptBroadcastEvents
        '
        Me.LabelPromptBroadcastEvents.AutoSize = True
        Me.LabelPromptBroadcastEvents.Location = New System.Drawing.Point(12, 9)
        Me.LabelPromptBroadcastEvents.Name = "LabelPromptBroadcastEvents"
        Me.LabelPromptBroadcastEvents.Size = New System.Drawing.Size(91, 13)
        Me.LabelPromptBroadcastEvents.TabIndex = 1
        Me.LabelPromptBroadcastEvents.Text = "Broadcast Events"
        '
        'frmBroadcastEventMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(640, 279)
        Me.Controls.Add(Me.LabelPromptBroadcastEvents)
        Me.Controls.Add(Me.ListBoxBroadcastEvents)
        Me.Name = "frmBroadcastEventMonitor"
        Me.TabText = "frmBroadcastEventMonitor"
        Me.Text = "Broadcast Event Monitor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListBoxBroadcastEvents As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptBroadcastEvents As System.Windows.Forms.Label
End Class
