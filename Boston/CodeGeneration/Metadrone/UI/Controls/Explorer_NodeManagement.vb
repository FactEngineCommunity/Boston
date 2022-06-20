Imports Boston.Persistence

Namespace UI

    Partial Friend Class Explorer

        Private Delegate Sub Sort()

        Friend Function GetPackageOwnerNode(ByVal node As TreeNode) As TreeNode
            If node Is Nothing Then Return Nothing
            If TypeOf node.Tag Is MDPackageTag Then Return node

            Dim pn As TreeNode = node.Parent
            If pn Is Nothing Then Return Nothing
            If TypeOf pn.Tag Is MDPackageTag Then
                Return pn
            Else
                Return Me.GetPackageOwnerNode(pn)
            End If
        End Function

        Friend Function GetPackageOwnerGUID(ByVal node As TreeNode) As String
            Dim pn As TreeNode = Me.GetPackageOwnerNode(node)

            If pn Is Nothing Then
                Return Nothing
            Else
                Return CType(pn.Tag, MDPackageTag).GUID
            End If
        End Function

        Private Function GetPackageOwner(ByVal node As TreeNode) As MDPackage
            Return Me.GetPackage("", Me.GetPackageOwnerNode(node))
        End Function


        'Change context menu according to selected node
        Private Sub HandleNodeSelect(ByVal Node As TreeNode)
            Me.tvwMain.SelectedNode = Node

            Me.tvwMain.ContextMenuStrip = Nothing
            Me.mniAdd.Visible = False
            Me.mniAddNewPackage.Visible = False
            Me.mniAddNewSource.Visible = False
            Me.mniAddNewFolder.Visible = False
            Me.mniAddNewProjectFolder.Visible = False
            Me.mniAddNewTemplate.Visible = False
            Me.mniAddNewCS.Visible = False
            Me.mniAddNewVB.Visible = False
            Me.mniOpen.Visible = False
            Me.mniDelete.Visible = False
            Me.mniRename.Visible = False
            Me.tvwMain.LabelEdit = False
            Me.mniSep1.Visible = False
            Me.mniCut.Visible = False
            Me.mniCopy.Visible = False
            Me.mniPaste.Visible = False
            Me.mniPaste.Enabled = False

            Me.tvwMain.ContextMenuStrip = Me.mnuMain

            If Node.Tag.ToString.Equals(TAG_PROJECT) Then
                Me.mniAdd.Visible = True
                Me.mniAddNewPackage.Visible = True
                Me.mniAddNewSource.Visible = True
                Me.mniAddNewProjectFolder.Visible = True
                Me.mniRename.Visible = True
                Me.tvwMain.LabelEdit = True
                Me.mniSep1.Visible = True
                Me.mniPaste.Visible = True
                Me.mniPaste.Enabled = Me.DisablePaste(Node)

            ElseIf Node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                Me.mniAdd.Visible = True
                Me.mniAddNewPackage.Visible = True
                Me.mniAddNewSource.Visible = True
                Me.mniAddNewProjectFolder.Visible = True
                Me.mniRename.Visible = True
                Me.tvwMain.LabelEdit = True
                Me.mniDelete.Visible = True
                Me.mniSep1.Visible = True
                Me.mniCut.Visible = True
                Me.mniCopy.Visible = True
                Me.mniPaste.Visible = True
                Me.mniPaste.Enabled = Me.DisablePaste(Node)

            ElseIf TypeOf Node.Tag Is Source Then
                Me.mniOpen.Visible = True
                Me.mniDelete.Visible = True
                Me.mniRename.Visible = True
                Me.tvwMain.LabelEdit = True
                Me.mniSep1.Visible = True
                Me.mniCut.Visible = True
                Me.mniCopy.Visible = True

            ElseIf Node.Tag.ToString.Equals(TAG_FOLDER) Then
                Me.mniAdd.Visible = True
                Me.mniAddNewFolder.Visible = True
                Me.mniAddNewTemplate.Visible = True
                Me.mniAddNewCS.Visible = True
                Me.mniAddNewVB.Visible = True
                Me.mniDelete.Visible = True
                Me.mniRename.Visible = True
                Me.tvwMain.LabelEdit = True
                Me.mniSep1.Visible = True
                Me.mniCut.Visible = True
                Me.mniCopy.Visible = True
                Me.mniPaste.Visible = True
                Me.mniPaste.Enabled = Me.DisablePaste(Node)

            ElseIf TypeOf Node.Tag Is MDPackageTag Then
                Me.mniAdd.Visible = True
                Me.mniAddNewFolder.Visible = True
                Me.mniAddNewTemplate.Visible = True
                Me.mniAddNewCS.Visible = True
                Me.mniAddNewVB.Visible = True
                Me.mniDelete.Visible = True
                Me.mniRename.Visible = True
                Me.tvwMain.LabelEdit = True
                Me.mniSep1.Visible = True
                Me.mniCut.Visible = True
                Me.mniCopy.Visible = True
                Me.mniPaste.Visible = True
                Me.mniPaste.Enabled = Me.DisablePaste(Node)

            ElseIf TypeOf Node.Tag Is Persistence.Properties Then
                Me.mniOpen.Visible = True

            ElseIf TypeOf Node.Tag Is Main Then
                Me.mniOpen.Visible = True

            ElseIf TypeOf Node.Tag Is Template Then
                Me.mniOpen.Visible = True
                Me.mniDelete.Visible = True
                Me.mniRename.Visible = True
                Me.tvwMain.LabelEdit = True
                Me.mniSep1.Visible = True
                Me.mniCut.Visible = True
                Me.mniCopy.Visible = True

            ElseIf TypeOf Node.Tag Is CodeDOM_VB Then
                Me.mniOpen.Visible = True
                Me.mniDelete.Visible = True
                Me.mniRename.Visible = True
                Me.tvwMain.LabelEdit = True
                Me.mniSep1.Visible = True
                Me.mniCut.Visible = True
                Me.mniCopy.Visible = True

            ElseIf TypeOf Node.Tag Is CodeDOM_CS Then
                Me.mniOpen.Visible = True
                Me.mniDelete.Visible = True
                Me.mniRename.Visible = True
                Me.tvwMain.LabelEdit = True
                Me.mniSep1.Visible = True
                Me.mniCut.Visible = True
                Me.mniCopy.Visible = True

            End If
        End Sub



        Public Function CreateNewProject() As Boolean
            Dim frm As New NewItem(NewItem.Types.Project, Nothing, Nothing, Nothing, Nothing)
            If frm.ShowDialog = System.Windows.Forms.DialogResult.Cancel Then Return False

            Dim projectName As String = frm.txtProjectName.Text
            If String.IsNullOrEmpty(projectName) Then Return False

            Me.tvwMain.Nodes.Clear()

            Dim newNode As New TreeNode(projectName, IMG_PROJECT, IMG_PROJECT)
            newNode.Tag = TAG_PROJECT

            'Add properties
            Dim propNode As New TreeNode("Properties", IMG_PROPERTIES, IMG_PROPERTIES)
            propNode.Tag = New Persistence.Properties()
            'Super main calling the package
            CType(propNode.Tag, Persistence.Properties).SuperMain = Parser.Syntax.Constants.ACTION_CALL & " " & frm.txtPackageName.Text
            newNode.Nodes.Add(propNode)

            'With default Source
            Dim srcNode As TreeNode = Me.NewSourceNode("Source")
            If srcNode IsNot Nothing Then newNode.Nodes.Add(srcNode)

            'And default package
            Dim pkgNode As TreeNode = Me.NewPackage(frm.txtPackageName.Text)
            If pkgNode IsNot Nothing Then newNode.Nodes.Add(pkgNode)

            Me.tvwMain.Nodes.Add(newNode)
            Me.tvwMain.TopNode = Me.tvwMain.Nodes(0)
            Me.tvwMain.TopNode.Expand()

            CType(Me.ParentForm, MainForm).projDirty = False

            RaiseEvent NewProject()
            Return True
        End Function

        Public Function AddNewItem(ByVal templateText As System.Text.StringBuilder) As Boolean
            Dim newNode As TreeNode

            'Blank - create new project
            If Me.tvwMain.Nodes.Count = 0 Then
                Dim res = Me.CreateNewProject() : If Not res Then Return False

                'If not adding template lib text, finish here
                If templateText Is Nothing Then Return res
            End If

            'If adding a template lib item,
            'look for the first available package if one not selected (or a package folder not selected)
            If templateText IsNot Nothing Then
                Dim foundHim As Boolean = False

                If Me.tvwMain.SelectedNode Is Nothing Then
                    For Each node As TreeNode In Me.tvwMain.Nodes(0).Nodes
                        If TypeOf node.Tag Is MDPackageTag Then
                            Me.tvwMain.SelectedNode = node
                            foundHim = True
                            Exit For
                        End If
                    Next
                Else
                    'Check if selected node is valid
                    If Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_PROJECT) OrElse _
                       Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        'Go to first package
                        For Each node As TreeNode In Me.tvwMain.Nodes(0).Nodes
                            If TypeOf node.Tag Is MDPackageTag Then
                                Me.tvwMain.SelectedNode = node
                                foundHim = True
                                Exit For
                            End If
                        Next
                    ElseIf TypeOf Me.tvwMain.SelectedNode.Tag Is MDPackageTag Or _
                           Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_FOLDER) Then
                        'This is fine
                        foundHim = True
                    ElseIf TypeOf Me.tvwMain.SelectedNode.Tag Is Template Or _
                           TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_VB Or _
                           TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_CS Then
                        'Look up to the package owner
                        While True
                            If Me.tvwMain.SelectedNode.Parent Is Nothing Then Exit While
                            Me.tvwMain.SelectedNode = Me.tvwMain.SelectedNode.Parent
                            If TypeOf Me.tvwMain.SelectedNode.Tag Is MDPackageTag Or _
                               Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_FOLDER) Then
                                foundHim = True
                                Exit While
                            End If
                        End While
                    End If
                End If

                'Cancel if not successful
                If Not foundHim Then Return False
            End If

            'Can create new project/template
            Dim frm As New NewItem(NewItem.Types.ProjectChild, _
                                   Me.GetPackageOwner(Me.tvwMain.SelectedNode), _
                                   Me.GetPackages(), Me.GetSources(), _
                                   templateText)

            If templateText IsNot Nothing Then
                'Template to add
                frm.Type = NewItem.Types.TemplateOnly

            ElseIf Me.tvwMain.SelectedNode Is Nothing Then
                'Don't need to alter new item type

            ElseIf Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_PROJECT) OrElse
                   Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_PROJECT_FOLDER) OrElse
                   TypeOf Me.tvwMain.SelectedNode.Tag Is Source OrElse
                   TypeOf Me.tvwMain.SelectedNode.Tag Is Persistence.Properties OrElse
                   TypeOf Me.tvwMain.SelectedNode.Tag Is Main Then
                'Don't allow add item to project, project folder, source, or properties/main nodes
                frm.Type = NewItem.Types.ProjectChild

            ElseIf TypeOf Me.tvwMain.SelectedNode.Tag Is MDPackageTag Or _
                   Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_FOLDER) Or _
                   TypeOf Me.tvwMain.SelectedNode.Tag Is Template Or _
                   TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_VB Or _
                   TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_CS Then
                frm.Type = NewItem.Types.PackageChild
                'Default to add..
                If TypeOf Me.tvwMain.SelectedNode.Tag Is Template Then frm.rbTemplate.Checked = True
                If TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_VB Then frm.rbVB.Checked = True
                If TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_CS Then frm.rbCS.Checked = True

                'Apply to parent node of item
                If TypeOf Me.tvwMain.SelectedNode.Tag Is Template Or _
                   TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_VB Or _
                   TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_CS Then
                    Me.tvwMain.SelectedNode = Me.tvwMain.SelectedNode.Parent
                End If

            End If

            If frm.ShowDialog = System.Windows.Forms.DialogResult.Cancel Then Return False

            If frm.rbPackage.Checked Then
                If frm.chkNewProject.Checked Then
                    Dim projectName As String = frm.txtProjectName.Text
                    If String.IsNullOrEmpty(projectName) Then Return False

                    If CType(Me.ParentForm, MainForm).IsDirty Then
                        Select Case MessageBox.Show("Do you want to save changes?", "Save changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                            Case System.Windows.Forms.DialogResult.Yes
                                If CType(Me.ParentForm, MainForm).WillSaveProject Then
                                    Me.Cursor = Cursors.WaitCursor
                                    For i As Integer = 0 To CType(Me.ParentForm, MainForm).tcMain.TabPages.Count - 1
                                        Call CType(Me.ParentForm, MainForm).CopyTabDataBackIntoNode(CType(Me.ParentForm, MainForm).tcMain.TabPages(i))
                                    Next
                                    Call CType(Me.ParentForm, MainForm).SaveProject()
                                    Me.Cursor = Cursors.Default
                                End If

                            Case System.Windows.Forms.DialogResult.No

                            Case System.Windows.Forms.DialogResult.Cancel
                                Return False

                        End Select
                    End If

                    'New Project
                    Me.tvwMain.Nodes.Clear()

                    newNode = New TreeNode(projectName, IMG_PROJECT, IMG_PROJECT)
                    newNode.Tag = TAG_PROJECT

                    'Add properties
                    Dim propNode As New TreeNode("Properties", IMG_PROPERTIES, IMG_PROPERTIES)
                    propNode.Tag = New Persistence.Properties()
                    'Super main calling the package
                    CType(propNode.Tag, Persistence.Properties).SuperMain = Parser.Syntax.Constants.ACTION_CALL & " " & frm.txtPackageName.Text
                    newNode.Nodes.Add(propNode)

                    'With default Source
                    Dim srcNode As TreeNode = Me.NewSourceNode("Source")
                    If srcNode IsNot Nothing Then newNode.Nodes.Add(srcNode)

                    'And default package
                    Dim pkgNode As TreeNode = Me.NewPackage(frm.txtPackageName.Text)
                    If pkgNode IsNot Nothing Then
                        newNode.Nodes.Add(pkgNode)
                    End If

                    Me.tvwMain.Nodes.Add(newNode)
                    Me.tvwMain.TopNode = Me.tvwMain.Nodes(0)
                    Me.tvwMain.TopNode.Expand()

                    RaiseEvent NewProject()
                Else
                    'Just add new package
                    Call Me.AddNewPackage(frm.txtPackageName.Text)
                End If

            ElseIf frm.rbDBSource.Checked Then
                'New source
                Call Me.AddNewSource(frm.txtDBSourceName.Text)

            ElseIf frm.rbTemplate.Checked Then
                'New template
                Dim mdp As New MDPackage("", True)
                Dim templ As Template = mdp.NewTemplate(frm.txtTemplate.Text, Me.GetPackageOwnerGUID(Me.tvwMain.SelectedNode))
                If frm.DownloadedTemplateText IsNot Nothing Then templ.Text = frm.DownloadedTemplateText.ToString
                newNode = New TreeNode(templ.Name, IMG_TEMPLATE, IMG_TEMPLATE)
                newNode.Tag = templ
                Call Me.AddNewTemplate(templ)

            ElseIf frm.rbVB.Checked Then
                'New VB code file
                Dim mdp As New MDPackage("", True)
                Dim vb As CodeDOM_VB = mdp.NewCodeDOM_VB(frm.txtCodeFileName.Text, Me.GetPackageOwnerGUID(Me.tvwMain.SelectedNode))
                newNode = New TreeNode(vb.Name, IMG_VB, IMG_VB)
                newNode.Tag = vb
                Call Me.AddNewCodeDOM_VB(vb)

            ElseIf frm.rbCS.Checked Then
                'New CS code file
                Dim mdp As New MDPackage("", True)
                Dim cs As CodeDOM_CS = mdp.NewCodeDOM_CS(frm.txtCodeFileName.Text, Me.GetPackageOwnerGUID(Me.tvwMain.SelectedNode))
                newNode = New TreeNode(cs.Name, IMG_CS, IMG_CS)
                newNode.Tag = cs
                Call Me.AddNewCodeDOM_CS(cs)

            End If

            Return True
        End Function

        'Create a new Package node, with default main, and a template
        Private Function NewPackage(ByVal packageName As String) As TreeNode
            If String.IsNullOrEmpty(packageName) Then packageName = InputBox("Package name:", "New Package", "Package")
            If String.IsNullOrEmpty(packageName) Then Return Nothing

            Dim mdp As New MDPackage(packageName, True)

            Dim rootNode As New TreeNode(packageName, IMG_PACKAGE, IMG_PACKAGE)
            Dim mainNode As New TreeNode("Main", IMG_MAIN, IMG_MAIN)
            Dim templateNode As New TreeNode(mdp.Templates(0).Name, IMG_TEMPLATE, IMG_TEMPLATE)
            rootNode.Tag = mdp.TagVal
            mainNode.Tag = mdp.Main
            templateNode.Tag = mdp.Templates(0)

            rootNode.Nodes.Add(mainNode)
            rootNode.Nodes.Add(templateNode)
            rootNode.Expand()

            Return rootNode
        End Function

        Public Sub AddNewPackage(ByVal packageName As String)
            'Add new package to root node
            Dim node As TreeNode = Me.NewPackage(packageName)
            If node IsNot Nothing Then
                node.Expand()

                If Me.tvwMain.SelectedNode IsNot Nothing AndAlso Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                    'To selected project folder node
                    Call Me.InsertAlphabetically(Me.tvwMain.SelectedNode, node)
                Else
                    'To root node
                    Call Me.InsertAlphabetically(Me.tvwMain.TopNode, node)
                End If

                RaiseEvent NewPackageItem(packageName)
            End If
        End Sub

        'Create a new Source node
        Private Function NewSourceNode(ByVal SourceName As String) As TreeNode
            If String.IsNullOrEmpty(SourceName) Then SourceName = InputBox("Source name:", "New Source", "Source")
            If String.IsNullOrEmpty(SourceName) Then Return Nothing

            'Create source defaulted as SQL server (1st provider)
            Dim src As New Source()
            src.Name = SourceName
            src.Provider = PluginInterface.Sources.Descriptions.SQLSERVER.ProviderName

            Dim manDB As New ManageDBSQLServer()
            With manDB
                .Setup()
                'src.ConnectionString = 'leave connection string empty until user modifies it
                src.SchemaQuery = .SchemaQuery
                src.TableSchemaQuery = .TableSchemaQuery
                src.ColumnSchemaQuery = .ColumnSchemaQuery
                src.TableNamePlaceHolder = .txtTableName.Text
                src.RoutineSchemaQuery = .RoutineSchemaQuery
                src.Transformations = .Transformations
            End With

            Dim Node As New TreeNode(SourceName, IMG_Source, IMG_Source)
            Node.Tag = src

            Return Node
        End Function

        Public Sub AddNewSource(ByVal SourceName As String)
            'Add new Source node
            Dim node As TreeNode = Me.NewSourceNode(SourceName)

            If Me.tvwMain.SelectedNode IsNot Nothing AndAlso Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                'To selected project folder node
                Call Me.InsertAlphabetically(Me.tvwMain.SelectedNode, node)
            Else
                'To root node
                Call Me.InsertAlphabetically(Me.tvwMain.TopNode, node)
            End If

            RaiseEvent NewSource()
        End Sub

        Public Sub AddNewFolder()
            'Just add folder node
            Dim folderName As String = InputBox("Folder name:", "New Folder", "Folder")
            If String.IsNullOrEmpty(folderName) Then Exit Sub
            Dim Node As New TreeNode(folderName, IMG_FOLDER_CLOSED, IMG_FOLDER_CLOSED)
            Node.Tag = TAG_FOLDER

            Dim atIndex As Integer = 0
            Dim toIndex As Integer = Me.tvwMain.SelectedNode.Nodes.Count - 1

            'Add after properties/main nodes
            If TypeOf Me.tvwMain.SelectedNode.Tag Is MDPackageTag Then
                atIndex = 2
            End If

            'Don't insert within any templates
            Dim noFolders As Boolean = True
            For i As Integer = atIndex To toIndex
                toIndex = i
                If Not Me.tvwMain.SelectedNode.Nodes(i).Tag.ToString.Equals(TAG_FOLDER) Then
                    noFolders = False
                    Exit For
                End If
            Next

            Call Me.InsertAlphabetically(Me.tvwMain.SelectedNode, Node)

            RaiseEvent NewFolder()
        End Sub

        Public Sub AddNewProjectFolder()
            'Just add folder node
            Dim folderName As String = InputBox("Folder name:", "New Folder", "Folder")
            If String.IsNullOrEmpty(folderName) Then Exit Sub
            Dim Node As New TreeNode(folderName, IMG_PROJECT_FOLDER_CLOSED, IMG_PROJECT_FOLDER_CLOSED)
            Node.Tag = TAG_PROJECT_FOLDER

            Call Me.InsertAlphabetically(Me.tvwMain.SelectedNode, Node)

            RaiseEvent NewProjectFolder()
        End Sub

        Public Sub AddNewTemplate(ByVal Template As Persistence.Template)
            'Prompt for new template if not specified
            If Template Is Nothing Then
                Dim frm As New NewItem(NewItem.Types.TemplateOnly, Me.GetPackageOwner(Me.tvwMain.SelectedNode), Nothing, Nothing, Nothing)
                If frm.ShowDialog = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
                If frm.txtTemplate.Text.Length = 0 Then Exit Sub

                'Add template
                Dim mdp As New MDPackage("", True)
                Template = mdp.NewTemplate(frm.txtTemplate.Text, Me.GetPackageOwnerGUID(Me.tvwMain.SelectedNode))
                If frm.DownloadedTemplateText IsNot Nothing Then Template.Text = frm.DownloadedTemplateText.ToString
            End If

            'Add template
            Dim Node As New TreeNode(Template.Name, IMG_TEMPLATE, IMG_TEMPLATE)
            Node.Tag = Template

            Dim atIndex As Integer = 0
            Dim toIndex As Integer = Me.tvwMain.SelectedNode.Nodes.Count - 1

            'Add after properties/main nodes
            If TypeOf Me.tvwMain.SelectedNode.Tag Is MDPackageTag Then
                atIndex = 2
            End If

            'Don't insert within any folders
            For i As Integer = atIndex To toIndex
                If Not Me.tvwMain.SelectedNode.Nodes(i).Tag.ToString.Equals(TAG_FOLDER) Then
                    atIndex = i
                    Exit For
                End If
            Next

            'Add node to tree view
            Call Me.InsertAlphabetically(Me.tvwMain.SelectedNode, Node)

            'Get main node
            Dim pkgNode As TreeNode = Me.GetPackageOwnerNode(Me.tvwMain.SelectedNode)
            Dim mnNode As TreeNode = Nothing
            For Each n As TreeNode In pkgNode.Nodes
                If TypeOf n.Tag Is Main Then
                    mnNode = n
                    Exit For
                End If
            Next

            'Tell the others!
            RaiseEvent NewTemplate(Template, mnNode)
        End Sub

        Public Sub AddNewCodeDOM_VB(ByVal vb As Persistence.CodeDOM_VB)
            'Prompt for new code file if not specified
            If vb Is Nothing Then
                Dim frm As New NewItem(NewItem.Types.VBCodeOnly, Me.GetPackageOwner(Me.tvwMain.SelectedNode), Nothing, Nothing, Nothing)
                If frm.ShowDialog = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
                If frm.txtCodeFileName.Text.Length = 0 Then Exit Sub

                'Add code file
                Dim mdp As New MDPackage("", True)
                vb = mdp.NewCodeDOM_VB(frm.txtCodeFileName.Text, Me.GetPackageOwnerGUID(Me.tvwMain.SelectedNode))
            End If

            'Add code file
            Dim Node As New TreeNode(vb.Name, IMG_VB, IMG_VB)
            Node.Tag = vb

            Dim atIndex As Integer = 0
            Dim toIndex As Integer = Me.tvwMain.SelectedNode.Nodes.Count - 1

            'Add after properties/main nodes
            If TypeOf Me.tvwMain.SelectedNode.Tag Is MDPackageTag Then
                atIndex = 2
            End If

            'Don't insert within any folders
            For i As Integer = atIndex To toIndex
                If Not Me.tvwMain.SelectedNode.Nodes(i).Tag.ToString.Equals(TAG_FOLDER) Then
                    atIndex = i
                    Exit For
                End If
            Next

            'Add node to tree view
            Call Me.InsertAlphabetically(Me.tvwMain.SelectedNode, Node)

            RaiseEvent NewCodeDOM_VB()
        End Sub

        Public Sub AddNewCodeDOM_CS(ByVal cs As Persistence.CodeDOM_CS)
            'Prompt for new code file if not specified
            If cs Is Nothing Then
                Dim frm As New NewItem(NewItem.Types.CSCodeOnly, Me.GetPackageOwner(Me.tvwMain.SelectedNode), Nothing, Nothing, Nothing)
                If frm.ShowDialog = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
                If frm.txtCodeFileName.Text.Length = 0 Then Exit Sub

                'Add code file
                Dim mdp As New MDPackage("", True)
                cs = mdp.NewCodeDOM_CS(frm.txtCodeFileName.Text, Me.GetPackageOwnerGUID(Me.tvwMain.SelectedNode))
            End If

            'Add code file
            Dim Node As New TreeNode(cs.Name, IMG_CS, IMG_CS)
            Node.Tag = cs

            Dim atIndex As Integer = 0
            Dim toIndex As Integer = Me.tvwMain.SelectedNode.Nodes.Count - 1

            'Add after properties/main nodes
            If TypeOf Me.tvwMain.SelectedNode.Tag Is MDPackageTag Then
                atIndex = 2
            End If

            'Don't insert within any folders
            For i As Integer = atIndex To toIndex
                If Not Me.tvwMain.SelectedNode.Nodes(i).Tag.ToString.Equals(TAG_FOLDER) Then
                    atIndex = i
                    Exit For
                End If
            Next

            'Add node to tree view
            Call Me.InsertAlphabetically(Me.tvwMain.SelectedNode, Node)

            RaiseEvent NewCodeDOM_CS()
        End Sub

        'Check if name already exists
        'Private Function CheckTemplateInNode(ByVal templateName As String, ByVal Nodes As TreeNodeCollection) As Boolean
        '    Dim yes = False
        '    For Each n As TreeNode In Nodes
        '        If n.Tag IsNot Nothing AndAlso TypeOf n.Tag Is Template Then
        '            If CType(n.Tag, Template).Name.Equals(templateName, StringComparison.CurrentCultureIgnoreCase) Then
        '                Return True
        '            End If
        '        End If
        '        If CheckTemplateInNode(templateName, n.Nodes) Then Return True
        '    Next
        '    Return yes
        'End Function


        Private Sub InsertAlphabetically(ByRef TargetNode As TreeNode, _
                                         ByVal NodeToInsert As TreeNode)
            'Just add for empties
            If TargetNode.Nodes.Count = 0 Then
                TargetNode.Nodes.Add(NodeToInsert)
            Else
                TargetNode.Nodes.Add(NodeToInsert)
                Me.tvwMain.BeginUpdate()
                Call Me.SortNodes(TargetNode)
                Me.tvwMain.EndUpdate()
            End If
        End Sub

        Private Class NodeSorter : Implements IComparer
            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
                Return String.Compare(CType(x, TreeNode).Text, CType(y, TreeNode).Text)
            End Function
        End Class

        Private Sub SortTreeNodes(ByRef tvw As TreeView)
            Dim list As New ArrayList()
            For Each n As TreeNode In tvw.Nodes
                list.Add(n)
            Next
            list.Sort(New NodeSorter())

            tvw.BeginUpdate()
            tvw.Nodes.Clear()
            For Each n As TreeNode In list
                tvw.Nodes.Add(n)
            Next
            tvw.EndUpdate()
        End Sub

        Private Function NodeClone(ByVal Node As TreeNode) As TreeNode
            Dim newNode As TreeNode = CType(Node.Clone, TreeNode)
            If Node.IsExpanded Then newNode.Expand()
            Return newNode
        End Function

        Private Function SortFolderNodes(ByRef Node As TreeNode, ByVal FolderTag As String) As TreeView
            'Sort folders under here
            Dim fldTree As New TreeView()
            For Each n As TreeNode In Node.Nodes
                If n.Tag.ToString.Equals(FolderTag) Then fldTree.Nodes.Add(Me.NodeClone(n))
            Next
            'If there's sub folders..
            Dim FolderHasSubFolders As Boolean = False
            For Each n As TreeNode In fldTree.Nodes
                If n.Nodes.Count > 0 Then
                    For Each nn As TreeNode In n.Nodes
                        If nn.Tag.ToString.Equals(FolderTag) Then
                            FolderHasSubFolders = True
                            Exit For
                        End If
                    Next
                    If FolderHasSubFolders Then Exit For
                End If
            Next
            If FolderHasSubFolders Then
                'Sort recursively
                For Each n As TreeNode In fldTree.Nodes
                    Call Me.SortNodes(n)
                Next
            Else
                'Sort this temp folder tree
                Call Me.SortTreeNodes(fldTree)
            End If
            Return fldTree
        End Function

        Private Sub SortNodes(ByRef Node As TreeNode)
            If Node Is Nothing Then Exit Sub
            If Node.Nodes.Count = 0 Then Exit Sub

            If Node.Tag.ToString.Equals(TAG_PROJECT) Or Node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                'Project items
                'Sort project folders
                Dim fldTree As TreeView = Me.SortFolderNodes(Node, TAG_PROJECT_FOLDER)

                'Get properties node
                Dim propNode As TreeNode = Nothing
                For Each n As TreeNode In Node.Nodes
                    If TypeOf n.Tag Is Persistence.Properties Then
                        propNode = Me.NodeClone(n)
                        Exit For
                    End If
                Next

                'Sort sources
                Dim dsTree As New TreeView()
                For Each n As TreeNode In Node.Nodes
                    If TypeOf n.Tag Is Source Then dsTree.Nodes.Add(Me.NodeClone(n))
                Next
                Call Me.SortTreeNodes(dsTree)

                'Sort packages
                Dim pkgTree As New TreeView()
                For Each n As TreeNode In Node.Nodes
                    If TypeOf n.Tag Is MDPackageTag Then pkgTree.Nodes.Add(Me.NodeClone(n))
                Next
                Call Me.SortTreeNodes(pkgTree)
                'Re-arrange package items
                For Each n As TreeNode In pkgTree.Nodes
                    Call Me.SortNodes(n)
                Next

                'Add properties, then folders, then sources, then packages
                Node.Nodes.Clear()
                If propNode IsNot Nothing Then Node.Nodes.Add(propNode)
                For Each n As TreeNode In fldTree.Nodes
                    Node.Nodes.Add(Me.NodeClone(n))
                Next
                For Each n As TreeNode In dsTree.Nodes
                    Node.Nodes.Add(Me.NodeClone(n))
                Next
                For Each n As TreeNode In pkgTree.Nodes
                    Node.Nodes.Add(Me.NodeClone(n))
                Next

            ElseIf TypeOf Node.Tag Is MDPackageTag Then
                'Package items
                'Find main node
                Dim mnNode As TreeNode = Nothing
                For Each n As TreeNode In Node.Nodes
                    If TypeOf n.Tag Is Main Then
                        mnNode = Me.NodeClone(n)
                    End If
                    If mnNode IsNot Nothing Then Exit For
                Next

                'Sort folders/templates under here
                Dim fldTree As TreeView = Me.SortFolderNodes(Node, TAG_FOLDER)

                'Sort templates and code files
                Dim tmpTree As New TreeView()
                For Each n As TreeNode In Node.Nodes
                    If TypeOf n.Tag Is Template Or
                       TypeOf n.Tag Is CodeDOM_VB Or
                       TypeOf n.Tag Is CodeDOM_CS Then
                        tmpTree.Nodes.Add(Me.NodeClone(n))
                    End If
                Next
                Call Me.SortTreeNodes(tmpTree)

                Node.Nodes.Clear()
                'Insert the main node back
                Node.Nodes.Add(Me.NodeClone(mnNode))
                'Add folders then templates
                For Each n As TreeNode In fldTree.Nodes
                    Node.Nodes.Add(Me.NodeClone(n))
                Next
                For Each n As TreeNode In tmpTree.Nodes
                    Node.Nodes.Add(Me.NodeClone(n))
                Next

            ElseIf Node.Tag.ToString.Equals(TAG_FOLDER) Then
                'Folders/templates under folder
                Dim fldTree As TreeView = Me.SortFolderNodes(Node, TAG_FOLDER)

                'Sort templates and code files
                Dim tmpTree As New TreeView()
                For Each n As TreeNode In Node.Nodes
                    If TypeOf n.Tag Is Template Or
                       TypeOf n.Tag Is CodeDOM_VB Or
                       TypeOf n.Tag Is CodeDOM_CS Then
                        tmpTree.Nodes.Add(Me.NodeClone(n))
                    End If
                Next
                Call Me.SortTreeNodes(tmpTree)

                'Add folders then templates
                Node.Nodes.Clear()
                For Each n As TreeNode In fldTree.Nodes
                    Node.Nodes.Add(Me.NodeClone(n))
                Next
                For Each n As TreeNode In tmpTree.Nodes
                    Node.Nodes.Add(Me.NodeClone(n))
                Next

            ElseIf TypeOf Node.Tag Is Persistence.Properties Then
                'Should not have any nodes under here

            ElseIf TypeOf Node.Tag Is Source Then
                'Should not have any nodes under here

            ElseIf TypeOf Node.Tag Is Template Then
                'Should not have any nodes under here

            ElseIf TypeOf Node.Tag Is CodeDOM_VB Then
                'Should not have any nodes under here

            ElseIf TypeOf Node.Tag Is CodeDOM_CS Then
                'Should not have any nodes under here

            End If

        End Sub

        Private Sub ArrangeAlphabetically()
            If Me.ChangedNode Is Nothing Then Exit Sub
            If Me.ChangedNode.Parent Is Nothing Then Exit Sub
            If Me.ChangedNode.Parent.Nodes.Count = 0 Then Exit Sub

            Me.tvwMain.SelectedNode = Me.ChangedNode

            Me.tvwMain.BeginUpdate()
            Call Me.SortNodes(Me.ChangedNode.Parent)
            Me.tvwMain.EndUpdate()

            Me.tvwMain.SelectedNode = Me.ChangedNode
            Me.ChangedNode = Nothing
        End Sub

    End Class

End Namespace