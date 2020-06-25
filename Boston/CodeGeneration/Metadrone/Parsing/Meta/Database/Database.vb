Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr

Namespace Parser.Meta.Database

#Region "Replace classes"

    Friend Class ReplaceAllList
        Friend Class ReplaceAll
            Friend OldVal As String
            Friend NewVal As String

            Public Sub New(ByVal OldVal As String, ByVal NewVal As String)
                Me.OldVal = OldVal : Me.NewVal = NewVal
            End Sub
        End Class

        Friend List As New List(Of ReplaceAll)

        Friend Sub Add(ByVal OldVal As String, ByVal NewVal As String)
            Me.List.Add(New ReplaceAll(OldVal, NewVal))
        End Sub

        Friend Function ApplyReplaces(ByVal Value As Object) As Object
            If Me.List.Count = 0 Then Return Value

            'Perform replaces
            For Each rep In Me.List
                If String.IsNullOrEmpty(rep.OldVal) Then Continue For
                If PackageBuilder.PreProc.IgnoreCase Then
                    Value = ReplaceInsensitive(Value.ToString, rep.OldVal, rep.NewVal)
                Else
                    Value = Value.ToString.Replace(rep.OldVal, rep.NewVal)
                End If
            Next

            Return Value
        End Function
    End Class

#End Region

End Namespace