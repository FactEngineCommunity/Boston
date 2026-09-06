Imports Newtonsoft.Json

Namespace TestManagement

    ''' <summary>
    ''' Class for sequences of tests. NB Not really used...is derivable from the TestSteps in the TestCase.
    ''' </summary>
    Public Class TestSequence
        Inherits Identifiable

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub

    End Class

End Namespace