Namespace Persistence.AngryArmy_1_0

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
        Public VBCode As New List(Of CodeDOM_VB)
        Public CSCode As New List(Of CodeDOM_CS)

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
            For Each vb In Me.VBCode
                Copy.VBCode.Add(CType(vb.GetCopy, CodeDOM_VB))
            Next
            For Each cs In Me.CSCode
                Copy.CSCode.Add(CType(cs.GetCopy, CodeDOM_CS))
            Next
            Return Copy
        End Function

    End Class

End Namespace