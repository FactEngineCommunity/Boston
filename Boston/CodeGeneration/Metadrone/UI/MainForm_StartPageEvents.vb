Namespace UI

    Partial Friend Class MainForm

        Private settings As New Persistence.Settings.Settings()

        Private Sub StartPage1_NewProject() Handles StartPage1.NewProject
            Call Me.HidePopup()

            If Me.IsDirty Then If Me.SaveChanges() = System.Windows.Forms.DialogResult.Cancel Then Exit Sub

            If Me.tvwExplorer.CreateNewProject() Then
                Me.btnSave.Enabled = True
                Me.btnBuild.Enabled = True
            End If
        End Sub

        Private Sub StartPage1_OpenProject() Handles StartPage1.OpenProject
            Call Me.OpenProject()
        End Sub

        Private Sub StartPage1_OpenRecent(ByVal path As String) Handles StartPage1.OpenRecent
            Call Me.HidePopup()

            If Me.IsDirty Then If Me.SaveChanges() = System.Windows.Forms.DialogResult.Cancel Then Exit Sub

            Try
                Me.Cursor = Cursors.WaitCursor
                Me.lblStatus.Text = "Opening..."
                Me.Refresh()
                Call Me.OpenProject(path)
                Me.lblStatus.Text = path
                Me.Refresh()
                Me.ProjectPath = path
                Me.btnSave.Enabled = True

                'Update settings
                Me.settings.AddProject(New Persistence.Settings.Recent(Me.tvwExplorer.Transform(Me.tvwExplorer.tvwMain.Nodes(0)).Name, Me.ProjectPath))
                Call Me.settings.Save()
                'and reload in start page
                Call CType(Me.tcMain.TabPages(0).Controls(0), StartPage).LoadRecents(Me.settings)

                Me.projDirty = False

                Me.Cursor = Cursors.Default
            Catch ex As System.Exception
                Me.lblStatus.Text = "Open failed"
                Me.Refresh()
                Me.Cursor = Cursors.Default
                Dim msg As String = ex.Message & vbCrLf & vbCrLf & _
                                    "Would you like to remove this item from recents?"
                If System.Windows.Forms.MessageBox.Show(msg, "Open Error", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) = System.Windows.Forms.DialogResult.Yes Then
                    'Update settings
                    Me.settings.RemoveRecent(path)
                    Call Me.settings.Save()
                    'and reload in start page
                    Call CType(Me.tcMain.TabPages(0).Controls(0), StartPage).LoadRecents(Me.settings)
                End If
            End Try
        End Sub

    End Class

End Namespace