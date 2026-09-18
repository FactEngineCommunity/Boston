Imports System.Xml.Serialization

Namespace XMLModel17

    <Serializable()>
    Public Class JoinPath

        ''' <summary>
        ''' The set of Roles traversed in order to form the JoinPath.
        ''' </summary>
        ''' <remarks></remarks>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RolePath As New List(Of XMLModel.RoleReference)
        Public Property RolePath As List(Of XMLModel.RoleReference)
            Get
                Return Me._RolePath
            End Get
            Set(value As List(Of XMLModel.RoleReference))
                Me._RolePath = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _JoinPathError As pcenumJoinPathError = pcenumJoinPathError.None
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

End Namespace
