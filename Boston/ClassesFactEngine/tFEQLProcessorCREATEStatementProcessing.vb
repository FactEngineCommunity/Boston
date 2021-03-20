Imports System.Reflection

Namespace FEQL
    Partial Public Class Processor

        Public Sub processCREATEDATABASEStatement(ByVal asFEQLStatement As String)

            Try
                Dim lrCREATESDATABASEtatement = New FEQL.CREATEDATABASEStatement

                Call Me.GetParseTreeTokensReflection(lrCREATESDATABASEtatement, Me.Parsetree.Nodes(0))

                Dim lrModel = prApplication.createDatabase(lrCREATESDATABASEtatement)

                'Create the actual database. SQLite only at this stage.
                Me.DatabaseManager.Connection = New FactEngine.SQLiteConnection(lrModel, lrCREATESDATABASEtatement.FILELOCATIONNAME, 100, True)
                Call Me.DatabaseManager.Connection.createDatabase(lrCREATESDATABASEtatement.FILELOCATIONNAME)

                lrModel.TargetDatabaseType = pcenumDatabaseType.SQLite
                lrModel.TargetDatabaseConnectionString = "Data Source=" & lrCREATESDATABASEtatement.FILELOCATIONNAME & ";"
                lrModel.TargetDatabaseConnectionString &= "Version=3;"

                lrModel.DatabaseConnection = Me.DatabaseManager.establishConnection(lrModel.TargetDatabaseType,
                                                                                    lrModel.TargetDatabaseConnectionString)

                lrModel.IsDatabaseSynchronised = True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function ProcessCREATEStatement(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lrRecordset = New ORMQL.Recordset(FactEngine.pcenumFEQLStatementType.CREATEStatement)

            Try
                Dim lrInsertTable As New RDS.Table(New RDS.Model, "DummyTableName", Nothing)
                Dim larInsertColumn As New List(Of RDS.Column)
                Dim lsInsertStatement As String = ""
                Dim liInd As Integer

                If Me.DatabaseManager.Connection Is Nothing Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(prApplication.WorkingModel.TargetDatabaseType, prApplication.WorkingModel.TargetDatabaseConnectionString)
                    If Me.DatabaseManager.Connection Is Nothing Then
                        Throw New Exception("No database connection has been established.")
                    End If
                ElseIf Me.DatabaseManager.Connection.Connected = False Then
                    Throw New Exception("The database is not connected.")
                End If

                'Richmond.WriteToStatusBar("Processsing WHICH Statement.", True)
                Me.CREATEStatement = New FEQL.CREATEStatement

                Call Me.GetParseTreeTokensReflection(Me.CREATEStatement, Me.Parsetree.Nodes(0))

                'Get primary Table
                Dim lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.MODELELEMENTNAME(0))
                lrInsertTable.Name = lrTable.Name

                '========================================================================
                'Get the rest of the Columns
                Dim lrQueryGraph As New FactEngine.QueryGraph(Me.Model)
                Dim lrQueryEdge As FactEngine.QueryEdge
                '-----------------------------------------
                For Each lrPredicateNodePropertyIndentification In Me.CREATEStatement.PREDICATENODEPROPERTYIDENTIFICATION
                    lrQueryEdge = New FactEngine.QueryEdge(lrQueryGraph, Nothing)

                    Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(lrInsertTable.Name)
                    Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(lrPredicateNodePropertyIndentification.NODE.MODELELEMENTNAME) 'PROPERTYIDENTIFICATION.MODELELEMENTNAME)
                    lrQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject, lrQueryEdge)
                    lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject, lrQueryEdge)
                    lrQueryEdge.TargetNode.PreboundText = lrPredicateNodePropertyIndentification.NODE.PREBOUNDREADINGTEXT

                    '---------------------------------------------------------
                    'Get the Predicate. Every which clause has a Predicate.
                    For Each lsPredicatePart In lrPredicateNodePropertyIndentification.PREDICATECLAUSE.PREDICATE
                        lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                    Next

                    'Get the relevant FBM.FactType
                    Call lrQueryEdge.getAndSetFBMFactType(lrQueryEdge.BaseNode,
                                                          lrQueryEdge.TargetNode,
                                                          lrQueryEdge.Predicate)



                    Dim lrInsertColumn As RDS.Column

                    If lrQueryEdge.FBMFactType.IsManyTo1BinaryFactType Or lrQueryEdge.FBMFactType.Is1To1BinaryFactType Then
                        Select Case lrQueryEdge.FBMFactType.RoleGroup(1).JoinedORMObject.GetType
                            Case GetType(FBM.EntityType)
                                'Need to get the value of the PrimaryKey of row for the Table/EntityType based on the UniqueIndex value supplied by the user
                                Dim lrTargetTable = lrQueryEdge.TargetNode.FBMModelObject.getCorrespondingRDSTable
                                Dim larUniqueIndexColumn = lrTargetTable.getFirstUniquenessConstraintColumns

                                Dim lsSQLQuery As String = "SELECT "
                                liInd = 0
                                For Each lrPrimaryIndexColumn In lrTargetTable.getPrimaryKeyColumns
                                    If liInd > 0 Then lsSQLQuery &= ", "
                                    lsSQLQuery &= lrPrimaryIndexColumn.Name
                                    liInd += 1
                                Next
                                lsSQLQuery &= " FROM " & lrTargetTable.Name
                                lsSQLQuery &= " WHERE "

                                'CodeSafe
                                If larUniqueIndexColumn.Count <> lrPredicateNodePropertyIndentification.NODE.NODEPROPERTYIDENTIFICATION.IDENTIFIER.Count Then
                                    Throw New Exception("The primary unique index for model element, '" & lrTargetTable.Name & "', has " & larUniqueIndexColumn.Count.ToString & " columns, rather than the " & lrPredicateNodePropertyIndentification.NODE.NODEPROPERTYIDENTIFICATION.IDENTIFIER.Count.ToString & " for which values are provided.")
                                End If

                                Dim lsSelectValues = ""
                                liInd = 0
                                For Each lrUniqueIndexColumn In larUniqueIndexColumn
                                    If liInd > 0 Then lsSQLQuery &= " AND "
                                    lsSQLQuery &= lrUniqueIndexColumn.Name & " = "
                                    Dim lsSelectValue = ""
                                    If lrUniqueIndexColumn.DataTypeIsText Then lsSelectValue &= "'"
                                    lsSelectValue &= lrPredicateNodePropertyIndentification.NODE.NODEPROPERTYIDENTIFICATION.IDENTIFIER(liInd)
                                    If lrUniqueIndexColumn.DataTypeIsText Then lsSelectValue &= "'"
                                    If liInd > 0 Then lsSelectValue &= ", "
                                    lsSelectValues &= lsSelectValue

                                    lsSQLQuery &= lsSelectValue
                                    liInd += 1
                                Next

                                Dim lrInterimRecordset = Me.DatabaseManager.Connection.GO(lsSQLQuery)

                                If lrInterimRecordset.CurrentFact Is Nothing Then
                                    Throw New Exception("Could not find a row in " & lrTargetTable.Name & " for value/s " & lsSelectValues)
                                End If

                                For Each lrPrimaryIndexColumn In lrTargetTable.getPrimaryKeyColumns
                                    If lrQueryEdge.FBMFactType.IsLinkFactType Then
                                        lrInsertColumn = lrTable.Column.Find(Function(x) x.Role Is lrQueryEdge.FBMFactType.LinkFactTypeRole)
                                    Else
                                        lrInsertColumn = lrTable.Column.Find(Function(x) x.Role Is lrQueryEdge.FBMFactType.RoleGroup(0)).Clone(Nothing, Nothing)
                                    End If
                                    lrInsertColumn.TemporaryData = lrInterimRecordset.CurrentFact.Data(0).Data
                                    larInsertColumn.Add(lrInsertColumn)
                                Next
                            Case GetType(FBM.ValueType)
                                lrInsertColumn = lrTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType).Clone(Nothing, Nothing)
                                lrInsertColumn.TemporaryData = lrPredicateNodePropertyIndentification.NODE.NODEPROPERTYIDENTIFICATION.IDENTIFIER(0)
                                larInsertColumn.Add(lrInsertColumn)
                        End Select
                    End If
                Next

                'PK Columns
                If lrTable.getPrimaryKeyColumns.Count = 1 Then
                    Dim lrPKColumn = lrTable.getPrimaryKeyColumns(0)
                    If lrPKColumn.getMetamodelDataType = pcenumORMDataType.NumericAutoCounter Then
                        Dim lrInsertColumn As New RDS.Column(lrInsertTable, lrPKColumn.Name, Nothing, Nothing)
                        If Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.QUOTEDIDENTIFIERLIST IsNot Nothing Then
                            If Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.QUOTEDIDENTIFIERLIST.COLON IsNot Nothing Then
                                lrInsertColumn.TemporaryData = Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.IDENTIFIER(0)
                            Else
                                lrInsertColumn.TemporaryData = "1"
                            End If
                        Else
                            lrInsertColumn.TemporaryData = "1"
                        End If

                        larInsertColumn.Add(lrInsertColumn)
                    Else
                        'Dim lrInsertColumn As New RDS.Column(lrInsertTable, lrPKColumn.Name, Nothing, Nothing)
                        Dim lrInsertColumn = lrPKColumn.Clone(Nothing, Nothing)
                        lrInsertColumn.TemporaryData = Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.IDENTIFIER(0)
                        larInsertColumn.Add(lrInsertColumn)
                    End If
                Else
                    If Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.IDENTIFIER.Count > 0 And
                       Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.IDENTIFIER.Count <> lrTable.getPrimaryKeyColumns.Count Then
                        Throw New Exception("The model element, '" & lrInsertTable.Name & "', has " & lrTable.getPrimaryKeyColumns.Count.ToString & " primary key columns. You have specified " & Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.IDENTIFIER.Count.ToString & " primary key columns.")
                    ElseIf Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.IDENTIFIER.Count = 0 Then
                        For Each lrColumn In lrTable.getPrimaryKeyColumns
                            If Not larInsertColumn.Contains(lrColumn) Then
                                Throw New Exception("You haven't specified relationships for each of the primary key properties of the Object Type.")
                            End If
                        Next
                    Else
                        liInd = 0
                        For Each lrPKColumn In lrTable.getPrimaryKeyColumns
                            'Dim lrInsertColumn As New RDS.Column(lrInsertTable, lrPKColumn.Name, Nothing, Nothing)
                            Dim lrInsertColumn = lrPKColumn.Clone(Nothing, Nothing)

                            If Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.QUOTEDPROPERTYIDENTIFIERLIST IsNot Nothing Then
                                Dim liInd2 = Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.MODELELEMENTNAME.IndexOf(lrInsertColumn.Name)

                                If liInd2 < 0 Then
                                    Throw New Exception("Cannot find property, " & lrInsertColumn.Name & ", in your statement. Required for Primmary Key of the Object Type, " & lrTable.Name)
                                End If

                                lrInsertColumn.TemporaryData = Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.IDENTIFIER(liInd2 - 1)
                            Else
                                lrInsertColumn.TemporaryData = Me.CREATEStatement.NODEPROPERTYNAMEIDENTIFICATION.IDENTIFIER(liInd)
                            End If
                            larInsertColumn.Add(lrInsertColumn)
                            liInd += 1
                        Next
                    End If
                End If

                'Data Types set, check.
                If larInsertColumn.FindAll(Function(x) x.getMetamodelDataType = pcenumORMDataType.DataTypeNotSet).Count > 0 Then
                    Throw New Exception("Make sure the Data Type is set for all properties before creating a new record.")
                End If

                'Mandatory Column check
                For Each lrColumn In lrTable.Column.FindAll(Function(x) x.IsMandatory)
                    If larInsertColumn.Find(Function(x) x.Name = lrColumn.Name) Is Nothing Then
                        Throw New Exception("The Object Type, " & lrTable.Name & ", requires that " & lrColumn.Name & " is polulated.")
                    End If
                Next

                'Data Type check
                For Each lrColumn In larInsertColumn
                    Select Case lrColumn.getMetamodelDataType
                        Case Is = pcenumORMDataType.TextFixedLength
                            If Len(lrColumn.TemporaryData) > lrColumn.ActiveRole.JoinsValueType.DataTypeLength Then
                                Throw New Exception("Property, " & lrColumn.Name & ", has a maximum data length of " & lrColumn.ActiveRole.JoinsValueType.DataTypeLength & ".")
                            End If
                        Case Is = pcenumORMDataType.NumericFloatSinglePrecision
                            If Not IsNumeric(lrColumn.TemporaryData) Then
                                Throw New Exception("Property, " & lrColumn.Name & ", requires a Numeric Single Precision compatible value.")
                            End If
                    End Select
                Next

                'Ring Constraint check
                Dim lrRingConstraint As FBM.RoleConstraint = Nothing
                If lrTable.isConstrainedByRingConstraint(lrRingConstraint) Then

                    Select Case lrRingConstraint.RingConstraintType
                        Case Is = pcenumRingConstraintType.Asymmetric

                            Dim lsSQLQuery As String = "SELECT *"
                            lsSQLQuery &= " FROM " & lrTable.Name
                            lsSQLQuery &= " WHERE "

                            liInd = 0
                            Dim lrTempRingConstraint As FBM.RoleConstraint = lrRingConstraint.Clone(Me.Model, False, False)
                            Call lrTempRingConstraint.RoleConstraintRole.Reverse()
                            For Each lrRole In lrTempRingConstraint.Role
                                For Each lrColumn In larInsertColumn.FindAll(Function(x) x.Role.Id = lrRole.Id)
                                    If liInd > 0 Then lsSQLQuery &= " AND "
                                    lsSQLQuery &= lrColumn.Name & " = "
                                    If lrColumn.DataTypeIsText Then lsSQLQuery &= "'"
                                    lsSQLQuery &= lrColumn.TemporaryData
                                    If lrColumn.DataTypeIsText Then lsSQLQuery &= "'"
                                Next
                                liInd += 1
                            Next

                            Dim lrInterimRecordset = Me.DatabaseManager.Connection.GO(lsSQLQuery)

                            If lrInterimRecordset.Facts.Count > 0 Then
                                Throw New Exception(lrRingConstraint.RingConstraintType.ToString & " Ring Constraint, " & lrRingConstraint.Id & ", violated. No record inserted.")
                            End If

                    End Select
                End If

                'Indexes Check                
                For Each lrIndex In lrTable.Index
                    Dim larColumn = larInsertColumn.FindAll(Function(x) lrIndex.Column.Find(Function(y) y.Name = x.Name) IsNot Nothing)
                    If Me.IndexedValuesExistInDatabase(lrIndex, larColumn) Then
                        Dim lsMessage = "The values, "
                        Dim lsColumnNames = ""
                        liInd = 0
                        For Each lrColumn In larColumn
                            If liInd > 0 Then
                                lsMessage &= ", "
                                lsColumnNames &= ", "
                            End If
                            lsMessage &= lrColumn.TemporaryData
                            lsColumnNames &= lrColumn.Name
                            liInd += 1
                        Next
                        Throw New Exception(lsMessage & " already uniquely exist for Columns, " & lsColumnNames)
                    End If
                Next

                'Value Constraint check
                For Each lrColumn In larInsertColumn.FindAll(Function(x) x.ActiveRole.JoinsValueType.ValueConstraint.Count > 0)
                    If Not lrColumn.ActiveRole.JoinsValueType.ValueConstraint.Contains(lrColumn.TemporaryData) Then
                        Throw New Exception("Value, " & lrColumn.TemporaryData & ", for Property, " & lrColumn.Name & ", is outside the Value Constraints for that Property.")
                    End If
                Next

                'Create the INSERT Statement
                lsInsertStatement = "INSERT INTO " & lrInsertTable.Name
                lsInsertStatement &= " ("
                liInd = 0
                For Each lrInsertColumn In larInsertColumn
                    If liInd > 0 Then lsInsertStatement &= "," & vbCrLf
                    lsInsertStatement &= lrInsertColumn.Name
                    liInd += 1
                Next
                lsInsertStatement &= vbCrLf & ")"
                lsInsertStatement &= " VALUES ("
                liInd = 0
                For Each lrInsertColumn In larInsertColumn
                    If liInd > 0 Then lsInsertStatement &= "," & vbCrLf
                    If lrInsertColumn.DataTypeIsText Then lsInsertStatement &= "'"
                    lsInsertStatement &= lrInsertColumn.TemporaryData
                    If lrInsertColumn.DataTypeIsText Then lsInsertStatement &= "'"
                    liInd += 1
                Next
                lsInsertStatement &= vbCrLf & ")"

                lrRecordset = Me.DatabaseManager.Connection.GONonQuery(lsInsertStatement)

                If lrRecordset.ErrorString Is Nothing Then
                    lrRecordset.ErrorString = "1 row inserted"
                End If

                Return lrRecordset

            Catch ex As Exception
                If ex.InnerException Is Nothing Then
                    lrRecordset.ErrorString = ex.Message
                Else
                    lrRecordset.ErrorString = ex.InnerException.Message
                End If

                Return lrRecordset
            End Try

        End Function

    End Class
End Namespace
