Imports System.Reflection

Namespace TableConceptInstance

    Module Table_concept_instance

        Public Sub AddConceptInstance(ByVal arConceptInstance As FBM.ConceptInstance)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "INSERT INTO ModelConceptInstance"
                lsSQLQuery &= "  VALUES("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= ",'" & Trim(arConceptInstance.ModelId) & "'"
                lsSQLQuery &= ",'" & Trim(arConceptInstance.PageId) & "'"
                lsSQLQuery &= ",'" & Replace(Trim(arConceptInstance.Symbol), "'", "`") & "'"
                lsSQLQuery &= ",'" & arConceptInstance.ConceptType.ToString & "'"
                lsSQLQuery &= ",'" & arConceptInstance.RoleId.ToString & "'"
                lsSQLQuery &= "," & arConceptInstance.X
                lsSQLQuery &= "," & arConceptInstance.Y
                lsSQLQuery &= ",100" '& arConceptInstance.orientation & "'"
                lsSQLQuery &= "," & arConceptInstance.Visible
                lsSQLQuery &= ")"


                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String = ""
                lsMessage = "Error: TableConceptInstance.AddConceptInstance"
                lsMessage &= vbCrLf & vbCrLf & "ModelId: " & arConceptInstance.ModelId
                lsMessage &= vbCrLf & "PageId: " & arConceptInstance.PageId
                lsMessage &= vbCrLf & "Symbol:" & arConceptInstance.Symbol
                lsMessage &= vbCrLf & "ConceptType:" & arConceptInstance.ConceptType.ToString
                lsMessage &= vbCrLf & ex.Message
                pdbConnection.RollbackTrans()
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteConceptInstance(ByVal arConceptInstance As FBM.ConceptInstance)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "DELETE FROM ModelConceptInstance"
                lsSQLQuery &= " WHERE ModelId = '" & arConceptInstance.ModelId & "'"
                lsSQLQuery &= "   AND PageId = '" & arConceptInstance.PageId & "'"
                lsSQLQuery &= "   AND Symbol = '" & arConceptInstance.Symbol & "'"
                lsSQLQuery &= "   AND ConceptType = '" & arConceptInstance.ConceptType.ToString & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub DeleteConceptInstancesForPage(ByVal ar_page As FBM.Page)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery &= "DELETE FROM ModelConceptInstance"
                lsSQLQuery &= "  WHERE PageId = '" & Trim(ar_page.PageId) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableconceptInstance.DeleteConceptInstanceForPage"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub GetConceptInstanceDetails(ByRef arConceptInstance As FBM.ConceptInstance)

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT ci.*"
            lsSQLQuery &= "  FROM ModelConceptInstance ci"
            lsSQLQuery &= " WHERE ci.PageId = '" & Trim(arConceptInstance.PageId) & "'"
            lsSQLQuery &= "   AND ci.ModelId = '" & Trim(arConceptInstance.ModelId) & "'"
            lsSQLQuery &= "   AND ci.Symbol = '" & Trim(arConceptInstance.Symbol) & "'"
            lsSQLQuery &= "   AND ci.ConceptType = '" & arConceptInstance.ConceptType.ToString & "'"
            lsSQLQuery &= "   AND ci.RoleId = '" & arConceptInstance.RoleId.ToString & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                arConceptInstance.X = lREcordset("x").Value
                arConceptInstance.Y = lREcordset("y").Value
                arConceptInstance.Orientation = lREcordset("Orientation").Value
                arConceptInstance.Visible = lREcordset("IsVisible").Value
            End If

            lREcordset.Close()

        End Sub

        Public Function ExistsConceptInstance(ByVal arConceptInstance As FBM.ConceptInstance) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arConceptInstance.ModelId) & "'"
            lsSQLQuery &= "   AND PageId = '" & Trim(arConceptInstance.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(Replace(arConceptInstance.Symbol, "'", "''")) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & Trim(arConceptInstance.ConceptType.ToString) & "'"
            lsSQLQuery &= "   AND RoleId = '" & Trim(arConceptInstance.RoleId.ToString) & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value = 0 Then
                ExistsConceptInstance = False
            Else
                ExistsConceptInstance = True
            End If

            lREcordset.Close()

        End Function

        Public Function ExistsConceptInstanceByModelPageConceptTypeRoleId(ByVal arConceptInstance As FBM.ConceptInstance) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arConceptInstance.ModelId) & "'"
            lsSQLQuery &= "   AND PageId = '" & Trim(arConceptInstance.PageId) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & Trim(arConceptInstance.ConceptType.ToString) & "'"
            lsSQLQuery &= "   AND RoleId = '" & Trim(arConceptInstance.RoleId.ToString) & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value = 0 Then
                Return False
            Else
                Return True
            End If

            lREcordset.Close()

        End Function


        Function GetNextSymbol() As Integer

            GetNextSymbol = System.Guid.NewGuid.ToString

        End Function

        Public Sub ModifyKey(ByVal arModelObject As FBM.ModelObject, ByVal asNewKey As String)

            Dim lsSQLQuery As String
            Dim asModelObjectId As String = ""

            Try
                '----------------------------------
                'Find the key for the ModelObject
                '----------------------------------
                Select Case arModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        asModelObjectId = arModelObject.Id
                    Case Is = pcenumConceptType.ValueType
                        asModelObjectId = arModelObject.Id
                    Case Is = pcenumConceptType.FactType
                        asModelObjectId = arModelObject.Id
                    Case Else
                        Throw New ApplicationException("Error: TableConceptInstance.ModifyKey: Unknown ModelObject.ConceptType")
                End Select

                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET Symbol = '" & Trim(asNewKey) & "'"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arModelObject.Model.ModelId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & asModelObjectId & "'"
                lsSQLQuery &= "   AND ConceptType = '" & arModelObject.ConceptType.ToString & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableConceptInstance.ModifyKey"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub ModifySymbol(ByVal arConceptInstance As FBM.ConceptInstance, ByVal as_new_Symbol As String)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET Symbol = '" & Trim(as_new_Symbol) & "'"
                lsSQLQuery &= "       ,IsVisible = " & arConceptInstance.Visible
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arConceptInstance.ModelId) & "'"
                lsSQLQuery &= "   AND PageId = '" & Trim(arConceptInstance.PageId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Trim(arConceptInstance.Symbol) & "'"
                lsSQLQuery &= "   AND ConceptType = '" & Trim(arConceptInstance.ConceptType.ToString) & "'"
                lsSQLQuery &= "   AND RoleId = '" & Trim(arConceptInstance.RoleId.ToString) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception

                pdbConnection.RollbackTrans()

                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub UpdateConceptInstance(ByVal arConceptInstance As FBM.ConceptInstance)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET x = " & arConceptInstance.X
                lsSQLQuery &= "       ,y = " & arConceptInstance.Y
                lsSQLQuery &= "       ,ConceptType = '" & Trim(arConceptInstance.ConceptType.ToString) & "'"
                lsSQLQuery &= "       ,IsVisible = " & arConceptInstance.Visible
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arConceptInstance.ModelId) & "'"
                lsSQLQuery &= "   AND PageId = '" & Trim(arConceptInstance.PageId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Trim(arConceptInstance.Symbol) & "'"
                lsSQLQuery &= "   AND ConceptType = '" & Trim(arConceptInstance.ConceptType.ToString) & "'"
                lsSQLQuery &= "   AND RoleId = '" & Trim(arConceptInstance.RoleId.ToString) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub UpdateConceptInstanceByModelPageConceptTypeRoleId(ByVal arConceptInstance As FBM.ConceptInstance,
                                                                     Optional asOldSymbol As String = Nothing)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET x = " & arConceptInstance.X
                lsSQLQuery &= "       ,y = " & arConceptInstance.Y
                lsSQLQuery &= "       ,Symbol = '" & Trim(arConceptInstance.Symbol) & "'"
                lsSQLQuery &= "       ,IsVisible = " & arConceptInstance.Visible
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arConceptInstance.ModelId) & "'"
                lsSQLQuery &= "   AND PageId = '" & Trim(arConceptInstance.PageId) & "'"
                lsSQLQuery &= "   AND ConceptType = '" & Trim(arConceptInstance.ConceptType.ToString) & "'"
                lsSQLQuery &= "   AND RoleId = '" & Trim(arConceptInstance.RoleId.ToString) & "'"
                If asOldSymbol IsNot Nothing Then
                    lsSQLQuery &= "   AND Symbol = '" & Trim(asOldSymbol) & "'"
                End If

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                pdbConnection.RollbackTrans()
            End Try

        End Sub

    End Module

End Namespace