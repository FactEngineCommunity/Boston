Imports System.Reflection
Imports System.IO
Imports System.Web.UI
Public Class frmUnifiedOntologyBrowser

    Public WithEvents zrUnifiedOntology As Ontology.UnifiedOntology
    Private zrFrmORMDiagramViewer As frmDiagramORMForGlossary

    ''' <summary>
    ''' The Model for the information shown in the WebBrowser.
    ''' E.g. When the user clicks on an OntologyItem in the ListBox, the WebBrowser is populated with semantics of the OntologyItem.
    ''' Is the Model of the OntologyItem (ModelElement) selected in the ListBox (which can contain ModelElements from more than one Model).
    ''' </summary>
    Private mrWebBrowserModel As FBM.Model

    Private Sub frmGlossary_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Me.LabelOntologyName.Text = Me.zrUnifiedOntology.Name
            Call Me.ShowGlossary(Me.zrUnifiedOntology)

            '======================================================================================
            'Load the Sub ORM Diagram Form/Viewer
            Dim formToShow As New frmDiagramORMForGlossary
            formToShow.TopLevel = False
            formToShow.WindowState = FormWindowState.Maximized
            formToShow.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            formToShow.Visible = True
            'Dim lrPage As New FBM.Page(zrModel, "GlossaryPage", "GlossaryPage", pcenumLanguage.ORMModel)
            'formToShow.zrPage = lrPage
            'lrPage.Form = New Windows.Forms.Form
            'lrPage.Form = formToShow
            'lrPage.Diagram = formToShow.Diagram
            'lrPage.DiagramView = formToShow.DiagramView
            'Me.SplitContainer2.Panel2.Controls.Add(formToShow)

            formToShow.Show()
            formToShow.Height = Me.SplitContainer2.Panel2.Height
            'formToShow.Anchor = AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Bottom + AnchorStyles.Top

            'Call formToShow.DisplayORMModelPage(lrPage)
            zrFrmORMDiagramViewer = formToShow
            '======================================================================================

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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

    Private Sub ShowGlossary(ByRef arUnifiedOntology As Ontology.UnifiedOntology)

        Me.ListBox1.Items.Clear()

        Dim larUnifiedOntology As Ontology.UnifiedOntology = arUnifiedOntology

        Dim larModelDictionaryEntry = From Model In larUnifiedOntology.Model
                                      From ModelDictionaryEntry In Model.ModelDictionary
                                      Where ModelDictionaryEntry.isValueType Or
                                            ModelDictionaryEntry.isEntityType
                                      Select ModelDictionaryEntry
                                      Order By ModelDictionaryEntry.Symbol Ascending

        For Each lrModelDictionaryEntry In larModelDictionaryEntry
            Dim lrComboBoxItem As New tComboboxItem(lrModelDictionaryEntry.Symbol, lrModelDictionaryEntry.Symbol, lrModelDictionaryEntry)
            Me.ListBox1.Items.Add(lrComboBoxItem)
        Next

        'For Each lrEntityType In arModel.EntityType.FindAll(Function(x) x.IsObjectifyingEntityType = False _
        '                                                        And x.IsMDAModelElement = False)
        '    Me.ListBox1.Items.Add(lrEntityType.Name)
        'Next

        'For Each lrValueType In arModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
        '    Me.ListBox1.Items.Add(lrValueType.Name)
        'Next

        'For Each lrFactType In arModel.FactType.FindAll(Function(x) (x.IsObjectified And x.IsMDAModelElement = False) Or x.isRDSTable)
        '    Me.ListBox1.Items.Add(lrFactType.Name)
        'Next

    End Sub

    Public Sub FocusModelElement(ByRef arModelElement As FBM.ModelObject)

        Try
            Me.ListBox1.SelectedIndex = Me.ListBox1.Items.IndexOf(arModelElement.Id)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        'Dim items = From it In ListBox1.Items.Cast(Of Object)() _
        '    Where it.ToString().IndexOf(TextBox1.Text, StringComparison.CurrentCultureIgnoreCase) >= 0

        Dim items = From it In prApplication.WorkingModel.ModelDictionary
                    Where it.Symbol.IndexOf(TextBox1.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 _
                    And (it.isEntityType Or it.isValueType)

        'Dim matchingItemList As List(Of Object) = items.ToList()

        ListBox1.BeginUpdate()
        ListBox1.Items.Clear()
        For Each item In items 'matchingItemList
            ListBox1.Items.Add(item.Symbol)
        Next
        ListBox1.EndUpdate()

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

            With New WaitCursor
                If Me.ListBox1.SelectedIndex >= 0 Then
                    lrModelDictionaryEntry = ListBox1.SelectedItem.Tag
                End If

                If lrModelDictionaryEntry IsNot Nothing Then

                    Dim lrModelElement As FBM.ModelObject = Nothing

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
                            lrModelDictionaryEntry.Model.LoadFactTypesRelatedToModelElement(lrModelElement)

                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityType As FBM.EntityType

                            lrEntityType = lrModelDictionaryEntry.Model.EntityType.Find(Function(x) x.Id = lrModelDictionaryEntry.Symbol)

                            If lrEntityType Is Nothing Then
                                lrEntityType = New FBM.EntityType(lrModelDictionaryEntry.Model,
                                                                  pcenumLanguage.ORMModel,
                                                                  lrModelDictionaryEntry.Symbol,
                                                                  Nothing,
                                                                  True)
                                lrEntityType.Model.EntityType.AddUnique(TableEntityType.GetEntityTypeDetails(lrEntityType))

                                Call lrEntityType.Model.LoadEntityTypesReferenceSchemeModelElements(lrEntityType)

                                Call lrEntityType.SetReferenceModeObjects()
                            End If


                            lrModelElement = lrEntityType

                            'Load the related FactTypes.
                            lrModelDictionaryEntry.Model.LoadFactTypesRelatedToModelElement(lrModelElement)
                        Case Else
                            lrModelElement = Nothing
                    End Select

                    If lrModelElement IsNot Nothing Then
                        Call Me.DescribeModelElement(lrModelElement)
                    End If
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

    Public Sub DescribeModelElement(ByVal arModelElement As FBM.ModelObject)

        Try
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

            '-----------------------------------------------
            If arModelElement Is Nothing Then
                '-----------------------------------------------------------------
                'Clear the ORMDiagramView
                Me.zrFrmORMDiagramViewer.clear_diagram()

                '==============================================================
                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                If IsSomething(lrPropertyGridForm) Then
                    Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute})
                    lrPropertyGridForm.PropertyGrid.SelectedObject = arModelElement.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(arModelElement.Id))
                End If
            Else
                'Call Me.DisplayORMDiagramViewForModelObject(arModelElement)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub DisplayORMDiagramViewForModelObject(ByRef arModelElement As FBM.ModelObject)

        'Clear the ORM Diagram View's Page/Diagram
        Call zrFrmORMDiagramViewer.zrPage.ClearFast(True)

        '==============================================================
        Dim loPt As New PointF(50, 50)

        Select Case arModelElement.ConceptType
            Case Is = pcenumConceptType.ValueType
                Dim lrValueType As FBM.ValueType
                lrValueType = arModelElement
                Call zrFrmORMDiagramViewer.zrPage.DropValueTypeAtPoint(lrValueType, loPt)
                Call zrFrmORMDiagramViewer.LoadAssociatedFactTypes(lrValueType)
            Case Is = pcenumConceptType.EntityType
                Dim lrEntityType As FBM.EntityType
                lrEntityType = arModelElement
                Dim larSuptypeRelationship = From EntityType In arModelElement.Model.EntityType
                                             From SubtypeRelationship In EntityType.SubtypeRelationship
                                             Where EntityType.Id = lrEntityType.Id
                                             Select SubtypeRelationship

                For Each lrSubtypeRelationship In larSuptypeRelationship
                    If Not zrFrmORMDiagramViewer.zrPage.ContainsModelElement(lrSubtypeRelationship.parentModelElement) Then
                        Select Case lrSubtypeRelationship.parentModelElement.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                Call zrFrmORMDiagramViewer.zrPage.DropEntityTypeAtPoint(lrSubtypeRelationship.parentModelElement, New PointF(30, 30))
                        End Select
                    End If
                Next

                Call zrFrmORMDiagramViewer.zrPage.DropEntityTypeAtPoint(lrEntityType, loPt)
                Call zrFrmORMDiagramViewer.LoadAssociatedFactTypes(lrEntityType)
            Case Is = pcenumConceptType.FactType
                Dim lrFactType As FBM.FactType
                lrFactType = arModelElement
                Call zrFrmORMDiagramViewer.zrPage.DropFactTypeAtPoint(lrFactType, loPt, False)
            Case Is = pcenumConceptType.RoleConstraint
                Dim lrRoleConstraint As FBM.RoleConstraint
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                lrRoleConstraint = arModelElement

                Select Case lrRoleConstraint.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                        lrRoleConstraintInstance = zrFrmORMDiagramViewer.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                    Case Is = pcenumRoleConstraintType.RingConstraint,
                              pcenumRoleConstraintType.EqualityConstraint,
                              pcenumRoleConstraintType.ExternalUniquenessConstraint,
                              pcenumRoleConstraintType.ExclusiveORConstraint,
                              pcenumRoleConstraintType.ExclusionConstraint,
                              pcenumRoleConstraintType.SubsetConstraint
                        lrRoleConstraintInstance = zrFrmORMDiagramViewer.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                    Case Is = pcenumRoleConstraintType.FrequencyConstraint
                        Call zrFrmORMDiagramViewer.DropFrequencyConstraintAtPoint(lrRoleConstraint, loPt)
                End Select
        End Select

        Dim larSuptypeRelationshipInstance = From EntityTypeInstance In Me.zrFrmORMDiagramViewer.zrPage.EntityTypeInstance
                                             From SubtypeRelationshipInstance In EntityTypeInstance.SubtypeRelationship
                                             Select SubtypeRelationshipInstance


        For Each lrSubtypeRelationship In larSuptypeRelationshipInstance
            Call lrSubtypeRelationship.DisplayAndAssociate()
        Next

        Call zrFrmORMDiagramViewer.AutoLayout()
        zrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
        zrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
        zrFrmORMDiagramViewer.DiagramView.ZoomToFit()
        If zrFrmORMDiagramViewer.DiagramView.ZoomFactor > 150 Then
            zrFrmORMDiagramViewer.DiagramView.ZoomFactor = 150
        End If

    End Sub

    Private Sub WebBrowser_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser.DocumentCompleted

        '===================================================================================
        Dim liHeight As Integer = Me.WebBrowser.Height - 1

        liHeight = Me.WebBrowser.Document.Body.ScrollRectangle.Height

        Me.SplitContainer2.SplitterDistance = liHeight
        zrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
        zrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
        '===================================================================================

    End Sub

    Public Sub VerbaliseGeneralConcept(ByVal arDictionaryEntry As FBM.DictionaryEntry)
        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        lrVerbaliser.VerbaliseHeading(arDictionaryEntry.Symbol)
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
            lrVerbaliser.VerbaliseModelObject(lrFactType)
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

                    If IsSomething(lrFactTypeReading) Then

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
                        If Not IsSomething(lrRoleConstraint.Role.Find(AddressOf lrRole.Equals)) Then
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub frmGlossary_MenuStart(sender As Object, e As EventArgs) Handles Me.MenuStart

        zrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height

    End Sub

    Private Sub WebBrowser_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles WebBrowser.Navigating

        Dim lasURLArgument() As String
        Dim lsModelObjectName As String

        lasURLArgument = e.Url.ToString.Split(":")

        If lasURLArgument(0) = "elementid" Then

            lsModelObjectName = lasURLArgument(1)

            Dim lrModelObject As FBM.ModelObject

            lrModelObject = Me.mrWebBrowserModel.GetModelObjectByName(lsModelObjectName)

            Select Case lrModelObject.ConceptType
                Case Is = pcenumConceptType.ValueType
                    Call Me.VerbaliseValueType(lrModelObject)
                Case Is = pcenumConceptType.EntityType
                    Call Me.VerbaliseEntityType(lrModelObject)
                Case Is = pcenumConceptType.FactType
                    Call Me.VerbaliseFactType(lrModelObject)
                Case Is = pcenumConceptType.RoleConstraint
                    'TBA
            End Select

            Call Me.DisplayORMDiagramViewForModelObject(lrModelObject)

            Me.TextBox1.Text = ""
            Call Me.LoadGlossaryListbox()
            If Me.ListBox1.Items.Contains(lrModelObject.Id) Then
                Me.ListBox1.SelectedIndex = Me.ListBox1.FindString(lrModelObject.Id)
            End If

            '------------------------------------------------------------------------------------------
            'Cancel the Navigation so that the new verbalisation isn't wiped out.
            '  i.e. Because the Navication (e.URL) isn't to an actual URL, an error WebPage is shown,
            '  rather than the new Verbalisation. Cancelling the Navigation fixes this.
            '------------------------------------------------------------------------------------------
            e.Cancel = True

        End If

    End Sub

    Private Sub frmGlossary_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

        If IsSomething(zrFrmORMDiagramViewer) Then
            zrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
            zrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
        End If

    End Sub

    Private Sub SplitContainer2_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer2.SplitterMoved

        If IsSomething(zrFrmORMDiagramViewer) Then
            zrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
            zrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
        End If

    End Sub

End Class