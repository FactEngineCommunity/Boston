Public Class tReferenceTable

    Public reference_table_id As Integer
    Public name As String

    ''' <summary>
    ''' Parameterless Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    Public Sub New(ByVal aiTableId As Integer, ByVal asTableName As String)
        Me.reference_table_id = aiTableId
        Me.name = asTableName
    End Sub

End Class
