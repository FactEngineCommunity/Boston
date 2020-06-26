Imports Boston.PluginInterface.Sources

Namespace SourcePlugins.Boston
    Public Class SourceDescription
        Implements PluginInterface.Sources.ISourceDescription

        Public ReadOnly Property ProviderName As String Implements ISourceDescription.ProviderName
            Get
                Return "Boston Model direct access"
            End Get
        End Property

        Public ReadOnly Property Description As String Implements ISourceDescription.Description
            Get
                Return "Allows direct access to a Model in Boston"
            End Get
        End Property

        Public ReadOnly Property LogoImage As Image Implements ISourceDescription.LogoImage
            Get
                Return My.Resources.MenuImages.Boston32x32
            End Get
        End Property
    End Class

End Namespace

