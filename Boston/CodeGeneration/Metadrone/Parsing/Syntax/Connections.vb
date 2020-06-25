Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Meta.Database
Imports Boston.Parser.Meta

Namespace Parser.Syntax

    Friend Class Connections
        Inherits Collections.CollectionBase

        Public Enum LoadModes
            None = 0
            Schema = 1
            Routines = 2
        End Enum

        Public Sub Add(ByVal Source As Parser.Source.Source, ByVal LoadMode As LoadModes)
            If Me.Item(Source.Name) Is Nothing Then
                Dim schema As New Schema(Source)
                Select Case LoadMode
                    Case LoadModes.Schema : schema.LoadSchema()
                    Case LoadModes.Routines : schema.LoadRoutineSchema()
                End Select

                Me.List.Add(schema)
            Else
                Select Case LoadMode
                    Case LoadModes.Schema : Me.Item(Source.Name).LoadSchema()
                    Case LoadModes.Routines : Me.Item(Source.Name).LoadRoutineSchema()
                End Select

            End If
        End Sub

        Public Sub Remove(ByVal Name As String)
            For i As Integer = 0 To Me.Count - 1
                If StrEq(Me.Item(i).GetName, Name) Then
                    Me.List.RemoveAt(i)
                    Exit Sub
                End If
            Next
        End Sub

        Public Function Item(ByVal index As Integer) As IEntityConnection
            Return CType(Me.List(index), IEntityConnection)
        End Function

        Public Function Item(ByVal Name As String) As IEntityConnection
            For Each v As IEntityConnection In Me
                If StrEq(v.GetName, Name) Then Return v
            Next
            Return Nothing
        End Function

    End Class

End Namespace