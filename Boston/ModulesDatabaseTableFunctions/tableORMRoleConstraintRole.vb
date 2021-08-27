Imports System.Reflection

Namespace TableRoleConstraintRole
    Public Module TableRoleConstraintRole

        Sub AddRoleConstraintRole(ByVal arRoleConstraintRole As FBM.RoleConstraintRole)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelRoleConstraintRole"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraintRole.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraintRole.RoleConstraint.Id) & "'"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraintRole.Role.Id) & "'"
                lsSQLQuery &= " ," & arRoleConstraintRole.SequenceNr
                lsSQLQuery &= " ," & arRoleConstraintRole.IsEntry
                lsSQLQuery &= " ," & arRoleConstraintRole.IsExit
                If arRoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                    lsSQLQuery &= " ,'" & arRoleConstraintRole.RoleConstraintArgument.Id & "'"
                Else
                    lsSQLQuery &= " ,NULL"
                End If
                lsSQLQuery &= " ," & NullVal(arRoleConstraintRole.ArgumentSequenceNr, 0)
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableRoleConstraintRole.AddRoleConstraintRole"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & "RoleConstraintId: " & arRoleConstraintRole.RoleConstraint.Id
                lsMessage &= vbCrLf & "RoleId: " & arRoleConstraintRole.Role.Id
                lsMessage &= vbCrLf & "Role.FactType.Id: " & arRoleConstraintRole.Role.FactType.Id
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteRoleConstraintRole(ByVal arRoleConstraintRole As FBM.RoleConstraintRole)

            '-----------------------------------------------
            'deletes the specified RoleConstraint from the
            'RoleConstraint table
            '-----------------------------------------------

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "DELETE FROM MetaModelRoleConstraintRole"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arRoleConstraintRole.Model.ModelId) & "'"
                lsSQLQuery &= "   AND RoleConstraintId = '" & Trim(arRoleConstraintRole.RoleConstraint.Id) & "'"
                lsSQLQuery &= "   AND RoleId = '" & arRoleConstraintRole.Role.Id & "'"

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

        Public Sub DeleteRoleConstraintRolesByRoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

            '-----------------------------------------------
            'deletes the specified RoleConstraint from the
            'RoleConstraint table
            '-----------------------------------------------

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "DELETE FROM MetaModelRoleConstraintRole"
                lsSQLQuery = lsSQLQuery & " WHERE ModelId = '" & Trim(arRoleConstraint.Model.ModelId) & "'"
                lsSQLQuery = lsSQLQuery & "   AND RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"

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

        Public Function ExistsRoleConstraintRole(ByVal arRoleConstraintRole As FBM.RoleConstraintRole) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                '------------------------
                'Initialise return value
                '------------------------
                ExistsRoleConstraintRole = False

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelRoleConstraintRole"
                lsSQLQuery &= " WHERE RoleConstraintId = '" & Trim(arRoleConstraintRole.RoleConstraint.Id) & "'"
                lsSQLQuery &= "   AND ModelId = '" & arRoleConstraintRole.Model.ModelId & "'"
                lsSQLQuery &= "   AND RoleId = '" & arRoleConstraintRole.Role.Id & "'"


                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsRoleConstraintRole = True
                Else
                    ExistsRoleConstraintRole = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Function getRoleConstraintRoles_by_RoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint) As List(Of FBM.RoleConstraintRole)

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Dim lsId As String

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            getRoleConstraintRoles_by_RoleConstraint = New List(Of FBM.RoleConstraintRole)

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT rcr.*"
                lsSQLQuery &= "  FROM MetaModelRoleConstraintRole rcr"
                lsSQLQuery &= " WHERE rcr.RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"
                lsSQLQuery &= "   AND rcr.ModelId = '" & Trim(arRoleConstraint.Model.ModelId) & "'"
                lsSQLQuery &= " ORDER BY SequenceNr"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrRoleConstraintRole = New FBM.RoleConstraintRole
                        lrRoleConstraintRole.isDirty = False
                        lrRoleConstraintRole.Model = arRoleConstraint.Model
                        lrRoleConstraintRole.RoleConstraint = arRoleConstraint
                        lrRoleConstraintRole.SequenceNr = lREcordset("SequenceNr").Value
                        lrRoleConstraintRole.IsEntry = lREcordset("IsEntry").Value
                        lrRoleConstraintRole.IsExit = lREcordset("IsExit").Value
                        lrRoleConstraintRole.isDirty = False

                        If NullVal(lREcordset("ArgumentId").Value, Nothing) Is Nothing Then
                            lrRoleConstraintRole.RoleConstraintArgument = Nothing
                        Else
                            lrRoleConstraintRole.RoleConstraintArgument = arRoleConstraint.Argument.Find(Function(x) x.Id = Trim(lREcordset("ArgumentId").Value))
                            lrRoleConstraintRole.RoleConstraintArgument.RoleConstraintRole.Add(lrRoleConstraintRole)
                        End If
                        lrRoleConstraintRole.ArgumentSequenceNr = NullVal(lREcordset("ArgumentSequenceNr").Value, 0)

                        lsId = Trim(lREcordset("RoleId").Value)
                        lrRoleConstraintRole.Role = arRoleConstraint.Model.Role.Find(Function(x) x.Id = lsId)

                        'CodeSafe: Role must exist
                        If lrRoleConstraintRole.Role IsNot Nothing Then
                            getRoleConstraintRoles_by_RoleConstraint.Add(lrRoleConstraintRole)

                            '-----------------------------------------------------------------
                            'Add the RoleConstraintRole to the (byRef) RoleConstraint as well
                            '-----------------------------------------------------------------
                            arRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                        End If

                        lREcordset.MoveNext()
                    End While
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

        Sub UdateRoleConstraintRole(ByVal arRoleConstraintRole As FBM.RoleConstraintRole)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelRoleConstraintRole"
                lsSQLQuery &= "   SET RoleId = '" & Trim(arRoleConstraintRole.Role.Id) & "'"
                lsSQLQuery &= "       ,IsEntry = " & arRoleConstraintRole.IsEntry
                lsSQLQuery &= "       ,IsExit = " & arRoleConstraintRole.IsExit
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arRoleConstraintRole.Model.ModelId) & "'"
                lsSQLQuery &= "   AND RoleConstraintId = '" & Trim(arRoleConstraintRole.RoleConstraint.Id) & "'"
                lsSQLQuery &= "   AND RoleId = '" & Trim(arRoleConstraintRole.Role.Id) & "'"
                lsSQLQuery &= "   AND SequenceNr = " & arRoleConstraintRole.SequenceNr

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

    End Module

End Namespace