
Imports Boston.FBM.STM

Namespace STD

    Public Class Diagram

        Public Page As FBM.Page = Nothing

        Public ValueType As FBM.ValueType = Nothing
        Public State As New List(Of STD.State)
        Public StateTransition As New List(Of STD.StateTransition)
        Public EndStateIndicator As New List(Of STD.EndStateIndicator)
        Public EndStateTransition As New List(Of STD.EndStateTransition)

        Public StartBubble As STD.StartStateIndicator

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
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreValueTypeHasFinishCoreElementState.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"

                Call Me.Page.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            End If

        End Sub

    End Class

End Namespace