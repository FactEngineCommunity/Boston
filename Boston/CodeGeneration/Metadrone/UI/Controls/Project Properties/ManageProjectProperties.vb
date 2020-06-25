Namespace UI

    Friend Class ManageProjectProperties

        Private Const RowIdx_BeginTag As Integer = 0
        Private Const RowIdx_EndTag As Integer = 1

        Public Event SavePress()
        Public Event Run()
        Public Shadows Event ValueChanged(ByVal value As Object)

        Private Sub ManageProjectProperties_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Me.Tag Is Nothing Then Exit Sub

            Me.grdMain.Rows.Add(New String() {"Tag Begin", CType(Me.Tag, Persistence.Properties).BeginTag})
            Me.grdMain.Rows.Add(New String() {"Tag End", CType(Me.Tag, Persistence.Properties).EndTag})
            Me.grdMain.Rows(0).Selected = True
            Me.SetSettingsDescriptions(0)
            Me.SuperMain.Text = CType(Me.Tag, Persistence.Properties).SuperMain
            Me.btnSupermain_Click(Me.btnSupermain, Nothing)
        End Sub

        Private Sub btnSupermain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSupermain.Click
            Me.SuperMain.Visible = True
            Me.splitProps.Visible = False
            Call Me.HiLightButton(sender)
            Me.SuperMain.FocusText()
        End Sub

        Private Sub btnSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettings.Click
            Me.SuperMain.Visible = False
            Me.splitProps.Visible = True
            Call Me.HiLightButton(sender)
        End Sub

        Private Sub HiLightButton(ByVal sender As System.Object)
            For Each btn As ToolStripButton In Me.tsMenu.Items
                btn.BackColor = Me.tsMenu.BackColor
                btn.ForeColor = Me.tsMenu.ForeColor
            Next
            CType(sender, ToolStripButton).BackColor = Color.LightSteelBlue
            CType(sender, ToolStripButton).ForeColor = Color.White
            Me.Refresh()
        End Sub

        Private Sub grdMain_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdMain.SelectionChanged
            If Me.grdMain.CurrentRow Is Nothing Then
                Me.lblTitle.Text = ""
                Me.lblDescription.Text = ""
                Exit Sub
            End If

            Me.SetSettingsDescriptions(Me.grdMain.CurrentRow.Index)
        End Sub

        Private Sub SetSettingsDescriptions(ByVal index As Integer)
            Me.lblTitle.Text = Me.grdMain.Rows(index).Cells(0).Value.ToString
            Select Case index
                Case RowIdx_BeginTag
                    Me.lblDescription.Text = "Begin tag for Metadrone code." & System.Environment.NewLine & "(Currently read-only)."
                Case RowIdx_EndTag
                    Me.lblDescription.Text = "End tag to end Metadrone code." & System.Environment.NewLine & "(Currently read-only)."
            End Select
        End Sub

        Private Sub grdMain_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdMain.CellValueChanged
            Select Case e.RowIndex
                'Revert begin/end tag changes for now..
                Case 0
                    Me.grdMain.Rows(0).Cells(1).Value = CType(Me.Tag, Persistence.Properties).BeginTag
                Case 1
                    Me.grdMain.Rows(1).Cells(1).Value = CType(Me.Tag, Persistence.Properties).BeginTag
                Case Else
                    RaiseEvent ValueChanged(Me.grdMain.Rows(e.RowIndex).Cells(1).Value)
            End Select
        End Sub

        Public Sub UpdateTag()
            If Me.Tag Is Nothing Then Exit Sub

            If Me.grdMain.Rows(RowIdx_BeginTag).Cells(1).Value Is Nothing Then Me.grdMain.Rows(RowIdx_BeginTag).Cells(1).Value = ""
            If Me.grdMain.Rows(RowIdx_EndTag).Cells(1).Value Is Nothing Then Me.grdMain.Rows(RowIdx_EndTag).Cells(1).Value = ""

            With CType(Me.Tag, Persistence.Properties)
                .BeginTag = Me.grdMain.Rows(RowIdx_BeginTag).Cells(1).Value.ToString
                .EndTag = Me.grdMain.Rows(RowIdx_EndTag).Cells(1).Value.ToString
                .SuperMain = Me.SuperMain.Text
            End With
        End Sub

        Private Sub SuperMain_SavePress() Handles SuperMain.SavePress
            RaiseEvent SavePress()
        End Sub

        Private Sub SuperMain_Run() Handles SuperMain.Run
            RaiseEvent Run()
        End Sub

        Private Sub SuperMain_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SuperMain.TextChanged
            RaiseEvent ValueChanged(Me.SuperMain.Text)
        End Sub

    End Class

End Namespace