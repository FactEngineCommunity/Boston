Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.IO
Imports System.ComponentModel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Azure.AI.OpenAI
Imports Azure
Imports System.Text.Json
Imports GemBox.Email
Imports System.Linq.Expressions
Imports System.Reflection


Namespace OSM

    <Serializable>
    Public Class Task

#Region "Properties"

#Region "Brain Bits"

        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private WithEvents _Brain As tBrain = Nothing

        <JsonIgnore>
        <XmlIgnore>
        Public Property Brain As tBrain
            Get
                Return Me._Brain
            End Get
            Set(value As tBrain)
                Me._Brain = value
            End Set
        End Property

        ''' <summary>
        ''' True if is the high level AGI of a suite of Tasks/Bots. Else False.
        ''' </summary>
        <XmlIgnore>
        <NonSerialized>
        Private _IsAGI As Boolean = False
        <XmlAttribute>
        Public Property IsAGI As Boolean
            Get
                Return Me._IsAGI
            End Get
            Set(value As Boolean)
                Me._IsAGI = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _IgnoreNonFunctionAGIResponses As Boolean = False
        <XmlAttribute>
        Public Property IgnoreNonFunctionAGIResponses As Boolean
            Get
                Return Me._IgnoreNonFunctionAGIResponses
            End Get
            Set(value As Boolean)
                Me._IgnoreNonFunctionAGIResponses = value
            End Set
        End Property

#End Region

#Region "Engine/Model"
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private _AIEngineUsed As pcenumOSMAIEngineName = pcenumOSMAIEngineName.OpenAI
        <JsonProperty>
        <XmlAttribute>
        Public Property AIEngineUsed As pcenumOSMAIEngineName
            Get
                Return Me._AIEngineUsed
            End Get
            Set(value As pcenumOSMAIEngineName)
                Me._AIEngineUsed = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private _AIModelName As pcenumOSMAIModelName = pcenumOSMAIModelName.gpt_4_0125_preview
        <JsonProperty>
        <XmlAttribute>
        Public Property AIModelName As pcenumOSMAIModelName
            Get
                Return Me._AIModelName
            End Get
            Set(value As pcenumOSMAIModelName)
                Me._AIModelName = value
            End Set
        End Property
#End Region

#Region "Other Properties"
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Public UserId As Integer = 1 'For now, for testing.

        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Public DebugMode As pcenumDebugMode = pcenumDebugMode.NoLogging

        <XmlIgnore>
        <NonSerialized>
        Private _TaskId As String = System.Guid.NewGuid.ToString
        <XmlAttribute>
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
        Private _TaskInstanceId As String = System.Guid.NewGuid.ToString
        ''' <summary>
        ''' The unique Id of an 'instance' of the Task.
        '''   Each Task may/will have multiple 'instances' over time.
        ''' </summary>
        ''' <returns></returns>
        <JsonIgnore>
        <XmlIgnore>
        Public Property TaskInstanceId As String
            Get
                Return Me._TaskInstanceId
            End Get
            Set(value As String)
                Me._TaskInstanceId = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Public TaskStatus As pcenumOSMTaskStatus = pcenumOSMTaskStatus.Running

#Region "Sub-Task Management Properties"

        <XmlIgnore>
        <NonSerialized>
        Private _IsSubTask As Boolean
        ''' <summary>
        ''' True if the Task is to be used exclusively as a Sub-Task. I.e. Witin a workflow may be beneficial to delegate certain work.
        ''' </summary>
        ''' <returns></returns>
        <JsonIgnore>
        <XmlIgnore>
        Public Property IsSubTask As Boolean
            Get
                Return Me._IsSubTask
            End Get
            Set(value As Boolean)
                Me._IsSubTask = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _ExpectedResult As New List(Of OSM.TaskProperty)
        ''' <summary>
        ''' The list of Properties that are expected to be returned from the Task. E.g. Credit Card Details.
        ''' </summary>
        ''' <returns></returns>
        <JsonIgnore>
        <XmlIgnore>
        Public Property ExpectedResult As List(Of OSM.TaskProperty)
            Get
                Return Me._ExpectedResult
            End Get
            Set(value As List(Of OSM.TaskProperty))
                Me._ExpectedResult = value
            End Set
        End Property

#End Region

        ''' <summary>
        ''' Used when talking about "That"...e.g. Whatever is in the Box
        ''' </summary>
        <XmlIgnore>
        <JsonIgnore>
        <NonSerialized>
        Private ThatVariable As Object = Nothing

#Region "Registers"

        ''' <summary>
        ''' Can be updated by a function call and used in further procesing. I.e. Fed back to the SAC via a Template.
        '''   Forms part of Registers A through D
        ''' </summary>
        <XmlIgnore>
        <JsonIgnore>
        <NonSerialized>
        Private RegisterA As Object = Nothing

        ''' <summary>
        ''' Can be updated by a function call and used in further procesing. I.e. Fed back to the SAC via a Template.
        '''   Forms part of Registers A through D
        ''' </summary>
        <XmlIgnore>
        <JsonIgnore>
        <NonSerialized>
        Private RegisterB As Object = Nothing

        ''' <summary>
        ''' Can be updated by a function call and used in further procesing. I.e. Fed back to the SAC via a Template.
        '''   Forms part of Registers A through D
        ''' </summary>
        <XmlIgnore>
        <JsonIgnore>
        <NonSerialized>
        Private RegisterC As Object = Nothing

        ''' <summary>
        ''' Can be updated by a function call and used in further procesing. I.e. Fed back to the SAC via a Template.
        '''   Forms part of Registers A through D
        ''' </summary>
        <XmlIgnore>
        <JsonIgnore>
        <NonSerialized>
        Private RegisterD As Object = Nothing

#End Region

        <XmlIgnore>
        <NonSerialized>
        Private _Name As String
        ''' <summary>
        ''' The name of the Task.
        ''' </summary>
        <XmlAttribute>
        Public Property Name As String
            Get
                Return Me._Name
            End Get
            Set(value As String)
                Me._Name = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _BotName As String = ""
        ''' <summary>
        ''' The personal name of the Bot that the Task represents. E.g. Phillip, Kathy, Robert, etc.
        ''' </summary>
        <XmlAttribute>
        Public Property BotName As String
            Get
                Return Me._BotName
            End Get
            Set(value As String)
                Me._BotName = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _Description As String
        ''' <summary>
        ''' A description of the Task. See also GoalShortDescription and GoalLongDescription.
        ''' </summary>
        <XmlElement>
        Public Property Description As String
            Get
                Return Me._Description
            End Get
            Set(value As String)
                Me._Description = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _GoalShortDescription As String
        <XmlElement>
        Public Property GoalShortDescription As String
            Get
                Return Me._GoalShortDescription
            End Get
            Set(value As String)
                Me._GoalShortDescription = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _GoalLongDescription As String
        <XmlElement>
        Public Property GoalLongDescription As String
            Get
                Return Me._GoalLongDescription
            End Get
            Set(value As String)
                Me._GoalLongDescription = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Public TheBox As frmOSMTheBox

        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Public DataModel As RDS.Model

        <XmlAttribute>
        <NonSerialized>
        Private _ModelId As String = Nothing
        <XmlElement>
        Public Property ModelId As String
            Get
                If Me._ModelId IsNot Nothing Then
                    Return Me._ModelId
                ElseIf Me.DataModel Is Nothing Then
                    Return Nothing
                Else
                    Return Me.DataModel.Model.ModelId
                End If
            End Get
            Set(value As String)
                Me._ModelId = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _TaskPIJSON As String
        ''' <summary>
        ''' The Pseudocode In JSON (PI-JSON) for this Task.
        ''' </summary>
        <XmlElement>
        Public Property TaskPIJSON As String
            Get
                Return Me._TaskPIJSON
            End Get
            Set(value As String)
                Me._TaskPIJSON = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _TaskShortPIJSON As String
        ''' <summary>
        ''' The Pseudocode In JSON (PI-JSON) for this Task.
        ''' </summary>
        <XmlElement>
        Public Property TaskShortPIJSON As String
            Get
                Return Me._TaskShortPIJSON
            End Get
            Set(value As String)
                Me._TaskShortPIJSON = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _UsePIJSON As Boolean = False
        ''' <summary>
        ''' The Pseudocode In JSON (PI-JSON) for this Task.
        ''' </summary>
        <XmlElement>
        Public Property UsePIJSON As Boolean
            Get
                Return Me._UsePIJSON
            End Get
            Set(value As Boolean)
                Me._UsePIJSON = value
            End Set
        End Property


        <XmlIgnore>
        <NonSerialized>
        Private _UseShortPIJSON As Boolean = False
        ''' <summary>
        ''' The Pseudocode In JSON (PI-JSON) for this Task.
        ''' </summary>
        <XmlElement>
        Public Property UseShortPIJSON As Boolean
            Get
                Return Me._UseShortPIJSON
            End Get
            Set(value As Boolean)
                Me._UseShortPIJSON = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _CompressionKey As String
        ''' <summary>
        ''' The Pseudocode In JSON (PI-JSON) for this Task.
        ''' </summary>
        <XmlElement>
        Public Property CompressionKey As String
            Get
                Return Me._CompressionKey
            End Get
            Set(value As String)
                Me._CompressionKey = value
            End Set
        End Property



        ''' <summary>
        ''' The next Step Line Number in the PIJSON that needs to be executed.
        ''' </summary>
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Public NextPIJSONLineNumber As String

        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Public CurrentPIJSONLineNumber As Integer

        ''' <summary>
        ''' Used for SAC injection in certain instances.
        ''' </summary>
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private ContextPrefix As String = Nothing

        ''' <summary>
        ''' The conversation flow with the user/cutomer. [Bot] [User]
        ''' </summary>
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Public ConversationFlow As String

        <XmlIgnore>
        <NonSerialized>
        Private _Template As String = ""
        ''' <summary>
        ''' The Template used to construct the NPU Context
        ''' </summary>
        <XmlElement>
        Public Property Template As String
            Get
                Return Me._Template
            End Get
            Set(value As String)
                Me._Template = value
            End Set
        End Property

        <JsonIgnore>
        <XmlArray("OpenAIFunctions"), XmlArrayItem("OpenAIFunction")>
        Public OpenAIFunctions As New List(Of OSM.OpenAIFunction)

        <JsonIgnore>
        <XmlArray("FEQLQueries"), XmlArrayItem("TaskFEQLQuery")>
        Public FEQLQuery As New List(Of OSM.TaskFEQLQuery)

        ''' <summary>
        ''' The last results returned by a User based FEQL query.
        ''' </summary>
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private LastFEQLQueryResults As String = Nothing

        ''' <summary>
        ''' When writing (UpSerting) to the database, the OSM may ask the user to wait.
        '''   This member is used to store that 'wait' chat message, and is checked so the same wait chat message is not repeated.
        ''' </summary>
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private LastPleaseWaitStatement As String = Nothing

#Region "OpenAI"

        ' Replace 'YOUR_API_KEY' with your actual API key from OpenAI
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private apiKey As String = My.Settings.FactEngineOpenAIAPIKey 'Azure: "8e287a42bbf94dabadac71755d57b05b"

        ' Set the API endpoint
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private apiUrl As String = "https://boston-openai.openai.azure.com/"

        <XmlIgnore>
        <NonSerialized>
        Private AzureKeyCredential As AzureKeyCredential

        ' Create the client
        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private openAIClient = New OpenAIClient(apiKey)  'Azure: New OpenAIClient(New Uri(apiUrl), New AzureKeyCredential(apiKey))

        <JsonIgnore>
        <XmlIgnore>
        <NonSerialized>
        Private mrChatCompletionsOptions As New ChatCompletionsOptions() With {
                .User = "Bot",
                .MaxTokens = 4000,
                .Functions = {},
                .Temperature = Single.Parse("0.2")
        }

        ''' <summary>
        ''' Used to (re)kickstart the Task if a stalemate (in conversation)/impasse has been reached.
        '''   Mostly because GPT4-Turbo doesn't know how to call multiple CreateTableInstance function calls at the same time and in sequence.
        '''   So, if limiting the Response.Choices of GPT4 to 1, the first CreateTableInstance function must kickstart itself to execute the rest of the insert into sequences.
        ''' </summary>
        <XmlIgnore>
        <NonSerialized>
        Public KickstartTimer As Timer

        ''' <summary>
        ''' The latest/last OSM Context generated.
        ''' </summary>
        <XmlIgnore>
        <NonSerialized>
        Public LatestOSMContext As String = ""

        ''' <summary>
        ''' The OSM Context has just been generated.
        ''' </summary>
        <NonSerialized>
        Public Event OSMContextGenerated()

#End Region
#End Region 'Other Properties

#End Region
        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            Me.KickstartTimer = New Timer
            AddHandler KickstartTimer.Tick, AddressOf KickstartTimer_Tick

            If Me.apiKey <> "" Then
                Me.AzureKeyCredential = New AzureKeyCredential(apiKey)
            End If

        End Sub

#Region "Function Loading (i.e. Functions that the LLM can call)"


        Private Function GetTaskOpenAIFunctions() As List(Of FunctionDefinition)

            Dim larOpenAIFunctionDefinition As New List(Of FunctionDefinition)

            Try
                For Each lrOpenAIFunction In Me.OpenAIFunctions

                    Dim options = New JsonDocumentOptions() With {
                                                                .AllowTrailingCommas = True
                                                                 }

                    Dim jsonDoc = JsonDocument.Parse(lrOpenAIFunction.parameters, options)
                    Dim fixedJsonString = jsonDoc.RootElement.GetRawText()

                    Dim loBinaryData = BinaryData.FromString(fixedJsonString)

                    larOpenAIFunctionDefinition.Add(New FunctionDefinition() With {
                                                                                    .Name = lrOpenAIFunction.name,
                                                                                    .Description = lrOpenAIFunction.description,
                                                                                    .Parameters = loBinaryData
                                                                                    }
                    )

                Next

                Return larOpenAIFunctionDefinition

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return larOpenAIFunctionDefinition
            End Try

        End Function

#End Region

#Region "Function Management (i.e. Processing LLM Function calls)"

        Private Async Function ExecuteOSMRunTask(ByVal asGPTResponse As String) As Threading.Tasks.Task(Of String)

            Try
                'Get the Arguments of the Function
                Dim jsonObject As JObject = JObject.Parse(asGPTResponse)

                Dim lsTaskName = jsonObject("task_name").ToString

                Dim lrTask = Me.Brain.LoadOSMTask(lsTaskName)

                Dim lsResponse As String = ""

                If lrTask Is Nothing Then
                    lsResponse = "Could not find the Task, " & lsTaskName & "."
                Else
                    lsResponse = Await lrTask.ProcessUserMessage(lrTask.GenerateOSMContext)
                End If


                Return lsResponse

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning)

                Return lsMessage
            End Try

        End Function


        ''' <summary>
        ''' Lists all the WorkingOnTasks of the Brain associated with (calling) this Task. I.e. What tasks are running.
        '''   Generally this would be, or should be, only called from the Topmost governing Task. I.e. Percy.
        ''' </summary>
        Private Sub ExecuteListBrainWorkingOnTasks()

            Try
                Dim lsHTML As String = ""

                If Me.TheBox Is Nothing Then
                    Me.TheBox = prApplication.MainForm.LoadOSMTheBox(Me.TheBox)
                End If

                lsHTML = "Tasks<br>"
                lsHTML.AppendLine("=====<br>")

                For Each lrTask In Me.Brain.WorkingOnTasks
                    lsHTML.AppendLine(lrTask.Name & "<br>")
                Next

                Me.TheBox.PopulateDocumentText(lsHTML)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Function ExecuteSendEmail(asGPTResponse As String) As String


            Try
                Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = Me.ModelId)

                If lrModel Is Nothing Or Me.ModelId Is Nothing Then

                    Return "No Model Found"

                Else
                    If Not lrModel.Loaded Then Call lrModel.Load(False)

                    'Get the Arguments of the Function
                    Dim jsonObject As JObject = JObject.Parse(asGPTResponse)

                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Sending an email.",,,, True, True, Color.Salmon)

                    ' If using Professional version, put your serial key below.
                    GemBox.Email.ComponentInfo.SetLicense("FREE-LIMITED-KEY")

                    Dim jsonString As String = jsonObject("recipient_mail_addresses").ToString
                    Dim emailList As List(Of String) = JsonConvert.DeserializeObject(Of List(Of String))(jsonString)

#Region "Get the SMTP Details"
                    Dim lrFEQLTokenType As FEQL.TokenType = Nothing
                    Dim lrFEQLParseTree As FEQL.ParseTree = Nothing

#Region "Query Language"
                    Dim liQueryLanguageToUse As pcenumDatabaseQueryLanguage = pcenumDatabaseQueryLanguage.SQL
                    liQueryLanguageToUse = lrModel.TargetDatabaseType.GetAttributeValue(Of DefaultQueryLanguageAttribute, pcenumDatabaseQueryLanguage)
#End Region

                    Dim lsQuery = "WHICH LegalEntity has (LegalEntity_Id:" & Me.UserId.ToString & ") AND has WHICH EmailAccount RETURN EmailAccount.EmailAddress, EmailAccount.SMTPClientAddress , EmailAccount.Username, EmailAccount.Password"

                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Running FEQL Query: " & lsQuery,,,, True, True, Color.Salmon)

                    '======================================================================
                    'Run the query
                    Dim lrFEQLProcessor = New FEQL.Processor(lrModel)
                    Dim lrRecordset = lrFEQLProcessor.ProcessFEQLStatement(lsQuery, lrFEQLTokenType, lrFEQLParseTree, liQueryLanguageToUse)
                    '======================================================================
#End Region

                    ' Create a new email message.
                    Dim message As New MailMessage(New MailAddress(lrRecordset("EmailAddress").Data))

                    For Each lsToEmailAddress In emailList
                        message.To.Add(New MailAddress(lsToEmailAddress))
                    Next

                    ' Add subject and body.
                    message.Subject = jsonObject("subject").ToString
                    message.BodyText = jsonObject("body_text").ToString

                    ' Create a new SMTP client and send the email message.
                    Using smtp As New GemBox.Email.Smtp.SmtpClient(lrRecordset("SMTPClientAddress").Data)
                        smtp.Connect()
                        smtp.Authenticate(lrRecordset("Username").Data, lrRecordset("Password").Data)
                        smtp.SendMessage(message)
                    End Using

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return lsMessage
            End Try

        End Function

        Public Function ExecuteTableStringColumnAppend(asGPTResponse As String) As String
            Try
                Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = Me.ModelId)

                If lrModel Is Nothing Or Me.ModelId Is Nothing Then

                    Return "No Model Found"

                Else
                    If Not lrModel.Loaded Then Call lrModel.Load(False)

                    'Get the Arguments of the Function
                    Dim jsonObject As JObject = JObject.Parse(asGPTResponse)

                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Uppending string column in table.",,,, True, True, Color.Salmon)

                    Call lrModel.DatabaseConnection.AppendStringColumn(jsonObject("table_name").ToString,
                                                                     jsonObject("column_name").ToString,
                                                                     jsonObject("append_string").ToString,
                                                                     jsonObject("json_where_clause").ToString,
                                                                     jsonObject("with_line_break").ToString)

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return lsMessage
            End Try

        End Function

        Public Function ExecuteDeleteTableDataInstance(asGPTResponse As String) As String

            Try
                Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = Me.ModelId)

                If lrModel Is Nothing Or Me.ModelId Is Nothing Then

                    Return "No Model Found"

                Else
                    If Not lrModel.Loaded Then Call lrModel.Load(False)

                    Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                    Dim ljsonTableInstance As String = jsonObject("table_instance_in_json").ToString

                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Deleting Data Instance: " & ljsonTableInstance,,,, True, True, Color.Salmon)

                    Call lrModel.DatabaseConnection.DeleteTableInstance(ljsonTableInstance)

                    Try
                        Call Me.SetLineStatusToCompleted(asGPTResponse, ljsonTableInstance)
                    Catch ex As Exception
                        'We tried
                    End Try

                    Try
                        Me.NextPIJSONLineNumber = jsonObject("next_step_line_number").ToString()
                    Catch ex As Exception
                        'We tried
                    End Try

                    Try
                        Dim lsPrivateThoughts As String = "Private thoughts: " & jsonObject("private_thoughts").ToString()
                        Me.ConversationFlow &= vbCrLf & lsPrivateThoughts
                        Me.ContextPrefix = "Private thoughts: " & lsPrivateThoughts
                        Me.ContextPrefix.AppendLine("If you've already removed the required Order Items don't repeat.")
                    Catch ex As Exception

                    End Try

                    Me.KickstartTimer.Interval = 200
                    Me.KickstartTimer.Enabled = True
                    Call Me.KickstartTimer.Start()


                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Return lsMessage
            End Try

        End Function

        Public Sub ExecuteDeleteNearAGIDBMemoryUnit(asGPTResponse As String)

            Try
                Dim lrNearAGIMemoryUnit As New NearAGI.MemoryUnit

                Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                Dim ljsonNearAGIMemoryUnit As String = jsonObject("memory_unit_instance_in_json").ToString

                ' Deserialize JSON string to MemoryUnit object
                lrNearAGIMemoryUnit = JsonConvert.DeserializeObject(Of NearAGI.MemoryUnit)(ljsonNearAGIMemoryUnit)

                'UpSert the NearAGI.MemoryUnit
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of NearAGI.MemoryUnit, Boolean)) = Function(t) t.Id = lrNearAGIMemoryUnit.Id
                lrDataStore.Delete(Of NearAGI.MemoryUnit)(whereClause)

                Dim lsPrivateThoughts = "Private thoughts: Just Deleted the Memory Unit: " & ljsonNearAGIMemoryUnit
                Me.ConversationFlow &= vbCrLf & lsPrivateThoughts
                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsPrivateThoughts,,,, True, True, Color.Salmon)

                Me.KickstartTimer.Interval = 200
                Me.KickstartTimer.Enabled = True
                Call Me.KickstartTimer.Start()



            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function ExecuteFindNearAGIDBMemoryUnits(asGPTResponse As String) As pcenumOSMFactEngineQueryResultStatus

            Try
                Dim liReturnCode As pcenumOSMFactEngineQueryResultStatus = pcenumOSMFactEngineQueryResultStatus.Fail
                Dim lsReturnString As String = ""
                Dim lrNearAGIMemoryUnit As New NearAGI.MemoryUnit

                Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                'Dim ljsonNearAGIMemoryUnit As String = jsonObject("memory_unit_instance_in_json").ToString

                Dim lsSearchTerm As String = jsonObject("search_string").ToString()

                'UpSert the NearAGI.MemoryUnit
                Dim lrDataStore As New DataStore.Store
                Dim larNearAGIMemoryUnit As List(Of NearAGI.MemoryUnit) = lrDataStore.Get(Of NearAGI.MemoryUnit)

                Dim larFoundMemoryUnits = (From NearAGIMemoryUnit In larNearAGIMemoryUnit
                                           Where NearAGIMemoryUnit.Data.LCase.Contains(lsSearchTerm.LCase)
                                           Select NearAGIMemoryUnit).OrderBy(Function(x) x.SequenceNr)

                Try
                    If larFoundMemoryUnits.Count = 0 Then
                        lsReturnString &= "No results returned. Move on gracefully with the conversation."
                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                        'If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("No results returned for query: " & lsQuery,,,, True, True, aoDebugColour)
                    Else
                        ' Create a new instance of StringBuilder
                        Dim stringBuilder As New System.Text.StringBuilder()

                        ' Use the StringBuilder to append text
                        For Each lrFoundMemoryUnit As NearAGI.MemoryUnit In larFoundMemoryUnits
                            stringBuilder.AppendLine(lrFoundMemoryUnit.ToString)
                        Next

                        ' Once all text has been appended, update the label's text in one operation
                        lsReturnString &= stringBuilder.ToString()

                        Me.Brain.send_data("Query successful: " & lsReturnString.Replace(vbCrLf, ""),,,, True, True, Color.MediumSlateBlue)

                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Success

                    End If

                Catch ex As Exception
                    Dim lsMessage As String
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsMessage,,,, True, True, Color.Salmon)

                    Return pcenumOSMFactEngineQueryResultStatus.Fail
                Finally

                    '=========================================================
                    'OSM has called the FEQL query itself

                    Me.LastFEQLQueryResults = "Database query results below:"
                    Me.LastFEQLQueryResults.AppendLine(liReturnCode.ToString)
                    Me.LastFEQLQueryResults.AppendLine(lsReturnString)

                    Select Case liReturnCode
                            Case Is = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                                Me.ContextPrefix = "Internal Thoughts: I have just run a query and got no results, I won't rerun the function for that query. Otherwise I'll go in circles."
                                Me.ContextPrefix.AppendLine("IMPORTANT: If you were expecting results, and you believe you inserted the data, re-upsert the data.")
                                Me.LastFEQLQueryResults.AppendLine("Internal Thoughts: ...I don't want to repeat the query, I'll steer the conversation away. If I inserted the data, I'll UpSert the data again.")
                            Case Is = pcenumOSMFactEngineQueryResultStatus.Success
                                Me.ContextPrefix = "My/your query was successful...Check the conversation transcript. If you are repeating yourself, ask a different query."
                                'Me.ContextPrefix.AppendLine("Just ran query: " & lsQuery)
                                Me.LastFEQLQueryResults.AppendLine("...answer the user based on this")
                            Case Else
                                Me.LastFEQLQueryResults.AppendLine("...answer the user based on this")
                        End Select
                    Me.ThatVariable = Me.LastFEQLQueryResults
                    '=========================================================
                End Try

                Return liReturnCode

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Return lsMessage
            End Try

        End Function


        Public Function ExecuteUpSertNearAGIDBMemoryUnit(asGPTResponse As String) As String

            Try
                Dim lrNearAGIMemoryUnit As New NearAGI.MemoryUnit

                Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                Dim ljsonNearAGIMemoryUnit As String = jsonObject("memory_unit_instance_in_json").ToString

                ' Deserialize JSON string to MemoryUnit object
                lrNearAGIMemoryUnit = JsonConvert.DeserializeObject(Of NearAGI.MemoryUnit)(ljsonNearAGIMemoryUnit)

                'UpSert the NearAGI.MemoryUnit
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of NearAGI.MemoryUnit, Boolean)) = Function(t) t.Id = lrNearAGIMemoryUnit.Id
                lrDataStore.Upsert(lrNearAGIMemoryUnit, whereClause)

                Dim lsPrivateThoughts = "Private thoughts: Just UpSerted the following to Near AGI Database: " & ljsonNearAGIMemoryUnit
                Me.ConversationFlow &= vbCrLf & lsPrivateThoughts
                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsPrivateThoughts,,,, True, True, Color.Salmon)

                Me.KickstartTimer.Interval = 200
                Me.KickstartTimer.Enabled = True
                Call Me.KickstartTimer.Start()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Return lsMessage
            End Try

        End Function
        Public Function ExecuteUpSertTableDataInstance(asGPTResponse As String) As String

            Try
                Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = Me.ModelId)

                If lrModel Is Nothing Or Me.ModelId Is Nothing Then

                    Return "No Model Found"

                Else
                    If Not lrModel.Loaded Then Call lrModel.Load(False)

                    Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                    Dim ljsonTableInstance As String = jsonObject("table_instance_in_json").ToString

                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("UpSerting Data Instance: " & ljsonTableInstance,,,, True, True, Color.Salmon)

                    Dim lsErrorMessage As String = Nothing
                    Call lrModel.DatabaseConnection.CreateTableInstance(ljsonTableInstance, lsErrorMessage)

                    Try
                        Call Me.SetLineStatusToCompleted(asGPTResponse, ljsonTableInstance)
                    Catch ex As Exception
                        'We tried
                    End Try

                    Try
                        Me.NextPIJSONLineNumber = jsonObject("next_step_line_number").ToString()
                    Catch ex As Exception
                        'We tried
                    End Try

                    Try
                        Dim lsPrivateThoughts = jsonObject("private_thoughts").ToString()
                        Me.ConversationFlow &= vbCrLf & "Private thoughts: " & lsPrivateThoughts
                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsPrivateThoughts,,,, True, True, Color.Salmon)
                    Catch ex As Exception

                    End Try

                    Dim lbKickStart As Boolean = False
                    Try
                        lbKickStart = jsonObject("kickstart")
                    Catch ex As Exception
                        'We tried
                    End Try

                    If lbKickStart Then
                        Me.KickstartTimer.Interval = 200
                        Me.KickstartTimer.Enabled = True
                        Call Me.KickstartTimer.Start()
                    End If

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Return lsMessage
            End Try

        End Function

        Private Function ExecuteCloseTask()

            Try
                Me.TaskStatus = pcenumOSMTaskStatus.Closed

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Private Function ExecuteFEQL(Optional asFEQLQuery As String = Nothing,
                                     Optional ByRef aoDebugColour As Color? = Nothing) As String

            Dim lsReturnString As String = ""
            Dim liReturnCode As pcenumOSMFactEngineQueryResultStatus = pcenumOSMFactEngineQueryResultStatus.Fail
            Dim lsQuery As String = "<Error>"

            Try
                Me.ContextPrefix = Nothing

                If aoDebugColour Is Nothing Then
                    aoDebugColour = Color.Salmon
                End If

                Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = Me.ModelId)

                If lrModel Is Nothing Then
                    lrModel = New FBM.Model
                    lrModel.ModelId = Me.ModelId
                    TableModel.GetModelDetails(lrModel)
                    Call lrModel.Load()
                    prApplication.Models.AddUnique(lrModel)
                End If

                If lrModel Is Nothing Or Me.ModelId Is Nothing Then

                    liReturnCode = pcenumOSMFactEngineQueryResultStatus.NoModelFound

                Else
                    If Not lrModel.Loaded Then Call lrModel.Load(False)
                    Dim lrFEQLTokenType As FEQL.TokenType = Nothing
                    Dim lrFEQLParseTree As FEQL.ParseTree = Nothing

                    '====================================================================
                    'Run the query
#Region "Query Language"
                    Dim liQueryLanguageToUse As pcenumDatabaseQueryLanguage = pcenumDatabaseQueryLanguage.SQL
                    liQueryLanguageToUse = lrModel.TargetDatabaseType.GetAttributeValue(Of DefaultQueryLanguageAttribute, pcenumDatabaseQueryLanguage)
#End Region

                    lsQuery = asFEQLQuery

                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Running FEQL Query: " & lsQuery,,,, True, True, aoDebugColour)

                    '======================================================================
                    'Run the query
                    Dim lrFEQLProcessor = New FEQL.Processor(lrModel)
                    Dim lrRecordset = lrFEQLProcessor.ProcessFEQLStatement(lsQuery, lrFEQLTokenType, lrFEQLParseTree, liQueryLanguageToUse)
                    '======================================================================

                    Dim lsErrorMessage As String

                    If lrRecordset.ErrorReturned Then
                        lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, Color.Salmon)
                        lsReturnString.AppendString(lsErrorMessage)
                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                    Else
                        If lrRecordset.Query IsNot Nothing Then
                            lsReturnString.AppendString(lrRecordset.Query)
                        End If

                        If lrRecordset.ErrorString IsNot Nothing Then
#Region "Error"
                            If lrRecordset.ApplicationException IsNot Nothing Then
                                'Put code here to engage the VirtualAnalyst to create a FactTypeReading
                                '2021-VM-Use the following type of code for adding new FactTypeReadings
                                If lrRecordset.ApplicationException.Data.Contains("QueryEdgeGetFBMFactTypeFail") Then
                                    Dim lrQueryEdge As FactEngine.QueryEdge
                                    lrQueryEdge = lrRecordset.ApplicationException.Data.Item("QueryEdgeGetFBMFactTypeFail")

                                    Dim larModelObject As New List(Of FBM.ModelObject)

                                    If lrQueryEdge.BaseNode Is Nothing Or lrQueryEdge.TargetNode Is Nothing Then
                                        lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, Color.Salmon)
                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                                    End If
                                Else
                                    lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, Color.Salmon)
                                    liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                                End If
                            Else
                                lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, Color.Salmon)
                                liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                            End If
#End Region
                        Else
                            Select Case lrRecordset.StatementType
                                Case Is = FactEngine.pcenumFEQLStatementType.DESCRIBEStatement
                                'Call Me.DesbribeModelElement(lrRecordset.ModelElement)
                                'For now
                                Case Is = FactEngine.pcenumFEQLStatementType.SHOWStatement
                                    'Call Me.ShowModelElement(lrRecordset.ModelElement)
                                    'For now
                                Case Else
#Region "Query Results"
                                    lsReturnString = ""

                                    If lrRecordset.StatementType = FactEngine.pcenumFEQLStatementType.DIDStatement Then
                                        If lrRecordset.Facts.Count = 0 Then

                                            lsReturnString &= "I don't know." & vbCrLf & vbCrLf
                                        Else
                                            lsReturnString &= "Yes." & vbCrLf & vbCrLf
                                        End If
                                    End If

                                    If lrRecordset.Facts.Count = 0 Then
                                        lsReturnString &= "No results returned. Move on gracefully with the conversation."
                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("No results returned for query: " & lsQuery,,,, True, True, aoDebugColour)
                                    Else
                                        lsReturnString &= ""

                                        If lrRecordset.Warning.Count > 0 Then
                                            For Each lsWarning In lrRecordset.Warning
                                                lsReturnString &= lsWarning & vbCrLf
                                            Next
                                            lsReturnString &= vbCrLf
                                        End If

                                        Dim liInd = 0
                                        For Each lsColumnName In lrRecordset.ColumnNames
                                            liInd += 1
                                            lsReturnString &= " " & lsColumnName & " "
                                            If liInd < lrRecordset.Columns.Count Then lsReturnString &= ","
                                        Next
                                        lsReturnString &= vbCrLf & "=======================================" & vbCrLf


                                        ' Create a new instance of StringBuilder
                                        Dim stringBuilder As New System.Text.StringBuilder()

                                        ' Use the StringBuilder to append text
                                        For Each lrFact As FBM.Fact In lrRecordset.Facts
                                            If lrFact.GetType = GetType(ORMQL.PathFact) Then
                                                stringBuilder.AppendLine("")
                                                liInd = 0
                                                For Each lrFactData As FBM.FactData In lrFact.Data
                                                    If liInd > 0 Then stringBuilder.Append(", ")
                                                    stringBuilder.Append(lrFactData.Role.Id)
                                                    liInd += 1
                                                Next
                                                stringBuilder.AppendLine(vbCrLf & "=======================================")
                                            End If
                                            stringBuilder.AppendLine(lrFact.EnumerateAsBracketedFact(True))
                                        Next

                                        ' Once all text has been appended, update the label's text in one operation
                                        lsReturnString &= stringBuilder.ToString()
                                        If Not aoDebugColour = Color.MediumBlue Then
                                            If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Query successful: " & lsReturnString.Replace(vbCrLf, ""),,,, True, True, aoDebugColour)
                                        End If

                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Success
                                    End If 'Processing Results
#End Region
                            End Select
                        End If

                    End If 'lrRecordset IsNot Nothing

                End If 'Model found

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsMessage,,,, True, True, Color.Salmon)

                Return pcenumOSMFactEngineQueryResultStatus.Fail
            Finally

                '=========================================================
                'OSM has called the FEQL query itself
                If asFEQLQuery Is Nothing Then

                    Me.LastFEQLQueryResults = "Database query results below:"
                    Me.LastFEQLQueryResults.AppendLine(liReturnCode.ToString)
                    Me.LastFEQLQueryResults.AppendLine(lsReturnString)
                    Select Case liReturnCode
                        Case Is = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                            Me.ContextPrefix = "Internal Thoughts: I have just run a query and got no results, I won't rerun the function for that query. Otherwise I'll go in circles."
                            Me.ContextPrefix.AppendLine("IMPORTANT: If you were expecting results, and you believe you inserted the data, re-upsert the data.")
                            Me.LastFEQLQueryResults.AppendLine("Internal Thoughts: ...I don't want to repeat the query, I'll steer the conversation away. If I inserted the data, I'll UpSert the data again.")
                        Case Is = pcenumOSMFactEngineQueryResultStatus.Success
                            Me.ContextPrefix = "My/your query was successful...Check the conversation transcript. If you are repeating yourself, ask a different query."
                            Me.ContextPrefix.AppendLine("Just ran query: " & lsQuery)
                            Me.LastFEQLQueryResults.AppendLine("...answer the user based on this")
                        Case Else
                            Me.LastFEQLQueryResults.AppendLine("...answer the user based on this")
                    End Select
                    Me.ThatVariable = Me.LastFEQLQueryResults
                End If
                '=========================================================
            End Try

            Return lsReturnString

        End Function

        Private Async Function ExecuteTheBoxOpenWebPage(asGPTResponse As String) As Threading.Tasks.Task(Of String)

            Try

                Dim lsURL As String = ""

                Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                lsURL = jsonObject("url").ToString()

                If Me.TheBox Is Nothing Then
                    Me.TheBox = prApplication.MainForm.LoadOSMTheBox(Me.TheBox)
                End If


                Me.TheBox.OpenURL(lsURL)

                Return ""

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning)

                Return lsMessage
            End Try

        End Function


        Private Async Function ExecuteTheBoxPopulateTheBox(asGPTResponse As String) As Threading.Tasks.Task(Of String)

            Dim lsDummyString As String = ""
            Try
                Dim lsHTML As String = ""

                Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                lsHTML = jsonObject("html_for_the_box").ToString()

                If Me.TheBox Is Nothing Then
                    Me.TheBox = prApplication.MainForm.LoadOSMTheBox(Me.TheBox)
                End If

                Try
                    If jsonObject("use_that_variable") Then
                        'Set in FEQL query...Me.ThatVariable 
                        Me.ThatVariable = lsHTML
                    End If
                Catch ex As Exception

                End Try


                Me.TheBox.PopulateDocumentText(lsHTML)

                Try
                    Me.NextPIJSONLineNumber = jsonObject("next_step_line_number").ToString
                Catch ex As Exception

                End Try

                Try
                    Call Me.SetLineStatusToCompleted(asGPTResponse)
                Catch ex As Exception
                    'We tried
                End Try

                If Me.NextPIJSONLineNumber.Trim <> "" Then
                    Me.ContextPrefix = "Go To the Next PI-JSON statement. Line Number: " & Me.NextPIJSONLineNumber
                End If

                'Me.KickstartTimer.Interval = 200
                'Me.KickstartTimer.Enabled = True
                'Call Me.KickstartTimer.Start()

                ' Load the temporary file URL into the CefSharp browser
                'Me.TheBox.Browser.Navigate("file://" & tempFileName)                

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

            Return "Complete"

        End Function



        Private Function ExecuteFEQL(asGPTResponse As String,
                                     Optional asFEQLQuery As String = Nothing,
                                     Optional ByRef aoDebugColour As Color? = Nothing) As pcenumOSMFactEngineQueryResultStatus

            Dim lsReturnString As String = ""
            Dim liReturnCode As pcenumOSMFactEngineQueryResultStatus = pcenumOSMFactEngineQueryResultStatus.Fail
            Dim lsQuery As String

            Try
                If aoDebugColour Is Nothing Then
                    aoDebugColour = Color.Salmon
                End If

                Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = Me.ModelId)

                If lrModel Is Nothing Then
                    lrModel = New FBM.Model
                    lrModel.ModelId = Me.ModelId
                    TableModel.GetModelDetails(lrModel)
                    Call lrModel.Load()
                    prApplication.Models.AddUnique(lrModel)
                End If

                If lrModel Is Nothing Or Me.ModelId Is Nothing Then

                    liReturnCode = pcenumOSMFactEngineQueryResultStatus.NoModelFound

                Else
                    If Not lrModel.Loaded Then Call lrModel.Load(False)
                    Dim lrFEQLTokenType As FEQL.TokenType = Nothing
                    Dim lrFEQLParseTree As FEQL.ParseTree = Nothing

                    '====================================================================
                    'Run the query
#Region "Query Language"
                    Dim liQueryLanguageToUse As pcenumDatabaseQueryLanguage = pcenumDatabaseQueryLanguage.SQL
                    liQueryLanguageToUse = lrModel.TargetDatabaseType.GetAttributeValue(Of DefaultQueryLanguageAttribute, pcenumDatabaseQueryLanguage)
#End Region
                    If asFEQLQuery Is Nothing Then
                        'Parse the JSON string into a JObject &
                        '  Extract the FactEngine Query Language (FEQL) Query
                        Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                        lsQuery = jsonObject("feql_query").ToString()
                    Else
                        lsQuery = asFEQLQuery
                    End If

                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Running FEQL Query: " & lsQuery,,,, True, True, aoDebugColour)

                    '======================================================================
                    'Run the query
                    Dim lrFEQLProcessor = New FEQL.Processor(lrModel)
                    Dim lrRecordset = lrFEQLProcessor.ProcessFEQLStatement(lsQuery, lrFEQLTokenType, lrFEQLParseTree, liQueryLanguageToUse)
                    '======================================================================

                    Dim lsErrorMessage As String

                    If lrRecordset.ErrorReturned Then
                        lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, aoDebugColour)
                        lsReturnString.AppendString(lsErrorMessage)
                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                    Else
                        If lrRecordset.Query IsNot Nothing Then
                            lsReturnString.AppendString(lrRecordset.Query)
                        End If

                        If lrRecordset.ErrorString IsNot Nothing Then
#Region "Error"
                            If lrRecordset.ApplicationException IsNot Nothing Then
                                'Put code here to engage the VirtualAnalyst to create a FactTypeReading
                                '2021-VM-Use the following type of code for adding new FactTypeReadings
                                If lrRecordset.ApplicationException.Data.Contains("QueryEdgeGetFBMFactTypeFail") Then
                                    Dim lrQueryEdge As FactEngine.QueryEdge
                                    lrQueryEdge = lrRecordset.ApplicationException.Data.Item("QueryEdgeGetFBMFactTypeFail")

                                    Dim larModelObject As New List(Of FBM.ModelObject)

                                    If lrQueryEdge.BaseNode Is Nothing Or lrQueryEdge.TargetNode Is Nothing Then
                                        lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, aoDebugColour)
                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                                    End If
                                Else
                                    lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, aoDebugColour)
                                    liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                                End If
                            Else
                                lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, aoDebugColour)
                                liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                            End If
#End Region
                        Else
                            Select Case lrRecordset.StatementType
                                Case Is = FactEngine.pcenumFEQLStatementType.DESCRIBEStatement
                                'Call Me.DesbribeModelElement(lrRecordset.ModelElement)
                                'For now
                                Case Is = FactEngine.pcenumFEQLStatementType.SHOWStatement
                                    'Call Me.ShowModelElement(lrRecordset.ModelElement)
                                    'For now
                                Case Else
#Region "Query Results"
                                    lsReturnString = ""

                                    If lrRecordset.StatementType = FactEngine.pcenumFEQLStatementType.DIDStatement Then
                                        If lrRecordset.Facts.Count = 0 Then

                                            lsReturnString &= "I don't know." & vbCrLf & vbCrLf
                                        Else
                                            lsReturnString &= "Yes." & vbCrLf & vbCrLf
                                        End If
                                    End If

                                    If lrRecordset.Facts.Count = 0 Then
                                        lsReturnString &= "No results returned. Move on gracefully with the conversation."
                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("No results returned for query: " & lsQuery,,,, True, True, aoDebugColour)
                                    Else
                                        lsReturnString &= ""

                                        If lrRecordset.Warning.Count > 0 Then
                                            For Each lsWarning In lrRecordset.Warning
                                                lsReturnString &= lsWarning & vbCrLf
                                            Next
                                            lsReturnString &= vbCrLf
                                        End If

                                        Dim liInd = 0
                                        For Each lsColumnName In lrRecordset.ColumnNames
                                            liInd += 1
                                            lsReturnString &= " " & lsColumnName & " "
                                            If liInd < lrRecordset.Columns.Count Then lsReturnString &= ","
                                        Next
                                        lsReturnString &= vbCrLf & "=======================================" & vbCrLf


                                        ' Create a new instance of StringBuilder
                                        Dim stringBuilder As New System.Text.StringBuilder()

                                        ' Use the StringBuilder to append text
                                        For Each lrFact As FBM.Fact In lrRecordset.Facts
                                            If lrFact.GetType = GetType(ORMQL.PathFact) Then
                                                stringBuilder.AppendLine("")
                                                liInd = 0
                                                For Each lrFactData As FBM.FactData In lrFact.Data
                                                    If liInd > 0 Then stringBuilder.Append(", ")
                                                    stringBuilder.Append(lrFactData.Role.Id)
                                                    liInd += 1
                                                Next
                                                stringBuilder.AppendLine(vbCrLf & "=======================================")
                                            End If
                                            stringBuilder.AppendLine(lrFact.EnumerateAsBracketedFact(True))
                                        Next

                                        ' Once all text has been appended, update the label's text in one operation
                                        lsReturnString &= stringBuilder.ToString()
                                        If Not aoDebugColour = Color.MediumBlue Then
                                            If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Query successful: " & lsReturnString.Replace(vbCrLf, ""),,,, True, True, aoDebugColour)
                                        End If

                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Success
                                    End If 'Processing Results
#End Region
                            End Select
                        End If

                    End If 'lrRecordset IsNot Nothing

                End If 'Model found

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsMessage,,,, True, True, Color.Salmon)

                Return pcenumOSMFactEngineQueryResultStatus.Fail
            Finally
#Region "Finally"
                '=========================================================
                'OSM has called the FEQL query itself
                If asFEQLQuery Is Nothing Then

                    Me.LastFEQLQueryResults = "Database query results below:"
                    Me.LastFEQLQueryResults.AppendLine(liReturnCode.ToString)
                    Me.LastFEQLQueryResults.AppendLine(lsReturnString)
                    Select Case liReturnCode
                        Case Is = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                            Me.ContextPrefix = "Internal Thoughts: I have just run a query and got not results, don't rerun the function for that query. Otherwise I'll go in circles."
                            Me.ContextPrefix.AppendLine("If you were expecting results, check that you actually inserted the data, or re-upsert it.")
                            Me.LastFEQLQueryResults.AppendLine("Internal Thoughts: ...I don't want to repeat the query, I'll steer the conversation away.")
                        Case Is = pcenumOSMFactEngineQueryResultStatus.Success
                            Me.ContextPrefix = "My/your query was successful...Check the conversation transcript. If you are repeating yourself, ask a different query."
                            Me.ContextPrefix.AppendLine("Just ran query: " & lsQuery)
                            Me.LastFEQLQueryResults.AppendLine("...answer the user based on this")
                        Case Else
                            Me.LastFEQLQueryResults.AppendLine("...answer the user based on this")
                    End Select
                    Me.ThatVariable = Me.LastFEQLQueryResults
                End If
                '=========================================================
#End Region
            End Try

            Return liReturnCode

        End Function

        Private Function ExecuteFEQLNaturalLanguageQuery(asGPTResponse As String,
                                                         Optional ByRef aoDebugColour As Color? = Nothing) As pcenumOSMFactEngineQueryResultStatus

            Dim lsReturnString As String = ""
            Dim liReturnCode As pcenumOSMFactEngineQueryResultStatus = pcenumOSMFactEngineQueryResultStatus.Fail
            Dim lsQuery As String

            Try
                If aoDebugColour Is Nothing Then
                    aoDebugColour = Color.Salmon
                End If

                Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = Me.ModelId)

                If lrModel Is Nothing Then
                    lrModel = New FBM.Model
                    lrModel.ModelId = Me.ModelId
                    TableModel.GetModelDetails(lrModel)
                    Call lrModel.Load()
                    prApplication.Models.AddUnique(lrModel)
                End If

                If lrModel Is Nothing Or Me.ModelId Is Nothing Then

                    liReturnCode = pcenumOSMFactEngineQueryResultStatus.NoModelFound

                Else
                    If Not lrModel.Loaded Then Call lrModel.Load(False)
                    Dim lrFEQLTokenType As FEQL.TokenType = Nothing
                    Dim lrFEQLParseTree As FEQL.ParseTree = Nothing

                    '====================================================================
                    'Run the query
#Region "Query Language"
                    Dim liQueryLanguageToUse As pcenumDatabaseQueryLanguage = pcenumDatabaseQueryLanguage.SQL
                    liQueryLanguageToUse = lrModel.TargetDatabaseType.GetAttributeValue(Of DefaultQueryLanguageAttribute, pcenumDatabaseQueryLanguage)
#End Region

                    Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                    lsQuery = jsonObject("natural_language_query").ToString()


                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Running Natural Language Query: " & lsQuery,,,, True, True, aoDebugColour)

                    '======================================================================
                    'Run the query
                    Dim lrFEQLProcessor = New FEQL.Processor(lrModel)
                    Dim lrRecordset = lrFEQLProcessor.ProcessNaturalLanguageQuery(lsQuery, lrFEQLTokenType, lrFEQLParseTree, liQueryLanguageToUse, Me.ModelId)
                    '======================================================================

                    'Regardless of what happened leave private thoughts
                    Dim lsPrivateThoughts As String = jsonObject("private_thoughts").ToString()
                    lsPrivateThoughts.AppendLine("Natural Language Query Run: " & lsQuery)
                    Me.ConversationFlow.AppendLine("You/Bot: Private Thoughts: " & lsPrivateThoughts)

                    Dim lsErrorMessage As String

                    If lrRecordset.ErrorReturned Then
                        lsErrorMessage = "Error running query: " & lrRecordset.FEQLQuery
                        lsErrorMessage.AppendLine(lrRecordset.ErrorString)
                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, aoDebugColour)
                        lsReturnString.AppendString(lsErrorMessage)
                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                    Else
                        If lrRecordset.Query IsNot Nothing Then
                            lsReturnString.AppendString(lrRecordset.Query)
                            Me.Brain.send_data("FEQL Query Returned: " & lrRecordset.Query,,,, True, True, aoDebugColour)
                        End If

                        If lrRecordset.ErrorString IsNot Nothing Then
#Region "Error"
                            If lrRecordset.ApplicationException IsNot Nothing Then
                                'Put code here to engage the VirtualAnalyst to create a FactTypeReading
                                '2021-VM-Use the following type of code for adding new FactTypeReadings
                                If lrRecordset.ApplicationException.Data.Contains("QueryEdgeGetFBMFactTypeFail") Then
                                    Dim lrQueryEdge As FactEngine.QueryEdge
                                    lrQueryEdge = lrRecordset.ApplicationException.Data.Item("QueryEdgeGetFBMFactTypeFail")

                                    Dim larModelObject As New List(Of FBM.ModelObject)

                                    If lrQueryEdge.BaseNode Is Nothing Or lrQueryEdge.TargetNode Is Nothing Then
                                        lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, aoDebugColour)
                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                                    End If
                                Else
                                    lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                    If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, aoDebugColour)
                                    liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                                End If
                            Else
                                lsErrorMessage = "Error running query: " & lrRecordset.ErrorString
                                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsErrorMessage,,,, True, True, aoDebugColour)
                                liReturnCode = pcenumOSMFactEngineQueryResultStatus.Fail
                            End If
#End Region
                        Else
                            Select Case lrRecordset.StatementType
                                Case Is = FactEngine.pcenumFEQLStatementType.DESCRIBEStatement
                                'Call Me.DesbribeModelElement(lrRecordset.ModelElement)
                                'For now
                                Case Is = FactEngine.pcenumFEQLStatementType.SHOWStatement
                                    'Call Me.ShowModelElement(lrRecordset.ModelElement)
                                    'For now
                                Case Else
#Region "Query Results"
                                    lsReturnString = ""

                                    If lrRecordset.StatementType = FactEngine.pcenumFEQLStatementType.DIDStatement Then
                                        If lrRecordset.Facts.Count = 0 Then

                                            lsReturnString &= "I don't know." & vbCrLf & vbCrLf
                                        Else
                                            lsReturnString &= "Yes." & vbCrLf & vbCrLf
                                        End If
                                    End If

                                    If lrRecordset.Facts.Count = 0 Then
                                        lsReturnString &= "No results returned. Move on gracefully with the conversation."
                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("No results returned for query: " & lsQuery,,,, True, True, aoDebugColour)
                                    Else
                                        lsReturnString &= ""

                                        If lrRecordset.Warning.Count > 0 Then
                                            For Each lsWarning In lrRecordset.Warning
                                                lsReturnString &= lsWarning & vbCrLf
                                            Next
                                            lsReturnString &= vbCrLf
                                        End If

                                        Dim liInd = 0
                                        For Each lsColumnName In lrRecordset.ColumnNames
                                            liInd += 1
                                            lsReturnString &= " " & lsColumnName & " "
                                            If liInd < lrRecordset.Columns.Count Then lsReturnString &= ","
                                        Next
                                        lsReturnString &= vbCrLf & "=======================================" & vbCrLf


                                        ' Create a new instance of StringBuilder
                                        Dim stringBuilder As New System.Text.StringBuilder()

                                        ' Use the StringBuilder to append text
                                        For Each lrFact As FBM.Fact In lrRecordset.Facts
                                            If lrFact.GetType = GetType(ORMQL.PathFact) Then
                                                stringBuilder.AppendLine("")
                                                liInd = 0
                                                For Each lrFactData As FBM.FactData In lrFact.Data
                                                    If liInd > 0 Then stringBuilder.Append(", ")
                                                    stringBuilder.Append(lrFactData.Role.Id)
                                                    liInd += 1
                                                Next
                                                stringBuilder.AppendLine(vbCrLf & "=======================================")
                                            End If
                                            stringBuilder.AppendLine(lrFact.EnumerateAsBracketedFact(True))
                                        Next

                                        ' Once all text has been appended, update the label's text in one operation
                                        lsReturnString &= stringBuilder.ToString()
                                        If Not aoDebugColour = Color.MediumBlue Then
                                            If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Query successful: " & lsReturnString.Replace(vbCrLf, ""),,,, True, True, aoDebugColour)
                                        End If

                                        liReturnCode = pcenumOSMFactEngineQueryResultStatus.Success
                                    End If 'Processing Results
#End Region
                            End Select
                        End If

                    End If 'lrRecordset IsNot Nothing

                End If 'Model found

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsMessage,,,, True, True, Color.Salmon)

                Return pcenumOSMFactEngineQueryResultStatus.Fail
            Finally
#Region "Finally"
                Me.LastFEQLQueryResults = "Database query results below:"
                Me.LastFEQLQueryResults.AppendLine(liReturnCode.ToString)
                Me.LastFEQLQueryResults.AppendLine(lsReturnString)
                Select Case liReturnCode
                    Case Is = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                        Me.ContextPrefix = "Internal Thoughts: I have just run a query and got not results, don't rerun the function for that query. Otherwise I'll go in circles."
                        Me.ContextPrefix.AppendLine("If you were expecting results, check that you actually inserted the data, or re-upsert it.")
                        Me.LastFEQLQueryResults.AppendLine("Internal Thoughts: ...I don't want to repeat the query, I'll steer the conversation away.")
                    Case Is = pcenumOSMFactEngineQueryResultStatus.Success
                        Me.ContextPrefix = "My/your query was successful...Check the conversation transcript. If you are repeating yourself, ask a different query."
                        Me.ContextPrefix.AppendLine("Just ran query: " & lsQuery)
                        Me.LastFEQLQueryResults.AppendLine("...answer the user based on this")
                    Case Else
                        Me.LastFEQLQueryResults.AppendLine("...answer the user based on this")
                End Select
                Me.ThatVariable = Me.LastFEQLQueryResults
                '=========================================================
#End Region
            End Try

            Return liReturnCode

        End Function

        Private Sub SetLineStatusToCompleted(asGPTResponse As String, Optional ajsonTableInstance As String = Nothing)

            Try
                Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
                Dim liLineNumber As String = jsonObject("line_number_to_close").ToString()

                If liLineNumber = 0 Then
                    liLineNumber = jsonObject("line_number_processing").ToString()
                End If

                ' Deserialize the JSON string into a JObject
                Dim lineStatuses As JObject
                Dim lsCompleted As String = "Completed"
                If Me.UseShortPIJSON Then
                    lineStatuses = JObject.Parse(Me.TaskShortPIJSON)
                    lsCompleted = "CP"
                Else
                    lineStatuses = JObject.Parse(Me.TaskPIJSON)
                End If

                ' Use LINQ to find the specific line number, regardless of its nesting level
                Dim targetLine = lineStatuses.DescendantsAndSelf().
                                    OfType(Of JProperty)().
                                    Where(Function(prop) prop.Name = liLineNumber AndAlso TypeOf prop.Value Is JObject).
                                    Select(Function(prop) DirectCast(prop.Value, JObject)).
                                    FirstOrDefault()

                If targetLine IsNot Nothing Then
                    ' Update the status of the found line
                    If Me.UseShortPIJSON Then
                        targetLine("S") = lsCompleted
                    Else
                        targetLine("status") = lsCompleted
                    End If


                    If ajsonTableInstance IsNot Nothing Then
                        Try
                            If targetLine("table_instances_already_created") Is Nothing Then
                                targetLine("table_instances_already_created") = ajsonTableInstance
                            Else
                                targetLine("table_instances_already_created") = targetLine("table_instances_already_created").ToString & ajsonTableInstance
                            End If

                        Catch ex As Exception
                            Dim lsMessage = "No table_instances_already_created property for Line Number: " & liLineNumber.ToString
                            If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsMessage,,,, True, True, Color.Salmon)
                        End Try

                        Try
                            If targetLine("private_thoughts") Is Nothing Then
                                targetLine("private_thoughts") = jsonObject("private_thoughts").ToString()
                            Else
                                targetLine("private_thoughts") = targetLine("private_thoughts").ToString & "|" & jsonObject("private_thoughts").ToString()
                            End If
                        Catch ex As Exception
                            Dim lsMessage = "No private_thoughts property for Line Number: " & liLineNumber.ToString
                            If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data(lsMessage,,,, True, True, Color.Salmon)
                        End Try
                    End If
                Else
                    Throw New KeyNotFoundException($"Line number {liLineNumber} not found in JSON data.")
                End If

                ' Serialize the updated JSON object back into a JSON string
                If Me.UseShortPIJSON Then
                    Me.TaskShortPIJSON = JsonConvert.SerializeObject(lineStatuses, Newtonsoft.Json.Formatting.Indented)
                Else
                    Me.TaskPIJSON = JsonConvert.SerializeObject(lineStatuses, Newtonsoft.Json.Formatting.Indented)
                End If


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, abUseFlashCard:=True)
            End Try

        End Sub


        Private Sub SetLineStatusToCompleted(ByVal aiLineNumberToComplete As Integer)

            Try
                Dim liLineNumber As String = aiLineNumberToComplete

                ' Deserialize the JSON string into a JObject
                Dim lineStatuses As JObject = JObject.Parse(Me.TaskPIJSON)

                ' Use LINQ to find the specific line number, regardless of its nesting level
                Dim targetLine = lineStatuses.DescendantsAndSelf().
                                    OfType(Of JProperty)().
                                    Where(Function(prop) prop.Name = liLineNumber AndAlso TypeOf prop.Value Is JObject).
                                    Select(Function(prop) DirectCast(prop.Value, JObject)).
                                    FirstOrDefault()

                If targetLine IsNot Nothing Then
                    ' Update the status of the found line
                    targetLine("status") = "Completed"
                Else
                    Throw New KeyNotFoundException($"Line number {liLineNumber} not found in JSON data.")
                End If

                ' Serialize the updated JSON object back into a JSON string
                Me.TaskPIJSON = JsonConvert.SerializeObject(lineStatuses, Newtonsoft.Json.Formatting.Indented)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Private Sub TouchLineStatus(asGPTResponse As String, Optional ajsonTableInstance As String = Nothing)

            Dim jsonObject As JObject = JObject.Parse(asGPTResponse)
            Dim liLineNumber As String = jsonObject("task_line_number").ToString()

            ' Deserialize the JSON string into a JObject
            Dim lineStatuses As JObject = JObject.Parse(Me.TaskPIJSON)

            ' Use LINQ to find the specific line number, regardless of its nesting level
            Dim targetLine = lineStatuses.DescendantsAndSelf().
                                OfType(Of JProperty)().
                                Where(Function(prop) prop.Name = liLineNumber AndAlso TypeOf prop.Value Is JObject).
                                Select(Function(prop) DirectCast(prop.Value, JObject)).
                                FirstOrDefault()

            If targetLine IsNot Nothing Then
                ' Update the status of the found line
                If ajsonTableInstance IsNot Nothing Then
                    Try
                        targetLine("table_instances_already_created") = targetLine("table_instances_already_created").ToString & ajsonTableInstance
                    Catch ex As Exception
                        Dim lsMessage As String
                        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                        lsMessage &= vbCrLf & vbCrLf & ex.Message
                        prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                    End Try
                End If
            Else
                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data($"Line number {liLineNumber} not found in JSON data.",,,, True, True, Color.Salmon)
            End If

            ' Serialize the updated JSON object back into a JSON string
            Me.TaskPIJSON = JsonConvert.SerializeObject(lineStatuses, Newtonsoft.Json.Formatting.Indented)

        End Sub


        Public Sub SetTaskId(ByVal asNewTaskId As String)

            Try
                Me.TaskId = asNewTaskId

                For Each lrFEQLQuery In Me.FEQLQuery
                    lrFEQLQuery.TaskId = asNewTaskId
                Next


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub Save()

            Try

                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of OSM.Task, Boolean)) = Function(t) t.TaskId = Me.TaskId

                lrDataStore.Upsert(Me, whereClause)

#Region "OpenAI Functions"

                For Each lrOpenAIFunction In Me.OpenAIFunctions

                    Dim lrTaskOpenAIFunction = New OSM.TaskOpenAIFunction(Me.TaskId, lrOpenAIFunction.name)

                    Dim loTaskOpenAIFunctionWhereClause As Expression(Of Func(Of OSM.TaskOpenAIFunction, Boolean)) = Function(t) t.TaskId = Me.TaskId And t.OpenAIFunctionName = lrOpenAIFunction.name
                    lrDataStore.Upsert(lrTaskOpenAIFunction, loTaskOpenAIFunctionWhereClause)

                Next

#End Region

#Region "FEQL Queries"

                For Each lrTaskFEQLQuery In Me.FEQLQuery

                    Dim whereClauseFEQL As Expression(Of Func(Of OSM.TaskFEQLQuery, Boolean)) = Function(t) t.ID = lrTaskFEQLQuery.ID
                    lrDataStore.Upsert(lrTaskFEQLQuery, whereClauseFEQL)

                Next

#End Region

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Here for reference only.
        ''' </summary>
        ''' <param name="response"></param>
        Private Sub FormatResponseData(response As String)
            ' Parse the JSON string into a JObject
            Dim jsonObject As JObject = JObject.Parse(response)

            ' Extract the "role"
            Dim role As String = jsonObject("role").ToString()

            ' Extract the "CandidateName"
            Dim name As String = jsonObject("name").ToString()

            ' Extract the "actionableFeedback" array
            Dim actionableFeedbackArray As JArray = TryCast(jsonObject("actionableFeedback"), JArray)
            Dim actionableFeedbackList As List(Of String) = If(actionableFeedbackArray IsNot Nothing, actionableFeedbackArray.ToObject(Of List(Of String))(), New List(Of String))

            ' Extract the "Highlights" array
            Dim highlightsArray As JArray = TryCast(jsonObject("highlights"), JArray)
            Dim highlightsList As List(Of TraitProperties) = If(highlightsArray IsNot Nothing, highlightsArray.ToObject(Of List(Of TraitProperties))(), New List(Of TraitProperties))

            ' Extract the "lowLights" array
            Dim lowlightsArray As JArray = TryCast(jsonObject("lowlights"), JArray)
            Dim lowLightsList As List(Of TraitProperties) = If(lowlightsArray IsNot Nothing, lowlightsArray.ToObject(Of List(Of TraitProperties))(), New List(Of TraitProperties))

            ' Prepare the output string
            Dim output As New StringBuilder()

            output.AppendLine($"Generating interview insights for Candidate : {name}")
            output.AppendLine()
            output.AppendLine($"Role:{role}")
            output.AppendLine()

            If actionableFeedbackList.Any() Then
                output.AppendLine("Actionable Feedback:")
                For Each feedback As String In actionableFeedbackList
                    output.AppendLine("- " & feedback)
                Next
                output.AppendLine()
            End If

            If highlightsList.Any() Then
                output.AppendLine("Highlights:")
                For Each traits As TraitProperties In highlightsList
                    output.Append("- Trait: " & traits.Trait)
                    output.AppendLine("  [Weight: " & traits.Weight & " ]")
                Next
                output.AppendLine()
            End If

            If lowLightsList.Any() Then
                output.AppendLine("LowLights:")
                For Each traits As TraitProperties In lowLightsList
                    output.Append("- Trait: " & traits.Trait)
                    output.AppendLine("  [Weight: " & traits.Weight & " ]")
                Next
                output.AppendLine()
            End If

            ' Set the TextBoxOutput text
            'TextBoxOutput.Text &= vbCrLf & output.ToString()
        End Sub

#End Region
        Public Async Function ProcessUserMessage(ByVal asUserMessage As String) As Threading.Tasks.Task(Of String) 'Async Function <FunctionNameAndArguments>...... As Threading.Tasks.Task

            Try
#Region "Boston Specific: Stop listening for speech."
                'Stop taking RealTime Transcription, no matter what.
                If prApplication.Brain.AssemblyAIRelTimeTranscriber IsNot Nothing Then
                    prApplication.Brain.AssemblyAIRelTimeTranscriber.Dispose()
                    poCancellationTokenSource.Cancel()
                End If
#End Region
                If Me.TaskStatus = pcenumOSMTaskStatus.Closed Then Return "Closed"
                Call prApplication.Brain.TriggerSpeaking()
                Call Application.DoEvents()

                Dim lrUserMessage = New ChatMessage(ChatRole.User, asUserMessage) 'I'd like to provide with comprehensive feedback on the recent technical interview for the Software Engineer role for Mr.X. Overall, the technical knowledge and problem-solving skills were evident, but the engagement during the interview is low. Mr.X displayed a solid grasp of fundamental concepts, particularly in areas such as data structures and algorithms. Mr.X's ability to break down complex problems into manageable steps and systematically approach solutions was impressive. Additionally, Mr.X's coding skills were commendable. Mr.X code was well-structured and readable, and Mr.X articulated his thought process clearly during the coding exercises. While the code was correct, there was room for optimization in terms of time or space complexity. Paying closer attention to edge cases and potential corner scenarios is crucial, as they can impact the completeness of a solution. In terms of communication, consider providing a bit more context before diving into code to enhance clarity. Mr.X did not ask clarifying questions to confirm the understanding. He missed to use descriptive variable names and adding comments, especially for complex logic, so the code readability was low. Candidate needs further evaluation to decide as a culture fit as per the company culture or not.")

#Region "Completions"


                mrChatCompletionsOptions.Messages.Clear()
                mrChatCompletionsOptions.Messages.Add(lrUserMessage)
                mrChatCompletionsOptions.ChoiceCount = 1 '20240113-VM-Was 10, then 5....now reduced to 1. See KickStartTimer.
                mrChatCompletionsOptions.FrequencyPenalty = 0
                mrChatCompletionsOptions.Temperature = 1
                mrChatCompletionsOptions.MaxTokens = 2000

                mrChatCompletionsOptions.Functions = New List(Of FunctionDefinition)

                Dim tempFunctions As New List(Of FunctionDefinition)(mrChatCompletionsOptions.Functions)

                For Each lrFunctionDefinition In Me.GetTaskOpenAIFunctions
                    tempFunctions.Add(lrFunctionDefinition)
                Next

                mrChatCompletionsOptions.Functions = tempFunctions
#End Region

                '==================================
                'LLM Response.                
                Select Case Me.AIEngineUsed
                    Case Is = pcenumOSMAIEngineName.OpenAI
#Region "OpenAI Response Processing"

                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Calling OpenAI API.",,,, True, True, Color.Salmon)

                        'OpenAI 'gpt-4-1106-preview
                        'Work - Below
                        'gpt-4-0613
                        'gpt-4
                        'gpt-4-turbo-preview
                        Dim response As NullableResponse(Of ChatCompletions) = openAIClient.GetChatCompletions(GetEnumDescription(Me.AIModelName), mrChatCompletionsOptions) 'Can use gpt-4o

                        If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("OpenAI API PI: Responded.",,,, True, True, Color.Salmon)
#Region "Azure OpenAI API"
                        ' Read and process the response content
                        If response.Value?.Choices IsNot Nothing Then

                            Dim lbRespondedToUser As Boolean = False
                            Dim jsonObject As JObject = Nothing

                            For Each lrChoice In response.Value?.Choices

                                If lrChoice.FinishReason = "function_call" Then
#Region "Function Calls"
                                    Dim finalContent As String = lrChoice.Message.FunctionCall.Arguments

                                    'If Not lasCalledFunctions.Contains(lrChoice.Message.FunctionCall.Name) Then

                                    Select Case lrChoice.Message.FunctionCall.Name.Trim

#Region "AGI Functions"

                                        Case Is = "OSMRunTask"

                                            Call Me.ExecuteOSMRunTask(finalContent)

                                            'Kickstart so can process results.
                                            Me.KickstartTimer.Interval = 200
                                            Me.KickstartTimer.Enabled = True
                                            Call Me.KickstartTimer.Start()
#End Region

#Region "The Box"
                                        Case Is = "ListBrainWorkingOnTasks"

                                            Call Me.ExecuteListBrainWorkingOnTasks()

                                            If Me.Name = "Percy" Then
                                                Dim lsUserResponse = "Thank you. Processed by Percy."
                                                Me.Brain.send_data(lsUserResponse, False, True)
                                                Me.ConversationFlow.AppendDoubleLineBreak("You/Bot: " & lsUserResponse)
                                                If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Debug:" & lrChoice.Message.FunctionCall.Name & ":" & lsUserResponse, False, True, , True, True, Color.Salmon)
                                                lbRespondedToUser = True
                                                Return "Thank you. Processed by Percy."
                                            End If

                                        Case Is = "OSMTheBoxPopulateTheBox"

                                            Await Me.ExecuteTheBoxPopulateTheBox(finalContent)

                                        Case Is = "OSMTheBoxOpenWebPage"

                                            Call Me.ExecuteTheBoxOpenWebPage(finalContent)
#End Region

                                        Case Is = "EmailSendEmail"

                                            Call Me.ExecuteSendEmail(finalContent)

                                        Case Is = "FactEngineRunNaturalLanguageQuery"
#Region "FactEngine Natural Language Query"
                                            jsonObject = JObject.Parse(finalContent)
                                            Try
                                                If Not lbRespondedToUser Then
                                                    'Extract the FactEngine Query Language (FEQL) Query
                                                    Dim lsResponseToUser As String = jsonObject("response_to_user").ToString()
                                                    If Not Me.LastPleaseWaitStatement = lsResponseToUser Then
                                                        Me.Brain.send_data(lsResponseToUser, False, True,, False)
                                                        Me.LastPleaseWaitStatement = lsResponseToUser
                                                    End If
                                                    Me.ConversationFlow.AppendDoubleLineBreak("You/Bot: " & lsResponseToUser)
                                                    lbRespondedToUser = True
                                                End If
                                            Catch ex As Exception
                                            End Try

                                            Select Case Me.ExecuteFEQLNaturalLanguageQuery(finalContent,)
                                                Case Is = pcenumOSMFactEngineQueryResultStatus.Success
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                                    Return Nothing
                                                Case Is = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                                    Return Nothing
                                                Case Is = pcenumOSMFactEngineQueryResultStatus.Fail,
                                                          pcenumOSMFactEngineQueryResultStatus.NoModelFound

                                                    'KickStart the OSM. Because the response_to_user may not be valid any more.
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                                Case Else

                                                    'KickStart the OSM. Because the response_to_user may not be valid any more.
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                            End Select

                                            lbRespondedToUser = True

#End Region

                                        Case Is = "FactEngineRunFactEngineQueryLanguageQuery"

#Region "FactEngine Query"
                                            jsonObject = JObject.Parse(finalContent)
                                            Try
                                                If Not lbRespondedToUser Then
                                                    'Extract the FactEngine Query Language (FEQL) Query
                                                    Dim lsResponseToUser As String = jsonObject("response_to_user").ToString()
                                                    If Not Me.LastPleaseWaitStatement = lsResponseToUser Then
                                                        Me.Brain.send_data(lsResponseToUser, False, True,, False)
                                                        Me.LastPleaseWaitStatement = lsResponseToUser
                                                    End If
                                                    Me.ConversationFlow.AppendDoubleLineBreak("You/Bot: " & lsResponseToUser)
                                                    lbRespondedToUser = True
                                                End If
                                            Catch ex As Exception

                                            End Try

                                            Select Case Me.ExecuteFEQL(finalContent,,)
                                                Case Is = pcenumOSMFactEngineQueryResultStatus.Success
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                                    Return Nothing
                                                Case Is = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                                    Return Nothing
                                                Case Is = pcenumOSMFactEngineQueryResultStatus.Fail,
                                          pcenumOSMFactEngineQueryResultStatus.NoModelFound

                                                    'KickStart the OSM. Because the response_to_user may not be valid any more.
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                                Case Else

                                                    'KickStart the OSM. Because the response_to_user may not be valid any more.
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                            End Select

                                            lbRespondedToUser = True
#End Region

                                        Case Is = "RDSUpSertTableInstance"

                                            Call Me.ExecuteUpSertTableDataInstance(finalContent)
#Region "NearAGI Memory Units"
                                        Case Is = "UpSertNearAGIDBMemoryUnit"

                                            Call Me.ExecuteUpSertNearAGIDBMemoryUnit(finalContent)

                                        Case Is = "DeleteNearAGIDBMemoryUnit"

                                            Call Me.ExecuteDeleteNearAGIDBMemoryUnit(finalContent)

                                        Case Is = "FindNearAGIMemoryUnits"

#Region "Find Memories"
                                            jsonObject = JObject.Parse(finalContent)
                                            Try
                                                If Not lbRespondedToUser Then
                                                    'Extract the FactEngine Query Language (FEQL) Query
                                                    Dim lsResponseToUser As String = jsonObject("response_to_user").ToString()
                                                    If Not Me.LastPleaseWaitStatement = lsResponseToUser Then
                                                        Me.Brain.send_data(lsResponseToUser, False, True,, False)
                                                        Me.LastPleaseWaitStatement = lsResponseToUser
                                                    End If
                                                    Me.ConversationFlow.AppendDoubleLineBreak("You/Bot: " & lsResponseToUser)
                                                    lbRespondedToUser = True
                                                End If
                                            Catch ex As Exception

                                            End Try


                                            Select Case Me.ExecuteFindNearAGIDBMemoryUnits(finalContent)
                                                Case Is = pcenumOSMFactEngineQueryResultStatus.Success

                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                                    Return Nothing

                                                Case Is = pcenumOSMFactEngineQueryResultStatus.EmptyResultSet

                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()
                                                    Return Nothing

                                                Case Is = pcenumOSMFactEngineQueryResultStatus.Fail 'pcenumOSMFactEngineQueryResultStatus.NoModelFound

                                                    'KickStart the OSM. Because the response_to_user may not be valid any more.
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()

                                                Case Else
                                                    'KickStart the OSM. Because the response_to_user may not be valid any more.
                                                    Me.KickstartTimer.Interval = 200
                                                    Me.KickstartTimer.Enabled = True
                                                    Call Me.KickstartTimer.Start()

                                            End Select

                                            lbRespondedToUser = True
#End Region
#End Region

                                        Case Is = "RDSDeleteTableInstance"

                                            Call Me.ExecuteDeleteTableDataInstance(finalContent)

                                        Case Is = "RDSAppendTableStringColumn"

                                            Call Me.ExecuteTableStringColumnAppend(finalContent)

                                        Case Is = "NPUMarkTaskStepAsComplete"
                                            Call Me.SetLineStatusToCompleted(finalContent)
                                            lbRespondedToUser = False

                                        Case Is = "SimpleUserResponse"
                                            'Nothing to do here. See response_to_user below.
                                            lbRespondedToUser = False

                                        Case Is = "OSMCloseTask"

                                            Call Me.ExecuteCloseTask()

                                        Case Else
                                            Me.Brain.send_data("Debug:" & lrChoice.Message.FunctionCall.Name & vbCrLf & lrChoice.Message.FunctionCall.Arguments.ToString, False, True, , True, True, Color.Salmon)

                                    End Select

#Region "Response to user"
                                    ' Parse the JSON string into a JObject
                                    jsonObject = JObject.Parse(finalContent)

                                    Try
                                        If Not lbRespondedToUser Then
                                            'Extract the FactEngine Query Language (FEQL) Query
                                            Dim lsUserResponse As String = jsonObject("response_to_user").ToString()
                                            Me.Brain.send_data(lsUserResponse, False, True)
                                            Me.ConversationFlow.AppendDoubleLineBreak("You/Bot: " & lsUserResponse)
                                            If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Debug:" & lrChoice.Message.FunctionCall.Name & ":" & lsUserResponse, False, True, , True, True, Color.Salmon)
                                            lbRespondedToUser = True
                                        End If

                                    Catch ex As Exception
                                        'We tried.
                                    End Try

                                    'Threading.Thread.Sleep(1000)
#End Region

                                    'End If
                                Else
                                    'CodeSafe
                                    If Me.ConversationFlow Is Nothing Then Me.ConversationFlow = ""
                                    Me.ConversationFlow.AppendDoubleLineBreak("You/Bot: " & lrChoice.Message.Content)
                                    Me.Brain.send_data(lrChoice.Message.Content, False, True,, False)
                                    Return lrChoice.Message.Content
                                End If
#End Region
                            Next

                            If Me.DebugMode = pcenumDebugMode.Debug Then Me.Brain.send_data("Next Line Number: " & Me.NextPIJSONLineNumber,,,, True, True, Color.Salmon)
                        End If
#End Region

#End Region
                    Case Is = pcenumOSMAIEngineName.Groq
#Region "Groq"
#Region "Functions"
                        'Create a JArray to hold the functions
                        Dim functionsArray As New JArray()

                        'Add each function to the JObject
                        For Each lrOpenAIFunction In Me.OpenAIFunctions
                            Dim lrFunctionDetails As New JObject()
                            lrFunctionDetails.Add("name", lrOpenAIFunction.name)
                            lrFunctionDetails.Add("description", lrOpenAIFunction.description)
                            'Parse the parameters property as a JSON object
                            Dim parametersJson As JObject = JObject.Parse(lrOpenAIFunction.parameters)
                            lrFunctionDetails.Add("parameters", parametersJson)

                            'Create the function object and add the function details to it
                            Dim lrFunctionObject As New JObject()
                            lrFunctionObject.Add("type", "function")
                            lrFunctionObject.Add("function", lrFunctionDetails)

                            'Add the function object to the functions array
                            functionsArray.Add(lrFunctionObject)
                        Next
#End Region

                        ''Create the main JSON object
                        Dim jsonData As New JObject() '"be a bot. Call a function")) for below, was, for system. 20240719
                        ',New JObject(New JProperty("role", "user"), New JProperty("content", asUserMessage))
                        jsonData.Add("messages", New JArray(New JObject(New JProperty("role", "system"), New JProperty("content", asUserMessage))))
                        'jsonData.Add("messages", New JArray(New JObject(New JProperty("role", "user"), New JProperty("content", asUserMessage)))) ' "Hello World"))))
                        jsonData.Add("model", "mixtral-8x7b-32768")
                        'Doesn't work: jsonData.Add("response_format", New JObject(New JProperty("type", "json_object")))
                        jsonData.Add("tools", functionsArray)
                        jsonData.Add("tool_choice", "auto")
                        jsonData.Add("max_tokens", 700) ' Updated key to tools

                        Dim lsGroqResponse = Boston.GetGroqResponse(jsonData.ToString)

                        'Dim ljtokenChoiceContent As String = lsGroqResponse.SelectToken("choices[0].message.content").ToString()
                        Dim lsContent = If(lsGroqResponse.SelectToken("choices[0].message.content") Is Nothing, Nothing, lsGroqResponse.SelectToken("choices[0].message.content").ToString())
                        Dim ljtokenMessageResponse = lsGroqResponse.SelectToken("choices[0].message")
                        Try


                            If lsContent Is Nothing Then

                                Dim toolCallToken = ljtokenMessageResponse.SelectToken("tool_calls")

                            Else
                                'CodeSafe
                                If Me.ConversationFlow Is Nothing Then Me.ConversationFlow = ""
                                Me.ConversationFlow.AppendDoubleLineBreak("You/Bot: " & lsContent)
                                Me.Brain.send_data(lsContent, False, True,, False)
                                Return lsContent
                            End If

                            'If Response IsNot Nothing Then 'Not response.HasValue Then
                            '    Throw New Exception("Error: " & Response.ToString())
                            'Else
                            '    Dim choices As JArray = Response("choices")
                            '    If choices IsNot Nothing AndAlso choices.Count > 0 Then
                            '        For Each choice As JObject In choices

                            '            Dim lbRespondedToUser As Boolean = False
                            '            Dim jsonObject As JObject = Nothing
                            '        Next
                            '    End If
                            'End If

                        Catch ex As Exception

                        End Try

#End Region

                End Select

            Catch ex As Exception
                ' Handle any exceptions that may occur
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Brain.send_data(lsMessage, False, True,, True, True, Color.Salmon)
            End Try

        End Function


        Public Sub LoadDataModel()

            Try
                If Me._ModelId IsNot Nothing Then
                    Dim lrModel = prApplication.Models.Find(Function(x) x.ModelId = Me._ModelId)

                    If lrModel IsNot Nothing Then

                        If Not lrModel.Loaded Then
                            Call lrModel.Load()
                        End If

                        Me.DataModel = lrModel.RDS
                    Else
                        prApplication.ThrowMessage("The ModelId for the Task, " & Me.Name & " is not set.", pcenumErrorType.Warning, abUseFlashCard:=True)
                    End If
                Else
                    prApplication.ThrowMessage("The ModelId for the Task, " & Me.Name & " is not set.", pcenumErrorType.Warning, abUseFlashCard:=True)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Re/Generates the OSM Context. I.e. The Self-Assembled Context Space.
        ''' </summary>
        ''' <returns></returns>
        Public Function GenerateOSMContext() As String

            Dim lsContext As String = ""

            lsContext = Me.Template

            Dim lasTemplateVariable As New List(Of String)

            lasTemplateVariable.Add("{TaskInstanceId}")
            lasTemplateVariable.Add("{TaskName}")
            lasTemplateVariable.Add("{CurrentLocalDateTime}")
            lasTemplateVariable.Add("{TaskDescription}")
            lasTemplateVariable.Add("{TaskShortGoalDescription}")
            lasTemplateVariable.Add("{TaskLongGoalDescription}")
            lasTemplateVariable.Add("{OSMSecretSource}")
            lasTemplateVariable.Add("{OSMSecretSourceCompressed}")
            lasTemplateVariable.Add("{DataModel}")
            lasTemplateVariable.Add("{DataModelDataInstances}")
            lasTemplateVariable.Add("{TaskPIJSON}")
            lasTemplateVariable.Add("{TaskShortPI-JSON}")
            lasTemplateVariable.Add("{ThatVariable}")
            lasTemplateVariable.Add("{TaskUserInteractionTranscript}")
            lasTemplateVariable.Add("{FunctionList}")
            lasTemplateVariable.Add("{FunctionCallJSONSchema}")
            lasTemplateVariable.Add("{NearAGIDBNearTermMemory}")

            Try
                For Each lsTemplateVariable In lasTemplateVariable
                    Select Case lsTemplateVariable
                        Case Is = "{TaskInstanceId}"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.TaskInstanceId)
                        Case Is = "{TaskName}"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.Name)
                        Case Is = "{CurrentLocalDateTime}"
                            lsContext = lsContext.Replace(lsTemplateVariable, "The current date and time is: CurrentLocalDateTime:" & DateTime.Now.ToString("ddd d MMMM yyyy h:mmtt"))
                        Case Is = "{TaskDescription}"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.Description)
                        Case Is = "{TaskShortGoalDescription}"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.GoalShortDescription)
                        Case Is = "{TaskLongGoalDescription}"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.GoalLongDescription)
                        Case Is = "{OSMSecretSource}"
#Region "Secret Source"
                            Dim assembly As Assembly = Assembly.GetExecutingAssembly()
                            Dim resourceName As String = "Boston.OSM-SecretSource.txt"
                            Dim lsSecretSource As String
                            Using stream As Stream = assembly.GetManifestResourceStream(resourceName)
                                If stream IsNot Nothing Then
                                    Using reader As New StreamReader(stream)
                                        lsSecretSource = reader.ReadToEnd()
                                    End Using
                                Else
                                    Throw New Exception("Resource not found.")
                                End If
                            End Using
#End Region
                            lsContext = lsContext.Replace(lsTemplateVariable, lsSecretSource)
                        Case Is = "{OSMSecretSourceCompressed}"
#Region "Secret Source - Compressed"
                            Dim assembly As Assembly = Assembly.GetExecutingAssembly()
                            Dim resourceName As String = "Boston.OSM-SecretSource-Compressed.txt"
                            Dim lsSecretSourceCompressed As String
                            Using stream As Stream = assembly.GetManifestResourceStream(resourceName)
                                If stream IsNot Nothing Then
                                    Using reader As New StreamReader(stream)
                                        lsSecretSourceCompressed = reader.ReadToEnd()
                                    End Using
                                Else
                                    Throw New Exception("Resource not found.")
                                End If
                            End Using
#End Region
                            lsContext = lsContext.Replace(lsTemplateVariable, lsSecretSourceCompressed)
                        Case Is = "{DataModel}"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.GenerateDataModelContext)
                        Case Is = "{DataModelDataInstances"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.GenerateTableDataInstanceContext())
                        Case Is = "{TaskPIJSON}"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.TaskPIJSON)
                        Case Is = "{TaskShortPI-JSON}"
                            Dim lsShortPIJSON = Me.CompressionKey
                            lsShortPIJSON.AppendDoubleLineBreak(Me.TaskShortPIJSON)
                            lsContext = lsContext.Replace(lsTemplateVariable, lsShortPIJSON)
                        Case Is = "{ThatVariable}"
                            lsContext = lsContext.Replace(lsTemplateVariable, Me.ThatVariable)
                        Case Is = "{TaskUserInteractionTranscript}"
                            Dim lsConversationFlow = Me.ConversationFlow
                            If Me.LastFEQLQueryResults IsNot Nothing Then
                                lsConversationFlow.AppendDoubleLineBreak(Me.LastFEQLQueryResults)
                                Me.LastFEQLQueryResults = Nothing
                            End If
                            lsContext = lsContext.Replace(lsTemplateVariable, lsConversationFlow)
                        Case Is = "{FunctionList}"
#Region "Function List"
                            'Create a JArray to hold the functions
                            Dim functionsArray As New JArray()

                            'Add each function to the JObject
                            For Each lrOpenAIFunction In Me.OpenAIFunctions
                                Dim lrFunctionJSONObject As New JObject()
                                lrFunctionJSONObject.Add("name", lrOpenAIFunction.name)
                                lrFunctionJSONObject.Add("description", lrOpenAIFunction.description)
                                'Parse the parameters property as a JSON object
                                Dim parametersJson As JObject = JObject.Parse(lrOpenAIFunction.parameters)
                                lrFunctionJSONObject.Add("parameters", parametersJson)
                                functionsArray.Add(lrFunctionJSONObject)
                            Next
#End Region
                            lsContext = lsContext.Replace(lsTemplateVariable, functionsArray.ToString)
                        Case Is = "{FunctionCallJSONSchema}"
#Region "FunctionCallJSONSchema"
                            Dim lsJSONSchema = "{" &
                                                """$schema"": ""http://json-schema.org/draft-07/schema#""," &
                                                """title"": ""TranslationResponse""," &
                                                """type"": ""object""," &
                                                """properties"": {" &
                                                    """success"": {" &
                                                        """type"": ""boolean""," &
                                                        """description"": ""Indicates whether the translation was successful.""}" &
                                                    "," &
                                                    """data"": {" &
                                                        """type"": ""object""," &
                                                        """properties"": {" &
                                                            """translations"": {" &
                                                                """type"": ""array""," &
                                                                """items"": {" &
                                                                    """type"": ""object""," &
                                                                    """properties"": {" &
                                                                        """translated_text"": {" &
                                                                            """type"": ""string""," &
                                                                            """description"": ""The translated text.""" &
                                                                        "}" &
                                                                    "}," &
                                                                    """required"": [""translated_text""]" &
                                                                "}" &
                                                            "}" &
                                                        "}," &
                                                        """required"": [""translations""]" &
                                                    "}" &
                                                "}," &
                                                """required"": [""success"", ""data""]" &
                                            "}"
#End Region
                            lsContext = lsContext.Replace(lsTemplateVariable, lsJSONSchema)

                        Case Is = "{NearAGIDBNearTermMemory}"

                            Dim lrDataStore As New DataStore.Store
                            Dim lsLastWeekDateTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss").ToString
                            Dim whereClause As Expression(Of Func(Of NearAGI.MemoryUnit, Boolean)) = Function(t) t.DateTimeStamp > lsLastWeekDateTime

                            Dim larNearTermMemories = lrDataStore.Get(whereClause)

                            Dim lsNearTermMemories As String = ""
                            For Each lrNearTermMemory In larNearTermMemories
                                lsNearTermMemories.AppendLine(lrNearTermMemory.ToString)
                            Next

                            lsContext = lsContext.Replace(lsTemplateVariable, lsNearTermMemories)

                    End Select
                Next

                'FEQL Queries
                For Each lrFEQLQuery In Me.FEQLQuery.FindAll(Function(x) Not x.IsUserQueryAnswerer)
                    Dim lsFactEngineResults As String = ""
                    lsFactEngineResults = lrFEQLQuery.QueryName
                    lsFactEngineResults.AppendDoubleLineBreak(Me.ExecuteFEQL(lrFEQLQuery.FEQLQuery, Color.MediumBlue))
                    lsFactEngineResults.AppendLine("----" & vbCrLf)
                    lsContext = lsContext.Replace(lrFEQLQuery.QueryName, lsFactEngineResults)
                Next

                'FEQL User Based Queries
                Dim lsContextAddition As String = ""
                For Each lrFEQLQuery In Me.FEQLQuery
                    lsContextAddition &= lrFEQLQuery.QueryName
                    lsContextAddition.AppendLine(lrFEQLQuery.QueryResultDescription)
                    lsContextAddition.AppendLine("FEQL Query To run " & lrFEQLQuery.FEQLQuery)
                    lsContextAddition.AppendLine("----" & vbCrLf)
                Next
                lsContext = lsContext.Replace("{FactEngineUserBasedQueries}", lsContextAddition)

                Me.LatestOSMContext = lsContext

                RaiseEvent OSMContextGenerated()

                Return lsContext

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Return "Error Generating NPU Context".AppendDoubleLineBreak(ex.Message)
            End Try

        End Function

        Public Function GenerateDataModelContext() As String

            Dim lsContext As String = ""

            'For Each lrTable In Me.DataModel.Table

            '    lsContext &= vbCrLf & "The following Is the data definition for the " & lrTable.Name & " table used as part of this task."
            '    lsContext &= vbCrLf & lrTable.GenerateJSON
            'Next
            lsContext.AppendLine("")

#Region "JSON serialisation of the Model.RDS"
            lsContext = JsonConvert.SerializeObject(Me.DataModel, Newtonsoft.Json.Formatting.Indented)

            Return lsContext
#End Region


#Region "XML serialisation of the Model.RDS"
            'XML
            Dim loXMLSerializer As New XmlSerializer(GetType(RDS.Model))

            ' Create a StringBuilder to hold the XML
            Dim sb As New StringBuilder()

            ' StringWriter to use with the StringBuilder
            Using sw As New StringWriter(sb)
                ' Serialize the object to the StringWriter
                loXMLSerializer.Serialize(sw, Me.DataModel)
            End Using

            ' The serialized XML is now in the StringBuilder
            lsContext &= sb.ToString()
#End Region

            Return lsContext

        End Function

        Public Function GenerateTableDataInstanceContext() As String

            Dim lsContext As String = ""

            For Each lrTable In Me.DataModel.Table

                lsContext.AppendDoubleLineBreak("The following are instance data for the " & lrTable.Name & " table, And as used as part of this task.")
                lsContext &= vbCrLf & lrTable.GenerateDataInstanceContext

            Next

            Return lsContext

        End Function

        Private Sub KickstartTimer_Tick(sender As Object, e As EventArgs)

            Try
                Me.KickstartTimer.Stop()

                Dim lsOSMContext As String = ""

                If Me.UsePIJSON Then lsOSMContext = "NextPIJSONLineNumber " & Me.NextPIJSONLineNumber
                Me.NextPIJSONLineNumber = "Consult the user transcript for where you should be."

                If Me.ContextPrefix IsNot Nothing Then
                    lsOSMContext.AppendLine(Me.ContextPrefix)
                End If

                lsOSMContext.AppendDoubleLineBreak("You are now in AGI mode processing the below. In AGI mode it Is your job to analyse the context space,)")
                lsOSMContext.AppendLine(" 1. Especially the Conversation Transcript And PIJSON, And")
                lsOSMContext.AppendLine(" a. Respond to the user based on the Conversation Transcript if required")
                If Me.UsePIJSON Then
                    lsOSMContext.AppendLine(" b) Decide which PIJSON line number to execute if required; And")
                    lsOSMContext.AppendLine(" c) Execute that PISON line number by responding appropriately (with function call/s And conversational flow).")
                End If
                lsOSMContext.AppendLine(" And...")
                lsOSMContext.AppendLine(" Pay attention to the conversation flow/conversation transcript. E.g. If no rows returned from a query Or an error from a query to the database, don't repeat the query.")
                lsOSMContext.AppendDoubleLineBreak("")

                lsOSMContext &= Me.GenerateOSMContext

                Call Me.ProcessUserMessage(lsOSMContext)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub Brain_FormTheBoxClosed() Handles _Brain.FormTheBoxClosed
            Me.TheBox = Nothing
        End Sub

    End Class

End Namespace