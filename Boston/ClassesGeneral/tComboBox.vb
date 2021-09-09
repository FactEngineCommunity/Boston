Public Class tComboboxItem
    Implements IEquatable(Of tComboboxItem)

    Public _ItemData As Object = Nothing
    Public Property ItemData() As Object
        Get
            Return Me._ItemData
        End Get
        Set(ByVal value As Object)
            Me._ItemData = value
        End Set
    End Property

    Public _Text As String = ""
    Public Property Text() As String
        Get
            Return Me._Text
        End Get
        Set(ByVal value As String)
            Me._Text = value
        End Set
    End Property

    Public Tag As Object = Nothing


    Public Sub New(ByVal aiItemdata As Object, ByVal as_text As String, Optional ByRef ao_tag_object As Object = Nothing)

        'store these values
        Itemdata = aiItemdata
        Text = as_text

        If Not IsNothing(ao_tag_object) Then
            Tag = ao_tag_object
        End If

    End Sub

    Public Overrides Function ToString() As String

        Return Trim(Me.Text)

    End Function

    Public Shadows Function Equals(ByVal other As tComboboxItem) As Boolean Implements System.IEquatable(Of tComboboxItem).Equals

        If Me.Itemdata = other.Itemdata Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function EqualsAll(ByVal other As tComboboxItem) As Boolean

        Return Me.ItemData = other.ItemData And Me.Text = other.Text

    End Function

End Class


