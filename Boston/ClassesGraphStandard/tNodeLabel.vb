Namespace GraphStandard

    Public Class NodeLabel
        Public Property id As String
        Public Property Token As String

        ' Parameterless Constructor
        Public Sub New()
            ' Default constructor with no arguments
        End Sub

        ' Standard Constructor with Arguments
        Public Sub New(ByVal id As String, ByVal token As String)
            Me.id = id
            Me.Token = token
        End Sub
    End Class

End Namespace