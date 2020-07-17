Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports MindFusion.Diagramming
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports MindFusion.Diagramming.WinForms

Imports HatchBrush = MindFusion.Drawing.HatchBrush
Imports LinearGradientBrush = MindFusion.Drawing.LinearGradientBrush
Imports SolidBrush = MindFusion.Drawing.SolidBrush
Imports TextureBrush = MindFusion.Drawing.TextureBrush
Imports System.IO
Imports System.Threading
Imports System.ComponentModel

Public Class frmDiagramORM

    Private zfrmOverview As frmToolboxOverview = New frmToolboxOverview

    Public WithEvents zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing 'The TreeNode within the Enterprise viewer from which the Page was launched
    Public zrSpecialDragMode As New tSpecialDragMode

    Public Briana As New Viev.Animator.Animator

    Dim pdfExp As New MindFusion.Diagramming.Export.PdfExporter
    Private MorphVector As New List(Of tMorphVector)

    '--------------------------------------------
    'Declare variables for use with ComboBoxFact
    '--------------------------------------------
    Public _row As Integer = -1
    Public _col As Integer = -1
    Public _table As TableNode = Nothing
    Public _ComboBoxFactInstance As New ComboBox
    Public _ComboBoxEntityTypeInstance As New ComboBox
    Public _ComboBoxValueTypeInstance As New ComboBox


    Public Shadows Sub BringToFront(Optional asSelectModelElementId As String = Nothing)

        Call MyBase.BringToFront()

        If asSelectModelElementId IsNot Nothing Then
            Me.Diagram.Selection.Items.Clear()

            Dim lrModelElement As Object = Me.zrPage.GetAllPageObjects.Find(Function(x) x.Id = asSelectModelElementId)
            If lrModelElement IsNot Nothing Then
                lrModelElement.Shape.Selected = True
            End If
        End If

    End Sub

    Private Sub Reset()

        '-----------------------
        'Used for ComboboxFact
        '-----------------------
        Me.ComboBoxFact.Visible = False
        _row = -1
        _col = -1
        _table = Nothing

        Me.ComboBoxEntityType.Visible = False
        Me.ComboBoxValueType.Visible = False
    End Sub

    Private Sub ComboBoxFact_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxFact.Leave
        '-----------------------
        'Used for ComboboxFact
        '-----------------------
        Call Me.Reset()
    End Sub

    Private Sub ComboBoxEntityType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBoxEntityType.KeyDown

        If e.KeyCode = Keys.Enter Then
            Me.DiagramView.Focus()
        End If

    End Sub

    Private Sub ComboBoxEntityType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEntityType.Leave

        Dim lrFactDataInstance As New FBM.FactDataInstance
        Dim lsDebugMessage As String = ""
        Dim lsNewValue As String = ""

        Try
            '--------------------------------------------
            'Get the FactDataInstance being operated on
            '--------------------------------------------
            lrFactDataInstance = _table(_col, _row).Tag

            '-------------------------------------------------------------------------------
            'Check if the FactData has actually changed
            If Me.ComboBoxEntityType.Text = lrFactDataInstance.FactData.Data Then
                Call Me.Reset()
                Me.DiagramView.Focus()
                Exit Sub
            Else
                Call Me.EnableSaveButton()
            End If

            '--------------------------------------
            'Get the EntityType being operated on
            '--------------------------------------
            Dim lrEntityType As New FBM.EntityType
            lrEntityType = lrFactDataInstance.Role.Role.JoinedORMObject


            '----------------------------------------------------------
            'Get the new Data value for the FactDataInstance/FactData
            '----------------------------------------------------------
            If lrEntityType.HasCompoundReferenceMode Then
                lsNewValue = Me.ComboBoxEntityType.SelectedItem.ItemData()
            Else
                lsNewValue = Me.ComboBoxEntityType.Text
            End If

            '--------------------------------------------
            'Update the Instance data for the EntityType
            '--------------------------------------------
            If Not lrEntityType.Instance.Exists(Function(x) x = lsNewValue) Then
                If lrEntityType.Instance.IndexOf(lrFactDataInstance.Data) >= 0 Then
                    lrEntityType.ModifyDataInstance(lrFactDataInstance.Data, lsNewValue)
                End If
            End If

            '---------------------------------------------
            'Update the FactData of the FactDataInstance
            '  and the Data of the FactDataInstance
            '---------------------------------------------
            Dim lrDictionaryEntry = New FBM.DictionaryEntry(Me.zrPage.Model, lsNewValue, pcenumConceptType.Value)
            Dim lsMessage As String = ""

            Dim lsNewInstance = lsNewValue
            Dim lsOldInstance = lrFactDataInstance.Data

            Dim liUsingOldFactDataCount = Aggregate FactType In Me.zrPage.Model.FactType
                                From Fact In FactType.Fact
                                From FactData In Fact.Data
                                Where FactData.Data = lsOldInstance _
                                And Not (FactData Is lrFactDataInstance.FactData)
                                Into Count()

            lrDictionaryEntry = Me.zrPage.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

            If liUsingOldFactDataCount > 0 Then
                '------------------------------------------------------------------------------------------
                'The FactData.Concept is already linked to other FactData.Concept instance in other Facts
                '------------------------------------------------------------------------------------------
                lsMessage = "Other Facts use the old Value ('" & lsOldInstance & "') in Sample Populations."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Do you wish to change all instances of that Value accross all Facts that use that Value?"
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Or would you like to change only this Value instance, '" & lrFactDataInstance.Data & "' to '" & Me.ComboBoxEntityType.Text & "'?"
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Click [Yes] to change all Value instances, or [No] to change only this one instance."

                If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    lrFactDataInstance.FactData.Data = lsNewValue
                    lrFactDataInstance.Data = lrFactDataInstance.FactData.Data
                Else
                    lrDictionaryEntry = New FBM.DictionaryEntry(Me.zrPage.Model, lsNewValue, pcenumConceptType.Value)
                    Call Me.zrPage.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                    lrFactDataInstance.FactData.SwitchConcept(lrDictionaryEntry.Concept, pcenumConceptType.Value)
                    lrFactDataInstance.SwitchConcept(lsNewInstance)
                End If
            Else
                lrDictionaryEntry = New FBM.DictionaryEntry(Me.zrPage.Model, lsNewValue, pcenumConceptType.Value)
                Call Me.zrPage.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                lrFactDataInstance.FactData.SwitchConcept(lrDictionaryEntry.Concept, pcenumConceptType.Value)
                lrFactDataInstance.SwitchConcept(lsNewInstance)
            End If

            lrFactDataInstance.Cell.Text = Me.ComboBoxEntityType.Text

            lrFactDataInstance.Cell.Table.ResizeToFitText(True, False)
            Me.Diagram.Invalidate(True)

            '---------------------------------------------------
            'Remove unwanted DataInstances from the EntityType
            '---------------------------------------------------
            Call lrEntityType.RemoveUnwantedDataInstances()

            Call Me.Reset()

            Dim lrFactTypeInstance As FBM.FactTypeInstance

            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                Call lrFactTypeInstance.FactTable.ResortFactTable()
            Next

            '-------------------------------------
            'Check for PopulationUniquenssErrors
            '-------------------------------------
            Call Me.zrPage.Model.checkForErrors()

            '---------------------------------------------------------------------------------------------
            'Stops bug in Mindfusion where mouse event is unhandled and DiagramView thinks user is
            '  dragging to select DiagramNodes/Shapes
            '---------------------------------------------------------------------------------------------
            Me.DiagramView.Behavior = Behavior.DrawLinks

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ComboBoxValueType_KeyDown(sender As Object, e As KeyEventArgs) Handles ComboBoxValueType.KeyDown

        If e.KeyCode = Keys.Enter Then
            Me.DiagramView.Focus()
        End If

    End Sub

    Private Sub ComboBoxValueType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxValueType.Leave

        Dim lrFactDataInstance As New FBM.FactDataInstance

        If _col <> -1 And _row <> -1 And (Not _table Is Nothing) Then


            Dim lsDebugMessage As String = ""

            lrFactDataInstance = _table(_col, _row).Tag

            '-------------------------------------------------------------------------------
            'Check if the FactData has actually changed
            If Me.ComboBoxValueType.Text = lrFactDataInstance.FactData.Data Then
                Call Me.Reset()
                Me.DiagramView.Focus()
                Exit Sub
            Else
                Me.zrPage.MakeDirty()
                Call Me.EnableSaveButton()
            End If

            '--------------------------------------------
            'Update the Instance data for the ValueType
            '--------------------------------------------
            Dim lrValueType As New FBM.ValueType
            lrValueType = lrFactDataInstance.Role.Role.JoinedORMObject
            If Not lrValueType.Instance.Exists(Function(x) x = Me.ComboBoxValueType.Text) Then
                If lrValueType.Instance.IndexOf(lrFactDataInstance.Data) >= 0 Then
                    lrValueType.Instance(lrValueType.Instance.IndexOf(lrFactDataInstance.Data)) = Me.ComboBoxValueType.Text
                End If
            End If

            '--------------------------------------------------
            'Update the FactData.Data of the FactDataInstance
            '  and the Data of the FactDataInstance
            '--------------------------------------------------
            lrFactDataInstance.FactData.Data = Me.ComboBoxValueType.Text
            lrFactDataInstance.Data = lrFactDataInstance.FactData.Data

            lrFactDataInstance.Cell.Text = Me.ComboBoxValueType.Text

            lrFactDataInstance.Cell.Table.ResizeToFitText(True)



        End If

        Call Me.Reset()

        Dim lrFactTypeInstance As FBM.FactTypeInstance

        For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
            Call lrFactTypeInstance.FactTable.ResortFactTable()
        Next

        '---------------------------------------------------------------------------------------------
        'Stops bug in Mindfusion where mouse event is unhandled and DiagramView thinks user is
        '  dragging to select DiagramNodes/Shapes
        '---------------------------------------------------------------------------------------------
        Me.DiagramView.Behavior = Behavior.DrawLinks
    End Sub

    Private Sub ComboBoxFact_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxFact.SelectedIndexChanged
        '-----------------------
        'Used for ComboboxFact
        '-----------------------

        If _col <> -1 And _row <> -1 And (Not _table Is Nothing) Then

            Dim lrFactDataInstance As New FBM.FactDataInstance
            Dim lsDebugMessage As String = ""

            lrFactDataInstance = _table(_col, _row).Tag

            lsDebugMessage = "Changing FactDataInstance at Row:" & _row & ", Col:" & _col & " in FactTable"
            If IsSomething(lrFactDataInstance.FactType) Then
                lsDebugMessage &= vbCrLf & "FactType.Id: " & lrFactDataInstance.FactType.Id
            Else
                lsDebugMessage &= vbCrLf & "FactType.Id: Nothing"
            End If
            If IsSomething(lrFactDataInstance.Fact) Then
                lsDebugMessage &= vbCrLf & "Fact.Symbol: " & lrFactDataInstance.Fact.Symbol
                lsDebugMessage &= vbCrLf & lrFactDataInstance.Fact.EnumerateAsBracketedFact
            Else
                lsDebugMessage &= vbCrLf & "Fact.Symbol: Nothing"
            End If
            lsDebugMessage &= vbCrLf & "Role.Id :" & lrFactDataInstance.Role.Id
            lsDebugMessage &= vbCrLf & vbCrLf
            If IsSomething(lrFactDataInstance.FactData.FactType) Then
                lsDebugMessage &= vbCrLf & "FactData.FactType.Id: " & lrFactDataInstance.FactData.FactType.Id
            Else
                lsDebugMessage &= vbCrLf & "FactData.FactType.Id: Nothing"
            End If
            If IsSomething(lrFactDataInstance.FactData.Fact) Then
                lsDebugMessage &= vbCrLf & "FactData.Fact.Symbol: " & lrFactDataInstance.FactData.Fact.Symbol
                lsDebugMessage &= vbCrLf & lrFactDataInstance.FactData.Fact.EnumerateAsBracketedFact
            Else
                lsDebugMessage &= vbCrLf & "FactData.Fact.Symbol: Nothing"
            End If
            lsDebugMessage &= vbCrLf & "FactData.Role.Id :" & lrFactDataInstance.FactData.Role.Id
            Call prApplication.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information)

            Call prApplication.ThrowErrorMessage("...." & lrFactDataInstance.Fact.Symbol, pcenumErrorType.Information)

            '---------------------------------------------
            'Update the FactData of the FactDataInstance
            '  and the Data of the FactDataInstance
            '---------------------------------------------
            lrFactDataInstance.FactData.Data = Me.ComboBoxFact.Items(Me.ComboBoxFact.SelectedIndex).ItemData
            lrFactDataInstance.Data = lrFactDataInstance.FactData.Data

            Call prApplication.ThrowErrorMessage("...." & lrFactDataInstance.Fact.Symbol, pcenumErrorType.Information)

            lrFactDataInstance.Cell.Text = Me.ComboBoxFact.SelectedItem.Tag.EnumerateAsBracketedFact()
        End If

        'Me.zrPage.MakeDirty()
        'Me.zrPage.Model.MakeDirty()

    End Sub

    Private Sub ComboBoxEntityType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEntityType.TextChanged

        Try
            If _col <> -1 And _row <> -1 And (Not _table Is Nothing) Then

                Dim lrFactDataInstance As New FBM.FactDataInstance
                Dim lsDebugMessage As String = ""

                lrFactDataInstance = _table(_col, _row).Tag

                ''--------------------------------------------
                ''Update the Instance data for the EntityType
                ''--------------------------------------------
                'Dim lrEntityType As New FBM.EntityType
                'lrEntityType = lrFactDataInstance.Role.Role.JoinedORMObject
                'If Not lrEntityType.Instance.Exists(Function(x) x = Me.ComboBoxEntityType.Text) Then
                '    If lrEntityType.Instance.IndexOf(lrFactDataInstance.Data) >= 0 Then
                '        lrEntityType.ModifyDataInstance(lrFactDataInstance.Data, Me.ComboBoxEntityType.Text)
                '    End If
                'End If

                ''---------------------------------------------
                ''Update the FactData of the FactDataInstance
                ''  and the Data of the FactDataInstance
                ''---------------------------------------------
                'lrFactDataInstance.FactData.Data = Me.ComboBoxEntityType.Text
                'lrFactDataInstance.Data = lrFactDataInstance.FactData.Data

                lrFactDataInstance.Cell.Text = Me.ComboBoxEntityType.Text

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub frmDiagramORM_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

        prApplication.ActivePages.Remove(Me)
        Call frmMain.ShowHideMenuOptions()

    End Sub

    Private Sub frm_ORMModel_page_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave

        '------------------------------------------------
        'Rest the Node and Link colors because User may
        '  be just jumping to a different page.
        '------------------------------------------------
        Call Me.ResetNodeAndLinkColors()

    End Sub

    Private Sub ORMModel_frm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call SetupForm() 'NB DisplayORMModelPage is the method that loads the objects onto the Page. This is called by frmMain.LoadORMModelPage

        Me._ComboBoxFactInstance = Me.ComboBoxFact
        Me._ComboBoxEntityTypeInstance = Me.ComboBoxEntityType
        Me._ComboBoxValueTypeInstance = Me.ComboBoxValueType

        prApplication.ActivePages.Add(Me)

        Call frmMain.ShowHideMenuOptions()

    End Sub

    Sub SetupForm()

        'NB DisplayORMModelPage is the method that loads the objects onto the Page. This is called by frmMain.LoadORMModelPage

        Me.Briana = Me.Animator1

        Me.MenuItemHelpTips.Checked = True

        Me.DiagramView.InplaceEditAcceptOnEnter = True
        Me.DiagramView.InplaceEditCancelOnEsc = True

        Diagram.UndoManager.UndoEnabled = True
        Diagram.Selection.AllowMultipleSelection = True

        Me.ViewFactTablesToolStripMenuItem.Checked = My.Settings.ShowFactTablesOnORMModelLoad

        Call Me.ShowHideToolStripMenuItems()

    End Sub

    Private Sub ShowHideToolStripMenuItems()


    End Sub

    Public Sub LoadAssociatedFactTypes(ByVal aoModelObject As FBM.ModelObject)

        Dim loPoint As New PointF(0, 0)
        Dim lrFactType As FBM.FactType

        Dim larFactType = (From Role In Me.zrPage.Model.Role
                           Where Role.JoinedORMObject.Id = aoModelObject.Id
                           Select Role.FactType).Distinct

        loPoint.X = CInt(Math.Floor((300 - 10 + 1) * Rnd())) + 10
        loPoint.Y = CInt(Math.Floor((300 - 10 + 1) * Rnd())) + 10


        For Each lrFactType In larFactType
            If lrFactType.IsObjectified Then
                Me.zrPage.DropEntityTypeAtPoint(lrFactType.ObjectifyingEntityType, loPoint)
            End If
            Call Me.zrPage.DropFactTypeAtPoint(lrFactType, loPoint, False)
        Next

    End Sub


    Private Sub frm_ORMModel_page_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrmModelExplorer) Then
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
            End If
        End If

        Call Me.SetToolbox()

        Me.LabelHelp.Text = ""

        Call frmMain.hide_unnecessary_forms(Me.zrPage)

    End Sub

    Private Sub frm_ORMModel_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        Try
            Call Directory.SetCurrentDirectory(Richmond.MyPath)


            Dim lrPage As New FBM.Page

            '20180620-Remove if all okay. Commented out because Richmond.PageDataExistsInClipboad caused some sort of unsolveable threading issue
            '  when the form is loading and when this form is the second/third/etc frmDiagramORM form loaded for a Model.
            'If Richmond.PageDataExistsInClipboard(lrPage) Then
            '    If IsSomething(Me.zrPage) Then
            '        If lrPage.CopiedPageId <> Me.zrPage.PageId Then
            '            prApplication.MainForm.PasteToolStripMenuItem.Enabled = True
            '        Else
            '            prApplication.MainForm.PasteToolStripMenuItem.Enabled = False
            '        End If
            '    End If
            'Else
            '    prApplication.MainForm.PasteToolStripMenuItem.Enabled = False
            'End If

            If IsSomething(Me.zoTreeNode) Then
                If IsSomething(frmMain.zfrmModelExplorer) Then
                    frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
                End If
            End If

            '--------------------------------------
            'Set the ZoomFactor on the Main form.
            '--------------------------------------
            prApplication.MainForm.ToolStripComboBox_zoom.SelectedIndex =
                   prApplication.MainForm.ToolStripComboBox_zoom.FindStringExact(CInt(Me.DiagramView.ZoomFactor).ToString & "%")

            If IsSomething(Me.zrPage) Then
                If prApplication.WorkingModel Is Nothing Then
                    prApplication.WorkingModel = Me.zrPage.Model
                End If

                Dim lrModelDictionaryForm As frmToolboxModelDictionary
                lrModelDictionaryForm = prApplication.GetToolboxForm(frmToolboxModelDictionary.Name)

                If IsSomething(lrModelDictionaryForm) Then
                    Call lrModelDictionaryForm.LoadToolboxModelDictionary(Me.zrPage.Language)
                End If
            End If

            If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
                frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
            End If

            '-----------------------------------------------------------------------------------------------------------
            'If the Virtual Analyst toolbox is loaded, set the Brain's Page (of the toolbox) to the Page of this form.
            '-----------------------------------------------------------------------------------------------------------
            If prApplication.ToolboxForms.FindAll(Function(x) x.Name = frmToolboxBrainBox.Name).Count > 0 Then
                prApplication.Brain.Page = Me.zrPage
            End If

            Me.ComboBoxFact.BringToFront()
            Me.ComboBoxEntityType.BringToFront()
            Me.ComboBoxValueType.BringToFront()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub ORMModel_frm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '-------------------------------------------
        'Process the page associated with the form.
        '-------------------------------------------
        If IsSomething(Me.zrPage) Then
            If Me.zrPage.IsDirty And Not Me.zrPage.UserRejectedSave And (Not GetType(FBM.DiagramSpyPage) Is Me.zrPage.GetType) Then

                Dim lsMessage As String = ""

                lsMessage = "Changes have been made to the Page, '" & Me.zrPage.Name & "', in model, '" & Me.zrPage.Model.Name & "'."
                lsMessage.AppendString(vbCrLf & vbCrLf)
                lsMessage.AppendString(" Would you like to save those changes?")

                Select Case MsgBox(lsMessage, MsgBoxStyle.YesNoCancel)
                    Case Is = MsgBoxResult.Yes
                        Me.zrPage.Save()
                    Case Is = MsgBoxResult.No
                        Me.zrPage.UserRejectedSave = True
                    Case Is = MsgBoxResult.Cancel
                        e.Cancel = True
                        pbCancelClosing = True
                        Exit Sub
                End Select
            End If
            Me.zrPage.Form = Nothing
            Me.zrPage.ReferencedForm = Nothing
        End If

        For Each lrValueTypeInstance In Me.zrPage.ValueTypeInstance
            If lrValueTypeInstance.Shape IsNot Nothing Then
                lrValueTypeInstance.Shape.Dispose()
                lrValueTypeInstance.Shape = Nothing
            End If
        Next

        '----------------------------------------------
        'Reset the PageLoaded flag on the Page so
        '  that the User can open the Page again
        '  if they want.
        '----------------------------------------------        
        Me.zrPage.FormLoaded = False

        '----------------------------------------------------------------------------
        'Reset the IsDisplayedAssociated boolen for each lrPage.FactTypeInstance
        '  to False so that the FactTypeInstances will be DisplayedAssociated
        '  the next time the Page is loaded.
        '----------------------------------------------------------------------------
        Call Me.zrPage.ResetFactTypeInstancesForLoading()

        prApplication.WorkingModel = Nothing
        prApplication.WorkingPage = Nothing

        '------------------------------------------------
        'If the 'Properties' window is open, close that.
        '------------------------------------------------
        If Not IsNothing(frmMain.zfrm_properties) Then
            frmMain.zfrm_properties.Close()
            frmMain.zfrm_properties = Nothing
        End If

        Me.Hide()

        frmMain.ToolStripButton_Save.Enabled = False
        frmMain.ToolStripButtonPrint.Enabled = False

    End Sub



    Private Sub ORMDiagramView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.Click

        '---------------------------------------------------------------
        'Update the Working Environment within the Main form so that
        '  the User can see where they are working/viewing.
        '---------------------------------------------------------------
        Dim lr_working_environment As New tWorkingEnvironment

        '---------------------
        'Reset the LabelHelp
        '---------------------
        If Me.zrPage.SelectedObject.Count = 0 Then
            Me.LabelHelp.Text = ""
        End If

        lr_working_environment.Model = Me.zrPage.Model
        lr_working_environment.Page = Me.zrPage

        prApplication.ChangeWorkingEnvironment(lr_working_environment)

        Me.Cursor = Cursors.Default
        Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.None

    End Sub


    Private Sub DiagramView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragDrop

        Dim liInd As Integer = 0
        Dim ls_message As String = ""
        Dim apat1 As AnchorPattern = Nothing
        Dim lrFactType As New FBM.FactType
        Dim lo_FactType_node As New ShapeNode
        Dim lrModel As New FBM.Model
        Dim liModelId As String = ""
        Dim liPageId As String = ""
        Dim loDropTargetNode As New ShapeNode
        Dim lsMessage As String = ""
        Dim loDropPtF As PointF
        Dim G As Graphics

        If Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.ORMSubtypeConnector Then
            'Dim lrE As New System.Windows.Forms.GiveFeedbackEventArgs(DragDropEffects.Copy, False)
            'Call Me.ORMDiagramView.DoDragDrop(New Object, DragDropEffects.Copy)

            'Dim lrE As New System.Windows.Forms.GiveFeedbackEventArgs(DragDropEffects.Copy, False)
            'Call Me.ORMDiagramView.DoDragDrop(New Object, DragDropEffects.Copy)
            'Call Me.ORMDiagramView_GiveFeedback(Me, lrE)

            If (Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.ORMSubtypeConnector) _
                And (Me.zrPage.SelectedObject.Count = 1) Then
                Me.Cursor = Cursors.Default
                Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.None
                Me.zrSpecialDragMode.MouseUpCounter = 0
                Me.zrPage.SelectedObject.Clear()
            End If

            Me.Cursor = New Cursor(My.Resources.Icons.SubtypeConnectorSmallCursor.Handle)
            Exit Sub
        End If


        lrModel = Me.zrPage.Model
        liModelId = Me.zrPage.Model.ModelId
        liPageId = Me.zrPage.PageId

        Dim p As Point = DiagramView.PointToClient(New Point(e.X, e.Y))
        Dim loPt As PointF = DiagramView.ClientToDoc(New Point(p.X, p.Y))


        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            Dim lrDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)
            '-----------------------------------------------------------------------
            'Get the Object being dragged (if there is one).
            '  If the user is dragging from the ModelDictionary form, then 
            '  the DragItem will have a Tag of the ModelObject being dragged.
            '-----------------------------------------------------------------------
            Dim lrDroppedObject As Object
            Dim lrModelObject As FBM.ModelObject
            lrDroppedObject = lrDraggedNode.Tag

            If Not (TypeOf (lrDroppedObject) Is MindFusion.Diagramming.Shape) Then
                '---------------------------------------------------------------------
                'DraggedObject is a ModelObject (from the ModelDictionary form etc),
                ' rather than a Shape Object dragged from the Toolbox.
                '---------------------------------------------------------------------
                If Not TypeOf (lrDraggedNode.Tag) Is FBM.ModelObject Then
                    Exit Sub
                End If
                lrModelObject = lrDraggedNode.Tag

                If (GetType(FBM.DiagramSpyPage) Is Me.zrPage.GetType) Then
                    '---------------------------------------------------------------------------------------------------
                    'Dropping a ModelObject onto the DiagramSpy Page. Need to show that ModelObject in the DiagramSpy.
                    '---------------------------------------------------------------------------------------------------
                    Call Me.ShowSpyInformationForModelObject(lrModelObject)
                    Exit Sub
                End If

                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType 'Entity Type
                        Dim lrEntityType As New FBM.EntityType

                        'VM-20141202-Change the below to assignment of a Clone of the EntityType
                        lrEntityType = lrModelObject
                        lrEntityType.Model = lrModel
                        '-------------------------------------------------------------------------------------
                        'Create an EntityTypeInstance to see if the EntityType is already loaded on the Page
                        '-------------------------------------------------------------------------------------
                        Dim lrEntityTypeInstance As New FBM.EntityTypeInstance(lrModel, pcenumLanguage.ORMModel, lrEntityType.Name, True)

                        If IsSomething(Me.zrPage.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)) Then
                            MsgBox("This Page already contains the Entity Type, '" & lrEntityType.Name & "'.")
                        Else
                            '------------------------------------------------------------------------------------
                            'Check to see if the lrEntityType is already in the ModelDictionary for the lrModel
                            '------------------------------------------------------------------------------------
                            Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrEntityType.Name, pcenumConceptType.EntityType)
                            If lrModel.ModelDictionary.Exists(AddressOf lrDictionaryEntry.Equals) Then
                                '--------------------------------------------------------------------------------------------------------
                                'Do not add the lrDictionaryEntry to the ModelDictionary because already exists in the ModelDictionary,
                                '--------------------------------------------------------------------------------------------------------
                            Else
                                lrModel.AddModelDictionaryEntry(lrDictionaryEntry)
                            End If

                            Dim loEntityTypeNameStringSize As New SizeF
                            G = Me.zrPage.Form.CreateGraphics
                            loEntityTypeNameStringSize = Me.zrPage.Diagram.MeasureString(Trim(Me.Name), Me.zrPage.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                            loDropPtF = New Point(loPt.X - CInt(loEntityTypeNameStringSize.Width / 3), loPt.Y - 5)
                            lrEntityTypeInstance = Me.zrPage.DropEntityTypeAtPoint(lrEntityType, loDropPtF, True)

                            Call lrEntityTypeInstance.showSubtypeRelationships()

                            Me.zrPage.Save()
                            '---------------------------------------
                            'Create the UserAction for the UndoLog
                            '---------------------------------------
                            Dim lrUserAction As New tUserAction(lrEntityTypeInstance, pcenumUserAction.AddPageObjectToPage, Me.zrPage)
                            lrUserAction.PreActionModelObject = lrEntityTypeInstance.Clone(Me.zrPage)
                            lrUserAction.PostActionModelObject = lrEntityTypeInstance.Clone(Me.zrPage)
                            prApplication.AddUndoAction(lrUserAction)
                            frmMain.ToolStripMenuItemUndo.Enabled = True
                        End If
                        Exit Sub
                    Case Is = pcenumConceptType.ValueType 'Value Type
                        Dim lrValueType As New FBM.ValueType

                        'VM-20141202-Change the below to assignment of a Clone of the EntityType
                        lrValueType = lrModelObject
                        lrValueType.Model = lrModel
                        '--------------------------------------------------------------------------------
                        'Create a FactTypeInstance to see if the FactType is already loaded on the Page
                        '--------------------------------------------------------------------------------
                        Dim lrValueTypeInstance As New FBM.ValueTypeInstance(Me.zrPage.Model, Me.zrPage, pcenumLanguage.ORMModel, lrValueType.Name, True)

                        If IsSomething(Me.zrPage.ValueTypeInstance.Find(AddressOf lrValueTypeInstance.Equals)) Then
                            MsgBox("This Page already contains the Value Type, '" & lrValueType.Name & "'.")
                        Else
                            '------------------------------------------------------------------------------------
                            'Check to see if the lrValueType is already in the ModelDictionary for the lrModel
                            '------------------------------------------------------------------------------------
                            Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrValueType.Name, pcenumConceptType.ValueType)
                            If lrModel.ModelDictionary.Exists(AddressOf lrDictionaryEntry.Equals) Then
                                '--------------------------------------------------------------------------------------------------------
                                'Do not add the lrDictionaryEntry to the ModelDictionary because already exists in the ModelDictionary
                                '--------------------------------------------------------------------------------------------------------
                            Else
                                lrModel.AddModelDictionaryEntry(lrDictionaryEntry)
                            End If

                            Dim loEntityTypeNameStringSize As New SizeF
                            G = Me.zrPage.Form.CreateGraphics
                            loEntityTypeNameStringSize = Me.zrPage.Diagram.MeasureString(Trim(Me.Name), Me.zrPage.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                            loDropPtF = New Point(loPt.X - CInt(loEntityTypeNameStringSize.Width / 3), loPt.Y - 5)
                            lrValueTypeInstance = Me.zrPage.DropValueTypeAtPoint(lrValueType, loDropPtF, True)
                            Me.zrPage.Save()
                            '---------------------------------------
                            'Create the UserAction for the UndoLog
                            '---------------------------------------
                            Dim lrUserAction As New tUserAction(lrValueTypeInstance, pcenumUserAction.AddPageObjectToPage, Me.zrPage)
                            lrUserAction.PreActionModelObject = lrValueTypeInstance.Clone(Me.zrPage)
                            lrUserAction.PostActionModelObject = lrValueTypeInstance.Clone(Me.zrPage)
                            prApplication.AddUndoAction(lrUserAction)
                            frmMain.ToolStripMenuItemUndo.Enabled = True

                            Call Me.EnableSaveButton()
                        End If
                        Exit Sub
                    Case Is = pcenumConceptType.FactType 'Fact Type

                        lrFactType = New FBM.FactType

                        lrFactType = lrModelObject

                        '----------------------------------------------------------------------
                        'Change the Model of the FactType to the Model of the current Page.
                        '  The reason for this is that the FactType may have been dragged
                        '  from the ModelDictionary of another Model.
                        'Make a Clone of the FactType so that FactData, RoleConstraints etc
                        '  are all for the current Model.
                        '----------------------------------------------------------------------
                        If lrFactType.Model Is lrModel Then
                            '----------------------------------------------------------------------------
                            'The user has dragged a FactType from the ModelDictionary of the same Model
                            '  as the loaded Page, so leave the Model for the FactType as it is.
                            '----------------------------------------------------------------------------
                        Else
                            lrFactType = lrFactType.Clone(lrModel, True)
                        End If

                        '--------------------------------------------------------------------------------
                        'Create a FactTypeInstance to see if the FactType is already loaded on the Page
                        '--------------------------------------------------------------------------------
                        Dim lrFactTypeInstance As New FBM.FactTypeInstance(lrModel, Me.zrPage, pcenumLanguage.ORMModel, lrFactType.Name, True)

                        If IsSomething(Me.zrPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)) Then
                            MsgBox("This Page already contains the Fact Type, '" & lrFactType.Name & "'.")
                        Else
                            '--------------------------------------------------------------------------------------
                            'Check to see if the ModelObjects joined by the Roles of the FactType are already
                            '  in the ModelDictionary for the lrModel, otherwise add them to the ModelDictionary
                            '--------------------------------------------------------------------------------------
                            Dim lrRole As FBM.Role
                            For Each lrRole In lrFactType.RoleGroup
                                lrModelObject = lrRole.JoinedORMObject
                                Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrModelObject.Name, lrModelObject.ConceptType)
                                If lrModel.ModelDictionary.Exists(AddressOf lrDictionaryEntry.Equals) Then
                                    '--------------------------------------------------------------------------------------------------------
                                    'Do not add the lrDictionaryEntry to the ModelDictionary because already exists in the ModelDictionary,
                                    '--------------------------------------------------------------------------------------------------------
                                Else
                                    lrModel.AddModelDictionaryEntry(lrDictionaryEntry)
                                End If
                            Next

                            lrFactTypeInstance = Me.zrPage.DropFactTypeAtPoint(lrFactType,
                                                                               loPt,
                                                                               Me.ViewFactTablesToolStripMenuItem.Checked,
                                                                               True,
                                                                               True)

                            Me.zrPage.Save()

                            '---------------------------------------
                            'Create the UserAction for the UndoLog
                            '---------------------------------------
                            'Dim lrUserAction As New tUserAction(lrFactTypeInstance, pcenumUserAction.AddPageObjectToPage, Me.zrPage)
                            'lrUserAction.PreActionModelObject = lrFactTypeInstance.Clone(Me.zrPage)
                            'lrUserAction.PostActionModelObject = lrFactTypeInstance.Clone(Me.zrPage)
                            'prApplication.AddUndoAction(lrUserAction)
                            'frmMain.ToolStripMenuItemUndo.Enabled = True

                        End If

                        Exit Sub
                    Case Is = pcenumConceptType.RoleConstraint
                        Dim lrRoleConstraint As New FBM.RoleConstraint
                        lrRoleConstraint = lrModelObject

                        '--------------------------------------------------------------------------------
                        'Create a RoleConstraintInstance to see if the FactType is already loaded on the Page
                        '--------------------------------------------------------------------------------
                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                        If Me.zrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraint.Equals) Then
                            MsgBox("This Page already contains the Role Constraint, '" & lrRoleConstraint.Name & "'.")
                        Else
                            Select Case lrRoleConstraint.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                    lrRoleConstraintInstance = Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                                    Me.zrPage.Save()

                                Case Is = pcenumRoleConstraintType.RingConstraint,
                                          pcenumRoleConstraintType.EqualityConstraint,
                                          pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                          pcenumRoleConstraintType.ExclusiveORConstraint,
                                          pcenumRoleConstraintType.ExclusionConstraint,
                                          pcenumRoleConstraintType.SubsetConstraint
                                    lrRoleConstraintInstance = Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt, True)
                                    Me.zrPage.Save()

                                    '---------------------------------------
                                    'Create the UserAction for the UndoLog
                                    '---------------------------------------
                                    Dim lrUserAction As New tUserAction(lrRoleConstraintInstance, pcenumUserAction.AddPageObjectToPage, Me.zrPage)
                                    lrUserAction.PreActionModelObject = lrRoleConstraintInstance.Clone(Me.zrPage)
                                    lrUserAction.PostActionModelObject = lrRoleConstraintInstance.Clone(Me.zrPage)
                                    prApplication.AddUndoAction(lrUserAction)
                                    frmMain.ToolStripMenuItemUndo.Enabled = True

                                Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                    Call Me.zrPage.DropFrequencyConstraintAtPoint(lrRoleConstraint, loPt, True)
                                    Me.zrPage.Save()
                            End Select
                            'Call Me.zrPage.DropRoleConstraintAtPoint(loPt, lrRoleConstraint.RoleConstraintType)
                        End If

                        Exit Sub
                End Select

                Me.zrPage.DiagramView.Focus()
            End If

            If (GetType(FBM.DiagramSpyPage) Is Me.zrPage.GetType) Then
                MsgBox("You can't drop shapes from the Toolbox onto the DiagramSpy Page.")
                Me.zrPage.DiagramView.Focus()
                Exit Sub
            End If

            Dim liDropTarget As Integer = pcenumShapeDropTarget.Canvas
            '------------------------------------------------------------------------------
            'Check using the Index position within the ShapeListBox list (MindFusion)
            '  Eventually deprecate this manner using the 'ConceptType' above.
            '------------------------------------------------------------------------------
            Dim lrToolboxForm As frmToolbox
            lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)
            If lrDraggedNode.Index >= 0 Then
                If (lrDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount) Then

                    If IsNothing(Diagram.GetNodeAt(loPt)) Then
                        liDropTarget = pcenumShapeDropTarget.Canvas
                    Else
                        liDropTarget = pcenumShapeDropTarget.OtherShapeObject
                        loDropTargetNode = Diagram.GetNodeAt(loPt)
                    End If

                    Select Case lrDraggedNode.Tag.Id
                        Case Is = "Binary Fact Type"
                            Dim lsFactTypeName As String = ""
                            lsFactTypeName = Me.zrPage.Model.CreateUniqueFactTypeName("NewFactType", 0)
                            lrFactType = Me.zrPage.Model.CreateFactType(lsFactTypeName,
                                                                        Nothing,
                                                                        False,
                                                                        True,
                                                                        False,
                                                                        Nothing,
                                                                        False)

                            'lrFactType = New FBM.FactType(Me.zrPage.Model, lsFactTypeName, True)
                            lrFactType.CreateRole(Nothing)
                            lrFactType.CreateRole(Nothing)
                            Dim lrFactTypeInstance As FBM.FactTypeInstance
                            loDropPtF = New Point(loPt.X - 15, loPt.Y - 5)
                            lrFactTypeInstance = Me.zrPage.DropFactTypeAtPoint(lrFactType,
                                                                               loDropPtF,
                                                                               False,
                                                                               False)
                            'Me.zrPage.Save() - 20190929-VM-Caused errors because called removeRolesThatReferenceNothing, removing the FactType just dropped.
                        Case Is = "Subtype Connector"
                            Me.Cursor = New Cursor(My.Resources.Icons.SubtypeConnectorCursor.Handle)
                        Case Is = "Entity Type"

                            Dim lr_combobox_item As New tComboboxItem("0", "[Create New Entity Type]")
                            Dim lrGenericSelection As New tGenericSelection
                            Dim lrEntityType As New FBM.EntityType(Me.zrPage.Model, pcenumLanguage.ORMModel)

                            If My.Settings.ToolboxDragOffersSelection Then
                                If Richmond.DisplayGenericSelectForm(lrGenericSelection, "Entity Type", "MetaModelModelDictionary", "Symbol", "Symbol", "WHERE ModelId = '" & Trim(Me.zrPage.Model.ModelId) & "' AND IsEntityType = " & True, lr_combobox_item) = Windows.Forms.DialogResult.OK Then
                                    '----------------------------------------------------
                                    'Establish a new EntityType for the dropped object
                                    '----------------------------------------------------
                                    If lrGenericSelection.SelectIndex = "0" Then
                                        '--------------------------------------------
                                        'User has elected to create a new EntityType 
                                        '--------------------------------------------
                                        lrEntityType = Me.zrPage.Model.CreateEntityType
                                    Else
                                        lrEntityType.Id = lrGenericSelection.SelectIndex
                                        Call TableEntityType.GetEntityTypeDetails(lrEntityType)
                                    End If
                                End If
                            Else
                                lrEntityType = Me.zrPage.Model.CreateEntityType(Nothing, False)
                            End If
                            '---------------------------------
                            'Drop the EntityType on the Page
                            '---------------------------------
                            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
                            loDropPtF = New Point(loPt.X - 15, loPt.Y - 5)
                            lrEntityTypeInstance = Me.zrPage.DropEntityTypeAtPoint(lrEntityType, loDropPtF)
                            Me.zrPage.Save()

                            Dim lrUserAction As New tUserAction(lrEntityTypeInstance, pcenumUserAction.AddNewPageObjectToPage, Me.zrPage)
                            lrUserAction.PreActionModelObject = lrEntityTypeInstance.Clone(Me.zrPage)
                            lrUserAction.PostActionModelObject = lrEntityTypeInstance.Clone(Me.zrPage)
                            prApplication.AddUndoAction(lrUserAction)
                            frmMain.ToolStripMenuItemUndo.Enabled = True

                            '------------------------------------------------------------------
                        Case Is = "Value Type"

                            Dim lrValueType As New FBM.ValueType(Me.zrPage.Model, pcenumLanguage.ORMModel, "", False)
                            Dim lr_combobox_item As New tComboboxItem("0", "[Create New Value Type]")
                            Dim lrGenericSelection As New tGenericSelection

                            If My.Settings.ToolboxDragOffersSelection Then
                                If Richmond.DisplayGenericSelectForm(lrGenericSelection, "Value Type", "MetaModelModelDictionary", "Symbol", "Symbol", "WHERE ModelId = '" & Trim(Me.zrPage.Model.ModelId) & "' AND IsValueType = " & True, lr_combobox_item) = Windows.Forms.DialogResult.OK Then
                                    If lrGenericSelection.SelectIndex = "0" Then
                                        '--------------------------------------------
                                        'User has elected to create a new ValueType 
                                        '--------------------------------------------
                                        lrValueType = Me.zrPage.Model.CreateValueType
                                    Else
                                        lrValueType.Id = lrGenericSelection.SelectIndex
                                        Call TableValueType.GetValueTypeDetails(lrValueType)
                                    End If
                                End If
                            Else
                                lrValueType = Me.zrPage.Model.CreateValueType(Nothing, False)
                            End If
                            loDropPtF = New Point(loPt.X - 15, loPt.Y - 5)

                            With New WaitCursor
                                Call Me.zrPage.DropValueTypeAtPoint(lrValueType, loDropPtF)
                                Me.zrPage.Save()
                            End With
                            '------------------------------------------------------------------
                        Case Is = "Subtype Connector"

                            Call Me.zrSpecialDragMode.SetSpecialDragMode(pcenumSpecialDragMode.ORMSubtypeConnector)
                            '------------------------------------------------------------------
                        Case Is = "Role"
                            '---------------------------------------------------------------
                            'Check if there are any EntityTypes within the canvas.
                            '  If there are none, then cannot drop a role onto the canvas.
                            '---------------------------------------------------------------
                            If Me.zrPage.Model.EntityType.Count = 0 Then
                                lsMessage = "There are no Entity Types within the current ORM diagram."
                                lsMessage &= vbCrLf & vbCrLf
                                lsMessage &= "You must have at least one Entity Type within the current diagram before adding a Role to the drawing."
                                MsgBox(lsMessage)
                                Exit Sub
                            End If
                            '---------------------------------------------------------------
                            'Check to see if the Role was dropped onto another ModelObject
                            '---------------------------------------------------------------
                            If liDropTarget = pcenumShapeDropTarget.OtherShapeObject Then
                                '------------------------------------------------------
                                'Get the ShapeNode that the ShapeNode was dropped onto
                                '------------------------------------------------------
                                loDropTargetNode = Diagram.GetNodeAt(loPt, True, False)

                                '--------------------------------------------------------
                                'Abort if the ShapeNode was not dropped onto another Role
                                '--------------------------------------------------------
                                If loDropTargetNode.Tag.ConceptType <> pcenumConceptType.Role Then
                                    MsgBox("You must drop new Roles on either the canvas or another Role.")
                                    Exit Sub
                                Else
                                    '------------------------------------------------------------------------------------------------------------
                                    'Set the FactType of the Role to the FactType of the ShapeNode/Role that the ShapeNode/Role was dropped onto
                                    '------------------------------------------------------------------------------------------------------------
                                    loDropTargetNode = loDropTargetNode.Tag.FactType.Shape
                                    Call Me.DropRoleAtPoint(loPt, liDropTarget, loDropTargetNode)
                                End If
                            Else
                                Call Me.DropRoleAtPoint(loPt, liDropTarget, Nothing)
                            End If

                            '-------------------------------------------------------------------------------
                        Case Is = "Ring Constraint"
                            Dim lrRoleConstraint As FBM.RoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.RingConstraint, , , , , False)
                            lrRoleConstraint.RingConstraintType = pcenumRingConstraintType.Acyclic
                            Call Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Me.zrPage.Save()
                            '-------------------------------------------------------------------------------
                        Case Is = "External Uniqueness Constraint"
                            Dim lrRoleConstraint As FBM.RoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.ExternalUniquenessConstraint, , , , , False)
                            Call Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Me.zrPage.Save()
                            '-------------------------------------------------------------------------------
                        Case Is = "Equality Constraint"
                            Dim lrRoleConstraint As FBM.RoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.EqualityConstraint, , , , , False)
                            Call Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Me.zrPage.Save()
                            '-------------------------------------------------------------------------------
                        Case Is = "Subset Constraint"
                            Dim lrRoleConstraint As FBM.RoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.SubsetConstraint, , , , , False)
                            Call Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Me.zrPage.Save()
                            '-------------------------------------------------------------------------------
                        Case Is = "Exclusion Constraint"
                            Dim lrRoleConstraint As FBM.RoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.ExclusionConstraint, , , , , False)
                            Call Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Me.zrPage.Save()
                            '-------------------------------------------------------------------------------
                        Case Is = "Inclusive-OR Constraint"
                            Dim lrRoleConstraint As FBM.RoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.InclusiveORConstraint, , , , , False)
                            Call Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Me.zrPage.Save()
                            '-------------------------------------------------------------------------------
                        Case Is = "Exclusive-OR Constraint"
                            Dim lrRoleConstraint As FBM.RoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.ExclusiveORConstraint, , , , , False)
                            Call Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Me.zrPage.Save()
                            '-------------------------------------------------------------------------------
                        Case Is = "Frequency Constraint"
                            If liDropTarget = pcenumShapeDropTarget.OtherShapeObject Then
                                If loDropTargetNode.Tag.ConceptType = pcenumConceptType.Role Then
                                    Dim larRoleInstance As New List(Of FBM.RoleInstance)
                                    larRoleInstance.Add(loDropTargetNode.Tag)
                                    Call Me.AddFrequencyConstraint(larRoleInstance, loPt)
                                Else
                                    ls_message = "You must drop a Frequency Constraint on the Role to which you would like it to relate."
                                    MsgBox(ls_message)
                                End If
                            Else
                                ls_message = "You must drop a Frequency Constraint on the Role to which you would like it to relate."
                                MsgBox(ls_message)
                            End If
                        Case Is = "Model Note"
                            Dim lrModelNote = Me.zrPage.Model.CreateModelNote
                            Call Me.DropModelNoteAtPoint(lrModelNote, loPt)
                            Me.zrPage.Save()
                    End Select
                End If
            End If
        Else

        End If

        Me.zrPage.DiagramView.Focus()

    End Sub

    Private Function DropModelNoteAtPoint(ByRef arModelNote As FBM.ModelNote, ByVal aoPoint As PointF) As FBM.ModelNoteInstance


        Dim lrModelNoteInstance As FBM.ModelNoteInstance

        '----------------------------------------------------------------------------
        'Add the ModelNote to the Model if it is not already within the Model.
        '----------------------------------------------------------------------------
        If Not Me.zrPage.Model.ModelNote.Exists(AddressOf arModelNote.Equals) Then
            Me.zrPage.Model.ModelNote.Add(arModelNote)
        End If

        '-----------------------------------
        'Create the ModelNoteInstance
        '-----------------------------------
        lrModelNoteInstance = arModelNote.CloneInstance(Me.zrPage)
        lrModelNoteInstance.ModelNote = arModelNote
        lrModelNoteInstance.X = aoPoint.X
        lrModelNoteInstance.Y = aoPoint.Y

        Call lrModelNoteInstance.DisplayAndAssociate()

        '---------------------------------------------
        'Add the ModelNoteInstance to the Page.
        '---------------------------------------------
        Me.zrPage.ModelNoteInstance.Add(lrModelNoteInstance)

        Call Me.zrPage.MakeDirty()

        Return lrModelNoteInstance

    End Function

    Private Sub DropRoleAtPoint(ByVal aoPt As PointF, ByVal aiDropTarget As pcenumShapeDropTarget, ByRef aoDropTargetNode As ShapeNode)

        Dim lrRole As New FBM.Role
        Dim lrRoleInstance As New FBM.RoleInstance
        Dim lrFactType As New FBM.FactType
        Dim lrFactTypeInstance As FBM.FactTypeInstance
        Dim loNode As New ShapeNode

        Try
            '------------------------------------------------------------------------------------------------------------------
            'Create the corresponding FactType and add it to the model.
            '  Name is set within the RoleInstance form by the ObjectTypes that the User selects to join the RoleInstance to.
            '------------------------------------------------------------------------------------------------------------------
            If IsSomething(aoDropTargetNode) Then
                lrFactType = aoDropTargetNode.Tag.FactType
                lrFactType = Me.zrPage.Model.FactType.Find(Function(x) x.Id = lrFactType.Id)
                '--------------------------------
                'Create a Role for the FactType
                '--------------------------------
                'lrRole = lrFactType.CreateRole()
                lrRole = New FBM.Role()
                lrRole.Model = Me.zrPage.Model
                lrRole.FactType = lrFactType
                lrFactTypeInstance = New FBM.FactTypeInstance
                lrFactTypeInstance.Id = lrFactType.Id
                lrFactTypeInstance = Me.zrPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
            Else
                '------------------------------------------------------------------------------------
                'The Role was dropped on the Canvas. 
                '  NB The(FactType.Name) will be set automatically when the User selects the 
                '  ModelObject to which the new Role/RoleInstance is joined to.
                '------------------------------------------------------------------------------------
                Dim lsFactTypeName As String = Me.zrPage.Model.CreateUniqueFactTypeName("NewFactType", 0)
                lrFactType = New FBM.FactType(Me.zrPage.Model, lsFactTypeName, False)
                '--------------------------------
                'Create a Role for the FactType
                '--------------------------------
                lrRole = lrFactType.CreateRole()

                '20200515-VM-Remove if everything okay.
                '------------------------------------------------------------------------------------------------------------------------------
                'Create a new FactTypeInstance for the ORMModel.
                '  NB IMPORTANT: It may turn out that the FactTypeInstance already exists within the model
                '  in which case the FactTypeInstance (created below) must get the details of the existing FactTypeInstance (from the database)
                '------------------------------------------------------------------------------------------------------------------------------
                'lrFactTypeInstance = lrFactType.CloneInstance(Me.zrPage, False)
            End If

            '------------------------------------------------------
            'Get user entered details about the role being dropped
            '------------------------------------------------------
            Dim lrRoleInstanceForm As New frmCRUDRoleInstance

            lrRoleInstanceForm.zrBaseFactType = lrFactType
            lrRoleInstanceForm.zrRole = lrRole
            If IsSomething(aoDropTargetNode) Then
                lrRoleInstanceForm.zbExtendingExistingFactType = True
            End If
            If lrRoleInstanceForm.ShowDialog(lrRole, Me.zrPage) = Windows.Forms.DialogResult.OK Then
                '-------------------------------------------------------------
                'The user successfully entered the details for the new Role.
                '-------------------------------------------------------------
                If aiDropTarget = pcenumShapeDropTarget.Canvas Then
                    '----------------------------------------------------------
                    'Role dropped on canvas. i.e. Not dropped onto another Role
                    '---------------------------------------------------------------------
                    If Me.zrPage.Model.FactType.Exists(AddressOf lrFactType.EqualsByName) Then
                        '-----------------------------------------------------------
                        'The FactType.Name already exists within the database
                        '  so create a new FactType.Name
                        '-----------------------------------------------------------
                        lrFactType.SetName(Me.zrPage.Model.CreateUniqueFactTypeName(Viev.Strings.RemoveWhiteSpace(lrFactType.Name), 0))
                    End If

                    '----------------------------------------------
                    'DisplayAndAssociate the new FactTypeInstance
                    '----------------------------------------------                    
                    aoPt.X -= 3
                    aoPt.Y -= 3
                    'Call lrFactTypeInstance.DisplayAndAssociate( Me.ViewFactTablesToolStripMenuItem.Checked, My.Settings.ShowFactTypeNamesOnORMModelLoad)

                    '---------------------------------------------------------------------
                    'Add a FactType to the ORMModel and the FactTypeInstance to the Page
                    '---------------------------------------------------------------------
                    Me.zrPage.Model.AddFactType(lrFactType, True, True)

                    lrFactTypeInstance = Me.zrPage.DropFactTypeAtPoint(lrFactType,
                                                                       aoPt,
                                                                       Me.ViewFactTablesToolStripMenuItem.Checked,
                                                                       True,
                                                                       True)

                    Call lrFactTypeInstance.SortRoleGroup()
                    Me.zrPage.FactTypeInstance.AddUnique(lrFactTypeInstance)

                    '----------------------------------                    
                    'Initiate a new instances of the RoleInstance struct
                    '-----------------------------------------------------
                    lrRoleInstance = lrFactTypeInstance.RoleGroup(0)
                    'Add the RoleInstance to the Page
                    Me.zrPage.RoleInstance.AddUnique(lrRoleInstance)
                Else
                    '---------------------------
                    'Dropped onto another Role
                    '---------------------------            
                    '-----------------------------------------------------
                    'Initiate a new instances of the RoleInstance struct
                    '-----------------------------------------------------
                    Call lrFactType.AddRole(lrRole, True, True)
                    'lrRoleInstance = lrRole.CloneInstance(Me.zrPage, False)
                    'lrFactTypeInstance.RoleGroup.Add(lrRoleInstance)

                    '--------------------------------------------
                    'Assign the RoleInstance.FactType[Instance]
                    '--------------------------------------------
                    'lrRoleInstance.FactType = lrFactTypeInstance

                    'lrRoleInstance.DisplayAndAssociate(lrRoleInstance.FactType)
                    'lrRoleInstance.FactType.SortRoleGroup()
                End If 'Dropped on existing Role

                Call Me.zrPage.MakeDirty()

            Else
                '----------------------------------------------
                'User cancelled the addition of the new Role.
                '----------------------------------------------
                If aiDropTarget = pcenumShapeDropTarget.OtherShapeObject Then
                    lrFactType.RemoveRole(lrRole, False, False)
                End If

            End If 'RoleForm closed with [OK]

        Catch ex As Exception
            Dim lsMessage As String
            lsMessage = "Error: frmDiagramORM.DropRoleAtPoint"
            lsMessage &= vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ORMDiagramView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragOver

        Try
            Dim p As Point = DiagramView.PointToClient(New Point(e.X, e.Y))
            Dim lo_point As PointF = DiagramView.ClientToDoc(New Point(p.X, p.Y))
            Dim loNode As ShapeNode
            Dim loMouseLocation As New Point

            If Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.ORMSubtypeConnector Then
                loMouseLocation = New Point(e.X, e.Y)

                loMouseLocation = PointToClient(loMouseLocation)
                lo_point = Me.DiagramView.ClientToDoc(loMouseLocation)

                '--------------------------------------------------
                'Just to be sure...set the Richmond.WorkingProject
                '--------------------------------------------------
                prApplication.WorkingPage = Me.zrPage

                If IsSomething(Diagram.GetNodeAt(lo_point)) Then
                    '----------------------------
                    'Mouse is over an ShapeNode
                    '----------------------------
                    loNode = Diagram.GetNodeAt(lo_point)
                    '------------------------------------
                    '  enforce the selection of the object
                    '------------------------------------                
                    Select Case loNode.Tag.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                            lrEntityTypeInstance = loNode.Tag
                            lrEntityTypeInstance.Shape.Selected = True
                            Exit Sub
                        Case Else
                    End Select
                End If
            End If

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
                            Case Is = "Subtype Connector"

                                e.Effect = DragDropEffects.Copy
                                Me.LabelHelp.Text = "Hint: Release the left Mouse button first, then drag from the child Entity Type to the Parent Entity Type."

                                Call Me.zrSpecialDragMode.SetSpecialDragMode(pcenumSpecialDragMode.ORMSubtypeConnector)

                                Dim lrE As New System.Windows.Forms.GiveFeedbackEventArgs(DragDropEffects.Copy, False)
                                Call Me.DiagramView.DoDragDrop(New Object, DragDropEffects.Copy)
                                Call Me.ORMDiagramView_GiveFeedback(Me, lrE)

                                If (Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.ORMSubtypeConnector) _
                                    And Me.zrSpecialDragMode.MouseUpCounter = 1 Then
                                    Me.zrPage.SelectedObject.Clear()
                                End If

                            Case Is = "Frequency Constraint"
                                Me.LabelHelp.Text = "Drop the Frequency Constraint on a Role."
                                e.Effect = DragDropEffects.Copy
                            Case Is = "Role"
                                If IsSomething(Diagram.GetNodeAt(lo_point)) Then
                                    '-------------------------------------------
                                    'Change the color of any object that the
                                    '  item is dragged over. Visual aid.
                                    '-------------------------------------------
                                    loNode = Diagram.GetNodeAt(lo_point)

                                    Dim lrDraggedOverModelObject As FBM.ModelObject
                                    lrDraggedOverModelObject = loNode.Tag
                                    Select Case lrDraggedOverModelObject.ConceptType
                                        Case Is = pcenumConceptType.EntityType,
                                                  pcenumConceptType.ValueType,
                                                  pcenumConceptType.FactType,
                                                  pcenumConceptType.FactTypeReading
                                            e.Effect = DragDropEffects.None
                                        Case Is = pcenumConceptType.Role
                                            loNode.Pen = New MindFusion.Drawing.Pen(Color.Brown)
                                            e.Effect = DragDropEffects.Copy
                                        Case Else
                                            e.Effect = DragDropEffects.Copy
                                    End Select
                                Else
                                    e.Effect = DragDropEffects.Copy
                                End If
                            Case Else
                                e.Effect = DragDropEffects.Copy
                                Call Me.zrSpecialDragMode.SetSpecialDragMode(pcenumSpecialDragMode.None)
                                Me.LabelHelp.Text = ""
                        End Select
                    End If
                End If
            ElseIf e.Data.GetDataPresent("System.Windows.Forms.TreeNode", False) Then
                '-------------------------------------------------------------------
                'If the item is a TreeNode item from the EnterpriseTreeView form
                '-------------------------------------------------------------------
                e.Effect = DragDropEffects.Copy
            ElseIf e.Data.GetDataPresent(GetType(FBM.EntityType).ToString, False) Then
                Call Me.zrSpecialDragMode.SetSpecialDragMode(pcenumSpecialDragMode.None)
                e.Effect = DragDropEffects.Copy
            ElseIf e.Data.GetDataPresent(GetType(FBM.ValueType).ToString, False) Then
                Call Me.zrSpecialDragMode.SetSpecialDragMode(pcenumSpecialDragMode.None)
                e.Effect = DragDropEffects.Copy
            ElseIf e.Data.GetDataPresent(GetType(FBM.FactType).ToString, False) Then
                Call Me.zrSpecialDragMode.SetSpecialDragMode(pcenumSpecialDragMode.None)
                e.Effect = DragDropEffects.Copy
            Else
                'e.Effect = DragDropEffects.None
            End If


            'If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            '    '-------------------------------------------
            '    'Change the color of any object that the
            '    '  item is dragged over. Visual aid.
            '    '-------------------------------------------
            '    loNode = Diagram.GetNodeAt(lo_point)
            '    loNode.Pen = New MindFusion.Drawing.Pen(Color.Brown)
            'End If

            Dim lrRoleInstance As FBM.RoleInstance
            For Each lrRoleInstance In Me.zrPage.RoleInstance
                loNode = Diagram.GetNodeAt(lo_point)
                If lrRoleInstance.Shape Is loNode Then
                    '-------------------------
                    'Already coloured brown.
                    '-------------------------
                Else
                    lrRoleInstance.Shape.Pen = New MindFusion.Drawing.Pen(Color.Black)
                End If
            Next

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub mnuOption_OverviewTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_OverviewTool.Click


        If mnuOption_OverviewTool.Checked Then

            zfrmOverview.SetDocument(DiagramView)
            zfrmOverview.Owner = Me

            zfrmOverview.Show()
        Else
            zfrmOverview.Hide()
        End If


    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click

        DiagramView.PrintOptions.DocumentName = "Flowchart"
        DiagramView.PrintOptions.EnableImages = False
        DiagramView.PrintOptions.EnableInterior = True
        DiagramView.PrintOptions.EnableShadows = True
        DiagramView.PrintOptions.Scale = 50
        DiagramView.PrintPreviewEx()

    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click

        DiagramView.PrintOptions.DocumentName = "Flowchart"
        DiagramView.PrintOptions.EnableImages = False
        DiagramView.PrintOptions.EnableInterior = True
        DiagramView.PrintOptions.EnableShadows = True
        DiagramView.PrintOptions.Scale = 100
        DiagramView.Print()

    End Sub

    Private Sub ExportToToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToToolStripMenuItem.Click

        SaveFileDialog.DefaultExt = "pdf"
        SaveFileDialog.Filter = "PDF files|*.pdf"
        If SaveFileDialog.ShowDialog() = DialogResult.OK Then
            pdfExp.Export(DiagramView.Diagram, SaveFileDialog.FileName)
        End If

    End Sub

    Private Sub ExportToSVGToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToSVGToolStripMenuItem.Click

        SaveFileDialog.DefaultExt = "svg"
        SaveFileDialog.Filter = "SVG files|*.svg"
        If SaveFileDialog.ShowDialog() = DialogResult.OK Then

            Dim svgExp As MindFusion.Diagramming.Export.SvgExporter =
             New MindFusion.Diagramming.Export.SvgExporter()
            svgExp.Export(DiagramView.Diagram, SaveFileDialog.FileName)
        End If

    End Sub

    Private Sub ExportToToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToToolStripMenuItem1.Click

        SaveFileDialog.DefaultExt = "png"
        SaveFileDialog.Filter = "PNG files|*.png"
        If SaveFileDialog.ShowDialog() = DialogResult.OK Then

            Dim image As Image = Diagram.CreateImage
            image.Save(SaveFileDialog.FileName)
            image.Dispose()
        End If

    End Sub


    Private Sub ExportToToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToToolStripMenuItem2.Click

        SaveFileDialog.DefaultExt = "vdx"
        SaveFileDialog.Filter = "Visio files|*.vdx"
        If SaveFileDialog.ShowDialog() = DialogResult.OK Then
            Dim visioExp As MindFusion.Diagramming.Export.VisioExporter = New MindFusion.Diagramming.Export.VisioExporter()
            'visioExp.TemplatePath = Application.ExecutablePath
            visioExp.CreateVisioGroups = True
            visioExp.ExportImagesAsGroups = True
            visioExp.ExportTablesAsGroups = True
            visioExp.Export(DiagramView.Diagram, SaveFileDialog.FileName)
        End If

    End Sub

    Private Sub ExportToDXFToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToDXFToolStripMenuItem.Click

        SaveFileDialog.DefaultExt = "dxf"
        SaveFileDialog.Filter = "Visio files|*.dxf"
        If SaveFileDialog.ShowDialog() = DialogResult.OK Then
            Dim dxfExp As MindFusion.Diagramming.Export.DxfExporter = New MindFusion.Diagramming.Export.DxfExporter()
            dxfExp.ExportImages = True
            dxfExp.ExportTextAsMultiline = True
            dxfExp.Export(DiagramView.Diagram, SaveFileDialog.FileName)
        End If

    End Sub

    Private Sub ImportVisioDiagramToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportVisioDiagramToolStripMenuItem.Click

        OpenFileDialog.DefaultExt = "vdx"
        OpenFileDialog.Filter = "Visio files|*.vdx"
        If OpenFileDialog.ShowDialog() = DialogResult.OK Then
            Dim visioImp As MindFusion.Diagramming.Import.VisioImporter = New MindFusion.Diagramming.Import.VisioImporter()
            visioImp.ImportEntitiesAsTables = True
            MsgBox(OpenFileDialog.FileName)
            visioImp.ImportPage(OpenFileDialog.FileName, DiagramView.Diagram, 0)
        End If

    End Sub

    Private Sub Diagram_CellClicked(ByVal sender As Object, ByVal e As MindFusion.Diagramming.CellEventArgs) Handles Diagram.CellClicked

        Dim lrFactTable As New FBM.FactTable
        Dim lrFactDataInstance As New FBM.FactDataInstance

        '--------------------------------------------------
        'See also: FactTypeInstance.Page.TableCellClicked
        '--------------------------------------------------

        Try

            lrFactTable = e.Table.Tag
            lrFactTable.ResetBlackCellText()
            lrFactTable.SelectedRow = e.Row + 1

            lrFactDataInstance = e.Cell.Tag

            For Each lrFactDataInstance In lrFactDataInstance.Fact.Data
                lrFactDataInstance.Cell.TextColor = Color.Blue
                lrFactDataInstance.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)
            Next

            If e.MouseButton = MouseButton.Left Then
                '-------------------------------------------------
                'Taken care of in Me.ORMDiagramView.NodeSelected
                '-------------------------------------------------
                Me.zrPage.SelectedObject.Add(lrFactDataInstance.Fact)

            ElseIf e.MouseButton = MouseButton.Right Then
                Me.DiagramView.ContextMenuStrip = Nothing
                Me.Refresh()
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_FactTable
                Me.Refresh()

                Me.zrPage.SelectedObject.Add(lrFactTable)
                DeleteRowToolStripMenuItem.Enabled = True
                Me.DeleteRowFactFromPageAndModelToolStripMenuItem.Enabled = True
            End If

            '==============================================================================================
            Dim lrModel As FBM.Model
            Dim lrFact As New FBM.Fact
            Dim lrFactInstance As New FBM.FactInstance
            Dim lo_menu_option As ToolStripItem
            If lrFactTable.SelectedRow > 0 Then
                lrFactInstance = lrFactTable.FactTypeInstance.Fact(lrFactTable.SelectedRow - 1)
                lrFact = lrFactInstance.Fact
                lrModel = lrFact.Model
                'ToolStripMenuItemFactModelErrors
                Me.ToolStripMenuItemFactModelErrors.DropDownItems.Clear()
                Dim lrModelError As FBM.ModelError
                If lrFact.ModelError.Count > 0 Then
                    Me.ToolStripMenuItemFactModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
                    For Each lrModelError In lrFact.ModelError
                        lo_menu_option = Me.ToolStripMenuItemFactModelErrors.DropDownItems.Add(lrModelError.Description)
                        lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
                    Next
                Else
                    Me.ToolStripMenuItemFactModelErrors.Image = My.Resources.MenuImages.Cloud216x16
                    lo_menu_option = Me.ToolStripMenuItemFactModelErrors.DropDownItems.Add("There are no Model Errors for this Fact.")
                    lo_menu_option.Image = My.Resources.MenuImages.Cloud216x16
                End If
            End If
            '==============================================================================================

            '----------------
            'Verbalise Fact
            '----------------
            Dim lrToolboxForm As frmToolboxORMVerbalisation
            lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
            If IsSomething(lrToolboxForm) Then
                lrToolboxForm.zrModel = Me.zrPage.Model
                If lrFactTable.FactTypeInstance.Fact.Count > 0 Then
                    lrFactDataInstance = lrFactTable.TableShape.Item(0, lrFactTable.SelectedRow - 1).Tag
                    lrFact = lrFactDataInstance.Fact.Fact
                    Call lrToolboxForm.VerbaliseFact(lrFact)
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            lsMessage = "Error: frmDiagramORM.Diagram.CellClicked"
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub Diagram_CellTextEdited(ByVal sender As Object, ByVal e As MindFusion.Diagramming.EditCellTextEventArgs) Handles Diagram.CellTextEdited
        '----------------------------------------
        'The Cell of a FactTable has been edited
        '----------------------------------------``

        Dim liInd, liInd2 As Integer
        Dim lrTableNode As New TableNode
        Dim lr_cell As MindFusion.Diagramming.TableNode.Cell

        If e.NewText = e.OldText Then
            '--------------------------------------------
            ' No change to the RoleData so exit the sub
            '--------------------------------------------
            Exit Sub
        End If

        lrTableNode = e.Table

        '--------------------------------
        'Resize the cells of the table.
        '--------------------------------
        If lrTableNode.RowCount > 0 Then
            Call lrTableNode.ResizeToFitText(True, True)
        End If

        '--------------------
        'Update the model
        '--------------------
        Dim lrFactDataInstance As New FBM.FactDataInstance
        lrFactDataInstance = e.Cell.Tag
        If IsSomething(lrFactDataInstance) Then
            lrFactDataInstance.ChangeData(e.NewText)
        Else
            '----------------------------------------------------------
            'the FactDataInstance is not yet set. If not an error/bug,
            '  is simply that this is a new Fact and there simply 
            '  does not exist a FactDataInstance for the Cell yet.
            '----------------------------------------------------------
        End If

        Me.zrPage.MakeDirty()
        Me.zrPage.Model.MakeDirty()


        '---------------------------------------------------------------------
        'Get rid of any Rows with no data
        '---------------------------------------------------------------------
        If e.Row = (e.Table.RowCount - 1) And (StrComp("", e.NewText) <> 0) Then
            '-----------------------------------------------------------
            'User entering data into new Fact (first Fact in FactTable)
            '-----------------------------------------------------------
            Exit Sub
        End If

        For liInd = lrTableNode.RowCount - 1 To 0 Step -1
            For liInd2 = 0 To lrTableNode.ColumnCount - 1
                lr_cell = lrTableNode.Item(liInd2, liInd)
                If lr_cell.Text <> "" Then
                    Exit Sub
                End If
            Next
            lrTableNode.RowCount = liInd
        Next

    End Sub

    Private Sub Diagram_Clicked(sender As Object, e As DiagramEventArgs) Handles Diagram.Clicked


    End Sub


    Private Sub Diagram_DrawAnchorPoint(sender As Object, e As DrawAnchorPointEventArgs) Handles Diagram.DrawAnchorPoint

    End Sub

    Private Sub Diagram_DrawBackground(ByVal sender As Object, ByVal e As MindFusion.Diagramming.DiagramEventArgs) Handles Diagram.DrawBackground

        Dim lrModelObject As Object
        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        If (Me.zrPage.SelectedObject.Count = 1) And Me.zrPage.IsInvalidated Then
            lrModelObject = Me.zrPage.SelectedObject(0)
            Dim lrToolboxForm As frmToolboxORMVerbalisation
            lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
            If IsSomething(lrToolboxForm) Then
                lrToolboxForm.zrModel = Me.zrPage.Model
                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        Call lrToolboxForm.VerbaliseEntityType(lrModelObject.EntityType)
                    Case Is = pcenumConceptType.ValueType
                        Call lrToolboxForm.VerbaliseValueType(lrModelObject.ValueType)
                    Case Is = pcenumConceptType.FactType
                        Call lrToolboxForm.VerbaliseFactType(lrModelObject.FactType)
                    Case Is = pcenumConceptType.RoleConstraint
                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                        lrRoleConstraintInstance = lrModelObject
                        Select Case lrRoleConstraintInstance.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.SubsetConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintSubset(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.EqualityConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintEqualityConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.ExclusionConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintExclusionConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.InclusiveORConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintInclusiveORConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.ExclusiveORConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintExclusiveORConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintUniquenessConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintFrequencyConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.RingConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
                        End Select
                End Select
            End If
        End If

        Me.zrPage.IsInvalidated = False

    End Sub

    Private Sub Diagram_LinkCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkCreated

        Dim lrModelObject As FBM.ModelObject
        Dim lrTargetModelObject As FBM.ModelObject
        Dim lrOriginModelObject As FBM.ModelObject
        Dim lrLink As DiagramLink
        Dim lrLinkToRemove As New DiagramLink(Me.Diagram)

        Try
            lrModelObject = e.Link.Tag
            lrTargetModelObject = e.Link.Destination.Tag
            lrOriginModelObject = e.Link.Origin.Tag

            Select Case lrOriginModelObject.ConceptType
                Case Is = pcenumConceptType.Role

                    With New WaitCursor
                        Dim lrRoleInstance As New FBM.RoleInstance
                        lrRoleInstance = lrModelObject

                        '------------------------------------------------------------------------------------------------------
                        'Remove the link either way, because if lrTargetModelObject is something, then RoleInstance.ResetLink
                        '  will recreate the link, and lrRoleInstance may not be the only RoleInstance in the Model that
                        '  will have RoleInstance.ResetLink method called.
                        '------------------------------------------------------------------------------------------------------
                        Me.Diagram.Links.Remove(e.Link)

                        If lrRoleInstance.FactType.IsLinkFactType Then
                            MsgBox("You cannot directly reassign the Role of a Link Fact Type.")
                            Exit Sub
                        End If

                        If lrTargetModelObject Is Nothing Then
                            lrRoleInstance = lrModelObject
                            lrRoleInstance.Link.Pen.Color = Color.Black

                            Exit Sub
                        Else
                            Select Case lrTargetModelObject.ConceptType
                                Case Is = pcenumConceptType.ValueType
                                    Dim lrValueTypeInstance As FBM.ValueTypeInstance = lrTargetModelObject
                                    lrRoleInstance.Role.ReassignJoinedModelObject(lrValueTypeInstance.ValueType, True)
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance = lrTargetModelObject
                                    lrRoleInstance.Role.ReassignJoinedModelObject(lrEntityTypeInstance.EntityType, True)
                                Case Is = pcenumConceptType.EntityTypeName
                                    Dim lrEntityTypeNameInstance As FBM.EntityTypeName = lrTargetModelObject
                                    lrRoleInstance.Role.ReassignJoinedModelObject(lrEntityTypeNameInstance.EntityTypeInstance.EntityType, True)
                                Case Is = pcenumConceptType.FactType
                                    Dim lrFactTypeInstance As FBM.FactTypeInstance = lrTargetModelObject
                                    lrRoleInstance.Role.ReassignJoinedModelObject(lrFactTypeInstance.FactType, True)
                            End Select
                        End If

                        If lrRoleInstance.FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject IsNot Nothing).Count = lrRoleInstance.FactType.Arity Then
                            Call lrRoleInstance.FactType.SortRoleGroup()
                        End If

                        Me.Diagram.AllowUnanchoredLinks = False
                        Me.Diagram.AllowUnconnectedLinks = False
                    End With
                Case Is = pcenumConceptType.ModelNote
                    Dim lrModelNoteInstance As FBM.ModelNoteInstance
                    lrModelNoteInstance = lrOriginModelObject
                    For Each lrLink In lrModelNoteInstance.Shape.OutgoingLinks
                        If (lrModelNoteInstance.Shape.OutgoingLinks.Count > 1) And (lrLink.Origin Is e.Link.Origin) And (lrLink.Destination Is e.Link.Destination) Then
                            '------------------------------------------------------------------
                            'Don't allow the user to create the same link that already exists
                            '------------------------------------------------------------------                        
                            lrLinkToRemove = lrLink
                            Exit For
                        ElseIf (lrModelNoteInstance.Shape.OutgoingLinks.Count = 1) And (lrLink.Origin Is e.Link.Origin) And (lrLink.Destination Is e.Link.Destination) Then
                            '------------------------------------------------------------------
                            'Don't remove the new ModelNote link from the existing collection
                            '------------------------------------------------------------------
                            lrLinkToRemove = Nothing
                        Else
                            lrLinkToRemove = lrLink
                            Exit For
                        End If
                    Next
                    If IsSomething(lrLinkToRemove) Then
                        Me.Diagram.Links.Remove(lrLinkToRemove)
                    End If
                    lrModelNoteInstance.JoinedObjectType = New FBM.ModelObject
                    lrModelNoteInstance.JoinedObjectType = lrTargetModelObject
                    lrModelNoteInstance.ModelNote.JoinedObjectType = New FBM.ModelObject
                    lrModelNoteInstance.ModelNote.JoinedObjectType.Id = lrTargetModelObject.Id
            End Select

            If (lrTargetModelObject Is Nothing) And
             Not (TypeOf e.Link.Destination Is DummyNode) Then
                Me.Diagram.Links.Remove(e.Link)
                Exit Sub
            End If

            Dim lrSubtypeConstraintInstance As FBM.SubtypeRelationshipInstance = Nothing
            Dim lbJoinedSubtypingRelationship As Boolean = False
            Dim a As DiagramLink = e.Link

            ' if not connected to a real node. VM-20180316-Leave this here whatever you do. It is how Mindfusion determines that you are joining to a Link rather than a ShapeNode
            If TypeOf a.Destination Is DummyNode Then

                Dim endPt As PointF = a.ControlPoints(a.ControlPoints.Count - 1)
                a.Locked = True ' so we can exclude it from the search
                Dim trg As DiagramLink = Me.Diagram.GetLinkAt(endPt, 1, True)
                If Not trg Is Nothing Then

                    Dim lrRoleInstance As New FBM.RoleInstance
                    lrSubtypeConstraintInstance = trg.Tag
                    lbJoinedSubtypingRelationship = True
                    lrRoleInstance = lrSubtypeConstraintInstance.FactType.FindFirstRoleByModelObject(lrSubtypeConstraintInstance.parentEntityType)

                    lrTargetModelObject = lrRoleInstance

                End If

                Me.Diagram.Links.Remove(a)
            End If

            If IsSomething(lrTargetModelObject) Then
                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.ModelNote
                        e.Link.Pen.DashPattern = New Single() {2, 1, 2, 1}
                        e.Link.Pen.Width = 0.1
                        e.Link.Pen.Color = Color.LightGray
                    Case Is = pcenumConceptType.RoleConstraint
                        If lrTargetModelObject.ConceptType = pcenumConceptType.Role Then

                            '----------------------------------------------------------------------------
                            'Validate the Roles to which the ExternalUniquenessConstraint is connected.
                            '----------------------------------------------------------------------------
                            Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
                            Dim lrRoleConstraint As New FBM.RoleConstraint

                            lrRoleConstraintInstance = lrModelObject
                            lrRoleConstraint = lrRoleConstraintInstance.RoleConstraint

                            '-------------------------------------------------------------------------------------------
                            'Start a RoleConstraintArgument for the RoleConstraint if one hasn't already been started.
                            '-------------------------------------------------------------------------------------------                            
                            If Not lrRoleConstraintInstance.CreatingArgument Then
                                lrRoleConstraintInstance.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraintInstance.RoleConstraint, lrRoleConstraintInstance.RoleConstraint.GetNextArgumentSequenceNr)
                                lrRoleConstraintInstance.CreatingArgument = True
                            End If

                            Select Case lrRoleConstraintInstance.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.RingConstraint
                                    If lrTargetModelObject.ConceptType = pcenumConceptType.Role Then
                                        Dim lrRoleInstance As FBM.RoleInstance
                                        lrRoleInstance = lrTargetModelObject
                                        Dim lrFactType As FBM.FactType = lrRoleInstance.FactType.FactType
                                        If lrFactType.DoesAtLeastTwoRolesReferenceTheSameModelObject And (lrFactType.Arity > 1) Then
                                            '------------------------------------------------------------------------------------------------------
                                            'Is okay. Linking from a RingConstraint to a BinaryFactType with each Role linked to one ModelObject.
                                            '------------------------------------------------------------------------------------------------------
                                            Dim lrRoleConstraintRole As FBM.RoleConstraintRole '(lrRoleInstance.Role, lrRoleConstraint)

                                            Me.Diagram.Links.Remove(e.Link)
                                            lrRoleConstraintRole = lrRoleConstraint.CreateRoleConstraintRole(lrRoleInstance.Role,
                                                                                                             lrRoleConstraintInstance.CurrentArgument) ' RoleConstraintRole.Add(lrRoleConstraintRole)

                                            'Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance = lrRoleConstraintRole.CloneInstance(Me.zrPage)
                                            'lrRoleConstraintInstance.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                                            'e.Link.Tag = lrRoleConstraintRoleInstance
                                            'e.Link.HeadPen.Color = Color.Purple
                                            'e.Link.Pen.DashPattern = New Single() {2, 1, 2, 1}
                                            'e.Link.Pen.Width = 0.4

                                            'If lrRoleConstraintInstance.CreatingArgument Then
                                            '    lrRoleConstraintInstance.CurrentArgument.AddRoleConstraintRole(lrRoleConstraintRole)
                                            'End If

                                            Call Me.zrPage.MakeDirty()
                                            Call Me.EnableSaveButton()
                                        Else
                                            MsgBox("You can only link a Ring Constraint to a Fact Type with more than one Role and where at least 2 Role are hosted by the same Object Type.")
                                            Me.Diagram.Links.Remove(e.Link)
                                        End If
                                    Else
                                        MsgBox("You can only create links from a Ring Constraint to the Roles of a Fact Type.")
                                        Me.Diagram.Links.Remove(e.Link)
                                    End If
                                Case Is = pcenumRoleConstraintType.EqualityConstraint,
                                          pcenumRoleConstraintType.ExclusionConstraint,
                                          pcenumRoleConstraintType.InclusiveORConstraint,
                                          pcenumRoleConstraintType.ExclusiveORConstraint

                                    Dim lrRoleInstance As FBM.RoleInstance = lrTargetModelObject
                                    Dim lrRole As FBM.Role = lrRoleInstance.Role
                                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance = Nothing
                                    Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                                    '------------------------------------
                                    Me.Diagram.Links.Remove(e.Link)
                                    lrRoleConstraintRole = lrRoleConstraintInstance.RoleConstraint.CreateRoleConstraintRole(lrRole,
                                                                                                                            lrRoleConstraintInstance.CurrentArgument,
                                                                                                                            lrSubtypeConstraintInstance)

                                    'lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint)
                                    'lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)

                                    'lrRoleConstraintRoleInstance = lrRoleConstraintRole.CloneInstance(Me.zrPage)
                                    'lrRoleConstraintInstance.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                                    'e.Link.Tag = lrRoleConstraintRoleInstance
                                    'e.Link.HeadPen.Color = Color.Purple
                                    'e.Link.Pen.DashPattern = New Single() {2, 1, 2, 1}
                                    'e.Link.Pen.Width = 0.4
                                    Call Me.zrPage.MakeDirty()
                                    Call Me.EnableSaveButton()
                                    'End If

                                    'If lbJoinedSubtypingRelationship Then
                                    '    lrRoleConstraintRoleInstance.SubtypeConstraintInstance = lrSubtypeConstraintInstance
                                    'End If
                                Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint

                                    Dim lrRoleInstance As FBM.RoleInstance = lrTargetModelObject
                                    Dim lrRole As FBM.Role = lrRoleInstance.Role

                                    '------------------------------------                                    
                                    Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                                    ' lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)


                                    Me.Diagram.Links.Remove(e.Link)
                                    lrRoleConstraintRole = lrRoleConstraintInstance.RoleConstraint.CreateRoleConstraintRole(lrRole, lrRoleConstraintInstance.CurrentArgument)
                                    'Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance = lrRoleConstraintRole.CloneInstance(Me.zrPage)
                                    'lrRoleConstraintInstance.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                                    'e.Link.Tag = lrRoleConstraintRoleInstance
                                    'e.Link.HeadPen.Color = Color.Purple
                                    'e.Link.Pen.DashPattern = New Single() {2, 1, 2, 1}
                                    'e.Link.Pen.Width = 0.4

                                    'If lrRoleConstraintInstance.CreatingArgument Then
                                    '    lrRoleConstraintInstance.CurrentArgument.AddRoleConstraintRole(lrRoleConstraintRole)
                                    'End If

                                    Call Me.zrPage.MakeDirty()
                                    Call Me.EnableSaveButton()

                                Case Is = pcenumRoleConstraintType.SubsetConstraint
                                    Dim lrRoleInstance As FBM.RoleInstance = lrTargetModelObject
                                    Dim lrRole As FBM.Role = lrRoleInstance.Role

                                    Dim lrRoleConstraintRole As New FBM.RoleConstraintRole(lrRole, lrRoleConstraint)

                                    'If lrRoleConstraint.RoleConstraintRole.Count >= 1 Then
                                    '    '---------------------------------------------------------------------------------
                                    '    'Check to see if the last Role added is for a SubsetConstraint.Out/Of type Role.
                                    '    '  SubsetConstraints have an In/Out combination where the In Role is a subset of
                                    '    '  the Out Role. The 'In' and 'Out' Roles must adjoin the same ModelObject/Domain.
                                    '    '---------------------------------------------------------------------------------
                                    '    If lrRoleConstraint.ExistsRoleConstraintRoleForModelObject(lrRole.JoinedORMObject, True) Then
                                    '        lrRoleConstraintRole.IsEntry = True
                                    '        lrRoleConstraintRole.IsExit = False
                                    '    Else
                                    '        e.Link.HeadShape = ArrowHead.PointerArrow
                                    '        e.Link.HeadShapeSize = 3
                                    '        e.Link.HeadPen.Color = Color.Purple
                                    '        e.Link.Pen.DashPattern = New Single() {2, 1, 2, 1}
                                    '        lrRoleConstraintRole.IsEntry = False
                                    '        lrRoleConstraintRole.IsExit = True
                                    '    End If
                                    'Else
                                    '    lrRoleConstraintRole.IsEntry = True
                                    '    lrRoleConstraintRole.IsExit = False
                                    'End If

                                    'lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)

                                    'Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance = lrRoleConstraintRole.CloneInstance(Me.zrPage)
                                    'lrRoleConstraintInstance.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)

                                    'e.Link.Tag = lrRoleConstraintRoleInstance

                                    Me.Diagram.Links.Remove(e.Link)
                                    Call lrRoleConstraintInstance.RoleConstraint.CreateRoleConstraintRole(lrRole,
                                                                                                          lrRoleConstraintInstance.CurrentArgument)

                                    'If lrRoleConstraintInstance.CreatingArgument Then
                                    '    lrRoleConstraintInstance.CurrentArgument.AddRoleConstraintRole(lrRoleConstraintRole)
                                    'End If

                                    Call Me.zrPage.MakeDirty()
                                    Call Me.EnableSaveButton()

                            End Select
                        Else
                            MsgBox("You can only create links from a Role Constraint to the Roles of a Fact Type.")
                            Me.Diagram.Links.Remove(e.Link)
                        End If

                    Case Else

                End Select
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Me.Diagram.Links.Remove(e.Link)
        End Try


        Call Me.ResetNodeAndLinkColors()

    End Sub

    Private Sub Diagram_LinkCreating(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkValidationEventArgs) Handles Diagram.LinkCreating

        Dim lrModelObject As FBM.ModelObject

        Try
            lrModelObject = e.Origin.Tag

            If lrModelObject Is Nothing Then
                e.Cancel = True
                Exit Sub
            End If

            Select Case lrModelObject.ConceptType
                Case Is = pcenumConceptType.EntityType
                    e.Cancel = True
                Case Is = pcenumConceptType.RoleConstraint
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    lrRoleConstraintInstance = lrModelObject
                    Select Case lrRoleConstraintInstance.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.RingConstraint,
                                  pcenumRoleConstraintType.EqualityConstraint,
                                  pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                  pcenumRoleConstraintType.ExclusionConstraint,
                                  pcenumRoleConstraintType.ExclusiveORConstraint,
                                  pcenumRoleConstraintType.InclusiveORConstraint,
                                  pcenumRoleConstraintType.SubsetConstraint
                            e.Link.Tag = lrModelObject
                            e.Link.BaseShape = ArrowHead.None
                            e.Link.HeadShape = ArrowHead.None
                    End Select
                    Dim loNode As DiagramNode
                    loNode = Me.Diagram.GetNodeAt(e.MousePosition)
                    If IsSomething(loNode) Then
                        If loNode.Tag.ConceptType = pcenumConceptType.Role Then
                            Me.DiagramView.DrawLinkCursor = Cursors.Hand
                            loNode.AllowIncomingLinks = True
                        Else
                            Me.DiagramView.DrawLinkCursor = Cursors.No
                        End If
                    End If
                Case Is = pcenumConceptType.ModelNote
                    e.Link.Tag = lrModelObject
                Case Is = pcenumConceptType.Role
                    e.Link.Tag = lrModelObject
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Me.Diagram.Links.Remove(e.Link)
            e.Cancel = True
        End Try

    End Sub

    Private Sub Diagram_LinkDeleting(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkValidationEventArgs) Handles Diagram.LinkDeleting

        Dim lrPageObject As New FBM.ModelObject

        lrPageObject = e.Link.Tag

        'MsgBox(lrPageObject.ConceptType.ToString)

        If lrPageObject Is Nothing Then
            e.Cancel = True
            Exit Sub
        End If

        Select Case lrPageObject.ConceptType
            Case Is = pcenumConceptType.RoleConstraintRole
                Dim lrRoleConstraint As FBM.RoleConstraint
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                lrRoleConstraintRoleInstance = e.Link.Tag
                lrRoleConstraintInstance = lrRoleConstraintRoleInstance.RoleConstraint

                lrRoleConstraintRole = lrRoleConstraintRoleInstance.RoleConstraintRole
                lrRoleConstraint = lrRoleConstraintInstance.RoleConstraint

                Dim lsMessage As String = ""
                lsMessage = "Click [OK] to remove the Role Constraint link from the Model, otherwise click [Cancel]."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Role Constraint links cannot be removed from a Page without being removed from the Model."

                If MsgBox(lsMessage, MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    '-----------------------------------------------------------------------
                    'Remove the RoleConstraintRoleInstance from the RoleConstraintInstance
                    '-----------------------------------------------------------------------
                    lrRoleConstraintInstance.RoleConstraintRole.Remove(lrRoleConstraintRoleInstance)

                    '-------------------------------------------------------
                    'Remove the RoleConstraintRole from the RoleConstraint
                    '-------------------------------------------------------
                    lrRoleConstraint.RoleConstraintRole.Remove(lrRoleConstraintRole)

                    Call TableRoleConstraintRole.DeleteRoleConstraintRole(lrRoleConstraintRole)
                End If
            Case Is = pcenumConceptType.SubtypeConstraint
                If MsgBox("Remove the Subtype from the Model?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                    Dim lrSubtypeInstance As FBM.SubtypeRelationshipInstance = e.Link.Tag
                    Call lrSubtypeInstance.SubtypeRelationship.RemoveFromModel()
                End If
        End Select


    End Sub

    Private Sub Diagram_LinkDoubleClicked(sender As Object, e As LinkEventArgs) Handles Diagram.LinkDoubleClicked

    End Sub

    Private Sub Diagram_LinkModifying(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkValidationEventArgs) Handles Diagram.LinkModifying

    End Sub

    Private Sub Diagram_LinkSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkSelected

        Try
            Select Case e.Link.Tag.ConceptType
                Case Is = pcenumConceptType.SubtypeConstraint
                    Me.zrPage.SelectedObject.Add(e.Link.Tag)
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_SubtypeRelationship
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Diagram_LinkSelecting(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkValidationEventArgs) Handles Diagram.LinkSelecting

        Try
            Select Case e.Link.Tag.ConceptType
                Case Is = pcenumConceptType.Role
                    e.Cancel = True
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Diagram_NodeClicked(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeClicked

        'NB DiagramView.MouseDown happens before this.
        'NB This method is only really applicable for Roles.

        Dim loNode As Object 'Because may be ShapeNode or TableNode
        Dim lrModelObject As FBM.ModelObject

        If IsSomething(Diagram.GetNodeAt(e.MousePosition)) Then
            loNode = Diagram.GetNodeAt(e.MousePosition, True, False)
            lrModelObject = loNode.tag
            Select Case lrModelObject.ConceptType
                Case Is = pcenumConceptType.Role
                    '----------------------------------------------------------------------------------------------
                    'See if the user is adding a Role to a RoleConstraint.Argument
                    If Me.zrPage.RoleConstraintInstance.FindAll(Function(x) x.CreatingArgument = True).Count > 0 Then

                        Dim lrRoleInstance As FBM.RoleInstance
                        lrRoleInstance = lrModelObject

                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                        Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                        '-------------------------------------------------------------------------------------
                        'Get the RoleConstraint/Instance
                        lrRoleConstraintInstance = Me.GetRoleConstraintCurrentlyCreatingArgument

                        If Control.ModifierKeys = Keys.Control Then
                            '--------------------------------------------------------------------------------------------------
                            'Manually creating the JoinPath for the RoleConstraint/Argument
                            If lrRoleConstraintInstance.CurrentArgument.JoinPath.RolePath.Count = 0 Then
                                If lrRoleConstraintInstance.CurrentArgument.RoleConstraintRole.Count > 0 Then
                                    lrRoleConstraintInstance.CurrentArgument.JoinPath.RolePath.Add(lrRoleConstraintInstance.CurrentArgument.RoleConstraintRole(0).Role)
                                End If
                            End If
                            lrRoleConstraintInstance.CurrentArgument.ManuallyCreatedJoinPath = True
                            lrRoleConstraintInstance.CurrentArgument.JoinPath.RolePath.Add(lrRoleInstance.Role)
                            Call lrRoleConstraintInstance.RefreshRoleConstraintRoleTexts()
                            Exit Sub
                        End If

                        If lrRoleConstraintInstance.RoleConstraint.Argument.Count >= 1 Then
                            '--------------------------------------------------------------------------------------------------
                            'User has already created the first Argument in the set. Must check to see that any Roles
                            '  added to subsequent Arguments are compatible with their respective Role in the first Argument.
                            '--------------------------------------------------------------------------------------------------
                            Dim liInd As Integer
                            liInd = lrRoleConstraintInstance.CurrentArgument.RoleConstraintRole.Count
                            'MsgBox(Me.zrPage.Model.AreRolesCompatible(lrRoleInstance.Role, _
                            '                                           lrRoleConstraintInstance.RoleConstraint.Argument(0).RoleConstraintRole(liInd).Role))
                        End If

                        Dim larRole As New List(Of FBM.Role)
                        Dim lrRoleConstraintRoleInd As FBM.RoleConstraintRole
                        For Each lrRoleConstraintRoleInd In lrRoleConstraintInstance.CurrentArgument.RoleConstraintRole
                            larRole.Add(lrRoleConstraintRoleInd.Role)
                        Next
                        larRole.Add(lrRoleInstance.Role)

                        Dim liJoinPathError As pcenumJoinPathError = pcenumJoinPathError.None

                        '----------------------------------------------------------------------------------------------------------
                        'Check to see if creating 2nd, 3rd, 4th etc Argument for a RoleConstraint that doesn't require a JoinPath
                        '  NB See also: frmDiagramORM.LinkCreated (for saving the RoleConstraintRole  to the CurrentArgument). Happens before this.
                        If Not lrRoleConstraintInstance.RequiresJoinPath Then

                            Call Me.SaveAnyRoleConstraintArgumentsBeingCreated()

                            lrRoleConstraintInstance.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraintInstance.RoleConstraint, lrRoleConstraintInstance.RoleConstraint.GetNextArgumentSequenceNr)
                            lrRoleConstraintInstance.CreatingArgument = True


                            lrRoleConstraintRole = lrRoleConstraintInstance.RoleConstraint.CreateRoleConstraintRole(lrRoleInstance.Role)
                            lrRoleConstraintInstance.CurrentArgument.AddRoleConstraintRole(lrRoleConstraintRole)
                            lrRoleInstance.Shape.Text = lrRoleConstraintInstance.CurrentArgument.SequenceNr.ToString &
                                                        "." &
                                                        lrRoleConstraintRole.ArgumentSequenceNr.ToString

                        ElseIf Me.zrPage.Model.ExistsJoinPathForRoles(larRole, liJoinPathError, lrRoleConstraintInstance.RoleConstraint) Then

                            If lrRoleConstraintInstance.RoleConstraintType = pcenumRoleConstraintType.SubsetConstraint Then

                                lrRoleConstraintRole = lrRoleConstraintInstance.RoleConstraint.CreateRoleConstraintRole(lrRoleInstance.Role, lrRoleConstraintInstance.CurrentArgument)
                                lrRoleConstraintInstance.CurrentArgument.AddRoleConstraintRole(lrRoleConstraintRole)
                                lrRoleInstance.Shape.Text = lrRoleConstraintInstance.CurrentArgument.SequenceNr.ToString &
                                                            "." &
                                                            lrRoleConstraintRole.ArgumentSequenceNr.ToString

                                If lrRoleConstraintInstance.CurrentArgument.RoleConstraintRole.Count = 2 Then
                                    Call Me.SaveAnyRoleConstraintArgumentsBeingCreated()

                                    If lrRoleConstraintInstance.RoleConstraint.Argument.Count = 2 Then
                                        lrRoleConstraintInstance.CreatingArgument = False
                                    Else
                                        lrRoleConstraintInstance.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraintInstance.RoleConstraint, lrRoleConstraintInstance.RoleConstraint.GetNextArgumentSequenceNr)
                                        lrRoleConstraintInstance.CreatingArgument = True
                                    End If
                                End If

                            ElseIf lrRoleConstraintInstance.RoleConstraintType = pcenumRoleConstraintType.RingConstraint Then

                                Call Me.SaveAnyRoleConstraintArgumentsBeingCreated()

                                lrRoleConstraintInstance.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraintInstance.RoleConstraint, lrRoleConstraintInstance.RoleConstraint.GetNextArgumentSequenceNr)
                                lrRoleConstraintInstance.CreatingArgument = True

                                lrRoleConstraintRole = lrRoleConstraintInstance.RoleConstraint.CreateRoleConstraintRole(lrRoleInstance.Role)
                                lrRoleConstraintInstance.CurrentArgument.AddRoleConstraintRole(lrRoleConstraintRole)
                                lrRoleInstance.Shape.Text = lrRoleConstraintInstance.CurrentArgument.SequenceNr.ToString &
                                                            "." &
                                                            lrRoleConstraintRole.ArgumentSequenceNr.ToString

                            Else
                                lrRoleConstraintRole = lrRoleConstraintInstance.RoleConstraint.CreateRoleConstraintRole(lrRoleInstance.Role)
                                lrRoleConstraintInstance.CurrentArgument.AddRoleConstraintRole(lrRoleConstraintRole)
                                lrRoleInstance.Shape.Text = lrRoleConstraintInstance.CurrentArgument.SequenceNr.ToString &
                                                            "." &
                                                            lrRoleConstraintRole.ArgumentSequenceNr.ToString
                            End If
                        Else
                            Dim lsMessage As String = ""
                            lsMessage = "Tried to add a RoleConstraintRole to the Argument of a RoleConstraint, where there is no JoinPath between the existing Roles of the Argument, and the Role being added."
                            lsMessage &= vbCrLf & vbCrLf
                            lsMessage &= "Error: " & liJoinPathError.ToString
                            lsMessage &= vbCrLf & vbCrLf
                            lsMessage &= "Do you want to add the Role to the Role Constaint anyway?"
                            If MsgBox(lsMessage, MsgBoxStyle.Exclamation + MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                                lrRoleConstraintRole = lrRoleConstraintInstance.RoleConstraint.CreateRoleConstraintRole(lrRoleInstance.Role)
                                lrRoleConstraintInstance.CurrentArgument.AddRoleConstraintRole(lrRoleConstraintRole, True)
                                lrRoleInstance.Shape.Text = lrRoleConstraintInstance.CurrentArgument.SequenceNr.ToString &
                                                            "." &
                                                            lrRoleConstraintRole.ArgumentSequenceNr.ToString
                            End If
                        End If

                    End If
            End Select
        End If

    End Sub


    Private Sub Diagram_NodeDeleting(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeValidationEventArgs) Handles Diagram.NodeDeleting

        Dim loObject As Object

        loObject = Me.Diagram.Selection.Items(0).Tag

        If Me.zrPage.SelectedObject.Count = 1 Then
            If TypeOf loObject Is FBM.ModelObject Then
                Select Case loObject.ConceptType
                    Case Is = pcenumConceptType.FactType
                        '--------------------------------------------
                        'Remove the FactTypeInstance from the Model
                        '--------------------------------------------
                        Dim lrFactTypeInstance As New FBM.FactTypeInstance
                        lrFactTypeInstance = loObject
                        Call lrFactTypeInstance.RemoveFromPage(True)

                        If MsgBox("Remove the Fact Type from the Model?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            Call lrFactTypeInstance.FactType.RemoveFromModel()
                        End If
                    Case Is = pcenumConceptType.EntityType
                        Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
                        lrEntityTypeInstance = loObject
                        Call lrEntityTypeInstance.RemoveFromPage(True)

                        If MsgBox("Remove the Entity Type from the Model?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            Call lrEntityTypeInstance.EntityType.RemoveFromModel(False)
                        End If
                    Case Is = pcenumConceptType.ValueType
                        Dim lrValueTypeInstance As New FBM.ValueTypeInstance
                        lrValueTypeInstance = loObject
                        If lrValueTypeInstance.RemoveFromPage(True) Then
                            '---------------------------------------------------
                            'Successfully removed the ValueType from the Page
                            '---------------------------------------------------
                        Else
                            e.Cancel = True
                            Exit Sub
                        End If

                        If MsgBox("Remove the Value Type from the Model?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            If lrValueTypeInstance.ValueType.RemoveFromModel() Then
                                '-----------------------------------------------
                                'Successfully removed ValueType from the Model
                                '-----------------------------------------------
                            Else
                                e.Cancel = True
                            End If
                        End If
                    Case Is = pcenumConceptType.Role
                        If e.Node.Tag.ConceptType = pcenumConceptType.FactType Then
                            '---------------------------------------------------------------------
                            'Richmond selects a FactTypeInstance with a RoleInstance (user click)
                            '  so that FactTypeInstance moves when Click/Drag.
                            '  But, even when the FactTypeInstance is removed from the Selection
                            '  MindFusion will continue with the Deletion as if it was still there.
                            '  So, we just cancel the deletion.
                            '  NB MindFusion will delete the Role, because only the deletion of the
                            '  FactTypeInstance is cancelled.
                            '----------------------------------------------------------------------
                            e.Cancel = True
                            Exit Sub
                        End If

                        e.Cancel = True
                    Case Else
                        e.Cancel = True

                End Select
            Else
                If TypeOf loObject Is FBM.ModelObject Then
                    Select Case loObject.ConceptType
                        Case Is = pcenumConceptType.Role

                            '----------------------------------------------------------
                            'Removes the FactType from the Selection, so that
                            '  only the Role is Deleted/Removed from the group.
                            '  The reason that the FactType is selected with the
                            '  Role is a MindFusion thing. Selecting/Moving the Role
                            '  within selecting the FactType only moves the one Role,
                            '  not the whole RoleGroup.
                            '----------------------------------------------------------
                            Me.Diagram.Selection.RemoveItem(Me.Diagram.Selection.Items(1))

                            Dim lrRoleInstance As FBM.RoleInstance = loObject
                            Dim lrFactTypeInstance As FBM.FactTypeInstance = lrRoleInstance.FactType

                            If e.Node.Tag.ConceptType = pcenumConceptType.FactType Then
                                '---------------------------------------------------------------------
                                'Richmond selects a FactTypeInstance with a RoleInstance (user click)
                                '  so that FactTypeInstance moves when Click/Drag.
                                '  But, even when the FactTypeInstance is removed from the Selection
                                '  MindFusion will continue with the Deletion as if it was still there.
                                '  So, we just cancel the deletion.
                                '  NB MindFusion will delete the Role, because only the deletion of the
                                '  FactTypeInstance is cancelled.
                                '----------------------------------------------------------------------
                                e.Cancel = True
                                Exit Sub
                            End If

                            '--------------------------------------------------------------------------------
                            'Remove the RoleInstance from the FactTypeInstance.
                            '  NB If the FactTypeInstance.Arity =1 (lrRoleInstance is the last RoleInstance
                            '  for the FactTypeInstance) RemoveRole (below) removes the FactTypeInstance
                            '  from the Page
                            '--------------------------------------------------------------------------------
                            Call lrFactTypeInstance.FactType.RemoveRole(lrRoleInstance.Role, True, True)

                    End Select
                End If
            End If
        End If

        frmMain.ToolStripButton_Save.Enabled = True

    End Sub

    Private Sub Diagram_NodeDeselected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeDeselected

        '---------------------------------------------------------
        'Try this...if one node is deselected, then deselect all nodes
        'Me.zrPage.SelectedObject.Clear() 'Didn't seem to work

        If IsSomething(e.Node) Then
            Select Case e.Node.Tag.ConceptType
                Case Is = pcenumConceptType.EntityType
                    If Not (Control.ModifierKeys = Keys.Control) Then
                        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                        lrEntityTypeInstance = e.Node.Tag
                        Call lrEntityTypeInstance.NodeDeselected()
                    End If
                Case Is = pcenumConceptType.RoleConstraint
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    lrRoleConstraintInstance = e.Node.Tag
                    Call lrRoleConstraintInstance.NodeDeselected()
                Case Is = pcenumConceptType.FactTable
                    Call e.Node.Tag.GetFactsFromTableNode()
                    e.Node.Tag.ResetBlackCellText()
                Case Is = pcenumConceptType.Role
                    If Not (Control.ModifierKeys = Keys.Control) Then
                        Call e.Node.Tag.NodeDeselected()
                    End If
            End Select
        End If




    End Sub

    Private Sub Diagram_NodeDoubleClicked(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeDoubleClicked

        Dim lo_point As PointF = e.MousePosition
        Dim loNode As Object 'Because may be ShapeNode or TableNode
        Dim lrModelObject As New FBM.ModelObject


        If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            loNode = Diagram.GetNodeAt(lo_point, True, False)
            lrModelObject = loNode.tag
            Select Case lrModelObject.ConceptType
                Case Is = pcenumConceptType.FactTable
                    '--------------------------------------
                    'User has DoubleClicked on a FactTable
                    '--------------------------------------
                    If loNode.bounds.height = loNode.captionHeight Then
                        '--------------------
                        'Expand the Table
                        '--------------------
                        Dim lo_rectangle As New Rectangle(loNode.Bounds.X, loNode.Bounds.Y, loNode.bounds.width, 20)
                        loNode.setRect(lo_rectangle, True)
                        Me.DiagramView.EndEdit(False)
                    Else
                        '--------------------
                        'Collapse the Table
                        '--------------------
                        Dim lo_rectangle As New Rectangle(loNode.Bounds.X, loNode.Bounds.Y, loNode.bounds.width, loNode.captionHeight)
                        loNode.setRect(lo_rectangle, False)
                        Me.DiagramView.EndEdit(False)
                    End If
                Case Is = pcenumConceptType.Role
                    'See Me.GetRoleConstraintCurrentlyCreatingArgument
                    '  * See below, Double Clicking on a RoleConstraint initiates the process of creating and Argument for that RoleConstraint.
                    '  * See also Me.Diagram.NodeClicked for when the User singl clicks on a Role.
                    If Me.zrPage.RoleConstraintInstance.FindAll(Function(x) x.CreatingArgument = True).Count = 1 Then
                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                        lrRoleConstraintInstance = Me.GetRoleConstraintCurrentlyCreatingArgument
                        lrRoleConstraintInstance.RoleConstraint.AddArgument(lrRoleConstraintInstance.CurrentArgument, True)
                        lrRoleConstraintInstance.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraintInstance.RoleConstraint,
                                                                                                  lrRoleConstraintInstance.RoleConstraint.Argument.Count + 1)
                    End If
                Case Is = pcenumConceptType.RoleConstraint
                    '-------------------------------------------------------------------------------------------------------------
                    'Toggle the RoleConstraintArgument creation status of the RoleConstraint(Instance).
                    '  NB First, make sure that no other RoleConstraint is having a RoleConstraintArgument created for it.
                    '  i.e. CancelAllRoleConstraintArgumentCreationStati()
                    '  * See also Me.SaveAnyRoleConstraintArgumentsBeingCreated()
                    '  * See also above (when the User double clicks on a Role).
                    '-------------------------
                    '  NB If you're looking for the code the draws the line/link that follows the mouse,
                    '    see tRoleConstraintLink's overrie of the Draw method.
                    '-------------------------------------------------------------------------------------------------------------                    
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    lrRoleConstraintInstance = lrModelObject
                    Dim lbCurrentArgumentCreationStatus As Boolean = lrRoleConstraintInstance.CreatingArgument
                    Call Me.CancelAllRoleConstraintArgumentCreationStati()
                    lrRoleConstraintInstance.CreatingArgument = Not lbCurrentArgumentCreationStatus
                    If lrRoleConstraintInstance.CreatingArgument Then
                        lrRoleConstraintInstance.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraintInstance.RoleConstraint, lrRoleConstraintInstance.RoleConstraint.GetNextArgumentSequenceNr)
                    Else 'Not creating Argument, but was.
                        '--------------------------------------------------------------------------
                        'Finalise the creation of the Argument for the respective RoleConstraint.
                        '--------------------------------------------------------------------------
                        lrRoleConstraintInstance.RoleConstraint.AddArgument(lrRoleConstraintInstance.CurrentArgument, True)
                        lrRoleConstraintInstance.CurrentArgument = Nothing
                    End If
                Case Is = pcenumConceptType.EntityType
                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                    lrEntityTypeInstance = lrModelObject
                    lrEntityTypeInstance.EntityTypeNameShape.Locked = False

                Case Is = pcenumConceptType.ModelNote

                    'Nothing to do here. See DiagramView.EnterInplaceEditMode for setting the size of the text edit control

            End Select

        End If
        e.MousePosition = Nothing

    End Sub



    Private Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click

        Diagram.UndoManager.Undo()

    End Sub

    Private Sub SaveToXMLFieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToXMLFieToolStripMenuItem.Click

        SaveFileDialog.DefaultExt = "xml"
        SaveFileDialog.Filter = "Boston XML files (*.xml)|*.xml|All Files (*.*)|*.*"
        If SaveFileDialog.ShowDialog() = DialogResult.OK Then
            Diagram.SaveToXml(SaveFileDialog.FileName)
        End If

    End Sub

    ''' <summary>
    ''' When the user doubleclicks on a ShapeNode to edit the text, this method changes the Width and Height of the editing textbox to suit the text of the ShapeNode.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DiagramView_EnterInplaceEditMode(sender As Object, e As InPlaceEditEventArgs) Handles DiagramView.EnterInplaceEditMode

        Dim graphics As Graphics = Me.DiagramView.CreateGraphics()
        Dim liStringWidth, liStringHeight As Integer


        If e.EditControl.Text = Trim("") Then
            liStringWidth = graphics.MeasureString(e.EditControl.Text + "MMMMMMMMMMMMMMM", e.EditControl.Font).Width
            liStringHeight = graphics.MeasureString("M", e.EditControl.Font).Height + 5
        Else
            liStringWidth = graphics.MeasureString(e.EditControl.Text + "MMMM", e.EditControl.Font).Width
            liStringHeight = graphics.MeasureString(e.EditControl.Text, e.EditControl.Font).Height + 5
        End If

        Dim loDiagramView As DiagramView = e.EditControl.Parent
        Dim loRectangle As New Rectangle(e.Node.Bounds.X, e.Node.Bounds.Y, e.Node.Bounds.Width, e.Node.Bounds.Height)

        e.EditControl.Width = Viev.Greater(liStringWidth * ((loDiagramView.ZoomFactor + 20) / 100), liStringWidth)
        e.EditControl.Height = Viev.Greater(liStringHeight * ((loDiagramView.ZoomFactor + 20) / 100), liStringHeight)


    End Sub

    Private Sub ORMDiagramView_GiveFeedback(ByVal sender As Object, ByVal e As System.Windows.Forms.GiveFeedbackEventArgs) Handles DiagramView.GiveFeedback

        '----------------------------------------------------------------------------------
        'Set the mouse cursor to a custom cursor for SubtypeConnector
        '  See the following discussion for more detail.
        'http://stackoverflow.com/questions/432379/set-custom-cursor-from-resource-file
        '---------------------------------------------------------------------------------
        e.UseDefaultCursors = False
        'Dim ms As New System.IO.MemoryStream(My.Resources.SubtypeConnectorCursor)

        Me.Cursor = New Cursor(My.Resources.Icons.SubtypeConnectorSmallCursor.Handle)

        Dim graphics As Graphics = Me.CreateGraphics()
        Dim rectangle As New Rectangle(New Point(10, 10), New Size(Cursor.Size.Width / 3, Cursor.Size.Height / 3))
        Me.Cursor.DrawStretched(graphics, rectangle)


        Dim p As Point = DiagramView.PointToClient(New Point(System.Windows.Forms.Form.MousePosition.X, System.Windows.Forms.Form.MousePosition.Y))
        Dim lo_point As PointF = DiagramView.ClientToDoc(New Point(p.X, p.Y))

        '20200510-VM-Remove if all okay
        'Dim loMouseLocation As Point

        If Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.ORMSubtypeConnector Then
            '20200510-VM-Remove if all okay
            'loMouseLocation = New Point(System.Windows.Forms.Form.MousePosition.X, System.Windows.Forms.Form.MousePosition.Y)
            'loMouseLocation = PointToClient(loMouseLocation)
            'lo_point = Me.ORMDiagramView.ClientToDoc(loMouseLocation)

            '--------------------------------------------------
            'Just to be sure...set the Richmond.WorkingProject
            '--------------------------------------------------
            prApplication.WorkingPage = Me.zrPage

            If IsSomething(Diagram.GetNodeAt(lo_point)) Then
                If Me.zrPage.SelectedObject.Count = 2 Then
                    If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.EntityType And Me.zrPage.SelectedObject(1).ConceptType = pcenumConceptType.EntityType Then
                        Dim lrEntityType_1 As New FBM.EntityTypeInstance
                        Dim lrEntityType_2 As New FBM.EntityTypeInstance

                        lrEntityType_1 = Me.zrPage.SelectedObject(0)
                        lrEntityType_2 = Me.zrPage.SelectedObject(1)

                        Call lrEntityType_1.AddSubtypeConstraint(lrEntityType_2)
                        Call Me.zrSpecialDragMode.ResetSpecialDragMode()
                    End If
                End If
            Else
                If System.Windows.Forms.Form.MouseButtons = Windows.Forms.MouseButtons.None Then
                    Me.zrSpecialDragMode.MouseUpCounter += 1
                    If Me.zrSpecialDragMode.MouseUpCounter > 1 Then
                        Call Me.zrSpecialDragMode.ResetSpecialDragMode()
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub ORMDiagramView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DiagramView.KeyDown

        Dim ls_message As String = ""
        Dim li_FactType_cardinality As Integer = 0
        Dim lsFactTypeName As String = ""
        Dim lrFactType As FBM.FactType
        Dim lrFactTypeInstance As FBM.FactTypeInstance

        Select Case e.KeyCode
            Case Is = Keys.Delete
                e.Handled = True
            Case Is = Keys.M
                Call frmMain.LoadToolboxModelDictionary()
                Call Me.DiagramView.Focus()
            Case Is = Keys.T
                Call frmMain.LoadToolbox()
                Call Me.SetToolbox()
                Call Me.DiagramView.Focus()
            Case Is = Keys.Right 'Right Arrow
                If Me.zrPage.SelectedObject.Count > 0 Then
                    Dim loObject As Object
                    For Each loObject In Me.zrPage.SelectedObject
                        If loObject.ConceptType <> pcenumConceptType.Role Then
                            loObject.shape.move((loObject.shape.bounds.x + 1), loObject.shape.bounds.y)
                        End If
                    Next
                End If
            Case Is = Keys.Left 'Left Arrow
                If Me.zrPage.SelectedObject.Count > 0 Then
                    Dim loObject As Object
                    For Each loObject In Me.zrPage.SelectedObject
                        If loObject.ConceptType <> pcenumConceptType.Role Then
                            loObject.shape.move((loObject.shape.bounds.x - 1), loObject.shape.bounds.y)
                        End If
                    Next
                End If
            Case Is = Keys.Up 'Up Arrow
                If Me.zrPage.SelectedObject.Count > 0 Then
                    Dim loObject As Object
                    For Each loObject In Me.zrPage.SelectedObject
                        If loObject.ConceptType <> pcenumConceptType.Role Then
                            loObject.shape.move(loObject.shape.bounds.x, loObject.shape.bounds.y - 1)
                        End If
                    Next
                End If
            Case Is = Keys.Down 'Down Arrow
                If Me.zrPage.SelectedObject.Count > 0 Then
                    Dim loObject As Object
                    For Each loObject In Me.zrPage.SelectedObject
                        If loObject.ConceptType <> pcenumConceptType.Role Then
                            loObject.shape.move(loObject.shape.bounds.x, loObject.shape.bounds.y + 1)
                        End If
                    Next
                End If
            Case Is = Keys.U
                Call AddUniquenessConstraint()
            Case Is = Keys.P
                Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                If IsSomething(lrPropertyGridForm) Then
                    If Me.Diagram.Selection.Items.Count = 1 Then
                        lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(Me.Diagram.Selection.Items.Count - 1).Tag
                        Me.Diagram.Selection.Items(Me.Diagram.Selection.Items.Count - 1).Tag.Shape.Selected = True
                    Else
                        Call Me.DiagramView.Focus()
                    End If
                End If


            Case Is = Keys.R
                '-----------------------------------------------------------
                'The User has elected to add a Role to the Model.
                'Create a FactType (RoleGroup) linking the selected Objects.
                '-----------------------------------------------------------
                If Me.zrPage.SelectedObject.Count > 0 Then
                    If Me.zrPage.AreNoSelectedObjectsRoles() Then
                        '----------------------------------------------------------------
                        'User has elected to join role/s (Fact Type) to any of a set of
                        '  - Entities
                        '  - Value Types
                        '  - Objectified Fact Types
                        '----------------------------------------------------------------------------------------------
                        'PSEUDOCODE
                        '  * Calculate a point midway between the selected objects
                        '  * FOREACH ORMObject WITHIN THE ORMModel.SelectedObject list
                        '      * Create a FactType for the new FactType within the ORMModel.FactType list
                        '      * Create a Role for the ORMObject (within the ORMModel.Role list
                        '      * Create a RoleInstance for the ORMObject (within the ORMModel.RoleInstance list
                        '      * Display and attach the the RoleInstance.Shape to the new RoleInstance
                        '           (use the calculated position midway between the SelectedObject list of ORMObjects)
                        '           (For each RoleInstance that is not the first RoleInstance created,
                        '              attach it to the first RoleInstance)
                        '  * LOOP
                        '----------------------------------------------------------------------------------------------
                        Dim liCounter As Integer = 0 'Used to count as the Roles are created
                        Dim loObject As Object
                        Dim loNode As New ShapeNode

                        '------------------------------------------------------------------
                        'Abort if any SelectedObject is a FactType that is not Objectified.
                        '  * The reason for this is that you cannot attach a FactType
                        '    to a FactType that is not Objectified.
                        '------------------------------------------------------------------
                        For Each loObject In Me.zrPage.SelectedObject
                            Select Case loObject.ConceptType
                                Case Is = pcenumConceptType.FactType
                                    If loObject.IsObjectified = False Then
                                        MsgBox("You have a Fact Type selected which is not objectified. You may not attach a Fact Type to a Fact Type that is not first Objectified.")
                                        Exit Sub
                                    End If
                            End Select
                        Next

                        '-------------------------------------------------------
                        'Construct the FactTypeName for the new FactType as an
                        '  almalgamation of the Names of the selected objects.
                        '-------------------------------------------------------
                        For Each loObject In Me.zrPage.SelectedObject
                            Select Case loObject.ConceptType
                                Case Is = pcenumConceptType.EntityType, pcenumConceptType.ValueType, pcenumConceptType.FactType
                                    lsFactTypeName &= loObject.name
                            End Select
                        Next

                        '-------------------------------------------------------------------------
                        'Create a list of the selected objects so that a FactType can be created
                        '  with as many Roles (linked to the selected objects) as there are
                        '  selected ModelObjects.
                        '-------------------------------------------------------------------------
                        Dim larModelObject As New List(Of FBM.ModelObject)
                        For Each loObject In Me.zrPage.SelectedObject
                            Select Case loObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    larModelObject.Add(loObject.EntityType)
                                Case Is = pcenumConceptType.ValueType
                                    larModelObject.Add(loObject.ValueType)
                                Case Is = pcenumConceptType.FactType
                                    larModelObject.Add(loObject.FactType)
                            End Select
                        Next

                        '-------------------------------------------------------------------------------------------
                        'Create a new FactType for the ORMModel with Roles joined to the list of selected objects.
                        '-------------------------------------------------------------------------------------------
                        lrFactType = Me.zrPage.Model.CreateFactType(lsFactTypeName, larModelObject, , , , , , Me.zrPage)

                        '------------------------------------------------
                        '  Create the FactTypeInstance for the FactType
                        '------------------------------------------------
                        Dim lo_point As PointF = Me.zrPage.GetMidOfSelectedObjects

                        If lrFactType.Arity = 2 Then
                            '---------------------------------------------------------
                            'See if optimal placement of the FactType can be found.
                            '  i.e. Make the FactTypeInstance on the same Y-axis as
                            '  the ModelObject with the least number of associated
                            '  FactTypes.
                            '---------------------------------------------------------
                            Dim lrModelObject As New Object

                            For Each lrModelObject In Me.zrPage.SelectedObject
                                If lrModelObject.ConceptType = pcenumConceptType.ValueType Then
                                    lo_point.Y = lrModelObject.Y
                                End If
                            Next

                            '===========================================================================================================================
                            'Create an initial FactTypeReading if the selected ModelObjects contains an EntityType/ObjectifiedFactType and a ValueType
                            Dim lbContainsEntityTypeOrObjectifiedFactType As Boolean = False
                            Dim lasConceptTypeSet = New List(Of pcenumConceptType) From {pcenumConceptType.EntityType, pcenumConceptType.FactType}
                            Dim lrRole As FBM.Role = lrFactType.RoleGroup.Find(Function(x) lasConceptTypeSet.Contains(x.JoinedORMObject.ConceptType))
                            lbContainsEntityTypeOrObjectifiedFactType = lrRole IsNot Nothing
                            Select Case lrRole.JoinedORMObject
                                Case Else
                                    If lrRole.JoinedORMObject.ConceptType = pcenumConceptType.FactType Then
                                        If DirectCast(lrRole.JoinedORMObject, FBM.FactType).IsObjectified Then
                                            lbContainsEntityTypeOrObjectifiedFactType = True
                                        End If
                                    Else
                                        lbContainsEntityTypeOrObjectifiedFactType = True
                                    End If
                            End Select

                            Dim lbContainsValueType As Boolean = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.ConceptType = pcenumConceptType.ValueType) IsNot Nothing

                            If lbContainsEntityTypeOrObjectifiedFactType And lbContainsValueType Then

                                Dim lrSentence As New Language.Sentence("FirstObject has SecondObject", "FirstObject has SecondObject")
                                lrSentence.PredicatePart.Add(New Language.PredicatePart("has"))
                                lrSentence.PredicatePart.Add(New Language.PredicatePart(""))
                                Dim larRole As New List(Of FBM.Role)

                                larRole.Add(lrFactType.RoleGroup.Find(Function(x) lasConceptTypeSet.Contains(x.JoinedORMObject.ConceptType)))
                                larRole.Add(lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.ConceptType = pcenumConceptType.ValueType))

                                Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, lrSentence)

                                'lrFactTypeReading.IsPreferred = True
                                'lrFactTypeReading.TypedPredicateId = lrFactType.GetTypedPredicateIdByRoleSequence(lrFactTypeReading)
                                'lrFactTypeReading.IsPreferredForPredicate = (lrFactType.FactTypeReading.FindAll(AddressOf lrFactTypeReading.EqualsByRoleSequence).Count = 0)

                                lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)
                            End If

                        End If

                        Dim loFactTypeInstancePt As New PointF(lo_point.X - 15, lo_point.Y)

                        lrFactTypeInstance = Me.zrPage.DropFactTypeAtPoint(lrFactType, loFactTypeInstancePt, Me.ViewFactTablesToolStripMenuItem.Checked)

                        '------------------------------------------------------------------------------------
                        'Have just created a FactType so if frmToolboxModelDictionary is loaded then
                        '  reset the zrLoadedModel to Nothing so that when the user moves the mouse over the 
                        '  Page the ModelDictionary is reloaded.
                        '------------------------------------------------------------------------------------
                        Dim child As frmToolboxModelDictionary
                        If prApplication.RightToolboxForms.FindAll(Function(x) x.Name = frmToolboxModelDictionary.Name).Count >= 1 Then
                            child = prApplication.RightToolboxForms.Find(Function(x) x.Name = frmToolboxModelDictionary.Name)
                            child.zrLoadedModel = Nothing
                        End If

                    ElseIf Me.zrPage.role_and_object_type_selected Then
                        '------------------------------------------------------------------
                        'The user has selected a Role and an EntityType/ValueType, so the user 
                        ' wants to extend the FactType/RoleGroup of the selected Role
                        ' to include a new Role to the selected EntityType 
                        '------------------------------------------------------------------

                        '------------------------------------------------------------------------------
                        'PSEUDOCODE
                        '  * Get the details of the RoleInstance that has been selected
                        '  * Get the details of the EntityTypeInstance/ValueTypeInstance that has been selected
                        '  * Create a new Model-level Role within the Model-level FactType of 
                        '      the RoleInstance that has been selected, referencing the Model-level
                        '      EntityType of the selected EntityTypeInstance
                        '  * Create a new RoleInstance to be attached to the FactTypeInstance
                        '      of the selected RoleInstance
                        '  * Inject the new RoleInstance within the appropriate FactTypeInstance                        
                        '  * Display the new RoleInstance and associated link within the Page.
                        '------------------------------------------------------------------------------
                        Dim lrRole As New FBM.Role
                        Dim lrRoleInstance As New FBM.RoleInstance

                        lrFactType = New FBM.FactType
                        lrFactTypeInstance = New FBM.FactTypeInstance

                        '--------------------------------------------------------------
                        ' Get the details of the RoleInstance that has been selected.                        
                        '--------------------------------------------------------------
                        lrRoleInstance = Me.zrPage.GetFirstSelectedRoleInstance()
                        lrFactTypeInstance = lrRoleInstance.FactType
                        lrFactType = lrFactTypeInstance.FactType

                        '--------------------------------------------------------------
                        ' Get the details of the EntityTypeInstance/ValueTypeInstance
                        '   that has been selected.
                        '--------------------------------------------------------------
                        Dim lrModelObjectInstance As Object
                        lrModelObjectInstance = Me.zrPage.GetFirstSelectedModelObject()

                        Me.Cursor = Cursors.WaitCursor
                        Select Case lrModelObjectInstance.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                lrRole = lrFactType.CreateRole(lrModelObjectInstance.EntityType, True, True)
                            Case Is = pcenumConceptType.ValueType
                                lrRole = lrFactType.CreateRole(lrModelObjectInstance.ValueType, True, True)
                            Case Is = pcenumConceptType.FactType
                                lrRole = lrFactType.CreateRole(lrModelObjectInstance.FactType, True, True)
                        End Select
                        Me.Cursor = Cursors.Default

                        Me.Diagram.Invalidate()
                    End If
                End If
            Case Is = Keys.M
                If Me.zrPage.AreAllSelectedObjectsRoles Then
                    Dim loObject As Object
                    For Each loObject In Me.zrPage.SelectedObject
                        loObject.mandatory = Not loObject.mandatory
                        Call loObject.refresh_role_instance()
                    Next loObject
                End If
            Case Is = Keys.X
                'See the PreviewKeyDown Event for this event
            Case Is = (e.Alt + Keys.X)
                'See the PreviewKeyDown Event for this event
            Case Is = Keys.G
                If Me.zrPage.SelectedObject.Count > 2 Then
                    '--------------------------------------------------------
                    'Ring Constraints can only be added to Binary FactTypes
                    '--------------------------------------------------------
                    Exit Sub
                End If

                If Me.zrPage.AreAllSelectedObjectsRoles And (Me.zrPage.SelectedObject.Count = 2) Then

                    If Me.zrPage.are_all_selected_roles_within_the_same_FactType Then
                        '--------------------------------------------------------
                        'User has selected the Roles within one binary FactType
                        '--------------------------------------------------------
                        lrFactTypeInstance = Me.zrPage.SelectedObject(0).FactType

                        Call Me.zrPage.CreateRingConstraint(lrFactTypeInstance)

                        Me.zrPage.SelectedObject.Clear()
                    End If
                Else
                    Exit Sub
                End If

            Case Is = Keys.O
                If Me.zrPage.AreAllSelectedObjectsRoles Then
                    Dim lo_point As PointF = Me.zrPage.GetMidOfSelectedObjects
                    Dim lrRoleInstance As FBM.RoleInstance
                    Dim larRoleInstance As New List(Of FBM.RoleInstance)

                    For Each lrRoleInstance In Me.zrPage.SelectedObject
                        larRoleInstance.AddUnique(lrRoleInstance)
                    Next
                    Call Me.CreateInclusiveORConstraint(larRoleInstance, lo_point)
                End If
            Case Is = Keys.Add
                '---------------------------------
                'Zooming in
                '---------------------------------
                If frmMain.ToolStripComboBox_zoom.SelectedIndex < 7 Then
                    frmMain.ToolStripComboBox_zoom.SelectedIndex += 1
                    Me.DiagramView.ZoomFactor = frmMain.ToolStripComboBox_zoom.SelectedItem.itemdata
                    Me.Diagram.Invalidate()
                End If
            Case Is = Keys.Subtract
                '---------------------------------
                'Zooming Out
                '---------------------------------
                If frmMain.ToolStripComboBox_zoom.SelectedIndex > 0 Then
                    frmMain.ToolStripComboBox_zoom.SelectedIndex -= 1
                    Me.DiagramView.ZoomFactor = frmMain.ToolStripComboBox_zoom.SelectedItem.itemdata
                    Me.Diagram.Invalidate()

                    Dim lrDiagramOverviewForm As frmToolboxOverview
                    lrDiagramOverviewForm = prApplication.GetToolboxForm(frmToolboxOverview.Name)

                    If IsSomething(lrDiagramOverviewForm) Then
                        lrDiagramOverviewForm.SetDocument(Me.DiagramView)
                    End If
                End If
            Case Is = (e.Control And Keys.E)
                Call Me.CreateEntityType()
            Case Is = Keys.E
                If e.Alt Then Exit Sub
                If Me.zrPage.AreAllSelectedObjectsRoles Then
                    Dim lo_point As PointF = Me.zrPage.GetMidOfSelectedObjects
                    Dim larRoleInstance As New List(Of FBM.RoleInstance)
                    Dim lrRoleInstance As FBM.RoleInstance
                    For Each lrRoleInstance In Me.zrPage.SelectedObject
                        larRoleInstance.AddUnique(lrRoleInstance)
                    Next
                    Call Me.CreateEqualityConstraint(larRoleInstance, lo_point)
                End If
            Case Is = Keys.F
                If Me.zrPage.AreAllSelectedObjectsRoles Then
                    If Me.zrPage.SelectedObject.Count = 1 Then
                        '----------------------------
                        'User has selected one Role
                        '----------------------------
                        Dim lrRoleInstance As FBM.RoleInstance
                        lrRoleInstance = Me.zrPage.SelectedObject(0)
                        Call Me.CreateFrequencyConstraint(lrRoleInstance)
                    End If
                End If
            Case Is = (e.Control And Keys.L)
                Call CreateValueType()
            Case Is = Keys.ShiftKey
                If Me.zrPage.InternalUniquenessConstraintsExpanded Then
                    Call contract_InternalUniquenessConstraints()
                    Me.zrPage.InternalUniquenessConstraintsExpanded = False
                Else
                    Call expand_InternalUniquenessConstraints()
                    Me.zrPage.InternalUniquenessConstraintsExpanded = True
                End If
            Case Is = Keys.N
                If Me.zrPage.AreAllSelectedObjectsRoles Then
                    Dim lrRoleInstance As FBM.RoleInstance
                    Dim larRoleInstance As New List(Of FBM.RoleInstance)
                    For Each lrRoleInstance In Me.zrPage.SelectedObject
                        larRoleInstance.Add(lrRoleInstance)
                    Next
                    Dim lo_point As PointF = Me.zrPage.GetMidOfSelectedObjects
                    Call Me.CreateExternalUniquenessConstraint(larRoleInstance, lo_point)
                End If
            Case Is = Keys.S 'Subtype
                If ((Me.zrPage.SelectedObject.Count = 2) And Me.zrPage.are_all_SelectedObjects_entity_types) Or
                   ((Me.zrPage.SelectedObject.Count = 2) And Me.zrPage.areSelectedObjectsEntityTypeAndFactType) Then

                    Dim lrEntityType_1 As New FBM.EntityTypeInstance
                    Dim lrEntityType_2 As New FBM.EntityTypeInstance
                    Dim lrParentModelObject As FBM.ModelObject

                    lrParentModelObject = Me.zrPage.SelectedObject(1)

                    Select Case lrParentModelObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            lrEntityType_1 = Me.zrPage.SelectedObject(0)
                            lrEntityType_2 = Me.zrPage.SelectedObject(1)

                            Call lrEntityType_1.AddSubtypeConstraint(lrEntityType_2)
                        Case Is = pcenumConceptType.FactType

                            lrFactTypeInstance = New FBM.FactTypeInstance
                            lrFactTypeInstance = Me.zrPage.SelectedObject(1)

                            If lrFactTypeInstance.IsObjectified Then
                                lrEntityType_1 = Me.zrPage.SelectedObject(0)
                                lrEntityType_2 = lrFactTypeInstance.ObjectifyingEntityType

                                Call lrEntityType_1.AddSubtypeConstraint(lrEntityType_2)
                            Else
                                '--------------------------------------------------------------------------------------------------------
                                'Ignore, can't create a Subtype Constraint to a Parent that is not an EntityType or ObjectifiedFactType
                                '--------------------------------------------------------------------------------------------------------
                            End If

                    End Select


                End If
            Case Is = Keys.T
                '-----------------------------------
                'TotalInternalUniquenessConstraint
                '-----------------------------------
                If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
                If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.FactType Then
                    lrFactTypeInstance = CType(Me.zrPage.SelectedObject(0), FBM.FactTypeInstance)

                    Call Me.zrPage.SelectedObject.Clear()

                    Dim larRole As New List(Of FBM.Role)
                    Dim lrRoleInstance As New FBM.RoleInstance

                    For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
                        larRole.Add(lrRoleInstance.Role)
                    Next

                    Call lrFactTypeInstance.FactType.RemoveInternalUniquenessConstraints(True)

                    Call lrFactTypeInstance.FactType.CreateInternalUniquenessConstraint(larRole)
                End If
        End Select

    End Sub

    Public Function CreateEntityType(Optional ByVal as_entity_type_name As String = Nothing, Optional ByVal ab_use_entity_type_name_as_id As Boolean = False) As FBM.EntityTypeInstance

        Dim lrEntityType As New FBM.EntityType
        Dim ls_entity_type_name As String = ""
        Dim lo_point_client As Point
        Dim lo_point As PointF

        lo_point_client = New Point(Me.Width / 2, Me.Height / 2)
        lo_point = DiagramView.ClientToDoc(New Point(lo_point_client.X, lo_point_client.Y))

        '--------------------------------------------
        'Establish the EntityType for the dropped object
        '--------------------------------------------
        lrEntityType = Me.zrPage.Model.CreateEntityType(as_entity_type_name)

        '----------------------------------------------------
        'Create an EntityTypeInstance for the new EntityType
        '----------------------------------------------------
        Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

        '---------------------------------
        'Drop the EntityType on the Page
        '---------------------------------
        lrEntityTypeInstance = Me.zrPage.DropEntityTypeAtPoint(lrEntityType, lo_point, True)

        Return lrEntityTypeInstance

    End Function

    Public Function CreateValueType(Optional ByVal as_value_type_name As String = Nothing,
                                    Optional ByVal ab_use_value_type_name_as_id As Boolean = False,
                                    Optional ByVal aiDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet,
                                    Optional ByVal aiLength As Integer = Nothing,
                                    Optional ByVal aiPrecision As Integer = Nothing) As FBM.ValueTypeInstance

        Dim lo_ValueTypeInstance As FBM.ValueTypeInstance
        Dim ls_value_type_name As String = ""
        Dim lo_point_client As Point
        Dim lo_point As PointF
        lo_point_client = New Point(Me.Width / 2, Me.Height / 2)
        lo_point = DiagramView.ClientToDoc(New Point(lo_point_client.X, lo_point_client.Y))

        '============================================================
        Dim lrValueType As FBM.ValueType
        lrValueType = Me.zrPage.Model.CreateValueType(as_value_type_name)
        lrValueType.DataType = aiDataType
        If IsSomething(aiLength) Then
            lrValueType.DataTypeLength = aiLength
        End If
        If IsSomething(aiPrecision) Then
            lrValueType.DataTypePrecision = aiPrecision
        End If
        lo_ValueTypeInstance = Me.zrPage.DropValueTypeAtPoint(lrValueType, lo_point)
        '=================================================================

        Call lo_ValueTypeInstance.MoveToClearSpace()

        Return lo_ValueTypeInstance

    End Function

    Public Sub create_new_FactType_to_join_SelectedObjects()
        '-------------------------------------------------------------
        'Joins by way of a Fact Type the selected set of
        '  - Entities
        '  - Value Types
        '  - Objectified Fact Types
        '-------------------------------------------------------------

        Dim loObject As New Object
        Dim ls_FactType_name As String = ""
        Dim lrFactType As New FBM.FactType
        Dim lrFactTypeInstance As FBM.FactTypeInstance

        '------------------------------------------------
        'Construct the FactTypeName for the new FactType
        '  as an almalgamation of the Names of the 
        '  selected objects.
        '------------------------------------------------
        For Each loObject In Me.zrPage.SelectedObject
            Select Case loObject.ConceptType
                Case Is = pcenumConceptType.EntityType
                    ls_FactType_name &= loObject.name
                Case Is = pcenumConceptType.FactType
                    ls_FactType_name &= loObject.name
            End Select
        Next

        '-----------------------------------------------
        'Create the list of ModelObjects that the Roles
        '  within the FactType are linked to.
        '-----------------------------------------------
        Dim larModel_object As New List(Of FBM.ModelObject)
        For Each loObject In Me.zrPage.SelectedObject
            Select Case loObject.ConceptType
                Case Is = pcenumConceptType.EntityType
                    larModel_object.Add(loObject.EntityType)
                Case Is = pcenumConceptType.ValueType
                    larModel_object.Add(loObject.ValueType)
                Case Is = pcenumConceptType.FactType
                    larModel_object.Add(loObject.FactType)
            End Select
        Next

        '----------------------------------------
        'Create a new FactType for the ORMModel
        '----------------------------------------
        lrFactType = Me.zrPage.Model.CreateFactType(ls_FactType_name, larModel_object)

        '---------------------------------------------
        'Create the FactTypeInstance for the FactType
        '---------------------------------------------
        Dim lo_point As PointF = Me.zrPage.GetMidOfSelectedObjects
        Dim lrFactTypeInstance_pt As New PointF(lo_point.X - 5, lo_point.Y - 5)

        lrFactTypeInstance = Me.zrPage.DropFactTypeAtPoint(lrFactType, lrFactTypeInstance_pt, Me.ViewFactTablesToolStripMenuItem.Checked)


    End Sub

    Private Sub ORMDiagramView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown

        '----------------------------------------------------------------------------------------
        'Not true - NB Setting Node.Selected = True in this method stops the method LinkCreating, if you want to stop an object from having links drawn.

        'NB lrPropertyGridForm set here.

        Dim lo_point As System.Drawing.PointF
        Dim loNode As New ShapeNode
        Dim loSelectedNode As New Object

        Try
            Me.DiagramView.Behavior = Behavior.DrawLinks

            prApplication.WorkingPage = Me.zrPage

            lo_point = Me.DiagramView.ClientToDoc(e.Location)

            DiagramView.SmoothingMode = SmoothingMode.AntiAlias

            '---------------------------------------------------------------------------------------------------------------------
            'RichtClick handling is mostly taken care of in Diagram.NodeSelected
            '---------------------------------------------------------------------
            If e.Button = Windows.Forms.MouseButtons.Right Then
                loSelectedNode = Diagram.GetNodeAt(lo_point)
                Dim loSelectedLink As DiagramLink = Diagram.GetLinkAt(lo_point, 2)
                If (loSelectedNode Is Nothing) And (loSelectedLink Is Nothing) Then
                    Me.zrPage.SelectedObject.Clear()
                    Me.Diagram.Selection.Clear()
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
                ElseIf IsSomething(loSelectedNode) Then
                    '--------------------------------------------------------------------------------------------------------------------------
                    'VM-20160122-Commented out this code. Not sure what it is for and caused problems when Right-Clicking on a
                    '  multi-role InternalUniquenessConstraint. Cleared the selection, when the selection needed to be there to (optionally)
                    '  remove the InternalUniquenessConstraint.
                    '
                    '  If after some time things are going well and this code is not missed, then remove this commented-out code completely.
                    'If IsSomething(loSelectedNode) Then
                    '    Me.zrPage.SelectedObject.Clear()
                    '    Dim loDiagramNode As DiagramNode
                    '    loDiagramNode = loSelectedNode
                    '    loDiagramNode.Selected = True
                    'End If
                    '--------------------------------------------------------------------------------------------------------------------------
                    '------------------------------------------------------------------------------------
                    'Special handling for FactTypes.
                    '  FactTypeInstances are not added to zrPage.SelectedObject in Diagram.NodeSelected
                    '------------------------------------------------------------------------------------
                    If loSelectedNode.Tag.ConceptType = pcenumConceptType.FactType Then
                        Me.zrPage.SelectedObject.Clear()
                        Me.zrPage.SelectedObject.Add(loSelectedNode.Tag)
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_FactType
                    End If
                End If

                Exit Sub
            End If

            '--------------------------------------------------
            'Just to be sure...set the Boston.WorkingPage
            '--------------------------------------------------
            prApplication.WorkingPage = Me.zrPage

            If IsSomething(Diagram.GetNodeAt(lo_point)) Then
                '----------------------------
                'Mouse is over an ShapeNode
                '----------------------------

                '----------------------------------------------------
                'Get the Node/Shape under the mouse cursor.
                '----------------------------------------------------
                If TypeOf Diagram.GetNodeAt(lo_point) Is MindFusion.Diagramming.TableNode Then
                    Dim lrTableNode As New TableNode
                    lrTableNode = Diagram.GetNodeAt(lo_point)
                    Me.zrPage.SelectedObject.Clear()
                    lrTableNode.Selected = True
                    Exit Sub
                End If
                loNode = Diagram.GetNodeAt(lo_point)

                If Control.ModifierKeys And Keys.Control Then
                    '------------------------------------
                    'Use is holding down the CtrlKey so
                    '  enforce the selection of the object
                    '------------------------------------                

                    Me.zrPage.MultiSelectionPerformed = True

                    Select Case loNode.Tag.ConceptType
                        Case Is = pcenumConceptType.EntityTypeName
                            Dim lrEntityTypeName As FBM.EntityTypeName
                            lrEntityTypeName = loNode.Tag
                            lrEntityTypeName.EntityTypeInstance.Shape.Selected = True
                        Case Is = pcenumConceptType.Role
                            '----------------------------------------------------------------------
                            'This stops a Role AND its FactType being selected at the same time
                            '----------------------------------------------------------------------
                            Me.zrPage.SelectedObject.Remove(loNode.Tag.factType)
                        Case Is = pcenumConceptType.FactType
                            '---------------------------------------------------------------------------------------
                            'Special processing for FactTypes
                            '---------------------------------------------------------------------------------------
                            Dim lrFactTypeInstance As FBM.FactTypeInstance
                            lrFactTypeInstance = loNode.Tag
                            If Me.zrPage.SelectedObject.Contains(lrFactTypeInstance) Then
                                '-----------------------------------
                                'Already selected
                            Else
                                Me.zrPage.SelectedObject.Add(lrFactTypeInstance)
                            End If
                        Case Else
                            loNode.Selected = True
                            loNode.Pen.Color = Color.Blue
                            Me.Diagram.Invalidate()
                    End Select

                    '-------------------------------------------------------
                    'Don't allow UnconnectedLinks when doing a multiselect.
                    '-------------------------------------------------------
                    Me.Diagram.AllowUnconnectedLinks = False

                    Exit Sub
                Else
                    If Me.zrPage.MultiSelectionPerformed Then
                        If Diagram.Selection.Nodes.Contains(loNode) Then
                            '-----------------------------------------------------------------------
                            'Don't clear the SelectedObjects if the ShapeNode selected/clicked on 
                            '  is within the Diagram.Selection because the user has just performed
                            '  a MultiSelection, ostensibly (one would assume) to then 'move'
                            '  or 'delete' the selection of objects.
                            '-----------------------------------------------------------------------
                            '------------------------------------------------------------------------------
                            'Unless the Shape.Tag is a FactType, then just select it
                            '  The reason for this, is because of MindFusion.Groups.
                            '  When a User selects a Role...the whole RoleGroup (all Roles in the FactType)
                            '  are selected, so it is a MultiSelection by default.
                            '------------------------------------------------------------------------------
                            Select Case loNode.Tag.ConceptType
                                Case Is = pcenumConceptType.FactType
                                    Me.zrPage.SelectedObject.Clear()
                                    Diagram.Selection.Clear()
                                    '-----------------------------------------------
                                    'Select the ShapeNode/ORMObject just clicked on
                                    '-----------------------------------------------                    
                                    loNode.Selected = True
                                    loNode.Pen.Color = Color.Blue

                                    '-------------------------------------------------------------------
                                    'Reset the MultiSelectionPerformed flag on the ORMModel
                                    '-------------------------------------------------------------------
                                    Me.zrPage.MultiSelectionPerformed = False
                            End Select
                        Else
                            '---------------------------------------------------------------------------
                            'Clear the SelectedObjects because the user neither did a MultiSelection
                            '  nor held down the [Ctrl] key before clicking on the ShapeNode.
                            '  Clearing the SelectedObject groups, allows for new objects to be selected
                            '  starting with the ShapeNode/ORMObject just clicked on.
                            '---------------------------------------------------------------------------
                            Me.zrPage.SelectedObject.Clear()
                            Diagram.Selection.Clear()
                            '-----------------------------------------------
                            'Select the ShapeNode/ORMObject just clicked on
                            '-----------------------------------------------                                            
                            loNode.Pen.Color = Color.Blue

                            Select Case loNode.Tag.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    loNode.Selected = True
                                Case Is = pcenumConceptType.Role
                                    '---------------------------------------------------------------------
                                    'Deselect the Role.FactType from the list of ORMModel.SelectedObjects
                                    '  because the user clicked on the Role rather than the FactType,
                                    '  but the FactType is added to the ORMModel.SelectedObjects
                                    '  when the 'Shape' is 'Selected' so that the 'Group' is selected for 
                                    '  dragging purposes.
                                    '---------------------------------------------------------------------     
                                    loNode.Locked = False
                                    Me.Diagram.AllowUnconnectedLinks = True
                                    Me.zrPage.SelectedObject.Remove(loNode.Tag.factType)
                                Case Is = pcenumConceptType.RoleConstraint
                                    Select Case loNode.Tag.RoleConstraintType
                                        Case Is = pcenumRoleConstraintType.RingConstraint
                                            '----------------------------
                                            'Mouse is over an ShapeNode
                                            '----------------------------
                                            Me.Diagram.AllowUnconnectedLinks = True
                                            Me.DiagramView.DrawLinkCursor = Cursors.Hand
                                            Cursor.Show()
                                    End Select
                            End Select

                            '-------------------------------------------------------------------
                            'Eitehr way, reset the MultiSelectionPerformed flag on the ORMModel
                            '-------------------------------------------------------------------
                            Me.zrPage.MultiSelectionPerformed = False

                        End If
                    Else 'MultiSelectionPerformed (below is Not MultiSelectionPerformed )
                        '---------------------------------------------------------------------------
                        'Clear the SelectedObjects because the user neither did a MultiSelection
                        '  nor held down the [Ctrl] key before clicking on the ShapeNode.
                        '  Clearing the SelectedObject groups, allows for new objects to be selected
                        '  starting with the ShapeNode/ORMObject just clicked on.
                        '---------------------------------------------------------------------------
                        Me.zrPage.SelectedObject.Clear()
                        Diagram.Selection.Clear()
                        '-----------------------------------------------
                        'Select the ShapeNode/ORMObject just clicked on
                        '-----------------------------------------------                                                      
                        Select Case loNode.Tag.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                loNode.Pen.Color = Color.Blue
                                loNode.Selected = True
                                Me.Diagram.AllowUnconnectedLinks = False
                                Me.Diagram.AllowUnanchoredLinks = False
                            Case Is = pcenumConceptType.EntityTypeName
                                loNode.Selected = True
                            Case Is = pcenumConceptType.ValueType
                                loNode.Pen.Color = Color.Blue
                                loNode.Selected = True

                            Case Is = pcenumConceptType.FactType
                                Dim lrFactTypeInstance As FBM.FactTypeInstance
                                lrFactTypeInstance = loNode.Tag
                                Call lrFactTypeInstance.MouseDown()
                                loNode.Selected = True
                            Case Is = pcenumConceptType.FactTypeName
                                loNode.Selected = True
                            Case Is = pcenumConceptType.FactTypeDerivationText
                                loNode.Selected = True
                            Case Is = pcenumConceptType.RoleName
                                loNode.Selected = True
                            Case Is = pcenumConceptType.RoleConstraint
                                loNode.Selected = True
                                Me.Diagram.AllowUnconnectedLinks = True
                                Me.Diagram.AllowUnanchoredLinks = True
                            Case Is = pcenumConceptType.Role
                                '-----------------------------------------------------------------------
                                'Deselect the Role.FactType from the list of ORMModel.SelectedObjects
                                '  because the user clicked on the Role rather than the FactType,
                                '  but the FactType is added to the ORMModel.SelectedObjects
                                '  when the 'Shape' is 'Selected' so that the 'Group' is selected for 
                                '  dragging purposes.
                                '-----------------------------------------------------------------------                            
                                Me.zrPage.SelectedObject.Remove(loNode.Tag.factType)
                                Me.Diagram.AllowUnconnectedLinks = True
                                Me.Diagram.AllowUnanchoredLinks = True
                                loNode.Locked = False
                                loNode.AllowOutgoingLinks = True

                                Dim lrRoleInstance As FBM.RoleInstance
                                lrRoleInstance = loNode.Tag

                                If IsSomething(lrRoleInstance.Link) Then
                                    lrRoleInstance.Link.Pen.Color = Color.LightGray
                                End If
                                Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
                                    If IsSomething(lrEntityTypeInstance.Shape) Then
                                        lrEntityTypeInstance.Shape.AllowIncomingLinks = True
                                    End If
                                Next

                            Case Is = pcenumConceptType.RoleConstraint
                                Select Case loNode.Tag.RoleConstraintType
                                    Case Is = pcenumRoleConstraintType.RingConstraint,
                                              pcenumRoleConstraintType.ExclusionConstraint,
                                              pcenumRoleConstraintType.ExternalUniquenessConstraint

                                        '----------------------------
                                        'Mouse is over an ShapeNode
                                        '----------------------------
                                        Me.Diagram.AllowUnconnectedLinks = True
                                        Me.DiagramView.DrawLinkCursor = Cursors.Hand
                                        Cursor.Show()
                                End Select
                            Case Is = pcenumConceptType.ModelNote
                                loNode.Selected = True
                        End Select

                        '-------------------------------------------------------
                        'Reset the MultiSelectionPerformed flag on the ORMModel
                        '-------------------------------------------------------
                        Me.zrPage.MultiSelectionPerformed = False
                    End If
                End If

                'ORMDiagramView.DrawLinkCursor = Cursors.SizeAll
                Cursor.Show()

                '-----------------------------------------------------------------------------------------------------------------------
                'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the ORMModel object selected
                '-----------------------------------------------------------------------------------------------------------------------
                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)


                If IsSomething(lrPropertyGridForm) And IsSomething(loNode) Then
                    Dim lrModelObject As FBM.ModelObject
                    lrModelObject = loNode.Tag
                    lrPropertyGridForm.PropertyGrid.BrowsableAttributes = Nothing
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                    Select Case lrModelObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            Dim lrValueTypeInstance As FBM.ValueTypeInstance
                            lrValueTypeInstance = lrModelObject
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            Select Case lrValueTypeInstance.DataType
                                Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                          pcenumORMDataType.NumericDecimal,
                                          pcenumORMDataType.NumericMoney
                                    Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                    Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                Case Is = pcenumORMDataType.RawDataFixedLength,
                                          pcenumORMDataType.RawDataLargeLength,
                                          pcenumORMDataType.RawDataVariableLength,
                                          pcenumORMDataType.TextFixedLength,
                                          pcenumORMDataType.TextLargeLength,
                                          pcenumORMDataType.TextVariableLength
                                    Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                    Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Case Else
                                    Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            End Select
                            If lrPropertyGridForm.PropertyGrid.SelectedObject IsNot Nothing Then
                                lrPropertyGridForm.PropertyGrid.SelectedObject = New Object
                            End If
                            lrPropertyGridForm.zrSelectedObject = lrValueTypeInstance
                            lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrValueTypeInstance
                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                            lrEntityTypeInstance = lrModelObject
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            If lrEntityTypeInstance.EntityType.HasSimpleReferenceScheme Then
                                Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataType", True)
                                Select Case lrEntityTypeInstance.DataType
                                    Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                              pcenumORMDataType.NumericDecimal,
                                              pcenumORMDataType.NumericMoney
                                        Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                        Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                    Case Is = pcenumORMDataType.RawDataFixedLength,
                                              pcenumORMDataType.RawDataLargeLength,
                                              pcenumORMDataType.RawDataVariableLength,
                                              pcenumORMDataType.TextFixedLength,
                                              pcenumORMDataType.TextLargeLength,
                                              pcenumORMDataType.TextVariableLength
                                        Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                        Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Case Else
                                        Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                        Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                End Select
                            Else
                                Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataType", False)
                                Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            End If
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeInstance
                            lrPropertyGridForm.PropertyGrid.Refresh()
                        Case Is = pcenumConceptType.EntityTypeName
                            Dim lrEntityTypeName As FBM.EntityTypeName
                            lrEntityTypeName = lrModelObject
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            If lrEntityTypeName.EntityTypeInstance.EntityType.HasSimpleReferenceScheme Then
                                Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataType", True)
                                Select Case lrEntityTypeName.EntityTypeInstance.DataType
                                    Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                              pcenumORMDataType.NumericDecimal,
                                              pcenumORMDataType.NumericMoney
                                        Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                        Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                    Case Is = pcenumORMDataType.RawDataFixedLength,
                                              pcenumORMDataType.RawDataLargeLength,
                                              pcenumORMDataType.RawDataVariableLength,
                                              pcenumORMDataType.TextFixedLength,
                                              pcenumORMDataType.TextLargeLength,
                                              pcenumORMDataType.TextVariableLength
                                        Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                        Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Case Else
                                        Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                        Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                End Select
                            Else
                                Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataType", False)
                            End If
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeName.EntityTypeInstance
                            lrPropertyGridForm.PropertyGrid.Refresh()
                        Case Is = pcenumConceptType.RoleConstraintRole
                            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                            lrRoleConstraintRoleInstance = lrModelObject
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintRoleInstance.RoleConstraint
                        Case Is = pcenumConceptType.RoleConstraint
                            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                            lrRoleConstraintInstance = lrModelObject

                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            Select Case lrRoleConstraintInstance.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                    Dim lrFrequencyConstraintInstance As FBM.FrequencyConstraint
                                    lrFrequencyConstraintInstance = lrModelObject
                                    lrPropertyGridForm.PropertyGrid.SelectedObject = lrFrequencyConstraintInstance
                                Case Else
                                    lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
                            End Select
                        Case Else
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelObject
                    End Select

                End If

                Me.Diagram.Invalidate()

            Else
                '---------------------------------------------------
                'MouseDown is on canvas (not on object).
                'If any objects are already highlighted as blue, 
                '  then change the outline to black/originalcolour
                '---------------------------------------------------
                Call Me.SaveAnyRoleConstraintArgumentsBeingCreated()
                Call Me.CancelAllRoleConstraintArgumentCreationStati()

                If Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.ORMSubtypeConnector Then
                    Me.zrSpecialDragMode.ResetSpecialDragMode()
                End If

                '----------------------------
                'Mouse is over the Canvas
                '----------------------------
                Me.Diagram.AllowUnconnectedLinks = False

                '-------------------------------------------------------
                'Set the context menu to the default for the Diagram,
                '  because the User did not click on a ShapeNode.
                '-------------------------------------------------------
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

                '---------------------------------------------------------
                'Set the PropertiesGrid.SeletedObject to the Page itself
                '---------------------------------------------------------
                Dim lrPropertyGridForm As frmToolboxProperties

                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                If IsSomething(lrPropertyGridForm) Then

                    Dim myfilterattribute As Attribute = New System.ComponentModel.CategoryAttribute("Page")
                    ' And you pass it to the PropertyGrid,
                    ' via its BrowsableAttributes property :
                    lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage

                End If

                '----------------------------------
                'Clear the selected items (if any)
                '----------------------------------
                Diagram.Selection.Clear()
                If prApplication.WorkingPage Is Nothing Then
                    prApplication.WorkingPage = Me.zrPage
                End If
                Me.zrPage.SelectedObject.Clear()
                Me.PerformCleanup()

                Call Me.ResetNodeAndLinkColors()

                '-------------------------------------------------------
                'ORM Verbalisation
                '-------------------------------------------------------
                Dim lrToolboxForm As frmToolboxORMVerbalisation
                lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
                If IsSomething(lrToolboxForm) Then
                    Call lrToolboxForm.VerbalisePage(Me.zrPage)
                End If

                Me.DiagramView.Refresh()

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ORMDiagramView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseMove

        Try
            '---------------------------------------------------------------------------------------------
            'Stops bug in Mindfusion where mouse event is unhandled and DiagramView thinks user is
            '  dragging to select DiagramNodes/Shapes
            '---------------------------------------------------------------------------------------------
            If e.Button = Windows.Forms.MouseButtons.None Then
                If Not Me.zrPage.MultiSelectionPerformed Then
                    Me.DiagramView.Behavior = Behavior.DoNothing
                End If
            Else
                Me.DiagramView.Behavior = Behavior.DrawLinks
            End If

            '-------------------------------------------------------------------------------
            'Allow creation of links from specific RoleConstraints to Subtype Relationships.
            '-------------------------------------------------------------------------------
            Me.Diagram.AllowUnconnectedLinks = True
            Dim clientPt As Point = New Point(e.X, e.Y)
            Dim docPt As PointF = Me.DiagramView.ClientToDoc(clientPt)
            Dim arrow As DiagramLink = Me.Diagram.GetLinkAt(docPt, 1, True)
            If arrow Is Nothing Then
                'Me.DiagramView.AllowLinkCursor = Cursors.No
            Else
                Me.DiagramView.AllowLinkCursor = Cursors.Hand
            End If

            '--------------------------------------------------------------------------------------------
            'If the ModelDictionary toolbox is loaded, update the ModelDictionary for the current Model
            '--------------------------------------------------------------------------------------------
            Dim child As frmToolboxModelDictionary
            If e.Button = Windows.Forms.MouseButtons.None Then
                If prApplication.RightToolboxForms.FindAll(Function(x) x.Name = frmToolboxModelDictionary.Name).Count >= 1 Then
                    child = prApplication.RightToolboxForms.Find(Function(x) x.Name = frmToolboxModelDictionary.Name)
                    child.zrORMModel = prApplication.WorkingModel
                    If IsSomething(prApplication.WorkingPage) _
                    And child.zrLoadedModel IsNot prApplication.WorkingModel Then
                        Call child.LoadToolboxModelDictionary(prApplication.WorkingPage.Language)
                    End If
                End If
            End If


            If Me.zrSpecialDragMode.SpecialDragMode = pcenumSpecialDragMode.ORMSubtypeConnector Then
                Dim lrE As New System.Windows.Forms.GiveFeedbackEventArgs(DragDropEffects.Copy, False)
                If System.Windows.Forms.Form.MouseButtons = Windows.Forms.MouseButtons.Left Then
                    Call Me.DiagramView.DoDragDrop(New Object, DragDropEffects.Copy)
                Else
                    Me.zrSpecialDragMode.MouseUpCounter = 0
                End If
            Else
                Me.DiagramView.Cursor = Cursors.Default
            End If

            Dim lo_point As System.Drawing.PointF
            Dim loNode As New ShapeNode

            If e.Button = Windows.Forms.MouseButtons.Right Then
                Exit Sub
            End If

            '------------------------------------------------
            'Check to see if the Cursor is over a ShapeNode
            '------------------------------------------------
            lo_point = Diagram.PixelToUnit(e.Location)

            If TypeOf Diagram.GetNodeAt(lo_point) Is TableNode Then
                Me.LabelHelp.Text = "DoubleClick the header of the Fact Table to show or hide the Facts for the Fact Type"
                Exit Sub
            End If

            Dim lrRoleInstance As FBM.RoleInstance
            Dim lrOtherRoleInstance As FBM.RoleInstance

            If IsSomething(Diagram.GetNodeAt(lo_point)) Then
                '----------------------------------------------
                'Mouse is over a ShapeNode
                '----------------------------------------------
                loNode = Diagram.GetNodeAt(lo_point)
                If loNode.Tag Is Nothing Then Exit Sub 'Sometimes this happens.

                Dim lrRoleName As FBM.RoleName
                Select Case loNode.Tag.ConceptType
                    Case Is = pcenumConceptType.Role
                        'loNode.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)

                        lrRoleInstance = loNode.Tag
                        lrRoleInstance.RoleName.Shape.TextColor = Color.BlueViolet
                        For Each lrOtherRoleInstance In Me.zrPage.RoleInstance
                            If Not (lrOtherRoleInstance Is lrRoleInstance) Then
                                'lrOtherRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                                lrOtherRoleInstance.RoleName.SetAppropriateColour()
                            End If
                        Next

                    Case Is = pcenumConceptType.RoleName
                        lrRoleName = loNode.Tag
                        lrRoleName.Shape.TextColor = Color.BlueViolet
                        lrRoleName.RoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)
                        For Each lrOtherRoleInstance In Me.zrPage.RoleInstance
                            If Not (lrOtherRoleInstance Is lrRoleName.RoleInstance) Then
                                lrOtherRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                                lrOtherRoleInstance.RoleName.SetAppropriateColour()
                            End If
                        Next
                    Case Is = pcenumConceptType.EntityType
                        Me.DiagramView.AllowLinkCursor = Cursors.Hand
                    Case Is = pcenumConceptType.ValueType
                        Me.DiagramView.AllowLinkCursor = Cursors.Hand
                    Case Else
                        For Each lrRoleInstance In Me.zrPage.RoleInstance
                            'lrRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                            lrRoleInstance.RoleName.SetAppropriateColour()
                        Next
                End Select
            Else
                For Each lrRoleInstance In Me.zrPage.RoleInstance
                    lrRoleInstance.RoleName.SetAppropriateColour()
                Next
            End If

            Me.DiagramView.Invalidate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub ORMDiagramView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseUp

        Dim liInd As Integer = 0
        Dim lo_point As System.Drawing.PointF
        Dim loNode As New ShapeNode

        '----------------------------------------------------
        'Check to see if the user has used the Control key to
        '  do a multi-select
        '----------------------------------------------------
        If Control.ModifierKeys And Keys.Control Then
            Exit Sub
        End If

        If e.Button = Windows.Forms.MouseButtons.Right Then
            Exit Sub
        End If

        '-------------------------------------------------------
        'Check to see if the user was clicking over a ShapeNode
        '-------------------------------------------------------
        lo_point = Me.DiagramView.ClientToDoc(e.Location)

        For Each lrRoleInstance In Me.zrPage.RoleInstance
            lrRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
            lrRoleInstance.RoleName.SetAppropriateColour()
        Next

        If TypeOf Diagram.GetNodeAt(lo_point) Is TableNode Then
            Me.Diagram.Invalidate()
            Exit Sub
        End If

        If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            '----------------------------------------------
            'Mouse is over a ShapeNode
            '----------------------------------------------

            '----------------------------------------------
            'Reset the cursor to a hand
            '----------------------------------------------
            DiagramView.DrawLinkCursor = Cursors.Hand
            Cursor.Show()

            '-------------------------------------------------------------------------------------------
            'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
            '-------------------------------------------------------------------------------------------            
            If Not IsNothing(loNode.Tag) Then
                loNode.Tag.x = loNode.Bounds.X
                loNode.Tag.y = loNode.Bounds.Y
            End If

            Call Me.ResetNodeAndLinkColors()
        Else
            '------------------------------------------------------------------
            'Mouse is over the canvas and a MultiSelection may have taken place
            '------------------------------------------------------------------

        End If

        '---------------------------------------------
        'Refresh the Diagram drawing (ShapeNode/Links)
        '---------------------------------------------
        Diagram.Invalidate()

        If Me.zrPage.SelectedObject.Count = 0 Then
            frmMain.ToolStripButtonCopy.Enabled = False
            'frmMain.ToolStripButtonCut.Enabled = False
        Else
            frmMain.ToolStripButtonCopy.Enabled = True
            'frmMain.ToolStripButtonCut.Enabled = True

            Select Case Me.zrPage.SelectedObject(0).ConceptType
                Case Is = pcenumConceptType.RoleConstraint
                    Call frmMain.focusVerbalisationToolbox()
                Case Is = pcenumConceptType.FactType
                    Dim lrFactTypeInstance As FBM.FactTypeInstance = Me.zrPage.SelectedObject(0)
                    If lrFactTypeInstance.FactType.FactTypeReading.Count = 0 Then
                        Call frmMain.focusFactTypeReadingToolbox()
                    End If
            End Select
        End If

    End Sub

    Private Sub Diagram_NodeModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeModified

        Dim loNode As New ShapeNode

        '-----------------------------------------
        'Set the 'Dirty' flag on the Page to True
        '-----------------------------------------        
        frmMain.ToolStripButton_Save.Enabled = True

        If IsSomething(e.Node.Tag) Then

            Dim lrUserAction As New tUserAction(e.Node.Tag, pcenumUserAction.MoveModelObject, Me.zrPage)
            lrUserAction.PreActionModelObject = New Boston.tUndoRedoObject(e.Node.Tag.X, e.Node.Tag.Y)

            e.Node.Tag.x = e.Node.Bounds.X
            e.Node.Tag.y = e.Node.Bounds.Y

            Dim loORMObject As Object = e.Node.Tag
            '==============================================================================
            'Broadcast the moving of the Object
            '  NB See also: SelectionMoved. Need this code in both places for some reason VM-20180316
            If My.Settings.UseClientServer And My.Settings.InitialiseClient Then

                Dim lrModel As New Viev.FBM.Interface.Model
                Dim lrPage As New Viev.FBM.Interface.Page()

                lrModel.ModelId = Me.zrPage.Model.ModelId
                lrPage.Id = Me.zrPage.PageId
                lrPage.ConceptInstance = New Viev.FBM.Interface.ConceptInstance
                lrPage.ConceptInstance.X = loORMObject.Shape.Bounds.X
                lrPage.ConceptInstance.Y = loORMObject.Shape.Bounds.Y
                lrPage.ConceptInstance.ModelElementId = loORMObject.Id
                lrModel.Page = lrPage

                Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                lrBroadcast.Model = lrModel
                Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.PageMovePageObject, lrBroadcast)

            End If
            '==============================================================================

            'lrUserAction.PostActionModelObject = New Boston.tUndoRedoObject(e.Node.Bounds.X, e.Node.Bounds.Y)
            'prApplication.AddUndoAction(lrUserAction)
            'frmMain.ToolStripMenuItemUndo.Enabled = True

            Select Case e.Node.Tag.ConceptType
                Case Is = pcenumConceptType.EntityType,
                          pcenumConceptType.ValueType
                    Call Me.SortJoiningFactTypes(e.Node.Tag)
                Case Is = pcenumConceptType.FactType
                    '-------------------------------------------------------------------
                    'Set the X,Y coordinants of the Roles within the FactType/RoleGroup
                    '-------------------------------------------------------------------
                    Dim lo_role_instance As FBM.RoleInstance

                    For Each lo_role_instance In e.Node.Tag.RoleGroup
                        lo_role_instance.X = lo_role_instance.Shape.Bounds.X
                        lo_role_instance.Y = lo_role_instance.Shape.Bounds.Y
                    Next
            End Select
        End If

        '==============================================================================
        'Broadcast the moving of the Object is done in Me.SelectionMoved
        '==============================================================================

        Call Me.zrPage.MakeDirty() 'Leave this as last call, because triggers events

    End Sub


    Private Sub Diagram_NodeSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeSelected

        '--------------------------------------------------------------------------------
        'NB IMPORTANT: lrPropertyGridForm: performed in ORMDiagram.MouseDown. Management of which object is displayed in the PropertyGrid
        'NB Nodes clicked on receive the NodeMouseDown event before this one. 
        '--------------------------------------------------------------------------------
        Try

            If My.Settings.UseClientServer Then
                If Not prApplication.User.ProjectPermission.FindAll(Function(x) x.Permission = pcenumPermission.Alter).Count > 0 Then
                    Dim lfrmFlashCard As New frmFlashCard
                    lfrmFlashCard.ziIntervalMilliseconds = 3500
                    lfrmFlashCard.zsText = "Please note that you do not have Alter Permission on this Project."
                    lfrmFlashCard.Show(Me) 'Dim liDialogResult As DialogResult = 

                    Me.DiagramView.Focus()
                    Exit Sub
                End If
            End If

            '---------------------------------------------
            'Do specific Object/ConceptType processing
            ' Adding to Page.SelectedObject
            '---------------------------------------------
            Dim laiValidConceptTypes() As pcenumConceptType = {pcenumConceptType.Role,
                                                               pcenumConceptType.FactType}
            If Not laiValidConceptTypes.Contains(e.Node.Tag.ConceptType) Then
                Call Me.zrPage.Model.RemoveRolesThatReferenceNothing(True)
            End If

            If TypeOf (e.Node) Is ShapeNode Then

                Select Case e.Node.Tag.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        Me.zrPage.SelectedObject.Add(e.Node.Tag)
                    Case Is = pcenumConceptType.ValueType
                        Me.zrPage.SelectedObject.Add(e.Node.Tag)
                    Case Is = pcenumConceptType.FactType
                        'Not done here, because when a Role is selected, the FactType of the Role is selected also.
                        '  This is so that when a Role is moved, the FactType(Instance) moves also.
                        Call e.Node.Tag.AdjustBorderHeight()
                    Case Is = pcenumConceptType.FactTable
                        Me.zrPage.SelectedObject.Add(e.Node.Tag)
                    Case Is = pcenumConceptType.EntityTypeName
                        'N/A
                    Case Is = pcenumConceptType.Role
                        If Not Me.zrPage.SelectedObject.Contains(e.Node.Tag) Then
                            Me.zrPage.SelectedObject.Add(e.Node.Tag)
                        End If
                    Case Is = pcenumConceptType.FactTypeName
                        'N/A
                    Case Is = pcenumConceptType.FactTypeReading
                        'N/A
                    Case Is = pcenumConceptType.RoleConstraint
                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                        lrRoleConstraintInstance = e.Node.Tag
                        If Me.zrPage.IsObjectSelected(lrRoleConstraintInstance) Then
                            '-------------------------------
                            'Don't select the object again
                            '-------------------------------
                        Else
                            Me.zrPage.SelectedObject.Add(lrRoleConstraintInstance)
                        End If
                    Case Is = pcenumConceptType.RoleConstraintRole
                        'N/A
                    Case Is = pcenumConceptType.ModelNote
                        If Not Me.zrPage.SelectedObject.Contains(e.Node.Tag) Then
                            Me.zrPage.SelectedObject.Add(e.Node.Tag)
                        End If
                    Case Else
                        'N/A
                End Select
            ElseIf TypeOf (e.Node) Is TableNode Then
                Select Case e.Node.Tag.ConceptType
                    Case Is = pcenumConceptType.FactTable
                        Me.zrPage.SelectedObject.Add(e.Node.Tag)
                End Select

            End If

            '------------------------------------------------------
            'Set the ContextMenuStrip menu for the selected item.
            '------------------------------------------------------
            Select Case e.Node.Tag.ConceptType 'Me.Diagram.Selection.Items(0).Tag.ConceptType
                Case Is = pcenumConceptType.Role
                    Dim lrRoleInstance As FBM.RoleInstance
                    lrRoleInstance = e.Node.Tag
                    If lrRoleInstance.Mandatory Then
                        mnuOption_Mandatory.Checked = True
                    Else
                        mnuOption_Mandatory.Checked = False
                    End If
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Role
                Case Is = pcenumConceptType.FactType
                    Dim lrFactTypeInstance As New FBM.FactTypeInstance
                    lrFactTypeInstance = e.Node.Tag
                    If Me.zrPage.SelectedObject.Count > 0 Then
                        If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.FactType Then
                            Me.DiagramView.ContextMenuStrip = ContextMenuStrip_FactType
                            mnuOption_IsObjectified.Checked = lrFactTypeInstance.IsObjectified
                            Me.ToolStripMenuItemFactTypeInstanceRemoveFromPage.Enabled = Not lrFactTypeInstance.isPreferredReferenceMode
                        End If
                    End If
                    Call lrFactTypeInstance.Selected()
                Case Is = pcenumConceptType.EntityType
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_EntityType
                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                    lrEntityTypeInstance = e.Node.Tag
                    Call lrEntityTypeInstance.NodeSelected()
                Case Is = pcenumConceptType.EntityTypeName
                    Dim lrEntityTypeName As FBM.EntityTypeName
                    lrEntityTypeName = e.Node.Tag
                    Call lrEntityTypeName.EntityTypeInstance.NodeSelected()
                Case Is = pcenumConceptType.FactTable
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_FactTable

                    Dim lo_table_node As TableNode = e.Node.Tag.TableShape
                    lo_table_node.TextColor = Color.Black
                    DeleteRowToolStripMenuItem.Enabled = False
                    Me.DeleteRowFactFromPageAndModelToolStripMenuItem.Enabled = False
                Case Is = pcenumConceptType.ValueType
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_ValueType
                Case Is = pcenumConceptType.RoleConstraint
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    lrRoleConstraintInstance = e.Node.Tag
                    Select Case lrRoleConstraintInstance.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.FrequencyConstraint
                            Dim lrFrequencyConstraint As FBM.FrequencyConstraint
                            lrFrequencyConstraint = e.Node.Tag
                            Me.DiagramView.ContextMenuStrip = Me.ContextMenuStrip_FrequencyConstraint
                        Case Is = pcenumRoleConstraintType.RingConstraint
                            Me.DiagramView.ContextMenuStrip = ContextMenuStrip_RingConstraint
                        Case Is = pcenumRoleConstraintType.ExclusionConstraint,
                                  pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                  pcenumRoleConstraintType.EqualityConstraint,
                                  pcenumRoleConstraintType.SubsetConstraint,
                                  pcenumRoleConstraintType.ExclusiveORConstraint,
                                  pcenumRoleConstraintType.InclusiveORConstraint
                            Me.DiagramView.ContextMenuStrip = ContextMenuStrip_ExternalRoleConstraint
                    End Select
                    Call lrRoleConstraintInstance.NodeSelected()
                Case Is = pcenumConceptType.RoleConstraintRole
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                    lrRoleConstraintRoleInstance = e.Node.Tag
                    Select Case lrRoleConstraintRoleInstance.RoleConstraint.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                            Me.DiagramView.ContextMenuStrip = Me.ContextMenuStrip_InternalUniquenessConstraint
                    End Select
                Case Is = pcenumConceptType.ModelNote
                    Me.DiagramView.ContextMenuStrip = Me.ContextMenuStripModelNote
                Case Else
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
            End Select

            '-------------------------------------------------------
            'ORM Verbalisation
            '-------------------------------------------------------
            Dim lrToolboxForm As frmToolboxORMVerbalisation
            lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
            If IsSomething(lrToolboxForm) Then
                lrToolboxForm.zrModel = Me.zrPage.Model
                Select Case e.Node.Tag.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        Call lrToolboxForm.VerbaliseEntityType(e.Node.Tag.EntityType)
                    Case Is = pcenumConceptType.ValueType
                        Call lrToolboxForm.VerbaliseValueType(e.Node.Tag.ValueType)
                    Case Is = pcenumConceptType.FactType
                        Call lrToolboxForm.VerbaliseFactType(e.Node.Tag.FactType)
                    Case Is = pcenumConceptType.Role
                        If Control.ModifierKeys And Keys.Control Then
                            Call lrToolboxForm.verbaliseRole(e.Node.Tag.Role)
                        End If
                    Case Is = pcenumConceptType.FactTable
                        '----------------------------------------------------------------------------------
                        'See frmDiagramORM.Diagram.CellClicked
                        '  The Fact selected for verbalisation is set there and event comes after 
                        '  NodeSelected.
                        '----------------------------------------------------------------------------------
                    Case Is = pcenumConceptType.RoleConstraint
                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                        lrRoleConstraintInstance = e.Node.Tag
                        Select Case lrRoleConstraintInstance.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.SubsetConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintSubset(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.EqualityConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintEqualityConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.ExclusionConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintExclusionConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.InclusiveORConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintInclusiveORConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.ExclusiveORConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintExclusiveORConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintUniquenessConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintFrequencyConstraint(lrRoleConstraintInstance.RoleConstraint)
                            Case Is = pcenumRoleConstraintType.RingConstraint
                                Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
                        End Select
                End Select
            End If

            '---------------------------------------------
            'Do specific Object/ConceptType processing
            '---------------------------------------------
            If TypeOf (e.Node) Is ShapeNode Then

                Select Case e.Node.Tag.ConceptType
                    Case Is = pcenumConceptType.EntityType

                    Case Is = pcenumConceptType.ValueType

                    Case Is = pcenumConceptType.FactType
                        Dim lrFactTypeInstance As FBM.FactTypeInstance
                        lrFactTypeInstance = e.Node.Tag
                        'Call lrFactTypeInstance.NodeSelected()
                    Case Is = pcenumConceptType.EntityTypeName
                        e.Node.Tag.EntityTypeInstance.Shape.Selected = True
                    Case Is = pcenumConceptType.Role


                        '-------------------------------------------------------------------------------------
                        'Make sure that the FactTypeInstance of every RoleGroup is selected
                        'IMPORTANT: Without the following, the FactType.Shape does not 'move' correctly when 
                        ' dragging the associated Role.Shapenodes.
                        '-------------------------------------------------------------------------------------
                        Dim lrRoleInstance As FBM.RoleInstance
                        lrRoleInstance = e.Node.Tag
                        'e.Node.Locked = False
                        'lrRoleInstance.FactType.Shape.Selected = True
                        If lrRoleInstance.FactType.IsObjectified Then
                            lrRoleInstance.FactType.Shape.Pen.Color = Color.Black
                        Else
                            lrRoleInstance.FactType.Shape.Pen.Color = Color.LightGray
                        End If
                        lrRoleInstance.FactType.FactTypeName.Shape.TextColor = Color.BlueViolet
                        lrRoleInstance.FactType.Shape.Visible = True
                        e.Node.Pen.Color = Color.Blue
                    Case Is = pcenumConceptType.FactTypeName
                        Dim lrFactTypeName As New FBM.FactTypeName
                        lrFactTypeName = e.Node.Tag
                        lrFactTypeName.FactTypeInstance.Shape.Visible = True
                        lrFactTypeName.FactTypeInstance.Shape.Pen.Color = Color.BlueViolet
                    Case Is = pcenumConceptType.FactTypeReading
                        Dim lrFactTypeReadingInstance As New FBM.FactTypeReadingInstance
                        lrFactTypeReadingInstance = e.Node.Tag
                        lrFactTypeReadingInstance.FactType.Shape.Visible = True
                        lrFactTypeReadingInstance.FactType.Shape.Pen.Color = Color.BlueViolet
                    Case Is = pcenumConceptType.RoleConstraint
                        'NB See FBM.RoleConstraint.NodeSelected for setting the Argument/RoleConstraintRole #s for a RoleConstraint with Arguments
                    Case Is = pcenumConceptType.RoleConstraintRole
                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                        lrRoleConstraintInstance = e.Node.Tag.RoleConstraint
                        For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                            lrRoleConstraintRoleInstance.Shape.Pen.Color = Color.Blue
                            Me.zrPage.AddSelectedObject(lrRoleConstraintRoleInstance)
                            If lrRoleConstraintInstance.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                                lrRoleConstraintRoleInstance.Role.FactType.Shape.Selected = True
                            End If
                            Me.Diagram.Invalidate()
                        Next
                    Case Is = pcenumConceptType.ModelNote
                        e.Node.AllowOutgoingLinks = True
                    Case Else
                        e.Node.Pen.Color = Color.Blue
                End Select

                '-----------
                'Hint Text
                '-----------
                Me.LabelHelp.Text = ""

                If Me.zrPage.AreSelectedObjectsMultipleObjectTypes Then
                    Me.LabelHelp.Text &= "Hint: Press the [R] key to create a Fact Type where the Roles join the Model Objects you have selected."
                    Me.LabelHelp.Text &= vbCrLf
                End If

                Select Case e.Node.Tag.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        If Me.zrPage.MultiSelectionPerformed Then
                            If Me.zrPage.AreNoSelectedObjectsRoles() Then
                                If Me.zrPage.are_all_SelectedObjects_entity_types And Me.zrPage.SelectedObject.Count = 2 Then
                                    Me.LabelHelp.Text &= "Hint: Press the [S] key to make Entity Type, '" & Me.zrPage.SelectedObject(0).Name & "', a Subtype of Entity Type, '" & Me.zrPage.SelectedObject(1).Name & "'."
                                    Me.LabelHelp.Text &= vbCrLf
                                    Me.LabelHelp.Text &= "Hold down the [Control] key and Left Click on another Model Object to select more than one Model Object."
                                Else
                                    Me.LabelHelp.Text &= "Hold down the [Control] key and Left Click on another Model Object to select more than one Model Object."
                                End If
                            ElseIf Me.zrPage.role_and_object_type_selected Then
                                Me.LabelHelp.Text &= "Hint: Press the [R] key to add a Role to the Fact Type, with the new Role joined to the selected Model Object."
                                Me.LabelHelp.Text &= vbCrLf
                                Me.LabelHelp.Text &= "Hold down the [Control] key and Left Click on another Model Object to select more than one Model Object."
                            Else
                                Me.LabelHelp.Text &= "Hold down the [Control] key and Left Click on another Model Object to select more than one Model Object."
                            End If

                        Else
                            Me.LabelHelp.Text &= "Hint: Press the [R] key to create a Unary Fact Type against the Entity Type."
                            Me.LabelHelp.Text &= vbCrLf
                            Me.LabelHelp.Text &= "Hint: Press the [P] key to see the Properties of the Entity Type."
                            Me.LabelHelp.Text &= vbCrLf
                            Me.LabelHelp.Text &= "Hold down the [Control] key and Left Click on another Model Object to select more than one Model Object."
                        End If
                    Case Is = pcenumConceptType.ValueType
                        Me.LabelHelp.Text &= "Hint: Press the [P] key to see the Properties of the Value Type."
                        Me.LabelHelp.Text &= vbCrLf
                        Me.LabelHelp.Text &= "Hold down the [Control] key and Left Click on another Model Object to select more than one Model Object."
                    Case Is = pcenumConceptType.FactType
                    Case Is = pcenumConceptType.EntityTypeName
                    Case Is = pcenumConceptType.Role
                        If Me.zrPage.role_and_object_type_selected Then
                            Me.LabelHelp.Text &= "Hint: Press the [R] key to add a Role to the Fact Type with the new Role joined to the selected Model Object."
                        Else
                            If Me.zrPage.SelectedObject.Count = 1 Then
                                Me.LabelHelp.Text &= "Press the [Delete] button to remove the Role from the selected Fact Type."
                                'ElseIf Me.zrPage.are_all_SelectedObjects_roles Then
                            Else
                                If Me.zrPage.are_all_selected_roles_within_the_same_FactType Then
                                    Me.LabelHelp.Text &= "Hint: Press the [U] key to create an (Internal) Uniqueness Constraint agaist the selected Role/s."
                                ElseIf Me.zrPage.AreAllSelectedRolesJoinedToTheSameModelObject Then
                                    Me.LabelHelp.Text &= "Hint: Press the [U] key to create an (External) Uniqueness Constraint agaist the selected Role/s."
                                End If
                            End If

                        End If
                    Case Is = pcenumConceptType.FactTypeName
                    Case Is = pcenumConceptType.FactTypeReading
                    Case Is = pcenumConceptType.RoleConstraint
                    Case Is = pcenumConceptType.RoleConstraintRole
                    Case Else
                End Select

            ElseIf TypeOf (e.Node) Is TableNode Then
                Dim lo_table_node As TableNode = e.Node
                lo_table_node.CellFrameStyle = MindFusion.Diagramming.CellFrameStyle.Simple

                Dim lrFactTable As New FBM.FactTable
                lrFactTable = e.Node.Tag
                lrFactTable.FactTypeInstance.Shape.Visible = True
                lrFactTable.FactTypeInstance.Shape.Pen.Color = Color.BlueViolet

            End If


            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Public Sub DisplayORMModelPage(ByRef arPage As FBM.Page)

        '------------------------------------------------------------------------
        ' Displays an ORM Diagram based on the input parameters
        '
        'PARAMETERS
        '  as_ModelId:  The Enterprise currently being worked on.
        '  aiSubject_area_id:  The SubjectArea currently being worked on.
        '  aiProject_id:  The Project currently being worked on.
        '
        'PSEUDOCODE
        '        
        '  * Display the different Symbols of the ORMModelPage while logically associating 
        '       each Symbol with the corresponding 'object' within the ORMModel object
        '       i.e. * FOR EACH EntityTypeInstance within the ORMModel
        '                * Create a Shape object within the DiagramView object (View of MVC)
        '                * Associate the Shape object with the ORMModel.EntityTypeInstance (View level of MVC)
        '                * Associate the ORMModelPage.EntityTypeInstance with the ORMModel.EntityType (View level to Model level link, MVC)
        '              LOOP
        '------------------------------------------------------------------------

        Try
            'Me.SetVisibleCore(False)

            prApplication.ThrowErrorMessage("Entered frmDiagramORM.DisplayORMModelPage", pcenumErrorType.Information)

            Dim loDroppedNode As ShapeNode = Nothing
            Dim lrValueTypeInstance As FBM.ValueTypeInstance

            Me.Diagram.AutoResize = True
            '-------------------------------------------------------
            'Set the Caption/Title of the Page to the PageName
            '-------------------------------------------------------
            Me.zrPage = arPage
            Me.Tag = arPage
            Me.TabText = arPage.Name

            '===========================
            'Set the Zoom factor
            Me.DiagramView.ZoomFactor = My.Settings.DefaultPageZoomFactor

            '-----------------------------------------------------------------------------------------------------------
            'If the Virtual Analyst toolbox is loaded, set the Brain's Page (of the toolbox) to the Page of this form.
            '-----------------------------------------------------------------------------------------------------------
            If prApplication.ToolboxForms.FindAll(Function(x) x.Name = frmToolboxBrainBox.Name).Count > 0 Then
                prApplication.Brain.Page = Me.zrPage
                prApplication.Brain.Model = Me.zrPage.Model
            End If

            '-----------------------------------------------------
            'Override default in Settings for an individual Page
            '-----------------------------------------------------
            Me.ViewFactTablesToolStripMenuItem.Checked = Me.zrPage.ShowFacts

            '------------------------------------------------------------------------
            'Display the ORMDiagram and logically associating  each Shape object
            '   with the corresponding 'object' within the ORMModelPage object
            '------------------------------------------------------------------------

            prApplication.ThrowErrorMessage("frmDiagramORM.DisplayORMModelPage, about to load the Instances.", pcenumErrorType.Information)

            '----------------------------------------------
            'Display the EntityTypes within the ORMDiagram
            '----------------------------------------------
            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            For Each lrEntityTypeInstance In arPage.EntityTypeInstance
                lrEntityTypeInstance.ExpandReferenceMode = False
                Call lrEntityTypeInstance.DisplayAndAssociate()
            Next

            Me.HelpProvider.SetHelpKeyword(Me.DiagramView, "Entity")
            Me.HelpProvider.SetHelpNavigator(Me.DiagramView, HelpNavigator.KeywordIndex)

            '----------------------------------------------
            'Display the ValueTypes within the ORMDiagram
            '----------------------------------------------
            For Each lrValueTypeInstance In arPage.ValueTypeInstance
                '--------------------------------------------------------------------
                'Create a Shape for the EntityTypeInstance on the DiagramView object
                '--------------------------------------------------------------------            
                lrValueTypeInstance.DisplayAndAssociate()
            Next

            '-----------------------
            'Display the FactTypes
            '-----------------------
            Dim lrFactTypeInstance As FBM.FactTypeInstance
            For Each lrFactTypeInstance In arPage.FactTypeInstance
                '----------------------------------------------
                '  Create the new ShapeNode for the FactType.
                '----------------------------------------------            
                Call lrFactTypeInstance.DisplayAndAssociate(Me.ViewFactTablesToolStripMenuItem.Checked, My.Settings.ShowFactTypeNamesOnORMModelLoad)
            Next

            '--------------------------------
            'Display any Subtype constraints
            '--------------------------------            
            For Each lrEntityTypeInstance In arPage.EntityTypeInstance
                For Each lrSubtypeRelationship In lrEntityTypeInstance.SubtypeRelationship
                    Call lrSubtypeRelationship.DisplayAndAssociate()
                Next
            Next

            '-----------------------------
            'Display the RoleConstraints
            '-----------------------------
            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
            For Each lrRoleConstraintInstance In Me.zrPage.RoleConstraintInstance
                '-------------------------
                'Load the RoleConstraint
                '-------------------------
                Select Case lrRoleConstraintInstance.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                        'Dim lrUniquenessConstraintInstance As New tUniquenessConstraint
                        'lrUniquenessConstraintInstance = lrRoleConstraintInstance.CloneUniquenessConstraintInstance(lrRoleConstraintInstance.Page)
                        'lrUniquenessConstraintInstance.DisplayAndAssociate()
                        Call lrRoleConstraintInstance.DisplayAndAssociate()
                    Case Is = pcenumRoleConstraintType.FrequencyConstraint
                        'Dim lrFrequencyConstraintInstance As New FBM.tFrequencyConstraint
                        'lrFrequencyConstraintInstance = lrRoleConstraintInstance.CloneFrequencyConstraintInstance(lrRoleConstraintInstance.Page)
                        'lrFrequencyConstraintInstance.DisplayAndAssociate()
                        Call lrRoleConstraintInstance.DisplayAndAssociate()
                    Case Else
                        Call lrRoleConstraintInstance.DisplayAndAssociate()
                End Select

            Next

            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance.FindAll(Function(x) x.Visible = True)
                Call lrFactTypeInstance.AdjustBorderHeight()
            Next

            '------------------------
            'Display the ModelNotes
            '------------------------
            Dim lrModelNoteInstance As FBM.ModelNoteInstance
            For Each lrModelNoteInstance In Me.zrPage.ModelNote
                '-------------------------
                'Load the RoleConstraint
                '-------------------------
                Call lrModelNoteInstance.DisplayAndAssociate()
            Next

            '-------------------------------------------------------------------
            'Load any State Transitions that relate to EntityTypes on the Page
            '-------------------------------------------------------------------
            Call Me.loadStateTransitions

            '------------------------------------------------------------------
            'Set the PropertiesGrid.SeletedObject to the ORMModel.Page itself
            '------------------------------------------------------------------
            Dim lrPropertyGridForm As frmToolboxProperties
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)

            If IsSomething(lrPropertyGridForm) Then
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                lrPropertyGridForm.PropertyGrid.SelectedObject = arPage
            End If

            '--------------------------------------------------------------------------
            'Because ShapeItems are 'selected' during this process, calling the below
            '  resets all the default colours for when no items are selected
            '--------------------------------------------------------------------------
            Call Me.ResetNodeAndLinkColors()


            '----------------------------------
            'Clear the selected items (if any)
            '----------------------------------
            Diagram.Selection.Clear()
            arPage.SelectedObject.Clear()
            Me.PerformCleanup()

            prApplication.ThrowErrorMessage("frmDiagramORM.DisplayORMModelPage setting the toolbox.", pcenumErrorType.Information)
            Call Me.SetToolbox()

            'Me.Diagram.ResizeToFitItems(1 )
            Me.Diagram.Invalidate()
            Me.zrPage.FormLoaded = True

            prApplication.ThrowErrorMessage("frmDiagramORM.DisplayORMModelPage, finished loading the Page", pcenumErrorType.Information)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' Loads any State Transitions that relate to Entity Types on the Page. Called when the Page is loaded/displayed for the first time.
    ''' </summary>
    Private Sub loadStateTransitions()

        Dim lsSQLQuery As String = ""
        Dim lrRecordset As ORMQL.Recordset

        'PSEUDOCODE
        'When an ORM Page opens the loading procedure should do the following after the ORM model elements have been loaded onto the Page.
        '1. Check to see if any EntityType Is a Subtype of a Supertype that Is part of a StateControlling FactType, Or Is such a Supertype itself.
        '2. If the EntityType Is a Subtype/Supertype with an associated StateControlling FactType, then 
        '3. For that EntityType check to see if it Is the FromState of any State Transition within the CMML, And if it Is...whether the ToState (as an EntityType) Is on the same Page as the first Entity Type
        '4. If the FromState And ToState EntityTypes are both on the same page, then link between them.
        'End Of Story

        Try
            Dim larSupertypeEntityType = From FactType In Me.zrPage.Model.FactType
                                         From Role In FactType.RoleGroup
                                         Where FactType.IsSubtypeStateControlling = True _
                                         And Role.JoinedORMObject.ConceptType = pcenumConceptType.EntityType
                                         Select Role.JoinedORMObject

            For Each lrEntityType In larSupertypeEntityType
                For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance.FindAll(Function(x) x.EntityType.GetTopmostSupertype.Id = lrEntityType.Id)

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreStateTransition.ToString
                    lsSQLQuery &= " WHERE Concept1 = '" & lrEntityTypeInstance.Id & "'"

                    lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lsFromStateConceptId, lsToStateConceptId As String
                    Dim lsEventName As String

                    While Not lrRecordset.EOF

                        lsFromStateConceptId = lrRecordset("Concept1").Data
                        lsToStateConceptId = lrRecordset("Concept2").Data
                        lsEventName = lrRecordset("Event").Data

                        Dim lrFromEntityTypeInstance, lrToEntityTypeInstance As FBM.EntityTypeInstance

                        lrFromEntityTypeInstance = Me.zrPage.EntityTypeInstance.Find(Function(x) x.Id = lsFromStateConceptId)
                        lrToEntityTypeInstance = Me.zrPage.EntityTypeInstance.Find(Function(x) x.Id = lsToStateConceptId)

                        If lrFromEntityTypeInstance IsNot Nothing And lrToEntityTypeInstance IsNot Nothing Then
                            '----------------------------------------------------------------------------
                            'Draw the link between the EntityTypeInstances
                            Dim loNode1 As MindFusion.Diagramming.ShapeNode = lrFromEntityTypeInstance.Shape
                            Dim loNode2 As MindFusion.Diagramming.ShapeNode = lrToEntityTypeInstance.Shape
                            Dim loLink As New DiagramLink(Me.zrPage.Diagram, loNode1, loNode2)
                            loLink.Locked = False
                            loLink.Tag = Me

                            loLink.Visible = True
                            loLink.ShadowColor = Color.White
                            loLink.Pen.Color = Color.Green
                            loLink.Pen.Width = 0.7
                            loLink.Pen.DashStyle = DashStyle.Dash
                            loLink.HeadShape = ArrowHead.Arrow
                            loLink.Text = lsEventName
                            Me.zrPage.Diagram.Links.Add(loLink)

                            Dim lrStateTransition As New CMML.StateTransition
                            loLink.Tag = lrStateTransition
                        End If

                        lrRecordset.MoveNext()
                    End While
                Next
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub clear_diagram()

        Me.Diagram.Nodes.Clear()

    End Sub

    Private Sub mnuOption_DockRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_DockRight.Click


        frmMain.zfrm_toolbox.ShapeListBox.Dock = DockStyle.Right

    End Sub

    Private Sub mnuOption_DockLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_DockLeft.Click

        frmMain.zfrm_toolbox.ShapeListBox.Dock = DockStyle.Left

    End Sub


    Private Sub mnuOption_DockTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_DockTop.Click

        frmMain.zfrm_toolbox.ShapeListBox.Dock = DockStyle.Top

    End Sub

    Private Sub PropertiesToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem1.Click

        Try

            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

            Dim lrPropertyGridForm As frmToolboxProperties

            If IsSomething(prApplication.GetToolboxForm(frmToolboxProperties.Name)) Then
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})

                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub


    Private Sub mnuOption_Mandatory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_Mandatory.Click

        Try
            Dim lrRoleInstance As FBM.RoleInstance

            mnuOption_Mandatory.Checked = Not mnuOption_Mandatory.Checked

            lrRoleInstance = Me.zrPage.SelectedObject(0)
            lrRoleInstance.SetMandatory(mnuOption_Mandatory.Checked)

            '--------------------------------------------
            'Refresh the PropertiesGrid if it is showing
            '--------------------------------------------
            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)

            If IsSomething(lrPropertyGridForm) Then
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleInstance
            End If

            Call Me.EnableSaveButton()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub PropertieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertieToolStripMenuItem.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
        If IsSomething(lrPropertyGridForm) Then
            If Me.Diagram.Selection.Items.Count > 0 Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(Me.Diagram.Selection.Items.Count - 1).Tag
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0) ' Diagram.Selection.Items(0).Tag
            End If
        End If

    End Sub


    Sub CreateExternalExclusionConstraint(ByRef aarRole As List(Of FBM.Role), ByVal ao_pointf As PointF)

        Dim lsRoleConstraintName As String = ""
        Dim lrRoleConstraint As FBM.RoleConstraint
        'Dim loDroppedNode As ShapeNode
        'Dim lrRoleInstance As FBM.RoleInstance


        Try
            lrRoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.ExclusionConstraint,
                                                                    aarRole,
                                                                    lsRoleConstraintName,
                                                                    0)

            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

            lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(Me.zrPage, True)

            lrRoleConstraintInstance.X = ao_pointf.X
            lrRoleConstraintInstance.Y = ao_pointf.Y
            Call lrRoleConstraintInstance.DisplayAndAssociate()

            ''---------------------------
            ''Create the constraint
            ''---------------------------
            'loDroppedNode = Me.Diagram.Factory.CreateShapeNode(ao_pointf.X, ao_pointf.Y, 6, 6, Shapes.Ellipse)
            'loDroppedNode.Pen.Color = Color.White
            'loDroppedNode.Image = My.Resources.ORMShapes.exclusion
            'loDroppedNode.ImageAlign = ImageAlign.Fit
            'loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
            'loDroppedNode.Visible = True

            'Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.ExternalExclusionConstraint)

            'loDroppedNode.Tag = New Object
            'loDroppedNode.Tag = lrRoleConstraintInstance

            'lrRoleConstraintInstance.Shape = loDroppedNode

            'For Each lrRoleInstance In aarRoleInstance
            '    'lrRoleInstance.constraint.Add(lrRoleConstraint)
            '    'lrRoleInstance.constraint(lrRoleInstance.constraint.Count - 1).shape = loDroppedNode
            '    '-------------------------------------------------------------
            '    'Create the link between the Role and the ORMObject that it 
            '    '  joins to.
            '    '-------------------------------------------------------------
            '    Dim lo_link As New DiagramLink(Diagram, loDroppedNode, lrRoleInstance.Shape)
            '    lo_link.BaseShape = ArrowHead.None
            '    lo_link.HeadShape = ArrowHead.None
            '    Diagram.Links.Add(lo_link)
            'Next

            Me.zrPage.SelectedObject.Clear()
            Diagram.Selection.Clear()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Sub CreateExternalUniquenessConstraint(ByRef aarRoleInstance As List(Of FBM.RoleInstance), ByVal aoPointf As PointF)

        Dim lrRoleConstraint As FBM.RoleConstraint
        Dim lrRoleInstance As FBM.RoleInstance
        Dim larRole As New List(Of FBM.Role)


        Try

            For Each lrRoleInstance In aarRoleInstance
                larRole.Add(lrRoleInstance.Role)
            Next

            '---------------------------
            'Create the RoleConstraint
            '---------------------------
            lrRoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                                                    larRole,
                                                                    "ExternalUniquenessConstraint",
                                                                    0)

            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

            lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(Me.zrPage, True)
            lrRoleConstraintInstance.X = aoPointf.X
            lrRoleConstraintInstance.Y = aoPointf.Y

            Call lrRoleConstraintInstance.DisplayAndAssociate()

            Me.zrPage.SelectedObject.Clear()
            Diagram.Selection.Clear()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Sub CreateEqualityConstraint(ByRef aarRoleInstance As List(Of FBM.RoleInstance), ByVal ao_pointf As PointF)

        Dim larRole As New List(Of FBM.Role)
        Dim lrRoleConstraint As FBM.RoleConstraint

        Try

            Dim lrRoleInstance As FBM.RoleInstance

            For Each lrRoleInstance In aarRoleInstance
                larRole.Add(lrRoleInstance.Role)
            Next

            '--------------------------------
            'Create the RoleConstraint
            '--------------------------------
            lrRoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.EqualityConstraint,
                                                                    larRole,
                                                                    "EqualityConstraint",
                                                                    0)

            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

            lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(Me.zrPage, True)
            lrRoleConstraintInstance.X = ao_pointf.X
            lrRoleConstraintInstance.Y = ao_pointf.Y

            Call lrRoleConstraintInstance.DisplayAndAssociate()

            Me.zrPage.SelectedObject.Clear()
            Diagram.Selection.Clear()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub CreateFrequencyConstraint(ByRef arRoleInstance As FBM.RoleInstance)

        Dim larRole As New List(Of FBM.Role)
        Dim lrRoleConstraint As FBM.RoleConstraint
        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        Try

            larRole.Add(arRoleInstance.Role)

            lrRoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.FrequencyConstraint,
                                                                    larRole,
                                                                    "FrequencyConstraint",
                                                                    0)

            lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(Me.zrPage, True)

            lrRoleConstraintInstance.X = arRoleInstance.X
            lrRoleConstraintInstance.Y = arRoleInstance.Y - 10

            Call lrRoleConstraintInstance.DisplayAndAssociate()

            Me.zrPage.SelectedObject.Clear()
            Diagram.Selection.Clear()


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Sub AddFrequencyConstraint(ByRef aarRoleInstance As List(Of FBM.RoleInstance), ByVal ao_pointf As PointF)


        Try
            '---------------------------
            'Create the RoleConstraint
            '---------------------------
            Dim lrRoleInstance As FBM.RoleInstance
            Dim larRoleConstraintRole As New List(Of FBM.Role)

            For Each lrRoleInstance In aarRoleInstance
                larRoleConstraintRole.Add(lrRoleInstance.Role)
            Next

            Dim lrRoleConstraint As FBM.RoleConstraint
            lrRoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.FrequencyConstraint, larRoleConstraintRole, , , , False)
            lrRoleConstraint.Cardinality = 2
            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOREqual

            Call Me.zrPage.DropFrequencyConstraintAtPoint(lrRoleConstraint, ao_pointf)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Sub CreateInclusiveORConstraint(ByRef aarRoleInstance As List(Of FBM.RoleInstance), ByVal aoPointf As PointF)

        Dim lrRoleConstraint As FBM.RoleConstraint
        Dim lrRoleInstance As FBM.RoleInstance
        Dim larRole As New List(Of FBM.Role)

        Try
            For Each lrRoleInstance In aarRoleInstance
                larRole.Add(lrRoleInstance.Role)
            Next

            '---------------------------
            'Create the RoleConstraint
            '---------------------------
            lrRoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.InclusiveORConstraint,
                                                                    larRole,
                                                                    "InclusiveOrConstraint",
                                                                    0)

            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

            lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(Me.zrPage, True)
            lrRoleConstraintInstance.X = aoPointf.X
            lrRoleConstraintInstance.Y = aoPointf.Y

            Call lrRoleConstraintInstance.DisplayAndAssociate()

            Me.zrPage.SelectedObject.Clear()
            Diagram.Selection.Clear()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Sub CreateExclusiveOrConstraint(ByRef aarRoleInstance As List(Of FBM.RoleInstance), ByVal aoPointf As PointF)

        Dim lrRoleConstraint As FBM.RoleConstraint
        Dim lrRoleInstance As FBM.RoleInstance
        Dim larRole As New List(Of FBM.Role)

        Try
            For Each lrRoleInstance In aarRoleInstance
                larRole.Add(lrRoleInstance.Role)
            Next

            '---------------------------
            'Create the RoleConstraint
            '---------------------------
            lrRoleConstraint = Me.zrPage.Model.CreateRoleConstraint(pcenumRoleConstraintType.ExclusiveORConstraint,
                                                                    larRole,
                                                                    "ExclusiveOrConstraint",
                                                                    0)

            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

            lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(Me.zrPage, True)
            lrRoleConstraintInstance.X = aoPointf.X
            lrRoleConstraintInstance.Y = aoPointf.Y

            Call lrRoleConstraintInstance.DisplayAndAssociate()

            Me.zrPage.SelectedObject.Clear()
            Diagram.Selection.Clear()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub PerformCleanup()

        '------------------------------------------------
        'Performs various cleanup processes
        '------------------------------------------------
        Try
            Select Case Me.zrPage.SelectedObject.Count
                Case Is = 0
                    '-------------------------------
                    'There are no selected objects
                    '-------------------------------
                    Dim lrORMReadingEditor As frmToolboxORMReadingEditor
                    lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

                    If IsSomething(lrORMReadingEditor) Then

                        '-------------------------------------------------------------------------
                        'Tidy up the ORMFactTypeReading editor if the ORMFactTypeReading is open
                        '-------------------------------------------------------------------------
                        lrORMReadingEditor.zrFactTypeInstance = New FBM.FactTypeInstance()
                        lrORMReadingEditor.zrFactTypeInstance = Nothing
                        lrORMReadingEditor.DataGrid_Readings.DataSource = Nothing
                        lrORMReadingEditor.DataGrid_Readings.Refresh()
                        lrORMReadingEditor.DataGrid_Readings.RefreshEdit()
                        lrORMReadingEditor.DataGrid_Readings.Rows.Clear()
                        lrORMReadingEditor.LabelFactTypeName.Text = "No Fact Type Selected"
                    End If
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub Diagram_NodeSelecting(sender As Object, e As NodeValidationEventArgs) Handles Diagram.NodeSelecting

    End Sub

    Private Sub Diagram_NodeStartModifying(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeValidationEventArgs) Handles Diagram.NodeStartModifying

        Dim lrModelObject As New FBM.ModelObject

        lrModelObject = e.Node.Tag

        'Select lrModelObject.ConceptType
        '    Case Is = pcenumConceptType.RoleConstraintRole
        '        Dim lrRoleConstraintRole As FBM.RoleConstraintRole = lrModelObject
        '        If lrRoleConstraintRole.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
        '            e.Cancel = True
        '        End If
        'End Select

    End Sub

    Private Sub Diagram_NodeTextEdited(ByVal sender As Object, ByVal e As MindFusion.Diagramming.EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        Dim lrModelObject As New FBM.ModelObject

        lrModelObject = e.Node.Tag

        Select Case lrModelObject.ConceptType
            Case Is = pcenumConceptType.EntityTypeName
                Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
                Dim lrEntityTypeName As New FBM.EntityTypeName
                lrEntityTypeName = e.Node.Tag
                lrEntityTypeInstance = lrEntityTypeName.EntityTypeInstance

                '------------------------------------------------
                'Set the values in the underlying Model.EntityType
                '------------------------------------------------
                If e.NewText = lrEntityTypeInstance.EntityType.Name Then
                    '------------------------------------------------------------
                    'Nothing to do. Name of the EntityType has not been changed.
                    '------------------------------------------------------------
                Else
                    Dim lrEntityTypeDictionaryEntry As New FBM.DictionaryEntry(lrEntityTypeInstance.Model, e.NewText, pcenumConceptType.EntityType)
                    Dim lrValueTypeDictionaryEntry As New FBM.DictionaryEntry(lrEntityTypeInstance.Model, e.NewText, pcenumConceptType.ValueType)
                    Dim lrFactTypeDictionaryEntry As New FBM.DictionaryEntry(lrEntityTypeInstance.Model, e.NewText, pcenumConceptType.FactType)
                    Dim lrRoleConstraintDictionaryEntry As New FBM.DictionaryEntry(lrEntityTypeInstance.Model, e.NewText, pcenumConceptType.RoleConstraint)

                    If lrEntityTypeInstance.Model.ModelDictionary.Exists(AddressOf lrEntityTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                        MsgBox("An Entity Type with the name, '" & lrEntityTypeDictionaryEntry.Symbol & "', already exists in the Model, '" & lrEntityTypeInstance.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                        lrEntityTypeInstance.EntityTypeNameShape.Text = e.OldText
                    ElseIf lrEntityTypeInstance.Model.ModelDictionary.Exists(AddressOf lrValueTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                        MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Value Type of the same name in the Model, '" & lrEntityTypeInstance.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                        lrEntityTypeInstance.EntityTypeNameShape.Text = e.OldText
                    ElseIf lrEntityTypeInstance.Model.ModelDictionary.Exists(AddressOf lrFactTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                        MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Fact Type of the same name in the Model, '" & lrEntityTypeInstance.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                        lrEntityTypeInstance.EntityTypeNameShape.Text = e.OldText
                    ElseIf lrEntityTypeInstance.Model.ModelDictionary.Exists(AddressOf lrRoleConstraintDictionaryEntry.EqualsByConceptTypeOnly) Then
                        MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Role Constraint of the same name in the Model, '" & lrEntityTypeInstance.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                        lrEntityTypeInstance.EntityTypeNameShape.Text = e.OldText
                    Else
                        lrEntityTypeInstance.EntityType.SetName(e.NewText)
                    End If
                End If

            Case Is = pcenumConceptType.ValueType
                Dim lrValueTypeInstance As FBM.ValueTypeInstance

                lrValueTypeInstance = e.Node.Tag

                If e.NewText = lrValueTypeInstance.ValueType.Name Then
                    '------------------------------------------------------------
                    'Nothing to do. Name of the ValueType has not been changed.
                    '------------------------------------------------------------
                Else
                    Dim lrEntityTypeDictionaryEntry As New FBM.DictionaryEntry(lrValueTypeInstance.Model, e.NewText, pcenumConceptType.EntityType)
                    Dim lrValueTypeDictionaryEntry As New FBM.DictionaryEntry(lrValueTypeInstance.Model, e.NewText, pcenumConceptType.ValueType)
                    Dim lrFactTypeDictionaryEntry As New FBM.DictionaryEntry(lrValueTypeInstance.Model, e.NewText, pcenumConceptType.FactType)
                    Dim lrRoleConstraintDictionaryEntry As New FBM.DictionaryEntry(lrValueTypeInstance.Model, e.NewText, pcenumConceptType.RoleConstraint)

                    If lrValueTypeInstance.Model.ModelDictionary.Exists(AddressOf lrValueTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                        MsgBox("A Value Type with the name, '" & lrEntityTypeDictionaryEntry.Symbol & "', already exists in the Model, '" & lrValueTypeInstance.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                        lrValueTypeInstance.Shape.Text = e.OldText
                    ElseIf lrValueTypeInstance.Model.ModelDictionary.Exists(AddressOf lrEntityTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                        MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Entity Type of the same name in the Model, '" & lrValueTypeInstance.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                        lrValueTypeInstance.Shape.Text = e.OldText
                    ElseIf lrValueTypeInstance.Model.ModelDictionary.Exists(AddressOf lrFactTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                        MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Fact Type of the same name in the Model, '" & lrValueTypeInstance.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                        lrValueTypeInstance.Shape.Text = e.OldText
                    ElseIf lrValueTypeInstance.Model.ModelDictionary.Exists(AddressOf lrRoleConstraintDictionaryEntry.EqualsByConceptTypeOnly) Then
                        MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Role Constraint of the same name in the Model, '" & lrValueTypeInstance.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                        lrValueTypeInstance.Shape.Text = e.OldText
                    Else
                        lrValueTypeInstance.ValueType.SetName(e.NewText)
                        lrValueTypeInstance.Model.MakeDirty()
                        Call Me.EnableSaveButton()
                    End If
                End If

            Case Is = pcenumConceptType.RoleConstraint
                Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
                lrRoleConstraintInstance = lrModelObject.CloneRoleConstraintInstance(Me.zrPage)
                lrRoleConstraintInstance = Me.zrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                lrRoleConstraintInstance.Name = e.NewText
                lrRoleConstraintInstance.RoleConstraint.SetName(e.NewText)

            Case Is = pcenumConceptType.ModelNote

                Dim lrModelNoteInstance As FBM.ModelNoteInstance

                lrModelNoteInstance = e.Node.Tag
                lrModelNoteInstance.Shape.ResizeToFitText(FitSize.KeepRatio)
                lrModelNoteInstance.Text = e.NewText
                Call lrModelNoteInstance.ModelNote.SetText(Trim(e.NewText))
                lrModelNoteInstance.Model.MakeDirty(True)

            Case Else

        End Select

        Me.Diagram.Invalidate()

    End Sub

    Private Sub Diagram_NodeTextEditing(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeValidationEventArgs) Handles Diagram.NodeTextEditing

        Dim lo_point As PointF = e.MousePosition
        Dim loNode As Object 'Because may be ShapeNode or TableNode
        Dim lrModelObject As New FBM.ModelObject

        loNode = e.Node
        lrModelObject = loNode.tag
        Select Case lrModelObject.ConceptType
            Case Is = pcenumConceptType.Role,
                      pcenumConceptType.RoleConstraint,
                      pcenumConceptType.EntityType,
                      pcenumConceptType.FactType,
                      pcenumConceptType.FactTypeReading
                e.Cancel = True
        End Select

        '=====================================================================================================================================
        'Set the size of the editing box (used to change the Zoom factor here to 100%, but much better to modify the height of the edit box.
        'See DiagramView_EnterInplaceEditMode.

    End Sub


    Private Sub Diagram_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Diagram.SelectionChanged

        Dim loObject As New Object

        '---------------------------------------------------------------------------
        'If the ORM(FactType)ReadingEditor is loaded, then
        '  do the appropriate processing so that the data in the ReadingEditor grid
        '  matches the selected FactType (if a FactType is selected by the user)
        '---------------------------------------------------------------------------
        Dim lrORMReadingEditor As frmToolboxORMReadingEditor
        lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

        If IsSomething(lrORMReadingEditor) Then
            Select Case Me.zrPage.SelectedObject.Count
                Case Is = 0
                    lrORMReadingEditor.zrFactTypeInstance = New FBM.FactTypeInstance
                    lrORMReadingEditor.zrFactTypeInstance = Nothing
                    lrORMReadingEditor.DataGrid_Readings.DataSource = Nothing
                    lrORMReadingEditor.DataGrid_Readings.Refresh()
                    lrORMReadingEditor.DataGrid_Readings.RefreshEdit()
                    lrORMReadingEditor.DataGrid_Readings.Rows.Clear()
                Case Else
                    lrORMReadingEditor.zrPage = Me.zrPage
                    Select Case Me.zrPage.SelectedObject(0).ConceptType
                        Case Is = pcenumConceptType.FactType
                            If lrORMReadingEditor.zrFactTypeInstance IsNot Me.zrPage.SelectedObject(0) Then
                                lrORMReadingEditor.zrFactTypeInstance = Me.zrPage.SelectedObject(0)
                                Call lrORMReadingEditor.SetupForm()
                            End If
                        Case Is = pcenumConceptType.Role
                            If lrORMReadingEditor.zrFactTypeInstance IsNot Me.zrPage.SelectedObject(0).FactType Then
                                lrORMReadingEditor.zrFactTypeInstance = Me.zrPage.SelectedObject(0).FactType
                                Call lrORMReadingEditor.SetupForm()
                            End If
                    End Select
            End Select
        End If

        Select Case Me.zrPage.SelectedObject.Count
            Case Is = 0
                '------------------------
                'Reset ShapeNode.Colors
                '------------------------
                'Call Me.ResetNodeAndLinkColors()
                Me.zrPage.MultiSelectionPerformed = False
            Case Is = 1
                '-------------------------------------
                'A MultiSelection has NOT taken place
                '-------------------------------------
                Me.zrPage.MultiSelectionPerformed = False
            Case Is > 1
                '----------------------------------
                'A MultiSelection has taken place
                '----------------------------------
                If IsSomething(Me.zrPage) Then
                    '---------------------------------------------------------------
                    'WorkingPage exists. Sometimes, if the user 'closes' the form
                    '  the SelectionChanged gets triggered, but the WorkingPage
                    '  no longer exists.
                    '---------------------------------------------------------------
                    Me.zrPage.MultiSelectionPerformed = True
                End If
        End Select

        If Me.zrPage.SelectedObject.Count = 0 Then
            frmMain.ToolStripButtonCopy.Enabled = False
            'frmMain.ToolStripButtonCut.Enabled = False
        Else
            frmMain.ToolStripButtonCopy.Enabled = True
            'frmMain.ToolStripButtonCut.Enabled = True
        End If

    End Sub

    Private Sub Diagram_SelectionMoved(ByVal sender As Object, ByVal e As System.EventArgs) Handles Diagram.SelectionMoved

        Dim loNode As New ShapeNode
        Dim loORMObject As Object
        Dim loLink As DiagramLink
        Dim lrFactTypeInstance As FBM.FactTypeInstance

        Dim lsCommonTransaction As String = System.Guid.NewGuid.ToString

        For Each loORMObject In Me.zrPage.SelectedObject

            Dim lrUserAction As New tUserAction(loORMObject, pcenumUserAction.MoveModelObject, Me.zrPage, lsCommonTransaction)
            lrUserAction.PreActionModelObject = New Boston.tUndoRedoObject(loORMObject.X, loORMObject.Y) 'loORMObject.Clone(Me.zrPage)

            '---------------------------------------------
            'Set the X,Y Co-Ordinates of the ORM Object
            '---------------------------------------------
            Select Case loORMObject.ConceptType
                Case Is = pcenumConceptType.FactTable
                    loORMObject.x = loORMObject.TableShape.bounds.x
                    loORMObject.y = loORMObject.TableShape.bounds.y
                Case Else
                    loORMObject.x = loORMObject.shape.bounds.x
                    loORMObject.y = loORMObject.shape.bounds.y

                    If loORMObject.ConceptType = pcenumConceptType.Role Then
                        lrFactTypeInstance = loORMObject.FactType
                        lrFactTypeInstance.X = lrFactTypeInstance.Shape.Bounds.X
                        lrFactTypeInstance.Y = lrFactTypeInstance.Shape.Bounds.Y
                    End If

                    '==============================================================================
                    'Broadcast the moving of the Object
                    Dim lrModel As New Viev.FBM.Interface.Model
                    Dim lrPage As New Viev.FBM.Interface.Page()

                    lrModel.ModelId = Me.zrPage.Model.ModelId
                    lrPage.Id = Me.zrPage.PageId
                    lrPage.ConceptInstance = New Viev.FBM.Interface.ConceptInstance
                    lrPage.ConceptInstance.X = loORMObject.Shape.Bounds.X
                    lrPage.ConceptInstance.Y = loORMObject.Shape.Bounds.Y
                    lrPage.ConceptInstance.ModelElementId = loORMObject.Id
                    lrModel.Page = lrPage

                    If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                        Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                        lrBroadcast.Model = lrModel
                        Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.PageMovePageObject, lrBroadcast)
                    End If
                    '==============================================================================
            End Select

            lrUserAction.PostActionModelObject = New Boston.tUndoRedoObject(loORMObject.X, loORMObject.Y) 'loORMObject.Clone(Me.zrPage)
            prApplication.AddUndoAction(lrUserAction)
            frmMain.ToolStripMenuItemUndo.Enabled = True

            Select Case loORMObject.ConceptType
                Case pcenumConceptType.EntityType, pcenumConceptType.ValueType

                    Call loORMObject.Moved()

                    Call Me.SortJoiningFactTypes(loORMObject)

                    '------------------------------------------------------------
                    'Resort the RoleGroup of any FactType associated with the 
                    '  ObjectTypes for asthetic reasons.
                    '  i.e. So the Links from the Roles in the FactType are 
                    '  visually appealling on the Page.
                    '------------------------------------------------------------
                    For Each loLink In loORMObject.shape.IncomingLinks
                        If loLink.Origin.Tag.ConceptType = pcenumConceptType.Role Then
                            loLink.Origin.Tag.FactType.SortRoleGroup()
                        End If
                    Next

            End Select
        Next

        '--------------------------------
        'clear the Selection (as a whole)
        '--------------------------------
        Diagram.Selection.Clear()
        Me.zrPage.SelectedObject.Clear()
        Me.zrPage.MultiSelectionPerformed = False

        Call Me.ResetNodeAndLinkColors()

        Call Me.zrPage.MakeDirty()

        Call Me.EnableSaveButton()

    End Sub

    ''' <summary>
    ''' Used when Object Types, such as Entity Types, Value Types or Objectified Fact Types, are moved within an ORM Diagram.
    ''' Resorts the Roles of the Fact Type/s that are (Role) joined to the Object Type.
    ''' </summary>
    ''' <param name="arModelObject"></param>
    ''' <remarks></remarks>
    Private Sub SortJoiningFactTypes(ByRef arModelObject As Object)

        Dim lo_link As DiagramLink

        Try
            Select Case arModelObject.ConceptType
                Case Is = pcenumConceptType.EntityType,
                          pcenumConceptType.ValueType
                    '------------------------------------------------
                    'All okay
                    '------------------------------------------------
                Case Else
                    Throw New NotSupportedException("Can only sort FactTypes for EntityTypes and ValueTypes")
            End Select


            For Each lo_link In arModelObject.shape.IncomingLinks
                If lo_link.Origin.Tag.ConceptType = pcenumConceptType.Role Then
                    '-------------------------------------------------
                    'Sort the RoleGroup of the FactType instance
                    ' of all FactTypes associated with the ObjectType
                    '-------------------------------------------------
                    Dim lrFactTypeInstance As FBM.FactTypeInstance = lo_link.Origin.Tag.factType
                    lrFactTypeInstance.SortRoleGroup()

                    '------------------------------------------------
                    'Find the best FactTypeReading for the FactType
                    '------------------------------------------------
                    Dim lrFactTypeReading As New FBM.FactTypeReading
                    Dim lrFactTypeReadingInstance As New FBM.FactTypeReadingInstance

                    Dim larRole As New List(Of FBM.Role)
                    Dim lrRole As FBM.RoleInstance

                    For Each lrRole In lrFactTypeInstance.RoleGroup
                        larRole.Add(lrRole.Role)
                    Next

                    Call lrFactTypeInstance.FindSuitableFactTypeReading()

                    'lrFactTypeReading = lrFactTypeInstance.FactType.FindSuitableFactTypeReadingByRoles(larRole)

                    'If IsSomething(lrFactTypeReading) Then
                    '    lrFactTypeReadingInstance = lrFactTypeReading.CloneInstance(lo_link.Origin.Tag.factType.Page)
                    '    lrFactTypeReadingInstance.shape = lo_link.Origin.Tag.factType.FactTypeReadingShape.shape
                    '    lo_link.Origin.Tag.FactType.FactTypeReadingShape = lrFactTypeReadingInstance
                    '    lo_link.Origin.Tag.FactType.FactTypeReadingShape.RefreshShape()
                    'Else
                    '    If IsSomething(lrFactTypeInstance.FactTypeReadingShape) Then
                    '        If IsSomething(lrFactTypeInstance.FactTypeReadingShape.shape) Then
                    '            lrFactTypeInstance.FactTypeReadingShape.shape.Text = ""
                    '        End If
                    '    End If
                    'End If
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


    Private Sub mnuOption_IsObjectified_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_IsObjectified.Click

        Dim lrFactTypeInstance As FBM.FactTypeInstance

        Try
            lrFactTypeInstance = Me.zrPage.SelectedObject(0)

            mnuOption_IsObjectified.Checked = Not mnuOption_IsObjectified.Checked

            Select Case mnuOption_IsObjectified.Checked
                Case Is = True
                    '------------------------
                    'Objectify the FactType
                    '------------------------
                    Using loWaitCursor As New WaitCursor
                        '-------------------------------------------------------------------------------
                        'Set the 'IsObjectified' value for the FactTypeInstance and underlying FactType
                        '-------------------------------------------------------------------------------
                        lrFactTypeInstance.IsObjectified = mnuOption_IsObjectified.Checked
                        lrFactTypeInstance.FactType.Objectify()

                        If lrFactTypeInstance.InternalUniquenessConstraint.Count = 0 Then
                            '------------------------------------------------------------------
                            'Create an InternalUniquenessConstraint for the FactType/Instance
                            '------------------------------------------------------------------
                            Call lrFactTypeInstance.FactType.CreateInternalUniquenessConstraint(lrFactTypeInstance.FactType.RoleGroup)
                        End If
                    End Using

                Case Is = False
                    '--------------------------------------------
                    'Remove the objectification of the FactType
                    '--------------------------------------------        
                    If lrFactTypeInstance.FactType.hasAssociatedFactTypes Then
                        Dim lsMessage As String = "Remove all links to Fact Types linked to this Fact Type before removing the objectification of this Fact Type."
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= "Hint: See the list of associated Fact Types in the ORM Verbalisation toolbox to choose which Fact Types to remove or modify in the model before removing the objectification of this Fact Type."
                        MsgBox(lsMessage)

                        'Sample Code to find the linked FactTypes. Can extend message above to show the FactType Names to remove.
                        'Dim larFactType = From FactType In Me.Model.FactType
                        '                  From Role In FactType.RoleGroup
                        '                  Where Role.JoinedORMObject.Id = Me.Id
                        '                  Where FactType.isLinkFactType = False
                        '                  Select FactType

                    Else
                        lrFactTypeInstance.IsObjectified = mnuOption_IsObjectified.Checked
                        lrFactTypeInstance.FactType.RemoveObjectification(True)
                    End If

            End Select

            Call Me.ResetNodeAndLinkColors()

            '--------------------------------------------
            'Refresh the PropertiesGrid if it is showing
            '--------------------------------------------
            Dim lrPropertyGridForm As frmToolboxProperties
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)

            If IsSomething(lrPropertyGridForm) Then
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                If Me.Diagram.Selection.Items.Count > 0 Then
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(Me.Diagram.Selection.Items.Count - 1).Tag
                Else
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
                End If
            End If

            Call Me.EnableSaveButton()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub mnuOption_ViewReadingEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_ViewReadingEditor.Click

        If Me.zrPage.SelectedObject.Count > 0 Then
            If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.Role Then
                prApplication.WorkingPage = Me.zrPage
                Call frmMain.loadToolboxORMReadingEditor(Me.zrPage, Me.DockPanel.ActivePane) 'frm_orm_reading_editor.ShowDialog()
            End If
        End If

    End Sub

    Private Sub mnuOption_EntityTypeProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_EntityTypeProperties.Click

        Dim lrEntityTypeInstance As FBM.EntityTypeInstance

        Try

            If Me.Diagram.Selection.Items.Count > 0 Then
                lrEntityTypeInstance = Me.Diagram.Selection.Items(Me.Diagram.Selection.Items.Count - 1).Tag
            Else
                lrEntityTypeInstance = Me.zrPage.SelectedObject(0)
            End If

            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")

            If IsSomething(lrPropertyGridForm) Then

                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})

                If lrEntityTypeInstance.EntityType.HasSimpleReferenceScheme Then
                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataType", True)
                    Select Case lrEntityTypeInstance.DataType
                        Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                  pcenumORMDataType.NumericDecimal,
                                  pcenumORMDataType.NumericMoney
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                        Case Is = pcenumORMDataType.RawDataFixedLength,
                                  pcenumORMDataType.RawDataLargeLength,
                                  pcenumORMDataType.RawDataVariableLength,
                                  pcenumORMDataType.TextFixedLength,
                                  pcenumORMDataType.TextLargeLength,
                                  pcenumORMDataType.TextVariableLength
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                        Case Else
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                    End Select
                Else
                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataType", False)
                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                End If

                lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeInstance
                lrPropertyGridForm.PropertyGrid.Refresh()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub RemoveRoleConstraintArgument(ByVal sender As Object, ByVal e As EventArgs)


        Try
            Dim lsMessage As String = ""
            Dim lrRoleConstraintArgumentToRemove As FBM.RoleConstraintArgument

            lrRoleConstraintArgumentToRemove = sender.Tag

            lsMessage = "Are you sure that you want to remove Argument number, " & lrRoleConstraintArgumentToRemove.SequenceNr & "?"
            If lrRoleConstraintArgumentToRemove.SequenceNr < lrRoleConstraintArgumentToRemove.RoleConstraint.Argument.Count Then
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "All higher Arguments will be removed also."
            End If

            If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                lrRoleConstraintInstance = Me.zrPage.RoleConstraintInstance.Find(Function(x) x.Id = lrRoleConstraintArgumentToRemove.RoleConstraint.Id)
                Call lrRoleConstraintInstance.ClearJoinPathBrushesForArgument(lrRoleConstraintArgumentToRemove.SequenceNr)
                Call lrRoleConstraintArgumentToRemove.RoleConstraint.RemoveArgumentBySequenceNr(lrRoleConstraintArgumentToRemove.SequenceNr)
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub ResetNodeAndLinkColors()

        Dim liInd As Integer = 0

        Try
            '------------------------------------------------------------------------------------
            'Reset the border colors of the ShapeNodes (to what they were before being selected
            '------------------------------------------------------------------------------------
            For liInd = 1 To Diagram.Nodes.Count
                If Diagram.Nodes(liInd - 1).Selected Then
                    If IsSomething(Diagram.Nodes(liInd - 1).Tag) Then
                        Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
                                lrEntityTypeInstance = Diagram.Nodes(liInd - 1).Tag
                                lrEntityTypeInstance.Shape.Pen.Color = Color.Blue
                            Case Is = pcenumConceptType.FactType
                                If Diagram.Nodes(liInd - 1).Selected Then
                                    Dim lrFactTypeInstance As FBM.FactTypeInstance
                                    Dim lrRoleInstance As FBM.RoleInstance
                                    lrFactTypeInstance = Diagram.Nodes(liInd - 1).Tag
                                    For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
                                        If lrRoleInstance.Shape.Selected Then
                                            lrFactTypeInstance.Shape.Pen.Color = Color.LightGray
                                            Exit Select
                                        End If
                                    Next
                                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                                Else
                                    Diagram.Nodes(liInd - 1).Pen.Color = Color.LightGray
                                End If
                                Call Diagram.Nodes(liInd - 1).Tag.SetAppropriateColour()
                            Case Else
                                Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                                Call Diagram.Nodes(liInd - 1).Tag.SetAppropriateColour()
                        End Select
                    Else
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                    End If

                Else 'Non-Selected Nodes.
                    If IsSomething(Diagram.Nodes(liInd - 1).Tag) Then
                        Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                lrEntityTypeInstance = Diagram.Nodes(liInd - 1).Tag
                                If lrEntityTypeInstance.Shape.Selected Then
                                    lrEntityTypeInstance.Shape.Pen.Color = Color.Blue
                                Else
                                    lrEntityTypeInstance.Shape.Pen.Color = Color.Navy
                                End If
                                lrEntityTypeInstance.EntityTypeNameShape.Pen.Color = Color.White
                                Call Diagram.Nodes(liInd - 1).Tag.SetAppropriateColour()
                            Case Is = pcenumConceptType.ValueType
                                'Diagram.Nodes(liInd - 1).Pen.Color = Color.Navy
                                Call Diagram.Nodes(liInd - 1).Tag.SetAppropriateColour()
                            Case Is = pcenumConceptType.FactType
                                Dim lrFactTypeInstance As FBM.FactTypeInstance
                                lrFactTypeInstance = Diagram.Nodes(liInd - 1).Tag
                                If Diagram.Nodes(liInd - 1).Tag.IsObjectified Then
                                    Diagram.Nodes(liInd - 1).Visible = True
                                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                                Else
                                    Diagram.Nodes(liInd - 1).Pen.Color = Color.LightGray
                                    If Diagram.Nodes(liInd - 1).Selected Then
                                        Diagram.Nodes(liInd - 1).Visible = True
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                                    Else
                                        If lrFactTypeInstance.FactType.HasModelError Then
                                            Call lrFactTypeInstance.SetAppropriateColour()
                                        Else
                                            Diagram.Nodes(liInd - 1).Visible = False
                                        End If

                                    End If
                                End If
                                Call Diagram.Nodes(liInd - 1).Tag.SetAppropriateColour()
                            Case Is = pcenumConceptType.FactTypeName
                                Dim lrShapeNode As New ShapeNode
                                lrShapeNode = Diagram.Nodes(liInd - 1)
                                lrShapeNode.TextColor = Color.Blue
                            Case Is = pcenumConceptType.Role
                                Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                                Diagram.Nodes(liInd - 1).Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                            Case Is = pcenumConceptType.RoleName
                                Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                            Case Is = pcenumConceptType.RoleConstraint
                                Select Case Diagram.Nodes(liInd - 1).Tag.RoleConstraintType
                                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.White
                                    Case Is = pcenumRoleConstraintType.EqualityConstraint,
                                                pcenumRoleConstraintType.ExclusionConstraint,
                                                pcenumRoleConstraintType.ExclusiveORConstraint,
                                                pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                                pcenumRoleConstraintType.InclusiveORConstraint,
                                                pcenumRoleConstraintType.RingConstraint,
                                                pcenumRoleConstraintType.SubsetConstraint
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.White
                                        Me.Diagram.Invalidate()
                                    Case Else
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.White
                                End Select
                                Call Diagram.Nodes(liInd - 1).Tag.SetAppropriateColour()
                            Case Is = pcenumConceptType.RoleConstraintRole

                                Select Case Diagram.Nodes(liInd - 1).Tag.RoleConstraint.RoleConstraintType
                                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                                        lrRoleConstraintRoleInstance = Diagram.Nodes(liInd - 1).Tag
                                        lrRoleConstraintRoleInstance.Shape.Pen.Color = Color.White
                                    Case Else
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Purple
                                End Select
                            Case Is = pcenumConceptType.FactTable
                                Diagram.Nodes(liInd - 1).Pen.Color = Color.LightGray
                                Dim lo_table_node As TableNode = Diagram.Nodes(liInd - 1)
                                If lo_table_node.Selected Then
                                    lo_table_node.CellFrameStyle = MindFusion.Diagramming.CellFrameStyle.Simple
                                    lo_table_node.ZTop()
                                    lo_table_node.Scrollable = True
                                    lo_table_node.TextColor = Color.Black
                                    Dim lrFactTable As FBM.FactTable
                                    lrFactTable = lo_table_node.Tag
                                    lrFactTable.ResortFactTable()
                                Else
                                    lo_table_node.CellFrameStyle = MindFusion.Diagramming.CellFrameStyle.None
                                    lo_table_node.ZBottom()
                                    lo_table_node.Scrollable = False
                                    lo_table_node.TextColor = Color.LightGray
                                End If
                            Case Is = pcenumConceptType.ModelNote
                                Diagram.Nodes(liInd - 1).Pen.Color = Color.Gray
                            Case Else
                                Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                        End Select

                    End If
                End If
            Next

            For liInd = 1 To Diagram.Links.Count
                If IsSomething(Diagram.Links(liInd - 1).Tag) Then
                    Select Case Diagram.Links(liInd - 1).Tag.ConceptType
                        Case Is = pcenumConceptType.RoleConstraint
                            Select Case Diagram.Links(liInd - 1).Tag.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.SubsetConstraint
                                    Diagram.Links(liInd - 1).Pen.Color = Color.Purple
                            End Select
                        Case Is = pcenumConceptType.RoleConstraintRole
                            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                            lrRoleConstraintInstance = Diagram.Links(liInd - 1).Tag.RoleConstraint
                            Select Case lrRoleConstraintInstance.IsDeontic
                                Case Is = True
                                    Diagram.Links(liInd - 1).Pen.Color = Color.Blue
                                Case Else
                                    Diagram.Links(liInd - 1).Pen.Color = Color.Purple
                            End Select
                        Case Is = pcenumConceptType.ModelNote
                            Diagram.Links(liInd - 1).Pen.Color = Color.LightGray
                        Case Is = pcenumConceptType.SubtypeConstraint
                            Diagram.Links(liInd - 1).Pen.Color = Color.Purple
                        Case Is = pcenumConceptType.StateTransition
                            'Leave it as it is
                        Case Else
                            Diagram.Links(liInd - 1).Pen.Color = Color.Black
                    End Select
                End If
            Next

            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub ResetPropertiesGridToolbox(ByRef arObject As Object)

        Try

            If IsSomething(arObject) Then
                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)

                If IsSomething(lrPropertyGridForm) Then
                    Dim lrModelObject As FBM.ModelObject
                    lrModelObject = arObject
                    lrPropertyGridForm.PropertyGrid.BrowsableAttributes = Nothing
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                    Select Case lrModelObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                            lrEntityTypeInstance = lrModelObject
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeInstance
                        Case Is = pcenumConceptType.EntityTypeName
                            Dim lrEntityTypeName As FBM.EntityTypeName
                            lrEntityTypeName = lrModelObject
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeName.EntityTypeInstance
                        Case Is = pcenumConceptType.RoleConstraintRole
                            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                            lrRoleConstraintRoleInstance = lrModelObject
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintRoleInstance.RoleConstraint
                        Case Is = pcenumConceptType.RoleConstraint

                            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                            lrRoleConstraintInstance = lrModelObject
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
                        Case Else
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelObject
                    End Select
                End If
            Else
                Throw New Exception("No object passed as argument, 'arObject'")
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Public Sub SaveAnyRoleConstraintArgumentsBeingCreated()

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        If Me.zrPage.RoleConstraintInstance.FindAll(Function(x) x.CreatingArgument = True).Count > 0 Then

            lrRoleConstraintInstance = Me.GetRoleConstraintCurrentlyCreatingArgument

            '--------------------------------------------------------------------------
            'Finalise the creation of the Argument for the respective RoleConstraint.
            '--------------------------------------------------------------------------
            If lrRoleConstraintInstance.CurrentArgument.RoleConstraintRole.Count > 0 Then
                '---------------------------------------------------------------------------------
                'Only commit the Argument if there are RoleConstraintRoles against the Argument.
                '---------------------------------------------------------------------------------
                lrRoleConstraintInstance.RoleConstraint.AddArgument(lrRoleConstraintInstance.CurrentArgument, True)
                lrRoleConstraintInstance.CurrentArgument = Nothing
            End If
        End If

    End Sub

    Private Sub ORMDiagramView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseWheel

        Select Case e.Delta
            Case Is = 0
                'Do Nothing
            Case Is < 0
                If frmMain.ToolStripComboBox_zoom.SelectedIndex > 0 Then
                    frmMain.ToolStripComboBox_zoom.SelectedIndex -= 1
                End If
            Case Is > 0
                If frmMain.ToolStripComboBox_zoom.SelectedIndex < frmMain.ToolStripComboBox_zoom.Items.Count Then
                    If frmMain.ToolStripComboBox_zoom.SelectedIndex < frmMain.ToolStripComboBox_zoom.Items.Count - 1 Then
                        frmMain.ToolStripComboBox_zoom.SelectedIndex += 1
                    End If
                End If
        End Select

    End Sub


    Private Sub ORMDiagramView_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles DiagramView.PreviewKeyDown

        '------------------------------------------------------
        'NB e.Alt processing must be before other processing
        '------------------------------------------------------
        Dim lo_point As PointF

        Select Case e.KeyValue
            Case Is = (e.Control And Keys.C)
                Call frmMain.CopySelectedObjectsToClipboard()
            Case Is = (e.Control And Keys.V)
                Call frmMain.PasteToCurrentPageFromClipboard()
            Case Is = (e.Alt And Keys.X)
                If Me.zrPage.AreAllSelectedObjectsRoles Then
                    lo_point = Me.zrPage.GetMidOfSelectedObjects
                    Dim lrRoleInstance As FBM.RoleInstance
                    Dim larRoleInstance As New List(Of FBM.RoleInstance)
                    For Each lrRoleInstance In Me.zrPage.SelectedObject
                        larRoleInstance.Add(lrRoleInstance)
                    Next
                    Call Me.CreateExclusiveOrConstraint(larRoleInstance, lo_point)
                End If

            Case Is = Keys.X
                lo_point = Me.zrPage.GetMidOfSelectedObjects
                If Me.zrPage.AreAllSelectedObjectsRoles Then
                    Dim larRole As New List(Of FBM.Role)
                    Dim lrRoleInstance As Object

                    For Each lrRoleInstance In Me.zrPage.SelectedObject
                        larRole.Add(lrRoleInstance.Role)
                    Next
                    Call Me.CreateExternalExclusionConstraint(larRole, lo_point)
                End If

        End Select

    End Sub

    Public Sub AutoLayout()

        Dim liHighestOutgoingLinkCount As Integer = 0
        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
        Dim lrFactTypeInstance As FBM.FactTypeInstance
        Dim lrCentralEntityTypeInstance As New FBM.EntityTypeInstance
        'Dim lrLink As MindFusion.Diagramming.DiagramLink
        'Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        Try

            If Me.zrPage.EntityTypeInstance.Count = 0 Then
                Exit Sub
            End If

            'Every EntityType at least 10,10 from the corner of the screen
            'For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
            '    lrEntityTypeInstance.HasBeenMoved = False
            '    If IsSomething(lrEntityTypeInstance.shape) Then
            '        lrEntityTypeInstance.shape.Move(Viev.Greater(10, lrEntityTypeInstance.shape.Bounds.X), _
            '                                        Viev.Greater(10, lrEntityTypeInstance.shape.Bounds.Y))
            '    End If
            'Next

            'For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
            '    lrFactTypeInstance.HasBeenMoved = False
            '    If IsSomething(lrFactTypeInstance.Shape) Then
            '        lrFactTypeInstance.Shape.Move(Viev.Greater(10, lrFactTypeInstance.Shape.Bounds.X), Viev.Greater(10, lrFactTypeInstance.Shape.Bounds.Y))
            '    End If
            'Next

            '---------------------------------------------------------------
            'Put the EntityTypes with SubTypes towards the top of the page
            '---------------------------------------------------------------
            For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance.FindAll(Function(x) x.HasSubTypes = True)
                lrEntityTypeInstance.Shape.Move(100, 85)
                lrEntityTypeInstance.HasBeenMoved = True
                Call lrEntityTypeInstance.AutoLayout(False, False)

                Dim lrSubtypeEntityTypeInstance As FBM.EntityTypeInstance
                Dim liInd As Integer = 30
                Dim larSubTypes As New List(Of FBM.EntityTypeInstance)

                larSubTypes = lrEntityTypeInstance.GetSubTypes
                larSubTypes.Sort(AddressOf FBM.EntityTypeInstance.CompareTotalLinks)
                For Each lrSubtypeEntityTypeInstance In larSubTypes
                    lrSubtypeEntityTypeInstance.Shape.Move(liInd, 120)
                    Call lrSubtypeEntityTypeInstance.AutoLayout(True, False)
                    lrSubtypeEntityTypeInstance.HasBeenMoved = True
                    liInd += 50
                Next
            Next

            '-----------------------------
            'Find the central EntityType
            '-----------------------------
            For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance.FindAll(Function(x) x.Shape IsNot Nothing)

                Dim liTotalLinkCount = lrEntityTypeInstance.Shape.IncomingLinks.Count + lrEntityTypeInstance.Shape.OutgoingLinks.Count

                If liTotalLinkCount >= liHighestOutgoingLinkCount Then
                    liHighestOutgoingLinkCount = liTotalLinkCount
                    lrCentralEntityTypeInstance = lrEntityTypeInstance
                End If

            Next

            Dim liX As Integer = Viev.Lesser(100, DiagramView.ClientToDoc(New Point(Me.Width, Me.Height)).X / 3)
            Dim liY As Integer = Viev.Lesser(100, DiagramView.ClientToDoc(New Point(Me.Width, Me.Height)).Y / 3)

            If lrCentralEntityTypeInstance.HasBeenMoved Then
                '--------------------
                'Moved as a SubType
                '--------------------
            Else
                lrCentralEntityTypeInstance.shape.Move(liX, liY)
                lrCentralEntityTypeInstance.HasBeenMoved = True
            End If

            Dim liDegrees As Integer
            If lrCentralEntityTypeInstance.HasSubTypes Then
                liDegrees = 270
            Else
                liDegrees = 320
            End If




            'For Each lrLink In lrCentralEntityTypeInstance.Shape.IncomingLinks


            'If lrLink.Tag.ConceptType = pcenumConceptType.Role Then

            For Each lrFactTypeInstance In Me.zrPage.GetModelElementsJoinedFactTypes(lrCentralEntityTypeInstance)

                Dim lrRoleInstance As FBM.RoleInstance = lrFactTypeInstance.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lrCentralEntityTypeInstance.Id)
                'lrRoleInstance = lrLink.Tag
                'lrFactTypeInstance = lrRoleInstance.FactType

                liX = Viev.Greater(10, lrCentralEntityTypeInstance.Shape.Bounds.X + Math.Cos(liDegrees * (Math.PI / 180)) * 40)
                liY = Viev.Greater(10, lrCentralEntityTypeInstance.Shape.Bounds.Y + Math.Sin(liDegrees * (Math.PI / 180)) * 40)
                If lrFactTypeInstance.Shape IsNot Nothing Then
                    lrFactTypeInstance.Shape.Move(liX, liY)
                End If

                lrFactTypeInstance.HasBeenMoved = True

                If lrFactTypeInstance.IsBinaryFactType And Not lrFactTypeInstance.FactType.IsSubtypeRelationshipFactType Then
                    Dim lrObject As Object
                    lrObject = lrFactTypeInstance.GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject
                    liX = Viev.Greater(40, lrCentralEntityTypeInstance.Shape.Bounds.X + Math.Cos(liDegrees * (Math.PI / 180)) * 65)
                    liY = Viev.Greater(40, lrCentralEntityTypeInstance.Shape.Bounds.Y + Math.Sin(liDegrees * (Math.PI / 180)) * 65)
                    If Not lrObject.HasBeenMoved Then
                        lrObject.Shape.Move(liX, liY)
                    End If
                    lrObject.HasBeenMoved = True

                    If (lrObject.ConceptType = pcenumConceptType.EntityType) Then

                        Dim lrChildEntityTypeInstance As FBM.EntityTypeInstance
                        lrChildEntityTypeInstance = lrObject

                        Call lrChildEntityTypeInstance.AutoLayout(False, False)

                    End If

                End If


                liDegrees += 21

            Next

            'For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance.FindAll(Function(x) x.Shape IsNot Nothing)
            '    If lrEntityTypeInstance.HasBeenMoved Then
            '    Else
            '        lrEntityTypeInstance.Shape.Move(50, 50)
            '        Call lrEntityTypeInstance.AutoLayout(False, False)
            '    End If
            'Next

            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                lrFactTypeInstance.SortRoleGroup()
                lrFactTypeInstance.HasBeenMoved = False
            Next

            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                Call lrFactTypeInstance.BringStrandedJoinedObjectsCloser()
            Next

            'Dim lrValueTypeInstance As FBM.ValueTypeInstance
            'For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance.FindAll(Function(x) x.IsObjectified)
            '    For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
            '        Select Case lrRoleInstance.JoinedORMObject.ConceptType
            '            Case Is = pcenumConceptType.ValueType
            '                lrValueTypeInstance = lrRoleInstance.JoinedORMObject
            '                lrValueTypeInstance.X = lrRoleInstance.X
            '                lrValueTypeInstance.Y = lrRoleInstance.Y + (20 * (lrValueTypeInstance.Y.CompareTo(lrRoleInstance.Y)))
            '                lrValueTypeInstance.RepellNeighbouringPageObjects(15)
            '            Case Is = pcenumConceptType.EntityType
            '                lrEntityTypeInstance = lrRoleInstance.JoinedORMObject
            '                lrEntityTypeInstance.X = lrRoleInstance.X
            '                lrEntityTypeInstance.Y = lrRoleInstance.Y + (20 * (lrEntityTypeInstance.Y.CompareTo(lrRoleInstance.Y)))
            '                lrEntityTypeInstance.RepellNeighbouringPageObjects(15)
            '            Case Is = pcenumConceptType.FactType
            '                lrFactTypeInstance = lrRoleInstance.JoinedORMObject
            '                lrFactTypeInstance.X = lrRoleInstance.X
            '                lrFactTypeInstance.Y = lrRoleInstance.Y + (20 * (lrFactTypeInstance.Y.CompareTo(lrRoleInstance.Y)))
            '                Call lrFactTypeInstance.RepellNeighbouringPageObjects(15)
            '        End Select
            '    Next
            'Next

            For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
                lrEntityTypeInstance.RepellNeighbouringPageObjects(1)
            Next

            'For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
            '    Call lrEntityTypeInstance.SetAdjoinedFactTypesBetweenModelElements()
            'Next

            'For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance.FindAll(Function(x) x.IsObjectified)
            '    For Each lrRoleInstance In lrFactTypeInstance.RoleGroup

            '        Select Case lrRoleInstance.JoinedORMObject.ConceptType
            '            Case Is = pcenumConceptType.ValueType
            '                lrValueTypeInstance = lrRoleInstance.JoinedORMObject
            '                lrValueTypeInstance.X = lrRoleInstance.X
            '                lrValueTypeInstance.Y = lrRoleInstance.Y + (20 * (lrValueTypeInstance.Y.CompareTo(lrRoleInstance.Y)))
            '                lrValueTypeInstance.RepellNeighbouringPageObjects(25)
            '            Case Is = pcenumConceptType.EntityType
            '                lrEntityTypeInstance = lrRoleInstance.JoinedORMObject
            '                lrEntityTypeInstance.X = lrRoleInstance.X
            '                lrEntityTypeInstance.Y = lrRoleInstance.Y + (20 * (lrEntityTypeInstance.Y.CompareTo(lrRoleInstance.Y)))
            '                lrEntityTypeInstance.RepellNeighbouringPageObjects(25)
            '            Case Is = pcenumConceptType.FactType
            '                lrFactTypeInstance = lrRoleInstance.JoinedORMObject
            '                lrFactTypeInstance.X = lrRoleInstance.X
            '                lrFactTypeInstance.Y = lrRoleInstance.Y + (20 * (lrFactTypeInstance.Y.CompareTo(lrRoleInstance.Y)))
            '                Call lrFactTypeInstance.RepellNeighbouringPageObjects(50)
            '        End Select
            '    Next
            'Next

            'For Each lrRoleConstraintInstance In Me.zrPage.RoleConstraintInstance.FindAll(Function(x) x.RoleConstraintType <> pcenumRoleConstraintType.InternalUniquenessConstraint)

            '    Dim larRoleInstance As New List(Of FBM.RoleInstance)
            '    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
            '    Dim lrPointF As PointF

            '    For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
            '        larRoleInstance.Add(lrRoleConstraintRoleInstance.Role)
            '    Next
            '    lrPointF = Me.zrPage.GetMidOfRoleInstances(larRoleInstance)
            '    lrRoleConstraintInstance.Shape.Move(lrPointF.X, lrPointF.Y)
            'Next

            'For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
            '    lrEntityTypeInstance.HasBeenMoved = False
            'Next

            'For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
            '    Call lrFactTypeInstance.RepellNeighbouringPageObjects(1)
            'Next

            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                Call lrFactTypeInstance.SetSuitableFactTypeReading()
            Next

            For Each lrModelElement In Me.zrPage.GetAllPageObjects
                lrModelElement.HasBeenMoved = False
            Next

            Call Me.ResetNodeAndLinkColors()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub AutoLayoutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoLayoutToolStripMenuItem.Click

        Call Me.AutoLayout()

    End Sub

    Sub AddUniquenessConstraint()

        Dim ls_message As String
        Dim liFactTypeCardinality As Integer = 0
        Dim lrFactTypeInstance As FBM.FactTypeInstance
        Dim lrRoleInstance As FBM.RoleInstance

        If Me.zrPage.AreAllSelectedObjectsRoles Then
            If Me.zrPage.are_all_selected_roles_within_the_same_FactType Then

                '------------------------------------------------------
                'Get the FactType for the InternalUniquenessConstraint
                '------------------------------------------------------                    
                lrFactTypeInstance = Me.zrPage.SelectedObject(0).factType
                liFactTypeCardinality = lrFactTypeInstance.Arity

                If Me.zrPage.SelectedObject.Count = 1 Then
                    lrRoleInstance = Me.zrPage.SelectedObject(0)
                    If lrRoleInstance.FactType.Arity = 1 Then
                        MsgBox("Unary Fact Types don't require an Internal Uniqueness Constraint.")
                        Exit Sub
                    End If
                End If

                If Me.zrPage.do_selected_roles_span_minimum_for_FactType Then
                    If lrFactTypeInstance.FactType.InternalUniquenessConstraint.Count > 0 Then
                        '---------------------------------------------------
                        'The FactType already has an InternalRoleConstraint
                        '---------------------------------------------------
                        '------------------------------------------------------------------------
                        'If the SelectedRoles spans 'All' the Roles within the FactType, then
                        '  remove all existing InternalUniquenessConstraints from the FactType
                        '  before adding the TotalInternalUniquenessConstraint (because when a
                        '  TotalInternalUniquenessConstraint is on a FactType, it is the 'only'
                        '  InternalUniquenessConstraint allowed. 
                        '  NB Granted, a TotalInternalUniquenessConstraint may already exist on 
                        '  the FactType, but deleting it causes no harm if it is immediately replaced.
                        '------------------------------------------------------------------------                        
                        If Me.zrPage.SelectedObject.Count = lrFactTypeInstance.Arity Then
                            If lrFactTypeInstance.FactType.InternalUniquenessConstraint(0).RoleConstraintRole.Count = lrFactTypeInstance.Arity Then
                                MsgBox("The Fact Type, '" & lrFactTypeInstance.Name & "', already has a total internal uniqueness constraint.")
                                Exit Sub
                            Else
                                Call Me.zrPage.SelectedObject(0).factType.FactType.RemoveInternalUniquenessConstraints(True)  'Because we are about to replace the TotalInternalUniquenessConstraint with one that spans FT.Arity-1 Roles.
                            End If
                        ElseIf lrFactTypeInstance.FactType.InternalUniquenessConstraint(0).RoleConstraintRole.Count = lrFactTypeInstance.Arity Then
                            Call lrFactTypeInstance.FactType.RemoveInternalUniquenessConstraints(True) 'Because we are about to replace the TotalInternalUniquenessConstraint with one that spans FT.Arity-1 Roles.
                        End If

                    End If

                    '--------------------------------------------------
                    'Get the list of (Model level) Roles for which to 
                    '  assign the InternalUniquenessConstraint
                    '--------------------------------------------------
                    Dim larRole As New List(Of FBM.Role)

                    For Each lrRoleInstance In Me.zrPage.SelectedObject
                        larRole.Add(lrRoleInstance.Role)
                    Next

                    '------------------------------------------------------------------------------
                    'Create a dummy RoleConstraint for the InternalUniquenessConstraint
                    '  so that we can check if it already exists for the FactType.
                    '  NB This is only a dummy RoleConstraint and is not added to 
                    '  the model (Parameter: abAddToModel is False).
                    '  Once we confirm that the InternalUniquenssConstraint does not
                    '  already exist, we create the InternalUniquenessConstraint/RoleConstraint
                    '  via the FactTypeInstance (see futher below).
                    '------------------------------------------------------------------------------

                    Dim lrRoleConstraint As New FBM.RoleConstraint(lrFactTypeInstance.Model, pcenumRoleConstraintType.InternalUniquenessConstraint, larRole)

                    If lrFactTypeInstance.FactType.exists_InternalUniquenessConstraint_by_role_span(lrRoleConstraint) Then
                        '----------------------------------------------------------------
                        'InternalUniquenessConstraint already exists for Roles selected
                        '----------------------------------------------------------------
                    Else
                        '------------------------------------------------------
                        'Create a Uniqueness Constraint for the selected Roles
                        '  using the RoleConstraint created for the Model
                        '------------------------------------------------------
                        Call lrFactTypeInstance.FactType.CreateInternalUniquenessConstraint(larRole)

                        Call lrFactTypeInstance.Model.Save()

                        Call Me.EnableSaveButton()
                    End If

                    '---------------------------
                    'Clear the selected objects
                    '---------------------------
                    Me.zrPage.SelectedObject.Clear()
                    Diagram.Selection.Clear()
                Else
                    ls_message = "The 'Fact Type', " & Chr(34) & lrFactTypeInstance.Name & Chr(34) & ", spans " & liFactTypeCardinality & " 'Roles'. You must select at least " & (liFactTypeCardinality - 1) & " Roles before creating an 'Internal Uniqueness Constraint' for this Fact Type."
                    MsgBox(ls_message)
                End If
            Else
                ls_message = "Please select 'Roles' within one 'Fact Type' before creating an 'Internal Uniqueness Constraint'"
                MsgBox(ls_message)
            End If
        Else
            ls_message = "Please select 'Roles' within one 'Fact Type' before creating an 'Internal Uniqueness Constraint'"
            MsgBox(ls_message)
        End If

    End Sub

    Private Sub mnuOption_AddUniquenessConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_AddUniquenessConstraint.Click

        Using loWaitCursor As New WaitCursor
            Call Me.AddUniquenessConstraint()
            Call Me.zrPage.Model.Save()
        End Using

    End Sub

    Private Sub mnuOption_Toolbox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_Toolbox.Click

        Call frmMain.LoadToolbox()
        Call Me.SetToolbox()

    End Sub

    Private Sub mnuOption_CopyImageToClipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_CopyImageToClipboard.Click

        Call Me.CopyImageToClipboard()

    End Sub

    Private Sub DeonticToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeonticToolStripMenuItem.Click

        DeonticToolStripMenuItem.Checked = Not DeonticToolStripMenuItem.Checked

        Dim lrRoleInstance As FBM.RoleInstance

        lrRoleInstance = Me.zrPage.SelectedObject(0)

        lrRoleInstance.Deontic = DeonticToolStripMenuItem.Checked
        Call lrRoleInstance.refresh_role_instance()
        '--------------------------------------------
        'Refresh the PropertiesGrid if it is showing
        '--------------------------------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleInstance
        End If

    End Sub

    Private Sub DeonticToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeonticToolStripMenuItem.Click

    End Sub

    Private Sub ErrorListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ErrorListToolStripMenuItem.Click

        'Call Me.zrPage.Model.ReviewModelErrors()
        Call frmMain.loadToolboxErrorListForm(Me.DockPanel.ActivePane)

    End Sub



    Public Sub select_all()

        Dim liInd As Integer = 0
        Dim lrValueTypeInstance As FBM.ValueTypeInstance
        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
        Dim lrFactTypeInstance As FBM.FactTypeInstance
        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
        Dim lrModelNoteInstance As FBM.ModelNoteInstance

        Me.zrPage.SelectedObject.Clear()

        For Each lrValueTypeInstance In Me.zrPage.ValueTypeInstance
            Me.zrPage.SelectedObject.Add(lrValueTypeInstance)
        Next

        For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
            Me.zrPage.SelectedObject.Add(lrEntityTypeInstance)
        Next

        For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
            Me.zrPage.SelectedObject.Add(lrFactTypeInstance)
        Next

        For Each lrRoleConstraintInstance In Me.zrPage.RoleConstraintInstance
            Select Case lrRoleConstraintInstance.RoleConstraintType
                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                    For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                        'lrRoleConstraintRoleInstance.Shape.Selected = True
                    Next
                    Me.zrPage.SelectedObject.Add(lrRoleConstraintInstance)
                Case Else
                    'lrRoleConstraintInstance.Shape.Selected = True
                    Me.zrPage.SelectedObject.Add(lrRoleConstraintInstance)
            End Select
        Next

        For Each lrModelNoteInstance In Me.zrPage.ModelNote
            'lrModelNoteInstance.Shape.Selected = True
            Me.zrPage.SelectedObject.Add(lrModelNoteInstance)
        Next

    End Sub

    Private Sub ORMVerbalisationViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ORMVerbalisationViewToolStripMenuItem.Click

        prApplication.WorkingModel = Me.zrPage.Model
        prApplication.WorkingPage = Me.zrPage

        Call frmMain.loadToolboxORMVerbalisationForm(Me.zrPage.Model, Me.DockPanel.ActivePane)

    End Sub


    Private Sub RichmondBrainBoxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichmondBrainBoxToolStripMenuItem.Click

        prApplication.WorkingModel = Me.zrPage.Model
        prApplication.WorkingPage = Me.zrPage

        frmMain.Cursor = Cursors.WaitCursor
        Call frmMain.loadToolboxRichmondBrainBox(Me.zrPage, Me.DockPanel.ActivePane)
        frmMain.Cursor = Cursors.Default

    End Sub

    ''' <summary>
    ''' Adds a Fact to the selected FactTable/FactType
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripMenuItemAddFact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemAddFact.Click

        Dim liInd As Integer = 0
        Dim lrTableNode As New TableNode
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        Dim lrFactData As New FBM.FactData
        Dim lsMessage As String

        Try

            If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub

            If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.FactTable Then

                '------------------------
                'Add a row to the table
                '------------------------
                lrTableNode = Me.zrPage.SelectedObject(0).TableShape
                Call lrTableNode.AddRow()

                '-----------------------------------
                'Reset the height of the FactTable
                '-----------------------------------
                Dim liX As Single = lrTableNode.Bounds.X
                Dim liY As Single = lrTableNode.Bounds.Y
                Dim liWidth As Single = lrTableNode.Bounds.Width
                Dim liHeight As Single = lrTableNode.RowHeight * (lrTableNode.RowCount + 1)
                Dim loRectangle As New Rectangle(liX, liY, liWidth, liHeight)
                lrTableNode.SetRect(loRectangle, False)

                '--------------------------------
                'Add a new Fact to the FactType
                '--------------------------------
                Dim lrFactType As New FBM.FactType
                Dim lrRole As New FBM.Role

                lrFactTypeInstance = lrTableNode.Tag.FactTypeInstance
                lrFactType = lrFactTypeInstance.FactType

                Dim lrFact As New FBM.Fact

                If lrFactType.canCreateFactFor() Then

                    lrFact = Me.zrPage.Model.CreateFact(lrFactType)
                    Call lrFact.makeDirty() 'Because we want this fact saved to the database
                    Call Me.zrPage.MakeDirty()

                    If IsSomething(lrFact) Then
                        '---------------------------------
                        'Ad the new Fact to the FactType
                        '---------------------------------
                        lrFactType.AddFact(lrFact)

                        lrFactTypeInstance.Id = lrFactType.Id
                        lrFactTypeInstance = Me.zrPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)

                        Dim lrFactInstance As FBM.FactInstance

                        lrFactInstance = Me.zrPage.CreateFactInstance(lrFactTypeInstance, lrFact)

                        '--------------------------------------------------
                        'Add the new FactInstance to the FactTypeInstance
                        '--------------------------------------------------
                        lrFactTypeInstance.Fact.Add(lrFactInstance)

                        lrFactTypeInstance.FactTable.ResortFactTable()

                        If Not lrFactType.IsObjectified Then
                            If lrFactType.IsBinaryFactType Then
                                '----------------------------------------------------------------------
                                'FactType must join to an EntityType or an ObjectifiedFactType
                                Dim larETorFT = {pcenumConceptType.EntityType, pcenumConceptType.FactType}
                                Dim liETorOFTCount As Integer = lrFactType.RoleGroup.FindAll(Function(x) larETorFT.Contains(x.JoinedORMObject.ConceptType)).Count
                                If liETorOFTCount = 1 Then
                                    Dim lrModelObject As FBM.ModelObject = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.ConceptType <> pcenumConceptType.ValueType).JoinedORMObject
                                    If lrModelObject.ConceptType = pcenumConceptType.EntityType Then
                                        Dim lrEntityType As FBM.EntityType = lrModelObject
                                        'Get primary key data for the Fact just created.
                                        Dim lsPrimaryKeyData As String = lrFact.Data.Find(Function(x) x.Role.JoinedORMObject.Id = lrEntityType.Id).Data
                                        If lrEntityType.HasSimpleReferenceScheme Then

                                            Dim lasData As New List(Of String)
                                            lasData.Add(lsPrimaryKeyData)
                                            lasData.Add(lsPrimaryKeyData)

                                            lrFact = Me.zrPage.Model.CreateFact(lrEntityType.ReferenceModeFactType, lasData)

                                            Dim lrEntityTypeInstance As FBM.EntityTypeInstance = lrFactTypeInstance.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lrEntityType.Id).JoinedORMObject

                                            lrEntityTypeInstance.ReferenceModeFactType.FactType.AddFact(lrFact)

                                            lrFactInstance = lrFact.CloneInstance(Me.zrPage)
                                            lrEntityTypeInstance.ReferenceModeFactType.Fact.Add(lrFactInstance)
                                            lrEntityTypeInstance.ReferenceModeFactType.FactTable.ResortFactTable()
                                        End If
                                    End If
                                End If
                            End If
                        End If

                    End If 'Selected object is a FactTable

                    Call Me.zrPage.Model.checkForErrors()

                    Me.Diagram.Invalidate()

                    Call Me.EnableSaveButton()

                Else
                    '-------------------------------------------------------------------------------------
                    'Explain to the User why they can't create a Fact for this FactType.
                    For Each lrRole In lrFactType.RoleGroup.FindAll(Function(x) x.TypeOfJoin = pcenumRoleJoinType.FactType)
                        '---------------------------------------------------------------
                        'New FactData must reference a Fact in the joined FactType
                        '---------------------------------------------------------------
                        Dim lrJoinedFactType As New FBM.FactType
                        lrJoinedFactType = lrRole.JoinedORMObject
                        If lrJoinedFactType.Fact.Count = 0 Then
                            lsMessage = "The Fact Type, '" & lrJoinedFactType.Id & "', must have at least one Sample Population before adding a Fact to this Fact Type."
                            MsgBox(lsMessage)
                        End If
                    Next 'Role

                End If

            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ResizeToFitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToFitToolStripMenuItem1.Click

        Dim liInd As Integer = 0
        Dim liInd2 As Integer = 0
        Dim lr_cell As MindFusion.Diagramming.TableNode.Cell
        Dim lrTableNode As MindFusion.Diagramming.TableNode

        If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.FactTable Then
            '------------------
            'Get the TableNode
            '------------------
            lrTableNode = Me.zrPage.SelectedObject(0).TableShape

            If lrTableNode.RowCount > 0 Then
                lrTableNode.ResizeToFitText(True, False)
            End If

            For liInd = 0 To lrTableNode.ColumnCount - 1
                lrTableNode.Columns(liInd).ColumnStyle = ColumnStyle.AutoWidth
            Next

            '---------------------------------------------------------------------
            'Get rid of any Rows with no data
            '---------------------------------------------------------------------
            For liInd = lrTableNode.RowCount - 1 To 0 Step -1
                For liInd2 = 0 To lrTableNode.ColumnCount - 1
                    lr_cell = lrTableNode.Item(liInd2, liInd)
                    If lr_cell.Text <> "" Then
                        Exit Sub
                    End If
                Next
                lrTableNode.RowCount = liInd
            Next
        End If

    End Sub

    Sub expand_InternalUniquenessConstraints()

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

        ''-------------------------------------------------------
        ''Check to see if the user was clicking over a ShapeNode
        ''-------------------------------------------------------
        'lo_point = Diagram.PixelToUnit(e.Location)

        'If IsSomething(Diagram.GetNodeAt(lo_point)) Then
        '----------------------------------------------
        'Mouse is over a ShapeNode
        '----------------------------------------------
        '-----------------------------------------------------------------------------------
        'Set/Reset the color of the ShapeNode under the mouse cursor
        '-----------------------------------------------------------------------------------
        'loNode = Diagram.GetNodeAt(lo_point)

        '------------------------------------------------------------------------------------
        'If the Node is a FactType or a Role make the InternalUniquenessConstraints
        '  bigger so that they can be selected more easily.
        '------------------------------------------------------------------------------------

        For Each lrRoleConstraintInstance In Me.zrPage.RoleConstraintInstance
            Select Case lrRoleConstraintInstance.RoleConstraintType
                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                    For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                        lrRoleConstraintRoleInstance.shape.Resize(6, 3)
                    Next
            End Select
        Next

    End Sub

    ''' <summary>
    ''' Makes sure that no RoleConstraintInstance is in the process of having a RoleConstraintArgument created.
    '''   - See Me.Diagram.NodeDoubleClicked.
    '''   - See Me.DiagramView.MouseDown (When clicking on the Canvas, that will cancel all RoleConstrateArgument creation processes.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CancelAllRoleConstraintArgumentCreationStati()

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
        Dim lrRoleInstance As FBM.RoleInstance

        For Each lrRoleConstraintInstance In Me.zrPage.RoleConstraintInstance
            lrRoleConstraintInstance.CreatingArgument = False
        Next

        For Each lrRoleInstance In Me.zrPage.RoleInstance
            lrRoleInstance.Shape.Text = ""
        Next

    End Sub

    Sub contract_InternalUniquenessConstraints()

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

        For Each lrRoleConstraintInstance In Me.zrPage.RoleConstraintInstance
            Select Case lrRoleConstraintInstance.RoleConstraintType
                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                    For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                        lrRoleConstraintRoleInstance.shape.Resize(6, 0.05)
                    Next
            End Select
        Next

        Me.zrPage.InternalUniquenessConstraintsExpanded = False

    End Sub


    Sub SetToolbox()

        Try

            Dim lrToolboxForm As frmToolbox
            Dim loShapeLibrary As ShapeLibrary

            lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

            If IsSomething(lrToolboxForm) Then

                Call Directory.SetCurrentDirectory(Richmond.MyPath)
                loShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.ORMShapeLibrary)

                lrToolboxForm.ShapeListBox.Shapes = loShapeLibrary.Shapes

                Dim lo_shape As Shape

                For Each lo_shape In lrToolboxForm.ShapeListBox.Shapes
                    Select Case lo_shape.Id
                        Case Is = "Entity Type"
                            lo_shape.Image = My.Resources.ORMShapes.EntityType
                        Case Is = "Value Type"
                            lo_shape.Image = My.Resources.ORMShapes.ValueType
                        Case Is = "Test"
                            lo_shape.Image = My.Resources.ORMShapes.SubtypeConnector
                        Case Is = "Subtype Connector"
                            lo_shape.Image = My.Resources.ORMShapes.SubtypeConnector
                        Case Is = "Ring Constraint"
                            lo_shape.Image = My.Resources.ORMShapes.acyclic
                        Case Is = "External Uniqueness Constraint"
                            lo_shape.Image = My.Resources.ORMShapes.externalUniqueness
                        Case Is = "Equality Constraint"
                            lo_shape.Image = My.Resources.ORMShapes.equality
                        Case Is = "Exclusion Constraint"
                            lo_shape.Image = My.Resources.ORMShapes.exclusion
                        Case Is = "Inclusive-OR Constraint"
                            lo_shape.Image = My.Resources.ORMShapes.inclusive_or
                        Case Is = "Exclusive-OR Constraint"
                            lo_shape.Image = My.Resources.ORMShapes.exclusiveOr
                        Case Is = "Subset Constraint"
                            lo_shape.Image = My.Resources.ORMShapes.subset
                        Case Is = "Frequency Constraint"
                            lo_shape.Image = My.Resources.ORMShapes.frequency_ge
                        Case Is = "Model Note"
                            lo_shape.ImageRectangle = New RectangleF(0, 0, 75, 120)
                            lo_shape.Image = My.Resources.ORMShapes.ModelNote
                    End Select
                Next
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub ToolboxShapeListItemClick(ByVal aiToolboxShapeFunction As pcenumToolboxShapeFunction)

        Select Case aiToolboxShapeFunction
            Case Is = pcenumToolboxShapeFunction.SubtypeConnectorFunction
                '-----------------------------------------------------------------------
                'The User has selected the SubtypeConnector Shape within the Toolbox
                '  [and wants to make one EntityType the Subtype of another EntityType
                '-----------------------------------------------------------------------

        End Select

    End Sub

    Public Function CropImage(ByVal img As Image, ByVal backgroundColor As Color, Optional ByVal margin As Integer = 0) As Image

        Dim minX As Integer = img.Width
        Dim minY As Integer = img.Height
        Dim maxX As Integer = 0
        Dim maxY As Integer = 0

        Using bmp As New Bitmap(img)

            For y As Integer = 0 To bmp.Height - 1
                For x As Integer = 0 To bmp.Width - 1
                    If bmp.GetPixel(x, y).ToArgb <> backgroundColor.ToArgb Then
                        If x < minX Then
                            minX = x
                        ElseIf x > maxX Then
                            maxX = x
                        End If
                        If y < minY Then
                            minY = y
                        ElseIf y > maxY Then
                            maxY = y
                        End If
                    End If
                Next
            Next

            Dim rect As New Rectangle(minX - margin, minY - margin, maxX - minX + 2 * margin + 1, maxY - minY + 2 * margin + 1)
            Dim cropped As Bitmap = bmp.Clone(rect, bmp.PixelFormat)

            Return cropped

        End Using

    End Function

    Private Function CreateFramedImage(ByVal Source As Image, ByVal BorderColor As Color, ByVal BorderThickness As Integer) As Image

        Dim b As New Bitmap(Source.Width + BorderThickness * 2, Source.Height + BorderThickness * 2)
        Dim g As Graphics = Graphics.FromImage(b)
        g.Clear(BorderColor)
        g.DrawImage(Source, BorderThickness, BorderThickness)
        g.Dispose()
        Return b
    End Function

    Sub CopyImageToClipboard()

        Dim li_rectf As New RectangleF
        li_rectf = Me.Diagram.GetContentBounds(False, True)

        'Dim lo_image_processor As New t_image_processor(Diagram.CreateImage(li_rectf, 100))

        Dim lr_image As Image = Diagram.CreateImage(li_rectf, 100)

        lr_image = Me.CropImage(lr_image, Color.White, 0)
        lr_image = Me.CreateFramedImage(lr_image, Color.White, 15)

        Me.Diagram.ShowGrid = False

        Me.Cursor = Cursors.WaitCursor

        Windows.Forms.Clipboard.SetImage(lr_image)

        '---------------------------------
        'Set the grid back to what it was
        '---------------------------------
        Me.Diagram.ShowGrid = mnuOption_ViewGrid.Checked

        Me.Cursor = Cursors.Default

    End Sub

    Public Sub writeImageToFile(ByVal asFileLocation As String)

        Dim li_rectf As New RectangleF
        li_rectf = Me.Diagram.GetContentBounds(False, True)

        'Dim lo_image_processor As New t_image_processor(Diagram.CreateImage(li_rectf, 100))

        Dim lr_image As Image = Diagram.CreateImage(li_rectf, 100)

        lr_image = Me.CropImage(lr_image, Color.White, 0)
        lr_image = Me.CreateFramedImage(lr_image, Color.White, 15)

        lr_image.Save(asFileLocation, System.Drawing.Imaging.ImageFormat.Jpeg)

    End Sub


    Private Sub ModelDictionaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModelDictionaryToolStripMenuItem.Click

        Call frmMain.LoadToolboxModelDictionary(True)

    End Sub


    Public Sub morphToORMDiagram(ByVal sender As Object, ByVal e As EventArgs)

        '-----------------------------------------------
        'Get the Menu that just called this procedure.
        '-----------------------------------------------
        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
        If Me.zrPage.SelectedObject.Count > 1 Then Exit Sub

        '----------------------------------------------------------
        'Diagram1 is on the HiddenDiagramView
        '  Is a MindFusion Diagram on which the morphing is done.
        '----------------------------------------------------------
        Me.HiddenDiagram.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        'Set the Zoom factor
        Me.HiddenDiagramView.ZoomFactor = My.Settings.DefaultPageZoomFactor
        Call Me.HiddenDiagramView.BringToFront()

        Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
        lrEntityTypeInstance = Me.zrPage.SelectedObject(0)

        Dim lrShapeNode As ShapeNode
        lrShapeNode = New ShapeNode(lrEntityTypeInstance.Shape)

        Me.MorphVector(0).Shape = lrShapeNode
        Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
        Me.HiddenDiagram.Invalidate()

        If IsSomething(frmMain.zfrmModelExplorer) Then

            Dim lrEnterpriseView As tEnterpriseEnterpriseView

            lrEnterpriseView = item.Tag
            prApplication.WorkingPage = lrEnterpriseView.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Actor/EntityType being morphed
            '------------------------------------------------------------------
            Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
            lrPage = lrEnterpriseView.Tag

            Dim larEntityTypeInstance = From EntityTypeInstance In lrPage.EntityTypeInstance
                                        Where EntityTypeInstance.Id = lrEntityTypeInstance.Id
                                        Select EntityTypeInstance

            For Each lrEntityTypeInstance In larEntityTypeInstance
                Exit For
            Next

            'Me.MorphTimer.Enabled = True
            'Me.MorphStepTimer.Enabled = True
            Me.MorphVector(0) = New tMorphVector(Me.MorphVector(0).StartPoint.X,
                                                 Me.MorphVector(0).StartPoint.Y,
                                                 lrEntityTypeInstance.X,
                                                 lrEntityTypeInstance.Y,
                                                 40,
                                                 Me.MorphVector(0).Shape)

            Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)
            If lrEntityTypeInstance.Shape IsNot Nothing Then
                Me.MorphVector(0).EndSize = New Rectangle(0, 0, lrEntityTypeInstance.Shape.Bounds.Width, lrEntityTypeInstance.Shape.Bounds.Height)
            Else
                Me.MorphVector(0).EndSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)
            End If

            Me.MorphVector(0).ModelElementId = Me.zrPage.SelectedObject(0).Id
            Me.MorphVector(0).InitialZoomFactor = Me.DiagramView.ZoomFactor
            Me.MorphVector(0).Shape.Image = My.Resources.ORMShapes.Blank
            If lrPage.DiagramView IsNot Nothing Then
                Me.MorphVector(0).TargetZoomFactor = lrPage.DiagramView.ZoomFactor
            Else
                Me.MorphVector(0).TargetZoomFactor = My.Settings.DefaultPageZoomFactor
            End If

            Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
            Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
            Me.MorphStepTimer.Start()
            'Me.MorphTimer.Start()
        End If

    End Sub

    Public Sub MorphToORMDiagramValueType(ByVal sender As Object, ByVal e As EventArgs)

        '-----------------------------------------------
        'Get the Menu that just called this procedure.
        '-----------------------------------------------
        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
        If Me.zrPage.SelectedObject.Count > 1 Then Exit Sub

        '----------------------------------------------------------
        'Diagram1 is on the HiddenDiagramView
        '  Is a MindFusion Diagram on which the morphing is done.
        '----------------------------------------------------------
        Me.HiddenDiagram.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        Dim lrValueTypeInstance As New FBM.ValueTypeInstance
        lrValueTypeInstance = Me.zrPage.SelectedObject(0)

        Dim lrShapeNode As ShapeNode
        lrShapeNode = lrValueTypeInstance.Shape.Clone(True)

        Me.MorphVector(0).Shape = lrShapeNode
        Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
        Me.HiddenDiagram.Invalidate()

        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lrEnterpriseView As tEnterpriseEnterpriseView
            lrEnterpriseView = item.Tag
            prApplication.WorkingPage = lrEnterpriseView.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Actor/EntityType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lrEnterpriseView.Tag.Model)
            lr_page = lrEnterpriseView.Tag
            Dim lrValueTypeInstanceList = From ValueTypeInstance In lr_page.ValueTypeInstance
                                          Where ValueTypeInstance.Id = lrValueTypeInstance.Id
                                          Select New FBM.ValueTypeInstance(lr_page.Model,
                                                                    lr_page,
                                                                    pcenumLanguage.ORMModel,
                                                                    lrValueTypeInstance.Name,
                                                                    True,
                                                                    ValueTypeInstance.X,
                                                                    ValueTypeInstance.Y)

            For Each lrValueTypeInstance In lrValueTypeInstanceList
                Exit For
            Next

            'Me.MorphTimer.Enabled = True
            'Me.MorphStepTimer.Enabled = True
            Me.MorphVector(0) = New tMorphVector(Me.MorphVector(0).StartPoint.X,
                                                 Me.MorphVector(0).StartPoint.Y,
                                                 lrValueTypeInstance.X,
                                                 lrValueTypeInstance.Y,
                                                 40,
                                                 Me.MorphVector(0).Shape)
            Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
            Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
            Me.MorphStepTimer.Start()
            'Me.MorphTimer.Start()
        End If

    End Sub

    Public Sub morphToERDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim item As ToolStripItem = CType(sender, ToolStripItem)

            Me.HiddenDiagramView.ZoomFactor = Me.DiagramView.ZoomFactor

            Me.HiddenDiagram.Nodes.Clear()
            Call Me.DiagramView.SendToBack()
            Call Me.HiddenDiagramView.BringToFront()

            'If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub

            Dim lrPageObject As New FBM.PageObject
            lrPageObject = Me.zrPage.SelectedObject(0).ClonePageObject

            Dim lrShapeNode As ShapeNode
            lrShapeNode = lrPageObject.Shape.Clone(True)
            lrShapeNode = New ShapeNode(lrPageObject.Shape)

            lrShapeNode.Text = lrPageObject.Name

            Me.MorphVector(0).ModelElementId = Me.zrPage.SelectedObject(0).Id
            Me.MorphVector(0).Shape = lrShapeNode
            Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
            Me.HiddenDiagram.Invalidate()

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = item.Tag 'Set when the ContextMenu.Opening event is triggered.            
                Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
                prApplication.WorkingPage = lrEnterpriseView.Tag

                '------------------------------------------------------------------
                'Get the X,Y co-ordinates of the Actor/EntityType being morphed
                '------------------------------------------------------------------
                Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
                lrPage = lrEnterpriseView.Tag

                Me.MorphVector(0).InitialZoomFactor = Me.DiagramView.ZoomFactor
                If lrPage.DiagramView IsNot Nothing Then
                    Me.MorphVector(0).TargetZoomFactor = lrPage.DiagramView.ZoomFactor
                End If

                Dim larFactDataInstance = From FactTypeInstance In lrPage.FactTypeInstance
                                          From FactInstance In FactTypeInstance.Fact
                                          From FactDataInstance In FactInstance.Data
                                          Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                                   And FactDataInstance.Role.Name = pcenumCMML.Element.ToString _
                                   And FactDataInstance.Concept.Symbol = lrPageObject.Name
                                          Select New FBM.FactDataInstance(lrPage, FactInstance, FactDataInstance.Role, FactDataInstance.Concept, FactDataInstance.X, FactDataInstance.Y)

                Dim lrFactDataInstance As New FBM.FactDataInstance
                For Each lrFactDataInstance In larFactDataInstance
                    Exit For
                Next

                If lrFactDataInstance.TableShape IsNot Nothing Then
                    Me.MorphVector(0).EndSize = New Rectangle(lrFactDataInstance.X,
                                                              lrFactDataInstance.Y,
                                                              lrFactDataInstance.TableShape.Bounds.Width,
                                                              lrFactDataInstance.TableShape.Bounds.Height)
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                End If

                '===========================================
                Dim lrEntity As ERD.Entity
                lrEntity = lrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrPageObject.Name)

                If lrEntity IsNot Nothing Then
                    Me.MorphVector(0).EndSize = New Rectangle(lrEntity.X,
                                                              lrEntity.Y,
                                                              lrEntity.TableShape.Bounds.Width,
                                                              lrEntity.TableShape.Bounds.Height)
                    Me.MorphVector(0).EndPoint = New Point(lrEntity.TableShape.Bounds.X, lrEntity.TableShape.Bounds.Y) ' (lrFactDataInstance.x, lrFactDataInstance.y)
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                    Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.X, lrFactDataInstance.Y)
                End If

                '===========================================
                Me.MorphVector(0).Shape.Font = Me.zrPage.Diagram.Font
                Me.MorphVector(0).Shape.Text = Me.MorphVector(0).ModelElementId
                Me.MorphVector(0).Shape.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
                Me.MorphVector(0).Shape.TextFormat.Alignment = StringAlignment.Center
                Me.MorphVector(0).Shape.TextFormat.LineAlignment = StringAlignment.Center

                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True
                Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
                Me.MorphStepTimer.Start()
                Me.MorphTimer.Start()

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub MorphToORMDiagramFactType(ByVal sender As Object, ByVal e As EventArgs)

        '-----------------------------------------------
        'Get the Menu that just called this procedure.
        '-----------------------------------------------
        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
        If Me.zrPage.SelectedObject.Count > 1 Then Exit Sub

        '----------------------------------------------------------
        'Diagram1 is on the HiddenDiagramView
        '  Is a MindFusion Diagram on which the morphing is done.
        '----------------------------------------------------------
        Me.HiddenDiagram.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        lrFactTypeInstance = Me.zrPage.SelectedObject(0)

        Dim lrShapeNode As ShapeNode
        lrShapeNode = lrFactTypeInstance.Shape.Clone(True)

        Me.MorphVector(0).Shape = lrShapeNode
        Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
        Me.HiddenDiagram.Invalidate()

        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lrEnterpriseView As tEnterpriseEnterpriseView
            lrEnterpriseView = item.Tag
            prApplication.WorkingPage = lrEnterpriseView.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Actor/EntityType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lrEnterpriseView.Tag.Model)
            lr_page = lrEnterpriseView.Tag
            Dim lrFactTypeInstanceList = From FactTypeInstance In lr_page.FactTypeInstance
                                         Where FactTypeInstance.Id = lrFactTypeInstance.Id
                                         Select New FBM.FactTypeInstance(lr_page.Model,
                                                                    lr_page,
                                                                    pcenumLanguage.ORMModel,
                                                                    lrFactTypeInstance.Name,
                                                                    True,
                                                                    FactTypeInstance.X,
                                                                    FactTypeInstance.Y)

            For Each lrFactTypeInstance In lrFactTypeInstanceList
                Exit For
            Next

            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True
            Me.MorphVector(0) = New tMorphVector(Me.MorphVector(0).StartPoint.X,
                                                 Me.MorphVector(0).StartPoint.Y,
                                                 lrFactTypeInstance.X,
                                                 lrFactTypeInstance.Y,
                                                 40,
                                                 Me.MorphVector(0).Shape)
            Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
            Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
            Me.MorphStepTimer.Start()
            Me.MorphTimer.Start()
        End If

    End Sub

    Public Sub morphToPGSDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim item As ToolStripItem = CType(sender, ToolStripItem)

            Me.HiddenDiagramView.ZoomFactor = Me.DiagramView.ZoomFactor

            Me.HiddenDiagram.Nodes.Clear()
            Call Me.DiagramView.SendToBack()
            Call Me.HiddenDiagramView.BringToFront()

            'If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub

            Dim lrPageObject As New FBM.PageObject
            lrPageObject = Me.zrPage.SelectedObject(0).ClonePageObject

            Dim lrShapeNode As ShapeNode
            lrShapeNode = lrPageObject.Shape.Clone(True)
            lrShapeNode = New ShapeNode(lrPageObject.Shape)

            lrShapeNode.Text = lrPageObject.Name

            Me.MorphVector(0).ModelElementId = Me.zrPage.SelectedObject(0).Id
            Me.MorphVector(0).Shape = lrShapeNode
            Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
            Me.HiddenDiagram.Invalidate()

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = item.Tag 'Set when the ContextMenu.Opening event is triggered.            
                Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
                prApplication.WorkingPage = lrEnterpriseView.Tag

                '------------------------------------------------------------------
                'Get the X,Y co-ordinates of the Actor/EntityType being morphed
                '------------------------------------------------------------------
                Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
                lrPage = lrEnterpriseView.Tag

                Me.MorphVector(0).InitialZoomFactor = Me.DiagramView.ZoomFactor
                If lrPage.DiagramView IsNot Nothing Then
                    Me.MorphVector(0).TargetZoomFactor = lrPage.DiagramView.ZoomFactor
                End If

                Dim larFactDataInstance = From FactTypeInstance In lrPage.FactTypeInstance
                                          From FactInstance In FactTypeInstance.Fact
                                          From FactDataInstance In FactInstance.Data
                                          Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                                   And FactDataInstance.Role.Name = pcenumCMML.Element.ToString _
                                   And FactDataInstance.Concept.Symbol = lrPageObject.Name
                                          Select New FBM.FactDataInstance(lrPage, FactInstance, FactDataInstance.Role, FactDataInstance.Concept, FactDataInstance.X, FactDataInstance.Y)

                Dim lrFactDataInstance As New FBM.FactDataInstance
                For Each lrFactDataInstance In larFactDataInstance
                    Exit For
                Next

                If lrFactDataInstance.shape IsNot Nothing Then
                    Me.MorphVector(0).EndSize = New Rectangle(lrFactDataInstance.X,
                                                              lrFactDataInstance.Y,
                                                              lrFactDataInstance.shape.Bounds.Width,
                                                              lrFactDataInstance.shape.Bounds.Height)
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                End If

                'Start size
                Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)
                '===========================================
                Dim lrNode As PGS.Node

                lrNode = lrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrPageObject.Name)

                If lrNode IsNot Nothing Then
                    If lrNode.NodeType = pcenumPGSEntityType.Relationship Then
                        Dim lrRelation As ERD.Relation
                        lrRelation = lrPage.ERDiagram.Relation.Find(Function(x) x.ActualPGSNode.Id = lrPageObject.Name)

                        Dim lrPGSLink As PGS.Link = lrRelation.Link

                        Me.MorphVector(0).EndSize = New Rectangle(lrPGSLink.Link.Bounds.X, lrPGSLink.Link.Bounds.Y, lrPGSLink.Link.Bounds.Width, Viev.Greater(1, lrPGSLink.Link.Bounds.Height))
                        Me.MorphVector(0).EndPoint = New Point(lrRelation.Link.Link.Bounds.X, lrRelation.Link.Link.bounds.Y)
                    Else
                        Me.MorphVector(0).EndSize = New Rectangle(lrNode.X,
                                                                  lrNode.Y,
                                                                  lrNode.shape.Bounds.Width,
                                                                  lrNode.shape.Bounds.Height)
                        Me.MorphVector(0).EndPoint = New Point(lrNode.shape.Bounds.X, lrNode.shape.Bounds.Y) ' (lrFactDataInstance.x, lrFactDataInstance.y)
                    End If
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 20)
                    Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.X, lrFactDataInstance.Y)
                End If

                '===========================================
                Me.MorphVector(0).Shape.Font = Me.zrPage.Diagram.Font
                Me.MorphVector(0).Shape.Text = Me.MorphVector(0).ModelElementId
                Me.MorphVector(0).Shape.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
                Me.MorphVector(0).Shape.TextFormat.Alignment = StringAlignment.Center
                Me.MorphVector(0).Shape.TextFormat.LineAlignment = StringAlignment.Center

                Me.MorphVector(0).TargetShape = pcenumTargetMorphShape.Circle

                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True
                Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
                Me.MorphStepTimer.Start()
                Me.MorphTimer.Start()

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        Me.HiddenDiagram.Nodes.Clear()
        Me.CircularProgressBar.SendToBack()
        Call Me.HiddenDiagramView.SendToBack()

        Me.MorphTimer.Stop()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick

        Dim lrPoint As New Point
        Dim lrRectangle As New Rectangle
        Dim lrMorphVector As tMorphVector

        Try
            For Each lrMorphVector In Me.MorphVector
                lrPoint = lrMorphVector.getNextMorphVectorStepPoint
                lrMorphVector.Shape.Move(lrPoint.X, lrPoint.Y)

                lrRectangle = lrMorphVector.getNextMorphVectorRectangle
                lrMorphVector.Shape.Resize(lrRectangle.Width, lrRectangle.Height)

                If lrMorphVector.VectorStep > lrMorphVector.VectorSteps / 2 Then
                    Select Case lrMorphVector.TargetShape
                        Case Is = pcenumTargetMorphShape.Circle
                            lrMorphVector.Shape.Shape = Shapes.Ellipse
                            lrMorphVector.Shape.Image = My.Resources.ORMShapes.Blank
                        Case Else
                            lrMorphVector.Shape.Shape = Shapes.RoundRect
                    End Select
                End If
            Next

            Me.HiddenDiagramView.ZoomFactor = Me.MorphVector(0).InitialZoomFactor + ((Me.MorphVector(0).VectorStep / Me.MorphVector(0).VectorSteps) * (Me.MorphVector(0).TargetZoomFactor - Me.MorphVector(0).InitialZoomFactor))

            Me.HiddenDiagram.Invalidate()

            If Me.MorphVector(0).VectorStep > Me.MorphVector(0).VectorSteps Then
                Me.MorphStepTimer.Stop()
                'Me.MorphStepTimer.Enabled = False
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.MorphVector(0).EnterpriseTreeView.TreeNode
                Call frmMain.zfrmModelExplorer.LoadSelectedPage(Me.MorphVector(0).ModelElementId)
                Thread.Sleep(2000)
                Me.DiagramView.BringToFront()
                Me.Diagram.Invalidate()
                'Me.MorphTimer.Enabled = False
            End If

        Catch ex As Exception

            Me.MorphStepTimer.Stop()
            Me.DiagramView.BringToFront()
            Me.Diagram.Invalidate()

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

        End Try

    End Sub

    Private Sub ContextMenuStrip_ValueType_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ValueType.Opening

        Dim lr_page As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lrValueType As New FBM.ValueType
        Dim lrValueTypeInstance As New FBM.ValueTypeInstance
        Dim lo_menu_option As ToolStripItem

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            lrValueType = Me.zrPage.SelectedObject(0).ValueType
            lrValueTypeInstance = Me.zrPage.SelectedObject(0)
            lr_model = lrValueType.Model

            '--------------------------------------------------------------------
            'ModelErrors - Add menu items for the ModelErrors for the ValueType
            '--------------------------------------------------------------------
            Me.ToolStripMenuItemValueTypeModelErrors.DropDownItems.Clear()
            Dim lrModelError As FBM.ModelError
            If lrValueType.ModelError.Count > 0 Then
                Me.ToolStripMenuItemValueTypeModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
                For Each lrModelError In lrValueType.ModelError
                    lo_menu_option = Me.ToolStripMenuItemValueTypeModelErrors.DropDownItems.Add(lrModelError.Description)
                    lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
                Next
            Else
                Me.ToolStripMenuItemValueTypeModelErrors.Image = My.Resources.MenuImages.Cloud216x16
                lo_menu_option = Me.ToolStripMenuItemValueTypeModelErrors.DropDownItems.Add("There are no Model Errors for this Value Type.")
                lo_menu_option.Image = My.Resources.MenuImages.Cloud216x16
            End If


            '---------------------------------------------------------------------------------------------
            'Set the initial MorphVector for the selected ValueType. Morphing the EntityType to another 
            '  shape, and to/into another diagram starts at the MorphVector.
            '---------------------------------------------------------------------------------------------
            Me.MorphVector.Clear()
            Me.MorphVector.Add(New tMorphVector(lrValueTypeInstance.X, lrValueTypeInstance.Y, 0, 0, 40))

            '----------------------------------------------------------------
            'Clear the list of ORMDiagrams that may relate to the ValueType
            '----------------------------------------------------------------
            Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram.DropDownItems.Clear()

            '-----------------------------------------------------------------------------
            'Clear the list of StateTransitionDiagrams that may relate to the ValueType
            '-----------------------------------------------------------------------------
            Me.ToolStripMenuItem_StateTransitionDiagram.DropDownItems.Clear()

            '--------------------------------------------------------
            'Load the ORMDiagrams that relate to the ValueType
            '  as selectable menuOptions
            '--------------------------------------------------------        
            larPage_list = prApplication.CMML.get_orm_diagram_pages_for_value_type(lrValueType)

            For Each lr_page In larPage_list
                '----------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, 
                '  they will not be in the TreeView and so a menuOption
                '  is now added for those hidden Pages.
                '----------------------------------------------------------
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                           lr_page,
                                                           lr_model.ModelId,
                                                           pcenumLanguage.ORMModel,
                                                           Nothing,
                                                           lr_page.PageId)
                lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                If IsSomething(lrEnterpriseView) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    'lo_menu_option = Me.ORMDiagramToolStripMenuItem.DropDownItems.Add(lr_page.Name)
                    lo_menu_option = Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram.DropDownItems.Add(lr_page.Name)
                    lo_menu_option.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    AddHandler lo_menu_option.Click, AddressOf Me.MorphToORMDiagramValueType
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

    Public Sub EnableSaveButton()

        Try
            '-------------------------------------
            'Raised from ModelObjects themselves
            '-------------------------------------
            frmMain.ToolStripButton_Save.Enabled = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ContextMenuStrip_FactType_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_FactType.Opening

        Dim lr_page As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lrFactType As New FBM.FactType
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        Dim lo_menu_option As ToolStripItem


        If Me.zrPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If

        Dim lrModelObject As FBM.ModelObject = Me.zrPage.SelectedObject(0)

        Select Case lrModelObject.ConceptType
            Case Is = pcenumConceptType.Role
                Dim lrRoleInstance As FBM.RoleInstance
                lrRoleInstance = Me.zrPage.SelectedObject(0)
                lrFactTypeInstance = lrRoleInstance.FactType
            Case Is = pcenumConceptType.FactType
                lrFactTypeInstance = Me.zrPage.SelectedObject(0)
        End Select
        lrFactType = lrFactTypeInstance.FactType
        lr_model = lrFactType.Model

        '----------------------------------------------------------------------------------------------
        'If the FactType is a LinkFactType then the user can't remove the FactType from the Model,
        '  except by chaning the FactType to being one that is not Objectified.
        '----------------------------------------------------------------------------------------------
        'Me.ToolStripMenuItemFactTypeRemoveFromPageModel.Enabled = Not lrFactType.IsLinkFactType

        '==============================================================================================
        'Special menu options for SubtypeRelationship FactTypes.        
        Me.ToolStripMenuItemFactTypeInstanceRemoveFromPage.Enabled = Not lrFactType.IsSubtypeRelationshipFactType
        Me.ToolStripMenuItemFactTypeRemoveFromPageModel.Enabled = Not (lrFactType.IsSubtypeRelationshipFactType Or lrFactType.IsLinkFactType)
        '===================---------------------------------------------------------------------------

        '--------------------------------------------------------------------
        'ModelErrors - Add menu items for the ModelErrors for the FactType
        '--------------------------------------------------------------------
        Me.ToolStripMenuItemFactTypeModelErrors.DropDownItems.Clear()
        Dim lrModelError As FBM.ModelError
        If lrFactType.ModelError.Count > 0 Then
            Me.ToolStripMenuItemFactTypeModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
            For Each lrModelError In lrFactType.ModelError
                lo_menu_option = Me.ToolStripMenuItemFactTypeModelErrors.DropDownItems.Add(lrModelError.Description)
                lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
            Next
        Else
            Me.ToolStripMenuItemFactTypeModelErrors.Image = My.Resources.MenuImages.Cloud216x16
            lo_menu_option = Me.ToolStripMenuItemFactTypeModelErrors.DropDownItems.Add("There are no Model Errors for this Fact Type.")
            lo_menu_option.Image = My.Resources.MenuImages.Cloud216x16
        End If

        ToolStripMenuItemViewFactTable.Checked = lrFactTypeInstance.FactTable.TableShape.Visible

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for the selected FactType. Morphing the FactType to another 
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Me.MorphVector.Clear()
        Me.MorphVector.Add(New tMorphVector(lrFactTypeInstance.X, lrFactTypeInstance.Y, 0, 0, 40))

        '--------------------------------------------------------------
        'Clear the list of diagrams that may relate to the FactType
        '--------------------------------------------------------------
        Me.ORMFromFactTypeToolStripMenuItem.DropDownItems.Clear()
        Me.ERDiagramFromFactTypeToolStripMenuItem.DropDownItems.Clear()
        Me.PGSDiagramToolStripMenuItem.DropDownItems.Clear()

        '--------------------------------------------------------
        'Load the ORMDiagrams that relate to the FactType
        '  as selectable menuOptions
        '--------------------------------------------------------        
        larPage_list = prApplication.CMML.get_orm_diagram_pages_for_FactType(lrFactType)

        For Each lr_page In larPage_list
            '----------------------------------------------------------
            'Try and find the Page within the EnterpriseView.TreeView
            '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
            '  is now added for those hidden Pages.
            '----------------------------------------------------------
            Dim lrEnterpriseView As tEnterpriseEnterpriseView

            lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = lr_page.PageId)

            If IsSomething(lrEnterpriseView) Then
                '---------------------------------------------------
                'Add the Page(Name) to the MenuOption.DropDownItems
                '---------------------------------------------------
                lo_menu_option = Me.ORMFromFactTypeToolStripMenuItem.DropDownItems.Add(lr_page.Name)
                lo_menu_option.Tag = lrEnterpriseView
                AddHandler lo_menu_option.Click, AddressOf Me.MorphToORMDiagramFactType
            End If
        Next

        '--------------------------------------------------------
        'Load the ER Diagrams that relate to the FactType
        '  as selectable menuOptions
        '--------------------------------------------------------        
        larPage_list = prApplication.CMML.getERDiagramPagesForModelElementName(lrFactType.Model, lrFactType.Id)

        For Each lr_page In larPage_list
            '----------------------------------------------------------
            'Try and find the Page within the EnterpriseView.TreeView
            '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
            '  is now added for those hidden Pages.
            '----------------------------------------------------------
            Dim lrEnterpriseView As tEnterpriseEnterpriseView

            lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = lr_page.PageId)

            If IsSomething(lrEnterpriseView) Then
                '---------------------------------------------------
                'Add the Page(Name) to the MenuOption.DropDownItems
                '---------------------------------------------------
                lo_menu_option = Me.ERDiagramFromFactTypeToolStripMenuItem.DropDownItems.Add(lr_page.Name)
                lo_menu_option.Tag = lrEnterpriseView
                AddHandler lo_menu_option.Click, AddressOf Me.morphToERDiagram
            End If
        Next
        If larPage_list.Count = 0 Then
            '----------------------------------------------------------------------------------
            'Provide and option for the User to create a PGS Page for the current Page.
            Dim lsMessage As String = "Create an Entity Relationship Diagram for this current Page. No ERD Page currently exists for the model element you have selected."
            lo_menu_option = Me.ERDiagramFromFactTypeToolStripMenuItem.DropDownItems.Add(lsMessage, My.Resources.MenuImages.ERD16x16)
            lo_menu_option.Tag = True
            AddHandler lo_menu_option.Click, AddressOf Me.createEntityRelationshipDiagramPageForCurrentPage
        End If
        '==================================================================================================

        '==================================================================================================
        'Load the Property Graph Schema Diagrams that relate to the EntityType, as selectable menuOptions
        '--------------------------------------------------------        
        larPage_list = prApplication.CMML.getPGSDiagramPagesForModelElementName(lrFactType.Model, lrFactType.Id)

        For Each lrPage In larPage_list
            '----------------------------------------------------------
            'Try and find the Page within the EnterpriseView.TreeView
            '  NB If 'Core' Pages are not shown for the model, 
            '  they will not be in the TreeView and so a menuOption
            '  is now added for those hidden Pages.
            '----------------------------------------------------------
            Dim lrEnterpriseView As tEnterpriseEnterpriseView

            lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId) ' AddressOf lrEnterpriseView.Equals)

            If IsSomething(lrEnterpriseView) Then
                '---------------------------------------------------
                'Add the Page(Name) to the MenuOption.DropDownItems
                '---------------------------------------------------
                lo_menu_option = Me.PGSDiagramToolStripMenuItem.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.PGS16x16)
                lo_menu_option.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                AddHandler lo_menu_option.Click, AddressOf Me.morphToPGSDiagram
            End If

            Me.ERDiagramToolStripMenu.Enabled = True
        Next

        If larPage_list.Count = 0 Then
            '----------------------------------------------------------------------------------
            'Provide and option for the User to create a PGS Page for the current Page.
            Dim lsMessage As String = "Create an Property Graph Schema for this current Page. No PGS Page currently exists for the model element you have selected."
            lo_menu_option = Me.PGSDiagramToolStripMenuItem.DropDownItems.Add(lsMessage, My.Resources.MenuImages.ERD16x16)
            lo_menu_option.Tag = True
            AddHandler lo_menu_option.Click, AddressOf Me.createPropertyGraphSchemaPageFromCurrentPage
        End If
        '======================================================================================


    End Sub


    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click

        Call frmMain.loadToolboxORMReadingEditor(Me.zrPage, Me.DockPanel.ActivePane)

    End Sub

    Private Sub RemoveallInternalUniquenessConstraintsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveallInternalUniquenessConstraintsToolStripMenuItem.Click

        Dim lrFactTypeInstance As FBM.FactTypeInstance
        Dim lsMessage As String = ""

        '------------------------------------------------------
        'Get the FactType for the InternalUniquenessConstraint
        '------------------------------------------------------                    
        lrFactTypeInstance = Me.zrPage.SelectedObject(0)

        If lrFactTypeInstance.FactType.IsObjectified Then
            lsMessage = "Remove the objectification of this Fact Type before removing all of the Internal Uniqueness Constraints of the Fact Type."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "A Fact Type cannot be objectified if it has no Internal Uniqueness Constraints."
            MsgBox(lsMessage)
        Else
            Call lrFactTypeInstance.FactType.RemoveInternalUniquenessConstraints(True)
        End If



    End Sub

    Private Sub LockToThisPositionOnPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LockToThisPositionOnPageToolStripMenuItem.Click

        Dim lrShapeNode As New MindFusion.Diagramming.ShapeNode

        lrShapeNode = Me.zrPage.SelectedObject(0).Shape

        Me.LockToThisPositionOnPageToolStripMenuItem.Checked = Not Me.LockToThisPositionOnPageToolStripMenuItem.Checked

        If Me.LockToThisPositionOnPageToolStripMenuItem.Checked Then
            lrShapeNode.IgnoreLayout = True
        Else
            lrShapeNode.IgnoreLayout = False
        End If

    End Sub

    Private Sub ToolStripMenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem8.Click

        prApplication.WorkingPage = Me.zrPage
        Call frmMain.loadToolboxORMReadingEditor(Me.zrPage, Me.DockPanel.ActivePane)

    End Sub

    Private Sub ToolStripMenuItem9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemViewFactTable.Click

        Dim lrFactTypeInstance As FBM.FactTypeInstance

        Try
            lrFactTypeInstance = Me.zrPage.SelectedObject(0)
            ToolStripMenuItemViewFactTable.Checked = Not ToolStripMenuItemViewFactTable.Checked

            lrFactTypeInstance.FactTable.TableShape.Visible = ToolStripMenuItemViewFactTable.Checked

            lrFactTypeInstance.FactTable.TableShape.Move(lrFactTypeInstance.Shape.Bounds.X + 3, lrFactTypeInstance.Shape.Bounds.Bottom + 5)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RemoveFromPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromPageToolStripMenuItem.Click

        Dim lrEntityTypeInstance As FBM.EntityTypeInstance

        Try
            lrEntityTypeInstance = Me.zrPage.SelectedObject(0)

            Call lrEntityTypeInstance.RemoveFromPage(True)

            '-----------------------------------
            'Setup the Undo for the UserAction
            '-----------------------------------
            Dim lrUserAction As New tUserAction(lrEntityTypeInstance, pcenumUserAction.RemovedPageObjectFromPage, Me.zrPage)
            prApplication.AddUndoAction(lrUserAction)
            frmMain.ToolStripMenuItemUndo.Enabled = True

            Me.zrPage.MakeDirty()
            Call Me.EnableSaveButton()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub RemoveFromPageModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromPageModelToolStripMenuItem.Click

        Dim lrEntityTypeInstance As FBM.EntityTypeInstance

        Try
            With New WaitCursor
                lrEntityTypeInstance = Me.zrPage.SelectedObject(0)

                Me.zrPage.SelectedObject.Remove(lrEntityTypeInstance)

                If lrEntityTypeInstance Is Nothing Then Exit Sub

                Call lrEntityTypeInstance.EntityType.RemoveFromModel(False, True, True)
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem11.Click

        Dim lrValueTypeInstance As FBM.ValueTypeInstance

        Try
            lrValueTypeInstance = Me.zrPage.SelectedObject(0)

            Call lrValueTypeInstance.RemoveFromPage(True)

            Dim lrUserAction As New tUserAction(lrValueTypeInstance, pcenumUserAction.RemovedPageObjectFromPage, Me.zrPage)
            prApplication.AddUndoAction(lrUserAction)
            frmMain.ToolStripMenuItemUndo.Enabled = True

            Me.zrPage.MakeDirty()
            Call Me.EnableSaveButton()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ToolStripMenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem12.Click

        Try
            Dim lrValueTypeInstance As FBM.ValueTypeInstance

            lrValueTypeInstance = Me.zrPage.SelectedObject(0)

            'Vm-20180329-The below probably not required.
            'Call lrValueTypeInstance.RemoveFromPage()
            Call lrValueTypeInstance.ValueType.RemoveFromModel(False, True, True)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItem13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemFactTypeInstanceRemoveFromPage.Click

        Try
            Dim lrFactTypeInstance As FBM.FactTypeInstance

            lrFactTypeInstance = Me.zrPage.SelectedObject(0)

            Call lrFactTypeInstance.RemoveFromPage(True)

            Dim lrUserAction As New tUserAction(lrFactTypeInstance, pcenumUserAction.RemovedPageObjectFromPage, Me.zrPage)
            prApplication.AddUndoAction(lrUserAction)
            frmMain.ToolStripMenuItemUndo.Enabled = True

            Me.zrPage.MakeDirty()
            Call Me.EnableSaveButton()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemFactTypeRemoveFromPageModel.Click

        Try
            Dim lrFactTypeInstance As FBM.FactTypeInstance

            lrFactTypeInstance = Me.zrPage.SelectedObject(0)

            Using loWaitCursor As New WaitCursor
                Call lrFactTypeInstance.FactType.RemoveFromModel(True, , True)
            End Using

        Catch ex As Exception

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

        End Try

    End Sub

    Private Sub MenuItemHelpTips_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemHelpTips.Click

        MenuItemHelpTips.Checked = Not MenuItemHelpTips.Checked

        Me.LabelHelp.Visible = MenuItemHelpTips.Checked


    End Sub

    Private Sub ToolStripMenuItem25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintIrreflexiveToolStrip.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.Irreflexive
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.irreflexive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub RingConsgtraintAsymmetricMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConsgtraintAsymmetric.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.Asymmetric
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.Asymmetric

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub RnigConstraintIntransitiveMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRnigConstraintIntransitive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0) '  Me.Diagram.Selection.Items(0).Tag
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.Intransitive
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.intransitive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub RingConstraintAntisymmetricMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintAntisymmetric.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0) ' Me.Diagram.Selection.Items(0).Tag
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.Antisymmetric
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.Antisymmetric

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub RingConstraintAcyclicMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintAcyclic.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.Acyclic
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.acyclic

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub MenuItemRingConstraintAsymmetricIntransitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintAsymmetricIntransitive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.AsymmetricIntransitive
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.asymmetric_intransitive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub MenuItemRingConstraintAcyclicIntransitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintAcyclicIntransitive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.AcyclicIntransitive
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.acyclic_intransitive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub MenuItemRingConstraintSymmetric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintSymmetric.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.Symmetric
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.symmetric

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub MenuItemRingConstraintSymmetricIrreflexive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintSymmetricIrreflexive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.SymmetricIrreflexive
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.symmetric_irreflexive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub MenuItemRingConstraintSymmetricIntransitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintSymmetricIntransitive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.SymmetricIntransitive
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.symmetric_intransitive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)

        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub MenuItemRingConstraintPurelyReflexive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintPurelyReflexive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.PurelyReflexive
        lrRoleConstraintInstance.IsDeontic = False
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.purely_reflexive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

        '--------------------
        'Properties Toolbox
        '--------------------
        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        If IsSomething(lrPropertyGridForm) Then
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticIrreflexive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticIrreflexive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticIrreflexive
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_irreflexive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticAsymmetric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticAsymmetric.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticAssymmetric
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_asymmetric

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticIntransitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticIntransitive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticIntransitive
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_intransitive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticAntisymmetric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticAntisymmetric.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticAntisymmetric
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_antisymmetric

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemDeonticAcyclic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemDeonticAcyclic.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticAcyclic
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_acyclic

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticAsymmetricIntransitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticAsymmetricIntransitive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticAssymmetricIntransitive
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_asymmetric_intransitive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticAcyclicIntransitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticAcyclicIntransitive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticAcyclicIntransitive
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_acyclic_intransitive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticSymmetric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticSymmetric.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticSymmetric
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_symmetric

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticSymmetricIrreflexive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticSymmetricIrreflexive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticSymmetricIrreflexive
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_symmetric_irreflexive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticSymmetricIntransitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticSymmetricIntransitive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticSymmetricIntransitive
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_symmetric_intransitive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub MenuItemRingConstraintDeonticPurelyReflexive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRingConstraintDeonticPurelyReflexive.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
        lrRoleConstraintInstance.RingConstraintType = pcenumRingConstraintType.DeonticPurelyReflexive
        lrRoleConstraintInstance.Shape.Image = My.Resources.ORMShapes.deontic_purely_reflexive

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            Call lrToolboxForm.VerbaliseRoleConstraintRingConstraint(lrRoleConstraintInstance.RoleConstraint)
        End If

    End Sub

    Private Sub DeleteRowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteRowToolStripMenuItem.Click

        Dim lrTableNode As New TableNode
        Dim lrFactTable As New FBM.FactTable
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        Dim lrFactInstance As New FBM.FactInstance

        If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.FactTable Then

            lrTableNode = Me.zrPage.SelectedObject(0).TableShape
            lrFactTable = lrTableNode.Tag

            If lrFactTable.SelectedRow > 0 Then
                lrFactTypeInstance = lrFactTable.FactTypeInstance
                lrFactInstance = lrFactTypeInstance.Fact(lrFactTable.SelectedRow - 1)

                If lrFactInstance.FactType.Model.FactIsOnAnotherPage(Me.zrPage, lrFactInstance.Fact) Then
                    lrFactTable.TableShape.DeleteRow(lrFactTable.SelectedRow - 1)
                    lrFactTypeInstance.RemoveFact(lrFactInstance)
                Else
                    Dim lsMessage As String = ""
                    lsMessage = "The selected Fact is not shown on any other Page for the Model."
                    lsMessage &= vbCrLf
                    lsMessage &= "The Fact will be permanently deleted from the Model and the operation cannot be undone."
                    lsMessage &= vbCrLf & vbCrLf
                    lsMessage &= "Click [Ok] to proceed or [Cancel] to cancel the deletion of the Fact"
                    If MsgBox(lsMessage, MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                        lrFactTable.TableShape.DeleteRow(lrFactTable.SelectedRow - 1)
                        lrFactTypeInstance.RemoveFact(lrFactInstance)
                        lrFactTypeInstance.FactType.RemoveFactById(lrFactInstance.Fact)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Diagram_ValidateAnchorPoint(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkValidationEventArgs) Handles Diagram.ValidateAnchorPoint

        If IsSomething(e.Link) Then
            If IsSomething(e.Link.Tag) Then
                Select Case e.Link.Tag.ConceptType
                    Case Is = pcenumConceptType.Role
                        If e.ChangingOrigin Then
                            Select Case e.AnchorIndex
                                Case Is = 3, 4
                                    e.Cancel = True
                            End Select
                        End If
                    Case Is = pcenumConceptType.RoleConstraintRole

                        If e.ChangingDestination Then
                            If e.Link.Tag.IsEntry Then
                                Select Case e.AnchorIndex
                                    Case Is = 0, 1, 2
                                        e.Cancel = True
                                End Select
                            End If
                        End If
                End Select
            End If
        End If

    End Sub

    Private Sub mnuOption_ViewGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_ViewGrid.Click

        mnuOption_ViewGrid.Checked = Not mnuOption_ViewGrid.Checked

        If mnuOption_ViewGrid.Checked Then
            Me.Diagram.ShowGrid = True
        Else
            Me.Diagram.ShowGrid = False
        End If

    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        Dim lrPropertyGridForm As frmToolboxProperties

        If IsSomething(prApplication.GetToolboxForm(frmToolboxProperties.Name)) Then
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
            If Me.Diagram.Selection.Items.Count > 0 Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage
            End If
        End If

    End Sub

    Private Sub ToolStripMenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem10.Click

        Try
            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
            If IsSomething(lrPropertyGridForm) Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            lsMessage = "Error: frmDiagramORM.ToolStripMenuItem10_Click"
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub SubtypeConstraintsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemSubtypeConstraints.Click

        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
        Dim lrSubtypeInstance As FBM.SubtypeRelationshipInstance

        ToolStripMenuItemSubtypeConstraints.Checked = Not ToolStripMenuItemSubtypeConstraints.Checked

        If ToolStripMenuItemSubtypeConstraints.Checked Then
            For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
                For Each lrSubtypeInstance In lrEntityTypeInstance.SubtypeRelationship
                    If IsSomething(lrSubtypeInstance.Link) Then
                        lrSubtypeInstance.Link.Visible = True
                    End If
                Next
            Next
        Else
            For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
                For Each lrSubtypeInstance In lrEntityTypeInstance.SubtypeRelationship
                    If IsSomething(lrSubtypeInstance.Link) Then
                        lrSubtypeInstance.Link.Visible = False
                    End If
                Next
            Next
        End If

    End Sub

    Private Sub RoleConstraintsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemRoleConstraints.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

        ToolStripMenuItemRoleConstraints.Checked = Not ToolStripMenuItemRoleConstraints.Checked

        If ToolStripMenuItemRoleConstraints.Checked Then
            For Each lrRoleConstraintInstance In Me.zrPage.RoleConstraintInstance
                If IsSomething(lrRoleConstraintInstance.Shape) Then
                    lrRoleConstraintInstance.Shape.Visible = True
                End If

                For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                    If IsSomething(lrRoleConstraintRoleInstance.Link) Then
                        lrRoleConstraintRoleInstance.Link.Visible = True
                    End If
                Next
            Next
        Else
            For Each lrRoleConstraintInstance In Me.zrPage.RoleConstraintInstance
                If IsSomething(lrRoleConstraintInstance.Shape) Then
                    lrRoleConstraintInstance.Shape.Visible = False
                End If

                For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                    If IsSomething(lrRoleConstraintRoleInstance.Link) Then
                        lrRoleConstraintRoleInstance.Link.Visible = False
                    End If
                Next
            Next
        End If

    End Sub

    Private Sub ToolStripMenuItemRoleNames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemRoleNames.Click

        Dim lrFactTypeInstance As FBM.FactTypeInstance
        Dim lrRoleInstance As FBM.RoleInstance

        ToolStripMenuItemRoleNames.Checked = Not ToolStripMenuItemRoleNames.Checked

        If ToolStripMenuItemRoleNames.Checked Then
            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
                    If IsSomething(lrRoleInstance.RoleName.Shape) Then
                        lrRoleInstance.RoleName.Shape.Visible = True
                    End If
                Next
            Next
        Else
            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
                    If IsSomething(lrRoleInstance.RoleName.Shape) Then
                        lrRoleInstance.RoleName.Shape.Visible = False
                    End If
                Next
            Next
        End If


    End Sub

    Private Sub ToolStripMenuItemFactTypeReadings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemFactTypeReadings.Click

        Dim lrFactTypeInstance As FBM.FactTypeInstance

        ToolStripMenuItemFactTypeReadings.Checked = Not ToolStripMenuItemFactTypeReadings.Checked

        If ToolStripMenuItemFactTypeReadings.Checked Then
            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                If IsSomething(lrFactTypeInstance.FactTypeReadingShape.shape) And Not lrFactTypeInstance.isPreferredReferenceMode Then
                    lrFactTypeInstance.FactTypeReadingShape.shape.Visible = True
                End If
            Next
        Else
            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                If IsSomething(lrFactTypeInstance.FactTypeReadingShape.shape) Then
                    lrFactTypeInstance.FactTypeReadingShape.shape.Visible = False
                End If
            Next
        End If
    End Sub

    Private Sub ToolStripMenuItemFactTypeNames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemFactTypeNames.Click

        Dim lrFactTypeInstance As FBM.FactTypeInstance

        ToolStripMenuItemFactTypeNames.Checked = Not ToolStripMenuItemFactTypeNames.Checked

        If ToolStripMenuItemFactTypeNames.Checked Then
            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                If IsSomething(lrFactTypeInstance.FactTypeNameShape.Shape) And lrFactTypeInstance.ShowFactTypeName Then
                    lrFactTypeInstance.FactTypeNameShape.Visible = True
                End If
            Next
        Else
            For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
                If IsSomething(lrFactTypeInstance.FactTypeNameShape.Shape) Then
                    lrFactTypeInstance.FactTypeNameShape.Visible = False
                End If
            Next
        End If

    End Sub

    Private Sub ViewFactTablesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewFactTablesToolStripMenuItem.Click

        ViewFactTablesToolStripMenuItem.Checked = Not ViewFactTablesToolStripMenuItem.Checked

        Me.zrPage.ShowFacts = ViewFactTablesToolStripMenuItem.Checked

        '------------------------
        'Refresh the FactTables
        '------------------------
        Dim lrFactTypeInstance As FBM.FactTypeInstance
        For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance.FindAll(Function(x) x.isPreferredReferenceMode = False)
            lrFactTypeInstance.FactTable.TableShape.Visible = ViewFactTablesToolStripMenuItem.Checked
        Next

        Call Me.zrPage.MakeDirty()

    End Sub

    Private Sub ImportFactFromModelLevelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportFactFromModelLevelToolStripMenuItem.Click

        Dim liInd As Integer = 0
        Dim lrTableNode As New TableNode
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        Dim lrFactData As New FBM.FactData

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub

            If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.FactTable Then

                '------------------------
                'Add a row to the table
                '------------------------
                lrTableNode = Me.zrPage.SelectedObject(0).TableShape

                lrFactTypeInstance = lrTableNode.Tag.FactTypeInstance

                If lrFactTypeInstance Is Nothing Then Throw New Exception("No Fact Type Instance found for the Table.")

                Dim lrFactImportForm As New frmCRUDImportFact

                lrFactImportForm.zrFactTypeInstance = lrFactTypeInstance

                If lrFactImportForm.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Call Me.zrPage.Model.checkForErrors()
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

    Private Sub HideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideToolStripMenuItem.Click

        Try
            Dim lrTableNode As MindFusion.Diagramming.TableNode

            If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.FactTable Then
                '------------------
                'Get the TableNode
                '------------------
                lrTableNode = Me.zrPage.SelectedObject(0).TableShape
                lrTableNode.Visible = False
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ReoveFromPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReoveFromPageToolStripMenuItem.Click

        Try
            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

            Call lrRoleConstraintInstance.RemoveFromPage(True)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RemoveFromPageAndModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromPageAndModelToolStripMenuItem.Click

        Dim lrFrequencyConstraintInstance As FBM.FrequencyConstraint
        lrFrequencyConstraintInstance = Me.zrPage.SelectedObject(0)

        'Call lrFrequencyConstraintInstance.RemoveFromPage()
        Call lrFrequencyConstraintInstance.RoleConstraint.RemoveFromModel()

    End Sub

    Private Sub RemoveFromPageModelToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromPageModelToolStripMenuItem1.Click

        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

        Try
            lrRoleConstraintRoleInstance = Me.zrPage.SelectedObject(0)

            Using loWaitCursor As New WaitCursor
                If lrRoleConstraintRoleInstance.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then

                    Dim lrFactType As FBM.FactType = lrRoleConstraintRoleInstance.Role.Role.FactType
                    If lrFactType.IsObjectified And lrFactType.InternalUniquenessConstraint.Count = 1 Then
                        MsgBox("You cannot remove the last Internal Uniqueness Constraint from a Fact Type that is Objectified.")
                    Else
                        Call lrRoleConstraintRoleInstance.RoleConstraint.RoleConstraint.RemoveFromModel(False, True, True)
                    End If
                Else
                    Call lrRoleConstraintRoleInstance.RoleConstraint.RoleConstraint.RemoveFromModel(False, True, True)
                End If

                Call Me.zrPage.Model.Save()

            End Using

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub RemoveFromPageToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromPageToolStripMenuItem1.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        Try
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

            Call lrRoleConstraintInstance.RemoveFromPage(True)

            Call Me.EnableSaveButton()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RemoveFromPageModelToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromPageModelToolStripMenuItem2.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        Try
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

            If lrRoleConstraintInstance.RoleConstraint.RemoveFromModel Then

                Call Me.EnableSaveButton()

                Dim lrModelDictionaryForm As frmToolboxModelDictionary
                lrModelDictionaryForm = prApplication.GetToolboxForm(frmToolboxModelDictionary.Name)

                If IsSomething(lrModelDictionaryForm) Then
                    Call lrModelDictionaryForm.LoadToolboxModelDictionary(Me.zrPage.Language, True)
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

    Private Sub ContextMenuStrip_ExternalRoleConstraint_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ExternalRoleConstraint.Opening

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
        Dim lrRoleConstraint As FBM.RoleConstraint
        Dim loMenuOption As ToolStripItem

        Try
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)
            lrRoleConstraint = Me.zrPage.SelectedObject(0).RoleConstraint

            Me.ToolStripMenuItemRemoveAllArguments.Tag = lrRoleConstraint

            '--------------------------------------------------------------------
            'ModelErrors - Add menu items for the ModelErrors for the EntityType
            '--------------------------------------------------------------------
            Me.ToolStripMenuItemModelErrorsExternalRoleConstraint.DropDownItems.Clear()
            Dim lrModelError As FBM.ModelError
            If lrRoleConstraint.ModelError.Count > 0 Then
                Me.ToolStripMenuItemModelErrorsExternalRoleConstraint.Image = My.Resources.MenuImages.RainCloudRed16x16
                For Each lrModelError In lrRoleConstraint.ModelError
                    loMenuOption = Me.ToolStripMenuItemModelErrorsExternalRoleConstraint.DropDownItems.Add(lrModelError.Description)
                    loMenuOption.Image = My.Resources.MenuImages.RainCloudRed16x16
                Next
            Else
                Me.ToolStripMenuItemModelErrorsExternalRoleConstraint.Image = My.Resources.MenuImages.Cloud216x16
                loMenuOption = Me.ToolStripMenuItemModelErrorsExternalRoleConstraint.DropDownItems.Add("There are no Model Errors for this Constraint.")
                loMenuOption.Image = My.Resources.MenuImages.Cloud216x16
            End If

            '---------------------------------------------------------------------------------------------
            'Setup ability to remove Arguments from the RoleConstraint.
            '------------------------------------------------------------
            Me.ToolStripMenuItemRemoveArgument.DropDownItems.Clear()
            If lrRoleConstraint.Argument.Count > 0 Then
                Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
                For Each lrRoleConstraintArgument In lrRoleConstraint.Argument
                    loMenuOption = Me.ToolStripMenuItemRemoveArgument.DropDownItems.Add("&" & lrRoleConstraintArgument.SequenceNr.ToString)
                    loMenuOption.Tag = lrRoleConstraintArgument
                    AddHandler loMenuOption.Click, AddressOf Me.RemoveRoleConstraintArgument
                Next
            End If

            '--------------------------------------------------------------------------------------------
            'Show/Hide relevant sub-menus
            Select Case lrRoleConstraintInstance.RoleConstraintType
                Case Is = pcenumRoleConstraintType.CardinalityConstraint
                    '---------------------
                    'Not yet implemented
                    '---------------------
                Case Is = pcenumRoleConstraintType.EqualityConstraint
                    Me.ToolStripMenuItemChangeToUniquenessConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticUniqueness.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Visible = False

                Case Is = pcenumRoleConstraintType.ExclusionConstraint
                    Me.ToolStripMenuItemChangeToUniquenessConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticUniqueness.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToEqualityConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticEqualityConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToValueComparison.Visible = False
                Case Is = pcenumRoleConstraintType.ExclusiveORConstraint
                    Me.ToolStripMenuItemChangeToUniquenessConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticUniqueness.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Visible = False

                Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                    '-------------------------------------------------------
                    'Can't change this to any other type of RoleConstraint
                    ' except a Preferred External Uniqueness Constraint.
                    '-------------------------------------------------------
                    Me.ToolStripMenuItemChangeToEqualityConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToExclusiveConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToExclusiveOrConstranit.Visible = False
                    Me.ToolStripMenuItemChangeToInclusiveOrConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticEqualityConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticExclusiveConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticExclusiveOrConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticInclusiveOrConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToValueComparison.Visible = False

                Case Is = pcenumRoleConstraintType.FrequencyConstraint
                    '-------------------------------
                    'Has its own contextmenu
                Case Is = pcenumRoleConstraintType.InclusiveORConstraint
                    Me.ToolStripMenuItemChangeToUniquenessConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticUniqueness.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Visible = False

                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                    '----------------------------------------
                    'N/A Doesn't apply to this context menu
                    '----------------------------------------
                Case Is = pcenumRoleConstraintType.RingConstraint
                    '----------------------------------------
                    'N/A Doesn't apply to this context menu
                    '----------------------------------------
                Case Is = pcenumRoleConstraintType.SubsetConstraint
                    Me.ToolStripMenuItemChangeToUniquenessConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticUniqueness.Visible = False
                    Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Visible = False
                Case Else
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Click

        Dim lrExternalUniquenessConstraintInstance As FBM.tUniquenessConstraint

        lrExternalUniquenessConstraintInstance = Me.zrPage.SelectedObject(0)

        Call lrExternalUniquenessConstraintInstance.RoleConstraint.SetIsDeontic(True)
        Call lrExternalUniquenessConstraintInstance.SetIsPreferredIdentifier(True)
        lrExternalUniquenessConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.ExternalUniquenessConstraint)
        Call lrExternalUniquenessConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToUniquenessConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToUniquenessConstraint.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(False)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.ExternalUniquenessConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToUniquenessPreferredConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToUniquenessPreferredConstraint.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(False)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.ExternalUniquenessConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(True)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToDeonticUniqueness_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToDeonticUniqueness.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(True)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.ExternalUniquenessConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()


    End Sub

    Private Sub ToolStripMenuItemChangeToExclusiveConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToExclusiveConstraint.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(False)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.ExclusionConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToInclusiveOrConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToInclusiveOrConstraint.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(False)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.InclusiveORConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToExclusiveOrConstranit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToExclusiveOrConstranit.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(False)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.ExclusiveORConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToEqualityConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToEqualityConstraint.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(False)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.EqualityConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToDeonticExclusiveConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToDeonticExclusiveConstraint.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(True)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.ExclusionConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToDeonticInclusiveOrConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToDeonticInclusiveOrConstraint.Click


        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(True)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.InclusiveORConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToDeonticExclusiveOrConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToDeonticExclusiveOrConstraint.Click


        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(True)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.ExclusiveORConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemChangeToDeonticEqualityConstraint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemChangeToDeonticEqualityConstraint.Click


        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

        lrRoleConstraintInstance.RoleConstraint.SetIsDeontic(True)
        lrRoleConstraintInstance.RoleConstraint.SetRoleConstraintType(pcenumRoleConstraintType.EqualityConstraint)
        lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(False)
        Call lrRoleConstraintInstance.RefreshShape()

    End Sub

    Private Sub PropertiesToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem2.Click

        Try
            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
            If IsSomething(lrPropertyGridForm) Then
                If Me.Diagram.Selection.Items.Count > 0 Then
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(Me.Diagram.Selection.Items.Count - 1).Tag
                Else
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
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

    Private Sub createPropertyGraphSchemaPageFromCurrentPage(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '============================
        Dim lrPage As FBM.Page

        frmMain.Cursor = Cursors.WaitCursor
        Me.zrPage.Model.AllowCheckForErrors = False

        Me.CircularProgressBar.BringToFront()
        Me.CircularProgressBar.Value = 1
        Me.CircularProgressBar.Invalidate()
        Me.Invalidate()
        Me.BackgroundWorker.ReportProgress(0)

        lrPage = Me.zrPage.CreatePropertyGraphSchema(Me.BackgroundWorker)

        Me.CircularProgressBar.Value = 0
        Me.CircularProgressBar.Text = "0%"
        Me.CircularProgressBar.Invalidate()
        Me.CircularProgressBar.SendToBack()


        Me.zrPage.Model.AllowCheckForErrors = True
        frmMain.Cursor = Cursors.Default

        Dim lrEnterpriseView As tEnterpriseEnterpriseView = Nothing
        If IsSomething(lrPage) Then
            lrEnterpriseView = frmMain.zfrmModelExplorer.AddExistingPageToModel(lrPage, lrPage.Model, lrPage.Model.TreeNode, True)

            MsgBox("Added the new Property Graph Schema Page, '" & lrPage.Name & "', to the Model.")

            Dim loToolStripItem As ToolStripItem = CType(sender, ToolStripItem)

            If loToolStripItem.Tag = True Then
                Dim lrToolstripItem As New tDummyToolStripItem(lrEnterpriseView)
                Call Me.morphToERDiagram(lrToolstripItem, lrEnterpriseView)
            End If

        End If


    End Sub

    Private Sub PropertyGraphSchemaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertyGraphSchemaToolStripMenuItem.Click

        Call Me.createPropertyGraphSchemaPageFromCurrentPage(sender, e)

    End Sub

    Private Sub ContextMenuStrip_EntityType_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_EntityType.Opening

        Dim larPage_list As New List(Of FBM.Page)
        Dim lo_menu_option As ToolStripItem
        Dim lrModel As FBM.Model
        Dim lrEntityType As New FBM.EntityType
        Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            If Me.zrPage.Model.HasCoreModel Then
                Me.ERDiagramToolStripMenu.Visible = True
            End If

            lrEntityType = Me.zrPage.SelectedObject(0).EntityType
            lrEntityTypeInstance = Me.zrPage.SelectedObject(0)
            lrModel = lrEntityType.Model

            If lrEntityType.HasSimpleReferenceScheme And Trim(lrEntityType.ReferenceMode) <> "" Then
                If lrEntityTypeInstance.ExpandReferenceMode = True Then
                    Me.ExpandTheReferenceSchemeToolStripMenuItem.Visible = False
                    Me.HideTheReferenceSchemeToolStripMenuItem.Visible = True
                Else
                    Me.ExpandTheReferenceSchemeToolStripMenuItem.Visible = True
                    Me.HideTheReferenceSchemeToolStripMenuItem.Visible = False
                End If
            End If

            '--------------------------------------------------------------------
            'ModelErrors - Add menu items for the ModelErrors for the EntityType
            '--------------------------------------------------------------------
            Me.ToolStripMenuItemEntityTypeModelErrors.DropDownItems.Clear()
            Dim lrModelError As FBM.ModelError
            If lrEntityType.ModelError.Count > 0 Then
                For Each lrModelError In lrEntityType.ModelError
                    lo_menu_option = Me.ToolStripMenuItemEntityTypeModelErrors.DropDownItems.Add(lrModelError.Description)
                    lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
                Next
                ToolStripMenuItemEntityTypeModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
            Else
                Me.ToolStripMenuItemEntityTypeModelErrors.Image = My.Resources.MenuImages.Cloud216x16
                lo_menu_option = Me.ToolStripMenuItemEntityTypeModelErrors.DropDownItems.Add("There are no Model Errors for this Entity Type.")
                lo_menu_option.Image = My.Resources.MenuImages.Cloud216x16
            End If

            If lrEntityType.HasSimpleReferenceScheme Then
                Dim lrSupertype As FBM.EntityType = lrEntityType.GetTopmostSupertype
                If lrSupertype.ReferenceModeValueType.ModelError.Count > 0 Then
                    For Each lrModelError In lrSupertype.ReferenceModeValueType.ModelError
                        lo_menu_option = Me.ToolStripMenuItemEntityTypeModelErrors.DropDownItems.Add(lrModelError.Description)
                        lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
                    Next
                    ToolStripMenuItemEntityTypeModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
                End If
            End If

            '---------------------------------------------------------------------------------------------
            'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
            '  shape, and to/into another diagram starts at the MorphVector.
            '---------------------------------------------------------------------------------------------
            Me.MorphVector.Clear()
            Me.MorphStepTimer = New System.Windows.Forms.Timer 'Kind of stops degradation of the timer interval. Not sure why the timer degrades.
            Me.MorphStepTimer.Interval = 17
            Me.MorphTimer.Stop()
            Me.MorphVector.Add(New tMorphVector(lrEntityTypeInstance.X, lrEntityTypeInstance.Y, 0, 0, 40))

            Me.ORMDiagramToolStripMenuItem.DropDownItems.Clear()
            Me.ERDiagramToolStripMenu.DropDownItems.Clear()
            Me.ToolStripMenuItemPropertyGraphSchema.DropDownItems.Clear()

            '----------------------------------------------------------------
            'Clear the list of ORMDiagrams that may relate to the ValueType
            '----------------------------------------------------------------
            Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram.DropDownItems.Clear()

            '-----------------------------------------------------------------------------
            'Clear the list of StateTransitionDiagrams that may relate to the ValueType
            '-----------------------------------------------------------------------------
            Me.ToolStripMenuItem_StateTransitionDiagram.DropDownItems.Clear()

            '--------------------------------------------------------
            'Load the ORMDiagrams that relate to the EntityType
            '  as selectable menuOptions
            '--------------------------------------------------------        
            larPage_list = prApplication.CMML.GetORMDiagramPagesForEntityType(lrEntityType)

            For Each lrPage In larPage_list
                '----------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, 
                '  they will not be in the TreeView and so a menuOption
                '  is now added for those hidden Pages.
                '----------------------------------------------------------
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                           lrPage,
                                                           lrModel.ModelId,
                                                           pcenumLanguage.ORMModel,
                                                           Nothing,
                                                           lrPage.PageId)

                lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)

                If IsSomething(lrEnterpriseView) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    lo_menu_option = Me.ORMDiagramToolStripMenuItem.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ORM16x16)
                    lo_menu_option.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    AddHandler lo_menu_option.Click, AddressOf Me.morphToORMDiagram
                End If
            Next

            '====================================================================================
            'Load the ERDiagrams that relate to the EntityType as selectable menuOptions
            '--------------------------------------------------------        
            larPage_list = prApplication.CMML.GetERDiagramPagesForEntityType(lrEntityType)

            For Each lrPage In larPage_list
                '----------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, 
                '  they will not be in the TreeView and so a menuOption
                '  is now added for those hidden Pages.
                '----------------------------------------------------------
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                           lrPage,
                                                           lrModel.ModelId,
                                                           pcenumLanguage.ORMModel,
                                                           Nothing,
                                                           lrPage.PageId)

                lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId) ' AddressOf lrEnterpriseView.Equals)

                If IsSomething(lrEnterpriseView) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    lo_menu_option = Me.ERDiagramToolStripMenu.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ERD16x16)
                    lo_menu_option.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    AddHandler lo_menu_option.Click, AddressOf Me.MorphToERDiagram
                End If

                Me.ERDiagramToolStripMenu.Enabled = True
            Next

            If larPage_list.Count = 0 Then
                '----------------------------------------------------------------------------------
                'Provide and option for the User to create a PGS Page for the current Page.
                Dim lsMessage As String = "Create an Entity Relationship Diagram for this current Page. No ERD Page currently exists for the model element you have selected."
                lo_menu_option = Me.ERDiagramToolStripMenu.DropDownItems.Add(lsMessage, My.Resources.MenuImages.ERD16x16)
                lo_menu_option.Tag = True
                AddHandler lo_menu_option.Click, AddressOf Me.createEntityRelationshipDiagramPageForCurrentPage
            End If
            '======================================================================================

            '==================================================================================================
            'Load the Property Graph Schema Diagrams that relate to the EntityType, as selectable menuOptions
            '--------------------------------------------------------        
            larPage_list = prApplication.CMML.getPGSDiagramPagesForEntityType(lrEntityType)

            For Each lrPage In larPage_list
                '----------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, 
                '  they will not be in the TreeView and so a menuOption
                '  is now added for those hidden Pages.
                '----------------------------------------------------------
                Dim lrEnterpriseView As tEnterpriseEnterpriseView

                lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId) ' AddressOf lrEnterpriseView.Equals)

                If IsSomething(lrEnterpriseView) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    lo_menu_option = Me.ToolStripMenuItemPropertyGraphSchema.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.PGS16x16)
                    lo_menu_option.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    AddHandler lo_menu_option.Click, AddressOf Me.MorphToPGSDiagram
                End If

                Me.ERDiagramToolStripMenu.Enabled = True
            Next

            If larPage_list.Count = 0 Then
                '----------------------------------------------------------------------------------
                'Provide and option for the User to create a PGS Page for the current Page.
                Dim lsMessage As String = "Create a Property Graph Schema for this current Page. No PGS Page currently exists for the model element you have selected."
                lo_menu_option = Me.ToolStripMenuItemPropertyGraphSchema.DropDownItems.Add(lsMessage, My.Resources.MenuImages.PGS16x16)
                lo_menu_option.Tag = True
                AddHandler lo_menu_option.Click, AddressOf Me.createPropertyGraphSchemaPageFromCurrentPage
            End If

            '======================================================================================


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ContextMenuStrip_FactTable_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_FactTable.Opening

        '------------------------------------------------------------------------------------
        'NB See Diagram.CellClicked for adding of ModelErrors to the FactTable context menu
        '------------------------------------------------------------------------------------
        Dim lrFact As FBM.Fact
        Dim lrFactTable As FBM.FactTable
        Dim lo_menu_option As ToolStripItem

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            lrFactTable = Me.zrPage.SelectedObject(0)

            Dim larFactsWithErrors As New List(Of FBM.Fact)
            larFactsWithErrors = lrFactTable.FactTypeInstance.FactType.Fact.FindAll(Function(x) x.ModelError.Count > 0)
            If larFactsWithErrors.Count > 0 Then
                Me.ToolStripMenuItemFactModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
                Me.ToolStripMenuItemFactModelErrors.DropDownItems.Clear()
                For Each lrFact In lrFactTable.FactTypeInstance.FactType.Fact
                    For Each lrModelError In lrFact.ModelError
                        lo_menu_option = Me.ToolStripMenuItemFactModelErrors.DropDownItems.Add(lrModelError.Description)
                        lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
                    Next
                Next
            Else
                Me.ToolStripMenuItemFactModelErrors.Image = My.Resources.MenuImages.Cloud216x16
                Me.ToolStripMenuItemFactModelErrors.DropDownItems.Clear()
                lo_menu_option = Me.ToolStripMenuItemFactModelErrors.DropDownItems.Add("There are no Model Errors for the Facts of this Fact Type.")
                lo_menu_option.Image = My.Resources.MenuImages.Cloud216x16
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub SetNameFromHostingObjectTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetNameFromHostingObjectTypeToolStripMenuItem.Click

        If Me.zrPage.SelectedObject.Count <> 1 Then
            Exit Sub
        End If

        Dim lrRoleInstance As FBM.RoleInstance

        lrRoleInstance = Me.zrPage.SelectedObject(0)

        lrRoleInstance.Role.SetName(lrRoleInstance.JoinedORMObject.Name, True)

    End Sub

    Private Sub HideMeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideMeToolStripMenuItem.Click

        prApplication.Brain.send_data("Goodbye")
        Me.Animator1.Visible = False

    End Sub

    Private Sub createEntityRelationshipDiagramPageForCurrentPage(ByVal sender As Object, ByVal e As EventArgs)

        Dim lrPage As FBM.Page

        frmMain.Cursor = Cursors.WaitCursor
        Me.zrPage.Model.AllowCheckForErrors = False

        Me.CircularProgressBar.BringToFront()
        Me.CircularProgressBar.Value = 1
        Me.CircularProgressBar.Invalidate()
        Me.Invalidate()
        Me.BackgroundWorker.ReportProgress(0)

        lrPage = Me.zrPage.CreateEntityRelationshipDiagram(Me.BackgroundWorker)

        Me.CircularProgressBar.Value = 0
        Me.CircularProgressBar.Text = "0%"
        Me.CircularProgressBar.Invalidate()
        Me.CircularProgressBar.SendToBack()

        Me.zrPage.Model.AllowCheckForErrors = True
        frmMain.Cursor = Cursors.Default

        Dim lrEnterpriseView As tEnterpriseEnterpriseView = Nothing
        If IsSomething(lrPage) Then
            lrEnterpriseView = frmMain.zfrmModelExplorer.AddExistingPageToModel(lrPage, lrPage.Model, lrPage.Model.TreeNode, True)

            MsgBox("Added the new Entity Relationship Diagram Page, '" & lrPage.Name & "' to the Model.")

            Dim loToolStripItem As ToolStripItem = CType(sender, ToolStripItem)

            If loToolStripItem.Tag = True Then
                Dim lrToolstripItem As New tDummyToolStripItem(lrEnterpriseView)
                Call Me.morphToERDiagram(lrToolstripItem, lrEnterpriseView)
            End If

        End If

    End Sub

    Private Sub EntityRelationshipDiagramToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EntityRelationshipDiagramToolStripMenuItem.Click

        Call Me.createEntityRelationshipDiagramPageForCurrentPage(sender, e)

    End Sub

    Private Sub HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem.Click

        Dim lrRoleInstance As FBM.RoleInstance
        Dim lrFactTypeInstance As FBM.FactTypeInstance

        For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
            For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
                If IsSomething(lrRoleInstance.RoleName.Shape) Then
                    lrRoleInstance.RoleName.Shape.Visible = False
                End If

                If IsSomething(lrFactTypeInstance.FactTypeReadingShape.shape) Then
                    lrFactTypeInstance.FactTypeReadingShape.shape.Visible = False
                End If

                If IsSomething(lrFactTypeInstance.FactTypeNameShape.Shape) Then
                    lrFactTypeInstance.FactTypeNameShape.Visible = False
                End If
            Next
        Next

        ToolStripMenuItemRoleNames.Checked = False
        ToolStripMenuItemFactTypeReadings.Checked = False
        ToolStripMenuItemFactTypeNames.Checked = False

    End Sub

    Public Sub ShowBriana()
        Me.Briana.Visible = True
    End Sub

    Private Sub ShowSpyInformationForModelObject(ByRef arModelObject As FBM.ModelObject)

        Me.Diagram.Links.Clear()
        Me.Diagram.Nodes.Clear()
        Me.Diagram.Invalidate()

        prApplication.WorkingModel = arModelObject.Model

        Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(arModelObject.Model, "123", "Diagram Spy", pcenumLanguage.ORMModel)
        Dim loPt As New PointF(100, 100)

        lrDiagramSpyPage.Form = New Windows.Forms.Form
        lrDiagramSpyPage.Form = Me
        lrDiagramSpyPage.ReferencedForm = Me
        lrDiagramSpyPage.Diagram = Me.Diagram
        lrDiagramSpyPage.DiagramView = Me.DiagramView

        Me.zrPage = lrDiagramSpyPage

        Select Case arModelObject.ConceptType
            Case Is = pcenumConceptType.ValueType
                Dim lrValueType As FBM.ValueType
                lrValueType = arModelObject
                Call Me.zrPage.DropValueTypeAtPoint(lrValueType, loPt)
                Call Me.LoadAssociatedFactTypes(lrValueType)
            Case Is = pcenumConceptType.EntityType
                Dim lrEntityType As FBM.EntityType
                lrEntityType = arModelObject
                Call Me.zrPage.DropEntityTypeAtPoint(lrEntityType, loPt)
                Call Me.LoadAssociatedFactTypes(lrEntityType)
            Case Is = pcenumConceptType.FactType
                Dim lrFactType As FBM.FactType
                lrFactType = arModelObject
                Call Me.zrPage.DropFactTypeAtPoint(lrFactType, loPt, False)
            Case Is = pcenumConceptType.RoleConstraint
                Dim lrRoleConstraint As FBM.RoleConstraint
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                lrRoleConstraint = arModelObject

                Select Case lrRoleConstraint.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                        lrRoleConstraintInstance = Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                    Case Is = pcenumRoleConstraintType.RingConstraint,
                              pcenumRoleConstraintType.EqualityConstraint,
                              pcenumRoleConstraintType.ExternalUniquenessConstraint,
                              pcenumRoleConstraintType.ExclusiveORConstraint,
                              pcenumRoleConstraintType.ExclusionConstraint,
                              pcenumRoleConstraintType.SubsetConstraint
                        lrRoleConstraintInstance = Me.zrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                    Case Is = pcenumRoleConstraintType.FrequencyConstraint
                        Call Me.zrPage.DropFrequencyConstraintAtPoint(lrRoleConstraint, loPt)
                End Select
        End Select

        Call Me.AutoLayout()

    End Sub

    Public Function GetRoleConstraintCurrentlyCreatingArgument() As FBM.RoleConstraintInstance

        Try

            If Me.zrPage.RoleConstraintInstance.FindAll(Function(x) x.CreatingArgument = True).Count = 0 Then
                Throw New Exception("There is not RoleConstraint for which an Argument is currently being created.")
            Else
                Return Me.zrPage.RoleConstraintInstance.Find(Function(x) x.CreatingArgument = True)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return Nothing
        End Try

    End Function

    Public Sub HideBriana()
        Me.Briana.Visible = False
    End Sub

    Private Sub RemoveLinksToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveLinksToolStripMenuItem.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        Try
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

            lrRoleConstraintInstance.RoleConstraint.RemoveRoleConstraintRoles()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RemoveSubtypeRelationshipFromTheModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveSubtypeRelationshipFromTheModelToolStripMenuItem.Click


        Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance

        Try
            lrSubtypeRelationshipInstance = Me.zrPage.SelectedObject(0)

            Call lrSubtypeRelationshipInstance.SubtypeRelationship.RemoveFromModel()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub RemovefromPageAndModelToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RemovefromPageAndModelToolStripMenuItem1.Click

        Try
            Dim lrRoleInstance As FBM.RoleInstance

            lrRoleInstance = Me.zrPage.SelectedObject(0)

            Me.Cursor = Cursors.WaitCursor
            Call lrRoleInstance.FactType.FactType.RemoveRole(lrRoleInstance.Role, True, True)
            Me.Cursor = Cursors.Default

            '-----------------------------------------------------------------------------------------
            'The FactType/FactTypeInstance may well have just been removed completely from the Model
            '  so if frmToolboxModelDictionary is loaded then reset the zrLoadedModel to Nothing 
            '  so that when the user moves the mouse over the Page the ModelDictionary is reloaded.
            '-----------------------------------------------------------------------------------------
            Dim child As New frmToolboxModelDictionary
            If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count >= 1 Then
                child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
                child.zrLoadedModel = Nothing
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ContextMenuStrip_FrequencyConstraint_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_FrequencyConstraint.Opening

        Dim lrRoleConstraint As FBM.RoleConstraint
        Dim loMenuOption As ToolStripItem

        lrRoleConstraint = Me.zrPage.SelectedObject(0).RoleConstraint

        '--------------------------------------------------------------------
        'ModelErrors - Add menu items for the ModelErrors for the EntityType
        '--------------------------------------------------------------------
        Me.ToolStripMenuItemModelErrorsFrequencyConstraint.DropDownItems.Clear()
        Dim lrModelError As FBM.ModelError
        If lrRoleConstraint.ModelError.Count > 0 Then
            Me.ToolStripMenuItemModelErrorsFrequencyConstraint.Image = My.Resources.MenuImages.RainCloudRed16x16
            For Each lrModelError In lrRoleConstraint.ModelError
                loMenuOption = Me.ToolStripMenuItemModelErrorsFrequencyConstraint.DropDownItems.Add(lrModelError.Description)
                loMenuOption.Image = My.Resources.MenuImages.RainCloudRed16x16
            Next
        Else
            Me.ToolStripMenuItemModelErrorsFrequencyConstraint.Image = My.Resources.MenuImages.Cloud216x16
            loMenuOption = Me.ToolStripMenuItemModelErrorsFrequencyConstraint.DropDownItems.Add("There are no Model Errors for this Frequency Constraint.")
            loMenuOption.Image = My.Resources.MenuImages.Cloud216x16
        End If

    End Sub

    Private Sub DeleteRowFactFromPageAndModelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteRowFactFromPageAndModelToolStripMenuItem.Click

        Dim lrTableNode As New TableNode
        Dim lrFactTable As New FBM.FactTable
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        Dim lrFactInstance As New FBM.FactInstance

        '------------------------------------------------------------------------------------------------------------------
        '20170119-VM-Need to test this code. If the code works fine, remove this message.
        '  NB Code copied/adapted from the [Delete Fact From Page] menu option.

        If Me.zrPage.SelectedObject(0).ConceptType = pcenumConceptType.FactTable Then

            lrTableNode = Me.zrPage.SelectedObject(0).TableShape
            lrFactTable = lrTableNode.Tag

            If lrFactTable.SelectedRow > 0 Then
                lrFactTypeInstance = lrFactTable.FactTypeInstance
                lrFactInstance = lrFactTypeInstance.Fact(lrFactTable.SelectedRow - 1)

                lrFactTable.TableShape.DeleteRow(lrFactTable.SelectedRow - 1)
                lrFactTypeInstance.RemoveFact(lrFactInstance)
                lrFactTypeInstance.FactType.RemoveFactById(lrFactInstance.Fact)
            End If
        End If

    End Sub

    Private Sub ContextMenuStrip_RingConstraint_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_RingConstraint.Opening

        Dim lrRoleConstraint As FBM.RoleConstraint
        Dim loMenuOption As ToolStripItem

        lrRoleConstraint = Me.zrPage.SelectedObject(0).RoleConstraint

        '--------------------------------------------------------------------
        'ModelErrors - Add menu items for the ModelErrors for the EntityType
        '--------------------------------------------------------------------
        Me.ToolStripMenuItemModelErrorsRingConstraint.DropDownItems.Clear()
        Dim lrModelError As FBM.ModelError
        If lrRoleConstraint.ModelError.Count > 0 Then
            Me.ToolStripMenuItemModelErrorsRingConstraint.Image = My.Resources.MenuImages.RainCloudRed16x16
            For Each lrModelError In lrRoleConstraint.ModelError
                loMenuOption = Me.ToolStripMenuItemModelErrorsRingConstraint.DropDownItems.Add(lrModelError.Description)
                loMenuOption.Image = My.Resources.MenuImages.RainCloudRed16x16
            Next
        Else
            Me.ToolStripMenuItemModelErrorsRingConstraint.Image = My.Resources.MenuImages.Cloud216x16
            loMenuOption = Me.ToolStripMenuItemModelErrorsRingConstraint.DropDownItems.Add("There are no Model Errors for this Ring Constraint.")
            loMenuOption.Image = My.Resources.MenuImages.Cloud216x16
        End If

    End Sub

    Private Sub ToolStripMenuItem17_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem17.Click

        '-------------------------------------------------------------------------
        'NB Dummy menu item. See ContextMenuStrip_ExternalRoleConstraint_Opening
        '  This menu item is dynamically removed (cleared) and replaced by
        '  a set of menu items, one for each Argument of the RoleConstraint.
        '-------------------------------------------------------------------------

    End Sub

    Private Sub BackgroundWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker.ProgressChanged

        Me.CircularProgressBar.Value = 0
        Me.CircularProgressBar.Value = e.ProgressPercentage
        Me.CircularProgressBar.Text = e.ProgressPercentage & "%"
        Me.CircularProgressBar.Invalidate()
        Me.Invalidate()
        Me.Refresh()

    End Sub

    Private Sub BackgroundWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker.RunWorkerCompleted

        Call Me.CircularProgressBar.SendToBack()

    End Sub

    Private Sub RedoJoinPathToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedoJoinPathToolStripMenuItem.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        Try
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

            For Each lrRoleConstraintArgument In lrRoleConstraintInstance.RoleConstraint.Argument.FindAll(Function(x) x.JoinPath.RolePath.Count = 0)
                Call lrRoleConstraintArgument.ConstructJoinPathForAssociatedRoleConstraintRoles()
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ContextMenuStrip_SubtypeRelationship_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_SubtypeRelationship.Opening

        Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance

        Try
            lrSubtypeRelationshipInstance = Me.zrPage.SelectedObject(0)

            If lrSubtypeRelationshipInstance.FactType.IsDisplayedAssociated Then
                If lrSubtypeRelationshipInstance.FactType.RoleGroup(0).Shape.Visible = True Then
                    Me.ToolStripMenuItemSubtypeShowCorrespondingFactType.Visible = False
                    Me.HideCorrespondingFactTypeToolStripMenuItem.Visible = True
                Else
                    Me.ToolStripMenuItemSubtypeShowCorrespondingFactType.Visible = True
                    Me.HideCorrespondingFactTypeToolStripMenuItem.Visible = False
                End If
            Else
                Me.ToolStripMenuItemSubtypeShowCorrespondingFactType.Visible = True
                Me.HideCorrespondingFactTypeToolStripMenuItem.Visible = False
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemSubtypeShowCorrespondingFactType_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSubtypeShowCorrespondingFactType.Click

        Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance

        Try
            lrSubtypeRelationshipInstance = Me.zrPage.SelectedObject(0)

            If lrSubtypeRelationshipInstance.FactType.IsDisplayedAssociated Then
                Call lrSubtypeRelationshipInstance.FactType.Show()
            Else
                Call lrSubtypeRelationshipInstance.FactType.DisplayAndAssociate(False, False, True)
                Call lrSubtypeRelationshipInstance.FactType.MoveToBetweenAssociatedModelObjects(False)
                For Each lrRoleConstraint In lrSubtypeRelationshipInstance.FactType.InternalUniquenessConstraint
                    Call lrRoleConstraint.DisplayAndAssociate()
                Next
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub HideToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HideToolStripMenuItem1.Click

        Me.MenuItemHelpTips.Checked = False
        Me.LabelHelp.Visible = False

    End Sub

    Private Sub ToolStripMenuItem13_Click_1(sender As Object, e As EventArgs) Handles ToolStripMenuItem13.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        Try
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

            If lrRoleConstraintInstance.RoleConstraint.RemoveFromModel Then

                Call Me.EnableSaveButton()

                Dim lrModelDictionaryForm As frmToolboxModelDictionary
                lrModelDictionaryForm = prApplication.GetToolboxForm(frmToolboxModelDictionary.Name)

                If IsSomething(lrModelDictionaryForm) Then
                    Call lrModelDictionaryForm.LoadToolboxModelDictionary(Me.zrPage.Language, True)
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

    Private Sub ToolStripMenuItem9_Click_1(sender As Object, e As EventArgs) Handles ToolStripMenuItem9.Click

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

        Try
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

            Call lrRoleConstraintInstance.RemoveFromPage(True)

            Call Me.EnableSaveButton()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub RemoveAllArgumentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemRemoveAllArguments.Click

        Dim lrRoleConstraint As FBM.RoleConstraint

        lrRoleConstraint = Me.ToolStripMenuItemRemoveAllArguments.Tag

        Dim liInd As Integer = 1
        For Each lrArgument In lrRoleConstraint.Argument.ToArray
            Call lrRoleConstraint.RemoveArgument(lrArgument)
            liInd += 1
        Next

        For Each lrRoleInstance In Me.zrPage.RoleInstance
            lrRoleInstance.Shape.Text = ""
        Next

        Me.zrPage.Model.MakeDirty(False, True)

        Call Me.ResetNodeAndLinkColors()

    End Sub

    Private Sub ShowInDiagramSpyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowInDiagramSpyToolStripMenuItem.Click

        Try
            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.zrPage.Model, "123", "Diagram Spy", pcenumLanguage.ORMModel)

            lrEntityTypeInstance = Me.zrPage.SelectedObject(0)
            Call frmMain.LoadDiagramSpy(lrDiagramSpyPage, lrEntityTypeInstance.EntityType)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub HideCorrespondingFactTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideCorrespondingFactTypeToolStripMenuItem.Click

        Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance

        Try
            lrSubtypeRelationshipInstance = Me.zrPage.SelectedObject(0)

            If lrSubtypeRelationshipInstance.FactType.Shape IsNot Nothing Then
                lrSubtypeRelationshipInstance.FactType.Hide()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ExpandTheReferenceSchemeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExpandTheReferenceSchemeToolStripMenuItem.Click

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            lrEntityTypeInstance = Me.zrPage.SelectedObject(0)

            If lrEntityTypeInstance.ExpandReferenceMode Then
                Exit Sub
            Else
                Call lrEntityTypeInstance.ExpandTheReferenceScheme()
                lrEntityTypeInstance.ExpandReferenceMode = True
                Me.ExpandTheReferenceSchemeToolStripMenuItem.Visible = False
                Me.HideTheReferenceSchemeToolStripMenuItem.Visible = True
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub HideTheReferenceSchemeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideTheReferenceSchemeToolStripMenuItem.Click

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            lrEntityTypeInstance = Me.zrPage.SelectedObject(0)

            If lrEntityTypeInstance.ExpandReferenceMode = False Then
                Exit Sub
            Else
                Call lrEntityTypeInstance.HideTheReferenceScheme()
                lrEntityTypeInstance.ExpandReferenceMode = False
                Me.ExpandTheReferenceSchemeToolStripMenuItem.Visible = True
                Me.HideTheReferenceSchemeToolStripMenuItem.Visible = False
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ShowInModelDictionaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowInModelDictionaryToolStripMenuItem.Click

        Dim lfrmModelDictionary As New frmToolboxModelDictionary

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            lrEntityTypeInstance = Me.zrPage.SelectedObject(0)

            If prApplication.RightToolboxForms.FindAll(AddressOf lfrmModelDictionary.EqualsByName).Count = 0 Then
                Call frmMain.LoadToolboxModelDictionary()
            End If

            lfrmModelDictionary = prApplication.RightToolboxForms.Find(AddressOf lfrmModelDictionary.EqualsByName)

            Call lfrmModelDictionary.FindTreeNode(lrEntityTypeInstance.Id)

            lfrmModelDictionary.Show()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ShowInModelDictionaryToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ShowInModelDictionaryToolStripMenuItem1.Click

        Dim lfrmModelDictionary As New frmToolboxModelDictionary

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            Dim lrValueTypeInstance As FBM.ValueTypeInstance
            lrValueTypeInstance = Me.zrPage.SelectedObject(0)

            If prApplication.RightToolboxForms.FindAll(AddressOf lfrmModelDictionary.EqualsByName).Count = 0 Then
                Call frmMain.LoadToolboxModelDictionary()
            End If

            lfrmModelDictionary = prApplication.RightToolboxForms.Find(AddressOf lfrmModelDictionary.EqualsByName)

            Call lfrmModelDictionary.FindTreeNode(lrValueTypeInstance.Id)

            lfrmModelDictionary.Show()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ShowInModelDictionaryToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ShowInModelDictionaryToolStripMenuItem2.Click

        Dim lfrmModelDictionary As New frmToolboxModelDictionary

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            lrFactTypeInstance = Me.zrPage.SelectedObject(0)

            If prApplication.RightToolboxForms.FindAll(AddressOf lfrmModelDictionary.EqualsByName).Count = 0 Then
                Call frmMain.LoadToolboxModelDictionary()
            End If

            lfrmModelDictionary = prApplication.RightToolboxForms.Find(AddressOf lfrmModelDictionary.EqualsByName)

            Call lfrmModelDictionary.FindTreeNode(lrFactTypeInstance.Id)

            lfrmModelDictionary.Show()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ShowInModelDictionaryToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ShowInModelDictionaryToolStripMenuItem3.Click

        Dim lfrmModelDictionary As New frmToolboxModelDictionary

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
            lrRoleConstraintInstance = Me.zrPage.SelectedObject(0)

            If prApplication.RightToolboxForms.FindAll(AddressOf lfrmModelDictionary.EqualsByName).Count = 0 Then
                Call frmMain.LoadToolboxModelDictionary()
            End If

            lfrmModelDictionary = prApplication.RightToolboxForms.Find(AddressOf lfrmModelDictionary.EqualsByName)

            Call lfrmModelDictionary.FindTreeNode(lrRoleConstraintInstance.Id)

            lfrmModelDictionary.Show()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ShowInModelDictionaryToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ShowInModelDictionaryToolStripMenuItem4.Click

        Dim lfrmModelDictionary As New frmToolboxModelDictionary

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
            lrRoleConstraintRoleInstance = Me.zrPage.SelectedObject(0)

            If prApplication.RightToolboxForms.FindAll(AddressOf lfrmModelDictionary.EqualsByName).Count = 0 Then
                Call frmMain.LoadToolboxModelDictionary()
            End If

            lfrmModelDictionary = prApplication.RightToolboxForms.Find(AddressOf lfrmModelDictionary.EqualsByName)

            Call lfrmModelDictionary.FindTreeNode(lrRoleConstraintRoleInstance.RoleConstraint.Id)

            lfrmModelDictionary.Show()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub PropertiesToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem3.Click

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
            lrRoleConstraintRoleInstance = Me.zrPage.SelectedObject(0)

            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
            If IsSomething(lrPropertyGridForm) Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintRoleInstance.RoleConstraint
                lrPropertyGridForm.Show()
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemExtendToCoverAllRolesInTheFactType_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemExtendToCoverAllRolesInTheFactType.Click

        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

        Try
            lrRoleConstraintRoleInstance = Me.zrPage.SelectedObject(0)
            If lrRoleConstraintRoleInstance Is Nothing Then Exit Sub 'CodeSafe

            Dim lrUncoveredRole As FBM.Role
            Dim lrRoleConstraint As FBM.RoleConstraint = lrRoleConstraintRoleInstance.RoleConstraint.RoleConstraint
            Dim lrFactType As FBM.FactType = lrRoleConstraint.Role(0).FactType

            lrUncoveredRole = lrFactType.getRoleNotCoveredByInternalUniquenessConstraint(lrRoleConstraint)

            Dim lrRoleConstraintRole As New FBM.RoleConstraintRole(lrUncoveredRole, lrRoleConstraint)

            Call lrRoleConstraint.AddRoleConstraintRole(lrRoleConstraintRole)


        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

        End Try

    End Sub

    Private Sub ContextMenuStrip_InternalUniquenessConstraint_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_InternalUniquenessConstraint.Opening

        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

        Try
            lrRoleConstraintRoleInstance = Me.zrPage.SelectedObject(0)
            If lrRoleConstraintRoleInstance Is Nothing Then Exit Sub 'CodeSafe

            'ToDo:Removed for v4.1. Not complete/tested.
            'If lrRoleConstraintRoleInstance.Role.FactType.Arity > lrRoleConstraintRoleInstance.RoleConstraint.RoleConstraintRole.Count Then
            '    Me.ToolStripMenuItemExtendToCoverAllRolesInTheFactType.Visible = (lrRoleConstraintRoleInstance.Role.FactType.InternalUniquenessConstraint.Count = 1)        
            'Else
            Me.ToolStripMenuItemExtendToCoverAllRolesInTheFactType.Visible = False
            'End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

        End Try

    End Sub


    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click

        Me.Close()

    End Sub

    Private Sub CloseAllButThisPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllButThisPageToolStripMenuItem.Click

        For Each lrForm In prApplication.ActivePages.ToArray

            Select Case lrForm.GetType
                Case Is = GetType(frmDiagramORM), GetType(frmDiagramERD), GetType(frmDiagramPGS)
                    If lrForm IsNot Me Then
                        Call lrForm.Close()
                    End If
            End Select
        Next

    End Sub

    Private Sub zrPage_PageUpdated() Handles zrPage.PageUpdated

        Call Me.EnableSaveButton()

    End Sub

    Private Sub Diagram_DrawLink(sender As Object, e As DrawLinkEventArgs) Handles Diagram.DrawLink

    End Sub

    Private Sub ToolStripMenuItemModelNoteRemoveFromPageAndModel_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemModelNoteRemoveFromPageAndModel.Click

        Dim lrModelNoteInstance As FBM.ModelNoteInstance
        Dim lrModelNote As FBM.ModelNote

        lrModelNote = Me.zrPage.SelectedObject(0).ModelNote
        lrModelNoteInstance = Me.zrPage.SelectedObject(0)

        Call lrModelNote.RemoveFromModel(True, False, True)

    End Sub

    Private Sub DiagramView_CreateEditControl(sender As Object, e As InPlaceEditEventArgs) Handles DiagramView.CreateEditControl

    End Sub

    Private Sub ContextMenuStrip_Diagram_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_Diagram.Opening

        Dim lrPage As New FBM.Page
        Dim loForm As Form

        loForm = frmMain.DockPanel.ActiveDocument

        If frmMain.PageDataExistsInClipboard(lrPage) Then
            '------------------------------------
            'Page data exists in the clipboard.
            '------------------------------------
            If loForm.Name = frmMain.zrORMModel_view.Name Then
                '------------------------------------------------------------------------
                'Current form is an ORM Diagram form, so can at least paste to the Page
                '  if it isn't the same page the data was copied from
                '------------------------------------------------------------------------
                Dim loORMDiagramForm As frmDiagramORM
                loORMDiagramForm = loForm
                If IsSomething(loORMDiagramForm.zrPage) Then
                    If lrPage.CopiedPageId <> loORMDiagramForm.zrPage.PageId Then
                        Me.ToolStripMenuItemPaste.Enabled = True
                    Else
                        Me.ToolStripMenuItemPaste.Enabled = False
                    End If
                End If
            End If
        Else
            Me.ToolStripMenuItemPaste.Enabled = False
        End If

    End Sub

    Private Sub ToolStripMenuItemPaste_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemPaste.Click
        Call frmMain.PasteToCurrentPageFromClipboard()

        Me.ToolStripMenuItemPaste.Enabled = False
    End Sub

    Private Sub CopyToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemCopy.Click

        Call frmMain.CopySelectedObjectsToClipboard()

    End Sub

End Class