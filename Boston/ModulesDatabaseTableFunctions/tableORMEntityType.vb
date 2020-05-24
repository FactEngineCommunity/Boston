Imports System.Reflection

Namespace TableEntityType

    Module Table_entity_type

        Sub AddEntityType(ByVal arEntityType As FBM.EntityType)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "INSERT INTO MetaModelEntityType"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(arEntityType.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(arEntityType.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arEntityType.Name, "'", "`")) & "'"
                If IsSomething(arEntityType.ReferenceModeValueType) Then
                    lsSQLQuery &= " ,'" & Trim(arEntityType.ReferenceModeValueType.Id) & "'"
                Else
                    lsSQLQuery &= " ,''"
                End If
                lsSQLQuery &= " ,'" & Trim(arEntityType.ReferenceMode) & "'"
                If IsSomething(arEntityType.ReferenceModeRoleConstraint) Then
                    lsSQLQuery &= " ,'" & Trim(arEntityType.ReferenceModeRoleConstraint.Id) & "'"
                Else
                    lsSQLQuery &= " ,''"
                End If
                lsSQLQuery &= "," & arEntityType.IsMDAModelElement                
                lsSQLQuery &= "," & arEntityType.IsPersonal
                lsSQLQuery &= ",'" & Trim(arEntityType.GUID) & "'"
                lsSQLQuery &= "," & arEntityType.IsAbsorbed
                lsSQLQuery &= "," & arEntityType.IsIndependent
                lsSQLQuery &= "," & arEntityType.IsDerived
                lsSQLQuery &= ",'" & Trim(Replace(arEntityType.DerivationText, "'", "''")) & "'"
                lsSQLQuery &= ")"


                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableEntityType.AddEntityType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteEntityType(ByVal arEntityType As FBM.EntityType)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelEntityType"
            lsSQLQuery &= " WHERE EntityTypeId = '" & arEntityType.Id & "'"
            lsSQLQuery &= "   AND ModelId = '" & arEntityType.Model.ModelId & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsEntityType(ByVal arEntityType As FBM.EntityType) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                '------------------------
                'Initialise return value
                '------------------------
                ExistsEntityType = False

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelEntityType ET"
                lsSQLQuery &= " WHERE EntityTypeId = '" & Trim(arEntityType.Id) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(arEntityType.Model.ModelId) & "'"



                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsEntityType = True
                Else
                    ExistsEntityType = False
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

        Public Function ExistsEntityTypeByModel(ByVal arEntityType As FBM.EntityType) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                '------------------------
                'Initialise return value
                '------------------------
                ExistsEntityTypeByModel = False

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelEntityType ET,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE ET.EntityTypeId = '" & Trim(arEntityType.Id) & "'"
                lsSQLQuery &= "   AND ET.EntityTypeId = MD.Symbol"
                lsSQLQuery &= "   AND ET.ModelId = MD.ModelId"
                lsSQLQuery &= "   AND MD.ModelId = '" & Trim(arEntityType.Model.ModelId) & "'"


                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsEntityTypeByModel = True
                Else
                    ExistsEntityTypeByModel = False
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

        Public Function ExistsEntityTypeIdByName(ByVal arEntityType As FBM.EntityType) As Boolean
            '------------------------------------------------------------------------------------------------------------
            'Used to check to see if it is okay to use the Name of an EntityType as the EntityTypeId for the EntityType
            '  itself. See below.
            '------------------------------------------------------------------------------------------------------------
            'Richmond is based on the philosophy that a 'Concept' 'is' its own identity.
            '  e.g. The concept "Napoleon", only has meaning within the context of a Model.
            '  e.g. Within one Model, "Napoleon" may represent Napoleon Bonaparte, and within another Model, "Napoleon"
            '         may represent the name of a dog.
            '  Under this philosophy, any Concept (e.g. an Entity or Entity Type) is only declared once, and it doesn't
            '  hurt to have the EntityTypeId (i.e. the surrogate key) of an EntityType as exactly the same as the Name
            '  of the EntityType (e.g. EntityId of an EntityType named "Person", with EntityTypeId as "Person".)
            '  - It makes no difference to the result of modeling in Richmond, and makes debugging the database easier.
            '  - It is also a philosophy which answers an age old question, "What is identity?".
            '  - Identity is the formulated meaning of a Concept within the model formulated or simulated within the
            '  interpreter (e.g. If I say "Napoleon" and intend the meaning of the man, and you interpret "Napoleon"
            '  as the name of a dog; then the identity of the Napoleon that I picture (frame within a Model in my mind)
            '  is different from the identity of the Napoleon that you picture (frame within a Model in your mind).
            '  - In terms of Object Identity, Richmond only allows the allocation of a Concept to a Model 'once', 
            '  so that Object Identity is preserved within any one Model under review within Richmond.
            '------------------------------------------------------------------------------------------------------------
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '------------------------
            'Initialise return value
            '------------------------
            ExistsEntityTypeIdByName = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelEntityType"
            lsSQLQuery &= " WHERE EntityTypeId = '" & Trim(arEntityType.Name) & "'"


            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsEntityTypeIdByName = True
            Else
                ExistsEntityTypeIdByName = False
            End If

            lREcordset.Close()

        End Function

        Function GetEntityTypeCountByModel(ByVal as_ModelId As String) As Integer


            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelEntityType ET,"
            lsSQLQuery &= "       MetaModelModelDictionary MD"
            lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(as_ModelId) & "'"
            lsSQLQuery &= "   AND MD.Symbol = ET.EntityTypeId"
            lsSQLQuery &= "   AND MD.ModelId = ET.ModelId"


            lREcordset.Open(lsSQLQuery)

            GetEntityTypeCountByModel = lREcordset(0).Value

            lREcordset.Close()

        End Function

        Sub GetEntityTypeDetails(ByRef arEntityType As FBM.EntityType)

            Try
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM MetaModelEntityType"
                lsSQLQuery &= " WHERE EntityTypeId = '" & Trim(arEntityType.Id) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(arEntityType.Model.ModelId) & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    arEntityType.Id = arEntityType.Id
                    arEntityType.Name = lREcordset("EntityTypeName").Value
                    arEntityType.ReferenceMode = Viev.NullVal(lREcordset("ReferenceMode").Value, "")
                    arEntityType.PreferredIdentifierRCId = Trim(Viev.NullVal(lREcordset("PreferredIdentifierRCId").Value, ""))
                    arEntityType.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                    arEntityType.IsIndependent = CBool(lREcordset("IsIndependent").Value)
                    arEntityType.IsPersonal = CBool(lREcordset("IsPersonal").Value)
                    arEntityType.GUID = Trim(lREcordset("GUID").Value)
                    arEntityType.IsAbsorbed = CBool(lREcordset("IsAbsorbed").Value)
                    arEntityType.IsDerived = CBool(lREcordset("IsDerived").Value)
                    arEntityType.DerivationText = Trim(Viev.NullVal(lREcordset("DerivationText").Value, ""))
                    arEntityType.isDirty = False
                Else
                    MsgBox("Error: GetEntityTypeDetailsById: No Project returned for EntityTypeId: " & arEntityType.Id)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function getEntityTypesByModel(ByRef arModel As FBM.Model) As List(Of FBM.EntityType)

            Dim lsMessage As String = ""
            Dim lrEntityType As FBM.EntityType
            Dim lb_at_least_one_entity_type_found As Boolean = False 'Safeguard. Usually we don't require this type of simplistic Boolean, however
            'the two sets of SQL make it easier to manage this way. Leave here for this function.
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Dim lsId As String

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            getEntityTypesByModel = New List(Of FBM.EntityType)

            Try

                '------------------------------------------------------
                'First get all the EntityTypes in the model
                '  without regard for links to their ParentEntityType
                '------------------------------------------------------
                lsSQLQuery = " SELECT ET.*, MD.*"
                lsSQLQuery &= "  FROM MetaModelEntityType ET,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arModel.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = ET.EntityTypeId"
                lsSQLQuery &= "   AND MD.ModelId = ET.ModelId"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    lb_at_least_one_entity_type_found = True
                    While Not lREcordset.EOF
                        lrEntityType = New FBM.EntityType
                        lrEntityType.isDirty = False
                        lrEntityType.Model = arModel
                        lrEntityType.Id = lREcordset("EntityTypeId").Value
                        lrEntityType.Name = lREcordset("EntityTypeName").Value
                        lrEntityType.ReferenceMode = Viev.NullVal(lREcordset("ReferenceMode").Value, "")
                        lrEntityType.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                        lrEntityType.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
                        If Viev.NullVal(lREcordset("ValueTypeId").Value, "") = "" Then
                            lrEntityType.ReferenceModeValueType = Nothing
                        Else
                            lsId = Trim(Viev.NullVal(lREcordset("ValueTypeId").Value, ""))
                            lrEntityType.ReferenceModeValueType = arModel.ValueType.Find(Function(x) x.Id = lsId)
                        End If
                        lrEntityType.PreferredIdentifierRCId = Trim(Viev.NullVal(lREcordset("PreferredIdentifierRCId").Value, ""))
                        lrEntityType.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                        lrEntityType.IsIndependent = CBool(lREcordset("IsIndependent").Value)
                        lrEntityType.IsPersonal = CBool(lREcordset("IsPersonal").Value)
                        lrEntityType.GUID = NullVal(lREcordset("GUID").Value, System.Guid.NewGuid.ToString)
                        lrEntityType.IsAbsorbed = CBool(lREcordset("IsAbsorbed").Value)
                        lrEntityType.IsDerived = CBool(lREcordset("IsDerived").Value)
                        lrEntityType.DerivationText = Trim(Viev.NullVal(lREcordset("DerivationText").Value, ""))

                        '------------------------------------------------
                        'Link to the Concept within the ModelDictionary
                        '------------------------------------------------
                        Dim lrDictionaryEntry As New FBM.DictionaryEntry(arModel, lrEntityType.Name, pcenumConceptType.EntityType)
                        lrDictionaryEntry = arModel.AddModelDictionaryEntry(lrDictionaryEntry, True, False)


                        If lrDictionaryEntry Is Nothing Then
                            lsMessage = "Cannot find DictionaryEntry in the ModelDictionary for FactType:"
                            lsMessage &= vbCrLf & "Model.Id: " & arModel.ModelId
                            lsMessage &= vbCrLf & "FactType.Id: " & lrEntityType.Id
                            Throw New Exception(lsMessage)
                        End If

                        lrEntityType.Concept = lrDictionaryEntry.Concept


                        getEntityTypesByModel.Add(lrEntityType)
                        lREcordset.MoveNext()
                    End While
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

        Public Function GetParentEntityTypesForEntityType(ByVal arEntityType As FBM.EntityType) As List(Of FBM.EntityType)

            Dim lrEntityType As New FBM.EntityType
            Dim lr_parentEntityType As New FBM.EntityType
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetParentEntityTypesForEntityType = New List(Of FBM.EntityType)

            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM MetaModelSubtypeRelationship"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arEntityType.Model.ModelId) & "'"
            lsSQLQuery &= "   AND ObjectTypeId = '" & arEntityType.Id & "'"


            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then

                While Not lREcordset.EOF
                    lrEntityType = New FBM.EntityType
                    lrEntityType.Model = arEntityType.Model
                    lrEntityType.Id = lREcordset("SupertypeObjectTypeId").Value
                    Call GetEntityTypeDetails(lrEntityType)

                    GetParentEntityTypesForEntityType.Add(lrEntityType)
                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        End Function

        Public Sub ModifyKey(ByVal arEntityType As FBM.EntityType, ByVal asNewKey As String)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelEntityType"
                lsSQLQuery &= "   SET EntityTypeId = '" & Replace(Trim(asNewKey), "'", "`") & "'"
                lsSQLQuery &= " WHERE EntityTypeId = '" & arEntityType.Id & "'"
                lsSQLQuery &= "   AND ModelId = '" & arEntityType.Model.ModelId & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

                lsSQLQuery = " UPDATE MetaModelSubtypeRelationship"
                lsSQLQuery &= "   SET ObjectTypeId = '" & Trim(Replace(asNewKey, "'", "`")) & "'"
                lsSQLQuery &= " WHERE ObjectTypeId = '" & arEntityType.Id & "'"
                lsSQLQuery &= "   AND ModelId = '" & arEntityType.Model.ModelId & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

                lsSQLQuery = " UPDATE MetaModelSubtypeRelationship"
                lsSQLQuery &= "   SET SupertypeObjectTypeId = '" & Trim(Replace(asNewKey, "'", "`")) & "'"
                lsSQLQuery &= " WHERE SupertypeObjectTypeId = '" & arEntityType.Id & "'"
                lsSQLQuery &= "   AND ModelId = '" & arEntityType.Model.ModelId & "'"

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

        Public Sub UpdateEntityType(ByVal arEntityType As FBM.EntityType)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelEntityType"
                lsSQLQuery &= "   SET EntityTypeName = '" & Trim(Replace(arEntityType.Name, "'", "`")) & "'"
                If IsSomething(arEntityType.ReferenceModeValueType) Then
                    lsSQLQuery &= "       ,ValueTypeId = '" & Viev.NullVal(arEntityType.ReferenceModeValueType.Id, "") & "'"
                Else
                    lsSQLQuery &= "       ,ValueTypeId = ''"
                End If
                lsSQLQuery &= "       ,ReferenceMode = '" & Viev.NullVal(arEntityType.ReferenceMode, "") & "'"
                lsSQLQuery &= "       ,PreferredIdentifierRCId = '" & Viev.NullVal(arEntityType.PreferredIdentifierRCId, "") & "'"
                lsSQLQuery &= "       ,IsMDAModelElement = " & arEntityType.IsMDAModelElement
                lsSQLQuery &= "       ,IsIndependent = " & arEntityType.IsIndependent
                lsSQLQuery &= "       ,IsPersonal = " & arEntityType.IsPersonal
                lsSQLQuery &= "       ,[GUID] = '" & Trim(arEntityType.GUID) & "'"
                lsSQLQuery &= "       ,IsAbsorbed = " & arEntityType.IsAbsorbed
                lsSQLQuery &= "       ,IsDerived = " & arEntityType.IsDerived
                lsSQLQuery &= "       ,DerivationText = '" & Replace(arEntityType.DerivationText, "'", "''") & "'"
                lsSQLQuery &= " WHERE EntityTypeId = '" & Trim(arEntityType.Id) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(arEntityType.Model.ModelId) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableEntityType.UpdateEntityType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module

End Namespace