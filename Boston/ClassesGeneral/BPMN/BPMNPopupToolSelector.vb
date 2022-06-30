Imports System.Reflection

Public Class BPMNPopupToolSelector

    Public Type As pcenumBPMNElementType
    Public Result As String

    Public mrCMMLModel As CMML.Model
    Public msLinkedProcessId As String
    Public moDiagram As MindFusion.Diagramming.Diagram

    Public mrPage As FBM.Page
    Public Node As MindFusion.Diagramming.WinForms.ControlNode
    Public AttachedToNode As MindFusion.Diagramming.ShapeNode

    Private Delegate Sub MyDelegate(ByRef arPage As FBM.Page, ByRef asInstructionType As String)

    Private Sub BPMNPopupToolSelector_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Public Sub SetupForm()

        Try
            Select Case Me.Type
                Case Is = pcenumBPMNElementType.Activity
                    Call Me.SetupForBPMNActivity()
            End Select

            Me.Size = New Size(Me.TableLayoutPanel.Width + 2, Me.TableLayoutPanel.Height + 2)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub SetupForBPMNActivity()

        Try
            Dim liRow, liCol As Integer

            For liInd = 1 To 9

                liRow = Math.Truncate(liInd / 3.1)
                liCol = Boston.returnIfTrue((liInd Mod 3) - 1 < 0, 3, (liInd Mod 3) - 1)

                Dim lrButton As New Button()
                lrButton.Text = ""
                lrButton.Size = New Size(26, 26)
                lrButton.ForeColor = Color.White
                AddHandler lrButton.Click, AddressOf Me.Button_Click
                lrButton.Dock = DockStyle.Fill
                lrButton.Margin = New Padding(0)
                lrButton.FlatAppearance.BorderColor = Color.White
                lrButton.FlatStyle = FlatStyle.Flat

                Select Case liInd
                    Case Is = 1
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.Event_End_None_Standard_25x25
                        lrButton.Tag = "EventEndNoneStandard"
                    Case Is = 2
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.Gateway_Exclusive_25x25
                        lrButton.Tag = "GatewayExclusive"
                    Case Is = 3
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.Task_25x25
                        lrButton.Tag = "Task"
                    Case Is = 4
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.Event_Intermediate_None_Throwing_25x25
                        lrButton.Tag = "EventIntermediateNoneThrowing"
                    Case Is = 5
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.Annotation_25x25
                        lrButton.Tag = "Annotation"
                    Case Is = 6
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.More_25x25
                        lrButton.Tag = "More"
                    Case Is = 7
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.ChangeType_25x25
                        lrButton.Tag = "ChangeType"
                    Case Is = 8
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.Bin_25x25
                        lrButton.Tag = "Bin"
                    Case Is = 9
                        lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.Connector_25x25
                        lrButton.Tag = "Connector"
                End Select

                Me.TableLayoutPanel.Controls.Add(lrButton, liCol, liRow)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs)

        Try
            Me.Result = CType(sender, Button).Tag

            If Me.Result = "ChangeType" Then
#Region "Element Changer"
                Try
                    'GoTo SkipPopup
                    Dim lrPopupToolElementChanger = New BPMNPopupToolElementChanger()
                    lrPopupToolElementChanger.mrCMMLModel = Me.mrCMMLModel
                    Dim liX, liY As Integer
                    Select Case Me.Type
                        Case Is = pcenumBPMNElementType.Activity
                            liX = Me.Node.Bounds.X
                            liY = Me.Node.Bounds.Y + Me.Node.Bounds.Height + 2
                    End Select
                    lrPopupToolElementChanger.moDiagram = Me.moDiagram
                    lrPopupToolElementChanger.mrPage = Me.mrPage
                    lrPopupToolElementChanger.Tag = lrPopupToolElementChanger
                    If Me.mrPage.Form.mrPopupToolElementChanger IsNot Nothing Then
                        Me.mrPage.Form.Diagram.Nodes.Remove(Me.mrPage.Form.mrPopupToolElementChanger.Node)
                    End If
                    Me.mrPage.Form.mrPopupToolElementChanger = lrPopupToolElementChanger
                    Dim lrControlNode As New MindFusion.Diagramming.WinForms.ControlNode(Me.mrPage.DiagramView, lrPopupToolElementChanger)
                    lrPopupToolElementChanger.Node = lrControlNode
                    Me.mrPage.Diagram.Nodes.Add(lrControlNode)
                    lrControlNode.AttachTo(Me.AttachedToNode, MindFusion.Diagramming.AttachToNode.TopCenter)
                    Call lrControlNode.Move(liX, liY)
SkipPopup:

                Catch ex As Exception
                    Dim lsMessage As String
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                End Try
#End Region
            Else
                Me.moDiagram.Nodes.Remove(Me.Node)

                Dim delegate1 As MyDelegate = New MyDelegate(AddressOf frmDiagramBPMNCollaboration.ProcessPopupToolSelector)
                delegate1(Me.mrPage, Me.Result)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class
