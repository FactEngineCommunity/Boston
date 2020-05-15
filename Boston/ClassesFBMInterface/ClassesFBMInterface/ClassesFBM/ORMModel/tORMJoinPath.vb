Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class JoinPath

    ''' <summary>
    ''' The set of Roles traversed in order to form the JoinPath.
    ''' </summary>
    ''' <remarks></remarks>
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _RolePath As New List(Of RoleReference)
    <DataMember()> _
    <XmlElement()> _
    Public Property RolePath As List(Of RoleReference)
        Get
            Return Me._RolePath
        End Get
        Set(value As List(Of RoleReference))
            Me._RolePath = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _JoinPathError As pcenumJoinPathError = pcenumJoinPathError.None
    <DataMember()> _
    <XmlAttribute()>
    Public Property JoinPathError As pcenumJoinPathError
        Get
            Return Me._JoinPathError
        End Get
        Set(value As pcenumJoinPathError)
            Me._JoinPathError = value
        End Set
    End Property

End Class
