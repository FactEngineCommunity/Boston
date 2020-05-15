Imports System.Reflection

Public Class class_richtext_cell
    Inherits DataGridViewTextBoxCell
    Implements IDisposable

    Public term_list As New List(Of String)
    Public Scanner As FTR.Scanner
    Public Parser As FTR.Parser
    Public RichTextBox As New RichTextBox
    Public FTRHighlighter As FTR.TextHighlighter
    'See InitializeEditingControl below (for commenting out FTRHighlighter)
    'See Me.Dispose below/bottom (for commenting out FTRHighlighter)

    Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)

        ' Set the value of the editing control to the current cell value.
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)

        Dim ctl As tRichtextEditingControl = CType(DataGridView.EditingControl, tRichtextEditingControl)

        ctl.Text = CType(Me.Value, String)
        Me.Tag = ctl

        Me.RichTextBox = ctl

        Me.Scanner = New FTR.Scanner
        Me.Parser = New FTR.Parser(Me.Scanner)
        Me.FTRHighlighter = New FTR.TextHighlighter(Me.RichTextBox, Me.Scanner, Me.Parser)
        Call Me.FTRHighlighter.HighlightText()

    End Sub

    Public Overrides ReadOnly Property EditType() As Type
        Get
            ' Return the type of the editing contol that RichTextCell uses.
            Return GetType(tRichtextEditingControl)
        End Get
    End Property

    Public Overrides ReadOnly Property ValueType() As Type
        Get
            ' Return the type of the value that RichTextCell contains.
            Return GetType(String)
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultNewRowValue() As Object
        Get
            'Empty String is default value.
            Return ""
        End Get
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            MyBase.Dispose(disposing)
            If IsSomething(Me.FTRHighlighter) Then
                Call Me.FTRHighlighter.Dispose()
                Me.Scanner = Nothing
                Me.Parser = Nothing
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub


End Class
