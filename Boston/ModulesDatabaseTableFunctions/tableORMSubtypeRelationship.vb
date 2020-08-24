Imports System.Reflection

Namespace TableSubtypeRelationship

    Module Table_SubtypeRelationship

        Sub add_parentEntityType(ByVal ar_subtype_constraint As FBM.tSubtypeRelationship)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "INSERT INTO MetaModelSubtypeRelationship"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(ar_subtype_constraint.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_subtype_constraint.EntityType.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_subtype_constraint.parentEntityType.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_subtype_constraint.FactType.Id) & "'"
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
            lsSQLQuery &= "   AND ObjectTypeId = '" & arSubtype.EntityType.Id & "'"
            lsSQLQuery &= "   AND SupertypeObjectTypeId = '" & arSubtype.parentEntityType.Id & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function exists_parentEntityType(ByVal ar_subtype_constraint As FBM.tSubtypeRelationship) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                '------------------------
                'Initialise return value
                '------------------------
                exists_parentEntityType = False

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelSubtypeRelationship"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(ar_subtype_constraint.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ObjectTypeId = '" & Trim(ar_subtype_constraint.EntityType.Id) & "'"
                lsSQLQuery &= "   AND SupertypeObjectTypeId = '" & Trim(ar_subtype_constraint.parentEntityType.Id) & "'"


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
            Dim lrRecordset As New ADODB.Recordset
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

                    Dim lrSubtypeConstraint As New FBM.tSubtypeRelationship(lrEntityType, lrParentEntityType, lrFactType)
                    lrSubtypeConstraint.isDirty = False

                    lrEntityType.SubtypeRelationship.Add(lrSubtypeConstraint)

                    GetSubtypeRelationshipsByModel.Add(lrSubtypeConstraint)

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
            Dim lrRecordset As New ADODB.Recordset
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
                                Dim lrSubtypeConstraint As New FBM.tSubtypeRelationship
                                lrSubtypeConstraint.EntityType = lrEntityTypeInstance.EntityType
                                lrSubtypeConstraint.parentEntityType = lrParentEntityTypeInstance.EntityType

                                lrSubtypeConstraint = lrEntityTypeInstance.EntityType.SubtypeRelationship.Find(AddressOf lrSubtypeConstraint.Equals)

                                Dim lrSubtypeConstraintInstance As FBM.SubtypeRelationshipInstance

                                lrSubtypeConstraintInstance = lrSubtypeConstraint.CloneInstance(arPage, True)

                                lrEntityTypeInstance.SubtypeRelationship.Add(lrSubtypeConstraintInstance)
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

        Public Sub update_parentEntityType(ByVal ar_subtype_constraint As FBM.tSubtypeRelationship)

            '--------------------------------------------------------------------------------------------------------
            'Updates to itself at the moment, but is placeholder for the future if more attributes are added to the
            '  representation of a SubType join 
            '--------------------------------------------------------------------------------------------------------

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = " UPDATE MetaModelSubtypeRelationship"
                lsSQLQuery &= "   SET ObjectTypeId = '" & Trim(ar_subtype_constraint.EntityType.Id) & "'"
                lsSQLQuery &= "       ,SupertypeObjectTypeId = '" & Trim(ar_subtype_constraint.parentEntityType.Id) & "'"
                lsSQLQuery &= " WHERE ObjectTypeId = '" & Trim(ar_subtype_constraint.EntityType.Id) & "'"
                lsSQLQuery &= "   AND SupertypeObjectTypeId = '" & Trim(ar_subtype_constraint.parentEntityType.Id) & "'"

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