Imports Boston.Parser.Syntax.Constants

Namespace Persistence

    Public Class MDPackage : Implements IMDPersistenceItem
        Public TagVal As New MDPackageTag()
        Public Name As String
        Public Main As New Main()

        Public Folders As New List(Of Folder)
        Public Templates As New List(Of Template)
        Public VBCode As New List(Of CodeDOM_VB)
        Public CSCode As New List(Of CodeDOM_CS)

        Friend Path As String

        Public Sub New()
            Me.TagVal.CreateNewGUID()
        End Sub

        Public Sub New(ByVal packageName As String, ByVal CreateDefault As Boolean)
            Call Me.new()
            Me.Name = packageName
            If Not CreateDefault Then Exit Sub

            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine(RESERVED_COMMENT_LINE & "**** Preprocessors ******")
            sb.Append(RESERVED_PREPROC & RESERVED_PREPROC_IGNORECASE & " " & RESERVED_PREPROC_IGNORECASE_ON)
            sb.AppendLine("  " & RESERVED_COMMENT_LINE & " case sensitivity of string comparisons")
            sb.Append(RESERVED_PREPROC & RESERVED_PREPROC_SAFEBEGIN & " = """"")
            sb.AppendLine(" " & RESERVED_COMMENT_LINE & " begin of safe zone")
            sb.Append(RESERVED_PREPROC & RESERVED_PREPROC_SAFEEND & " = """"")
            sb.AppendLine("   " & RESERVED_COMMENT_LINE & " end of safe zone")

            sb.AppendLine()
            sb.AppendLine(RESERVED_COMMENT_LINE & "**** Define globals ******")
            sb.AppendLine("set MainConn = sources.Source")
            sb.AppendLine("set BasePath = """"")
            sb.AppendLine()
            sb.AppendLine(ACTION_CALL & " Template")
            sb.AppendLine()
            sb.AppendLine(RESERVED_COMMENT_LINE & " Perform any file operations")
            sb.AppendLine(RESERVED_COMMENT_LINE & SYS_MAKEDIR & "(""create\directory"")")
            sb.AppendLine(RESERVED_COMMENT_LINE & SYS_FILECOPY & "(""source file"",""dest file"")")
            sb.AppendLine()
            sb.AppendLine(RESERVED_COMMENT_LINE & " or any command line operations")
            sb.AppendLine(RESERVED_COMMENT_LINE & SYS_COMMAND & "(""file name"",""arguments"")")
            sb.AppendLine(RESERVED_COMMENT_LINE & SYS_COMMAND & "(""rename "",""source.file dest.file"")")
            Me.Main.Text = sb.ToString
            Me.Main.OwnerGUID = Me.TagVal.GUID

            Me.Templates.Add(Me.NewTemplate("Template", Me.TagVal.GUID))
        End Sub

        Friend Function NewTemplate(ByVal templateName As String, ByVal OwnerGUID As String) As Template
            Dim templ As New Template()
            templ.Name = templateName
            Dim sb As New System.Text.StringBuilder()
            sb = New System.Text.StringBuilder()
            sb.AppendLine(TAG_BEGIN_DEFAULT & ACTION_HEADER & RESERVED_SEPERATOR & " " & HEADER_IS & " " & templateName & "()")
            sb.AppendLine("    " & RESERVED_COMMENT_LINE & " " & ACTION_FOR & " " & OBJECT_TABLE & " " & "tbl " & ACTION_IN & " " & "MainConn")
            sb.AppendLine("    " & RESERVED_COMMENT_LINE & " " & SYS_RETURN & " " & "BasePath + ""Tables\"" + " & "tbl + "".txt""")
            sb.AppendLine("    " & SYS_RETURN & " " & "BasePath + ""destination.txt""")
            sb.AppendLine(ACTION_END & TAG_END_DEFAULT)
            sb.AppendLine()
            sb.AppendLine(TAG_BEGIN & ACTION_FOR & " " & OBJECT_TABLE & " " & "tbl " & ACTION_IN & " " & "MainConn" & TAG_END)
            sb.AppendLine(TAG_BEGIN & ACTION_FOR & " " & OBJECT_COLUMN & " " & "col " & ACTION_IN & " " & "tbl" & TAG_END)
            sb.AppendLine("  " & TAG_BEGIN & "tbl" & TAG_END & " - " & TAG_BEGIN & "col" & TAG_END & " : " & TAG_BEGIN & "col." & VARIABLE_ATTRIBUTE_DATATYPE & TAG_END)
            sb.AppendLine(TAG_BEGIN & ACTION_END & TAG_END)
            sb.AppendLine(TAG_BEGIN & ACTION_END & TAG_END)
            templ.Text = sb.ToString
            templ.OwnerGUID = OwnerGUID
            Return templ
        End Function

        Friend Function NewCodeDOM_VB(ByVal CodeName As String, ByVal OwnerGUID As String) As CodeDOM_VB
            Dim vb As New CodeDOM_VB()
            vb.Name = CodeName
            Dim sb As New System.Text.StringBuilder()
            sb = New System.Text.StringBuilder()
            sb.AppendLine("Namespace VB")
            sb.AppendLine("    ")
            sb.AppendLine("    Public Class " & CodeName)
            sb.AppendLine("    ")
            sb.AppendLine("    End Class")
            sb.AppendLine("    ")
            sb.AppendLine("End Namespace")
            vb.Text = sb.ToString
            vb.OwnerGUID = OwnerGUID
            Return vb
        End Function

        Friend Function NewCodeDOM_CS(ByVal CodeName As String, ByVal OwnerGUID As String) As CodeDOM_CS
            Dim cs As New CodeDOM_CS()
            cs.Name = CodeName
            Dim sb As New System.Text.StringBuilder()
            sb = New System.Text.StringBuilder()
            sb.AppendLine("namespace CS")
            sb.AppendLine("{")
            sb.AppendLine("    public static class " & CodeName)
            sb.AppendLine("    {")
            sb.AppendLine("    ")
            sb.AppendLine("    }")
            sb.AppendLine("}")
            cs.Text = sb.ToString
            cs.OwnerGUID = OwnerGUID
            Return cs
        End Function

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

        Friend ReadOnly Property TemplateList() As List(Of String)
            Get
                Dim Templates As New List(Of String)
                For Each t In Me.Templates
                    Templates.Add(t.Name)
                Next
                For Each f In Me.Folders
                    Call Me.GetTemplates(f, Templates)
                Next
                Return Templates
            End Get
        End Property

        Private Sub GetTemplates(ByVal Folder As Persistence.Folder, ByRef Templates As List(Of String))
            For Each t In Folder.Templates
                Templates.Add(t.Name)
            Next
            For Each f In Folder.Folders
                Call Me.GetTemplates(f, Templates)
            Next
        End Sub

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New MDPackage()
            Copy.Name = Me.Name
            Copy.TagVal.GUID = Me.TagVal.GUID
            Copy.Main = CType(Me.Main.GetCopy, Main)
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