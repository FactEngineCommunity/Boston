Namespace UCD
    Public Class ActorProcessRelation
        Inherits UML.ActorProcessRelation
        Implements FBM.iPageObject

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal arActor As UCD.Actor, ByVal arProcess As UCD.Process)

            Me.Actor = arActor
            Me.Process = arProcess

        End Sub

    End Class

End Namespace
