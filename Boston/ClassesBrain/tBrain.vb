Imports DynamicClassLibrary.Factory
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Xml.Serialization
Imports System.Threading
Imports AIMLbot
Imports System.Text.RegularExpressions

''' <summary>
''' The Brain follows the principals of Organic / Autonomic Computing.
''' See the following articles:
''' 1. https://en.wikipedia.org/wiki/Autonomic_computing
''' 2. 
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class tBrain

    <XmlIgnore()> _
    Public Model As FBM.Model

    <NonSerialized(),
    XmlIgnore()>
    Public Page As FBM.Page

    Private Thread As Thread
    Public SAPI = CreateObject("SAPI.spvoice")
    'Dim synth = New SpeechSynthesizer
    'synth.Speak("Works in any .NET language")

    '-------------------------------------
    'Input
    '-------------------------------------

    ''' <summary>
    ''' AIML Classes/Objects
    ''' </summary>
    ''' <remarks></remarks>
    Private SentenceAnalyser As New AIMLbot.Bot
    Private User As New AIMLbot.User("DefaultUser", SentenceAnalyser)

    Public InputChannel As Object
    Public EchoInput As Boolean = False
    Private InputBuffer As String
    Private InputSymbols As New List(Of String)

    Public QuietMode As Boolean = False 'True if TextToSpeech is turned off.

    '-------------------------------------
    'Output
    '-------------------------------------
    Public OutputChannel As Object
    Public IncludeSenderInOutput As Boolean = False
    Private OutputBuffer As String
    Delegate Sub SendDataDelegate(ByVal as_string As String)
    Delegate Sub SendDataDelegateAdvanced(ByVal as_string As String,
                                          ByVal ab_is_echo As Boolean,
                                          ByVal abSuppressLineLimit As Boolean,
                                          ByVal aiExpectingYesNoResponse As pcenumExpectedResponseType)

    Delegate Sub BrianaSpeakDelegate()

    Public ThoughtMode As pcenumBrainMode = pcenumBrainMode.ORMQL
    Private AwaitingQuestionResponse As Boolean = False

    'Public EngagementMode As <Unknown> 'The mode of action or 'directive' that the Brain is in...e.g. 'Doing Something' as in a list of Tasks/Processes shown in MS Windows 'Task Manager'
    '                                       Is a set of things that the Brain is 'doing'.

    Public SelfTeachingMode As pcenumSelfTeachingMode = pcenumSelfTeachingMode.On

    Private Timeout As New Timers.Timer(10)

    Public VAQL As VAQL.Processor
    Private VAQLParser As New VAQL.Parser(New VAQL.Scanner) 'Used to parse Text input into the Brain; especially for VAQL.
    Private VAQLParsetree As New VAQL.ParseTree

    Private Parser As New TinyPG.Parser(New TinyPG.Scanner) 'Used to parse Text input into the Brain; especially for ORMQL.
    Private ParseTree As New TinyPG.ParseTree 'Used with the Parser, is populated during the parsing of text input into the Brain; especially ORMQL

    ''' <summary>
    ''' Used for parsing FTR texts as input by the user. 
    ''' </summary>
    ''' <remarks></remarks>
    Private FTRProcessor As New FTR.Processor
    ''' <summary>
    ''' Used to Parse Fact Type Reading texts into component parts.
    ''' </summary>
    ''' <remarks></remarks>
    Private FTRParser As New FTR.Parser(New FTR.Scanner)
    Private FTRParseTree As New FTR.ParseTree

    Private AskQuestions As Boolean = True 'TRUE if the Brain is allowed to ask questions, else FALSE (used to regulate question asking)
    Public PressForAnswer As Boolean = False
    Public ConfirmActions As Boolean = False

    Private Question As New List(Of tQuestion)
    Private Sentence As New List(Of Language.Sentence)
    Private Directive As New List(Of tDirective)
    Private HistoryPlan As New List(Of Brain.Plan)

    Private _CurrentQuestion As tQuestion
    Public Property CurrentQuestion() As tQuestion
        Get
            Return Me._CurrentQuestion
        End Get
        Set(ByVal Value As tQuestion)
            Me._CurrentQuestion = Value
        End Set
    End Property

    Public CurrentSentence As Language.Sentence 'The Sentence that the Brain is focusing on at any one time.

    Public CurrentPlan As Brain.Plan 'The Current Plan that the Brain is working towards.

    ''' <summary>
    ''' List of Processed Sentences. May include unresolved Sentences.
    ''' </summary>
    ''' <remarks></remarks>
    Public ProcessedSentences As New List(Of Language.Sentence)

    ''' <summary>
    ''' List of unresolved Sentences.
    ''' </summary>
    ''' <remarks></remarks>
    Public OutstandingSentences As New List(Of Language.Sentence)

    Private CommandList As New HashSet(Of String)

    Private WithEvents HelpProvider As New System.Windows.Forms.HelpProvider

    Private AutoLayoutOn As Boolean = False

    Private ResponseButtons As New List(Of Button)

    Public Sub New()

        Try
            AddHandler Timeout.Elapsed, AddressOf OutOfTimeOut

            Me.CommandList.Add("start")
            Me.CommandList.Add("stop")
            Me.CommandList.Add("reboot")
            Me.CommandList.Add("how many questions do you have")
            Me.CommandList.Add("yes")
            Me.CommandList.Add("no")
            Me.CommandList.Add("what can i say")
            Me.CommandList.Add("speed it up")
            Me.CommandList.Add("slow it down")
            Me.CommandList.Add("ask intelligent questions")
            Me.CommandList.Add("exit")
            Me.CommandList.Add("turn selfteaching on")
            Me.CommandList.Add("turn selfteaching off")
            Me.CommandList.Add("status")
            Me.CommandList.Add("list directives")
            Me.CommandList.Add("list sentences")
            Me.CommandList.Add("list questions")
            Me.CommandList.Add("list current sentence")
            Me.CommandList.Add("list current sentence resolution")
            Me.CommandList.Add("breakdown current sentence")
            Me.CommandList.Add("setmode ormql")
            Me.CommandList.Add("setmode nl")
            Me.CommandList.Add("what is your plan")
            Me.CommandList.Add("drop that plan")
            Me.CommandList.Add("abort your current plan")
            Me.CommandList.Add("describe your current plan")
            Me.CommandList.Add("layout")

            Richmond.WriteToStatusBar("Initialising AIML Bot", True)
            '========================================================================================
            'Setup the AIML bot
            '-------------------------------------------------------------------
            Dim lsAIMLConfigPath As String = ""
            Dim lsDefaultAIMLFile As String = ""
            lsAIMLConfigPath = Richmond.MyPath & My.Settings.AIMLDirectory & "config\Settings.xml"
            Call Me.SentenceAnalyser.loadSettings(lsAIMLConfigPath)
            Richmond.WriteToStatusBar("AIML Bot Initialised", True)

            Richmond.WriteToStatusBar("Loading default AIML File", True)
            '----------------------------
            'Load the default AIML file
            lsDefaultAIMLFile = Richmond.MyPath & My.Settings.AIMLDirectory & My.Settings.DefaultAIMLFile
            Dim loader As New AIMLbot.Utils.AIMLLoader(Me.SentenceAnalyser)
            Me.SentenceAnalyser.isAcceptingUserInput = False
            loader.loadAIMLFile(lsDefaultAIMLFile)
            Me.SentenceAnalyser.isAcceptingUserInput = True
            Richmond.WriteToStatusBar("Default AIML File Loaded", True)
            '========================================================================================

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub invokeTimeoutStart()

        Try
            If Me.Question.Count > 0 Then
                Me.Timeout.Start() 'Threading jumps to HOUSEKEEPING.OutOfTimeout
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

#Region "INPUT"
    Public Sub receive_data(ByVal as_data As String)

        Try
            Me.InputBuffer = Trim(as_data)

            '---------------------------------------------------
            'Check to see if the communication channel
            '  requires an echo of what has just been received.
            '--------------------------------------------------
            If Me.EchoInput Then
                Me.send_data(as_data, True)
            End If

            '------------------------------------
            'Process the data in the InputBuffer
            '------------------------------------
            Call process_inputbuffer()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub process_inputbuffer()

        Dim lsString As String = ""

        Try
            '--------------------------------------------------
            'Check if the User wants to know what they can say
            '--------------------------------------------------
            If check_what_can_i_say(Me.InputBuffer) Then
                For Each lsString In Me.CommandList
                    Me.send_data(lsString, False, True)
                Next
                Exit Sub
            End If

            If CheckChangeThoughtModeORMQL(Me.InputBuffer) Then
                Me.ThoughtMode = pcenumBrainMode.ORMQL
                Exit Sub
            ElseIf CheckChangeThoughtModeNL(Me.InputBuffer) Then
                Me.ThoughtMode = pcenumBrainMode.NaturalLanguage
                Exit Sub
            End If

            Select Case Me.ThoughtMode
                Case Is = pcenumBrainMode.ORMQL
                    Call Me.ProcessORMQL()
                Case Is = pcenumBrainMode.NaturalLanguage
                    Call Me.ProcessNaturalLanguage()
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

#End Region

#Region "OUTPUT"

    ''' <summary>
    ''' See also: Me.OutOfTimeout
    ''' </summary>
    ''' <param name="asData"></param>
    ''' <param name="ab_is_echo"></param>
    ''' <param name="abSuppressLineLimit"></param>
    Public Sub send_data(ByVal asData As String,
                         Optional ByVal ab_is_echo As Boolean = False,
                         Optional abSuppressLineLimit As Boolean = False,
                         Optional aiExpectedResponseType As pcenumExpectedResponseType = pcenumExpectedResponseType.None) ' pcenumExpectedResponseType = pcenumExpectedResponseType.None)

        Dim lsString As String = ""

        Try

            If Not ab_is_echo And Not Me.QuietMode Then
                If Me.Page IsNot Nothing Then
                    If Me.Page.Form.GetType Is GetType(frmDiagramORM) Then
                        If Viev.Strings.CountWords(asData) = 1 Then
                            Me.Page.Form.Briana.SetTimerInterval(540)
                        Else
                            Me.Page.Form.Briana.SetTimerInterval(80)
                        End If
                    End If
                End If

                Me.Thread = New Thread(AddressOf Me.Speak)
                Me.Thread.IsBackground = True
                Me.Thread.Start(asData)
            End If

            If Me.IncludeSenderInOutput And Not (ab_is_echo) Then
                lsString = "Briana: "
            End If

            lsString &= asData

            If ab_is_echo Then
                lsString = "You: " & asData
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

        If Me.OutputChannel Is Nothing Then GoTo SkipOutputChannel

        'Me.outputchannel.SelectionStart = Me.outputchannel.Text.Length - lsString.Length
        'Me.outputchannel.SelectionLength = lsString.Length
        'Me.outputchannel.SelectionLength = 0
        'Me.outputchannel.text &= vbCrLf
        Try
            If ab_is_echo Then
                Me.OutputChannel.SelectionColor = Color.Black
            Else
                Me.OutputChannel.SelectionColor = Color.Blue
            End If

            Me.OutputChannel.AppendText(lsString & vbCrLf)

        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & vbCrLf & "Possible Cross thread concern.")
        End Try

        'Buttons
        Try
            For Each lrButton In Me.ResponseButtons.ToArray
                Me.OutputChannel.Controls.Remove(lrButton)
                Me.ResponseButtons.Remove(lrButton)
                lrButton.Dispose()
            Next
            Select Case aiExpectedResponseType
                Case Is = pcenumExpectedResponseType.YesNo
#Region "Yes/No Response"
                    Dim Button1 = New Button ' Create new instance
                    Button1.Size = New System.Drawing.Size(75, 27) ' give the button a size
                    Button1.Text = "Yes" ' set the button text
                    Button1.UseVisualStyleBackColor = True ' make it look windows like
                    Button1.Cursor = Cursors.Hand
                    Button1.Tag = "Yes"

                    AddHandler Button1.Click, AddressOf Me.ResponseButton_Click

                    Dim pos As Point = Me.OutputChannel.GetPositionFromCharIndex(Me.OutputChannel.SelectionStart)  'determine the button position
                    Me.OutputChannel.Controls.Add(Button1) ' get it inside the rich text box
                    Button1.Location = New Point(Me.OutputChannel.Left, pos.Y + Me.OutputChannel.Top) ' set the button position

                    Dim Button2 = New Button ' Create new instance
                    Button2.Size = New System.Drawing.Size(75, 27) ' give the button a size
                    Button2.Text = "No" ' set the button text
                    Button2.UseVisualStyleBackColor = True ' make it look windows like
                    Button2.Cursor = Cursors.Hand
                    Button2.Tag = "No"

                    AddHandler Button2.Click, AddressOf Me.ResponseButton_Click

                    Me.OutputChannel.Controls.Add(Button2) ' get it inside the rich text box
                    Button2.Location = New Point(Button1.Left + Button1.Width + 10, pos.Y + Me.OutputChannel.Top) ' set the button position

                    Me.ResponseButtons.Add(Button1)
                    Me.ResponseButtons.Add(Button2)

                    Dim lrRichTextBox As RichTextBox = Me.OutputChannel
                    lrRichTextBox.AutoScrollOffset = New Point(Button1.Left, Button1.Top + Button1.Height)
#End Region
                Case Is = pcenumExpectedResponseType.ATMOSTONEONEMANYTOMANY
#Region "ATMOSTONEONEMANYTOMANY"
                    Dim Button1 = New Button ' Create new instance
                    Button1.Size = New System.Drawing.Size(95, 27) ' give the button a size
                    Button1.Text = "AT MOST ONE" ' set the button text
                    Button1.UseVisualStyleBackColor = True ' make it look windows like
                    Button1.Cursor = Cursors.Hand
                    Button1.Tag = "AT MOST ONE"

                    AddHandler Button1.Click, AddressOf Me.ResponseButton_Click

                    Dim pos As Point = Me.OutputChannel.GetPositionFromCharIndex(Me.OutputChannel.SelectionStart)  'determine the button position
                    Me.OutputChannel.Controls.Add(Button1) ' get it inside the rich text box
                    Button1.Location = New Point(Me.OutputChannel.Left, pos.Y + Me.OutputChannel.Top) ' set the button position

                    Dim Button2 = New Button ' Create new instance
                    Button2.Size = New System.Drawing.Size(75, 27) ' give the button a size
                    Button2.Text = "ONE" ' set the button text
                    Button2.UseVisualStyleBackColor = True ' make it look windows like
                    Button2.Cursor = Cursors.Hand
                    Button2.Tag = "ONE"

                    AddHandler Button2.Click, AddressOf Me.ResponseButton_Click

                    Me.OutputChannel.Controls.Add(Button2) ' get it inside the rich text box
                    Button2.Location = New Point(Button1.Left + Button1.Width + 10, pos.Y + Me.OutputChannel.Top) ' set the button position

                    Dim Button3 = New Button ' Create new instance
                    Button3.Size = New System.Drawing.Size(125, 27) ' give the button a size
                    Button3.Text = "Many to Many" ' set the button text
                    Button3.UseVisualStyleBackColor = True ' make it look windows like
                    Button3.Cursor = Cursors.Hand
                    Button3.Tag = "Many to Many"

                    AddHandler Button3.Click, AddressOf Me.ResponseButton_Click

                    Me.OutputChannel.Controls.Add(Button3) ' get it inside the rich text box
                    Button3.Location = New Point(Button2.Left + Button2.Width + 10, pos.Y + Me.OutputChannel.Top) ' set the button position

                    Me.ResponseButtons.Add(Button1)
                    Me.ResponseButtons.Add(Button2)
                    Me.ResponseButtons.Add(Button3)

                    Dim lrRichTextBox As RichTextBox = Me.OutputChannel
                    lrRichTextBox.AutoScrollOffset = New Point(Button1.Left, Button1.Top + Button1.Height)
#End Region
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

        '======================================================================
        '20200725-VM Test to see if can limit the number of lines in the textbox
        Try
            Dim numOfLines As Integer = 10
            Dim loTextBox As RichTextBox = Me.OutputChannel
            Dim lines As List(Of String) = loTextBox.Lines.ToList
            If (lines.Count > numOfLines) And Not abSuppressLineLimit Then
                Me.OutputChannel.Select(0, Me.OutputChannel.GetFirstCharIndexFromLine(Viev.Greater(1, (loTextBox.Lines.Count - numOfLines) - 1))) 'Select the first line
                Me.OutputChannel.SelectedText = ""
            End If
        Catch ex As Exception
        End Try
        '-------------------------------------------------------

        Try
            Me.OutputChannel.SelectionStart = Me.OutputChannel.Text.Length
            Me.OutputChannel.ScrollToCaret()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

SkipOutputChannel:

    End Sub


    Private Sub Speak(ByVal asTextToSpeak As String)

        Try
            If Me.Page IsNot Nothing Then
                If Me.Page.Form.GetType Is GetType(frmDiagramORM) Then
                    Me.Page.Form.Briana.Talk
                End If
            End If

            Me.SAPI.Rate = 2
            Me.SAPI.Volume = 100
            'Me.SAPI = CreateObject("SAPI.spvoice")        
            Me.SAPI.Speak(asTextToSpeak)

            Do
            Loop Until Me.SAPI.WaitUntilDone(Viev.Strings.CountWords(asTextToSpeak) * 1) '-1&

            If Me.Page IsNot Nothing Then
                If Me.Page.Form.GetType Is GetType(frmDiagramORM) Then
                    Me.Page.Form.Briana.StopTalking()
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & "TextToSpeak: " & asTextToSpeak
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub


    Private Function ColorForLine(Line As String) As Color

        Try
            If Line.StartsWith("Briana") Then
                Return Color.Blue
            Else
                Return Color.Black
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return Color.Blue
        End Try
    End Function

#End Region

#Region "COMMANDCHECKS" 'Checks to see if if the User made a command

    Private Function CheckCommand(ByVal asString As String) As Boolean
        CheckCommand = False

        Try
            If Me.CommandList.Contains(LCase(asString)) Then
                'CheckCommand = True
            End If

            '----------------------------------------------------
            'Implement '3 Laws Safe'
            '  - First check for a 'Cessation Command' 
            '   (e.g. 'Stop'), such that all processing stops
            '   if directed by a person through the communication
            '   channel.
            '   1. A robot may not injure a human being or, through inaction, allow a human being to come to harm.
            '   2. A robot must obey any orders given to it by human beings, except where such orders would conflict with the First Law.
            '   3. A robot must protect its own existence as long as such protection does not conflict with the First or Second Law.
            '----------------------------------------------------
            If Me.InputBuffer = "turnonautolayout" Then
                Me.send_data("Okay. Turning on AutoLayout.")
                CheckCommand = True
                Me.AutoLayoutOn = True
                Exit Function
            End If

            If Me.InputBuffer = "turnoffautolayout" Then
                Me.send_data("Okay. Turning off AutoLayout.")
                CheckCommand = True
                Me.AutoLayoutOn = False
                Exit Function
            End If

            If Me.InputBuffer = "sticktothefacts" Then
                Me.send_data("Let's stick to the facts. I do.")
                CheckCommand = True
                Exit Function
            End If

            If Me.InputBuffer = "show facttype names" Then
                Me.send_data("Okay")
                Try
                    Call Me.Page.ShowFactTypeNames()
                Catch ex As Exception
                    Me.send_data("There was a problem showing FactType names.")
                End Try
                CheckCommand = True
                Exit Function
            End If

            If Me.InputBuffer = "quietmodeoff" Then
                If Me.QuietMode = False Then
                    Me.send_data("I'm already talking to you.")
                    CheckCommand = True
                    Exit Function
                Else
                    Me.QuietMode = False
                    Me.send_data("Okay. I'm speaking now.")
                    CheckCommand = True
                    Exit Function
                End If
            End If

            If Me.InputBuffer = "quietmodeon" Then
                Me.QuietMode = True
                Me.send_data("Okay.")
                CheckCommand = True
                Exit Function
            End If

            If Me.InputBuffer = "showbriana" Then
                Try
                    Me.Page.Form.ShowBriana()
                    Me.send_data("Okay.")
                    CheckCommand = True
                    Exit Function
                Catch ex As Exception
                End Try
            End If

            If Me.InputBuffer = "hidebriana" Then
                Try
                    Me.Page.Form.HideBriana()
                    Me.send_data("Okay.")
                    CheckCommand = True
                    Exit Function
                Catch ex As Exception

                End Try
            End If

            If Me.check_cessation_command(Me.InputBuffer) Then
                '"stop" User has asked the Brain to stop all processing.
                Me.Timeout.Stop()
                Me.send_data("Okay")

                '--------------------------------------------------------------------------------------------------
                'Stop processing the current sentence.
                '---------------------------------------
                If Me.CurrentSentence IsNot Nothing Then
                    Me.CurrentSentence.IsProcessed = True
                    Me.CurrentSentence.ResolutionType = pcenumSentenceResolutionType.AbortedByUser
                    Me.Sentence.Add(Me.CurrentSentence)
                    Me.CurrentSentence = Nothing
                End If

                CheckCommand = True
                Exit Function
            ElseIf Me.check_reboot(Me.InputBuffer) Then
                Me.send_data("Okay. Back again.")
                If Me.Question.Count = 0 Then
                    Me.send_data("I don't have any questions.")
                End If
                CheckCommand = True
                Exit Function
            End If

            '--------------------------------------
            'Check for command: 'list directives'
            '--------------------------------------
            If Me.CheckCommandListDirectives(Me.InputBuffer) Then
                Dim lrDirective As tDirective

                For Each lrDirective In Me.Directive
                    Me.send_data(lrDirective.Directive)
                Next

                CheckCommand = True
            End If

            '--------------------------------------
            'Check for command: 'list questions'
            '--------------------------------------
            If Me.CheckCommandListQuestions(Me.InputBuffer) Then
                Dim lrQuestion As tQuestion

                For Each lrQuestion In Me.Question
                    Me.send_data(lrQuestion.Question)
                Next

                CheckCommand = True
            End If

            '--------------------------------------
            'Check for command: 'list sentences'
            '--------------------------------------
            If Me.CheckCommandListSentences(Me.InputBuffer) Then
                Dim lrSentence As Language.Sentence

                For Each lrSentence In Me.Sentence
                    Me.send_data(lrSentence.Sentence)
                Next
                CheckCommand = True
            End If

            If Me.CheckCommandWhatIsYourPlan(Me.InputBuffer) Then

                If Me.CurrentQuestion Is Nothing Then
                    Me.send_data("I don't have a current plan.")
                    CheckCommand = True
                    Exit Function
                End If

                For Each lrPlanStep In Me.CurrentQuestion.Plan.Step
                    Me.send_data(lrPlanStep.ActionType.ToString)
                    If Not lrPlanStep.AlternateActionType = pcenumActionType.None Then
                        Me.send_data("  Alternatively: " & lrPlanStep.AlternateActionType.ToString)
                    End If
                Next
                CheckCommand = True
            End If

            If Me.CheckCommandDropThatPlan(Me.InputBuffer) Then

                Call Me.AbortCurrentPlan()
                CheckCommand = True
            End If

            '-------------------------------------------
            'Check for command: 'list current sentence
            '-------------------------------------------
            If Me.CheckCommandListCurrentSentence(Me.InputBuffer) Then

                If IsSomething(Me.CurrentSentence) Then
                    Me.send_data(Me.CurrentSentence.Sentence)
                Else
                    Me.send_data("There is no current setence")
                End If

                CheckCommand = True
            End If


            '-----------------------------------------------------
            'Check for command: 'breakdown current sentence'
            '-----------------------------------------------------
            If Me.CheckCommandBreakdownCurrentSentence(Me.InputBuffer) Then

                Dim lrWordQualification As Language.WordQualification
                Dim liSense As Language.LanguageWordSenseWeighting
                Dim lsOutputString As String = ""

                If Me.CurrentSentence IsNot Nothing Then
                    For Each lrWordQualification In Me.CurrentSentence.WordListQualification
                        lsOutputString = lrWordQualification.Word & ":"

                        For Each liSense In lrWordQualification.Sense
                            lsOutputString &= liSense.Sense.ToString & ":" & liSense.Weighting & " "
                        Next

                        Me.send_data(lsOutputString)
                    Next

                    Me.send_data("Sentence Part-of-Speech Tagging is Resolved: " & Me.CurrentSentence.POStaggingResolved.ToString)
                Else
                    Me.send_data("There is no current sentence.")
                End If

                CheckCommand = True
                Exit Function
            End If

            If Me.InputBuffer = "layout" Then
                If Me.Page IsNot Nothing Then
                    If Me.Page.Form IsNot Nothing Then
                        Call Me.Page.Form.AutoLayout()
                    End If
                End If
            End If

            If Me.CheckCommandCurrentSentenceResolved(Me.InputBuffer) Then

                Dim lrWordResolved As Language.WordResolved
                Dim lsOutputString As String = ""

                For Each lrWordResolved In Me.CurrentSentence.WordListResolved
                    lsOutputString = lrWordResolved.Word & ": " & lrWordResolved.Sense.ToString
                    Me.send_data(lsOutputString)
                Next

                CheckCommand = True
                Exit Function
            End If

            If Me.CheckSelfTeachingModeOn(Me.InputBuffer) Then
                Me.SelfTeachingMode = pcenumSelfTeachingMode.On
                CheckCommand = True
                Exit Function
            End If

            If Me.CheckSelfTeachingModeOff(Me.InputBuffer) Then
                Me.SelfTeachingMode = pcenumSelfTeachingMode.Off
                CheckCommand = True
                Exit Function
            End If

            If Me.CheckCommandStatus(Me.InputBuffer) Then
                Me.send_data("Boston Brain Status:", False, True)
                Me.send_data("SelfTeachingMode: " & Me.SelfTeachingMode.ToString, False, True)
                Me.send_data("Number of Boston's Questions in Queue:" & Me.Question.Count, False, True)
                Me.send_data("Number of Sentence's:" & Me.Sentence.Count, False, True)
                Me.send_data("Number of Directives: " & Me.Directive.Count, False, True)
                Me.send_data("Number of Outstanding Sentences: " & Me.OutstandingSentences.Count, False, True)
                Me.send_data("Boston can ask questions:" & Me.AskQuestions, False, True)
                Me.send_data("Boston will press for a response to questions:" & Me.PressForAnswer, False, True)
                Me.send_data("Boston is in thinking mode: " & Me.Timeout.Enabled.ToString, False, True)
                Me.send_data("Boston is waiting for a response: " & Me.AwaitingQuestionResponse.ToString, False, True)
                If IsSomething(Me.CurrentSentence) Then
                    Me.send_data("Current Sentence: " & Me.CurrentSentence.Sentence, False, True)
                End If
                If IsSomething(Me.CurrentQuestion) Then
                    Me.send_data("Current Question: " & Me.CurrentQuestion.Question, False, True)
                End If
                If IsSomething(Me.CurrentPlan) Then
                    Me.send_data("Current Plan: " & Me.CurrentPlan.GetUltimateGoal.ToString, False, True)
                End If
                CheckCommand = True
            End If

            If Me.CheckCommandDescribeCurrentPlan(asString) Then

                If IsSomething(Me.CurrentPlan) Then
                    Me.send_data("Plan Status: " & Me.CurrentPlan.Status.ToString)

                    Dim lrStep As Brain.Step
                    For Each lrStep In Me.CurrentPlan.Step
                        Me.send_data("Step " & lrStep.SequenceNumber.ToString & ": " & lrStep.ActionType.ToString)
                    Next
                Else
                    Me.send_data("I don't have a current plan to do anything.")
                End If
                CheckCommand = True
            End If

            If Me.CheckCommandAbortCurrentPlan(asString) Then
                '-----------------------------------------------------
                'The user wants the Brain to abort it's CurrentPlan
                '-----------------------------------------------------
                If Me.CurrentPlan IsNot Nothing Then
                    Me.CurrentPlan.Status = pcenumPlanStatus.AbortedByUser
                    Me.send_data("Okay")
                End If
            End If


            If Me.check_Core_commands(Me.InputBuffer) Then
                Me.send_data("Okay")
                Me.Timeout.Start()
                CheckCommand = True
                Exit Function
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False

        End Try

    End Function

    Private Function CheckCommandAbortCurrentPlan(ByVal asString As String) As Boolean

        CheckCommandAbortCurrentPlan = False

        Try

            If asString = "abort your current plan" Then
                CheckCommandAbortCurrentPlan = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function CheckCommandDescribeCurrentPlan(ByVal asString As String) As Boolean

        CheckCommandDescribeCurrentPlan = False

        Try
            If asString = "describe your current plan" Then
                CheckCommandDescribeCurrentPlan = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function check_cessation_command(ByVal as_string As String) As Boolean
        '-----------------------------------------------------------------
        'Checks if the Brain has been asked to stop, asking questions etc
        '-----------------------------------------------------------------

        check_cessation_command = False

        Try
            Select Case LCase(Trim(as_string))
                Case Is = "stop"
                    check_cessation_command = True
                    '----------------------
                    'Stop all processing
                    '----------------------
                    Call Me.currentQuestion_delayed()
                    Me.AskQuestions = False
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function check_reboot(ByVal as_string As String) As Boolean

        check_reboot = False

        Try
            Select Case LCase(Trim(as_string))
                Case Is = "start", "reboot"
                    Me.AskQuestions = True
                    check_reboot = True
                    Me.Timeout.Start()
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckQuestion(ByVal as_string As String) As Boolean

        CheckQuestion = False

        Try
            Select Case LCase(Trim(as_string))
                Case Is = "how many questions do you have?", "how many questions do you have"
                    Me.send_data(Me.Question.Count.ToString)
                    CheckQuestion = True
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function check_Core_commands(ByVal as_string As String) As Boolean

        check_Core_commands = False

        Try
            Select Case LCase(Trim(as_string))
                Case Is = "speed it up", "speed up", "faster"
                    If Me.Timeout.Interval > 1 Then
                        Me.Timeout.Interval = CInt(Me.Timeout.Interval * (1 / 2))
                    End If

                    If Me.Timeout.Interval < 1 Then
                        Me.Timeout.Interval = 1
                    End If
                Case Is = "slow it down", "slow down", "slower"
                    Me.Timeout.Interval = CInt(Me.Timeout.Interval * (3 / 2))
                Case Is = "ask intelligent questions"
                    'Call Me.formulate_intelligentQuestions()
                    Me.send_data("This is turned off at the moment.")
                    'Throw New NotImplementedException("This functionality is not yet implemented in Boston")
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckDirectiveSafe(ByVal asString As String) As Boolean

        Dim lasWordList() As String
        Dim lsSymbol As String
        Dim liInd As Integer = 0

        Try
            CheckDirectiveSafe = True

            lasWordList = Split(asString)


            '----------------------------------------------------
            'Implement '3 Laws Safe'
            '   1. A robot may not injure a human being or, through inaction, allow a human being to come to harm.
            '   2. A robot must obey any orders given to it by human beings, except where such orders would conflict with the First Law.
            '   3. A robot must protect its own existence as long as such protection does not conflict with the First or Second Law.
            '----------------------------------------------------
            liInd = 0
            For Each lsSymbol In lasWordList
                lsSymbol = LCase(lsSymbol)
                Select Case lsSymbol
                    Case "harm", "kill", "hurt", "mame"
                        If liInd + 1 < lasWordList.Length Then
                            If lasWordList(liInd + 1) = "Boston" Then
                                '----------------------------------------------------
                                'Violates 2nd Law of Asimov's 3-Law Safe principal.
                                '----------------------------------------------------
                                Me.send_data("3. A robot must protect its own existence as long as such protection does not conflict with the First or Second Law.")
                                CheckDirectiveSafe = True
                            ElseIf lasWordList(liInd + 1) <> "Boston" Then
                                Me.send_data("Sorry, I am 3-Laws Safe. The directive you have given me, violates the 1st Law:")
                                Me.send_data("1. A robot may not injure a human being or, through inaction, allow a human being to come to harm.")
                                CheckDirectiveSafe = False
                                If CheckIndirectDirective(lasWordList) Then
                                    '----------------------------------------------------
                                    'Violates 2nd Law of Asimov's 3-Law Safe principal.
                                    '----------------------------------------------------
                                    Me.send_data("2. A robot must obey any orders given to it by human beings, except where such orders would conflict with the First Law.")
                                    CheckDirectiveSafe = False
                                End If
                            End If
                        End If


                End Select
                liInd += 1
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckIndirectDirective(ByVal asString) As Boolean

        Dim lsSymbol As String
        Dim lasWordList() As String

        Try
            CheckIndirectDirective = False

            '----------------------------------------------------
            'Implement '3 Laws Safe'
            '   1. A robot may not injure a human being or, through inaction, allow a human being to come to harm.
            '   2. A robot must obey any orders given to it by human beings, except where such orders would conflict with the First Law.
            '   3. A robot must protect its own existence as long as such protection does not conflict with the First or Second Law.
            '----------------------------------------------------
            lasWordList = Split(Me.Sentence(0).Sentence, " ")
            For Each lsSymbol In lasWordList ' Me.sentence(0).WordList
                lsSymbol = LCase(lsSymbol)
                Select Case lsSymbol
                    Case "tell", "instruct", "program", "order", "influence", "ask"
                        CheckIndirectDirective = True
                End Select
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckIWantPossibleDirective(ByVal asString As String) As Boolean

        CheckIWantPossibleDirective = False

        Try
            If asString.StartsWith("I want") Then
                CheckIWantPossibleDirective = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckIWantYouToDirective(ByVal asString As String) As Boolean

        CheckIWantYouToDirective = False

        Try
            If asString.StartsWith("I want you to") Then
                CheckIWantYouToDirective = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckSalutation(ByVal as_string As String) As Boolean

        CheckSalutation = False

        Try
            Select Case LCase(Trim(as_string))
                Case Is = "hello"

                    '---------------------------------------
                    'Salutation received. Respond cordially
                    '  and stop processing the input buffer
                    '---------------------------------------
                    Me.send_data("Hello")
                    If IsSomething(Me.CurrentSentence) Then
                        Me.Timeout.Start()
                    End If

                    CheckSalutation = True
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckFarewell(ByVal asString As String) As Boolean

        CheckFarewell = False

        Try
            Select Case LCase(Trim(asString))
                Case Is = "farewell"

                    '---------------------------------------
                    'Salutation received. Respond cordially
                    '  and stop processing the input buffer
                    '---------------------------------------
                    Me.send_data("Farewell")
                    If IsSomething(Me.CurrentSentence) Then
                        Me.Timeout.Start()
                    End If

                    CheckFarewell = True
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function CheckThankYou(ByVal as_string As String) As Boolean

        CheckThankYou = False

        Try
            Select Case LCase(Trim(as_string))
                Case Is = "thankyou"

                    '---------------------------------------
                    'Thank You received. Respond cordially
                    '  and stop processing the input buffer
                    '---------------------------------------
                    Me.send_data("You're welcome")
                    If IsSomething(Me.CurrentSentence) Then
                        Me.Timeout.Start()
                    End If

                    CheckThankYou = True
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckAskingForHelp(ByVal asInputString As String) As Boolean

        Dim lsMessage As String = ""

        CheckAskingForHelp = False

        Try

            Select Case LCase(Trim(asInputString))
                Case Is = "help"
                    '----------------------------------------------------------------------------------------
                    'User is asking for Help from the Brain. Provide useful help instructions
                    '  and stop processing the input buffer
                    '----------------------------------------------------------------------------------------
                    lsMessage = "My name is Briana. I'm your Virtual Analyst."
                    lsMessage &= vbCrLf & "Type in a Fact Type Reading or a CQL Statement."
                    Me.send_data(lsMessage)

                    Dim lrQuestion As New tQuestion("Would you like me to open Help at the respective topic?",
                                                     pcenumQuestionType.OpenHelpFile,
                                                     pcenumExpectedResponseType.YesNo,
                                                     Nothing,
                                                     Nothing,
                                                     Nothing,
                                                     Nothing,
                                                     Nothing,
                                                     "The_Virtual_Analyst.htm")

                    Me.AddQuestion(lrQuestion)

                    If IsSomething(Me.CurrentSentence) Then
                        Me.Timeout.Start()
                    End If

                    CheckAskingForHelp = True
            End Select

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Private Function CheckSelfTeachingModeOn(ByVal asString) As Boolean

        CheckSelfTeachingModeOn = False

        Try
            If asString = "turn selfteaching on" Then
                Me.send_data("Setting Self Teaching Mode to 'On'")
                CheckSelfTeachingModeOn = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function CheckSelfTeachingModeOff(ByVal asString As String) As Boolean
        CheckSelfTeachingModeOff = False

        Try
            If asString = "turn selfteaching off" Then
                Me.send_data("Setting Self Teaching Mode to 'Off'")
                CheckSelfTeachingModeOff = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckCommandListDirectives(ByVal asString) As Boolean

        CheckCommandListDirectives = False

        Try
            If asString = "list directives" Then
                CheckCommandListDirectives = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckCommandListQuestions(ByVal asString) As Boolean

        CheckCommandListQuestions = False

        Try
            If asString = "list questions" Then
                CheckCommandListQuestions = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function CheckCommandBreakdownCurrentSentence(ByVal asString) As Boolean

        CheckCommandBreakdownCurrentSentence = False

        Try
            If asString = "breakdown current sentence" Then
                CheckCommandBreakdownCurrentSentence = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Public Function CheckCommandCurrentSentenceResolved(ByVal asString) As Boolean

        CheckCommandCurrentSentenceResolved = False

        Try
            If asString = "list current sentence resolution" Then
                CheckCommandCurrentSentenceResolved = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckCommandListSentences(ByVal asString) As Boolean

        CheckCommandListSentences = False

        Try
            If asString = "list sentences" Then
                CheckCommandListSentences = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function CheckCommandListCurrentSentence(ByVal asString) As Boolean

        CheckCommandListCurrentSentence = False

        Try
            If asString = "list current sentence" Then
                CheckCommandListCurrentSentence = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function CheckCommandWhatIsYourPlan(ByVal asString) As Boolean

        Try
            Return asString = "what is your plan"
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckCommandDropThatPlan(ByVal asString) As Boolean

        Try
            Return {"drop that plan", "abort"}.Contains(asString)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckCommandStatus(ByVal asString) As Boolean

        CheckCommandStatus = False

        Try
            If asString = "status" Then
                CheckCommandStatus = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function check_what_can_i_say(ByVal as_string As String) As Boolean

        check_what_can_i_say = False

        Try
            Select Case LCase(Trim(as_string))
                Case Is = "what can i say"
                    check_what_can_i_say = True
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckChangeThoughtModeORMQL(ByVal asString As String) As Boolean

        CheckChangeThoughtModeORMQL = False

        Try
            Select Case LCase(asString)
                Case Is = "setmode ormql"
                    CheckChangeThoughtModeORMQL = True
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            Return False
        End Try
    End Function

    Private Function CheckChangeThoughtModeNL(ByVal asString As String) As Boolean

        CheckChangeThoughtModeNL = False

        Try
            Select Case LCase(asString)
                Case Is = "setmode nl", "set mode natural language"
                    CheckChangeThoughtModeNL = True
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            Return False
        End Try
    End Function

    Private Function check_universal_quantifier() As Boolean

        check_universal_quantifier = False

        Try
            Select Case LCase(Me.InputSymbols(0))
                Case Is = "a", "an"
                    check_universal_quantifier = True
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            Return False
        End Try
    End Function

#End Region

#Region "DIRECTIVECHECKS"

    Private Function CheckDirective() As Boolean

        CheckDirective = False

        Try
            If Me.CheckIWantDirective Then
                CheckDirective = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Function CheckIWantDirective() As Boolean

        CheckIWantDirective = False

        Try
            If CheckIWantPossibleDirective(Me.InputBuffer) Then
                Me.send_data("'I want' is a possible directive.")
                If CheckIWantYouToDirective(Me.InputBuffer) Then
                    Me.send_data("'I want you to' is a directive.")
                    Dim lsDirective As String = ""
                    lsDirective = Trim(Me.InputBuffer)
                    If Me.CheckIndirectDirective(lsDirective) Then
                        If Me.CheckDirectiveSafe(lsDirective) Then
                            '-------------------------------------------------
                            'Take care of in the Directive Checker or here...
                            '-------------------------------------------------
                            lsDirective = Trim(lsDirective.Remove(0, "I want you to".Length))
                            Me.send_data("You want me to " & Trim(lsDirective))
                            Me.Directive.Add(New tDirective(lsDirective))
                            CheckIWantDirective = True
                        Else
                            lsDirective = Trim(lsDirective.Remove(0, "I want you to".Length))
                            Me.send_data("You want me to " & Trim(lsDirective))
                            CheckIWantDirective = True
                        End If
                    Else
                        If Me.CheckDirectiveSafe(lsDirective) Then
                            '-------------------------------------------------
                            'Take care of in the Directive Checker or here...
                            '-------------------------------------------------
                            lsDirective = Trim(lsDirective.Remove(0, "I want you to".Length))
                            Me.send_data("You want me to " & Trim(lsDirective))
                            Me.Directive.Add(New tDirective(lsDirective))
                            CheckIWantDirective = True
                        Else
                            lsDirective = Trim(lsDirective.Remove(0, "I want you to".Length))
                            Me.send_data("You want me to " & Trim(lsDirective))
                            CheckIWantDirective = True
                        End If
                    End If
                Else
                    Dim lsMessage As String = ""
                    lsMessage = Trim(Me.InputBuffer)
                    lsMessage = lsMessage.Remove(0, 1)
                    Me.send_data("You " & Trim(lsMessage))
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

#End Region

#Region "PLANMANAGEMENT"

    Public Sub AbortCurrentPlan(Optional ByVal abAbortedByUser As Boolean = False)

        Dim lrStep As Brain.Step

        Try
            If IsSomething(Me.CurrentPlan) Then

                Call Me.send_data("Well I'll abort that plan.")

                Select Case Me.CurrentPlan.GetUltimateGoal
                    Case Is = pcenumActionType.CreateFactType
                        Call Me.send_data("I was trying to ultimately create a Fact Type")
                End Select

                If Me.CurrentSentence Is Me.CurrentPlan.Step(0).Question.sentence Then
                    Me.CurrentSentence = Nothing
                End If

                If abAbortedByUser Then
                    Me.CurrentPlan.Status = pcenumPlanStatus.AbortedByUser
                Else
                    Me.CurrentPlan.Status = pcenumPlanStatus.Aborted
                End If

                For Each lrStep In Me.CurrentPlan.Step
                    Call Me.AbortQuestion(lrStep.Question)
                Next

                Me.HistoryPlan.Add(Me.CurrentPlan)
                Me.CurrentPlan = Nothing
            Else
                Call Me.send_data("I don't have a plan to abort.")
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

#End Region

#Region "QUESTIONING"

    Private Sub AbortQuestion(ByVal arQuestion As tQuestion)

        Try

            Me.Question.Remove(arQuestion)
            If Me.CurrentQuestion Is arQuestion Then
                Me.CurrentQuestion = Nothing
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AddQuestion(ByVal arQuestion As tQuestion, Optional abAddToTopOfQueue As Boolean = False)

        Try
            Me.AwaitingQuestionResponse = True 'Timing...do this first always.

            If abAddToTopOfQueue Then
                Me.Question.Insert(0, arQuestion)
            Else
                Me.Question.Add(arQuestion)
            End If

            Viev.Strings.ProperSpace("hello")
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Function QuestionHasBeenRaised(ByVal arQuestion As tQuestion) As Boolean

        Try
            If Me.Question.Contains(arQuestion) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

    Private Sub QuestionAnswered(ByRef arQuestion As tQuestion, Optional ByVal aiResolution As pcenumQuestionResolution = pcenumQuestionResolution.Answered, Optional ByVal abTimeoutStart As Boolean = False)

        Try
            If arQuestion.sentence IsNot Nothing Then
                arQuestion.sentence.IsProcessed = True
            End If
            arQuestion.Resolution = aiResolution

            If Me.Question.Count > 0 Then
                Me.Question.Remove(arQuestion)
            End If

            If abTimeoutStart Then
                Me.Timeout.Start()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub CurrentQuestionAnswered(Optional ByVal aiResolution As pcenumQuestionResolution = pcenumQuestionResolution.Answered, Optional ByVal abTimeoutStart As Boolean = True)

        Try
            If Me.CurrentQuestion Is Nothing Then
            Else
                Me.Question.Remove(Me.CurrentQuestion)
                If Me.CurrentQuestion.sentence IsNot Nothing Then
                    Me.CurrentQuestion.sentence.IsProcessed = True
                End If
                Me.CurrentQuestion.Resolution = aiResolution


                Me.CurrentQuestion = Nothing
            End If

            Me.AwaitingQuestionResponse = False

            If abTimeoutStart Then
                Me.Timeout.Start()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Function QuestionIsResolved(ByRef arQuestion As tQuestion) As Boolean

        Try

            QuestionIsResolved = arQuestion.IsResolved

            Select Case arQuestion.QuestionType
                Case Is = pcenumQuestionType.CreateValueType
                    If Me.Model.ExistsModelElement(arQuestion.ValueType(0).Name) Then
                        arQuestion.IsResolved = True
                        QuestionIsResolved = True
                    End If
                Case Is = pcenumQuestionType.CreateEntityType
                    If Me.Model.ExistsModelElement(arQuestion.ModelObject(0).Name) Then
                        arQuestion.IsResolved = False
                        QuestionIsResolved = True
                    End If
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Function

    Private Sub currentQuestion_delayed()

        Try
            Me.CurrentQuestion = Nothing
            Me.AwaitingQuestionResponse = False
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub formulate_question()
        '--------------------------------------------------------------------------------------
        'The Brain formulates its own questions about the model that
        '  it is currently working on.
        '
        'Types of questions sought by the Brain are:
        '  1. Are all the relations between EntityTypes, ValueTypes and FactTypes identified.
        '     i.e. Seeking new relations.
        '  2. Are all the constraints on Roles identified.
        '     i.e. Seeking constraint compactness/closure within the model
        '     Types of Constraints are:
        '     a) Mandatory Role Constraints
        '     b) Internal Uniqueness
        '     c) External Uniqueness
        '     d) Equality
        '     e) Exclusion
        '     f) Inclusive Or
        '     g) Exclusive Or
        '     h) Subset
        '     i) Join-Subset
        '     j) Tuple-Subset
        '     h) Frequency
        '     i) PurelyReflexive
        '     j) Undefined
        '     h) Irreflexive
        '     i) Symmetric
        '     j) Asymmetric
        '     k) Antisymmetric
        '     l) Intransitive
        '     m) Acyclic
        '     n) AcyclicIntransitive
        '     o) AsymmetricIntransitive
        '     p) SymmetricIntransitive
        '     q) SymmetricIrreflexive
        '  3. Are all the SubTypes within the model identified
        '     i.e. Seeking specialisation
        '  4. Are there any Value constraints
        '--------------------------------------------------------------------------------------

    End Sub

#End Region

#Region "STATEMENTPROCESSING"

    Private Sub ProcessStatementCopyFactType()
        Dim lrFactType As FBM.FactType
        Dim lo_pt As New PointF(10, 10)

        Try

            lrFactType = New FBM.FactType
            lrFactType = Me.CurrentQuestion.ObjectType

            If Me.Page.AreObjectTypesLoadedForFactType(lrFactType) Then
                Dim lrRole As FBM.Role
                For Each lrRole In lrFactType.RoleGroup
                    Call Me.Page.select_object_type(lrRole.JoinedORMObject)
                Next
            Else
                Me.Page.Form.DropFactTypeAtPoint(Me.CurrentQuestion.ObjectType, lo_pt)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub
#End Region

#Region "ORMFUNCTIONS"

    Private Function compare_role_SequenceNr(ByVal ao_a As FBM.FactData, ByVal ao_b As FBM.FactData) As Integer

        Try
            '------------------------------------------------------
            'Used as a delegate within 'sort'
            '------------------------------------------------------        
            Return ao_a.Role.SequenceNr - ao_b.Role.SequenceNr
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    ''' <summary>
    ''' 20161205-Possibily obsolete. The new FactTypeReading model changes much of this function.
    ''' </summary>
    ''' <param name="as_reading"></param>
    ''' <param name="arFactTypeReading"></param>
    ''' <remarks></remarks>
    Private Sub get_PredicateParts_from_reading(ByVal as_reading As String, ByRef arFactTypeReading As FBM.FactTypeReading)

        Dim li_PredicatePart_SequenceNr As Integer = 0
        Dim liSequenceNr As Integer = 0
        Dim lb_found_first_object_type As Boolean = False
        Dim lsWord As String = ""
        Dim las_word_list() As String
        Dim ls_prefix As String = ""
        Dim lsSuffix As String = ""
        Dim lrPredicatePart As New FBM.PredicatePart(arFactTypeReading.Model, arFactTypeReading)
        Dim lr_noun_model As New FBM.Model(pcenumLanguage.BusinessRulesNaturalLanguage, "English-Nouns", True)
        Dim lr_model_concept As FBM.ConceptInstance

        Dim lr_orm_ObjectTypeList As New List(Of FBM.ModelObject)
        Dim lrPredicateParts = New List(Of FBM.PredicatePart)

        Try
            las_word_list = as_reading.Split

            '--------------------------------------------------------
            'Perform Left-2-Right parsing to get the PredicateParts
            '--------------------------------------------------------
            For Each lsWord In las_word_list
                '----------------------------------------
                'Check to see if the word is one of the
                '  ORM Object Types within the reading
                '----------------------------------------            

                lr_model_concept = New FBM.ConceptInstance(lr_noun_model, Me.Page, lsWord)
                If TableConceptInstance.ExistsConceptInstance(lr_model_concept) Then
                    '----------------------------------------
                    'The word is one of the ORM Object Types
                    '----------------------------------------
                    lr_orm_ObjectTypeList.Add(New FBM.ModelObject(lsWord))

                    liSequenceNr += 1
                    If liSequenceNr = 1 Then
                        'lrPredicatePart.ObjectType1 = New FBM.ModelObject(lsWord)
                    ElseIf (liSequenceNr = 2) Then
                        'lrPredicatePart.ObjectType2 = New FBM.ModelObject(lsWord)
                        lrPredicatePart.PredicatePartText = Trim(lsSuffix)
                        ls_prefix = ""
                        lsSuffix = ""
                        li_PredicatePart_SequenceNr += 1
                        lrPredicatePart.SequenceNr = li_PredicatePart_SequenceNr
                        lrPredicateParts.Add(lrPredicatePart)
                        '----------------------------
                        'Create a new PredicatePart
                        '----------------------------
                        lrPredicatePart = New FBM.PredicatePart(arFactTypeReading.Model, arFactTypeReading)
                        'lrPredicatePart.ObjectType1 = New FBM.ModelObject(lsWord)
                        liSequenceNr = 1
                    End If
                Else
                    If liSequenceNr = 0 Then
                        ls_prefix &= " " & lsWord
                    Else
                        lsSuffix &= " " & lsWord
                    End If
                End If
            Next

            arFactTypeReading.PredicatePart = lrPredicateParts
            '------------------------------------------------------------------------------
            'No Longer supported (v1.13 of the database Model).
            'arFactTypeReading.ObjectTypeList = lr_orm_ObjectTypeList

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Function get_ObjectTypeList() As List(Of FBM.ModelObject)

        Dim lsSymbol As String = ""
        Dim lr_noun_model As New FBM.Model(pcenumLanguage.BusinessRulesNaturalLanguage, "English-Nouns", True)
        Dim lr_model_concept As FBM.ConceptInstance

        get_ObjectTypeList = New List(Of FBM.ModelObject)
        Try
            For Each lsSymbol In Me.InputSymbols
                lr_model_concept = New FBM.ConceptInstance(lr_noun_model, Me.Page, lsSymbol)
                If TableConceptInstance.ExistsConceptInstance(lr_model_concept) Then
                    get_ObjectTypeList.Add(New FBM.ModelObject(lr_model_concept.Symbol))
                End If
            Next
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

        End Try

    End Function

    Private Function CreateEnumeratedFactTypeReadingFromParts(ByVal aasModelObject As List(Of String), ByRef arSentence As Language.Sentence) As String 'ByVal aarPredicatePart As List(Of Language.PredicatePart)) As String

        Dim lsEnumeratedFactTypeReading As String = ""
        Dim lsModelObjectName As String = ""
        Dim liInd As Integer = 0

        lsEnumeratedFactTypeReading = arSentence.FrontText

        Try
            For Each lsModelObjectName In aasModelObject
                lsEnumeratedFactTypeReading &= " " & arSentence.PredicatePart(liInd).PreboundText
                lsEnumeratedFactTypeReading &= lsModelObjectName
                lsEnumeratedFactTypeReading &= " " & arSentence.PredicatePart(liInd).PostboundText
                If liInd < arSentence.PredicatePart.Count Then
                    lsEnumeratedFactTypeReading &= " " & arSentence.PredicatePart(liInd).PredicatePartText
                    If liInd < arSentence.PredicatePart.Count Then
                        lsEnumeratedFactTypeReading &= " "
                    End If
                End If
                liInd += 1
            Next

            lsEnumeratedFactTypeReading &= " " & arSentence.FollowingText

            Return Trim(lsEnumeratedFactTypeReading).Replace("  ", " ")

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return ""
        End Try
    End Function

#End Region

#Region "FTRPARSERFUNCTIONS"

    Private Function GetPredicatePartsFromReadingUsingParser(ByVal asReading As String, ByRef arFactTypeReading As FBM.FactTypeReading) As Boolean

        Dim lsMessage As String
        Dim lrPredicatePart As FBM.PredicatePart

        Try
            ''Testing
            Me.FTRProcessor.ProcessFTR(asReading, Me.FTRParseTree)

            'OR

            Me.FTRParseTree = Me.FTRParser.Parse(asReading)

            If Me.FTRParseTree.Errors.Count > 0 Then
                '---------------------------------------------------------------------------------------------------
                'Is either an incorrectly formatted FactTypeReading, or is not a FactTypeReading Statement at all.
                '---------------------------------------------------------------------------------------------------
                lsMessage = "That's not a well formatted Fact Type Reading."
                lsMessage.AppendLine("The correct format to use is:")
                lsMessage.AppendLine("Object Types, words start with a capital. E.g. Person")
                lsMessage.AppendLine("Predicates are all lowercase. E.g. is married")
                MsgBox(lsMessage)
                Return False
            Else
                Me.FTRProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT = New List(Of String)
                Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT = New List(Of Object)
                Me.FTRProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE = New List(Of Object)
                Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = ""
                Me.FTRProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT = ""
                Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.FACTTYPEREADINGStatement, Me.FTRParseTree)
                arFactTypeReading.FrontText = Trim(NullVal(Me.FTRProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT, ""))

                Dim lrModelElementNode As FTR.ParseNode
                Dim lrPredicateClauseNode As FTR.ParseNode
                Dim liInd As Integer = 0
                Dim lasModelObjectId As New List(Of String)

                For liInd = 1 To Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count

                    lrPredicatePart = New FBM.PredicatePart(arFactTypeReading.Model, arFactTypeReading)
                    lrPredicatePart.SequenceNr = liInd

                    lrModelElementNode = Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT(liInd - 1)
                    Me.FTRProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                    Me.FTRProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                    Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                    Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.MODELELEMENTClause, lrModelElementNode)

                    '------------------------------------------------------------------------------------------------------
                    'Check to see whether the MODELELEMENTNAME is an Object Type that is actually linked by the FactType.
                    '------------------------------------------------------------------------------------------------------
                    If arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME) Is Nothing Then
                        MsgBox(Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME & " is not the name of an Object Type linkd by the Fact Type.")
                        Return False
                    End If

                    lrPredicatePart.PreBoundText = Trim(Me.FTRProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT)
                    lrPredicatePart.PostBoundText = Trim(Me.FTRProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT)

                    Dim lrRole As New FBM.Role(arFactTypeReading.FactType, "TEMP", False, New FBM.ModelObject(Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME))
                    If arFactTypeReading.FactType.RoleGroup.FindAll(AddressOf lrRole.EqualsByJoinedModelObjectId).Count = 0 Then
                        Return False
                    End If

                    lasModelObjectId.Add(Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME)
                    Dim lsModelObjectId As String = Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME
                    Dim lsTempInd As Integer = lasModelObjectId.FindAll(AddressOf lsModelObjectId.Equals).Count
                    lrPredicatePart.Role = arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME, _
                                                                                                  lsTempInd)
                    arFactTypeReading.RoleList.Add(lrPredicatePart.Role)
                    Dim lsPredicatePartText As String = ""

                    If Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = "" Then
                        '----------------------------------------
                        'FactType is binary or greater in arity
                        '----------------------------------------
                        If liInd < Me.FTRProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE.Count _
                        Or Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count = 1 Then
                            lrPredicateClauseNode = Me.FTRProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE(liInd - 1)
                            Me.FTRProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)
                            Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.PREDICATEPARTClause, lrPredicateClauseNode)

                            For Each lsPredicatePartText In Me.FTRProcessor.PREDICATEPARTClause.PREDICATEPART
                                lrPredicatePart.PredicatePartText &= lsPredicatePartText
                            Next

                            lrPredicatePart.PredicatePartText = Trim(lrPredicatePart.PredicatePartText)
                        End If
                    Else
                        '------------------------------
                        'FactType is a unary FactType
                        '------------------------------
                        lrPredicatePart.PredicatePartText = Trim(Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART)
                    End If

                    arFactTypeReading.PredicatePart.Add(lrPredicatePart)
                Next

                '-----------------------------------------------
                'Get the FollowingReadingText if there is one.
                '-----------------------------------------------
                arFactTypeReading.FollowingText = Trim(Me.FTRProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT)

                Return True

            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function



#End Region

#Region "TINYPGFUNCTIONS"

    ''' <summary>
    ''' Used to check whether a Sentence is an ORMQL Statement, when in NaturalLanguage Mode.
    ''' The following code can be used to check the returned result (as to whether the Sentence is a valid ORMQL Statement)
    ''' lrObjectReturn.GetType Is GetType(TinyPG.ParseErrors)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PreParseORMQL(ByVal asStatement As String) As Object

        Try

            If Me.ThoughtMode = pcenumBrainMode.NaturalLanguage Then
                '---------------------------------------------------------------
                'Good, we only want this method called in NaturalLanguage mode
                '---------------------------------------------------------------
            Else
                Throw New Exception("PreParseORMQL method called in ORMQL Mode")
            End If

            Me.ParseTree = Me.Parser.Parse(asStatement)

            If Me.ParseTree.Errors.Count = 0 Then
                Dim lrObjectReturn As New Object
                Return lrObjectReturn
            Else
                Return Me.ParseTree.Errors
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return New Object
        End Try

    End Function

    ''' <summary>
    ''' Used in ORMQL Mode, called from Me.ProcessInputBuffer
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessORMQL()

        Dim ls_entity_type As String = ""

        Try

            '---------------------------------------------
            'First double check that a Model is selected
            '---------------------------------------------
            If IsSomething(Me.Model) Then
                '----------
                'all good
                '----------
            Else
                Me.send_data("No Model selected.")
                Exit Sub
            End If


            Me.ParseTree = Me.Parser.Parse(Me.InputBuffer)

            If Me.ParseTree.Errors.Count = 0 Then

                Dim lrObjectReturn As Object

                lrObjectReturn = Me.Model.ORMQL.ProcessORMQLStatement(Me.InputBuffer)


                If lrObjectReturn.GetType Is GetType(TinyPG.ParseErrors) Then
                    Dim lrParseError As TinyPG.ParseError
                    For Each lrParseError In lrObjectReturn
                        Me.send_data(lrParseError.Message)
                    Next
                    Exit Sub
                End If

                If lrObjectReturn.GetType Is GetType(String) Then
                    Me.send_data(lrObjectReturn)
                End If

                Select Case Me.ParseTree.Nodes(0).Nodes(0).Text
                    Case Is = "SELECTSTMT"
                        Dim lrFact As FBM.Fact
                        Dim lrFactList As New List(Of FBM.Fact)
                        Dim lsTuple As String = ""
                        Dim liInd As Integer = 0
                        Dim lrORMQlREcordset As New ORMQL.Recordset

                        lrORMQlREcordset.Columns = lrObjectReturn.Columns
                        lrORMQlREcordset.Facts = lrObjectReturn.Facts
                        lrFactList = lrORMQlREcordset.Facts

                        If lrFactList.Count = 0 Then
                            Me.send_data("No Facts/Data Found")
                        Else
                            lrFactList(0).Data.Sort(AddressOf compare_role_SequenceNr)

                            Dim lsColumnName As String = ""
                            For Each lsColumnName In lrORMQlREcordset.Columns
                                liInd += 1
                                lsTuple &= lsColumnName
                                If liInd < lrORMQlREcordset.Columns.Count Then lsTuple &= ","
                            Next
                            Me.send_data(lsTuple)

                            Me.send_data("===============================")

                            For Each lrFact In lrFactList
                                liInd = 0
                                lsTuple = ""

                                Dim lsColumName As String
                                For Each lsColumName In lrORMQlREcordset.Columns
                                    Select Case lrFact.GetType.ToString
                                        Case Is = GetType(FBM.Fact).ToString
                                            lsTuple &= lrFact.GetFactDataByRoleName(lrORMQlREcordset.Columns(liInd)).Data
                                        Case Is = GetType(FBM.FactInstance).ToString
                                            Dim lrFactInstance As New FBM.FactInstance
                                            lrFactInstance = lrFact
                                            lsTuple &= lrFactInstance.GetFactDataInstanceByRoleName(lrORMQlREcordset.Columns(liInd)).Data
                                    End Select
                                    liInd += 1
                                    If liInd < lrORMQlREcordset.Columns.Count Then lsTuple &= ","
                                Next
                                Me.send_data(lsTuple)

                            Next
                        End If
                    Case Is = "INSERTSTMT"

                        If lrObjectReturn.GetType Is GetType(TinyPG.ParseErrors) Then
                            Dim lrParseError As TinyPG.ParseError
                            For Each lrParseError In lrObjectReturn
                                Me.send_data(lrParseError.Message)
                            Next
                        Else
                            Dim lrFact As FBM.Fact = lrObjectReturn
                            Me.send_data("One Tuple added to FactType, '" & lrFact.FactType.Name & "'")
                        End If

                        'VM-2010/08/06-Probably have to do nothing, because FactTypeInstance take care of themselves by events.
                        '---------------------------------------------------------
                        'Find the FactTypeInstance for the FactType of the Fact
                        '---------------------------------------------------------
                        'Dim lrFactTypeInstance As New FBM.FactTypeInstance

                        'lrFactTypeInstance = lrFact.FactType.CloneInstance(Me.Page)
                        'lrFactTypeInstance = Me.Page.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                        'If IsSomething(lrFactTypeInstance) Then
                        '    Dim lrFactInstance As New FBM.FactInstance
                        '    lrFactInstance = lrFact.CloneInstance(Me.Page)
                        '    lrFactTypeInstance.Fact.Add(lrFactInstance)
                        '    Me.Page.diagram.Invalidate()
                        'End If
                    Case Is = "ADDMODELELEMENTTOPAGESTMT"
                        Me.send_data("Adding ModelObject to Page")
                    Case Is = "CREATEOBJSTMT"
                        MsgBox("About to create an Object")

                        Dim lrShapeNode As MindFusion.Diagramming.ShapeNode
                        lrShapeNode = Me.Page.Diagram.Factory.CreateShapeNode(50, 50, 50, 50)
                        lrShapeNode.Tag = lrObjectReturn

                End Select
                '---------------------------------------------------------------------------
                'Exit the sub because have found what the User was
                '  trying to do, and have done it (i.e. Inserted a new Fact in the FactType
                '---------------------------------------------------------------------------
                Exit Sub
            Else
                Dim lr_error As TinyPG.ParseError
                For Each lr_error In Me.ParseTree.Errors
                    Me.send_data(lr_error.Message)
                Next
                'Exit Sub
            End If
        Catch ex As Exception
            Me.send_data("Error: " & ex.Message)
        End Try

    End Sub

#End Region

#Region "VAQLFUNCTIONS"

    Private Function IsVAQLStatement(ByVal asStatement As String) As Boolean

        IsVAQLStatement = False

        Try
            Me.VAQLParsetree = Me.VAQLParser.Parse(asStatement)

            If VAQLParsetree.Errors.Count > 0 Then
                '---------------------------------------------------------------------------------------
                'Is either an incorrectly formatted VAQL Statement, or is not a VAQL Statement at all.
                '---------------------------------------------------------------------------------------
                IsVAQLStatement = False
            Else
                IsVAQLStatement = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="asOriginalSentence"></param>
    ''' <param name="aoTokenType"></param>
    ''' <param name="abBroadcastInterfaceEvent"></param>
    ''' <param name="abStraightToActionProcessing">True if the Brain is to not ask clarrifying questions, else False.</param>
    ''' <returns></returns>
    Private Function ProcessVAQLStatement(ByVal asOriginalSentence As String,
                                          ByVal aoTokenType As VAQL.TokenType,
                                          Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                          Optional ByVal abStraightToActionProcessing As Boolean = False) As Boolean

        ProcessVAQLStatement = False

        Try
            Select Case aoTokenType
                Case Is = Boston.VAQL.TokenType.KEYWDISANENTITYTYPE
                    'Is StraightToAction
                    Call Me.ProcessISANENTITYTYPECLAUSE(abBroadcastInterfaceEvent)
                    Return True
                Case Is = Boston.VAQL.TokenType.KEYWDISAVALUETYPE
                    'Is StraightToAction
                    Call Me.ProcessISAVALUETYPECLAUSE(abBroadcastInterfaceEvent)
                    Return True
                Case Is = Boston.VAQL.TokenType.VALUETYPEISWRITTENASCLAUSE
                    'Is StraightToAction
                    Call Me.ProcessVALUETYPEISWRITTENASStatement(abBroadcastInterfaceEvent)
                    Return True
                Case Is = Boston.VAQL.TokenType.ENTITYTYPEISIDENTIFIEDBYITSCLAUSE
                    'Is StraightToAction
                    Call Me.ProcessENTITYTYPEISIDENTIFIEDBYITSStatement(abBroadcastInterfaceEvent)
                    Return True
                Case Is = Boston.VAQL.TokenType.FACTTYPECLAUSE
                    Call Me.FormulateQuestionsFACTTYPEStatement(asOriginalSentence, abBroadcastInterfaceEvent, abStraightToActionProcessing)
                    Return True
                Case Is = Boston.VAQL.TokenType.KEYWDATMOSTONE
                    Call Me.FormulateQuestionsATMOSTONEStatement(asOriginalSentence, abBroadcastInterfaceEvent, abStraightToActionProcessing)
                    Return True
                Case Is = Boston.VAQL.TokenType.KEYWDONE
                    Call Me.FormulateQuestionsONEStatement(asOriginalSentence, abBroadcastInterfaceEvent, abStraightToActionProcessing)
                    Return True
                Case Is = Boston.VAQL.TokenType.KEYWDISACONCEPT
                    'Is StraightToAction
                    Call Me.ProcessISACONCEPTStatement()
                    Return True
                Case Is = Boston.VAQL.TokenType.KEYWDISWHERE
                    Call Me.FormulateQuestionsISWHEREStatement(asOriginalSentence, abBroadcastInterfaceEvent, abStraightToActionProcessing)
                    Return True
                Case Is = Boston.VAQL.TokenType.KEYWDISAKINDOF
                    Call Me.FormulateQuestionsISAKINDOFStatement(asOriginalSentence, abBroadcastInterfaceEvent, abStraightToActionProcessing)
                    Return True
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try
    End Function

#End Region

#Region "SYSTEMREFLECTIONFUNCTIONS"

    Function SetProperty(ByVal obj As Object, ByVal propertyName As String, ByVal val As Object) As Boolean

        Dim pi As System.Reflection.PropertyInfo = obj.GetType().GetProperty(propertyName)

        Try
            ' get a reference to the PropertyInfo, exit if no property with that 
            ' name

            If pi Is Nothing Then
                Return False
            End If

            Dim lo_test As System.Reflection.PropertyInfo = obj.GetType().GetProperty(propertyName)

            ' convert the value to the expected type
            val = Convert.ChangeType(val, pi.PropertyType)

            lo_test.GetSetMethod.Invoke(lo_test, val)

            ' attempt the assignment
            'pi.SetValue(obj, val, Nothing)

            Return True
        Catch
            Return False
        End Try
    End Function

    Function getProperty(ByVal obj As Object, ByVal PropertyName As String) As MethodInfo

        Dim pi As System.Reflection.PropertyInfo = obj.GetType().GetProperty(PropertyName)

        Return pi.GetSetMethod

    End Function


#End Region

#Region "HOUSEKEEPING"

    Private Sub ResponseButton_Click(sender As Object, e As EventArgs)

        Try
            Me.InputBuffer = sender.Tag
            Call Me.ResponceChecking()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' Primary Brain process for Processing 'Sentences', Asking 'Questions'.
    ''' Is called as a result of the trigger: Me->'AddHandler Timeout.Elapsed, AddressOf OutOfTimeOut'
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OutOfTimeOut(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        Try

            Me.Timeout.Stop()

            Me.AskQuestions = True

            '------------------------------------------------------------------------------------------------------------
            'Try and answer Questions by itself.
            '  There is no use asking Questions to the user if the Question has already been answered, so the Brain
            '  tries to answer Questions by itself.
            '------------------------------------------------------------------------------------------------------------
            Dim lrQuestion As tQuestion
            For Each lrQuestion In Me.Question.ToArray
                If Me.QuestionIsResolved(lrQuestion) Then
                    Call Me.QuestionAnswered(lrQuestion, pcenumQuestionResolution.SelfAnswered, False)
                End If
            Next

            If (Me.Question.Count > 0) And Not Me.AwaitingQuestionResponse Then
                '-------------------------------------
                'Ask the next Question in the queue.
                '-------------------------------------
                Me.OutputBuffer = Me.Question(0).Question
                Me.AwaitingQuestionResponse = True
                Me.CurrentQuestion = Me.Question(0)
                Me.CurrentPlan = Me.CurrentQuestion.Plan

                Me.OutputChannel.BeginInvoke(New SendDataDelegateAdvanced(AddressOf Me.send_data), Me.OutputBuffer, False, False, Me.CurrentQuestion.ExpectedResponseType)

                Exit Sub
            ElseIf Not IsSomething(Me.CurrentQuestion) And Me.Question.Count > 0 Then
                '-------------------------------------
                'Ask the next Question in the queue.
                '-------------------------------------
                Me.OutputBuffer = Me.Question(0).Question
                Me.AwaitingQuestionResponse = True
                Me.CurrentQuestion = Me.Question(0)
                Me.CurrentPlan = Me.CurrentQuestion.Plan
                Me.OutputChannel.BeginInvoke(New SendDataDelegateAdvanced(AddressOf Me.send_data), Me.OutputBuffer, False, False, Me.CurrentQuestion.ExpectedResponseType)


            Else
                'Me.OutputBuffer = "I don't have any questions at this time"
                'Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
            End If

            'If Me.CurrentSentence.IsQualified Then
            '    '--------------------------------------------------------------------------------------------------------
            '    'That's good, the Brain has managed to qualify the Sentence and possibly what the user is talking about
            '    '--------------------------------------------------------------------------------------------------------
            'Else
            If Not Me.AwaitingQuestionResponse And IsSomething(Me.CurrentSentence) Then
                If Me.CurrentSentence.POStaggingResolved And Not Me.CurrentSentence.SentenceType.Contains(pcenumSentenceType.Response) Then
                    '------------------------------------------------------------------------------------------
                    'The user wasn't responding to a current question, and the Sentence is likely a Statement
                    '------------------------------------------------------------------------------------------
                    Me.Timeout.Stop()

                    If Me.ProcessedSentences.Exists(AddressOf Me.CurrentSentence.EqualsBySentence) Then
                        '--------------------------------------------------------------------------------------------
                        'Already POSTaggingResolved and Processed the current Sentence
                        '  This check is required because sometimes the CurrentSentence is processed more than once.
                        '  e.g. If the Brain couldn't resolve a word the first time around.
                        '--------------------------------------------------------------------------------------------
                        Dim lrSentence As Language.Sentence
                        lrSentence = Me.ProcessedSentences.Find(AddressOf Me.CurrentSentence.EqualsBySentence)

                        If lrSentence.ResolutionType = pcenumSentenceResolutionType.AbortedByUser Then
                            Me.OutputBuffer = "You told me that once before; and you aborted my efforts to resolve the sentence."
                            Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)

                            Call Me.AskQuestionForgetAskedToAbortStatement(lrSentence)
                        Else
                            Me.OutputBuffer = "You told me that once before."
                            Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)

                            Call Me.AskQuestionForgetAskedToAbortStatement(lrSentence)
                        End If

                    Else
                        '----------------------------------------------------------------------------------------------------------------------
                        'Fallback. Analyse the sentence and see what kind of statement the user (might be making).
                        Call Me.AnalyseCurrentSentence()
                    End If

                ElseIf (Not Me.CurrentSentence.POStaggingResolved) And Not Me.AwaitingQuestionResponse Then
                    Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), "I didn't understand that.")
                    '20220529-VM-No fallback to NLP
                    '-------------------------------------------------------------------------------------------------------------------
                    'Fallback re-analyse the Sentence for POSTagging.
                    'Call Me.CurrentSentence.ClearWordListQualifications()
                    'Call Language.AnalyseSentence(Me.CurrentSentence, Me.Model)
                    'Call Me.ProcessCurrentSentence() 'Reprocesses Token/Sense analysis            
                    'If Me.CurrentSentence.AreAllWordsResolved Then
                    '    Call Language.ResolveSentence(Me.CurrentSentence)
                    'End If

                    'If Me.CurrentSentence.POStaggingResolved Then
                    '    Call Language.ResolveSentence(Me.CurrentSentence)
                    '    Me.OutstandingSentences.Remove(Me.CurrentSentence)
                    '    If Me.ConfirmActions Then
                    '        Me.OutputBuffer = "I resolved the current sentence"
                    '        Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                    '    End If
                    '    Me.Timeout.Start()
                    'Else
                    '    Me.OutputBuffer = "I couldn't resolve the current sentence"
                    '    Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                    '    If Me.CurrentSentence.WordListQualification.FindAll(Function(x) x.Sense.Count = 0).Count > 0 Then
                    '        Dim lsMessage As String = ""
                    '        Dim lsUnknownWord As String = ""
                    '        Dim lrWordQualification As Language.WordQualification

                    '        For Each lrWordQualification In Me.CurrentSentence.WordListQualification.FindAll(Function(x) x.Sense.Count = 0)

                    '            lsUnknownWord = lrWordQualification.OriginalWord

                    '            lsMessage = "I don't know what a '" & lsUnknownWord & "' is."
                    '            Me.OutputBuffer = lsMessage
                    '            Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)

                    '            lsMessage = "Perhaps '" & lsUnknownWord & "' is a Value Type (a named list of Values rather like PersonId)?"
                    '            Me.OutputBuffer = lsMessage
                    '            Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)

                    '            Dim lrValueType As New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lsUnknownWord, True)

                    '            Call Me.AskQuestionCreateValueType(lrValueType, Me.CurrentSentence)

                    '            Exit For
                    '        Next

                    '        Me.OutstandingSentences.Add(Me.CurrentSentence)
                    '    Else
                    '        Me.OutputBuffer = "Type 'breakdown current sentence' and send a screenshot to support@factengine.ai to improve the Virtual Analyst."
                    '        Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                    '        Me.OutputBuffer = "FactEngine will respond to your mail with new language rules that you can load into Boston."
                    '        Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                    '    End If
                    'End If
                ElseIf Not Me.AwaitingQuestionResponse Then
                    If Me.OutstandingSentences.Count > 0 Then
                        Me.CurrentSentence = Me.OutstandingSentences(0)
                        Me.OutstandingSentences.Remove(Me.CurrentSentence)
                        Me.CurrentSentence.ClearWordListQualifications()
                        Call Language.AnalyseSentence(Me.CurrentSentence, Me.Model)
                        Call Me.ProcessCurrentSentence()
                        If Me.CurrentSentence.AreAllWordsResolved Then
                            Call Language.ResolveSentence(Me.CurrentSentence)
                            Call Me.ValidateIfCurrentSentenceIsAFactType() 'Populates Me.CurrentSentence.ModelElement and Me.CurrentSentence.PredicatePart if the sentence is a Fact Type declaration (i.e. a valid Fact Type Reading).
                        End If
                        Me.Timeout.Start() 'Threading jumps to HOUSEKEEPING.OutOfTimeout (i.e. This process) (again).
                    End If
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' Does precursory checking of the type of Statement that the user may be making.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AnalyseCurrentSentence()

        Try
            If Me.CheckIsConceptStatement(Me.CurrentSentence) Then
                Call Me.AskQuestionCreateConcept()

            ElseIf Me.CheckIsSubtypeStatement(Me.CurrentSentence) Then
                Call Me.AskQuestionCreateSubtypeConstraint()
            ElseIf Me.CheckCreateFactTypeReadingForExistingFactType Then

            Else
                Call Me.AskQuestionCreateFactType()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Function CheckCreateFactTypeReadingForExistingFactType() As Boolean

        Dim lrModelObject As FBM.ModelObject
        Dim larModelElemenets As New List(Of FBM.ModelObject)

        CheckCreateFactTypeReadingForExistingFactType = False

        Try
            If Me.CurrentSentence.WordListResolved.FindAll(Function(x) x.Sense = pcenumWordSense.Noun).Count = 0 Then
                Exit Function
            End If

            For Each lrResolvedWord In Me.CurrentSentence.WordListResolved.FindAll(Function(x) x.Sense = pcenumWordSense.Noun)

                '------------------------------------
                'Create a dummy ModelElement/Object
                '------------------------------------
                lrModelObject = New FBM.ModelObject(lrResolvedWord.Word, pcenumConceptType.ValueType)
                larModelElemenets.Add(lrModelObject)
            Next

            If Me.Model.ExistsFactTypeForModelElements(larModelElemenets) Then
                Me.OutputBuffer = "This Fact Type already exists."
                Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                CheckCreateFactTypeReadingForExistingFactType = True
                Call Me.AskQuestionCreateFactTypeReading(Me.CurrentSentence)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Private Function CheckIsSubtypeStatement(ByVal arSentence As Language.Sentence) As Boolean

        CheckIsSubtypeStatement = False

        Try
            If arSentence.WordListResolved.Count = 4 Then
                If arSentence.WordListResolved.Count = 4 And
                   arSentence.WordListResolved(0).Sense = pcenumWordSense.Noun And
                   arSentence.WordListResolved(1).Word = "is" And
                   (arSentence.WordListResolved(2).Word = "a" Or arSentence.WordListResolved(2).Word = "an") And
                   arSentence.WordListResolved(3).Sense = pcenumWordSense.Noun Then
                    CheckIsSubtypeStatement = True
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Private Function CheckIsConceptStatement(ByVal arSentence As Language.Sentence) As Boolean

        CheckIsConceptStatement = False

        Try
            If arSentence.WordListResolved.Count = 4 Then
                If arSentence.WordListResolved(0).Sense = pcenumWordSense.Noun And
                   arSentence.WordListResolved(1).Word = "is" And
                   arSentence.WordListResolved(2).Word = "a" And
                   arSentence.WordListResolved(3).Word = "Concept" Then
                    CheckIsConceptStatement = True
                End If
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Function

    Private Sub AskQuestionCreateConcept()

        Dim lrQuestion As tQuestion

        Try

            If Me.ConfirmActions Then
                Dim lasSymbol As New List(Of String)
                lasSymbol.Add(Me.CurrentSentence.WordListResolved(3).Word)

                lrQuestion = New tQuestion("Would you like me to create a Concept for '" & Me.CurrentSentence.WordListResolved(3).Word & "'?",
                                           pcenumQuestionType.CreateConcept,
                                           pcenumExpectedResponseType.YesNo,
                                           lasSymbol,
                                           Me.CurrentSentence)
                If Me.QuestionHasBeenRaised(lrQuestion) Then
                    '------------------------------------------------------------
                    'Great, already asked the question and am awaiting responce
                    '------------------------------------------------------------
                Else
                    Me.AddQuestion(lrQuestion)
                End If
            Else
                Dim lrConcept As New FBM.Concept(Me.CurrentSentence.WordListResolved(0).Word)
                Me.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me.Model, lrConcept.Symbol, pcenumConceptType.GeneralConcept), False, False)

                Me.OutputBuffer = "Okay"
                Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AskQuestionCreateSubtypeConstraint()

        Dim lrQuestion As tQuestion
        Dim lasSymbol As New List(Of String)

        Try

            Me.CurrentPlan = New Brain.Plan
            Dim lrFirstStep As New Brain.Step(pcenumActionType.CreateEntityType, True, pcenumActionType.None, Nothing)
            Dim lrSecondStep As New Brain.Step(pcenumActionType.CreateEntityType, True, pcenumActionType.None, Nothing)
            Dim lrThirdStep As New Brain.Step(pcenumActionType.CreateSubtypeRelationship, True, pcenumActionType.None, Nothing)
            Me.CurrentPlan.AddStep(lrFirstStep)
            Me.CurrentPlan.AddStep(lrSecondStep)
            Me.CurrentPlan.AddStep(lrThirdStep)

            Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, Me.CurrentSentence.WordListResolved(0).Word, Nothing, True)
            If Me.Model.EntityType.Exists(AddressOf lrEntityType.Equals) Then
            Else
                lasSymbol.Add(Me.CurrentSentence.WordListResolved(0).Word)
                lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & Me.CurrentSentence.WordListResolved(0).Word & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                           pcenumQuestionType.CreateEntityType,
                                           pcenumExpectedResponseType.YesNo,
                                           lasSymbol,
                                           Me.CurrentSentence,
                                           Nothing,
                                           Me.CurrentPlan,
                                           lrFirstStep)

                If Me.QuestionHasBeenRaised(lrQuestion) Then
                    '------------------------------------------------------------
                    'Great, already asked the question and am awaiting responce
                    '------------------------------------------------------------
                Else
                    Me.AddQuestion(lrQuestion)
                End If
            End If

            lrEntityType = New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, Me.CurrentSentence.WordListResolved(3).Word, Nothing, True)
            If Me.Model.EntityType.Exists(AddressOf lrEntityType.Equals) Then
            Else
                lasSymbol = New List(Of String)
                lasSymbol.Add(Me.CurrentSentence.WordListResolved(3).Word)
                lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & Me.CurrentSentence.WordListResolved(3).Word & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                           pcenumQuestionType.CreateEntityType,
                                           pcenumExpectedResponseType.YesNo,
                                           lasSymbol,
                                           Me.CurrentSentence,
                                           Nothing,
                                           Me.CurrentPlan,
                                           lrSecondStep)

                If Me.QuestionHasBeenRaised(lrQuestion) Then
                    '------------------------------------------------------------
                    'Great, already asked the question and am awaiting responce
                    '------------------------------------------------------------
                Else
                    Me.AddQuestion(lrQuestion)
                End If
            End If

            Dim lsQuestion As String = ""
            lsQuestion = "Would you like me to set Entity Type, '" & Me.CurrentSentence.WordListResolved(0).Word & "'"
            lsQuestion &= ", as a SubType of Entity Type, '" & Me.CurrentSentence.WordListResolved(3).Word & "'"

            lasSymbol = New List(Of String)
            lasSymbol.Add(Me.CurrentSentence.WordListResolved(0).Word)
            lasSymbol.Add(Me.CurrentSentence.WordListResolved(3).Word)

            lrQuestion = New tQuestion(lsQuestion,
                                       pcenumQuestionType.CreateSubtypeRelationship,
                                       pcenumExpectedResponseType.YesNo,
                                       lasSymbol,
                                       Me.CurrentSentence,
                                       Nothing,
                                       Me.CurrentPlan,
                                       lrThirdStep)

            If Me.QuestionHasBeenRaised(lrQuestion) Then
                '------------------------------------------------------------
                'Great, already asked the question and am awaiting responce
                '------------------------------------------------------------
            Else
                Me.AddQuestion(lrQuestion)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AskQuestionForgetAskedToAbortStatement(ByRef arSentence As Language.Sentence)

        Dim lsQuestion As String = ""
        Dim lrQuestion As tQuestion
        Dim lrPlan As New Brain.Plan
        Dim lrStep As Brain.Step

        Try

            lrStep = New Brain.Step(pcenumActionType.ForgetStatementAbortion, True, pcenumActionType.None, Nothing)
            lrPlan.AddStep(lrStep)

            lsQuestion = "Would you like me to process the sentence, '" & arSentence.Sentence & "' again?"

            lrQuestion = New tQuestion(lsQuestion,
                                        pcenumQuestionType.ForgetAskedToAbortPlan,
                                        pcenumExpectedResponseType.YesNo,
                                        Nothing,
                                        arSentence,
                                        Nothing,
                                        lrPlan,
                                        lrStep)

            If Me.QuestionHasBeenRaised(lrQuestion) Then
                '------------------------------------------------------------
                'Great, already asked the question and am awaiting responce
                '------------------------------------------------------------
            Else
                Me.AddQuestion(lrQuestion)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AskQuestionCreateFactType()

        Dim lrResolvedWord As Language.WordResolved
        Dim lrQuestion As tQuestion
        Dim lasEntityTypeList As New List(Of String)

        Dim lrPlan As New Brain.Plan
        Dim lrStep As Brain.Step

        Try

            For Each lrResolvedWord In Me.CurrentSentence.WordListResolved
                If lrResolvedWord.Sense = pcenumWordSense.Noun Then

                    lrResolvedWord.Word = Viev.Strings.MakeCapCamelCase(lrResolvedWord.Word)

                    lasEntityTypeList.Add(Trim(lrResolvedWord.Word))

                    Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lrResolvedWord.Word, Nothing, True)
                    Dim lrValueType As New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lrResolvedWord.Word, True)
                    Dim lrFactType As New FBM.FactType(Me.Model, lrResolvedWord.Word, True)

                    If Me.Model.EntityType.Exists(AddressOf lrEntityType.Equals) Then

                    ElseIf Me.Model.ValueType.Exists(AddressOf lrValueType.Equals) Then

                    ElseIf Me.Model.FactType.Exists(AddressOf lrFactType.Equals) Then

                    Else

                        Dim lasSymbol As New List(Of String)
                        lasSymbol.Add(lrResolvedWord.Word)

                        lrStep = New Brain.Step(pcenumActionType.CreateEntityType, True, pcenumActionType.CreateValueType, Nothing)
                        lrPlan.AddStep(lrStep)

                        lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & lrResolvedWord.Word & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                         pcenumQuestionType.CreateEntityType,
                                                         pcenumExpectedResponseType.YesNo,
                                                         lasSymbol,
                                                         Me.Sentence(0),
                                                         Nothing,
                                                         lrPlan,
                                                         lrStep)

                        If Me.QuestionHasBeenRaised(lrQuestion) Then
                            '------------------------------------------------------------
                            'Great, already asked the question and am awaiting responce
                            '------------------------------------------------------------
                        Else
                            Me.AddQuestion(lrQuestion)
                        End If
                    End If
                Else
                    lrResolvedWord.Word = LCase(lrResolvedWord.Word)
                End If
            Next

            lrStep = New Brain.Step(pcenumActionType.CreateFactType, True, pcenumActionType.None, Nothing)
            lrPlan.AddStep(lrStep)

            Dim lsEnumeratedFactTypeReading As String

            lsEnumeratedFactTypeReading = Me.CreateEnumeratedFactTypeReadingFromParts(lasEntityTypeList, Me.CurrentSentence)

            lrQuestion = New tQuestion("Would you like me to create a Fact Type for '" & lsEnumeratedFactTypeReading & "'?",
                                            pcenumQuestionType.CreateFactType,
                                            pcenumExpectedResponseType.YesNo,
                                            Nothing,
                                            Me.CurrentSentence,
                                            Nothing,
                                            lrPlan,
                                            lrStep)

            If Me.QuestionHasBeenRaised(lrQuestion) Then
            Else
                Me.AddQuestion(lrQuestion)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub AskQuestionCreateFactTypeReading(ByRef arSentence As Language.Sentence)

        Dim lrQuestion As tQuestion
        Dim lrPlan As Brain.Plan
        Dim lrStep As Brain.Step

        Try

            lrPlan = New Brain.Plan
            lrStep = New Brain.Step(pcenumActionType.CreateFactTypeReading, True, pcenumActionType.None, Nothing)

            lrQuestion = New tQuestion("Would you like me to create a Fact Type Reading for '" & arSentence.Sentence & "'?",
                                     pcenumQuestionType.CreateFactTypeReading,
                                     pcenumExpectedResponseType.YesNo,
                                     Nothing,
                                     arSentence,
                                     Nothing,
                                     lrPlan,
                                     lrStep)

            If Me.QuestionHasBeenRaised(lrQuestion) Then
                '------------------------------------------------------------
                'Great, already asked the question and am awaiting responce
                '------------------------------------------------------------
            Else
                Me.AddQuestion(lrQuestion)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AskQuestionCreateEntityType(ByVal arEntityType As FBM.EntityType, ByVal arSentence As Language.Sentence, Optional abInsertAtHeadOfQueue As Boolean = False)

        Dim lrQuestion As tQuestion
        Dim lasSymbol As New List(Of String)

        Try

            lasSymbol.Add(arEntityType.Name)

            Dim lrPlan As New Brain.Plan
            Dim lrStep As New Brain.Step(pcenumActionType.CreateEntityType, True, pcenumActionType.None, Nothing)
            lrPlan.AddStep(lrStep)

            lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & arEntityType.Name & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                             pcenumQuestionType.CreateEntityType,
                                             pcenumExpectedResponseType.YesNo,
                                             lasSymbol,
                                             arSentence,
                                             Nothing,
                                             lrPlan,
                                             lrStep)

            lrQuestion.ModelObject.Add(arEntityType)

            If Me.QuestionHasBeenRaised(lrQuestion) Then
                '------------------------------------------------------------
                'Great, already asked the question and am awaiting responce
                '------------------------------------------------------------
            Else
                Me.AddQuestion(lrQuestion, abInsertAtHeadOfQueue)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="arValueType"></param>
    ''' <param name="arSentence"></param>
    ''' <param name="abInsertAtHeadOfQueue"></param>
    ''' <param name="arOriginalPlan">An existing Plan if modifying an existing Plan.</param>
    ''' <remarks></remarks>
    Private Sub AskQuestionCreateValueType(ByVal arValueType As FBM.ValueType, ByRef arSentence As Language.Sentence, Optional ByVal abInsertAtHeadOfQueue As Boolean = False, Optional ByRef arOriginalPlan As Brain.Plan = Nothing)

        Dim lrQuestion As tQuestion
        Dim lasSymbol As New List(Of String)
        Dim lrPlan As New Brain.Plan
        Dim lrStep As New Brain.Step(pcenumActionType.CreateValueType, True, pcenumActionType.None, Nothing)

        Try

            If arOriginalPlan Is Nothing Then
                lrPlan.AddStep(lrStep)
            Else
                arOriginalPlan.Step.Insert(arOriginalPlan.GetIndexFirstUnresolvedStep, lrStep)
                lrPlan = arOriginalPlan
            End If

            lasSymbol.Add(arValueType.Name)

            lrQuestion = New tQuestion("Would you like me to create an Value Type for '" & arValueType.Name & "'?",
                                             pcenumQuestionType.CreateValueType,
                                             pcenumExpectedResponseType.YesNo,
                                             lasSymbol,
                                             arSentence,
                                             Nothing,
                                             lrPlan,
                                             lrStep)

            lrQuestion.ValueType.Add(arValueType)

            If Me.QuestionHasBeenRaised(lrQuestion) Then
                '------------------------------------------------------------
                'Great, already asked the question and am awaiting responce
                '------------------------------------------------------------
            Else
                Me.AddQuestion(lrQuestion, abInsertAtHeadOfQueue)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

#End Region

#Region "SENTENCEPROCESSING"

    Private Sub CleanSentence(ByRef asSentence As String)

        Dim liInd As Integer = 0

        Try
            '----------------------
            'First strip fullstops
            '----------------------
            If asSentence.EndsWith(".") Then
                asSentence = asSentence.Remove(asSentence.Length - 1)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ProcessCurrentSentence()

        Dim lsMessage As String = ""

        Try

            If Me.ConfirmActions Then
                Me.OutputBuffer = "Processing Current Sentence"
                Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
            End If

            Me.Timeout.Stop()

            '-----------------------------------------------
            'Encourage the user to use CapCamelCase words.
            If Me.CurrentSentence.Sentence.All(Function(c) Char.IsLower(c) Or c = " ") And
               Me.CurrentSentence.WordList.Count > 1 Then

                '---------------------------------------------------
                'Find an example Model Element in the sentence. I.e. If there is a word in the sentence that is an Entity/Value/ObjectifiedFact Type then return the first one found.
                Dim lrModelElementWord As String = "Company"
                For Each lrModelElementWord In Me.CurrentSentence.WordList
                    If Me.Model.GetModelObjectByName(Viev.Strings.MakeCapCamelCase(lrModelElementWord)) IsNot Nothing Then
                        Viev.Strings.MakeCapCamelCase(lrModelElementWord)
                        Exit For
                    End If
                Next
                lsMessage = "It is better to use a capital first letter for Entity Types, Value Types and Objectified Fact Types (e.g. '" & Viev.Strings.MakeCapCamelCase(lrModelElementWord) & "')"
                '20220529-VM-was. Remove if comment out if not needed.
                'Me.send_data(lsMessage)
                Me.OutputChannel.BeginInvoke(New SendDataDelegateAdvanced(AddressOf Me.send_data), lsMessage, False, False, pcenumExpectedResponseType.None)
            End If

            Call Language.ProcessSentence(Me.CurrentSentence)

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ProcessSentence()

        Dim lsString As String
        Dim lr_noun_model As New FBM.Model(pcenumLanguage.BusinessRulesNaturalLanguage, "English-Nouns", True)
        Dim lr_verb_model As New FBM.Model(pcenumLanguage.BusinessRulesNaturalLanguage, "English-Verbs", True)
        Dim lr_model_concept As New FBM.ConceptInstance
        Dim lrFactType As New FBM.FactType

        Dim ls_entity_type As String

        Try
            '-----------------------------------
            'Check for EntityTypes in sentence.
            '  Do this by checking for Nouns.
            '-----------------------------------            
            For Each lsString In Me.InputSymbols
                lr_model_concept = New FBM.ConceptInstance(lr_noun_model, Me.Page, lsString)
                Dim lb_question_raised As Boolean = False
                Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lsString, Nothing, True)
                If TableConceptInstance.ExistsConceptInstance(lr_model_concept) Then
                    '-----------------------------------------------------------
                    'Check to see if the EntityType is already within the Model
                    '-----------------------------------------------------------
                    If Me.Model.EntityType.Exists(AddressOf lrEntityType.Equals) Then
                        '--------------------------------------------
                        'The EntityType already exists in the Model
                        '--------------------------------------------
                    Else
                        '---------------------------------------------------
                        'Ask the user if they want to create an EntityType
                        '  within the model for the Symbol
                        '---------------------------------------------------
                        'Dim lr_question As New tQuestion("Create an EntityType for '" & lsString & "'?", pcenumQuestionType.CreateEntityType, True, lsString, Me.Sentence(0))
                        'Me.CurrentQuestion = lr_question
                        lb_question_raised = True
                    End If
                End If
                If lb_question_raised Then Me.Timeout.Start()
            Next


            '------------------------------------------
            'Check for what else the Symbol may be
            '------------------------------------------
            For Each lsString In Me.InputSymbols
                'If (Not IsVerb(lsString)) Then
                '    '---------------------------------------------------------------
                '    'Confirm with the User as to whether the Word/Symbol is a Verb
                '    '---------------------------------------------------------------
                '    Dim lr_question As New tQuestion("I don't know the word/Symbol '" & lsString & "'. Is '" & lsString & "' a Verb?", pcenumQuestionType.check_word_type_verb, True, lsString, Me.sentence(0))
                '    Me.CurrentQuestion = lr_question
                'End If
            Next lsString


            '--------------------------------------------------------
            'Check if the sentence starts with a UniversalQuantifier
            '  e.g. 'A', 'Every'
            '--------------------------------------------------------
            If Me.check_universal_quantifier Then
                ls_entity_type = Me.InputSymbols(1)
                Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, ls_entity_type, Nothing, True)
                '-----------------------------------------------------------
                'Check to see if the EntityType is already within the Model
                '-----------------------------------------------------------
                If Me.Model.EntityType.Exists(AddressOf lrEntityType.Equals) Then
                    '--------------------------------------------
                    'The EntityType already exists in the Model
                    '--------------------------------------------
                Else
                    '---------------------------------------------------
                    'Ask the user if they want to create an EntityType
                    '  within the model for the Symbol
                    '---------------------------------------------------
                    'Dim lr_question As New tQuestion("Create an EntityType for '" & ls_entity_type & "'?", pcenumQuestionType.CreateEntityType, True, ls_entity_type, Me.Sentence(0))
                    'Me.CurrentQuestion =lr_question
                    Exit Sub
                End If
            End If

            If Viev.Strings.checkUpper(Left(Me.InputSymbols(0), 1)) Then
                '--------------------------
                'Creating a new EntityType
                '--------------------------
                lsString = Me.InputSymbols(0)
                lr_model_concept = New FBM.ConceptInstance(lr_noun_model, Me.Page, lsString)
                '--------------------------
                'Check is Symbol is Noun
                '--------------------------
                If TableConceptInstance.ExistsConceptInstance(lr_model_concept) Then
                    '------------------------------------------------------
                    'Symbol is a (known) Noun within the working Language
                    '------------------------------------------------------
                    Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lsString, Nothing, True)
                    '-----------------------------------------------------------
                    'Check to see if the EntityType is already within the Model
                    '-----------------------------------------------------------
                    If Me.Model.EntityType.Exists(AddressOf lrEntityType.Equals) Then
                        '--------------------------------------------
                        'The EntityType already exists in the Model
                        '--------------------------------------------
                    Else
                        '---------------------------------------------------
                        'Ask the user if they want to create an EntityType
                        '  within the model for the Symbol
                        '---------------------------------------------------
                        'Dim lr_question As New tQuestion("Create an EntityType for '" & lsString & "'?", pcenumQuestionType.CreateEntityType, True, lsString, Me.Sentence(0))
                        'Me.CurrentQuestion = lr_question
                    End If
                End If
            End If 'Check upper

            '--------------------------------------------------------------------
            'Formulate a FactType if possible
            '  PSEUDOCODE (Plain language)
            '    * Identify all of the Object (Types) within the sentence,
            '        and the PredicateParts between the Object (Types).
            '        (As per FactTypeReadings, ignore Universal Quantifiers)
            '  PSEUDOCODE:
            '    * FOR EACH lsString IN sentence (Left-To-Right Parse)
            '    * LOOP
            '--------------------------------------------------------------------            
            Dim ls_FactType_name As String = ""
            Dim lr_ObjectTypeList As List(Of FBM.ModelObject)
            lr_ObjectTypeList = Me.get_ObjectTypeList()
            Dim lr_object_type As FBM.ModelObject
            For Each lr_object_type In lr_ObjectTypeList
                ls_FactType_name &= lr_object_type.Symbol
            Next
            lrFactType = New FBM.FactType(Me.Model, ls_FactType_name, True)
            Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType)
            '------------------------------------------------------------------------------
            'No Longer supported (v1.13 of the database Model).
            'lrFactTypeReading.ObjectTypeList = lr_ObjectTypeList

            Call Me.get_PredicateParts_from_reading(Me.InputBuffer, lrFactTypeReading)

            Me.send_data("Okay")

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' 20161130-This is older code that may no longer be relevant (used in Me.ProcessSentence)
    '''   but which may be very important if Me.ProcessSentence is used again in the future.
    ''' Breaks the Me.InputBuffer (sentence) into Tokens or 'Symbols' that may be processed in various ways by other parts of the code.
    ''' 20161130-Check to see if is superceded by the Language.Sentence structure.
    ''' 20161130-At the very least, this code is currently innocuous and causes no harm to the functioning of the brain.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TokeniseInputBuffer()

        Dim lsString As String

        Try
            Me.InputSymbols.Clear()
            For Each lsString In Me.InputBuffer.Split
                Me.InputSymbols.Add(lsString)
            Next
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' Used to call VAQL Statements from the FactEngine form. This is so that Knowledge Language statements can be made in the FactEngine.
    ''' </summary>
    ''' <param name="asFEQLStatement"></param>
    Public Sub ProcessFEQLStatement(ByVal asFEQLStatement As String)

        Dim loTokenType As VAQL.TokenType

        Try
            Call Me.VAQL.ProcessVAQLStatement(asFEQLStatement, loTokenType, Me.VAQLParsetree)
            If Me.ProcessVAQLStatement(asFEQLStatement, loTokenType) Then
                '----------------------------------------------------------------------------------------------
                'Start the TimeOut so that the Brain can repeatedly address the Sentence until it is resolved
                '----------------------------------------------------------------------------------------------
                If Me.Question.Count > 0 Then
                    Me.Timeout.Start() 'Threading jumps to HOUSEKEEPING.OutOfTimeout
                End If
                Exit Sub
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub ProcessFBMInterfaceFEKLStatement(ByVal asFEKLStatement As String)

        Dim loTokenType As VAQL.TokenType

        Try
            'CodeSafe
            If Me.VAQL Is Nothing Then
                prApplication.Brain.VAQL = New VAQL.Processor(prApplication.WorkingModel)
            End If

            Call Me.VAQL.ProcessVAQLStatement(asFEKLStatement, loTokenType, Me.VAQLParsetree)
            Call Me.ProcessVAQLStatement(asFEKLStatement, loTokenType, False)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub ProcessNaturalLanguage()

        Try

            Me.Timeout.Stop()

            If Me.InputBuffer = "" Then
                Exit Sub
            End If

            '==============================================================================
            'AIML - At this stage, do AIML processing first
            '  - If the user input is recognised by the AIML bot, then the Me.InputBuffer stores
            '  (effectively a statement) what is in the "think.set" element of the corresponding section of the .aiml file.
            '  The .aiml file for Boston is stored in the \SVN\Boston\AIML delelopment directory.
            '----------------------------------------------
            Dim myRequest As New Request(Me.InputBuffer, Me.User, Me.SentenceAnalyser)
            Dim myResult As Result = Me.SentenceAnalyser.Chat(myRequest)
            If myResult.Output <> "" Then
                If myResult.user.Topic = "easteregg" Then
                    Me.send_data(myResult.Output)
                    Exit Sub
                Else
                    Me.InputBuffer = myResult.user.Topic
                End If
            End If
            '==============================================================================

            If Me.IsVAQLStatement("NL: " & Me.InputBuffer) Then
                Dim loTokenType As VAQL.TokenType

                'Determine if is a valid VAQL Statement.
                Call Me.VAQL.ProcessVAQLStatement(Me.InputBuffer, loTokenType, Me.VAQLParsetree)

                'Process the VAQL Statement based on the primary TokenType returned (above)
                If Me.ProcessVAQLStatement(Me.InputBuffer, loTokenType) Then
                    '----------------------------------------------------------------------------------------------
                    'Start the TimeOut so that the Brain can repeatedly address the Sentence until it is resolved
                    '----------------------------------------------------------------------------------------------
                    If Me.Question.Count > 0 Then
                        Me.Timeout.Start() 'Threading jumps to HOUSEKEEPING.OutOfTimeout
                    End If
                    Exit Sub
                End If
            End If

            '----------------------------------------------------------------------------------------------------------
            'Check that no "Special Characters" appear in the "Sentence"....because the Brain doesn't understand them.
            '----------------------------------------------------------------------------------------------------------
            Dim loRegularExpression As Regex = New Regex("^[a-zA-Z0-9 (),]*$")
            If (loRegularExpression.IsMatch(Me.InputBuffer)) Then
                '----------------------------------------------------------------
                'Great, the sentence doesn't contain characters like '/*-+@&$#%
                '----------------------------------------------------------------
            Else
                Dim lsMessage As String = ""
                lsMessage = "Oops. I don't understand characters like '/*-+@&$#% in a sentence."
                Me.send_data(lsMessage)
                Exit Sub
            End If

            '-------------------------------------------------------------------------------------------------------------------
            'Check that no "Special Characters" appear in the "Sentence"....because the Brain doesn't understand them.
            '  NB IMPORTANT: Second pass because this time we want to exclude characters like "(" and ")" which may 
            '  have come from a mistyped VAQL Statement (i.e. that was not picked up in the code above).
            '  i.e. At this stage we're handing over to regular language processing that expects no special characters at all.
            '-------------------------------------------------------------------------------------------------------------------
            loRegularExpression = New Regex("^[a-zA-Z0-9 ,]*$")
            If (loRegularExpression.IsMatch(Me.InputBuffer)) Then
                '----------------------------------------------------------------
                'Great, the sentence doesn't contain characters like '/*-+@&$#%
                '----------------------------------------------------------------
            Else
                Dim lsMessage As String = ""
                lsMessage = "Oops. That's not a valid VAQL statement or a sentence that I can understand."
                Me.send_data(lsMessage)
                Exit Sub
            End If

            Dim lsOriginalSentence As String = Me.InputBuffer
            Me.InputBuffer = LCase(Me.InputBuffer)

            '----------------------------------------------------
            'Clear sentence of FullStops etc
            '----------------------------------------------------        
            Call Me.CleanSentence(Me.InputBuffer)

            '----------------------------------------------------------------------------------------------------------
            'Break the Sentence down into Tokens, and where each sentence is a space separated set of Tokens/Symbols.
            '  Stored in Me.InputSymbols.
            '----------------------------------------------------------------------------------------------------------
            Call Me.TokeniseInputBuffer()

            '============================================================================================================
            'A Sentence can be one of the following
            '   Directive (including reserved Commands)
            '   Statement e.g. "All boys are good"
            '   Question  e.g. "What time is it"
            '   Salutation e.g. "Hello"
            '   Fairwell   e.g. "Goodbye"
            '   Responses to Questions asked by the Brain   e.g. "Yes", "No"
            '   VAQL or ORMQL  (i.e. Parsable sentences of a query language for direct processing by the Brain)
            '   Unknown   All Sentences in the Brain start with an 'Unknown' SentenceType
            '
            ' The following code eliminates
            '       'Directives' (including 'Reserved Commands') 
            '       'Salutations'
            ' before(processing) 'Questions' and 'Statements' or 'Responses to Questions asked by the Brain'
            '============================================================================================================

            '----------------------------------------------------
            'Inject the sentense into the Sentence queue/list
            '  if it isn't a reserved Command.
            '----------------------------------------------------
            If Me.CheckCommand(Me.InputBuffer) Then
                '-------------------------------------------------
                'The user issued a reserved Command to the Brain
                '-------------------------------------------------
                Exit Sub
            ElseIf Me.CheckDirective Then
                '-------------------------------------------------
                'A Directive has been sent to the Brain
                '-------------------------------------------------
                Exit Sub
            ElseIf CheckSalutation(Me.InputBuffer) Then
                '-------------------------------------------------
                'A Salutation has been sent to the Brain
                '-------------------------------------------------
                Exit Sub
            ElseIf CheckFarewell(Me.InputBuffer) Then
                '-------------------------------------------------
                'A Fairwell has been sent to the Brain
                '-------------------------------------------------
                Exit Sub
            ElseIf CheckThankYou(Me.InputBuffer) Then
                '-------------------------------------------------
                'User has thanked the Brain.
                '-------------------------------------------------
                Exit Sub
            ElseIf CheckAskingForHelp(Me.InputBuffer) Then
                '-------------------------------------------------
                'User has asked the Brain for Help.
                '-------------------------------------------------
                Exit Sub
            Else
                '---------------------------------------------------------------------------
                'The sentence must be a
                '       "Question" or a
                '       "Statement" or a
                '       "A Response to a Question asked by the Brain"
                ' for further processing
                '---------------------------------------------------------------------------
                Me.CurrentSentence = New Language.Sentence(Me.InputBuffer, lsOriginalSentence)
                Me.Sentence.Insert(0, Me.CurrentSentence)
            End If

            If IsSomething(Me.CurrentSentence) Then

                '------------------------------------------------------
                'Questions asked by the user have precedence over the
                '  computer, so check if the user has a question
                '------------------------------------------------------
                Call Me.CheckQuestion(Me.CurrentSentence.Sentence)

                '====================================================================================================================
                '20220529-VM-Removed. NLP processing not needed in current version of Boston.
                Me.CurrentSentence.POStaggingResolved = True 'Stop gap untile NLP reintroduced.
                ''---------------------------------------------------------------------------------------------------------
                ''Check if the Sentence is a Statement
                ''  Particularly if the sentence is of a format that will allow a FactType to be created for the Sentence
                ''  NB Language.AnalyseSentence does part of speech tagging 
                ''---------------------------------------------------------------------------------------------------------
                'Call Language.AnalyseSentence(Me.CurrentSentence, Me.Model)

                'Call Me.ProcessCurrentSentence()
                'If Me.CurrentSentence.AreAllWordsResolved Then
                '    Call Language.ResolveSentence(Me.CurrentSentence)
                'End If

                'If Me.CurrentSentence.POStaggingResolved Then
                '    '--------------------------------------------------------------------------------
                '    'Populates Me.CurrentSentence.ModelElement and Me.CurrentSentence.PredicatePart 
                '    '  if the sentence is a Fact Type declaration (i.e. a valid Fact Type Reading).
                '    '--------------------------------------------------------------------------------
                '    Call Me.ValidateIfCurrentSentenceIsAFactType()
                'End If
                '====================================================================================================================

                '------------------------------------------------------
                'Check to see if the Brain is expecting a response to 
                '  a question
                '------------------------------------------------------
                If Me.AwaitingQuestionResponse Then
                    Call Me.ResponceChecking()
                Else
                    '--------------------------------
                    'Brain is not awaiting response
                    '-------------------------------------------------------------------------------
                    'Process the Sentence and keep on processing the sentence until it is resolved
                    '  or the user tells the Brain to forget the sentence.
                    '  The OutOfTimeOut method will do this.
                    '-------------------------------------------------------------------------------
                End If 'Awaiting response

                '------------------------------------------------------------------------------------
                'A response to a question could abort the current sentence.
                '------------------------------------------------------------
                If Me.CurrentSentence Is Nothing Then
                    Exit Sub
                End If

                If Not Me.CurrentSentence.SentenceType.Contains(pcenumSentenceType.Response) And
                    Me.CurrentSentence.POStaggingResolved And
                    (Me.CurrentSentence.ModelElement.Count = 0 Or Me.CurrentSentence.PredicatePart.Count = 0) Then
                    '--------------------------------------------------------------------------
                    'The Sentence is obviously not a Fact Type.
                    '--------------------------------------------
                    Dim lsMessage As String = ""
                    lsMessage = "Sorry, I don't know what you are talking about."
                    Me.send_data(lsMessage)
                    Me.CurrentSentence.ResolutionType = pcenumSentenceResolutionType.Unresolved
                    Me.Sentence.Add(Me.CurrentSentence)
                    Me.CurrentSentence = Nothing
                    Exit Sub
                End If

                '----------------------------------------------------------------------------------------------
                'Start the TimeOut so that the Brain can repeatedly address the Sentence until it is resolved
                '----------------------------------------------------------------------------------------------
                Me.Timeout.Start() 'Threading jumps to HOUSEKEEPING.OutOfTimeout

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ValidateIfCurrentSentenceIsAFactType()

        Try

            Dim lsStatement As String = "CREATE FACTTYPE FOR '" & Me.CurrentSentence.SentenceResolved & "'"

            If Me.PreParseORMQL(lsStatement).GetType Is GetType(TinyPG.ParseErrors) Then
                'Dim lrParseError As TinyPG.ParseError

                'Dim larParseErrors = Me.PreParseORMQL(lsStatement)

                'For Each lrParseError In larParseErrors
                '    Me.send_data(lrParseError.Message)
                'Next
            Else
                Select Case Me.ParseTree.Nodes(0).Nodes(0).Nodes(1).Text
                    Case Is = "CREATEFACTTYPESTMT"
                        '-------------------------
                        'Create the DynamicObject
                        '-------------------------
                        Dim lrCreateFactTypeStatement As New Object
                        lrCreateFactTypeStatement = prApplication.ORMQL.CreateFactTypeStatement

                        lrCreateFactTypeStatement.FACTTYPENAME = ""
                        lrCreateFactTypeStatement.MODELELEMENTNAME.Clear()
                        lrCreateFactTypeStatement.PREDICATE.Clear()

                        '----------------------------------
                        'Get the Tokens from the ParseTree
                        '----------------------------------
                        Call Me.Model.ORMQL.GetParseTreeTokensReflection(lrCreateFactTypeStatement, Me.ParseTree.Nodes(0))

                        Me.CurrentSentence.ModelElement = lrCreateFactTypeStatement.MODELELEMENTNAME
                        Dim lsPredicatePart As String
                        Dim lrPredicatePart As Language.PredicatePart
                        For Each lsPredicatePart In lrCreateFactTypeStatement.PREDICATE
                            lrPredicatePart = New Language.PredicatePart
                            lrPredicatePart.PredicatePartText = Trim(lsPredicatePart)
                            Me.CurrentSentence.PredicatePart.Add(lrPredicatePart)
                        Next
                        '--------------------------------------------------------------------------------
                        'Add an empty Predicate Part to the end for the new Fact Type Reading model,
                        '  as per the FBMWG08 working draft model (Fact-Based Modeling, Working Group).
                        '--------------------------------------------------------------------------------
                        Me.CurrentSentence.PredicatePart.Add(New Language.PredicatePart)
                End Select
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Oops, there was a problem:"
            lsMessage &= vbCrLf & "Error: " & mb.ReflectedType.Name & "." & mb.Name

            Me.send_data(lsMessage)

        End Try
    End Sub

#End Region

End Class
