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
                lsSQLQuery &= " ,'" & Trim(arRoleConstraint.MinimumValue) & "'"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraint.MaximumValue) & "'"
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

        Public Function getAndLoadJoinedFactTypesByRoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint) As List(Of FBM.FactType)

            Dim larFactType As New List(Of FBM.FactType)
            Dim lsMessage As String
            Try

                Dim lrFactType As FBM.FactType
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                Dim lsModelId As String = arRoleConstraint.Model.ModelId

                lsSQLQuery = " SELECT FT.*,MD.*"
                lsSQLQuery &= "  FROM MetaModelModelDictionary MD,"
                lsSQLQuery &= "       MetaModelFactType FT,"
                lsSQLQuery &= "       MetaModelRoleConstraint RC,"
                lsSQLQuery &= "       MetaModelRoleConstraintRole RCR,"
                lsSQLQuery &= "       MetaModelRole R"
                lsSQLQuery &= " WHERE MD.ModelId = '" & lsModelId & "'"
                lsSQLQuery &= "   AND MD.Symbol = RC.RoleConstraintId"
                lsSQLQuery &= "   AND FT.ModelId = '" & lsModelId & "'"
                lsSQLQuery &= "   AND FT.FactTypeId = R.FactTypeId"
                lsSQLQuery &= "   AND R.ModelId = '" & lsModelId & "'"
                lsSQLQuery &= "   AND R.RoleId = RCR.RoleId"
                lsSQLQuery &= "   AND RCR.ModelId = '" & lsModelId & "'"
                lsSQLQuery &= "   AND RCR.RoleConstraintId = RC.RoleConstraintId"
                lsSQLQuery &= "   AND RC.ModelId = '" & lsModelId & "'"
                lsSQLQuery &= "   AND RC.RoleConstraintId = '" & arRoleConstraint.Id & "'"
                lsSQLQuery &= " ORDER BY FT.IsObjectified ASC, FT.IsLinkFactType ASC"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrFactType = New FBM.FactType
                        lrFactType.Model = arRoleConstraint.Model
                        lrFactType._Symbol = lREcordset("FactTypeId").Value
                        lrFactType.Id = lREcordset("FactTypeId").Value
                        lrFactType.Name = lREcordset("FactTypeName").Value
                        lrFactType.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                        lrFactType.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
                        lrFactType.IsObjectified = CBool(lREcordset("IsObjectified").Value)
                        lrFactType.IsCoreFactType = CBool(lREcordset("IsCoreFactType").Value)
                        lrFactType.IsPreferredReferenceMode = CBool(lREcordset("IsPreferredReferenceMode").Value)
                        lrFactType.IsSubtypeRelationshipFactType = CBool(lREcordset("IsSubtypeFactType").Value)
                        lrFactType.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                        lrFactType.IsDerived = CBool(lREcordset("IsDerived").Value)
                        lrFactType.IsStored = CBool(lREcordset("IsStored").Value)
                        lrFactType.DerivationText = NullVal(lREcordset("DerivationText").Value, "")
                        lrFactType.IsLinkFactType = CBool(lREcordset("IsLinkFactType").Value)
                        If lrFactType.IsLinkFactType Then
                            lrFactType.LinkFactTypeRole = lrFactType.Model.Role.Find(Function(x) x.Id = NullVal(lREcordset("LinkFactTypeRoleId").Value, ""))
                        End If
                        lrFactType.GUID = lREcordset("GUID").Value
                        lrFactType.IsIndependent = CBool(lREcordset("IsIndependent").Value)
                        lrFactType.IsSubtypeStateControlling = CBool(lREcordset("IsSubtypeStateControlling").Value)
                        lrFactType.StoreFactCoordinates = CBool(lREcordset("StoreFactCoordinates").Value)
                        lrFactType.isDirty = False

                        '---------------------------------------------------------------------------------
                        'Dynamically load related ModelElements (ValueTypes,EntityTypes) if required.
                        '-----------------------------------------------------------------------------
                        Call lrFactType.LoadRelatedModelElementsToModel()

                        '------------------------------------------------------------
                        'Get the Roles within the RoleGroup for the FactType as well
                        '------------------------------------------------------------
                        lrFactType.RoleGroup = TableRole.GetRolesForModelFactType(lrFactType, True)

                        '------------------------------------------------------------
                        'Get the FactTypeReadings for the FactType
                        '------------------------------------------------------------
                        lrFactType.FactTypeReading = TableFactTypeReading.GetFactTypeReadingsForFactType(lrFactType)

                        '----------------------------------------------
                        'Get the Facts (FactTypeData) for the FactType
                        '----------------------------------------------
                        TableFact.GetFactsForFactType(lrFactType)

                        '---------------------------------------------
                        'ObjectifyingEntityType
                        '---------------------------------------------
                        If Viev.NullVal(lREcordset("ObjectifyingEntityTypeId").Value, "") = "" Then
                            lrFactType.ObjectifyingEntityType = Nothing
                        Else
                            Dim lsEntityTypeId = lREcordset("ObjectifyingEntityTypeId").Value
                            lrFactType.ObjectifyingEntityType = lrFactType.Model.EntityType.Find(Function(x) x.Id = lsEntityTypeId)
                            If lrFactType.ObjectifyingEntityType IsNot Nothing Then
                                '---------------------------------------------
                                'Okay, have found the ObjectifyingEntityType
                                '---------------------------------------------
                                lrFactType.ObjectifyingEntityType.IsObjectifyingEntityType = True
                                lrFactType.ObjectifyingEntityType.ObjectifiedFactType = New FBM.FactType
                                lrFactType.ObjectifyingEntityType.ObjectifiedFactType = lrFactType
                            Else
                                lsMessage = "No EntityType found in the Model for Objectifying Entity Type of the FactType"
                                lsMessage &= vbCrLf & vbCrLf & "Creating one. Save the Model after loading."

                                lsMessage &= vbCrLf & vbCrLf & "ModelId: " & lrFactType.Model.ModelId
                                lsMessage &= vbCrLf & "FactTypeId: " & lrFactType.Id
                                lsMessage &= vbCrLf & "Looking for EntityTypeId: " & lsEntityTypeId

                                Dim lrEntityType = lrFactType.Model.CreateEntityType(lsEntityTypeId, True)
                                lrEntityType.IsObjectifyingEntityType = True
                                lrEntityType.ObjectifiedFactType = lrFactType
                                lrFactType.Model.IsDirty = True

                                lREcordset.Close()
                                Throw New Exception(lsMessage)
                            End If

                        End If

                        arRoleConstraint.Model.FactType.AddUnique(lrFactType)
                        lREcordset.MoveNext()
                    End While
                End If

                Return larFactType

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return larFactType
            End Try

        End Function

        Public Function getRoleConstraintDetailsByModel(ByRef arRoleConstraint As FBM.RoleConstraint) As FBM.RoleConstraint

            Try
                Dim lsMessage As String
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                Dim lrFactType As FBM.FactType

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT RC.*, MD.*"
                lsSQLQuery &= "  FROM MetaModelRoleConstraint RC,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arRoleConstraint.Model.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = RC.RoleConstraintId"
                lsSQLQuery &= "   AND MD.ModelId = RC.ModelId"
                lsSQLQuery &= "   AND RC.RoleConstraintId = '" & arRoleConstraint.Id & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        arRoleConstraint.isDirty = False
                        arRoleConstraint.Id = Trim(lREcordset("RoleConstraintId").Value)
                        arRoleConstraint.Name = Trim(Viev.NullVal(lREcordset("RoleConstraintName").Value, ""))
                        arRoleConstraint.ConceptType = pcenumConceptType.RoleConstraint
                        arRoleConstraint.RoleConstraintType = CType([Enum].Parse(GetType(pcenumRoleConstraintType), lREcordset("RoleConstraintType").Value), pcenumRoleConstraintType)
                        arRoleConstraint.RingConstraintType = CType([Enum].Parse(GetType(pcenumRingConstraintType), lREcordset("RingConstraintType").Value), pcenumRingConstraintType)
                        arRoleConstraint.LevelNr = lREcordset("LevelNr").Value
                        arRoleConstraint.IsPreferredIdentifier = lREcordset("IsPreferredUniqueness").Value
                        arRoleConstraint.IsDeontic = lREcordset("IsDeontic").Value
                        arRoleConstraint.Cardinality = lREcordset("Cardinality").Value
                        arRoleConstraint.MaximumFrequencyCount = lREcordset("MaximumFrequencyCount").Value
                        arRoleConstraint.MinimumFrequencyCount = lREcordset("MinimumFrequencyCount").Value
                        Select Case lREcordset("CardinalityRangeType").Value
                            Case Is = pcenumCardinalityRangeType.LessThanOrEqual.ToString
                                arRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOrEqual
                            Case Is = pcenumCardinalityRangeType.Equal.ToString
                                arRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Equal
                            Case Is = pcenumCardinalityRangeType.GreaterThanOrEqual.ToString
                                arRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.GreaterThanOrEqual
                            Case Is = pcenumCardinalityRangeType.Between.ToString
                                arRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Between
                        End Select
                        arRoleConstraint.ValueRangeType = CType([Enum].Parse(GetType(pcenumValueRangeType), lREcordset("ValueRangeType").Value), pcenumValueRangeType)
                        arRoleConstraint.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                        arRoleConstraint.LongDescription = lREcordset("LongDescription").Value
                        arRoleConstraint.ShortDescription = lREcordset("ShortDescription").Value
                        arRoleConstraint.GUID = lREcordset("GUID").Value
                        arRoleConstraint.MinimumValue = Trim(Viev.NullVal(lREcordset("MinimumValue").Value, ""))
                        arRoleConstraint.MaximumValue = Trim(Viev.NullVal(lREcordset("MaximumValue").Value, ""))
                        arRoleConstraint.isDirty = False

                        '------------------------------------------------
                        'Link to the Concept within the ModelDictionary
                        '------------------------------------------------
                        Dim lrDictionaryEntry As New FBM.DictionaryEntry(arRoleConstraint.Model, arRoleConstraint.Id, pcenumConceptType.RoleConstraint, arRoleConstraint.ShortDescription, arRoleConstraint.LongDescription)
                        lrDictionaryEntry = arRoleConstraint.Model.AddModelDictionaryEntry(lrDictionaryEntry, True, False, False, False, True, True)

                        If lrDictionaryEntry Is Nothing Then
                            lsMessage = "Cannot find DictionaryEntry in the ModelDictionary for RoleConstraint:"
                            lsMessage &= vbCrLf & "Model.Id: " & arRoleConstraint.Model.ModelId
                            lsMessage &= vbCrLf & "RoleConstraint.Id: " & arRoleConstraint.Id
                            Throw New Exception(lsMessage)
                        End If

                        arRoleConstraint.Concept = lrDictionaryEntry.Concept

                        arRoleConstraint.Argument = TableRoleConstraintArgument.GetArgumentsForRoleConstraint(arRoleConstraint)
                        arRoleConstraint.Role = TableRoleConstraint.getRoles_for_RoleConstraint(arRoleConstraint)

                        '----------------------------------------------------------
                        'Get the RoleConstraintRole list for the RoleConstraint.
                        '  NB the SequenceNr of a RoleConstraintRole is 'not' the 
                        '  same as a SequenceNr on a Role, and Richmond may very
                        '  well make SequenceNr on Role redundant.
                        '  SequenceNr on a RoleConstraintRole is many things, but
                        '  relates particularly to DataIn, DataOut integrity matching.
                        '----------------------------------------------------------
                        arRoleConstraint.RoleConstraintRole = TableRoleConstraintRole.getRoleConstraintRoles_by_RoleConstraint(arRoleConstraint)

                        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
                            prApplication.ThrowErrorMessage("No RoleConstraintRoles found for RoleConstraint.Id: " & arRoleConstraint.Id, pcenumErrorType.Information)
                        Else
                            lrFactType = arRoleConstraint.Role(0).FactType
                            lrFactType = arRoleConstraint.Model.FactType.Find(AddressOf lrFactType.Equals)

                            Select Case arRoleConstraint.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                    Call lrFactType.AddInternalUniquenessConstraint(arRoleConstraint)
                            End Select
                        End If

                        '----------------------------------------
                        'ValueConstraints
                        If arRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.RoleValueConstraint Then

                            Call TableRoleValueConstraint.GetValueConstraintsByRoleConstraint(arRoleConstraint)

                            Try
                                'Allocate the RoleConstraint to the Role it belongs to.
                                arRoleConstraint.RoleConstraintRole(0).Role.RoleConstraintRoleValueConstraint = arRoleConstraint

                                For Each lsConstraintValue In arRoleConstraint.ValueConstraint
                                    arRoleConstraint.RoleConstraintRole(0).Role.ValueConstraint.Add(lsConstraintValue)
                                Next
                            Catch ex As Exception
                                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                                lsMessage &= vbCrLf & vbCrLf & ex.Message
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                            End Try

                        End If

                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()

                Return arRoleConstraint
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return arRoleConstraint
            End Try
        End Function

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
                        lrRoleConstraint.MinimumValue = Trim(Viev.NullVal(lREcordset("MinimumValue").Value, ""))
                        lrRoleConstraint.MaximumValue = Trim(Viev.NullVal(lREcordset("MaximumValue").Value, ""))
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

                        '----------------------------------------
                        'ValueConstraints
                        If lrRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.RoleValueConstraint Then

                            Call TableRoleValueConstraint.GetValueConstraintsByRoleConstraint(lrRoleConstraint)

                            Try
                                'Allocate the RoleConstraint to the Role it belongs to.
                                lrRoleConstraint.RoleConstraintRole(0).Role.RoleConstraintRoleValueConstraint = lrRoleConstraint

                                For Each lsConstraintValue In lrRoleConstraint.ValueConstraint
                                    lrRoleConstraint.RoleConstraintRole(0).Role.ValueConstraint.Add(lsConstraintValue)
                                Next
                            Catch ex As Exception
                                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                                lsMessage &= vbCrLf & vbCrLf & ex.Message
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                            End Try

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
                lsSQLQuery &= "       ,MinimumValue = '" & Trim(arRoleConstraint.MinimumValue) & "'"
                lsSQLQuery &= "       ,MaximumValue = '" & Trim(arRoleConstraint.MaximumValue) & "'"
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