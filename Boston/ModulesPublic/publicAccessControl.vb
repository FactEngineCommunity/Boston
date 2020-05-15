Imports System
Imports System.IO
Imports System.Security.AccessControl

'Allows the ability to set or remove access control to a directory.
'See https://msdn.microsoft.com/en-us/library/system.io.directory.setaccesscontrol.aspx

Public Module publicAccessControl

    ''' <summary>
    ''' Adds an ACL entry on the specified directory for the specified account.
    ''' AddDirectorySecurity(DirectoryName, "MYDOMAIN\MyAccount", FileSystemRights.ReadData, AccessControlType.Allow)
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <param name="Account"></param>
    ''' <param name="Rights"></param>
    ''' <param name="ControlType"></param>
    ''' <remarks></remarks>
    Public Sub AddDirectorySecurity(ByVal FileName As String, ByVal Account As String, ByVal Rights As FileSystemRights, ByVal ControlType As AccessControlType)
        ' Create a new DirectoryInfoobject.
        Dim dInfo As New DirectoryInfo(FileName)

        ' Get a DirectorySecurity object that represents the 
        ' current security settings.
        Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()

        ' Add the FileSystemAccessRule to the security settings. 
        'dSecurity.AddAccessRule(New FileSystemAccessRule(Account, Rights, ControlType))
        dSecurity.AddAccessRule(New FileSystemAccessRule(Account, Rights, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, ControlType))

        ' Set the new access settings.
        dInfo.SetAccessControl(dSecurity)

    End Sub


    ' Removes an ACL entry on the specified directory for the specified account.
    Sub RemoveDirectorySecurity(ByVal FileName As String, ByVal Account As String, ByVal Rights As FileSystemRights, ByVal ControlType As AccessControlType)
        ' Create a new DirectoryInfo object.
        Dim dInfo As New DirectoryInfo(FileName)

        ' Get a DirectorySecurity object that represents the 
        ' current security settings.
        Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()

        ' Add the FileSystemAccessRule to the security settings. 
        dSecurity.RemoveAccessRule(New FileSystemAccessRule(Account, Rights, ControlType))

        ' Set the new access settings.
        dInfo.SetAccessControl(dSecurity)

    End Sub
End Module
