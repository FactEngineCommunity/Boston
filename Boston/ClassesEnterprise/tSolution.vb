Imports Boston.DataStore

Namespace Enterprise

    Public Class Solution
        Inherits DataStore.DataStorePrototype

        <PrimaryKeyField>
        Public Property SolutionId As String = System.Guid.NewGuid.ToString

        <ForeignKeyReference(GetType(Enterprise), NameOf(Enterprise.EnterpriseId), True)>
        Public Property EnterpriseId As String

        Public SolutionName As String = ""

    End Class

End Namespace
