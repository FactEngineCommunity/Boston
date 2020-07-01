Imports Boston.Parser.Syntax
Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Source
Imports Boston.Tools
Imports Boston.PluginInterface.Sources


Namespace Parser.Meta.Database

    ''' <summary>
    ''' Boston specific. Not part of the orginal Metadrone.
    ''' Used for RDS.Relations, such that ForeignKey constraints can be created in SQL/Cypher etc.
    ''' </summary>
    Friend Class Relation
        Implements IEntity

        Private mValue As Object = Nothing
        Private mId As String
        Private mReferencedTableName As String
        Private ReplaceAllList As New ReplaceAllList()

        Public Sub New()
        End Sub

        Public Sub New(ByVal asId As String,
                       ByVal asReferencedTableName As String)
            Me.mId = asId
            Me.mReferencedTableName = asReferencedTableName
        End Sub

        Public Sub SetAttributeValue(AttribName As String, value As Object) Implements IEntity.SetAttributeValue
            Throw New NotImplementedException()
        End Sub

        Public Sub InitEntities() Implements IEntity.InitEntities

        End Sub

        Public Function GetConnectionString() As String Implements IEntity.GetConnectionString
            Return ""
        End Function

        Public Function GetAttributeValue(AttribName As String, Params As List(Of Object), LookTransformsIfNotFound As Boolean, ByRef ExitBlock As Boolean) As Object Implements IEntity.GetAttributeValue

            If Params Is Nothing Then Params = New List(Of Object)

            If AttribName Is Nothing Then AttribName = ""

            If AttribName.Length = 0 Or StrEq(AttribName, VARIABLE_ATTRIBUTE_VALUE) Then
                'return value
                Call Me.CheckParamsForPropertyCall(VARIABLE_ATTRIBUTE_VALUE, Params)
                Return Me.ReplaceAllList.ApplyReplaces(Me.Value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ID) And LookTransformsIfNotFound Then
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.Id

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_REFERENCEDTABLENAME) And LookTransformsIfNotFound Then
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.ReferencedTableName

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISIDENTITY) Then
                '    'return isidentity
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.IsIdentity

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISPRIMARYKEY) Then
                '    'return isprimarykey
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.IsPrimaryKey

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISFOREIGNKEY) Then
                '    'return isforeignkey
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.IsForeignKey

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_NULLABLE) Then
                '    'return nullable
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.Nullable

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATION) Then 'Boston specific. Not part of original Metadrone.
                '    'return relation
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.Relation

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH) Then 'Boston specific. Not part of original Metadrone.
                '    'return allowZeroLength
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.AllowZeroLength

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LENGTH) Then
                '    'return length
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.Length

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PRECISION) Then
                '    'return precision
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.Precision

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SCALE) Then
                '    'return scale
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.Scale

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTCOUNT) Then
                '    'return listcount
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.ListCount

                'ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_LISTPOS) Then
                '    'return listpos
                '    Call Me.CheckParamsForPropertyCall(AttribName, Params)
                '    Return Me.ListPos

                'ElseIf StrIdxOf(AttribName, VARIABLE_METHOD_REPLACE) = 0 Then
                '    'employ replace method
                '    Select Case Params.Count
                '        Case 0 : Throw New Exception("Expecting argument OldVal in '" & VARIABLE_METHOD_REPLACE & "'.")
                '        Case 1 : Throw New Exception("Expecting argument NewVal in '" & VARIABLE_METHOD_REPLACE & "'.")
                '        Case Is > 2 : Throw New Exception("Too many arguments in '" & VARIABLE_METHOD_REPLACE & "'.")
                '    End Select
                '    If PackageBuilder.PreProc.IgnoreCase Then
                '        Return ReplaceInsensitive(Me.ReplaceAllList.ApplyReplaces(Me.Value).ToString, Params(0).ToString, Params(1).ToString)
                '    Else
                '        Return Me.ReplaceAllList.ApplyReplaces(Me.Value).ToString.Replace(Params(0).ToString, Params(1).ToString)
                '    End If

                'ElseIf StrIdxOf(AttribName, FUNC_REPLACEALL) = 0 Then
                '    'add replace all value
                '    Select Case Params.Count
                '        Case 0 : Throw New Exception("Expecting argument OldVal in '" & FUNC_REPLACEALL & "'.")
                '        Case 1 : Throw New Exception("Expecting argument NewVal in '" & FUNC_REPLACEALL & "'.")
                '        Case Is > 2 : Throw New Exception("Too many arguments in '" & FUNC_REPLACEALL & "'.")
                '    End Select
                '    Me.ReplaceAllList.Add(Params(0).ToString, Params(1).ToString)
                '    Return Nothing

                'ElseIf StrIdxOf(AttribName, FUNC_IGNORE) = 0 Then
                '    'add ignore
                '    Select Case Params.Count
                '        Case 0 : Throw New Exception("Expecting argument IgnoreValue in '" & FUNC_IGNORE & "'.")
                '        Case Is > 1 : Throw New Exception("Too many arguments in '" & FUNC_IGNORE & "'.")
                '    End Select
                '    If TypeOf Me.Owner Is Table Then
                '        CType(Me.Owner, Table).ApplyIgnore(Params(0).ToString)
                '    ElseIf TypeOf Me.Owner Is Routine Then
                '        CType(Me.Owner, Routine).ApplyColumnIgnore(Params(0).ToString)
                '    Else
                '        Throw New Exception("Invalid attribute: " & AttribName & ". ")
                '    End If
                '    ExitBlock = True
                '    Return Nothing

                'ElseIf StrIdxOf(AttribName, FUNC_USEONLY) = 0 Then
                '    'add use only
                '    Select Case Params.Count
                '        Case 0 : Throw New Exception("Expecting argument UseOnlyValue in '" & FUNC_USEONLY & "'.")
                '        Case Is > 1 : Throw New Exception("Too many arguments in '" & FUNC_USEONLY & "'.")
                '    End Select
                '    If TypeOf Me.Owner Is Table Then
                '        CType(Me.Owner, Table).ApplyUseOnly(Params(0).ToString)
                '    ElseIf TypeOf Me.Owner Is Routine Then
                '        CType(Me.Owner, Routine).ApplyColumnUseOnly(Params(0).ToString)
                '    Else
                '        Throw New Exception("Invalid attribute: " & AttribName & ". ")
                '    End If
                '    ExitBlock = True
                '    Return Nothing

                'Else
                '    If LookTransformsIfNotFound Then
                '        Return Me.Transforms.GetAttributeValue(Me, Me.Owner, AttribName)
                '    Else
                '        'This will avoid stack overflow when called from SourceTransforms
                '        Throw New Exception("Invalid attribute: " & AttribName & ". ")
                '    End If
            Else
                Return "" 'Boston specific. May not be the best thing to do here.
            End If
        End Function

        ''' <summary>
        ''' Copied from Column
        ''' </summary>
        ''' <param name="AttribName"></param>
        ''' <param name="Params"></param>
        Private Sub CheckParamsForPropertyCall(ByVal AttribName As String, ByVal Params As List(Of Object))
            If Params.Count > 0 Then Throw New Exception("'" & AttribName & "' is not a method.")
        End Sub

        Public Function GetEntities(Entity As Syntax.SyntaxNode.ExecForEntities) As List(Of IEntity) Implements IEntity.GetEntities

            Return New List(Of IEntity)
        End Function

        Public Function GetCopy() As IEntity Implements IEntity.GetCopy

            Dim rel As New Relation()

            With Me
                rel.Id = .Id
                rel.ReferencedTableName = .ReferencedTableName
            End With
            'col.ListCount = Me.ListCount
            'col.ListPos = Me.ListPos
            Return rel

        End Function

        Public Property Value() As Object
            Get
                Return Me.mValue
            End Get
            Set(ByVal value As Object)
                Me.mValue = value
            End Set
        End Property

        Public Property Id() As String
            Get
                Return Me.mId
            End Get
            Set(ByVal value As String)
                Me.mId = value
            End Set
        End Property

        Public Property ReferencedTableName() As String
            Get
                Return Me.mReferencedTableName
            End Get
            Set(ByVal value As String)
                Me.mReferencedTableName = value
            End Set
        End Property

    End Class

End Namespace
