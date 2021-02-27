Imports Boston.FBM.STM
Imports System.Reflection

Namespace FBM


    Partial Public Class Page
        <NonSerialized>
        Public WithEvents STModel As New FBM.STM.Model

        Public Function dropStateAtPoint(ByRef arSTMState As STM.State, ByVal aoPtF As PointF) As STD.State

            Try

                Dim lrSTDState As STD.State

                Dim lsSQLQuery = "ADD FACT '" & arSTMState.Fact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreValueTypeHasState.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Dim lrFactInstance As FBM.FactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrSTDState = lrFactInstance("State").CloneState(Me)
                lrSTDState.STMState = arSTMState
                lrSTDState.StateName = arSTMState.Name
                lrSTDState.ValueType = arSTMState.ValueType

                lrSTDState.Move(aoPtF.X, aoPtF.Y, False)
                lrSTDState.DisplayAndAssociate()

                Return lrSTDState

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Used to clean things up before saving the Page, esp for STDiagrams.
        ''' </summary>
        Private Sub performCMMLPreSaveProcessing()

            Try

                If Me.Language = pcenumLanguage.StateTransitionDiagram Then

                    If Me.STDiagram.EndStateTransition.Count = 0 And Me.STDiagram.EndStateIndicator.Count > 0 Then
                        'Get rid of all the EndStateIndicators that are orphaned.
                        For Each lrSTDEndStateIndicator In Me.STDiagram.EndStateIndicator.ToArray
                            Call lrSTDEndStateIndicator.STMEndStateIndicator.removeFromModel()
                        Next
                    End If

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the ValueType for a StateTransitionDiagram Page.
        '''   NB Checks to see that no other ValueType is set for the Page. Each STD Page is for one ValueType and its ValueType.ValueConstraints.
        ''' </summary>
        ''' <param name="arFact"></param>
        Public Sub setValueTypeAsStateTransitionBased(ByRef arFact As FBM.Fact)

            Dim lsSQLQuery As String

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreValueTypeIsStateTransitionBased.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

            Dim lrRecordset As ORMQL.Recordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If lrRecordset.Facts.Count > 0 Then

                Dim lrFact As FBM.Fact = arFact
                Dim lrFactInstance = Me.FactTypeInstance.Find(Function(x) x.Id = lrFact.FactType.Id).Fact.Find(Function(x) x.Id = lrRecordset.CurrentFact.Id)

                If lrFactInstance IsNot Nothing Then
                    Me.FactTypeInstance.Find(Function(x) x.Id = lrFact.FactType.Id).RemoveFactById(lrFactInstance.Fact)
                End If
            End If

            lsSQLQuery = "ADD FACT '" & arFact.Id & "'"
            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreValueTypeIsStateTransitionBased.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

            Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


        End Sub


        ''' <summary>
        ''' Handles the event when a new EndStateTransition is added to the (ST) Model.
        ''' </summary>
        ''' <param name="arEndStateTransition"></param>
        Private Sub STModel_EndStateTransitionAdded(ByRef arEndStateTransition As EndStateTransition) Handles STModel.EndStateTransitionAdded

            If Not Me.Language = pcenumLanguage.StateTransitionDiagram Then Exit Sub
            If Me.STDiagram.ValueType Is Nothing Then Exit Sub

            If Me.STDiagram.ValueType.Id = arEndStateTransition.ValueType.Id Then

                Dim lsSQLQuery = "ADD FACT '" & arEndStateTransition.Fact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Dim lrFactInstance As FBM.FactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lsFromStateName = arEndStateTransition.State.Name
                Dim lrFromState = Me.STDiagram.State.Find(Function(x) x.StateName = lsFromStateName)

                Dim lsEndStateId = arEndStateTransition.EndStateId
                Dim lrEndStateIndicator = Me.STDiagram.EndStateIndicator.Find(Function(x) x.EndStateId = lsEndStateId)

                Dim lrEndStateTransition As STD.EndStateTransition
                lrEndStateTransition = lrFactInstance.CloneEndStateTransition(Me, lrFromState, lrEndStateIndicator)
                lrEndStateTransition.STMEndStateTransition = arEndStateTransition

                Me.STDiagram.EndStateTransition.AddUnique(lrEndStateTransition)
                Call lrEndStateTransition.DisplayAndAssociate()

            End If

        End Sub

        Private Sub STModel_StateTransitionAdded(ByRef arStateTransition As StateTransition) Handles STModel.StateTransitionAdded

            If Not Me.Language = pcenumLanguage.StateTransitionDiagram Then Exit Sub
            If Me.STDiagram.ValueType Is Nothing Then Exit Sub

            If Me.STDiagram.ValueType.Id = arStateTransition.ValueType.Id Then

                Dim lsSQLQuery = "ADD FACT '" & arStateTransition.Fact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreStateTransition.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Dim lrFactInstance As FBM.FactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lsFromStateName = arStateTransition.FromState.Name
                Dim lrFromState = Me.STDiagram.State.Find(Function(x) x.StateName = lsFromStateName)

                Dim lsToStateName = arStateTransition.ToState.Name
                Dim lrToState = Me.STDiagram.State.Find(Function(x) x.StateName = lsToStateName)

                Dim lrStateTransition As New STD.StateTransition
                lrStateTransition = lrFactInstance.CloneStateTransition(Me, lrFromState, lrToState, arStateTransition.Event)

                If lrToState.Fact Is Nothing Then
                    'State hasn't been allocated to a FactDataInstance yet
                    Dim lrNewToState = lrFactInstance("Concept2").CloneState(Me)
                    Me.Diagram.Nodes.Remove(lrToState.Shape)
                    Me.STDiagram.State.Remove(lrToState)
                    Me.STDiagram.State.Add(lrNewToState)
                    lrNewToState.Move(lrToState.X, lrToState.Y, False)
                    Call lrNewToState.DisplayAndAssociate()
                    lrStateTransition.ToState = lrNewToState
                End If

                lrStateTransition.STMStateTransition = arStateTransition
                Me.STDiagram.StateTransition.AddUnique(lrStateTransition)
                Call lrStateTransition.DisplayAndAssociate()

            End If

        End Sub

        Private Sub STModel_EndStateTransitionRemoved(ByRef arEndStateTransition As STM.EndStateTransition) Handles STModel.EndStateTransitionRemoved

            Dim lrEndStateTransition As STM.EndStateTransition = arEndStateTransition

            Dim lrSTDEndStateTransition As STD.EndStateTransition = Me.STDiagram.EndStateTransition.Find(Function(x) x.ValueType.Id = lrEndStateTransition.ValueType.Id And
                                                                                                    x.FromState.Name = lrEndStateTransition.State.Name And
                                                                                                    x.EndStateIndicator.EndStateId = lrEndStateTransition.EndStateId)

            If lrSTDEndStateTransition IsNot Nothing Then
                'EndStateTransition is on the Page.

                Me.Diagram.Links.Remove(lrSTDEndStateTransition.Link)

                Me.STDiagram.EndStateTransition.Remove(lrSTDEndStateTransition)
            End If

        End Sub

    End Class

End Namespace
