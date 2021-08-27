Imports System.Reflection

Namespace TableJoinPathRole

    Module tableORMJoinPathRole

        Sub AddJoinPathRole(ByVal arJoinPathRole As FBM.JoinPathRole)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelJoinPathRole"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(Replace(arJoinPathRole.Argument.Model.ModelId, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arJoinPathRole.Argument.Id, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arJoinPathRole.Role.Id, "'", "`")) & "'"
                lsSQLQuery &= " ," & arJoinPathRole.SequenceNr
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Function ExistsJoinPathRole(ByRef arJoinPathRole As FBM.JoinPathRole) As Boolean

            Try
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                '------------------------
                'Initialise return value
                '------------------------
                ExistsJoinPathRole = False

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelJoinPathRole"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(Replace(arJoinPathRole.Argument.Model.ModelId, "'", "`")) & "'"
                lsSQLQuery &= " AND RoleConstraintArgumentId = '" & Trim(Replace(arJoinPathRole.Argument.Id, "'", "`")) & "'"
                lsSQLQuery &= " AND RoleId = '" & Trim(Replace(arJoinPathRole.Role.Id, "'", "`")) & "'"
                lsSQLQuery &= " AND SequenceNr = " & arJoinPathRole.SequenceNr

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsJoinPathRole = True
                Else
                    ExistsJoinPathRole = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Function

        Public Function GetJoinPathForArgument(ByRef arRoleConstraintArgument As FBM.RoleConstraintArgument) As FBM.JoinPath

            Try
                Dim lsMessage As String = ""
                Dim lrRole As FBM.Role
                Dim lrJoinPath As New FBM.JoinPath
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset
                Dim lsId As String

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM MetaModelJoinPathRole"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arRoleConstraintArgument.Model.ModelId) & "'"
                lsSQLQuery &= "   AND RoleConstraintArgumentId = '" & Trim(arRoleConstraintArgument.Id) & "'"
                lsSQLQuery &= " ORDER BY SequenceNr"

                lREcordset.Open(lsSQLQuery)

                While Not lREcordset.EOF
                    lsId = Trim(lREcordset("RoleId").Value)
                    lrRole = arRoleConstraintArgument.Model.Role.Find(Function(x) x.Id = lsId)

                    If lrRole Is Nothing Then
                        lsMessage = "Couldn't find Role in Model for JoinPath"
                        lREcordset.Close()
                        Throw New Exception(lsMessage)
                    End If

                    lrJoinPath.RolePath.Add(lrRole)
                    lREcordset.MoveNext()
                End While

                lREcordset.Close()

                Call lrJoinPath.ConstructFactTypePath()

                Return lrJoinPath

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                Dim lsMessage As String = ""
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try


        End Function


    End Module

End Namespace
