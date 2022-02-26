Imports System.Reflection
Imports System.Xml.Serialization
Imports Boston.FBM

Namespace RDS

    <Serializable()>
    Public Class Table
        Implements IEquatable(Of RDS.Table)

        <XmlIgnore()>
        <NonSerialized()>
        Public Model As RDS.Model

        <XmlAttribute()>
        Public Name As String

        <XmlIgnore()>
        Private _DatabaseName As String = ""

        Public Property DatabaseName As String
            Get
                If Me.FBMModelElement Is Nothing Then
                    'Should only be used when reverse engineering and where the TempModel that is initially
                    '  populated From the database does not have FBMModelElements.
                    Return Me._DatabaseName
                Else
                    If Me.FBMModelElement.IsDatabaseReservedWord Then
                        Return "[" & Me.FBMModelElement.DatabaseName & "]"
                    Else
                        Return Me.FBMModelElement.DatabaseName
                    End If
                End If
            End Get
            Set(value As String)
                'Only used when reverse engineering at this stage.
                Me._DatabaseName = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a variable name for the Table, as in when needed for TypeQL queries for TypeDB.
        '''   NB An Alias is added as part of FEQL processing.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DBVariableName As String
            Get
                If Me.FBMModelElement Is Nothing Then
                    'Should only be used when reverse engineering and where the TempModel that is initially
                    '  populated From the database does not have FBMModelElements.
                    Return Me._DatabaseName.Replace(" ", "")
                Else

                    Return Me.FBMModelElement.DatabaseName.Replace(" ", "")

                End If
            End Get
        End Property


        <XmlElement()>
        Public Column As New List(Of RDS.Column)

        <XmlElement()>
        Public WithEvents Index As New List(Of RDS.Index)

        <XmlAttribute()>
        Public IsSystemTable As Boolean = False

        <XmlAttribute()>
        Public Remarks As String = ""

        <XmlAttribute()>
        Public isPGSRelation As Boolean = False

        <XmlIgnore()>
        <NonSerialized()>
        Public WithEvents FBMModelElement As FBM.ModelObject 'The ModelElement that the Table relates to. Could be an EntityType or a FactType.

        Public ReadOnly Property isAbsorbed As Boolean
            Get
                Return Me.FBMModelElement.IsAbsorbed
            End Get
        End Property

        Public ReadOnly Property Arity As Integer
            Get
                If Me.FBMModelElement.ConceptType = pcenumConceptType.FactType Then
                    Return CType(Me.FBMModelElement, FBM.FactType).Arity
                Else
                    Return 0
                End If
            End Get
        End Property

        ''' <summary>
        ''' Used to determine the number of Non ValueType Model Elements joined by a PGSRelation (a table that is a PGSRelation).
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PGSArity As Integer
            Get
                If Me.FBMModelElement.ConceptType = pcenumConceptType.FactType Then
                    Dim lrFactType As FBM.FactType = CType(Me.FBMModelElement, FBM.FactType)
                    If lrFactType.Arity < 3 Then
                        'Because this property is only concerned with those FactTypes that join 3 or more model elements.
                        Return lrFactType.Arity
                    Else
                        Dim liCount = (From Role In lrFactType.RoleGroup
                                       Where Role.JoinedORMObject.GetType <> GetType(FBM.ValueType)).Count
                        Return liCount
                    End If
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property isSubtype() As Boolean
            Get
                Return Me.FBMModelElement.isSubtype
            End Get
        End Property

        Public ReadOnly Property OutgoingRelations As List(Of RDS.Relation)
            Get
                Dim larRelation = From Column In Me.Column
                                  From Relation In Column.Relation
                                  Where Relation.OriginTable Is Me
                                  Select Relation

                Return larRelation.ToList
            End Get
        End Property

        Public ReadOnly Property HasPrimaryKeyIndex As Boolean
            Get
                Return Me.Index.Find(Function(x) x.IsPrimaryKey) IsNot Nothing
            End Get
        End Property

        ''' <summary>
        ''' Predominantly used with TypeDB at this stage. Returns the Primary Supertype Name of the FBMModelElement of the Table. Could be 'entity' or 'relation'.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PrimarySupertypeName
            Get
                Return Me.FBMModelElement.PrimarySupertypeName
            End Get
        End Property

        ''' <summary>
        ''' TypeDB specific. The Roles 'played' by the Table.
        ''' </summary>
        <NonSerialized()>
        Public RolesPlayed As New List(Of RDS.Plays)

        ''' <summary>
        ''' TypeDB Specific. Is populated if Table is both a TypeDB Relation and has a Rule.
        ''' </summary>
        Public DerivationRule As String = Nothing

        ''' <summary>
        ''' TypeDB specific. The Roles 'related' by the Table.
        '''   I.e. e.g. If the Table is a 'relation' in TypeDB...then the set of RoleNames that the Table/Relation relates.
        '''   i.e. e.g. As in an ObjectifiedFactType, is the set of RoleNames related by the Table.
        ''' </summary>
        Public RelatedRoleNames As New List(Of String)

        ''' <summary>
        ''' Used in reverse engineering. TypeDB Specific at this stage. True if the Table is the result of a Relation in the database.
        ''' </summary>
        Public IsDBRelation As Boolean = False

        ''' <summary>
        ''' Used only in reverse engineering at this stage. TypeDB specific at this stage.
        ''' </summary>
        Private _PrimarySupertype As String = Nothing
        ''' <summary>
        ''' Used primarily in reverse engineering. TypeDB specific at this stage. The primary supertype of the Table/Entity/Node
        ''' </summary>
        ''' <returns></returns>
        Public Property PrimarySupertype As String
            Get
                If Me._PrimarySupertype IsNot Nothing Then
                    Return Me._PrimarySupertype
                Else
                    Return Me.FBMModelElement.PrimarySupertypeName
                End If
            End Get
            Set(value As String)
                Me._PrimarySupertype = value
            End Set
        End Property

        Public Event ColumnRemoved(ByVal arColumn As RDS.Column)
        Public Event ColumnAdded(ByRef arColumn As RDS.Column)
        Public Event IndexAdded(ByRef arIndex As RDS.Index)
        Public Event IndexModified(ByRef arIndex As RDS.Index)
        Public Event IndexColumnAdded(ByRef arIndex As RDS.Index, ByRef arColumn As RDS.Column)
        Public Event IndexRemoved(ByRef arIndex As RDS.Index)
        Public Event IsPGSRelationChanged(ByVal abNewValue As Boolean)
        Public Event NameChanged(ByVal asNewName As String)
        Public Event SubtypeRelationshipAdded()
        Public Event SubtypeRelationshipRemoved()

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As RDS.Model, ByVal asName As String, ByRef arModelElement As FBM.ModelObject)

            Me.Model = arModel
            Me.Name = asName
            Me.FBMModelElement = arModelElement

        End Sub

        ''' <summary>
        ''' Clones the table. Limited functionality at this stage. [VM-20210313]
        ''' </summary>
        ''' <returns></returns>
        Public Function Clone() As RDS.Table

            Dim lrTable As New RDS.Table
            Try
                With Me
                    lrTable.Name = .Name
                End With

                Return lrTable

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrTable
            End Try

        End Function

        Public Function CloneEntity(ByRef arPage As FBM.Page) As ERD.Entity

            Dim lrEntity As New ERD.Entity(arPage, Me)
            Try
                Return lrEntity
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Shadows Function Equals(other As Table) As Boolean Implements IEquatable(Of Table).Equals

            Return Me.Name = other.Name

        End Function

        Public Function EqualsByName(ByVal other As RDS.Table) As Boolean

            Return Me.Name = other.Name
        End Function

        Public Shared Function CompareName(x As Table, y As Table) As Integer

            Return String.Compare(x.Name, y.Name)

        End Function

        Public Sub absorbSubtypeColumns(ByRef arSubtypeTable As RDS.Table)

            Try
                If Not arSubtypeTable.isAbsorbed Then Throw New Exception("The Table, '" & arSubtypeTable.Name & "', is not absorbed.")

                For Each lrColumn In arSubtypeTable.Column
                    Dim lrClonedColumn = lrColumn.Clone(Me, Nothing)
                    Me.Column.Add(lrClonedColumn)
                Next

                For Each lrTable In arSubtypeTable.getSubtypeTables.FindAll(Function(x) x.isAbsorbed)
                    'Recursive call to get all Columns from absorbed subtype Tables.
                    Call Me.absorbSubtypeColumns(lrTable)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub absorbSupertypeColumns()

            If Me.isSubtype = False Then Exit Sub

            If Me.isAbsorbed Then Exit Sub

            Try
                For Each lrTable In Me.getSupertypeTables
                    For Each lrColumn In lrTable.Column

                        'Move Origins of relevant Relations.
                        Dim larRelation = From Relation In lrColumn.OutgoingRelation
                                          Select Relation

                        For Each lrRelation In larRelation.ToArray
                            lrRelation.OriginTable = Me
                            Call Me.Model.Model.updateRelationOriginTable(lrRelation, Me)
                        Next

                        'Move Destinations of relevant Relations.
                        larRelation = From Relation In lrColumn.IncomingRelation
                                      Select Relation

                        For Each lrRelation In larRelation.ToArray
                            If lrRelation.ResponsibleFactType.PointsToTable.Contains(Me) Then
                                Call Me.Model.Model.updateRelationDestinationTable(lrRelation, Me)
                                Call lrRelation.setDestinationTable(Me)
                            End If
                        Next

                        Dim lrNewColumn = lrColumn.Clone(Me, Nothing)
                        lrNewColumn.Relation.AddRange(lrColumn.Relation)

                        Dim lrExistingColumn = Me.Column.Find(Function(x) x.Role Is lrNewColumn.Role And x.ActiveRole Is lrNewColumn.ActiveRole)
                        If lrExistingColumn Is Nothing Then
                            Call Me.addColumn(lrNewColumn)
                        End If
                    Next
                Next
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Adds the specified Column to the Table.
        ''' </summary>
        ''' <param name="arColumn"></param>
        ''' <param name="abAddToDatabase">True if the column is to be added to the connected database.</param>
        Public Function addColumn(ByRef arColumn As RDS.Column,
                                  Optional abAddToDatabase As Boolean = False,
                                  Optional abProcessCMML As Boolean = True) As Boolean

            Try
                Call arColumn.setOrdinalPosition(Me.Column.Count + 1)
                arColumn.Table = Me 'CodeSafe

                'CodeSafe: Don't add the Column if it already exists.
                If Me.Column.Contains(arColumn) Then
                    Return False
                End If

                Me.Column.AddUnique(arColumn)

                If arColumn.Role.isRDSForeignKeyRole Then

                    Dim lrRelation As RDS.Relation = arColumn.Role.belongsToRelation
                    If lrRelation IsNot Nothing And arColumn.Relation IsNot Nothing Then
                        lrRelation.OriginColumns.AddUnique(arColumn)
                        arColumn.Relation.AddUnique(lrRelation)
                    End If
                End If

                '20200726-VM-Review the follow because is not true. Have modified the code to stop this.
                '  If something is going wrong then revise. If a a PGSRelation Node/Table adds a Column for an Active role
                '  on a ManyTo1FT attached to the PGSRelationNode, for instance, then certainly the PGSRelation Note/Table remains.
                'Is only IsPGSRelation if the Objectified Fact Type (i.e. The objectified relation) only references ValueTypes. 
                'i.e.Where ObjectifiedFactType Is only joined To ValueTypes.
                ' If Is joined To any EntityType Or another ObjectifiedFactType, Then cannot be a PGSRelation, And must be a Node.
                If (arColumn.isPartOfPrimaryKey = False) And (TypeOf (arColumn.Role.JoinedORMObject) IsNot FBM.ValueType) Then '20210505-VM-Was ContributesToPrimaryKey
                    '--------------------------------------------------------------------
                    'Remove IsPGSRelation if join is not to a ValueType
                    'Call Me.setIsPGSRelation(False) '20200726-VM-Commented this out. See notes above.
                ElseIf TypeOf (Me.FBMModelElement) Is FBM.FactType Then
                    'FBMModelObject is the ModelElement that the Table relates to.
                    If Not (arColumn.isPartOfPrimaryKey) And (arColumn.FactType IsNot Me.FBMModelElement) And (TypeOf (arColumn.Role.JoinedORMObject) IsNot FBM.ValueType) Then '20210505-VM-Was ContributesToPrimaryKey
                        Call Me.setIsPGSRelation(False)
                    End If
                End If

                'NonAbsorbed Subtypes
                For Each lrTable In Me.getSubtypeTables(False).FindAll(Function(x) x.isAbsorbed = False)
                    Call lrTable.addColumn(arColumn)
                Next

                '------------------------------------------------------------------------------
                'CMML Code
                If abProcessCMML Then
                    Call Me.Model.Model.createCMMLAttribute(Me.Name, arColumn.Name, arColumn.Role, arColumn)
                End If

                '------------------------------------------------------------------------------
                'Database Synchronisation Code
                If abAddToDatabase Then
                    Call Me.Model.Model.connectToDatabase()
                    If Me.Column.Count = 1 Then
                        'Create the Table/Column combination, because can't create the Column without the Table and is first Column in Table.
                        Call Me.Model.Model.DatabaseConnection.createTable(Me, arColumn)
                    Else
                        Call Me.Model.Model.DatabaseConnection.addColumn(arColumn)
                    End If
                End If

                RaiseEvent ColumnAdded(arColumn)

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Public Sub addIndex(ByRef arIndex As RDS.Index, Optional ByRef arRoleConstraint As FBM.RoleConstraint = Nothing)

            If arRoleConstraint Is Nothing Then
                arIndex.ResponsibleRoleConstraint = arIndex.getResponsibleRoleConstraintFromORMModel()
            Else
                arIndex.ResponsibleRoleConstraint = arRoleConstraint
            End If

            Me.Index.AddUnique(arIndex)
            Me.Model.Index.AddUnique(arIndex)

            '-----------------------------------------------------------------------------
            'CMML Code
            Call Me.Model.Model.createCMMLIndex(arIndex.Name,
                                                arIndex.Table.Name,
                                                arIndex.IndexQualifier,
                                                arIndex.AscendingOrDescending,
                                                arIndex.IsPrimaryKey,
                                                arIndex.Unique,
                                                arIndex.IgnoresNulls,
                                                arIndex.Column)


            RaiseEvent IndexAdded(arIndex)

            'Database synchronisation
            If Me.Model.Model.IsDatabaseSynchronised Then
                Call Me.Model.Model.connectToDatabase()
                Call Me.Model.Model.DatabaseConnection.addIndex(arIndex)
            End If

        End Sub

        Public Function CountNonValueTypeColumns() As Integer

            Try
                Return Me.Column.FindAll(Function(x) Not x.ActiveRole.HasInternalUniquenessConstraint _
                                                     And x.ActiveRole.JoinedORMObject.GetType = GetType(FBM.ValueType) _
                                                     And x.Role.FactType Is x.ActiveRole.FactType).Count

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function getIncomingRelations() As List(Of RDS.Relation)

            Try
                Dim larIncomingRelations = From Relation In Me.Model.Model.RDS.Relation
                                           From Column In Relation.DestinationColumns
                                           Where Column.Table.Name = Me.Name
                                           Select Relation Distinct

                Return larIncomingRelations.ToList

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Relation)
            End Try

        End Function

        Public Function getRelations() As List(Of RDS.Relation)

            Try
                Dim larRelation As New List(Of RDS.Relation)

                Dim larOutgoingRelation = From Relation In Me.Model.Model.RDS.Relation
                                          From Column In Relation.OriginColumns
                                          Where Column.Table.Name = Me.Name
                                          Select Relation Distinct

                larRelation.AddRange(larOutgoingRelation.ToList)

                Dim larIncomingRelation = From Relation In Me.Model.Model.RDS.Relation
                                          From Column In Relation.DestinationColumns
                                          Where Column.Table.Name = Me.Name
                                          Select Relation Distinct

                larRelation.AddRange(larIncomingRelation.ToList)

                Return larRelation

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Relation)
            End Try

        End Function

        Public Function getOutgoingRelations() As List(Of RDS.Relation)

            Try
                Dim larOutgoingRelation = From Relation In Me.Model.Model.RDS.Relation
                                          Where Relation.OriginTable.Name = Me.Name
                                          Select Relation Distinct

                Return larOutgoingRelation.ToList

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Relation)
            End Try

        End Function

        Public Function getPGSEdgeName() As String

            Try
                If Not Me.FBMModelElement.ConceptType = pcenumConceptType.FactType Then Return "" 'Throw New Exception("Table does not represent a Fact Type.")

                Dim lrFactType = CType(Me.FBMModelElement, FBM.FactType)

                'If Me.isPGSRelation Then Throw New Exception("Table is not a Property Graph Schema Relation/Edge, for Fact Type: '" & lrFactType.Id & "'")

                If lrFactType.FactTypeReading.Count = 0 Then Throw New Exception("Fact Type, '" & lrFactType.Id & "', has no Fact Type Reading.")

                Dim lrFactTypeReading = lrFactType.FactTypeReading(0)

                Return Viev.Strings.MakeLowerCapCamelCase(lrFactTypeReading.PredicatePart(0).PredicatePartText)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return "ErrorGettingEdgeNameForTable"
            End Try

        End Function

        ''' <summary>
        ''' Returns the first Uniqueness Constraint Columns, even if the Index is the PrimaryKey
        ''' </summary>
        ''' <returns></returns>
        Public Function getFirstUniquenessConstraintColumns() As List(Of RDS.Column)

            Try
                Dim lrIndex As RDS.Index = Me.Index.Find(Function(x) x.IsPrimaryKey = False)

                If lrIndex Is Nothing Then
                    Dim larColumn = From Column In Me.Column
                                    Where Column.isPartOfPrimaryKey = True
                                    Select Column Distinct
                                    Order By Column.OrdinalPosition

                    Return larColumn.ToList
                Else
                    Dim larColumn = From Column In lrIndex.Column
                                    Select Column
                                    Order By Column.OrdinalPosition

                    Return larColumn.ToList
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Column)
            End Try

        End Function

        Public Function getPreferredIdentifierRoleConstraint() As FBM.RoleConstraint

            Try
                If Me.FBMModelElement.GetType = GetType(FBM.EntityType) Then
                    Return CType(Me.FBMModelElement, FBM.EntityType).ReferenceModeRoleConstraint
                Else
                    'FBModelElement must be a FactType
                    Dim lrFactType As FBM.FactType = Me.FBMModelElement
                    Dim lrRoleConstraint As FBM.RoleConstraint

                    lrRoleConstraint = lrFactType.InternalUniquenessConstraint.Find(Function(x) x.IsPreferredIdentifier)

                    Return lrRoleConstraint

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function getPrimaryKeyColumns() As List(Of RDS.Column)

            Try
                Dim larColumn = From Column In Me.Column
                                Where Column.isPartOfPrimaryKey = True
                                Select Column Distinct

                Return larColumn.ToList

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Column)
            End Try

        End Function

        Public Function getPrimaryKeyIndex() As RDS.Index

            Try
                Return Me.Index.Find(Function(x) x.IsPrimaryKey)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                Return Nothing
            End Try
        End Function

        Public Function generateUniqueQualifier(ByVal asRootQualifier As String, Optional ByVal aiIndex As Integer = 0) As String

            Dim lsQualifier As String = ""

            If aiIndex = 0 Then
                lsQualifier = asRootQualifier
            Else
                lsQualifier = asRootQualifier & aiIndex.ToString
            End If

            If Me.Index.FindAll(Function(x) x.IndexQualifier = lsQualifier).Count = 0 Then
                Return lsQualifier
            Else
                Return Me.generateUniqueQualifier(asRootQualifier, aiIndex + 1)
            End If

        End Function

        Public Function getRelationByFBMModelObjects(ByVal aarFBMModelObject As List(Of FBM.ModelObject),
                                                     ByVal arFactType As FBM.FactType,
                                                     Optional ByRef arQueryEdge As FactEngine.QueryEdge = Nothing) As RDS.Relation

            Try
                Dim larRelation = From Relation In Me.getOutgoingRelations
                                  Where aarFBMModelObject.Contains(Relation.DestinationTable.FBMModelElement)
                                  Where aarFBMModelObject.Contains(Relation.OriginTable.FBMModelElement)
                                  Where Relation.ResponsibleFactType.Id = arFactType.Id
                                  Select Relation

                If larRelation.Count = 0 Then

                    For Each lrRelation In Me.getOutgoingRelations

                        If lrRelation.DestinationTable.FBMModelElement.hasModelElementAsDownstreamSubtype(aarFBMModelObject(1)) And
                            aarFBMModelObject.Contains(lrRelation.OriginTable.FBMModelElement) Then
                            Return lrRelation
                        End If
                    Next

                End If

                Return larRelation.First

            Catch ex As Exception
                Dim lsMessage = "Error trying to find a relation between ModelElements, "
                Dim liInd = 1
                For Each lrModelElement In aarFBMModelObject
                    If liInd > 1 Then lsMessage &= ", "
                    lsMessage &= "'" & lrModelElement.Id & "'"
                    liInd += 1
                Next
                lsMessage &= ", for FactType: " & arFactType.Id & ","
                lsMessage &= " with Predicates"
                liInd = 1
                For Each lrFactTypeReading In arFactType.FactTypeReading
                    For Each lrPredicate In lrFactTypeReading.PredicatePart.FindAll(Function(x) x.PredicatePartText <> "")
                        If liInd > 1 Then lsMessage &= ", "
                        lsMessage &= "'" & lrPredicate.PredicatePartText & "'"
                        liInd += 1
                    Next
                Next
                If arQueryEdge IsNot Nothing Then
                    lsMessage &= ". Are you sure you have the right predicate reading, '" & arQueryEdge.BaseNode.Name & " " & arQueryEdge.Predicate & " " & arQueryEdge.TargetNode.Name & "'?"
                End If

                Throw New Exception(lsMessage)
            End Try

        End Function

        ''' <summary>
        ''' Non recursive, single layer return of subtype Tables.
        ''' </summary>
        ''' <returns></returns>
        Public Function getSubtypeTables(Optional ByVal abCreateTableIfNotExists As Boolean = True,
                                         Optional ByVal abPrimarySubtypeRelationshipsOnly As Boolean = False) As List(Of RDS.Table)

            Dim larSubtypeTable As New List(Of RDS.Table)

            Dim larModelObject = Me.FBMModelElement.getSubtypes(abPrimarySubtypeRelationshipsOnly)

            For Each lrModelObject In larModelObject
                If Not lrModelObject.IsAbsorbed Then 'Absorbed Subtypes do not have Tables.
                    Dim lrTable = CType(lrModelObject, FBM.EntityType).getCorrespondingRDSTable(abCreateTableIfNotExists)
                    If lrTable IsNot Nothing Then
                        larSubtypeTable.Add(lrTable)
                    End If
                End If
            Next

            Return larSubtypeTable

        End Function

        Public Function getSupertypeTables(Optional ByRef aarSupertypeTable As List(Of RDS.Table) = Nothing,
                                           Optional arSubtypeRelationship As FBM.tSubtypeRelationship = Nothing,
                                           Optional abIsRecursive As Boolean = True) As List(Of RDS.Table)

            Try

                Dim larSupertypeTable As New List(Of RDS.Table)
                Dim larSubtypeRelationship As New List(Of FBM.tSubtypeRelationship)

                If arSubtypeRelationship Is Nothing Then
                    larSubtypeRelationship = Me.FBMModelElement.SubtypeRelationship
                Else
                    larSubtypeRelationship.Add(arSubtypeRelationship)
                End If

                For Each lrSubtypeRelationship In larSubtypeRelationship
                    Dim lrSupertypeTable = lrSubtypeRelationship.parentModelElement.getCorrespondingRDSTable
                    larSupertypeTable.AddUnique(lrSupertypeTable)
                    If abIsRecursive Then
                        Call lrSupertypeTable.getSupertypeTables(larSupertypeTable)
                    End If
                Next

                If aarSupertypeTable IsNot Nothing Then
                    aarSupertypeTable.AddRange(larSupertypeTable)
                End If

                Return larSupertypeTable

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Table)
            End Try

        End Function

        ''' <summary>
        ''' Used in sorting of Tables by Tables referenced by other Tables, as in when for creating output SQL in the propper Table order
        '''   such that Foreign Key reference constraints are created in the correct order.
        ''' NB Relies on the ListPos property of the Table being correct.
        ''' </summary>
        ''' <returns></returns>
        Public Function hasHigherReferencedTable() As Boolean

            Dim liInd As Integer
            Dim lrComparisonTable As RDS.Table
            Try
                If Me.Model.Table.IndexOf(Me) = Me.Model.Table.Count - 1 Then Return False

                For liInd = Me.Model.Table.IndexOf(Me) To Me.Model.Table.Count - 2
                    For Each lrRelation In Me.getOutgoingRelations
                        lrComparisonTable = Me.Model.Table(liInd + 1)

                        If lrRelation.DestinationTable Is lrComparisonTable Then
                            If lrComparisonTable.getOutgoingRelations.Find(Function(x) x.DestinationTable Is Me) Is Nothing Then
                                'Tables that don't reference themselves in a loop
                                Return True
                            End If
                        End If
                    Next
                Next

                Return False

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function


        ''' <summary>
        ''' Returns TRUE if the Table has a PrimaryKey with one Column, 
        ''' ELSE returns FALSE
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function hasSingleColumnPrimaryKey() As Boolean

            hasSingleColumnPrimaryKey = False

            If Me.Index.FindAll(Function(x) x.IsPrimaryKey = True).Count = 0 Then
                Return False
            End If

            If Me.Index.Find(Function(x) x.IsPrimaryKey = True).Column.Count = 1 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function getIndexByColumns(ByRef aarIndexColumn As List(Of RDS.Column)) As RDS.Index

            Dim lrIndex As New RDS.Index

            lrIndex.Column = aarIndexColumn

            Return Me.Index.Find(AddressOf lrIndex.EqualsByColumns)

        End Function

        Public Sub getCMMLColumns()

            Dim lsSQLQuery As String
            Dim lrORMRecordset2, lrORMRecordset3 As ORMQL.Recordset
            Dim lsColumnName As String
            Dim lrResponsibleRole, lrActiveRole As FBM.Role
            Dim lrColumn As RDS.Column

            '==========================================================================================================
            'Columns
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
            lsSQLQuery &= " WHERE ModelObject = '" & Me.Name & "'"

            lrORMRecordset2 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            While Not lrORMRecordset2.EOF

                'Column Name
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CorePropertyHasPropertyName" '(Property, PropertyName)
                lsSQLQuery &= " WHERE Property = '" & lrORMRecordset2("Attribute").Data & "'"

                lrORMRecordset3 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lsColumnName = lrORMRecordset3("PropertyName").Data

                'Responsible Role
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                lsSQLQuery &= " WHERE Property = '" & lrORMRecordset2("Attribute").Data & "'"

                lrORMRecordset3 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrResponsibleRole = Me.Model.Model.Role.Find(Function(x) x.Id = lrORMRecordset3("Role").Data)

                'Active Role
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
                lsSQLQuery &= " WHERE Property = '" & lrORMRecordset2("Attribute").Data & "'"

                lrORMRecordset3 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrActiveRole = Me.Model.Model.Role.Find(Function(x) x.Id = lrORMRecordset3("Role").Data)

                'New Column
                lrColumn = New RDS.Column(Me, lsColumnName, lrResponsibleRole, lrActiveRole)
                lrColumn.Id = lrORMRecordset2("Attribute").Data

                'IsMandatory
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsMandatory.ToString
                lsSQLQuery &= " WHERE IsMandatory = '" & lrColumn.Id & "'"

                lrORMRecordset3 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrColumn.IsMandatory = Not lrORMRecordset3.EOF

                'Fact Type
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                lrORMRecordset3 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrColumn.FactType = Me.Model.Model.FactType.Find(Function(x) x.Id = lrORMRecordset3("FactType").Data)

                'Ordinal Position
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                lrORMRecordset3 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrColumn.OrdinalPosition = lrORMRecordset3("Position").Data

                Me.Column.Add(lrColumn)

                lrORMRecordset2.MoveNext()

            End While
            '==========================================================================================================

            '===========================
            'Indexes
            Call Me.getCMMLIndexes()

        End Sub

        Public Sub getCMMLIndexes()

            Dim lrIndex As RDS.Index
            Dim lrColumn As RDS.Column
            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset

            Try
                '-------------------------------------------------
                'Must create a Primary Identifier for the Entity
                '-------------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
                lsSQLQuery &= " WHERE Entity = '" & Me.Name & "'"

                lrRecordset = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrRecordset.EOF

                    lrIndex = New RDS.Index(Me, lrRecordset("Index").Data)

                    'Columns 
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString '(Index, Property)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    While Not lrRecordset1.EOF

                        lrColumn = New RDS.Column
                        lrColumn = Me.Column.Find(Function(x) x.Id = lrRecordset1("Property").Data)

                        'Code Safe. If Column doesn't exist, need to remove it from the Index.
                        If lrColumn Is Nothing Then
                            lrColumn = New RDS.Column(Me, "Dummy", Nothing, Nothing)
                            lrColumn.Id = lrRecordset1("Property").Data
                            lrIndex.removeColumn(lrColumn)
                        Else
                            lrColumn.Index.AddUnique(lrIndex)

                            lrIndex.Column.AddUnique(lrColumn)
                        End If

                        lrRecordset1.MoveNext()
                    End While

                    'Restrains to Unique Values
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexRestrainsToUniqueValues.ToString
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.Unique = Not lrRecordset1.EOF


                    'Is PrimaryKey
                    lsSQLQuery = "SELECT * "
                    lsSQLQuery &= "FROM " & pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.IsPrimaryKey = Not lrRecordset1.EOF

                    '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                    'For Each lrColumn In lrIndex.Column
                    '    lrColumn.ContributesToPrimaryKey = Not lrRecordset1.EOF
                    'Next

                    'Ignore Nulls
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIgnoresNulls.ToString
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.IgnoresNulls = Not lrRecordset1.EOF

                    'Direction
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexHasDirection.ToString '(Index, IndexDirection)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.AscendingOrDescending.GetByDescription(lrRecordset1("IndexDirection").Data)

                    'Qualifier
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexHasQualifier.ToString '(Index, Qualifier)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.IndexQualifier = lrRecordset1("Qualifier").Data

                    '==========================
                    'Add the Index to the RDS
                    Me.Model.Model.RDS.Index.AddUnique(lrIndex)

                    Me.Index.AddUnique(lrIndex)

                    lrRecordset.MoveNext()
                End While 'Indexes in the Model for the arTable.

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function getColumnByOrdingalPosition(ByVal aiOrdinalPosition As Integer) As RDS.Column

            Try

                Return Me.Column.Find(Function(x) x.OrdinalPosition = aiOrdinalPosition)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function getFirstNonPrimaryKeyColumnOrdinalPosition() As Integer

            Try
                For Each lrColumn In Me.Column.OrderBy(Function(x) x.OrdinalPosition)
                    If lrColumn.isPartOfPrimaryKey = False Then '20210505-VM-Was ContributesToPrimaryKey
                        Return lrColumn.OrdinalPosition
                    End If
                Next

                Throw New Exception("Table, " & Me.Name & " does not have any Column that is not part of the Primary Key.")

                Return 0

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return 0
            End Try

        End Function

        Private Sub FBMModelElement_NameChanged(ByVal asNewName As String) Handles FBMModelElement.NameChanged

            'For database synchronisation
            Dim lrTempTable = Me.Clone

            Me.Name = asNewName

            RaiseEvent NameChanged(asNewName)

            'Database synchronisation
            If Me.Model.Model.IsDatabaseSynchronised Then
                Call Me.Model.Model.connectToDatabase()
                Call Me.Model.Model.DatabaseConnection.RenameTable(lrTempTable, asNewName)
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arColumn">Use 'Nothing' if adding new column, rather than renaming column.</param>
        ''' <param name="asColumnName">The trial column name</param>
        ''' <param name="aiStartingInd">Start with 0. Recusion adds 1 each iteration. 0 is not added to the asColumnName</param>
        ''' <returns></returns>
        Public Function createUniqueColumnName(ByVal arColumn As RDS.Column, ByVal asColumnName As String, ByVal aiStartingInd As Integer) As String

            Dim lsUniqueColumnName As String = ""

            Dim lsSQLQuery As String = ""

            If aiStartingInd = 0 Then
                lsUniqueColumnName = asColumnName
            Else
                lsUniqueColumnName = asColumnName & aiStartingInd.ToString
            End If

            If arColumn Is Nothing Then
                If Me.Column.FindAll(Function(x) LCase(x.Name) = LCase(lsUniqueColumnName)).Count > 0 Then
                    lsUniqueColumnName = Me.createUniqueColumnName(arColumn, asColumnName, aiStartingInd + 1)
                End If
            ElseIf Me.Column.FindAll(Function(x) LCase(x.Name) = LCase(lsUniqueColumnName) And x.Id <> arColumn.Id).Count > 0 Then
                lsUniqueColumnName = Me.createUniqueColumnName(arColumn, asColumnName, aiStartingInd + 1)
            End If

            Return lsUniqueColumnName

        End Function

        ''' <summary>
        ''' TRUE if is a table like 'LecturerLikesLecturer' joining on Lecturer table twice.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Function isCircularToTable(ByVal arTable As RDS.Table) As Boolean

            Try
                Dim larJoinColumn = (From Column In Me.Column.FindAll(Function(x) x.Relation.Count > 0)
                                     From Relation In Column.Relation.FindAll(Function(x) x.OriginTable Is Me)
                                     Where Relation.DestinationTable.Name = arTable.Name
                                     Select Column
                                     Order By Column.OrdinalPosition).ToList

                Return larJoinColumn.Count = 2

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Function

        Public Function isConstrainedByRingConstraint(ByRef arRoleConstraint As FBM.RoleConstraint) As Boolean

            Try
                For Each lrRingConstraint In Me.Model.Model.RoleConstraint.FindAll(Function(x) x.RoleConstraintType = pcenumRoleConstraintType.RingConstraint)
                    If lrRingConstraint.Role(0).JoinedORMObject Is Me.Column(0).Role.JoinedORMObject Then
                        arRoleConstraint = lrRingConstraint
                        Return True
                    End If
                Next

                Return False

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Public Function IsPartOfPrimarySubtypeRelationshipPath(ByRef arTable As RDS.Table) As Boolean

            Try
                If arTable Is Me Then
                    Return True
                Else
                    For Each lrSubtypeRelationship In arTable.FBMModelElement.SubtypeRelationship.FindAll(Function(x) x.IsPrimarySubtypeRelationship)
                        Select Case lrSubtypeRelationship.parentModelElement.GetType
                            Case Is = GetType(FBM.EntityType)
                                Dim lrEntityType As FBM.EntityType = CType(lrSubtypeRelationship.parentModelElement, FBM.EntityType)
                                If lrEntityType.IsObjectifyingEntityType Then
                                    If lrEntityType.ObjectifiedFactType Is Me.FBMModelElement Then
                                        Return True
                                    End If
                                Else
                                    If lrSubtypeRelationship.parentModelElement Is Me.FBMModelElement Then
                                        Return True
                                    End If
                                End If
                            Case Else
                                If lrSubtypeRelationship.parentModelElement Is Me.FBMModelElement Then
                                    Return True
                                End If
                        End Select
                        Return lrSubtypeRelationship.parentModelElement.getCorrespondingRDSTable.IsPartOfPrimarySubtypeRelationshipPath(arTable)
                    Next
                End If


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function isPGSNode() As Boolean

            Try
                If Me.FBMModelElement.GetType = GetType(FBM.FactType) Then

                    Dim lrFactType = CType(Me.FBMModelElement, FBM.FactType)

                    If Me.FBMModelElement.IsObjectified Then
                        Return True
                    ElseIf lrFactType.HasTotalRoleConstraint Or lrFactType.HasPartialButMultiRoleConstraint Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        ''' <summary>
        ''' Used when the User changes a Unique Index (that is not the existing Primary Key) to the Primary Key. Therefore, the existing Primary Key becomes simple a Unique Index (not Primary Key)
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub makeExistingPrimaryKeySimplyUnique(ByRef larColumnsAffected As List(Of RDS.Column))

            Try
                Dim larIndex = (From Index In Me.Index
                                Where Index.IsPrimaryKey = True
                                Select Index).ToList

                If larIndex.Count = 0 Then
                    larIndex.Add(Me.getIndexByColumns(larColumnsAffected))
                End If

                If larIndex.Count > 0 Then

                    Dim lrIndex As RDS.Index = larIndex.First

                    If lrIndex IsNot Nothing Then
                        lrIndex.setIsPrimaryKey(False)
                        lrIndex.setQualifier(Me.generateUniqueQualifier("UC"))
                        lrIndex.setName(Me.Name & "_" & lrIndex.IndexQualifier)

                        For Each lrColumn In lrIndex.Column
                            Call lrColumn.triggerForceRefreshEvent()
                        Next
                    End If

                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub movePrimaryKeyColumnsToTopOrdinalPosition()

            Dim lrColumn As RDS.Column
            Dim lrSwitchingColumn As RDS.Column

            If Me.Column.FindAll(Function(x) x.isPartOfPrimaryKey = True).Count = 1 Then '20210505-VM-Was ContributesToPrimaryKey

                lrSwitchingColumn = Me.Column.Find(Function(x) x.OrdinalPosition = 1)
                lrColumn = Me.Column.Find(Function(x) x.isPartOfPrimaryKey = True) '20210505-VM-Was ContributesToPrimaryKey

                Dim liOriginalOrdinalPosition = lrColumn.OrdinalPosition

                If liOriginalOrdinalPosition <> 1 Then
                    lrColumn.OrdinalPosition = 1
                    Call Me.Model.Model.setCMMLAttributeOrdinalPosition(lrColumn.Id, lrColumn.OrdinalPosition)

                    lrSwitchingColumn.OrdinalPosition = liOriginalOrdinalPosition
                    Call Me.Model.Model.setCMMLAttributeOrdinalPosition(lrSwitchingColumn.Id, lrSwitchingColumn.OrdinalPosition)
                End If
            Else
                Dim larColumn = From Column In Me.Column
                                Where Column.isPartOfPrimaryKey = True '20210505-VM-Was ContributesToPrimaryKey
                                Select Column

                Dim liOrdinalPosition As Integer = 0

                If larColumn.Count < Me.Column.Count Then

                    For Each lrColumn In larColumn
                        If lrColumn.hasNonPrimaryKeyColumnsAboveIt() Then
                            liOrdinalPosition = Me.getFirstNonPrimaryKeyColumnOrdinalPosition
                            Call lrColumn.moveToOrdinalPosition(liOrdinalPosition, lrColumn.OrdinalPosition)
                        End If
                    Next

                End If
            End If

        End Sub

        Public Sub RaiseIndexModifiedEvent(ByRef arIndex As RDS.Index)

            Try
                RaiseEvent IndexModified(arIndex)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        ''' <summary>
        ''' Removes a Column from the table.
        ''' </summary>
        ''' <param name="arColumn">The actual Column object to be removed from the Table.</param>
        ''' <param name="abRemoveResidualCMML">Use False if the CMML for the Column/Attribute/Property is to be kept.</param>
        ''' <remarks></remarks>
        Public Sub removeColumn(ByRef arColumn As RDS.Column,
                                Optional abRemoveFromDatabase As Boolean = False,
                                Optional abRemoveResidualCMML As Boolean = True)

            Try
                Dim lrColumn As RDS.Column = arColumn
                Me.Column.Remove(arColumn)

                Dim lrRemovedColumn As RDS.Column = arColumn
                For Each lrColumn In Me.Column.FindAll(Function(x) x.OrdinalPosition > lrRemovedColumn.OrdinalPosition)
                    lrColumn.OrdinalPosition -= 1
                    Call Me.Model.Model.setCMMLAttributeOrdinalPosition(lrColumn.Id, lrColumn.OrdinalPosition)
                Next

                '------------------------------------------------------------------------------------------------------
                'Relations
                Dim larRelation = From Relation In Me.getOutgoingRelations
                                  Where Relation.OriginColumns.Contains(lrColumn)
                                  Select Relation

                For Each lrRelation In larRelation.ToArray
                    lrRelation.OriginColumns.Remove(arColumn)
                    If lrRelation.OriginColumns.Count = 0 Then
                        Call Me.Model.removeRelation(lrRelation)
                    End If
                Next

                RaiseEvent ColumnRemoved(arColumn)

                '------------------------------------------------------------------------------
                'CMML Code
                If arColumn.Table Is Me And abRemoveResidualCMML Then
                    'If Column is from another Table, may want to keep it (RemoveResidualCMML = False).
                    '  E.g. If changing a Subtype EntityType to Not IsAbsorbed, a Column may be moved from a
                    '  Supertype to that Subtype, so do not want to remove the CMML.
                    Call Me.Model.Model.removeCMMLAttribute(arColumn.Table.Name, arColumn.Id)
                Else
                    Call Me.Model.Model.removeCMMLAttributeFromTableOnly(arColumn, Me)
                End If

                'Database synchronisation
                If abRemoveFromDatabase Then
                    If Me.Model.Model.IsDatabaseSynchronised Then
                        Call Me.Model.Model.connectToDatabase()
                        Call Me.Model.Model.DatabaseConnection.removeColumn(arColumn)
                    End If
                End If

                If Me.Column.Count = 0 Then
                    Call Me.Model.removeTable(Me)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Removes Columns that are actually from the given other (Sub/Supertype) Table.
        '''   This is used, for example, when a Subtype is changed from IsAbsorbed = True to IsAbsorbed = False.
        ''' </summary>
        ''' <param name="arTable"></param>
        Public Sub RemoveColumnsFromTable(ByRef arTable As RDS.Table)

            Try
                Dim lrTable As RDS.Table = arTable

                Dim larColumn = From Column In Me.Column
                                Where Column.Role.JoinedORMObject.Id = lrTable.Name
                                Select Column

                For Each lrColumn In larColumn.ToArray
                    Call Me.removeColumn(lrColumn,, False)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeExistingPrimaryKey()

            Dim larIndex = From Index In Me.Index
                           Where Index.IsPrimaryKey = True
                           Select Index

            If larIndex.Count > 0 Then
                Call Me.removeIndex(larIndex.First)
            End If

        End Sub

        Public Sub removeExistingPrimaryKeyColumnsAndIndex(Optional ByVal abRecursive As Boolean = False)

            'Remove the Index
            Dim larIndex = From Index In Me.Index
                           Where Index.IsPrimaryKey = True
                           Select Index

            If larIndex.Count > 0 Then
                Call Me.removeIndex(larIndex.First)
            End If

            'Remove the Columns
            Dim larColumn = From Column In Me.Column
                            Where Column.isPartOfPrimaryKey '20210505-BM-Was ContributesToPrimaryKey
                            Select Column

            For Each lrColumn In larColumn.ToArray
                Call Me.removeColumn(lrColumn)
            Next

            Me.triggerSubtypeRelationshipRemoved()

            If abRecursive Then
                For Each lrTable In Me.getSubtypeTables.FindAll(Function(x) x.isAbsorbed = False)
                    Call lrTable.removeExistingPrimaryKeyColumnsAndIndex(abRecursive)
                Next
            End If

        End Sub

        Public Sub removeIndex(ByRef arIndex As RDS.Index)

            Call Me.Index.Remove(arIndex)
            Call Me.Model.Index.Remove(arIndex)

            '----------------------------------------------------------------------
            'CMML
            Call Me.Model.Model.removeCMMLIndex(arIndex)

            RaiseEvent IndexRemoved(arIndex)

        End Sub

        Public Sub removeIndexByRoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint)

            Try
                Dim larIndexColumn As New List(Of RDS.Column)
                For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole

                    Dim larColumn = From Column In Me.Column
                                    Where lrRoleConstraintRole.Role.FactType.Arity = 2
                                    Where Column.Role.Id = lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id).Id
                                    Select Column

                    For Each lrColumn In larColumn
                        larIndexColumn.Add(lrColumn)
                    Next 'Column

                Next 'RoleConstraintRole

                Dim lrIndex As RDS.Index

                '------------------------------------------------------------------------
                'FInd any existing Index for the previous version of the RoleConstraint
                lrIndex = Me.getIndexByColumns(larIndexColumn)

                If lrIndex IsNot Nothing Then
                    Call Me.Model.removeIndex(lrIndex)
                End If

                For Each lrSubtypeTable In Me.getSubtypeTables
                    Call lrSubtypeTable.removeIndexByRoleConstraint(arRoleConstraint)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub removeSupertypeColumns(ByRef arSubtypeRelationship As FBM.tSubtypeRelationship)

            Try
                For Each lrTable In Me.getSupertypeTables(Nothing, arSubtypeRelationship)
                    For Each lrSupertypeColumn In lrTable.Column
                        Dim lrColumn = Me.Column.Find(AddressOf lrSupertypeColumn.EqualsByRoleActiveRole)
                        If lrColumn IsNot Nothing Then
                            Call Me.removeColumn(lrColumn, True, False)
                        End If
                    Next
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub resetColumnOrdinalPositions()

            Try
                For liInd = 0 To Me.Column.Count - 1
                    Call Me.Column(liInd).setOrdinalPosition(liInd + 1)
                Next
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setIsPGSRelation(ByVal abIsPGSRelation As Boolean)

            Try
                'CodeSafe: Abort if not actually making a change.
                If Me.isPGSRelation = abIsPGSRelation Then Exit Sub 'Nothing to do here

                Me.isPGSRelation = abIsPGSRelation

                If Me.isPGSRelation Then
                    Call Me.Model.Model.addCMMLIsPGSRelation(Me)
                Else
                    Call Me.Model.Model.removeCMMLIsPGSRelation(Me)
                End If

                RaiseEvent IsPGSRelationChanged(abIsPGSRelation)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function PlaysRole(ByVal asRoleName As String) As Boolean

            Try
                Return Me.RolesPlayed.Find(Function(x) x.RoleName = asRoleName) IsNot Nothing

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub setName(ByVal asNewName As String)
            Try
                Call Me.Model.Model.updateCMMLTableName(Me.Name, asNewName)
                Me.Name = asNewName
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub addPrimaryKeyToNonAbsorbedTables(ByRef arPrimaryKeyIndex As RDS.Index, ByVal abIsPreferredIdentifier As Boolean)

            Try
                For Each lrTable In Me.getSubtypeTables(True, True).FindAll(Function(x) x.isAbsorbed = False)

                    Dim larIndexColumn As New List(Of RDS.Column)
                    For Each lrColumn In arPrimaryKeyIndex.Column

                        Dim lrNewColumn = lrColumn.Clone(lrTable) 'Without arRelation, so the Cloned Column.Relation is Nothing, see below.

                        lrNewColumn.Id = System.Guid.NewGuid.ToString
                        lrNewColumn.IsMandatory = True 'To be on the safe side                    
                        lrNewColumn.OrdinalPosition = lrTable.Column.Count + 1

                        If lrTable.Column.Find(AddressOf lrNewColumn.EqualsByRoleActiveRole) IsNot Nothing Then
                            lrNewColumn = lrTable.Column.Find(AddressOf lrNewColumn.EqualsByRoleActiveRole)
                        Else
                            lrTable.addColumn(lrNewColumn)
                        End If

                        lrNewColumn.Relation = New List(Of RDS.Relation) 'Leave here, otherwise Table.addColumn will assign any relation for the Original Column (in the Supertype)

                        larIndexColumn.AddUnique(lrNewColumn)

                    Next

                    For Each lrColumn In Me.Column
                        For Each lrRelation In lrColumn.Relation.ToArray

                            Dim lrNewRelation = lrRelation.Clone(lrTable)

                            lrNewRelation.Id = System.Guid.NewGuid.ToString
                            lrNewRelation.ResponsibleFactType = lrRelation.ResponsibleFactType
                            lrNewRelation.ReverseDestinationColumns.Add(lrColumn)
                            lrNewRelation.ReverseOriginColumns = lrRelation.ReverseOriginColumns

                            Dim lrNewColumn = lrTable.Column.Find(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id)

                            'The NewColumn will have a Relation because cloning the Relation adds the Relation to the NewColum in lrTable.
                            lrNewRelation = lrNewColumn.Relation.Find(AddressOf lrNewRelation.EqualsByOriginColumnsDesinationTable) 'Should be the same Relation
                            If lrNewRelation IsNot Nothing Then
                                If Me.Model.Relation.Find(AddressOf lrNewRelation.EqualsByOriginColumnsDesinationTable) Is Nothing Then
                                    Me.Model.addRelation(lrNewRelation)
                                Else
                                    lrNewColumn.Relation.Remove(lrNewRelation)
                                    lrNewColumn.Relation.Add(Me.Model.Relation.Find(AddressOf lrNewRelation.EqualsByOriginColumnsDesinationTable))
                                End If
                            End If
                        Next
                    Next

                    '------------------------
                    'Index 
                    Dim lrExistingIndex As RDS.Index = lrTable.getIndexByColumns(larIndexColumn)

                    Dim lsQualifier As String = "UC"
                    Dim lsQualifierExtention As String = lrTable.generateUniqueQualifier("UC")

                    If abIsPreferredIdentifier Then
                        lsQualifier = "PK"
                        lsQualifierExtention = lrTable.generateUniqueQualifier("PK")
                    End If

                    If lrExistingIndex Is Nothing Then

                        Dim lbIsPrimaryKey As Boolean = abIsPreferredIdentifier
                        Dim lsIndexName As String = lrTable.Name & "_" & Trim(lsQualifierExtention)

                        'Add the new Index
                        Dim lrIndex As New RDS.Index(lrTable,
                                                 lsIndexName,
                                                 lsQualifier,
                                                 pcenumODBCAscendingOrDescending.Ascending,
                                                 lbIsPrimaryKey,
                                                 True,
                                                 False,
                                                 larIndexColumn,
                                                 False,
                                                 True)

                        Call lrTable.addIndex(lrIndex)
                    Else
                        Call lrExistingIndex.setQualifier(lsQualifier)
                        Call lrExistingIndex.setName(lrTable.Name & "_" & lsQualifierExtention)
                        Call lrExistingIndex.setIsPrimaryKey(True)
                    End If

                    'Recursive
                    Call lrTable.addPrimaryKeyToNonAbsorbedTables(arPrimaryKeyIndex, abIsPreferredIdentifier)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub triggerIndexColumnAdded(ByRef arIndex As RDS.Index, ByRef arColumn As RDS.Column)
            RaiseEvent IndexColumnAdded(arIndex, arColumn)
        End Sub

        Public Sub triggerIndexModified(ByRef arIndex As RDS.Index)
            RaiseEvent IndexModified(arIndex)
        End Sub

        Public Sub triggerSubtypeRelationshipAdded()
            RaiseEvent SubtypeRelationshipAdded()
        End Sub

        Public Sub triggerSubtypeRelationshipRemoved()
            RaiseEvent SubtypeRelationshipRemoved()
        End Sub

    End Class

End Namespace
