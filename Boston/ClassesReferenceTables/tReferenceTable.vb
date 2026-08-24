Imports System.Xml.Serialization

<Serializable()>
Public Class ReferenceTable

    <XmlAttribute>
    Public ReferenceTableId As Integer

    <XmlAttribute>
    Public Name As String

    Public ReferenceTuples As New List(Of ReferenceTuple)

    <NonSerialized>
    <XmlIgnore>
    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _Column As New List(Of tReferenceField)
    <XmlIgnore>
    Public ReadOnly Property Column As List(Of tReferenceField)
        Get
            If Me._Column.Count = 0 Then
                Call tableReferenceField.GetReferenceFieldListByReferenceTableId(Me.ReferenceTableId)
            End If
            Return Me._Column
        End Get
    End Property

    ''' <summary>
    ''' Parameterless Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    Public Sub New(ByVal aiTableId As Integer, ByVal asTableName As String)
        Me.ReferenceTableId = aiTableId
        Me.Name = asTableName
    End Sub

End Class

<Serializable>
Public Class ReferenceTuple

    <XmlIgnore>
    Private _RowId As String
    <XmlAttribute>
    Public Property RowId As String
        Get
            Return Me._RowId
        End Get
        Set(value As String)
            Me._RowId = value
        End Set
    End Property

    Public KeyValuePairs As New List(Of KeyValuePair)

    ''' <summary>
    ''' Parameterless Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    Public Sub New(ByVal asRowId As String)
        Me.RowId = asRowId
    End Sub

End Class

<Serializable>
Public Class KeyValuePair

    <XmlIgnore>
    Private _Key As String
    <XmlAttribute>
    Public Property Key As String
        Get
            Return Me._Key
        End Get
        Set(value As String)
            Me._Key = value
        End Set
    End Property

    <XmlIgnore>
    Private _Value As String
    <XmlAttribute>
    Public Property Value As String
        Get
            Return Me._Value
        End Get
        Set(value As String)
            Me._Value = value
        End Set
    End Property

    ''' <summary>
    ''' Parameterless Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    Public Sub New(ByVal asKey As String, ByVal asValue As String)
        Me.Key = asKey
        Me.Value = asValue
    End Sub

End Class

