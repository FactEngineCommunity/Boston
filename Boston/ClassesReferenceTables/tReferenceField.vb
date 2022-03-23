Public Class tReferenceField

    Public reference_table_id As Integer
    Public reference_field_id As Integer
    Public label As String
    Public data_type As Integer
    Public cardinality As Integer
    Public required As Boolean
    Public system As Boolean 'Indicates whether the ReferenceField is System supplied (or User supplied/editable).
    'Users cannot modify a 'System' ReferenceField.

    ''' <summary>
    ''' Parameterless Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    Public Sub New(ByVal aiTableId As Integer,
                   ByVal aiFieldId As Integer,
                   ByVal asLabel As String,
                   ByVal aiDataType As Integer,
                   ByVal aiCardinality As Integer,
                   ByVal abRequired As Boolean,
                   ByVal abIsSystemField As Boolean)

        Me.reference_table_id = aiTableId
        Me.reference_field_id = aiFieldId
        Me.label = asLabel
        Me.data_type = aiDataType
        Me.cardinality = aiCardinality
        Me.required = abRequired
        Me.system = abIsSystemField

    End Sub

End Class
