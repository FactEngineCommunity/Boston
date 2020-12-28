Namespace PluginInterface.Sources

    Friend Class Loader

        Public Class Plugin
            Public AssemblyName As String = Nothing
            Public AssemblyVersion As String = Nothing
            Public Path As String = Nothing
            Public Connection As IConnection = Nothing
            Public WithEvents Manager As IManageSource = Nothing
            Public SourceDescription As ISourceDescription = Nothing
            Public PluginDescription As IPluginDescription = Nothing

            Private Sub Manager_ValueChanged(value As Object) Handles Manager.ValueChanged

            End Sub
        End Class

        Public Plugins As New List(Of Plugin)

        Public Sub LoadAllAssemblies()
            Dim di As New IO.DirectoryInfo(My.Application.Info.DirectoryPath)
            Dim aryFi As IO.FileInfo() = di.GetFiles("*.dll")
            For Each fi As IO.FileInfo In aryFi
                Call Me.LoadAssembly(fi.FullName)
            Next
        End Sub

        ''' <summary>
        ''' Loads the BostonDummyPlugin
        ''' </summary>
        Public Sub loadBostonCodeGenerationPlugin()

            'Load the Boston Dummy Plugin
            Dim plugin As New Plugin()
            plugin.AssemblyName = "Boston Plugin"
            plugin.AssemblyVersion = "1.0"
            plugin.Path = ""
            plugin.Connection = New SourcePlugins.Boston.Connection
            plugin.Manager = New SourcePlugins.Boston.Manage
            plugin.PluginDescription = New SourcePlugins.Boston.Description
            plugin.SourceDescription = New SourcePlugins.Boston.SourceDescription
            Me.Plugins.Add(plugin)

        End Sub

        Public Function LoadAssembly(ByVal Path As String) As Boolean

            Dim asm As Reflection.Assembly = Me.GetAssembly(Path)
            If asm Is Nothing Then Return False

            Dim plugin As New Plugin()
            plugin.AssemblyName = asm.GetName.Name.Trim
            plugin.AssemblyVersion = asm.GetName.Version.ToString.Trim
            plugin.Path = Path

            'Create and return class instance
            'Boston plugin code.not metadrone code
            'objPlugin = asm.CreateInstance("PluginInterface.Sources.IConnection")
            ''plugin.Connection = DirectCast(PluginServices.CreateInstance(prApplication.Plugins(liInd)), PluginInterface.Sources.IConnection)

            plugin.Connection = Me.GetConnectionFromAssembly(asm)
            If plugin.Connection Is Nothing Then Return False

            plugin.Manager = Me.GetManagerFromAssembly(asm)
            If plugin.Manager Is Nothing Then Return False

            plugin.SourceDescription = Me.GetSourceDescriptionFromAssembly(asm)
            If plugin.SourceDescription Is Nothing Then Return False

            plugin.PluginDescription = Me.GetPluginDescriptionFromAssembly(asm)
            If plugin.PluginDescription Is Nothing Then Return False

            Me.Plugins.Add(plugin)
            Return True
        End Function

        Public Function GetPlugin(ByVal ProviderName As String) As Plugin
            For Each pi In Me.Plugins
                If pi.SourceDescription.ProviderName.Equals(ProviderName, StringComparison.CurrentCultureIgnoreCase) Then
                    Return pi
                End If
            Next

            Return Nothing
        End Function

        Private Function GetAssembly(ByVal Path As String) As Reflection.Assembly
            'Load assembly
            Dim asm As Reflection.Assembly = Reflection.Assembly.LoadFrom(Path)

            'Get assembly's GUID
            Dim Attributes As Object()
            Attributes = asm.GetCustomAttributes(GetType(System.Runtime.InteropServices.GuidAttribute), False)
            If Attributes.Length > 0 Then
                'Make sure not checking against Metadrone assembly
                If DirectCast(Attributes(0), System.Runtime.InteropServices.GuidAttribute).Value.Equals("672cebab-5530-4703-8471-9f0121df2576") Then
                    Return Nothing
                End If
            End If

            Return asm
        End Function

        Private Function GetConnectionFromAssembly(ByVal asm As Reflection.Assembly) As IConnection

            Try
                For Each typeAsm In asm.GetTypes
                    If typeAsm.GetInterface(GetType(IConnection).FullName) IsNot Nothing Then
                        Return CType(asm.CreateInstance(typeAsm.FullName, True), IConnection)
                    End If
                Next

                Return Nothing

            Catch ex As system.Reflection.ReflectionTypeLoadException
                Dim lsMessage As String = ""
                For Each inner As Exception In ex.LoaderExceptions
                    lsMessage &= inner.Message & vbCrLf
                Next
                MsgBox(lsMessage)
                Return Nothing
            End Try
        End Function

        Private Function GetManagerFromAssembly(ByVal asm As Reflection.Assembly) As IManageSource
            For Each typeAsm In asm.GetTypes
                If typeAsm.GetInterface(GetType(IManageSource).FullName) IsNot Nothing Then
                    Return CType(asm.CreateInstance(typeAsm.FullName, True), IManageSource)
                End If
            Next

            Return Nothing
        End Function

        Private Function GetSourceDescriptionFromAssembly(ByVal asm As Reflection.Assembly) As ISourceDescription
            For Each typeAsm In asm.GetTypes
                If typeAsm.GetInterface(GetType(ISourceDescription).FullName) IsNot Nothing Then
                    Return CType(asm.CreateInstance(typeAsm.FullName, True), ISourceDescription)
                End If
            Next

            Return Nothing
        End Function

        Private Function GetPluginDescriptionFromAssembly(ByVal asm As Reflection.Assembly) As IPluginDescription
            For Each typeAsm In asm.GetTypes
                If typeAsm.GetInterface(GetType(IPluginDescription).FullName) IsNot Nothing Then
                    Return CType(asm.CreateInstance(typeAsm.FullName, True), IPluginDescription)
                End If
            Next

            Return Nothing
        End Function

    End Class

End Namespace