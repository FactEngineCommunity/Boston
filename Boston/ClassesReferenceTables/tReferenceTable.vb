Imports System.Xml.Serialization

<Serializable()>
Public Class ReferenceTable

    <XmlAttribute>
    Public ReferenceTableId As Integer

    <XmlAttribute>
    Public Name As String

    Public ReferenceTuple As New List(Of ReferenceTuple)

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _Column As New List(Of tReferenceField)
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

    <XmlAttribute>
    Public RowId As String

    Public KeyValuePair As New List(Of KeyValuePair)

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

    <XmlAttribute>
    Public Key As String

    <XmlAttribute>
    Public Value As String

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

