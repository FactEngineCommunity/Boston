Imports System.Reflection

Namespace CMML
    Public Class ActorProcessRelation

        Public Actor As CMML.Actor
        Public Process As CMML.Process

        Public WithEvents CMMLModel As CMML.Model

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

        Public Sub New(ByRef arCMMLModel As CMML.Model, ByRef arActor As CMML.Actor, ByRef arProcess As CMML.Process)

            Me.CMMLModel = arCMMLModel
            Me.Actor = arActor
            Me.Process = arProcess

        End Sub

        Public Sub RemoveFromModel()

            Try
                Call Me.CMMLModel.RemoveActorProcessRelation(Me)

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
