Imports System.Reflection
Imports MindFusion.Diagramming
Imports System.Drawing.Drawing2D

Namespace BPMN
    Public Class ShapeNode
        Inherits MindFusion.Diagramming.ShapeNode


        Public BPMNElement As BPMN.iPageObject

        Public Overrides Sub Draw(ByVal graphics As MindFusion.Drawing.IGraphics, ByVal options As MindFusion.Diagramming.RenderOptions)
            MyBase.Draw(graphics, options)

            Dim liX, liY As Single
            Dim liCentreX, liCentreY As Single
            Dim pt1 As PointF
            Dim pt2 As PointF

            Try
                Dim lrPen As New System.Drawing.Pen(Color.Purple, 0.4)

                liCentreX = Me.Bounds.X + (Me.Bounds.Width / 2)
                liCentreY = (Me.Bounds.Y + (Me.Bounds.Height / 2))

                If Me.BPMNElement.IsCreatingLink Then

                    liX = Math.Abs(liCentreX - pt2.X)
                    liY = Math.Abs(liCentreY - pt2.Y)

                    Dim liRSquared As Integer = (liX ^ 2) + (liY ^ 2)
                    Dim liBigR As Single = Math.Sqrt(liRSquared)

                    Dim liProportion As Single = (liBigR / 6) * Math.PI

                    liX = liCentreX - (Math.Sign(liCentreX - pt2.X) * (liX / liProportion))
                    liY = liCentreY - (Math.Sign(liCentreY - pt2.Y) * (liY / liProportion))

                    pt1 = New PointF(liX, liY)

                    lrPen.DashStyle = DashStyle.Custom
                    lrPen.DashPattern = New Single() {3, 2, 3, 2}

                    '===============================
                    'Put the ArrowHead on the line
                    lrPen.DashStyle = DashStyle.Solid
                    Dim capPath As GraphicsPath = New GraphicsPath()

                    capPath.AddLine(-3, -6, 0, 0)
                    capPath.AddLine(3, -6, 0, 0)

                    lrPen.CustomEndCap = New System.Drawing.Drawing2D.CustomLineCap(Nothing, capPath)


                    Dim loMousePoint As PointF = Me.BPMNElement.BPMNPage.DiagramView.ClientToDoc(New Point(Me.BPMNElement.BPMNPage.DiagramView.PointToClient(Control.MousePosition).X, Me.BPMNElement.BPMNPage.DiagramView.PointToClient(Control.MousePosition).Y))
                    graphics.DrawLine(lrPen, pt1, loMousePoint)

                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace
