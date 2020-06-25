Imports Boston.Parser.Syntax.Constants

Namespace Persistence.Beta11

    Public Class MDPackage : Implements IMDPersistenceItem
        Public TagVal As New MDPackageTag()
        Public Name As String
        Public Properties As New Properties()
        Public Directives As New Directives()

        Public Folders As New List(Of Folder)
        Public Templates As New List(Of Template)
        Public VBCode As New List(Of CodeDOM_VB)
        Public CSCode As New List(Of CodeDOM_CS)

        Public Sub New()
            Me.TagVal.CreateNewGUID()
        End Sub

        Public Sub New(ByVal packageName As String)
            Call Me.new()
            Me.Name = packageName
        End Sub

        Public ReadOnly Property TemplateCount() As Integer
            Get
                Return Me.Templates.Count + Me.GetTemplateCountInFolder(Me.Folders)
            End Get
        End Property

        Private Function GetTemplateCountInFolder(ByVal Folders As List(Of Folder)) As Integer
            Dim cnt As Integer = 0
            For Each fld In Folders
                cnt += fld.Templates.Count
                cnt += Me.GetTemplateCountInFolder(fld.Folders)
            Next
            Return cnt
        End Function

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New MDPackage()
            Copy.Name = Me.Name
            Copy.TagVal.GUID = Me.TagVal.GUID
            Copy.Properties = CType(Me.Properties.GetCopy, Properties)
            Copy.Directives = CType(Me.Directives.GetCopy, Directives)
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