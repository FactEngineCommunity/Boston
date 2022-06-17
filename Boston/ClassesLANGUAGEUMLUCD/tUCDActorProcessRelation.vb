Imports System.Reflection

Namespace UCD
    Public Class ActorProcessRelation
        Inherits UML.ActorProcessRelation
        Implements FBM.iPageObject

        Public Shadows WithEvents CMMLActorProcessRelation As CMML.ActorProcessRelation

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arUMLModel As UML.Model, ByRef arActor As UCD.Actor, ByRef arProcess As UCD.Process)

            Me.UMLModel = arUMLModel
            Me.Actor = arActor
            Me.Process = arProcess

        End Sub

        Private Sub CMMLActorProcessRelation_RemovedFromModel() Handles CMMLActorProcessRelation.RemovedFromModel

            Try
                Call Me.RemoveFromPage()

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
