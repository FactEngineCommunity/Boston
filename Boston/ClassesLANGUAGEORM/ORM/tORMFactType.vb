Imports System.ComponentModel
Imports System.Collections.Generic
Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class FactType
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.FactType)
        Implements ICloneable
        Implements iMDAObject
        Implements FBM.iValidationErrorHandler
        Implements FBM.iFBMIndependence

        <XmlAttribute()> _
        Public Shadows Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
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
        Public _IsObjectified As Boolean = False
        <XmlAttribute()> _
        <CategoryAttribute("Fact Type"), _
        DefaultValueAttribute(False),
        DescriptionAttribute("True if the Fact Type is 'Objectified', else False.")>
        Public Overrides Property IsObjectified() As Boolean
            Get
                Return _IsObjectified
            End Get
            Set(ByVal Value As Boolean)
                _IsObjectified = Value
            End Set
        End Property

        ''' <summary>
        ''' Only used for Simple Reference Schemes. Allows the FactType to be hidden.
        ''' To find out if a FactType is for a ReferenceScheme use FactType.IsUsedInReferenceScheme
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _IsPreferredReferenceMode As Boolean = False
        <XmlAttribute()> _
        Public Overridable Property IsPreferredReferenceMode() As Boolean
            Get
                Return Me._IsPreferredReferenceMode
            End Get
            Set(ByVal value As Boolean)
                Me._IsPreferredReferenceMode = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _IsLinkFactType As Boolean = False
        <XmlAttribute()> _
        Public Overridable Property IsLinkFactType() As Boolean
            Get
                Return Me._IsLinkFactType
            End Get
            Set(ByVal value As Boolean)
                Me._IsLinkFactType = value
            End Set
        End Property

        ''' <summary>
        ''' If the FactType is a LinkFactType, then is the Role of the ObjectifiedFactType that this FactType belongs to.
        '''   LinkFactTypes belong to ObjectifiedFactTypes, and each LinkFactType of an ObjectifiedFactType corresponds to a Role of that ObjectifiedFactType.
        ''' </summary>
        ''' <remarks></remarks>
        Public LinkFactTypeRole As FBM.Role

        <XmlAttribute()>
        Public IsSubtypeRelationshipFactType As Boolean = False

        ''' <summary>
        ''' True if the FactType is a relation between a Supertype and a ValueType such that values of the ValueType determine the state of Subtypes of the Supertype, else False.
        ''' E.g. if a Supertype, 'Person', has subtypes 'Child', 'Teenager', 'Adult', and an assciated ValueType 'AgeRange' (limited to 'Child', 'Teenager' and 'Adult'), then
        ''' if this FactType is the FactType linking the Supertype (EntityType) to the ValueType, this field is set to True, else False.
        ''' i.e. In 99.9999% of FactType cases this field will be false.
        ''' </summary>
        <XmlAttribute()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _IsSubtypeStateControlling As Boolean
        <XmlAttribute()>
        <CategoryAttribute("Model Object"),
        DefaultValueAttribute(False),
        DescriptionAttribute("True if the Fact Type governs the Subtype/State relationship of Subtype/Value Type value relationships.")>
        Public Property IsSubtypeStateControlling As Boolean
            Get
                Return _IsSubtypeStateControlling
            End Get
            Set(value As Boolean)
                _IsSubtypeStateControlling = value
            End Set
        End Property

        ''' <summary>
        ''' NB Only used for MDA FactTypes in the CMML. True if coordinates are stored for Facts allocated to a Page. Normally FactData coordinates are stored only. FactData/Values are not always unique on a Page. Facts are always unique on a Page.
        ''' </summary>
        Public StoreFactCoordinates As Boolean = False


        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _ShowFactTypeName As Boolean = False
        <XmlIgnore()> _
        <CategoryAttribute("Fact Type"), _
        DefaultValueAttribute(True), _
        DescriptionAttribute("Display the Fact Type Name.")> _
        Public Property ShowFactTypeName() As Boolean
            Get
                Return _ShowFactTypeName
            End Get

            Set(ByVal value As Boolean)
                _ShowFactTypeName = value
            End Set
        End Property


        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Arity As Integer = 0 'The number of roles in the fact type. Minimum is 1 and this is set when the Role is added to the FactType.
        <XmlAttribute()> _
        <CategoryAttribute("Fact Type"), _
             DefaultValueAttribute(GetType(Integer), "0"), _
             DescriptionAttribute("The Cardinality of the Fact Type."), _
             ReadOnlyAttribute(True),
             Browsable(False)>
        Public ReadOnly Overridable Property Arity() As Integer
            Get
                Return Me.RoleGroup.Count
            End Get
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _FactTypeOrientation As pcenumFactTypeOrientation = pcenumFactTypeOrientation.Horizontal
        <CategoryAttribute("Fact Type"), _
             DefaultValueAttribute(pcenumFactTypeOrientation.Horizontal), _
             DescriptionAttribute("The visible orientation of the Fact Type."), _
             Browsable(False)> _
        Public Property FactTypeOrientation() As pcenumFactTypeOrientation
            Get
                Return _FactTypeOrientation
            End Get
            Set(ByVal Value As pcenumFactTypeOrientation)
                _FactTypeOrientation = Value
            End Set
        End Property

        <XmlIgnore()> _
        Private _HighestInternalUniquenessConstraintLevel As Integer = 0
        Public ReadOnly Property HighestInternalUniquenessConstraintLevel() As Integer
            Get
                Return Me.InternalUniquenessConstraint.Count
            End Get
        End Property

        <XmlIgnore()> _
        Public InternalUniquenessConstraint As New List(Of FBM.RoleConstraint) 'RoleConstraints are serialised seperately

        <XmlIgnore()> _
        Public FactTypeReading As New List(Of FBM.FactTypeReading)

        ''' <summary>
        ''' The list of Roles within the FactType (forming a 'RoleGroup')    
        ''' </summary>
        ''' <remarks></remarks>
        Public RoleGroup As New List(Of FBM.Role)

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Fact As New List(Of FBM.Fact)
        Public Property Fact() As List(Of FBM.Fact)
            Get
                Return Me._Fact
            End Get
            Set(ByVal value As List(Of FBM.Fact))
                Me._Fact = value
                If Me.Model.Loaded Then Call Me.Model.MakeDirty(False, False)
                RaiseEvent Updated()
            End Set
        End Property

        ''' <summary>
        ''' The EntityType for the FactType if the FactType is objectified.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public ObjectifyingEntityType As FBM.EntityType

        <XmlIgnore()> _
        Public KLFunctionLabel As String = "" 'When generating a proof for a model under KL, is the unique pcenumKLFunction label assigned to the FactType

        <XmlAttribute()> _
        Public IsCoreFactType As Boolean = False

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _IsDerived As Boolean = False
        <XmlAttribute()>
        <CategoryAttribute("Fact Type"),
        DefaultValueAttribute(False),
        DescriptionAttribute("True if the Fact Type is derived.")>
        Public Property IsDerived As Boolean
            Get
                Return Me._IsDerived
            End Get
            Set(value As Boolean)
                Me._IsDerived = value
            End Set
        End Property

        ''' <summary>
        ''' Only set by the FactEngine FEQL Processor at query time, so that FBM objects are not coupled to the FactEngine.
        ''' </summary>
        Public DerivationType As FactEngine.pcenumFEQLDerivationType = FactEngine.Constants.pcenumFEQLDerivationType.None


        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _IsStored As Boolean = False
        <XmlIgnore()> _
        <CategoryAttribute("Derivation"), _
        Browsable(False), _
        DefaultValueAttribute(False), _
        DescriptionAttribute("True if the Fact Type is derived and is stored.")> _
        Public Property IsStored As Boolean
            Get
                Return Me._IsStored
            End Get
            Set(value As Boolean)
                Me._IsStored = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsIndependent As Boolean
        <XmlAttribute()> _
        <CategoryAttribute("Model Object"), _
        DefaultValueAttribute(False), _
        DescriptionAttribute("True if the Model Object is independent.")> _
        Public Property IsIndependent As Boolean Implements iFBMIndependence.IsIndependent
            Get
                Return Me._IsIndependent
            End Get
            Set(ByVal value As Boolean)
                Me._IsIndependent = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _DerivationText As String = ""
        <XmlAttribute()> _
        <CategoryAttribute("Derivation"), _
        Browsable(False), _
        DescriptionAttribute("The text for the derivation of the Fact Type when the Fact Type is derived."),
        Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Property DerivationText As String
            Get
                Return Me._DerivationText
            End Get
            Set(value As String)
                Me._DerivationText = value
            End Set
        End Property

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

        Public Shadows Event Updated()
        Public Event DerivationTextChanged(ByVal asDerivationText As String)
        Public Event FactTableUpdated()
        Public Event IUConstraintAdded(ByRef arFactType As FBM.FactType, ByRef arRoleConstraint As FBM.RoleConstraint)
        Public Event IUConstraintRemoved(ByRef arFactType As FBM.FactType, ByVal arRoleConstraint As FBM.RoleConstraint)
        'Because otherwise Copy will try and serialise the frmToolboxORMReadingEditor
        <NonSerialized()> _
        Public Event FactTypeReadingAdded(ByRef arFactTypeReading As FBM.FactTypeReading)
        Public Event FactTypeReadingModified(ByRef arFactTypeReading As FBM.FactTypeReading)
        'Because otherwise Copy will try and serialise the frmToolboxORMReadingEditor
        <NonSerialized()> _
        Public Event FactTypeReadingRemoved(ByRef arFactTypeReading As FBM.FactTypeReading)
        Public Event RoleAdded(ByRef arRole As FBM.Role)
        Public Event RoleRemoved(ByRef arRole As FBM.Role)
        Public Event FactRemoved(ByRef arFact As FBM.Fact)
        Public Event IsDerivedChanged(ByVal abIsDerived As Boolean)
        Public Event IsIndependentChanged(ByVal abNewIsIndependent As Boolean) Implements iFBMIndependence.IsIndependentChanged
        Public Event IsObjectifiedChanged(ByVal abNewIsObjectified As Boolean)
        Public Event IsStoredChanged(ByVal abIsStored As Boolean)
        Public Event IsSubtypeStateControllingChanged(ByVal abIsSubtypeStateControlling As Boolean)
        Public Event IsPreferredReferenceModeChanged(ByVal abNewIsPreferrdReferenceMode As Boolean)
        Public Event IsSubtypeRelationshipFactTypeChanged(ByVal abNewIsSubtypeRelationshipFactType As Boolean)
        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded
        Public Event Objectified() 'When the FactType is changed to an ObjectifiedFactType
        Public Event ObjectifyingEntityTypeChanged(ByRef arNewObjectifyingEntityType As FBM.EntityType)
        Public Event ObjectificationRemoved() 'When the objectification of the FactType is removed.
        Public Event RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean)
        Public Event ShowFactTypeNameChanged(ByVal abNewShowFactTypeName As Boolean)

        Public Sub New()
            '-------------------------------------------------------------
            'Default - Serialisation requires a parameterless constructor
            '-------------------------------------------------------------
            Me.ConceptType = pcenumConceptType.FactType

        End Sub

        Public Sub New(ByVal asFactTypeName As String, Optional ByVal abUseFactTypeNameAsId As Boolean = False)

            MyBase.ConceptType = pcenumConceptType.FactType
            If abUseFactTypeNameAsId Then
                Me.Id = Trim(asFactTypeName)
            Else
                Me.Id = System.Guid.NewGuid.ToString()
            End If

            If IsSomething(asFactTypeName) Then
                Me.Name = asFactTypeName
            End If

            Me.Symbol = Me.Id

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal asFactTypeName As String, ByVal abUseFactTypeNameAsId As Boolean)

            Me.Model = arModel
            Me.ConceptType = pcenumConceptType.FactType
            If abUseFactTypeNameAsId Then
                Me.Id = Trim(asFactTypeName)
            Else
                Me.Id = System.Guid.NewGuid.ToString()
            End If

            If IsSomething(asFactTypeName) Then
                Me.Name = asFactTypeName
            End If

            Me.Symbol = Me.Id

            Me.Concept = New FBM.Concept(Me.Symbol)

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal asFactTypeName As String, ByVal as_FactTypeId As String)

            Call Me.New()

            Me.Model = arModel
            Me.Id = as_FactTypeId
            Me.Name = asFactTypeName

            Me.Symbol = Me.Id

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.FactType) As Boolean Implements System.IEquatable(Of FBM.FactType).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Returns True if the FactType's set of Role referenced ModelElements matches the other FactType's set of Role reference ModelElements,
        '''   else return False.
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EqualsByModelElements(ByVal other As FBM.FactType) As Boolean

            Dim liModelElementCount As Integer = 0
            Dim lrRole As FBM.Role

            EqualsByModelElements = True

            For Each lrRole In Me.RoleGroup

                liModelElementCount = Me.RoleGroup.FindAll(Function(x) x.JoinedORMObject.Id = lrRole.JoinedORMObject.Id).Count

                If other.RoleGroup.FindAll(Function(x) x.JoinedORMObject.Id = lrRole.JoinedORMObject.Id).Count = 0 Then
                    EqualsByModelElements = False
                End If
            Next

        End Function

        Public Shadows Function EqualsByName(ByVal other As FBM.FactType) As Boolean

            If Trim(Me.Name) = Trim(other.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByNameLike(ByVal other As FBM.FactType) As Boolean

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

        Public Overloads Function Clone(ByRef arModel As FBM.Model, _
                                        Optional ByVal abAddToModel As Boolean = False, _
                                        Optional ByVal abIsMDAModelElement As Boolean = False) As Object

            Dim lrFactType As New FBM.FactType
            Dim lrRole As FBM.Role
            Dim lrFact As FBM.Fact
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lrRoleConstraint As FBM.RoleConstraint

            Try

                If arModel.FactType.Exists(AddressOf Me.Equals) Then
                    '---------------------------------------------------------------------------------------------------------------------
                    'The target FactType already exists in the target Model, so return the existing FactType (from the target Model)
                    '  20150127-There seems no logical reason to clone an FactType to a target Model if it already exists in the target
                    '  Model. This method is used when copying/pasting from one Model to a target Model, and (in general) the FactType
                    '  won't exist in the target Model. If it does, then that's the FactType that's needed.
                    '  NB Testing to see if the Signature of the FactType already exists in the target Model is already performed in the
                    '  Paste proceedure before dropping the FactType onto a target Page/Model. If there is/was any clashes, then the 
                    '  FactType being copied/pasted will have it's Id/Name/Symbol changed and will not be affected by this test to see
                    '  if the FactType already exists in the target Model.
                    '---------------------------------------------------------------------------------------------------------------------
                    lrFactType = arModel.FactType.Find(AddressOf Me.Equals)
                Else


                    With Me
                        lrFactType.Model = arModel
                        lrFactType.Symbol = .Symbol
                        lrFactType.Id = .Id
                        lrFactType.Name = .Name

                        lrFactType.FactTypeOrientation = .FactTypeOrientation
                        lrFactType.IsObjectified = .IsObjectified
                        lrFactType.IsPreferredReferenceMode = .IsPreferredReferenceMode
                        lrFactType.IsSubtypeRelationshipFactType = .IsSubtypeRelationshipFactType
                        lrFactType.ShowFactTypeName = .ShowFactTypeName
                        lrFactType.IsDerived = .IsDerived
                        lrFactType.IsStored = .IsStored
                        lrFactType.DerivationText = .DerivationText
                        lrFactType.IsIndependent = .IsIndependent
                        lrFactType.IsLinkFactType = .IsLinkFactType
                        lrFactType.isDirty = True

                        If lrFactType.IsLinkFactType Then
                            lrFactType.LinkFactTypeRole = .LinkFactTypeRole.Clone(arModel, True)
                        End If

                        If abIsMDAModelElement = False Then
                            lrFactType.IsMDAModelElement = .IsMDAModelElement
                        Else
                            lrFactType.IsMDAModelElement = abIsMDAModelElement
                        End If

                        If abAddToModel Then
                            arModel.AddFactType(lrFactType)
                        End If

                        If Me.IsObjectified Then
                            lrFactType.ObjectifyingEntityType = .ObjectifyingEntityType.Clone(arModel, abAddToModel, .ObjectifyingEntityType.IsMDAModelElement)
                        End If

                        For Each lrRole In .RoleGroup
                            Select Case lrRole.TypeOfJoin
                                Case Is = pcenumRoleJoinType.FactType
                                    If arModel.FactType.Exists(AddressOf lrRole.JoinedORMObject.Equals) Then
                                        '--------------------------------------------------------------------------
                                        'That's good, the JoinedORMObject (FactType) already exists in the Model.
                                        '--------------------------------------------------------------------------
                                    Else
                                        Dim lrJoinedFactType As New FBM.FactType
                                        lrJoinedFactType = lrRole.JoinedORMObject
                                        lrJoinedFactType = lrJoinedFactType.Clone(arModel, abAddToModel)
                                        arModel.AddFactType(lrJoinedFactType)
                                    End If
                            End Select

                            Dim lrClonedRole As New FBM.Role
                            lrClonedRole = lrRole.Clone(arModel)
                            lrClonedRole.FactType = lrFactType

                            lrFactType.RoleGroup.Add(lrClonedRole)

                            If Not arModel.Role.Exists(AddressOf lrClonedRole.Equals) Then
                                arModel.Role.Add(lrClonedRole)
                            End If
                        Next

                        For Each lrFactTypeReading In .FactTypeReading
                            lrFactType.FactTypeReading.Add(lrFactTypeReading.Clone(arModel, lrFactType, abAddToModel))
                        Next

                        For Each lrFact In .Fact
                            lrFactType.Fact.Add(lrFact.Clone(arModel, lrFactType))
                        Next

                        For Each lrRoleConstraint In Me.InternalUniquenessConstraint
                            lrFactType.InternalUniquenessConstraint.Add(lrRoleConstraint.Clone(arModel, abAddToModel))
                        Next

                    End With

                End If

                Return lrFactType

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: FBM.tFactType.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrFactType
            End Try

        End Function

        ''' <summary>
        ''' Creates a cloned list of the Facts in the FactType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function CloneFacts() As List(Of FBM.Fact)

            Dim lrFact As FBM.Fact
            Dim larFact As New List(Of FBM.Fact)

            For Each lrFact In Me.Fact
                larFact.Add(lrFact.Clone(Me, True))
            Next

            Return larFact

        End Function

        ''' <summary>
        ''' Used to 'Sort' Enumerated lists Of FBM.FactType
        ''' </summary>
        ''' <param name="ao_a"></param>
        ''' <param name="ao_b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CompareFactTypeNames(ByVal ao_a As FBM.FactType, ByVal ao_b As FBM.FactType) As Integer

            Return StrComp(ao_a.Name, ao_b.Name, CompareMethod.Text)

        End Function

        Public Shared Function CompareRolesJoiningFactTypesCount(ByVal aoA As FBM.FactType, ByVal aoB As FBM.FactType) As Integer

            Try

                Return aoA.GetCountRolesJoiningFactTypes - aoB.GetCountRolesJoiningFactTypes

                MsgBox(aoA.GetCountRolesJoiningFactTypes & vbCrLf & aoB.GetCountRolesJoiningFactTypes)

            Catch ex As Exception
                prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Shared Function CompareRoleJoinedObjectIds(ByVal aoA As FBM.Role, ByVal aoB As FBM.Role) As Integer

            Try

                Return StrComp(aoA.JoinedORMObject.Id, aoB.JoinedORMObject.Id, CompareMethod.Text)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function allRolesJoinSomething() As Boolean

            Return Me.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count = 0

        End Function

        Public Function atLeasOneRoleJoinsAValueType() As Boolean

            Try
                Return Me.RoleGroup.FindAll(Function(x) x.JoinedORMObject.ConceptType = pcenumConceptType.ValueType).Count > 0

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Public Function DoesAtLeastTwoRolesReferenceTheSameModelObject() As Boolean

            Dim lrRole As FBM.Role
            Dim lrModelObject As FBM.ModelObject
            Dim larModelObject As New List(Of FBM.ModelObject)

            DoesAtLeastTwoRolesReferenceTheSameModelObject = False

            For Each lrRole In Me.RoleGroup
                If Not larModelObject.Exists(AddressOf lrRole.JoinedORMObject.Equals) Then
                    larModelObject.Add(lrRole.JoinedORMObject)
                End If
            Next

            For Each lrModelObject In larModelObject
                Dim liMaxJoinedObjectCount = Aggregate Role In Me.RoleGroup _
                                             Where Role.JoinedORMObject.Id = lrModelObject.Id _
                                             Into Count()

                If liMaxJoinedObjectCount >= 2 Then
                    Return True
                    Exit For
                End If
            Next

        End Function

        Public Function DoesEachRoleReferenceTheSameModelObject() As Boolean

            Dim lrRole As FBM.Role
            Dim lrModelObject As New FBM.ModelObject
            Dim liCounter As Integer = 0

            For Each lrRole In Me.RoleGroup
                liCounter += 1
                If liCounter = 1 Then
                    lrModelObject = lrRole.JoinedORMObject
                Else
                    If Not (lrModelObject Is lrRole.JoinedORMObject) Then
                        Return False
                    End If
                End If
            Next

            Return True

        End Function

        ''' <summary>
        ''' RETURNS TRUE if a Fact Type Reading exists for the Fact Type, and where that Fact Type Reading has the same Roles
        '''   in the same sequence as the supplied Fact Type Reading;
        ''' ELSE RETURNS FALSE
        ''' </summary>
        ''' <param name="arFactTypeReading">The Fact Type Reading against which a match within the Fact Type will be searched for (by Role/Sequence).</param>
        ''' <returns>TRUE if a Fact Type Reading exists for the Fact Type, and where that Fact Type Reading has the same Roles
        '''   in the same sequence as the supplied Fact Type Reading;
        ''' ELSE RETURNS FALSE
        ''' </returns>
        ''' <remarks></remarks>
        Public Function ExistsFactTypeReadingByRoleSequence(ByVal arFactTypeReading As FBM.FactTypeReading) As Boolean

            If Me.FactTypeReading.Find(AddressOf arFactTypeReading.EqualsByRoleSequence) Is Nothing Then
                Return False
            Else
                Return True
            End If

        End Function

        ''' <summary>
        ''' Returns the existing TypedPredicateId for the given Role sequence.
        ''' PRECONDITION: A FactTypeReading already exists for the Role sequence and as within the arFactTypeReading argument.
        ''' </summary>
        ''' <param name="arFactTypeReading"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTypedPredicateIdByRoleSequence(ByVal arFactTypeReading As FBM.FactTypeReading) As String

            Try

                If Me.FactTypeReading.Find(AddressOf arFactTypeReading.EqualsByRoleSequence) Is Nothing Then
                    Throw New Exception("No Fact Type Reading exists with the passed Role sequence.")
                Else
                    Return Me.FactTypeReading.Find(AddressOf arFactTypeReading.EqualsByRoleSequence).TypedPredicateId
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return System.Guid.NewGuid.ToString
            End Try

        End Function

        ''' <summary>
        ''' Returns TRUE if a Predicate Part exists for a Fact Type Reading and for the specified Role.
        ''' </summary>
        ''' <param name="arFactTypeReading">The Fact Type Reading to be checked.</param>
        ''' <param name="asRoleId">The Id of the Role within the Fact Type Reading to be checked, to be checked.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExistsPredicatePart(ByVal arFactTypeReading As FBM.FactTypeReading, ByVal asRoleId As String) As Boolean

            Dim lrPredicatePartCount As Integer

            lrPredicatePartCount = From FactTypeReading In Me.FactTypeReading _
                                   From PredicatePart In FactTypeReading.PredicatePart _
                                   Where FactTypeReading.Id = arFactTypeReading.Id _
                                   And PredicatePart.RoleId = asRoleId _
                                   Select PredicatePart Distinct.Count

            Return lrPredicatePartCount > 0

        End Function

        Public Function ExistsRoleNameForEveryRole() As Boolean

            Dim lrRole As FBM.Role

            For Each lrRole In Me.RoleGroup
                If Trim(lrRole.Name) = "" Then
                    Return False
                End If
            Next

            Return True

        End Function


        Public Function ShallowCopy() As Object

            Return Me.MemberwiseClone

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arPage">The Page for which the FactType is to be cloned to a FactTypeInstance</param>
        ''' <param name="abAddToPage">True if the FactTypeInstance is to be added to the Page, else False</param>
        ''' <returns></returns>
        Public Overrides Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.ModelObject

            Dim lrFactTypeInstance As New FBM.FactTypeInstance

            Try

                With Me
                    lrFactTypeInstance.Model = arPage.Model
                    lrFactTypeInstance.Page = arPage
                    lrFactTypeInstance.Id = .Id
                    lrFactTypeInstance.GUID = .GUID
                    lrFactTypeInstance.Name = .Name
                    lrFactTypeInstance.Symbol = .Symbol
                    lrFactTypeInstance.ConceptType = .ConceptType
                    lrFactTypeInstance.FactType = Me
                    lrFactTypeInstance.ShortDescription = .ShortDescription
                    lrFactTypeInstance.LongDescription = .LongDescription
                    lrFactTypeInstance.IsIndependent = .IsIndependent
                    lrFactTypeInstance.IsObjectified = .IsObjectified
                    lrFactTypeInstance.isPreferredReferenceMode = .IsPreferredReferenceMode
                    lrFactTypeInstance.IsSubtypeRelationshipFactType = .IsSubtypeRelationshipFactType
                    lrFactTypeInstance.IsSubtypeStateControlling = .IsSubtypeStateControlling
                    lrFactTypeInstance.FactTypeName.Model = arPage.Model
                    lrFactTypeInstance.FactTypeName.Page = arPage
                    lrFactTypeInstance.FactTypeName.FactTypeInstance = New FBM.FactTypeInstance
                    lrFactTypeInstance.FactTypeName.FactTypeInstance = lrFactTypeInstance
                    lrFactTypeInstance.FactTable.Model = arPage.Model
                    lrFactTypeInstance.FactTable.Page = arPage
                    lrFactTypeInstance.FactTable.FactTypeInstance = New FBM.FactTypeInstance
                    lrFactTypeInstance.FactTable.FactTypeInstance = lrFactTypeInstance
                    lrFactTypeInstance.ShowFactTypeName = .ShowFactTypeName
                    If .IsObjectified Then
                        lrFactTypeInstance.ShowFactTypeName = True
                    End If

                    lrFactTypeInstance.IsDerived = .IsDerived
                    lrFactTypeInstance.IsStored = .IsStored
                    lrFactTypeInstance.DerivationText = .DerivationText
                    lrFactTypeInstance.IsLinkFactType = .IsLinkFactType
                    If .IsLinkFactType Then
                        lrFactTypeInstance.LinkFactTypeRole = arPage.RoleInstance.Find(Function(x) x.Id = .LinkFactTypeRole.Id)
                    End If

                    If .ObjectifyingEntityType IsNot Nothing Then
                        lrFactTypeInstance.ObjectifyingEntityType = arPage.EntityTypeInstance.Find(Function(x) x.Id = .ObjectifyingEntityType.Id)
                        If lrFactTypeInstance.ObjectifyingEntityType Is Nothing Then
                            lrFactTypeInstance.ObjectifyingEntityType = .ObjectifyingEntityType.CloneInstance(arPage, True)
                        End If
                        lrFactTypeInstance.ObjectifyingEntityType.IsObjectifyingEntityType = True
                    End If


                    If abAddToPage Then
                        If Not arPage.FactTypeInstance.Exists(AddressOf lrFactTypeInstance.Equals) Then
                            arPage.FactTypeInstance.AddUnique(lrFactTypeInstance)
                        End If
                    End If

                    Dim lrRole As FBM.Role
                    Dim lrRoleInstance As FBM.RoleInstance

                    '----------------------------------------------
                    'Create the RoleGroup for the FactTypeInstance
                    '----------------------------------------------
                    For Each lrRole In .RoleGroup
                        '----------------------------------------------------
                        'Initiate a new instances of the RoleInstance struct
                        '----------------------------------------------------
                        lrRoleInstance = New FBM.RoleInstance
                        lrRoleInstance = lrRole.CloneInstance(arPage, abAddToPage)
                        lrRoleInstance.FactType = lrFactTypeInstance
                        lrFactTypeInstance.RoleGroup.Add(lrRoleInstance)
                    Next

                    '----------------------------------------------------------------------------------------------------------------
                    'Clone InternalUniquenessConstraint type RoleConstraints that relate only to this FactType.
                    '  NB The reason for the restriction is because there may be RoleConstraints against Roles within the FactType
                    '  that do not relate to FactType (and Instances of the other ModelObjects to which the RoleConstraint relates,
                    '  othere FactType.Roles, may not be loaded on the Page).
                    '----------------------------------------------------------------------------------------------------------------
                    Dim lrRoleConstraint As FBM.RoleConstraint
                    Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance

                    '---------------------------------------------------------------------------------------------------
                    'PSEUDOCODE
                    ' * FOR EACH RoleConstraint allocated as an InternalUniquenessConstraint to the FactType
                    '   * Clone an Instance of the RoleConstraint
                    '   * FOR EACH Role within the FactType
                    '     * FOR EACH RoleConstraintRole within the Role
                    '       * IF the RoleConstraint of the RoleConstraintRole is an InternalUniquenessConstraint THEN
                    '           * Clone an Instance of the RoleConstraintRole
                    '           * Append the RoleConstrainInstance to the RoleInstance
                    '       * END IF
                    '   * LOOP
                    ' * LOOP
                    '---------------------------------------------------------------------------------------------------

                    For Each lrRoleConstraint In Me.InternalUniquenessConstraint

                        lrRoleConstraintInstance = lrRoleConstraint.CloneUniquenessConstraintInstance(arPage, abAddToPage)

                        lrFactTypeInstance.InternalUniquenessConstraint.Add(lrRoleConstraintInstance)
                    Next

                End With

                Return lrFactTypeInstance

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tFactType.CloneInstance"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Sub AddRole(ByRef arRole As FBM.Role, Optional ByRef abBroadcastInterfaceEvent As Boolean = True, Optional abDoRDSProcessing As Boolean = False)

            arRole.isDirty = True

            Me.RoleGroup.AddUnique(arRole)
            arRole.SequenceNr = Me.Arity

            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lrPredicatePart As FBM.PredicatePart

            For Each lrFactTypeReading In Me.FactTypeReading
                lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading, arRole)
                lrFactTypeReading.AddPredicatePart(lrPredicatePart)
            Next

            '-----------------------------------------------------------------------------
            'Add a new (but empty) data item to the FactData of each Fact in the FactType
            '-----------------------------------------------------------------------------
            Dim lrFact As FBM.Fact
            For Each lrFact In Me.Fact
                Dim lrFactData As FBM.FactData
                lrFactData = New FBM.FactData(arRole, New FBM.Concept("a"), lrFact)
                lrFact.Data.Add(lrFactData)
                arRole.Data.Add(lrFactData)

                Dim lsMessage As String = ""
                lsMessage = "tFactType.AddRole:"
                lsMessage &= vbCrLf & "Adding FactData"
                lsMessage &= vbCrLf & "Fact.Symbol: " & lrFact.Symbol
                lsMessage &= vbCrLf & "FactData.Fact.Id:" & lrFactData.Fact.Id
                lsMessage &= vbCrLf & "FactData.Role.Id:" & lrFactData.Role.Id
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information)
            Next

            If Me.IsObjectified Then
                Call Me.createLinkFactTypeForRole(arRole)
            End If

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelAddRoleToFactType, arRole, Nothing)
            End If

            Me.Model.Role.AddUnique(arRole)

            '================================================================================================================
            'RDS
            If abDoRDSProcessing Then

                '20180806-Not sure why this is there. A new Role is unlikely to have an InternalUniquenessConstraint.
                'If Me.InternalUniquenessConstraint.Count > 0 Then

                Dim larUpstreamResponsibleColumns As New List(Of RDS.Column)

                If Me.isRDSTable Then
                    For Each lrRole In Me.RoleGroup
                        For Each lrColumn In lrRole.getResponsibleColumns
                            If Not Me.RoleGroup.Contains(lrColumn.Role) Then
                                larUpstreamResponsibleColumns.AddUnique(lrColumn)
                            End If
                        Next
                    Next
                End If

                If larUpstreamResponsibleColumns.Count > 0 Then ' Or arRole.FactType.isRDSTable Then

                    If larUpstreamResponsibleColumns.Count > 0 Then

                        Dim lrColumn As RDS.Column
                        Dim larTable = From Column In larUpstreamResponsibleColumns _
                                       Select Column.Table Distinct

                        For Each lrTable In larTable

                            lrColumn = lrTable.Column.Find(Function(x) larUpstreamResponsibleColumns.Contains(x))

                            For Each lrNewColumn In arRole.getColumns(lrTable, lrColumn.Role)

                                lrNewColumn.Name = lrColumn.Table.createUniqueColumnName(lrNewColumn, lrNewColumn.Name, 0)
                                Call lrColumn.Table.addColumn(lrNewColumn)
                            Next

                        Next

                    End If 'Role is Nested Active Role

                End If

                If arRole.FactType.isRDSTable Then

                    'Column/s
                    Dim lrResponsibleRole As FBM.Role = arRole

                    Dim lrTable As RDS.Table = arRole.FactType.getCorrespondingRDSTable()

                    For Each lrNewColumn In arRole.getColumns(lrTable, lrResponsibleRole)
                        lrNewColumn.Name = lrNewColumn.Table.createUniqueColumnName(lrNewColumn, lrNewColumn.Name, 0)
                        lrNewColumn.Table.addColumn(lrNewColumn)
                    Next

                    Dim lrRole As FBM.Role = arRole

                    'Relation  
                    If arRole.FactType.IsObjectified Then
                        Dim larLinkFactTypeRole = From FactType In Me.Model.FactType _
                                                 Where FactType.IsLinkFactType = True _
                                                 And FactType.LinkFactTypeRole Is lrRole _
                                                 Select FactType.RoleGroup(0)

                        For Each lrLinkFactTypeRole In larLinkFactTypeRole
                            Call Me.Model.generateRelationForManyTo1BinaryFactType(lrLinkFactTypeRole)
                        Next
                    Else
                        Select Case arRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.EntityType, pcenumConceptType.FactType
                                Call Me.Model.generateRelationForManyToManyFactTypeRole(lrRole)
                        End Select

                    End If

                End If

            End If 'abDoRDSProcessing

            RaiseEvent RoleAdded(arRole)

        End Sub


        Public Sub AddFact(ByVal arFact As FBM.Fact)

            Dim lrFactData As FBM.FactData

            Try
                '-------------------------------------------------------------
                'Make sure the Fact is in the ModelDictionary for the Model.
                '-------------------------------------------------------------
                Call Me.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me.Model, arFact.Id, pcenumConceptType.Fact), False)

                Me.Fact.Add(arFact)

                For Each lrFactData In arFact.Data
                    '----------------------------------------------------------------------------
                    'Make sure the FactData.Data Value is in the ModelDictionary for the Model.
                    '----------------------------------------------------------------------------
                    Call Me.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me.Model, lrFactData.Data, pcenumConceptType.Value), False)
                Next

                Dim lrRole As FBM.Role
                For Each lrRole In Me.RoleGroup
                    lrRole.Data.Add(arFact.GetFactDataByRoleId(lrRole.Id))
                Next

                '--------------------------------------------------------------------------------------------------------
                'If the FactType is objectified, add the Fact.Id to the Instances of the FactType.ObjectifyingEntityType
                '--------------------------------------------------------------------------------------------------------
                If Me.IsObjectified Then
                    '------------------------------------------------
                    'Code Safe: Instance data is not that important
                    '------------------------------------------------
                    If IsSomething(Me.ObjectifyingEntityType) Then
                        Me.ObjectifyingEntityType.Instance.Add(arFact.Id)
                    End If
                End If

                RaiseEvent FactTableUpdated()


                Me.Model.MakeDirty(False, False)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: FBM.FactType.AddFact"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub AddFactTypeReading(ByRef arFactTypeReading As FBM.FactTypeReading,
                                      ByVal abMakeModelDirty As Boolean,
                                      ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                arFactTypeReading.isDirty = True
                Me.FactTypeReading.Add(arFactTypeReading)

                If abMakeModelDirty Then
                    Me.Model.MakeDirty()
                End If

                '==================================================================================================
                'RDS 
                If Me.IsManyTo1BinaryFactType Then

                    Dim larRole = From Role In Me.RoleGroup
                                  Where Role.HasInternalUniquenessConstraint
                                  Select Role

                    Dim lrRole As FBM.Role = larRole(0)

                    Dim larRelation = From Relation In Me.Model.RDS.Relation
                                      Where Relation.ResponsibleFactType.Id = Me.Id
                                      Select Relation

                    If larRelation.Count > 0 Then

                        Dim lrRelation As RDS.Relation = larRelation.First

                        If arFactTypeReading.PredicatePart(0).Role.Id = lrRole.Id Then
                            Dim lsDestinationPredicate = arFactTypeReading.PredicatePart(0).PredicatePartText & " " & arFactTypeReading.PredicatePart(1).PreBoundText
                            Call lrRelation.setDestinationPredicate(lsDestinationPredicate)
                        Else
                            Dim lsOriginPredicate = arFactTypeReading.PredicatePart(0).PredicatePartText & " " & arFactTypeReading.PredicatePart(1).PreBoundText
                            Call lrRelation.setOriginPredicate(lsOriginPredicate)
                        End If

                    End If

                ElseIf Me.IsUnaryFactType Then

                    Dim larColumn = From Table In Me.Model.RDS.Table
                                    From Column In Table.Column
                                    Where Column.FactType Is Me
                                    Select Column

                    If larColumn.Count > 0 Then

                        Dim lrColumn = larColumn.First

                        Call lrColumn.setName(Viev.Strings.MakeCapCamelCase(arFactTypeReading.PredicatePart(0).PredicatePartText, True))
                    End If

                End If

                RaiseEvent FactTypeReadingAdded(arFactTypeReading)

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelAddFactTypeReading,
                                                                        arFactTypeReading,
                                                                        Nothing)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function hasAssociatedFactTypes() As Boolean

            Dim liFactTypeInstanceCount = Aggregate FactType In Me.Model.FactType
                                               From Role In FactType.RoleGroup
                                              Where Role.JoinedORMObject.Id = Me.Id
                                              Where FactType.IsLinkFactType = False
                                               Into Count()

            Return liFactTypeInstanceCount > 0

        End Function

        Public Function MakeNameFromFactTypeReadings() As String

            Try
                Dim lsName As String = ""
                Dim lrFactTypeReading As FBM.FactTypeReading

                '--------------------------------------------------------------------
                'CodeSafe: Make sure that all the Roles in the FactType reference a ModelElement
                Dim larRolesReferencingNothing = From R In Me.RoleGroup
                                                 Where R.JoinedORMObject Is Nothing
                                                 Select R

                If larRolesReferencingNothing.Count > 0 Then
                    Return Me.Model.CreateUniqueFactTypeName("NewFactType", 0, True)
                End If

                Select Case Me.FactTypeReading.Count
                    Case Is = 0
                        For Each lrRole In Me.RoleGroup
                            lsName &= lrRole.JoinedORMObject.Id
                        Next
                    Case Is = 1
                        lrFactTypeReading = Me.FactTypeReading(0)
                        lsName = Viev.Strings.MakeCapCamelCase(lrFactTypeReading.GetReadingText)
                        lsName = Viev.Strings.RemoveWhiteSpace(lsName)
                        lsName = lsName.Replace("-", "")

                    Case Else

                        lrFactTypeReading = Me.FactTypeReading.Find(Function(x) x.IsPreferred = True)
                        If lrFactTypeReading Is Nothing Then lrFactTypeReading = Me.FactTypeReading(0)

                        lsName = Viev.Strings.MakeCapCamelCase(lrFactTypeReading.GetReadingText)
                        lsName = Viev.Strings.RemoveWhiteSpace(lsName)
                        lsName = lsName.Replace("-", "")

                End Select

                If lsName = Me.Id Then
                    Return Me.Id
                Else
                    Return Me.Model.CreateUniqueFactTypeName(lsName, 0, True)
                End If


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Me.Id
            End Try

        End Function


        Public Function PermutateRoleGroupsRoles() As List(Of List(Of Object))

            Dim laoElements As New List(Of Object)
            Dim laoOrder As New List(Of Object)
            Dim laoOrders As New List(Of List(Of Object))

            Dim lrRole As FBM.Role

            For Each lrRole In Me.RoleGroup
                laoElements.Add(lrRole.Id)
            Next

            Call publicPermutations.Permutate(1, laoElements, laoOrder, laoOrders)

            Return laoOrders

        End Function

        Public Sub RemoveFactByData(ByVal arFact As FBM.Fact, ByVal ab_delete_all As Boolean)

            '---------------------------------------------------------------------------------------
            'PSEUDOCODE
            '  * If the FactInstance exists in the list of FactInstances for the FactTypeInstance
            '    * For each FactData of the FactInstance
            '      * If the Value is not used by any other Fact in the Model
            '        * Remove the Value from the ModelDictionary
            '      * EndIf 
            '    * Loop
            '    * Remove the Fact from the ModelDictionary
            '    
            '  * End If
            '---------------------------------------------------------------------------------------

            Try
                Dim lrFactList As New List(Of FBM.Fact)
                Dim lrFact As FBM.Fact
                Dim lrReferencingFact As FBM.Fact
                Dim lrDictionaryEntry As FBM.DictionaryEntry

                If ab_delete_all Then
                    lrFactList = Me.Fact.FindAll(AddressOf arFact.Equals)

                    If lrFactList.Count > 0 Then
                        For Each lrFact In lrFactList.ToArray

                            Me.Fact.Remove(lrFact)

                            For Each lrFactData In lrFact.Data
                                lrDictionaryEntry = New FBM.DictionaryEntry(Me.Model, lrFactData.Data, pcenumConceptType.Value)
                                Me.Model.DeprecateRealisationsForDictionaryEntry(lrDictionaryEntry, pcenumConceptType.Value)
                            Next

                            If IsSomething(Me.ObjectifyingEntityType) Then
                                Me.ObjectifyingEntityType.Instance.Remove(lrFact.Id)
                            End If

                            RaiseEvent FactRemoved(lrFact)

                            Call lrFact.RemoveFromModel(ab_delete_all) 'Permanently deletes the Fact from the database.

                            '--------------------------------------------------------
                            'Cascading delete of Facts referencing the Fact removed
                            '--------------------------------------------------------
                            Dim larFactType As New List(Of FBM.FactType)
                            Dim lrFactType As FBM.FactType
                            Dim larFact As New List(Of FBM.Fact)
                            larFactType = Me.Model.FactType.FindAll(Function(x) x.RoleGroup.FindAll(Function(y) y.JoinedORMObject.Id = Me.Id).Count > 0)

                            For Each lrFactType In larFactType
                                larFact = lrFactType.Fact.FindAll(Function(x) x.Data.FindAll(Function(y) y.Data = lrFact.Id).Count > 0)
                                For Each lrReferencingFact In larFact
                                    lrFactType.RemoveFactById(lrReferencingFact)
                                Next
                            Next

                        Next
                        Me.Model.MakeDirty(False, False)
                        'RaiseEvent Updated()
                    End If
                Else
                    lrFact = Me.Fact.Find(AddressOf arFact.Equals)

                    If IsSomething(lrFact) Then
                        Me.Fact.Remove(lrFact)
                        If IsSomething(Me.ObjectifyingEntityType) Then
                            Me.ObjectifyingEntityType.Instance.Remove(lrFact.Id)
                        End If
                        Call lrFact.RemoveFromModel() 'Permanently deletes the Fact from the database.

                        For Each lrFactData In lrFact.Data
                            lrDictionaryEntry = New FBM.DictionaryEntry(Me.Model, lrFactData.Data, pcenumConceptType.Value)
                            Me.Model.DeprecateRealisationsForDictionaryEntry(lrDictionaryEntry, pcenumConceptType.Value)
                        Next

                        '--------------------------------------------------------
                        'Cascading delete of Facts referencing the Fact removed
                        '--------------------------------------------------------
                        Dim larFactType As New List(Of FBM.FactType)
                        Dim lrFactType As FBM.FactType
                        Dim larFact As New List(Of FBM.Fact)
                        larFactType = Me.Model.FactType.FindAll(Function(x) x.RoleGroup.FindAll(Function(y) y.JoinedORMObject.Id = Me.Id).Count > 0)

                        For Each lrFactType In larFactType
                            larFact = lrFactType.Fact.FindAll(Function(x) x.Data.FindAll(Function(y) y.Data = lrFact.Id).Count > 0)
                            For Each lrFact In larFact
                                lrFactType.RemoveFactById(lrFact)
                            Next
                        Next

                        Me.Model.MakeDirty()
                        'RaiseEvent Updated()
                        'RaiseEvent FactRemoved(lrFact)
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

        Public Sub RemoveFactById(ByVal arFact As FBM.Fact)

            Dim lrFact As FBM.Fact
            Dim lrDictionaryEntry As FBM.DictionaryEntry

            lrFact = Me.Fact.Find(AddressOf arFact.EqualsById)

            If IsSomething(lrFact) Then

                Me.Fact.Remove(lrFact)

                If IsSomething(Me.ObjectifyingEntityType) Then
                    Me.ObjectifyingEntityType.Instance.Remove(lrFact.Id)
                End If

                Call lrFact.RemoveFromModel() 'Permanently deletes the Fact from the database.

                For Each lrFactData In lrFact.Data
                    lrDictionaryEntry = New FBM.DictionaryEntry(Me.Model, lrFactData.Data, pcenumConceptType.Value)
                    Me.Model.DeprecateRealisationsForDictionaryEntry(lrDictionaryEntry, pcenumConceptType.Value)
                Next

                Me.Model.MakeDirty(False, False)

                RaiseEvent Updated()
                RaiseEvent FactRemoved(lrFact)

            End If

            '--------------------------------------------------------
            'Cascading delete of Facts referencing the Fact removed
            '--------------------------------------------------------
            Dim larFactType As New List(Of FBM.FactType)
            Dim lrFactType As FBM.FactType
            Dim larFact As New List(Of FBM.Fact)
            larFactType = Me.Model.FactType.FindAll(Function(x) x.RoleGroup.FindAll(Function(y) y.JoinedORMObject.Id = Me.Id).Count > 0)

            For Each lrFactType In larFactType
                larFact = lrFactType.Fact.FindAll(Function(x) x.Data.FindAll(Function(y) y.Data = arFact.Id).Count > 0)
                For Each lrFact In larFact
                    lrFactType.RemoveFactById(lrFact)
                Next
            Next

        End Sub


        Function FindFirstRoleByModelObject(ByRef arModelObject As FBM.ModelObject) As FBM.Role

            '-----------------------------
            'Initialise the Return value
            '-----------------------------
            FindFirstRoleByModelObject = Nothing

            Dim lrRole As FBM.Role

            For Each lrRole In Me.RoleGroup
                If lrRole.JoinedORMObject.Id = arModelObject.Id Then
                    FindFirstRoleByModelObject = lrRole
                End If
            Next



        End Function

        Public Function FindSuitableFactTypeReading() As FBM.FactTypeReading
            '-----------------------------------------------------------------------------------
            'This is a beautiful little function. It finds a FactTypeReading within the FactType
            '  based on the order of the ORMObjectTypes withn the argument list.
            '
            '  * The reason for this function is that when a user moves an ObjectType associated
            '  with the FactType(Instance) within a Page, the RoleGroup within the 
            '  FactType(Instance) is sorted. When this happens, the FactTypeReading (as it appears)
            '  may no longer match the RoleGroup. This function looks up the FactTypeReadings and
            '  tries to find a FactTypeReading with the ORMObjectTypes in the same sequential order
            '  as the argument list.
            '  e.g. If a FactType is for three Entity Types and is in the order Part, Bin, Warehouse
            '  this function will look for a FactTypeReading with the same sequential order.
            '  (e.g. 'A Part is in a Bin in a Warehouse'.
            '-----------------------------------------------------------------------------------
            Try
                If IsSomething(Me.FactTypeReading) Then
                    Dim lrFactTypeReading As New FBM.FactTypeReading(Me, Me.Id)
                    FindSuitableFactTypeReading = Me.FactTypeReading.Find(AddressOf lrFactTypeReading.MatchesByFactTypesRoles)

                    If (FindSuitableFactTypeReading Is Nothing) And _
                       (Me.Arity = 2) And _
                       (Me.FactTypeReading.Count > 0) Then
                        FindSuitableFactTypeReading = Me.FactTypeReading(0)
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

        Public Function FindSuitableFactTypeReadingByRoles(ByVal aarRole As List(Of FBM.Role), Optional abBeStrict As Boolean = False) As FBM.FactTypeReading
            '-----------------------------------------------------------------------------------
            'This is a beautiful little function. It finds a FactTypeReading within the FactType
            '  based on the order of the ORMObjectTypes withn the argument list.
            '
            '  * The reason for this function is that when a user moves an ObjectType associated
            '  with the FactType(Instance) within a Page, the RoleGroup within the 
            '  FactType(Instance) is sorted. When this happens, the FactTypeReading (as it appears)
            '  may no longer match the RoleGroup. This function looks up the FactTypeReadings and
            '  tries to find a FactTypeReading with the ORMObjectTypes in the same sequential order
            '  as the argument list.
            '  e.g. If a FactType is for three Entity Types and is in the order Part, Bin, Warehouse
            '  this function will look for a FactTypeReading with the same sequential order.
            '  (e.g. 'A Part is in a Bin in a Warehouse'.
            '-----------------------------------------------------------------------------------
            Try
                Dim lrFactTypeReading As New FBM.FactTypeReading
                Dim lrPredicatePart As FBM.PredicatePart
                Dim lrRole As FBM.Role

                For Each lrRole In aarRole.FindAll(Function(x) x.FactType.Id = Me.Id)
                    lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading)
                    lrPredicatePart.Role = lrRole
                    lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                Next

                If IsSomething(Me.FactTypeReading) Then
                    FindSuitableFactTypeReadingByRoles = Me.FactTypeReading.Find(AddressOf lrFactTypeReading.MatchesByRoles)

                    If (FindSuitableFactTypeReadingByRoles Is Nothing) And _
                       (Me.Arity = 2) And _
                       (Me.FactTypeReading.Count > 0) And _
                       Not abBeStrict Then
                        FindSuitableFactTypeReadingByRoles = Me.FactTypeReading(0)
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

        ''' <summary>
        ''' Creates a Role an adds it to the FactType.
        ''' </summary>
        ''' <param name="aoJoinedObject">The ModelElement to which the new Role relates.</param>
        ''' <param name="abBroadcastInterfaceEvent">Client/Server: True if the method is required to broadcast the addition of the Role to the FactType.</param>        
        ''' <returns>FBM.Role</returns>
        ''' <remarks></remarks>
        Public Overridable Function CreateRole(Optional ByRef aoJoinedObject As FBM.ModelObject = Nothing,
                                               Optional abBroadcastInterfaceEvent As Boolean = False,
                                               Optional abDoRDSProcessing As Boolean = False) As FBM.Role

            Dim lrRole As FBM.Role

            '-----------------------------------------------------
            'Create the new Role.
            '  NB Adds the Role to the RoleGroup of the FactType
            '-----------------------------------------------------
            lrRole = New FBM.Role(Me, aoJoinedObject)
            lrRole.makeDirty()

            '------------------------------
            'Add the Role to the ORMModel
            '------------------------------
            Me.Model.Role.AddUnique(lrRole)

            Me.AddRole(lrRole, abBroadcastInterfaceEvent, abDoRDSProcessing)

            CreateRole = lrRole

        End Function

        Public Function CreateUniqueRoleName(ByVal asRootRoleName As String, ByVal aiCounter As Integer) As String

            Dim lsTrialRoleName As String

            If aiCounter = 0 Then
                lsTrialRoleName = asRootRoleName
            Else
                lsTrialRoleName = asRootRoleName & CStr(aiCounter)
            End If

            CreateUniqueRoleName = lsTrialRoleName

            If Me.RoleGroup.Exists(Function(x) x.Name = lsTrialRoleName) Then
                CreateUniqueRoleName = Me.CreateUniqueRoleName(asRootRoleName, aiCounter + 1)
            Else
                Return lsTrialRoleName
            End If

        End Function

        ''' <summary>
        ''' Returns TRUE if not all of the RoleGroup Role order permutations are used for a FactType with more than on Role referencing the same ModelObject,
        '''   else returns FALSE.
        ''' NB Only to be used for FactTypes where more than on Role references the same ModelObject.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExistsAvailablePermutation(ByVal arInitialFactTypeReading As FBM.FactTypeReading) As Boolean

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            Try
                If arInitialFactTypeReading.FactType IsNot Me Then
                    lsMessage = "Function called for Fact Type that is not relevant to the Initial Fact Type Reading."
                    Throw New Exception(lsMessage)
                End If

                If Me.HasMoreThanOneRoleReferencingTheSameModelObject Then
                    Dim liInd As Integer = 0
                    Dim liInd2 As Integer = 0
                    Dim lrRole As New FBM.Role
                    Dim lsRoleId As String = ""
                    Dim laRoleOrders As List(Of List(Of Object))
                    Dim lrPredicatePart As FBM.PredicatePart
                    Dim lrFactTypeReading As New FBM.FactTypeReading(Me)

                    laRoleOrders = Me.PermutateRoleGroupsRoles

                    For liInd = 1 To laRoleOrders.Count
                        lrFactTypeReading = New FBM.FactTypeReading(Me)
                        lrFactTypeReading.RoleList = New List(Of FBM.Role)
                        liInd2 = 1
                        For Each lsRoleId In laRoleOrders(liInd - 1)
                            lrRole.JoinedORMObject = Me.RoleGroup.Find(Function(x) x.Id = lsRoleId).JoinedORMObject
                            lrFactTypeReading.RoleList.Add(lrRole)
                            lrPredicatePart = New FBM.PredicatePart(liInd2, lrRole, "")
                            lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                            liInd2 += 1
                        Next
                        'If arInitialFactTypeReading.EqualsByRoleJoinedModelObjectSequence(lrFactTypeReading) Then
                        If Me.FactTypeReading.FindAll(AddressOf lrFactTypeReading.EqualsByRoleSequence).Count = 0 Then
                            Return True
                        End If
                        'End If
                    Next

                    Return False
                Else
                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & "Function called for FactType with no two Roles referencing the same Model Object."

                    Throw New Exception(lsMessage)
                End If

            Catch ex As Exception
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function


        Public Function exists_InternalUniquenessConstraint_by_role_span(ByVal arRole_contraint As FBM.RoleConstraint) As Boolean

            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            Dim li_max_role_span As Integer = 0

            exists_InternalUniquenessConstraint_by_role_span = False

            '-------------------------------------------------------------------
            'First check by Role span count, it's quickest
            '  * IF no InternalUniquenessConstraint for the FactType 
            '      has the same number of Roles as arRoleConstraint THEN
            '      it is not possible that the arRoleConstraint
            '      exists within the FactType's InternalUniquenessConstraints
            '-------------------------------------------------------------------
            For Each lrRoleConstraint In Me.InternalUniquenessConstraint
                If lrRoleConstraint.RoleConstraintRole.Count > li_max_role_span Then
                    li_max_role_span = lrRoleConstraint.RoleConstraintRole.Count
                    If li_max_role_span = arRole_contraint.RoleConstraintRole.Count Then
                        Exit For 'Because have already matched the arRoleConstraint.RoleConstraintRole.count
                    End If
                End If
            Next

            If li_max_role_span < arRole_contraint.RoleConstraintRole.Count Then
                '---------------------------------------------------------------------
                'None of the InternalUniquenessConstraints span as many Roles as the
                '  one being checked, so return False
                '---------------------------------------------------------------------
                exists_InternalUniquenessConstraint_by_role_span = False
                Exit Function
            End If

            '-------------------------------------------------------------------------------------------
            'check each InternalUniquenessConstraint to see if it is the same as the one being checked
            '-------------------------------------------------------------------------------------------
            Dim li_match_count As Integer = 0
            For Each lrRoleConstraint In Me.InternalUniquenessConstraint
                If lrRoleConstraint.RoleConstraintRole.Count = arRole_contraint.RoleConstraintRole.Count Then
                    li_match_count = 0
                    For Each lrRoleConstraintRole In arRole_contraint.RoleConstraintRole
                        '--------------------------------------------------------------------------------------------------
                        'Set the Id the same, so that we are just checking the Roles.
                        '  i.e. Assume that it is the same RoleConstraint, then check to see if the assumption is correct
                        '  by checking to see if the Roles match.
                        '--------------------------------------------------------------------------------------------------
                        lrRoleConstraintRole.RoleConstraint.Id = lrRoleConstraint.Id
                        If lrRoleConstraint.RoleConstraintRole.Exists(AddressOf lrRoleConstraintRole.Equals) Then
                            li_match_count += 1
                        End If
                    Next
                    If li_match_count = arRole_contraint.RoleConstraintRole.Count Then
                        exists_InternalUniquenessConstraint_by_role_span = True
                        Exit Function
                    End If
                Else
                    '----------------------------------------------------------------------
                    'Don't bother checking, because the spanned Role count isn't the same
                    '----------------------------------------------------------------------
                End If
            Next

        End Function

        Public Sub AddInternalUniquenessConstraint(ByRef arRoleConstraint As FBM.RoleConstraint)

            Try
                Me.InternalUniquenessConstraint.Add(arRoleConstraint)

                RaiseEvent IUConstraintAdded(Me, arRoleConstraint)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function CountReferencesToModelObject(ByRef arModelObject As FBM.ModelObject) As Integer

            Try
                Dim lrModelObject As FBM.ModelObject

                lrModelObject = arModelObject

                Return Me.RoleGroup.FindAll(Function(x) x.JoinedORMObject.Id = lrModelObject.Id).Count

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return 0
            End Try

        End Function

        Public Function CreateInternalUniquenessConstraint(ByRef aarRole As List(Of FBM.Role),
                                                           Optional ByVal abIsPreferredIdentifier As Boolean = False,
                                                           Optional ByVal abMakeModelDirty As Boolean = True,
                                                           Optional ByVal abAddToModel As Boolean = True,
                                                           Optional ByVal abIsSubtypeRelationshipSubtypeRole As Boolean = False,
                                                           Optional ByRef arTopmostSupertypeModelObject As FBM.ModelObject = Nothing) As FBM.RoleConstraint

            Try
                '---------------------------------
                'Create the RoleConstraint object
                '---------------------------------
                Dim lrRoleConstraint As New FBM.RoleConstraint
                Dim liNextUICLevel As Integer = 0

                liNextUICLevel = Me.HighestInternalUniquenessConstraintLevel + 1

                lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                                                                 aarRole,
                                                                 pcenumRoleConstraintType.InternalUniquenessConstraint.ToString,
                                                                 liNextUICLevel,
                                                                 abMakeModelDirty,
                                                                 False)

                lrRoleConstraint.IsPreferredIdentifier = abIsPreferredIdentifier

                '------------------------------------------------------
                'Add the InternalUniquenessConstraint to the FactType
                '------------------------------------------------------                
                Me.AddInternalUniquenessConstraint(lrRoleConstraint)

                If abAddToModel Then
                    Call Me.Model.AddRoleConstraint(lrRoleConstraint, abMakeModelDirty, True, Nothing, abIsSubtypeRelationshipSubtypeRole, arTopmostSupertypeModelObject)
                End If

                Return lrRoleConstraint

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try


        End Function

        Public Sub CreateOneToOneInternalUniquenessConstraint(ByVal abBroadcastInterfaceEvent As Boolean)

            Dim larRole As New List(Of FBM.Role)

            Call Me.RemoveInternalUniquenessConstraints(abBroadcastInterfaceEvent)

            larRole.Add(Me.RoleGroup(0))
            Call Me.CreateInternalUniquenessConstraint(larRole)

            larRole.Clear()
            larRole.Add(Me.RoleGroup(1))

            Call Me.CreateInternalUniquenessConstraint(larRole)

        End Sub

        Public Function canCreateFactFor() As Boolean

            Try
                For Each lrRole In Me.RoleGroup
                    '-----------------------------------------------------------------------------------------
                    'If the Role references a FactType, the referenced FactType must have at least one Fact.
                    '-----------------------------------------------------------------------------------------
                    If lrRole.TypeOfJoin = pcenumRoleJoinType.FactType Then
                        '---------------------------------------------------------------
                        'New FactData must reference a Fact in the joined FactType
                        '---------------------------------------------------------------
                        Dim lrJoinedFactType As New FBM.FactType
                        lrJoinedFactType = lrRole.JoinedORMObject
                        If lrJoinedFactType.Fact.Count = 0 Then
                            Return False
                        End If
                    End If
                Next 'Role

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean

            If Me.HasTotalRoleConstraint Then
                '-----------------------------------------------------------
                '20150616-VM-Needs to be refined
                '  Objectified FactTypes with Roles from other FactTypes joining to that ObjectifiedFactType
                '  cannot be removed safely.
                '-----------------------------------------------------------
                If Me.IsObjectified Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If

        End Function


        ''' <summary>
        ''' Changes the Model of the FactType to the target Model.
        ''' </summary>
        ''' <param name="arTargetModel">The Model to which the FactType will be associated on completion of this method.</param>
        ''' <remarks></remarks>
        Public Shadows Sub ChangeModel(ByRef arTargetModel As FBM.Model, ByVal abAddToModel As Boolean)

            Dim lrRole As FBM.Role

            Me.Model = arTargetModel

            If abAddToModel Then
                arTargetModel.AddFactType(Me)
            End If

            For Each lrRole In Me.RoleGroup
                lrRole.ChangeModel(arTargetModel, abAddToModel)
                Call lrRole.JoinedORMObject.changeModel(arTargetModel, abAddToModel)
            Next

            For Each lrFactTypeReading In Me.FactTypeReading
                Call lrFactTypeReading.ChangeModel(arTargetModel)
            Next

            For Each lrInternalUniquenessConstraint In Me.InternalUniquenessConstraint
                Call lrInternalUniquenessConstraint.ChangeModel(arTargetModel, abAddToModel)
            Next

            If Me.IsLinkFactType Then
                Me.LinkFactTypeRole.ChangeModel(arTargetModel, abAddToModel)
            End If

        End Sub

        Public Sub createLinkFactTypeForRole(ByRef arRole As FBM.Role)

            Dim larRole As New List(Of FBM.Role)
            Dim larModelObject As New List(Of FBM.ModelObject)
            Dim lrFactType As FBM.FactType
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lasPredicatePart As New List(Of String)

            Try
                larModelObject.Clear()
                larModelObject.Add(Me)
                larModelObject.Add(arRole.JoinedORMObject)

                lrFactType = Me.Model.CreateFactType(arRole.JoinedORMObject.Id & Me.Id,
                                                     larModelObject,
                                                     False,
                                                     True,
                                                     True,
                                                     arRole,
                                                     False)

                Call Me.Model.AddFactType(lrFactType, False, True)

                larRole.Clear()
                larRole.Add(lrFactType.RoleGroup(0))
                lrFactType.CreateInternalUniquenessConstraint(larRole, False, False)

                larRole.Clear()
                larRole.Add(lrFactType.RoleGroup(0))
                larRole.Add(lrFactType.RoleGroup(1))
                lasPredicatePart.Clear()
                lasPredicatePart.Add("involves")
                lasPredicatePart.Add("")
                lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lasPredicatePart)
                lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)

                larRole.Clear()
                larRole.Add(lrFactType.RoleGroup(1))
                larRole.Add(lrFactType.RoleGroup(0))
                lasPredicatePart.Clear()
                lasPredicatePart.Add("is involved in")
                lasPredicatePart.Add("")
                lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lasPredicatePart)
                lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        ''' <summary>
        ''' Creates the LinkFactTypes for the FactType.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreateLinkFactTypes(Optional ByVal abMakeModelDirty As Boolean = True)

            Dim lrRole As FBM.Role
            Dim larRole As New List(Of FBM.Role)
            Dim larModelObject As New List(Of FBM.ModelObject)
            Dim lrFactType As FBM.FactType
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lasPredicatePart As New List(Of String)

            Try
                For Each lrRole In Me.RoleGroup
                    larModelObject.Clear()
                    larModelObject.Add(Me)
                    larModelObject.Add(lrRole.JoinedORMObject)

                    lrFactType = Me.Model.CreateFactType(lrRole.JoinedORMObject.Id & Me.Id,
                                                         larModelObject,
                                                         False,
                                                         abMakeModelDirty,
                                                         True,
                                                         lrRole,
                                                         False)

                    Call Me.Model.AddFactType(lrFactType, False, True)

                    larRole.Clear()
                    larRole.Add(lrFactType.RoleGroup(0))
                    lrFactType.CreateInternalUniquenessConstraint(larRole, False, False)

                    larRole.Clear()
                    larRole.Add(lrFactType.RoleGroup(0))
                    larRole.Add(lrFactType.RoleGroup(1))
                    lasPredicatePart.Clear()
                    lasPredicatePart.Add("involves")
                    lasPredicatePart.Add("")
                    lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lasPredicatePart)
                    lrFactType.AddFactTypeReading(lrFactTypeReading, abMakeModelDirty, True)

                    larRole.Clear()
                    larRole.Add(lrFactType.RoleGroup(1))
                    larRole.Add(lrFactType.RoleGroup(0))
                    lasPredicatePart.Clear()
                    lasPredicatePart.Add("is involved in")
                    lasPredicatePart.Add("")
                    lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lasPredicatePart)
                    lrFactType.AddFactTypeReading(lrFactTypeReading, abMakeModelDirty, True)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub CreateManyToOneInternalUniquenessConstraint(ByVal arModelObject As FBM.ModelObject)

            Dim larRole As New List(Of FBM.Role)

            Call Me.RemoveInternalUniquenessConstraints(True)

            larRole.Add(Me.FindFirstRoleByModelObject(arModelObject))
            Call Me.CreateInternalUniquenessConstraint(larRole)

        End Sub

        Function IsSingleMandatory1To1FactType() As Boolean
            '----------------------------------------------
            'RETURNS TRUE if the FactType is a 1 to 1 binary role and has only one mandatory role
            'ELSE returns FALSE
            '----------------------------------------------

            IsSingleMandatory1To1FactType = False

            If Me.Is1To1BinaryFactType() Then
                If Me.GetMandatoryRoleCount() = 1 Then
                    IsSingleMandatory1To1FactType = True
                End If
            End If

        End Function

        Function IsUnaryFactType() As Integer

            '---------------------------------------------------
            'RETURNS true if the FactType is Unary
            'ELSE returns false
            '---------------------------------------------------

            '----------
            'Failsafe
            '----------
            IsUnaryFactType = False

            If Me.Arity = 1 Then
                IsUnaryFactType = True
            Else
                IsUnaryFactType = False
            End If

        End Function

        Public Function IsUsedInReferenceScheme() As Boolean

            Dim larRoleConstraint = (From RoleConstraint In Me.Model.RoleConstraint
                                     Where RoleConstraint.IsPreferredIdentifier = True
                                     From Role In RoleConstraint.Role
                                     Where Role.FactType.Id = Me.Id
                                     Select RoleConstraint).Count

            Return larRoleConstraint > 0

        End Function

        ''' <summary>
        ''' Used for getting the FactTypes joined to an ObjectifiedFactType.
        ''' </summary>
        ''' <returns></returns>
        Public Function JoinedFactTypes() As List(Of FBM.FactType)

            Try
                If Not Me.IsObjectified Then Return New List(Of FBM.FactType)

                Dim larFactType = From FT In Me.Model.FactType
                                  From Role In FT.RoleGroup
                                  Where Role.JoinedORMObject Is Me
                                  Select FT

                Dim larNotLinkFactType = From FT In larFactType
                                         Where FT.IsLinkFactType = False
                                         Select FT

                Return larNotLinkFactType.ToList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.FactType)
            End Try

        End Function

        ''' <summary>
        ''' Returns a list of Roles that join the FactType and where this FactType is Objectified. Roles are not of a LinkFactType
        ''' </summary>
        ''' <returns></returns>
        Public Function JoinedRoles() As List(Of FBM.Role)

            Try
                If Not Me.IsObjectified Then Return New List(Of FBM.Role)

                Dim larRole = From FT In Me.Model.FactType
                              From Role In FT.RoleGroup
                              Where Role.JoinedORMObject Is Me
                              Select Role

                Dim larNotLinkFactTypeRole = From Role In larRole
                                             Where Role.FactType.IsLinkFactType = False
                                             Select Role

                Return larNotLinkFactTypeRole.ToList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.Role)
            End Try

        End Function

        Public Sub GetFactsFromDatabase()

            Call TableFact.GetFactsForFactType(Me)

        End Sub

        Public Function GetFirstRoleWithInternalUniquenessConstraint() As FBM.Role

            Try

                If Me.InternalUniquenessConstraint.Count = 0 Then
                    Throw New Exception("Function called for Fact Type with no Internal Uniqueness Constraint.")
                End If

                Dim larRole = From RoleConstraint In Me.InternalUniquenessConstraint _
                              From RoleConstraintRole In RoleConstraint.RoleConstraintRole _
                              Select RoleConstraintRole.Role

                If larRole.Count = 0 Then
                    Return Nothing
                Else
                    Return larRole(0)
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


        Public Overridable Function GetHighestConstraintLevel() As Integer

            '------------------------------------------------------------------------
            'RETURNS the highest level of InternalUniquenessConstraint on a FactType
            '------------------------------------------------------------------------
            GetHighestConstraintLevel = Me.InternalUniquenessConstraint.Count

        End Function


        ''' <summary>
        ''' Finds and returns an available permutation for a FactType with more than one Role referencing the same ModelObject.
        ''' NB Only to be used for FactTypes with more than one Role referencing the same ModelObject.
        ''' </summary>
        ''' <param name="arInitialFactTypeReading">The initial FactTypeReading from which to find a similar FactTypeReading (similar in Role.JoinedORMObject sequence)</param>
        ''' <returns>returns an available permutation for a FactType with more than one Role referencing the same ModelObject.</returns>
        ''' <remarks></remarks>
        Public Function GetAvailableFTRPermutation(ByRef arInitialFactTypeReading As FBM.FactTypeReading) As FBM.FactTypeReading

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
            Dim larRoleList As New List(Of FBM.Role)

            Try
                If Not Me.ExistsAvailablePermutation(arInitialFactTypeReading) Then
                    lsMessage = "Function called where no availale Fact Type Reading permutation exists"
                    Throw New Exception(lsMessage)
                End If

                If Me.HasMoreThanOneRoleReferencingTheSameModelObject Then
                    Dim lrPredicatePart As FBM.PredicatePart
                    Dim lrRole As FBM.Role
                    Dim lsRoleId As String = ""
                    Dim laRoleOrders As List(Of List(Of Object))
                    Dim liInd As Integer = 0
                    Dim liInd2 As Integer = 0
                    Dim lrFactTypeReading As New FBM.FactTypeReading(Me)

                    laRoleOrders = Me.PermutateRoleGroupsRoles

                    For liInd = 1 To laRoleOrders.Count
                        lrFactTypeReading = New FBM.FactTypeReading(Me)
                        lrFactTypeReading.RoleList = New List(Of FBM.Role)
                        liInd2 = 0
                        For Each lsRoleId In laRoleOrders(liInd - 1)
                            lrRole = Me.RoleGroup.Find(Function(x) x.Id = lsRoleId)
                            lrFactTypeReading.RoleList.Add(lrRole)
                            lrPredicatePart = New FBM.PredicatePart(liInd2, lrRole, "")
                            lrPredicatePart.FactTypeReading = arInitialFactTypeReading
                            lrPredicatePart.Model = arInitialFactTypeReading.Model
                            lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                            liInd2 += 1
                        Next
                        'If arInitialFactTypeReading.EqualsByRoleJoinedModelObjectSequence(lrFactTypeReading) Then
                        If Me.FactTypeReading.FindAll(AddressOf lrFactTypeReading.EqualsByRoleSequence).Count = 0 Then
                            lrFactTypeReading.Id = arInitialFactTypeReading.Id
                            lrFactTypeReading.PredicatePart = arInitialFactTypeReading.PredicatePart
                            liInd2 = 0
                            For Each lrRole In lrFactTypeReading.RoleList
                                lrFactTypeReading.PredicatePart(liInd2).Role = Me.RoleGroup.Find(Function(x) x.Id = laRoleOrders(liInd - 1)(liInd2).ToString)
                                liInd2 += 1
                            Next
                            Return lrFactTypeReading
                        End If
                        'End If
                    Next

                    lsMessage = "Function called where no availale Fact Type Reading permutation exists"
                    Throw New Exception(lsMessage)
                Else
                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & "Function called for FactType with no two Roles referencing the same Model Object."

                    Throw New Exception(lsMessage)
                End If

            Catch ex As Exception
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' PRECONDITION: FactType must have a corresponding RDS Table. Used to save typing.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function getCorrespondingRDSTable(Optional ByVal arModelObject As FBM.ModelObject = Nothing) As RDS.Table

            Try
                Dim lrTable As RDS.Table

                If arModelObject IsNot Nothing Then
                    lrTable = Me.Model.RDS.Table.Find(Function(x) x.FBMModelElement.Id = arModelObject.Id)
                Else
                    lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = Me.Id)
                End If


                If lrTable Is Nothing Then
                    Throw New Exception("There is no corresponding table for FactType: '" & Me.Id & "'")
                Else
                    Return lrTable
                End If

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
        ''' Returns a count of the Roles within the RoleGroup of the FactType, where those Roles join a FactType.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Example of use: Sorting FactTypes.</remarks>
        Public Overridable Function GetCountRolesJoiningFactTypes() As Integer

            Dim liCount As Integer = 0
            Dim lrRole As FBM.Role

            For Each lrRole In Me.RoleGroup
                If lrRole.TypeOfJoin = pcenumRoleJoinType.FactType Then
                    liCount += 1
                    liCount += lrRole.JoinsFactType.GetCountRolesJoiningFactTypes
                End If
            Next

            Return liCount

        End Function

        ''' <summary>
        ''' Returns the set of Destination Model Objects on LinkFactTypes associated with the FactType.
        '''   Used to add the FactType's corresponding Table to a Page when all the ModelObjects are on the Page.
        '''   Used predominantly for PGS Pages, when both joined nodes of a binaryFactType are on the Page, such that the corresponding Relation/Edge link 
        '''   can be added to the Page.
        ''' </summary>
        ''' <returns></returns>
        Public Function getDesinationModelObjects() As List(Of FBM.ModelObject)

            Dim larModelObject As New List(Of FBM.ModelObject)
            For Each lrRole In Me.RoleGroup
                If lrRole.HasInternalUniquenessConstraint Or Me.Arity = 2 Then
                    larModelObject.Add(lrRole.JoinedORMObject)
                End If
            Next

            Return larModelObject

        End Function

        ''' <summary>
        ''' Returns the CQL for the EntityType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCQLText() As String

            Try
                Dim lsCQLStatement As String = ""
                Dim liInd As Integer

                liInd = 0
                For Each lrFactTypeReading In Me.FactTypeReading
                    If liInd > 0 Then lsCQLStatement.AppendString("," & vbCrLf & vbTab)
                    lsCQLStatement.AppendString(lrFactTypeReading.GetReadingTextCQL(True, True))
                    liInd += 1
                Next

                lsCQLStatement.AppendString(";")

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

        Public Function GetPreboundReadingTextForModelElementAtPosition(ByRef arModelObject As FBM.ModelObject,
                                                                        Optional ByVal aiPosition As Integer = 0) As String

            Dim lsMessage As String = ""
            Dim lsModelObjectId As String = ""
            Dim lsPreboundReadingText As String = ""
            Try
                lsModelObjectId = arModelObject.Id

                If aiPosition > Me.Arity Then
                    lsMessage = "There is no Model Element within the Fact Type at position:" & aiPosition
                    lsMessage.AppendString(vbCrLf)
                    lsMessage.AppendString("Arity of the Fact Type is: " & Me.Arity)
                    Throw New Exception(lsMessage)
                End If

                If Me.FactTypeReading.Count = 0 Then
                    lsMessage = "There is no Fact Type Readings for the Fact Type." & aiPosition
                    lsMessage.AppendString(vbCrLf)
                    lsMessage.AppendString("So it can't have a PreboundReadingText for a Model Element.")
                    Throw New Exception(lsMessage)
                End If

                If aiPosition = 0 Then
                    Dim larPredicatePart = From FactTypeReading In Me.FactTypeReading _
                                           From PredicatePart In FactTypeReading.PredicatePart _
                                           Where PredicatePart.Role.JoinedORMObject.Id = lsModelObjectId _
                                           Select PredicatePart

                    For Each lrPredicatePart In larPredicatePart
                        Return lrPredicatePart.PreBoundText
                    Next
                Else
                    For Each lrFactTypeReading In Me.FactTypeReading.FindAll(Function(x) x.RoleList(aiPosition - 1).JoinedORMObject.Id = lsModelObjectId)
                        lsPreboundReadingText = lrFactTypeReading.PredicatePart(aiPosition - 1).PreBoundText
                        arModelObject.PreboundReadingText = lsPreboundReadingText
                        Return lsPreboundReadingText
                    Next
                End If

                Return ""

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return ""
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arFactTypeReading"></param>
        ''' <param name="aiSequenceNr"></param>
        ''' <param name="asRoleId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPredicatePart(ByVal arFactTypeReading As FBM.FactTypeReading, ByVal aiSequenceNr As Integer, ByVal asRoleId As String) As FBM.PredicatePart

            Dim lrPredicatePart As FBM.PredicatePart

            Try
                Dim larPredicatePart = From FactTypeReading In Me.FactTypeReading
                                       From PredicatePart In FactTypeReading.PredicatePart
                                       Where FactTypeReading.Id = arFactTypeReading.Id _
                                      And PredicatePart.SequenceNr = aiSequenceNr _
                                      And PredicatePart.RoleId = asRoleId
                                       Select PredicatePart

                If IsSomething(larPredicatePart) Then
                    For Each lrPredicatePart In larPredicatePart
                        Return lrPredicatePart
                        Exit Function
                    Next
                Else
                    Throw New Exception("Couldn't find PredicatePart for FactType")
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            End Try

            Return Nothing

        End Function

        Public Function getPredicatePartsForModelObject(ByVal arModelObject As FBM.ModelObject,
                                                        Optional ByVal abGetAdjoiningFactTypePredicateParts As Boolean = False) As List(Of FBM.PredicatePart)

            Try

                Dim larReturnPredicatePart As New List(Of FBM.PredicatePart)

                Dim larPredicatePart = From FactTypeReading In Me.FactTypeReading
                                       From PredicatePart In FactTypeReading.PredicatePart
                                       Where PredicatePart.Role.JoinedORMObject.Id = arModelObject.Id
                                       Select PredicatePart

                If abGetAdjoiningFactTypePredicateParts Then

                    Dim larAdjoiningFactType = From FactType In Me.Model.FactType
                                               From Role In FactType.RoleGroup
                                               Where Role.JoinedORMObject.Id = Me.Id
                                               Select FactType

                    Dim larAdjoiningPredicateParts = From FactType In larAdjoiningFactType
                                                     From FactTypeReading In FactType.FactTypeReading
                                                     From PredicatePart In FactTypeReading.PredicatePart
                                                     Where PredicatePart.Role.JoinedORMObject Is Me
                                                     Select PredicatePart

                    larReturnPredicatePart.AddRange(larPredicatePart.ToList)
                    larReturnPredicatePart.AddRange(larAdjoiningPredicateParts.ToList)

                    Return larReturnPredicatePart
                End If

                larReturnPredicatePart.AddRange(larPredicatePart.ToList)
                Return larReturnPredicatePart

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function getPreferredInternalUniquenessConstraint() As FBM.RoleConstraint

            Try
                If Me.InternalUniquenessConstraint.Count = 0 Then
                    Throw New Exception("Function should only be called for a Fact Type with at least one InternalUniquenessConstraint.")

                ElseIf Me.InternalUniquenessConstraint.Count = 1 Then
                    Return Me.InternalUniquenessConstraint(0)

                ElseIf Me.InternalUniquenessConstraint.Find(Function(x) x.IsPreferredIdentifier = True) IsNot Nothing Then
                    Return Me.InternalUniquenessConstraint.Find(Function(x) x.IsPreferredIdentifier = True)

                Else
                    Return Me.InternalUniquenessConstraint.Find(Function(x) x.LevelNr = 1)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function getPrimaryFactTypeReading() As FBM.FactTypeReading

            Try
                Dim larFactTypeReading = From FactTypeReading In Me.FactTypeReading
                                         Where FactTypeReading.IsPreferred
                                         Select FactTypeReading

                If larFactTypeReading.Count = 0 Then
                    Return Me.FactTypeReading(0)
                Else
                    Return larFactTypeReading.First
                End If


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
        ''' Returns the Role object within the RoleGroup of the FactType, given the RoleId of the Role.
        ''' </summary>
        ''' <param name="asRoleId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetRoleById(ByVal asRoleId As String) As FBM.Role

            Dim lrRole As FBM.Role

            Try
                lrRole = Me.RoleGroup.Find(Function(x) x.Id = asRoleId)

                If lrRole IsNot Nothing Then
                    Return lrRole
                Else
                    prApplication.ThrowErrorMessage("No Role exists for RoleId: " & "'" & asRoleId & "', for FactType with FactTypeId: '" & Me.Id & "', in FactType.GetRoleById", pcenumErrorType.Critical, Nothing, True)
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

        ''' <summary>
        ''' RETURNS the first Role that joins a MOdelObject with a matching asJoinedObjectTypeId,
        ''' OR if a SequenceNr is provided, the nth Role that has a matching asJoinedObjectTypeId
        ''' ELSE RETURNS Nothing
        ''' </summary>
        ''' <param name="aiSequenceNr"></param>
        ''' <param name="asJoinedObjectTypeId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetRoleByJoinedObjectTypeId(ByVal asJoinedObjectTypeId As String, Optional ByVal aiSequenceNr As Integer = 1) As FBM.Role

            Dim lrRole As FBM.Role

            If aiSequenceNr = 1 Then
                lrRole = Me.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = asJoinedObjectTypeId)
            Else
                lrRole = Me.RoleGroup.FindAll(Function(x) x.JoinedORMObject.Id = asJoinedObjectTypeId)(aiSequenceNr - 1)
            End If

            Return lrRole

        End Function

        Public Function getRoleNotCoveredByInternalUniquenessConstraint(ByRef arRoleConstraint As FBM.RoleConstraint) As FBM.Role

            Try
                For Each lrRole In Me.RoleGroup

                    If Not arRoleConstraint.Role.Contains(lrRole) Then
                        Return lrRole
                    End If
                Next

                Throw New Exception("Function must be called for a Fact Type with more Roles than the given RoleConstraint.")

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

        Public Function getRolesReferencingNothingCount() As Integer

            Dim larRolesReferencingNothing = From R In Me.RoleGroup
                                             Where R.JoinedORMObject Is Nothing
                                             Select R

            Return larRolesReferencingNothing.Count

        End Function

        ''' <summary>
        ''' Returns the unique Signature of the FactType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetSignature() As String

            Dim lsSignature As String
            Dim lrClonedFactType As New FBM.FactType
            Dim lrRole As FBM.Role


            lrClonedFactType = Me.Clone(Me.Model)

            lrClonedFactType.RoleGroup.Sort(AddressOf FBM.FactType.CompareRoleJoinedObjectIds)

            lsSignature = Viev.Strings.RemoveWhiteSpace(Me.Id)

            For Each lrRole In Me.RoleGroup
                lsSignature &= Viev.Strings.RemoveWhiteSpace(lrRole.JoinedORMObject.Id)
            Next

            Return lsSignature

        End Function

        Public Function HasAtLeastOneRolePointingToNothing() As Boolean

            Return (Me.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count > 0) _
                    Or Me.RoleGroup.Count = 0

        End Function

        Public Function hasLinkFactTypes() As Boolean

            Return Me.IsObjectified 'Must be true if the FactType is Objectified.

        End Function

        ''' <summary>
        ''' This function is used to determine of more than one Role of the FactType references the same ModelObject.
        ''' 
        ''' RETURNS TRUE if more than one Role of the FactType references the same ModelObject
        ''' ELSE RETURNS FALSE
        ''' </summary>
        ''' <returns>TRUE if more than one Role of the FactType references the same ModelObject
        ''' ELSE RETURNS FALSE</returns>
        ''' <remarks></remarks>
        Public Function HasMoreThanOneRoleReferencingTheSameModelObject() As Boolean

            If Me.Arity = 1 Then Return False

            Dim lrJoinedORMObject = From Role In Me.RoleGroup _
                                    Select Role.JoinedORMObject _
                                    Group By JoinedORMObject Into Group _
                                    Where Group.Count > 1

            If lrJoinedORMObject.Count = 0 Then
                Return False
            Else
                Return True
            End If

        End Function

        ''' <summary>
        ''' This Function is used to see if a RoleGroup has an internal uniqueness constraint 
        ''' spanning more than 1 role but not a total_RoleConstraint.
        ''' 
        ''' RETURNS TRUE if the FactType has a partial role constraint (but more than unary and less than total)
        ''' ELSE RETURNS FALSE
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasPartialButMultiRoleConstraint() As Boolean

            HasPartialButMultiRoleConstraint = False

            If Me.HasTotalRoleConstraint() Then
                HasPartialButMultiRoleConstraint = False
                Exit Function
            End If

            Dim lrRoleConstraint As FBM.RoleConstraint

            If Me.InternalUniquenessConstraint.Count > 0 Then
                For Each lrRoleConstraint In Me.InternalUniquenessConstraint
                    If (Me.Arity > 2) And (lrRoleConstraint.RoleConstraintRole.Count >= 2) Then
                        Return True
                    End If
                Next
            End If
            'For liInd = 1 To prApplication.workingpage.RoleConstraintInstance_count
            '    If (prApplication.workingpage.RoleConstraintInstance(liInd).constraint_type = 0) And (prApplication.workingmodel.getRole_by_id(prApplication.workingpage.RoleConstraintInstance(liInd).joins_RoleId).MasterRole.Id = Me.MasterRole.Id) Then
            '        'then is internal uniqueness constraint and in this RoleGroup
            '        If prApplication.workingpage.RoleConstraintInstance(liInd).get_constraint_arity > 1 Then
            '            HasPartialButMultiRoleConstraint = True
            '            Exit Function
            '        End If
            '    End If
            'Next liInd

        End Function


        ''' <summary>
        ''' TRUE if the FactType is binary and has an associated Transitive Ring Constraint, else FALSE
        ''' </summary>
        ''' <returns></returns>
        Public Function hasTransitiveRingConstraint() As Boolean

            Try
                '20201030-VM-May have 3 Roles, but use 2 for now.
                If Me.Arity <> 2 Then Return False

                Dim larBinaryRoleConstraint = From RoleConstraint In Me.Model.RoleConstraint
                                              Where RoleConstraint.RoleConstraintRole.Count = 2
                                              Select RoleConstraint

                Dim larTransitiveRingConstraint = From RoleConstraint In larBinaryRoleConstraint
                                                  Where RoleConstraint.RoleConstraintRole(0).Role.FactType Is Me
                                                  Where RoleConstraint.RoleConstraintRole(1).Role.FactType Is Me
                                                  Where RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.RingConstraint
                                                  Where RoleConstraint.RingConstraintType = pcenumRingConstraintType.Transitive
                                                  Select RoleConstraint

                Return larTransitiveRingConstraint.Count > 0

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Public Overrides Function HasTotalRoleConstraint() As Boolean

            '------------------------------------------------------------------------
            'RETURNS TRUE if a Total Role Constraint exists within
            'the FactType
            'Else returns False
            '------------------------------------------------------------------------
            'PSEUDOCODE
            '  * IF the RoleGroup.Constraint.Count < FactType.Cardinality THEN
            '      * RETURN FALSE
            '    ELSE IF the RoleGroup.Constraint.Count = FactType.Cardinality THEN
            '      * RETURN TRUE
            '    ELSE
            '      * IF the count of InternalUniquenessConstraints at level 1 = 
            '           the FactType.Cardinality for the RoleGroup THEN
            '         * RETURN TRUE
            '    END IF
            '------------------------------------------------------------------------

            If Me.InternalUniquenessConstraint.Count = 1 Then
                If Me.InternalUniquenessConstraint(0).RoleConstraintRole.Count = Me.Arity Then
                    '-------------------------------------------------
                    'Then must be a TotalInternalUniquenessConstraint
                    '-------------------------------------------------
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function


        Function Is1To1BinaryFactType() As Boolean

            '--------------------------------------------
            'Returns True if 1:1 binary fact type
            'Else returns False
            '--------------------------------------------
            If Me.IsBinaryFactType() And (Me.GetHighestConstraintLevel() > 1) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overrides Function IsBinaryFactType() As Boolean

            '--------------------------------------------------
            'RETURNS TRUE if the FactType is a BinaryFactType
            '--------------------------------------------------
            If Me.Arity = 2 Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Returns TRUE if the FactType within wich RoleId is present is a 1 to 1 binary FactType
        ''' and has two mandatory roles else returns FALSE
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsDoubleMandatory1To1FactType() As Boolean

            If Me.Is1To1BinaryFactType() Then
                If Me.GetMandatoryRoleCount() = 2 Then
                    Return True
                End If
            End If

            Return False

        End Function

        ''' <summary>
        ''' RETURNS TRUE if the FactType has a TotalInternalUniquenessConstraint or a PartialButMultiInternalUniquenessConstraint. Used in RDS Processing.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function isRDSTable() As Boolean

            Return (Me.HasTotalRoleConstraint Or Me.HasPartialButMultiRoleConstraint Or Me.IsObjectified) And Not Me.IsMDAModelElement

        End Function

        ''' <summary>
        ''' Returns True if 1:1 binary fact type
        ''' Else returns False
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function IsManyTo1BinaryFactType() As Boolean

            If Me.IsBinaryFactType() And (Me.GetHighestConstraintLevel() = 1) And Not Me.HasTotalRoleConstraint Then
                Return True
            Else
                Return False
            End If

        End Function

        Function IsManyToOneByRoleOrder(aarRole As List(Of FBM.Role)) As Boolean

            Try
                If aarRole.Count <> 2 Then
                    Throw New ApplicationException("Validating Role count must equal 2 (two).")
                End If

                If Not Me.IsManyTo1BinaryFactType Then
                    Return False
                End If

                If Me.InternalUniquenessConstraint(0).RoleConstraintRole(0).Role.Id <> aarRole(0).Id Then
                    Return False
                End If

                Dim lrSecondRole As FBM.Role = aarRole(1)

                Dim larRolesInInternalUniquenessConstraints = From IUC In Me.InternalUniquenessConstraint _
                                                              From RCR In IUC.RoleConstraintRole _
                                                              Where RCR.Role.Id = lrSecondRole.Id _
                                                              Select RCR.Role

                If larRolesInInternalUniquenessConstraints.Count <> 0 Then
                    Return False
                End If

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

        Public Function getImpliedFactTypes() As List(Of FBM.FactType)

            Try

                If Not Me.IsObjectified Then
                    Throw New Exception(Me.Id & " is not an objectified FactType.")
                End If

                Dim larImpliedFactTypes = From FactType In Me.Model.FactType
                                          Where FactType.IsLinkFactType
                                          Select FactType

                Dim larFactType = From lrFactType In larImpliedFactTypes
                                  Where Me.RoleGroup.FindAll(Function(x) x.Id = lrFactType.LinkFactTypeRole.Id).Count > 0
                                  Select lrFactType

                'Dim larImpliedFactTypes As New List(Of FBM.FactType)

                'For Each lrFactType In larFactType
                '    larImpliedFactTypes.Add(lrFactType)
                'Next

                Return larFactType.ToList

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.FactType)
            End Try

        End Function

        Function getLinkFactTypes() As List(Of FBM.FactType)

            Try
                Dim larLinkFactType = From FactType In Me.Model.FactType _
                                      Where FactType.IsLinkFactType = True _
                                      And Me.RoleGroup.Contains(FactType.LinkFactTypeRole) _
                                      Select FactType

                Return larLinkFactType.ToList

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
        ''' Returns the Count of MandatoryRoles within the FactType.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetMandatoryRoleCount() As Integer

            Dim liInd As Integer = 0
            Dim lrRole As FBM.Role

            For Each lrRole In Me.RoleGroup
                If lrRole.Mandatory Then
                    liInd += 1
                End If
            Next

            Return liInd

        End Function

        ''' <summary>
        ''' Gets the counts of (Role) referenced ModelObjects and returns those counts in the dictionary argument.
        ''' </summary>
        ''' <param name="arModelObjectCountDictionary"></param>
        ''' <remarks></remarks>
        Public Sub GetReferencedModelObjectCounts(ByRef arModelObjectCountDictionary As Dictionary(Of String, Integer))

            For Each lrRole In Me.RoleGroup

                If arModelObjectCountDictionary.ContainsKey(lrRole.JoinedORMObject.Id) Then
                    arModelObjectCountDictionary.Item(lrRole.JoinedORMObject.Id) += 1
                Else
                    arModelObjectCountDictionary.Add(lrRole.JoinedORMObject.Id, 1)
                End If
            Next

        End Sub

        Function GetModelObjects() As List(Of FBM.ModelObject)

            Dim larModelObject As New List(Of FBM.ModelObject)

            Dim larRoleJoinedModelObject = From Role In Me.RoleGroup
                                           Select Role.JoinedORMObject

            larModelObject = larRoleJoinedModelObject.ToList

            Return larModelObject

        End Function

        Public Function getNotOutgoingFactTypeReadings(ByVal arModelObject As FBM.ModelObject) As List(Of FBM.FactTypeReading)

            Dim larFactTypeReading = From FactTypeReading In Me.FactTypeReading
                                     Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id <> arModelObject.Id
                                     Select FactTypeReading

            If larFactTypeReading.Count = 0 Then
                Return Nothing
            Else
                Return larFactTypeReading.ToList
            End If

        End Function


        Public Function getOutgoingFactTypeReading(ByVal arModelObject As FBM.ModelObject) As FBM.FactTypeReading

            Dim larFactTypeReading = From FactTypeReading In Me.FactTypeReading
                                     Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = arModelObject.Id
                                     Select FactTypeReading

            If larFactTypeReading.Count = 0 Then
                Return Nothing
            Else
                Return larFactTypeReading.First
            End If

        End Function

        ''' <summary>
        ''' Returns an Outgoing FactTypeReading for this FactType for the given Table.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Function getOutgoingFactTypeReadingForTabe(ByVal arTable As RDS.Table) As FBM.FactTypeReading

            Dim lrFactTypeReading = From FactTypeReading In Me.FactTypeReading
                                    Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = arTable.Name
                                    Select FactTypeReading

            Return lrFactTypeReading.First

        End Function
        Function GetOtherRoleOfBinaryFactType(ByVal asRoleId As String) As FBM.Role

            'RETURNS the complimentory RoleId of the FactType in which asRoleId is involved
            'Assumes that the RoleGroup is populated returns an error if called with a l_RoleId that belongs to other
            'than a binary FactType
            Try
                Dim lrRole As FBM.Role

                If Not Me.IsBinaryFactType() Then
                    Throw New System.Exception("Error: FBM.tFactType.GetOtherRoleOfBinaryFactType: Non binary FactType for Role: aiRoleId: " & asRoleId)
                End If

                GetOtherRoleOfBinaryFactType = Nothing

                For Each lrRole In Me.RoleGroup
                    If Not lrRole.Id = asRoleId Then
                        GetOtherRoleOfBinaryFactType = lrRole
                    End If
                Next

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
        ''' Returns the TableName that a FactType belongs to when converting an ORMDiagram to a RelationalModel
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetTableName() As String
            '--------------------------------------------------------
            'RETURNS - The Table_Name to which the FactType belongs
            '--------------------------------------------------------

            GetTableName = ""

            If Me.HasTotalRoleConstraint() Then
                'Rule 3 and part of 1
                GetTableName = Me.Name
                Exit Function
            ElseIf Me.HasPartialButMultiRoleConstraint Then
                'Rule 1
                GetTableName = Me.Name
                Exit Function
            ElseIf Me.IsUnaryFactType Then
                Select Case Me.RoleGroup(0).TypeOfJoin
                    Case Is = pcenumRoleJoinType.EntityType, pcenumRoleJoinType.FactType
                        GetTableName = Me.RoleGroup(0).JoinedORMObject.Name
                        Exit Function
                End Select
            ElseIf Me.IsBinaryFactType() Then
                'Rules 2 and 4
                Dim lrRole As FBM.Role
                For Each lrRole In Me.RoleGroup
                    Select Case lrRole.TypeOfJoin
                        Case Is = pcenumRoleJoinType.EntityType
                            'Then Role joins to EntityType
                            If Not Me.Is1To1BinaryFactType() Then
                                If lrRole.HasInternalUniquenessConstraint() Then
                                    GetTableName = lrRole.JoinedORMObject.Name
                                    Exit Function
                                End If
                            Else 'is 1:1 binary fact type
                                If lrRole.Mandatory Then
                                    If Me.GetMandatoryRoleCount() = 1 Then
                                        'Part 1 of Rule 4
                                        GetTableName = lrRole.JoinedORMObject.Name
                                        Exit Function
                                    ElseIf Me.GetMandatoryRoleCount() = 2 Then
                                        'Part 2 of Rule 4
                                        GetTableName = lrRole.Name
                                        Exit Function
                                    End If
                                End If
                            End If
                        Case Is = pcenumRoleJoinType.FactType
                            'Joins nested FactType (ObjectifiedFactType)
                            GetTableName = lrRole.JoinedORMObject.Name
                    End Select
                Next 'Role in RoleGroup
            End If 'IsBinaryFactType

        End Function

        ''' <summary>
        ''' Removes the FactType from the Model if it is possible to do so.
        '''   i.e. If there are no RoleConstraints that are not InternalUniquenessConstraints attached to Roles of the FactType etc.
        ''' </summary>
        ''' <param name="abForceRemoval"></param>
        ''' <param name="abCheckForErrors"></param>
        ''' <param name="abDoDatabaseProcessing">In Client/Server mode, we might not require that database functions are performed, because another Client may have already done the processing.</param>
        ''' <param name="abIncludeSubtypeRelationshipFactTypes">Only used for Entity Types.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = True) As Boolean

            Dim liInd As Integer = 0
            Dim lrRole As FBM.Role
            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrFactTypeReading As FBM.FactTypeReading

            Try

                If Me.IsPreferredReferenceMode Then
                    Dim larEntityType = From Role In Me.RoleGroup
                                        From EntityType In Me.Model.EntityType
                                        Where EntityType.Id = Role.JoinedORMObject.Id
                                        Select EntityType

                    If IsSomething(larEntityType) Then
                        Dim lrEntityType As FBM.EntityType
                        For Each lrEntityType In larEntityType
                            If lrEntityType.ReferenceModeFactType Is Me Then
                                lrEntityType.RemoveSimpleReferenceScheme(abForceRemoval)
                            End If
                        Next
                    End If
                End If

                'LinkFactTypes
                If Me.IsObjectified Then
                    Dim larLinkFactType = Me.getLinkFactTypes

                    For Each lrFactType In larLinkFactType.ToArray
                        Call lrFactType.RemoveFromModel()
                    Next

                    'Objectifying EntityType
                    Call Me.ObjectifyingEntityType.RemoveFromModel(True, , True)
                End If

                For liInd = 1 To Me.FactTypeReading.Count
                    lrFactTypeReading = New FBM.FactTypeReading
                    lrFactTypeReading = Me.FactTypeReading(0)
                    lrFactTypeReading.RemoveFromModel(False, False, abDoDatabaseProcessing)
                Next

                For Each lrRoleConstraint In Me.InternalUniquenessConstraint.ToArray
                    Call lrRoleConstraint.RemoveFromModel(True, False, abDoDatabaseProcessing)
                Next

                '20180425-VM-Was before removing FactTypeReadings, but that caused bugs. Instances of RoleInstance removed before RoleConstraints associated with those RoleInstances.
                For Each lrRole In Me.RoleGroup.ToArray
                    lrRole.RemoveFromModel(False, False, abDoDatabaseProcessing)
                Next

                Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.FactType)
                lrModelDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrModelDictionaryEntry.Equals)

                If lrModelDictionaryEntry IsNot Nothing Then
                    lrModelDictionaryEntry.removeConceptType(pcenumConceptType.FactType)
                End If

                If abDoDatabaseProcessing Then
                    Call TableFactType.DeleteFactType(Me)
                End If

                Me.Model.RemoveFactType(Me, abDoDatabaseProcessing)

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

        Public Sub RemoveInternalUniquenessConstraint(ByRef arRoleConstraint As FBM.RoleConstraint,
                                                      ByVal abMakeModelDirty As Boolean,
                                                      ByVal abCheckForErrors As Boolean)

            Try
                Me.InternalUniquenessConstraint.Remove(arRoleConstraint)

                Dim liInd As Integer = 1
                For liInd = 1 To Me.InternalUniquenessConstraint.Count
                    Me.InternalUniquenessConstraint(liInd - 1).LevelNr = liInd
                Next

                '================================================================================================
                'RDS - If there is no longer an InternalUniquenessConstraint on the FactType, then the 
                '  relative Relation/s must be removed from the RDS Model.
                If (Me.Arity = 2) And (Me.InternalUniquenessConstraint.Count = 0) Then

                    Dim larRelation = From Relation In Me.Model.RDS.Relation _
                                      Where Relation.ResponsibleFactType.Id = Me.Id _
                                      Select Relation

                    For Each lrRelation In larRelation.ToList
                        Call Me.Model.RDS.removeRelation(lrRelation)
                    Next

                End If

                RaiseEvent IUConstraintRemoved(Me, arRoleConstraint)

                Call Me.Model.MakeDirty(True, abCheckForErrors)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Sub RemoveInternalUniquenessConstraints(ByRef abBroadcastInterfaceEvent As Boolean, Optional ByVal abReplacingRoleConstraint As Boolean = False)

            Dim lrRoleConstraint As FBM.RoleConstraint

            Try

                For Each lrRoleConstraint In Me.InternalUniquenessConstraint.ToArray

                    Me.Model.RemoveRoleConstraint(lrRoleConstraint, False, abBroadcastInterfaceEvent, abReplacingRoleConstraint)
                    Me.InternalUniquenessConstraint.Remove(lrRoleConstraint)

                    RaiseEvent IUConstraintRemoved(Me, lrRoleConstraint)

                    If abBroadcastInterfaceEvent Then
                        Call TableRoleConstraintRole.DeleteRoleConstraintRolesByRoleConstraint(lrRoleConstraint)
                        Call TableRoleConstraint.delete_RoleConstraint(lrRoleConstraint)
                    End If
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
        ''' Objectifies the FactType
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Objectify()

            Try
                Me.ObjectifyingEntityType = Me.Model.CreateEntityType(Nothing, False)
                Me.ObjectifyingEntityType.IsObjectifyingEntityType = True
                Me.Model.AddEntityType(Me.ObjectifyingEntityType, True, True, Nothing)

                Call Me.CreateLinkFactTypes(True)

                Call Me.SetIsObjectified(True, True)

                Me.Model.Save()

                '====================================================================
                'RDS
                If Me.IsManyTo1BinaryFactType Then

                    '--------------------------------------------------------
                    'Special case. Need to remove the existing RDS.Relation
                    '  and create a RDS.Table that is a PGSRelationNode
                    Dim lrRelation As RDS.Relation = Me.Model.RDS.getRelationByResponsibleFactType(Me)
                    Call Me.Model.RDS.removeRelation(lrRelation)

                    Dim lrTable As New RDS.Table(Me.Model.RDS, Me.Name, Me)

                    Dim larColumn As New List(Of RDS.Column)

                    For Each lrRole In Me.RoleGroup
                        Call larColumn.AddRange(lrRole.getColumns(lrTable, lrRole))
                    Next

                    lrTable.setIsPGSRelation(True)

                    Call Me.Model.RDS.addTable(lrTable)

                    For Each lrColumn In larColumn
                        Call lrTable.addColumn(lrColumn)
                    Next

                    For Each lrRole In Me.RoleGroup

                        Dim larLinkFactTypeRole = From FactType In Me.Model.FactType
                                                  Where FactType.IsLinkFactType = True _
                                                  And FactType.LinkFactTypeRole Is lrRole
                                                  Select FactType.RoleGroup(0)

                        For Each lrLinkFactTypeRole In larLinkFactTypeRole
                            Call Me.Model.generateRelationForManyTo1BinaryFactType(lrLinkFactTypeRole)
                        Next
                    Next

                Else
                    Dim lrTable As RDS.Table = Me.Model.RDS.Table.Find(Function(x) x.Name = Me.Id)

                    '  Move Relations that reference this FactType to their respective LinkFactType
                    'NB Do this before setting IsPGSRelation, such that the single Relation is created.
                    Call Me.Model.moveRelationsOfFactTypeToRespectiveLinkFactTypes(Me)

                    If Not lrTable.isPGSRelation Then
                        lrTable.setIsPGSRelation(True)
                    End If

                End If

                    Call Me.Model.MakeDirty(True, False)

                RaiseEvent Objectified()


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        ''' <summary>
        ''' Removes a FactTypeReading from the list of FactTypeReadings for the FactType.
        ''' </summary>
        ''' <param name="arFactTypeReading"></param>
        ''' <remarks></remarks>
        Public Sub RemoveFactTypeReading(ByVal arFactTypeReading As FBM.FactTypeReading,
                                         ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                Me.FactTypeReading.Remove(arFactTypeReading)

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelDeleteFactTypeReading, arFactTypeReading, Nothing)
                End If

                Call Me.Model.MakeDirty(False, True)

                RaiseEvent FactTypeReadingRemoved(arFactTypeReading)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Removes the Objectification of the FactType
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveObjectification(ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                Me.IsObjectified = False

                Dim larLinkFactType As New List(Of FBM.FactType)

                'Find the LinkFactTypes for the FactType, but move the RDS Relations for those LinkFactTypes before removing the LinkFactTypes.
                For Each lrFactType In Me.Model.FactType.FindAll(Function(x) x.IsLinkFactType)
                    If Me.RoleGroup.Contains(lrFactType.LinkFactTypeRole) Then
                        larLinkFactType.Add(lrFactType)
                    End If
                Next

                '=================================================================================================================
                'RDS-Move the RDS Relations for the LinkFactTypes back to being against the FactType
                '  The reason is that the FactType will still represent an Entity/Node in the RDS, and with associated Relations
                '  but those Relations are no longer associated with the relative LinkFactType, but the FactType itself.
                'Removing the LinkFactType from the Model will otherwise remove the Relations from the RDS Model.
                'NB Different for ManyTo1BinaryFactType, as below.
                If Me.IsManyTo1BinaryFactType Then
                    'Because the Relations are no longer for an RDSRelation Table, but the ModelObject/Table on the Many side of the FactType.
                    Call Me.Model.removeRDSRelationsForLinkFactTypesForFactType(Me)
                    Dim lrRole = Me.RoleGroup.Find(Function(x) x.HasInternalUniquenessConstraint)
                    Call Me.Model.generateRelationForManyTo1BinaryFactType(lrRole)
                Else
                    Call Me.Model.moveRelationsOfLinkFactTypesToRespectiveFactType(Me)
                End If

                'Remove the LinkFactTypes for the FactType.
                For Each lrFactType In larLinkFactType.ToArray
                    lrFactType.RemoveFromModel(False, True, abBroadcastInterfaceEvent)
                Next

                'RDS-Remove the RDSRelation Table if the FactType is a ManyTo1BinaryFactType
                If Me.IsManyTo1BinaryFactType Then
                    Dim lrRDSRelationTable = Me.getCorrespondingRDSTable
                    Call Me.Model.RDS.removeTable(lrRDSRelationTable) 'Because is no longer needed for anything.
                End If

                RaiseEvent ObjectificationRemoved()

                Call Me.ObjectifyingEntityType.RemoveFromModel(True, False, abBroadcastInterfaceEvent)
                Me.ObjectifyingEntityType = Nothing

                '------------------------------------------------------------------------------------------------------------
                'Save the FactType because there's a need to remove the ObjectifyingEntityType reference from the FactType.
                '------------------------------------------------------------------------------------------------------------
                'If abBroadcastInterfaceEvent Then 'Because if not, is the receiving Client and the sending Client has already performed the database actions.
                Me.makeDirty()
                Me.Save()
                'End If


                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
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
        ''' Removes a Role from the RoleGroup of the FactType
        ''' </summary>
        ''' <param name="arRole"></param>
        ''' <param name="abRemoveFromDatabase"></param>
        ''' <remarks></remarks>
        Sub RemoveRole(ByRef arRole As FBM.Role,
                       ByVal abRemoveFromDatabase As Boolean,
                       ByVal abDoDatabaseProcessing As Boolean)

            '--------------------------------
            'Remove any associated FactData
            '--------------------------------
            Try
                Dim lsRoleId As String = arRole.Id
                Dim larFactData = From f In Me.Fact, _
                                      fd In f.Data _
                                      Where fd.Role.Id = lsRoleId _
                                     Select fd

                Dim lrFactData As FBM.FactData
                Dim larFactDataSet As New System.Collections.Generic.List(Of FBM.FactData)
                For Each lrFactData In larFactData
                    larFactDataSet.Add(lrFactData)
                Next
                For Each lrFactData In larFactDataSet
                    lrFactData.RemoveFromModel()
                Next

                '-----------------------------------------------------------------------
                'Remove the Role from any FactTypeReading associated with the FactType
                '-----------------------------------------------------------------------
                Dim lrFactTypeReading As FBM.FactTypeReading
                For Each lrFactTypeReading In Me.FactTypeReading
                    lrFactTypeReading.RemovePredicatePartForRole(arRole)
                Next

                Dim lrRoleConstraint As FBM.RoleConstraint
                For Each lrRoleConstraint In Me.InternalUniquenessConstraint.ToArray
                    If lrRoleConstraint.Role.Contains(arRole) Then
                        If lrRoleConstraint.RoleConstraintRole.Count = 1 Then
                            'Remove the whole RoleConstraint
                            Call Me.Model.RemoveRoleConstraint(lrRoleConstraint, False, abDoDatabaseProcessing)
                        Else
                            Call lrRoleConstraint.RemoveRoleConstraintRoleByRole(arRole, abDoDatabaseProcessing)
                        End If

                    End If
                    If lrRoleConstraint.RoleConstraintRole.Count = 0 Then
                        Me.InternalUniquenessConstraint.Remove(lrRoleConstraint)
                        Call Me.Model.RemoveRoleConstraint(lrRoleConstraint, False, True)
                    End If
                Next


                '--------------
                'LinkFactType
                If arRole.FactType.IsObjectified Then

                    Dim lrRole As FBM.Role = arRole

                    'Must remove the corresponding LinkFactType
                    Dim larLinkFactType = From FactType In Me.Model.FactType _
                                          Where FactType.IsLinkFactType _
                                          And FactType.LinkFactTypeRole Is lrRole _
                                          Select FactType

                    If larLinkFactType.Count > 0 Then
                        Call larLinkFactType.First.RemoveFromModel(True, False, True)
                    End If

                End If

                '=================================================================
                'RDS - Must be called before removing the Role from the FactType
                Call Me.Model.removeColumnsIndexColumnsForRole(arRole)

                Me.RoleGroup.Remove(arRole)

                '-------------------
                'Model is now Dirty
                '-------------------
                Me.Model.IsDirty = True

                '------------------------------------------
                'Check to see if the Role is to be removed 
                '  from the database
                '------------------------------------------
                If abRemoveFromDatabase Then
                    Call TableRole.DeleteRole(arRole)
                End If

                Me.Model.Role.Remove(arRole)

                RaiseEvent RoleRemoved(arRole)

                If Me.Arity = 0 Then
                    Call Me.RemoveFromModel(True, False, abDoDatabaseProcessing)
                ElseIf Me.Arity = 1 Then
                    '------------------------------------------------------------------------------------------------------------
                    'Special case: If the FactType has Arity two, and the other Role of the FactType is joined to a ValueType
                    '  then remove the whole FactType.
                    If Me.RoleGroup(0).JoinedORMObject IsNot Nothing Then
                        If Me.RoleGroup(0).JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                            Call Me.RemoveFromModel(True, False, abDoDatabaseProcessing)
                        Else
                            If Me.InternalUniquenessConstraint.Count = 1 Then
                                Dim lrInternalUniquenessConstraint As FBM.RoleConstraint = Me.InternalUniquenessConstraint(0)
                                Call lrInternalUniquenessConstraint.RemoveFromModel(True, False, abDoDatabaseProcessing)
                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        ''' <summary>
        ''' Saves the FactType to the database
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Dim lrRole As FBM.Role
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lrPredicatePart As FBM.PredicatePart
            Dim lrFact As FBM.Fact


            Try
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.FactType, Me.ShortDescription, Me.LongDescription)
                lrDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                lrDictionaryEntry.isFactType = True

                If abRapidSave Then
                    pdbConnection.BeginTrans()
                    Call TableFactType.AddFactType(Me)
                    pdbConnection.CommitTrans()

                ElseIf Me.isDirty Then

                    If TableFactType.ExistsFactTypeByModel(Me) Then
                        Call TableFactType.UpdateFactType(Me)
                        Call lrDictionaryEntry.Save()
                    Else
                        Try
                            pdbConnection.BeginTrans()
                            Call lrDictionaryEntry.Save()
                            If TableFactType.ExistsFactType(Me) Then
                                Call TableFactType.UpdateFactType(Me)
                            Else
                                Call TableFactType.AddFactType(Me)
                            End If
                            pdbConnection.CommitTrans()
                        Catch ar_err As Exception
                            MsgBox("Error: FBM.tFactType.Save: " & ar_err.Message & ": FactTypeId: " & Me.Id)
                            pdbConnection.RollbackTrans()
                        End Try
                    End If

                    Me.isDirty = False
                End If

                '-----------------------------------------
                'Save the Roles within the RoleGroup
                '-----------------------------------------
                For Each lrRole In Me.RoleGroup
                    Try
                        lrRole.Save(abRapidSave)
                    Catch arErr As Exception
                        MsgBox("Error: FBM.tFactType.Save: " & arErr.Message & ": FactTypeId: " & Me.Id)
                    End Try
                Next

                '---------------------------
                'Save the FactTypeReadings
                '---------------------------
                For Each lrFactTypeReading In Me.FactTypeReading
                    lrFactTypeReading.Save(abRapidSave)
                    For Each lrPredicatePart In lrFactTypeReading.PredicatePart
                        lrPredicatePart.Save(abRapidSave)
                    Next
                Next

                '----------------
                'Save the Facts
                '----------------
                For Each lrFact In Me.Fact
                    Try
                        lrFact.Save(abRapidSave)
                    Catch arErr As Exception
                        Dim lsMessage As String
                        lsMessage = "Error: FBM.FactType.Save"
                        lsMessage &= vbCrLf & "FactTypeId: " & Me.Id
                        lsMessage &= vbCrLf & vbCrLf & arErr.Message
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, arErr.StackTrace)
                    End Try
                Next

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

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent DerivationTextChanged(asDerivationText)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
            End If

        End Sub

        ''' <summary>
        ''' Resets an already existing FactTypeReading
        ''' </summary>
        ''' <param name="arFactTypeReading"></param>
        ''' <remarks></remarks>
        Public Sub SetFactTypeReading(ByRef arFactTypeReading As FBM.FactTypeReading,
                                      ByRef abBroadcastInterfaceEvent As Boolean)

            Try
                Dim liIndex As Integer = -1
                liIndex = Me.FactTypeReading.IndexOf(arFactTypeReading)

                If liIndex >= 0 Then

                    Me.FactTypeReading(liIndex) = arFactTypeReading

                    '-------------------------------------------------------
                    'RDS
                    Dim lrFactTypeReading As FBM.FactTypeReading = arFactTypeReading
                    If Me.Arity = 1 Then
                        'Need to change the name of the corresponding RDS.Column name for the modified FactTypeReading.

                        Try
                            Dim lrTable = Me.RoleGroup(0).JoinedORMObject.getCorrespondingRDSTable
                            Dim lrColumn = (From Column In lrTable.Column
                                            Where Column.Role Is Me.RoleGroup(0)
                                            Where Column.ActiveRole Is Me.RoleGroup(0)
                                            Select Column).First

                            lrColumn.setName(Viev.Strings.MakeCapCamelCase(arFactTypeReading.PredicatePart(0).PredicatePartText, True))

                        Catch ex As Exception

                        End Try

                    ElseIf Me.Arity = 2 Then

                        Dim larColumn = From Table In Me.Model.RDS.Table
                                        From Column In Table.Column
                                        Where Column.Role.Id = lrFactTypeReading.PredicatePart(0).Role.Id
                                        Select Column

                        For Each lrColumn In larColumn
                            Call lrColumn.setName(lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lrColumn.getAttributeName)
                        Next

                        If Me.IsManyTo1BinaryFactType Then

                            Dim larRole = From Role In Me.RoleGroup _
                                          Where Role.HasInternalUniquenessConstraint _
                                          Select Role

                            Dim lrRole As FBM.Role = larRole(0)

                            Dim larRelation = From Relation In Me.Model.RDS.Relation _
                                              Where Relation.ResponsibleFactType.Id = Me.Id
                                              Select Relation

                            If larRelation.Count > 0 Then
                                Dim lrRelation As RDS.Relation = larRelation.First

                                If arFactTypeReading.PredicatePart(0).Role.Id = lrRole.Id Then
                                    Dim lsDestinationPredicate = arFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                                    Call lrRelation.setDestinationPredicate(lsDestinationPredicate)
                                Else
                                    Dim lsOriginPredicate = arFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                                    Call lrRelation.setOriginPredicate(lsOriginPredicate)
                                End If
                            End If
                        End If

                    End If


                    RaiseEvent FactTypeReadingModified(arFactTypeReading)

                    If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then

                        Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactTypeReading,
                                                                            arFactTypeReading,
                                                                            Nothing)
                    End If

                Else
                    Throw New Exception("No FactTypeReading found to change.")
                End If


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub SetIsDerived(ByVal abIsDerived As Boolean, ByVal abBroadcastInterfaceEvent As Boolean)

            Me.IsDerived = abIsDerived

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent IsDerivedChanged(abIsDerived)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
            End If

        End Sub

        Public Sub SetIsObjectified(ByVal abNewIsObjectified As Boolean, ByVal abBroadcastInterfaceEvent As Boolean)

            Me.IsObjectified = abNewIsObjectified

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent IsObjectifiedChanged(abNewIsObjectified)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
            End If

        End Sub

        Public Sub SetIsPreferredFactTypeReadingForFactType(ByRef arFactTypeReading As FBM.FactTypeReading)

            For Each lrFactTypeReading In Me.FactTypeReading

                If lrFactTypeReading.Id = arFactTypeReading.Id Then
                    lrFactTypeReading.IsPreferred = True
                Else
                    lrFactTypeReading.IsPreferred = False
                End If
            Next

        End Sub

        Public Sub SetIsPreferredFactTypeReadingForPredicate(ByRef arFactTypeReading As FBM.FactTypeReading)

            Dim lsTypedPredicateId As String = arFactTypeReading.TypedPredicateId

            For Each lrFactTypeReading In Me.FactTypeReading.FindAll(Function(x) x.TypedPredicateId = lsTypedPredicateId)
                If lrFactTypeReading.Id = arFactTypeReading.Id Then
                    lrFactTypeReading.IsPreferredForPredicate = True
                Else
                    lrFactTypeReading.IsPreferredForPredicate = False
                End If
            Next

        End Sub

        Public Sub SetIsStored(ByVal abIsStored As Boolean, ByVal abBroadcastInterfaceEvent As Boolean)

            Me.IsStored = abIsStored

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent IsStoredChanged(abIsStored)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
            End If

        End Sub

        Public Overrides Sub setName(ByVal asNewName As String, Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

            Dim lsMessage As String = ""
            '-----------------------------------------------------------------------------------------------------------
            'The following explains the logic and philosophy of Richmond.
            '  A FactType.Id/Name represents the same thing accross all Models in Richmond, otherwise the Richmond 
            '  user would have a different FactType.Id/Name for the differing Concepts (not excluding that in Richmond
            '  a FactType in one Model can have a wildly different RoleGroup (ModelObject associations) than the same
            '  named FactType in another Model).
            '  So, for example, 'Person' is the same Concept accross all Models.
            '  If in this Model the user changes the FactType.Id/Name of 'Person' to 'Persona' then the following 
            '  code works like this...If 'Person' is used by other Models, then 'Persona' is added as a new FactType.
            '  Otherwise, there is no risk in changing the FactType.Id/Name of 'Person' to 'Persona' in the current
            '  model.
            '  This approach takes a pessimistic view, that when changing a FactType.Id/Name in one Model the user
            '  does not want the FactType.Id/Name to be changed in all Models.
            '-----------------------------------------------------------------------------------------------------------
            '-----------------------------------------------------------
            'Set the name and Symbol of the FactType to the new asNewName.
            '  The Id of the FactType is modified later in this Set.
            '-----------------------------------------------------------
            Try

                '--------------------------------------------------------------------------------------------------------
                'CodeSafe: Abort if an attempt is made to set the Name/Id/Symbol of the ModelObject to "" (EmptyString)
                '--------------------------------------------------------------------------------------------------------
                If asNewName = "" Then
                    lsMessage = "Failed: Attempt to set FactType.Name to Empty String, ''."
                    Throw New Exception(lsMessage)
                End If

                _Name = asNewName
                Me.Symbol = asNewName

                Dim lsOldName As String = Me.Id

                '---------------------------------------------------------------------------------------------------------
                'The surrogate key (FactType.Id) for the FactType is about to change (to match the name of the FactType)
                '  so update the ModelDictionary entry for the Concept/Symbol (the nominalistic idenity of the FactType)
                '---------------------------------------------------------------------------------------------------------
                If StrComp(Me.Id, asNewName) <> 0 Then
                    '------------------------------------------------------
                    'The new FactType.Name does not match the FactType.Id
                    '------------------------------------------------------
                    Call Me.SwitchConcept(New FBM.Concept(asNewName, True), pcenumConceptType.FactType)

                    '------------------------------------------------------------------------------------------
                    'Update the Model(database) immediately. There is no choice. The reason why
                    '  is because the (in-memory) key is changing, so the database must be updated to 
                    '  reflect the new key, otherwise it will not be possible to Update an existing FactType.
                    '------------------------------------------------------------------------------------------
                    Dim lrDictionaryEntry As New DictionaryEntry(Me.Model, asNewName, pcenumConceptType.FactType, , , True, True)
                    lrDictionaryEntry.Save()
                    Call TableFactType.ModifyKey(Me, asNewName)

                    If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                        Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
                    End If

                    Me.Id = asNewName
                    Call TableFactType.UpdateFactType(Me) 'Sets the new Name, TableFactType.ModifyKey (above) only modifies the key.

                    Me.Model.MakeDirty()
                    Call Me.RaiseEventNameChanged(asNewName)
                    RaiseEvent Updated()

                    Dim larRole = From Role In Me.Model.Role _
                                  Where Role.JoinedORMObject Is Me _
                                  Select Role

                    For Each lrRole In larRole
                        lrRole.makeDirty()
                        lrRole.FactType.makeDirty()
                        lrRole.FactType.Save()
                    Next

                End If 'Me.Id <> asNewName

                'If abTriggerEvents Then
                '    RaiseEvent Updated()
                '    Call Me.RaiseEventNameChanged(asNewName)
                '    Call Me.Model.MakeDirty(True)
                'End If

                '------------------------------------------------------------------------------------
                'Must save the Model because Roles that reference the EntityType must be saved.
                '  NB If Roles are saved, the FactType must be saved. If the FactType is saved,
                '  the RoleGroup's references (per Role) must be saved. A Role within the RoleGroup 
                '  may reference another FactType, so that FactType must be saved...etc.
                '  i.e. It's easier and safer to simply save the whole model.
                '------------------------------------------------------------------------------------
                For Each lrRole In Me.Model.Role.FindAll(Function(x) x.JoinedORMObject.Id = Me.Id)
                    lrRole.makeDirty()
                    Call lrRole.Save()
                Next

                '-------------------------------------------------------------
                'To make sure all the FactData and FactDataInstances/Pages are saved for RDS
                Me.Model.Save()                

            Catch ex As Exception
                lsMessage = "Error: tFactType.SetName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetShowFactTypeName(ByVal abNewShowFactTypeName As Boolean)

            Me.ShowFactTypeName = abNewShowFactTypeName

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent ShowFactTypeNameChanged(abNewShowFactTypeName)

        End Sub

        ''' <summary>
        ''' Transforms a given FactTypeReading to an available permutation of that FactTypeReading (Role sequence) for a
        '''   the FactType of that FactTypeReading and where that FactType has more than one Role referencing the same ModelObject.
        ''' NB Should be called when creating a new FactTypeReading and where the user supplied FactTypeReading (Role sequence) has
        '''   already been used within the FactType.
        ''' NB Should only be called for a FactTypeReading.FactType where that FactType has more than one Role referencing the same ModelObject.
        ''' </summary>
        ''' <param name="arFactTypeReading"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TransformFactTypeReadingToAvailablePermutation(ByRef arFactTypeReading As FBM.FactTypeReading) As FBM.FactTypeReading

            Return Me.GetAvailableFTRPermutation(arFactTypeReading)

        End Function

        Function TypeOfBinaryFactType() As Integer
            'This Function returns one of the following values
            '0- If the FactType has no mandatory roles
            '1- if the FactType only has one manatory role
            '2- if the FactType has two mandatory roles

            Dim lrRole As FBM.Role

            TypeOfBinaryFactType = 0

            For Each lrRole In Me.RoleGroup
                If lrRole.Mandatory Then
                    TypeOfBinaryFactType += 1
                End If
            Next

        End Function

        'Private Sub update_from_concept() Handles concept.updated

        '    Me.Name = Me.concept.Symbol

        'End Sub

        Public Sub ClearModelErrors() Implements iValidationErrorHandler.ClearModelErrors
            Me.ModelError.Clear()
        End Sub

        Public Sub AddModelError(ByRef arModelError As ModelError) Implements iValidationErrorHandler.AddModelError

            Me.ModelError.Add(arModelError)
            RaiseEvent ModelErrorAdded(arModelError)

        End Sub

        Public Sub SetIsIndependent(ByVal abNewIsIndependent As Boolean,
                                    ByVal abBroadcastInterfaceEvent As Boolean) Implements iFBMIndependence.SetIsIndependent

            Try
                Me.IsIndependent = abNewIsIndependent

                Call Me.makeDirty()
                Call Me.Model.MakeDirty(False, False)

                RaiseEvent IsIndependentChanged(abNewIsIndependent)

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetIsSubtypeStateControlling(ByVal abIsSubtypeStateControlling As Boolean,
                                                ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                Me.IsSubtypeStateControlling = abIsSubtypeStateControlling

                Call Me.makeDirty()
                Call Me.Model.MakeDirty(False, False)

                RaiseEvent IsSubtypeStateControllingChanged(abIsSubtypeStateControlling)

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateFactType, Me, Nothing)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

    'To be implemented on move to Viev.FBM.Generic
    'Public Sub GetFactsFromDatabase()
    '    Call TableFact.GetFactsForFactType(Me)
    'End Sub

End Namespace