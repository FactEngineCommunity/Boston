Namespace TableFactTypeDerivationText

    Module tableFactTypeDerivationText

        Public Sub DeleteFactTypeDerivationTextInstance(ByVal arFactTypeDerivationTextInstance As FBM.FactTypeDerivationText)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arFactTypeDerivationTextInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arFactTypeDerivationTextInstance.FactTypeInstance.Id) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactTypeDerivationText.ToString & "'"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Sub GetFactTypeDerivationTextDetails(ByRef arFactTypeDerivationText As FBM.FactTypeDerivationText)

            Dim lrConceptInstance As New FBM.ConceptInstance

            lrConceptInstance.ModelId = arFactTypeDerivationText.model.ModelId
            lrConceptInstance.PageId = arFactTypeDerivationText.Page.PageId
            lrConceptInstance.Symbol = arFactTypeDerivationText.FactTypeInstance.Id
            lrConceptInstance.ConceptType = pcenumConceptType.FactTypeDerivationText
            lrConceptInstance.X = 0
            lrConceptInstance.Y = 0

            If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                '20210827-VM-Removed the below to vastly speed things up.
                'Call TableConceptInstance.GetConceptInstanceDetails(lrConceptInstance)
                arFactTypeDerivationText.X = lrConceptInstance.X
                arFactTypeDerivationText.Y = lrConceptInstance.Y
            End If

        End Sub

        ''' <summary>
        ''' Modifies the key of the ModelConceptInstance table.
        '''   NB IMPORTANT: Does not match on 'PageId' because if the key is changed on one Page in the Model the key must change on all Pages in the Model.
        ''' </summary>
        ''' <param name="arFactTypeDerivationTextInstance"></param>
        ''' <param name="as_new_key"></param>
        ''' <remarks></remarks>
        Public Sub ModifyKey(ByVal arFactTypeDerivationTextInstance As FBM.FactTypeDerivationText, ByVal as_new_key As String)

            Dim lsSQLQuery As String = ""

            Try
                'NB IMPORTANT: Does not match on 'PageId' because if the key is changed on one Page in the Model the key must change on all Pages in the Model.
                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET Symbol = '" & Replace(Trim(as_new_key), "'", "`") & "'"
                lsSQLQuery &= " WHERE ModelId = '" & Replace(arFactTypeDerivationTextInstance.Model.ModelId, "'", "`") & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(arFactTypeDerivationTextInstance.FactTypeInstance.Id, "'", "`") & "'"
                lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactTypeDerivationText.ToString & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeDerivationTextInstance.ModifyKey"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "SQL: " & lsSQLQuery
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module

End Namespace
