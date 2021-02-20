Imports System.Reflection

Namespace FBM.STM

    ''' <summary>
    ''' NB Each State is actually a ValueConstraint on the ValueType, but are stored redundently within the STDModel attribute of a ValueType.
    ''' </summary>
    Public Class State
        Implements IEquatable(Of FBM.STM.State)

        Public Model As STM.Model = Nothing

        Public Id As String

        Public Name As String = ""

        Public ValueType As FBM.ValueType = Nothing

        Public IsStart As Boolean = False
        Public StartStateFact As FBM.Fact = Nothing

        Public IsStop As Boolean = False

        Public Event FactChanged(ByRef arFact As FBM.Fact)
        Public Event IsStartStateChanged(abStartStateStatus As Boolean)
        Public Event IsEndStateChanged(abEndStateStatus As Boolean)
        Public Event NameChanged(asNewName As String)

        Public Fact As FBM.Fact 'The Fact that represents the State in the CMML/Core Model of the FBM Model.

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        Public Sub New()
            Me.Id = System.Guid.NewGuid.ToString
        End Sub

        Public Sub New(ByRef arSTModel As STM.Model,
                       ByVal asId As String,
                       ByRef arValueType As FBM.ValueType,
                       ByVal asStateName As String)

            Me.Id = asId
            Me.Model = arSTModel
            Me.ValueType = arValueType
            Me.Name = asStateName

        End Sub

        Public Shadows Function Equals(other As State) As Boolean Implements IEquatable(Of State).Equals

            Return Me.Id = other.Id
            ' Return (Me.Name = other.Name) And (Me.ValueType Is other.ValueType)

        End Function

        ''' <summary>
        ''' Sets the State as a StartState for the STM
        ''' </summary>
        Public Function makeStartState() As FBM.Fact

            Me.IsStart = True

            'CMML
            Me.StartStateFact = Me.Model.Model.addCMMLStartState(Me)

            RaiseEvent IsStartStateChanged(True)

            Return Me.StartStateFact

        End Function

        Public Sub setEndState(ByVal abEndStateStatus As Boolean)

            Me.IsStop = abEndStateStatus

        End Sub

        Public Sub setFact(ByRef arFact As FBM.Fact)

            Me.Fact = arFact

            RaiseEvent FactChanged(arFact)

        End Sub

        Public Sub setName(ByVal asNewName As String)

            Try
                Dim lsOldStateName = Me.Name

                '===================================================================================
                'States can belong to more than ValueType.
                Dim larDuplicateState = Me.Model.State.FindAll(Function(x) x.ValueType IsNot Me.ValueType And x.Name = Me.Name)

                Me.Name = asNewName

                'FBM Model level
                If Me.ValueType IsNot Nothing Then
                    Me.ValueType.renameValueConstraint(lsOldStateName, asNewName)
                End If

                'CMML
                Call Me.Model.Model.changeCMMLStateName(Me, lsOldStateName)

                For Each lrDuplicateState In larDuplicateState
                    'Need to create a new State in the CMML ElementHasElementType relation.
                    Call Me.Model.Model.createCMMLState(lrDuplicateState.ValueType, lsOldStateName)
                    Exit For
                Next

                RaiseEvent NameChanged(asNewName)

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
