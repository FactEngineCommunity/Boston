Imports System.Xml.Serialization
Imports System.Xml.Linq
Imports System.IO
Imports System.Reflection
Imports System.Data.Common

Public Class frmCRUDBostonConfiguration

    Sub LoadDebugModes()

        Dim loWorkingClass As New Object
        Dim lloDebugModes As New List(Of Object)
        Dim liReferenceTableId As Integer = 0
        Dim liInd As Integer = 0
        Dim liNewIndex As Integer = 0

        If pdbConnection.State <> 0 Then
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

        Me.CheckBoxThrowInformationDebugMessagesToScreen.Checked = My.Settings.ThrowInformationDebugMessagesToScreen
        Me.CheckBoxThrowCriticalDebugMessagesToScreen.Checked = My.Settings.ThrowCriticalDebugMessagesToScreen

        '---------------------------------------------------------
        'Virtual Analyst
        '-----------------
        Me.CheckBoxVirtualAnalystDisplayBriana.Checked = My.Settings.DisplayBrianaVirtualAnalyst
        Me.CheckBoxStartVirtualAnalystInQuietMode.Checked = My.Settings.StartVirtualAnalystInQuietMode

        Call Me.LoadDatabaseTypes()

        Me.TextBoxDatabaseConnectionString.Text = My.Settings.DatabaseConnectionString

        Me.LabelConfigurationFileLocation.Text = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
        Me.LabelUserConfigurationFileLocation.Text = Richmond.GetConfigFileLocation

        Me.ComboBoxDatabaseType.Enabled = False

        Me.CheckBoxEnableClientServer.Checked = My.Settings.UseClientServer
        Me.CheckBoxUseRemoteUI.Checked = My.Settings.UseVirtualUI

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

        If check_fields(lsReturnString) Then

            My.Settings.DebugMode = ComboBoxDebugMode.SelectedItem
            My.Settings.DatabaseType = Me.ComboBoxDatabaseType.SelectedItem.ToString
            My.Settings.DatabaseConnectionString = Me.TextBoxDatabaseConnectionString.Text
            My.Settings.DisplayBrianaVirtualAnalyst = Me.CheckBoxVirtualAnalystDisplayBriana.Checked
            My.Settings.StartVirtualAnalystInQuietMode = Me.CheckBoxStartVirtualAnalystInQuietMode.Checked
            My.Settings.ThrowCriticalDebugMessagesToScreen = Me.CheckBoxThrowCriticalDebugMessagesToScreen.Checked
            My.Settings.ThrowInformationDebugMessagesToScreen = Me.CheckBoxThrowInformationDebugMessagesToScreen.Checked
            My.Settings.UseClientServer = Me.CheckBoxEnableClientServer.Checked
            My.Settings.LoggingOutEndsSession = Me.CheckBoxLoggingOutEndsSession.Checked
            My.Settings.UseVirtualUI = Me.CheckBoxUseRemoteUI.Checked

            My.Settings.Save()
            Me.Hide()
            Me.Close()
            Me.Dispose()
        Else
            MsgBox(lsReturnString)
        End If

    End Sub

    Private Function check_fields(ByRef asReturnMessage As String) As Boolean

        If Not Me.checkDatabaseConnectionString(asReturnMessage) Then
            Return False
        End If

        Return True

    End Function

    Private Function checkDatabaseConnectionString(ByRef asReturnMessage As String) As Boolean

        Dim lsDatabaseLocation, lsDataProvider As String

        Try
            Dim lrSQLConnectionStringBuilder As System.Data.Common.DbConnectionStringBuilder = Nothing

            Try
                lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True) With {
                   .ConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)
                }

                lsDatabaseLocation = lrSQLConnectionStringBuilder("Data Source")
                lsDataProvider = lrSQLConnectionStringBuilder("Provider")

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

            If lsDataProvider <> "Microsoft.Jet.OLEDB.4.0" Then
                asReturnMessage = "This version of Boston uses a MSJet database. Please fix the Data Provider in the Database Connection String and try again."
                asReturnMessage &= vbCrLf & vbCrLf & "Try Provider=Microsoft.Jet.OLEDB.4.0"
                Return False
            End If

            Try
                Dim ldbConnection As New ADODB.Connection
                Call ldbConnection.Open(Trim(Me.TextBoxDatabaseConnectionString.Text))

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace)

            Return False
        End Try

    End Function

    Sub LoadDatabaseTypes()

        Dim loWorkingClass As New Object
        Dim llo_strategy_terms As New List(Of Object)
        Dim liReferenceTableId As Integer = 0
        Dim liInd As Integer = 0
        Dim liNewIndex As Integer = 0

        If pdbConnection.State <> 0 Then
            liReferenceTableId = TableReferenceTable.GetReferenceTableIdByName("DatabaseType")
            llo_strategy_terms = TableReferenceFieldValue.GetReferenceFieldValueTuples(liReferenceTableId, loWorkingClass)

            For liInd = 1 To llo_strategy_terms.Count
                liNewIndex = Me.ComboBoxDatabaseType.Items.Add(llo_strategy_terms(liInd - 1).DatabaseType)
                If llo_strategy_terms(liInd - 1).DatabaseType = My.Settings.DatabaseType Then
                    Me.ComboBoxDatabaseType.SelectedIndex = liNewIndex
                End If
            Next
        Else
            Me.ComboBoxDatabaseType.Items.Add(pcenumDatabaseType.MSJet.ToString)
            Me.ComboBoxDatabaseType.Items.Add(pcenumDatabaseType.SQLServer.ToString)

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

        '    If Richmond.IsSerializable(lrModel) Then

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
        '    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

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

End Class