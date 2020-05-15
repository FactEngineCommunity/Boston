Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ExampleDuplexServiceClient
	Class UpdatedListEventArgs
		Inherits EventArgs
		Private _items As List(Of String)

		Public Property ItemList() As List(Of String)
			Get
				Return _items
			End Get
			Set
				_items = value
			End Set
		End Property

		Public Sub New(items As List(Of String))
			_items = items
		End Sub
	End Class
End Namespace
