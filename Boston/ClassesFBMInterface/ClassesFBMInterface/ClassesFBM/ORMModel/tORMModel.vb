Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<Serializable()> _
<DataContractFormat()> _
Public Class Model

    <DataMember()> _
    <XmlAttribute()> _
    Public ModelId As String = ""

    <DataMember()> _
    <XmlAttribute()> _
    Public Name As String = ""

    <DataMember()> _
    <XmlAttribute()> _
    Public ProjectId As String = ""

    <DataMember()> _
    <XmlAttribute()> _
    Public [Namespace] As String = ""

    <DataMember()> _
    <XmlElement()> _
    Public ValueType As New List(Of ValueType)

    <DataMember()> _
    <XmlElement()> _
    Public EntityType As New List(Of EntityType)

    <DataMember()> _
    Public FactType As New List(Of FactType)

    <DataMember()> _
    Public RoleConstraint As New List(Of RoleConstraint)

    <DataMember()>
    Public Page As Page

    <DataMember()>
    Public StoreAsXML As Boolean


    ' ''' <summary>
    ' ''' The Relational Data Schema for the Model.
    ' ''' </summary>
    ' ''' <remarks></remarks>
    ' ''' 
    '<DataMember()> _
    '<XmlIgnore()> _
    'Public RDS As New RDS.Model

    ''' <summary>
    ''' Parameterless Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '-------------------
        'Parameterless New
        '-------------------
    End Sub


    ''' <summary>
    ''' Empties the Model
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Empty()

        'Me.RoleConstraint = New List(Of RoleConstraint)
        'Me.FactType = New List(Of FactType)
        'Me.EntityType = New List(Of EntityType)
        'Me.ValueType = New List(Of ValueType)

    End Sub

End Class
