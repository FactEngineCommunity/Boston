Imports System.Reflection

Public Class frmFactEngine

    Public zrScanner As New FEQL.Scanner
    Public zrParser As New FEQL.Parser(Me.zrScanner)
    Public WithEvents zrTextHighlighter As FEQL.TextHighlighter
    Public WithEvents Application As tRichmondApplication = prApplication
    Private AutoComplete As frmAutoComplete
    Public zsIntellisenseBuffer As String = ""

    Public msPreviousProductionLookedFor As String

    Public FEQLProcessor As New FEQL.Processor(prApplication.WorkingModel)
    Private TextMarker As FEQL.Controls.TextMarker

    Private Sub AddEnterpriseAwareItem(ByVal asEAItem As String, Optional ByVal aoTagObject As Object = Nothing)

        Dim lrListItem = New tComboboxItem(asEAItem, asEAItem, aoTagObject)

        If (asEAItem <> "") And Not (Me.AutoComplete.ListBox.FindStringExact(asEAItem) >= 0) Then
            Me.AutoComplete.ListBox.Items.Add(lrListItem)
        End If

    End Sub

    Public Sub CheckStartProductions(ByRef arParseTree As FEQL.ParseTree)

        'If (arParseTree.Depth(0) = 3) And (arParseTree.Count(1) <= 5) Then
        If Trim(Me.TextBoxInput.Text) = "" Then
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDASSERT"))
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDCREATE"))
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDDELETE"))
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDEACH"))
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDEITHER"))
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDENUMERATE"))
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDNULL"))
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDMATCH"))
            arParseTree.Optionals.Add(New FEQL.ParseError("Start Production", &H1001, 0, 0, 0, 0, "KEYWDWHICH"))
        End If

    End Sub

    Private Function AddFactTypePredicatePartsToEnterpriseAware() As Boolean

        Dim lasPredicatePart As New List(Of String)
        Dim lsPredicateReadingText As String = ""

        'If prApplication.WorkingModel.FactTypeReading.Count > 0 Then

        Me.AutoComplete.ListBox.Sorted = True

        'For Each lrFactTypeReading In prApplication.WorkingModel.FactTypeReading

        '    lsPredicateReadingText = lrFactTypeReading.GetPredicateText

        '    If Trim(Me.TextBoxInput.Text).LastIndexOf(lsPredicateReadingText, Trim(Me.TextBoxInput.Text).Length - 1, Viev.Lesser(Trim(Me.TextBoxInput.Text).Length - 1, lsPredicateReadingText.Length)) > 0 Then
        '        lasPredicatePart.Clear()
        '        Exit For
        '    Else
        '        lasPredicatePart.Add(lsPredicateReadingText)
        '    End If
        'Next

        Dim lrModelElement As FBM.ModelObject
        Dim lsModelElementName As String
        lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
        lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
        If lrModelElement Is Nothing Then
            Dim liInd = Me.TextBoxInput.Text.Trim.Split(" ").Length - 2
            If liInd >= 0 Then
                lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ")(liInd)
                lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
            End If
        End If
        If IsSomething(lrModelElement) Then
            Select Case lrModelElement.GetType
                Case Is = GetType(FBM.FactType)
                    Call Me.AddPredicatePartsToEnterpriseAware(CType(lrModelElement, FBM.FactType).getPredicatePartsForModelObject(lrModelElement))
                Case Is = GetType(FBM.EntityType)
                    For Each lrFactType In lrModelElement.getConnectedFactTypes
                        Call Me.AddPredicatePartsToEnterpriseAware(lrFactType.getPredicatePartsForModelObject(lrModelElement))
                    Next
            End Select
        Else
            Dim larCharBeginning() As Char = {"("}
            Dim larCharEnd() As Char = {")"}
            lsModelElementName = lsModelElementName.TrimStart(larCharBeginning).TrimEnd(larCharEnd)
            lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
            If IsSomething(lrModelElement) Then
                If lrModelElement.GetType = GetType(FBM.FactType) Then
                    Call Me.AddPredicatePartsToEnterpriseAware(CType(lrModelElement, FBM.FactType).getPredicatePartsForModelObject(lrModelElement))
                End If
            End If
        End If


        'For Each lsPredicateReadingText In lasPredicatePart
        '        Call Me.AddEnterpriseAwareItem(lsPredicateReadingText)
        '    Next

        '    If Me.AutoComplete.ListBox.Items.Count > 0 Then
        '        Me.AutoComplete.ListBox.SelectedIndex = 0
        '    End If
        'End If

    End Function

    Private Sub AddFactTypeReadingsToEnterpriseAware()

        Dim lrFactTypeReading As FBM.FactTypeReading

        If prApplication.WorkingModel.FactTypeReading.Count > 0 Then
            Me.AutoComplete.ListBox.Items.Clear()
            Me.AutoComplete.ListBox.Sorted = True

            For Each lrFactTypeReading In prApplication.WorkingModel.FactTypeReading

                ''=========================================================================================
                ''LinFu code
                'Dim loDynamicPageObject As New LinFu.Reflection.DynamicObject

                'loDynamicPageObject.MixWith(lrFactTypeReading)
                'loDynamicPageObject.MixWith(New LinFuORM.FactTypeReading)

                'Dim lrLinFuFactTypeReading As LinFuORM.FactTypeReading = loDynamicPageObject.CreateDuck(Of LinFuORM.FactTypeReading)()
                ''=========================================================================================

                Call Me.AddEnterpriseAwareItem(lrFactTypeReading.GetPredicateText)
            Next


            Me.AutoComplete.Show()
            Me.AutoComplete.ListBox.Focus()
            'Me.AutoComplete.ListBox.SelectedIndex = 0
        End If

    End Sub

    Private Sub AddPredicatePartsToEnterpriseAware(ByVal aarPredicatePart As List(Of FBM.PredicatePart))

        Try
            Me.AutoComplete.ListBox.Sorted = True

            Dim lasPredicatePartText As New List(Of String)

            For Each lrPredicatePart In aarPredicatePart
                lasPredicatePartText.AddUnique(lrPredicatePart.PredicatePartText)
            Next

            Dim lbBufferIgnored = False
            For Each lsPredicatePartText In lasPredicatePartText
                If zsIntellisenseBuffer.Length > 0 Then
                    If lsPredicatePartText.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                        Call Me.AddEnterpriseAwareItem(lsPredicatePartText, FEQL.TokenType.PREDICATE)
                    End If
                Else
                    Call Me.AddEnterpriseAwareItem(lsPredicatePartText, FEQL.TokenType.PREDICATE)
                    lbBufferIgnored = True
                End If

            Next

            If Not lbBufferIgnored Then
                For Each lsPredicatePartText In lasPredicatePartText
                    If zsIntellisenseBuffer.Length > 0 Then
                        If Not lsPredicatePartText.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                            Call Me.AddEnterpriseAwareItem(lsPredicatePartText, FEQL.TokenType.PREDICATE)
                        End If
                    End If
                Next
            End If

            Me.AutoComplete.Show()
            Me.AutoComplete.ListBox.Focus()
            If (aarPredicatePart.Count > 0) And (Me.AutoComplete.ListBox.Items.Count > 0) Then
                'Me.AutoComplete.ListBox.SelectedIndex = 0
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub DesbribeModelElement(ByRef arModelElement As FBM.ModelObject)

        Me.LabelError.Text = ""

        If arModelElement Is Nothing Then
            Me.LabelError.ForeColor = Color.Orange
            Me.LabelError.Text = "No Model Element found."
        Else
            Me.LabelError.ForeColor = Color.Black
            Me.LabelError.Text = ""

            Dim lrTable = arModelElement.getCorrespondingRDSTable

            Me.LabelError.Text &= lrTable.Name
            Me.LabelError.Text &= vbCrLf & vbCrLf
            For Each lrCOlumn In lrTable.Column
                Dim lsString = String.Format("{0,6}{1,-" & 20 - lrCOlumn.Name.Length & "}{2}", lrCOlumn.Name, " ", lrCOlumn.getMetamodelDataType.ToString)
                Me.LabelError.Text &= lsString & vbCrLf
            Next

            Me.LabelError.Text &= vbCrLf & vbCrLf
            Me.LabelError.Text &= "Relations" & vbCrLf & vbCrLf

            For Each lrRelation In lrTable.getOutgoingRelations

                Dim lrFactType = lrRelation.ResponsibleFactType

                Dim lrFactTypeReading = lrFactType.getOutgoingFactTypeReadingForTabe(lrTable)
                Me.LabelError.Text &= lrFactTypeReading.GetReadingTextCQL & vbCrLf

            Next

        End If

    End Sub


    Private Sub displayModelName()
        If prApplication.WorkingModel Is Nothing Then
            Me.ToolStripStatusLabelWorkingModelName.ForeColor = Color.Orange
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: Select a Model in the Model Explorer"
        Else
            Me.ToolStripStatusLabelWorkingModelName.ForeColor = Color.Black
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: " & prApplication.WorkingModel.Name
            If Trim(prApplication.WorkingModel.TargetDatabaseConnectionString) = "" Then
                Me.ToolStripStatusLabelRequiresConnectionString.ForeColor = Color.Orange
                Me.ToolStripStatusLabelRequiresConnectionString.Text = "Model requires a database connection string"
            Else
                Me.ToolStripStatusLabelRequiresConnectionString.Text = ""
            End If
        End If


    End Sub

    Private Sub GetMODELELEMENTParseNodes(ByRef arParseNode As FEQL.ParseNode, ByRef aarParseNode As List(Of FEQL.ParseNode))

        Dim lrParseNode As FEQL.ParseNode

        If arParseNode.Token.Type = FEQL.TokenType.MODELELEMENTNAME Then
            aarParseNode.Add(arParseNode)
        End If

        For Each lrParseNode In arParseNode.Nodes
            Call GetMODELELEMENTParseNodes(lrParseNode, aarParseNode)
        Next

    End Sub

    Private Sub GetPredicateNodes(ByRef arParseNode As FEQL.ParseNode, ByRef aarParseNode As List(Of FEQL.ParseNode))

        Dim lrParseNode As FEQL.ParseNode

        If arParseNode.Token.Type = FEQL.TokenType.PREDICATE Then
            aarParseNode.Add(arParseNode)
        End If

        For Each lrParseNode In arParseNode.Nodes
            Call GetPredicateNodes(lrParseNode, aarParseNode)
        Next

    End Sub

    Private Sub GetPredicateClauseNodes(ByRef arParseNode As FEQL.ParseNode, ByRef aarParseNode As List(Of FEQL.ParseNode))

        Dim lrParseNode As FEQL.ParseNode

        If arParseNode.Token.Type = FEQL.TokenType.PREDICATECLAUSE Then
            aarParseNode.Add(arParseNode)
        End If

        For Each lrParseNode In arParseNode.Nodes
            Call GetPredicateClauseNodes(lrParseNode, aarParseNode)
        Next

    End Sub

    Private Sub frmFactEngine_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.AutoComplete = New frmAutoComplete(Me.TextBoxInput)

        '-------------------------------------------------------
        'Setup the Text Highlighter
        '----------------------------
        Me.zrTextHighlighter = New FEQL.TextHighlighter(
                               Me.TextBoxInput,
                               Me.zrScanner,
                               Me.zrParser)

        Me.TextMarker = New FEQL.Controls.TextMarker(Me.TextBoxInput)

        Call Me.displayModelName()
        Me.FEQLProcessor = New FEQL.Processor(prApplication.WorkingModel)

        If prApplication.WorkingModel IsNot Nothing Then
            If Trim(prApplication.WorkingModel.TargetDatabaseConnectionString) = "" Then
                Me.ToolStripStatusLabelRequiresConnectionString.ForeColor = Color.Orange
                Me.ToolStripStatusLabelRequiresConnectionString.Text = "Model requires a database connection string"
            Else
                Me.ToolStripStatusLabelRequiresConnectionString.Text = ""
            End If
        End If

        Me.ToolStripStatusLabelCurrentProduction.Text = "FactEngine Statement"

    End Sub

    Private Sub ToolStripButtonGO_Click(sender As Object, e As EventArgs) Handles ToolStripButtonGO.Click

        Call Me.GO()

    End Sub

    Private Sub GO()
        Dim lrRecordset As New ORMQL.Recordset

        If prApplication.WorkingModel.TargetDatabaseConnectionString = "" Then
            Me.LabelError.ForeColor = Color.Orange
            Me.LabelError.Text = "The Model needs a database connection string."
            Exit Sub
        End If

        Me.LabelError.Text = ""
        Dim lsQuery = Me.TextBoxInput.Text.Replace(vbLf, " ")
        If Me.TextBoxInput.SelectionLength > 0 Then
            lsQuery = Me.TextBoxInput.SelectedText
        End If

        lrRecordset = Me.FEQLProcessor.ProcessFEQLStatement(lsQuery)

        If lrRecordset.Query IsNot Nothing Then
            Me.TextBoxQuery.Text = lrRecordset.Query
        End If

        If lrRecordset.ErrorString IsNot Nothing Then
            Me.LabelError.BringToFront()
            Me.LabelError.Text = lrRecordset.ErrorString
        Else
            Select Case lrRecordset.StatementType
                Case Is = FactEngine.pcenumFEQLStatementType.DESCRIBEStatement
                    Call Me.DesbribeModelElement(lrRecordset.ModelElement)
                Case Else

                    Me.LabelError.Text = ""

                    If lrRecordset.Facts.Count = 0 Then
                        Me.LabelError.ForeColor = Color.Orange
                        Me.LabelError.Text = "No results returned"
                    Else
                        Me.LabelError.ForeColor = Color.Black
                        Me.LabelError.Text = ""

                        Dim liInd = 0
                        For Each lsColumnName In lrRecordset.Columns
                            liInd += 1
                            Me.LabelError.Text &= " " & lsColumnName & " "
                            If liInd < lrRecordset.Columns.Count Then Me.LabelError.Text &= ","
                        Next
                        Me.LabelError.Text &= vbCrLf & "=======================================" & vbCrLf
                        For Each lrFact In lrRecordset.Facts

                            Me.LabelError.Text &= lrFact.EnumerateAsBracketedFact(True) & vbCrLf
                        Next
                    End If
            End Select
        End If

        Me.TabPageResults.Show()

    End Sub

    Private Sub Application_WorkingModelChanged() Handles Application.WorkingModelChanged
        Call Me.displayModelName()
        Me.FEQLProcessor = New FEQL.Processor(prApplication.WorkingModel)
    End Sub

    Private Sub TextBoxInput_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxInput.KeyDown

        Try
            If Me.TextBoxInput.SelectionColor = Color.Black Then Me.TextBoxInput.SelectionColor = Color.Wheat

            'Handle Paste
            If (e.KeyCode = Keys.ControlKey) Or (e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V) Then
                Exit Sub
            End If

            '===============================================================================
            'Intellisense Buffer. Populate first for AutoComplete below that...
            Select Case e.KeyCode
                Case Is = Keys.Control & Keys.V
                    Debugger.Break()
                Case Is = Keys.Back
                    If zsIntellisenseBuffer.Length > 0 Then
                        zsIntellisenseBuffer = zsIntellisenseBuffer.Substring(0, zsIntellisenseBuffer.Length - 1)
                    End If
                Case Is = Keys.Space, Keys.Escape, Keys.Down, Keys.Up, Keys.Shift, Keys.ShiftKey
                    Me.zsIntellisenseBuffer = ""
                Case Else
                    zsIntellisenseBuffer &= LCase(e.KeyCode.ToString)
            End Select

            Select Case e.KeyCode
                Case Is = Keys.Escape
                    Me.AutoComplete.Hide()
                    Exit Sub
            End Select

            If Not e.KeyCode = Keys.Down Then
                Call Me.ProcessAutoComplete(e)
            End If


            Select Case e.KeyCode
                Case Is = Keys.F5
                    Me.AutoComplete.Hide()
                    Call Me.GO()
                Case Is = Keys.Down
                    If (Me.AutoComplete.ListBox.Items.Count > 0) Or Me.AutoComplete.Visible Then
                        Me.AutoComplete.ListBox.Focus()
                        If Me.AutoComplete.ListBox.Items.Count > 0 Then
                            Me.AutoComplete.ListBox.SelectedIndex = 0
                        End If
                        e.Handled = True
                        Exit Sub
                    End If
                Case Is = Keys.Space
                Case Is = Keys.Back
                Case Is = Keys.Left
                Case Else
                    'e.Handled = True
            End Select

            Me.TextBoxInput.Focus()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub TextBoxInput_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBoxInput.KeyUp

        Select Case e.KeyCode
            Case Is = Keys.Space,
                      Keys.Enter,
                      Keys.Back,
                      Keys.Up
                Me.TextBoxInput.SelectionColor = Color.Wheat
        End Select

        If Me.TextBoxInput.SelectionColor = Color.Black Then Me.TextBoxInput.SelectionColor = Color.Wheat

        If Me.zrTextHighlighter.Tree.Nodes.Count > 0 Then
            If Me.zrTextHighlighter.Tree.Nodes(0).Nodes.Count > 0 Then
                If Me.zrTextHighlighter.Tree.Nodes(0).Nodes(0).Nodes.Count > 0 Then
                    Dim liInd = Me.zrTextHighlighter.Tree.Nodes(0).Nodes(0).Nodes.Count
                    If liInd > 0 Then
                        Dim lrLastToken = Me.zrTextHighlighter.Tree.Nodes(0).Nodes(0).Nodes(liInd - 1).Token
                        If lrLastToken.Type = FEQL.TokenType.EOF Then
                            Me.ToolStripStatusLabelGOPrompt.Text = "Valid FEQL Statement"
                            Me.ToolStripStatusLabelGOPrompt.Visible = True
                        Else
                            Me.ToolStripStatusLabelGOPrompt.Visible = False
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub ProcessAutoComplete(ByVal e As System.Windows.Forms.KeyEventArgs)

        Dim lsExpectedToken As String = ""
        Dim liTokenType As FEQL.TokenType
        Dim lsCurrentTokenType As Object

        '-------------------
        'Get the ParseTree
        '-------------------
        Me.zrTextHighlighter.Tree = Me.zrParser.Parse(Trim(Me.TextBoxInput.Text))

        '=================================================================
        'Check valid ModelElement Names
        Dim lrParseNode As FEQL.ParseNode
        Dim larModelElementNameParseNode As New List(Of FEQL.ParseNode)
        Dim larModelPredicateClauseParseNode As New List(Of FEQL.ParseNode)
        Dim larParseNode As New List(Of FEQL.ParseNode)
        Dim lrModelElement As FBM.ModelObject

        If Me.TextBoxInput.Text.Length > 10 Then 'was 21
            Me.TextMarker.Clear()

            Call Me.GetMODELELEMENTParseNodes(Me.zrTextHighlighter.Tree.Nodes(0), larModelElementNameParseNode)

            For Each lrParseNode In larModelElementNameParseNode
                lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrParseNode.Token.Text)
                If lrModelElement Is Nothing Then
                    Me.TextMarker.AddWord(lrParseNode.Token.StartPos, lrParseNode.Token.Length, Color.Red, "Uknown Model Element")
                End If
            Next

            larParseNode = New List(Of FEQL.ParseNode)
            Call Me.GetPredicateClauseNodes(Me.zrTextHighlighter.Tree.Nodes(0), larModelPredicateClauseParseNode)
            For Each lrParseNode In larModelPredicateClauseParseNode
                Dim larPredicateNode As New List(Of FEQL.ParseNode)
                Call Me.GetPredicateNodes(lrParseNode, larPredicateNode)
                Dim lsPredicatePartText As String = ""
                For Each lrPredicateNode In larPredicateNode
                    lsPredicatePartText &= Trim(lrPredicateNode.Token.Text) & " "
                Next
                If Not prApplication.WorkingModel.existsPredicatePart(Trim(lsPredicatePartText)) Then
                    Me.TextMarker.AddWord(lrParseNode.Token.StartPos, lrParseNode.Token.Length, Color.Red, "Uknown Predicate")
                End If
            Next

            Me.TextMarker.MarkWords()
        End If
        '=======================================

        Call Me.CheckStartProductions(Me.zrTextHighlighter.Tree)

        'Me.AutoComplete.Hide()
        Me.AutoComplete.ListBox.Items.Clear()

        '============================================================================================
        'Do ultrasmart checking. Finds the last ModelElementName and the last PredicateClause
        '  and if a FactTypeReading is being attempted to be made, finds the next ModelElementName
        If larModelElementNameParseNode.Count > 0 And larModelPredicateClauseParseNode.Count > 0 Then

            Dim lrLastModelElementNameParseNode = larModelElementNameParseNode(larModelElementNameParseNode.Count - 1)
            Dim lrLastPredicateClauseParseNode = larModelPredicateClauseParseNode(larModelPredicateClauseParseNode.Count - 1)

            lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrLastModelElementNameParseNode.Token.Text)

            If lrModelElement Is Nothing Then
                'Nothing to do here
            Else
                Select Case Me.FEQLProcessor.getWhichStatementType(Trim(Me.TextBoxInput.Text), True)
                    Case Is = FactEngine.pcenumFEQLStatementType.WHICHSELECTStatement

                        Dim lrWHICHSELECTStatement As New FEQL.WHICHSELECTStatement
                        Call Me.FEQLProcessor.GetParseTreeTokensReflection(lrWHICHSELECTStatement, Me.zrTextHighlighter.Tree.Nodes(0))

                        Dim loLastWhichClauseObject = lrWHICHSELECTStatement.WHICHCLAUSE(lrWHICHSELECTStatement.WHICHCLAUSE.Count - 1)
                        Dim lrLastWhichClause As New FEQL.WHICHCLAUSE
                        Call Me.FEQLProcessor.GetParseTreeTokensReflection(lrLastWhichClause, loLastWhichClauseObject)

                        Dim lsPredicateText As String = ""
                        For Each lsPredicatePart In lrLastWhichClause.PREDICATE
                            lsPredicateText = Trim(lsPredicateText & " " & lsPredicatePart)
                        Next

                        Dim larFactTypeReading = From FactType In prApplication.WorkingModel.FactType
                                                 From FactTypeReading In FactType.FactTypeReading
                                                 From PredicatePart In FactTypeReading.PredicatePart
                                                 Where PredicatePart.Role.JoinedORMObject.Id = lrModelElement.Id
                                                 Where PredicatePart.PredicatePartText = lsPredicateText
                                                 Select FactTypeReading

                        If larFactTypeReading.Count = 0 Then
                            'nothing to do here
                        Else
                            Me.AutoComplete.ListBox.Items.Clear()
                            Dim lrFactTypeReading = larFactTypeReading.First
                            Call Me.AddEnterpriseAwareItem(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id, FEQL.TokenType.MODELELEMENTNAME)
                            If Me.AutoComplete.Visible = False Then
                                Me.AutoComplete.Show()
                            End If
                        End If

                End Select

            End If
        End If


        '============================================================================================

        If (Me.zrTextHighlighter.Tree.Errors.Count > 0) Or (Me.zrTextHighlighter.Tree.Optionals.Count > 0) Then
            If Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
                lsExpectedToken = Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken
            Else
                lsExpectedToken = Me.zrTextHighlighter.Tree.Optionals(0).ExpectedToken
            End If
            If lsExpectedToken <> "" Then
                liTokenType = DirectCast([Enum].Parse(GetType(FEQL.TokenType), lsExpectedToken), FEQL.TokenType)
                'MsgBox("Expecting: " & Me.zrScanner.Patterns(liTokenType).ToString)
            End If

            If Me.zrTextHighlighter.Tree.Optionals.Count > 0 Then
                Call Me.PopulateEnterpriseAwareFromOptionals(Me.zrTextHighlighter.Tree.Optionals)
            End If

            Select Case liTokenType
                Case Is = FEQL.TokenType.BROPEN
                    Me.AutoComplete.Enabled = True
                    Call Me.AddEnterpriseAwareItem("(", liTokenType)
                Case Is = FEQL.TokenType.BRCLOSE
                    Me.AutoComplete.Enabled = True
                    Call Me.AddEnterpriseAwareItem(")", liTokenType)
                Case Is = FEQL.TokenType._NONE_
                    Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                Case Is = FEQL.TokenType.KEYWDNULL
                    Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                Case Is = FEQL.TokenType.FACTTYPEPRODUCTION
                    Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                Case Is = FEQL.TokenType.PREDICATE
                    Dim lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
                    If lrModelElement IsNot Nothing Then
                        Select Case lrModelElement.GetType
                            Case Is = GetType(FBM.FactType)
                                Call Me.AddPredicatePartsToEnterpriseAware(CType(lrModelElement, FBM.FactType).getPredicatePartsForModelObject(lrModelElement, True))
                            Case Is = GetType(FBM.EntityType)
                                For Each lrFactType In lrModelElement.getConnectedFactTypes
                                    Call Me.AddPredicatePartsToEnterpriseAware(lrFactType.getPredicatePartsForModelObject(lrModelElement))
                                Next
                        End Select
                    End If
                Case Is = FEQL.TokenType.PREDICATESPACE
                    Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                Case Is = FEQL.TokenType.SPACE
                    Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                Case Is = FEQL.TokenType.MODELELEMENTNAME
                    Me.AutoComplete.Enabled = True
                    If Not e.KeyCode = Keys.ShiftKey Then
                        Call Me.PopulateEnterpriseAwareWithObjectTypes()
                    End If
                    '20200729-VM-Might be able to remove the below because no optionals or errors.
                    Dim lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
                    If lrModelElement IsNot Nothing Then
                        Select Case lrModelElement.GetType
                            Case Is = GetType(FBM.FactType)
                                Call Me.AddPredicatePartsToEnterpriseAware(CType(lrModelElement, FBM.FactType).getPredicatePartsForModelObject(lrModelElement, True))
                            Case Is = GetType(FBM.EntityType)
                                For Each lrFactType In lrModelElement.getConnectedFactTypes
                                    Call Me.AddPredicatePartsToEnterpriseAware(lrFactType.getPredicatePartsForModelObject(lrModelElement))
                                Next
                        End Select
                    End If
                Case Is = FEQL.TokenType.VALUE
                Case Is = FEQL.TokenType.IDENTIFIER
                Case Else
                    Me.AutoComplete.Enabled = True
                    Me.AddEnterpriseAwareItem(Me.zrScanner.Patterns(liTokenType).ToString, liTokenType)
            End Select

            lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext
            If IsSomething(lsCurrentTokenType) And (Me.TextBoxInput.Text.Length > 0) Then
                lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type.ToString
                Me.msPreviousProductionLookedFor = Me.ToolStripStatusLabelCurrentProduction.Text
                If lsCurrentTokenType IsNot Nothing Then
                    Me.ToolStripStatusLabelCurrentProduction.Text = lsCurrentTokenType
                End If
                Select Case Me.zrTextHighlighter.GetCurrentContext.Token.Type
                    Case Is = FEQL.TokenType.KEYWDNULL
                        Me.AutoComplete.Enabled = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = FEQL.TokenType.FACTTYPEPRODUCTION
                        Me.AutoComplete.Enabled = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = FEQL.TokenType.MODELELEMENTNAME
                        Me.AutoComplete.Enabled = True
                        Call Me.PopulateEnterpriseAwareWithObjectTypes()
                    Case Is = FEQL.TokenType.PREDICATE,
                              FEQL.TokenType.PREDICATESPACE
                        Me.AutoComplete.Enabled = True
                        Call Me.AddFactTypePredicatePartsToEnterpriseAware()
                End Select
            End If

            If e.KeyCode = Keys.Down Then
                If Me.AutoComplete.ListBox.Items.Count > 0 Then
                    Me.AutoComplete.ListBox.Focus()
                    'Me.AutoComplete.ListBox.SelectedIndex = 0
                    e.Handled = True
                End If
                e.Handled = True
            End If

            If e.Control Then
                If e.KeyValue = Keys.J Then
                    Call Me.AddFactTypeReadingsToEnterpriseAware()
                    Exit Sub
                End If
            End If

            If Me.AutoComplete.Enabled And Me.AutoComplete.ListBox.Items.Count > 0 Then

                Me.AutoComplete.Owner = Me
                Me.AutoComplete.Show()

                Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
                lo_point.X += Me.TextBoxInput.Bounds.X
                lo_point.Y += Me.TextBoxInput.Bounds.Y
                lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 6
                Me.AutoComplete.Location = PointToScreen(lo_point)
            ElseIf Me.AutoComplete.ListBox.Items.Count = 0 Then
                Me.AutoComplete.Hide()
            End If

            If e.KeyCode <> Keys.Down Then
                Me.TextBoxInput.Focus()
            End If
        Else
            lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext
            If IsSomething(lsCurrentTokenType) And (Me.TextBoxInput.Text.Length > 0) Then
                lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type.ToString
                Me.ToolStripStatusLabelCurrentProduction.Text = lsCurrentTokenType
                Select Case Me.zrTextHighlighter.GetCurrentContext.Token.Type
                    Case Is = FEQL.TokenType.PREDICATE,
                              FEQL.TokenType.PREDICATESPACE
                        Me.AutoComplete.Enabled = True
                        Call Me.AddFactTypeReadingsToEnterpriseAware()
                    Case Is = FEQL.TokenType.EOF
                        Dim lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
                        lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
                        If lrModelElement IsNot Nothing Then
                            Select Case lrModelElement.GetType
                                Case Is = GetType(FBM.FactType)
                                    Call Me.AddPredicatePartsToEnterpriseAware(CType(lrModelElement, FBM.FactType).getPredicatePartsForModelObject(lrModelElement, True))
                                Case Is = GetType(FBM.EntityType)
                                    For Each lrFactType In lrModelElement.getConnectedFactTypes
                                        Call Me.AddPredicatePartsToEnterpriseAware(lrFactType.getPredicatePartsForModelObject(lrModelElement))
                                    Next
                            End Select
                        End If
                    Case Is = FEQL.TokenType.MODELELEMENTNAME
                        Me.AutoComplete.Enabled = True
                        Call Me.PopulateEnterpriseAwareWithObjectTypes()
                        Dim lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
                        lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
                        If lrModelElement IsNot Nothing Then
                            Select Case lrModelElement.GetType
                                Case Is = GetType(FBM.FactType)
                                    Call Me.AddPredicatePartsToEnterpriseAware(CType(lrModelElement, FBM.FactType).getPredicatePartsForModelObject(lrModelElement, True))
                                Case Is = GetType(FBM.EntityType)
                                    For Each lrFactType In lrModelElement.getConnectedFactTypes
                                        Call Me.AddPredicatePartsToEnterpriseAware(lrFactType.getPredicatePartsForModelObject(lrModelElement))
                                    Next
                            End Select
                        End If
                    Case Else
                        Me.AutoComplete.Enabled = False
                End Select
            End If

            If Me.AutoComplete.Enabled And Me.AutoComplete.ListBox.Items.Count > 0 Then

                Me.AutoComplete.Owner = Me
                Me.AutoComplete.Show()

                Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
                lo_point.X += Me.TextBoxInput.Bounds.X
                lo_point.Y += Me.TextBoxInput.Bounds.Y
                lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 8
                Me.AutoComplete.Location = PointToScreen(lo_point)
            ElseIf Me.AutoComplete.ListBox.Items.Count = 0 Then
                Me.AutoComplete.Hide()
            End If

            If e.KeyCode <> Keys.Down Then
                Me.TextBoxInput.Focus()
            End If

        End If

    End Sub

    Private Function CheckIfCanDisplayEnterpriseAwareBox()

        If Me.AutoComplete.ListBox.Items.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub PopulateEnterpriseAwareFromOptionals(ByVal aarParseErrors As FEQL.ParseErrors)

        Dim lrParseError As FEQL.ParseError
        Dim lsToken As String = ""
        Dim liTokenType As FEQL.TokenType

        For Each lrParseError In aarParseErrors
            liTokenType = DirectCast([Enum].Parse(GetType(FEQL.TokenType), lrParseError.ExpectedToken), FEQL.TokenType)
            Select Case liTokenType
                Case Is = FEQL.TokenType.BROPEN
                    Call Me.AddEnterpriseAwareItem("(", liTokenType)
                Case Is = FEQL.TokenType.PREDICATE
                    Dim lrModelElement As FBM.ModelObject
                    Dim lsModelElementName As String
                    lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
                    If IsSomething(lrModelElement) Then
                        If lrModelElement.GetType = GetType(FBM.FactType) Then
                            Call Me.AddPredicatePartsToEnterpriseAware(CType(lrModelElement, FBM.FactType).getPredicatePartsForModelObject(lrModelElement))
                        End If
                    Else
                        Dim larCharBeginning() As Char = {"("}
                        Dim larCharEnd() As Char = {")"}
                        lsModelElementName = lsModelElementName.TrimStart(larCharBeginning).TrimEnd(larCharEnd)
                        lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
                        If IsSomething(lrModelElement) Then
                            If lrModelElement.GetType = GetType(FBM.FactType) Then
                                Call Me.AddPredicatePartsToEnterpriseAware(CType(lrModelElement, FBM.FactType).getPredicatePartsForModelObject(lrModelElement))
                            End If
                        End If
                    End If
                Case Is = FEQL.TokenType.MODELELEMENTNAME
                    Call Me.PopulateEnterpriseAwareWithObjectTypes()
                Case Else
                    Call Me.AddEnterpriseAwareItem(Me.zrScanner.Patterns(liTokenType).ToString, liTokenType)
            End Select
        Next

    End Sub

    Private Sub PopulateEnterpriseAwareWithObjectTypes()

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

        'Example
        'Dim lbStartsWith As Boolean = False
        'lbStartsWith = "asdf".StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture)

        For Each lrValueType In prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            If zsIntellisenseBuffer.Length > 0 Then
                If lrValueType.Name.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                    Call Me.AddEnterpriseAwareItem(lrValueType.Name, FEQL.TokenType.MODELELEMENTNAME)
                End If
            Else
                Call Me.AddEnterpriseAwareItem(lrValueType.Name, FEQL.TokenType.MODELELEMENTNAME)
            End If
        Next

        For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
            If zsIntellisenseBuffer.Length > 0 Then
                If lrEntityType.Name.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                    Call Me.AddEnterpriseAwareItem(lrEntityType.Name, FEQL.TokenType.MODELELEMENTNAME)
                End If
            Else
                Call Me.AddEnterpriseAwareItem(lrEntityType.Name, FEQL.TokenType.MODELELEMENTNAME)
            End If
        Next

        For Each lrFactType In prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsObjectified And x.IsMDAModelElement = False)
            If zsIntellisenseBuffer.Length > 0 Then
                If lrFactType.Name.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                    Call Me.AddEnterpriseAwareItem(lrFactType.Name, FEQL.TokenType.MODELELEMENTNAME)
                End If
            Else
                Call Me.AddEnterpriseAwareItem(lrFactType.Name, FEQL.TokenType.MODELELEMENTNAME)
            End If
        Next

    End Sub

    Private Sub frmFactEngine_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        Me.AutoComplete.Hide()
    End Sub

    Private Sub frmFactEngine_Leave(sender As Object, e As EventArgs) Handles Me.Leave
        Me.AutoComplete.Hide()
    End Sub

End Class