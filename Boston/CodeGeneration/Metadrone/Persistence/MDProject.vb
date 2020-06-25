Namespace Persistence

    <Serializable()> _
    Public Class MDProject

        Public Name As String
        Public Profile As New Profile()
        Public Properties As New Properties()
        Public Folders As New List(Of ProjectFolder)
        Public Sources As New List(Of Source)
        Public Packages As New List(Of MDPackage)
        Public Bin As New MDObj

        Public Sub New()

        End Sub

        Friend Shared Function CheckValidName(ByVal value As String) As Boolean
            If value.IndexOf(" ") > -1 OrElse _
               Parser.Syntax.Constants.IsReservedWord(value) OrElse _
               Parser.Syntax.Constants.ContainsOperator(value) Then
                Return False
            Else
                Return Not String.IsNullOrEmpty(value)
            End If
        End Function

    End Class

End Namespace