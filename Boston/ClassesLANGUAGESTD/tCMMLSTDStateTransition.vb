Imports System.Reflection
Imports MindFusion.Diagramming



Namespace CMML
    Public Class StateTransition
        Inherits FBM.FactInstance

        Public FromState As CMML.State
        Public ToState As CMML.State

        Public EventName As String = ""

        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.StateTransition

        Public Link As DiagramLink

        Public Sub New()

        End Sub

        Public Sub New(ByRef arFromState As CMML.State, ByRef arToState As CMML.State, ByVal asEventName As String)

            Me.FromState = arFromState
            Me.ToState = arToState
            Me.EventName = asEventName

        End Sub

        Public Sub DisplayAndAssociate()

            '========================================================================================
            'Create a Link on the Page for the StateTransition and between the corresponding States
            '----------------------------------------------                                    
            Try
                Dim loNode1 As MindFusion.Diagramming.ShapeNode = Me.FromState.Shape
                Dim loNode2 As MindFusion.Diagramming.ShapeNode = Me.ToState.Shape
                Dim loLink As New DiagramLink(Me.Page.Diagram, loNode1, loNode2)
                loLink.Locked = False
                loLink.Tag = Me
                Me.Link = loLink
                Me.Link.Visible = True
                Me.Link.ShadowColor = Color.White
                Me.Link.Pen.Width = 0.3
                Me.Link.Text = Me.EventName
                Me.Page.Diagram.Links.Add(loLink)

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

