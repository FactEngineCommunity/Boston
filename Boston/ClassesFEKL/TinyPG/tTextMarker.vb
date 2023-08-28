Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace FEKL.Controls

    Public NotInheritable Class TextMarker
        Inherits NativeWindow
        Implements IDisposable
        ' Methods
        Public Sub New(ByVal textbox As RichTextBox)
            Me.Textbox = textbox
            AddHandler Me.Textbox.MouseMove, New MouseEventHandler(AddressOf Me.Textbox_MouseMove)
            MyBase.AssignHandle(Me.Textbox.Handle)
            Me.ToolTip = New ToolTip
            Me.Clear()
            Me.lastMousePos = New Point
        End Sub

        Public Sub AddWord(ByVal wordstart As Integer, ByVal wordlen As Integer, ByVal color As Color)
            Me.AddWord(wordstart, wordlen, color, "")
        End Sub

        Public Sub AddWord(ByVal wordstart As Integer, ByVal wordlen As Integer, ByVal color As Color, ByVal ToolTip As String)
            Dim word As New Word With {
                .Start = wordstart,
                .Length = wordlen,
                .Color = color,
                .ToolTip = ToolTip
            }
            Me.MarkedWords.Add(word)
        End Sub

        Public Sub Clear()
            Me.MarkedWords = New List(Of Word)
        End Sub

        Public Sub Dispose() Implements System.IDisposable.Dispose
            Me.ReleaseHandle()
        End Sub

        Private Sub MarkWord(ByVal word As Word, ByVal graphics As Graphics)
            Dim path As New GraphicsPath
            Dim points As New List(Of Point)
            Dim p1 As Point = Me.Textbox.GetPositionFromCharIndex(word.Start)
            Dim p2 As Point = Me.Textbox.GetPositionFromCharIndex((word.Start + word.Length))
            If (word.Length = 0) Then
                p1.X = (p1.X - 5)
                p2.X = (p2.X + 5)
            End If
            p1.Y = (p1.Y + (Me.Textbox.Font.Height - 1))
            points.Add(p1)
            Dim up As Boolean = True
            Dim x As Integer = (p1.X + 2)
            Do While (x < (p2.X + 2))
                Dim p As Point = If(up, New Point(x, (p1.Y + 2)), New Point(x, p1.Y))
                points.Add(p)
                up = Not up
                x = (x + 2)
            Loop
            If (points.Count >= 1) Then
                path.StartFigure()
                path.AddLines(points.ToArray)
            End If
            Dim pen As New Pen(word.Color)
            graphics.DrawPath(pen, path)
            pen.Dispose()
            path.Dispose()
        End Sub

        Public Sub MarkWords()
            If ((Not Me.Textbox.IsDisposed AndAlso Me.Textbox.Enabled) AndAlso Me.Textbox.Visible) Then
                Dim graphics As Graphics = Me.Textbox.CreateGraphics
                Dim minpos As Integer = Me.Textbox.GetCharIndexFromPosition(New Point(0, 0))
                Dim maxpos As Integer = Me.Textbox.Text.Length 'GetCharIndexFromPosition(New Point(Me.Textbox.Width - 1, Me.Textbox.Height - 1))
                Dim w As Word
                For Each w In Me.MarkedWords
                    If (((w.Start + w.Length) >= minpos) AndAlso (w.Start <= maxpos)) Then
                        Me.MarkWord(w, graphics)
                    End If
                Next
                graphics.Dispose()
            End If
        End Sub

        Private Sub Textbox_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            If ((Me.lastMousePos.X <> e.X) AndAlso (Me.lastMousePos.Y <> e.Y)) Then
                Me.lastMousePos = New Point(e.X, e.Y)
                Dim i As Integer = Me.Textbox.GetCharIndexFromPosition(Me.lastMousePos)
                Dim found As Boolean = False
                Dim w As Word
                For Each w In Me.MarkedWords
                    If ((w.Start <= i) AndAlso ((w.Start + w.Length) > i)) Then
                        Dim p As Point = Me.Textbox.GetPositionFromCharIndex(w.Start)
                        p.Y = (p.Y + &H12)
                        Me.ToolTip.Show(w.ToolTip, Me.Textbox, p)
                        found = True
                    End If
                Next
                If Not found Then
                    Me.ToolTip.Hide(Me.Textbox)
                End If
            End If
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            Dim msgIndex As Integer = m.Msg

            Select Case msgIndex
                Case &H113
                    MyBase.WndProc((m))
                    Me.MarkWords()
                    Return
                Case &H114
                    MyBase.WndProc((m))
                    Me.MarkWords()
                    Return
                Case &H115
                    MyBase.WndProc((m))
                    Me.MarkWords()
                    Return
                Case &H101
                    MyBase.WndProc((m))
                    Me.MarkWords()
                    Return
                Case Else
                    MyBase.WndProc(m)
            End Select

        End Sub


        ' Fields
        Private lastMousePos As Point
        Private MarkedWords As List(Of Word)
        Public Textbox As RichTextBox
        Private ToolTip As ToolTip

        ' Nested Types
        <StructLayout(LayoutKind.Sequential)>
        Private Structure Word
            Public Start As Integer
            Public Length As Integer
            Public Color As Color
            Public ToolTip As String
        End Structure
    End Class
End Namespace


