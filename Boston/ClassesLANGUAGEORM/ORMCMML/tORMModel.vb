Imports System.ComponentModel
Imports System.Reflection

Namespace FBM

    Partial Public Class Model 'See also the folder ORMRDS/tORMModel.vb

        <NonSerialized()>
        Public Event StateTransitionAdded(ByRef lrFact As FBM.Fact)

        Public Sub AddCore(Optional aoBackgroundWorker As BackgroundWorker = Nothing)

            Try
#Region "Has No Core Model"
                '==================================================
                'RDS - Create a CMML Page and then dispose of it.
                Dim lrPage As FBM.Page '(lrModel)
                Dim lrCorePage As FBM.Page

                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page Model Elements for the EntityRelationshipDiagram into the metamodel

                'StateTransitionDiagrams
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page Model Elements for the StateTransitionDiagram into the metamodel

                'Derivations
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page Model Elements for the CoreDerivations into the metamodel

                'UseCaseDiagrams
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page Model Elements for the CoreUMLUseCaseDiagram into the metamodel

                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreProperty.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreProperty.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, False, True, False) 'Injects the lrCorePage's Model Elements into the Model. No need to do anything more with the lrCorePage at all.

                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreRelationship.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreRelationship.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, False, True, False) 'Injects the lrCorePage's Model Elements into the Model. No need to do anything more with the lrCorePage at all.
                '==================================================

                'CodeSafe: Set the CoreModel VersionNr of the Model.
                Me.CoreVersionNumber = prApplication.CMML.Core.CoreVersionNumber
#End Region

                Call Me.createEntityRelationshipArtifacts()
                Call Me.PopulateAllCoreStructuresFromCoreMDAElements(aoBackgroundWorker)
            Catch ex As Exception

            End Try
        End Sub


        Public Sub addCMMLColumnToRelationOrigin(ByRef arRelation As RDS.Relation,
                                                 ByRef arColumn As RDS.Column,
                                                 ByVal aiOrdinalPosition As Integer)

            Dim lsSQLQuery As String
            Dim lrFact As FBM.Fact

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin.ToString
            lsSQLQuery &= " (Attribute, Relation)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & arColumn.Id & "'"
            lsSQLQuery &= ",'" & arRelation.Id & "'"
            lsSQLQuery &= " )"

            lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
            lsSQLQuery &= " (RelationAttribute, OrdinalPosition)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & lrFact.Id & "'"
            lsSQLQuery &= ",'" & aiOrdinalPosition & "'"
            lsSQLQuery &= " )"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


        End Sub

        Public Sub addCMMLColumnToIndex(ByRef arIndex As RDS.Index,
                                        ByRef arColumn As RDS.Column)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString & " (Index, Property) VALUES ("
                lsSQLQuery &= "'" & arIndex.Name & "','" & arColumn.Id & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLColumnToRelationDestination(ByRef arRelation As RDS.Relation,
                                                      ByRef arColumn As RDS.Column,
                                                      ByVal aiOrdinalPosition As Integer)

            Dim lsSQLQuery As String
            Dim lrFact As FBM.Fact

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination.ToString
            lsSQLQuery &= " (Attribute, Relation)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & arColumn.Id & "'"
            lsSQLQuery &= ",'" & arRelation.Id & "'"
            lsSQLQuery &= " )"

            lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestinationHasOrdinalPosition.ToString
            lsSQLQuery &= " (RelationAttribute, OrdinalPosition)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & lrFact.Id & "'"
            lsSQLQuery &= ",'" & aiOrdinalPosition & "'"
            lsSQLQuery &= " )"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


        End Sub

        Public Sub addCMMLIsPGSRelation(ByRef arTable As RDS.Table)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                lsSQLQuery &= " (IsPGSRelation)"
                lsSQLQuery &= " VALUES ('" & arTable.Name & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLIsDerivedFactTypeParameter(ByRef arColumn As RDS.Column)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsDerivedFactTypeParameter.ToString
                lsSQLQuery &= " (IsDerivedFactTypeParameter)"
                lsSQLQuery &= " VALUES ('" & arColumn.Id & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function addCMMLRelationIsForFactType(ByRef arRDSRelation As RDS.Relation, ByRef arFactType As FBM.FactType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
                lsSQLQuery &= "(Relation, FactType)"
                lsSQLQuery &= $" VALUES ('{arRDSRelation.Id}','{arFactType.Id}')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Function

        ''' <summary>
        ''' For the STM (State Transition Model), for Value Type/Value Constraints within the Model.
        ''' </summary>
        ''' <param name="arStartState">The State for a ValueType that is a start of a STM.</param>
        Public Function addCMMLStartState(ByRef arStartState As FBM.STM.State) As FBM.Fact

            Try

                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString
                lsSQLQuery &= " (ValueType, CoreElement, Event)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arStartState.ValueType.Id & "'"
                lsSQLQuery &= ",'" & arStartState.Id & "'"
                lsSQLQuery &= ",''"
                lsSQLQuery &= " )"

                Return Me.ORMQL.ProcessORMQLStatement(lsSQLQuery) 'As Fact

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' For the STM (State Transition Model), for Value Type/Value Constraints within the Model.
        ''' </summary>
        ''' <param name="arStateTransition"></param>
        Public Sub addCMMLStateTransition(ByRef arStateTransition As FBM.STM.StateTransition)

            Try

                Dim lsSQLQuery As String
                Dim lrFact As FBM.Fact

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreStateTransition.ToString
                lsSQLQuery &= " (ValueType, Concept1, Concept2, Event)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arStateTransition.ValueType.Id & "'"
                lsSQLQuery &= ",'" & arStateTransition.FromState.Id & "'"
                lsSQLQuery &= ",'" & arStateTransition.ToState.Id & "'"
                lsSQLQuery &= ",'" & arStateTransition.Event & "'"
                lsSQLQuery &= " )"

                lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                arStateTransition.Id = lrFact.Id

                arStateTransition.Fact = lrFact

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        ''' <summary>
        ''' For the STM (State Transition Model), for Value Type/Value Constraints within the Model.
        ''' </summary>
        ''' <param name="arStopState">The State for a ValueType that is a Stop of a STM.</param>
        Public Sub addCMMLStopState(ByRef arStopState As FBM.STM.State)

            Try
                Dim lsSQLQuery As String
                Dim lrFact As FBM.Fact

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString
                lsSQLQuery &= " (ValueType, CoreElement)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arStopState.ValueType.Id & "'"
                lsSQLQuery &= ",'" & arStopState.Id & "'"
                lsSQLQuery &= " )"

                lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Sub addCMMLProcessActivityTaskType(ByRef arCMMLProcess As CMML.Process, aiBPMNActivityTaskType As pcenumBPMNActivityTaskType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNActivityTaskType.ToString
                lsSQLQuery &= " (Process, ActivityTaskType)"
                lsSQLQuery &= " VALUES (" & arCMMLProcess.Id & "," & aiBPMNActivityTaskType.ToString & ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLProcessProcessType(ByRef arCMMLProcess As CMML.Process, aiBPMNProcessType As pcenumBPMNProcessType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNProcessType.ToString
                lsSQLQuery &= " (Process, ProcessType)"
                lsSQLQuery &= " VALUES ('" & arCMMLProcess.Id & "','" & aiBPMNProcessType.ToString & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Sub addCMMLProcessActivityType(ByRef arCMMLProcess As CMML.Process, aiBPMNActivityType As pcenumBPMNActivityType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNActivityType.ToString
                lsSQLQuery &= " (Process, ActivityType)"
                lsSQLQuery &= " VALUES ('" & arCMMLProcess.Id & "','" & aiBPMNActivityType.ToString & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLProcessActivityMarker(ByRef arCMMLProcess As CMML.Process, aiBPMNActivityMarker As pcenumBPMNActivityMarker)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessHasCoreBPMNActivityMarker.ToString
                lsSQLQuery &= " (Process, ActivityMarker)"
                lsSQLQuery &= " VALUES ('" & arCMMLProcess.Id & "','" & aiBPMNActivityMarker.ToString & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLProcessEventType(ByRef arCMMLProcess As CMML.Process, aiBPMNEventType As pcenumBPMNEventType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventType.ToString
                lsSQLQuery &= " (Process, EventType)"
                lsSQLQuery &= " VALUES ('" & arCMMLProcess.Id & "','" & aiBPMNEventType.ToString & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLProcessConversationType(ByRef arCMMLProcess As CMML.Process, aiBPMNConversationType As pcenumBPMNConversationType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNConversationType.ToString
                lsSQLQuery &= " (Process, ConversationType)"
                lsSQLQuery &= " VALUES ('" & arCMMLProcess.Id & "','" & aiBPMNConversationType.ToString & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLProcessEventPosition(ByRef arCMMLProcess As CMML.Process, aiBPMNEventPosition As pcenumBPMNEventPosition)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventPosition.ToString
                lsSQLQuery &= " (Process, EventPosition)"
                lsSQLQuery &= " VALUES ('" & arCMMLProcess.Id & "','" & aiBPMNEventPosition.ToString & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLProcessEventSubType(ByRef arCMMLProcess As CMML.Process, aiBPMNEventSubType As pcenumBPMNEventSubType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventSubType.ToString
                lsSQLQuery &= " (Process, EventSubType)"
                lsSQLQuery &= " VALUES ('" & arCMMLProcess.Id & "','" & aiBPMNEventSubType.ToString & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addCMMLProcessGatewayType(ByRef arCMMLProcess As CMML.Process, aiBPMNGatewayType As pcenumBPMNGatewayType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNGatewayType.ToString
                lsSQLQuery &= " (Process, GatewayType)"
                lsSQLQuery &= " VALUES ('" & arCMMLProcess.Id & "','" & aiBPMNGatewayType.ToString & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Changes the name of an Attribute in the RDS
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Sub updateCMMLAttributeName(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
            lsSQLQuery &= " SET PropertyName = '" & arColumn.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        ''' <summary>
        ''' Changes the name of an Attribute in the RDS
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Sub updateCMMLPropertyDBName(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyHasDBName.ToString
            lsSQLQuery &= " SET DBName = '" & arColumn.DBName & "'"
            lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub


        Public Sub updateCMMLAttributeEntityForColumn(ByRef arColumn As RDS.Column, ByRef arTable As RDS.Table)

            Try

                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreERDAttribute.ToString
                lsSQLQuery &= " SET ModelObject = '" & arTable.Name & "'"
                lsSQLQuery &= " WHERE Attribute = '" & arColumn.Id & "'"
                lsSQLQuery &= "   AND ModelObject = '" & arColumn.Table.Name & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub updateCMMLStateName(ByRef arState As STM.State, ByVal asOldStateName As String)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreStateHasName.ToString
                lsSQLQuery &= " SET StateName = '" & arState.Name & "'"
                lsSQLQuery &= " WHERE State = '" & arState.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function CMMLActorHasActorProcessParticipationRelations(ByRef arCMMLActor As CMML.Actor) As Boolean

            Try
                Dim lsSQLQuery As String
                Dim lrRecordset As ORMQL.Recordset

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
                lsSQLQuery &= " WHERE Actor = '" & arCMMLActor.Name & "'"

                lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                Return Not lrRecordset.EOF

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        ''' <summary>
        ''' Connects to the database if it is not already connected
        ''' </summary>
        Public Function connectToDatabase(Optional abForceConnection As Boolean = False,
                                          Optional abThrowErrors As Boolean = True) As Boolean

            Try
                'CodeSafe
                If Me.TargetDatabaseType = pcenumDatabaseType.None Then
                    Return False
                End If

                If abForceConnection Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(Me.TargetDatabaseType, Me.TargetDatabaseConnectionString)
                    If Me.DatabaseConnection Is Nothing Then
                        Throw New Exception("No database connection has been established. Please check the database connection settings for the Model in the Model Configuration Form.")
                    End If
                ElseIf Me.DatabaseConnection Is Nothing Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(Me.TargetDatabaseType, Me.TargetDatabaseConnectionString, False)
                    If Me.DatabaseConnection Is Nothing Then
                        Throw New Exception("No database connection has been established. Please check the database connection settings for the Model in the Model Configuration Form.")
                    End If
                ElseIf Me.DatabaseManager.Connection Is Nothing Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(Me.TargetDatabaseType, Me.TargetDatabaseConnectionString)
                    If Me.DatabaseConnection Is Nothing Then
                        Throw New Exception("No database connection has been established. Please check the database connection settings for the Model in the Model Configuration Form.")
                    End If
                ElseIf Me.DatabaseConnection.Connected = False Then
                    Throw New Exception("The database is not connected.")
                End If

                If Me.IsDatabaseSynchronised And Me.RDS.DatabaseDataType.Count = 0 Then
                    Dim lsPath = Boston.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
                    Dim reader As System.IO.TextReader = New System.IO.StreamReader(lsPath)

                    Dim csvReader = New CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture)
                    Me.RDS.DatabaseDataType = csvReader.GetRecords(Of DatabaseDataType).ToList
                End If

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message

                If abThrowErrors Then
                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Information, ex.StackTrace, False,, True)
                End If

                Return False

            End Try
        End Function

        Public Sub createCMMLAttribute(ByVal asEntityName As String,
                                       ByVal asAttributeName As String,
                                       ByRef arRole As FBM.Role,
                                       ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String = ""
            Dim lrFact As New FBM.Fact
            Dim lrTable As RDS.Table
            Dim lrModelElement As FBM.ModelObject

            Try
                '---------------------------------------------------------------------
                'Check to see that the Entity (already) exists in the ERD MetaModel.
                '  If the Entity doesn't exist, create it.
                '---------------------------------------------------------------------
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & asEntityName & "'"

                Dim lrORMRecordset As ORMQL.Recordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrORMRecordset.EOF Then
                    '---------------------------------------------------------------
                    'The Entity does not exist in the ERD MetaModel, so create it.
                    '---------------------------------------------------------------
                    Boston.WriteToStatusBar("Creating Entity, '" & asEntityName & "'")

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " (Element, ElementType)"
                    lsSQLQuery &= " VALUES ('" & asEntityName & "', 'Entity')"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '---------------------------------------------------------------------------------------
                    'Get the underlying ModelElement
                    lrModelElement = Me.GetModelObjectByName(asEntityName)

                    lrTable = New RDS.Table(Me.RDS, asEntityName, lrModelElement)
                    Me.RDS.Table.AddUnique(lrTable)
                End If

                '======================
                'Create the Attribute
                '======================
                '----------------------------------------------------------------------------------------------------
                'First create the Property for the Attribute.
                '  Properties themselves are GUIDs with a PropertyName.
                '  The reason why Properties are unique and non-SelfDescriptive (i.e. The name of the Property is not
                '  the Id of the Property) is because multiple Entities may have Attributes with the same name.
                '  So each Entity links to its Properties/Attributes by their Id (i.e. EntityType Instance) and by
                '  default have a Property/Attribute Name.
                '----------------------------------------------------------------------------------------------------
                Dim lsPropertyInstanceId As String
                If arColumn Is Nothing Then
                    lsPropertyInstanceId = System.Guid.NewGuid.ToString
                Else
                    lsPropertyInstanceId = arColumn.Id
                End If

                '---------------------------------------------
                'Create the Attribute against the Entity
                '---------------------------------------------
                lsSQLQuery = "INSERT INTO CoreERDAttribute (ModelObject, Attribute)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & asEntityName & "'"
                lsSQLQuery &= " ,'" & lsPropertyInstanceId & "'"
                lsSQLQuery &= " )"

                lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                Dim lrRecordsetCount = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


                '====================================================================================================
                Dim lbForceStorePropertyInformation As Boolean = False

                If arColumn.Role Is Nothing Then
                    'Independent ValueType single Column Tables have no Role on the Column.
                    lbForceStorePropertyInformation = True
                ElseIf arColumn.Role.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                    'If arColumn.Role.JoinsEntityType.IsObjectifyingEntityType Then '20240124-VM-Was, can't understand why was not for ordinary EntityType (??). Rmove if not missed after 6 months.
                    lbForceStorePropertyInformation = True
                    'End If
                End If
                If asEntityName = arColumn.Table.Name And asEntityName = arColumn.Role.JoinedORMObject.GetTopmostNonAbsorbedSupertype(True).Id Then
                    'Column may be for Supertype of absorbed Subtype.
                    lbForceStorePropertyInformation = True
                End If

                If (lbForceStorePropertyInformation Or (asEntityName = arColumn.JoinedORMObjectId Or asEntityName = arColumn.Role.FactType.Id)) And lrRecordsetCount("Count").Data = 0 Then
                    'Columns can be reused on Subtype Entities, and don't need their definition twice,
                    '  just their relationship with the ERD Entity (above).

                    lsSQLQuery = "INSERT INTO CorePropertyHasPropertyName (Property, PropertyName)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                    lsSQLQuery &= " ,'" & asAttributeName & "'"
                    lsSQLQuery &= " )"

                    lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "INSERT INTO CorePropertyHasDBName (Property, DBName)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                    lsSQLQuery &= " ,'" & asAttributeName & "'"
                    lsSQLQuery &= " )"

                    lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


                    lsSQLQuery = "INSERT INTO CorePropertyIsForFactType (Property, FactType)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                    lsSQLQuery &= " ,'" & arRole.FactType.Id & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "INSERT INTO CorePropertyIsForRole (Property, Role)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                    lsSQLQuery &= " ,'" & arRole.Id & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If arColumn.ActiveRole IsNot Nothing Then
                        'Independent ValueType single Column Tables have no Role/ActiveRole on the Column.                    
                        lsSQLQuery = "INSERT INTO CorePropertyHasActiveRole (Property, Role)"
                        lsSQLQuery &= " VALUES ("
                        lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                        lsSQLQuery &= " ,'" & arColumn.ActiveRole.Id & "'"
                        lsSQLQuery &= " )"
                    End If

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '--------------------------------------------------
                    'Set the Ordinal Position of the Attribute
                    '--------------------------------------------------
                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                    lsSQLQuery &= " WHERE ModelObject = '" & asEntityName & "'"

                    Dim lrCountRecordset As New ORMQL.Recordset

                    lrCountRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                    lsSQLQuery &= " (Property, Position)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lsPropertyInstanceId & "'"
                    lsSQLQuery &= ",'" & lrCountRecordset("Count").Data & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '--------------------------------------------
                    'Check to see if the Attribute is Mandatory
                    '--------------------------------------------
                    If arColumn.Nullable = False Then

                        Boston.WriteToStatusBar("Creating MandatoryConstraint for Attribute, '" & asAttributeName & "', for Entity, '" & asEntityName & "'", True)

                        lsSQLQuery = "INSERT INTO CoreIsMandatory (IsMandatory)"
                        lsSQLQuery &= " VALUES ("
                        lsSQLQuery &= " '" & lsPropertyInstanceId & "'" 'lsAttributeName & "'"
                        lsSQLQuery &= " )"

                        lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub createCMMLAttributeIsMandatory(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO CoreIsMandatory (IsMandatory)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arColumn.Id & "'"
            lsSQLQuery &= " )"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLEntityByRDSTable(ByRef arTable As RDS.Table)

            Dim lsSQLQuery As String

            For Each lrColumn In arTable.Column

                lsSQLQuery = "REMOVE INSTANCE '" & lrColumn.Id & "' FROM CoreElement"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            Next

            lsSQLQuery = "REMOVE INSTANCE '" & arTable.Name & "' FROM CoreElement"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE Element = '" & arTable.Name & "'"
            lsSQLQuery &= " AND ElementType = 'Entity'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub


        Public Sub createCMMLIndex(ByVal asIndexName As String,
                                   ByVal asEntityName As String,
                                   ByVal asQualifier As String,
                                   ByVal aiIndexDirection As pcenumCMMLIndexDirection,
                                   ByVal abIsPrimaryKey As Boolean,
                                   ByVal abRestrainsToUniqueValues As Boolean,
                                   ByVal abIndexIgnoresNulls As Boolean,
                                   ByVal aarColumn As List(Of RDS.Column))

            Try
                Dim lsSQLQuery As String = ""

                '-------------------------------------------------
                'Must create a Primary Identifier for the Entity
                '-------------------------------------------------
                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsForEntity.ToString
                lsSQLQuery &= " (Entity, Index)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & asEntityName & "'"
                lsSQLQuery &= ",'" & asIndexName & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                For Each lrColumn In aarColumn
                    '-------------------------------------
                    'Add the Attribute to the PrimaryKey
                    '-------------------------------------
                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
                    lsSQLQuery &= " (Index, Property)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & asIndexName & "'"
                    lsSQLQuery &= ",'" & lrColumn.Id & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                Next

                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexRestrainsToUniqueValues.ToString
                lsSQLQuery &= " (Index)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & asIndexName & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If abIsPrimaryKey Then
                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                    lsSQLQuery &= " (Index)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & asIndexName & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If abIndexIgnoresNulls Then
                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexIgnoresNulls.ToString
                    lsSQLQuery &= " (Index)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & asIndexName & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexHasDirection.ToString
                lsSQLQuery &= " (Index, IndexDirection)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & asIndexName & "'"
                lsSQLQuery &= ",'" & aiIndexDirection.ToString & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexHasQualifier.ToString
                lsSQLQuery &= " (Index, Qualifier)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & asIndexName & "'"
                lsSQLQuery &= ",'" & asQualifier & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub createCMMLRelation(ByRef arRelation As RDS.Relation)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO CoreRelationIsForEntity (Entity, Relation)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.OriginTable.Name & "'"
                lsSQLQuery &= " ,'" & arRelation.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO CoreRelationHasDestinationEntity (Entity, Relation)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.DestinationTable.Name & "'"
                lsSQLQuery &= " ,'" & arRelation.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO CoreOriginMultiplicity (ERDRelation, Multiplicity)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.Id & "'"
                lsSQLQuery &= " ,'" & arRelation.OriginMultiplicity.ToString & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO CoreDestinationMultiplicity (ERDRelation, Multiplicity)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.Id & "'"
                lsSQLQuery &= " ,'" & arRelation.DestinationMultiplicity.ToString & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString & " (Relation, FactType)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.Id & "'"
                lsSQLQuery &= " , '" & arRelation.ResponsibleFactType.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If arRelation.RelationOriginIsMandatory Then
                    lsSQLQuery = "INSERT INTO CoreOriginIsMandatory (OriginIsMandatory)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & arRelation.Id & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If arRelation.RelationDestinationIsMandatory Then
                    lsSQLQuery = "INSERT INTO CoreDestinationIsMandatory (DestinationIsMandatory)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & arRelation.Id & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                lsSQLQuery &= " (Relation, Predicate)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arRelation.Id & "'"
                lsSQLQuery &= ",'" & arRelation.OriginPredicate & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                lsSQLQuery &= " (Relation, Predicate)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arRelation.Id & "'"
                lsSQLQuery &= ",'" & arRelation.DestinationPredicate & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim liInd As Integer = 1
                Dim lrFact As FBM.Fact

                For Each lrOriginColumn In arRelation.OriginColumns

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin.ToString
                    lsSQLQuery &= " (Attribute, Relation)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrOriginColumn.Id & "'"
                    lsSQLQuery &= ",'" & arRelation.Id & "'"
                    lsSQLQuery &= " )"

                    lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
                    lsSQLQuery &= " (RelationAttribute, OrdinalPosition)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrFact.Id & "'"
                    lsSQLQuery &= ",'" & liInd & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    liInd += 1
                Next

                liInd = 1
                For Each lrDestinationColumn In arRelation.DestinationColumns

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination.ToString
                    lsSQLQuery &= " (Attribute, Relation)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrDestinationColumn.Id & "'"
                    lsSQLQuery &= ",'" & arRelation.Id & "'"
                    lsSQLQuery &= " )"

                    lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestinationHasOrdinalPosition.ToString
                    lsSQLQuery &= " (RelationAttribute, OrdinalPosition)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrFact.Id & "'"
                    lsSQLQuery &= ",'" & liInd & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    liInd += 1
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub setCMMLProcessToProcessRelationIsExtends(ByRef arProcessToProcessRelation As CMML.ProcessProcessRelation, ByVal abIsExtends As Boolean)

            Dim lsSQLQuery As String

            Try

                If abIsExtends Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreP2PIsExtends.ToString
                    lsSQLQuery &= " (CoreP2PIsExtends)"
                    lsSQLQuery &= " VALUES ('" & arProcessToProcessRelation.Fact.Id & "')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreP2PIsExtends.ToString
                    lsSQLQuery &= " WHERE CoreP2PIsExtends = '" & arProcessToProcessRelation.Fact.Id & "'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub setCMMLProcessToProcessRelationIsIncludes(ByRef arProcessToProcessRelation As CMML.ProcessProcessRelation, ByVal abIsIncludes As Boolean)

            Dim lsSQLQuery As String

            Try

                If abIsIncludes Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreP2PIsIncludes.ToString
                    lsSQLQuery &= " (CoreP2PIsIncludes)"
                    lsSQLQuery &= " VALUES ('" & arProcessToProcessRelation.Fact.Id & "')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreP2PIsIncludes.ToString
                    lsSQLQuery &= " WHERE CoreP2PIsIncludes = '" & arProcessToProcessRelation.Fact.Id & "'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Sub setCoreElementIsActor(ByRef arModelElement As FBM.ModelObject, ByVal abIsActor As Boolean)

            Dim lsSQLQuery As String

            Try

                If abIsActor Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " (Element, ElementType)"
                    lsSQLQuery &= " VALUES ('" & arModelElement.Id & "', 'Actor')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " WHERE Element = '" & arModelElement.Id & "'"
                    lsSQLQuery &= " AND ElementType = 'Actor'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub setCMMLCoreRelationEnforcesOnCascadeUpdate(ByRef arRDSRelation As RDS.Relation, ByVal abEnforcesOnCascadeUpdate As Boolean)

            Dim lsSQLQuery As String

            Try

                If abEnforcesOnCascadeUpdate Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreRelationEnforcesOnCascadeUpdate.ToString
                    lsSQLQuery &= " (Relation)"
                    lsSQLQuery &= " VALUES ('" & arRDSRelation.Id & "')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreRelationEnforcesOnCascadeUpdate.ToString
                    lsSQLQuery &= " WHERE Relation = '" & arRDSRelation.Id & "'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub setCMMLCoreRelationEnforcesOnCascadeDelete(ByRef arRDSRelation As RDS.Relation, ByVal abEnforcesOnCascadeDelete As Boolean)

            Dim lsSQLQuery As String

            Try

                If abEnforcesOnCascadeDelete Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreRelationEnforcesOnCascadeDelete.ToString
                    lsSQLQuery &= " (Relation)"
                    lsSQLQuery &= " VALUES ('" & arRDSRelation.Id & "')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreRelationEnforcesOnCascadeDelete.ToString
                    lsSQLQuery &= " WHERE Relation = '" & arRDSRelation.Id & "'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub setCMMLCoreRelationEnforcesReferentialIntegrity(ByRef arRDSRelation As RDS.Relation, ByVal abEnforcesReferentialIntegrity As Boolean)

            Dim lsSQLQuery As String

            Try

                If abEnforcesReferentialIntegrity Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreRelationEnforcesReferentialIntegrity.ToString
                    lsSQLQuery &= " (Relation)"
                    lsSQLQuery &= " VALUES ('" & arRDSRelation.Id & "')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreRelationEnforcesReferentialIntegrity.ToString
                    lsSQLQuery &= " WHERE Relation = '" & arRDSRelation.Id & "'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub createCMMLActor(ByRef arActor As CMML.Actor)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " (Element, ElementType)"
            lsSQLQuery &= " VALUES ('" & arActor.Name & "', 'Actor')"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementName.ToString
            lsSQLQuery &= " (Element, ElementName)"
            lsSQLQuery &= " VALUES ('" & arActor.Name & "','" & arActor.Name & "')"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Function createCMMLActorProcessRelation(ByRef arCMMLActorProcessRelation As CMML.ActorProcessRelation) As FBM.Fact

            Try
                '----------------------------------
                'Create the Fact within the Model
                '----------------------------------
                Dim lsSQLString As String = ""
                lsSQLString = "INSERT INTO " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
                lsSQLString &= " (Actor, Process, Data)"
                lsSQLString &= " VALUES ("
                lsSQLString &= "'" & arCMMLActorProcessRelation.Actor.Name & "'"
                lsSQLString &= ",'" & arCMMLActorProcessRelation.Process.Id & "'"
                lsSQLString &= ",''"
                lsSQLString &= ")"

                '----------------------------------
                'Create the Fact within the Model
                '----------------------------------
                Return Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing
            End Try

        End Function


        Public Function createCMMLProcessProcessRelation(ByRef arCMMLProcessProcessRelation As CMML.ProcessProcessRelation) As FBM.Fact

            Try
                '----------------------------------
                'Create the Fact within the Model
                '----------------------------------
                Dim lsSQLString As String = ""
                lsSQLString = "INSERT INTO " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
                lsSQLString &= " (Process1, Process2, Data)"
                lsSQLString &= " VALUES ("
                lsSQLString &= "'" & arCMMLProcessProcessRelation.Process1.Id & "'"
                lsSQLString &= ",'" & arCMMLProcessProcessRelation.Process2.Id & "'"
                lsSQLString &= ",''"
                lsSQLString &= ")"

                '----------------------------------
                'Create the Fact within the Model
                '----------------------------------
                Return Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing
            End Try

        End Function


        Public Sub createCMMLTable(ByRef arTable As RDS.Table)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " (Element, ElementType)"
            lsSQLQuery &= " VALUES ('" & arTable.Name & "', 'Entity')"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If arTable.isPGSRelation Then
                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                lsSQLQuery &= " (IsPGSRelation)"
                lsSQLQuery &= " VALUES ('" & arTable.Name & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If

        End Sub

        Public Sub createColumnForUnaryFactType(ByRef arFactType As FBM.FactType)

            Try
                'CodeSafe
                If arFactType.IsMDAModelElement Then Exit Sub 'We don't create Tables/Columns for the Core model.

                'RDS
                If arFactType.Arity = 1 Then
                    'Unary FactType, so add boolean Column to the corresponding RDS Table.

                    Dim lrFactType = arFactType

                    'CodeSafe
                    If lrFactType.RoleGroup.Count = 0 Then Exit Sub
                    If lrFactType.RoleGroup(0).JoinedORMObject Is Nothing Then Throw New ApplicationException("The Unary Role on Fact Type, " & lrFactType.Id & ", is not joined to anything. Consider removing the Role.")

                    Dim lrTable As RDS.Table = Me.RDS.Table.Find(Function(x) x.Name = lrFactType.RoleGroup(0).JoinedORMObject.Id)

                    If lrTable Is Nothing Then
                        lrTable = Me.RDS.Table.Find(Function(x) x.Name = lrFactType.RoleGroup(0).JoinedORMObject.GetTopmostNonAbsorbedSupertype.Id)
                    End If

                    Dim lsColumnName = "DummyFactTypeReadingRequired"

                    If lrFactType.FactTypeReading.Count > 0 Then
                        lsColumnName = FEStrings.MakeCapCamelCase(lrFactType.FactTypeReading(0).PredicatePart(0).PredicatePartText, True)
                    End If

                    lsColumnName = lrTable.createUniqueColumnName(lsColumnName, Nothing, 0)

                    Dim lrColumn As New RDS.Column(lrTable, lsColumnName, arFactType.RoleGroup(0), arFactType.RoleGroup(0), False)
                    lrColumn.FactType = arFactType

                    Call lrTable.addColumn(lrColumn)

                Else
                    Throw New Exception("FactType is not Unary")
                End If

            Catch appEx As ApplicationException
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = appEx.Message
                lsMessage.AppendDoubleLineBreak("Error: " & mb.ReflectedType.Name & "." & mb.Name)

                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, Nothing, False, False, True)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub



        Public Function CreateUniqueEntityName(ByVal asEntityName As String, Optional ByVal aiStartingInd As Integer = 0) As String

            Dim lsUniqueEntityName As String = ""
            Dim lsSQLQuery As String
            Dim lrRecordset As ORMQL.Recordset

            If aiStartingInd = 0 Then
                lsUniqueEntityName = asEntityName
            Else
                lsUniqueEntityName = asEntityName & aiStartingInd.ToString
            End If


            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE " & pcenumCMML.Element.ToString & " = '" & lsUniqueEntityName & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If CInt(lrRecordset("Count").Data) > 0 Then

                lsUniqueEntityName = Me.CreateUniqueEntityName(asEntityName, aiStartingInd + 1)

            End If

            Return lsUniqueEntityName

        End Function

        Public Function CreateUniquePropertyName(ByRef arAttribute As ERD.Attribute, ByRef aiStartingInd As Integer) As String

            Dim lsUniquePropertyName As String = ""

            Dim lsSQLQuery As String
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset

            If aiStartingInd = 0 Then
                lsUniquePropertyName = arAttribute.AttributeName
            Else
                lsUniquePropertyName = arAttribute.AttributeName & aiStartingInd.ToString
            End If

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
            lsSQLQuery &= " WHERE ModelObject = '" & arAttribute.Entity.Name & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If Not lrRecordset.EOF Then
                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'"
                lsSQLQuery &= "   AND PropertyName = '" & lsUniquePropertyName & "'"

                lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset1("Count").Data) > 0 Then
                    lsUniquePropertyName = Me.CreateUniquePropertyName(arAttribute, aiStartingInd + 1)
                End If
            End If

            Return lsUniquePropertyName

        End Function

        Public Function ModelElementIsGeneralConceptOnly(ByVal asModelElementName As String) As Boolean

            Try
                Dim lrModelDictionaryEntry As FBM.DictionaryEntry = Me.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(asModelElementName))

                If lrModelDictionaryEntry Is Nothing Then
                    Return False
                Else
                    Return lrModelDictionaryEntry.isGeneralConceptOnly
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                Return False

            End Try

        End Function

        ''' <summary>
        ''' Sets the ResponsibleFactType of existing Relations for the FactType to the relations respective LinkFactType.
        ''' This method called when a suitable FactType is objectified.
        ''' NB See also the converse moveRelationsOfLinkFactTypesToRespectiveFactType
        ''' </summary>
        ''' <param name="arFactType"></param>
        ''' <remarks></remarks>
        Public Sub moveRelationsOfFactTypeToRespectiveLinkFactTypes(ByVal arFactType As FBM.FactType)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset
            Dim lasRelationId As New List(Of String)

            Try

                lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                lsSQLQuery &= " WHERE FactType = '" & arFactType.Id & "'"

                lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrRecordset.EOF

                    lasRelationId.Add(lrRecordset("Relation").Data)

                    lrRecordset.MoveNext()
                End While

                Dim larLinkFactType = From FactType In Me.FactType
                                      Where FactType.IsLinkFactType = True _
                                      And arFactType.RoleGroup.Contains(FactType.LinkFactTypeRole)
                                      Select FactType

                For Each lrLinkFactType In larLinkFactType

                    'For Each lsRelationId In lasRelationId.ToArray ''20211005-VM-Removed because step through them below.
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                    lsSQLQuery &= " WHERE Entity = '" & lrLinkFactType.LinkFactTypeRole.JoinedORMObject.GetTopmostNonAbsorbedSupertype.Id & "'"
                    'lsSQLQuery &= " AND Relation = '" & lsRelationId & "'" '20211005-VM-Removed because couldn't possibly know this.

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    While Not lrRecordset1.EOF
                        If lasRelationId.Contains(lrRecordset1("Relation").Data) Then
                            lasRelationId.Remove(lrRecordset1("Relation").Data)
                            Exit While
                        End If
                        lrRecordset1.MoveNext()
                    End While

                    If Not lrRecordset1.EOF Then
                        lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        lsSQLQuery &= " SET FactType = '" & lrLinkFactType.Id & "'"
                        lsSQLQuery &= " WHERE Relation = '" & lrRecordset1("Relation").Data & "'"

                        '============================================================================
                        'RDS
                        Dim lrRDSRelation As RDS.Relation = Me.RDS.Relation.Find(Function(x) x.Id = lrRecordset1("Relation").Data) 'lsRelationId)
                        Try
                            Call lrRDSRelation.changeResponsibleFactType(lrLinkFactType)
                        Catch ex As Exception
                            '20200726-VM-Unusual occurence, consider throwing an Exception.
                            Throw New Exception("Error trying to change the Responsible Fact Type for RDS Relation.")
                        End Try

                        Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    'Next
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub moveRelationsOfLinkFactTypesToRespectiveFactType(ByVal arFactType As FBM.FactType)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try

                Dim larFactType = From FactType In Me.FactType
                                  Where FactType.IsLinkFactType = True _
                                  And arFactType.RoleGroup.Contains(FactType.LinkFactTypeRole)
                                  Select FactType

                For Each lrFactType In larFactType

                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                    lsSQLQuery &= " WHERE FactType = '" & lrFactType.Id & "'"

                    lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    While Not lrRecordset.EOF
                        lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        lsSQLQuery &= " SET FactType = '" & arFactType.Id & "'"
                        lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                        '============================================================================
                        'RDS
                        Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        Dim lrRDSRelation As RDS.Relation = Me.RDS.Relation.Find(Function(x) x.Id = lrRecordset("Relation").Data)
                        Try
                            Call lrRDSRelation.changeResponsibleFactType(arFactType)
                        Catch ex As Exception
                            '20200726-VM-Unusual occurence, consider throwing an Exception.
                            Throw New Exception("Error trying to change Responsible Fact Type for RDSRelation.")
                        End Try

                        lrRecordset.MoveNext()
                    End While

                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        ''' <summary>
        ''' Called when a FactType is removed from a Model and that Model contains a Language within the Core that references that FactType.
        ''' </summary>
        ''' <param name="arFactType"></param>
        ''' <remarks></remarks>
        Public Sub RemoveFactTypeReferencesFromCore(ByRef arFactType As FBM.FactType)

            Dim lsSQLQuery As String
            'Dim lrColumn As RDS.Column
            'Dim lrTable As RDS.Table

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
            lsSQLQuery &= " WHERE FactType = '" & arFactType.Id & "'"
            'lsSQLQuery &= " AND Attribute = '" & lrAttribute.Name & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
            lsSQLQuery &= " WHERE FactType = '" & arFactType.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLAttribute(ByVal asAttributeId As String)

            Dim lsSQLQuery As String

            lsSQLQuery = "REMOVE INSTANCE '" & asAttributeId & "' FROM CoreElement"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '----------------------------------------------------------------------------------
            'Reset the OrdinalPosition of the Attributes above the Attribute that was deleted
            '  is done at the RDS Level.
            '----------------------------------------------------------------------------------
            '20180606-VM-ToDo: Maybe already done. Check this
        End Sub

        Public Sub removeCMMLAttributeFromTableOnly(ByRef arColumn As RDS.Column, arTable As RDS.Table)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString  '" & asAttributeId & "' FROM CoreElement"
                lsSQLQuery &= " WHERE ModelObject = '" & arTable.Name & "'"
                lsSQLQuery &= " AND Attribute = '" & arColumn.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub removeCMMLAttributeIsMandatory(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "DELETE FROM CoreIsMandatory"
            lsSQLQuery &= " WHERE IsMandatory = '" & arColumn.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLIndex(ByRef arIndex As RDS.Index)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "REMOVE INSTANCE '" & arIndex.Name & "' FROM CoreIndex"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLIsDerivedFactTypeParameter(ByRef arColumn As RDS.Column)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsDerivedFactTypeParameter.ToString
                lsSQLQuery &= " WHERE IsDerivedFactTypeParameter = '" & arColumn.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Sub removeCMMLIsPGSRelation(ByRef arTable As RDS.Table)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                lsSQLQuery &= " WHERE IsPGSRelation = '" & arTable.Name & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeCMMLPropertyFromIndexByRDSIndexColumn(ByRef arIndex As RDS.Index, ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
            lsSQLQuery &= " WHERE Index = '" & arIndex.Name & "'"
            lsSQLQuery &= " AND Property = '" & arColumn.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLActor(ByRef arCMMLActor As CMML.Actor)

            Try
                Dim lsSQLString As String = ""

                lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLString &= " WHERE Element = '" & arCMMLActor.Name & "'"
                lsSQLString &= "   AND ElementType = 'Actor'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeCMMLProcess(ByRef arCMMLProcess As CMML.Process)

            Try
                Dim lsSQLString As String = ""

                lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLString &= " WHERE Element = '" & arCMMLProcess.Id & "'"
                lsSQLString &= "   AND ElementType = 'Process'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLString)


                lsSQLString = "REMOVE INSTANCE '" & arCMMLProcess.Id & "' FROM CoreElement"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeCMMLActorProcessRelation(ByRef arCMMLActorProcessRelation As CMML.ActorProcessRelation)

            Try
                Dim lsSQLString As String = ""

                lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
                lsSQLString &= " WHERE Actor = '" & arCMMLActorProcessRelation.Actor.Name & "'"
                lsSQLString &= "   AND Process = '" & arCMMLActorProcessRelation.Process.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeCMMLProcessProcessRelation(ByRef arCMMLProcesProcessRelation As CMML.ProcessProcessRelation)

            Try
                Dim lsSQLString As String = ""

                lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
                lsSQLString &= " WHERE Process1 = '" & arCMMLProcesProcessRelation.Process1.Id & "'"
                lsSQLString &= "   AND Process2 = '" & arCMMLProcesProcessRelation.Process2.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeCMMLRelation(ByRef arRelation As RDS.Relation)

            Dim lsSQLQuery As String

            lsSQLQuery = "REMOVE INSTANCE '" & arRelation.Id & "' FROM CoreElement"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLRelationDestinationColumn(ByRef arRelation As RDS.Relation,
                                                  ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination.ToString
                lsSQLQuery &= " WHERE Attribute = '" & arColumn.Id & "'"
                lsSQLQuery &= " AND Relation = '" & arRelation.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                'lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
                'lsSQLQuery &= " WHERE RelationAttribute = '" & lrFact.Id & "'"

                'Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeCMMLRelationOriginColumn(ByRef arRelation As RDS.Relation,
                                                  ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin.ToString
                lsSQLQuery &= " WHERE Attribute = '" & arColumn.Id & "'"
                lsSQLQuery &= " AND Relation = '" & arRelation.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                'lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
                'lsSQLQuery &= " WHERE RelationAttribute = '" & lrFact.Id & "'"

                'Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' For the State Transition Model (STM) of the FBM Model...for ValueType/ValueConstraint state transitions
        ''' </summary>
        ''' <param name="arStartState"></param>
        Public Sub removeCMMLStartState(ByRef arStartState As FBM.STM.State)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString
                lsSQLQuery &= " WHERE ValueType = '" & arStartState.ValueType.Id & "'"
                lsSQLQuery &= " AND CoreElement = '" & arStartState.Name & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeCMMLState(ByRef arState As STM.State)

            Dim lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreValueTypeHasState.ToString
            lsSQLQuery &= " WHERE ValueType = '" & arState.ValueType.Id & "'"
            lsSQLQuery &= " AND State = '" & arState.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreStateHasName.ToString
            lsSQLQuery &= " WHERE State = '" & arState.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "REMOVE INSTANCE '" & arState.Id & "' FROM CoreElement"
            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        ''' <summary>
        ''' For the State Transition Model (STM) of the FBM Model...for ValueType/ValueConstraint state transitions
        ''' </summary>
        ''' <param name="arStateTransition">The StateTransition to be removed from the STM within the FBM.</param>
        Public Sub removeCMMLStateTransition(ByRef arStateTransition As FBM.STM.StateTransition)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreStateTransition.ToString
                lsSQLQuery &= " WHERE ValueType = '" & arStateTransition.ValueType.Id & "'"
                lsSQLQuery &= " AND Concept1 = '" & arStateTransition.FromState.Id & "'"
                lsSQLQuery &= " AND Concept2 = '" & arStateTransition.ToState.Id & "'"
                lsSQLQuery &= " AND Event = '" & arStateTransition.Event & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' For the State Transition Model (STM) of the FBM Model...for ValueType/ValueConstraint state transitions
        ''' </summary>
        ''' <param name="arStopState">The StopState being removed from the State Transition Model.</param>
        Public Sub removeCMMLStopState(ByRef arStopState As FBM.STM.State)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString
                lsSQLQuery &= " WHERE ValueType = '" & arStopState.ValueType.Id & "'"
                lsSQLQuery &= " AND CoreElement = '" & arStopState.Name & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Sub setCMMLAttributeOrdinalPosition(ByVal asAttributeId As String, ByVal aiOrdinalPosition As Integer)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                lsSQLQuery &= " SET Position = '" & aiOrdinalPosition.ToString & "'"
                lsSQLQuery &= " WHERE Property = '" & asAttributeId & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                'lrFactInstance = lrRecordset.CurrentFact
                'lrFactInstance.GetFactDataInstanceByRoleName("Position").Data = aiOrdinalPosition.ToString

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub setCMMLPropertyOrdinalPositionForEntity(ByVal asAttributeId As String, ByVal asEntityName As String, ByVal aiOrdinalPosition As Integer)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyHasOrdinalPositionForEntity.ToString
                lsSQLQuery &= " SET OrdinalPosition = '" & aiOrdinalPosition.ToString & "'"
                lsSQLQuery &= " WHERE Property = '" & asAttributeId & "'"
                lsSQLQuery &= "   AND Entity = '" & asEntityName & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub updateCMMLIndexIsPrimaryKey(ByRef arIndex As RDS.Index, ByVal abIsPrimaryKey As Boolean)

            Try
                Dim lsSQLQuery As String = ""

                If abIsPrimaryKey Then

                    lsSQLQuery = $"SELECT * FROM {pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString}"
                    lsSQLQuery &= $" WHERE Index = '{arIndex.Name}'"

                    Dim lrRecordset As ORMQL.Recordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset.EOF Then

                        lsSQLQuery = "INSERT INTO "
                        lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                        lsSQLQuery &= " (Index)"
                        lsSQLQuery &= " VALUES ("
                        lsSQLQuery &= "'" & arIndex.Name & "'"
                        lsSQLQuery &= ")"

                        Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                    lsSQLQuery &= " WHERE Index = '" & arIndex.Name & "'"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            End Try

        End Sub

        Public Sub updateCMMLIndexName(ByVal asOriginalName As String, ByVal asNewName As String)

            Dim lsSQLQuery As String

            lsSQLQuery = "RENAME INSTANCE '" & asOriginalName & "'"
            lsSQLQuery &= " IN CoreIndex"
            lsSQLQuery &= " TO '" & asNewName & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLIndexQualifier(ByRef arIndex As RDS.Index, ByVal asNewQualifier As String)

            Dim lsSQLQuery As String

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreIndexHasQualifier.ToString
            lsSQLQuery &= " SET Qualifier = '" & asNewQualifier & "'"
            lsSQLQuery &= " WHERE Index = '" & arIndex.Name & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLRelationDestinationMultiplicity(ByRef arRelation As RDS.Relation, ByVal aiMultiplicity As pcenumCMMLMultiplicity)

            Dim lsSQLQuery As String

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
            lsSQLQuery &= " SET Multiplicity = '" & aiMultiplicity.ToString & "'"
            lsSQLQuery &= " WHERE ERDRelation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLRelationOriginMultiplicity(ByRef arRelation As RDS.Relation, ByVal aiMultiplicity As pcenumCMMLMultiplicity)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString
            lsSQLQuery &= " SET Multiplicity = '" & aiMultiplicity.ToString & "'"
            lsSQLQuery &= " WHERE ERDRelation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLActorName(ByVal asOldName As String, ByVal asNewName As String)
            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " SET Element = '" & asNewName & "'"
                lsSQLQuery &= " WHERE Element '" & asOldName & "'"
                lsSQLQuery &= " AND ElementType = 'Actor'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub updateCMMLTableName(ByVal asOldName As String, ByVal asNewName As String)
            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "RENAME INSTANCE '" & asOldName & "' IN " & pcenumCMMLCoreModel.CoreElement.ToString & " TO '" & asNewName & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub updateORSetCMMLPropertyActiveRole(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            lsSQLQuery = "SELECT * "
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
            lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If lrRecordset.EOF Then
                lsSQLQuery = "INSERT INTO CorePropertyHasActiveRole (Property, Role)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arColumn.Id & "'"
                lsSQLQuery &= " ,'" & arColumn.ActiveRole.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            Else
                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
                lsSQLQuery &= " SET Role = '" & arColumn.ActiveRole.Id & "'"
                lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If

        End Sub

        Public Sub updateORSetCMMLPropertyRole(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            lsSQLQuery = "SELECT * "
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
            lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If lrRecordset.EOF Then
                lsSQLQuery = "INSERT INTO CorePropertyIsForRole (Property, Role)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arColumn.Id & "'"
                lsSQLQuery &= " ,'" & arColumn.Role.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            Else
                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                lsSQLQuery &= " SET Role = '" & arColumn.Role.Id & "'"
                lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If

        End Sub

        Public Sub updateRelationDestinationPredicate(ByRef arRelation As RDS.Relation, ByVal asDestinationPredicate As String)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                lsSQLQuery &= " SET Predicate = '" & asDestinationPredicate & "'"
                lsSQLQuery &= " WHERE Relation= '" & arRelation.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub updateRelationOriginPredicate(ByRef arRelation As RDS.Relation, ByVal asDestinationPredicate As String)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreOriginPredicate.ToString
            lsSQLQuery &= " SET Predicate = '" & asDestinationPredicate & "'"
            lsSQLQuery &= " WHERE Relation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateRelationDestinationTable(ByRef arRelation As RDS.Relation, ByRef arTable As RDS.Table)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
            lsSQLQuery &= " SET Entity = '" & arTable.Name & "'"
            lsSQLQuery &= " WHERE Relation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateRelationOriginTable(ByRef arRelation As RDS.Relation, ByRef arTable As RDS.Table)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
            lsSQLQuery &= " SET Entity = '" & arTable.Name & "'"
            lsSQLQuery &= " WHERE Relation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLProcessActivityTaskType(ByRef arCMMLProcess As CMML.Process, aiBPMNActivityTaskType As pcenumBPMNActivityTaskType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNActivityTaskType.ToString
                lsSQLQuery &= " SET ActivityTaskType = '" & aiBPMNActivityTaskType.ToString & "'"
                lsSQLQuery &= " WHERE Process = '" & arCMMLProcess.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Sub updateCMMLProcessActivityType(ByRef arCMMLProcess As CMML.Process, aiBPMNActivityType As pcenumBPMNActivityType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNActivityType.ToString
                lsSQLQuery &= " SET ActivityType = '" & aiBPMNActivityType.ToString & "'"
                lsSQLQuery &= " WHERE Process = '" & arCMMLProcess.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub updateCMMLProcessActivityMarker(ByRef arCMMLProcess As CMML.Process, aiBPMNActivityMarker As pcenumBPMNActivityMarker)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessHasCoreBPMNActivityMarker.ToString
                lsSQLQuery &= " SET ActivityMarker = '" & aiBPMNActivityMarker.ToString & "'"
                lsSQLQuery &= " WHERE Process = '" & arCMMLProcess.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub updateCMMLProcessEventType(ByRef arCMMLProcess As CMML.Process, aiBPMNEventType As pcenumBPMNEventType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventType.ToString
                lsSQLQuery &= " SET EventType = '" & aiBPMNEventType.ToString & "'"
                lsSQLQuery &= " WHERE Process = '" & arCMMLProcess.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub updateCMMLProcessConversationType(ByRef arCMMLProcess As CMML.Process, aiBPMNConversationType As pcenumBPMNConversationType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNConversationType.ToString
                lsSQLQuery &= " SET ConversationType = '" & aiBPMNConversationType.ToString & "'"
                lsSQLQuery &= " WHERE Process = '" & arCMMLProcess.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub updateCMMLProcessEventPosition(ByRef arCMMLProcess As CMML.Process, aiBPMNEventPosition As pcenumBPMNEventPosition)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventPosition.ToString
                lsSQLQuery &= " SET EventPosition = '" & aiBPMNEventPosition.ToString & "'"
                lsSQLQuery &= " WHERE Process = '" & arCMMLProcess.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub updateCMMLProcessEventSubType(ByRef arCMMLProcess As CMML.Process, aiBPMNEventSubType As pcenumBPMNEventSubType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventSubType.ToString
                lsSQLQuery &= " SET EventSubType = '" & aiBPMNEventSubType.ToString & "'"
                lsSQLQuery &= " WHERE Process = '" & arCMMLProcess.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub updateCMMLProcessGatewayType(ByRef arCMMLProcess As CMML.Process, aiBPMNGatewayType As pcenumBPMNGatewayType)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNGatewayType.ToString
                lsSQLQuery &= " SET GatewayType = '" & aiBPMNGatewayType.ToString & "'"
                lsSQLQuery &= " WHERE Process = '" & arCMMLProcess.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Sub AddCoreERDPGSSTMUMLModelElements(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Dim lfrmFlashCard As New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = 2600
            lfrmFlashCard.BackColor = Color.LightGray
            Dim lsMessage As String = ""
            lsMessage = "Adding core data structure."
            lfrmFlashCard.zsText = lsMessage
            Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(frmMain)

            '==================================================
            'RDS - Create a CMML Page and then dispose of it.
            Dim lrPage As FBM.Page
            Dim lrCorePage As FBM.Page

            'Now for ERDs/PGSs which have the same basic metamodel
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Element for the EntityRelationshipDiagram into the core metamodel.

            'Now for CoreDerivations
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Elements into the core metamodel.

            'Now for StateTransitionDiagrams
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Elements into the core metamodel.

            'Core Property
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreProperty.ToString)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreProperty.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Elements into the core metamodel.                

#Region "Core Relationship"
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreRelationship.ToString)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreRelationship.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Elements into the core metamodel.                
#End Region

#Region "Core UML Use Case Diagram"
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.                
#End Region

            '==================================================

            '----------------------------------------------------------------------------------------
            'Populate the Facts/FactData within the ERD/PGS Model Element metamodel Model Elements.
            Call Me.createEntityRelationshipArtifacts(aoBackgroundWorker)

            Me.ContainsLanguage.AddUnique(pcenumLanguage.EntityRelationshipDiagram)
            Me.ContainsLanguage.AddUnique(pcenumLanguage.PropertyGraphSchema)
            Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)
            Me.ContainsLanguage.AddUnique(pcenumLanguage.UMLUseCaseDiagram)

            '------------------------------------------------------------------
            'Set the CoreVersionNumber
            Me.CoreVersionNumber = prApplication.CMML.Core.CoreVersionNumber
            Call TableModel.update_model(Me)

        End Sub


        Public Sub performCoreManagement(Optional ByVal abSaveModel As Boolean = True)

            Dim lsSQLQuery As String

            If Me.CoreVersionNumber <> My.Settings.CoreVersionNumber Then
                prApplication.ThrowMessage("Upgrading the model to the new core version. This won't take long.", pcenumErrorType.Warning,, False, False, True,, True,, True)
                Boston.WriteToStatusBar("Updating the core for Model, " & Me.Name, True)
            End If

            If {"", "1.0"}.Contains(Me.CoreVersionNumber) Then
#Region " '' CoreVesionNumber"
                'Entity Relationship Diagrams / Property Graph Schema
                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                End If
                Dim lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page Model Elements for the EntityRelationshipDiagram into the metamodel

                'StateTransitions
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.
                Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)

                'CoreDerivations
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.                

#Region "Core BPMN"
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreBPMNDiagram.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreBPMNDiagram.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.                
#End Region

#Region "Core UML Use Case Diagram"
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.                
#End Region

                Me.CoreVersionNumber = "2.3"
                Me.MakeDirty(False, False)
                If abSaveModel Then Call Me.Save()
#End Region
            ElseIf Me.CoreVersionNumber = "2.0" Then
#Region "2.0"
                'NB Tightly coupled to the v5.5 release of Boston.
                'NB Must upgrade to v2.1 Core, at least nominally, because new model elements are created/copied when the user creates a STD Page.
                '  No need to create/modify the model elements here...just need to remove the old ones.
                'STM (State Transition Model) totally changed.

                If Me.GetModelObjectByName("CoreValueTypeHasFinishCoreElementState") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreValueTypeHasFinishCoreElementState"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If Me.GetModelObjectByName("CoreValueTypeHasStartCoreElementState") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreValueTypeHasStartCoreElementState"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If Me.GetModelObjectByName("CoreValueTypeIsSubtypeStateControlled") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreValueTypeIsSubtypeStateControlled"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If Me.GetModelObjectByName("CoreStateTransitionIsForValueType") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreStateTransitionIsForValueType"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If Me.GetModelObjectByName("CoreStateTransition") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreStateTransition"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                'Upgrade to CoreVersionNumber, v2.1
                Me.CoreVersionNumber = "2.1"

                If abSaveModel Then Call TableModel.update_model(Me)

                '==================================================
                'CMML. STM - (State Transition Model). Create a CMML Page and then dispose of it.                
                'For StateTransitionDiagrams
                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                End If

                Dim lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.
                Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)
#End Region
            ElseIf Me.CoreVersionNumber = "2.1" Then
#Region "2.1"
                'CodeSafe
                If Me.GetModelObjectByName(pcenumCMMLRelations.CoreValueTypeHasState.ToString) Is Nothing Then
                    Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                    If lrCorePage Is Nothing Then
                        Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                    End If

                    Dim lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.
                    Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)
                End If
#End Region
            End If

#Region "2.1"
            If Me.CoreVersionNumber = "2.1" Then
                Dim lrFactType As FBM.FactType = Me.GetModelObjectByName("CoreERDAttribute")
                If lrFactType.InternalUniquenessConstraint(0).Role.Count = 1 Then

                    Dim lsSQLCommand = "EXTEND ROLECONSTRAINT CoreInternalUniquenessConstraint16 WITH ROLE JOINING CoreEntity IN FACTTYPE CoreERDAttribute"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLCommand)
                End If
                Me.CoreVersionNumber = "2.2"
                If abSaveModel Then Call Me.Save()
            End If
#End Region

#Region "2.2"
            If Me.CoreVersionNumber = "2.2" Then

                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
                End If

                Dim lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.                

                Me.CoreVersionNumber = "2.3"
                Me.MakeDirty(False, False)
                If abSaveModel Then Call Me.Save()
            End If
#End Region

#Region "2.3"
            If Me.CoreVersionNumber = "2.3" Then
                'NB Check EnterpriseExplorer createModel as well. Line 1828
                '#Region "Core BPMN"
                '                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreBPMNDiagram.ToString)
                '                If lrCorePage Is Nothing Then
                '                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreBPMNDiagram.ToString & "', in the Core Model.")
                '                End If
                '                Dim lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.                
                '#End Region

#Region "Core UML Use Case Diagram"
                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString & "', in the Core Model.")
                End If
                Dim lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Element for the Use Case Diagrams into the core metamodel.                
#End Region
                Me.CoreVersionNumber = "2.4"
                Me.MakeDirty(False, False)
                If abSaveModel Then Call Me.Save()
            End If
#End Region

#Region "2.4"
            If {"2.4", "2.5"}.Contains(Me.CoreVersionNumber) Then

#Region "Core Property"
                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreProperty.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreProperty.ToString & "', in the Core Model.")
                End If
                Dim lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Elements into the core metamodel.                

                lsSQLQuery = "SELECT * FROM CoreERDAttribute"
                Dim lrRecordset As ORMQL.Recordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                'CodeSafe
                prApplication.WorkingModel = Me
                prApplication.Brain.Model = Me
                prApplication.WorkingPage = Nothing

                Dim liInd As Integer = 1
                prApplication.WriteToStatusBar("...upgrading")
                With New WaitCursor
                    While Not lrRecordset.EOF

                        lsSQLQuery = "CoreProperty, '" & lrRecordset("Attribute").Data & "', has CoreDBName,''."
                        Call prApplication.Brain.ProcessFEQLStatement(lsSQLQuery)

                        'prApplication.WriteToStatusBar("Upgrading to Core v2.4", False, CInt(liInd / lrRecordset.Facts.Count) * 100, False)

                        liInd += 1
                        lrRecordset.MoveNext()
                    End While
                End With
#End Region

#Region "Core Relationship"
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreRelationship.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreRelationship.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(Me, True, True, False) 'Clone the Page's Model Elements into the core metamodel.                
#End Region

                Me.CoreVersionNumber = "2.6"
                Me.MakeDirty(False, False)
                If abSaveModel Then Call Me.Save()
            End If
#End Region

        End Sub

        Private Function GetCoreKeyValue(ByVal aiRelation As pcenumCMMLRelations, ByVal asKeyName As String, ByVal asKeyIndexValue As String) As ORMQL.Recordset

            Dim lrRecordset As New ORMQL.Recordset

            Try
                Dim lrFactType = Me.FactType.Find(Function(x) x.Id = aiRelation.ToString)
                If lrFactType Is Nothing Then
                    Throw New Exception("Core Fact Type not found:" & aiRelation.ToString)
                End If

                Dim lrFactPredicate As New FBM.FactPredicate
                Dim lrRoleData = New FBM.FactData(New FBM.Role(lrFactType, asKeyName, True), New FBM.Concept(asKeyIndexValue))
                lrFactPredicate.data.Add(lrRoleData)


                '--------------------------------------------------------------------
                'Retrieve all the Facts from the FactType that match the predicate.
                '--------------------------------------------------------------------

                Dim larFactList = From Fact In lrFactType.Fact
                                  From FactData In Fact.Data
                                  Where FactData.Role.Name = asKeyName
                                  Where FactData.Data = asKeyIndexValue
                                  Select Fact

                'Dim larFactList = lrFactType.Fact.FindAll(AddressOf lrFactPredicate.Equals)

                lrRecordset.Facts = larFactList.ToList

                If larFactList.Count > 0 Then
                    lrRecordset.ColumnNames = larFactList(0).FactType.RoleGroup.Select(Function(x) x.Name).ToList
                End If

                Return lrRecordset

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return lrRecordset
            End Try

        End Function

        Private Function GetCoreKeyValue(ByVal aiRelation As pcenumCMMLRelations, ByVal aasKeyName() As String, ByVal aasKeyIndexValue() As String) As ORMQL.Recordset

            Dim lrRecordset As New ORMQL.Recordset

            Try
                Dim lrFactType = Me.FactType.Find(Function(x) x.Id = aiRelation.ToString)
                If lrFactType Is Nothing Then
                    Throw New Exception("Core Fact Type not found:" & aiRelation.ToString)
                End If

                Dim lrFactPredicate As New FBM.FactPredicate
                Dim liInd = 0
                For Each lsKeyName In aasKeyName
                    Dim lrRoleData = New FBM.FactData(New FBM.Role(lrFactType, lsKeyName, True), New FBM.Concept(aasKeyIndexValue(liInd)))
                    lrFactPredicate.data.Add(lrRoleData)
                    liInd += 1
                Next

                '--------------------------------------------------------------------
                'Retrieve all the Facts from the FactType that match the predicate.
                '--------------------------------------------------------------------
                Dim larFactList = lrFactType.Fact.FindAll(AddressOf lrFactPredicate.Equals)

                'Dim larFactList = lrFactType.Fact.FindAll(AddressOf lrFactPredicate.Equals)

                lrRecordset.Facts = larFactList.ToList

                If larFactList.Count > 0 Then
                    lrRecordset.ColumnNames = larFactList(0).Data.Select(Function(x) x.Role.Name).ToList 'FactType.RoleGroup.Select(Function(x) x.Name).ToList
                End If

                Return lrRecordset

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return lrRecordset
            End Try

        End Function

        Public Function GetSertKeyValue(ByVal aiRelation As pcenumCMMLRelations, ByVal aasKeyName() As String, ByVal aasKeyIndexValue() As String, ByVal asValue As String) As ORMQL.Recordset

            Try
                '============================================================================================
                'First check to see if the Fact exists for the FactType/Relation
                Dim lrRecordset = Me.GetCoreKeyValue(aiRelation, aasKeyName, aasKeyIndexValue)

                If lrRecordset.Facts.Count = 0 Then
                    '========================================================================================
                    'Fact does not exist, so create a new Fact and add it to the FactType/Relation

                    Dim lrFactType = Me.FactType.Find(Function(x) x.Id = aiRelation.ToString)
                    If lrFactType Is Nothing Then
                        Throw New Exception("Core Fact Type not found:" & aiRelation.ToString)
                    End If

                    Dim lrRole As FBM.Role
                    Dim lrFactData As FBM.FactData
                    Dim lrFact = New FBM.Fact(lrFactType, True)
                    Dim larRole As New List(Of FBM.Role)
                    Dim lrModelDictionaryEntry As FBM.DictionaryEntry
                    Dim liInd = 0

                    For Each lsKeyName In aasKeyName

                        lrRole = lrFactType.RoleGroup.Find(Function(x) x.Name = lsKeyName) 'AddressOf lrRole.EqualsByName)
                        larRole.Add(lrRole)

                        lrModelDictionaryEntry = New FBM.DictionaryEntry(Me, aasKeyIndexValue(liInd), pcenumConceptType.Value)
                        lrModelDictionaryEntry = Me.AddModelDictionaryEntry(lrModelDictionaryEntry, True, True, False)

                        lrFactData = New FBM.FactData(lrRole, lrModelDictionaryEntry.Concept, lrFact)

                        lrFact.Data.Add(lrFactData)

                        liInd += 1
                    Next

                    Dim lrLastRole = lrFactType.RoleGroup.Find(Function(x) Not larRole.Contains(x))
                    lrModelDictionaryEntry = New FBM.DictionaryEntry(Me, asValue, pcenumConceptType.Value)
                    lrModelDictionaryEntry = Me.AddModelDictionaryEntry(lrModelDictionaryEntry, True, True, False)
                    lrFactData = New FBM.FactData(lrLastRole, lrModelDictionaryEntry.Concept, lrFact, True)
                    lrFact.Data.Add(lrFactData)

                    lrFactType.AddFact(lrFact, False, Nothing)

                    lrRecordset.Facts.Add(lrFact)

                End If

                Return lrRecordset

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return New ORMQL.Recordset()
            End Try

        End Function

        Private Sub GetTableDetailsFromCMML(ByRef arTable As RDS.Table, ByRef aiInd As Integer)

            Dim lrORMRecordset2 As ORMQL.Recordset
            Dim lrORMRecordset3 As ORMQL.Recordset
            Dim lrResponsibleRole As FBM.Role
            Dim lrActiveRole As FBM.Role
            Dim lrColumn As RDS.Column
            Dim lrTable = arTable
            Dim lrDummyTable As RDS.Table
            Dim lsSQLQuery As String = ""
            Dim lsColumnName As String = ""


            Try

#Region "Get Table Details | Columns"
                '==========================================================================================================
                'Columns

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                lsSQLQuery &= " WHERE ModelObject = '" & lrTable.Name & "'"

                'lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrORMRecordset2 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreERDAttribute, "ModelObject", lrTable.Name)

                Dim lsColumnId As String = "" 'Used also for Debugging/ErrorThrowing.
                Dim lrSupertypeColumn As RDS.Column = Nothing

                Dim liInd = 1 'Used for OrdinalPosition if does not exist.
                While Not lrORMRecordset2.EOF

                    lrColumn = Nothing

                    Try
                        lsColumnId = lrORMRecordset2("Attribute").Data

                        'Responsible Role
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                        lsSQLQuery &= " WHERE Property = '" & lsColumnId & "'"

                        'lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        lrORMRecordset3 = Me.GetCoreKeyValue(pcenumCMMLRelations.CorePropertyIsForRole, "Property", lsColumnId)

                        If lrORMRecordset3("Role").Data = "" Then Throw New Exception("No Responsible Role for Property")

                        lrResponsibleRole = Me.Role.Find(Function(x) x.Id = lrORMRecordset3("Role").Data)

                        Dim lrResponsibleRoleTable As RDS.Table = lrResponsibleRole.getCorrespondingRDSTable
                        If lrTable IsNot lrResponsibleRoleTable And lrResponsibleRoleTable.Column.Count > 0 Then

                            lrSupertypeColumn = lrResponsibleRoleTable.Column.Find(Function(x) x.Id = lsColumnId)
                            If lrSupertypeColumn Is Nothing Then
                                'CodeSafe...fall back to ResponsibleRole
                                lrSupertypeColumn = lrResponsibleRoleTable.Column.Find(Function(x) x.Role.Id = lrResponsibleRole.Id)
                            End If
                            lrColumn = lrSupertypeColumn 'So can get/set OrdinalPosition if required. See setting of OrdinalPosition by Table below.

                            lrDummyTable = lrTable

                            Dim lrNewColumn = lrSupertypeColumn.Clone(lrDummyTable, Nothing, True)

                            Dim lrExistingColumn = lrTable.Column.Find(Function(x) x.Role Is lrNewColumn.Role And x.ActiveRole Is lrNewColumn.ActiveRole)
                            If lrExistingColumn Is Nothing Then
                                Call lrTable.Column.Add(lrNewColumn)
                                lrColumn = lrNewColumn 'So can get/set OrdinalPosition if required. See setting of OrdinalPosition by Table below.
                            End If
                        Else
                            'Column Name
                            'lsSQLQuery = "SELECT *"
                            'lsSQLQuery &= " FROM CorePropertyHasPropertyName" '(Property, PropertyName)
                            'lsSQLQuery &= " WHERE Property = '" & lrORMRecordset2("Attribute").Data & "'"

                            'lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            lrORMRecordset3 = Me.GetCoreKeyValue(pcenumCMMLRelations.CorePropertyHasPropertyName, "Property", lrORMRecordset2("Attribute").Data)
                            lsColumnName = lrORMRecordset3("PropertyName").Data

                            'Active Role
                            'lsSQLQuery = "SELECT *"
                            'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
                            'lsSQLQuery &= " WHERE Property = '" & lrORMRecordset2("Attribute").Data & "'"

                            'lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            lrORMRecordset3 = Me.GetCoreKeyValue(pcenumCMMLRelations.CorePropertyHasActiveRole, "Property", lrORMRecordset2("Attribute").Data)
                            If lrORMRecordset3("Role").Data <> "" Then
                                lrActiveRole = Me.Role.Find(Function(x) x.Id = lrORMRecordset3("Role").Data)
                            End If

                            'New Column
                            lrColumn = New RDS.Column(lrTable, lsColumnName, lrResponsibleRole, lrActiveRole)
                            lrColumn.Id = lsColumnId

                            lrORMRecordset3 = Me.GetCoreKeyValue(pcenumCMMLRelations.CorePropertyHasDBName, "Property", lrORMRecordset2("Attribute").Data)
                            lrColumn.DBName = lrORMRecordset3("DBName").Data

                            'IsMandatory
                            'lsSQLQuery = "SELECT *"
                            'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsMandatory.ToString
                            'lsSQLQuery &= " WHERE IsMandatory = '" & lrColumn.Id & "'"

                            'lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            lrORMRecordset3 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreIsMandatory, "IsMandatory", lrColumn.Id)
                            lrColumn.IsMandatory = Not lrORMRecordset3.EOF

                            'Fact Type
                            'lsSQLQuery = "SELECT *"
                            'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                            'lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                            'lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            lrORMRecordset3 = Me.GetCoreKeyValue(pcenumCMMLRelations.CorePropertyIsForFactType, "Property", lrColumn.Id)
                            If lrORMRecordset3("FactType").Data <> "" Then
                                lrColumn.FactType = Me.FactType.Find(Function(x) x.Id = lrORMRecordset3("FactType").Data)
                            End If

                            'Ordinal Position
                            'lsSQLQuery = "SELECT *"
                            'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                            'lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                            'lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            'Special handling for OrdinalPosition of Subtypes                    
                            If lrTable.isSubtype Then
                                lrORMRecordset3 = Me.GetSertKeyValue(pcenumCMMLRelations.CorePropertyHasOrdinalPositionForEntity, {"Property", "Entity"}, {lrColumn.Id, lrTable.Name}, liInd)
                                lrColumn.OrdinalPosition = lrORMRecordset3("OrdinalPosition").Data
                            Else
                                lrORMRecordset3 = Me.GetCoreKeyValue(pcenumCMMLRelations.CorePropertyHasOrdinalPosition, "Property", lrColumn.Id)
                                lrColumn.OrdinalPosition = lrORMRecordset3("Position").Data
                            End If


                            'NB Special handling for Ordinal Position of Subtypes below Try

                            'Is Derived Fact Type Parameter
                            'lsSQLQuery = " SELECT COUNT(*)"
                            'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsDerivedFactTypeParameter.ToString
                            'lsSQLQuery &= " WHERE IsDerivedFactTypeParameter = '" & lrColumn.Id & "'"

                            'lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            lrORMRecordset3 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreAttributeIsDerivedFactTypeParameter, "IsDerivedFactTypeParameter", lrColumn.Id)

                            If lrORMRecordset3.Facts.Count > 0 Then 'Not doing SELECT COUNT(*) and more. lrORMRecordset3(0).Data > 0 Then
                                lrColumn.IsDerivationParameter = True
                            End If

                            lrTable.Column.AddUnique(lrColumn)
                        End If

                    Catch ex As Exception
#Region "Exception Handling"
                        If My.Settings.AutomaticallyDeleteTroublesomeColumns Then
                            'NB This is not advisable. Turning it on deleted all the columns in the CinemaBookings Model.
                            'Call Me.removeCMMLAttribute(lsColumnId)
                        ElseIf My.Settings.RDSOnXMLLoadIgnoreTroublesomeColumns Then
                            liInd += 1
                            lrORMRecordset2.MoveNext()
                            Continue While
                        Else

                            Dim lsErrorMessage As String = "Trouble loading Column for Table, " & lrTable.Name & ". Column.Id = " & lsColumnId
                            lsErrorMessage &= vbCrLf & vbCrLf & "Skipping loading of the Column from the ORM model (only) as a precaution."
                            If Me.IsDatabaseSynchronised Then
                                lsErrorMessage &= " Contact support for instructions how to fix this problem."
                            End If
                            lsErrorMessage.AppendDoubleLineBreak("Optionally you can delete the Column from the database altogether. Click [Yes] to delete the Column, or [No] to keep the Column.")
                            lsErrorMessage.AppendString(" You should only delete the Column if you know what you are doing or as advised by support. Backup your database before deleting core elements.")

                            'Dim lbDatabaseSynchronisation As Boolean = Me.IsDatabaseSynchronised
                            'Me.IsDatabaseSynchronised = False
                            'Call Me.removeCMMLAttribute(lrTable.Name, lsColumnId)
                            'Me.IsDatabaseSynchronised = lbDatabaseSynchronisation

                            lsErrorMessage &= vbCrLf & vbCrLf & ex.StackTrace
                            If prApplication.ThrowMessage(lsErrorMessage, pcenumErrorType.Information,
                                                                ex.StackTrace,
                                                                False,
                                                                False,
                                                                True,
                                                                MessageBoxButtons.YesNo) = DialogResult.Yes Then
                                '20210813-VM-If this gets out of hand, remove this functionality.
                                Call Me.removeCMMLAttribute(lsColumnId)
                            End If
                        End If
#End Region
                    End Try

                    liInd += 1
                    lrORMRecordset2.MoveNext()
                End While
                '==========================================================================================================
                '===========================
                'Indexes                    
                Call Me.loadIndexesForTable(lrTable)

                Dim larNullActiveRoles = From Column In lrTable.Column
                                         Where Column.ActiveRole Is Nothing
                                         Select Column

                If larNullActiveRoles.Count > 0 Then
                    'CodeSafe
                    For Each lrColumn In larNullActiveRoles.ToArray
                        Try
                            If lrColumn.Role.FactType.Id = lrColumn.Table.Name And
lrColumn.Role.FactType.IsObjectified Then
                                If lrColumn.Role.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                                    lrColumn.ActiveRole = lrColumn.Role
                                    Call Me.updateORSetCMMLPropertyActiveRole(lrColumn)
                                End If
                            End If
                        Catch ex As Exception
                            'CodeSafe
                            If lrColumn.ActiveRole Is Nothing And lrColumn.Role Is Nothing And lrColumn.FactType Is Nothing Then
                                Call lrTable.removeColumn(lrColumn)
                            End If
                        End Try
                    Next
                End If

                aiInd += 1 'For reporting progress on Tables loaded.
#End Region 'Table Details

            Catch ex As Exception

            End Try
        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aoBackgroundWorker">To report progress. Start at 80%.</param>
        Public Sub PopulateAllCoreStructuresFromCoreMDAElements(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Try
                Dim loBackgroundWorker = aoBackgroundWorker

                '20240229-VM-For now. Set pbDoDatabaseProcessing = True at end
                pbDoDatabaseProcessing = False

                'CodeSafe
                If Me.RDS.Table.Count > 0 Then Exit Sub


                Me.RDSLoading = True
                Dim lsMessage As String

                Dim lsSQLQuery As String = ""
                Dim lrTable As RDS.Table

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE ElementType = 'Entity'"

                Dim lrORMRecordset,
                    lrORMRecordset3 As ORMQL.Recordset

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrModelElement As FBM.ModelObject
                Dim lsColumnName As String = ""



                While Not lrORMRecordset.EOF
#Region "Tables"
                    '-----------------------------------------------------
                    'Get the underlying ModelElement
                    lrModelElement = Me.GetModelObjectByName(lrORMRecordset("Element").Data,,,,, pcenumConceptType.EntityType)

                    If lrModelElement Is Nothing Then
                        'This is dire. Create a dummy FBMEntityType for the Table, and add the EntityType to the Model. 
                        '  The user can then elect to delete the EntityType/Table if it shouldn't be in the model
                        Dim lrEntityType = Me.CreateEntityType(lrORMRecordset("Element").Data, True, False, False, True)
                        lrModelElement = lrEntityType

                        'Let the user know what happened.
                        lsMessage = "The Table, '" & lrModelElement.Name & "', within the relational model had no corresponding Object-Role Model model element."
                        lsMessage &= vbCrLf & vbCrLf & "An Entity Type has been created in the model to cater for this. If you no longer need this model element remove it from the model."
                        Call prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical)
                    End If

                    lrTable = New RDS.Table(Me.RDS, lrORMRecordset("Element").Data, lrModelElement)
                    Me.RDS.Table.AddUnique(lrTable)

                    'PGS Relation
                    Try
                        lsSQLQuery = " SELECT COUNT(*)"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                        lsSQLQuery &= " WHERE IsPGSRelation = '" & lrTable.Name & "'"

                        lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset3(0).Data > 0 Then
                            lrTable.isPGSRelation = True
                        End If
                    Catch ex As Exception
                        'Not a biggie at this stage.
                    End Try

                    lrORMRecordset.MoveNext()
                End While 'Stepping through Tables

                '==Sort the tables===========================
                Dim larSortedTables = Me.RDS.Table.OrderBy(Function(x) x.getSupertypeTables.Count)

                Dim maxConcurrentThreads As Integer = 2 ' Environment.ProcessorCount * 2 ' Adjust as needed

                '==Get the Table details===========================
#Region "Table Details. Columns etc"
                Dim liInd As Integer = 0 'For reporting progress on Tables loaded.
                For Each lrTable In larSortedTables

                    Boston.WriteToStatusBar("Loading Model. CMML Load for Entity: " & lrTable.Name, True)

                    'Table/Column Details
                    Call Me.GetTableDetailsFromCMML(lrTable, liInd)

                    If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(Viev.Lesser(99, 80 + CInt(19 * (liInd / larSortedTables.Count))))

                Next
#End Region
                '===================================================
#End Region


                '==========================================================================================================
                'Relations                
                Call Me.populateRDSRelationsFromCoreMDAElements()

                '==========================================================
                'State Transition Model
                '  NB Is called from within this thread so as to not clash on the ORMQL Parser.
                If CDbl(Viev.NullVal(Me.CoreVersionNumber, 0)) >= 2.1 Then
                    Call Me.PopulateSTMStructureFromCoreMDAElements()
                End If

                If CDbl(Viev.NullVal(Me.CoreVersionNumber, 0)) >= 2.2 Then
                    Call Me.PopulateCMMLStructureFromCoreMDAElements()
                End If

                pbDoDatabaseProcessing = True

                SyncLock Me
                    Me.RDSLoading = False
                End SyncLock

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Me.RDSLoading = False
            End Try

        End Sub


        Public Sub PopulateCMMLStructureFromCoreMDAElements(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Try
                Me.RDSLoading = True
                Dim lsMessage As String

                Dim lsSQLQuery As String = ""
                Dim lrActor As CMML.Actor
                Dim lrProcess, lrProcess1, lrProcess2 As CMML.Process
                Dim lrActorProcessRelation As CMML.ActorProcessRelation
                Dim lrProcessProcessRelation As CMML.ProcessProcessRelation

#Region "Actors"
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE ElementType = 'Actor'"

                Dim lrORMRecordset, lrORMRecordset2 As ORMQL.Recordset

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrModelElement As FBM.ModelObject
                Dim lsColumnName As String = ""
                Dim liInd As Integer = 0 'For reporting progress on Actors loaded.

                While Not lrORMRecordset.EOF
                    '-----------------------------------------------------
                    'Get the underlying ModelElement
                    lrModelElement = Me.GetModelObjectByName(lrORMRecordset("Element").Data)

                    If lrModelElement Is Nothing Then
                        'This is dire. Create a dummy FBMEntityType for the Actor, and add the EntityType to the Model. 
                        '  The user can then elect to delete the EntityType/Actor if it shouldn't be in the model
                        Dim lrEntityType = Me.CreateEntityType(lrORMRecordset("Element").Data, True)
                        lrModelElement = lrEntityType

                        'Let the user know what happened.
                        lsMessage = "The Actor, '" & lrModelElement.Name & "', within the relational model had no corresponding Object-Role Model model element."
                        lsMessage &= vbCrLf & vbCrLf & "An Entity Type has been created in the model to cater for this. If you no longer need this model element remove it from the model."
                        Call prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical)
                    End If

                    lrActor = New CMML.Actor(Me.UML, lrORMRecordset("Element").Data, lrModelElement)

                    Select Case lrModelElement.GetType
                        Case Is = GetType(FBM.EntityType)
                            Call CType(lrModelElement, FBM.EntityType).setIsActor(True, True)
                        Case Is = GetType(FBM.FactType)
                            '20220619-VM-Not implemented yet.
                            'CType(lrModelElement, FBM.FactType).IsActor = True
                    End Select

                    Me.UML.Actor.Add(lrActor)

                    lrORMRecordset.MoveNext()

                End While 'Stepping through Actors
#End Region

#Region "Processes"
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE ElementType = 'Process'"

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lsProcessText As String = ""

                While Not lrORMRecordset.EOF

#Region "Process Text"
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsProcessText = "Error-No Process Text"
                    If Not lrORMRecordset2.EOF Then
                        lsProcessText = lrORMRecordset2("ProcessText").Data
                    End If
#End Region

                    lrProcess = New CMML.Process(Me.UML, lrORMRecordset("Element").Data, lsProcessText)

                    '===================================================
                    'For 6.4 Release Skip BPMN artefacts.
                    '======================================
                    GoTo SkipBPMNFactTypes

#Region "Process Type"
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNProcessType.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.ProcessType = CType([Enum].Parse(GetType(pcenumBPMNProcessType), Trim(lrORMRecordset2("ProcessType").Data)), pcenumBPMNProcessType)
                    End If
#End Region

#Region "ActivityTaskType"
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNActivityTaskType.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.ActivityTaskType = CType([Enum].Parse(GetType(pcenumBPMNActivityTaskType), Trim(lrORMRecordset2("ActivityTaskType").Data)), pcenumBPMNActivityTaskType)
                    End If
#End Region

#Region "ActivityType"
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNActivityType.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.ActivityType = CType([Enum].Parse(GetType(pcenumBPMNActivityType), Trim(lrORMRecordset2("ActivityType").Data)), pcenumBPMNActivityType)
                    End If
#End Region

#Region "ActivityMarker"
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreProcessHasCoreBPMNActivityMarker.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.ActivityMarker = CType([Enum].Parse(GetType(pcenumBPMNActivityMarker), Trim(lrORMRecordset2("ActivityMarker").Data)), pcenumBPMNActivityMarker)
                    End If
#End Region

#Region "EventType"
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventType.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.EventType = CType([Enum].Parse(GetType(pcenumBPMNEventType), Trim(lrORMRecordset2("EventType").Data)), pcenumBPMNEventType)
                    End If
#End Region

#Region "ConversationType"
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNConversationType.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.ConversationType = CType([Enum].Parse(GetType(pcenumBPMNConversationType), Trim(lrORMRecordset2("ConversationType").Data)), pcenumBPMNConversationType)
                    End If
#End Region

#Region "EventPosition"
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventPosition.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.EventPosition = CType([Enum].Parse(GetType(pcenumBPMNEventPosition), Trim(lrORMRecordset2("EventPosition").Data)), pcenumBPMNEventPosition)
                    End If
#End Region

#Region "EventSubType"

                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNEventSubType.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.EventSubType = CType([Enum].Parse(GetType(pcenumBPMNEventSubType), Trim(lrORMRecordset2("EventSubType").Data)), pcenumBPMNEventSubType)
                    End If
#End Region

#Region "Gateway Type"
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessIsOfCoreBPMNGatewayType.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcess.GatewayType = CType([Enum].Parse(GetType(pcenumBPMNGatewayType), Trim(lrORMRecordset2("GatewayType").Data)), pcenumBPMNGatewayType)
                    End If
#End Region

SkipBPMNFactTypes:

                    Me.UML.Process.Add(lrProcess)

                    lrORMRecordset.MoveNext()

                End While 'Stepping through Processes
#End Region

#Region "Actor to Process Relations"
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    lrActor = Me.UML.Actor.Find(Function(x) x.Name = lrORMRecordset("Actor").Data)
                    lrProcess = Me.UML.Process.Find(Function(x) x.Id = lrORMRecordset("Process").Data)

                    lrActorProcessRelation = New CMML.ActorProcessRelation(Me.UML, lrActor, lrProcess)
                    lrActorProcessRelation.Fact = lrORMRecordset.CurrentFact

                    Me.UML.ActorProcessRelation.Add(lrActorProcessRelation)

                    lrActor.Process.Add(lrProcess)

                    lrORMRecordset.MoveNext()

                End While 'Stepping through Actor to Process Relations
#End Region

#Region "Process to Process Relations"
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    lrProcess1 = Me.UML.Process.Find(Function(x) x.Id = lrORMRecordset("Process1").Data)
                    lrProcess2 = Me.UML.Process.Find(Function(x) x.Id = lrORMRecordset("Process2").Data)

                    lrProcessProcessRelation = New CMML.ProcessProcessRelation(Me.UML, lrProcess1, lrProcess2)
                    lrProcessProcessRelation.Fact = lrORMRecordset.CurrentFact

#Region "Is Extends"
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreP2PIsExtends"
                    lsSQLQuery &= " WHERE CoreP2PIsExtends = '" & lrProcessProcessRelation.Fact.Id & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcessProcessRelation.IsExtends = True
                    End If
#End Region

#Region "Is Includes"
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreP2PIsIncludes"
                    lsSQLQuery &= " WHERE CoreP2PIsIncludes = '" & lrProcessProcessRelation.Fact.Id & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcessProcessRelation.IsIncludes = True
                    End If
#End Region

                    Me.UML.ProcessProcessRelation.AddUnique(lrProcessProcessRelation)

                    'lrProcess1.Process.Add(lrProcess2)

                    lrORMRecordset.MoveNext()

                End While 'Stepping through Process-to-Process Relations
#End Region

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Me.RDSLoading = False
            End Try

        End Sub


    End Class

End Namespace
