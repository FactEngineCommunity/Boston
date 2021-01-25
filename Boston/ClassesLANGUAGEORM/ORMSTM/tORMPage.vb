Imports Boston.FBM.STM

Namespace FBM


    Partial Public Class Page
        <NonSerialized>
        Public WithEvents STModel As New FBM.STM.Model

        ''' <summary>
        ''' Handles the event when a new EndStateTransition is added to the (ST) Model.
        ''' </summary>
        ''' <param name="arEndStateTranstion"></param>
        Private Sub STModel_EndStateTransitionAdded(ByRef arEndStateTranstion As EndStateTransition) Handles STModel.EndStateTransitionAdded

            'Dim lsSQLQuery = "ADD FACT " & 

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

                Me.STDiagram.StateTransition.AddUnique(lrStateTransition)
                Call lrStateTransition.DisplayAndAssociate()

            End If

        End Sub

    End Class

End Namespace
