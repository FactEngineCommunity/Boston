Imports System.Xml
Imports System.Xml.Schema
Imports System.IO
Imports Boston.DataStore

Namespace XSD
    Public Class XSD

        <JsonProperty>
        <PrimaryKeyField>
        Public Property Identifier As String = System.Guid.NewGuid.ToString

        <JsonProperty>
        <ForeignKeyReference(GetType(FBM.Model), NameOf(FBM.Model.ModelId), True)>
        Public Shadows Property ModelId As String

        <JsonProperty>
        Public Property Name As String

        ' Persisted source of truth (store this in your DataStore)
        <JsonProperty>
        Public Property XSD As String

        <JsonProperty>
        Public Property SchemaFileName As String

        <JsonProperty>
        Public Property TargetNamespace As String = "http://example.org/example"

        <JsonProperty>
        Public Property XMLNamespacePrefix As String = "ex"

        <JsonIgnore>
        Private _xmlSchema As XmlSchema

        <JsonIgnore>
        Public Property XmlSchema As XmlSchema
            Get
                If _xmlSchema Is Nothing AndAlso Not String.IsNullOrWhiteSpace(XSD) Then
                    Using sr As New StringReader(XSD)
                        Using xr As XmlReader = XmlReader.Create(sr)
                            _xmlSchema = XmlSchema.Read(xr, AddressOf OnSchemaValidation)
                        End Using
                    End Using
                End If
                Return _xmlSchema
            End Get
            Set(value As XmlSchema)
                _xmlSchema = value
                _schemaSet = Nothing
                If value IsNot Nothing Then
                    Dim sb As New StringWriter()
                    Using xw = XmlWriter.Create(sb, New XmlWriterSettings With {.Indent = True})
                        value.Write(xw)
                    End Using
                    Dim s = sb.ToString()

                    ' Ensure we always have an explicit open/close schema tag
                    If s.Contains("<xs:schema") AndAlso s.EndsWith("/>") Then
                        s = s.Replace("/>", ">" & vbCrLf & "</xs:schema>")
                    End If

                    XSD = s
                Else
                    XSD = Nothing
                End If
            End Set
        End Property

        <JsonIgnore>
        Private _schemaSet As XmlSchemaSet

        <JsonIgnore>
        Public Property SchemaSet As XmlSchemaSet
            Get
                If _schemaSet Is Nothing Then
                    _schemaSet = New XmlSchemaSet()
                    If Not String.IsNullOrWhiteSpace(TargetNamespace) AndAlso XmlSchema IsNot Nothing Then
                        _schemaSet.Add(XmlSchema)
                        _schemaSet.Compile()
                    End If
                End If
                Return _schemaSet
            End Get
            Set(value As XmlSchemaSet)
                Me._schemaSet = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Private Sub OnSchemaValidation(sender As Object, e As ValidationEventArgs)
            ' You can log or attach to your DataStore error mechanism here.
            ' Example: Me.AddError(e.Message)  ' if your base provides it
        End Sub

    End Class

End Namespace
