Imports System.Reflection

Public Class frmAutoComplete

    Private zoTextEditor As RichTextBox
    Public OpacityValue As Single = 0.8
    Public TransparencyColour As System.Drawing.Color = Color.White
    Public zrCallingForm As Object

    Public zsIntellisenseBuffer As String

    Public Sub New(ByRef aoTextEditor As RichTextBox)

        Me.zoTextEditor = aoTextEditor

        Call InitializeComponent()

    End Sub

    Private Sub frmAutoComplete_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call Me.roundCorners(Me)
        Me.Opacity = 1

    End Sub

    Private Sub ListBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles ListBox.DrawItem

        If e.Index < 0 Then
            e.DrawFocusRectangle()
            Exit Sub
        End If
        Dim CurrentText As String = Me.ListBox.Items(e.Index).ToString
        Dim ForeColor As Color = e.ForeColor

        ' Draw the background of the ListBox control for each item.
        e.DrawBackground()

        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds)
        End If

        Dim liTokenType As VAQL.TokenType

        liTokenType = Me.ListBox.Items(e.Index).Tag


        Select Case liTokenType
            Case Is = FEQL.TokenType.MODELELEMENTNAME, VAQL.TokenType.MODELELEMENTNAME
                ForeColor = Color.FromArgb(76, 153, 0)
            Case Is = VAQL.TokenType.PREDICATEPART, FEQL.TokenType.PREDICATE
                ForeColor = Color.FromArgb(153, 0, 153)
        End Select

        Select Case liTokenType.ToString Like "KEYWD*"
            Case Is = True
                ForeColor = Color.Blue
        End Select

        Select Case True
            Case e.Index = Me.ListBox.SelectedIndex
                ForeColor = Color.White
            Case Else
                'ForeColor = Color.Black
        End Select

        e.DrawFocusRectangle()
        e.Graphics.DrawString(CurrentText, Me.ListBox.Font, New SolidBrush(ForeColor), e.Bounds, StringFormat.GenericDefault)

    End Sub

    Private Sub MeasureItem(ByVal sender As Object, ByVal e As MeasureItemEventArgs) Handles ListBox.MeasureItem
        e.ItemHeight = ListBox.DefaultItemHeight + 5
    End Sub

    ''==============================================================
    ''From SackExchange for above
    'Class SurroundingClass
    '    Private Sub DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs)
    '        e.DrawBackground()
    '        e.DrawFocusRectangle()
    '        e.Graphics.DrawString(Data(e.Index), New Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold), New SolidBrush(Color(e.Index)), e.Bounds)
    '    End Sub

    '    Private Sub MeasureItem(ByVal sender As Object, ByVal e As MeasureItemEventArgs)
    '        e.ItemHeight = 25
    '    End Sub
    'End Class
    ''==============================================================

    Private Sub ListBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox.GotFocus

        Me.Opacity = 1

    End Sub

    Private Sub ListBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox.KeyDown

        Try
            If e.KeyCode = Keys.Escape Then
                e.Handled = True
                Me.zoTextEditor.Focus()
                Me.Hide()
            End If

            If ((e.KeyCode = Keys.Enter) Or (e.KeyCode = Keys.Space) Or (e.KeyCode = Keys.Tab)) And (Me.ListBox.SelectedIndex >= 0) Then
                Call Me.processKeyDown()
                e.Handled = True
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub processKeyDown()

        Try
            Dim liInd As Integer
            Dim lsSubString As String = ""
            Dim liRemoveFromPosition As Integer = -1

            If Me.zoTextEditor.Text.Length = 0 Then
                '-------------------
                'Nothing to remove
                '-------------------
            Else
                If Me.zoTextEditor.Text.Substring(Me.zoTextEditor.Text.Length - 1, 1) = " " Then
                    '---------------------
                    'Don't remove spaces
                    '---------------------
                ElseIf Me.ListBox.SelectedItem.ToString.Length = 1 Then
                    If Me.zoTextEditor.Text.Substring(Me.zoTextEditor.Text.Length - 1, 1) = Me.ListBox.SelectedItem.ToString Then
                        liRemoveFromPosition = Me.zoTextEditor.Text.Length - 1
                    End If
                Else
                    For liInd = Me.ListBox.SelectedItem.ToString.Length - 1 To 0 Step -1
                        lsSubString = Me.ListBox.SelectedItem.ToString.Substring(0, liInd + 1)
                        If Me.zoTextEditor.Text.LastIndexOf(lsSubString) >= 0 Then
                            If Me.zoTextEditor.Text.LastIndexOf(lsSubString) + lsSubString.Length = Me.zoTextEditor.Text.Length Then
                                liRemoveFromPosition = Me.zoTextEditor.Text.LastIndexOf(lsSubString)
                                Exit For
                            End If
                        End If
                    Next
                End If

                If liRemoveFromPosition >= 0 Then
                    Me.zoTextEditor.SelectionProtected = False
                    Dim lsOldText = Me.zoTextEditor.Text
                    Me.zoTextEditor.Text = ""
                    If (Me.zoTextEditor.Text.Length - liRemoveFromPosition) <= Me.ListBox.SelectedItem.ToString.Length Then
                        Me.zoTextEditor.Text = lsOldText.Remove(liRemoveFromPosition, lsSubString.Length)
                    End If
                End If

            End If

            Me.zoTextEditor.SelectionProtected = False
            Me.zoTextEditor.SelectionStart = Me.zoTextEditor.Text.Length
            Me.zoTextEditor.AppendText(Trim(Me.ListBox.SelectedItem.ToString) & " ") 'Text.AppendString
            Me.zoTextEditor.SelectionColor = Me.zoTextEditor.ForeColor

            Me.Hide()

            Me.zoTextEditor.Focus()

            Me.zsIntellisenseBuffer = Trim(Me.ListBox.SelectedItem.ToString)
            Select Case Me.zrCallingForm.Name
                Case Is = "frmFactEngine"
                    CType(Me.zrCallingForm, frmFactEngine).ProcessAutoComplete(New KeyEventArgs(Keys.Space))
            End Select


            'Me.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox

        Catch ex As Exception

        End Try

    End Sub

    Private Sub frmAutoComplete_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        'Me.TopMost = True
    End Sub

    Private Sub ListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox.SelectedIndexChanged

        Me.ListBox.Refresh()

    End Sub

    Private Sub ListBox_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles ListBox.PreviewKeyDown

        If e.KeyCode = Keys.Tab Then
            Call Me.processKeyDown()
        End If

    End Sub

    Private Sub roundCorners(obj As Form)

        obj.FormBorderStyle = FormBorderStyle.None
        obj.BackColor = Color.GhostWhite

        Dim DGP As New Drawing2D.GraphicsPath
        DGP.StartFigure()
        'top left corner
        DGP.AddArc(New Rectangle(0, 0, 40, 40), 180, 90)
        DGP.AddLine(40, 0, obj.Width - 40, 0)

        'top right corner
        DGP.AddArc(New Rectangle(obj.Width - 40, 0, 40, 40), -90, 90)
        DGP.AddLine(obj.Width, 40, obj.Width, obj.Height - 40)

        'buttom right corner
        DGP.AddArc(New Rectangle(obj.Width - 40, obj.Height - 40, 40, 40), 0, 90)
        DGP.AddLine(obj.Width - 40, obj.Height, 40, obj.Height)

        'buttom left corner
        DGP.AddArc(New Rectangle(0, obj.Height - 40, 40, 40), 90, 90)
        DGP.CloseFigure()

        obj.Region = New Region(DGP)

    End Sub

End Class