Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings

Namespace UI

    Friend Class SyntaxPopupList

        Public Enum Contexts
            DontShow = 0
            StartInMain = 1
            StartInTemplate = 2
            [For] = 3
            Expression = 4
            [Call] = 5
            [Set] = 6
            InSources = 7
            InTable_View_Routine = 8
            InRoutineOnly = 9
            Sources = 10
            Member = 11
        End Enum

        Private _items As New List(Of SyntaxPopupListItem)

        Private Sub AddListItem(ByVal Item As String, ByVal Description As String, ByVal DefaultCompletion As String, _
                                ByVal Icon As SyntaxPopupListItem.Icons)
            Me._items.Add(New SyntaxPopupListItem(Item, Description, DefaultCompletion, Icon))
        End Sub

        Public ReadOnly Property Items() As List(Of SyntaxPopupListItem)
            Get
                Return Me._items
            End Get
        End Property

        Public Sub Fill(ByVal lineWords As Boston.UI.txtBox.LineWords, ByVal Context As Contexts, ByVal analysis As Parser.CodeCompletion.Analyser)
            Me.Items.Clear()

            Select Case Context
                Case Contexts.StartInMain, Contexts.StartInTemplate
                    'Starter keywords
                    Me.AddListItem(ACTION_CALL, DOCO_ACTION_CALL, Nothing, SyntaxPopupListItem.Icons.SYSTEM)
                    Me.AddListItem(ACTION_FOR, DOCO_ACTION_FOR, Nothing, SyntaxPopupListItem.Icons.SYSTEM)
                    Me.AddListItem(ACTION_IF, DOCO_ACTION_IF, Nothing, SyntaxPopupListItem.Icons.SYSTEM)
                    Me.AddListItem(ACTION_ELSE, DOCO_ACTION_ELSE, Nothing, SyntaxPopupListItem.Icons.SYSTEM)
                    Me.AddListItem(ACTION_ELSEIF, DOCO_ACTION_ELSEIF, Nothing, SyntaxPopupListItem.Icons.SYSTEM)
                    Me.AddListItem(ACTION_END, DOCO_ACTION_END, Nothing, SyntaxPopupListItem.Icons.SYSTEM)
                    Me.AddListItem(ACTION_SET, DOCO_ACTION_SET, Nothing, SyntaxPopupListItem.Icons.SYSTEM)
                    Me.AddListItem(ACTION_EXIT & " " & ACTION_WHEN, DOCO_ACTION_EXIT, ACTION_EXIT & " " & ACTION_WHEN, SyntaxPopupListItem.Icons.SYSTEM)
                    'Me.AddListItem(RESERVED_COMMENT_LINE, DOCO_RESERVED_COMMENT_LINE, Nothing, SyntaxPopupListItem.Icons.SYSTEM)
                    Me.AddListItem(SYS_RETURN, DOCO_SYS_RETURN, SYS_RETURN & "(""path"")", SyntaxPopupListItem.Icons.METHOD)

                    'These supported only in templates
                    If Context = Contexts.StartInTemplate Then
                        Me.AddListItem(SYS_OUT, DOCO_SYS_OUT, SYS_OUT & "(""text value"")", SyntaxPopupListItem.Icons.METHOD)
                        Me.AddListItem(SYS_OUTLN, DOCO_SYS_OUTLN, SYS_OUTLN & "(""text value"")", SyntaxPopupListItem.Icons.METHOD)
                    End If

                    'Functions
                    Me.AddListItem(SYS_COUT, DOCO_SYS_COUT, SYS_COUT & "(""text value"")", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_COUTLN, DOCO_SYS_COUTLN, SYS_COUTLN & "(""text value"")", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_CLCON, DOCO_SYS_CLCON, SYS_CLCON & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MAKEDIR, DOCO_SYS_MAKEDIR, SYS_MAKEDIR & "(""create\directory"")", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_FILECOPY, DOCO_SYS_FILECOPY, SYS_FILECOPY & "(""source file"", ""dest file"")", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_COMMAND, DOCO_SYS_COMMAND, SYS_COMMAND & "(""file name"", ""arguments"")", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_RUNCS, DOCO_SYS_RUNCS, SYS_RUNCS & "(""namespace.class.method()"")", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_RUNVB, DOCO_SYS_RUNVB, SYS_RUNVB & "(""Namespace.Class.Method()"")", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_RUNSCRIPT, DOCO_SYS_RUNSCRIPT, SYS_RUNSCRIPT & "(source, ""file name"")", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_CNUM, DOCO_SYS_CNUM, SYS_CNUM & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MATHS_SIN, DOCO_SYS_MATHS_SIN, SYS_MATHS_SIN & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MATHS_COS, DOCO_SYS_MATHS_COS, SYS_MATHS_COS & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MATHS_TAN, DOCO_SYS_MATHS_TAN, SYS_MATHS_TAN & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MATHS_SQRT, DOCO_SYS_MATHS_SQRT, SYS_MATHS_SQRT & "()", SyntaxPopupListItem.Icons.METHOD)

                    'All non-source vars
                    If analysis IsNot Nothing Then
                        For Each var As Parser.CodeCompletion.Variable In analysis.MainVars
                            If var.Type = Parser.CodeCompletion.Variable.Types.Source Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in MAIN", Nothing, SyntaxPopupListItem.Icons.VAR_MAIN)
                        Next
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type = Parser.CodeCompletion.Variable.Types.Source Then Continue For
                            If var.Type = Parser.CodeCompletion.Variable.Types.TemplateParameter Then
                                Me.AddListItem(var.Name, "Template parameter", Nothing, SyntaxPopupListItem.Icons.TEMPLATE_PARAM)
                            Else
                                Me.AddListItem(var.Name, var.Type.ToString & " variable in template", Nothing, SyntaxPopupListItem.Icons.VAR_TEMPLATE)
                            End If
                        Next
                    End If

                Case Contexts.For
                    Me.AddListItem(OBJECT_TABLE, DOCO_OBJECT_TABLE, Nothing, SyntaxPopupListItem.Icons.OBJECT_TABLE)
                    Me.AddListItem(OBJECT_VIEW, DOCO_OBJECT_VIEW, Nothing, SyntaxPopupListItem.Icons.OBJECT_VIEW)
                    Me.AddListItem(OBJECT_PROCEDURE, DOCO_OBJECT_PROCEDURE, Nothing, SyntaxPopupListItem.Icons.OBJECT_PROC)
                    Me.AddListItem(OBJECT_FUNCTION, DOCO_OBJECT_FUNCTION, Nothing, SyntaxPopupListItem.Icons.OBJECT_FUNC)
                    Me.AddListItem(OBJECT_FILE, DOCO_OBJECT_FILE, Nothing, SyntaxPopupListItem.Icons.OBJECT_FILE)
                    Me.AddListItem(OBJECT_INDEX, DOCO_OBJECT_INDEX, Nothing, SyntaxPopupListItem.Icons.OBJECT_COLUMN) 'ToDo: Copy OBJECT_COLUMN icon. Boston Specific. Not part of original Meadrone. Used for RDS.Relations
                    Me.AddListItem(OBJECT_RELATION, DOCO_OBJECT_RELATION, Nothing, SyntaxPopupListItem.Icons.OBJECT_COLUMN) 'ToDo: Copy OBJECT_COLUMN icon. Boston Specific. Not part of original Meadrone. Used for RDS.Relations
                    Me.AddListItem(OBJECT_COLUMN, DOCO_OBJECT_COLUMN, Nothing, SyntaxPopupListItem.Icons.OBJECT_COLUMN)
                    Me.AddListItem(OBJECT_IDCOLUMN, DOCO_OBJECT_IDCOLUMN, Nothing, SyntaxPopupListItem.Icons.OBJECT_COLUMN)
                    Me.AddListItem(OBJECT_PKCOLUMN, DOCO_OBJECT_PKCOLUMN, Nothing, SyntaxPopupListItem.Icons.OBJECT_COLUMN)
                    Me.AddListItem(OBJECT_FKCOLUMN, DOCO_OBJECT_FKCOLUMN, Nothing, SyntaxPopupListItem.Icons.OBJECT_COLUMN)
                    Me.AddListItem(OBJECT_PARAMETER, DOCO_OBJECT_PARAMETER, Nothing, SyntaxPopupListItem.Icons.OBJECT_PARAM)
                    Me.AddListItem(OBJECT_INPARAMETER, DOCO_OBJECT_INPARAMETER, Nothing, SyntaxPopupListItem.Icons.OBJECT_INPARAM)
                    Me.AddListItem(OBJECT_OUTPARAMETER, DOCO_OBJECT_OUTPARAMETER, Nothing, SyntaxPopupListItem.Icons.OBJECT_OUTPARAM)
                    Me.AddListItem(OBJECT_INOUTPARAMETER, DOCO_OBJECT_INOUTPARAMETER, Nothing, SyntaxPopupListItem.Icons.OBJECT_INOUTPARAM)

                Case Contexts.Expression
                    'All non-source vars
                    If analysis IsNot Nothing Then
                        For Each var As Parser.CodeCompletion.Variable In analysis.MainVars
                            If var.Type = Parser.CodeCompletion.Variable.Types.Source Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in MAIN", Nothing, SyntaxPopupListItem.Icons.VAR_MAIN)
                        Next
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type = Parser.CodeCompletion.Variable.Types.Source Then Continue For
                            If var.Type = Parser.CodeCompletion.Variable.Types.TemplateParameter Then
                                Me.AddListItem(var.Name, "Template parameter", Nothing, SyntaxPopupListItem.Icons.TEMPLATE_PARAM)
                            Else
                                Me.AddListItem(var.Name, var.Type.ToString & " variable in template", Nothing, SyntaxPopupListItem.Icons.VAR_TEMPLATE)
                            End If
                        Next
                    End If

                    'Functions with returns
                    Me.AddListItem(SYS_CNUM, DOCO_SYS_CNUM, SYS_CNUM & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MATHS_SIN, DOCO_SYS_MATHS_SIN, SYS_MATHS_SIN & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MATHS_COS, DOCO_SYS_MATHS_COS, SYS_MATHS_COS & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MATHS_TAN, DOCO_SYS_MATHS_TAN, SYS_MATHS_TAN & "()", SyntaxPopupListItem.Icons.METHOD)
                    Me.AddListItem(SYS_MATHS_SQRT, DOCO_SYS_MATHS_SQRT, SYS_MATHS_SQRT & "()", SyntaxPopupListItem.Icons.METHOD)

                Case Contexts.Call
                    'Templates
                    For Each tmp In analysis.Templates
                        Dim sbComplete As New System.Text.StringBuilder(tmp.Name & "(")
                        For i As Integer = 0 To tmp.Params.Count - 1
                            sbComplete.Append(tmp.Params(i))
                            If i < tmp.Params.Count - 1 Then sbComplete.Append(", ")
                        Next
                        sbComplete.Append(")")

                        Me.AddListItem(tmp.Name, sbComplete.ToString, sbComplete.ToString, SyntaxPopupListItem.Icons.TEMPLATE)
                    Next

                Case Contexts.Set
                    'All non-source vars
                    If analysis IsNot Nothing Then
                        For Each var As Parser.CodeCompletion.Variable In analysis.MainVars
                            If var.Type = Parser.CodeCompletion.Variable.Types.Source Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in MAIN", Nothing, SyntaxPopupListItem.Icons.VAR_MAIN)
                        Next
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type = Parser.CodeCompletion.Variable.Types.Source Then Continue For
                            If var.Type = Parser.CodeCompletion.Variable.Types.TemplateParameter Then
                                Me.AddListItem(var.Name, "Template parameter", Nothing, SyntaxPopupListItem.Icons.TEMPLATE_PARAM)
                            Else
                                Me.AddListItem(var.Name, var.Type.ToString & " variable in template", Nothing, SyntaxPopupListItem.Icons.VAR_TEMPLATE)
                            End If
                        Next
                    End If

                Case Contexts.InSources
                    'this for table/routine/view in source
                    'All sources as source.source_name
                    For Each src In analysis.Sources
                        Dim sb As New System.Text.StringBuilder("Database connection" & vbCrLf & "Provider: " & src.Provider)
                        Me.AddListItem(GLOBALS_SOURCES & "." & src.Name, sb.ToString, Nothing, SyntaxPopupListItem.Icons.SOURCE)
                    Next

                    'All source vars
                    If analysis IsNot Nothing Then
                        For Each var As Parser.CodeCompletion.Variable In analysis.MainVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.Source Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in MAIN", Nothing, SyntaxPopupListItem.Icons.VAR_MAIN)
                        Next
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.Source Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in this template", Nothing, SyntaxPopupListItem.Icons.VAR_TEMPLATE)
                        Next
                        'Template params too
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.TemplateParameter Then Continue For
                            Me.AddListItem(var.Name, "Template parameter", Nothing, SyntaxPopupListItem.Icons.TEMPLATE_PARAM)
                        Next
                    End If

                Case Contexts.InTable_View_Routine
                    'this for column in table
                    If analysis IsNot Nothing Then
                        'All table vars
                        For Each var As Parser.CodeCompletion.Variable In analysis.MainVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.Table Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in MAIN", Nothing, SyntaxPopupListItem.Icons.VAR_MAIN)
                        Next
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.Table Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in this template", Nothing, SyntaxPopupListItem.Icons.VAR_TEMPLATE)
                        Next

                        'All view vars
                        For Each var As Parser.CodeCompletion.Variable In analysis.MainVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.View Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in MAIN", Nothing, SyntaxPopupListItem.Icons.VAR_MAIN)
                        Next
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.View Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in this template", Nothing, SyntaxPopupListItem.Icons.VAR_TEMPLATE)
                        Next

                        'All routine vars
                        For Each var As Parser.CodeCompletion.Variable In analysis.MainVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.Procedure Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in MAIN", Nothing, SyntaxPopupListItem.Icons.VAR_MAIN)
                        Next
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.Procedure Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in this template", Nothing, SyntaxPopupListItem.Icons.VAR_TEMPLATE)
                        Next

                        'Template params too
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.TemplateParameter Then Continue For
                            Me.AddListItem(var.Name, "Template parameter", Nothing, SyntaxPopupListItem.Icons.TEMPLATE_PARAM)
                        Next
                    End If

                Case Contexts.InRoutineOnly
                    'this for param in routine
                    'All routine vars
                    If analysis IsNot Nothing Then
                        For Each var As Parser.CodeCompletion.Variable In analysis.MainVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.Procedure Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in MAIN", Nothing, SyntaxPopupListItem.Icons.VAR_MAIN)
                        Next
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.Procedure Then Continue For
                            Me.AddListItem(var.Name, var.Type.ToString & " variable in this template", Nothing, SyntaxPopupListItem.Icons.VAR_TEMPLATE)
                        Next

                        'Template params too
                        For Each var As Parser.CodeCompletion.Variable In analysis.TemplateVars
                            If var.Type <> Parser.CodeCompletion.Variable.Types.TemplateParameter Then Continue For
                            Me.AddListItem(var.Name, "Template parameter", Nothing, SyntaxPopupListItem.Icons.TEMPLATE_PARAM)
                        Next
                    End If

                Case Contexts.Sources
                    'All sources
                    For Each src In analysis.Sources
                        Dim sb As New System.Text.StringBuilder("Database connection" & vbCrLf & "Provider: " & src.Provider)
                        Me.AddListItem(src.Name, sb.ToString, Nothing, SyntaxPopupListItem.Icons.SOURCE)
                    Next

                Case Contexts.Member
                    If analysis Is Nothing Then Exit Sub

                    'Get variable reference - remember name is up to the dot
                    Dim varName As String = lineWords.lastWord.Substring(0, lineWords.lastWord.IndexOf("."))
                    Dim var = analysis.TemplateVars.Item(varName)

                    'Try in main
                    If var Is Nothing Then var = analysis.MainVars.Item(varName)

                    'No members for variable that can't be found
                    If var Is Nothing Then Exit Sub

                    'None for these
                    Select Case var.Type
                        Case Parser.CodeCompletion.Variable.Types.File,
                             Parser.CodeCompletion.Variable.Types.Primitive
                            Exit Sub
                    End Select

                    Select Case var.Type
                        Case Parser.CodeCompletion.Variable.Types.TemplateParameter,
                             Parser.CodeCompletion.Variable.Types.Table,
                             Parser.CodeCompletion.Variable.Types.View,
                             Parser.CodeCompletion.Variable.Types.Procedure,
                             Parser.CodeCompletion.Variable.Types.Function
                            Me.AddListItem(VARIABLE_ATTRIBUTE_VALUE, DOCO_VARIABLE_ATTRIBUTE_VALUE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_LISTCOUNT, DOCO_VARIABLE_ATTRIBUTE_LISTCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_LISTPOS, DOCO_VARIABLE_ATTRIBUTE_LISTPOS, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                            Me.AddListItem(VARIABLE_ATTRIBUTE_COLUMNCOUNT, DOCO_VARIABLE_ATTRIBUTE_COLUMNCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_PKCOLUMNCOUNT, DOCO_VARIABLE_ATTRIBUTE_PKCOLUMNCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_FKCOLUMNCOUNT, DOCO_VARIABLE_ATTRIBUTE_FKCOLUMNCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_IDCOLUMNCOUNT, DOCO_VARIABLE_ATTRIBUTE_IDCOLUMNCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                            Select Case var.Type
                                Case Parser.CodeCompletion.Variable.Types.Procedure, Parser.CodeCompletion.Variable.Types.Function
                                    Me.AddListItem(VARIABLE_ATTRIBUTE_PARAMCOUNT, DOCO_VARIABLE_ATTRIBUTE_PARAMCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                                    Me.AddListItem(VARIABLE_ATTRIBUTE_INPARAMCOUNT, DOCO_VARIABLE_ATTRIBUTE_INPARAMCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                                    Me.AddListItem(VARIABLE_ATTRIBUTE_OUTPARAMCOUNT, DOCO_VARIABLE_ATTRIBUTE_OUTPARAMCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                                    Me.AddListItem(VARIABLE_ATTRIBUTE_INOUTPARAMCOUNT, DOCO_VARIABLE_ATTRIBUTE_INOUTPARAMCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            End Select

                            'Add transforms from the source involved
                            For Each src In analysis.Sources
                                If StrEq(src.Name, var.SourceTag) Then
                                    src.Transforms = New Parser.Syntax.SourceTransforms(src.Name, src.Transformations)
                                    src.Transforms.Build()

                                    For Each n In src.Transforms.TransformInstructions.Nodes
                                        Select Case var.Type
                                            Case Parser.CodeCompletion.Variable.Types.Table, Parser.CodeCompletion.Variable.Types.View
                                                If StrEq(n.SetSubj, Parser.Syntax.SyntaxToken.TransformTargets.Table.ToString) And Not _
                                                   Parser.Syntax.Constants.IsSystemAttribute(n.SetSubjAttrib) Then
                                                    Me.AddListItem(n.SetSubjAttrib, "User defined transformation", Nothing, SyntaxPopupListItem.Icons.TRANSFORMATION)
                                                End If
                                            Case Parser.CodeCompletion.Variable.Types.Procedure, Parser.CodeCompletion.Variable.Types.Function
                                                If StrEq(n.SetSubj, Parser.Syntax.SyntaxToken.TransformTargets.Routine.ToString) And Not _
                                                   Parser.Syntax.Constants.IsSystemAttribute(n.SetSubjAttrib) Then
                                                    Me.AddListItem(n.SetSubjAttrib, "User defined transformation", Nothing, SyntaxPopupListItem.Icons.TRANSFORMATION)
                                                End If
                                        End Select
                                    Next

                                    Exit For
                                End If
                            Next

                            Me.AddListItem(VARIABLE_METHOD_INDEXOFCOL, DOCO_VARIABLE_METHOD_INDEXOFCOL, VARIABLE_METHOD_INDEXOFCOL & "(""colname"")", SyntaxPopupListItem.Icons.METHOD)

                        Case Parser.CodeCompletion.Variable.Types.Relation 'Boston specific. Not part of original Metadrone.
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ID, DOCO_VARIABLE_ATTRIBUTE_ID, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_COLUMNCOUNT, DOCO_VARIABLE_ATTRIBUTE_COLUMNCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_REFERENCINGTABLENAME, DOCO_VARIABLE_ATTRIBUTE_REFERENCINGTABLENAME, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_REFERENCINGCOLUMNNAME, DOCO_VARIABLE_ATTRIBUTE_REFERENCINGCOLUMNNAME, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_REFERENCEDTABLENAME, DOCO_VARIABLE_ATTRIBUTE_REFERENCEDTABLENAME, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_REFERENCEDCOLUMNNAME, DOCO_VARIABLE_ATTRIBUTE_REFERENCEDCOLUMNNAME, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                        Case Parser.CodeCompletion.Variable.Types.Index 'Boston specific. Not part of original Metadrone.
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ID, DOCO_VARIABLE_ATTRIBUTE_ID, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_COLUMNCOUNT, DOCO_VARIABLE_ATTRIBUTE_COLUMNCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                        Case Parser.CodeCompletion.Variable.Types.TemplateParameter,
                             Parser.CodeCompletion.Variable.Types.Column,
                             Parser.CodeCompletion.Variable.Types.PKColumn,
                             Parser.CodeCompletion.Variable.Types.FKColumn,
                             Parser.CodeCompletion.Variable.Types.IDColumn
                            Me.AddListItem(VARIABLE_ATTRIBUTE_VALUE, DOCO_VARIABLE_ATTRIBUTE_VALUE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_LISTCOUNT, DOCO_VARIABLE_ATTRIBUTE_LISTCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_LISTPOS, DOCO_VARIABLE_ATTRIBUTE_LISTPOS, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                            Me.AddListItem(VARIABLE_ATTRIBUTE_RELATION, DOCO_VARIABLE_ATTRIBUTE_RELATION, Nothing, SyntaxPopupListItem.Icons.PROPERTY) 'Boston specific. Not part of original Metadrone.
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH, DOCO_VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH, Nothing, SyntaxPopupListItem.Icons.PROPERTY) 'Boston specific. Not part of original Metadrone.
                            Me.AddListItem(VARIABLE_ATTRIBUTE_DATATYPE, DOCO_VARIABLE_ATTRIBUTE_DATATYPE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_LENGTH, DOCO_VARIABLE_ATTRIBUTE_LENGTH, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_PRECISION, DOCO_VARIABLE_ATTRIBUTE_PRECISION, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_SCALE, DOCO_VARIABLE_ATTRIBUTE_SCALE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                            Me.AddListItem(VARIABLE_ATTRIBUTE_ISIDENTITY, DOCO_VARIABLE_ATTRIBUTE_ISIDENTITY, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ISPRIMARYKEY, DOCO_VARIABLE_ATTRIBUTE_ISPRIMARYKEY, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ISFOREIGNKEY, DOCO_VARIABLE_ATTRIBUTE_ISFOREIGNKEY, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_NULLABLE, DOCO_VARIABLE_ATTRIBUTE_NULLABLE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                            'Add transforms from the source involved
                            If var.Type <> Parser.CodeCompletion.Variable.Types.TemplateParameter Then
                                For Each src In analysis.Sources
                                    If StrEq(src.Name, var.SourceTag) Then
                                        src.Transforms = New Parser.Syntax.SourceTransforms(src.Name, src.Transformations)
                                        src.Transforms.Build()

                                        For Each n In src.Transforms.TransformInstructions.Nodes
                                            If StrEq(n.SetSubj, Parser.Syntax.SyntaxToken.TransformTargets.Column.ToString) And Not _
                                               Parser.Syntax.Constants.IsSystemAttribute(n.SetSubjAttrib) Then
                                                Me.AddListItem(n.SetSubjAttrib, "User defined transformation", Nothing, SyntaxPopupListItem.Icons.TRANSFORMATION)
                                            End If
                                        Next

                                        Exit For
                                    End If
                                Next
                            End If

                        Case Parser.CodeCompletion.Variable.Types.TemplateParameter,
                             Parser.CodeCompletion.Variable.Types.Parameter,
                             Parser.CodeCompletion.Variable.Types.InParameter,
                             Parser.CodeCompletion.Variable.Types.OutParameter,
                             Parser.CodeCompletion.Variable.Types.InOutParameter
                            Me.AddListItem(VARIABLE_ATTRIBUTE_VALUE, DOCO_VARIABLE_ATTRIBUTE_VALUE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_LISTCOUNT, DOCO_VARIABLE_ATTRIBUTE_LISTCOUNT, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_LISTPOS, DOCO_VARIABLE_ATTRIBUTE_LISTPOS, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                            Me.AddListItem(VARIABLE_ATTRIBUTE_RELATION, DOCO_VARIABLE_ATTRIBUTE_RELATION, Nothing, SyntaxPopupListItem.Icons.PROPERTY) 'Boston specific. Not part of original Metadrone.
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH, DOCO_VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH, Nothing, SyntaxPopupListItem.Icons.PROPERTY) 'Boston specific. Not part of original Metadrone.
                            Me.AddListItem(VARIABLE_ATTRIBUTE_DATATYPE, DOCO_VARIABLE_ATTRIBUTE_DATATYPE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_LENGTH, DOCO_VARIABLE_ATTRIBUTE_LENGTH, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_PRECISION, DOCO_VARIABLE_ATTRIBUTE_PRECISION, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_SCALE, DOCO_VARIABLE_ATTRIBUTE_SCALE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                            Me.AddListItem(VARIABLE_ATTRIBUTE_MODE, DOCO_VARIABLE_ATTRIBUTE_MODE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ISINMODE, DOCO_VARIABLE_ATTRIBUTE_ISINMODE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ISOUTMODE, DOCO_VARIABLE_ATTRIBUTE_ISOUTMODE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)
                            Me.AddListItem(VARIABLE_ATTRIBUTE_ISINOUTMODE, DOCO_VARIABLE_ATTRIBUTE_ISINOUTMODE, Nothing, SyntaxPopupListItem.Icons.PROPERTY)

                            'Add transforms from the source involved
                            If var.Type <> Parser.CodeCompletion.Variable.Types.TemplateParameter Then
                                For Each src In analysis.Sources
                                    If StrEq(src.Name, var.SourceTag) Then
                                        src.Transforms = New Parser.Syntax.SourceTransforms(src.Name, src.Transformations)
                                        src.Transforms.Build()

                                        For Each n In src.Transforms.TransformInstructions.Nodes
                                            If StrEq(n.SetSubj, Parser.Syntax.SyntaxToken.TransformTargets.Param.ToString) And Not _
                                               Parser.Syntax.Constants.IsSystemAttribute(n.SetSubjAttrib) Then
                                                Me.AddListItem(n.SetSubjAttrib, "User defined transformation", Nothing, SyntaxPopupListItem.Icons.TRANSFORMATION)
                                            End If
                                        Next

                                        Exit For
                                    End If
                                Next
                            End If

                        Case Parser.CodeCompletion.Variable.Types.Source

                    End Select

                    Select Case var.Type
                        Case Parser.CodeCompletion.Variable.Types.Source
                        Case Else
                            Me.AddListItem(VARIABLE_METHOD_REPLACE, DOCO_VARIABLE_METHOD_REPLACE, VARIABLE_METHOD_REPLACE & "(""replace"",""with"")", SyntaxPopupListItem.Icons.METHOD)
                            Me.AddListItem(FUNC_IGNORE, DOCO_FUNC_IGNORE, FUNC_IGNORE & "(""ignore"")", SyntaxPopupListItem.Icons.METHOD)
                            Me.AddListItem(FUNC_USEONLY, DOCO_FUNC_USEONLY, FUNC_USEONLY & "(""useonly"")", SyntaxPopupListItem.Icons.METHOD)
                            Me.AddListItem(FUNC_REPLACEALL, DOCO_FUNC_REPLACEALL, FUNC_REPLACEALL & "(""replace"",""with"")", SyntaxPopupListItem.Icons.METHOD)

                    End Select

            End Select
        End Sub

    End Class

End Namespace
