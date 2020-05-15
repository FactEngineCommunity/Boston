Imports System.Text
Imports System.Reflection

Public Module publicORMFunctions


    Public Function CreateRandomFactData(ByVal aiDataType As pcenumORMDataType, ByVal aiDataTypeLength As Integer) As String

        Dim lsFactData As String = System.Guid.NewGuid.ToString
        Dim liRandomNumber As Integer

        Select Case aiDataType
            Case Is = pcenumORMDataType.DataTypeNotSet
                '-----------------------------------------------------------------
                'Do Nothing, just use the random GUID created for the lsFactData
                '-----------------------------------------------------------------
            Case Is = pcenumORMDataType.LogicalTrueFalse
                liRandomNumber = GenerateRandomInteger(1, 2)
                If liRandomNumber = 1 Then
                    lsFactData = "True"
                Else
                    lsFactData = "False"
                End If
            Case Is = pcenumORMDataType.LogicalYesNo
                liRandomNumber = GenerateRandomInteger(1, 2)
                If liRandomNumber = 1 Then
                    lsFactData = "Yes"
                Else
                    lsFactData = "No"
                End If
            Case Is = pcenumORMDataType.NumericAutoCounter
                '-------------------------------------------------------------------------------------
                'Code Safe: Not generally handled in this function, but return a value just the same
                '-------------------------------------------------------------------------------------
                lsFactData = GenerateRandomInteger(1, 1000).ToString
            Case Is = pcenumORMDataType.NumericDecimal, _
                      pcenumORMDataType.NumericFloatCustomPrecision, _
                      pcenumORMDataType.NumericFloatSinglePrecision, _
                      pcenumORMDataType.NumericFlotDoublePrecision, _
                      pcenumORMDataType.NumericMoney, _
                      pcenumORMDataType.NumericSignedBigInteger, _
                      pcenumORMDataType.NumericSignedInteger, _
                      pcenumORMDataType.NumericSignedSmallInteger, _
                      pcenumORMDataType.NumericUnsignedBigInteger, _
                      pcenumORMDataType.NumericUnsignedInteger, _
                      pcenumORMDataType.NumericUnsignedSmallInteger, _
                      pcenumORMDataType.NumericUnsignedTinyInteger
                lsFactData = GenerateRandomInteger(1, 1000).ToString
            Case Is = pcenumORMDataType.OtherObjectID
                '-----------------------------------------------------------------
                'Do Nothing, just use the random GUID created for the lsFactData
                '-----------------------------------------------------------------
            Case Is = pcenumORMDataType.OtherRowID
                lsFactData = GenerateRandomInteger(1, 1000).ToString
            Case Is = pcenumORMDataType.RawDataFixedLength
                If aiDataTypeLength > 0 Then
                    lsFactData = TruncateString(lsFactData, aiDataTypeLength)
                End If
            Case Is = pcenumORMDataType.RawDataLargeLength, _
                      pcenumORMDataType.RawDataOLEObject, _
                      pcenumORMDataType.RawDataPicture, _
                      pcenumORMDataType.RawDataVariableLength
                '-----------------------------------------------------------------
                'Do Nothing, just use the random GUID created for the lsFactData
                '-----------------------------------------------------------------
            Case Is = pcenumORMDataType.TemporalAutoTimestamp
                lsFactData = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds).ToString
            Case Is = pcenumORMDataType.TemporalDate
                lsFactData = Now.ToString("yyyy/MM/dd")
            Case Is = pcenumORMDataType.TemporalDateAndTime
                lsFactData = Now.ToString("yyyy/MM/dd HH:mm:ss")
            Case Is = pcenumORMDataType.TemporalTime
                lsFactData = Now.ToString("HH:mm:ss")
            Case Is = pcenumORMDataType.TextFixedLength
                If aiDataTypeLength > 0 Then
                    lsFactData = GenerateRandomString(GenerateRandomInteger(1, aiDataTypeLength))
                Else
                    lsFactData = GenerateRandomString(20)
                End If
            Case Is = pcenumORMDataType.TextLargeLength, _
                      pcenumORMDataType.TextVariableLength
                If aiDataTypeLength > 0 Then
                    lsFactData = GenerateRandomString(GenerateRandomInteger(1, aiDataTypeLength))
                Else
                    lsFactData = GenerateRandomString(GenerateRandomInteger(1, 20))
                End If
        End Select

        Return lsFactData

    End Function

    Public Function GenerateRandomInteger(ByVal Min As Integer, ByVal Max As Integer) As Integer
        ' by making Generator static, we preserve the same instance '
        ' (i.e., do not create new instances with the same seed over and over) '
        ' between calls '
        Dim Generator As System.Random = New System.Random()

        Return CInt(Math.Floor((Max - Min + 1) * Rnd())) + Min 'Generator.Next(Min, Max) + 1
    End Function

    Public Function TruncateString(asValue As String, aiMaxLength As Integer) As String

        If String.IsNullOrEmpty(asValue) Then
            Return asValue
        End If
        Return If(asValue.Length <= aiMaxLength, asValue, asValue.Substring(0, aiMaxLength))
    End Function

    Public Function GenerateRandomString(ByVal aiStringLength As Integer) As String

        Try
            Dim lsAlphabetCharacters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
            Dim sb As New StringBuilder
            For liInd As Integer = 1 To aiStringLength
                Dim idx As Integer = GenerateRandomInteger(0, lsAlphabetCharacters.Length - 1)
                sb.Append(lsAlphabetCharacters.Substring(idx, 1))
            Next
            Return sb.ToString()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return "Error"
        End Try


    End Function

End Module
