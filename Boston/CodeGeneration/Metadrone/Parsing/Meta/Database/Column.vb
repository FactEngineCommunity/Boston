Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Source
Imports Boston.Tools
Imports Boston.PluginInterface.Sources

Namespace Parser.Meta.Database

    Friend Class Column
        Implements IEntity

        Private mValue As Object = Nothing
        Private SchemaRowVal As SchemaRow = Nothing
        Private Owner As IEntity = Nothing
        Private Connection As IConnection = Nothing

        Private ReplaceAllList As New ReplaceAllList()

        Friend Relations As New List(Of IEntity)
        Friend IncomingRelations As New List(Of IEntity)

        Private FilteredRelations As New List(Of IEntity) 'Boston specific. For stepping through Relations for the Column.

        Private FilteredIncomingRelations As New List(Of IEntity) 'Boston specific. For stepping through Incoming Relations for the Column.

        Private Transforms As Syntax.SourceTransforms = Nothing

        Friend ListCount As Int64 = -1
        Friend ListPos As Int64 = -1

        Public Sub New(ByVal Value As Object,
                       ByVal SchemaRow As SchemaRow,
                       ByVal Owner As IEntity,
                       ByVal Connection As IConnection,
                       ByVal Transforms As Syntax.SourceTransforms)
            Me.Value = Value
            Me.SchemaRowVal = SchemaRow
            Me.Owner = Owner
            Me.Connection = Connection
            Me.Transforms = Transforms

            If SchemaRow Is Nothing Then Exit Sub 'Not used for columns on Indexes            

            For Each lrRelation In SchemaRow.Relation

                Try
                    If lrRelation.OriginColumns.FindAll(Function(x) x.Name = Me.Value).Count > 0 Then
                        Dim liIndex As Integer = lrRelation.OriginColumns.IndexOf(lrRelation.OriginColumns.Find(Function(x) x.Name = Me.Value))
                        'For Each lrDestinationColumn In lrRelation.DestinationColumns '20221202-VM-Was
                        Dim lrDestinationColumn = lrRelation.DestinationColumns(liIndex)
                        Dim lsReferencedTableName As String = ""
                            Dim lsOriginRoleName As String = ""
                            Dim lsDestinationRoleName As String = ""

                            Dim lrOriginColumn As RDS.Column = lrRelation.OriginColumns.Find(Function(x) x.ActiveRole.Id = lrDestinationColumn.ActiveRole.Id) ' SchemaRow.ColumnId)

                            If lrOriginColumn Is Nothing Then Continue For

                            lsOriginRoleName = lrOriginColumn.Role.Name

                            Dim lsDestinationColumnName As String = ""
                            If lrRelation.DestinationColumns.Count = 1 Then
                                lsDestinationColumnName = lrRelation.DestinationColumns(0).Name
                                lsDestinationRoleName = lrRelation.DestinationColumns(0).Role.Name
                            Else
                                'Dim lrDestinationColumn As RDS.Column = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole.Id = lrOriginColumn.ActiveRole.Id)
                                lsDestinationColumnName = lrDestinationColumn.Name
                                lsDestinationRoleName = lrDestinationColumn.Role.Name
                            End If

                            Me.Relations.Add(New Relation(lrRelation.Id,
                                                          Me.Owner.GetAttributeValue("Value", Nothing, False, False),
                                                          Me.Value,
                                                          lrRelation.DestinationTable.Name,
                                                          lsDestinationColumnName,
                                                          lrRelation.OriginColumns.Count,
                                                          lrRelation.ResponsibleFactType.Id,
                                                          lsOriginRoleName,
                                                          lsDestinationRoleName,
                                                          lrRelation.ResponsibleFactType.IsLinkFactType))

                        'Next
                    End If
                Catch ex As Exception
                    Throw New Exception("Error setting Relation In Column.New")
                End Try
            Next

        End Sub

        Public Function GetCopy() As IEntity Implements IEntity.GetCopy

            Dim col As New Column(Me.mValue, Me.SchemaRowVal.GetCopy, Me.Owner, Me.Connection, Me.Transforms)
            For Each r In Me.ReplaceAllList.List
                col.ReplaceAllList.Add(r.OldVal, r.NewVal)
            Next
            col.ListCount = Me.ListCount
            col.ListPos = Me.ListPos
            Return col

        End Function

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

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone.
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


        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone.
        ''' </summary>
        ''' <returns></returns>
        Public Property AllowZeroLength() As Boolean
            Get
                Return Me.SchemaRowVal.AllowZeroLength
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.AllowZeroLength = value
            End Set
        End Property

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone.
        ''' </summary>
        ''' <returns></returns>
        Public Property CheckValue As List(Of String)
            Get
                Return Me.SchemaRowVal.CheckValue
            End Get
            Set(ByVal value As List(Of String))
                Me.SchemaRowVal.CheckValue = value
            End Set
        End Property

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone.
        ''' </summary>
        ''' <returns></returns>
        Public Property HasCheckValue As Boolean
            Get
                Return Me.SchemaRowVal.CheckValue.Count > 0
            End Get
            Set(ByVal value As Boolean)
            End Set
        End Property

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone.
        ''' </summary>
        ''' <returns></returns>
        Public Property ShortDescription() As String
            Get
                Return Me.SchemaRowVal.ShortDescription
            End Get
            Set(ByVal value As String)
                Me.SchemaRowVal.ShortDescription = value
            End Set
        End Property

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone.
        ''' </summary>
        ''' <returns></returns>
        Public Property Predicate() As String
            Get
                Return Me.SchemaRowVal.Predicate
            End Get
            Set(ByVal value As String)
                Me.SchemaRowVal.Predicate = value
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

        Public Property DataType() As String
            Get
                Return Me.SchemaRowVal.Data_Type
            End Get
            Set(ByVal value As String)
                Me.SchemaRowVal.Data_Type = value
            End Set
        End Property

        Public Property IsIdentity() As Boolean
            Get
                Return Me.SchemaRowVal.IsIdentity
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.IsIdentity = value
            End Set
        End Property

        Public Property IsPrimaryKey() As Boolean
            Get
                Return Me.SchemaRowVal.IsPrimaryKey
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.IsPrimaryKey = value
            End Set
        End Property

        Public Property IsForeignKey() As Boolean
            Get
                Return Me.SchemaRowVal.IsForeignKey
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.IsForeignKey = value
            End Set
        End Property

        Public Property Nullable() As Boolean
            Get
                Return Me.SchemaRowVal.Nullable
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.Nullable = value
            End Set
        End Property

        Public Property Length() As Int64
            Get
                Return Me.SchemaRowVal.Length
            End Get
            Set(ByVal value As Int64)
                Me.SchemaRowVal.Length = value
            End Set
        End Property

        Public Property Precision() As Int64
            Get
                Return Me.SchemaRowVal.Precision
            End Get
            Set(ByVal value As Int64)
                Me.SchemaRowVal.Precision = value
            End Set
        End Property

        Public Property Scale() As Int64
            Get
                Return Me.SchemaRowVal.Scale
            End Get
            Set(ByVal value As Int64)
                Me.SchemaRowVal.Scale = value
            End Set
        End Property

        Public Sub SetAttributeValue(ByVal AttribName As String, ByVal value As Object) Implements IEntity.SetAttributeValue
            If AttribName Is Nothing Then AttribName = ""
            If AttribName.Length = 0 Or StrEq(AttribName, VARIABLE_ATTRIBUTE_VALUE) Then
                'set value
                Me.Value = value

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_DATATYPE) Then
                'set provider datatype
                Me.DataType = value.ToString

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISIDENTITY) Then
                'set isidentity
                Me.IsIdentity = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISPRIMARYKEY) Then
                'set isprimarykey
                Me.IsPrimaryKey = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISFOREIGNKEY) Then
                'set isforeignkey
                Me.IsForeignKey = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_NULLABLE) Then
                'set nullable
                Me.Nullable = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATIONS) Then
                'set Relation
                For Each lrIEntityRelation In value
                    Me.Relations.Add(CType(lrIEntityRelation, Relation))
                Next

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INCOMINGRELATIONS) Then
                'set Relation
                For Each lrIEntityRelation In value
                    Me.IncomingRelations.Add(CType(lrIEntityRelation, Relation))
                Next

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATION) Then
                'set Relation
                Me.Relation = CType(value, List(Of RDS.Relation))

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INCOMINGRELATION) Then
                'set Relation
                Me.IncomingRelation = CType(value, List(Of RDS.Relation))

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH) Then
                'set allowZeroLength
                Me.AllowZeroLength = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SHORTDESCRIPTION) Then 'Boston Specific
                'set ShortDescription
                Me.ShortDescription = Conv.ToString(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PREDICATE) Then 'Boston Specific
                'set Predicate
                Me.Predicate = Conv.ToString(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_CHECKVALUE) Then 'Boston Specific
                'set Predicate
                Me.CheckValue = value

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LENGTH) Then
                'set length
                Me.Length = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PRECISION) Then
                'set precision
                Me.Precision = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SCALE) Then
                'set scale
                Me.Scale = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTCOUNT) Then
                'set listcount
                Me.ListCount = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTPOS) Then
                'set listpos
                Me.ListPos = Conv.ToInteger(value)

            ElseIf StrEq(AttribName, VARIABLE_METHOD_REPLACE) Then
                Throw New Exception("""" & VARIABLE_METHOD_REPLACE & """ Is a method And cannot be a target Of an assignment.")

            ElseIf StrIdxOf(AttribName, FUNC_IGNORE) = 0 Then
                Throw New Exception("""" & FUNC_IGNORE & """ Is a method And cannot be a target Of an assignment.")

            ElseIf StrIdxOf(AttribName, FUNC_USEONLY) = 0 Then
                Throw New Exception("""" & FUNC_USEONLY & """ Is a method And cannot be a target Of an assignment.")

            Else
                Throw New Exception("Syntax Error.")

            End If
        End Sub

        Public Function GetAttributeValue(ByVal AttribName As String, ByVal Params As List(Of Object),
                                          ByVal LookTransformsIfNotFound As Boolean, ByRef ExitBlock As Boolean) As Object Implements IEntity.GetAttributeValue

            If AttribName Is Nothing Then AttribName = ""

            Try

                If AttribName.Length = 0 Or StrEq(AttribName, VARIABLE_ATTRIBUTE_VALUE) Then
                    'return value
                    If Params IsNot Nothing Then Call Me.CheckParamsForPropertyCall(VARIABLE_ATTRIBUTE_VALUE, Params)
                    Dim loReturnObject As Object
                    'Return Me.ReplaceAllList.ApplyReplaces(Me.Value)  Was. Return if things go pear shape.

                    If Params Is Nothing Then
                        Return Me.mValue
                    Else
                        If AttribName = "" Then 'Fixes bug where if a Transform such as column.value = "Date" then column.value = "[Date]" modifies the standard transform function.
                            Return Me.mValue
                        Else
                            loReturnObject = Me.Transforms.GetAttributeValue(Me, Me.Owner, AttribName)
                        End If

                        Return loReturnObject
                    End If

                End If

                If Params Is Nothing Then Params = New List(Of Object)

                If StrEq(AttribName, VARIABLE_ATTRIBUTE_DATATYPE) And LookTransformsIfNotFound Then
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Try
                        Return Me.Transforms.GetAttributeValue(Me, Me.Owner, AttribName)
                    Catch ex As Exception
                        Return Me.DataType
                    End Try

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SCHEMAROWVAL) Then
                    'return listpos               
                    Return Me.SchemaRowVal

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_OWNER) Then
                    'return listpos               
                    Return Me.Owner

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_DATATYPE) Then
                    'return provider datatype
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.DataType

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SCHEMAROWVAL) Then
                    'return SchemaRowVal              
                    Return Me.SchemaRowVal

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISIDENTITY) Then
                    'return isidentity
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.IsIdentity

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISPRIMARYKEY) Then
                    'return isprimarykey
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.IsPrimaryKey

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISFOREIGNKEY) Then
                    'return isforeignkey
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.IsForeignKey

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_NULLABLE) Then
                    'return nullable
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.Nullable

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

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH) Then 'Boston specific. Not part of original Metadrone.
                    'return allowZeroLength
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.AllowZeroLength

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SHORTDESCRIPTION) Then 'Boston specific. Not part of original Metadrone.
                    'return allowZeroLength
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.ShortDescription

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PREDICATE) Then 'Boston specific. Not part of original Metadrone.
                    'return Predicate
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.Predicate

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_CHECKVALUE) Then 'Boston specific. Not part of original Metadrone.
                    'return CheckValue
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.CheckValue

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_HASCHECKVALUE) Then 'Boston specific. Not part of original Metadrone.
                    'return HasCheckValue
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.HasCheckValue

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LENGTH) Then
                    'return length
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.Length

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PRECISION) Then
                    'return precision
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.Precision

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SCALE) Then
                    'return scale
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.Scale

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTCOUNT) Then
                    'return listcount
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.ListCount

                ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTPOS) Then
                    'return listpos
                    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                    Return Me.ListPos

                ElseIf StrIdxOf(AttribName, VARIABLE_METHOD_REPLACE) = 0 Then
                    'employ replace method
                    Select Case Params.Count
                        Case 0 : Throw New Exception("Expecting argument OldVal In '" & VARIABLE_METHOD_REPLACE & "'.")
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

                ElseIf StrIdxOf(AttribName, FUNC_IGNORE) = 0 Then
                    'add ignore
                    Select Case Params.Count
                        Case 0 : Throw New Exception("Expecting argument IgnoreValue in '" & FUNC_IGNORE & "'.")
                        Case Is > 1 : Throw New Exception("Too many arguments in '" & FUNC_IGNORE & "'.")
                    End Select
                    If TypeOf Me.Owner Is Table Then
                        CType(Me.Owner, Table).ApplyIgnore(Params(0).ToString)
                    ElseIf TypeOf Me.Owner Is Routine Then
                        CType(Me.Owner, Routine).ApplyColumnIgnore(Params(0).ToString)
                    Else
                        Throw New Exception("Invalid attribute: " & AttribName & ". ")
                    End If
                    ExitBlock = True
                    Return Nothing

                ElseIf StrIdxOf(AttribName, FUNC_USEONLY) = 0 Then
                    'add use only
                    Select Case Params.Count
                        Case 0 : Throw New Exception("Expecting argument UseOnlyValue in '" & FUNC_USEONLY & "'.")
                        Case Is > 1 : Throw New Exception("Too many arguments in '" & FUNC_USEONLY & "'.")
                    End Select
                    If TypeOf Me.Owner Is Table Then
                        CType(Me.Owner, Table).ApplyUseOnly(Params(0).ToString)
                    ElseIf TypeOf Me.Owner Is Routine Then
                        CType(Me.Owner, Routine).ApplyColumnUseOnly(Params(0).ToString)
                    Else
                        Throw New Exception("Invalid attribute: " & AttribName & ". ")
                    End If
                    ExitBlock = True
                    Return Nothing

                Else
                    If LookTransformsIfNotFound Then
                        Return Me.Transforms.GetAttributeValue(Me, Me.Owner, AttribName)
                    Else
                        'This will avoid stack overflow when called from SourceTransforms
                        Throw New Exception("Invalid attribute: " & AttribName & ". ")
                    End If

                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Function

        Private Sub CheckParamsForPropertyCall(ByVal AttribName As String, ByVal Params As List(Of Object))
            If Params.Count > 0 Then Throw New Exception("'" & AttribName & "' is not a method.")
        End Sub

        Public Function GetConnectionString() As String Implements IEntity.GetConnectionString
            Return ""
        End Function

        Public Sub InitEntities() Implements IEntity.InitEntities

            Dim liInd As Integer = 0

            For Each lrRelation In Me.Relations
                Try
                    With CType(Me.Relations(liInd), Relation)
                        Me.FilteredRelations.Add(.GetCopy)
                    End With
                Catch ex As Exception
                    Throw New Parser.Syntax.ExecException("Column.InitEntities: " & ex.Message, 0)
                End Try
                liInd += 1
            Next

            For Each lrRelation In Me.IncomingRelations
                Try
                    With CType(Me.IncomingRelations(liInd), Relation)
                        Me.FilteredIncomingRelations.Add(.GetCopy)
                    End With
                Catch ex As Exception
                    Throw New Parser.Syntax.ExecException("Column.InitEntities: " & ex.Message, 0)
                End Try
                liInd += 1
            Next

        End Sub

        Public Function GetEntities(ByVal Entity As Syntax.SyntaxNode.ExecForEntities) As List(Of IEntity) Implements IEntity.GetEntities

            Select Case Entity
                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_RELATION
                    Return Me.FilteredRelations
                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_INCOMINGRELATION
                    Return Me.FilteredIncomingRelations
                Case Else
                    Return New List(Of IEntity)
            End Select

        End Function

    End Class

End Namespace