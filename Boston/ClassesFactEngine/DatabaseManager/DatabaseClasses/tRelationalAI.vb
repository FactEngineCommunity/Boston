Imports Boston.FactEngine
Imports Boston.ORMQL
Imports System.Reflection
Imports System.Threading.Tasks
Imports RelationalAI

Namespace FactEngine
    Public Class RelationalAIConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        'Public DatabaseConnectionString As String

        'Private Client As RelationalAI.Client = Nothing

        ''' <summary>
        ''' RAI credentials are an ID and a secret key. To get them, follow these steps:   
        ''' - The Organization/Team/Person requests an account from RAI. Once the account Is Set up, an admin user Is assigned To the account.
        ''' - This admin creates OAuth clients. Each client Is identified by a client_id And a client_secret. 
        '''         This can be done Using the RAI Console — see the Managing OAuth Clients section In the Managing Users And OAuth Clients guide.
        '''         You can also Do this Using any Of the SDKs — see the Managing OAuth Clients section In Each SDK guide.
        ''' - The SDK user adds the client_id And client_secret To their configuration file.
        ''' --------------------------
        ''' host = azure.relationalai.com
        ''' client_id = ######### [from RelationalAI admin]
        ''' client_secret = ######### [from RelationalAI admin]
        ''' ------------------------------------------------------
        ''' Host = (config.ContainsKey("host") && config["host"] != null) ? (string)config["host"] : null;
        ''' Port = (config.ContainsKey("port") && config["port"] != null) ? (string)config["port"]  null;
        ''' Scheme = (config.ContainsKey("scheme") && config["scheme"] != null) ? (string)config["scheme"] : null;
        ''' Region = (config.ContainsKey("region") && config["region"] != null) ? (string)config["region"] : null;
        ''' Credentials = (config.ContainsKey("credentials") && config["credentials"] != null) ?
        ''' (ICredentials)config["credentials"] : null;
        ''' ========================================================
        ''' [default]
        ''' host = azure.relationalai.com
        ''' client_id = ######### [from RelationalAI admin]
        ''' client_secret = ######### [from RelationalAI admin]
        ''' 
        ''' Note that In place Of ######### the actual OAuth user credentials should appear. Here, [default] Is the profile name. You will refer To this profile name When Using the SDK.
        ''' 
        ''' Within the configuration file, you can also specify some Optional parameters, With the following Default values:
        ''' 
        ''' [default]
        ''' region = us-east
        ''' port = 443
        ''' scheme = https
        ''' client_credentials_url = https://login.relationalai.com/oauth/token
        ''' </summary>
        ''' <param name="arFBMModel"></param>
        ''' <param name="asDatabaseConnectionString"></param>
        ''' <param name="aiDefaultQueryLimit"></param>
        ''' <param name="abCreatingNewDatabase"></param>
        Public Sub New(ByRef arFBMModel As FBM.Model,
                       ByVal asDatabaseConnectionString As String,
                       ByVal aiDefaultQueryLimit As Integer,
                       Optional ByVal abCreatingNewDatabase As Boolean = False)

            Me.FBMModel = arFBMModel
            Me.DatabaseConnectionString = asDatabaseConnectionString
            Me.DefaultQueryLimit = aiDefaultQueryLimit

            Dim config As New Dictionary(Of String, Object) 'Generally uses config file, but we are not. E.g. Was Dim config = RelationalAI.Config.Read("", profile)

            Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
            lrSQLConnectionStringBuilder.ConnectionString = Me.DatabaseConnectionString

            'Host="azure.relationalai.com";Profile="default";ClientId="TestClientId";Secret="TestSecret";CredentialsURL="https://login.relationalai.com/oauth/token"

            Try

                config.Add("host", lrSQLConnectionStringBuilder("Host"))
                config.Add("profile", lrSQLConnectionStringBuilder("Profile"))
                config.Add("client_id", lrSQLConnectionStringBuilder("ClientId"))
                config.Add("client_secret", lrSQLConnectionStringBuilder("Secret"))
                config.Add("client_credentials_url", lrSQLConnectionStringBuilder("CredentialsURL"))
                config.Add("credentials", New RelationalAI.ClientCredentials(lrSQLConnectionStringBuilder("ClientId"), lrSQLConnectionStringBuilder("Secret"), lrSQLConnectionStringBuilder("CredentialsURL")))

                Dim context = New Client.Context(config)
                Dim client = New Client(context)

                If abCreatingNewDatabase Then Exit Sub

                Me.Connected = True 'Connections are actually made for each Query.                

            Catch ex As Exception
                Me.Connected = False
                Throw New Exception("Could Not connect to the database. Check the Model Configuration's Connection String.")
            End Try

        End Sub

        ''' <summary>
        ''' </summary>
        ''' <param name="database"></param>
        ''' <param name="profile"></param>
        ''' <returns></returns>
        Public Shadows Async Function createDatabase(ByVal database As String, ByVal Optional profile As String = "default") As Task(Of ORMQL.Recordset)

            Await Me.Client.CreateDatabaseAsync(database)

            Return New ORMQL.Recordset
        End Function

        Public Overrides Function GO(asQuery As String) As Recordset Implements iDatabaseConnection.GO
            Throw New NotImplementedException()
        End Function

        Public Overrides Function GOAsync(asQuery As String) As Task(Of Recordset) Implements iDatabaseConnection.GOAsync
            Throw New NotImplementedException()
        End Function

        Public Overrides Function GONonQuery(asQuery As String) As Recordset Implements iDatabaseConnection.GONonQuery
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace