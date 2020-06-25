Imports Boston.Parser.Syntax.Constants
Imports Boston.Persistence

Namespace UI

    Partial Friend Class MainForm

        Private SysChange As Boolean = False

        Friend projDirty As Boolean = False

        Friend ProjectPath As String = ""

        Friend ReadOnly Property IsDirty() As Boolean
            Get
                If Me.projDirty Then Return True

                If Me.tcMain.SelectedTab Is Nothing Then Return False
                If Me.tcMain.SelectedTab.Controls.Count = 0 Then Return False

                For Each tab As TabPage In Me.tcMain.TabPages
                    If tab.Controls.Count = 0 Then Continue For

                    If TypeOf tab.Controls(0) Is CodeEditor Then
                        Dim editor As CodeEditor = CType(tab.Controls(0), CodeEditor)

                        If TypeOf editor.Tag Is Template Then
                            If CType(editor.Tag, Template).GetDirty Then Return True

                        ElseIf TypeOf editor.Tag Is CodeDOM_VB Then
                            If CType(editor.Tag, CodeDOM_VB).GetDirty Then Return True

                        ElseIf TypeOf editor.Tag Is CodeDOM_CS Then
                            If CType(editor.Tag, CodeDOM_CS).GetDirty Then Return True

                        ElseIf TypeOf editor.Tag Is Main Then
                            If CType(editor.Tag, Main).GetDirty Then Return True

                        End If

                    ElseIf TypeOf tab.Controls(0) Is ManageProjectProperties Then
                        Dim editor As ManageProjectProperties = CType(tab.Controls(0), ManageProjectProperties)

                        If TypeOf editor.Tag Is Boston.Persistence.Properties Then
                            If CType(editor.Tag, Boston.Persistence.Properties).GetDirty Then Return True
                        End If

                    ElseIf TypeOf tab.Controls(0) Is ManageSource Then
                        Dim editor As ManageSource = CType(tab.Controls(0), ManageSource)

                        If TypeOf editor.Tag Is Source Then
                            If CType(editor.Tag, Source).GetDirty Then Return True
                        End If

                    End If
                Next

                Return False
            End Get
        End Property

        Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
            Call Me.HidePopup()

            If Me.tvwExplorer.AddNewItem(Nothing) Then
                Me.btnSave.Enabled = True
                Me.btnBuild.Enabled = True
            End If
        End Sub

        Private Sub tvwExplorer_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tvwExplorer.AfterLabelEdit
            Me.projDirty = True

            'Get node to match tab info
            Dim nodeGuid As String = ""
            If TypeOf e.Node.Tag Is MDPackageTag Then
                nodeGuid = CType(e.Node.Tag, MDPackageTag).GUID
            ElseIf TypeOf e.Node.Tag Is IEditorItem Then
                nodeGuid = CType(e.Node.Tag, IEditorItem).EditorGUID
            End If
            If nodeGuid.Length = 0 Then Exit Sub

            'Update open tab pages
            For Each tp As TabPage In Me.tcMain.TabPages
                'Look for it
                If tp.Tag Is Nothing Then Continue For
                If Not TypeOf tp.Tag Is TreeNode Then Continue For

                Dim tabGuid As String = ""
                Dim ownerGuid As String = ""
                If TypeOf CType(tp.Tag, TreeNode).Tag Is IEditorItem Then
                    tabGuid = CType(CType(tp.Tag, TreeNode).Tag, IEditorItem).EditorGUID
                    ownerGuid = CType(CType(tp.Tag, TreeNode).Tag, IEditorItem).OwnerGUID
                End If
                'Check against owner GUID if node changed was an owner (package node)
                If TypeOf e.Node.Tag Is MDPackageTag Then tabGuid = ownerGuid
                If Not tabGuid.Equals(nodeGuid) Then Continue For

                'Update tab tag and text
                If Not TypeOf e.Node.Tag Is MDPackageTag Then
                    tp.Tag = e.Node
                    tp.Text = CustTabControl.ShortenTextForTab(e.Node.Text)
                End If

                'Update code editor
                If tp.Controls.Count > 0 AndAlso TypeOf tp.Controls(0) Is CodeEditor Then
                    'Tag
                    If Not TypeOf e.Node.Tag Is MDPackageTag Then tp.Controls(0).Tag = e.Node.Tag

                    'Description
                    Dim packageText As String = e.Node.Text
                    If Not TypeOf e.Node.Tag Is MDPackageTag Then
                        packageText = Me.tvwExplorer.GetPackageOwnerNode(e.Node).Text
                    End If
                    CType(tp.Controls(0), CodeEditor).Description = packageText & "." & CType(tp.Tag, TreeNode).Text
                End If
            Next
        End Sub

        Private Sub tvwExplorer_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwExplorer.NodeMouseDoubleClick
            Call Me.OpenNode(e.Node)
        End Sub

        Private Sub tvwExplorer_OpenItem(ByVal Node As TreeNode) Handles tvwExplorer.OpenItem
            Call Me.OpenNode(Node)
        End Sub

        Private Sub tvwExplorer_NewProject() Handles tvwExplorer.NewProject
            Dim tps As New List(Of TabPage)
            Me.ClearTabs(tps)
            Me.lblStatus.Text = "(new)"
            Me.Refresh()
            Me.ProjectPath = ""
            If Me.tvwExplorer.tvwMain.Nodes.Count > 0 Then
                Me.projDirty = True
            End If
        End Sub

        Private Sub tvwExplorer_NewSource() Handles tvwExplorer.NewSource
            If Me.tvwExplorer.tvwMain.Nodes.Count > 0 Then
                Me.projDirty = True
            End If
        End Sub

        Private Sub tvwExplorer_NewTemplate(ByVal Template As Persistence.Template, ByVal mainNode As TreeNode) Handles tvwExplorer.NewTemplate
            'Add call statement to main, open first
            Call Me.OpenNode(mainNode)

            'add the text
            For Each tp As TabPage In Me.tcMain.TabPages
                If tp.Tag Is Nothing Then Continue For
                If Not TypeOf tp.Tag Is TreeNode Then Continue For
                If CType(tp.Tag, TreeNode).Tag.Equals(mainNode.Tag) Then
                    Dim sb As New System.Text.StringBuilder(CType(tp.Controls(0), CodeEditor).Text)
                    sb.AppendLine(System.Environment.NewLine & ACTION_CALL & " " & Template.Name)
                    CType(tp.Controls(0), CodeEditor).Text = sb.ToString
                    Exit For
                End If
            Next

            Me.projDirty = True
        End Sub

        Private Sub tvwExplorer_NewFolder() Handles tvwExplorer.NewFolder
            Me.projDirty = True
        End Sub

        Private Sub tvwExplorer_NewProjectFolder() Handles tvwExplorer.NewProjectFolder
            Me.projDirty = True
        End Sub

        Private Sub tvwExplorer_NewPackageItem(ByVal name As String) Handles tvwExplorer.NewPackageItem
            'Add call statement to super main, open first
            For Each n As TreeNode In Me.tvwExplorer.tvwMain.Nodes(0).Nodes
                If TypeOf n.Tag Is Boston.Persistence.Properties Then
                    Call Me.OpenNode(n)
                    Exit For
                End If
            Next

            'Update super main
            For Each tp As TabPage In Me.tcMain.TabPages
                If tp.Tag Is Nothing Then Continue For
                If Not TypeOf tp.Tag Is TreeNode Then Continue For
                If TypeOf CType(tp.Tag, TreeNode).Tag Is Boston.Persistence.Properties Then
                    Dim sb As New System.Text.StringBuilder(CType(tp.Controls(0), ManageProjectProperties).SuperMain.Text)
                    sb.AppendLine(System.Environment.NewLine & ACTION_CALL & " " & name)
                    CType(tp.Controls(0), ManageProjectProperties).SuperMain.Text = sb.ToString
                    Exit For
                End If
            Next

            Me.projDirty = True
        End Sub

        Private Sub tvwExplorer_NewCodeDOM_VB() Handles tvwExplorer.NewCodeDOM_VB
            Me.projDirty = True
        End Sub

        Private Sub tvwExplorer_NewCodeDOM_CS() Handles tvwExplorer.NewCodeDOM_CS
            Me.projDirty = True
        End Sub

        Private Sub tvwExplorer_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvwExplorer.MouseDown
            Call Me.HidePopup()
        End Sub

        Private Sub tvwExplorer_DeleteItem(ByVal Node As System.Windows.Forms.TreeNode) Handles tvwExplorer.DeleteItem
            If MessageBox.Show("'" & Node.Text & "' will be removed.", _
                             "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, _
                             MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Cancel Then Exit Sub

            'Remove tab pages first
            Call Me.RemTabPages(Node)

            'Now remove node
            Me.tvwExplorer.SelectedNode.Remove()

            Me.projDirty = True
        End Sub

        'Recursively remove tab pages according to treenode
        Private Sub RemTabPages(ByVal Node As System.Windows.Forms.TreeNode)
            If TypeOf Node.Tag Is MDPackageTag Then
                'Remove tab pages opened under this package
                Dim guid As String = CType(Node.Tag, MDPackageTag).GUID
                Dim i As Integer = -1
                While i < Me.tcMain.TabPages.Count - 1
                    i += 1
                    If Me.tcMain.TabPages(i).Tag Is Nothing Then Continue While
                    If TypeOf CType(Me.tcMain.TabPages(i).Tag, TreeNode).Tag Is IEditorItem Then
                        If CType(CType(Me.tcMain.TabPages(i).Tag, TreeNode).Tag, IEditorItem).OwnerGUID Is Nothing Then
                            Continue While
                        End If
                        If CType(CType(Me.tcMain.TabPages(i).Tag, TreeNode).Tag, IEditorItem).OwnerGUID.Equals(guid) Then
                            Me.tcMain.TabPages.RemoveAt(i)
                            i -= 1
                        End If
                    End If
                End While

                'Recursively just in case
                For Each n As TreeNode In Node.Nodes
                    Me.RemTabPages(n)
                Next

            ElseIf TypeOf Node.Tag Is String AndAlso Node.Tag.ToString.Equals(Explorer.TAG_FOLDER) Then
                For Each n As TreeNode In Node.Nodes
                    Me.RemTabPages(n)
                Next

            Else
                For Each tp As TabPage In Me.tcMain.TabPages
                    If tp.Tag IsNot Nothing AndAlso tp.Tag.Equals(Node) Then
                        Me.tcMain.TabPages.Remove(tp)
                        Exit For
                    End If
                Next

                For Each n As TreeNode In Node.Nodes
                    Me.RemTabPages(n)
                Next

            End If
        End Sub

        Private Sub OpenNode(ByVal Node As TreeNode)
            If Node Is Nothing Then Exit Sub

            Call Me.HidePopup()

            'Select tab page if already open
            For Each tp As TabPage In Me.tcMain.TabPages
                If tp.Tag Is Nothing Then Continue For
                If Not TypeOf tp.Tag Is TreeNode Then Continue For

                If TypeOf CType(tp.Tag, TreeNode).Tag Is Main And TypeOf Node.Tag Is Main Then
                    If Not CType(CType(tp.Tag, TreeNode).Tag, Main).EditorGUID.Equals(CType(Node.Tag, Main).EditorGUID) Then
                        Continue For
                    End If

                ElseIf TypeOf CType(tp.Tag, TreeNode).Tag Is IEditorItem And TypeOf Node.Tag Is IEditorItem Then
                    If Not CType(CType(tp.Tag, TreeNode).Tag, IEditorItem).EditorGUID.Equals(CType(Node.Tag, IEditorItem).EditorGUID) Then
                        Continue For
                    End If

                Else
                    Continue For

                End If

                Me.tcMain.SelectedTab = tp
                If Me.tcMain.SelectedTab.Controls.Count > 0 AndAlso _
                   TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then
                    CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).FocusText()
                End If
                Exit Sub
            Next

            Me.SysChange = True

            'Add to tab control
            If TypeOf Node.Tag Is Main Then
                'Make a copy of the node's tag
                Dim Copy As Main = CType(CType(Node.Tag, Main).GetCopy(), Persistence.Main)

                'Set up text editor
                Dim txt As New CodeEditor(txtBox.HighlightModes.MetadroneMain)
                txt.Dock = DockStyle.Fill
                txt.DontHilight = True
                txt.Enabled = True
                txt.Text = Copy.Text
                txt.Tag = Copy
                txt.Description = Me.tvwExplorer.GetPackageOwnerNode(Node).Text & "." & Node.Text
                txt.btnPreview.Visible = False
                AddHandler txt.SearchRequested, AddressOf Me.RequestSearch
                AddHandler txt.SearchReset, AddressOf Me.SearchReset
                AddHandler txt.Save, AddressOf Me.SaveOpenTab
                AddHandler txt.Run, AddressOf Me.RunBuild
                AddHandler txt.TextChanged, AddressOf Me.EditTextChanged

                'Set up tab page and add
                Dim tp As New TabPage(CustTabControl.ShortenTextForTab(Node.Text))
                tp.Controls.Add(txt)
                tp.ImageIndex = IMG_MAIN
                tp.Tag = Node
                Me.tcMain.TabPages.Add(tp)
                Me.tcMain.SelectedTab = tp
                CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).FocusText()

            ElseIf TypeOf Node.Tag Is Template Then
                'Make a copy of the node's tag
                Dim Copy As Template = CType(CType(Node.Tag, Template).GetCopy(), Persistence.Template)

                'Set up text editor
                Dim txt As New CodeEditor(txtBox.HighlightModes.MetadroneTemplate)
                txt.Dock = DockStyle.Fill
                txt.DontHilight = False
                txt.Enabled = True
                txt.Text = Copy.Text
                txt.Tag = Copy
                txt.Description = Me.tvwExplorer.GetPackageOwnerNode(Node).Text & "." & Node.Text
                txt.btnPreview.Visible = True
                AddHandler txt.SearchRequested, AddressOf Me.RequestSearch
                AddHandler txt.SearchReset, AddressOf Me.SearchReset
                AddHandler txt.Save, AddressOf Me.SaveOpenTab
                AddHandler txt.Run, AddressOf Me.RunBuild
                AddHandler txt.TextChanged, AddressOf Me.EditTextChanged

                'Set up tab page and add
                Dim tp As New TabPage(CustTabControl.ShortenTextForTab(Node.Text))
                tp.Controls.Add(txt)
                tp.ImageIndex = IMG_TEMPLATE
                tp.Tag = Node
                Me.tcMain.TabPages.Add(tp)
                Me.tcMain.SelectedTab = tp
                CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).FocusText()

            ElseIf TypeOf Node.Tag Is CodeDOM_VB Then
                'Make a copy of the node's tag
                Dim Copy As CodeDOM_VB = CType(CType(Node.Tag, CodeDOM_VB).GetCopy(), Persistence.CodeDOM_VB)

                'Set up text editor
                Dim txt As New CodeEditor(txtBox.HighlightModes.VB)
                txt.Dock = DockStyle.Fill
                txt.DontHilight = False
                txt.Enabled = True
                txt.Text = Copy.Text
                txt.Tag = Copy
                txt.Description = Me.tvwExplorer.GetPackageOwnerNode(Node).Text & "." & Node.Text
                txt.btnPreview.Visible = False
                AddHandler txt.SearchRequested, AddressOf Me.RequestSearch
                AddHandler txt.SearchReset, AddressOf Me.SearchReset
                AddHandler txt.Save, AddressOf Me.SaveOpenTab
                AddHandler txt.Run, AddressOf Me.RunBuild
                AddHandler txt.TextChanged, AddressOf Me.EditTextChanged

                'Set up tab page and add
                Dim tp As New TabPage(CustTabControl.ShortenTextForTab(Node.Text))
                tp.Controls.Add(txt)
                tp.ImageIndex = IMG_VB
                tp.Tag = Node
                Me.tcMain.TabPages.Add(tp)
                Me.tcMain.SelectedTab = tp
                CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).FocusText()

            ElseIf TypeOf Node.Tag Is CodeDOM_CS Then
                'Make a copy of the node's tag
                Dim Copy As CodeDOM_CS = CType(CType(Node.Tag, CodeDOM_CS).GetCopy(), Persistence.CodeDOM_CS)

                'Set up text editor
                Dim txt As New CodeEditor(txtBox.HighlightModes.CS)
                txt.Dock = DockStyle.Fill
                txt.DontHilight = False
                txt.Enabled = True
                txt.Text = Copy.Text
                txt.Tag = Copy
                txt.Description = Me.tvwExplorer.GetPackageOwnerNode(Node).Text & "." & Node.Text
                txt.btnPreview.Visible = False
                AddHandler txt.SearchRequested, AddressOf Me.RequestSearch
                AddHandler txt.SearchReset, AddressOf Me.SearchReset
                AddHandler txt.Save, AddressOf Me.SaveOpenTab
                AddHandler txt.Run, AddressOf Me.RunBuild
                AddHandler txt.TextChanged, AddressOf Me.EditTextChanged

                'Set up tab page and add
                Dim tp As New TabPage(CustTabControl.ShortenTextForTab(Node.Text))
                tp.Controls.Add(txt)
                tp.ImageIndex = IMG_CS
                tp.Tag = Node
                Me.tcMain.TabPages.Add(tp)
                Me.tcMain.SelectedTab = tp
                CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).FocusText()

            ElseIf TypeOf Node.Tag Is Boston.Persistence.Properties Then
                'Make a copy of the node's tag
                Dim Copy As Boston.Persistence.Properties = CType(CType(Node.Tag, Boston.Persistence.Properties).GetCopy(), Persistence.Properties)

                'Set up properties control
                Dim man As New ManageProjectProperties()
                man.Dock = DockStyle.Fill
                man.Tag = Copy
                AddHandler man.ValueChanged, AddressOf Me.PropertiesValueChanged
                AddHandler man.SavePress, AddressOf Me.SaveOpenTab
                AddHandler man.Run, AddressOf Me.RunBuild

                'Set up tab page and add
                Dim tp As New TabPage(CustTabControl.ShortenTextForTab(Node.Text))
                tp.Controls.Add(man)
                tp.ImageIndex = IMG_PROPERTIES
                tp.Tag = Node
                Me.tcMain.TabPages.Add(tp)
                Me.tcMain.SelectedTab = tp

            ElseIf TypeOf Node.Tag Is Source Then
                'Make a copy of the node's tag
                Dim Copy As Source = CType(CType(Node.Tag, Source).GetCopy(), Persistence.Source)

                'Set up Source editor
                Dim man As New ManageSource()
                man.Dock = DockStyle.Fill
                man.Tag = Copy
                man.Initialise(Copy)
                AddHandler man.ValueChanged, AddressOf Me.SourceValueChanged
                AddHandler man.Save, AddressOf Me.SaveOpenTab

                'Set up tab page and add
                Dim tp As New TabPage(CustTabControl.ShortenTextForTab(Node.Text))
                tp.Controls.Add(man)
                tp.ImageIndex = IMG_SOURCE
                tp.Tag = Node
                Me.tcMain.TabPages.Add(tp)
                Me.tcMain.SelectedTab = tp

            End If

            Me.SysChange = False
        End Sub

        'Get node that has it's tag matching the guid
        Private Function LookForNode(ByVal Node As TreeNode, ByVal GUID As String) As TreeNode
            If TypeOf Node.Tag Is IEditorItem Then
                If CType(Node.Tag, IEditorItem).EditorGUID.Equals(GUID) Then Return Node
            End If
            For Each n As TreeNode In Node.Nodes
                Dim nd As TreeNode = Me.LookForNode(n, GUID)
                If nd IsNot Nothing Then Return nd
            Next

            Return Nothing
        End Function

        Private Sub EditTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            If Me.SysChange Then Exit Sub

            If Me.tcMain.SelectedTab.Controls.Count = 0 Then Exit Sub
            If Not TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then Exit Sub
            Me.textAlteredSoSearchAgain = True

            Dim editor As CodeEditor = CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor)
            Try
                If Not editor.GetSender.Equals(sender) Then Exit Sub
            Catch ex As Exception
                Exit Sub
            End Try

            If TypeOf editor.Tag Is Template Then
                CType(editor.Tag, Template).Text = editor.Text
                If Not CType(editor.Tag, Template).GetDirty Then Me.tcMain.SelectedTab.Text &= "*"
                CType(editor.Tag, Template).SetDirty(True)

            ElseIf TypeOf editor.Tag Is CodeDOM_VB Then
                CType(editor.Tag, CodeDOM_VB).Text = editor.Text
                If Not CType(editor.Tag, CodeDOM_VB).GetDirty Then Me.tcMain.SelectedTab.Text &= "*"
                CType(editor.Tag, CodeDOM_VB).SetDirty(True)

            ElseIf TypeOf editor.Tag Is CodeDOM_CS Then
                CType(editor.Tag, CodeDOM_CS).Text = editor.Text
                If Not CType(editor.Tag, CodeDOM_CS).GetDirty Then Me.tcMain.SelectedTab.Text &= "*"
                CType(editor.Tag, CodeDOM_CS).SetDirty(True)

            ElseIf TypeOf editor.Tag Is Main Then
                CType(editor.Tag, Main).Text = editor.Text
                If Not CType(editor.Tag, Main).GetDirty Then Me.tcMain.SelectedTab.Text &= "*"
                CType(editor.Tag, Main).SetDirty(True)

            End If
        End Sub

        Private Sub PropertiesValueChanged(ByVal value As Object)
            If Me.SysChange Then Exit Sub

            If Me.tcMain.SelectedTab.Controls.Count = 0 Then Exit Sub
            If Not TypeOf Me.tcMain.SelectedTab.Controls(0) Is ManageProjectProperties Then Exit Sub

            Dim man As ManageProjectProperties = CType(Me.tcMain.SelectedTab.Controls(0), ManageProjectProperties)
            man.UpdateTag()
            If Not CType(man.Tag, Boston.Persistence.Properties).GetDirty Then Me.tcMain.SelectedTab.Text &= "*"
            CType(man.Tag, Boston.Persistence.Properties).SetDirty(True)
        End Sub

        Private Sub SourceValueChanged(ByVal value As Object)
            If Me.SysChange Then Exit Sub

            If Me.tcMain.SelectedTab.Controls.Count = 0 Then Exit Sub
            If Not TypeOf Me.tcMain.SelectedTab.Controls(0) Is ManageSource Then Exit Sub

            Dim man As ManageSource = CType(Me.tcMain.SelectedTab.Controls(0), ManageSource)
            man.UpdateTag()
            If Not CType(man.Tag, Source).GetDirty Then Me.tcMain.SelectedTab.Text &= "*"
            CType(man.Tag, Persistence.Source).SetDirty(True)
        End Sub

        Private Sub SaveOpenTab()
            If Me.tcMain.SelectedTab.Controls.Count = 0 Then Exit Sub

            If Me.WillSaveProject Then
                Me.Cursor = Cursors.WaitCursor
                Call Me.CopyTabDataBackIntoNode(Me.tcMain.SelectedTab)
                Call Me.SaveProject()
                Me.Cursor = Cursors.Default
            End If
        End Sub

        Friend Sub CopyTabDataBackIntoNode(ByRef Tab As TabPage)
            If Tab.Controls.Count = 0 Then Exit Sub
            If TypeOf Tab.Controls(0) Is CodeEditor Then
                Dim editor As CodeEditor = CType(Tab.Controls(0), CodeEditor)
                Dim node As TreeNode = CType(Tab.Tag, TreeNode)
                If TypeOf node.Tag Is Template Then
                    CType(node.Tag, Template).Text = CType(editor.Tag, Template).Text
                    CType(editor.Tag, Template).SetDirty(False)

                    If Tab.Text.LastIndexOf("*").Equals(Tab.Text.Length - 1) Then
                        Tab.Text = Tab.Text.Substring(0, Tab.Text.Length - 1)
                    End If

                ElseIf TypeOf node.Tag Is CodeDOM_VB Then
                    CType(node.Tag, CodeDOM_VB).Text = CType(editor.Tag, CodeDOM_VB).Text
                    CType(editor.Tag, CodeDOM_VB).SetDirty(False)
                    If Tab.Text.LastIndexOf("*").Equals(Tab.Text.Length - 1) Then
                        Tab.Text = Tab.Text.Substring(0, Tab.Text.Length - 1)
                    End If

                ElseIf TypeOf node.Tag Is CodeDOM_CS Then
                    CType(node.Tag, CodeDOM_CS).Text = CType(editor.Tag, CodeDOM_CS).Text
                    CType(editor.Tag, CodeDOM_CS).SetDirty(False)
                    If Tab.Text.LastIndexOf("*").Equals(Tab.Text.Length - 1) Then
                        Tab.Text = Tab.Text.Substring(0, Tab.Text.Length - 1)
                    End If

                ElseIf TypeOf node.Tag Is Main Then
                    CType(node.Tag, Main).Text = CType(editor.Tag, Main).Text
                    CType(editor.Tag, Main).SetDirty(False)
                    If Tab.Text.LastIndexOf("*").Equals(Tab.Text.Length - 1) Then
                        Tab.Text = Tab.Text.Substring(0, Tab.Text.Length - 1)
                    End If

                End If

            ElseIf TypeOf Tab.Controls(0) Is ManageProjectProperties Then
                Dim man As ManageProjectProperties = CType(Tab.Controls(0), ManageProjectProperties)
                Dim node As TreeNode = CType(Tab.Tag, TreeNode)

                If TypeOf node.Tag Is Boston.Persistence.Properties Then
                    node.Tag = CType(man.Tag, Boston.Persistence.Properties).GetCopy
                    CType(man.Tag, Boston.Persistence.Properties).SetDirty(False)

                    If Tab.Text.LastIndexOf("*").Equals(Tab.Text.Length - 1) Then
                        Tab.Text = Tab.Text.Substring(0, Tab.Text.Length - 1)
                    End If
                End If

            ElseIf TypeOf Tab.Controls(0) Is ManageSource Then
                Dim man As ManageSource = CType(Tab.Controls(0), ManageSource)
                Dim node As TreeNode = CType(Tab.Tag, TreeNode)

                If TypeOf node.Tag Is Source Then
                    node.Tag = CType(man.Tag, Source).GetCopy
                    CType(man.Tag, Source).SetDirty(False)

                    If Tab.Text.LastIndexOf("*").Equals(Tab.Text.Length - 1) Then
                        Tab.Text = Tab.Text.Substring(0, Tab.Text.Length - 1)
                    End If
                End If

            End If
        End Sub

        Friend Function WillSaveProject() As Boolean
            'New project save, open save as dialog
            If Me.ProjectPath.Length = 0 Then
                Dim dlg As New System.Windows.Forms.SaveFileDialog()
                dlg.Filter = "Metadrone Project Files (*.mdrone)|*.mdrone|All files (*.*)|*.*"
                dlg.InitialDirectory = Me.settings.LastWorkLocation
                If dlg.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    Me.ProjectPath = dlg.FileName
                    Me.settings.LastWorkLocation = IO.Path.GetDirectoryName(dlg.FileName)
                Else
                    Return False
                End If
            End If

            Return True
        End Function

        Friend Sub OpenProject()
            Call Me.HidePopup()

            If Me.IsDirty Then If Me.SaveChanges() = System.Windows.Forms.DialogResult.Cancel Then Exit Sub

            Try
                Dim dlg As New System.Windows.Forms.OpenFileDialog()
                dlg.Filter = "Metadrone Project Files (*.mdrone)|*.mdrone|All files (*.*)|*.*"
                dlg.Title = "Open Metadrone Project File"
                dlg.InitialDirectory = Me.settings.LastWorkLocation
                If dlg.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                    Me.Cursor = Cursors.WaitCursor
                    Me.lblStatus.Text = "Opening..."
                    Me.Refresh()
                    Call Me.OpenProject(dlg.FileName)
                    Me.lblStatus.Text = dlg.FileName
                    Me.Refresh()
                    Me.ProjectPath = dlg.FileName
                    Me.btnSave.Enabled = True

                    'Update settings
                    Me.settings.LastWorkLocation = IO.Path.GetDirectoryName(dlg.FileName)
                    Me.settings.AddProject(New Persistence.Settings.Recent(Me.tvwExplorer.Transform(Me.tvwExplorer.tvwMain.Nodes(0)).Name, Me.ProjectPath))
                    Call Me.settings.Save()
                    'and reload in start page
                    Call CType(Me.tcMain.TabPages(0).Controls(0), StartPage).LoadRecents(Me.settings)

                    Me.projDirty = False

                    Me.Cursor = Cursors.Default
                End If
            Catch ex As System.Exception
                Me.lblStatus.Text = "Open failed"
                Me.Refresh()
                Me.Cursor = Cursors.Default
                System.Windows.Forms.MessageBox.Show(ex.Message, "Open Error")
            End Try
        End Sub

        Private Sub OpenProject(ByVal FileName As String)
            Call Me.tvwExplorer.LoadProject(FileName)

            Me.txtResult.Text = ""
            Me.grdOutput.Rows.Clear()

            Dim tps As New List(Of TabPage)
            Me.ClearTabs(tps)

            Me.ProjectPath = FileName
            Me.btnBuild.Enabled = True

            'Open tabs from profile
            For Each Guid In Me.tvwExplorer.GetLoadedProfile().OpenEditorGUIDs
                Dim n As TreeNode = Me.LookForNode(Me.tvwExplorer.Nodes(0), Guid)
                If n IsNot Nothing Then Call Me.OpenNode(n)
            Next

            'Select tab
            If Me.tvwExplorer.GetLoadedProfile.SelectedEditorGUID Is Nothing Then Exit Sub

            For Each tp As TabPage In Me.tcMain.TabPages
                If TypeOf tp.Tag Is TreeNode Then
                    If TypeOf CType(tp.Tag, TreeNode).Tag Is IEditorItem Then
                        If CType(CType(tp.Tag, TreeNode).Tag, IEditorItem).EditorGUID.Equals(Me.tvwExplorer.GetLoadedProfile.SelectedEditorGUID) Then
                            Me.tcMain.SelectedTab = tp
                            Exit Sub
                        End If
                    End If
                End If
            Next
        End Sub

        Friend Function SaveProject() As Boolean
            Me.Cursor = Cursors.WaitCursor
            Call Me.tvwExplorer.SetLoadedProfile(Me.tcMain)
            Call Me.tvwExplorer.SaveProject(Me.ProjectPath)
            Me.Cursor = Cursors.Default

            'Refresh
            Me.lblStatus.Text = Me.ProjectPath
            Me.Refresh()
            Me.projDirty = False
            Me.btnSave.Enabled = True

            'Update settings
            Me.settings.AddProject(New Persistence.Settings.Recent(Me.tvwExplorer.Transform(Me.tvwExplorer.tvwMain.Nodes(0)).Name, Me.ProjectPath))
            Call Me.settings.Save()
            'and reload in start page
            Call CType(Me.tcMain.TabPages(0).Controls(0), StartPage).LoadRecents(Me.settings)

            Return True
        End Function

        Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpen.Click
            Call Me.OpenProject()
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Call Me.HidePopup()

            Try
                If Me.WillSaveProject Then
                    Me.Cursor = Cursors.WaitCursor
                    For i As Integer = 0 To Me.tcMain.TabPages.Count - 1
                        Call Me.CopyTabDataBackIntoNode(Me.tcMain.TabPages(i))
                    Next
                    Call Me.SaveProject()
                    Me.Cursor = Cursors.Default
                End If
            Catch ex As System.Exception
                Me.Cursor = Cursors.Default
                System.Windows.Forms.MessageBox.Show(ex.Message, "Save Error")
            End Try
        End Sub

        Private Function SaveChanges(Optional ByVal TabPage As TabPage = Nothing) As System.Windows.Forms.DialogResult
            If Me.tvwExplorer.Nodes.Count = 0 Then Return System.Windows.Forms.DialogResult.OK

            Dim f As New SaveChanges()
            If TabPage IsNot Nothing Then
                'Check save changes for tab page
                If f.FillDirty(Me.tvwExplorer.Nodes(0), TabPage) Then
                    f.ShowDialog(Me)
                End If
            Else
                'Check save changes for all items
                If f.FillDirty(Me.tvwExplorer.Nodes(0), Me.tcMain, Me.projDirty) Then
                    f.ShowDialog(Me)
                End If
            End If

            If f.DialogResult = System.Windows.Forms.DialogResult.Yes Then
                If Me.WillSaveProject Then
                    Me.Cursor = Cursors.WaitCursor
                    If TabPage IsNot Nothing Then
                        'Copy tab data back
                        Call Me.CopyTabDataBackIntoNode(TabPage)
                    Else
                        'Copy all tab data back
                        For Each tp As TabPage In Me.tcMain.TabPages
                            Call Me.CopyTabDataBackIntoNode(tp)
                        Next
                    End If

                    'Save
                    Call Me.SaveProject()
                    Me.Cursor = Cursors.Default

                Else
                    Return System.Windows.Forms.DialogResult.Cancel

                End If
            End If

            Return f.DialogResult
        End Function

    End Class

End Namespace