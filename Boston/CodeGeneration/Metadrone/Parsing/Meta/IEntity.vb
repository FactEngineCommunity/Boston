Namespace Parser.Meta

    Friend Interface IEntity

        Function GetConnectionString() As String
        Sub SetAttributeValue(ByVal AttribName As String, ByVal value As Object)
        Function GetAttributeValue(ByVal AttribName As String, ByVal Params As List(Of Object), _
                                   ByVal LookTransformsIfNotFound As Boolean, ByRef ExitBlock As Boolean) As Object
        Sub InitEntities()
        Function GetEntities(ByVal Entity As Syntax.SyntaxNode.ExecForEntities) As List(Of IEntity)
        Function GetCopy() As IEntity

    End Interface

End Namespace