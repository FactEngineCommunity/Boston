Imports Boston.Parser.Output
Imports Boston.Parser.Syntax
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Bin

Namespace Parser

    Friend Class SuperBuilder

        Private proj As Persistence.MDProject = Nothing

        Public PackagesToBuild As New List(Of Persistence.MDPackage)
        Public StopRequest As Boolean = False

        Public Sub New(ByVal proj As Persistence.MDProject)
            Me.proj = proj
        End Sub

        Public Sub Parse()
            Me.StopRequest = False
            If String.IsNullOrEmpty(Me.proj.Properties.SuperMain) Then Exit Sub

            Dim sn As New Parser.Syntax.SyntaxNode(ACTION_ROOT, False, 0, False, Nothing)

            'Read line by line, this will allow setting the physical line number for the code line
            Dim sr As New System.IO.StringReader(Me.proj.Properties.SuperMain)
            Try
                Dim lineNumber As Integer = 1
                Dim line As String = sr.ReadLine
                While line IsNot Nothing
                    'Abort processing
                    If Me.StopRequest Then Exit Sub

                    line = line.Trim

                    'No blanks or comments
                    If line.Length = 0 Or StrIdxOf(line, Syntax.Constants.RESERVED_COMMENT_LINE) = 0 Then
                        line = sr.ReadLine
                        lineNumber += 1
                        Continue While
                    End If

                    'Keep adding node
                    Dim node As New Parser.Syntax.SyntaxNode(line, False, lineNumber, False, Nothing)
                    If node.Action <> SyntaxNode.ExecActions.ACTION_CALL Then
                        Throw New Exception("Syntax error. Line: " & lineNumber.ToString)
                    End If
                    sn.Nodes.Add(node)

                    'Get next line
                    line = sr.ReadLine
                    lineNumber += 1
                End While

            Catch ex As Exception
                Throw ex

            Finally
                sr.Close()
                sr.Dispose()
                sr = Nothing

            End Try

            'Parse nodes to get package calls
            For Each n In sn.Nodes
                Dim pkg As Persistence.MDPackage = Me.GetPackage(n.Tokens(1).Text.Trim)
                If pkg Is Nothing Then
                    Throw New Exception("Package '" & n.Tokens(1).Text.Trim & "' is not defined. Line: " & n.LineNumber.ToString)
                End If
                Me.PackagesToBuild.Add(pkg)
            Next
        End Sub

        Private Function GetPackage(ByVal name As String) As Persistence.MDPackage
            For Each pkg In Me.proj.Packages
                If StrEq(pkg.Name, name) Then Return pkg
            Next
            For Each fld In Me.proj.Folders
                Return Me.GetPackage(name, fld)
            Next
            Return Nothing
        End Function

        Private Function GetPackage(ByVal name As String, ByVal fld As Persistence.ProjectFolder) As Persistence.MDPackage
            For Each pkg In fld.Packages
                If StrEq(pkg.Name, name) Then Return pkg
            Next
            For Each f In fld.Folders
                Return Me.GetPackage(name, f)
            Next
            Return Nothing
        End Function

    End Class

End Namespace