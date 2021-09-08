Imports System.Reflection

Public Class frmToolboxTableData

    Public mrModel As FBM.Model
    Public mrTable As RDS.Table = Nothing
    Private mrRecordset As ORMQL.Recordset
    Private mrDataGridList As ORMQL.RecordsetDataGridList

    'For Cell text editting
    Private OldValue, NewValue As String

    Public Function EqualsByName(ByVal other As Form) As Boolean
        Return Me.Name = other.Name
    End Function

    Private Sub frmToolboxTableData_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Public Sub SetupForm()

        'RemoveHandler Me.AdvancedDataGridView.RowsRemoved, AddressOf DataGridView_RowsRemoved

        Dim lsSQLQuery As String

        If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
        Else
            If Me.mrTable Is Nothing Then
            Else
                lsSQLQuery = "SELECT * FROM " & mrTable.DatabaseName & vbCrLf
                lsSQLQuery &= " LIMIT 100"
                Me.mrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(lsSQLQuery)

                Me.mrDataGridList = New ORMQL.RecordsetDataGridList(Me.mrRecordset, Me.mrTable)
                'Me.AdvancedDataGridView.DataSource = Me.mrDataGridList
                Me.AdvancedDataGridView.DataSource = Me.mrDataGridList
            End If
        End If

        Me.ToolStripStatusLabel.Text = ""

        Me.AdvancedDataGridView.RowTemplate.Height = Me.AdvancedDataGridView.Font.Height + 8

        If Me.mrTable IsNot Nothing Then
            Me.GroupBox1.Text = "Table Name: " & Me.mrTable.Name
        End If

        'AddHandler Me.AdvancedDataGridView.RowsRemoved, AddressOf DataGridView_RowsRemoved

    End Sub

    Private Sub frmToolboxTableData_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub ToolStripButtonCommit_Click(sender As Object, e As EventArgs) Handles ToolStripButtonCommit.Click

        Try
            Dim larNewModifiedFacts = From Fact In Me.mrRecordset.Facts
                                      Where Fact.IsNewFact
                                      Select Fact

            Dim lsSQLQuery As String
            For Each lrFact In larNewModifiedFacts
                lsSQLQuery = "INSERT INTO " & Me.mrTable.Name & "("
                Dim liInd = 0
                Dim lrColumn As RDS.Column
                Dim larColumn As New List(Of RDS.Column)
                For Each lsColumn In Me.mrRecordset.Columns
                    If liInd > 0 Then lsSQLQuery &= ","
                    lsSQLQuery &= lsColumn
                    lrColumn = Me.mrTable.Column.Find(Function(x) x.Name = lsColumn)
                    larColumn.Add(lrColumn)
                    liInd += 1
                Next
                lsSQLQuery &= ") VALUES ("
                liInd = 0
                For Each lsColumn In Me.mrRecordset.Columns
                    If liInd > 0 Then lsSQLQuery &= ","
                    lsSQLQuery &= Richmond.returnIfTrue(larColumn(liInd).DataTypeIsText, "'", "")
                    lsSQLQuery &= lrFact.Data(liInd).Data
                    lsSQLQuery &= Richmond.returnIfTrue(larColumn(liInd).DataTypeIsText, "'", "")
                    liInd += 1
                Next
                lsSQLQuery &= ")"

                Dim lrRecordset = Me.mrModel.DatabaseConnection.GO(lsSQLQuery)

                If Not lrRecordset.ErrorReturned Then
                    Me.ToolStripButtonCommit.Enabled = False
                    Me.ToolStripStatusLabel.Text = lrRecordset.ErrorString
                    Me.ToolStripStatusLabel.ForeColor = Color.Black
                Else
                    Me.ToolStripStatusLabel.Text = lrRecordset.ErrorString
                    Me.ToolStripStatusLabel.ForeColor = Color.Red
                End If
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ToolStripButtonUndo_Click(sender As Object, e As EventArgs) Handles ToolStripButtonUndo.Click

        Try
            Dim lbNewFactRemoved As Boolean = False
            Dim larNewFact = From Fact In Me.mrRecordset.Facts
                             Where Fact.IsNewFact
                             Select Fact

            For Each lrFact In larNewFact.ToArray
                Me.mrRecordset.Facts.Remove(lrFact)
                lbNewFactRemoved = True
            Next

            If lbNewFactRemoved Then
                'Dim lrDataGridList As New ORMQL.RecordsetDataGridList(Me.mrRecordset, Me.mrTable)
                Me.AdvancedDataGridView.DataSource = Nothing
                Me.AdvancedDataGridView.DataSource = Me.mrDataGridList
            End If

            Me.ToolStripButtonUndo.Enabled = False
            Me.ToolStripButtonCommit.Enabled = False
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AdvancedDataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles AdvancedDataGridView.CellBeginEdit

        Me.ToolStripStatusLabel.Text = ""

        If e.RowIndex <= Me.mrRecordset.Facts.Count - 1 Then
            Me.OldValue = Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data
        Else
            Dim lrNewFact As FBM.Fact = Me.mrRecordset.Facts(0).Clone(Me.mrRecordset.Facts(0).FactType, True)
            lrNewFact.Id = System.Guid.NewGuid.ToString
            For Each lrFactData In lrNewFact.Data
                lrFactData.setData("", pcenumConceptType.Value, False)
            Next
            lrNewFact.IsNewFact = True
            Me.mrRecordset.Facts.Add(lrNewFact)
            Me.ToolStripButtonCommit.Enabled = True
            Me.ToolStripButtonUndo.Enabled = True
        End If

    End Sub

    Private Sub AdvancedDataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView.CellEndEdit

        Try
            Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data = Me.NewValue


            If Me.mrRecordset.Facts(e.RowIndex).IsNewFact Then Exit Sub

            Dim lrColumn As RDS.Column = Me.mrTable.Column.Find(Function(x) x.Name = Me.mrRecordset.Columns(e.ColumnIndex))

            Dim larPKColumn As List(Of RDS.Column) = Me.mrTable.getPrimaryKeyColumns

            Dim liColumnIndex As Integer
            Dim lsValue As String
            For Each lrPKColumn In larPKColumn
                liColumnIndex = Me.mrRecordset.Columns.IndexOf(lrPKColumn.Name)
                lsValue = Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(liColumnIndex)).Data
                lrPKColumn.TemporaryData = lsValue
            Next

            Dim lrRecordset = prApplication.WorkingModel.DatabaseConnection.UpdateAttributeValue(Me.mrTable.Name, lrColumn, Me.NewValue, larPKColumn)

            If Not lrRecordset.ErrorReturned Then
                Me.ToolStripButtonCommit.Enabled = False
                Me.ToolStripStatusLabel.Text = lrRecordset.ErrorString
                Me.ToolStripStatusLabel.ForeColor = Color.Black
            Else
                Me.ToolStripStatusLabel.Text = lrRecordset.ErrorString
                Me.ToolStripStatusLabel.ForeColor = Color.Red
                Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data = Me.OldValue
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AdvancedDataGridView1_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles AdvancedDataGridView.CellValidating

        Me.NewValue = e.FormattedValue

    End Sub

    Private Sub AdvancedDataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles AdvancedDataGridView.KeyDown

        Try
            If e.KeyCode = Keys.Delete Then
                If Me.AdvancedDataGridView.SelectedRows.Count > 0 Then
                    If MsgBox("Are you sure you want to delete this row from the database?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                        Dim lrFact As FBM.Fact = Me.mrRecordset.Facts(Me.AdvancedDataGridView.SelectedRows(0).Index)
                        '20210608-VM-Put in code here to remove the record from the database
                        '=========================================================================
                        Dim lsSQLQuery As String = ""
                        lsSQLQuery.AppendString("DELETE FROM " & Me.mrTable.Name & vbCrLf & " WHERE ")
                        Dim liInd = 0
                        For Each lrColumn In Me.mrTable.getPrimaryKeyColumns
                            If liInd > 0 Then lsSQLQuery &= " AND "
                            lsSQLQuery.AppendString(lrColumn.Name & " = ")
                            Dim liValueColumnIndex = mrRecordset.Columns.IndexOf(lrColumn.Name)
                            lsSQLQuery &= Richmond.returnIfTrue(lrColumn.DataTypeIsText, "'", "")
                            lsSQLQuery &= lrFact.Data(liValueColumnIndex).Data
                            lsSQLQuery &= Richmond.returnIfTrue(lrColumn.DataTypeIsText, "'", "")
                            liInd += 1
                        Next
                        Dim lrRecordset = Me.mrModel.DatabaseConnection.GONonQuery(lsSQLQuery)
                        '=========================================================================
                        'Remove the Fact
                        Me.mrRecordset.Facts.RemoveAt(Me.AdvancedDataGridView.SelectedRows(0).Index)

                        Me.AdvancedDataGridView.DataSource = Nothing
                        Me.AdvancedDataGridView.DataSource = Me.mrDataGridList
                    End If
                End If
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AdvancedDataGridView_FilterStringChanged(sender As Object, e As EventArgs) Handles AdvancedDataGridView.FilterStringChanged

        Dim lsSQLQuery As String

        Try
            If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
            Else
                If Me.mrTable Is Nothing Then
                Else
                    lsSQLQuery = "SELECT * FROM " & mrTable.DatabaseName & vbCrLf

                    For liInd = 0 To Me.mrTable.Column.Count - 1
                        Call Me.AdvancedDataGridView.EnableFilter(Me.AdvancedDataGridView.Columns(liInd))
                    Next

                    Dim lsFilterString As String
                    lsFilterString = Me.AdvancedDataGridView.FilterString.Replace("[", "").Replace("]", "")
                    'WHERE Clause                    
                    If Trim(lsFilterString) <> "" Then
                        lsSQLQuery &= " WHERE " & lsFilterString
                    End If
                    lsSQLQuery &= vbCrLf & " LIMIT 100"

                    Me.mrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(lsSQLQuery)

                    Me.mrDataGridList = New ORMQL.RecordsetDataGridList(Me.mrRecordset, Me.mrTable)

                        Me.AdvancedDataGridView.DataSource = Me.mrDataGridList
                    End If
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