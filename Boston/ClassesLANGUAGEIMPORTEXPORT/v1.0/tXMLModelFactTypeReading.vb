Imports System.Xml.Serialization

Namespace XMLModel1
    <Serializable()>
    Public Class FactTypeReading

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
        Private _PredicateParts As New List(Of XMLModel1.PredicatePart)
        Public Property PredicateParts() As List(Of XMLModel1.PredicatePart)
            Get
                Return Me._PredicateParts
            End Get
            Set(ByVal value As List(Of XMLModel1.PredicatePart))
                Me._PredicateParts = value
            End Set
        End Property

    End Class

End Namespace
