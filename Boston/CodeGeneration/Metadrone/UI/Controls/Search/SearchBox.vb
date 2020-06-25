Namespace UI

    Friend Class SearchBox

        Private scope As SearchScopes = SearchScopes.Project
        Private alreadyFocused As Boolean = False 'from: http://stackoverflow.com/questions/97459/automatically-select-all-text-on-focus-in-winforms-textbox

        Public Event Search(ByVal searchText As String, ByVal SearchScope As SearchScopes)
        Public Event LeaveSearch()
        Public Event RequestLeave()

        Public Enum SearchScopes
            Project = 1
            Package = 2
            CurrentDocument = 3
        End Enum

        Public Property SearchScope() As SearchScopes
            Get
                Return Me.scope
            End Get
            Set(ByVal value As SearchScopes)
                Me.scope = value
                Select Case Me.scope
                    Case SearchScopes.Project
                        Me.pnlDown1.BackgroundImage = Me.ImageList1.Images(0)
                        Me.ToolTip1.SetToolTip(Me.pnlDown1, "Search project")
                        Me.ToolTip1.SetToolTip(Me.pnlDown2, "Search project")

                    Case SearchScopes.Package
                        Me.pnlDown1.BackgroundImage = Me.ImageList1.Images(1)
                        Me.ToolTip1.SetToolTip(Me.pnlDown1, "Search current package")
                        Me.ToolTip1.SetToolTip(Me.pnlDown2, "Search current package")

                    Case SearchScopes.CurrentDocument
                        Me.pnlDown1.BackgroundImage = Me.ImageList1.Images(2)
                        Me.ToolTip1.SetToolTip(Me.pnlDown1, "Search current document")
                        Me.ToolTip1.SetToolTip(Me.pnlDown2, "Search current document")

                End Select

            End Set
        End Property

        Public Sub SetFocus()
            Me.txtSearch.Focus()
        End Sub

        Private Sub SearchBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.SearchScope = SearchScopes.Project
        End Sub

        Private Sub pnlDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlDown1.Click, pnlDown2.Click
            Dim loc As System.Drawing.Point = Me.pnlDown1.Location
            loc.Y += Me.pnlDown1.Height - 2
            Me.mnuScope.Show(Me.pnlDown1, loc)
        End Sub

        Private Sub mniProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniProject.Click
            Me.SearchScope = SearchScopes.Project
        End Sub

        Private Sub mniPackage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniPackage.Click
            Me.SearchScope = SearchScopes.Package
        End Sub

        Private Sub mniDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniDocument.Click
            Me.SearchScope = SearchScopes.CurrentDocument
        End Sub

        Private Sub pnlSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlSearch.Click
            If Me.txtSearch.Text.Length = 0 Then Exit Sub

            RaiseEvent Search(Me.txtSearch.Text, Me.scope)
        End Sub

        Private Sub txtSearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.GotFocus
            ' Select all text only if the mouse isn't down.
            ' This makes tabbing to the textbox give focus.
            If MouseButtons = MouseButtons.None Then
                Me.txtSearch.SelectAll()
                Me.alreadyFocused = True
            End If

            Me.pnlSearch.BackgroundImage = Me.imlBackGroundStart.Images(1)
            Me.pnlText.BackgroundImage = Me.imlBackGroundMiddle.Images(1)
            Me.pnlEnd.BackgroundImage = Me.imlBackGroundEnd.Images(1)
        End Sub

        Private Sub txtSearch_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.Leave
            Me.alreadyFocused = False
            RaiseEvent LeaveSearch()

            Me.pnlSearch.BackgroundImage = Me.imlBackGroundStart.Images(0)
            Me.pnlText.BackgroundImage = Me.imlBackGroundMiddle.Images(0)
            Me.pnlEnd.BackgroundImage = Me.imlBackGroundEnd.Images(0)
        End Sub

        Private Sub txtSearch_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.LostFocus
            Me.pnlSearch.BackgroundImage = Me.imlBackGroundStart.Images(0)
            Me.pnlText.BackgroundImage = Me.imlBackGroundMiddle.Images(0)
            Me.pnlEnd.BackgroundImage = Me.imlBackGroundEnd.Images(0)
        End Sub

        Private Sub txtSearch_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtSearch.MouseUp
            ' Web browsers like Google Chrome select the text on mouse up.
            ' They only do it if the textbox isn't already focused,
            ' and if the user hasn't selected all text.
            If Not Me.alreadyFocused And Me.txtSearch.SelectionLength = 0 Then
                Me.alreadyFocused = True
                Me.txtSearch.SelectAll()
            End If
        End Sub

        Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
            Select Case e.KeyCode
                Case Keys.Up
                    Select Case Me.SearchScope
                        Case SearchScopes.Package : Me.SearchScope = SearchScopes.Project
                        Case SearchScopes.CurrentDocument : Me.SearchScope = SearchScopes.Package
                    End Select
                    e.Handled = True
                    e.SuppressKeyPress = True

                Case Keys.Down
                    Select Case Me.SearchScope
                        Case SearchScopes.Project : Me.SearchScope = SearchScopes.Package
                        Case SearchScopes.Package : Me.SearchScope = SearchScopes.CurrentDocument
                    End Select
                    e.Handled = True
                    e.SuppressKeyPress = True

                Case Keys.Escape
                    Me.alreadyFocused = False
                    e.SuppressKeyPress = True
                    RaiseEvent RequestLeave()

                Case Keys.F3
                    e.Handled = True
                    e.SuppressKeyPress = True
                    RaiseEvent RequestLeave()
                    RaiseEvent Search(Me.txtSearch.Text, Me.scope)

            End Select
        End Sub

        Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
            If Me.txtSearch.Text.Length = 0 Then Exit Sub

            Select Case e.KeyChar
                Case ControlChars.Cr, ControlChars.Lf, ControlChars.CrLf.ToCharArray, ControlChars.NewLine.ToCharArray, ControlChars.FormFeed
                    RaiseEvent RequestLeave()
                    RaiseEvent Search(Me.txtSearch.Text, Me.scope)
                    e.Handled = True

            End Select
        End Sub

    End Class

End Namespace