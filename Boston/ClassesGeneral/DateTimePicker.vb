Public Class DateTimePicker

    Public Event DateSelected(ByVal adDate As Date)

    Private Sub ButtonSelect_Click(sender As Object, e As EventArgs) Handles ButtonSelect.Click

        RaiseEvent DateSelected(Me.DateTimePicker1.Value)

    End Sub

End Class
