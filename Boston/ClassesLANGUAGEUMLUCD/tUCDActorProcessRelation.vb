Namespace UCD
    Public Class ActorProcessRelation
        Inherits UML.ActorProcessRelation
        Implements FBM.iPageObject

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
