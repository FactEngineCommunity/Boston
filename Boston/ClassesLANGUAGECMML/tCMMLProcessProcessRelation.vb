Imports System.Reflection

Namespace CMML
    Public Class ProcessProcessRelation

        Public CMMLModel As CMML.Model

        Public Process1 As CMML.Process
        Public Process2 As CMML.Process

        ''' <summary>
        ''' The Fact that stores this ProcessProcessRelation at the Model/CMML level (I.e. Not the Page level, the Model level)
        ''' </summary>
        Public Fact As FBM.Fact

        Public Event RemovedFromModel()

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arCMMLModel As CMML.Model, ByRef arProcess1 As CMML.Process, ByRef arProcess2 As CMML.Process)

            Me.CMMLModel = arCMMLModel
            Me.Process1 = arProcess1
            Me.Process2 = arProcess2

        End Sub

        Public Sub RemoveFromModel()

            Try
                Call Me.CMMLModel.RemoveProcessProcessRelation(Me)

                RaiseEvent RemovedFromModel()

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
