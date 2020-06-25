Namespace PluginInterface.Sources


#Region "Description implementations"

    Friend Class DescriptionImplementations

        Public Class SQLServer
            Implements ISourceDescription

            Public ReadOnly Property Description() As String Implements ISourceDescription.Description
                Get
                    Return "SQL Server"
                End Get
            End Property

            Public ReadOnly Property LogoImage() As System.Drawing.Image Implements ISourceDescription.LogoImage
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property ProviderName() As String Implements ISourceDescription.ProviderName
                Get
                    Return "SQLServer"
                End Get
            End Property
        End Class

        Public Class Oracle
            Implements ISourceDescription

            Public ReadOnly Property Description() As String Implements ISourceDescription.Description
                Get
                    Return "Oracle"
                End Get
            End Property

            Public ReadOnly Property LogoImage() As System.Drawing.Image Implements ISourceDescription.LogoImage
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property ProviderName() As String Implements ISourceDescription.ProviderName
                Get
                    Return "Oracle"
                End Get
            End Property
        End Class

        Public Class OLEDB
            Implements ISourceDescription

            Public ReadOnly Property Description() As String Implements ISourceDescription.Description
                Get
                    Return "Generic OLEDB"
                End Get
            End Property

            Public ReadOnly Property LogoImage() As System.Drawing.Image Implements ISourceDescription.LogoImage
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property ProviderName() As String Implements ISourceDescription.ProviderName
                Get
                    Return "OLEDB"
                End Get
            End Property
        End Class

        Public Class ODBC
            Implements ISourceDescription

            Public ReadOnly Property Description() As String Implements ISourceDescription.Description
                Get
                    Return "Generic ODBC"
                End Get
            End Property

            Public ReadOnly Property LogoImage() As System.Drawing.Image Implements ISourceDescription.LogoImage
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property ProviderName() As String Implements ISourceDescription.ProviderName
                Get
                    Return "ODBC"
                End Get
            End Property
        End Class

        Public Class Access
            Implements ISourceDescription

            Public ReadOnly Property Description() As String Implements ISourceDescription.Description
                Get
                    Return "Microsoft Access"
                End Get
            End Property

            Public ReadOnly Property LogoImage() As System.Drawing.Image Implements ISourceDescription.LogoImage
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property ProviderName() As String Implements ISourceDescription.ProviderName
                Get
                    Return "Access"
                End Get
            End Property
        End Class

        Public Class Access2K
            Implements ISourceDescription

            Public ReadOnly Property Description() As String Implements ISourceDescription.Description
                Get
                    Return "Microsoft Access"
                End Get
            End Property

            Public ReadOnly Property LogoImage() As System.Drawing.Image Implements ISourceDescription.LogoImage
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property ProviderName() As String Implements ISourceDescription.ProviderName
                Get
                    Return "Access2k"
                End Get
            End Property
        End Class

        Public Class Excel
            Implements ISourceDescription

            Public ReadOnly Property Description() As String Implements ISourceDescription.Description
                Get
                    Return "Microsoft Excel"
                End Get
            End Property

            Public ReadOnly Property LogoImage() As System.Drawing.Image Implements ISourceDescription.LogoImage
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property ProviderName() As String Implements ISourceDescription.ProviderName
                Get
                    Return "Excel"
                End Get
            End Property
        End Class

        Public Class Excel2k
            Implements ISourceDescription

            Public ReadOnly Property Description() As String Implements ISourceDescription.Description
                Get
                    Return "Microsoft Excel"
                End Get
            End Property

            Public ReadOnly Property LogoImage() As System.Drawing.Image Implements ISourceDescription.LogoImage
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property ProviderName() As String Implements ISourceDescription.ProviderName
                Get
                    Return "Excel2k"
                End Get
            End Property
        End Class

    End Class

#End Region


    Friend Class Descriptions

        Public Shared SQLSERVER As New DescriptionImplementations.SQLServer()
        Public Shared ORACLE As New DescriptionImplementations.Oracle()
        Public Shared OLEDB As New DescriptionImplementations.OLEDB()
        Public Shared ODBC As New DescriptionImplementations.ODBC()
        Public Shared ACCESS As New DescriptionImplementations.Access()
        Public Shared ACCESS2K As New DescriptionImplementations.Access2K()
        Public Shared EXCEL As New DescriptionImplementations.Excel()
        Public Shared EXCEL2K As New DescriptionImplementations.Excel2k()

    End Class

End Namespace