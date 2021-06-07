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

        Call Me.SetupForm

    End Sub

    Public Sub SetupForm()

        Dim lsSQLQuery As String

        If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
        Else
            If Me.mrTable Is Nothing Then
            Else
                lsSQLQuery = "SELECT * FROM " & mrTable.Name
                Me.mrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(lsSQLQuery)

                Me.mrDataGridList = New ORMQL.RecordsetDataGridList(Me.mrRecordset, Me.mrTable)
                Me.DataGridView.DataSource = Me.mrDataGridList

            End If
        End If

        Me.ToolStripStatusLabel.Text = ""

        Me.DataGridView.RowTemplate.Height = 16

    End Sub

    Private Sub frmToolboxTableData_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub DataGridView_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView.CellEndEdit

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

        End Try

    End Sub

    Private Sub DataGridView_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView.CellBeginEdit

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
            Debugger.Break()
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
                Me.DataGridView.DataSource = Nothing
                Me.DataGridView.DataSource = Me.mrDataGridList
            End If

            Me.ToolStripButtonUndo.Enabled = False
            Me.ToolStripButtonCommit.Enabled = False
        Catch ex As Exception
            Debugger.Break()
        End Try

    End Sub

    Private Sub DataGridView_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DataGridView.CellValidating

        Me.NewValue = e.FormattedValue

    End Sub
End Class