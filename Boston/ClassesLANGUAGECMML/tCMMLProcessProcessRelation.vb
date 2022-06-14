Namespace CMML
    Public Class ProcessProcessRelation
        Inherits FBM.FactInstance
        Implements FBM.iPageObject

        Public Model As CMML.Model

        Public Process1 As CMML.Process
        Public Process2 As CMML.Process


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
