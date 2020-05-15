Namespace RDS

    <Serializable()> _
    Public Class Index

        Public Name As String

        Public Table As RDS.Entity

        Public Column As New List(Of RDS.Attribute)

        Public IsPrimaryKey As Boolean = False
        Public NonUnique As Boolean = False
        Public Unique As Boolean = False
        Public IndexQualifier As String = ""
        Public Type As pcenumODBCIndexType
        Public AscendingOrDescending As pcenumODBCAscendingOrDescending
        Public Cardinality As Integer = 0
        Public Pages As Integer = 0 'Can be DBNull from ODBC
        Public FilterCondition As String 'Can be DBNull from ODBC

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arTable As RDS.Entity, ByVal asName As String)
            Me.Table = arTable
            Me.Name = asName
        End Sub

        Public Function EqualsByName(ByVal other As RDS.Index) As Boolean
            Return Me.Name = other.Name
        End Function


    End Class

End Namespace
