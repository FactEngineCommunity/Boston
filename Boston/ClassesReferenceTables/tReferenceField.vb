Public Class tReferenceField

    Public reference_table_id As Integer
    Public reference_field_id As Integer
    Public label As String
    Public data_type As Integer
    Public cardinality As Integer
    Public required As Integer
    Public system As Boolean 'Indicates whether the ReferenceField is System supplied (or User supplied/editable).
    'Users cannot modify a 'System' ReferenceField.

End Class
