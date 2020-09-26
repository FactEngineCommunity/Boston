Imports System.Text.RegularExpressions

Namespace FBM

    Public Module FBMGeneral

        Public Function IsAcceptableObjectTypeName(ByVal asCandidateObjectTypeName As String) As Boolean

            Dim loRegularExpression = New Regex("^[a-zA-Z0-9_ ]*$")
            If loRegularExpression.IsMatch(asCandidateObjectTypeName) Then
                Return True
            Else
                Return False
            End If

        End Function

    End Module

End Namespace

