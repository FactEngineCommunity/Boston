<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDRemoveUnneededConcepts
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCRUDRemoveUnneededConcepts))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.LabelUnneededConceptsRemoved = New System.Windows.Forms.Label
        Me.ButtonRemoveUnneededConcepts = New System.Windows.Forms.Button
        Me.LabelExplanation = New System.Windows.Forms.Label
        Me.ButtonClose = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LabelUnneededConceptsRemoved)
        Me.GroupBox1.Controls.Add(Me.ButtonRemoveUnneededConcepts)
        Me.GroupBox1.Controls.Add(Me.LabelExplanation)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(352, 201)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'LabelUnneededConceptsRemoved
        '
        Me.LabelUnneededConceptsRemoved.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.LabelUnneededConceptsRemoved.Location = New System.Drawing.Point(178, 160)
        Me.LabelUnneededConceptsRemoved.Name = "LabelUnneededConceptsRemoved"
        Me.LabelUnneededConceptsRemoved.Size = New System.Drawing.Size(168, 32)
        Me.LabelUnneededConceptsRemoved.TabIndex = 2
        Me.LabelUnneededConceptsRemoved.Text = "Unneeded Concepts successfully removed from Boston."
        Me.LabelUnneededConceptsRemoved.Visible = False
        '
        'ButtonRemoveUnneededConcepts
        '
        Me.ButtonRemoveUnneededConcepts.Location = New System.Drawing.Point(11, 160)
        Me.ButtonRemoveUnneededConcepts.Name = "ButtonRemoveUnneededConcepts"
        Me.ButtonRemoveUnneededConcepts.Size = New System.Drawing.Size(158, 24)
        Me.ButtonRemoveUnneededConcepts.TabIndex = 1
        Me.ButtonRemoveUnneededConcepts.Text = "&Remove unneeded Concepts"
        Me.ButtonRemoveUnneededConcepts.UseVisualStyleBackColor = True
        '
        'LabelExplanation
        '
        Me.LabelExplanation.Location = New System.Drawing.Point(6, 14)
        Me.LabelExplanation.Name = "LabelExplanation"
        Me.LabelExplanation.Size = New System.Drawing.Size(340, 141)
        Me.LabelExplanation.TabIndex = 0
        Me.LabelExplanation.Text = resources.GetString("LabelExplanation.Text")
        '
        'ButtonClose
        '
        Me.ButtonClose.Location = New System.Drawing.Point(370, 21)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 1
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'frmCRUDRemoveUnneededConcepts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(453, 246)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmCRUDRemoveUnneededConcepts"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Remove unneeded Concepts from Boston"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonClose As System.Windows.Forms.Button
    Friend WithEvents LabelExplanation As System.Windows.Forms.Label
    Friend WithEvents LabelUnneededConceptsRemoved As System.Windows.Forms.Label
    Friend WithEvents ButtonRemoveUnneededConcepts As System.Windows.Forms.Button
End Class
