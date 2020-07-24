Namespace TableConcept

    Public Module TableConcept

        Public Sub AddConcept(ByVal ar_concept As FBM.Concept)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery &= "INSERT INTO MetaModelConcept"
                lsSQLQuery &= "  VALUES("
                lsSQLQuery &= "'" & Trim(Replace(ar_concept.Symbol, "'", "`")) & "'"
                lsSQLQuery &= ")"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableConcept.AddConcept"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub addConceptWhereNotExists()

            '20200725-This works and is very fast
            '        INSERT INTO MetaModelConcept (Symbol)
            'Select Case TOP 1 'Hi there' AS Symbol  
            'From MetaModelConcept
            'Where
            '  Not EXISTS(SELECT TOP 1 Symbol 
            '                From MetaModelConcept
            '                Where Symbol = 'Hi there');

        End Sub

        ''' <summary>
        ''' Deletes any Concepts no longer required by any Model/Page within Richmond.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub DeleteUnneededConcepts()

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelConcept MMC"
            lsSQLQuery &= " WHERE Symbol NOT IN ("
            lsSQLQuery &= "   SELECT Symbol FROM MetaModelModelDictionary MMD"
            lsSQLQuery &= "    WHERE MMD.Symbol = MMC.Symbol"
            lsSQLQuery &= "                     )"
            lsSQLQuery &= "   AND Symbol NOT IN ("
            lsSQLQuery &= "   SELECT Symbol FROM ModelConceptInstance MCI"
            lsSQLQuery &= "    WHERE MCI.Symbol = MMC.Symbol"
            lsSQLQuery &= "                     )"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsConcept(ByVal ar_concept As FBM.Concept) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelConcept"
                lsSQLQuery &= " WHERE Symbol = '" & Trim(Replace(ar_concept.Symbol, "'", "`")) & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value = 0 Then
                    ExistsConcept = False
                Else
                    ExistsConcept = True
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableConcept.ExistsConcept"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Function

        Public Sub UpdateConcept(ByVal ar_concept As FBM.Concept)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = " UPDATE MetaModelConcept"
                lsSQLQuery &= "   SET Symbol = '" & Trim(Replace(ar_concept.Symbol, "'", "`")) & "'"
                lsSQLQuery &= " WHERE Symbol = '" & Trim(Replace(ar_concept.Symbol, "'", "`")) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableConcept.UpdateConcept"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module
End Namespace