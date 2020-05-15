Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class Invitation

    <DataMember()> _
    Public DateTime As DateTime

    <DataMember()> _
    Public InvitationType As pcenumInvitationType 'GroupToJoinProject | UserToJoinGroup | UserToJoinProject

    <DataMember()> _
    Public InvitingUserId As String 'The User who made the Invitation.

    <DataMember()> _
    Public InvitedUserId As String 'Ther User who is invited to join a Group or Project.

    <DataMember()> _
    Public InvitedGroupId As String 'The Group that is invited to join a Project.

    <DataMember()> _
    Public InvitedToJoinGroupId As String 'The Project to which a User or Group is invited to join.

    <DataMember()> _
    Public InvitedToJoinProjectId As String 'The Project to which a User or Group is invited to join.

End Class
