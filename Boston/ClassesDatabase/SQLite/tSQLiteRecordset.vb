Imports System.Reflection
Imports System.Data.SQLite
Imports ADODB

Namespace SQLite
    Public Class Recordset
        Inherits Database.GenericRecordset

        Public Shadows ActiveConnection As FactEngine.SQLiteConnection
        Public Shadows CursorType As Integer

        Private _EOF As Boolean = False
        Private _RowIndex As Integer = -1

        Private SQLiteDataReader As SQLiteDataReader

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Overrides ReadOnly Property EOF As Boolean
            Get
                If Me.SQLiteDataReader.HasRows Then
                    Return Me._EOF
                Else
                    Return True
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property Fields As ADODB.Fields
            Get
                '====================
                Dim larField As New Database.Fields
                Dim schemaTable As DataTable = Me.SQLiteDataReader.GetSchemaTable()
                For Each row As DataRow In schemaTable.Rows
                    Dim fieldType As DataTypeEnum = Me.GetFieldType(row("DataType").ToString)
                    Dim lrField As New Database.FieldClass(row("ColumnName").ToString(),
                                                   fieldType,
                                                   Me.Item(row("ColumnName").ToString()))
                    larField.Append(lrField)
                Next
                Return larField

            End Get
        End Property

        Default Public Overrides Property Item(ByVal asItemValue As String) As Object
            Get
                Try
                    If Me._RowIndex = -1 And Me.SQLiteDataReader.HasRows Then
                        Dim lbSuccess = Me.Read()
                        If lbSuccess Then Me._RowIndex += 1

                    End If

                    If asItemValue.IsNumeric Then
                        Dim liIndex = CInt(asItemValue)
                        Dim value As Object = Me.SQLiteDataReader.GetValue(liIndex)
                        Select Case value.GetType
                            Case GetType(Integer)
                                Return New With {.value = CInt(value)}
                            Case GetType(Double)
                                Return New With {.value = CDbl(value)}
                            Case GetType(String)
                                Return New With {.value = CStr(value)}
                            Case GetType(Boolean)
                                Return New With {.value = CBool(value)}
                            Case GetType(DateTime)
                                Return New With {.value = CDate(value)}
                            Case GetType(Byte())
                                Return CType(value, Byte())
                                ' handle binary data
                            Case Else
                                Return New With {.value = value}
                        End Select
                    Else
                        Dim value As Object = System.DBNull.Value
                        Try
                            value = Me.SQLiteDataReader.GetValue(Me.SQLiteDataReader.GetOrdinal(asItemValue))
                        Catch
                        End Try

                        Select Case Me.SQLiteDataReader.GetFieldType(Me.SQLiteDataReader.GetOrdinal(asItemValue))
                            Case GetType(Int32)
                                Return New With {.value = CInt(value)}
                            Case GetType(String)
                                Return New With {.value = CStr(NullVal(value, ""))} '20230528-VM-At least this date, was: Return CStr(value)

                            Case GetType(Double)
                                Return New With {.value = CDbl(value)}
                            Case GetType(Boolean)
                                Return New With {.value = CBool(NullVal(value, False))}
                            Case GetType(DateTime)
                                Return New With {.value = CDate(value)}
                            Case GetType(Byte())
                                Return New With {.value = CType(value, Byte())}
                            Case Else
                                Return New With {.value = value}
                        End Select
                    End If
                Catch ex As Exception
                    Return System.DBNull.Value
                End Try
            End Get
            Set(ByVal value As Object)
                Throw New NotImplementedException("Cannot change values in a SQLiteDataReader result set.")
            End Set

        End Property

        Public Overrides Sub Close()
            'Me.SQLiteDataReader.Close()
        End Sub


        Private Function GetFieldType(dataType As String) As DataTypeEnum
            Select Case dataType
                Case "System.String", "System.Char", "System.Guid"
                    Return DataTypeEnum.adVarChar
                Case "System.Byte", "System.SByte", "System.Int16", "System.UInt16", "System.Int32", "System.UInt32", "System.Int64", "System.UInt64"
                    Return DataTypeEnum.adInteger
                Case "System.Single", "System.Double", "System.Decimal"
                    Return DataTypeEnum.adDouble
                Case "System.Boolean"
                    Return DataTypeEnum.adBoolean
                Case "System.DateTime"
                    Return DataTypeEnum.adDate
                Case Else
                    Return DataTypeEnum.adEmpty
            End Select
        End Function

        Private Function GetDefinedSize(dataType As ADODB.DataTypeEnum, columnSize As Object) As Integer
            'Get the defined size of the field based on the data type
            Select Case dataType
                Case ADODB.DataTypeEnum.adBoolean, ADODB.DataTypeEnum.adInteger, ADODB.DataTypeEnum.adDouble, ADODB.DataTypeEnum.adDate
                    Return 0
                Case ADODB.DataTypeEnum.adVarChar, ADODB.DataTypeEnum.adLongVarBinary
                    Return CInt(columnSize)
                Case Else
                    Throw New Exception("Unhandled ADODB data type: " + dataType.ToString())
            End Select
        End Function

        Public Overrides Function MoveFirst()
            'Here as a filler. Not needed/capabile-of-being-used for SLQiteDataReader
            If Me._RowIndex = -1 Then
                Call Me.Read()
                Return True
            Else
                'Can't do anything
                Return False
            End If
        End Function

        Public Overrides Function MoveNext()
            Try
                Dim lbSuccess = Me.SQLiteDataReader.Read
                If lbSuccess Then
                    Me._RowIndex += 1
                Else
                    Me._EOF = True
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Function

        Public Overrides Function Open(ByVal asQuery As String) As Boolean

            Try
                Dim sqlite_cmd As SQLiteCommand

                sqlite_cmd = Me.ActiveConnection.Connection.CreateCommand()
                sqlite_cmd.CommandText = asQuery

                Me.SQLiteDataReader = sqlite_cmd.ExecuteReader()

            Catch ex As Exception
                Dim lsMessage As String

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage.AppendDoubleLineBreak(asQuery)
                lsMessage.AppendDoubleLineBreak("ActiveConnection IsNot Nothing: " & (Me.ActiveConnection IsNot Nothing).ToString)
                Try
                    lsMessage.AppendLine("ActiveConnection.Connection IsNot Nothing: " & (Me.ActiveConnection.Connection IsNot Nothing).ToString)
                Catch ex1 As Exception
                    lsMessage.AppendLine("Error returning ActiveConnection.Connection")
                End Try

                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Public Overrides Function Read() As Boolean

            Try
                If Me.SQLiteDataReader.Read() Then
                    _EOF = False
                    Me._RowIndex += 1
                    Return True
                Else
                    _EOF = True
                    Return False
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

    End Class

End Namespace
