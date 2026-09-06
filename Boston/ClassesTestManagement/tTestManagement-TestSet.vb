Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Newtonsoft.Json

Namespace TestManagement

    ''' <summary>
    ''' Class for sets of TestCases, and where a TestCase has a sequence of TestSteps.
    ''' </summary>
    <Serializable>
    Public Class TestSet
        Inherits TestManagement.Identifiable

        ''' <summary>
        ''' The FBM Model that is being tested.
        ''' </summary>
        <JsonIgnore>
        <XmlIgnore>
        Public Model As FBM.Model

        <JsonIgnore>
        <XmlIgnore>
        Private _ModelId As String

        ''' <summary>
        ''' The FBM ModelId of the Model that is being tested.
        ''' </summary>
        <JsonProperty>
        <XmlAttribute>
        Public Property ModelId As String
            Get
                If Me.Model Is Nothing Then
                    Return Me._ModelId
                Else
                    Return Me.Model.ModelId
                End If
            End Get
            Set(value As String)
                Me._ModelId = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Public TestPlan As TestManagement.TestPlan = Nothing

        <JsonIgnore>
        <XmlIgnore>
        Private _TestPlanIdentifier As String = Nothing

        <JsonProperty>
        <XmlAttribute>
        <ForeignKeyReference(GetType(TestManagement.TestPlan), NameOf(TestManagement.TestPlan.Identifier), True)>
        Public Property TestPlanIdentifier As String
            Get
                If Me.TestPlan Is Nothing Then
                    Return Me._TestPlanIdentifier
                Else
                    Return Me.TestPlan.Identifier
                End If
            End Get
            Set(value As String)
                Me._TestPlanIdentifier = value
            End Set
        End Property

        'Name is in Identifiable

        <JsonIgnore>
        <XmlIgnore>
        Private _TestSetPurpose As String = ""

        <JsonProperty>
        <XmlElement>
        Public Property TestSetPurpose As String
            Get
                Return Me._TestSetPurpose
            End Get
            Set(value As String)
                Me._TestSetPurpose = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _TestCase As New BindingList(Of TestManagement.TestCase)

        <JsonIgnore>
        <XmlArray("TestCases")>
        <XmlArrayItem("TestCase", GetType(TestCase))>
        Public Property TestCase As BindingList(Of TestManagement.TestCase)
            Get
                Return _TestCase
            End Get
            Set(value As BindingList(Of TestManagement.TestCase))
                _TestCase = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub

    End Class

End Namespace