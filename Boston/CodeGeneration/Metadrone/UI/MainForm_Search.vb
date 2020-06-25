Namespace UI

    Partial Friend Class MainForm

        Private searchingFromExplorer As Boolean = False
        Private leavingSearch As Boolean = False 'This is for a wierdo bug where tabbing from search will always tab to the explorer control


#Region "Behaviour"

        Private Sub SearchReset()
            Me.lblSearchMessage.Text = ""
        End Sub

        'Called by codeeditor CTRL-F
        Private Sub RequestSearch()
            Me.searchingFromExplorer = False
            Me.SearchBox.SetFocus()

            If Me.tcMain.SelectedTab IsNot Nothing Then
                If TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then
                    'Assume search within main/template
                    Me.SearchBox.SearchScope = UI.SearchBox.SearchScopes.CurrentDocument
                End If
            End If
        End Sub

        'Called by form CTRL-F
        Private Sub RequestSearchFromForm()
            'If texteditor got focus, use that function
            If Me.tcMain.SelectedTab IsNot Nothing AndAlso _
               Me.tcMain.SelectedTab.Controls.Count > 0 AndAlso _
               TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor AndAlso _
               CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).Focused Then
                Me.RequestSearch()
                Exit Sub
            End If

            Me.searchingFromExplorer = True
            Me.SearchBox.SetFocus()

            'Default to project scope
            Me.SearchBox.SearchScope = UI.SearchBox.SearchScopes.Project
            If Me.tvwExplorer.SelectedNode IsNot Nothing AndAlso Me.tvwExplorer.SelectedNode.Tag IsNot Nothing Then
                'Selected node, go to appropriate scope
                If Me.tvwExplorer.SelectedNode.Tag.ToString.Equals(Explorer.TAG_PROJECT) Then
                    Me.SearchBox.SearchScope = UI.SearchBox.SearchScopes.Project
                Else
                    Me.SearchBox.SearchScope = UI.SearchBox.SearchScopes.Package
                End If
            Else
                'Select the project node
                If Me.tvwExplorer.Nodes.Count > 0 Then
                    Me.tvwExplorer.SelectedNode = Me.tvwExplorer.Nodes(0)
                    Me.SearchBox.SearchScope = UI.SearchBox.SearchScopes.Project
                End If
            End If
        End Sub

        Private Sub SearchBox_RequestLeave() Handles SearchBox.RequestLeave
            If Me.tcMain.SelectedTab Is Nothing Then Exit Sub
            If Me.tcMain.SelectedTab.Controls.Count = 0 Then Exit Sub
            If TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then
                Me.lblSearchMessage.Text = ""
                CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).Focus()
                CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).ClearSelection()
            End If
        End Sub

        Private Sub SearchBox_Search(ByVal searchText As String, ByVal SearchScope As SearchBox.SearchScopes) Handles SearchBox.Search
            Call Me.Search(searchText, SearchScope)
        End Sub

        Private Sub SearchBox_LeaveSearch() Handles SearchBox.LeaveSearch
            Me.leavingSearch = True
        End Sub

        Private Sub MainForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
            'Form-wide key catch
            Select Case e.KeyCode
                Case Keys.F
                    'CTRL-F find
                    If e.Modifiers = Keys.Control Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                        Call Me.RequestSearchFromForm()
                    End If

                Case Keys.F3
                    'F3 invoke find
                    e.Handled = True
                    e.SuppressKeyPress = True
                    Call Me.Search(Me.SearchBox.txtSearch.Text, Me.SearchBox.SearchScope)
                    Call Me.FocusCurrentTab()

            End Select
        End Sub

        Private Sub tvwExplorer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvwExplorer.GotFocus
            If Me.leavingSearch Then
                Call Me.FocusCurrentTab()
                Me.leavingSearch = False
            End If
        End Sub

        Private Sub FocusCurrentTab()
            If Me.tcMain.SelectedTab Is Nothing Then Exit Sub
            If Me.tcMain.SelectedTab.Controls.Count = 0 Then Exit Sub
            If TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then
                CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).Focus()
            End If
        End Sub

#End Region


#Region "Searching"

#Region "Search documents"

        Public Class SearchMatch
            Public Index As Integer = -1
            Public Length As Integer = 0

            Public Sub New(ByVal index As Integer, ByVal length As Integer)
                Me.Index = index
                Me.Length = length
            End Sub
        End Class

        Public Class SearchDocument
            Public CodeEditorGUID As String
            Public Matches As New List(Of SearchMatch)

            Public CurrentMatchIndex As Integer = -1

            Public Sub New(ByVal codeEditorGUID As String, ByVal pattern As String, ByVal ignoreCase As Boolean, ByVal source As String)
                Me.CodeEditorGUID = codeEditorGUID

                Me.Matches = New List(Of SearchMatch)
                Dim currIdx As Integer = 0
                Dim foundIdx As Integer = -1
                If ignoreCase Then
                    foundIdx = source.IndexOf(pattern, StringComparison.CurrentCultureIgnoreCase)
                Else
                    foundIdx = source.IndexOf(pattern)
                End If
                While foundIdx > -1
                    Me.Matches.Add(New SearchMatch(foundIdx, pattern.Length))
                    currIdx = foundIdx + 1
                    If ignoreCase Then
                        foundIdx = source.IndexOf(pattern, currIdx, StringComparison.CurrentCultureIgnoreCase)
                    Else
                        foundIdx = source.IndexOf(pattern, currIdx)
                    End If
                End While
            End Sub
        End Class

#End Region

        Private currentScope As Boston.UI.SearchBox.SearchScopes = UI.SearchBox.SearchScopes.Project
        Private currentSearchPattern As String = ""
        Private textAlteredSoSearchAgain As Boolean = False
        Private searchDocuments As New List(Of SearchDocument)
        Private currentDocIdx As Integer = -1
        Private sessionStartDocIdx As Integer = -1 'For proj/package scope searching, we want to cycle from where we started, not from zero.
        Private sessionFinished As Boolean = False


        Private Sub Search(ByVal searchText As String, ByVal SearchScope As SearchBox.SearchScopes, _
                           Optional ByVal overrideNewSearch As Boolean = False)
            If searchText.Length = 0 Then Exit Sub
            If Me.tvwExplorer.Nodes.Count = 0 Then Exit Sub
            If Me.tvwExplorer.SelectedNode Is Nothing Then Me.tvwExplorer.SelectedNode = Me.tvwExplorer.Nodes(0)

            If Not searchText.Equals(Me.currentSearchPattern, StringComparison.CurrentCultureIgnoreCase) Or _
               SearchScope <> Me.currentScope Or _
               textAlteredSoSearchAgain Or overrideNewSearch Then
                'New search

                'Clear up
                Me.sessionFinished = False
                Me.sessionStartDocIdx = -1
                Me.currentDocIdx = -1
                Call Me.searchDocuments.Clear()
                Call Me.ClearSelections()

                'Build searches
                Select Case SearchScope
                    Case Boston.UI.SearchBox.SearchScopes.Project
                        'Build search list of entire project
                        For Each node As TreeNode In Me.tvwExplorer.Nodes
                            Call Me.GetNodes(searchText, node)
                        Next

                    Case Boston.UI.SearchBox.SearchScopes.Package
                        'Build search list of the package

                        'Get package node to search
                        Dim pkgNode As TreeNode = Nothing
                        If Me.tcMain.SelectedTab IsNot Nothing AndAlso Me.tcMain.SelectedTab.Controls.Count > 0 AndAlso _
                           TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then
                            'If an editor is open, go to the package owner of that document
                            pkgNode = Me.FindNode(Me.CodeEditorGUID(Me.tcMain.SelectedTab.Controls(0)), Me.tvwExplorer.tvwMain)
                            pkgNode = Me.tvwExplorer.GetPackageOwnerNode(pkgNode)
                        Else
                            'Go to the package owner of the selected node
                            pkgNode = Me.tvwExplorer.GetPackageOwnerNode(Me.tvwExplorer.SelectedNode)
                        End If
                        If pkgNode IsNot Nothing Then
                            For Each node As TreeNode In pkgNode.Nodes
                                Call Me.GetNodes(searchText, node)
                            Next
                        End If

                    Case Boston.UI.SearchBox.SearchScopes.CurrentDocument
                        'Open explorer node if launched from there
                        If Me.searchingFromExplorer Then
                            Me.OpenNode(Me.tvwExplorer.SelectedNode)
                            'And set focus back to search box
                            Me.SearchBox.Focus()
                            Me.searchingFromExplorer = False
                        End If

                        'Build single search for the document
                        Me.currentDocIdx = 0
                        If Me.tcMain.SelectedTab IsNot Nothing AndAlso Me.tcMain.SelectedTab.Controls.Count > 0 AndAlso _
                           TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then
                            Me.searchDocuments.Add(New SearchDocument(Me.CodeEditorGUID(Me.tcMain.SelectedTab.Controls(0)), _
                                                                      searchText, True, _
                                                                      CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).Text))
                        End If

                End Select

                Me.currentScope = SearchScope
                Me.currentSearchPattern = searchText
                Me.textAlteredSoSearchAgain = False
            End If

            'Find next
            Select Case SearchScope
                Case Boston.UI.SearchBox.SearchScopes.Project, Boston.UI.SearchBox.SearchScopes.Package
                    If Me.searchDocuments.Count = 0 Then
                        'No documents, report no matches
                        Me.lblSearchMessage.Text = "No matches"
                    Else
                        If Me.tcMain.SelectedTab Is Nothing Then
                            'Open if not already
                            Me.currentDocIdx = 0
                            Me.sessionStartDocIdx = 0
                            Me.OpenNode(Me.FindNode(Me.searchDocuments(Me.currentDocIdx).CodeEditorGUID, Me.tvwExplorer.tvwMain))
                        Else
                            If Me.sessionFinished Then
                                'Finished the session, open the next document
                                Me.OpenNode(Me.FindNode(Me.searchDocuments(Me.currentDocIdx).CodeEditorGUID, Me.tvwExplorer.tvwMain))
                            Else
                                'If currently open tab is different, go there first
                                Me.currentDocIdx = Me.FindDocIdx()
                                If Me.currentDocIdx = -1 Then
                                    Me.currentDocIdx = 0
                                    Me.OpenNode(Me.FindNode(Me.searchDocuments(Me.currentDocIdx).CodeEditorGUID, Me.tvwExplorer.tvwMain))
                                End If
                            End If

                            'Initialise the start doc idx to this if first time round
                            If Me.sessionStartDocIdx = -1 Then Me.sessionStartDocIdx = Me.currentDocIdx
                        End If

                        'Find next in the current document
                        Dim matchCount As Integer = CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).txtBox.FindNext(Me.searchDocuments(Me.currentDocIdx))
                        Select Case matchCount
                            Case -1
                                'Prepare to search next document, and reset to beginning if gone past end
                                Me.currentDocIdx += 1
                                If Me.currentDocIdx > Me.searchDocuments.Count - 1 Then Me.currentDocIdx = 0

                                'Finished the session
                                Me.sessionFinished = (Me.currentDocIdx = Me.sessionStartDocIdx)
                                If Me.sessionFinished Then Me.lblSearchMessage.Text = "Search completed."

                                'Only search again if haven't finished session (need to let user see report).
                                If Not Me.sessionFinished Then
                                    'Open next document
                                    Me.OpenNode(Me.FindNode(Me.searchDocuments(Me.currentDocIdx).CodeEditorGUID, Me.tvwExplorer.tvwMain))

                                    'Resume search
                                    Call Me.Search(searchText, SearchScope)
                                End If

                            Case 0
                                'No matches here
                                Me.lblSearchMessage.Text = "No matches"
                            Case Else
                                'Keep searching
                                Me.lblSearchMessage.Text = (Me.searchDocuments(Me.currentDocIdx).CurrentMatchIndex + 1).ToString & " of " & matchCount.ToString
                        End Select
                    End If


                Case Boston.UI.SearchBox.SearchScopes.CurrentDocument
                    If Me.searchDocuments.Count = 0 Then
                        'No documents, report no matches
                        Me.lblSearchMessage.Text = "No matches"
                        Exit Sub
                    ElseIf Me.searchDocuments.Count = 1 Then
                        'Just use first doc
                        Me.currentDocIdx = 0
                        'If the selected tab has changed, search again in document scope
                        If Me.FindDocIdx() = -1 Then
                            If Not overrideNewSearch Then
                                Call Me.Search(searchText, SearchScope, True)
                                Exit Sub
                            Else
                                'Searched again, but no matching documents
                                Me.lblSearchMessage.Text = "No matches"
                                Exit Sub
                            End If
                        End If
                    Else
                        'Multiple docs mean was previously in a package scope. Find the selected tab's search doc.
                        Dim prevIdx As Integer = Me.currentDocIdx
                        Me.currentDocIdx = Me.FindDocIdx()
                        'If not found in the selected, open up the next document
                        If Me.currentDocIdx = -1 Then
                            Me.currentDocIdx = prevIdx + 1
                            If Me.currentDocIdx > Me.searchDocuments.Count - 1 Then Me.currentDocIdx = 0
                            Me.OpenNode(Me.FindNode(Me.searchDocuments(Me.currentDocIdx).CodeEditorGUID, Me.tvwExplorer.tvwMain))
                        End If
                    End If
                    'Find next in current document
                    Dim matchCount As Integer = CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).txtBox.FindNext(Me.searchDocuments(Me.currentDocIdx))
                    Select Case matchCount
                        Case -1
                            Me.lblSearchMessage.Text = "Search completed."
                        Case 0
                            Me.lblSearchMessage.Text = "No matches"
                        Case Else
                            Me.lblSearchMessage.Text = (Me.searchDocuments(Me.currentDocIdx).CurrentMatchIndex + 1).ToString & " of " & matchCount.ToString
                    End Select

            End Select
        End Sub

        Private Sub ClearSelections()
            For Each tb As TabPage In Me.tcMain.TabPages
                If tb.Controls.Count > 0 AndAlso TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then
                    CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).txtBox.ClearSelection()
                End If
            Next
        End Sub

        Private Sub GetNodes(ByVal searchPattern As String, ByVal currentNode As TreeNode)
            If TypeOf currentNode.Tag Is Persistence.Main Then
                With CType(currentNode.Tag, Persistence.Main)
                    Dim sd As New SearchDocument(.EditorGUID, searchPattern, True, .Text)
                    If sd.Matches.Count > 0 Then Me.searchDocuments.Add(sd)
                End With

            ElseIf TypeOf currentNode.Tag Is Persistence.Template Then
                With CType(currentNode.Tag, Persistence.Template)
                    Dim sd As New SearchDocument(.EditorGUID, searchPattern, True, .Text)
                    If sd.Matches.Count > 0 Then Me.searchDocuments.Add(sd)
                End With

            ElseIf TypeOf currentNode.Tag Is Persistence.CodeDOM_VB Then
                With CType(currentNode.Tag, Persistence.CodeDOM_VB)
                    Dim sd As New SearchDocument(.EditorGUID, searchPattern, True, .Text)
                    If sd.Matches.Count > 0 Then Me.searchDocuments.Add(sd)
                End With

            ElseIf TypeOf currentNode.Tag Is Persistence.CodeDOM_CS Then
                With CType(currentNode.Tag, Persistence.CodeDOM_CS)
                    Dim sd As New SearchDocument(.EditorGUID, searchPattern, True, .Text)
                    If sd.Matches.Count > 0 Then Me.searchDocuments.Add(sd)
                End With

            End If

            For Each node As TreeNode In currentNode.Nodes
                Call Me.GetNodes(searchPattern, node)
            Next
        End Sub

        Private Function FindNode(ByVal GUID As String, ByVal tree As TreeView) As TreeNode
            For Each node As TreeNode In tree.Nodes
                Dim n As TreeNode = Me.FindNode(GUID, node)
                If n IsNot Nothing Then Return n
            Next

            Return Nothing
        End Function

        Private Function FindNode(ByVal GUID As String, ByVal currentNode As TreeNode) As TreeNode
            If currentNode.Tag IsNot Nothing Then
                If TypeOf currentNode.Tag Is Persistence.IEditorItem Then
                    If CType(currentNode.Tag, Persistence.IEditorItem).EditorGUID.Equals(GUID) Then Return currentNode
                Else
                    For Each node As TreeNode In currentNode.Nodes
                        Dim n As TreeNode = Me.FindNode(GUID, node)
                        If n IsNot Nothing Then Return n
                    Next
                End If
            End If

            Return Nothing
        End Function

        Private Function CodeEditorGUID(ByVal ctl As Control) As String
            If TypeOf ctl.Tag Is Persistence.Template Then
                Return CType(ctl.Tag, Persistence.Template).EditorGUID
            ElseIf TypeOf ctl.Tag Is Persistence.Main Then
                Return CType(ctl.Tag, Persistence.Main).EditorGUID
            ElseIf TypeOf ctl.Tag Is Persistence.CodeDOM_VB Then
                Return CType(ctl.Tag, Persistence.CodeDOM_VB).EditorGUID
            ElseIf TypeOf ctl.Tag Is Persistence.CodeDOM_CS Then
                Return CType(ctl.Tag, Persistence.CodeDOM_CS).EditorGUID
            Else
                Return ""
            End If
        End Function

        Private Function FindDocIdx() As Integer
            If Me.tcMain.SelectedTab.Controls(0).Tag Is Nothing Then Return -1

            Dim guid As String = Me.CodeEditorGUID(Me.tcMain.SelectedTab.Controls(0))
            If guid.Length = 0 Then Return -1

            For i As Integer = 0 To Me.searchDocuments.Count - 1
                If Me.searchDocuments(i).CodeEditorGUID.Equals(guid) Then Return i
            Next

            Return -1
        End Function


#End Region


    End Class

End Namespace