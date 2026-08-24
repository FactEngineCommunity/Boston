Imports System.Reflection

Public Class frmLogin

    Private mbUsingLastLoginPassword As Boolean = False
    Private msLastLoginPasswordHash As String = Nothing

    Private Sub login_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '----------------------------
        'Centre the form
        '----------------------------
        Try
            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
            Me.Top = (Screen.PrimaryScreen.WorkingArea.Height / 3) - (Me.Height / 3)

            Dim lsIPAddress As String = "ImpossibleIPAddress"

#Region "Users Last Login from IP Address"
            If prThinfinity IsNot Nothing AndAlso prThinfinity.BrowserInfo IsNot Nothing Then

                Try
                    If prThinfinity.BrowserInfo.UniqueBrowserId.Trim <> "" Then 'IPAddress.Trim <> "" Then
                        lsIPAddress = prThinfinity.BrowserInfo.UniqueBrowserId 'IPAddress
                    Else
                        Exit Sub
                    End If
                Catch ex As Exception
                    'Thinfinity might be configured to be used, but run from the desktop (i.e. e.g. Run from the vitual machine hosting Boston).
                    Exit Sub
                End Try

                Dim lrUser As ClientServer.User = tableClientServerLog.GetLastLogonUsernamePasswordFromBrowserId(lsIPAddress)

                If lrUser IsNot Nothing AndAlso lrUser.Username.Trim <> "" Then
                    RemoveHandler Me.TextBox_password.TextChanged, AddressOf Me.TextBox_password_TextChanged
                    RemoveHandler Me.TextBox_username.TextChanged, AddressOf Me.TextBox_username_TextChanged
                    Me.TextBox_username.Text = lrUser.Username
                    Me.TextBox_password.Text = "DummyValue"
                    AddHandler Me.TextBox_password.TextChanged, AddressOf Me.TextBox_password_TextChanged
                    AddHandler Me.TextBox_username.TextChanged, AddressOf Me.TextBox_username_TextChanged

                    Me.mbUsingLastLoginPassword = True
                    Me.msLastLoginPasswordHash = lrUser.PasswordHash
                End If
            End If
#End Region

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub button_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_cancel.Click

        Try
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
            Me.Dispose()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        Dim lsUsername As String
        Dim lsPassword As String


        Try

            lsUsername = LTrim(RTrim(TextBox_username.Text))
            lsPassword = LTrim(RTrim(TextBox_password.Text))

            'CodeSafe
            If lsUsername = "" Or lsPassword = "" Then GoTo InvalidUsernamePassword

            Dim lsPasswordHash = ClientServer.getHash(lsPassword)

            Dim lbIsValidUser As Boolean = False

            lbIsValidUser = tableClientServerUser.IsValidUser(lsUsername, lsPasswordHash, Me.msLastLoginPasswordHash)

            If lbIsValidUser Then

                'g_operator_login_used = True
                'g_user_name = lsUsername
                prApplication.User = New ClientServer.User
                Call tableClientServerUser.getUserDetailsByUsername(lsUsername, prApplication.User)

                prApplication.User.IsLoggedIn = True

                Me.Hide()

                If prApplication.User.ResetPassword Then
                    MsgBox("Please change your password to something new.")
                    'lfrm_edit_operator_frm.ShowDialog()
                End If

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

            Else
InvalidUsernamePassword:
                MsgBox("Invalid Username or Password. Try another combination.")
                TextBox_username.Focus()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub login_frm_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Validating

        Try
            e.Cancel = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBox_password_TextChanged(sender As Object, e As EventArgs) Handles TextBox_password.TextChanged

        Try
            Me.msLastLoginPasswordHash = Nothing
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBox_username_TextChanged(sender As Object, e As EventArgs) Handles TextBox_username.TextChanged

        Try
            Me.msLastLoginPasswordHash = Nothing
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function check_fields() As Boolean

        Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Function


End Class