Imports System.Reflection

Namespace FBM.STM

    ''' <summary>
    ''' NB Each State is actually a ValueConstraint on the ValueType, but are stored redundently within the STDModel attribute of a ValueType.
    ''' </summary>
    Public Class State
        Implements IEquatable(Of FBM.STM.State)

        Public Model As STM.Model = Nothing

        Public Name As String = ""

        Public ValueType As FBM.ValueType = Nothing

        Public IsStart As Boolean = False
        Public StartStateFact As FBM.Fact = Nothing

        Public IsStop As Boolean = False

        Public Event IsStartStateChanged(abStartStateStatus As Boolean)
        Public Event IsEndStateChanged(abEndStateStatus As Boolean)
        Public Event NameChanged(asNewName As String)

        ''' <summary>
        ''' Parameterless new
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arSTModel As STM.Model, ByRef arValueType As FBM.ValueType, ByVal asStateName As String)

            Me.Model = arSTModel
            Me.ValueType = arValueType
            Me.Name = asStateName

        End Sub

        Public Shadows Function Equals(other As State) As Boolean Implements IEquatable(Of State).Equals
            Return (Me.Name = other.Name) And (Me.ValueType.Id = other.ValueType.Id)
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

        Public Sub setName(ByVal asNewName As String)

            Try
                Dim lsOldStateName = Me.Name
                Me.Name = asNewName

                'FBM Model level
                Me.ValueType.renameValueConstraint(lsOldStateName, asNewName)

                'CMML
                Call Me.Model.Model.changeCMMLStateName(Me, lsOldStateName)

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
