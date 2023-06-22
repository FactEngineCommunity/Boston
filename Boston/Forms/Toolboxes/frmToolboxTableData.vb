Imports System.Reflection

Public Class frmToolboxTableData

    Public mrModel As FBM.Model
    Public mrTable As RDS.Table = Nothing 'For when editing data of a Table/Node/Entity.
    Public mrFactType As FBM.FactType = Nothing 'For when editing data of a link between Tables/Nodes/Entities
    Public mrRDSRelation As RDS.Relation = Nothing
    Private mrRecordset As ORMQL.Recordset
    Private mrDataGridList As ORMQL.RecordsetDataGridList

    'For Cell text editting
    Private OldValue, NewValue As String

    'For Filtering Columns
    Private msFilterString As String = ""

    Public Function EqualsByName(ByVal other As Form) As Boolean
        Return Me.Name = other.Name
    End Function

    Private Sub frmToolboxTableData_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Public Sub SetupForm()

        'RemoveHandler Me.AdvancedDataGridView.RowsRemoved, AddressOf DataGridView_RowsRemoved

        Dim lsSQLQuery As String = ""

        Try
            Call Me.mrModel.connectToDatabase(True)

            If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
            Else
                If Me.mrTable IsNot Nothing Then

                    Dim larColumn As New List(Of RDS.Column) 'Needed if a virtual table is needed for a Neo4j/Graph Edge Type;

                    Select Case Me.mrModel.TargetDatabaseType
                        Case Is = pcenumDatabaseType.Neo4j,
                                  pcenumDatabaseType.KuzuDB
                            If Me.mrTable.isPGSRelation Then
                                Dim larRelation = mrTable.getRelations
                                If larRelation.Count > 2 Then
                                Else
                                    'Example:
                                    'MATCH (person:Person)-[:DRIVES]->(carType:CarType)
                                    'Return ID(person) As personId, person.FirstName As firstName, person.LastName As lastName, ID(carType) As carTypeId, carType.CarTypeName As carTypeName
                                    lsSQLQuery = "MATCH "
                                    lsSQLQuery &= "(" & LCase(larRelation(0).DestinationTable.Name) & ":" & larRelation(0).DestinationTable.Name & ")"
                                    lsSQLQuery &= "-[:" & Me.mrTable.DBName & "]-"
                                    lsSQLQuery &= "(" & LCase(larRelation(1).DestinationTable.Name) & ":" & larRelation(1).DestinationTable.Name & ")" & vbCrLf
                                    lsSQLQuery &= "RETURN "
                                    Dim lsColumnList As String = String.Join(" + ' ' + ", larRelation(0).DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) LCase(larRelation(0).DestinationTable.Name) & "." & x.Name))
                                    lsColumnList &= ", " & String.Join(" + ' ' + ", larRelation(1).DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) LCase(larRelation(1).DestinationTable.Name) & "." & x.Name))
                                    larColumn.AddRange(larRelation(0).DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) x.Clone(Nothing, Nothing)).ToList())
                                    larColumn.AddRange(larRelation(1).DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) x.Clone(Nothing, Nothing)).ToList())
                                    lsSQLQuery &= lsColumnList & vbCrLf & "LIMIT 100;"
                                End If
                            Else
                                lsSQLQuery = "MATCH (" & LCase(mrTable.DatabaseName) & ":" & mrTable.DatabaseName & ")" & vbCrLf
                                '20230525-VM-Move on this. Make Model store Setting ShowPrimaryKeyColumns in TableDataEditForm...then check on isPartOfPrimaryKey accordingly.
                                '  The reason why is that if we are creating the table then we might not want to see that data. But if is a database like Northwind NEO4j, we might want to see OrderId for instance.
                                Dim lsColumnList As String = String.Join(",", Me.mrTable.Column.FindAll(Function(x) Not (x.isPartOfPrimaryKey Or x.isForeignKey)).Select(Function(x) LCase(mrTable.DatabaseName) & "." & x.DBName))
                                lsSQLQuery &= "RETURN " & lsColumnList & ";"
                            End If
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

                    If Me.mrTable Is Nothing Then
                        Call Me.PopulateDataGridFromDatabaseQuery(lsSQLQuery,, larColumn)
                    Else
                        If Me.mrTable.isPGSRelation Then
                            Call Me.PopulateDataGridFromDatabaseQuery(lsSQLQuery,, larColumn)
                        Else
                            Call Me.PopulateDataGridFromDatabaseQuery(lsSQLQuery, Me.mrTable)
                        End If

                    End If


                ElseIf Me.mrFactType IsNot Nothing Then

                    Dim lrRDSRelation As RDS.Relation = Nothing

                    Try
                        lrRDSRelation = (From Relation In Me.mrModel.RDS.Relation
                                         Where Relation.ResponsibleFactType.Id = Me.mrFactType.Id
                                         Select Relation).First

                    Catch ex As Exception
                        Throw New Exception("No relation found for the Fact Type, " & Me.mrFactType.Id)
                    End Try

                    Dim larColumn As New List(Of RDS.Column)

                    Select Case Me.mrModel.TargetDatabaseType
                        Case Is = pcenumDatabaseType.Neo4j,
                                  pcenumDatabaseType.KuzuDB
                            Dim lsFirstColumnName As String = String.Join("", lrRDSRelation.OriginTable.getFirstUniquenessConstraintColumns.Select(Function(x) x.Name))
                            Dim lsSecondColumnName As String = String.Join("", lrRDSRelation.DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) x.Name))

                            larColumn.Add(New RDS.Column(lrRDSRelation.OriginTable, lsFirstColumnName, Nothing, Nothing, False, Nothing))
                            larColumn.Add(New RDS.Column(lrRDSRelation.DestinationTable, lsSecondColumnName, Nothing, Nothing, False, Nothing))

                            lsSQLQuery = "MATCH "
                            lsSQLQuery &= "(" & LCase(lrRDSRelation.OriginTable.Name) & ":" & lrRDSRelation.OriginTable.Name & ")"
                            lsSQLQuery &= "-[:" & Me.mrFactType.DBName & "]-"
                            lsSQLQuery &= "(" & LCase(lrRDSRelation.DestinationTable.Name) & ":" & lrRDSRelation.DestinationTable.Name & ")" & vbCrLf
                            lsSQLQuery &= "RETURN "
                            lsFirstColumnName = String.Join(" + ' ' + ", lrRDSRelation.OriginTable.getFirstUniquenessConstraintColumns.Select(Function(x) LCase(lrRDSRelation.OriginTable.Name) & "." & x.Name))
                            lsSecondColumnName = String.Join(" + ' ' + ", lrRDSRelation.DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) LCase(lrRDSRelation.DestinationTable.Name) & "." & x.Name))
                            Dim lsColumnList = lsFirstColumnName & ", " & lsSecondColumnName
                            lsSQLQuery &= lsColumnList & ";"

                        Case Is = pcenumDatabaseType.TypeDB
                            'lsSQLQuery = "match $table isa " & mrTable.DatabaseName
                            'For Each lrColumn In Me.mrTable.Column
                            '    lsSQLQuery &= ", has " & lrColumn.Name & " $" & lrColumn.Name
                            'Next
                            'lsSQLQuery &= ";" & vbCrLf
                            'Dim lsColumnList As String = String.Join(",", Me.mrTable.Column.Select(Function(x) "$" & x.Name))

                            'lsSQLQuery &= "get " & lsColumnList & ";"
                        Case Else
                            larColumn.AddRange(lrRDSRelation.OriginTable.getFirstUniquenessConstraintColumns.ConvertAll(Function(x) x.Clone(Nothing, Nothing)))
                            larColumn.AddRange(lrRDSRelation.DestinationTable.getFirstUniquenessConstraintColumns.ConvertAll(Function(x) x.Clone(Nothing, Nothing)))
                            lsSQLQuery = "SELECT "
                            Dim liInd = 0
                            For Each lrColumn In larColumn
                                If liInd > 0 Then lsSQLQuery &= ", "
                                lsSQLQuery &= lrColumn.Table.DatabaseName & "." & lrColumn.Name
                                liInd += 1
                            Next
                            lsSQLQuery.AppendLine("FROM " & lrRDSRelation.OriginTable.DatabaseName & vbCrLf & ", " & lrRDSRelation.DestinationTable.DatabaseName)
                            lsSQLQuery.AppendLine(" WHERE ")
                            For liInd = 0 To lrRDSRelation.OriginColumns.Count - 1
                                If liInd > 0 Then lsSQLQuery.AppendLine("AND ")
                                lsSQLQuery &= lrRDSRelation.OriginTable.DatabaseName & "." & lrRDSRelation.OriginColumns(liInd).Name
                                lsSQLQuery &= " = " & lrRDSRelation.DestinationTable.DatabaseName & "." & lrRDSRelation.DestinationColumns(liInd).Name
                            Next
                            lsSQLQuery.AppendLine(" LIMIT 100")
                    End Select

                    Dim lrTable As New RDS.Table(Me.mrModel.RDS, "DummyTable", Nothing)
                    lrTable.Column.AddRange(larColumn)

                    If Me.mrTable IsNot Nothing Then
                        Call Me.PopulateDataGridFromDatabaseQuery(lsSQLQuery, lrTable)
                    Else
                        Call Me.PopulateDataGridFromDatabaseQuery(lsSQLQuery, Nothing, larColumn)
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="asDatabaseQuery"></param>
    ''' <param name="arTable"></param>
    ''' <param name="aarColumn">If for an Edge Type a virtual table needs to be created, with Unique Identifiers (e.g. FirstName + ' ' + LastName) rather than the Many-2-Many Foreign Key reference Ids.</param>
    Private Sub PopulateDataGridFromDatabaseQuery(ByVal asDatabaseQuery As String,
                                                  Optional arTable As RDS.Table = Nothing,
                                                  Optional aarColumn As List(Of RDS.Column) = Nothing)

        Try
            Me.mrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(asDatabaseQuery)

            Dim lrTable As RDS.Table
            If arTable Is Nothing And aarColumn IsNot Nothing Then
                lrTable = New RDS.Table(Me.mrModel.RDS, "DummyEdgeTable", Nothing)
                lrTable.Column = aarColumn
            Else
                lrTable = arTable.Clone
                lrTable.Model = arTable.Model
                Select Case Me.mrModel.TargetDatabaseType
                    Case Is = pcenumDatabaseType.KuzuDB
                        Dim lrPKColumn As RDS.Column = lrTable.getPrimaryKeyColumns(0)
                        lrTable.Column.RemoveAt(lrTable.Column.IndexOf(lrPKColumn))
                End Select
            End If

            If aarColumn IsNot Nothing Then
                Dim liInd = 0
                For Each lrColumn In aarColumn
                    Me.mrRecordset.Columns(liInd) = lrColumn.Name
                    liInd += 1
                Next
            End If

            If Me.mrRecordset.Facts.Count = 0 And Me.mrTable IsNot Nothing Then
                Select Case Me.mrModel.TargetDatabaseType
                    Case Is = pcenumDatabaseType.Neo4j,
                              pcenumDatabaseType.KuzuDB
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

            Me.mrDataGridList = New ORMQL.RecordsetDataGridList(Me.mrRecordset, lrTable)
            Me.AdvancedDataGridView.DataSource = Me.mrDataGridList

            If Me.mrRecordset.Facts.Count = 0 Then
                Me.ButtonAddRow.Enabled = True
            End If

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
                        lsSQLQuery = "INSERT INTO [" & Me.mrTable.Name & "] ("

                        Dim lrColumn As RDS.Column
                        Dim larColumn As New List(Of RDS.Column)
                        liInd = 0
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
                    Case Is = pcenumDatabaseType.Neo4j,
                              pcenumDatabaseType.KuzuDB
#Region "Cypher"
                        If Me.mrTable.isPGSRelation Then
#Region "PGS Relation - Propery Graph Schema Relation, Table is relation, rather than a Foreign Key Relationship"
                            '=======================================================================================
                            'Example
                            'MATCH(person: Person)
                            'WHERE person.FirstName = {firstName} And person.LastName = {lastName}
                            'With person
                            'MATCH(carType: CarType)
                            'WHERE carType.CarTypeName = {carTypeName}
                            'MATCH (person:Person)-[:DRIVES]->(carType:CarType)
                            'Return ID(person) As personId, person.FirstName As firstName, person.LastName As lastName, ID(carType) As carTypeId, carType.CarTypeName As carTypeName

                            'Later we'll use this for CREATE as well.
                            'CREATE(person)-[:DRIVES]->(carType)
                            '=====================================================================================
                            Dim larRelation = mrTable.getRelations

                            lsSQLQuery = "MATCH "
                            lsSQLQuery &= "(" & LCase(larRelation(0).DestinationTable.Name) & ":" & larRelation(0).DestinationTable.Name & ")" & vbCrLf
                            lsSQLQuery &= "MATCH "
                            lsSQLQuery &= "(" & LCase(larRelation(1).DestinationTable.Name) & ":" & larRelation(1).DestinationTable.Name & ")" & vbCrLf

                            '================WHERE===================                                      
                            lsSQLQuery.AppendLine(" WHERE ")
                            liInd = 0
                            For Each lrColumn In Me.mrDataGridList.mrTable.Column
                                If liInd > 0 Then lsSQLQuery.AppendLine("AND ")
                                lsSQLQuery &= LCase(lrColumn.Table.Name) & "." & lrColumn.Name & " = "
                                lsSQLQuery &= Me.mrTable.Model.Model.DatabaseConnection.DataTypeWrapper(lrColumn.getMetamodelDataType)
                                lsSQLQuery &= lrFact.Data(liInd).Data
                                lsSQLQuery &= Me.mrTable.Model.Model.DatabaseConnection.DataTypeWrapper(lrColumn.getMetamodelDataType)
                                liInd += 1
                            Next

                            '==CREATE======================================
                            lsSQLQuery.AppendLine("CREATE (")
                            lsSQLQuery &= LCase(larRelation(0).DestinationTable.Name) & ")-[:" & Me.mrTable.DBName & "]->(" & LCase(larRelation(1).DestinationTable.Name) & ")"
#End Region
                        Else
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
                        End If
#End Region
                    Case Else
                        lsMessage = "Cannot create records for the database type, " & Me.mrModel.TargetDatabaseType.ToString
                        Exit Sub
                End Select

                Dim lrRecordset = Me.mrModel.DatabaseConnection.GONonQuery(lsSQLQuery)

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
        Dim lsDatabaseQuery As String = ""

        Try
            If Me.mrRDSRelation IsNot Nothing Then
                'Create ComboBox Cell
                Dim cmbcell As New DataGridViewComboBoxCell
                If e.ColumnIndex = 0 Then
                Else
                    Select Case Me.mrModel.TargetDatabaseType
                        Case Is = pcenumDatabaseType.Neo4j
                        Case Is = pcenumDatabaseType.TypeDB
                        Case Else
                            lsDatabaseQuery = "SELECT "
                            Dim liInd = 0
                            Dim larPKColumn = Me.mrRDSRelation.DestinationTable.getPrimaryKeyColumns
                            If larPKColumn.Count > 1 Then lsDatabaseQuery &= "{"
                            For Each lrColumn In larPKColumn
                                If liInd > 0 Then lsDatabaseQuery &= ","
                                lsDatabaseQuery &= lrColumn.Table.DatabaseName & "." & lrColumn.Name
                                liInd += 1
                            Next
                            If larPKColumn.Count > 1 Then lsDatabaseQuery &= "}"
                            lsDatabaseQuery &= " AS ItemData, "
                            Dim larUCColumn = Me.mrRDSRelation.DestinationTable.getFirstUniquenessConstraintColumns
                            If larUCColumn.Count > 1 Then lsDatabaseQuery &= "{"
                            liInd = 0
                            For Each lrColumn In larUCColumn
                                If liInd > 0 Then lsDatabaseQuery &= ","
                                lsDatabaseQuery &= lrColumn.Table.DatabaseName & "." & lrColumn.Name
                                liInd += 1
                            Next
                            If larUCColumn.Count > 1 Then lsDatabaseQuery &= "}"
                            lsDatabaseQuery &= " AS Text"
                            lsDatabaseQuery.AppendLine("FROM " & Me.mrRDSRelation.DestinationTable.DatabaseName)
                            lsDatabaseQuery.AppendLine("LIMIT 100")
                    End Select
                End If
                Dim lrTable As New RDS.Table(Me.mrModel.RDS, "DummyTable", Nothing)
                lrTable.Column.Add(New RDS.Column(lrTable, "ItemData", Nothing, Nothing, True))
                lrTable.Column.Add(New RDS.Column(lrTable, "Text", Nothing, Nothing, True))
                Dim lrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(lsDatabaseQuery)
                Dim lrDataGridList = New ORMQL.RecordsetDataGridList(lrRecordset, lrTable)

                cmbcell.DataSource = lrDataGridList
                cmbcell.DisplayMember = "Text"
                cmbcell.ValueMember = "Text"

                Me.AdvancedDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex) = cmbcell

                'Dim normalcell As DataGridViewCell = New DataGridViewTextBoxCell()
                'normalcell.Value = "Name"
                'dataGridView1.Rows(iRowIndex).Cells(1) = normalcell

            End If


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

    ''' <summary>
    ''' NB If we have allowed column reordering in the DataGridView, the displayed column order may differ from the actual column index.
    ''' In such cases, you can use the DisplayIndex property instead of ColumnIndex to retrieve the correct column index based on the current display order.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AdvancedDataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles AdvancedDataGridView.CellEndEdit

        Dim lsMessage As String

        Try
            Dim lsColumnName As String

#Region "Get the Column"
            Dim liIndex = Me.AdvancedDataGridView.Columns(e.ColumnIndex).DisplayIndex
            Dim lsColumn As String = Me.mrRecordset.Columns(liIndex) 'e.ColumnIndex)

            Select Case Me.mrModel.TargetDatabaseType
                Case Is = pcenumDatabaseType.Neo4j,
                          pcenumDatabaseType.KuzuDB
                    Try
                        lsColumnName = lsColumn.Substring(lsColumn.IndexOf(".") + 1)
                    Catch ex As Exception
                        lsColumnName = lsColumn
                    End Try
                Case Else
                    lsColumnName = lsColumn
            End Select

            Dim lrTable As RDS.Table = Me.mrTable

            If Me.mrTable.isPGSRelation Then
                'Dummy Table used for PGS Relations, especially for Neo4J (Because show Unique Key values, rather than PK Values on many-to-many tables etc.
                lrTable = Me.mrDataGridList.mrTable
            End If

            Dim lrColumn As RDS.Column = lrTable.Column.Find(Function(x) x.Name = lsColumnName)

            If lrColumn Is Nothing Then
                lsMessage = "A column with the name, " & lsColumnName & ", does not exist in the Entity, " & Me.mrTable.Name & ", in the Model you are working on."
                Me.ToolStripStatusLabel.Text = lsMessage
                Me.ToolStripStatusLabel.ForeColor = Color.Red
                Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(liIndex)).Data = Me.OldValue 'Was e.ColumnIndex
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False,, True,, True)
                Exit Sub
            End If
#End Region

#Region "Get the New Value"
            Select Case lrColumn.getMetamodelDataType
                Case Is = pcenumORMDataType.TemporalDate,
                              pcenumORMDataType.TemporalDateAndTime
                    Me.NewValue = Me.mrTable.Model.Model.DatabaseConnection.FormatDateTime(Me.NewValue)
                Case Else
                    'Nothing to do.
            End Select
#End Region

            Dim lsOldValue = Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(liIndex)).Data 'Was e.ColumnIndex

            '------------------------------------------------
            'CodeSafe - Exit if no change
            If Me.NewValue = lsOldValue Then Exit Sub

            '------------------------------------------------
            'Get a clone of the Fact for PGSRelations
            Dim lrOriginalFact As FBM.Fact = Me.mrRecordset.Facts(e.RowIndex).Clone(New FBM.FactType(Me.mrModel, "DummyFactType", True), True)

            '==========================================================================================
            'Update the FactData for the Fact
            Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(liIndex)).Data = Me.NewValue 'Was e.ColumnIndex 'Delimits the Fact by its RoleName. E.g. Fact("DateTime"). Boston gets the appropriate RoleData.
            '==========================================================================================

            '============================================================
            'No need to continue if is new Record/Row.
            If Me.mrRecordset.Facts(e.RowIndex).IsNewFact Then Exit Sub

            If Me.mrRDSRelation Is Nothing And Not Me.mrTable.isPGSRelation Then
#Region "Table - Straight/Standard Table"
                '---------------------------------------------
                'Straight Table. I.e. mrTable isnot Nothing.

                Dim larPKColumn As List(Of RDS.Column)
                '= Me.mrTable.getPrimaryKeyColumns
                Select Case Me.mrModel.TargetDatabaseType
                    Case Is = pcenumDatabaseType.Neo4j,
                              pcenumDatabaseType.KuzuDB
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
                        lsValue = Me.mrRecordset.Facts(e.RowIndex)(lrPKColumn.Name).Data 'Me.mrRecordset.Columns(liColumnIndex)
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
#End Region
            ElseIf Me.mrTable.isPGSRelation Then
#Region "PGS Relation - Property Graph Schema Relation Table, as opposed Foreign Key Relationship."
                'Special handling of a PGS Relation, especially if is a Neo4j database

                If Me.AllDataGridCulumnsPopulatedByRow(e.RowIndex) Then
                    Debugger.Break()
#Region "Create Boilerplate"
                    Dim larRelation = mrTable.getRelations

                    Dim lsQuery As String = ""
                    Dim lsMatchClause As String = ""
                    Dim lsWhereClause As String = ""

                    '================WHERE===================                                      
                    lsWhereClause = " WHERE "
                    Dim liInd = 0
                    For Each lrColumn In Me.mrDataGridList.mrTable.Column
                        If liInd > 0 Then lsWhereClause.AppendLine("AND ")
                        lsWhereClause &= LCase(lrColumn.Table.Name) & "." & lrColumn.Name & " = "
                        lsWhereClause &= Me.mrTable.Model.Model.DatabaseConnection.DataTypeWrapper(lrColumn.getMetamodelDataType)
                        lsWhereClause &= "{" & liInd.ToString & "}" 'lrFact.Data(liInd).Data
                        lsWhereClause &= Me.mrTable.Model.Model.DatabaseConnection.DataTypeWrapper(lrColumn.getMetamodelDataType)
                        liInd += 1
                    Next
#End Region


#Region "Delete Existing Relationship"

                    lsMatchClause = "MATCH "
                    lsMatchClause &= "(" & LCase(larRelation(0).DestinationTable.Name) & ":" & larRelation(0).DestinationTable.Name & ")"
                    lsMatchClause.AppendString("-[r:" & Me.mrTable.DBName & "]-")
                    lsMatchClause &= "(" & LCase(larRelation(1).DestinationTable.Name) & ":" & larRelation(1).DestinationTable.Name & ")" & vbCrLf

                    lsQuery.AppendString(lsMatchClause)
                    Dim lsDeleteWhereClause = String.Format(lsWhereClause, lrOriginalFact.Data(0).Data, lrOriginalFact.Data(1).Data)
                    lsQuery.AppendLine(lsDeleteWhereClause)

                    '==DELETE======================================
                    lsQuery.AppendLine("DELETE r")

                    Dim lrRecordset = Me.mrModel.DatabaseConnection.GONonQuery(lsQuery)
#End Region

#Region "Create New Relationship"
                    lsQuery = ""

                    lsMatchClause = "MATCH "
                    lsMatchClause &= "(" & LCase(larRelation(0).DestinationTable.Name) & ":" & larRelation(0).DestinationTable.Name & ")" & vbCrLf
                    lsMatchClause &= "MATCH "
                    lsMatchClause &= "(" & LCase(larRelation(1).DestinationTable.Name) & ":" & larRelation(1).DestinationTable.Name & ")" & vbCrLf


                    lsQuery.AppendString(lsMatchClause)
                    Dim lsCreateWhereClause = String.Format(lsWhereClause, Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(0)).Data, Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(1)).Data)
                    lsQuery.AppendLine(lsCreateWhereClause)

                    '==CREATE======================================
                    lsQuery.AppendLine("CREATE (")
                    lsQuery &= LCase(larRelation(0).DestinationTable.Name) & ")-[:" & Me.mrTable.DBName & "]->(" & LCase(larRelation(1).DestinationTable.Name) & ")"

                    lrRecordset = Me.mrModel.DatabaseConnection.GONonQuery(lsQuery)
#End Region
                Else
                    Exit Sub
                End If
#End Region
            Else
#Region "RDS Relation"
                'RDS Relation
                '20230523-VM-I don't understand this code. Need to look at it more.
                '  E.g. Why would e.ColumnIndex have to be 1?
                Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data = Me.NewValue

                If e.ColumnIndex = 1 Then

                    Dim lsDatabaseQuery As String = ""

                    Dim larPKColumn As List(Of RDS.Column)

                    Select Case Me.mrModel.TargetDatabaseType
                        Case Is = pcenumDatabaseType.Neo4j,
                                  pcenumDatabaseType.KuzuDB

                            larPKColumn = Me.mrRDSRelation.OriginTable.getFirstUniquenessConstraintColumns
                        Case Else
                            larPKColumn = Me.mrRDSRelation.OriginTable.getPrimaryKeyColumns
                    End Select

                    If larPKColumn.Count = 0 Then
                        lsMessage = "Please ensure that your table/node-type has a primary key/uniqueness constraint."
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False,, True, Nothing, True)
                    End If

                    If Me.mrRDSRelation.OriginTable.getPrimaryKeyColumns.Count = 1 Then

                        lsDatabaseQuery = "SELECT "
                        Dim liInd = 0
                        For Each lrColumn In Me.mrRDSRelation.OriginTable.getPrimaryKeyColumns
                            If liInd > 0 Then lsDatabaseQuery &= ","
                            lsDatabaseQuery.AppendString(lrColumn.Table.DatabaseName & "." & lrColumn.Name)
                            liInd += 1
                        Next
                        lsDatabaseQuery.AppendLine("FROM " & Me.mrRDSRelation.OriginTable.DatabaseName)
                        lsDatabaseQuery.AppendLine("WHERE ")
                        lsDatabaseQuery.AppendString(Me.mrRDSRelation.OriginTable.DatabaseName & "." & Me.mrRDSRelation.OriginTable.getFirstUniquenessConstraintColumns(0).Name)
                        lsDatabaseQuery.AppendString(" = ")
                        lsDatabaseQuery.AppendString(Boston.returnIfTrue(Me.mrRDSRelation.OriginTable.getFirstUniquenessConstraintColumns(0).DataTypeIsNumeric, "", "'"))
                        lsDatabaseQuery.AppendString(Database.MakeStringSafe(Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(0)).Data))
                        lsDatabaseQuery.AppendString(Boston.returnIfTrue(Me.mrRDSRelation.OriginTable.getFirstUniquenessConstraintColumns(0).DataTypeIsNumeric, "", "'"))


                        Dim lsValue As String = Me.mrModel.DatabaseConnection.GO(lsDatabaseQuery).Facts(0)(Me.mrRDSRelation.OriginTable.getPrimaryKeyColumns(0).Name).Data

                        larPKColumn(0).TemporaryData = lsValue
                    End If

                    If Me.mrRDSRelation.DestinationTable.getPrimaryKeyColumns.Count = 1 Then

                        lsDatabaseQuery = "SELECT "
                        Dim liInd = 0
                        For Each lrColumn In Me.mrRDSRelation.DestinationTable.getPrimaryKeyColumns
                            If liInd > 0 Then lsDatabaseQuery &= ","
                            lsDatabaseQuery.AppendString(lrColumn.Table.DatabaseName & "." & lrColumn.Name)
                            liInd += 1
                        Next
                        lsDatabaseQuery.AppendLine("FROM " & Me.mrRDSRelation.DestinationTable.DatabaseName)
                        lsDatabaseQuery.AppendLine("WHERE ")
                        lsDatabaseQuery.AppendString(Me.mrRDSRelation.DestinationTable.DatabaseName & "." & Me.mrRDSRelation.DestinationTable.getFirstUniquenessConstraintColumns(0).Name)
                        lsDatabaseQuery.AppendString(" = ")
                        lsDatabaseQuery.AppendString(Boston.returnIfTrue(Me.mrRDSRelation.DestinationTable.getFirstUniquenessConstraintColumns(0).DataTypeIsNumeric, "", "'"))
                        lsDatabaseQuery.AppendString(Database.MakeStringSafe(Me.NewValue))
                        lsDatabaseQuery.AppendString(Boston.returnIfTrue(Me.mrRDSRelation.DestinationTable.getFirstUniquenessConstraintColumns(0).DataTypeIsNumeric, "", "'"))
                    End If

                    Dim lsNewValue = Me.mrModel.DatabaseConnection.GO(lsDatabaseQuery).Facts(0)(Me.mrRDSRelation.DestinationTable.getPrimaryKeyColumns(0).Name).Data

                    Dim lrRecordset = Me.mrModel.DatabaseConnection.UpdateAttributeValue(Me.mrRDSRelation.OriginTable.DatabaseName,
                                                                                         Me.mrRDSRelation.OriginColumns(0),
                                                                                         lsNewValue,
                                                                                         larPKColumn)
                End If
#End Region
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
                            Case Is = pcenumDatabaseType.Neo4j,
                                      pcenumDatabaseType.KuzuDB
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
                Case Is = pcenumDatabaseType.Neo4j,
                          pcenumDatabaseType.KuzuDB
                    If Me.mrTable.isPGSRelation Then
                        larColumn = Me.mrDataGridList.mrTable.Column
                    Else
                        larColumn = Me.mrTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey)
                    End If

                Case Else
                    larColumn = Me.mrTable.Column.ToList
            End Select

            For Each lrColumn In larColumn

                Select Case Me.mrModel.TargetDatabaseType
                    Case Is = pcenumDatabaseType.Neo4j,
                              pcenumDatabaseType.KuzuDB

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

        Dim lsSQLQuery As String = ""

        Try
            If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
            Else
                Dim larColumn As List(Of RDS.Column) = Nothing

                If Me.mrTable Is Nothing Then
                Else

                    Select Case Me.mrModel.TargetDatabaseType
                        Case Is = pcenumDatabaseType.Neo4j,
                                  pcenumDatabaseType.KuzuDB

                            If Me.mrTable.isPGSRelation Then
                                '=======================================================================================
                                'Example
                                'MATCH(person: Person)
                                'WHERE person.FirstName = {firstName} And person.LastName = {lastName}
                                'With person
                                'MATCH(carType: CarType)
                                'WHERE carType.CarTypeName = {carTypeName}
                                'MATCH (person:Person)-[:DRIVES]->(carType:CarType)
                                'Return ID(person) As personId, person.FirstName As firstName, person.LastName As lastName, ID(carType) As carTypeId, carType.CarTypeName As carTypeName

                                'Later we'll use this for CREATE as well.
                                'CREATE(person)-[:DRIVES]->(carType)
                                '=====================================================================================
                                Dim larRelation = mrTable.getRelations

                                lsSQLQuery = "MATCH "
                                lsSQLQuery &= "(" & LCase(larRelation(0).DestinationTable.Name) & ":" & larRelation(0).DestinationTable.Name & ")" & vbCrLf
                                lsSQLQuery &= "MATCH "
                                lsSQLQuery &= "(" & LCase(larRelation(1).DestinationTable.Name) & ":" & larRelation(1).DestinationTable.Name & ")" & vbCrLf

                                '================WHERE===================
                                For liInd = 0 To Me.mrDataGridList.mrTable.Column.Count - 1
                                    For liInd2 = 0 To Me.AdvancedDataGridView.Columns.Count - 1
                                        If Me.AdvancedDataGridView.Columns(liInd2).Name = Me.mrTable.Column(liInd).Name Then
                                            Call Me.AdvancedDataGridView.EnableFilter(Me.AdvancedDataGridView.Columns(liInd2))
                                        End If
                                    Next
                                Next

                                Dim lsFilterString As String = Me.AdvancedDataGridView.FilterString

                                For Each lrColumn In Me.mrDataGridList.mrTable.Column
                                    lsFilterString = lsFilterString.Replace("[" & lrColumn.Name & "]", LCase(lrColumn.Table.Name) & "." & lrColumn.Name)
                                Next

                                'lsFilterString = Me.AdvancedDataGridView.FilterString.Replace("[", "").Replace("]", "")
                                lsFilterString = lsFilterString.Replace("(", "[").Replace(")", "]")
                                lsFilterString = lsFilterString.Replace("%", ".*")
                                lsFilterString = lsFilterString.Replace("LIKE", "=~")
                                Try
                                    lsFilterString = lsFilterString.Substring(1, lsFilterString.Length - 2)
                                Catch ex As Exception
                                    'Not a biggie at this stage.
                                End Try

                                'WHERE Clause                    
                                If Trim(lsFilterString) <> "" Then
                                    lsSQLQuery &= " WHERE " & lsFilterString
                                End If
                                '========================================

                                lsSQLQuery.AppendLine("MATCH ")
                                lsSQLQuery &= "(" & LCase(larRelation(0).DestinationTable.Name) & ":" & larRelation(0).DestinationTable.Name & ")"
                                lsSQLQuery &= "-[:" & Me.mrTable.DBName & "]-"
                                lsSQLQuery &= "(" & LCase(larRelation(1).DestinationTable.Name) & ":" & larRelation(1).DestinationTable.Name & ")" & vbCrLf
                                lsSQLQuery.AppendLine("RETURN ")
                                Dim lsColumnList As String = String.Join(" + ' ' + ", larRelation(0).DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) LCase(larRelation(0).DestinationTable.Name) & "." & x.Name))
                                lsColumnList &= ", " & String.Join(" + ' ' + ", larRelation(1).DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) LCase(larRelation(1).DestinationTable.Name) & "." & x.Name))
                                larColumn = New List(Of RDS.Column)
                                larColumn.AddRange(larRelation(0).DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) x.Clone(Nothing, Nothing)).ToList())
                                larColumn.AddRange(larRelation(1).DestinationTable.getFirstUniquenessConstraintColumns.Select(Function(x) x.Clone(Nothing, Nothing)).ToList())
                                lsSQLQuery &= lsColumnList & ";"


                            Else
                                'Normal Table/NodeType

                                lsSQLQuery = "MATCH (" & LCase(mrTable.Name) & ":" & mrTable.DatabaseName & ")" & vbCrLf

                                For liInd = 0 To Me.mrTable.Column.Count - 1
                                    For liInd2 = 0 To Me.AdvancedDataGridView.Columns.Count - 1
                                        If Me.AdvancedDataGridView.Columns(liInd2).Name = Me.mrTable.Column(liInd).Name Then
                                            Call Me.AdvancedDataGridView.EnableFilter(Me.AdvancedDataGridView.Columns(liInd2))
                                        End If
                                    Next
                                Next

                                Dim lsFilterString As String = Me.AdvancedDataGridView.FilterString

                                For Each lrColumn In Me.mrTable.Column
                                    lsFilterString = lsFilterString.Replace("[" & lrColumn.Name & "]", LCase(Me.mrTable.Name) & "." & lrColumn.Name)
                                Next

                                'lsFilterString = Me.AdvancedDataGridView.FilterString.Replace("[", "").Replace("]", "")
                                lsFilterString = lsFilterString.Replace("(", "[").Replace(")", "]")
                                lsFilterString = lsFilterString.Replace("%", ".*")
                                lsFilterString = lsFilterString.Replace("LIKE", "=~")
                                Try
                                    lsFilterString = lsFilterString.Substring(1, lsFilterString.Length - 2)
                                Catch ex As Exception
                                    'Not a biggie at this stage.
                                End Try

                                'WHERE Clause                    
                                If Trim(lsFilterString) <> "" Then
                                    lsSQLQuery &= " WHERE " & lsFilterString
                                End If
                                Dim lsColumnList As String = String.Join(",", Me.mrTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey).Select(Function(x) LCase(mrTable.DatabaseName) & "." & x.Name))
                                lsSQLQuery &= vbCrLf & "RETURN " & lsColumnList
                                lsSQLQuery &= vbCrLf & " LIMIT 100"
                            End If

                        Case Is = pcenumDatabaseType.SQLite,
                                  pcenumDatabaseType.SQLServer,
                                  pcenumDatabaseType.ORACLE,
                                  pcenumDatabaseType.ODBC
#Region "SQL"
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

                            If lsFilterString = "" Then
                                Me.msFilterString = ""
                            Else
                                If Me.msFilterString <> "" Then
                                    Me.msFilterString.AppendString(" AND " & lsFilterString)
                                Else
                                    Me.msFilterString = lsFilterString
                                End If
                            End If
                            lsFilterString = Me.msFilterString

                            'WHERE Clause                    
                            If Trim(lsFilterString) <> "" Then
                                lsSQLQuery &= " WHERE " & lsFilterString
                            End If

                            lsSQLQuery &= vbCrLf & "LIMIT 100"
#End Region
                    End Select


                    If Me.mrTable.isPGSRelation Then
                        Call Me.PopulateDataGridFromDatabaseQuery(lsSQLQuery,, larColumn)
                    Else
                        Call Me.PopulateDataGridFromDatabaseQuery(lsSQLQuery, Me.mrTable)
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

    Private Sub AdvancedDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles AdvancedDataGridView.DataError
        'Debugger.Break()
    End Sub

    Private Sub ToolStripButtonCSVImport_Click(sender As Object, e As EventArgs) Handles ToolStripButtonCSVImport.Click

        Try
            Dim lfrmCSVLoader As New frmCSVLoader

            lfrmCSVLoader.miFormFunction = pcenumCSVFormFunction.ImportCSVData
            lfrmCSVLoader.mrModel = Me.mrModel
            lfrmCSVLoader.mrTable = Me.mrTable

            Call lfrmCSVLoader.ShowDialog()

            If lfrmCSVLoader.mdtData IsNot Nothing Then

                '=====================================================================
                'Import the CSV Data to the Recordset
                '======================================
                Try
                    Dim lrDummyFactType As New FBM.FactType(Me.mrModel, "DummyFactType", True)
                    Dim liInd As Integer = 0
                    Dim larColumn As List(Of RDS.Column)

                    Select Case Me.mrModel.TargetDatabaseType
                        Case Is = pcenumDatabaseType.Neo4j,
                          pcenumDatabaseType.KuzuDB
                            If Me.mrTable.isPGSRelation Then
                                larColumn = Me.mrDataGridList.mrTable.Column
                            Else
                                larColumn = Me.mrTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey)
                            End If

                        Case Else
                            larColumn = Me.mrTable.Column.ToList
                    End Select

                    For Each lrColumn In larColumn

                        Select Case Me.mrModel.TargetDatabaseType
                            Case Is = pcenumDatabaseType.Neo4j,
                                  pcenumDatabaseType.KuzuDB

                                lrColumn.TemporaryData = LCase(Me.mrTable.Name) & "." & lrColumn.Name
                            Case Else
                                lrColumn.TemporaryData = lrColumn.Name
                        End Select

                        If lrColumn IsNot Nothing Then
                            Dim lrRole = New FBM.Role(lrDummyFactType, lrColumn.TemporaryData, True, Nothing)
                            lrDummyFactType.RoleGroup.AddUnique(lrRole)
                        End If
                    Next

                    For Each lrRow As DataRow In lfrmCSVLoader.mdtData.Rows

                        Dim lrFact = New FBM.Fact(lrDummyFactType, False)
                        lrFact.Id = System.Guid.NewGuid.ToString
                        lrFact.IsNewFact = True

                        liInd = 0
                        For Each loColumnValue In lrRow.ItemArray

                            Dim lsType = loColumnValue.GetType().ToString

                            Dim lrRole = New FBM.Role(lrDummyFactType, larColumn(liInd).TemporaryData, True, Nothing)
                            lrDummyFactType.RoleGroup.AddUnique(lrRole)
                            lrRole.SequenceNr = Me.mrRecordset.Columns.FindIndex(Function(x) x = larColumn(liInd).TemporaryData) + 1

                            Dim lrFactData = New FBM.FactData(lrRole, New FBM.Concept(""), lrFact)
                            lrFactData.setData(loColumnValue.ToString, pcenumConceptType.Value, False)
                            lrFact.Data.Add(lrFactData)

                            liInd += 1
                        Next

                        lrFact.Data = lrFact.Data.OrderBy(Function(f) f.Role.SequenceNr).ToList()

                        Me.mrRecordset.Facts.Add(lrFact)
                    Next

                    Me.mrDataGridList = New ORMQL.RecordsetDataGridList(Me.mrRecordset, Me.mrTable)
                    Me.AdvancedDataGridView.DataSource = Me.mrDataGridList

                Catch ex As Exception
                    Dim lsMessage As String
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                End Try

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripButtonCSVExport_Click(sender As Object, e As EventArgs) Handles ToolStripButtonCSVExport.Click

        Try
            Dim lfrmCSVLoader As New frmCSVLoader

            lfrmCSVLoader.miFormFunction = pcenumCSVFormFunction.ExportCSVData
            lfrmCSVLoader.mrModel = Me.mrModel
            lfrmCSVLoader.mrTable = Me.mrTable
            lfrmCSVLoader.mrRecordset = Me.mrRecordset

            Call lfrmCSVLoader.ShowDialog()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function AllDataGridCulumnsPopulatedByRow(ByVal aiRowIndex As Integer)

        Try

            For liInd = 0 To Me.AdvancedDataGridView.ColumnCount - 1
                If Trim(Me.AdvancedDataGridView.Rows(aiRowIndex).Cells(liInd).Value) = "" Then
                    Return False
                End If
            Next

            Return True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

End Class