Imports System.Xml.Serialization
Imports System.Xml.Linq
Imports System.IO
Imports System.Reflection
Imports System.Configuration
Imports System.Data.SQLite
Imports ADOX

Public Class frmCRUDModel

    Public WithEvents zrModel As FBM.Model

    Private mrODBCConnection As System.Data.Odbc.OdbcConnection

    'Sample ConnectionStrings
    '   "Driver={SQL Server};Server=(local);Trusted_Connection=Yes;Database=AdventureWorks;"
    '   "Driver={Microsoft ODBC for Oracle};Server=ORACLE8i7;Persist Security Info=False;Trusted_Connection=Yes"
    '   "Driver={Microsoft Access Driver (*.mdb)};DBQ=c:\bin\Northwind.mdb"
    '   "Driver={Microsoft Excel Driver (*.xls)};DBQ=c:\bin\book1.xls"
    '   "Driver={Microsoft Text Driver (*.txt; *.csv)};DBQ=c:\bin"
    '   "DSN=dsnname"

    Private Sub SetupForm()

        Me.LabelModelName.Text = Me.zrModel.Name
        Me.LabelCoreVersion.Text = Me.zrModel.CoreVersionNumber

        Call Me.LoadDatabaseTypes()

        Me.TextBoxDatabaseConnectionString.Text = Me.zrModel.TargetDatabaseConnectionString
        Me.CheckBoxIsDatabaseSynchronised.Checked = Me.zrModel.IsDatabaseSynchronised

        If Me.zrModel.IsEmpty Then
            Me.GroupBoxReverseEngineering.Visible = True
            Me.ButtonReverseEngineerDatabase.Enabled = True
        End If

    End Sub

    Private Sub frmCRUDModel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click

        Me.Hide()
        Me.Close()

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        If check_fields() Then

            Me.zrModel.TargetDatabaseType = Me.ComboBoxDatabaseType.SelectedItem.Tag
            Me.zrModel.TargetDatabaseConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)
            Me.zrModel.IsDatabaseSynchronised = Me.CheckBoxIsDatabaseSynchronised.Checked

            Try
                If Me.zrModel.TreeNode IsNot Nothing Then
                    If My.Settings.FactEngineShowDatabaseLogoInModelExplorer Then
                        Select Case Me.zrModel.TargetDatabaseType
                            Case Is = pcenumDatabaseType.MongoDB
                                Me.zrModel.TreeNode.ImageIndex = 6
                                Me.zrModel.TreeNode.SelectedImageIndex = 6
                            Case Is = pcenumDatabaseType.SQLServer
                                Me.zrModel.TreeNode.ImageIndex = 9
                                Me.zrModel.TreeNode.SelectedImageIndex = 9
                            Case Is = pcenumDatabaseType.MSJet
                                Me.zrModel.TreeNode.ImageIndex = 7
                                Me.zrModel.TreeNode.SelectedImageIndex = 7
                            Case Is = pcenumDatabaseType.SQLite
                                Me.zrModel.TreeNode.ImageIndex = 8
                                Me.zrModel.TreeNode.SelectedImageIndex = 8
                            Case Is = pcenumDatabaseType.ODBC
                                Me.zrModel.TreeNode.ImageIndex = 10
                                Me.zrModel.TreeNode.SelectedImageIndex = 10
                            Case Is = pcenumDatabaseType.None
                                Me.zrModel.TreeNode.ImageIndex = 1
                                Me.zrModel.TreeNode.SelectedImageIndex = 1
                        End Select
                    End If
                End If
            Catch ex As Exception
            End Try

            Me.zrModel.Save()

            Me.Hide()
            Me.Close()
            Me.Dispose()
        End If

    End Sub

    Private Function check_fields() As Boolean

        Return True

    End Function

    Sub LoadDatabaseTypes()

        Dim loWorkingClass As New Object
        Dim larDatabaseType As New List(Of Object)
        Dim liReferenceTableId As Integer = 0
        Dim liInd As Integer = 0
        Dim liNewIndex As Integer = 0

        Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.None, pcenumDatabaseType.None.ToString, pcenumDatabaseType.None))
        Me.ComboBoxDatabaseType.SelectedIndex = 0

        If pdbConnection.State <> 0 Then
            liReferenceTableId = TableReferenceTable.GetReferenceTableIdByName("DatabaseType")
            larDatabaseType = TableReferenceFieldValue.GetReferenceFieldValueTuples(liReferenceTableId, loWorkingClass)

            For liInd = 1 To larDatabaseType.Count
                Dim liDatabaseType = CType([Enum].Parse(GetType(pcenumDatabaseType), Viev.NullVal(larDatabaseType(liInd - 1).DatabaseType, pcenumDatabaseType.None)), pcenumDatabaseType)
                Dim lrComboboxItem As New tComboboxItem(larDatabaseType(liInd - 1).DatabaseType, larDatabaseType(liInd - 1).DatabaseType, liDatabaseType)
                liNewIndex = Me.ComboBoxDatabaseType.Items.Add(lrComboboxItem)
                If larDatabaseType(liInd - 1).DatabaseType = Trim(Me.zrModel.TargetDatabaseType.ToString) Then
                    Me.ComboBoxDatabaseType.SelectedIndex = liNewIndex
                End If
            Next
        Else
            Me.ComboBoxDatabaseType.Items.Add(pcenumDatabaseType.SQLite.ToString)
            Me.ComboBoxDatabaseType.Items.Add(pcenumDatabaseType.MSJet.ToString)
            Me.ComboBoxDatabaseType.Items.Add(pcenumDatabaseType.SQLServer.ToString)

            Me.ComboBoxDatabaseType.SelectedIndex = Me.ComboBoxDatabaseType.FindString(Me.zrModel.TargetDatabaseType.ToString)
        End If

    End Sub

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

            Try
                Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                    Case Is = pcenumDatabaseType.SQLite
                        If Database.SQLiteDatabase.CreateConnection(lsConnectionString) Is Nothing Then
                            Throw New Exception("Can't connect to the database with that connection string.")
                        End If
                    Case Is = pcenumDatabaseType.MSJet
                        Dim ldbConnection As New ADODB.Connection
                        Call ldbConnection.Open(lsConnectionString)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Sub ButtonTestConnection_Click(sender As Object, e As EventArgs) Handles ButtonTestConnection.Click

        Try
            Call Me.TestConnection
        Catch ex As Exception

        End Try

    End Sub

    Private Function TestConnection() As Boolean

        Try
            If Trim(Me.TextBoxDatabaseConnectionString.Text) = "" Then
                MsgBox("Please provide a Connection String for the database.")
                Return False
            End If


            With New WaitCursor
                Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                    Case Is = pcenumDatabaseType.SQLite
                        Dim lsReturnMessage As String = Nothing
                        If Not Me.checkDatabaseConnectionString(lsReturnMessage) Then
                            MsgBox(lsReturnMessage)
                            Return False
                        End If


                        Me.LabelOpenSuccessfull.Visible = True
                        If Database.SQLiteDatabase.CreateConnection(Me.TextBoxDatabaseConnectionString.Text) Is Nothing Then
                            Me.LabelOpenSuccessfull.ForeColor = Color.Red
                            Me.LabelOpenSuccessfull.Text = "Fail"
                        Else
                            Me.LabelOpenSuccessfull.ForeColor = Color.Green
                            Me.LabelOpenSuccessfull.Text = "Success"
                        End If

                    Case Is = pcenumDatabaseType.MSJet

                        Dim ldbConnection As New ADODB.Connection
                        Me.LabelOpenSuccessfull.Text = "Testing Connection"
                        Me.LabelOpenSuccessfull.Visible = True

                        ldbConnection.Open(Me.TextBoxDatabaseConnectionString.Text)

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"

                        ldbConnection.Close()

                    Case Is = pcenumDatabaseType.SQLServer
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        Me.LabelOpenSuccessfull.Text = "Testing Connection"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"

                        lrODBCConnection.Close()

                    Case Is = pcenumDatabaseType.MongoDB
                        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("mongosqld --mongo-uri 'mongodb: //university-shard-00-02.8dmfw.azure.mongodb.net:27017,university-shard-00-00.8dmfw.azure.mongodb.net:27017,university-shard-00-01.8dmfw.azure.mongodb.net:27017/?ssl=true&replicaSet=atlas-7kqhl6-shard-0&retryWrites=true&w=majority' --auth - u Viev -p Viev").ConnectionString
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Close()

                    Case Is = pcenumDatabaseType.PostgreSQL
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Close()

                    Case Is = pcenumDatabaseType.ODBC
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Close()
                    Case Else

                        Me.LabelOpenSuccessfull.ForeColor = Color.Red
                        Me.LabelOpenSuccessfull.Text = "Unknown database type, '" & prApplication.WorkingModel.TargetDatabaseType.ToString & "'."
                        Me.LabelOpenSuccessfull.Visible = True

                End Select
            End With

            Return True
        Catch ex As Exception

            Me.LabelOpenSuccessfull.ForeColor = Color.Red
            Me.LabelOpenSuccessfull.Text = "Fail: " & ex.Message
            Me.LabelOpenSuccessfull.Visible = True

            Return False
        End Try

    End Function



    Private Sub AddREMessage(ByVal asMessage As String)

        Try
            Me.RichTextBoxREMessages.AppendText(vbCrLf & asMessage)
        Catch ex As Exception
            Debugger.Break()
        End Try
    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonReverseEngineerDatabase.Click

        Try
            Dim lasSchemaName As New List(Of String)

            With New WaitCursor

                Me.RichTextBoxREMessages.Clear()
                Me.ProgressBarReverseEngineering.Value = 0

                If Not Me.TestConnection Then
                    Call Me.AddREMessage("- Failed to connect to database. Have you set the Database Type and its Connection String?")
                    Exit Sub
                End If

                Me.zrModel.RDS.TargetDatabaseType = Me.ComboBoxDatabaseType.SelectedItem.Tag 'DirectCast(System.[Enum].Parse(GetType(pcenumDatabaseType), Me.ComboBoxDatabaseType.SelectedItem), pcenumDatabaseType)

                Dim lrReverseEngineer As New ODBCDatabaseReverseEngineer(Me.zrModel,
                                                                         Trim(Me.TextBoxDatabaseConnectionString.Text),
                                                                         True,
                                                                         Me.ProgressBarReverseEngineering)

                Call Me.AddREMessage("- Reverse engineering started.")
                Dim lsErrorMessage As String = ""
                If Not lrReverseEngineer.ReverseEngineerDatabase(lsErrorMessage) Then
                    Me.ErrorProvider.SetError(Me.ButtonReverseEngineerDatabase, lsErrorMessage)
                Else
                    Call Me.AddREMessage("- Finished reverse engineering the database.")
                    Call Me.AddREMessage("- Saving the model.")
                    Call Me.zrModel.Save()
                    Call Me.AddREMessage("- Complete.")
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


    Private Sub ComboBoxDatabaseType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxDatabaseType.SelectedIndexChanged

        Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
            Case Is = pcenumDatabaseType.SQLite,
                      pcenumDatabaseType.MSJet
                Me.ButtonCreateDatabase.Visible = True
                Me.ButtonFileSelect.Visible = True
                If Trim(Me.TextBoxDatabaseConnectionString.Text) = "" Then
                    Me.ButtonCreateDatabase.Enabled = True
                Else
                    Me.ButtonCreateDatabase.Enabled = False
                End If
            Case Else
                Me.ButtonCreateDatabase.Visible = False
                Me.ButtonCreateDatabase.Enabled = False
        End Select


    End Sub

    Private Sub ButtonCreateDatabase_Click(sender As Object, e As EventArgs) Handles ButtonCreateDatabase.Click

        Dim lrSaveFileDialog As New SaveFileDialog()

        Select Case ComboBoxDatabaseType.SelectedItem.Tag
            Case Is = pcenumDatabaseType.SQLite
                lrSaveFileDialog.Filter = "SQLite database file (*.db)|*.db"
                lrSaveFileDialog.FilterIndex = 0
                lrSaveFileDialog.RestoreDirectory = True

                If (lrSaveFileDialog.ShowDialog() = DialogResult.OK) Then
                    If Not System.IO.File.Exists(lrSaveFileDialog.FileName()) Then
                        SQLiteConnection.CreateFile(lrSaveFileDialog.FileName)
                        Dim lsConnectionString = "Data Source=" & lrSaveFileDialog.FileName & ";Version=3;"
                        Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
                        Me.ButtonCreateDatabase.Enabled = False
                    End If
                End If
            Case Is = pcenumDatabaseType.MSJet
                lrSaveFileDialog.Filter = "MSJet database file (*.mdb)|*.mdb"
                lrSaveFileDialog.FilterIndex = 0
                lrSaveFileDialog.RestoreDirectory = True

                If (lrSaveFileDialog.ShowDialog() = DialogResult.OK) Then
                    If Not System.IO.File.Exists(lrSaveFileDialog.FileName()) Then

                        Call System.IO.File.Copy(Richmond.MyPath & "\emptydatabases\emptymdbdatabase.mdb", lrSaveFileDialog.FileName)

                        Dim lsConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & lrSaveFileDialog.FileName

                        Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
                        Me.ButtonCreateDatabase.Enabled = False
                    End If
                End If
        End Select

    End Sub

    Private Sub TextBoxDatabaseConnectionString_TextChanged(sender As Object, e As EventArgs) Handles TextBoxDatabaseConnectionString.TextChanged

        If Trim(Me.TextBoxDatabaseConnectionString.Text) = "" Then
            Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                Case Is = pcenumDatabaseType.SQLite
                    Me.ButtonCreateDatabase.Visible = True
                    Me.ButtonCreateDatabase.Enabled = True
                    Me.ButtonFileSelect.Visible = True
            End Select
        End If

        Me.LabelOpenSuccessfull.Text = ""

        Me.ButtonApply.Enabled = True

    End Sub

    Private Sub ButtonFileSelect_Click(sender As Object, e As EventArgs) Handles ButtonFileSelect.Click

        Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
            Case Is = pcenumDatabaseType.SQLite
                Using lrOpenFileDialog As New OpenFileDialog

                    If lrOpenFileDialog.ShowDialog = DialogResult.OK Then
                        Dim lsReturnMessage As String = Nothing
                        Dim lsConnectionString = "Data Source=" & lrOpenFileDialog.FileName & ";Version=3;"
                        If Me.checkDatabaseConnectionString(lsReturnMessage, lsConnectionString) Then
                            Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
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
                        Else
                            MsgBox("The file you selected is not a MSJet database.")
                        End If
                    End If

                End Using
        End Select

    End Sub

    Private Sub ButtonApply_Click(sender As Object, e As EventArgs) Handles ButtonApply.Click

        Try
            If Me.check_fields() Then

                Me.zrModel.TargetDatabaseType = Me.ComboBoxDatabaseType.SelectedItem.Tag
                Me.zrModel.TargetDatabaseConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)
                Me.zrModel.IsDatabaseSynchronised = Me.CheckBoxIsDatabaseSynchronised.Checked

                Try
                    If Me.zrModel.TreeNode IsNot Nothing Then
                        If My.Settings.FactEngineShowDatabaseLogoInModelExplorer Then
                            Select Case Me.zrModel.TargetDatabaseType
                                Case Is = pcenumDatabaseType.MongoDB
                                    Me.zrModel.TreeNode.ImageIndex = 6
                                    Me.zrModel.TreeNode.SelectedImageIndex = 6
                                Case Is = pcenumDatabaseType.SQLServer
                                    Me.zrModel.TreeNode.ImageIndex = 9
                                    Me.zrModel.TreeNode.SelectedImageIndex = 9
                                Case Is = pcenumDatabaseType.MSJet
                                    Me.zrModel.TreeNode.ImageIndex = 7
                                    Me.zrModel.TreeNode.SelectedImageIndex = 7
                                Case Is = pcenumDatabaseType.SQLite
                                    Me.zrModel.TreeNode.ImageIndex = 8
                                    Me.zrModel.TreeNode.SelectedImageIndex = 8
                                Case Is = pcenumDatabaseType.ODBC
                                    Me.zrModel.TreeNode.ImageIndex = 10
                                    Me.zrModel.TreeNode.SelectedImageIndex = 10
                                Case Is = pcenumDatabaseType.None
                                    Me.zrModel.TreeNode.ImageIndex = 1
                                    Me.zrModel.TreeNode.SelectedImageIndex = 1
                            End Select
                        End If
                    End If
                Catch ex As Exception
                End Try

                Me.zrModel.Save()

            End If
            Me.ButtonApply.Enabled = False

        Catch ex As Exception

        End Try
    End Sub

    Private Sub zrModel_Deleting() Handles zrModel.Deleting

        Me.Hide()
        Me.Close()

    End Sub

End Class