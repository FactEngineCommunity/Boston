Imports System.Xml
Imports System.Xml.Serialization
Imports Newtonsoft.Json
Imports System.Reflection


Namespace OSM

    <Serializable>
    Public Class TaskFEQLQuery

        <JsonIgnore>
        <XmlIgnore>
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

        <JsonIgnore>
        <XmlIgnore>
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

        <JsonIgnore>
        <XmlIgnore>
        Public _QueryName As String
        ''' <summary>
        ''' The name of the FEQL Query. E.g. "GetMenuForPizzeria".
        ''' </summary>
        ''' <returns></returns>
        Public Property QueryName As String
            Get
                Return Me._QueryName
            End Get
            Set(value As String)
                Me._QueryName = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Public _QueryResultDescription As String
        ''' <summary>
        ''' The ResultDescription of the FEQL Query. E.g. "GetMenuForPizzeria".
        ''' </summary>
        ''' <returns></returns>
        Public Property QueryResultDescription As String
            Get
                Return Me._QueryResultDescription
            End Get
            Set(value As String)
                Me._QueryResultDescription = value
            End Set
        End Property


        <JsonIgnore>
        <XmlIgnore>
        Public _FEQLQuery As String
        ''' <summary>
        ''' The FEQL Query to run.
        ''' </summary>
        ''' <returns></returns>
        Public Property FEQLQuery As String
            Get
                Return Me._FEQLQuery
            End Get
            Set(value As String)
                Me._FEQLQuery = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Public _IsUserQueryAnswerer As Boolean
        ''' <summary>
        ''' The name of the FEQL Query. E.g. "GetMenuForPizzeria".
        ''' </summary>
        ''' <returns></returns>
        Public Property IsUserQueryAnswerer As Boolean
            Get
                Return Me._IsUserQueryAnswerer
            End Get
            Set(value As Boolean)
                Me._IsUserQueryAnswerer = value
            End Set
        End Property


        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <param name="asTaskId">The TaskId of the Task that the named FEQL Query is allocated to.</param>
        Public Sub New(ByVal asTaskId As String)

            Try
                Me.TaskId = asTaskId

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace
