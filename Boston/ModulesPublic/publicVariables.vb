Imports Mindscape.Raygun4Net

Public Module publicVariables

    Public prApplication As tApplication
    'Public pdbConnection As New ADODB.Connection 'New FactEngine.SQLiteConnection(Nothing, "", 1000, False)
    'Public pdb_OLEDB_connection As New OleDb.OleDbConnection 'New FactEngine.SQLiteConnection(Nothing, "", 1000, False) '2023-For Now. Will Fail for SQLite databases when doing database upgrades.

    Public pdbConnection As Object ' New FactEngine.SQLiteConnection(Nothing, "", 1000, False)
    Public pdb_OLEDB_connection As Object 'New FactEngine.SQLiteConnection(Nothing, "", 1000, False) '2023-For Now. Will Fail for SQLite databases when doing database upgrades.

    Public pdbDatabaseUpgradeConnection As New ADODB.Connection

    Public prPageNodes As New List(Of tEnterpriseEnterpriseView)  'Stores a list of the Pages in the EnterpriseView form. Used for Morphing.

    Public psStartupFBMFile As String = ""

    Public pbCancelClosing As Boolean = False

    Public pbLogStartup As Boolean = False

    Public prLogger As New tErrorLogger

    Public prUser As ClientServer.User 'Used when loging in using VirtualUI

    Public prSoftwareCategory As pcenumSoftwareCategory

    Public psApplicationApplicationVersionNr As String
    Public psApplicationDatabaseVersionNr As String
    Public psAssemblyFileVersionNumber As String

    Public prThinfinity As New Cybele.Thinfinity.VirtualUI

    'Crash Reporting - Automated/Online using www.raygun.com
    Public prRaygunClient As New RaygunClient("7M0bwJKUwe7HEJZen4wOpg")

    ''' <summary>
    ''' Used for when loading models, don't want to do database processing
    ''' </summary>
    Public pbDoDatabaseProcessing As Boolean = True

    '------------------------------------------------------
    'Reference Suite of Tables
    '------------------------------------------------------    
    Public prReferenceFieldValue As New tReferenceFieldValue

End Module