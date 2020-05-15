Public Class tSpecialDragMode

    Public SpecialDragMode As pcenumSpecialDragMode
    Public MouseUpCounter As Integer = 0

    Public Sub ResetSpecialDragMode()
        Me.SpecialDragMode = pcenumSpecialDragMode.None
        Me.MouseUpCounter = 0
    End Sub

    Public Sub SetSpecialDragMode(ByVal aiSpecialDragMode As pcenumSpecialDragMode)

        Me.SpecialDragMode = aiSpecialDragMode

    End Sub

End Class
