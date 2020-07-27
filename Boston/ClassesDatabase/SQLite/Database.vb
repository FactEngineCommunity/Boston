Imports System
Imports System.Collections.Generic
Imports System.Data.SQLite
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace Database

    Module SQLiteDatabase

        Public Function CreateConnection(ByVal asConnectionString As String) As SQLiteConnection

            Dim sqlite_conn As SQLiteConnection
            Try
                sqlite_conn = New SQLiteConnection(asConnectionString)
                sqlite_conn.Open()
            Catch ex As Exception
                Return Nothing
            End Try

            Return sqlite_conn
        End Function

        Public Function getReaderForSQL(ByVal conn As SQLiteConnection, ByVal asSQLQuery As String) As SQLiteDataReader

            Dim sqlite_datareader As SQLiteDataReader
            Dim sqlite_cmd As SQLiteCommand
            sqlite_cmd = conn.CreateCommand()
            sqlite_cmd.CommandText = asSQLQuery
            sqlite_datareader = sqlite_cmd.ExecuteReader()

            Return sqlite_datareader

        End Function
    End Module

End Namespace
