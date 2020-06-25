Namespace Tools

    Friend Class Conv
        'Conversion functions

        ''' <summary>
        ''' Convert any object value to string
        ''' </summary>
        ''' <param name="Value">Value to convert</param>
        ''' <param name="NullAsEmpty">If true, return nothing if null or an invalid string, if false return empty "" string</param>
        Public Shared Shadows Function ToString(ByVal Value As Object, _
                                                Optional ByVal NullAsEmpty As Boolean = False) As String
            Try
                If Value Is DBNull.Value Then
                    If NullAsEmpty Then
                        Return ""
                    Else
                        Return Nothing
                    End If
                End If
                Return Convert.ToString(Value)
            Catch ex As Exception
                If NullAsEmpty Then
                    Return ""
                Else
                    Return Nothing
                End If
            End Try
        End Function

        ''' <summary>
        ''' Convert any object value to Int32, return nothing if null or an invalid integer
        ''' </summary>
        ''' <param name="Value">Value to convert</param>
        Public Shared Function ToInteger(ByVal Value As Object) As Integer
            Try
                If Value Is DBNull.Value Then Return 0
                Return Convert.ToInt32(Value)
            Catch ex As Exception
                Try
                    Return CInt(Value)
                Catch e As Exception
                    Return 0
                End Try
            End Try
        End Function

        ''' <summary>
        ''' Convert any object value to Int64, return nothing if null or an invalid integer
        ''' </summary>
        ''' <param name="Value">Value to convert</param>
        Public Shared Function ToLong(ByVal Value As Object) As Long
            Try
                If Value Is DBNull.Value Then Return 0
                Return Convert.ToInt64(Value)
            Catch ex As Exception
                Try
                    Return CLng(Value)
                Catch e As Exception
                    Return 0
                End Try
            End Try
        End Function

        ''' <summary>
        ''' Convert any object value to double, return nothing if null or an invalid double
        ''' </summary>
        ''' <param name="Value">Value to convert</param>
        Public Shared Function ToDouble(ByVal Value As Object) As Double
            Try
                If Value Is DBNull.Value Then Return 0
                Return Convert.ToDouble(Value)
            Catch ex As Exception
                Return 0
            End Try
        End Function

        ''' <summary>
        ''' Convert any object value to decimal, return nothing if null or an invalid decimal
        ''' </summary>
        ''' <param name="Value">Value to convert</param>
        Public Shared Function ToDecimal(ByVal Value As Object) As Decimal
            Try
                If Value Is DBNull.Value Then Return 0
                Return Convert.ToDecimal(Value)
            Catch ex As Exception
                Return 0
            End Try
        End Function

        ''' <summary>
        ''' Convert any object value to DateTime, return nothing if null or an invalid DateTime
        ''' </summary>
        ''' <param name="Value">Value to convert</param>
        Public Shared Function ToDateTime(ByVal Value As Object) As Date
            Try
                If Value Is DBNull.Value Then Return Nothing
                Return Convert.ToDateTime(Value)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Convert any object value to boolean, return nothing if null or an invalid boolean
        ''' </summary>
        ''' <param name="Value">Value to convert</param>
        Public Shared Function ToBoolean(ByVal Value As Object) As Boolean
            Try
                If Value Is DBNull.Value Then Return False
                Return Convert.ToBoolean(Value)
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Class

End Namespace