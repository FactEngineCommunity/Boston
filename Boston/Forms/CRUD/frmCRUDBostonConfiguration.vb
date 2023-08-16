Imports System.Xml.Serialization
Imports System.Xml.Linq
Imports System.IO
Imports System.Reflection
Imports System.Data.Common
Imports System.ComponentModel

Public Class frmCRUDBostonConfiguration

    Public mbSuperUserModeClicks As Integer = 0
    Public msConnectionString As String = My.Settings.DatabaseConnectionString
    Public msDatabaseType As String = My.Settings.DatabaseType

    Sub LoadDebugModes()

        Dim loWorkingClass As New Object
        Dim lloDebugModes As New List(Of Object)
        Dim liReferenceTableId As Integer = 0
        Dim liInd As Integer = 0
        Dim liNewIndex As Integer = 0

        If pdbConnection IsNot Nothing AndAlso pdbConnection.State <> 0 Then
            liReferenceTableId = TableReferenceTable.GetReferenceTableIdByName("DebugMode")
            lloDebugModes = TableReferenceFieldValue.GetReferenceFieldValueTuples(liReferenceTableId, loWorkingClass)

            For liInd = 1 To lloDebugModes.Count
                liNewIndex = Me.ComboBoxDebugMode.Items.Add(lloDebugModes(liInd - 1).DebugMode)
                If lloDebugModes(liInd - 1).DebugMode = My.Settings.DebugMode Then
                    Me.ComboBoxDebugMode.SelectedIndex = liNewIndex
                End If
            Next
        Else
            Me.ComboBoxDebugMode.Items.Add(pcenumDebugMode.Debug.ToString)
            Me.ComboBoxDebugMode.Items.Add(pcenumDebugMode.DebugCriticalErrorsOnly.ToString)
            Me.ComboBoxDebugMode.Items.Add(pcenumDebugMode.NoLogging.ToString)

            Me.ComboBoxDebugMode.SelectedIndex = Me.ComboBoxDebugMode.FindString(My.Settings.DebugMode)

        End If

    End Sub

    Private Sub SetupForm()

        Call Me.LoadDebugModes()

        Try

            'Error Management
            Me.CheckBoxThrowInformationDebugMessagesToScreen.Checked = My.Settings.ThrowInformationDebugMessagesToScreen
            Me.CheckBoxThrowCriticalDebugMessagesToScreen.Checked = My.Settings.ThrowCriticalDebugMessagesToScreen
            Me.CheckBoxAutomaticallyReportErrorEvents.Checked = My.Settings.UseAutomatedErrorReporting
            Me.CheckBoxShowStackTrace.Checked = My.Settings.BostonErrorMessagesShowStackTrace
            Me.CheckBoxUseFlashCardErrorMessages.Checked = My.Settings.BostonErrorMessagesShowFlashCard

            'Import/Export
            Me.CheckBoxExportSuppressMDAModelElements.Checked = My.Settings.ExportFBMExcludeMDAModelElements

            '---------------------------------------------------------
            'Virtual Analyst
            '-----------------
            Me.CheckBoxVirtualAnalystDisplayBriana.Checked = My.Settings.DisplayBrianaVirtualAnalyst
            Me.CheckBoxStartVirtualAnalystInQuietMode.Checked = My.Settings.StartVirtualAnalystInQuietMode

            '---------------------------------------------------------
            'Database
            '-----------------
            Call Me.LoadDatabaseTypes()
            RemoveHandler CheckBoxUseThreadingDatabaseLoad.CheckedChanged, AddressOf CheckBoxUseThreadingDatabaseLoad_CheckedChanged
            Me.CheckBoxUseThreadingDatabaseLoad.Checked = My.Settings.ModelLoadPagesUseThreading
            AddHandler CheckBoxUseThreadingDatabaseLoad.CheckedChanged, AddressOf CheckBoxUseThreadingDatabaseLoad_CheckedChanged
            Me.CheckBoxStoreAndUseBinarySerialisations.Checked = My.Settings.DatabaseStoreModelsAsBLOBsParallelToXML

            Me.TextBoxDatabaseConnectionString.Text = My.Settings.DatabaseConnectionString

            Me.LabelConfigurationFileLocation.Text = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
            Me.LabelUserConfigurationFileLocation.Text = Boston.GetConfigFileLocation

            'Boston Tab
            Me.CheckBoxAutomaticallyCheckForUpdates.Checked = My.Settings.UseAutoUpdateChecker

            Me.ComboBoxDatabaseType.Enabled = True

            'Client/Server
            Me.CheckBoxUseRemoteUI.Checked = My.Settings.UseVirtualUI
            Me.CheckBoxEnableClientServer.Checked = My.Settings.UseClientServer
            Me.CheckBoxLoggingOutEndsSession.Checked = My.Settings.LoggingOutEndsSession
            Me.CheckBoxClientServerRequireLoginAtStartup.Checked = My.Settings.RequireLoginAtStartup
            Me.CheckBoxClientServerInitialiseClient.Checked = My.Settings.InitialiseClient
            Me.TextBoxBostonServerPortNumber.Text = Trim(My.Settings.BostonServerPortNumber)

            If My.Settings.FactEngineDefaultQueryResultLimit = 0 Then
                Me.DomainUpDownFactEngineDefaultQueryResultLimit.Text = "Infinite"
            Else
                Me.DomainUpDownFactEngineDefaultQueryResultLimit.Text = My.Settings.FactEngineDefaultQueryResultLimit
            End If

            'FactEngine
            Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.Checked = My.Settings.FactEngineShowDatabaseLogoInModelExplorer
            Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.Checked = My.Settings.FactEngineUseReferenceModeOnlyForSimpleReferenceSchemes
            Me.ComboBoxFactEngineUserDateFormat.Text = My.Settings.FactEngineUserDateFormat
            Me.ComboBoxFactEngineUserDateTimeFormat.Text = My.Settings.FactEngineUserDateTimeFormat
            Me.CheckBoxFactEngineShowStackTrace.Checked = My.Settings.ShowStackTraceFactEngineQuery
            Me.CheckBoxFactEngineUseTransformations.Enabled = My.Settings.SuperuserMode
            Me.CheckBoxFactEngineUseTransformations.Checked = My.Settings.FactEngineUseTransformations
            Me.CheckBoxFactEngineUseGPT3.Checked = My.Settings.FactEngineUseGPT3
            Me.TextBoxFactEngineOpenAIAPIKey.Text = Trim(My.Settings.FactEngineOpenAIAPIKey)

            'ER Diagrams
            Me.CheckBoxHideUnknownPredicates.Checked = My.Settings.ERDViewHideUnknowPredicates
            Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName.Checked = My.Settings.ImportExportColumnNameForFKReferenceEqualsReferencedEntityName 'As in when importing NORMA .orm file.

            'Diagrams General
            Me.CheckBoxDiagramSpyShowLinkFactTypes.Checked = My.Settings.DiagramSpyShowLinkFactTypes

            'Superuser Mode
            Me.CheckBoxSuperuserMode.Enabled = My.Settings.SuperuserMode
            RemoveHandler CheckBoxSuperuserMode.CheckedChanged, AddressOf CheckBoxSuperuserMode_CheckedChanged
            Me.CheckBoxSuperuserMode.Checked = My.Settings.SuperuserMode
            AddHandler CheckBoxSuperuserMode.CheckedChanged, AddressOf CheckBoxSuperuserMode_CheckedChanged

            Me.CheckBoxAutoCompleteSingleClickSelects.Checked = My.Settings.AutoCompleteSingleClickSelects

            'Reverse Engineering
            Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.Checked = My.Settings.ReverseEngineeringKeepDatabaseColumnNames
            Me.TextBoxReverseEngineeringDefaultReferenceMode.Text = Trim(My.Settings.ReverseEngineeringDefaultReferenceMode)

            'Modelling
            Me.CheckBoxUseDefaultReferenceMode.Checked = My.Settings.UseDefaultReferenceModeNewEntityTypes
            Me.TextBoxDefaultReferenceMode.Text = Trim(My.Settings.DefaultReferenceMode)
            Me.ComboBoxDefaultGeneralConceptConversion.SelectedIndex = Me.ComboBoxDefaultGeneralConceptConversion.Items.IndexOf(My.Settings.DefaultGeneralConceptToObjectTypeConversion)
            Me.CheckBoxModelllingUseThreadingLoadingXMLPage.Checked = My.Settings.ModelingUseThreadedXMLPageLoading
            Me.CheckBoxHideReferenceModeOnReferenceModeSet.Checked = My.Settings.HideAllReferenceModesOnReferenceModeSet

            'Code Generation
            Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.Checked = My.Settings.CodeGenerationUseSquareBracketsSQLTableNames

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

        Finally

            If My.Settings.SuperuserMode Then
                Call Me.SetupUsingSuperUserMode()
            End If

        End Try

    End Sub

    Private Sub SetupUsingSuperUserMode()

        Try
            Me.CheckBoxFactEngineUseTransformations.Enabled = True
            Me.CheckBoxClientServerInitialiseClient.Enabled = True
            Me.CheckBoxClientServerRequireLoginAtStartup.Enabled = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub frmCRUDRichmondConfiguration_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call Me.SetupForm()


    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click

        Me.Hide()
        Me.Close()

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        Dim lsReturnString As String = ""
        Dim lbApplicationRestartRequired As Boolean = False

        Try

            If Not Me.ErrorProvider.IsValid Then
                Dim lrInvalidControl As Control = Me.ErrorProvider.getInvalidControl
                lrInvalidControl.Show()
                lrInvalidControl.Focus()
                Me.ErrorProvider.SetError(Me.button_okay, "Invalid value in one field. Check each tab for errors.")
                Exit Sub
            End If

            If check_fields(lsReturnString) Then

                '------------------------------------------------------------------------
                'DatabaseType - Check if changed
                '---------------------------------
                If Me.ComboBoxDatabaseType.SelectedItem.Tag.ToString <> My.Settings.DatabaseType Then

                    If MsgBox("Changing the database type requires an application restart. Are you happy with that?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                        lbApplicationRestartRequired = True
                    Else
                        Exit Sub
                    End If

                End If

                My.Settings.DebugMode = ComboBoxDebugMode.SelectedItem
                My.Settings.DatabaseType = Me.ComboBoxDatabaseType.SelectedItem.ToString
                My.Settings.DatabaseConnectionString = Me.TextBoxDatabaseConnectionString.Text
                My.Settings.DisplayBrianaVirtualAnalyst = Me.CheckBoxVirtualAnalystDisplayBriana.Checked
                My.Settings.StartVirtualAnalystInQuietMode = Me.CheckBoxStartVirtualAnalystInQuietMode.Checked

                'Database
                My.Settings.ModelLoadPagesUseThreading = Me.CheckBoxUseThreadingDatabaseLoad.Checked
                My.Settings.DatabaseStoreModelsAsBLOBsParallelToXML = Me.CheckBoxStoreAndUseBinarySerialisations.Checked

                'Import/Export
                My.Settings.ExportFBMExcludeMDAModelElements = Me.CheckBoxExportSuppressMDAModelElements.Checked

                'Error messages
                My.Settings.ThrowCriticalDebugMessagesToScreen = Me.CheckBoxThrowCriticalDebugMessagesToScreen.Checked
                My.Settings.ThrowInformationDebugMessagesToScreen = Me.CheckBoxThrowInformationDebugMessagesToScreen.Checked
                My.Settings.BostonErrorMessagesShowStackTrace = Me.CheckBoxShowStackTrace.Checked
                My.Settings.BostonErrorMessagesShowFlashCard = Me.CheckBoxUseFlashCardErrorMessages.Checked

                'Client/Server
                My.Settings.UseVirtualUI = Me.CheckBoxUseRemoteUI.Checked
                My.Settings.UseClientServer = Me.CheckBoxEnableClientServer.Checked
                My.Settings.LoggingOutEndsSession = Me.CheckBoxLoggingOutEndsSession.Checked
                My.Settings.RequireLoginAtStartup = Me.CheckBoxClientServerRequireLoginAtStartup.Checked
                My.Settings.InitialiseClient = Me.CheckBoxClientServerInitialiseClient.Checked
                My.Settings.BostonServerPortNumber = Trim(Me.TextBoxBostonServerPortNumber.Text)

                'FactEngine
                My.Settings.FactEngineShowDatabaseLogoInModelExplorer = Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.Checked
                My.Settings.FactEngineUseReferenceModeOnlyForSimpleReferenceSchemes = Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.Checked
                My.Settings.ShowStackTraceFactEngineQuery = Me.CheckBoxFactEngineShowStackTrace.Checked
                My.Settings.FactEngineUseTransformations = Me.CheckBoxFactEngineUseTransformations.Checked
                My.Settings.FactEngineUseGPT3 = Me.CheckBoxFactEngineUseGPT3.Checked
                My.Settings.FactEngineOpenAIAPIKey = Trim(Me.TextBoxFactEngineOpenAIAPIKey.Text)

                My.Settings.ERDViewHideUnknowPredicates = Me.CheckBoxHideUnknownPredicates.Checked

                My.Settings.AutoCompleteSingleClickSelects = Me.CheckBoxAutoCompleteSingleClickSelects.Checked
                My.Settings.ReverseEngineeringKeepDatabaseColumnNames = Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.Checked
                My.Settings.ReverseEngineeringDefaultReferenceMode = Trim(Me.TextBoxReverseEngineeringDefaultReferenceMode.Text)
                My.Settings.DiagramSpyShowLinkFactTypes = Me.CheckBoxDiagramSpyShowLinkFactTypes.Checked
                My.Settings.UseAutoUpdateChecker = Me.CheckBoxAutomaticallyCheckForUpdates.Checked
                My.Settings.UseAutomatedErrorReporting = Me.CheckBoxAutomaticallyReportErrorEvents.Checked

                'Modelling
                My.Settings.UseDefaultReferenceModeNewEntityTypes = Me.CheckBoxUseDefaultReferenceMode.Checked
                My.Settings.DefaultReferenceMode = Trim(Me.TextBoxDefaultReferenceMode.Text)
                My.Settings.DefaultGeneralConceptToObjectTypeConversion = Me.ComboBoxDefaultGeneralConceptConversion.SelectedItem.ToString
                My.Settings.ModelingUseThreadedXMLPageLoading = Me.CheckBoxModelllingUseThreadingLoadingXMLPage.Checked
                My.Settings.HideAllReferenceModesOnReferenceModeSet = Me.CheckBoxHideReferenceModeOnReferenceModeSet.Checked

                'CodeGeneration
                'Code Generation
                My.Settings.CodeGenerationUseSquareBracketsSQLTableNames = Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.Checked

                'Superuser Mode          
                My.Settings.SuperuserMode = Me.CheckBoxSuperuserMode.Checked

                Try
                    If Me.DomainUpDownFactEngineDefaultQueryResultLimit.Text = "Infinite" Then
                        My.Settings.FactEngineDefaultQueryResultLimit = 0
                    Else
                        My.Settings.FactEngineDefaultQueryResultLimit = CInt(Me.DomainUpDownFactEngineDefaultQueryResultLimit.Text)
                    End If
                Catch ex As Exception

                End Try

                My.Settings.Save()
                Me.Hide()
                Me.Close()
                Me.Dispose()

                Call prApplication.triggerConfigurationChanged()

                If lbApplicationRestartRequired Then
                    Process.Start(Application.ExecutablePath)
                    Application.Exit()
                End If
            Else
                MsgBox(lsReturnString)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function check_fields(ByRef asReturnMessage As String) As Boolean

        If Not Me.checkDatabaseConnectionString(asReturnMessage, Trim(Me.TextBoxDatabaseConnectionString.Text)) Then
            Return False
        End If

        Return True

    End Function

    Private Function checkDatabaseConnectionString(ByRef asReturnMessage As String,
                                                   Optional ByVal asConnectionString As String = Nothing) As Boolean

        Dim lsDatabaseLocation As String
        Dim lsConnectionString As String

        If asConnectionString IsNot Nothing Then
            lsConnectionString = asConnectionString
        Else
            lsConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)
        End If

        Try
            Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                Case Is = pcenumDatabaseType.SQLite, pcenumDatabaseType.MSJet
                    'Nothing to do here.
                Case Else
                    GoTo TestConnectionString
            End Select


            Dim lrSQLConnectionStringBuilder As System.Data.Common.DbConnectionStringBuilder = Nothing

            Try
                lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True) With {
                   .ConnectionString = lsConnectionString
                }

                lsDatabaseLocation = lrSQLConnectionStringBuilder("Data Source")

            Catch ex As Exception
                asReturnMessage = "Please fix the Database Connection String and try again." & vbCrLf & vbCrLf & ex.Message
                Return False
            End Try

            If Not System.IO.File.Exists(lsDatabaseLocation) Then
                asReturnMessage = "The database source of the Database Connection String you provided points to a file that does not exist."
                asReturnMessage &= vbCrLf & vbCrLf
                asReturnMessage &= "Please fix the Database Connection String and try again."
                Return False
            End If

TestConnectionString:
            Try
                Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                    Case Is = pcenumDatabaseType.SQLite
                        If Database.SQLiteDatabase.CreateConnection(lsConnectionString) Is Nothing Then
                            Throw New Exception("Can't connect to the database with that connection string.")
                        End If
                    Case Is = pcenumDatabaseType.MSJet
                        Dim ldbConnection As New ADODB.Connection
                        Call ldbConnection.Open(lsConnectionString)
                    Case Is = pcenumDatabaseType.PostgreSQL
                        ' Create an instance of the OdbcConnection class
                        Using connection As New System.Data.Odbc.OdbcConnection(lsConnectionString)
                            Try
                                ' Open the connection (which will test the connection string)
                                connection.Open()

                                ' If the connection string is valid, this block will be executed
                                Console.WriteLine("Connection successful!")
                            Catch ex As Exception
                                ' If there is an exception, the connection string is likely invalid or there is an issue with the database server
                                Throw New Exception("Connection failed!" & ex.Message)

                            End Try
                        End Using
                End Select
            Catch ex As Exception
                asReturnMessage &= "Please fix the Database Connection String and try again." & vbCrLf & vbCrLf & ex.Message
                Return False
            End Try

            Return True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

    Sub LoadDatabaseTypes()

        Dim loWorkingClass As New Object
        Dim larDatabaseType As New List(Of Object)
        Dim liReferenceTableId As Integer = 0
        Dim liInd As Integer = 0
        Dim liNewIndex As Integer = 0

        If pdbConnection IsNot Nothing AndAlso pdbConnection.State <> 0 Then
            liReferenceTableId = TableReferenceTable.GetReferenceTableIdByName("DatabaseType")
            larDatabaseType = TableReferenceFieldValue.GetReferenceFieldValueTuples(liReferenceTableId, loWorkingClass)

            Dim laiDatabaseType = {pcenumDatabaseType.MSJet, pcenumDatabaseType.SQLite, pcenumDatabaseType.PostgreSQL}

            For liInd = 1 To larDatabaseType.Count

                Dim liDatabaseType = CType([Enum].Parse(GetType(pcenumDatabaseType), Viev.NullVal(larDatabaseType(liInd - 1).DatabaseType, pcenumDatabaseType.None)), pcenumDatabaseType)

                If laiDatabaseType.Contains(liDatabaseType) Then
                    Dim lrComboboxItem As New tComboboxItem(larDatabaseType(liInd - 1).DatabaseType, larDatabaseType(liInd - 1).DatabaseType, liDatabaseType)
                    liNewIndex = Me.ComboBoxDatabaseType.Items.Add(lrComboboxItem)

                    If larDatabaseType(liInd - 1).DatabaseType = My.Settings.DatabaseType Then
                        Me.ComboBoxDatabaseType.SelectedIndex = liNewIndex
                    End If
                End If
            Next
        Else
            Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.MSJet, pcenumDatabaseType.MSJet.ToString, pcenumDatabaseType.MSJet))
            Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.SQLite, pcenumDatabaseType.SQLite.ToString, pcenumDatabaseType.SQLite))
            Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.PostgreSQL, pcenumDatabaseType.PostgreSQL.ToString, pcenumDatabaseType.PostgreSQL))

            Me.ComboBoxDatabaseType.SelectedIndex = Me.ComboBoxDatabaseType.FindString(My.Settings.DatabaseType)

        End If

    End Sub

    Private Sub ButtonImportLanguageRules_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportLanguageRules.Click

        Try
            If Me.DialogOpenFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then


                'Deserialize text file to a new object.
                Dim objStreamReader As New StreamReader(Me.DialogOpenFile.FileName)
                Dim p2 As New LPIO.LanguageParserRules
                Dim x As New XmlSerializer(GetType(LPIO.LanguageParserRules))
                p2 = x.Deserialize(objStreamReader)
                objStreamReader.Close()

                Dim lrXMLLanguagePhrase As LPIO.LanguagePhrase
                Dim lrXMLLPTSWS As LPIO.LPTSWS
                Dim lrLanguagePhrase As Language.LanguagePhrase
                Dim lrLPTSWS As Language.LanguagePhraseTokenSequenceWordSense

                For Each lrXMLLanguagePhrase In p2.LanguagePhrases

                    lrLanguagePhrase = New Language.LanguagePhrase With {
                        .PhraseId = lrXMLLanguagePhrase.PhraseId,
                        .PhraseType = lrXMLLanguagePhrase.PhraseType,
                        .ResolvesToLanguagePhraseId = lrXMLLanguagePhrase.ResolvesToLanguagePhraseId,
                        .Example = lrXMLLanguagePhrase.Example
                    }
                    Call lrLanguagePhrase.Save()
                    For Each lrXMLLPTSWS In lrXMLLanguagePhrase.TokenSequence
                        lrLPTSWS = New Language.LanguagePhraseTokenSequenceWordSense With {
                            .Phrase = lrLanguagePhrase,
                            .Token = lrXMLLPTSWS.Token,
                            .SequenceNr = lrXMLLPTSWS.SequenceNr,
                            .WordSense = lrXMLLPTSWS.WordSense
                        }
                        Call lrLPTSWS.Save()
                    Next
                Next

                MsgBox("Completed importing the Language Phrases.")

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

        '===========================================================
        'Dim lsFolderLocation As String = ""
        'Dim lsFileName As String = ""
        'Dim loStreamWriter As StreamWriter ' Create file by FileStream class
        'Dim loXMLSerialiser As XmlSerializer ' Create binary object
        'Dim lrModel As New LPIO.LanguageParserRules

        'Try
        '    '-----------------------------------------
        '    'Get the Model from the selected TreeNode
        '    '-----------------------------------------
        '    Dim lrLanguagePhrase As New LPIO.LanguagePhrase
        '    lrLanguagePhrase.TokenSequence.Add(New LPIO.LPTSWS)
        '    lrModel.LanguagePhrases.Add(lrLanguagePhrase)
        '    lrLanguagePhrase = New LPIO.LanguagePhrase
        '    lrLanguagePhrase.TokenSequence.Add(New LPIO.LPTSWS)
        '    lrModel.LanguagePhrases.Add(lrLanguagePhrase)

        '    If Boston.IsSerializable(lrModel) Then

        '        If DialogFolderBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '            lsFolderLocation = DialogFolderBrowser.SelectedPath
        '            lsFileName = "LanguagePhraseExport"

        '            loStreamWriter = New StreamWriter(lsFolderLocation & "\" & lsFileName & ".xml")

        '            'loXMLSerialiser = New XmlSerializer(GetType(FBM.tORMModel))
        '            loXMLSerialiser = New XmlSerializer(GetType(LPIO.LanguageParserRules))

        '            ' Serialize object to file
        '            loXMLSerialiser.Serialize(loStreamWriter, lrModel)
        '            loStreamWriter.Close()

        '            MsgBox("Your file is ready for viewing.")
        '        End If

        '    End If

        'Catch ex As Exception
        '    Dim lsMessage As String = ""
        '    lsMessage = "Error: frnToolboxEnterpriseTree.ExportToORMCMMLToolStripMenuItem: " & vbCrLf & vbCrLf & ex.Message
        '    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

        'End Try

    End Sub

    Private Sub CheckBoxThrowInformationDebugMessagesToScreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxThrowInformationDebugMessagesToScreen.Click

        Dim lsMessage As String = ""

        If CheckBoxThrowInformationDebugMessagesToScreen.Checked Then

            lsMessage = "WARNING: If the Debug Mode is set to [Debug] you may receive hundreds of information messages when starting Boston."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "Do you want to keep this setting as checked?"

            If MsgBox(lsMessage, MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.No Then

                CheckBoxThrowInformationDebugMessagesToScreen.Checked = False
            End If
        End If

    End Sub

    Private Sub DomainUpDownFactEngineDefaultQueryResultLimit_Validating(sender As Object, e As CancelEventArgs) Handles DomainUpDownFactEngineDefaultQueryResultLimit.Validating

        Try
            Me.ErrorProvider.SetError(Me.DomainUpDownFactEngineDefaultQueryResultLimit, "")
            Me.ErrorProvider.SetError(Me.button_okay, "")
            If Me.DomainUpDownFactEngineDefaultQueryResultLimit.Text = "Infinite" Then
            Else
                Dim liDummyInt = CInt(Me.DomainUpDownFactEngineDefaultQueryResultLimit.Text)

                If liDummyInt < 0 Then Throw New Exception("Dummy Exception")
            End If
        Catch
            Me.ErrorProvider.SetError(Me.DomainUpDownFactEngineDefaultQueryResultLimit, "Invalid limit. Enter an integer or 'Infinite'")
            Me.TabPage3.Show()
            Me.TabControl1.SelectedTab = Me.TabPage3
        End Try

    End Sub

    Private Sub CheckBoxSuperuserMode_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxSuperuserMode.CheckedChanged

        If Me.CheckBoxSuperuserMode.Checked Then
            Dim lsMessage = "Use Superuser Mode with caution. Superuser Mode allows you to modify a model in such a way that it may become unusable."
            lsMessage &= vbCrLf & vbCrLf & "Please contact support if you have any questions."
            MsgBox(lsMessage, MsgBoxStyle.Exclamation)

            Call frmMain.ShowHideMenuOptions()

            Call Me.SetupUsingSuperUserMode()

        End If

    End Sub

    Private Sub GroupBox4_Click(sender As Object, e As EventArgs) Handles GroupBox4.Click

        Try
            mbSuperUserModeClicks += 1

            If mbSuperUserModeClicks = 7 Then
                mbSuperUserModeClicks = 0
                Me.CheckBoxSuperuserMode.Enabled = True
                Me.ButtonReplaceCoreMetamodel.Visible = True
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonReplaceCoreMetamodel_Click(sender As Object, e As EventArgs) Handles ButtonReplaceCoreMetamodel.Click

        Try
            Call DatabaseUpgradeFunctions.ReplaceCoreModel(True)
            Boston.WriteToStatusBar("")
            MsgBox("Core Metamodel Replaced.")
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonCopyModelIdToClipboard_Click(sender As Object, e As EventArgs) Handles ButtonCopyModelIdToClipboard.Click

        System.Windows.Forms.Clipboard.SetText(Boston.GetConfigFileLocation)

        Dim lfrmFlashCard As New frmFlashCard
        lfrmFlashCard.ziIntervalMilliseconds = 1500
        lfrmFlashCard.zsText = "Saved to clipboard."
        lfrmFlashCard.Show(frmMain, "White")

    End Sub

    Private Sub TextBoxBostonServerPortNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxBostonServerPortNumber.KeyPress

        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub ButtonImportConfigurationTableItems_Click(sender As Object, e As EventArgs) Handles ButtonImportConfigurationTableItems.Click

        Dim lsMessage As String

        Try
            If Me.DialogOpenFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                'Deserialize text file to a new object.
                Dim objStreamReader As New StreamReader(Me.DialogOpenFile.FileName)
                Dim lrReferenceTable As New ReferenceTable
                Dim x As New XmlSerializer(GetType(ReferenceTable))
                lrReferenceTable = x.Deserialize(objStreamReader)
                objStreamReader.Close()

                If TableReferenceTable.ExistsReferenceTableByName(lrReferenceTable.Name) Then
                    lsMessage = "Are you sure that you want to update the Reference Table, " & lrReferenceTable.Name & "?"
                Else
                    lsMessage = "Are you sure that you want to import the Reference Table, " & lrReferenceTable.Name & "?"
                End If

                If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                    If Not TableReferenceTable.ExistsReferenceTableByName(lrReferenceTable.Name) Then
                        Call TableReferenceTable.AddReferenceTable(lrReferenceTable)

                        Dim liInd = 1
                        For Each lrKeyValuePair In lrReferenceTable.ReferenceTuple(0).KeyValuePair
                            Dim lrReferenceField As New tReferenceField(lrReferenceTable.ReferenceTableId, liInd, lrKeyValuePair.Key, 3, 100, False, False)
                            Call tableReferenceField.AddReferenceField(lrReferenceField)
                            liInd += 1
                        Next
                    Else
                        Dim liInd = 1
                        For Each lrKeyValuePair In lrReferenceTable.ReferenceTuple(0).KeyValuePair
                            Dim lrReferenceField As New tReferenceField(lrReferenceTable.ReferenceTableId, liInd, lrKeyValuePair.Key, 3, 100, False, False)
                            tableReferenceField.CreateReferenceFieldIfNotExists(lrReferenceField)
                            liInd += 1
                        Next
                    End If

                    For Each lrReferenceTuple In lrReferenceTable.ReferenceTuple
                        For Each lrKeyValuePair In lrReferenceTuple.KeyValuePair
                            Dim lrReferenceFieldValue As New tReferenceFieldValue(lrReferenceTable.ReferenceTableId, 1, lrReferenceTuple.RowId, lrKeyValuePair.Value)
                            lrReferenceFieldValue.ReferenceFieldId = tableReferenceField.GetReferenceTableFieldIdByLabel(lrReferenceTable.ReferenceTableId, lrKeyValuePair.Key)
                            Call TableReferenceFieldValue.AddReferenceFieldValue(lrReferenceFieldValue, True)
                        Next
                    Next

                    prApplication.ThrowErrorMessage("Configuration loaded successfully. Reference Table Name: " & lrReferenceTable.Name, pcenumErrorType.Warning, abThrowtoMSGBox:=True, abUseFlashCard:=True)
                End If

            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxDatabaseType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxDatabaseType.SelectedIndexChanged

        Try
            Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                Case Is = pcenumDatabaseType.SQLite,
                          pcenumDatabaseType.MSJet
                    Me.ButtonFileSelect.Visible = True
                    Me.ButtonFileSelect.Enabled = True
                Case Else
                    Me.ButtonFileSelect.Visible = False
                    Me.ButtonFileSelect.Enabled = False
            End Select
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonFileSelect_Click(sender As Object, e As EventArgs) Handles ButtonFileSelect.Click

        Try
            Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                Case Is = pcenumDatabaseType.SQLite
                    Using lrOpenFileDialog As New OpenFileDialog

                        If lrOpenFileDialog.ShowDialog = DialogResult.OK Then
                            Dim lsReturnMessage As String = Nothing
                            Dim lsConnectionString = "Data Source=" & lrOpenFileDialog.FileName & ";Version=3;"
                            If Me.checkDatabaseConnectionString(lsReturnMessage, lsConnectionString) Then
                                Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
                                Me.msConnectionString = lsConnectionString
                                Me.msDatabaseType = Me.ComboBoxDatabaseType.Text
                            Else
                                MsgBox("The file you selected is not a SQLite database.")
                            End If
                        End If

                    End Using
                Case Is = pcenumDatabaseType.MSJet
                    Using lrOpenFileDialog As New OpenFileDialog

                        If lrOpenFileDialog.ShowDialog = DialogResult.OK Then
                            Dim lsReturnMessage As String = Nothing
                            Dim lsConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & lrOpenFileDialog.FileName
                            If Me.checkDatabaseConnectionString(lsReturnMessage, lsConnectionString) Then
                                Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
                                Me.msConnectionString = lsConnectionString
                                Me.msDatabaseType = Me.ComboBoxDatabaseType.Text
                            Else
                                MsgBox("The file you selected is not a MSJet database.")
                            End If
                        End If

                    End Using
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CheckBoxUseThreadingDatabaseLoad_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxUseThreadingDatabaseLoad.CheckedChanged

        Try
            If Me.CheckBoxUseThreadingDatabaseLoad.Checked Then
                Dim lsMessage = "For some database types it is recommended that Threading is turned off for Boston database loading."
                lsMessage.AppendDoubleLineBreak("E.g. Databases such as SQLite may lock during database load if threading is turned on.")
                lsMessage.AppendDoubleLineBreak("Consult with FactEngine if you are unsure as to how to proceed with this option.")
                MsgBox(lsMessage)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class