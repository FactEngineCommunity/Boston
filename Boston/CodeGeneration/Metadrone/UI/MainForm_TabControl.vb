Imports Boston.Parser.Syntax.Constants
Imports Boston.Persistence

Namespace UI

    Partial Friend Class MainForm

        Friend Sub ClearTabs(ByVal ExceptTheseTabs As List(Of TabPage))
            'Remove all but the start
            Dim i As Integer = 0
            While i < Me.tcMain.TabPages.Count - 1
                i += 1

                'Don't remove the exceptions
                If ExceptTheseTabs IsNot Nothing Then
                    For Each exc In ExceptTheseTabs
                        If i = Me.tcMain.TabPages.IndexOf(exc) Then Continue While
                    Next
                End If

                Me.tcMain.TabPages.RemoveAt(i)
                i -= 1
            End While
        End Sub

        Private Sub tcMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcMain.SelectedIndexChanged
            Call Me.HidePopup()

            If Me.tcMain.TabPages.Count = 0 Then Exit Sub

            If Me.tcMain.SelectedTab.Controls.Count > 0 AndAlso _
               TypeOf Me.tcMain.SelectedTab.Controls(0) Is CodeEditor Then
                CType(Me.tcMain.SelectedTab.Controls(0), CodeEditor).FocusText()
            End If
        End Sub

        Private Sub tcMain_TabClosing(ByVal Tab As TabPage, ByRef Cancel As Boolean) Handles tcMain.TabClosing
            If Me.SaveChanges(Tab) = System.Windows.Forms.DialogResult.Cancel Then Cancel = True
        End Sub

        Private Sub tcMain_RightClick(ByVal SelectedTabIndex As Integer) Handles tcMain.RightClickTab
            'Enable context menu
            Me.mniCloseTab.Enabled = True
            Me.tcMain.ContextMenuStrip = Me.mnuTabControl
            If SelectedTabIndex = 0 Then Me.mniCloseTab.Enabled = False
        End Sub

        Private Sub mniCloseTab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniCloseTab.Click
            Call Me.tcMain.CloseTab(Me.tcMain.SelectedTab)
        End Sub

        Private Sub mniCloseOtherTabs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniCloseOtherTabs.Click
            If Me.SaveChanges() = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
            Dim tps As New List(Of TabPage)
            tps.Add(Me.tcMain.SelectedTab)
            Call Me.ClearTabs(tps)
        End Sub

        Private Sub mniCloseAllTabs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mniCloseAllTabs.Click
            If Me.SaveChanges() = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
            Call Me.ClearTabs(Nothing)
        End Sub

        Private Sub mnuTabControl_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles mnuTabControl.Closed
            'Reset context menu, so it won't show when right-clicking elsewhere on the tab
            Me.tcMain.ContextMenuStrip = Nothing
        End Sub

    End Class

End Namespace
