Public Class frmCRUDAddGroup

    Private zrGroup As New ClientServer.Group


    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        If Me.checkFields() Then
            Call Me.getFields(Me.zrGroup)
            Call tableClientServerGroup.addGroup(Me.zrGroup)

            Me.Hide()
            Me.Close()
            Me.Dispose()

            If MsgBox("Would you like to invite/add Users to the Group now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                frmMain.LoadCRUDEditGroup(Me.zrGroup)
            End If
        End If

    End Sub

    Private Function checkFields() As Boolean

        checkFields = True

        If Trim(Me.TextBoxGroupName.Text) = "" Then
            checkFields = False
        End If

    End Function

    Private Sub getFields(ByRef arGroup As ClientServer.Group)

        arGroup.Name = Trim(Me.TextBoxGroupName.Text)
        arGroup.CreatedByUser = prApplication.User

    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub frmCRUDAddGroup_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Me.setupForm()

    End Sub

    Private Sub setupForm()

        Me.ErrorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink

    End Sub

    Private Sub TextBoxGroupName_TextChanged(sender As Object, e As EventArgs) Handles TextBoxGroupName.TextChanged

        If (Me.TextBoxGroupName.Text) = "" Then
            Me.ErrorProvider.SetError(Me.TextBoxGroupName, "Name is mandatory.")
        Else
            Me.ErrorProvider.Clear()
        End If

    End Sub
End Class