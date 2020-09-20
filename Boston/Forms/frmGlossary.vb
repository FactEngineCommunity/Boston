Imports System.Reflection

Public Class frmGlossary

    Public WithEvents zrModel As FBM.Model
    Private zrFrmORMDiagramViewer As frmDiagramORMForGlossary

    Private Sub frmGlossary_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus

        If prApplication.WorkingModel IsNot Nothing Then
            Me.zrModel = prApplication.WorkingModel
            Me.LabelModelName.Text = Me.zrModel.Name
            Call Me.ShowGlossary(zrModel)
        End If

    End Sub

    Private Sub frmGlossary_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            If prApplication.WorkingModel IsNot Nothing Then
                Me.zrModel = prApplication.WorkingModel
                Me.LabelModelName.Text = Me.zrModel.Name
                Call Me.ShowGlossary(prApplication.WorkingModel)
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
            Dim lrPage As New FBM.Page(zrModel, "GlossaryPage", "GlossaryPage", pcenumLanguage.ORMModel)
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

        Dim items As New List(Of FBM.Concept)

        For Each lrValueType In prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            items.Add(lrValueType.Concept)
        Next

        For Each lrEntityType In prApplication.WorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False)
            items.Add(lrEntityType.Concept)
        Next

        If Me.CheckBoxShowGeneralConcepts.Checked Then
            For Each lrDictionaryEntry In prApplication.WorkingModel.ModelDictionary.FindAll(Function(x) x.isGeneralConcept)
                items.Add(lrDictionaryEntry.Concept)
            Next
        End If

        ListBox1.BeginUpdate()
        ListBox1.Items.Clear()
        For Each item In items 'matchingItemList
            ListBox1.Items.Add(item.Symbol)
        Next
        ListBox1.EndUpdate()

    End Sub

    Private Sub ShowGlossary(ByRef arModel As FBM.Model)

        Dim lrEntityType As FBM.EntityType

        Me.ListBox1.Items.Clear()

        For Each lrEntityType In arModel.EntityType.FindAll(Function(x) x.IsObjectifyingEntityType = False _
                                                                And x.IsMDAModelElement = False)
            Me.ListBox1.Items.Add(lrEntityType.Name)
        Next

        For Each lrValueType In arModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            Me.ListBox1.Items.Add(lrValueType.Name)
        Next

        For Each lrFactType In arModel.FactType.FindAll(Function(x) x.IsObjectified = True And x.IsMDAModelElement = False)
            Me.ListBox1.Items.Add(lrFactType.Name)
        Next

        If Me.CheckBoxShowGeneralConcepts.Checked Then
            For Each lrDictionaryEntry In arModel.ModelDictionary.FindAll(Function(x) x.isGeneralConcept = True)
                Me.ListBox1.Items.Add(lrDictionaryEntry.Symbol)
            Next
        End If

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        'Dim items = From it In ListBox1.Items.Cast(Of Object)() _
        '    Where it.ToString().IndexOf(TextBox1.Text, StringComparison.CurrentCultureIgnoreCase) >= 0

        Dim items = From it In prApplication.WorkingModel.ModelDictionary _
                    Where it.Symbol.IndexOf(TextBox1.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 _
                    And ((it.isEntityType Or it.isValueType) _
                    Or (Me.CheckBoxShowGeneralConcepts.Checked And it.isGeneralConcept))


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
        Dim FactType = From ft In zrModel.FactType, _
                            rl In ft.RoleGroup _
                            Where rl.JoinedORMObject.Id = arEntityType.Id _
                            Select ft Distinct

        For Each lrFactType In FactType


            lrVerbaliser.VerbaliseQuantifierLight("Each ")

            If lrFactType.FactTypeReading.Count = 0 Then
                lrVerbaliser.VerbaliseModelObject(lrFactType)
            Else
                Call lrFactType.FactTypeReading(0).GetReadingText(lrVerbaliser, True)
                lrVerbaliser.HTW.Write(" (")
                lrVerbaliser.VerbaliseModelObjectLight(lrFactType)
                lrVerbaliser.HTW.Write(") ")
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
                            lrFactType.FactTypeReading(1).GetReadingText(lrVerbaliser, True)
                        End If
                    Else
                        If lrFactType.FactTypeReading.Count > 1 Then
                            lrVerbaliser.HTW.WriteBreak()
                            lrVerbaliser.VerbaliseIndent()
                            lrFactType.FactTypeReading(1).GetReadingText(lrVerbaliser, True)
                        End If
                    End If
                End If
                '=======================================================================================                
            End If

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

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub


    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click

        Dim lsSelectedString As String = ""

        If Me.ListBox1.SelectedIndex >= 0 Then
            lsSelectedString = ListBox1.GetItemText(ListBox1.SelectedItem)
        End If

        Dim lrModelObject As FBM.ModelObject

        If lsSelectedString <> "" Then
            lrModelObject = Me.zrModel.GetModelObjectByName(lsSelectedString)
            Select Case zrModel.GetConceptTypeByNameFuzzy(lsSelectedString, lsSelectedString)
                Case Is = pcenumConceptType.EntityType
                    Dim lrEntityType As FBM.EntityType
                    lrEntityType = Me.zrModel.GetModelObjectByName(lsSelectedString)
                    Call Me.VerbaliseEntityType(lrEntityType)

                Case Is = pcenumConceptType.ValueType
                    Dim lrValueType As FBM.ValueType
                    lrValueType = Me.zrModel.GetModelObjectByName(lsSelectedString)
                    Call Me.VerbaliseValueType(lrValueType)

                Case Is = pcenumConceptType.FactType
                    Dim lrFactType As FBM.FactType
                    lrFactType = Me.zrModel.GetModelObjectByName(lsSelectedString)
                    Call Me.VerbaliseFactType(lrFactType)
                Case Is = pcenumConceptType.GeneralConcept
                    Call Me.VerbaliseGeneralConcept(Me.zrModel.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(lsSelectedString)))
            End Select

            '-----------------------------------------------
            If lrModelObject Is Nothing Then
                '-----------------------------------------------------------------
                'Clear the ORMDiagramView
                Me.zrFrmORMDiagramViewer.clear_diagram()

                '==============================================================
                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                If IsSomething(lrPropertyGridForm) Then
                    Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute})
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrModel.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(lsSelectedString))
                End If
            Else
                Call Me.DisplayORMDiagramViewForModelObject(lrModelObject)
            End If

        End If

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
                Dim larSuptypeRelationship = From EntityType In Me.zrModel.EntityType
                                             From SubtypeRelationship In EntityType.SubtypeRelationship
                                             Where EntityType.Id = lrEntityType.Id
                                             Select SubtypeRelationship

                For Each lrSubtypeRelationship In larSuptypeRelationship
                    If Not zrFrmORMDiagramViewer.zrPage.ContainsModelElement(lrSubtypeRelationship.parentEntityType) Then
                        Select Case lrSubtypeRelationship.parentEntityType.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                Call zrFrmORMDiagramViewer.zrPage.DropEntityTypeAtPoint(lrSubtypeRelationship.parentEntityType, New PointF(30, 30))
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
                    Case Is = pcenumRoleConstraintType.RingConstraint, _
                              pcenumRoleConstraintType.EqualityConstraint, _
                              pcenumRoleConstraintType.ExternalUniquenessConstraint, _
                              pcenumRoleConstraintType.ExclusiveORConstraint, _
                              pcenumRoleConstraintType.ExclusionConstraint, _
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
        Dim RoleData = From ft In zrModel.FactType, _
                            rl In ft.RoleGroup, _
                            fct In ft.Fact, _
                            data In fct.Data _
                            Where rl.JoinedORMObject.Id = arValueType.Id _
                            And data.Role.JoinedORMObject.Id = arValueType.Id _
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
        Dim FactType = From ft In zrModel.FactType, _
                            rl In ft.RoleGroup _
                            Where rl.JoinedORMObject.Id = arValueType.Id _
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

            lrModelObject = Me.zrModel.GetModelObjectByName(lsModelObjectName)

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

    Private Sub CheckBoxShowGeneralConcepts_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxShowGeneralConcepts.CheckedChanged

        Call Me.ShowGlossary(Me.zrModel)

    End Sub

    Private Sub zrModel_ModelUpdated() Handles zrModel.ModelUpdated

        frmMain.ToolStripButton_Save.Enabled = True

    End Sub

    Private Sub ButtonGenerateHTMLGlossary_Click(sender As Object, e As EventArgs) Handles ButtonGenerateHTMLGlossary.Click

        Dim loSaveFileDialog = New SaveFileDialog
        Dim lrGlossaryMaker As New FBM.ORMGlossaryMaker

        loSaveFileDialog.Filter = "HTML File (*.html)|*.html"

        If loSaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then

            MsgBox(lrGlossaryMaker.Create)

        End If



    End Sub

End Class