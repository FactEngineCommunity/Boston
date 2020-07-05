Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Meta
Imports Boston.Parser.Meta.Database
Imports Boston.Parser.Output

Namespace Parser.Syntax

    Friend Class Exec_For
        Private ExecNode As SyntaxNode
        Private BasePath As String = Nothing
        Private PreviewMode As Boolean = False
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0

        Private WithEvents proc As Exec_Base = Nothing

        Friend UserReturnPath As String = Nothing
        Friend UserReturnPathSet As Boolean = False

        Public OutputList As OutputCollection = Nothing

        Public Event Notify(ByVal Message As String)
        Public Event OutputWritten(ByVal Path As String)
        Public Event ConsoleOut(ByVal Message As String)
        Public Event ConsoleClear()

        Public Sub New(ByVal ExecNode As SyntaxNode, ByVal BasePath As String, ByVal PreviewMode As Boolean, _
                       ByVal TemplateContext As String, ByVal ScopeDepth As Integer)
            Me.ExecNode = ExecNode
            Me.BasePath = BasePath
            Me.PreviewMode = PreviewMode
            Me.TemplateContext = TemplateContext
            Me.ScopeDepth = ScopeDepth
            Me.OutputList = New OutputCollection(BasePath)
        End Sub

        Public Function Process() As System.Text.StringBuilder
            Select Case Me.ExecNode.ForEntity
                Case SyntaxNode.ExecForEntities.OBJECT_TABLE, SyntaxNode.ExecForEntities.OBJECT_VIEW
                    'Table/view loop
                    Return Me.Process_TableOrView()

                Case SyntaxNode.ExecForEntities.OBJECT_RELATIONS, SyntaxNode.ExecForEntities.OBJECT_RELATIONS 'Boston specific. Not part of original Metadrone.
                    'Relation loop
                    Return Me.Process_Relation()

                Case SyntaxNode.ExecForEntities.OBJECT_RELATION, SyntaxNode.ExecForEntities.OBJECT_RELATION 'Boston specific. Not part of original Metadrone.
                    'Relation loop
                    Return Me.Process_Relation()

                Case SyntaxNode.ExecForEntities.OBJECT_COLUMN, SyntaxNode.ExecForEntities.OBJECT_IDCOLUMN, _
                     SyntaxNode.ExecForEntities.OBJECT_PKCOLUMN, SyntaxNode.ExecForEntities.OBJECT_FKCOLUMN
                    'Column loop
                    Return Me.Process_Column()

                Case SyntaxNode.ExecForEntities.OBJECT_PROCEDURE, SyntaxNode.ExecForEntities.OBJECT_FUNCTION
                    'Routine loop
                    Return Me.Process_Routine()

                Case SyntaxNode.ExecForEntities.OBJECT_PARAMETER, SyntaxNode.ExecForEntities.OBJECT_INPARAMETER, _
                     SyntaxNode.ExecForEntities.OBJECT_OUTPARAMETER, SyntaxNode.ExecForEntities.OBJECT_INOUTPARAMETER
                    'Parameter loop
                    Return Me.Process_Parameter()

                Case SyntaxNode.ExecForEntities.OBJECT_FILE
                    'File loop
                    Return Me.Process_File()

                Case Else
                    Throw New Exception("Entity not set. Try recompiling.")

            End Select
        End Function


        Private Function Process_TableOrView() As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Get connection
            Dim conn As Object = EvalExpression(Me.ExecNode.Tokens, 4, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)
            Dim sourceName As String = ""
            If conn IsNot Nothing AndAlso TypeOf conn Is Source.Source Then
                'Add to connections
                sourceName = CType(conn, Source.Source).Name
                PackageBuilder.Connections.Add(CType(conn, Source.Source), Connections.LoadModes.Schema)

            Else
                sb = New System.Text.StringBuilder()
                For i As Integer = 4 To Me.ExecNode.Tokens.Count - 1
                    sb.Append(Me.ExecNode.Tokens(i))
                Next
                Throw New Exception("Could not evaluate connection source: " & sb.ToString)

            End If

            'Get variable name
            Dim varName As String = Me.ExecNode.Tokens(2).Text

            'Add variable one scope level down
            PackageBuilder.Variables.Add(Me.TemplateContext, varName, Me.ScopeDepth + 1, New Variable(Nothing, Variable.Types.Variable))

            'Set up entities first
            Call PackageBuilder.Connections.Item(sourceName).InitEntities()

            'Loop through connection entities
            Dim ExitIndex As Integer = -1
            Dim CurrentLoopIdx As Integer = 0
            Dim CurrentBlockIdx As Integer = 0
            Do
                'Repeat if had to exit

                'Prevent infinite loop
                If CurrentLoopIdx >= PackageBuilder.Connections.Item(sourceName).GetEntities(Me.ExecNode.ForEntity).Count Then ExitIndex = -1

                'Perform loop on block
                For i As Integer = CurrentLoopIdx To PackageBuilder.Connections.Item(sourceName).GetEntities(Me.ExecNode.ForEntity).Count - 1
                    'Abort on cancel
                    If PackageBuilder.StopRequest Then Return sb

                    'Check that any filtering has not made this index go past the array range
                    If i > PackageBuilder.Connections.Item(sourceName).GetEntities(Me.ExecNode.ForEntity).Count - 1 Then Exit For

                    'Set variable's value
                    With PackageBuilder.Variables.Item(Me.TemplateContext, varName, Me.ScopeDepth + 1)
                        .Value = PackageBuilder.Connections.Item(sourceName).GetEntities(Me.ExecNode.ForEntity).Item(i)
                    End With

                    'Process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 2)
                    sb.Append(Me.proc.ProcessBlock(CurrentBlockIdx, Me.ExecNode.Nodes.Count - 1, ExitIndex).ToString)

                    'Clear variables inside this block
                    PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 2)

                    'Don't continue on exit
                    If Me.proc.UserExitLoop Then
                        ExitIndex = -1
                        Exit For
                    End If

                    'Pass on output
                    For Each o As OutputItem In Me.proc.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'If return encountered
                    If Me.proc.UserReturnPathSet Then
                        'Indicate as so
                        Me.UserReturnPath = Me.proc.UserReturnPath
                        Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                        'Exit processing
                        ExitIndex = -1
                        Exit For
                    End If

                    If ExitIndex = -1 Then
                        'Table processed
                        'RaiseEvent Notify(".")
                    Else
                        'Redo because of exit
                        CurrentLoopIdx = i
                        CurrentBlockIdx = ExitIndex + 1
                        Exit For
                    End If
                Next
            Loop Until ExitIndex = -1

            'Clear the entity variable declared for the loop iterator
            PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

            Return sb
        End Function

        ''' <summary>
        ''' Boston specific. Not part of original Metadrone. Used to step through RDS.Relations for a Table/Column
        ''' </summary>
        ''' <returns></returns>
        Private Function Process_Relation() As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Get parent entity (table variable)
            Dim vals As List(Of Object) = Exec_Expr.EvalExpressionIntoParameters(Me.ExecNode.Tokens, 4, 4, Me.TemplateContext, Me.ScopeDepth, False)
            Dim relation As IEntity = Nothing
            If Not TypeOf vals(0) Is Variable Then
                Throw New Exception("Variable '" & Me.ExecNode.Tokens(4).Text & "' is not a valid entity variable.")
            End If
            If TypeOf CType(vals(0), Variable).Value Is IEntity Then
                relation = CType(CType(vals(0), Variable).Value, IEntity)
            Else
                Throw New Exception("Variable '" & Me.ExecNode.Tokens(4).Text & "' is not a valid entity variable.")
            End If

            'Get variable name
            Dim varName As String = Me.ExecNode.Tokens(2).Text

            'Add variable one scope level down
            PackageBuilder.Variables.Add(Me.TemplateContext, varName, Me.ScopeDepth + 1, New Variable(Nothing, Variable.Types.Variable))

            'Set up entities first
            Call relation.InitEntities()

            'Loop through child entities (parent table columns)
            Dim ExitIndex As Integer = -1
            Dim CurrentLoopIdx As Integer = 0
            Dim CurrentBlockIdx As Integer = 0

            Do
                'Repeat if had to exit

                'Prevent infinite loop
                If CurrentLoopIdx >= relation.GetEntities(Me.ExecNode.ForEntity).Count Then ExitIndex = -1

                'Perform loop on block
                For i As Integer = CurrentLoopIdx To relation.GetEntities(Me.ExecNode.ForEntity).Count - 1
                    'Abort on cancel
                    If PackageBuilder.StopRequest Then Return sb

                    'Check that any filtering has not made this index go past the array range
                    If i > relation.GetEntities(Me.ExecNode.ForEntity).Count - 1 Then Exit For

                    'Set variable's value
                    PackageBuilder.Variables.Item(Me.TemplateContext, varName, Me.ScopeDepth + 1).Value = relation.GetEntities(Me.ExecNode.ForEntity).Item(i)

                    'Process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 2)
                    sb.Append(Me.proc.ProcessBlock(CurrentBlockIdx, Me.ExecNode.Nodes.Count - 1, ExitIndex).ToString)

                    'Clear variables inside this block
                    PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 2)

                    'Don't continue on exit
                    If Me.proc.UserExitLoop Then
                        ExitIndex = -1
                        Exit For
                    End If

                    'Pass on output
                    For Each o As OutputItem In Me.proc.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'If return encountered
                    If Me.proc.UserReturnPathSet Then
                        'Indicate as so
                        Me.UserReturnPath = Me.proc.UserReturnPath
                        Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                        'Exit processing
                        ExitIndex = -1
                        Exit For
                    End If

                    If ExitIndex = -1 Then
                        'Column processed
                        'RaiseEvent Notify(".")
                    Else
                        'Redo because of exit
                        CurrentLoopIdx = i
                        CurrentBlockIdx = ExitIndex + 1
                        Exit For
                    End If
                Next
            Loop Until ExitIndex = -1

            'Clear the entity variable declared for the loop iterator
            PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

            Return sb
        End Function

        Private Function Process_Column() As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Get parent entity (table variable)
            Dim vals As List(Of Object) = Exec_Expr.EvalExpressionIntoParameters(Me.ExecNode.Tokens, 4, 4, Me.TemplateContext, Me.ScopeDepth, False)
            Dim entity As IEntity = Nothing
            If Not TypeOf vals(0) Is Variable Then
                Throw New Exception("Variable '" & Me.ExecNode.Tokens(4).Text & "' is not a valid entity variable.")
            End If
            If TypeOf CType(vals(0), Variable).Value Is IEntity Then
                entity = CType(CType(vals(0), Variable).Value, IEntity)
            Else
                Throw New Exception("Variable '" & Me.ExecNode.Tokens(4).Text & "' is not a valid entity variable.")
            End If

            'Get variable name
            Dim varName As String = Me.ExecNode.Tokens(2).Text

            'Add variable one scope level down
            PackageBuilder.Variables.Add(Me.TemplateContext, varName, Me.ScopeDepth + 1, New Variable(Nothing, Variable.Types.Variable))

            'Set up entities first
            Call entity.InitEntities()

            'Loop through child entities (parent table columns)
            Dim ExitIndex As Integer = -1
            Dim CurrentLoopIdx As Integer = 0
            Dim CurrentBlockIdx As Integer = 0
            Do
                'Repeat if had to exit

                'Prevent infinite loop
                If CurrentLoopIdx >= entity.GetEntities(Me.ExecNode.ForEntity).Count Then ExitIndex = -1

                'Perform loop on block
                For i As Integer = CurrentLoopIdx To entity.GetEntities(Me.ExecNode.ForEntity).Count - 1
                    'Abort on cancel
                    If PackageBuilder.StopRequest Then Return sb

                    'Check that any filtering has not made this index go past the array range
                    If i > entity.GetEntities(Me.ExecNode.ForEntity).Count - 1 Then Exit For

                    'Set variable's value
                    PackageBuilder.Variables.Item(Me.TemplateContext, varName, Me.ScopeDepth + 1).Value = entity.GetEntities(Me.ExecNode.ForEntity).Item(i)

                    'Process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 2)
                    sb.Append(Me.proc.ProcessBlock(CurrentBlockIdx, Me.ExecNode.Nodes.Count - 1, ExitIndex).ToString)

                    'Clear variables inside this block
                    PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 2)

                    'Don't continue on exit
                    If Me.proc.UserExitLoop Then
                        ExitIndex = -1
                        Exit For
                    End If

                    'Pass on output
                    For Each o As OutputItem In Me.proc.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'If return encountered
                    If Me.proc.UserReturnPathSet Then
                        'Indicate as so
                        Me.UserReturnPath = Me.proc.UserReturnPath
                        Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                        'Exit processing
                        ExitIndex = -1
                        Exit For
                    End If

                    If ExitIndex = -1 Then
                        'Column processed
                        'RaiseEvent Notify(".")
                    Else
                        'Redo because of exit
                        CurrentLoopIdx = i
                        CurrentBlockIdx = ExitIndex + 1
                        Exit For
                    End If
                Next
            Loop Until ExitIndex = -1

            'Clear the entity variable declared for the loop iterator
            PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

            Return sb
        End Function


        Private Function Process_Routine() As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Get connection
            Dim conn As Object = EvalExpression(Me.ExecNode.Tokens, 4, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)
            Dim sourceName As String = ""
            If conn IsNot Nothing AndAlso TypeOf conn Is Source.Source Then
                'Add to connections
                sourceName = CType(conn, Source.Source).Name
                PackageBuilder.Connections.Add(CType(conn, Source.Source), Connections.LoadModes.Routines)

            Else
                sb = New System.Text.StringBuilder()
                For i As Integer = 4 To Me.ExecNode.Tokens.Count - 1
                    sb.Append(Me.ExecNode.Tokens(i))
                Next
                Throw New Exception("Could not evaluate connection source: " & sb.ToString)

            End If

            'Get variable name
            Dim varName As String = Me.ExecNode.Tokens(2).Text

            'Add variable one scope level down
            PackageBuilder.Variables.Add(Me.TemplateContext, varName, Me.ScopeDepth + 1, New Variable(Nothing, Variable.Types.Variable))

            'Set up entities first
            Call PackageBuilder.Connections.Item(sourceName).InitEntities()

            'Loop through connection entities
            Dim ExitIndex As Integer = -1
            Dim CurrentLoopIdx As Integer = 0
            Dim CurrentBlockIdx As Integer = 0
            Do
                'Repeat if had to exit

                'Prevent infinite loop
                If CurrentLoopIdx >= PackageBuilder.Connections.Item(sourceName).GetEntities(Me.ExecNode.ForEntity).Count Then ExitIndex = -1

                'Perform loop on block
                For i As Integer = CurrentLoopIdx To PackageBuilder.Connections.Item(sourceName).GetEntities(Me.ExecNode.ForEntity).Count - 1
                    'Abort on cancel
                    If PackageBuilder.StopRequest Then Return sb

                    'Check that any filtering has not made this index go past the array range
                    If i > PackageBuilder.Connections.Item(sourceName).GetEntities(Me.ExecNode.ForEntity).Count - 1 Then Exit For

                    'Set variable's value
                    With PackageBuilder.Variables.Item(Me.TemplateContext, varName, Me.ScopeDepth + 1)
                        .Value = PackageBuilder.Connections.Item(sourceName).GetEntities(Me.ExecNode.ForEntity).Item(i)
                    End With

                    'Process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 2)
                    sb.Append(Me.proc.ProcessBlock(CurrentBlockIdx, Me.ExecNode.Nodes.Count - 1, ExitIndex).ToString)

                    'Clear variables inside this block
                    PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 2)

                    'Don't continue on exit
                    If Me.proc.UserExitLoop Then
                        ExitIndex = -1
                        Exit For
                    End If

                    'Pass on output
                    For Each o As OutputItem In Me.proc.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'If return encountered
                    If Me.proc.UserReturnPathSet Then
                        'Indicate as so
                        Me.UserReturnPath = Me.proc.UserReturnPath
                        Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                        'Exit processing
                        ExitIndex = -1
                        Exit For
                    End If

                    If ExitIndex = -1 Then
                        'Routine processed
                        'RaiseEvent Notify(".")
                    Else
                        'Redo because of exit
                        CurrentLoopIdx = i
                        CurrentBlockIdx = ExitIndex + 1
                        Exit For
                    End If
                Next
            Loop Until ExitIndex = -1

            'Clear the entity variable declared for the loop iterator
            PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

            Return sb
        End Function


        Private Function Process_Parameter() As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Get parent entity (routine variable)
            Dim vals As List(Of Object) = Exec_Expr.EvalExpressionIntoParameters(Me.ExecNode.Tokens, 4, 4, Me.TemplateContext, Me.ScopeDepth, False)
            Dim entity As IEntity = Nothing
            If Not TypeOf vals(0) Is Variable Then
                Throw New Exception("Variable '" & Me.ExecNode.Tokens(4).Text & "' is not a valid entity variable.")
            End If
            If TypeOf CType(vals(0), Variable).Value Is IEntity Then
                entity = CType(CType(vals(0), Variable).Value, IEntity)
            Else
                Throw New Exception("Variable '" & Me.ExecNode.Tokens(4).Text & "' is not a valid entity variable.")
            End If

            'Get variable name
            Dim varName As String = Me.ExecNode.Tokens(2).Text

            'Add variable one scope level down
            PackageBuilder.Variables.Add(Me.TemplateContext, varName, Me.ScopeDepth + 1, New Variable(Nothing, Variable.Types.Variable))

            'Set up entities first
            Call entity.InitEntities()

            'Loop through child entities (parent routine params)
            Dim ExitIndex As Integer = -1
            Dim CurrentLoopIdx As Integer = 0
            Dim CurrentBlockIdx As Integer = 0
            Do
                'Repeat if had to exit

                'Prevent infinite loop
                If CurrentLoopIdx >= entity.GetEntities(Me.ExecNode.ForEntity).Count Then ExitIndex = -1

                'Perform loop on block
                For i As Integer = CurrentLoopIdx To entity.GetEntities(Me.ExecNode.ForEntity).Count - 1
                    'Abort on cancel
                    If PackageBuilder.StopRequest Then Return sb

                    'Check that any filtering has not made this index go past the array range
                    If i > entity.GetEntities(Me.ExecNode.ForEntity).Count - 1 Then Exit For

                    'Set variable's value
                    With PackageBuilder.Variables.Item(Me.TemplateContext, varName, Me.ScopeDepth + 1)
                        .Value = entity.GetEntities(Me.ExecNode.ForEntity).Item(i)
                    End With

                    'Process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 2)
                    sb.Append(Me.proc.ProcessBlock(CurrentBlockIdx, Me.ExecNode.Nodes.Count - 1, ExitIndex).ToString)

                    'Clear variables inside this block
                    PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 2)

                    'Don't continue on exit
                    If Me.proc.UserExitLoop Then
                        ExitIndex = -1
                        Exit For
                    End If

                    'Pass on output
                    For Each o As OutputItem In Me.proc.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'If return encountered
                    If Me.proc.UserReturnPathSet Then
                        'Indicate as so
                        Me.UserReturnPath = Me.proc.UserReturnPath
                        Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                        'Exit processing
                        ExitIndex = -1
                        Exit For
                    End If

                    If ExitIndex = -1 Then
                        'Parameter processed
                        'RaiseEvent Notify(".")
                    Else
                        'Redo because of exit
                        CurrentLoopIdx = i
                        CurrentBlockIdx = ExitIndex + 1
                        Exit For
                    End If
                Next
            Loop Until ExitIndex = -1

            'Clear the entity variable declared for the loop iterator
            PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

            Return sb
        End Function


        Private Function Process_File() As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Get directory
            Dim path As Object = EvalExpression(Me.ExecNode.Tokens, 4, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)
            Dim dirinfo As IO.DirectoryInfo = Nothing
            If TypeOf path Is String Then
                path = IO.Path.Combine(Me.BasePath, path.ToString)
                dirinfo = New IO.DirectoryInfo(path.ToString)

            Else
                sb = New System.Text.StringBuilder()
                For i As Integer = 4 To Me.ExecNode.Tokens.Count - 1
                    sb.Append(Me.ExecNode.Tokens(i))
                Next
                Throw New Exception("Could not evaluate directory: " & sb.ToString)

            End If

            'Get variable name
            Dim varName As String = Me.ExecNode.Tokens(2).Text

            'Add variable one scope level down
            PackageBuilder.Variables.Add(Me.TemplateContext, varName, Me.ScopeDepth + 1, New Variable(Nothing, Variable.Types.Primitive))

            'Loop through files in directory
            Dim ExitIndex As Integer = -1
            Dim CurrentLoopIdx As Integer = 0
            Dim CurrentBlockIdx As Integer = 0
            Do
                'Repeat if had to exit

                Dim files() As IO.FileInfo = dirinfo.GetFiles()

                'Perform loop on block
                If files Is Nothing Then Throw New Exception("Failed to get files from directory.")
                For i As Integer = CurrentLoopIdx To files.Count - 1
                    'Abort on cancel
                    If PackageBuilder.StopRequest Then Return sb

                    'Set variable's value
                    PackageBuilder.Variables.Item(Me.TemplateContext, varName, Me.ScopeDepth).Value = files(i).Name

                    'Process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 2)
                    sb.Append(Me.proc.ProcessBlock(CurrentBlockIdx, Me.ExecNode.Nodes.Count - 1, ExitIndex).ToString)

                    'Clear variables inside this block
                    PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 2)

                    'Don't continue on exit
                    If Me.proc.UserExitLoop Then
                        ExitIndex = -1
                        Exit For
                    End If

                    'Pass on output
                    For Each o As OutputItem In Me.proc.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'If return encountered
                    If Me.proc.UserReturnPathSet Then
                        'Indicate as so
                        Me.UserReturnPath = Me.proc.UserReturnPath
                        Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                        'Exit processing
                        ExitIndex = -1
                        Exit For
                    End If

                    If ExitIndex = -1 Then
                        'File processed
                        'RaiseEvent Notify(".")
                    Else
                        'Redo because of exit
                        CurrentLoopIdx = i
                        CurrentBlockIdx = ExitIndex + 1
                        Exit For
                    End If
                Next
            Loop Until ExitIndex = -1

            'Clear the entity variable declared for the loop iterator
            PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

            Return sb
        End Function

        Private Sub proc_Notify(ByVal Message As String) Handles proc.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub proc_OutputWritten(ByVal Path As String) Handles proc.OutputWritten
            RaiseEvent OutputWritten(Path)
        End Sub

        Private Sub proc_ConsoleOut(ByVal Message As String) Handles proc.ConsoleOut
            RaiseEvent ConsoleOut(Message)
        End Sub

        Private Sub proc_ConsoleClear() Handles proc.ConsoleClear
            RaiseEvent ConsoleClear()
        End Sub

    End Class

End Namespace