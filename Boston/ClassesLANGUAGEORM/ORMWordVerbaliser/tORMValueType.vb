Imports System.Reflection

Namespace FBM

    Partial Public Class ValueType

        Public Sub verbaliseCQL(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lbIncludeSingleQuotes As Boolean = False

                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifierLight(" is written as ")
                arORMWordVerbaliser.VerbaliseDataType(Me.DataType.ToString)

                Select Case Me.DataType
                    Case Is = pcenumORMDataType.NumericFloatCustomPrecision, _
                              pcenumORMDataType.NumericDecimal, _
                              pcenumORMDataType.NumericMoney
                        arORMWordVerbaliser.VerbaliseDataType("(")
                        arORMWordVerbaliser.VerbaliseDataType(Me.DataTypePrecision)
                        arORMWordVerbaliser.VerbaliseDataType(")")

                    Case Is = pcenumORMDataType.RawDataFixedLength, _
                              pcenumORMDataType.RawDataLargeLength, _
                              pcenumORMDataType.RawDataVariableLength, _
                              pcenumORMDataType.TextFixedLength, _
                              pcenumORMDataType.TextLargeLength, _
                              pcenumORMDataType.TextVariableLength
                        arORMWordVerbaliser.VerbaliseDataType("(")
                        arORMWordVerbaliser.VerbaliseDataType(Me.DataTypeLength)
                        arORMWordVerbaliser.VerbaliseDataType(")")
                    Case Else
                        'Nothing to add.
                End Select

                Select Case Me.DataType
                    Case Is = pcenumORMDataType.TextFixedLength, _
                              pcenumORMDataType.TextLargeLength, _
                              pcenumORMDataType.TextVariableLength
                        lbIncludeSingleQuotes = True
                End Select

                If Me.ValueConstraint.Count > 0 Then
                    arORMWordVerbaliser.VerbaliseQuantifierLight(" restricted to (")

                    Dim liInd As Integer = 0
                    For Each lsValueConstraintValue In Me.ValueConstraint
                        If liInd > 0 Then arORMWordVerbaliser.VerbaliseString(",")
                        If lbIncludeSingleQuotes Then arORMWordVerbaliser.VerbaliseString("'")
                        arORMWordVerbaliser.VerbaliseString(lsValueConstraintValue)
                        If lbIncludeSingleQuotes Then arORMWordVerbaliser.VerbaliseString("'")
                        liInd += 1
                    Next

                    arORMWordVerbaliser.VerbaliseQuantifierLight(")")
                End If

                arORMWordVerbaliser.WriteLine()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


    End Class

End Namespace
