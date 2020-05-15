Public Module publicVariables

    Public prApplication As tRichmondApplication
    Public pdbConnection As New ADODB.Connection
    Public pdbDatabaseUpgradeConnection As New ADODB.Connection
    Public pdb_OLEDB_connection As New OleDb.OleDbConnection
    Public prPageNodes As New List(Of tEnterpriseEnterpriseView)  'Stores a list of the Pages in the EnterpriseView form. Used for Morphing.

    Public psStartupFBMFile As String = ""

    Public pbCancelClosing As Boolean = False

    Public pbLogStartup As Boolean = False

    Public prLogger As New tErrorLogger

    Public prUser As ClientServer.User 'Used when loging in using VirtualUI

    Public prSoftwareCategory As pcenumSoftwareCategory
    Public prApplicationApplicationVersionNr As String
    Public prApplicationDatabaseVersionNr As String

    Public liThinfinity As New Cybele.Thinfinity.VirtualUI

End Module