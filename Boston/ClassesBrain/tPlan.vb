Imports System.Reflection

Namespace Brain
    ''' <summary>
    ''' Plans may have a Goal, such as creating a FactType. To reach that Goal, 'Steps' are generally required (e.g. Creation of a ValueType that 
    '''   hosts a Role in the FactType). The Steps of the Plan lead towards achieving a Goal. If Steps fail, then (generally) the Plan fails.
    '''   Steps may fail because the user has answered a Question of the Step in the negative, negating the successful conclusion of the Step, 
    '''   and negating the achievement of the Goal.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Plan

        Public [Step] As New List(Of Brain.Step)

        ''' <summary>
        ''' True if the Plan was aborted.
        ''' </summary>
        ''' <remarks></remarks>
        Public IsAborted As Boolean = False

        Public StepIndex As Integer = 1 'The index within the set of Steps that the Plan is up to (dereferenced as 'StepIndex - 1' for .Net)

        Private _Status As pcenumPlanStatus = pcenumPlanStatus.Active
        Public Property Status() As pcenumPlanStatus
            Get
                Return Me._Status
            End Get
            Set(ByVal value As pcenumPlanStatus)
                Me._Status = value
                If Me._Status = pcenumPlanStatus.AbortedByUser Then
                    Me.IsAborted = True
                End If
            End Set
        End Property

        Public Sub AddStep(ByVal arStep As Brain.Step)

            Me.Step.Add(arStep)

        End Sub


        Public Function GetIndexFirstUnresolvedStep() As Integer

            Dim liInd As Integer = 1
            Dim lrStep As Brain.Step

            For Each lrStep In Me.Step
                If lrStep.StepStatus = pcenumBrainPlanStepStatus.Unresolved Then
                    Return liInd
                End If
                liInd += 1
            Next

            Return Me.Step.Count

        End Function

        Public Function GetUltimateGoal() As pcenumActionType

            If Me.Step.Count = 0 Then
                Return Nothing
            Else
                Return Me.Step(Me.Step.Count - 1).ActionType
            End If

        End Function

        Public Function HasBeenSuccessfullyExecuted() As Boolean

            Dim liUnresolvedCount = Me.Step.FindAll(Function(x) x.StepStatus = pcenumBrainPlanStepStatus.Unresolved).Count

            Return liUnresolvedCount = 0

        End Function

        Public Sub IncrementStepIndex()

            Try
                If Me.StepIndex = Me.Step.Count Then
                    Throw New Exception("Tried to increment the Plan's StepIndex past the number of steps in the Plan.")
                End If

                Me.StepIndex += 1

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace
