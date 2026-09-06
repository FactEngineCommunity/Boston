Imports System.IO
Imports System.Xml.Serialization
Imports System.Reflection

Namespace OSM

    <Serializable>
    Public Class OpenAIFunction
        Implements IEquatable(Of OpenAIFunction)

        <XmlIgnore>
        <NonSerialized>
        Private _name As String

        Public Property name As String
            Get
                Return Me._name
            End Get
            Set(value As String)
                Me._name = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _description As String

        Public Property description As String
            Get
                Return Me._description
            End Get
            Set(value As String)
                Me._description = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _parameters As String

        Public Property parameters As String
            Get
                Return Me._parameters
            End Get
            Set(value As String)
                Me._parameters = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()

            Try
                Dim assembly As Assembly = Assembly.GetExecutingAssembly()
                Dim resourceName As String = "Boston.OSM-ExampleOpenAIFunctionParameterJSON.txt"

                Using stream As Stream = assembly.GetManifestResourceStream(resourceName)
                    If stream IsNot Nothing Then
                        Using reader As New StreamReader(stream)
                            Me.parameters = reader.ReadToEnd()
                        End Using
                    Else
                        Throw New Exception("Resource not found.")
                    End If
                End Using

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Shadows Function Equals(other As OpenAIFunction) As Boolean Implements IEquatable(Of OpenAIFunction).Equals
            Return Me.name = other.name
        End Function
    End Class

End Namespace
