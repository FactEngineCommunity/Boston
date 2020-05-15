Imports System.Reflection

Namespace TableRoleConstraintArgument

    Module Table_RoleConstraintArgument

        Public Sub AddRoleConstraintArgument(ByVal arArgument As FBM.RoleConstraintArgument)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO MetaModelRoleConstraintArgument"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(arArgument.Model.ModelId) & "'"                
                lsSQLQuery &= " ,'" & Trim(arArgument.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(arArgument.RoleConstraint.Id) & "'"
                lsSQLQuery &= " ," & arArgument.SequenceNr
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

        Public Sub DeleteRoleConstraintArgument(ByVal arArgument As FBM.RoleConstraintArgument)

            Try

                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM MetaModelRoleConstraintArgument"
                lsSQLQuery &= " WHERE Id = '" & arArgument.Id & "'"
                lsSQLQuery &= "   AND ModelId = '" & arArgument.Model.ModelId & "'"

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

        Public Sub DeleteRoleConstraintArgumentsByModelRoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM MetaModelRoleConstraintArgument"
                lsSQLQuery &= " WHERE RoleConstraintId = '" & arRoleConstraint.Id & "'"
                lsSQLQuery &= "   AND ModelId = '" & arRoleConstraint.Model.ModelId & "'"

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

        Public Function ExistsRoleConstraintArgument(ByVal arRoleConstraintArgument As FBM.RoleConstraintArgument) As Boolean

            Try
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelRoleConstraintArgument"
                lsSQLQuery &= " WHERE Id = '" & Trim(arRoleConstraintArgument.Id) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(arRoleConstraintArgument.Model.ModelId) & "'"


                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsRoleConstraintArgument = True
                Else
                    ExistsRoleConstraintArgument = False
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

        Public Function GetArgumentsForRoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint) As List(Of FBM.RoleConstraintArgument)

            Try
                Dim larRoleConstraintArgument As New List(Of FBM.RoleConstraintArgument)
                Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument                
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM MetaModelRoleConstraintArgument"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arRoleConstraint.Model.ModelId) & "'"
                lsSQLQuery &= "   AND RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"

                lREcordset.Open(lsSQLQuery)

                While Not lREcordset.EOF
                    lrRoleConstraintArgument = New FBM.RoleConstraintArgument(arRoleConstraint, _
                                                                              lREcordset("SequenceNr").Value, _
                                                                              Trim(lREcordset("Id").Value) _
                                                                              )
                    lrRoleConstraintArgument.isDirty = False
                    lrRoleConstraintArgument.JoinPath = TableJoinPathRole.GetJoinPathForArgument(lrRoleConstraintArgument)

                    larRoleConstraintArgument.Add(lrRoleConstraintArgument)

                    lREcordset.MoveNext()
                End While

                Return larRoleConstraintArgument

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Sub UpdateRoleConstraintArgument(ByVal arArgument As FBM.RoleConstraintArgument)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelRoleConstraintArgument"
                lsSQLQuery &= "   SET SequenceNr = " & arArgument.SequenceNr
                lsSQLQuery &= " WHERE Id = '" & Trim(arArgument.Id) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(arArgument.Model.ModelId) & "'"

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

    End Module

End Namespace
