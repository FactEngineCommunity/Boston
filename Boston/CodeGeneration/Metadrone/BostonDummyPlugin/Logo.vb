Imports Boston.PluginInterface.Sources

Namespace SourcePlugins.Boston
    Public Class Logo
        Inherits UserControl
        Implements PluginInterface.Sources.ISourceDescription

        Friend WithEvents picLogo As PictureBox

        Public ReadOnly Property ProviderName As String Implements ISourceDescription.ProviderName
            Get
                Return "Boston Model"
            End Get
        End Property

        Public ReadOnly Property Description As String Implements ISourceDescription.Description
            Get
                Return "Boston Model"
            End Get
        End Property

        Public ReadOnly Property LogoImage As Image Implements ISourceDescription.LogoImage
            Get
                Return picLogo.Image
            End Get
        End Property

        Private Sub InitializeComponent()
            Me.picLogo = New System.Windows.Forms.PictureBox()
            CType(Me.picLogo, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'picLogo
            '
            Me.picLogo.Image = Global.Boston.My.Resources.MenuImagesMain.ORMStudio32x322
            Me.picLogo.Location = New System.Drawing.Point(0, 0)
            Me.picLogo.Name = "picLogo"
            Me.picLogo.Size = New System.Drawing.Size(32, 32)
            Me.picLogo.TabIndex = 0
            Me.picLogo.TabStop = False
            '
            'Logo
            '
            Me.Controls.Add(Me.picLogo)
            Me.Name = "Logo"
            Me.Size = New System.Drawing.Size(32, 32)
            CType(Me.picLogo, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
    End Class

End Namespace

