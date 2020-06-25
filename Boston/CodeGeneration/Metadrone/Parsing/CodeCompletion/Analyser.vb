Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax

Namespace Parser.CodeCompletion

    Friend Class Analyser

        Private mdProject As Persistence.MDProject = Nothing
        Private packageOwnerGUID As String = ""
        Private mainSource As String = ""
        Private templateSource As String = ""

        Private varScopeDepth As Integer = 0

        Public CancelRequest As Boolean = False
        Public Completed As Boolean = True

        Public Sources As New List(Of Source)
        Public Templates As New List(Of Template)
        Public MainVars As New Variables()
        Public TemplateVars As New Variables()

        Public Sub New()
          
        End Sub

        Public Sub Init(ByVal mdProject As Persistence.MDProject, _
                        ByVal packageOwnerGUID As String, _
                        ByVal mainSource As String, ByVal templateSource As String)
            Me.mdProject = mdProject
            Me.packageOwnerGUID = packageOwnerGUID
            Me.mainSource = mainSource
            Me.templateSource = templateSource
            Me.Sources.Clear()
            Me.Templates.Clear()
            Me.MainVars.Clear()
            Me.TemplateVars.Clear()
        End Sub

        Public Sub Begin()
            Me.CancelRequest = False
            Me.Completed = False

            'Get all sources
            For Each src In Me.mdProject.Sources
                Call Me.GetSource(src)
            Next
            For Each fld In Me.mdProject.Folders
                Call Me.GetSources(fld)
            Next

            'Get all templates in the package
            For Each pkg In Me.mdProject.Packages
                If pkg.TagVal.GUID.Equals(Me.packageOwnerGUID) Then
                    For Each tmp In pkg.Templates
                        'Get template name and params
                        Dim t As New Template()
                        If Boston.UI.MainForm IsNot Nothing Then
                            Dim idx As Integer = Boston.UI.MainForm.TagFoundInTabPages(tmp)
                            If idx > -1 Then
                                'If in editor, use code there
                                If TypeOf Boston.UI.MainForm.tcMain.TabPages(idx).Controls(0) Is UI.CodeEditor Then
                                    Dim editor As UI.CodeEditor = CType(Boston.UI.MainForm.tcMain.TabPages(idx).Controls(0), UI.CodeEditor)
                                    Dim tag As Boston.Persistence.Template = CType(editor.Tag, Boston.Persistence.Template)
                                    t = New Compilation("", tag.Text, False, True).GetTemplateParams()
                                End If
                            Else
                                'Just from loaded
                                t = New Compilation("", tmp.Text, False, True).GetTemplateParams()
                            End If
                        Else
                            t = New Compilation("", tmp.Text, False, True).GetTemplateParams()
                        End If

                        'Good to go
                        If Not String.IsNullOrEmpty(t.Name) Then Me.Templates.Add(t)
                    Next
                    Exit For
                End If
            Next

            'Compile to get vars
            'Ignore compilation errors for now
            Try
                Me.varScopeDepth = 0
                For Each node In New Compilation("", Me.mainSource, True, True).Compile().Nodes
                    If Me.CancelRequest Then
                        Me.Completed = True
                        Exit Sub
                    End If
                    Call Me.GetVars(True, node)
                Next
            Catch ex As Exception
            End Try
            If Not String.IsNullOrEmpty(Me.templateSource) Then
                Try
                    Me.varScopeDepth = 0
                    For Each node In New Compilation("", Me.templateSource, False, True).Compile().Nodes
                        If Me.CancelRequest Then
                            Me.Completed = True
                            Exit Sub
                        End If
                        Call Me.GetVars(False, node)
                    Next
                Catch ex As Exception
                End Try
            End If
            Me.Completed = True
        End Sub


        Private Sub GetSource(ByVal src As Persistence.Source)
            Dim idx As Integer = Boston.UI.MainForm.TagFoundInTabPages(src)
            If idx > -1 Then
                'If in editor, use values there
                If TypeOf Boston.UI.MainForm.tcMain.TabPages(idx).Controls(0) Is UI.ManageSource Then
                    Dim tag As Object = CType(Boston.UI.MainForm.tcMain.TabPages(idx).Controls(0), UI.ManageSource).Tag
                    Dim s As Parser.Source.Source = CType(tag, Boston.Persistence.Source).ToParserSource
                    Me.Sources.Add(New Source(s.Name, s.Provider, s.Transformations))
                End If
            Else
                'Just from loaded
                Me.Sources.Add(New Source(src.Name, src.Provider, src.Transformations))
            End If
        End Sub

        Private Sub GetSources(ByVal fld As Persistence.ProjectFolder)
            For Each src In fld.Sources
                Call Me.GetSource(src)
            Next
            For Each f In fld.Folders
                Call Me.GetSources(f)
            Next
        End Sub

        Private Sub GetVars(ByVal AsMain As Boolean, ByVal ExecNode As SyntaxNode)
            If Me.CancelRequest Then Exit Sub

            'Retrieve variables, don't worry about value
            Select Case ExecNode.Action
                Case SyntaxNode.ExecActions.PLAINTEXT
                    'Not this
                    Exit Sub

                Case SyntaxNode.ExecActions.HEADER_IS
                    Dim t As Template = New Compilation("", "", False, True).GetTemplateParams(ExecNode)
                    For Each prm In t.Params
                        'Keep the scope away from deletion after the end block
                        Call Me.AddVar(AsMain, New Variable(prm, Variable.Types.TemplateParameter, -1, ""))
                    Next

                Case SyntaxNode.ExecActions.ACTION_FOR
                    'First try to get the source to tag to the variable
                    Dim sourceTag As String = ""

                    'Examine variable or source ref after 'in'
                    If ExecNode.Tokens.Count > 4 Then
                        If ExecNode.Tokens(4).QualifiedExpressionTokens.Count = 2 Then
                            'sources.source_name here, should only concern table/view/routine loops
                            Select Case ExecNode.ForEntity
                                Case SyntaxNode.ExecForEntities.OBJECT_TABLE, SyntaxNode.ExecForEntities.OBJECT_VIEW, _
                                     SyntaxNode.ExecForEntities.OBJECT_PROCEDURE, SyntaxNode.ExecForEntities.OBJECT_FUNCTION
                                    If StrEq(ExecNode.Tokens(4).QualifiedExpressionTokens(0).Text, Constants.GLOBALS_SOURCES) Then
                                        sourceTag = ExecNode.Tokens(4).QualifiedExpressionTokens(1).Text
                                    End If
                            End Select
                        Else
                            'look for variable reference to get the source tag
                            If AsMain Then
                                For Each v As Variable In Me.MainVars
                                    If StrEq(v.Name, ExecNode.Tokens(4).Text) Then
                                        sourceTag = v.SourceTag
                                        Exit For
                                    End If
                                Next
                            Else
                                'Look in template vars first
                                For Each v As Variable In Me.TemplateVars
                                    If StrEq(v.Name, ExecNode.Tokens(4).Text) Then
                                        sourceTag = v.SourceTag
                                        Exit For
                                    End If
                                Next
                                If String.IsNullOrEmpty(sourceTag) Then
                                    'Then try main
                                    For Each v As Variable In Me.MainVars
                                        If StrEq(v.Name, ExecNode.Tokens(4).Text) Then
                                            sourceTag = v.SourceTag
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If

                    'Now we can add the var
                    Select Case ExecNode.ForEntity
                        Case SyntaxNode.ExecForEntities.OBJECT_TABLE
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.Table, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_VIEW
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.View, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_PROCEDURE
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.Procedure, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_FUNCTION
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.Function, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_COLUMN
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.Column, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_PKCOLUMN
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.PKColumn, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_FKCOLUMN
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.FKColumn, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_IDCOLUMN
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.IDColumn, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_FILE
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.File, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_PARAMETER
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.Parameter, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_INPARAMETER
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.InParameter, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_OUTPARAMETER
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.OutParameter, Me.varScopeDepth, sourceTag))

                        Case SyntaxNode.ExecForEntities.OBJECT_INOUTPARAMETER
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(2).Text, Variable.Types.InOutParameter, Me.varScopeDepth, sourceTag))

                    End Select

                Case SyntaxNode.ExecActions.ACTION_END
                    'Remove variables inside this scope
                    Dim i As Integer = 0
                    If AsMain Then
                        While i < Me.MainVars.Count
                            If Me.MainVars.Item(i).ScopeDepth = Me.varScopeDepth - 1 Then
                                Me.MainVars.RemoveAt(i)
                                Continue While
                            End If
                            i += 1
                        End While
                    Else
                        While i < Me.TemplateVars.Count
                            If Me.TemplateVars.Item(i).ScopeDepth = Me.varScopeDepth - 1 Then
                                Me.TemplateVars.RemoveAt(i)
                                Continue While
                            End If
                            i += 1
                        End While
                    End If

                Case SyntaxNode.ExecActions.ACTION_SET
                    'Set variable
                    If ExecNode.Tokens(3).QualifiedExpressionTokens.Count = 2 Then
                        'Check if a source variable
                        If StrEq(ExecNode.Tokens(3).QualifiedExpressionTokens(0).Text, Constants.GLOBALS_SOURCES) Then
                            Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(1).Text, Variable.Types.Source, Me.varScopeDepth, _
                                                                ExecNode.Tokens(3).QualifiedExpressionTokens(1).Text))
                        End If
                    Else
                        'Otherwise, just a primitive
                        Call Me.AddVar(AsMain, New Variable(ExecNode.Tokens(1).Text, Variable.Types.Primitive, Me.varScopeDepth, ""))
                    End If

            End Select

            If ExecNode.Nodes.Count = 0 Then Exit Sub

            'Look in sub nodes

            Me.varScopeDepth += 1

            For Each node In ExecNode.Nodes
                Call Me.GetVars(AsMain, node)
            Next

            Me.varScopeDepth -= 1
        End Sub

        Private Sub AddVar(ByVal AsMain As Boolean, ByVal variable As Variable)
            If AsMain Then
                Me.MainVars.Add(variable)
            Else
                Me.TemplateVars.Add(variable)
            End If
        End Sub

    End Class

End Namespace
