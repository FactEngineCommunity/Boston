Namespace FEQL
    Partial Public Class Processor

        Public Function ProcessCREATEStatement(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lrRecordset = New ORMQL.Recordset(FactEngine.pcenumFEQLStatementType.CREATEStatement)

            Try
                Dim lrInsertTable As New RDS.Table(New RDS.Model, "DummyTableName", Nothing)
                Dim larInsertColumn As New List(Of RDS.Column)
                Dim lsInsertStatement As String = ""
                Dim liInd As Integer


                'Richmond.WriteToStatusBar("Processsing WHICH Statement.", True)
                Me.CREATEStatement = New FEQL.CREATEStatement

                Call Me.GetParseTreeTokensReflection(Me.CREATEStatement, Me.Parsetree.Nodes(0))

                'Get primary Table
                Dim lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = Me.CREATEStatement.NODEPROPERTYIDENTIFICATION(0).MODELELEMENTNAME)
                lrInsertTable.Name = lrTable.Name

                If lrTable.getPrimaryKeyColumns.Count = 1 Then
                    Dim lrPKColumn = lrTable.getPrimaryKeyColumns(0)
                    If lrPKColumn.getMetamodelDataType = pcenumORMDataType.NumericAutoCounter Then
                        Dim lrInsertColumn As New RDS.Column(lrInsertTable, lrPKColumn.Name, Nothing, Nothing)
                        lrInsertColumn.TemporaryData = "1"
                        larInsertColumn.Add(lrInsertColumn)
                    Else
                        Dim lrInsertColumn As New RDS.Column(lrInsertTable, lrPKColumn.Name, Nothing, Nothing)
                        lrInsertColumn.TemporaryData = Me.CREATEStatement.NODEPROPERTYIDENTIFICATION(0).IDENTIFIER(0)
                        larInsertColumn.Add(lrInsertColumn)
                    End If
                Else
                    If Me.CREATEStatement.NODEPROPERTYIDENTIFICATION(0).IDENTIFIER.Count <> lrTable.getPrimaryKeyColumns.Count Then
                        Throw New Exception("The model element, '" & lrInsertTable.Name & "', has " & lrTable.getPrimaryKeyColumns.Count.ToString & " primary key columns. You have specified " & Me.CREATEStatement.NODEPROPERTYIDENTIFICATION(0).IDENTIFIER.Count.ToString & " primary key columns.")
                    End If
                    liInd = 0
                    For Each lrPKColumn In lrTable.getPrimaryKeyColumns
                        Dim lrInsertColumn As New RDS.Column(lrInsertTable, lrPKColumn.Name, Nothing, Nothing)
                        lrInsertColumn.TemporaryData = Me.CREATEStatement.NODEPROPERTYIDENTIFICATION(0).IDENTIFIER(liInd)
                        larInsertColumn.Add(lrInsertColumn)
                        liInd += 1
                    Next
                End If

                '========================================================================
                'Get the rest of the Columns
                Dim lrQueryGraph As New FactEngine.QueryGraph(Me.Model)
                Dim lrQueryEdge As FactEngine.QueryEdge
                '-----------------------------------------
                For Each lrPredicateNodePropertyIndentification In Me.CREATEStatement.PREDICATENODEPROPERTYIDENTIFICATION
                    lrQueryEdge = New FactEngine.QueryEdge(lrQueryGraph, Nothing)

                    Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(lrInsertTable.Name)
                    Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(lrPredicateNodePropertyIndentification.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                    lrQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject, lrQueryEdge)
                    lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject, lrQueryEdge)

                    '---------------------------------------------------------
                    'Get the Predicate. Every which clause has a Predicate.
                    For Each lsPredicatePart In lrPredicateNodePropertyIndentification.PREDICATECLAUSE.PREDICATE
                        lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                    Next

                    'Get the relevant FBM.FactType
                    Call lrQueryEdge.getAndSetFBMFactType(lrQueryEdge.BaseNode,
                                                          lrQueryEdge.TargetNode,
                                                          lrQueryEdge.Predicate)

                    Dim lrInsertColumn = lrTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType).Clone(Nothing, Nothing)
                    lrInsertColumn.TemporaryData = lrPredicateNodePropertyIndentification.NODEPROPERTYIDENTIFICATION.IDENTIFIER(0)
                    larInsertColumn.Add(lrInsertColumn)
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

                lrRecordset.ErrorString = lsInsertStatement
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
