Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()>
<Serializable()>
Public Class Broadcast

    <DataMember()>
    Public Model As Model

    <DataMember()>
    Public UserManagement As UserManagement

    <DataMember()>
    Public Invitation As Invitation

    <DataMember()>
    Public FEKLStatement As FEKLStatement

    <DataMember()>
    Public ErrorCode As pcenumErrorType

    <DataMember()>
    Public ErrorMessage As String

End Class

<DataContractFormat()>
<Serializable()>
Public Class FEKLStatement

    <DataMember()>
    Public ModelId As String

    <DataMember()>
    Public Statement As String
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
