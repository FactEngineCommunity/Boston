<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFixModelErrors
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LabelPromptWhatToDo = New System.Windows.Forms.Label()
        Me.ButtonFixModelErrors = New System.Windows.Forms.Button()
        Me.CheckedListBoxFixTypes = New System.Windows.Forms.CheckedListBox()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.LabelModelName)
        Me.GroupBox1.Controls.Add(Me.LabelPromptModel)
        Me.GroupBox1.Controls.Add(Me.LabelPromptWhatToDo)
        Me.GroupBox1.Controls.Add(Me.ButtonFixModelErrors)
        Me.GroupBox1.Controls.Add(Me.CheckedListBoxFixTypes)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(962, 486)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'LabelPromptWhatToDo
        '
        Me.LabelPromptWhatToDo.AutoSize = True
        Me.LabelPromptWhatToDo.Location = New System.Drawing.Point(117, 46)
        Me.LabelPromptWhatToDo.Name = "LabelPromptWhatToDo"
        Me.LabelPromptWhatToDo.Size = New System.Drawing.Size(238, 13)
        Me.LabelPromptWhatToDo.TabIndex = 2
        Me.LabelPromptWhatToDo.Text = "Check which type of model errors you want fixed."
        '
        'ButtonFixModelErrors
        '
        Me.ButtonFixModelErrors.Location = New System.Drawing.Point(6, 41)
        Me.ButtonFixModelErrors.Name = "ButtonFixModelErrors"
        Me.ButtonFixModelErrors.Size = New System.Drawing.Size(105, 23)
        Me.ButtonFixModelErrors.TabIndex = 1
        Me.ButtonFixModelErrors.Text = "&Fix Model Errors"
        Me.ButtonFixModelErrors.UseVisualStyleBackColor = True
        '
        'CheckedListBoxFixTypes
        '
        Me.CheckedListBoxFixTypes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckedListBoxFixTypes.FormattingEnabled = True
        Me.CheckedListBoxFixTypes.Items.AddRange(New Object() {"Roles without JoinedORMObject. Remove from Model.", "Relations, Invalid ActiveRole on OriginColumns (OriginColumn.ActiveRole <> Destin" &
                "ation.ActiveRole). OriginColumn.Count <> DestinationColumn.Count", "Columns where ActiveRole is Nothing. Try and fix.", "Columns where ActiveRole is Nothing. Remove the Column.", "InternalUniquenessConstraints, where LevelNumbers are not correct. Correct the le" &
                "vel numbers.", "Column Ordinal Positions. Reset where out of synchronous order.", "RDS Tables with no Columns. Remove those Tables.", "RDS Columns that should be Mandatory (i.e. on Objectifed Fact Types). Make mandat" &
                "ory.", "Remove FactTypeInstances from Page where FactTypeIntance has RoleInstance that Jo" &
                "ins nothing.", "Relational Data Structure, Tables where the number of PrimaryKey columns does not" &
                " match the number of Roles in the PreferredIdentifier. Fix that.", "Duplicate Facts. Remove duplicates.", "RDS Tables and PGS Nodes (Nodes/Tables of ObjectifiedFactTypes) that are missing " &
                "Relations. Add the relations.", "RDS Tables with more than one relation for the same FactType/Join. Prune extra re" &
                "lations.", "RDS Relations, that have no OriginColumns. Try and create the OriginColumns."})
        Me.CheckedListBoxFixTypes.Location = New System.Drawing.Point(6, 70)
        Me.CheckedListBoxFixTypes.Name = "CheckedListBoxFixTypes"
        Me.CheckedListBoxFixTypes.Size = New System.Drawing.Size(950, 409)
        Me.CheckedListBoxFixTypes.TabIndex = 0
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(6, 16)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 3
        Me.LabelPromptModel.Text = "Model:"
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(51, 16)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(90, 13)
        Me.LabelModelName.TabIndex = 4
        Me.LabelModelName.Text = "LabelModelName"
        '
        'frmFixModelErrors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(986, 510)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmFixModelErrors"
        Me.Text = "Fix Model Errors"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LabelPromptWhatToDo As Label
    Friend WithEvents ButtonFixModelErrors As Button
    Friend WithEvents CheckedListBoxFixTypes As CheckedListBox
    Friend WithEvents LabelModelName As Label
    Friend WithEvents LabelPromptModel As Label
End Class
