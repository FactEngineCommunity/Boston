Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Source
Imports Boston.Tools
Imports Boston.PluginInterface.Sources

Namespace Parser.Meta.Database

    Friend Class Table
        Implements IEntity

        Public Schema As Parser.Meta.Database.Schema
        Private mValue As Object = Nothing
        Private SchemaRowVal As SchemaRow = Nothing
        Private Owner As IEntityConnection = Nothing
        Private ConnStr As String = Nothing

        Private ReplaceAllList As New ReplaceAllList()

        Private UseOnlyList As New List(Of String)
        Private IgnoreList As New List(Of String)

        Friend Indexes As New List(Of IEntity) 'Boston specific.
        Friend Relations As New List(Of IEntity) 'Boston specific I think.
        Friend IncomingRelations As New List(Of IEntity) 'Boston specific.

        Private FilteredRelations As New List(Of IEntity) 'Boston specific. For stepping through Relations for the Table.
        Private FilteredIncomingRelations As New List(Of IEntity) 'Boston specific. For stepping through Incoming Relations for the Table.
        Private FilteredIndexes As New List(Of IEntity) 'Boston specific. For stepping through Indexes for the Table. Can be List(Of Index) containing Columns

        Private mIsPGSRelation As Boolean = False 'Boston specific. True if the Table represents a Property Graph Schema Relation.
        Private mPGSEdgeName As String = ""

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

        Public Sub New(ByVal aarSchemaRow As List(Of SchemaRow),
                       ByVal Owner As IEntityConnection,
                       ByVal Connection As IConnection,
                       ByVal Transforms As Syntax.SourceTransforms,
                       ByRef SchemaRowIdx As Integer,
                       ByRef arSchema As Parser.Meta.Database.Schema
                       ) 'SchemaRowIdx was ByRef

            '20200702-VM-'SchemaRowIdx was ByRef
            Try
                Me.Schema = arSchema
                Me.Transforms = Transforms

                'Set table value and add first column
                Me.Value = aarSchemaRow(SchemaRowIdx).Name
                Me.SchemaRowVal = aarSchemaRow(SchemaRowIdx)

                '20200705-VM-Remove if all seems okay. Moved to below, so that the Relations are loaded.
                'Me.AddCol(Schema(SchemaRowIdx), Connection)
                'SchemaRowIdx += 1

                Me.Owner = Owner
                Me.ConnStr = Owner.GetConnectionString
            Catch ex As Exception
                Throw New Parser.Syntax.ExecException(ex.Message, 0)
            End Try


            'Add columns
            While SchemaRowIdx < aarSchemaRow.Count
                If aarSchemaRow(SchemaRowIdx).Name.Equals(Me.Value.ToString) Then
                    Me.AddCol(aarSchemaRow(SchemaRowIdx), Connection)

                    Me.mIsPGSRelation = aarSchemaRow(SchemaRowIdx).IsPGSRelation
                    Me.mPGSEdgeName = aarSchemaRow(SchemaRowIdx).PGSEdgeName

                    'Add Outgoing Relations            
                    For Each lrRelation In aarSchemaRow(SchemaRowIdx).Relation

                        Try
                            Dim lsReferencedTableName As String = ""
                            Dim lsDestinationColumnName As String = ""
                            Dim liInd As Integer = SchemaRowIdx
                            Dim lsOriginColumnName As String = ""
                            Dim lsOriginRoleName As String = ""
                            Dim lsDestinationRoleName As String = ""

                            If lrRelation.DestinationColumns.Count = 1 Then
                                lsOriginColumnName = lrRelation.OriginColumns(0).Name
                                lsOriginRoleName = lrRelation.OriginColumns(0).Role.DerivedRoleName
                                lsDestinationColumnName = lrRelation.DestinationColumns(0).Name
                                lsDestinationRoleName = lrRelation.ResponsibleFactType.GetOtherRoleOfBinaryFactType(lrRelation.OriginColumns(0).Role.Id).DerivedRoleName
                            Else
                                Dim lrOriginColumn As RDS.Column = lrRelation.OriginColumns.Find(Function(x) x.Id = aarSchemaRow(liInd).ColumnId)
                                lsOriginColumnName = lrOriginColumn.Name
                                Dim lrDestinationColumn As RDS.Column = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole.Id = lrOriginColumn.ActiveRole.Id)
                                If lrDestinationColumn Is Nothing Then
                                    Throw New Exception("No destination column found for relationship with originating column:" & lrOriginColumn.Table.Name & "." & lrOriginColumn.Name)
                                End If
                                lsDestinationColumnName = lrDestinationColumn.Name
                                lsOriginRoleName = lrOriginColumn.Role.DerivedRoleName
                                lsDestinationRoleName = lrRelation.ResponsibleFactType.GetOtherRoleOfBinaryFactType(lrOriginColumn.Role.Id).DerivedRoleName
                            End If

                            If Me.Relation.Find(Function(x) x.Id = lrRelation.Id) Is Nothing Then
                                Me.Relation.AddUnique(lrRelation)
                            End If

                            Me.Relations.Add(New Relation(lrRelation.Id,
                                                          lrRelation.OriginTable.Name,
                                                          lsOriginColumnName,
                                                          lrRelation.DestinationTable.Name,
                                                          lsDestinationColumnName,
                                                          lrRelation.OriginColumns.Count,
                                                          lrRelation.ResponsibleFactType.Id,
                                                          lsOriginRoleName,
                                                          lsDestinationRoleName))
                        Catch ex As Exception
                            Throw New Parser.Syntax.ExecException(ex.Message, 0)
                        End Try
                    Next

                    'Add Incoming Relations            
                    For Each lrRelation In aarSchemaRow(SchemaRowIdx).IncomingRelation

                        Try
                            Dim lsReferencedTableName As String = ""

                            Dim lsDestinationColumnName As String = ""

                            Dim liInd As Integer = SchemaRowIdx
                            Dim lsOriginColumnName As String = ""
                            Dim lsOriginRoleName As String = ""
                            Dim lsDestinationRoleName As String = ""

                            If lrRelation.DestinationColumns.Count = 1 Then
                                lsDestinationColumnName = lrRelation.OriginColumns(0).Name
                                lsDestinationRoleName = lrRelation.OriginColumns(0).Role.DerivedRoleName
                                lsOriginColumnName = lrRelation.DestinationColumns(0).Name
                                lsOriginRoleName = lrRelation.ResponsibleFactType.GetOtherRoleOfBinaryFactType(lrRelation.OriginColumns(0).Role.Id).DerivedRoleName
                            Else
                                Dim lrDestinationColumn As RDS.Column = lrRelation.DestinationColumns.Find(Function(x) x.Id = aarSchemaRow(liInd).ColumnId)
                                lsDestinationColumnName = lrDestinationColumn.Name
                                Dim lrOriginColumn As RDS.Column = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole.Id = lrDestinationColumn.ActiveRole.Id)
                                If lrOriginColumn Is Nothing Then
                                    Throw New Exception("No destination column found for relationship with originating column:" & lrOriginColumn.Table.Name & "." & lrOriginColumn.Name)
                                End If
                                lsOriginColumnName = lrOriginColumn.Name
                                lsDestinationRoleName = lrDestinationColumn.Role.DerivedRoleName
                                lsOriginRoleName = lrRelation.ResponsibleFactType.GetOtherRoleOfBinaryFactType(lrDestinationColumn.Role.Id).DerivedRoleName
                            End If

                            If Me.IncomingRelation.Find(Function(x) x.Id = lrRelation.Id) Is Nothing Then
                                Me.IncomingRelation.AddUnique(lrRelation)
                            End If

                            Me.IncomingRelations.Add(New Relation(lrRelation.Id,
                                                                  lrRelation.OriginTable.Name,
                                                                  lsOriginColumnName,
                                                                  lrRelation.DestinationTable.Name,
                                                                  lsDestinationColumnName,
                                                                  lrRelation.OriginColumns.Count,
                                                                  lrRelation.ResponsibleFactType.Id,
                                                                  lsOriginRoleName,
                                                                  lsDestinationRoleName))
                        Catch ex As Exception
                            Throw New Parser.Syntax.ExecException(ex.Message, 0)
                        End Try
                    Next

                    '============================
                    'Indexes
                    For Each lrIndex In aarSchemaRow(SchemaRowIdx).Index

                        Try
                            Dim lrEntityIndex = New Index(lrIndex.Name,
                                                      lrIndex.Column,
                                                      Me,
                                                      Nothing,
                                                      Me.Transforms,
                                                      lrIndex.IsPrimaryKey)

                            If Me.Indexes.Find(Function(x) x.GetAttributeValue("name", Nothing, True, False) = lrIndex.Name) Is Nothing Then
                                Me.Indexes.Add(lrEntityIndex)
                            End If
                        Catch ex As Exception
                            Throw New Parser.Syntax.ExecException(ex.Message, 0)
                        End Try

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

            '20200705-VM-Aparently not used yet. As per the bottom of this method.
            'This not really used (just yet)

            Dim Table As New Table()
            Table.Value = Me.Value
            Table.Owner = Me.Owner
            Table.ConnStr = Me.ConnStr
            Table.Transforms = Me.Transforms
            'For Each col In Me.Columns
            '    Table.Columns.Add(CType(col, Column).GetCopy)
            'Next
            For Each r In Me.ReplaceAllList.List
                Table.ReplaceAllList.Add(r.OldVal, r.NewVal)
            Next
            'For Each s In Me.UseOnlyList
            '    Table.UseOnlyList.Add(s)
            'Next
            'For Each s In Me.IgnoreList
            '    Table.IgnoreList.Add(s)
            'Next
            'Table.InitEntities()
            'Table.ListCount = Me.ListCount
            'Table.ListPos = Me.ListPos
            Return Table '20200703-VM-Was commented out

            'This not really used (just yet)
            'Return New Table() '20200703-VM-Was not commented out
        End Function

        Private Sub AddCol(ByVal SchemaRow As SchemaRow, ByVal Connection As IConnection)
            Dim Column As New Column(SchemaRow.Column_Name, SchemaRow.GetCopy, Me, Connection, Me.Transforms)
            Column.ListPos = SchemaRow.Ordinal_Position
            Column.ListCount = -1

            Me.Columns.Add(Column)
        End Sub

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone. The set of Indexes associated with the Table.
        ''' </summary>
        ''' <returns></returns>
        Public Property Index() As List(Of RDS.Index)
            Get
                Return Me.SchemaRowVal.Index
            End Get
            Set(ByVal value As List(Of RDS.Index))
                Me.SchemaRowVal.Index = value
            End Set
        End Property

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone. The set of Relations associated with the Table.
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

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone. The set of Relations associated with the Table.
        ''' </summary>
        ''' <returns></returns>
        Public Property IncomingRelation() As List(Of RDS.Relation)
            Get
                Return Me.SchemaRowVal.IncomingRelation
            End Get
            Set(ByVal value As List(Of RDS.Relation))
                Me.SchemaRowVal.IncomingRelation = value
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

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PGSEDGENAME) Then
                'set value
                Me.mPGSEdgeName = value

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISPGSRELATION) Then
                'set value
                Me.mIsPGSRelation = value

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATIONS) Then 'The set of Relations stemming from the Table. Not at the Column level, but the Table level.
                'set Relations
                For Each lrIEntityRelation In value
                    Me.Relations.Add(CType(lrIEntityRelation, Relation))
                Next

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INCOMINGRELATIONS) Then 'The set of Relations coming into the Table. Not at the Column level, but the Table level.
                'set Relations
                For Each lrIEntityRelation In value
                    Me.IncomingRelations.Add(CType(lrIEntityRelation, Relation))
                Next

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATION) Then 'The set of Relations stemming from the Table at the Column level.
                'set Relation
                Me.Relation = CType(value, List(Of RDS.Relation))

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INCOMINGRELATION) Then 'The set of Incoming Relations stemming from the Table at the Column level.
                'set Relation
                Me.IncomingRelation = CType(value, List(Of RDS.Relation))

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INDEX) Then 'The set of Indexes for the Table.
                'set Relation
                Me.Index = CType(value, List(Of RDS.Index))

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

        ''' <summary>
        ''' Used in sorting of Tables by Tables referenced by other Tables, as in when for creating output SQL in the propper Table order
        '''   such that Foreign Key reference constraints are created in the correct order.
        ''' NB Relies on the ListPos property of the Table being correct.
        ''' </summary>
        ''' <returns></returns>
        Public Function hasHigherReferencedTable() As Boolean

            Dim larEntity As New List(Of IEntity)
            Dim liInd As Integer
            Try

                larEntity = Me.Schema.GetEntities(Syntax.SyntaxNode.ExecForEntities.OBJECT_TABLE)

                If Me.ListPos = larEntity.Count - 1 Then Return False

                For liInd = Me.ListPos To larEntity.Count - 2
                    For Each lrRelation In Me.Relation
                        Dim lrComparisonTable As Table = CType(larEntity(liInd + 1), Table)

                        If lrRelation.DestinationTable.Name = lrComparisonTable.Value Then
                            If lrComparisonTable.Relation.Find(Function(x) x.DestinationTable.Name = Me.Value) Is Nothing Then
                                'Tables don't reference themselves in a loop
                                Return True
                            End If
                        End If
                    Next
                Next

            Catch ex As Exception
                Return False
            End Try

            Return False

        End Function

        Public Function GetAttributeValue(ByVal AttribName As String, ByVal Params As List(Of Object),
                                          ByVal LookTransformsIfNotFound As Boolean, ByRef ExitBlock As Boolean) As Object Implements IEntity.GetAttributeValue

            If Params Is Nothing Then Params = New List(Of Object)

            If AttribName Is Nothing Then AttribName = ""

            If AttribName.Length = 0 Or StrEq(AttribName, VARIABLE_ATTRIBUTE_VALUE) Then
                'return value
                Call Me.CheckParamsForPropertyCall(VARIABLE_ATTRIBUTE_VALUE, Params)
                Return Me.ReplaceAllList.ApplyReplaces(Me.Value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PGSEDGENAME) Then 'Boston specific. Not part of original Metadrone.
                'return mPGSEdgeName
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.mPGSEdgeName

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISPGSRELATION) Then 'Boston specific. Not part of original Metadrone.
                'return mIsPGSRelation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.mIsPGSRelation

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATIONS) Then 'Boston specific. Not part of original Metadrone.
                'return relation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.Relations

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INCOMINGRELATIONS) Then 'Boston specific. Not part of original Metadrone.
                'return relation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.IncomingRelations

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATION) Then 'Boston specific. Not part of original Metadrone.
                'return relation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.Relation

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INCOMINGRELATION) Then 'Boston specific. Not part of original Metadrone.
                'return relation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.IncomingRelation

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INDEX) Then 'Boston specific. Not part of original Metadrone.
                'return relation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.Index

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
            Me.FilteredIncomingRelations = New List(Of IEntity)
            Me.FilteredIndexes = New List(Of IEntity)

            Dim liInd As Integer = 0
            Try
                For Each lrRelation In Me.Relations

                    Dim lrEntityRelation = CType(Me.Relations(liInd), Relation)
                    With lrEntityRelation
                        Dim lsReferencedTableName = lrEntityRelation.GetAttributeValue("referencedtablename", Nothing, True, False)
                        If Me.FilteredRelations.Find(Function(x) x.GetAttributeValue("referencedtablename", Nothing, True, False) =
                                                     lsReferencedTableName) Is Nothing Then
                            Me.FilteredRelations.Add(.GetCopy)
                        End If
                    End With
                    liInd += 1
                Next

            Catch ex As Exception
                Throw New Exception("Error initialising Relations for Table.")
            End Try

            liInd = 0
            Try
                For Each lrRelation In Me.IncomingRelations

                    Dim lrEntityRelation = CType(Me.IncomingRelations(liInd), Relation)
                    With lrEntityRelation
                        Dim lsReferencedTableName = lrEntityRelation.GetAttributeValue("referencedtablename", Nothing, True, False)
                        If Me.FilteredIncomingRelations.Find(Function(x) x.GetAttributeValue("referencedtablename", Nothing, True, False) =
                                                             lsReferencedTableName) Is Nothing Then
                            Me.FilteredIncomingRelations.Add(.GetCopy)
                        End If
                    End With
                    liInd += 1
                Next

            Catch ex As Exception
                Throw New Exception("Error initialising Incoming Relations for Table.")
            End Try

            Try

                liInd = 0
                For Each lrIndex In Me.Indexes

                    Dim lrEntityIndex = CType(Me.Indexes(liInd), Index)
                    With lrEntityIndex
                        Me.FilteredIndexes.Add(.GetCopy)
                    End With
                    liInd += 1
                Next
            Catch ex As Exception
                Throw New Exception("Error initialising Indexes for Table.")
            End Try

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

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_RELATIONS 'Boston specific. Not part of original Metadrone.
                    Return Me.Relations

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_INCOMINGRELATIONS 'Boston specific. Not part of original Metadrone.
                    Return Me.IncomingRelations

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_RELATION 'Boston specific. Not part of original Metadrone.
                    Return Me.FilteredRelations

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_INCOMINGRELATION 'Boston specific. Not part of original Metadrone.
                    Return Me.FilteredIncomingRelations

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_INDEX 'Boston specific. Not part of original Metadrone.
                    Return Me.FilteredIndexes

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