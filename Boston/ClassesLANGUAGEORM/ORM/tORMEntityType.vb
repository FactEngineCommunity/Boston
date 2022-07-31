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
        Implements FBM.iFBMIndependence

        <XmlAttribute()>
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

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsMDAModelElement As Boolean = False
        <XmlAttribute()>
        Public Overrides Property IsMDAModelElement() As Boolean Implements iMDAObject.IsMDAModelElement
            Get
                Return Me._IsMDAModelElement
            End Get
            Set(ByVal value As Boolean)
                Me._IsMDAModelElement = value
            End Set
        End Property

        Private _IsActor As Boolean
        Public Property IsActor As Boolean
            Get
                Return Me._IsActor
            End Get
            Set(value As Boolean)
                Me._IsActor = value
            End Set
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ReferenceMode As String = ""
        <XmlIgnore()>
        Public Shadows Property ReferenceMode As String
            Get
                Return Me._ReferenceMode
            End Get
            Set(value As String)
                Me._ReferenceMode = value
            End Set
        End Property

        <XmlIgnore()>
        Public primitive_type_entity_id As Integer = 0
        <XmlIgnore()>
        Public PrimativeType As String = ""

        <XmlIgnore()>
        Public _ReferenceModeFactType As FBM.FactType = Nothing

        <XmlIgnore()>
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
                        Throw New Exception("Tried to set ReferenceModeFactType for EntityType.Id: " &
                                            Me.Id & ", where there is no ReferenceMode set for the EntityType")
                    End If
                Catch ex As Exception
                    Dim lsMessage As String
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                End Try
            End Set
        End Property


        Public Overrides Property IsObjectified() As Boolean
            Get
                Return False
            End Get
            Set(value As Boolean)
                'Nothing to do here.
            End Set
        End Property


        <XmlIgnore()>
        Public WithEvents ReferenceModeValueType As FBM.ValueType = Nothing

        <XmlIgnore()>
        Public _PreferredIdentifierRCId As String = Nothing
        <XmlIgnore()>
        Public Property PreferredIdentifierRCId() As String
            Get
                Return Me._PreferredIdentifierRCId
            End Get
            Set(ByVal value As String)
                Me._PreferredIdentifierRCId = value
            End Set
        End Property

        <XmlElement()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ReferenceModeRoleConstraint As FBM.RoleConstraint = Nothing
        <XmlIgnore()>
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

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsIndependent As Boolean
        <XmlAttribute()>
        <CategoryAttribute("Model Object"),
        DefaultValueAttribute(False),
        DescriptionAttribute("True if the Model Object is independent.")>
        Public Property IsIndependent As Boolean Implements iFBMIndependence.IsIndependent
            Get
                Return Me._IsIndependent
            End Get
            Set(ByVal value As Boolean)
                Me._IsIndependent = value
            End Set
        End Property

        <XmlIgnore()>
        Public value_constraint As New StringCollection
        <XmlIgnore()>
        <CategoryAttribute("Entity Type"),
         Browsable(False),
         [ReadOnly](False),
         DescriptionAttribute("The List of Values that Objects of this Entity Type may take."),
         Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
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

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _IsPersonal As Boolean = False
        <XmlAttribute()>
        <CategoryAttribute("Entity Type"),
        DefaultValueAttribute(False),
        DescriptionAttribute("True if the Entity Type is personal.")>
        Public Property IsPersonal As Boolean
            Get
                Return Me._IsPersonal
            End Get
            Set(value As Boolean)
                Me._IsPersonal = value
            End Set
        End Property

        '20200422-Removed. Keep removed if all okay.
        '<XmlIgnore()>
        '<DebuggerBrowsable(DebuggerBrowsableState.Never)>
        'Public Shadows _IsAbsorbed As Boolean = False
        <XmlAttribute()>
        <CategoryAttribute("Entity Type"),
        DefaultValueAttribute(False),
        DescriptionAttribute("True if the Entity Type is absorbed by a SuperType.")>
        Public Overloads Property IsAbsorbed As Boolean
            Get
                Return Me._IsAbsorbed
            End Get
            Set(value As Boolean)
                Me._IsAbsorbed = value
            End Set
        End Property

        <XmlIgnore()>
        Public KLLetter As String  'When doing proofs in ORM, is the letter within the formal theory of KL (Knowledge Language) assigned to this EntityType
        <XmlIgnore()>
        Public date_created As Date 'The date that the EntityType was created within Boston.
        <XmlIgnore()>
        Public last_modified As Date 'The date on which the EntityType was last modified.
        <XmlIgnore()>
        Public last_modified_user_id As String 'The Id of the Richmond User who last created/modified the EntityType.


        <XmlAttribute()>
        <CategoryAttribute("Entity Type"),
        DefaultValueAttribute(False),
        DescriptionAttribute("True if the Entity Type is derived.")>
        Public Overrides Property IsDerived As Boolean
            Get
                Return Me._IsDerived
            End Get
            Set(value As Boolean)
                Me._IsDerived = value
            End Set
        End Property

        <XmlAttribute()>
        <CategoryAttribute("Derivation"),
        Browsable(False),
        DescriptionAttribute("The text for the derivation of the Entity Type when the Entity Type is derived."),
        Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Overrides Property DerivationText As String
            Get
                Return Me._DerivationText
            End Get
            Set(value As String)
                Me._DerivationText = value
            End Set
        End Property

        Public Shadows Property ModelError() As System.Collections.Generic.List(Of ModelError) Implements iValidationErrorHandler.ModelError
            Get
                Dim larModelError As New List(Of FBM.ModelError)

                larModelError = Me._ModelError.ToList
                If Me.ReferenceModeValueType IsNot Nothing Then
                    larModelError.AddRange(Me.ReferenceModeValueType.ModelError)
                End If
                Return larModelError
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

        Public Event ChangingToFactType(ByRef arFactType As FBM.FactType)
        Public Event DataTypeChanged(ByVal aiNewDataType As pcenumORMDataType)
        Public Event DataTypePrecisionChanged(ByVal aiNewDataTypePrecision As Integer)
        Public Event DataTypeLengthChanged(ByVal aiDataTypeLength As Integer)
        Public Event DerivationTextChanged(ByVal asDerivationText As String)
        Public Event ExpandReferenceScheme()
        Public Event IsActorChanged(ByVal abIsActor As Boolean)
        Public Event IsDatabaseReservedWordChanged(ByVal abIsDatabaseReservedWord As Boolean)
        Public Event IsDerivedChanged(ByVal abIsDerived As Boolean)
        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded
        'Public Event NameChanged(ByVal asNewName As String)
        Public Shadows Event SubtypeRelationshipAdded(ByRef arSubtypeConstraint As FBM.tSubtypeRelationship, ByVal abBroadcastInterfaceEvent As Boolean)
        Public Event SubtypeConstraintRemoved(ByRef arSubtypeConstraint As FBM.tSubtypeRelationship)
        Public Event ReferenceModeChanged(ByVal asNewReferenceMode As String, ByVal abSimpleAssignment As Boolean, ByVal abBroadcastInterfaceEvent As Boolean)
        Public Event ReferenceModeFactTypeChanged(ByRef arNewReferenceModeFactType As FBM.FactType)
        Public Event ReferenceModeValueTypeChanged(ByRef arNewReferenceModeValueType As FBM.ValueType)
        Public Event RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean)
        Public Event PreferredIdentifierRCIdChanged(ByVal asNewPreferredIndentifierRCId As String)
        Public Event ReferenceModeRoleConstraintChanged(ByRef arNewReferenceModeRoleConstraint As FBM.RoleConstraint)
        Public Event IsObjectifyingEntityTypeChanged(ByVal abNewIsObjectifyingEntityType As Boolean)
        Public Event IsIndependentChanged(ByVal abNewIsIndependent As Boolean) Implements iFBMIndependence.IsIndependentChanged
        Public Event IsPersonalChanged(ByVal abNewIsPersonal As Boolean)
        Public Event IsAbsorbedChanged(ByVal abNewIsAbsorbed As Boolean)
        Public Event ObjectifiedFactTypeChanged(ByRef arNewObjectifiedFactType As FBM.FactType)
        Public Event SimpleReferenceSchemeRemoved()

        ''' <summary>
        ''' Parameterless Constructor.
        ''' </summary>
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

        Public Overrides Function EqualsBySignature(ByVal other As FBM.ModelObject) As Boolean

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

                Return aoA.SubtypeRelationship.Count - aoB.SubtypeRelationship.Count

            Catch ex As Exception
                prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Overloads Overrides Function Clone(ByRef arModel As FBM.Model,
                                            Optional ByVal abAddToModel As Boolean = False,
                                            Optional ByVal abIsMDAModelElement As Boolean = False) As Object

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
                    lrEntityType.IsIndependent = .IsIndependent
                    lrEntityType.IsPersonal = .IsPersonal
                    lrEntityType.isDirty = True

                    If abIsMDAModelElement = False Then
                        lrEntityType.IsMDAModelElement = .IsMDAModelElement
                    Else
                        lrEntityType.IsMDAModelElement = abIsMDAModelElement
                    End If

                    lrEntityType.IsObjectifyingEntityType = .IsObjectifyingEntityType

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

        Public Overloads Sub AddDataInstance(ByVal asDataInstance As String)

            Me.Instance.AddUnique(asDataInstance)

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

                Dim lsIndexPart As String
                lsReturnString = "{"
                For Each lsIndexPart In lasReturnString
                    lsReturnString &= lsIndexPart
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
        Public Function ExistsRolesAssociatedWithEntityType(Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = False) As Boolean

            ExistsRolesAssociatedWithEntityType = False

            Dim larRoles = From FactType In Me.Model.FactType
                           From Role In FactType.RoleGroup
                           Where FactType.IsSubtypeRelationshipFactType = abIncludeSubtypeRelationshipFactTypes
                           Where Role.TypeOfJoin = pcenumRoleJoinType.EntityType
                           Where Role.JoinsEntityType Is Me
                           Select Role


            Dim lrRole As New FBM.Role

            For Each lrRole In larRoles
                Return True
            Next

        End Function

        Public Sub getCompoundReferenceSchemeColumns(ByRef arTable As RDS.Table,
                                                     ByRef arResponsibleRole As FBM.Role,
                                                     ByRef aarColumn As List(Of RDS.Column))

            Try
                If Not Me.HasCompoundReferenceMode Then
                    Throw New Exception("Entity Type does not have a Compound Reference Scheme")
                End If

                Dim lrActiveRole As FBM.Role = Nothing
                Dim lsColumnName As String = ""
                Dim larRole As List(Of FBM.Role)
                Dim lrFactTypeReading As FBM.FactTypeReading

                Dim larIndex = From Table In Me.Model.RDS.Table
                               From Index In Table.Index
                               Where Index.ResponsibleRoleConstraint Is Me.ReferenceModeRoleConstraint
                               Select Index

                If larIndex.Count > 0 Then
                    Dim lrIndex = larIndex.First

                    For Each lrColumn In lrIndex.Column
                        lsColumnName = arTable.createUniqueColumnName(lrColumn.Name, Nothing, 0)
                        aarColumn.Add(New RDS.Column(arTable, lsColumnName, arResponsibleRole, lrColumn.ActiveRole))
                    Next
                    Exit Sub
                End If

                For Each lrRoleConstraintRole In Me.ReferenceModeRoleConstraint.RoleConstraintRole

                    Select Case lrRoleConstraintRole.Role.JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.ValueType

                            lrActiveRole = lrRoleConstraintRole.Role

                            lsColumnName = lrRoleConstraintRole.Role.JoinedORMObject.Name

                            larRole = New List(Of FBM.Role)
                            larRole.Add(lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))
                            larRole.Add(lrRoleConstraintRole.Role)
                            lrFactTypeReading = lrRoleConstraintRole.Role.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                            If lrFactTypeReading IsNot Nothing Then
                                lsColumnName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsColumnName
                            End If

                            lsColumnName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsColumnName))
                            lsColumnName = arTable.createUniqueColumnName(lsColumnName, Nothing, 0)

                            aarColumn.Add(New RDS.Column(arTable, lsColumnName, arResponsibleRole, lrActiveRole))

                        Case Is = pcenumConceptType.EntityType

                            Dim lrEntityType As FBM.EntityType = lrRoleConstraintRole.Role.JoinedORMObject

                            If lrEntityType.HasSimpleReferenceScheme Then

                                '--------------------------------------------------------------------------------------------------------
                                'Trap potential errors
                                If lrEntityType.ReferenceModeFactType Is Nothing Then
                                    If lrEntityType.IsSubtype Then
                                        Dim lrModelObject As FBM.ModelObject = lrEntityType.GetTopmostSupertype
                                        If lrModelObject.ConceptType = pcenumConceptType.EntityType Then
                                            Dim lrTopmostSupertypeEntityType As FBM.EntityType = lrModelObject

                                            lrActiveRole = lrTopmostSupertypeEntityType.ReferenceModeFactType.RoleGroup(1) 'lrRoleConstraintRole.Role

                                            lsColumnName = lrTopmostSupertypeEntityType.ReferenceModeValueType.Id
                                            lsColumnName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsColumnName))
                                            lsColumnName = arTable.createUniqueColumnName(lsColumnName, Nothing, 0)
                                        Else 'Is am ObjectifiedFactType
                                            Throw New NotImplementedException("Called EntityType.getCompoundReferenceSchemeColumns for an EntityType that has a Topmost Supertype that is an Objectified Fact Type. This is not implemented.")
                                        End If
                                    End If

                                Else
                                    If lrEntityType.ReferenceModeFactType.RoleGroup.Count = 0 Then
                                        Throw New Exception("Entity Type, " & lrEntityType.Id & " has a ReferenceModeFactType with no Roles.")
                                    End If

                                    lrActiveRole = lrEntityType.ReferenceModeFactType.RoleGroup(1) 'lrRoleConstraintRole.Role

                                    lsColumnName = lrEntityType.ReferenceModeValueType.Id
                                    lsColumnName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsColumnName))
                                    lsColumnName = arTable.createUniqueColumnName(lsColumnName, Nothing, 0)

                                End If

                                aarColumn.Add(New RDS.Column(arTable, lsColumnName, arResponsibleRole, lrActiveRole))

                            ElseIf lrEntityType.HasCompoundReferenceMode Then
                                Call lrEntityType.getCompoundReferenceSchemeColumns(arTable, arResponsibleRole, aarColumn)
                            Else
                                lrActiveRole = lrRoleConstraintRole.Role

                                lsColumnName = lrEntityType.Name & " requires a Reference Scheme"

                                arTable.addColumn(New RDS.Column(arTable, lsColumnName, arResponsibleRole, lrActiveRole))
                            End If
                        Case Is = pcenumConceptType.FactType
                            aarColumn.AddRange(lrRoleConstraintRole.Role.getColumns(arTable, arResponsibleRole))

                    End Select

                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Function getCorrespondingCMMLActor(Optional ByVal abCreateActorIfNotExists As Boolean = False) As CMML.Actor

            Try
                Dim lsSQLQuery As String
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CoreElementHasElementType"
                lsSQLQuery &= " WHERE Element = '" & Me.Id & "'"
                lsSQLQuery &= " AND ElementType = 'Actor'"

                Dim lrRecordset As ORMQL.Recordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If Not lrRecordset.EOF Then
                    'CodeSafe: Search for the Actor rather than having a CMMLActor member in case not linked.
                    '20220619-VM-May need to have CMMLActor member in the future.
                    Dim lrCMMLActor As CMML.Actor = Me.Model.UML.Actor.Find(Function(x) x.Name = Me.Id)
                    If lrCMMLActor IsNot Nothing Then
                        Return lrCMMLActor
                    Else
                        If abCreateActorIfNotExists Then
                            lrCMMLActor = New CMML.Actor(Me.Model.UML, Me.Id, Me)
                            Me.Model.UML.addActor(lrCMMLActor)
                            Return lrCMMLActor
                        Else
                            Return Nothing
                        End If
                    End If
                Else
                    Return Nothing
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

        Public Function getAssociatedCMMLProcesses() As List(Of CMML.Process)

            Try
                Dim larCMMLProcess As New List(Of CMML.Process)

                If Me.IsActor Then

                    Dim larAssociatedProcess = From ActorProcessRelation In Me.Model.UML.ActorProcessRelation
                                               Where ActorProcessRelation.Actor.Name = Me.Id
                                               Select ActorProcessRelation.Process

                    Return larAssociatedProcess.ToList

                Else
                    Return larCMMLProcess
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        ''' <summary>
        ''' PRECONDITION: FactType must have a corresponding RDS Table. Used to save typing.
        ''' </summary>
        ''' <returns></returns>
        Public Shadows Function getCorrespondingRDSTable(Optional abCreateTableIfNotExists As Boolean = False) As RDS.Table

            Try
                Dim lrTable As RDS.Table

                If Me.IsObjectifyingEntityType Then
                    lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = Me.ObjectifiedFactType.Id)
                Else
                    lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = Me.Id)
                End If

                If lrTable Is Nothing And abCreateTableIfNotExists Then
                    lrTable = New RDS.Table(Me.Model.RDS, Me.Id, Me)
                    Me.Model.RDS.addTable(lrTable)
                    Return lrTable
                End If

                If lrTable Is Nothing Then
                    'Try to get TopmostNonAbsorbedSuperType
                    lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = Me.GetTopmostNonAbsorbedSupertype.Id)

                    Return lrTable
                Else
                    Return lrTable
                End If

                Return Nothing

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function
        Public Function GetCountRolesAssociatedWithEntityType() As Integer


            Dim larRoles = From FactType In Me.Model.FactType
                           From Role In FactType.RoleGroup
                           Where Role.TypeOfJoin = pcenumRoleJoinType.EntityType _
                            And Role.JoinsEntityType Is Me
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

            Dim larSubTypes = From SubType In Me.Model.EntityType
                              From EntityType In SubType.parentModelObjectList
                              Where EntityType.Id = Me.Id
                              Select EntityType

            Dim lrEntityType As New FBM.EntityType

            For Each lrEntityType In larSubTypes
                Return True
            Next

        End Function

        '20220713VM-Commented out because clashed with other CloneInstance by parameters when having introduced IngnoreExistingInstance
        '''' <summary>
        '''' See also CloneInstance override.
        '''' </summary>
        '''' <param name="arPage"></param>
        '''' <param name="abAddToPage"></param>
        '''' <param name="abIgnoreSubtypeRelationships"></param>
        '''' <returns></returns>
        'Public Overloads Function CloneInstance(ByRef arPage As FBM.Page,
        '                                        ByVal abIgnoreSubtypeRelationships As Boolean,
        '                                        Optional ByVal abAddToPage As Boolean = False) As FBM.ModelObject

        '    Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

        '    Try

        '        With Me
        '            lrEntityTypeInstance.Model = arPage.Model
        '            lrEntityTypeInstance.Page = arPage
        '            lrEntityTypeInstance.Id = .Id
        '            lrEntityTypeInstance.Name = .Name
        '            lrEntityTypeInstance.Symbol = .Symbol
        '            lrEntityTypeInstance.GUID = .GUID
        '            lrEntityTypeInstance.ConceptType = .ConceptType
        '            lrEntityTypeInstance.EntityType = Me
        '            lrEntityTypeInstance.ShortDescription = .ShortDescription
        '            lrEntityTypeInstance.LongDescription = .LongDescription
        '            lrEntityTypeInstance.PrimativeType = .PrimativeType
        '            lrEntityTypeInstance.primitive_type_entity_id = .primitive_type_entity_id
        '            lrEntityTypeInstance.ReferenceMode = .ReferenceMode
        '            lrEntityTypeInstance.IsMDAModelElement = .IsMDAModelElement
        '            lrEntityTypeInstance.IsIndependent = .IsIndependent
        '            lrEntityTypeInstance.IsPersonal = .IsPersonal
        '            lrEntityTypeInstance.IsAbsorbed = .IsAbsorbed
        '            lrEntityTypeInstance.DerivationText = .DerivationText
        '            lrEntityTypeInstance.DBName = .DBName

        '            'CMML
        '            lrEntityTypeInstance.IsActor = .IsActor

        '            lrEntityTypeInstance.IsObjectifyingEntityType = .IsObjectifyingEntityType

        '            If lrEntityTypeInstance.IsObjectifyingEntityType Then
        '                If IsSomething(.ObjectifiedFactType) Then
        '                    lrEntityTypeInstance.ObjectifiedFactType = New FBM.FactTypeInstance
        '                    Dim lrFactTypeInstance As FBM.FactTypeInstance
        '                    lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(x) x.Id = .ObjectifiedFactType.Id)
        '                    lrEntityTypeInstance.ObjectifiedFactType = lrFactTypeInstance
        '                End If
        '            End If

        '            If abAddToPage Then
        '                If Not arPage.EntityTypeInstance.Exists(AddressOf lrEntityTypeInstance.Equals) Then
        '                    arPage.EntityTypeInstance.Add(lrEntityTypeInstance)
        '                End If
        '            End If

        '            If IsSomething(.ReferenceModeValueType) Then
        '                lrEntityTypeInstance.ReferenceModeValueType = .ReferenceModeValueType.CloneInstance(arPage, abAddToPage)
        '            End If
        '            If IsSomething(.ReferenceModeFactType) Then
        '                lrEntityTypeInstance.ReferenceModeFactType = .ReferenceModeFactType.CloneInstance(arPage, abAddToPage)
        '            End If

        '            If IsSomething(.ReferenceModeRoleConstraint) Then
        '                lrEntityTypeInstance.ReferenceModeRoleConstraint = arPage.RoleConstraintInstance.Find(Function(x) x.Id = .ReferenceModeRoleConstraint.Id)
        '            End If

        '        End With

        '        If Not abIgnoreSubtypeRelationships Then
        '            For Each lrSubtypeRelationship In Me.SubtypeRelationship
        '                Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance = lrSubtypeRelationship.CloneInstance(arPage, False)
        '                lrSubtypeRelationshipInstance.ModelElement = lrEntityTypeInstance
        '                lrEntityTypeInstance.SubtypeRelationship.Add(lrSubtypeRelationshipInstance)
        '            Next
        '        End If

        '        'For Each lrParentEntityType In Me.parentModelObjectList
        '        '    '----------------------------------------------------------------------
        '        '    'The ParentEntityType is at least part of the model under review
        '        '    '  i.e. If currently looking at an ORM model...is within the ORM model
        '        '    '----------------------------------------------------------------------
        '        '    lsId = Trim(lrParentEntityType.Id)
        '        '    lr_parentEntityTypeInstance = arPage.EntityTypeInstance.Find(Function(x) x.Id = Trim(lrParentEntityType.Id))
        '        '    Dim lr_sub_type_constraint As New FBM.SubtypeRelationshipInstance(arPage, lrEntityTypeInstance, lr_parentEntityTypeInstance)
        '        '    lrEntityTypeInstance.SubtypeRelationship.Add(lr_sub_type_constraint)
        '        'Next

        '    Catch ex As Exception
        '        Dim lsMessage As String
        '        lsMessage = "Error: tORMEntityType.CloneInstance:"
        '        lsMessage &= vbCrLf & vbCrLf & ex.Message
        '        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        '    End Try

        '    Return lrEntityTypeInstance

        'End Function

        Public Overloads Function CloneInstance(ByRef arPage As FBM.Page,
                                                Optional ByVal abAddToPage As Boolean = False,
                                                Optional ByVal abIgnoreExistingInstance As Boolean = False,
                                                Optional ByVal abIgnoreSubtypeRelationships As Boolean = False) As FBM.ModelObject

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

            Try
                With Me
                    lrEntityTypeInstance.Model = arPage.Model
                    lrEntityTypeInstance.Page = arPage
                    lrEntityTypeInstance.Id = .Id
                    lrEntityTypeInstance.Name = .Name
                    lrEntityTypeInstance.Symbol = .Symbol
                    lrEntityTypeInstance.GUID = .GUID
                    lrEntityTypeInstance.ConceptType = .ConceptType
                    lrEntityTypeInstance.EntityType = Me
                    lrEntityTypeInstance.ShortDescription = .ShortDescription
                    lrEntityTypeInstance.LongDescription = .LongDescription
                    lrEntityTypeInstance.PrimativeType = .PrimativeType
                    lrEntityTypeInstance.primitive_type_entity_id = .primitive_type_entity_id
                    lrEntityTypeInstance.ReferenceMode = .ReferenceMode
                    lrEntityTypeInstance.IsMDAModelElement = .IsMDAModelElement
                    lrEntityTypeInstance.IsIndependent = .IsIndependent
                    lrEntityTypeInstance.IsPersonal = .IsPersonal
                    lrEntityTypeInstance.IsAbsorbed = .IsAbsorbed
                    lrEntityTypeInstance.DerivationText = .DerivationText
                    lrEntityTypeInstance.DBName = .DBName

                    'CMML
                    lrEntityTypeInstance.IsActor = .IsActor

                    lrEntityTypeInstance.IsObjectifyingEntityType = .IsObjectifyingEntityType

                    If lrEntityTypeInstance.IsObjectifyingEntityType Then
                        If IsSomething(.ObjectifiedFactType) Then
                            lrEntityTypeInstance.ObjectifiedFactType = New FBM.FactTypeInstance
                            Dim lrFactTypeInstance As FBM.FactTypeInstance
                            lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(x) x.Id = .ObjectifiedFactType.Id)
                            lrEntityTypeInstance.ObjectifiedFactType = lrFactTypeInstance
                        End If
                    End If

                    If abAddToPage Then
                        arPage.EntityTypeInstance.AddUnique(lrEntityTypeInstance)
                    End If

                    If IsSomething(.ReferenceModeValueType) Then
                        lrEntityTypeInstance.ReferenceModeValueType = .ReferenceModeValueType.CloneInstance(arPage, abAddToPage)
                    End If

                    If IsSomething(.ReferenceModeFactType) Then
                        lrEntityTypeInstance.ReferenceModeFactType = .ReferenceModeFactType.CloneInstance(arPage, abAddToPage)
                    End If

                    If IsSomething(.ReferenceModeRoleConstraint) Then
                        lrEntityTypeInstance.ReferenceModeRoleConstraint = arPage.RoleConstraintInstance.Find(Function(x) x.Id = .ReferenceModeRoleConstraint.Id)
                    End If

                End With

                If Not abIgnoreSubtypeRelationships Then
                    For Each lrSubtypeRelationship In Me.SubtypeRelationship
                        Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance = lrSubtypeRelationship.CloneInstance(arPage, False)
                        lrSubtypeRelationshipInstance.ModelElement = lrEntityTypeInstance
                        lrEntityTypeInstance.SubtypeRelationship.Add(lrSubtypeRelationshipInstance)
                    Next
                End If

                '20220713-VM-Remove if not missed.
                'For Each lrParentEntityType In Me.parentModelObjectList
                '    '----------------------------------------------------------------------
                '    'The ParentEntityType is at least part of the model under review
                '    '  i.e. If currently looking at an ORM model...is within the ORM model
                '    '----------------------------------------------------------------------
                '    lsId = Trim(lrParentEntityType.Id)
                '    lr_parentEntityTypeInstance = arPage.EntityTypeInstance.Find(Function(x) x.Id = Trim(lrParentEntityType.Id))
                '    Dim lr_sub_type_constraint As New FBM.SubtypeRelationshipInstance(arPage, lrEntityTypeInstance, lr_parentEntityTypeInstance)
                '    lrEntityTypeInstance.SubtypeRelationship.Add(lr_sub_type_constraint)
                'Next

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tORMEntityType.CloneInstance:"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrEntityTypeInstance

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean

            Return False
        End Function

        ''' <summary>
        ''' Changes the Model of the EntityType to the target Model.
        ''' </summary>
        ''' <param name="arTargetModel">The Model to which the EntityType will be associated on completion of this method.</param>
        ''' <remarks></remarks>
        Public Shadows Function ChangeModel(ByRef arTargetModel As FBM.Model,
                                            ByVal abAddToModel As Boolean,
                                            Optional ByVal abReturnExistingModelElementIfExists As Boolean = False) As FBM.ModelObject

            Try

                If abReturnExistingModelElementIfExists And arTargetModel.ModelId = Me.Model.OriginModelId Then
                    Dim lrEntityType As FBM.EntityType = arTargetModel.EntityType.Find(Function(x) x.Id = Me.Id)
                    If lrEntityType IsNot Nothing Then
                        Return lrEntityType
                    End If
                End If

                Me.Model = arTargetModel

                If abAddToModel Then
                    arTargetModel.AddEntityType(Me)
                End If

                If Me.ReferenceModeValueType IsNot Nothing Then
                    Me.ReferenceModeValueType.ChangeModel(arTargetModel, abAddToModel)
                End If

                If Me.ReferenceModeFactType IsNot Nothing Then
                    Me.ReferenceModeFactType.ChangeModel(arTargetModel, abAddToModel)
                End If

                For Each lrSubtypeRelationship In Me.SubtypeRelationship
                    lrSubtypeRelationship.Model = arTargetModel
                Next

                '20220319-VM-Was commented out. If not missed, remove.
                'If Me.ReferenceModeRoleConstraint IsNot Nothing Then
                '            Me.ReferenceModeRoleConstraint.ChangeModel(arTargetModel)
                'End If

                Return Me

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Me
            End Try

        End Function

        ''' <summary>
        ''' Converts an appropriate EntityType to a FactType
        ''' </summary>
        Public Sub ConvertToFactType()

            Try
                If Not Me.HasPrimaryReferenceScheme Then
                    MsgBox("This Entity Type cannot be converted to a Fact Type. Create a Primary Reference Scheme for the Entity Type.")
                    Exit Sub
                End If

                Dim larModelElement As New List(Of FBM.ModelObject)

                For Each lrRole In Me.ReferenceModeRoleConstraint.Role
                    larModelElement.Add(lrRole.JoinedORMObject)
                Next

                Dim lbHasSimpleReferenceScheme As Boolean = Me.HasSimpleReferenceScheme

                If lbHasSimpleReferenceScheme Then
                    Call Me.TriggerExpandReferenceScheme()
                End If

                Dim lrFactType = Me.Model.CreateFactType(Me.Id, larModelElement, False, True, False, Nothing, True, Nothing, False)

                Call lrFactType.CreateInternalUniquenessConstraint(lrFactType.RoleGroup, True, True, True, False, Nothing, True, True)

                Call Me.TriggerChangingToFactType(lrFactType)

                lrFactType.Objectify(True, False, Me)

                If lbHasSimpleReferenceScheme Then
                    Call lrFactType.RoleGroup(0).ReassignJoinedModelObject(lrFactType, True, Nothing, True)
                    Me.ReferenceModeFactType.SetIsLinkFactType(True)
                    Me.ReferenceModeFactType.LinkFactTypeRole = lrFactType.RoleGroup(0)
                    Me.ReferenceModeFactType.makeDirty()
                Else
                    Dim larLinkFactType As New List(Of FBM.FactType)

                    For Each lrPRSRoleConstraintRole In Me.ReferenceModeRoleConstraint.Role
                        Dim lrOtherRole = lrPRSRoleConstraintRole.FactType.GetOtherRoleOfBinaryFactType(lrPRSRoleConstraintRole.Id)
                        Call lrOtherRole.ReassignJoinedModelObject(lrFactType, True, Nothing, True)
                        lrPRSRoleConstraintRole.FactType.SetIsLinkFactType(True)
                        larLinkFactType.AddUnique(lrPRSRoleConstraintRole.FactType)
                        lrPRSRoleConstraintRole.FactType.makeDirty()
                    Next

                    For Each lrRole In lrFactType.RoleGroup
                        larLinkFactType = larLinkFactType.ToList

                        Dim lrLinkFactType = (From LinkFactType In larLinkFactType
                                              From LFTRole In LinkFactType.RoleGroup
                                              Where LinkFactType.LinkFactTypeRole Is Nothing
                                              Where LFTRole.JoinedORMObject.Id <> lrFactType.Id
                                              Select New With {Key .FactType = LinkFactType, Key .Role = LFTRole}).First


                        lrLinkFactType.FactType.SetLinkFactTypeRole(lrRole)
                        lrLinkFactType.FactType.makeDirty()
                    Next

                    '===================================================
                    'RDS
                    Dim larProcessedColumn As New List(Of RDS.Column)
                    For Each lrRole In lrFactType.RoleGroup.ToArray

                        Dim lrLinkFactType = (From FactType In larLinkFactType
                                              Where FactType.LinkFactTypeRole.Id = lrRole.Id
                                              Select FactType).First

                        Dim lrLFKRole = (From Role In lrLinkFactType.RoleGroup
                                         Where Role.JoinedORMObject.Id <> lrFactType.Id
                                         Select Role).First

                        Dim lrModelElement = lrLFKRole.JoinedORMObject

                        Dim larColumn = From Column In lrFactType.getCorrespondingRDSTable.Column
                                        Where Column.Role.FactType.Id = lrLinkFactType.Id
                                        Where Not larProcessedColumn.Contains(Column)
                                        Select Column

                        Dim lrColumn = larColumn.First
                        lrColumn.setRole(lrRole)

                        If lrRole.JoinedORMObject.GetType = GetType(FBM.ValueType) Then
                            lrColumn.setActiveRole(lrRole)
                        End If

                        lrColumn.FactType = lrFactType
                        '20220523-vm-was but wrong.
                        'lrColumn.setActiveRole(lrRole)
                        larProcessedColumn.Add(lrColumn)
                    Next
                End If

                Dim larRole = From FactType In Me.Model.FactType
                              From Role In FactType.RoleGroup
                              Where Role.JoinedORMObject IsNot Nothing
                              Where Role.JoinedORMObject.Id = Me.Id
                              Where Role.TypeOfJoin = pcenumRoleJoinType.EntityType
                              Select Role

                For Each lrRole In larRole
                    Call lrRole.ReassignJoinedModelObject(lrFactType, True, Nothing, True)
                Next

                Call Me.ReferenceModeRoleConstraint.RemoveFromModel(True, True, True, True, False)



#Region "FactTypeReading"
                Dim lsSentence As String = ""
                For Each lrRole In lrFactType.RoleGroup
                    lsSentence &= lrRole.JoinedORMObject.Id
                    If Not lrRole Is lrFactType.RoleGroup(lrFactType.RoleGroup.Count - 1) Then
                        lsSentence &= " has "
                    End If
                Next

                Dim lrSentence As New Language.Sentence(lsSentence)
                For Each lrRole In lrFactType.RoleGroup
                    If Not lrRole Is lrFactType.RoleGroup(lrFactType.RoleGroup.Count - 1) Then
                        lsSentence &= " has "
                        lrSentence.PredicatePart.Add(New Language.PredicatePart("has"))
                    Else
                        lrSentence.PredicatePart.Add(New Language.PredicatePart(""))
                    End If

                Next

                Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, lrFactType.RoleGroup, lrSentence)
                Call lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)
#End Region

                Call Me.TriggerChangedToFactType(lrFactType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

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
        Public Sub CreateReferenceMode(ByVal asReferenceMode As String,
                                       Optional ByVal asValueTypeName As String = Nothing,
                                       Optional ByVal aiORMDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet,
                                       Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

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

                    If Not lrValueType.IsIndependent And abBroadcastInterfaceEvent Then
                        Dim laoRole = From Role In Me.Model.Role
                                      Where Role.JoinedORMObject Is lrValueType
                                      Select Role

                        Dim lrRole As FBM.Role

                        For Each lrRole In laoRole
                            lrRole.ReassignJoinedModelObject(Me, abBroadcastInterfaceEvent)
                        Next
                    End If
                Else
                    Me.ReferenceMode = asReferenceMode
                    '------------------------------------------------
                    'Add the ValueType to the Model/ModelDictionary
                    '------------------------------------------------
                    lrValueType = New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lsValueTypeName, True, aiORMDataType)
                    Me.Model.AddValueType(lrValueType, True, abBroadcastInterfaceEvent,, True)
                End If

                '------------------------------------------
                'Create and add the FactType to the Model
                '------------------------------------------
                Dim lrFactType As FBM.FactType
                Dim lsFactTypeName As String = Me.Name & "Has" & lrValueType.Name

                If abBroadcastInterfaceEvent = True Then
                    '----------------------------------------------------------------------------------------------------------
                    'NB If abBroadcastInterfaceEvent is False, then there is no need to get a new/unique FactTypeName
                    '  because the FactType from the Broadcasting instance of Boston has already sent the new Fact Type
                    '  to 'this' effective instance of Boston...so the FactType already exists in the Model (both on the sending
                    '  and receiving instance of Boston). The next statement (GetConceptTypeByNameFuzzy) will get that FactType.
                    '
                    lsFactTypeName = Me.Model.CreateUniqueFactTypeName(lsFactTypeName, 0, False)
                End If

                If Me.Model.GetConceptTypeByNameFuzzy(lsFactTypeName, lsActualModelElementName) = pcenumConceptType.FactType Then
                    '--------------------------------------------------------------------------------------------------------
                    'This especially when in Broadcast/Client-Server Model, because the FactType will have already been created 
                    '  by the time we get to this point...so just get the FactType.
                    lrFactType = Me.Model.GetModelObjectByName(lsActualModelElementName)
                Else
                    Dim larModelObjectList As New List(Of FBM.ModelObject)

                    larModelObjectList.Add(Me)
                    larModelObjectList.Add(lrValueType)

                    lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObjectList, True, False, False, Nothing, False)
                    lrFactType.IsPreferredReferenceMode = True

                    lrFactType.FindFirstRoleByModelObject(Me).Mandatory = True

                    Call Me.Model.AddFactType(lrFactType, False, abBroadcastInterfaceEvent)

                End If

                '-----------------------------------------------------------
                'Create the FactTypeReadings for the ReferenceModeFactType
                '-----------------------------------------------------------
                Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType)
                '------------------------------------------------------------------------------
                'No Longer supported (v1.13 of the database Model).
                'lrFactTypeReading.ObjectTypeList = New List(Of FBM.ModelObject)
                'lrFactTypeReading.ObjectTypeList.Add(Me)
                'lrFactTypeReading.ObjectTypeList.Add(lrValueType)
                Dim lrPredicatePart As New FBM.PredicatePart(Me.Model, lrFactTypeReading, Nothing, True)
                lrPredicatePart.SequenceNr = 1
                lrPredicatePart.Role = lrFactType.GetRoleByJoinedObjectTypeId(Me.Id)
                lrPredicatePart.PredicatePartText = "has"
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)

                lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading, Nothing, True)
                lrPredicatePart.SequenceNr = 2
                lrPredicatePart.Role = lrFactType.GetRoleByJoinedObjectTypeId(lrValueType.Id)
                lrPredicatePart.PredicatePartText = ""
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)

                '--------------------------------------------
                'Add the Fact Type Reading to the Fact Type
                '--------------------------------------------
                lrFactType.AddFactTypeReading(lrFactTypeReading, False, abBroadcastInterfaceEvent)

                lrFactTypeReading = New FBM.FactTypeReading(lrFactType)
                lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading, Nothing, True)
                lrPredicatePart.SequenceNr = 1
                lrPredicatePart.Role = lrFactType.GetRoleByJoinedObjectTypeId(lrValueType.Id)
                lrPredicatePart.PredicatePartText = "is of"
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)

                lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading, Nothing, True)
                lrPredicatePart.SequenceNr = 2
                lrPredicatePart.Role = lrFactType.GetRoleByJoinedObjectTypeId(Me.Id)
                lrPredicatePart.PredicatePartText = ""
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)

                '--------------------------------------------
                'Add the Fact Type Reading to the Fact Type
                '--------------------------------------------
                lrFactType.AddFactTypeReading(lrFactTypeReading, False, abBroadcastInterfaceEvent)
                'End If

                Me.ReferenceModeValueType = lrValueType
                Me.ReferenceModeFactType = lrFactType

                '-------------------------------------------------------------------------------------------------------------
                'Create the UniquenessConstraints for the ReferenceModeFactType.
                '  Do this after the ReferenceModeValueType and ReferenceModeFactType have been added to the EntityType,
                '    otherwise adding the Role will try and create an RDS Column (adding a Fact to CoreEntityIsOfEntityType)
                '    and in doing so, forcing a check for errors on the Model, and where the EntityType doesn't yet
                '    have a ReferenceModeValueType and ReferenceModeFactType.
                '----------------------------------------------------------------
                Dim lrRoleConstraint As FBM.RoleConstraint
                Dim larRole As New List(Of FBM.Role)
                larRole.Add(lrFactType.FindFirstRoleByModelObject(Me))
                lrRoleConstraint = lrFactType.CreateInternalUniquenessConstraint(larRole, False, False, False)
                Call Me.Model.AddRoleConstraint(lrRoleConstraint, False, abBroadcastInterfaceEvent)

                larRole.Clear()
                larRole.Add(lrFactType.FindFirstRoleByModelObject(lrValueType))
                lrRoleConstraint = lrFactType.CreateInternalUniquenessConstraint(larRole, True, False, False)
                Call Me.Model.AddRoleConstraint(lrRoleConstraint, False, abBroadcastInterfaceEvent)

                Dim lrExistingReferenceModeRoleConstraint As FBM.RoleConstraint = Me.ReferenceModeRoleConstraint

                Me.ReferenceModeRoleConstraint = lrRoleConstraint 'Leave before setting the PreferredIdentifier flag, so Index created can get ResponsibleRoleConstraint

                Call lrRoleConstraint.SetIsPreferredIdentifier(True, True, lrExistingReferenceModeRoleConstraint) 'Already true, but triggers RDS processing to make respective Column part of the PrimaryKey.



                '====================================================================================
                'RDS
                Dim lsColumnName As String = ""

                For Each lrColumn In Me.Model.RDS.getColumnsThatReferenceValueType(Me.ReferenceModeValueType)

                    If lrColumn.Role.FactType.Id = lrColumn.ActiveRole.FactType.Id Then
                        Call lrColumn.setName(lrColumn.Role.GetAttributeName)
                    Else
                        lsColumnName = Me.ReferenceModeValueType.Id
                        lsColumnName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsColumnName))
                        Call lrColumn.setName(lsColumnName)
                    End If
                Next

                'Reassign the ActiveRole of respective Columns
                Dim larColumn = From Table In Me.Model.RDS.Table
                                From Column In Table.Column
                                Where Column.ActiveRole.JoinedORMObject Is Me
                                Where Not Column.Table.FBMModelElement Is Me
                                Select Column

                For Each lrColumn In larColumn.ToList
                    lrColumn.setActiveRole(Me.ReferenceModeFactType.RoleGroup(1))
                    lrColumn.setName(lrColumn.getAttributeName)
                Next

                'Find RDS.Relations that reference this EntityType, and set their OriginColumns and DestinationColumns

                Dim larRelation = From Relation In Me.Model.RDS.Relation
                                  Where Relation.DestinationTable.Name = Me.Id
                                  Select Relation

                For Each lrRelation In larRelation

                    Dim lrOriginColumn = lrRelation.OriginTable.Column.Find(Function(x) x.ActiveRole Is Me.ReferenceModeFactType.RoleGroup(1))
                    Dim lrDestinationColumn = Me.getCorrespondingRDSTable.Column.Find(Function(x) x.ActiveRole Is Me.ReferenceModeFactType.RoleGroup(1))

                    If lrOriginColumn IsNot Nothing And lrDestinationColumn IsNot Nothing Then
                        lrRelation.OriginColumns.Add(lrOriginColumn)
                        lrRelation.DestinationColumns.Add(lrDestinationColumn)


                        Call Me.Model.addCMMLColumnToRelationOrigin(lrRelation, lrOriginColumn, 1)
                        Call Me.Model.addCMMLColumnToRelationDestination(lrRelation, lrDestinationColumn, 1)
                    Else
                        Dim lrResponsibleRole As FBM.Role = lrRelation.OriginColumns(0).Role

                        Dim larIndex As New List(Of RDS.Index)

                        Dim lbIsMandatory As Boolean = lrRelation.OriginColumns(0).IsMandatory

                        For Each lrColumn In lrRelation.OriginColumns
                            Call Me.Model.removeCMMLRelationOriginColumn(lrRelation, lrColumn)

                            'See if any Index in Relation.OriginTable contains the Column being removed.
                            Dim larExistingIndex = (From Index In lrRelation.OriginTable.Index
                                                    Where Index.Column.Contains(lrColumn)
                                                    Select Index).ToList

                            For Each lrIndex In larExistingIndex
                                larIndex.AddUnique(lrIndex)
                            Next

                            lrRelation.OriginTable.removeColumn(lrColumn)
                        Next
                        lrRelation.OriginColumns.Clear()

                        Dim lrNewColumn = New RDS.Column(lrRelation.OriginTable, Me.ReferenceModeValueType.Id, lrResponsibleRole, Me.ReferenceModeFactType.RoleGroup(1), lbIsMandatory)
                        Call lrRelation.OriginTable.addColumn(lrNewColumn)
                        lrNewColumn.Relation.Add(lrRelation)
                        Call Me.Model.addCMMLColumnToRelationOrigin(lrRelation, lrNewColumn, 1)

                        'Add the new Column to the ExistingIndexes for the OriginTable
                        For Each lrIndex In larIndex
                            Call lrIndex.addColumn(lrNewColumn)
                        Next

                        lrNewColumn = lrRelation.DestinationTable.getPrimaryKeyColumns.First
                        For Each lrColumn In lrRelation.DestinationColumns
                            Call Me.Model.removeCMMLRelationDestinationColumn(lrRelation, lrColumn)
                        Next
                        lrRelation.DestinationColumns.Clear()
                        lrRelation.DestinationColumns.Add(lrNewColumn)
                        Call Me.Model.addCMMLColumnToRelationDestination(lrRelation, lrNewColumn, 1)
                    End If

                Next


                '===================================================================================
                '20180716-VM-Remove if all okay. lrRoleConstraint.SetIsPreferredIndentifier(True) (above) creates the Index.
                'RDS - Index for the PK
                'Dim lrTable As RDS.Table = (From Table In Me.Model.RDS.Table _
                '                            Where Table.Name = Me.Id _
                '                            Select Table).First

                'If (lrTable IsNot Nothing) Then

                '    Call lrTable.removeExistingPrimaryKey()

                '    Dim lrPKColumn = (From Column In lrTable.Column _
                '                     Where Column.Role.Id = lrFactType.FindFirstRoleByModelObject(Me).Id _
                '                     Select Column).First

                '    Dim larColumn As New List(Of RDS.Column)

                '    larColumn.Add(lrPKColumn)

                '    Dim lrPrimaryKeyIndex As New RDS.Index(lrTable, _
                '                                           lrTable.Name & "_PK",
                '                                           "PK",
                '                                           pcenumODBCAscendingOrDescending.Ascending,
                '                                           True,
                '                                           True,
                '                                           False,
                '                                           larColumn,
                '                                           False,
                '                                           True)

                '    Call lrTable.addIndex(lrPrimaryKeyIndex)

                'End If
                '====================================================================================
                Me.isDirty = True
                Me.Model.MakeDirty()

                RaiseEvent PreferredIdentifierRCIdChanged(Me.PreferredIdentifierRCId)

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        ''' <summary>
        ''' Creates a SubtypeRelationship for the Model Element.
        ''' </summary>
        ''' <param name="arParentModelElement"></param>
        ''' <param name="abIsPrimarySubtypeRelationship"></param>
        ''' <param name="asSubtypeRoleId">Used when importing NORMA .orm files.</param>
        ''' <param name="asSupertypeRoleId">Used when importing NORMA .orm files.</param>
        ''' <returns></returns>
        Public Overrides Function CreateSubtypeRelationship(ByVal arParentModelElement As FBM.ModelObject,
                                                            Optional ByVal abIsPrimarySubtypeRelationship As Boolean = False,
                                                            Optional ByVal asSubtypeRoleId As String = Nothing,
                                                            Optional ByVal asSupertypeRoleId As String = Nothing,
                                                            Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                            Optional ByVal arUsingFactType As FBM.FactType = Nothing) As FBM.tSubtypeRelationship

            Try
                Dim lrSubtypeRelationship As New FBM.tSubtypeRelationship

                lrSubtypeRelationship.Model = Me.Model
                lrSubtypeRelationship.ModelElement = Me
                lrSubtypeRelationship.parentModelElement = arParentModelElement
                lrSubtypeRelationship.IsPrimarySubtypeRelationship = abIsPrimarySubtypeRelationship

                Me.parentModelObjectList.Add(arParentModelElement)
                arParentModelElement.childModelObjectList.Add(Me)

                '---------------------------------------------
                'Create a FactType for the SubtypeConstraint
                '---------------------------------------------
#Region "Create the FactType for the SubtypeRelationship"
                If arUsingFactType Is Nothing Then

                    Dim lsFactTypeName As String = ""
                    Dim larModelObject As New List(Of FBM.ModelObject)
                    Dim larRole As New List(Of FBM.Role)

                    larModelObject.Add(Me)
                    If arParentModelElement.IsObjectifyingEntityType Then
                        lsFactTypeName = Viev.Strings.RemoveWhiteSpace(Me.Name & "IsSubtypeOf" & arParentModelElement.ObjectifiedFactType.Id)
                        larModelObject.Add(arParentModelElement.ObjectifiedFactType)
                    Else
                        lsFactTypeName = Viev.Strings.RemoveWhiteSpace(Me.Name & "IsSubtypeOf" & arParentModelElement.Name)
                        larModelObject.Add(arParentModelElement)
                    End If

                    lrSubtypeRelationship.FactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject, False, False,,, False,,, False)
                    lrSubtypeRelationship.FactType.IsSubtypeRelationshipFactType = True

                    lrSubtypeRelationship.FactType.RoleGroup(0).Name = "Subtype"
                    lrSubtypeRelationship.FactType.RoleGroup(1).Name = "Supertype"

                    If asSubtypeRoleId IsNot Nothing Then
                        lrSubtypeRelationship.FactType.RoleGroup(0).Id = asSubtypeRoleId
                    End If

                    If asSupertypeRoleId IsNot Nothing Then
                        lrSubtypeRelationship.FactType.RoleGroup(1).Id = asSupertypeRoleId
                    End If

                    lrSubtypeRelationship.FactType.RoleGroup(0).Mandatory = True

                    Me.Model.AddFactType(lrSubtypeRelationship.FactType,, abBroadcastInterfaceEvent)

#Region "FactTypeReading"
                    Dim lrFactTypeReading As FBM.FactTypeReading
                    Dim lasPredicatePart As New List(Of String)
                    lasPredicatePart.Add("is")
                    lasPredicatePart.Add("")

                    larRole.Clear()
                    larRole.Add(lrSubtypeRelationship.FactType.RoleGroup(0))
                    larRole.Add(lrSubtypeRelationship.FactType.RoleGroup(1))
                    lrFactTypeReading = New FBM.FactTypeReading(lrSubtypeRelationship.FactType, larRole, lasPredicatePart)
                    lrSubtypeRelationship.FactType.FactTypeReading.Add(lrFactTypeReading)

                    larRole.Clear()
                    larRole.Add(lrSubtypeRelationship.FactType.RoleGroup(1))
                    larRole.Add(lrSubtypeRelationship.FactType.RoleGroup(0))
                    lrFactTypeReading = New FBM.FactTypeReading(lrSubtypeRelationship.FactType, larRole, lasPredicatePart)
                    lrSubtypeRelationship.FactType.FactTypeReading.Add(lrFactTypeReading)
#End Region

                    larRole.Clear()
                    larRole.Add(lrSubtypeRelationship.FactType.RoleGroup(0))
                    lrSubtypeRelationship.FactType.CreateInternalUniquenessConstraint(larRole, False, False, True, True, arParentModelElement.GetTopmostSupertype)

                    larRole.Clear()
                    larRole.Add(lrSubtypeRelationship.FactType.RoleGroup(1))
                    lrSubtypeRelationship.FactType.CreateInternalUniquenessConstraint(larRole, False, False, True)
                Else
                    lrSubtypeRelationship.FactType = arUsingFactType
                End If
#End Region

                If Me.SubtypeRelationship.Count = 0 Then
                    lrSubtypeRelationship.IsPrimarySubtypeRelationship = True
                End If

                Me.SubtypeRelationship.Add(lrSubtypeRelationship)

                If Me.getCorrespondingRDSTable Is Nothing Then
                    If Not Me.IsAbsorbed Then
                        'Create a Table for the EntityType.
                        Dim lrTable As New RDS.Table(Me.Model.RDS, Me.Id, Me)
                        Call Me.Model.RDS.addTable(lrTable)
                    End If
                Else
                    Call Me.getCorrespondingRDSTable.triggerSubtypeRelationshipAdded()
                End If

                RaiseEvent SubtypeRelationshipAdded(lrSubtypeRelationship, abBroadcastInterfaceEvent)
                Call Me.Model.TriggerSubtypeRelationshipAdded(lrSubtypeRelationship)

                '=========================================================================
                'RDS
                'Manage Subtype Primary Reference Schemes.
                '  * Need to pull the Primary Reference Scheme from the Supertype if this EntityType is not absorbed.
                Call Me.getRDSPrimaryReferenceSchemeFromSupertypeIfNecessary()

                If Me.IsAbsorbed Then
                    Dim lrSupertypeTable = arParentModelElement.GetTopmostNonAbsorbedSupertype.getCorrespondingRDSTable
                    Call lrSupertypeTable.absorbSubtypeColumns(Me.getCorrespondingRDSTable)
                ElseIf Me.getCorrespondingRDSTable IsNot Nothing Then
                    Call Me.getCorrespondingRDSTable.absorbSupertypeColumns()
                    'Supertype needs to get Columns from this subtype
                End If 'Not Absorbed

                Call Me.Model.MakeDirty()

                Return lrSubtypeRelationship

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Pulls the RDS (Columns) Primary Reference Scheme from a Supertype of the ModelElement if the ModelElement is not absorbed.
        ''' </summary>
        Public Sub getRDSPrimaryReferenceSchemeFromSupertypeIfNecessary()

            'If Me.IsAbsorbed then there is nothing to do here.
            If Me.IsAbsorbed Then Exit Sub

            Dim lrTopmostNonAbsorbedEntityType = CType(Me.GetTopmostNonAbsorbedSupertype, FBM.EntityType)

            If lrTopmostNonAbsorbedEntityType.HasPrimaryReferenceScheme _
                And Not lrTopmostNonAbsorbedEntityType Is Me Then
                Dim lrSupertypeTable = lrTopmostNonAbsorbedEntityType.getCorrespondingRDSTable

                Dim larPKColumns = lrSupertypeTable.getPrimaryKeyColumns

                Dim lrTable = Me.getCorrespondingRDSTable
                Dim lrNewColumn As RDS.Column
                Dim larIndexColumn As New List(Of RDS.Column)

                For Each lrPKColumn In larPKColumns
                    lrNewColumn = lrPKColumn.Clone(lrTable)

                    lrNewColumn.Id = System.Guid.NewGuid.ToString
                    lrNewColumn.IsMandatory = True 'To be on the safe side                    
                    lrNewColumn.OrdinalPosition = lrTable.Column.Count + 1
                    'lrNewColumn.ContributesToPrimaryKey = True

                    If lrTable.Column.Contains(lrNewColumn) Then
                        lrNewColumn = lrTable.Column.Find(AddressOf lrNewColumn.Equals)
                    Else
                        lrTable.addColumn(lrNewColumn)
                    End If

                    lrNewColumn.Relation = New List(Of RDS.Relation) 'Leave here, otherwise Table.addColumn will assign any relation for the Original Column (in the Supertype)

                    lrTable.Column.AddUnique(lrNewColumn)
                    larIndexColumn.Add(lrNewColumn)
                Next

                For Each lrPKColumn In larPKColumns
                    For Each lrRelation In lrPKColumn.Relation

                        Dim lrNewRelation = lrRelation.Clone(lrTable)

                        lrNewRelation.Id = System.Guid.NewGuid.ToString
                        lrNewRelation.ResponsibleFactType = lrRelation.ResponsibleFactType
                        lrNewRelation.ReverseDestinationColumns.Add(lrPKColumn)
                        lrNewRelation.ReverseOriginColumns = lrRelation.ReverseOriginColumns

                        lrNewColumn = lrTable.Column.Find(Function(x) x.ActiveRole.Id = lrPKColumn.ActiveRole.Id)

                        'The NewColumn will have a Relation because cloning the Relation adds the Relation to the NewColum in lrTable.
                        lrNewRelation = lrNewColumn.Relation.Find(AddressOf lrNewRelation.EqualsByOriginColumnsDesinationTable) 'Should be the same Relation
                        If lrNewRelation IsNot Nothing Then
                            If Me.Model.RDS.Relation.Find(AddressOf lrNewRelation.EqualsByOriginColumnsDesinationTable) Is Nothing Then
                                Me.Model.RDS.addRelation(lrNewRelation)
                            Else
                                lrNewColumn.Relation.Remove(lrNewRelation)
                                lrNewColumn.Relation.Add(Me.Model.RDS.Relation.Find(AddressOf lrNewRelation.EqualsByOriginColumnsDesinationTable))
                            End If
                        End If
                    Next
                Next

                '------------------------
                'Index 
                Dim lrExistingIndex As RDS.Index = lrTable.getIndexByColumns(larPKColumns)

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

                    Call lrExistingIndex.setQualifier("PK")
                    Call lrExistingIndex.setName(lrTable.Name & "_PK")
                    Call lrExistingIndex.setIsPrimaryKey(True)
                End If
            End If

            For Each lrSubtype In Me.getSubtypes
                Call CType(lrSubtype, FBM.EntityType).getRDSPrimaryReferenceSchemeFromSupertypeIfNecessary()
            Next
        End Sub

        ''' <summary>
        ''' Returns the DataType of the EntityType or its topmost Supertype if the EntityType is a Subtype of an EntityType with a ReferenceModeValueType.
        ''' PRECONDITIONS: EntityType has a Simple Reference Scheme
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDataType() As pcenumORMDataType

            Try

                If Me.HasSimpleReferenceScheme Then
                    Dim lrTopmostSupertype As FBM.EntityType = Me.GetTopmostSupertype
                    Return lrTopmostSupertype.ReferenceModeValueType.DataType
                Else
                    Throw New Exception("EntityType: " & Me.Id & " does not have a Simple Reference Scheme.")
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return pcenumORMDataType.DataTypeNotSet
            End Try

        End Function

        ''' <summary>
        ''' Returns the DataTypeLength of the EntityType or its topmost Supertype if the EntityType is a Subtype of an EntityType with a ReferenceModeValueType.
        ''' PRECONDITIONS: EntityType has a Simple Reference Scheme
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDataTypeLength() As Integer

            Try

                If Me.HasSimpleReferenceScheme Then
                    Dim lrTopmostSupertype As FBM.EntityType = Me.GetTopmostSupertype
                    Return lrTopmostSupertype.ReferenceModeValueType.DataTypeLength
                Else
                    Throw New Exception("EntityType: " & Me.Id & " does not have a Simple Reference Scheme.")
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return 0
            End Try

        End Function

        ''' <summary>
        ''' Only used for EntityTypes with a CompoundReferenceScheme
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDownstreamActiveRoles(ByRef aarCoveredRoles As List(Of FBM.Role)) As List(Of FBM.Role)

            Try
                Dim larRolesToReturn As New List(Of FBM.Role)



                If Me.HasSimpleReferenceScheme Then

                    '20210824-VM-Needed when need to get the Active Role of a downstream EntityType with Simple Reference Scheme.
                    larRolesToReturn.Add(Me.ReferenceModeRoleConstraint.Role(0))
                    'Throw New Exception("This function not called for EntityTypes that have a SimpleReferenceScheme.")

                ElseIf Me.HasCompoundReferenceMode Then

                    Try
                        Dim lrTable As RDS.Table = Me.getCorrespondingRDSTable

                        Dim larColumn = lrTable.getPrimaryKeyColumns

                        Dim larRole = From Column In larColumn
                                      Select Column.ActiveRole

                        If larRole.Count > 0 Then
                            Return larRole.ToList
                        End If

                    Catch ex As Exception
                        GoTo FailsafeContinue
                    End Try

FailsafeContinue:
                    For Each lrRoleConstraintRole In Me.ReferenceModeRoleConstraint.RoleConstraintRole

                        If aarCoveredRoles.Contains(lrRoleConstraintRole.Role) Then
                            Return larRolesToReturn
                        End If

                        Select Case lrRoleConstraintRole.Role.JoinedORMObject.ConceptType

                            Case Is = pcenumConceptType.ValueType

                                aarCoveredRoles.Add(lrRoleConstraintRole.Role)
                                aarCoveredRoles.Add(lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))

                                larRolesToReturn.Add(lrRoleConstraintRole.Role)
                                '20210824-VM-Commented out below, because Only want ActiveRole. Not sure why this was included in RolesToReturn (below)
                                'larRolesToReturn.Add(lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))

                            Case Is = pcenumConceptType.EntityType

                                aarCoveredRoles.Add(lrRoleConstraintRole.Role)
                                aarCoveredRoles.Add(lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))

                                '20210824-VM-Removed the below, because was returning Roles not part of ActiveRoles.
                                'larRolesToReturn.Add(lrRoleConstraintRole.Role)
                                'larRolesToReturn.Add(lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))

                                larRolesToReturn.AddRange(lrRoleConstraintRole.Role.JoinsEntityType.getDownstreamActiveRoles(aarCoveredRoles))

                            Case Is = pcenumConceptType.FactType

                                larRolesToReturn.AddRange(lrRoleConstraintRole.Role.FactType.RoleGroup(0).getDownstreamRoleActiveRoles(aarCoveredRoles))

                        End Select

                    Next

                Else
                    Throw New Exception("This function only called for EntityTypes that have CompoundReferenceScheme.")
                End If

                Return larRolesToReturn

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.Role)
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

                If Me.HasSimpleReferenceScheme Then
                    lsCQLStatement = Me.Name & " is identified by its " & Me.ReferenceMode & ";"

                ElseIf Me.HasCompoundReferenceMode Then

                    lsCQLStatement = Me.Name & " is identified by "

                    Dim lrModelObject As FBM.ModelObject
                    Dim larModelObject As New List(Of FBM.ModelObject)
                    Dim larFactType As New List(Of FBM.FactType)
                    For Each lrRoleConstraintRole In Me.ReferenceModeRoleConstraint.RoleConstraintRole
                        lrModelObject = lrRoleConstraintRole.Role.JoinedORMObject
                        Call lrRoleConstraintRole.Role.FactType.GetPreboundReadingTextForModelElementAtPosition(lrModelObject, 2)
                        larModelObject.Add(lrModelObject)
                        larFactType.AddUnique(lrRoleConstraintRole.Role.FactType)
                    Next

                    Dim liInd As Integer = 0
                    For Each lrModelObject In larModelObject
                        If liInd <> 0 Then
                            lsCQLStatement.AppendString(" and ")
                        End If
                        lsCQLStatement.AppendString(lrModelObject.PreboundReadingText)
                        lsCQLStatement.AppendString(lrModelObject.Name)
                        liInd += 1
                    Next
                    lsCQLStatement.AppendString(" where " & vbCrLf)

                    liInd = 0
                    For Each lrFactType In larFactType
                        For Each lrFactTypeReading In lrFactType.FactTypeReading
                            If liInd > 0 Then lsCQLStatement.AppendString("," & vbCrLf)
                            lsCQLStatement.AppendString(vbTab & lrFactTypeReading.GetReadingTextCQL(True, True))
                            liInd += 1
                        Next
                    Next

                    lsCQLStatement.AppendString(";")
                End If

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
        ''' Returns the unique Signature of the EntityType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetSignature() As String

            Dim lsSignature As String

            lsSignature = Me.Id

            If IsSomething(Me.ReferenceModeValueType) Then
                lsSignature &= Me.ReferenceModeValueType.Id & Me.ReferenceModeValueType.DataType.ToString
                lsSignature &= Me.PreferredIdentifierRCId
            End If

            Return lsSignature

        End Function

        Public Overrides Function getSubtypes(Optional ByVal abPrimarySubtypeRelationshipsOnly As Boolean = False) As List(Of FBM.ModelObject)

            Try
                Dim larModelElement As List(Of FBM.ModelObject)

                If abPrimarySubtypeRelationshipsOnly Then
                    larModelElement = (From EntityType In Me.Model.EntityType
                                       From SubtypeRelationship In EntityType.SubtypeRelationship
                                       Where SubtypeRelationship.parentModelElement.Id = Me.Id
                                       Where SubtypeRelationship.IsPrimarySubtypeRelationship = True
                                       Select SubtypeRelationship.ModelElement).ToList
                Else
                    larModelElement = (From EntityType In Me.Model.EntityType
                                       From SubtypeRelationship In EntityType.SubtypeRelationship
                                       Where SubtypeRelationship.parentModelElement.Id = Me.Id
                                       Select SubtypeRelationship.ModelElement).ToList
                End If

                Return larModelElement

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.ModelObject)
            End Try

        End Function

        ''' <summary>
        ''' Returns True if the EntityType has a Compound Reference Mode, else returns False
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasCompoundReferenceMode() As Boolean

            Try
                HasCompoundReferenceMode = False


                Dim lrModelElement As FBM.EntityType = Me 'Even if checking for an ObjectifiedFactType, will be checking against the ObjectifyingEntityType

                If Me.IsSubtype Then
                    lrModelElement = Me.GetTopmostNonAbsorbedSupertype(True)
                End If

                If (lrModelElement.ReferenceMode = "") And (lrModelElement.ReferenceModeRoleConstraint Is Nothing) Then
                    HasCompoundReferenceMode = False
                ElseIf lrModelElement.ReferenceModeRoleConstraint IsNot Nothing Then
                    If lrModelElement.ReferenceModeRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
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

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Overrides Function HasPrimaryReferenceScheme() As Boolean

            Try
                If Me.IsObjectifyingEntityType Then
                    Return True 'Because the Objectified Fact Type has a Primary Reference Scheme by default (or should).
                ElseIf (Me.HasSimpleReferenceScheme Or Me.HasCompoundReferenceMode) Then
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

                Return False
            End Try

        End Function

        Public Overridable Function HasSimpleReferenceScheme(Optional ByVal abIgnoreThrowingRemoveReferenceModeError As Boolean = False) As Boolean

            Try


                If Me.IsSubtype Then
                    Dim lrTopmostSupertype As FBM.EntityType = Me.GetTopmostSupertype(True)

                    If Trim(lrTopmostSupertype.ReferenceMode) = "" Then
                        Return False
                    ElseIf lrTopmostSupertype.HasCompoundReferenceMode Then
                        Return False
                    ElseIf Trim(lrTopmostSupertype.ReferenceMode) <> "" Then
                        Return True
                    End If
                Else
                    If Trim(Me.ReferenceMode) = "" Then
                        Return False
                    ElseIf Me.HasCompoundReferenceMode Then
                        Return False
                    ElseIf Trim(Me.ReferenceMode) <> "" Then
                        If Me.ReferenceModeValueType Is Nothing Or
                           Me.ReferenceModeFactType Is Nothing Then

                            '--------------------------------------------------------------------------------------
                            'CodeSafe: Model is in an unstable state. Remove the ReferenceMode and warn the user.
                            '--------------------------------------------------------------------------------------
                            Dim lsOldReferenceMode As String = Me.ReferenceMode
                            Me.ReferenceMode = ""
                            Me.ReferenceModeRoleConstraint = Nothing
                            Me.ReferenceModeFactType = Nothing
                            Me.ReferenceModeValueType = Nothing
                            If Me.Model.StoreAsXML Then
                                Me.Model.Save()
                            Else
                                Me.Save()
                            End If

                            If Not abIgnoreThrowingRemoveReferenceModeError Then
                                Dim lsMessage As String = ""
                                lsMessage = "Entity Type, '" & Me.Id & "', had a Reference Mode but no associated Value Type or Fact Type."
                                lsMessage &= vbCrLf & "As a precaution, the ReferenceMode has been removed and the Model saved."
                                lsMessage.AppendDoubleLineBreak("Reference Mode was: " & lsOldReferenceMode)
                                Try
                                    If Me.IsObjectifyingEntityType Then
                                        lsMessage.AppendLine("Objectifying Entity Type for Fact Type: " & Me.ObjectifiedFactType.Id)
                                    End If
                                Catch ex As Exception
                                    'Not a biggie, but want the message returned no matter what.
                                End Try

                                Throw New Exception(lsMessage)
                            End If
                        ElseIf Me.ReferenceModeRoleConstraint IsNot Nothing Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If

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
        ''' Returns True if the EntityType is a Subtype, else returns False.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function IsSubtype() As Boolean

            Return Me.parentModelObjectList.Count > 0

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

            Dim larFact = From FactType In Me.Model.FactType
                          From Fact In FactType.Fact
                          From FactData In Fact.Data
                          Where FactData.Role.JoinedORMObject Is Me
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

            Dim larChildEntityType = From EntityType In Me.Model.EntityType
                                     From lrParentEntityType In EntityType.parentModelObjectList
                                     Where lrParentEntityType Is Me
                                     Select EntityType

            For Each lrEntityType In larChildEntityType
                If Not aarObjectType.Contains(lrEntityType) Then
                    Call lrEntityType.RemoveDataInstanceRecursive(asInstance, aarObjectType)
                End If
            Next

        End Sub

        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = True,
                                                  Optional ByVal abRemoveIndex As Boolean = True,
                                                  Optional ByVal abIsPartOfSimpleReferenceScheme As Boolean = False) As Boolean

            Dim lrEntityType As FBM.EntityType
            Dim lrSubtype As FBM.tSubtypeRelationship

            Try
                If abForceRemoval Then
                Else
                    If Me.IsObjectifyingEntityType And Me.ObjectifiedFactType IsNot Nothing Then
                        MsgBox("You cannot remove Entity Type, '" & Trim(Me.Name) & "' while it is the Objectifying Entity Type of a Fact Type.")
                        Return False
                        Exit Function
                    End If

                    If Me.ExistsRolesAssociatedWithEntityType(abIncludeSubtypeRelationshipFactTypes) And
                       Not (Me.HasSimpleReferenceScheme And
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

                If abDoDatabaseProcessing Then
                    For Each lrEntityType In Me.parentModelObjectList
                        lrSubtype = New FBM.tSubtypeRelationship
                        lrSubtype.ModelElement = Me
                        lrSubtype.parentModelElement.Id = lrEntityType.Id
                        lrSubtype.Model = Me.Model
                        Call TableSubtypeRelationship.DeleteParentEntityType(lrSubtype)
                    Next
                End If

                Dim lrValueType As FBM.ValueType
                lrValueType = Me.ReferenceModeValueType 'Do this first, because removing the FactType (below) will remove the ReferenceModelValueType from the EntityType if the FT is the ReferenceModeFactType of the ET.

                Dim lrFactType As FBM.FactType
                lrFactType = Me.ReferenceModeFactType
                If IsSomething(Me.ReferenceModeFactType) Then
                    Call lrFactType.RemoveFromModel(True, False, abDoDatabaseProcessing)
                End If


                If IsSomething(lrValueType) Then
                    Call lrValueType.RemoveFromModel(False, False, abDoDatabaseProcessing)
                End If

                If abDoDatabaseProcessing Then
                    Call TableEntityType.DeleteEntityType(Me)

                    '-----------------------------------------------------------------------------------------------------
                    'Models Stored as XML need to be saved to remove the appropriate ModelElements, and is a quick save.
                    If Me.Model.StoreAsXML Then Me.Model.Save()
                End If

                For Each lrSubtypeRelationship In Me.SubtypeRelationship
                    Call Me.Model.RemoveFactType(lrSubtypeRelationship.FactType, True)
                Next

                Me.Model.RemoveEntityType(Me, abDoDatabaseProcessing)

                RaiseEvent RemovedFromModel(abDoDatabaseProcessing)

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                If Me.HasSimpleReferenceScheme And Me.ReferenceModeFactType Is Nothing Then
                    'Likely not a Supertype. I.e. Is likely a subtype with no actual ReferenceMode but inherited.
                    Exit Sub
                End If

                If Me.HasSimpleReferenceScheme Or Me.ReferenceModeFactType IsNot Nothing Then


                    '================================================================================================
                    'RDS - Do RDS Processing first.
                    Dim larColumn = From Table In Me.Model.RDS.Table
                                    From Column In Table.Column
                                    Where Column.ActiveRole Is Me.ReferenceModeFactType.RoleGroup(1)
                                    Select Column

                    For Each lrColumn In larColumn.ToArray

                        Dim larCoveredRoles As New List(Of FBM.Role) 'So a recurring loop doesn't happen.

                        Dim larDownstreamRole As New List(Of FBM.Role)
                        larDownstreamRole = (From Role In lrColumn.Role.getDownstreamRolePaths(Nothing, larCoveredRoles).FindAll(Function(x) x.JoinedORMObject Is Me)
                                             Select Role Distinct).ToList

                        If larDownstreamRole.Count = 1 Then
                            'Great, this is simple
                            lrColumn.setActiveRole(larDownstreamRole(0))
                        Else
                            For Each lrRole In larDownstreamRole
                                Dim lrActualColumn = lrColumn.Table.Column.Find(Function(x) x.ActiveRole Is Me.ReferenceModeFactType.RoleGroup(1))
                                lrActualColumn.setActiveRole(lrRole) 'May not be lrColumn, but that's okay...will get that Column in subsequent round.
                            Next
                        End If
                    Next
                    '==End RDS=====================================================================================


                    Me.ReferenceModeFactType.IsPreferredReferenceMode = False
                    Me.ReferenceModeFactType = Nothing
                    Me.ReferenceModeValueType = Nothing
                    Me.ReferenceModeRoleConstraint.SetIsPreferredIdentifier(False)
                    Me.ReferenceModeRoleConstraint = Nothing
                    'NB Me.PreferredIdentifierRCId set in the Property.Set method of ,ReferenceModeRoleConstraint (set to "")
                    Me.ReferenceMode = ""

                    Me.isDirty = True
                    Call Me.Model.MakeDirty(False, False)

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
                        RaiseEvent ReferenceModeChanged(Me.ReferenceMode, False, False)
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Sub RemoveSubtypeRelationship(ByRef arSubtypeRelationship As FBM.tSubtypeRelationship)

            Me.parentModelObjectList.Remove(arSubtypeRelationship.parentModelElement)
            arSubtypeRelationship.parentModelElement.childModelObjectList.Remove(Me)
            Me.SubtypeRelationship.Remove(arSubtypeRelationship)
            '--------------------------------------------------------------------------------
            'Deletion from the database is handled in FBM.SubtypeRelationship.RemoveFromModel
            '--------------------------------------------------------------------------------

            If Me.SubtypeRelationship.Count = 0 Then
                'No point in the ModelObject being Absorbed.
                Me.IsAbsorbed = False
            End If

            Call Me.Model.TriggerSubtypeRelationshipRemoved(arSubtypeRelationship)

            RaiseEvent SubtypeConstraintRemoved(arSubtypeRelationship)

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

                Dim liUsingFactDataCount = Aggregate FactType In Me.Model.FactType
                                            From Fact In FactType.Fact
                                            From FactData In Fact.Data
                                            Where FactData.Role.JoinedORMObject.Id = Me.Id _
                                            And FactData.Data = lsInstance
                                            Into Count()

                If liUsingFactDataCount = 0 Then
                    Me.Instance.Remove(lsInstance)
                End If

            Next


        End Sub

        ''' <summary>
        ''' Saves the EntityType to the database
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Dim lsMessage As String = ""

            Try
                '20220129-VM-Haven't seen this error for a long time. If after a time, not missed, then remove.
                'Dim lrDictionaryEntry = Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id) And x.isEntityType)

                ''--------------------------------------------------------------------------------
                ''CodeSafe: If there is no dictionary entry for the EntityType, then create one.
                ''--------------------------------------------------------------------------------
                'If lrDictionaryEntry Is Nothing Then
                '    lsMessage = "Tried to save an EntityType with no corresponding DictionaryEntry."
                '    lsMessage &= vbCrLf & vbCrLf & "Creating a DictionaryEntry for the EntityType"

                '    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)

                '    lrDictionaryEntry = New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.EntityType, Me.ShortDescription, Me.LongDescription)
                '    lrDictionaryEntry.isDirty = True
                'End If

                'lrDictionaryEntry.isEntityType = True

                If abRapidSave Then
                    Try
                        pdbConnection.BeginTrans()
                        '20220129-VM-Haven't seen this error for a long time. If after a time, not missed, then remove.
                        'Call lrDictionaryEntry.Save()
                        Call TableEntityType.AddEntityType(Me)
                        pdbConnection.CommitTrans()
                        Me.isDirty = False
                    Catch ar_err As Exception
                        pdbConnection.RollbackTrans()
                        Throw New ApplicationException("Error: EntityType.Save: " & ar_err.Message)
                    End Try
                ElseIf Me.isDirty Then

                    If TableEntityType.ExistsEntityTypeByModel(Me) Then
                        Call TableEntityType.UpdateEntityType(Me)
                        '20220129-VM-Haven't seen this error for a long time. If after a time, not missed, then remove.
                        'Call lrDictionaryEntry.Save()
                    Else
                        Try
                            pdbConnection.BeginTrans()
                            '20220129-VM-Haven't seen this error for a long time. If after a time, not missed, then remove.
                            'Call lrDictionaryEntry.Save()
                            If TableEntityType.ExistsEntityType(Me) Then
                                '----------------------------------------------------------------
                                'Entity Type already exists in Richmond DB, so no need to add.
                                '  i.e. EntityType did not exist in Model, but did in Boston.
                                '  EntityTypes in Richmond are model independant.
                                '----------------------------------------------------------------
                            Else
                                Call TableEntityType.AddEntityType(Me)
                            End If
                            pdbConnection.CommitTrans()
                        Catch ar_err As Exception
                            pdbConnection.RollbackTrans()
                            Throw New ApplicationException("Error: EntityType.Save: " & ar_err.Message)
                        End Try
                    End If

                    Me.isDirty = False
                End If

                '--------------------------------------------------------------
                'Save any SubtypeRelationships associated with the EntityType
                '-----------------------------------------
                Dim lrSubtypeRelationship As FBM.tSubtypeRelationship
                For Each lrSubtypeRelationship In Me.SubtypeRelationship
                    Call lrSubtypeRelationship.Save(abRapidSave)
                Next

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="asReferenceMode">The ReferenceMode for the EntityType</param>
        ''' <param name="abSimpleAssignment">True if just setting the ReferenceMode without creating references or changing anything, else False</param>
        ''' <param name="asValueTypeName">Provided if the name for the ValueType is known and need not be generated.</param>
        ''' <remarks></remarks>
        Public Overrides Sub SetReferenceMode(ByVal asReferenceMode As String,
                                              Optional ByVal abSimpleAssignment As Boolean = False,
                                              Optional ByVal asValueTypeName As String = Nothing,
                                              Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                              Optional ByVal aiORMDataType As pcenumORMDataType = pcenumORMDataType.TextVariableLength,
                                              Optional ByVal abSuppressModelSave As Boolean = False,
                                              Optional ByVal abSuppressSettingReferenceModeFTVT As Boolean = False)

            Try
                If IsSomething(Me.ReferenceModeValueType) Or IsSomething(Me.ReferenceModeFactType) Then
                    '-----------------------------------------------------------------------------------------------------------------
                    ' The EntityType already has a ReferenceMode, so change the ReferenceMode values of the respective Model Objects
                    '  to the new ReferenceMode
                    '-----------------------------------------------------------------------------------------------------------------
                    Dim lrValueType As FBM.ValueType

                    Me.ReferenceMode = asReferenceMode
                    lrValueType = Me.ReferenceModeValueType
                    lrValueType.DataType = aiORMDataType

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
                        Me.ReferenceModeFactType.RemoveFromModel()
                        Me.ReferenceModeRoleConstraint = Nothing 'Was before removing the ReferenceModeFactType

                        Me.ReferenceModeFactType = Nothing

                        lrValueType.RemoveFromModel()
                        Me.ReferenceModeValueType = Nothing

                        Dim lrModelError As New FBM.ModelError(pcenumModelErrors.EntityTypeRequiresReferenceSchemeError, "", Nothing, Me, True)
                    Else
                        lrValueType.SetName(Me.MakeReferenceModeName, abBroadcastInterfaceEvent)
                    End If
                ElseIf abSimpleAssignment Then
                    '------------------------------------------------------------------------------------------------------------------------
                    'Simply setting the ReferenceMode of the EntityType and which needs to be set for all corresponding EntityTypeInstances
                    '------------------------------------------------------------------------------------------------------------------------
                    'CodeSafe: Make sure the relevant references are set
                    Dim lrRoleConstraint As FBM.RoleConstraint = Me.Model.RoleConstraint.Find(Function(x) x.Id = Me.PreferredIdentifierRCId)

                    If asReferenceMode <> "" Then
                        Me.ReferenceMode = asReferenceMode
                        If Not abSuppressSettingReferenceModeFTVT Then
                            Me.ReferenceModeFactType = lrRoleConstraint.RoleConstraintRole(0).Role.FactType
                            Me.ReferenceModeValueType = lrRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject
                        End If
                    End If

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
                    Me.CreateReferenceMode(asReferenceMode, asValueTypeName, aiORMDataType, abBroadcastInterfaceEvent)
                End If

                Me.ReferenceMode = asReferenceMode
                Me.isDirty = True
                Me.Model.MakeDirty()

                '-------------------------------------------------------------------------------------------------------------------
                'VM-20151014-At this stage, 'MakeReferenceModeName', is called from within the EntityTypeInstance.
                '  i.e. A ReferenceMode of '.Id' will have a ValueType name of '<EntityTypeName>_Id' set by the EntityTypeInstance
                '  by virtue of the following Event being raised.
                '  NB I don't know why this is the case. At some stage this functionality may be moved back down to the Model level.
                '-------------------------------------------------------------------------------------------------------------------
                RaiseEvent ReferenceModeChanged(asReferenceMode, abSimpleAssignment, abBroadcastInterfaceEvent)

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateEntityType, Me, Nothing)
                End If

                'So much happens/changes, so save.
                If Not abSuppressModelSave Then
                    Call Me.Model.Save()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function MakeReferenceModeName() As String

            Dim lsReferenceModeName As String = ""

            Try
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
                        If Me.ReferenceMode.Substring(0, 1) = "." Then
                            lsReferenceModeName = Me.Id & "_" & Me.ReferenceMode.Substring(1, Me.ReferenceMode.Length - 1)
                        Else
                            lsReferenceModeName = Me.ReferenceMode
                        End If
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lsReferenceModeName

        End Function

        Public Overrides Sub renameInstance(ByVal asOriginalInstance As String, ByVal asNewInstance As String)

            Try
                Dim larFactType = From FactType In Me.Model.FactType
                                  From Role In FactType.RoleGroup
                                  Where Role.JoinedORMObject.Id = Me.Id
                                  Select FactType

                For Each lrFactType In larFactType

                    Dim lrFact As New FBM.Fact(lrFactType)
                    Dim lrRole As New FBM.Role

                    lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = Me.Id)
                    lrFact.Data.Add(New FBM.FactData(lrRole, New FBM.Concept(Trim(asOriginalInstance))))

                    Dim larFact As New List(Of FBM.Fact)

                    larFact = lrFactType.Fact.FindAll(Function(x) x.Data.Find(Function(y) y.Role.Id = lrRole.Id).Data = asOriginalInstance)

                    For Each lrModifyingFact In larFact
                        lrModifyingFact.Data.Find(Function(x) x.Role.Id = lrRole.Id).Data = asNewInstance
                        lrModifyingFact.makeDirty()
                    Next

                Next

                Me.Instance.Remove(asOriginalInstance)
                Me.Instance.Add(asNewInstance)

                For Each lrEntityType In Me.getSubtypes
                    Call lrEntityType.renameInstance(asOriginalInstance, asNewInstance)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Overrides Sub removeInstance(ByVal asInstance As String)

            Try
                Dim larFactType = From FactType In Me.Model.FactType
                                  From Role In FactType.RoleGroup
                                  Where Role.JoinedORMObject IsNot Nothing
                                  Where Role.JoinedORMObject.Id = Me.Id
                                  Select FactType

                For Each lrFactType In larFactType.ToArray
                    Dim lrFact As New FBM.Fact(lrFactType)
                    Dim lrRole As New FBM.Role

                    lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = Me.Id)
                    lrFact.Data.Add(New FBM.FactData(lrRole, New FBM.Concept(Trim(asInstance))))

                    lrFactType.RemoveFactByData(lrFact, True)
                Next

                Me.Instance.Remove(asInstance)

                For Each lrEntityType In Me.getSubtypes
                    Call lrEntityType.removeInstance(asInstance)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveComplexReferenceScheme()

            Dim lsMessage As String

            Try
                If Me.ReferenceModeRoleConstraint Is Nothing Then
                    Throw New Exception("Entity Type has no Reference Scheme. ReferenceShemeRoleConstraint is Nothing.")
                ElseIf Me.ReferenceModeRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    lsMessage = "Tried to remove CompoundReferenceScheme from an EntityType that has a SimpleReferenceScheme."
                    Throw New Exception(lsMessage)
                End If

                If Me.ReferenceModeRoleConstraint.IsPreferredIdentifier Then Me.ReferenceModeRoleConstraint.SetIsPreferredIdentifier(False)

                Me.ReferenceModeRoleConstraint = Nothing

                Me.isDirty = True
                Call Me.Model.MakeDirty()

                RaiseEvent ReferenceModeRoleConstraintChanged(Nothing)

            Catch ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="abIsActor"></param>
        ''' <param name="abSimpleSet">True when loading the CMML (Model.UML) model. So that EntityTypeInstances are updated.</param>
        Public Sub setIsActor(ByVal abIsActor As Boolean, Optional abSimpleSet As Boolean = False)

            Try
                Me._IsActor = abIsActor

                'CMML Model
                If Not abSimpleSet Then
                    Dim lrCMMLActor = Me.getCorrespondingCMMLActor
                    If Me._IsActor And lrCMMLActor Is Nothing Then
                        lrCMMLActor = New CMML.Actor(Me.Model.UML, Me.Id, Me)
                        Call Me.Model.UML.addActor(lrCMMLActor)
                    ElseIf Me._IsActor And lrCMMLActor IsNot Nothing Then
                        'Do nothing because something has gone wrong and this EntityType should have already been IsActor=True.
                    ElseIf Not Me._IsActor Then
                        Call Me.Model.UML.RemoveActor(lrCMMLActor)
                    End If
                End If
                RaiseEvent IsActorChanged(abIsActor)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetDataType(ByVal aiORMDataType As pcenumORMDataType,
                               Optional ByVal aiDataTypeLength As Integer = Nothing,
                               Optional ByVal aiDataTypePrecision As Integer = Nothing,
                               Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

            Try
                If Me.ReferenceModeValueType IsNot Nothing Then
                    Call Me.ReferenceModeValueType.SetDataType(aiORMDataType, aiDataTypeLength, aiDataTypePrecision, abBroadcastInterfaceEvent)
                Else
                    prApplication.ThrowErrorMessage("The Entity Type, " & Me.Id & ", does not have a Reference Mode Value Type to set its data type.", pcenumErrorType.Warning,, False,, True)
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetDerivationText(ByVal asDerivationText As String, ByVal abBroadcastInterfaceEvent As Boolean)

            Me.DerivationText = asDerivationText
            RaiseEvent DerivationTextChanged(asDerivationText)

            Me.isDirty = True
            Call Me.Model.MakeDirty(False, True)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
            End If

        End Sub

        ''' <summary>
        ''' Sets the Name, Symbol and Id of an EntityType.
        ''' </summary>
        ''' <param name="asNewName"></param>
        ''' <remarks>Preconditions: The uniqueness of the new EntityType.Name amoungst EntityTypes, ValueTypes, FactTypes and RoleConstraints has already been verified.</remarks>
        Public Overrides Function SetName(ByVal asNewName As String,
                                          Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                          Optional ByVal abSuppressModelSave As Boolean = False) As Boolean
            '-----------------------------------------------------------------------------------------------------------------
            'The following explains the logic and philosophy of Boston.
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
                Dim lsOldName As String = Me.Id

                _Name = asNewName
                Me.Symbol = asNewName

                'Check to see that the name begins with a capital letter.
                If Not Char.IsUpper(asNewName.Chars(0)) Then
                    Throw New tInformationException("Object Type names must start with a capital letter followed by one or more lowercase letters.")
                    Return False
                End If

                '--------------------------------------------------------------------------------------------------------
                'The surrogate key for the EntityType is about to change (to match the name of the EntityType)
                '  so update the ModelDictionary entry for the Concept/Symbol (the nominalistic idenity of the VactType
                '--------------------------------------------------------------------------------------------------------
                If StrComp(Me.Id, asNewName) <> 0 Then
                    '----------------------------------------------------------
                    'The new EntityType.Name does not match the EntityType.Id
                    '----------------------------------------------------------
                    Dim lrModelDictionaryEntry = Me.SwitchConcept(New FBM.Concept(asNewName, True), pcenumConceptType.EntityType)

                    '------------------------------------------------------------------------------------------
                    'Update the Model(database) immediately. There is no choice. The reason why
                    '  is because the (in-memory) key is changing, so the database must be updated to 
                    '  reflect the new key, otherwise it will not be possible to Update an existing EntityType.
                    '------------------------------------------------------------------------------------------
                    Call TableEntityType.ModifyKey(Me, asNewName, lrModelDictionaryEntry)
                    Call TableConceptInstance.ModifyKey(Me, asNewName)

                    '-------------------------------------------------------
                    'Update all of the respective ParentEntityType records
                    '  in the database as well
                    '-------------------------------------------------------
                    Call TableSubtypeRelationship.ModifyKey(Me, asNewName)

                    If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                        Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateEntityType, Me, Nothing)
                    End If

                    Me.Id = asNewName

                    If Me.Model.StoreAsXML Then
                        Me.Model.Save()
                    Else
                        Call TableEntityType.UpdateEntityType(Me) 'Sets the new Name

                        Dim larRole = From Role In Me.Model.Role
                                      Where Role.JoinedORMObject Is Me
                                      Select Role

                        For Each lrRole In larRole
                            lrRole.makeDirty()
                            lrRole.FactType.makeDirty()
                            If Not abSuppressModelSave Then lrRole.FactType.Save(False)
                        Next
                    End If

                    '====================================================================================
                    'RDS
                    Dim lrTable As RDS.Table = Me.Model.RDS.Table.Find(Function(x) x.FBMModelElement Is Me)
                    If lrTable IsNot Nothing Then
                        Call lrTable.setName(asNewName)
                    End If

                    Me.Model.MakeDirty()

                End If

                RaiseEvent updated(Me.ConceptType)
                Call Me.RaiseEventNameChanged(lsOldName, asNewName)
                Call Me.Model.TriggerEventModelElementModified(Me)

                '------------------------------------------------------------------------------------
                'Must save the Model because Roles that reference the EntityType must be saved.
                '  NB If Roles are saved, the FactType must be saved. If the FactType is saved,
                '  the RoleGroup's references (per Role) must be saved. A Role within the RoleGroup 
                '  may reference another FactType, so that FactType must be saved...etc.
                '  i.e. It's easier and safer to simply save the whole model.
                '------------------------------------------------------------------------------------
                If Me.Model.StoreAsXML Then
                    Me.Model.Save()
                Else
                    For Each lrRole In Me.Model.Role.FindAll(Function(x) x.JoinedORMObject.Id = Me.Id)
                        lrRole.makeDirty()
                        lrRole.FactType.makeDirty()
                        If Not abSuppressModelSave Then lrRole.FactType.Save()
                    Next
                End If

                '==============================================================================================
                'ReferenceModeFactTypeName if one exists
                If Me.HasSimpleReferenceScheme And Me.ReferenceModeValueType IsNot Nothing Then
                    Dim lsValueTypeName = Me.MakeReferenceModeName
                    Call Me.ReferenceModeValueType.SetName(lsValueTypeName, abBroadcastInterfaceEvent, abSuppressModelSave)
                    Dim lsFactTypeName As String = Me.Id & "Has" & Me.ReferenceModeValueType.Id
                    Call Me.ReferenceModeFactType.setName(lsFactTypeName, abBroadcastInterfaceEvent, abSuppressModelSave)
                End If
                '==============================================================================================

                '-------------------------------------------------------------
                'To make sure all the FactData and FactDataInstances/Pages are saved for RDS
                If Not abSuppressModelSave Then
                    Me.Model.Save()
                End If

                Return True

            Catch iex As tInformationException
                prApplication.ThrowErrorMessage(iex.Message, pcenumErrorType.Information, Nothing, False, False, True)
                Return False
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tEntityType.SetName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Public Sub SetReferenceModeObjects()

            Try
                If Me.PreferredIdentifierRCId = "" Then
                    Me.ReferenceModeFactType = Nothing
                    Me.ReferenceModeRoleConstraint = Nothing
                    Me.ReferenceModeValueType = Nothing
                Else
                    'Me.ReferenceModeRoleConstraint = New FBM.RoleConstraint(Me.Model,  Me.PreferredIdentifierRCId, True, pcenumRoleConstraintType.InternalUniquenessConstraint)
                    Me.ReferenceModeRoleConstraint = Me.Model.RoleConstraint.Find(Function(x) x.Id = Me.PreferredIdentifierRCId)

                    If Me.ReferenceModeRoleConstraint Is Nothing Then
                        'CodeSafe
                        Me.PreferredIdentifierRCId = ""
                        Me.ReferenceMode = ""
                        If Me.Model.StoreAsXML Then
                            Me.Model.Save()
                        Else
                            Call Me.Save()
                        End If
                        Exit Sub
                    End If

                    If Me.ReferenceModeRoleConstraint.RoleConstraintType <> pcenumRoleConstraintType.InternalUniquenessConstraint Then
                        GoTo SkipSettingReferenceModeObjects
                    End If

                    If Me.ReferenceModeRoleConstraint.Role.Count = 1 Then

                        '--------------------------------------------------------------------------------------------------
                        'CodeSafe: Remove the ReferenceMode altogether if the Me.ReferenceModeRoleConstraint.Role(0).JoinedORMObject is an EntityType.
                        '  Error has happened before when having copied a set of ModelElements from another Page (i.e. a Diagram).
                        '  While that rightly needs to be bug free, so to the successful loading of a Model
                        '--------------------------------------------------------------------------------------------------
                        If Me.ReferenceModeRoleConstraint.Role(0).JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                            Call Me.RemoveSimpleReferenceScheme()
                            Throw New Exception("The reference mode role constraint for Entity Type, " & Me.Id & ", refererenced an Entity Type rather than a Value Type. As a precaution the Reference Mode for the Entity Type was removed.")
                        End If

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
SkipSettingReferenceModeObjects:

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: FBM.EntityType.SetReferenceModeObjects"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "Model.Id: " & Me.Model.ModelId
                lsMessage &= vbCrLf & "EntityType.Id: " & Me.Id
                lsMessage &= vbCrLf & "PreferredIdentifierRCId: " & Me.PreferredIdentifierRCId
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub ConceptUpdated(ByVal aiConceptType As pcenumConceptType) Handles Me.updated

            Me.Id = Me.Symbol
            Me.Name = Me.Symbol

        End Sub

        Public Shadows Event updated(ByVal aiConceptType As pcenumConceptType)
        Public Event ModelErrorsRemoved() Implements iValidationErrorHandler.ModelErrorsRemoved

        Private Sub Concept_Updated() Handles Concept.ConceptSymbolUpdated

            'VM-20180319-Commented out, because was calling SetName twice/again when the Name of the EntityType was simply changed.
            'Me.SetName(Me.Concept.Symbol)

            'VM-20180319-Dummy Code to trap the debug stoppoint, to see if removing the above affects anything
            Dim liDummyCodeInd As Integer = 0

        End Sub

        ''' <summary>
        ''' Sets the CompoundReferenceScheme.RoleConstraint for the EntityType.
        ''' NB Precondition: EntityType has no ReferenceMode (SimpleReferenceScheme), else throws exception.
        ''' </summary>
        ''' <param name="arRoleConstraint">The RoleConstraint that defines the CompoundReferenceScheme for the EntityType</param>
        ''' <remarks></remarks>
        Public Overrides Sub SetCompoundReferenceSchemeRoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint)

            Dim lsMessage As String

            Try
                If Me.HasSimpleReferenceScheme Then
                    lsMessage = "Entity Type: " & Me.Id & vbCrLf
                    lsMessage &= "Tried to set a CompoundReferenceMode for an EntityType that already has a SimpleReferenceScheme." & vbCrLf
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
                    Me.isDirty = True
                    Me.Model.MakeDirty(True, True)
                    RaiseEvent ReferenceModeRoleConstraintChanged(arRoleConstraint)
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub setIsDatabaseReservedWord(ByVal abIsDatabaseReservedWord As Boolean)
            Me.IsDatabaseReservedWord = abIsDatabaseReservedWord
            RaiseEvent IsDatabaseReservedWordChanged(abIsDatabaseReservedWord)
        End Sub

        ''' <summary>
        ''' Changes whether or not the EntityType is an Objectifying Entity Type.
        '''   NB Rare, if ever, this method will be used.
        ''' </summary>
        ''' <param name="abIsObjectifyingEntityType"></param>
        ''' <remarks></remarks>
        Public Sub SetIsObjectifyingEntityType(ByVal abIsObjectifyingEntityType As Boolean, Optional ByVal abBroadcastInterfaceEvent As Boolean = True)
            Me.IsObjectifyingEntityType = abIsObjectifyingEntityType

            RaiseEvent IsObjectifyingEntityTypeChanged(abIsObjectifyingEntityType)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateEntityType, Me, Nothing)
            End If

            Me.isDirty = True
            Me.Model.MakeDirty(False)
        End Sub

        Public Sub SetIsPersonal(ByVal abNewIsPersonal As Boolean, Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

            Me.IsPersonal = abNewIsPersonal
            RaiseEvent IsPersonalChanged(abNewIsPersonal)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateEntityType, Me, Nothing)
            End If

            Me.isDirty = True
            Me.Model.MakeDirty(False, False)
        End Sub

        ''' <summary>
        ''' Changed the FactType objectified by the EntityType
        '''   NB Rare, if ever, this method will be used.
        ''' </summary>
        ''' <param name="arNewObjectifiedFactType"></param>
        ''' <remarks></remarks>
        Public Sub SetObjectifiedFactType(ByRef arNewObjectifiedFactType As FBM.FactType)

            Me.ObjectifiedFactType = arNewObjectifiedFactType

            Me.isDirty = True
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent ObjectifiedFactTypeChanged(arNewObjectifiedFactType)
        End Sub

        Public Sub ClearModelErrors() Implements iValidationErrorHandler.ClearModelErrors
            Me.ModelError.Clear()
        End Sub

        Public Sub AddModelError(ByRef arModelError As ModelError) Implements iValidationErrorHandler.AddModelError

            Me.ModelError.Add(arModelError)
            RaiseEvent ModelErrorAdded(arModelError)

        End Sub

        ''' <summary>
        ''' Sets whether the corresponding RDS Table for the EntityType is absorbed into a supertype or not.
        ''' See also BubbleUpColumns
        ''' </summary>
        ''' <param name="abNewIsAbsorbed"></param>
        ''' <param name="abBroadcastInterfaceEvent"></param>
        Public Sub SetIsAbsorbed(ByVal abNewIsAbsorbed As Boolean, Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

            Dim lbWasAbsorbed = Me.IsAbsorbed

            Me.IsAbsorbed = abNewIsAbsorbed

            '==========================================================================================================
            'Reassign Columns if need be.
            If Not lbWasAbsorbed And abNewIsAbsorbed = True Then
                'Absorbed
                'Need to move all the Columns up the subtype heirarchy.
                Dim lrSubtypeTable As RDS.Table = Me.getCorrespondingRDSTable
                Dim lrSupertypeTable = Me.GetTopmostNonAbsorbedSupertype.getCorrespondingRDSTable
                For Each lrColumn In lrSubtypeTable.Column.ToArray
                    If lrColumn.Role.JoinedORMObject IsNot Me And
                       lrColumn.Role.JoinedORMObject Is lrSupertypeTable Then
                        'Is already from a higher type. Just remove the Column
                        lrSubtypeTable.removeColumn(lrColumn,, False)

                    Else
                        'Move Origins of relevant Relations.
                        Dim larRelation = From Relation In lrColumn.OutgoingRelation
                                          Select Relation

                        For Each lrRelation In larRelation.ToArray
                            Call lrRelation.setOriginTable(lrSupertypeTable)
                        Next

                        'Move Destinations of relevant Relations.
                        larRelation = From Relation In lrColumn.IncomingRelation
                                      Select Relation

                        For Each lrRelation In larRelation.ToArray
                            Call lrRelation.setDestinationTable(lrSupertypeTable)
                        Next

                        'Move the Column
                        Dim lbModifyCMML As Boolean = lrSupertypeTable.Column.Find(Function(x) x.Id = lrColumn.Id) Is Nothing
                        Call lrColumn.setTable(lrSupertypeTable, lbModifyCMML) 'Do this first, otherwise can't set the table.
                        Call lrSubtypeTable.removeColumn(lrColumn,, False)
                        Call lrSupertypeTable.addColumn(lrColumn,, False)
                    End If
                Next

                Call Me.Model.RDS.removeTable(lrSubtypeTable)

            ElseIf lbWasAbsorbed And Not abNewIsAbsorbed Then
                'Not Absorbed
                Dim lrTable = Me.getCorrespondingRDSTable(True)

                Call lrTable.absorbSupertypeColumns()

                For Each lrSupertypeTable In lrTable.getSupertypeTables
                    Call lrSupertypeTable.RemoveColumnsFromTable(lrTable)
                    For Each lrSubtype In Me.getSubtypes
                        Dim lrSubtypeTable = New RDS.Table(Me.Model.RDS, lrSubtype.Id, lrSubtype)
                        Call lrSupertypeTable.RemoveColumnsFromTable(lrSubtypeTable)
                    Next
                Next
            End If
            '==========================================================================================================

            RaiseEvent IsAbsorbedChanged(abNewIsAbsorbed)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateEntityType, Me, Nothing)
            End If

            Me.isDirty = True
            Me.Model.MakeDirty(False, False)

        End Sub

        Public Sub SetIsDerived(ByVal abIsDerived As Boolean, ByVal abBroadcastInterfaceEvent As Boolean)

            Me.IsDerived = abIsDerived
            RaiseEvent IsDerivedChanged(abIsDerived)

            Me.isDirty = True
            Call Me.Model.MakeDirty(False, True)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
            End If

        End Sub

        Public Sub SetIsIndependent(abNewIsIndependent As Boolean, abBroadcastInterfaceEvent As Boolean) Implements iFBMIndependence.SetIsIndependent

            Try
                Me.IsIndependent = abNewIsIndependent

                RaiseEvent IsIndependentChanged(abNewIsIndependent)

                Me.isDirty = True
                Me.Model.MakeDirty(False, False)

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateEntityType, Me, Nothing)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub TriggerExpandReferenceScheme()
            RaiseEvent ExpandReferenceScheme()
        End Sub

        Private Sub TriggerChangingToFactType(ByRef arFactType As FBM.FactType)
            RaiseEvent ChangingToFactType(arFactType)
        End Sub

    End Class

End Namespace