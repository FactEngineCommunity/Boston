Imports MindFusion.Diagramming
Imports System.Drawing.Drawing2D


Public Class tORMDiagrammingShapeNode
    Inherits MindFusion.Diagramming.ShapeNode

    Public Sub New()

        MyBase.New()        

    End Sub

    Public Overrides Sub Draw(ByVal graphics As MindFusion.Drawing.IGraphics, ByVal options As MindFusion.Diagramming.RenderOptions)

        Try
            MyBase.Draw(graphics, options)

            If Me.Tag.Page.Form IsNot Nothing Then
                If Me.Tag.Page.Form.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.ORMSubtypeConnector Then

                    Dim pt1, pt2 As PointF

                    Dim liX, liY As Single
                    Dim liCentreX, liCentreY As Single

                    Dim lrPen As New System.Drawing.Pen(Color.Purple, 0.4)


                    If Me.Tag.Page.SelectedObject.Count > 0 Then

                        If Me.Tag.Id = Me.Tag.Page.SelectedObject(0).Id Then

                            liCentreX = Me.Bounds.X + (Me.Bounds.Width / 2)
                            liCentreY = (Me.Bounds.Y + (Me.Bounds.Height / 2))

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

                            '================================
                            'Put the ArrowHead on the line

                            lrPen.DashStyle = DashStyle.Solid
                            Dim capPath As GraphicsPath = New GraphicsPath()

                            capPath.AddLine(-3, -6, 0, 0)
                            capPath.AddLine(3, -6, 0, 0)

                            lrPen.CustomEndCap = New System.Drawing.Drawing2D.CustomLineCap(Nothing, capPath)

                            Dim loMousePoint As PointF = Me.Tag.Page.DiagramView.ClientToDoc(New Point(Me.Tag.Page.DiagramView.PointToClient(Control.MousePosition).X, Me.Tag.Page.DiagramView.PointToClient(Control.MousePosition).Y))
                            graphics.DrawLine(lrPen, pt1, loMousePoint)

                            Me.Tag.Page.Diagram.Invalidate()

                        End If

                    End If

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

End Class
