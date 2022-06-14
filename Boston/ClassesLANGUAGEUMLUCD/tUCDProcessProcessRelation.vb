Namespace UCD
    Public Class ProcessProcessRelation
        Inherits UML.ProcessProcessRelation
        Implements FBM.iPageObject

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal arProcess1 As CMML.Process, ByVal arProcess2 As CMML.Process)

            Me.Process1 = arProcess1
            Me.Process2 = Process2

        End Sub

    End Class

End Namespace
