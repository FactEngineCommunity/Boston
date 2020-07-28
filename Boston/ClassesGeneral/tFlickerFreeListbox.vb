Friend Class FlickerFreeListBox
    Inherits System.Windows.Forms.ListBox

    Public Sub New()
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        Me.DrawMode = DrawMode.OwnerDrawFixed
    End Sub

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        If Me.Items.Count > 0 Then
            e.DrawBackground()

            If e.Index >= 0 Then
                'e.Graphics.DrawString(Me.Items(e.Index).ToString(), e.Font, New SolidBrush(Me.ForeColor), New PointF(e.Bounds.X, e.Bounds.Y))

                If e.Index < 0 Then
                    Exit Sub
                End If
                Dim CurrentText As String = Me.Items(e.Index).ToString
                Dim ForeColor As Color = e.ForeColor

                ' Draw the background of the ListBox control for each item.
                e.DrawBackground()

                Dim liTokenType As FEQL.TokenType

                liTokenType = Me.Items(e.Index).Tag

                Select Case liTokenType
                    Case Is = FEQL.TokenType.MODELELEMENTNAME
                        ForeColor = Color.FromArgb(76, 153, 0)
                    Case Is = FEQL.TokenType.PREDICATE
                        ForeColor = Color.FromArgb(153, 0, 153)
                End Select

                Select Case liTokenType.ToString Like "KEYWD*"
                    Case Is = True
                        ForeColor = Color.Blue
                End Select

                Select Case True
                    Case e.Index = Me.SelectedIndex
                        ForeColor = Color.White
                    Case Else
                        'ForeColor = Color.Black
                End Select

                e.Graphics.DrawString(CurrentText, Me.Font, New SolidBrush(ForeColor), e.Bounds, StringFormat.GenericDefault)
                e.DrawFocusRectangle()
            End If
        End If

        'MyBase.OnDrawItem(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim iRegion As Region = New Region(e.ClipRectangle)
        e.Graphics.FillRegion(New SolidBrush(Me.BackColor), iRegion)

        If Me.Items.Count > 0 Then

            For i As Integer = 0 To Me.Items.Count - 1
                Dim irect As System.Drawing.Rectangle = Me.GetItemRectangle(i)

                If e.ClipRectangle.IntersectsWith(irect) Then

                    If (Me.SelectionMode = SelectionMode.One AndAlso Me.SelectedIndex = i) OrElse (Me.SelectionMode = SelectionMode.MultiSimple AndAlso Me.SelectedIndices.Contains(i)) OrElse (Me.SelectionMode = SelectionMode.MultiExtended AndAlso Me.SelectedIndices.Contains(i)) Then
                        OnDrawItem(New DrawItemEventArgs(e.Graphics, Me.Font, irect, i, DrawItemState.Selected, Me.ForeColor, Me.BackColor))
                    Else
                        OnDrawItem(New DrawItemEventArgs(e.Graphics, Me.Font, irect, i, DrawItemState.[Default], Me.ForeColor, Me.BackColor))
                    End If

                    iRegion.Complement(irect)
                End If
            Next
        End If

        MyBase.OnPaint(e)
    End Sub
End Class

