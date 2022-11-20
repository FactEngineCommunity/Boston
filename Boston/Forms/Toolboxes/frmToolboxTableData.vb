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

        Try
            Call Me.mrModel.connectToDatabase(True)

            If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
            Else
                If Me.mrTable Is Nothing Then
                Else
                    Select Case Me.mrModel.TargetDatabaseType
                        Case Is = pcenumDatabaseType.Neo4j
                            lsSQLQuery = "MATCH (" & LCase(mrTable.DatabaseName) & ":" & mrTable.DatabaseName & ")" & vbCrLf
                            Dim lsColumnList As String = String.Join(",", Me.mrTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey).Select(Function(x) LCase(mrTable.DatabaseName) & "." & x.Name))
                            lsSQLQuery &= "RETURN " & lsColumnList & ";"
                        Case Is = pcenumDatabaseType.TypeDB
                            lsSQLQuery = "match $table isa " & mrTable.DatabaseName
                            For Each lrColumn In Me.mrTable.Column
                                lsSQLQuery &= ", has " & lrColumn.Name & " $" & lrColumn.Name
                            Next
                            lsSQLQuery &= ";" & vbCrLf
                            Dim lsColumnList As String = String.Join(",", Me.mrTable.Column.Select(Function(x) "$" & x.Name))

                            lsSQLQuery &= "get " & lsColumnList & ";"
                        Case Else
                            lsSQLQuery = "SELECT * FROM " & mrTable.DatabaseName & vbCrLf
                            lsSQLQuery &= " LIMIT 100"
                    End Select

                    Me.mrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(lsSQLQuery)

                    If Me.mrRecordset.Facts.Count = 0 Then
                        Select Case Me.mrModel.TargetDatabaseType
                            Case Is = pcenumDatabaseType.Neo4j
                                Dim larColumn = Me.mrTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey)

                                For Each lrColumn In larColumn
                                    Me.mrRecordset.Columns.Add(LCase(mrTable.DatabaseName) & "." & lrColumn.Name)
                                Next

                            Case Is = pcenumDatabaseType.TypeDB
                                'N/A
                            Case Else
                                'N/A
                        End Select
                    End If

                    Me.mrDataGridList = New ORMQL.RecordsetDataGridList(Me.mrRecordset, Me.mrTable)
                    'Me.AdvancedDataGridView.DataSource = Me.mrDataGridList
                    Me.AdvancedDataGridView.DataSource = Me.mrDataGridList

                    If Me.mrRecordset.Facts.Count = 0 Then
                        Me.ButtonAddRow.Enabled = True
                    End If
                End If
            End If

            Me.ToolStripStatusLabel.Text = ""

            Me.AdvancedDataGridView.RowTemplate.Height = Me.AdvancedDataGridView.Font.Height + 8

            If Me.mrTable IsNot Nothing Then
                Me.GroupBox1.Text = "Table Name: " & Me.mrTable.Name
            End If

            'AddHandler Me.AdvancedDataGridView.RowsRemoved, AddressOf DataGridView_RowsRemoved

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub frmToolboxTableData_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub ToolStripButtonCommit_Click(sender As Object, e As EventArgs) Handles ToolStripButtonCommit.Click

        Dim lsMessage As String

        Try
            'Finalise Editing
            If Me.AdvancedDataGridView.IsCurrentCellInEditMode Then
                Call Me.AdvancedDataGridView.EndEdit()
            End If
            'Me.mrRecordset.Facts(liRowIndex)(Me.mrRecordset.Columns(liColumnIndex)).Data = Me.NewValue


            Dim larNewModifiedFacts = From Fact In Me.mrRecordset.Facts
                                      Where Fact.IsNewFact
                                      Select Fact

            Dim lsSQLQuery As String = Nothing
            Dim liInd = 0

            For Each lrFact In larNewModifiedFacts
                Select Case Me.mrModel.TargetDatabaseType
                    Case Is = pcenumDatabaseType.SQLite,
                              pcenumDatabaseType.PostgreSQL,
                              pcenumDatabaseType.SQLServer,
                              pcenumDatabaseType.ODBC
#Region "SQL"
                        lsSQLQuery = "INSERT INTO " & Me.mrTable.Name & "("

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
                            lsSQLQuery &= Me.mrTable.Model.Model.DatabaseConnection.DataTypeWrapper(larColumn(liInd).getMetamodelDataType) ' Was DataTypeIsText, "'", "")
                            Select Case larColumn(liInd).getMetamodelDataType
                                Case Is = pcenumORMDataType.TemporalDate,
                                  pcenumORMDataType.TemporalDateAndTime
                                    lsSQLQuery &= Me.mrTable.Model.Model.DatabaseConnection.FormatDateTime(lrFact.Data(liInd).Data)
                                Case Else
                                    lsSQLQuery &= lrFact.Data(liInd).Data
                            End Select

                            lsSQLQuery &= Me.mrTable.Model.Model.DatabaseConnection.DataTypeWrapper(larColumn(liInd).getMetamodelDataType) ' Was DataTypeIsText, "'", "")
                            liInd += 1
                        Next
                        lsSQLQuery &= ")"
#End Region
                    Case Is = pcenumDatabaseType.Neo4j
#Region "Cypher"
                        lsSQLQuery = "CREATE (" & LCase(Me.mrTable.Name) & ":" & Me.mrTable.Name & " {"

                        liInd = 0
                        For Each lrColumn In Me.mrTable.Column.FindAll(Function(x) Not x.FactType.IsDerived And Not x.isPartOfPrimaryKey)
                            If liInd > 0 Then lsSQLQuery &= ","
                            lsSQLQuery &= lrColumn.Name & ":"
                            lsSQLQuery &= Me.mrTable.Model.Model.DatabaseConnection.DataTypeWrapper(lrColumn.getMetamodelDataType) ' Was DataTypeIsText, "'", "")
                            Select Case lrColumn.getMetamodelDataType
                                Case Is = pcenumORMDataType.TemporalDate,
                                  pcenumORMDataType.TemporalDateAndTime
                                    lsSQLQuery &= Me.mrTable.Model.Model.DatabaseConnection.FormatDateTime(lrFact.Data(liInd).Data)
                                Case Else
                                    lsSQLQuery &= lrFact.Data(liInd).Data
                            End Select

                            lsSQLQuery &= Me.mrTable.Model.Model.DatabaseConnection.DataTypeWrapper(lrColumn.getMetamodelDataType) ' Was DataTypeIsText, "'", "")
                            liInd += 1
                        Next

                        lsSQLQuery &= "})"
#End Region
                    Case Else
                        lsMessage = "Cannot create records for the database type, " & Me.mrModel.TargetDatabaseType.ToString
                        Exit Sub
                End Select

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
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AdvancedDataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles AdvancedDataGridView.CellBeginEdit

        Me.ToolStripStatusLabel.Text = ""

        Try

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

            If Me.mrRecordset.Facts(e.RowIndex).IsNewFact Then
                Me.ToolStripButtonCommit.Enabled = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AdvancedDataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView.CellEndEdit

        Dim lsMessage As String

        Try
            Dim lsColumnName As String

            Dim lsColumn As String = Me.mrRecordset.Columns(e.ColumnIndex)

            Select Case Me.mrModel.TargetDatabaseType
                Case Is = pcenumDatabaseType.Neo4j
                    Try
                        lsColumnName = lsColumn.Substring(lsColumn.IndexOf(".") + 1)
                    Catch ex As Exception
                        lsColumnName = lsColumn
                    End Try
                Case Else
                    lsColumnName = lsColumn
            End Select

            Dim lrColumn As RDS.Column = Me.mrTable.Column.Find(Function(x) x.Name = lsColumnName)

            Select Case lrColumn.getMetamodelDataType
                Case Is = pcenumORMDataType.TemporalDate,
                          pcenumORMDataType.TemporalDateAndTime
                    Me.NewValue = Me.mrTable.Model.Model.DatabaseConnection.FormatDateTime(Me.NewValue)
                Case Else
                    'Nothing to do.
            End Select

            Dim lsOldValue = Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data
            Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data = Me.NewValue 'Delimits the Fact by its RoleName. E.g. Fact("DateTime"). Boston gets the appropriate RoleData.

            If Me.mrRecordset.Facts(e.RowIndex).IsNewFact Then Exit Sub

            Dim larPKColumn As List(Of RDS.Column)
            '= Me.mrTable.getPrimaryKeyColumns
            Select Case Me.mrModel.TargetDatabaseType
                Case Is = pcenumDatabaseType.Neo4j
                    larPKColumn = Me.mrTable.getFirstUniquenessConstraintColumns
                Case Else
                    larPKColumn = Me.mrTable.getPrimaryKeyColumns
            End Select

            If larPKColumn.Count = 0 Then
                lsMessage = "Please ensure that your table/node-type has a primary key/uniqueness constraint."
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False,, True, Nothing, True)
            End If

            Dim liColumnIndex As Integer
            Dim lsValue As String
            For Each lrPKColumn In larPKColumn
                liColumnIndex = Me.mrRecordset.Columns.IndexOf(lrPKColumn.Name)
                If lrPKColumn.Name = lsColumnName Then 'Me.mrRecordset.Columns(e.ColumnIndex) 
                    lsValue = lsOldValue
                Else
                    lsValue = Me.mrRecordset.Facts(e.RowIndex)(lsColumnName).Data 'Me.mrRecordset.Columns(liColumnIndex)
                End If

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
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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

                        Select Case Me.mrModel.TargetDatabaseType
                            Case Is = pcenumDatabaseType.Neo4j
                                lsSQLQuery = "MATCH (" & LCase(Me.mrTable.Name) & ":" & Me.mrTable.Name & " {"

                                Dim larPKColumn As List(Of RDS.Column)

                                larPKColumn = Me.mrTable.getFirstUniquenessConstraintColumns

                                Dim liInd = 0
                                For Each lrColumn In larPKColumn

                                    If liInd > 0 Then lsSQLQuery &= ", "

                                    lsSQLQuery &= lrColumn.Name & ": "

                                    Dim liValueColumnIndex = mrRecordset.Columns.IndexOf(LCase(Me.mrTable.Name) & "." & lrColumn.Name)
                                    lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsText, "'", "")
                                    lsSQLQuery &= lrFact.Data(liValueColumnIndex).Data
                                    lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsText, "'", "")
                                    liInd += 1
                                Next

                                lsSQLQuery &= "})" & vbCrLf
                                lsSQLQuery &= "DELETE " & LCase(Me.mrTable.Name)


                            Case Is = pcenumDatabaseType.TypeDB
                                Throw New NotImplementedException("Not implemented for TypeDB.")
                            Case Else
                                lsSQLQuery.AppendString("DELETE FROM " & Me.mrTable.Name & vbCrLf & " WHERE ")
                                Dim liInd = 0
                                For Each lrColumn In Me.mrTable.getPrimaryKeyColumns
                                    If liInd > 0 Then lsSQLQuery &= " AND "
                                    lsSQLQuery.AppendString(lrColumn.Name & " = ")
                                    Dim liValueColumnIndex = mrRecordset.Columns.IndexOf(lrColumn.Name)
                                    lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsText, "'", "")
                                    lsSQLQuery &= lrFact.Data(liValueColumnIndex).Data
                                    lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsText, "'", "")
                                    liInd += 1
                                Next
                        End Select

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonAddRow_Click(sender As Object, e As EventArgs) Handles ButtonAddRow.Click

        Try
            Dim lrDummyFactType As New FBM.FactType(Me.mrModel, "DummyFactType", True)
            Dim lrFact = New FBM.Fact(lrDummyFactType, False)
            lrFact.Id = System.Guid.NewGuid.ToString
            lrFact.IsNewFact = True

            Dim liInd As Integer = 0
            Dim larColumn As List(Of RDS.Column)

            Select Case Me.mrModel.TargetDatabaseType
                Case Is = pcenumDatabaseType.Neo4j
                    larColumn = Me.mrTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey)
                Case Else
                    larColumn = Me.mrTable.Column.ToList
            End Select

            For Each lrColumn In larColumn

                Select Case Me.mrModel.TargetDatabaseType
                    Case Is = pcenumDatabaseType.Neo4j
                        lrColumn.TemporaryData = LCase(Me.mrTable.Name) & "." & lrColumn.Name
                    Case Else
                        lrColumn.TemporaryData = lrColumn.Name
                End Select

                If lrColumn IsNot Nothing Then
                    Dim lrRole = New FBM.Role(lrDummyFactType, lrColumn.TemporaryData, True, Nothing)
                    lrDummyFactType.RoleGroup.AddUnique(lrRole)
                    Dim lrFactData = New FBM.FactData(lrRole, New FBM.Concept(""), lrFact)
                    lrFactData.setData("", pcenumConceptType.Value, False)
                    lrFact.Data.Add(lrFactData)
                End If
            Next

            Me.mrRecordset.Facts.Add(lrFact)

            Me.mrDataGridList = New ORMQL.RecordsetDataGridList(Me.mrRecordset, Me.mrTable)

            Me.AdvancedDataGridView.DataSource = Me.mrDataGridList

            Me.ButtonAddRow.Enabled = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                        For liInd2 = 0 To Me.AdvancedDataGridView.Columns.Count - 1
                            If Me.AdvancedDataGridView.Columns(liInd2).Name = Me.mrTable.Column(liInd).Name Then
                                Call Me.AdvancedDataGridView.EnableFilter(Me.AdvancedDataGridView.Columns(liInd2))
                            End If
                        Next
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub
End Class