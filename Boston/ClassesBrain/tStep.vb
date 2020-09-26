Namespace Brain

    Public Class [Step]
        Implements IEquatable(Of Brain.Step)

        ''' <summary>
        ''' The ordinal position/SequenceNumber of the Step within a Plan.
        ''' </summary>
        ''' <remarks></remarks>
        Public SequenceNumber As Integer = 1

        ''' <summary>
        ''' The type of action the Brain is attempting to achieve by this Step within a Plan.
        ''' </summary>
        ''' <remarks></remarks>
        Public ActionType As pcenumActionType

        Public FactTypeAttributes() As pcenumStepFactTypeAttributes

        ''' <summary>
        ''' The Question associated with the Step.
        '''   e.g. The Step may be to create an EntityType and which needs ratification by the User.
        ''' </summary>
        ''' <remarks></remarks>
        Public Question As tQuestion

        Public AbortPlanIfStepIsAborted As Boolean = False

        Public StepStatus As pcenumBrainPlanStepStatus = pcenumBrainPlanStepStatus.Unresolved

        Public AlternateActionType As pcenumActionType


        Public Sub New(ByVal arActionType As pcenumActionType,
                       ByVal arAbortPlanIfStepIsAborted As Boolean,
                       ByVal aiAlternateActionType As pcenumActionType,
                       ByRef aarModelObject As List(Of FBM.ModelObject),
                       ByVal ParamArray aaiFactTypeAttributes As pcenumStepFactTypeAttributes()
                       )

            Me.ActionType = arActionType
            Me.AbortPlanIfStepIsAborted = arAbortPlanIfStepIsAborted

            Me.FactTypeAttributes = aaiFactTypeAttributes
            Me.AlternateActionType = aiAlternateActionType

        End Sub

        Public Shadows Function Equals(ByVal other As [Step]) As Boolean Implements System.IEquatable(Of [Step]).Equals

            If (Me.SequenceNumber = other.SequenceNumber) And (Me.ActionType = other.ActionType) Then
                Return True
            Else
                Return False
            End If

        End Function

    End Class

End Namespace
