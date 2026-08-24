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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBox
        '
        Me.TextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox.Location = New System.Drawing.Point(20, 3)
        Me.TextBox.Name = "TextBox"
        Me.TextBox.Size = New System.Drawing.Size(224, 20)
        Me.TextBox.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Button1.BackgroundImage = Global.Boston.My.Resources.Resources.Spyglass20x20
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.ForeColor = System.Drawing.Color.Transparent
        Me.Button1.Location = New System.Drawing.Point(245, 4)
        Me.Button1.Margin = New System.Windows.Forms.Padding(0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(18, 18)
        Me.Button1.TabIndex = 1
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ButtonClear
        '
        Me.ButtonClear.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ButtonClear.BackgroundImage = Global.Boston.My.Resources.Resources.Cross_20x20
        Me.ButtonClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonClear.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonClear.Location = New System.Drawing.Point(1, 4)
        Me.ButtonClear.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(18, 18)
        Me.ButtonClear.TabIndex = 2
        Me.ButtonClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonClear.UseVisualStyleBackColor = True
        '
        'CustomSearchTextbox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ButtonClear)
        Me.Controls.Add(Me.TextBox)
        Me.Name = "CustomSearchTextbox"
        Me.Size = New System.Drawing.Size(270, 28)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents ButtonClear As Button
End Class
