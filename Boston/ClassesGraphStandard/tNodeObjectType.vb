Namespace GraphStandard
    Public Class NodeObjectType
        Public Property id As String
        Public Property Labels As List(Of NodeLabelRef)
        Public Property Properties As List(Of NodeProperty)

        ' Parameterless Constructor
        Public Sub New()
            ' Default constructor with no arguments
            Labels = New List(Of NodeLabelRef)()
            Properties = New List(Of NodeProperty)()
        End Sub

        ' Standard Constructor with Arguments
        Public Sub New(ByVal id As String, ByVal labels As List(Of NodeLabelRef), ByVal properties As List(Of NodeProperty))
            Me.id = id
            Me.Labels = labels
            Me.Properties = properties
        End Sub
    End Class
End Namespace