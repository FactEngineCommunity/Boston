Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Source
Imports Boston.Tools
Imports Boston.PluginInterface.Sources

Namespace Parser.Meta.Database

    Friend Class Parameter
        Implements IEntity

        Private mValue As Object = Nothing
        Private SchemaRowVal As SchemaRow = Nothing
        Private Owner As IEntity = Nothing
        Private Connection As IConnection = Nothing

        Private ReplaceAllList As New ReplaceAllList()

        Private Transforms As Syntax.SourceTransforms = Nothing

        Friend ListCount As Int64 = -1
        Friend ListPos As Int64 = -1

        Public Sub New(ByVal Value As Object, ByVal SchemaRow As SchemaRow,
                       ByVal Owner As IEntity, ByVal Connection As IConnection,
                       ByVal Transforms As Syntax.SourceTransforms)
            Me.Value = Value
            Me.SchemaRowVal = SchemaRow
            Me.Owner = Owner
            Me.Connection = Connection
            Me.Transforms = Transforms
        End Sub

        Public Function GetCopy() As IEntity Implements IEntity.GetCopy
            Dim parm As New Parameter(Me.mValue, Me.SchemaRowVal.GetCopy, Me.Owner, Me.Connection, Me.Transforms)
            For Each r In Me.ReplaceAllList.List
                parm.ReplaceAllList.Add(r.OldVal, r.NewVal)
            Next
            parm.ListCount = Me.ListCount
            parm.ListPos = Me.ListPos
            Return parm
        End Function

        Public Property Relation As List(Of RDS.Relation) 'Boston specific. Not part of original Metadrone.
            Get
                Return Me.SchemaRowVal.Relation
            End Get
            Set(value As List(Of RDS.Relation))
                Me.SchemaRowVal.Relation = value
            End Set
        End Property

        Public Property IncomingRelation As List(Of RDS.Relation) 'Boston specific. Not part of original Metadrone.
            Get
                Return Me.SchemaRowVal.IncomingRelation
            End Get
            Set(value As List(Of RDS.Relation))
                Me.SchemaRowVal.IncomingRelation = value
            End Set
        End Property

        Public Property AllowZeroLength() As Boolean 'Boston specific. Not part of original Metadrone.
            Get
                Return Me.SchemaRowVal.AllowZeroLength
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.AllowZeroLength = value
            End Set
        End Property

        Public Property ShortDescription() As String 'Boston specific. Not part of original Metadrone. The ShortDescription of the JoinedORMObject for the ActiveRole of the Column
            Get
                Return Me.SchemaRowVal.ShortDescription
            End Get
            Set(ByVal value As String)
                Me.SchemaRowVal.ShortDescription = value
            End Set
        End Property

        Public Property Predicate() As String 'Boston specific. Not part of original Metadrone. A predicate for the Column, from the FactType for the Column and as per ActiveRole/Role of the Column.
            Get
                Return Me.SchemaRowVal.Predicate
            End Get
            Set(ByVal value As String)
                Me.SchemaRowVal.Predicate = value
            End Set
        End Property

        Public Property CheckValue() As List(Of String) 'Boston specific. Not part of original Metadrone. A predicate for the Column, from the FactType for the Column and as per ActiveRole/Role of the Column.
            Get
                Return Me.SchemaRowVal.CheckValue
            End Get
            Set(ByVal value As List(Of String))
                Me.SchemaRowVal.CheckValue = value
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

        Public Property Mode() As String
            Get
                Return Me.SchemaRowVal.Parameter_Mode
            End Get
            Set(ByVal value As String)
                Me.SchemaRowVal.Parameter_Mode = value
            End Set
        End Property

        Public Property IsInMode() As Boolean
            Get
                Return Me.SchemaRowVal.IsInMode
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.IsInMode = value
            End Set
        End Property

        Public Property IsOutMode() As Boolean
            Get
                Return Me.SchemaRowVal.IsOutMode
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.IsOutMode = value
            End Set
        End Property

        Public Property IsInOutMode() As Boolean
            Get
                Return Me.SchemaRowVal.IsInOutMode
            End Get
            Set(ByVal value As Boolean)
                Me.SchemaRowVal.IsInOutMode = value
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

        Public Property Relations As List(Of IEntity) Implements IEntity.Relations
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As List(Of IEntity))
                Throw New NotImplementedException()
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

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_MODE) Then
                'set mode
                Me.Mode = value.ToString

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISINMODE) Then
                'set in mode
                Me.IsInMode = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISOUTMODE) Then
                'set out mode
                Me.IsOutMode = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISINOUTMODE) Then
                'set in/out mode
                Me.IsInOutMode = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATION) Then 'Boston specific. Not part of original Metadrone.
                'set Relation
                Me.Relation = CType(value, List(Of RDS.Relation))

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INCOMINGRELATION) Then 'Boston specific. Not part of original Metadrone.
                'set Relation
                Me.IncomingRelation = CType(value, List(Of RDS.Relation))

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH) Then 'Boston specific. Not part of original Metadrone.
                'set allowZeroLength
                Me.AllowZeroLength = Conv.ToBoolean(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SHORTDESCRIPTION) Then 'Boston specific. Not part of original Metadrone.
                'set allowZeroLength
                Me.ShortDescription = Conv.ToString(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_PREDICATE) Then 'Boston specific. Not part of original Metadrone.
                'set Predicate
                Me.Predicate = Conv.ToString(value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_CHECKVALUE) Then 'Boston specific. Not part of original Metadrone.
                'set CheckValue
                Me.Predicate = value

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
                Throw New Exception("""" & VARIABLE_METHOD_REPLACE & """ is a method and cannot be a target of an assignment.")

            ElseIf StrIdxOf(AttribName, FUNC_IGNORE) = 0 Then
                Throw New Exception("""" & FUNC_IGNORE & """ is a method and cannot be a target of an assignment.")

            ElseIf StrIdxOf(AttribName, FUNC_USEONLY) = 0 Then
                Throw New Exception("""" & FUNC_USEONLY & """ is a method and cannot be a target of an assignment.")

            Else
                Throw New Exception("Syntax error.")

            End If
        End Sub

        Public Function GetAttributeValue(ByVal AttribName As String, ByVal Params As List(Of Object),
                                          ByVal LookTransformsIfNotFound As Boolean, ByRef ExitBlock As Boolean) As Object Implements IEntity.GetAttributeValue
            If Params Is Nothing Then Params = New List(Of Object)
            If AttribName Is Nothing Then AttribName = ""
            If AttribName.Length = 0 Or StrEq(AttribName, VARIABLE_ATTRIBUTE_VALUE) Then
                'return value
                Call Me.CheckParamsForPropertyCall(VARIABLE_ATTRIBUTE_VALUE, Params)
                Return Me.ReplaceAllList.ApplyReplaces(Me.Value)

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_DATATYPE) Then
                'return provider datatype
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.DataType

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_MODE) Then
                'return mode
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.Mode

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISINMODE) Then
                'return in mode
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.IsInMode

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISOUTMODE) Then
                'return out mode
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.IsOutMode

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISINOUTMODE) Then
                'return in/out mode
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.IsInOutMode

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_RELATION) Then 'Boston specific. Not part of original Metadrone.
                'return Relation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.Relation

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_INCOMINGRELATION) Then 'Boston specific. Not part of original Metadrone.
                'return Relation
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.IncomingRelation

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH) Then 'Boston specific. Not part of original Metadrone.
                'return allowZeroLength
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.AllowZeroLength

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_SHORTDESCRIPTION) Then 'Boston specific. Not part of original Metadrone.
                'return ShortDescription
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

            ElseIf StrIdxOf(AttribName, FUNC_IGNORE) = 0 Then
                'add ignore
                Select Case Params.Count
                    Case 0 : Throw New Exception("Expecting argument IgnoreValue in '" & FUNC_IGNORE & "'.")
                    Case Is > 1 : Throw New Exception("Too many arguments in '" & FUNC_IGNORE & "'.")
                End Select
                If TypeOf Me.Owner Is Table Then
                    CType(Me.Owner, Table).ApplyIgnore(Params(0).ToString)
                ElseIf TypeOf Me.Owner Is Routine Then
                    CType(Me.Owner, Routine).ApplyParamIgnore(Params(0).ToString)
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
                    CType(Me.Owner, Routine).ApplyParamUseOnly(Params(0).ToString)
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
        End Function

        Private Sub CheckParamsForPropertyCall(ByVal AttribName As String, ByVal Params As List(Of Object))
            If Params.Count > 0 Then Throw New Exception("'" & AttribName & "' is not a method.")
        End Sub

        Public Function GetConnectionString() As String Implements IEntity.GetConnectionString
            Return ""
        End Function

        Public Sub InitEntities() Implements IEntity.InitEntities

        End Sub

        Public Function GetEntities(ByVal Entity As Syntax.SyntaxNode.ExecForEntities) As List(Of IEntity) Implements IEntity.GetEntities
            Return New List(Of IEntity)
        End Function

    End Class

End Namespace