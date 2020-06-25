Namespace Persistence.Beta11

    Public Class MDObj : Implements IMDPersistenceItem

        Friend Bin As New Parser.Bin.BinObj()

        Public obj() As Byte = Nothing

        Public Sub New()

        End Sub

        Friend Sub Load()
            Me.Bin = New Parser.Bin.BinObj()
            If Me.obj Is Nothing Then Exit Sub

            'Deserialise, decompress object data
            Try
                Me.Bin = CType(Tools.Reflection.ConvertBytesToObject(Tools.Compression.Decompress(Me.obj)), Parser.Bin.BinObj)

            Catch ex As Exception
                If My.Application.EXEC_ErrorSensitivity = My.MyApplication.ErrorSensitivity.Standard Then
                    'On fail, start from scratch - clear the package data
                    Me.Bin = New Parser.Bin.BinObj()
                Else
                    'Otherwise report
                    Throw
                End If

            End Try
        End Sub

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New MDObj()
            Array.Copy(Me.obj, Copy.obj, Me.obj.Length)
            Return Copy
        End Function

    End Class

End Namespace