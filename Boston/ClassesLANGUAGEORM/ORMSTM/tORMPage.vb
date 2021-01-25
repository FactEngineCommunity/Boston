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

            If Me.STDiagram.StateTransitionValueType.Id = arStateTransition.ValueType.Id Then

                Dim lsSQLQuery = "ADD FACT '" & arStateTransition.Fact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreStateTransition.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                'Draw the link.

            End If

        End Sub

    End Class

End Namespace
