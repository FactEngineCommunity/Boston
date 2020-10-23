Imports System.Reflection
Imports MindFusion.Diagramming.Layout

Public Class frmFactEngine

    Public zrScanner As New FEQL.Scanner
    Public zrParser As New FEQL.Parser(Me.zrScanner)
    Public WithEvents zrTextHighlighter As FEQL.TextHighlighter
    Public WithEvents Application As tApplication = prApplication
    Private AutoComplete As frmAutoComplete
    Public zsIntellisenseBuffer As String = ""

    Public msPreviousProductionLookedFor As String

    Public FEQLProcessor As New FEQL.Processor(prApplication.WorkingModel)
    Private TextMarker As FEQL.Controls.TextMarker

    Private GraphNodes As New List(Of FactEngine.DisplayGraph.Node)

    Private miDefaultForeColour As Color = Color.Wheat

    Private mrModel As FBM.Model

    Public Sub autoLayout()

        '---------------------------------------------------------------------------------------
        ' Create the layouter object
        Dim layout As New MindFusion.Diagramming.Layout.SpringLayout

        ' Adjust the attributes of the layouter
        layout.MultipleGraphsPlacement = MultipleGraphsPlacement.MinimalArea
        layout.KeepGroupLayout = True
        layout.NodeDistance = 20
        layout.RandomSeed = 50
        layout.MinimizeCrossings = True
        layout.RepulsionFactor = 10
        layout.EnableClusters = True
        layout.IterationCount = 100
        'layout.GridSize = 20 'Not part of SpringLayout.

        layout.Arrange(Diagram)

        'Dim SecondLayout = New MindFusion.Diagramming.Layout.OrthogonalLayout
        'SecondLayout.Arrange(Me.Diagram)
        Dim lrLink As MindFusion.Diagramming.DiagramLink

        Me.Diagram.RouteAllLinks()

        For Each lrLink In Me.Diagram.Links
            lrLink.Style = MindFusion.Diagramming.LinkStyle.Polyline
            lrLink.SegmentCount = 1
        Next

    End Sub

    Private Sub AddEnterpriseAwareItem(ByVal asEAItem As String,
                                       Optional ByVal aoTagObject As Object = Nothing,
                                       Optional abSetSelectedIndex As Boolean = False,
                                       Optional asModelElementName As String = "",
                                       Optional ab0Index As Boolean = False)

        Dim lrListItem = New tComboboxItem(asModelElementName, asEAItem, aoTagObject)

        If (asEAItem <> "") And Not (Me.AutoComplete.ListBox.FindStringExact(asEAItem) >= 0) Then
            If ab0Index Then
                Me.AutoComplete.ListBox.Items.Insert(0, lrListItem)
            Else
                Me.AutoComplete.ListBox.Items.Add(lrListItem)
            End If
            If abSetSelectedIndex Then
                Me.AutoComplete.ListBox.SelectedIndex = Me.AutoComplete.ListBox.Items.Count - 1
            End If
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

    Private Sub clearGraphView()

        Try
            Me.GraphNodes.Clear()
            Me.Diagram.Nodes.Clear()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Function AddFactTypePredicatePartsToEnterpriseAware() As Boolean

        Dim lasPredicatePart As New List(Of String)
        Dim lsPredicateReadingText As String = ""

        'If prApplication.WorkingModel.FactTypeReading.Count > 0 Then        

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

            Call Me.showAutoCompleteForm()
        End If

    End Sub

    Private Sub AddPredicatePartsToEnterpriseAware(ByVal aarPredicatePart As List(Of FBM.PredicatePart))

        Try
            aarPredicatePart.OrderBy(Function(x) x.PredicatePartText)

            Dim lbBufferIgnored = False

            For Each lrPredicatePart In aarPredicatePart
                If zsIntellisenseBuffer.Length > 0 Then
                    If lrPredicatePart.PredicatePartText.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                        If lrPredicatePart.FactTypeReading.PredicatePart.Count > 1 Then
                            Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE, , lrPredicatePart.FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id)
                        Else
                            Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE)
                        End If
                    End If
                Else
                    If lrPredicatePart.FactTypeReading.PredicatePart.Count > 1 Then
                        Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE, , lrPredicatePart.FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id)
                    Else
                        Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE)
                    End If
                    lbBufferIgnored = True
                End If

            Next

            If Not lbBufferIgnored Then
                For Each lrPredicatePart In aarPredicatePart
                    If zsIntellisenseBuffer.Length > 0 Then
                        If Not lrPredicatePart.PredicatePartText.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                            If lrPredicatePart.FactTypeReading.PredicatePart.Count > 1 Then
                                Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE, , lrPredicatePart.FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id)
                            Else
                                Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE)
                            End If
                        End If
                    End If
                Next
            End If

            Call Me.showAutoCompleteForm()

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

        Try
            If arModelElement Is Nothing Then
                Me.LabelError.ForeColor = Color.Orange
                Me.LabelError.Text = "No Model Element found."
            Else
                Me.LabelError.ForeColor = Color.Black
                Me.LabelError.Text = ""

                Select Case arModelElement.ConceptType
                    Case Is = pcenumConceptType.ValueType

                        Dim lsString = String.Format("{0,6}{1,-" & 20 - arModelElement.Id.Length & "}{2}", arModelElement.Id, " ", CType(arModelElement, FBM.ValueType).DataType.ToString)
                        Me.LabelError.Text &= lsString
                        Dim lrValueType = CType(arModelElement, FBM.ValueType)
                        If lrValueType.DataTypeLength <> 0 Then
                            Me.LabelError.Text &= "(" & lrValueType.DataTypeLength.ToString
                            If lrValueType.DataTypePrecision <> 0 Then
                                Me.LabelError.Text &= "," & lrValueType.DataTypePrecision.ToString & ")"
                            Else
                                Me.LabelError.Text &= ")"
                            End If
                        End If

                    Case Is = pcenumConceptType.EntityType,
                              pcenumConceptType.FactType

                        Dim lrTable = arModelElement.getCorrespondingRDSTable

                        If lrTable Is Nothing Then
                            Me.LabelError.Text &= arModelElement.Id & " does not manifest as a Table or Node within the model. " & arModelElement.Id & " is a/n " & arModelElement.ConceptType.ToString & "."
                            Exit Sub
                        End If

                        Me.LabelError.Text &= lrTable.Name
                        Me.LabelError.Text &= vbCrLf & vbCrLf
                        For Each lrCOlumn In lrTable.Column
                            Dim lsString = String.Format("{0,6}{1,-" & 20 - lrCOlumn.Name.Length & "}{2}", lrCOlumn.Name, " ", lrCOlumn.getMetamodelDataType.ToString)
                            Me.LabelError.Text &= lsString
                            If lrCOlumn.getMetamodelDataTypeLength <> 0 Then
                                Me.LabelError.Text &= "(" & lrCOlumn.getMetamodelDataTypeLength.ToString
                                If lrCOlumn.getMetamodelDataTypePrecision <> 0 Then
                                    Me.LabelError.Text &= "," & lrCOlumn.getMetamodelDataTypePrecision.ToString & ")"
                                Else
                                    Me.LabelError.Text &= ")"
                                End If
                            End If
                            Me.LabelError.Text &= vbCrLf
                        Next

                        Me.LabelError.Text &= vbCrLf & vbCrLf
                        Me.LabelError.Text &= "Relations" & vbCrLf & vbCrLf

                        Dim larRelation As New List(Of RDS.Relation)

                        larRelation.AddRange(lrTable.getOutgoingRelations)
                        For Each lrRelation In lrTable.getIncomingRelations
                            larRelation.AddUnique(lrRelation)
                        Next

                        For Each lrRelation In larRelation

                            Dim lrFactType = lrRelation.ResponsibleFactType
                            Dim lrFactTypeReading As FBM.FactTypeReading
                            If lrRelation.OriginTable Is lrTable Then
                                lrFactTypeReading = lrFactType.getOutgoingFactTypeReadingForTabe(lrTable)
                                If lrFactTypeReading IsNot Nothing Then
                                    Me.LabelError.Text &= lrFactTypeReading.GetReadingTextCQL & vbCrLf
                                Else
                                    Me.LabelError.Text &= lrFactType.Id & "(requires a Fact Type Reading)"
                                End If
                            Else
                                lrFactTypeReading = lrFactType.FactTypeReading.First
                                If lrFactTypeReading IsNot Nothing Then
                                    Me.LabelError.Text &= lrFactTypeReading.GetReadingTextCQL & vbCrLf
                                Else
                                    Me.LabelError.Text &= lrFactType.Id & "(requires a Fact Type Reading)"
                                End If
                            End If
                        Next
                End Select

                'Verbalisation
                Dim lrToolboxForm As frmToolboxORMVerbalisation
                lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
                If IsSomething(lrToolboxForm) Then
                    lrToolboxForm.zrModel = prApplication.WorkingModel
                    Call lrToolboxForm.verbaliseModelElement(arModelElement)
                End If

                'Properties
                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                If lrPropertyGridForm IsNot Nothing Then
                    Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                    If IsSomething(lrPropertyGridForm) Then
                        Dim lrModelElementInstance As FBM.ModelObject = Nothing
                        Dim lrPage As New FBM.Page
                        Select Case arModelElement.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                lrModelElementInstance = CType(arModelElement, FBM.EntityType).CloneInstance(lrPage, False)
                            Case Is = pcenumConceptType.ValueType
                                lrModelElementInstance = CType(arModelElement, FBM.ValueType).CloneInstance(lrPage, False)
                            Case Is = pcenumConceptType.FactType
                                lrModelElementInstance = CType(arModelElement, FBM.FactType).CloneInstance(lrPage, False)
                            Case Is = pcenumConceptType.RoleConstraint
                                lrModelElementInstance = CType(arModelElement, FBM.RoleConstraint).CloneInstance(lrPage, False)
                        End Select
                        If lrModelElementInstance IsNot Nothing Then
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelElementInstance
                            lrPropertyGridForm.BringToFront()
                            lrPropertyGridForm.Show()
                        End If
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
        Me.AutoComplete.Owner = Me

        Me.TextBoxInput.AllowDrop = True

        '-------------------------------------------------------
        'Setup the Text Highlighter
        '----------------------------
        Me.zrTextHighlighter = New FEQL.TextHighlighter(Me.TextBoxInput,
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
                Call Me.FEQLProcessor.DatabaseManager.establishConnection(prApplication.WorkingModel.TargetDatabaseType, prApplication.WorkingModel.TargetDatabaseConnectionString)
            End If
        End If

        Me.ToolStripStatusLabelCurrentProduction.Text = "FactEngine Statement"

    End Sub

    Private Sub ToolStripButtonGO_Click(sender As Object, e As EventArgs) Handles ToolStripButtonGO.Click

        Try
            Call Me.GO()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub GO()

        Try
            With New WaitCursor
                Dim lrRecordset As New ORMQL.Recordset

                'If prApplication.WorkingModel.TargetDatabaseConnectionString = "" Then
                '    Me.LabelError.ForeColor = Color.Orange
                '    Me.LabelError.Text = "The Model needs a database connection string."
                '    Exit Sub
                'End If

                Me.LabelError.Text = ""
                Dim lsQuery = Me.TextBoxInput.Text.Replace(vbLf, " ")
                If Me.TextBoxInput.SelectionLength > 0 Then
                    lsQuery = Me.TextBoxInput.SelectedText
                End If

                Dim lrFEQLTokenType As FEQL.TokenType = Nothing
                Dim lrFEQLParseTree As FEQL.ParseTree = Nothing
                lrRecordset = Me.FEQLProcessor.ProcessFEQLStatement(lsQuery, lrFEQLTokenType, lrFEQLParseTree)

                '====================================================
                If lrRecordset Is Nothing Then
                    'Make sure the VirtualAnalyst is open
                    Call Me.loadVirtualAnalyst()

                    Me.LabelError.BringToFront()
                    Me.TabControl1.SelectedTab = Me.TabPageResults

                    'Pass the FEQL Statement to the Brain.
                    Select Case lrFEQLTokenType
                        Case Is = FEQL.TokenType.VALUETYPEISWRITTENASSTMT
                            Call prApplication.Brain.ProcessFEQLStatement(lsQuery)
                            Me.LabelError.Text = "See the Virtual Analyst toolbox for results/required actions."

                        Case Is = FEQL.TokenType.KEYWDISIDENTIFIEDBYITS
                            Call prApplication.Brain.ProcessFEQLStatement(lsQuery)
                            Me.LabelError.Text = "See the Virtual Analyst toolbox for results/required actions."

                        Case Is = FEQL.TokenType.KEYWDISWHERE
                            Call prApplication.Brain.ProcessFEQLStatement(lsQuery)
                            Me.LabelError.Text = "See the Virtual Analyst toolbox for results/required actions."

                        Case Is = FEQL.TokenType.FACTTYPEPRODUCTION
                            Call prApplication.Brain.ProcessFEQLStatement(lsQuery)
                            Me.LabelError.Text = "See the Virtual Analyst toolbox for results/required actions."

                        Case Is = FEQL.TokenType.KEYWDISAKINDOF
                            Call prApplication.Brain.ProcessFEQLStatement(lsQuery)
                            Me.LabelError.Text = "See the Virtual Analyst toolbox for results/required actions."

                    End Select
                    Exit Sub
                End If
                '=====================================================

                If lrRecordset.Query IsNot Nothing Then
                    Me.TextBoxQuery.Text = lrRecordset.Query
                End If

                If lrRecordset.ErrorString IsNot Nothing Then
                    Me.LabelError.BringToFront()
                    Me.LabelError.Text = lrRecordset.ErrorString
                    Me.TabControl1.SelectedTab = Me.TabPageResults
                Else
                    Select Case lrRecordset.StatementType
                        Case Is = FactEngine.pcenumFEQLStatementType.DESCRIBEStatement
                            Call Me.DesbribeModelElement(lrRecordset.ModelElement)

                        Case Is = FactEngine.pcenumFEQLStatementType.SHOWStatement
                            Call Me.ShowModelElement(lrRecordset.ModelElement)

                        Case Else

                            Me.LabelError.Text = ""

                            If lrRecordset.StatementType = FactEngine.pcenumFEQLStatementType.DIDStatement Then
                                If lrRecordset.Facts.Count = 0 Then

                                    Me.LabelError.Text &= "I don't know." & vbCrLf & vbCrLf
                                Else
                                    Me.LabelError.Text &= "Yes." & vbCrLf & vbCrLf
                                End If
                            End If

                            If lrRecordset.Facts.Count = 0 Then
                                Me.LabelError.ForeColor = Color.Orange
                                Me.LabelError.Text &= "No results returned"
                            Else
                                Me.LabelError.ForeColor = Color.Black
                                Me.LabelError.Text &= ""

                                If lrRecordset.Warning.Count > 0 Then
                                    For Each lsWarning In lrRecordset.Warning
                                        Me.LabelError.Text &= lsWarning & vbCrLf
                                    Next
                                    Me.LabelError.Text &= vbCrLf
                                End If

                                Dim liInd = 0
                                For Each lsColumnName In lrRecordset.Columns
                                    liInd += 1
                                    Me.LabelError.Text &= " " & lsColumnName & " "
                                    If liInd < lrRecordset.Columns.Count Then Me.LabelError.Text &= ","
                                Next
                                Me.LabelError.Text &= vbCrLf & "=======================================" & vbCrLf



                                'For the GraphView
                                Dim lrTable = New RDS.Table(prApplication.WorkingModel.RDS, "DummyTable", Nothing)
                                Dim larColumn As New List(Of RDS.Column)
                                Me.GraphNodes.Clear()
                                Me.Diagram.Nodes.Clear()

                                For Each lrFact In lrRecordset.Facts
                                    Me.LabelError.Text &= lrFact.EnumerateAsBracketedFact(True) & vbCrLf
                                Next

#Region "GraphView"
                                For Each lrFact In lrRecordset.Facts

                                    Dim larTupleNode As New List(Of FactEngine.DisplayGraph.Node)
                                    Dim liColumnInd = 0
                                    Dim lrNodeColumn As RDS.Column

                                    If lrRecordset.QueryGraph Is Nothing Then Exit For

                                    Dim larProjectionColumn = lrRecordset.QueryGraph.ProjectionColumn

                                    'Set up color numbers
                                    Dim liProjectionColumnInd = 0
                                    Dim liProjectionColumnInd2 As Integer
                                    For liProjectionColumnInd = 0 To larProjectionColumn.Count - 1
                                        If liProjectionColumnInd = 0 Then
                                            larProjectionColumn(0).ProjectionOrdinalPosition = 1
                                        ElseIf larProjectionColumn(liProjectionColumnInd).ProjectionOrdinalPosition <> 0 Then
                                            'Don't do anything because has already been set
                                        Else
                                            Dim liMax = (From ProjectionColumn In larProjectionColumn
                                                         Select ProjectionColumn.ProjectionOrdinalPosition).Max
                                            larProjectionColumn(liProjectionColumnInd).ProjectionOrdinalPosition = liMax + 1
                                        End If
                                        For liProjectionColumnInd2 = liProjectionColumnInd + 1 To larProjectionColumn.Count - 1
                                            If larProjectionColumn(liProjectionColumnInd2).Table.Name = larProjectionColumn(liProjectionColumnInd).Table.Name Then
                                                larProjectionColumn(liProjectionColumnInd2).ProjectionOrdinalPosition = larProjectionColumn(liProjectionColumnInd).ProjectionOrdinalPosition
                                            End If
                                        Next
                                    Next

                                    For Each lrRDSColumn In larProjectionColumn

                                        Dim lrTempGraphNode = larTupleNode.Find(Function(x) x.Type = lrRDSColumn.GraphNodeType And
                                                                                        x.Alias = lrRDSColumn.TemporaryAlias)
                                        If lrTempGraphNode Is Nothing Then
                                            larColumn = New List(Of RDS.Column)
                                            lrNodeColumn = lrRDSColumn.Clone(Nothing, Nothing)
                                            lrNodeColumn.TemporaryData = lrFact.Data(liColumnInd).Data
                                            larColumn.Add(lrNodeColumn)
                                            Dim lrGraphNode = New FactEngine.DisplayGraph.Node(Me.Diagram,
                                                                                lrTable,
                                                                                larColumn,
                                                                                lrNodeColumn.GraphNodeType,
                                                                                lrNodeColumn.TemporaryData,
                                                                                lrNodeColumn.TemporaryAlias,
                                                                                New List(Of FactEngine.DisplayGraph.Edge)
                                                                                )
                                            lrGraphNode.OrdinalPosition = lrNodeColumn.ProjectionOrdinalPosition
                                            larTupleNode.AddUnique(lrGraphNode)
                                        Else
                                            lrNodeColumn = lrRDSColumn.Clone(Nothing, Nothing)
                                            lrNodeColumn.TemporaryData = lrFact.Data(liColumnInd).Data
                                            lrTempGraphNode.Column.AddUnique(lrNodeColumn)
                                            Dim lrQueryEdgeAssurityColumn = lrTempGraphNode.Column.Find(AddressOf lrNodeColumn.Equals)
                                            If lrQueryEdgeAssurityColumn IsNot Nothing Then
                                                lrQueryEdgeAssurityColumn.QueryEdge = lrNodeColumn.QueryEdge
                                            End If

                                            lrTempGraphNode.OrdinalPosition = lrNodeColumn.ProjectionOrdinalPosition
                                            larTupleNode.AddUnique(lrTempGraphNode)
                                        End If

                                        liColumnInd += 1
                                    Next

                                    For Each lrTupleNode In larTupleNode
                                        lrTupleNode.Name = ""
                                        For Each lrColumn In lrTupleNode.Column.FindAll(Function(x) x.IsPartOfUniqueIdentifier)
                                            lrTupleNode.Name &= lrColumn.TemporaryData & " "
                                        Next
                                        lrTupleNode.Name = Trim(lrTupleNode.Name)
                                        Dim lrActualGraphNode = Me.GraphNodes.Find(Function(x) x.Type = lrTupleNode.Type And
                                                                                           x.Name = lrTupleNode.Name)
                                        If lrActualGraphNode Is Nothing Then
                                            Me.GraphNodes.Add(lrTupleNode)
                                        End If
                                    Next

                                    liInd = 1
                                    For Each lrTupleNode In larTupleNode.ToArray
                                        Dim lrActualGraphNode = Me.GraphNodes.Find(Function(x) x.Type = lrTupleNode.Type And
                                                                                           x.Name = lrTupleNode.Name)

                                        'Edge/s based on the TupleNode Column's QueryEdge
                                        If liInd > 1 And lrActualGraphNode.Column(0).QueryEdge IsNot Nothing Then
                                            Dim lrBaseTupleNode = larTupleNode.Find(Function(x) x.Type = lrActualGraphNode.Column(0).QueryEdge.BaseNode.Name And
                                                                                            x.Alias = lrActualGraphNode.Column(0).QueryEdge.BaseNode.Alias)

                                            If lrBaseTupleNode IsNot Nothing Then
                                                Dim lrBaseGraphNode = Me.GraphNodes.Find(Function(x) x.Type = lrBaseTupleNode.Type And
                                                                                             x.Name = lrBaseTupleNode.Name)
                                                Dim lrEdge As New FactEngine.DisplayGraph.Edge(lrBaseGraphNode, lrActualGraphNode)

                                                If lrBaseGraphNode Is Nothing Then
                                                    Throw New Exception("frmFactEngine.GO: BaseGraphNode is Nothing.")
                                                End If
                                                lrBaseGraphNode.Edge.Add(lrEdge)
                                            End If
                                        End If
                                        liInd += 1
                                    Next


                                Next

                                For Each lrNode In Me.GraphNodes
                                    Call lrNode.DisplayAndAssociate()
                                Next

                                Dim larEdge = From Node In Me.GraphNodes
                                              From Edge In Node.Edge
                                              Select Edge

                                For Each lrEdge In larEdge

                                    If lrEdge.BaseNode.Shape Is Nothing Or lrEdge.TargetNode.Shape Is Nothing Then
                                        Throw New Exception("frmFactEngine.GO: lrEdge.BaseNode.Shape Is Nothing Or lrEdge.TargetNode.Shape Is Nothing")
                                    End If
                                    Dim lrPGSLink As New MindFusion.Diagramming.DiagramLink(Me.Diagram,
                                                                                            lrEdge.BaseNode.Shape,
                                                                                            lrEdge.TargetNode.Shape)

                                    lrPGSLink.Style = MindFusion.Diagramming.LinkStyle.Polyline

                                    lrPGSLink.SnapToNodeBorder = True
                                    lrPGSLink.ShadowColor = Color.White
                                    lrPGSLink.Brush = New MindFusion.Drawing.SolidBrush(Drawing.Color.DeepSkyBlue)
                                    lrPGSLink.Pen.Color = Drawing.Color.DeepSkyBlue
                                    'lrPGSLink.Text = Me.SentData(0)
                                    lrPGSLink.HeadPen.Color = Drawing.Color.DeepSkyBlue
                                    lrPGSLink.AutoRoute = False
                                    lrPGSLink.Locked = True

                                    'lrPGSLink.Tag = Me
                                    'Me.Link = lrPGSLink                                

                                    Me.Diagram.Links.Add(lrPGSLink)
                                Next

                                Call Me.autoLayout()
#End Region

                            End If
                    End Select
                End If

                If Me.TabControl1.SelectedTab.Name = Me.TabPageGraph.Name Then
                Else
                    Me.TabControl1.SelectedTab = Me.TabPageResults
                    Me.TabPageResults.Show()
                End If
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub loadVirtualAnalyst()
        prApplication.WorkingPage = Nothing
        frmMain.Cursor = Cursors.WaitCursor
        Call frmMain.loadToolboxRichmondBrainBox(Nothing, Me.DockPanel.ActivePane)
        frmMain.Cursor = Cursors.Default
        Me.TextBoxInput.Focus()
    End Sub

    Private Sub Application_WorkingModelChanged() Handles Application.WorkingModelChanged

        Try
            Call Me.displayModelName()

            Me.mrModel = prApplication.WorkingModel

            Me.FEQLProcessor = New FEQL.Processor(prApplication.WorkingModel)

            If prApplication.WorkingModel.TargetDatabaseConnectionString = "" Then
                Me.LabelError.ForeColor = Color.Orange
                Me.LabelError.Text = "The Model needs a database connection string."
                Exit Sub
            End If

            Try
                If prApplication.WorkingModel.DatabaseConnection Is Nothing Then

                    prApplication.WorkingModel.DatabaseConnection = Me.FEQLProcessor.DatabaseManager.establishConnection(prApplication.WorkingModel.TargetDatabaseType,
                                                                                                                     prApplication.WorkingModel.TargetDatabaseConnectionString)
                ElseIf Not prApplication.WorkingModel.DatabaseConnection.Connected Then

                    prApplication.WorkingModel.DatabaseConnection = Me.FEQLProcessor.DatabaseManager.establishConnection(prApplication.WorkingModel.TargetDatabaseType,
                                                                                                                     prApplication.WorkingModel.TargetDatabaseConnectionString)
                End If

            Catch ex As Exception
                Me.LabelError.ForeColor = Color.Orange
                Me.LabelError.Text = ex.Message
            End Try

        Catch ex As Exception
                Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBoxInput_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxInput.KeyDown

        Try
            If Me.TextBoxInput.SelectionColor = Color.Black Then Me.TextBoxInput.SelectionColor = Me.miDefaultForeColour

            Select Case e.KeyCode
                Case Is = Keys.Home, Keys.End
                    Exit Sub
            End Select

            'Handle Paste
            Dim ctrlV As Boolean = e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V
            Dim shiftIns As Boolean = e.Modifiers = Keys.Shift AndAlso e.KeyCode = Keys.Insert
            If ctrlV OrElse shiftIns Then
                Exit Sub
            End If

            If (e.KeyCode = Keys.ShiftKey) Or (e.KeyCode = Keys.ControlKey) Then 'Or (e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V) Then
                Exit Sub
            End If

            Dim liIndex = 0
            Select Case e.KeyData
                Case Is = Keys.Control Or Keys.A
                    If Me.AutoComplete.ListBox.Items.Count > 0 Then
                        If Trim(Me.AutoComplete.ListBox.Items(0).Text) = "AND" And Me.AutoComplete.ListBox.Items.Count > 1 Then liIndex = 1
                        Dim lrComboboxItem As tComboboxItem = Me.AutoComplete.ListBox.Items(liIndex)
                        Me.TextBoxInput.Text &= Trim(lrComboboxItem.Text) & " A " & Trim(lrComboboxItem.ItemData)
                    End If
                    e.SuppressKeyPress = True
                    e.Handled = True
                    Exit Sub
                Case Is = Keys.Control Or Keys.T
                    If Me.AutoComplete.ListBox.Items.Count > 0 Then
                        If Trim(Me.AutoComplete.ListBox.Items(0).Text) = "AND" And Me.AutoComplete.ListBox.Items.Count > 1 Then liIndex = 1
                        Dim lrComboboxItem As tComboboxItem = Me.AutoComplete.ListBox.Items(liIndex)
                        Me.TextBoxInput.Text &= "THAT " & Trim(lrComboboxItem.Text) & " A " & Trim(lrComboboxItem.ItemData)
                    End If
                    Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                    Me.TextBoxInput.SelectionLength = 0
                    e.SuppressKeyPress = True
                    e.Handled = True
                    Exit Sub
                Case Is = Keys.Control Or Keys.N
                    If Me.AutoComplete.ListBox.Items.Count > 0 Then
                        If Trim(Me.AutoComplete.ListBox.Items(0).Text) = "AND" And Me.AutoComplete.ListBox.Items.Count > 1 Then liIndex = 1
                        Dim lrComboboxItem As tComboboxItem = Me.AutoComplete.ListBox.Items(liIndex)
                        Me.TextBoxInput.Text &= Trim(lrComboboxItem.Text) & " (" & Trim(lrComboboxItem.ItemData) & ":'"
                    End If
                    Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                    Me.TextBoxInput.SelectionLength = 0
                    Exit Sub
                Case Is = Keys.Control Or Keys.W
                    If Me.AutoComplete.ListBox.Items.Count > 0 Then
                        Dim lrComboboxItem As tComboboxItem = Me.AutoComplete.ListBox.Items(0)
                        Me.TextBoxInput.Text &= Trim(lrComboboxItem.Text) & " WHICH " & Trim(lrComboboxItem.ItemData) & ":'"
                    End If
                    Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                    Me.TextBoxInput.SelectionLength = 0
                    Exit Sub
            End Select

            '===============================================================================
            'Intellisense Buffer. Populate first for AutoComplete below that...
            Select Case e.KeyCode
                Case Is = Keys.Tab
                    e.Handled = True
                    e.SuppressKeyPress = True
                    Exit Sub
                Case Is = Keys.Control & Keys.V
                Case Is = Keys.Back
                    If zsIntellisenseBuffer.Length > 0 Then
                        zsIntellisenseBuffer = zsIntellisenseBuffer.Substring(0, zsIntellisenseBuffer.Length - 1)
                    End If
                Case Is = Keys.Space, Keys.Escape, Keys.Down, Keys.Up, Keys.Shift, Keys.ShiftKey
                    Me.zsIntellisenseBuffer = ""
                Case Is <> Keys.Menu
                    zsIntellisenseBuffer &= LCase(e.KeyCode.ToString)
            End Select

            Select Case e.KeyCode
                Case Is = Keys.Escape
                    Call Me.hideAutoComplete()
                    Exit Sub
                Case Is = Keys.Down ',Keys.Space
                Case Is = Keys.Left, Keys.Right
                Case Else
                    Call Me.ProcessAutoComplete(e)
            End Select

            Select Case e.KeyCode
                Case Is = Keys.F5
                    Call Me.hideAutoComplete()
                    Call Me.GO()
                Case Is = Keys.Down
                    If Me.TextBoxInput.GetLineFromCharIndex(Me.TextBoxInput.SelectionStart) < Me.TextBoxInput.Lines.Count - 1 Then
                    ElseIf (Me.AutoComplete.ListBox.Items.Count > 0) Or Me.AutoComplete.Visible Then
                        e.Handled = True
                        Call Me.showAutoCompleteForm()
                        Me.AutoComplete.ListBox.SelectedIndex = 0
                        Me.AutoComplete.ListBox.Focus()
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

        Try
            Select Case e.KeyCode
                Case Is = Keys.Space,
                          Keys.Enter,
                          Keys.Back,
                          Keys.Up
                    Me.TextBoxInput.SelectionColor = Me.ForeColor
            End Select

            Dim ctrlV As Boolean = e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V
            Dim shiftIns As Boolean = e.Modifiers = Keys.Shift AndAlso e.KeyCode = Keys.Insert
            If ctrlV OrElse shiftIns Then
                Me.zrTextHighlighter.HighlightText()
                Exit Sub
            End If

            If Me.TextBoxInput.SelectionColor = Color.Black Then Me.TextBoxInput.SelectionColor = Me.ForeColor

            If Me.zrTextHighlighter.Tree.Nodes.Count > 0 Then
                If Me.zrTextHighlighter.Tree.Nodes(0).Nodes.Count > 0 Then
                    If Me.zrTextHighlighter.Tree.Nodes(0).Nodes(0).Nodes.Count > 0 Then
                        Dim liInd = Me.zrTextHighlighter.Tree.Nodes(0).Nodes(0).Nodes.Count
                        If liInd > 0 Then
                            Dim lrLastToken = Me.zrTextHighlighter.Tree.Nodes(0).Nodes(0).Nodes(liInd - 1).Token
                            If lrLastToken.Type = FEQL.TokenType.EOF Then
                                Me.ToolStripStatusLabelGOPrompt.Text = "Valid FEQL Syntax"
                                Me.ToolStripStatusLabelGOPrompt.Visible = True
                            Else
                                Me.ToolStripStatusLabelGOPrompt.Visible = False
                            End If
                        End If
                    End If
                End If
            End If

            Select Case e.KeyCode
                Case Is = Keys.Escape, Keys.F5, Keys.Home, Keys.End, Keys.ShiftKey
                Case Is = Keys.Down, Keys.Up, Keys.Space
                Case Is = Keys.A
                Case Else
                    Select Case e.KeyData
                        Case Is = Keys.Left Or Keys.Shift
                        Case Is = Keys.Right Or Keys.Shift

                        Case Is = Keys.ControlKey Or Keys.A
                            Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                            Me.TextBoxInput.SelectionLength = 0
                            e.SuppressKeyPress = True
                            e.Handled = True
                            Call Me.setAutoCompletePosition()
                        Case Is = Keys.ControlKey Or Keys.T
                            Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                            Me.TextBoxInput.SelectionLength = 0
                            e.SuppressKeyPress = True
                            e.Handled = True
                            Call Me.setAutoCompletePosition()
                        Case Is = Keys.A Or Keys.Control
                            Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                            Me.TextBoxInput.SelectionLength = 0
                            e.SuppressKeyPress = True
                            e.Handled = True
                            Call Me.setAutoCompletePosition()
                        Case Else
                            Call Me.ProcessAutoComplete(New KeyEventArgs(e.KeyCode))
                    End Select
            End Select

            Call Me.setAutoCompletePosition()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub ProcessAutoComplete(ByVal e As System.Windows.Forms.KeyEventArgs)

        Dim lsExpectedToken As String = ""
        Dim liTokenType As FEQL.TokenType
        Dim lsCurrentTokenType As Object

        Try

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

                Dim larModelMatch As New List(Of ModelMatch)
                For Each lrParseNode In larModelElementNameParseNode
                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrParseNode.Token.Text)
                    If lrModelElement Is Nothing Then
                        Me.TextMarker.AddWord(lrParseNode.Token.StartPos, lrParseNode.Token.Length, Color.Red, "Uknown Model Element")

                        For Each lrModel In prApplication.getModelsByModelElementName(lrParseNode.Token.Text)
                            Dim lrModelMatch = New ModelMatch(lrModel, lrParseNode.Token.Text)
                            If larModelMatch.Contains(lrModelMatch) Then
                                larModelMatch.Find(Function(x) x.Model Is lrModelMatch.Model).addKeyWord(lrParseNode.Token.Text)
                            Else
                                larModelMatch.Add(lrModelMatch)
                            End If
                        Next
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

                        For Each lrModel In prApplication.getModelsByPredicatePartText(lsPredicatePartText)
                            Dim lrModelMatch = New ModelMatch(lrModel, lrParseNode.Token.Text)
                            If larModelMatch.Contains(lrModelMatch) Then
                                larModelMatch.Find(Function(x) x.Model Is lrModelMatch.Model).addKeyWord(lsPredicatePartText)
                            Else
                                larModelMatch.Add(lrModelMatch)
                            End If
                        Next
                    End If
                Next

                Me.TextMarker.MarkWords()

                If larModelMatch.Count > 0 Then
                    Me.LabelError.Text = "Are you wanting any of the following models:" & vbCrLf & vbCrLf
                    For Each lrModelMatch In larModelMatch.OrderByDescending(Function(x) x.HitCount)
                        Dim lsKeyWordMatches As String = ""
                        Dim liInd = 0
                        For Each lsKeyWord In lrModelMatch.KeyWords
                            If liInd > 0 Then lsKeyWordMatches &= ","
                            lsKeyWordMatches &= lsKeyWord
                            liInd += 1
                        Next
                        For Each lsKeyWord In lrModelMatch.KeyWords

                        Next
                        Me.LabelError.Text &= lrModelMatch.Model.Name & " " & lrModelMatch.HitCount & " {" & lsKeyWordMatches & "}"
                        Me.LabelError.Text &= vbCrLf
                    Next
                Else
                    Me.LabelError.Text = ""
                End If

            End If
            '=======================================

            Call Me.CheckStartProductions(Me.zrTextHighlighter.Tree)

            If e.KeyCode = Keys.Up And Me.AutoComplete.ListBox.Items.Count > 0 Then
                Me.AutoComplete.ListBox.Items.Clear()
            Else
                Select Case e.KeyCode
                    Case Is = Keys.Down
                    Case Else
                        Me.AutoComplete.ListBox.Items.Clear()
                End Select

            End If

            '============================================================================================
            'Do ultrasmart checking. Finds the last ModelElementName and the last PredicateClause
            '  and if a FactTypeReading is being attempted to be made, finds the next ModelElementName
            If larModelElementNameParseNode.Count > 0 Or larModelPredicateClauseParseNode.Count > 0 Then

                Dim lrFirstModelElementNameParseNode = larModelElementNameParseNode(0)
                Dim lrLastModelElementNameParseNode = larModelElementNameParseNode(larModelElementNameParseNode.Count - 1)
                'Dim lrLastPredicateClauseParseNode = larModelPredicateClauseParseNode(larModelPredicateClauseParseNode.Count - 1)

                Dim lrFirstModelElement = prApplication.WorkingModel.GetModelObjectByName(lrFirstModelElementNameParseNode.Token.Text)
                Dim lrlastModelElement = prApplication.WorkingModel.GetModelObjectByName(lrLastModelElementNameParseNode.Token.Text)

                lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrLastModelElementNameParseNode.Token.Text)

                If lrModelElement Is Nothing Then
                    'Nothing to do here
                Else
                    Select Case Me.FEQLProcessor.getWhichStatementType(Trim(Me.TextBoxInput.Text), True)
                        Case Is = FactEngine.pcenumFEQLStatementType.WHICHSELECTStatement
                            Dim lrWHICHSELECTStatement As New FEQL.WHICHSELECTStatement
                            Call Me.FEQLProcessor.GetParseTreeTokensReflection(lrWHICHSELECTStatement, Me.zrTextHighlighter.Tree.Nodes(0))

                            If lrWHICHSELECTStatement.WHICHCLAUSE.Count = 0 Then
                                lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrWHICHSELECTStatement.MODELELEMENTNAME(0))

                                If lrModelElement IsNot Nothing Then
                                    Dim larFactTypeReading = lrModelElement.getOutgoingFactTypeReadings(2)

                                    If Me.zsIntellisenseBuffer.Length > 0 Then
                                        larFactTypeReading = larFactTypeReading.FindAll(Function(x) x.PredicatePart(0).PredicatePartText.StartsWith(Me.zsIntellisenseBuffer))
                                    End If

                                    For Each lrFactTypeReading In larFactTypeReading.OrderBy(Function(x) x.GetPredicateText)
                                        If lrFactTypeReading.PredicatePart.Count > 1 Then
                                            Dim lsPreboundReadingText = lrFactTypeReading.PredicatePart(1).PreBoundText
                                            Call Me.AddEnterpriseAwareItem(lrFactTypeReading.GetPredicateText, FEQL.TokenType.PREDICATE, , lsPreboundReadingText & lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id)
                                        Else
                                            Call Me.AddEnterpriseAwareItem(lrFactTypeReading.GetPredicateText, FEQL.TokenType.PREDICATE)
                                        End If
                                    Next

                                    If Me.AutoComplete.Visible = False Then
                                        Me.showAutoCompleteForm()
                                    End If
                                End If

                            Else
                                Dim loLastWhichClauseObject = lrWHICHSELECTStatement.WHICHCLAUSE(lrWHICHSELECTStatement.WHICHCLAUSE.Count - 1)
                                Dim lrLastWhichClause As New FEQL.WHICHCLAUSE
                                Call Me.FEQLProcessor.GetParseTreeTokensReflection(lrLastWhichClause, loLastWhichClauseObject)

                                '==========================================================
                                'ModelElement 
                                Dim lsPredicateText As String = ""
                                For Each lsPredicatePart In lrLastWhichClause.PREDICATE
                                    lsPredicateText = Trim(lsPredicateText & " " & Trim(lsPredicatePart))
                                Next

                                Dim larPredicatePart = From FactType In prApplication.WorkingModel.FactType
                                                       From FactTypeReading In FactType.FactTypeReading
                                                       From PredicatePart In FactTypeReading.PredicatePart
                                                       Where PredicatePart.Role.JoinedORMObject.Id = lrModelElement.Id
                                                       Where PredicatePart.PredicatePartText = lsPredicateText
                                                       Select PredicatePart

                                If larPredicatePart.Count = 0 Then
                                    'nothing to do here
                                Else
                                    Me.AutoComplete.ListBox.Items.Clear()
                                    Dim lrPredicatePart = larPredicatePart.First
                                    If lrPredicatePart.FactTypeReading.PredicatePart.Count > 1 Then
                                        If Me.TextBoxInput.Text.Trim.Split(" ").Last <> lrPredicatePart.FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id Then
                                            Dim lsPreboundPart As String = ""
                                            If lrPredicatePart.FactTypeReading.PredicatePart(1).PreBoundText <> "" Then
                                                lsPreboundPart = lrPredicatePart.FactTypeReading.PredicatePart(1).PreBoundText & "-"
                                            End If
                                            Dim lsFullModelElementReference = lsPreboundPart & lrPredicatePart.FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id
                                            Call Me.AddEnterpriseAwareItem(lsFullModelElementReference, FEQL.TokenType.MODELELEMENTNAME, False, lrPredicatePart.FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id)
                                            If Me.AutoComplete.Visible = False Then
                                                Me.showAutoCompleteForm()
                                                Exit Sub
                                            End If
                                        End If
                                    End If
                                End If

                                    'Predicates
                                    Dim lrPredicateModelObject As FBM.ModelObject
                                If lrLastWhichClause.KEYWDAND IsNot Nothing And lrLastWhichClause.KEYWDTHAT.Count = 1 Then
                                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrLastModelElementNameParseNode.Token.Text)
                                End If
                                If lrLastWhichClause.KEYWDWHICH IsNot Nothing Then
                                    lrPredicateModelObject = lrlastModelElement
                                ElseIf lrLastWhichClause.KEYWDTHAT.Count > 0 Then
                                    lrPredicateModelObject = lrlastModelElement
                                Else
                                    lrPredicateModelObject = lrFirstModelElement
                                End If

                                If lrPredicateModelObject IsNot Nothing Then

                                    Dim larFactTypeReading = lrPredicateModelObject.getOutgoingFactTypeReadings(2)

                                    If Me.zsIntellisenseBuffer.Length > 0 Then
                                        larFactTypeReading = larFactTypeReading.FindAll(Function(x) x.PredicatePart(0).PredicatePartText.StartsWith(Me.zsIntellisenseBuffer))
                                    End If

                                    For Each lrFactTypeReading In larFactTypeReading.OrderBy(Function(x) x.GetPredicateText)

                                        Dim lsPreboundReadingText = ""
                                        'For Each lrFactTypeReading In lrPredicateModelObject.getOutgoingFactTypeReadings(2)
                                        If lrFactTypeReading.PredicatePart.Count > 1 Then
                                            If lrFactTypeReading.PredicatePart(1).PreBoundText <> "" Then
                                                lsPreboundReadingText = lrFactTypeReading.PredicatePart(1).PreBoundText
                                            End If
                                            Call Me.AddEnterpriseAwareItem(lrFactTypeReading.GetPredicateText, FEQL.TokenType.PREDICATE, , lsPreboundReadingText & lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id)
                                        Else
                                            Call Me.AddEnterpriseAwareItem(lrFactTypeReading.GetPredicateText, FEQL.TokenType.PREDICATE)
                                        End If
                                    Next

                                    If Me.AutoComplete.Visible = False Then
                                        Me.showAutoCompleteForm()
                                    End If
                                End If

                            End If

                    End Select


                    Dim laiExpectedToken As New List(Of FEQL.TokenType)
                    If Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
                        If Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken <> "" Then
                            laiExpectedToken.Add(DirectCast([Enum].Parse(GetType(FEQL.TokenType), Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken), FEQL.TokenType))
                        End If

                        For Each lrParseError In Me.zrTextHighlighter.Tree.Optionals
                            laiExpectedToken.Add(DirectCast([Enum].Parse(GetType(FEQL.TokenType), lrParseError.ExpectedToken), FEQL.TokenType))
                        Next
                    End If


                    If (Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.IDENTIFIER) Or
                        laiExpectedToken.Contains(FEQL.TokenType.IDENTIFIER) Then
                        lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrLastModelElementNameParseNode.Token.Text)

                        If lrModelElement.ConceptType = pcenumConceptType.ValueType Then
                            If CType(lrModelElement, FBM.ValueType).ValueConstraint.Count > 0 Then
                                For Each lsValue In CType(lrModelElement, FBM.ValueType).ValueConstraint
                                    Call Me.AddEnterpriseAwareItem(lsValue,,,, True)
                                Next
                            End If
                        End If
                        Dim lsSQLQuery = "SELECT "
                        Dim liInd = 0
                        For Each lrColumn In lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                            If liInd > 0 Then lsSQLQuery &= " ,"
                            lsSQLQuery &= lrColumn.Name
                            liInd += 1
                        Next
                        lsSQLQuery &= vbCrLf & "FROM " & lrModelElement.Id
                        lsSQLQuery &= vbCrLf & "LIMIT 20"
                        Dim lrRecordset As ORMQL.Recordset
                        Try
                            lrRecordset = Me.FEQLProcessor.DatabaseManager.GO(lsSQLQuery)

                            For Each lrFact In lrRecordset.Facts
                                Dim lsString As String = ""
                                liInd = 0
                                For Each lrData In lrFact.Data
                                    If liInd > 0 Then lsString &= ","
                                    lsString &= lrData.Data
                                Next
                                Call Me.AddEnterpriseAwareItem(lsString)
                            Next
                        Catch ex As Exception
                            Me.LabelError.Text = ex.Message
                        End Try

                        Call Me.showAutoCompleteForm()
                        Exit Sub
                    End If

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
                    Case Is = FEQL.TokenType.EOF
                    Case Is = FEQL.TokenType.MODELELEMENTSUFFIX
                    Case Is = FEQL.TokenType.BROPEN
                        Me.AutoComplete.Enabled = True
                        Call Me.AddEnterpriseAwareItem("(", liTokenType, True)
                    Case Is = FEQL.TokenType.BRCLOSE
                        Me.AutoComplete.Enabled = True
                        Call Me.AddEnterpriseAwareItem(")", liTokenType, True)
                    Case Is = FEQL.TokenType.PREBOUNDREADINGTEXT, FEQL.TokenType.POSTBOUNDREADINGTEXT
                        'Nothing at this stage for the actual reading texts
                        Call Me.PopulateEnterpriseAwareWithObjectTypes()
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
                        Me.AddEnterpriseAwareItem(Me.zrScanner.Patterns(liTokenType).ToString, liTokenType, True)
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
                        'Me.AutoComplete.Enabled = Me.CheckIfCanDisplayEnterpriseAwareBox
                        Case Is = FEQL.TokenType.FACTTYPEPRODUCTION
                        'Me.AutoComplete.Enabled = Me.CheckIfCanDisplayEnterpriseAwareBox
                        Case Is = FEQL.TokenType.MODELELEMENTNAME
                            Me.AutoComplete.Enabled = True
                            'Call Me.PopulateEnterpriseAwareWithObjectTypes()
                        Case Is = FEQL.TokenType.PREDICATE,
                                  FEQL.TokenType.PREDICATESPACE
                            Me.AutoComplete.Enabled = True
                            Call Me.AddFactTypePredicatePartsToEnterpriseAware()
                    End Select
                End If

                If e.KeyCode = Keys.Down Then
                    If Me.AutoComplete.ListBox.Items.Count > 0 Then
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

                If Me.AutoComplete.ListBox.Items.Count > 0 Then

                    Call Me.showAutoCompleteForm()

                    Call Me.setAutoCompletePosition()
                ElseIf Me.AutoComplete.ListBox.Items.Count = 0 Then
                    Call Me.hideAutoComplete()
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
                        Case Is = FEQL.TokenType.MODELELEMENTSUFFIX
                        Case Is = FEQL.TokenType.PREBOUNDREADINGTEXT, FEQL.TokenType.POSTBOUNDREADINGTEXT
                            'Nothing at this stage
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
                            Else
                                Call Me.PopulateEnterpriseAwareWithObjectTypes()
                            End If

                            Call Me.AddEnterpriseAwareItem("AND ", Nothing, False, , True)
                        Case Is = FEQL.TokenType.MODELELEMENTNAME
                            Me.AutoComplete.Enabled = True
                            'Call Me.PopulateEnterpriseAwareWithObjectTypes()
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
                            'Me.AutoComplete.Enabled = False
                    End Select
                End If

                If Me.AutoComplete.ListBox.Items.Count > 0 Then
                    If Me.AutoComplete.Visible = False Then
                        Call Me.showAutoCompleteForm()
                    End If

                    Call Me.setAutoCompletePosition()
                ElseIf Me.AutoComplete.ListBox.Items.Count = 0 Then
                    Call Me.hideAutoComplete()
                End If

                If e.KeyCode <> Keys.Down Then
                    Me.TextBoxInput.Focus()
                End If

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.StackTrace

            Me.LabelError.Text = lsMessage
        End Try

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
                Case Is = FEQL.TokenType.EMAILADDRESS
                    'Nothing to do here.
                Case Is = FEQL.TokenType.MODELELEMENTSUFFIX
                    'Nothing to do here.
                Case Is = FEQL.TokenType.PREBOUNDREADINGTEXT, FEQL.TokenType.POSTBOUNDREADINGTEXT
                    'Nothing at this stage
                Case Is = FEQL.TokenType.NUMBER, FEQL.TokenType.MATHFUNCTION
                    'Nothing to do here.
                Case Is = FEQL.TokenType.FOLLOWINGREADINGTEXT
                    'Nothing to do here.
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

        Me.AutoComplete.ListBox.Sorted = False

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

        For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
            If zsIntellisenseBuffer.Length > 0 Then
                If LCase(lrEntityType.Name).StartsWith(LCase(zsIntellisenseBuffer), True, System.Globalization.CultureInfo.CurrentUICulture) Then
                    Call Me.AddEnterpriseAwareItem(lrEntityType.Name, FEQL.TokenType.MODELELEMENTNAME)
                End If
            Else
                Call Me.AddEnterpriseAwareItem(lrEntityType.Name, FEQL.TokenType.MODELELEMENTNAME)
            End If
        Next

        For Each lrValueType In prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            If zsIntellisenseBuffer.Length > 0 Then
                If lrValueType.Name.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                    Call Me.AddEnterpriseAwareItem(lrValueType.Name, FEQL.TokenType.MODELELEMENTNAME)
                End If
            Else
                Call Me.AddEnterpriseAwareItem(lrValueType.Name, FEQL.TokenType.MODELELEMENTNAME)
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

    Private Sub frmFactEngine_Leave(sender As Object, e As EventArgs) Handles Me.Leave
        Call Me.hideAutoComplete()
    End Sub

    Private Sub showAutoCompleteForm()
        If Me.AutoComplete.ListBox.Items.Count > 0 Then
            Me.AutoComplete.zsIntellisenseBuffer = Me.zsIntellisenseBuffer
            Me.AutoComplete.zrCallingForm = Me
            If Me.AutoComplete.Visible = False Then
                Me.AutoComplete.Visible = True
                Me.TextBoxInput.Focus()
            End If
            Me.AutoComplete.Owner = Me
            Call Me.populateHelpLabel()
        End If
        'Me.AutoComplete.ListBox.Focus()
    End Sub

    Private Sub setAutoCompletePosition()
        Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
        lo_point.X += Me.TextBoxInput.Bounds.X
        lo_point.Y += Me.TextBoxInput.Bounds.Y
        lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 6
        Me.AutoComplete.Location = PointToScreen(lo_point)
        Me.TextBoxInput.Focus()
    End Sub

    Private Sub TextBoxInput_GotFocus(sender As Object, e As EventArgs) Handles TextBoxInput.GotFocus
        Call Me.setAutoCompletePosition()
    End Sub

    Private Sub TextBoxInput_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxInput.KeyPress

        If Me.AutoComplete.ListBox.Items.Count > 0 Then
            If e.KeyChar.ToString = Me.AutoComplete.ListBox.Items(0).ToString Then
                Call Me.ProcessAutoComplete(New KeyEventArgs(Keys.Space))
            End If
        End If

    End Sub

    Private Sub TextBoxInput_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles TextBoxInput.PreviewKeyDown

        Dim liInd As Integer
        Dim lsSubString As String = ""
        Dim liRemoveFromPosition As Integer = -1
        Dim lsSelectedItem As String = ""

        Try
            Select Case e.KeyCode
                Case Is = Keys.Tab
                    If Me.AutoComplete.ListBox.Items.Count > 0 Then
                        Dim liListboxInd = 0
                        If Trim(Me.AutoComplete.ListBox.Items(0).Text) = "AND" And Me.AutoComplete.ListBox.Items.Count > 1 Then liListboxInd = 1
                        Me.AutoComplete.ListBox.SelectedIndex = liListboxInd
                        '==============================================================
                        'Calculate liRemoveFromPosition to remove relevant characters
                        If Me.TextBoxInput.Text.Substring(Me.TextBoxInput.Text.Length - 1, 1) = " " Then
                            '---------------------
                            'Don't remove spaces
                            '---------------------
                        ElseIf Me.AutoComplete.ListBox.SelectedItem.ToString.Length = 1 Then
                            If Me.TextBoxInput.Text.Substring(Me.TextBoxInput.Text.Length - 1, 1) = Me.AutoComplete.ListBox.SelectedItem.ToString Then
                                liRemoveFromPosition = Me.TextBoxInput.Text.Length - 1
                            End If
                        Else
                            For liInd = Me.AutoComplete.ListBox.SelectedItem.ToString.Length - 1 To 0 Step -1
                                lsSubString = Me.AutoComplete.ListBox.SelectedItem.ToString.Substring(0, liInd + 1)
                                If Me.TextBoxInput.Text.LastIndexOf(Me.AutoComplete.ListBox.SelectedItem.ToString.Substring(0, liInd + 1)) >= 0 Then
                                    If Me.TextBoxInput.Text.LastIndexOf(lsSubString) + lsSubString.Length = Me.TextBoxInput.Text.Length Then
                                        liRemoveFromPosition = Me.TextBoxInput.Text.LastIndexOf(lsSubString)
                                        Exit For
                                    End If
                                End If
                            Next
                        End If

                        If liRemoveFromPosition >= 0 Then
                            Me.TextBoxInput.SelectionProtected = False
                            Dim lsOldText = Me.TextBoxInput.Text
                            Me.TextBoxInput.Text = ""
                            If (Me.TextBoxInput.Text.Length - liRemoveFromPosition) <= Me.AutoComplete.ListBox.SelectedItem.ToString.Length Then
                                Me.TextBoxInput.Text = lsOldText.Remove(liRemoveFromPosition, lsSubString.Length)
                            End If
                        End If

                        If Me.AutoComplete.ListBox.Items.Count > 0 Then
                            Me.TextBoxInput.SelectionProtected = False
                            Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                            If Me.AutoComplete.ListBox.SelectedIndex < 0 Then
                                Me.TextBoxInput.AppendText(Trim(Me.AutoComplete.ListBox.Items(0).ToString)) 'Text.AppendString
                            Else
                                Me.TextBoxInput.AppendText(Trim(Me.AutoComplete.ListBox.SelectedItem.ToString))
                            End If
                            Me.TextBoxInput.SelectionColor = Me.TextBoxInput.ForeColor
                            Call Me.hideAutoComplete()
                        End If
                    End If
            End Select
        Catch ex As Exception
        End Try
    End Sub

    Private Sub populateHelpLabel()

        If Me.AutoComplete.ListBox.Items.Count > 0 Then
            Me.LabelHelp.Text = ""

            Dim lrComboboxItem As tComboboxItem = Me.AutoComplete.ListBox.Items(0)

            Me.LabelHelp.Text &= "Press [Tab] to add " & lrComboboxItem.Text

            If lrComboboxItem.ItemData <> "" Then Me.LabelHelp.Text &= vbCrLf & "Press [Ctrl] and [A] to add '" & Trim(lrComboboxItem.Text) & " A " & Trim(lrComboboxItem.ItemData) & "'"
            If lrComboboxItem.ItemData <> "" Then Me.LabelHelp.Text &= vbCrLf & "Press [Ctrl] and [T] to add 'THAT " & Trim(lrComboboxItem.Text) & " A " & Trim(lrComboboxItem.ItemData) & "'"
            If lrComboboxItem.ItemData <> "" Then Me.LabelHelp.Text &= vbCrLf & "Press [Ctrl] and [N] to add '" & Trim(lrComboboxItem.Text) & " (" & Trim(lrComboboxItem.ItemData) & ":'"
            If lrComboboxItem.ItemData <> "" Then Me.LabelHelp.Text &= vbCrLf & "Press [Ctrl] and [W] to add '" & Trim(lrComboboxItem.Text) & " WHICH " & Trim(lrComboboxItem.ItemData) & "'"

        End If

    End Sub

    Public Sub hideAutoComplete()
        Me.AutoComplete.Hide()
        Me.LabelHelp.Text = ""
    End Sub

    Private Sub TextBoxInput_LostFocus(sender As Object, e As EventArgs) Handles TextBoxInput.LostFocus
        Me.LabelHelp.Text = ""
    End Sub

    Private Sub HideToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideToolStripMenuItem.Click
        Me.ToolStripMenuItemHelpTips.Checked = False
        Me.LabelHelp.Visible = False
    End Sub

    Private Sub ToolStripMenuItemHelpTips_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemHelpTips.Click
        Me.ToolStripMenuItemHelpTips.Checked = Not Me.ToolStripMenuItemHelpTips.Checked

        Me.LabelHelp.Visible = Me.ToolStripMenuItemHelpTips.Checked
    End Sub

    Private Sub ToolStripButtonQueryGO_Click(sender As Object, e As EventArgs) Handles ToolStripButtonQueryGO.Click

        Dim lrRecordset As New ORMQL.Recordset

        Try
            'Clear the Graph View because there is not enough information to create a graph.
            Call Me.clearGraphView()

            If prApplication.WorkingModel.TargetDatabaseConnectionString = "" Then
                Me.LabelError.ForeColor = Color.Orange
                Me.LabelError.Text = "The Model needs a database connection string."
                Exit Sub
            End If

            Me.LabelError.Text = ""

            'Get the Query from the SQL/Cypher/etc query textbox
            Dim lsQuery = Me.TextBoxQuery.Text '.Replace(vbLf, " ") 'Leave this here.

            'If the user has highlighted a section of a query to execute, execute that.
            If Me.TextBoxQuery.SelectionLength > 0 Then
                lsQuery = Me.TextBoxInput.SelectedText
            End If

            lrRecordset = Me.FEQLProcessor.DatabaseManager.GO(lsQuery)

            If lrRecordset.Query IsNot Nothing Then
                Me.TextBoxQuery.Text = lrRecordset.Query
            End If

            If lrRecordset.ErrorString IsNot Nothing Then
                Me.LabelError.Show()
                Me.LabelError.BringToFront()
                Me.LabelError.Text = lrRecordset.ErrorString
                Me.TabControl1.SelectedTab = Me.TabPageResults
            Else
                Me.TabControl1.SelectedTab = Me.TabPageResults
                Me.LabelError.Show()
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

        Catch ex As Exception
            Dim lsMessage As String
        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
        lsMessage &= vbCrLf & vbCrLf & ex.Message
        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub frmFactEngine_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed

        frmMain.zfrmFactEngine = Nothing

    End Sub

    Private Sub LayoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LayoutToolStripMenuItem.Click

        Me.autoLayout()

    End Sub

    Private Sub GraphView_MouseWheel(sender As Object, e As MouseEventArgs) Handles GraphView.MouseWheel

        Select Case e.Delta
            Case Is = 0
            Case Is < 0
                Me.GraphView.ZoomFactor = Viev.Greater(0, Me.GraphView.ZoomFactor - 5)
            Case Is > 0
                Me.GraphView.ZoomFactor = Viev.Lesser(100, Me.GraphView.ZoomFactor + 5)
        End Select

    End Sub

    Private Sub Application_ConfigurationChanged() Handles Application.ConfigurationChanged

        Try
            If Me.FEQLProcessor.DatabaseManager.Connection IsNot Nothing Then
                Me.FEQLProcessor.DatabaseManager.Connection.DefaultQueryLimit = My.Settings.FactEngineDefaultQueryResultLimit
            End If
        Catch ex As Exception
            Me.LabelError.Show()
            Me.LabelError.BringToFront()
            Me.LabelError.Text = ex.Message
            Me.TabControl1.SelectedTab = Me.TabPageResults
        End Try


    End Sub

    Private Sub VirtualAnalystToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VirtualAnalystToolStripMenuItem.Click

        prApplication.WorkingPage = Nothing

        frmMain.Cursor = Cursors.WaitCursor
        Call frmMain.loadToolboxRichmondBrainBox(Nothing, Me.DockPanel.ActivePane)
        frmMain.Cursor = Cursors.Default

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub ModelDictionaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModelDictionaryToolStripMenuItem.Click

        Call frmMain.LoadToolboxModelDictionary(True)

        Dim lrFrmModelDictionary = CType(prApplication.GetToolboxForm(frmToolboxModelDictionary.Name), frmToolboxModelDictionary)
        Call lrFrmModelDictionary.LoadToolboxModelDictionary(pcenumLanguage.EntityRelationshipDiagram, True)

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        Dim lrPropertyGridForm As frmToolboxProperties

        If IsSomething(prApplication.GetToolboxForm(frmToolboxProperties.Name)) Then
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
            'If Me.Diagram.Selection.Items.Count > 0 Then
            '    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            'Else
            '    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage
            'End If
        End If

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub ErrorListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ErrorListToolStripMenuItem.Click

        Call frmMain.loadToolboxErrorListForm(Me.DockPanel.ActivePane)

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem8.Click

        Call frmMain.loadToolboxORMReadingEditor(Nothing, Me.DockPanel.ActivePane)

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub ORMVerbalisationViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ORMVerbalisationViewToolStripMenuItem.Click

        Call frmMain.loadToolboxORMVerbalisationForm(prApplication.WorkingModel, Me.DockPanel.ActivePane)

        Me.TextBoxInput.Focus()

    End Sub

    Private Sub ShowModelElement(ByRef arModelElement As FBM.ModelObject)

        If prApplication.WorkingModel Is Nothing Then
            MsgBox("Select a Model in the Model Explorer. There is currently no Model selected.")
            Exit Sub
        End If

        Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(prApplication.WorkingModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

        Call frmMain.LoadDiagramSpy(lrDiagramSpyPage, arModelElement)

    End Sub

    Private Sub ToolStripMenuItemDarkBackground_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemDarkBackground.Click

        Me.ToolStripMenuItemDarkBackground.Checked = Not Me.ToolStripMenuItemDarkBackground.Checked
        Me.ToolStripMenuItemLightBackground.Checked = Not Me.ToolStripMenuItemDarkBackground.Checked

        If Me.ToolStripMenuItemDarkBackground.Checked Then
            Me.TextBoxInput.BackColor = Color.FromArgb(64, 64, 64)
            Me.TextBoxInput.ForeColor = Color.Wheat
            Me.miDefaultForeColour = Color.Wheat
            Me.AutoComplete.BackColor = Color.GhostWhite
        Else
            Me.TextBoxInput.BackColor = Color.FromArgb(255, 255, 255)
            Me.TextBoxInput.ForeColor = Color.Sienna
            Me.miDefaultForeColour = Color.Sienna
            Me.AutoComplete.BackColor = Color.LightGray
        End If

    End Sub

    Private Sub ToolStripMenuItemLightBackground_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemLightBackground.Click

        Me.ToolStripMenuItemLightBackground.Checked = Not Me.ToolStripMenuItemLightBackground.Checked
        Me.ToolStripMenuItemDarkBackground.Checked = Not Me.ToolStripMenuItemLightBackground.Checked

        If Me.ToolStripMenuItemLightBackground.Checked Then
            Me.TextBoxInput.BackColor = Color.FromArgb(255, 255, 255)
            Me.TextBoxInput.ForeColor = Color.Sienna
            Me.miDefaultForeColour = Color.Sienna
            Me.AutoComplete.BackColor = Color.LightGray
        Else
            Me.TextBoxInput.BackColor = Color.FromArgb(64, 64, 64)
            Me.TextBoxInput.ForeColor = Color.Wheat
            Me.miDefaultForeColour = Color.Wheat
            Me.AutoComplete.BackColor = Color.GhostWhite
        End If

    End Sub

    Private Sub TextBoxInput_DragDrop(sender As Object, e As DragEventArgs) Handles TextBoxInput.DragDrop

        Dim lsMessage As String

        Try
            If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

                '------------------------------------------------------------------------------------------------------------------------------------
                'Make sure the current page points to the Diagram on this form. The reason is that the user may be viewing the Page as an ORM Model
                '------------------------------------------------------------------------------------------------------------------------------------
                Dim loDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

                If loDraggedNode.Tag.GetType Is GetType(RDS.Table) Then

                    Dim lrTable As RDS.Table
                    lrTable = loDraggedNode.Tag

                    '==================================================================================================================
                    Dim lsSQLQuery As String = ""
                    Dim lrRecordset As ORMQL.Recordset


                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " WHERE Element = '" & lrTable.Name & "'"

                    lrRecordset = prApplication.WorkingModel.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset.EOF Then
                        lsMessage = "The Entity, '" & lrTable.Name & "', does not seem to have any Attributes at this stage. Make sure that the corresponding Model Element in your Object-Role Model at least has a Primary Reference Scheme."
                        lsMessage &= vbCrLf & vbCrLf & "Entities in Entity-Relationship Diagrams in Boston have their Attributes created by the relative relations of the corresponding Model Element in your Object-Role Model."

                        MsgBox(lsMessage)
                        Exit Sub
                    Else
                        With New WaitCursor
                            Dim lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrTable.Name)
                            Call Me.DesbribeModelElement(lrModelElement)

                        End With

                    End If
                End If
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try



    End Sub

    Private Sub TextBoxInput_DragOver(sender As Object, e As DragEventArgs) Handles TextBoxInput.DragOver

        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            Dim lrDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            '-----------------------------------------------------------------------
            'Get the Object being dragged (if there is one).
            '  If the user is dragging from the ModelDictionary form, 
            '  then the DragItem will have a Tag of the ModelObject being dragged.
            '-----------------------------------------------------------------------
            Dim lrModelOject As Object
            lrModelOject = lrDraggedNode.Tag

            If Not (TypeOf (lrModelOject) Is MindFusion.Diagramming.Shape) Then
                e.Effect = DragDropEffects.Copy
            ElseIf lrDraggedNode.Index >= 0 Then
                Dim lrToolboxForm As frmToolbox
                lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)
                If (lrDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount) Then
                    Select Case lrDraggedNode.Tag.Id
                        Case Is = "Entity"
                            e.Effect = DragDropEffects.Copy
                        Case Else
                            e.Effect = DragDropEffects.None
                    End Select
                End If
            End If
        End If
    End Sub

    Private Sub frmFactEngine_Enter(sender As Object, e As EventArgs) Handles Me.Enter

        prApplication.WorkingPage = Nothing

    End Sub

End Class