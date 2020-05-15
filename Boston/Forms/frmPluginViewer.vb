Public Class frmPluginViewer

    Private Sub button_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_close.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub plugin_viewer_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call setup_form()

    End Sub

    Sub setup_form()

        Call load_installed_plugins()

    End Sub

    Sub load_installed_plugins()

        Dim li_ind As Integer

        If prApplication.Plugins IsNot Nothing Then
            For li_ind = 1 To prApplication.Plugins.Count
                listbox_plugin_name.Items.Add(prApplication.Plugins(li_ind - 1).PluginObject.Name)
            Next li_ind
        Else
            Me.listbox_plugin_name.Items.Add("There are no Plugins loaded into Boston.")
        End If

    End Sub


    Private Sub listbox_plugin_name_DoubleClick(sender As Object, e As EventArgs) Handles listbox_plugin_name.DoubleClick

        If listbox_plugin_name.SelectedIndex <> -1 Then
            Call prApplication.Plugins(listbox_plugin_name.SelectedIndex).PluginObject.Setup()
        End If

    End Sub

    Private Sub ShowPluginFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowPluginFormToolStripMenuItem.Click

        If listbox_plugin_name.SelectedIndex <> -1 Then
            Call prApplication.Plugins(listbox_plugin_name.SelectedIndex).PluginObject.ShowForm()
        End If

    End Sub

End Class