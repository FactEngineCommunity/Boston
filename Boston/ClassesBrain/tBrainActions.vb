Imports System.Reflection

Partial Public Class tBrain

    Private Function ProcessENTITYTYPEISIDENTIFIEDBYITSStatement(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                                 Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String
        Dim lsReferenceMode As String
        Dim lsUnabatedReferenceMode As String
        Dim lrPGSNodeType As PGS.Node = Nothing

        Try
            Me.Model = prApplication.WorkingModel

            Dim lrEntityType As FBM.EntityType
            Dim lrEntityTypeInstance As FBM.EntityTypeInstance = Nothing

            Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement.MODELELEMENTNAME = ""
            Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement.REFERENCEMODE = ""
            Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement.KEYWDWRITTENAS = Nothing
            Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement.VALUETYPEWRITTENASCLAUSE = Nothing

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement, Me.VAQLParsetree.Nodes(0))

            Dim lsEntityTypeName As String = Trim(Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement.MODELELEMENTNAME)
            Dim lsActualModelElementName As String = ""

            If Me.Model.GetConceptTypeByNameFuzzy(lsEntityTypeName, lsActualModelElementName) = pcenumConceptType.EntityType Then
                '---------------------------------------------------------
                'Great! The name identified by the user is an EntityType
                '---------------------------------------------------------

                lsEntityTypeName = lsActualModelElementName

                Dim lrModelObject = Me.Model.GetModelObjectByName(lsEntityTypeName)
                lrEntityType = lrModelObject
            Else
                If Me.Model.ExistsModelElement(lsEntityTypeName) Then
                    Dim lrModelObject = Me.Model.GetModelObjectByName(lsEntityTypeName)
                    If lrModelObject.GetType IsNot GetType(FBM.EntityType) Then
                        lsMessage = lsEntityTypeName & " is not an Entity Type"
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)
                        Return False
                    End If
                End If
                lrEntityType = Me.Model.CreateEntityType(lsEntityTypeName, True, abBroadcastInterfaceEvent)

                If Me.Page IsNot Nothing Then
                    Select Case Me.Page.Language
                        Case Is = pcenumLanguage.ORMModel
                            lrEntityTypeInstance = Me.Page.DropEntityTypeAtPoint(lrEntityType, New PointF(100, 100))
                        Case Is = pcenumLanguage.PropertyGraphSchema
                            lrPGSNodeType = Me.Page.LoadPGSNodeTypeFromRDSTable(lrEntityType.getCorrespondingRDSTable, New PointF(50, 50), True)
                    End Select

                End If

                'Call lrEntityTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                'Call lrEntityTypeInstance.Move(lrEntityTypeInstance.X, lrEntityTypeInstance.Y, True)
            End If

            lsReferenceMode = Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement.REFERENCEMODE
            lsUnabatedReferenceMode = lsReferenceMode

            Dim items As Array
            items = System.Enum.GetValues(GetType(pcenumReferenceModeEndings))
            Dim item As pcenumReferenceModeEndings
            For Each item In items
                If lsReferenceMode.EndsWith(GetEnumDescription(item)) Then
                    lsReferenceMode = "." & lsReferenceMode
                    Exit For
                ElseIf lsReferenceMode.EndsWith(GetEnumDescription(item).Trim({"."c})) Then 'See https://msdn.microsoft.com/en-us/library/kxbw3kwc(v=vs.110).aspx
                    lsReferenceMode = "." & lsReferenceMode
                    Exit For
                End If
            Next

            Call lrEntityType.SetReferenceMode(lsReferenceMode, False, Nothing, abBroadcastInterfaceEvent)

            'Check if the DataType of the ReferenceModeValueType has been specified
            Dim lsDataTypeName As String = ""
            Dim liDataTypeLength As Integer = 0
            Dim liDataTypePrecision As Integer = 0
            Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet

            If lrEntityType.ReferenceModeValueType IsNot Nothing And lrEntityType.ReferenceModeFactType IsNot Nothing Then

                If Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement.KEYWDWRITTENAS IsNot Nothing Then

                    Me.VAQLProcessor.VALUETYPEWRITTENASClause = Me.VAQLProcessor.ENTITYTYPEISIDENTIFIEDBYITSStatement.VALUETYPEWRITTENASCLAUSE

                    If Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE IsNot Nothing Then
                        lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE.Nodes(0).Token.Text
                    ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH IsNot Nothing Then
                        lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH.Nodes(0).Token.Text
                        liDataTypeLength = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                    ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION IsNot Nothing Then
                        lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION.Nodes(0).Token.Text
                        liDataTypePrecision = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                    End If

                    lsDataTypeName = DataTypeAttribute.Get(GetType(pcenumORMDataType), lsDataTypeName)
                    If lsDataTypeName Is Nothing Then
                        lsMessage = "That's not a valid Data Type."
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)
                        Return False
                    End If

                    Try
                        liDataType = DirectCast([Enum].Parse(GetType(pcenumORMDataType), lsDataTypeName), pcenumORMDataType)
                    Catch ex As Exception
                        lsMessage = "That's not a valid Data Type."
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)
                        Return False
                    End Try

                    Call lrEntityType.ReferenceModeValueType.SetDataType(liDataType, liDataTypeLength, liDataTypePrecision, abBroadcastInterfaceEvent)
                Else
                    lsMessage = "Remember that you can set the Data Type for the Value Type of the Reference Scheme by saying something like:"
                    lsMessage.AppendLine(lrEntityType.Id & " IS IDENTIFIED BY ITS " & lsUnabatedReferenceMode & " WRITTEN AS AutoCounter")
                    lsMessage.AppendLine("I.e. Add a 'WRITTEN AS' clause to your 'IS IDENTIFIED BY ITS' statement.")
                    Me.send_data(lsMessage)
                End If

                If Not Me.Model.StoreAsXML Then
                    Call lrEntityType.ReferenceModeValueType.Save()
                    Call lrEntityType.ReferenceModeFactType.Save()

                    For Each lrInternalUniquenessConstraint In lrEntityType.ReferenceModeFactType.InternalUniquenessConstraint
                        Call lrInternalUniquenessConstraint.Save()
                    Next
                End If
            End If

            If Not Me.Model.StoreAsXML Then
                Call lrEntityType.Save()
            End If

            If Me.Page IsNot Nothing Then
                If lrEntityTypeInstance IsNot Nothing Then
                    Select Case Me.Page.Language
                        Case Is = pcenumLanguage.ORMModel
                            Call lrEntityTypeInstance.SetAppropriateColour()
                    End Select
                ElseIf lrPGSNodeType IsNot Nothing Then
                    Select Case Me.Page.Language
                        Case Is = pcenumLanguage.PropertyGraphSchema
                            Call lrPGSNodeType.SetAppropriateColour()
                    End Select
                End If
            End If

            Me.send_data("Ok")

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False

        End Try

    End Function

    Private Function ProcessOBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                                          Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String
        Dim lrPGSNodeType As PGS.Node = Nothing

        Try
            Me.Model = prApplication.WorkingModel

            Dim lrFactType As FBM.FactType
            Dim lrFactTypeInstance As FBM.FactTypeInstance = Nothing

            Me.VAQLProcessor.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement.MODELELEMENT = New List(Of VAQL.ModelElementClause)

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement, Me.VAQLParsetree.Nodes(0))

            Dim lsFactTypeName As String = Trim(Me.VAQLProcessor.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement.MODELELEMENT(0).MODELELEMENTNAME)
            Dim lsActualModelElementName As String = ""

            Dim lrModelObject = Me.Model.GetModelObjectByName(lsFactTypeName)

            If Me.Model.GetConceptTypeByNameFuzzy(lsFactTypeName, lsActualModelElementName) = pcenumConceptType.FactType Then
                '---------------------------------------------------------
                'Great! The name identified by the user is an EntityType
                '---------------------------------------------------------

                lsFactTypeName = lsActualModelElementName
                lrModelObject = Me.Model.GetModelObjectByName(lsFactTypeName)

            ElseIf lrModelObject Is Nothing Then
                lsMessage = lsFactTypeName & " is not a Fact Type. You need to either create the Fact Type or make sure the name does not conflict with the name of another Object Type."
                If arDSCError IsNot Nothing Then
                    arDSCError.Success = False
                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                    arDSCError.ErrorString = lsMessage
                End If
                Me.send_data(lsMessage)
                Return False

            ElseIf lrModelObject.GetType IsNot GetType(FBM.FactType) Then
                lsMessage = lsFactTypeName & " is not a Fact Type. You need to either create the Fact Type or make sure the name does not conflict with the name of another Object Type."
                If arDSCError IsNot Nothing Then
                    arDSCError.Success = False
                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                    arDSCError.ErrorString = lsMessage
                End If
                Me.send_data(lsMessage)
                Return False

            End If

            lrFactType = lrModelObject

            '=========================================================================
            'Create the Internal Uniqueness Constraint

            Dim larRole As New List(Of FBM.Role)


            For liInd = 1 To Me.VAQLProcessor.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement.MODELELEMENT.Count - 1

                Dim lsModelElementName = Me.VAQLProcessor.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement.MODELELEMENT(liInd).MODELELEMENTNAME
                lrModelObject = Me.Model.GetModelObjectByName(lsModelElementName)

                If lrModelObject Is Nothing Then
                    lsMessage = lsModelElementName & " is not a Object Type in the Model."
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                ElseIf lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementName) Is Nothing Then
                    lsMessage = lsModelElementName & " is not a Object Type that plays any Role in the Fact Type, " & lrFactType.Id & "."
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                Else
                    larRole.Add(lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementName))
                End If
            Next

#Region "Checks before creating Internal Uniqueness Constraint"

            Dim lbRemoveExistingRoleConstraint As Boolean = False

            If larRole.Count < lrFactType.RoleGroup.Count - 1 Then

                lsMessage = "The number of Object Types played by Roles in the Internal Uniqueness Constraint need to be at least the number of Roles in the Fact Type minus one."
                If arDSCError IsNot Nothing Then
                    arDSCError.Success = False
                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                    arDSCError.ErrorString = lsMessage
                End If
                Me.send_data(lsMessage)
                Return False
            Else

                If lrFactType.InternalUniquenessConstraint.Count > 0 Then
                    '---------------------------------------------------
                    'The FactType already has an InternalRoleConstraint
                    '---------------------------------------------------
                    '------------------------------------------------------------------------
                    'If the SelectedRoles spans 'All' the Roles within the FactType, then
                    '  remove all existing InternalUniquenessConstraints from the FactType
                    '  before adding the TotalInternalUniquenessConstraint (because when a
                    '  TotalInternalUniquenessConstraint is on a FactType, it is the 'only'
                    '  InternalUniquenessConstraint allowed. 
                    '  NB Granted, a TotalInternalUniquenessConstraint may already exist on 
                    '  the FactType, but deleting it causes no harm if it is immediately replaced.
                    '------------------------------------------------------------------------                        
                    If larRole.Count = lrFactType.Arity Then
                        If lrFactType.InternalUniquenessConstraint(0).RoleConstraintRole.Count = lrFactType.Arity Then
                            lsMessage = "The Fact Type, '" & lrFactType.Id & "', already has a total internal uniqueness constraint."
                            If arDSCError IsNot Nothing Then
                                arDSCError.Success = False
                                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                                arDSCError.ErrorString = lsMessage
                            End If
                            Me.send_data(lsMessage)
                            Return False
                        Else
                            Call lrFactType.RemoveInternalUniquenessConstraints(True)  'Because we are about to replace the TotalInternalUniquenessConstraint with one that spans FT.Arity-1 Roles.
                        End If
                    ElseIf lrFactType.InternalUniquenessConstraint(0).RoleConstraintRole.Count = lrFactType.Arity Then
                        lbRemoveExistingRoleConstraint = True 'Because we are about to replace the TotalInternalUniquenessConstraint with one that spans FT.Arity-1 Roles.
                    End If

                End If

                '------------------------------------------------------------------------------
                'Create a dummy RoleConstraint for the InternalUniquenessConstraint
                '  so that we can check if it already exists for the FactType.
                '  NB This is only a dummy RoleConstraint and is not added to 
                '  the model (Parameter: abAddToModel is False).
                '  Once we confirm that the InternalUniquenssConstraint does not
                '  already exist, we create the InternalUniquenessConstraint/RoleConstraint
                '  via the FactTypeInstance (see futher below).
                '------------------------------------------------------------------------------

                Dim lrRoleConstraint As New FBM.RoleConstraint(lrFactType.Model, pcenumRoleConstraintType.InternalUniquenessConstraint, larRole)

                If lrFactType.exists_InternalUniquenessConstraint_by_role_span(lrRoleConstraint) Then
                    '----------------------------------------------------------------
                    'InternalUniquenessConstraint already exists for Roles selected
                    '----------------------------------------------------------------
                    lsMessage = "The Fact Type, " & lrFactType.Id & " already has an Internal Uniqueness Constraint plays by the Roles that link the nominated Object Types."
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                End If

                If lbRemoveExistingRoleConstraint Then
                    Call lrFactType.RemoveInternalUniquenessConstraints(True, True)
                End If
#End Region

                Call lrFactType.CreateInternalUniquenessConstraint(larRole, False, True, True, False, Nothing, abBroadcastInterfaceEvent, False)

            End If

            If Me.Page IsNot Nothing Then
                Select Case Me.Page.Language
                    Case Is = pcenumLanguage.ORMModel
                        If Me.Page.FactTypeInstance.Find(Function(x) x.Id = lrFactType.Id) Is Nothing Then
                            lrFactTypeInstance = Me.Page.DropFactTypeAtPoint(lrFactType, New PointF(100, 100), False,, abBroadcastInterfaceEvent,, True, False, True)
                        End If
                    Case Is = pcenumLanguage.PropertyGraphSchema
                        lrPGSNodeType = Me.Page.LoadPGSNodeTypeFromRDSTable(lrFactType.getCorrespondingRDSTable, New PointF(50, 50), True)
                End Select
            End If

            If Not Me.Model.StoreAsXML Then
                Call lrFactType.Save()
            End If

            If Me.Page IsNot Nothing Then
                If lrFactTypeInstance IsNot Nothing Then
                    Select Case Me.Page.Language
                        Case Is = pcenumLanguage.ORMModel
                            Call lrFactTypeInstance.SetAppropriateColour()
                    End Select
                ElseIf lrPGSNodeType IsNot Nothing Then
                    Select Case Me.Page.Language
                        Case Is = pcenumLanguage.PropertyGraphSchema
                            Call lrPGSNodeType.SetAppropriateColour()
                    End Select
                End If
            End If

            Me.send_data("Ok")

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False

        End Try

    End Function

    Public Sub executeMakeFactTypeManyToMany(ByRef arFactType As FBM.FactType)

        Try

            Call arFactType.MakeManyToManyRelationship()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Function ProcessFactStatement(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                                          Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String
        Dim lrPGSNodeType As PGS.Node = Nothing

        Try
            Me.Model = prApplication.WorkingModel

            Dim lrFact As FBM.Fact
            Dim lrFactType As FBM.FactType
            Dim lrFactTypeInstance As FBM.FactTypeInstance = Nothing

            Me.VAQLProcessor.FACTStatement = New VAQL.FactStatement

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.FACTStatement, Me.VAQLParsetree.Nodes(0))

#Region "Get Fact Type"

            Dim lsFactTypeReading As String = ""

            Dim liInd = 0
            For Each lrIdentifierModelElement In Me.VAQLProcessor.FACTStatement.IDENTIFIERMODELELEMENT

                lsFactTypeReading &= lrIdentifierModelElement.MODELELEMENT.PREBOUNDREADINGTEXT
                lsFactTypeReading &= lrIdentifierModelElement.MODELELEMENT.MODELELEMENTNAME
                lsFactTypeReading &= lrIdentifierModelElement.MODELELEMENT.POSTBOUNDREADINGTEXT
                lsFactTypeReading &= " "

                If liInd < Me.VAQLProcessor.FACTStatement.IDENTIFIERMODELELEMENT.Count - 1 Then
                    For Each lsPredicatePart In Me.VAQLProcessor.FACTStatement.PREDICATECLAUSE(liInd).PREDICATEPART
                        lsFactTypeReading &= Trim(lsPredicatePart) & " "
                    Next
                End If

                liInd += 1
            Next

            Dim lsReturnErrorMessage As String = ""

            lrFactType = Me.Model.getFactTypeByNaturalLanguageFactTypeReading(lsFactTypeReading, Nothing, True, lsReturnErrorMessage)

            If lrFactType Is Nothing Then
                lsMessage = lsReturnErrorMessage
                If arDSCError IsNot Nothing Then
                    arDSCError.Success = False
                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                    arDSCError.ErrorString = lsMessage
                End If
                Me.send_data(lsMessage)
                Return False
            End If

#Region "Create the Fact"

            Dim lrFactData As New FBM.FactData

            Try
                '--------------------------------
                'Add a new Fact to the FactType
                '--------------------------------
                Dim lrRole As New FBM.Role

                lrFact = Me.Model.CreateFact(lrFactType)
                Call lrFact.makeDirty() 'Because we want this fact saved to the database

                liInd = 0
                For Each lrFactData In lrFact.Data
                    lrFactData.setData(Me.VAQLProcessor.FACTStatement.IDENTIFIERMODELELEMENT(liInd).VALUE, pcenumConceptType.Value, True)
                    liInd += 1
                Next

                '---------------------------------
                'Ad the new Fact to the FactType
                '---------------------------------
                lrFactType.AddFact(lrFact, True)

                Call Me.Model.checkForErrors()

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message

                If arDSCError IsNot Nothing Then
                    arDSCError.Success = False
                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                    arDSCError.ErrorString = lsMessage
                End If
                Me.send_data(lsMessage)

                Return Nothing
            End Try

#End Region

            '========================================================


#End Region

            'Dim lsFactTypeName As String = Trim(Me.VAQLProcessor.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement.MODELELEMENT(0).MODELELEMENTNAME)
            'Dim lsActualModelElementName As String = ""

            'Dim lrModelObject = Me.Model.GetModelObjectByName(lsFactTypeName)

            'If Me.Model.GetConceptTypeByNameFuzzy(lsFactTypeName, lsActualModelElementName) = pcenumConceptType.FactType Then
            '    '---------------------------------------------------------
            '    'Great! The name identified by the user is an EntityType
            '    '---------------------------------------------------------

            '    lsFactTypeName = lsActualModelElementName
            '    lrModelObject = Me.Model.GetModelObjectByName(lsFactTypeName)

            'ElseIf lrModelObject Is Nothing Then
            '    lsMessage = lsFactTypeName & " is not a Fact Type. You need to either create the Fact Type or make sure the name does not conflict with the name of another Object Type."
            '    If arDSCError IsNot Nothing Then
            '        arDSCError.Success = False
            '        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
            '        arDSCError.ErrorString = lsMessage
            '    End If
            '    Me.send_data(lsMessage)
            '    Return False

            'ElseIf lrModelObject.GetType IsNot GetType(FBM.FactType) Then
            '    lsMessage = lsFactTypeName & " is not a Fact Type. You need to either create the Fact Type or make sure the name does not conflict with the name of another Object Type."
            '    If arDSCError IsNot Nothing Then
            '        arDSCError.Success = False
            '        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
            '        arDSCError.ErrorString = lsMessage
            '    End If
            '    Me.send_data(lsMessage)
            '    Return False

            'End If

            'lrFactType = lrModelObject

            ''=========================================================================
            ''Create the Internal Uniqueness Constraint

            'Dim larRole As New List(Of FBM.Role)


            'For liInd = 1 To Me.VAQLProcessor.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement.MODELELEMENT.Count - 1

            '    Dim lsModelElementName = Me.VAQLProcessor.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement.MODELELEMENT(liInd).MODELELEMENTNAME
            '    lrModelObject = Me.Model.GetModelObjectByName(lsModelElementName)

            '    If lrModelObject Is Nothing Then
            '        lsMessage = lsModelElementName & " is not a Object Type in the Model."
            '        If arDSCError IsNot Nothing Then
            '            arDSCError.Success = False
            '            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
            '            arDSCError.ErrorString = lsMessage
            '        End If
            '        Me.send_data(lsMessage)
            '        Return False
            '    ElseIf lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementName) Is Nothing Then
            '        lsMessage = lsModelElementName & " is not a Object Type that plays any Role in the Fact Type, " & lrFactType.Id & "."
            '        If arDSCError IsNot Nothing Then
            '            arDSCError.Success = False
            '            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
            '            arDSCError.ErrorString = lsMessage
            '        End If
            '        Me.send_data(lsMessage)
            '        Return False
            '    Else
            '        larRole.Add(lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementName))
            '    End If
            'Next

            'If Not Me.Model.StoreAsXML Then
            '    Call lrFact.Save()
            'End If

            Me.send_data("Ok")

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False

        End Try

    End Function 'Fact Statement


    Private Function ProcessISACONCEPTStatement() As Boolean

        Try
            Me.VAQLProcessor.ISACONCEPTStatement.MODELELEMENTNAME = ""
            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ISACONCEPTStatement, Me.VAQLParsetree.Nodes(0))

            Dim lsConceptName As String = Me.VAQLProcessor.ISACONCEPTStatement.MODELELEMENTNAME
            Dim lrConcept As New FBM.Concept(lsConceptName)
            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, lrConcept.Symbol, pcenumConceptType.GeneralConcept)

            If Me.Model.ModelDictionary.Exists(AddressOf lrDictionaryEntry.Equals) Then
                lrDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                If lrDictionaryEntry.isGeneralConcept Then
                    Me.OutputBuffer = Trim(lrDictionaryEntry.Symbol) & " is already a General Concept within the Model. But okay."
                    Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                Else
                    lrDictionaryEntry.AddConceptType(pcenumConceptType.GeneralConcept)
                    Me.OutputBuffer = "Okay"
                    Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)

                    Me.Model.MakeDirty(False, False)
                End If
            Else
                lrDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry, False, False,,,, True)

                Try
                    Me.OutputBuffer = "Okay"
                    Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                Catch ex As Exception
                    'Not a biggie.
                End Try


                Me.Model.MakeDirty(False, False)
            End If

            Return True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

    Private Sub ProcessStatementAddEntityType(ByRef arQuestion As tQuestion,
                                              Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

        Me.Model = prApplication.WorkingModel

        Dim lrEnityTypeInstance As FBM.EntityTypeInstance

        Dim lsOldEntityTypeName As String = ""
        Dim lsEntityTypeName As String = ""

        Me.Timeout.Stop()

        lsOldEntityTypeName = arQuestion.ModelObject(0).Id
        lsEntityTypeName = Viev.Strings.MakeCapCamelCase(arQuestion.ModelObject(0).Id)

        If IsSomething(arQuestion.sentence) Then
            arQuestion.sentence.Sentence = arQuestion.sentence.Sentence.Replace(lsOldEntityTypeName, lsEntityTypeName)
            arQuestion.sentence.ResetSentence()

            Call Language.AnalyseSentence(arQuestion.sentence, Me.Model)
            Call Language.ProcessSentence(arQuestion.sentence)
            If arQuestion.sentence.AreAllWordsResolved Then
                Call Language.ResolveSentence(arQuestion.sentence)
            End If

        End If

        Dim lrEntityType As FBM.EntityType

        lrEntityType = Me.Model.CreateEntityType(lsEntityTypeName, True, abBroadcastInterfaceEvent)

        If My.Settings.UseDefaultReferenceModeNewEntityTypes Then
            Call lrEntityType.SetReferenceMode(My.Settings.DefaultReferenceMode)
            Call lrEntityType.SetDataType(pcenumORMDataType.TextFixedLength, 50, 0, True)
        Else
            Me.send_data("Don't forget to give the new Entity Type a Primary Reference Scheme as soon as possible.")
        End If

        If Me.Page IsNot Nothing Then

            Select Case Me.Page.Language
                Case Is = pcenumLanguage.ORMModel
                    lrEnityTypeInstance = Me.Page.DropEntityTypeAtPoint(lrEntityType, New PointF(100, 10)) 'VM-20180329-Me.Page.Form.CreateEntityType(lsEntityTypeName, True)

                    Call lrEnityTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                    Call lrEnityTypeInstance.Move(lrEnityTypeInstance.X, lrEnityTypeInstance.Y, True)
                Case Is = pcenumLanguage.PropertyGraphSchema

                    Call Me.Page.LoadPGSNodeTypeFromRDSTable(lrEntityType.getCorrespondingRDSTable, New PointF(50, 50), True)

            End Select


            If Me.AutoLayoutOn Then
                Me.Page.Form.AutoLayout()
            End If
        End If

        Me.Timeout.Start()

    End Sub

    Private Function executeStatementAddFactType(ByRef arQuestion As tQuestion,
                                                 Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                 Optional abStraightToActionProcessing As Boolean = False,
                                                 Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lrFactType As FBM.FactType
        Dim lrResolvedWord As Language.WordResolved
        Dim lrEntityType As FBM.EntityType
        Dim lrValueType As FBM.ValueType = Nothing
        Dim lrJoinedFactType As FBM.FactType = Nothing
        Dim larModelObject As New List(Of FBM.ModelObject)
        Dim lsMessage As String

        Try
            Me.Model = prApplication.WorkingModel

            executeStatementAddFactType = True

            For Each lrResolvedWord In arQuestion.sentence.WordListResolved.FindAll(Function(x) x.Sense = pcenumWordSense.Noun)
                lrEntityType = New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lrResolvedWord.Word, Nothing, True)
                lrEntityType = Me.Model.EntityType.Find(AddressOf lrEntityType.Equals)

                If lrEntityType Is Nothing Then
                    lrValueType = New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lrResolvedWord.Word, True)
                    lrValueType = Me.Model.ValueType.Find(AddressOf lrValueType.Equals)
                    If lrValueType Is Nothing Then
                        lrJoinedFactType = New FBM.FactType(Me.Model, lrResolvedWord.Word, True)
                        lrJoinedFactType = Me.Model.FactType.Find(AddressOf lrJoinedFactType.Equals)

                        If lrJoinedFactType.IsObjectified Then
                            larModelObject.Add(lrJoinedFactType)
                        Else
                            lsMessage = ""
                            lsMessage = "The Fact Type, '" & lrJoinedFactType.Name & "', must be objectified before it can be involved in other Fact Types."
                            lsMessage &= vbCrLf & "Objectify the Fact Type and try again."
                            Me.OutputBuffer = lsMessage
                            Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                            Return False
                            Exit Function
                        End If
                    Else
                        larModelObject.Add(lrValueType)
                    End If
                Else
                    larModelObject.Add(lrEntityType)
                End If
            Next

            Dim lrModelObject As FBM.ModelObject
            Dim lsFactTypeName As String = ""

            If arQuestion.ObjectType IsNot Nothing Then
                lsFactTypeName = arQuestion.ObjectType.Id
            Else
                For Each lrModelObject In larModelObject
                    lsFactTypeName &= lrModelObject.Name
                Next
            End If

            If arQuestion.sentence.PredicatePart.Count = 1 Then
                lsFactTypeName &= Viev.Strings.MakeCapCamelCase(arQuestion.sentence.PredicatePart(0).PredicatePartText)
                lsFactTypeName = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
            End If

            '==========================================================================
            'Adding the FactType to the Model is done in the 'DropFactTypeAtPoint' stage, 
            '  which also broadcasts the event if in Client/Server mode. The below just creates the FactType ready for adding to the Model.
            lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject, False, True, , , False,,, abBroadcastInterfaceEvent)

            Dim larRole As New List(Of FBM.Role)

            For Each lrRole In lrFactType.RoleGroup
                larRole.Add(lrRole)
            Next

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim loPointF As PointF

#Region "FactTypeReading"
            Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, arQuestion.sentence)
            lrFactTypeReading.IsPreferred = True

            Dim larFactTypeReading = From FactType In Me.Model.FactType
                                     From FactTypeReading In FactType.FactTypeReading
                                     Where FactTypeReading.EqualsByPredicatePartTextModelElements(lrFactTypeReading)
                                     Select FactTypeReading

            If larFactTypeReading.Count > 0 Then
                arDSCError.Success = False
                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                arDSCError.ErrorString = "A Fact Type with this predicate already exists."
                Return False
            End If
#End Region

            Call Me.Model.AddFactType(lrFactType)

            '====================================================================
            'Check to see that the FactType doesn't clash with another FactType
            Dim lrClashFactType = Me.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject, lrFactTypeReading)
            If lrClashFactType IsNot Nothing Then
                Call Me.send_data("A Fact Type already exists with the Fact Type Reading, '" & lrFactTypeReading.GetReadingText & "'.")
                Return False
            End If

            Call lrFactType.AddFactTypeReading(lrFactTypeReading, False, abBroadcastInterfaceEvent)

            'Additional FactTypeReadings
            If arQuestion.AdditionalSentence IsNot Nothing Then
                For Each lrSentence In arQuestion.AdditionalSentence

                    larRole = New List(Of FBM.Role)
                    For Each lrPredicatePart In lrSentence.PredicatePart
                        Dim lrPredicateRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lrPredicatePart.ObjectName _
                                                                                    And Not larRole.Contains(x))
                        larRole.Add(lrPredicateRole)
                    Next
                    lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lrSentence)

                    If lrFactType.ExistsFactTypeReadingByRoleSequence(lrFactTypeReading) Then

                        lrFactTypeReading.TypedPredicateId = lrFactType.GetTypedPredicateIdByRoleSequence(lrFactTypeReading)

                        lrFactTypeReading.IsPreferredForPredicate = (lrFactType.FactTypeReading.FindAll(AddressOf lrFactTypeReading.EqualsByRoleSequence).Count = 0)

                        '---------------------------------------------------------------------------------------------
                        'Check to see whether the Fact Type has more than one Role referencing the same Object Type.
                        '---------------------------------------------------------------------------------------------
                        If lrFactType.HasMoreThanOneRoleReferencingTheSameModelObject Then
                            '---------------------------------------------------------------------------------------------------
                            'Need to check to see whether an alternate sequence of Roles (within the FTR) is possible for the 
                            '  FactType. It may well be that each possible combination of FTR Role Sequences has been filled
                            '  for the Fact Type, which is a good scenario. If this is the case, then simply ther is no new
                            '  FTR to apply to the Fact Type.
                            '  Otherwise, the algorithm is to select an unused FTR/Role Sequence combination and apply that
                            '  automatically for the Fact Type.
                            '---------------------------------------------------------------------------------------------------
                            If lrFactTypeReading.FactType.ExistsAvailablePermutation(lrFactTypeReading) Then
                                lrFactTypeReading = lrFactTypeReading.FactType.TransformFactTypeReadingToAvailablePermutation(lrFactTypeReading)
                                lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)

                            End If
                        Else
                            lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)
                        End If
                    Else
                        lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)
                    End If
                Next
            End If

            If arQuestion.DerivationText IsNot Nothing Then
                lrFactType.IsDerived = True
                lrFactType.DerivationText = arQuestion.DerivationText
            End If

            If arQuestion.ObjectType IsNot Nothing Then
                Call lrFactType.setName(lsFactTypeName, False)
            Else
                If lrFactType.MakeNameFromFactTypeReadings <> lrFactType.Id Then
                    Call lrFactType.setName(lrFactType.MakeNameFromFactTypeReadings, False)
                End If
            End If

            'Create a Column for unary FactTypes. Is an exception because does not have a RoleConstraint
            If lrFactType.RoleGroup.Count = 1 Then
                Call Me.Model.createColumnForUnaryFactType(lrFactType)
            End If

            If lrFactType.Arity = 2 And Not abStraightToActionProcessing Then
                Call Me.FormulateQuestionCreateInternalUniquenessConstraint(lrFactType, lrFactTypeReading)
            End If

            If Me.Page Is Nothing Then
                Me.Model.AddFactType(lrFactType, True, abBroadcastInterfaceEvent, Nothing)
            Else
                Select Case Me.Page.Language
                    Case Is = pcenumLanguage.ORMModel
                        Dim lbAllObjectsOnPage As Boolean = True
                        For Each lrRole In lrFactType.RoleGroup
                            If Not Me.Page.ContainsModelElement(lrRole.JoinedORMObject) Then
                                lbAllObjectsOnPage = False
                            End If
                        Next

                        If lbAllObjectsOnPage Then
                            loPointF = Me.Page.GetMidPointOfModelObjects(lrFactType.GetModelObjects)
                        Else
                            loPointF = New PointF(100, 100)
                        End If

                        lrFactTypeInstance = Me.Page.DropFactTypeAtPoint(lrFactType, loPointF, False, False, abBroadcastInterfaceEvent)
                        lrFactTypeInstance.Visible = True

                        Call lrFactTypeInstance.RepellFromNeighbouringPageObjects(0, abBroadcastInterfaceEvent)

                        Dim loPoint As New PointF(100, 100)
                        If lrFactTypeInstance.Arity = 2 Then
                            Dim lrFirstModelObject, lrSecondModelObject As Object

                            lrFirstModelObject = lrFactTypeInstance.RoleGroup(0).JoinedORMObject
                            lrSecondModelObject = lrFactTypeInstance.RoleGroup(1).JoinedORMObject

                            loPoint.X = (lrFirstModelObject.X + lrSecondModelObject.X) / 2
                            loPoint.Y = (lrFirstModelObject.y + lrSecondModelObject.Y) / 2

                            lrFactTypeInstance.Move(loPoint.X, loPoint.Y, abBroadcastInterfaceEvent)
                        End If

                        Call lrFactTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                        Call lrFactTypeInstance.Move(lrFactTypeInstance.X, lrFactTypeInstance.Y, abBroadcastInterfaceEvent)

                    Case Is = pcenumLanguage.PropertyGraphSchema

                        Me.Model.AddFactType(lrFactType, True, abBroadcastInterfaceEvent, Nothing)

                        If lrFactType.Arity > 2 Then
                            Call lrFactType.CreateInternalUniquenessConstraint(lrFactType.RoleGroup, True, True, True, False, Nothing, True, False)
                            Call lrFactType.Objectify(False, True)
                            Dim lrTable = lrFactType.getCorrespondingRDSTable(Nothing, True)
                            If lrTable IsNot Nothing Then
                                Call Me.Page.LoadPGSNodeTypeFromRDSTable(lrFactType.getCorrespondingRDSTable, loPointF)
                            End If
                        Else
                            lsMessage = "NB The new Fact Type has no Internal Uniqueness Constraint, and so no Edge Type will appear for the Fact Type."
                            lsMessage.AppendLine("Edit the Fact Type in an Object-Role Model view to add an Internal Uniqueness Constraint to the Fact Type.")
                            lsMessage.AppendLine("Hint: In the future use something like:" & lrFactType.RoleGroup(0).JoinedORMObject.Id & " has ONE " & lrFactType.RoleGroup(1).JoinedORMObject.Id)
                            Call Me.send_data(lsMessage)
                        End If
                End Select

                If Me.AutoLayoutOn Then
                    Me.Page.Form.AutoLayout()
                End If

            End If

EndProcessing:
            Try
                Me.OutputChannel.Focus()
            Catch ex As Exception
            End Try

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    ''' <summary>
    ''' Used when the ModelElements are already identified
    '''   e.g. When a Sentence of type 'Person has AT MOST ONE FirstName' is processed, the ModelElements are identified by the VAQL Parser.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function executeStatementAddFactTypePredetermined(ByRef arQuestion As tQuestion,
                                                              Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                              Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lrFactType As FBM.FactType
        Dim lsResolvedModelElementName As String
        Dim lrModelObject As FBM.ModelObject
        Dim larModelObject As New List(Of FBM.ModelObject)

        Try
            Me.Model = prApplication.WorkingModel

            executeStatementAddFactTypePredetermined = True

            For Each lsResolvedModelElementName In arQuestion.FocalSymbol
                lrModelObject = Me.Model.GetModelObjectByName(lsResolvedModelElementName)
                If IsSomething(lrModelObject) Then
                    larModelObject.Add(lrModelObject)
                Else
                    executeStatementAddFactTypePredetermined = False
                    Exit Function
                End If
            Next

            Dim lsFactTypeName As String = ""

            For Each lrModelObject In larModelObject
                lsFactTypeName &= lrModelObject.Name
            Next

            lsFactTypeName = Me.Model.CreateUniqueFactTypeName(lsFactTypeName, 0, True)

            lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject, False, True, , , False,  ,, abBroadcastInterfaceEvent)

            Dim lrRole As FBM.Role
            Dim larRole As New List(Of FBM.Role)

            If arQuestion.PlanStep.FactTypeAttributes.Count > 0 Then

                If arQuestion.PlanStep.FactTypeAttributes.Contains(pcenumStepFactTypeAttributes.MandatoryFirstRole) Then
                    Dim lsModelElementId As String = arQuestion.FocalSymbol(0)

                    lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementId)
                    lrRole.SetMandatory(True, abBroadcastInterfaceEvent)
                End If

            End If

            '==========================================================================================
            'FactTypeReading            
            larRole.Clear()
            For Each lrRole In lrFactType.RoleGroup
                larRole.Add(lrRole)
            Next

            Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, arQuestion.sentence)

            Dim larFactTypeReading = From FactType In Me.Model.FactType
                                     From FactTypeReading In FactType.FactTypeReading
                                     Where FactTypeReading.EqualsByPredicatePartTextModelElements(lrFactTypeReading)
                                     Select FactTypeReading

            If larFactTypeReading.Count > 0 Then
                arDSCError.Success = False
                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                arDSCError.ErrorString = "A Fact Type with this predicate already exists."
                Return False
            End If

            Call lrFactType.AddFactTypeReading(lrFactTypeReading, True, abBroadcastInterfaceEvent)

            If lrFactType.MakeNameFromFactTypeReadings <> lrFactType.Id Then
                lrFactType.setName(lrFactType.MakeNameFromFactTypeReadings, abBroadcastInterfaceEvent)
            End If

            Call Me.Model.AddFactType(lrFactType)
            '===========================================================

            '===========================
            'RoleConstraint
            larRole = New List(Of FBM.Role)
            Dim lrRoleConstraint As FBM.RoleConstraint

            If arQuestion.PlanStep.FactTypeAttributes.Contains(pcenumStepFactTypeAttributes.OneToMany) Then

                Dim lsModelElementId As String = arQuestion.FocalSymbol(1)

                lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementId)
                larRole.Add(lrRole)

                lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                                                                 larRole,
                                                                 "InternalUniquenessConstraint",
                                                                 1,
                                                                 False,
                                                                 False)

                lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)

                Me.Model.AddRoleConstraint(lrRoleConstraint, True, abBroadcastInterfaceEvent)


            ElseIf arQuestion.PlanStep.FactTypeAttributes.Contains(pcenumStepFactTypeAttributes.ManyToOne) Then

                Dim lsModelElementId As String = arQuestion.FocalSymbol(0)

                lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementId)
                larRole.Add(lrRole)

                lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                                                                 larRole,
                                                                 "InternalUniquenessConstraint",
                                                                 1,
                                                                 False,
                                                                 False)

                lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)

                Me.Model.AddRoleConstraint(lrRoleConstraint, True, abBroadcastInterfaceEvent)

            ElseIf arQuestion.PlanStep.FactTypeAttributes.Contains(pcenumStepFactTypeAttributes.ManyToMany) Then

                lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                                                                     lrFactType.RoleGroup,
                                                                     "InternalUniquenessConstraint",
                                                                     1,
                                                                     False,
                                                                     False)

                lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)

                Me.Model.AddRoleConstraint(lrRoleConstraint, True, abBroadcastInterfaceEvent)
            End If

            '===========================
#Region "Page Drawing - Model Elements"
            If Me.Page IsNot Nothing Then
                Select Case Me.Page.Language
                    Case Is = pcenumLanguage.ORMModel
                        Dim lrFactTypeInstance As FBM.FactTypeInstance
                        Dim loPointF As PointF

                        Dim lbAllObjectsOnPage As Boolean = True
                        For Each lrRole In lrFactType.RoleGroup
                            If Not Me.Page.ContainsModelElement(lrRole.JoinedORMObject) Then
                                lbAllObjectsOnPage = False
                            End If
                        Next

                        If lbAllObjectsOnPage Then
                            loPointF = Me.Page.GetMidPointOfModelObjects(lrFactType.GetModelObjects)
                        Else
                            loPointF = New PointF(100, 100)
                        End If

                        lrFactTypeInstance = Me.Page.DropFactTypeAtPoint(lrFactType, loPointF, False, False, abBroadcastInterfaceEvent)

                        Call lrFactTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                        Call lrFactTypeInstance.Move(lrFactTypeInstance.X, lrFactTypeInstance.Y, abBroadcastInterfaceEvent)
                    Case Else
                        'NA for ERD and PGS Pages, because will automatically look to create new links (Foreign Key References, Edge Types).

                End Select

                If Me.AutoLayoutOn Then
                    Me.Page.Form.AutoLayout()
                End If
            End If
#End Region

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Private Function ProcessStatementAddFactTypeReading() As Boolean

        Dim larModelObject As New List(Of FBM.ModelObject)
        Dim lrModelObject As FBM.ModelObject
        Dim lrFactType As FBM.FactType
        Dim lrResolvedWord As Language.WordResolved
        Dim lrRole As FBM.Role
        Dim larRole As New List(Of FBM.Role)

        Try
            Me.Model = prApplication.WorkingModel

            ProcessStatementAddFactTypeReading = True

            lrFactType = New FBM.FactType(Me.Model, "DummyFactType", True)

            For Each lrResolvedWord In Me.CurrentQuestion.sentence.WordListResolved.FindAll(Function(x) x.Sense = pcenumWordSense.Noun)

                lrModelObject = New FBM.ModelObject(lrResolvedWord.Word, pcenumConceptType.ValueType)
                lrRole = New FBM.Role(lrFactType, New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lrModelObject.Id, True))
                lrFactType.RoleGroup.Add(lrRole)
                larModelObject.Add(lrModelObject)
            Next

            lrFactType = Me.Model.FactType.Find(AddressOf lrFactType.EqualsByModelElements)
            For Each lrResolvedWord In Me.CurrentQuestion.sentence.WordListResolved.FindAll(Function(x) x.Sense = pcenumWordSense.Noun)
                larRole.Add(lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lrResolvedWord.Word))
            Next

            'CodeSafe
#Region "CodeSafe: Make sure the Sentence has the right number of PredicateParts"
            If Me.CurrentQuestion.sentence.PredicatePart.Count < lrFactType.RoleGroup.Count Then
                For liInd = 1 To lrFactType.RoleGroup.Count - Me.CurrentQuestion.sentence.PredicatePart.Count
                    Dim lrPredicatePart = New Language.PredicatePart("")
                    Me.CurrentQuestion.sentence.PredicatePart.Add(lrPredicatePart)
                Next
            End If
#End Region

            Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, Me.CurrentQuestion.sentence)

            Call lrFactType.AddFactTypeReading(lrFactTypeReading, False, True)

            If Me.Page IsNot Nothing Then

                Select Case Me.Page.Language
                    Case Is = pcenumLanguage.ORMModel
                        Dim lrFactTypeInstance As New FBM.FactTypeInstance
                        lrFactTypeInstance = lrFactType.CloneInstance(Me.Page, False)
                        lrFactTypeInstance = Me.Page.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)

                        If lrFactTypeInstance IsNot Nothing Then
                            Call lrFactTypeInstance.SetSuitableFactTypeReading()
                        End If
                    Case Is = pcenumLanguage.PropertyGraphSchema

                        Call Me.Page.Form.ResetNodeAndLinkColors

                End Select

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Private Sub ProcessStatementAddValueType(ByRef arQuestion As tQuestion,
                                             Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

        Dim lrValueTypeInstance As FBM.ValueTypeInstance

        Try

            Dim lsOldValueTypeName As String = ""
            Dim lsValueTypeName As String = ""

            Me.Timeout.Stop()

            Me.Model = prApplication.WorkingModel

            Dim lrValueType As FBM.ValueType

            If arQuestion.ObjectType IsNot Nothing Then
                lrValueType = arQuestion.ObjectType '20220829-VM-Was Me.CurrentQuestion.ObjectType
            Else

                Dim lrDummyValueType As FBM.ValueType = arQuestion.ValueType(0)
                lsOldValueTypeName = arQuestion.ValueType(0).Id
                lsValueTypeName = Viev.Strings.MakeCapCamelCase(arQuestion.ValueType(0).Id)

                If arQuestion.sentence IsNot Nothing And Me.CurrentQuestion IsNot Nothing Then
                    arQuestion.sentence.Sentence = Me.CurrentQuestion.sentence.Sentence.Replace(lsOldValueTypeName, lsValueTypeName)
                    arQuestion.sentence.ResetSentence()

                    'Call Language.AnalyseSentence(Me.CurrentQuestion.sentence, Me.Model)
                    Call Language.ProcessSentence(arQuestion.sentence)
                    If arQuestion.sentence.AreAllWordsResolved Then
                        Call Language.ResolveSentence(arQuestion.sentence)
                    End If
                End If

                lrValueType = Me.Model.CreateValueType(lsValueTypeName,
                                                       False,
                                                       lrDummyValueType.DataType,
                                                       lrDummyValueType.DataTypeLength,
                                                       lrDummyValueType.DataTypePrecision,
                                                       abBroadcastInterfaceEvent)

            End If

            'Add Error to ValueType because does not have a DataType
            Call lrValueType.CheckForErrors()

            Me.Model.AddValueType(lrValueType, True, abBroadcastInterfaceEvent, Nothing, True)

            If Me.Page IsNot Nothing Then

                Select Case Me.Page.Language
                    Case Is = pcenumLanguage.ORMModel
                        lrValueTypeInstance = Me.Page.DropValueTypeAtPoint(lrValueType, New PointF(100, 100)) 'VM-20181329-Remove this commented-out code, if all okay. Me.Page.Form.CreateValueType(lsValueTypeName, True)

                        If Me.Page.Diagram IsNot Nothing Then
                            Call lrValueTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                            Call lrValueTypeInstance.Move(lrValueTypeInstance.X, lrValueTypeInstance.Y, abBroadcastInterfaceEvent)

                            If Me.AutoLayoutOn Then
                                Me.Page.Form.AutoLayout()
                            End If
                        End If
                End Select
            End If

            Me.Timeout.Start()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Me.Timeout.Start()
        End Try

    End Sub

    Private Function ProcessISANENTITYTYPECLAUSE(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                 Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String

        Try
            With New WaitCursor
                Me.Model = prApplication.WorkingModel

                Me.VAQLProcessor.ISANENTITYTYPEStatement.KEYWDISANENTITYTYPE = ""
                Me.VAQLProcessor.ISANENTITYTYPEStatement.MODELELEMENTNAME = ""

                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ISANENTITYTYPEStatement, Me.VAQLParsetree.Nodes(0))

                Me.Timeout.Stop()

                Dim lsEntityTypeName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQLProcessor.ISANENTITYTYPEStatement.MODELELEMENTNAME))

                If Me.Model.ExistsModelElement(lsEntityTypeName) Then
                    lsMessage = "There is already a Model Element with the name, '" & lsEntityTypeName & "'. Try another name"
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                End If

                '20220711-VM-Commented out in lieu of the above.
                'If Me.Model.ModelDictionary.Find(Function(x) x.Symbol = lsEntityTypeName And x.isEntityType) IsNot Nothing Then
                '    Me.send_data("I know.")
                '    Exit Sub
                'End If

                'Have already checked to see wither it is okay to create the EntityType above.
                Dim lrEntityType = Me.Model.CreateEntityType(Trim(lsEntityTypeName), True, abBroadcastInterfaceEvent)

                If My.Settings.UseDefaultReferenceModeNewEntityTypes Then
                    Call lrEntityType.SetReferenceMode(My.Settings.DefaultReferenceMode,,, abBroadcastInterfaceEvent)
                    Call lrEntityType.SetDataType(pcenumORMDataType.TextFixedLength, 50, 0, abBroadcastInterfaceEvent)
                Else
                    Me.send_data("Don't forget to give the new Entity Type a Primary Reference Scheme as soon as possible.")
                    Dim lrModelError As New FBM.ModelError(pcenumModelErrors.EntityTypeRequiresReferenceSchemeError, "", Nothing, lrEntityType, True)
                End If

                Try
                    Select Case prApplication.WorkingPage.Language
                        Case Is = pcenumLanguage.ORMModel,
                                  pcenumLanguage.PropertyGraphSchema
                            Me.Page = prApplication.WorkingPage
                    End Select
                Catch ex As Exception
                    'Not a biggie if left as is
                End Try

                If Me.Page IsNot Nothing Then

                    Select Case Me.Page.Language
                        Case Is = pcenumLanguage.ORMModel
                            Dim lrEnityTypeInstance = Me.Page.DropEntityTypeAtPoint(lrEntityType, New PointF(100, 10)) 'VM-20180329-Me.Page.Form.CreateEntityType(lsEntityTypeName, True)

                            Call lrEnityTypeInstance.RepellFromNeighbouringPageObjects(1, abBroadcastInterfaceEvent)
                            Call lrEnityTypeInstance.Move(lrEnityTypeInstance.X, lrEnityTypeInstance.Y, abBroadcastInterfaceEvent)
                        Case Is = pcenumLanguage.PropertyGraphSchema

                            Call Me.Page.LoadPGSNodeTypeFromRDSTable(lrEntityType.getCorrespondingRDSTable, New PointF(50, 50), True)
                        Case Is = pcenumLanguage.EntityRelationshipDiagram

                            Call Me.Page.LoadERDEntityFromRDSTable(lrEntityType.getCorrespondingRDSTable, New PointF(50, 50))
                    End Select

                    If Me.AutoLayoutOn Then
                        Me.Page.Form.AutoLayout()
                    End If
                End If

                Me.send_data("Ok")

                Me.Timeout.Start()
            End With

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

    Private Function ProcessVALUECONSTRAINTCLAUSE(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                  Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String

        Try
            With New WaitCursor
                Me.Model = prApplication.WorkingModel

                Me.VAQLProcessor.VALUECONSTRAINTClause.MODELELEMENTNAME = ""
                Me.VAQLProcessor.VALUECONSTRAINTClause.VALUECONSTRAINTVALUE = New List(Of String)

                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.VALUECONSTRAINTClause, Me.VAQLParsetree.Nodes(0))

                Me.Timeout.Stop()

                Dim lsValueTypeName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQLProcessor.VALUECONSTRAINTClause.MODELELEMENTNAME))

                If Not Me.Model.ExistsModelElement(lsValueTypeName) Then
                    lsMessage = "There is no Model Element with the name, '" & lsValueTypeName & "'. Try another name"
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                End If

                Dim lrModelElement As FBM.ModelObject = Me.Model.GetModelObjectByName(lsValueTypeName, True)

                If Not lrModelElement.GetType = GetType(FBM.ValueType) Then
                    lsMessage = "There is no Value Type called, '" & lsValueTypeName & "'."
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                End If

                Dim lrValueType = CType(lrModelElement, FBM.ValueType)


                For Each lsValueConstraintValue In Me.VAQLProcessor.VALUECONSTRAINTClause.VALUECONSTRAINTVALUE

                    If lrValueType.ValueConstraint.Contains(lsValueConstraintValue) Then
                        lsMessage = "The value, " & lsValueConstraintValue & ", already exists in the Value Constraint for Value Type, " & lsValueTypeName & "."
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)
                        Return False
                    Else
                        Call lrValueType.AddValueConstraint(lsValueConstraintValue)
                    End If
                Next

                Call Me.Model.MakeDirty(True, False, lrValueType)

                Me.send_data("Ok")

                Me.Timeout.Start()
            End With

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False

        End Try

    End Function

    Private Function ProcessADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                                             Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String

        Try
            With New WaitCursor

                Me.Model = prApplication.WorkingModel

                Me.VAQLProcessor.ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement.KEYWDADDOBJECTTYPESRELATEDTO = ""
                Me.VAQLProcessor.ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement.MODELELEMENTNAME = ""
                Me.VAQLProcessor.ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement.PAGENAME = ""
                Me.VAQLProcessor.ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement.KEYWDANYFACTTYPE = Nothing

                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement, Me.VAQLParsetree.Nodes(0))

                Me.Timeout.Stop()

                Dim lsModelElementName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQLProcessor.ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement.MODELELEMENTNAME))

                Dim lrModelObject As FBM.ModelObject = Me.Model.GetModelObjectByName(lsModelElementName, True)

                If lrModelObject Is Nothing Then
                    lsMessage = "That's not a valid Object Type Name."
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                Else
                    Dim lsPageName As String = Me.VAQLProcessor.ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement.PAGENAME

                    Dim lrPage As FBM.Page = Me.Model.Page.Find(Function(x) x.Name = lsPageName)

                    If lrPage Is Nothing Then
                        lsMessage = "That's not a valid Page Name in the Model."
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)
                        Return False
                    Else
                        If lrPage.getModelElementById(lrModelObject.Id) Is Nothing Then
                            Call lrPage.DropModelElementAtPoint(lrModelObject, New PointF(Boston.RandomInteger(10, 100), Boston.RandomInteger(10, 100)), abBroadcastInterfaceEvent, False)
                        End If

                        Dim larFactType As List(Of FBM.FactType) = Nothing

                        If Me.VAQLProcessor.ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement.KEYWDANYFACTTYPE IsNot Nothing Then
                            larFactType = lrModelObject.getOutgoingFactTypes(True).FindAll(Function(x) x.IsLinkFactType = False)
                        Else
                            larFactType = lrModelObject.getOutgoingFactTypes.FindAll(Function(x) x.IsLinkFactType = False)
                        End If


                        For Each lrFactType In larFactType
                            Call lrPage.DropFactTypeAtPoint(lrFactType, New PointF(Boston.RandomInteger(10, 100), Boston.RandomInteger(10, 100)), False,, abBroadcastInterfaceEvent, False, True, False, False)
                        Next
                    End If
                End If

            End With

            Try
                Me.OutputBuffer = "Okay"
                Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
            Catch ex As Exception
                'Not a biggie.
            End Try

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

    Private Function ProcessCREATEPAGEStatement(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String
        Dim lsErrorMessage = "Error creating the new Page."

        Try
            With New WaitCursor
                Me.Model = prApplication.WorkingModel

                Me.VAQLProcessor.CREATEPAGEStatement.KEYWDCREATE = ""
                Me.VAQLProcessor.CREATEPAGEStatement.KEYWDPAGE = ""
                Me.VAQLProcessor.CREATEPAGEStatement.PAGENAME = ""

                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.CREATEPAGEStatement, Me.VAQLParsetree.Nodes(0))

                Me.Timeout.Stop()

                Dim lsPageName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQLProcessor.CREATEPAGEStatement.PAGENAME))

                Dim lrPage As New FBM.Page(Me.Model, Nothing, lsPageName, pcenumLanguage.ORMModel)

                If Me.Model.Page.Find(Function(x) x.Name = lrPage.Name) IsNot Nothing Then
                    lsErrorMessage = "A Page with the name, " & lrPage.Name & ", already exists."
                End If

                If lrPage Is Nothing Or Me.Model.Page.Find(Function(x) x.Name = lrPage.Name) IsNot Nothing Then
                    lsMessage = lsErrorMessage
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                Else
                    Try
                        Me.Model.Page.AddUnique(lrPage)
                        lrPage.Loaded = True
                        If frmMain.zfrmModelExplorer IsNot Nothing Then
                            frmMain.zfrmModelExplorer.AddExistingPageToModel(lrPage, Me.Model, Nothing, True)
                        End If
                    Catch ex As Exception
                        'Not a biggie
                    End Try
                End If

                Try
                    Me.OutputBuffer = "Okay"
                    Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                Catch ex As Exception
                    'Not a biggie.
                End Try

                Return True

            End With

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function


    Private Function ProcessADDOBJECTTYPETOPAGEStatement(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                         Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String

        Try
            With New WaitCursor
                Me.Model = prApplication.WorkingModel

                Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement.KEYWDADDOBJECTTYPE = ""
                Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement.KEYWDTOPAGE = ""
                Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement.KEYWDSTANDALONE = Nothing
                Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement.MODELELEMENTNAME = ""
                Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement.PAGENAME = ""

                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement, Me.VAQLParsetree.Nodes(0))

                Me.Timeout.Stop()

                Dim lsModelElementName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement.MODELELEMENTNAME))

                Dim lrModelObject As FBM.ModelObject = Me.Model.GetModelObjectByName(lsModelElementName, True)

                If lrModelObject Is Nothing Then
                    lsMessage = "That's not a valid Object Type Name."
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                Else
                    Dim lsPageName As String = Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement.PAGENAME

                    Dim lrPage As FBM.Page = Me.Model.Page.Find(Function(x) x.Name = lsPageName)

                    If lrPage Is Nothing Then
                        lsMessage = "That's not a valid Page Name in the Model."
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)
                        Return False
                    Else
                        Call lrPage.DropModelElementAtPoint(lrModelObject, New PointF(Boston.RandomInteger(10, 100), Boston.RandomInteger(10, 100)), abBroadcastInterfaceEvent, False)

                        If Me.VAQLProcessor.ADDOBJECTTYPETOPAGEStatement.KEYWDSTANDALONE Is Nothing Then
                            Dim larFactType = lrModelObject.getOutgoingFactTypes()

                            For Each lrFactType In larFactType
                                Call lrPage.DropFactTypeAtPoint(lrFactType, New PointF(Boston.RandomInteger(10, 100), Boston.RandomInteger(10, 100)), False,, abBroadcastInterfaceEvent, False, True, False, False)
                            Next
                        End If

                        lrPage.Loaded = True
                    End If
                End If

                Try
                    Me.OutputBuffer = "Okay"
                    Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                Catch ex As Exception
                    'Not a biggie.
                End Try

                Return True

            End With

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function


    Private Function ProcessISAVALUETYPECLAUSE(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                               Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String

        Try
            With New WaitCursor
                Me.Model = prApplication.WorkingModel

                Me.VAQLProcessor.ISAVALUETYPEStatement.KEYWDISAVALUETYPE = ""
                Me.VAQLProcessor.ISAVALUETYPEStatement.MODELELEMENTNAME = ""
                Me.VAQLProcessor.ISAVALUETYPEStatement.KEYWDWRITTENAS = Nothing
                Me.VAQLProcessor.ISAVALUETYPEStatement.VALUETYPEWRITTENASCLAUSE = Nothing

                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ISAVALUETYPEStatement, Me.VAQLParsetree.Nodes(0))

                Me.Timeout.Stop()

                Dim lsValueTypeName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQLProcessor.ISAVALUETYPEStatement.MODELELEMENTNAME))

                If Me.Model.ExistsModelElement(lsValueTypeName) Then
                    lsMessage = "There is already a Model Element with the name, '" & lsValueTypeName & "'. Try another name"
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False
                End If

                '20220711VM-Was. But covered by the above.
                'If Me.Model.ModelDictionary.Find(Function(x) x.Symbol = lsValueTypeName And x.isValueType) IsNot Nothing Then
                '    Me.send_data("I know.")
                '    Exit Sub
                'End If

                'Have already checked to see wither it is okay to create the ValueType above.
                Dim lrValueType = Me.Model.CreateValueType(Trim(lsValueTypeName), True,,,, abBroadcastInterfaceEvent)

                '=================================================================================================================
                'Check if the DataType of the ValueType has been specified
                Dim lsDataTypeName As String = ""
                Dim liDataTypeLength As Integer = 0
                Dim liDataTypePrecision As Integer = 0
                Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet

                If Me.VAQLProcessor.ISAVALUETYPEStatement.KEYWDWRITTENAS IsNot Nothing Then

                    Me.VAQLProcessor.VALUETYPEWRITTENASClause = Me.VAQLProcessor.ISAVALUETYPEStatement.VALUETYPEWRITTENASCLAUSE

                    If Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE IsNot Nothing Then
                        lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE.Nodes(0).Token.Text
                    ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH IsNot Nothing Then
                        lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH.Nodes(0).Token.Text
                        liDataTypeLength = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                    ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION IsNot Nothing Then
                        lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION.Nodes(0).Token.Text
                        liDataTypePrecision = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                    End If

                    lsDataTypeName = DataTypeAttribute.Get(GetType(pcenumORMDataType), lsDataTypeName)
                    If lsDataTypeName Is Nothing Then
                        lsMessage = "That's not a valid Data Type."
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)
                        Return False
                    End If

                    Try
                        liDataType = DirectCast([Enum].Parse(GetType(pcenumORMDataType), lsDataTypeName), pcenumORMDataType)
                    Catch ex As Exception
                        lsMessage = "That's not a valid Data Type."
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.UndocumentedError
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)
                        Return False
                    End Try

                    Call lrValueType.SetDataType(liDataType, liDataTypeLength, liDataTypePrecision, abBroadcastInterfaceEvent)

                    Dim lrModelError As New FBM.ModelError(127, lrValueType)
                    lrValueType._ModelError.RemoveAll(AddressOf lrModelError.EqualsByErrorIdModelElementId)
                    Call Me.Model.RemoveModelError(lrModelError)
                Else
                    lsMessage = "Remember that you can set the Data Type for the Value Type by saying something like:"
                    lsMessage.AppendLine(lrValueType.Id & " IS A VALUE TYPE WRITTEN AS AutoCounter")
                    lsMessage.AppendLine("I.e. Add a 'WRITTEN AS' clause to your 'IS A VALUE TYPE' statement.")
                    Me.send_data(lsMessage)
                End If
                '=================================================================================================================

                If Me.Page IsNot Nothing Then
                    Select Case Me.Page.Language
                        Case Is = pcenumLanguage.ORMModel

                            Dim lrValueTypeInstance = Me.Page.DropValueTypeAtPoint(lrValueType, New PointF(100, 100)) 'VM-20180329-Me.Page.Form.CreateEntityType(lsEntityTypeName, True)

                            Call lrValueTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                            Call lrValueTypeInstance.Move(lrValueTypeInstance.X, lrValueTypeInstance.Y, abBroadcastInterfaceEvent)

                    End Select

                    If Me.AutoLayoutOn Then
                        Me.Page.Form.AutoLayout()
                    End If

                End If

                Me.send_data("Ok")

                Me.Timeout.Start()
            End With

            Return True
        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

    Public Sub executeStatementAddInternalUniquenessConstraint(ByRef arQuestion As tQuestion, ByVal asInputBuffer As String)

        Try
            Dim lrFactType As FBM.FactType = arQuestion.ModelObject(0)

            Select Case LCase(asInputBuffer)
                Case Is = "one"
                    Call lrFactType.CreateInternalUniquenessConstraint(New List(Of FBM.Role) From {lrFactType.RoleGroup(0)}, False, True, True, False, Nothing, True, False)
                    Call lrFactType.RoleGroup(0).SetMandatory(True, True)
                Case Is = "at most one"
                    Call lrFactType.CreateInternalUniquenessConstraint(New List(Of FBM.Role) From {lrFactType.RoleGroup(0)}, False, True, True, False, Nothing, True, False)
                Case Is = "many to many"
                    Call lrFactType.CreateInternalUniquenessConstraint(New List(Of FBM.Role) From {lrFactType.RoleGroup(0), lrFactType.RoleGroup(1)}, False, True, True, False, Nothing, True, False)
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ProcessStatementCreateSubtypeRelationship(ByRef arQuestion As tQuestion,
                                                          Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

        Try
            Dim lrEntityType1, lrEntityType2 As New FBM.EntityType

            lrEntityType1.Id = Trim(arQuestion.FocalSymbol(0))
            lrEntityType2.Id = Trim(arQuestion.FocalSymbol(1))

            lrEntityType1 = Me.Model.EntityType.Find(AddressOf lrEntityType1.Equals)
            lrEntityType2 = Me.Model.EntityType.Find(AddressOf lrEntityType2.Equals)

            If IsSomething(lrEntityType1) And IsSomething(lrEntityType2) Then
                '----------------------------------------
                'Create a Model level SubtypeConstraint
                '----------------------------------------
                Call lrEntityType1.CreateSubtypeRelationship(lrEntityType2,,,, abBroadcastInterfaceEvent)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub createPlanStepForModelElement(ByVal asModelObjectName As String,
                                             ByRef arPlan As Brain.Plan,
                                             Optional ByVal abEntityTypeOnly As Boolean = False)

        Dim lrQuestion As tQuestion
        Dim lrStep As Brain.Step 'For Steps added to the Plan.

        Try
            Dim lbProceedToCreateObjectTypeQuestion As Boolean = False

            If Me.Model.ExistsModelElement(asModelObjectName) Then
                If Array.IndexOf({pcenumConceptType.ValueType,
                                      pcenumConceptType.EntityType,
                                      pcenumConceptType.FactType},
                                  Me.Model.GetModelObjectByName(asModelObjectName).ConceptType) >= 0 Then
                    '-----------------------------------------------------------------------------
                    'A ObjectType already exists within the Model for the name lsModelObjectName
                    '-----------------------------------------------------------------------------
                Else
                    lbProceedToCreateObjectTypeQuestion = True
                End If
            Else
                lbProceedToCreateObjectTypeQuestion = True
            End If

            If lbProceedToCreateObjectTypeQuestion Then

                Dim lbIsLikelyValueType As Boolean = False
                Dim items As Array
                items = System.Enum.GetValues(GetType(pcenumReferenceModeEndings))
                Dim item As pcenumReferenceModeEndings
                For Each item In items
                    If asModelObjectName.EndsWith(GetEnumDescription(item)) Then
                        lbIsLikelyValueType = True
                        Exit For
                    ElseIf asModelObjectName.EndsWith(GetEnumDescription(item).Trim({"."c})) Then 'See https://msdn.microsoft.com/en-us/library/kxbw3kwc(v=vs.110).aspx
                        lbIsLikelyValueType = True
                        Exit For
                    End If
                Next

                If lbIsLikelyValueType And Not abEntityTypeOnly Then
                    lrStep = New Brain.Step(pcenumActionType.CreateValueType, True, pcenumActionType.None, Nothing)
                    arPlan.AddStep(lrStep)

                    Dim lasSymbol As New List(Of String)
                    lasSymbol.Add(asModelObjectName)

                    lrQuestion = New tQuestion("Would you like me to create an Value Type for '" & asModelObjectName & "'?",
                                                         pcenumQuestionType.CreateValueType,
                                                         pcenumExpectedResponseType.YesNo,
                                                         lasSymbol,
                                                         Nothing,
                                                         Nothing,
                                                         arPlan,
                                                         lrStep)
                Else
                    lrStep = New Brain.Step(pcenumActionType.CreateEntityType, True, pcenumActionType.CreateValueType, Nothing)
                    arPlan.AddStep(lrStep)

                    Dim lasSymbol As New List(Of String)
                    lasSymbol.Add(asModelObjectName)

                    lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & asModelObjectName & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                         pcenumQuestionType.CreateEntityType,
                                                         pcenumExpectedResponseType.YesNo,
                                                         lasSymbol,
                                                         Nothing,
                                                         Nothing,
                                                         arPlan,
                                                         lrStep)
                End If


                If Me.QuestionHasBeenRaised(lrQuestion) Then
                    '------------------------------------------------------------
                    'Great, already asked the question and am awaiting responce
                    '------------------------------------------------------------
                Else
                    Me.AddQuestion(lrQuestion)
                End If

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function ProcessVALUETYPEISWRITTENASStatement(Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                          Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing) As Boolean

        Dim lsMessage As String

        Try
            Me.Model = prApplication.WorkingModel

            Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.MODELELEMENTNAME = ""
            Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPE = New Object
            Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPELENGTH = New Object
            Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPEPRECISION = New Object
            Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.NUMBER = ""

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.VALUETYPEISWRITTENASStatement, Me.VAQLParsetree.Nodes(0))

            Dim lrValueTypeInstance As FBM.ValueTypeInstance
            Dim lsValueTypeName As String = ""
            Dim lsDataTypeName As String = ""
            Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet

            lsValueTypeName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.MODELELEMENTNAME))

            Dim liDataTypeLength As Integer = 0
            Dim liDataTypePrecision As Integer = 0

            If Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPE.GetType Is GetType(VAQL.ParseNode) Then
                lsDataTypeName = Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPE.Nodes(0).Token.Text
            ElseIf Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPELENGTH.GetType Is GetType(VAQL.ParseNode) Then
                lsDataTypeName = Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPELENGTH.Nodes(0).Token.Text
                liDataTypeLength = CInt(Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.NUMBER)
            ElseIf Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPEPRECISION.GetType Is GetType(VAQL.ParseNode) Then
                lsDataTypeName = Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.DATATYPEPRECISION.Nodes(0).Token.Text
                liDataTypePrecision = CInt(Me.VAQLProcessor.VALUETYPEISWRITTENASStatement.NUMBER)
            End If

            lsDataTypeName = DataTypeAttribute.Get(GetType(pcenumORMDataType), lsDataTypeName)
            If lsDataTypeName Is Nothing Then
                lsMessage = "That's not a valid Data Type."
                If arDSCError IsNot Nothing Then
                    arDSCError.Success = False
                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                    arDSCError.ErrorString = lsMessage
                End If
                Me.send_data(lsMessage)
                Return False
            End If

            Try
                liDataType = DirectCast([Enum].Parse(GetType(pcenumORMDataType), lsDataTypeName), pcenumORMDataType)
            Catch ex As Exception
                lsMessage = "That's not a valid Data Type."
                If arDSCError IsNot Nothing Then
                    arDSCError.Success = False
                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                    arDSCError.ErrorString = lsMessage
                End If
                Me.send_data(lsMessage)
                Return False
            End Try

            Dim lsActualModelElementName As String = ""

            If Me.Model.GetConceptTypeByNameFuzzy(lsValueTypeName, lsActualModelElementName) = pcenumConceptType.ValueType Then

                Dim lrValueType As FBM.ValueType
                Dim liInitialDataType As pcenumORMDataType

                lrValueType = Me.Model.GetModelObjectByName(lsActualModelElementName)
                liInitialDataType = lrValueType.DataType

                Dim lsInitialDataType = liInitialDataType.ToString
                If lrValueType.DataTypeLength > 0 Then
                    lsInitialDataType &= "(" & lrValueType.DataTypeLength.ToString
                    If lrValueType.DataTypePrecision > 0 Then
                        lsInitialDataType &= "," & lrValueType.DataTypePrecision.ToString
                    End If
                    lsInitialDataType &= ")"
                End If

                Call lrValueType.SetDataType(liDataType, liDataTypeLength, liDataTypePrecision, abBroadcastInterfaceEvent)

                Dim lsNewDataType = liDataType.ToString
                If liDataTypeLength > 0 Then
                    lsNewDataType &= "(" & liDataTypeLength.ToString
                    If liDataTypePrecision > 0 Then
                        lsNewDataType &= "," & liDataTypePrecision.ToString
                    End If
                    lsNewDataType &= ")"
                End If

                If Me.AutoLayoutOn Then
                    Me.Page.Form.AutoLayout()
                End If

                Me.send_data("Thank you. I changed the DataType of Value Type, '" & lsActualModelElementName & "' from " & lsInitialDataType & " to " & lsNewDataType & ".", False)
            Else

                Me.Timeout.Stop()

                Dim lrValueType As FBM.ValueType
                lrValueType = Me.Model.CreateValueType(lsValueTypeName, True, liDataType, liDataTypeLength, liDataTypePrecision, abBroadcastInterfaceEvent)

                If Me.Page IsNot Nothing Then

                    lrValueTypeInstance = Me.Page.DropValueTypeAtPoint(lrValueType, New PointF(100, 100), abBroadcastInterfaceEvent)
                    Call lrValueTypeInstance.RepellFromNeighbouringPageObjects(1, abBroadcastInterfaceEvent)
                    Call lrValueTypeInstance.Move(lrValueTypeInstance.X, lrValueTypeInstance.Y, abBroadcastInterfaceEvent)

                    If Me.AutoLayoutOn Then
                        Me.Page.Form.AutoLayout()
                    End If
                End If
                Me.Timeout.Start()

            End If

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

End Class
