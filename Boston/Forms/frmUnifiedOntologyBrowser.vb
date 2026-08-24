Imports System.Reflection
Imports System.IO
Imports System.Web.UI
Imports System.ComponentModel

Public Class frmUnifiedOntologyBrowser

    Public WithEvents zrUnifiedOntology As Ontology.UnifiedOntology
    Private mrFrmORMDiagramViewer As frmDiagramORMForOntologyBrowser
    Private mbORMViewExpanded As Boolean = False

    Private mrSelectedModelElement As FBM.ModelObject

    ''' <summary>
    ''' The Model for the information shown in the WebBrowser.
    ''' E.g. When the user clicks on an OntologyItem in the ListBox, the WebBrowser is populated with semantics of the OntologyItem.
    ''' Is the Model of the OntologyItem (ModelElement) selected in the ListBox (which can contain ModelElements from more than one Model).
    ''' </summary>
    Private mrWebBrowserModel As FBM.Model

    Private mbShadeDuplicates As Boolean = False

    ' Helper function to convert an Image to base64
    Private Function ImageToBase64(image As Image) As String
        Using ms As New System.IO.MemoryStream()
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
            Return Convert.ToBase64String(ms.ToArray())
        End Using
    End Function

    Private Sub frmGlossary_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Me.LabelOntologyName.Text = Me.zrUnifiedOntology.Name

            '=================================================================
            'Put the Models in the Combobox
            Dim lrComboboxItem = New tComboboxItem(Nothing, "All", Nothing)
            Me.ComboBoxModel.Items.Add(lrComboboxItem)
            For Each lrModel In Me.zrUnifiedOntology.Model
                lrComboboxItem = New tComboboxItem(lrModel.ModelId, lrModel.Name, lrModel)
                Me.ComboBoxModel.Items.Add(lrComboboxItem)
            Next
            Me.ComboBoxModel.SelectedIndex = 0

            '=================================================================
            'Show the Glossary
            Call Me.ShowGlossary(Me.zrUnifiedOntology, Nothing)

            Me.ToolStripStatusLabelModel.Visible = True
            Me.ToolStripStatusLabelModel.Text = "Term Count:  " & Me.ListBox1.Items.Count

            Try
#Region "Load the Image file"
                ' Load the image from the file
                Dim imageFilePath As String = Me.zrUnifiedOntology.ImageFileLocationName
                Dim image As Image = Image.FromFile(imageFilePath)

                ' Attach the DocumentCompleted event handler to the WebBrowser control
                Dim htmlImageTag As String = "<html><body style='margin:0;padding:0;'><img src='data:image/jpeg;base64," & ImageToBase64(image) & "' width='" & image.Width & "' height='" & image.Height & "'/></body></html>"
                Me.WebBrowser.DocumentText = htmlImageTag
#End Region
            Catch ex As Exception
                'We tried
            End Try

            '======================================================================================
            'Load the Sub ORM Diagram Form/Viewer
            Dim formToShow As New frmDiagramORMForOntologyBrowser
            formToShow.TopLevel = False
            formToShow.WindowState = FormWindowState.Maximized
            formToShow.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            formToShow.Visible = True
            Me.SplitContainer2.Panel2.Controls.Add(formToShow)

            formToShow.Show()
            formToShow.Height = Me.SplitContainer2.Panel2.Height
            'formToShow.Anchor = AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Bottom + AnchorStyles.Top

            mrFrmORMDiagramViewer = formToShow
            '======================================================================================

            prApplication.ActivePages.Add(Me)

            If Me.ListBox1.Items.Count > 0 Then
                Me.ListBox1.SelectedItem = 0
            End If
            Me.ListBox1.Refresh()
            Me.ListBox1.Invalidate()

            Me.ToolStripDropDownButton2.Image = My.Resources.MenuImages.Expand16x16

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub LoadGlossaryListbox()

        Dim items As New List(Of FBM.Concept)

        For Each lrValueType In prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            items.Add(lrValueType.Concept)
        Next

        For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
            items.Add(lrEntityType.Concept)
        Next

        ListBox1.BeginUpdate()
        ListBox1.Items.Clear()
        For Each item In items 'matchingItemList
            ListBox1.Items.Add(item.Symbol)
        Next
        ListBox1.EndUpdate()

    End Sub

    Private Sub ListBox1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles ListBox1.DrawItem

        Try

            If e.Index >= 0 AndAlso e.Index < ListBox1.Items.Count Then

                Dim item As tComboboxItem = DirectCast(ListBox1.Items(e.Index), tComboboxItem)
                Dim g As Graphics = e.Graphics
                Dim itemBounds As Rectangle = e.Bounds

                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    ' Item is selected, so draw with default selection colors
                    g.FillRectangle(SystemBrushes.Highlight, itemBounds)
                    g.DrawString(item.ToString(), e.Font, SystemBrushes.HighlightText, itemBounds)
                Else
                    Dim blackTextBounds As New Rectangle(itemBounds.Left, itemBounds.Top, 150, itemBounds.Height)
                    Dim grayTextBounds As New Rectangle(itemBounds.Left + 150, itemBounds.Top, itemBounds.Width - 150, itemBounds.Height)

                    ' Item is not selected, draw with custom colors
                    g.FillRectangle(SystemBrushes.Window, itemBounds)

                    g.DrawString(item.ItemData, e.Font, Brushes.Black, blackTextBounds)
                    Dim lsModelName As String = item.Tag.Model.Name
                    g.DrawString(lsModelName.CondenseString(15, 7, 5), e.Font, Brushes.Silver, grayTextBounds)

#Region "Duplicates"
                    If Me.mbShadeDuplicates Then
                        Dim duplicates As Boolean = Me.ListBox1.Items.OfType(Of tComboboxItem)().Count(Function(x) item.ItemData = x.ItemData) > 1
                        If duplicates Then
                            e.Graphics.FillRectangle(Brushes.LightBlue, itemBounds)
                        End If
                        g.DrawString(item.ItemData, e.Font, Brushes.Black, blackTextBounds)
                        g.DrawString(lsModelName.CondenseString(15, 7, 5), e.Font, Brushes.Silver, grayTextBounds)
                    End If
#End Region
                End If

                    e.DrawFocusRectangle()
            End If



        Catch ex As Exception
            'We tried.
        End Try
    End Sub

    Private Sub ShowGlossary(ByRef arUnifiedOntology As Ontology.UnifiedOntology,
                             Optional ByRef arModel As FBM.Model = Nothing)

        '------------------------
        'Clear the list
        Me.ListBox1.Items.Clear()

        Try

            'Housekeeping
            Me.ToolStripDropDownButton2.Image = My.Resources.MenuImages.Collapse16x16 'Because we are effectively resetting the ORM view.

            Dim larUnifiedOntology As Ontology.UnifiedOntology = arUnifiedOntology

            '---------------------------------------------------------
            'Get the ModelDictionary entries from the Model/s
            Dim larModelDictionaryEntry As List(Of FBM.DictionaryEntry)

            Dim larFinalModelDictionary As New List(Of FBM.DictionaryEntry)

            If arModel IsNot Nothing Then
                Dim lrModel = arModel
                larModelDictionaryEntry = (From ModelDictionaryEntry In lrModel.ModelDictionary
                                           Where ModelDictionaryEntry.isValueType Or
                                                 ModelDictionaryEntry.isEntityType Or
                                                 ModelDictionaryEntry.isGeneralConcept
                                           Where Not ModelDictionaryEntry.Symbol.StartsWith("Core")
                                           Select ModelDictionaryEntry
                                           Order By ModelDictionaryEntry.Symbol Ascending).ToList

                Dim lrActualModel = prApplication.Models.Find(Function(x) x.ModelId = lrModel.ModelId)

                If lrActualModel IsNot Nothing Then

                    Dim larActualModelDictionaryEntry = (From ModelDictionaryEntry In lrActualModel.ModelDictionary
                                                         Where ModelDictionaryEntry.isValueType Or
                                                              ModelDictionaryEntry.isEntityType Or
                                                              ModelDictionaryEntry.isGeneralConcept
                                                         From ModelElement In lrActualModel.getModelObjects
                                                         Where Not ModelElement.IsMDAModelElement
                                                         Where ModelDictionaryEntry.Symbol = ModelElement.Id
                                                         Select ModelDictionaryEntry
                                                         Order By ModelDictionaryEntry.Symbol Ascending).ToList

                    Dim larUniqueModelDictionaryEntry = larActualModelDictionaryEntry.Except(larModelDictionaryEntry)
                    larModelDictionaryEntry.AddRange(larUniqueModelDictionaryEntry)
                End If
            Else

                For Each lrModel In Me.zrUnifiedOntology.Model

                    larModelDictionaryEntry = (From Model In larUnifiedOntology.Model
                                               Where Model.ModelId = lrModel.ModelId
                                               From ModelDictionaryEntry In Model.ModelDictionary
                                               Where ModelDictionaryEntry.isValueType Or
                                                     ModelDictionaryEntry.isEntityType Or
                                                     ModelDictionaryEntry.isGeneralConcept
                                               Where Not ModelDictionaryEntry.Symbol.StartsWith("Core")
                                               Select ModelDictionaryEntry
                                               Order By ModelDictionaryEntry.Symbol Ascending).ToList

                    Dim lrActualModel = prApplication.Models.Find(Function(x) x.ModelId = lrModel.ModelId)

                    If lrActualModel IsNot Nothing Then

                        Dim larActualModelDictionaryEntry = (From ModelDictionaryEntry In lrActualModel.ModelDictionary
                                                             Where ModelDictionaryEntry.isValueType Or
                                                              ModelDictionaryEntry.isEntityType Or
                                                              ModelDictionaryEntry.isGeneralConcept
                                                             From ModelElement In lrActualModel.getModelObjects
                                                             Where Not ModelElement.IsMDAModelElement
                                                             Where ModelDictionaryEntry.Symbol = ModelElement.Id
                                                             Where Not larModelDictionaryEntry.Contains(ModelDictionaryEntry)
                                                             Select ModelDictionaryEntry
                                                             Order By ModelDictionaryEntry.Symbol Ascending).ToList

                        Dim loDictionaryEntryComparer = New FBM.CustomDictionaryEntryComparer
                        Dim larUniqueModelDictionaryEntry = larActualModelDictionaryEntry.Except(larModelDictionaryEntry, loDictionaryEntryComparer)
                        larModelDictionaryEntry.AddRange(larUniqueModelDictionaryEntry)
                    End If

                    larFinalModelDictionary.Addrange(larModelDictionaryEntry)
                Next
            End If

            Dim lrComboBoxItem As tComboboxItem
            For Each lrModelDictionaryEntry In larFinalModelDictionary

                Dim lsConceptSymbol As String = lrModelDictionaryEntry.Model.Name.CondenseString(15, 7, 5)

                lrComboBoxItem = New tComboboxItem(lrModelDictionaryEntry.Symbol, lrModelDictionaryEntry.Symbol & " - " & lsConceptSymbol, lrModelDictionaryEntry)

                Me.ListBox1.Items.Add(lrComboBoxItem)
            Next

            Me.ListBox1.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub FocusModelElement(ByRef arModelElement As FBM.ModelObject)

        Try
            Me.ListBox1.SelectedIndex = Me.ListBox1.Items.IndexOf(arModelElement.Id)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ShowSelectedItem()

        Try

            With New WaitCursor
                Dim larDictionaryEntry = (From Model In Me.zrUnifiedOntology.Model
                                          From DictionaryEntry In Model.ModelDictionary
                                          Where (DictionaryEntry.Symbol.IndexOf(SearchTextbox.TextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 _
                                               Or Fastenshtein.Levenshtein.Distance(DictionaryEntry.Symbol, SearchTextbox.TextBox.Text) < 4 _
                                               Or Boston.Soundex(DictionaryEntry.Symbol, 4) = Boston.Soundex(SearchTextbox.TextBox.Text, 4))
                                          Where DictionaryEntry.isEntityType Or DictionaryEntry.isValueType Or DictionaryEntry.isGeneralConcept
                                          Select DictionaryEntry).ToList

                For Each lrModel In Me.zrUnifiedOntology.Model
                    Dim lrActualModel = prApplication.Models.Find(Function(x) x.ModelId = lrModel.ModelId)

                    If lrActualModel IsNot Nothing Then

                        Dim larActualModelDictionaryEntry = (From ModelDictionaryEntry In lrActualModel.ModelDictionary
                                                             Where ModelDictionaryEntry.isValueType Or
                                                                   ModelDictionaryEntry.isEntityType Or
                                                                   ModelDictionaryEntry.isGeneralConcept
                                                             Where (ModelDictionaryEntry.Symbol.IndexOf(SearchTextbox.TextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 _
                                                                    Or Fastenshtein.Levenshtein.Distance(ModelDictionaryEntry.Symbol, SearchTextbox.TextBox.Text) < 4 _
                                                                    Or Boston.Soundex(ModelDictionaryEntry.Symbol, 4) = Boston.Soundex(SearchTextbox.TextBox.Text, 4))
                                                             Select ModelDictionaryEntry
                                                             Order By ModelDictionaryEntry.Symbol Ascending).ToList

                        Dim larUniqueModelDictionaryEntry = larActualModelDictionaryEntry.Except(lrModel.ModelDictionary)
                        larDictionaryEntry.AddRange(larUniqueModelDictionaryEntry)
                    End If
                Next

                ListBox1.BeginUpdate()
                ListBox1.Items.Clear()
                Dim lrComboBoxItem As tComboboxItem
                For Each DictionaryEntry In larDictionaryEntry 'matchingItemList
                    lrComboBoxItem = New tComboboxItem(DictionaryEntry.Symbol, DictionaryEntry.Symbol, DictionaryEntry)
                    ListBox1.Items.Add(lrComboBoxItem)
                Next

                ListBox1.EndUpdate()
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Public Sub VerbaliseEntityType(ByVal arEntityType As FBM.EntityType)
        '------------------------------------------------------
        'PSEUDOCODE
        '  * Declare that the EntityType(Name) is an EntityType
        '  * Verbalise the ReferenceScheme
        '  * FOR EACH IncomingLink (from a Role)
        '      * Verbalise the FactType for the associated Role
        '  * LOOP 
        '------------------------------------------------------
        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Try

            '------------------------------------------------------
            'Declare that the EntityType(Name) is an EntityType
            '------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arEntityType)
            lrVerbaliser.VerbaliseQuantifier(" is an Entity Type.")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.VerbaliseBlackText("Model: ")
            lrVerbaliser.VerbaliseQuantifier(arEntityType.Model.Name)
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If (arEntityType.ShortDescription <> "") Or (arEntityType.LongDescription <> "") Then
                lrVerbaliser.VerbaliseQuantifier("Informally: ")
                lrVerbaliser.HTW.WriteBreak()
                If arEntityType.ShortDescription <> "" Then
                    lrVerbaliser.VerbaliseQuantifier("(Short Description) ")
                    lrVerbaliser.VerbaliseHeading(arEntityType.ShortDescription)
                End If
                If arEntityType.LongDescription <> "" Then
                    lrVerbaliser.VerbaliseQuantifier("(Long Description) ")
                    lrVerbaliser.VerbaliseHeading(arEntityType.LongDescription)
                End If
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()
            End If

            '------------------------------
            'Verbalise the ReferenceScheme
            '------------------------------                   
            Dim lrTopmostSupertype As FBM.EntityType
            Dim lrFactType As FBM.FactType
            Dim liInd As Integer = 0

            If arEntityType.IsSubtype Then
                lrTopmostSupertype = arEntityType.GetTopmostSupertype
            Else 'Is Not Subtype
                lrTopmostSupertype = arEntityType
            End If

            If lrTopmostSupertype.HasSimpleReferenceScheme Then
                lrVerbaliser.VerbaliseQuantifier("Reference Scheme: " & lrTopmostSupertype.Name & " has ")
                lrVerbaliser.VerbaliseModelObject(lrTopmostSupertype)
                lrVerbaliser.VerbaliseQuantifier(" has ")

                lrVerbaliser.VerbaliseModelObject(lrTopmostSupertype.ReferenceModeValueType)

                lrVerbaliser.HTW.WriteBreak()

                '----------------------------
                'Verbalise the ReferenceMode
                '----------------------------
                lrVerbaliser.VerbaliseQuantifier("Reference Mode: ")
                lrVerbaliser.VerbaliseQuantifier(lrTopmostSupertype.ReferenceMode)
            ElseIf lrTopmostSupertype.HasCompoundReferenceMode Then
                lrVerbaliser.VerbaliseQuantifier("Reference Scheme: ")
                For Each lrRoleConstraintRole In lrTopmostSupertype.ReferenceModeRoleConstraint.RoleConstraintRole
                    lrFactType = lrRoleConstraintRole.Role.FactType
                    Dim larRole As New List(Of FBM.Role)
                    larRole.Add(lrFactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))
                    larRole.Add(lrRoleConstraintRole.Role)
                    Dim lrFactTypeReading As FBM.FactTypeReading
                    lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                    If lrFactTypeReading IsNot Nothing Then
                        Call lrFactTypeReading.GetReadingText(lrVerbaliser)
                    Else
                        lrVerbaliser.VerbaliseError("Provide a Fact Type Reading for the Fact Type:")
                        lrVerbaliser.VerbaliseModelObject(lrFactType)
                        lrVerbaliser.VerbaliseError(" with  " & lrRoleConstraintRole.Role.JoinedORMObject.Id & " at the last position in the reading.")
                    End If
                    If liInd < lrTopmostSupertype.ReferenceModeRoleConstraint.RoleConstraintRole.Count - 1 Then
                        lrVerbaliser.VerbaliseQuantifier(", ")
                    End If
                    liInd += 1
                Next
            Else
                lrVerbaliser.VerbaliseError("Provide a Reference Mode for the Entity Type:")
                lrVerbaliser.VerbaliseModelObject(lrTopmostSupertype)
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            '-------------------------------------------------
            'FOR EACH IncomingLink (from a Role)
            '  Verbalise the FactType for the associated Role
            'LOOP 
            '-------------------------------------------------
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Fact Types:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            'LINQ
            Dim FactType = From ft In arEntityType.Model.FactType,
                                rl In ft.RoleGroup
                           Where rl.JoinedORMObject.Id = arEntityType.Id
                           Select ft Distinct

            For Each lrFactType In FactType

                lrVerbaliser.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "FTR")
                lrVerbaliser.HTW.RenderBeginTag(HtmlTextWriterTag.Div)

                If lrFactType.FactTypeReading.Count = 0 Then
                    lrVerbaliser.VerbaliseModelObject(lrFactType)
                Else
                    Dim lrFactTypeReading = lrFactType.getOutgoingFactTypeReading(arEntityType)

                    If lrFactTypeReading Is Nothing Then
                        Call lrFactType.FactTypeReading(0).GetReadingText(lrVerbaliser, True)
                    Else
                        If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                            lrVerbaliser.VerbaliseQuantifierLight("Each ")
                        End If
                        lrFactTypeReading.GetReadingText(lrVerbaliser, True)
                    End If

                    lrVerbaliser.VerbaliseTextLightGray(" (")
                    lrVerbaliser.VerbaliseModelObjectLightGray(lrFactType)
                    lrVerbaliser.VerbaliseTextLightGray(") ")
                    '=======================================================================================
                    If lrFactType.IsBinaryFactType Then
                        If lrFactType.Is1To1BinaryFactType Then
                            If lrFactType.FactTypeReading.Count = 1 Then
                                'No reverse reading is provided for the FactType.
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.VerbaliseIndent()
                                Dim lrRole As FBM.Role
                                lrRole = lrFactType.GetOtherRoleOfBinaryFactType(lrFactType.GetRoleByJoinedObjectTypeId(arEntityType.Id).Id)
                                lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                                lrVerbaliser.VerbaliseQuantifierLight(" uniquely identifies ")
                                lrVerbaliser.VerbaliseModelObject(arEntityType)
                            Else
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.VerbaliseIndent()
                                lrFactType.getNotOutgoingFactTypeReadings(arEntityType)(0).GetReadingText(lrVerbaliser, True)
                            End If
                        Else
                            If lrFactType.FactTypeReading.Count > 1 Then
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.VerbaliseIndent()
                                Try
                                    lrFactType.getNotOutgoingFactTypeReadings(arEntityType)(0).GetReadingText(lrVerbaliser, True)
                                Catch ex As Exception
                                    'Not a biggie.
                                End Try

                            End If
                        End If
                    End If
                    '=======================================================================================                
                End If

                lrVerbaliser.HTW.RenderEndTag()
                lrVerbaliser.HTW.WriteBreak()

            Next

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("Subtypes:")
            lrVerbaliser.HTW.WriteBreak()

            Dim lrModelObject As FBM.ModelObject
            If arEntityType.childModelObjectList.Count = 0 Then
                lrVerbaliser.VerbaliseQuantifierLight("There are no Subtypes of this Entity Type.")
                lrVerbaliser.HTW.WriteBreak()
            Else
                lrVerbaliser.HTW.WriteBreak()
                For Each lrModelObject In arEntityType.childModelObjectList
                    lrVerbaliser.VerbaliseModelObject(lrModelObject)
                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("Supertypes:")
            lrVerbaliser.HTW.WriteBreak()

            If arEntityType.parentModelObjectList.Count = 0 Then
                lrVerbaliser.VerbaliseQuantifierLight("There are no Supertypes of this Entity Type.")
            Else
                lrVerbaliser.HTW.WriteBreak()
                For Each lrModelObject In arEntityType.parentModelObjectList
                    lrVerbaliser.VerbaliseModelObject(lrModelObject)
                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

        Catch ex As Exception
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseError(ex.Message)
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseError(ex.StackTrace)
        Finally
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
        End Try

    End Sub


    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click

        Try
            Dim lrModelDictionaryEntry As FBM.DictionaryEntry = Nothing

            Me.ListBox1.SuspendLayout()

            With New WaitCursor
                If Me.ListBox1.SelectedIndex >= 0 Then
                    lrModelDictionaryEntry = ListBox1.SelectedItem.Tag
                End If

                'Load the Model Elements for the Model because are required in Verbalisation.
                If lrModelDictionaryEntry IsNot Nothing Then

                    Me.mrWebBrowserModel = lrModelDictionaryEntry.Model

                    Me.ToolStripStatusLabelModel.Visible = True
                    Me.ToolStripStatusLabelModel.Text = "Model: " & lrModelDictionaryEntry.Model.Name

                    Dim lrModelElement As FBM.ModelObject = Nothing

                    Select Case lrModelDictionaryEntry.GetModelObjectConceptType

                        Case Is = pcenumConceptType.GeneralConcept

                            lrModelElement = New FBM.ModelObject(lrModelDictionaryEntry.Symbol, pcenumConceptType.GeneralConcept)
                            lrModelElement.Model = lrModelDictionaryEntry.Model

                        Case Is = pcenumConceptType.ValueType
                            Dim lrValueType As FBM.ValueType
                            lrValueType = lrModelDictionaryEntry.Model.ValueType.Find(Function(x) x.Id = lrModelDictionaryEntry.Symbol)

                            If lrValueType Is Nothing Then
                                lrValueType = New FBM.ValueType(lrModelDictionaryEntry.Model,
                                                                pcenumLanguage.ORMModel,
                                                                lrModelDictionaryEntry.Symbol,
                                                                lrModelDictionaryEntry.Symbol)

                                lrValueType.Model.ValueType.Add(TableValueType.GetValueTypeDetails(lrValueType))
                            End If

                            lrModelElement = lrValueType

                            'Load the related FactTypes.
                            lrModelDictionaryEntry.Model.LoadFactTypesRelatedToModelElement(lrModelElement, True)
                            Call TableSubtypeRelationship.GetSubtypeRelationshipsForModelElementByModel(lrValueType, True)

                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityType As FBM.EntityType

                            lrEntityType = lrModelDictionaryEntry.Model.EntityType.Find(Function(x) x.Id = lrModelDictionaryEntry.Symbol)

                            If lrEntityType Is Nothing Then
                                lrEntityType = New FBM.EntityType(lrModelDictionaryEntry.Model,
                                                                  pcenumLanguage.ORMModel,
                                                                  lrModelDictionaryEntry.Symbol,
                                                                  Nothing,
                                                                  True)
                                Boston.WriteToStatusBar("Getting Entity Type details.", True)
                                lrEntityType = TableEntityType.GetEntityTypeDetails(lrEntityType)
                                lrEntityType.Model.EntityType.AddUnique(lrEntityType)

                                Boston.WriteToStatusBar("Loading Entity Type Reference Scheme Model Elements.", True)
                                Call lrEntityType.Model.LoadEntityTypesReferenceSchemeModelElements(lrEntityType)
                                Call lrEntityType.SetReferenceModeObjects()
                                Boston.WriteToStatusBar("Getting Subtype Relationships for Model Element.", True)
                                Call TableSubtypeRelationship.GetSubtypeRelationshipsForModelElementByModel(lrEntityType, True)
                            End If


                            lrModelElement = lrEntityType

                            'Load the related FactTypes.
                            lrModelDictionaryEntry.Model.LoadFactTypesRelatedToModelElement(lrModelElement, True)
                        Case Else
                            lrModelElement = Nothing
                    End Select

                    If lrModelElement IsNot Nothing Then

                        Me.mrSelectedModelElement = lrModelElement

                        Call Me.DescribeModelElement(lrModelElement)

                        Call lrModelElement.GetConceptClassifications()

                        'Setup the Concept Classificaations toolbox
                        Dim lfrmToolboxConceptClassication As frmToolboxConceptClassification
                        lfrmToolboxConceptClassication = prApplication.GetToolboxForm(frmToolboxConceptClassification.Name)
                        If lfrmToolboxConceptClassication IsNot Nothing Then
                            Call lfrmToolboxConceptClassication.SetupForm(lrModelElement)
                        End If

                    End If
                End If
            End With

            Me.ListBox1.ResumeLayout()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub DescribeModelElement(ByVal arModelElement As FBM.ModelObject)

        Try
            Me.mrSelectedModelElement = arModelElement

            With New WaitCursor
                If arModelElement.ConceptType = pcenumConceptType.GeneralConcept Then

                    Dim lrDictionaryEntry As FBM.DictionaryEntry = arModelElement.Model.ModelDictionary.Find(Function(x) x.Symbol = arModelElement.Id)
                    Call Me.VerbaliseGeneralConcept(lrDictionaryEntry)

                Else
                    Select Case arModelElement.Model.GetConceptTypeByNameFuzzy(arModelElement.Id, arModelElement.Id)
                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityType As FBM.EntityType
                            lrEntityType = arModelElement.Model.GetModelObjectByName(arModelElement.Id)
                            Call Me.VerbaliseEntityType(lrEntityType)

                        Case Is = pcenumConceptType.ValueType
                            Dim lrValueType As FBM.ValueType
                            lrValueType = arModelElement.Model.GetModelObjectByName(arModelElement.Id)
                            Call Me.VerbaliseValueType(lrValueType)

                        Case Is = pcenumConceptType.FactType
                            Dim lrFactType As FBM.FactType
                            lrFactType = arModelElement.Model.GetModelObjectByName(arModelElement.Id)
                            Call Me.VerbaliseFactType(lrFactType)
                        Case Is = pcenumConceptType.GeneralConcept
                            Call Me.VerbaliseGeneralConcept(arModelElement.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(arModelElement.Id)))
                    End Select

                End If
            End With

            '-----------------------------------------------
            If arModelElement Is Nothing Then
                '-----------------------------------------------------------------
                'Clear the ORMDiagramView
                Me.mrFrmORMDiagramViewer.clear_diagram()

                '==============================================================
                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                If lrPropertyGridForm IsNot Nothing Then
                    Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute})
                    lrPropertyGridForm.PropertyGrid.SelectedObject = arModelElement.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(arModelElement.Id))
                End If
            Else
                Call Me.DisplayORMDiagramViewForModelObject(arModelElement)
            End If

            Me.mbORMViewExpanded = False
            Me.ToolStripDropDownButton2.Image = My.Resources.MenuImages.Expand16x16

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub DisplayORMDiagramViewForModelObject(ByRef arModelElement As FBM.ModelObject)

        Try
            Me.mrFrmORMDiagramViewer.zrPage = New FBM.Page(arModelElement.Model, Nothing, "Ontology Page", pcenumLanguage.ORMModel)
            Me.mrFrmORMDiagramViewer.zrPage.DiagramView = Me.mrFrmORMDiagramViewer.DiagramView
            Me.mrFrmORMDiagramViewer.zrPage.Diagram = Me.mrFrmORMDiagramViewer.Diagram
            Me.mrFrmORMDiagramViewer.zrPage.Form = Me.mrFrmORMDiagramViewer

            'Clear the ORM Diagram View's Page/Diagram
            Call Me.mrFrmORMDiagramViewer.zrPage.ClearFast(True)

            '==============================================================
            Dim loPt As New PointF(50, 50)

            Select Case arModelElement.ConceptType
                Case Is = pcenumConceptType.ValueType
                    Dim lrValueType As FBM.ValueType
                    lrValueType = arModelElement
                    Call mrFrmORMDiagramViewer.zrPage.DropValueTypeAtPoint(lrValueType, loPt)
                    Call mrFrmORMDiagramViewer.LoadAssociatedFactTypes(lrValueType)
                Case Is = pcenumConceptType.EntityType
                    Dim lrEntityType As FBM.EntityType
                    lrEntityType = arModelElement
                    Dim larSuptypeRelationship = From EntityType In arModelElement.Model.EntityType
                                                 From SubtypeRelationship In EntityType.SubtypeRelationship
                                                 Where EntityType.Id = lrEntityType.Id
                                                 Select SubtypeRelationship

                    For Each lrSubtypeRelationship In larSuptypeRelationship
                        If Not mrFrmORMDiagramViewer.zrPage.ContainsModelElement(lrSubtypeRelationship.parentModelElement) Then
                            Select Case lrSubtypeRelationship.parentModelElement.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Call mrFrmORMDiagramViewer.zrPage.DropEntityTypeAtPoint(lrSubtypeRelationship.parentModelElement, New PointF(30, 30))
                            End Select
                        End If
                    Next

                    Call mrFrmORMDiagramViewer.zrPage.DropEntityTypeAtPoint(lrEntityType, loPt)
                    Call mrFrmORMDiagramViewer.LoadAssociatedFactTypes(lrEntityType)
                Case Is = pcenumConceptType.FactType
                    Dim lrFactType As FBM.FactType
                    lrFactType = arModelElement
                    Call mrFrmORMDiagramViewer.zrPage.DropFactTypeAtPoint(lrFactType, loPt, False,,,,,,,, True)
                Case Is = pcenumConceptType.RoleConstraint
                    Dim lrRoleConstraint As FBM.RoleConstraint
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                    lrRoleConstraint = arModelElement

                    Select Case lrRoleConstraint.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                            lrRoleConstraintInstance = mrFrmORMDiagramViewer.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                        Case Is = pcenumRoleConstraintType.RingConstraint,
                                  pcenumRoleConstraintType.EqualityConstraint,
                                  pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                  pcenumRoleConstraintType.ExclusiveORConstraint,
                                  pcenumRoleConstraintType.ExclusionConstraint,
                                  pcenumRoleConstraintType.SubsetConstraint
                            lrRoleConstraintInstance = mrFrmORMDiagramViewer.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                        Case Is = pcenumRoleConstraintType.FrequencyConstraint
                            Call mrFrmORMDiagramViewer.DropFrequencyConstraintAtPoint(lrRoleConstraint, loPt)
                    End Select
            End Select

            Dim larSuptypeRelationshipInstance = From EntityTypeInstance In Me.mrFrmORMDiagramViewer.zrPage.EntityTypeInstance
                                                 From SubtypeRelationshipInstance In EntityTypeInstance.SubtypeRelationship
                                                 Select SubtypeRelationshipInstance


            For Each lrSubtypeRelationship In larSuptypeRelationshipInstance
                Call lrSubtypeRelationship.DisplayAndAssociate()
            Next

            Call mrFrmORMDiagramViewer.AutoLayoutSimple()
            mrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
            mrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
            mrFrmORMDiagramViewer.DiagramView.ZoomToFit()
            If mrFrmORMDiagramViewer.DiagramView.ZoomFactor > 150 Then
                mrFrmORMDiagramViewer.DiagramView.ZoomFactor = 150
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub WebBrowser_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser.DocumentCompleted

        '===================================================================================
        Dim liHeight As Integer = Me.WebBrowser.Height - 1

        liHeight = Me.WebBrowser.Document.Body.ScrollRectangle.Height

        Me.SplitContainer2.SplitterDistance = liHeight
        mrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
        mrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
        '===================================================================================

    End Sub

    Public Sub VerbaliseGeneralConcept(ByVal arDictionaryEntry As FBM.DictionaryEntry)
        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        lrVerbaliser.VerbaliseHeading(arDictionaryEntry.Symbol)
        lrVerbaliser.VerbaliseQuantifier(" is an General Concept.")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseIndent()
        lrVerbaliser.VerbaliseError("Change this General Concept to an Object Type as soon as possible.")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier("Informally: ")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        If arDictionaryEntry.ShortDescription <> "" Then
            lrVerbaliser.VerbaliseQuantifier("(Short Description) ")
            lrVerbaliser.VerbaliseQuantifierLight(arDictionaryEntry.ShortDescription)
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
        End If
        If arDictionaryEntry.LongDescription <> "" Then
            lrVerbaliser.VerbaliseQuantifier("(Long Description) ")
            lrVerbaliser.VerbaliseQuantifierLight(arDictionaryEntry.LongDescription)
        End If
        If arDictionaryEntry.ShortDescription.Trim = "" And arDictionaryEntry.LongDescription.Trim = "" Then
            lrVerbaliser.VerbaliseIndent()
            lrVerbaliser.VerbaliseQuantifierLight("There is no Short or Long Description for this Model Element yet.")
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseValueType(ByVal arValueType As FBM.ValueType)
        '------------------------------------------------------
        'PSEUDOCODE
        '  * Declare that the ValueType(Name) is an ValueType
        '------------------------------------------------------
        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------
        'Declare that the EntityType(Name) is an EntityType
        '------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arValueType)
        lrVerbaliser.VerbaliseQuantifier(" is a Value Type.")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        lrVerbaliser.VerbaliseBlackText("Model: ")
        lrVerbaliser.VerbaliseQuantifier(arValueType.Model.Name)
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If (arValueType.ShortDescription <> "") Or (arValueType.LongDescription <> "") Then
            lrVerbaliser.VerbaliseQuantifier("Informally: ")
            lrVerbaliser.HTW.WriteBreak()
            If arValueType.ShortDescription <> "" Then
                lrVerbaliser.VerbaliseQuantifier("(Short Description) ")
                lrVerbaliser.VerbaliseHeading(arValueType.ShortDescription)
            End If
            If arValueType.LongDescription <> "" Then
                lrVerbaliser.VerbaliseQuantifier("(Long Description) ")
                lrVerbaliser.VerbaliseHeading(arValueType.LongDescription)
            End If
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
        End If

        '-------------------------------------------------
        'FOR EACH IncomingLink (from a Role)
        '  Verbalise the FactType for the associated Role
        'LOOP 
        '-------------------------------------------------
        lrVerbaliser.VerbaliseQuantifier("Data Type: ")
        lrVerbaliser.HTW.Write(arValueType.DataType.ToString)
        lrVerbaliser.HTW.WriteBreak()


        If arValueType.DataTypeLength <> 0 Then
            lrVerbaliser.VerbaliseQuantifier("Data Type Length: ")
            lrVerbaliser.HTW.Write(arValueType.DataTypeLength.ToString)
            lrVerbaliser.HTW.WriteBreak()
        End If

        If arValueType.DataTypePrecision <> 0 Then
            lrVerbaliser.VerbaliseQuantifier("Data Type Precition: ")
            lrVerbaliser.HTW.Write(arValueType.DataTypePrecision.ToString)
            lrVerbaliser.HTW.WriteBreak()
        End If

        '-------------------------------------------------
        'FOR EACH IncomingLink (from a Role)
        '  Verbalise the FactType for the associated Role
        'LOOP 
        '-------------------------------------------------
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseHeading("Value Constraints:")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()


        Dim liCounter As Integer = 0
        Dim lrValueTypeConstraintString As String = ""

        If arValueType.ValueConstraint.Count > 0 Then
            lrVerbaliser.VerbaliseQuantifier("Possible Values for ")
            lrVerbaliser.VerbaliseModelObject(arValueType)
            lrVerbaliser.VerbaliseQuantifier(" are {")
            For Each lrValueTypeConstraintString In arValueType.ValueConstraint
                liCounter += 1
                If liCounter = 1 Then
                    lrVerbaliser.HTW.Write("'" & lrValueTypeConstraintString & "'")
                Else
                    lrVerbaliser.HTW.Write(", '" & lrValueTypeConstraintString & "'")
                End If
            Next
            lrVerbaliser.VerbaliseQuantifier("}")
        Else
            lrVerbaliser.VerbaliseIndent()
            lrVerbaliser.VerbaliseQuantifierLight("There are no Value Constraints for this Value Type.")
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseHeading("Sample Values:")
        lrVerbaliser.HTW.WriteBreak()

        'LINQ
        Dim RoleData = From ft In arValueType.Model.FactType,
                            rl In ft.RoleGroup,
                            fct In ft.Fact,
                            data In fct.Data
                       Where rl.JoinedORMObject.Id = arValueType.Id _
                            And data.Role.JoinedORMObject.Id = arValueType.Id
                       Select data Distinct

        Dim lrRoleData As FBM.FactData

        For Each lrRoleData In RoleData
            lrVerbaliser.HTW.Write("'" & lrRoleData.Data & "'")
        Next

        '-------------------------------------------------
        'FOR EACH IncomingLink (from a Role)
        '  Verbalise the FactType for the associated Role
        'LOOP 
        '-------------------------------------------------
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseHeading("Fact Types:")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()


        'LINQ
        Dim FactType = From ft In arValueType.Model.FactType,
                            rl In ft.RoleGroup
                       Where rl.JoinedORMObject.Id = arValueType.Id
                       Select ft Distinct

        Dim lrFactType As FBM.FactType


        For Each lrFactType In FactType
            lrVerbaliser.VerbaliseTextLightGray(" (")
            lrVerbaliser.VerbaliseModelObjectLightGray(lrFactType)
            lrVerbaliser.VerbaliseTextLightGray(") ")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseIndent()
            If lrFactType.FactTypeReading.Count > 0 Then
                Call lrFactType.FactTypeReading(0).GetReadingText(lrVerbaliser, True)
                '=======================================================================================
                If lrFactType.IsBinaryFactType Then
                    'No reverse reading is provided for the FactType.
                    If lrFactType.Is1To1BinaryFactType Then
                        lrVerbaliser.HTW.WriteBreak()
                        lrVerbaliser.VerbaliseIndent()
                        If lrFactType.FactTypeReading.Count = 1 Then
                            Dim lrRole As FBM.Role
                            lrRole = lrFactType.GetOtherRoleOfBinaryFactType(lrFactType.GetRoleByJoinedObjectTypeId(arValueType.Id).Id)
                            lrVerbaliser.VerbaliseModelObject(arValueType)
                            lrVerbaliser.VerbaliseQuantifierLight(" uniquely identifies ")
                            lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        Else
                            lrFactType.FactTypeReading(1).GetReadingText(lrVerbaliser, True)
                        End If
                    End If
                End If
                '======================================================================================= 
            End If
            lrVerbaliser.HTW.WriteBreak()
        Next

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseFactType(ByVal arFactType As FBM.FactType)
        '------------------------------------------------------
        'PSEUDOCODE
        '  * Declare that the FactType(Name) is an FactType
        '------------------------------------------------------

        Try
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim liInd As Integer = 0
            Dim lrFact As FBM.Fact
            Dim lrRole As FBM.Role
            Dim lrRoleConstraint As FBM.RoleConstraint

            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            '------------------------------------------------------
            'Declare that the EntityType(Name) is an EntityType
            '------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arFactType)
            lrVerbaliser.VerbaliseQuantifier(" is a Fact Type.")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.VerbaliseBlackText("Model: ")
            lrVerbaliser.VerbaliseQuantifier(arFactType.Model.Name)
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If (arFactType.ShortDescription <> "") Or (arFactType.LongDescription <> "") Then
                lrVerbaliser.VerbaliseQuantifier("Informally: ")
                lrVerbaliser.HTW.WriteBreak()
                If arFactType.ShortDescription <> "" Then
                    lrVerbaliser.VerbaliseQuantifier("(Short Description) ")
                    lrVerbaliser.VerbaliseHeading(arFactType.ShortDescription)
                End If
                If arFactType.LongDescription <> "" Then
                    lrVerbaliser.VerbaliseQuantifier("(Long Description) ")
                    lrVerbaliser.VerbaliseHeading(arFactType.LongDescription)
                End If
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()
            End If

            Select Case arFactType.Arity
                Case Is = 1
                Case Is = 2
                    If arFactType.FactTypeReading.Count > 0 Then
                        lrFactTypeReading = arFactType.FactTypeReading(0)
                        lrFactTypeReading.GetReadingText(lrVerbaliser)
                        lrVerbaliser.HTW.WriteBreak()


                        If arFactType.IsManyToOneByRoleOrder(lrFactTypeReading.RoleList) Then
                            lrVerbaliser.VerbaliseIndent()
                            lrVerbaliser.VerbaliseQuantifier("Each ")
                            lrVerbaliser.VerbaliseModelObject(arFactType.RoleGroup(0).JoinedORMObject)
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            lrVerbaliser.VerbaliseQuantifier(" at most one ")
                            lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                            lrVerbaliser.VerbaliseModelObject(arFactType.RoleGroup(1).JoinedORMObject)

                            lrVerbaliser.HTW.WriteBreak()
                            lrVerbaliser.VerbaliseIndent()

                            lrVerbaliser.VerbaliseQuantifier("It is possible that more than one ")
                            lrVerbaliser.VerbaliseModelObject(arFactType.RoleGroup(0).JoinedORMObject)
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            lrVerbaliser.VerbaliseQuantifier(" the same ")
                            lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                            lrVerbaliser.VerbaliseModelObject(arFactType.RoleGroup(1).JoinedORMObject)
                        End If
                    End If
                Case Else
            End Select

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.VerbaliseModelObject(arFactType)
            lrVerbaliser.VerbaliseQuantifier(" is where ")
            Call arFactType.FactTypeReading(0).GetReadingText(lrVerbaliser)



            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Fact Type Readings:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            For Each lrFactTypeReading In arFactType.FactTypeReading
                lrFactTypeReading.GetReadingText(lrVerbaliser)
                lrVerbaliser.HTW.WriteBreak()
            Next

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Sample Facts:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.Fact.Count = 0 Then
                lrVerbaliser.VerbaliseQuantifierLight("There are no Sample Facts against the Fact Type.")
                lrVerbaliser.HTW.WriteBreak()
            Else
                For Each lrFact In arFactType.Fact
                    lrVerbaliser.HTW.Write(lrFact.GetReading)
                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

            '------------------------
            'Uniqueness Constraints
            '------------------------
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Uniqueness Constraints:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()


            If arFactType.HasTotalRoleConstraint Then
                lrRoleConstraint = arFactType.InternalUniquenessConstraint(0)

                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(" has a Total Internal Uniqueness Constraint:")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("Role Constraint: ")
                lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseIndent()
                lrVerbaliser.VerbaliseQuantifier("In each population of ")
                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(" each")

                liInd = 0
                For Each lrRole In arFactType.RoleGroup
                    lrVerbaliser.HTW.Write(" ")
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    liInd += 1
                    If liInd < arFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" combination occurs at most once.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("This association of ")

                liInd = 0
                For Each lrRole In arFactType.RoleGroup
                    lrVerbaliser.HTW.Write(" ")
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    If liInd < arFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" provides the preferred identification scheme for ")
                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(".")
                lrVerbaliser.HTW.WriteBreak()

            ElseIf arFactType.Arity = 2 Then
                '--------------------
                'Is Binary FactType
                '--------------------
                Dim larRole As New List(Of FBM.Role)

                For Each lrRoleConstraint In arFactType.InternalUniquenessConstraint

                    lrVerbaliser.VerbaliseQuantifier("Role Constraint: ")
                    lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.VerbaliseIndent()

                    lrRole = lrRoleConstraint.Role(0)
                    larRole.Add(lrRole)

                    For Each lrRole In arFactType.RoleGroup
                        If lrRole.Id <> lrRoleConstraint.Role(0).Id Then
                            larRole.Add(lrRole)
                        End If
                    Next

                    lrFactTypeReading = arFactType.FindSuitableFactTypeReadingByRoles(larRole)

                    If lrFactTypeReading IsNot Nothing Then

                        lrVerbaliser.VerbaliseQuantifier("Each ")
                        lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject)
                        lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)

                        If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                            lrVerbaliser.VerbaliseQuantifier(" one ")
                        Else
                            lrVerbaliser.VerbaliseQuantifier(" at most one ")
                        End If

                        lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                    End If

                    If lrRoleConstraint.IsPreferredIdentifier Then
                        lrVerbaliser.VerbaliseQuantifier(" (Preferred Identifier)")
                    End If

                    lrVerbaliser.HTW.WriteBreak()
                Next
            Else
                '-------------------------------------------
                'Is Partial Internal Uniqueness Constraint
                '-------------------------------------------
                For Each lrRoleConstraint In arFactType.InternalUniquenessConstraint

                    lrVerbaliser.VerbaliseQuantifier("Role Constraint: ")
                    lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.VerbaliseIndent()
                    lrVerbaliser.VerbaliseQuantifier("In each population of ")
                    lrVerbaliser.VerbaliseModelObject(arFactType)
                    lrVerbaliser.VerbaliseQuantifier(" each")

                    liInd = 0
                    For Each lrRole In lrRoleConstraint.Role
                        lrVerbaliser.HTW.Write(" ")
                        lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        liInd += 1
                        If liInd < lrRoleConstraint.Role.Count Then
                            lrVerbaliser.VerbaliseSeparator(",")
                        End If
                    Next
                    lrVerbaliser.VerbaliseQuantifier(" combination ")

                    For Each lrRole In arFactType.RoleGroup
                        If Not lrRoleConstraint.Role.Find(AddressOf lrRole.Equals) IsNot Nothing Then
                            lrVerbaliser.VerbaliseQuantifier("is unique and relates to exactly one instance of ")
                            lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        End If
                    Next

                    If lrRoleConstraint.IsPreferredIdentifier Then
                        lrVerbaliser.VerbaliseQuantifier(" (Preferred Identifier)")
                    End If

                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

            '---------------------------
            'Mandatory RoleConstraints
            '---------------------------
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Mandatory Role Constraints:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.RoleGroup.FindAll(Function(x) x.Mandatory = True).Count = 0 Then
                lrVerbaliser.VerbaliseQuantifierLight("There are no Mandatory Role Constraints against Roles of the Fact Type.")
            Else
                For Each lrRole In arFactType.RoleGroup
                    If lrRole.Mandatory = True Then
                        lrVerbaliser.VerbaliseQuantifier("For each instance of ")
                        lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        lrVerbaliser.VerbaliseQuantifier(" it is mandatory that that instance participate in ")
                        lrVerbaliser.VerbaliseModelObject(arFactType)
                        lrVerbaliser.HTW.Write(" ")
                        lrVerbaliser.HTW.WriteBreak()
                    End If
                Next
            End If

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub frmGlossary_MenuStart(sender As Object, e As EventArgs) Handles Me.MenuStart

        mrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height

    End Sub

    Private Sub WebBrowser_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles WebBrowser.Navigating

        Dim lasURLArgument() As String
        Dim lsModelObjectName As String
        Dim lrModelElement As FBM.ModelObject

        Try

            lasURLArgument = e.Url.ToString.Split(":")

            If lasURLArgument(0) = "elementid" Then

                lsModelObjectName = lasURLArgument(1)

                lrModelElement = Me.mrWebBrowserModel.GetModelObjectByName(lsModelObjectName)

                If lrModelElement Is Nothing Then
#Region "Get ModelDictionaryEntry from Model"

                    Dim lrModelDictionaryEntry As FBM.DictionaryEntry

                    Try
                        lrModelDictionaryEntry = (From ModelDictionaryEntry In Me.mrWebBrowserModel.ModelDictionary
                                                  Where ModelDictionaryEntry.Symbol = lsModelObjectName
                                                  Select ModelDictionaryEntry).First
                    Catch ex As Exception
                        Exit Sub
                    End Try

                    Select Case lrModelDictionaryEntry.GetModelObjectConceptType
                        Case Is = pcenumConceptType.ValueType
                            Dim lrValueType As FBM.ValueType
                            lrValueType = lrModelDictionaryEntry.Model.ValueType.Find(Function(x) x.Id = lrModelDictionaryEntry.Symbol)

                            If lrValueType Is Nothing Then
                                lrValueType = New FBM.ValueType(lrModelDictionaryEntry.Model,
                                                                    pcenumLanguage.ORMModel,
                                                                    lrModelDictionaryEntry.Symbol,
                                                                    lrModelDictionaryEntry.Symbol)

                                lrValueType.Model.ValueType.Add(TableValueType.GetValueTypeDetails(lrValueType))
                            End If

                            lrModelElement = lrValueType

                            'Load the related FactTypes.
                            lrModelDictionaryEntry.Model.LoadFactTypesRelatedToModelElement(lrModelElement, True)
                            Call TableSubtypeRelationship.GetSubtypeRelationshipsForModelElementByModel(lrValueType, True)

                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityType As FBM.EntityType

                            lrEntityType = lrModelDictionaryEntry.Model.EntityType.Find(Function(x) x.Id = lrModelDictionaryEntry.Symbol)

                            If lrEntityType Is Nothing Then
                                lrEntityType = New FBM.EntityType(lrModelDictionaryEntry.Model,
                                                                      pcenumLanguage.ORMModel,
                                                                      lrModelDictionaryEntry.Symbol,
                                                                      Nothing,
                                                                      True)
                                lrEntityType = TableEntityType.GetEntityTypeDetails(lrEntityType)
                                lrEntityType.Model.EntityType.AddUnique(lrEntityType)

                                Call lrEntityType.Model.LoadEntityTypesReferenceSchemeModelElements(lrEntityType)
                                Call lrEntityType.SetReferenceModeObjects()
                                Call TableSubtypeRelationship.GetSubtypeRelationshipsForModelElementByModel(lrEntityType, True)
                            End If


                            lrModelElement = lrEntityType

                            'Load the related FactTypes.
                            lrModelDictionaryEntry.Model.LoadFactTypesRelatedToModelElement(lrModelElement, True)
                        Case Else
                            lrModelElement = Nothing
                    End Select
#End Region
                Else
                    lrModelElement.Model.LoadFactTypesRelatedToModelElement(lrModelElement, True)
                End If

                'CodeSafe
                If lrModelElement Is Nothing Then Exit Sub

                Me.mrSelectedModelElement = lrModelElement

                Select Case lrModelElement.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        Call Me.VerbaliseValueType(lrModelElement)
                    Case Is = pcenumConceptType.EntityType
                        Call Me.VerbaliseEntityType(lrModelElement)
                    Case Is = pcenumConceptType.FactType
                        Call Me.VerbaliseFactType(lrModelElement)
                    Case Is = pcenumConceptType.RoleConstraint
                        'TBA
                End Select

                Call Me.DisplayORMDiagramViewForModelObject(lrModelElement)

                SearchTextbox.TextBox.Text = ""
                'Call Me.LoadGlossaryListbox()
                If Me.ListBox1.Items.Contains(lrModelElement.Id) Then
                    Me.ListBox1.SelectedIndex = Me.ListBox1.FindString(lrModelElement.Id)
                End If

                '------------------------------------------------------------------------------------------
                'Cancel the Navigation so that the new verbalisation isn't wiped out.
                '  i.e. Because the Navication (e.URL) isn't to an actual URL, an error WebPage is shown,
                '  rather than the new Verbalisation. Cancelling the Navigation fixes this.
                '------------------------------------------------------------------------------------------
                e.Cancel = True

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub frmGlossary_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

        If mrFrmORMDiagramViewer IsNot Nothing Then
            mrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
            mrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
        End If

    End Sub

    Private Sub SplitContainer2_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer2.SplitterMoved

        If mrFrmORMDiagramViewer IsNot Nothing Then
            mrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
            mrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
        End If

    End Sub

    Private Sub ListBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseDown

        Try
            If e.Button = MouseButtons.Right Then
                Me.ListBox1.ContextMenuStrip = Me.ContextMenuStripModelElementPages
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ContextMenuStripModelElementPages_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStripModelElementPages.Opening

        Try
            Dim larPage As New List(Of FBM.Page)
            Dim lrModel As FBM.Model
            Dim lrModelDictionaryEntry As FBM.DictionaryEntry = Nothing
            Dim loMenuOption As ToolStripItem

            If Me.ListBox1.SelectedIndex >= 0 Then
                lrModelDictionaryEntry = ListBox1.SelectedItem.Tag
            Else
                Exit Sub
            End If

            Dim lrCopyPage As New FBM.Page(lrModelDictionaryEntry.Model, "CopyPage", "CopyPage", pcenumLanguage.ORMModel)

            '--------------------------------------------------------------------------------
            'Load the Pages/ORMDiagrams that relate to the ModelElement as selectable menuOptions
            '--------------------------------------------------------------------------------
#Region "Pages"
            lrModel = lrModelDictionaryEntry.Model
            Select Case lrModelDictionaryEntry.GetModelObjectConceptType
                Case Is = pcenumConceptType.ValueType
                    Dim lrValueType As New FBM.ValueType(lrModel, pcenumLanguage.ORMModel, lrModelDictionaryEntry.Symbol, True)

                    larPage = TableConceptInstance.getPagesContainingDictionaryEntry(lrModelDictionaryEntry)
                    For Each lrPage In prApplication.CMML.getStateTransitionDiagramPagesForValueType(lrValueType)
                        larPage.AddUnique(lrPage)
                    Next

                    '-------------------------------------------------------------------
                    'Add to CopyPage, so can copy to another Model if the user wants
                    lrCopyPage.SelectedObject.Add(lrValueType.CloneInstance(lrCopyPage, True))

                Case Is = pcenumConceptType.EntityType
                    Dim lrEntityType As New FBM.EntityType(lrModel, pcenumLanguage.ORMModel, lrModelDictionaryEntry.Symbol, Nothing, True)

                    larPage = TableConceptInstance.getPagesContainingDictionaryEntry(lrModelDictionaryEntry)
                    For Each lrPage In prApplication.CMML.getORMDiagramPagesForEntityType(lrEntityType)
                        larPage.AddUnique(lrPage)
                    Next

                    '-------------------------------------------------------------------
                    'Add to CopyPage, so can copy to another Model if the user wants
                    lrCopyPage.SelectedObject.Add(lrEntityType.CloneInstance(lrCopyPage, True, True))

                Case Is = pcenumConceptType.FactType
            End Select

            Me.ToolStripMenuItemViewOnPage.DropDownItems.Clear()
            For Each lrPage In larPage
                '----------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, 
                '  they will not be in the TreeView and so a menuOption
                '  is now added for those hidden Pages.
                '----------------------------------------------------------
                '---------------------------------------------------
                'Add the Page(Name) to the MenuOption.DropDownItems
                '---------------------------------------------------
                loMenuOption = Me.ToolStripMenuItemViewOnPage.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ORM16x16)
                loMenuOption.Tag = lrModel.Page.Find(Function(x) x.PageId = lrPage.PageId)
                AddHandler loMenuOption.Click, AddressOf Me.OpenORMModelPage
            Next
#End Region 'Pages

            '----------------------------------------------------------------------------------------
            'Load the Pages/ORMDiagrams that relate to the ModelElement as selectable menuOptions
            '--------------------------------------------------------------------------------
            Me.ToolStripMenuItemCopyToModel.DropDownItems.Clear()
            Call frmMain.CopySelectedObjectsToClipboard(lrCopyPage)

            '-------------------------------------------------------------------------------------------------
            'The user can copy the ModelElement to another Model
            '  Add the list of Models to the two respective menu items. One within the Glossary, and one within the Verbalisation view.
            For Each lrModel In Me.zrUnifiedOntology.Model.FindAll(Function(x) x.ModelId <> lrModelDictionaryEntry.Model.ModelId)
                '---------------------------------------------------
                'Add the Model(Name) to the MenuOption.DropDownItems
                '---------------------------------------------------
                loMenuOption = Me.ToolStripMenuItemCopyToModel.DropDownItems.Add(lrModel.Name, My.Resources.MenuImages.ORM16x16)
                Dim lrCopyToPage As New FBM.Page(lrModel, "CopyToPage", "CopyToPage", pcenumLanguage.ORMModel)
                loMenuOption.Tag = New With {.Model = lrModel}
                AddHandler loMenuOption.Click, AddressOf Me.CopyModelElementToModel

                loMenuOption = Me.ToolStripMenuItemCopyModelElementToModel.DropDownItems.Add(lrModel.Name, My.Resources.MenuImages.ORM16x16)
                lrCopyToPage = New FBM.Page(lrModel, "CopyToPage", "CopyToPage", pcenumLanguage.ORMModel)
                loMenuOption.Tag = New With {.Model = lrModel}
                AddHandler loMenuOption.Click, AddressOf Me.CopyModelElementToModel
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub CopyModelElementToModel(ByVal sender As Object, ByVal e As EventArgs)

        Try

            Dim loSenderItem As ToolStripItem = CType(sender, ToolStripItem)

            Dim lrModel As FBM.Model = loSenderItem.Tag.Model 'The prototype of the Model to copy to. Not the actual model in prApplication.Models (as in the Model Explorer)

            Dim lrCopyToModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = lrModel.ModelId)
            Dim lrCopyToPage As New FBM.Page(lrCopyToModel, "CopyToPage", "CopyToPage", pcenumLanguage.ORMModel)

            If MsgBox("Are you sure you want to copy the selected Model Element to the Model, " & lrModel.Name & "?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                If Not lrCopyToModel.Loaded Then
                    Boston.ShowFlashCard("Loading the Model, " & lrCopyToModel.Name, Color.LightGray)
                    With New WaitCursor
                        Call lrCopyToModel.Load(True)
                    End With
                End If

                'Page with ModelElement is in Clipboard, per the Opening method of the ContextMenuItem.
                Call frmMain.PasteToPageFromClipboard(lrCopyToModel, lrCopyToPage)

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub OpenORMModelPage(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim lrPage As FBM.Page

            With New WaitCursor
                '-----------------------------------------------
                'Get the Menu that just called this procedure.
                '-----------------------------------------------
                Dim lrToolStripItem As ToolStripItem = CType(sender, ToolStripItem)

                lrPage = lrToolStripItem.Tag
                prApplication.WorkingPage = lrPage

                If Not lrPage.Loaded Or lrPage.GetAllPageObjects.Count = 0 Then
                    Call lrPage.Model.loadModelLevelModelElementsForPage(lrPage)
                    lrPage.Loaded = False
                    Call lrPage.Load(True) 'Load and add to Model
                End If

                Try
                    If lrPage.FormLoaded Then
                        '-------------------------------------------------------------------
                        'The Page has already been loaded for Editing
                        '  Set the ZOrder of the Form on which the Page is loaded to OnTop
                        '-------------------------------------------------------------------
                        lrPage.Form.BringToFront() 'Can put ModelElement to be focused here as parameter.
                    Else
                        '--------------------------------------------------------------------------------------------------
                        'Add the Page to the Model.
                        '  NB The Model is loaded when the User clicks on the Model in the TreeView, so is already loaded.
                        '--------------------------------------------------------------------------------------------------  
                        Select Case lrPage.Language
                            Case Is = pcenumLanguage.ORMModel
                                Call frmMain.loadOntologyORMModelPage(lrPage, Nothing)
                        End Select

                    End If
                Catch ex As Exception
                    Dim lsMessage As String
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                End Try
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub VerbaliseUnfiedOntology(ByVal arUnifiedOntology As Ontology.UnifiedOntology)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------
        'Declare that the EntityType(Name) is an EntityType
        '------------------------------------------------------
        lrVerbaliser.VerbaliseHeading(arUnifiedOntology.Name)
        lrVerbaliser.VerbaliseQuantifier(" is a Unified Ontology.")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If arUnifiedOntology.Description <> "" Then
            lrVerbaliser.VerbaliseQuantifier("Description: ")
            lrVerbaliser.HTW.WriteBreak()
            If arUnifiedOntology.Description <> "" Then
                lrVerbaliser.VerbaliseHeading(arUnifiedOntology.Description)
            End If
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseHeading("Included Models:")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()




        lrVerbaliser.VerbaliseQuantifier("The ontology includes the following models: ")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        For Each lrModel In arUnifiedOntology.Model
            lrVerbaliser.HTW.Write(lrModel.Name)
            lrVerbaliser.HTW.WriteBreak()
        Next

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.Write("-------------------------------------------------")

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Private Sub ButtonDescribeUnifiedOntology_Click(sender As Object, e As EventArgs) Handles ButtonDescribeUnifiedOntology.Click

        Try
            Call Me.VerbaliseUnfiedOntology(Me.zrUnifiedOntology)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub SearchTextbox_InitiateSearch(asSearchString As String) Handles SearchTextbox.InitiateSearch

        Try
            Call Me.ShowSelectedItem()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub


    Private Sub frmUnifiedOntologyBrowser_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        Try
            prApplication.ActivePages.Remove(Me)
            prApplication.ToolboxForms.Remove(Me)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub DataLineageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataLineageToolStripMenuItem.Click

        Dim lrModelDictionaryEntry As FBM.DictionaryEntry
        Dim lrModelElement As FBM.ModelObject

        Try
            If Me.ListBox1.SelectedIndex >= 0 Then
                lrModelDictionaryEntry = ListBox1.SelectedItem.Tag
            Else
                Exit Sub
            End If

            Try
                Dim lrModel As FBM.Model = lrModelDictionaryEntry.Model

                Try
                    If lrModelDictionaryEntry.isGeneralConcept Then
                        lrModelElement = New FBM.ModelObject(lrModelDictionaryEntry.Symbol, pcenumConceptType.GeneralConcept)
                        lrModelElement.Model = lrModel
                    Else
                        lrModelElement = lrModel.GetModelObjectByName(lrModelDictionaryEntry.Symbol, True)
                    End If

                    If lrModelElement Is Nothing Then Throw New Exception("trip")
                Catch ex As Exception
                    Throw ex
                End Try

                If lrModelElement Is Nothing Then Throw New Exception("Couldn't find Model Element for: " & Me.ListBox1.Text)
            Catch
                Exit Sub
            End Try

            Call frmMain.LoadDataLineageForm(lrModelElement)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click

        Try
            Me.ComboBoxModel.SelectedIndex = 0
            Call Me.ShowGlossary(Me.zrUnifiedOntology)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxModel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxModel.SelectedIndexChanged

        Try
            Call Me.ShowGlossary(Me.zrUnifiedOntology, Me.ComboBoxModel.SelectedItem.Tag)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CopyToClipboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToClipboardToolStripMenuItem.Click

        Try
            Try
                Clipboard.Clear()
                Dim selectionText As String = WebBrowser.Document.Body.InnerText
                'Clipboard.SetText(selectionText)
                Clipboard.SetData(DataFormats.Text, CType(selectionText, Object))
            Catch ex As Exception
                'Not a biggie.
            End Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripDropDownButton2_Click(sender As Object, e As EventArgs) Handles ToolStripDropDownButton2.Click

        Me.mbORMViewExpanded = Not Me.mbORMViewExpanded

        Try

            If Me.mbORMViewExpanded Then
                Me.SplitContainer2.SplitterDistance = Me.SplitContainer2.Height / 4
                Me.ToolStripDropDownButton2.Image = My.Resources.MenuImages.Collapse16x16

                prApplication.WorkingPage = Me.mrFrmORMDiagramViewer.zrPage
                frmMain.ToolStripComboBox_zoom.SelectedIndex = 3
                Me.mrFrmORMDiagramViewer.zrPage.DiagramView.ZoomFactor = frmMain.ToolStripComboBox_zoom.SelectedItem.ItemData
            Else
                Dim liHeight As Integer = Me.WebBrowser.Height - 1

                Me.SplitContainer2.SplitterDistance = 0

                liHeight = Me.WebBrowser.Document.Body.ScrollRectangle.Height + 22 'StatusBar Height=22

                Me.SplitContainer2.SplitterDistance = liHeight
                Me.mrFrmORMDiagramViewer.DiagramView.ZoomFactor = 80
                Me.ToolStripDropDownButton2.Image = My.Resources.MenuImages.Expand16x16
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SplitContainer2_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer2.Paint

        Try
            Dim s As SplitContainer = TryCast(sender, SplitContainer)

            If s IsNot Nothing Then
                Dim top As Integer = 5
                Dim bottom As Integer = s.Height - 5
                Dim left As Integer = s.SplitterDistance
                Dim right As Integer = left + s.SplitterWidth - 1
                e.Graphics.FillRectangle(Brushes.LightGray, s.SplitterRectangle)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SplitContainer1_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Paint

        Try
            Dim s As SplitContainer = TryCast(sender, SplitContainer)

            If s IsNot Nothing Then
                Dim top As Integer = 5
                Dim bottom As Integer = s.Height - 5
                Dim left As Integer = s.SplitterDistance
                Dim right As Integer = left + s.SplitterWidth - 1
                e.Graphics.FillRectangle(Brushes.LightGray, s.SplitterRectangle)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ModelElementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModelElementToolStripMenuItem.Click

        Try
            Dim loMenuOption As ToolStripMenuItem

            'CodeSafe
            If Me.mrSelectedModelElement Is Nothing Then Exit Sub

            Me.ToolStripMenuItemCopyModelElementToModel.DropDownItems.Clear()

            Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.mrSelectedModelElement.Model, Me.mrSelectedModelElement.Id, Me.mrSelectedModelElement.ConceptType)

            Dim lrCopyPage As New FBM.Page(Me.mrSelectedModelElement.Model, System.Guid.NewGuid.ToString, "CopyToPage", pcenumLanguage.ORMModel)
            lrCopyPage.SelectedObject.Clear()
            Select Case Me.mrSelectedModelElement.GetType
                Case Is = GetType(FBM.ValueType)
                    lrCopyPage.SelectedObject.Add(CType(Me.mrSelectedModelElement, FBM.ValueType).CloneInstance(lrCopyPage, True, True))
                Case = GetType(FBM.EntityType)
                    lrCopyPage.SelectedObject.Add(CType(Me.mrSelectedModelElement, FBM.EntityType).CloneInstance(lrCopyPage, True, True, True))
                Case Is = GetType(FBM.FactType)
                    lrCopyPage.SelectedObject.Add(CType(Me.mrSelectedModelElement, FBM.FactType).CloneInstance(lrCopyPage, True, True))
            End Select

            Call frmMain.CopySelectedObjectsToClipboard(lrCopyPage)

            For Each lrModel In Me.zrUnifiedOntology.Model.FindAll(Function(x) x.ModelId <> lrModelDictionaryEntry.Model.ModelId)
                '---------------------------------------------------
                'Add the Model(Name) to the MenuOption.DropDownItems
                '---------------------------------------------------
                loMenuOption = Me.ToolStripMenuItemCopyModelElementToModel.DropDownItems.Add(lrModel.Name, My.Resources.MenuImages.ORM16x16)
                Dim lrCopyToPage As New FBM.Page(lrModel, "CopyToPage", "CopyToPage", pcenumLanguage.ORMModel)
                loMenuOption.Tag = New With {.Model = lrModel}
                AddHandler loMenuOption.Click, AddressOf Me.CopyModelElementToModel
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub RadioButtonShadeDuplicates_Click(sender As Object, e As EventArgs) Handles RadioButtonShadeDuplicates.Click

        Try
            Me.RadioButtonShadeDuplicates.Checked = Not Me.RadioButtonShadeDuplicates.Checked
            Me.RadioButtonShadeDuplicates.AutoCheck = False
            Me.RadioButtonShadeDuplicates.Refresh()
            Me.mbShadeDuplicates = Me.RadioButtonShadeDuplicates.Checked
            Me.ListBox1.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub MenuStripModelElement_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStripModelElement.ItemClicked

        Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class