Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Namespace UI

    <ToolboxBitmap(GetType(TabControl))> _
    Friend Class CustTabControl : Inherits TabControl

#Region "    Variables "

        Private _DisplayManager As TabControlDisplayManager = TabControlDisplayManager.Framework
        Private HiLightCloseTabIndex As Integer = -1
        Private HiDownCloseTabIndex As Integer = -1

        Private ThisNoClosing As Boolean = False

        Private _TabsStationary As Boolean = False

#End Region

#Region "    Properties "

        <System.ComponentModel.DefaultValue(GetType(TabControlDisplayManager), "Framework"), System.ComponentModel.Category("Appearance")> _
        Public Property DisplayManager() As TabControlDisplayManager
            Get
                Return _DisplayManager
            End Get
            Set(ByVal value As TabControlDisplayManager)
                _DisplayManager = value
                If Me._DisplayManager.Equals(TabControlDisplayManager.Framework) Then
                    Me.SetStyle(ControlStyles.UserPaint, True)
                    Me.ItemSize = New Size(0, 15)
                    Me.Padding = New Point(9, 0)
                Else
                    Me.ItemSize = New Size(0, 0)
                    Me.Padding = New Point(6, 3)
                    Me.SetStyle(ControlStyles.UserPaint, False)
                End If
            End Set
        End Property

        'I know this is dodgy. We should be overriding the tab page objects.
        'Anyway, this hack should do. We want the tab control to have no close buttons.
        Public Property NoClosing() As Boolean
            Get
                Return Me.ThisNoClosing
            End Get
            Set(ByVal value As Boolean)
                Me.ThisNoClosing = value
            End Set
        End Property

        Public Property TabsStationary() As Boolean
            Get
                Return Me._TabsStationary
            End Get
            Set(ByVal value As Boolean)
                Me._TabsStationary = value
            End Set
        End Property

#End Region

#Region "    Constructor "

        Public Sub New()
            MyBase.New()
            If Me._DisplayManager.Equals(TabControlDisplayManager.Framework) Then
                Me.SetStyle(ControlStyles.UserPaint, True)
                Me.ItemSize = New Size(0, 15)
                Me.Padding = New Point(9, 0)
            End If
            Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)
            Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            Me.SetStyle(ControlStyles.ResizeRedraw, True)
            Me.ResizeRedraw = True
        End Sub

#End Region

#Region "    Private Methods "

        Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
            Get
                Select Case System.Environment.OSVersion.Version.Major
                    Case 5
                        'Windows xp, don't try to stop flicker
                        Return MyBase.CreateParams

                    Case Else
                        'Other
                        Dim cp As CreateParams = MyBase.CreateParams
                        cp.ExStyle = cp.ExStyle Or &H2000000
                        Return cp

                End Select
            End Get
        End Property

        Private Function GetPath(ByVal index As Integer) As System.Drawing.Drawing2D.GraphicsPath
            Dim path As New System.Drawing.Drawing2D.GraphicsPath()
            path.Reset()

            Dim rect As Rectangle = Me.GetTabRect(index)

            If index = 0 Then
                path.AddLine(rect.Left + 1, rect.Bottom + 1, rect.Left + 1, rect.Top + 1)
                path.AddLine(rect.Left + 1, rect.Top + 1, rect.Left + 2, rect.Top)
                path.AddLine(rect.Left + rect.Height + 1, rect.Top, rect.Right - 3, rect.Top)
                path.AddLine(rect.Right - 1, rect.Top + 2, rect.Right - 1, rect.Bottom + 1)
            Else
                If index = Me.SelectedIndex Then
                    path.AddLine(rect.Left + 5 - rect.Height, rect.Bottom + 1, rect.Left + 4, rect.Top + 2)
                    path.AddLine(rect.Left + 8, rect.Top, rect.Right - 3, rect.Top)
                    path.AddLine(rect.Right - 1, rect.Top + 2, rect.Right - 1, rect.Bottom + 1)
                    path.AddLine(rect.Right - 1, rect.Bottom + 1, rect.Left + 5 - rect.Height, rect.Bottom + 1)
                Else
                    path.AddLine(rect.Left, rect.Top + 6, rect.Left + 4, rect.Top + 2)
                    path.AddLine(rect.Left + 8, rect.Top, rect.Right - 3, rect.Top)
                    path.AddLine(rect.Right - 1, rect.Top + 2, rect.Right - 1, rect.Bottom + 1)
                    path.AddLine(rect.Right - 1, rect.Bottom + 1, rect.Left, rect.Bottom + 1)
                End If
            End If
            Return path
        End Function

        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            Me.SuspendLayout()

            '   Paint the Background
            Me.PaintTransparentBackground(e.Graphics, Me.ClientRectangle)

            '   Paint all the tabs
            If Me.TabCount > 0 Then
                For index As Integer = 0 To Me.TabCount - 1
                    Me.PaintTab(e, index)
                Next
            End If

            '   paint a border round the pagebox
            '   We can't make the border disappear so have to do it like this.
            If Me.TabCount > 0 Then

                Dim borderRect As Rectangle = Me.TabPages(0).Bounds
                If Me.SelectedTab IsNot Nothing Then
                    borderRect = Me.SelectedTab.Bounds
                End If

                borderRect.Inflate(1, 1)
                ControlPaint.DrawBorder(e.Graphics, borderRect, ThemedColours.ToolBorder, ButtonBorderStyle.Solid)

            End If

            '   repaint the bit where the selected tab is
            Select Case Me.SelectedIndex
                Case -1
                Case 0
                    Dim selrect As Rectangle = Me.GetTabRect(Me.SelectedIndex)
                    Dim selrectRight As Integer = selrect.Right
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, selrect.Left + 2, selrect.Bottom + 1, selrectRight - 2, selrect.Bottom + 1)
                Case Else
                    Dim selrect As Rectangle = Me.GetTabRect(Me.SelectedIndex)
                    Dim selrectRight As Integer = selrect.Right
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, selrect.Left + 6 - selrect.Height, selrect.Bottom + 1, selrectRight - 2, selrect.Bottom + 1)
            End Select

            Me.ResumeLayout()
        End Sub

        Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
            If Me.DesignMode Then
                Dim backBrush As New System.Drawing.Drawing2D.LinearGradientBrush(Me.Bounds, SystemColors.ControlLightLight, SystemColors.ControlLight, Drawing2D.LinearGradientMode.Vertical)
                pevent.Graphics.FillRectangle(backBrush, Me.Bounds)
                backBrush.Dispose()
            Else
                Me.PaintTransparentBackground(pevent.Graphics, Me.ClientRectangle)
            End If
        End Sub

        Protected Sub PaintTransparentBackground(ByVal g As Graphics, ByVal clipRect As Rectangle)
            If (Me.Parent IsNot Nothing) Then
                clipRect.Offset(Me.Location)
                Dim e As PaintEventArgs = New PaintEventArgs(g, clipRect)
                Dim state As GraphicsState = g.Save
                g.SmoothingMode = SmoothingMode.HighSpeed
                Try
                    g.TranslateTransform(CType(-Me.Location.X, Single), CType(-Me.Location.Y, Single))
                    Me.InvokePaintBackground(Me.Parent, e)
                    Me.InvokePaint(Me.Parent, e)

                Finally
                    g.Restore(state)
                    clipRect.Offset(-Me.Location.X, -Me.Location.Y)
                End Try
            Else
                Dim backBrush As New System.Drawing.Drawing2D.LinearGradientBrush(Me.Bounds, SystemColors.ControlLightLight, SystemColors.ControlLight, Drawing2D.LinearGradientMode.Vertical)
                g.FillRectangle(backBrush, Me.Bounds)
                backBrush.Dispose()
            End If
        End Sub

        Private Sub PaintTab(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal index As Integer)
            Dim path As System.Drawing.Drawing2D.GraphicsPath = Me.GetPath(index)
            Me.PaintTabBackground(e.Graphics, index, path)
            Me.PaintTabBorder(e.Graphics, index, path)
            Me.PaintTabText(e.Graphics, index)
            Me.PaintTabImage(e.Graphics, index)
        End Sub

        Private Sub PaintTabBackground(ByVal graph As System.Drawing.Graphics, ByVal index As Integer, ByVal path As System.Drawing.Drawing2D.GraphicsPath)
            Dim rect As Rectangle = Me.GetTabRect(index)

            If rect.Height > 1 AndAlso rect.Width > 1 Then
                Dim buttonBrush As System.Drawing.Brush = _
                    New System.Drawing.Drawing2D.LinearGradientBrush( _
                        rect, _
                        SystemColors.ControlLightLight, _
                        SystemColors.ControlLight, _
                        Drawing2D.LinearGradientMode.Vertical)

                If index = Me.SelectedIndex Then
                    buttonBrush = New System.Drawing.SolidBrush(SystemColors.ControlLightLight)
                End If

                graph.FillPath(buttonBrush, path)
                buttonBrush.Dispose()
            End If
        End Sub

        Private Sub PaintTabBorder(ByVal graph As System.Drawing.Graphics, ByVal index As Integer, ByVal path As System.Drawing.Drawing2D.GraphicsPath)
            Dim borderPen As New Pen(SystemColors.ControlDark)
            If index = Me.SelectedIndex Then
                borderPen = New Pen(ThemedColours.ToolBorder)
            End If

            graph.DrawPath(borderPen, path)
            borderPen.Dispose()
        End Sub

        Private Sub PaintTabImage(ByVal graph As System.Drawing.Graphics, ByVal index As Integer)
            Dim tabImage As Image = Nothing
            If Me.TabPages(index).ImageIndex > -1 AndAlso Me.ImageList IsNot Nothing Then
                tabImage = Me.ImageList.Images(Me.TabPages(index).ImageIndex)
            ElseIf Me.TabPages(index).ImageKey.Trim().Length > 0 AndAlso Me.ImageList IsNot Nothing Then
                tabImage = Me.ImageList.Images(Me.TabPages(index).ImageKey)
            End If
            Dim rect As Rectangle = Me.GetTabRect(index)
            If tabImage IsNot Nothing Then
                graph.DrawImage(tabImage, rect.Left + 6, 4, rect.Height - 2, rect.Height - 2)
            End If

            If index = 0 Or Me.NoClosing Then Exit Sub

            'Draw the close box
            If Me.HiLightCloseTabIndex = index Then
                graph.DrawImage(My.Resources.Closehil16x16, rect.Right - 11, 4, rect.Height - 2, rect.Height - 2)
            ElseIf Me.HiDownCloseTabIndex = index Then
                graph.DrawImage(My.Resources.CloseDownl16x16, rect.Right - 11, 4, rect.Height - 2, rect.Height - 2)
            Else
                graph.DrawImage(My.Resources.Close16x16, rect.Right - 11, 4, rect.Height - 2, rect.Height - 2)
            End If

        End Sub

        Private Sub PaintTabText(ByVal graph As System.Drawing.Graphics, ByVal index As Integer)
            Dim rect As Rectangle = Me.GetTabRect(index)
            Dim rect2 As New Rectangle(rect.Left + 20, rect.Top + 1, rect.Width - 6, rect.Height)
            If index = 0 Then rect2 = New Rectangle(rect.Left + 20, rect.Top + 1, rect.Width - rect.Height, rect.Height)

            Dim tabtext As String = Me.TabPages(index).Text

            Dim format As New System.Drawing.StringFormat()
            format.Alignment = StringAlignment.Near
            format.LineAlignment = StringAlignment.Center
            format.Trimming = StringTrimming.EllipsisCharacter

            Dim forebrush As Brush = Nothing

            If Me.TabPages(index).Enabled = False Then
                forebrush = SystemBrushes.ControlDark
            Else
                forebrush = SystemBrushes.ControlText
            End If

            Dim tabFont As Font = Me.Font
            If index = Me.SelectedIndex Then
                tabFont = New Font(Me.Font, FontStyle.Bold)
                If index = 0 Then
                    rect2 = New Rectangle(rect.Left + 20, rect.Top + 1, rect.Width - rect.Height + 5, rect.Height)
                End If
            End If

            graph.DrawString(tabtext, tabFont, forebrush, rect2, format)

        End Sub

#End Region

        Public Event TabClosing(ByVal Tab As TabPage, ByRef Cancel As Boolean)
        Public Event RightClickTab(ByVal SelectedTabIndex As Integer)

        Public Enum TabControlDisplayManager As Integer
            [Default]
            Framework
        End Enum

        Friend Sub CloseTab(ByVal Tab As TabPage)
            If Me.TabPages.IndexOf(Tab) = 0 Or Me.NoClosing Then Exit Sub

            Dim Cancel As Boolean = False
            RaiseEvent TabClosing(Tab, Cancel)

            Dim idx As Integer = Me.TabPages.IndexOf(Tab)

            If Not Cancel Then
                Me.TabPages.Remove(Tab)
                If idx < Me.TabPages.Count Then
                    Me.SelectedIndex = idx
                Else
                    Me.SelectedIndex = Me.TabPages.Count - 1
                End If
            End If
        End Sub

        Protected Overrides Sub OnSelecting(ByVal e As System.Windows.Forms.TabControlCancelEventArgs)
            If e.Action = TabControlAction.Selecting _
                AndAlso e.TabPage IsNot Nothing _
                AndAlso e.TabPage.Enabled = False Then

                e.Cancel = True
                If e.TabPageIndex = 0 AndAlso Me.TabPages.Count = 1 Then
                    If Me.TabPages(0).Controls.Count > 0 Then
                        Dim item As Form = TryCast(Me.TabPages(0).Controls(0), Form)
                        If item IsNot Nothing Then item.Close()
                    End If
                    Me.TabPages.RemoveAt(0)
                ElseIf e.TabPageIndex = 0 AndAlso Me.TabPages.Count > 1 Then
                    e.Cancel = False
                End If
            End If
            MyBase.OnSelecting(e)
        End Sub

        Protected Overrides Sub OnSelected(ByVal e As System.Windows.Forms.TabControlEventArgs)
            If e.Action = TabControlAction.Selected _
                AndAlso e.TabPage IsNot Nothing _
                AndAlso e.TabPage.Enabled = False Then

                If Me.TabPages.Count > e.TabPageIndex + 1 Then
                    Me.SelectedIndex = e.TabPageIndex + 1
                ElseIf e.TabPageIndex > 0 Then
                    Me.SelectedIndex = e.TabPageIndex - 1
                End If
            End If
            MyBase.OnSelected(e)
            Me.Invalidate()
        End Sub

        <DllImport("user32.dll")> _
        Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function

        Private Const WM_SETFONT As Integer = &H30
        Private Const WM_FONTCHANGE As Integer = &H1D

        Protected Overloads Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            Me.OnFontChanged(EventArgs.Empty)
        End Sub

        Protected Overloads Overrides Sub OnFontChanged(ByVal e As EventArgs)
            MyBase.OnFontChanged(e)
            Dim f As New Font(Me.Font.FontFamily, Me.Font.Size + 1, FontStyle.Bold, GraphicsUnit.Pixel)
            Dim hFont As IntPtr = f.ToHfont()
            SendMessage(Me.Handle, WM_SETFONT, hFont, CType((-1), IntPtr))
            SendMessage(Me.Handle, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero)
            Me.UpdateStyles()
            Me.ItemSize = New Size(0, Me.Font.Height + 2)
        End Sub

        Private Sub MDTabControl_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
            If e.Button = System.Windows.Forms.MouseButtons.Right Then Exit Sub
            If Me.SelectedIndex = 0 Or Me.NoClosing Then Exit Sub

            Dim rect As Rectangle = Me.GetTabRect(Me.SelectedIndex)
            Dim HitTest As New Rectangle(rect.X + rect.Width - 10, rect.Y, 10, 11)

            If HitTest.Contains(New Point(e.X, e.Y)) Then
                Call Me.CloseTab(Me.SelectedTab)
            End If
        End Sub

        Private Sub MDTabControl_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
            'Select tab on right mouse click
            If e.Button = System.Windows.Forms.MouseButtons.Right Then
                For i As Integer = 0 To Me.TabPages.Count - 1
                    If Me.GetTabRect(i).Contains(New Point(e.X, e.Y)) Then
                        Me.SelectedIndex = i
                        Exit For
                    End If
                Next
                RaiseEvent RightClickTab(Me.SelectedIndex)

            ElseIf e.Button = System.Windows.Forms.MouseButtons.Left Then
                Me.HiLightCloseTabIndex = -1
                For i As Integer = 0 To Me.TabCount - 1
                    Dim rect As Rectangle = Me.GetTabRect(i)
                    rect = New Rectangle(rect.X + rect.Width - 10, rect.Y, 10, 11)
                    If rect.Contains(e.Location) Then
                        Me.HiDownCloseTabIndex = i
                        Me.Refresh()
                        Exit Sub
                    End If
                Next
                Me.HiDownCloseTabIndex = -1
                Me.Refresh()
            End If
        End Sub

        Private Sub CustTabControl_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
            Me.HiDownCloseTabIndex = -1
            Me.Refresh()
        End Sub

        Private Sub MDTabControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
            'Left button down means a tab slide
            If Not Me.TabsStationary And e.Button = System.Windows.Forms.MouseButtons.Left And Me.SelectedIndex <> 0 Then
                Dim selPos As System.Drawing.Rectangle = Me.GetTabRect(Me.SelectedIndex)
                Dim ovrPos As System.Drawing.Rectangle = Nothing

                'Find the tab we're sliding over
                Dim overIdx As Integer = -1
                For i As Integer = 0 To Me.TabPages.Count - 1
                    ovrPos = Me.GetTabRect(i)
                    If ovrPos.Contains(New Point(e.X, e.Y)) Then
                        overIdx = i
                        Exit For
                    End If
                Next
                'Don't swap with the start page or itself
                If overIdx < 1 Or overIdx = Me.SelectedIndex Then Exit Sub

                'If the over tab is longer than the selected tab
                If ovrPos.Width > selPos.Width Then
                    'Only swap if the mouse moves past the length of the larger over tab width minus the smaller selected tab's width
                    If e.X < ovrPos.Left + (ovrPos.Width - selPos.Width) Then Exit Sub
                End If

                'Swap the two tabs
                Dim tabSel As TabPage = Me.TabPages(Me.SelectedIndex)
                Dim tabOvr As TabPage = Me.TabPages(overIdx)

                Me.TabPages(Me.SelectedIndex) = tabOvr
                Me.TabPages(overIdx) = tabSel

                Me.SelectedIndex = overIdx
                Application.DoEvents()

                Exit Sub
            End If

            'Hilight the close button on hover
            For i As Integer = 0 To Me.TabCount - 1
                Dim rect As Rectangle = Me.GetTabRect(i)
                rect = New Rectangle(rect.X + rect.Width - 10, rect.Y, 10, 11)
                If rect.Contains(e.Location) Then
                    If e.Button = System.Windows.Forms.MouseButtons.Left Then
                        Me.HiLightCloseTabIndex = -1
                        Me.HiDownCloseTabIndex = i
                    Else
                        Me.HiLightCloseTabIndex = i
                        Me.HiDownCloseTabIndex = -1
                    End If

                    Me.Refresh()
                    Exit Sub
                End If
            Next

            Me.HiLightCloseTabIndex = -1
            Me.HiDownCloseTabIndex = -1
            Me.Refresh()
        End Sub

        Private Sub MDTabControl_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
            Me.HiLightCloseTabIndex = -1
            Me.HiDownCloseTabIndex = -1
            Me.Refresh()
        End Sub

        Public Shared Function ShortenTextForTab(ByVal TabText As String) As String
            If TabText.Length > 35 Then
                Dim left As String = TabText.Substring(0, 26)
                Dim right As String = TabText.Substring(TabText.Length - 5, 5)
                Return left & "...." & right
            Else
                Return TabText
            End If
        End Function

    End Class

End Namespace