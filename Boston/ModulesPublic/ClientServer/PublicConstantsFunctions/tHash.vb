Imports System.Security.Cryptography

Namespace ClientServer

    Public Module Hash

        Private Property lohashedBytes As Byte()

        Public Function getHash(ByVal asText As String) As String

            Dim loSHA256 As SHA256 = SHA256Managed.Create()

            lohashedBytes = loSHA256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(asText))
            Return BitConverter.ToString(lohashedBytes).Replace("-", "").ToLower()

        End Function

    End Module

End Namespace