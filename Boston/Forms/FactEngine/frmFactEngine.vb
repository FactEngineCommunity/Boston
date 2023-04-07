Imports System.Reflection
Imports MindFusion.Diagramming.Layout
Imports MindFusion.Diagramming
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports OpenAI_API
Imports OpenAI_API.Completions
Imports OpenAI_API.Models
Imports System.Threading.Tasks

Public Class frmFactEngine

    Public zrScanner As New FEQL.Scanner
    Public zrParser As New FEQL.Parser(Me.zrScanner)
    Public WithEvents zrTextHighlighter As FEQL.TextHighlighter
    Public WithEvents Application As tApplication = prApplication
    Private AutoComplete As frmAutoComplete
    Public zsIntellisenseBuffer As String = ""

    Private zbTextBoxNaturalLanguageFocused As Boolean = False

    Public msPreviousProductionLookedFor As String

    Public FEQLProcessor As New FEQL.Processor(prApplication.WorkingModel)
    Private TextMarker As FEQL.Controls.TextMarker

    Private GraphNodes As New List(Of FactEngine.DisplayGraph.Node)

    Private miDefaultForeColour As Color = Color.Wheat

    Private mrModel As FBM.Model

    Dim mrOpenAIAPI As New OpenAI_API.OpenAIAPI(New APIAuthentication(My.Settings.FactEngineOpenAIAPIKey))

    <DllImport("user32.dll")>
    Private Shared Function GetKeyboardState(ByVal lpKeyState As Byte()) As Boolean
    End Function
    <DllImport("user32.dll")>
    Private Shared Function MapVirtualKey(ByVal uCode As UInteger, ByVal uMapType As UInteger) As UInteger
    End Function
    <DllImport("user32.dll")>
    Private Shared Function GetKeyboardLayout(ByVal idThread As UInteger) As IntPtr
    End Function
    <DllImport("user32.dll")>
    Private Shared Function ToUnicodeEx(ByVal wVirtKey As UInteger, ByVal wScanCode As UInteger, ByVal lpKeyState As Byte(),
    <Out, MarshalAs(UnmanagedType.LPWStr)> ByVal pwszBuff As System.Text.StringBuilder, ByVal cchBuff As Integer, ByVal wFlags As UInteger, ByVal dwhkl As IntPtr) As Integer
    End Function

    Private Sub frmFactEngine_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.AutoComplete = New frmAutoComplete(Me.TextBoxInput)
        Me.AutoComplete.Owner = Me
        Me.AutoComplete.moBackgroundWorker = Me.BackgroundWorker

        Me.TextBoxInput.AllowDrop = True

        Me.ToolStripMenuItemNaturalLanguage.Visible = My.Settings.FactEngineUseTransformations

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
            If prApplication.WorkingModel.RequiresConnectionString And Trim(prApplication.WorkingModel.TargetDatabaseConnectionString) = "" Then
                Me.ToolStripStatusLabelRequiresConnectionString.ForeColor = Color.Orange
                Me.ToolStripStatusLabelRequiresConnectionString.Text = "Model requires a database connection string"
            Else
                Me.ToolStripStatusLabelRequiresConnectionString.Text = ""
                Call Me.FEQLProcessor.DatabaseManager.establishConnection(prApplication.WorkingModel.TargetDatabaseType, prApplication.WorkingModel.TargetDatabaseConnectionString)
            End If
        End If

        Me.ToolStripStatusLabelCurrentProduction.Text = "FactEngine Statement"
        Me.ToolStripStatusLabelError.Text = ""

        Call Me.SetupForm()

    End Sub

    Public Function KeyCodeToUnicode(ByVal key As Keys) As String
        Dim keyboardState As Byte() = New Byte(254) {}
        Dim keyboardStateStatus As Boolean = GetKeyboardState(keyboardState)

        If Not keyboardStateStatus Then
            Return ""
        End If

        Dim virtualKeyCode As UInteger = CUInt(key)
        Dim scanCode As UInteger = MapVirtualKey(virtualKeyCode, 0)
        Dim inputLocaleIdentifier As IntPtr = GetKeyboardLayout(0)
        Dim result As New System.Text.StringBuilder()
        ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, CInt(5), CUInt(0), inputLocaleIdentifier)
        Return result.ToString()
    End Function

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

        Call Me.DisambiguateOverlappingLinks()

    End Sub

    Private Sub DisambiguateOverlappingLinks()

        '--------------------------------
        'Disambiguate overlapping links
        '--------------------------------

        For Each lrLink In Me.Diagram.Links
            Dim commonLinks As DiagramLinkCollection = GetCommonLinks(lrLink.Origin, lrLink.Destination)

            Dim pt1 As PointF = lrLink.ControlPoints(0)
            Dim pt2 As PointF = lrLink.ControlPoints(lrLink.ControlPoints.Count - 1)

            If commonLinks.Count > 1 Then
                For c As Integer = 0 To commonLinks.Count - 1

                    Dim link As DiagramLink = commonLinks(c)

                    If link.Origin Is link.Destination Then

                    Else
                        '===================================                        
                        'If Not link.Tag.HasBeenMoved Then
                        link.Style = LinkStyle.Bezier
                        link.SegmentCount = 1 'Keep as 1, because bows () links. 2 makes funny bezier links.

                        Dim cp1 As New PointF(pt1.X + 1 * (pt2.X - pt1.X) / 3, pt1.Y + 1 * (pt2.Y - pt1.Y) / 3)
                        Dim cp2 As New PointF(pt1.X + 2 * (pt2.X - pt1.X) / 3, pt1.Y + 2 * (pt2.Y - pt1.Y) / 3)

                        Dim angle As Single = 0, radius As Single = 0
                        CarteseanToPolar(pt1, pt2, angle, radius)

                        Dim pairOffset As Integer = (c / 2 + 1) * 5
                        'If commonLinks.Count Mod 2 = 0 Then
                        PolarToCartesean(cp1, If(c Mod 2 = 0, angle - 90, angle + 90), pairOffset, cp1)
                        PolarToCartesean(cp2, If(c Mod 2 = 0, angle - 90, angle + 90), pairOffset, cp2)

                        If link.ControlPoints(0) = pt1 Then
                            link.ControlPoints(1) = cp1
                            link.ControlPoints(2) = cp2
                        Else
                            link.ControlPoints(1) = cp2
                            link.ControlPoints(2) = cp1
                        End If

                        'link.Tag.HasBeenMoved = True

                        link.UpdateFromPoints()

                        '===================================
                    End If
                Next
            Else
                'lrLink.AutoRoute = True
                lrLink.SegmentCount = 2
                lrLink.SegmentCount = 1
            End If
        Next

    End Sub


    Private Sub CarteseanToPolar(ByVal coordCenter As PointF, ByVal dekart As PointF, ByRef a As Single, ByRef r As Single)
        If coordCenter = dekart Then
            a = 0
            r = 0
            Return
        End If

        Dim dx As Single = dekart.X - coordCenter.X
        Dim dy As Single = dekart.Y - coordCenter.Y
        r = CSng(Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)))

        a = CSng(Math.Atan(-dy / dx) * 180 / Math.PI)
        If dx < 0 Then
            a += 180
        End If
    End Sub

    Private Sub PolarToCartesean(ByVal coordCenter As PointF, ByVal a As Single, ByVal r As Single, ByRef dekart As PointF)
        If r = 0 Then
            dekart = coordCenter
            Return
        End If

        dekart.X = CSng(coordCenter.X + Math.Cos(a * Math.PI / 180) * r)
        dekart.Y = CSng(coordCenter.Y - Math.Sin(a * Math.PI / 180) * r)
    End Sub

    Private Function GetCommonLinks(ByVal node1 As DiagramNode, ByVal node2 As DiagramNode) As DiagramLinkCollection
        Dim commonLinks As New DiagramLinkCollection()

        For Each link As DiagramLink In node1.OutgoingLinks
            If link.Destination.Tag.Name = node2.Tag.Name Then
                commonLinks.Add(link)
            End If
        Next

        For Each link As DiagramLink In node1.IncomingLinks
            If link.Origin.Tag.Name = node2.Tag.Name Then
                commonLinks.Add(link)
            End If
        Next

        Return commonLinks

    End Function

    Private Sub AddEnterpriseAwareItem(ByVal asEAItem As String,
                                       Optional ByRef aoTagObject As Object = Nothing,
                                       Optional ByVal abSetSelectedIndex As Boolean = False,
                                       Optional ByVal asModelElementName As String = "",
                                       Optional ByVal ab0Index As Boolean = False,
                                       Optional ByVal abOverrideAlreadyExists As Boolean = False)

        If aoTagObject Is Nothing Then aoTagObject = VAQL.TokenType.PREDICATEPART

        Dim lrListItem = New tComboboxItem(asModelElementName, asEAItem, aoTagObject)

        Dim lbAlreadyExists As Boolean = False

        Dim larExists = From Item In Me.AutoComplete.ListBox.Items
                        Where lrListItem.EqualsAll(Item)
                        Select Item

        lbAlreadyExists = larExists.Count > 0

        If (asEAItem <> "") And (Not lbAlreadyExists Or abOverrideAlreadyExists) Then
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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

            'Call Me.showAutoCompleteForm()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                            Me.LabelError.Text &= LTrim(lsString)
                            If lrCOlumn.getMetamodelDataTypeLength <> 0 Then
                                Me.LabelError.Text &= "(" & lrCOlumn.getMetamodelDataTypeLength.ToString
                                If lrCOlumn.getMetamodelDataTypePrecision <> 0 Then
                                    Me.LabelError.Text &= "," & lrCOlumn.getMetamodelDataTypePrecision.ToString & ")"
                                Else
                                    Me.LabelError.Text &= ")"
                                End If
                            End If

                            If lrCOlumn.Role.Mandatory Then Me.LabelError.Text &= "  NOT NULL"

                            If lrCOlumn.getReferencedColumn IsNot Nothing Then
                                Me.LabelError.Text &= "  " & lrCOlumn.FactType.getOutgoingFactTypeReading(lrCOlumn.Role.JoinedORMObject).GetReadingText
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub


    Private Sub displayModelName()
        If prApplication.WorkingModel Is Nothing Then
            Me.ToolStripStatusLabelWorkingModelName.ForeColor = Color.Orange
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: Select a Model in the Model Explorer"
        Else
            Me.ToolStripStatusLabelWorkingModelName.ForeColor = Color.Black
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: " & prApplication.WorkingModel.Name
            If prApplication.WorkingModel.RequiresConnectionString And Trim(prApplication.WorkingModel.TargetDatabaseConnectionString) = "" Then
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

    Private Sub ToolStripButtonGO_Click(sender As Object, e As EventArgs) Handles ToolStripButtonGO.Click

        Try
            Dim lbNaturalLanguageTextBoxHasFocus = Me.zbTextBoxNaturalLanguageFocused

            If My.Computer.Keyboard.CtrlKeyDown Then
                prApplication.WorkingModel.connectToDatabase(True)
            End If

            Call Me.GO(lbNaturalLanguageTextBoxHasFocus)

            If Me.zbTextBoxNaturalLanguageFocused Then
                Me.TextBoxNaturalLanguage.Focus()
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="asNLQuery">May contain training data</param>
    ''' <param name="asActualNLQuery">The actual NL query being processed.</param>
    ''' <returns></returns>
    Private Function GetGPT3Result(ByVal asNLQuery As String, Optional ByVal asActualNLQuery As String = "") As CompletionResult

        Try
            Dim liMaxTokens As Integer = Viev.Greater(80, CInt(asActualNLQuery.Split(" ").Length * 3.5))

            Return Task.Run(Function() Me.mrOpenAIAPI.Completions.CreateCompletionAsync(
                New CompletionRequest(asNLQuery, model:=Model.DavinciText, temperature:=0, max_tokens:=liMaxTokens)
                )).Result

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return New CompletionResult
        End Try

    End Function

    Private Function GetGPT3ChatResult(ByVal asNLQuery As String) As OpenAI_API.Chat.ChatResult

        Try
            Dim _timeout As Integer = 5000 ' 5 seconds
            Dim _cancellationTokenSource As New System.Threading.CancellationTokenSource

            'Return Task.Run(Function() Me.mrOpenAIAPI.Chat.CreateChatCompletionAsync(ByVal request As ChatRequest) As Task(Of ChatResult)
            Return Task.Run(Function() Me.mrOpenAIAPI.Chat.CreateChatCompletionAsync(
                            New OpenAI_API.Chat.ChatRequest() With {.Model = Model.ChatGPTTurbo,
                                                                    .Temperature = 0.1,
                                                                    .MaxTokens = 50,
                                                                    .Messages = New OpenAI_API.Chat.ChatMessage() {New OpenAI_API.Chat.ChatMessage(OpenAI_API.Chat.ChatMessageRole.User, asNLQuery)}})).Result

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Private Sub GO(Optional ByVal abUseNaturalLanguage As Boolean = False)

        Try
            With New WaitCursor
                Dim lrRecordset As New ORMQL.Recordset

                'If prApplication.WorkingModel.TargetDatabaseConnectionString = "" Then
                '    Me.LabelError.ForeColor = Color.Orange
                '    Me.LabelError.Text = "The Model needs a database connection string."
                '    Exit Sub
                'End If

#Region "Transformations"
                'TextBoxNaturalLanguage
                If My.Settings.FactEngineUseTransformations And abUseNaturalLanguage Then

                    'If abUseNaturalLanguage Then
                    Dim lsNaturalLanguageQuery = Trim(Me.TextBoxNaturalLanguage.Text)
                    Me.TextBoxInput.Text = lsNaturalLanguageQuery
                    'End If

                    Dim lrLanguageParser As New Language.LanguageGeneric(My.Settings.WordNetDictionaryEnglishPath)

                    Try
                        If My.Settings.FactEngineUseGPT3 Then
#Region "GPT3 Transforms"
                            Dim loTransformation As Object = New System.Dynamic.ExpandoObject
                            Dim larTransformationTuples = TableReferenceFieldValue.GetReferenceFieldValueTuples(36, loTransformation)

                            Dim lsGPT3TrainingExamplesFilePath = larTransformationTuples.Where(Function(x) x.ModelId = prApplication.WorkingModel.ModelId).Select(Function(x) x.GPT3TrainingFileLocation)(0)

                            If lsGPT3TrainingExamplesFilePath Is Nothing Then Throw New Exception("Check to see if you have configured a GPT3 Training file path for the current model.")

                            Dim lsGPTTrainingExamples = System.IO.File.ReadAllText(lsGPT3TrainingExamplesFilePath)

                            Dim lrCompletionResult = Me.GetGPT3Result(lsGPTTrainingExamples & "#" & lsNaturalLanguageQuery & vbCrLf, lsNaturalLanguageQuery)
                            Dim lsGPT3ReturnString = lrCompletionResult.Completions(0).Text

                            'Dim lrCompletionResult = Me.GetGPT3ChatResult(lsGPTTrainingExamples & "#" & lsNaturalLanguageQuery & vbCrLf)
                            'Dim lsGPT3ReturnString = lrCompletionResult.Choices(0).Message.Content

                            Me.TextBoxInput.Text = lsGPT3ReturnString.Substring(0, Boston.returnIfTrue(lsGPT3ReturnString.IndexOf(vbCrLf) = -1, lsGPT3ReturnString.Length, lsGPT3ReturnString.IndexOf(vbCrLf)))
#End Region

                        Else
#Region "Regular Expression Transforms"

#Region "Substitute for ModelElement names"
                            Dim lasWords() = Me.TextBoxInput.Text.Split(" ")

                            Dim lrModelElement As FBM.ModelObject
                            For Each lsWord In lasWords
                                Dim lsTempWord = lsWord.Replace(",", "")
                                Dim lsLemma As String = Nothing
                                If lrLanguageParser.WordIsNoun(lsTempWord, lsLemma) Or (prApplication.WorkingModel.GetModelObjectByName(lsTempWord, True, True) IsNot Nothing) Then

                                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsTempWord, True, True)

                                    If lrModelElement Is Nothing Then
                                        lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsLemma, True, True)
                                    End If

                                    If lrModelElement Is Nothing Then
                                        lrModelElement = prApplication.WorkingModel.ModelElements.Find(Function(x) Fastenshtein.Levenshtein.Distance(x.Id, lsTempWord) < 2)
                                    End If

                                    If lrModelElement IsNot Nothing Then
                                        'CodeSafe
                                        If lrModelElement.Id = "" Then GoTo NextModelElementFind 'Found something like this once.
                                        Me.TextBoxInput.Text = Me.TextBoxInput.Text.Replace(lsTempWord, lrModelElement.Id)
                                    End If
                                End If
NextModelElementFind:
                            Next
#End Region

                            Dim loTransformation As Object = New System.Dynamic.ExpandoObject
                            Dim larTransformationTuples = TableReferenceFieldValue.GetReferenceFieldValueTuples(35, loTransformation).OrderBy(Function(x) x.SequenceNr)

#Region "Check for Double_Word with underscore model element names"
                            Dim Words() As String = Me.TextBoxInput.Text.Split(" ")
                            Dim lsConcatWords As String = Nothing
                            Dim lsConcatWordsUnderscore As String = Nothing
                            Dim lsCamelConcatWords As String = Nothing

                            Dim lasSkipWords() = {"a"}

                            For liWordCount = 3 To 2 Step -1

                                For liInd = 0 To Words.Count - liWordCount

                                    lsConcatWords = ""
                                    For liInd2 = 0 To liWordCount - 1
                                        If lasSkipWords.Contains(Words(liInd + liInd2)) Then GoTo SkipWord
                                        lsConcatWords &= Words(liInd + liInd2) & " "
                                    Next
                                    lsConcatWords = Trim(lsConcatWords)

                                    lsConcatWordsUnderscore = ""
                                    For liInd2 = 0 To liWordCount - 1
                                        lsConcatWordsUnderscore &= Words(liInd + liInd2)
                                        If liInd2 < liWordCount - 1 Then
                                            lsConcatWordsUnderscore &= "_"
                                        End If
                                    Next
                                    'Words(liInd) & "_" & Words(liInd + 1)
                                    lsCamelConcatWords = Viev.Strings.MakeCapCamelCase(lsConcatWordsUnderscore)
                                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsCamelConcatWords, True,,, True)
                                    If lrModelElement IsNot Nothing Then
                                        Me.TextBoxInput.Text = Me.TextBoxInput.Text.Replace(lsConcatWords, lrModelElement.Id)
                                    End If

                                    lsCamelConcatWords = Viev.Strings.MakeCapCamelCase(lsConcatWords)
                                    If prApplication.WorkingModel.GetModelObjectByName(lsCamelConcatWords, True,,, True) IsNot Nothing Then
                                        Me.TextBoxInput.Text = Me.TextBoxInput.Text.Replace(lsConcatWords, lsCamelConcatWords)
                                    End If
SkipWord:
                                Next
                            Next
#End Region

                            For Each loTransformation In larTransformationTuples
                                Dim regex As Regex = New Regex(loTransformation.FindRegEx)
                                Dim match As Match = regex.Match(Me.TextBoxInput.Text)
                                If match.Success Then
                                    Me.TextBoxInput.Text = System.Text.RegularExpressions.Regex.Replace(Me.TextBoxInput.Text, loTransformation.FindRegEx, loTransformation.ReplaceWithRegEx)
                                End If
                            Next

                            'Tidy up
                            lasWords = Me.TextBoxInput.Text.Split(" ")

                            For Each lsWord In lasWords

                                'CodeSafe
                                If lsWord = "" Then GoTo NextWord

                                lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsWord, True, True)

                                If lrModelElement IsNot Nothing Then
                                    Me.TextBoxInput.Text = Me.TextBoxInput.Text.Replace(lsWord, lrModelElement.Id)
                                End If
NextWord:
                            Next
#End Region

                        End If
                    Catch ex As Exception
                        Me.LabelError.BringToFront()
                        Me.LabelError.Text = "Woops. Error executing Transformations. Contact FactEngine support." & vbCrLf & vbCrLf & ex.Message
                        Me.TabControl1.SelectedTab = Me.TabPageResults
                        Me.LabelError.ForeColor = Color.Orange
                        Exit Sub
                    End Try
                End If




#End Region
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

                        Case Is = FEQL.TokenType.CREATEDATABASESTMT
                            'Nothing at this stage
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
                    Me.LabelError.ForeColor = Color.Orange

                    If lrRecordset.ApplicationException IsNot Nothing Then
                        'Put code here to engage the VirtualAnalyst to create a FactTypeReading
                        '2021-VM-Use the following type of code for adding new FactTypeReadings
                        If lrRecordset.ApplicationException.Data.Contains("QueryEdgeGetFBMFactTypeFail") Then
                            Dim lrQueryEdge As FactEngine.QueryEdge
                            lrQueryEdge = lrRecordset.ApplicationException.Data.Item("QueryEdgeGetFBMFactTypeFail")

                            Dim larModelObject As New List(Of FBM.ModelObject)

                            If lrQueryEdge.BaseNode Is Nothing Or lrQueryEdge.TargetNode Is Nothing Then
                                Me.LabelError.Text.AppendDoubleLineBreak(lrRecordset.ApplicationException.StackTrace)
                                Exit Sub
                            End If

                            larModelObject.Add(lrQueryEdge.BaseNode.FBMModelObject)
                            larModelObject.Add(lrQueryEdge.TargetNode.FBMModelObject)

                            If prApplication.WorkingModel.getFactTypeByModelObjects(larModelObject).Count = 1 Then
                                Dim lsSentence As String = lrQueryEdge.BaseNode.Name & " " & lrQueryEdge.Predicate & " " & lrQueryEdge.TargetNode.Name
                                Dim lrSentence As New Language.Sentence(lsSentence)
                                Dim lrPredicatePart As New Language.PredicatePart(lrQueryEdge.Predicate)
                                lrSentence.PredicatePart.Add(lrPredicatePart)
                                lrSentence.addResolvedNounsByFBMModelObjectList(larModelObject)
                                Call Me.loadVirtualAnalyst()
                                'Call prApplication.Brain.processFactTypeReadingStatement
                                Call prApplication.Brain.send_data("There is only one Fact Type between model elements, " & larModelObject(0).Id & " and " & larModelObject(1).Id & ".")
                                Call prApplication.Brain.AskQuestionCreateFactTypeReading(lrSentence)
                                Call prApplication.Brain.invokeTimeoutStart()
                            End If
                        End If

                    End If
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
                                            'Dim lrBaseTupleNode = larTupleNode.Find(Function(x) x.Type = lrActualGraphNode.Column(0).QueryEdge.BaseNode.Name And
                                            '                                                x.Alias = lrActualGraphNode.Column(0).QueryEdge.BaseNode.Alias)
                                            Dim lrBaseTupleNode = larTupleNode.Find(Function(x) x.Type = lrTupleNode.Column(0).QueryEdge.BaseNode.Name And
                                                                                                x.Alias = lrTupleNode.Column(0).QueryEdge.BaseNode.Alias)

                                            If lrBaseTupleNode IsNot Nothing Then
                                                Dim lrBaseGraphNode = Me.GraphNodes.Find(Function(x) x.Type = lrBaseTupleNode.Type And
                                                                                                     x.Name = lrBaseTupleNode.Name)

                                                Dim lrEdge As New FactEngine.DisplayGraph.Edge(lrBaseGraphNode, lrActualGraphNode)
                                                lrEdge.Predicate = lrTupleNode.Column(0).QueryEdge.Predicate

                                                If lrBaseGraphNode Is Nothing Then
                                                    Throw New Exception("frmFactEngine.GO: BaseGraphNode is Nothing.")
                                                End If

                                                Dim larDuplicateEdge = From Edge In lrBaseGraphNode.Edge
                                                                       Where Edge.BaseNode Is lrEdge.BaseNode
                                                                       Where Edge.TargetNode Is lrEdge.TargetNode
                                                                       Where Edge.QueryEdge Is lrEdge.QueryEdge
                                                                       Select Edge

                                                If larDuplicateEdge.Count = 0 Then
                                                    lrBaseGraphNode.Edge.Add(lrEdge)
                                                End If
                                            Else
                                                'Throw New Exception("Error duplicating edge. Graph View. FactEngine.GO.")
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
                                    lrPGSLink.Pen.Width = 0.2
                                    'lrPGSLink.Text = Me.SentData(0)
                                    lrPGSLink.HeadPen.Color = Drawing.Color.DeepSkyBlue
                                    lrPGSLink.AutoRoute = False
                                    lrPGSLink.Locked = True
                                    lrPGSLink.HeadShapeSize = 3

                                    Try
                                        lrPGSLink.Text = lrEdge.Predicate 'argetNode.Column(0).QueryEdge.Predicate
                                    Catch ex As Exception
                                    End Try

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

            Call Me.hideAutoComplete()

            If Me.ToolStripMenuItemDefaultToResultsTab.Checked Then
                Me.TabControl1.SelectedTab = Me.TabPageResults
            ElseIf Me.ToolStripMenuItemDefaultToQueryTab.Checked Then
                Me.TabControl1.SelectedTab = Me.TabPageQuery
            Else
                Me.TabControl1.SelectedTab = Me.TabPageResults
            End If

            Me.zbTextBoxNaturalLanguageFocused = False
            Me.TextBoxInput.Focus()
            Me.TextBoxInput.Text &= " "
            Me.zrTextHighlighter.HighlightText()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function loadVirtualAnalyst() As frmToolboxBrainBox
        prApplication.WorkingPage = Nothing
        frmMain.Cursor = Cursors.WaitCursor
        Dim lrFrmToolboxBrainBox As frmToolboxBrainBox
        lrFrmToolboxBrainBox = frmMain.loadToolboxRichmondBrainBox(Nothing, Me.DockPanel.ActivePane)
        frmMain.Cursor = Cursors.Default
        Me.TextBoxInput.Focus()

        Return lrFrmToolboxBrainBox
    End Function

    Private Sub Application_WorkingModelChanged() Handles Application.WorkingModelChanged

        Try
            Call Me.displayModelName()

            Me.mrModel = prApplication.WorkingModel

            Me.FEQLProcessor = New FEQL.Processor(prApplication.WorkingModel)

            Me.LabelError.Text = ""

            If prApplication.WorkingModel.RequiresConnectionString And prApplication.WorkingModel.TargetDatabaseConnectionString = "" Then
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                Case Is = Keys.Space, Keys.Escape, Keys.Down, Keys.Up, Keys.Shift, Keys.ShiftKey, Keys.D9
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
                Case Is = Keys.Space
                    Exit Sub
                Case Else
                    Call Me.ProcessAutoComplete(e)
            End Select

            Select Case e.KeyCode
                Case Is = Keys.F5
                    Call Me.hideAutoComplete()
                    Call Me.GO(Me.TextBoxNaturalLanguage.Focused)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                Case Is = Keys.Down, Keys.Up
                Case Is = Keys.Space '20221108-VM-Was not calling ProcessAutoComplete which was probably fine.
                    Call Me.ProcessAutoComplete(New KeyEventArgs(e.KeyCode))
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
                        Case Is = Keys.Back
                            'Do nothing 
                        Case Else
                            'Call Me.ProcessAutoComplete(New KeyEventArgs(e.KeyCode))
                    End Select
            End Select

            Call Me.setAutoCompletePosition()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            Dim lsLastKeyDown = KeyCodeToUnicode(e.KeyCode)
            If Not e.KeyCode = Keys.Back Then
                Me.zrTextHighlighter.Tree = Me.zrParser.Parse(Trim(Me.TextBoxInput.Text & lsLastKeyDown))
            End If

            '=================================================================
            'Check valid ModelElement Names
#Region "Text Marker"
            Dim lrParseNode As FEQL.ParseNode
            Dim larModelElementNameParseNode As New List(Of FEQL.ParseNode)
            Dim larModelPredicateClauseParseNode As New List(Of FEQL.ParseNode)
            Dim larParseNode As New List(Of FEQL.ParseNode)
            Dim lrModelElement As FBM.ModelObject

            If Me.TextBoxInput.Text.Length > 5 Then 'was 10            
                Me.TextMarker.Clear()

                Call Me.GetMODELELEMENTParseNodes(Me.zrTextHighlighter.Tree.Nodes(0), larModelElementNameParseNode)

                Dim larModelMatch As New List(Of ModelMatch)
                Dim larMissingModelElement = From ParseNode In larModelElementNameParseNode
                                             Where prApplication.WorkingModel.GetModelObjectByName(Trim(ParseNode.Token.Text)) Is Nothing
                                             Select ParseNode

                For Each lrParseNode In larMissingModelElement ' ModelElementNameParseNode

                    'CodeSafe - An opportunity to help heal a Model if it is broken.
                    '  Calling GetModelObjectByName in SafeMode self heals the ModelDictionary if the ModelElement is found.
                    prApplication.WorkingModel.GetModelObjectByName(Trim(lrParseNode.Token.Text), True,, True)

                    Me.TextMarker.AddWord(lrParseNode.Token.StartPos, lrParseNode.Token.Length, Color.Red, "Uknown Model Element")

                    For Each lrModel In prApplication.getModelsByModelElementName(lrParseNode.Token.Text)
                        Dim lrModelMatch = New ModelMatch(lrModel, lrParseNode.Token.Text)
                        If larModelMatch.Contains(lrModelMatch) Then
                            larModelMatch.Find(Function(x) x.Model Is lrModelMatch.Model).addKeyWord(lrParseNode.Token.Text)
                        Else
                            larModelMatch.Add(lrModelMatch)
                        End If
                    Next
                    'End If
                Next

                larParseNode = New List(Of FEQL.ParseNode)
                Call Me.GetPredicateClauseNodes(Me.zrTextHighlighter.Tree.Nodes(0), larModelPredicateClauseParseNode)
                Dim lsPredicatePartText As String

                Dim larPredicateNode As List(Of FEQL.ParseNode)
                Dim lbMadeChanges As Boolean = False

                For Each lrParseNode In larModelPredicateClauseParseNode
                    larPredicateNode = New List(Of FEQL.ParseNode)
                    Call Me.GetPredicateNodes(lrParseNode, larPredicateNode)
                    Dim lasPredicate = (From PredicateNode In larPredicateNode
                                        Select Trim(PredicateNode.Token.Text)).ToArray

                    lsPredicatePartText = Trim(Strings.Join(lasPredicate, " "))

                    '========================================                    
                    'Good FactEngine styling
                    Dim lasToken As String() = {"A", "THAT", "AND", "WHICH", "AN", "WHAT"}

                    If Me.ToolStripMenuItemAutoCapitalise.Checked Then
                        For Each lsToken In lasToken
                            If lsPredicatePartText.EndsWith(" " & LCase(lsToken)) Then
                                lsPredicatePartText &= " "
                            End If
                            If lsPredicatePartText = LCase(lsToken) Then
                                lsPredicatePartText = lsToken
                                lbMadeChanges = True
                            ElseIf lsPredicatePartText.Contains(" " & LCase(lsToken) & " ") Then
                                lsPredicatePartText = lsPredicatePartText.Replace(" " & LCase(lsToken) & " ", " " & lsToken & " ")
                                lbMadeChanges = True
                            End If
                            If lbMadeChanges Then
                                Dim lsText As String = Me.TextBoxInput.Text.Remove(lrParseNode.Token.StartPos, lrParseNode.Token.Length)
                                Me.TextBoxInput.Text = lsText.Insert(lrParseNode.Token.StartPos, (" " & lsPredicatePartText).Replace("  ", " "))
                                Me.TextBoxInput.Text = Me.TextBoxInput.Text.Replace("  ", " ")
                                Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                                Exit Sub
                            End If
                        Next
                    End If

                    Dim lastWord As String = Me.TextBoxInput.Text.Split(" ").ToList.FindLast(Function(x) x.Length > 0)
                    If e.KeyCode = Keys.Space And Me.zrTextHighlighter.Tree.Optionals.Find(Function(x) x.ExpectedToken = FEQL.TokenType.MODELELEMENTNAME.ToString) IsNot Nothing Then
                        If prApplication.WorkingModel.GetModelObjectByName(Viev.Strings.MakeCapCamelCase(lastWord)) IsNot Nothing Then
                            Me.TextBoxInput.Text = Trim(Me.TextBoxInput.Text).Remove(Trim(Me.TextBoxInput.Text).Length - lastWord.Length)
                            Me.TextBoxInput.Text &= Viev.Strings.MakeCapCamelCase(lastWord)
                            Me.TextBoxInput.SelectionStart = Me.TextBoxInput.Text.Length
                        End If
                    End If

                    If Not prApplication.WorkingModel.existsPredicatePart(lsPredicatePartText) Then
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
#End Region

#Region "Main"
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

            Dim laiExpectedToken As New List(Of FEQL.TokenType)
            If Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
                If Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken <> "" Then
                    laiExpectedToken.Add(DirectCast([Enum].Parse(GetType(FEQL.TokenType), Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken), FEQL.TokenType))
                End If
            End If
            For Each lrParseError In Me.zrTextHighlighter.Tree.Optionals
                laiExpectedToken.Add(DirectCast([Enum].Parse(GetType(FEQL.TokenType), lrParseError.ExpectedToken), FEQL.TokenType))
            Next

            Dim lrLastModelElementNameParseNode As FEQL.ParseNode

            '============================================================================================
            'Do ultrasmart checking. Finds the last ModelElementName and the last PredicateClause
            '  and if a FactTypeReading is being attempted to be made, finds the next ModelElementName
            If larModelElementNameParseNode.Count > 0 Or larModelPredicateClauseParseNode.Count > 0 Then

                Dim lrFirstModelElementNameParseNode = larModelElementNameParseNode(0)
                lrLastModelElementNameParseNode = larModelElementNameParseNode(larModelElementNameParseNode.Count - 1)
                'Dim lrLastPredicateClauseParseNode = larModelPredicateClauseParseNode(larModelPredicateClauseParseNode.Count - 1)

                Dim lrFirstModelElement = prApplication.WorkingModel.GetModelObjectByName(lrFirstModelElementNameParseNode.Token.Text)
                Dim lrlastModelElement = prApplication.WorkingModel.GetModelObjectByName(lrLastModelElementNameParseNode.Token.Text)

                lrModelElement = prApplication.WorkingModel.GetModelObjectByName(Trim(lrLastModelElementNameParseNode.Token.Text))

                Dim liCurrentContext As FEQL.TokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type

                If lrModelElement Is Nothing Then
                    'Nothing to do here
                Else
                    Select Case Me.FEQLProcessor.getWhichStatementType(Trim(Me.TextBoxInput.Text), True)
                        Case Is = FactEngine.pcenumFEQLStatementType.WHICHSELECTStatement,
                                  FactEngine.pcenumFEQLStatementType.DIDStatement
                            Dim lrWHICHSELECTStatement As New FEQL.WHICHSELECTStatement
                            Call Me.FEQLProcessor.GetParseTreeTokensReflection(lrWHICHSELECTStatement, Me.zrTextHighlighter.Tree.Nodes(0))

                            'Set intellisense to predicate if at Predicate token.
                            If Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.PREDICATE Then

                                Dim lrPredicateClauseParseNode = larModelPredicateClauseParseNode.Last
                                Dim larPredicateNode = New List(Of FEQL.ParseNode)
                                Call Me.GetPredicateNodes(lrPredicateClauseParseNode, larPredicateNode)
                                Dim lasPredicate = (From PredicateNode In larPredicateNode
                                                    Select Trim(PredicateNode.Token.Text)).ToArray

                                Me.zsIntellisenseBuffer = Trim(Strings.Join(lasPredicate, " "))
                            End If

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
                                            Call Me.AddEnterpriseAwareItem(lrFactTypeReading.GetPredicateText, FEQL.TokenType.PREDICATE,,, True)
                                        End If
                                    Next

                                    Dim larFactTypeReading2 = lrModelElement.getPartialFactTypeReadings()

                                    Dim larPredicatePart = From FactTypeReading In larFactTypeReading2
                                                           From PredicatePart In FactTypeReading.PredicatePart
                                                           Where PredicatePart.Role.JoinedORMObject.Id = lrModelElement.Id
                                                           Where PredicatePart.SequenceNr < FactTypeReading.PredicatePart.Count
                                                           Select PredicatePart

                                    For Each lrPredicatePart In larPredicatePart
                                        Dim lrNextPredicatePart = lrPredicatePart.FactTypeReading.PredicatePart(lrPredicatePart.SequenceNr)
                                        Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE, False, lrNextPredicatePart.Role.JoinedORMObject.Id, False)
                                    Next

                                    If Me.AutoComplete.Visible = False Then
                                        Me.showAutoCompleteForm()
                                    End If
                                End If

                            Else
                                Dim loLastWhichClauseObject = lrWHICHSELECTStatement.WHICHCLAUSE(lrWHICHSELECTStatement.WHICHCLAUSE.Count - 1)
                                Dim lrLastWhichClause As New FEQL.WHICHCLAUSE
                                Call Me.FEQLProcessor.GetParseTreeTokensReflection(lrLastWhichClause, loLastWhichClauseObject)

                                Dim larPredicatePart As IEnumerable(Of FBM.PredicatePart)

                                Dim lbUseSecondLastWhichClause As Boolean = False
                                If lrLastWhichClause.MODELELEMENTNAME.Count = 0 And lrLastWhichClause.MODELELEMENT.Count = 0 Then
                                    If lrWHICHSELECTStatement.WHICHCLAUSE.Count - 2 >= 0 Then
                                        loLastWhichClauseObject = lrWHICHSELECTStatement.WHICHCLAUSE(lrWHICHSELECTStatement.WHICHCLAUSE.Count - 2)
                                        lrLastWhichClause = New FEQL.WHICHCLAUSE
                                        Call Me.FEQLProcessor.GetParseTreeTokensReflection(lrLastWhichClause, loLastWhichClauseObject)
                                        lbUseSecondLastWhichClause = True
                                    End If
                                Else
                                    '==========================================================
                                    'ModelElement 
                                    Dim lsPredicateText As String = ""
                                    For Each lsPredicatePart In lrLastWhichClause.PREDICATE
                                        lsPredicateText = Trim(lsPredicateText & " " & Trim(lsPredicatePart))
                                    Next

                                    larPredicatePart = From FactType In prApplication.WorkingModel.FactType
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
                                End If

                                'Predicates
                                Dim lrPredicateModelObject As FBM.ModelObject
                                If lrLastWhichClause.KEYWDAND IsNot Nothing And lrLastWhichClause.KEYWDTHAT.Count = 1 Then
                                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(Trim(lrLastModelElementNameParseNode.Token.Text))
                                End If
                                If lrLastWhichClause.KEYWDWHICH IsNot Nothing Then
                                    lrPredicateModelObject = lrlastModelElement
                                ElseIf lrLastWhichClause.KEYWDTHAT.Count > 0 Then
                                    lrPredicateModelObject = lrlastModelElement
                                Else
                                    lrPredicateModelObject = lrFirstModelElement
                                End If
                                If lbUseSecondLastWhichClause Then
                                    lrPredicateModelObject = lrlastModelElement
                                End If

                                Dim larFactTypeReading2 As List(Of FBM.FactTypeReading)

                                If liCurrentContext = FEQL.TokenType.KEYWDTHAT Then
                                    larFactTypeReading2 = lrModelElement.getOutgoingFactTypeReadings
                                Else
                                    larFactTypeReading2 = lrModelElement.getPartialFactTypeReadings()
                                    Dim larFactTypeReading = lrModelElement.getOutgoingFactTypeReadings(2)

                                    If Me.zsIntellisenseBuffer.Length > 0 Then
                                        larFactTypeReading = larFactTypeReading.FindAll(Function(x) x.PredicatePart(0).PredicatePartText.StartsWith(Me.zsIntellisenseBuffer))
                                        For Each lrFactTypeReading In larFactTypeReading
                                            larFactTypeReading2.AddUnique(lrFactTypeReading)
                                        Next
                                    End If

                                End If

                                larPredicatePart = From FactTypeReading In larFactTypeReading2
                                                   From PredicatePart In FactTypeReading.PredicatePart
                                                   Where PredicatePart.Role.JoinedORMObject.Id = lrModelElement.Id
                                                   Where (PredicatePart.SequenceNr < FactTypeReading.PredicatePart.Count) Or FactTypeReading.PredicatePart.Count = 1
                                                   Select PredicatePart

                                For Each lrPredicatePart In larPredicatePart
                                    If lrPredicatePart.FactTypeReading.PredicatePart.Count = 1 Then
                                        Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE, False, , True)
                                    Else
                                        Dim lrNextPredicatePart = lrPredicatePart.FactTypeReading.PredicatePart(lrPredicatePart.SequenceNr)
                                        Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE, False, lrNextPredicatePart.Role.JoinedORMObject.Id, True)
                                    End If
                                Next

                                Dim larPredicatePartTemp = larPredicatePart.ToList

                                If Not liCurrentContext = FEQL.TokenType.KEYWDTHAT Then
                                    larFactTypeReading2 = lrFirstModelElement.getOutgoingFactTypeReadings()

                                    larPredicatePart = From FactTypeReading In larFactTypeReading2
                                                       From PredicatePart In FactTypeReading.PredicatePart
                                                       Where PredicatePart.Role.JoinedORMObject.Id = lrFirstModelElement.Id
                                                       Where PredicatePart.SequenceNr < FactTypeReading.PredicatePart.Count
                                                       Where Not larPredicatePartTemp.Contains(PredicatePart)
                                                       Select PredicatePart

                                    For Each lrPredicatePart In larPredicatePart
                                        Dim lrNextPredicatePart = lrPredicatePart.FactTypeReading.PredicatePart(lrPredicatePart.SequenceNr)
                                        If lrPredicatePart.PredicatePartText.StartsWith(Me.zsIntellisenseBuffer) Then
                                            Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE, False, lrNextPredicatePart.Role.JoinedORMObject.Id, True, True)
                                        Else
                                            Call Me.AddEnterpriseAwareItem(lrPredicatePart.PredicatePartText, FEQL.TokenType.PREDICATE, False, lrNextPredicatePart.Role.JoinedORMObject.Id, False)
                                        End If

                                    Next
                                End If

                                If lrPredicateModelObject IsNot Nothing Then

                                    Dim larFactTypeReading As New List(Of FBM.FactTypeReading)

                                    larFactTypeReading.AddRange(lrPredicateModelObject.getOutgoingFactTypeReadings(2))

                                    If Me.zsIntellisenseBuffer.Length > 0 Then
                                        larFactTypeReading.RemoveAll(Function(x) Not x.PredicatePart(0).PredicatePartText.StartsWith(Me.zsIntellisenseBuffer) Or larPredicatePartTemp.Contains(x.PredicatePart(0)))
                                    ElseIf lrLastWhichClause.PREDICATE.Count > 0 Then
                                        larFactTypeReading = larFactTypeReading.FindAll(Function(x) x.PredicatePart(0).PredicatePartText.StartsWith(lrLastWhichClause.PREDICATE(0)))
                                    End If

                                    For Each lrFactTypeReading In larFactTypeReading.OrderBy(Function(x) x.GetPredicateText)

                                        Dim lsPreboundReadingText = ""
                                        'For Each lrFactTypeReading In lrPredicateModelObject.getOutgoingFactTypeReadings(2)
                                        If lrFactTypeReading.PredicatePart.Count > 1 Then
                                            If lrFactTypeReading.PredicatePart(1).PreBoundText <> "" Then
                                                lsPreboundReadingText = lrFactTypeReading.PredicatePart(1).PreBoundText
                                            End If
                                            Call Me.AddEnterpriseAwareItem(lrFactTypeReading.GetPredicateText, FEQL.TokenType.PREDICATE, , lsPreboundReadingText & lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id, True)
                                        Else
                                            Call Me.AddEnterpriseAwareItem(lrFactTypeReading.GetPredicateText, FEQL.TokenType.PREDICATE)
                                        End If
                                    Next

                                    If Me.AutoComplete.Visible = False Then
                                        Me.showAutoCompleteForm()
                                    End If
                                    'Exit Sub '20210901-VM-Something funky happens after here...might be removing what was put in above.
                                End If

                            End If

                    End Select


                    'Dim laiExpectedToken As New List(Of FEQL.TokenType)
                    'If Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
                    '    If Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken <> "" Then
                    '        laiExpectedToken.Add(DirectCast([Enum].Parse(GetType(FEQL.TokenType), Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken), FEQL.TokenType))
                    '    End If

                    '    For Each lrParseError In Me.zrTextHighlighter.Tree.Optionals
                    '        laiExpectedToken.Add(DirectCast([Enum].Parse(GetType(FEQL.TokenType), lrParseError.ExpectedToken), FEQL.TokenType))
                    '    Next
                    'End If

                    '=========================================================================
                    'If (Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.IDENTIFIER) Or
                    '    laiExpectedToken.Contains(FEQL.TokenType.IDENTIFIER) Then

                    '    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrLastModelElementNameParseNode.Token.Text)

                    '    If lrModelElement.ConceptType = pcenumConceptType.ValueType Then
                    '        If CType(lrModelElement, FBM.ValueType).ValueConstraint.Count > 0 Then
                    '            For Each lsValue In CType(lrModelElement, FBM.ValueType).ValueConstraint
                    '                Call Me.AddEnterpriseAwareItem(lsValue,,,, True)
                    '            Next
                    '        End If
                    '    End If
                    '    Dim lsSQLQuery = "SELECT "
                    '    Dim liInd = 0
                    '    For Each lrColumn In lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                    '        If liInd > 0 Then lsSQLQuery &= " ,"
                    '        lsSQLQuery &= lrColumn.Name
                    '        liInd += 1
                    '    Next
                    '    lsSQLQuery &= vbCrLf & "FROM " & lrModelElement.Id
                    '    If Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.IDENTIFIER Then
                    '        Try
                    '            If lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.Count > 0 Then
                    '                Dim lsDatabaseWildcardOperator = Database.gerLikeWildcardOperator(prApplication.WorkingModel.TargetDatabaseType)
                    '                lsSQLQuery &= vbCrLf & "WHERE " & lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns(0).Name & " LIKE '" & Me.zrTextHighlighter.GetCurrentContext.Token.Text & lsDatabaseWildcardOperator & "'"
                    '            End If
                    '        Catch ex As Exception
                    '            'Do nothing. Just don't add anything to the SQL.
                    '        End Try
                    '    End If
                    '    lsSQLQuery &= vbCrLf & "LIMIT 20"
                    '    Dim lrRecordset As ORMQL.Recordset
                    '    Try
                    '        lrRecordset = Me.FEQLProcessor.DatabaseManager.GO(lsSQLQuery)

                    '        For Each lrFact In lrRecordset.Facts
                    '            Dim lsString As String = ""
                    '            liInd = 0
                    '            For Each lrData In lrFact.Data
                    '                If liInd > 0 Then lsString &= ","
                    '                lsString &= lrData.Data
                    '            Next
                    '            Call Me.AddEnterpriseAwareItem(lsString)
                    '        Next
                    '    Catch ex As Exception
                    '        Me.LabelError.Text = ex.Message
                    '    End Try

                    '    Call Me.showAutoCompleteForm()
                    '    Exit Sub
                    'End If
                    '=========================================================================

                End If

            End If
#End Region

#Region "Identifier"
            If larModelElementNameParseNode.Count > 0 Then

                lrLastModelElementNameParseNode = larModelElementNameParseNode(larModelElementNameParseNode.Count - 1)

                If (Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.IDENTIFIER) Or
                        laiExpectedToken.Contains(FEQL.TokenType.IDENTIFIER) Then

                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lrLastModelElementNameParseNode.Token.Text)

                    If lrModelElement.ConceptType = pcenumConceptType.ValueType Then

                        If CType(lrModelElement, FBM.ValueType).DataType = pcenumORMDataType.TemporalDate Then
                            Me.AutoComplete.ListBox.Items.Clear()
                            Call Me.showAutoCompleteForm()
                            Me.AutoComplete.DateTimePicker.BringToFront()
                            Me.AutoComplete.DateTimePicker.Visible = True
                            'Exit Sub
                        End If

                        If CType(lrModelElement, FBM.ValueType).ValueConstraint.Count > 0 Then
                            For Each lsValue In CType(lrModelElement, FBM.ValueType).ValueConstraint
                                Call Me.AddEnterpriseAwareItem(lsValue,,,, True)
                            Next
                        End If

                        Try
                            If CType(lrModelElement, FBM.ValueType).DataType = pcenumORMDataType.TemporalDateAndTime And
                                (Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.IDENTIFIER) Then
                                'DateTime
                                Dim lsUserDateTime = Me.zrTextHighlighter.GetCurrentContext.Token.Text
                                Dim loDateTime As DateTime = Nothing
                                If Not DateTime.TryParse(lsUserDateTime, loDateTime) Then
                                    Me.ToolStripStatusLabelError.Text = "Hint: " & lsUserDateTime & " is not a valid DateTime. The configured FactEngine DateTime format is: " & My.Settings.FactEngineUserDateTimeFormat
                                Else
                                    Me.ToolStripStatusLabelError.Text = "Hint: Expecting DateTime value in format: " & My.Settings.FactEngineUserDateTimeFormat
                                End If
                            ElseIf CType(lrModelElement, FBM.ValueType).DataType = pcenumORMDataType.TemporalDate And
                                (Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.IDENTIFIER) Then
                                'Date
                                Dim lsUserDateTime = Me.zrTextHighlighter.GetCurrentContext.Token.Text
                                Dim loDateTime As DateTime = Nothing
                                If Not DateTime.TryParse(lsUserDateTime, loDateTime) Then
                                    Me.ToolStripStatusLabelError.Text = "Hint: " & lsUserDateTime & " is not a valid Date. The configured FactEngine Date format is: " & My.Settings.FactEngineUserDateTimeFormat
                                Else
                                    Me.ToolStripStatusLabelError.Text = "Hint: Expecting DateTime value in format: " & My.Settings.FactEngineUserDateFormat
                                End If
                            Else
                                Me.ToolStripStatusLabelError.Text = ""
                            End If
                        Catch ex As Exception
                            'Not a biggie. Don't abort.
                        End Try


                    End If

                    Dim lsSQLQuery = "SELECT DISTINCT "
                    'FEQL Query
                    Dim lsFEQLQuery As String = "WHICH "

                    Dim liInd As Integer
                    If lrModelElement.ConceptType = pcenumConceptType.ValueType Then
                        'Try and find the FactType that belongs to the PreviousModelElement/PredicateClause/ValueType

                        If larModelElementNameParseNode.Count > 1 Then
                            Try
                                Dim lrWhichSelectStatement As New FEQL.WHICHSELECTStatement
                                Call Me.FEQLProcessor.GetParseTreeTokensReflection(lrWhichSelectStatement, Me.zrTextHighlighter.Tree.Nodes(0))
                                Dim lrQueryGraph As FactEngine.QueryGraph = Me.FEQLProcessor.getQueryGraph(lrWhichSelectStatement)
                                Dim lrQueryEdge As FactEngine.QueryEdge = Nothing
                                If lrQueryGraph.QueryEdges.Count > 0 Then
                                    lrQueryEdge = lrQueryGraph.QueryEdges.Last
                                Else
                                    'Dim lrQueryEdge As New FactEngine.QueryEdge(New FactEngine.QueryGraph(prApplication.WorkingModel), Nothing)
                                    'Dim lrBaseModelElement = prApplication.WorkingModel.GetModelObjectByName(Trim(larModelElementNameParseNode(larModelElementNameParseNode.Count - 2).Token.Text))
                                    'Dim lrBaseNode = New FactEngine.QueryNode(lrBaseModelElement, lrQueryEdge)
                                    'Dim lrTargetNode = New FactEngine.QueryNode(lrModelElement, lrQueryEdge, True)
                                    'lrQueryEdge.BaseNode = lrBaseNode
                                    'lrQueryEdge.TargetNode = lrTargetNode
                                    'Dim lrPredicateClauseNode = larModelPredicateClauseParseNode.Last
                                    'Dim larPredicateNode = New List(Of FEQL.ParseNode)
                                    'Call Me.GetPredicateNodes(lrPredicateClauseNode, larPredicateNode)
                                    'Dim lasPredicate = (From PredicateNode In larPredicateNode
                                    '                    Select Trim(PredicateNode.Token.Text)).ToArray
                                    'Dim lsPredicatePartText = Trim(Strings.Join(lasPredicate, " "))
                                    'lrQueryEdge.Predicate = lsPredicatePartText
                                    'Call lrQueryEdge.getAndSetFBMFactType(lrBaseNode, lrTargetNode, lsPredicatePartText,, True)
                                End If

                                Dim lrColumn As RDS.Column = lrQueryEdge.BaseNode.RDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                                If lrColumn IsNot Nothing Then
                                    lsSQLQuery &= lrColumn.Name
                                Else
                                    lsSQLQuery &= lrModelElement.Id
                                End If

                                If lrColumn Is Nothing Then
                                    Dim larColumn = From Table In Me.FEQLProcessor.Model.RDS.Table
                                                    From Column In Table.Column
                                                    Where Column.FactType Is lrQueryEdge.FBMFactType
                                                    Select Column

                                    If larColumn.Count > 0 Then
                                        lrColumn = larColumn.First
                                    End If
                                End If

                                If lrQueryEdge.IsPartialFactTypeMatch Then
                                    lsSQLQuery &= " FROM " & lrQueryEdge.FBMFactType.getCorrespondingRDSTable.DatabaseName
                                    lsFEQLQuery &= lrQueryEdge.FBMFactType.getCorrespondingRDSTable.Name
                                Else
                                    lsSQLQuery &= " FROM " & lrQueryEdge.BaseNode.RDSTable.DatabaseName
                                    lsFEQLQuery &= lrQueryEdge.BaseNode.RDSTable.Name
                                End If

                                If (Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.IDENTIFIER) Then 'Or laiExpectedToken.Contains(FEQL.TokenType.IDENTIFIER)
                                    Try
                                        Dim lsDatabaseWildcardOperator = Database.getLikeWildcardOperator(prApplication.WorkingModel.TargetDatabaseType)

                                        Dim lsFEQLQueryAddition As String = ""
                                        lsFEQLQuery &= " " & lrQueryEdge.Predicate
                                        If lrColumn IsNot Nothing Then
                                            lsSQLQuery &= vbCrLf & "WHERE " & lrColumn.Name
                                            lsFEQLQueryAddition &= lrColumn.ActiveRole.JoinedORMObject.Id
                                        Else
                                            lsSQLQuery &= vbCrLf & "WHERE " & lrModelElement.Id
                                            lsFEQLQueryAddition &= lrModelElement.Id
                                        End If

                                        Dim larTemporalDataTypes = {pcenumORMDataType.TemporalDate, pcenumORMDataType.TemporalDateAndTime}
                                        If larTemporalDataTypes.Contains(CType(lrModelElement, FBM.ValueType).DataType) Then
                                            lsSQLQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                        End If
                                        lsSQLQuery &= " LIKE '" & Me.zrTextHighlighter.GetCurrentContext.Token.Text & lsDatabaseWildcardOperator & "'"
                                        lsFEQLQuery &= " WHICH DISTINCT "
                                        lsFEQLQuery &= " (" & lsFEQLQueryAddition & "~'" & Me.zrTextHighlighter.GetCurrentContext.Token.Text & lsDatabaseWildcardOperator & "')"
                                        If lrColumn IsNot Nothing Then
                                            lsFEQLQuery &= " RETURN " & lrColumn.Table.Name & "." & lrColumn.Name
                                        End If
                                    Catch ex As Exception
                                        If lrColumn IsNot Nothing Then
                                            lsFEQLQuery &= " RETURN " & lrColumn.Table.Name & "." & lrColumn.Name
                                        End If
                                        'Do nothing. Just don't add anything to the SQL.
                                    End Try
                                Else
                                    If lrColumn IsNot Nothing Then
                                        lsFEQLQuery &= " RETURN " & lrColumn.Table.Name & "." & lrColumn.Name
                                    End If
                                End If
                                lsSQLQuery &= vbCrLf & " LIMIT 20"
                            Catch ex As Exception
                                Me.LabelError.Text = ex.Message
                            End Try
                        End If
                    Else
                        liInd = 0

                        For Each lrColumn In lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                            If liInd > 0 Then lsSQLQuery &= " ,"
                            lsSQLQuery &= lrColumn.Name
                            liInd += 1
                        Next

                        lsSQLQuery &= vbCrLf & "FROM " & lrModelElement.getCorrespondingRDSTable.DatabaseName
                        lsFEQLQuery &= lrModelElement.getCorrespondingRDSTable.Name

                        If Me.zrTextHighlighter.GetCurrentContext.Token.Type = FEQL.TokenType.IDENTIFIER Then
                            Try
                                If lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.Count > 0 Then
                                    Dim lsCurrentTokenText As String = Me.zrTextHighlighter.GetCurrentContext.Token.Text
                                    Dim lsDatabaseWildcardOperator = Database.getLikeWildcardOperator(prApplication.WorkingModel.TargetDatabaseType)
                                    Dim lsCurrentToken As String = Trim(lsCurrentTokenText)
                                    lsSQLQuery &= vbCrLf & "WHERE " & lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns(0).Name & " LIKE '" & lsCurrentToken & lsDatabaseWildcardOperator & "'"

                                    For Each lrColumn In lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns

                                        Try
                                            Dim lrPredicatePart As FBM.PredicatePart = lrColumn.FactType.FactTypeReading.Find(Function(x) x.PredicatePart(0).Role Is lrColumn.Role).PredicatePart(0)
                                            lsFEQLQuery &= " " & lrPredicatePart.PredicatePartText & " (" & lrColumn.ActiveRole.JoinedORMObject.Id & "~'" & lsCurrentTokenText & lsDatabaseWildcardOperator & "')"
                                        Catch ex As Exception
                                            'Not a biggie
                                        End Try
                                    Next

                                End If
                            Catch ex As Exception
                                'Do nothing. Just don't add anything to the SQL.
                            End Try
                        End If

                        lsSQLQuery &= vbCrLf & "LIMIT 20"
                    End If
                    Try
                        Dim lrRecordset As ORMQL.Recordset
                        '===================================================================================
                        'FEQL
                        lrRecordset = Me.FEQLProcessor.ProcessFEQLStatement(lsFEQLQuery, Nothing, Nothing)

                        '===================================================================================
                        'SQL
                        'lrRecordset = Me.FEQLProcessor.DatabaseManager.GO(lsSQLQuery)

                        For Each lrFact In lrRecordset.Facts
                            Dim lsString As String = ""
                            liInd = 0
                            For Each lrData In lrFact.Data
                                If liInd > 0 Then lsString &= ","
                                lsString &= lrData.Data
                                liInd += 1
                            Next
                            Select Case lrModelElement.GetType
                                Case Is = GetType(FBM.ValueType)
                                    Select Case CType(lrModelElement, FBM.ValueType).DataType
                                        Case Is = pcenumORMDataType.TemporalDateAndTime
                                            lsString = String.Format("{0:" & My.Settings.FactEngineUserDateTimeFormat & "}", CDate(lsString))
                                    End Select
                            End Select

                            Call Me.AddEnterpriseAwareItem(lsString,,,, True)
                        Next
                    Catch ex As Exception
                        Me.LabelError.Text = ex.Message
                    End Try

                    Call Me.showAutoCompleteForm()
                    Exit Sub
                End If
            End If
#End Region


            '============================================================================================

            If Me.zrTextHighlighter.Tree.Errors.Count = 0 Then

                zsIntellisenseBuffer = Me.zrTextHighlighter.GetCurrentContext.Token.Text

                Select Case Me.zrTextHighlighter.GetCurrentContext.Token.Type
                    Case Is = FEQL.TokenType.MODELELEMENTNAME

#Region "Predicates"
                        Dim lsModelElementName = zsIntellisenseBuffer
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
#End Region

                        Call Me.PopulateEnterpriseAwareWithObjectTypes(False)
                End Select

            ElseIf (Me.zrTextHighlighter.Tree.Errors.Count > 0) Or (Me.zrTextHighlighter.Tree.Optionals.Count > 0) Then
                If Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
                    lsExpectedToken = Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken
                Else
                    lsExpectedToken = Me.zrTextHighlighter.Tree.Optionals(0).ExpectedToken
                End If
                If lsExpectedToken <> "" Then
                    liTokenType = DirectCast([Enum].Parse(GetType(FEQL.TokenType), lsExpectedToken), FEQL.TokenType)
                    'MsgBox("Expecting: " & Me.zrScanner.Patterns(liTokenType).ToString)
                End If

                Select Case liTokenType
                    Case Is = FEQL.TokenType.COLUMNNAMESTR
                        If larModelElementNameParseNode.Count > 0 Then
                            Dim lrTable As RDS.Table = prApplication.WorkingModel.RDS.Table.Find(Function(x) x.Name = larModelElementNameParseNode.Last.Token.Text)
                            For Each lrColumn In lrTable.Column
                                Me.AddEnterpriseAwareItem(lrColumn.Name)
                            Next
                        End If
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
                            Call Me.PopulateEnterpriseAwareWithObjectTypes(True)
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

                If Me.zrTextHighlighter.Tree.Optionals.Count > 0 Then
                    Call Me.PopulateEnterpriseAwareFromOptionals(Me.zrTextHighlighter.Tree.Optionals)
                End If

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
                        Case Is = FEQL.TokenType.RETURNCOLUMN
                            Me.AutoComplete.Enabled = True
                            Dim larModelElementParseNodes As New List(Of FEQL.ParseNode)
                            Call Me.GetMODELELEMENTParseNodes(Me.zrTextHighlighter.Tree.Nodes(0), larModelElementParseNodes)
                            For Each lrModelElementParseNode In larModelElementParseNodes
                                Call Me.AddEnterpriseAwareItem(lrModelElementParseNode.Token.Text, Nothing, False, lrModelElementParseNode.Token.Text, True)
                            Next
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
                            '20210909-VM-Removed
                            'Call Me.AddFactTypeReadingsToEnterpriseAware()
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
                Case Is = FEQL.TokenType.COLUMNNAMESTR
                    'Nothing to do here.
                    Dim lrModelElement As FBM.ModelObject
                    Dim lsModelElementName As String
                    lsModelElementName = Me.TextBoxInput.Text.Trim.Split(" ").Last
                    lrModelElement = prApplication.WorkingModel.GetModelObjectByName(lsModelElementName)
                    Dim lrTable As RDS.Table = lrModelElement.getCorrespondingRDSTable
                    For Each lrColumn In lrTable.Column
                        Me.AddEnterpriseAwareItem(lrColumn.Name)
                    Next
                Case Is = FEQL.TokenType.KEYWDCOUNTSTAR
                    Me.AddEnterpriseAwareItem("COUNT(*)", liTokenType)
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
                Case Is = FEQL.TokenType.STAR
                    Me.AddEnterpriseAwareItem("*", liTokenType)
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

    Private Sub PopulateEnterpriseAwareWithObjectTypes(Optional ByVal abUse0Index As Boolean = False)

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


        If zsIntellisenseBuffer.Length > 0 Then
            For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False And
                                                                                   LCase(x.Name).StartsWith(LCase(zsIntellisenseBuffer), True, System.Globalization.CultureInfo.CurrentUICulture))
                Call Me.AddEnterpriseAwareItem(lrEntityType.Name, FEQL.TokenType.MODELELEMENTNAME,,, abUse0Index)
            Next
        Else
            For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
                Call Me.AddEnterpriseAwareItem(lrEntityType.Name, FEQL.TokenType.MODELELEMENTNAME)
            Next
        End If



        If zsIntellisenseBuffer.Length > 0 Then
            For Each lrValueType In prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False And
                                                                                 LCase(x.Name).StartsWith(LCase(zsIntellisenseBuffer), True, System.Globalization.CultureInfo.CurrentUICulture))
                Call Me.AddEnterpriseAwareItem(lrValueType.Name, FEQL.TokenType.MODELELEMENTNAME,,, abUse0Index)
            Next
        Else
            For Each lrValueType In prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
                Call Me.AddEnterpriseAwareItem(lrValueType.Name, FEQL.TokenType.MODELELEMENTNAME)
            Next
        End If



        If zsIntellisenseBuffer.Length > 0 Then
            For Each lrFactType In prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsObjectified And x.IsMDAModelElement = False And
                                                                               LCase(x.Name).StartsWith(LCase(zsIntellisenseBuffer), True, System.Globalization.CultureInfo.CurrentUICulture))
                Call Me.AddEnterpriseAwareItem(lrFactType.Name, FEQL.TokenType.MODELELEMENTNAME,,, abUse0Index)
            Next
        Else
            For Each lrFactType In prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsObjectified And x.IsMDAModelElement = False)
                Call Me.AddEnterpriseAwareItem(lrFactType.Name, FEQL.TokenType.MODELELEMENTNAME)
            Next
        End If


    End Sub

    Private Sub frmFactEngine_Leave(sender As Object, e As EventArgs) Handles Me.Leave
        Call Me.hideAutoComplete()
    End Sub

    Private Sub showAutoCompleteForm()

        Me.AutoComplete.DateTimePicker.Visible = False
        Me.AutoComplete.DateTimePicker.SendToBack()
        Me.AutoComplete.ListBox.BringToFront()

        If Me.AutoComplete.ListBox.Items.Count > 0 Then
            Me.AutoComplete.ListBox.SelectedIndex = 0
            Me.AutoComplete.ListBox.SelectedIndex = -1
            Me.AutoComplete.zsIntellisenseBuffer = Me.zsIntellisenseBuffer
            Me.AutoComplete.zrCallingForm = Me
            If Me.AutoComplete.Visible = False Then
                Me.AutoComplete.Visible = True
                'Me.TextBoxInput.Focus() 'Originally was here. 20210416
            End If
            Me.TextBoxInput.Focus()
            Me.AutoComplete.Owner = Me
            Call Me.populateHelpLabel()
        End If

    End Sub

    Private Sub setAutoCompletePosition()
        Dim lo_point As New Point(Me.TextBoxInput.GetPositionFromCharIndex(Me.TextBoxInput.SelectionStart))
        lo_point.X += Me.TextBoxInput.Bounds.X
        lo_point.Y += Me.TextBoxInput.Bounds.Y
        lo_point.Y += CInt(Me.TextBoxInput.Font.GetHeight()) + 6
        Me.AutoComplete.Location = PointToScreen(lo_point)
        Me.TextBoxInput.Focus()
    End Sub

    Private Sub SetupForm()

        Try
            Me.ToolStripMenuItemAutoCapitalise.Checked = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
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
        'Me.LabelHelp.Text = ""
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
            With New WaitCursor
                'Clear the Graph View because there is not enough information to create a graph.
                Call Me.clearGraphView()

                'If prApplication.WorkingModel.TargetDatabaseConnectionString = "" Then
                '    Me.LabelError.ForeColor = Color.Orange
                '    Me.LabelError.Text = "The Model needs a database connection string."
                '    Exit Sub
                'End

                If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
                    Call prApplication.WorkingModel.connectToDatabase()
                End If

                Try
                    If Not prApplication.WorkingModel.DatabaseConnection.Connected Or prApplication.WorkingModel.DatabaseManager.Connection Is Nothing Then
                        prApplication.WorkingModel.connectToDatabase()
                    End If
                Catch ex As Exception
                    prApplication.ThrowErrorMessage("Oops. Check the database conection configuration for the Model you are trying to connect to.", pcenumErrorType.Warning,, False,, True,, False)
                    Exit Sub
                End Try

                Me.FEQLProcessor.DatabaseManager = prApplication.WorkingModel.DatabaseManager

                Me.LabelError.Text = ""

                'Get the Query from the SQL/Cypher/etc query textbox
                Dim lsQuery = Me.TextBoxQuery.Text '.Replace(vbLf, " ") 'Leave this here.

                'If the user has highlighted a section of a query to execute, execute that.
                If Me.TextBoxQuery.SelectionLength > 0 Then
                    lsQuery = Me.TextBoxQuery.SelectedText
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
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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

    Private Sub ClearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearToolStripMenuItem.Click

        Try
            Me.Diagram.Links.Clear()
            Me.Diagram.Nodes.Clear()
            Me.Diagram.Invalidate()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub TextBoxQuery_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxQuery.KeyDown

        Try

            If e.KeyCode = Keys.F5 Then

                Call Me.TextBoxQueryGO()

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxQueryGO()

        Try
            With New WaitCursor
                Dim lrRecordset As ORMQL.Recordset

                'Clear the Graph View because there is not enough information to create a graph.
                Call Me.clearGraphView()

                If prApplication.WorkingModel.RequiresConnectionString And prApplication.WorkingModel.TargetDatabaseConnectionString = "" Then
                    Me.LabelError.ForeColor = Color.Orange
                    Me.LabelError.Text = "The Model needs a database connection string."
                    Exit Sub
                End If

                Me.LabelError.Text = ""

                'Get the Query from the SQL/Cypher/etc query textbox
                Dim lsQuery = Me.TextBoxQuery.Text '.Replace(vbLf, " ") 'Leave this here.

                'If the user has highlighted a section of a query to execute, execute that.
                If Me.TextBoxQuery.SelectionLength > 0 Then
                    lsQuery = Me.TextBoxQuery.SelectedText
                End If

                lrRecordset = Me.FEQLProcessor.DatabaseManager.GO(lsQuery)


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

            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker.ProgressChanged

        Try

            Dim lrProgressObject As ProgressObject = CType(e.UserState, ProgressObject)

            If lrProgressObject.Message IsNot Nothing Then
                If lrProgressObject.SimpleAppend Then
                    Me.LabelHelp.Text = lrProgressObject.Message

                ElseIf lrProgressObject.IsError Then
                    Me.LabelHelp.Text = lrProgressObject.Message
                Else
                    Me.LabelHelp.Text = lrProgressObject.Message
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub ToolStripMenuItemDefaultToResultsTab_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemDefaultToResultsTab.Click

        ToolStripMenuItemDefaultToResultsTab.Checked = Not ToolStripMenuItemDefaultToResultsTab.Checked
        ToolStripMenuItemDefaultToQueryTab.Checked = Not ToolStripMenuItemDefaultToResultsTab.Checked
    End Sub

    Private Sub ToolStripMenuItemDefaultToQueryTab_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemDefaultToQueryTab.Click

        ToolStripMenuItemDefaultToQueryTab.Checked = Not ToolStripMenuItemDefaultToQueryTab.Checked
        ToolStripMenuItemDefaultToResultsTab.Checked = Not ToolStripMenuItemDefaultToQueryTab.Checked

    End Sub

    Private Sub ToolStripMenuItemAutoCapitalise_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemAutoCapitalise.Click

        ToolStripMenuItemAutoCapitalise.Checked = Not ToolStripMenuItemAutoCapitalise.Checked

    End Sub

    Private Sub ToolStripMenuItemNaturalLanguage_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemNaturalLanguage.Click

        Me.ToolStripMenuItemNaturalLanguage.Checked = Not Me.ToolStripMenuItemNaturalLanguage.Checked

        Me.ToolStripNaturalLanguage.Visible = Me.ToolStripMenuItemNaturalLanguage.Checked

    End Sub

    Private Sub TextBoxNaturalLanguage_GotFocus(sender As Object, e As EventArgs) Handles TextBoxNaturalLanguage.GotFocus

        Me.zbTextBoxNaturalLanguageFocused = True

    End Sub

    Private Sub TextBoxNaturalLanguage_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxNaturalLanguage.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Call Me.GO(True)
                e.Handled = True
                e.SuppressKeyPress = True
            ElseIf e.KeyCode = Keys.Tab Then
                Me.TextBoxInput.Focus()
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxInput_Click(sender As Object, e As EventArgs) Handles TextBoxInput.Click

        Try
            Me.zbTextBoxNaturalLanguageFocused = False
            Me.hideAutoComplete()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxNaturalLanguage_Click(sender As Object, e As EventArgs) Handles TextBoxNaturalLanguage.Click

        Try
            Me.hideAutoComplete()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxNaturalLanguage_LostFocus(sender As Object, e As EventArgs) Handles TextBoxNaturalLanguage.LostFocus
        Me.zbTextBoxNaturalLanguageFocused = False
    End Sub
End Class