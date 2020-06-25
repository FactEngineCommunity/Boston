Namespace Persistence.Settings

    Public Class Recent

        Public ProjectName As String = ""
        Public Path As String = ""

        Public Sub New()

        End Sub

        Friend Sub New(ByVal projectName As String, ByVal path As String)
            Me.ProjectName = projectName
            Me.Path = path
        End Sub

    End Class


    Public Class Settings

        Private settingsNamespace As String = "www.metadrone.com.settings.angry.army"

        Friend Enum pathTypes
            Application = 0
            UserAppData = 1
            Other = 2
        End Enum

        Friend pathType As pathTypes = pathTypes.Application

        Private _path As String = Me.Path


        Public LastWorkLocation As String = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)

        Public RecentProjects As New List(Of Recent)

        Friend Sub AddProject(ByVal recent As Recent)
            If recent Is Nothing Then Throw New Exception("Invalid recent project.")
            If String.IsNullOrEmpty(recent.ProjectName) Then Throw New Exception("Invalid recent project.")
            If String.IsNullOrEmpty(recent.Path) Then Throw New Exception("Invalid recent project.")

            'Limit projects to add
            If Me.RecentProjects.Count > 7 Then
                Me.RecentProjects.RemoveAt(0)
            End If

            'Don't duplicate, but move to end
            For i As Integer = 0 To Me.RecentProjects.Count - 1
                If Me.RecentProjects(i).ProjectName.Equals(recent.ProjectName, StringComparison.CurrentCultureIgnoreCase) And _
                   Me.RecentProjects(i).Path.Equals(recent.Path, StringComparison.CurrentCultureIgnoreCase) Then
                    Me.RecentProjects.RemoveAt(i)
                    Exit For
                End If
            Next

            Me.RecentProjects.Add(recent)
        End Sub

        Friend Sub RemoveRecent(ByVal path As String)
            For i As Integer = 0 To Me.RecentProjects.Count - 1
                If Me.RecentProjects(i).Path.Equals(path, StringComparison.CurrentCultureIgnoreCase) Then
                    Me.RecentProjects.RemoveAt(i)
                    Exit Sub
                End If
            Next
        End Sub

        Friend Property Path() As String
            Get
                Select Case Me.pathType
                    Case pathTypes.Application
                        Return IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location), _
                                               "Metadrone.settings")
                    Case pathTypes.UserAppData
                        Dim p As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _
                                                          "Metadrone")
                        Return IO.Path.Combine(p, "Metadrone.settings")
                    Case Else
                        Return Me._path
                End Select
            End Get
            Set(ByVal value As String)
                Me.pathType = pathTypes.Other
                Me._path = Path
            End Set
        End Property

        Private Sub Reset()
            Me.pathType = pathTypes.Application
            Me.LastWorkLocation = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
            Me.RecentProjects.Clear()
        End Sub

        Private Sub Copy(ByVal sett As Settings)
            Me.LastWorkLocation = sett.LastWorkLocation
            For Each r In sett.RecentProjects
                Me.RecentProjects.Add(New Recent(r.ProjectName, r.Path))
            Next
        End Sub

        Private Function CanWrite(ByVal path As String) As Boolean
            Dim perm As New Security.Permissions.FileIOPermission(Security.Permissions.FileIOPermissionAccess.Write, _
                                                                  IO.Path.GetDirectoryName(path))
            Return perm.IsUnrestricted()
        End Function

        Friend Sub Load()
            'Reset first
            Me.Reset()

            'Check if already existing in application path
            Dim tmpSett As New Settings()
            tmpSett.pathType = pathTypes.Application
            If Not IO.File.Exists(tmpSett.Path) Then
                'Check if already existing in user's appdata
                tmpSett.pathType = pathTypes.UserAppData
                If Not IO.File.Exists(tmpSett.Path) Then
                    'None to load
                    Exit Sub
                End If
                Me.pathType = tmpSett.pathType
            End If

            'Load
            Dim DataFile As System.IO.FileStream = Nothing
            Try
                DataFile = New System.IO.FileStream(Me.Path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None)
                Dim Deserializer As New System.Xml.Serialization.XmlSerializer(GetType(Persistence.Settings.Settings), Me.settingsNamespace)
                tmpSett = CType(Deserializer.Deserialize(DataFile), Persistence.Settings.Settings)

                'Copy from loaded
                Me.Copy(tmpSett)

            Catch ex As Exception
                'ignore

            Finally
                If DataFile IsNot Nothing Then DataFile.Close()

            End Try
        End Sub

        Friend Sub Save()
            'Can't figure out check write yet, so just catch/ignore fails
            Select Case Me.pathType
                Case pathTypes.Application
                    Try
                        Call Me.SaveSettings()
                    Catch ex As Exception
                        'If can't write try application path, try the app data path
                        Me.pathType = pathTypes.UserAppData
                        Call Me.Save()
                    End Try

                Case pathTypes.UserAppData, pathTypes.Other
                    Try
                        Call Me.SaveSettings()
                    Catch ex As Exception
                        'ignore
                    End Try

            End Select
        End Sub

        Private Sub SaveSettings()
            Dim DataFile As System.IO.FileStream = Nothing
            Try
                'Make sure directory is there first
                If Not IO.Directory.Exists(IO.Path.GetDirectoryName(Me.Path)) Then
                    IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(Me.Path))
                End If

                DataFile = New System.IO.FileStream(Me.Path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)
                Dim Serializer As New System.Xml.Serialization.XmlSerializer(GetType(Persistence.Settings.Settings), Me.settingsNamespace)
                Serializer.Serialize(DataFile, Me)

            Catch ex As Exception
                Throw ex

            Finally
                If DataFile IsNot Nothing Then DataFile.Close()

            End Try
        End Sub

    End Class

End Namespace
