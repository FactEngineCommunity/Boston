Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class Fact

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Id As String
        <XmlAttribute()>
        Public Property Id() As String
            Get
                Return Me._Id
            End Get
            Set(ByVal value As String)
                Me._Id = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Data As New List(Of XMLModel16.FactData)
        Public Property Data() As List(Of XMLModel16.FactData)
            Get
                Return Me._Data
            End Get
            Set(ByVal value As List(Of XMLModel16.FactData))
                Me._Data = value
            End Set
        End Property

    End Class

End Namespace

