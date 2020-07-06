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
    ''' Used for RDS.Indexes on Tables.
    ''' </summary>
    Friend Class Index
        Implements IEntity

        Private SchemaRowVal As SchemaRow = Nothing

        Private mValue As Object = Nothing
        Private mId As String
        Private mIsPrimaryKey As Boolean = False

        Friend Columns As New List(Of IEntity)

        Private FilteredColumns As New List(Of IEntity)

        Private mColumnCount As Integer = 0

        Private ReplaceAllList As New ReplaceAllList()

        Private ColumnField As New List(Of RDS.Column)

        Public Sub New()
        End Sub

        Public Sub New(ByVal asId As String,
                       ByRef aarColumn As List(Of RDS.Column),
                       ByVal abIsPrimaryKey As Boolean)

            Me.mId = asId
            Me.ColumnField = aarColumn
            Me.IsPrimaryKey = abIsPrimaryKey
            Me.mColumnCount = Me.ColumnField.Count

        End Sub

        Public Sub SetAttributeValue(AttribName As String, value As Object) Implements IEntity.SetAttributeValue

            If AttribName Is Nothing Then AttribName = ""

            If AttribName.Length = 0 Or StrEq(AttribName, VARIABLE_ATTRIBUTE_VALUE) Then
                'set value
                Me.Value = value

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_COLUMN) Then 'The set of Columns for the Index.
                'set Columns
                For Each lrIEntityIndex In value
                    Me.Columns.Add(CType(lrIEntityIndex, Index))
                Next
            Else
                Throw New Exception("Invalid attribute for Index: '" & AttribName & "'. ")

            End If

        End Sub

        Public Sub InitEntities() Implements IEntity.InitEntities

            Me.FilteredColumns = New List(Of IEntity)

            Dim liInd As Integer = 0

            For Each lrRDSIndex In Me.ColumnField
                Dim lrEntityColumn As Column
                lrEntityColumn = New Column(lrRDSIndex.Name, Me.SchemaRowVal, Me, Nothing, Nothing)
                Me.Columns.Add(lrEntityColumn)
            Next

            For Each lrColumn In Me.Columns

                Dim lrEntityColumn = CType(Me.Columns(liInd), Column)
                With lrEntityColumn
                    Me.FilteredColumns.Add(.GetCopy)
                End With
                liInd += 1
            Next

            'Update column counts
            Me.ColumnCount = Me.FilteredColumns.Count

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

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_ISPRIMARYKEY) And LookTransformsIfNotFound Then
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.IsPrimaryKey

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_COLUMN) And LookTransformsIfNotFound Then
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.Column

            ElseIf StrEq(AttribName, VARIABLE_ATTRIBUTE_COLUMNCOUNT) And LookTransformsIfNotFound Then
                Call Me.CheckParamsForPropertyCall(AttribName, Params)
                Return Me.ColumnCount
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

            'Check predicate against type of entity, and return appropriate collection
            Select Case Entity

                Case Syntax.SyntaxNode.ExecForEntities.OBJECT_COLUMN
                    Return Me.FilteredColumns

                Case Else
                    Throw New Exception("Invalid entity: '" & Entity.ToString & "'. Try recompiling.")

            End Select

        End Function

        Public Function GetCopy() As IEntity Implements IEntity.GetCopy

            Dim lrIndex As New Index()

            With Me
                lrIndex.Id = .Id
                lrIndex.IsPrimaryKey = .IsPrimaryKey
                lrIndex.ColumnField = .ColumnField
                lrIndex.Column = .Column
                lrIndex.ColumnCount = .mColumnCount
            End With

            Return lrIndex

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

        Public Property IsPrimaryKey() As Boolean
            Get
                Return Me.mIsPrimaryKey
            End Get
            Set(ByVal value As Boolean)
                Me.mIsPrimaryKey = value
            End Set
        End Property

        Public Property ColumnCount() As Integer
            Get
                Return Me.mColumnCount
            End Get
            Set(ByVal value As Integer)
                Me.mColumnCount = value
            End Set
        End Property

        ''' <summary>
        ''' Boston specific. Not originally part of Metadrone. The set of Indexes associated with the Table.
        ''' </summary>
        ''' <returns></returns>
        Public Property Column() As List(Of RDS.Column) 'Might need to be IEntity
            Get
                Return Me.ColumnField
            End Get
            Set(ByVal value As List(Of RDS.Column))
                Me.ColumnField = value
            End Set
        End Property


    End Class

End Namespace
