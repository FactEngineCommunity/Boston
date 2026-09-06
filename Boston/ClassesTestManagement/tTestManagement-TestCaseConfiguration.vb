Namespace TestManagement

    Public Class TestCaseConfigurationItem


        <JsonProperty>
        <ForeignKeyReference(GetType(TestManagement.TestCase), NameOf(TestManagement.TestCase.Identifier), True)>
        Public Property TestCaseIdentifier As String

        <JsonProperty>
        <ForeignKeyReference(GetType(TestManagement.ConfigurationItem), NameOf(TestManagement.ConfigurationItem.Identifier), True)>
        Public Property ConfigurationItemIdentifier As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

    End Class

End Namespace
