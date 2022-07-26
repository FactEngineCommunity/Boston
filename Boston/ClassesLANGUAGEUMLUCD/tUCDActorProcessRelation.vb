Imports System.Reflection

Namespace UCD
    Public Class ActorProcessRelation
        Inherits UML.ActorProcessRelation
        Implements FBM.iPageObject

        Public WithEvents _CMMLActorProcessRelation As CMML.ActorProcessRelation

        Public Overrides Property CMMLActorProcessRelation As CMML.ActorProcessRelation
            Get
                Return Me._CMMLActorProcessRelation
            End Get
            Set(value As CMML.ActorProcessRelation)
                Me._CMMLActorProcessRelation = value
            End Set
        End Property

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

        Private Sub CMMLActorProcessRelation_RemovedFromModel() Handles _CMMLActorProcessRelation.RemovedFromModel

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
