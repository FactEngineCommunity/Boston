Imports Boston.Parser.Meta
Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings

Namespace Parser.Syntax

    <Serializable()> Friend Class SourceTransforms

        <Serializable()> Public Class Attrib
            Public Target As SyntaxToken.TransformTargets = SyntaxToken.TransformTargets.NotSet
            Public AttribName As String = ""
            Public AttribVal As Object = Nothing
        End Class


        Public Name As String = ""
        Public Source As String = Nothing
        Public TransformInstructions As Syntax.SyntaxNode = Nothing

        Public Attribs As New List(Of Attrib)

        Public StopRequest As Boolean = False
        Public Completed As Boolean = False
        Public CompileException As Exception = Nothing


        Public Event Tick()


        Public Sub New(ByVal SourceName As String, ByVal source As String)
            Me.Name = SourceName
            Me.Source = source
        End Sub

        Public Sub Build()
            Me.Completed = False
            Try
                Me.CompileException = Nothing
                Call Me.Compile()
            Catch ex As Exception
                Me.CompileException = ex
            End Try
            Me.Completed = True
        End Sub

        Private Sub Compile()
            Me.TransformInstructions = New Parser.Syntax.SyntaxNode(ACTION_ROOT, False, 0, False, Nothing)
            If String.IsNullOrEmpty(Me.Source) Then Exit Sub

            'Keep users in anticipation..
            RaiseEvent Tick()

            'Read line by line, this will allow setting the physical line number for the code line
            Dim sr As New System.IO.StringReader(Me.Source)
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

                    If StrIdxOf(line, Syntax.Constants.ACTION_IF) = 0 Then
                        'Add block
                        Dim n As Parser.Syntax.SyntaxNode = Me.GetBlock(line, lineNumber, sr)
                        If n IsNot Nothing Then
                            For i As Integer = 1 To n.Tokens.Count - 1
                                n.Tokens(i).SetTransformTarget()
                            Next
                            Me.TransformInstructions.Nodes.Add(n)
                        End If

                    Else
                        'Keep adding node
                        Try
                            Dim node As New Parser.Syntax.SyntaxNode(line, False, lineNumber, False, Nothing)
                            For i As Integer = 1 To node.Tokens.Count - 1
                                node.Tokens(i).SetTransformTarget()
                            Next
                            Me.TransformInstructions.Nodes.Add(node)
                        Catch ex As Exception
                            Throw New Exception("Error at line " & lineNumber & " of transformations '" & Me.GetName & "': " & ex.Message)
                        End Try

                    End If

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

            RaiseEvent Tick()
        End Sub

        Private Function GetBlock(ByVal currentLine As String, ByRef LineNumber As Integer, ByRef sr As System.IO.StringReader) As Parser.Syntax.SyntaxNode
            Dim topNode As New Parser.Syntax.SyntaxNode(currentLine, False, LineNumber, False, Nothing)

            currentLine = sr.ReadLine
            LineNumber += 1

            While currentLine IsNot Nothing
                'Abort processing
                If Me.StopRequest Then Return Nothing

                currentLine = currentLine.Trim

                'No blanks or comments
                If currentLine.Length = 0 Or StrIdxOf(currentLine, Syntax.Constants.RESERVED_COMMENT_LINE) = 0 Then
                    currentLine = sr.ReadLine
                    LineNumber += 1
                    Continue While
                End If

                If StrIdxOf(currentLine, Syntax.Constants.ACTION_IF) = 0 Then
                    'Add block
                    Dim n As Parser.Syntax.SyntaxNode = Me.GetBlock(currentLine, LineNumber, sr)
                    If n IsNot Nothing Then
                        For i As Integer = 1 To n.Tokens.Count - 1
                            n.Tokens(i).SetTransformTarget()
                        Next
                        topNode.Nodes.Add(n)
                    End If

                ElseIf StrIdxOf(currentLine, Syntax.Constants.ACTION_END) = 0 Then
                    'Block finished, build branches
                    Try
                        Dim comp As New Syntax.Compilation("", "", False, False)
                        comp.BuildIfBranches(topNode, True)

                    Catch ex As Exception
                        Throw New Exception("Error at line " & topNode.LineNumber & " of transformations '" & Me.GetName & "': " & ex.Message)

                    End Try

                    'And return
                    Return topNode

                Else
                    'Keep adding node
                    Try
                        Dim node As New Parser.Syntax.SyntaxNode(currentLine, False, LineNumber, False, Nothing)
                        For i As Integer = 0 To node.Tokens.Count - 1
                            node.Tokens(i).SetTransformTarget()
                        Next
                        topNode.Nodes.Add(node)
                    Catch ex As Exception
                        Throw New Exception(ex.Message & " of transformations '" & Me.GetName & "'.")
                    End Try

                End If

                'Get next line
                currentLine = sr.ReadLine
                LineNumber += 1
            End While

            Throw New Exception("End expected for block contsruct at line " & topNode.LineNumber & " of transformations '" & Me.GetName & "'.")
        End Function

        Private Function GetAttrib(ByVal target As SyntaxToken.TransformTargets, ByVal AttribName As String) As Attrib
            'Seek and get
            For Each att In Me.Attribs
                If att.Target = target AndAlso StrEq(att.AttribName, AttribName) Then Return att
            Next
            Return Nothing
        End Function

        Private Sub SetAttrib(ByVal target As SyntaxToken.TransformTargets, ByVal attribName As String, ByVal AttribVal As Object)
            'Check target set in compilation
            If target = SyntaxToken.TransformTargets.NotSet Then Throw New Exception("Transform target not set in compilation.")

            'Seek and set
            Dim att As Attrib = Me.GetAttrib(target, attribName)
            If att IsNot Nothing Then
                att.AttribVal = AttribVal

            Else
                'Or set new
                att = New Attrib()
                att.Target = target
                att.AttribName = attribName
                att.AttribVal = AttribVal
                Me.Attribs.Add(att)

            End If
        End Sub

        Private Sub PrepareExpressionBuildTok(ByVal source As IEntity, ByVal sourceOwner As IEntity,
                                              ByVal tokens As SyntaxTokenCollection, ByVal i As Integer,
                                              ByRef tokRet As SyntaxTokenCollection, ByVal nodeLineNumber As Integer)
            If tokens(i).QualifiedExpressionTokens.Count <> 2 Then Throw New Exception("Syntax error.")

            'Qualified expression column.attrib or table.attrib, etc - get attribute value
            Dim val As Object = Nothing
            Dim exprRight As String = tokens(i).QualifiedExpressionTokens(1).Text
            Select Case tokens(i).TransformTarget
                Case SyntaxToken.TransformTargets.NotSet
                    Throw New Exception("Error at line " & nodeLineNumber & " of transformations '" & Me.GetName & "': Transform target not set in compilation. ")

                Case SyntaxToken.TransformTargets.Table
                    'Table - get attrib from the source (if source is a table), or from the source's owner (if source is columns..)
                    If TypeOf source Is Parser.Meta.Database.Table Then
                        val = source.GetAttributeValue(exprRight, New List(Of Object), False, False)
                    ElseIf TypeOf source Is Parser.Meta.Database.Column Then
                        val = sourceOwner.GetAttributeValue(exprRight, New List(Of Object), False, False)
                    Else
                        val = ""
                    End If

                Case SyntaxToken.TransformTargets.Routine
                    'Routine - get attrib from the source (if source is a routine), or from the source's owner (if source is params..)
                    If TypeOf source Is Parser.Meta.Database.Routine Then
                        val = source.GetAttributeValue(exprRight, New List(Of Object), False, False)
                    ElseIf TypeOf source Is Parser.Meta.Database.Parameter Then
                        val = sourceOwner.GetAttributeValue(exprRight, New List(Of Object), False, False)
                    Else
                        val = ""
                    End If

                Case SyntaxToken.TransformTargets.Column
                    'Column - get attrib from the source (if source is a column), otherwise return empty
                    If TypeOf source Is Parser.Meta.Database.Column Then
                        'Column - get attrib from source
                        If exprRight = "value" Then '20200703-VM-Added check for "value" below such that column.values can be changed in a transform.
                            val = source.GetAttributeValue("value", Nothing, Nothing, False)
                        Else
                            val = source.GetAttributeValue(exprRight, New List(Of Object), False, False)
                        End If
                    Else
                        val = ""
                    End If

                Case SyntaxToken.TransformTargets.Param
                    'Parameter - get attrib from the source (if source is a parameter), otherwise return empty
                    If TypeOf source Is Parser.Meta.Database.Parameter Then
                        'Parameter - get attrib from source
                        val = source.GetAttributeValue(exprRight, New List(Of Object), False, False)
                    Else
                        val = ""
                    End If

            End Select

            If TypeOf val Is String Then
                tokRet.Add(New SyntaxToken(val.ToString, tokens(i).CodeLineNumber, SyntaxToken.ElementTypes.String))
            Else
                tokRet.Add(New SyntaxToken(val.ToString, tokens(i).CodeLineNumber, SyntaxToken.ElementTypes.NotSet))
            End If
        End Sub

        Private Function PrepareExpression(ByVal source As IEntity, ByVal sourceOwner As IEntity,
                                           ByVal tokens As SyntaxTokenCollection, ByVal startIdx As Integer,
                                           ByVal endIdx As Integer,
                                           ByVal nodeLineNumber As Integer) As SyntaxTokenCollection
            'Set up tokens to evaluate: assign primitive values to column. or table. and use them
            Dim tokRet As New SyntaxTokenCollection()
            For i As Integer = startIdx To endIdx
                If tokens(i).Type = SyntaxToken.ElementTypes.Variable Then
                    'Most likely a variable (check first)
                    Call Me.PrepareExpressionBuildTok(source, sourceOwner, tokens, i, tokRet, nodeLineNumber)

                ElseIf tokens(i).Type = SyntaxToken.ElementTypes.Number Then
                    'Numeric, just add
                    tokRet.Add(New SyntaxToken(tokens(i).Text, tokens(i).CodeLineNumber, SyntaxToken.ElementTypes.NotSet))

                ElseIf tokens(i).Type = SyntaxToken.ElementTypes.String Then
                    'Has string literals, just add
                    tokRet.Add(New SyntaxToken(tokens(i).Text, tokens(i).CodeLineNumber, SyntaxToken.ElementTypes.NotSet))

                ElseIf tokens(i).QualifiedExpressionTokens.Count = 0 Then
                    'No qualifiers, just add
                    tokRet.Add(New SyntaxToken(tokens(i).Text, tokens(i).CodeLineNumber, SyntaxToken.ElementTypes.NotSet))

                Else
                    Call Me.PrepareExpressionBuildTok(source, sourceOwner, tokens, i, tokRet, nodeLineNumber)

                End If
            Next

            Return tokRet
        End Function

        Private Sub Process(ByVal source As IEntity, ByVal sourceOwner As IEntity)
            Me.Attribs.Clear()
            If Me.TransformInstructions Is Nothing Then Exit Sub

            For Each node In Me.TransformInstructions.Nodes
                Call Me.Process(source, sourceOwner, node)
            Next
        End Sub

        'Process - parse the transformation instructions to apply the attribute values according to the variable source (column/table/parameter/etc)
        Private Sub Process(ByVal source As IEntity, ByVal sourceOwner As IEntity, ByVal node As Syntax.SyntaxNode)
            If node.Action = Syntax.SyntaxNode.ExecActions.ACTION_ROOT Or node.Action = Syntax.SyntaxNode.ExecActions.COMMENT Then
                'Skip

            ElseIf node.Action = Syntax.SyntaxNode.ExecActions.ACTION_SET Then
                'Evaluate attribute value first - we need to substitute attribute gets with their intended primitive values.
                'ie we are not using variables called 'column' or 'table'.
                Try
                    'Get value
                    Dim val As Object =
                        Exec_Expr.EvalExpression(Me.PrepareExpression(source, sourceOwner, node.Tokens, 3, node.Tokens.Count - 1, node.LineNumber),
                                                 Nothing, 0)

                    'Set attribute value
                    'Target is (should be) the second token of the set expression
                    Me.SetAttrib(node.Tokens(1).TransformTarget, node.SetSubjAttrib, val)

                Catch ex As Exception
                    Throw New Exception("Error at line " & node.LineNumber & " of transformations '" & Me.GetName & "': " & ex.Message)

                End Try

            ElseIf node.Action = Syntax.SyntaxNode.ExecActions.ACTION_IF Then
                'Process condition, and execute sub nodes in block

                Dim lower As Integer = 0
                Dim upper As Integer = node.Nodes.Count - 1

                Dim toks As SyntaxTokenCollection = Me.PrepareExpression(source, sourceOwner, node.Tokens, 1, node.Tokens.Count - 1, node.LineNumber)

                If CBool(Exec_Expr.EvalExpression(toks, 0, toks.Count - 1, Nothing, 0)) Then
                    'If condition satisfied, process child nodes up to else, elseif, or end
                    If node.IfBranch.ElseIfNodeIndexes.Count > 0 Then
                        'Up to the first elseif
                        upper = node.IfBranch.ElseIfNodeIndexes.Item(0) - 1

                    ElseIf node.IfBranch.ElseNodeIndex > -1 Then
                        'Up to the else
                        upper = node.IfBranch.ElseNodeIndex - 1

                    Else 'Up to the end
                        upper = node.Nodes.Count - 1

                    End If

                ElseIf node.IfBranch.ElseIfNodeIndexes.Count > 0 Then


                    'Asses elseif conditions
                    For i As Integer = 0 To node.IfBranch.ElseIfNodeIndexes.Count - 1
                        toks = Me.PrepareExpression(source, sourceOwner, node.Nodes(node.IfBranch.ElseIfNodeIndexes(i)).Tokens,
                                                        1, node.Nodes(node.IfBranch.ElseIfNodeIndexes(i)).Tokens.Count - 1,
                                                        node.Nodes(node.IfBranch.ElseIfNodeIndexes(i)).LineNumber)

                        If CBool(Exec_Expr.EvalExpression(toks, 0, toks.Count - 1, Nothing, 0)) Then
                            'Determine which nodes to process
                            lower = node.IfBranch.ElseIfNodeIndexes(i)
                            upper = node.Nodes.Count - 1

                            If i < node.IfBranch.ElseIfNodeIndexes.Count - 1 Then
                                'Up to the next elseif
                                upper = node.IfBranch.ElseIfNodeIndexes.Item(i + 1)

                            ElseIf node.IfBranch.ElseNodeIndex > -1 Then
                                'Up to the else
                                upper = node.IfBranch.ElseNodeIndex

                            Else 'Up to the end
                                'Bug found: Will not process the last elseif when "-1" is left in below. Bug in original Metadrone code.
                                upper = node.Nodes.Count '- 1

                            End If
                            'process block
                            lower += 1 : upper -= 1
                            For j As Integer = lower To upper
                                Call Me.Process(source, sourceOwner, node.Nodes(j))
                            Next

                            Exit Sub

                        End If
                    Next

                    'None of the elseif conditions were met, go to else
                    If node.IfBranch.ElseNodeIndex > -1 Then
                        'Nodes to process are from else to end
                        lower = node.IfBranch.ElseNodeIndex + 1
                        upper = node.Nodes.Count - 1
                    End If

                ElseIf node.IfBranch.ElseNodeIndex > -1 Then
                    'Do else
                    lower = node.IfBranch.ElseNodeIndex + 1
                    upper = node.Nodes.Count - 1

                End If

                'process block
                For i As Integer = lower To upper
                    Call Me.Process(source, sourceOwner, node.Nodes(i))
                Next

            Else
                Throw New Exception("Syntax error at line " _
                                    & node.LineNumber & " of transformations '" & Me.GetName & "'." _
                                    & " Source Owner:" & sourceOwner.GetAttributeValue("value", Nothing, False, False) _
                                    & " Source:" & source.GetAttributeValue("value", Nothing, False, False)
                                    )

            End If
        End Sub

        Private Function GetName() As String
            Return GLOBALS_SOURCES & "." & Me.Name
        End Function



        Public Function GetAttributeValue(ByVal source As Parser.Meta.Database.Table, ByVal AttribName As String) As Object
            Call Me.Process(source, source)
            Dim attrib As Attrib = Me.GetAttrib(SyntaxToken.TransformTargets.Table, AttribName)
            If attrib Is Nothing Then
                Throw New Exception("Attribute not defined: '" & AttribName & "'. ")
            End If
            Return attrib.AttribVal
        End Function

        Public Function GetAttributeValue(ByVal source As Parser.Meta.Database.Routine, ByVal AttribName As String) As Object
            Call Me.Process(source, source)
            Dim attrib As Attrib = Me.GetAttrib(SyntaxToken.TransformTargets.Routine, AttribName)
            If attrib Is Nothing Then
                Throw New Exception("Attribute not defined: '" & AttribName & "'. ")
            End If
            Return attrib.AttribVal
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="sourceOwner"></param>
        ''' <param name="AttribName"></param>
        ''' <returns></returns>
        Public Function GetAttributeValue(ByVal source As Parser.Meta.Database.Column,
                                          ByVal sourceOwner As IEntity,
                                          ByVal AttribName As String
                                          ) As Object
            Call Me.Process(source, sourceOwner)
            Dim attrib As Attrib = Me.GetAttrib(SyntaxToken.TransformTargets.Column, AttribName)
            If attrib Is Nothing Then
                Throw New Exception("Attribute not defined: '" & AttribName & "'. ")
            End If
            Return attrib.AttribVal
        End Function

        Public Function GetAttributeValue(ByVal source As Parser.Meta.Database.Parameter, ByVal sourceOwner As IEntity, ByVal AttribName As String) As Object
            Call Me.Process(source, sourceOwner)
            Dim attrib As Attrib = Me.GetAttrib(SyntaxToken.TransformTargets.Param, AttribName)
            If attrib Is Nothing Then
                Throw New Exception("Attribute not defined: '" & AttribName & "'. ")
            End If
            Return attrib.AttribVal
        End Function

    End Class

End Namespace