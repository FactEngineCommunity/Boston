Imports System.Reflection

Public Class frmAutoComplete

    'NB There is a DateTimePicker hidden under the listbox    

    Private zoTextEditor As RichTextBox
    Public zrCallingForm As Object
    Public zsIntellisenseBuffer As String
    Public moBackgroundWorker As System.ComponentModel.BackgroundWorker

    Public OpacityValue As Single = 0.8
    Public TransparencyColour As System.Drawing.Color = Color.White
    Public mbSpaceActionEqualsTabAction As Boolean = False


    Public Sub New(ByRef aoTextEditor As RichTextBox)

        Me.zoTextEditor = aoTextEditor

        Call InitializeComponent()

    End Sub

    Private Sub frmAutoComplete_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Hide the DateTimePicker. Calling code must display
        Me.DateTimePicker.Visible = False
        Me.DateTimePicker.SendToBack()

        Me.FormBorderStyle = FormBorderStyle.None
        Me.ShowInTaskbar = False
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
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(210, 210, 210)), e.Bounds)
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

        Dim lrFEQLTokenType = CType(Me.ListBox.Items(e.Index), tComboboxItem)
        If lrFEQLTokenType.Tag = FEQL.TokenType.PREDICATE Then
            Dim liStringWidth = e.Graphics.MeasureString(CurrentText, Me.ListBox.Font).Width
            Dim lrRectangle = New Rectangle(e.Bounds.X + liStringWidth + 2, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height)
            e.Graphics.DrawString(lrFEQLTokenType.ItemData, Me.ListBox.Font, New SolidBrush(Color.FromArgb(158, 158, 158)), lrRectangle, StringFormat.GenericDefault)
        End If

    End Sub

    Private Sub MeasureItem(ByVal sender As Object, ByVal e As MeasureItemEventArgs) Handles ListBox.MeasureItem
        Dim font As Font = Me.ListBox.Font
        e.ItemHeight = ListBox.DefaultItemHeight + 5
        e.ItemWidth = e.Graphics.MeasureString(font.Name, font).Width
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

            Select Case e.KeyCode
                Case Is = Keys.Space
                    Call Me.processKeyDown(publicConstantsAutoComplete.pcenumACActionType.Space)
                    e.Handled = True
                    Me.zoTextEditor.Focus()

                Case Is = Keys.Enter,
                          Keys.Space,
                          Keys.Tab
                    Call Me.processKeyDown(publicConstantsAutoComplete.pcenumACActionType.None)
                    e.Handled = True
                    Me.zoTextEditor.Focus()
                Case Is = Keys.A
                    Call Me.processKeyDown(publicConstantsAutoComplete.pcenumACActionType.A)
                    e.Handled = True
                    Me.zoTextEditor.Focus()
                Case Is = Keys.N
                    Call Me.processKeyDown(publicConstantsAutoComplete.pcenumACActionType.NodePropertyIdentification)
                    e.Handled = True
                    Me.zoTextEditor.Focus()
                Case Is = Keys.T
                    Call Me.processKeyDown(publicConstantsAutoComplete.pcenumACActionType.THAT)
                    e.Handled = True
                    Me.zoTextEditor.Focus()
                Case Is = Keys.W
                    Call Me.processKeyDown(publicConstantsAutoComplete.pcenumACActionType.WHICH)
                    e.Handled = True
                    Me.zoTextEditor.Focus()
                Case Is = Keys.M
                    Call Me.processKeyDown(publicConstantsAutoComplete.pcenumACActionType.ModelElement)
                    e.Handled = True
                    Me.zoTextEditor.Focus()
            End Select

            '========================================================
            'DateTimePicker
            'Just to be sure...hide and send to back.
            Me.DateTimePicker.Visible = False
            Me.DateTimePicker.SendToBack()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub processKeyDown(Optional ByRef aiActionType As publicConstantsAutoComplete.pcenumACActionType = Nothing)

        Try
            If aiActionType = pcenumACActionType.Space Then
                If Me.mbSpaceActionEqualsTabAction Then
                    aiActionType = pcenumACActionType.Tab
                End If
            ElseIf aiActionType <> pcenumACActionType.None And Me.Owner.GetType = frmToolboxBrainBox.GetType Then
                Exit Sub
            End If

            If Me.ListBox.SelectedIndex >= 0 Then
                Dim liInd As Integer
                Dim lsSubString As String = ""
                Dim liRemoveFromPosition As Integer = -1
                Dim lsSelectedItem As String = ""

                If Me.zoTextEditor.Text.Length = 0 Then
                    '-------------------
                    'Nothing to remove
                    '-------------------
                Else
                    Select Case aiActionType
                        Case Is = publicConstantsAutoComplete.pcenumACActionType.None
                            lsSelectedItem = Me.ListBox.SelectedItem.ToString
                        Case Is = publicConstantsAutoComplete.pcenumACActionType.A
                            lsSelectedItem = Me.ListBox.SelectedItem.ToString & " A " & Trim(Me.ListBox.SelectedItem.ItemData) & " "
                        Case Is = publicConstantsAutoComplete.pcenumACActionType.ModelElement
                            lsSelectedItem = Trim(Me.ListBox.SelectedItem.ItemData) & " "
                        Case Is = publicConstantsAutoComplete.pcenumACActionType.NodePropertyIdentification
                            If Me.ListBox.SelectedItem.ItemData.Contains("-") Then
                                lsSelectedItem = Me.ListBox.SelectedItem.ToString & " " & Me.ListBox.SelectedItem.ItemData.ToString.Split("-").First & "-(" & Me.ListBox.SelectedItem.ItemData.ToString.Split("-").Last & ":'"
                            Else
                                If Me.ListBox.SelectedItem.ToString = Me.ListBox.SelectedItem.ItemData Then
                                    lsSelectedItem = "(" & Trim(Me.ListBox.SelectedItem.ItemData) & ":'"
                                Else
                                    lsSelectedItem = Me.ListBox.SelectedItem.ToString & " (" & Trim(Me.ListBox.SelectedItem.ItemData) & ":'"
                                End If
                            End If
                        Case Is = publicConstantsAutoComplete.pcenumACActionType.Space
                            lsSelectedItem = Me.ListBox.SelectedItem.ToString & " " & Trim(Me.ListBox.SelectedItem.ItemData) & " "
                        Case Is = publicConstantsAutoComplete.pcenumACActionType.Tab
                            lsSelectedItem = Me.ListBox.SelectedItem.ToString & " "
                        Case Is = publicConstantsAutoComplete.pcenumACActionType.THAT
                            lsSelectedItem = " THAT " & Me.ListBox.SelectedItem.ToString & " A " & Trim(Me.ListBox.SelectedItem.ItemData)
                        Case Is = publicConstantsAutoComplete.pcenumACActionType.WHICH
                            lsSelectedItem = Me.ListBox.SelectedItem.ToString & " WHICH " & Trim(Me.ListBox.SelectedItem.ItemData)
                    End Select

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
                            If Me.zoTextEditor.Text.LastIndexOf(Me.ListBox.SelectedItem.ToString.Substring(0, liInd + 1)) >= 0 Then
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

                '20201112-VM-Changing to insert at selected position

                If (Me.zoTextEditor.SelectionStart <> 0) And (Me.zoTextEditor.SelectionStart = Me.zoTextEditor.Text.Length) Then
                    Me.zoTextEditor.SelectionStart = Me.zoTextEditor.Text.Length
                    Me.zoTextEditor.AppendText(lsSelectedItem)
                ElseIf Me.zoTextEditor.SelectionStart = 0 Then
                    Me.zoTextEditor.SelectionStart = Me.zoTextEditor.Text.Length
                    Me.zoTextEditor.AppendText(lsSelectedItem)
                Else
                    Me.zoTextEditor.SelectionLength = 0
                    Me.zoTextEditor.SelectedText = lsSelectedItem
                End If

                Me.zoTextEditor.SelectionColor = Me.zoTextEditor.ForeColor

                Me.Hide()

                Me.zoTextEditor.Focus()

                Me.zsIntellisenseBuffer = Trim(Me.ListBox.SelectedItem.ToString)
                Select Case Me.zrCallingForm.Name
                    Case Is = "frmFactEngine"
                        CType(Me.zrCallingForm, frmFactEngine).ProcessAutoComplete(New KeyEventArgs(Keys.Space))
                End Select

                'Me.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub frmAutoComplete_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        'Me.TopMost = True
    End Sub

    Private Sub ListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox.SelectedIndexChanged

        Try
            Me.ListBox.Refresh()

            Dim lsMessage As String

            If Me.ListBox.SelectedIndex < 0 Then Exit Sub

            lsMessage = "Press [A] to add '" & Me.ListBox.SelectedItem.ToString & " A " & Me.ListBox.SelectedItem.ItemData & "'"
            lsMessage &= vbCrLf & "Press [T] to add 'THAT " & Me.ListBox.SelectedItem.ToString & " A " & Me.ListBox.SelectedItem.ItemData & "'"
            lsMessage &= vbCrLf & "Press [N] to add '" & Me.ListBox.SelectedItem.ToString & " (" & Me.ListBox.SelectedItem.ItemData & ":'"
            lsMessage &= vbCrLf & "Press [W] to add '" & Me.ListBox.SelectedItem.ToString & " WHICH " & Me.ListBox.SelectedItem.ItemData & "'"

            If Me.moBackgroundWorker IsNot Nothing Then
                Dim lrProgressObject As New ProgressObject(False, lsMessage, True)
                Me.moBackgroundWorker.ReportProgress(0, lrProgressObject)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ListBox_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles ListBox.PreviewKeyDown

        Select Case e.KeyCode
            Case Is = Keys.Tab
                Call Me.processKeyDown(pcenumACActionType.None)
        End Select

    End Sub

    Private Sub roundCorners(obj As Form)

        'obj.FormBorderStyle = FormBorderStyle.None

        obj.BackColor = Me.BackColor 'Color.GhostWhite

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

    Private Sub DateTimePicker_DateSelected(adDate As Date) Handles DateTimePicker.DateSelected

        Me.zoTextEditor.SelectionProtected = False

        '20201112-VM-Changing to insert at selected position

        If (Me.zoTextEditor.SelectionStart <> 0) And (Me.zoTextEditor.SelectionStart = Me.zoTextEditor.Text.Length) Then
            Me.zoTextEditor.SelectionStart = Me.zoTextEditor.Text.Length
            Me.zoTextEditor.AppendText(adDate.ToString("yyyy-MM-dd"))
        ElseIf Me.zoTextEditor.SelectionStart = 0 Then
            Me.zoTextEditor.SelectionStart = Me.zoTextEditor.Text.Length
            Me.zoTextEditor.AppendText(adDate.ToString("yyyy-MM-dd"))
        Else
            Me.zoTextEditor.SelectionLength = 0
            Me.zoTextEditor.SelectedText = adDate.ToString("yyyy-MM-dd")
        End If

        Me.zoTextEditor.SelectionColor = Me.zoTextEditor.ForeColor

        Me.Hide()

        Me.zoTextEditor.Focus()

        'Hide the DateTimePicker
        Me.DateTimePicker.Visible = False
        Me.DateTimePicker.SendToBack()
        Me.ListBox.BringToFront()
        Me.Invalidate()

    End Sub

    Private Sub ListBox_DoubleClick(sender As Object, e As EventArgs) Handles ListBox.DoubleClick

        Try
            Call Me.processKeyDown(publicConstantsAutoComplete.pcenumACActionType.None)
            Me.zoTextEditor.Focus()

            '========================================================
            'DateTimePicker
            'Just to be sure...hide and send to back.
            Me.DateTimePicker.Visible = False
            Me.DateTimePicker.SendToBack()

        Catch ex As Exception

        End Try


    End Sub
End Class