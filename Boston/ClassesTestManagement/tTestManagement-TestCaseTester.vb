Namespace TestManagement

    Public Class TestCaseTester


        <JsonProperty>
        <ForeignKeyReference(GetType(TestManagement.TestCase), NameOf(TestManagement.TestCase.Identifier), True)>
        Public Property TestCaseIdentifier As String

        <JsonProperty>
        Public Property UserId As String 'ClientServer.User

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

    End Class

End Namespace
