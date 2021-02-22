Imports System.Reflection
Imports Boston.FBM
Imports MindFusion.Diagramming


Namespace STD
    Public Class StartStateTransition
        Inherits FBM.FactInstance
        Implements FBM.iPageObject

        Public ValueType As FBM.ValueType
        Public FromState As STD.State
        Public StartStateIndicator As STD.StartStateIndicator

        Public EventName As String = ""

        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.StartStateTransition

        Public Link As DiagramLink

        Public WithEvents STMStartStateTransition As FBM.STM.StartStateTransition 'The StartStateTransition as the STModel level of the FBM.Model.

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arFromState As STD.State,
                       ByRef arStartStateIndicator As STD.StartStateIndicator,
                       ByVal asEventName As String)

            Me.Page = arFromState.Page
            Me.FromState = arFromState
            Me.StartStateIndicator = arStartStateIndicator
            Me.EventName = asEventName

        End Sub

        Private Property iPageObject_X As Integer Implements iPageObject.X
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Private Property iPageObject_Y As Integer Implements iPageObject.Y
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Sub DisplayAndAssociate()

            '========================================================================================
            'Create a Link on the Page for the StateTransition and between the corresponding States
            '----------------------------------------------                                    
            Try
                Dim loNode1 As MindFusion.Diagramming.ShapeNode = Me.StartStateIndicator.Shape
                Dim loNode2 As MindFusion.Diagramming.ShapeNode = Me.FromState.Shape

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

        Public Sub MouseDown() Implements iPageObject.MouseDown
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected
            Call Me.SetAppropriateColour()
        End Sub

        Public Sub Move(aiNewX As Integer, aiNewY As Integer, abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            If Me.FactInstance IsNot Nothing Then
                Me.FactInstance.X = aiNewX
                Me.FactInstance.Y = aiNewY
            End If

        End Sub

        Public Sub Moved() Implements iPageObject.Moved
            Throw New NotImplementedException()
        End Sub

        Public Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects
            Throw New NotImplementedException()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

            If IsSomething(Me.Link) Then
                If Me.Link.Selected Then
                    Me.Link.Pen.Color = Color.Blue
                Else
                    Me.Link.Pen.Color = Color.Black
                End If

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Invalidate()
                End If

            End If

        End Sub

        Private Sub STMStartStateTransition_EventNameChanged(asNewEventName As String) Handles STMStartStateTransition.EventNameChanged

            Me.Link.Text = asNewEventName

        End Sub

        Private Sub STMStartStateTransition_RemovedFromModel() Handles STMStartStateTransition.RemovedFromModel

            Me.Page.Diagram.Links.Remove(Me.Link)
            Me.Link.Dispose()

        End Sub

    End Class

End Namespace

