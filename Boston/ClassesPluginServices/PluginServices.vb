Imports System.IO
Imports System.Reflection

Public Class PluginServices

    Public Structure AvailablePlugin
        Public AssemblyPath As String
        Public ClassName As String
        Public PluginObject As Viev.PluginFramework.IPlugin
    End Structure

    Public Shared Function FindPlugins(ByVal asPath As String, ByVal asInterface As String) As AvailablePlugin()

        Dim Plugins As ArrayList = New ArrayList()
        Dim strDLLs() As String, intIndex As Integer
        Dim objDLL As [Assembly]

        'Go through all DLLs in the directory, attempting to load them
        strDLLs = Directory.GetFileSystemEntries(asPath, "*.dll")
        For intIndex = 0 To strDLLs.Length - 1
            Try
                objDLL = [Assembly].LoadFrom(strDLLs(intIndex))
                ExamineAssembly(objDLL, asInterface, Plugins)
            Catch e As Exception
                'Error loading DLL, we don't need to do anything special
                MsgBox(e.Message)
            End Try
        Next

        'Return all plugins found
        Dim Results(Plugins.Count - 1) As AvailablePlugin

        If Plugins.Count <> 0 Then
            Plugins.CopyTo(Results)
            Return Results
        Else
            Return Nothing
        End If

    End Function

    Private Shared Sub ExamineAssembly(ByVal objDLL As [Assembly], ByVal strInterface As String, ByVal Plugins As ArrayList)
        Dim objType As Type
        Dim objInterface As Type
        Dim Plugin As AvailablePlugin

        'Loop through each type in the DLL
        For Each objType In objDLL.GetTypes
            'Only look at public types
            If objType.IsPublic = True Then
                'Ignore abstract classes
                If Not ((objType.Attributes And TypeAttributes.Abstract) = TypeAttributes.Abstract) Then

                    'See if this type implements our interface
                    objInterface = objType.GetInterface(strInterface, True)

                    If Not (objInterface Is Nothing) Then
                        'It does
                        Plugin = New AvailablePlugin()
                        Plugin.AssemblyPath = objDLL.Location
                        Plugin.ClassName = objType.FullName
                        Plugins.Add(Plugin)
                    End If

                End If
            End If
        Next
    End Sub

    Public Shared Function CreateInstance(ByVal Plugin As AvailablePlugin) As Object
        Dim objDLL As [Assembly]
        Dim objPlugin As Object

        Try
            'Load dll
            objDLL = [Assembly].LoadFrom(Plugin.AssemblyPath)

            'Create and return class instance
            objPlugin = objDLL.CreateInstance(Plugin.ClassName)
        Catch e As Exception
            Return Nothing
        End Try

        Return objPlugin
    End Function

End Class
