Public Class tReferenceFieldValue

    Public RowId As String = ""
    Public ReferenceTableId As Integer = 0
    Public ReferenceFieldId As Integer = 0
    Public Data As String = ""

    ''' <summary>
    ''' Parameterless Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    Public Sub New(ByVal aiReferenceTableId As Integer,
                   ByVal aiReferenceFieldId As Integer,
                   ByVal aiRowId As String,
                   ByVal asData As String)

        Me.ReferenceTableId = aiReferenceTableId
        Me.ReferenceFieldId = aiReferenceFieldId
        Me.RowId = aiRowId
        Me.Data = asData

    End Sub

    Public Sub Save()
        '-----------------------------------------
        'Saves the ReferenceTypeValue to the database
        '-----------------------------------------
        If TableReferenceFieldValue.ExistsReferenceFieldValue(Me) Then
            Call TableReferenceFieldValue.UpdateReferenceFieldValue(Me)
        Else
            Call TableReferenceFieldValue.AddReferenceFieldValue(Me)
        End If

    End Sub

    Public Sub delete()
        '-----------------------------------------
        'Saves the ReferenceTypeValue to the database
        '-----------------------------------------
        Call TableReferenceFieldValue.DeleteReferenceFieldValue(Me)

    End Sub

End Class
