Imports Boston.Parser.Syntax.Constants

Namespace Persistence

    Public Class ProjectManager

        Friend Shared PersistenceNamespace As String = AngryArmy_1_0.MDProject.PersistenceNamespace

        Public Shared Sub Save(ByVal MDProject As MDProject, ByVal FileName As String)
            Dim DataFile As New System.IO.FileStream(FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)
            Try
                Dim Serializer As New System.Xml.Serialization.XmlSerializer(GetType(MDProject), PersistenceNamespace)
                Serializer.Serialize(DataFile, MDProject)

            Catch ex As Exception
                Throw ex

            Finally
                DataFile.Close()

            End Try
        End Sub


        Public Shared Function Load(ByVal FileName As String) As MDProject
            'Current version
            Try
                Dim proj As New AngryArmy_1_0.MDProject()
                proj.Load(FileName)
                Return proj.ToCurrent()
            Catch ex As Exception
            End Try


            'Previous
            Try
                Dim proj As New Beta11.MDProject()
                proj.Load(FileName)

                Return proj.ToCurrent().ToCurrent()
            Catch ex As Exception
            End Try

            Try
                Dim proj As New Beta10.MDProject()
                proj.Load(FileName)

                Return proj.ToCurrent().ToCurrent().ToCurrent()
            Catch ex As Exception
                Throw New Exception("Unknown file format." & System.Environment.NewLine & ex.Message)
            End Try
        End Function

    End Class

End Namespace