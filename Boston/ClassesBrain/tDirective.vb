Public Class tDirective

    Public DirectiveId As String = System.Guid.NewGuid.ToString
    Public Directive As String = ""

    Public Sub New(ByVal asDirective As String)
        Me.Directive = asDirective
    End Sub

End Class
