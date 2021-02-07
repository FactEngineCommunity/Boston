Imports System.Reflection
Imports MindFusion.Diagramming


Namespace STD
    Public Class EndStateTransition
        Inherits FBM.FactInstance

        Public ValueType As FBM.ValueType
        Public FromState As STD.State
        Public EndStateIndicator As STD.EndStateIndicator

        Public EventName As String = ""

        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.StateTransition

        Public Link As DiagramLink

        Public WithEvents STMEndStateTransition As FBM.STM.EndStateTransition 'The EndStateTransition as the STModel level of the FBM.Model.

        Public Sub New()

        End Sub

        Public Sub New(ByRef arFromState As STD.State,
                       ByRef arEndStateIndicator As STD.EndStateIndicator,
                       ByVal asEventName As String)

            Me.Page = arFromState.Page
            Me.FromState = arFromState
            Me.EndStateIndicator = arEndStateIndicator
            Me.EventName = asEventName

        End Sub

        Public Sub DisplayAndAssociate()

            '========================================================================================
            'Create a Link on the Page for the StateTransition and between the corresponding States
            '----------------------------------------------                                    
            Try
                Dim loNode1 As MindFusion.Diagramming.ShapeNode = Me.FromState.Shape
                Dim loNode2 As MindFusion.Diagramming.ShapeNode = Me.EndStateIndicator.Shape
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

        Private Sub STMEndStateTransition_EventNameChanged(asNewEventName As String) Handles STMEndStateTransition.EventNameChanged

            Me.Link.Text = asNewEventName

        End Sub

    End Class

End Namespace

