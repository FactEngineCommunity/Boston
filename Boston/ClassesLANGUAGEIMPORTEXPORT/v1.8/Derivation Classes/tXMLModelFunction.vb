Imports System.Xml.Serialization
Imports edu.stanford.nlp.trees

Namespace XMLModel
    <Serializable()>
    Public Class [Function]
        Implements IEquatable(Of XMLModel.Function)

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
        Private _OperatorSymbol As String = ""
        <XmlAttribute()>
        Public Property OperatorSymbol() As String
            Get
                Return Me._OperatorSymbol
            End Get
            Set(ByVal value As String)
                Me._OperatorSymbol = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsBoolean As Boolean
        <XmlAttribute()>
        Public Property IsBoolean As Boolean
            Get
                Return Me._IsBoolean
            End Get
            Set(ByVal value As Boolean)
                Me._IsBoolean = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Parameters As New List(Of XMLModel.Parameter)
        <XmlArray("Parameters")>
        <XmlArrayItem("Parameter")>
        Public Property Parameters As List(Of XMLModel.Parameter)
            Get
                Return Me._Parameters
            End Get
            Set(value As List(Of XMLModel.Parameter))
                Me._Parameters = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Shadows Function Equals(other As [Function]) As Boolean Implements IEquatable(Of [Function]).Equals
            Return Me.Id = other.Id
        End Function

    End Class

End Namespace
