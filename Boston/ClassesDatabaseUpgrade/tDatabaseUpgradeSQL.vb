Namespace DatabaseUpgrade
    Public Class UpgradeSQL
        Public UpgradeId As Integer
        Public UpgradeType As Integer = -1
        Public SequenceNr As Integer
        Public SQLString As String
        Public TableName As String
        Public FieldName As String
        Public DataType As Integer
        Public Cardinality As Integer
        Public DefaultValue As String
        Public OrdinalPosition As String
        Public CollatingOrder As Integer
        Public ValidationText As String
        Public ValidationRule As String
        Public Attributes As Integer
        Public DataUpdatable As Boolean
        Public Required As Boolean
        Public AllowZeroLength As Boolean
        Public CodeToExecute As String
    End Class
End Namespace