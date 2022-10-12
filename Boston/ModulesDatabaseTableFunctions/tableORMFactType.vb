Imports System.Reflection

Namespace TableFactType

    Module TableFactType

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arFactType"></param>
        Sub AddFactType(ByRef arFactType As FBM.FactType)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelFactType"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactType.Model.ModelId, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactType.Id, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactType.Name, "'", "`")) & "'"
                lsSQLQuery &= " ," & arFactType.IsObjectified
                lsSQLQuery &= " ," & arFactType.IsCoreFactType
                lsSQLQuery &= " ," & arFactType.IsPreferredReferenceMode
                If IsSomething(arFactType.ObjectifyingEntityType) Then
                    lsSQLQuery &= " ,'" & Trim(arFactType.ObjectifyingEntityType.Id) & "'"
                Else
                    lsSQLQuery &= " ,''"
                End If
                lsSQLQuery &= " ," & arFactType.IsSubtypeRelationshipFactType
                lsSQLQuery &= " ," & arFactType.IsMDAModelElement
                lsSQLQuery &= " ," & arFactType.IsDerived
                lsSQLQuery &= " ," & arFactType.IsStored
                lsSQLQuery &= " ,'" & Trim(Replace(arFactType.DerivationText, "'", "`")) & "'"
                lsSQLQuery &= " ," & arFactType.IsLinkFactType
                lsSQLQuery &= " ,'" & arFactType.GUID & "'"
                If arFactType.LinkFactTypeRole IsNot Nothing Then
                    lsSQLQuery &= " ,'" & arFactType.LinkFactTypeRole.Id & "'"
                Else
                    lsSQLQuery &= " ,''"
                End If
                lsSQLQuery &= " ," & arFactType.IsIndependent
                lsSQLQuery &= " ," & arFactType.IsSubtypeStateControlling
                lsSQLQuery &= " ," & arFactType.StoreFactCoordinates
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

        Sub DeleteFactType(ByVal arFactType As FBM.FactType)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "DELETE FROM MetaModelFactType"
                lsSQLQuery &= " WHERE FactTypeId = '" & Replace(arFactType.Id, "'", "`") & "'"
                lsSQLQuery &= "   AND ModelId = '" & Replace(arFactType.Model.ModelId, "'", "`") & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactType.DeleteFactType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Sub DeleteFactTypesByModel(ByVal arModel As FBM.Model)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "DELETE FROM MetaModelFactType"
                lsSQLQuery &= " WHERE ModelId = '" & Replace(arModel.ModelId, "'", "`") & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Function ExistsFactType(ByVal arFactType As FBM.FactType) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelFactType FT"
                lsSQLQuery &= " WHERE FT.FactTypeId = '" & Trim(Replace(arFactType.Id, "'", "`")) & "'"
                lsSQLQuery &= "   AND FT.ModelId = '" & Trim(Replace(arFactType.Model.ModelId, "'", "`")) & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsFactType = True
                Else
                    ExistsFactType = False
                End If

                lREcordset.Close()
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactType.ExistsFactType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Function ExistsFactTypeByModel(ByVal arFactType As FBM.FactType) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelFactType FT,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arFactType.Model.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = FT.FactTypeId"
                lsSQLQuery &= "   AND MD.ModelId = FT.ModelId"
                lsSQLQuery &= "   AND FT.FactTypeId = '" & Trim(Replace(arFactType.Id, "'", "`")) & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsFactTypeByModel = True
                Else
                    ExistsFactTypeByModel = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        Function ExistsFactTypeByAnyModel(ByVal arFactType As FBM.FactType) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelFactType FT,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.Symbol = FT.FactTypeId"
                lsSQLQuery &= " WHERE MD.ModelId = FT.ModelId"
                lsSQLQuery &= "   AND FT.FactTypeId = '" & Trim(Replace(arFactType.Id, "'", "`")) & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsFactTypeByAnyModel = True
                Else
                    ExistsFactTypeByAnyModel = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        ''' <summary>
        ''' Gets the details of a FactType from the Model.
        ''' </summary>
        ''' <param name="arFactType">The FactType to load.</param>
        ''' <param name="abAddFactTypeToModel">True if the FactType is to be added to the Model.</param>
        ''' <param name="abDynamicallyLoadRelatedModelElements">As used by the Unified Ontology Browser so the whole Model doesn't have to be loaded at once.</param>
        Public Function GetFactTypeDetailsByModel(ByRef arFactType As FBM.FactType,
                                                  Optional ByVal abAddFactTypeToModel As Boolean = False,
                                                  Optional ByVal abDynamicallyLoadRelatedModelElements As Boolean = False,
                                                  Optional ByRef abAborted As Boolean = False) As FBM.FactType

            Dim lsMessage As String
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= "  FROM MetaModelFactType FT,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arFactType.Model.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = FT.FactTypeId"
                lsSQLQuery &= "   AND MD.ModelId = FT.ModelId"
                lsSQLQuery &= "   AND FT.FactTypeId = '" & Trim(arFactType.Id) & "'"


                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    arFactType._Symbol = lREcordset("FactTypeId").Value
                    arFactType.Id = lREcordset("FactTypeId").Value
                    arFactType.Name = lREcordset("FactTypeName").Value
                    arFactType.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                    arFactType.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
                    arFactType.IsObjectified = CBool(lREcordset("IsObjectified").Value)
                    arFactType.IsCoreFactType = CBool(lREcordset("IsCoreFactType").Value)
                    arFactType.IsPreferredReferenceMode = CBool(lREcordset("IsPreferredReferenceMode").Value)
                    arFactType.IsSubtypeRelationshipFactType = CBool(lREcordset("IsSubtypeFactType").Value)
                    arFactType.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                    arFactType.IsDerived = CBool(lREcordset("IsDerived").Value)
                    arFactType.IsStored = CBool(lREcordset("IsStored").Value)
                    arFactType.DerivationText = NullVal(lREcordset("DerivationText").Value, "")
                    arFactType.IsLinkFactType = CBool(lREcordset("IsLinkFactType").Value)
                    If arFactType.IsLinkFactType Then
                        arFactType.LinkFactTypeRole = arFactType.Model.Role.Find(Function(x) x.Id = NullVal(lREcordset("LinkFactTypeRoleId").Value, ""))
                        If arFactType.LinkFactTypeRole Is Nothing Then
                            arFactType.IsLinkFactType = False
                        ElseIf Not arFactType.LinkFactTypeRole.FactType.IsObjectified Then
                            '20220808-VM-Keep here for further testing.
                            'Debugger.Break()
                        End If
                    End If
                    arFactType.GUID = lREcordset("GUID").Value
                    arFactType.IsIndependent = CBool(lREcordset("IsIndependent").Value)
                    arFactType.IsSubtypeStateControlling = CBool(lREcordset("IsSubtypeStateControlling").Value)
                    arFactType.StoreFactCoordinates = CBool(lREcordset("StoreFactCoordinates").Value)
                    arFactType.isDirty = False

                    '---------------------------------------------------------------------------------
                    'Dynamically load related ModelElements (ValueTypes,EntityTypes) if required.
                    '-----------------------------------------------------------------------------
                    If abDynamicallyLoadRelatedModelElements Then Call arFactType.LoadRelatedModelElementsToModel()

                    '------------------------------------------------------------
                    'Get the Roles within the RoleGroup for the FactType as well
                    '------------------------------------------------------------
                    arFactType.RoleGroup = TableRole.GetRolesForModelFactType(arFactType, abAddFactTypeToModel)

                    '------------------------------------------------------------
                    'Get the FactTypeReadings for the FactType
                    '------------------------------------------------------------
                    arFactType.FactTypeReading = TableFactTypeReading.GetFactTypeReadingsForFactType(arFactType)

                    '----------------------------------------------
                    'Get the Facts (FactTypeData) for the FactType
                    '----------------------------------------------
                    TableFact.GetFactsForFactType(arFactType)

                    '---------------------------------------------
                    'ObjectifyingEntityType
                    '---------------------------------------------
                    If Viev.NullVal(lREcordset("ObjectifyingEntityTypeId").Value, "") = "" Then
                        arFactType.ObjectifyingEntityType = Nothing
                    Else
                        Dim lsEntityTypeId = lREcordset("ObjectifyingEntityTypeId").Value
                        arFactType.ObjectifyingEntityType = arFactType.Model.EntityType.Find(Function(x) x.Id = lsEntityTypeId)
                        If arFactType.ObjectifyingEntityType IsNot Nothing Then
                            '---------------------------------------------
                            'Okay, have found the ObjectifyingEntityType
                            '---------------------------------------------
                            arFactType.ObjectifyingEntityType.IsObjectifyingEntityType = True
                            arFactType.ObjectifyingEntityType.ObjectifiedFactType = arFactType
                        ElseIf arFactType.ObjectifiedFactType Is Nothing And abDynamicallyLoadRelatedModelElements Then
                            arFactType.ObjectifyingEntityType = arFactType.Model.LoadModelElementById(pcenumConceptType.EntityType, lsEntityTypeId)
                            arFactType.ObjectifyingEntityType.IsObjectifyingEntityType = True
                            arFactType.ObjectifyingEntityType.ObjectifiedFactType = arFactType
                        Else
                            lsMessage = "No EntityType found in the Model for Objectifying Entity Type of the FactType"
                            lsMessage &= vbCrLf & vbCrLf & "Creating one. Save the Model after loading."

                            lsMessage &= vbCrLf & vbCrLf & "ModelId: " & arFactType.Model.ModelId
                            lsMessage &= vbCrLf & "FactTypeId: " & arFactType.Id
                            lsMessage &= vbCrLf & "Looking for EntityTypeId: " & lsEntityTypeId

                            If arFactType.IsObjectified Then lsEntityTypeId = arFactType.Id

                            Dim lrEntityType = arFactType.Model.CreateEntityType(lsEntityTypeId, True, False, False, True)
                            lrEntityType.IsObjectifyingEntityType = True
                            lrEntityType.ObjectifiedFactType = arFactType
                            arFactType.Model.IsDirty = True
                            arFactType.ObjectifyingEntityType = lrEntityType
                            arFactType.isDirty = True
                            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, Nothing, False, False, True,, True)
                        End If
                    End If

                Else
                    lsMessage = "Error: No FactType exists in the database for FactType.Id: " & arFactType.Id
                    Throw New System.Exception(lsMessage)
                End If

                lREcordset.Close()

                If abAddFactTypeToModel Then
                    Dim lrDictionaryEntry As FBM.DictionaryEntry = arFactType.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(arFactType.Model, arFactType.Id, pcenumConceptType.FactType))
                    arFactType.DBName = lrDictionaryEntry.DBName
                    arFactType.Model.AddFactType(arFactType, False, False)
                End If

                Return arFactType

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage.AppendDoubleLineBreak("Click [Yes] if you would like Boston to try and remove this Fact Type from the Model.")
                Dim liMessageResponse As MsgBoxResult
                liMessageResponse = prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, False, False, True, MessageBoxButtons.YesNo)

                If liMessageResponse = MsgBoxResult.Yes Then
                    If arFactType.CanSafelyRemoveFromModel() Then
                        Call TableFactType.DeleteFactType(arFactType)
                        abAborted = True
                        Return Nothing
                    End If
                End If
                If abAddFactTypeToModel Then
                    arFactType.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(arFactType.Model, arFactType.Id, pcenumConceptType.FactType))
                    arFactType.Model.AddFactType(arFactType, False, False)
                End If
FinishAnyway:
                Return arFactType

            End Try

        End Function

        Public Function getFactTypeIdByModelRoleId(ByRef arModel As FBM.Model, ByVal asRoleId As String) As String

            Try
                Dim lsFactTypeId As String = ""

                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT FT.*"
                lsSQLQuery &= "  FROM MetaModelFactType FT,"
                lsSQLQuery &= "       MetaModelRole R"
                lsSQLQuery &= " WHERE FT.ModelId = '" & Trim(arModel.ModelId) & "'"
                lsSQLQuery &= "   AND FT.FactTypeId = R.FactTypeId"
                lsSQLQuery &= "   AND FT.ModelId = R.ModelId"
                lsSQLQuery &= "   AND R.RoleId = '" & asRoleId & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    lsFactTypeId = lREcordset("FactTypeId").Value
                End If

                lREcordset.Close()

                Return lsFactTypeId

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return ""
            End Try

        End Function

        Public Function GetFactTypesByModel(ByRef arModel As FBM.Model, Optional ByVal abAddToModel As Boolean = False) As List(Of FBM.FactType)

            Dim lsMessage As String
            Dim lrFactType As FBM.FactType
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT FT.*, MD.*"
            lsSQLQuery &= "  FROM MetaModelFactType FT,"
            lsSQLQuery &= "       MetaModelModelDictionary MD"
            lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arModel.ModelId) & "'"
            lsSQLQuery &= "   AND MD.Symbol = FT.FactTypeId"
            lsSQLQuery &= "   AND MD.ModelId = FT.ModelId"
            lsSQLQuery &= "   AND MD.IsFactType = " & True
            lsSQLQuery &= " ORDER BY FT.IsObjectified ASC, FT.IsLinkFactType ASC"


            lREcordset.Open(lsSQLQuery)

            GetFactTypesByModel = New List(Of FBM.FactType)

            Try

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrFactType = New FBM.FactType
                        lrFactType.isDirty = False
                        lrFactType.Model = arModel
                        lrFactType.Id = lREcordset("FactTypeId").Value

                        Dim abAborted = False
                        TableFactType.GetFactTypeDetailsByModel(lrFactType, abAddToModel,, abAborted)
                        If abAborted Then GoTo MoveNext

                        GetFactTypesByModel.Add(lrFactType)

                        If abAddToModel Then
                            arModel.AddFactType(lrFactType, False, False, Nothing)
                        End If
MoveNext:
                        lREcordset.MoveNext()
                    End While
                Else
                    arModel.FactType = New List(Of FBM.FactType)
                End If

                '--------------------------------------------------------------
                'Go through each of the Roles in each of the FactTypes,
                '  and for those Roles that join a FactType, make
                '  sure that the JoinedORMObject is populated for the Role.
                '
                'The reason that this is required is because the 'Joined' FactType
                '  for a Role may not have been loaded at the time the Role
                '  was loaded, so the 'find' function will have returned 'Nothing'
                '--------------------------------------------------------------
                Dim lrRole As FBM.Role
                Dim larRole = From FactType In arModel.FactType
                              From Role In FactType.RoleGroup
                              Where Role.JoinedORMObject Is Nothing
                              Select Role

                For Each lrRole In larRole
                    Try
                        lrRole.JoinedORMObject = arModel.FactType.Find(Function(x) x.Id = lrRole.JoinsFactType.Id)
                    Catch ex As Exception
                        'Throw warning
                    End Try
                Next
                'For Each lrFactType In arModel.FactType
                '    For Each lrRole In lrFactType.RoleGroup.FindAll(Function(p) p.TypeOfJoin = pcenumRoleJoinType.FactType)
                '        lrRole.JoinedORMObject = arModel.FactType.Find(Function(x) x.Id = lrRole.JoinsFactType.Id)
                '    Next
                'Next

                lREcordset.Close()

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "Loading FactTypes for Model: '" & arModel.ModelId & "'"
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return GetFactTypesByModel
            End Try


        End Function

        Public Function GetFactTypesByModelJoinedModelElement(ByRef arModel As FBM.Model,
                                                              ByRef arModelElement As FBM.ModelObject,
                                                              Optional ByVal abAddToModel As Boolean = False,
                                                              Optional ByVal abLoadAssociatedInternalUniquenessConstraints As Boolean = False) As List(Of FBM.FactType)

            Dim lsMessage As String
            Dim lrFactType As FBM.FactType
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Dim larFactType As New List(Of FBM.FactType)

            Try

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT FT.*"
                lsSQLQuery &= "  FROM MetaModelFactType FT,"
                lsSQLQuery &= "       MetaModelRole MMR"
                lsSQLQuery &= " WHERE FT.ModelId = '" & Trim(arModel.ModelId) & "'"
                lsSQLQuery &= "   AND FT.FactTypeId = MMR.FactTypeId"
                lsSQLQuery &= "   AND FT.ModelId = MMR.ModelId"
                lsSQLQuery &= "   AND MMR.JoinsModelElementId = '" & arModelElement.Id & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then

                    Boston.WriteToStatusBar("Found " & lREcordset.RecordCount.ToString & " related Fact Types.", True)
                    While Not lREcordset.EOF
                        lrFactType = New FBM.FactType
                        lrFactType.isDirty = False
                        lrFactType.Model = arModel
                        lrFactType.Id = lREcordset("FactTypeId").Value

                        If arModel.FactType.Find(Function(x) x.Id = lrFactType.Id) IsNot Nothing Then GoTo SkipFactType

                        Boston.WriteToStatusBar("Getting Fact Type details for Fact Type: " & lrFactType.Id, True)
                        Call TableFactType.GetFactTypeDetailsByModel(lrFactType, abAddToModel, True)

                        larFactType.Add(lrFactType)

                        If abAddToModel Then
                            arModel.AddFactType(lrFactType, False, False, Nothing)
                        End If

SkipFactType:
                        If abLoadAssociatedInternalUniquenessConstraints Then
                            Call TableFactType.getUniquenessConstraintsForFactTypeByModel(lrFactType)
                        End If

                        lREcordset.MoveNext()
                    End While
                End If

                '--------------------------------------------------------------
                'Go through each of the Roles in each of the FactTypes,
                '  and for those Roles that join a FactType, make
                '  sure that the JoinedORMObject is populated for the Role.
                '
                'The reason that this is required is because the 'Joined' FactType
                '  for a Role may not have been loaded at the time the Role
                '  was loaded, so the 'find' function will have returned 'Nothing'
                '--------------------------------------------------------------
                Dim lrRole As FBM.Role
                Dim larRole = From FactType In arModel.FactType
                              From Role In FactType.RoleGroup
                              Where Role.JoinedORMObject Is Nothing
                              Select Role

                For Each lrRole In larRole
                    Try
                        lrRole.JoinedORMObject = arModel.FactType.Find(Function(x) x.Id = lrRole.JoinsFactType.Id)
                    Catch ex As Exception
                        'Throw warning
                    End Try
                Next

                lREcordset.Close()

                Return larFactType

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "Loading FactTypes for Model: '" & arModel.ModelId & "'"
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return larFactType
            End Try

        End Function

        Public Function getUniquenessConstraintsForFactTypeByModel(ByRef arFactType As FBM.FactType) As List(Of FBM.RoleConstraint)

            Dim larRoleConstraint As New List(Of FBM.RoleConstraint)

            Dim lsMessage As String
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Try
                Dim lsFactTypeId As String = arFactType.Id

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT RC.*"
                lsSQLQuery &= "  FROM MetaModelFactType FT,"
                lsSQLQuery &= "       MetaModelRole R,"
                lsSQLQuery &= "       MetaModelRoleConstraintRole RCR,"
                lsSQLQuery &= "       MetaModelRoleConstraint RC"
                lsSQLQuery &= " WHERE FT.ModelId = '" & Trim(arFactType.Model.ModelId) & "'"
                lsSQLQuery &= "   AND FT.FactTypeId = '" & Trim(arFactType.Id) & "'"
                lsSQLQuery &= "   AND R.ModelId = FT.ModelId"
                lsSQLQuery &= "   AND R.FactTypeId = FT.FactTypeId"
                lsSQLQuery &= "   AND RCR.RoleId = R.RoleId"
                lsSQLQuery &= "   AND RCR.ModelId = R.ModelId"
                lsSQLQuery &= "   AND RC.RoleConstraintId = RCR.RoleConstraintId"
                lsSQLQuery &= "   AND RC.RoleConstraintType = 'InternalUniquenessConstraint'"
                lsSQLQuery &= "   AND RC.ModelId = R.ModelId"

                lREcordset.Open(lsSQLQuery)

                Dim lrRoleConstraint As FBM.RoleConstraint

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF

                        If arFactType.Model.RoleConstraint.Find(Function(x) x.Id = lsFactTypeId) IsNot Nothing Then GoTo SkipRoleConstraint

                        lrRoleConstraint = arFactType.Model.LoadModelElementById(pcenumConceptType.RoleConstraint, lREcordset("RoleConstraintId").Value)

                        larRoleConstraint.Add(lrRoleConstraint)
SkipRoleConstraint:
                        lREcordset.MoveNext()
                    End While
                End If

                Return larRoleConstraint

                lREcordset.Close()
            Catch ex As Exception


                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return larRoleConstraint
            End Try

        End Function

        Function getFactTypeCountByModel(ByVal as_ModelId As String) As Integer

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelFactType FT,"
            lsSQLQuery &= "       MetaModelModelDictionary MD"
            lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(as_ModelId) & "'"
            lsSQLQuery &= "   AND MD.Symbol = FT.FactTypeId"
            lsSQLQuery &= "   AND MD.ModelId = FT.ModelId"


            lREcordset.Open(lsSQLQuery)

            getFactTypeCountByModel = lREcordset(0).Value

            lREcordset.Close()

        End Function

        Public Sub ModifyKey(ByVal arFactType As FBM.FactType, ByVal asNewKey As String)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelFactType"
                lsSQLQuery &= "   SET FactTypeId = '" & Trim(Replace(asNewKey, "'", "`")) & "'"
                lsSQLQuery &= " WHERE FactTypeId = '" & arFactType.Id & "'"
                lsSQLQuery &= "   AND ModelId = '" & arFactType.Model.ModelId & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

                lsSQLQuery = " UPDATE MetaModelSubtypeRelationship"
                lsSQLQuery &= "   SET SubtypingFactTypeId = '" & Trim(Replace(asNewKey, "'", "`")) & "'"
                lsSQLQuery &= " WHERE SubtypingFactTypeId = '" & arFactType.Id & "'"
                lsSQLQuery &= "   AND ModelId = '" & arFactType.Model.ModelId & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            End Try

        End Sub


        Public Sub UpdateFactType(ByVal arFactType As FBM.FactType)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = " UPDATE MetaModelFactType"
                lsSQLQuery &= "   SET FactTypeName = '" & Trim(Replace(arFactType.Name, "'", "`")) & "'"
                lsSQLQuery &= "       , IsObjectified = " & arFactType.IsObjectified
                lsSQLQuery &= "       , IsCoreFactType = " & arFactType.IsCoreFactType
                lsSQLQuery &= "       , IsPreferredReferenceMode = " & arFactType.IsPreferredReferenceMode
                If IsSomething(arFactType.ObjectifyingEntityType) Then
                    lsSQLQuery &= "       , ObjectifyingEntityTypeId = '" & Trim(arFactType.ObjectifyingEntityType.Id) & "'"
                Else
                    lsSQLQuery &= "       , ObjectifyingEntityTypeId = ''"
                End If
                lsSQLQuery &= "       , IsSubtypeFactType = " & arFactType.IsSubtypeRelationshipFactType
                lsSQLQuery &= "       , IsMDAModelElement = " & arFactType.IsMDAModelElement
                lsSQLQuery &= "       , IsDerived = " & arFactType.IsDerived
                lsSQLQuery &= "       , IsStored = " & arFactType.IsStored
                lsSQLQuery &= "       , DerivationText = '" & Trim(Database.MakeStringSafe(arFactType.DerivationText)) & "'"
                lsSQLQuery &= "       , IsLinkFactType = " & arFactType.IsLinkFactType
                lsSQLQuery &= "       , [GUID] = '" & arFactType.GUID & "'"
                If arFactType.LinkFactTypeRole Is Nothing Then
                    lsSQLQuery &= "       , LinkFactTypeRoleId = ''"
                Else
                    lsSQLQuery &= "       , LinkFactTypeRoleId = '" & arFactType.LinkFactTypeRole.Id & "'"
                End If
                lsSQLQuery &= "       , IsIndependent = " & arFactType.IsIndependent
                lsSQLQuery &= "       , IsSubtypeStateControlling = " & arFactType.IsSubtypeStateControlling
                lsSQLQuery &= "       , StoreFactCoordinates = " & arFactType.StoreFactCoordinates
                lsSQLQuery &= " WHERE FactTypeId = '" & Replace(arFactType.Id, "'", "`") & "'"
                lsSQLQuery &= "   AND ModelId = '" & Replace(arFactType.Model.ModelId, "'", "`") & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactType.UpdateFactType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "SQL: " & lsSQLQuery
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Module

End Namespace