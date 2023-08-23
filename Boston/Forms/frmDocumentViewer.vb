Imports System.Reflection
Imports PdfiumViewer
Imports System.IO

Public Class frmPDFDocumentViewer


    Public msDocumentFilePath As String = Nothing
    Public miPageNumber As Integer = 0
    Public msObjectTypeName As String = Nothing

    Private _searchManager As PdfSearchManager
    Private _findDirty As Boolean

    Private Sub frmDocumentViewer_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            Call Me.SetupForm()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub SetupForm()

        Try
            Me.TableLayoutPanel1.Controls.Add(PdfViewer, 0, 1)
            PdfViewer.Dock = DockStyle.Fill

            Dim loBytes() As Byte = {}
            Try
                loBytes = System.IO.File.ReadAllBytes(Me.msDocumentFilePath)
            Catch
                Boston.ShowFlashCard("Check to see that the Document Name and Location/Path are correct.", Color.LightGray, 2500, 10)
                Exit Sub
            End Try
            Dim loStream = New MemoryStream(loBytes)

                Dim lrPDFDocument As PdfDocument = PdfDocument.Load(loStream)

                PdfViewer.Document = lrPDFDocument

                AddHandler PdfViewer.Renderer.DisplayRectangleChanged, AddressOf Me.Renderer_DisplayRectangleChanged

                'Page
                Me.TimerGotoPage.Enabled = True
                Me.TimerGotoPage.Start()

#Region "Search"
                _searchManager = New PdfSearchManager(PdfViewer.Renderer)

                '_matchCase.Checked = _searchManager.MatchCase
                '_matchWholeWord.Checked = _searchManager.MatchWholeWord
                '_highlightAll.Checked = _searchManager.HighlightAllMatches
                _searchManager.HighlightAllMatches = True

                If Me.msObjectTypeName IsNot Nothing Then
                    Call Me.FindText(Me.msObjectTypeName, True)
                End If
#End Region


            Catch ex As Exception
                Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripTextBoxPage_KeyDown(sender As Object, e As KeyEventArgs) Handles ToolStripTextBoxPage.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                e.Handled = True

                Dim liPageNumber As Integer
                If Integer.TryParse(Me.ToolStripTextBoxPage.Text, liPageNumber) Then
                    PdfViewer.Renderer.Page = liPageNumber - 1
                End If

            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub GotoPage(ByVal aiPageNumber As Integer)

        Try
            'CodeSafe
            If aiPageNumber <= 0 Then Exit Sub

            PdfViewer.Renderer.Page = aiPageNumber - 1

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TimerGotoPage_Tick(sender As Object, e As EventArgs) Handles TimerGotoPage.Tick

        Try
            If Me.miPageNumber > 0 Then
                Call Me.GotoPage(Me.miPageNumber)
            End If
            Me.TimerGotoPage.Stop()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub FindText(ByVal asSearchText As String, ByVal abForward As Boolean)

        If Not _searchManager.Search(asSearchText) Then
            Exit Sub
        End If

        Call _searchManager.FindNext(True)

    End Sub

    Private Sub ToolStripButtonGetPageText_Click(sender As Object, e As EventArgs) Handles ToolStripButtonGetPageText.Click

        Try
            With New WaitCursor
                Dim liPageNumber As Integer
                If Not Integer.TryParse(Me.ToolStripTextBoxPage.Text, liPageNumber) Then
                    Exit Sub
                End If

                'CodeSafe
                If liPageNumber - 1 < 0 Then Exit Sub

                Dim lsPageText = Me.PdfViewer.Document.GetPdfText(liPageNumber - 1)

                My.Computer.Clipboard.SetText(lsPageText)

                If My.Computer.Keyboard.AltKeyDown Then
                    Dim dataObject As New DataObject()
                    dataObject.SetText(lsPageText)
                    Clipboard.SetDataObject(dataObject)
                End If

                Boston.ShowFlashCard("Copied the current page to the Clipboard.", Color.MediumSeaGreen, 2500, 10)

            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Renderer_DisplayRectangleChanged(sender As Object, e As EventArgs)
        Try
            Me.ToolStripTextBoxPage.Text = (PdfViewer.Renderer.Page + 1).ToString()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click

        Try
            Me.Hide()
            Me.Close()
            Me.Dispose()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class