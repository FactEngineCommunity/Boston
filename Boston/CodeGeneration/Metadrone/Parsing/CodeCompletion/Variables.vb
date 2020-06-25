Imports Boston.Parser.Syntax.Strings

Namespace Parser.CodeCompletion

    Friend Class Variables
        Inherits Collections.CollectionBase

        Public Sub Add(ByVal Variable As Variable)
            'Check if already declared
            For Each v As Variable In Me
                If StrEq(v.Name, Variable.Name) Then
                    v.Type = Variable.Type
                    v.ScopeDepth = Variable.ScopeDepth
                    Exit Sub
                End If
            Next

            Me.List.Add(Variable)
        End Sub

        Public Sub Remove(ByVal Name As String)
            For i As Integer = 0 To Me.Count - 1
                If StrEq(Me.Item(i).Name, Name) Then
                    Me.List.RemoveAt(i)
                    Exit Sub
                End If
            Next
        End Sub

        Public Function Item(ByVal index As Integer) As Variable
            Return CType(Me.List(index), Variable)
        End Function

        Public Function Item(ByVal Name As String) As Variable
            For Each v As Variable In Me
                If StrEq(v.Name, Name) Then Return v
            Next

            Return Nothing
        End Function

        Public Function IndexOf(ByVal Name As String) As Integer
            For i As Integer = 0 To Me.Count - 1
                If StrEq(Me.Item(i).Name, Name) Then Return i
            Next

            Return -1
        End Function

    End Class

End Namespace