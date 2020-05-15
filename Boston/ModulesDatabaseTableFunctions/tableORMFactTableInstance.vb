Namespace TableFactTableInstance

    Module zTableFactTableInstance

        Public Sub DeleteFactTableInstance(ByVal arFactTableInstance As FBM.FactTable)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arFactTableInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arFactTableInstance.FactTypeInstance.Id) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactTable.ToString & "'"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Function GetFactTableCountByPage(ByRef arPage As FBM.Page) As Integer

            '-------------------------------------------------------------------------------------------------
            'NB EnterpriseInstances never manifest within the database, they manifest within the database as
            '  SymbolInstances, so we must count the SymbolInstances that match the arguments.
            '-------------------------------------------------------------------------------------------------

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelConceptInstance ci"
            lsSQLQuery &= " WHERE ci.ModelId = '" & Trim(arPage.Model.ModelId) & "'"
            lsSQLQuery &= "   AND ci.PageId = '" & Trim(arPage.PageId) & "'"
            lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.FactTable.ToString & "'"


            lREcordset.Open(lsSQLQuery)

            GetFactTableCountByPage = lREcordset(0).Value

            lREcordset.Close()

        End Function

        Public Sub GetFactTableDetails(ByRef arFactTable As FBM.FactTable)

            Dim lrConceptInstance As New FBM.ConceptInstance

            lrConceptInstance.ModelId = arFactTable.model.ModelId
            lrConceptInstance.PageId = arFactTable.Page.PageId
            lrConceptInstance.Symbol = arFactTable.Id
            lrConceptInstance.ConceptType = pcenumConceptType.FactTable
            lrConceptInstance.X = 0
            lrConceptInstance.Y = 0

            If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                Call TableConceptInstance.GetConceptInstanceDetails(lrConceptInstance)
                arFactTable.X = lrConceptInstance.X
                arFactTable.Y = lrConceptInstance.Y
            Else
                '------------------------------------------
                'Simply return the referenced arFactTable.
                '  i.e. Do nothing.
                '------------------------------------------
            End If

        End Sub

        Function getFactTablesByPage(ByRef ar_page As FBM.Page) As List(Of FBM.FactTable)

            Dim lrFactTableInstance As FBM.FactTable
            Dim lsSQLQuery As String = ""
            Dim lRecordset As New ADODB.Recordset

            lRecordset.ActiveConnection = pdbConnection
            lRecordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT et.*, ci.*"
            lsSQLQuery &= "  FROM ModelConceptInstance ci"
            lsSQLQuery &= " WHERE ci.PageId = '" & Trim(ar_page.PageId) & "'"
            lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.FactTable.ToString & "'"

            lRecordset.Open(lsSQLQuery)

            '-----------------------------
            'Initialise the return value
            '-----------------------------

            getFactTablesByPage = New List(Of FBM.FactTable)

            If Not lRecordset.EOF Then
                While Not lRecordset.EOF
                    lrFactTableInstance = New FBM.FactTable
                    lrFactTableInstance.Model = ar_page.Model
                    lrFactTableInstance.Page = ar_page
                    lrFactTableInstance.Id = lRecordset("Symbol").Value
                    lrFactTableInstance.Name = lRecordset("Symbol").Value
                    lrFactTableInstance.FactTypeInstance = ar_page.FactTypeInstance.Find(Function(x) x.Id = lrFactTableInstance.Id)

                    lrFactTableInstance.X = lRecordset("x").Value
                    lrFactTableInstance.Y = lRecordset("y").Value

                    getFactTablesByPage.Add(lrFactTableInstance)
                    lRecordset.MoveNext()
                End While
            Else
                '----------------------------------------
                'Nothing to add
                '-------------------
            End If

        End Function

        ''' <summary>
        ''' Modifies the key of the ModelConceptInstance table.
        '''   NB IMPORTANT: Does not match on 'PageId' because if the key is changed on one Page in the Model the key must change on all Pages in the Model.
        ''' </summary>
        ''' <param name="arFactTableInstance"></param>
        ''' <param name="as_new_key"></param>
        ''' <remarks></remarks>
        Public Sub ModifyKey(ByVal arFactTableInstance As FBM.FactTable, ByVal as_new_key As String)

            Dim lsSQLQuery As String = ""

            Try
                'NB IMPORTANT: Does not match on 'PageId' because if the key is changed on one Page in the Model the key must change on all Pages in the Model.
                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET Symbol = '" & Replace(Trim(as_new_key), "'", "`") & "'"
                lsSQLQuery &= " WHERE ModelId = '" & Replace(arFactTableInstance.Model.ModelId, "'", "`") & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(arFactTableInstance.FactTypeInstance.Id, "'", "`") & "'"
                lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactTable.ToString & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTableInstance.ModifyKey"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "SQL: " & lsSQLQuery
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module

End Namespace