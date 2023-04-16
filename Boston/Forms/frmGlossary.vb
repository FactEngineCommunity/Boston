Imports System.Reflection
Imports System.IO
Imports System.Web.UI
Public Class frmGlossary

    Public WithEvents mrModel As FBM.Model
    Private zrFrmORMDiagramViewer As frmDiagramORMForGlossary
    Private mrCurrentModelElement As FBM.ModelObject

    ''' <summary>
    ''' If a verbalisation is for a FactType, on the right hand side there is a faded-text link to that FactType.
    ''' Trouble is...if you copy the verbalisation text to a Word document (for example)...the faded-text FactType names
    ''' are copied over too. A Business Analysis document might not want that...and a Business Analyst might not
    ''' want to have to cut those Fact Type names out of the document after they have done a copy/paste.
    ''' </summary>
    Private mbHideFadedFactTypeNames As Boolean = False


    Private Sub frmGlossary_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus

        If prApplication.WorkingModel IsNot Nothing Then
            Me.mrModel = prApplication.WorkingModel
            Me.LabelModelName.Text = Me.mrModel.Name
            Call Me.ShowGlossary(Me.mrModel)
        End If

    End Sub

    Private Sub frmGlossary_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            If prApplication.WorkingModel IsNot Nothing Then
                Me.mrModel = prApplication.WorkingModel
                Me.LabelModelName.Text = Me.mrModel.Name
                Call Me.ShowGlossary(Me.mrModel)
            Else
                Throw New Exception("There is no current Model selected. Try selecting a Model in the Model Explorer and trying again.")
            End If

            '======================================================================================
            'Load the Sub ORM Diagram Form/Viewer
            Dim formToShow As New frmDiagramORMForGlossary
            formToShow.TopLevel = False
            formToShow.WindowState = FormWindowState.Maximized
            formToShow.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            formToShow.Visible = True
            Dim lrPage As New FBM.Page(Me.mrModel, "GlossaryPage", "GlossaryPage", pcenumLanguage.ORMModel)
            formToShow.zrPage = lrPage
            Me.SplitContainer2.Panel2.Controls.Add(formToShow)

            lrPage.Form = New Windows.Forms.Form
            lrPage.Form = formToShow
            'lrPage.ReferencedForm = formToShow
            lrPage.Diagram = formToShow.Diagram
            lrPage.DiagramView = formToShow.DiagramView

            formToShow.Show()
            formToShow.Height = Me.SplitContainer2.Panel2.Height
            'formToShow.Anchor = AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Bottom + AnchorStyles.Top

            Call formToShow.DisplayORMModelPage(lrPage)
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

        Dim items As New List(Of FBM.ModelObject)

        For Each lrValueType In Me.mrModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            items.Add(lrValueType)
        Next

        For Each lrEntityType In Me.mrModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
            items.Add(lrEntityType)
        Next

        ListBoxGlossary.BeginUpdate()
        ListBoxGlossary.Items.Clear()

        For Each item In items 'matchingItemList
            ListBoxGlossary.Items.Add(New tComboboxItem(item, item.Id, item))
        Next

        If Me.CheckBoxShowGeneralConcepts.Checked Then
            For Each lrDictionaryEntry In Me.mrModel.ModelDictionary.FindAll(Function(x) x.isGeneralConcept)
                ListBoxGlossary.Items.Add(New tComboboxItem(lrDictionaryEntry, lrDictionaryEntry.Symbol, lrDictionaryEntry))
            Next
        End If

        ListBoxGlossary.EndUpdate()



    End Sub

    Private Sub ShowGlossary(ByRef arModel As FBM.Model)

        Dim lrEntityType As FBM.EntityType

        Me.ListBoxGlossary.Items.Clear()

        For Each lrEntityType In arModel.EntityType.FindAll(Function(x) x.IsObjectifyingEntityType = False _
                                                                And x.IsMDAModelElement = False)

            Me.ListBoxGlossary.Items.Add(New tComboboxItem(lrEntityType, lrEntityType.Id, lrEntityType))
        Next

        For Each lrValueType In arModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            Me.ListBoxGlossary.Items.Add(New tComboboxItem(lrValueType, lrValueType.Id, lrValueType))
        Next

        For Each lrFactType In arModel.FactType.FindAll(Function(x) (x.IsObjectified And x.IsMDAModelElement = False) Or x.isRDSTable)
            Me.ListBoxGlossary.Items.Add(New tComboboxItem(lrFactType, lrFactType.Id, lrFactType))
        Next

        If Me.CheckBoxShowGeneralConcepts.Checked Then
            For Each lrDictionaryEntry In arModel.ModelDictionary.FindAll(Function(x) x.isGeneralConcept = True)
                Me.ListBoxGlossary.Items.Add(lrDictionaryEntry.Symbol)
            Next
        End If

    End Sub

    Public Sub FocusModelElement(ByRef arModelElement As FBM.ModelObject)

        Try
            Me.ListBoxGlossary.SelectedIndex = Me.ListBoxGlossary.Items.IndexOf(arModelElement.Id)

            Dim Index As Integer = Me.ListBoxGlossary.FindString(arModelElement.Id) 'Find the index of the item starting with whatever is in TextBox1.
            If Index > -1 Then 'Check if the item exists/was found.
                Me.ListBoxGlossary.SelectedIndex = Index
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            lrVerbaliser.VerbaliseQuantifier(" is an Entity Type")
            If arEntityType.IsActor Then
                lrVerbaliser.VerbaliseQuantifier(" and Actor.")
            Else
                lrVerbaliser.VerbaliseQuantifier(".")
            End If
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
                lrVerbaliser.VerbaliseQuantifier("Reference Scheme: ")
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
            Dim FactType = From ft In Me.mrModel.FactType,
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

                    If Not Me.mbHideFadedFactTypeNames Then
                        lrVerbaliser.VerbaliseTextLightGray(" (")
                        lrVerbaliser.VerbaliseModelObjectLightGray(lrFactType)
                        lrVerbaliser.VerbaliseTextLightGray(") ")
                    End If
                    '=======================================================================================
                    If lrFactType.IsBinaryFactType Then
                            If lrFactType.Is1To1BinaryFactType Then
                                If lrFactType.FactTypeReading.Count = 1 Then
                                    'No reverse reading is provided for the FactType.
                                    lrVerbaliser.HTW.WriteBreak()
                                    lrVerbaliser.VerbaliseIndent()
                                    Dim lrRole As FBM.Role
                                    Dim larRole As New List(Of FBM.Role)
                                    lrRole = lrFactType.GetRoleByJoinedObjectTypeId(arEntityType.Id)
                                    larRole.Add(lrRole)
                                    lrRole = lrFactType.GetOtherRoleOfBinaryFactType(lrRole.Id)
                                    larRole.Add(lrRole)
                                    Dim lrSentence As New Language.Sentence(larRole(0).JoinedORMObject.Id & " has " & larRole(1).JoinedORMObject.Id)
                                    lrSentence.PredicatePart.Add(New Language.PredicatePart("has"))
                                    lrSentence.PredicatePart.Add(New Language.PredicatePart(""))
                                    lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lrSentence)
                                    lrFactTypeReading = lrFactType.FactTypeReading.Find(AddressOf lrFactTypeReading.EqualsByRoleSequence)
                                    If lrFactTypeReading IsNot Nothing Then
                                        lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                                    End If
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

            If arEntityType.IsActor Then
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("Associated Processes:")
                lrVerbaliser.HTW.WriteBreak()

                If arEntityType.getAssociatedCMMLProcesses.Count = 0 Then
                    lrVerbaliser.VerbaliseQuantifierLight("There are no associated Processes for this Actor/Entity Type.")
                    lrVerbaliser.HTW.WriteBreak()
                Else
                    lrVerbaliser.HTW.WriteBreak()
                    For Each lrCMMLProcess In arEntityType.getAssociatedCMMLProcesses
                        lrVerbaliser.VerbalisePredicateText("'" & lrCMMLProcess.Text & "'")
                        lrVerbaliser.HTW.WriteBreak()
                    Next
                End If
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


    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBoxGlossary.Click

        Dim lsSelectedString As String = ""

        With New WaitCursor
            If Me.ListBoxGlossary.SelectedIndex >= 0 Then
                lsSelectedString = ListBoxGlossary.GetItemText(ListBoxGlossary.SelectedItem)
            End If

            Dim lrModelObject As FBM.ModelObject

            If lsSelectedString <> "" Then
                lrModelObject = Me.mrModel.GetModelObjectByName(lsSelectedString)

                Call Me.DescribeModelElement(lrModelObject)

                'House Keeping
                Select Case lrModelObject.GetType
                    Case Is = GetType(FBM.FactType)
                        Dim lrORMReadingEditor As frmToolboxORMReadingEditor
                        lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

                        If IsSomething(lrORMReadingEditor) Then

                            '-------------------------------------------------------------------------
                            'Tidy up the ORMFactTypeReading editor if the ORMFactTypeReading is open
                            '-------------------------------------------------------------------------
                            Dim lrPage As FBM.Page = New FBM.Page(Me.mrModel,, "Glossary Page", pcenumLanguage.ORMModel)
                            Dim lrFactTypeInstance As FBM.FactTypeInstance = CType(lrModelObject, FBM.FactType).CloneInstance(lrPage)
                            lrORMReadingEditor.zrFactTypeInstance = lrFactTypeInstance
                            lrORMReadingEditor.zrFactType = lrModelObject
                            lrORMReadingEditor.DataGrid_Readings.DataSource = Nothing
                            lrORMReadingEditor.DataGrid_Readings.Refresh()
                            lrORMReadingEditor.DataGrid_Readings.RefreshEdit()
                            lrORMReadingEditor.DataGrid_Readings.Rows.Clear()
                            Call lrORMReadingEditor.SetupForm(lrPage, lrFactTypeInstance)
                        End If
                End Select

            End If

        End With

    End Sub

    Public Sub DescribeModelElement(ByVal arModelElement As FBM.ModelObject)

        Try
            Me.mrCurrentModelElement = arModelElement

            Select Case Me.mrModel.GetConceptTypeByNameFuzzy(arModelElement.Id, arModelElement.Id)
                Case Is = pcenumConceptType.EntityType
                    Dim lrEntityType As FBM.EntityType
                    lrEntityType = Me.mrModel.GetModelObjectByName(arModelElement.Id)
                    Call Me.VerbaliseEntityType(lrEntityType)

#Region "ORM Verbalisation"
                    '-------------------------------------------------------
                    'ORM Verbalisation
                    '-------------------------------------------------------
                    Dim lrToolboxForm As frmToolboxORMVerbalisation
                    lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
                    If IsSomething(lrToolboxForm) Then
                        Dim lfrmToolboxVerbaliser As frmToolboxORMVerbalisation = CType(lrToolboxForm, frmToolboxORMVerbalisation)
                        lfrmToolboxVerbaliser.verbaliseModelElement(arModelElement)
                    End If
#End Region
                Case Is = pcenumConceptType.ValueType
                    Dim lrValueType As FBM.ValueType
                    lrValueType = Me.mrModel.GetModelObjectByName(arModelElement.Id)
                    Call Me.VerbaliseValueType(lrValueType)

#Region "ORM Verbalisation"
                    '-------------------------------------------------------
                    'ORM Verbalisation
                    '-------------------------------------------------------
                    Dim lrToolboxForm As frmToolboxORMVerbalisation
                    lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
                    If IsSomething(lrToolboxForm) Then
                        Dim lfrmToolboxVerbaliser As frmToolboxORMVerbalisation = CType(lrToolboxForm, frmToolboxORMVerbalisation)
                        lfrmToolboxVerbaliser.verbaliseModelElement(arModelElement)
                    End If
#End Region

                Case Is = pcenumConceptType.FactType
                    Dim lrFactType As FBM.FactType
                    lrFactType = Me.mrModel.GetModelObjectByName(arModelElement.Id)
                    Call Me.VerbaliseFactType(lrFactType)
                Case Is = pcenumConceptType.GeneralConcept
                    Call Me.VerbaliseGeneralConcept(Me.mrModel.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(arModelElement.Id)))
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
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.mrModel.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(arModelElement.Id))
                End If
            Else
                Call Me.DisplayORMDiagramViewForModelObject(arModelElement)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub DisplayORMDiagramViewForModelObject(ByRef arModelObject As FBM.ModelObject)

        'Clear the ORM Diagram View's Page/Diagram
        Call zrFrmORMDiagramViewer.zrPage.ClearFast(True)

        '==============================================================
        Dim loPt As New PointF(50, 50)

        Select Case arModelObject.ConceptType
            Case Is = pcenumConceptType.ValueType
                Dim lrValueType As FBM.ValueType
                lrValueType = arModelObject
                Call zrFrmORMDiagramViewer.zrPage.DropValueTypeAtPoint(lrValueType, loPt)
                Call zrFrmORMDiagramViewer.LoadAssociatedFactTypes(lrValueType)
            Case Is = pcenumConceptType.EntityType
                Dim lrEntityType As FBM.EntityType
                lrEntityType = arModelObject
                Dim larSuptypeRelationship = From EntityType In Me.mrModel.EntityType
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
                lrFactType = arModelObject
                Call zrFrmORMDiagramViewer.zrPage.DropFactTypeAtPoint(lrFactType, loPt, False)
            Case Is = pcenumConceptType.RoleConstraint
                Dim lrRoleConstraint As FBM.RoleConstraint
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                lrRoleConstraint = arModelObject

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

        Me.SplitContainer2.SplitterDistance = 0

        liHeight = Me.WebBrowser.Document.Body.ScrollRectangle.Height + 22 'StatusBar Height=22

        Me.SplitContainer2.SplitterDistance = liHeight
        'zrFrmORMDiagramViewer.Height = Me.SplitContainer2.Panel2.Height
        'zrFrmORMDiagramViewer.Width = Me.SplitContainer2.Panel2.Width
        zrFrmORMDiagramViewer.DiagramView.ZoomFactor = 80
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
        Dim RoleData = From ft In Me.mrModel.FactType,
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
        Dim FactType = From ft In Me.mrModel.FactType,
                            rl In ft.RoleGroup
                       Where rl.JoinedORMObject.Id = arValueType.Id
                       Select ft Distinct

        Dim lrFactType As FBM.FactType


        For Each lrFactType In FactType
            lrVerbaliser.HTW.WriteBreak()
            If lrFactType.FactTypeReading.Count > 0 Then
                Call lrFactType.FactTypeReading(0).GetReadingText(lrVerbaliser, True)
                lrVerbaliser.VerbaliseBlackText(" ")
                lrVerbaliser.VerbaliseModelObjectLightGray(lrFactType)
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

        Try

            lasURLArgument = e.Url.ToString.Split(":")

            If lasURLArgument(0) = "elementid" Then

                lsModelObjectName = lasURLArgument(1)

                Dim lrModelObject As FBM.ModelObject

                lrModelObject = Me.mrModel.GetModelObjectByName(lsModelObjectName)

                Me.mrCurrentModelElement = lrModelObject

                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        Call Me.VerbaliseValueType(lrModelObject)

#Region "ORM Verbalisation"
                        '-------------------------------------------------------
                        'ORM Verbalisation
                        '-------------------------------------------------------
                        Dim lrToolboxForm As frmToolboxORMVerbalisation
                        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
                        If IsSomething(lrToolboxForm) Then
                            Dim lfrmToolboxVerbaliser As frmToolboxORMVerbalisation = CType(lrToolboxForm, frmToolboxORMVerbalisation)
                            lfrmToolboxVerbaliser.verbaliseModelElement(lrModelObject)
                        End If
#End Region
                    Case Is = pcenumConceptType.EntityType
                        Call Me.VerbaliseEntityType(lrModelObject)

#Region "ORM Verbalisation"
                        '-------------------------------------------------------
                        'ORM Verbalisation
                        '-------------------------------------------------------
                        Dim lrToolboxForm As frmToolboxORMVerbalisation
                        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
                        If IsSomething(lrToolboxForm) Then
                            Dim lfrmToolboxVerbaliser As frmToolboxORMVerbalisation = CType(lrToolboxForm, frmToolboxORMVerbalisation)
                            lfrmToolboxVerbaliser.verbaliseModelElement(lrModelObject)
                        End If
#End Region
                    Case Is = pcenumConceptType.FactType
                        Call Me.VerbaliseFactType(lrModelObject)

                        'House Keeping/ORM Reading Editor/ORM Verbaliser
#Region "HouseKeeping"
                        '-------------------------------------------------------
                        'ORM Reading Editor
                        '-------------------------------------------------------
#Region "ORM Reading Editor"
                        Dim lrORMReadingEditor As frmToolboxORMReadingEditor
                        lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

                        If IsSomething(lrORMReadingEditor) Then
                            '-------------------------------------------------------------------------
                            'Tidy up the ORMFactTypeReading editor if the ORMFactTypeReading is open
                            '-------------------------------------------------------------------------
                            Dim lrPage As FBM.Page = New FBM.Page(Me.mrModel,, "Glossary Page", pcenumLanguage.ORMModel)
                            Dim lrFactTypeInstance As FBM.FactTypeInstance = CType(lrModelObject, FBM.FactType).CloneInstance(lrPage)
                            lrORMReadingEditor.zrFactTypeInstance = lrFactTypeInstance
                            lrORMReadingEditor.zrFactType = lrModelObject
                            lrORMReadingEditor.DataGrid_Readings.DataSource = Nothing
                            lrORMReadingEditor.DataGrid_Readings.Refresh()
                            lrORMReadingEditor.DataGrid_Readings.RefreshEdit()
                            lrORMReadingEditor.DataGrid_Readings.Rows.Clear()
                            Call lrORMReadingEditor.SetupForm(lrPage, lrFactTypeInstance)
                        End If
#End Region

                        '-------------------------------------------------------
                        'ORM Verbalisation
                        '-------------------------------------------------------
                        Dim lrToolboxForm As frmToolboxORMVerbalisation
                        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
                        If IsSomething(lrToolboxForm) Then
                            Dim lfrmToolboxVerbaliser As frmToolboxORMVerbalisation = CType(lrToolboxForm, frmToolboxORMVerbalisation)
                            lfrmToolboxVerbaliser.verbaliseModelElement(lrModelObject)
                        End If
#End Region
                    Case Is = pcenumConceptType.RoleConstraint
                        'TBA
                End Select

                Call Me.DisplayORMDiagramViewForModelObject(lrModelObject)

                Me.TextBoxSearch.Text = ""
                Call Me.LoadGlossaryListbox()
                If Me.ListBoxGlossary.FindString(lrModelObject.Id) > -1 Then
                    Me.ListBoxGlossary.SelectedIndex = Me.ListBoxGlossary.FindString(lrModelObject.Id)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

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

    Private Sub CheckBoxShowGeneralConcepts_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxShowGeneralConcepts.CheckedChanged

        Call Me.ShowGlossary(Me.mrModel)

    End Sub

    Private Sub zrModel_ModelUpdated() Handles mrModel.ModelUpdated

        frmMain.ToolStripButton_Save.Enabled = True

    End Sub

    Private Sub ButtonGenerateHTMLGlossary_Click(sender As Object, e As EventArgs) Handles ButtonGenerateHTMLGlossary.Click

        Dim loSaveFileDialog = New SaveFileDialog
        Dim lrGlossaryMaker As New FBM.ORMGlossaryMaker

        loSaveFileDialog.OverwritePrompt = True
        loSaveFileDialog.Filter = "HTML File (*.html)|*.html"
        loSaveFileDialog.FileName = "index.html"

        If loSaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim lsFileLocationName = loSaveFileDialog.FileName

            Dim lsGlossaryHTML = lrGlossaryMaker.Create

            Dim loStreamWriter As StreamWriter
            loStreamWriter = My.Computer.FileSystem.OpenTextFileWriter(lsFileLocationName, False)
            loStreamWriter.WriteLine(lsGlossaryHTML)
            loStreamWriter.Close()

            'Copy the glossaryfiles folder
            Dim loComputer = New Microsoft.VisualBasic.Devices.Computer()
            Try
                loComputer.FileSystem.CreateDirectory(Path.GetDirectoryName(lsFileLocationName) & "\glossaryfiles")
                loComputer.FileSystem.CopyDirectory(Boston.MyPath & "\glossaryfiles", Path.GetDirectoryName(lsFileLocationName) & "\glossaryfiles", True)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            Me.WebBrowser.Navigate(lsFileLocationName)

            'MsgBox(lsGlossaryHTML)

        End If



    End Sub

    Private Sub ContextMenuStripMain_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStripMain.Opening

        Try
            'CodeSafe
            If Me.ListBoxGlossary.SelectedIndex < 0 Then
                Me.ContextMenuStripMain.Hide()
                Exit Sub
            End If

            Dim loModelObject As FBM.ModelObject = Me.ListBoxGlossary.SelectedItem.Tag

            Me.ToolStripMenuItemViewOnPage.DropDownItems.Clear()

            If IsSomething(loModelObject) Then
                '-----------------------------------------------
                'Establish the ContextMenu for the SelectedNode
                '-----------------------------------------------
                Select Case loModelObject.GetType
                    Case Is = GetType(FBM.EntityType)
                        Call Me.LoadPagesForEntityType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                    Case Is = GetType(FBM.ValueType)
                        Call LoadPagesForValueType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                    Case Is = GetType(FBM.FactType)
                        Call LoadPagesForFactType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                End Select
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Sub OpenORMDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            Dim item As ToolStripItem = CType(sender, ToolStripItem)

            '---------------------------------------------------------------------------
            'Find and select the TreeViewNode in the EnterpriseTreeViewer for the Page
            '---------------------------------------------------------------------------
            lr_enterprise_view = item.Tag
            frmMain.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prApplication.WorkingPage = New FBM.Page
            prApplication.WorkingPage = lr_enterprise_view.Tag
            Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Sub LoadPagesForEntityType(ByRef aoMenuStripItem As ToolStripMenuItem, ByVal asEntityTypeId As String)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        Try
            lrModel = Me.mrModel

            aoMenuStripItem.DropDownItems.Clear()

            Dim lsWorkingPageId As String = Nothing
            If prApplication.WorkingPage IsNot Nothing Then
                lsWorkingPageId = prApplication.WorkingPage.PageId
            End If

            Dim larPage = From Page In lrModel.Page
                          From EntityTypeInstance In Page.EntityTypeInstance
                          Where (EntityTypeInstance.Id = asEntityTypeId)
                          Select Page Distinct
                          Order By Page.Name

            If IsSomething(larPage) Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem
                    Dim lr_enterprise_view As tEnterpriseEnterpriseView

                    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               pcenumLanguage.ORMModel,
                                                               Nothing, lrPage.PageId)


                    lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)

                    If IsSomething(lr_enterprise_view) Then
                        '---------------------------------------------------
                        'Add the Page(Name) to the MenuOption.DropDownItems
                        '---------------------------------------------------
                        lr_enterprise_view.FocusModelElement = Me.mrModel.GetModelObjectByName(asEntityTypeId)

                        loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                        loToolStripMenuItem.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                        AddHandler loToolStripMenuItem.Click, AddressOf Me.OpenORMDiagram
                        aoMenuStripItem.Enabled = True
                    End If

                Next
            Else
                Dim loToolStripMenuItem As ToolStripMenuItem
                loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add("Not yet added to a page")
                loToolStripMenuItem.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Warning, ex.StackTrace, True, False, False)
        End Try

    End Sub

    Sub LoadPagesForFactType(ByVal aoMenuStripItem As ToolStripMenuItem, ByVal asFactTypeId As String)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        Try
            lrModel = Me.mrModel

            aoMenuStripItem.DropDownItems.Clear()

            Dim lsWorkingPageId As String = Nothing
            If prApplication.WorkingPage IsNot Nothing Then
                lsWorkingPageId = prApplication.WorkingPage.PageId
            End If

            Dim larPage = From Page In lrModel.Page
                          From FactTypeInstance In Page.FactTypeInstance
                          Where (FactTypeInstance.Id = asFactTypeId)
                          Select Page Distinct
                          Order By Page.Name

            If IsSomething(larPage) Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem
                    Dim lr_enterprise_view As tEnterpriseEnterpriseView

                    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               pcenumLanguage.ORMModel,
                                                               Nothing, lrPage.PageId)

                    lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                    lr_enterprise_view.FocusModelElement = Me.mrModel.GetModelObjectByName(asFactTypeId)
                    loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                    loToolStripMenuItem.Tag = lr_enterprise_view

                    AddHandler loToolStripMenuItem.Click, AddressOf Me.OpenORMDiagram
                Next
            Else
                Dim loToolStripMenuItem As ToolStripMenuItem
                loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add("Not yet added to a page")
                loToolStripMenuItem.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub LoadPagesForValueType(ByVal aoMenuStripItem As ToolStripMenuItem, ByVal asValueTypeId As String)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        Try

            lrModel = Me.mrModel

            aoMenuStripItem.DropDownItems.Clear()

            Dim lsWorkingPageId As String = Nothing
            If prApplication.WorkingPage IsNot Nothing Then
                lsWorkingPageId = prApplication.WorkingPage.PageId
            End If

            Dim larPage = From Page In lrModel.Page
                          From ValueTypeInstance In Page.ValueTypeInstance
                          Where (ValueTypeInstance.Id = asValueTypeId)
                          Select Page Distinct
                          Order By Page.Name

            If IsSomething(larPage) Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem
                    Dim lr_enterprise_view As tEnterpriseEnterpriseView

                    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               pcenumLanguage.ORMModel,
                                                               Nothing, lrPage.PageId)

                    lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                    lr_enterprise_view.FocusModelElement = Me.mrModel.GetModelObjectByName(asValueTypeId)
                    loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                    loToolStripMenuItem.Tag = lr_enterprise_view

                    AddHandler loToolStripMenuItem.Click, AddressOf Me.OpenORMDiagram
                Next
            Else
                Dim loToolStripMenuItem As ToolStripMenuItem
                loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add("Not yet added to a page")
                loToolStripMenuItem.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace, True, False, False)
        End Try

    End Sub

    Private Sub ToolStripMenuItemViewInDiagramSpy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemViewInDiagramSpy.Click

        Try
            Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.mrModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

            Dim lrModelObject As FBM.ModelObject

            Try
                lrModelObject = Me.mrModel.GetModelObjectByName(Me.ListBoxGlossary.SelectedItem.Tag.Id)

                If frmMain.IsDiagramSpyFormLoaded Then
                    prApplication.ActivePages.Find(Function(x) x.Tag.GetType Is GetType(FBM.DiagramSpyPage)).Close()
                End If

                Call frmMain.LoadDiagramSpy(lrDiagramSpyPage, lrModelObject)
            Catch ex As Exception

            End Try


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemRemoveFromModel_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemRemoveFromModel.Click

        Dim lsMessage As String = ""
        Dim lrModelObject As FBM.ModelObject

        Try
            Select Case Me.ListBoxGlossary.SelectedItem.Tag.GetType
                Case Is = GetType(RDS.Table)
                    Dim lrTable As RDS.Table = Me.ListBoxGlossary.SelectedItem.Tag
                    lrModelObject = lrTable.FBMModelElement
                Case Else
                    lrModelObject = Me.ListBoxGlossary.SelectedItem.Tag
            End Select


            lsMessage = "The " & lrModelObject.ConceptType.ToString & ", '" & lrModelObject.Name & "', will be removed from the Model and all Pages."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "This action cannot be undone. Click [OK] to proceed, or [Cancel]."

            If MsgBox(lsMessage, MsgBoxStyle.OkCancel + MsgBoxStyle.Exclamation) = MsgBoxResult.Ok Then

                If lrModelObject.RemoveFromModel(False, True, True) Then

                    '----------------------------------------------------------------------------------------------------
                    'NB If Model.StructureChanged event is triggered by the removal of the ModelObject from the Model,
                    '  the whole tree will be refreshed in this form, so no TreeNode will be selected.
                    '  i.e. The node that would otherwise need to be removed, is likely already removed by the refresh.
                    '----------------------------------------------------------------------------------------------------
                    If Me.ListBoxGlossary.SelectedItem IsNot Nothing Then
                        Me.ListBoxGlossary.Items.RemoveAt(Me.ListBoxGlossary.SelectedIndex)
                    End If

                    Dim lrModel As FBM.Model = lrModelObject.Model
                    Dim larPage As New List(Of FBM.Page)


                    larPage = lrModel.GetPagesContainingModelObject(lrModelObject)

                    'Vm-20180329-The below probably not required.
                    'Dim lrPage As FBM.Page
                    'For Each lrPage In larPage
                    '    lrPage.RemoveModelObject(lrModelObject)
                    'Next
                End If

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub PropertiesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem1.Click

        Try
            Dim lrModelObject As FBM.ModelObject

            Select Case Me.ListBoxGlossary.SelectedItem.Tag.GetType
                Case Is = GetType(RDS.Table)
                    lrModelObject = Me.mrModel.GetModelObjectByName(Me.ListBoxGlossary.SelectedItem.Tag.Name, True)
                Case Is = GetType(FBM.ValueType),
                              GetType(FBM.EntityType),
                              GetType(FBM.FactType)
                    lrModelObject = Me.mrModel.GetModelObjectByName(Me.ListBoxGlossary.SelectedItem.Tag.Id, True)
                Case Else
                    Exit Sub
            End Select

            'CodeSafe 
            If lrModelObject Is Nothing Then Exit Sub

            Dim lrModelElementInstance As Object = Nothing

            Select Case lrModelObject.GetType
                Case Is = GetType(FBM.EntityType)
                    lrModelElementInstance = CType(lrModelObject, FBM.EntityType).CloneInstance(New FBM.Page(Me.mrModel), False, False, Nothing)
                Case Is = GetType(FBM.ValueType)
                    Dim lrValueType As FBM.ValueType = lrModelObject
                    lrModelElementInstance = lrValueType.CloneInstance(New FBM.Page(Me.mrModel), False, False)
                Case Is = GetType(FBM.FactType)
                    Dim lrFactType As FBM.FactType = lrModelObject
                    lrModelElementInstance = lrFactType.CloneInstance(New FBM.Page(Me.mrModel), False, False, 1)
                Case Else
                    Exit Sub
            End Select

            Call frmMain.LoadToolboxPropertyWindow(frmMain.DockPanel.ActivePane, lrModelElementInstance)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ListBoxGlossary_MouseDown(sender As Object, e As MouseEventArgs) Handles ListBoxGlossary.MouseDown

        Try
            Me.ListBoxGlossary.SelectedIndex = Me.ListBoxGlossary.IndexFromPoint(e.X, e.Y)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click

        Try
            Me.TextBoxSearch.Text = ""
            Call Me.ShowGlossary(Me.mrModel)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CheckBoxHideFadedFactTypeNamesVerbalisationView_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxHideFadedFactTypeNamesVerbalisationView.CheckedChanged

        Try
            'If a verbalisation is for a FactType, on the right hand side there is a faded-text link to that FactType.
            'Trouble is...if you copy the verbalisation text to a Word document (for example)...the faded-text FactType names
            'are copied over too. A Business Analysis document might not want that...and a Business Analyst might not
            'want to have to cut those Fact Type names out of the document after they have done a copy/paste.
            Me.mbHideFadedFactTypeNames = Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.Checked

            If Me.mrCurrentModelElement IsNot Nothing Then
                Call Me.DescribeModelElement(Me.mrCurrentModelElement)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ShowInModelDictionaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowInModelDictionaryToolStripMenuItem.Click
        Dim lrModelObject As FBM.ModelObject

        Try
            lrModelObject = Me.mrModel.GetModelObjectByName(Me.ListBoxGlossary.SelectedItem.Tag.Id)

            Dim lrModelDictionaryForm As frmToolboxModelDictionary
            lrModelDictionaryForm = prApplication.GetToolboxForm(frmToolboxModelDictionary.Name)

            If lrModelDictionaryForm Is Nothing Then
                lrModelDictionaryForm = frmMain.LoadToolboxModelDictionary(True)
            End If

            Call lrModelDictionaryForm.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)

            Call lrModelDictionaryForm.FindTreeNode(lrModelObject.Id)

            lrModelDictionaryForm.Show()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextboxSearch_InitiateSearch(asSearchString As String) Handles TextboxSearch.InitiateSearch

        Try
            'Dim items = From it In ListBox1.Items.Cast(Of Object)() _
            '    Where it.ToString().IndexOf(TextBox1.Text, StringComparison.CurrentCultureIgnoreCase) >= 0

            Dim items = From it In Me.mrModel.ModelDictionary
                        Where it.Symbol.IndexOf(TextboxSearch.TextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 _
                        And ((it.isEntityType Or it.isValueType) _
                        Or (Me.CheckBoxShowGeneralConcepts.Checked And it.isGeneralConcept))


            'Dim matchingItemList As List(Of Object) = items.ToList()

            ListBoxGlossary.BeginUpdate()
            ListBoxGlossary.Items.Clear()
            For Each item In items 'matchingItemList
                Dim lrModelObject As FBM.ModelObject = Me.mrModel.GetModelObjectByName(item.Symbol, True)
                ListBoxGlossary.Items.Add(New tComboboxItem(lrModelObject, lrModelObject.Id, lrModelObject))
            Next
            ListBoxGlossary.EndUpdate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextboxSearch_TextBoxCleared() Handles TextboxSearch.TextBoxCleared

        Try
            Call Me.ShowGlossary(Me.mrModel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class