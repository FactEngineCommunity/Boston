<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class CustomSearchTextbox
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TextBox = New System.Windows.Forms.TextBox()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBox
        '
        Me.TextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox.Location = New System.Drawing.Point(28, 3)
        Me.TextBox.Name = "TextBox"
        Me.TextBox.Size = New System.Drawing.Size(229, 20)
        Me.TextBox.TabIndex = 0
        '
        'ButtonClear
        '
        Me.ButtonClear.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.ButtonClear.Image = Global.Boston.My.Resources.MenuImages.delete10x10
        Me.ButtonClear.Location = New System.Drawing.Point(240, 4)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(15, 18)
        Me.ButtonClear.TabIndex = 2
        Me.ButtonClear.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Image = Global.Boston.My.Resources.MenuImages.Spyglass10x10
        Me.Button1.Location = New System.Drawing.Point(3, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(19, 18)
        Me.Button1.TabIndex = 1
        Me.Button1.UseVisualStyleBackColor = True
        '
        'CustomSearchTextbox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ButtonClear)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox)
        Me.Name = "CustomSearchTextbox"
        Me.Size = New System.Drawing.Size(260, 26)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents ButtonClear As Button
End Class
