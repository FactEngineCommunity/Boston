Namespace FBM.STM

    ''' <summary>
    ''' STM Stands for State Transition Model, rather than STD (State Transition Diagram), because a STM may have more than one diagram.
    ''' </summary>
    Public Class Model

        ''' <summary>
        ''' The FBM Model to which this STM belongs.
        ''' </summary>
        Public Model As FBM.Model = Nothing

        ''' <summary>
        ''' The ValueType for which the State Transition Model (STM) exists. I.e. States of the STM are Value Constraint values for the ValueType of the STM.
        ''' </summary>
        Public ValueType As FBM.ValueType = Nothing

        ''' <summary>
        ''' The list of States for the STM
        ''' </summary>
        Public State As New List(Of FBM.STM.State)

        ''' <summary>
        ''' The list of StateTransitions for the STM.
        ''' </summary>
        Public StateTransition As New List(Of FBM.STM.StateTransition)

        ''' <summary>
        ''' The list of StartStates for the STM.
        ''' </summary>
        Public StartState As New List(Of FBM.STM.State)

        ''' <summary>
        ''' The list of StopStates for the STM.
        ''' </summary>
        Public StopState As New List(Of FBM.STM.State)

        Public EndStateTransition As New List(Of FBM.STM.EndStateTransition)

        '================================
        'Events
        Public Event EndStateTransitionAdded(ByRef arEndStateTransition As FBM.STM.EndStateTransition)
        Public Event EndStateTransitionRemoved(ByRef arEndStateTransition As FBM.STM.EndStateTransition)
        Public Event StateTransitionAdded(ByRef arStateTransition As FBM.STM.StateTransition)

        ''' <summary>
        ''' Parmeterless constructor
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="arModel">The FactBasedModel that this State Transition Model belongs to.</param>
        Public Sub New(ByRef arModel As FBM.Model)

            Me.Model = arModel

        End Sub

        Public Sub addEndStateTransition(ByRef arEndStateTransition As STM.EndStateTransition)

            Me.EndStateTransition.Add(arEndStateTransition)

            'CMML
            Call Me.Model.createCMMLEndStateTransition(arEndStateTransition)

            RaiseEvent EndStateTransitionAdded(arEndStateTransition)
        End Sub

        Public Sub addStartState(ByRef arStartState As FBM.STM.State)

            Me.StartState.AddUnique(arStartState)

            'CMML
            Call Me.Model.addCMMLStartState(arStartState)

        End Sub


        Public Sub addState(ByRef arState As FBM.STM.State)

            Me.State.AddUnique(arState)

        End Sub

        Public Sub addStateTransition(ByRef arStateTransition As FBM.STM.StateTransition)

            Me.StateTransition.AddUnique(arStateTransition)

            'CMML
            Call Me.Model.addCMMLStateTransition(arStateTransition)

            RaiseEvent StateTransitionAdded(arStateTransition)

        End Sub

        Public Sub removeEndStateTransition(ByRef arEndStateTransition As STM.EndStateTransition)

            Me.EndStateTransition.Remove(arEndStateTransition)

            'CMML
            Call Me.Model.removeCMMLEndStateTransition(arEndStateTransition)

            RaiseEvent EndStateTransitionRemoved(arEndStateTransition)

        End Sub

        Public Sub removeState(ByVal arState As FBM.STM.State)

            Call Me.State.Remove(arState)

            'Remove all the StateTransitions that reference the State being removed.
            Dim larStateTransition = From StateTransition In Me.StateTransition
                                     Where (StateTransition.FromState.Name = arState.Name _
                                     Or StateTransition.ToState.Name = arState.Name)
                                     Where StateTransition.ValueType.Id = arState.ValueType.Id
                                     Select StateTransition

            For Each lrStateTransition In larStateTransition
                Call Me.removeStateTransition(lrStateTransition)
            Next

            'Remove StartState/s for the State
            Dim larStartState = From StartState In Me.StartState
                                Where StartState.Name = arState.Name
                                Where StartState.ValueType.Id = arState.ValueType.Id
                                Select StartState

            For Each lrStartState In larStartState
                Call Me.removeStartState(lrStartState)
            Next

            'Remove StartState/s for the State
            Dim larStopState = From StopState In Me.StartState
                               Where StopState.Name = arState.Name
                               Where StopState.ValueType.Id = arState.ValueType.Id
                               Select StopState

            For Each lrStopState In larStopState
                Call Me.removeStopState(lrStopState)
            Next

        End Sub

        Public Sub removeStartState(ByRef arStartState As FBM.STM.State)

            Call Me.StartState.Remove(arStartState)
            arStartState.IsStart = False

            'CMML
            Call Me.Model.removeCMMLStartState(arStartState)
        End Sub

        Public Sub removeStateTransition(ByRef arStateTransition As FBM.STM.StateTransition)

            Call Me.StateTransition.Remove(arStateTransition)

            'CMML
            Call Me.Model.removeCMMLStateTransition(arStateTransition)
        End Sub

        Public Sub removeStopState(ByRef arStopState As FBM.STM.State)

            Call Me.StopState.Remove(arStopState)

            'CMML
            Call Me.Model.removeCMMLStopState(arStopState)
        End Sub

    End Class

End Namespace
