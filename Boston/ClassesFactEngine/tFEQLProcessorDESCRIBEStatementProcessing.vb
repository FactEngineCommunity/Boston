Namespace FEQL
    Partial Public Class Processor

        Public Function ProcessDESCRIBEStatement(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lrRecordset = New ORMQL.Recordset(FactEngine.pcenumFEQLStatementType.DESCRIBEStatement)

            Try
                'Richmond.WriteToStatusBar("Processsing WHICH Statement.", True)
                Me.DESCRIBEStatement = New FEQL.DESCRIBEStatement

                Call Me.GetParseTreeTokensReflection(Me.DESCRIBEStatement, Me.Parsetree.Nodes(0))

                '----------------------------------------
                'Create the HeadNode for the QueryGraph
                Dim lrFBMModelObject As FBM.ModelObject = Me.Model.GetModelObjectByName(Me.DESCRIBEStatement.MODELELEMENTNAME)
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.DESCRIBEStatement.MODELELEMENTNAME & "'.")

                lrRecordset.ModelElement = lrFBMModelObject

                Return lrRecordset

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
