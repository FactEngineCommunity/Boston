Imports System.Runtime.InteropServices

Namespace UI

    Partial Friend Class MainForm
        Private WithEvents main As New Parser.Main()

        Private projectToBuild As New Persistence.MDProject()
        Private buildCompleted As Boolean = True

        <DllImport("user32.dll")> _
        Public Shared Function LockWindowUpdate(ByVal hWndLock As IntPtr) As Boolean
        End Function

        Private Sub HidePopup()
            If Me.tcMain.TabPages.Count > 1 Then
                If TypeOf Me.tcMain.TabPages(1).Controls(0) Is CodeEditor Then
                    Call CType(Me.tcMain.TabPages(1).Controls(0), CodeEditor).HidePopup(False)
                End If
            End If
        End Sub

        Private Sub GetPackages(ByVal prj As Persistence.MDProject)
            'Packages
            For Each pkg In prj.Packages
                Me.projectToBuild.Packages.Add(pkg)
            Next

            'Folders
            For Each fld In prj.Folders
                Call Me.GetPackages(fld)
            Next
        End Sub

        Private Sub GetPackages(ByVal folder As Persistence.ProjectFolder)
            'Packages
            For Each pkg In folder.Packages
                Me.projectToBuild.Packages.Add(pkg)
            Next

            'Sub folders
            For Each fld In folder.Folders
                Call Me.GetPackages(fld)
            Next
        End Sub

        Private Sub GetSources(ByVal prj As Persistence.MDProject)
            'Sources
            For Each src In prj.Sources
                Me.projectToBuild.Sources.Add(src)
            Next

            'Folders
            For Each fld In prj.Folders
                Call Me.GetSources(fld)
            Next
        End Sub

        Private Sub GetSources(ByVal folder As Persistence.ProjectFolder)
            'Sources
            For Each src In folder.Sources
                Me.projectToBuild.Sources.Add(src)
            Next

            'Sub folders
            For Each fld In folder.Folders
                Call Me.GetSources(fld)
            Next
        End Sub


        Private Sub btnBuild_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuild.Click
            Call Me.RunBuild()
        End Sub

        Private Sub RunBuild()

            Call Me.HidePopup()

            Me.txtResult.Clear()
            Me.txtResult.Refresh()
            Me.grdOutput.Rows.Clear()
            Me.buildCompleted = False
            Application.DoEvents()

            'Get project details
            Dim prj As Persistence.MDProject = Nothing

            'Transform from explorer
            Try
                Me.projectToBuild = Me.tvwExplorer.Transform(Me.tvwExplorer.tvwMain.Nodes(0))

            Catch ex As Exception
                Me.WriteResult("Critical error: " & ex.Message & System.Environment.NewLine)
                Exit Sub

            End Try

            'Load the object data
            If Not String.IsNullOrEmpty(Me.ProjectPath) Then
                Me.projectToBuild.Bin = Persistence.ProjectManager.Load(Me.ProjectPath).Bin
            End If

            'Build

            'Doubling up on a thread call will magically remove the focus off the 'build' button when pressed..
            If My.Application.EXEC_ParseInThread Then
                Dim t As New Threading.Thread(AddressOf Me.BuildProject)
                t.Start()
            Else
                Me.BuildProject()
            End If
        End Sub

        Private Sub BuildProject()
            Try
                Call Me.SetBuildMode(True)

                'Initialise parser
                Me.main = New Parser.Main()
                Me.main.SetInteractive(Me)
                Me.main.LoadProject(Me.projectToBuild)
                Me.main.ProjectPath = Me.ProjectPath

                'Begin parsing
                If My.Application.EXEC_ParseInThread Then
                    Dim t As New Threading.Thread(AddressOf Me.main.BuildProject)
                    t.Start()

                    While Not Me.buildCompleted
                        Application.DoEvents()
                    End While
                Else
                    Me.main.BuildProject()
                End If

                If Me.main.BuildError IsNot Nothing Then Throw Me.main.BuildError

            Catch ex As Exception
                Me.WriteResult(System.Environment.NewLine & "Error encountered. " & ex.Message & System.Environment.NewLine)

            Finally
                Call Me.SetBuildMode(False)

            End Try
        End Sub

        Private Sub SetBuildMode(ByVal Building As Boolean)
            Me.tvwExplorer.tvwMain.Enabled = Not Building
            Me.btnNew.Enabled = Not Building
            Me.btnOpen.Enabled = Not Building
            Me.btnSave.Enabled = Not Building
            For Each tab As TabPage In Me.tcMain.TabPages
                If tab.Controls.Count = 0 Then Continue For
                If Not TypeOf tab.Controls(0) Is CodeEditor Then Continue For
                CType(tab.Controls(0), CodeEditor).ReadOnly = Building
            Next
            Me.btnBuild.Enabled = Not Building
            Me.btnCancel.Enabled = Building
        End Sub

        'Check tag in tab pages editor control (main, templates, etc)
        'Return index of tab page
        Friend Function TagFoundInTabPages(ByVal Tag As Boston.Persistence.IEditorItem) As Integer
            For i As Integer = 0 To Me.tcMain.TabPages.Count - 1
                If Me.tcMain.TabPages(i).Controls.Count = 0 Then Continue For
                If Me.tcMain.TabPages(i).Controls(0).Tag Is Nothing Then Continue For
                If Not TypeOf Me.tcMain.TabPages(i).Controls(0).Tag Is Boston.Persistence.IEditorItem Then Continue For

                If CType(Me.tcMain.TabPages(i).Controls(0).Tag, Boston.Persistence.IEditorItem).EditorGUID.Equals(Tag.EditorGUID) Then
                    Return i
                End If
            Next

            Return -1
        End Function

        Private Sub WriteResult(ByVal text As String)
            Try
                LockWindowUpdate(Me.txtResult.Handle)
                Me.txtResult.AppendText(text)
                Me.txtResult.ScrollToCaret()
                'Application.DoEvents() '20200718-VM-Commented this out and is much faster.
            Finally
                LockWindowUpdate(IntPtr.Zero)
            End Try
        End Sub

        Private Sub WriteOutput(ByVal text As String)
            'Get file info
            Try
                Dim fi As New IO.FileInfo(text)
                Dim row(2) As String
                row(0) = fi.FullName
                Dim size As Integer = CInt(fi.Length / 1000)
                If size = 0 Then size = 1
                row(1) = size.ToString("#,###") & " KB"
                row(2) = fi.LastWriteTime.ToString
                Me.grdOutput.Rows.Add(row)

            Catch ex As Exception
                'fi fail. just add path
                Dim row(2) As String
                row(0) = text
                Me.grdOutput.Rows.Add(row)

            End Try
        End Sub

        Private Sub WriteConsole(ByVal text As String)
            'We're having issues here. On the first run nothing appears in the text box (threaded).
            'But doing it this round-about way bypasses this weird problem.
            Try
                'LockWindowUpdate(Me.txtConsole.Handle)
                'Me.txtConsole.AppendText(text)
                Dim sb As New System.Text.StringBuilder(Me.txtConsole.Text)
                sb.Append(text)
                Me.txtConsole.Text = sb.ToString
            Finally
                'LockWindowUpdate(IntPtr.Zero)
                Me.txtConsole.SelectionStart = Me.txtConsole.TextLength
                Me.txtConsole.SelectionLength = 0
                Me.txtConsole.ScrollToCaret()
                Me.txtConsole.Refresh()
                'Application.DoEvents() '20200718-VM-Commented this out.
            End Try
        End Sub

        Private Sub main_BuildCompleted() Handles main.BuildCompleted
            Me.buildCompleted = True
        End Sub

        Private Sub packageParser_Notify(ByVal Message As String) Handles main.Notify
            Me.WriteResult(Message)
        End Sub

        Private Sub packageParser_OutputWritten(ByVal Path As String) Handles main.OutputWritten
            Me.WriteOutput(Path)
        End Sub

        Private Sub main_ConsoleOut(ByVal Message As String) Handles main.ConsoleOut
            Me.WriteConsole(Message)
        End Sub

        Private Sub main_ConsoleClear() Handles main.ConsoleClear
            Me.txtConsole.Clear()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.main.CancelBuild()
        End Sub

    End Class

End Namespace