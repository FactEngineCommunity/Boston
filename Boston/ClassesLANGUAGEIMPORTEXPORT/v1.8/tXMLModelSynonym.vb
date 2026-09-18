Imports System.Xml.Serialization

Namespace XMLModel

    <Serializable()>
    Partial Public Class Synonym

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ModelElementId As String
        <XmlAttribute()>
        Public Property ModelElementId() As String
            Get
                Return Me._ModelElementId
            End Get
            Set(ByVal value As String)
                Me._ModelElementId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Synonym As String
        <XmlAttribute()>
        Public Property Synonym As String
            Get
                Return Me._Synonym
            End Get
            Set(ByVal value As String)
                Me._Synonym = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Costructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asModelElementId As String, ByVal asSynonym As String)
            Me.ModelElementId = asModelElementId
            Me.Synonym = asSynonym
        End Sub

    End Class

End Namespace
