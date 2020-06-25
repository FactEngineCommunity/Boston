Namespace UI

    Public Class QueryResults

        Private ReadyToLoad As Boolean = False

        Public Property Messages() As String
            Get
                Return Me.txtMessages.Text
            End Get
            Set(ByVal value As String)
                Me.txtMessages.Text = value
                Me.txtMessages.Refresh()
            End Set
        End Property

        Public Sub PrepareSourceLoad()
            Me.grdResults.DataSource = Nothing
            Me.TabControl1.SelectedIndex = 1

            Me.Messages = "Executing query..." & System.Environment.NewLine

            Me.ReadyToLoad = True
        End Sub

        Public Sub SetSource(ByVal Source As Object)
            If Not Me.ReadyToLoad Then Throw New Exception("Must prepare first call: QueryResults.PrepareSourceLoad().")
            Try
                Me.grdResults.DataSource = Source

                Me.Messages &= Me.grdResults.RowCount.ToString & " row(s)."
                Me.TabControl1.SelectedIndex = 0

            Catch ex As Exception
                Me.Messages = ex.Message

            End Try
        End Sub

    End Class

End Namespace