Imports System.Reflection

Public Module tableClientServerNotifications

    Public Sub closeNotification(ByRef arNotification As ClientServer.NotificationGeneral)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "UPDATE ClientServerNotification"
            lsSQLQuery &= "  SET Closed = TRUE"
            lsSQLQuery &= " WHERE DateTime = #" & arNotification.DateTime & "#"
            lsSQLQuery &= "   AND TargetUserId = '" & Trim(Replace(arNotification.TargetUser.Id, "'", "`")) & "'"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
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

    Public Function getOpenNotificationsByUserByType(ByRef arUser As ClientServer.User, _
                                                     ByRef aiNotificationType As pcenumNotificationType) _
                                                 As List(Of ClientServer.NotificationGeneral)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim larNotification As New List(Of ClientServer.NotificationGeneral)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerNotification"
            lsSQLQuery &= " WHERE TargetUserId = '" & Trim(arUser.Id) & "'"
            lsSQLQuery &= "   AND Closed = FALSE"
            lsSQLQuery &= "   AND NotificationType = '" & aiNotificationType.ToString & "'"

            lREcordset.Open(lsSQLQuery)

            Dim lrNotification As ClientServer.NotificationGeneral

            While Not lREcordset.EOF
                lrNotification = New ClientServer.NotificationGeneral
                lrNotification.DateTime = lREcordset("DateTime").Value
                lrNotification.TargetUser = arUser
                lrNotification.NotificationType = aiNotificationType
                lrNotification.Text = Trim(lREcordset("NotificationText").Value)
                lrNotification.Gotit = False

                larNotification.Add(lrNotification)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larNotification

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return larNotification
        End Try

    End Function


End Module
