Imports System.Reflection
Imports System.Xml.Serialization

Namespace RDS

    <Serializable()> _
    Public Class Column
        Implements IEquatable(Of RDS.Column)

        <XmlIgnore()> _
        <NonSerialized()> _
        Public Model As RDS.Model

        <XmlIgnore()> _
        <NonSerialized()> _
        Public Table As RDS.Table

        <XmlIgnore()> _
        <NonSerialized()> _
        Public Index As New List(Of RDS.Index)

        Public Relation As New List(Of RDS.Relation)

        <XmlAttribute()>
        Public Id As String = System.Guid.NewGuid.ToString

        ''' <summary>
        ''' Used in reverse engineering.
        ''' </summary>
        <XmlIgnore>
        Public DatabaseName As String = ""

        <XmlAttribute()>
        Public Name As String = ""

        ''' <summary>
        ''' As when "SELECT Horse.Id AS Horse_Id"
        ''' </summary>
        <XmlIgnore()>
        Public AsName As String = Nothing

        <XmlAttribute()> _
        Public OrdinalPosition As Integer = 1

        <XmlElement()> _
        Public DataType As RDS.DataType

        <XmlAttribute()> _
        Public DataTypeName As String

        <XmlAttribute()> _
        Public ODBCDataType As Integer = 0

        <XmlAttribute()> _
        Public Nullable As Boolean = False

        Public IsMandatory As Boolean = False

        <XmlAttribute()> _
        Public IsNullable As Boolean = False

        <XmlAttribute()> _
        Public TableCategory As String

        <XmlAttribute()> _
        Public TableSchema As String

        <XmlAttribute()> _
        Public ColumnSize As Integer

        <XmlAttribute()> _
        Public BufferLength As Integer

        <XmlAttribute()> _
        Public DecimalDigits As Integer

        <XmlAttribute()> _
        Public NumPrecRadix As Integer

        <XmlAttribute()> _
        Public Remarks As String = ""

        <XmlAttribute()> _
        Public ColumnDef As String = ""

        <XmlAttribute()> _
        Public SQLDataType As Integer = 0

        <XmlAttribute()> _
        Public SQLDateTimeSub As String = ""

        <XmlAttribute()> _
        Public CharOctetLength As Integer = 0

        <XmlAttribute()>
        Public SSDataType As Integer = 0

        <XmlAttribute()>
        Public IsDerivationParameter As Boolean = False

        '<XmlAttribute()> _
        'Public ContributesToPrimaryKey As Boolean = False

        'CrossLayer Members
        ''' <summary>
        ''' The FactType (usually Binary) that is responsible for this Column within the Model from which this 
        '''   Column is derived.
        ''' </summary>
        ''' <remarks></remarks>        
        <XmlIgnore()> _
        <NonSerialized()> _
        Public FactType As FBM.FactType

        ''' <summary>
        ''' The Role that was responsible for the derivation of the Column.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        <NonSerialized()> _
        Public WithEvents Role As FBM.Role

        ''' <summary>
        ''' The ultimate Role that resulted in the Column. Where ObjectifiedFactTypes are refererced by other Roles, the first Role is the responsible Role...the nested/absorbed Role is the ActiveRole
        '''   e.g. See the TimetableBookings Page of the University Model.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        <NonSerialized()>
        Public WithEvents ActiveRole As FBM.Role

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

        Public ReadOnly Property DBCreateString() As String
            Get
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
            End Get
        End Property

        Public ReadOnly Property OutgoingRelation As List(Of RDS.Relation)
            Get
                Dim larRelation = From Relation In Me.Relation
                                  Where Relation.OriginTable.Name Is Me.Role.getCorrespondingRDSTable.Name
                                  Select Relation

                Return larRelation.ToList
            End Get
        End Property

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
        Public TemporaryAlias As String = Nothing
        Public TemporaryData As String = Nothing 'E.g. As in 'Peter' for a Node with a PK Column of FirstName and where the Node has FirstName and LastName (at least) Columns
        Public IsPartOfUniqueIdentifier As Boolean = False 'FactEngine specific. True if Column is part of unique Identifier.
        Public QueryEdge As FactEngine.QueryEdge 'The Edge that resulted in the Column, if is not the HeadNode of the Nodes of the QueryGraph
        Public ProjectionOrdinalPosition As Integer = 0 'The ordinal position of the Column in the set of ProjectionColumns such that the color can be set for the corresponding node in the GraphView of the FactEngine form.
#End Region

        ''' <summary>
        ''' FactEngine specific. Used to tell the type of Node for each result in the Query.
        ''' </summary>
        Public GraphNodeType As String = ""

        Public Event ActiveRoleChanged()
        Public Event AddedToPrimaryKey()
        Public Event IndexAdded(ByRef arIndex As RDS.Index)
        Public Event IndexRemoved(ByRef arIndex As RDS.Index)
        Public Event IsDerivationParameterChanged(ByVal abIsDerivationParameter As Boolean)
        Public Event IsMandatoryChanged(ByRef abIsMandatory As Boolean)
        Public Event NameChanged(ByVal asNewName As String)
        Public Event ContributesToPrimaryKeyChanged(ByVal abContributesToPrimaryKey As Boolean)
        Public Event forceRefresh()
        Public Event OrdinalPositionChanged(ByVal aiNewOrdinalPosition As Integer)

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arTable As RDS.Table, asName As String,
                       ByRef arResponsibleRole As FBM.Role,
                       ByRef arActiveRole As FBM.Role,
                       Optional ByVal abIsMandatory As Boolean = False)

            Me.Model = arTable.Model
            Me.Table = arTable
            Me.Name = asName
            Me.Role = arResponsibleRole
            Me.ActiveRole = arActiveRole
            Me.Nullable = Not abIsMandatory
            Me.IsMandatory = abIsMandatory

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arOriginTable">Must be populated if arRelation is populated. Specify if you want the cloned Column to be of that Table.</param>
        ''' <param name="arRelation">Populate if cloning for a Relation</param>
        ''' <returns></returns>
        Public Function Clone(Optional ByRef arOriginTable As RDS.Table = Nothing,
                              Optional ByRef arRelation As RDS.Relation = Nothing) As RDS.Column

            Dim lrColumn As New RDS.Column

            With Me
                If arOriginTable IsNot Nothing Then
                    lrColumn.Table = arOriginTable
                Else
                    lrColumn.Table = .Table
                End If
                lrColumn.Id = .Id
                lrColumn.ActiveRole = .ActiveRole
                lrColumn.DataType = .DataType
                lrColumn.FactType = .FactType
                lrColumn.IsMandatory = .IsMandatory
                lrColumn.IsNullable = .IsNullable
                lrColumn.Model = .Model
                lrColumn.Name = .Name
                lrColumn.Nullable = .Nullable
                lrColumn.OrdinalPosition = .OrdinalPosition
                lrColumn.Relation = New List(Of RDS.Relation)
                '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                'lrColumn.ContributesToPrimaryKey = .ContributesToPrimaryKey
                If arOriginTable Is Nothing Then
                    For Each lrRelation In .Relation
                        If arRelation IsNot Nothing Then
                            If lrColumn.Relation Is Nothing Then lrColumn.Relation = New List(Of RDS.Relation)
                            'Add to that Relation, because most likely calling Clone from Cloning the Relation.
                            '  The cloned Column is added to the reciprocal Relation in Relation.Clone
                            lrColumn.Relation.Add(arRelation) 'The relation, most likely, being cloned.
                        End If
                    Next
                End If
                lrColumn.Role = .Role

                'FactEngine specific
                lrColumn.GraphNodeType = .GraphNodeType
                lrColumn.IsPartOfUniqueIdentifier = .IsPartOfUniqueIdentifier
                lrColumn.QueryEdge = .QueryEdge
                lrColumn.TemporaryAlias = .TemporaryAlias
                lrColumn.ProjectionOrdinalPosition = .ProjectionOrdinalPosition

            End With

            Return lrColumn

        End Function

        Public Shadows Function Equals(other As Column) As Boolean Implements IEquatable(Of Column).Equals

            If other.ActiveRole Is Nothing Then
                Return Me.Id = other.Id
            Else
                Return ((Me.Table.Name = other.Table.Name) And (Me.Name = other.Name)) And (Me.ActiveRole.Id = other.ActiveRole.Id) And (Me.TemporaryAlias = other.TemporaryAlias) And (Me.Role.Id = other.Role.Id)
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

                lsAttributeName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsAttributeName))

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
                lsAttributeName = Me.Table.createUniqueColumnName(Me, lsAttributeName, 0)

                Return lsAttributeName

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function getMetamodelValueContraintValues(ByRef arModelObject As FBM.ModelObject) As StringCollection

            Try
                If Me.ActiveRole Is Nothing Then Return New StringCollection()
                'Get it using the FactType of the Function.
                If Me.ActiveRole.FactType.IsUnaryFactType Then
                    Return New StringCollection
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then 'FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then                    
                    arModelObject = Me.ActiveRole.JoinsValueType
                    Return Me.ActiveRole.JoinsValueType.ValueConstraint
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    If Me.ActiveRole.JoinsEntityType.HasSimpleReferenceScheme Then
                        arModelObject = Me.ActiveRole.JoinsEntityType.ReferenceModeValueType
                        Return Me.ActiveRole.JoinsEntityType.ReferenceModeValueType.ValueConstraint
                    Else
                        Return New StringCollection
                    End If
                Else
                    Return New StringCollection
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New StringCollection()
            End Try

        End Function

        Public Function getReferencedColumn() As RDS.Column

            Try
                If Me.Relation.Count = 0 Then Return Nothing
                Return Me.Relation.Find(Function(x) x.OriginTable Is Me.Table).DestinationColumns.Find(Function(x) x.ActiveRole Is Me.ActiveRole)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                Return Nothing
            End Try


        End Function

        Public Function getMetamodelDataTypeLength() As Integer

            Try
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function getMetamodelDataTypePrecision() As Integer

            Try
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
        Public Function isPartOfPrimaryKey() As Boolean

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
                        lrTable = Me.FactType.getCorrespondingRDSTable
                    Else
                        'This is required because some Columns may be inherited by Not IsAbsorbed on corresponding ModelElement.
                        lrTable = Me.Role.JoinedORMObject.getCorrespondingRDSTable
                    End If

                    If Not lrTable.IsPartOfPrimarySubtypeRelationshipPath(Me.Table) Then
                        Return False
                    ElseIf lrTable IsNot Me.Table And lrTable IsNot Me.Table.FBMModelElement.GetTopmostNonAbsorbedSupertype.getCorrespondingRDSTable Then
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
                        Return Nothing
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Public Sub moveToOrdinalPosition(ByVal aiNewOrdinalPosition As Integer, ByVal aiFromOrdinalPosition As Integer)

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

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Call Me.Table.resetColumnOrdinalPositions()
            End Try

        End Sub

        Public Sub removeIndex(ByRef arIndex As RDS.Index)

            Call Me.Index.Remove(arIndex)

            RaiseEvent IndexRemoved(arIndex)

        End Sub

        Public Sub setActiveRole(ByRef arActiveRole As FBM.Role)

            Me.ActiveRole = arActiveRole

            Call Me.Model.Model.updateORSetCMMLPropertyActiveRole(Me)

            RaiseEvent ActiveRoleChanged()

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                Call Me.Model.Model.createCMMLAttributeIsMandatory(Me)
            Else
                Call Me.Model.Model.removeCMMLAttributeIsMandatory(Me)
            End If

            RaiseEvent IsMandatoryChanged(abIsMandatory)

            'Database synchronisation
            If Me.Model.Model.IsDatabaseSynchronised Then
                Call Me.Model.Model.connectToDatabase()
                Call Me.Model.Model.DatabaseConnection.columnSetMandatory(Me, abIsMandatory)
            End If

        End Sub

        Public Sub setName(ByVal asNewName As String)

            Try
                'For database modification
                Dim lrTempColumn = Me.Clone(Nothing, Nothing)
                lrTempColumn.Name = Me.Name

                Me.Name = asNewName

                'CMML
                Call Me.Model.Model.changeCMMLAttributeName(Me)

                RaiseEvent NameChanged(asNewName)

                If Me.Model.Model.IsDatabaseSynchronised Then

                    If Me.Model.Model.DatabaseConnection Is Nothing Then
                        'Try and establish a connection
                        Call Me.Model.Model.DatabaseManager.establishConnection(Me.Model.Model.TargetDatabaseType, Me.Model.Model.TargetDatabaseConnectionString)
                        If Me.Model.Model.DatabaseConnection Is Nothing Then
                            Throw New Exception("No database connection has been established.")
                        End If
                    ElseIf Me.Model.model.DatabaseConnection.Connected = False Then
                        Throw New Exception("The database is not connected.")
                    End If


                    Call Me.Model.Model.DatabaseConnection.renameColumn(lrTempColumn, asNewName)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub setOrdinalPosition(ByRef aiOrdinalPosition As Integer)

            Try
                Me.OrdinalPosition = aiOrdinalPosition

                Call Me.Model.Model.setCMMLAttributeOrdinalPosition(Me.Id, aiOrdinalPosition)

                RaiseEvent OrdinalPositionChanged(aiOrdinalPosition)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                    Call Me.Model.Model.changeCMMLAttributeEntityForColumn(Me, arTable)
                End If

                Me.Table = arTable

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

    End Class

End Namespace
