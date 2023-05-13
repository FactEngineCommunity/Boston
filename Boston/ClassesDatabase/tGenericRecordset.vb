

Namespace Database
    Public MustInherit Class GenericRecordset

        Public ActiveConnection As Object
        Public CursorType As Integer

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Overridable ReadOnly Property EOF As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overridable ReadOnly Property Fields As ADODB.Fields
            Get
                Return Nothing
            End Get
        End Property

        Default Public Overridable Property Item(ByVal asItemValue As String) As Object
            Get
                Return Nothing
            End Get
            Set(ByVal value As Object)
                Throw New NotImplementedException("Cannot change values in a SQLiteDataReader result set.")
            End Set

        End Property

        Public Overridable Sub Close()
        End Sub

        Public Overridable Function MoveFirst()
            Return True
        End Function

        Public Overridable Function MoveNext()
            Return True
        End Function

        Public Overridable Function Open(ByVal asQuery As String) As Boolean

            Return False

        End Function

        Public Overridable Function Read() As Boolean

            Return False

        End Function

    End Class

End Namespace
