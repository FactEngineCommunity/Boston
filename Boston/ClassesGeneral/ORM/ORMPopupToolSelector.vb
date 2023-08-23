Imports System.Reflection

Public Class ORMPopupToolSelector

    Public Type As pcenumConceptType
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
            Me.Visible = False
            Select Case Me.Type
                Case Is = pcenumConceptType.EntityType
                    Call Me.SetupForEntityType()
                Case Is = pcenumConceptType.FactType
                    Call Me.SetupForFactType()
            End Select

            Me.Size = New Size(Me.TableLayoutPanel.Width + 2, Me.TableLayoutPanel.Height + 2)

            Me.Visible = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub SetupForEntityType()

        Try
            Dim liRow, liCol As Integer

            For liInd = 1 To 3

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
                lrButton.Image = Nothing
                lrButton.BackColor = Color.White

                Select Case liInd
                    Case Is = 1
                        lrButton.Image = My.Resources.ORMShapes.EntityType
                        lrButton.Tag = "EntityType"
                    Case Is = 2
                        lrButton.Image = My.Resources.ORMShapes.ValueTypePopupToolSelector
                        lrButton.Tag = "ValueType"
                    Case Is = 3
                        lrButton.Image = My.Resources.ORMPopupToolSelectorImages.Properties25x25
                        lrButton.Tag = "Properties"
                    Case Else
                        'CodeSafe
                        GoTo SkipButton
                End Select

                Me.TableLayoutPanel.Controls.Add(lrButton, liCol, liRow)
SkipButton:
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub SetupForFactType()

        Try

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

            '            If Me.Result = "ChangeType" Then
            '#Region "Element Changer"
            '                Try
            '                    'GoTo SkipPopup
            '                    Dim lrPopupToolElementChanger = New BPMNPopupToolElementChanger()
            '                    lrPopupToolElementChanger.mrCMMLModel = Me.mrCMMLModel
            '                    lrPopupToolElementChanger.Type = Me.Type
            '                    Dim liX, liY As Integer
            '                    Select Case Me.Type
            '                        Case Is = pcenumBPMNProcessType.Activity,
            '                                  pcenumBPMNProcessType.Gateway,
            '                                  pcenumBPMNProcessType.Event
            '                            liX = Me.Node.Bounds.X
            '                            liY = Me.Node.Bounds.Y + Me.Node.Bounds.Height + 2
            '                    End Select
            '                    lrPopupToolElementChanger.moDiagram = Me.moDiagram
            '                    lrPopupToolElementChanger.mrPage = Me.mrPage
            '                    lrPopupToolElementChanger.Tag = lrPopupToolElementChanger
            '                    If Me.mrPage.Form.mrPopupToolElementChanger IsNot Nothing Then
            '                        Me.mrPage.Diagram.Nodes.Remove(Me.mrPage.Form.mrPopupToolElementChanger.Node)
            '                    End If
            '                    Me.mrPage.Form.mrPopupToolElementChanger = lrPopupToolElementChanger
            '                    Dim lrControlNode As New MindFusion.Diagramming.WinForms.ControlNode(Me.mrPage.DiagramView, lrPopupToolElementChanger)
            '                    lrPopupToolElementChanger.Node = lrControlNode
            '                    Me.mrPage.Diagram.Nodes.Add(lrControlNode)
            '                    lrControlNode.AttachTo(Me.AttachedToNode, MindFusion.Diagramming.AttachToNode.TopCenter)
            '                    Call lrControlNode.Move(liX, liY)
            'SkipPopup:

            '                Catch ex As Exception
            '                    Dim lsMessage As String
            '                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            '                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            '                    lsMessage &= vbCrLf & vbCrLf & ex.Message
            '                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            '                End Try
            '#End Region
            '            Else
            Me.moDiagram.Nodes.Remove(Me.Node)

            Dim delegate1 As MyDelegate = New MyDelegate(AddressOf frmDiagramORM.ProcessPopupToolSelector)
            delegate1(Me.mrPage, Me.Result)
            'End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class
