Namespace ClientServer

    Public Class Log

        Public DateTime As DateTime

        Public User As ClientServer.User

        Public LogType As pcenumLogType

        ''' <summary>
        ''' The IP Address that the User logged in from, else "NOTHING" (Desktop Mode).
        ''' </summary>
        ''' <remarks></remarks>
        Public IPAddress As String = ""

    End Class

End Namespace
