Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class RoleConstraint
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.RoleConstraint)
        Implements ICloneable
        Implements iModelObject
        Implements iMDAObject
        Implements FBM.iValidationErrorHandler

        <XmlAttribute()> _
        Public Shadows Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsMDAModelElement As Boolean = False
        <XmlAttribute()> _
        Public Property IsMDAModelElement() As Boolean Implements iMDAObject.IsMDAModelElement
            Get
                Return Me._IsMDAModelElement
            End Get
            Set(ByVal value As Boolean)
                Me._IsMDAModelElement = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _RoleConstraintType As pcenumRoleConstraintType
        <XmlAttribute()> _
        Public Overridable Property RoleConstraintType() As pcenumRoleConstraintType
            Get
                Return Me._RoleConstraintType
            End Get
            Set(ByVal value As pcenumRoleConstraintType)
                Me._RoleConstraintType = value
            End Set
        End Property

        <XmlIgnore()> _
        Public _RingConstraintType As pcenumRingConstraintType
        <XmlAttribute()> _
        <Browsable(False), _
        [ReadOnly](True)> _
        Public Overridable Property RingConstraintType() As pcenumRingConstraintType
            Get
                Return Me._RingConstraintType
            End Get
            Set(ByVal value As pcenumRingConstraintType)
                Me._RingConstraintType = value
            End Set
        End Property

        <XmlIgnore()> _
        Public LevelNr As Single = 1 'Only used on 'InternalUniquenessConstraints'. Must be at least 1

        ''' <summary>
        ''' Only used on FrequencyConstraints.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlAttribute()> _
        Public CardinalityRangeType As pcenumCardinalityRangeType

        ''' <summary>
        ''' Used in ValueComparisonConstraints
        ''' </summary>
        ''' <remarks></remarks>
        Public ValueRangeType As pcenumValueRangeType = pcenumValueRangeType.LessThanOrEqual

        ''' <summary>
        ''' Only used on FrequencyConstraints.
        ''' Set in relation to the values of MinimumFrequencyCount and MaximumFrequencyCount and CardinalityRangeType
        ''' Case CardinalityRangeType 
        '''     Case Is = pcenumCardinalityRangeType.LessThanOREqual
        '''        Me.Cardinality = MaximumFrequencyCount   
        '''     Case Is = pcenumCardinalityRangeType.Equal
        '''        Me.Cardinality = (Either) MaximumFrequencyCount or MinimumFrequencyCount...because they will be the same
        '''     Case Is = pcenumCardinalityRangeType.GreaterThanOREqual
        '''        Me.Cardinality = MinimumFrequencyCount
        '''     Case Is = pcenumCardinalityRangeType.Between
        '''        Me.Cardinality = 0
        ''' End Case
        ''' </summary>
        ''' <remarks></remarks>
        <XmlAttribute()> _
        Public Cardinality As Integer = 0

        ''' <summary>
        ''' Only used on 'FrequencyConstraints'.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _MinimumFrequencyCount As Integer = 0
        <XmlAttribute()> _
        <Browsable(False), _
        [ReadOnly](True)> _
        Public Overridable Property MinimumFrequencyCount() As Integer
            Get
                Return _MinimumFrequencyCount
            End Get
            Set(ByVal value As Integer)
                _MinimumFrequencyCount = value
            End Set
        End Property

        ''' <summary>
        ''' Only used on 'FrequencyConstraints'.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _MaximumFrequencyCount As Integer = 0
        <XmlAttribute()>
        <Browsable(False),
        [ReadOnly](True)>
        Public Overridable Property MaximumFrequencyCount() As Integer
            Get
                Return _MaximumFrequencyCount
            End Get
            Set(ByVal value As Integer)
                _MaximumFrequencyCount = value
            End Set
        End Property

        ''' <summary>
        ''' Used on 'ValueConstraints'.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _MinimumValue As String = ""
        <XmlAttribute()>
        <Browsable(False),
        [ReadOnly](True)>
        Public Overridable Property MinimumValue As String
            Get
                Return _MinimumValue
            End Get
            Set(ByVal value As String)
                _MinimumValue = value
            End Set
        End Property

        ''' <summary>
        ''' Used on 'ValueConstraints'.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _MaximumValue As String = ""
        <XmlAttribute()>
        <Browsable(False),
        [ReadOnly](True)>
        Public Overridable Property MaximumValue As String
            Get
                Return _MaximumValue
            End Get
            Set(ByVal value As String)
                _MaximumValue = value
            End Set
        End Property


        <XmlAttribute()> _
        Public IsDeontic As Boolean

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _IsPreferredIdentifier As Boolean = False
        <Browsable(False)> _
        <XmlAttribute()> _
        Public Overridable Property IsPreferredIdentifier() As Boolean
            Get
                Return Me._IsPreferredIdentifier
            End Get
            Set(ByVal value As Boolean)
                Me._IsPreferredIdentifier = value
            End Set
        End Property

        <XmlIgnore()> _
        Public _Role As New List(Of FBM.Role)
        <XmlIgnore()> _
        Public Property Role As List(Of FBM.Role)
            Get
                Dim larRole As New List(Of FBM.Role)
                For Each lrRoleConstraintRole In Me.RoleConstraintRole
                    larRole.Add(lrRoleConstraintRole.Role)
                Next
                Return larRole
            End Get
            Set(value As List(Of FBM.Role))
                Me._Role = value
            End Set
        End Property

        <XmlElement()>
        Public RoleConstraintRole As New List(Of FBM.RoleConstraintRole)

        <XmlIgnore()>
        Public ReadOnly Property FirstRoleConstraintRole As FBM.Role
            Get
                Return Me.RoleConstraintRole(0).Role
            End Get
        End Property

        Public ReadOnly Property FirstRoleConstraintRoleFactType As FBM.FactType
            Get
                Return Me.RoleConstraintRole(0).Role.FactType
            End Get
        End Property

        ''' <summary>
        ''' List of Arguments for the RoleConstraint, if the RoleConstraint is of a type that has Arguments.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlElement()> _
        Public Argument As New List(Of FBM.RoleConstraintArgument)

        ''' <summary>
        ''' Only used when creating a new Argument dynamically. Once the Argument is created, can be added to Me.Argument.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public CurrentArgument As FBM.RoleConstraintArgument  'See also: frmDiagramORM.DiagramView.MouseDown for saving CurrentArgument to a RoleConstraint

        <XmlIgnore()> _
        Public ReadOnly Property HasModelError() As Boolean Implements iValidationErrorHandler.HasModelError
            Get
                Return Me.ModelError.Count > 0
            End Get
        End Property

        <XmlIgnore()> _
        Private _ModelError As New List(Of FBM.ModelError)
        Public Property ModelError() As System.Collections.Generic.List(Of ModelError) Implements iValidationErrorHandler.ModelError
            Get
                Return Me._ModelError
            End Get
            Set(ByVal value As System.Collections.Generic.List(Of ModelError))
                Me._ModelError = value
            End Set
        End Property

#Region "Value Constraint"
        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ValueConstraint As New List(Of FBM.Concept)

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ValueConstraintList As New Viev.Strings.StringCollection
        '<XmlIgnore()> _
        <CategoryAttribute("Value Type"),
         Browsable(True),
         [ReadOnly](False),
         DescriptionAttribute("The List of Values that Objects of this Value Type may take."),
         Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Property ValueConstraint() As Viev.Strings.StringCollection 'StringCollection 
            '   DefaultValueAttribute(""), _
            '   BindableAttribute(True), _
            '   DesignOnly(False), _
            Get
                Return Me._ValueConstraintList
            End Get
            Set(ByVal Value As Viev.Strings.StringCollection)
                Me._ValueConstraintList = Value
                '----------------------------------------------------
                'Update the set of Concepts/Symbols/Values
                '  within the 'value_constraint' for this ValueType.
                '----------------------------------------------------
                Dim lsString As String
                For Each lsString In Me._ValueConstraintList
                    Dim lrConcept As New FBM.Concept(lsString)
                    If Me._ValueConstraint.Contains(lrConcept) Then
                        '-------------------------------------------------
                        'Nothing to do, because the Concept/Symbol/Value
                        '  already exists for the 'value_constraint'
                        '  for this ValueType.
                        '-------------------------------------------------
                    Else
                        '-------------------------------------------
                        'Add the Concept/Symbol/Value to the Model
                        '-----------------------------------------
                        Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, lrConcept.Symbol, pcenumConceptType.Value)
                        Me.Model.AddModelDictionaryEntry(lrModelDictionaryEntry)
                        '-----------------------------------------
                        'Add the Concept/Symbol/Value to the
                        '  'value_constraint' for this ValueType
                        '-----------------------------------------
                        Me._ValueConstraint.Add(lrConcept)
                    End If
                Next
            End Set
        End Property
#End Region

        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded

        Public Event CardinalityChanged(ByVal aiNewCardinality As Integer)
        Public Event RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean) Implements iModelObject.RemovedFromModel
        Public Event RoleConstraintTypeChanged(ByVal aiNewRoleConstraintType As pcenumRoleConstraintType)
        Public Event RingConstraintTypeChanged(ByVal aiNewRingConstraintType As pcenumRingConstraintType)
        Public Event LevelNrChanged(ByVal aiNewLevelNr As Integer)
        Public Event CardinalityRangeTypeChanged(ByVal aiNewCardinalityRangeType As pcenumCardinalityRangeType)
        Public Event MaximumFrequencyCountChanged(ByVal aiNewMaximumFrequencyCount As Integer)
        Public Event MaximumValueChanged(ByVal aiMaximumValue As String)
        Public Event MinimumFrequencyCountChanged(ByVal aiNewMinimumFrequencyCount As Integer)
        Public Event MinimumValueChanged(ByVal aiMaximumValue As String)
        Public Event IsDeonticChanged(ByVal abNewIsDeontic As Boolean)
        Public Event IsPreferredIdentifierChanged(ByVal abNewIsPreferredIdentifier As Boolean)
        Public Event RoleConstraintRoleAdded(ByRef arRoleConstraintRole As FBM.RoleConstraintRole, ByRef arSubtypeRelationship As FBM.tSubtypeRelationship)
        Public Event RoleConstraintRoleRemoved(ByVal arRoleConstraintRole As FBM.RoleConstraintRole)
        Public Event ArgumentRemoved(ByRef arRoleConstraintArgument As FBM.RoleConstraintArgument)
        Public Event ValueConstraintAdded(ByVal asNewValueConstraint As String)
        Public Event ValueConstraintRemoved(ByVal asRemovedValueConstraint As String)
        Public Event ValueConstraintModified(ByVal asOldValue As String, ByVal asNewValue As String)
        Public Event ValueRangeTypeChanged(ByVal aiNewValueRangeType As pcenumValueRangeType)


        Sub New()
            '------------------------------------------
            'Set the unique key for the RoleConstraint
            '------------------------------------------
            Id = System.Guid.NewGuid.ToString
            Me.ConceptType = pcenumConceptType.RoleConstraint

        End Sub

        Public Sub New(ByRef arModel As FBM.Model)
            Me.New()
            Me.Model = arModel
        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       ByVal asRoleConstraintName As String,
                       ByVal abUseNameAsId As Boolean,
                       Optional ByVal aiRoleConstraintType As pcenumRoleConstraintType = Nothing,
                       Optional ByRef aarRole As List(Of FBM.Role) = Nothing,
                       Optional ByVal abMakeDirty As Boolean = False)

            Me.New()

            Dim lrRole As FBM.Role
            Dim liCounter As Integer = 0
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            Me.Model = arModel
            Me.Name = asRoleConstraintName
            If abUseNameAsId Then
                Me.Id = Me.Name
            End If
            Me.isDirty = abMakeDirty

            If IsSomething(aiRoleConstraintType) Then
                Me.RoleConstraintType = aiRoleConstraintType
            End If

            '-----------------------------------------------------------
            'Create the RoleConstraintRole group for the RoleConstraint
            '-----------------------------------------------------------
            If IsSomething(aarRole) Then
                '------------------------------------------------------------
                'Establish the link between each role and the RoleConstraint
                '------------------------------------------------------------            
                For Each lrRole In aarRole
                    If IsSomething(lrRole) Then
                        liCounter += 1
                        Me.Role.Add(lrRole)

                        '------------------------------------------------------------------------
                        'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                        '------------------------------------------------------------------------
                        lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, Me, False, False, liCounter, abMakeDirty)

                        '----------------------------------------------------
                        'Attach the RoleConstraintRole to the RoleConstraint
                        '----------------------------------------------------
                        Me.RoleConstraintRole.Add(lrRoleConstraintRole)

                        '------------------------------------------
                        'Attach the RoleConstraintRole to the Role
                        '------------------------------------------
                        lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)
                    End If
                Next

            End If

        End Sub

        ''' <summary>
        ''' PRECONDITIONS: If aarRole is populated, all Roles in aarRole have the same FactType.
        ''' </summary>
        ''' <param name="arModel"></param>
        ''' <param name="aiRoleConstraintType"></param>
        ''' <param name="aarRole"></param>
        ''' <param name="abAddToModel"></param>
        ''' <param name="aiLevelNr"></param>
        ''' <remarks></remarks>
        Sub New(ByRef arModel As FBM.Model,
                ByVal aiRoleConstraintType As pcenumRoleConstraintType,
                Optional ByRef aarRole As List(Of FBM.Role) = Nothing,
                Optional ByVal abAddToModel As Boolean = Nothing,
                Optional ByVal aiLevelNr As Integer = Nothing,
                Optional ByVal abMakeDirty As Boolean = False)

            Call Me.New()

            Dim liCounter As Integer = 0
            Dim lrRole As FBM.Role
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            '------------------------------------------
            'Set the unique key for the RoleConstraint
            '------------------------------------------
            Me.Id = System.Guid.NewGuid.ToString
            Me.Model = arModel
            Me.RoleConstraintType = aiRoleConstraintType
            Me.Name = Me.RoleConstraintType.ToString
            Me.isDirty = abMakeDirty

            Select Case aiRoleConstraintType
                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                    Me.ConceptType = pcenumConceptType.RoleConstraint
                    Me.Name = "InternalUniquenessConstraint" & arModel.RoleConstraint.Count + 1
                    If IsSomething(aiLevelNr) Then
                        Me.LevelNr = aiLevelNr
                    End If
                Case Else
                    Me.ConceptType = pcenumConceptType.RoleConstraint
            End Select

            '-----------------------------------------------------------
            'Create the RoleConstraintRole group for the RoleConstraint
            '-----------------------------------------------------------
            If IsSomething(aarRole) Then
                '-------------------------------------------------------------------
                'Throw an exception if this constructor has been called for any
                '  other than a UniquenessConstraint, because just Role information
                '  will not be enough for other types of constraints (i.e. need
                '  'Entry' and 'Exit' information.
                '-------------------------------------------------------------------
                Select Case aiRoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint, _
                              pcenumRoleConstraintType.ExternalUniquenessConstraint
                        '---------------------------------
                        'Is OK, is Uniqueness Constraint
                        '---------------------------------
                    Case Else
                        Throw New System.Exception("RoleConstraint.New: Can only use this constructor with Uniqueness constraints.")
                End Select

                '------------------------------------------------------------
                'Establish the link between each role and the RoleConstraint
                '------------------------------------------------------------            
                For Each lrRole In aarRole
                    liCounter += 1
                    Me.Role.Add(lrRole)

                    '------------------------------------------------------------------------
                    'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                    '------------------------------------------------------------------------
                    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, Me, False, False, liCounter, abMakeDirty)

                    '----------------------------------------------------
                    'Attach the RoleConstraintRole to the RoleConstraint
                    '----------------------------------------------------
                    Me.RoleConstraintRole.Add(lrRoleConstraintRole)

                    '------------------------------------------
                    'Attach the RoleConstraintRole to the Role
                    '------------------------------------------
                    lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)
                Next

            End If

            '-----------------------------------------------
            'Add the RoleConstraint to the underlying Model
            '-----------------------------------------------
            If IsSomething(abAddToModel) Then
                If abAddToModel Then
                    Me.Model.AddRoleConstraint(Me)
                End If
            End If

        End Sub

        ''' <summary>
        ''' Constructor used for a FrequencyConstraint type RoleConstraint.
        ''' </summary>
        ''' <param name="arModel"></param>
        ''' <param name="aiRoleConstraintType"></param>
        ''' <param name="aarRole"></param>
        ''' <param name="aiCardinality"></param>
        ''' <param name="aiCardinalityRangeType"></param>
        ''' <param name="abAddToModel"></param>
        ''' <remarks></remarks>
        Sub New(ByVal arModel As FBM.Model,
                ByVal aiRoleConstraintType As pcenumRoleConstraintType,
                ByRef aarRole As List(Of FBM.Role),
                ByVal aiCardinality As Integer,
                ByVal aiCardinalityRangeType As pcenumCardinalityRangeType,
                Optional ByVal abAddToModel As Boolean = Nothing,
                Optional ByVal abMakeDirty As Boolean = False)

            Me.New()

            Dim lrRole As FBM.Role
            Dim liCounter As Integer
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            Me.Model = arModel
            Me.RoleConstraintType = aiRoleConstraintType
            Me.Cardinality = aiCardinality
            Me.CardinalityRangeType = aiCardinalityRangeType
            Me.isDirty = abMakeDirty

            For Each lrRole In aarRole
                liCounter += 1
                Me.Role.Add(lrRole)

                '------------------------------------------------------------------------
                'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                '------------------------------------------------------------------------
                lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, Me, False, False, liCounter, abMakeDirty)

                '----------------------------------------------------
                'Attach the RoleConstraintRole to the RoleConstraint
                '----------------------------------------------------
                Me.RoleConstraintRole.Add(lrRoleConstraintRole)

                '------------------------------------------
                'Attach the RoleConstraintRole to the Role
                '------------------------------------------
                lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)
            Next

            '-----------------------------------------------
            'Add the RoleConstraint to the underlying Model
            '-----------------------------------------------
            If IsSomething(abAddToModel) Then
                If abAddToModel Then
                    Me.Model.AddRoleConstraint(Me)
                End If
            End If

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.RoleConstraint) As Boolean Implements System.IEquatable(Of FBM.RoleConstraint).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByName(ByVal other As FBM.RoleConstraint) As Boolean

            If Me.Name = other.Name Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsByRoleConstraintType(ByVal other As FBM.RoleConstraint) As Boolean

            If Me.RoleConstraintType = other.RoleConstraintType Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overrides Function EqualsBySignature(ByVal other As FBM.ModelObject) As Boolean

            If other.GetSignature = Me.GetSignature Then
                Return True
            Else
                Return False
            End If

        End Function


        Public Function EqualsByRoleConstraintRoleGroup(ByVal other As FBM.RoleConstraint) As Boolean

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                If Not other.RoleConstraintRole.Exists(AddressOf lrRoleConstraintRole.EqualsByRole) Then
                    Return False
                End If
            Next

            Return True

        End Function

        Public Sub AddArgument(ByRef arArgument As FBM.RoleConstraintArgument, ByVal abBroadcastInterfaceEvent As Boolean)

            arArgument.isDirty = True

            Me.Argument.Add(arArgument)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelAddRoleConstraintArgument,
                                                                    arArgument,
                                                                    Nothing)
            End If

        End Sub

        Public Sub AddRoleConstraintRole(ByRef arRoleConstraintRole As FBM.RoleConstraintRole,
                                         Optional arSubtypeRelationship As FBM.tSubtypeRelationship = Nothing)

            Try
                Me.RoleConstraintRole.Add(arRoleConstraintRole)

                '=======================================================================
                'RDS
                '------------------------
                'Index 
                If Me.RoleConstraintType = pcenumRoleConstraintType.ExternalUniquenessConstraint _
                    And Me.RoleConstraintRole.Count > 1 Then

                    If Me.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then

                        Dim lrNewRole As FBM.Role = arRoleConstraintRole.Role
                        Dim lrOtherRoleOfFactType = arRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(arRoleConstraintRole.Role.Id)

                        If lrOtherRoleOfFactType.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                            '------------------------------------------------------------
                            'Unique identifier for an Entity Type
                            '--------------------------------------
                            Dim lrEntityType As FBM.EntityType
                            lrEntityType = lrOtherRoleOfFactType.JoinedORMObject

                            Dim lrTable As RDS.Table = Me.Model.RDS.Table.Find(Function(x) x.Name = lrEntityType.Name)

                            Dim larIndexColumn As New List(Of RDS.Column)

                            'Find new Column
                            Dim larColumn = From Column In lrTable.Column
                                            Where Column.Role.Id = lrOtherRoleOfFactType.Id
                                            Select Column

                            If larColumn.Count > 0 Then

                                Dim lrNewColumn As RDS.Column = larColumn.First

                                For Each lrRoleConstraintRole In Me.RoleConstraintRole.FindAll(Function(x) x.Role.Id <> lrNewRole.Id)

                                    larColumn = From Column In lrTable.Column
                                                Where Column.Role.Id = lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id).Id
                                                Select Column

                                    For Each lrColumn In larColumn
                                        larIndexColumn.Add(lrColumn)
                                    Next 'Column

                                Next 'RoleConstraintRole

                                Dim lrIndex As RDS.Index

                                '------------------------------------------------------------------------
                                'FInd any existing Index for the previous version of the RoleConstraint
                                lrIndex = lrTable.getIndexByColumns(larIndexColumn)

                                larIndexColumn.Add(lrNewColumn)

                                If lrIndex Is Nothing Then
                                    'No existing Index for the previous version of the RoleConstraint (i.e. Minus the new RoleConstraintRole)
                                    Dim lsQualifier As String
                                    If arRoleConstraintRole.RoleConstraint.IsPreferredIdentifier Then
                                        lsQualifier = lrTable.generateUniqueQualifier("PK")
                                    Else
                                        lsQualifier = lrTable.generateUniqueQualifier("UC")
                                    End If

                                    Dim lsIndexName As String = lrTable.Name & "_" & Trim(lsQualifier)

                                    'Add the new Index
                                    lrIndex = New RDS.Index(lrTable,
                                                            lsIndexName,
                                                            lsQualifier,
                                                            pcenumODBCAscendingOrDescending.Ascending,
                                                            arRoleConstraintRole.RoleConstraint.IsPreferredIdentifier,
                                                            True,
                                                            False,
                                                            larIndexColumn,
                                                            False,
                                                            True)

                                    Call lrTable.addIndex(lrIndex, Me)

                                    For Each lrSubtypeTable In lrTable.getSubtypeTables.FindAll(Function(x) x.isAbsorbed = False)
                                        Call lrTable.addPrimaryKeyToNonAbsorbedTables(lrIndex, False)
                                    Next
                                Else
                                    'Just add the Column (for the RoleConstraintRole) to the Index
                                    Dim lrColumn As RDS.Column
                                    lrColumn = lrTable.Column.Find(Function(x) x.Role.Id = lrOtherRoleOfFactType.Id)
                                    lrIndex.addColumn(lrColumn)
                                End If

                            End If 'There is a Column to make the index for.
                        End If
                    End If

                End If 'Is ExternalUniquenessConstraint.

                RaiseEvent RoleConstraintRoleAdded(arRoleConstraintRole, arSubtypeRelationship)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub AddValueConstraint(ByVal asValueConstraint As String)

            Me.ValueConstraint.Add(asValueConstraint)
            Call Me.makeDirty()

            RaiseEvent ValueConstraintAdded(asValueConstraint)

        End Sub

        Public Function atLeastOneRoleJoinsAValueType() As Boolean

            Return Me.RoleConstraintRole.FindAll(Function(x) x.Role.JoinedORMObject.ConceptType = pcenumConceptType.ValueType).Count > 0

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        Public Sub ChangeRoleConstraintType(ByVal aiNewRoleConstraintType As pcenumRoleConstraintType)

            Me.RoleConstraintType = aiNewRoleConstraintType

            RaiseEvent RoleConstraintTypeChanged(aiNewRoleConstraintType)

        End Sub

        ''' <summary>
        ''' Used to 'Sort' Enumerated lists of tRoleConstraint
        ''' </summary>
        ''' <param name="ao_a"></param>
        ''' <param name="ao_b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CompareRoleConstraintNames(ByVal ao_a As FBM.RoleConstraint, ByVal ao_b As FBM.RoleConstraint) As Integer

            Return StrComp(ao_a.Name, ao_b.Name, CompareMethod.Text)

        End Function

        Public Sub SetCardinality(ByVal aiCardinalityRangeType As pcenumCardinalityRangeType, ByVal aiCardinality As Integer, ByVal aiMinimumFrequencyCount As Integer, ByVal aiMaximumFrequencyCount As Integer)

            Try
                Me.SetCardinalityRangeType(aiCardinalityRangeType)
                Me.SetCardinality(aiCardinality)
                Me.SetMinimumFrequencyCount(aiMinimumFrequencyCount)
                Me.SetMaximumFrequencyCount(aiMaximumFrequencyCount)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Function DoRoleConstraintRolesMeetTypeOfConstraint(ByRef arModelErrors As List(Of FBM.ModelError)) As Boolean

            'Dim lrModelError As tModelError
            DoRoleConstraintRolesMeetTypeOfConstraint = True

            Select Case Me.RoleConstraintType
                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                    '--------------------------------------------------------------------
                    'Check to see that all RoleConstraintRoles are of the same FactType
                    '--------------------------------------------------------------------
                    If Me.IsEachRoleInTheSameFactType() Then
                        '------------------------------------
                        'All good, that's what we are after
                        '------------------------------------
                    Else
                        DoRoleConstraintRolesMeetTypeOfConstraint = False
                    End If
                Case Is = pcenumRoleConstraintType.SubsetConstraint
                    '------------------------------------------------------------------------
                    'Check to see that each In/Out combination (of the RoleConstraintRoles)
                    '  have Roles that join the same ModelObject
                    '------------------------------------------------------------------------

                    '-------------------------------------------------------------------
                    'If the SubsetConstraint is a SubsetJoinConstraint, check to see 
                    '  that there is a JoinPath for the set of RoleConstraintRoles
                    '-------------------------------------------------------------------
            End Select

            Return DoRoleConstraintRolesMeetTypeOfConstraint

        End Function

        Public Function IsEachRoleFactTypeBinary() As Boolean

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            IsEachRoleFactTypeBinary = True

            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                If lrRoleConstraintRole.Role.FactType.Arity <> 2 Then
                    IsEachRoleFactTypeBinary = False
                End If
            Next

            Return IsEachRoleFactTypeBinary

        End Function

        ''' <summary>
        ''' Returns True if each Role/FactType (from RoleConstraintRole) is binary, and each opposite Role in that binary FactType joins the same ModelObject, for all RoleConstraintRoles for this RoleConstraint;
        ''' else returns False.
        ''' </summary>
        ''' <returns>True if each Role/FactType (from RoleConstraintRole) is binary, and each opposite Role in that binary FactType joins the same ModelObject, for all RoleConstraintRoles for this RoleConstraint;
        ''' else returns False.</returns>
        ''' <remarks>A Role/FactType is the FactType of the Role of a RoleConstraintRole</remarks>
        Public Function DoesEachRoleFactTypeOppositeRoleJoinSameModelObject() As Boolean

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            Dim lrFirstModelObject As New FBM.ModelObject
            Dim liInd As Integer = 0

            DoesEachRoleFactTypeOppositeRoleJoinSameModelObject = True

            If Me.IsEachRoleFactTypeBinary Then
                For Each lrRoleConstraintRole In Me.RoleConstraintRole
                    If liInd = 0 Then
                        lrFirstModelObject = lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id).JoinedORMObject
                    End If
                    If lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id).JoinedORMObject Is lrFirstModelObject Then
                        '-------------------------------------
                        'All good, that's what we are after.
                        '-------------------------------------
                    Else
                        DoesEachRoleFactTypeOppositeRoleJoinSameModelObject = False
                    End If

                    liInd += 1
                Next
            Else
                DoesEachRoleFactTypeOppositeRoleJoinSameModelObject = False
            End If

            Return DoesEachRoleFactTypeOppositeRoleJoinSameModelObject

        End Function

        Private Function IsEachRoleInTheSameFactType() As Boolean

            Dim lrFirstFactType As New FBM.FactType
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            Dim liInd As Integer = 0

            IsEachRoleInTheSameFactType = True

            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                If liInd = 0 Then
                    lrFirstFactType = lrRoleConstraintRole.Role.FactType
                End If
                If lrRoleConstraintRole.Role.FactType Is lrFirstFactType Then
                    '-------------------------------------
                    'All good, that's what we are after.
                    '-------------------------------------
                Else
                    IsEachRoleInTheSameFactType = False
                End If
                liInd += 1
            Next

            Return IsEachRoleInTheSameFactType

        End Function

        ''' <summary>
        ''' RETURNS TRUE if the RoleConstraint implies a single Column for a RDS Table, else RETURNS FALSE
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function impliesSingleColumnForRDSTable() As Boolean

            Dim lrRole As FBM.Role

            Try
                If Me.RoleConstraintType <> pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    Return False
                Else
                    If Me.RoleConstraintRole.Count > 1 Then
                        '------------------------------------------------------------------------------------
                        'Can't imply one Column, because the RoleConstraint spans more than one Column
                        Return False
                    Else

                        If Me.RoleConstraintRole.Count = 0 Then Return False

                        lrRole = Me.RoleConstraintRole(0).Role

                        If lrRole.FactType.IsLinkFactType Then
                            'Columns are not created for LinkFactTypes. But Relations are created for LinkFactTypes
                            Return False
                        Else
                            Select Case lrRole.TypeOfJoin
                                Case Is = pcenumRoleJoinType.ValueType 'ValueType                    
                                    Return False
                                Case Is = pcenumRoleJoinType.EntityType
                                    If lrRole.FactType.Is1To1BinaryFactType Then
                                        'Rule 4
                                        If IsSomething(lrRole.JoinsEntityType.ReferenceModeFactType) Then
                                            If lrRole.FactType.Id = lrRole.JoinsEntityType.ReferenceModeFactType.Id Then
                                                '---------------------------------------------------
                                                'Is Role on ReferenceModeFactType
                                                '----------------------------------
                                                If lrRole.JoinsEntityType.ReferenceModeRoleConstraint.Role(0).Id = lrRole.Id Then
                                                    Return False
                                                Else
                                                    Return True
                                                End If
                                            Else
                                                Dim lrModelElement As FBM.ModelObject = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject
                                                Select Case lrModelElement.ConceptType
                                                    Case Is = pcenumConceptType.ValueType
                                                        Return True
                                                    Case Is = pcenumConceptType.EntityType
                                                        Dim lrEntityType As FBM.EntityType = lrModelElement
                                                        If lrEntityType.HasCompoundReferenceMode Then
                                                            Return False 'Because implied more than one Column
                                                        Else
                                                            Return True
                                                        End If
                                                End Select
                                            End If
                                        Else
                                            Return True
                                        End If
                                    ElseIf lrRole.FactType.Arity = 1 Then
                                        'Is Unary Role, so must be a Property
                                        Return True
                                    ElseIf (lrRole.FactType.HasTotalRoleConstraint And lrRole.FactType.Arity > 1) _
                                        Or lrRole.FactType.HasPartialButMultiRoleConstraint _
                                        Or lrRole.FactType.Arity = Me.RoleConstraintRole.Count Then
                                        Return False
                                    ElseIf lrRole.FactType.IsObjectified Then
                                        Return False
                                    ElseIf lrRole.FactType.IsBinaryFactType Then
                                        'Rule 2
                                        Dim lrModelElement As FBM.ModelObject = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject
                                        Select Case lrModelElement.ConceptType
                                            Case Is = pcenumConceptType.ValueType
                                                Return True
                                            Case Is = pcenumConceptType.EntityType
                                                Dim lrEntityType As FBM.EntityType = lrModelElement
                                                If lrEntityType.ConceptType = pcenumConceptType.EntityType _
                                                    And lrEntityType.HasCompoundReferenceMode Then
                                                    Return False 'Because implies more than one Column
                                                Else
                                                    Return True
                                                End If
                                            Case Is = pcenumConceptType.FactType
                                                Return False 'Because likely implies more than one Column
                                            Case Else
                                                Throw New Exception("Role doesn't reference an EntityType, ValueType or FactType.")
                                        End Select

                                    End If
                                Case Is = pcenumRoleJoinType.FactType
                                    'ObjectifiedFactType
                                    If lrRole.FactType.Arity = 1 Then
                                        Return True
                                    ElseIf (lrRole.FactType.HasTotalRoleConstraint And lrRole.FactType.Arity > 1) _
                                        Or lrRole.FactType.Arity = Me.RoleConstraintRole.Count Then
                                        Return False
                                    ElseIf lrRole.FactType.IsObjectified Then
                                        Return False
                                    ElseIf lrRole.FactType.IsBinaryFactType Then
                                        Return True
                                    End If
                            End Select

                        End If

                    End If
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

        Public Function IsReferenceSchemePreferredIdentifierRC() As Boolean

            Try

                Dim larEntityType = From EntityType In Me.Model.EntityType _
                                    Where EntityType.PreferredIdentifierRCId = Me.Id _
                                    Select EntityType

                If larEntityType.Count > 0 Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function isSubtypeRelationshipFactTypeIUConstraint() As Boolean

            If Not Me.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                Return False
            ElseIf Me.RoleConstraintRole.Count > 0 Then
                Return Me.Role(0).FactType.IsSubtypeRelationshipFactType                
            Else
                Return False
            End If

            Return False

        End Function

        Public Function getArgument(ByVal aiArgumentNr As Integer) As FBM.RoleConstraintArgument

            Try
                If Me.Argument.Count = 0 Then
                    Throw New Exception("There are no Arguments for Role Constraint, '" & Me.Id & "'.")
                ElseIf aiArgumentNr > Me.Argument.Count Then
                    Throw New Exception("Argument # " & aiArgumentNr & " does not exist for Role Constraint, '" & Me.Id & "'.")
                End If

                Return Me.Argument.Find(Function(x) x.SequenceNr = aiArgumentNr)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function GetCommonArgumentModelObjects() As List(Of FBM.ModelObject)

            Try
                Dim larModelObject As New List(Of FBM.ModelObject)
                Dim larSecondaryList As New List(Of FBM.ModelObject)
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                If Me.Argument.Count > 0 Then
                    For Each lrRoleConstraintRole In Me.Argument(0).RoleConstraintRole
                        larModelObject.Add(lrRoleConstraintRole.Role.JoinedORMObject)
                    Next
                    'VM-20180315-Remove this if all seems okay
                    'For Each lrArgument In Me.Argument.FindAll(Function(x) x.RoleConstraintRole.Count > 0)
                    '    If Not larModelObject.Contains(lrArgument.RoleConstraintRole(0).Role.JoinedORMObject) Then
                    '        larModelObject.Add(lrArgument.RoleConstraintRole(0).Role.JoinedORMObject)
                    '    End If
                    'Next
                Else
                    For Each lrRoleConstraintRole In Me.RoleConstraintRole
                        larModelObject.AddUnique(lrRoleConstraintRole.Role.JoinedORMObject)
                    Next
                End If

                '-------------------------------------------------------------------------------------------------
                'Leave any Subtypes out of the list, because they must be Subtypes of other members of the list.
                '  i.e. e.g. As used in Verbalisations requiring e.g. "some Employee that is that Person"
                '-------------------------------------------------------------------------------------------------
                'See ModelObjectIsSubtypeOfModelObject
                Dim lrModelObject As FBM.ModelObject
                Dim lrCandidateSupertypeMO As FBM.ModelObject
                For Each lrModelObject In larModelObject
                    larSecondaryList.Add(lrModelObject)
                    For Each lrCandidateSupertypeMO In larModelObject.FindAll(Function(x) x.Id <> lrModelObject.Id)
                        If Me.Model.ModelObjectIsSubtypeOfModelObject(lrModelObject, lrCandidateSupertypeMO) Then
                            '----------------------------------------
                            'Leave the ModelObject out of the list.
                            '----------------------------------------
                            larSecondaryList.Remove(lrModelObject)
                        End If
                    Next
                Next

                If larModelObject.Count = 1 Then
                    larSecondaryList.AddUnique(larModelObject(0))
                End If

                Return larSecondaryList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.ModelObject)
            End Try

        End Function

        Public Function getCorrespondingIndex() As RDS.Index

            Try
                If Me.RoleConstraintType <> pcenumRoleConstraintType.ExternalUniquenessConstraint Then
                    Throw New Exception("Function is only for External Uniqueness Constraints.")
                End If

                Dim lrTable As RDS.Table = Nothing
                Dim lrIndex As RDS.Index
                Dim larIndexColumn As New List(Of RDS.Column)

                Dim lrOtherRoleOfFactType = Me.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(Me.RoleConstraintRole(0).Role.Id)

                If lrOtherRoleOfFactType.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    '------------------------------------------------------------
                    'Unique identifier for an Entity Type
                    '--------------------------------------
                    Dim lrEntityType As FBM.EntityType
                    lrEntityType = lrOtherRoleOfFactType.JoinedORMObject

                    lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = lrEntityType.Name)
                End If

                If lrTable Is Nothing Then
                    Return Nothing
                End If

                For Each lrRoleConstraintRole In Me.RoleConstraintRole

                    Dim larColumn = From Column In lrTable.Column _
                                    Where Column.Role.Id = lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id).Id _
                                    Select Column

                    For Each lrColumn In larColumn
                        larIndexColumn.Add(lrColumn)
                    Next 'Column

                Next 'RoleConstraintRole

                '------------------------------------------------------------------------
                'FInd any existing Index for the previous version of the RoleConstraint
                lrIndex = lrTable.getIndexByColumns(larIndexColumn)

                Return lrIndex

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Returns the CQL for the EntityType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCQLText() As String

            Try
                Dim lsCQLStatement As String = ""
                Dim lrFactType As FBM.FactType
                Dim lrFactTypeReading As FBM.FactTypeReading
                Dim liInd As Integer

                Select Case Me.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.ExclusiveORConstraint

                        lsCQLStatement = "for each " & Me.RoleConstraintRole(0).Role.JoinedORMObject.Name & " exactly one of these holds:" & vbCrLf

                        liInd = 0
                        For Each lrRoleConstraintRole In Me.RoleConstraintRole
                            lrFactType = lrRoleConstraintRole.Role.FactType
                            Dim larRole As New List(Of FBM.Role)
                            larRole.Add(lrRoleConstraintRole.Role)
                            lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole, False)
                            If liInd > 0 Then lsCQLStatement.AppendString("," & vbCrLf)
                            lsCQLStatement.AppendString(vbTab & lrFactTypeReading.GetReadingTextCQL(False, True))
                            liInd += 1
                        Next

                        lsCQLStatement.AppendString(";")
                End Select

                Return lsCQLStatement

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return "Error generating CQL for Entity Type: " & Me.Id
            End Try

        End Function


        ''' <summary>
        ''' Returns the count of Arguments already created for the RoleConstraint plus one.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNextArgumentSequenceNr() As Integer

            Return Me.Argument.Count + 1

        End Function

        ''' <summary>
        ''' Returns the unique Signature of the RoleConstraint
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetSignature() As String

            Dim lsSignature As String

            lsSignature = Me.Id

            Return lsSignature

        End Function


        Function ExistsRoleConstraintRoleForModelObject(ByVal aoModelObject As FBM.ModelObject, Optional ByVal abIsExit As Boolean = Nothing) As Boolean

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole


            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                If lrRoleConstraintRole.Role.JoinedORMObject Is aoModelObject Then
                    If IsSomething(abIsExit) Then
                        If lrRoleConstraintRole.IsExit Then
                            Return True
                        Else
                            Return False
                        End If

                    Else
                        Return True
                    End If
                End If
            Next

            Return False

        End Function

        Public Shadows Function Clone(ByRef arModel As FBM.Model, _
                                      Optional abAddToModel As Boolean = False, _
                                      Optional ByVal abIsMDAModelElement As Boolean = False) As Object 'Implements ICloneable.Clone

            Dim lrRole As FBM.Role
            Dim lrRoleConstraint As New FBM.RoleConstraint
            Dim lrRoleConstraintRole As New FBM.RoleConstraintRole

            Try
                If arModel.RoleConstraint.Exists(AddressOf Me.Equals) Then
                    '---------------------------------------------------------------------------------------------------------------------
                    'The target RoleConstraint already exists in the target Model, so return the existing RoleConstraint (from the target Model)
                    '  20150127-There seems no logical reason to clone an RoleConstraint to a target Model if it already exists in the target
                    '  Model. This method is used when copying/pasting from one Model to a target Model, and (in general) the RoleConstraint
                    '  won't exist in the target Model. If it does, then that's the RoleConstraint that's needed.
                    '  NB Testing to see if the Signature of the RoleConstraint already exists in the target Model is already performed in the
                    '  Paste proceedure before dropping the RoleConstraint onto a target Page/Model. If there is/was any clashes, then the 
                    '  RoleConstraint being copied/pasted will have it's Id/Name/Symbol changed and will not be affected by this test to see
                    '  if the RoleConstraint already exists in the target Model.
                    '---------------------------------------------------------------------------------------------------------------------
                    lrRoleConstraint = arModel.RoleConstraint.Find(AddressOf Me.Equals)
                Else
                    With Me
                        lrRoleConstraint.Model = arModel
                        lrRoleConstraint.Symbol = .Symbol
                        lrRoleConstraint.Id = .Id
                        lrRoleConstraint.Name = .Name
                        lrRoleConstraint.isDirty = True

                        If abIsMDAModelElement = False Then
                            lrRoleConstraint.IsMDAModelElement = .IsMDAModelElement
                        Else
                            lrRoleConstraint.IsMDAModelElement = abIsMDAModelElement
                        End If

                        lrRoleConstraint.RoleConstraintType = .RoleConstraintType
                        lrRoleConstraint.RingConstraintType = .RingConstraintType

                        lrRoleConstraint.IsDeontic = .IsDeontic
                        lrRoleConstraint.IsPreferredIdentifier = .IsPreferredIdentifier
                        lrRoleConstraint.Cardinality = .Cardinality
                        lrRoleConstraint.CardinalityRangeType = .CardinalityRangeType
                        lrRoleConstraint.LevelNr = .LevelNr
                        lrRoleConstraint.ShortDescription = .ShortDescription
                        lrRoleConstraint.LongDescription = .LongDescription
                        lrRoleConstraint.MaximumFrequencyCount = .MaximumFrequencyCount
                        lrRoleConstraint.MinimumFrequencyCount = .MinimumFrequencyCount
                        lrRoleConstraint.ValueRangeType = .ValueRangeType

                        For Each lrRole In .Role
                            lrRoleConstraint.Role.Add(lrRole.Clone(arModel, abAddToModel))
                        Next

                        For Each lrRoleConstraintRole In .RoleConstraintRole
                            lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole.Clone(arModel, lrRoleConstraint, abAddToModel))
                        Next

                        For Each lrRoleConstraintArgument In .Argument
                            lrRoleConstraint.Argument.Add(lrRoleConstraintArgument.Clone(arModel, lrRoleConstraint))
                        Next

                        If abAddToModel Then
                            If arModel.RoleConstraint.Find(Function(x) x.Id = lrRoleConstraint.Id) IsNot Nothing Then
                                lrRoleConstraint.Id = arModel.CreateUniqueRoleConstraintName(lrRoleConstraint.RoleConstraintType.ToString, 0)
                                lrRoleConstraint.Name = lrRoleConstraint.Id
                            End If
                            arModel.AddRoleConstraint(lrRoleConstraint)
                        End If

                    End With
                End If

                Return lrRoleConstraint
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tRoleConstraint.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrRoleConstraint
            End Try

        End Function

        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.RoleConstraintInstance

            Try
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                Select Case Me.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint, _
                              pcenumRoleConstraintType.ExternalUniquenessConstraint
                        lrRoleConstraintInstance = New FBM.tUniquenessConstraint
                    Case Is = pcenumRoleConstraintType.RingConstraint
                        lrRoleConstraintInstance = New FBM.RingConstraint()
                    Case Else
                        lrRoleConstraintInstance = New FBM.RoleConstraintInstance(Me.RoleConstraintType)
                End Select


                With Me
                    lrRoleConstraintInstance.Model = arPage.Model
                    lrRoleConstraintInstance.Page = arPage
                    lrRoleConstraintInstance.RoleConstraint = Me
                    lrRoleConstraintInstance.Id = .Id
                    lrRoleConstraintInstance.Name = .Name
                    lrRoleConstraintInstance.ConceptType = .ConceptType
                    lrRoleConstraintInstance.RoleConstraintType = .RoleConstraintType
                    lrRoleConstraintInstance.RingConstraintType = .RingConstraintType
                    lrRoleConstraintInstance.LevelNr = .LevelNr
                    lrRoleConstraintInstance.MinimumFrequencyCount = .MinimumFrequencyCount
                    lrRoleConstraintInstance.MaximumFrequencyCount = .MaximumFrequencyCount
                    lrRoleConstraintInstance.MinimumValue = .MinimumValue
                    lrRoleConstraintInstance.MaximumValue = .MaximumValue
                    lrRoleConstraintInstance.IsDeontic = .IsDeontic
                    lrRoleConstraintInstance.IsPreferredIdentifier = .IsPreferredIdentifier
                    lrRoleConstraintInstance.Cardinality = .Cardinality
                    lrRoleConstraintInstance.CardinalityRangeType = .CardinalityRangeType
                    lrRoleConstraintInstance.ValueRangeType = .ValueRangeType

                    If abAddToPage And Not arPage.RoleConstraintInstance.Contains(lrRoleConstraintInstance) Then
                        arPage.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance)
                    End If

                    '-------------------------------------------------------------------------
                    'Associate the RoleInstances to which the RoleConstraintInstance relates
                    '-------------------------------------------------------------------------                    
                    Dim lrRoleInstance As FBM.RoleInstance
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                    For Each lrRoleConstraintRole In Me.RoleConstraintRole

                        lrRoleInstance = New FBM.RoleInstance(.Model, arPage)
                        lrRoleInstance.Id = lrRoleConstraintRole.Role.Id
                        lrRoleInstance = arPage.RoleInstance.Find(AddressOf lrRoleInstance.Equals)
                        lrRoleConstraintInstance.Role.Add(lrRoleInstance)

                        '--------------------------------------------------------------------
                        'Create a RoleConstraintRoleInstance for the RoleConstraintInstance
                        '--------------------------------------------------------------------
                        lrRoleConstraintRoleInstance = New FBM.RoleConstraintRoleInstance(lrRoleConstraintRole, lrRoleConstraintInstance, lrRoleInstance)
                        lrRoleConstraintRoleInstance.IsEntry = lrRoleConstraintRole.IsEntry
                        lrRoleConstraintRoleInstance.IsExit = lrRoleConstraintRole.IsExit

                        If lrRoleInstance.Role.FactType.IsSubtypeRelationshipFactType Then
                            lrRoleConstraintRoleInstance.SubtypeConstraintInstance = arPage.SubtypeRelationship.Find(Function(x) x.SubtypeRelationship.FactType.Id = lrRoleInstance.Role.FactType.Id)
                        End If

                        lrRoleConstraintInstance.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                    Next

                End With

                Return lrRoleConstraintInstance

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function CloneFrequencyConstraintInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.FrequencyConstraint

            Dim lrRoleConstraintInstance As New FBM.FrequencyConstraint()

            With Me
                lrRoleConstraintInstance.Model = .Model
                lrRoleConstraintInstance.Page = arPage
                lrRoleConstraintInstance.RoleConstraint = Me
                lrRoleConstraintInstance.Id = .Id
                lrRoleConstraintInstance.Name = .Name
                lrRoleConstraintInstance.ConceptType = .ConceptType
                lrRoleConstraintInstance.RoleConstraintType = .RoleConstraintType
                lrRoleConstraintInstance.LevelNr = .LevelNr
                lrRoleConstraintInstance.MinimumFrequencyCount = .MinimumFrequencyCount
                lrRoleConstraintInstance.MaximumFrequencyCount = .MaximumFrequencyCount
                lrRoleConstraintInstance.IsDeontic = .IsDeontic
                lrRoleConstraintInstance.IsPreferredIdentifier = .IsPreferredIdentifier
                lrRoleConstraintInstance.Cardinality = .Cardinality
                lrRoleConstraintInstance.CardinalityRangeType = .CardinalityRangeType

                If abAddToPage Then
                    arPage.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance)
                End If

                '-------------------------------------------------------------------------
                'Associate the RoleInstances to which the RoleConstraintInstance relates
                '-------------------------------------------------------------------------
                Dim lrRole As FBM.Role
                Dim lrRoleInstance As FBM.RoleInstance
                Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                For Each lrRole In Me.Role
                    lrRoleInstance = New FBM.RoleInstance(.Model, arPage)
                    lrRoleInstance.Id = lrRole.Id
                    lrRoleInstance = arPage.RoleInstance.Find(AddressOf lrRoleInstance.Equals)
                    lrRoleConstraintInstance.Role.Add(lrRoleInstance)

                    '--------------------------------------------------------------------
                    'Create a RoleConstraintRoleInstance for the RoleConstraintInstance
                    '--------------------------------------------------------------------
                    Dim lrRoleConstraintRole As New FBM.RoleConstraintRole(lrRole, Me)
                    lrRoleConstraintRole = Me.RoleConstraintRole.Find(AddressOf lrRoleConstraintRole.Equals)
                    lrRoleConstraintRoleInstance = New FBM.RoleConstraintRoleInstance(lrRoleConstraintRole, lrRoleConstraintInstance, lrRoleInstance)
                    lrRoleConstraintInstance.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                Next

            End With

            Return lrRoleConstraintInstance

        End Function

        Public Function CloneUniquenessConstraintInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As tUniquenessConstraint

            Dim lrRoleConstraintInstance As New tUniquenessConstraint

            With Me
                lrRoleConstraintInstance.Model = arPage.Model
                lrRoleConstraintInstance.Page = arPage
                lrRoleConstraintInstance.RoleConstraint = Me
                lrRoleConstraintInstance.Id = .Id
                lrRoleConstraintInstance.Name = .Name
                lrRoleConstraintInstance.ConceptType = .ConceptType
                lrRoleConstraintInstance.RoleConstraintType = .RoleConstraintType
                lrRoleConstraintInstance.LevelNr = .LevelNr
                lrRoleConstraintInstance.MinimumFrequencyCount = .MinimumFrequencyCount
                lrRoleConstraintInstance.MaximumFrequencyCount = .MaximumFrequencyCount
                lrRoleConstraintInstance.IsDeontic = .IsDeontic
                lrRoleConstraintInstance.IsPreferredIdentifier = .IsPreferredIdentifier
                lrRoleConstraintInstance.Cardinality = .Cardinality
                lrRoleConstraintInstance.CardinalityRangeType = .CardinalityRangeType

                If abAddToPage Then
                    arPage.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance)
                End If

                '-------------------------------------------------------------------------
                'Associate the RoleInstances to which the RoleConstraintInstance relates
                '-------------------------------------------------------------------------
                Dim lrRole As FBM.Role
                Dim lrRoleInstance As FBM.RoleInstance
                Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                For Each lrRole In Me.Role
                    lrRoleInstance = New FBM.RoleInstance(.Model, arPage)
                    lrRoleInstance.Id = lrRole.Id
                    lrRoleInstance = arPage.RoleInstance.Find(AddressOf lrRoleInstance.Equals)
                    lrRoleConstraintInstance.Role.Add(lrRoleInstance)

                    '--------------------------------------------------------------------
                    'Create a RoleConstraintRoleInstance for the RoleConstraintInstance
                    '--------------------------------------------------------------------
                    Dim lrRoleConstraintRole As New FBM.RoleConstraintRole(lrRole, Me)
                    lrRoleConstraintRole = Me.RoleConstraintRole.Find(AddressOf lrRoleConstraintRole.Equals)
                    lrRoleConstraintRoleInstance = New FBM.RoleConstraintRoleInstance(lrRoleConstraintRole, lrRoleConstraintInstance, lrRoleInstance)
                    lrRoleConstraintInstance.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                Next

            End With

            Return lrRoleConstraintInstance

        End Function

        ''' <summary>
        ''' Used for checking that a new JoinPath doesn't cover a Role in a Path of an existing Argument.JoinPath
        ''' </summary>
        ''' <param name="aarRoleList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExistingArgumentsContainsMemberOfRoleList(ByVal aarRoleList As List(Of FBM.Role)) As Boolean

            For Each lrRole In aarRoleList
                If Me.Role.Contains(lrRole) Then
                    Return True
                End If
            Next

            For Each lrArgument In Me.Argument
                For Each lrRole In lrArgument.JoinPath.RolePath
                    If aarRoleList.Contains(lrRole) Then
                        Return True
                    End If
                Next
            Next

            Return False

        End Function

        ''' <summary>
        ''' Changes the Model of the RoleConstraint to the target Model.
        ''' </summary>
        ''' <param name="arTargetModel">The Model to which the RoleConstraint will be associated on completion of this method.</param>
        ''' <remarks></remarks>
        Public Shadows Sub ChangeModel(ByRef arTargetModel As FBM.Model, ByVal abAddToModel As Boolean)

            Me.Model = arTargetModel

            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                lrRoleConstraintRole.Model = arTargetModel
                lrRoleConstraintRole.Role.Model = arTargetModel
            Next

            For Each lrRoleConstraintArgument In Me.Argument
                lrRoleConstraintArgument.Model = arTargetModel
            Next

            If abAddToModel Then
                Call arTargetModel.AddRoleConstraint(Me)
            End If

        End Sub

        Public Function CreateRoleConstraintRole(ByRef arRole As FBM.Role,
                                                 Optional arArgument As FBM.RoleConstraintArgument = Nothing,
                                                 Optional arSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance = Nothing) As FBM.RoleConstraintRole

            Try
                Dim lrRoleConstraintRole As New FBM.RoleConstraintRole(arRole, Me, False, False, Me.RoleConstraintRole.Count + 1, True)

                If arArgument IsNot Nothing Then
                    lrRoleConstraintRole.RoleConstraintArgument = arArgument
                End If

                '--------------------------------------------------------------------
                'Add the RoleConstraintRole and do RDS processing at the same time.
                If arSubtypeRelationshipInstance Is Nothing Then
                    Me.AddRoleConstraintRole(lrRoleConstraintRole)
                Else
                    Me.AddRoleConstraintRole(lrRoleConstraintRole, arSubtypeRelationshipInstance.SubtypeRelationship)
                End If


                If arArgument IsNot Nothing Then
                    arArgument.AddRoleConstraintRole(lrRoleConstraintRole)
                End If

                Me.Model.MakeDirty(False, True)

                Return lrRoleConstraintRole

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function DoesEachRoleReferenceTheSameModelObject() As Boolean

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            Dim lrModelObject As New FBM.ModelObject
            Dim liCounter As Integer = 0

            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                liCounter += 1
                If liCounter = 1 Then
                    lrModelObject = lrRoleConstraintRole.Role.JoinedORMObject
                Else
                    If Not (lrModelObject Is lrRoleConstraintRole.Role.JoinedORMObject) Then
                        Return False
                    End If
                End If
            Next

            Return True

        End Function

        ''' <summary>
        ''' Removes a RoleConstraintArgument from the RoleConstraint based on the provided SequenceNr
        ''' </summary>
        ''' <param name="aiSequenceNr">The SequenceNr of the Argument to be removed.</param>
        ''' <remarks></remarks>
        Public Sub RemoveArgumentBySequenceNr(ByVal aiSequenceNr As Integer)

            Try
                Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                lrRoleConstraintArgument = Me.Argument.Find(Function(x) x.SequenceNr = aiSequenceNr)

                If lrRoleConstraintArgument Is Nothing Then
                    Throw New Exception("No Argument exists for SequenceNr: " & aiSequenceNr.ToString)
                End If

                '-----------------------------------------------------------------------------------------
                'Remove the RoleConstraintArgument with the specified SequenceNr and all 
                '  Arguments with a higher SequenceNr.
                For Each lrRoleConstraintArgument In Me.Argument.FindAll(Function(x) x.SequenceNr = aiSequenceNr)

                    For Each lrRoleConstraintRole In lrRoleConstraintArgument.RoleConstraintRole
                        Me.RemoveRoleConstraintRole(lrRoleConstraintRole)
                    Next

                    Call Me.RemoveArgument(lrRoleConstraintArgument)
                Next

                For Each lrRoleConstraintArgument In Me.Argument.FindAll(Function(x) x.SequenceNr > aiSequenceNr)
                    lrRoleConstraintArgument.SequenceNr -= 1
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveArgument(ByRef arRoleConstraintArgument As FBM.RoleConstraintArgument)

            Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
            Dim lrRemovingRoleConstraintArgument As FBM.RoleConstraintArgument = arRoleConstraintArgument

            For Each lrRoleConstraintArgument In Me.Argument.FindAll(Function(x) x.SequenceNr > lrRemovingRoleConstraintArgument.SequenceNr)
                lrRoleConstraintArgument.SequenceNr -= 1
                TableRoleConstraintArgument.UpdateRoleConstraintArgument(lrRoleConstraintArgument)
            Next

            For Each lrRoleConstraintRole In arRoleConstraintArgument.RoleConstraintRole
                If Me.RoleConstraintRole.Contains(lrRoleConstraintRole) Then
                    Me.RemoveRoleConstraintRole(lrRoleConstraintRole)
                End If
            Next

            Me.Argument.Remove(arRoleConstraintArgument)

            TableRoleConstraintArgument.DeleteRoleConstraintArgument(arRoleConstraintArgument)

            '----------------------------------------------------------------------------------------------------
            'NB The following event does nothing at the RoleConstraintInstance level because RoleConstraintArguments
            '  are not stored at the instance level.
            RaiseEvent ArgumentRemoved(arRoleConstraintArgument)

        End Sub

        Public Sub RemoveRoleConstraintRole(ByRef arRoleConstraintRole As FBM.RoleConstraintRole)

            Try
                '=================================================================
                'RDS - Do RDS processing first.
                If Me.RoleConstraintType = pcenumRoleConstraintType.ExternalUniquenessConstraint Then

                    Dim larIndexColumn As New List(Of RDS.Column)
                    Dim lrTable As RDS.Table = Nothing

                    If Me.DoesEachRoleReferenceTheSameModelObject Then
                        If Me.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then
                            'All good. Can look for an Column/Index to remove
                        Else
                            Exit Try
                        End If
                    Else
                        Exit Try
                    End If

                    Dim lrIndex As RDS.Index

                    lrIndex = Me.getCorrespondingIndex()

                    Dim lrOtherRoleOfFactType = arRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(arRoleConstraintRole.Role.Id)
                    Dim lrColumnToRemove As RDS.Column = lrIndex.Column.Find(Function(x) x.Role.Id = lrOtherRoleOfFactType.Id)

                    If lrIndex IsNot Nothing Then

                        lrIndex.removeColumn(lrColumnToRemove)

                        If lrIndex.Column.Count = 1 Then
                            Call Me.Model.RDS.removeIndex(lrIndex)
                        End If
                    End If

                End If 'Is ExternalUniquenessConstraint


                Me.RoleConstraintRole.Remove(arRoleConstraintRole)

                Dim lrRoleConstraintRole As FBM.RoleConstraintRole = arRoleConstraintRole
                Dim larArgument = From Argument In Me.Argument
                                  From RoleConstraintRole In Argument.RoleConstraintRole
                                  Where lrRoleConstraintRole.Equals(RoleConstraintRole)
                                  Select Argument

                If larArgument.Count > 0 Then
                    Me.Argument.Remove(larArgument.First)
                End If

                Me.Role.Remove(arRoleConstraintRole.Role)

                TableRoleConstraintRole.DeleteRoleConstraintRole(arRoleConstraintRole)

                RaiseEvent RoleConstraintRoleRemoved(arRoleConstraintRole)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Removes the RoleConstraint from the Model.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = True) As Boolean Implements iModelObject.RemoveFromModel

            Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.RoleConstraint)

            Try

                If Me.IsReferenceSchemePreferredIdentifierRC And Not abForceRemoval Then

                    Dim larEntityType = From EntityType In Me.Model.EntityType
                                        Where EntityType.PreferredIdentifierRCId = Me.Id _
                                        And EntityType.HasSimpleReferenceScheme
                                        Select EntityType

                    Dim lrEntityType As FBM.EntityType

                    For Each lrEntityType In larEntityType
                        '-------------------------------------------------------------------------------
                        'Will only get here is an EntityType has a SimpleReferenceScheme and the 
                        ' PreferredIndentifierRCID of the EntityType is 'me' (the RoleConstraint).
                        '-------------------------------------------------------------------------------                        
                        Call prApplication.ThrowErrorMessage("Error: Cannot remove a Role Constraint which identifies the Simple Reference Scheme of an Entity Type",
                                                                     pcenumErrorType.Critical,
                                                                     Nothing,
                                                                     False)
                        Return False
                        Exit Function
                    Next
                End If

                lrModelDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrModelDictionaryEntry.Equals)

                If abDoDatabaseProcessing Then
                    Call TableRoleConstraintRole.DeleteRoleConstraintRolesByRoleConstraint(Me)
                    Call TableRoleConstraintArgument.DeleteRoleConstraintArgumentsByModelRoleConstraint(Me)
                    Call TableRoleConstraint.delete_RoleConstraint(Me)
                End If

                Select Case Me.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                        '------------------------------------------------------------------------
                        'CodeSafe: If No RoleConstraintRoles, don't want to abort at this stage
                        '------------------------------------------------------------------------
                        If Me.RoleConstraintRole.Count > 0 Then
                            Me.RoleConstraintRole(0).Role.FactType.RemoveInternalUniquenessConstraint(Me, False, False)
                        End If
                End Select

                For Each lrRoleConstraintRole In Me.RoleConstraintRole
                    lrRoleConstraintRole.Role.RoleConstraintRole.Remove(lrRoleConstraintRole)
                Next

                RaiseEvent RemovedFromModel(abDoDatabaseProcessing)

                Me.Model.RemoveRoleConstraint(Me, abCheckForErrors, abDoDatabaseProcessing)

                Me.Model.MakeDirty(False, abCheckForErrors)

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Public Sub RemoveRoleConstraintRoleByRole(ByRef arRole As FBM.Role, ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                Dim lrRoleConstraintRoleToRemove As FBM.RoleConstraintRole
                Dim lrRoleToRemove As FBM.Role

                lrRoleToRemove = arRole
                lrRoleConstraintRoleToRemove = Me.RoleConstraintRole.Find(Function(x) x.Role.Id = lrRoleToRemove.Id)


                Me.RoleConstraintRole.Remove(lrRoleConstraintRoleToRemove)
                Call lrRoleConstraintRoleToRemove.Delete()

                Call Me.Role.Remove(lrRoleToRemove)

                Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                For Each lrRoleConstraintRole In Me.RoleConstraintRole.FindAll(Function(x) x.SequenceNr >= lrRoleConstraintRoleToRemove.SequenceNr)
                    lrRoleConstraintRole.SequenceNr -= 1
                Next

                If Me.RoleConstraintRole.Count = 0 Then
                    Call Me.Model.RemoveRoleConstraint(Me, True, abBroadcastInterfaceEvent)
                End If

                RaiseEvent RoleConstraintRoleRemoved(lrRoleConstraintRoleToRemove)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveRoleConstraintRoles()

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            For Each lrRoleConstraintRole In Me.RoleConstraintRole.ToArray
                Me.RoleConstraintRole.Remove(lrRoleConstraintRole)
                Call lrRoleConstraintRole.Delete()
                RaiseEvent RoleConstraintRoleRemoved(lrRoleConstraintRole)
            Next

            For Each lrArgument In Me.Argument.ToArray
                Me.Argument.Remove(lrArgument)
                RaiseEvent ArgumentRemoved(lrArgument)
            Next

        End Sub

        Public Sub RemoveValueConstraint(ByVal asValueConstraint As String)

            Me.ValueConstraint.Remove(asValueConstraint)

            Call TableRoleValueConstraint.DeleteValueConstraint(Me, asValueConstraint)

            RaiseEvent ValueConstraintRemoved(asValueConstraint)

        End Sub

        Public Function RequiresJoinPath() As Boolean

            Select Case Me.RoleConstraintType
                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint,
                          pcenumRoleConstraintType.ExternalUniquenessConstraint,                         
                          pcenumRoleConstraintType.InclusiveORConstraint,
                          pcenumRoleConstraintType.ExclusiveORConstraint
                    Return False
                Case Else
                    'pcenumRoleConstraintType.ExclusionConstraint
                    Return True
            End Select

        End Function

        Public Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            Try
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.RoleConstraint)
                lrDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                lrDictionaryEntry.isRoleConstraint = True

                Call lrDictionaryEntry.Save()


                If abRapidSave Then
                    Call TableRoleConstraint.AddRoleConstraint(Me)

                ElseIf Me.isDirty Then

                    If TableRoleConstraint.ExistsRoleConstraint(Me) Then
                        Call TableRoleConstraint.UpdateRoleConstraint(Me)
                    Else
                        Call TableRoleConstraint.AddRoleConstraint(Me)
                    End If

                    Me.isDirty = False
                End If

                For Each lrRoleConstraintArgument In Me.Argument
                    Call lrRoleConstraintArgument.Save(abRapidSave)
                Next

                For Each lrRoleConstraintRole In Me.RoleConstraintRole
                    lrRoleConstraintRole.Save(abRapidSave)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function SetName(ByVal asNewName As String,
                                          Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                          Optional ByVal abSuppressModelSave As Boolean = False) As Boolean

            '-----------------------------------------------------------------------------------------------------------
            'The following explains the logic and philosophy of Boston.
            '  A RoleConstraint.Id/Name represents the same thing accross all Models in Boston, otherwise the Boston
            '  user would have a different RoleConstraint.Id/Name for the differing Concepts (not excluding that in Boston
            '  a RoleConstraint in one Model can have a wildly different RoleGroup (ModelObject associations) than the same
            '  named RoleConstraint in another Model).
            '  So, for example, 'Person' is the same Concept accross all Models.
            '  If in this Model the user changes the RoleConstraint.Id/Name of 'Person' to 'Persona' then the following 
            '  code works like this...If 'Person' is used by other Models, then 'Persona' is added as a new RoleConstraint.
            '  Otherwise, there is no risk in changing the RoleConstraint.Id/Name of 'Person' to 'Persona' in the current
            '  model.
            '  This approach takes a pessimistic view, that when changing a RoleConstraint.Id/Name in one Model the user
            '  does not want the RoleConstraint.Id/Name to be changed in all Models.
            '-----------------------------------------------------------------------------------------------------------
            Try
                '-----------------------------------------------------------
                'Set the name and Symbol of the RoleConstraint to the new asNewName.
                '  The Id of the RoleConstraint is modified later in this Set.
                '-----------------------------------------------------------
                _Name = asNewName
                Me.Symbol = asNewName

                'Check to see that the name begins with a capital letter.
                If Not Char.IsUpper(asNewName.Chars(0)) Then
                    Throw New tInformationException("Object Type names must start with a capital letter followed by one or more lowercase letters.")
                    Return False
                End If

                '--------------------------------------------------------------------------------------------------------
                'The surrogate key for the RoleConstraint is about to change (to match the name of the RoleConstraint)
                '  so update the ModelDictionary entry for the Concept/Symbol (the nominalistic idenity of the VactType
                '--------------------------------------------------------------------------------------------------------
                If StrComp(Me.Id, asNewName) <> 0 Then
                    '----------------------------------------------------------
                    'The new RoleConstraint.Name does not match the RoleConstraint.Id
                    '----------------------------------------------------------
                    Call Me.SwitchConcept(New FBM.Concept(asNewName, True), pcenumConceptType.RoleConstraint)

                    '------------------------------------------------------------------------------------------
                    'Update the Model(database) immediately. There is no choice. The reason why
                    '  is because the (in-memory) key is changing, so the database must be updated to 
                    '  reflect the new key, otherwise it will not be possible to Update an existing ValueType.
                    '------------------------------------------------------------------------------------------
                    Call TableRoleConstraint.ModifyKey(Me, asNewName)
                    Me.Id = asNewName
                    Call TableRoleConstraint.UpdateRoleConstraint(Me) 'Sets the new Name

                    Call Me.RaiseEventNameChanged(asNewName)

                    Me.Model.MakeDirty()
                End If

                '-------------------------------------------------------------
                'To make sure all the FactData and FactDataInstances/Pages are saved for RDS
                Me.Model.Save()

                Return True

            Catch iex As tInformationException
                prApplication.ThrowErrorMessage(iex.Message, pcenumErrorType.Information, Nothing, False, False, True)
                Return False
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tRoleConstraint.SetName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Public Sub SetRoleConstraintType(ByVal aiRoleConstraintType As pcenumRoleConstraintType)

            Me.RoleConstraintType = aiRoleConstraintType

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent RoleConstraintTypeChanged(aiRoleConstraintType)

        End Sub

        Public Sub SetRingConstraintType(ByVal aiRingConstraintType As pcenumRingConstraintType)

            Me.RingConstraintType = aiRingConstraintType

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent RingConstraintTypeChanged(aiRingConstraintType)

        End Sub

        Public Sub SetLevelNr(ByVal aiLevelNr As Integer)

            Me.LevelNr = aiLevelNr

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent LevelNrChanged(aiLevelNr)

        End Sub

        Public Sub SetCardinalityRangeType(ByVal aiCardinalityRangeType As pcenumCardinalityRangeType)

            Me.CardinalityRangeType = aiCardinalityRangeType

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent CardinalityRangeTypeChanged(aiCardinalityRangeType)
        End Sub

        Public Sub SetCardinality(ByVal aiCardinality As Integer)

            Me.Cardinality = aiCardinality

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent CardinalityChanged(aiCardinality)

        End Sub

        Public Sub SetMaximumFrequencyCount(ByVal aiMaximumFrequencyCount As Integer)

            Me.MaximumFrequencyCount = aiMaximumFrequencyCount

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, True)

            RaiseEvent MaximumFrequencyCountChanged(aiMaximumFrequencyCount)

        End Sub

        Public Sub SetMaximumValue(ByVal aiMaximumValue As String)

            Me.MaximumValue = aiMaximumValue

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, True)

            RaiseEvent MaximumValueChanged(aiMaximumValue)

        End Sub

        Public Sub SetMinimumFrequencyCount(ByVal aiMinimumFrequencyCount As Integer)

            Me.MinimumFrequencyCount = aiMinimumFrequencyCount

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, True)

            RaiseEvent MinimumFrequencyCountChanged(aiMinimumFrequencyCount)

        End Sub

        Public Sub SetMinimumValue(ByVal aiMinimumValue As String)

            Me.MinimumValue = aiMinimumValue

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, True)

            RaiseEvent MinimumValueChanged(aiMinimumValue)

        End Sub

        Public Sub SetIsDeontic(ByVal abIsDeontic As Boolean)

            Me.IsDeontic = abIsDeontic

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent IsDeonticChanged(abIsDeontic)

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="abIsPreferredIdentifier"></param>
        ''' <param name="abDoRDSProcessing">When creating the first Uniqueness Constraint for a Ternary/greater Fact Type, we want to set the RC.IsPreferredIndentifier to True, 
        ''' but not create the Index, because that is done further in Model.AddRoleConstraint, so abort at RDS processing here.</param>
        Public Sub SetIsPreferredIdentifier(ByVal abIsPreferredIdentifier As Boolean,
                                            Optional ByVal abDoRDSProcessing As Boolean = True,
                                            Optional ByRef arExistingPreferedReferenceSchemeRoleConstraint As FBM.RoleConstraint = Nothing)

            Try
                '============================================================================================
                'If changing the PK/Preferred Identifier for a FactType, make sure the exising RC/preferredIdentifier is False
                '  E.g. If a Fact Type accross Part, Bin and Warehouse has a binary Internal RC that is Preferred,
                '    and now we are making a second IUC preferred, then neeed to set the existing preferred IUC to isPreferred = False.
                If Me.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    If Me.Role(0).FactType.Is1To1BinaryFactType Then
                        If Me.Role(0).FactType.IsPreferredReferenceMode Then
                            Dim lrModelObject As FBM.ModelObject = Me.Role(0).FactType.GetOtherRoleOfBinaryFactType(Me.Role(0).Id).JoinedORMObject
                            Select Case lrModelObject.GetType
                                Case = GetType(FBM.EntityType)
                                    If arExistingPreferedReferenceSchemeRoleConstraint IsNot Nothing Then
                                        Call arExistingPreferedReferenceSchemeRoleConstraint.SetIsPreferredIdentifier(False)
                                    End If
                                Case Else
                                    Throw New NotImplementedException("Only Entity Types catered for at this stage. Contact support.")
                            End Select
                        End If
                    Else
                        Dim lrExistingPreferredIndentifierRoleConstraint = Me.RoleConstraintRole(0).Role.FactType.getPreferredInternalUniquenessConstraint
                        If (Not lrExistingPreferredIndentifierRoleConstraint Is Me) And abIsPreferredIdentifier Then
                            Call lrExistingPreferredIndentifierRoleConstraint.SetIsPreferredIdentifier(False)
                        End If
                    End If
                End If

                Me.IsPreferredIdentifier = abIsPreferredIdentifier

                Call Me.makeDirty()
                Call Me.Model.MakeDirty(False, False)

                RaiseEvent IsPreferredIdentifierChanged(abIsPreferredIdentifier)

                Dim larColumnsAffected As New List(Of RDS.Column)

                If Not abDoRDSProcessing Then Exit Sub
                '=================================================================================================
                'RDS Processing
                For Each lrRoleConstraintRole In Me.RoleConstraintRole
                    'Columns/Attributes/Properties will be set to ContributesToPrimaryKey = True
                    Dim larColumn As Object
                    If Me.RoleConstraintRole(0).Role.FactType.Arity = 2 Then
                        larColumn = From Table In Me.Model.RDS.Table
                                    From Column In Table.Column
                                    Where Column.Role.Id = lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id).Id
                                    Where lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id).JoinedORMObject Is Table.FBMModelElement
                                    Select Column
                    Else
                        larColumn = From Table In Me.Model.RDS.Table
                                    From Column In Table.Column
                                    Where Column.Role.Id = lrRoleConstraintRole.Role.Id
                                    Select Column
                    End If

                    For Each lrColumn In larColumn
                        larColumnsAffected.Add(lrColumn)
                    Next
                Next

                If larColumnsAffected.Count > 0 Then

                    '==================================================
                    'Index
                    Dim lrTable As RDS.Table = larColumnsAffected.First.Table

                    '--------------------------------
                    'Modify the existing PrimaryKey
                    '20200722-VM-Removed because if exists, should change the actual Index
                    'Call lrTable.makeExistingPrimaryKeySimplyUnique(larColumnsAffected)

                    Dim lrExistingIndex As RDS.Index = lrTable.getIndexByColumns(larColumnsAffected)

                    If lrExistingIndex Is Nothing Then

                        Dim lsQualifier As String = "UC"
                        'No existing Index exists for the Columns
                        If abIsPreferredIdentifier = True Then
                            lsQualifier = "PK"
                        End If
                        '------------------------------
                        'Add the new PrimaryKey Index
                        Dim lrPrimaryKeyIndex As New RDS.Index(lrTable,
                                                               Me.Model.RDS.createUniqueIndexName(lrTable.Name & "_" & lsQualifier, 0),
                                                               lsQualifier,
                                                               pcenumODBCAscendingOrDescending.Ascending,
                                                               True,
                                                               True,
                                                               False,
                                                               larColumnsAffected,
                                                               False,
                                                               True)

                        Call lrTable.addIndex(lrPrimaryKeyIndex)

                        'Need to add the Columns and Index to each Subtype ModelObject/Table that is not absorbed
                        Call lrTable.addPrimaryKeyToNonAbsorbedTables(lrPrimaryKeyIndex, abIsPreferredIdentifier)

                    Else 'Index already exists
                        If abIsPreferredIdentifier Then
                            lrExistingIndex.IsPrimaryKey = abIsPreferredIdentifier
                            Call lrExistingIndex.setQualifier("PK")
                            Call lrExistingIndex.setName(Me.Model.RDS.createUniqueIndexName(lrTable.Name & "_PK", 0))
                            Call lrExistingIndex.setIsPrimaryKey(True)
                        Else
                            lrExistingIndex.IsPrimaryKey = abIsPreferredIdentifier
                            Call lrExistingIndex.setQualifier("UC")
                            Call lrExistingIndex.setName(Me.Model.RDS.createUniqueIndexName(lrTable.Name & "_UC", 0))
                            Call lrExistingIndex.setIsPrimaryKey(False)
                        End If

                        'Need to add the Columns and Index to each Subtype ModelObject/Table that is not absorbed
                        Call lrTable.addPrimaryKeyToNonAbsorbedTables(lrExistingIndex, abIsPreferredIdentifier)
                    End If

                    For Each lrColumn In larColumnsAffected
                        Call lrColumn.triggerForceRefreshEvent()
                    Next

                End If 'larColumnsAffected.Count > 0

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub SetValueRangeType(ByVal aiValueRangeType As pcenumValueRangeType)

            Try
                Me.ValueRangeType = aiValueRangeType

                Call Me.makeDirty()
                Call Me.Model.MakeDirty(False, False)

                RaiseEvent ValueRangeTypeChanged(aiValueRangeType)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub ClearModelErrors() Implements iValidationErrorHandler.ClearModelErrors
            Me.ModelError.Clear()
        End Sub

        Public Sub AddModelError(ByRef arModelError As ModelError) Implements iValidationErrorHandler.AddModelError

            Me.ModelError.AddUnique(arModelError)
            RaiseEvent ModelErrorAdded(arModelError)

        End Sub

    End Class

End Namespace