Public Class frmSplash

    Public msAssemblyFileVersionNumber As String = Nothing

    Private Sub title_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.TopMost = True
        '-------------------------------
        'Centre the form
        '-------------------------------
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height / 2) - (Me.Height / 2)

        Call SetupForm()


    End Sub

    Private Sub SetupForm()

        'Original colour scheme.
        '150, 163, 165
        'SteelBlue()

        Dim ls_message As String = ""

        If prSoftwareCategory = pcenumSoftwareCategory.Student Then
            Me.LabelSoftwareCategory.Text = "Student"
        End If

        ls_message = "Boston version: v" & psApplicationApplicationVersionNr
        If Me.msAssemblyFileVersionNumber IsNot Nothing Then
            ls_message.AppendString(" (Assembly: " & Me.msAssemblyFileVersionNumber & ")" & vbCrLf)
        Else
            ls_message &= vbCrLf
        End If

        ls_message &= "Required Boston database version: v" & psApplicationDatabaseVersionNr

        label_splash.Text = ls_message

        Me.PictureboxSplash.Image = My.Resources.Splash.Shutterstock_267251567

        'Dim lo_random As New Random
        'Dim li_random As Integer = lo_random.Next(1, 5)

        'Select Case li_random
        '    Case Is = 1
        '        Me.PictureboxSplash.Image = My.Resources.Splash.Shutterstock_267251567
        '    Case Is = 2
        '        Me.PictureboxSplash.Image = My.Resources.Splash.Shutterstock_218779708
        '    Case Is = 3
        '        Me.PictureboxSplash.Image = My.Resources.Splash.Shutterstock_218779708
        '    Case Is = 4
        '        Me.PictureboxSplash.Image = My.Resources.Splash.Shutterstock_267251567
        '    Case Is = 5
        '        Me.PictureboxSplash.Image = My.Resources.Splash.Shutterstock_267251567
        'End Select

    End Sub

    Private Sub time_close_form_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles time_close_form.Tick

        Me.Close()

    End Sub

End Class