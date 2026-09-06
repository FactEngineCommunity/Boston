Imports Boston.DataStore

Namespace Enterprise

    Public Class Ticket
        Inherits DataStore.DataStorePrototype

        <PrimaryKeyField>
        Public Property TicketId As String = System.Guid.NewGuid.ToString

        <ForeignKeyReference(GetType(Enterprise), NameOf(Enterprise.EnterpriseId), True)>
        Public Property EnterpriseId As String

        Public Property TicketNumber As String = ""

        Public Property DateTimeRaised As DateTime?

        Public Property Subject As String = ""

        Public Property Message As String = ""

        <ForeignKeyReference(GetType(Organisation), NameOf(Organisation.OrganisationId), True)>
        Public Property OrganisationId As String = Nothing

        <ForeignKeyReference(GetType(Solution), NameOf(Solution.SolutionId), True)>
        Public Property SolutionId As String = Nothing

        Public Priority As String = "Medium"

        Public Property Status As String = "Open" 'Open, Closed

        Public Property RaisedByUserId As String

        Public Property AssignedToUserId As String

    End Class

End Namespace
