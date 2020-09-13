Namespace FEQL
    Partial Public Class Processor

        Public Function ProcessCREATEStatement(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lrRecordset = New ORMQL.Recordset(FactEngine.pcenumFEQLStatementType.DESCRIBEStatement)

            Try

                'Richmond.WriteToStatusBar("Processsing WHICH Statement.", True)
                Me.CREATEStatement = New FEQL.CREATEStatement

                Call Me.GetParseTreeTokensReflection(Me.CREATEStatement, Me.Parsetree.Nodes(0))

                Return New ORMQL.Recordset

            Catch ex As Exception
                If ex.InnerException Is Nothing Then
                    lrRecordset.ErrorString = ex.Message
                Else
                    lrRecordset.ErrorString = ex.InnerException.Message
                End If

                Return lrRecordset
            End Try

        End Function

    End Class
End Namespace
