Public Class tRegistrationResult

    Public IsRegistered As Boolean = False

    ''' <summary>
    ''' E.g. Professional, Student
    ''' </summary>
    Public SoftwareType As String = "Student"

    ''' <summary>
    ''' E.g. None, Subscription, Trial
    ''' </summary>
    Public SubscriptionType As String = "None"

    ''' <summary>
    ''' Format dd/MM/yyyy
    ''' </summary>
    Public RegisteredToDate As String

End Class
