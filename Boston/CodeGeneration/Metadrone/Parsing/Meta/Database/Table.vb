Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Source
Imports Boston.Tools
Imports Boston.PluginInterface.Sources

Namespace Parser.Meta.Database

    Friend Class Table
        Implements IEntity

        Private mValue As Object = Nothing
        Private SchemaRowVal As SchemaRow = Nothing
        Private Owner As IEntityConnection = Nothing
        Private ConnStr As String = Nothing

        Private ReplaceAllList As New ReplaceAllList()

        Private UseOnlyList As New List(Of String)
        Private IgnoreList As New List(Of String)

        Friend Relations As New List(Of IEntity) 'Boston specific I think.

        Private FilteredRelations As New List(Of IEntity) 'Boston specific. For stepping through Relations for the Column.

        Friend Columns As New List(Of IEntity)

        Private FilteredColumns As New List(Of IEntity)
        Private FilteredIDColumns As New List(Of IEntity)
        Private FilteredPKColumns As New List(Of IEntity)
        Private FilteredFKColumns As New List(Of IEntity)

        Private Transforms As Syntax.SourceTransforms = Nothing

        Friend ColumnCount As Integer = -1
        Friend PKColumnCount As Integer = -1
        Friend FKColumnCount As Integer = -1
        Friend IDColumnCount As Integer = -1
        Friend ListCount As Integer = -1
        Friend ListPos As Integer = -1

        Public Sub New()

        End Sub

        Public Sub New(ByVal Schema As List(Of SchemaRow),
                       ByVal Owner As IEntityConnection,
                       ByVal Connection As IConnection,
                       ByVal Transforms As Syntax.SourceTransforms,
                       ByRef SchemaRowIdx As Integer
                       ) 'SchemaRowIdx was ByRef

            '20200702-VM-'SchemaRowIdx was ByRef

            Me.Transforms = Transforms

            'Set table value and add first column
            Me.Value = Schema(SchemaRowIdx).Name
            Me.SchemaRowVal = Schema(SchemaRowIdx)
            'Me.AddCol(Schema(SchemaRowIdx), Connection)
            'SchemaRowIdx += 1

            Me.Owner = Owner
            Me.ConnStr = Owner.GetConnectionString

            'Add columns
            While SchemaRowIdx < Schema.Count
                If Schema(SchemaRowIdx).Name.Equals(Me.Value.ToString) Then
                    Me.AddCol(Schema(SchemaRowIdx), Connection)

                    'Add Relations            
                    'Dim larRelation As List(Of RDS.Relation)
                    'larRelation = Schema(SchemaRowIdx).Relation.Select(Function(x) x.Relation)
                    For Each lrRelation In Schema(SchemaRowIdx).Relation 'larRelation
                        Dim lsReferencedTableName As String = ""

                        Dim lsDestinationColumnName As String = ""
                        If lrRelation.DestinationColumns.Count = 1 Then
                            lsDestinationColumnName = lrRelation.DestinationColumns(0).Name
                        Else
                            Dim liInd As Integer = SchemaRowIdx
                            Dim lrOriginColumn As RDS.Column = lrRelation.OriginColumns.Find(Function(x) x.Id = Schema(liInd).ColumnId)
                            Dim lrDestinationColumn As RDS.Column = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole.Id = lrOriginColumn.ActiveRole.Id)
                            lsDestinationColumnName = lrDestinationColumn.Name
                        End If

                        Me.Relations.Add(New Relation(lrRelation.Id,
                                                      lrRelation.DestinationTable.Name,
                                                      lsDestinationColumnName))
                    Next

                Else
                    'Passed table
                    Exit While
                End If

                SchemaRowIdx += 1

            End While

            'Set listcount
            For Each col In Me.Columns
                CType(col, Column).ListCount = Me.Columns.Count
            Next
        End Sub

        Public Function GetCopy() As IEntity Implements IEntity.GetCopy
            'Dim Table As New Table()
            'Table.Value = Me.Value
            'Table.Owner = Me.Owner
            'Table.ConnStr = Me.ConnStr
            'Table.Transforms = Me.Transforms
            'For Each col In Me.Columns
            '    Table.Columns.Add(CType(col, Column).GetCopy)
            'Next
            'For Each r In Me.ReplaceAllList.List
            '    Table.ReplaceAllList.Add(r.OldVal, r.NewVal)
            'Next
            'For Each s In Me.UseOnlyList
            '    Table.UseOnlyList.Add(s)
            'Next
            'For Each s In Me.IgnoreList
            '    Table.IgnoreList.Add(s)
            'Next
            'Table.InitEntities()
            'Table.ListCount = Me.ListCount
            'Table.ListPos = Me.ListPos
            'Return Table

            'This not really used (just yet)
            Return New Table()
        End Function

        Private Sub AddCol(ByVal SchemaRow As SchemaRow, ByVal Connection As IConnection)
            Dim Column As New Column(SchemaRow.Column_Name, SchemaRow.GetCopy, Me, Connection, Me.Transforms)
            Column.ListPos = SchemaRow.Ordinal_Position
            Column.ListCount = -1

            Me.Columns.Add(Column)
        End Sub

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone.
        ''' </summary>
        ''' <returns></returns>
        Public Property Relation() As List(Of RDS.Relation)
            Get
                Return Me.SchemaRowVal.Relation
            End Get
            Set(ByVal value As List(Of RDS.Relation))
                Me.SchemaRowVal.Relation = value
            End Set
        End Property

        Public Property Value() As Object
            Get
                Return Me.mValue
            End Get
            Set(ByVal value As Object)
                Me.mValue = value
            End Set
        End Property

        Public ReadOnly Property IsTypeTable() As Boolean
            Get
                Return Me.SchemaRowVal.IsTable
            End Get
        End Property

        Public ReadOnly Property IsTypeView() As Boolean
            Get
                Return Me.SchemaRowVal.IsView
            End Get
        End Property

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

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATION) Then
                'set Relation
                Me.Relation = CType(value, List(Of RDS.Relation))

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

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATION) Then 'Boston specific. Not part of original Metadrone.
                'return relation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.Relation

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
                If Params.Count = 0 Then Throw New Exception("Expecting argument IndexOfColValue in '" & VARIABLE_METHOD_INDEXOFCOL & "'.")
                If Params.Count > 1 Then Throw New Exception("Too many arguments in '" & VARIABLE_METHOD_INDEXOFCOL & "'.")
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

        Friend Sub ApplyUseOnly(ByVal Name As String)
            'Add to list
            Me.UseOnlyList.Add(Name)

            Call Me.InitEntities()
        End Sub

        Friend Sub ApplyIgnore(ByVal Name As String)
            'Add to list
            Me.IgnoreList.Add(Name)

            Call Me.InitEntities()
        End Sub

        Public Sub InitEntities() Implements IEntity.InitEntities
            'Build use indexes
            Dim UseIdx As New List(Of Integer)
            'From use only list
            For i As Integer = 0 To Me.Columns.Count - 1
                If Me.UseOnlyList.Count > 0 Then
                    For j As Integer = 0 To Me.UseOnlyList.Count - 1
                        If PackageBuilder.PreProc.IgnoreCase Then
                            If StrEq(Me.Columns(i).GetAttributeValue("", Nothing, True, False).ToString, Me.UseOnlyList(j).ToString) Then
                                UseIdx.Add(i)
                            End If
                        Else
                            If Me.Columns(i).GetAttributeValue("", Nothing, True, False).ToString.Equals(Me.UseOnlyList(j).ToString) Then
                                UseIdx.Add(i)
                            End If
                        End If
                    Next
                Else
                    UseIdx.Add(i)
                End If
            Next

            'Remove any from ignore list
            For i As Integer = 0 To Me.IgnoreList.Count - 1
                Dim removed As Boolean = True
                While removed
                    removed = False
                    For j As Integer = 0 To UseIdx.Count - 1
                        If PackageBuilder.PreProc.IgnoreCase Then
                            If StrEq(Me.Columns(UseIdx(j)).GetAttributeValue("", Nothing, True, False).ToString, Me.IgnoreList(i).ToString) Then
                                UseIdx.RemoveAt(j)
                                removed = True
                                Exit For
                            End If
                        Else
                            If Me.Columns(UseIdx(j)).GetAttributeValue("", Nothing, True, False).ToString.Equals(Me.IgnoreList(i).ToString) Then
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
            Me.FilteredRelations = New List(Of IEntity)

            Dim liInd As Integer = 0

            For Each lrRelation In Me.Relations

                With CType(Me.Relations(liInd), Relation)
                    Me.FilteredRelations.Add(.GetCopy)
                End With
                liInd += 1
            Next

            For i As Integer = 0 To UseIdx.Count - 1
                With CType(Me.Columns(UseIdx(i)), Column)
                    Me.FilteredColumns.Add(.GetCopy)
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

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_RELATION 'Boston specific. Not part of original Metadrone.
                    Return Me.FilteredRelations

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_IDCOLUMN
                    Return Me.FilteredIDColumns

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_PKCOLUMN
                    Return Me.FilteredPKColumns

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_FKCOLUMN
                    Return Me.FilteredFKColumns

                Case Else
                    Throw New Exception("Invalid entity. Try recompiling.")

            End Select
        End Function

    End Class

End Namespace