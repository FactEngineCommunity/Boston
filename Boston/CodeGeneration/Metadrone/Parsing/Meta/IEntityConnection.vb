Namespace Parser.Meta

    Friend Interface IEntityConnection

        Function GetName() As String
        Function GetConnectionString() As String
        Sub LoadSchema()
        Sub LoadRoutineSchema()
        Sub InitEntities()
        Function GetEntities(ByVal Entity As Syntax.SyntaxNode.ExecForEntities) As List(Of IEntity)
        Sub RunScriptFile(ByVal ScriptFile As String)

    End Interface

End Namespace