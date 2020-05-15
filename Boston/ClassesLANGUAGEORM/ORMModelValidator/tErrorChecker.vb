Namespace Validation

    ''' <summary>
    ''' Inherited Only
    ''' Base class for ErrorChecker classes.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ErrorChecker

        ''' <summary>
        ''' The Model being checked.
        ''' </summary>
        ''' <remarks></remarks>
        Public Model As FBM.Model

        Public Sub New(ByRef arModel As FBM.Model)

            Me.Model = arModel
        End Sub

        Public Overridable Sub CheckForErrors()

        End Sub

    End Class

End Namespace

