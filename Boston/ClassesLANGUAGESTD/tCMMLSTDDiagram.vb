
Imports Boston.FBM.STM

Namespace STD

    Public Class Diagram

        Public Page As FBM.Page = Nothing

        Public ValueType As FBM.ValueType = Nothing
        Public State As New List(Of STD.State)
        Public StateTransition As New List(Of STD.StateTransition)

        Public StartIndicator As STD.StartStateIndicator
        Public EndStateIndicator As New List(Of STD.EndStateIndicator)

        Public StartStateTransition As New List(Of STD.StartStateTransition)
        Public EndStateTransition As New List(Of STD.EndStateTransition)

        Public WithEvents STM As FBM.STM.Model

        Public Sub New(ByRef arPage As FBM.Page)
            Me.Page = arPage
        End Sub

        Private Sub STM_EndStateTransitionAdded(ByRef arEndStateTranstion As FBM.STM.EndStateTransition) Handles STM.EndStateTransitionAdded

            Dim lsSQLQuery As String

            '-----------------------------------------------------------------------------------------------------------------------
            'Only add the EndStateTransition to the Page if the Page.STDiagram is for the same ValueType as the EndStateTransition.           
            If arEndStateTranstion.ValueType.Id = Me.ValueType.Id Then

                lsSQLQuery = "ADD FACT '" & arEndStateTranstion.Fact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"

                Dim lrFactInstance As FBM.FactInstance = Me.Page.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lsFromStateName As String = arEndStateTranstion.State.Name
                Dim lrFromState As STD.State = Me.State.Find(Function(x) x.StateName = lsFromStateName)
                Dim lsEndStateId As String = arEndStateTranstion.EndStateId
                Dim lrEndStateIndicator As STD.EndStateIndicator = Me.EndStateIndicator.Find(Function(x) x.EndStateId = lsEndStateId)
                Dim lrEndStateTransition = lrFactInstance.CloneEndStateTransition(Me.Page, lrFromState, lrEndStateIndicator)

                Call lrEndStateTransition.DisplayAndAssociate()

            End If

        End Sub

    End Class

End Namespace