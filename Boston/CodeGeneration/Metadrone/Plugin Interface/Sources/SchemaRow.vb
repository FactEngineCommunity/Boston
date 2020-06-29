Namespace PluginInterface.Sources

    Public Class SchemaRow
        Private NameField As String
        Private TypeField As String
        Private Column_NameField As String
        Private Data_TypeField As String
        Private Ordinal_PositionField As Int64
        Private LengthField As Int64
        Private PrecisionField As Int64
        Private ScaleField As Int64
        Private NullableField As Boolean
        Private IsIdentityField As Boolean

        Private IsTableField As Boolean
        Private IsViewField As Boolean

        Private IsPrimaryKeyField As Boolean
        Private IsForeignKeyField As Boolean

        Private IsProcedureField As Boolean
        Private IsFunctionField As Boolean
        Private Parameter_NameField As String
        Private Parameter_ModeField As String

        Private IsInMode_Field As Boolean
        Private IsOutMode_Field As Boolean
        Private IsInOutMode_Field As Boolean

        'Added for Boston. Not in original Metadrone code.
        Private AllowZeroLengthField As Boolean


        Public Function GetCopy() As SchemaRow
            Dim schema As New SchemaRow()
            With schema
                .Name = Me.Name
                .Type = Me.Type
                .Column_Name = Me.Column_Name
                .Data_Type = Me.Data_Type
                .Ordinal_Position = Me.Ordinal_Position
                .Length = Me.Length
                .Precision = Me.Precision
                .Scale = Me.Scale
                .Nullable = Me.Nullable
                .IsIdentity = Me.IsIdentity
                .IsTable = Me.IsTable
                .IsView = Me.IsView
                .IsPrimaryKey = Me.IsPrimaryKey
                .IsForeignKey = Me.IsForeignKey
                .IsProcedure = Me.IsProcedure
                .IsFunction = Me.IsFunction
                .Parameter_Name = Me.Parameter_Name
                .Parameter_Mode = Me.Parameter_Mode
                .IsInMode = Me.IsInMode
                .IsOutMode = Me.IsOutMode
                .IsInOutMode = Me.IsInOutMode

                'Test for Boston
                .AllowZeroLength = Me.AllowZeroLength
            End With
            Return schema
        End Function

#Region "Properties"

        Public Property AllowZeroLength() As Boolean
            Get
                Return Me.AllowZeroLengthField
            End Get
            Set(ByVal value As Boolean)
                Me.AllowZeroLengthField = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me.NameField
            End Get
            Set(ByVal value As String)
                Me.NameField = value
            End Set
        End Property

        Public Property Type() As String
            Get
                Return Me.TypeField
            End Get
            Set(ByVal value As String)
                Me.TypeField = value
            End Set
        End Property

        Public Property Column_Name() As String
            Get
                Return Me.Column_NameField
            End Get
            Set(ByVal value As String)
                Me.Column_NameField = value
            End Set
        End Property

        Public Property Data_Type() As String
            Get
                Return Me.Data_TypeField
            End Get
            Set(ByVal value As String)
                Me.Data_TypeField = value
            End Set
        End Property

        Public Property Ordinal_Position() As Int64
            Get
                Return Me.Ordinal_PositionField
            End Get
            Set(ByVal value As Int64)
                Me.Ordinal_PositionField = value
            End Set
        End Property

        Public Property Length() As Int64
            Get
                Return Me.LengthField
            End Get
            Set(ByVal value As Int64)
                Me.LengthField = value
            End Set
        End Property

        Public Property Precision() As Int64
            Get
                Return Me.PrecisionField
            End Get
            Set(ByVal value As Int64)
                Me.PrecisionField = value
            End Set
        End Property

        Public Property Scale() As Int64
            Get
                Return Me.ScaleField
            End Get
            Set(ByVal value As Int64)
                Me.ScaleField = value
            End Set
        End Property

        Public Property Nullable() As Boolean
            Get
                Return Me.NullableField
            End Get
            Set(ByVal value As Boolean)
                Me.NullableField = value
            End Set
        End Property

        Public Property IsIdentity() As Boolean
            Get
                Return Me.IsIdentityField
            End Get
            Set(ByVal value As Boolean)
                Me.IsIdentityField = value
            End Set
        End Property

        Public Property IsTable() As Boolean
            Get
                Return Me.IsTableField
            End Get
            Set(ByVal value As Boolean)
                Me.IsTableField = value
            End Set
        End Property

        Public Property IsView() As Boolean
            Get
                Return Me.IsViewField
            End Get
            Set(ByVal value As Boolean)
                Me.IsViewField = value
            End Set
        End Property

        Public Property IsPrimaryKey() As Boolean
            Get
                Return Me.IsPrimaryKeyField
            End Get
            Set(ByVal value As Boolean)
                Me.IsPrimaryKeyField = value
            End Set
        End Property

        Public Property IsForeignKey() As Boolean
            Get
                Return Me.IsForeignKeyField
            End Get
            Set(ByVal value As Boolean)
                Me.IsForeignKeyField = value
            End Set
        End Property

        Public Property IsProcedure() As Boolean
            Get
                Return Me.IsProcedureField
            End Get
            Set(ByVal value As Boolean)
                Me.IsProcedureField = value
            End Set
        End Property

        Public Property IsFunction() As Boolean
            Get
                Return Me.IsFunctionField
            End Get
            Set(ByVal value As Boolean)
                Me.IsFunctionField = value
            End Set
        End Property

        Public Property Parameter_Name() As String
            Get
                Return Me.Parameter_NameField
            End Get
            Set(ByVal value As String)
                Me.Parameter_NameField = value
            End Set
        End Property

        Public Property Parameter_Mode() As String
            Get
                Return Me.Parameter_ModeField
            End Get
            Set(ByVal value As String)
                Me.Parameter_ModeField = value
            End Set
        End Property

        Public Property IsInMode() As Boolean
            Get
                Return Me.IsInMode_Field
            End Get
            Set(ByVal value As Boolean)
                Me.IsInMode_Field = value
            End Set
        End Property

        Public Property IsOutMode() As Boolean
            Get
                Return Me.IsOutMode_Field
            End Get
            Set(ByVal value As Boolean)
                Me.IsOutMode_Field = value
            End Set
        End Property

        Public Property IsInOutMode() As Boolean
            Get
                Return Me.IsInOutMode_Field
            End Get
            Set(ByVal value As Boolean)
                Me.IsInOutMode_Field = value
            End Set
        End Property

#End Region

    End Class

End Namespace