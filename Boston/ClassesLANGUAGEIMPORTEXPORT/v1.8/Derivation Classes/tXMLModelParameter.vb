Imports System.Xml.Serialization
Imports edu.stanford.nlp.trees

Namespace XMLModel
    <Serializable()>
    Public Class Parameter
        Implements IEquatable(Of XMLModel.Parameter)

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
        Private _Name As String
        <XmlAttribute()>
        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _BagInput As Boolean
        <XmlAttribute()>
        Public Property BagInput As Boolean
            Get
                Return Me._BagInput
            End Get
            Set(ByVal value As Boolean)
                Me._BagInput = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Shadows Function Equals(other As Parameter) As Boolean Implements IEquatable(Of Parameter).Equals
            Return Me.Id = other.Id
        End Function

    End Class

End Namespace
