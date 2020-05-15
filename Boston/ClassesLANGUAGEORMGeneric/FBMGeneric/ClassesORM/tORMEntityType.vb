Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class EntityType
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.EntityType)
        Implements ICloneable
        Implements iMDAObject
        Implements FBM.iValidationErrorHandler

        <XmlAttribute()> _
        Public Shadows Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
                Me.Symbol = value
                Me.Id = value
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
        Public ReferenceMode As String = ""

        <XmlElement()> _
        Public SubtypeConstraint As New List(Of FBM.tSubtypeRelationship)

        <XmlIgnore()> _
        Public primitive_type_entity_id As Integer = 0
        <XmlIgnore()> _
        Public PrimativeType As String = ""

        <XmlIgnore()> _
        Public _ReferenceModeFactType As FBM.FactType = Nothing
        <XmlIgnore()> _
        Public Property ReferenceModeFactType() As FBM.FactType
            Get
                Return Me._ReferenceModeFactType
            End Get
            Set(ByVal value As FBM.FactType)
                Try
                    Me._ReferenceModeFactType = value

                    '-------------------------------------------------------------------------------
                    'CodeSafe: ReferenceModeFactType is functionally dependand on Me.ReferenceMode
                    '-------------------------------------------------------------------------------
                    If (Trim(Me.ReferenceMode) = "") And (value IsNot Nothing) Then
                        Throw New Exception("Tried to set ReferenceModeFactType for EntityType.Id: " & _
                                            Me.Id & ", where there is no ReferenceMode set for the EntityType")
                    End If
                Catch ex As Exception
                    Dim lsMessage As String
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
                End Try
            End Set
        End Property


        <XmlIgnore()> _
        Public ReferenceModeValueType As FBM.ValueType = Nothing

        <XmlIgnore()> _
        Public _PreferredIdentifierRCId As String
        <XmlIgnore()> _
        Public Property PreferredIdentifierRCId() As String
            Get
                Return Me._PreferredIdentifierRCId
            End Get
            Set(ByVal value As String)
                Me._PreferredIdentifierRCId = value
            End Set
        End Property

        <XmlElement()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _ReferenceModeRoleConstraint As FBM.RoleConstraint = Nothing
        <XmlIgnore()> _
        Public Property ReferenceModeRoleConstraint() As FBM.RoleConstraint
            Get
                Return Me._ReferenceModeRoleConstraint
            End Get
            Set(ByVal value As FBM.RoleConstraint)
                Me._ReferenceModeRoleConstraint = value
                If IsSomething(Me._ReferenceModeRoleConstraint) Then
                    Me.PreferredIdentifierRCId = Me._ReferenceModeRoleConstraint.Id
                Else
                    Me.PreferredIdentifierRCId = ""
                End If
            End Set
        End Property

        <XmlAttribute()> _
        Public IsObjectifyingEntityType As Boolean = False

        <XmlIgnore()> _
        Public ObjectifiedFactType As FBM.FactType = Nothing

        <XmlIgnore()> _
        Public value_constraint As New StringCollection
        <XmlIgnore()> _
        <CategoryAttribute("Entity Type"), _
         Browsable(False), _
         [ReadOnly](False), _
         DescriptionAttribute("The List of Values that Objects of this Entity Type may take."), _
         Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Property ValueConstraint() As StringCollection 'StringCollection 
            '   DefaultValueAttribute(""), _
            '   BindableAttribute(True), _
            '   DesignOnly(False), _
            Get
                Return Me.value_constraint
            End Get
            Set(ByVal Value As StringCollection)
                Me.value_constraint = Value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _IsPersonal As Boolean = False
        <XmlAttribute()> _
        <CategoryAttribute("Entity Type"), _
        DefaultValueAttribute(False), _
        DescriptionAttribute("True if the Entity Type is personal.")> _
        Public Property IsPersonal As Boolean
            Get
                Return Me._IsPersonal
            End Get
            Set(value As Boolean)
                Me._IsPersonal = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _IsIndependant As Boolean = False
        <XmlAttribute()> _
        <CategoryAttribute("Entity Type"), _
        DefaultValueAttribute(False), _
        DescriptionAttribute("True if the Entity Type is independant.")> _
        Public Property IsIndependant As Boolean
            Get
                Return Me._IsIndependant
            End Get
            Set(value As Boolean)
                Me._IsIndependant = value
            End Set
        End Property


        <XmlIgnore()> _
        Public KLLetter As String  'When doing proofs in ORM, is the letter within the formal theory of KL (Knowledge Language) assigned to this EntityType
        <XmlIgnore()> _
        Public date_created As Date 'The date that the EntityType was created within Richmond.
        <XmlIgnore()> _
        Public last_modified As Date 'The date on which the EntityType was last modified.
        <XmlIgnore()> _
        Public last_modified_user_id As String 'The Id of the Richmond User who last created/modified the EntityType.

        Private _ModelError As New List(Of FBM.ModelError)
        Public Property ModelError() As System.Collections.Generic.List(Of ModelError) Implements iValidationErrorHandler.ModelError
            Get
                Return Me._ModelError
            End Get
            Set(ByVal value As System.Collections.Generic.List(Of ModelError))
                Me._ModelError = value
            End Set
        End Property

        Public ReadOnly Property HasModelError() As Boolean Implements iValidationErrorHandler.HasModelError
            Get
                Return Me.ModelError.Count > 0
            End Get
        End Property

        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded
        Public Event SubtypeConstraintAdded(ByRef arSubtypeConstraint As FBM.tSubtypeRelationship)
        Public Event SubtypeConstraintRemoved(ByRef arSubtypeConstraint As FBM.tSubtypeRelationship)
        Public Event ReferenceModeChanged(ByVal asNewReferenceMode As String, ByVal abSimpleAssignment As Boolean)
        Public Event ReferenceModeFactTypeChanged(ByRef arNewReferenceModeFactType As FBM.FactType)
        Public Event ReferenceModeValueTypeChanged(ByRef arNewReferenceModeValueType As FBM.ValueType)
        Public Event RemovedFromModel()
        Public Event PreferredIdentifierRCIdChanged(ByVal asNewPreferredIndentifierRCId As String)
        Public Event ReferenceModeRoleConstraintChanged(ByRef arNewReferenceModeRoleConstraint As FBM.RoleConstraint)
        Public Event IsObjectifyingEntityTypeChanged(ByVal abNewIsObjectifyingEntityType As Boolean)
        Public Event IsIndependantChanged(ByVal abNewIsIndependant As Boolean)
        Public Event IsPersonalChanged(ByVal abNewIsPersonal As Boolean)
        Public Event ObjectifiedFactTypeChanged(ByRef arNewObjectifiedFactType As FBM.FactType)
        Public Event SimpleReferenceSchemeRemoved()

        Public Sub New()
            '--------
            'Default
            '--------
            Me.ConceptType = pcenumConceptType.EntityType

            Me.Id = "New Entity Type"
            Me._Name = "New Entity Type"

        End Sub

        Public Sub New(ByVal arModel As FBM.Model, ByVal aiLanguageId As pcenumLanguage, Optional ByVal as_entity_type_name As String = Nothing, Optional ByVal arValueType As FBM.ValueType = Nothing, Optional ByVal ab_use_entity_type_name As Boolean = False)

            Call Me.New()

            Me.Model = arModel

            If IsSomething(as_entity_type_name) Then
                Me.Name = as_entity_type_name
            Else
                Me.Name = "New Entity Type"
            End If

            If IsSomething(arValueType) Then
                Me.ReferenceModeValueType = arValueType
            End If

            If ab_use_entity_type_name Then
                Me.Id = Trim(Me.Name)
            Else
                Me.Id = System.Guid.NewGuid.ToString
            End If

            Me.Concept = New FBM.Concept(Me.Id)

        End Sub

        Public Sub New(ByVal arModel As FBM.Model, ByVal aiLanguageId As pcenumLanguage, ByVal as_entity_type_name As String, ByVal as_EntityTypeId As String, Optional ByVal arValueType As FBM.ValueType = Nothing)

            Call Me.New()

            Me.Model = arModel

            Me.Name = as_entity_type_name
            Me.Id = as_EntityTypeId

            Me.Concept = New FBM.Concept(Me.Id)

            If IsSomething(arValueType) Then
                Me.ReferenceModeValueType = arValueType
            End If

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.EntityType) As Boolean Implements System.IEquatable(Of FBM.EntityType).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByName(ByVal other As FBM.EntityType) As Boolean

            If Me.Name = other.Name Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByNameLike(ByVal other As FBM.EntityType) As Boolean

            If other.Name Like (Me.Name & "*") Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsBySignature(ByVal other As FBM.EntityType) As Boolean

            If other.GetSignature = Me.GetSignature Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Used to sort EntityTypes based on whether they have a SubtypeConstraint or not. Those with not first.
        ''' </summary>
        ''' <param name="aoA"></param>
        ''' <param name="aoB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CompareSubtypeConstraintExistance(ByVal aoA As FBM.EntityType, ByVal aoB As FBM.EntityType) As Integer

            Try

                Return aoA.SubtypeConstraint.Count - aoB.SubtypeConstraint.Count

            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
                Return 0
            End Try

        End Function

        Public Overloads Function Clone(ByRef arModel As FBM.Model, Optional ByVal abAddToModel As Boolean = False) As Object

            Dim lrEntityType As New FBM.EntityType
            Dim lrParentEntityType As FBM.EntityType

            If arModel.EntityType.Exists(AddressOf Me.Equals) Then
                '---------------------------------------------------------------------------------------------------------------------
                'The target EntityType already exists in the target Model, so return the existing EntityType (from the target Model)
                '  NB This becomes especially necessary when cloning ParentEntityTypes (see below).
                '  20150127-There seems no logical reason to clone an EntityType to a target Model if it already exists in the target
                '  Model. This method is used when copying/pasting from one Model to a target Model, and (in general) the EntityType
                '  won't exist in the target Model. If it does, then that's the EntityType that's needed.
                '  NB Testing to see if the Signature of the EntityType already exists in the target Model is already performed in the
                '  Paste proceedure before dropping the EntityType onto a target Page/Model. If there is/was any clashes, then the 
                '  EntityType being copied/pasted will have it's Id/Name/Symbol changed and will not be affected by this test to see
                '  if the EntityType already exists in the target Model.
                '---------------------------------------------------------------------------------------------------------------------
                lrEntityType = arModel.EntityType.Find(AddressOf Me.Equals)
            Else
                With Me
                    lrEntityType.Model = arModel
                    lrEntityType.Id = .Id
                    'Call lrEntityType.SetName(.Name)  '20170824-This doesn't seem to be needed. Changed to just setting the Name (below)
                    lrEntityType.Name = .Name
                    lrEntityType.Symbol = .Symbol
                    lrEntityType.ReferenceMode = .ReferenceMode
                    lrEntityType.ShortDescription = .ShortDescription
                    lrEntityType.LongDescription = .LongDescription
                    lrEntityType.value_constraint = .value_constraint
                    lrEntityType.KLLetter = .KLLetter
                    lrEntityType.IsMDAModelElement = .IsMDAModelElement
                    lrEntityType.IsIndependant = .IsIndependant
                    lrEntityType.IsPersonal = .IsPersonal

                    If abAddToModel Then
                        arModel.AddEntityType(lrEntityType)
                    End If

                    For Each lrParentEntityType In .parentModelObjectList
                        lrEntityType.parentModelObjectList.Add(lrParentEntityType.Clone(arModel))
                    Next

                    If IsSomething(.ReferenceModeValueType) Then
                        lrEntityType.ReferenceModeValueType = .ReferenceModeValueType.Clone(arModel, abAddToModel)
                    End If

                    If IsSomething(.ReferenceModeFactType) Then
                        lrEntityType.ReferenceModeFactType = .ReferenceModeFactType.Clone(arModel, abAddToModel)
                    End If

                    If IsSomething(.ReferenceModeRoleConstraint) Then
                        lrEntityType.ReferenceModeRoleConstraint = .ReferenceModeRoleConstraint.Clone(arModel, abAddToModel)
                    End If

                End With
            End If

            Return lrEntityType

        End Function

        ''' <summary>
        ''' Used to 'Sort' Enumerated lists Of FBM.tEntityType
        ''' </summary>
        ''' <param name="ao_a"></param>
        ''' <param name="ao_b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CompareEntityTypeNames(ByVal ao_a As FBM.EntityType, ByVal ao_b As FBM.EntityType) As Integer

            Return StrComp(ao_a.Name, ao_b.Name)

        End Function

        ''' <summary>
        ''' Adds a BinaryFactType relation between the EntityType and a ValueType. Adds the ValueType to the Model if it does not already exist.
        ''' </summary>
        ''' <param name="arValueType"></param>
        ''' <param name="aiRelationMultiplicityValue"></param>
        ''' <remarks></remarks>
        Public Function AddBinaryRelationToValueType(ByRef arValueType As FBM.ValueType, ByVal aiRelationMultiplicityValue As pcenumBinaryRelationMultiplicityType) As FBM.FactType

            '------------------------------------------------------------------------------
            'Add the ValueType to the Model if it does not already exist within the Model
            '------------------------------------------------------------------------------
            If IsSomething(Me.Model.ValueType.Find(AddressOf arValueType.Equals)) Then
                '-------------------------------------------
                'ValueType already exists within the Model
                '-------------------------------------------
            Else
                Me.Model.AddValueType(arValueType)
            End If

            '---------------------------------------
            'Create the FactType for the relation.
            '---------------------------------------
            Dim lrFactType As New FBM.FactType
            Dim larModelObject As New List(Of FBM.ModelObject)
            Dim lsFactTypeName As String = Me.Name & arValueType.Name
            Dim larRole As New List(Of FBM.Role)

            '---------------------------------------------------------------------
            'Create the list of ModelObjects referenced by Roles in the FactType
            '---------------------------------------------------------------------
            larModelObject.Add(Me)
            larModelObject.Add(arValueType)
            '---------------------
            'Create the FactType
            '---------------------
            lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject)

            '-----------------------------------------------------------------------------------
            'Create the InternalUniquenessConstraint (MultiplicityConstraint) for the FactType
            '-----------------------------------------------------------------------------------
            Select Case aiRelationMultiplicityValue
                Case Is = pcenumBinaryRelationMultiplicityType.OneToOne
                    larRole.Add(lrFactType.FindFirstRoleByModelObject(Me))
                    Call lrFactType.CreateInternalUniquenessConstraint(larRole)
                    larRole.Clear()
                    larRole.Add(lrFactType.FindFirstRoleByModelObject(arValueType))
                    Call lrFactType.CreateInternalUniquenessConstraint(larRole)
                Case Is = pcenumBinaryRelationMultiplicityType.OneToMany
                    larRole.Add(lrFactType.FindFirstRoleByModelObject(arValueType))
                    Call lrFactType.CreateInternalUniquenessConstraint(larRole)
                Case Is = pcenumBinaryRelationMultiplicityType.ManyToOne
                    larRole.Add(lrFactType.FindFirstRoleByModelObject(Me))
                    Call lrFactType.CreateInternalUniquenessConstraint(larRole)
                Case Is = pcenumBinaryRelationMultiplicityType.ManyToMany
                    larRole.Add(lrFactType.FindFirstRoleByModelObject(Me))
                    larRole.Add(lrFactType.FindFirstRoleByModelObject(arValueType))
                    Call lrFactType.CreateInternalUniquenessConstraint(larRole)
            End Select

            Return lrFactType

        End Function

        Public Sub AddDataInstance(ByVal asDataInstance As String)

            Me.Instance.Add(asDataInstance)

            If IsSomething(Me.parentModelObjectList) Then
                Dim lrEntityType As FBM.EntityType
                For Each lrEntityType In Me.parentModelObjectList
                    lrEntityType.AddDataInstance(asDataInstance)
                Next
            End If

        End Sub

        ''' <summary>
        ''' If the EntityType has a Compound ReferenceMode returns an enumeration of the Instance/Identity,
        '''   ELSE returns the supplied Instance
        ''' </summary>
        ''' <param name="asInstance"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EnumerateInstance(ByVal asInstance As String) As String

            Dim lasReturnString As New List(Of String)
            Dim lsReturnString As String

            If Me.HasCompoundReferenceMode Then

                Dim lrExternalUniquenessConstraint As FBM.RoleConstraint
                lrExternalUniquenessConstraint = Me.ReferenceModeRoleConstraint

                'lrExternalUniquenessConstraint.RoleConstraintRole.Sort()

                Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                Dim lrFactType As FBM.FactType
                For Each lrRoleConstraintRole In lrExternalUniquenessConstraint.RoleConstraintRole

                    lrFactType = lrRoleConstraintRole.Role.FactType

                    Dim lrRole As FBM.Role

                    lrRole = lrFactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id)

                    Dim lrFactData As New FBM.FactData(lrRole, New FBM.Concept(asInstance))
                    Dim lrFactPredicate As New FBM.FactPredicate
                    Dim lrFact As FBM.Fact
                    Dim lrReturnFactData As FBM.FactData

                    lrFactPredicate.data.Add(lrFactData)

                    lrFact = lrFactType.Fact.Find(AddressOf lrFactPredicate.EqualsByRoleIdData)

                    If IsSomething(lrFact) Then
                        lrReturnFactData = lrFact.GetFactDataByRoleId(lrRoleConstraintRole.Role.Id)
                        lasReturnString.Add(lrReturnFactData.Data)
                    Else
                        Return asInstance
                    End If

                Next 'RoleConstraintRole

                Dim liInd As Integer = 0

                Dim lsIdentificationPart As String
                lsReturnString = "{"
                For Each lsIdentificationPart In lasReturnString
                    lsReturnString &= lsIdentificationPart
                    If liInd < lasReturnString.Count - 1 Then
                        lsReturnString &= ","
                    End If
                    liInd += 1
                Next
                lsReturnString &= "}"
                Return lsReturnString
            Else
                Return asInstance
            End If

        End Function


        ''' <summary>
        ''' Returns TRUE if there are any Roles (within FactTypes) that are associated with the EntityType, else returns FALSE
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExistsRolesAssociatedWithEntityType() As Boolean

            ExistsRolesAssociatedWithEntityType = False

            Dim larRoles = From FactType In Me.Model.FactType _
                           From Role In FactType.RoleGroup _
                          Where Role.TypeOfJoin = pcenumRoleJoinType.EntityType _
                            And Role.JoinsEntityType Is Me _
                         Select Role

            Dim lrRole As New FBM.Role

            For Each lrRole In larRoles
                Return True
            Next

        End Function

        ''' <summary>
        ''' Returns a list of the Roles that join to the EntityType.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetAdjoinedRoles() As List(Of FBM.Role)

            Try

                Dim lrRole As FBM.Role
                Dim larReturnRoles As New List(Of FBM.Role)

                Dim larRoles = From FactType In Me.Model.FactType _
                               From Role In FactType.RoleGroup _
                              Where Role.TypeOfJoin = pcenumRoleJoinType.EntityType _
                                And Role.JoinedORMObject.Id = Me.Id _
                             Select Role

                For Each lrRole In larRoles
                    larReturnRoles.Add(lrRole)
                Next
                Return larReturnRoles

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return Nothing
            End Try

        End Function

        Public Function GetCountRolesAssociatedWithEntityType() As Integer


            Dim larRoles = From FactType In Me.Model.FactType _
                           From Role In FactType.RoleGroup _
                          Where Role.TypeOfJoin = pcenumRoleJoinType.EntityType _
                            And Role.JoinsEntityType Is Me _
                         Select Role

            Return larRoles.Count

        End Function

        ''' <summary>
        ''' Returns TRUE if there are any Subtypes of the EntityType else returns false.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExistsSubyTypeForEntityType() As Boolean

            ExistsSubyTypeForEntityType = False

            Dim larSubTypes = From SubType In Me.Model.EntityType _
                              From EntityType In SubType.parentModelObjectList _
                              Where EntityType.Id = Me.Id _
                              Select EntityType

            Dim lrEntityType As New FBM.EntityType

            For Each lrEntityType In larSubTypes
                Return True
            Next

        End Function


        Public Overrides Function CanSafelyRemoveFromModel() As Boolean

            Return False
        End Function

        ''' <summary>
        ''' Changes the Model of the EntityType to the target Model.
        ''' </summary>
        ''' <param name="arTargetModel">The Model to which the EntityType will be associated on completion of this method.</param>
        ''' <remarks></remarks>
        Public Sub ChangeModel(ByRef arTargetModel As FBM.Model)

            Me.Model = arTargetModel

            If arTargetModel.EntityType.Exists(AddressOf Me.Equals) Then
                '----------------------------------------------
                'The EntityType is already in the TargetModel
                '----------------------------------------------
            Else
                arTargetModel.AddEntityType(Me)
            End If

        End Sub

        ''' <summary>
        ''' Creates a ReferenceMode where there is none. Sets up ReferenceModeFactType, ReferenceModeValueType, ReferenceModeRoleConstraint, PreferredIdentifierRCId.
        ''' </summary>
        ''' <param name="asReferenceMode">The ReferenceMode to be assigned to the EntityType</param>
        ''' <param name="asValueTypeName">The name for the referenced ValueType if it is already known.</param>
        ''' <remarks>This method should not be called if any of the following are already set for the EntityType: 
        '''              ReferenceModeFactType,
        '''              ReferenceModeValueType,
        '''              ReferenceModeRoleConstraint,
        '''              PreferredIdentifierRCId.
        ''' </remarks>
        Public Sub CreateReferenceMode(ByVal asReferenceMode As String, _
                                       Optional ByVal asValueTypeName As String = Nothing)

            Dim lsMessage As String = ""

            Try

                '--------------------------------------------------------------------------------------
                '20150610-VM-Can likely ditch the commented out code below.
                '  Is covered by the code below that and EntityType may very well have ReferenceModeRoleConstraint
                '  and PreferredIdentifierRCId, because the EntityType might have a CompoundReferenceScheme.
                ''--------------------------------------------------------------------------------------------------------
                ''CodeSafe: Throw an error if any of the following are already set for the EntityType: 
                ''  ReferenceModeFactType, ReferenceModeValueType, ReferenceModeRoleConstraint, PreferredIdentifierRCId.
                ''--------------------------------------------------------------------------------------------------------
                'If IsSomething(Me.ReferenceModeFactType) Or _
                '   IsSomething(Me.ReferenceModeValueType) Or _
                '   IsSomething(Me.ReferenceModeRoleConstraint) Or _
                '   Trim(PreferredIdentifierRCId) <> "" Then

                '    lsMessage = "Tried to setup a ReferenceMode for EntityType where EntityType already has one of the following:"
                '    lsMessage &= vbCrLf & "ReferenceModeFactType, ReferenceModeValueType, ReferenceModeRoleConstraint, PreferredIdentifierRCId"
                '    lsMessage &= vbCrLf & "EntityType.Id : '" & Me.Id & "'"
                '    Throw New Exception(lsMessage)
                'End If

                '-------------------------------------------------------------------------------------------------
                'Code Safe: Throw an error if the ReferenceModeValueType or ReferenceModeFactType already exist.
                '-------------------------------------------------------------------------------------------------
                If IsSomething(Me.ReferenceModeValueType) Or IsSomething(Me.ReferenceModeFactType) Then
                    lsMessage = "Tried to create a ReferenceMode for an EntityType, where ReferenceMode already exists."
                    lsMessage &= vbCrLf & "EntityType.Id : '" & Me.Id & "'"
                    Throw New Exception(lsMessage)
                End If


                Dim lsValueTypeName As String = ""
                If asValueTypeName IsNot Nothing Then
                    lsValueTypeName = asValueTypeName
                Else
                    Me.ReferenceMode = asReferenceMode
                    lsValueTypeName = Me.MakeReferenceModeName
                End If

                Dim lsActualModelElementName As String = ""
                Dim lrValueType As FBM.ValueType

                If Me.Model.GetConceptTypeByNameFuzzy(lsValueTypeName, lsActualModelElementName) = pcenumConceptType.ValueType Then
                    '-----------------------------------------------
                    'The ValueType already exists within the Model
                    '-----------------------------------------------
                    Me.ReferenceMode = lsActualModelElementName
                    lrValueType = Me.Model.GetModelObjectByName(lsActualModelElementName)

                    If Not lrValueType.IsIndependant Then
                        Dim laoRole = From Role In Me.Model.Role _
                                      Where Role.JoinedORMObject Is lrValueType _
                                      Select Role

                        Dim lrRole As FBM.Role

                        For Each lrRole In laoRole
                            lrRole.ReassignJoinedModelObject(Me)
                        Next
                    End If
                Else
                    Me.ReferenceMode = asReferenceMode
                    '------------------------------------------------
                    'Add the ValueType to the Model/ModelDictionary
                    '------------------------------------------------
                    lrValueType = New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lsValueTypeName, True)                
                    Me.Model.AddValueType(lrValueType, False)
                End If

                '------------------------------------------
                'Create and add the FactType to the Model
                '------------------------------------------
                Dim lrFactType As FBM.FactType
                Dim lsFactTypeName As String = Me.Name & "Has" & lrValueType.Name
                lsFactTypeName = Me.Model.CreateUniqueFactTypeName(lsFactTypeName, 0)

                Dim larModelObjectList As New List(Of FBM.ModelObject)

                larModelObjectList.Add(Me)
                larModelObjectList.Add(lrValueType)

                lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObjectList, True, False)
                lrFactType.IsPreferredReferenceMode = True

                lrFactType.FindFirstRoleByModelObject(Me).Mandatory = True

                '----------------------------------------------------------------
                'Create the UniquenessConstraints for the ReferenceModeFactType
                '----------------------------------------------------------------
                Dim larRole As New List(Of FBM.Role)
                larRole.Add(lrFactType.FindFirstRoleByModelObject(Me))
                lrFactType.CreateInternalUniquenessConstraint(larRole, False, False)

                Dim lrRoleConstraint As FBM.RoleConstraint
                larRole.Clear()
                larRole.Add(lrFactType.FindFirstRoleByModelObject(lrValueType))
                lrRoleConstraint = lrFactType.CreateInternalUniquenessConstraint(larRole, True, False)

                Me.ReferenceModeRoleConstraint = lrRoleConstraint


                '-----------------------------------------------------------
                'Create the FactTypeReadings for the ReferenceModeFactType
                '-----------------------------------------------------------
                Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType)
                '------------------------------------------------------------------------------
                'No Longer supported (v1.13 of the database Model).
                'lrFactTypeReading.ObjectTypeList = New List(Of FBM.ModelObject)
                'lrFactTypeReading.ObjectTypeList.Add(Me)
                'lrFactTypeReading.ObjectTypeList.Add(lrValueType)
                Dim lrPredicatePart As New FBM.PredicatePart(Me.Model, lrFactTypeReading)
                lrPredicatePart.SequenceNr = 1
                lrPredicatePart.Role = lrFactType.GetRoleByJoinedObjectTypeId(Me.Id)
                lrPredicatePart.PredicatePartText = "has"
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)

                lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading)
                lrPredicatePart.SequenceNr = 2
                lrPredicatePart.Role = lrFactType.GetRoleByJoinedObjectTypeId(lrValueType.Id)
                lrPredicatePart.PredicatePartText = ""
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)

                '--------------------------------------------
                'Add the Fact Type Reading to the Fact Type
                '--------------------------------------------
                lrFactType.FactTypeReading.Add(lrFactTypeReading)

                lrFactTypeReading = New FBM.FactTypeReading(lrFactType)
                lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading)
                lrPredicatePart.SequenceNr = 1
                lrPredicatePart.Role = lrFactType.GetRoleByJoinedObjectTypeId(lrValueType.Id)
                lrPredicatePart.PredicatePartText = "is of"
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)

                lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading)
                lrPredicatePart.SequenceNr = 2
                lrPredicatePart.Role = lrFactType.GetRoleByJoinedObjectTypeId(Me.Id)
                lrPredicatePart.PredicatePartText = ""
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)

                '--------------------------------------------
                'Add the Fact Type Reading to the Fact Type
                '--------------------------------------------
                lrFactType.FactTypeReading.Add(lrFactTypeReading)

                Me.ReferenceModeValueType = lrValueType
                Me.ReferenceModeFactType = lrFactType

                Me.Model.MakeDirty()

                RaiseEvent PreferredIdentifierRCIdChanged(Me.PreferredIdentifierRCId)

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try


        End Sub


        Public Function CreateSubtypeConstraint(ByVal ar_parentEntityType As FBM.EntityType) As FBM.tSubtypeRelationship

            Dim lrSubtypeConstraint As New FBM.tSubtypeRelationship

            lrSubtypeConstraint.Model = Me.Model
            lrSubtypeConstraint.EntityType = Me
            lrSubtypeConstraint.parentEntityType = ar_parentEntityType

            Me.parentModelObjectList.Add(ar_parentEntityType)
            ar_parentEntityType.childModelObjectList.Add(Me)

            '---------------------------------------------
            'Create a FactType for the SubtypeConstraint
            '---------------------------------------------
            Dim lsFactTypeName As String = ""
            Dim larModelObject As New List(Of FBM.ModelObject)
            Dim larRole As New List(Of FBM.Role)

            lsFactTypeName = VievLibrary.Strings.RemoveWhiteSpace(Me.Name & "IsSubtypeOf" & ar_parentEntityType.Name)
            larModelObject.Add(Me)
            larModelObject.Add(ar_parentEntityType)

            lrSubtypeConstraint.FactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject, False)
            lrSubtypeConstraint.FactType.IsSubtypeConstraintFactType = True

            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lasPredicatePart As New List(Of String)
            lasPredicatePart.Add("is")
            lasPredicatePart.Add("")

            larRole.Add(lrSubtypeConstraint.FactType.RoleGroup(0))
            larRole.Add(lrSubtypeConstraint.FactType.RoleGroup(1))
            lrFactTypeReading = New FBM.FactTypeReading(lrSubtypeConstraint.FactType, larRole, lasPredicatePart)
            lrSubtypeConstraint.FactType.FactTypeReading.Add(lrFactTypeReading)

            larRole.Clear()
            larRole.Add(lrSubtypeConstraint.FactType.RoleGroup(1))
            larRole.Add(lrSubtypeConstraint.FactType.RoleGroup(0))
            lrFactTypeReading = New FBM.FactTypeReading(lrSubtypeConstraint.FactType, larRole, lasPredicatePart)
            lrSubtypeConstraint.FactType.FactTypeReading.Add(lrFactTypeReading)

            Me.SubtypeConstraint.Add(lrSubtypeConstraint)

            RaiseEvent SubtypeConstraintAdded(lrSubtypeConstraint)

            Call Me.Model.MakeDirty()

            Return lrSubtypeConstraint

        End Function


        ''' <summary>
        ''' Returns the unique Signature of the EntityType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetSignature() As String

            Dim lsSignature As String

            lsSignature = Me.Id

            If IsSomething(Me.ReferenceModeValueType) Then
                lsSignature &= Me.ReferenceModeValueType.Id & Me.ReferenceModeValueType.DataType
                lsSignature &= Me.PreferredIdentifierRCId
            End If

            Return lsSignature

        End Function

        ''' <summary>
        ''' Returns True if the EntityType has a Compound Reference Mode, else returns False
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasCompoundReferenceMode() As Boolean

            HasCompoundReferenceMode = False

            If (Me.ReferenceMode = "") And (Me.ReferenceModeRoleConstraint Is Nothing) Then
                HasCompoundReferenceMode = False
            ElseIf IsSomething(Me.ReferenceModeRoleConstraint) Then
                If Me.ReferenceModeRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    '-----------------------------------------------------------------------------------------------
                    'Must be an EntityType without a Compound Reference Mode.
                    '  Only EntityTypes with a ReferenceModeRoleConstraint that is an ExternalUniquenessConstraint
                    '  are those EntityTypes that have a CompoundReferenceMode.
                    '-----------------------------------------------------------------------------------------------
                    HasCompoundReferenceMode = False
                Else
                    '-----------------------------------------------------------------------------------------
                    'Must be an EntityType with a ReferenceMode and where that ReferenceMode is defined by a 
                    '  ReferenceModeRoleConstraint that is an ExternalUniquenessConstraint
                    '-----------------------------------------------------------------------------------------
                    HasCompoundReferenceMode = True
                End If
            End If

        End Function

        Public Overridable Function HasPrimaryReferenceScheme() As Boolean

            If Me.HasSimpleReferenceMode Or Me.HasCompoundReferenceMode Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overridable Function HasSimpleReferenceMode() As Boolean

            Try
                If Trim(Me.ReferenceMode) = "" Then
                    Return False
                Else
                    If Me.ReferenceModeValueType Is Nothing Or _
                       Me.ReferenceModeFactType Is Nothing Then

                        '--------------------------------------------------------------------------------------
                        'CodeSafe: Model is in an unstable state. Remove the ReferenceMode and warn the user.
                        '--------------------------------------------------------------------------------------
                        Me.ReferenceMode = ""
                        Me.ReferenceModeRoleConstraint = Nothing

                        Me.Model.Save()

                        Dim lsMessage As String = ""
                        lsMessage = "Entity Type, '" & Me.Id & "', had a Reference Mode but no associated Value Type or Fact Type."
                        lsMessage &= vbCrLf & "As a precaution, the ReferenceMode has been removed and the Model saved."

                        Throw New Exception(lsMessage)
                    Else
                        Return True
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return False
            End Try

        End Function

        ''' <summary>
        ''' Returns True if the EntityType is a Subtype, else returns False.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsSubtype() As Boolean

            If Me.parentModelObjectList.Count = 0 Then
                Return False
            Else
                Return True
            End If

        End Function


        Public Sub ModifyDataInstance(ByVal asOldValue As String, ByVal asNewValue As String)

            If Me.Instance.IndexOf(asOldValue) > 0 Then

                Me.Instance(Me.Instance.IndexOf(asOldValue)) = asNewValue

                If IsSomething(Me.parentModelObjectList) Then
                    Dim lrEntityType As FBM.EntityType
                    For Each lrEntityType In Me.parentModelObjectList
                        lrEntityType.ModifyDataInstance(asOldValue, asNewValue)
                    Next
                End If
            End If


        End Sub

        ''' <summary>
        ''' Removes a Data Instance from the Entity and all associated Sample Populations (within associated Fact Types).
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveDataInstance(ByVal asInstance As String)

            Dim lrFact As FBM.Fact

            Dim larFact = From FactType In Me.Model.FactType _
                          From Fact In FactType.Fact _
                          From FactData In Fact.Data _
                          Where FactData.Role.JoinedORMObject Is Me _
                          Select Fact

            For Each lrFact In larFact.ToArray
                '----------------------------------------------------------------------------
                'Remove the associated Fact with cascading delete on those Facts that are 
                '  SamplPopulations of an ObjectifiedFactType
                '--------------------------------------------------------------------------
                lrFact.FactType.RemoveFactById(lrFact)
            Next

            Me.Instance.Remove(asInstance)

        End Sub

        Public Sub RemoveDataInstanceRecursive(ByVal asInstance As String, ByVal aarObjectType As List(Of FBM.ModelObject))

            Dim lrEntityType As FBM.EntityType

            Call Me.RemoveDataInstance(asInstance)

            aarObjectType.Add(Me)

            If Me.parentModelObjectList.Count > 0 Then
                For Each lrEntityType In Me.parentModelObjectList
                    Call lrEntityType.RemoveDataInstanceRecursive(asInstance, aarObjectType)
                Next
            End If

            Dim larChildEntityType = From EntityType In Me.Model.EntityType _
                                    From lrParentEntityType In EntityType.parentModelObjectList _
                                    Where lrParentEntityType Is Me _
                                    Select EntityType

            For Each lrEntityType In larChildEntityType
                If Not aarObjectType.Contains(lrEntityType) Then
                    Call lrEntityType.RemoveDataInstanceRecursive(asInstance, aarObjectType)
                End If
            Next

        End Sub

        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean


            Try
                If abForceRemoval Then
                Else
                    If Me.IsObjectifyingEntityType Then
                        MsgBox("You cannot remove Entity Type, '" & Trim(Me.Name) & "' while it is the Objectifying Entity Type of a Fact Type.")
                        Return False
                        Exit Function
                    End If

                    If Me.ExistsRolesAssociatedWithEntityType And _
                       Not (Me.HasSimpleReferenceMode And _
                            (Me.GetCountRolesAssociatedWithEntityType = 1)) Then
                        MsgBox("You cannot remove Entity Type, '" & Trim(Me.Name) & "' while there are Fact Types/Roles within the Model associated with the Entity Type.")
                        Return False
                        Exit Function
                    End If

                    If Me.ExistsSubyTypeForEntityType Then
                        MsgBox("You cannot remove Entity Type, '" & Trim(Me.Name) & "' while there are Subtypes of the Entity Type within the Model.")
                        Return False
                        Exit Function
                    End If
                End If

                '-----------------------------------------------------------------------------------------------------------------
                'Set the PreferredIdentifierRCId to Nothing, regardless of whether the Entity Type has a Simple Reference Scheme,
                '  so that if the ET has a Simple Reference Scheme the RC can be removed from the Model
                '  without question. Otherwise when trying to remove the RC from the Model an error will be triggered because
                '  it is the RC that identifies the Simple Reference Scheme (Reference Mode) for the ET.
                '-----------------------------------------------------------------------------------------------------------------
                Me.PreferredIdentifierRCId = Nothing

                Dim lrFactType As FBM.FactType
                lrFactType = Me.ReferenceModeFactType
                If IsSomething(Me.ReferenceModeFactType) Then
                    Call lrFactType.RemoveFromModel()
                End If

                Dim lrValueType As FBM.ValueType
                lrValueType = Me.ReferenceModeValueType
                If IsSomething(Me.ReferenceModeValueType) Then
                    Call lrValueType.RemoveFromModel()
                End If

                Me.Model.RemoveEntityType(Me)

                RaiseEvent RemovedFromModel()

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try


        End Function

        ''' <summary>
        ''' Removes the Simple Reference Scheme from the Entity Type.
        '''   NB Does not remove the ReferenceModeFactType from the Model. It is possible to remove the Simple Reference Scheme without
        '''   removing the associated FactType and ValueType. Other functions remove those if necessary.
        '''   See EntityTypeInstance._EntityType_ReferenceModeChanged (event Raised below).
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveSimpleReferenceScheme(Optional ByVal abRaiseEventReferenceModeChanged As Boolean = True)

            Dim lsMessage As String = ""

            Try
                If Me.HasSimpleReferenceMode Then

                    Me.ReferenceModeFactType.IsPreferredReferenceMode = False
                    Me.ReferenceModeFactType = Nothing
                    Me.ReferenceModeValueType = Nothing
                    Me.ReferenceModeRoleConstraint.SetIsPreferredIdentifier(False)
                    Me.ReferenceModeRoleConstraint = Nothing
                    'NB Me.PreferredIdentifierRCId set in the Property.Set method of ReferenceModeRoleConstraint (set to "")
                    Me.ReferenceMode = ""

                    '------------------------------------------------------------------------------------------------------------------------------
                    'RaiseEvent SimpleReferenceSchemeRemoved()
                    '  Not required to be raised in this instance as the following events will achieve the same result.
                    '------------------------------------------------------------------------------------------------------------------------------
                    If abRaiseEventReferenceModeChanged Then
                        '--------------------------------------------------------------------------------------------------------------------------
                        'If within the process of removing a ReferenceMode/SimpleReferenceScheme, the following event will be raised at the end
                        '  of that process. 
                        '  abRaiseEventReferenceMode will be set to False when removing the ReferenceModeFactType from the model
                        '  which calls this method.
                        '--------------------------------------------------------------------------------------------------------------------------
                        RaiseEvent ReferenceModeChanged(Me.ReferenceMode, False)
                    End If

                    '------------------------------------------------------------------------------------------------------------------------------
                    'Call this first, because will force EntityType instances to expand the ReferenceMode.
                    '  e.g. If when changing the preferredReferenceScheme of an EntityType from a SimpleReferenceScheme to a CompoundReferenceScheme.
                    '  The FactType and ValueType are not removed from the Model, just the ReferenceScheme has changed.
                    '  NB If this method has been called in the process of actually removing the FactType of a SimpleReferenceScheme, then the
                    '  FactType will be removed anyway (so raising the event SimpleReferenceSchemeRemoved causes no harm).
                    '----------------------------------------------------------------------------------------------------------------------------------
                    RaiseEvent SimpleReferenceSchemeRemoved()

                    RaiseEvent ReferenceModeFactTypeChanged(Nothing)
                    RaiseEvent ReferenceModeValueTypeChanged(Nothing)
                    RaiseEvent ReferenceModeRoleConstraintChanged(Nothing)
                Else
                    lsMessage = "Tried to remove Simple Reference Scheme from an Entity Type that doesn't have one."
                    lsMessage &= vbCrLf & vbCrLf & "EntityType.Id: " & Me.Id
                    Throw New Exception(lsMessage)
                End If
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Sub RemoveSubtypeRelationship(ByRef arSubtypeConstraint As FBM.tSubtypeRelationship)

            Me.parentModelObjectList.Remove(arSubtypeConstraint.parentEntityType)
            arSubtypeConstraint.parentEntityType.childModelObjectList.Remove(Me)
            Me.SubtypeConstraint.Remove(arSubtypeConstraint)
            '--------------------------------------------------------------------------------
            'Deletion from the database is handled in FBM.SubtypeConstraint.RemoveFromModel
            '--------------------------------------------------------------------------------

            RaiseEvent SubtypeConstraintRemoved(arSubtypeConstraint)

        End Sub

        ''' <summary>
        ''' Removes unused DataInstances from the EntityType
        ''' </summary>        
        ''' <remarks></remarks>
        Public Sub RemoveUnwantedDataInstances()

            '--------------------------------------------------------------------------------------------------------------------------------
            'PSEUDOCODE
            '
            '  * FOR EACH EntityType.Instance
            '    * IF no FactData for any FactType.Fact.FactData with Role referencing the EntityType uses that EnityType.Instance THEN
            '      * Remove the EntityType.Instance from the list of Instances for the EntityType
            '    * END IF
            '  * LOOP
            '--------------------------------------------------------------------------------------------------------------------------------

            Dim lsInstance As String = ""

            For Each lsInstance In Me.Instance.ToArray

                Dim liUsingFactDataCount = Aggregate FactType In Me.Model.FactType _
                                            From Fact In FactType.Fact _
                                            From FactData In Fact.Data _
                                            Where FactData.Role.JoinedORMObject.Id = Me.Id _
                                            And FactData.Data = lsInstance _
                                            Into Count()

                If liUsingFactDataCount = 0 Then
                    Me.Instance.Remove(lsInstance)
                End If

            Next


        End Sub

        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="asReferenceMode">The ReferenceMode for the EntityType</param>
        ''' <param name="abSimpleAssignment">True if just setting the ReferenceMode without creating references or changing anything, else False</param>
        ''' <param name="asValueTypeName">Provided if the name for the ValueType is known and need not be generated.</param>
        ''' <remarks></remarks>
        Public Sub SetReferenceMode(ByVal asReferenceMode As String, _
                                    Optional ByVal abSimpleAssignment As Boolean = False, _
                                    Optional ByVal asValueTypeName As String = Nothing)

            If IsSomething(Me.ReferenceModeValueType) Or IsSomething(Me.ReferenceModeFactType) Then
                '-----------------------------------------------------------------------------------------------------------------
                ' The EntityType already has a ReferenceMode, so change the ReferenceMode values of the respective Model Objects
                '  to the new ReferenceMode
                '-----------------------------------------------------------------------------------------------------------------
                Dim lrValueType As FBM.ValueType

                Me.ReferenceMode = asReferenceMode
                lrValueType = Me.ReferenceModeValueType

                If Trim(asReferenceMode) = "" Then
                    '-------------------------------------------------------------------------------
                    'Must remove the ReferenceModeFactType and (likely) the ReferenceModeValueType
                    '-------------------------------------------------------------------------------
                    '--------------------------------------------------------------------------------------------------------------
                    'Removing the ReferenceModeFactType from the Model will remove the SimpleReferenceScheme from the EntityType.
                    '  This is because the process of removing the FactType forces the FactType to check whether it is the 
                    '  ReferenceModeFactType of an EntityType, if it is (which it is in this case) the SimpleReferenceScheme of the
                    '  EntityType is removed.
                    '--------------------------------------------------------------------------------------------------------------
                    Me.ReferenceModeRoleConstraint = Nothing

                    Me.ReferenceModeFactType.RemoveFromModel()
                    Me.ReferenceModeFactType = Nothing

                    lrValueType.RemoveFromModel()
                    Me.ReferenceModeValueType = Nothing
                Else
                    lrValueType.SetName(Me.MakeReferenceModeName)
                End If
            ElseIf abSimpleAssignment Then
                '------------------------------------------------------------------------------------------------------------------------
                'Simply setting the ReferenceMode of the EntityType and which needs to be set for all corresponding EntityTypeInstances
                '------------------------------------------------------------------------------------------------------------------------
            ElseIf Trim(asReferenceMode) = "" Then
                '--------------------------------------------------------------------------------------------
                'Tried to set null ReferenceMode to an EntityType without a ReferenceMode.
                '  Simply ignore the request.
                '--------------------------------------------------------------------------------------------
            Else
                '--------------------------------------------------------------------------------------------------------------------
                'Initially sets the ValueType name to the actual ReferernceMode string.
                '  VM-20151014-See the comments below. Setting the long name of the ValueType may (and likely should) be moved to
                '  Me.CreateReferenceMode (below), rather than at the EntityTypeInstance level.
                '--------------------------------------------------------------------------------------------------------------------
                Me.CreateReferenceMode(asReferenceMode, asValueTypeName)
            End If

            Me.ReferenceMode = asReferenceMode
            Me.Model.MakeDirty()

            '-------------------------------------------------------------------------------------------------------------------
            'VM-20151014-At this stage, 'MakeReferenceModeName', is called from within the EntityTypeInstance.
            '  i.e. A ReferenceMode of '.Id' will have a ValueType name of '<EntityTypeName>_Id' set by the EntityTypeInstance
            '  by virtue of the following Event being raised.
            '  NB I don't know why this is the case. At some stage this functionality may be moved back down to the Model level.
            '-------------------------------------------------------------------------------------------------------------------
            RaiseEvent ReferenceModeChanged(asReferenceMode, abSimpleAssignment)

        End Sub

        Public Function MakeReferenceModeName() As String

            Dim lsReferenceModeName As String = ""

            Select Case Me.ReferenceMode
                Case Is = GetEnumDescription(pcenumReferenceMode.DotHash)
                    lsReferenceModeName = Me.Name & "_#"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotLowercaseCode)
                    lsReferenceModeName = Me.Name & "_code"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotUppercaseCode)
                    lsReferenceModeName = Me.Name & "_Code"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotCamelcaseId)
                    lsReferenceModeName = Me.Name & "_Id"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotLowercaseId)
                    lsReferenceModeName = Me.Name & "_id"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotALLCAPSID)
                    lsReferenceModeName = Me.Name & "_ID"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotLowercaseName)
                    lsReferenceModeName = Me.Name & "_name"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotUppercaseName)
                    lsReferenceModeName = Me.Name & "_Name"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotUppwercaseNr)
                    lsReferenceModeName = Me.Name & "_Nr"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotLowercaseNr)
                    lsReferenceModeName = Me.Name & "_nr"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotUppercaseTitle)
                    lsReferenceModeName = Me.Name & "_Title"
                Case Is = GetEnumDescription(pcenumReferenceMode.DotLowercaseTitle)
                    lsReferenceModeName = Me.Name & "_title"
                Case Is = GetEnumDescription(pcenumReferenceMode.AUD)
                    lsReferenceModeName = "AUD"
                Case Is = GetEnumDescription(pcenumReferenceMode.CE)
                    lsReferenceModeName = "CE"
                Case Is = GetEnumDescription(pcenumReferenceMode.Celsius)
                    lsReferenceModeName = "Celcius"
                Case Is = GetEnumDescription(pcenumReferenceMode.cm)
                    lsReferenceModeName = "cm"
                Case Is = GetEnumDescription(pcenumReferenceMode.EUR)
                    lsReferenceModeName = "EUR"
                Case Is = GetEnumDescription(pcenumReferenceMode.Fahrenheit)
                    lsReferenceModeName = "Fahrenheit"
                Case Is = GetEnumDescription(pcenumReferenceMode.kg)
                    lsReferenceModeName = "kg"
                Case Is = GetEnumDescription(pcenumReferenceMode.km)
                    lsReferenceModeName = "km"
                Case Is = GetEnumDescription(pcenumReferenceMode.mile)
                    lsReferenceModeName = "mile"
                Case Is = GetEnumDescription(pcenumReferenceMode.mm)
                    lsReferenceModeName = "mm"
                Case Is = GetEnumDescription(pcenumReferenceMode.USD)
                    lsReferenceModeName = "USD"
                Case Else
                    lsReferenceModeName = Me.ReferenceMode
            End Select

            Return lsReferenceModeName

        End Function

        Public Sub RemoveComplexReferenceScheme()

            Me.ReferenceModeRoleConstraint = Nothing

            Call Me.Model.MakeDirty()

            RaiseEvent ReferenceModeRoleConstraintChanged(Nothing)

        End Sub



        ''' <summary>
        ''' Sets the CompoundReferenceScheme.RoleConstraint for the EntityType.
        ''' NB Precondition: EntityType has no ReferenceMode (SimpleReferenceScheme), else throws exception.
        ''' </summary>
        ''' <param name="arRoleConstraint">The RoleConstraint that defines the CompoundReferenceScheme for the EntityType</param>
        ''' <remarks></remarks>
        Public Sub SetCompoundReferenceSchemeRoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint)

            Dim lsMessage As String

            Try
                If Me.HasSimpleReferenceMode Then
                    lsMessage = "Tried to set a CompoundReferenceMode for an EntityType that already has a SimpleReferenceScheme."
                    lsMessage &= "This process requires that the SimpleReferenceScheme for an EntityType has been removed before assigning the RoleConstraint for a CompoundReferenceScheme"
                    Throw New Exception(lsMessage)
                Else
                    '-----------------------------------------------------------------------------------------------------------------
                    'CodeSafe: Do double checking to make sure the EntityType is primed for receipt of a RoleConstraint defining the 
                    '  CompoundReferenceScheme for the EntityType.
                    '-----------------------------------------------------------------------------------------------------------------
                    If IsSomething(Me.ReferenceModeFactType) Or IsSomething(Me.ReferenceModeValueType) Then
                        lsMessage = "EntityType has no ReferenceMode (does not have a SimpleReferenceScheme) but either of the following are set for the EntityType:"
                        lsMessage &= vbCrLf & "ReferenceModeFactType and/or ReferenceModeValueType"
                        lsMessage &= vbCrLf & "EntityType.Id: " & Me.Id
                        Throw New Exception(lsMessage)
                    End If

                    Me.ReferenceModeRoleConstraint = arRoleConstraint
                    Me.Model.MakeDirty()
                    RaiseEvent ReferenceModeRoleConstraintChanged(arRoleConstraint)
                End If


            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the Name, Symbol and Id of an EntityType.
        ''' </summary>
        ''' <param name="asNewName"></param>
        ''' <remarks>Preconditions: The uniqueness of the new EntityType.Name amoungst EntityTypes, ValueTypes, FactTypes and RoleConstraints has already been verified.</remarks>
        Public Overridable Sub SetName(ByVal asNewName As String)
            '-----------------------------------------------------------------------------------------------------------------
            'The following explains the logic and philosophy of Richmond.
            '  A EntityType.Id/Name represents the same thing accross all Models in Richmond, otherwise the Richmond 
            '  user would have a different EntityType.Id/Name for the differing Concepts (not excluding that in Richmond
            '  a FactType in one Model can have a wildly different RoleGroup (ModelObject associations) than the same
            '  named FactType in another Model).
            '  So, for example, 'Person' is the same Concept accross all Models.
            '  NB A Concept.Symbol/ConceptType combination is unique to a Model except for ConceptTypes, Fact and Value
            '  where the Symbol of a Fact of one FactType may be a Value of FactData of Facts of another FactType.
            '  NB Concept.Symbols must be unique in a Model amoungst EntityTypes, ValueTypes, FactTypes and RoleConstraints.
            '-----------------------------------------------------------------------------------------------------------------
            '-----------------------------------------------------------------
            'Set the name and Symbol of the EntityType to the new asNewName.
            '  The Id of the EntityType is modified later in this method.
            '-----------------------------------------------------------------
            Try
                _Name = asNewName
                Me.Symbol = asNewName

                '--------------------------------------------------------------------------------------------------------
                'The surrogate key for the EntityType is about to change (to match the name of the EntityType)
                '  so update the ModelDictionary entry for the Concept/Symbol (the nominalistic idenity of the VactType
                '--------------------------------------------------------------------------------------------------------
                If StrComp(Me.Id, asNewName) <> 0 Then
                    '----------------------------------------------------------
                    'The new EntityType.Name does not match the EntityType.Id
                    '----------------------------------------------------------
                    Call Me.SwitchConcept(New FBM.Concept(asNewName))

                    'Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.EntityType)

                    'If Me.Model.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsBySymbol) Then
                    '    Call Me.Model.UpdateDictionarySymbol(Me.Id, asNewName, pcenumConceptType.EntityType)
                    '    Call TableModelDictionary.ModifySymbol(Me.Model, lrDictionaryEntry, asNewName, pcenumConceptType.EntityType)
                    'Else
                    '    '----------------------------------------------------------------------------------------------------------------------
                    '    'CodeSafe: See if can switch to a DictionaryEntry for the new EntityType.Name. Validation as to whether there are 
                    '    '  conflicts with ValueTypes, FactTypes, RoleConstraints, etc should have already been done.
                    '    'NB The DictionaryEntry may have changed because the EntityType had the same Id/Name as a FactData.Data value and that
                    '    '  FactData.Data value was changed. e.g. A 'Storeman' Actor and 'Storeman' EntityType had the same DictionaryEntry.
                    '    '  'Storeman' for Actors is stored in a FactData/Instance for pages such as DFDs, UseCaseDiagrams, etc.
                    '    'Becomes a switch rather than an update.
                    '    'NB If EntityType/FactData.Data scenario, the Concept for the EntityType has already been updated and the below is token.
                    '    '----------------------------------------------------------------------------------------------------------------------
                    '    lrDictionaryEntry = New FBM.DictionaryEntry(Me.Model, asNewName, pcenumConceptType.EntityType)
                    '    If Me.Model.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsBySymbol) Then
                    '        lrDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                    '        Me.Concept = lrDictionaryEntry.Concept
                    '    Else

                    '        Dim lsMessage As String = ""
                    '        lsMessage = "Tried to modify the Name of an EntityType where no Dictionary Entry exists for that EntityType."
                    '        lsMessage &= vbCrLf & "Original DictionaryEntry.Symbol: " & Me.Id.ToString
                    '        lsMessage &= vbCrLf & "New DictionaryEntry.Symbol: " & asNewName
                    '        Throw New System.Exception(lsMessage)
                    '    End If
                    'End If
                    Me.Id = asNewName

                    Me.Model.MakeDirty()

                End If

                RaiseEvent updated(Me.ConceptType)

                '------------------------------------------------------------------------------------
                'Must save the Model because Roles that reference the EntityType must be saved.
                '  NB If Roles are saved, the FactType must be saved. If the FactType is saved,
                '  the RoleGroup's references (per Role) must be saved. A Role within the RoleGroup 
                '  may reference another FactType, so that FactType must be saved...etc.
                '  i.e. It's easier and safer to simply save the whole model.
                '------------------------------------------------------------------------------------
                Me.Model.Save()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tEntityType.SetName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Sub SetReferenceModeObjects()

            Try
                If Me.PreferredIdentifierRCId = "" Then
                    Me.ReferenceModeFactType = Nothing
                    Me.ReferenceModeRoleConstraint = Nothing
                    Me.ReferenceModeValueType = Nothing
                Else
                    Me.ReferenceModeRoleConstraint = New FBM.RoleConstraint(Me.Model, Me.PreferredIdentifierRCId, True, pcenumRoleConstraintType.InternalUniquenessConstraint)
                    Me.ReferenceModeRoleConstraint = Me.Model.RoleConstraint.Find(AddressOf Me.ReferenceModeRoleConstraint.Equals)
                    If Me.ReferenceModeRoleConstraint.Role.Count = 1 Then
                        Me.ReferenceModeValueType = Me.ReferenceModeRoleConstraint.Role(0).JoinedORMObject
                        Me.ReferenceModeFactType = New FBM.FactType

                        '--------------------------------------------------------------------------------------------------------
                        'CodeSafe: If the ReferenceMode = "" (i.e. isnot set), then set it to at least something.
                        '  The used can update the ReferenceMode on screen, but at least the objects are set for the EntityType
                        '--------------------------------------------------------------------------------------------------------
                        If Trim(Me.ReferenceMode) = "" Then
                            Me.ReferenceMode = Me.ReferenceModeRoleConstraint.Role(0).JoinedORMObject.Id
                        End If

                        Me.ReferenceModeFactType = Me.ReferenceModeRoleConstraint.RoleConstraintRole(0).Role.FactType

                        '------------------------------------------------------------------------
                        'CodeSafe: Set the IsPreferredReferenceMode member of the FactType to True
                        '---------------------------------------------------------------------------
                        Me.ReferenceModeFactType.IsPreferredReferenceMode = True

                    End If
                End If
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: FBM.EntityType.SetReferenceModeObjects"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "Model.Id: " & Me.Model.ModelId
                lsMessage &= vbCrLf & "EntityType.Id: " & Me.Id
                lsMessage &= vbCrLf & "PreferredIdentifierRCId: " & Me.PreferredIdentifierRCId
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Private Sub ConceptUpdated(ByVal aiConceptType As pcenumConceptType) Handles Me.updated

            Me.Id = Me.Symbol
            Me.Name = Me.Symbol

        End Sub

        Public Shadows Event updated(ByVal aiConceptType As pcenumConceptType)


        Private Sub Concept_Updated() Handles Concept.ConceptSymbolUpdated

            Me.SetName(Me.Concept.Symbol)

        End Sub

        ''' <summary>
        ''' Changes whether or not the EntityType is an Objectifying Entity Type.
        '''   NB Rare, if ever, this method will be used.
        ''' </summary>
        ''' <param name="abIsObjectifyingEntityType"></param>
        ''' <remarks></remarks>
        Public Sub SetIsObjectifyingEntityType(ByVal abIsObjectifyingEntityType As Boolean)
            Me.IsObjectifyingEntityType = abIsObjectifyingEntityType
            RaiseEvent IsObjectifyingEntityTypeChanged(abIsObjectifyingEntityType)

            Me.Model.MakeDirty(False)
        End Sub

        Public Sub SetIsIndependant(ByVal abNewIsIndependant As Boolean)
            Me.IsIndependant = abNewIsIndependant
            RaiseEvent IsIndependantChanged(abNewIsIndependant)

            Me.Model.MakeDirty(False)
        End Sub

        Public Sub SetIsPersonal(ByVal abNewIsPersonal As Boolean)
            Me.IsPersonal = abNewIsPersonal
            RaiseEvent IsPersonalChanged(abNewIsPersonal)

            Me.Model.MakeDirty(False)
        End Sub

        ''' <summary>
        ''' Changed the FactType objectified by the EntityType
        '''   NB Rare, if ever, this method will be used.
        ''' </summary>
        ''' <param name="arNewObjectifiedFactType"></param>
        ''' <remarks></remarks>
        Public Sub SetObjectifiedFactType(ByRef arNewObjectifiedFactType As FBM.FactType)
            Me.ObjectifiedFactType = arNewObjectifiedFactType
            RaiseEvent ObjectifiedFactTypeChanged(arNewObjectifiedFactType)
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