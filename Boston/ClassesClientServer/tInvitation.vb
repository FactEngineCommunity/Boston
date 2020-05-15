Namespace ClientServer

    Public Class Invitation

        Public DateTime As DateTime

        Public InvitationType As pcenumInvitationType

        Public InvitingUser As ClientServer.User 'The User who made the Invitation.

        Public InvitedUser As ClientServer.User 'Ther User who is invited to join a Group or Project.

        Public InvitedGroup As ClientServer.Group 'The Group who is invited to join a Project.

        Public InvitedToJoinProject As ClientServer.Project 'The Project to which a User or Group is invited to join.

        ''' <summary>
        ''' The ProjectId or GroupId of the Invitation
        ''' NB If the InvitationType is GroupToJoinProject, the Tag is the Project.Id of the Project to which 
        '''   the InvitedGroup is invited to join.
        ''' </summary>
        ''' <remarks></remarks>
        Public Tag As String 'The ProjectId or GroupId of the Invitation

        Public Accepted As Boolean = False 'True if the invitation was accepted. Saved back to the database.

        Public Closed As Boolean = False 'True when the invitation has been responded to. Saved back to the database.

    End Class

End Namespace
