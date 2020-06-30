Namespace UI
    Friend Class MainForm

        Private Const IMG_METADRONE As Integer = 0
        Private Const IMG_MAIN As Integer = 1
        Private Const IMG_TEMPLATE As Integer = 2
        Private Const IMG_VB As Integer = 3
        Private Const IMG_CS As Integer = 4
        Private Const IMG_SOURCE As Integer = 5
        Private Const IMG_PROPERTIES As Integer = 6
        Private Const IMG_TEMPLATE_LIBRARY As Integer = 7

        Private Sub MainForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
            Me.Cursor = Cursors.WaitCursor

            'Indicate debug switches
            If Not My.Application.EXEC_ParseInThread Then Me.Text &= "      (parse no thread)"
            If My.Application.EXEC_ErrorSensitivity = My.MyApplication.ErrorSensitivity.Noisy Then Me.Text &= "      (noisy)"

            'Initialise
            Call Me.Initialise()

            'Apply start-up mode
            Select Case My.Application.Mode
                Case My.MyApplication.ExecModes.Open
                    'Open project file
                    Try
                        Me.lblStatus.Text = "Opening..."
                        Me.Refresh()
                        Call Me.OpenProject(My.Application.ProjectPath)
                        Me.lblStatus.Text = My.Application.ProjectPath
                        Me.Refresh()
                        Me.ProjectPath = My.Application.ProjectPath
                        Me.btnSave.Enabled = True

                        'But default to start page
                        Me.tcMain.SelectedIndex = 0
                    Catch ex As System.Exception
                        Me.lblStatus.Text = "Open failed"
                        Me.Refresh()
                        Me.Cursor = Cursors.Default
                        System.Windows.Forms.MessageBox.Show(ex.Message, "Open Error")
                    End Try

            End Select

            Me.lblStatus.Text = ""
            Me.Refresh()

            Me.Cursor = Cursors.Default
        End Sub

        Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            'Me.txtTemplate.HidePopup()
            If Not Me.buildCompleted Then
                If MessageBox.Show("Do you want to cancel building?", "Stil Building", MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                    Me.main.CancelBuild()
                    While Not Me.buildCompleted
                        Application.DoEvents()
                    End While

                Else
                    e.Cancel = True
                    Exit Sub

                End If
            End If

            Globals.SourcePlugins.Plugins.Clear()

            If Me.IsDirty Then
                If Me.SaveChanges() = System.Windows.Forms.DialogResult.Cancel Then
                    e.Cancel = True
                End If
            End If
        End Sub

        Private Sub MainForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
            Call Me.settings.Save()

            frmMain.zfrmCodeGenerator = Nothing
        End Sub

        Private Sub MainForm_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
            Me.SearchBox.Refresh()
        End Sub

        Private Sub grdOutput_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdOutput.DoubleClick
            Try
                Process.Start(Me.grdOutput.SelectedRows(0).Cells(0).Value.ToString)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Open File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End Sub

    End Class

End Namespace