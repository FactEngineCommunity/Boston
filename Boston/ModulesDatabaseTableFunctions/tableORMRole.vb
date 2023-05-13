Imports System.Reflection

Namespace TableRole

    Public Module TableRole

        Public Sub AddRole(ByVal arRole As FBM.Role)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelRole"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= "," & pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= " ,'" & Trim(arRole.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(arRole.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(arRole.Name) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arRole.FactType.Id, "'", "`")) & "'"
                lsSQLQuery &= " ," & arRole.part_of_key
                lsSQLQuery &= " ," & arRole.FactType.Arity
                lsSQLQuery &= " ," & arRole.SequenceNr
                'If IsSomething(arRole.JoinsEntityType) Then
                '    lsSQLQuery &= "       ,'" & Replace(Viev.NullVal(arRole.JoinsEntityType.Id, ""), "'", "`") & "'"
                'Else
                '    lsSQLQuery &= "       ,''"
                'End If
                'If IsSomething(arRole.JoinsValueType) Then
                '    lsSQLQuery &= "       ,'" & Replace(Viev.NullVal(arRole.JoinsValueType.Id, ""), "'", "`") & "'"
                'Else
                '    lsSQLQuery &= "       ,''"
                'End If
                'If IsSomething(arRole.JoinsFactType) Then
                '    lsSQLQuery &= "       ,'" & Replace(Viev.NullVal(arRole.JoinsFactType.Id, ""), "'", "`") & "'"
                'Else
                '    lsSQLQuery &= "       ,''"
                'End If
                lsSQLQuery &= " ," & arRole.TypeOfJoin
                lsSQLQuery &= " ," & arRole.Mandatory
                lsSQLQuery &= " ," & arRole.IsArray
                lsSQLQuery &= " ,'" & arRole.JoinedORMObject.Id & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub DeleteRole(ByVal arRole As FBM.Role)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "DELETE FROM MetaModelRole"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arRole.Model.ModelId) & "'"
                lsSQLQuery &= "   AND RoleId = '" & Trim(arRole.Id) & "'"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String

                lsMessage = "Error: TableRole.DeleteRole"
                lsMessage &= vbCrLf & "ModelId: " & arRole.Model.ModelId
                lsMessage &= vbCrLf & "RoleId: " & arRole.Id
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            End Try

        End Sub


        Public Function ExistsRole(ByVal arRole As FBM.Role) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelRole"
            lsSQLQuery &= " WHERE RoleId = '" & Trim(arRole.Id) & "'"
            lsSQLQuery &= "   AND ModelId = '" & Trim(arRole.Model.ModelId) & "'"


            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsRole = True
            Else
                ExistsRole = False
            End If

            lREcordset.Close()

        End Function

        Public Function GetRolesForModelFactType(ByRef arFactType As FBM.FactType,
                                                 Optional ByVal abAddToModel As Boolean = False,
                                                 Optional ByVal abSkipJoiningRoles As Boolean = False) As List(Of FBM.Role)
            '--------------------------------------------------------------------------------
            ' 
            'PRECONDITIONS
            '  * The EntityTypes for the Model have been loaded
            '  * The ValueTypes for the Model have been loaded
            '  * The FactTypes for the Model have been loaded
            '--------------------------------------------------------------------------------

            Dim lrRole As FBM.Role
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy
            Dim lsMessage As String = ""
            Dim lsId As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            Try
                '-----------------------------
                'Initialise the return value
                '-----------------------------
                GetRolesForModelFactType = New List(Of FBM.Role)

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM MetaModelRole"
                lsSQLQuery &= " WHERE FactTypeId = '" & Trim(arFactType.Id) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(arFactType.Model.ModelId) & "'"
                lsSQLQuery &= " ORDER BY SequenceNr"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrRole = New FBM.Role
                        lrRole.isDirty = False
                        lrRole.Model = arFactType.Model
                        lrRole.Id = Trim(lREcordset("RoleId").Value)
                        lrRole._Symbol = lrRole.Id
                        lrRole.Name = Viev.NullVal(lREcordset("RoleName").Value, "")
                        lrRole.SequenceNr = lREcordset("SequenceNr").Value
                        lrRole.Mandatory = lREcordset("IsMandatory").Value
                        lrRole.IsArray = lREcordset("IsArray").Value
                        lrRole.FactType = arFactType

                        lsId = Trim(Viev.NullVal(lREcordset("JoinsModelElementId").Value, ""))
                        lrRole.JoinsModelElementId = lsId

                        If abSkipJoiningRoles Then
                            'Should only be called when loading ModelElements and related FactTypes in the UnifiedOntologyBrowser.
                            Select Case lREcordset("TypeOfJoin").Value
                                Case Is = pcenumRoleJoinType.EntityType
                                    lrRole._TypeOfJoin = pcenumRoleJoinType.EntityType
                                Case Is = pcenumRoleJoinType.ValueType
                                    lrRole._TypeOfJoin = pcenumRoleJoinType.ValueType
                                Case Is = pcenumRoleJoinType.FactType
                                    lrRole._TypeOfJoin = pcenumRoleJoinType.FactType
                            End Select
                            lrRole.JoinedORMObject = Nothing
                            GoTo SkipJoiningRole 'As used by UnifiedOntologyBrowser. Dynamically loads ModelElements into Model.
                        End If

                        Select Case lREcordset("TypeOfJoin").Value
                            Case Is = pcenumRoleJoinType.FactType
                                lrRole.JoinedORMObject = arFactType.Model.FactType.Find(Function(x) x.Id = lsId)
                                '202020514-VM-Original pattern for the above as well
                                'Dim lrFactType As New FBM.FactType
                                'lrRole.JoinsFactType = New FBM.FactType
                                'lrRole.JoinsFactType.Id = Trim(Viev.NullVal(lREcordset("JoinsNestedFactTypeId").Value, ""))
                                'lrFactType.Id = lrRole.JoinsFactType.Id
                                'lrRole.JoinsFactType = arFactType.Model.FactType.Find(AddressOf lrFactType.Equals)
                                If lrRole.JoinedORMObject Is Nothing Then
                                    If lrRole.FactType.Id = lsId Then Exit Select
                                    lrRole.JoinedORMObject = New FBM.FactType(lrRole.Model, lsId, True) ' Trim(Viev.NullVal(lREcordset("JoinsNestedFactTypeId").Value, "")), True)
                                    Dim lrFactType = TableFactType.GetFactTypeDetailsByModel(lrRole.JoinsFactType, True)
                                    If lrFactType Is Nothing Then
                                        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                                        lsMessage.AppendDoubleLineBreak("Could not find linked Fact Type for Role on Fact Type, " & arFactType.Id & ".")
                                        lsMessage.AppendDoubleLineBreak("Click [Yes] if you would like Boston to try and remove this Fact Type from the Model.")
                                        Dim liMessageResponse As MsgBoxResult
                                        liMessageResponse = prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, , False, False, True, MessageBoxButtons.YesNo)

                                        If liMessageResponse = MsgBoxResult.Yes Then
                                            If arFactType.CanSafelyRemoveFromModel() Then
                                                Call TableFactType.DeleteFactType(arFactType)
                                                GoTo ContinueThoughFaulty
                                            End If
                                        End If
                                    End If
                                End If
                            Case Is = pcenumRoleJoinType.EntityType
                                lrRole.JoinedORMObject = arFactType.Model.EntityType.Find(Function(x) x.Id = lsId)
                            Case Is = pcenumRoleJoinType.ValueType
                                lrRole.JoinedORMObject = arFactType.Model.ValueType.Find(Function(x) x.Id = lsId)
                        End Select

ContinueThoughFaulty:
                        lrRole.isDirty = False

                        If (lrRole.JoinedORMObject Is Nothing) And Not (lrRole.TypeOfJoin = pcenumRoleJoinType.FactType) Then
                            Dim lsMissingId As String = ""
                            lsMissingId = Trim(Viev.NullVal(lREcordset("JoinsModelelementId").Value, ""))
                            lsMessage = "No ModelObject (" & lrRole.TypeOfJoin.ToString & ") with Id: '"
                            lsMessage &= lsMissingId
                            lsMessage &= "', exists in the Model"
                            lsMessage.AppendDoubleLineBreak("Model.Id: " & arFactType.Model.ModelId)
                            lsMessage &= vbCrLf & "FactType.Id: " & arFactType.Id
                            lsMessage &= vbCrLf & "Role.Id: " & lrRole.Id & vbCrLf

                            Select Case lREcordset("TypeOfJoin").Value
                                Case Is = 2
                                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(arFactType.Model, lsMissingId, pcenumConceptType.ValueType,,, True, True)
                                    If arFactType.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.EqualsLowercase) Is Nothing Then
                                        lsMessage &= vbCrLf & vbCrLf
                                        lsMessage &= "To keep the show on the road, a Value Type with Id: " & lsMissingId & " will be created in the model."
                                        Dim lrValueType As New FBM.ValueType(arFactType.Model, pcenumLanguage.ORMModel, lsMissingId, True)
                                        arFactType.Model.AddValueType(lrValueType, False, True, Nothing, True)
                                        Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(arFactType.Model, lsMissingId, pcenumConceptType.ValueType,,, True, True,)
                                        lrModelDictionaryEntry.isDirty = True
                                        Call lrModelDictionaryEntry.Save()
                                        lrValueType.Save()
                                        lrRole.JoinedORMObject = lrValueType
                                        MsgBox(lsMessage)
                                    Else
                                        lrRole.JoinedORMObject = arFactType.Model.ValueType.Find(Function(x) LCase(x.Id) = LCase(lsMissingId))
                                        Call lrRole.makeDirty()
                                        Call arFactType.makeDirty()
                                    End If
                                Case Is = pcenumRoleJoinType.EntityType
                                    lsMessage &= vbCrLf & vbCrLf
                                    lsMessage &= "To keep the show on the road, a Entity Type with Id: " & lsMissingId & " will be created in the model."
                                    Dim lrEntityType As New FBM.EntityType(arFactType.Model, pcenumLanguage.ORMModel, lsMissingId, lsMissingId, Nothing)
                                    arFactType.Model.AddEntityType(lrEntityType, True, True, Nothing, True)
                                    Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(arFactType.Model, lsMissingId, pcenumConceptType.EntityType,,, True, True,)
                                    lrModelDictionaryEntry.isDirty = True
                                    Call lrModelDictionaryEntry.Save()
                                    Call lrEntityType.Save()
                                    lrRole.JoinedORMObject = lrEntityType
                                    MsgBox(lsMessage)
                                Case Is = pcenumRoleJoinType.FactType
                                    Throw New Exception(lsMessage)
                            End Select
                        End If

                        '--------------------------------------------------
                        'Add the Role to the Model (list of Role) as well
                        '--------------------------------------------------
                        If abAddToModel Then
                            arFactType.Model.Role.Add(lrRole)
                        End If
SkipJoiningRole:
                        GetRolesForModelFactType.Add(lrRole)
                        lREcordset.MoveNext()
                    End While
                Else
                    GetRolesForModelFactType = Nothing
                    Throw New Exception("Error: There are no Roles for FactType with FactTypeId:" & arFactType.Id)
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage1 As String
                lsMessage1 = "TableRole.GetRolesForModelFactType:"
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                lsMessage1 &= vbCrLf & vbCrLf & "Loading Roles for "
                lsMessage1 &= vbCrLf & "  FactTypeId: '" & arFactType.Id & "' for"
                lsMessage1 &= vbCrLf & "  ModelId: '" & arFactType.Model.ModelId & "'"
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                GetRolesForModelFactType = New List(Of FBM.Role)
            End Try

        End Function

        Public Sub updateRole(ByVal arRole As FBM.Role)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = " UPDATE MetaModelRole"
                lsSQLQuery &= "   SET RoleName = '" & arRole.Name & "'"
                lsSQLQuery &= "       ,FactTypeId = '" & Replace(arRole.FactType.Id, "'", "`") & "'"
                lsSQLQuery &= "       ,PartOfKey = " & arRole.part_of_key
                lsSQLQuery &= "       ,Cardinality = " & arRole.FactType.Arity
                lsSQLQuery &= "       ,SequenceNr = " & arRole.SequenceNr

                lsSQLQuery &= "       ,JoinsModelElementId = '" & arRole.JoinedORMObject.Id & "'"

                'If IsSomething(arRole.JoinsEntityType) Then
                '    lsSQLQuery &= "       ,JoinsEntityTypeId = '" & Viev.NullVal(arRole.JoinsEntityType.Id, "") & "'"
                'Else
                '    lsSQLQuery &= "       ,JoinsEntityTypeId = ''"
                'End If
                'If IsSomething(arRole.JoinsValueType) Then
                '    lsSQLQuery &= "       ,JoinsValueTypeId = '" & Viev.NullVal(arRole.JoinsValueType.Id, "") & "'"
                'Else
                '    lsSQLQuery &= "       ,JoinsValueTypeId = ''"
                'End If
                'If IsSomething(arRole.JoinsFactType) Then
                '    lsSQLQuery &= "       ,JoinsNestedFactTypeId = '" & Viev.NullVal(arRole.JoinsFactType.Id, "") & "'"
                'Else
                '    lsSQLQuery &= "       ,JoinsNestedFactTypeId = ''"
                'End If

                lsSQLQuery &= "       ,TypeOfJoin = " & arRole.TypeOfJoin
                lsSQLQuery &= "       ,IsMandatory = " & arRole.Mandatory
                lsSQLQuery &= "       ,IsArray = " & arRole.IsArray
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arRole.Model.ModelId) & "'"
                lsSQLQuery &= "   AND RoleId = '" & Trim(arRole.Id) & "'"

                pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactType.UpdateRole"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "SQL: " & lsSQLQuery
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Module
End Namespace