Namespace Parser.CodeCompletion

    Friend Class Source

        Public Name As String = ""
        Public Provider As String = ""
        Public Transformations As String = ""
        Public Transforms As Parser.Syntax.SourceTransforms = Nothing

        Public Sub New(ByVal name As String, ByVal provider As String, ByVal Transformations As String)
            Me.Name = name
            Me.Provider = provider
            Me.Transformations = Transformations
        End Sub

    End Class

End Namespace
