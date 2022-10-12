Imports System.Reflection

Namespace UML
    Public Class ActorProcessRelation
        Inherits FBM.FactInstance
        Implements FBM.iPageObject

        Public Actor As UML.Actor
        Public Process As UML.Process

        Public WithEvents UMLModel As UML.Model

        Public WithEvents _CMMLActorProcessRelation As CMML.ActorProcessRelation
        Public Overridable Property CMMLActorProcessRelation As CMML.ActorProcessRelation
            Get
                Return Me._CMMLActorProcessRelation
            End Get
            Set(value As CMML.ActorProcessRelation)
                Me._CMMLActorProcessRelation = value
            End Set
        End Property

        Public Link As MindFusion.Diagramming.DiagramLink

        Public Shadows Event RemovedFromModel()

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arUMLModel As UML.Model, ByRef arActor As UML.Actor, ByRef arProcess As UML.Process)

            Me.UMLModel = arUMLModel
            Me.Actor = arActor
            Me.Process = arProcess

        End Sub

        Public Shadows Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abDeleteAll As Boolean = False) As Boolean

            Try
                Call Me.Model.UML.RemoveActorProcessRelation(Me.CMMLActorProcessRelation)

                RaiseEvent RemovedFromModel()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub RemoveFromPage()

            Try
                Call Me.UMLModel.ActorToProcessParticipationRelationFTI.RemoveFact(Me.FactInstance)

                If Me.Page.Diagram IsNot Nothing And Me.Link IsNot Nothing Then
                    Call Me.Page.Diagram.Links.Remove(Me.Link)
                End If


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

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
