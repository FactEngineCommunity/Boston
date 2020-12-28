Imports System.Reflection

Namespace TableFactTypeReading
    Module tableORMFactTypeReading

        Public Sub AddFactTypeReading(ByVal arFactTypeReading As FBM.FactTypeReading)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelFactTypeReading"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(arFactTypeReading.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(arFactTypeReading.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactTypeReading.FactType.Id, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactTypeReading.FrontText, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactTypeReading.FollowingText, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactTypeReading.TypedPredicateId, "'", "`")) & "'"
                lsSQLQuery &= " , " & arFactTypeReading.IsPreferred
                lsSQLQuery &= " , " & arFactTypeReading.IsPreferredForPredicate
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeReading.AddFactTypeReading"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function ExistsFactTypeReading(ByVal arFactTypeReading As FBM.FactTypeReading) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelFactTypeReading"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arFactTypeReading.Model.ModelId) & "'"
                lsSQLQuery &= "   AND FactTypeReadingId = '" & Trim(arFactTypeReading.Id) & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsFactTypeReading = True
                Else
                    ExistsFactTypeReading = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub DeleteFactTypeReading(ByVal arFactTypeReading As FBM.FactTypeReading)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = "DELETE FROM MetaModelFactTypeReading"
                lsSQLQuery = lsSQLQuery & " WHERE ModelId = '" & Trim(arFactTypeReading.Model.ModelId) & "'"
                lsSQLQuery = lsSQLQuery & "   AND FactTypeReadingId = '" & Trim(arFactTypeReading.Id) & "'"

                pdbConnection.Execute(lsSQLQuery)
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeReading.DeleteFactTypeReading"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function GetFactTypeReadingsForFactType(ByRef arFactType As FBM.FactType) As List(Of FBM.FactTypeReading)

            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Dim lsMessage As String = ""

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT ftr.*"
            lsSQLQuery &= "  FROM MetaModelFactTypeReading ftr"
            lsSQLQuery &= " WHERE ftr.ModelId = '" & Trim(arFactType.Model.ModelId) & "'"
            lsSQLQuery &= "   AND ftr.FactTypeId = '" & Trim(arFactType.Id) & "'"

            lREcordset.Open(lsSQLQuery)

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetFactTypeReadingsForFactType = New List(Of FBM.FactTypeReading)

            Try

                Dim liSequenceNr As Integer = 0                

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrFactTypeReading = New FBM.FactTypeReading
                        lrFactTypeReading.isDirty = False
                        lrFactTypeReading.Model = arFactType.Model
                        lrFactTypeReading.FactType = arFactType
                        lrFactTypeReading.Id = lREcordset("FactTypeReadingId").Value
                        lrFactTypeReading.FrontText = NullVal(lREcordset("FrontText").Value, "")
                        lrFactTypeReading.FollowingText = NullVal(lREcordset("FollowingText").Value, "")
                        lrFactTypeReading.TypedPredicateId = lREcordset("TypedPredicateId").Value
                        lrFactTypeReading.IsPreferred = CBool(lREcordset("IsPreferred").Value)
                        lrFactTypeReading.IsPreferredForPredicate = CBool(lREcordset("IsPreferredForPredicate").Value)
                        lrFactTypeReading.isDirty = False

                        '--------------------------------------------------
                        'Get the PredicateParts within the FactTypeReading
                        '--------------------------------------------------
                        lrFactTypeReading.PredicatePart = tableORMPredicatePart.GetPredicatePartsByFactTypeReading(lrFactTypeReading)

                        '------------------------------------------------------------------------------------
                        '20201228-VM-Can remove the below. Happy with the database model the way it is, and never adopted the FBMWG's recommendations. The Boston model is sound and is at new Boston model.
                        '20161110-VM-New code for FactTypeReadingRole collection goes here.
                        '  NB Eventually, the code above (retrieving the PredicatePart collection) will be 
                        '  removed once the migration to the new database model is complete.
                        '-------------------------------------------------------------------------------------
                        'The strategy is to use the existing MetaModelPredicatePart table, and rename the table to MetaModelFactTypeReadingRole
                        '  once the migration to the new data model is complete.
                        '----------------------------------------------------------------------------------------------------

                        '-------------------------------------------------------------------------------------------------
                        '20161110-VM-Eventually, this is the code that will be called.
                        '-------------------------------------------------------------------------------------------------
                        'lrFactTypeReading.FactTypeReadingRole = TableORMFactTypeReadingRole.GetFactTypeReadingRolesByFactTypeReading(lrFactTypeReading)

                        '=============================================================================
                        'CodeSafe/Organic Self Healing
                        If lrFactTypeReading.PredicatePart.FindAll(Function(x) x.Role Is Nothing).Count > 0 Then
                            'Something has gone wrong and there is a PredicatePart with a Role that is not in the database.
                            Dim larNullRolePredicateParts = lrFactTypeReading.PredicatePart.FindAll(Function(x) x.Role Is Nothing)
                            If lrFactTypeReading.PredicatePart.Count > arFactType.Arity Then
                                'Remove the PredicateParts that have NULL/Nothing Roles
                                For Each lrPredicatePart In larNullRolePredicateParts
                                    lrFactTypeReading.PredicatePart.Remove(lrPredicatePart)
                                Next
                            End If

                            'Find a Role for the PredicateParts that have Roles that are Nothing
                            For Each lrRole In arFactType.RoleGroup
                                If lrFactTypeReading.PredicatePart.FindAll(Function(x) x.Role IsNot Nothing).Find(Function(x) x.Role.Id = lrRole.Id) Is Nothing Then
                                    Dim lrPredicatePart = larNullRolePredicateParts.Find(Function(x) x.Role Is Nothing)
                                    lrPredicatePart.Role = lrRole
                                    lrPredicatePart.makeDirty()
                                End If
                            Next
                            arFactType.makeDirty()
                            lrFactTypeReading.makeDirty()
                        End If

                        If lrFactTypeReading.PredicatePart Is Nothing Then
                            lsMessage = "Error: TableFactTypeReading.GetFactTypeReadingsForFactType: "
                            lsMessage &= vbCrLf & "No PredicateParts found for:"
                            lsMessage &= vbCrLf & "FactType.Id: '" & arFactType.Id & "'"
                            lsMessage &= vbCrLf & "FactTypeReading.Id: '" & lrFactTypeReading.Id & "'"
                            Throw New Exception(lsMessage)
                        End If

                        GetFactTypeReadingsForFactType.Add(lrFactTypeReading)

                        lREcordset.MoveNext()
                    End While
                Else
                    GetFactTypeReadingsForFactType = New List(Of FBM.FactTypeReading)
                End If

                lREcordset.Close()
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try



        End Function

        Public Sub UpdateFactTypeReading(ByVal arFactTypeReading As FBM.FactTypeReading)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = " UPDATE MetaModelFactTypeReading"
                lsSQLQuery &= "   SET FactTypeReadingId = FactTypeReadingId"
                lsSQLQuery &= "       ,FactTypeId = '" & Trim(Replace(arFactTypeReading.FactType.Id, "'", "`")) & "'"
                lsSQLQuery &= "       ,FrontText = '" & Trim(Replace(arFactTypeReading.FrontText, "'", "`")) & "'"
                lsSQLQuery &= "       ,FollowingText = '" & Trim(Replace(arFactTypeReading.FollowingText, "'", "`")) & "'"
                lsSQLQuery &= "       ,TypedPredicateId = '" & Trim(Replace(arFactTypeReading.TypedPredicateId, "'", "`")) & "'"
                lsSQLQuery &= "       ,IsPreferred = " & arFactTypeReading.IsPreferred
                lsSQLQuery &= "       ,IsPreferredForPredicate = " & arFactTypeReading.IsPreferredForPredicate
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arFactTypeReading.Model.ModelId) & "'"
                lsSQLQuery &= "   AND FactTypeReadingId = '" & Trim(arFactTypeReading.Id) & "'"

                pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeReading.UpdateFactTypeReading"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module
End Namespace