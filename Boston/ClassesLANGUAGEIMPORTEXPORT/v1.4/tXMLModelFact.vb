Imports System.Xml.Serialization

Namespace XMLModel14
    <Serializable()> _
    Public Class Fact

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _Id As String
        <XmlAttribute()> _
        Public Property Id() As String
            Get
                Return Me._Id
            End Get
            Set(ByVal value As String)
                Me._Id = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Data As New List(Of XMLModel14.FactData)
        Public Property Data() As List(Of XMLModel14.FactData)
            Get
                Return Me._Data
            End Get
            Set(ByVal value As List(Of XMLModel14.FactData))
                Me._Data = value
            End Set
        End Property

    End Class

End Namespace

