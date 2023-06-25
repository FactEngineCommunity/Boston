Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Class RoundedButton
    Inherits Button

    Private b_radius As Integer = 50
    Private b_width As Single = 1.75F
    Private b_color As Color = Color.Blue
    Private b_over_color, b_down_color As Color
    Private b_over_width As Single = 0
    Private b_down_width As Single = 0
    Public Property IsMouseOver As Boolean
    Private Property IsMouseDown As Boolean

    <Category("Border"), DisplayName("Border Width")>
    Public Property BorderWidth As Single
        Get
            Return b_width
        End Get
        Set(ByVal value As Single)
            If b_width = value Then Return
            b_width = value
            Invalidate()
        End Set
    End Property

    <Category("Border"), DisplayName("Border Over Width")>
    Public Property BorderOverWidth As Single
        Get
            Return b_over_width
        End Get
        Set(ByVal value As Single)
            If b_over_width = value Then Return
            b_over_width = value
            Invalidate()
        End Set
    End Property

    <Category("Border"), DisplayName("Border Down Width")>
    Public Property BorderDownWidth As Single
        Get
            Return b_down_width
        End Get
        Set(ByVal value As Single)
            If b_down_width = value Then Return
            b_down_width = value
            Invalidate()
        End Set
    End Property

    <Category("Border"), DisplayName("Border Color")>
    Public Property BorderColor As Color
        Get
            Return b_color
        End Get
        Set(ByVal value As Color)
            If b_color = value Then Return
            b_color = value
            Invalidate()
        End Set
    End Property

    <Category("Border"), DisplayName("Border Over Color")>
    Public Property BorderOverColor As Color
        Get
            Return b_over_color
        End Get
        Set(ByVal value As Color)
            If b_over_color = value Then Return
            b_over_color = value
            Invalidate()
        End Set
    End Property

    <Category("Border"), DisplayName("Border Down Color")>
    Public Property BorderDownColor As Color
        Get
            Return b_down_color
        End Get
        Set(ByVal value As Color)
            If b_down_color = value Then Return
            b_down_color = value
            Invalidate()
        End Set
    End Property

    <Category("Border"), DisplayName("Border Radius")>
    Public Property BorderRadius As Integer
        Get
            Return b_radius
        End Get
        Set(ByVal value As Integer)
            If b_radius = value Then Return
            b_radius = value
            Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Parameterless Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    Public Sub New(ByRef arBackColor As Drawing.Color, ByRef arBorderColor As Drawing.Color)

        Me.BackColor = arBackColor
        Me.BorderColor = arBorderColor
        Me.ForeColor = Color.DarkGray

    End Sub

    Private Function GetRoundPath(ByVal Rect As RectangleF, ByVal radius As Integer, ByVal Optional width As Single = 0) As GraphicsPath
        radius = CInt(Math.Max((Math.Min(radius, Math.Min(Rect.Width, Rect.Height)) - width), 1))
        Dim r2 As Single = radius / 2.0F
        Dim w2 As Single = width / 2.0F
        Dim GraphPath As GraphicsPath = New GraphicsPath()
        GraphPath.AddArc(Rect.X + w2, Rect.Y + w2, radius, radius, 180, 90)
        GraphPath.AddArc(Rect.X + Rect.Width - radius - w2, Rect.Y + w2, radius, radius, 270, 90)
        GraphPath.AddArc(Rect.X + Rect.Width - w2 - radius, Rect.Y + Rect.Height - w2 - radius, radius, radius, 0, 90)
        GraphPath.AddArc(Rect.X + w2, Rect.Y - w2 + Rect.Height - radius, radius, radius, 90, 90)
        GraphPath.AddLine(Rect.X + w2, Rect.Y + Rect.Height - r2 - w2, Rect.X + w2, Rect.Y + r2 + w2)
        Return GraphPath
    End Function

    Private Sub DrawText(ByVal g As Graphics, ByVal Rect As RectangleF)
        Dim r2 As Single = (BorderRadius / 2.0F)
        Dim w2 As Single = BorderWidth / 2.0F
        Dim point As Point = New Point()
        Dim format As StringFormat = New StringFormat()

        Select Case TextAlign
            Case ContentAlignment.TopLeft
                point.X = CInt((Rect.X + r2 / 2 + w2 + Padding.Left))
                point.Y = CInt((Rect.Y + r2 / 2 + w2 + Padding.Top))
                format.LineAlignment = StringAlignment.Center
            Case ContentAlignment.TopCenter
                point.X = CInt((Rect.X + Rect.Width / 2.0F))
                point.Y = CInt((Rect.Y + r2 / 2 + w2 + Padding.Top))
                format.LineAlignment = StringAlignment.Center
                format.Alignment = StringAlignment.Center
            Case ContentAlignment.TopRight
                point.X = CInt((Rect.X + Rect.Width - r2 / 2 - w2 - Padding.Right))
                point.Y = CInt((Rect.Y + r2 / 2 + w2 + Padding.Top))
                format.LineAlignment = StringAlignment.Center
                format.Alignment = StringAlignment.Far
            Case ContentAlignment.MiddleLeft
                point.X = CInt((Rect.X + r2 / 2 + w2 + Padding.Left))
                point.Y = CInt((Rect.Y + Rect.Height / 2))
                format.LineAlignment = StringAlignment.Center
            Case ContentAlignment.MiddleCenter
                point.X = CInt((Rect.X + Rect.Width / 2))
                point.Y = CInt((Rect.Y + Rect.Height / 2))
                format.LineAlignment = StringAlignment.Center
                format.Alignment = StringAlignment.Center
            Case ContentAlignment.MiddleRight
                point.X = CInt((Rect.X + Rect.Width - r2 / 2 - w2 - Padding.Right))
                point.Y = CInt((Rect.Y + Rect.Height / 2))
                format.LineAlignment = StringAlignment.Center
                format.Alignment = StringAlignment.Far
            Case ContentAlignment.BottomLeft
                point.X = CInt((Rect.X + r2 / 2 + w2 + Padding.Left))
                point.Y = CInt((Rect.Y + Rect.Height - r2 / 2 - w2 - Padding.Bottom))
                format.LineAlignment = StringAlignment.Center
            Case ContentAlignment.BottomCenter
                point.X = CInt((Rect.X + Rect.Width / 2))
                point.Y = CInt((Rect.Y + Rect.Height - r2 / 2 - w2 - Padding.Bottom))
                format.LineAlignment = StringAlignment.Center
                format.Alignment = StringAlignment.Center
            Case ContentAlignment.BottomRight
                point.X = CInt((Rect.X + Rect.Width - r2 / 2 - w2 - Padding.Right))
                point.Y = CInt((Rect.Y + Rect.Height - r2 / 2 - w2 - Padding.Bottom))
                format.LineAlignment = StringAlignment.Center
                format.Alignment = StringAlignment.Far
            Case Else
        End Select

        Using brush As Brush = New SolidBrush(ForeColor)
            g.DrawString(Text, New Font("Arial", 9), brush, point, format)
        End Using
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality
        Dim Rect As RectangleF = New RectangleF(0, 0, Me.Width, Me.Height)
        Dim brush As Brush = New SolidBrush(Me.BackColor)
        Dim GraphPath As GraphicsPath = GetRoundPath(Rect, BorderRadius)
        Me.Region = New Region(GraphPath)

        If IsMouseDown AndAlso Not FlatAppearance.MouseDownBackColor.IsEmpty Then

            Using mouseDownBrush As Brush = New SolidBrush(FlatAppearance.MouseDownBackColor)
                e.Graphics.FillPath(mouseDownBrush, GraphPath)
            End Using
        ElseIf IsMouseOver AndAlso Not FlatAppearance.MouseOverBackColor.IsEmpty Then

            Using overBrush As Brush = New SolidBrush(FlatAppearance.MouseOverBackColor)
                e.Graphics.FillPath(overBrush, GraphPath)
            End Using
        Else
            e.Graphics.FillPath(brush, GraphPath)
        End If

        Dim GraphInnerPath As GraphicsPath
        Dim pen As Pen

        If IsMouseDown AndAlso Not BorderDownColor.IsEmpty Then
            GraphInnerPath = GetRoundPath(Rect, BorderRadius, BorderDownWidth)
            pen = New Pen(BorderDownColor, BorderDownWidth)
        ElseIf IsMouseOver AndAlso Not BorderOverColor.IsEmpty Then
            GraphInnerPath = GetRoundPath(Rect, BorderRadius, BorderOverWidth)
            pen = New Pen(BorderOverColor, BorderOverWidth)
        Else
            GraphInnerPath = GetRoundPath(Rect, BorderRadius, BorderWidth)
            pen = New Pen(BorderColor, BorderWidth)
        End If

        pen.Alignment = PenAlignment.Inset
        If pen.Width > 0 Then e.Graphics.DrawPath(pen, GraphInnerPath)
        DrawText(e.Graphics, Rect)
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        IsMouseOver = True
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        IsMouseOver = False
        IsMouseDown = False
        Invalidate()
        MyBase.OnMouseHover(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal mevent As MouseEventArgs)
        IsMouseDown = True
        Invalidate()
        MyBase.OnMouseDown(mevent)
        MyBase.InvokeOnClick(Me, mevent)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal mevent As MouseEventArgs)
        IsMouseDown = False
        Invalidate()
        MyBase.OnMouseDown(mevent)
    End Sub

End Class

