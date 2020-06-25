Namespace Persistence.Beta11

    <Serializable()> _
    Public Class MDProject

        Friend Shared PersistenceNamespace As String = "www.metadrone.com.beta.0.11"

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

        Public Function ToCurrent() As Persistence.AngryArmy_1_0.MDProject
            'Set up project
            Dim proj As New Persistence.AngryArmy_1_0.MDProject()
            proj.Name = Me.Name
            proj.Properties = New Persistence.AngryArmy_1_0.Properties()

            'Profile
            proj.Profile.OpenEditorGUIDs.Clear()
            For Each Guid In Me.Profile.OpenEditorGUIDs
                proj.Profile.OpenEditorGUIDs.Add(Guid)
            Next
            proj.Profile.SelectedEditorGUID = Me.Profile.SelectedEditorGUID

            'Folders
            For Each fld In Me.Folders
                Dim copy As New Persistence.AngryArmy_1_0.ProjectFolder()
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
            Dim sbCallPackages As New System.Text.StringBuilder()
            For Each pkg In Me.Packages
                proj.Packages.Add(Me.ConvPackage(pkg))
                sbCallPackages.AppendLine(Parser.Syntax.Constants.ACTION_CALL & " " & pkg.Name)
            Next

            'Set super main
            proj.Properties.SuperMain = sbCallPackages.ToString

            'Bin object data
            Me.Bin.Load()
            proj.Bin.obj = Me.Bin.obj
            proj.Bin.Bin = Me.Bin.Bin

            Return proj
        End Function

        Private Function ConvSource(ByVal source As Persistence.Beta11.Source) As Persistence.AngryArmy_1_0.Source
            Dim Copy As New Persistence.AngryArmy_1_0.Source()
            Copy.Name = source.Name
            Copy.Provider = source.Provider

            Copy.ConnectionString = source.ConnectionString
            Copy.SchemaQuery = source.SchemaQuery
            Copy.TableSchemaQuery = source.TableSchemaQuery
            Copy.ColumnSchemaQuery = source.ColumnSchemaQuery
            Copy.TableNamePlaceHolder = source.TableNamePlaceHolder
            Copy.RoutineSchemaQuery = source.RoutineSchemaQuery
            Copy.Transformations = source.Transformations

            Copy.EditorGUID = source.GUID

            Return Copy
        End Function

        Private Function ConvPackage(ByVal package As Persistence.Beta11.MDPackage) As Persistence.AngryArmy_1_0.MDPackage
            Dim Copy As New Persistence.AngryArmy_1_0.MDPackage()
            Copy.Name = package.Name

            Copy.TagVal = New Persistence.AngryArmy_1_0.MDPackageTag()
            Copy.TagVal.GUID = package.TagVal.GUID

            Copy.Main = New Persistence.AngryArmy_1_0.Main()
            Copy.Main.Text = Me.ModifyMain(package.Directives.Text, _
                                           package.Properties.BeginSafe, _
                                           package.Properties.EndSafe).ToString
            Copy.Main.EditorGUID = package.Directives.EditorGUID
            Copy.Main.OwnerGUID = package.Directives.OwnerGUID

            'Convert folders
            For Each fld In package.Folders
                Copy.Folders.Add(Me.ConvFolder(fld))
            Next

            'Convert templates
            For Each t In package.Templates
                Dim tc As New Persistence.AngryArmy_1_0.Template()
                tc.Name = t.Name
                tc.Text = Me.ModifyTemplate(t.Name, t.Text).ToString
                tc.EditorGUID = t.EditorGUID
                tc.OwnerGUID = t.OwnerGUID
                Copy.Templates.Add(tc)
            Next

            'Convert VB code files
            For Each vb In package.VBCode
                Dim v As New Persistence.AngryArmy_1_0.CodeDOM_VB()
                v.Name = vb.Name
                v.Text = vb.Text
                v.EditorGUID = vb.EditorGUID
                v.OwnerGUID = vb.OwnerGUID
                Copy.VBCode.Add(v)
            Next

            'Convert CS code files
            For Each cs In package.CSCode
                Dim c As New Persistence.AngryArmy_1_0.CodeDOM_CS()
                c.Name = cs.Name
                c.Text = cs.Text
                c.EditorGUID = cs.EditorGUID
                c.OwnerGUID = cs.OwnerGUID
                Copy.CSCode.Add(c)
            Next

            Return Copy
        End Function

        Private Function ModifyMain(ByVal source As String, ByVal BeginSafe As String, ByVal EndSafe As String) As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()
            If Not String.IsNullOrEmpty(BeginSafe) Then
                sb.Append(Parser.Syntax.Constants.RESERVED_PREPROC)
                sb.Append(Parser.Syntax.Constants.RESERVED_PREPROC_SAFEBEGIN & " = """)
                sb.AppendLine(BeginSafe & """")
            End If

            If Not String.IsNullOrEmpty(EndSafe) Then
                sb.Append(Parser.Syntax.Constants.RESERVED_PREPROC)
                sb.Append(Parser.Syntax.Constants.RESERVED_PREPROC_SAFEEND & " = """)
                sb.AppendLine(EndSafe & """")
            End If

            sb.Append(source)

            Return sb
        End Function

        Private Function ModifyTemplate(ByVal sourceName As String, ByVal source As String) As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            Try
                'Break into code and plain text
                Dim comp As New Parser.Syntax.Compilation(sourceName, source, False, False)
                Dim tags As List(Of Parser.Syntax.Compilation.TagVal) = comp.BuildTags()

                For Each tag In tags
                    If tag.IsCode Then
                        'Just add this code normally
                        If tag.Text.Trim.IndexOf(Parser.Syntax.Constants.ACTION_HEADER) = -1 Then
                            sb.Append(Parser.Syntax.Constants.TAG_BEGIN)
                            sb.Append(tag.Text)
                            sb.Append(Parser.Syntax.Constants.TAG_END)
                            Continue For
                        End If

                        'Remove method
                        tag.Text = Parser.Syntax.Strings.ReplaceInsensitive(tag.Text, "method single", "")
                        tag.Text = Parser.Syntax.Strings.ReplaceInsensitive(tag.Text, "method", "")
                        sb.Append(Parser.Syntax.Constants.TAG_BEGIN)
                        sb.Append(tag.Text)
                        sb.Append(Parser.Syntax.Constants.TAG_END)

                    Else
                        'Just add plain text
                        sb.Append(tag.Text)
                    End If
                Next

            Catch ex As Exception
                'Ignore, and just use the source
                sb.Append(source)
            End Try

            Return sb
        End Function

        Private Function ConvProjectFolder(ByVal Folder As Persistence.Beta11.ProjectFolder) As Persistence.AngryArmy_1_0.ProjectFolder
            Dim fld As New Persistence.AngryArmy_1_0.ProjectFolder(Folder.Name)

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

        Private Function ConvFolder(ByVal Folder As Persistence.Beta11.Folder) As Persistence.AngryArmy_1_0.Folder
            Dim fld As New Persistence.AngryArmy_1_0.Folder(Folder.Name)

            'Convert templates
            For Each t In Folder.Templates
                Dim tmp As New Persistence.AngryArmy_1_0.Template()
                tmp.Name = t.Name
                tmp.Text = t.Text
                tmp.EditorGUID = t.EditorGUID
                tmp.OwnerGUID = t.OwnerGUID
                fld.Templates.Add(tmp)
            Next

            'Convert VB code files
            For Each vb In Folder.VBCode
                Dim v As New Persistence.AngryArmy_1_0.CodeDOM_VB()
                v.Name = vb.Name
                v.Text = vb.Text
                v.EditorGUID = vb.EditorGUID
                v.OwnerGUID = vb.OwnerGUID
                fld.VBCode.Add(v)
            Next

            'Convert CS code files
            For Each cs In Folder.CSCode
                Dim c As New Persistence.AngryArmy_1_0.CodeDOM_CS()
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