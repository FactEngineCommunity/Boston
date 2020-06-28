Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Boston.PluginInterface.Sources
Imports System.Reflection
Imports Boston.FBM

Namespace SourcePlugins.Boston
    Partial Public Class Manage
        Inherits UserControl
        Implements PluginInterface.Sources.IManageSource

        'Public Event ValueChanged As PluginInterface.Sources.IManageSource.ValueChangedEventHandler
        Public Delegate Sub ValueChangedEventHandler(ByVal value As Object)
        'Public Event Save As PluginInterface.Sources.IManageSource.SaveEventHandler
        Public Shadows Event ValueChanged(ByVal value As Object) Implements IManageSource.ValueChanged
        Public Event Save() Implements PluginInterface.Sources.IManageSource.Save
        'Private Event Save() Implements IManageSource.Save

        Public Delegate Sub SaveEventHandler()

        Public Sub New()
            InitializeComponent()

            AddHandler Me.Load, New System.EventHandler(AddressOf Me.Manage_Load)
            'AddHandler Me.txtServer.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.chkPort.CheckedChanged, New System.EventHandler(AddressOf Me.chkPort_CheckedChanged)
            'AddHandler Me.txtPort.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.txtDatabase.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.txtUsername.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.txtPassword.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.txtConnectionString.TextChanged, New System.EventHandler(AddressOf Me.txtConnectionString_TextChanged)
            'AddHandler Me.btnTest.Click, New System.EventHandler(AddressOf Me.btnTest_Click)
            'Me.txtSchemaQuery.KeyDown += New Metadrone.UI.SQLEditor.KeyDownEventHandler(AddressOf Me.txtSchemaQuery_KeyDown)
            'Me.lnkPreviewSchema.LinkClicked += New System.Windows.Forms.LinkLabelLinkClickedEventHandler(AddressOf Me.lnkPreviewSchema_LinkClicked)
            'Me.txtRoutineSchemaQuery.KeyDown += New Metadrone.UI.SQLEditor.KeyDownEventHandler(AddressOf Me.txtRoutineSchemaQuery_KeyDown)
            'Me.lnkPreviewRoutineSchema.LinkClicked += New System.Windows.Forms.LinkLabelLinkClickedEventHandler(AddressOf Me.lnkPreviewRoutineSchema_LinkClicked)
            'Me.txtSchemaQuery.TextChanged += New Metadrone.UI.SQLEditor.TextChangedEventHandler(AddressOf Me.txtSchemaQuery_TextChanged)
            'Me.txtSchemaQuery.SavePress += New Metadrone.UI.SQLEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'Me.txtTableSchemaQuery.TextChanged += New Metadrone.UI.SQLEditor.TextChangedEventHandler(AddressOf Me.txtTableSchemaQuery_TextChanged)
            'Me.txtTableSchemaQuery.SavePress += New Metadrone.UI.SQLEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'Me.txtColumnSchemaQuery.TextChanged += New Metadrone.UI.SQLEditor.TextChangedEventHandler(AddressOf Me.txtColumnSchemaQuery_TextChanged)
            'Me.txtColumnSchemaQuery.SavePress += New Metadrone.UI.SQLEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'Me.txtRoutineSchemaQuery.TextChanged += New Metadrone.UI.SQLEditor.TextChangedEventHandler(AddressOf Me.txtRoutineSchemaQuery_TextChanged)
            'Me.txtRoutineSchemaQuery.SavePress += New Metadrone.UI.SQLEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'AddHandler Me.txtTableName.TextChanged, New System.EventHandler(AddressOf Me.txtTableName_TextChanged)
            'AddHandler Me.rbApproachSingle.CheckedChanged, New System.EventHandler(AddressOf Me.rbMeta_CheckedChanged)
            'AddHandler Me.rbApproachTableColumn.CheckedChanged, New System.EventHandler(AddressOf Me.rbMeta_CheckedChanged)
            'AddHandler Me.rbTableDefault.CheckedChanged, New System.EventHandler(AddressOf Me.rbMeta_CheckedChanged)
            'AddHandler Me.rbTableQuery.CheckedChanged, New System.EventHandler(AddressOf Me.rbMeta_CheckedChanged)
            AddHandler Me.txtTransformations.SavePress, New UI.TransformationsEditor.SavePressEventHandler(AddressOf Me.SavePress) 'Boston.UI.TransformationsEditor.SavePressEventHandler
            AddHandler Me.txtTransformations.TextChanged, New UI.TransformationsEditor.TextChangedEventHandler(AddressOf Me.txtTransformations_TextChanged) 'Metadrone.UI.TransformationsEditor.TextChangedEventHandler

        End Sub

        Public Sub Setup() Implements IManageSource.Setup

            'NB See ManageDataSources.Initialise for initial setting of properties

            'Boston specific Setup
            If prApplication.WorkingModel Is Nothing Then
                Me.LabelWorkingModel.Text = "You must be working from a selected model in the Model Explorer."
            Else
                Me.LabelWorkingModel.Text = prApplication.WorkingModel.Name
                Me.LabelModelName.Text = prApplication.WorkingModel.Name

                Call Me.LoadERDModelDictionary()
            End If

            '---------------------------------------------------------------------------------
            'Load the set of Models loaded into the Boston application (prApplication.Models
            If Me.ComboBoxModel.Items.Count = 0 Then
                Dim loComboBoxItem As New tComboboxItem("0", "<No Model Selected>", Nothing)
                Dim loWorkingComboboxItem As tComboboxItem = Nothing
                Me.ComboBoxModel.Items.Add(loComboBoxItem)
                For Each lrModel In prApplication.Models
                    loComboBoxItem = New tComboboxItem(lrModel.ModelId, lrModel.Name, lrModel)
                    Me.ComboBoxModel.Items.Add(loComboBoxItem)
                    If Me.BostonModel IsNot Nothing Then
                        If Me.BostonModel.ModelId = lrModel.ModelId Then loWorkingComboboxItem = loComboBoxItem
                    End If
                Next
                loComboBoxItem = New tComboboxItem(Me.BostonModel.ModelId, Me.BostonModel.Name, Me.BostonModel)
                Me.ComboBoxModel.SelectedIndex = Me.ComboBoxModel.Items.IndexOf(loWorkingComboboxItem)
            End If

            If Me.BostonModel IsNot Nothing Then
                Me.Label1.Text = Me.BostonModel.Name
            End If

            If Me.SchemaQuery.Length = 0 Then
                'Me.SchemaQuery = New Connection("", "").GetQuery(Connection.QueryEnum.SchemaQuery)
            End If

            If Me.RoutineSchemaQuery.Length = 0 Then
                'Me.RoutineSchemaQuery = New Connection("", "").GetQuery(Connection.QueryEnum.RoutineSchemaQuery)
            End If

            'Trying to resolve problem where if two Project Sources open at the same time, one Project Source Panel for a Plugin won't show.
            Me.Dock = DockStyle.Fill
            Me.Parent.Parent.Visible = True
            Me.Parent.Visible = True
            Me.tcMain.Visible = True
            Me.Panel1.Visible = True
            Me.Panel2.Visible = True

        End Sub

        Private Sub txtTransformations_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            RaiseEvent ValueChanged(Me.txtTransformations.Text)
        End Sub

        Private Sub ComboBoxModel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxModel.SelectedIndexChanged

            Me.BostonModel = Me.ComboBoxModel.SelectedItem.Tag

            RaiseEvent ValueChanged(Me.ComboBoxModel.SelectedItem.ItemData)
        End Sub

        Private Sub TestConn()
            'Dim conn As Connection = New Connection("", Me.txtConnectionString.Text)
            'conn.TestConnection()
        End Sub

        Public Property ConnectionString As String Implements IManageSource.ConnectionString
            Get
                Return "None Required"
            End Get
            Set(ByVal value As String)
                'Nothing to do here
            End Set
        End Property

        Public Property SingleResultApproach As Boolean Implements IManageSource.SingleResultApproach
            Get
                Return True 'Me.rbApproachSingle.Checked
            End Get
            Set(ByVal value As Boolean)
                'Nothing to do here
                'Me.rbApproachSingle.Checked = value
                'Me.rbApproachTableColumn.Checked = Not value
            End Set
        End Property

        Public Property TableSchemaGeneric As Boolean Implements IManageSource.TableSchemaGeneric
            Get
                Return True 'Me.rbTableDefault.Checked
            End Get
            Set(ByVal value As Boolean)
                'Nothing to do here
                'Me.rbTableDefault.Checked = value
                'Me.rbTableQuery.Checked = Not value
            End Set
        End Property

        Public Property ColumnSchemaGeneric As Boolean Implements IManageSource.ColumnSchemaGeneric
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)
                'Nothing to do here
            End Set
        End Property

        Public Property SchemaQuery As String Implements IManageSource.SchemaQuery
            Get
                Return "Boston Model direct query. No need for a Schema Query here at all" 'Me.txtSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtSchemaQuery.Text = value
            End Set
        End Property

        Public Property TableSchemaQuery As String Implements IManageSource.TableSchemaQuery
            Get
                Return "Boston Model direct query. No need for a Schema Query here at all" 'Me.txtTableSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtTableSchemaQuery.Text = value
            End Set
        End Property

        Public Property ColumnSchemaQuery As String Implements IManageSource.ColumnSchemaQuery
            Get
                Return "Boston Model direct query. No need for a Schema Query here at all." 'Return Me.txtColumnSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtColumnSchemaQuery.Text = value
            End Set
        End Property

        Public Property TableName As String Implements IManageSource.TableName
            Get
                Return "Boston Model direct query. No need for a Schema Query here." 'Return Me.txtTableName.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtTableName.Text = value
            End Set
        End Property

        Public Property RoutineSchemaQuery As String Implements IManageSource.RoutineSchemaQuery
            Get
                Return "Boston Model direct query. No need for a Schema Query here." 'Return Me.txtRoutineSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtRoutineSchemaQuery.Text = value
            End Set
        End Property

        Public Property Transformations As String Implements IManageSource.Transformations
            Get
                Return Me.txtTransformations.Text
            End Get
            Set(ByVal value As String)
                Me.txtTransformations.Text = value
            End Set
        End Property

        Private Property IManageSource_Transformations As String
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public _BostonModel As FBM.Model
        Public Property BostonModel As Model Implements IManageSource.BostonModel
            Get
                Return Me._BostonModel
            End Get
            Set(value As Model)
                Me._BostonModel = value
            End Set
        End Property

        Private Sub Manage_Load(ByVal sender As Object, ByVal e As EventArgs)
            'Me.rbMeta_CheckedChanged(sender, e)
        End Sub

        Private Sub SavePress()
            RaiseEvent Save()
        End Sub

        Private Sub LoadERDModelDictionary()

            Try
                Dim loNode As New TreeNode

                loNode = Me.TreeView1.Nodes.Add("Entity", "Entity", 0, 0)
                loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.pageERD, Nothing)

                If prApplication.WorkingModel Is Nothing Then
                    Exit Sub
                End If

                Dim lrEntity As RDS.Table

                prApplication.WorkingModel.RDS.Table.Sort(AddressOf RDS.Table.CompareName)

                Dim loColumnNode As New TreeNode
                For Each lrEntity In prApplication.WorkingModel.RDS.Table
                    'loNode = New TreeNode
                    loNode = Me.TreeView1.Nodes("Entity").Nodes.Add("Entity" & lrEntity.Name, lrEntity.Name, 0, 0)
                    loNode.Tag = lrEntity

                    For Each lrAttribute In lrEntity.Column
                        loColumnNode = loNode.Nodes.Add("Attribute" & lrAttribute.Name, lrAttribute.Name, 14, 14)
                        loColumnNode.Tag = lrAttribute
                    Next

                Next

                Me.TreeView1.Nodes("Entity").Expand()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        'Private Sub txtSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtSchemaQuery.Text)
        'End Sub

        'Private Sub txtTableSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtTableSchemaQuery.Text)
        'End Sub

        'Private Sub txtColumnSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtColumnSchemaQuery.Text)
        'End Sub

        'Private Sub txtTableName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtTableName.Text)
        'End Sub

        'Private Sub txtRoutineSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtRoutineSchemaQuery.Text)
        'End Sub

        'Private Sub txtTransformations_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtTransformations.Text)
        'End Sub

        'Private Sub rbMeta_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '    Me.splitQuery.Panel1Collapsed = Not Me.rbApproachSingle.Checked
        '    Me.splitQuery.Panel2Collapsed = Me.rbApproachSingle.Checked
        '    Me.grpTableSchema.Visible = Not Me.rbApproachSingle.Checked
        '    Me.grpColumnSchema.Visible = Not Me.rbApproachSingle.Checked
        '    Me.splitTableColumn.Panel1Collapsed = Not Me.rbTableQuery.Checked
        '    Dim conn As Connection = New Connection("", "")

        '    If Me.rbApproachSingle.Checked Then
        '        If Me.SchemaQuery.Length = 0 Then Me.SchemaQuery = conn.GetQuery(Connection.QueryEnum.SchemaQuery)
        '        Me.txtTableSchemaQuery.Text = ""
        '        Me.txtColumnSchemaQuery.Text = ""
        '        Me.txtTableName.Text = ""
        '    Else
        '        Me.splitMain.Panel2Collapsed = True

        '        If Me.rbTableQuery.Checked Then
        '            If Me.txtTableSchemaQuery.Text.Length = 0 Then Me.txtTableSchemaQuery.Text = conn.GetQuery(Connection.QueryEnum.TableQuery)
        '        Else
        '            Me.txtTableSchemaQuery.Text = ""
        '        End If

        '        If Me.txtColumnSchemaQuery.Text.Length = 0 Then Me.txtColumnSchemaQuery.Text = conn.GetQuery(Connection.QueryEnum.ColumnQuery)

        '        If Me.txtTableName.Text.Length = 0 Then
        '            Me.txtTableName.Text = Metadrone.Persistence.Source.Default_TableNamePlaceHolder
        '        End If

        '        Me.SchemaQuery = ""
        '    End If
        'End Sub

        'Private Sub BuildConnectionString()
        '    If Me.chkPort.Checked Then
        '        Me.txtConnectionString.Text = "Server=" & Me.txtServer.Text & ";Port=" + Me.txtPort.Text & ";Database=" + Me.txtDatabase.Text & ";Uid=" + Me.txtUsername.Text & ";Pwd=" + Me.txtPassword.Text
        '    Else
        '        Me.txtConnectionString.Text = "Server=" & Me.txtServer.Text & ";Database=" + Me.txtDatabase.Text & ";Uid=" + Me.txtUsername.Text & ";Pwd=" + Me.txtPassword.Text
        '    End If
        'End Sub

        'Private Sub txtConnParts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        '    Me.BuildConnectionString()
        'End Sub

        'Private Sub chkPort_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        '    Me.txtPort.Enabled = Me.chkPort.Checked
        '    Me.BuildConnectionString()
        'End Sub

        'Private Sub txtConnectionString_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        '    RaiseEvent ValueChanged((CType(sender, TextBox)).Text)
        'End Sub

        'Private Sub btnTest_Click(ByVal sender As Object, ByVal e As EventArgs)
        '    Try
        '        Me.Cursor = Cursors.WaitCursor
        '        Me.TestConn()
        '        Me.Cursor = Cursors.[Default]
        '        MessageBox.Show("Test connection successful.", "Metadrone", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Catch ex As Exception
        '        Me.Cursor = Cursors.[Default]
        '        MessageBox.Show(ex.Message, "Metadrone", MessageBoxButtons.OK, MessageBoxIcon.[Stop], MessageBoxDefaultButton.Button1)
        '    End Try
        'End Sub

        'Private Sub lnkPreviewSchema_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        '    Me.TestQuery()
        'End Sub

        'Private Sub txtSchemaQuery_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '    If e.KeyCode = Keys.F5 Then
        '        Me.TestQuery()
        '        Me.txtSchemaQuery.Focus()
        '    End If
        'End Sub

        'Private Sub TestQuery()
        '    Me.splitMain.Panel2Collapsed = False
        '    Me.Cursor = Cursors.WaitCursor

        '    Try
        '        Me.QueryResults.PrepareSourceLoad()
        '        Dim dt As Connection = New Connection("", Me.txtConnectionString.Text)
        '        Me.QueryResults.SetSource(dt.TestQuery(Me.SchemaQuery))
        '    Catch ex As Exception
        '        Me.QueryResults.Messages = ex.Message
        '    End Try

        '    Me.Cursor = Cursors.[Default]
        'End Sub

        'Private Sub lnkPreviewRoutineSchema_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        '    Me.TestRoutineQuery()
        'End Sub

        'Private Sub txtRoutineSchemaQuery_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '    If e.KeyCode = Keys.F5 Then
        '        Me.TestRoutineQuery()
        '        Me.txtRoutineSchemaQuery.Focus()
        '    End If
        'End Sub

        'Private Sub TestRoutineQuery()
        '    Me.splitRoutine.Panel2Collapsed = False
        '    Me.Cursor = Cursors.WaitCursor

        '    Try
        '        Me.RoutineQueryResults.PrepareSourceLoad()
        '        Dim dt As Connection = New Connection("", Me.txtConnectionString.Text)
        '        Me.RoutineQueryResults.SetSource(dt.TestQuery(Me.RoutineSchemaQuery))
        '    Catch ex As Exception
        '        Me.RoutineQueryResults.Messages = ex.Message
        '    End Try

        '    Me.Cursor = Cursors.[Default]
        'End Sub

#Region "Form Initialisation"
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Manage))
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.GroupBox_Main = New System.Windows.Forms.GroupBox()
            Me.CheckBoxShowModelDictionary = New System.Windows.Forms.CheckBox()
            Me.CheckBoxShowCoreModelElements = New System.Windows.Forms.CheckBox()
            Me.LabelModelName = New System.Windows.Forms.Label()
            Me.LabelPrompt = New System.Windows.Forms.Label()
            Me.TreeView1 = New System.Windows.Forms.TreeView()
            Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.ComboBoxModel = New System.Windows.Forms.ComboBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.LabelPromptWorkingModel = New System.Windows.Forms.Label()
            Me.LabelWorkingModel = New System.Windows.Forms.Label()
            Me.tcMain = New System.Windows.Forms.TabControl()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.txtTransformations = New UI.TransformationsEditor()
            Me.TabPage2.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.GroupBox_Main.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.tcMain.SuspendLayout()
            Me.TabPage3.SuspendLayout()
            Me.SuspendLayout()
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.Panel1)
            Me.TabPage2.ImageIndex = 1
            Me.TabPage2.Location = New System.Drawing.Point(4, 4)
            Me.TabPage2.Margin = New System.Windows.Forms.Padding(0)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Size = New System.Drawing.Size(769, 553)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Tables/Columns"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.GroupBox_Main)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel1.Location = New System.Drawing.Point(0, 0)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(769, 553)
            Me.Panel1.TabIndex = 0
            '
            'GroupBox_Main
            '
            Me.GroupBox_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.GroupBox_Main.Controls.Add(Me.CheckBoxShowModelDictionary)
            Me.GroupBox_Main.Controls.Add(Me.CheckBoxShowCoreModelElements)
            Me.GroupBox_Main.Controls.Add(Me.LabelModelName)
            Me.GroupBox_Main.Controls.Add(Me.LabelPrompt)
            Me.GroupBox_Main.Controls.Add(Me.TreeView1)
            Me.GroupBox_Main.Dock = System.Windows.Forms.DockStyle.Fill
            Me.GroupBox_Main.ForeColor = System.Drawing.Color.Black
            Me.GroupBox_Main.Location = New System.Drawing.Point(0, 0)
            Me.GroupBox_Main.Name = "GroupBox_Main"
            Me.GroupBox_Main.Size = New System.Drawing.Size(769, 553)
            Me.GroupBox_Main.TabIndex = 1
            Me.GroupBox_Main.TabStop = False
            Me.GroupBox_Main.Text = "Model Dictionary:"
            '
            'CheckBoxShowModelDictionary
            '
            Me.CheckBoxShowModelDictionary.AutoSize = True
            Me.CheckBoxShowModelDictionary.Location = New System.Drawing.Point(183, 26)
            Me.CheckBoxShowModelDictionary.Name = "CheckBoxShowModelDictionary"
            Me.CheckBoxShowModelDictionary.Size = New System.Drawing.Size(135, 17)
            Me.CheckBoxShowModelDictionary.TabIndex = 5
            Me.CheckBoxShowModelDictionary.Text = "Show Model Dictionary"
            Me.CheckBoxShowModelDictionary.UseVisualStyleBackColor = True
            Me.CheckBoxShowModelDictionary.Visible = False
            '
            'CheckBoxShowCoreModelElements
            '
            Me.CheckBoxShowCoreModelElements.AutoSize = True
            Me.CheckBoxShowCoreModelElements.Location = New System.Drawing.Point(183, 9)
            Me.CheckBoxShowCoreModelElements.Name = "CheckBoxShowCoreModelElements"
            Me.CheckBoxShowCoreModelElements.Size = New System.Drawing.Size(156, 17)
            Me.CheckBoxShowCoreModelElements.TabIndex = 4
            Me.CheckBoxShowCoreModelElements.Text = "Show Core Model Elements"
            Me.CheckBoxShowCoreModelElements.UseVisualStyleBackColor = True
            Me.CheckBoxShowCoreModelElements.Visible = False
            '
            'LabelModelName
            '
            Me.LabelModelName.AutoSize = True
            Me.LabelModelName.Location = New System.Drawing.Point(53, 25)
            Me.LabelModelName.Name = "LabelModelName"
            Me.LabelModelName.Size = New System.Drawing.Size(102, 13)
            Me.LabelModelName.TabIndex = 3
            Me.LabelModelName.Text = "<LabelModelName>"
            '
            'LabelPrompt
            '
            Me.LabelPrompt.AutoSize = True
            Me.LabelPrompt.Location = New System.Drawing.Point(17, 25)
            Me.LabelPrompt.Name = "LabelPrompt"
            Me.LabelPrompt.Size = New System.Drawing.Size(39, 13)
            Me.LabelPrompt.TabIndex = 2
            Me.LabelPrompt.Text = "Model:"
            '
            'TreeView1
            '
            Me.TreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.TreeView1.ImageIndex = 0
            Me.TreeView1.ImageList = Me.ImageList
            Me.TreeView1.Location = New System.Drawing.Point(3, 49)
            Me.TreeView1.Name = "TreeView1"
            Me.TreeView1.SelectedImageIndex = 0
            Me.TreeView1.Size = New System.Drawing.Size(760, 498)
            Me.TreeView1.TabIndex = 1
            '
            'ImageList
            '
            Me.ImageList.ImageStream = CType(resources.GetObject("ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ImageList.TransparentColor = System.Drawing.Color.Transparent
            Me.ImageList.Images.SetKeyName(0, "model_dictionary_entity_type")
            Me.ImageList.Images.SetKeyName(1, "model_dictionary_value_type")
            Me.ImageList.Images.SetKeyName(2, "model_dictionary_FactType")
            Me.ImageList.Images.SetKeyName(3, "ObjectifiedFactType")
            Me.ImageList.Images.SetKeyName(4, "ExclusionConstraint")
            Me.ImageList.Images.SetKeyName(5, "EqualityConstraint")
            Me.ImageList.Images.SetKeyName(6, "SubsetConstraint")
            Me.ImageList.Images.SetKeyName(7, "InclusiveOrConstraint")
            Me.ImageList.Images.SetKeyName(8, "ExclusiveOrConstraint")
            Me.ImageList.Images.SetKeyName(9, "FrequencyConstraint")
            Me.ImageList.Images.SetKeyName(10, "RingConstraint")
            Me.ImageList.Images.SetKeyName(11, "ExternalUniquenessConstraint")
            Me.ImageList.Images.SetKeyName(12, "ERD-16-16.png")
            Me.ImageList.Images.SetKeyName(13, "ERDEntity16x16.png")
            Me.ImageList.Images.SetKeyName(14, "Attribute.png")
            Me.ImageList.Images.SetKeyName(15, "EntityTypeDerived")
            Me.ImageList.Images.SetKeyName(16, "FactTypeUnary")
            Me.ImageList.Images.SetKeyName(17, "SubtypeRelationship")
            Me.ImageList.Images.SetKeyName(18, "FactTypeDerived")
            Me.ImageList.Images.SetKeyName(19, "FactTypeUnaryDerived")
            Me.ImageList.Images.SetKeyName(20, "PreferredExternalUniqueness")
            Me.ImageList.Images.SetKeyName(21, "PGSNode.png")
            Me.ImageList.Images.SetKeyName(22, "PGSRelation.png")
            '
            'TabPage1
            '
            Me.TabPage1.BackColor = System.Drawing.Color.Transparent
            Me.TabPage1.Controls.Add(Me.ComboBoxModel)
            Me.TabPage1.Controls.Add(Me.Label1)
            Me.TabPage1.Controls.Add(Me.Panel2)
            Me.TabPage1.Controls.Add(Me.lblTitle)
            Me.TabPage1.Controls.Add(Me.LabelPromptWorkingModel)
            Me.TabPage1.Controls.Add(Me.LabelWorkingModel)
            Me.TabPage1.ImageIndex = 0
            Me.TabPage1.Location = New System.Drawing.Point(4, 4)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(769, 553)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Connection"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'ComboBoxModel
            '
            Me.ComboBoxModel.FormattingEnabled = True
            Me.ComboBoxModel.Location = New System.Drawing.Point(108, 87)
            Me.ComboBoxModel.Name = "ComboBoxModel"
            Me.ComboBoxModel.Size = New System.Drawing.Size(210, 21)
            Me.ComboBoxModel.TabIndex = 6
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(71, 128)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(39, 13)
            Me.Label1.TabIndex = 5
            Me.Label1.Text = "Label1"
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.Silver
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel2.Location = New System.Drawing.Point(3, 33)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(763, 1)
            Me.Panel2.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.BackColor = System.Drawing.Color.White
            Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.DimGray
            Me.lblTitle.Location = New System.Drawing.Point(3, 3)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Padding = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.lblTitle.Size = New System.Drawing.Size(763, 30)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Boston Model - Direct Access"
            '
            'LabelPromptWorkingModel
            '
            Me.LabelPromptWorkingModel.AutoSize = True
            Me.LabelPromptWorkingModel.Location = New System.Drawing.Point(17, 58)
            Me.LabelPromptWorkingModel.Name = "LabelPromptWorkingModel"
            Me.LabelPromptWorkingModel.Size = New System.Drawing.Size(82, 13)
            Me.LabelPromptWorkingModel.TabIndex = 0
            Me.LabelPromptWorkingModel.Text = "Working Model:"
            '
            'LabelWorkingModel
            '
            Me.LabelWorkingModel.AutoSize = True
            Me.LabelWorkingModel.Location = New System.Drawing.Point(105, 58)
            Me.LabelWorkingModel.Name = "LabelWorkingModel"
            Me.LabelWorkingModel.Size = New System.Drawing.Size(86, 13)
            Me.LabelWorkingModel.TabIndex = 4
            Me.LabelWorkingModel.Text = "lblWorkingModel"
            '
            'tcMain
            '
            Me.tcMain.Alignment = System.Windows.Forms.TabAlignment.Bottom
            Me.tcMain.Controls.Add(Me.TabPage1)
            Me.tcMain.Controls.Add(Me.TabPage2)
            Me.tcMain.Controls.Add(Me.TabPage3)
            Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcMain.Location = New System.Drawing.Point(0, 0)
            Me.tcMain.Name = "tcMain"
            Me.tcMain.SelectedIndex = 0
            Me.tcMain.Size = New System.Drawing.Size(777, 579)
            Me.tcMain.TabIndex = 2
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.txtTransformations)
            Me.TabPage3.Location = New System.Drawing.Point(4, 4)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Size = New System.Drawing.Size(769, 553)
            Me.TabPage3.TabIndex = 2
            Me.TabPage3.Text = "Transformations"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'txtTransformations
            '
            Me.txtTransformations.BackColor = System.Drawing.SystemColors.Window
            Me.txtTransformations.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtTransformations.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtTransformations.Location = New System.Drawing.Point(0, 0)
            Me.txtTransformations.Name = "txtTransformations"
            Me.txtTransformations.ReadOnly = False
            Me.txtTransformations.SelectedText = ""
            Me.txtTransformations.SelectionLength = 0
            Me.txtTransformations.SelectionStart = 0
            Me.txtTransformations.Size = New System.Drawing.Size(769, 553)
            Me.txtTransformations.TabIndex = 2
            '
            'Manage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.tcMain)
            Me.Name = "Manage"
            Me.Size = New System.Drawing.Size(777, 579)
            Me.TabPage2.ResumeLayout(False)
            Me.Panel1.ResumeLayout(False)
            Me.GroupBox_Main.ResumeLayout(False)
            Me.GroupBox_Main.PerformLayout()
            Me.TabPage1.ResumeLayout(False)
            Me.TabPage1.PerformLayout()
            Me.tcMain.ResumeLayout(False)
            Me.TabPage3.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents TabPage2 As TabPage
        Friend WithEvents TabPage1 As TabPage
        Friend WithEvents lblTitle As Label
        Friend WithEvents LabelPromptWorkingModel As Label
        Friend WithEvents LabelWorkingModel As Label
        Friend WithEvents tcMain As TabControl
        Friend WithEvents Panel1 As Panel
        Friend WithEvents Panel2 As Panel
        Friend WithEvents GroupBox_Main As GroupBox
        Friend WithEvents CheckBoxShowModelDictionary As CheckBox
        Friend WithEvents CheckBoxShowCoreModelElements As CheckBox
        Friend WithEvents LabelModelName As Label
        Friend WithEvents LabelPrompt As Label
        Friend WithEvents TreeView1 As TreeView
        Friend WithEvents ImageList As ImageList
        Private components As IContainer
        Friend WithEvents Label1 As Label
        Friend WithEvents ComboBoxModel As ComboBox
        Friend WithEvents TabPage3 As TabPage
        Friend WithEvents txtTransformations As UI.TransformationsEditor

#End Region

    End Class
End Namespace

