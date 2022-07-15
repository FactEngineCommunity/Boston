Namespace TableFactTypeName

    Module tableFactTypeName

        Public Sub DeleteFactTypeNameInstance(ByVal arFactTypeNameInstance As FBM.FactTypeName)

            Dim lsSQLQuery As String = ""
            Dim lsSQLQuery2 As String = ""

            lsSQLQuery = "DELETE FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arFactTypeNameInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arFactTypeNameInstance.FactTypeInstance.Id) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactTypeName.ToString & "'"
            lsSQLQuery &= "   AND InstanceNumber = " & arFactTypeNameInstance.InstanceNumber

            lsSQLQuery2 = "UPDATE ModelConceptInstance"
            lsSQLQUery2 &= " SET InstanceNumber = InstanceNumber - 1"
            lsSQLQuery2 &= " WHERE PageId = '" & Trim(arFactTypeNameInstance.Page.PageId) & "'"
            lsSQLQuery2 &= "    AND Symbol = '" & Trim(arFactTypeNameInstance.Id) & "'"
            lsSQLQuery2 &= "    AND ConceptType = '" & pcenumConceptType.FactType.ToString & "'"
            lsSQLQuery2 &= "    AND InstanceNumber > " & arFactTypeNameInstance.InstanceNumber

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            Call pdbConnection.Execute(lsSQLQuery2)
            pdbConnection.CommitTrans()

        End Sub

        Public Sub GetFactTypeNameDetails(ByRef arFactTypeName As FBM.FactTypeName)

            Dim lrConceptInstance As New FBM.ConceptInstance

            lrConceptInstance.ModelId = arFactTypeName.model.ModelId
            lrConceptInstance.PageId = arFactTypeName.Page.PageId
            lrConceptInstance.Symbol = arFactTypeName.Name
            lrConceptInstance.ConceptType = pcenumConceptType.FactTypeName
            lrConceptInstance.InstanceNumber = arFactTypeName.InstanceNumber
            lrConceptInstance.X = 0
            lrConceptInstance.Y = 0

            If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                '20210827-Removed below to vastly speed things up.
                'Call TableConceptInstance.GetConceptInstanceDetails(lrConceptInstance)
                arFactTypeName.X = lrConceptInstance.X
                arFactTypeName.Y = lrConceptInstance.Y
                arFactTypeName.FactTypeInstance.ShowFactTypeName = lrConceptInstance.Visible
            Else
                arFactTypeName.FactTypeInstance.ShowFactTypeName = False
            End If

        End Sub

        ''' <summary>
        ''' Modifies the key of the ModelConceptInstance table.
        '''   NB IMPORTANT: Does not match on 'PageId' because if the key is changed on one Page in the Model the key must change on all Pages in the Model.
        ''' </summary>
        ''' <param name="arFactTypeNameInstance"></param>
        ''' <param name="as_new_key"></param>
        ''' <remarks></remarks>
        Public Sub ModifyKey(ByVal arFactTypeNameInstance As FBM.FactTypeName, ByVal as_new_key As String)

            Dim lsSQLQuery As String = ""

            Try
                'NB IMPORTANT: Does not match on 'PageId' because if the key is changed on one Page in the Model the key must change on all Pages in the Model.
                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET Symbol = '" & Replace(Trim(as_new_key), "'", "`") & "'"
                lsSQLQuery &= " WHERE ModelId = '" & Replace(arFactTypeNameInstance.Model.ModelId, "'", "`") & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(arFactTypeNameInstance.FactTypeInstance.Id, "'", "`") & "'"
                lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactTypeName.ToString & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeNameInstance.ModifyKey"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "SQL: " & lsSQLQuery
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module

End Namespace
