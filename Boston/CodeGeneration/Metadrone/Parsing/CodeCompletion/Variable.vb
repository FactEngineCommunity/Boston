Namespace Parser.CodeCompletion

    Friend Class Variable
        Public Enum Types
            Primitive = 1
            Source = 2
            Table = 3
            View = 4
            Procedure = 5
            [Function] = 6
            Column = 7
            PKColumn = 8
            FKColumn = 9
            IDColumn = 10
            File = 11
            Parameter = 12
            InParameter = 13
            OutParameter = 14
            InOutParameter = 15
            TemplateParameter = 16
            Relation = 17 'Boston specific. Not part of original Metadrone.
        End Enum

        Public Name As String = ""
        Public Type As Types = Types.Primitive
        Public ScopeDepth As Integer = 0
        Public SourceTag As String = "" '<- the source this variable will work against

        Public Sub New(ByVal Name As String, ByVal Type As Types, ByVal ScopeDepth As Integer, ByVal SourceTag As String)
            Me.Name = Name
            Me.Type = Type
            Me.ScopeDepth = ScopeDepth
            Me.SourceTag = SourceTag
        End Sub

    End Class

End Namespace