Imports Boston.Parser.Syntax.Strings

Namespace Parser.Bin

    <Serializable()> Friend Class BinObj
        Public srces As New List(Of CompiledSource)
        Public pkgs As New List(Of CompiledPackage)
    End Class



    <Serializable()> Friend Class CompiledSource
        Public Name As String
        Public Provider As String
        Public Transforms As Syntax.SourceTransforms

        Public Sub New()

        End Sub

        Public Sub New(ByVal Name As String, ByVal Provider As String, ByVal Transforms As Syntax.SourceTransforms)
            Me.Name = Name
            Me.Provider = Provider
            Me.Transforms = Transforms
        End Sub

        Friend Sub CheckPluginAvailable()
            If StrEq(Me.Provider, PluginInterface.Sources.Descriptions.SQLSERVER.ProviderName) Or _
               StrEq(Me.Provider, PluginInterface.Sources.Descriptions.ORACLE.ProviderName) Or _
               StrEq(Me.Provider, PluginInterface.Sources.Descriptions.ACCESS.ProviderName) Or _
               StrEq(Me.Provider, PluginInterface.Sources.Descriptions.ACCESS2K.ProviderName) Or _
               StrEq(Me.Provider, PluginInterface.Sources.Descriptions.EXCEL.ProviderName) Or _
               StrEq(Me.Provider, PluginInterface.Sources.Descriptions.OLEDB.ProviderName) Or _
               StrEq(Me.Provider, PluginInterface.Sources.Descriptions.ODBC.ProviderName) Then
                'These are fine
                Exit Sub
            Else
                'Fail if provider could not be identified
                If Globals.SourcePlugins.GetPlugin(Me.Provider) Is Nothing Then
                    Throw New Exception("Failed to load provider '" & Me.Provider & "' for source '" & Me.Name & "'.")
                End If

            End If
        End Sub
    End Class



    <Serializable()> Friend Class CompiledTemplate
        Public Name As String
        Public BaseNode As Syntax.SyntaxNode

        Public Sub New()

        End Sub

        Public Sub New(ByVal Name As String, ByVal BaseNode As Syntax.SyntaxNode)
            Me.Name = Name
            Me.BaseNode = BaseNode
        End Sub
    End Class



    <Serializable()> Friend Class CompiledPackage
        Public GUID As String
        Public CompiledTemplates As New List(Of CompiledTemplate)

        Public Sub New()

        End Sub

        Public Sub New(ByVal GUID As String, ByVal CompiledTemplates As List(Of CompiledTemplate))
            Me.GUID = GUID
            Me.CompiledTemplates = CompiledTemplates
        End Sub
    End Class

End Namespace