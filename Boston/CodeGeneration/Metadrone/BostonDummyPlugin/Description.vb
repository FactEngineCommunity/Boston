Imports Boston.PluginInterface
Imports Boston.PluginInterface.Sources

Namespace SourcePlugins.Boston
    Public Class Description
        Implements PluginInterface.IPluginDescription

        Public ReadOnly Property ProductInformation As String Implements IPluginDescription.ProductInformation
            Get
                Return "Boston Model Connection"
            End Get
        End Property

        Public ReadOnly Property LicenceInformation As String Implements IPluginDescription.LicenceInformation
            Get
                Return "Standard to Boston."
            End Get
        End Property

    End Class

End Namespace
