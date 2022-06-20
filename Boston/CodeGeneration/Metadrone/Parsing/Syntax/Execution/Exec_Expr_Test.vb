Namespace Parser.Syntax

    Partial Friend Class Exec_Expr

        Public Shared Function TestEvalExpression(ByVal Tokens As SyntaxTokenCollection) As Object
            PackageBuilder.Variables = New Parser.Syntax.Variables()
            PackageBuilder.PreProc = New PreProcessor()
            PackageBuilder.PreProc.IgnoreCase = True

            PackageBuilder.Variables.Add(Nothing, "blah", 0, New Parser.Syntax.Variable(123, Variable.Types.Primitive))
            PackageBuilder.Variables.Add(Nothing, "strblah", 0, New Parser.Syntax.Variable("abc", Variable.Types.Primitive))

            Dim tbl As New Parser.Meta.Database.Table()
            tbl.ListPos = 123
            tbl.Value = "123"
            PackageBuilder.Variables.Add(Nothing, "tbl", 0, New Parser.Syntax.Variable(tbl, Variable.Types.Variable))

            Return EvalExpression(Tokens, Nothing, 0)
        End Function

        Public Shared Function TestEvalExpressionSource(ByVal Tokens As SyntaxTokenCollection) As String
            PackageBuilder.Variables = New Parser.Syntax.Variables()
            PackageBuilder.PreProc = New PreProcessor()
            PackageBuilder.PreProc.IgnoreCase = True

            Dim src As New Source.Source()
            src.Provider = "SQLServer"
            src.Name = "Source1"
            src.ConnectionString = "Data Source=sgc\sqlexpress;Initial Catalog=asm;Integrated Security=True;"
            PackageBuilder.Connections = New Parser.Syntax.Connections()
            PackageBuilder.ClearSources()
            PackageBuilder.AddSource(src)

            PackageBuilder.Variables.Add(Nothing, "src", 0, New Parser.Syntax.Variable(src, Variable.Types.SourceRef))

            Return CType(EvalExpression(Tokens, Nothing, 0), Source.Source).ConnectionString
        End Function

        Public Shared Function TestEvalAsArguments(ByVal Tokens As SyntaxTokenCollection) As List(Of Object)
            PackageBuilder.Variables = New Parser.Syntax.Variables()
            PackageBuilder.PreProc = New PreProcessor()
            PackageBuilder.PreProc.IgnoreCase = True

            Dim tbl As New Parser.Meta.Database.Table()
            tbl.ListPos = 123
            tbl.Value = "123"
            PackageBuilder.Variables.Add(Nothing, "tbl", 0, New Parser.Syntax.Variable(tbl, Variable.Types.Variable))

            Return EvalExpressionIntoParameters(Tokens, Nothing, 0, True)
        End Function

    End Class

End Namespace