Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.IO
Imports System.ComponentModel
Imports Newtonsoft.Json
Imports System.Reflection


Namespace OSM

    <Serializable>
    Public Class TaskOpenAIFunction
        Implements IEquatable(Of OSM.TaskOpenAIFunction)

        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private _ID As String = System.Guid.NewGuid.ToString
        ''' <summary>
        ''' The inique ID of the FEQL Query against a Task. I.e. The Name of the FEQL Query may change.
        ''' </summary>
        ''' <returns></returns>
        Public Property ID As String
            Get
                Return Me._ID
            End Get
            Set(value As String)
                Me._ID = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _TaskId As String
        ''' <summary>
        ''' The Task to which a Function is associated and that OpenAI/GPT can use for the Task.
        ''' </summary>
        ''' <returns></returns>
        Public Property TaskId As String
            Get
                Return Me._TaskId
            End Get
            Set(value As String)
                Me._TaskId = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _OpenAIFunctionName As String
        ''' <summary>
        ''' The name of the Function that OpenAI/GPT can use for the Task.
        ''' </summary>
        Public Property OpenAIFunctionName As String
            Get
                Return Me._OpenAIFunctionName
            End Get
            Set(value As String)
                Me._OpenAIFunctionName = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asTaskId As String, ByVal asOpenAITaskName As String)

            Me.TaskId = asTaskId
            Me.OpenAIFunctionName = asOpenAITaskName

        End Sub

        Public Shadows Function Equals(other As TaskOpenAIFunction) As Boolean Implements IEquatable(Of TaskOpenAIFunction).Equals

            Return Me.TaskId = other.TaskId And Me.OpenAIFunctionName = other.OpenAIFunctionName

        End Function
    End Class

End Namespace