Namespace TestManagement

    Public Class TestCaseResource

        <JsonProperty>
        <ForeignKeyReference(GetType(TestManagement.TestCase), NameOf(TestManagement.TestCase.Identifier), True)>
        Public Property TestCaseIdentifier As String

        <JsonProperty>
        <ForeignKeyReference(GetType(TestManagement.Resource), NameOf(TestManagement.Resource.Identifier), True)>
        Public Property ResourceIdentifier As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

    End Class

End Namespace
