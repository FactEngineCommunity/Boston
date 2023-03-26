Public Class SearchTextbox

    Public Event InitiateSearch(ByVal asSearchString As String)
    Public Event TextBoxCleared()

    Private Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonClear.Click
        Me.TextBox.Clear()
        RaiseEvent TextBoxCleared()
    End Sub

    Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox.KeyDown

        If e.KeyCode = Keys.Enter Then
            RaiseEvent InitiateSearch(Trim(Me.TextBox.Text))
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RaiseEvent InitiateSearch(Trim(Me.TextBox.Text))
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox.TextChanged

        If Trim(Me.TextBox.Text) = "" Then
            RaiseEvent TextBoxCleared()
        End If

    End Sub

End Class
