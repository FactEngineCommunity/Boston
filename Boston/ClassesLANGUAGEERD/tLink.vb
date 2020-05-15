Imports MindFusion.Diagramming
Imports System.Drawing.Drawing2D

Namespace ERD

    ''' <summary>
    ''' NB This class is used for drawing the ERD Link on a Diagram.
    '''   See ERD.Link for the Class that is used to store information about a relationship link in an ER Diagram.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class tERDLink
        Inherits MindFusion.Diagramming.DiagramLink

        ''' <summary>
        ''' The ERD Link that stores the relevant information required to draw this Link.
        '''   e.g. ERDLink.Relation stores the 'Multiplicity' and 'Mandatory' information required to draw this link.
        ''' </summary>
        ''' <remarks></remarks>
        Public ERDLink As ERD.Link

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '----------------------------------------------------------------------------------------------------
            'Parameterless New + call to MyBase.New required for MindFusion.Diagramming.DiagramLink
            ' NB Use the New with argument Mindfusion.Diagramming.Diagram so the link has a Diagram to draw on.
            '----------------------------------------------------------------------------------------------------
            MyBase.new(Nothing)

        End Sub

        ''' <summary>
        ''' New that sets the Diagram for the Link to draw on.
        ''' </summary>
        ''' <param name="asDiagram"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef asDiagram As MindFusion.Diagramming.Diagram)
            '----------------------------------------------------------
            'Must set the Mindfusion.Diagramming.Diagram for the Link
            '----------------------------------------------------------
            MyBase.New(asDiagram)

            Me.SnapToNodeBorder = True
            Me.Dynamic = True

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

            Dim pt1 As PointF
            Dim pt2 As PointF

            Dim liLastIndex As Integer = Me.ControlPoints.Count - 1

            '-------------------------
            'Must set Cascading here
            '-------------------------
            Me.Style = LinkStyle.Cascading

            '---------------------------------------------------------
            'Used for straight horizontal/vertical lines, autoRouted
            '---------------------------------------------------------
            If Me.SegmentCount <= 2 Then
                Me.SegmentCount = 3
            End If

            If Me.ControlPoints.Count >= 2 Then
                If Me.Origin Is Me.Destination Then
                    Me.SegmentCount = 3                    
                    liLastIndex = Me.ControlPoints.Count - 1
                    If (Me.ControlPoints(0).X = Me.ControlPoints(liLastIndex).X) And (Me.ControlPoints(0).Y = Me.ControlPoints(liLastIndex).Y) Then
                        If Math.Abs(Me.ControlPoints(0).Y - Me.Origin.Bounds.Y) < 0.5 Then
                            Me.ControlPoints(liLastIndex) = New PointF(Me.Bounds.X - 5, Me.Origin.Bounds.Top)
                        End If
                    End If
                    If (Me.ControlPoints(0).Y = Me.ControlPoints(liLastIndex).Y) Then
                        If Me.ControlPoints.Count > 2 Then
                            Me.ControlPoints(1) = New PointF(Me.ControlPoints(0).X, Me.Origin.Bounds.Top - 17)
                        End If
                        If Me.ControlPoints.Count > 3 Then
                            Me.ControlPoints(2) = New PointF(Me.ControlPoints(liLastIndex).X, Me.Origin.Bounds.Top - 17)
                        End If
                    End If
                End If

            End If

            '-----------------------------------------------------------------------------------------
            'Make visible at least the first segment in straight horizontal/veritcle autoRouted link
            '-----------------------------------------------------------------------------------------
            If Me.Bounds.Bottom = Me.Bounds.Top Then
                If Me.ControlPoints.Count >= 2 Then
                    If Me.ControlPoints(1).X = Me.Bounds.Left + (Me.Bounds.Right - Me.Bounds.Left) / 2 Then
                        '----------------------------------------------------------------------------------------
                        'Don't call UpdateFromPoints otherwise it'll call this method again in an infinite loop
                        '----------------------------------------------------------------------------------------
                    Else
                        Me.ControlPoints(1) = New PointF(Me.Bounds.Left + (Me.Bounds.Right - Me.Bounds.Left) / 2, Me.Bounds.Top)
                        'Me.UpdateFromPoints()
                    End If
                End If
            ElseIf Me.Bounds.Left = Me.Bounds.Right Then
                If Me.ControlPoints.Count >= 2 Then
                    Me.ControlPoints(1) = New PointF(Me.Bounds.Left, Me.Bounds.Top + (Me.Bounds.Bottom - Me.Bounds.Top) / 2)
                    'Me.UpdateFromPoints()
                End If
            End If

            Dim lrPen As New System.Drawing.Pen(Me.ERDLink.Color, 0.03)

            '-----------------------------------------
            'Dash/Solid for the appropriate segments
            '-----------------------------------------
            Dim liInd As Integer
            If Me.ControlPoints.Count >= 2 Then
                For liInd = 0 To Me.ControlPoints.Count - 2

                    pt1 = New PointF(Me.ControlPoints(liInd).X, Me.ControlPoints(liInd).Y)
                    pt2 = New PointF(Me.ControlPoints(liInd + 1).X, Me.ControlPoints(liInd + 1).Y)

                    lrPen = New System.Drawing.Pen(Me.ERDLink.Color, 0.2)

                    Select Case liInd
                        Case Is < (Me.ControlPoints.Count / 2)
                            If Me.ERDLink.Relation.OriginMandatory Then
                                lrPen.DashStyle = DashStyle.Solid
                            Else
                                lrPen.DashStyle = DashStyle.Custom
                                lrPen.DashPattern = New Single() {5, 3, 3, 3}
                            End If
                        Case Else
                            If Me.ERDLink.Relation.DestinationMandatory Then
                                lrPen.DashStyle = DashStyle.Solid
                            Else
                                lrPen.DashStyle = DashStyle.Custom
                                lrPen.DashPattern = New Single() {5, 3, 3, 3}
                            End If
                    End Select

                    graphics.DrawLine(lrPen, pt1, pt2)

                Next
            End If

            '===================================
            'Draw the Reading for the Relation
            '===================================
            Dim x As Single
            Dim y As Single
            Dim lsOriginPredicate As String = Me.ERDLink.Relation.OriginPredicate
            Dim lsDestinationPredicate As String = Me.ERDLink.Relation.DestinationPredicate
            Dim drawFont As New System.Drawing.Font("Arial", 6.5)
            Dim drawBrush As New System.Drawing.SolidBrush(Me.ERDLink.Color)
            Dim drawFormat As New System.Drawing.StringFormat
            Dim StringSize As New SizeF

            StringSize = graphics.MeasureString(lsOriginPredicate, drawFont, 1000, System.Drawing.StringFormat.GenericDefault)

            liLastIndex = Me.ControlPoints.Count - 1
            '---------------
            '1st Predicate
            '---------------
            If Me.ControlPoints(1).X > Me.ControlPoints(0).X Then
                x = Me.ControlPoints(0).X + 1
            Else
                x = Me.ControlPoints(0).X - (StringSize.Width + 1)
            End If

            If Me.ControlPoints(1).Y > Me.ControlPoints(0).Y Then
                y = Me.ControlPoints(0).Y + 5
            Else
                y = Me.ControlPoints(0).Y - 5
            End If

            If Me.Origin Is Me.Destination Then
                drawFormat.FormatFlags = StringFormatFlags.DirectionVertical
                x = Me.ControlPoints(0).X - 4
                y = Me.ControlPoints(0).Y - 15
            Else
                drawFormat.FormatFlags = Nothing
            End If

            graphics.DrawString(lsOriginPredicate, drawFont, drawBrush, x, y, drawFormat)

            '--------------
            '2nd Predicate
            '--------------
            StringSize = graphics.MeasureString(lsDestinationPredicate, drawFont, 1000, System.Drawing.StringFormat.GenericDefault)
            If Me.ControlPoints(Me.ControlPoints.Count - 2).X < Me.ControlPoints(Me.ControlPoints.Count - 1).X Then
                x = Me.ControlPoints(Me.ControlPoints.Count - 1).X - (StringSize.Width + 1)
            Else
                x = Me.ControlPoints(Me.ControlPoints.Count - 1).X
            End If

            y = Me.ControlPoints(Me.ControlPoints.Count - 1).Y

            If Me.ControlPoints(Me.ControlPoints.Count - 2).Y < Me.ControlPoints(Me.ControlPoints.Count - 1).Y Then
                y -= 5
            Else
                y += 2
            End If

            If Me.Origin Is Me.Destination Then
                drawFormat.FormatFlags = StringFormatFlags.DirectionVertical
                x = Me.ControlPoints(liLastIndex).X - 4
                y = Me.ControlPoints(liLastIndex).Y - 15
            Else
                drawFormat.FormatFlags = Nothing
            End If

            graphics.DrawString(lsDestinationPredicate, drawFont, drawBrush, x, y, drawFormat)

            drawFont.Dispose()
            drawBrush.Dispose()

            '===========================================
            'Draw the Link end Shapes (Crows Foot etc)
            '===========================================
            If Me.ERDLink.Relation.OriginMultiplicity = pcenumCMMLMultiplicity.Many Then
                Dim a As PointF = Me.ControlPoints(0)
                Dim b As PointF = Me.ControlPoints(1)
                Dim an As Double = 0, r = 0

                If Me.ControlPoints(0).Y = Me.ControlPoints(1).Y Then
                    an = 0
                Else
                    an = 90
                End If

                lrPen = New System.Drawing.Pen(Me.ERDLink.Color, 0.2)

                ' Find two points around the origin control point
                Dim lsglOffset As Single = 0
                If Me.ControlPoints(0).X <= Me.ControlPoints(1).X Then
                    lsglOffset = 2.7
                Else
                    lsglOffset = -2.7
                End If

                If Me.ControlPoints(0).Y > Me.ControlPoints(1).Y Then
                    lsglOffset = 2.7
                ElseIf Me.ControlPoints(0).Y < Me.ControlPoints(1).Y Then
                    lsglOffset = -2.7
                End If

                Conversion.PolarToDekart(a, an, lsglOffset, pt1)
                Conversion.PolarToDekart(a, an + 90, 2, pt2)
                graphics.DrawLine(lrPen, pt1, pt2)

                Conversion.PolarToDekart(a, an, lsglOffset, pt1)
                Conversion.PolarToDekart(a, an - 90, 2, pt2)
                graphics.DrawLine(lrPen, pt1, pt2)
            End If

            If Me.ERDLink.Relation.DestinationMultiplicity = pcenumCMMLMultiplicity.Many Then
                Dim a As PointF = Me.ControlPoints(Me.ControlPoints.Count - 1)
                Dim an As Double = 0, r = 0

                lrPen = New System.Drawing.Pen(Me.ERDLink.Color, 0.2)

                ' Find two points around the origin control point
                Conversion.PolarToDekart(a, an, -2.7, pt1)
                Conversion.PolarToDekart(a, an + 90, 2, pt2)
                graphics.DrawLine(lrPen, pt1, pt2)

                Conversion.PolarToDekart(a, an, -2.7, pt1)
                Conversion.PolarToDekart(a, an - 90, 2, pt2)
                graphics.DrawLine(lrPen, pt1, pt2)
            End If

            If Me.ERDLink.Relation.OriginContributesToPrimaryKey Then
                Dim a As PointF = Me.ControlPoints(0)
                Dim b As PointF = Me.ControlPoints(1)
                Dim an As Double = 0, r = 0

                lrPen = New System.Drawing.Pen(Me.ERDLink.Color, 0.2)

                Conversion.DekartToPolar(a, b, an, r)
                Conversion.PolarToDekart(a, an + 25, 3.0F, pt1)
                Conversion.PolarToDekart(a, an - 25, 3.0F, pt2)
                graphics.DrawLine(lrPen, pt1, pt2)
            End If

        End Sub

    End Class
End Namespace