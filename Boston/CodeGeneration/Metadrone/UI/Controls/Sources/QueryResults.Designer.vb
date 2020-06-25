Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class QueryResults
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
            Me.TabControl1 = New System.Windows.Forms.TabControl
            Me.TabPage1 = New System.Windows.Forms.TabPage
            Me.grdResults = New System.Windows.Forms.DataGridView
            Me.TabPage2 = New System.Windows.Forms.TabPage
            Me.txtMessages = New System.Windows.Forms.RichTextBox
            Me.TabControl1.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            CType(Me.grdResults, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage2.SuspendLayout()
            Me.SuspendLayout()
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.TabPage1)
            Me.TabControl1.Controls.Add(Me.TabPage2)
            Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControl1.Location = New System.Drawing.Point(0, 0)
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            Me.TabControl1.Size = New System.Drawing.Size(956, 741)
            Me.TabControl1.TabIndex = 0
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.grdResults)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(948, 715)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Results"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'grdResults
            '
            Me.grdResults.AllowUserToAddRows = False
            Me.grdResults.AllowUserToDeleteRows = False
            Me.grdResults.AllowUserToOrderColumns = True
            Me.grdResults.AllowUserToResizeRows = False
            Me.grdResults.BackgroundColor = System.Drawing.SystemColors.Window
            Me.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.grdResults.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grdResults.Location = New System.Drawing.Point(3, 3)
            Me.grdResults.Name = "grdResults"
            Me.grdResults.ReadOnly = True
            Me.grdResults.RowHeadersVisible = False
            Me.grdResults.Size = New System.Drawing.Size(942, 709)
            Me.grdResults.TabIndex = 1
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.txtMessages)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage2.Size = New System.Drawing.Size(948, 715)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Messages"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'txtMessages
            '
            Me.txtMessages.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtMessages.Location = New System.Drawing.Point(3, 3)
            Me.txtMessages.Name = "txtMessages"
            Me.txtMessages.Size = New System.Drawing.Size(942, 709)
            Me.txtMessages.TabIndex = 0
            Me.txtMessages.Text = ""
            '
            'QueryResults
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.TabControl1)
            Me.Name = "QueryResults"
            Me.Size = New System.Drawing.Size(956, 741)
            Me.TabControl1.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            CType(Me.grdResults, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage2.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents grdResults As System.Windows.Forms.DataGridView
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents txtMessages As System.Windows.Forms.RichTextBox

    End Class

End Namespace