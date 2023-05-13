Imports System.Reflection

Namespace TableFactData
    Module TableFactData

        Public Sub AddFactData(ByVal arFactData As FBM.FactData)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelFactData"
                lsSQLQuery &= " VALUES("
                lsSQLQuery &= pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= "," & pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= ",'" & arFactData.Model.ModelId & "'"
                lsSQLQuery &= ",'" & arFactData.FactType.Id & "'"
                lsSQLQuery &= ",'" & arFactData.Fact.Symbol & "'"
                lsSQLQuery &= ",'" & arFactData.Role.Id & "'"
                lsSQLQuery &= ",'" & Strings.Left(arFactData.Data, 100) & "'"
                lsSQLQuery &= " )"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactData.AddFactData"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                pdbConnection.RollbackTrans()
            End Try

        End Sub

        Public Sub DeleteFactData(ByVal arFactData As FBM.FactData)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelFactData"
            lsSQLQuery &= " WHERE FactSymbol = '" & arFactData.Fact.Symbol & "'"
            lsSQLQuery &= "   AND ValueSymbol = '" & arFactData.Data & "'"
            lsSQLQuery &= "   AND ModelId = '" & arFactData.Model.ModelId & "'"

            pdbConnection.Execute(lsSQLQuery)

        End Sub

        Public Sub DeleteFactDataByFact(ByVal arFact As FBM.Fact)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelFactData"
            lsSQLQuery &= " WHERE FactSymbol = '" & arFact.Symbol & "'"
            lsSQLQuery &= "   AND ModelId = '" & arFact.Model.ModelId & "'"

            pdbConnection.Execute(lsSQLQuery)

        End Sub

        Public Function ExistsFactData(ByVal arFactData As FBM.FactData) As Boolean

            '------------------------------------------------------------------
            'The FactTypeId or RowId is the Symbol attribute in the Fact table
            '------------------------------------------------------------------
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelFactData"
                lsSQLQuery &= " WHERE FactSymbol = '" & Trim(arFactData.Fact.Symbol) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(arFactData.Model.ModelId) & "'"
                lsSQLQuery &= "   AND RoleId = '" & Trim(arFactData.Role.Id) & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsFactData = True
                Else
                    ExistsFactData = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function


        Public Sub UpdateFactData(ByVal arFactData As FBM.FactData)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelFactData"
                lsSQLQuery &= "   SET ValueSymbol = '" & Trim(arFactData.Data) & "',"
                lsSQLQuery &= "       FactTypeId = '" & Trim(arFactData.FactType.Id) & "'"
                lsSQLQuery &= " WHERE FactSymbol = '" & Trim(arFactData.Fact.Symbol) & "'"
                lsSQLQuery &= "   AND RoleId = '" & arFactData.Role.Id & "'"
                lsSQLQuery &= "   AND ModelId = '" & arFactData.Model.ModelId & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactData.UpdateFactData"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module
End Namespace