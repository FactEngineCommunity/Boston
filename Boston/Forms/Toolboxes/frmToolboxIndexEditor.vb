Imports System.Reflection

Public Class frmToolboxIndexEditor

    Public mrTable As RDS.Table
    Public mrIndexList As IEnumerable(Of Object)

    Private Sub frmToolboxIndexEditor_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()
    End Sub

    Public Sub SetupForm()

        If Me.mrTable IsNot Nothing Then

            mrIndexList = (From [Index] In Me.mrTable.Index
                           From Column In [Index].Column
                           Select New With {.IndexName = [Index].Name, .Qualifier = [Index].IndexQualifier, [Index].IsPrimaryKey, .ColumnName = Column.Name}).ToList

            Me.DataGridView.DataSource = mrIndexList

        End If

    End Sub

    Private Sub frmToolboxIndexEditor_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Try
            prApplication.ToolboxForms.Remove(Me)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DataGridView_MouseDown(sender As Object, e As MouseEventArgs) Handles DataGridView.MouseDown

        If e.Button = MouseButtons.Right Then

            Me.DataGridView.ContextMenuStrip = Me.ContextMenuStripIndex
        End If

    End Sub

    Private Sub DeleteIndexToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteIndexToolStripMenuItem.Click

        Dim lsIndexName As String = Nothing
        Try
            If Me.DataGridView.SelectedRows.Count > 0 Then
                lsIndexName = Me.DataGridView.SelectedRows.Item(0).Cells.Item(0).Value
            ElseIf Me.DataGridView.SelectedCells.Count > 0 Then
                Dim liRowIndex = Me.DataGridView.SelectedCells.Item(0).RowIndex
                If liRowIndex >= 0 Then
                    lsIndexName = Me.DataGridView.Rows.Item(liRowIndex).Cells.Item(0).Value
                End If
            Else
                Exit Sub
            End If

            If lsIndexName IsNot Nothing Then
                Try
                    Dim lrIndex = mrTable.Model.Index.Find(Function(x) x.Name = lsIndexName)
                    Dim lrRoleConstraint As FBM.RoleConstraint = lrIndex.getResponsibleRoleConstraintFromORMModel
                    Dim lsMessage As String
                    lsMessage = "Are you absolutely sure you want to delete Index, '" & lsIndexName & "'?"
                    If lrRoleConstraint IsNot Nothing Then
                        lsMessage &= vbCrLf & vbCrLf & "The Index is tied to the Role Constraint, '" & lrRoleConstraint.Id & "'."
                    End If
                    If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Call mrTable.removeIndex(lrIndex)
                    End If
                Catch ex As Exception

                End Try
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class