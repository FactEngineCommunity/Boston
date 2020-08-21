<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDModel
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
        Me.components = New System.ComponentModel.Container()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.button_okay = New System.Windows.Forms.Button()
        Me.GroupBox_main = New System.Windows.Forms.GroupBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.LabelSchema = New System.Windows.Forms.Label()
        Me.ComboBoxSchema = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelModel = New System.Windows.Forms.Label()
        Me.GroupBoxDatabase = New System.Windows.Forms.GroupBox()
        Me.LabelOpenSuccessfull = New System.Windows.Forms.Label()
        Me.ButtonTestConnection = New System.Windows.Forms.Button()
        Me.ComboBoxDatabaseType = New System.Windows.Forms.ComboBox()
        Me.TextBoxDatabaseConnectionString = New System.Windows.Forms.TextBox()
        Me.LabelConnectionString = New System.Windows.Forms.Label()
        Me.LabelDatabaseType = New System.Windows.Forms.Label()
        Me.DialogOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.DialogFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.GroupBox_main.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBoxDatabase.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(924, 78)
        Me.Button_Cancel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(105, 37)
        Me.Button_Cancel.TabIndex = 8
        Me.Button_Cancel.Text = "&Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.Location = New System.Drawing.Point(924, 32)
        Me.button_okay.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(105, 37)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&Okay"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'GroupBox_main
        '
        Me.GroupBox_main.Controls.Add(Me.GroupBox1)
        Me.GroupBox_main.Controls.Add(Me.LabelModelName)
        Me.GroupBox_main.Controls.Add(Me.LabelModel)
        Me.GroupBox_main.Controls.Add(Me.GroupBoxDatabase)
        Me.GroupBox_main.Location = New System.Drawing.Point(18, 18)
        Me.GroupBox_main.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox_main.Name = "GroupBox_main"
        Me.GroupBox_main.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox_main.Size = New System.Drawing.Size(892, 582)
        Me.GroupBox_main.TabIndex = 6
        Me.GroupBox_main.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.LabelSchema)
        Me.GroupBox1.Controls.Add(Me.ComboBoxSchema)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Location = New System.Drawing.Point(28, 331)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Size = New System.Drawing.Size(834, 218)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Reverse Engineering"
        '
        'Button2
        '
        Me.Button2.Enabled = False
        Me.Button2.Location = New System.Drawing.Point(246, 29)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(296, 43)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Step 2: Reverse Engineer Database"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'LabelSchema
        '
        Me.LabelSchema.AutoSize = True
        Me.LabelSchema.Location = New System.Drawing.Point(12, 82)
        Me.LabelSchema.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelSchema.Name = "LabelSchema"
        Me.LabelSchema.Size = New System.Drawing.Size(76, 20)
        Me.LabelSchema.TabIndex = 2
        Me.LabelSchema.Text = "Schema :"
        '
        'ComboBoxSchema
        '
        Me.ComboBoxSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSchema.FormattingEnabled = True
        Me.ComboBoxSchema.Location = New System.Drawing.Point(99, 82)
        Me.ComboBoxSchema.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ComboBoxSchema.Name = "ComboBoxSchema"
        Me.ComboBoxSchema.Size = New System.Drawing.Size(164, 28)
        Me.ComboBoxSchema.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(14, 29)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(224, 43)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Step 1: Analyse Database"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(102, 37)
        Me.LabelModelName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(133, 20)
        Me.LabelModelName.TabIndex = 14
        Me.LabelModelName.Text = "LabelModelName"
        '
        'LabelModel
        '
        Me.LabelModel.AutoSize = True
        Me.LabelModel.Location = New System.Drawing.Point(30, 37)
        Me.LabelModel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelModel.Name = "LabelModel"
        Me.LabelModel.Size = New System.Drawing.Size(60, 20)
        Me.LabelModel.TabIndex = 13
        Me.LabelModel.Text = "Model :"
        '
        'GroupBoxDatabase
        '
        Me.GroupBoxDatabase.Controls.Add(Me.LabelOpenSuccessfull)
        Me.GroupBoxDatabase.Controls.Add(Me.ButtonTestConnection)
        Me.GroupBoxDatabase.Controls.Add(Me.ComboBoxDatabaseType)
        Me.GroupBoxDatabase.Controls.Add(Me.TextBoxDatabaseConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelDatabaseType)
        Me.GroupBoxDatabase.Location = New System.Drawing.Point(28, 91)
        Me.GroupBoxDatabase.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxDatabase.Name = "GroupBoxDatabase"
        Me.GroupBoxDatabase.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxDatabase.Size = New System.Drawing.Size(834, 231)
        Me.GroupBoxDatabase.TabIndex = 12
        Me.GroupBoxDatabase.TabStop = False
        Me.GroupBoxDatabase.Text = "Database:"
        '
        'LabelOpenSuccessfull
        '
        Me.LabelOpenSuccessfull.AutoSize = True
        Me.LabelOpenSuccessfull.Location = New System.Drawing.Point(192, 169)
        Me.LabelOpenSuccessfull.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelOpenSuccessfull.Name = "LabelOpenSuccessfull"
        Me.LabelOpenSuccessfull.Size = New System.Drawing.Size(168, 20)
        Me.LabelOpenSuccessfull.TabIndex = 6
        Me.LabelOpenSuccessfull.Text = "LabelOpenSuccessfull"
        Me.LabelOpenSuccessfull.Visible = False
        '
        'ButtonTestConnection
        '
        Me.ButtonTestConnection.Location = New System.Drawing.Point(42, 169)
        Me.ButtonTestConnection.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ButtonTestConnection.Name = "ButtonTestConnection"
        Me.ButtonTestConnection.Size = New System.Drawing.Size(141, 38)
        Me.ButtonTestConnection.TabIndex = 5
        Me.ButtonTestConnection.Text = "Test Connection"
        Me.ButtonTestConnection.UseVisualStyleBackColor = True
        '
        'ComboBoxDatabaseType
        '
        Me.ComboBoxDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDatabaseType.FormattingEnabled = True
        Me.ComboBoxDatabaseType.Location = New System.Drawing.Point(192, 31)
        Me.ComboBoxDatabaseType.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ComboBoxDatabaseType.Name = "ComboBoxDatabaseType"
        Me.ComboBoxDatabaseType.Size = New System.Drawing.Size(180, 28)
        Me.ComboBoxDatabaseType.TabIndex = 4
        '
        'TextBoxDatabaseConnectionString
        '
        Me.TextBoxDatabaseConnectionString.Location = New System.Drawing.Point(192, 78)
        Me.TextBoxDatabaseConnectionString.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBoxDatabaseConnectionString.Multiline = True
        Me.TextBoxDatabaseConnectionString.Name = "TextBoxDatabaseConnectionString"
        Me.TextBoxDatabaseConnectionString.Size = New System.Drawing.Size(613, 72)
        Me.TextBoxDatabaseConnectionString.TabIndex = 3
        '
        'LabelConnectionString
        '
        Me.LabelConnectionString.AutoSize = True
        Me.LabelConnectionString.Location = New System.Drawing.Point(38, 78)
        Me.LabelConnectionString.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelConnectionString.Name = "LabelConnectionString"
        Me.LabelConnectionString.Size = New System.Drawing.Size(144, 20)
        Me.LabelConnectionString.TabIndex = 2
        Me.LabelConnectionString.Text = "Connection String :"
        '
        'LabelDatabaseType
        '
        Me.LabelDatabaseType.AutoSize = True
        Me.LabelDatabaseType.Location = New System.Drawing.Point(38, 35)
        Me.LabelDatabaseType.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelDatabaseType.Name = "LabelDatabaseType"
        Me.LabelDatabaseType.Size = New System.Drawing.Size(125, 20)
        Me.LabelDatabaseType.TabIndex = 1
        Me.LabelDatabaseType.Text = "Database Type :"
        '
        'DialogOpenFile
        '
        Me.DialogOpenFile.FileName = "OpenFileDialog1"
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'frmCRUDModel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1048, 618)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Controls.Add(Me.GroupBox_main)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmCRUDModel"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TabText = "Model Configuration"
        Me.Text = "Model Configuration"
        Me.GroupBox_main.ResumeLayout(False)
        Me.GroupBox_main.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBoxDatabase.ResumeLayout(False)
        Me.GroupBoxDatabase.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents GroupBox_main As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxDatabase As System.Windows.Forms.GroupBox
    Friend WithEvents LabelConnectionString As System.Windows.Forms.Label
    Friend WithEvents LabelDatabaseType As System.Windows.Forms.Label
    Friend WithEvents TextBoxDatabaseConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents DialogOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DialogFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents LabelModelName As System.Windows.Forms.Label
    Friend WithEvents LabelModel As System.Windows.Forms.Label
    Friend WithEvents ComboBoxDatabaseType As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonTestConnection As System.Windows.Forms.Button
    Friend WithEvents LabelOpenSuccessfull As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents LabelSchema As System.Windows.Forms.Label
    Friend WithEvents ComboBoxSchema As System.Windows.Forms.ComboBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
End Class
