Imports System
Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Principal

Public Class CommonApplicationData
    Private applicationName As String
    Private companyName As String
    Private Shared ReadOnly directory As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)

    Public Sub New(ByVal companyName As String, ByVal applicationName As String)
        Me.New(companyName, applicationName, False)
    End Sub

    Public Sub New(ByVal companyFolder As String, ByVal applicationName As String, ByVal allUsers As Boolean)
        Me.applicationName = applicationName
        Me.companyName = companyFolder
    End Sub

    Public ReadOnly Property ProgramDataApplicationFolderPath As String
        Get
            Return Path.Combine(CompanyFolderPath, applicationName)
        End Get
    End Property

    Public ReadOnly Property CompanyFolderPath As String
        Get
            Return Path.Combine(directory, companyName)
        End Get
    End Property

    Public Sub CreateFolders(ByVal allUsers As Boolean)
        Dim directoryInfo As DirectoryInfo
        Dim directorySecurity As DirectorySecurity
        Dim rule As AccessRule
        Dim securityIdentifier As SecurityIdentifier = New SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, Nothing)

        If Not System.IO.Directory.Exists(CompanyFolderPath) Then
            directoryInfo = System.IO.Directory.CreateDirectory(CompanyFolderPath)
            Dim modified As Boolean
            directorySecurity = directoryInfo.GetAccessControl()
            rule = New FileSystemAccessRule(securityIdentifier, FileSystemRights.Write Or FileSystemRights.ReadAndExecute Or FileSystemRights.Modify, AccessControlType.Allow)
            directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, modified)
            directoryInfo.SetAccessControl(directorySecurity)
        End If

        If Not System.IO.Directory.Exists(ProgramDataApplicationFolderPath) Then
            directoryInfo = System.IO.Directory.CreateDirectory(ProgramDataApplicationFolderPath)

            If allUsers Then
                Dim modified As Boolean
                directorySecurity = directoryInfo.GetAccessControl()
                rule = New FileSystemAccessRule(securityIdentifier, FileSystemRights.Write Or FileSystemRights.ReadAndExecute Or FileSystemRights.Modify, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow)
                directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, modified)
                directoryInfo.SetAccessControl(directorySecurity)
            End If
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return ProgramDataApplicationFolderPath
    End Function
End Class
