Imports System.Reflection

Public Class tApplication

    Public ApplicationVersionNr As String = "" 'Set in frmMain.Load

    Public MainForm As frmMain

    ''' <summary>
    ''' Is the DatabaseVersion required by the Richmond application.
    '''   NB This may be quite different from My.Settings.DatabaseVersionNumber, which is the actual version number of the database.
    '''   When a Richmond upgrade is installed this Attribute may be different (higher version) than the installed database version number.
    '''   In the case where this attribute differs from My.Settings.DatabaseVersionNumber an upgrade to the database schema must be performed.
    '''   See frmDatabaseUpgrade an functionality in frmMain for more details on database schema upgrade.
    ''' </summary>
    ''' <remarks></remarks>
    Public DatabaseVersionNr As String = ""    'Set in frmMain.Load
    'NB To access the CoreVersionNumber, look to prApplication.CMML.Core.CoreVersionNumber. I.e. Is a function of the Core that ships with Boston or as updated by an upgrade.

    Public SoftwareCategory As pcenumSoftwareCategory = pcenumSoftwareCategory.Professional

    Public User As ClientServer.User

    Public CMML As New tCMML

    Public ORMQL As New ORMQL.Processor

    Public Language As Language.LanguageGeneric

    Public WithEvents WorkingModel As FBM.Model = Nothing
    Public WithEvents WorkingPage As FBM.Page = Nothing
    Public WorkingValueType As FBM.ValueType = Nothing
    Public WorkingProject As ClientServer.Project = Nothing
    Public WorkingNamespace As ClientServer.Namespace

    Public PluginInterface As New FBMInterfaceHost
    'Public objHost As BostonPluginFramework.IHost  'Set to 'Me' in the constructor for this class. See 'New' below.

    '--------------------------------------------------------------------
    'Allow for Plugins in Boston.
    Public Plugins() As PluginServices.AvailablePlugin

    Public ToolboxForms As List(Of WeifenLuo.WinFormsUI.Docking.DockContent)
    Public LeftToolboxForms As List(Of WeifenLuo.WinFormsUI.Docking.DockContent)
    Public RightToolboxForms As List(Of WeifenLuo.WinFormsUI.Docking.DockContent)

    ''' <summary>
    ''' Pages that are currently being displayed.
    ''' </summary>
    ''' <remarks></remarks>
    Public ActivePages As List(Of WeifenLuo.WinFormsUI.Docking.DockContent)

    Public Brain As New tBrain

    ''' <summary>
    ''' Collection of the Models loaded into memory when the EnterpriseViewer form is loaded.
    ''' </summary>
    ''' <remarks></remarks>
    Public Models As New List(Of FBM.Model)

    Public UndoLog As New List(Of tUserAction)
    Public RedoLog As New List(Of tUserAction)

    Public Event ModelAdded(ByRef arModel As FBM.Model)
    Public Event WorkingModelChanged()
    Public Event WorkingPageChanged()
    Public Event ConfigurationChanged()

    Public Sub New()

        Try
            Dim lrLanguageModel As FBM.Model

            Richmond.WriteToStatusBar("Loading Language", True)
            Me.Language = New Language.LanguageGeneric(My.Settings.WordNetDictionaryEnglishPath)
            Richmond.WriteToStatusBar("Language Loaded", True)

            Select Case My.Settings.DefaultLanguage
                Case Is = pcenumNaturalLanguage.English.ToString
                    lrLanguageModel = New FBM.Model(pcenumNaturalLanguage.English.ToString, pcenumNaturalLanguage.English.ToString)
                    Me.Language.Model = lrLanguageModel
            End Select
            Richmond.WriteToStatusBar("Created Model for Language", True)


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            MsgBox(lsMessage)
        End Try

    End Sub

    Public Sub addModel(ByRef arModel As FBM.Model)

        Me.Models.AddUnique(arModel)

        RaiseEvent ModelAdded(arModel)

    End Sub

    Public Sub UndoLastAction()

        Dim lrUserAction As tUserAction
        Dim lsLastTransactionId As String

        If Me.UndoLog.Count > 0 Then

            lrUserAction = Me.UndoLog(Me.UndoLog.Count - 1)
            lsLastTransactionId = lrUserAction.TransactionId

            Do
                Select Case lrUserAction.Action
                    Case Is = pcenumUserAction.MoveModelObject
                        If lrUserAction.Page.FormLoaded Then
                            Dim lrPageObject As Object
                            lrPageObject = lrUserAction.ModelObject
                            lrPageObject.Shape.Move(lrUserAction.PreActionModelObject.X, lrUserAction.PreActionModelObject.Y)
                            lrPageObject.X = lrPageObject.Shape.Bounds.X
                            lrPageObject.Y = lrPageObject.Shape.Bounds.Y
                        End If
                    Case Is = pcenumUserAction.RemovedPageObjectFromPage
                        If lrUserAction.Page.FormLoaded Then
                            Select Case lrUserAction.ModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim loPt As New PointF(lrUserAction.ModelObject.X, lrUserAction.ModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropEntityTypeAtPoint(lrUserAction.ModelObject.EntityType, loPt)
                                Case Is = pcenumConceptType.ValueType
                                    Dim loPt As New PointF(lrUserAction.ModelObject.X, lrUserAction.ModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropValueTypeAtPoint(lrUserAction.ModelObject.ValueType, loPt)
                                Case Is = pcenumConceptType.FactType
                                    Dim loPt As New PointF(lrUserAction.ModelObject.X, lrUserAction.ModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropFactTypeAtPoint(lrUserAction.ModelObject.FactType, loPt, False)
                            End Select
                        End If

                    Case Is = pcenumUserAction.AddPageObjectToPage
                        If lrUserAction.Page.FormLoaded Then
                            Select Case lrUserAction.ModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                    lrEntityTypeInstance = lrUserAction.ModelObject
                                    lrEntityTypeInstance.RemoveFromPage(True)
                                Case Is = pcenumConceptType.ValueType
                                    Dim lrValueTypeInstance As FBM.ValueTypeInstance
                                    lrValueTypeInstance = lrUserAction.ModelObject
                                    lrValueTypeInstance.RemoveFromPage(True)
                                Case Is = pcenumConceptType.FactType
                                    Dim lrFactTypeInstance As FBM.FactTypeInstance
                                    lrFactTypeInstance = lrUserAction.ModelObject
                                    lrFactTypeInstance.RemoveFromPage(True)
                                Case Is = pcenumConceptType.RoleConstraint
                                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                                    lrRoleConstraintInstance = lrUserAction.ModelObject
                                    lrRoleConstraintInstance.RemoveFromPage(True)
                            End Select
                        End If
                    Case Is = pcenumUserAction.AddNewPageObjectToPage
                        If lrUserAction.Page.FormLoaded Then
                            Select Case lrUserAction.ModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                    lrEntityTypeInstance = lrUserAction.ModelObject
                                    lrEntityTypeInstance.RemoveFromPage(True)
                                    lrEntityTypeInstance.EntityType.RemoveFromModel(False)
                                Case Is = pcenumConceptType.ValueType
                                    Dim lrValueTypeInstance As New FBM.ValueTypeInstance
                                    lrValueTypeInstance = lrUserAction.ModelObject
                                    lrValueTypeInstance.RemoveFromPage(True)
                                    lrValueTypeInstance.ValueType.RemoveFromModel()
                            End Select
                        End If
                End Select

                Me.RedoLog.Add(lrUserAction)
                Me.UndoLog.RemoveAt(Me.UndoLog.Count - 1)

                If Me.UndoLog.Count > 0 Then
                    lrUserAction = Me.UndoLog(Me.UndoLog.Count - 1)
                End If
            Loop Until (Me.UndoLog.Count = 0) Or (lrUserAction.TransactionId <> lsLastTransactionId)


            frmMain.ToolStripMenuItemUndo.Enabled = (Me.UndoLog.Count > 0)
            frmMain.ToolStripMenuItemRedo.Enabled = (Me.RedoLog.Count > 0)

            Call Me.SetUndoRedoMenuTexts()

        End If

    End Sub

    Public Sub RedoLastAction()

        Dim lrUserAction As tUserAction
        Dim lsLastTransactionId As String

        If Me.RedoLog.Count > 0 Then

            lrUserAction = Me.RedoLog(Me.RedoLog.Count - 1)
            lsLastTransactionId = lrUserAction.TransactionId

            Do
                Select Case lrUserAction.Action
                    Case Is = pcenumUserAction.MoveModelObject
                        If lrUserAction.Page.FormLoaded Then
                            Dim lrPageObject As Object
                            lrPageObject = lrUserAction.ModelObject
                            lrPageObject.Shape.Move(lrUserAction.PostActionModelObject.X, lrUserAction.PostActionModelObject.Y)
                            lrPageObject.X = lrPageObject.Shape.Bounds.X
                            lrPageObject.Y = lrPageObject.Shape.Bounds.Y
                        End If
                    Case Is = pcenumUserAction.RemovedPageObjectFromPage
                        If lrUserAction.Page.FormLoaded Then
                            Select Case lrUserAction.ModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                    lrEntityTypeInstance = lrUserAction.ModelObject
                                    lrEntityTypeInstance.RemoveFromPage(True)
                                Case Is = pcenumConceptType.ValueType
                                    Dim lrValueTypeInstance As FBM.ValueTypeInstance
                                    lrValueTypeInstance = lrUserAction.ModelObject
                                    lrValueTypeInstance.RemoveFromPage(True)
                                Case Is = pcenumConceptType.FactType
                                    Dim lrFactTypeInstance As FBM.FactTypeInstance
                                    lrFactTypeInstance = lrUserAction.ModelObject
                                    lrFactTypeInstance.RemoveFromPage(True)
                            End Select
                        End If

                    Case Is = pcenumUserAction.AddPageObjectToPage
                        If lrUserAction.Page.FormLoaded Then
                            Select Case lrUserAction.ModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim loPt As New PointF(lrUserAction.PreActionModelObject.X, lrUserAction.PreActionModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropEntityTypeAtPoint(lrUserAction.PreActionModelObject.EntityType, loPt)
                                Case Is = pcenumConceptType.ValueType
                                    Dim loPt As New PointF(lrUserAction.PreActionModelObject.X, lrUserAction.PreActionModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropValueTypeAtPoint(lrUserAction.PreActionModelObject.ValueType, loPt)
                                Case Is = pcenumConceptType.FactType
                                    Dim loPt As New PointF(lrUserAction.PreActionModelObject.X, lrUserAction.PreActionModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropFactTypeAtPoint(lrUserAction.PreActionModelObject.FactType, loPt, False)
                                Case Is = pcenumConceptType.RoleConstraint
                                    Dim loPt As New PointF(lrUserAction.PreActionModelObject.X, lrUserAction.PreActionModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropRoleConstraintAtPoint(lrUserAction.PreActionModelObject.RoleConstraint, loPt)
                            End Select
                        End If
                    Case Is = pcenumUserAction.AddNewPageObjectToPage
                        If lrUserAction.Page.FormLoaded Then
                            Select Case lrUserAction.ModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim loPt As New PointF(lrUserAction.PreActionModelObject.X, lrUserAction.PreActionModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropEntityTypeAtPoint(lrUserAction.PreActionModelObject.EntityType, loPt)
                                Case Is = pcenumConceptType.ValueType
                                    Dim loPt As New PointF(lrUserAction.PreActionModelObject.X, lrUserAction.PreActionModelObject.Y)
                                    lrUserAction.ModelObject = lrUserAction.Page.DropValueTypeAtPoint(lrUserAction.PreActionModelObject.ValueType, loPt)
                            End Select
                        End If
                End Select

                Me.UndoLog.Add(lrUserAction)
                Me.RedoLog.RemoveAt(Me.RedoLog.Count - 1)

                If Me.RedoLog.Count > 0 Then
                    lrUserAction = Me.RedoLog(Me.RedoLog.Count - 1)
                End If

            Loop Until (Me.RedoLog.Count = 0) Or (lrUserAction.TransactionId <> lsLastTransactionId)

            frmMain.ToolStripMenuItemRedo.Enabled = (Me.RedoLog.Count > 0)
            frmMain.ToolStripMenuItemUndo.Enabled = (Me.UndoLog.Count > 0)

            Call Me.SetUndoRedoMenuTexts()

        End If

    End Sub

    Public Sub AddUndoAction(ByRef arUserAction As tUserAction)

        Me.UndoLog.Add(arUserAction)

        Call Me.SetUndoRedoMenuTexts()

    End Sub

    Private Sub SetUndoRedoMenuTexts()

        frmMain.ToolStripMenuItemUndo.Text = "&Undo"
        If Me.UndoLog.Count > 0 Then
            Select Case Me.UndoLog(Me.UndoLog.Count - 1).Action
                Case Is = pcenumUserAction.AddNewPageObjectToPage
                    frmMain.ToolStripMenuItemUndo.Text &= " {Add new Page Object}"
                Case Is = pcenumUserAction.AddPageObjectToPage
                    frmMain.ToolStripMenuItemUndo.Text &= " {Add Page Object}"
                Case Is = pcenumUserAction.MoveModelObject
                    frmMain.ToolStripMenuItemUndo.Text &= " {Move Page Object}"
                Case Is = pcenumUserAction.RemovedPageObjectFromPage
                    frmMain.ToolStripMenuItemUndo.Text &= " {Remove Page Object}"
            End Select
        End If

        frmMain.ToolStripMenuItemRedo.Text = "&Redo"
        If Me.RedoLog.Count > 0 Then
            Select Case Me.RedoLog(Me.RedoLog.Count - 1).Action
                Case Is = pcenumUserAction.AddNewPageObjectToPage
                    frmMain.ToolStripMenuItemRedo.Text &= " {Add new Page Object}"
                Case Is = pcenumUserAction.AddPageObjectToPage
                    frmMain.ToolStripMenuItemRedo.Text &= " {Add Page Object}"
                Case Is = pcenumUserAction.MoveModelObject
                    frmMain.ToolStripMenuItemRedo.Text &= " {Move Page Object}"
                Case Is = pcenumUserAction.RemovedPageObjectFromPage
                    frmMain.ToolStripMenuItemRedo.Text &= " {Remove Page Object}"
            End Select
        End If

    End Sub

    Sub ChangeWorkingEnvironment(ByVal arWorkingEnvironment As tWorkingEnvironment)

        Call Me.ResetWorkingEnvironment()

        Me.WorkingModel = arWorkingEnvironment.Model
        Me.WorkingPage = arWorkingEnvironment.Page

        RaiseEvent WorkingModelChanged()

    End Sub

    Public Function createDatabase(ByVal arCreateDatabaseStatement As FEQL.CREATEDATABASEStatement) As FBM.Model

        Try
            If frmMain.zfrmModelExplorer IsNot Nothing Then

                Return frmMain.zfrmModelExplorer.addNewModelToBoston(arCreateDatabaseStatement.DATABASENAME, arCreateDatabaseStatement)
            Else
                MsgBox("Please check that the Model Explorer view is open before adding a new model to Boston.")
                Return Nothing
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

    ''' <summary>
    ''' Gets a Model from the list of Models loaded into the application (in this class), else returns Nothing
    ''' </summary>
    ''' <returns></returns>
    Public Function getBostonApplicationModelById(ByVal asModelId As String) As FBM.Model
        Return Me.Models.Find(Function(x) x.ModelId = asModelId)
    End Function


    Public Function GetToolboxForm(ByVal asFormName As String) As WeifenLuo.WinFormsUI.Docking.DockContent

        Dim lrForm As WeifenLuo.WinFormsUI.Docking.DockContent

        Select Case asFormName
            Case Is = "frmToolbox", frmToolboxProperties.Name, frmToolboxModelDictionary.Name
                For Each lrForm In Me.RightToolboxForms
                    If lrForm.Name = asFormName Then
                        Return lrForm
                    End If
                Next
            Case Else
                For Each lrForm In Me.ToolboxForms
                    If lrForm.Name = asFormName Then
                        Return lrForm
                    End If
                Next
        End Select

        Return Nothing

    End Function

    Public Sub ResetWorkingEnvironment()

        With Me
            .WorkingModel = Nothing
            .WorkingPage = Nothing
            .WorkingValueType = Nothing
        End With

    End Sub

    Public Function ThrowErrorMessage(ByVal asErrorMessage As String,
                                      ByVal aiMessageType As pcenumErrorType,
                                      Optional ByVal asStackTrace As String = Nothing,
                                      Optional ByVal abShowStackTrace As Boolean = True,
                                      Optional ByVal abAbortApplication As Boolean = False,
                                      Optional ByVal abThrowtoMSGBox As Boolean = False,
                                      Optional ByRef aiMessageBoxButtons As MessageBoxButtons = MessageBoxButtons.OK) As DialogResult

        Dim lsStackTrace As String = ""

        Try
            If IsSomething(asStackTrace) And abShowStackTrace Then
                asErrorMessage &= vbCrLf & vbCrLf & "Stack Trace: " & asStackTrace
            ElseIf IsSomething(asStackTrace) Then
                lsStackTrace = asStackTrace
            Else
                lsStackTrace = ""
            End If

            'CurrentFunctionName
            ' = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name;

            If My.Settings.DebugMode = pcenumDebugMode.Debug.ToString Then
                '-----------------------
                'Write to the ErrorLog
                '-----------------------
                prLogger.WriteToErrorLog(asErrorMessage, lsStackTrace, "Title")

                Select Case aiMessageType
                    Case Is = pcenumErrorType.Information
                        If My.Settings.ThrowInformationDebugMessagesToScreen Then
                            MsgBox(asErrorMessage)
                        End If
                    Case Is = pcenumErrorType.Critical
                        If My.Settings.ThrowCriticalDebugMessagesToScreen Then
                            MsgBox(asErrorMessage)
                        End If
                        Call prLogger.WriteToErrorLog(asErrorMessage, "", "Critial Error")
                    Case Is = pcenumErrorType.Warning
                        Call prLogger.WriteToErrorLog(asErrorMessage, "", "Warning")
                End Select
            ElseIf My.Settings.DebugMode = pcenumDebugMode.DebugCriticalErrorsOnly.ToString Then

                Select Case aiMessageType
                    Case Is = pcenumErrorType.Information
                        If pbLogStartup Then
                            '-----------------------
                            'Write to the ErrorLog
                            '-----------------------
                            Call prLogger.WriteToErrorLog(asErrorMessage, "", "Information")
                        Else
                            If abThrowtoMSGBox Then
                                Return MsgBox(asErrorMessage, aiMessageBoxButtons)
                            Else
                                '-----------------------------
                                'Do nothing
                                '-----------------------------
                            End If

                        End If
                    Case Is = pcenumErrorType.Critical
                        '-----------------------
                        'Write to the ErrorLog
                        '-----------------------
                        Call prLogger.WriteToErrorLog(asErrorMessage, "", "Critial Error")

                        If My.Settings.ThrowCriticalDebugMessagesToScreen Then

                            If abAbortApplication Then asErrorMessage &= vbCrLf & vbCrLf & "This is a critical error. Boston will now close."

                            MsgBox(asErrorMessage, MsgBoxStyle.Critical)

                            If abAbortApplication Then Call Application.Exit()

                        End If
                    Case Is = pcenumErrorType.Warning

                        If abThrowtoMSGBox Then
                            MsgBox(asErrorMessage, MsgBoxStyle.Exclamation)
                        End If
                End Select

            Else
                '--------------------------------------
                'Throw the ErrorMessage to the screen
                '--------------------------------------
                MsgBox(asErrorMessage)
            End If

        Catch ex As Exception

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            lsMessage &= vbCrLf & vbCrLf & asErrorMessage

            MsgBox(lsMessage)
        End Try

    End Function

    Public Sub setWorkingModel(ByRef arModel As FBM.Model)

        Dim lrOriginalWorkingModel = Me.WorkingModel
        Me.WorkingModel = arModel

        If lrOriginalWorkingModel IsNot Me.WorkingModel Then
            RaiseEvent WorkingModelChanged()
        End If
    End Sub

    Public Sub setWorkingPage(ByRef arPage As FBM.Page)

        If arPage.Model IsNot Me.WorkingModel Then
            Call Me.setWorkingModel(arPage.Model)
        End If

        Dim lrOriginalWorkingPage = Me.WorkingPage
        Me.WorkingPage = arPage

        If lrOriginalWorkingPage IsNot Me.WorkingPage Then
            RaiseEvent WorkingPageChanged()
        End If
    End Sub

    Private Sub WorkingModel_MadeDirty(abGlobalBroadcast As Boolean) Handles WorkingModel.MadeDirty

        frmMain.ToolStripButton_Save.Enabled = True
    End Sub

    Public Sub triggerConfigurationChanged()
        RaiseEvent ConfigurationChanged()
    End Sub

    Private Sub WorkingModel_Saved() Handles WorkingModel.Saved
        frmMain.ToolStripButton_Save.Enabled = False
    End Sub

    Private Sub WorkingPage_PageDeleted() Handles WorkingPage.PageDeleted

        Me.WorkingPage = Nothing

    End Sub

End Class
