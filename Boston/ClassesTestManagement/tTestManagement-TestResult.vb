Imports Newtonsoft.Json

Namespace TestManagement

    ''' <summary>
    ''' Class for results of tests
    ''' </summary>
    Public Class TestResult
        Inherits Identifiable

        <JsonProperty>
        Public Property StartDateTime As DateTime? '? means nullable DateTime.

        <JsonProperty>
        Public Property CompleteDateTime As DateTime?

        <JsonProperty>
        Public Property Verdict As String

        'ExecutionResult As String
        'LastAttemptDateTime As DateTime
        'VersionTested As 
        'Tester
        'ResultsObtained
        'ResultNotes


        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub

    End Class

End Namespace