Namespace UML
    Public Class ActorProcessRelation
        Inherits FBM.FactInstance
        Implements FBM.iPageObject

        Public Actor As CMML.Actor
        Public Process As CMML.Process

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal arActor As CMML.Actor, ByVal arProcess As CMML.Process)

            Me.Actor = arActor
            Me.Process = arProcess

        End Sub

    End Class

End Namespace
