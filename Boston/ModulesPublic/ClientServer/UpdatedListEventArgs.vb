Namespace DuplexServiceClient

    Class UpdatedListEventArgs
        Inherits EventArgs

        Private _items As List(Of String)

        Public Property ItemList() As List(Of String)
            Get
                Return _items
            End Get
            Set(value As List(Of String))
                _items = value
            End Set
        End Property

        Public Sub New(items As List(Of String))
            _items = items
        End Sub

    End Class

End Namespace