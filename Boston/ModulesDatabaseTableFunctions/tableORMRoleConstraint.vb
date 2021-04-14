Imports System.Reflection

Namespace TableRoleConstraint

    Public Module TableRoleConstraint

        ''' <summary>
        ''' Creates a record in the MetaModelRoleConstraint table.
        ''' </summary>
        ''' <param name="arRoleConstraint"></param>
        ''' <remarks></remarks>
        Public Sub AddRoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "INSERT INTO MetaModelRoleConstraint"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraint.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraint.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraint.Name) & "'"
                lsSQLQuery &= " ,'" & arRoleConstraint.RoleConstraintType.ToString & "'"
                lsSQLQuery &= " ,'" & arRoleConstraint.RingConstraintType.ToString & "'"
                lsSQLQuery &= " ," & arRoleConstraint.LevelNr
                lsSQLQuery &= " ," & arRoleConstraint.IsPreferredIdentifier
                lsSQLQuery &= " ," & arRoleConstraint.IsDeontic
                lsSQLQuery &= " ," & arRoleConstraint.MinimumFrequencyCount
                lsSQLQuery &= " ," & arRoleConstraint.MaximumFrequencyCount
                lsSQLQuery &= " ," & arRoleConstraint.Cardinality
                lsSQLQuery &= " ,'" & arRoleConstraint.CardinalityRangeType.ToString & "'"
                lsSQLQuery &= " ,'" & arRoleConstraint.ValueRangeType.ToString & "'"
                lsSQLQuery &= " ," & arRoleConstraint.IsMDAModelElement
                lsSQLQuery &= " ,'" & arRoleConstraint.GUID & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableRoleConstraint.AddRoleConstraint: "
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function ExistsRoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelRoleConstraint"
            lsSQLQuery &= " WHERE RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"
            lsSQLQuery &= "   AND ModelId = '" & Trim(arRoleConstraint.Model.ModelId) & "'"


            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsRoleConstraint = True
            Else
                ExistsRoleConstraint = False
            End If

            lREcordset.Close()

        End Function


        Public Sub delete_RoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

            '-----------------------------------------------
            'deletes the specified RoleConstraint from the
            'RoleConstraint table
            '-----------------------------------------------

            Dim lsSQLQuery As String

            lsSQLQuery = "DELETE FROM MetaModelRoleConstraint"
            lsSQLQuery = lsSQLQuery & " WHERE ModelId = '" & Trim(arRoleConstraint.Model.ModelId) & "'"
            lsSQLQuery = lsSQLQuery & "   AND RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()


        End Sub

        Function getRoleConstraintCountByModel(ByVal ar_model As FBM.Model) As Integer

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelModelDictionary"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(ar_model.ModelId) & "'"
            lsSQLQuery &= "   AND IsRoleConstraint = " & True

            lREcordset.Open(lsSQLQuery)

            getRoleConstraintCountByModel = lREcordset(0).Value

        End Function

        Public Function GetRoleConstraintsByModel(ByRef arModel As FBM.Model) As List(Of FBM.RoleConstraint)

            Dim lsMessage As String
            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Dim lrFactType As FBM.FactType

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetRoleConstraintsByModel = New List(Of FBM.RoleConstraint)

            Try
                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT RC.*, MD.*"
                lsSQLQuery &= "  FROM MetaModelRoleConstraint RC,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arModel.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = RC.RoleConstraintId"
                lsSQLQuery &= "   AND MD.ModelId = RC.ModelId"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.isDirty = False
                        lrRoleConstraint.Id = Trim(lREcordset("RoleConstraintId").Value)
                        lrRoleConstraint.Model = arModel
                        lrRoleConstraint.Name = Trim(Viev.NullVal(lREcordset("RoleConstraintName").Value, ""))
                        'lrRoleConstraint.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                        'lrRoleConstraint.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
                        lrRoleConstraint.ConceptType = pcenumConceptType.RoleConstraint
                        lrRoleConstraint.RoleConstraintType = CType([Enum].Parse(GetType(pcenumRoleConstraintType), lREcordset("RoleConstraintType").Value), pcenumRoleConstraintType)
                        lrRoleConstraint.RingConstraintType = CType([Enum].Parse(GetType(pcenumRingConstraintType), lREcordset("RingConstraintType").Value), pcenumRingConstraintType)
                        lrRoleConstraint.LevelNr = lREcordset("LevelNr").Value
                        lrRoleConstraint.IsPreferredIdentifier = lREcordset("IsPreferredUniqueness").Value
                        lrRoleConstraint.IsDeontic = lREcordset("IsDeontic").Value
                        lrRoleConstraint.Cardinality = lREcordset("Cardinality").Value
                        lrRoleConstraint.MaximumFrequencyCount = lREcordset("MaximumFrequencyCount").Value
                        lrRoleConstraint.MinimumFrequencyCount = lREcordset("MinimumFrequencyCount").Value
                        Select Case lREcordset("CardinalityRangeType").Value
                            Case Is = pcenumCardinalityRangeType.LessThanOREqual.ToString
                                lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOREqual
                            Case Is = pcenumCardinalityRangeType.Equal.ToString
                                lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Equal
                            Case Is = pcenumCardinalityRangeType.GreaterThanOREqual.ToString
                                lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.GreaterThanOREqual
                            Case Is = pcenumCardinalityRangeType.Between.ToString
                                lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Between
                        End Select
                        lrRoleConstraint.ValueRangeType = CType([Enum].Parse(GetType(pcenumValueRangeType), lREcordset("ValueRangeType").Value), pcenumValueRangeType)
                        lrRoleConstraint.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                        lrRoleConstraint.LongDescription = lREcordset("LongDescription").Value
                        lrRoleConstraint.ShortDescription = lREcordset("ShortDescription").Value
                        lrRoleConstraint.GUID = lREcordset("GUID").Value
                        lrRoleConstraint.isDirty = False

                        '------------------------------------------------
                        'Link to the Concept within the ModelDictionary
                        '------------------------------------------------
                        Dim lrDictionaryEntry As New FBM.DictionaryEntry(arModel, lrRoleConstraint.Id, pcenumConceptType.RoleConstraint, lrRoleConstraint.ShortDescription, lrRoleConstraint.LongDescription)
                        lrDictionaryEntry = arModel.AddModelDictionaryEntry(lrDictionaryEntry, True, False, False, False, True, True)

                        If lrDictionaryEntry Is Nothing Then
                            lsMessage = "Cannot find DictionaryEntry in the ModelDictionary for RoleConstraint:"
                            lsMessage &= vbCrLf & "Model.Id: " & arModel.ModelId
                            lsMessage &= vbCrLf & "RoleConstraint.Id: " & lrRoleConstraint.Id
                            Throw New Exception(lsMessage)
                        End If

                        lrRoleConstraint.Concept = lrDictionaryEntry.Concept

                        lrRoleConstraint.Argument = TableRoleConstraintArgument.GetArgumentsForRoleConstraint(lrRoleConstraint)
                        lrRoleConstraint.Role = TableRoleConstraint.getRoles_for_RoleConstraint(lrRoleConstraint)

                        '----------------------------------------------------------
                        'Get the RoleConstraintRole list for the RoleConstraint.
                        '  NB the SequenceNr of a RoleConstraintRole is 'not' the 
                        '  same as a SequenceNr on a Role, and Richmond may very
                        '  well make SequenceNr on Role redundant.
                        '  SequenceNr on a RoleConstraintRole is many things, but
                        '  relates particularly to DataIn, DataOut integrity matching.
                        '----------------------------------------------------------
                        lrRoleConstraint.RoleConstraintRole = TableRoleConstraintRole.getRoleConstraintRoles_by_RoleConstraint(lrRoleConstraint)

                        If lrRoleConstraint.RoleConstraintRole.Count = 0 Then
                            prApplication.ThrowErrorMessage("No RoleConstraintRoles found for RoleConstraint.Id: " & lrRoleConstraint.Id, pcenumErrorType.Information)
                        Else
                            lrFactType = lrRoleConstraint.Role(0).FactType
                            lrFactType = arModel.FactType.Find(AddressOf lrFactType.Equals)

                            Select Case lrRoleConstraint.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                    Call lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)
                            End Select
                        End If

                        GetRoleConstraintsByModel.Add(lrRoleConstraint)
                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()

            Catch ex As Exception
                lsMessage = "Error: TableRoleConstraint.GetRoleConstraintsByModel: "
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Function

        Private Function getRoles_for_RoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint) As List(Of FBM.Role)

            Dim lrRole As FBM.Role
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Dim lsMessage As String = ""
            Dim lsId As String

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '----------------------------
            'Initialise the return value
            '----------------------------
            getRoles_for_RoleConstraint = New List(Of FBM.Role)


            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM MetaModelRoleConstraintRole"
            lsSQLQuery &= " WHERE RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"
            lsSQLQuery &= "   AND ModelId = '" & Trim(arRoleConstraint.Model.ModelId) & "'"

            Try
                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF

                        lsId = Trim(lREcordset("RoleId").Value)
                        lrRole = arRoleConstraint.Model.Role.Find(Function(x) x.Id = lsId)

                        If Not IsSomething(lrRole) Then
                            lsMessage = "No Role found in Model for Role.Id = '" & Trim(lREcordset("RoleId").Value) & "'" & vbCrLf & "RoleConstraint.Id: " & Trim(arRoleConstraint.Id)
                            Throw New Exception(lsMessage)
                        End If

                        lrRole.RoleConstraintRole.Add(New FBM.RoleConstraintRole(lrRole, arRoleConstraint, , , , False))

                        getRoles_for_RoleConstraint.Add(lrRole)

                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()
            Catch ex As Exception
                Dim lsMessage1 As String
                lsMessage1 = "Error: TableRoleConstraint.getRolesForRoleConstraint: "
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub ModifyKey(ByVal arRoleConstraint As FBM.RoleConstraint, ByVal asNewKey As String)

            Dim lsSQLQuery As String

            lsSQLQuery = " UPDATE MetaModelRoleConstraint"
            lsSQLQuery &= "   SET RoleConstraintId = '" & Trim(asNewKey) & "'"
            lsSQLQuery &= " WHERE RoleConstraintId = '" & arRoleConstraint.Id & "'"
            lsSQLQuery &= " AND ModelId = '" & arRoleConstraint.Model.ModelId & "'"


            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub


        Public Sub UpdateRoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelRoleConstraint"
                lsSQLQuery &= "   SET RoleConstraintName = '" & arRoleConstraint.Name & "'"
                lsSQLQuery &= "       ,RoleConstraintType = '" & arRoleConstraint.RoleConstraintType.ToString & "'"
                lsSQLQuery &= "       ,RingConstraintType = '" & arRoleConstraint.RingConstraintType.ToString & "'"
                lsSQLQuery &= "       ,LevelNr = " & arRoleConstraint.LevelNr
                lsSQLQuery &= "       ,IsPreferredUniqueness = " & arRoleConstraint.IsPreferredIdentifier
                lsSQLQuery &= "       ,IsDeontic = " & arRoleConstraint.IsDeontic
                lsSQLQuery &= "       ,MinimumFrequencyCount = " & arRoleConstraint.MinimumFrequencyCount
                lsSQLQuery &= "       ,MaximumFrequencyCount = " & arRoleConstraint.MaximumFrequencyCount
                lsSQLQuery &= "       ,Cardinality = " & arRoleConstraint.Cardinality
                lsSQLQuery &= "       ,CardinalityRangeType = '" & arRoleConstraint.CardinalityRangeType.ToString & "'"
                lsSQLQuery &= "       ,ValueRangeType = '" & arRoleConstraint.ValueRangeType.ToString & "'"
                lsSQLQuery &= "       ,IsMDAModelElement = " & arRoleConstraint.IsMDAModelElement
                lsSQLQuery &= "       ,[GUID] = '" & arRoleConstraint.GUID & "'"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arRoleConstraint.Model.ModelId) & "'"
                lsSQLQuery &= "   AND RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception

                pdbConnection.RollbackTrans()

                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

    End Module

End Namespace