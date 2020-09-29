Imports System.Reflection
Imports System.Threading

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
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub frmToolboxBrainBox_Leave(sender As Object, e As EventArgs) Handles Me.Leave

        'Call Me.AutoComplete.Hide()
        Me.TextBoxInput.Text = ""

    End Sub


    Private Sub frm_Brain_box_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '----------------------------------------------------------------
        'Rregister the Input and Output channels with the Richmond.Brain
        '----------------------------------------------------------------
        prApplication.Brain = New tBrain
        prApplication.Brain.Page = prApplication.WorkingPage
        prApplication.Brain.Model = prApplication.WorkingModel
        prApplication.Brain.VAQL = New VAQL.Processor(prApplication.WorkingModel)

        Dim thread As New Thread(AddressOf prApplication.Brain.VAQL.setDynamicObjects)
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

        '==============================================================
        'Make the Brain useful/user-friendly
        prApplication.Brain.send_data("Type in a Fact Type Reading, like 'Person has first-Name'")
        prApplication.Brain.send_data("...and I will help you create the ORM diagram.")

    End Sub

    Public Sub setup(ByRef arPage As FBM.Page)

        prApplication.Brain.Page = arPage

        If arPage IsNot Nothing Then
            prApplication.Brain.Model = arPage.Model
        End If

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub append_text(ByVal box As RichTextBox, ByVal color As Color, ByVal text As String)

        Dim start As Integer = box.TextLength
        box.AppendText(text)
        Dim li_end As Integer = box.TextLength

        ' Textbox may transform chars, so (end-start) != text.Length
        box.Select(start, li_end - start)

        box.SelectionColor = color
        ' could set box.SelectionBackColor, box.SelectionFont too.

        box.SelectionLength = 0 ' // clear
    End Sub

    Private Sub TextBox_Input_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxInput.Enter

        prApplication.Brain.Page = prApplication.WorkingPage
        prApplication.Brain.Model = prApplication.WorkingModel

        Call Me.SetThoughtModeCursor()

    End Sub

    Private Sub TextBox_Input_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxInput.GotFocus

        Select Case prApplication.Brain.ThoughtMode
            Case Is = pcenumBrainMode.ORMQL
                If Trim(Replace(Me.TextBoxInput.Text, "ORMQL:", "")).Length = 0 Then
                    Call Me.SetThoughtModeCursor()
                End If
            Case Is = pcenumBrainMode.NaturalLanguage
                If Trim(Replace(Me.TextBoxInput.Text, "NL:", "")).Length = 0 Then
                    Call Me.SetThoughtModeCursor()
                End If
        End Select

        If Me.TextBoxInput.Text.Last = " " Then
            Me.zsIntellisenseBuffer = ""
        End If

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub SetThoughtModeCursor()

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

        End Select

    End Sub

    Private Sub TextBox_Input_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBoxInput.KeyDown

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
                If Me.inputbuffer.Count > 0 Then
                    If Me.inputbuffer_pointer >= Me.inputbuffer.Count Then Me.inputbuffer_pointer = 0
                    Me.inputbuffer_pointer += 1
                    Me.TextBoxInput.Text = ""
                    Call Me.SetThoughtModeCursor()
                    Select Case Trim(Me.inputbuffer(Me.inputbuffer.Count - Me.inputbuffer_pointer).ToLower)
                        Case Is = "yes", "no"
                            If Me.inputbuffer_pointer + 1 <= Me.inputbuffer.Count Then
                                Me.inputbuffer_pointer += 1
                            End If
                    End Select
                    Me.TextBoxInput.AppendText(LTrim(Me.inputbuffer(Me.inputbuffer.Count - Me.inputbuffer_pointer)))
                Else
                    'Me.TextBoxInput.Clear()
                End If
                e.Handled = True
            Case Is = Keys.Down  'DownArrow
                '============================================
                'If Optionals exist, then show AutoComplete

                Me.zrTextHighlighter.Tree = Me.zrParser.Parse(Me.TextBoxInput.Text)
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
            Case Is = Keys.Enter 'Enter

                Me.AutoComplete.Hide()

                '--------------------------------------------
                'Firstly, strip away the ThoughtMode prompt
                '--------------------------------------------
                Select Case prApplication.Brain.ThoughtMode
                    Case Is = pcenumBrainMode.ORMQL
                        Me.TextBoxInput.Text = Replace(Me.TextBoxInput.Text, "ORMQL:", "")
                    Case Is = pcenumBrainMode.NaturalLanguage
                        Me.TextBoxInput.Text = Replace(Me.TextBoxInput.Text, "NL:", "")
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

                '----------------------------------
                'Send data to the Richmond.Brain
                '----------------------------------
                Me.inputbuffer.Add(Me.TextBoxInput.Text)
                If Me.inputbuffer.Count >= 10 Then
                    Me.inputbuffer.RemoveAt(0)
                End If

                If CheckIfModelPageSelected() Then
                    prApplication.Brain.receive_data(Trim(Me.TextBoxInput.Text))
                End If

                Me.TextBoxInput.Clear()
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
                zsIntellisenseBuffer &= LCase(e.KeyCode.ToString)
        End Select


    End Sub

    Private Function CheckIfModelPageSelected() As Boolean

        If IsNothing(prApplication.Brain.Model) Then
            '-----------------------------------------------
            'Try and set the Model from the EnterpriseTree
            '-----------------------------------------------
            If IsSomething(frmMain.zfrmModelExplorer) Then
                If IsSomething(frmMain.zfrmModelExplorer.TreeView.SelectedNode) Then
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

    End Function

    Private Sub TextBox_Output_GotFocus(sender As Object, e As EventArgs) Handles TextBox_Output.GotFocus

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub TextBox_Output_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBox_Output.MouseDown

        Me.TextBoxInput.Focus()

        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.TextBox_Output.ContextMenuStrip = Me.ContextMenuStripBrainBox
        End If

    End Sub

    Private Sub TextBox_Output_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_Output.TextChanged

        Me.TextBox_Output.ScrollToCaret()

    End Sub

    Private Sub TextBoxInput_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBoxInput.KeyUp

        If (e.KeyCode = Keys.Down Or Trim(Me.TextBoxInput.Text) <> "NL:") And Not e.KeyCode = Keys.Space Then
            Call Me.ProcessAutoComplete(e)
        End If

        If e.KeyCode = Keys.Escape Then
            Me.AutoComplete.Hide()
        End If

        e.Handled = True

    End Sub

    Private Sub TextBox_Input_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBoxInput.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.TextBoxInput.ContextMenuStrip = Me.ContextMenuVirtualAnalyst
        End If

    End Sub

    Private Sub DictationModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemDictationMode.Click

        Me.ToolStripMenuItemDictationMode.Checked = Not Me.ToolStripMenuItemDictationMode.Checked

        Me.zbDictationMode = Me.ToolStripMenuItemDictationMode.Checked

        If Me.zbDictationMode Then
            Me.StatusLabelMain.Text = "Dictation: On"
        Else
            Me.StatusLabelMain.Text = "Dictation: Off"
        End If

    End Sub

    Private Sub TimerInput_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerInput.Tick

        Dim lsText As String = ""

        Me.TimerInput.Stop()

        If Me.zbDictationMode Then
            If Me.TextBoxInput.Text <> "" Then

                Select Case prApplication.Brain.ThoughtMode
                    Case Is = pcenumBrainMode.ORMQL
                        lsText = Trim(Replace(Me.TextBoxInput.Text, "ORMQL:", ""))
                    Case Is = pcenumBrainMode.NaturalLanguage
                        lsText = Trim(Replace(Me.TextBoxInput.Text, "NL:", ""))
                End Select

                Me.zbSentence = New Language.Sentence(Trim(lsText))

                Call Language.AnalyseSentence(Me.zbSentence)
                Call Language.ProcessSentence(Me.zbSentence)
                If Me.zbSentence.AreAllWordsResolved Then
                    Call Language.ResolveSentence(Me.zbSentence)
                End If

                If Me.zbSentence.POStaggingResolved Then
                    '----------------------------------
                    'Send data to the Richmond.Brain
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

    End Sub

    Private Sub TextBox_Input_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxInput.TextChanged

        If Me.zbDictationMode Then
            Me.TimerInput.Start()
        End If

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub QuietModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemQuietMode.Click

        Me.ToolStripMenuItemQuietMode.Checked = Not Me.ToolStripMenuItemQuietMode.Checked

        prApplication.Brain.QuietMode = Me.ToolStripMenuItemQuietMode.Checked

    End Sub

    Private Sub GetMODELELEMENTParseNodes(ByRef arParseNode As VAQL.ParseNode, ByRef aarParseNode As List(Of VAQL.ParseNode))

        Dim lrParseNode As VAQL.ParseNode

        If arParseNode.Token.Type = VAQL.TokenType.MODELELEMENTNAME Then
            aarParseNode.Add(arParseNode)
        End If

        For Each lrParseNode In arParseNode.Nodes
            Call GetMODELELEMENTParseNodes(lrParseNode, aarParseNode)
        Next

    End Sub

    Public Sub CheckStartProductions(ByRef arParseTree As VAQL.ParseTree)

        'If (arParseTree.Depth(0) = 3) And (arParseTree.Count(1) <= 5) Then
        If Trim(Me.TextBoxInput.Text) = "NL:" Then
            'arParseTree.Optionals.Add(New VAQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDREADING"))
        End If

    End Sub

    Private Function CheckIfCanDisplayEnterpriseAwareBox()

        If Me.AutoComplete.ListBox.Items.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub AddEnterpriseAwareItem(ByVal asEAItem As String, Optional ByVal aoTagObject As Object = Nothing)

        Dim lrListItem As tComboboxItem

        lrListItem = New tComboboxItem(asEAItem, asEAItem, aoTagObject)

        If (asEAItem <> "") And Not (Me.AutoComplete.ListBox.FindStringExact(asEAItem) >= 0) Then
            Me.AutoComplete.ListBox.Items.Add(lrListItem)
        End If

    End Sub

    Private Sub AddPredicatePartsToEnterpriseAware(ByVal aarPredicatePart As List(Of FBM.PredicatePart))

        Dim lrPredicatePart As FBM.PredicatePart

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

    End Sub

    Private Sub PopulateEnterpriseAwareFromOptionals(ByVal aarParseErrors As VAQL.ParseErrors)

        Dim lrParseError As VAQL.ParseError
        Dim lsToken As String = ""
        Dim liTokenType As VAQL.TokenType

        For Each lrParseError In aarParseErrors
            liTokenType = DirectCast([Enum].Parse(GetType(VAQL.TokenType), lrParseError.ExpectedToken), VAQL.TokenType)
            Select Case liTokenType
                Case Is = VAQL.TokenType.BROPEN
                    Call Me.AddEnterpriseAwareItem("(", liTokenType)
                Case Is = VAQL.TokenType.PREDICATEPART
                    'Dim lrModelElement As FBM.ModelObject
                    Dim lsModelElementName As String
                    lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
                    ''lrModelElement = prApplication.WorkingModel.GetModelElementByName(lsModelElementName)
                    'If IsSomething(lrModelElement) Then
                    '    Call Me.AddPredicatePartsToEnterpriseAware(prBradfordApplication.Database.MetaDataManager.GetPredicatePartsForModelObject(lrModelElement))
                    'Else
                    '    Dim larCharBeginning() As Char = {"("}
                    '    Dim larCharEnd() As Char = {")"}
                    '    lsModelElementName = lsModelElementName.TrimStart(larCharBeginning).TrimEnd(larCharEnd)
                    '    'lrModelElement = prApplication .WorkingModel.GetModelElementByName(lsModelElementName)
                    '    If IsSomething(lrModelElement) Then
                    '        Call Me.AddPredicatePartsToEnterpriseAware(prBradfordApplication.Database.MetaDataManager.GetPredicatePartsForModelObject(lrModelElement))
                    '    End If
                    'End If
                Case Is = VAQL.TokenType.UNARYPREDICATEPART
                    'Do Nothing
                Case Is = VAQL.TokenType.MODELELEMENTNAME
                    '----------------------------------------------------
                    '20180311-Sometimes is not triggered when half way through writing a ModelElementName...esp when at beginning of a FactTypeReading.
                    Call Me.PopulateEnterpriseAwareWithObjectTypes(Me.zsIntellisenseBuffer)
                Case Is = VAQL.TokenType.PREBOUNDREADINGTEXT, _
                          VAQL.TokenType.POSTBOUNDREADINGTEXT, _
                          VAQL.TokenType.FOLLOWINGREADINGTEXT, _
                          VAQL.TokenType.FRONTREADINGTEXT
                    '------------
                    'Do nothing
                    '------------
                Case Else
                    If Me.zrScanner.Patterns(liTokenType).ToString.ToLower.StartsWith(zsIntellisenseBuffer) Then
                        Call Me.AddEnterpriseAwareItem(Me.zrScanner.Patterns(liTokenType).ToString, liTokenType)
                    End If
            End Select
        Next

        If Me.AutoComplete.ListBox.Items.Count > 0 Then
            Me.AutoComplete.Enabled = True
        End If

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

        Dim lbStartsWith As Boolean = False
        lbStartsWith = "asdf".StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture)

        For Each lrValueType In prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            If zsIntellisenseBuffer.Length > 0 Then
                If lrValueType.Name.ToLower.StartsWith(zsIntellisenseBuffer) Then
                    Call Me.AddEnterpriseAwareItem(lrValueType.Name, VAQL.TokenType.MODELELEMENTNAME)
                End If
            Else
                Call Me.AddEnterpriseAwareItem(lrValueType.Name, VAQL.TokenType.MODELELEMENTNAME)
            End If
        Next

        For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
            If zsIntellisenseBuffer.Length > 0 Then
                If lrEntityType.Name.ToLower.StartsWith(zsIntellisenseBuffer) Then
                    Call Me.AddEnterpriseAwareItem(lrEntityType.Name, VAQL.TokenType.MODELELEMENTNAME)
                End If
            Else
                'Call Me.AddEnterpriseAwareItem(lrEntityType.Name, VAQL.TokenType.MODELELEMENTNAME)
            End If
        Next

        For Each lrFactType In prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsMDAModelElement = False And x.IsObjectified = True)
            If zsIntellisenseBuffer.Length > 0 Then
                If lrFactType.Name.ToLower.StartsWith(zsIntellisenseBuffer) Then
                    Call Me.AddEnterpriseAwareItem(lrFactType.Name, VAQL.TokenType.MODELELEMENTNAME)
                End If
            Else
                'Call Me.AddEnterpriseAwareItem(lrFactType.Name, VAQL.TokenType.MODELELEMENTNAME)
            End If
        Next

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

            '-------------------
            'Get the ParseTree
            '-------------------
            Me.zrTextHighlighter.Tree = Me.zrParser.Parse(Me.TextBoxInput.Text)

            Call Me.CheckStartProductions(Me.zrTextHighlighter.Tree)

            Me.AutoComplete.Hide()
            Me.AutoComplete.ListBox.Items.Clear()

            If (Me.zrTextHighlighter.Tree.Errors.Count > 0) Or (Me.zrTextHighlighter.Tree.Optionals.Count > 0) Then
                If Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
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
                    Case Is = VAQL.TokenType.PREBOUNDREADINGTEXT, _
                              VAQL.TokenType.POSTBOUNDREADINGTEXT, _
                              VAQL.TokenType.FOLLOWINGREADINGTEXT, _
                              VAQL.TokenType.FRONTREADINGTEXT
                        'Don't add anything 
                    Case Is = VAQL.TokenType.REFERENCEMODE
                        'Don't add anything
                    Case Is = VAQL.TokenType.PREDICATEPART
                        'Don't add anything
                    Case Is = VAQL.TokenType.NUMBER
                        'Don't add anything
                    Case Is = VAQL.TokenType.EOF
                        'Don't add anything
                    Case Is = VAQL.TokenType.PREDICATESPACE
                        Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = VAQL.TokenType.SPACE
                        Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = VAQL.TokenType.MODELELEMENTNAME
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

                If IsSomething(lsCurrentTokenType) And (Me.TextBoxInput.Text.Length > 0) Then

                    lrParseNode = Me.zrTextHighlighter.GetCurrentContext
                    If lrParseNode.Token.Type = VAQL.TokenType.EOF Then
                        If lrParseNode.Parent.Nodes.Count > 1 Then
                            lsCurrentTokenType = Me.zrTextHighlighter.FindNode(lrParseNode.Parent.Nodes(lrParseNode.Parent.Nodes.Count - 1), 0).Token.Type
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
                        Case Is = VAQL.TokenType.PREDICATEPART, _
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


                If Me.AutoComplete.Enabled And Me.AutoComplete.ListBox.Items.Count > 0 Then

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
                    'Me.AutoComplete.Show()

                    Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
                    lo_point.X += Me.TextBoxInput.Bounds.X
                    lo_point.Y += Me.TextBoxInput.Bounds.Y
                    lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 24
                    Me.AutoComplete.Location = PointToScreen(lo_point)
                End If

                If e IsNot Nothing Then
                    If e.KeyCode <> Keys.Down Then
                        Me.TextBoxInput.Focus()
                    End If
                End If

            Else 'No errors in ParseTree (below)

                lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext

                If IsSomething(lsCurrentTokenType) And (Me.TextBoxInput.Text.Length > 0) Then
                    lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type.ToString
                    Select Case Me.zrTextHighlighter.GetCurrentContext.Token.Type
                        Case Is = VAQL.TokenType.PREDICATEPART, _
                                  VAQL.TokenType.PREDICATESPACE
                            Me.AutoComplete.Enabled = True
                            'Call Me.AddFactTypeReadingsToEnterpriseAware()
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
                Me.AutoComplete.zrCallingForm = Me
                Me.AutoComplete.Show()
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
                        Me.AutoComplete.ListBox.SelectedIndex = 0
                    End If
                End If
            Else
                Me.TextBoxInput.Focus()
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ShowAutoCompleteTool()

        Me.AutoComplete.Owner = Me
        'Me.AutoComplete.Show()

        Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
        lo_point.X += Me.TextBoxInput.Bounds.X
        lo_point.Y += Me.TextBoxInput.Bounds.Y
        lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 13
        Me.AutoComplete.Location = PointToScreen(lo_point)

    End Sub


    Private Sub zrTextHighlighter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles zrTextHighlighter.KeyDown

        If (e.KeyCode = Keys.Down Or Trim(Me.TextBoxInput.Text) <> "NL:") Then 'And Not e.KeyCode = Keys.Space Then
            'Call Me.ProcessAutoComplete(e)
        End If

    End Sub

    Private Sub frmToolboxBrainBox_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus

        'Call Me.AutoComplete.Hide()

    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click

        Me.TextBox_Output.Copy()

    End Sub

End Class