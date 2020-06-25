Namespace Parser

    Friend Class Template
        Public Name As String = Nothing
        Public Source As String = Nothing

        Public Sub New()

        End Sub

        Public Sub New(ByVal Name As String, ByVal Source As String)
            Me.Name = Name
            Me.Source = Source
        End Sub
    End Class

    Friend Class Templates
        Inherits Collections.CollectionBase

        Public Sub New()

        End Sub

        Public Sub Add(ByVal Template As Template)
            Me.List.Add(Template)
        End Sub

        Public Sub Add(ByVal TemplateName As String, ByVal Source As String)
            Me.List.Add(New Template(TemplateName, Source))
        End Sub

        Public Sub Insert(ByVal index As Integer, ByVal Template As Template)
            Me.List.Insert(index, Template)
        End Sub

        Public Shadows Sub RemoveAt(ByVal index As Integer)
            Me.List.RemoveAt(index)
        End Sub

        Public Function IndexOf(ByVal Name As String, _
                                Optional ByVal comparisonType As System.StringComparison = StringComparison.CurrentCulture) As Integer
            For i As Integer = 0 To Me.List.Count - 1
                If CType(Me.List(i), Template).Name.Equals(Name, comparisonType) Then Return i
            Next

            Return -1
        End Function

        Public Function Item(ByVal index As Integer) As Template
            Return CType(Me.List.Item(index), Template)
        End Function

        Public Function Item(ByVal Name As String) As Template
            Dim idx As Integer = Me.IndexOf(Name)
            If idx = -1 Then Return Nothing
            Return CType(Me.List.Item(idx), Template)
        End Function
    End Class

End Namespace
