Namespace FBM.STM

    Public Class Model

        Dim State As List(Of FBM.STM.State)


        Public Sub addState(ByRef arState As FBM.STM.State)

            Me.State.AddUnique(arState)

        End Sub

    End Class

End Namespace
