Option Strict Off

Namespace UI

    Friend Class About
        Private sysIgnore As Boolean = True
        Private versionPage As New TabPage("Version History")

        Public Function GetCopyright() As String
            Dim Copyright As System.Reflection.AssemblyCopyrightAttribute = _
                System.Reflection.Assembly.GetExecutingAssembly.GetCustomAttributes(GetType(System.Reflection.AssemblyCopyrightAttribute), False)(0)
            Return Copyright.Copyright
        End Function

        Private Sub About_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.lblVersion.Text = "Version " & Application.ProductVersion

            Me.lblCopyright.Text = Me.GetCopyright()

            Me.grdComponents.Rows.Add(New String() {Globals.ASSEMBLY_NAME_METADRONE, Application.ProductVersion})
            Me.grdComponents.Rows.Add(New String() {Globals.ASSEMBLY_NAME_ICSHARPCODE, Globals.ASSEMBLY_VERSION_ICSHARPCODE}) 'Don't load assembly, because it's obfuscated

            For Each plugin In Globals.SourcePlugins.Plugins
                Me.grdPlugins.Rows.Add(New String() {plugin.AssemblyName, plugin.AssemblyVersion, IO.Path.GetFileName(plugin.Path)})
            Next

            For i As Integer = 0 To Me.grdComponents.Rows.Count - 1
                Me.grdComponents.Rows(i).Selected = False
            Next
            For i As Integer = 0 To Me.grdPlugins.Rows.Count - 1
                Me.grdPlugins.Rows(i).Selected = False
            Next
            Me.grdComponents.Rows(0).Selected = True
            Me.txtDetails.Text = Globals.ReadResource("Boston.ProductDetails.Metadrone.txt")
            Me.txtLicence.Text = Globals.ReadResource("Boston.LicenceDetails.Metadrone.txt")
            Dim rtxt As New RichTextBox()
            rtxt.Text = Globals.VersionHistory().ToString
            rtxt.ReadOnly = True
            rtxt.Dock = DockStyle.Fill
            rtxt.BackColor = System.Drawing.SystemColors.Window
            rtxt.ScrollBars = RichTextBoxScrollBars.Vertical
            Me.versionPage.Controls.Add(rtxt)
            Me.tcInfo.TabPages.Add(Me.versionPage)

            Me.sysIgnore = False
        End Sub

        Private Sub lnkDownload_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkDownload.LinkClicked
            Process.Start(Globals.METADRONE_URL)
        End Sub

        Private Sub grdComponents_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdComponents.SelectionChanged
            If Me.sysIgnore Then Exit Sub

            Me.sysIgnore = True

            For i As Integer = 0 To Me.grdPlugins.Rows.Count - 1
                Me.grdPlugins.Rows(i).Selected = False
            Next

            If Me.grdComponents.SelectedRows.Count = 0 Then Me.grdComponents.Rows(0).Selected = True
            If Me.grdComponents.SelectedRows(0).Cells(0).Value.Equals(Globals.ASSEMBLY_NAME_METADRONE) And _
               Me.grdComponents.SelectedRows(0).Cells(1).Value.Equals(Application.ProductVersion) Then
                Me.txtDetails.Text = Globals.ReadResource("Boston.ProductDetails.Metadrone.txt")
                Me.txtLicence.Text = Globals.ReadResource("Boston.LicenceDetails.Metadrone.txt")
                Me.tcInfo.TabPages.Add(Me.versionPage)

            ElseIf Me.grdComponents.SelectedRows(0).Cells(0).Value.Equals(Globals.ASSEMBLY_NAME_ICSHARPCODE) And _
                   Me.grdComponents.SelectedRows(0).Cells(1).Value.Equals(Globals.ASSEMBLY_VERSION_ICSHARPCODE) Then
                Me.txtDetails.Text = Globals.ReadResource("Boston.ProductDetails.ICSharpCodeTextEditor.txt")
                Me.txtLicence.Text = Globals.ReadResource("Boston.LicenceDetails.ICSharpCodeTextEditor.txt")
                Me.tcInfo.TabPages.Remove(Me.versionPage)

            End If
           
            Me.sysIgnore = False
        End Sub

        Private Sub grdPlugins_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdPlugins.SelectionChanged
            If Me.sysIgnore Then Exit Sub

            Me.sysIgnore = True

            For i As Integer = 0 To Me.grdComponents.Rows.Count - 1
                Me.grdComponents.Rows(i).Selected = False
            Next

            If Me.grdPlugins.SelectedRows.Count = 0 Then Me.grdPlugins.Rows(0).Selected = True
            For Each plugin In Globals.SourcePlugins.Plugins
                If plugin.AssemblyName.Equals(Me.grdPlugins.SelectedRows(0).Cells(0).Value) And _
                   plugin.AssemblyVersion.Equals(Me.grdPlugins.SelectedRows(0).Cells(1).Value) And _
                   IO.Path.GetFileName(plugin.Path).Equals(Me.grdPlugins.SelectedRows(0).Cells(2).Value) Then
                    Me.txtDetails.Text = plugin.PluginDescription.ProductInformation
                    Me.txtLicence.Text = plugin.PluginDescription.LicenceInformation
                    Exit For
                End If
            Next

            Me.sysIgnore = False
        End Sub

        Private Sub txtDetails_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles txtDetails.LinkClicked, _
                                                                                                                                 txtLicence.LinkClicked
            Process.Start(e.LinkText)
        End Sub

        Private Sub lnkDownload_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDownload.MouseEnter
            Me.lnkDownload.LinkColor = Color.CornflowerBlue
        End Sub

        Private Sub lnkDownload_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDownload.MouseLeave
            Me.lnkDownload.LinkColor = Color.RoyalBlue
        End Sub

    End Class

End Namespace