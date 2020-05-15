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
        Public ValueRangeType As pcenumValueRangeType = pcenumValueRangeType.LessThanOREqual

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
        <XmlAttribute()> _
        <Browsable(False), _
        [ReadOnly](True)> _
        Public Overridable Property MaximumFrequencyCount() As Integer
            Get
                Return _MaximumFrequencyCount
            End Get
            Set(ByVal value As Integer)
                _MaximumFrequencyCount = value
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
        Public Role As New List(Of FBM.Role)

        <XmlElement()> _
        Public RoleConstraintRole As New List(Of FBM.RoleConstraintRole)

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
        Public CurrentArgument As FBM.RoleConstraintArgument

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

        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded

        Public Event CardinalityChanged(ByVal aiNewCardinality As Integer)
        Public Event RemovedFromModel() Implements iModelObject.RemovedFromModel
        Public Event RoleConstraintTypeChanged(ByVal aiNewRoleConstraintType As pcenumRoleConstraintType)
        Public Event RingConstraintTypeChanged(ByVal aiNewRingConstraintType As pcenumRingConstraintType)
        Public Event LevelNrChanged(ByVal aiNewLevelNr As Integer)
        Public Event CardinalityRangeTypeChanged(ByVal aiNewCardinalityRangeType As pcenumCardinalityRangeType)
        Public Event MinimumFrequencyCountChanged(ByVal aiNewMinimumFrequencyCount As Integer)
        Public Event MaximumFrequencyCountChanged(ByVal aiNewMaximumFrequencyCount As Integer)
        Public Event IsDeonticChanged(ByVal abNewIsDeontic As Boolean)
        Public Event IsPreferredIdentifierChanged(ByVal abNewIsPreferredIdentifier As Boolean)
        Public Event RoleConstraintRoleAdded(ByRef arRoleConstraintRole As FBM.RoleConstraintRole)
        Public Event RoleConstraintRoleRemoved(ByRef arRoleConstraintRole As FBM.RoleConstraintRole)
        Public Event ArgumentRemoved(ByRef arRoleConstraintArgument As FBM.RoleConstraintArgument)

        Sub New()
            '------------------------------------------
            'Set the unique key for the RoleConstraint
            '------------------------------------------
            Id = System.Guid.NewGuid.ToString
            Me.ConceptType = pcenumConceptType.RoleConstraint

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal asRoleConstraintName As String, ByVal abUseNameAsId As Boolean, Optional ByVal aiRoleConstraintType As pcenumRoleConstraintType = Nothing, Optional ByRef aarRole As List(Of FBM.Role) = Nothing)

            Me.New()

            Dim lrRole As FBM.Role
            Dim liCounter As Integer = 0
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            Me.Model = arModel
            Me.Name = asRoleConstraintName
            If abUseNameAsId Then
                Me.Id = Me.Name
            End If

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
                        lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, Me, False, False, liCounter)

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

        Sub New(ByRef arModel As FBM.Model, ByVal aiRoleConstraintType As pcenumRoleConstraintType, Optional ByRef aarRole As List(Of FBM.Role) = Nothing, Optional ByVal ab_add_to_model As Boolean = Nothing, Optional ByVal aiLevelNr As Integer = Nothing)
            '---------------------------------------------------------------------------
            'PRECONDITIONS:
            '  * If aarRole is populated, all Roles in aarRole have the same FactType.
            '---------------------------------------------------------------------------
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
                    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, Me, False, False, liCounter)

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
            If IsSomething(ab_add_to_model) Then
                If ab_add_to_model Then
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
        ''' <param name="ab_add_to_model"></param>
        ''' <remarks></remarks>
        Sub New(ByVal arModel As FBM.Model, ByVal aiRoleConstraintType As pcenumRoleConstraintType, ByRef aarRole As List(Of FBM.Role), ByVal aiCardinality As Integer, ByVal aiCardinalityRangeType As pcenumCardinalityRangeType, Optional ByVal ab_add_to_model As Boolean = Nothing)

            Me.New()

            Dim lrRole As FBM.Role
            Dim liCounter As Integer
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            Me.Model = arModel
            Me.RoleConstraintType = aiRoleConstraintType
            Me.Cardinality = aiCardinality
            Me.CardinalityRangeType = aiCardinalityRangeType

            For Each lrRole In aarRole
                liCounter += 1
                Me.Role.Add(lrRole)

                '------------------------------------------------------------------------
                'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                '------------------------------------------------------------------------
                lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, Me, False, False, liCounter)

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
            If IsSomething(ab_add_to_model) Then
                If ab_add_to_model Then
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


        Public Function EqualsByRoleConstraintRoleGroup(ByVal other As FBM.RoleConstraint) As Boolean

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                If Not other.RoleConstraintRole.Exists(AddressOf lrRoleConstraintRole.EqualsByRole) Then
                    Return False
                End If
            Next

            Return True

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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Function

        Public Function GetCommonArgumentModelObjects() As List(Of FBM.ModelObject)

            Try
                Dim larModelObject As New List(Of FBM.ModelObject)
                Dim larSecondaryList As New List(Of FBM.ModelObject)
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                Dim lrArgument As FBM.RoleConstraintArgument

                If Me.Argument.Count > 0 Then
                    For Each lrArgument In Me.Argument.FindAll(Function(x) x.RoleConstraintRole.Count > 0)
                        If Not larModelObject.Contains(lrArgument.RoleConstraintRole(0).Role.JoinedORMObject) Then
                            larModelObject.Add(lrArgument.RoleConstraintRole(0).Role.JoinedORMObject)
                        End If
                    Next
                Else
                    For Each lrRoleConstraintRole In Me.RoleConstraintRole
                        If Not larModelObject.Contains(lrRoleConstraintRole.Role.JoinedORMObject) Then
                            larModelObject.Add(lrRoleConstraintRole.Role.JoinedORMObject)
                        End If
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
                    For Each lrCandidateSupertypeMO In larModelObject.FindAll(Function(x) x.Id <> lrModelObject.Id)
                        If Me.Model.ModelObjectIsSubtypeOfModelObject(lrModelObject, lrCandidateSupertypeMO) Then
                            '----------------------------------------
                            'Leave the ModelObject out of the list.
                            '----------------------------------------
                        Else
                            larSecondaryList.Add(lrModelObject)
                        End If
                    Next
                Next

                If larModelObject.Count = 1 Then
                    larSecondaryList.Add(larModelObject(0))
                End If

                Return larSecondaryList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return New List(Of FBM.ModelObject)
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

        Public Shadows Function Clone(ByRef arModel As FBM.Model, Optional abAddToModel As Boolean = False) As Object 'Implements ICloneable.Clone

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

                        If abAddToModel Then
                            arModel.AddRoleConstraint(lrRoleConstraint)
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
                        lrRoleConstraint.IsMDAModelElement = .IsMDAModelElement

                        For Each lrRole In .Role
                            lrRoleConstraint.Role.Add(lrRole.Clone(arModel, abAddToModel))
                        Next

                        For Each lrRoleConstraintRole In .RoleConstraintRole
                            lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole.Clone(arModel, lrRoleConstraint, abAddToModel))
                        Next
                    End With
                End If

                Return lrRoleConstraint
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tRoleConstraint.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return lrRoleConstraint
            End Try

        End Function


        Public Function CreateRoleConstraintRole(ByRef arRole As FBM.Role) As FBM.RoleConstraintRole

            Dim lrRoleConstraintRole As New FBM.RoleConstraintRole(arRole, Me, False, False, Me.RoleConstraintRole.Count + 1)
            Me.RoleConstraintRole.Add(lrRoleConstraintRole)

            RaiseEvent RoleConstraintRoleAdded(lrRoleConstraintRole)

            Return lrRoleConstraintRole

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
                For Each lrRoleConstraintArgument In Me.Argument.FindAll(Function(x) x.SequenceNr >= aiSequenceNr)
                    For Each lrRoleConstraintRole In lrRoleConstraintArgument.RoleConstraintRole
                        Me.RemoveRoleConstraintRole(lrRoleConstraintRole)
                    Next

                    Call Me.RemoveArgument(lrRoleConstraintArgument)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Overridable Sub RemoveArgument(ByRef arRoleConstraintArgument As FBM.RoleConstraintArgument)

            Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
            Dim lrRemovingRoleConstraintArgument As FBM.RoleConstraintArgument = arRoleConstraintArgument

            For Each lrRoleConstraintArgument In Me.Argument.FindAll(Function(x) x.SequenceNr > lrRemovingRoleConstraintArgument.SequenceNr)
                lrRoleConstraintArgument.SequenceNr -= 1
            Next

            Me.Argument.Remove(arRoleConstraintArgument)

            '----------------------------------------------------------------------------------------------------
            'NB The following event does nothing at the RoleConstraintInstance level because RoleConstraintArguments
            '  are not stored at the instance level.
            RaiseEvent ArgumentRemoved(arRoleConstraintArgument)

        End Sub

        Public Overridable Sub RemoveRoleConstraintRole(ByRef arRoleConstraintRole As FBM.RoleConstraintRole)

            Me.RoleConstraintRole.Remove(arRoleConstraintRole)

            RaiseEvent RoleConstraintRoleRemoved(arRoleConstraintRole)

        End Sub

        ''' <summary>
        ''' Removes the RoleConstraint from the Model.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean Implements iModelObject.RemoveFromModel

            Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.RoleConstraint)

            Try

                If Me.IsReferenceSchemePreferredIdentifierRC And Not abForceRemoval Then

                    Dim larEntityType = From EntityType In Me.Model.EntityType _
                                        Where EntityType.PreferredIdentifierRCId = Me.Id _
                                        And EntityType.HasSimpleReferenceMode _
                                        Select EntityType

                    Dim lrEntityType As FBM.EntityType

                    For Each lrEntityType In larEntityType
                        '-------------------------------------------------------------------------------
                        'Will only get here is an EntityType has a SimpleReferenceScheme and the 
                        ' PreferredIndentifierRCID of the EntityType is 'me' (the RoleConstraint).
                        '-------------------------------------------------------------------------------                        
                        Call Me.Model.ThrowErrorMessage("Error: Cannot remove a Role Constraint which identifies the Simple Reference Scheme of an Entity Type", _
                                                                     pcenumErrorType.Critical, _
                                                                     Nothing, _
                                                                     False)
                        Return False
                        Exit Function
                    Next
                End If

                lrModelDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrModelDictionaryEntry.Equals)

                Select Case Me.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                        '------------------------------------------------------------------------
                        'CodeSafe: If No RoleConstraintRoles, don't want to abort at this stage
                        '------------------------------------------------------------------------
                        If Me.RoleConstraintRole.Count > 0 Then
                            Me.RoleConstraintRole(0).Role.FactType.InternalUniquenessConstraint.Remove(Me)
                        End If
                End Select

                Me.Model.RemoveRoleConstraint(Me, abCheckForErrors)

                Me.Model.MakeDirty(False, abCheckForErrors)

                RaiseEvent RemovedFromModel()

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return False
            End Try

        End Function

        Public Sub RemoveRoleConstraintRoleByRole(ByRef arRole As FBM.Role)

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

                RaiseEvent RoleConstraintRoleRemoved(lrRoleConstraintRoleToRemove)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Sub RemoveRoleConstraintRoles()

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            For Each lrRoleConstraintRole In Me.RoleConstraintRole.ToArray
                Me.RoleConstraintRole.Remove(lrRoleConstraintRole)
                Call lrRoleConstraintRole.Delete()
                RaiseEvent RoleConstraintRoleRemoved(lrRoleConstraintRole)
            Next

        End Sub

        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save()
        End Sub

        Public Overridable Sub SetName(ByVal asNewName As String)

            '-----------------------------------------------------------------------------------------------------------
            'The following explains the logic and philosophy of Richmond.
            '  A RoleConstraint.Id/Name represents the same thing accross all Models in Richmond, otherwise the Richmond 
            '  user would have a different RoleConstraint.Id/Name for the differing Concepts (not excluding that in Richmond
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

                '--------------------------------------------------------------------------------------------------------
                'The surrogate key for the RoleConstraint is about to change (to match the name of the RoleConstraint)
                '  so update the ModelDictionary entry for the Concept/Symbol (the nominalistic idenity of the VactType
                '--------------------------------------------------------------------------------------------------------
                If StrComp(Me.Id, asNewName) <> 0 Then
                    '----------------------------------------------------------
                    'The new RoleConstraint.Name does not match the RoleConstraint.Id
                    '----------------------------------------------------------
                    Call Me.SwitchConcept(New FBM.Concept(asNewName))

                    '------------------------------------------------------------------------------------------
                    'Update the Model(database) immediately. There is no choice. The reason why
                    '  is because the (in-memory) key is changing, so the database must be updated to 
                    '  reflect the new key, otherwise it will not be possible to Update an existing ValueType.
                    '------------------------------------------------------------------------------------------               
                    Me.Id = asNewName

                    Me.Model.MakeDirty()
                    'If TableModelDictionary.DoesModelObjectExistInAntotherModel(Me) Then
                    '    '------------------------------------------------------------------------------
                    '    'The existing ModelObject (Id not yet modified) exists in another Model, so 
                    '    ' 'add' a new ModelObject rather than 'update' the ModelObject. 
                    '    ' The reason why you would add a new ModelObject is because the user doesn't
                    '    ' necessarily change the ModelObject in all models but only the model
                    '    ' currently being worked on.
                    '    '------------------------------------------------------------------------------
                    '    '-----------------------------------------------
                    '    'Set the RoleConstraint.Id to the same as the Name
                    '    '-----------------------------------------------
                    '    Call Me.Model.UpdateDictionarySymbol(Me.Id, asNewName, pcenumConceptType.RoleConstraint)
                    '    Me.Id = asNewName

                    '    If TableRoleConstraint.ExistsRoleConstraint(Me) Then
                    '        '----------------------------------------------------------------------
                    '        'Do nothing because the RoleConstraint already exists in the database.
                    '        '  Another model might use the new RoleConstraint.Id (likely) or it already
                    '        '  exists for Me.Model (unlikely because RoleConstraints are unique to
                    '        '  a Model), but possible if no checking is done before the change
                    '        '  is made to a RoleConstraint.Name to see whether the new RoleConstraint.Name
                    '        '  already exists in the Model.
                    '        '----------------------------------------------------------------------
                    '    Else
                    '        Call TableRoleConstraint.AddRoleConstraint(Me)
                    '    End If
                    'Else
                    '    '-------------------------------------------------------------------------
                    '    'The ModelObject is not utilised in any other Model.
                    '    ' It is safe to modify the database (update the table MetaModelFactType)
                    '    '-------------------------------------------------------------------------
                    '    Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.RoleConstraint)
                    '    If Me.Model.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsBySymbol) Then
                    '        Call Me.Model.UpdateDictionarySymbol(Me.Id, asNewName, pcenumConceptType.RoleConstraint)
                    '    Else
                    '        Dim lsMessage As String = ""
                    '        lsMessage = "Tried to modify the Name of a Role Constraint where no Dictionary Entry exists for that Role Constraint."
                    '        lsMessage &= vbCrLf & "Original DictionaryEntry.Symbol: " & Me.Id.ToString
                    '        lsMessage &= vbCrLf & "New DictionaryEntry.Symbol: " & asNewName
                    '        Throw New System.Exception(lsMessage)
                    '    End If

                    '    '------------------------------------------------------------------------------------------
                    '    'Update the Model(database) immediately. There is no choice. The reason why
                    '    '  is because the (in-memory) key is changing, so the database must be updated to 
                    '    '  reflect the new key, otherwise it will not be possible to Update an existing RoleConstraint.
                    '    '------------------------------------------------------------------------------------------
                    '    Call TableRoleConstraint.ModifyKey(Me, asNewName)
                    '    Me.Id = asNewName
                    '    Call TableRoleConstraint.UpdateRoleConstraint(Me) 'Sets the new Name
                    'End If

                    'Me.Model.MakeDirty()
                End If
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tRoleConstraint.SetName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Sub SetRoleConstraintType(ByVal aiRoleConstraintType As pcenumRoleConstraintType)

            Me.RoleConstraintType = aiRoleConstraintType
            RaiseEvent RoleConstraintTypeChanged(aiRoleConstraintType)

        End Sub

        Public Sub SetRingConstraintType(ByVal aiRingConstraintType As pcenumRingConstraintType)
            Me.RingConstraintType = aiRingConstraintType
            RaiseEvent RingConstraintTypeChanged(aiRingConstraintType)
        End Sub

        Public Sub SetLevelNr(ByVal aiLevelNr As Integer)
            Me.LevelNr = aiLevelNr
            RaiseEvent LevelNrChanged(aiLevelNr)
        End Sub

        Public Sub SetCardinalityRangeType(ByVal aiCardinalityRangeType As pcenumCardinalityRangeType)
            Me.CardinalityRangeType = aiCardinalityRangeType
            RaiseEvent CardinalityRangeTypeChanged(aiCardinalityRangeType)
        End Sub

        Public Sub SetCardinality(ByVal aiCardinality As Integer)
            Me.Cardinality = aiCardinality
            RaiseEvent CardinalityChanged(aiCardinality)
        End Sub

        Public Sub SetMinimumFrequencyCount(ByVal aiMinimumFrequencyCount As Integer)
            Me.MinimumFrequencyCount = aiMinimumFrequencyCount
            RaiseEvent MinimumFrequencyCountChanged(aiMinimumFrequencyCount)
        End Sub

        Public Sub SetMaximumFrequencyCount(ByVal aiMaximumFrequencyCount As Integer)
            Me.MaximumFrequencyCount = aiMaximumFrequencyCount
            RaiseEvent MaximumFrequencyCountChanged(aiMaximumFrequencyCount)
        End Sub

        Public Sub SetIsDeontic(ByVal abIsDeontic As Boolean)
            Me.IsDeontic = abIsDeontic
            RaiseEvent IsDeonticChanged(abIsDeontic)
        End Sub

        Public Sub SetIsPreferredIdentifier(ByVal abIsPreferredIdentifier As Boolean)
            Me.IsPreferredIdentifier = abIsPreferredIdentifier
            RaiseEvent IsPreferredIdentifierChanged(abIsPreferredIdentifier)
        End Sub

        Public Sub ClearModelErrors() Implements iValidationErrorHandler.ClearModelErrors
            Me.ModelError.Clear()
        End Sub

        Public Sub AddModelError(ByRef arModelError As ModelError) Implements iValidationErrorHandler.AddModelError

            Me.ModelError.Add(arModelError)
            RaiseEvent ModelErrorAdded(arModelError)

        End Sub

    End Class

End Namespace