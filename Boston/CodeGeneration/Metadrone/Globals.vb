Friend Class Globals

    Public Const ASSEMBLY_NAME_METADRONE As String = "Metadrone"
    Public Const ASSEMBLY_NAME_ICSHARPCODE As String = "ICSharpCode.TextEditor"

    Public Const ASSEMBLY_VERSION_METADRONE As String = "Angry Army 1.1"
    Public Const ASSEMBLY_VERSION_ICSHARPCODE As String = "3.2.1.6646"

    Public Const METADRONE_URL As String = "http://www.metadrone.com"

    Public Shared SourcePlugins As New PluginInterface.Sources.Loader()


    Public Shared Function ReadResource(ByVal resource As String) As String
        Using s As System.IO.Stream = GetType(Globals).Assembly.GetManifestResourceStream(resource)
            If s Is Nothing Then
                Throw New InvalidOperationException("Could not find embedded resource: " & resource)
            End If
            Using reader As System.IO.StreamReader = New System.IO.StreamReader(s)
                Return reader.ReadToEnd()
            End Using
        End Using
    End Function

    Public Shared Function VersionHistory() As System.Text.StringBuilder
        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("Angry Army 1.1")
        sb.AppendLine(" - Parameter counts for routines/functions.")
        sb.AppendLine()
        sb.AppendLine("Angry Army 1.0")
        sb.AppendLine(" - First release.")

        Return sb
    End Function

End Class
