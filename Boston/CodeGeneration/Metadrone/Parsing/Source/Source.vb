Imports Boston.Parser.Syntax.Strings

Namespace Parser.Source

    Friend Class Source

        Friend Transforms As Syntax.SourceTransforms = Nothing

        Public Name As String = Nothing
        Public Provider As String = PluginInterface.Sources.Descriptions.SQLSERVER.ProviderName

        Public ConnectionString As String = Nothing
        Public SchemaQuery As String = Nothing
        Public TableSchemaQuery As String = Nothing
        Public ColumnSchemaQuery As String = Nothing
        Public TableNamePlaceHolder As String = Nothing
        Public RoutineSchemaQuery As String = Nothing
        Public Transformations As String = Nothing

        Public Sub New()

        End Sub
    End Class

    Friend Class Sources
        Inherits Collections.CollectionBase

        Public Sub New()

        End Sub

        Public Sub Add(ByVal Source As Source)
            If Me.Item(Source.Name) IsNot Nothing Then Throw New Exception("Source '" & Source.Name & "' already declared.")
            Me.List.Add(Source)
        End Sub

        Public Sub Insert(ByVal index As Integer, ByVal Source As Source)
            If Me.Item(Source.Name) IsNot Nothing Then Throw New Exception("Source '" & Source.Name & "' already declared.")
            Me.List.Insert(index, Source)
        End Sub

        Public Shadows Sub RemoveAt(ByVal index As Integer)
            Me.List.RemoveAt(index)
        End Sub

        Public Function IndexOf(ByVal Name As String, _
                                Optional ByVal comparisonType As System.StringComparison = StringComparison.CurrentCulture) As Integer
            For i As Integer = 0 To Me.List.Count - 1
                If StrEq(CType(Me.List(i), Source).Name, Name) Then Return i
            Next

            Return -1
        End Function

        Public Function Item(ByVal index As Integer) As Source
            Return CType(Me.List(index), Source)
        End Function

        Public Function Item(ByVal Name As String) As Source
            Dim idx As Integer = Me.IndexOf(Name)
            If idx = -1 Then Return Nothing
            Return CType(Me.List.Item(idx), Source)
        End Function
    End Class

End Namespace
