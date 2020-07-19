Imports System.Reflection
Imports System.Xml.Serialization

Namespace RDS

    <Serializable()> _
    Public Class Table
        Implements IEquatable(Of RDS.Table)

        <XmlIgnore()> _
        <NonSerialized()> _
        Public Model As RDS.Model

        <XmlAttribute()> _
        Public Name As String

        <XmlElement()> _
        Public Column As New List(Of RDS.Column)

        <XmlElement()> _
        Public WithEvents Index As New List(Of RDS.Index)

        <XmlAttribute()> _
        Public IsSystemTable As Boolean = False

        <XmlAttribute()> _
        Public Remarks As String = ""

        <XmlAttribute()> _
        Public isPGSRelation As Boolean = False

        <XmlIgnore()>
        <NonSerialized()>
        Public WithEvents FBMModelElement As FBM.ModelObject 'The ModelElement that the Table relates to. Could be an EntityType or a FactType.

        Public ReadOnly Property isAbsorbed As Boolean
            Get
                Return Me.FBMModelElement.isAbsorbed
            End Get
        End Property

        Public Event ColumnRemoved(ByVal arColumn As RDS.Column)
        Public Event ColumnAdded(ByRef arColumn As RDS.Column)
        Public Event IndexAdded(ByRef arIndex As RDS.Index)
        Public Event IndexRemoved(ByRef arIndex As RDS.Index)
        Public Event IsPGSRelationChanged(ByVal abNewValue As Boolean)
        Public Event NameChanged(ByVal asNewName As String)


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

        Public Shadows Function Equals(other As Table) As Boolean Implements IEquatable(Of Table).Equals

            Return Me.Name = other.Name

        End Function

        Public Function EqualsByName(ByVal other As RDS.Table) As Boolean

            Return Me.Name = other.Name
        End Function

        Public Shared Function CompareName(x As Table, y As Table) As Integer

            Return String.Compare(x.Name, y.Name)

        End Function

        Public Sub addColumn(ByRef arColumn As RDS.Column)

            arColumn.OrdinalPosition = Me.Column.Count + 1

            Me.Column.AddUnique(arColumn)

            If arColumn.Role.isRDSForeignKeyRole Then

                Dim lrRelation As RDS.Relation = arColumn.Role.belongsToRelation
                If lrRelation IsNot Nothing Then
                    lrRelation.OriginColumns.Add(arColumn)
                    arColumn.Relation.Add(lrRelation)
                End If
            End If

            'Is only IsPGSRelation if the Objectified Fact Type (i.e. The objectified relation) only references ValueTypes. 
            'i.e.Where ObjectifiedFactType Is only joined To ValueTypes. If Is joined To any EntityType Or another ObjectifiedFactType, then cannot be a PGSRelation, And must be a Node.
            If (arColumn.ContributesToPrimaryKey = False) And (TypeOf (arColumn.Role.JoinedORMObject) IsNot FBM.ValueType) Then
                '--------------------------------------------------------------------
                'Remove IsPGSRelation if join is not to a ValueType
                Call Me.setIsPGSRelation(False)
            ElseIf TypeOf (Me.FBMModelElement) Is FBM.FactType Then
                'FBMModelObject is the ModelElement that the Table relates to.
                If Not (arColumn.ContributesToPrimaryKey) And (arColumn.FactType IsNot Me.FBMModelElement) And (TypeOf (arColumn.Role.JoinedORMObject) IsNot FBM.ValueType) Then
                    Call Me.setIsPGSRelation(False)
                End If
            End If

            '------------------------------------------------------------------------------
            'CMML Code
            Call Me.Model.Model.createCMMLAttribute(Me.Name, arColumn.Name, arColumn.Role, arColumn)

            RaiseEvent ColumnAdded(arColumn)

        End Sub

        Public Sub addIndex(ByRef arIndex As RDS.Index)

            Me.Index.AddUnique(arIndex)

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

        End Sub

        Public Function getIncomingRelations() As List(Of RDS.Relation)

            Try
                Dim larIncomingRelations = From Relation In Me.Model.Model.RDS.Relation _
                                           From Column In Relation.DestinationColumns _
                                           Where Column.Table.Name = Me.Name _
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

        Public Function getOutgoingRelations() As List(Of RDS.Relation)

            Try
                Dim larOutgoingRelation = From Relation In Me.Model.Model.RDS.Relation
                                          From Column In Relation.OriginColumns
                                          Where Column.Table.Name = Me.Name
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

        Public Function getPrimaryKeyColumns() As List(Of RDS.Column)

            Try
                Dim larColumn = From Column In Me.Column
                                Where Column.ContributesToPrimaryKey = True
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


        Public Function getSubtypeTables() As List(Of RDS.Table)

            Dim larSubtypeTable As New List(Of RDS.Table)

            Dim larEntityType = Me.FBMModelElement.getSubtypes()

            For Each lrEntityType In larEntityType
                larSubtypeTable.Add(CType(lrEntityType, FBM.EntityType).getCorrespondingRDSTable(True))
            Next

            Return larSubtypeTable

        End Function


        ''' <summary>
        ''' Returns TRUE if the Table has a PrimaryKey with one Column, 
        ''' ELSE returns FALSE
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function hasSingleColumnPrimaryKey() As Boolean

            HasSingleColumnPrimaryKey = False

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
                            lrColumn.Index.Add(lrIndex)

                            lrIndex.Column.Add(lrColumn)
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
                    For Each lrColumn In lrIndex.Column
                        lrColumn.ContributesToPrimaryKey = Not lrRecordset1.EOF
                    Next

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
                    Me.Model.Model.RDS.Index.Add(lrIndex)

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

        Public Function getFirstNonPrimaryKeyColumnOrdinalPosition() As Integer

            Try
                For Each lrColumn In Me.Column.OrderBy(Function(x) x.OrdinalPosition)
                    If lrColumn.ContributesToPrimaryKey = False Then
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

            Me.Name = asNewName

            RaiseEvent NameChanged(asNewName)

        End Sub

        Public Function createUniqueColumnName(ByVal arColumn As RDS.Column, ByVal asColumnName As String, ByVal aiStartingInd As Integer) As String

            Dim lsUniqueColumnName As String = ""

            Dim lsSQLQuery As String = ""

            If aiStartingInd = 0 Then
                lsUniqueColumnName = asColumnName
            Else
                lsUniqueColumnName = asColumnName & aiStartingInd.ToString
            End If

            If arColumn Is Nothing Then
                If Me.Column.FindAll(Function(x) x.Name = lsUniqueColumnName).Count > 0 Then
                    lsUniqueColumnName = Me.createUniqueColumnName(arColumn, asColumnName, aiStartingInd + 1)
                End If
            ElseIf Me.Column.FindAll(Function(x) x.Name = lsUniqueColumnName And x.Id <> arColumn.Id).Count > 0 Then
                lsUniqueColumnName = Me.createUniqueColumnName(arColumn, asColumnName, aiStartingInd + 1)
            End If

            Return lsUniqueColumnName

        End Function

        ''' <summary>
        ''' Used when the User changes a Unique Index (that is not the existing Primary Key) to the Primary Key. Therefore, the existing Primary Key becomes simple a Unique Index (not Primary Key)
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub makeExistingPrimaryKeySimplyUnique()

            Try
                Dim larIndex = From Index In Me.Index _
                               Where Index.IsPrimaryKey = True _
                               Select Index

                If larIndex.Count > 0 Then

                    Dim lrIndex As RDS.Index = larIndex.First

                    lrIndex.setIsPrimaryKey(False)
                    lrIndex.setQualifier(Me.generateUniqueQualifier("UC"))
                    lrIndex.setName(Me.Name & "_" & lrIndex.IndexQualifier)

                    For Each lrColumn In lrIndex.Column
                        Call lrColumn.triggerForceRefreshEvent()
                    Next

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

            If Me.Column.FindAll(Function(x) x.ContributesToPrimaryKey = True).Count = 1 Then

                lrSwitchingColumn = Me.Column.Find(Function(x) x.OrdinalPosition = 1)
                lrColumn = Me.Column.Find(Function(x) x.ContributesToPrimaryKey = True)

                Dim liOriginalOrdinalPosition = lrColumn.OrdinalPosition

                If liOriginalOrdinalPosition <> 1 Then
                    lrColumn.OrdinalPosition = 1
                    Call Me.Model.Model.setCMMLAttributeOrdinalPosition(lrColumn.Id, lrColumn.OrdinalPosition)

                    lrSwitchingColumn.OrdinalPosition = liOriginalOrdinalPosition
                    Call Me.Model.Model.setCMMLAttributeOrdinalPosition(lrSwitchingColumn.Id, lrSwitchingColumn.OrdinalPosition)
                End If
            Else
                Dim larColumn = From Column In Me.Column _
                                Where Column.ContributesToPrimaryKey = True _
                                Select Column

                Dim liOrdinalPosition As Integer = 0

                If larColumn.Count < Me.Column.Count Then

                    For Each lrColumn In larColumn
                        If lrColumn.hasNonPrimaryKeyColumnsAboveIt() Then
                            liOrdinalPosition = Me.getFirstNonPrimaryKeyColumnOrdinalPosition
                            Call lrColumn.moveToOrdinalPosition(liOrdinalPosition)
                        End If
                    Next

                End If
            End If

        End Sub

        ''' <summary>
        ''' Removes a Column from the table.
        ''' </summary>
        ''' <param name="arColumn">The actual Column object to be removed from the Table.</param>
        ''' <remarks></remarks>
        Public Sub removeColumn(ByRef arColumn As RDS.Column)

            Try
                Me.Column.Remove(arColumn)

                Dim lrRemovedColumn As RDS.Column = arColumn
                For Each lrColumn In Me.Column.FindAll(Function(x) x.OrdinalPosition > lrRemovedColumn.OrdinalPosition)
                    lrColumn.OrdinalPosition -= 1
                    Call Me.Model.Model.setCMMLAttributeOrdinalPosition(lrColumn.Id, lrColumn.OrdinalPosition)
                Next

                RaiseEvent ColumnRemoved(arColumn)

                '------------------------------------------------------------------------------
                'CMML Code
                Call Me.Model.Model.removeCMMLAttribute(arColumn.Table.Name, arColumn.Id)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeExistingPrimaryKey()

            Dim larIndex = From Index In Me.Index _
                           Where Index.IsPrimaryKey = True _
                           Select Index

            If larIndex.Count > 0 Then
                Call Me.removeIndex(larIndex.First)
            End If

        End Sub

        Public Sub removeIndex(ByRef arIndex As RDS.Index)

            Call Me.Index.Remove(arIndex)

            '----------------------------------------------------------------------
            'CMML
            Call Me.Model.Model.removeCMMLIndex(arIndex)

            RaiseEvent IndexRemoved(arIndex)

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

        Public Sub addPrimaryKeyToNonAbsorbedTables(ByRef arPrimaryKeyIndex As RDS.Index, ByVal abIsPreferredIdentifier As Boolean)

            Try
                For Each lrTable In Me.getSubtypeTables.FindAll(Function(x) x.isAbsorbed = False)

                    Dim larIndexColumn As New List(Of RDS.Column)
                    For Each lrColumn In arPrimaryKeyIndex.Column
                        Dim lrNewColumn = lrColumn.Clone

                        lrNewColumn.Id = System.Guid.NewGuid.ToString
                        lrNewColumn.IsMandatory = True 'To be on the safe side                    
                        lrNewColumn.OrdinalPosition = lrTable.Column.Count + 1
                        lrNewColumn.Table = lrTable

                        If lrTable.Column.Contains(lrNewColumn) Then
                            lrNewColumn = lrTable.Column.Find(AddressOf lrNewColumn.Equals)
                        Else
                            lrTable.addColumn(lrNewColumn)
                        End If

                        lrNewColumn.Relation = New List(Of RDS.Relation) 'Leave here, otherwise Table.addColumn will assign any relation for the Original Column (in the Supertype)

                        larIndexColumn.AddUnique(lrNewColumn)

                        For Each lrRelation In lrColumn.Relation
                            Dim lrNewRelation = lrRelation.Clone

                            lrNewRelation.Id = System.Guid.NewGuid.ToString
                            lrNewRelation.OriginColumns.AddRange(larIndexColumn)
                            lrNewRelation.OriginTable = lrTable
                            lrNewRelation.ResponsibleFactType = lrRelation.ResponsibleFactType
                            lrNewRelation.ReverseDestinationColumns.Add(lrNewColumn)
                            lrNewRelation.ReverseOriginColumns = lrRelation.ReverseOriginColumns

                            If lrNewColumn.Relation.Find(AddressOf lrNewRelation.EqualsByOriginColumnsDesinationTable) Is Nothing Then
                                lrNewColumn.Relation.Add(lrNewRelation)
                                Me.Model.addRelation(lrNewRelation)
                            End If
                        Next

                    Next

                    '------------------------
                    'Index 
                    Dim lrExistingIndex As RDS.Index = lrTable.getIndexByColumns(larIndexColumn)

                    If lrExistingIndex Is Nothing Then
                        Dim lsQualifier = lrTable.generateUniqueQualifier("PK")
                        Dim lbIsPrimaryKey As Boolean = True
                        Dim lsIndexName As String = lrTable.Name & "_" & Trim(lsQualifier)

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
                        If abIsPreferredIdentifier Then
                            Call lrExistingIndex.setQualifier("PK")
                            Call lrExistingIndex.setName(lrTable.Name & "_PK")
                            Call lrExistingIndex.setIsPrimaryKey(True)
                        Else
                            Call lrExistingIndex.setQualifier("UK")
                            Call lrExistingIndex.setName(lrTable.Name & "_UC")
                            Call lrExistingIndex.setIsPrimaryKey(abIsPreferredIdentifier)
                        End If
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

    End Class

End Namespace
