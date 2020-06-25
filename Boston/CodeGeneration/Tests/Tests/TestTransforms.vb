Imports System
Imports System.Text
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class TestTransforms

    Private testContextInstance As TestContext

    'Private Shared trans As Boston.Parser.Syntax.SourceTransforms
    'Private Shared src As Boston.Parser.Meta.Database.Column
    'Private Shared srcOwner As Boston.Parser.Meta.Database.Table
    'Private Shared srcParam As Boston.Parser.Meta.Database.Parameter


    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    ' You can use the following additional attributes as you write your tests:
    '
    ' Use ClassInitialize to run code before running the first test in the class
    <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        'trans = New Boston.Parser.Syntax.SourceTransforms("test", TransformsSource)
        'Call trans.BuildForThreadedCompilation()
        'src = New Boston.Parser.Meta.Database.Column(Nothing, New Boston.PluginInterface.Sources.SchemaRow(), Nothing, Nothing, trans)
        'srcOwner = New Boston.Parser.Meta.Database.Table()
        'srcParam = New Boston.Parser.Meta.Database.Parameter(Nothing, New Boston.PluginInterface.Sources.SchemaRow(), Nothing, Nothing, trans)
    End Sub
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

    Public Shared Function TransformsSource() As String
        Dim sb As New System.Text.StringBuilder()
        sb.AppendLine("//Set column property values")
        sb.AppendLine("set column.sqltype = column.datatype")
        sb.AppendLine("set column.sqltypefullspec = column.datatype")
        sb.AppendLine("")
        sb.AppendLine("if column.datatype = ""bit""")
        sb.AppendLine("	set column.vbtype = ""Boolean""")
        sb.AppendLine("	set column.cstype = ""bool""")
        sb.AppendLine("	set column.vbdefval = ""False""")
        sb.AppendLine("	set column.csdefval = ""false""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""int"" or column.datatype = ""int identity""")
        sb.AppendLine("	set column.vbtype = ""Integer""")
        sb.AppendLine("	set column.cstype = ""int""")
        sb.AppendLine("	set column.vbdefval = 0")
        sb.AppendLine("	set column.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""bigint"" or column.datatype = ""bigint identity""")
        sb.AppendLine("	set column.vbtype = ""Int64""")
        sb.AppendLine("	set column.cstype = ""int""")
        sb.AppendLine("	set column.vbdefval = 0")
        sb.AppendLine("	set column.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""smallint"" or column.datatype = ""smallint identity""")
        sb.AppendLine("	set column.vbtype = ""Int16""")
        sb.AppendLine("	set column.cstype = ""int""")
        sb.AppendLine("	set column.vbdefval = 0")
        sb.AppendLine("	set column.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""tinyint""")
        sb.AppendLine("	set column.vbtype = ""Byte""")
        sb.AppendLine("	set column.cstype = ""byte""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""varchar""")
        sb.AppendLine("	set column.vbtype = ""String""")
        sb.AppendLine("	set column.cstype = ""string""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	if column.length = -1")
        sb.AppendLine("		set column.sqltypefullspec = column.datatype + ""(max)""")
        sb.AppendLine("	else")
        sb.AppendLine("		set column.sqltypefullspec = column.datatype + ""("" + column.length + "")""")
        sb.AppendLine("	end")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""nvarchar""")
        sb.AppendLine("	set column.vbtype = ""String""")
        sb.AppendLine("	set column.cstype = ""string""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	if column.length = -1")
        sb.AppendLine("		set column.sqltypefullspec = column.datatype + ""(max)""")
        sb.AppendLine("	else")
        sb.AppendLine("		set column.sqltypefullspec = column.datatype + ""("" + column.length + "")""")
        sb.AppendLine("	end")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""text"" or column.datatype = ""ntext"" or column.datatype = ""xml"" or column.datatype = ""sysname""")
        sb.AppendLine("	set column.vbtype = ""String""")
        sb.AppendLine("	set column.cstype = ""string""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""char""")
        sb.AppendLine("	set column.vbtype = ""String""")
        sb.AppendLine("	set column.cstype = ""string""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	set column.sqltypefullspec = column.datatype + ""("" + column.length + "")""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""nchar""")
        sb.AppendLine("	set column.vbtype = ""String""")
        sb.AppendLine("	set column.cstype = ""string""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	set column.sqltypefullspec = column.datatype + ""("" + column.length + "")""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""decimal"" or column.datatype = ""numeric""")
        sb.AppendLine("	set column.vbtype = ""Decimal""")
        sb.AppendLine("	set column.cstype = ""decimal""")
        sb.AppendLine("	set column.vbdefval = 0")
        sb.AppendLine("	set column.csdefval = 0")
        sb.AppendLine("	set column.sqltypefullspec = column.datatype + ""("" + column.precision + "","" + column.scale + "")""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""float""")
        sb.AppendLine("	set column.vbtype = ""Double""")
        sb.AppendLine("	set column.cstype = ""float""")
        sb.AppendLine("	set column.vbdefval = 0")
        sb.AppendLine("	set column.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""real""")
        sb.AppendLine("	set column.vbtype = ""Single""")
        sb.AppendLine("	set column.cstype = ""single""")
        sb.AppendLine("	set column.vbdefval = 0")
        sb.AppendLine("	set column.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""money"" or column.datatype = ""smallmoney""")
        sb.AppendLine("	set column.vbtype = ""Decimal""")
        sb.AppendLine("	set column.cstype = ""decimal""")
        sb.AppendLine("	set column.vbdefval = 0")
        sb.AppendLine("	set column.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""datetime"" or column.datatype = ""smalldatetime"" or column.datatype = ""timestamp""")
        sb.AppendLine("	set column.vbtype = ""DateTime""")
        sb.AppendLine("	set column.cstype = ""datetime""")
        sb.AppendLine("	set column.vbdefval = ""System.DateTime.Now""")
        sb.AppendLine("	set column.csdefval = ""System.DateTime.Now""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""binary"" or column.datatype = ""image""")
        sb.AppendLine("	set column.vbtype = ""Byte()""")
        sb.AppendLine("	set column.cstype = ""byte[]""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	set column.sqltypefullspec = column.datatype + ""("" + column.length + "")""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""varbinary""")
        sb.AppendLine("	set column.vbtype = ""Byte()""")
        sb.AppendLine("	set column.cstype = ""byte[]""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	if column.length = -1")
        sb.AppendLine("		set column.sqltypefullspec = column.datatype + ""(max)""")
        sb.AppendLine("	else")
        sb.AppendLine("		set column.sqltypefullspec = column.datatype + ""("" + column.length + "")""")
        sb.AppendLine("	end")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""sql_variant""")
        sb.AppendLine("	set column.vbtype = ""Object""")
        sb.AppendLine("	set column.cstype = ""object""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("elseif column.datatype = ""uniqueidentifier""")
        sb.AppendLine("	set column.vbtype = ""Guid""")
        sb.AppendLine("	set column.cstype = ""Guid""")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("else")
        sb.AppendLine("	set column.vbtype = column.datatype")
        sb.AppendLine("	set column.cstype = column.datatype")
        sb.AppendLine("	set column.vbdefval = ""Nothing""")
        sb.AppendLine("	set column.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("end")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("")
        sb.AppendLine("//Set parameter property values")
        sb.AppendLine("set param.sqltype = param.datatype")
        sb.AppendLine("set param.sqltypefullspec = param.datatype")
        sb.AppendLine("")
        sb.AppendLine("if param.datatype = ""bit""")
        sb.AppendLine("	set param.vbtype = ""Boolean""")
        sb.AppendLine("	set param.cstype = ""bool""")
        sb.AppendLine("	set param.vbdefval = ""False""")
        sb.AppendLine("	set param.csdefval = ""false""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""int"" or param.datatype = ""int identity""")
        sb.AppendLine("	set param.vbtype = ""Integer""")
        sb.AppendLine("	set param.cstype = ""int""")
        sb.AppendLine("	set param.vbdefval = 0")
        sb.AppendLine("	set param.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""bigint"" or param.datatype = ""bigint identity""")
        sb.AppendLine("	set param.vbtype = ""Int64""")
        sb.AppendLine("	set param.cstype = ""int""")
        sb.AppendLine("	set param.vbdefval = 0")
        sb.AppendLine("	set param.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""smallint"" or param.datatype = ""smallint identity""")
        sb.AppendLine("	set param.vbtype = ""Int16""")
        sb.AppendLine("	set param.cstype = ""int""")
        sb.AppendLine("	set param.vbdefval = 0")
        sb.AppendLine("	set param.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""tinyint""")
        sb.AppendLine("	set param.vbtype = ""Byte""")
        sb.AppendLine("	set param.cstype = ""byte""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""varchar""")
        sb.AppendLine("	set param.vbtype = ""String""")
        sb.AppendLine("	set param.cstype = ""string""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	if param.length = -1")
        sb.AppendLine("		set param.sqltypefullspec = param.datatype + ""(max)""")
        sb.AppendLine("	else")
        sb.AppendLine("		set param.sqltypefullspec = param.datatype + ""("" + param.length + "")""")
        sb.AppendLine("	end")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""nvarchar""")
        sb.AppendLine("	set param.vbtype = ""String""")
        sb.AppendLine("	set param.cstype = ""string""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	if param.length = -1")
        sb.AppendLine("		set param.sqltypefullspec = param.datatype + ""(max)""")
        sb.AppendLine("	else")
        sb.AppendLine("		set param.sqltypefullspec = param.datatype + ""("" + param.length + "")""")
        sb.AppendLine("	end")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""text"" or param.datatype = ""ntext"" or param.datatype = ""xml"" or param.datatype = ""sysname""")
        sb.AppendLine("	set param.vbtype = ""String""")
        sb.AppendLine("	set param.cstype = ""string""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""char""")
        sb.AppendLine("	set param.vbtype = ""String""")
        sb.AppendLine("	set param.cstype = ""string""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	set param.sqltypefullspec = param.datatype + ""("" + param.length + "")""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""nchar""")
        sb.AppendLine("	set param.vbtype = ""String""")
        sb.AppendLine("	set param.cstype = ""string""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	set param.sqltypefullspec = param.datatype + ""("" + param.length + "")""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""decimal"" or param.datatype = ""numeric""")
        sb.AppendLine("	set param.vbtype = ""Decimal""")
        sb.AppendLine("	set param.cstype = ""decimal""")
        sb.AppendLine("	set param.vbdefval = 0")
        sb.AppendLine("	set param.csdefval = 0")
        sb.AppendLine("	set param.sqltypefullspec = param.datatype + ""("" + param.precision + "","" + param.scale + "")""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""float""")
        sb.AppendLine("	set param.vbtype = ""Double""")
        sb.AppendLine("	set param.cstype = ""float""")
        sb.AppendLine("	set param.vbdefval = 0")
        sb.AppendLine("	set param.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""real""")
        sb.AppendLine("	set param.vbtype = ""Single""")
        sb.AppendLine("	set param.cstype = ""single""")
        sb.AppendLine("	set param.vbdefval = 0")
        sb.AppendLine("	set param.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""money"" or param.datatype = ""smallmoney""")
        sb.AppendLine("	set param.vbtype = ""Decimal""")
        sb.AppendLine("	set param.cstype = ""decimal""")
        sb.AppendLine("	set param.vbdefval = 0")
        sb.AppendLine("	set param.csdefval = 0")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""datetime"" or param.datatype = ""smalldatetime"" or param.datatype = ""timestamp""")
        sb.AppendLine("	set param.vbtype = ""DateTime""")
        sb.AppendLine("	set param.cstype = ""datetime""")
        sb.AppendLine("	set param.vbdefval = ""System.DateTime.Now""")
        sb.AppendLine("	set param.csdefval = ""System.DateTime.Now""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""binary"" or param.datatype = ""image""")
        sb.AppendLine("	set param.vbtype = ""Byte()""")
        sb.AppendLine("	set param.cstype = ""byte[]""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	set param.sqltypefullspec = param.datatype + ""("" + param.length + "")""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""varbinary""")
        sb.AppendLine("	set param.vbtype = ""Byte()""")
        sb.AppendLine("	set param.cstype = ""byte[]""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	if param.length = -1")
        sb.AppendLine("		set param.sqltypefullspec = param.datatype + ""(max)""")
        sb.AppendLine("	else")
        sb.AppendLine("		set param.sqltypefullspec = param.datatype + ""("" + param.length + "")""")
        sb.AppendLine("	end")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""sql_variant""")
        sb.AppendLine("	set param.vbtype = ""Object""")
        sb.AppendLine("	set param.cstype = ""object""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("elseif param.datatype = ""uniqueidentifier""")
        sb.AppendLine("	set param.vbtype = ""Guid""")
        sb.AppendLine("	set param.cstype = ""Guid""")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("else")
        sb.AppendLine("	set param.vbtype = param.datatype")
        sb.AppendLine("	set param.cstype = param.datatype")
        sb.AppendLine("	set param.vbdefval = ""Nothing""")
        sb.AppendLine("	set param.csdefval = ""null""")
        sb.AppendLine("	")
        sb.AppendLine("end")
        sb.AppendLine("")

        Return sb.ToString
    End Function

#End Region


    '<TestMethod()> Public Sub Test_bit()
    '    src.DataType = "bit"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Boolean", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("bool", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("bit", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("bit", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_int()
    '    src.DataType = "int"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Integer", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("int", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("int", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("int", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_intid()
    '    src.DataType = "int identity"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Integer", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("int", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("int identity", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("int identity", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_bigint()
    '    src.DataType = "bigint"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Int64", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("int", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("bigint", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("bigint", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_smallint()
    '    src.DataType = "smallint"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Int16", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("int", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("smallint", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("smallint", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_tinyint()
    '    src.DataType = "tinyint"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Byte", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("byte", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("tinyint", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("tinyint", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_varchar()
    '    src.DataType = "varchar"
    '    src.Length = -1

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("String", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("string", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("varchar", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("varchar(max)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_nvarchar()
    '    src.DataType = "nvarchar"
    '    src.Length = 0

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("String", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("string", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("nvarchar", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("nvarchar(0)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_text()
    '    src.DataType = "text"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("String", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("string", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("text", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("text", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_ntext()
    '    src.DataType = "ntext"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("String", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("string", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("ntext", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("ntext", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_xml()
    '    src.DataType = "xml"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("String", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("string", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("xml", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("xml", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_char()
    '    src.DataType = "char"
    '    src.Length = 5

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("String", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("string", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("char", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("char(5)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_nchar()
    '    src.DataType = "nchar"
    '    src.Length = 26

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("String", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("string", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("nchar", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("nchar(26)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_decimal()
    '    src.DataType = "decimal"
    '    src.Scale = 2
    '    src.Precision = 18

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Decimal", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("decimal", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("decimal", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("decimal(18,2)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_float()
    '    src.DataType = "float"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Double", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("float", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("float", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("float", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_real()
    '    src.DataType = "real"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Single", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("single", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("real", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("real", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_money()
    '    src.DataType = "money"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Decimal", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("decimal", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("money", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("money", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_smallmoney()
    '    src.DataType = "smallmoney"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Decimal", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("decimal", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("smallmoney", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("smallmoney", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_datetime()
    '    src.DataType = "datetime"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("DateTime", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("datetime", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("datetime", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("datetime", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_smalldatetime()
    '    src.DataType = "smalldatetime"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("DateTime", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("datetime", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("smalldatetime", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("smalldatetime", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_timestamp()
    '    src.DataType = "timestamp"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("DateTime", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("datetime", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("timestamp", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("timestamp", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_binary()
    '    src.DataType = "binary"
    '    src.Length = 255

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Byte()", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("byte[]", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("binary", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("binary(255)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_image()
    '    src.DataType = "image"
    '    src.Length = 126

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Byte()", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("byte[]", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("image", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("image(126)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_varbinary()
    '    src.DataType = "varbinary"
    '    src.Length = 255

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Byte()", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("byte[]", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("varbinary", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("varbinary(255)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_varbinarymax()
    '    src.DataType = "varbinary"
    '    src.Length = -1

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Byte()", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("byte[]", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("varbinary", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("varbinary(max)", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_sql_variant()
    '    src.DataType = "sql_variant"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Object", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("object", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("sql_variant", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("sql_variant", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_uniqueidentifier()
    '    src.DataType = "uniqueidentifier"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("Guid", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("Guid", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("uniqueidentifier", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("uniqueidentifier", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_other()
    '    src.DataType = "blahblah"

    '    Dim val As Object = trans.GetAttributeValue(src, srcOwner, "vbtype")
    '    Assert.AreEqual("blahblah", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "cstype")
    '    Assert.AreEqual("blahblah", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltype")
    '    Assert.AreEqual("blahblah", val.ToString, False)
    '    val = trans.GetAttributeValue(src, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("blahblah", val.ToString, False)
    'End Sub

    '<TestMethod()> Public Sub Test_Param_int()
    '    srcParam.DataType = "int"

    '    Dim val As Object = trans.GetAttributeValue(srcParam, srcOwner, "vbtype")
    '    Assert.AreEqual("Integer", val.ToString, False)
    '    val = trans.GetAttributeValue(srcParam, srcOwner, "cstype")
    '    Assert.AreEqual("int", val.ToString, False)
    '    val = trans.GetAttributeValue(srcParam, srcOwner, "sqltype")
    '    Assert.AreEqual("int", val.ToString, False)
    '    val = trans.GetAttributeValue(srcParam, srcOwner, "sqltypefullspec")
    '    Assert.AreEqual("int", val.ToString, False)
    'End Sub

End Class
