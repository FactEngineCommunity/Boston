Namespace TableRoleName

    Module TableRoleName

        Public Sub delete_RoleName_instance(ByVal arRoleNameInstance As FBM.RoleName)

            Dim lsSQLQuery As String = ""
            Dim lsSQLQuery2 As String = ""

            lsSQLQuery = "DELETE FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arRoleNameInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arRoleNameInstance.Name) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.RoleName.ToString & "'"
            lsSQLQuery &= "   AND InstanceNumber = " & arRoleNameInstance.InstanceNumber

            lsSQLQuery2 = "UPDATE ModelConceptInstance"
            lsSQLQUery2 &= " SET InstanceNumber = InstanceNumber - 1"
            lsSQLQuery2 &= " WHERE PageId = '" & Trim(arRoleNameInstance.Page.PageId) & "'"
            lsSQLQuery2 &= "    AND Symbol = '" & Trim(arRoleNameInstance.Id) & "'"
            lsSQLQuery2 &= "    AND ConceptType = '" & pcenumConceptType.RoleName.ToString & "'"
            lsSQLQuery2 &= "    AND InstanceNumber > " & arRoleNameInstance.InstanceNumber

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            Call pdbConnection.Execute(lsSQLQuery2)
            pdbConnection.CommitTrans()
        End Sub

        Public Sub GetRoleNameDetails(ByRef arRoleName As FBM.RoleName)

            Dim lrConceptInstance As New FBM.ConceptInstance

            Try

                lrConceptInstance.ModelId = arRoleName.Model.ModelId
                lrConceptInstance.PageId = arRoleName.Page.PageId
                lrConceptInstance.Symbol = arRoleName.Name
                lrConceptInstance.ConceptType = pcenumConceptType.RoleName
                lrConceptInstance.RoleId = arRoleName.RoleInstance.Id
                lrConceptInstance.InstanceNumber = arRoleName.InstanceNumber
                lrConceptInstance.X = 0
                lrConceptInstance.Y = 0

                'If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then (commented out to save time)
                Call TableConceptInstance.GetConceptInstanceDetails(lrConceptInstance)
                arRoleName.X = lrConceptInstance.X
                arRoleName.Y = lrConceptInstance.Y
                'End If
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableRoleName.GetRoleNameDetails"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Function getRoleName_instance_count_by_page(ByVal ar_page As FBM.Page) As Integer

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelConceptInstance CI"
            lsSQLQuery &= " WHERE ci.ModelId = '" & Trim(ar_page.Model.ModelId) & "'"
            lsSQLQuery &= "   AND ci.PageId = '" & Trim(ar_page.PageId) & "'"
            lsSQLQuery &= "   AND ci.Symbol = et.RoleName_id"
            lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.RoleName.ToString & "'"

            lREcordset.Open(lsSQLQuery)

            getRoleName_instance_count_by_page = lREcordset(0).Value

            lREcordset.Close()

        End Function


    End Module

End Namespace
