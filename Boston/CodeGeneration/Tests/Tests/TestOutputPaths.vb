Imports System
Imports System.Text
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class TestOutputPaths

    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "Additional test attributes"
    '
    ' You can use the following additional attributes as you write your tests:
    '
    ' Use ClassInitialize to run code before running the first test in the class
    ' <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    ' End Sub
    '
    ' Use ClassCleanup to run code after all tests in a class have run
    ' <ClassCleanup()> Public Shared Sub MyClassCleanup()
    ' End Sub
    '
    ' Use TestInitialize to run code before running each test
    ' <TestInitialize()> Public Sub MyTestInitialize()
    ' End Sub
    '
    ' Use TestCleanup to run code after each test has run
    ' <TestCleanup()> Public Sub MyTestCleanup()
    ' End Sub
    '
#End Region


    '#Region "setup"

    '    Private Sub SetupDataSource()
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("SELECT DISTINCT ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.TABLES.TABLE_NAME                AS TABLE_NAME,")
    '        sb.AppendLine("    INFORMATION_SCHEMA.TABLES.TABLE_TYPE                AS TABLE_TYPE,")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME              AS COLUMN_NAME, ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS.DATA_TYPE                AS DATA_TYPE, ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS.ORDINAL_POSITION         AS ORDINAL_POSITION, ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS.CHARACTER_MAXIMUM_LENGTH AS CHARACTER_MAXIMUM_LENGTH, ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS.NUMERIC_PRECISION        AS NUMERIC_PRECISION, ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS.NUMERIC_SCALE            AS NUMERIC_SCALE, ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS.IS_NULLABLE              AS IS_NULLABLE, ")
    '        sb.AppendLine("    ISNULL(ColumnProperty(Object_ID(QuoteName(KEY_COLUMN_USAGE.TABLE_SCHEMA) + '.' + QuoteName(KEY_COLUMN_USAGE.TABLE_NAME)), ")
    '        sb.AppendLine("                                              KEY_COLUMN_USAGE.COLUMN_NAME, 'IsIdentity'), 0) ")
    '        sb.AppendLine("                                                        AS IS_IDENTITY, ")
    '        sb.AppendLine("    (SELECT TOP 1 INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_TYPE ")
    '        sb.AppendLine("     FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS ")
    '        sb.AppendLine("     INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ON INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME = INFORMATION_SCHEMA.TABLE_CONSTRAINTS.TABLE_NAME ")
    '        sb.AppendLine("     AND INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_NAME ")
    '        sb.AppendLine("     WHERE INFORMATION_SCHEMA.TABLE_CONSTRAINTS.TABLE_NAME = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME ")
    '        sb.AppendLine("     AND INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME = INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME) ")
    '        sb.AppendLine("                                                        AS CONSTRAINT_TYPE ")
    '        sb.AppendLine("FROM ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.TABLES ")
    '        sb.AppendLine("LEFT JOIN ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS ON INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = INFORMATION_SCHEMA.TABLES.TABLE_NAME ")
    '        sb.AppendLine("LEFT JOIN ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.KEY_COLUMN_USAGE ON INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME ")
    '        sb.AppendLine("    AND ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME = INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME ")
    '        sb.AppendLine("WHERE ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.TABLES.TABLE_NAME <> 'sysdiagrams' ")
    '        sb.AppendLine("AND ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.TABLES.TABLE_NAME <> 'sysmessages' ")
    '        sb.AppendLine("AND ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.TABLES.TABLE_NAME <> 'sysobjects' ")
    '        sb.AppendLine("ORDER BY ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.TABLES.TABLE_NAME, ")
    '        sb.AppendLine("    INFORMATION_SCHEMA.COLUMNS.ORDINAL_POSITION")

    '        Dim source As New Boston.Parser.Source.Source()
    '        source.Name = "Source"
    '        source.ConnectionString = "Data Source=sgc\sqlexpress;Initial Catalog=asm;Integrated Security=True;"
    '        source.Provider = "SQLSERVER"
    '        source.SchemaQuery = sb.ToString

    '        Boston.Parser.PackageBuilder.ClearSources()
    '        Boston.Parser.PackageBuilder.AddSource(source)
    '    End Sub

    '    Private Sub SetupSingleTemplate()
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("<<!header")
    '        sb.AppendLine("    is TestTemplate")
    '        sb.AppendLine("    method single")
    '        sb.AppendLine("    return @@BasePath + ""destination.txt""")
    '        sb.AppendLine("end!>>")
    '        sb.AppendLine("<<!for table tbl in @@MainConn!>>")
    '        sb.AppendLine("<<!for column col in tbl!>>")
    '        sb.AppendLine("  <<!tbl!>> - <<!col!>>")
    '        sb.AppendLine("<<!end!>>")
    '        sb.AppendLine("<<!end!>>")
    '        Me.pkgBuild.AddTemplate("TestTemplate", sb.ToString)
    '    End Sub

    '    Private Sub SetupForTemplate()
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("<<!header")
    '        sb.AppendLine("    is TestTemplate")
    '        sb.AppendLine("    method for table tablevar in @@MainConn")
    '        sb.AppendLine("    return @@BasePath + ""Tables\"" + tablevar + "".txt""")
    '        sb.AppendLine("end!>>")
    '        sb.AppendLine("<<!for table tbl in @@MainConn!>>")
    '        sb.AppendLine("<<!for column col in tbl!>>")
    '        sb.AppendLine("  <<!tbl!>> - <<!col!>>")
    '        sb.AppendLine("<<!end!>>")
    '        sb.AppendLine("<<!end!>>")
    '        Me.pkgBuild.AddTemplate("TestTemplate", sb.ToString)
    '    End Sub

    '#End Region


    '    Private WithEvents pkgBuild As Boston.Parser.PackageBuilder


    '    <TestMethod()> Public Sub NewSingle_NoDirectivesBasePath()
    '        'Set up with no base output path (new project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupSingleTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = """"")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("destination.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub NewSingle_WithDirectivesBasePath()
    '        'Set up with no base output path (new project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupSingleTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = ""Fred\""")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("Fred\destination.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub OpenSingle_NoDirectivesBasePath()
    '        'Set up with base output path (opened project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "c:\temp")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupSingleTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = """"")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("c:\temp\destination.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub OpenSingle_NoDirectivesBasePath_WithLongPath()
    '        'Set up with base output path (opened project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "c:\temp\temp\temp")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupSingleTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = """"")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("c:\temp\temp\temp\destination.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub OpenSingle_NoDirectivesBasePath_WithFullFilePath()
    '        'Set up with base output path (opened project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "c:\temp\test.txt")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupSingleTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = """"")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("c:\temp\destination.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub OpenSingle_WithDirectivesBasePath()
    '        'Set up with base output path (opened project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "c:\temp")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupSingleTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = ""Fred\""")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("c:\temp\Fred\destination.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub NewFor_NoDirectivesBasePath()
    '        'Set up with no base output path (new project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupForTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = """"")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("Tables\AccessLevel.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub NewFor_WithDirectivesBasePath()
    '        'Set up with no base output path (new project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupForTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = ""Fred\""")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("Fred\Tables\AccessLevel.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub OpenFor_NoDirectivesBasePath()
    '        'Set up with base output path (opened project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "c:\temp")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupForTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = """"")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("c:\temp\Tables\AccessLevel.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub OpenFor_WithDirectivesBasePath()
    '        'Set up with base output path (opened project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "c:\temp")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupForTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = ""Fred\""")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("c:\temp\Fred\Tables\AccessLevel.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub OpenSingle_WithRelativeDirectivesBasePath()
    '        'Set up with base output path (opened project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "c:\temp")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupSingleTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = ""..\Fred\""")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        Assert.AreEqual("c:\temp\..\Fred\destination.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


    '    <TestMethod()> Public Sub OpenFor_WithRelativeDirectivesBasePath()
    '        'Set up with base output path (opened project)
    '        Me.pkgBuild = New Boston.Parser.PackageBuilder("", "c:\temp")
    '        Call Me.SetupDataSource()
    '        Call Me.SetupForTemplate()

    '        'Set directives
    '        Dim sb As New System.Text.StringBuilder()
    '        sb.AppendLine("set @@MainConn = sources.Source")
    '        sb.AppendLine("set @@BasePath = ""..\Fred\""")
    '        sb.AppendLine("call TestTemplate")
    '        Me.pkgBuild.SetDirectives(sb.ToString)

    '        'Run
    '        Me.pkgBuild.Start()
    '        While Me.pkgBuild.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
    '        End While
    '        If Me.pkgBuild.ParseException IsNot Nothing Then Throw Me.pkgBuild.ParseException
    '        Assert.AreEqual("c:\temp\..\Fred\Tables\AccessLevel.txt", Me.pkgBuild.Output.Item(0).Path, False)
    '    End Sub


End Class
