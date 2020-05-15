Imports MindFusion.Diagramming
Imports System.Drawing.Drawing2D
Imports System.Reflection

Namespace FBM

    ''' <summary>
    ''' NB This class is used for drawing the ERD Link on a Diagram.
    '''   See ERD.Link for the Class that is used to store information about a relationship link in an ER Diagram.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RoleConstraintShape
        Inherits MindFusion.Diagramming.ShapeNode

        ''' <summary>
        ''' The RoleConstraintRole that stores the data for this link (i.e. e.g. The SubtypingConstraint that the link joins to).
        ''' </summary>
        ''' <remarks></remarks>
        Public RoleConstraint As FBM.RoleConstraintInstance

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '----------------------------------------------------------------------------------------------------
            'Parameterless New + call to MyBase.New required for MindFusion.Diagramming.DiagramLink
            ' NB Use the New with argument Mindfusion.Diagramming.Diagram so the link has a Diagram to draw on.
            '----------------------------------------------------------------------------------------------------
            MyBase.new()

        End Sub

        ''' <summary>
        ''' New that sets the Diagram for the Link to draw on.
        ''' </summary>
        ''' <param name="aoDiagram"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef aoDiagram As MindFusion.Diagramming.Diagram, ByRef arRoleConstraintInstance As FBM.RoleConstraintInstance)
            '----------------------------------------------------------
            'Must set the Mindfusion.Diagramming.Diagram for the Link
            '----------------------------------------------------------
            MyBase.New(aoDiagram)
            Me.RoleConstraint = arRoleConstraintInstance

        End Sub

        ''' <summary>
        ''' Overrides the default Draw method for a MindFusion.Diagramming.DiagramLink
        ''' Custom drawing for ERDLinks
        ''' </summary>
        ''' <param name="graphics"></param>
        ''' <param name="options"></param>
        ''' <remarks>Relies on Diagram.RouteAllLinks in Link/Node.Modified
        ''' - Diagram.LinkCustomDraw set to Full
        ''' - Form level must handle Diagram.DrawLink for other types of Links
        ''' - method handling Diagram.DrawLink must escape if e.Link is of this type (otherwise will double-up drawing of link).</remarks>
        Public Overrides Sub Draw(ByVal graphics As MindFusion.Drawing.IGraphics, ByVal options As MindFusion.Diagramming.RenderOptions)
            'MyBase.Draw(graphics, options)

            Try

                Dim pt1 As PointF
                Dim pt2 As PointF

                Call MyBase.Draw(graphics, options)

                Dim lrPen As New System.Drawing.Pen(Color.Purple, 0.4)

                'pt1 = New PointF(Me.Bounds.X + (Me.Bounds.Width / 2), Me.Bounds.Y + (Me.Bounds.Height / 2))

                'pt2 = New PointF(40, 40)

                'lrPen = New System.Drawing.Pen(Color.Purple, 0.2)

                Dim liX, liY As Single
                Dim liCentreX, liCentreY As Single

                liCentreX = Me.Bounds.X + (Me.Bounds.Width / 2)
                liCentreY = (Me.Bounds.Y + (Me.Bounds.Height / 2))

                'liX = Math.Abs(liCentreX - 40)
                'liY = Math.Abs(liCentreY - 40)

                'Dim liRSquared As Integer = (liX ^ 2) + (liY ^ 2)
                'Dim liBigR As Single = Math.Sqrt(liRSquared)

                'Dim liProportion As Single = (liBigR / 6) * Math.PI

                'liX = liCentreX - (Math.Sign(liCentreX - 40) * (liX / liProportion))
                'liY = liCentreY - (Math.Sign(liCentreY - 40) * (liY / liProportion))

                'pt1 = New PointF(liX, liY)

                graphics.DrawLine(lrPen, pt1, pt2)

                If Me.RoleConstraint.JoinsSubtypingRelationship Then
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

                    For Each lrRoleConstraintRoleInstance In Me.RoleConstraint.RoleConstraintRole

                        If lrRoleConstraintRoleInstance.SubtypeConstraintInstance IsNot Nothing Then
                            liX = (lrRoleConstraintRoleInstance.SubtypeConstraintInstance.Link.Bounds.Left + _
                                  lrRoleConstraintRoleInstance.SubtypeConstraintInstance.Link.Bounds.Right) / 2

                            liY = (lrRoleConstraintRoleInstance.SubtypeConstraintInstance.Link.Bounds.Top + _
                                  lrRoleConstraintRoleInstance.SubtypeConstraintInstance.Link.Bounds.Bottom) / 2

                            pt2 = New PointF(liX, liY)

                            '================================================

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

                            graphics.DrawLine(lrPen, pt1, pt2)
                        End If

                    Next
                Else
                    '--------------------------------------------------------------------------------------------
                    '20161214-VM-The following code draws a dashed line that follows the Mouse on the Page.
                    '  Relies on the frmDiagramORM.DiagramView.MouseMove event handler to call frmDiagramORM.DiagramView.Invalidate
                    '  to continually update the screen, otherwise the line will only be drawn when the user clicks on the DiagramView.
                    '  * Use this code when creating Arguments for a RoleConstraint.
                    '--------------------------------------------------------------------------------------------
                    If Me.RoleConstraint.CreatingArgument Then

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

                        '====================================================================
                        'Put the ArrowHead on the line if the Argument.SequenceNr = 2
                        If Me.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.SubsetConstraint _
                            And Me.RoleConstraint.CurrentArgument.SequenceNr = 2 Then
                            lrPen.DashStyle = DashStyle.Solid
                            Dim capPath As GraphicsPath = New GraphicsPath()

                            capPath.AddLine(-3, -6, 0, 0)
                            capPath.AddLine(3, -6, 0, 0)

                            lrPen.CustomEndCap = New System.Drawing.Drawing2D.CustomLineCap(Nothing, capPath)
                        End If

                        Dim loMousePoint As PointF = Me.RoleConstraint.Page.DiagramView.ClientToDoc(New Point(Me.RoleConstraint.Page.DiagramView.PointToClient(Control.MousePosition).X, Me.RoleConstraint.Page.DiagramView.PointToClient(Control.MousePosition).Y))
                        graphics.DrawLine(lrPen, pt1, loMousePoint)

                    End If

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