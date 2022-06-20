Namespace UI

    Friend Class CodeEditor
        Private Const TAB_REPLACE As String = "    "
        Private Default_Font As New Font("Lucida Console", 9)
        Private NoShowPopup As Boolean = False

        Private SysTextChange As Boolean = False
        Protected Friend ToggleHilightOn As Boolean = False

        Public WithEvents txtBox As txtBox = Nothing

        Public Shadows Event TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event Save()
        Public Event Run()
        Public Event SearchRequested()
        Public Event SearchReset()

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Try
                'Use AvalonEdit. Scintilla is a pain to configure.
                'Use ICSharpCode.TextEditor, AvalonEdit is not ready yet.
                Me.txtBox = New txtBox(UI.txtBox.HighlightModes.MetadroneTemplate)
                Me.txtBox.Dock = DockStyle.Fill
                Me.txtBox.ShowLineNumbers = True
                Me.pnlMain.Controls.Add(Me.txtBox)

            Catch ex As Exception
                'We won't add any code editor control.

            End Try
        End Sub

        Public Sub New(ByVal HighlightMode As UI.txtBox.HighlightModes)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Try
                Me.txtBox = New txtBox(HighlightMode)
                Me.txtBox.Dock = DockStyle.Fill
                Me.txtBox.ShowLineNumbers = True
                Me.pnlMain.Controls.Add(Me.txtBox)

            Catch ex As Exception
                'We won't add any code editor control.

            End Try
        End Sub

        Public Property NoPopup() As Boolean
            Get
                Return Me.NoShowPopup
            End Get
            Set(ByVal value As Boolean)
                Me.NoShowPopup = value
            End Set
        End Property

        Private Sub CodeEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Set tags from properties
            Try
                Dim props As Persistence.Properties = Me.GetProperties()
                Parser.Syntax.Constants.TAG_BEGIN = props.BeginTag
                Parser.Syntax.Constants.TAG_END = props.EndTag
            Catch ex As Exception
            End Try

            'Set focus, etc
            Me.TextFont = Me.Default_Font
            Me.FocusText()
        End Sub

        Public Shadows ReadOnly Property Focused() As Boolean
            Get
                Return Me.txtBox.Focused
            End Get
        End Property

        Public Property TextFont() As Font
            Get
                Return Me.txtBox.Font
            End Get
            Set(ByVal value As Font)
                Me.txtBox.Font = value
            End Set
        End Property

        Public Property [ReadOnly]() As Boolean
            Get
                Return Me.txtBox.ReadOnly
            End Get
            Set(ByVal value As Boolean)
                Me.txtBox.ReadOnly = value
            End Set
        End Property

        Public Sub FocusText()
            Me.txtBox.Focus()
        End Sub

        Public Property Description() As String
            Get
                Return Me.lblDescription.Text
            End Get
            Set(ByVal value As String)
                Me.lblDescription.Text = value
            End Set
        End Property

        Public Shadows Property Text() As String
            Get
                Return Me.txtBox.Text
            End Get
            Set(ByVal value As String)
                Me.txtBox.Text = value
            End Set
        End Property

        Public Shadows Property Enabled() As Boolean
            Get
                Return Me.txtBox.Enabled
            End Get
            Set(ByVal value As Boolean)
                Me.txtBox.Enabled = value
            End Set
        End Property

        Public Property DontHilight() As Boolean
            Get
                Return Me.txtBox.DontHilight
            End Get
            Set(ByVal value As Boolean)
                Me.txtBox.DontHilight = value
            End Set
        End Property

        Public Property SelectedText() As String
            Get
                Return Me.txtBox.SelectedText
            End Get
            Set(ByVal value As String)
                Me.txtBox.SelectedText = value
            End Set
        End Property

        Public Property SelectionStart() As Integer
            Get
                Return Me.txtBox.SelectionStart
            End Get
            Set(ByVal value As Integer)
               
            End Set
        End Property

        Public Property SelectionLength() As Integer
            Get
                Return Me.txtBox.SelectionLength
            End Get
            Set(ByVal value As Integer)
                
            End Set
        End Property

        Public Sub SetSelection(ByVal offsetStartChars As Integer, ByVal offsetCharLength As Integer)
            Me.txtBox.SetSelection(offsetStartChars, offsetCharLength)
        End Sub

        Public Sub ClearSelection()
            Me.txtBox.ClearSelection()
        End Sub

        Public Function GetSender() As Object
            Return Me.txtBox
        End Function

        Public Sub ClosePopup()
            Me.popup.Close()
        End Sub

        Public Shadows Sub Focus()
            Me.txtBox.Focus()
        End Sub

        Private Sub popup_InsertTag(ByVal TagVal As String) Handles popup.InsertTag
            TagVal = TagVal.Trim

            'No begin tags to bother with just insert popup's tag value
            If TagVal.IndexOf(Parser.Syntax.Constants.TAG_BEGIN) = -1 Then
                Me.SelectedText = TagVal
                Exit Sub
            End If

            'First check the line of this entry point
            Dim str As String = StrReverse(Me.Text.Substring(0, Me.SelectionStart))
            Dim idx As Integer = str.IndexOf(vbLf)
            str = Me.Text.Substring(Me.Text.Length - idx, idx)

            'If a double up of begin tags happens
            'eg <<! <<!blah
            idx = str.LastIndexOf(Parser.Syntax.Constants.TAG_BEGIN)
            If idx > -1 And TagVal.IndexOf(Parser.Syntax.Constants.TAG_BEGIN) = 0 Then
                'just remove that begin tag from tagval
                TagVal = TagVal.Substring(Parser.Syntax.Constants.TAG_BEGIN.Length, TagVal.Length - Parser.Syntax.Constants.TAG_BEGIN.Length)
            End If
            'Me.txtTemplate.SelectionStart = Me.txtTemplate.TextLength - idx

            'insert popup's tag value
            Me.SelectedText = TagVal
        End Sub

        Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
            'Get Sources related to this tag
            Dim srcs As New List(Of Persistence.Source)
            Try
                srcs = Me.GetSources()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End Try

            'Get main related to this tag
            Dim main As Persistence.Main = Nothing
            Try
                main = Me.GetMain()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End Try

            'Get properties related to this tag
            Dim props As Persistence.Properties = Nothing
            Try
                props = Me.GetProperties()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End Try

            'Get .net source
            Dim VBSource As System.Text.StringBuilder = Me.GetVBSource
            Dim CSSource As System.Text.StringBuilder = Me.GetCSSource

            'Open preview
            Dim tmp As New Persistence.Template()
            tmp.Name = CType(Me.Tag, Persistence.Template).Name
            tmp.Text = Me.Text

            Dim BasePath As String = ""
            Try
                BasePath = System.IO.Path.GetDirectoryName(CType(Me.ParentForm, MainForm).ProjectPath)
            Catch ex As Exception
            End Try

            Dim dlg As New PreviewOutput(BasePath, props, main.Text, srcs, tmp, VBSource, CSSource)
            dlg.ShowDialog(Me)
        End Sub

        Private Function GetProperties() As Persistence.Properties
            If TypeOf Me.Tag Is Persistence.Properties Then
                Return CType(Me.Tag, Persistence.Properties)

            ElseIf TypeOf Me.Tag Is Persistence.Template Then
                'Try to get properties from the parent form control
                'Get explorer control
                Dim ex As Explorer = Me.GetExplorerControl(Me.ParentForm)
                If ex Is Nothing Then
                    Throw New Exception("Failure to retrieve properties associated with this template")
                End If
                If ex.Nodes.Count = 0 Then Throw New Exception("Failure to retrieve properties associated with this template")
                For Each node As TreeNode In ex.Nodes(0).Nodes
                    If TypeOf node.Tag Is Persistence.Properties Then Return CType(node.Tag, Persistence.Properties)
                Next

            End If

            Throw New Exception("Failure to retrieve properties associated with this template")
        End Function

        Private Function GetTemplate() As Persistence.Template
            If TypeOf Me.Tag Is Persistence.Main Then
                Throw New Exception("This is not a template")

            ElseIf TypeOf Me.Tag Is Persistence.Template Then
                'Check if edited in tab page
                Dim tp As TabPage = Me.GetTabPage(Me.ParentForm, Me.Tag)
                If tp IsNot Nothing Then
                    For Each ctl As Control In tp.Controls
                        If TypeOf ctl Is CodeEditor Then
                            Dim tmp As Persistence.Template = CType(CType(ctl.Tag, Persistence.Template).GetCopy(), Persistence.Template)
                            tmp.Text = CType(ctl, CodeEditor).Text
                            Return tmp
                        End If
                    Next
                End If

                Return CType(Me.Tag, Persistence.Template)
            Else
                Throw New Exception("This is not a template")

            End If
        End Function

        Private Function GetMain() As Persistence.Main
            Dim main As Persistence.Main = Nothing

            If TypeOf Me.Tag Is Persistence.Main Then
                main = CType(CType(Me.Tag, Persistence.Main).GetCopy(), Persistence.Main)

            ElseIf TypeOf Me.Tag Is Persistence.Template Then
                'Try to get main from the parent form control
                'Get explorer control
                Dim ex As Explorer = Me.GetExplorerControl(Me.ParentForm)
                If ex Is Nothing Then
                    Throw New Exception("Failure to retrieve main associated with this template")
                End If
                'Get owner node (package node)
                Dim node As TreeNode = Me.FindNode(ex.Nodes(0), CType(Me.Tag, Persistence.IEditorItem).OwnerGUID)
                If node Is Nothing Then
                    Throw New Exception("Failure to retrieve main associated with this template")
                End If
                'Get main in that node
                node = Me.FindNode(node, GetType(Persistence.Main))
                If node Is Nothing Then
                    Throw New Exception("Failure to retrieve main associated with this template")
                End If

                main = CType(CType(node.Tag, Persistence.Main).GetCopy(), Persistence.Main)
            End If

            If main Is Nothing Then Throw New Exception("Failure to retrieve main associated with this template")

            'Check if edited in tab page
            Dim tp As TabPage = Me.GetTabPage(Me.ParentForm, main)
            If tp IsNot Nothing Then
                For Each ctl As Control In tp.Controls
                    If TypeOf ctl Is CodeEditor Then
                        main.Text = CType(ctl, CodeEditor).Text
                        Return main
                    End If
                Next
            End If

            Return main
        End Function

        Private Function GetSources() As List(Of Persistence.Source)
            'Try to get Sources from the parent form control
            'Get explorer control
            Dim ex As Explorer = Me.GetExplorerControl(Me.ParentForm)
            If ex Is Nothing Then
                Throw New Exception("Failure to retrieve sources")
            End If

            'Source nodes in the project node
            Return Me.GetSources(ex.Nodes(0))
        End Function

        Private Function GetSources(ByVal node As TreeNode) As List(Of Persistence.Source)
            Dim ret As New List(Of Persistence.Source)

            If TypeOf node.Tag Is Persistence.Source Then
                ret.Add(CType(node.Tag, Persistence.Source))
                Return ret
            End If

            'Sources in nodes
            For Each n As TreeNode In node.Nodes
                If TypeOf n.Tag Is Persistence.Source Then
                    ret.Add(CType(n.Tag, Persistence.Source))
                Else
                    'sources in subnodes
                    For Each r In Me.GetSources(n)
                        ret.Add(r)
                    Next
                End If
            Next

            Return ret
        End Function

        Private Function GetVBSource() As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Get explorer control
            Dim ex As Explorer = Me.GetExplorerControl(Me.ParentForm)
            If ex Is Nothing Then
                Throw New Exception("Failure to retrieve VB source")
            End If
            'Get owner node (package node)
            Dim node As TreeNode = Me.FindNode(ex.Nodes(0), CType(Me.Tag, Persistence.IEditorItem).OwnerGUID)
            If node Is Nothing Then
                Throw New Exception("Failure to retrieve VB source")
            End If
            'Build source
            For Each n As TreeNode In node.Nodes
                If TypeOf n.Tag Is Persistence.CodeDOM_VB Then
                    sb.AppendLine(CType(n.Tag, Persistence.CodeDOM_VB).Text)
                End If
            Next

            Return sb
        End Function

        Private Function GetCSSource() As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Get explorer control
            Dim ex As Explorer = Me.GetExplorerControl(Me.ParentForm)
            If ex Is Nothing Then
                Throw New Exception("Failure to retrieve C Sharp source")
            End If
            'Get owner node (package node)
            Dim node As TreeNode = Me.FindNode(ex.Nodes(0), CType(Me.Tag, Persistence.IEditorItem).OwnerGUID)
            If node Is Nothing Then
                Throw New Exception("Failure to retrieve C Sharp source")
            End If
            'Build source
            For Each n As TreeNode In node.Nodes
                If TypeOf n.Tag Is Persistence.CodeDOM_CS Then
                    sb.AppendLine(CType(n.Tag, Persistence.CodeDOM_CS).Text)
                End If
            Next

            Return sb
        End Function

        'Get tab page of tag from parent form
        Private Function GetTabPage(ByVal control As Control, ByVal Tag As Object) As TabPage
            Dim guid As String = ""
            If TypeOf Tag Is Persistence.Main Then
                guid = CType(Tag, Persistence.Main).EditorGUID
            ElseIf TypeOf Tag Is Persistence.Template Then
                guid = CType(Tag, Persistence.Template).EditorGUID
            Else
                Return Nothing
            End If

            If MainForm.tcMain Is Nothing Then Return Nothing

            For Each tp As TabPage In MainForm.tcMain.TabPages

                If tp.Tag Is Nothing Then Continue For
                If Not TypeOf tp.Tag Is TreeNode Then Continue For

                If TypeOf CType(tp.Tag, TreeNode).Tag Is Persistence.Main Then
                    Dim main As Persistence.Main = CType(CType(tp.Tag, TreeNode).Tag, Persistence.Main)
                    If main.EditorGUID.Equals(guid) Then Return tp

                ElseIf TypeOf CType(tp.Tag, TreeNode).Tag Is Persistence.Template Then
                    Dim temp As Persistence.Template = CType(CType(tp.Tag, TreeNode).Tag, Persistence.Template)
                    If temp.EditorGUID.Equals(guid) Then Return tp

                End If
            Next

            Return Nothing
        End Function

        'Get explorer control from parent form
        Private Function GetExplorerControl(ByVal control As Control) As Explorer
            If TypeOf control Is Explorer Then Return CType(control, Explorer)
            For Each ctl As Control In control.Controls
                If TypeOf ctl Is Explorer Then Return CType(ctl, Explorer)
                If ctl.Controls.Count > 0 Then
                    Dim ex As Explorer = Me.GetExplorerControl(ctl)
                    If ex IsNot Nothing Then Return ex
                End If
            Next
            Return Nothing
        End Function

        Private Function FindNode(ByVal currentNode As TreeNode, ByVal GUID As String) As TreeNode
            If TypeOf currentNode.Tag Is Persistence.MDPackageTag Then
                If CType(currentNode.Tag, Persistence.MDPackageTag).GUID.Equals(GUID) Then Return currentNode
            ElseIf TypeOf currentNode.Tag Is Persistence.IEditorItem Then
                If CType(currentNode.Tag, Persistence.IEditorItem).EditorGUID.Equals(GUID) Then Return currentNode
            End If
            For Each n As TreeNode In currentNode.Nodes
                Dim foundNode As TreeNode = Me.FindNode(n, GUID)
                If foundNode IsNot Nothing Then Return foundNode
            Next
            Return Nothing
        End Function

        Private Function FindNode(ByVal currentNode As TreeNode, ByVal tagType As Type) As TreeNode
            If currentNode.Tag.GetType.Equals(tagType) Then Return currentNode
            For Each n As TreeNode In currentNode.Nodes
                Dim foundNode As TreeNode = Me.FindNode(n, tagType)
                If foundNode IsNot Nothing Then Return foundNode
            Next
            Return Nothing
        End Function

        Private Sub txtBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtBox.MouseDown
            Me.HidePopup(False)
        End Sub

    End Class

End Namespace