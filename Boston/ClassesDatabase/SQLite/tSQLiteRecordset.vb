Imports System.Reflection
Imports System.Data.SQLite

Namespace SQLite
    Public Class Recordset
        Inherits Database.GenericRecordset

        Public Shadows ActiveConnection As SQLiteConnection
        Public Shadows CursorType As Integer

        Private _EOF As Boolean = False

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
                    If asItemValue.IsNumeric Then
                        Dim liIndex = CInt(asItemValue)
                        Dim value As Object = Me.SQLiteDataReader.GetValue(liIndex)
                        Select Case value.GetType
                            Case GetType(Integer)
                                Return CInt(value)
                            Case GetType(Double)
                                Return CDbl(value)
                            Case GetType(String)
                                Return CStr(value)
                            Case GetType(Boolean)
                                Return CBool(value)
                            Case GetType(DateTime)
                                Return CDate(value)
                            Case GetType(Byte())
                                Return CType(value, Byte())
                                ' handle binary data
                            Case Else
                                Return value
                        End Select
                    Else
                        Dim value As Object = Me.SQLiteDataReader.GetValue(Me.SQLiteDataReader.GetOrdinal(asItemValue))
                        Select Case Me.SQLiteDataReader.GetFieldType(Me.SQLiteDataReader.GetOrdinal(asItemValue))
                            Case GetType(Int32)
                                Return CInt(value)
                            Case GetType(String)
                                Return CStr(value)
                            Case GetType(Double)
                                Return CDbl(value)
                            Case GetType(Boolean)
                                Return CBool(value)
                            Case GetType(DateTime)
                                Return CDate(value)
                            Case GetType(Byte())
                                Return CType(value, Byte())
                            Case Else

                                Return Nothing
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
                Dim lSQLiteCommand As New SQLiteCommand

                lSQLiteCommand.Connection = Me.ActiveConnection

                Me.SQLiteDataReader = lSQLiteCommand.ExecuteReader(asQuery)

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
