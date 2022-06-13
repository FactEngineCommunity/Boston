Public Class tUseCaseSystemBoundary
    Inherits FBM.ModelObject

    Public X As Integer
    Public Y As Integer

    Sub New(Optional ByVal aiConceptType As pcenumConceptType = Nothing)
        If IsSomething(aiConceptType) Then
            Me.ConceptType = aiConceptType
        End If
    End Sub

    Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)
        '-------------------------------------------------------------------------------
        'Doesn't do anything at this stage, but leave here because may be called if the 
        '  user clicks on a SystemBoundary while the Properties form is open
        '-------------------------------------------------------------------------------
    End Sub

End Class

