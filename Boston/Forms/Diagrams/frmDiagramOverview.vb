Imports System.Reflection

Public Class frmDiagramOverview

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub SetDocument(ByVal aMFView_view As MindFusion.Diagramming.WinForms.DiagramView)

        Overview.DiagramView = aMFView_view
        Overview.FitAll = False
        Overview.FitAll = True

    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click

        Try
            If IsSomething(prApplication.WorkingPage) Then
                Select Case prApplication.WorkingPage.Language
                    Case Is = pcenumLanguage.ORMModel,
                              pcenumLanguage.EntityRelationshipDiagram,
                              pcenumLanguage.PropertyGraphSchema

                        Call Me.SetDocument(prApplication.WorkingPage.DiagramView)
                End Select
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