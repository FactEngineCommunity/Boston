Imports System
Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports NAudio.Wave
Imports NAudio.Lame
Imports System.Threading
Imports System.Reflection

Public Class frmToolboxBrainBox

    Private inputbuffer_pointer As Integer = 1
    Private inputbuffer As New List(Of String)
    Private zbDictationMode As Boolean = False
    Private zbSentence As Language.Sentence
    Public zsIntellisenseBuffer As String = ""

    Public zrScanner As VAQL.Scanner
    Public zrParser As VAQL.Parser
    Public WithEvents zrTextHighlighter As VAQL.TextHighlighter
    Private AutoComplete As frmAutoComplete
    Private zrTree As New VAQL.ParseTree

    Private WithEvents Brain As tBrain = prApplication.Brain

    Private synchronizationContext As SynchronizationContext

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Capture the synchronization context of the UI thread
        synchronizationContext = SynchronizationContext.Current

    End Sub

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frmToolboxBrainBox_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        prApplication.RightToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub frm_Brain_box_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Call Me.HideBriana()

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)
        Me.Hide()

    End Sub

    Private Sub HideBriana()

        Try
            '=================================
            'Hide Briana
            '====================
            If prApplication.Brain.Page IsNot Nothing Then
                If prApplication.Brain.Page.Language = pcenumLanguage.ORMModel Then
                    If prApplication.Brain.Page.Form IsNot Nothing Then
                        prApplication.Brain.Page.Form.Briana.Visible = False
                    End If
                End If
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub frmToolboxBrainBox_Leave(sender As Object, e As EventArgs) Handles Me.Leave

        Try
            'Call Me.AutoComplete.Hide()
            Me.TextBoxInput.Text = ""

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub frm_Brain_box_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '----------------------------------------------------------------
        'Rregister the Input and Output channels with the Boston.Brain
        '----------------------------------------------------------------
        Try
            If prApplication.Brain Is Nothing Then
                prApplication.Brain = New tBrain
            End If

            prApplication.Brain.Page = prApplication.WorkingPage
            prApplication.Brain.Model = prApplication.WorkingModel
            prApplication.Brain.VAQLProcessor = New VAQL.Processor(prApplication.WorkingModel)

            Dim thread As New Thread(AddressOf prApplication.Brain.VAQLProcessor.setDynamicObjects)
            thread.Start()

            prApplication.Brain.InputChannel = Me.TextBoxInput
            prApplication.Brain.OutputChannel = Me.TextBox_Output
            prApplication.Brain.EchoInput = True
            prApplication.Brain.IncludeSenderInOutput = True
            prApplication.Brain.PressForAnswer = True

            '=================================
            'Make Briana visible
            '====================
            If My.Settings.DisplayBrianaVirtualAnalyst Then
                prApplication.Brain.Page.Form.Briana.Visible = True
            End If

            '------------------------------------------------
            'Check for Quiet Mode
            '----------------------
            Me.ToolStripMenuItemQuietMode.Checked = My.Settings.StartVirtualAnalystInQuietMode
            prApplication.Brain.QuietMode = My.Settings.StartVirtualAnalystInQuietMode

            Select Case My.Settings.DefaultBrainMode
                Case Is = pcenumBrainMode.NaturalLanguage.ToString
                    prApplication.Brain.ThoughtMode = pcenumBrainMode.NaturalLanguage
                Case Else
                    prApplication.Brain.ThoughtMode = pcenumBrainMode.ORMQL
            End Select

            Call Me.LoadEnterpriseAwareListbox()
            Me.TextBoxInput.Focus()

            Me.StatusLabelMain.Text = ""

            '-------------------------------------------------------
            'Setup the Parser etc
            '---------------------
            zrScanner = New VAQL.Scanner
            zrParser = New VAQL.Parser(zrScanner)

            Me.zrTextHighlighter = New VAQL.TextHighlighter(
                                   Me.TextBoxInput,
                                   Me.zrScanner,
                                   Me.zrParser)

            'Me.TextMarker = New TinyPG.Controls.TextMarker(Me.TextBoxQuery)

            Me.AutoComplete = New frmAutoComplete(Me.TextBoxInput)
            Me.AutoComplete.mbSpaceActionEqualsTabAction = True

            '==============================================================
            'Make the Brain useful/user-friendly
            prApplication.Brain.send_data("Type in a Fact Type Reading, like 'Person has first-Name'")
            prApplication.Brain.send_data("...and I will help you create the ORM diagram.")

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub setup(ByRef arPage As FBM.Page)

        Try
            prApplication.Brain.Page = arPage

            If arPage IsNot Nothing Then
                prApplication.Brain.Model = arPage.Model
            End If

            Me.TextBoxInput.Focus()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub append_text(ByVal box As RichTextBox, ByVal color As Color, ByVal text As String)

        Dim start As Integer = box.TextLength
        box.AppendText(text)
        Dim li_end As Integer = box.TextLength

        Try
            ' Textbox may transform chars, so (end-start) != text.Length
            box.Select(start, li_end - start)

            box.SelectionColor = color
            ' could set box.SelectionBackColor, box.SelectionFont too.

            box.SelectionLength = 0 ' // clear

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBox_Input_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxInput.Enter

        Try
            prApplication.Brain.Page = prApplication.WorkingPage
            prApplication.Brain.Model = prApplication.WorkingModel

            Call Me.SetThoughtModeCursor()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBox_Input_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxInput.GotFocus

        Try
            'Not elegant, but hide the AutoComplete form in the ORMReadingEditor (in case it is open).
            Dim lfrmORMReadingEditor As frmToolboxORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)
            If lfrmORMReadingEditor IsNot Nothing Then
                Call lfrmORMReadingEditor.AutoComplete.Hide()
            End If


            Select Case prApplication.Brain.ThoughtMode
                Case Is = pcenumBrainMode.ORMQL
                    If Trim(Replace(Me.TextBoxInput.Text, "ORMQL:", "")).Length = 0 Then
                        Call Me.SetThoughtModeCursor()
                    End If
                Case Is = pcenumBrainMode.NaturalLanguage
                    If Trim(Replace(Me.TextBoxInput.Text, "NL:", "")).Length = 0 Then
                        Call Me.SetThoughtModeCursor()
                    End If
                Case Is = pcenumBrainMode.OSM
                Case Is = pcenumBrainMode.NaturalLanguage
                    If Trim(Replace(Me.TextBoxInput.Text, "OSM:", "")).Length = 0 Then
                        Call Me.SetThoughtModeCursor()
                    End If
            End Select

            Dim lsLastCharacter As String = ""

            Try
                lsLastCharacter = Me.TextBoxInput.Text.Last
                If lsLastCharacter Then
                    Me.zsIntellisenseBuffer = ""
                End If
            Catch ex As Exception
                'tried
                Me.zsIntellisenseBuffer = ""
            End Try

            Me.TextBoxInput.Focus()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub SetThoughtModeCursor()

        Try
            'CodeSafe
            If Me.Width < 10 Or Me.Height < 10 Then Exit Sub


            Select Case prApplication.Brain.ThoughtMode
                Case Is = pcenumBrainMode.ORMQL
                    If Me.TextBoxInput.Find("ORMQL: ") = -1 Then
                        Me.TextBoxInput.Text &= "ORMQL: "
                        Me.TextBoxInput.Find("ORMQL: ")
                        Me.TextBoxInput.SelectionColor = Color.Blue
                        Me.TextBoxInput.SelectionProtected = True
                        Me.TextBoxInput.DeselectAll()
                        Me.TextBoxInput.Select("ORMQL: ".Length, 0)
                        Me.TextBoxInput.DeselectAll()
                    End If
                Case Is = pcenumBrainMode.NaturalLanguage
#Region "Natural Language"
                    If Me.TextBoxInput.Find("NL: ") = -1 Then
                        Me.TextBoxInput.Text = "NL: "
                        Me.TextBoxInput.Find("NL: ")
                        Me.TextBoxInput.DeselectAll()
                        Me.TextBoxInput.SelectionColor = Color.Blue
                        Me.TextBoxInput.Select(0, "NL: ".Length)
                        'Me.TextBoxInput.SelectionProtected = True
                        Me.TextBoxInput.DeselectAll()
                    End If


                    Me.TextBoxInput.Select(0, Me.TextBoxInput.Text.Length)
                    Me.TextBoxInput.SelectionColor = Color.SteelBlue
                    Me.TextBoxInput.SelectionProtected = False
                    Me.TextBoxInput.DeselectAll()

                    Me.TextBoxInput.Select(0, "NL: ".Length)
                    Me.TextBoxInput.SelectionColor = Color.Blue
                    'Me.TextBoxInput.SelectionProtected = True


                    '------------------------------------
                    'Set cursor position to end of text
                    '  NB Have to use Select method
                    '------------------------------------
                    Me.TextBoxInput.Select(Me.TextBoxInput.Text.Length, 0)
                    Me.TextBoxInput.SelectionColor = Color.SteelBlue
#End Region
                Case Is = pcenumBrainMode.OSM
#Region "OSM - Observable STate Machine | Neural Processing Unit"
                    If Me.zrTextHighlighter IsNot Nothing Then Me.zrTextHighlighter.Dispose()

                    If Me.TextBoxInput.Find("OSM: ") = -1 Then
                        Me.TextBoxInput.Text = "OSM: "
                        Me.TextBoxInput.Find("OSM: ")
                        Me.TextBoxInput.DeselectAll()
                        Me.TextBoxInput.SelectionColor = Color.Blue
                        Me.TextBoxInput.Select(0, "OSM: ".Length)
                        'Me.TextBoxInput.SelectionProtected = True
                        Me.TextBoxInput.DeselectAll()
                    End If


                    Me.TextBoxInput.Select(0, Me.TextBoxInput.Text.Length)
                    Me.TextBoxInput.SelectionColor = Color.SteelBlue
                    Me.TextBoxInput.SelectionProtected = False
                    Me.TextBoxInput.DeselectAll()

                    Me.TextBoxInput.Select(0, "OSM: ".Length)
                    Me.TextBoxInput.SelectionColor = Color.Blue
                    'Me.TextBoxInput.SelectionProtected = True


                    '------------------------------------
                    'Set cursor position to end of text
                    '  NB Have to use Select method
                    '------------------------------------
                    Me.TextBoxInput.Select(Me.TextBoxInput.Text.Length, 0)
                    Me.TextBoxInput.SelectionColor = Color.SteelBlue
#End Region
            End Select

            '===========================================================================
            'Ancillary
            'Model
            If prApplication.Brain.Model Is Nothing Then
                Me.ToolStripStatusLabelModel.Text = "None selected"
            Else
                Me.ToolStripStatusLabelModel.Text = prApplication.Brain.Model.Name
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub TextBox_Input_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBoxInput.KeyDown

        Try
            'CodeSafe
            Select Case prApplication.Brain.ThoughtMode
                Case Is = pcenumBrainMode.OSM
                    Me.zrTextHighlighter.Dispose()
            End Select

            Select Case e.KeyCode
                Case Is = Keys.Up, Keys.Down
                    If Me.ToolStripMenuItemDictationMode.Checked Then
                        Me.zbDictationMode = False
                        Me.StatusLabelMain.Text = "Dictation: Suspended"
                    Else
                        Me.StatusLabelMain.Text = "Dictation: Off"
                    End If
                Case Is = Keys.Enter
                    If Me.ToolStripMenuItemDictationMode.Checked Then
                        Me.zbDictationMode = True
                        Me.StatusLabelMain.Text = "Dictation: On"
                    Else
                        Me.StatusLabelMain.Text = "Dictation: Off"
                    End If
                Case Else
                    If Me.ToolStripMenuItemDictationMode.Checked Then
                        Me.zbDictationMode = False
                        Me.StatusLabelMain.Text = "Dictation: Suspended"
                    Else
                        Me.StatusLabelMain.Text = "Dictation: Off"
                    End If
            End Select

            Select Case e.KeyCode
                Case Is = Keys.Back
                    If zsIntellisenseBuffer.Length > 0 Then
                        zsIntellisenseBuffer = zsIntellisenseBuffer.Substring(0, zsIntellisenseBuffer.Length - 1)
                    End If
                Case Is = Keys.Up 'UpArrow
#Region "Up Key"
                    'Ability to scroll back through previous inputs.
                    If Me.inputbuffer.Count > 0 Then
                        Me.inputbuffer_pointer -= 1
                        If Me.inputbuffer_pointer < 0 Then Me.inputbuffer_pointer = Me.inputbuffer.Count - 1
                        Me.TextBoxInput.Text = ""
                        Call Me.SetThoughtModeCursor()
                        Select Case Trim(Me.inputbuffer(Me.inputbuffer_pointer).ToLower)
                            Case Is = "yes", "no"
                                If Me.inputbuffer_pointer - 1 >= 0 Then
                                    Me.inputbuffer_pointer -= 1
                                Else
                                    Me.inputbuffer_pointer = Me.inputbuffer.Count - 1
                                End If
                        End Select
                        Me.TextBoxInput.AppendText(LTrim(Me.inputbuffer(Me.inputbuffer_pointer)))
                    Else
                        'Me.TextBoxInput.Clear()
                    End If
                    e.Handled = True
#End Region
                Case Is = Keys.Down  'DownArrow
#Region "Down Arrow Key"
                    '============================================
                    'If Optionals exist, then show AutoComplete

                    '20231124-VM-Removed 
                    Me.zrTextHighlighter.Tree = Me.zrParser.Parse(Me.TextBoxInput.Text & " ")
                    If (Me.zrTextHighlighter.Tree.Errors.Count > 0) Or (Me.zrTextHighlighter.Tree.Optionals.Count > 0) Then
                        Call Me.ProcessAutoComplete()
                        e.Handled = True
                        Exit Sub
                    End If

                    '======================================
                    If Me.inputbuffer.Count > 0 Then
                        Me.inputbuffer_pointer -= 1
                        If Me.inputbuffer_pointer < 0 Then Me.inputbuffer_pointer = Me.inputbuffer.Count - 1
                        'CodeSafe
                        If Me.inputbuffer_pointer = Me.inputbuffer.Count Then Me.inputbuffer_pointer = Me.inputbuffer.Count - 1
                        Me.TextBoxInput.Text = ""
                        Call Me.SetThoughtModeCursor()
                        Select Case Trim(Me.inputbuffer(Me.inputbuffer_pointer).ToLower)
                            Case Is = "yes", "no"
                                If Me.inputbuffer_pointer - 1 <= 0 Then
                                    Me.inputbuffer_pointer -= 1
                                End If
                        End Select
                        Me.TextBoxInput.AppendText(LTrim(Me.inputbuffer(Me.inputbuffer_pointer)))
                    Else
                        'Me.TextBoxInput.Clear()
                    End If
                    e.Handled = True
#End Region
                Case Is = Keys.Enter 'Enter
#Region "Enter Key"
                    Me.AutoComplete.Hide()

                    If Not prApplication.Brain.QuietMode And Me.zbDictationMode Then
                        Me.ToolStripSplitButtonMike.Image = My.Resources.MenuImages.Mike_Red24x24
                        Me.ToolStripSplitButtonMike.Invalidate()
                        Me.Refresh()
                    End If

                    '--------------------------------------------
                    'Firstly, strip away the ThoughtMode prompt
                    '--------------------------------------------
                    Select Case prApplication.Brain.ThoughtMode
                        Case Is = pcenumBrainMode.ORMQL
                            Me.TextBoxInput.Text = Replace(Me.TextBoxInput.Text, "ORMQL:", "")
                        Case Is = pcenumBrainMode.NaturalLanguage
                            Me.TextBoxInput.Text = Replace(Me.TextBoxInput.Text, "NL:", "")
                        Case Is = pcenumBrainMode.OSM
                            Me.TextBoxInput.Text = Replace(Me.TextBoxInput.Text, "OSM:", "")
                    End Select

                    '----------------------------------
                    'Check for BrainBox reserved words
                    '----------------------------------
                    Select Case LCase(Trim(Me.TextBoxInput.Text))
                        Case Is = "clear"
                            Me.TextBoxInput.Clear()
                            Me.TextBox_Output.Clear()
                            e.SuppressKeyPress = True
                            Exit Sub
                        Case Is = "exit"
                            Call Me.HideBriana()

                            prApplication.Brain = New tBrain
                            Me.Close()
                            frmMain.zfrm_Brain_box = Nothing
                            Exit Sub
                    End Select

                    Select Case LCase(Trim(Me.TextBoxInput.Text))
                        Case Is = "yes", "no", "abort"
                        Case Else
                            Me.inputbuffer.AddUnique(Me.TextBoxInput.Text)
                    End Select

                    If Me.inputbuffer.Count >= 10 Then
                        Me.inputbuffer.RemoveAt(0)
                    End If

                    Dim lsInput = Trim(Me.TextBoxInput.Text)
                    Me.TextBoxInput.Clear()

                    '----------------------------------
                    'Send data to the Boston.Brain
                    '----------------------------------
                    If CheckIfModelPageSelected() Then
                        Me.inputbuffer_pointer = Me.inputbuffer.Count
                        'Brain receives the data.
                        prApplication.Brain.receive_data(lsInput)
                    ElseIf prApplication.Brain IsNot Nothing Then
                        prApplication.Brain.receive_data(lsInput)
                    End If

                    Call Me.SetThoughtModeCursor()

                    e.SuppressKeyPress = True

                    Me.zsIntellisenseBuffer = ""
                'Case Is = Keys.OemPeriod  '(e.KeyChar = ".") Then
                '    '-------------------------------------------------------------------------------------
                '    'User wants to view the EnterpriseAware listbox. Has hit the '.' key on their keypad
                '    '-------------------------------------------------------------------------------------
                '    zsIntellisenseBuffer = ""

                '    Call Me.PopulateEnterpriseAwareWithObjectTypes(Me.zsIntellisenseBuffer)
                '    Me.AutoComplete.Enabled = True
                '    'Me.AutoComplete.TransparencyColour = Color.White
                '    Call Me.ShowAutoCompleteTool()


                '    'Call Me.LoadEnterpriseAwareListbox()

                '    'Dim lo_point As New Point(TextBoxInput.GetPositionFromCharIndex(TextBoxInput.SelectionStart))
                '    'lo_point.X += TextBoxInput.Bounds.X
                '    'lo_point.Y += TextBoxInput.Bounds.Y
                '    'lo_point.Y += CInt(TextBoxInput.Font.GetHeight()) + 5
                '    'lo_point.Y += SplitContainer1.Panel2.Location.Y
                '    'ListBoxEnterpriseAware.Location = lo_point
                '    'ListBoxEnterpriseAware.Show()

                '    'Me.ActiveControl = Me.ListBoxEnterpriseAware

                '    e.Handled = True
                '    e.SuppressKeyPress = True
                Case Is = Keys.Shift, Keys.ShiftKey
                'Do nothing
                Case Is = Keys.Space, Keys.OemMinus

                    Me.zsIntellisenseBuffer = ""
                Case Else
                    Try
                        zsIntellisenseBuffer = Me.TextBoxInput.Text.ToString.AppendString(LCase(e.KeyCode.ToString)).Split(" ").Last().ToLower
                    Catch ex As Exception
                        zsIntellisenseBuffer &= LCase(e.KeyCode.ToString)
                    End Try

#End Region
            End Select


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Function CheckIfModelPageSelected() As Boolean

        Try
            If IsNothing(prApplication.Brain.Model) Then
                '-----------------------------------------------
                'Try and set the Model from the EnterpriseTree
                '-----------------------------------------------
                If frmMain.zfrmModelExplorer IsNot Nothing Then
                    If frmMain.zfrmModelExplorer.TreeView.SelectedNode IsNot Nothing Then
                        Dim lrMenu As tEnterpriseEnterpriseView
                        lrMenu = frmMain.zfrmModelExplorer.TreeView.SelectedNode.Tag
                        Select Case lrMenu.MenuType
                            Case Is = pcenumMenuType.pageORMModel
                                Dim lrPage As FBM.Page = lrMenu.Tag
                                prApplication.WorkingModel = lrPage.Model
                                prApplication.Brain.Model = lrPage.Model
                                prApplication.WorkingPage = prApplication.WorkingModel.Page.Find(AddressOf lrPage.Equals)
                                Return True
                            Case Else
                                Return False
                        End Select
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Sub TextBox_Output_GotFocus(sender As Object, e As EventArgs) Handles TextBox_Output.GotFocus

        Try
            Me.TextBoxInput.Focus()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBox_Output_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBox_Output.MouseDown

        Try
            Me.TextBoxInput.Focus()

            If e.Button = Windows.Forms.MouseButtons.Right Then
                Me.TextBox_Output.ContextMenuStrip = Me.ContextMenuStripBrainBox
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBox_Output_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_Output.TextChanged

        Try
            Me.TextBox_Output.ScrollToCaret()

            Me.AutoComplete.Hide()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBoxInput_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBoxInput.KeyUp

        Try
            'CodeSafe
            Select Case prApplication.Brain.ThoughtMode
                Case Is = pcenumBrainMode.OSM
                    Me.zrTextHighlighter.Dispose()
                    GoTo HideAutoComplete
                Case Else
                    Me.zrTextHighlighter.Tree = Me.zrParser.Parse(Me.TextBoxInput.Text.Trim & " ")
                    If Me.zrTextHighlighter.Tree.Errors.Count = 0 Then
                        Call Me.zrTextHighlighter.HighlightTextInternal()
                    End If
            End Select


            If (e.KeyCode = Keys.Down Or Trim(Me.TextBoxInput.Text) <> "NL:") And Not e.KeyCode = Keys.Space Then
                Call Me.ProcessAutoComplete(e)
            ElseIf Me.TextBoxInput.Text(Me.TextBoxInput.Text.Length - 1) = " " Then
                Call Me.ProcessAutoComplete(e)
            End If

            If e.KeyCode = Keys.Escape Then
HideAutoComplete:
                Me.AutoComplete.Hide()
            End If

            e.Handled = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBox_Input_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBoxInput.MouseDown

        Try
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Me.TextBoxInput.ContextMenuStrip = Me.ContextMenuVirtualAnalyst
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Async Sub DictationModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemDictationMode.Click

        Try
            Me.ToolStripMenuItemDictationMode.Checked = Me.zbDictationMode
            Me.ToolStripMenuItemDictationMode.Checked = Not Me.ToolStripMenuItemDictationMode.Checked

            Me.zbDictationMode = Me.ToolStripMenuItemDictationMode.Checked

            If Me.zbDictationMode Then
                Me.StatusLabelMain.Text = "Dictation: On"
                prApplication.Brain.ConversationMode = pcenumOSMConversationMode.RealTimeTranscription

                'Assembly AI
                prApplication.Brain.httpClient = New HttpClient()
                Using prApplication.Brain.httpClient
                    prApplication.Brain.httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", My.Settings.OSMAssemblyAIAPIKey)
                End Using

                If prApplication.Brain.AssemblyAIRelTimeTranscriber IsNot Nothing Then
                    prApplication.Brain.AssemblyAIRelTimeTranscriber.Dispose()
                End If
                If prApplication.Brain.AssemblyAIRelTimeTranscriber Is Nothing Then
                    prApplication.Brain.AssemblyAIRelTimeTranscriber = New AssemblyAIRealtimeClient(Me.TextBoxInput)
                End If
                Using prApplication.Brain.AssemblyAIRelTimeTranscriber
                    Await prApplication.Brain.AssemblyAIRelTimeTranscriber.StartTranscriptionAsync() 'NB us poCancellationTokenSource in PublicVariablesOSM to cancel transcription 
                End Using

            Else
                Me.StatusLabelMain.Text = "Dictation: Off"
                prApplication.Brain.ConversationMode = pcenumOSMConversationMode.ChatBot
                If prApplication.Brain.AssemblyAIRelTimeTranscriber IsNot Nothing Then
                    Call prApplication.Brain.AssemblyAIRelTimeTranscriber.Dispose()
                    prApplication.Brain.AssemblyAIRelTimeTranscriber = Nothing
                    poCancellationTokenSource.Cancel()
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub TimerInput_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerInput.Tick

        Dim lsText As String = ""

        Try
            Me.TimerInput.Stop()

            If Me.zbDictationMode Then
                If Me.TextBoxInput.Text <> "" Then

                    Select Case prApplication.Brain.ThoughtMode
                        Case Is = pcenumBrainMode.ORMQL
                            lsText = Trim(Replace(Me.TextBoxInput.Text, "ORMQL:", ""))
                        Case Is = pcenumBrainMode.NaturalLanguage
                            lsText = Trim(Replace(Me.TextBoxInput.Text, "NL:", ""))
                        Case Is = pcenumBrainMode.OSM
                            lsText = Trim(Replace(Me.TextBoxInput.Text, "OSM:", ""))
                    End Select

                    Me.zbSentence = New Language.Sentence(Trim(lsText))

                    Call Language.AnalyseSentence(Me.zbSentence)
                    Call Language.ProcessSentence(Me.zbSentence)
                    If Me.zbSentence.AreAllWordsResolved Then
                        Call Language.ResolveSentence(Me.zbSentence)
                    End If

                    If Me.zbSentence.POStaggingResolved Then
                        '----------------------------------
                        'Send data to the Boston.Brain
                        '----------------------------------
                        Select Case lsText
                            Case Is = "yes", "no"
                                Me.inputbuffer.Add(Me.zbSentence.Sentence)
                                If Me.inputbuffer.Count >= 10 Then
                                    Me.inputbuffer.RemoveAt(0)
                                End If

                                If CheckIfModelPageSelected() Then
                                    prApplication.Brain.receive_data(Trim(Me.zbSentence.Sentence))
                                End If

                                Me.TextBoxInput.Clear()
                                Call Me.SetThoughtModeCursor()
                        End Select
                    End If

                End If
            End If

            Me.TextBoxInput.Focus()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub TextBox_Input_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxInput.TextChanged

        Try
            If Me.zbDictationMode Then
                Me.TimerInput.Start()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ListBoxEnterpriseAware_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBoxEnterpriseAware.KeyUp

        Dim ObjToSelect As New Object

        Try

            If Not (e.KeyCode = Keys.OemPeriod) Then


                Select Case e.KeyCode
                    Case Is = Keys.Escape
                        Me.ListBoxEnterpriseAware.Hide()
                        Me.ActiveControl = Me.TextBoxInput
                    Case Is = Keys.Back
                        If zsIntellisenseBuffer.Length > 0 Then
                            zsIntellisenseBuffer = zsIntellisenseBuffer.Substring(0, zsIntellisenseBuffer.Length - 1)
                        End If
                        Call Me.LoadEnterpriseAwareListbox()

                    Case Is = Keys.Up, Keys.Down
                        '------------
                        'Do nothing
                        '------------
                        Exit Sub
                    Case Is = Keys.Return, Keys.Space
                        '------------------------------------------
                        'User is selecting an item from the list.
                        '------------------------------------------
                        Dim lsModelObjectName As String = ListBoxEnterpriseAware.SelectedItem.ToString()

                        '-------------------------------
                        'Remove the item from the list
                        '-------------------------------
                        Me.ListBoxEnterpriseAware.Items.Remove(ListBoxEnterpriseAware.SelectedItem)
                        Me.ListBoxEnterpriseAware.Hide()

                        Me.TextBoxInput.DeselectAll()
                        Me.TextBoxInput.AppendText(lsModelObjectName)
                        Me.TextBoxInput.Select(Me.TextBoxInput.Text.Length - lsModelObjectName.Length, lsModelObjectName.Length + 1)
                        Me.TextBoxInput.SelectionColor = Color.SteelBlue
                        Me.TextBoxInput.SelectionProtected = False


                        Me.ActiveControl = Me.TextBoxInput

                    Case Else
                        Dim lrListbox As New System.Windows.Forms.ListBox
                        Dim larObject As New System.Windows.Forms.ListBox.ObjectCollection(lrListbox)
                        Dim lbStartsWith As Boolean = False
                        Dim lrModelObjectName As Object

                        'zsIntellisenseBuffer &= LCase(e.KeyData.ToString)


                        For Each lrModelObjectName In ListBoxEnterpriseAware.Items
                            larObject.Add(lrModelObjectName)
                        Next

                        For Each lrModelObjectName In larObject
                            Dim str As String = LCase(lrModelObjectName.Text) 'ToString())
                            If Not (str = "") Then
                                lbStartsWith = str.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture)
                                If lbStartsWith Then
                                    ObjToSelect = lrModelObjectName
                                Else
                                    ListBoxEnterpriseAware.Items.Remove(lrModelObjectName)
                                End If
                            End If
                        Next

                        If ObjToSelect.GetType.ToString = GetType(Object).ToString Then
                            zsIntellisenseBuffer = ""
                            ListBoxEnterpriseAware.Hide()
                            Me.ActiveControl = Me.TextBoxInput
                        Else
                            ListBoxEnterpriseAware.SelectedItem = ObjToSelect
                        End If

                End Select
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub LoadEnterpriseAwareListbox()
        '------------------------------------------------------------
        'Loads the list of ORM Object Type names within the FactType
        '  within the EnterpriseAware listbox.
        '------------------------------------------------------------
        Dim lrEntityType As FBM.EntityType
        Dim liInd As Integer = 0

        Try
            Me.ListBoxEnterpriseAware.Items.Clear()

            'CodeSafe
            If prApplication.WorkingModel Is Nothing Then Exit Sub

            For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
                liInd += 1
                If zsIntellisenseBuffer.Length > 0 Then
                    If lrEntityType.Name.StartsWith(zsIntellisenseBuffer) Then
                        Me.ListBoxEnterpriseAware.Items.Add(New tComboboxItem(liInd, lrEntityType.Name))
                    End If
                Else
                    Me.ListBoxEnterpriseAware.Items.Add(New tComboboxItem(liInd, lrEntityType.Name))
                End If
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub QuietModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemQuietMode.Click

        Try
            Me.ToolStripMenuItemQuietMode.Checked = Not Me.ToolStripMenuItemQuietMode.Checked

            prApplication.Brain.QuietMode = Me.ToolStripMenuItemQuietMode.Checked
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub GetMODELELEMENTParseNodes(ByRef arParseNode As VAQL.ParseNode, ByRef aarParseNode As List(Of VAQL.ParseNode))

        Dim lrParseNode As VAQL.ParseNode

        Try
            If arParseNode.Token.Type = VAQL.TokenType.MODELELEMENTNAME Then
                aarParseNode.Add(arParseNode)
            End If

            For Each lrParseNode In arParseNode.Nodes
                Call GetMODELELEMENTParseNodes(lrParseNode, aarParseNode)
            Next
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Public Sub CheckStartProductions(ByRef arParseTree As VAQL.ParseTree)

        Try
            'If (arParseTree.Depth(0) = 3) And (arParseTree.Count(1) <= 5) Then
            If Trim(Me.TextBoxInput.Text) = "NL:" Then
                'arParseTree.Optionals.Add(New VAQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDREADING"))
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Function CheckIfCanDisplayEnterpriseAwareBox()

        Try
            If Me.AutoComplete.ListBox.Items.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Sub AddEnterpriseAwareItem(ByVal asEAItem As String, Optional ByVal aoTagObject As Object = Nothing, Optional ByVal abInsertAtBeginning As Boolean = False)

        Dim lrListItem As tComboboxItem

        Try
            lrListItem = New tComboboxItem(asEAItem, asEAItem, aoTagObject)

            If (asEAItem <> "") And Not (Me.AutoComplete.ListBox.FindStringExact(asEAItem) >= 0) Then
                If abInsertAtBeginning Then
                    Me.AutoComplete.ListBox.Items.Insert(0, lrListItem)
                Else
                    Me.AutoComplete.ListBox.Items.Add(lrListItem)
                End If
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AddPredicatePartsToEnterpriseAware(ByVal aarPredicatePart As List(Of FBM.PredicatePart))

        Dim lrPredicatePart As FBM.PredicatePart

        Try
            '--------------------------------------------------
            'Code Safe/Smart
            '----------------
            If aarPredicatePart.Count = 0 Then
                Exit Sub
            End If

            Me.AutoComplete.ListBox.Sorted = True

            For Each lrPredicatePart In aarPredicatePart
                Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, VAQL.TokenType.PREDICATEPART)
            Next

            'Me.AutoComplete.Show()
            Me.AutoComplete.Owner = Me
            Me.AutoComplete.ListBox.Focus()
            If aarPredicatePart.Count > 0 Then
                Me.AutoComplete.ListBox.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub PopulateEnterpriseAwareFromOptionals(ByVal aarParseErrors As VAQL.ParseErrors)

        Dim lrParseError As VAQL.ParseError
        Dim lsToken As String = ""
        Dim liTokenType As VAQL.TokenType
        Dim lbTokenAdded As Boolean = False

        Try
            For Each lrParseError In aarParseErrors

                lbTokenAdded = False

                liTokenType = DirectCast([Enum].Parse(GetType(VAQL.TokenType), lrParseError.ExpectedToken), VAQL.TokenType)
                Select Case liTokenType
                    Case Is = VAQL.TokenType.BROPEN
                        Call Me.AddEnterpriseAwareItem("(", liTokenType)
                        lbTokenAdded = True
                    Case Is = VAQL.TokenType.PREDICATEPART
                        'Dim lrModelElement As FBM.ModelObject
                        Dim lsModelElementName As String
                        lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
                    ''lrModelElement = prApplication.WorkingModel.GetModelElementByName(lsModelElementName)
                    'If lrModelElement IsNot Nothing Then
                    '    Call Me.AddPredicatePartsToEnterpriseAware(prBradfordApplication.Database.MetaDataManager.GetPredicatePartsForModelObject(lrModelElement))
                    'Else
                    '    Dim larCharBeginning() As Char = {"("}
                    '    Dim larCharEnd() As Char = {")"}
                    '    lsModelElementName = lsModelElementName.TrimStart(larCharBeginning).TrimEnd(larCharEnd)
                    '    'lrModelElement = prApplication .WorkingModel.GetModelElementByName(lsModelElementName)
                    '    If lrModelElement IsNot Nothing Then
                    '        Call Me.AddPredicatePartsToEnterpriseAware(prBradfordApplication.Database.MetaDataManager.GetPredicatePartsForModelObject(lrModelElement))
                    '    End If
                    'End If
                    Case Is = VAQL.TokenType.UNARYPREDICATEPART
                        'Do Nothing
                        lbTokenAdded = True
                    Case Is = VAQL.TokenType.REFERENCEMODE
                        Dim items = System.Enum.GetValues(GetType(pcenumReferenceMode))
                        For Each item As pcenumReferenceModeEndings In items
                            Call Me.AddEnterpriseAwareItem(GetEnumDescription(item).Replace(".", ""),,)
                        Next
                    Case Is = VAQL.TokenType.MODELELEMENTNAME
                        '----------------------------------------------------
                        '20180311-Sometimes is not triggered when half way through writing a ModelElementName...esp when at beginning of a FactTypeReading.
                        Call Me.PopulateEnterpriseAwareWithObjectTypes(Me.zsIntellisenseBuffer)
                        lbTokenAdded = True
                    Case Is = VAQL.TokenType.PREBOUNDREADINGTEXT,
                              VAQL.TokenType.POSTBOUNDREADINGTEXT,
                              VAQL.TokenType.FOLLOWINGREADINGTEXT,
                              VAQL.TokenType.FRONTREADINGTEXT,
                              VAQL.TokenType.PREDICATESPACE
                        '------------
                        'Do nothing
                        '------------
                        lbTokenAdded = True
                    Case Else
                        If Me.zrScanner.Patterns(liTokenType).ToString.ToLower.StartsWith(zsIntellisenseBuffer.ToLower) Or
                            Me.zrScanner.Patterns(liTokenType).ToString.ToLower.Contains(zsIntellisenseBuffer.ToLower) Then
                            Call Me.AddEnterpriseAwareItem(Me.zrScanner.Patterns(liTokenType).ToString, liTokenType, True)
                            lbTokenAdded = True
                        End If

#Region "Check last words of input text"
                        ' Split the input text into words
                        Dim words As String() = Me.TextBoxInput.Text.Trim.Split(" "c).Select(Function(w) w.ToLower()).ToArray()

                        ' Initialize a variable to store the accumulated set of words
                        Dim accumulatedWords As String = ""

                        ' Start from the last word and work backward
                        For liInd As Integer = words.Length - 1 To 0 Step -1
                            ' Add the current word to the accumulated set of words
                            accumulatedWords = words(liInd) & " " & accumulatedWords

                            ' Remove any leading or trailing spaces
                            accumulatedWords = accumulatedWords.Trim()

                            ' Check if the accumulated words match the target text
                            If Me.zrScanner.Patterns(liTokenType).ToString.ToLower.StartsWith(accumulatedWords) Then
                                Call Me.AddEnterpriseAwareItem(Me.zrScanner.Patterns(liTokenType).ToString, liTokenType)
                                lbTokenAdded = True
                            End If
                        Next
#End Region
                End Select

                If lbTokenAdded = False Then
                    Call Me.AddEnterpriseAwareItem(Me.zrScanner.Patterns(liTokenType).ToString, liTokenType, False)
                End If
            Next

            If Me.AutoComplete.ListBox.Items.Count > 0 Then
                Me.AutoComplete.Enabled = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace)
        End Try

    End Sub

    Private Sub PopulateEnterpriseAwareWithObjectTypes(ByVal asInputBuffer As String)

        Dim lrValueType As FBM.ValueType
        Dim lrEntityType As FBM.EntityType

        'Select Case e.KeyCode
        '    Case Is = Keys.Back
        '        If zsIntellisenseBuffer.Length > 0 Then
        '            zsIntellisenseBuffer = zsIntellisenseBuffer.Substring(0, zsIntellisenseBuffer.Length - 1)
        '        End If
        '    Case Is = Keys.Space, Keys.Escape, Keys.Down, Keys.Up, Keys.Shift, Keys.ShiftKey
        '        Me.zsIntellisenseBuffer = ""
        '    Case Else
        '        zsIntellisenseBuffer &= LCase(e.KeyCode.ToString)
        'End Select

        Try
            'CodeSafe
            If prApplication.WorkingModel Is Nothing Then Exit Sub

            Dim lbStartsWith As Boolean = False
            lbStartsWith = "asdf".StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture)

            Dim larModelElementParts() = Split(zsIntellisenseBuffer, "-")
            Try
                zsIntellisenseBuffer = larModelElementParts(1)
            Catch ex As Exception
            End Try

            For Each lrValueType In prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
                If zsIntellisenseBuffer.Length > 0 Then
                    If lrValueType.Name.ToLower.StartsWith(zsIntellisenseBuffer.ToLower) Then
                        Call Me.AddEnterpriseAwareItem(lrValueType.Name, VAQL.TokenType.MODELELEMENTNAME, True)
                    End If
                Else
                    Call Me.AddEnterpriseAwareItem(lrValueType.Name, VAQL.TokenType.MODELELEMENTNAME)
                End If
            Next

            For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
                If zsIntellisenseBuffer.Length > 0 Then
                    If lrEntityType.Name.ToLower.StartsWith(zsIntellisenseBuffer.ToLower) Then
                        Call Me.AddEnterpriseAwareItem(lrEntityType.Name, VAQL.TokenType.MODELELEMENTNAME, True)
                    End If
                Else
                    Call Me.AddEnterpriseAwareItem(lrEntityType.Name, VAQL.TokenType.MODELELEMENTNAME)
                End If
            Next

            For Each lrFactType In prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsMDAModelElement = False And x.IsObjectified = True)
                If zsIntellisenseBuffer.Length > 0 Then
                    If lrFactType.Name.ToLower.StartsWith(zsIntellisenseBuffer.ToLower) Then
                        Call Me.AddEnterpriseAwareItem(lrFactType.Name, VAQL.TokenType.MODELELEMENTNAME, True)
                    End If
                Else
                    Call Me.AddEnterpriseAwareItem(lrFactType.Name, VAQL.TokenType.MODELELEMENTNAME)
                End If
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    'Private Function AddFactTypePredicatePartsToEnterpriseAware() As Boolean

    '    Dim lrFactTypeReading As FBM.FactTypeReading
    '    Dim lasPredicatePart As New List(Of String)
    '    Dim lsPredicateReadingText As String = ""

    '    If prBradfordApplication.Database.FactTypeReading.Count > 0 Then

    '        Me.AutoComplete.ListBox.Sorted = True

    '        For Each lrFactTypeReading In prBradfordApplication.Database.FactTypeReading

    '            lsPredicateReadingText = lrLinFuFactTypeReading.GetPredicateReadingText

    '            If Trim(Me.TextBoxQuery.Text).LastIndexOf(lsPredicateReadingText, Trim(Me.TextBoxQuery.Text).Length - 1, lsPredicateReadingText.Length) > 0 Then
    '                lasPredicatePart.Clear()
    '                Exit For
    '            Else
    '                lasPredicatePart.Add(lsPredicateReadingText)
    '            End If
    '        Next

    '        For Each lsPredicateReadingText In lasPredicatePart
    '            Call Me.AddEnterpriseAwareItem(lsPredicateReadingText)
    '        Next

    '        If Me.AutoComplete.ListBox.Items.Count > 0 Then
    '            Me.AutoComplete.ListBox.SelectedIndex = 0
    '        End If
    '    End If

    'End Function

    'Private Sub AddFactTypeReadingsToEnterpriseAware()

    '    Dim lrFactTypeReading As FBM.FactTypeReading

    '    If prBradfordApplication.Database.FactTypeReading.Count > 0 Then
    '        Me.AutoComplete.ListBox.Items.Clear()
    '        Me.AutoComplete.ListBox.Sorted = True

    '        For Each lrFactTypeReading In prBradfordApplication.Database.FactTypeReading

    '            Call Me.AddEnterpriseAwareItem(lrLinFuFactTypeReading.GetBradfordReadingText)
    '        Next


    '        Me.AutoComplete.Show()
    '        Me.AutoComplete.ListBox.Focus()
    '        Me.AutoComplete.ListBox.SelectedIndex = 0
    '    End If

    'End Sub

    Public Sub ProcessAutoComplete(Optional ByRef e As System.Windows.Forms.KeyEventArgs = Nothing)

        Try
            Dim lsExpectedToken As String = ""
            Dim liTokenType As VAQL.TokenType
            Dim lsCurrentTokenType As Object

            'CodeSafe
            'CodeSafe
            Select Case prApplication.Brain.ThoughtMode
                Case Is = pcenumBrainMode.OSM
                    Exit Sub 'Freeform text. Nothing to parse.
            End Select

            'Don't use for OSM (Observable State Machine | Neural Processing Unit)
            If prApplication.Brain.ThoughtMode = pcenumBrainMode.OSM Then
                Exit Sub
            End If

            '-------------------
            'Get the ParseTree
            '-------------------
            Me.zrTextHighlighter.Tree = Me.zrParser.Parse(Me.TextBoxInput.Text & " ")

#Region "Intellisense Buffer"
            Me.zsIntellisenseBuffer = Me.zsIntellisenseBuffer.Trim
            If Me.zsIntellisenseBuffer.Contains(" ") Then Me.zsIntellisenseBuffer = Me.zsIntellisenseBuffer.Split(" ").Last()
            Try
                If Me.TextBoxInput.Text.Trim.Split(" ").Count > 0 Then
                    If Me.TextBoxInput.Text.Trim.Split(" ").Last <> Me.zsIntellisenseBuffer Then
                        Me.zsIntellisenseBuffer = Me.TextBoxInput.Text.Trim.Split(" ").Last.Trim
                    End If
                End If
            Catch ex As Exception
                'All okay
            End Try

            If Me.zsIntellisenseBuffer.Trim = "" Then
                Dim text As String = Me.TextBoxInput.Text.TrimEnd() ' Trim any trailing whitespace
                Dim words As String() = text.Split(New Char() {" "c, ControlChars.Tab}, StringSplitOptions.RemoveEmptyEntries) ' Split the text into words
                Dim lastWord As String = ""

                If words.Length > 0 Then
                    lastWord = words(words.Length - 1) ' The last word is the last element in the array
                    Me.zsIntellisenseBuffer = lastWord
                End If
            End If
#End Region

            Call Me.CheckStartProductions(Me.zrTextHighlighter.Tree)

            'Me.AutoComplete.Hide()
            Me.AutoComplete.ListBox.Items.Clear()

            Dim lrLastToken As VAQL.TokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type

            If (Me.zrTextHighlighter.Tree.Errors.Count > 0) Or (Me.zrTextHighlighter.Tree.Optionals.Count > 0) Or (lrLastToken = VAQL.TokenType.EOF) Then

                If lrLastToken = VAQL.TokenType.EOF Then
                    liTokenType = VAQL.TokenType.EOF
                    GoTo ProcessToken
                ElseIf Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
                    lsExpectedToken = Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken
                Else
                    lsExpectedToken = Me.zrTextHighlighter.Tree.Optionals(0).ExpectedToken
                End If
                If lsExpectedToken <> "" Then
                    liTokenType = DirectCast([Enum].Parse(GetType(VAQL.TokenType), lsExpectedToken), VAQL.TokenType)
                    'MsgBox("Expecting: " & Me.zrScanner.Patterns(liTokenType).ToString)
                End If

                If Me.zrTextHighlighter.Tree.Optionals.Count > 0 Then
                    Call Me.PopulateEnterpriseAwareFromOptionals(Me.zrTextHighlighter.Tree.Optionals)
                End If

ProcessToken:
                Select Case liTokenType
                    Case Is = VAQL.TokenType.BROPEN
                        Me.AutoComplete.Enabled = True
                        Call Me.AddEnterpriseAwareItem("(", liTokenType)
                    Case Is = VAQL.TokenType.BRCLOSE
                        Me.AutoComplete.Enabled = True
                        Call Me.AddEnterpriseAwareItem(")", liTokenType)
                    Case Is = VAQL.TokenType._NONE_
                        Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = VAQL.TokenType.BASEPRODUCTION
                        Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = VAQL.TokenType.PREBOUNDREADINGTEXT,
                              VAQL.TokenType.POSTBOUNDREADINGTEXT,
                              VAQL.TokenType.FOLLOWINGREADINGTEXT,
                              VAQL.TokenType.FRONTREADINGTEXT
                        'Don't add anything 
                    Case Is = VAQL.TokenType.REFERENCEMODE
                        'Don't add anything
                    Case Is = VAQL.TokenType.PREDICATEPART
                        'Don't add anything
                    Case Is = VAQL.TokenType.NUMBER
                        'Don't add anything
                    Case Is = VAQL.TokenType.EOF
                        'Call Me.PopulateEnterpriseAwareWithObjectTypes(Me.zsIntellisenseBuffer)
                    Case Is = VAQL.TokenType.PREDICATESPACE
                        Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = VAQL.TokenType.SPACE
                        Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = VAQL.TokenType.MODELELEMENTNAME, VAQL.TokenType.UNARYPREDICATEPART
                        Me.AutoComplete.Enabled = True
                        Call Me.PopulateEnterpriseAwareWithObjectTypes(Me.zsIntellisenseBuffer)
                    Case Is = VAQL.TokenType.VALUE
                    Case Is = VAQL.TokenType.ID
                    Case Else
                        Me.AutoComplete.Enabled = True
                        Me.AddEnterpriseAwareItem(Me.zrScanner.Patterns(liTokenType).ToString, liTokenType)
                End Select


                lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext
                Dim lrParseNode As VAQL.ParseNode

                If lsCurrentTokenType IsNot Nothing And (Me.TextBoxInput.Text.Length > 0) Then

                    lrParseNode = Me.zrTextHighlighter.GetCurrentContext
                    If lrParseNode.Token.Type = VAQL.TokenType.EOF Then
                        If lrParseNode.Parent IsNot Nothing Then
                            If lrParseNode.Parent.Nodes.Count > 1 Then
                                lsCurrentTokenType = Me.zrTextHighlighter.FindNode(lrParseNode.Parent.Nodes(lrParseNode.Parent.Nodes.Count - 1), 0).Token.Type
                            End If
                        Else
                            lsCurrentTokenType = lrParseNode.Token.Type
                        End If
                    Else
                        lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type
                    End If

                    Select Case lsCurrentTokenType
                        Case Is = VAQL.TokenType.BASEPRODUCTION
                            Me.AutoComplete.Enabled = Me.CheckIfCanDisplayEnterpriseAwareBox
                        Case Is = VAQL.TokenType.MODELELEMENTNAME, VAQL.TokenType.PREBOUNDREADINGTEXT
                            Me.AutoComplete.Enabled = True
                            Call Me.PopulateEnterpriseAwareWithObjectTypes(Me.zsIntellisenseBuffer)
                        Case Is = VAQL.TokenType.PREDICATEPART,
                                  VAQL.TokenType.PREDICATESPACE
                            Me.AutoComplete.Enabled = True
                            'Call Me.AddFactTypePredicatePartsToEnterpriseAware()
                    End Select
                End If

                If e IsNot Nothing Then
                    'If e.KeyCode = Keys.Down Then
                    '    If Me.AutoComplete.ListBox.Items.Count > 0 Then
                    '        Me.AutoComplete.Owner = Me
                    '        Me.AutoComplete.ListBox.Focus()
                    '        Me.AutoComplete.ListBox.SelectedIndex = 0
                    '        e.Handled = True
                    '    End If
                    'End If

                    If e.Control Then
                        If e.KeyValue = Keys.J Then
                            'Call Me.AddFactTypeReadingsToEnterpriseAware()
                            Exit Sub
                        End If
                    End If
                End If

                If (Me.zrTextHighlighter.Tree.Errors.Count = 0 And Me.zrTextHighlighter.Tree.Optionals.Count = 0) Then
                    Me.AutoComplete.Hide()
                    Me.TextBoxInput.Focus()
                    Exit Sub
                ElseIf Me.AutoComplete.Enabled And Me.AutoComplete.ListBox.Items.Count > 0 Then

                    Dim myP As Point = Me.TextBox_Output.GetPositionFromCharIndex(Me.TextBox_Output.Text.Length) ' RichTextBox1.SelectionStart

                    'If myP.Y > 110 Then
                    '    Me.AutoComplete.TransparencyColour = Color.Red
                    'Else
                    '    Dim liLine As Integer
                    '    Dim liMaxRightPosition As Integer
                    '    If Me.TextBox_Output.Lines.Count > 1 Then
                    '        For liLine = 1 To Me.TextBox_Output.Lines.Count
                    '            myP = Me.TextBox_Output.GetPositionFromCharIndex(Me.TextBox_Output.GetFirstCharIndexFromLine(liLine) - 1)
                    '            If myP.X > liMaxRightPosition Then
                    '                liMaxRightPosition = myP.X
                    '            End If
                    '        Next
                    '        Dim loInputPoint As Point = Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.Text.Length)
                    '        If loInputPoint.X > liMaxRightPosition Then
                    '            Me.AutoComplete.TransparencyColour = Color.Red
                    '        Else
                    '            Me.AutoComplete.TransparencyColour = Color.White
                    '        End If
                    '    Else
                    '        Me.AutoComplete.TransparencyColour = Color.White
                    '    End If
                    'End If

                    Me.AutoComplete.Owner = Me

                    'Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
                    'lo_point.X += Me.TextBoxInput.Bounds.X
                    'lo_point.Y += Me.TextBoxInput.Bounds.Y
                    'lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 24
#Region "AutoComplete Position"
                    Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
                    lo_point.X += Me.TextBoxInput.Bounds.X
                    lo_point.Y += Me.TextBoxInput.Bounds.Y

                    ' Adjust the position to be relative to the TextBox's parent control
                    lo_point = Me.TextBoxInput.Parent.PointToScreen(lo_point)

                    ' Further adjust the Y position to move the modal form up
                    lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 8  ' Adjust this value to control the vertical position

                    ' Set the location of the AutoComplete form and display it
                    Me.AutoComplete.Location = lo_point
                    Me.AutoComplete.Show()
#End Region

                    'Me.AutoComplete.Location = PointToScreen(lo_point)
                    'Me.AutoComplete.Show()
                End If

                If e IsNot Nothing Then
                    If e.KeyCode <> Keys.Down Then
                        Me.TextBoxInput.Focus()
                    End If
                End If

            Else 'No errors in ParseTree (below)

                lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext

                If lsCurrentTokenType IsNot Nothing And (Me.TextBoxInput.Text.Length > 0) Then
                    lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type.ToString
                    Select Case Me.zrTextHighlighter.GetCurrentContext.Token.Type
                        Case Is = VAQL.TokenType.PREDICATEPART,
                                  VAQL.TokenType.PREDICATESPACE
                            Me.AutoComplete.Enabled = True
                            'Call Me.AddFactTypeReadingsToEnterpriseAware()
                        Case Is = VAQL.TokenType.MODELELEMENTNAME
                            Me.AutoComplete.Enabled = True
                        Case Else
                            Me.AutoComplete.Enabled = False
                    End Select
                End If

                If Me.AutoComplete.Enabled And Me.AutoComplete.ListBox.Items.Count > 0 Then

                    Me.AutoComplete.Owner = Me
                    'Me.AutoComplete.Show()

                    Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
                    lo_point.X += Me.TextBoxInput.Bounds.X
                    lo_point.Y += Me.TextBoxInput.Bounds.Y
                    lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 6
                    Me.AutoComplete.Location = PointToScreen(lo_point)
                End If

            End If

            If (Me.AutoComplete.Enabled = False) Or (Me.AutoComplete.ListBox.Items.Count = 0) Then
                Me.AutoComplete.Hide()
            ElseIf (Me.AutoComplete.Enabled = True) And (Me.AutoComplete.ListBox.Items.Count > 0) Then
                Me.AutoComplete.Owner = Me
                Me.AutoComplete.Show()
                Me.AutoComplete.zrCallingForm = Me
                Me.AutoComplete.ListBox.Focus()
                If e IsNot Nothing Then
                    e.Handled = True
                End If
            End If


            If e IsNot Nothing Then
                If e.KeyCode <> Keys.Down Then
                    Me.TextBoxInput.Focus()
                Else
                    e.Handled = True
                    If Me.AutoComplete.ListBox.Items.Count > 0 Then
                        Me.AutoComplete.Focus()
                        Me.AutoComplete.ListBox.SelectedIndex = 0
                    End If
                End If
            ElseIf Not Me.AutoComplete.Enabled Then
                Me.TextBoxInput.Focus()
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Warning, ex.StackTrace, abUseFlashCard:=True)
        End Try

    End Sub

    Private Sub ShowAutoCompleteTool()

        Try
            Me.AutoComplete.Owner = Me
            'Me.AutoComplete.Show()

            Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
            lo_point.X += Me.TextBoxInput.Bounds.X
            lo_point.Y += Me.TextBoxInput.Bounds.Y
            lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 13
            Me.AutoComplete.Location = PointToScreen(lo_point)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub zrTextHighlighter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles zrTextHighlighter.KeyDown

        Try
            If (e.KeyCode = Keys.Down Or Trim(Me.TextBoxInput.Text) <> "NL:") Then 'And Not e.KeyCode = Keys.Space Then
                'Call Me.ProcessAutoComplete(e)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click

        Try

            Me.TextBox_Output.Copy()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Async Sub ToolStripSplitButtonMike_ButtonClick(sender As Object, e As EventArgs) Handles ToolStripSplitButtonMike.ButtonClick

        Me.zbDictationMode = Not Me.zbDictationMode

        Select Case Me.zbDictationMode
            Case Is = True

                Me.ToolStripSplitButtonMike.Image = My.Resources.MenuImages.Mike_Green24x24

                'prApplication.Brain.AssemblyAIRelTimeTranscriber = New AssemblyAIRealtimeClient(Me.TextBoxInput)
                'Using prApplication.Brain.AssemblyAIRelTimeTranscriber
                '    Await prApplication.Brain.AssemblyAIRelTimeTranscriber.StartTranscriptionAsync() 'NB us poCancellationTokenSource in PublicVariablesOSM to cancel transcription 
                'End Using

                'Assembly AI
                prApplication.Brain.httpClient = New HttpClient()
                Using prApplication.Brain.httpClient
                    prApplication.Brain.httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", My.Settings.OSMAssemblyAIAPIKey)
                End Using


                Using Client As New AssemblyAIRealtimeClient(Me.TextBoxInput)
                    prApplication.Brain.AssemblyAIRelTimeTranscriber = Client
                    Await Client.StartTranscriptionAsync() 'NB us poCancellationTokenSource in PublicVariablesOSM to cancel transcription 
                End Using

            Case Is = False
                Me.ToolStripSplitButtonMike.Image = My.Resources.MenuImages.Mike_Red24x24

                poCancellationTokenSource.Cancel()
                prApplication.Brain.AssemblyAIRelTimeTranscriber.Dispose()

                If Me.TextBoxInput.Text.Trim.Length > 5 Then
                    ' Create a KeyEventArgs with the Enter key.
                    Dim enterKeyEventArgs As New System.Windows.Forms.KeyEventArgs(Keys.Enter)

                    ' Call the event handler directly.
                    TextBox_Input_KeyDown(TextBoxInput, enterKeyEventArgs)
                End If

        End Select

    End Sub

    Private moGreenImage As Object = My.Resources.MenuImages.Mike_Green24x24
    Private moRedImage As Object = My.Resources.MenuImages.Mike_Red24x24

    Private Sub TimerFormRefresh_Tick(sender As Object, e As EventArgs) Handles TimerFormRefresh.Tick

        If Me.zbDictationMode And Me.Brain.IsSpeaking Then
            Me.ToolStripSplitButtonMike.Image = moRedImage
        ElseIf Me.zbDictationMode And Not Me.Brain.IsSpeaking Then
            Me.ToolStripSplitButtonMike.Image = moGreenImage
        Else
            Me.ToolStripSplitButtonMike.Image = My.Resources.MenuImages.Mike24x24
        End If

        Me.ToolStripSplitButtonMike.Invalidate()
        Me.StatusStrip.Invalidate()
        Me.StatusStrip.Refresh()
        Me.Refresh()

    End Sub

    Private Sub TimerTaskManagement_Tick(sender As Object, e As EventArgs) Handles TimerTaskManagement.Tick

        Try
            If Me.Brain IsNot Nothing AndAlso Me.Brain.CurrentTask IsNot Nothing Then
                Me.ToolStripStatusLabelPromptCurrentTask.Visible = True
                Me.ToolStripStatusLabelPromptRunningTask.Visible = True
                Me.ToolStripStatusLabelPromptCurrentTask.Text = Me.Brain.CurrentTask.Name
            Else
                Me.ToolStripStatusLabelPromptCurrentTask.Visible = False
                Me.ToolStripStatusLabelPromptRunningTask.Visible = False
                Me.ToolStripStatusLabelPromptCurrentTask.Text = ""
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub frmToolboxBrainBox_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

        Try

            Dim lrChatBox = Me.TextBoxInput
            Dim lrChatHistory = Me.TextBox_Output

            Me.SplitContainer1.Panel1.Controls.Clear()
            Me.SplitContainer1.Panel2.Controls.Clear()

            If Me.Height > frmMain.Height / 2 Then
                Me.SplitContainer1.Panel1.Controls.Add(lrChatHistory)
                Me.SplitContainer1.Panel2.Controls.Add(lrChatBox)
                Me.SplitContainer1.SplitterDistance = Me.SplitContainer1.Height - Me.AutoComplete.Height + 30
            Else
                Me.SplitContainer1.Panel1.Controls.Add(lrChatBox)
                Me.SplitContainer1.Panel2.Controls.Add(lrChatHistory)
                Try
                    Me.SplitContainer1.SplitterDistance = 30
                Catch
                    'Fails when closing Boston.
                End Try
            End If

            Call Me.SetThoughtModeCursor()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub
End Class