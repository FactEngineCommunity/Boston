Imports System.Reflection

Namespace TableORMFactTypeReadingRole

    Module tableORMFactTypeReadingRole

        Public Sub AddFactTypeReadingRole(ByVal arFactTypeReadingRole As FBM.FactTypeReadingRole)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelFactTypeReadingRole"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(arFactTypeReadingRole.FactTypeReadingId) & "'"
                lsSQLQuery &= " ,'" & Trim(arFactTypeReadingRole.RoleId) & "'"
                lsSQLQuery &= " ," & arFactTypeReadingRole.SequenceNr
                lsSQLQuery &= " ,'" & Trim(Replace(arFactTypeReadingRole.PreBoundText, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactTypeReadingRole.PostBoundText, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arFactTypeReadingRole.PredicatePart, "'", "`")) & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeReading.AddFactTypeReadingRole"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteFactTypeReadingRole(ByRef arFactTypeReadingRole As FBM.FactTypeReadingRole)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = "DELETE FROM MetaModelFactTypeReading"
                lsSQLQuery = lsSQLQuery & " WHERE ModelId = '" & Trim(arFactTypeReadingRole.Model.ModelId) & "'"
                lsSQLQuery = lsSQLQuery & "   AND FactTypeReadingId = '" & Trim(arFactTypeReadingRole.FactTypeReadingId) & "'"
                lsSQLQuery = lsSQLQuery & "   AND RoleId = '" & Trim(arFactTypeReadingRole.RoleId) & "'"


                pdbConnection.Execute(lsSQLQuery)
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeReading.DeleteFactTypeReadingRole"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Function GetFactTypeReadingRolesByFactTypeReading(ByVal arFactTypeReading As FBM.FactTypeReading) As List(Of FBM.FactTypeReadingRole)

            Dim larFactTypeReadingRole As New List(Of FBM.FactTypeReadingRole)

            Return larFactTypeReadingRole

        End Function

        Public Sub UpdateFactTypeReadingRole(ByRef arFactTypeReadingRole As FBM.FactTypeReadingRole)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelFactTypeReadingRole"
                lsSQLQuery &= "   SET FactTypeReadingId = FactTypeReadingId,"
                lsSQLQuery &= "       PreBoundText = '" & Trim(Replace(arFactTypeReadingRole.PreBoundText, "'", "`")) & "'"
                lsSQLQuery &= "       PostBoundText = '" & Trim(Replace(arFactTypeReadingRole.PostBoundText, "'", "`")) & "'"
                lsSQLQuery &= "       PredicatePart = '" & Trim(Replace(arFactTypeReadingRole.PredicatePart, "'", "`")) & "'"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arFactTypeReadingRole.Model.ModelId) & "'"
                lsSQLQuery &= "   AND FactTypeReadingId = '" & Trim(arFactTypeReadingRole.FactTypeReadingId) & "'"
                lsSQLQuery &= "   AND RoleId = '" & Trim(arFactTypeReadingRole.RoleId) & "'"
                lsSQLQuery &= "   AND SequenceNr = " & arFactTypeReadingRole.SequenceNr

                pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeReading.UpdateFactTypeReadingRole"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module

End Namespace
