Imports System.Reflection

Public Class BPMNPopupToolSelector

    Public Type As pcenumBPMNElementType
    Public Result As CMML.Element

    Public mrCMMLModel As CMML.Model
    Public msLinkedProcessId As String
    Public moDiagram As MindFusion.Diagramming.Diagram

    Public mrPage As FBM.Page
    Public Node As MindFusion.Diagramming.WinForms.ControlNode

    Private Delegate Sub MyDelegate(ByRef arPage As FBM.Page, ByRef lrCMMLElement As CMML.Element)

    Private Sub BPMNPopupToolSelector_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Public Sub SetupForm()

        Try
            Select Case Me.Type
                Case Is = pcenumBPMNElementType.Activity
                    Call Me.SetupForBPMNActivity()
            End Select

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
            Dim lrButton As New Button()
            lrButton.Image = My.Resources.BPMNPopupToolSelectorImages.Task_25x25
            lrButton.SetBounds(0, 0, 28, 28)
            lrButton.ForeColor = Color.White
            AddHandler lrButton.Click, AddressOf Me.Button_Click
            Me.TableLayoutPanel.Controls.Add(lrButton, 0, 0)

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
            Dim lrCMMLElement As CMML.Element = Nothing

            Select Case Me.Type
                Case Is = pcenumBPMNElementType.Activity
                    Dim lrCMMLProcess As CMML.Process = New CMML.Process(Me.mrCMMLModel, System.Guid.NewGuid.ToString, "New BPMN Activity")
                    lrCMMLProcess.Text = Me.mrCMMLModel.CreateUniqueProcessText(lrCMMLProcess)
                    lrCMMLElement = lrCMMLProcess
            End Select


            Me.Result = lrCMMLElement

            Me.moDiagram.Nodes.Remove(Me.Node)

            Dim delegate1 As MyDelegate = New MyDelegate(AddressOf frmDiagramBPMNCollaboration.ProcessPopupToolSelector)
            delegate1(Me.mrPage, lrCMMLElement)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class
