<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GroupNotificationBar
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
        Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ButtonDecline = New System.Windows.Forms.Button()
        Me.LabelPrompt = New System.Windows.Forms.Label()
        Me.TableLayoutPanel.SuspendLayout
        Me.SuspendLayout
        '
        'TableLayoutPanel
        '
        Me.TableLayoutPanel.AutoSize = true
        Me.TableLayoutPanel.ColumnCount = 3
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75!))
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75!))
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel.Controls.Add(Me.Button1, 0, 0)
        Me.TableLayoutPanel.Controls.Add(Me.ButtonDecline, 1, 0)
        Me.TableLayoutPanel.Controls.Add(Me.LabelPrompt, 2, 0)
        Me.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        Me.TableLayoutPanel.RowCount = 1
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel.Size = New System.Drawing.Size(560, 49)
        Me.TableLayoutPanel.TabIndex = 4
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(3, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(60, 26)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Accept"
        Me.Button1.UseVisualStyleBackColor = true
        '
        'ButtonDecline
        '
        Me.ButtonDecline.Location = New System.Drawing.Point(78, 3)
        Me.ButtonDecline.Name = "ButtonDecline"
        Me.ButtonDecline.Size = New System.Drawing.Size(60, 25)
        Me.ButtonDecline.TabIndex = 1
        Me.ButtonDecline.Text = "Decline"
        Me.ButtonDecline.UseVisualStyleBackColor = true
        '
        'LabelPrompt
        '
        Me.LabelPrompt.AutoSize = true
        Me.LabelPrompt.Location = New System.Drawing.Point(153, 3)
        Me.LabelPrompt.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelPrompt.MaximumSize = New System.Drawing.Size(200, 0)
        Me.LabelPrompt.Name = "LabelPrompt"
        Me.LabelPrompt.Size = New System.Drawing.Size(39, 13)
        Me.LabelPrompt.TabIndex = 2
        Me.LabelPrompt.Text = "Label1"
        '
        'GroupNotificationBar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanel)
        Me.Name = "GroupNotificationBar"
        Me.Size = New System.Drawing.Size(560, 49)
        Me.TableLayoutPanel.ResumeLayout(false)
        Me.TableLayoutPanel.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

    End Sub

    Friend WithEvents TableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ButtonDecline As System.Windows.Forms.Button
    Friend WithEvents LabelPrompt As System.Windows.Forms.Label

End Class
