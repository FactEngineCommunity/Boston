Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Output

Namespace Parser.Syntax

    Friend Class Compilation
        Private AsMain As Boolean = False

        Private Source As System.Text.StringBuilder = Nothing
        Private BaseCompiledNode As New SyntaxNode(Nothing, False)
        Private NotStrict As Boolean = False

        Private PreProcFinished As Boolean = False

        Friend SourceName As String = Nothing
        Friend StopRequest As Boolean = False
        Friend Completed As Boolean = False
        Friend CompileException As Exception = Nothing

        Public Event Tick()

        Public Sub New(ByVal SourceName As String, ByVal Source As String, ByVal AsMain As Boolean, ByVal NotStrict As Boolean)
            Me.SourceName = SourceName
            Me.Source = New System.Text.StringBuilder(Source)
            Me.Source.Replace(vbCrLf, vbLf)
            Me.Source.Replace(vbCr, vbLf)

            Me.AsMain = AsMain
            Me.NotStrict = NotStrict 'This is for preview mode, and syntax node action checking will not throw exceptions
        End Sub

        Public Sub BuildForThreadedCompilation()
            Me.Completed = False
            Try
                Me.CompileException = Nothing
                Me.BaseCompiledNode = Me.Compile()
            Catch ex As Exception
                Me.CompileException = ex
            End Try
            Me.Completed = True
        End Sub

        Public ReadOnly Property BaseSyntaxNode() As SyntaxNode
            Get
                Return Me.BaseCompiledNode
            End Get
        End Property

        Public Function Compile() As SyntaxNode
            Dim baseNode As New SyntaxNode(Nothing, Me.AsMain)

            If Me.AsMain Then
                'Main is a single code tag
                Dim tags As New List(Of TagVal)
                tags.Add(New TagVal(True, Me.Source.ToString))
                If Me.StopRequest Then Return baseNode

                'Compile the code
                Return Me.Compile(tags)

            Else
                'Break into code and plain text
                Dim tags As List(Of TagVal) = Me.BuildTags()
                If Me.StopRequest Then Return baseNode

                'Compile the code/text
                Return Me.Compile(tags)

            End If
        End Function

        Private Function Compile(ByVal tags As List(Of TagVal)) As SyntaxNode
            Dim baseNode As New SyntaxNode(Nothing, Me.AsMain)

            'Build nodes and parse code tags
            Dim SyntaxNodes As List(Of SyntaxNode) = Me.BuildNodes(tags)
            If Me.StopRequest Then Return baseNode

            'Build syntax tree
            Dim idx As Integer = 0
            While idx < SyntaxNodes.Count
                If Me.StopRequest Then Return baseNode
                Call Me.GraftNode(SyntaxNodes, baseNode, idx)
                idx += 1
            End While
            If Me.StopRequest Then Return baseNode


            'Keep user in anticipation..
            RaiseEvent Tick()


            'Template quirks
            If Not Me.AsMain Then
                'Fix plaintext new line characters after code blocks
                Call Me.FixPostNewlines(baseNode, Nothing, 0)

                'Fix block enders
                Call Me.FixPreSpaces(baseNode)
            End If


            'Build if branches
            For Each node In baseNode.Nodes
                If Me.StopRequest Then Return baseNode
                Call Me.BuildIfBranches(node, False)
            Next


            'Don't need text for code nodes anymore
            Call Me.ClearNodeText(baseNode)


            'More template quirks
            If Not Me.AsMain Then
                'Conform new line chars
                Call Me.ConformNewlineChars(baseNode)

                'Template header
                Try
                    Call Me.CompileTemplateHeader(baseNode)
                Catch ex As Exception
                    If Not Me.NotStrict Then Throw ex
                End Try
            End If


            'Ready to use
            Return baseNode
        End Function

        Public Function GetTemplateParams() As CodeCompletion.Template
            'Build nodes. Just get the first two. The first may be plaintext, the following header. Ignore the rest.
            Dim SyntaxNodes As List(Of SyntaxNode) = Me.BuildNodesForTemplateParams(Me.BuildTags(2))

            'Look for header is
            Dim idx As Integer = 0
            While idx < SyntaxNodes.Count
                If Me.StopRequest Then Return New CodeCompletion.Template()

                If SyntaxNodes(idx).Action = SyntaxNode.ExecActions.HEADER_IS Then
                    'Get template
                    Return Me.GetTemplateParams(SyntaxNodes(idx))
                End If
                idx += 1
            End While

            Return New CodeCompletion.Template()
        End Function

        Public Function GetTemplateParams(ByVal node As SyntaxNode) As CodeCompletion.Template
            Dim ret As New CodeCompletion.Template()

            'get name
            If node.Tokens(1).QualifiedExpressionTokens.Count > 0 Then Return ret
            ret.Name = node.Tokens(1).Text

            'get params
            If node.Tokens.Count > 4 AndAlso _
               node.Tokens(2).Type = SyntaxToken.ElementTypes.LeftParen And _
               node.Tokens(node.Tokens.Count - 1).Type = SyntaxToken.ElementTypes.RightParen Then
                For i As Integer = 3 To node.Tokens.Count - 2
                    If node.Tokens(i).QualifiedExpressionTokens.Count > 0 Then Continue For
                    Select Case node.Tokens(i).Type
                        Case SyntaxToken.ElementTypes.Variable
                            ret.Params.Add(node.Tokens(i).Text)
                        Case SyntaxToken.ElementTypes.ParamSep
                            'ok skip
                        Case Else
                            'fail here
                            Return ret
                    End Select
                Next
            End If

            Return ret
        End Function

#Region "Build tags"

        'Break up source into code/text tags

        Friend Class TagVal
            Public IsCode As Boolean = False
            Public Text As String = ""
            Public Sub New(ByVal IsCode As Boolean, ByVal Text As String)
                Me.IsCode = IsCode
                Me.Text = Text
            End Sub
        End Class

        Friend Function BuildTags(Optional ByVal exitAfterTagCount As Integer = -1) As List(Of TagVal)
            'Break up into tags and plain text and add to nodes
            Dim tags As New List(Of TagVal)
            Dim sr As New System.IO.StringReader(Me.Source.ToString)
            Try
                If TAG_BEGIN.Length = 0 Then Throw New Exception("Begin tag not defined.")
                If TAG_END.Length = 0 Then Throw New Exception("End tag not defined.")

                Dim sb As New System.Text.StringBuilder()  'Read buffer
                Dim inTag As Boolean = False
                Dim inStr As Boolean = False
                Dim tagBeginFirstChar As Char = TAG_BEGIN.Substring(0, 1).ToCharArray()(0)
                Dim tagEndFirstChar As Char = TAG_END.Substring(0, 1).ToCharArray()(0)
                Dim lineNumber As Integer = 1
                While sr.Peek > -1
                    If Me.StopRequest Then Return tags

                    If Not inStr Then
                        'Not in string literals, so look out for tag defs
                        If Not inTag Then
                            'Not in a tag, peek ahead to see if we are entering a tag
                            If Convert.ToChar(sr.Peek).Equals(tagBeginFirstChar) Then
                                'Look for begin tag
                                Dim ch(TAG_BEGIN.Length - 1) As Char
                                Dim chLen As Integer = sr.Read(ch, 0, TAG_BEGIN.Length)
                                inTag = chLen = TAG_BEGIN.Length And CStr(ch).Equals(TAG_BEGIN)

                                'Increment line numbers if new line character found
                                For i As Integer = 0 To ch.Length - 1
                                    If ch(i).ToString.Equals(vbCr) Or ch(i).ToString.Equals(vbLf) Then lineNumber += 1
                                Next

                                If inTag Then
                                    'If have entered a tag, add the preceeding plain text from the buffer
                                    If sb.Length > 0 Then
                                        tags.Add(New TagVal(False, sb.ToString))
                                        If tags.Count = exitAfterTagCount Then Exit While
                                        sb = New System.Text.StringBuilder() 'Reset buffer
                                    End If

                                    'Check if entering string
                                    Dim s() As String = CStr(ch).Split("""".ToCharArray)
                                    inStr = s.Count Mod 2 = 0
                                Else
                                    'Otherwise, just add to buffer
                                    sb.Append(CStr(ch))
                                End If
                            Else
                                'Nothing to check here, just add to buffer
                                Dim ch As String = Convert.ToChar(sr.Read()).ToString
                                sb.Append(ch)

                                'Increment line numbers if new line character found
                                If ch.Equals(vbCr) Or ch.Equals(vbLf) Then lineNumber += 1
                            End If

                        Else
                            'In a tag, peek ahead to see if we are getting to an end tag
                            If Convert.ToChar(sr.Peek).Equals(tagEndFirstChar) Then
                                'Look for end tag
                                Dim ch(TAG_END.Length - 1) As Char
                                Dim chLen As Integer = sr.Read(ch, 0, TAG_END.Length)
                                inTag = Not (chLen = TAG_END.Length And CStr(ch).Equals(TAG_END))

                                'Increment line numbers if new line character found
                                For i As Integer = 0 To ch.Length - 1
                                    If ch(i).ToString.Equals(vbCr) Or ch(i).ToString.Equals(vbLf) Then lineNumber += 1
                                Next

                                If Not inTag Then
                                    'If have left the tag, add the preceeding code from the buffer
                                    sb = New System.Text.StringBuilder(sb.ToString)
                                    If sb.Length > 0 Then
                                        tags.Add(New TagVal(True, sb.ToString))
                                        If tags.Count = exitAfterTagCount Then Exit While
                                        sb = New System.Text.StringBuilder() 'Reset buffer
                                    End If
                                Else
                                    'Otherwise, just add to buffer
                                    sb.Append(CStr(ch))

                                    'Check if entering string
                                    Dim s() As String = CStr(ch).Split("""".ToCharArray)
                                    inStr = s.Count Mod 2 = 0
                                End If
                            Else
                                'Nothing to check here, just add to buffer
                                Dim ch As String = Convert.ToChar(sr.Read()).ToString
                                sb.Append(ch)

                                'Increment line numbers if new line character found
                                If ch.Equals(vbCr) Or ch.Equals(vbLf) Then lineNumber += 1

                                'Check if entering string
                                Dim s() As String = ch.Split("""".ToCharArray)
                                inStr = s.Count Mod 2 = 0
                            End If

                        End If

                    Else
                        'In string literals, look for literal termination
                        Dim ch As String = Convert.ToChar(sr.Read()).ToString
                        sb.Append(ch)
                        inStr = Not ch.Equals("""")

                    End If

                End While
                If tags.Count = exitAfterTagCount Then Return tags

                'Add last..
                If inTag Then
                    If Not Me.NotStrict Then Throw New Exception("Closing (end) tag expected line: " & lineNumber.ToString)
                    If sb.Length > 0 Then tags.Add(New TagVal(True, sb.ToString))
                    sb = New System.Text.StringBuilder()
                End If
                If sb.Length > 0 Then tags.Add(New TagVal(False, sb.ToString))

            Catch ex As Exception
                If Not Me.NotStrict Then Throw ex

            Finally
                sr.Close()
                sr.Dispose()
                sr = Nothing

            End Try

            Return tags
        End Function

#End Region


#Region "Build nodes"

        Private Function BuildNodes(ByVal tags As List(Of TagVal)) As List(Of SyntaxNode)
            Dim SyntaxNodes As New List(Of SyntaxNode)
            Dim lineNumber As Integer = 1
            Dim sr As System.IO.StringReader = Nothing
            Try
                For Each tag In tags
                    If Me.StopRequest Then Return SyntaxNodes

                    If Not tag.IsCode Then
                        'Add plain text
                        Dim n As SyntaxNode = New SyntaxNode(tag.Text, True, 0, Me.AsMain, Nothing, Me.NotStrict)
                        SyntaxNodes.Add(n)

                        'Count line numbers
                        Dim s() As String = tag.Text.Split(vbLf.ToCharArray)
                        lineNumber += s.Count - 1

                    Else
                        'Statements are seperated by either the seperator operator ; or a new line, so add node for each new line
                        sr = New System.IO.StringReader(tag.Text)
                        Dim lineIncs As Integer = 0 'Track readlines if more than one (last readline won't check for eol)
                        While sr.Peek > -1
                            lineIncs += 1
                            Dim lines() As String = sr.ReadLine.Split(RESERVED_SEPERATOR.ToCharArray)
                            For i As Integer = 0 To lines.Count - 1
                                If lines(i).Trim.Length = 0 And i < lines.Count - 1 Then
                                    Throw New Exception("Statement expected. Line: " & lineNumber & ".")
                                End If

                                'Only add code (ignore comments)
                                If lines(i).Trim.Length > 0 Then
                                    Dim n As SyntaxNode = New SyntaxNode(lines(i).Trim, False, lineNumber, Me.AsMain, Nothing, Me.NotStrict)
                                    If n.Action <> SyntaxNode.ExecActions.COMMENT Then SyntaxNodes.Add(n)
                                End If
                            Next

                            If sr.Peek > -1 Or lineIncs > 1 Then lineNumber += 1 'Update line number count
                        End While

                    End If
                Next

            Catch ex As Exception
                If Not Me.NotStrict Then Throw ex

            Finally
                sr.Close()
                sr.Dispose()
                sr = Nothing

            End Try

            Return SyntaxNodes
        End Function

        Private Function BuildNodesForTemplateParams(ByVal tags As List(Of TagVal)) As List(Of SyntaxNode)
            Dim SyntaxNodes As New List(Of SyntaxNode)
            Dim lineNumber As Integer = 1
            Dim sr As System.IO.StringReader = Nothing
            Try
                For Each tag In tags
                    If Me.StopRequest Then Return SyntaxNodes

                    If tag.IsCode Then
                        'Statements are seperated by either the seperator operator ; or a new line, so add node for each new line
                        sr = New System.IO.StringReader(tag.Text)
                        Dim lineIncs As Integer = 0 'Track readlines if more than one (last readline won't check for eol)
                        While sr.Peek > -1
                            lineIncs += 1
                            Dim lines() As String = sr.ReadLine.Split(RESERVED_SEPERATOR.ToCharArray)
                            For i As Integer = 0 To lines.Count - 1
                                If lines(i).Trim.Length = 0 And i < lines.Count - 1 Then
                                    Throw New Exception("Statement expected. Line: " & lineNumber & ".")
                                End If

                                'Look for header is
                                Dim n As SyntaxNode = New SyntaxNode(lines(i).Trim, False, lineNumber, Me.AsMain, Nothing, Me.NotStrict)
                                If n.Action = SyntaxNode.ExecActions.HEADER_IS Then
                                    SyntaxNodes.Add(n)
                                    Exit For
                                End If
                            Next

                            If sr.Peek > -1 Or lineIncs > 1 Then lineNumber += 1 'Update line number count
                        End While

                    End If
                Next

            Catch ex As Exception
                If Not Me.NotStrict Then Throw ex

            Finally
                sr.Close()
                sr.Dispose()
                sr = Nothing

            End Try

            Return SyntaxNodes
        End Function

#End Region


#Region "Build syntax tree"

        'We have determined syntax nodes, now just group them within appropriate blocks
        Private Function GraftNode(ByVal SyntaxNodes As List(Of SyntaxNode), ByVal TargetNode As SyntaxNode, ByRef NodeIndex As Integer) As Boolean
            'Check if bogus
            If SyntaxNodes(NodeIndex).Text.Length = 0 Then Return False

            'Plain text just a simple add
            If SyntaxNodes(NodeIndex).Action = SyntaxNode.ExecActions.PLAINTEXT Then
                TargetNode.Nodes.Add(New SyntaxNode(SyntaxNodes(NodeIndex).Text, _
                                                    True, _
                                                    SyntaxNodes(NodeIndex).LineNumber, _
                                                    Me.AsMain, _
                                                    TargetNode, _
                                                    Me.NotStrict))
                Return False
            End If

            'Set up node
            Dim node As SyntaxNode = SyntaxNodes(NodeIndex).GetCopy()
            node.SetOwner(TargetNode)

            'Determine if starting/ending a block
            Dim GetBlock As Boolean = False
            Select Case node.Action
                Case SyntaxNode.ExecActions.ACTION_HEADER, SyntaxNode.ExecActions.ACTION_FOR, SyntaxNode.ExecActions.ACTION_IF
                    Me.PreProcFinished = True

                    'Of these, only for is allowed in header
                    If TargetNode.Action = SyntaxNode.ExecActions.ACTION_HEADER Then
                        If node.Action = SyntaxNode.ExecActions.ACTION_FOR Then
                            'For constructs don't need a block end inside a header
                            GetBlock = False
                        Else
                            Throw New Exception("Syntax error. Line: " & node.LineNumber.ToString & ".")
                        End If
                    Else
                        GetBlock = True
                    End If

                Case SyntaxNode.ExecActions.ACTION_END
                    Me.PreProcFinished = True

                    'Check the owner
                    Select Case TargetNode.Action
                        Case SyntaxNode.ExecActions.ACTION_HEADER, SyntaxNode.ExecActions.ACTION_FOR, SyntaxNode.ExecActions.ACTION_IF
                            'Add node to tree
                            TargetNode.Nodes.Add(node)
                            Return True

                        Case Else
                            Throw New Exception("End does not belong to a block construct. Line: " & node.LineNumber.ToString & ".")

                    End Select

                Case Else
                    'Make sure preprocessors are first
                    If node.Tokens(0).Type = SyntaxToken.ElementTypes.Preproc Then
                        If Me.PreProcFinished Then
                            Throw New Exception("Preprocessors must be defined before any action. Line: " & node.LineNumber.ToString)
                        End If
                    Else
                        Me.PreProcFinished = True
                    End If

            End Select

            'Add block code to this node
            Dim FoundEnd As Boolean = False
            Dim BlockStartNodeIndex As Integer = NodeIndex
            If GetBlock Then
                NodeIndex += 1
                While NodeIndex < SyntaxNodes.Count
                    If Me.StopRequest Then Return False

                    If Me.GraftNode(SyntaxNodes, node, NodeIndex) Then
                        FoundEnd = True
                        Exit While
                    End If
                    NodeIndex += 1
                End While

                If Not FoundEnd And Not Me.NotStrict Then
                    Throw New Exception("End expected for block contsruct. Line: " & SyntaxNodes(BlockStartNodeIndex).LineNumber.ToString & ".")
                End If
            End If

            'Add node to tree
            TargetNode.Nodes.Add(node)
            Return False
        End Function

#End Region


#Region "Fix post new lines and pre spaces"

        'If the following plaintext node's first character is a new line, remove it.
        'These conditions must be met:
        '  1. Non-returning code
        '  2. The block isn't on the same line.
        '
        'eg  <<for>>(newline)
        'or  <<set>>(newline)
        'not <<tbl.value>>(newline)
        'or  <<end>>(newline)
        'or  <<call>>(newline)
        'not <<if>> blah <<end>>(newline)
        Private Sub FixPostNewlines(ByVal CurrentNode As SyntaxNode, ByVal OwnerNode As SyntaxNode, ByVal CurrentNodeIdx As Integer)
            For i As Integer = 0 To CurrentNode.Nodes.Count - 1
                Select Case CurrentNode.Nodes(i).Action
                    Case SyntaxNode.ExecActions.ACTION_HEADER, SyntaxNode.ExecActions.ACTION_FOR, SyntaxNode.ExecActions.ACTION_IF
                        'Don't if non header block all on the same line
                        If CurrentNode.Nodes(i).Action <> SyntaxNode.ExecActions.ACTION_HEADER And _
                           CurrentNode.Nodes(i).Nodes.Count > 0 AndAlso _
                           CurrentNode.Nodes(i).Nodes(CurrentNode.Nodes(i).Nodes.Count - 1).LineNumber = CurrentNode.Nodes(i).LineNumber Then
                            Continue For
                        End If

                        'Look for the next plaintext node within this block
                        For j As Integer = 0 To CurrentNode.Nodes(i).Nodes.Count - 1
                            If CurrentNode.Nodes(i).Nodes(j).Action = SyntaxNode.ExecActions.PLAINTEXT Then
                                'Fix newline char
                                If CurrentNode.Nodes(i).Nodes(j).Text.IndexOf(vbLf) = 0 Then
                                    CurrentNode.Nodes(i).Nodes(j).Text = _
                                     CurrentNode.Nodes(i).Nodes(j).Text.Substring(1, CurrentNode.Nodes(i).Nodes(j).Text.Length - 1)
                                End If
                                Exit For
                            End If
                        Next

                        'Process sub-nodes
                        Call Me.FixPostNewlines(CurrentNode.Nodes(i), CurrentNode, i)

                    Case SyntaxNode.ExecActions.ACTION_ELSE, SyntaxNode.ExecActions.ACTION_ELSEIF
                        'Else's and elseif's don't have sub nodes, but are considered block enders.
                        'Look for the next plaintext node within this block
                        For j As Integer = i To CurrentNode.Nodes.Count - 1
                            If CurrentNode.Nodes(j).Action = SyntaxNode.ExecActions.PLAINTEXT Then
                                'Fix newline char
                                If CurrentNode.Nodes(j).Text.IndexOf(vbLf) = 0 Then
                                    CurrentNode.Nodes(j).Text = CurrentNode.Nodes(j).Text.Substring(1, CurrentNode.Nodes(j).Text.Length - 1)
                                End If
                                Exit For
                            End If
                        Next

                    Case SyntaxNode.ExecActions.ACTION_END
                        'For enders, check the newline in the owner's next peer node
                        If OwnerNode Is Nothing Then Exit Select

                        If CurrentNodeIdx >= OwnerNode.Nodes.Count - 1 Then Exit Select

                        If OwnerNode.Nodes(CurrentNodeIdx + 1).Action = SyntaxNode.ExecActions.PLAINTEXT Then
                            'Fix newline char
                            If OwnerNode.Nodes(CurrentNodeIdx + 1).Text.IndexOf(vbLf) = 0 Then
                                OwnerNode.Nodes(CurrentNodeIdx + 1).Text = _
                                 OwnerNode.Nodes(CurrentNodeIdx + 1).Text.Substring(1, OwnerNode.Nodes(CurrentNodeIdx + 1).Text.Length - 1)
                            End If
                        End If

                    Case SyntaxNode.ExecActions.ACTION_SET, SyntaxNode.ExecActions.ACTION_CALL
                        'For non-returning operations, check the newline in the next peer node
                        If i >= CurrentNode.Nodes.Count - 1 Then Exit Select

                        If CurrentNode.Nodes(i + 1).Action = SyntaxNode.ExecActions.PLAINTEXT Then
                            'Fix newline char
                            If CurrentNode.Nodes(i + 1).Text.IndexOf(vbLf) = 0 Then
                                CurrentNode.Nodes(i + 1).Text = _
                                 CurrentNode.Nodes(i + 1).Text.Substring(1, CurrentNode.Nodes(i + 1).Text.Length - 1)
                            End If
                        End If

                    Case Else
                        'Check for non-returning attribute actions
                        If i >= CurrentNode.Nodes.Count - 1 Then Exit Select

                        If CurrentNode.Nodes(i).Tokens.Count > 0 AndAlso CurrentNode.Nodes(i).Tokens(0).QualifiedExpressionTokens.Count > 1 Then
                            If StrEq(CurrentNode.Nodes(i).Tokens(0).QualifiedExpressionTokens(1).Text, FUNC_IGNORE) Or _
                               StrEq(CurrentNode.Nodes(i).Tokens(0).QualifiedExpressionTokens(1).Text, FUNC_USEONLY) Or _
                               StrEq(CurrentNode.Nodes(i).Tokens(0).QualifiedExpressionTokens(1).Text, FUNC_REPLACEALL) Then
                                'Fix newline char
                                If CurrentNode.Nodes(i + 1).Text.IndexOf(vbLf) = 0 Then
                                    CurrentNode.Nodes(i + 1).Text = _
                                     CurrentNode.Nodes(i + 1).Text.Substring(1, CurrentNode.Nodes(i + 1).Text.Length - 1)
                                End If
                            End If
                        End If

                End Select
            Next
        End Sub

        'Remove white space immediately before code concerning block start/end
        '
        'eg:
        '  this plaintext
        '       <<!for next node...
        '^_remove white space
        '
        '  this plaintext
        '       <<!end
        '^_remove white space
        '
        ' this plaintext
        '       <<!tbl.value!>>
        '^_keep white space
        '
        ' this plaintext
        '       <<if>>(newline)
        '^_remove white space
        '
        ' this plaintext
        '       <<if>> blah(newline)
        '^_keep white space
        '
        ' this plaintext
        '       <<if>> blah <<end>>(newline)
        '^_keep white space
        Private Sub FixPreSpaces(ByVal CurrentNode As SyntaxNode)
            For i As Integer = 0 To CurrentNode.Nodes.Count - 1
                Select Case CurrentNode.Nodes(i).Action
                    Case SyntaxNode.ExecActions.ACTION_HEADER, SyntaxNode.ExecActions.ACTION_FOR, _
                         SyntaxNode.ExecActions.ACTION_IF, SyntaxNode.ExecActions.ACTION_ELSE, _
                         SyntaxNode.ExecActions.ACTION_ELSEIF, SyntaxNode.ExecActions.ACTION_END

                        'Don't if block all on the same line
                        If CurrentNode.Nodes(i).Nodes.Count > 0 AndAlso _
                           CurrentNode.Nodes(i).Nodes(CurrentNode.Nodes(i).Nodes.Count - 1).LineNumber = CurrentNode.Nodes(i).LineNumber Then
                            Continue For
                        End If

                        'Check the last line of the first plaintext node prior
                        For j As Integer = i To 0 Step -1
                            If CurrentNode.Nodes(j).Action = SyntaxNode.ExecActions.PLAINTEXT Then
                                'Fix spaces
                                Dim lidx As Integer = CurrentNode.Nodes(j).Text.LastIndexOf(vbLf)
                                If lidx = -1 Then
                                    CurrentNode.Nodes(j).Text = CurrentNode.Nodes(j).Text.TrimStart
                                Else
                                    Dim sb As New System.Text.StringBuilder(CurrentNode.Nodes(j).Text.Substring(0, lidx + 1))
                                    sb.Append(CurrentNode.Nodes(j).Text.Substring(lidx + 1, CurrentNode.Nodes(j).Text.Length - lidx - 1).TrimStart)
                                    CurrentNode.Nodes(j).Text = sb.ToString
                                End If
                                Exit For
                            End If
                        Next

                        'Process sub-nodes
                        If CurrentNode.Nodes(i).Action = SyntaxNode.ExecActions.ACTION_HEADER Or _
                           CurrentNode.Nodes(i).Action = SyntaxNode.ExecActions.ACTION_FOR Or _
                           CurrentNode.Nodes(i).Action = SyntaxNode.ExecActions.ACTION_IF Then
                            Call Me.FixPreSpaces(CurrentNode.Nodes(i))
                        End If
                End Select
            Next
        End Sub

#End Region


#Region "Build if branches"

        Friend Sub BuildIfBranches(ByVal CurrentNode As SyntaxNode, ByVal BuildTransformTargets As Boolean)
            If CurrentNode.Action = SyntaxNode.ExecActions.ACTION_IF Then
                'Find matching else
                CurrentNode.IfBranch.ElseNodeIndex = -1
                For i As Integer = 0 To CurrentNode.Nodes.Count - 1
                    If CurrentNode.Nodes(i).Action = SyntaxNode.ExecActions.ACTION_ELSE Then
                        CurrentNode.IfBranch.ElseNodeIndex = i
                        Exit For
                    End If
                Next

                'Don't allow other else's
                If CurrentNode.IfBranch.ElseNodeIndex > -1 Then
                    For i As Integer = CurrentNode.IfBranch.ElseNodeIndex + 1 To CurrentNode.Nodes.Count - 1
                        If CurrentNode.Nodes(i).Action = SyntaxNode.ExecActions.ACTION_ELSE Then
                            Throw New Exception(ACTION_ELSE & " must have matching " & ACTION_IF & _
                                                ". Line: " & CurrentNode.Nodes(i).LineNumber.ToString)
                        End If
                    Next
                End If

                'Build elseif list
                CurrentNode.IfBranch.ElseIfNodeIndexes = New List(Of Integer)
                For i As Integer = 0 To CurrentNode.Nodes.Count - 1
                    If CurrentNode.Nodes(i).Action = SyntaxNode.ExecActions.ACTION_ELSEIF Then
                        CurrentNode.IfBranch.ElseIfNodeIndexes.Add(i)
                    End If
                Next

                'No else before any elseif
                If CurrentNode.IfBranch.ElseNodeIndex > -1 Then
                    For Each i In CurrentNode.IfBranch.ElseIfNodeIndexes
                        If i >= CurrentNode.IfBranch.ElseNodeIndex Then
                            Throw New Exception(ACTION_ELSE & " cannot be before " & ACTION_ELSEIF & _
                                                ". Line: " & CurrentNode.Nodes(CurrentNode.IfBranch.ElseNodeIndex).LineNumber.ToString)
                        End If
                    Next
                End If
            End If

            'Check any sub nodes
            For Each node In CurrentNode.Nodes
                Call Me.BuildIfBranches(node, BuildTransformTargets)
            Next
        End Sub

#End Region


#Region "Compile template header"

        Private Sub CompileTemplateHeader(ByVal CurrentNode As SyntaxNode)
            For Each node In CurrentNode.Nodes
                If node.Action = Syntax.SyntaxNode.ExecActions.ACTION_HEADER Then
                    'Look for IS keyword, and set
                    For Each hn In node.Nodes
                        If hn.Action = Syntax.SyntaxNode.ExecActions.HEADER_IS Then
                            'Get template name
                            If hn.Tokens.Count < 2 Then Throw New Exception("Template name expected")
                            If IsReservedWord(hn.Tokens(1).Text) Or ContainsOperator(hn.Tokens(1).Text) Then
                                Throw New Exception("'" & hn.Tokens(1).Text & "' is an invalid template name.")
                            End If
                            CurrentNode.TemplateName = hn.Tokens(1).Text

                            'Check syntax
                            If hn.Tokens.Count > 2 Then
                                If Not hn.Tokens(2).Type = SyntaxToken.ElementTypes.LeftParen Then
                                    Throw New Exception("Template arguments require parenthesis.")
                                End If
                                If Not hn.Tokens(hn.Tokens.Count - 1).Type = SyntaxToken.ElementTypes.RightParen Then
                                    Throw New Exception("Template arguments require parenthesis.")
                                End If
                            End If

                            'Build parameters
                            Dim expectVal As Boolean = True
                            For j As Integer = 3 To hn.Tokens.Count - 2
                                If expectVal Then
                                    'Add parameter
                                    CurrentNode.TemplateParameters.Add(New Parser.Syntax.SyntaxNode.Parameter(hn.Tokens(j).Text, Nothing))
                                    expectVal = Not expectVal
                                Else
                                    'Expect comma
                                    If Not hn.Tokens(j).Type = Syntax.SyntaxToken.ElementTypes.ParamSep Then
                                        Throw New Exception("Comma or ')' expected. Line: " & hn.LineNumber.ToString)
                                    End If
                                    expectVal = Not expectVal
                                End If
                            Next

                            'All done
                            Exit For

                        End If
                    Next

                    'Set sub nodes owner template params to this
                    If CurrentNode.TemplateParameters.Count > 0 Then
                        For Each n In CurrentNode.Nodes
                            For i As Integer = 0 To CurrentNode.TemplateParameters.Count - 1
                                n.Owner_TemplateParamNames.Add(CurrentNode.TemplateParameters(i).Name)
                                n.Owner_TemplateParamValues.Add(Nothing) 'will assign value when being called
                            Next
                        Next
                    End If

                End If

                If String.IsNullOrEmpty(CurrentNode.TemplateName) Then Throw New Exception("Template name needs to be defined in header block.")
            Next
        End Sub

#End Region


        Private Sub ClearNodeText(ByVal node As SyntaxNode)
            If node.Action <> SyntaxNode.ExecActions.PLAINTEXT Then node.Text = Nothing
            For Each n In node.Nodes
                If Me.StopRequest Then Exit Sub
                Call Me.ClearNodeText(n)
            Next
        End Sub

        Private Sub ConformNewlineChars(ByVal node As SyntaxNode)
            If node.Action = SyntaxNode.ExecActions.PLAINTEXT Then
                node.Text = node.Text.Replace(vbCrLf, vbLf).Replace(vbCr, vbLf)
                node.Text = node.Text.Replace(vbLf, System.Environment.NewLine)
            Else
                For Each n In node.Nodes
                    If Me.StopRequest Then Exit Sub
                    Call Me.ConformNewlineChars(n)
                Next
            End If
        End Sub

    End Class

End Namespace
