Imports System.Reflection

Public Class frmVirtualBusinessAnalyst

    Public mrModel As FBM.Model
    Private mrOpenAIAPI As OpenAI_API.OpenAIAPI
    Private msQuestionsAsked As String = ""

    Private Sub frmVirtualBusinessAnalyst_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

        'Do something with the current 500-word chunk, such as write it to a file or process it in some way             
        Me.mrOpenAIAPI = New OpenAI_API.OpenAIAPI(New OpenAI_API.APIAuthentication(My.Settings.FactEngineOpenAIAPIKey))

    End Sub

    Private Sub SetupForm()

        Try

            Me.LabelModel.Text = Me.mrModel.Name

            Me.TextBoxResponse.Text = "Click on [Ask Questions abot the Model] and I'll interact with you to improve the model."
            Me.TextBoxResponse.Text.AppendLine("Or ask me questions about the model.")
            Me.TextBoxResponse.Text.AppendLine("Or tell me to do other Business Analysis type work with the model.")

            Me.TextBoxResponse.DeselectAll()
            Me.TextBoxResponse.SelectionStart = Me.TextBoxResponse.Text.Length

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonAskQuestions_Click(sender As Object, e As EventArgs) Handles ButtonAskQuestions.Click

        Try
            Call Me.AskQuestionsAboutTheModel()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AskQuestionsAboutTheModel()

        Try
            Dim lsPrompt As String = ""

            lsPrompt = Me.mrModel.GenerateFEKL

            lsPrompt &= vbCrLf & vbCrLf

            lsPrompt &= "Notes on the model:" & vbCrLf &
            "1. An ""IS WHERE"" relationship is a cardinaliity rule and is a many-to-many-(...to many) relationship." & vbCrLf &
            "    E.g. ""Event IS WHERE Person had ExperienceType at Place"" means the Person can have an ExperienceType at multiple Places and multiple ExperiencesType at that same Place." & vbCrLf &
            "    - It also means that a Place can have many instances of an ExperienceType (across different people);" & vbCrLf &
            "    - It also means that a Person can have multiple ExperienceTypes at the same Place." & vbCrLf &
            "    - It also means that a Person can have the same ExperienceType at multiple Places." & vbCrLf &
            "    You need to understand these cardinality rules. Study them carefully. It is like a Primary Key on a Table." & vbCrLf &
            "2. An ""IS WHERE"" relationship reifies a relationship." & vbCrLf &
            "    E.g. ""Event IS WHERE Person had ExperienceType at Place"" means that:" & vbCrLf &
            "        a) The Event has (is related to) that Person;" & vbCrLf &
            "        b) A Person can have multiple Events." & vbCrLf &
            "        c) The 'Event' encapsulates the union of Person, ExperienceType and Place." & vbCrLf &
            "3. An IS IDENTIFIED BY relationship means it is the unique identifier." & vbCrLf &
            "    That means it defines the universally unique identifier." & vbCrLf &
            "    It also means that no matter where the item is listed, it always has that identifier." & vbCrLf & vbCrLf &
            "Ask Me intelligent questions about the model to try And extrapolate more relations/fact types And entities/object types?" & vbCrLf &
            "Please don't explain the model...prompt me with questions that may stimulate me to extend or clarify the model." & vbCrLf &
            "In your questions, feel free to make suggestions on how to extend the model based on your experience of the subject area." & vbCrLf

            If Me.msQuestionsAsked.Trim <> "" Then
                lsPrompt.AppendDoubleLineBreak("Here's questions you have already asked:")
                lsPrompt.AppendDoubleLineBreak(Me.msQuestionsAsked)
            End If

            lsPrompt &= "Limit your questions to " & Me.NumericUpDownQuestionCountLimit.Text & " questions."

            With New WaitCursor
                Dim lrCompletionResult As OpenAI_API.Chat.ChatResult = Boston.GetGPTChatResponse(Me.mrOpenAIAPI, lsPrompt)

                Dim lsGPT3ReturnString = lrCompletionResult.Choices(0).Message.Content  'lrCompletionResult.Completions(0).Text

                Me.TextBoxResponse.Text.AppendDoubleLineBreak("--------------" & vbCrLf & vbCrLf & lsGPT3ReturnString.Replace(vbLf, vbCrLf))

                ' Scroll to the end (caret position)
                Me.TextBoxResponse.SelectionStart = Me.TextBoxResponse.Text.Length
                Me.TextBoxResponse.SelectionLength = 0
                Me.TextBoxResponse.ScrollToCaret()

                Me.msQuestionsAsked &= lsGPT3ReturnString
            End With
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonGO_Click(sender As Object, e As EventArgs) Handles ButtonGO.Click

        Try
            Call Me.GO

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub



    Private Sub ButtonEraser_Click(sender As Object, e As EventArgs) Handles ButtonEraser.Click

        Try
            Me.TextBoxResponse.Text = ""

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub GO()

        Dim lsPrompt As String = ""

            Try
                If Me.TextBoxPrompt.Text.Trim <> "" Then

                    With New WaitCursor

                        lsPrompt = Me.mrModel.GenerateFEKL

#Region "Previous Questions"
                    Dim lsInterimPrompt = lsPrompt & vbCrLf & vbCrLf & "Does this sentence look like I am asking you specifically to ""ask questions"" about the model, rather than telling you to do something, when I say: "
                    lsInterimPrompt.AppendDoubleLineBreak("""" & Me.TextBoxPrompt.Text.Trim & """. Answer ""No"" if I am asking questions 'about' the model.")
                        lsInterimPrompt.AppendDoubleLineBreak("Just answer ""Yes"" or ""No"". Don't add any embelishments or comments or narrative or explanation. Just answer ""Yes"" or ""No"".")

                        Dim lrCompletionResult As OpenAI_API.Chat.ChatResult = Boston.GetGPTChatResponse(Me.mrOpenAIAPI, lsInterimPrompt)

                        Dim lsGPT3ReturnString = lrCompletionResult.Choices(0).Message.Content

                        If lsGPT3ReturnString = "Yes" Then
                            Call Me.AskQuestionsAboutTheModel()
                        End If
#End Region
                    lsPrompt.AppendLine("Please don't make up anything. Be strict and professional about your response.")
                    lsPrompt.AppendLine("Limit your response explicitly to the text provided to you hear about the model.")

                    lsPrompt.AppendDoubleLineBreak("IMPORTANT: If the above does not look like a question or instruction about the model, just say 'I only really talk about the " & Me.mrModel.Name & " Model.'")

                        lsPrompt.AppendDoubleLineBreak("Here's what I want from you: " & vbCrLf & Me.TextBoxPrompt.Text.Trim)

                    Try

                        lrCompletionResult = Boston.GetGPTChatResponse(Me.mrOpenAIAPI, lsPrompt)

                        lsGPT3ReturnString = lrCompletionResult.Choices(0).Message.Content  'lrCompletionResult.Completions(0).Text

                    Catch ex As Exception
                        lsGPT3ReturnString = ""
                    End Try

                    Me.TextBoxResponse.Text.AppendDoubleLineBreak("--------------" & vbCrLf & vbCrLf & lsGPT3ReturnString.Replace(vbLf, vbCrLf))
                    Me.TextBoxResponse.Text &= vbCrLf
                    Me.TextBoxPrompt.Text = ""

                    ' Scroll to the end (caret position)
                    Me.TextBoxResponse.SelectionStart = Me.TextBoxResponse.Text.Length
                        Me.TextBoxResponse.SelectionLength = 0
                        Me.TextBoxResponse.ScrollToCaret()

                    End With

                End If

            Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxPrompt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxPrompt.KeyPress

        Try
            ' Check if the "Enter" key was pressed (key code 13)
            If e.KeyChar = vbCr Then
                ' Call the GO() function
                Call Me.GO()
                ' Prevent further processing of the "Enter" key
                e.Handled = True

                Me.TextBoxPrompt.Text = ""
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SpeakSelectedTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpeakSelectedTextToolStripMenuItem.Click

        Try
            Dim larVoice As List(Of BackCast.Voice) = Boston.GetElevenLabsVoices("5bf9e5dd50a7c5a519ecb8249cd097b3")

            Dim lrUtterance As New BackCast.Utterance()

            Dim lrVoice As BackCast.Voice = larVoice.Find(Function(x) x.name = "Aaron")
            lrUtterance.Voice = lrVoice
            lrUtterance.Text = Me.TextBoxResponse.SelectedText.Trim.Replace("""", "'").Replace("[", "").Replace("]", "").Replace(vbCrLf, " ")
            lrUtterance.Voice.SimilarityBoost = 0.5
            lrUtterance.Voice.Stability = 0.5

            Call Boston.GetElevenLabsSpeech(lrUtterance.Text, lrUtterance.Voice, lrUtterance, False)


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxResponse_MouseDown(sender As Object, e As MouseEventArgs) Handles TextBoxResponse.MouseDown

        Try
            ' Check if the [Alt] key is pressed and there is selected text
            If Control.ModifierKeys = Keys.Alt AndAlso e.Button = MouseButtons.Left Then
                ' Display your custom ContextMenuStripBusinessAnalyst context menu
                Me.TextBoxResponse.ContextMenu.Show(Me.TextBoxResponse, e.Location)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub
End Class