Imports Boston.Parser.Output

Namespace Parser.Syntax

    Friend Class Main
        Private Source As String = Nothing
        Private BasePath As String = Nothing
        Private PreviewMode As Boolean = False

        'Main exec
        Private WithEvents proc As Exec_Base = Nothing

        Public Event Notify(ByVal Message As String)
        Public Event OutputWritten(ByVal Path As String)
        Public Event ConsoleOut(ByVal Message As String)
        Public Event ConsoleClear()

        Public Sub New(ByVal Source As String, ByVal BasePath As String, ByVal PreviewMode As Boolean)
            Me.Source = Source
            Me.BasePath = BasePath
            Me.PreviewMode = PreviewMode
        End Sub

        Public ReadOnly Property OutputList() As OutputCollection
            Get
                If Me.proc Is Nothing Then Return New OutputCollection(Me.BasePath)
                If Me.proc.OutputList Is Nothing Then Return New OutputCollection(Me.BasePath)
                Return Me.proc.OutputList
            End Get
        End Property

        Public Sub Run()
            Try
                'Compile and execute
                Dim comp As New Parser.Syntax.Compilation("", Me.Source, True, False)
                Me.proc = New Exec_Base(comp.Compile(), Me.BasePath, Me.PreviewMode, Nothing, 0)
                Dim sb As System.Text.StringBuilder = proc.ProcessBlock()

            Catch ex As FindTemplateException
                'Just pass exception, calls may not necessarily be a result of the main code itself
                Throw ex

            Catch ex As CallTemplateException
                'Just pass exception, calls may not necessarily be a result of the main code itself
                Throw ex

            Catch ex As ExecException
                Throw New Exception("Error in MAIN: " & ex.Message)

            Catch ex As Exception
                Throw New Exception("Error in MAIN: " & ex.Message)

            End Try
        End Sub

        Private Sub Proc_Notify(ByVal Message As String) Handles proc.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub proc_OutputWritten(ByVal Path As String) Handles proc.OutputWritten
            RaiseEvent OutputWritten(Path)
        End Sub

        Private Sub Proc_ConsoleOut(ByVal Message As String) Handles proc.ConsoleOut
            RaiseEvent ConsoleOut(Message)
        End Sub

        Private Sub Proc_ConsoleClear() Handles proc.ConsoleClear
            RaiseEvent ConsoleClear()
        End Sub

    End Class

End Namespace