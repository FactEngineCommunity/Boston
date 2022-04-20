Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class Broadcast

    <DataMember()> _
    Public Model As Model

    <DataMember()> _
    Public UserManagement As UserManagement

    <DataMember()>
    Public Invitation As Invitation

    <DataMember()>
    Public FEKLStatement As String

    <DataMember()>
    Public ErrorCode As Integer

    <DataMember()>
    Public ErrorText As String

End Class

<DataContractFormat()> _
<Serializable()> _
Public Class UserManagement

    <DataMember()> _
    Public ProjectNamespace As ProjectNamespace

    <DataMember()> _
    Public GroupUser As GroupUser

    <DataMember()> _
    Public ProjectGroup As ProjectGroup

    <DataMember()> _
    Public ProjectGroupPermission As ProjectGroupPermission

    <DataMember()> _
    Public ProjectUser As ProjectUser

    <DataMember()> _
    Public ProjectUserPermission As ProjectUserPermission

    <DataMember()> _
    Public ProjectUserRole As ProjectUserRole

    <DataMember()> _
    Public RoleFunction As RoleFunction

End Class
