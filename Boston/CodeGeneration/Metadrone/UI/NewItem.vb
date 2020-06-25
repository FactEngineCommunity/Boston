Imports System.Windows.Forms

Namespace UI

    Friend Class NewItem
        Private FirstTime As Boolean = False
        Private mType As Types = Types.Project
        Private CurrentPackage As Persistence.MDPackage = Nothing
        Private CurrentPackageList As New List(Of String)
        Private CurrentSourceList As List(Of Persistence.Source) = Nothing

        Private downloadedTemplate As System.Text.StringBuilder = Nothing

        Public Enum Types
            Project = 0
            ProjectChild = 1
            PackageChild = 2
            PackageOnly = 3
            SourceOnly = 4
            TemplateOnly = 5
            VBCodeOnly = 6
            CSCodeOnly = 7
        End Enum

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.rbPackage.Checked = True
        End Sub

        Public Sub New(ByVal Type As Types, ByVal CurrentPackage As Persistence.MDPackage, _
                       ByVal CurrentPackageList As List(Of String), _
                       ByVal CurrentSourceList As List(Of Persistence.Source), _
                       ByVal InitialTemplateText As System.Text.StringBuilder)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.Type = Type
            Me.CurrentPackage = CurrentPackage
            Me.CurrentPackageList = CurrentPackageList
            Me.CurrentSourceList = CurrentSourceList
            If InitialTemplateText IsNot Nothing Then
                Me.downloadedTemplate = InitialTemplateText
            End If
        End Sub

        Public Property Type() As Types
            Get
                Return Me.mType
            End Get
            Set(ByVal value As Types)
                Me.mType = value

                Me.rbPackage.Text = "Package"

                Select Case Me.mType
                    Case NewItem.Types.Project
                        Me.rbPackage.Checked = True
                        Me.picProject.Visible = True
                        Me.picPackage.Visible = False
                        Me.rbDBSource.Visible = False
                        Me.picDBSource.Visible = False
                        Me.rbTemplate.Visible = False
                        Me.picTemplate.Visible = False
                        Me.rbVB.Visible = False
                        Me.picVB.Visible = False
                        Me.rbCS.Visible = False
                        Me.picCS.Visible = False
                        Me.chkNewProject.Visible = False

                        Me.rbPackage.Text = "Project"

                        Dim lbltop As Integer = Me.lblPackageName.Top
                        Dim txttop As Integer = Me.txtPackageName.Top
                        Me.lblPackageName.Top = Me.lblProjectName.Top
                        Me.txtPackageName.Top = Me.txtProjectName.Top
                        Me.lblProjectName.Top = lbltop
                        Me.txtProjectName.Top = txttop
                        Me.txtPackageName.TabIndex = Me.txtProjectName.TabIndex + 1

                    Case NewItem.Types.ProjectChild
                        Me.rbPackage.Checked = True
                        Me.picProject.Visible = False
                        Me.picPackage.Visible = True
                        Me.lblProjectName.Visible = False
                        Me.txtProjectName.Visible = False
                        Me.chkNewProject.Visible = True
                        Me.picTemplate.Visible = False
                        Me.rbTemplate.Visible = False
                        Me.rbVB.Visible = False
                        Me.picVB.Visible = False
                        Me.rbCS.Visible = False
                        Me.picCS.Visible = False

                    Case NewItem.Types.PackageChild
                        Me.rbPackage.Checked = True
                        Me.picProject.Visible = False
                        Me.picPackage.Visible = True
                        Me.lblProjectName.Visible = False
                        Me.txtProjectName.Visible = False
                        Me.chkNewProject.Visible = True
                        Me.picTemplate.Visible = True
                        Me.rbTemplate.Visible = True
                        Me.rbVB.Visible = True
                        Me.picVB.Visible = True
                        Me.rbCS.Visible = True
                        Me.picCS.Visible = True

                    Case NewItem.Types.PackageOnly
                        Me.rbPackage.Checked = True
                        Me.picProject.Visible = False
                        Me.picPackage.Visible = True
                        Me.rbDBSource.Visible = False
                        Me.picDBSource.Visible = False
                        Me.rbTemplate.Visible = False
                        Me.picTemplate.Visible = False
                        Me.rbVB.Visible = False
                        Me.picVB.Visible = False
                        Me.rbCS.Visible = False
                        Me.picCS.Visible = False
                        Me.chkNewProject.Visible = False
                        Me.lblProjectName.Visible = False
                        Me.txtProjectName.Visible = False

                    Case NewItem.Types.SourceOnly
                        Me.rbDBSource.Checked = True
                        Me.rbPackage.Visible = False
                        Me.picProject.Visible = False
                        Me.picPackage.Visible = False
                        Me.rbDBSource.Visible = True
                        Me.picDBSource.Visible = True
                        Me.rbDBSource.Top = Me.rbPackage.Top
                        Me.picDBSource.Top = Me.picPackage.Top
                        Me.rbTemplate.Visible = False
                        Me.picTemplate.Visible = False
                        Me.rbVB.Visible = False
                        Me.picVB.Visible = False
                        Me.rbCS.Visible = False
                        Me.picCS.Visible = False
                        Me.chkNewProject.Visible = False
                        Me.lblProjectName.Visible = False
                        Me.txtProjectName.Visible = False

                    Case NewItem.Types.TemplateOnly
                        Me.rbTemplate.Checked = True
                        Me.rbPackage.Visible = False
                        Me.picProject.Visible = False
                        Me.picPackage.Visible = False
                        Me.rbDBSource.Visible = False
                        Me.picDBSource.Visible = False
                        Me.rbTemplate.Visible = True
                        Me.picTemplate.Visible = True
                        Me.rbTemplate.Top = Me.rbPackage.Top
                        Me.picTemplate.Top = Me.picPackage.Top
                        Me.rbVB.Visible = False
                        Me.picVB.Visible = False
                        Me.rbCS.Visible = False
                        Me.picCS.Visible = False
                        Me.chkNewProject.Visible = False
                        Me.lblProjectName.Visible = False
                        Me.txtProjectName.Visible = False

                    Case NewItem.Types.VBCodeOnly
                        Me.rbPackage.Visible = False
                        Me.picProject.Visible = False
                        Me.picPackage.Visible = False
                        Me.rbDBSource.Visible = False
                        Me.picDBSource.Visible = False
                        Me.rbTemplate.Visible = False
                        Me.picTemplate.Visible = False
                        Me.rbVB.Checked = True
                        Me.rbVB.Visible = True
                        Me.picVB.Visible = True
                        Me.rbVB.Top = Me.rbPackage.Top
                        Me.picVB.Top = Me.picPackage.Top
                        Me.rbCS.Visible = False
                        Me.picCS.Visible = False
                        Me.chkNewProject.Visible = False
                        Me.lblProjectName.Visible = False
                        Me.txtProjectName.Visible = False

                    Case NewItem.Types.CSCodeOnly
                        Me.rbPackage.Visible = False
                        Me.picProject.Visible = False
                        Me.picPackage.Visible = False
                        Me.rbDBSource.Visible = False
                        Me.picDBSource.Visible = False
                        Me.rbTemplate.Visible = False
                        Me.picTemplate.Visible = False
                        Me.rbVB.Visible = False
                        Me.picVB.Visible = False
                        Me.rbCS.Checked = True
                        Me.rbCS.Visible = True
                        Me.picCS.Visible = True
                        Me.rbCS.Top = Me.rbPackage.Top
                        Me.picCS.Top = Me.picPackage.Top
                        Me.chkNewProject.Visible = False
                        Me.lblProjectName.Visible = False
                        Me.txtProjectName.Visible = False

                End Select
            End Set
        End Property

        Public ReadOnly Property DownloadedTemplateText() As System.Text.StringBuilder
            Get
                Return Me.downloadedTemplate
            End Get
        End Property

        Private Sub NewItem_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
            If Me.FirstTime Then Exit Sub
            Me.FirstTime = True
            Call Me.Setup()
        End Sub

        Private Sub NewItem_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.pnlTemplate.Location = Me.pnlProject.Location
            Me.pnlDBSource.Location = Me.pnlProject.Location
            Me.pnlCodeFile.Location = Me.pnlProject.Location

            Call Me.Setup()
        End Sub

        Public Sub Setup()
            Me.txtProjectName.Text = "Project"
            Me.txtPackageName.Text = "Package"
            Me.txtTemplate.Text = "Template"
            Me.txtDBSourceName.Text = "Source"
            Me.txtCodeFileName.Text = "Code"

            Me.pnlProject.Visible = False
            Me.pnlTemplate.Visible = False
            Me.pnlDBSource.Visible = False
            Me.pnlCodeFile.Visible = False

            If Me.rbPackage.Checked Then
                Me.pnlProject.Visible = True
                Me.pnlProject.Dock = DockStyle.Fill
                If Me.Type = Types.Project Then
                    Me.txtProjectName.Focus()
                Else
                    Me.txtPackageName.Focus()
                End If

            ElseIf Me.rbTemplate.Checked Then
                Me.pnlTemplate.Visible = True
                Me.pnlTemplate.Dock = DockStyle.Fill
                Me.txtTemplate.Focus()

            ElseIf Me.rbDBSource.Checked Then
                Me.pnlDBSource.Visible = True
                Me.pnlDBSource.Dock = DockStyle.Fill
                Me.txtDBSourceName.Focus()

            ElseIf Me.rbVB.Checked Then
                Me.pnlCodeFile.Visible = True
                Me.pnlCodeFile.Dock = DockStyle.Fill
                Me.txtCodeFileName.Text = "VB"
                Me.txtCodeFileName.Focus()

            ElseIf Me.rbCS.Checked Then
                Me.pnlCodeFile.Visible = True
                Me.pnlCodeFile.Dock = DockStyle.Fill
                Me.txtCodeFileName.Text = "CS"
                Me.txtCodeFileName.Focus()

            End If

            Call Me.SetupHelp()
        End Sub

        Private Sub SetupHelp()
            If Me.rbPackage.Checked Then
                Dim sb As New System.Text.StringBuilder("")
                If Me.txtProjectName.Visible Then
                    sb.AppendLine("Create a new project and a default new package for it. ")
                    sb.AppendLine()
                    sb.Append("If a project is already open it will be closed. ")
                    sb.Append("A project can have multiple packages to help break up code generation tasks.")
                    Me.lblDescription.Text = sb.ToString
                Else
                    sb.AppendLine("Add a new package to this existing project. ")
                    If Me.chkNewProject.Visible Then
                        sb.AppendLine()
                        sb.Append("Check 'create new project' to close this project and create a new one. ")
                    End If
                    Me.lblDescription.Text = sb.ToString
                End If

            ElseIf Me.rbTemplate.Checked Then
                Dim sb As New System.Text.StringBuilder("")
                sb.AppendLine("Add a new template to this package. ")
                sb.AppendLine()
                sb.Append("A templates is where the code generation really happens. ")
                sb.Append("You can have as many templates as you want, and each are responsible for single file output ")
                sb.AppendLine("or multiple files in a single directory.")
                sb.AppendLine()
                sb.Append("Call a template via 'Main' in its package. Templates can also call other templates. ")
                Me.lblDescription.Text = sb.ToString

            ElseIf Me.rbDBSource.Checked Then
                Dim sb As New System.Text.StringBuilder("")
                sb.AppendLine("Add a new meta data source to this project. ")
                sb.AppendLine()
                sb.Append("A source is a connection to a database. ")
                sb.Append("Refer to a source in your template to access the meta data such as tables and columns ")
                sb.Append("related to that database connection.")
                Me.lblDescription.Text = sb.ToString

            ElseIf Me.rbVB.Checked Then
                Dim sb As New System.Text.StringBuilder("")
                sb.AppendLine("Add a new Visual Basic code file to this package. ")
                sb.AppendLine()
                sb.Append("This is managed code using the .Net framework for special logic that the Metadrone syntax cannot support. ")
                sb.Append(".Net code is called either in 'Main' in its package or from within a template.")
                Me.lblDescription.Text = sb.ToString

            ElseIf Me.rbCS.Checked Then
                Dim sb As New System.Text.StringBuilder("")
                sb.AppendLine("Add a new C# code file to this package. ")
                sb.AppendLine()
                sb.Append("This is managed code using the .Net framework for special logic that the Metadrone syntax cannot support. ")
                sb.Append(".Net code is called either in 'Main' in its package or from within a template.")
                Me.lblDescription.Text = sb.ToString

            End If
        End Sub

        Public Function IsDuplicate_Template(ByVal templateName As String) As Boolean
            For Each t In Me.CurrentPackage.TemplateList
                If t.Equals(templateName, StringComparison.CurrentCultureIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function IsDuplicate_Package(ByVal packageName As String) As Boolean
            For Each pkg In Me.CurrentPackageList
                If pkg.Equals(packageName, StringComparison.CurrentCultureIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function IsDuplicate_Source(ByVal sourceName As String) As Boolean
            For Each src In Me.CurrentSourceList
                If src.Name.Equals(sourceName, StringComparison.CurrentCultureIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
            Try
                If Not Persistence.MDProject.CheckValidName(Me.txtProjectName.Text) Then Throw New Exception("Invalid project name.")
                If Not Persistence.MDProject.CheckValidName(Me.txtPackageName.Text) Then Throw New Exception("Invalid package name.")
                If Not Persistence.MDProject.CheckValidName(Me.txtTemplate.Text) Then Throw New Exception("Invalid template name.")
                If Not Persistence.MDProject.CheckValidName(Me.txtDBSourceName.Text) Then Throw New Exception("Invalid source name.")

                If Me.rbPackage.Checked And Me.CurrentPackageList IsNot Nothing Then
                    If Me.IsDuplicate_Package(Me.txtPackageName.Text) Then
                        Throw New Exception("Package '" & Me.txtPackageName.Text & "' already defined.")
                    End If

                ElseIf Me.rbDBSource.Checked And Me.CurrentSourceList IsNot Nothing Then
                    If Me.IsDuplicate_Source(Me.txtDBSourceName.Text) Then
                        Throw New Exception("Source '" & Me.txtDBSourceName.Text & "' already defined.")
                    End If

                ElseIf Me.rbTemplate.Checked And Me.CurrentPackage IsNot Nothing Then
                    If Me.IsDuplicate_Template(Me.txtTemplate.Text) Then
                        Throw New Exception("Template '" & Me.txtTemplate.Text & "' already defined.")
                    End If

                End If

            Catch ex As Exception
                Me.lblMsg.Text = ex.Message
                Exit Sub

            End Try

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End Sub

        Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub rb_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbPackage.CheckedChanged, _
                                                                                                   rbDBSource.CheckedChanged, _
                                                                                                   rbTemplate.CheckedChanged, _
                                                                                                   rbVB.CheckedChanged, _
                                                                                                   rbCS.CheckedChanged
            Call Me.Setup()
        End Sub

        Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProjectName.GotFocus, txtPackageName.GotFocus, _
                                                                                              txtTemplate.GotFocus
            CType(sender, TextBox).SelectAll()
        End Sub

        Private Sub chkNewProject_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNewProject.CheckedChanged
            Me.lblProjectName.Visible = Me.chkNewProject.Checked
            Me.txtProjectName.Visible = Me.chkNewProject.Checked
            If Me.chkNewProject.Checked Then
                Me.picProject.Visible = True
                Me.picPackage.Visible = False
                Me.rbPackage.Text = "Project"
            Else
                Me.picProject.Visible = False
                Me.picPackage.Visible = True
                Me.rbPackage.Text = "Package"
            End If
            Call Me.SetupHelp()
        End Sub

    End Class

End Namespace