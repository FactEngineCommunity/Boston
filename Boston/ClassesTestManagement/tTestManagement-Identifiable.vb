Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Newtonsoft.Json

Namespace TestManagement

    ' Base class for common identifiable properties
    <Serializable>
    Public Class Identifiable
        Implements INotifyPropertyChanged
        Implements IEquatable(Of TestManagement.Identifiable)

        <JsonIgnore>
        <XmlIgnore>
        Private _Identifier As String = System.Guid.NewGuid.ToString

        <JsonProperty>
        <XmlAttribute>
        <Browsable(False)>
        Public Property Identifier As String
            Get
                Return Me._Identifier
            End Get
            Set(value As String)
                Me._Identifier = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _Name As String = ""

        <JsonProperty>
        <XmlAttribute>
        Public Overridable Property Name As String
            Get
                Return Me._Name
            End Get
            Set(value As String)
                Me._Name = value
                Call OnPropertyChanged(NameOf(Name))
            End Set
        End Property

        <JsonProperty>
        <XmlElement>
        Public Overridable Property Description As String


        <JsonProperty>
        <XmlAttribute>
        Public Overridable Property DateLastUpdated As DateTime

        <NonSerialized>
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(propName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propName))
        End Sub

        ''' <summary>
        ''' Parameterless constructor for serialization
        ''' </summary>
        Public Sub New()
        End Sub

        Public Shadows Function Equals(other As Identifiable) As Boolean Implements IEquatable(Of Identifiable).Equals
            Return Me.Identifier = other.Identifier
        End Function

    End Class

End Namespace