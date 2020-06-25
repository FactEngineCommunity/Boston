Imports Boston.Persistence

Namespace UI

    Partial Friend Class Explorer

        Private ClipBoard As TreeNode = Nothing
        Private CutMode As Boolean = False

        Private Sub ChangeIcons(ByVal node As TreeNode, ByVal dimmed As Boolean)
            If node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                If node.SelectedImageIndex = node.ImageIndex Then
                    If dimmed Then
                        node.ImageIndex = IMG_DIMMED_PROJECT_FOLDER_CLOSED
                        node.SelectedImageIndex = IMG_DIMMED_PROJECT_FOLDER_CLOSED
                    Else
                        node.ImageIndex = IMG_PROJECT_FOLDER_CLOSED
                        node.SelectedImageIndex = IMG_PROJECT_FOLDER_CLOSED
                    End If
                Else
                    If dimmed Then
                        node.ImageIndex = IMG_DIMMED_PROJECT_FOLDER_CLOSED
                        node.SelectedImageIndex = IMG_DIMMED_PROJECT_FOLDER_OPENED
                    Else
                        node.ImageIndex = IMG_PROJECT_FOLDER_CLOSED
                        node.SelectedImageIndex = IMG_PROJECT_FOLDER_OPENED
                    End If
                End If

            ElseIf TypeOf node.Tag Is MDPackageTag Then
                If dimmed Then
                    node.ImageIndex = IMG_DIMMED_PACKAGE
                    node.SelectedImageIndex = IMG_DIMMED_PACKAGE
                Else
                    node.ImageIndex = IMG_PACKAGE
                    node.SelectedImageIndex = IMG_PACKAGE
                End If

            ElseIf TypeOf node.Tag Is Main Then
                If dimmed Then
                    node.ImageIndex = IMG_DIMMED_MAIN
                    node.SelectedImageIndex = IMG_DIMMED_MAIN
                Else
                    node.ImageIndex = IMG_MAIN
                    node.SelectedImageIndex = IMG_MAIN
                End If

            ElseIf TypeOf node.Tag Is Source Then
                If dimmed Then
                    node.ImageIndex = IMG_DIMMED_Source
                    node.SelectedImageIndex = IMG_DIMMED_Source
                Else
                    node.ImageIndex = IMG_Source
                    node.SelectedImageIndex = IMG_Source
                End If

            ElseIf node.Tag.ToString.Equals(TAG_FOLDER) Then
                If node.SelectedImageIndex = node.ImageIndex Then
                    If dimmed Then
                        node.ImageIndex = IMG_DIMMED_FOLDER_CLOSED
                        node.SelectedImageIndex = IMG_DIMMED_FOLDER_CLOSED
                    Else
                        node.ImageIndex = IMG_FOLDER_CLOSED
                        node.SelectedImageIndex = IMG_FOLDER_CLOSED
                    End If

                Else
                    If dimmed Then
                        node.ImageIndex = IMG_DIMMED_FOLDER_CLOSED
                        node.SelectedImageIndex = IMG_DIMMED_FOLDER_OPENED
                    Else
                        node.ImageIndex = IMG_FOLDER_CLOSED
                        node.SelectedImageIndex = IMG_FOLDER_OPENED
                    End If

                End If

            ElseIf TypeOf node.Tag Is Template Then
                If dimmed Then
                    node.ImageIndex = IMG_DIMMED_TEMPLATE
                    node.SelectedImageIndex = IMG_DIMMED_TEMPLATE
                Else
                    node.ImageIndex = IMG_TEMPLATE
                    node.SelectedImageIndex = IMG_TEMPLATE
                End If

            End If

            For Each n As TreeNode In node.Nodes
                Call Me.ChangeIcons(n, dimmed)
            Next
        End Sub

        Private Sub PerformCut(ByVal TargetNode As TreeNode)
            Me.ClipBoard = Me.tvwMain.SelectedNode

            Call Me.ChangeIcons(Me.tvwMain.SelectedNode, True)

            Me.CutMode = True
        End Sub

        Private Sub PerformCopy(ByVal TargetNode As TreeNode)
            Me.ClipBoard = Me.tvwMain.SelectedNode
            'TODO set copy graphic for selected node
            Me.CutMode = False
        End Sub

        Private Sub PerformPaste(ByVal TargetNode As TreeNode)
            If Me.ClipBoard Is Nothing Then Exit Sub

            If TypeOf TargetNode.Tag Is Boston.Persistence.Properties Then Exit Sub
            If TypeOf TargetNode.Tag Is Main Then Exit Sub
            If TypeOf TargetNode.Tag Is Template Then Exit Sub
            If TypeOf TargetNode.Tag Is Source Then Exit Sub

            If TypeOf Me.ClipBoard.Tag Is Boston.Persistence.Properties Then Exit Sub
            If TypeOf Me.ClipBoard.Tag Is Main Then Exit Sub

            If TargetNode.Tag.ToString.Equals(TAG_PROJECT) Then
                If TypeOf Me.ClipBoard.Tag Is Template Then Exit Sub
                If Me.ClipBoard.Tag.ToString.Equals(TAG_FOLDER) Then Exit Sub

            ElseIf TargetNode.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                If TypeOf Me.ClipBoard.Tag Is Template Then Exit Sub
                If Me.ClipBoard.Tag.ToString.Equals(TAG_FOLDER) Then Exit Sub

            ElseIf TypeOf TargetNode.Tag Is MDPackageTag Then
                If Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then Exit Sub
                If TypeOf Me.ClipBoard.Tag Is MDPackageTag Then Exit Sub
                If TypeOf Me.ClipBoard.Tag Is Source Then Exit Sub

            ElseIf TargetNode.Tag.ToString.Equals(TAG_FOLDER) Then
                If Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then Exit Sub
                If TypeOf Me.ClipBoard.Tag Is MDPackageTag Then Exit Sub
                If TypeOf Me.ClipBoard.Tag Is Source Then Exit Sub

            End If

            Call Me.ChangeIcons(Me.ClipBoard, False)

            'Remove source if cut
            If Me.CutMode Then Me.ClipBoard.Remove()

            'Now paste
            Call Me.InsertAlphabetically(TargetNode, Me.GetPasteCopy(Me.ClipBoard, TargetNode))

            'Clear clipboard, disable further pasting
            Me.ClipBoard.Remove()
            Me.ClipBoard = Nothing
        End Sub

        Private Function GetPasteCopy(ByVal sourceNode As TreeNode, ByVal targetNode As TreeNode) As TreeNode
            Dim OwnerGUID As String = Me.GetPackageOwnerGUID(targetNode)
            Return Me.GetNodeCopy(sourceNode, OwnerGUID)
        End Function

        Private Function GetNodeCopy(ByVal sourceNode As TreeNode, ByVal OwnerGUID As String, Optional ByVal dontRenameTemplate As Boolean = False) As TreeNode
            Dim node As New TreeNode(sourceNode.Text, sourceNode.ImageIndex, sourceNode.SelectedImageIndex)

            If sourceNode.Tag.ToString.Equals(TAG_PROJECT) Then
                Throw New Exception("Project nodes cannot be copied.")

            ElseIf sourceNode.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                node.Tag = TAG_PROJECT_FOLDER

            ElseIf TypeOf sourceNode.Tag Is MDPackageTag Then
                node.Tag = CType(sourceNode.Tag, IMDPersistenceItem).GetCopy
                If Not Me.CutMode Then
                    CType(node.Tag, MDPackageTag).GUID = System.Guid.NewGuid.ToString()

                    Dim frm As New NewItem(NewItem.Types.PackageOnly, Nothing, Me.GetPackages(), Nothing, Nothing)
                    Dim renameOk As Boolean = False
                    Dim newName As String = node.Text
                    Dim stopInfiniteLoop As Integer = 0
                    While Not renameOk And stopInfiniteLoop < 100
                        stopInfiniteLoop += 1
                        newName = "Copy_of_" & newName
                        renameOk = Not frm.IsDuplicate_Package(newName)
                    End While
                    If stopInfiniteLoop >= 100 Then Throw New Exception("Could not paste.")
                    node.Text = newName
                End If
                OwnerGUID = CType(node.Tag, MDPackageTag).GUID 'Sub-nodes need to use this GUID

            ElseIf TypeOf sourceNode.Tag Is Source Then
                node.Tag = CType(sourceNode.Tag, IMDPersistenceItem).GetCopy
                If Not Me.CutMode Then
                    CType(node.Tag, Source).EditorGUID = System.Guid.NewGuid.ToString()

                    Dim frm As New NewItem(NewItem.Types.Project, Nothing, Nothing, Me.GetSources(), Nothing)
                    Dim renameOk As Boolean = False
                    Dim newName As String = CType(node.Tag, Source).Name
                    Dim stopInfiniteLoop As Integer = 0
                    While Not renameOk And stopInfiniteLoop < 100
                        stopInfiniteLoop += 1
                        newName = "Copy_of_" & newName
                        renameOk = Not frm.IsDuplicate_Source(newName)
                    End While
                    If stopInfiniteLoop >= 100 Then Throw New Exception("Could not paste.")
                    CType(node.Tag, Source).Name = newName
                End If
                node.Text = CType(node.Tag, Source).Name

            ElseIf TypeOf sourceNode.Tag Is Main Then
                node.Tag = CType(sourceNode.Tag, IMDPersistenceItem).GetCopy
                If Not Me.CutMode Then CType(node.Tag, Main).EditorGUID = System.Guid.NewGuid.ToString()
                CType(node.Tag, IEditorItem).OwnerGUID = OwnerGUID

            ElseIf sourceNode.Tag.ToString.Equals(TAG_FOLDER) Then
                node.Tag = TAG_FOLDER

            ElseIf TypeOf sourceNode.Tag Is Template Then
                node.Tag = CType(sourceNode.Tag, IMDPersistenceItem).GetCopy
                CType(node.Tag, IEditorItem).OwnerGUID = OwnerGUID

                If Not Me.CutMode Then
                    CType(node.Tag, Template).EditorGUID = System.Guid.NewGuid.ToString()

                    If Not dontRenameTemplate Then
                        Dim frm As New NewItem(NewItem.Types.Project, Me.GetPackageOwner(Me.tvwMain.SelectedNode), Nothing, Nothing, Nothing)
                        Dim renameOk As Boolean = False
                        Dim newName As String = CType(node.Tag, Template).Name
                        Dim stopInfiniteLoop As Integer = 0
                        While Not renameOk And stopInfiniteLoop < 100
                            stopInfiniteLoop += 1
                            newName = "Copy_of_" & newName
                            renameOk = Not frm.IsDuplicate_Template(newName)
                        End While
                        If stopInfiniteLoop >= 100 Then Throw New Exception("Could not paste.")
                        CType(node.Tag, Template).Name = newName
                    End If
                End If
                node.Text = CType(node.Tag, Template).Name

            End If

            For Each n As TreeNode In sourceNode.Nodes
                node.Nodes.Add(Me.GetNodeCopy(n, OwnerGUID, True))
            Next

            If sourceNode.IsExpanded Then node.Expand()

            Return node
        End Function

        Private Function DisablePaste(ByVal Node As TreeNode) As Boolean
            If Node.Tag.ToString.Equals(TAG_PROJECT) Then
                If Me.ClipBoard IsNot Nothing Then
                    If TypeOf Me.ClipBoard.Tag Is MDPackageTag Or Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        Return True
                    ElseIf TypeOf Me.ClipBoard.Tag Is Source Then
                        Return True
                    ElseIf Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        Return True
                    ElseIf Me.ClipBoard.Tag.ToString.Equals(TAG_FOLDER) Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is Template Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is boston.Persistence.Properties Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is Main Then
                        Return False
                    End If
                End If

            ElseIf TypeOf Node.Tag Is MDPackageTag Then
                If Me.ClipBoard IsNot Nothing Then
                    If TypeOf Me.ClipBoard.Tag Is MDPackageTag Or Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is Source Then
                        Return False
                    ElseIf Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        Return False
                    ElseIf Me.ClipBoard.Tag.ToString.Equals(TAG_FOLDER) Then
                        Return True
                    ElseIf TypeOf Me.ClipBoard.Tag Is Template Then
                        Return True
                    ElseIf TypeOf Me.ClipBoard.Tag Is Boston.Persistence.Properties Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is Main Then
                        Return False
                    End If
                End If

            ElseIf Node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                If Me.ClipBoard IsNot Nothing Then
                    If TypeOf Me.ClipBoard.Tag Is MDPackageTag Or Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        Return True
                    ElseIf TypeOf Me.ClipBoard.Tag Is Source Then
                        Return True
                    ElseIf Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        Return True
                    ElseIf Me.ClipBoard.Tag.ToString.Equals(TAG_FOLDER) Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is Template Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is boston.Persistence.Properties Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is Main Then
                        Return False
                    End If
                End If

            ElseIf Node.Tag.ToString.Equals(TAG_FOLDER) Then
                If Me.ClipBoard IsNot Nothing Then
                    If TypeOf Me.ClipBoard.Tag Is MDPackageTag Or Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is Source Then
                        Return False
                    ElseIf Me.ClipBoard.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                        Return False
                    ElseIf Me.ClipBoard.Tag.ToString.Equals(TAG_FOLDER) Then
                        Return True
                    ElseIf TypeOf Me.ClipBoard.Tag Is Template Then
                        Return True
                    ElseIf TypeOf Me.ClipBoard.Tag Is Boston.Persistence.Properties Then
                        Return False
                    ElseIf TypeOf Me.ClipBoard.Tag Is Main Then
                        Return False
                    End If
                End If

            End If

            Return False
        End Function

    End Class

End Namespace