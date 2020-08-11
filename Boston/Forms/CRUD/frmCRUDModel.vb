Imports System.Xml.Serialization
Imports System.Xml.Linq
Imports System.IO
Imports System.Reflection

Public Class frmCRUDModel

    Public zrModel As FBM.Model

    'Sample ConnectionStrings
    '   "Driver={SQL Server};Server=(local);Trusted_Connection=Yes;Database=AdventureWorks;"
    '   "Driver={Microsoft ODBC for Oracle};Server=ORACLE8i7;Persist Security Info=False;Trusted_Connection=Yes"
    '   "Driver={Microsoft Access Driver (*.mdb)};DBQ=c:\bin\Northwind.mdb"
    '   "Driver={Microsoft Excel Driver (*.xls)};DBQ=c:\bin\book1.xls"
    '   "Driver={Microsoft Text Driver (*.txt; *.csv)};DBQ=c:\bin"
    '   "DSN=dsnname"

    Private Sub SetupForm()

        Me.LabelModelName.Text = Me.zrModel.Name

        Call Me.LoadDatabaseTypes()

        Me.TextBoxDatabaseConnectionString.Text = Me.zrModel.TargetDatabaseConnectionString

        If Not Me.zrModel.IsEmpty Then
            Me.Button1.Enabled = False
            Me.Button2.Enabled = False
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

            Me.zrModel.TargetDatabaseType = Trim(Me.ComboBoxDatabaseType.SelectedItem)
            Me.zrModel.TargetDatabaseConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)

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
        Dim llo_strategy_terms As New List(Of Object)
        Dim liReferenceTableId As Integer = 0
        Dim liInd As Integer = 0
        Dim liNewIndex As Integer = 0

        Me.ComboBoxDatabaseType.Items.Add("Not set")
        Me.ComboBoxDatabaseType.SelectedIndex = 0

        If pdbConnection.State <> 0 Then
            liReferenceTableId = TableReferenceTable.GetReferenceTableIdByName("DatabaseType")
            llo_strategy_terms = TableReferenceFieldValue.GetReferenceFieldValueTuples(liReferenceTableId, loWorkingClass)

            For liInd = 1 To llo_strategy_terms.Count
                liNewIndex = Me.ComboBoxDatabaseType.Items.Add(llo_strategy_terms(liInd - 1).DatabaseType)
                If llo_strategy_terms(liInd - 1).DatabaseType = Trim(Me.zrModel.TargetDatabaseType) Then
                    Me.ComboBoxDatabaseType.SelectedIndex = liNewIndex
                End If
            Next
        Else
            Me.ComboBoxDatabaseType.Items.Add(pcenumDatabaseType.MSJet.ToString)
            Me.ComboBoxDatabaseType.Items.Add(pcenumDatabaseType.SQLServer.ToString)

            Me.ComboBoxDatabaseType.SelectedIndex = Me.ComboBoxDatabaseType.FindString(Me.zrModel.TargetDatabaseType)
        End If

    End Sub

    Private Function checkDatabaseConnectionString(ByRef asReturnMessage As String) As Boolean

        Dim lsDatabaseLocation, lsDataProvider As String

        Try
            Dim lrSQLConnectionStringBuilder As System.Data.Common.DbConnectionStringBuilder = Nothing

            Try
                lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True) With {
                   .ConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)
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
                Select Case Trim(Me.ComboBoxDatabaseType.SelectedItem)
                    Case Is = "SQLite"
                        If Database.SQLiteDatabase.CreateConnection(Me.TextBoxDatabaseConnectionString.Text) Is Nothing Then
                            Throw New Exception("Can't connect to the database with that connection string.")
                        End If
                    Case Is = "MSJet"
                        Dim ldbConnection As New ADODB.Connection
                        Call ldbConnection.Open(Trim(Me.TextBoxDatabaseConnectionString.Text))
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
            Select Case Trim(Me.ComboBoxDatabaseType.SelectedItem)
                Case Is = "SQLite"
                    Dim lsReturnMessage As String = Nothing
                    If Not Me.checkDatabaseConnectionString(lsReturnMessage) Then
                        MsgBox(lsReturnMessage)
                        Exit Sub
                    End If


                    Me.LabelOpenSuccessfull.Visible = True
                    If Database.SQLiteDatabase.CreateConnection(Me.TextBoxDatabaseConnectionString.Text) Is Nothing Then
                        Me.LabelOpenSuccessfull.ForeColor = Color.Red
                        Me.LabelOpenSuccessfull.Text = "Fail"
                    Else
                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                    End If

                Case Else

                    Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                    Me.LabelOpenSuccessfull.Text = "Testing Connection"
                    Me.LabelOpenSuccessfull.Visible = True

                    lrODBCConnection.Open()

                    Me.LabelOpenSuccessfull.ForeColor = Color.Green
                    Me.LabelOpenSuccessfull.Text = "Success"

                    lrODBCConnection.Close()

            End Select

        Catch ex As Exception

            Me.LabelOpenSuccessfull.ForeColor = Color.Red
            Me.LabelOpenSuccessfull.Text = "Fail"
            Me.LabelOpenSuccessfull.Visible = True

        End Try



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try
            Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

            lrODBCConnection.Open()

            Me.LabelOpenSuccessfull.Text = "Success"
            Me.LabelOpenSuccessfull.Visible = True

            'Dim lrODBCTable As System.Data.DataTable
            'lrODBCTable = lrODBCConnection.GetSchema("Restrictions")
            'Call Me.DisplayData(lrODBCTable)

            Me.zrModel.RDS = New RDS.Model(Me.zrModel)

            Call Me.GetDataTypes()

            Call Me.GetSchemas()

            Me.ErrorProvider.SetError(Me.ComboBoxSchema, "Select a Schema from which to import Tables.")

            Me.Button1.Enabled = False

        Catch ex As Exception

            Me.LabelOpenSuccessfull.Text = "Fail"
            Me.LabelOpenSuccessfull.Visible = True

        End Try

    End Sub

    Private Sub DisplayData(ByRef table As DataTable)

        For Each row As DataRow In table.Rows
            For Each col As DataColumn In table.Columns
                MsgBox(col.ColumnName & " = " & row(col))
            Next
        Next

    End Sub

    Private Sub GetIndexes()
        Try
            Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)
            Dim lrODBCTable As System.Data.DataTable
            Dim lrTable As RDS.Table
            Dim lrIndex As RDS.Index

            lrODBCConnection.Open()

            For Each lrTable In Me.zrModel.RDS.Table
                lrODBCTable = lrODBCConnection.GetSchema("Indexes", New String() {Nothing, Me.ComboBoxSchema.SelectedItem, lrTable.Name, Nothing})

                Call Me.DisplayData(lrODBCTable)

                For Each lrRow As DataRow In lrODBCTable.Rows
                    lrIndex = lrTable.Index.Find(Function(x) x.Name = lrRow(lrODBCTable.Columns("INDEX_NAME")))

                    If lrIndex Is Nothing Then
                        lrIndex = New RDS.Index(lrTable, lrRow(lrODBCTable.Columns("INDEX_NAME")))
                        lrTable.Index.AddUnique(lrIndex)
                        Me.zrModel.RDS.Index.AddUnique(lrIndex)

                        lrIndex.NonUnique = CBool(lrRow(lrODBCTable.Columns("NON_UNIQUE")))
                        lrIndex.IndexQualifier = lrRow(lrODBCTable.Columns("INDEX_QUALIFIER"))

                        Select Case lrRow(lrODBCTable.Columns("TYPE"))
                            Case Is = 1
                                lrIndex.IsPrimaryKey = True
                        End Select

                        If Me.zrModel.RDS.TargetDatabaseType = pcenumDatabaseType.MSJet Then
                            If lrIndex.Name = "PrimaryKey" Then
                                lrIndex.IsPrimaryKey = True
                            End If
                        End If

                        lrIndex.Type = lrRow(lrODBCTable.Columns("TYPE"))

                        Select Case lrRow(lrODBCTable.Columns("ASC_OR_DESC"))
                            Case Is = "A"
                                lrIndex.AscendingOrDescending = pcenumODBCAscendingOrDescending.Ascending
                            Case Is = "D"
                                lrIndex.AscendingOrDescending = pcenumODBCAscendingOrDescending.Descending
                        End Select
                        lrIndex.Cardinality = NullVal(lrRow(lrODBCTable.Columns("CARDINALITY")), 0)
                        lrIndex.Pages = NullVal(lrRow(lrODBCTable.Columns("PAGES")), 0)
                        lrIndex.FilterCondition = NullVal(lrRow(lrODBCTable.Columns("FILTER_CONDITION")), "")
                    End If

                    lrIndex.Column.Add(lrTable.Column.Find(Function(x) x.Name = lrRow(lrODBCTable.Columns("COLUMN_NAME"))))
                Next
            Next

            lrODBCConnection.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub GetDataTypes()

        Try
            Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)
            Dim lrODBCTable As System.Data.DataTable

            lrODBCConnection.Open()

            lrODBCTable = lrODBCConnection.GetSchema("DataTypes")

            For Each lrRow As DataRow In lrODBCTable.Rows

                Dim lrDataType As New RDS.DataType

                lrDataType.TypeName = lrRow(lrODBCTable.Columns("TypeName"))
                lrDataType.ProviderDBType = lrRow(lrODBCTable.Columns("ProviderDbType"))
                lrDataType.ColumnSize = lrRow(lrODBCTable.Columns("ColumnSize"))
                lrDataType.CreateFormat = NullVal(lrRow(lrODBCTable.Columns("CreateFormat")), "")
                lrDataType.CreateParameters = NullVal(lrRow(lrODBCTable.Columns("CreateParameters")), "")
                lrDataType.DataType = lrRow(lrODBCTable.Columns("Datatype"))
                lrDataType.IsAutoIncrementable = NullVal(lrRow(lrODBCTable.Columns("IsAutoIncrementable")), False)
                lrDataType.IsBestMatch = NullVal(lrRow(lrODBCTable.Columns("IsBestMatch")), False)
                lrDataType.IsCaseSensitive = NullVal(lrRow(lrODBCTable.Columns("IsCaseSensitive")), False)
                lrDataType.IsFixedLength = NullVal(lrRow(lrODBCTable.Columns("IsFixedLength")), False)
                lrDataType.IsFixedPrecisionScale = NullVal(lrRow(lrODBCTable.Columns("IsFixedPrecisionScale")), False)
                lrDataType.IsLong = NullVal(lrRow(lrODBCTable.Columns("IsLong")), False)
                lrDataType.IsNullable = NullVal(lrRow(lrODBCTable.Columns("IsNullable")), False)
                lrDataType.IsSearchable = NullVal(lrRow(lrODBCTable.Columns("IsSearchable")), False)
                lrDataType.IsSearchableWithLike = NullVal(lrRow(lrODBCTable.Columns("IsSearchableWithLike")), False)
                lrDataType.IsUnsigned = NullVal(lrRow(lrODBCTable.Columns("IsUnsigned")), False)
                lrDataType.MaximumScale = NullVal(lrRow(lrODBCTable.Columns("MaximumScale")), 0)
                lrDataType.MinimumScale = NullVal(lrRow(lrODBCTable.Columns("MinimumScale")), 0)
                lrDataType.IsConcurrencyType = NullVal(lrRow(lrODBCTable.Columns("IsConcurrencyType")), False)
                lrDataType.IsLiteralSupported = NullVal(lrRow(lrODBCTable.Columns("IsLiteralSupported")), False)
                lrDataType.LiteralPrefix = NullVal(lrRow(lrODBCTable.Columns("LiteralPrefix")), "")
                lrDataType.LiteralSuffix = NullVal(lrRow(lrODBCTable.Columns("LiteralSuffix")), "")
                lrDataType.SQLtype = NullVal(lrRow(lrODBCTable.Columns("SQLType")), 0)

                Me.zrModel.RDS.DataType.Add(lrDataType)
            Next

            lrODBCConnection.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub GetColumns()

        Try
            Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)
            Dim lrODBCTable As System.Data.DataTable
            Dim lrTable As RDS.Table
            Dim lrColumn As RDS.Column

            lrODBCConnection.Open()

            lrODBCTable = lrODBCConnection.GetSchema("Columns", New String() {Nothing, Me.ComboBoxSchema.SelectedItem, Nothing, Nothing})

            For Each lrRow As DataRow In lrODBCTable.Rows
                lrTable = Me.zrModel.RDS.Table.Find(Function(x) x.Name = Trim(lrRow(lrODBCTable.Columns("TABLE_NAME"))))
                If lrTable IsNot Nothing Then
                    lrColumn = New RDS.Column(lrTable, lrRow(lrODBCTable.Columns("COLUMN_NAME")), Nothing, Nothing)

                    lrColumn.OrdinalPosition = lrRow(lrODBCTable.Columns("ORDINAL_POSITION"))
                    lrColumn.DataType = Me.zrModel.RDS.DataType.Find(Function(x) x.ProviderDBType = CInt(lrRow(lrODBCTable.Columns("DATA_TYPE"))))
                    lrColumn.ODBCDataType = lrRow(lrODBCTable.Columns("DATA_TYPE"))
                    lrColumn.DataTypeName = lrRow(lrODBCTable.Columns("TYPE_NAME"))
                    lrColumn.Nullable = CBool(lrRow(lrODBCTable.Columns("NULLABLE")))
                    lrColumn.IsNullable = lrRow(lrODBCTable.Columns("IS_NULLABLE")) = "YES"

                    lrColumn.TableCategory = lrRow(lrODBCTable.Columns("TABLE_CAT"))
                    lrColumn.TableSchema = NullVal(lrRow(lrODBCTable.Columns("TABLE_SCHEM")), "")
                    lrColumn.ColumnSize = CInt(lrRow(lrODBCTable.Columns("COLUMN_SIZE")))
                    lrColumn.BufferLength = CInt(NullVal(lrRow(lrODBCTable.Columns("BUFFER_LENGTH")), 0))
                    lrColumn.DecimalDigits = CInt(NullVal(lrRow(lrODBCTable.Columns("DECIMAL_DIGITS")), 0))
                    lrColumn.NumPrecRadix = CInt(NullVal(lrRow(lrODBCTable.Columns("NUM_PREC_RADIX")), 0))

                    lrColumn.Remarks = NullVal(lrRow(lrODBCTable.Columns("REMARKS")), "")
                    lrColumn.ColumnDef = NullVal(lrRow(lrODBCTable.Columns("COLUMN_DEF")), "")
                    lrColumn.SQLDataType = NullVal(lrRow(lrODBCTable.Columns("SQL_DATA_TYPE")), 0)
                    lrColumn.SQLDateTimeSub = NullVal(lrRow(lrODBCTable.Columns("SQL_DATETIME_SUB")), "")
                    lrColumn.CharOctetLength = NullVal(lrRow(lrODBCTable.Columns("CHAR_OCTET_LENGTH")), 0)
                    lrColumn.SSDataType = NullVal(lrRow(lrODBCTable.Columns("SQL_DATA_TYPE")), 0)

                    lrTable.Column.Add(lrColumn)
                End If
            Next

            lrODBCConnection.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub GetSchemas()

        Try
            Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)
            Dim lrODBCTable As System.Data.DataTable
            Dim lasSchemaName As New List(Of String)

            lrODBCConnection.Open()

            If lrODBCConnection.DataSource = "ACCESS" Then
                'Table Schemas don't exist for Access databases.
            Else
                lrODBCTable = lrODBCConnection.GetSchema("Tables")

                For Each lrRow As DataRow In lrODBCTable.Rows

                    If Not lasSchemaName.Contains(lrRow(lrODBCTable.Columns("TABLE_SCHEM"))) Then
                        lasSchemaName.Add(lrRow(lrODBCTable.Columns("TABLE_SCHEM")))
                    End If
                Next

                For Each lsSchemaName As String In lasSchemaName
                    Me.ComboBoxSchema.Items.Add(lsSchemaName)
                Next
            End If

            lrODBCConnection.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Try
            Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)
            Dim lrODBCTable As System.Data.DataTable
            Dim lasSchemaName As New List(Of String)
            Dim lrPage As FBM.Page
            Dim lrTable As RDS.Table

            Me.zrModel.RDS.TargetDatabaseType = DirectCast(System.[Enum].Parse(GetType(pcenumDatabaseType), Me.ComboBoxDatabaseType.SelectedItem), pcenumDatabaseType)

            lrODBCConnection.Open()

            lrODBCTable = lrODBCConnection.GetSchema("Tables")

            Select Case lrODBCConnection.DataSource
                Case Is = "ACCESS"
                    For Each lrRow As DataRow In lrODBCTable.Rows
                        lrTable = New RDS.Table(Me.zrModel.RDS, lrRow(lrODBCTable.Columns("TABLE_NAME")), Nothing)
                        Select Case lrRow(lrODBCTable.Columns("TABLE_TYPE"))
                            Case Is = "SYSTEM TABLE"
                                lrTable.IsSystemTable = True
                            Case Else
                                lrTable.IsSystemTable = False
                        End Select

                        If Not lrTable.IsSystemTable Then
                            Me.zrModel.RDS.Table.AddUnique(lrTable)
                            lrPage = New FBM.Page(Me.zrModel, Nothing, lrRow(lrODBCTable.Columns("TABLE_NAME")), pcenumLanguage.ORMModel)
                            frmMain.zfrmModelExplorer.AddPageToModel(Me.zrModel.TreeNode, lrPage, True)
                        End If
                    Next
                Case Else
                    For Each lrRow As DataRow In lrODBCTable.Rows
                        If lrRow(lrODBCTable.Columns("TABLE_SCHEM")) = Me.ComboBoxSchema.SelectedItem Then
                            Me.zrModel.RDS.Table.Add(New RDS.Table(Me.zrModel.RDS, lrRow(lrODBCTable.Columns("TABLE_NAME")), Nothing))
                            lrPage = New FBM.Page(Me.zrModel, Nothing, lrRow(lrODBCTable.Columns("TABLE_NAME")), pcenumLanguage.ORMModel)
                            frmMain.zfrmModelExplorer.AddPageToModel(Me.zrModel.TreeNode, lrPage, True)
                        End If
                    Next
            End Select

            '------------------------------------------------------------------------------
            'Get the Columns for the tables.
            Call Me.GetColumns()

            '------------------------------------------------------------------------------
            'Get the Indexes for the tables.
            Call Me.GetIndexes()

            '------------------------------------------------------------------------------
            'Create EntityTypes for each Table with a PrimaryKey with one Column.
            For Each lrTable In Me.zrModel.RDS.Table
                If lrTable.HasSingleColumnPrimaryKey Then
                    Dim lrEntityType As FBM.EntityType
                    lrEntityType = New FBM.EntityType(Me.zrModel, pcenumLanguage.ORMModel, lrTable.Name, lrTable.Name)
                    Me.zrModel.AddEntityType(lrEntityType, True)

                    Dim lsValueTypeName As String
                    Dim lsReferenceMode As String = ""
                    Dim lrPrimaryKeyColumn As RDS.Column
                    lrPrimaryKeyColumn = lrTable.Index.Find(Function(x) x.IsPrimaryKey = True).Column(0)
                    lsValueTypeName = lrPrimaryKeyColumn.Name

                    Dim items As Array
                    items = System.Enum.GetValues(GetType(pcenumReferenceModeEndings))
                    Dim item As pcenumReferenceModeEndings
                    For Each item In items
                        If lsValueTypeName.EndsWith(GetEnumDescription(item).Trim({"."c})) Then 'See https://msdn.microsoft.com/en-us/library/kxbw3kwc(v=vs.110).aspx
                            lsReferenceMode = GetEnumDescription(item).Trim({"."c})
                            Exit For
                        Else
                            lsReferenceMode = lsValueTypeName
                        End If
                    Next

                    '-----------------------------------------------------------------------------
                    'Create an EntityTypeInstance for the new EntityType and put it on the Page.
                    lrPage = Me.zrModel.Page.Find(Function(x) x.Name = lrEntityType.Name)
                    Call lrPage.DropEntityTypeAtPoint(lrEntityType, New PointF(50, 50))

                    lrEntityType.SetReferenceMode(lsReferenceMode, False, lsValueTypeName)

                    '=================================================================================================================
                    'Create joined FactTypes.

                    Dim lrFactType As FBM.FactType
                    Dim lrColumn As RDS.Column
                    Dim larModelObject As New List(Of FBM.ModelObject)

                    For Each lrColumn In lrTable.Column.FindAll(Function(x) x.Name <> lrPrimaryKeyColumn.Name)
                        larModelObject.Clear()
                        larModelObject.Add(lrEntityType)

                        Dim lrJoinedModelObject As FBM.ModelObject

                        lrJoinedModelObject = Me.zrModel.FindEntityTypeByValueTypeId(lrColumn.Name)
                        If lrJoinedModelObject Is Nothing Then
                            lrJoinedModelObject = Me.zrModel.CreateValueType(lrColumn.Name)
                        End If

                        larModelObject.Add(lrJoinedModelObject)

                        Dim lrModelObject As FBM.ModelObject
                        Dim lsFactTypeName As String = ""

                        For Each lrModelObject In larModelObject
                            lsFactTypeName &= lrModelObject.Name
                        Next
                        lrFactType = Me.zrModel.CreateFactType(lsFactTypeName, larModelObject, False)

                        Dim larRole As New List(Of FBM.Role)
                        Dim lrRole As FBM.Role

                        For Each lrRole In lrFactType.RoleGroup
                            larRole.Add(lrRole)
                        Next
                        Dim lasPredicatePart As New List(Of String)
                        lasPredicatePart.Add("has")
                        lasPredicatePart.Add("")

                        Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, lasPredicatePart)

                        Call lrFactType.AddFactTypeReading(lrFactTypeReading, False, False)

                        lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lrEntityType.Id)
                        larRole.Clear()
                        larRole.Add(lrRole)

                        Dim lrRoleConstraint As FBM.RoleConstraint

                        lrRoleConstraint = Me.zrModel.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint, _
                                                                         larRole, _
                                                                         "InternalUniquenessConstraint", _
                                                                         1)

                        lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)

                        Dim lrFactTypeInstance As FBM.FactTypeInstance
                        lrFactTypeInstance = lrPage.DropFactTypeAtPoint(lrFactType, New PointF(100, 100), False)
                    Next 'Column
                    '=================================================================================================================

                End If
            Next



            lrODBCConnection.Close()

            Me.Button2.Enabled = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ComboBoxSchema_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxSchema.SelectedIndexChanged

        If Me.ComboBoxSchema.SelectedIndex >= 0 Then
            Me.ErrorProvider.SetError(Me.ComboBoxSchema, "")
        End If

    End Sub

End Class