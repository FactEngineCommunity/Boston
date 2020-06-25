Imports Boston.Persistence

Namespace UI

    Partial Friend Class Explorer

        Private Function GetPackages() As List(Of String)
            If Me.tvwMain.TopNode Is Nothing Then Return Nothing
            Dim pkgs As New List(Of String)
            For Each node As TreeNode In Me.tvwMain.TopNode.Nodes
                If TypeOf node.Tag Is MDPackageTag Then
                    pkgs.Add(node.Text)
                ElseIf node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                    For Each pkg In Me.GetPackages(node)
                        pkgs.Add(pkg)
                    Next
                End If
            Next
            Return pkgs
        End Function

        Private Function GetPackages(ByVal folderNode As TreeNode) As List(Of String)
            Dim pkgs As New List(Of String)
            For Each node As TreeNode In folderNode.Nodes
                If TypeOf node.Tag Is MDPackageTag Then
                    pkgs.Add(node.Text)
                ElseIf node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                    For Each pkg In Me.GetPackages(node)
                        pkgs.Add(pkg)
                    Next
                End If
            Next
            Return pkgs
        End Function

        Private Function GetSources() As List(Of Source)
            If Me.tvwMain.TopNode Is Nothing Then Return Nothing
            Dim srces As New List(Of Source)
            For Each node As TreeNode In Me.tvwMain.TopNode.Nodes
                If TypeOf node.Tag Is Source Then
                    srces.Add(CType(node.Tag, Source))
                ElseIf node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                    For Each src In Me.GetSources(node)
                        srces.Add(src)
                    Next
                End If
            Next
            Return srces
        End Function

        Private Function GetSources(ByVal folderNode As TreeNode) As List(Of Source)
            Dim srces As New List(Of Source)
            For Each node As TreeNode In folderNode.Nodes
                If TypeOf node.Tag Is Source Then
                    srces.Add(CType(node.Tag, Source))
                ElseIf node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                    For Each src In Me.GetSources(node)
                        srces.Add(src)
                    Next
                End If
            Next
            Return srces
        End Function

        Private Function GetPackage(ByVal path As String, ByVal node As TreeNode) As MDPackage
            If node Is Nothing Then Return Nothing
            If Not TypeOf node.Tag Is MDPackageTag Then Return Nothing

            Dim pkg As New MDPackage(node.Text, False)
            pkg.TagVal = CType(node.Tag, MDPackageTag)
            pkg.Path = path

            'Get main, etc
            For Each n As TreeNode In node.Nodes
                If TypeOf n.Tag Is Main Then
                    pkg.Main = CType(n.Tag, Main)
                    Exit For
                End If
            Next

            'Folders and templates/code files
            Call Me.NodesToPackageItems(node.Nodes, pkg.Folders, pkg.Templates, pkg.VBCode, pkg.CSCode)

            'Return the package
            Return pkg
        End Function

        Private Function GetProjectFolder(ByVal path As String, ByVal node As TreeNode) As ProjectFolder
            If node Is Nothing Then Return Nothing
            If Not node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then Return Nothing

            Dim pf As New ProjectFolder(node.Text)
            For Each n As TreeNode In node.Nodes
                If n.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                    'Recursively, another folder
                    pf.Folders.Add(Me.GetProjectFolder(path & n.Text & "/", n))

                ElseIf TypeOf n.Tag Is MDPackageTag Then
                    'Add package
                    pf.Packages.Add(Me.GetPackage(path, n))

                ElseIf TypeOf n.Tag Is Source Then
                    'Add Source
                    pf.Sources.Add(CType(CType(n.Tag, Source).GetCopy(), Persistence.Source))

                End If
            Next

            Return pf
        End Function


        'Transforms treenode data into project object
        Friend Function Transform(ByVal MainNode As TreeNode) As MDProject
            Dim proj As New MDProject()
            proj.Name = Me.tvwMain.TopNode.Text

            For Each Node As TreeNode In MainNode.Nodes
                If TypeOf Node.Tag Is Boston.Persistence.Properties Then
                    'Properties
                    proj.Properties = CType(Node.Tag, Boston.Persistence.Properties)

                ElseIf Node.Tag.ToString.Equals(TAG_PROJECT_FOLDER) Then
                    'Add project folder
                    proj.Folders.Add(Me.GetProjectFolder(Node.Text & "/", Node))

                ElseIf TypeOf Node.Tag Is MDPackageTag Then
                    'Add the package
                    proj.Packages.Add(Me.GetPackage("", Node))

                ElseIf TypeOf Node.Tag Is Source Then
                    'Add the Source
                    proj.Sources.Add(CType(CType(Node.Tag, Source).GetCopy(), Persistence.Source))

                End If
            Next

            'Profile
            proj.Profile = Me.LoadedProfile

            Return proj
        End Function

        'Transform project object into treenode data
        Friend Function Transform(ByVal Project As MDProject) As TreeNode
            'Project node first
            Dim projNode As New TreeNode(Project.Name, IMG_PROJECT, IMG_PROJECT)
            projNode.Tag = TAG_PROJECT

            'Profile
            Me.LoadedProfile = Project.Profile

            'Properties
            Dim propertiesNode As New TreeNode("Properties", IMG_PROPERTIES, IMG_PROPERTIES)
            propertiesNode.Tag = Project.Properties
            projNode.Nodes.Add(propertiesNode)

            'Project folders
            For Each fld In Project.Folders
                Dim node As New TreeNode(fld.Name, IMG_PROJECT_FOLDER_CLOSED, IMG_PROJECT_FOLDER_CLOSED)
                node.Tag = TAG_PROJECT_FOLDER
                For Each n In Me.ProjectFolderItemsToNodes(fld.Folders, fld.Sources, fld.Packages)
                    node.Nodes.Add(n)
                Next
                projNode.Nodes.Add(node)
            Next

            'Sources
            For Each src In Project.Sources
                Dim node = New TreeNode(src.Name, IMG_Source, IMG_Source)
                node.Tag = src
                projNode.Nodes.Add(node)
            Next

            'Packages
            For Each pkg In Project.Packages
                Dim pkgNode As New TreeNode(pkg.Name, IMG_PACKAGE, IMG_PACKAGE)
                Dim mainNode As New TreeNode("Main", IMG_MAIN, IMG_MAIN)

                pkgNode.Tag = pkg.TagVal
                mainNode.Tag = pkg.Main

                pkgNode.Nodes.Add(mainNode)

                Dim nds As List(Of TreeNode) = Me.PackageItemsToNodes(pkg.Folders, pkg.Templates, pkg.VBCode, pkg.CSCode)
                For i As Integer = 0 To nds.Count - 1
                    pkgNode.Nodes.Add(nds(i))
                Next

                projNode.Nodes.Add(pkgNode)
                pkgNode.Expand()
            Next

            Return projNode
        End Function

        Public Sub LoadProject(ByVal FileName As String)
            'Load project
            Dim proj As MDProject = ProjectManager.Load(FileName)

            'Clear
            Me.tvwMain.Nodes.Clear()

            'And add top node
            Me.tvwMain.Nodes.Add(Me.Transform(proj))
            Me.tvwMain.TopNode = Me.tvwMain.Nodes(0)
            Me.tvwMain.TopNode.Expand()
        End Sub

        Public Function GetLoadedProfile() As Profile
            Return Me.LoadedProfile
        End Function

        Public Sub SetLoadedProfile(ByVal tcMain As TabControl)
            Me.LoadedProfile = New Profile()
            For Each tp As TabPage In tcMain.TabPages
                If TypeOf tp.Tag Is TreeNode Then
                    If TypeOf CType(tp.Tag, TreeNode).Tag Is IEditorItem Then
                        Me.LoadedProfile.OpenEditorGUIDs.Add(CType(CType(tp.Tag, TreeNode).Tag, IEditorItem).EditorGUID)
                    End If
                End If
            Next
            If TypeOf tcMain.SelectedTab.Tag Is TreeNode Then
                If TypeOf CType(tcMain.SelectedTab.Tag, TreeNode).Tag Is IEditorItem Then
                    Me.LoadedProfile.SelectedEditorGUID = CType(CType(tcMain.SelectedTab.Tag, TreeNode).Tag, IEditorItem).EditorGUID
                End If
            End If
        End Sub

        Public Sub SaveProject(ByVal FileName As String)
            'Setup object
            Dim proj As MDProject = Me.Transform(Me.tvwMain.TopNode)

            'Save
            ProjectManager.Save(proj, FileName)
        End Sub


        Private Function PackageItemsToNodes(ByVal Folders As List(Of Folder), ByVal Templates As List(Of Template), _
                                             ByVal VBs As List(Of CodeDOM_VB), ByVal CSs As List(Of CodeDOM_CS)) As List(Of TreeNode)
            Dim tnodes = New List(Of TreeNode)

            'Folder nodes
            For Each f In Folders
                Dim Node As New TreeNode(f.Name, IMG_FOLDER_CLOSED, IMG_FOLDER_CLOSED)
                Node.Tag = TAG_FOLDER

                'Add sub nodes
                For Each n As TreeNode In Me.PackageItemsToNodes(f.Folders, f.Templates, f.VBCode, f.CSCode)
                    Node.Nodes.Add(n)
                Next

                tnodes.Add(Node)
            Next

            'Template nodes
            For Each t In Templates
                Dim Node As New TreeNode(t.Name, IMG_TEMPLATE, IMG_TEMPLATE)
                Node.Tag = t
                tnodes.Add(Node)
            Next

            'Code files
            For Each vb In VBs
                Dim Node As New TreeNode(vb.Name, IMG_VB, IMG_VB)
                Node.Tag = vb
                tnodes.Add(Node)
            Next
            For Each cs In CSs
                Dim Node As New TreeNode(cs.Name, IMG_CS, IMG_CS)
                Node.Tag = cs
                tnodes.Add(Node)
            Next


            Dim list As New ArrayList()
            For Each n As TreeNode In tnodes
                list.Add(n)
            Next
            list.Sort(New NodeSorter())

            tnodes = New List(Of TreeNode)
            For Each n As TreeNode In list
                tnodes.Add(n)
            Next

            Return tnodes
        End Function

        Private Sub NodesToPackageItems(ByVal Nodes As TreeNodeCollection, _
                                        ByRef Folders As List(Of Folder), ByRef Templates As List(Of Template), _
                                        ByRef VBs As List(Of CodeDOM_VB), ByRef CSs As List(Of CodeDOM_CS))
            For Each Node As TreeNode In Nodes
                If TypeOf Node.Tag Is Template Then
                    'Just add template
                    Templates.Add(CType(Node.Tag, Template))

                ElseIf TypeOf Node.Tag Is CodeDOM_VB Then
                    'VB
                    VBs.Add(CType(Node.Tag, CodeDOM_VB))

                ElseIf TypeOf Node.Tag Is CodeDOM_CS Then
                    'C Sharp
                    CSs.Add(CType(Node.Tag, CodeDOM_CS))

                ElseIf Node.Tag.ToString.Equals(TAG_FOLDER) Then
                    'Set up folder
                    Dim folder As New Folder(Node.Text)

                    'Items under this folder
                    For Each n As TreeNode In Node.Nodes
                        If TypeOf n.Tag Is Template Then
                            'Template
                            folder.Templates.Add(CType(n.Tag, Template))

                        ElseIf TypeOf n.Tag Is CodeDOM_VB Then
                            'VB
                            folder.VBCode.Add(CType(n.Tag, CodeDOM_VB))

                        ElseIf TypeOf n.Tag Is CodeDOM_CS Then
                            'C Sharp
                            folder.CSCode.Add(CType(n.Tag, CodeDOM_CS))

                        End If
                    Next

                    'Get sub-folders
                    Call Me.NodesToPackageItems(Node.Nodes, folder.Folders, New List(Of Template), _
                                                New List(Of CodeDOM_VB), New List(Of CodeDOM_CS))

                    'Add folder
                    Folders.Add(folder)
                End If
            Next
        End Sub

        Private Function ProjectFolderItemsToNodes(ByVal Folders As List(Of ProjectFolder), _
                                                   ByVal Sources As List(Of Source), ByVal Packages As List(Of MDPackage)) As List(Of TreeNode)
            Dim nodes As New List(Of TreeNode)

            'Folder nodes
            For Each f In Folders
                Dim Node As New TreeNode(f.Name, IMG_PROJECT_FOLDER_CLOSED, IMG_PROJECT_FOLDER_CLOSED)
                Node.Tag = TAG_PROJECT_FOLDER

                'Add sub nodes
                For Each n As TreeNode In Me.ProjectFolderItemsToNodes(f.Folders, f.Sources, f.Packages)
                    Node.Nodes.Add(n)
                Next

                nodes.Add(Node)
            Next

            'Sources
            For Each src In Sources
                Dim node = New TreeNode(src.Name, IMG_Source, IMG_Source)
                node.Tag = src
                nodes.Add(node)
            Next

            'Packages
            For Each pkg In Packages
                Dim pkgNode As New TreeNode(pkg.Name, IMG_PACKAGE, IMG_PACKAGE)
                Dim mainNode As New TreeNode("Main", IMG_MAIN, IMG_MAIN)

                pkgNode.Tag = pkg.TagVal
                mainNode.Tag = pkg.Main

                pkgNode.Nodes.Add(mainNode)

                For Each n In Me.PackageItemsToNodes(pkg.Folders, pkg.Templates, pkg.VBCode, pkg.CSCode)
                    pkgNode.Nodes.Add(n)
                Next

                nodes.Add(pkgNode)
                pkgNode.Expand()
            Next

            Return nodes
        End Function

    End Class

End Namespace