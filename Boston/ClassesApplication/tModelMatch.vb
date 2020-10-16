Imports Boston

Public Class ModelMatch
    Implements IEquatable(Of ModelMatch)

    Public Model As FBM.Model

    Public KeyWords As New List(Of String)

    Public HitCount As Integer = 0

    Public Sub New(ByRef arModel As FBM.Model,
                   ByVal asKeyWord As String)

        Me.Model = arModel
        Me.KeyWords.Add(asKeyWord)
        Me.HitCount += 1
    End Sub

    Public Sub addKeyWord(ByVal asKeyWord As String)
        Me.KeyWords.Add(asKeyWord)
        Me.HitCount += 1
    End Sub

    Public Shadows Function Equals(other As ModelMatch) As Boolean Implements IEquatable(Of ModelMatch).Equals
        Return Me.Model Is other.Model
    End Function

End Class
