Namespace TableFactTypeReadingInstance

    Module tableFactTypeReadingInstance

        Public Sub DeleteFactTypeReadingInstance(ByVal arFactTypeReadingInstance As FBM.FactTypeReadingInstance)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arFactTypeReadingInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arFactTypeReadingInstance.FactType.Id) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactTypeReading.ToString & "'"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Sub GetFactTypeReadingInstanceDetails(ByRef arFactTypeReadingInstance As FBM.FactTypeReadingInstance)

            Dim lrConceptInstance As New FBM.ConceptInstance

            lrConceptInstance.ModelId = arFactTypeReadingInstance.Model.ModelId
            lrConceptInstance.PageId = arFactTypeReadingInstance.Page.PageId
            lrConceptInstance.Symbol = arFactTypeReadingInstance.FactType.Id
            lrConceptInstance.ConceptType = pcenumConceptType.FactTypeReading
            lrConceptInstance.X = 0
            lrConceptInstance.Y = 0

            If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                '20210827-Removed below to vastly speed things up.
                'Call TableConceptInstance.GetConceptInstanceDetails(lrConceptInstance)
                arFactTypeReadingInstance.X = lrConceptInstance.X
                arFactTypeReadingInstance.Y = lrConceptInstance.Y
            End If

        End Sub

        ''' <summary>
        ''' Modifies the key of the ModelConceptInstance table.
        '''   NB IMPORTANT: Does not match on 'PageId' because if the key is changed on one Page in the Model the key must change on all Pages in the Model.
        ''' </summary>
        ''' <param name="arFactTypeReadingInstance"></param>
        ''' <param name="as_new_key"></param>
        ''' <remarks></remarks>
        Public Sub ModifyKey(ByVal arFactTypeReadingInstance As FBM.FactTypeReadingInstance, ByVal as_new_key As String)

            Dim lsSQLQuery As String = ""

            Try
                'NB IMPORTANT: Does not match on 'PageId' because if the key is changed on one Page in the Model the key must change on all Pages in the Model.
                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET Symbol = '" & Replace(Trim(as_new_key), "'", "`") & "'"
                lsSQLQuery &= " WHERE ModelId = '" & Replace(arFactTypeReadingInstance.Model.ModelId, "'", "`") & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(arFactTypeReadingInstance.FactType.Id, "'", "`") & "'"
                lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactTypeReading.ToString & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeReadingInstance.ModifyKey"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "SQL: " & lsSQLQuery
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module

End Namespace
