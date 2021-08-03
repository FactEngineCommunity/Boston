<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCRUDModel
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    'Form overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.button_okay = New System.Windows.Forms.Button()
        Me.GroupBox_main = New System.Windows.Forms.GroupBox()
        Me.LabelCoreVersion = New System.Windows.Forms.Label()
        Me.LabelPromptCoreVersion = New System.Windows.Forms.Label()
        Me.GroupBoxReverseEngineering = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelModel = New System.Windows.Forms.Label()
        Me.GroupBoxDatabase = New System.Windows.Forms.GroupBox()
        Me.ButtonFileSelect = New System.Windows.Forms.Button()
        Me.ButtonCreateDatabase = New System.Windows.Forms.Button()
        Me.LabelOpenSuccessfull = New System.Windows.Forms.Label()
        Me.ButtonTestConnection = New System.Windows.Forms.Button()
        Me.ComboBoxDatabaseType = New System.Windows.Forms.ComboBox()
        Me.TextBoxDatabaseConnectionString = New System.Windows.Forms.TextBox()
        Me.LabelConnectionString = New System.Windows.Forms.Label()
        Me.LabelDatabaseType = New System.Windows.Forms.Label()
        Me.CheckBoxIsDatabaseSynchronised = New System.Windows.Forms.CheckBox()
        Me.DialogOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.DialogFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.Tab1 = New System.Windows.Forms.TabPage()
        Me.Advanced = New System.Windows.Forms.TabPage()
        Me.LabelPromptIsDatabaseSynchronised = New System.Windows.Forms.Label()
        Me.GroupBox_main.SuspendLayout()
        Me.GroupBoxReverseEngineering.SuspendLayout()
        Me.GroupBoxDatabase.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.Tab1.SuspendLayout()
        Me.Advanced.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(649, 59)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(70, 24)
        Me.Button_Cancel.TabIndex = 8
        Me.Button_Cancel.Text = "&Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.Location = New System.Drawing.Point(649, 27)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(70, 24)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&Okay"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'GroupBox_main
        '
        Me.GroupBox_main.Controls.Add(Me.LabelCoreVersion)
        Me.GroupBox_main.Controls.Add(Me.LabelPromptCoreVersion)
        Me.GroupBox_main.Controls.Add(Me.GroupBoxReverseEngineering)
        Me.GroupBox_main.Controls.Add(Me.LabelModelName)
        Me.GroupBox_main.Controls.Add(Me.LabelModel)
        Me.GroupBox_main.Controls.Add(Me.GroupBoxDatabase)
        Me.GroupBox_main.Location = New System.Drawing.Point(12, 14)
        Me.GroupBox_main.Name = "GroupBox_main"
        Me.GroupBox_main.Size = New System.Drawing.Size(595, 329)
        Me.GroupBox_main.TabIndex = 6
        Me.GroupBox_main.TabStop = False
        '
        'LabelCoreVersion
        '
        Me.LabelCoreVersion.AutoSize = True
        Me.LabelCoreVersion.Location = New System.Drawing.Point(540, 24)
        Me.LabelCoreVersion.Name = "LabelCoreVersion"
        Me.LabelCoreVersion.Size = New System.Drawing.Size(90, 13)
        Me.LabelCoreVersion.TabIndex = 17
        Me.LabelCoreVersion.Text = "LabelCoreVersion"
        '
        'LabelPromptCoreVersion
        '
        Me.LabelPromptCoreVersion.AutoSize = True
        Me.LabelPromptCoreVersion.Location = New System.Drawing.Point(474, 24)
        Me.LabelPromptCoreVersion.Name = "LabelPromptCoreVersion"
        Me.LabelPromptCoreVersion.Size = New System.Drawing.Size(69, 13)
        Me.LabelPromptCoreVersion.TabIndex = 16
        Me.LabelPromptCoreVersion.Text = "Core version:"
        '
        'GroupBoxReverseEngineering
        '
        Me.GroupBoxReverseEngineering.Controls.Add(Me.Button2)
        Me.GroupBoxReverseEngineering.Location = New System.Drawing.Point(19, 215)
        Me.GroupBoxReverseEngineering.Name = "GroupBoxReverseEngineering"
        Me.GroupBoxReverseEngineering.Size = New System.Drawing.Size(556, 94)
        Me.GroupBoxReverseEngineering.TabIndex = 15
        Me.GroupBoxReverseEngineering.TabStop = False
        Me.GroupBoxReverseEngineering.Text = "Reverse Engineering"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(11, 19)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(197, 28)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Step 2: Reverse Engineer Database"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(68, 24)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(90, 13)
        Me.LabelModelName.TabIndex = 14
        Me.LabelModelName.Text = "LabelModelName"
        '
        'LabelModel
        '
        Me.LabelModel.AutoSize = True
        Me.LabelModel.Location = New System.Drawing.Point(20, 24)
        Me.LabelModel.Name = "LabelModel"
        Me.LabelModel.Size = New System.Drawing.Size(42, 13)
        Me.LabelModel.TabIndex = 13
        Me.LabelModel.Text = "Model :"
        '
        'GroupBoxDatabase
        '
        Me.GroupBoxDatabase.Controls.Add(Me.ButtonFileSelect)
        Me.GroupBoxDatabase.Controls.Add(Me.ButtonCreateDatabase)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelOpenSuccessfull)
        Me.GroupBoxDatabase.Controls.Add(Me.ButtonTestConnection)
        Me.GroupBoxDatabase.Controls.Add(Me.ComboBoxDatabaseType)
        Me.GroupBoxDatabase.Controls.Add(Me.TextBoxDatabaseConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelDatabaseType)
        Me.GroupBoxDatabase.Location = New System.Drawing.Point(19, 59)
        Me.GroupBoxDatabase.Name = "GroupBoxDatabase"
        Me.GroupBoxDatabase.Size = New System.Drawing.Size(556, 150)
        Me.GroupBoxDatabase.TabIndex = 12
        Me.GroupBoxDatabase.TabStop = False
        Me.GroupBoxDatabase.Text = "Database:"
        '
        'ButtonFileSelect
        '
        Me.ButtonFileSelect.BackColor = System.Drawing.Color.White
        Me.ButtonFileSelect.BackgroundImage = Global.Boston.My.Resources.Resources.folder16x16
        Me.ButtonFileSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonFileSelect.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonFileSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonFileSelect.ForeColor = System.Drawing.Color.White
        Me.ButtonFileSelect.Location = New System.Drawing.Point(275, 23)
        Me.ButtonFileSelect.Name = "ButtonFileSelect"
        Me.ButtonFileSelect.Size = New System.Drawing.Size(16, 16)
        Me.ButtonFileSelect.TabIndex = 8
        Me.ButtonFileSelect.UseVisualStyleBackColor = False
        Me.ButtonFileSelect.Visible = False
        '
        'ButtonCreateDatabase
        '
        Me.ButtonCreateDatabase.BackColor = System.Drawing.Color.White
        Me.ButtonCreateDatabase.BackgroundImage = Global.Boston.My.Resources.Resources.Add16x16
        Me.ButtonCreateDatabase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonCreateDatabase.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCreateDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCreateDatabase.ForeColor = System.Drawing.Color.White
        Me.ButtonCreateDatabase.Location = New System.Drawing.Point(253, 23)
        Me.ButtonCreateDatabase.Name = "ButtonCreateDatabase"
        Me.ButtonCreateDatabase.Size = New System.Drawing.Size(16, 16)
        Me.ButtonCreateDatabase.TabIndex = 7
        Me.ButtonCreateDatabase.UseVisualStyleBackColor = False
        '
        'LabelOpenSuccessfull
        '
        Me.LabelOpenSuccessfull.Location = New System.Drawing.Point(128, 110)
        Me.LabelOpenSuccessfull.Name = "LabelOpenSuccessfull"
        Me.LabelOpenSuccessfull.Size = New System.Drawing.Size(121, 26)
        Me.LabelOpenSuccessfull.TabIndex = 6
        Me.LabelOpenSuccessfull.Text = "LabelOpenSuccessfull"
        Me.LabelOpenSuccessfull.Visible = False
        '
        'ButtonTestConnection
        '
        Me.ButtonTestConnection.Location = New System.Drawing.Point(28, 110)
        Me.ButtonTestConnection.Name = "ButtonTestConnection"
        Me.ButtonTestConnection.Size = New System.Drawing.Size(94, 25)
        Me.ButtonTestConnection.TabIndex = 5
        Me.ButtonTestConnection.Text = "Test Connection"
        Me.ButtonTestConnection.UseVisualStyleBackColor = True
        '
        'ComboBoxDatabaseType
        '
        Me.ComboBoxDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDatabaseType.FormattingEnabled = True
        Me.ComboBoxDatabaseType.Location = New System.Drawing.Point(128, 20)
        Me.ComboBoxDatabaseType.Name = "ComboBoxDatabaseType"
        Me.ComboBoxDatabaseType.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxDatabaseType.TabIndex = 4
        '
        'TextBoxDatabaseConnectionString
        '
        Me.TextBoxDatabaseConnectionString.Location = New System.Drawing.Point(128, 51)
        Me.TextBoxDatabaseConnectionString.Multiline = True
        Me.TextBoxDatabaseConnectionString.Name = "TextBoxDatabaseConnectionString"
        Me.TextBoxDatabaseConnectionString.Size = New System.Drawing.Size(410, 48)
        Me.TextBoxDatabaseConnectionString.TabIndex = 3
        '
        'LabelConnectionString
        '
        Me.LabelConnectionString.AutoSize = True
        Me.LabelConnectionString.Location = New System.Drawing.Point(25, 51)
        Me.LabelConnectionString.Name = "LabelConnectionString"
        Me.LabelConnectionString.Size = New System.Drawing.Size(97, 13)
        Me.LabelConnectionString.TabIndex = 2
        Me.LabelConnectionString.Text = "Connection String :"
        '
        'LabelDatabaseType
        '
        Me.LabelDatabaseType.AutoSize = True
        Me.LabelDatabaseType.Location = New System.Drawing.Point(25, 23)
        Me.LabelDatabaseType.Name = "LabelDatabaseType"
        Me.LabelDatabaseType.Size = New System.Drawing.Size(86, 13)
        Me.LabelDatabaseType.TabIndex = 1
        Me.LabelDatabaseType.Text = "Database Type :"
        '
        'CheckBoxIsDatabaseSynchronised
        '
        Me.CheckBoxIsDatabaseSynchronised.AutoSize = True
        Me.CheckBoxIsDatabaseSynchronised.Location = New System.Drawing.Point(13, 15)
        Me.CheckBoxIsDatabaseSynchronised.Margin = New System.Windows.Forms.Padding(2)
        Me.CheckBoxIsDatabaseSynchronised.Name = "CheckBoxIsDatabaseSynchronised"
        Me.CheckBoxIsDatabaseSynchronised.Size = New System.Drawing.Size(150, 17)
        Me.CheckBoxIsDatabaseSynchronised.TabIndex = 7
        Me.CheckBoxIsDatabaseSynchronised.Text = "Is Database Synchronised"
        Me.CheckBoxIsDatabaseSynchronised.UseVisualStyleBackColor = True
        '
        'DialogOpenFile
        '
        Me.DialogOpenFile.FileName = "OpenFileDialog1"
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.Tab1)
        Me.TabControl1.Controls.Add(Me.Advanced)
        Me.TabControl1.Location = New System.Drawing.Point(8, 8)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(629, 381)
        Me.TabControl1.TabIndex = 9
        '
        'Tab1
        '
        Me.Tab1.Controls.Add(Me.GroupBox_main)
        Me.Tab1.Location = New System.Drawing.Point(4, 22)
        Me.Tab1.Margin = New System.Windows.Forms.Padding(2)
        Me.Tab1.Name = "Tab1"
        Me.Tab1.Padding = New System.Windows.Forms.Padding(2)
        Me.Tab1.Size = New System.Drawing.Size(621, 355)
        Me.Tab1.TabIndex = 0
        Me.Tab1.Text = "Database"
        Me.Tab1.UseVisualStyleBackColor = True
        '
        'Advanced
        '
        Me.Advanced.Controls.Add(Me.LabelPromptIsDatabaseSynchronised)
        Me.Advanced.Controls.Add(Me.CheckBoxIsDatabaseSynchronised)
        Me.Advanced.Location = New System.Drawing.Point(4, 22)
        Me.Advanced.Margin = New System.Windows.Forms.Padding(2)
        Me.Advanced.Name = "Advanced"
        Me.Advanced.Padding = New System.Windows.Forms.Padding(2)
        Me.Advanced.Size = New System.Drawing.Size(621, 355)
        Me.Advanced.TabIndex = 1
        Me.Advanced.Text = "Advanced"
        Me.Advanced.UseVisualStyleBackColor = True
        '
        'LabelPromptIsDatabaseSynchronised
        '
        Me.LabelPromptIsDatabaseSynchronised.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromptIsDatabaseSynchronised.Location = New System.Drawing.Point(13, 43)
        Me.LabelPromptIsDatabaseSynchronised.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelPromptIsDatabaseSynchronised.Name = "LabelPromptIsDatabaseSynchronised"
        Me.LabelPromptIsDatabaseSynchronised.Size = New System.Drawing.Size(549, 34)
        Me.LabelPromptIsDatabaseSynchronised.TabIndex = 8
        Me.LabelPromptIsDatabaseSynchronised.Text = "Warning: When 'Is Database Synchronised' is checked, Boston will modify the datab" &
    "ase when you change the ORM Model within the Model."
        '
        'frmCRUDModel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(763, 402)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Name = "frmCRUDModel"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TabText = "Model Configuration"
        Me.Text = "Model Configuration"
        Me.GroupBox_main.ResumeLayout(False)
        Me.GroupBox_main.PerformLayout()
        Me.GroupBoxReverseEngineering.ResumeLayout(False)
        Me.GroupBoxDatabase.ResumeLayout(False)
        Me.GroupBoxDatabase.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.Tab1.ResumeLayout(False)
        Me.Advanced.ResumeLayout(False)
        Me.Advanced.PerformLayout()
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
    Friend WithEvents GroupBoxReverseEngineering As System.Windows.Forms.GroupBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents CheckBoxIsDatabaseSynchronised As CheckBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents Tab1 As TabPage
    Friend WithEvents Advanced As TabPage
    Friend WithEvents LabelPromptIsDatabaseSynchronised As Label
    Friend WithEvents LabelCoreVersion As Label
    Friend WithEvents LabelPromptCoreVersion As Label
    Friend WithEvents ButtonCreateDatabase As Button
    Friend WithEvents ButtonFileSelect As Button
End Class
