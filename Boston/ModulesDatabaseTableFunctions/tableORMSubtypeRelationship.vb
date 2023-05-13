Imports System.Reflection

Namespace TableSubtypeRelationship

    Module Table_SubtypeRelationship

        Sub add_parentEntityType(ByVal arSubtypeRelationship As FBM.tSubtypeRelationship)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "INSERT INTO MetaModelSubtypeRelationship"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= "," & pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= " ,'" & Trim(arSubtypeRelationship.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(arSubtypeRelationship.ModelElement.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(arSubtypeRelationship.parentModelElement.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(arSubtypeRelationship.FactType.Id) & "'"
                lsSQLQuery &= " ," & arSubtypeRelationship.IsPrimarySubtypeRelationship
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteParentEntityType(ByVal arSubtype As FBM.tSubtypeRelationship)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelSubtypeRelationship"
            lsSQLQuery &= " WHERE ModelId = '" & arSubtype.Model.ModelId & "'"
            lsSQLQuery &= "   AND ObjectTypeId = '" & arSubtype.ModelElement.Id & "'"
            lsSQLQuery &= "   AND SupertypeObjectTypeId = '" & arSubtype.parentModelElement.Id & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function exists_parentEntityType(ByVal arSubtypeRelationship As FBM.tSubtypeRelationship) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            Try
                '------------------------
                'Initialise return value
                '------------------------
                exists_parentEntityType = False

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelSubtypeRelationship"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arSubtypeRelationship.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ObjectTypeId = '" & Trim(arSubtypeRelationship.ModelElement.Id) & "'"
                lsSQLQuery &= "   AND SupertypeObjectTypeId = '" & Trim(arSubtypeRelationship.parentModelElement.Id) & "'"


                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    exists_parentEntityType = True
                Else
                    exists_parentEntityType = False
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

        Public Function GetSubtypeRelationshipsByModel(ByRef arModel As FBM.Model) As List(Of FBM.tSubtypeRelationship)

            Dim lrEntityType As FBM.EntityType
            Dim lrParentEntityType As FBM.EntityType
            Dim lsSQLQuery As String
            Dim lrRecordset As New RecordsetProxy
            Dim lsId As String

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetSubtypeRelationshipsByModel = New List(Of FBM.tSubtypeRelationship)

            Try

                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = pcOpenStatic

                '------------------------------------------------------
                'Now Link the EntityTypes to their Parent EntityTypes
                '------------------------------------------------------
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM MetaModelSubtypeRelationship"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arModel.ModelId) & "'"

                lrRecordset.Open(lsSQLQuery)


                While Not lrRecordset.EOF
                    lsId = Trim(lrRecordset("ObjectTypeId").Value)
                    lrEntityType = arModel.EntityType.Find(Function(x) x.Id = lsId)
                    lsId = Trim(lrRecordset("SupertypeObjectTypeId").Value)
                    lrParentEntityType = arModel.EntityType.Find(Function(x) x.Id = lsId)
                    lrEntityType.parentModelObjectList.Add(lrParentEntityType)
                    lrParentEntityType.childModelObjectList.Add(lrEntityType)

                    Dim lrFactType As FBM.FactType
                    lsId = Trim(lrRecordset("SubtypingFactTypeId").Value)
                    lrFactType = arModel.FactType.Find(Function(x) x.Id = lsId)

                    Dim lrSubtypeRelationship As New FBM.tSubtypeRelationship(lrEntityType, lrParentEntityType, lrFactType)
                    lrSubtypeRelationship.isDirty = False
                    lrSubtypeRelationship.IsPrimarySubtypeRelationship = CBool(lrRecordset("IsPrimarySubtypeRelationship").Value)
                    lrEntityType.SubtypeRelationship.Add(lrSubtypeRelationship)

                    GetSubtypeRelationshipsByModel.Add(lrSubtypeRelationship)

                    lrRecordset.MoveNext()
                End While

                lrRecordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function GetSubtypeRelationshipsForModelElementByModel(ByRef arSubtypeModelElement As FBM.ModelObject,
                                                                      ByVal abAddToSubtypeModelElement As Boolean) As List(Of FBM.tSubtypeRelationship)

            Dim lrSupertypeModelElement As FBM.ModelObject
            Dim lsSQLQuery As String
            Dim lrRecordset As New RecordsetProxy
            Dim lsSubtypeModelElementId As String = arSubtypeModelElement.Id
            Dim lsId As String
            Dim lsMessage As String

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetSubtypeRelationshipsForModelElementByModel = New List(Of FBM.tSubtypeRelationship)

            Try
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = pcOpenStatic

                '------------------------------------------------------
                'Now Link the EntityTypes to their Parent EntityTypes
                '------------------------------------------------------
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM MetaModelSubtypeRelationship"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arSubtypeModelElement.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ObjectTypeId = '" & Trim(arSubtypeModelElement.Id) & "'"

                lrRecordset.Open(lsSQLQuery)

                While Not lrRecordset.EOF
                    lsId = Trim(lrRecordset("ObjectTypeId").Value)
                    lsId = Trim(lrRecordset("SupertypeObjectTypeId").Value)
                    lrSupertypeModelElement = arSubtypeModelElement.Model.GetModelObjectByName(lsId)
                    If lrSupertypeModelElement Is Nothing Then
                        Try
                            Dim lrModelDictionaryEntry As FBM.DictionaryEntry = arSubtypeModelElement.Model.ModelDictionary.Find(Function(x) x.Symbol = lsSubtypeModelElementId)
                            lrSupertypeModelElement = arSubtypeModelElement.Model.LoadModelElementById(lrModelDictionaryEntry.GetModelObjectConceptType, lsId)
                            Call TableSubtypeRelationship.GetSubtypeRelationshipsForModelElementByModel(lrSupertypeModelElement, True)
                        Catch ex As Exception
                            lsMessage = "Problems finding Supertype Model Element details in the Model Dictionary"
                            lsMessage.AppendDoubleLineBreak("Supertype Model Element Id: " & lsId)
                            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False, False, True)
                            GoTo SkipSubtypeRelationship
                        End Try

                    End If
                    arSubtypeModelElement.parentModelObjectList.Add(lrSupertypeModelElement)
                    lrSupertypeModelElement.childModelObjectList.Add(arSubtypeModelElement)

                    Dim lrFactType As FBM.FactType
                    lsId = Trim(lrRecordset("SubtypingFactTypeId").Value)
                    lrFactType = arSubtypeModelElement.Model.FactType.Find(Function(x) x.Id = lsId)

                    If lrFactType Is Nothing Then
                        lrFactType = New FBM.FactType(arSubtypeModelElement.Model, lsId, True)
                        lrFactType = TableFactType.GetFactTypeDetailsByModel(lrFactType, True, True)
                    End If

                    Dim lrSubtypeRelationship As New FBM.tSubtypeRelationship(arSubtypeModelElement, lrSupertypeModelElement, lrFactType)
                    lrSubtypeRelationship.isDirty = False
                    lrSubtypeRelationship.IsPrimarySubtypeRelationship = CBool(lrRecordset("IsPrimarySubtypeRelationship").Value)

                    If abAddToSubtypeModelElement Then
                        arSubtypeModelElement.SubtypeRelationship.Add(lrSubtypeRelationship)
                    End If

                    GetSubtypeRelationshipsForModelElementByModel.Add(lrSubtypeRelationship)
SkipSubtypeRelationship:
                    lrRecordset.MoveNext()
                End While

                lrRecordset.Close()

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        ''' <summary>
        ''' Gets the SubtypeInstances by Page.
        ''' NB Only returns those where both the EntityType and ParentEntityType are both on the Page.
        ''' </summary>
        ''' <param name="arPage"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSubtypeInstancesByPage(ByRef arPage As FBM.Page) As List(Of FBM.SubtypeRelationshipInstance)

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            Dim lrParentEntityTypeInstance As FBM.EntityTypeInstance
            Dim lsMessage As String
            Dim lrRecordset As New RecordsetProxy
            Dim lsSQLQuery As String
            Dim lsId As String

            Try
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = pcOpenStatic

                '-----------------------------
                'Initialise the return value
                '-----------------------------
                GetSubtypeInstancesByPage = New List(Of FBM.SubtypeRelationshipInstance)

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM MetaModelSubtypeRelationship"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arPage.Model.ModelId) & "'"

                lrRecordset.Open(lsSQLQuery)

                If Not lrRecordset.EOF Then
                    While Not lrRecordset.EOF
                        lsId = Trim(lrRecordset("ObjectTypeId").Value)
                        lrEntityTypeInstance = arPage.EntityTypeInstance.Find(Function(x) x.Id = lsId)

                        If IsSomething(lrEntityTypeInstance) Then
                            '----------------------------------------------------------------------
                            'The ParentEntityType is at least part of the Page/model under review
                            '  i.e. If currently looking at an ORM model...is within the ORM model
                            '----------------------------------------------------------------------
                            lsId = Trim(lrRecordset("SupertypeObjectTypeId").Value)
                            lrParentEntityTypeInstance = arPage.EntityTypeInstance.Find(Function(x) x.Id = lsId)

                            If IsSomething(lrParentEntityTypeInstance) Then
                                Dim lrSubtypeRelationship As New FBM.tSubtypeRelationship
                                lrSubtypeRelationship.ModelElement = lrEntityTypeInstance.EntityType
                                lrSubtypeRelationship.parentModelElement = lrParentEntityTypeInstance.EntityType

                                lrSubtypeRelationship = lrEntityTypeInstance.EntityType.SubtypeRelationship.Find(AddressOf lrSubtypeRelationship.Equals)
                                lrSubtypeRelationship.IsPrimarySubtypeRelationship = CBool(lrRecordset("IsPrimarySubtypeRelationship").Value)

                                Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance

                                lrSubtypeRelationshipInstance = lrSubtypeRelationship.CloneInstance(arPage, True)

                                lrEntityTypeInstance.SubtypeRelationship.Add(lrSubtypeRelationshipInstance)
                            End If
                        End If
                        lrRecordset.MoveNext()
                    End While
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.SubtypeRelationshipInstance)
            End Try

        End Function

        Public Sub ModifyKey(ByVal ar_entity_type As FBM.EntityType, ByVal as_new_key As String)

            Dim lsSQLQuery As String

            lsSQLQuery = " UPDATE MetaModelSubtypeRelationship"
            lsSQLQuery &= "   SET ObjectTypeId = '" & Trim(as_new_key) & "'"
            lsSQLQuery &= " WHERE ModelId = '" & ar_entity_type.Model.ModelId & "'"
            lsSQLQuery &= "   AND ObjectTypeId = '" & ar_entity_type.Id & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

            lsSQLQuery = " UPDATE MetaModelSubtypeRelationship"
            lsSQLQuery &= "   SET SupertypeObjectTypeId = '" & Trim(as_new_key) & "'"
            lsSQLQuery &= " WHERE ModelId = '" & ar_entity_type.Model.ModelId & "'"
            lsSQLQuery &= "   AND SupertypeObjectTypeId = '" & ar_entity_type.Id & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()


        End Sub

        Public Sub update_parentEntityType(ByVal arSubtypeRelationship As FBM.tSubtypeRelationship)

            '--------------------------------------------------------------------------------------------------------
            'Updates to itself at the moment, but is placeholder for the future if more attributes are added to the
            '  representation of a SubType join 
            '--------------------------------------------------------------------------------------------------------

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = " UPDATE MetaModelSubtypeRelationship"
                lsSQLQuery &= "   SET ObjectTypeId = '" & Trim(arSubtypeRelationship.ModelElement.Id) & "'"
                lsSQLQuery &= "       ,SupertypeObjectTypeId = '" & Trim(arSubtypeRelationship.parentModelElement.Id) & "'"
                lsSQLQuery &= "       ,IsPrimarySubtypeRelationship = " & arSubtypeRelationship.IsPrimarySubtypeRelationship
                lsSQLQuery &= " WHERE ObjectTypeId = '" & Trim(arSubtypeRelationship.ModelElement.Id) & "'"
                lsSQLQuery &= "   AND SupertypeObjectTypeId = '" & Trim(arSubtypeRelationship.parentModelElement.Id) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module
End Namespace