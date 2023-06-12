Namespace GraphStandard
    Public Class NodeLabelRef
        Public Property ref As String

        ' Parameterless Constructor
        Public Sub New()
            ' Default constructor with no arguments
        End Sub

        ' Standard Constructor with Argument
        Public Sub New(ByVal ref As String)
            Me.ref = ref
        End Sub
    End Class
End Namespace
