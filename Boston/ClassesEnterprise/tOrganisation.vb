Imports Boston.DataStore

Namespace Enterprise

    Public Class Organisation
        Inherits DataStore.DataStorePrototype

        <PrimaryKeyField>
        Public Property OrganisationId As String = System.Guid.NewGuid.ToString

        <ForeignKeyReference(GetType(Enterprise), NameOf(Enterprise.EnterpriseId), True)>
        Public Property EnterpriseId As String

        Public OrganisationName As String = ""

        Public OrganisationCode As String = ""

    End Class

End Namespace
