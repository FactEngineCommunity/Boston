Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Meta

Namespace Parser.Syntax

    Friend Class Exec_Set
        Private ExecNode As SyntaxNode
        Private subj As String = ""
        Private val As Object = Nothing
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0
        Private attribName As String = ""

        Public Sub New(ByVal ExecNode As SyntaxNode, ByVal TemplateContext As String, ByVal ScopeDepth As Integer)
            Me.ExecNode = ExecNode
            Me.TemplateContext = TemplateContext
            Me.ScopeDepth = ScopeDepth
        End Sub

        Public Sub Process()
            'Evaluate value expression
            Me.val = EvalExpression(Me.ExecNode.Tokens, 3, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)

            'Determine target
            If Me.ExecNode.Tokens(1).QualifiedExpressionTokens.Count = 0 Then
                Me.subj = Me.ExecNode.Tokens(1).Text
            Else
                Me.subj = Me.ExecNode.SetSubj
                Me.attribName = Me.ExecNode.SetSubjAttrib
            End If

            'Set variable
            If PackageBuilder.Variables.Exists(Me.TemplateContext, Me.subj, Me.ScopeDepth) Then
                Call Me.SetExistingVariable()
            Else
                Call Me.SetNewVariable()
            End If
        End Sub

        'This will recursively keep trying to get the variable with a value not of variable
        Private Function GetVar(ByVal var As Object) As Object
            If Not TypeOf var Is Variable Then Return var
            Dim value As Variable = CType(var, Variable)
            If TypeOf value.Value Is Variable Then
                Return Me.GetVar(value.Value)
            Else
                Return value
            End If
        End Function

        'This will recursively keep trying to get a primitive value
        Private Function GetVal(ByVal var As Object) As Object
            If TypeOf var Is Variable Then
                Return Me.GetVal(CType(var, Variable).Value)
            ElseIf TypeOf var Is IEntity Then
                Return CType(var, IEntity).GetAttributeValue("", Nothing, False, False)
            Else
                Return var
            End If
        End Function

        Private Sub SetNewVariable()
            'create new variable, declare with value.
            Dim varType As Variable.Types = Variable.Types.Primitive
            If Me.val Is Nothing Then
                varType = Variable.Types.Null
            ElseIf TypeOf Me.val Is Parser.Source.Source Then
                varType = Variable.Types.SourceRef
            ElseIf TypeOf Me.val Is Parser.Syntax.Variable Then
                varType = Variable.Types.Variable
            End If

            PackageBuilder.Variables.Add(Me.TemplateContext, Me.subj, Me.ScopeDepth, New Variable(Me.val, varType))
        End Sub

        Private Sub SetExistingVariable()
            If PackageBuilder.Variables.Item(Me.TemplateContext, Me.subj, Me.ScopeDepth).Type = Variable.Types.Variable Then
                CType(PackageBuilder.Variables.Item(Me.TemplateContext, Me.subj, Me.ScopeDepth).Value, IEntity).SetAttributeValue(Me.attribName, Me.GetVal(Me.val))
            Else
                PackageBuilder.Variables.Item(Me.TemplateContext, Me.subj, Me.ScopeDepth).Value = Me.val
            End If
        End Sub

    End Class

End Namespace