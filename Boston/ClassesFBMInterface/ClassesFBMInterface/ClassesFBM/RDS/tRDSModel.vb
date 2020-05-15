Namespace RDS

    <Serializable()> _
    Public Class Model

        Public Table As New List(Of RDS.Entity)
        Public DataType As New List(Of RDS.DataType)
        Public Index As New List(Of RDS.Index)

        Public TargetDatabaseType As pcenumDatabaseType = Nothing

    End Class

End Namespace

