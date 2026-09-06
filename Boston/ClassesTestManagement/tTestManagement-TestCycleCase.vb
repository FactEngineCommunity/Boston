Imports Newtonsoft.Json

Namespace TestManagement

    ''' <summary>
    ''' Class for assigning a Tester (ClientServer.User) to a Test Case on a Test Cycle.
    ''' </summary>
    Public Class TestCycleCase
        Inherits Identifiable

        <JsonIgnore>
        Private _TestCycleIdentifier As String

        <JsonProperty>
        <ForeignKeyReference(GetType(TestManagement.TestCycle), NameOf(TestManagement.TestCycle.Identifier), True)>
        Public Property TestCycleIdentifier As String
            Get
                Return Me._TestCycleIdentifier
            End Get
            Set(value As String)
                Me._TestCycleIdentifier = value
            End Set
        End Property


        <JsonIgnore>
        Private _TestCaseIdentifier As String

        <JsonProperty>
        <ForeignKeyReference(GetType(TestManagement.TestCase), NameOf(TestManagement.TestCase.Identifier), True)>
        Public Property TestCaseIdentifier As String
            Get
                Return Me._TestCaseIdentifier
            End Get
            Set(value As String)
                Me._TestCaseIdentifier = value
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