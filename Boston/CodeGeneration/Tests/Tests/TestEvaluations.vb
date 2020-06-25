Imports System
Imports System.Text
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class TestEvaluations

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

    'Private Function ToToken(ByVal val As String) As Boston.Parser.Syntax.SyntaxToken
    '    Return New Boston.Parser.Syntax.SyntaxToken(val, 0, Boston.Parser.Syntax.SyntaxToken.ElementTypes.NotSet)
    'End Function

    '<TestMethod()> Public Sub EvalJustaNum()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("234"))

    '    Assert.AreEqual("234", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalJustaUnary()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("+"))

    '    Try
    '        Dim val As Object = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '        Assert.Fail("Syntax error.")

    '    Catch ex As Exception

    '    End Try
    'End Sub

    '<TestMethod()> Public Sub EvalPlainMaths1()
    '    '1 + 2
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("1"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("2"))

    '    Assert.AreEqual((1 + 2).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalPlainMaths2()
    '    '(1 + 2) * 2
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("1"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("2"))

    '    Assert.AreEqual(((1 + 2) * 2).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalPlainMaths3()
    '    '1 + 2 * 2
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("1"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("2"))

    '    Assert.AreEqual((1 + 2 * 2).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalPlainMaths4()
    '    '2 * "2"
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("""2"""))

    '    Assert.AreEqual((2 * 2).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalPlainMaths5()
    '    '2 * "xx"
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("""xx"""))

    '    Try
    '        Dim val As Object = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '        Assert.Fail("Conversion from string ""xx"" to type 'Double' is not valid.")

    '    Catch ex As Exception

    '    End Try
    'End Sub

    '<TestMethod()> Public Sub EvalPlainMaths6()
    '    '2 * -2
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("-2"))

    '    Assert.AreEqual((2 * -2).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalPlainMaths7()
    '    '3 * "-2"
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("3"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("""-2"""))

    '    Assert.AreEqual("-6", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalJustaString()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("""abc"""))

    '    Assert.AreEqual("abc", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalConcatStrings1()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("""foo"""))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("""bar"""))

    '    Assert.AreEqual("foo" + "bar", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalConcatStrings2()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("""foo"""))
    '    tokens.Add(Me.ToToken("&"))
    '    tokens.Add(Me.ToToken("""bar"""))

    '    Assert.AreEqual("foo" & "bar", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalConcatStrings3()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("1"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("""2"""))

    '    Assert.AreEqual("1" + "2", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalPlainOldVar()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("blah"))

    '    Assert.AreEqual("123", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalBoolean()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("true"))

    '    Assert.AreEqual((True).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalNot1()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("not"))
    '    tokens.Add(Me.ToToken("true"))

    '    Assert.AreEqual((Not True).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalNot2()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("not"))
    '    tokens.Add(Me.ToToken("true"))
    '    tokens.Add(Me.ToToken("and"))
    '    tokens.Add(Me.ToToken("false"))

    '    Assert.AreEqual((Not True And False).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalVarNoAttrib()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("tbl"))

    '    Assert.AreEqual("123", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalPlainOldMethod1()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("tbl.listpos"))

    '    Assert.AreEqual("123", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalPlainOldMethod2()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("tbl.listpos"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken(")"))

    '    Try
    '        Dim val As Object = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '        Assert.Fail("'listpos' is not a method.")

    '    Catch ex As Exception

    '    End Try
    'End Sub

    '<TestMethod()> Public Sub EvalDataSource1()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("sources.Source1"))

    '    Assert.AreEqual("Data Source=sgc\sqlexpress;Initial Catalog=asm;Integrated Security=True;", Boston.Parser.Syntax.Exec_Expr.TestEvalExpressionSource(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalDataSource2()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("sources.Source1"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual("Data Source=sgc\sqlexpress;Initial Catalog=asm;Integrated Security=True;", Boston.Parser.Syntax.Exec_Expr.TestEvalExpressionSource(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalDataSource3()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("src"))

    '    Assert.AreEqual("Data Source=sgc\sqlexpress;Initial Catalog=asm;Integrated Security=True;", Boston.Parser.Syntax.Exec_Expr.TestEvalExpressionSource(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalBadBrackets1()
    '    '(123 * (456 + 321)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("321"))
    '    tokens.Add(Me.ToToken(")"))

    '    Try
    '        Dim val As Object = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '        Assert.Fail("Expecting: End parentheses ')' expected.")

    '    Catch ex As Exception

    '    End Try
    'End Sub

    '<TestMethod()> Public Sub EvalBadBrackets2()
    '    '123 * (456 + 321))
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("321"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken(")"))

    '    Try
    '        Dim val As Object = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '        Assert.Fail("Expecting: End parentheses ')' without matching begin parentheses '('.")

    '    Catch ex As Exception

    '    End Try
    'End Sub

    '<TestMethod()> Public Sub EvalPrimitives()
    '    '(123 * (456 + 321) - strblah + 234)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("321"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("strblah"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(")"))

    '    Try
    '        Dim val As Object = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '        Assert.Fail("Expecting: Conversion from string ""abc"" to type 'Double' is not valid.")

    '    Catch ex As Exception

    '    End Try
    'End Sub

    '<TestMethod()> Public Sub EvalMethod()
    '    '(123 * (456 + 321) - tbl.listpos + 234)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("321"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("tbl.listpos"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual(((123 * (456 + 321) - 123 + 234)).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalCompare1()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken("="))
    '    tokens.Add(Me.ToToken("234"))

    '    Assert.AreEqual((234 = 234).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalCompare2()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(">"))
    '    tokens.Add(Me.ToToken("123"))

    '    Assert.AreEqual((234 > 123).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalCompare3()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken(">="))
    '    tokens.Add(Me.ToToken("123"))

    '    Assert.AreEqual((123 >= 123).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalCompare4()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("<"))
    '    tokens.Add(Me.ToToken("456"))

    '    Assert.AreEqual((123 < 456).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalCompare5()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("<="))
    '    tokens.Add(Me.ToToken("456"))

    '    Assert.AreEqual((123 <= 456).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalCompare6()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("<>"))
    '    tokens.Add(Me.ToToken("456"))

    '    Assert.AreEqual((123 <> 456).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalLogic()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken("="))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken("and"))
    '    tokens.Add(Me.ToToken("134"))
    '    tokens.Add(Me.ToToken("<>"))
    '    tokens.Add(Me.ToToken("135"))

    '    Assert.AreEqual((234 = 234 And 134 <> 135).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalBoolsInParens()
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken("="))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken("or"))
    '    tokens.Add(Me.ToToken("134"))
    '    tokens.Add(Me.ToToken("<>"))
    '    tokens.Add(Me.ToToken("135"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("and"))
    '    tokens.Add(Me.ToToken("134"))
    '    tokens.Add(Me.ToToken(">"))
    '    tokens.Add(Me.ToToken("2"))

    '    Assert.AreEqual(((234 = 234 Or 134 <> 135) And 123 > 2).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalSimpleArgs1()
    '    '(123, 234)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(")"))

    '    Dim val As List(Of Object) = Boston.Parser.Syntax.Exec_Expr.TestEvalAsArguments(tokens)
    '    Assert.AreEqual("2", val.Count.ToString, False)
    '    Assert.AreEqual("123", val(0).ToString, False)
    '    Assert.AreEqual("234", val(1).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalSimpleArgs2()
    '    '(123, 234, 345)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("345"))
    '    tokens.Add(Me.ToToken(")"))

    '    Dim val As List(Of Object) = Boston.Parser.Syntax.Exec_Expr.TestEvalAsArguments(tokens)
    '    Assert.AreEqual("3", val.Count.ToString, False)
    '    Assert.AreEqual("123", val(0).ToString, False)
    '    Assert.AreEqual("234", val(1).ToString, False)
    '    Assert.AreEqual("345", val(2).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMathsFunc1()
    '    'sin(123+456)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("sin"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken(")"))

    '    Dim val As String = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '    Assert.AreEqual(Math.Sin(123 + 456).ToString, val, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMathsFunc2()
    '    'cos(345+567)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("cos"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("345"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("567"))
    '    tokens.Add(Me.ToToken(")"))

    '    Dim val As String = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '    Assert.AreEqual(Math.Cos(345 + 567).ToString, val, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMathsFunc3()
    '    'tan(345+567)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("tan"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("543"))
    '    tokens.Add(Me.ToToken(")"))

    '    Dim val As String = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '    Assert.AreEqual(Math.Tan(234 + 543).ToString, val, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMathsFunc4()
    '    'sqrt(12*12)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("sqrt"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("12"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("12"))
    '    tokens.Add(Me.ToToken(")"))

    '    Dim val As String = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '    Assert.AreEqual(Math.Sqrt(12 * 12).ToString, val, False)
    'End Sub

    '<TestMethod()> Public Sub EvalSimpleMethodWithArgs()
    '    'tbl.replace("123", "456"))
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("tbl.replace"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("""123"""))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual("456", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalSimpleMethodWithArgsHavingExpression()
    '    'tbl.replace(125 - 2, "456"))
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("tbl.replace"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("125"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual("456", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalSimpleArgsNoMethod()
    '    'tbl.replace(125 - 2, "456"))
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("""123"""))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))

    '    Try
    '        Dim val As Object = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '        Assert.Fail("Expecting: Syntax error.")

    '    Catch ex As Exception

    '    End Try
    'End Sub

    '<TestMethod()> Public Sub EvalSimpleArgsHavingExpressionNoMethod()
    '    'tbl.replace(125 - 2, "456"))
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("125"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))

    '    Try
    '        Dim val As Object = Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString
    '        Assert.Fail("Expecting: Syntax error.")

    '    Catch ex As Exception

    '    End Try
    'End Sub

    '<TestMethod()> Public Sub EvalMethodWithArgs()
    '    '(123 * (456 + 321) - tbl.replace("123", "456")) + 234)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("321"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("tbl.replace"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("""123"""))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual("95349", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMethodWithArgsHavingExpression1()
    '    '123 + tbl.replace(125 - 2, "456")
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("tbl.replace"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("125"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual("123" + "456", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMethodWithArgsHavingExpression2()
    '    '123 + cnum(tbl.replace(125 - 2, "456"))
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("cnum"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("tbl.replace"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("125"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual((123 + 456).ToString, Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMethodWithArgsHavingExpression3()
    '    '(123 * (456 + 321) - tbl.replace(125 - 2, "456")) + 234)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("321"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("tbl.replace"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("125"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual("95349", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMethodWithArgsHavingBracketedExpression1()
    '    '123 + tbl.replace( (125 - 2), "456"))
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("tbl.replace"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("125"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual("123" + "456", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalMethodWithArgsHavingBracketedExpression2()
    '    '(123 * (456 + 321) - tbl.replace( (125 - 2), "456")) + 234)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("321"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("tbl.replace"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("125"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("""456"""))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(")"))

    '    Assert.AreEqual("95349", Boston.Parser.Syntax.Exec_Expr.TestEvalExpression(tokens).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalExpression()
    '    '123 * (456 - (2 + tbl.listpos) )
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("tbl.listpos"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken(")"))

    '    Dim val As List(Of Object) = Boston.Parser.Syntax.Exec_Expr.TestEvalAsArguments(tokens)
    '    Assert.AreEqual((123 * (456 - (2 + 123))).ToString, val(0).ToString, False)
    'End Sub

    '<TestMethod()> Public Sub EvalAsArgs()
    '    '(123 * (456 - (2 + tbl.listpos) ) , 6 + 234)
    '    Dim tokens As New Boston.Parser.Syntax.SyntaxTokenCollection()
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("123"))
    '    tokens.Add(Me.ToToken("*"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("456"))
    '    tokens.Add(Me.ToToken("-"))
    '    tokens.Add(Me.ToToken("("))
    '    tokens.Add(Me.ToToken("2"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("tbl.listpos"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken(")"))
    '    tokens.Add(Me.ToToken(","))
    '    tokens.Add(Me.ToToken("6"))
    '    tokens.Add(Me.ToToken("+"))
    '    tokens.Add(Me.ToToken("234"))
    '    tokens.Add(Me.ToToken(")"))

    '    Dim val As List(Of Object) = Boston.Parser.Syntax.Exec_Expr.TestEvalAsArguments(tokens)
    '    Assert.AreEqual("2", val.Count.ToString, False)
    '    Assert.AreEqual((123 * (456 - (2 + 123))).ToString, val(0).ToString, False)
    '    Assert.AreEqual((6 + 234).ToString, val(1).ToString, False)
    'End Sub

End Class
