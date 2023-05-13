Imports System.Reflection

Public Module tableClientServerInvitation

    Public Sub addInvitation(ByRef arInvitation As ClientServer.Invitation)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerInvitation"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arInvitation.InvitationType.ToString & "'"
            lsSQLQuery &= " ,#" & Trim(Replace(arInvitation.DateTime, "'", "`")) & "#"
            lsSQLQuery &= " ,'" & Trim(Replace(arInvitation.InvitingUser.Id, "'", "`")) & "'"
            If arInvitation.InvitedUser Is Nothing Then
                lsSQLQuery &= " ,''"
            Else
                lsSQLQuery &= " ,'" & Trim(Replace(arInvitation.InvitedUser.Id, "'", "`")) & "'"
            End If

            If arInvitation.InvitedGroup Is Nothing Then
                lsSQLQuery &= " ,''"
            Else
                lsSQLQuery &= " ,'" & Trim(Replace(arInvitation.InvitedGroup.Id, "'", "`")) & "'"
            End If
            lsSQLQuery &= " ,'" & Trim(Replace(arInvitation.Tag, "'", "`")) & "'"
            lsSQLQuery &= " ," & arInvitation.Accepted
            lsSQLQuery &= " ," & arInvitation.Closed
            lsSQLQuery &= ")"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

    Public Sub closeAllInvitationsForUser(ByRef arUser As ClientServer.User)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "UPDATE ClientServerInvitation"
            lsSQLQuery &= "  SET Closed = TRUE"
            lsSQLQuery = " WHERE InvitedUserId = '" & Trim(Replace(arUser.Id, "'", "`")) & "'"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

    Public Sub closeInvitation(ByRef arInvitation As ClientServer.Invitation)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "UPDATE ClientServerInvitation"
            lsSQLQuery &= "  SET Accepted = " & arInvitation.Accepted
            lsSQLQuery &= "    , Closed = " & arInvitation.Closed
            lsSQLQuery &= " WHERE InvitationType = '" & arInvitation.InvitationType.ToString & "'"
            lsSQLQuery &= "   AND DateTime = #" & Trim(Replace(arInvitation.DateTime, "'", "`")) & "#"
            lsSQLQuery &= "  AND InvitingUserId = '" & Trim(Replace(arInvitation.InvitingUser.Id, "'", "`")) & "'"
            'lsSQLQuery &= "  AND InvitedUserId = '" & Trim(Replace(arInvitation.InvitedUser.Id, "'", "`")) & "'"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

    Public Function getOpenInvitationsForUserByType(ByRef arUser As ClientServer.User, aiInvitationType As pcenumInvitationType) As List(Of ClientServer.Invitation)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim larInvitation As New List(Of ClientServer.Invitation)

        Try
            'CodeSafe
            If arUser Is Nothing Then
                Return larInvitation
            End If

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerInvitation"
            lsSQLQuery &= " WHERE InvitedUserId = '" & Trim(arUser.Id) & "'"
            lsSQLQuery &= "   AND Closed = FALSE"
            lsSQLQuery &= "   AND InvitationType = '" & aiInvitationType.ToString & "'"

            lREcordset.Open(lsSQLQuery)

            Dim lrInvitation As ClientServer.Invitation

            While Not lREcordset.EOF
                lrInvitation = New ClientServer.Invitation
                lrInvitation.InvitationType = aiInvitationType
                lrInvitation.DateTime = lREcordset("DateTime").Value
                lrInvitation.InvitingUser = New ClientServer.User
                Call tableClientServerUser.getUserDetailsById(lREcordset("InvitingUserId").Value, lrInvitation.InvitingUser)
                lrInvitation.InvitedUser = arUser
                lrInvitation.Accepted = False
                lrInvitation.Closed = False
                lrInvitation.Tag = lREcordset("Tag").Value

                larInvitation.Add(lrInvitation)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larInvitation

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larInvitation
        End Try

    End Function

    ''' <summary>
    ''' Gets the list of open invitations for a Group.
    ''' NB Group invitations are only 'Group to join Project' type invitations.
    ''' </summary>
    ''' <param name="arGroup"></param>
    ''' <param name="aiInvitationType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getOpenInvitationsForGroupByType(ByRef arGroup As ClientServer.Group, aiInvitationType As pcenumInvitationType) As List(Of ClientServer.Invitation)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim larInvitation As New List(Of ClientServer.Invitation)

        Dim laiAllowedInvitationTypes = {pcenumInvitationType.GroupToJoinProject}

        If Not laiAllowedInvitationTypes.Contains(aiInvitationType) Then
            Throw New Exception("Invitation Type not allowed: ".AppendString(aiInvitationType.ToString))
        End If

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerInvitation"
            lsSQLQuery &= " WHERE InvitedGroupId = '" & Trim(arGroup.Id) & "'"
            lsSQLQuery &= "   AND Closed = FALSE"
            lsSQLQuery &= "   AND InvitationType = '" & aiInvitationType.ToString & "'"

            lREcordset.Open(lsSQLQuery)

            Dim lrInvitation As ClientServer.Invitation

            While Not lREcordset.EOF
                lrInvitation = New ClientServer.Invitation
                lrInvitation.InvitationType = aiInvitationType
                lrInvitation.DateTime = lREcordset("DateTime").Value
                lrInvitation.InvitingUser = New ClientServer.User
                Call tableClientServerUser.getUserDetailsById(lREcordset("InvitingUserId").Value, lrInvitation.InvitingUser)
                lrInvitation.InvitedGroup = arGroup
                lrInvitation.Accepted = False
                lrInvitation.Closed = False
                lrInvitation.Tag = lREcordset("Tag").Value

                Dim lrProject As New ClientServer.Project
                lrProject.Id = Trim(lrInvitation.Tag)
                Call tableClientServerProject.getProjectDetailsById(lrProject.Id, lrProject)
                lrInvitation.InvitedToJoinProject = lrProject

                larInvitation.Add(lrInvitation)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larInvitation

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larInvitation
        End Try

    End Function


End Module
