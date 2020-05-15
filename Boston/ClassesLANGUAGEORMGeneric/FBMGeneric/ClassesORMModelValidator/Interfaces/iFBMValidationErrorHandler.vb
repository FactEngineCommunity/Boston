Namespace FBM

    Public Interface iValidationErrorHandler
        Property ModelError() As List(Of FBM.ModelError)
        ReadOnly Property HasModelError() As Boolean
        Sub ClearModelErrors()
        Event ModelErrorAdded(ByRef arModelError As FBM.ModelError)
        Sub AddModelError(ByRef arModelError As FBM.ModelError)
    End Interface

End Namespace