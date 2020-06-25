Imports Boston.Persistence

Namespace UI

    Friend Class Explorer

        Private ChangedNode As TreeNode = Nothing

        Private Const IMG_PROJECT As Integer = 0
        Private Const IMG_PROJECT_FOLDER_CLOSED As Integer = 1
        Private Const IMG_PROJECT_FOLDER_OPENED As Integer = 2
        Private Const IMG_PACKAGE As Integer = 3
        Private Const IMG_MAIN As Integer = 4
        Private Const IMG_TEMPLATE As Integer = 5
        Private Const IMG_VB As Integer = 6
        Private Const IMG_CS As Integer = 7
        Private Const IMG_FOLDER_CLOSED As Integer = 8
        Private Const IMG_FOLDER_OPENED As Integer = 9
        Private Const IMG_Source As Integer = 10
        Private Const IMG_PROPERTIES As Integer = 11

        Private Const IMG_DIMMED_PROJECT_FOLDER_CLOSED As Integer = 12
        Private Const IMG_DIMMED_PROJECT_FOLDER_OPENED As Integer = 13
        Private Const IMG_DIMMED_PACKAGE As Integer = 14
        Private Const IMG_DIMMED_MAIN As Integer = 15
        Private Const IMG_DIMMED_TEMPLATE As Integer = 16
        Private Const IMG_DIMMED_FOLDER_CLOSED As Integer = 17
        Private Const IMG_DIMMED_FOLDER_OPENED As Integer = 18
        Private Const IMG_DIMMED_Source As Integer = 19

        Friend Const TAG_PROJECT As String = "PROJECT"
        Friend Const TAG_FOLDER As String = "FOLDER"
        Friend Const TAG_PROJECT_FOLDER As String = "PROJECTFOLDER"

        Public Event AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs)
        Public Event NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs)
        Public Event NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs)
        Public Event AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs)
        Public Shadows Event MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Public Shadows Event GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Shadows Event LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Public Event OpenItem(ByVal Node As TreeNode)
        Public Event DeleteItem(ByVal Node As TreeNode)
        Public Event NewProject()
        Public Event NewPackageItem(ByVal name As String)
        Public Event NewSource()
        Public Event NewProjectFolder()
        Public Event NewFolder()
        Public Event NewTemplate(ByVal Template As Persistence.Template, ByVal mainNode As TreeNode)
        Public Event NewCodeDOM_VB()
        Public Event NewCodeDOM_CS()

        Private LoadedProfile As Profile

        Public ReadOnly Property Nodes() As TreeNodeCollection
            Get
                Return Me.tvwMain.Nodes
            End Get
        End Property

        Public Property SelectedNode() As TreeNode
            Get
                Return Me.tvwMain.SelectedNode
            End Get
            Set(ByVal value As TreeNode)
                Me.tvwMain.SelectedNode = value
            End Set
        End Property

        Public Property LabelEdit() As Boolean
            Get
                Return Me.tvwMain.LabelEdit
            End Get
            Set(ByVal value As Boolean)
                Me.tvwMain.LabelEdit = value
            End Set
        End Property

        Public Property TopNode() As TreeNode
            Get
                Return Me.tvwMain.TopNode
            End Get
            Set(ByVal value As TreeNode)
                Me.tvwMain.TopNode = value
            End Set
        End Property

        Private Sub tvwMain_AfterCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwMain.AfterCollapse
            If e.Node.Tag.ToString.Equals(TAG_FOLDER) Then
                Me.tvwMain.BeginUpdate()
                e.Node.ImageIndex = IMG_FOLDER_CLOSED
                e.Node.SelectedImageIndex = IMG_FOLDER_CLOSED
                Me.tvwMain.EndUpdate()
            ElseIf e.Node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                Me.tvwMain.BeginUpdate()
                e.Node.ImageIndex = IMG_PROJECT_FOLDER_CLOSED
                e.Node.SelectedImageIndex = IMG_PROJECT_FOLDER_CLOSED
                Me.tvwMain.EndUpdate()
            End If
        End Sub

        Private Sub tvwMain_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwMain.AfterExpand
            If e.Node.Tag.ToString.Equals(TAG_FOLDER) Then
                Me.tvwMain.BeginUpdate()
                e.Node.ImageIndex = IMG_FOLDER_OPENED
                e.Node.SelectedImageIndex = IMG_FOLDER_OPENED
                Me.tvwMain.EndUpdate()
            ElseIf e.Node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                Me.tvwMain.BeginUpdate()
                e.Node.ImageIndex = IMG_PROJECT_FOLDER_OPENED
                e.Node.SelectedImageIndex = IMG_PROJECT_FOLDER_OPENED
                Me.tvwMain.EndUpdate()
            End If
        End Sub

        Private Sub tvwMain_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwMain.AfterSelect
            Call Me.HandleNodeSelect(e.Node)
            RaiseEvent AfterSelect(sender, e)
        End Sub

        Private Sub tvwMain_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwMain.NodeMouseClick
            Call Me.HandleNodeSelect(e.Node)
            RaiseEvent NodeMouseClick(sender, e)
        End Sub

        Private Sub tvwMain_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwMain.NodeMouseDoubleClick
            RaiseEvent NodeMouseDoubleClick(sender, e)
        End Sub

        Private Sub tvwMain_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tvwMain.AfterLabelEdit
            If e.CancelEdit Then Exit Sub
            If e.Label Is Nothing Then Exit Sub

            Try
                If TypeOf e.Node.Tag Is MDPackage Then
                    If Not Persistence.MDProject.CheckValidName(e.Label) Then Throw New Exception("Invalid package name.")
                    Dim frm As New NewItem(NewItem.Types.Project, Nothing, Me.GetPackages(), Nothing, Nothing)
                    If frm.IsDuplicate_Package(e.Label) Then Throw New Exception("Package '" & e.Label & "' already defined.")
                    CType(e.Node.Tag, MDPackage).Name = e.Label

                ElseIf TypeOf e.Node.Tag Is Source Then
                    If Not Persistence.MDProject.CheckValidName(e.Label) Then Throw New Exception("Invalid source name.")
                    Dim frm As New NewItem(NewItem.Types.Project, Nothing, Me.GetPackages(), Nothing, Nothing)
                    If frm.IsDuplicate_Source(e.Label) Then Throw New Exception("Source '" & e.Label & "' already defined.")
                    CType(e.Node.Tag, Source).Name = e.Label

                ElseIf TypeOf e.Node.Tag Is Template Then
                    If Not Persistence.MDProject.CheckValidName(e.Label) Then Throw New Exception("Invalid template name.")
                    Dim frm As New NewItem(NewItem.Types.Project, Me.GetPackageOwner(Me.tvwMain.SelectedNode), Nothing, Nothing, Nothing)
                    If frm.IsDuplicate_Template(e.Label) Then Throw New Exception("Template '" & e.Label & "' already defined.")
                    CType(e.Node.Tag, Template).Name = e.Label

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "Rename", MessageBoxButtons.OK, MessageBoxIcon.Information)
                e.CancelEdit = True
                Exit Sub

            End Try

            e.Node.Text = e.Label
            e.Node.EndEdit(False)
            Me.ChangedNode = e.Node
            Me.tvwMain.BeginInvoke(New Sort(AddressOf Me.ArrangeAlphabetically))
            RaiseEvent AfterLabelEdit(sender, e)
        End Sub

        Private Sub mniOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniOpen.Click
            RaiseEvent OpenItem(Me.tvwMain.SelectedNode)
        End Sub

        Private Sub mniAddNewPackage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniAddNewPackage.Click
            Dim frm As New NewItem(NewItem.Types.PackageOnly, Nothing, Me.GetPackages, Me.GetSources, Nothing)
            If frm.ShowDialog = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
            If frm.txtPackageName.Text.Length = 0 Then Exit Sub

            Call Me.AddNewPackage(frm.txtPackageName.Text)
        End Sub

        Private Sub mniAddNewSource_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniAddNewSource.Click
            Dim frm As New NewItem(NewItem.Types.SourceOnly, Nothing, Me.GetPackages, Me.GetSources, Nothing)
            If frm.ShowDialog = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
            If frm.txtDBSourceName.Text.Length = 0 Then Exit Sub

            Call Me.AddNewSource(frm.txtDBSourceName.Text)
        End Sub

        Private Sub mniAddNewFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniAddNewFolder.Click
            Call Me.AddNewFolder()
        End Sub

        Private Sub mniAddNewProjectFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniAddNewProjectFolder.Click
            Call Me.AddNewProjectFolder()
        End Sub

        Private Sub mniAddNewTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniAddNewTemplate.Click
            Call Me.AddNewTemplate(Nothing)
        End Sub

        Private Sub mniAddNewVB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniAddNewVB.Click
            Call Me.AddNewCodeDOM_VB(Nothing)
        End Sub

        Private Sub mniAddNewCS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniAddNewCS.Click
            Call Me.AddNewCodeDOM_CS(Nothing)
        End Sub

        Private Sub mniDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniDelete.Click
            RaiseEvent DeleteItem(Me.tvwMain.SelectedNode)
        End Sub

        Private Sub mniRename_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniRename.Click
            Me.tvwMain.SelectedNode.BeginEdit()
        End Sub

        Private Sub tvwMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvwMain.MouseDown
            RaiseEvent MouseDown(sender, e)
        End Sub

        Private Sub tvwMain_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvwMain.GotFocus
            RaiseEvent GotFocus(sender, e)
        End Sub

        Private Sub tvwMain_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvwMain.LostFocus
            RaiseEvent LostFocus(sender, e)
        End Sub

        Private Sub tvwMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tvwMain.KeyDown
            If Me.tvwMain.SelectedNode Is Nothing Then Exit Sub

            If TypeOf Me.tvwMain.SelectedNode.Tag Is MDPackageTag Then
                If e.KeyCode = Keys.Delete Then RaiseEvent DeleteItem(Me.tvwMain.SelectedNode)

            ElseIf Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_FOLDER) Then
                If e.KeyCode = Keys.Delete Then RaiseEvent DeleteItem(Me.tvwMain.SelectedNode)

            ElseIf Me.tvwMain.SelectedNode.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                If e.KeyCode = Keys.Delete Then RaiseEvent DeleteItem(Me.tvwMain.SelectedNode)

            ElseIf TypeOf Me.tvwMain.SelectedNode.Tag Is Template Then
                If e.KeyCode = Keys.Delete Then RaiseEvent DeleteItem(Me.tvwMain.SelectedNode)

            ElseIf TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_VB Then
                If e.KeyCode = Keys.Delete Then RaiseEvent DeleteItem(Me.tvwMain.SelectedNode)

            ElseIf TypeOf Me.tvwMain.SelectedNode.Tag Is CodeDOM_CS Then
                If e.KeyCode = Keys.Delete Then RaiseEvent DeleteItem(Me.tvwMain.SelectedNode)

            ElseIf TypeOf Me.tvwMain.SelectedNode.Tag Is Source Then
                If e.KeyCode = Keys.Delete Then RaiseEvent DeleteItem(Me.tvwMain.SelectedNode)

            End If
        End Sub

        Private Sub mniCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mniCut.Click
            Call Me.PerformCut(Me.tvwMain.SelectedNode)
        End Sub

        Private Sub mniCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mniCopy.Click
            Call Me.PerformCopy(Me.tvwMain.SelectedNode)
        End Sub

        Private Sub mniPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mniPaste.Click
            Try
                Call Me.PerformPaste(Me.tvwMain.SelectedNode)

            Catch ex As Exception
                MessageBox.Show(ex.Message, "Paste Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            End Try
        End Sub

    End Class

End Namespace