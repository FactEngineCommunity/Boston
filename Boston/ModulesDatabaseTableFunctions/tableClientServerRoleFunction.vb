Imports System.Reflection

Public Module tableClientServerRoleFunction

    Public Sub AddFunctionToRole(ByRef aiFunction As pcenumFunction, ByRef arRole As ClientServer.Role)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " INSERT INTO ClientServerRoleFunction"
            lsSQLQuery &= "  VALUES ("
            lsSQLQuery &= "  '" & arRole.Id & "'"
            lsSQLQuery &= "  ,'" & aiFunction.ToString & "'"
            lsSQLQuery &= " )"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function getFunctionsForRole(ByRef arRole As ClientServer.Role) As List(Of ClientServer.Function)

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim lrFunction As ClientServer.Function
        Dim larFunction As New List(Of ClientServer.Function)

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerFunction"
            lsSQLQuery &= " WHERE FunctionName IN (SELECT FunctionName FROM ClientServerRoleFunction WHERE RoleId = '" & arRole.Id & "')"


            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrFunction = New ClientServer.Function
                    lrFunction.Name = lREcordset("FunctionName").Value
                    lrFunction.Function =
                        CType([Enum].Parse(GetType(pcenumFunction), lREcordset("FunctionName").Value), pcenumFunction)
                    lrFunction.FullText = lREcordset("FunctionFullText").Value

                    arRole.Function.Add(lrFunction.Function)
                    larFunction.Add(lrFunction)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

            Return larFunction

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larFunction
        End Try

    End Function

    Public Sub removeFunctionFromRole(ByRef aiFunction As pcenumFunction, ByRef arRole As ClientServer.Role)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM ClientServerRoleFunction"
            lsSQLQuery &= "  WHERE RoleId = '" & arRole.Id & "'"
            lsSQLQuery &= "    AND FunctionName = '" & aiFunction.ToString & "'"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Module
