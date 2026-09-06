Imports System.IO
Imports System.ComponentModel
Imports Newtonsoft.Json

Namespace TestManagement

    ' Class for steps within a test
    Public Class TestRunArtifact
        Inherits Identifiable
        Implements IEquatable(Of TestManagement.Resource)

        <JsonProperty>
        <Browsable(False)>
        <ForeignKeyReference(GetType(TestManagement.TestRun), NameOf(TestManagement.TestRun.Identifier), True)>
        Public Property TestRunIdentifier As String

        Public ReadOnly Property ArtifactFileName As String
            Get
                If ArtifactFilePath IsNot Nothing AndAlso ArtifactFilePath?.Trim <> "" Then
                    Return Path.GetFileName(ArtifactFilePath)
                Else
                    Return ""
                End If
            End Get
        End Property


        Public Property ArtifactFilePath As String

        <Browsable(False)>
        Public Overrides Property Name As String
        <Browsable(False)>
        Public Overrides Property Description As String

        <Browsable(False)>
        Public Overrides Property DateLastUpdated As Date

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Shadows Function Equals(other As Resource) As Boolean Implements IEquatable(Of Resource).Equals
            Return Me.Identifier = other.Identifier
        End Function
    End Class

End Namespace