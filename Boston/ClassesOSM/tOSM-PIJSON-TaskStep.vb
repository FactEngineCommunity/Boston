Namespace OSM.PIJSON
    Public Class TaskStep

        ''' <summary>
        ''' The line number of the TaskStep in the set of TaskSteps that make up the Psecudocode In JSON.
        ''' </summary>
        Public LineNumber As Integer

        ''' <summary>
        ''' The action to take. Can include calling an API or Function.
        ''' ...includes { IF..., IF ..GOTO ...FOR, END FOR, GOTO}
        ''' High level (one-liner) description of Step.
        ''' </summary>
        Public Action As String

        ''' <summary>
        ''' The Status of the TaskStep {NotStarted, WorkingOnIt, Complete}.
        ''' </summary>
        Public Status As pcenumOSMPIJSONStatus

        ''' <summary>
        ''' A short description of the TaskStep.
        ''' </summary>
        Public Annotation As String

        ''' <summary>
        ''' The Script to read to the User if there is one.
        ''' </summary>
        Public Script As String

        ''' <summary>
        ''' Any sub-Steps (TaskSteps) that are required for this TaskStep.
        ''' </summary>
        Public SubStep As New List(Of OSM.PIJSON.TaskStep)

    End Class

End Namespace