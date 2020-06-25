Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Diagnostics

Namespace UI

    Partial Friend Class ImageListBox
        Inherits ListBox
        Private m_LastItemCount As Integer
        Private m_LastClientWidth As Integer
        Private m_LastImageSize As Integer
        Private m_MaxStringHeigth As Integer
        Private m_ImageList As ImageList

        Public Property ImageList() As ImageList
            Get
                Return m_ImageList
            End Get
            Set(ByVal value As ImageList)
                m_ImageList = value
            End Set
        End Property

        'InitializeComponent();

        'DrawMode = DrawMode.OwnerDrawFixed;
        Public Sub New()
        End Sub

        Protected Overloads Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
            Me.SuspendLayout()
            Dim item As ImageListBoxItem = If(e.Index < 0 OrElse Me.DesignMode, Nothing, TryCast(Items(e.Index), ImageListBoxItem))
            Dim draw As Boolean = m_ImageList IsNot Nothing AndAlso (item IsNot Nothing)

            If draw Then
                e.DrawBackground()
                e.DrawFocusRectangle()

                Dim imageSize As Size = m_ImageList.ImageSize
                If Me.ItemHeight <> imageSize.Height + 2 Then
                    Me.ItemHeight = imageSize.Height + 2
                End If

                CheckHorizonalScroll(e.Graphics, e.Font)

                Dim bounds As Rectangle = e.Bounds
                Dim strTextToDraw As String = item.ToString()
                Dim color__1 As Color = e.ForeColor

                If item.ImageIndex > -1 AndAlso item.ImageIndex < m_ImageList.Images.Count Then
                    m_ImageList.Draw(e.Graphics, bounds.Left, bounds.Top, item.ImageIndex)
                    If item.Color <> Color.Empty Then
                        color__1 = item.Color
                    End If
                End If

                'bounds.Left
                e.Graphics.DrawString(strTextToDraw, e.Font, New SolidBrush(color__1), 0 + imageSize.Width, bounds.Top + CInt((Me.ItemHeight - m_MaxStringHeigth) \ 2))
            Else
                MyBase.OnDrawItem(e)
            End If
            Me.ResumeLayout()
        End Sub

        Private Sub CheckHorizonalScroll(ByVal g As Graphics, ByVal f As Font)
            Dim maxStringWidth As Integer = 0
            Dim maxStringHeight As Integer = 0
            For Each item As Object In Items
                Dim s As String = item.ToString()
                Dim size As SizeF = g.MeasureString(s, f)
                If maxStringHeight < size.Height Then
                    maxStringHeight = CInt(Math.Truncate(size.Height))
                End If
                If maxStringWidth < size.Width Then
                    maxStringWidth = CInt(Math.Truncate(size.Width))
                End If
            Next

            m_LastImageSize = m_ImageList.ImageSize.Width
            m_LastClientWidth = ClientSize.Width
            m_LastItemCount = Items.Count
            m_MaxStringHeigth = maxStringHeight

            Dim he As Integer = maxStringWidth + m_LastImageSize
            If HorizontalExtent <> he Then
                HorizontalExtent = he
            End If

        End Sub

        Public Class ImageListBoxItem
            Private m_ImageIndex As Integer
            Private m_Item As Object
            Private m_Color As Color

            Public Property ImageIndex() As Integer
                Get
                    Return m_ImageIndex
                End Get
                Set(ByVal value As Integer)
                    m_ImageIndex = value
                End Set
            End Property

            Public Property Item() As Object
                Get
                    Return m_Item
                End Get
                Set(ByVal value As Object)
                    m_Item = value
                End Set
            End Property

            Public Property Color() As Color
                Get
                    Return m_Color
                End Get
                Set(ByVal value As Color)
                    m_Color = value
                End Set
            End Property


            Public Sub New()
                Me.New(Nothing)
            End Sub

            Public Sub New(ByVal item As Object)
                Me.New(-1, item)
            End Sub

            Public Sub New(ByVal imageIndex As Integer, ByVal item As Object)
                Me.New(imageIndex, Color.Empty, item)
            End Sub

            Public Sub New(ByVal imageIndex As Integer, ByVal color As Color, ByVal item As Object)
                m_ImageIndex = imageIndex
                m_Color = color
                m_Item = item
            End Sub

            Public Overloads Overrides Function ToString() As String
                If m_Item Is Nothing Then
                    Return "Null"
                End If
                Return m_Item.ToString()
            End Function
        End Class

    End Class

End Namespace