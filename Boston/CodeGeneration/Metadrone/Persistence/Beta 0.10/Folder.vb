Namespace Persistence.Beta10

    Public Class ProjectFolder : Implements IMDPersistenceItem
        Public Name As String
        Public Folders As New List(Of ProjectFolder)
        Public Sources As New List(Of Source)
        Public Packages As New List(Of MDPackage)

        Public Sub New()

        End Sub

        Public Sub New(ByVal Name As String)
            Me.Name = Name
        End Sub

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New ProjectFolder()
            Copy.Name = Me.Name
            For Each fld In Me.Folders
                Copy.Folders.Add(CType(fld.GetCopy, ProjectFolder))
            Next
            For Each src In Me.Sources
                Copy.Sources.Add(CType(src.GetCopy, Source))
            Next
            For Each src In Me.Sources
                Copy.Packages.Add(CType(src.GetCopy, MDPackage))
            Next
            Return Copy
        End Function

    End Class



    Public Class Folder : Implements IMDPersistenceItem
        Public Name As String
        Public Folders As New List(Of Folder)
        Public Templates As New List(Of Template)

        Public Sub New()

        End Sub

        Public Sub New(ByVal Name As String)
            Me.Name = Name
        End Sub

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New Folder()
            Copy.Name = Me.Name
            For Each fld In Me.Folders
                Copy.Folders.Add(CType(fld.GetCopy, Folder))
            Next
            For Each tmp In Me.Templates
                Copy.Templates.Add(CType(tmp.GetCopy, Template))
            Next
            Return Copy
        End Function

    End Class

End Namespace