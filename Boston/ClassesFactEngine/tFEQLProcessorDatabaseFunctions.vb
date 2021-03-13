Namespace FEQL

    Partial Public Class Processor

        Public Function IndexedValuesExistInDatabase(ByRef arIndex As RDS.Index, ByRef aarColumn As List(Of RDS.Column)) As Boolean

            Try
                If Me.DatabaseManager.Connection Is Nothing Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(prApplication.WorkingModel.TargetDatabaseType, prApplication.WorkingModel.TargetDatabaseConnectionString)
                    If Me.DatabaseManager.Connection Is Nothing Then
                        Throw New Exception("No database connection has been established.")
                    End If
                ElseIf Me.DatabaseManager.Connection.Connected = False Then
                    Throw New Exception("The database is not connected.")
                End If

                Dim lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & arIndex.Table.Name
                lsSQLQuery &= " WHERE "
                Dim liInd = 0
                For Each lrColumn In aarColumn
                    If liInd > 0 Then lsSQLQuery &= " AND "
                    lsSQLQuery &= lrColumn.Name & " = "
                    If lrColumn.DataTypeIsText Then lsSQLQuery &= "'"
                    lsSQLQuery &= lrColumn.TemporaryData
                    If lrColumn.DataTypeIsText Then lsSQLQuery &= "'"
                    liInd += 1
                Next

                Dim lrRecordset = Me.DatabaseManager.Connection.GO(lsSQLQuery)

                Return lrRecordset.Facts.Count > 0

            Catch ex As Exception
                Return False
            End Try

        End Function


    End Class

End Namespace
