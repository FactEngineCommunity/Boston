Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Source
Imports Boston.Tools
Imports Boston.PluginInterface.Sources

Namespace Parser.Meta.Database

    Friend Class Routine
        Implements IEntity

        Private mValue As Object = Nothing
        Private Owner As IEntityConnection = Nothing
        Private ConnStr As String = Nothing

        Private ReplaceAllList As New ReplaceAllList()

        Private ParamUseOnlyList As New List(Of String)
        Private ParamIgnoreList As New List(Of String)


        Private FilteredParams As New List(Of IEntity)
        Private FilteredInParams As New List(Of IEntity)
        Private FilteredOutParams As New List(Of IEntity)
        Private FilteredInOutParams As New List(Of IEntity)

        Private ColumnUseOnlyList As New List(Of String)
        Private ColumnIgnoreList As New List(Of String)

        Friend Columns As New List(Of IEntity)

        Private FilteredColumns As New List(Of IEntity)
        Private FilteredIDColumns As New List(Of IEntity)
        Private FilteredPKColumns As New List(Of IEntity)
        Private FilteredFKColumns As New List(Of IEntity)

        Private Transforms As Syntax.SourceTransforms = Nothing

        Friend Params As New List(Of IEntity)
        Friend SchemaRowVal As SchemaRow = Nothing

        Friend ColumnCount As Integer = -1
        Friend PKColumnCount As Integer = -1
        Friend FKColumnCount As Integer = -1
        Friend IDColumnCount As Integer = -1
        Friend ParamCount As Integer = -1
        Friend InParamCount As Integer = -1
        Friend OutParamCount As Integer = -1
        Friend InOutParamCount As Integer = -1
        Friend ListCount As Integer = -1
        Friend ListPos As Integer = -1

        Public Sub New()

        End Sub

        Public Sub New(ByVal Schema As List(Of SchemaRow), ByVal Owner As IEntityConnection, ByVal Connection As IConnection, _
                       ByVal Transforms As Syntax.SourceTransforms, ByRef SchemaRowIdx As Integer)
            Me.Transforms = Transforms

            'Set routine value and add first parameter
            Me.Value = Schema(SchemaRowIdx).Name
            Me.SchemaRowVal = Schema(SchemaRowIdx)
            If Not String.IsNullOrEmpty(Schema(SchemaRowIdx).Parameter_Name) Then Me.AddParam(Schema(SchemaRowIdx), Connection)
            SchemaRowIdx += 1

            Me.Owner = Owner
            Me.ConnStr = Owner.GetConnectionString

            'Add params
            While SchemaRowIdx < Schema.Count
                If Schema(SchemaRowIdx).Name.Equals(Me.Value.ToString) Then
                    If Not String.IsNullOrEmpty(Schema(SchemaRowIdx).Parameter_Name) Then Me.AddParam(Schema(SchemaRowIdx), Connection)
                Else
                    'Passed routine
                    Exit While
                End If

                SchemaRowIdx += 1
            End While

            'Set listcount
            For Each parm In Me.Params
                CType(parm, Parameter).ListCount = Me.Params.Count
            Next
        End Sub

        Public Function GetCopy() As IEntity Implements IEntity.GetCopy
            'This not really used (just yet)
            Return New Routine()
        End Function

        Private Sub AddParam(ByVal SchemaRow As SchemaRow, ByVal Connection As IConnection)
            Dim Param As New Parameter(SchemaRow.Parameter_Name, SchemaRow.GetCopy, Me, Connection, Me.Transforms)
            Param.ListPos = SchemaRow.Ordinal_Position
            Param.ListCount = -1

            Me.Params.Add(Param)
        End Sub

        Public Sub AddColumns(ByVal Schema As List(Of SchemaRow), ByVal Connection As IConnection)
            For Each sr In Schema
                Dim Column As New Column(sr.Column_Name, sr.GetCopy, Me, Connection, Me.Transforms)
                Column.ListPos = sr.Ordinal_Position
                Column.ListCount = -1

                Me.Columns.Add(Column)
            Next

            'Set listcount
            For Each col In Me.Columns
                CType(col, Column).ListCount = Me.Columns.Count
            Next
        End Sub

        Public Property Value() As Object
            Get
                Return Me.mValue
            End Get
            Set(ByVal value As Object)
                Me.mValue = value
            End Set
        End Property

        Public ReadOnly Property IsTypeProcedure() As Boolean
            Get
                Return Me.SchemaRowVal.IsProcedure
            End Get
        End Property

        Public ReadOnly Property IsTypeFunction() As Boolean
            Get
                Return Me.SchemaRowVal.IsFunction
            End Get
        End Property

        Public Property Relations As List(Of IEntity) Implements IEntity.Relations
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As List(Of IEntity))
                Throw New NotImplementedException()
            End Set
        End Property

        'This will return index based on 1 (like listpos)
        Public Function IndexOfParam(ByVal ParamName As String) As Integer
            'Best to warn user of empty param schema (may be using it incorrectly such as indexofparam on a parameter variable).
            If Me.Params Is Nothing Then Throw New Exception("No parameter schema is loaded for routine '" & Me.Value.ToString & "'")

            For i As Integer = 0 To Me.Params.Count - 1
                If CType(Me.Params(i), Parameter).Value.Equals(ParamName) Then Return i + 1
            Next

            Return -1
        End Function

        'This will return index based on 1 (like listpos)
        Public Function IndexOfCol(ByVal ColName As String) As Integer
            'If Me.ColSchema Is Nothing Then Return -1
            'Best to warn user of empty column schema (may be using it incorrectly such as indexofcol on a column variable).
            If Me.Columns Is Nothing Then Throw New Exception("No column schema is loaded for table '" & Me.Value.ToString & "'")

            For i As Integer = 0 To Me.Columns.Count - 1
                If CType(Me.Columns(i), Column).Value.Equals(ColName) Then Return i + 1
            Next

            Return -1
        End Function

        Public Sub SetAttributeValue(ByVal AttribName As String, ByVal value As Object) Implements IEntity.SetAttributeValue
            If AttribName Is Nothing Then AttribName = ""
            If AttribName.Length = 0 Or StrEq(AttribName, VARIABLE_ATTRIBUTE_VALUE) Then
                'set value
                Me.Value = value

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTCOUNT) Then
                'set listcount
                Me.ListCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTPOS) Then
                'set listpos
                Me.ListPos = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_COLUMNCOUNT) Then
                'set column count
                Me.ColumnCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PKCOLUMNCOUNT) Then
                'set pkcolumn count
                Me.PKColumnCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_FKCOLUMNCOUNT) Then
                'set fkcolumn count
                Me.FKColumnCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_IDCOLUMNCOUNT) Then
                'set idcolumn count
                Me.IDColumnCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PARAMCOUNT) Then
                'set param count
                Me.ParamCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INPARAMCOUNT) Then
                'set param count
                Me.InParamCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_OUTPARAMCOUNT) Then
                'set param count
                Me.OutParamCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INOUTPARAMCOUNT) Then
                'set param count
                Me.InOutParamCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_METHOD_REPLACE) Then
                Throw New Exception("""" & VARIABLE_METHOD_REPLACE & """ is a method and cannot be a target of an assignment.")

            ElseIf StrEq(AttribName, VARIABLE_METHOD_INDEXOFCOL) Then
                Throw New Exception("""" & VARIABLE_METHOD_INDEXOFCOL & """ is a method and cannot be a target of an assignment.")

            ElseIf StrIdxOf(AttribName, FUNC_IGNORE) = 0 Then
                Throw New Exception("""" & FUNC_IGNORE & """ is a method and cannot be a target of an assignment.")

            ElseIf StrIdxOf(AttribName, FUNC_USEONLY) = 0 Then
                Throw New Exception("""" & FUNC_USEONLY & """ is a method and cannot be a target of an assignment.")

            Else
                Throw New Exception("Invalid attribute: " & AttribName & ". ")

            End If
        End Sub

        Public Function GetAttributeValue(ByVal AttribName As String, ByVal Params As List(Of Object), _
                                          ByVal LookTransformsIfNotFound As Boolean, ByRef ExitBlock As Boolean) As Object Implements IEntity.GetAttributeValue
            If Params Is Nothing Then Params = New List(Of Object)
            If AttribName Is Nothing Then AttribName = ""
            If AttribName.Length = 0 Or StrEq(AttribName, VARIABLE_ATTRIBUTE_VALUE) Then
                'return value
                Call Me.CheckParamsForPropertyCall(VARIABLE_ATTRIBUTE_VALUE, Params)
                Return Me.ReplaceAllList.ApplyReplaces(Me.Value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTCOUNT) Then
                'return listcount
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.ListCount

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTPOS) Then
                'return listpos
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.ListPos

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_COLUMNCOUNT) Then
                'return column count
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.ColumnCount

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PKCOLUMNCOUNT) Then
                'return pkcolumn count
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.PKColumnCount

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_FKCOLUMNCOUNT) Then
                'return fkcolumn count
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.FKColumnCount

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_IDCOLUMNCOUNT) Then
                'return idcolumn count
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.IDColumnCount

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PARAMCOUNT) Then
                'return param count
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.ParamCount

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INPARAMCOUNT) Then
                'return in param count
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.InParamCount

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_OUTPARAMCOUNT) Then
                'return out param count
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.OutParamCount

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INOUTPARAMCOUNT) Then
                'return in-out param count
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.InOutParamCount

            ElseIf StrIdxOf(AttribName, VARIABLE_METHOD_REPLACE) = 0 Then
                'employ replace method
                Select Case Params.Count
                    Case 0 : Throw New Exception("Expecting argument OldVal in '" & VARIABLE_METHOD_REPLACE & "'.")
                    Case 1 : Throw New Exception("Expecting argument NewVal in '" & VARIABLE_METHOD_REPLACE & "'.")
                    Case Is > 2 : Throw New Exception("Too many arguments in '" & VARIABLE_METHOD_REPLACE & "'.")
                End Select
                If PackageBuilder.PreProc.IgnoreCase Then
                    Return ReplaceInsensitive(Me.ReplaceAllList.ApplyReplaces(Me.Value).ToString, Params(0).ToString, Params(1).ToString)
                Else
                    Return Me.ReplaceAllList.ApplyReplaces(Me.Value).ToString.Replace(Params(0).ToString, Params(1).ToString)
                End If

            ElseIf StrIdxOf(AttribName, FUNC_REPLACEALL) = 0 Then
                'add replace all value
                Select Case Params.Count
                    Case 0 : Throw New Exception("Expecting argument OldVal in '" & FUNC_REPLACEALL & "'.")
                    Case 1 : Throw New Exception("Expecting argument NewVal in '" & FUNC_REPLACEALL & "'.")
                    Case Is > 2 : Throw New Exception("Too many arguments in '" & FUNC_REPLACEALL & "'.")
                End Select
                Me.ReplaceAllList.Add(Params(0).ToString, Params(1).ToString)
                Return Nothing

            ElseIf StrIdxOf(AttribName, VARIABLE_METHOD_INDEXOFCOL) = 0 Then
                'return column index (if any)
                Select Case Params.Count
                    Case 0 : Throw New Exception("Expecting argument IndexOfColValue in '" & VARIABLE_METHOD_INDEXOFCOL & "'.")
                    Case Is > 1 : Throw New Exception("Too many arguments in '" & VARIABLE_METHOD_INDEXOFCOL & "'.")
                End Select
                Return Me.IndexOfCol(Params(0).ToString).ToString

            ElseIf StrIdxOf(AttribName, FUNC_IGNORE) = 0 Then
                'add ignore
                Select Case Params.Count
                    Case 0 : Throw New Exception("Expecting argument IgnoreValue in '" & FUNC_IGNORE & "'.")
                    Case Is > 1 : Throw New Exception("Too many arguments in '" & FUNC_IGNORE & "'.")
                End Select
                CType(Me.Owner, Schema).ApplyIgnore(Params(0).ToString)
                ExitBlock = True
                Return Nothing

            ElseIf StrIdxOf(AttribName, FUNC_USEONLY) = 0 Then
                'add use only
                Select Case Params.Count
                    Case 0 : Throw New Exception("Expecting argument UseOnlyValue in '" & FUNC_USEONLY & "'.")
                    Case Is > 1 : Throw New Exception("Too many arguments in '" & FUNC_USEONLY & "'.")
                End Select
                CType(Me.Owner, Schema).ApplyUseOnly(Params(0).ToString)
                ExitBlock = True
                Return Nothing

            Else
                If LookTransformsIfNotFound Then
                    Return Me.Transforms.GetAttributeValue(Me, AttribName)
                Else
                    'This will avoid stack overflow when called from SourceTransforms
                    Throw New Exception("Invalid attribute: " & AttribName & ". ")
                End If

            End If
        End Function

        Private Sub CheckParamsForPropertyCall(ByVal AttribName As String, ByVal Params As List(Of Object))
            If Params.Count > 0 Then Throw New Exception("'" & AttribName & "' is not a method.")
        End Sub

        Public Function GetConnectionString() As String Implements IEntity.GetConnectionString
            Return Me.ConnStr
        End Function

        Friend Sub ApplyParamUseOnly(ByVal Name As String)
            'Add to list
            Me.ParamUseOnlyList.Add(Name)

            Call Me.InitEntities()
        End Sub

        Friend Sub ApplyParamIgnore(ByVal Name As String)
            'Add to list
            Me.ParamIgnoreList.Add(Name)

            Call Me.InitEntities()
        End Sub

        Friend Sub ApplyColumnUseOnly(ByVal Name As String)
            'Add to list
            Me.ColumnUseOnlyList.Add(Name)

            Call Me.InitEntities()
        End Sub

        Friend Sub ApplyColumnIgnore(ByVal Name As String)
            'Add to list
            Me.ColumnIgnoreList.Add(Name)

            Call Me.InitEntities()
        End Sub

        Public Sub InitEntities() Implements IEntity.InitEntities
            'Build use indexes for parameters
            Dim UseIdx As New List(Of Integer)
            'From use only list
            For i As Integer = 0 To Me.Params.Count - 1
                If Me.ParamUseOnlyList.Count > 0 Then
                    For j As Integer = 0 To Me.ParamUseOnlyList.Count - 1
                        If PackageBuilder.PreProc.IgnoreCase Then
                            If StrEq(Me.Params(i).GetAttributeValue("", Nothing, True, False).ToString, Me.ParamUseOnlyList(j).ToString) Then
                                UseIdx.Add(i)
                            End If
                        Else
                            If Me.Params(i).GetAttributeValue("", Nothing, True, False).ToString.Equals(Me.ParamUseOnlyList(j).ToString) Then
                                UseIdx.Add(i)
                            End If
                        End If
                    Next
                Else
                    UseIdx.Add(i)
                End If
            Next
            'Remove any from ignore list
            For i As Integer = 0 To Me.ParamIgnoreList.Count - 1
                Dim removed As Boolean = True
                While removed
                    removed = False
                    For j As Integer = 0 To UseIdx.Count - 1
                        If PackageBuilder.PreProc.IgnoreCase Then
                            If StrEq(Me.Params(UseIdx(j)).GetAttributeValue("", Nothing, True, False).ToString, Me.ParamIgnoreList(i).ToString) Then
                                UseIdx.RemoveAt(j)
                                removed = True
                                Exit For
                            End If
                        Else
                            If Me.Params(UseIdx(j)).GetAttributeValue("", Nothing, True, False).ToString.Equals(Me.ParamIgnoreList(i).ToString) Then
                                UseIdx.RemoveAt(j)
                                removed = True
                                Exit For
                            End If
                        End If
                    Next
                End While
            Next

            'Rebuild lists from use indexes
            Me.FilteredParams = New List(Of IEntity)
            Me.FilteredInParams = New List(Of IEntity)
            Me.FilteredOutParams = New List(Of IEntity)
            Me.FilteredInOutParams = New List(Of IEntity)

            For i As Integer = 0 To UseIdx.Count - 1
                With CType(Me.Params(UseIdx(i)), Parameter)
                    Me.FilteredParams.Add(Me.Params(UseIdx(i)))
                    If .IsInMode Then Me.FilteredInParams.Add(.GetCopy)
                    If .IsOutMode Then Me.FilteredOutParams.Add(.GetCopy)
                    If .IsInOutMode Then Me.FilteredInOutParams.Add(.GetCopy)
                End With
            Next
            For i As Integer = 0 To Me.FilteredParams.Count - 1
                CType(Me.FilteredParams(i), Parameter).ListPos = i + 1
                CType(Me.FilteredParams(i), Parameter).ListCount = Me.FilteredParams.Count
            Next
            For i As Integer = 0 To Me.FilteredInParams.Count - 1
                CType(Me.FilteredInParams(i), Parameter).ListPos = i + 1
                CType(Me.FilteredInParams(i), Parameter).ListCount = Me.FilteredInParams.Count
            Next
            For i As Integer = 0 To Me.FilteredInOutParams.Count - 1
                CType(Me.FilteredInOutParams(i), Parameter).ListPos = i + 1
                CType(Me.FilteredInOutParams(i), Parameter).ListCount = Me.FilteredInOutParams.Count
            Next

            'Update param count
            Me.ParamCount = Me.FilteredParams.Count
            Me.InParamCount = Me.FilteredInParams.Count
            Me.OutParamCount = Me.FilteredOutParams.Count
            Me.InOutParamCount = Me.FilteredInOutParams.Count

            'Build use indexes for columns
            UseIdx = New List(Of Integer)
            'From use only list
            For i As Integer = 0 To Me.Columns.Count - 1
                If Me.ColumnUseOnlyList.Count > 0 Then
                    For j As Integer = 0 To Me.ColumnUseOnlyList.Count - 1
                        If PackageBuilder.PreProc.IgnoreCase Then
                            If StrEq(Me.Columns(i).GetAttributeValue("", Nothing, True, False).ToString, Me.ColumnUseOnlyList(j).ToString) Then
                                UseIdx.Add(i)
                            End If
                        Else
                            If Me.Columns(i).GetAttributeValue("", Nothing, True, False).ToString.Equals(Me.ColumnUseOnlyList(j).ToString) Then
                                UseIdx.Add(i)
                            End If
                        End If
                    Next
                Else
                    UseIdx.Add(i)
                End If
            Next
            'Remove any from ignore list
            For i As Integer = 0 To Me.ColumnIgnoreList.Count - 1
                Dim removed As Boolean = True
                While removed
                    removed = False
                    For j As Integer = 0 To UseIdx.Count - 1
                        If PackageBuilder.PreProc.IgnoreCase Then
                            If StrEq(Me.Columns(UseIdx(j)).GetAttributeValue("", Nothing, True, False).ToString, Me.ColumnIgnoreList(i).ToString) Then
                                UseIdx.RemoveAt(j)
                                removed = True
                                Exit For
                            End If
                        Else
                            If Me.Columns(UseIdx(j)).GetAttributeValue("", Nothing, True, False).ToString.Equals(Me.ColumnIgnoreList(i).ToString) Then
                                UseIdx.RemoveAt(j)
                                removed = True
                                Exit For
                            End If
                        End If
                    Next
                End While
            Next

            'Rebuild lists from use indexes
            Me.FilteredColumns = New List(Of IEntity)
            Me.FilteredIDColumns = New List(Of IEntity)
            Me.FilteredPKColumns = New List(Of IEntity)
            Me.FilteredFKColumns = New List(Of IEntity)

            For i As Integer = 0 To UseIdx.Count - 1
                With CType(Me.Columns(UseIdx(i)), Column)
                    Me.FilteredColumns.Add(Me.Columns(UseIdx(i)))
                    If .IsIdentity Then Me.FilteredIDColumns.Add(.GetCopy)
                    If .IsPrimaryKey Then Me.FilteredPKColumns.Add(.GetCopy)
                    If .IsForeignKey Then Me.FilteredFKColumns.Add(.GetCopy)
                End With
            Next
            For i As Integer = 0 To Me.FilteredColumns.Count - 1
                CType(Me.FilteredColumns(i), Column).ListPos = i + 1
                CType(Me.FilteredColumns(i), Column).ListCount = Me.FilteredColumns.Count
            Next
            For i As Integer = 0 To Me.FilteredIDColumns.Count - 1
                CType(Me.FilteredIDColumns(i), Column).ListPos = i + 1
                CType(Me.FilteredIDColumns(i), Column).ListCount = Me.FilteredIDColumns.Count
            Next
            For i As Integer = 0 To Me.FilteredPKColumns.Count - 1
                CType(Me.FilteredPKColumns(i), Column).ListPos = i + 1
                CType(Me.FilteredPKColumns(i), Column).ListCount = Me.FilteredPKColumns.Count
            Next
            For i As Integer = 0 To Me.FilteredFKColumns.Count - 1
                CType(Me.FilteredFKColumns(i), Column).ListPos = i + 1
                CType(Me.FilteredFKColumns(i), Column).ListCount = Me.FilteredFKColumns.Count
            Next

            'Update column counts
            Me.ColumnCount = Me.FilteredColumns.Count
            Me.PKColumnCount = Me.FilteredPKColumns.Count
            Me.FKColumnCount = Me.FilteredFKColumns.Count
            Me.IDColumnCount = Me.FilteredIDColumns.Count
        End Sub

        Public Function GetEntities(ByVal Entity As Syntax.SyntaxNode.ExecForEntities) As List(Of IEntity) Implements IEntity.GetEntities
            'Check predicate against type of entity, and return appropriate collection
            Select Case Entity
                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_COLUMN
                    Return Me.FilteredColumns

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_IDCOLUMN
                    Return Me.FilteredIDColumns

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_PKCOLUMN
                    Return Me.FilteredPKColumns

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_FKCOLUMN
                    Return Me.FilteredFKColumns

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_PARAMETER
                    Return Me.FilteredParams

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_INPARAMETER
                    Return Me.FilteredInParams

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_OUTPARAMETER
                    Return Me.FilteredOutParams

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_INOUTPARAMETER
                    Return Me.FilteredInOutParams

                Case Else
                    Throw New Exception("Invalid entity. Try recompiling.")

            End Select
        End Function

    End Class

End Namespace