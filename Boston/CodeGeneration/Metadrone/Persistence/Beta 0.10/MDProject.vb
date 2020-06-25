Namespace Persistence.Beta10

    <Serializable()> _
    Public Class MDProject

        Friend Shared PersistenceNamespace As String = "www.metadrone.com.beta.0.10"

        Public Name As String
        Public Profile As New Profile()
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
                If String.IsNullOrEmpty(pkg.Properties.OwnerGUID) Then pkg.Properties.OwnerGUID = pkg.TagVal.GUID
                If String.IsNullOrEmpty(pkg.Directives.OwnerGUID) Then pkg.Directives.OwnerGUID = pkg.TagVal.GUID
                Me.Packages.Add(pkg)
            Next

            'Bin
            Me.Bin.obj = proj.Bin.obj
        End Sub

        Public Function ToCurrent() As Persistence.Beta11.MDProject
            'Set up project
            Dim proj As New Persistence.Beta11.MDProject()
            proj.Name = Me.Name

            'Profile
            proj.Profile.OpenEditorGUIDs.Clear()
            For Each Guid In Me.Profile.OpenEditorGUIDs
                proj.Profile.OpenEditorGUIDs.Add(Guid)
            Next
            proj.Profile.SelectedEditorGUID = Me.Profile.SelectedEditorGUID

            'Folders
            For Each fld In Me.Folders
                Dim copy As New Persistence.Beta11.ProjectFolder()
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

        Private Function ConvSource(ByVal source As Persistence.Beta10.Source) As Persistence.Beta11.Source
            Dim Copy As New Persistence.Beta11.Source()
            Copy.Name = source.Name
            Copy.Provider = source.Provider

            Copy.ConnectionString = source.ConnectionString
            Copy.SchemaQuery = source.SchemaQuery
            Copy.TableSchemaQuery = source.TableSchemaQuery
            Copy.ColumnSchemaQuery = source.ColumnSchemaQuery
            Copy.TableNamePlaceHolder = source.TableNamePlaceHolder
            Copy.RoutineSchemaQuery = source.RoutineSchemaQuery
            Copy.Transformations = source.Transformations

            Copy.GUID = source.GUID

            Return Copy
        End Function

        Private Function ConvPackage(ByVal package As Persistence.Beta10.MDPackage) As Persistence.Beta11.MDPackage
            Dim Copy As New Persistence.Beta11.MDPackage()
            Copy.Name = package.Name

            Copy.TagVal = New Persistence.Beta11.MDPackageTag()
            Copy.TagVal.GUID = package.TagVal.GUID

            Copy.Properties = New Persistence.Beta11.Properties()
            Copy.Properties.BeginTag = package.Properties.BeginTag
            Copy.Properties.EndTag = package.Properties.EndTag
            Copy.Properties.BeginSafe = package.Properties.BeginSafe
            Copy.Properties.EndSafe = package.Properties.EndSafe
            Copy.Properties.EditorGUID = package.Properties.EditorGUID
            Copy.Properties.OwnerGUID = package.Properties.OwnerGUID

            Copy.Directives = New Persistence.Beta11.Directives()
            Copy.Directives.Text = package.Directives.Text
            Copy.Directives.EditorGUID = package.Directives.EditorGUID
            Copy.Directives.OwnerGUID = package.Directives.OwnerGUID

            'Convert folders
            For Each fld In package.Folders
                Copy.Folders.Add(Me.ConvFolder(fld))
            Next

            'Convert templates
            For Each t In package.Templates
                Dim tc As New Persistence.Beta11.Template()
                tc.Name = t.Name
                tc.Text = t.Text
                tc.EditorGUID = t.EditorGUID
                tc.OwnerGUID = t.OwnerGUID
                Copy.Templates.Add(tc)
            Next

            Return Copy
        End Function

        Private Function ConvProjectFolder(ByVal Folder As Persistence.Beta10.ProjectFolder) As Persistence.Beta11.ProjectFolder
            Dim fld As New Persistence.Beta11.ProjectFolder(Folder.Name)

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

        Private Function ConvFolder(ByVal Folder As Persistence.Beta10.Folder) As Persistence.Beta11.Folder
            Dim fld As New Persistence.Beta11.Folder(Folder.Name)

            'Convert templates
            For Each t In Folder.Templates
                Dim tmp As New Persistence.Beta11.Template()
                tmp.Name = t.Name
                tmp.Text = t.Text
                tmp.EditorGUID = t.EditorGUID
                tmp.OwnerGUID = t.OwnerGUID
                fld.Templates.Add(tmp)
            Next

            'Sub folders
            For Each f In Folder.Folders
                fld.Folders.Add(Me.ConvFolder(f))
            Next

            Return fld
        End Function

    End Class

End Namespace