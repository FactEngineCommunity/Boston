Namespace Persistence.AngryArmy_1_0

    <Serializable()> _
    Public Class MDProject

        Friend Shared PersistenceNamespace As String = "www.metadrone.com.angry.army.1.0"

        Public Name As String
        Public Profile As New Profile()
        Public Properties As New Properties()
        Public Folders As New List(Of ProjectFolder)
        Public Sources As New List(Of Source)
        Public Packages As New List(Of MDPackage)
        Public Bin As New MDObj()

        Public Sub Load(ByVal FileName As String)
            'Load and deserialise
            Dim DataFile As System.IO.FileStream = Nothing
            Dim proj As MDProject = Nothing
            Try
                DataFile = New System.IO.FileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None)
                Dim Deserializer As New System.Xml.Serialization.XmlSerializer(GetType(MDProject), PersistenceNamespace)
                proj = CType(Deserializer.Deserialize(DataFile), MDProject)

            Catch ex As Exception
                Throw ex

            Finally
                If DataFile IsNot Nothing Then DataFile.Close()

            End Try

            'Set up
            Me.Name = proj.Name

            'Profile
            Me.Profile.OpenEditorGUIDs.Clear()
            For Each Guid In proj.Profile.OpenEditorGUIDs
                Me.Profile.OpenEditorGUIDs.Add(Guid)
            Next
            Me.Profile.SelectedEditorGUID = proj.Profile.SelectedEditorGUID

            'Properties
            Me.Properties = proj.Properties

            'Folders
            For Each fld In proj.Folders
                Me.Folders.Add(fld)
            Next

            'Sources
            For Each src In proj.Sources
                Me.Sources.Add(src)
            Next

            'Packages
            For Each pkg In proj.Packages
                If String.IsNullOrEmpty(pkg.TagVal.GUID) Then pkg.TagVal.CreateNewGUID()
                For Each t As Template In pkg.Templates
                    If String.IsNullOrEmpty(t.OwnerGUID) Then t.OwnerGUID = pkg.TagVal.GUID
                Next
                If String.IsNullOrEmpty(pkg.Main.OwnerGUID) Then pkg.Main.OwnerGUID = pkg.TagVal.GUID
                Me.Packages.Add(pkg)
            Next

            'Bin
            Me.Bin.obj = proj.Bin.obj
        End Sub

        Public Function ToCurrent() As Persistence.MDProject
            'Set up project
            Dim proj As New Persistence.MDProject()
            proj.Name = Me.Name

            'Profile
            proj.Profile.OpenEditorGUIDs.Clear()
            For Each Guid In Me.Profile.OpenEditorGUIDs
                proj.Profile.OpenEditorGUIDs.Add(Guid)
            Next
            proj.Profile.SelectedEditorGUID = Me.Profile.SelectedEditorGUID

            'Properties
            proj.Properties = New Persistence.Properties()
            proj.Properties.BeginTag = Me.Properties.BeginTag
            proj.Properties.EndTag = Me.Properties.EndTag
            proj.Properties.EditorGUID = Me.Properties.EditorGUID
            proj.Properties.OwnerGUID = Me.Properties.OwnerGUID
            proj.Properties.SuperMain = Me.Properties.SuperMain

            'Folders
            For Each fld In Me.Folders
                Dim copy As New Persistence.ProjectFolder()
                copy.Name = fld.Name

                'Sub folders
                For Each f In fld.Folders
                    copy.Folders.Add(Me.ConvProjectFolder(f))
                Next

                'Sources in folder
                For Each src In fld.Sources
                    copy.Sources.Add(Me.ConvSource(src))
                Next

                'Packages in folder
                For Each pkg In fld.Packages
                    copy.Packages.Add(Me.ConvPackage(pkg))
                Next

                proj.Folders.Add(copy)
            Next

            'Sources
            For Each src In Me.Sources
                proj.Sources.Add(Me.ConvSource(src))
            Next

            'Packages
            For Each pkg In Me.Packages
                proj.Packages.Add(Me.ConvPackage(pkg))
            Next

            'Bin object data
            Me.Bin.Load()
            proj.Bin.obj = Me.Bin.obj
            proj.Bin.Bin = Me.Bin.Bin

            Return proj
        End Function

        Private Function ConvSource(ByVal source As Persistence.AngryArmy_1_0.Source) As Persistence.Source
            Dim Copy As New Persistence.Source()
            Copy.Name = source.Name
            Copy.Provider = source.Provider

            Copy.ConnectionString = source.ConnectionString
            Copy.SchemaQuery = source.SchemaQuery
            Copy.TableSchemaQuery = source.TableSchemaQuery
            Copy.ColumnSchemaQuery = source.ColumnSchemaQuery
            Copy.TableNamePlaceHolder = source.TableNamePlaceHolder
            Copy.RoutineSchemaQuery = source.RoutineSchemaQuery
            Copy.Transformations = source.Transformations

            Copy.BostonModelId = source.BostonModelId
            Copy.ModelName = source.ModelName

            Copy.EditorGUID = source.EditorGUID

            Return Copy
        End Function

        Private Function ConvPackage(ByVal package As Persistence.AngryArmy_1_0.MDPackage) As Persistence.MDPackage
            Dim Copy As New Persistence.MDPackage()
            Copy.Name = package.Name

            Copy.TagVal = New Persistence.MDPackageTag()
            Copy.TagVal.GUID = package.TagVal.GUID

            Copy.Main = New Persistence.Main()
            Copy.Main.Text = package.Main.Text
            Copy.Main.EditorGUID = package.Main.EditorGUID
            Copy.Main.OwnerGUID = package.Main.OwnerGUID

            'Convert folders
            For Each fld In package.Folders
                Copy.Folders.Add(Me.ConvFolder(fld))
            Next

            'Convert templates
            For Each t In package.Templates
                Dim tc As New Persistence.Template()
                tc.Name = t.Name
                tc.Text = t.Text
                tc.EditorGUID = t.EditorGUID
                tc.OwnerGUID = t.OwnerGUID
                Copy.Templates.Add(tc)
            Next

            'Convert VB code files
            For Each vb In package.VBCode
                Dim v As New Persistence.CodeDOM_VB()
                v.Name = vb.Name
                v.Text = vb.Text
                v.EditorGUID = vb.EditorGUID
                v.OwnerGUID = vb.OwnerGUID
                Copy.VBCode.Add(v)
            Next

            'Convert CS code files
            For Each cs In package.CSCode
                Dim c As New Persistence.CodeDOM_CS()
                c.Name = cs.Name
                c.Text = cs.Text
                c.EditorGUID = cs.EditorGUID
                c.OwnerGUID = cs.OwnerGUID
                Copy.CSCode.Add(c)
            Next

            Return Copy
        End Function

        Private Function ConvProjectFolder(ByVal Folder As Persistence.AngryArmy_1_0.ProjectFolder) As Persistence.ProjectFolder
            Dim fld As New Persistence.ProjectFolder(Folder.Name)

            'Sub folders
            For Each f In Folder.Folders
                fld.Folders.Add(Me.ConvProjectFolder(f))
            Next

            'Convert sources
            For Each src In Folder.Sources
                fld.Sources.Add(Me.ConvSource(src))
            Next

            'Convert packages
            For Each pkg In Folder.Packages
                fld.Packages.Add(Me.ConvPackage(pkg))
            Next

            Return fld
        End Function

        Private Function ConvFolder(ByVal Folder As Persistence.AngryArmy_1_0.Folder) As Persistence.Folder
            Dim fld As New Persistence.Folder(Folder.Name)

            'Convert templates
            For Each t In Folder.Templates
                Dim tmp As New Persistence.Template()
                tmp.Name = t.Name
                tmp.Text = t.Text
                tmp.EditorGUID = t.EditorGUID
                tmp.OwnerGUID = t.OwnerGUID
                fld.Templates.Add(tmp)
            Next

            'Convert VB code files
            For Each vb In Folder.VBCode
                Dim v As New Persistence.CodeDOM_VB()
                v.Name = vb.Name
                v.Text = vb.Text
                v.EditorGUID = vb.EditorGUID
                v.OwnerGUID = vb.OwnerGUID
                fld.VBCode.Add(v)
            Next

            'Convert CS code files
            For Each cs In Folder.CSCode
                Dim c As New Persistence.CodeDOM_CS()
                c.Name = cs.Name
                c.Text = cs.Text
                c.EditorGUID = cs.EditorGUID
                c.OwnerGUID = cs.OwnerGUID
                fld.CSCode.Add(c)
            Next

            'Sub folders
            For Each f In Folder.Folders
                fld.Folders.Add(Me.ConvFolder(f))
            Next

            Return fld
        End Function

    End Class

End Namespace