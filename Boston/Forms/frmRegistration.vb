Imports System.Reflection

Public Class frmRegistration

    Private msApplicationKey As String
    Private msRegistrationKey As String
    Private mbIsRegistered As Boolean = False

    Public Overloads Function ShowDialog() As Boolean
        MyBase.ShowDialog()
        Return Me.mbIsRegistered
    End Function

    Private Sub frmRegistration_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.msApplicationKey = TableReferenceFieldValue.GetReferenceFieldValue(34, 1)
        Dim lsRegistrationKey As String = TableReferenceFieldValue.GetReferenceFieldValue(34, 2)
        Dim lsDefaultRegistrationKey As String = TableReferenceFieldValue.GetReferenceFieldValue(34, 3)

        Me.LabelApplicationKey.Text = Me.msApplicationKey
        Me.TextBoxRegistrationKey.Text = lsRegistrationKey

        Call Me.ShowRegistrationInformation

    End Sub

    Private Sub ShowRegistrationInformation()

        Dim lsMessage As String = ""
        Dim lrRegistrationResult As New tRegistrationResult

        Try
            Me.msApplicationKey = TableReferenceFieldValue.GetReferenceFieldValue(34, 1)
            Dim lsRegistrationKey As String = TableReferenceFieldValue.GetReferenceFieldValue(34, 2)
            Dim lsDefaultRegistrationKey As String = TableReferenceFieldValue.GetReferenceFieldValue(34, 3)

            Me.mbIsRegistered = publicRegistration.CheckRegistration(Me.msApplicationKey, lsRegistrationKey, lrRegistrationResult, lsDefaultRegistrationKey)

            If lrRegistrationResult.IsRegistered = False Then
                lsMessage = "Please send the Application Key to FactEngine to get a valid Registration Key. (support@factengine.ai)" & vbCrLf
            End If

            lsMessage &= "Registered: " & lrRegistrationResult.IsRegistered.ToString & vbCrLf
            lsMessage.AppendLine("Software Type: " & lrRegistrationResult.SoftwareType)
            lsMessage.AppendLine("Subscription Type:" & lrRegistrationResult.SubscriptionType)
            lsMessage.AppendLine("Registered To Date: " & lrRegistrationResult.RegisteredToDate)

            Me.LabelRegistrationStatus.ForeColor = Boston.returnIfTrue(lrRegistrationResult.IsRegistered, Color.MediumSeaGreen, Color.Black)

            Me.LabelRegistrationStatus.Text = lsMessage

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub ButtonSaveRegistrationKey_Click(sender As Object, e As EventArgs) Handles ButtonSaveRegistrationKey.Click

        Dim lsMessage As String = ""

        Try
            'CodeSafe
            If Trim(Me.TextBoxRegistrationKey.Text) = "" Then Exit Sub

            Dim lrRegistrationResult As tRegistrationResult = Nothing

            If MsgBox("Are you sure that you want to change your Registration Key?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                'Get the Default Registration Key. Is designed such that a Subscription defaults to the Default Registration Key.
                '  This is so Subscriptions don't default to Not Registered. FactEngine has perpetual registration.
                lrRegistrationResult = New tRegistrationResult
                Me.mbIsRegistered = publicRegistration.CheckRegistration(Me.msApplicationKey, Trim(TextBoxRegistrationKey.Text), lrRegistrationResult, "")

                Dim lrReferenceFieldValue As tReferenceFieldValue

                If Not lrRegistrationResult.IsRegistered Then
                    MsgBox("That's not a valid registration key.")

                    lsMessage = "Registered: " & lrRegistrationResult.IsRegistered.ToString & vbCrLf
                    lsMessage.AppendLine("Software Type: " & lrRegistrationResult.SoftwareType)
                    lsMessage.AppendLine("Subscription Type:" & lrRegistrationResult.SubscriptionType)
                    lsMessage.AppendLine("Registered To Date: " & lrRegistrationResult.RegisteredToDate)

                    Me.LabelRegistrationStatus.Text = lsMessage
                    Exit Sub
                End If

                Dim lsProductIdentifier As String
                If lrRegistrationResult.SubscriptionType = "Trial" Then
                    lsProductIdentifier = publicRegistration.FormatProductIdentifier(Me.msApplicationKey, "Professional", "Trial", "None")
                Else
                    lsProductIdentifier = publicRegistration.FormatProductIdentifier(Me.msApplicationKey, "Professional", "None", "Perpetual")
                End If
                Dim lsDefaultRegistrationKey = publicRegistration.FormatLicenseKey(GetMd5Sum(lsProductIdentifier))

                lrReferenceFieldValue = New tReferenceFieldValue(34, 3, 1, lsDefaultRegistrationKey)
                lrReferenceFieldValue.Save()

                lrReferenceFieldValue = New tReferenceFieldValue(34, 2, 1, Trim(TextBoxRegistrationKey.Text))
                Call lrReferenceFieldValue.Save()

                Call Me.ShowRegistrationInformation()
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonCopyModelIdToClipboard_Click(sender As Object, e As EventArgs) Handles ButtonCopyModelIdToClipboard.Click

        Try
            System.Windows.Forms.Clipboard.SetText(Me.msApplicationKey)

            Dim lfrmFlashCard As New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = 1500
            lfrmFlashCard.zsText = "Saved to clipboard."
            lfrmFlashCard.Show(frmMain, "White")

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

End Class