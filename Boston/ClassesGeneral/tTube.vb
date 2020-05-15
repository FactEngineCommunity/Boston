
Public Class Tube

    Public [List] As New List(Of Object)

    Public Length As Integer = 1

    Public Event ObjectAdded(ByRef arObject As Object)

    Public Sub New(ByVal aiLength As Integer)

        Me.Length = aiLength

    End Sub

    Public Sub Add(ByRef arObject As Object)

        Me.List.Insert(0, arObject)

        If Me.List.Count > Me.Length Then
            Me.List.RemoveAt(Me.Length)
        End If

        RaiseEvent ObjectAdded(arObject)

    End Sub



End Class
