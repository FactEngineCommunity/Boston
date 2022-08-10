Imports System.IO
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Namespace NORMA

    <XmlRoot([Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMRoot", ElementName:="ORM2", IsNullable:=False)>
    Public Class ORMDocument

        ''' <summary>
        ''' Get or Set the ORM Model for XML file
        ''' </summary>
        ''' <returns></returns>
        <XmlElement([Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Public Property ORMModel() As ORMModel

        ''' <summary>
        ''' Get or Set the ModelErrorDisplayFilter property
        ''' </summary>
        <XmlElement([Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Public Property ModelErrorDisplayFilter() As ErrorDisplayFilter

        ''' <summary>
        ''' Get or Set the ORM Diagrams property
        ''' </summary>
        <XmlElement("ORMDiagram", [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
        Public Property ORMDiagram() As ORMDiagram.ORMDiagram() = {}

        ''' <summary>
        ''' Get or Set the DiagramDisplay property
        ''' </summary>
        ''' <returns></returns>
        <XmlElement([Namespace]:="http://schemas.neumont.edu/ORM/2008-11/DiagramDisplay")>
        Public Property DiagramDisplay() As DiagramDisplay

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            Me.ORMModel = New NORMA.ORMModel()
        End Sub

#Region "Serialize/Deserialize"

        ''' <summary>
        ''' This method will serialize the provided NORMA orm into xml file
        ''' </summary>
        ''' <param name="ORM">The NORMA class object which will be serialized</param>
        ''' <param name="filename">The file path where the serialized file will be save</param>
        Public Sub SerializeObject(ByRef ORM As NORMA.ORMDocument, ByVal filename As String)
            Dim serializer As New XmlSerializer(GetType(NORMA.ORMDocument))
            ' delete the file if already exist
            If File.Exists(filename) Then
                File.Delete(filename)
            End If
            ' Create an XmlTextWriter using a FileStream.
            Using fs As New FileStream(filename, FileMode.Create)
                Using writer = Xml.XmlWriter.Create(fs, New Xml.XmlWriterSettings() With {
                                          .NamespaceHandling = Xml.NamespaceHandling.OmitDuplicates,
                                          .OmitXmlDeclaration = False,
                                          .Indent = True,
                                          .Encoding = Text.Encoding.UTF8
                                          })
                    ' add NORMA orm namespaces in the settings
                    Dim ns As New XmlSerializerNamespaces()
                    ns.Add("orm", "http://schemas.neumont.edu/ORM/2006-04/ORMCore")
                    ns.Add("ormRoot", "http://schemas.neumont.edu/ORM/2006-04/ORMRoot")
                    ns.Add("ormDiagram", "http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")
                    ns.Add("diagramDisplay", "http://schemas.neumont.edu/ORM/2008-11/DiagramDisplay")
                    ' Serialize using the XmlTextWriter.
                    serializer.Serialize(writer, ORM, ns)
                End Using
            End Using

            ValidateSchema(filename)

        End Sub

        ''' <summary>
        ''' This method will be used to validate schema for NORMA XML file
        ''' This will check the XML of provided file and throw exception if there is any error
        ''' </summary>
        ''' <param name="fileName">Full file path</param>
        Public Sub ValidateSchema(ByVal fileName As String)
            Try

                Dim lrXMLReaderSettings As New XmlReaderSettings()
                lrXMLReaderSettings.Schemas.Add("http://schemas.neumont.edu/ORM/2006-04/ORMRoot", ".\ClassesNORMA\NORMAXSD\ORM2Root.xsd")
                lrXMLReaderSettings.Schemas.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore", ".\ClassesNORMA\NORMAXSD\ORM2Core.xsd")
                lrXMLReaderSettings.Schemas.Add("http://schemas.neumont.edu/ORM/2006-04/ORMDiagram", ".\ClassesNORMA\NORMAXSD\ORM2Diagram.xsd")
                lrXMLReaderSettings.Schemas.Add("http://schemas.neumont.edu/ORM/2008-11/DiagramDisplay", ".\ClassesNORMA\NORMAXSD\DiagramDisplay.xsd")
                lrXMLReaderSettings.ValidationType = ValidationType.Schema

                Dim lrXMLReader As XmlReader = XmlReader.Create(fileName, lrXMLReaderSettings)
                Dim lrXMLDocument As XmlDocument = New XmlDocument()
                lrXMLDocument.Load(lrXMLReader)

                Dim lrErrorEventHandler As New ValidationEventHandler(Sub(lrSender As Object, lrEvent As ValidationEventArgs)
                                                                          Select Case lrEvent.Severity
                                                                              Case XmlSeverityType.Error
                                                                                  Console.WriteLine("Error: {0}", lrEvent.Message)
                                                                              Case XmlSeverityType.Warning
                                                                                  Console.WriteLine("Warning {0}", lrEvent.Message)
                                                                          End Select
                                                                      End Sub)

                ' the following call to Validate succeeds.
                lrXMLDocument.Validate(lrErrorEventHandler)

            Catch ex As XmlSchemaValidationException
                Dim lsErrorMessgae As String = $"{If(ex.SourceObject?.ToString(), "Position")} : {ex.LineNumber} [Line Number], {ex.LinePosition} [Line Position]"
                lsErrorMessgae &= $"{vbCrLf}{ex.Message}"
                If Not IsNothing(ex.InnerException) Then
                    lsErrorMessgae &= $"{vbCrLf}InnerException: {ex.Message}"
                End If
                Throw New XmlSchemaValidationException(lsErrorMessgae, ex.InnerException, ex.LineNumber, ex.LinePosition)
            End Try

        End Sub

        ''' <summary>
        ''' This function will be used to read the NORMA orm file
        ''' </summary>
        ''' <param name="filename">Full file path for orm file</param>
        ''' <returns></returns>
        Public Shared Function Deserialize(ByVal filename As String) As NORMA.ORMDocument
            ' Create an instance of the XmlSerializer.
            Dim serializer As New XmlSerializer(GetType(NORMA.ORMDocument))

            Using reader As New FileStream(filename, FileMode.Open)

                ' Call the Deserialize method to restore the object's state.
                Return CType(serializer.Deserialize(reader), NORMA.ORMDocument)
            End Using
        End Function

#End Region

    End Class

End Namespace
