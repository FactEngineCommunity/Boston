Imports System.Reflection
Imports System.Data.SQLite

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
                Return Me._eof
            End Get
        End Property

        Default Public Overrides Property Item(ByVal asItemValue As String) As Object
            Get
                Try
                    If Me._RowIndex = -1 And Me.SQLiteDataReader.HasRows Then
                        Me.Read()
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
                        Dim value As Object = Me.SQLiteDataReader.GetValue(Me.SQLiteDataReader.GetOrdinal(asItemValue))
                        Select Case Me.SQLiteDataReader.GetFieldType(Me.SQLiteDataReader.GetOrdinal(asItemValue))
                            Case GetType(Int32)
                                Return New With {.value = CInt(value)}
                            Case GetType(String)
                                Return New With {.value = CStr(NullVal(value, ""))}
                                'Return CStr(value)
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
                    Return Nothing
                End Try
            End Get
            Set(ByVal value As Object)
                Throw New NotImplementedException("Cannot change values in a SQLiteDataReader result set.")
            End Set

        End Property

        Public Overrides Sub Close()
            Me.SQLiteDataReader.Close()
        End Sub

        Public Overrides Function MoveNext()
            Try
                Return Me.SQLiteDataReader.Read
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
