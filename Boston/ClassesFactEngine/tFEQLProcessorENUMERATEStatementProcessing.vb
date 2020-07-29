Namespace FEQL
    Partial Public Class Processor

        Public Function ProcessENUMERATEStatement(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lrRecordset As New ORMQL.Recordset
            lrRecordset.StatementType = FactEngine.Constants.pcenumFEQLStatementType.ENUMERATEStatement

            Try
                'Richmond.WriteToStatusBar("Processsing WHICH Statement.", True)
                Me.ENUMMERATEStatement = New FEQL.ENUMERATEStatement

                Call Me.GetParseTreeTokensReflection(Me.ENUMMERATEStatement, Me.Parsetree.Nodes(0))

                '----------------------------------------
                'Create the HeadNode for the QueryGraph
                Dim lrFBMModelObject As FBM.ModelObject = Me.Model.GetModelObjectByName(Me.ENUMMERATEStatement.MODELELEMENTNAME)
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.ENUMMERATEStatement.MODELELEMENTNAME & "'.")

                'Richmond.WriteToStatusBar("Generating SQL", True)

                '==========================================================================
                'Get the records
                Dim lsSQLQuery = "SELECT * FROM " & lrFBMModelObject.Name

                '==========================================================
                'Populate the lrRecordset with results from the database
                'Richmond.WriteToStatusBar("Connecting to database.", True)
                Dim lrSQLiteConnection = Database.CreateConnection(Me.Model.TargetDatabaseConnectionString)
                Dim lrSQLiteDataReader = Database.getReaderForSQL(lrSQLiteConnection, lsSQLQuery)

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.Model, "DummyFactType", True)
                Dim lrFact As FBM.Fact
                'Richmond.WriteToStatusBar("Reading results.", True)

                '=====================================================
                'Column Names        
                For Each lrProjectColumn In lrFBMModelObject.getCorrespondingRDSTable.Column.OrderBy(Function(x) x.OrdinalPosition)
                    lrRecordset.Columns.Add(lrProjectColumn.Name)
                    Dim lrRole = New FBM.Role(lrFactType, lrProjectColumn.Name, True, Nothing)
                    lrFactType.RoleGroup.AddUnique(lrRole)
                Next

                While lrSQLiteDataReader.Read()

                    lrFact = New FBM.Fact(lrFactType, False)
                    Dim loFieldValue As Object = Nothing
                    For liInd = 0 To lrSQLiteDataReader.FieldCount - 1
                        Select Case lrSQLiteDataReader.GetFieldType(liInd)
                            Case Is = GetType(String)
                                loFieldValue = lrSQLiteDataReader.GetString(liInd)
                            Case Else
                                loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                        End Select

                        lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(loFieldValue), lrFact))
                        '=====================================================
                    Next

                    larFact.Add(lrFact)

                End While
                lrRecordset.Facts = larFact
                lrSQLiteConnection.Close()

                'Run the SQL against the database
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
