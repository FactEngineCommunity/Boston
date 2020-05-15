Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Media
Imports System.Threading

Public Class GroupNotificationBar    
        Inherits UserControl

        Private flashTimer As New System.Windows.Forms.Timer()
        Private m_smallImageList As ImageList = Nothing
        Private imageKey As Integer = 0

        Private closeButtonSize As New Size(20, 20)
        Private closeButtonPadding As Integer = 6
        Private textY As Integer = 5

        Private playSoundOnVisible As Boolean = True
        Private mouseInBounds As Boolean = False
        Private controlHighlighted As Boolean = False
        Private inAnimation As Boolean = False

        Private tickCount As Integer = 0
        Private flashCount As Integer = 0
        Private flashTo As Integer = 0

        <Description("Image list containg images shown on the left side of the control"), Category("Appearance")> _
        Public Property SmallImageList() As ImageList
            Get
                Return m_smallImageList
            End Get
            Set(value As ImageList)
                m_smallImageList = value
            End Set
        End Property

        <DefaultValue(0)> _
        <Description("The index of an image contained in the SmallImageList in which to display on the control"), Category("Appearance")> _
        Public Property ImageIndex() As Integer
            Get
                Return imageKey
            End Get
            Set(value As Integer)
                imageKey = value
                Me.Invalidate()
            End Set
        End Property

        Public Overrides Property Text() As String
            Get
                Return MyBase.Text
            End Get
            Set(value As String)
                MyBase.Text = value
                Me.Invalidate()
            End Set
        End Property

        <DefaultValue(True)> _
        <Description("Weather or not a sound should be played when the control is shown"), Category("Behavior")> _
        Public Property PlaySoundWhenShown() As Boolean
            Get
                Return playSoundOnVisible
            End Get
            Set(value As Boolean)
                playSoundOnVisible = value
            End Set
        End Property

        Public Event AcceptPressed(ByVal asInvitation As Object)
        Public Event DeclinePressed(ByVal asProjectId As Object)

        Public Sub New()
            Call InitializeComponent()

            Me.BackColor = SystemColors.Info

            flashTimer.Interval = 1000
            AddHandler flashTimer.Tick, New EventHandler(AddressOf flashTimer_Tick)

            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.ResizeRedraw, True)
        End Sub

        Public Sub Flash(interval As Integer, times As Integer)
            If Me.Visible Then
                flashTo = times
                tickCount = 0

                flashTimer.Interval = interval
                flashTimer.Start()
            End If
        End Sub

        Public Sub Flash(numberOfTimes As Integer)
            Flash(1000, numberOfTimes)
        End Sub

        Public Sub FlashOnce(milliseconds As Integer)
            Flash(milliseconds, 1)
        End Sub

        Public Overloads Sub Show(animate As Boolean)
            If animate Then
                Dim origHeight As Integer = Me.Height
                Me.Height = 1
                Me.Show()
                inAnimation = True

                For height As Integer = Me.Height To origHeight - 1 Step 5
                    textY += 2
                    Me.Height = height
                    Me.Refresh()
                    Thread.Sleep(5)
                Next

                inAnimation = False
            Else
                Me.Show()
            End If

            Me.Refresh()
        End Sub

        Public Overloads Sub Hide(animate As Boolean)
            If animate Then
                Dim origHeight As Integer = Me.Height
                inAnimation = True

                For height As Integer = Me.Height To 2 Step -5
                    textY -= 2
                    Me.Height = height
                    Me.Refresh()
                    Thread.Sleep(5)
                Next
                Me.Hide()
                Me.Height = origHeight
                inAnimation = False
            Else
                Me.Hide()
            End If

            Me.Refresh()
        End Sub

        Private Sub flashTimer_Tick(sender As Object, e As EventArgs)
            If controlHighlighted Then
                Me.ForeColor = SystemColors.ControlText
                Me.BackColor = SystemColors.Info
                controlHighlighted = False
                flashCount += 1

                If flashCount = flashTo Then
                    flashTimer.[Stop]()
                    flashCount = 0
                End If
            Else
                Me.ForeColor = SystemColors.HighlightText
                Me.BackColor = SystemColors.Highlight
                controlHighlighted = True
            End If

            tickCount += 1
            Me.Refresh()
        End Sub

#Region "Protected Methods"

        Protected Sub DrawText(e As PaintEventArgs)

            Me.LabelPrompt.Text = Text

            'Dim leftPadding As Integer = 1

            'If m_smallImageList IsNot Nothing AndAlso m_smallImageList.Images.Count > 0 AndAlso m_smallImageList.Images.Count > imageKey Then
            '    leftPadding = m_smallImageList.ImageSize.Width + 4
            '    e.Graphics.DrawImage(m_smallImageList.Images(imageKey), New Point(2, 5))
            'End If

            'Dim textSize As Size = TextRenderer.MeasureText(e.Graphics, Me.Text, Me.Font)
            'Dim maxTextWidth As Integer = (Me.Width - (closeButtonSize.Width + (closeButtonPadding * 2)))
            'Dim lineHeight As Integer = textSize.Height + 2
            'Dim numLines As Integer = 1

            'If textSize.Width > maxTextWidth Then
            '    numLines = textSize.Width \ maxTextWidth + 1
            'End If

            'Dim textRect As New Rectangle()
            'textRect.Width = Me.Width - (closeButtonSize.Width + closeButtonPadding) - leftPadding
            'textRect.Height = (numLines * lineHeight)
            'textRect.X = leftPadding
            'textRect.Y = 5

            'If Me.Height < (numLines * lineHeight) + 10 Then
            '    If Not inAnimation Then
            '        textRect.Y = 5
            '        Me.Height = (numLines * lineHeight) + 10
            '    Else
            '        textRect.Y = textY
            '    End If
            'End If

            'TextRenderer.DrawText(e.Graphics, Me.Text, Me.Font, textRect, Me.ForeColor, TextFormatFlags.WordBreak Or TextFormatFlags.Left Or TextFormatFlags.Top)

        End Sub

        Protected Sub DrawCloseButton(e As PaintEventArgs)
            Dim closeButtonColor As Color = Color.Black

            If mouseInBounds Then
                closeButtonColor = Color.White
            End If

            Dim linePen As New Pen(Me.ForeColor, 2)
            Dim line1Start As New Point((Me.Width - (closeButtonSize.Width - closeButtonPadding)), closeButtonPadding)
            Dim line1End As New Point((Me.Width - closeButtonPadding), (closeButtonSize.Height - closeButtonPadding))
            Dim line2Start As New Point((Me.Width - closeButtonPadding), closeButtonPadding)
            Dim line2End As New Point((Me.Width - (closeButtonSize.Width - closeButtonPadding)), (closeButtonSize.Height - closeButtonPadding))

            e.Graphics.DrawLine(linePen, line1Start, line1End)
            e.Graphics.DrawLine(linePen, line2Start, line2End)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            DrawText(e)
            DrawCloseButton(e)

            MyBase.OnPaint(e)
        End Sub

        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            Me.ForeColor = SystemColors.HighlightText
            Me.BackColor = SystemColors.Highlight
            mouseInBounds = True

            MyBase.OnMouseEnter(e)
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            If controlHighlighted Then
                Me.ForeColor = SystemColors.HighlightText
                Me.BackColor = SystemColors.Highlight
            Else
                Me.ForeColor = SystemColors.ControlText
                Me.BackColor = SystemColors.Info
            End If

            Me.ForeColor = SystemColors.ControlText
            mouseInBounds = False

            MyBase.OnMouseLeave(e)
        End Sub

        Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
            If e.X >= (Me.Width - (closeButtonSize.Width + closeButtonPadding)) AndAlso e.Y <= 12 Then
                Me.Hide()
            Else
                If ContextMenuStrip IsNot Nothing Then
                    ContextMenuStrip.Show(Me, e.Location)
                End If
            End If

            MyBase.OnMouseClick(e)
        End Sub

        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            If Me.Visible AndAlso playSoundOnVisible Then
                SystemSounds.Beep.Play()
            End If

            MyBase.OnVisibleChanged(e)
        End Sub
#End Region


        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonDecline.Click

            RaiseEvent DeclinePressed(Me.Tag)

            Me.Hide()
            Me.Dispose()

        End Sub

        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

            RaiseEvent AcceptPressed(Me.Tag)
            Me.Hide()
            Me.Dispose()

        End Sub

    End Class
