Namespace OSM
    Partial Public Class Task

        '#Region "Status"
        '        <JsonIgnore>
        '        Public ReadOnly Property HasStarted As Boolean
        '            Get
        '                Return Me.GetHighestWorkingOnItLineNumber > 0 Or Me.GetLastCompletedStep IsNot Nothing
        '            End Get
        '        End Property

        '        <JsonIgnore>
        '        Public ReadOnly Property IsComplete As Boolean
        '            Get
        '                Return Me.GetUncompletedSteps.Count = 0
        '            End Get
        '        End Property

        '#End Region

        '        Public Function GetUncompletedSteps() As List(Of OSM.PIJSON.TaskStep)

        '            Throw New NotImplementedException

        '        End Function

        '        Public Function GetLastCompletedStep() As OSM.PIJSON.TaskStep

        '            Try
        '                Return Nothing

        '            Catch ex As Exception
        '                Dim lsMessage As String
        '                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

        '                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
        '                lsMessage &= vbCrLf & vbCrLf & ex.Message
        '                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        '            End Try

        '        End Function




        'Public Function GetLastStepLineNumber() As Integer

        '    If Me.TaskPseudocode Is Nothing Then Return 0
        '    If Me.TaskPseudocode.Step.Count = 0 Then Return 0

        '    Return Me.TaskPseudocode.Step.Last.LineNumber

        'End Function

        'Public Function SetStepCompletedByLineNumber(ByVal aiLineNumber) As Boolean

        '    Dim lrTaskStep = Me.TaskPseudocode.Step.Find(Function(x) x.LineNumber = aiLineNumber)

        '    If lrTaskStep IsNot Nothing Then
        '        lrTaskStep.Status = pcenumOSMPIJSONStatus.Completed
        '        Return True
        '    Else
        '        Return False
        '    End If

        'End Function

        'Public Function SetStepWorkingOnItByLineNumber(ByVal aiLineNumber As Integer) As Boolean

        '    Dim lrTaskStep = Me.TaskPseudocode.Step.Find(Function(x) x.LineNumber = aiLineNumber)

        '    If lrTaskStep IsNot Nothing Then
        '        lrTaskStep.Status = pcenumOSMPIJSONStatus.WorkingOnIt
        '        Return True
        '    Else
        '        Return False
        '    End If

        'End Function

        'Public Function IsStepCompleteByLineNumber(ByVal aiLineNumber As Integer) As Boolean

        '    Throw New NotImplementedException

        '    'Dim lrTaskStep = Me.TaskPseudocode.Step.Find(Function(x) x.LineNumber = aiLineNumber)

        '    'If lrTaskStep IsNot Nothing Then
        '    '    Return lrTaskStep.Status = pcenumOSMPIJSONStatus.Completed
        '    'Else
        '    '    Return False 'Might need to return an error here.
        '    'End If

        'End Function

        'Public Function GetHighestWorkingOnItLineNumber() As Integer

        '    Throw New NotImplementedException
        '    'If Me.TaskPseudocode Is Nothing Then Return 0
        '    'If Me.TaskPseudocode.Step.Count = 0 Then Return 0

        '    'Try
        '    '    Return Me.TaskPseudocode.Step.FindAll(Function(x) x.Status = pcenumOSMPIJSONStatus.WorkingOnIt).First.LineNumber
        '    'Catch
        '    '    Return 0
        '    'End Try

        'End Function


    End Class

End Namespace
