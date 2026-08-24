Imports System.Reflection
Imports System.Xml.Serialization
Imports Newtonsoft.Json
Imports System.ComponentModel

Namespace RDS

    ' Custom TypeConverter for DataType property
    Public Class DataTypeConverter
        Inherits TypeConverter

        ' Override GetStandardValues to provide SQLite data types
        Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overrides Function GetStandardValues(context As ITypeDescriptorContext) As StandardValuesCollection
            ' Call the function to get the list of data types
            Dim dataTypeList As List(Of String) = GetDataTypeList(context)

            ' Return the list as standard values collection
            Return New StandardValuesCollection(dataTypeList)
        End Function

        ' Function to get a list of strings based on the class of the object of the member
        Private Function GetDataTypeList(context As ITypeDescriptorContext) As List(Of String)
            ' Implement logic to generate the list of strings based on the class of the object
            Dim dataTypeList As New List(Of String)()

            ' Example logic:
            If context IsNot Nothing AndAlso context.Instance IsNot Nothing Then

                Dim instanceType As Type = context.Instance.GetType()
                Dim myObject = CType(context.Instance, Object).SelectedObject

                ' Determine the class of the object and populate the list accordingly
                If myObject.GetType Is GetType(ERD.Attribute) Then

                    Dim lrModel = myObject.Column.Model.Model
                    Dim liDatabaseType = lrModel.TargetDatabaseType

                    Select Case liDatabaseType
                        Case Is = pcenumDatabaseType.None
                            Return New List(Of String)
                        Case Else
                            Return lrModel.getDatabaseDataTypes(lrModel.TargetDatabaseType)

                    End Select

                End If
            End If

            Return dataTypeList
        End Function

    End Class


    <Serializable()>
    Public Class Column
        Implements IEquatable(Of RDS.Column)
        Implements IDisposable

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public Model As RDS.Model

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public Table As RDS.Table

        <JsonIgnore()>
        <XmlAttribute>
        Public ColumnType As pcenumRDSColumnType = pcenumRDSColumnType.StandardRDSColumn

        <JsonIgnore()>
        <XmlIgnore()>
        Public ReadOnly Property IsInherited As Boolean
            Get
                '20230415-VM-More rules to be added.
                Try
                    If Me.FactType Is Nothing Then Return False
                    If Me.FactType.IsManyTo1BinaryFactType And Me.Role.JoinedORMObject.Id <> Me.Table.Name Then
                        Return True
                    Else
                        Return False
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End Get
        End Property

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public Index As New List(Of RDS.Index)

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public Relation As New List(Of RDS.Relation)

        <XmlAttribute()>
        <JsonProperty(Order:=1)>
        Public Id As String = System.Guid.NewGuid.ToString

        ''' <summary>
        ''' Used in reverse engineering.
        ''' </summary>
        <XmlIgnore>
        <JsonIgnore()>
        Public DatabaseName As String = ""

        <XmlIgnore>
        <JsonIgnore()>
        Private _Name As String = ""
        <XmlAttribute()>
        <JsonProperty(Order:=2)>
        Public Property Name As String
            Get
                Return Me._Name
            End Get
            Set(value As String)
                Me._Name = value
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property ORMFEKL
            Get
                Dim lsDefaultORMFEKL As String
                Dim lsWrittenAsFEKL As String = " WRITTEN AS " & Me.ORMDataType
                lsDefaultORMFEKL = Me.Table.Name & " has " & If(Me.IsMandatory, "ONE", "AT MOST ONE") & " " & Me.Name
                Select Case Me.getMetamodelDataType
                    Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                          pcenumORMDataType.NumericDecimal,
                                          pcenumORMDataType.NumericMoney
                        lsWrittenAsFEKL &= "(" & If(Me.DataTypePrecision <> 0, "," & Me.DataTypePrecision, "") & ")"
                    Case Is = pcenumORMDataType.RawDataFixedLength,
                                      pcenumORMDataType.RawDataLargeLength,
                                      pcenumORMDataType.RawDataVariableLength,
                                      pcenumORMDataType.TextFixedLength,
                                      pcenumORMDataType.TextLargeLength,
                                      pcenumORMDataType.TextVariableLength
                        lsWrittenAsFEKL &= "(" & Me.DataTypeLength & ")"
                    Case Else
                        'Do nothing
                End Select

                lsDefaultORMFEKL &= lsWrittenAsFEKL

                Try
                    If Me.FactType IsNot Nothing AndAlso Me.FactType.FactTypeReading.Count = 0 Then
                        Return lsDefaultORMFEKL
                    Else
                        Dim lrFactTypeReading = From FactTypeReading In Me.FactType.FactTypeReading
                                                From PredicatePart In FactTypeReading.PredicatePart
                                                Where PredicatePart.Role.JoinedORMObject IsNot Nothing
                                                Where PredicatePart.Role.JoinedORMObject.Id = Me.Table.Name
                                                Select FactTypeReading

                        If lrFactTypeReading.Count = 0 Then
                            Return lsDefaultORMFEKL
                        Else
                            Return lrFactTypeReading.First.GetReadingText(True) & lsWrittenAsFEKL
                        End If
                    End If

                Catch ex As Exception
                    Return lsDefaultORMFEKL
                End Try

            End Get

        End Property

        <XmlAttribute>
        <JsonProperty(Order:=6)>
        Public Property ORMDataType As String
            Get
                Return Me.getMetamodelDataType.ToString
            End Get
            Set(value As String)
                Throw New NotImplementedException("Not Implemented. Please call Column.getMetamodelDataType")
            End Set
        End Property

        <XmlAttribute()>
        <JsonProperty(Order:=7)>
        Public Property DataTypeLength As Integer
            Get
                Return Me.getMetamodelDataTypeLength
            End Get
            Set(value As Integer)
                Throw New NotImplementedException("Not implemented. Call Column.getMetamodelDataTypeLength") 'Required for XML Serialisation.
            End Set
        End Property

        <XmlAttribute()>
        <JsonProperty(Order:=8)>
        Public Property DataTypePrecision As Integer
            Get
                Me.getMetamodelDataTypePrecision()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException("Not implemented. Call Column.getMetamodelDataTypePrecision") 'Required for XML Serialisation.
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Private _DBName As String = Nothing

        <XmlAttribute()>
        <JsonIgnore()>
        Public Property DBName As String
            Get
                Try
                    If Me.isForeignKey Then
                        If Me._DBName Is Nothing Then
                            Return Me.Name
                        Else
                            If Me._DBName Is Nothing Or Me._DBName.Trim = "" Then
                                Dim lsDBName = Me.ActiveRole.JoinedORMObject.DBName
                                If lsDBName Is Nothing Or lsDBName = "" Then
                                    lsDBName = Me.Name
                                End If
                                Return lsDBName
                            Else
                                Return Me._DBName
                            End If
                        End If
                    Else
                        If Me._DBName Is Nothing Or (Me._DBName IsNot Nothing AndAlso Me._DBName.Trim = "") Then
                            'CodeSafe
                            If Me.ActiveRole Is Nothing Then Return Me.Name
                            Dim lsDBName = Me.ActiveRole.JoinedORMObject.DBName
                            If lsDBName Is Nothing Or lsDBName = "" Then
                                lsDBName = Me.Name
                            End If
                            Return lsDBName
                        Else
                            Return Me._DBName
                        End If
                    End If
                Catch ex As Exception
                    Return Me.Name
                End Try
            End Get
            Set(value As String)
                Me._DBName = value
            End Set
        End Property


        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public _DBDataType As String = "STRING"

        ''' <summary>
        ''' Gets the Value Type's DBDataType, else _DBDataType, Default "STRING"
        ''' </summary>
        ''' <returns></returns>
        <JsonIgnore()>
        <XmlAttribute()>
        <CategoryAttribute("DB Level"),
        Browsable(True),
        [ReadOnly](False),
        BindableAttribute(True),
        DesignOnly(False),
        DefaultValueAttribute(GetType(String), ""),
        DescriptionAttribute("The Data Type in the underlying target database.")>
        Public Property DBDataType As String
            Get
                Try
                    'CodeSafe
                    If Me.ActiveRole.JoinsValueType Is Nothing Then Return "<Error>"
                    Return Me.ActiveRole.JoinsValueType.DBDataType
                Catch ex As Exception
                    Return _DBDataType
                End Try
            End Get
            Set(value As String)
                Throw New NotImplementedException("Not Implemented: Setter for Column.DBDataType.")  'Required for XML Serialisation.
            End Set
        End Property

        ''' <summary>
        ''' As when "SELECT Horse.Id AS Horse_Id"
        ''' </summary>
        <XmlIgnore()>
        <JsonIgnore()>
        Public AsName As String = Nothing

        <XmlAttribute()>
        <JsonProperty(Order:=5)>
        Public OrdinalPosition As Integer = 1

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public DataType As RDS.DataType 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public DataTypeName As String

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public ODBCDataType As Integer = 0

        <XmlAttribute()>
        <JsonIgnore()>
        Public Nullable As Boolean = False

        <XmlIgnore()>
        <JsonIgnore()>
        Private _IsMandatory As Boolean = False

        <XmlAttribute>
        <JsonProperty(Order:=3)>
        Public Property IsMandatory As Boolean
            Get
                If Me._IsMandatory Then Return True

                If Me.Role IsNot Nothing Then
                    Return Me.Role.Mandatory
                Else
                    Return Me._IsMandatory
                End If
            End Get
            Set(value As Boolean)
                Me._IsMandatory = value
            End Set
        End Property

        <XmlAttribute()>
        <JsonProperty(Order:=4)>
        Public IsNullable As Boolean = False

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public TableCategory As String 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public TableSchema As String 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public ColumnSize As Integer 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public BufferLength As Integer 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public DecimalDigits As Integer 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public NumPrecRadix As Integer 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public Remarks As String = "" 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public ColumnDef As String = "" 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public SQLDataType As Integer = 0 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public SQLDateTimeSub As String = "" 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public CharOctetLength As Integer = 0 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public SSDataType As Integer = 0 'ODBC

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Public IsDerivationParameter As Boolean = False 'Used by FactEngine for Derived Fact Types. For when user passes a value for a Column of a Derived Fact Type Table.

        ''' <summary>
        ''' Is the Column from the Supertype Table that is represented by this Column, if this Column is inherited from a Supertype table.
        ''' </summary>
        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Private WithEvents _SupertypeColumn As RDS.Column = Nothing
        <XmlIgnore>
        <JsonIgnore()>
        Public Property SupertypeColumn As RDS.Column
            Get
                Return Me._SupertypeColumn
            End Get
            Set(value As RDS.Column)
                Me._SupertypeColumn = value
            End Set
        End Property


        '<XmlAttribute()> _
        'Public ContributesToPrimaryKey As Boolean = False

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Private _FactType As FBM.FactType = Nothing
        'CrossLayer Members
        ''' <summary>
        ''' The FactType (usually Binary) that is responsible for this Column within the Model from which this 
        '''   Column is derived.
        ''' </summary>
        ''' <remarks></remarks>        
        <XmlIgnore()>
        <JsonIgnore()>
        Public Property FactType As FBM.FactType
            Get
                If Me._FactType Is Nothing Then
                    If Me.ActiveRole IsNot Nothing _
                        AndAlso Me.ActiveRole.FactType.Arity = 2 _
                        AndAlso Me.Role.FactType.Arity = 2 _
                        AndAlso Me.ActiveRole.FactType.Id = Me.Role.FactType.Id Then
                        Return Me.Role.FactType
                    Else
                        Return Nothing
                    End If
                Else
                    Return Me._FactType
                End If
            End Get
            Set(value As FBM.FactType)
                Me._FactType = value
            End Set
        End Property

        ''' <summary>
        ''' The Role that was responsible for the derivation of the Column.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        <NonSerialized()>
        Private WithEvents _Role As FBM.Role
        <XmlIgnore()>
        <JsonIgnore()>
        Public Property Role As FBM.Role
            Get
                Return Me._Role
            End Get
            Set(value As FBM.Role)
                Me._Role = value
            End Set
        End Property

        ''' <summary>
        ''' The ultimate Role that resulted in the Column. Where ObjectifiedFactTypes are refererced by other Roles, the first Role is the responsible Role...the nested/absorbed Role is the ActiveRole
        '''   e.g. See the TimetableBookings Page of the University Model.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Private WithEvents _ActiveRole As FBM.Role
        <XmlIgnore()>
        <JsonIgnore()>
        Public Property ActiveRole As FBM.Role
            Get
                Return Me._ActiveRole
            End Get
            Set(value As FBM.Role)
                Me._ActiveRole = value
            End Set
        End Property

        ''' <summary>
        ''' Not only makes life easier. But is necessary for ValueTypes that are Independent.
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        <JsonIgnore>
        Public ReadOnly Property JoinedORMObject As FBM.ModelObject
            Get
                If Me.Role Is Nothing And Me.ActiveRole Is Nothing Then
                    Return Nothing 'Should be Me._JoinedORMObject.Id
                ElseIf Me.ActiveRole IsNot Nothing Then
                    Return Me.ActiveRole.JoinedORMObject
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <XmlIgnore>
        <JsonIgnore>
        Public ReadOnly Property JoinedORMObjectId As String
            Get
                If Me.Role Is Nothing And Me.ActiveRole Is Nothing Then
                    Return Me.JoinedORMObject.Id
                ElseIf Me.ActiveRole IsNot Nothing Then
                    Return Me.ActiveRole.JoinedORMObject.Id
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Public ReadOnly Property DataTypeIsNumeric As Boolean
            Get
                Select Case Me.getMetamodelDataType
                    Case Is = pcenumORMDataType.NumericAutoCounter,
                              pcenumORMDataType.NumericDecimal,
                              pcenumORMDataType.NumericFloatCustomPrecision,
                              pcenumORMDataType.NumericFloatDoublePrecision,
                              pcenumORMDataType.NumericFloatSinglePrecision,
                              pcenumORMDataType.NumericMoney,
                              pcenumORMDataType.NumericSignedBigInteger,
                              pcenumORMDataType.NumericSignedInteger,
                              pcenumORMDataType.NumericSignedSmallInteger,
                              pcenumORMDataType.NumericUnsignedBigInteger,
                              pcenumORMDataType.NumericUnsignedInteger,
                              pcenumORMDataType.NumericUnsignedSmallInteger,
                              pcenumORMDataType.NumericUnsignedTinyInteger

                        Return True

                    Case Else
                        Return False

                End Select
            End Get
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Public ReadOnly Property DataTypeIsText As Boolean
            Get
                Select Case Me.getMetamodelDataType
                    Case Is = pcenumORMDataType.TextFixedLength,
                              pcenumORMDataType.TextLargeLength,
                              pcenumORMDataType.TextVariableLength

                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Public ReadOnly Property DataTypeIsTextOrDate As Boolean
            Get
                Select Case Me.getMetamodelDataType
                    Case Is = pcenumORMDataType.TextFixedLength,
                              pcenumORMDataType.TextLargeLength,
                              pcenumORMDataType.TextVariableLength,
                              pcenumORMDataType.TemporalDate,
                              pcenumORMDataType.TemporalDateAndTime,
                              pcenumORMDataType.TemporalTime

                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property

        ''' <summary>
        ''' Not currently used. Designed to return the string to create the Column in the database.
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        <JsonIgnore()>
        Public ReadOnly Property DBCreateString() As String
            Get
                Try
                    Dim lsCreateString As String

                    lsCreateString = Me.getMetamodelDataType.ToString
                    If Me.ActiveRole.JoinsValueType.DataTypeLength > 0 Then
                        lsCreateString &= "(" & Me.ActiveRole.JoinsValueType.DataTypeLength
                        If Me.ActiveRole.JoinsValueType.DataTypePrecision > 0 Then
                            lsCreateString &= "," & Me.ActiveRole.JoinsValueType.DataTypePrecision
                        End If
                        lsCreateString &= ")"
                    End If

                    Return lsCreateString
                Catch ex As Exception
                    Return "Error"
                End Try
            End Get
        End Property

        <JsonIgnore()>
        <XmlIgnore>
        Public ReadOnly Property OutgoingRelation As List(Of RDS.Relation)
            Get
                Dim larRelation = From Relation In Me.Relation
                                  Where Relation.OriginTable.Name Is Me.Role.getCorrespondingRDSTable.Name
                                  Select Relation

                Return larRelation.ToList
            End Get
        End Property

        <JsonIgnore()>
        <XmlIgnore>
        Public ReadOnly Property IncomingRelation As List(Of RDS.Relation)
            Get
                Dim larRelation = (From Relation In Me.Relation
                                   Where Relation.OriginTable IsNot Me.Role.getCorrespondingRDSTable
                                   Select Relation).ToList

                Dim larGlobalRelation = From Relation In Me.Model.Relation
                                        Where Relation.DestinationColumns.Contains(Me)
                                        Select Relation

                For Each lrRelation In larGlobalRelation
                    larRelation.AddUnique(lrRelation)
                Next

                Return larRelation
            End Get
        End Property


#Region "FactEngine secific"
        ''' <summary>
        ''' Used when creating SQL etc for FactEngine. When the set of Projection Columns is returned, this Alias is set so that ProjectionColumns refer to the correct Table in the From clause etc.
        ''' </summary>
        <XmlIgnore>
        <NonSerialized>
        Public TemporaryAlias As String = Nothing
        <XmlIgnore>
        <NonSerialized>
        Public TemporaryData As String = Nothing 'E.g. As in 'Peter' for a Node with a PK Column of FirstName and where the Node has FirstName and LastName (at least) Columns
        <XmlIgnore>
        <NonSerialized>
        Public IsPartOfUniqueIdentifier As Boolean = False 'FactEngine specific. True if Column is part of unique Identifier.
        <XmlIgnore>
        <NonSerialized>
        Public QueryEdge As FactEngine.QueryEdge 'The Edge that resulted in the Column, if is not the HeadNode of the Nodes of the QueryGraph
        <XmlIgnore>
        <NonSerialized>
        Public ProjectionOrdinalPosition As Integer = 0 'The ordinal position of the Column in the set of ProjectionColumns such that the color can be set for the corresponding node in the GraphView of the FactEngine form.
        <XmlIgnore>
        <NonSerialized>
        Public NodeModifierFunction As FEQL.pcenumFEQLNodeModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None
        <XmlIgnore>
        <NonSerialized>
        Public IsDistinct As Boolean = False 'True if the Column is to be returned in a DISTINCT Clause in the SQL,Cypher,TypeQL. E.g. In "WHICH Order was placed on WHICH DISTINCT OrderDate.YEAR AND is for (Product:'Steeleye Stout')"
        ''' <summary>
        ''' FactEngine specific. Used to tell the type of Node for each result in the Query.
        ''' </summary>
        <XmlIgnore>
        <NonSerialized>
        Public GraphNodeType As String = ""
#End Region

        <XmlIgnore>
        <JsonIgnore()>
        <NonSerialized>
        Private disposedValue As Boolean

        <NonSerialized>
        Public Event ActiveRoleChanged()
        <NonSerialized>
        Public Event AddedToPrimaryKey()
        <NonSerialized>
        Public Event DataTypeChanged(ByVal aiORMDataType As pcenumORMDataType)
        <NonSerialized>
        Public Event DataTypeLengthChanged(ByVal aiNewDataTypeLength As Integer)
        <NonSerialized>
        Public Event DataTypePrecisionChanged(ByVal aiNewDataTypePrecision As Integer)
        <NonSerialized>
        Public Event DBNameChanged(ByVal asNewDBName As String)
        <NonSerialized>
        Public Event IndexAdded(ByRef arIndex As RDS.Index)
        <NonSerialized>
        Public Event IndexRemoved(ByRef arIndex As RDS.Index)
        <NonSerialized>
        Public Event IsDerivationParameterChanged(ByVal abIsDerivationParameter As Boolean)
        <NonSerialized>
        Public Event IsMandatoryChanged(ByRef abIsMandatory As Boolean)
        <NonSerialized>
        Public Event NameChanged(ByVal asNewName As String)
        <NonSerialized>
        Public Event ContributesToPrimaryKeyChanged(ByVal abContributesToPrimaryKey As Boolean)
        <NonSerialized>
        Public Event forceRefresh()
        <NonSerialized>
        Public Event OrdinalPositionChanged(ByVal aiNewOrdinalPosition As Integer)

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <param name="asName"></param>
        ''' <param name="arResponsibleRole"></param>
        ''' <param name="arActiveRole"></param>
        ''' <param name="abIsMandatory"></param>
        ''' <param name="asColumnId"></param>
        ''' <param name="aiDataType"></param>
        Public Sub New(ByRef arTable As RDS.Table, asName As String,
                       ByRef arResponsibleRole As FBM.Role,
                       ByRef arActiveRole As FBM.Role,
                       Optional ByVal abIsMandatory As Boolean = False,
                       Optional ByVal asColumnId As String = Nothing)

            Me.Model = arTable.Model
            Me.Table = arTable
            Me.Name = asName
            Me.Role = arResponsibleRole
            Me.ActiveRole = arActiveRole
            Me.Nullable = Not abIsMandatory
            Me.IsMandatory = abIsMandatory

            If asColumnId IsNot Nothing Then
                Me.Id = asColumnId
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arOriginTable">Must be populated if arRelation is populated. Specify if you want the cloned Column to be of that Table.</param>
        ''' <param name="arRelation">Populate if cloning for a Relation</param>
        ''' <param name="aiNodeModifierFunction">FactEngine specific. Used for modifying the Column in SELECT/RETURN clauses. E.g. Date(DateTime)</param>
        ''' <returns></returns>
        Public Function Clone(Optional ByRef arOriginTable As RDS.Table = Nothing,
                              Optional ByRef arRelation As RDS.Relation = Nothing,
                              Optional ByVal abSetSupertypeColumnAsMe As Boolean = False,
                              Optional ByVal abCreateNewId As Boolean = False,
                              Optional ByVal aiNodeModifierFunction As FEQL.pcenumFEQLNodeModifierFunction = FEQL.pcenumFEQLNodeModifierFunction.None,
                              Optional ByVal abCloneRelation As Boolean = False) As RDS.Column

            Dim lrColumn As New RDS.Column

            With Me
                If arOriginTable IsNot Nothing Then
                    lrColumn.Table = arOriginTable
                Else
                    lrColumn.Table = .Table
                End If
                If abCreateNewId Then
                    lrColumn.Id = System.Guid.NewGuid.ToString
                Else
                    lrColumn.Id = .Id
                End If

                lrColumn.ActiveRole = .ActiveRole
                lrColumn.DataType = .DataType
                lrColumn.FactType = .FactType
                lrColumn.IsMandatory = .IsMandatory
                lrColumn.IsNullable = .IsNullable
                lrColumn.Model = .Model
                lrColumn.Name = .Name
                lrColumn.DBName = .DBName
                lrColumn.Nullable = .Nullable
                lrColumn.OrdinalPosition = .OrdinalPosition
                lrColumn.Relation = New List(Of RDS.Relation)
                '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                'lrColumn.ContributesToPrimaryKey = .ContributesToPrimaryKey
                If arOriginTable Is Nothing Then
                    For Each lrRelation In .Relation
                        If arRelation IsNot Nothing Or abCloneRelation Then
                            If lrColumn.Relation Is Nothing Then lrColumn.Relation = New List(Of RDS.Relation)
                            'Add to that Relation, because most likely calling Clone from Cloning the Relation.
                            '  The cloned Column is added to the reciprocal Relation in Relation.Clone
                            lrColumn.Relation.Add(arRelation) 'The relation, most likely, being cloned.
                        End If
                    Next
                End If
                lrColumn.Role = .Role

                'FactEngine specific
                'Select Case lrColumn.isPartOfPrimaryKey Or lrColumn.IsPartOfUniqueIdentifier
                '    Case Is = True
                lrColumn.GraphNodeType = .GraphNodeType
                '    Case Else
                '        lrColumn.GraphNodeType = .Name
                'End Select

                lrColumn.IsPartOfUniqueIdentifier = .IsPartOfUniqueIdentifier
                lrColumn.QueryEdge = .QueryEdge
                lrColumn.TemporaryAlias = .TemporaryAlias
                lrColumn.ProjectionOrdinalPosition = .ProjectionOrdinalPosition
                If aiNodeModifierFunction <> FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None Then
                    lrColumn.NodeModifierFunction = .NodeModifierFunction
                Else
                    lrColumn.NodeModifierFunction = aiNodeModifierFunction
                End If

                'SupertypeColumn
                If abSetSupertypeColumnAsMe Then
                    lrColumn.SupertypeColumn = Me
                End If

            End With

            Return lrColumn

        End Function

        Public Shadows Function Equals(other As Column) As Boolean Implements IEquatable(Of Column).Equals

            If other.ActiveRole Is Nothing Or Me.ActiveRole Is Nothing Then '202311225-VM-Added Me.ActiveRole Is Nothing as CodeSafe
                Return Me.Id = other.Id
            Else
                Return ((Me.Table.Name = other.Table.Name) And (Me.Name = other.Name)) And (Me.ActiveRole.Id = other.ActiveRole.Id) And (Me.TemporaryAlias = other.TemporaryAlias) And (Me.Role.Id = other.Role.Id) And (Me.NodeModifierFunction = other.NodeModifierFunction)
            End If

        End Function

        Public Function EqualsByRoleActiveRole(other As Column) As Boolean

            If other.ActiveRole Is Nothing Then
                Return Me.Role.Id = other.Role.Id
            Else
                Return (Me.Role.Id = other.Role.Id) And (Me.ActiveRole.Id = other.ActiveRole.Id)
            End If


        End Function


        Public Sub addIndex(ByRef arIndex As RDS.Index)

            Me.Index.AddUnique(arIndex)

            RaiseEvent IndexAdded(arIndex)

        End Sub

        Public Sub triggerForceRefreshEvent()

            RaiseEvent forceRefresh()

        End Sub

        Public Function BelongsToModelElement(ByRef arModelElement As FBM.ModelObject) As Boolean

            Try
                If Me.Role.JoinedORMObject.IsObjectified Then
                    Return Me.Role.FactType Is arModelElement
                Else
                    Return Me.Role.JoinedORMObject Is arModelElement
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        Public Function getAttributeName() As String

            Try
                Dim lsAttributeName As String = ""

                'CodeSafe
                If Me.ActiveRole Is Nothing Then Throw New Exception("Tried to get an AttributeName for a Column with no Active Role")

                Select Case Me.ActiveRole.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        lsAttributeName = Me.ActiveRole.JoinedORMObject.Name
                    Case Is = pcenumConceptType.EntityType
                        Dim lrEntityType As FBM.EntityType
                        lrEntityType = Me.ActiveRole.JoinedORMObject
                        If lrEntityType.HasSimpleReferenceScheme Then
                            lsAttributeName &= lrEntityType.ReferenceModeValueType.Id
                        Else
                            lsAttributeName = Me.ActiveRole.JoinedORMObject.Name
                        End If
                    Case Else
                        lsAttributeName = Me.ActiveRole.JoinedORMObject.Name
                End Select

                lsAttributeName = FEStrings.MakeCapCamelCase(FEStrings.ProperSpace(lsAttributeName))

                Dim larRole = New List(Of FBM.Role)
                larRole.Add(Me.Role)
                larRole.Add(Me.ActiveRole)
                If Me.FactType IsNot Nothing Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                    If lrFactTypeReading IsNot Nothing Then
                        lsAttributeName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsAttributeName
                    End If
                End If
                lsAttributeName = MakeCapCamelCase(lsAttributeName)
                lsAttributeName = Me.Table.createUniqueColumnName(lsAttributeName, Me, 0)

                Return lsAttributeName

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return "Error"
            End Try

        End Function

        Public Function getMetamodelDataType() As pcenumORMDataType

            Try
                If Me.ActiveRole Is Nothing Then Return pcenumORMDataType.DataTypeNotSet

                'Get it using the FactType of the Function.
                If Me.ActiveRole.FactType.IsUnaryFactType Then
                    Return pcenumORMDataType.Boolean
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then 'FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then
                    Return Me.ActiveRole.JoinsValueType.DataType
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    If Me.ActiveRole.JoinsEntityType.HasSimpleReferenceScheme Then
                        Return Me.ActiveRole.JoinsEntityType.getDataType
                    Else
                        Return pcenumORMDataType.DataTypeNotSet 'Throw New Exception("Column references an Entity Type, but the Entity Type does not have a Simple Reference Scheme.")
                    End If
                Else
                    Return pcenumORMDataType.DataTypeNotSet
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return pcenumORMDataType.DataTypeNotSet
            End Try

        End Function

        Public Function getMetamodelValueContraintValues(ByRef arModelObject As FBM.ModelObject) As FEStrings.StringCollection

            Try
                If Me.ActiveRole Is Nothing Then Return New FEStrings.StringCollection()
                'Get it using the FactType of the Function.
                If Me.ActiveRole.FactType.IsUnaryFactType Then
                    Return New FEStrings.StringCollection
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then 'FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then                    
                    arModelObject = Me.ActiveRole.JoinsValueType
                    Return Me.ActiveRole.JoinsValueType.ValueConstraint
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    If Me.ActiveRole.JoinsEntityType.HasSimpleReferenceScheme Then
                        arModelObject = Me.ActiveRole.JoinsEntityType.ReferenceModeValueType
                        Return Me.ActiveRole.JoinsEntityType.ReferenceModeValueType.ValueConstraint
                    Else
                        Return New FEStrings.StringCollection
                    End If
                Else
                    Return New FEStrings.StringCollection
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return New FEStrings.StringCollection()
            End Try

        End Function

        Public Function getReferencedColumn() As RDS.Column

            Try
                If Me.Relation.Count = 0 Then Return Nothing

                If Not Me.isForeignKey Then
                    Return Nothing
                End If

                'CodeSafe
#Region "CodeSafe: Remove Faulty Relations from the Column and raise an Error."
                Dim larFaultyRelation = (From Relation In Me.Relation
                                         Where Not Relation.OriginMultiplicity = pcenumCMMLMultiplicity.Many
                                         Where Not Relation.DestinationMultiplicity = pcenumCMMLMultiplicity.One
                                         Select Relation).ToList

                For Each lrFaultyRelation In larFaultyRelation
                    prApplication.ThrowMessage($"Relation for FactType, {lrFaultyRelation.ResponsibleFactType.Id} ended up on Column, {Me.Name}, in Table, {Me.Table.Name}.", pcenumErrorType.Warning, Nothing, False, False, False, , False, Nothing, False)
                Next
#End Region

                Dim larRelation = (From Relation In Me.Relation
                                   Where Relation.OriginTable Is Me.Table
                                   Select Relation).ToList

                larRelation = larRelation.Except(larFaultyRelation).ToList()

                If larRelation.Count > 0 Then
                    Dim lrDestinationColumn = larRelation(0).DestinationColumns.Find(Function(x) x.ActiveRole.Id = Me.ActiveRole.Id)
                    If larRelation(0).DestinationColumns.Count = 0 Or lrDestinationColumn Is Nothing Then

                        If larRelation(0).DestinationColumns.Count = 1 Then
                            Return larRelation(0).DestinationColumns(0)
                        Else
                            Throw New Exception($"In the Relationship between {larRelation(0).OriginTable.Name} and {larRelation(0).DestinationTable.Name} for the Column, {Me.Name}, in Table, {Me.Table.Name}, there is no Destination Column joining {Me.ActiveRole.JoinedORMObject.Id}")
                        End If

                    Else
                        Return lrDestinationColumn
                    End If

                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                Return Nothing
            End Try


        End Function

        Public Function getMetamodelDataTypeLength() As Integer

            Try
                'CodeSafe
                If Me.ActiveRole Is Nothing Then Return 0

                'Get it using the FactType of the Function.
                If Me.ActiveRole.FactType.IsUnaryFactType Then
                    Return 0
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then 'FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then
                    Return Me.ActiveRole.JoinsValueType.DataTypeLength
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    If Me.ActiveRole.JoinsEntityType.HasSimpleReferenceScheme Then
                        Return Me.ActiveRole.JoinsEntityType.ReferenceModeValueType.DataTypeLength
                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Public Function getMetamodelDataTypePrecision() As Integer

            Try
                'CodeSafe
                If Me.ActiveRole Is Nothing Then Return 0

                'Get it using the FactType of the Function.
                If Me.ActiveRole.FactType.IsUnaryFactType Then
                    Return 0
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then 'FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then
                    Return Me.ActiveRole.JoinsValueType.DataTypePrecision
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    If Me.ActiveRole.JoinsEntityType.HasSimpleReferenceScheme Then
                        Return Me.ActiveRole.JoinsEntityType.ReferenceModeValueType.DataTypePrecision
                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Public Function hasInboundRelation() As Boolean

            Try
                Dim larRelation = From Relation In Me.Model.Relation
                                  Where Relation.DestinationColumns.Any(Function(x) x.Id = Me.Id)
                                  Select Relation

                Return larRelation.Count > 0

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function


        Public Function hasOutboundRelation() As Boolean

            Try

                Return Me.Relation.Find(Function(x) x.OriginTable Is Me.Table) IsNot Nothing

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Public Function hasNonPrimaryKeyColumnsAboveIt() As Boolean

            Dim liNonPrimaryKeyCount = Aggregate Column In Me.Table.Column
                                       Where Column.isPartOfPrimaryKey = False _
                                       And Column.OrdinalPosition < Me.OrdinalPosition
                                       Into Count()

            Return liNonPrimaryKeyCount > 0

        End Function

        ''' <summary>
        ''' The Column is part of a Relation mapping the Column's Table to another Table, and may be one of many Columns in the associated Relation.
        ''' </summary>
        ''' <returns></returns>
        Public Function isForeignKey() As Boolean

            Return Me.Relation.Count > 0

        End Function

        ''' <summary>
        ''' Used to check if the Column is part of the PK Index for the Table. Especially when adding a new Unique Index to a Table,
        '''   such that the ERD.Attribute.PartOfPrimaryKey is not set to False.
        ''' </summary>
        ''' <returns></returns>
        Public Function isPartOfPrimaryKey(Optional ByVal abIgnoreGetRDSTableError As Boolean = False) As Boolean

            Dim lrTable As RDS.Table

            Try
                Try
                    If Me.Role Is Nothing Then
                        'CodeSafe
                        If Me.Table Is Nothing Then
                            Return False
                        Else
                            lrTable = Me.Table
                        End If
                    ElseIf Me.Role.JoinedORMObject.Id = Me.Table.Name Then
                        lrTable = Me.Table
                    ElseIf Me.Role.FactType.Id = Me.Table.Name Then
                        lrTable = Me.Role.FactType.getCorrespondingRDSTable(Nothing, abIgnoreGetRDSTableError)
                    Else
                        'This is required because some Columns may be inherited by Not IsAbsorbed on corresponding ModelElement.
                        If Me.Role.JoinedORMObject.GetType = GetType(FBM.ValueType) Then
                            lrTable = Me.Table
                        ElseIf Me.Table.Name <> Me.Role.JoinedORMObject.Id And Me.Role.FactType.IsObjectified Then
                            lrTable = Me.Role.FactType.getCorrespondingRDSTable
                        Else
                            lrTable = Me.Role.JoinedORMObject.getCorrespondingRDSTable
                        End If

                    End If

                    If Not lrTable.IsPartOfPrimarySubtypeRelationshipPath(Me.Table) Then
                        Return False
                    ElseIf lrTable IsNot Me.Table And Me.Table.FBMModelElement IsNot Nothing AndAlso lrTable IsNot Me.Table.FBMModelElement.GetTopmostNonAbsorbedSupertype.getCorrespondingRDSTable Then
                        Try
                            Return lrTable.Index.Find(Function(x) x.IsPrimaryKey And (x.Column.Find(Function(y) y.Id = Me.Id) IsNot Nothing)) IsNot Nothing
                        Catch ex As Exception
                            Return False
                        End Try
                    Else
                        Return lrTable.Index.Find(Function(x) x.IsPrimaryKey And (x.Column.Find(Function(y) y.Id = Me.Id) IsNot Nothing)) IsNot Nothing
                    End If

                Catch ex1 As Exception
                    Try
                        Return Me.Table.Index.Find(Function(x) x.IsPrimaryKey And (x.Column.Find(Function(y) y.Id = Me.Id) IsNot Nothing)) IsNot Nothing
                    Catch ex2 As Exception
                        Return False
                    End Try
                End Try
                '20200427-VM-was Me.Table.Index.Find(Function(x) x.IsPrimaryKey And (x.Column.Find(Function(y) y.Id = Me.Id) IsNot Nothing)) IsNot Nothing

            Catch ex As Exception
                Return False
            End Try

        End Function

        ''' <summary>
        ''' Simple Attributes are the result of a Binary Many-to-One FactType
        ''' </summary>
        ''' <returns></returns>
        Public Function isSimpleAttribute() As Boolean

            Try
                If Me.FactType Is Nothing Then
                    Return False
                ElseIf Me.FactType.IsManyTo1BinaryFactType Then
                    Return True
                ElseIf Me.FactType.Is1To1BinaryFactType Then
                    Return True
                ElseIf Me.isForeignKey Then
                    Return False
                End If

                Return False

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aiNewOrdinalPosition"></param>
        ''' <param name="aiFromOrdinalPosition"></param>
        ''' <param name="abResetOrdinalPositions">True if you want to reset all the ordinal positions of the Table, to be sure they are synchronous.</param>
        Public Sub moveToOrdinalPosition(ByVal aiNewOrdinalPosition As Integer,
                                         ByVal aiFromOrdinalPosition As Integer,
                                         Optional ByVal abResetOrdinalPositions As Boolean = False)

            Try
                If aiNewOrdinalPosition > Me.Table.Column.Count Then
                    Throw New Exception("Tried to move the Column to an Ordinal Position outside the range of Columns within the Table." & vbCrLf & vbCrLf & "Boston will try and fix this. Try the operation again.")
                End If

                Dim larColumn = From Column In Me.Table.Column
                                Where Column.OrdinalPosition >= Viev.Lesser(aiNewOrdinalPosition, aiFromOrdinalPosition) And
                                      Column.OrdinalPosition <= Viev.Greater(aiNewOrdinalPosition, aiFromOrdinalPosition)
                                Select Column

                For Each lrColumn In larColumn
                    If aiNewOrdinalPosition < aiFromOrdinalPosition Then
                        Call lrColumn.setOrdinalPosition(lrColumn.OrdinalPosition + 1)
                    Else
                        Call lrColumn.setOrdinalPosition(lrColumn.OrdinalPosition - 1)
                    End If
                Next

                Call Me.setOrdinalPosition(aiNewOrdinalPosition)

                If abResetOrdinalPositions Then
                    Me.Table.Column = Me.Table.Column.OrderBy(Function(x) x.OrdinalPosition).ToList
                    Me.Table.resetColumnOrdinalPositions()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Call Me.Table.resetColumnOrdinalPositions()
            End Try

        End Sub

        Public Sub removeIndex(ByRef arIndex As RDS.Index)

            Call Me.Index.RemoveAll(AddressOf arIndex.Equals)

            RaiseEvent IndexRemoved(arIndex)

        End Sub

        Public Sub setActiveRole(ByRef arActiveRole As FBM.Role)

            Me.ActiveRole = arActiveRole

            Call Me.Model.Model.updateORSetCMMLPropertyActiveRole(Me)

            RaiseEvent ActiveRoleChanged()

        End Sub

        Public Sub setRole(ByRef arRole As FBM.Role)

            Me.Role = arRole

            Call Me.Model.Model.updateORSetCMMLPropertyRole(Me)

            RaiseEvent ActiveRoleChanged()

        End Sub

        Public Sub SetDataType(ByVal aiORMDataType As pcenumORMDataType)

            Call Me.ActiveRole.JoinsValueType.SetDataType(aiORMDataType)
            RaiseEvent DataTypeChanged(aiORMDataType)

        End Sub

        Public Sub SetDataTypeLength(ByVal aiNewDataTypeLength As Integer)

            Call Me.ActiveRole.JoinsValueType.SetDataTypeLength(aiNewDataTypeLength)
            RaiseEvent DataTypeLengthChanged(aiNewDataTypeLength)

        End Sub

        Public Sub SetDataTypePrecision(ByVal aiNewDataTypePrecision As Integer)

            Call Me.ActiveRole.JoinsValueType.SetDataTypePrecision(aiNewDataTypePrecision, True)
            RaiseEvent DataTypePrecisionChanged(aiNewDataTypePrecision)

        End Sub

        Public Sub SetDBName(ByVal asNewDBName As String)

            Try
                Me._DBName = asNewDBName
                Call Me.Model.Model.updateCMMLPropertyDBName(Me)

                RaiseEvent DBNameChanged(asNewDBName)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub setIsDerivationParameter(ByVal abIsDerivationParameter As Boolean)

            Try
                'CMML - Do CMML First
                If abIsDerivationParameter And Not Me.IsDerivationParameter Then
                    Call Me.Model.Model.addCMMLIsDerivedFactTypeParameter(Me)
                ElseIf Not abIsDerivationParameter Then
                    Call Me.Model.Model.removeCMMLIsDerivedFactTypeParameter(Me)
                End If

                Me.IsDerivationParameter = abIsDerivationParameter

                RaiseEvent IsDerivationParameterChanged(abIsDerivationParameter)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub triggerContributesToPrimaryKey(ByVal abContributesToPrimaryKey As Boolean)

            'Me.ContributesToPrimaryKey = abContributesToPrimaryKey '20210505-VM-ContributesToPrimaryKey no loger used.
            RaiseEvent ContributesToPrimaryKeyChanged(abContributesToPrimaryKey)

        End Sub

        Public Sub setMandatory(ByVal abIsMandatory As Boolean)

            Me.IsMandatory = abIsMandatory
            Me.IsNullable = Not IsMandatory

            If abIsMandatory Then
                If Not Me.IsMandatory Then 'CodeSafe: Don't want to put duplicates in the CoreIsMandatory FactType/Facts.
                    Call Me.Model.Model.createCMMLAttributeIsMandatory(Me)
                End If
            Else
                Call Me.Model.Model.removeCMMLAttributeIsMandatory(Me)
            End If

            Call Me.Table.TriggerColumnModified(Me)

            RaiseEvent IsMandatoryChanged(abIsMandatory)

            'Database synchronisation
            If Me.Model.Model.IsDatabaseSynchronised Then
                Call Me.Model.Model.connectToDatabase()
                Call Me.Model.Model.DatabaseConnection.columnSetMandatory(Me, abIsMandatory)
            End If

        End Sub

        Public Sub setName(ByVal asNewName As String, Optional ByVal abSetDBNameToNewName As Boolean = False)

            Try
                'For database modification
                Dim lrTempColumn = Me.Clone(Nothing, Nothing)
                lrTempColumn.Name = Me.Name

                Me.Name = asNewName
                Me._DBName = asNewName

                If abSetDBNameToNewName Then
                    Me._DBName = asNewName
                End If

                'CMML
                Call Me.Model.Model.updateCMMLAttributeName(Me)

                RaiseEvent NameChanged(asNewName)

                'RDS-Database
                If Me.Model.Model.IsDatabaseSynchronised And Not Me.Model.Model.TargetDatabaseType = pcenumDatabaseType.None Then

                    If Me.Model.Model.DatabaseConnection Is Nothing Then
                        'Try and establish a connection
                        Call Me.Model.Model.DatabaseManager.establishConnection(Me.Model.Model.TargetDatabaseType, Me.Model.Model.TargetDatabaseConnectionString)
                        If Me.Model.Model.DatabaseConnection Is Nothing Then
                            Throw New Exception("No database connection has been established.")
                        End If
                    ElseIf Me.Model.Model.DatabaseConnection.Connected = False Then
                        Throw New Exception("The database is not connected.")
                    End If


                    Call Me.Model.Model.DatabaseConnection.renameColumn(lrTempColumn, asNewName)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try


        End Sub

        Public Sub setOrdinalPosition(ByRef aiOrdinalPosition As Integer)

            Try
                Me.OrdinalPosition = aiOrdinalPosition

                If Me.Table.isSubtype Then
                    Me.Model.Model.setCMMLPropertyOrdinalPositionForEntity(Me.Id, Me.Table.Name, aiOrdinalPosition)
                Else
                    Call Me.Model.Model.setCMMLAttributeOrdinalPosition(Me.Id, aiOrdinalPosition)
                End If

                RaiseEvent OrdinalPositionChanged(aiOrdinalPosition)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Sets the Table for a Column. Used particularly when changing 'IsAbsorbed' for a ModelObject.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <param name="abModifyCMML">Don't want to modify CMML that is to be removed as part of Absorption.</param>
        Public Sub setTable(ByRef arTable As RDS.Table,
                            Optional ByVal abModifyCMML As Boolean = False)

            Try
                If abModifyCMML Then
                    Call Me.Model.Model.updateCMMLAttributeEntityForColumn(Me, arTable)
                End If

                Me.Table = arTable

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Private Sub SupertypeColumn_OrdinalPositionChanged(aiNewOrdinalPosition As Integer) Handles _SupertypeColumn.OrdinalPositionChanged

            Try
                Me.OrdinalPosition = Me.SupertypeColumn.OrdinalPosition
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub TriggerDataTypeSet(ByVal aiORMDataType As pcenumORMDataType)

            Try
                RaiseEvent DataTypeChanged(aiORMDataType)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Me.Relation.Clear()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null
                disposedValue = True
            End If
        End Sub

        ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
        ' Protected Overrides Sub Finalize()
        '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class

End Namespace
