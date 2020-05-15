Imports System.Xml.Serialization

Namespace XMLModelv081
    <Serializable()> _
    Public Class FactTypeReading

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

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _PredicateParts As New List(Of XMLModelv081.PredicatePart)
        Public Property PredicateParts() As List(Of XMLModelv081.PredicatePart)
            Get
                Return Me._PredicateParts
            End Get
            Set(ByVal value As List(Of XMLModelv081.PredicatePart))
                Me._PredicateParts = value
            End Set
        End Property

    End Class

End Namespace
