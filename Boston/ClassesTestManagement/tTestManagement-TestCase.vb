Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Newtonsoft.Json
Imports System.Linq.Expressions
Imports System.Reflection

Namespace TestManagement

    ''' <summary>
    ''' Class for individual test cases
    ''' </summary>
    <Serializable>
    Public Class TestCase
        Inherits Identifiable
        Implements IEquatable(Of TestManagement.TestCase)

        <JsonIgnore>
        <XmlIgnore>
        <Browsable(False)>
        Public Property TestSet As TestSet

        ''' <summary>
        ''' E.g. 'TS1-EC-001'
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty>
        <XmlAttribute>
        Public Property Code As String 'E.g. 'TS1-EC-01' for the first Test Case in a Test Set with name, 'Test Set 1 - Error Conditions'.

        <JsonIgnore>
        <XmlIgnore>
        Private _TestSetIdentifier As String = Nothing

        <JsonProperty>
        <XmlAttribute>
        <ForeignKeyReference(GetType(TestManagement.TestSet), NameOf(TestManagement.TestSet.Identifier), True)>
        <Browsable(False)>
        Public Property TestSetIdentifier As String
            Get
                If Me.TestSet Is Nothing Then
                    Return Me._TestSetIdentifier
                Else
                    Return Me.TestSet.Identifier
                End If
            End Get
            Set(value As String)
                Me._TestSetIdentifier = value
            End Set
        End Property

        <JsonProperty>
        <XmlElement>
        Public Property TestCasePurpose As String

        ''' <summary>
        ''' The FBM ModelElement being tested.
        ''' </summary>
        <JsonIgnore>
        <XmlIgnore>
        Public ModelElement As FBM.ModelObject

        <JsonIgnore>
        <XmlIgnore>
        Private _ModelElementId As String

        ''' <summary>
        ''' The ModelElement.Id of the ModelElement being tested.
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty>
        <XmlAttribute>
        <Browsable(False)>
        Public Property ModelElementId As String
            Get
                If Me.ModelElement Is Nothing Then
                    Return Me._ModelElementId
                Else
                    Return Me.ModelElement.Id

                End If
            End Get
            Set(value As String)
                Me._ModelElementId = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _ExpectedDurationDays As Integer

        <JsonProperty>
        <XmlAttribute>
        Public Property ExpectedDurationDays As Integer
            Get
                Return _ExpectedDurationDays
            End Get
            Set(value As Integer)
                _ExpectedDurationDays = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _ExpectedDurationHours As Integer

        <JsonProperty>
        <XmlAttribute>
        Public Property ExpectedDurationHours As Integer
            Get
                Return _ExpectedDurationHours
            End Get
            Set(value As Integer)
                _ExpectedDurationHours = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _ExpectedDurationMinutes As Integer

        <JsonProperty>
        <XmlAttribute>
        Public Property ExpectedDurationMinutes As Integer
            Get
                Return _ExpectedDurationMinutes
            End Get
            Set(value As Integer)
                _ExpectedDurationMinutes = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _TestType As String

        <JsonProperty>
        <XmlAttribute>
        Public Property TestType As String
            Get
                Return _TestType
            End Get
            Set(value As String)
                _TestType = value
            End Set
        End Property


        <JsonIgnore>
        <XmlIgnore>
        Private _TestPhase As String

        <JsonProperty>
        <XmlAttribute>
        Public Property TestPhase As String
            Get
                Return _TestPhase
            End Get
            Set(value As String)
                _TestPhase = value
            End Set
        End Property

        <JsonProperty>
        <XmlAttribute>
        Public Property Priority As String

        <JsonIgnore>
        <XmlIgnore>
        Private _RecommendedConfiguration As New List(Of TestManagement.ConfigurationItem)

        <JsonProperty>
        <XmlIgnore>
        <Browsable(False)>
        Public Property RecommendedConfiguration As List(Of TestManagement.ConfigurationItem)
            Get
                Return _RecommendedConfiguration
            End Get
            Set(value As List(Of TestManagement.ConfigurationItem))
                _RecommendedConfiguration = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _RequiredResource As New List(Of TestManagement.Resource)

        <JsonProperty>
        <XmlIgnore>
        <Browsable(False)>
        Public Property RequiredResource As List(Of TestManagement.Resource)
            Get
                Return _RequiredResource
            End Get
            Set(value As List(Of TestManagement.Resource))
                _RequiredResource = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _BusinessRequirementDocument As New List(Of DataLineage.Document)

        <JsonProperty>
        <XmlElement>
        <Browsable(False)>
        Public Property BusinessRequirementDocument As List(Of DataLineage.Document)
            Get
                Return _BusinessRequirementDocument
            End Get
            Set(value As List(Of DataLineage.Document))
                _BusinessRequirementDocument = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _TestStep As New BindingList(Of TestManagement.TestStep)

        <JsonProperty>
        <XmlArray("TestSteps")>
        <XmlArrayItem("TestStep", GetType(TestStep))>
        <Browsable(False)>
        Public Property TestStep As BindingList(Of TestManagement.TestStep)
            Get
                Return _TestStep
            End Get
            Set(value As BindingList(Of TestManagement.TestStep))
                _TestStep = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        <Browsable(False)>
        Public Property WrittenByUser As ClientServer.User

        <JsonIgnore>
        <XmlIgnore>
        Public ReadOnly Property WrittenBy As String
            Get
                'LazyLoad
                If _WrittenByUserId IsNot Nothing Then
                    If Me.WrittenByUser Is Nothing Then
                        Dim lrUser As New ClientServer.User
                        Me.WrittenByUser = tableClientServerUser.getUserDetailsById(Me._WrittenByUserId, lrUser, False)
                        Return $"{Me.WrittenByUser.FirstName} {Me.WrittenByUser.LastName}"
                    Else
                        Return $"{Me.WrittenByUser.FirstName} {Me.WrittenByUser.LastName}"
                    End If
                Else
                    Return ""
                End If
            End Get
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _WrittenByUserId As String = Nothing

        <JsonProperty>
        <XmlIgnore>
        <Browsable(False)>
        Public Property WrittenByUserId As String
            Get
                If Me.WrittenByUser Is Nothing Then
                    Return Me._WrittenByUserId
                Else
                    Return Me.WrittenByUser.Id
                End If
            End Get
            Set(value As String)
                Me._WrittenByUserId = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Public UpdatedByUser As ClientServer.User = Nothing

        <JsonIgnore>
        <XmlIgnore>
        Private _UpdatedByUserId As String = Nothing

        <JsonProperty>
        <XmlIgnore>
        <Browsable(False)>
        Public Property UpdatedByUserId As String
            Get
                If Me.UpdatedByUser Is Nothing Then
                    Return Me._UpdatedByUserId
                Else
                    Return Me.UpdatedByUser.Id
                End If
            End Get
            Set(value As String)
                Me._UpdatedByUserId = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Public ReadOnly Property UpdatedBy As String
            Get
                'LazyLoad
                If _UpdatedByUserId IsNot Nothing Then
                    If Me.UpdatedByUser Is Nothing Then
                        Dim lrUser As New ClientServer.User
                        Me.UpdatedByUser = tableClientServerUser.getUserDetailsById(Me._UpdatedByUserId, lrUser, False)
                        Return $"{Me.UpdatedByUser.FirstName} {Me.UpdatedByUser.LastName}"
                    Else
                        Return $"{Me.UpdatedByUser.FirstName} {Me.UpdatedByUser.LastName}"
                    End If
                Else
                    Return ""
                End If
            End Get
        End Property

        <JsonIgnore>
        <XmlIgnore>
        <Browsable(False)>
        Public Property TestRun As New List(Of TestManagement.TestRun)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByRef arTestSet As TestSet,
                       Optional asTestType As String = "",
                       Optional asTestPhase As String = "",
                       Optional asPriority As String = "Medium")

            Me.TestSet = arTestSet
            Me.TestType = asTestType
            Me.TestPhase = asTestPhase
            Me.Priority = asPriority

            Call Me.CreateTestSetCode() 'Creates a Test Case Code from the Test Set

        End Sub

        Public Shadows Function Equals(other As TestCase) As Boolean Implements IEquatable(Of TestCase).Equals
            Return Me.Identifier = other.Identifier
        End Function

        Public Sub CreateTestSetCode()

            Try
                Dim lrDataStore As New DataStore.Store

                If Me.TestSetIdentifier Is Nothing Then Throw New Exception("Test Case requires a Test Set Identifier")

                If Me.TestSet Is Nothing Then

                    Dim whereClauseDataSet As Expression(Of Func(Of TestManagement.TestSet, Boolean)) = Function(t) t.Identifier = Me.TestSetIdentifier
                    Dim larTestSet = lrDataStore.Get(Of TestManagement.TestSet)(whereClauseDataSet)

                    Me.TestSet = larTestSet.First
                End If

                Dim whereClauseTestCases As Expression(Of Func(Of TestCase, Boolean)) = Function(t) t.TestSetIdentifier = Me.TestSetIdentifier
                Dim larTestCase = lrDataStore.Get(Of TestCase)(whereClauseTestCases)

                Me.Code = $"{Me.TestSet.Name.ToPascalCaseWithSpaces.ToInitialCode}-{larTestCase.Count.ToPaddedCode}"

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
            End Try
        End Sub


    End Class

End Namespace
