Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

Namespace UMS

    Public Class Facts

        ''' <summary>
        ''' E.g. Enlish, Dutch, German
        ''' </summary>
        Public Property Language As String = Nothing

        ''' <summary>
        ''' E.g. 'Part is in Bin in Warehouse', 'Bin houses Part in Bin'
        ''' </summary>
        Public Property Facts As List(Of String)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asLanguage As String)
            Me.Language = asLanguage
            Me.Facts = New List(Of String)
        End Sub

    End Class

End Namespace
