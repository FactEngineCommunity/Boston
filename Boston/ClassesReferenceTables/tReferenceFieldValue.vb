Public Class tReferenceFieldValue

    Public RowId As String = ""
    Public ReferenceTableId As Integer = 0
    Public ReferenceFieldId As Integer = 0
    Public Data As String = ""

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
