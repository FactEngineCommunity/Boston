Imports System.ComponentModel
Imports System.Collections.Generic
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
        DefaultValueAttribute(False), _
        DescriptionAttribute("True if the Fact Type is 'Objectified', else False.")> _
        Public Property IsObjectified() As Boolean
            Get
                Return _IsObjectified
            End Get
            Set(ByVal Value As Boolean)
                _IsObjectified = Value
            End Set
        End Property

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

        <XmlAttribute()> _
        Public IsSubtypeConstraintFactType As Boolean = False

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
             ReadOnlyAttribute(True), _
             Browsable(False)> _
        Public Overridable Property Arity() As Integer
            Get
                Return Me.RoleGroup.Count
            End Get
            Set(ByVal Value As Integer)
                _Arity = Value
            End Set
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
                Call Me.Model.MakeDirty()
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
        <XmlAttribute()> _
        <CategoryAttribute("Fact Type"), _
        DefaultValueAttribute(False), _
        DescriptionAttribute("True if the Fact Type is derived.")> _
        Public Property IsDerived As Boolean
            Get
                Return Me._IsDerived
            End Get
            Set(value As Boolean)
                Me._IsDerived = value
            End Set
        End Property

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
        Public _DerivationText As String = ""
        <XmlAttribute()> _
        <CategoryAttribute("Derivation"), _
        Browsable(False), _
        DescriptionAttribute("The text for the derivation of the Fact Type if the Fact Type is derived.")> _
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
        Public Event IdChanged()
        Public Event IUConstraintAdded(ByRef arFactType As FBM.FactType, ByRef arRoleConstraint As FBM.RoleConstraint)
        Public Event IUConstraintRemoved(ByRef arFactType As FBM.FactType, ByRef arRoleConstraint As FBM.RoleConstraint)
        Public Event FactTypeReadingAdded(ByRef arFactTypeReading As FBM.FactTypeReading)
        Public Event FactTypeReadingModified(ByRef arFactTypeReading As FBM.FactTypeReading)
        Public Event FactTypeReadingRemoved(ByRef arFactTypeReading As FBM.FactTypeReading)
        Public Event RoleAdded(ByRef arRole As FBM.Role)
        Public Event RoleRemoved(ByRef arRole As FBM.Role)
        Public Event FactRemoved(ByRef arFact As FBM.Fact)
        Public Event IsDerivedChanged(ByVal abIsDerived As Boolean)
        Public Event IsObjectifiedChanged(ByVal abNewIsObjectified As Boolean)
        Public Event IsStoredChanged(ByVal abIsStored As Boolean)
        Public Event IsPreferredReferenceModeChanged(ByVal abNewIsPreferrdReferenceMode As Boolean)
        Public Event IsSubtypeConstraintFactTypeChanged(ByVal abNewIsSubtypeConstraintFactType As Boolean)
        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded
        Public Event Objectified() 'When the FactType is changed to an ObjectifiedFactType
        Public Event ObjectifyingEntityTypeChanged(ByRef arNewObjectifyingEntityType As FBM.EntityType)
        Public Event ObjectificationRemoved() 'When the objectification of the FactType is removed.
        Public Event RemovedFromModel()
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

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal asFactTypeName As String, ByVal as_FactTypeId As String)

            Call Me.New()

            Me.Model = arModel
            Me.Id = as_FactTypeId
            Me.Name = asFactTypeName

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

        Public Overloads Function Clone(ByRef arModel As FBM.Model, Optional ByVal abAddToModel As Boolean = False) As Object

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
                        lrFactType.IsSubtypeConstraintFactType = .IsSubtypeConstraintFactType
                        lrFactType.ShowFactTypeName = .ShowFactTypeName
                        lrFactType.IsDerived = .IsDerived
                        lrFactType.IsStored = .IsStored
                        lrFactType.DerivationText = .DerivationText

                        If abAddToModel Then
                            arModel.AddFactType(lrFactType)
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
                Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

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
                larFact.Add(lrFact.Clone(Me))
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
                MsgBox("Error: " & ex.Message)

                Return 0
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
                MsgBox(lsMessage)

                Return 0
            End Try

        End Function

        Public Overridable Sub Delete()

        End Sub

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


        Public Sub AddRole(ByRef arRole As FBM.Role)

            Me.RoleGroup.Add(arRole)
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
                Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Information, "", False)
            Next

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
                Me.Model.MakeDirty(False)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: FBM.FactType.AddFact"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Sub AddFactTypeReading(ByRef arFactTypeReading As FBM.FactTypeReading)

            Me.FactTypeReading.Add(arFactTypeReading)
            Me.Model.MakeDirty()

            RaiseEvent FactTypeReadingAdded(arFactTypeReading)

        End Sub


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

            Dim lrFactList As New List(Of FBM.Fact)
            Dim lrFact As New FBM.Fact
            Dim lrReferencingFact As FBM.Fact

            If ab_delete_all Then
                lrFactList = Me.Fact.FindAll(AddressOf arFact.Equals)

                If lrFactList.Count > 0 Then
                    For Each lrFact In lrFactList.ToArray
                        Me.Fact.Remove(lrFact)
                        If IsSomething(Me.ObjectifyingEntityType) Then
                            Me.ObjectifyingEntityType.Instance.Remove(lrFact.Id)
                        End If
                        Call lrFact.RemoveFromModel() 'Permanently deletes the Fact from the database.
                        RaiseEvent FactRemoved(lrFact)

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
                    Me.Model.MakeDirty()
                    RaiseEvent Updated()
                End If
            Else
                lrFact = Me.Fact.Find(AddressOf arFact.Equals)

                If IsSomething(lrFact) Then
                    Me.Fact.Remove(lrFact)
                    If IsSomething(Me.ObjectifyingEntityType) Then
                        Me.ObjectifyingEntityType.Instance.Remove(lrFact.Id)
                    End If
                    Call lrFact.RemoveFromModel() 'Permanently deletes the Fact from the database.

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
                    RaiseEvent Updated()
                    RaiseEvent FactRemoved(lrFact)
                End If
            End If

        End Sub

        Public Sub RemoveFactById(ByVal arFact As FBM.Fact)

            Dim lrFact As FBM.Fact

            lrFact = Me.Fact.Find(AddressOf arFact.EqualsById)

            If IsSomething(lrFact) Then
                Me.Fact.Remove(lrFact)
                If IsSomething(Me.ObjectifyingEntityType) Then
                    Me.ObjectifyingEntityType.Instance.Remove(lrFact.Id)
                End If
                Call lrFact.RemoveFromModel() 'Permanently deletes the Fact from the database.
                Me.Model.MakeDirty()
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
                    FindSuitableFactTypeReading = Me.FactTypeReading.Find(Function(x) x.MatchesByFactTypesRoles)

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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return Nothing
            End Try

        End Function

        Public Function FindSuitableFactTypeReadingByRoles(ByVal aarRole As List(Of FBM.Role)) As FBM.FactTypeReading
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
            Dim lrFactTypeReading As New FBM.FactTypeReading
            Dim lrPredicatePart As FBM.PredicatePart
            Dim lrRole As FBM.Role

            For Each lrRole In aarRole
                lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading)
                lrPredicatePart.Role = lrRole
                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
            Next

            If IsSomething(Me.FactTypeReading) Then
                FindSuitableFactTypeReadingByRoles = Me.FactTypeReading.Find(AddressOf lrFactTypeReading.MatchesByRoles)

                If (FindSuitableFactTypeReadingByRoles Is Nothing) And _
                   (Me.Arity = 2) And _
                   (Me.FactTypeReading.Count > 0) Then
                    FindSuitableFactTypeReadingByRoles = Me.FactTypeReading(0)
                End If
            Else
                Return Nothing
            End If

        End Function

        ''' <summary>
        ''' Adds a Role to a FactType
        ''' </summary>
        ''' <param name="aoJoinedObject">The ModelObject to which rhe new Role relates.</param>
        ''' <returns>FBM.Role</returns>
        ''' <remarks></remarks>
        Public Overridable Function CreateRole(Optional ByRef aoJoinedObject As FBM.ModelObject = Nothing) As FBM.Role

            Dim lrRole As FBM.Role

            '-----------------------------------------------------
            'Create the new Role.
            '  NB Adds the Role to the RoleGroup of the FactType
            '-----------------------------------------------------
            lrRole = New FBM.Role(Me, aoJoinedObject)

            '------------------------------
            'Add the Role to the ORMModel
            '------------------------------
            Me.Model.Role.Add(lrRole)

            Me.AddRole(lrRole)

            CreateRole = lrRole

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
                    Dim lrRole As FBM.Role
                    Dim lsRoleId As String = ""
                    Dim laRoleOrders As List(Of List(Of Object))
                    Dim lrPredicatePart As FBM.PredicatePart
                    Dim lrFactTypeReading As New FBM.FactTypeReading(Me)

                    laRoleOrders = Me.PermutateRoleGroupsRoles

                    For liInd = 1 To laRoleOrders.Count
                        lrFactTypeReading.RoleList = New List(Of FBM.Role)
                        liInd2 = 1
                        For Each lsRoleId In laRoleOrders(liInd - 1)
                            lrRole = New FBM.Role(lrFactTypeReading.FactType, lsRoleId)
                            lrRole.JoinedORMObject = Me.RoleGroup.Find(AddressOf lrRole.EqualsBySymbol).JoinedORMObject
                            lrFactTypeReading.RoleList.Add(lrRole)
                            lrPredicatePart = New FBM.PredicatePart(liInd2, lrRole, "")
                            lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                            liInd2 += 1
                        Next
                        If arInitialFactTypeReading.EqualsByRoleJoinedModelObjectSequence(lrFactTypeReading) Then
                            If Me.FactTypeReading.FindAll(AddressOf lrFactTypeReading.EqualsByRoleSequence).Count = 0 Then
                                Return True
                            End If
                        End If
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

                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

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

            Me.InternalUniquenessConstraint.Add(arRoleConstraint)

            RaiseEvent IUConstraintAdded(Me, arRoleConstraint)

        End Sub

        Public Function CreateInternalUniquenessConstraint(ByRef aarRole As List(Of FBM.Role), _
                                                           Optional ByVal abIsPreferredIdentifier As Boolean = False, _
                                                           Optional ByVal abMakeModelDirty As Boolean = True) As FBM.RoleConstraint



            Try
                '---------------------------------
                'Create the RoleConstraint object
                '---------------------------------
                Dim lrRoleConstraint As New FBM.RoleConstraint
                Dim liNextUICLevel As Integer = 0

                liNextUICLevel = Me.HighestInternalUniquenessConstraintLevel + 1

                lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint, _
                                                                 aarRole, _
                                                                 Nothing, _
                                                                 liNextUICLevel, _
                                                                 abMakeModelDirty)

                lrRoleConstraint.IsPreferredIdentifier = abIsPreferredIdentifier

                '------------------------------------------------------
                'Add the InternalUniquenessConstraint to the FactType
                '------------------------------------------------------                
                Me.AddInternalUniquenessConstraint(lrRoleConstraint)

                Return lrRoleConstraint

            Catch ex As Exception
                MsgBox("AddInternalUniquenessConstraint: " & ex.Message)
                Return Nothing
            End Try


        End Function

        Public Sub CreateOneToOneInternalUniquenessConstraint()

            Dim larRole As New List(Of FBM.Role)

            Call Me.RemoveInternalUniquenessConstraints()

            larRole.Add(Me.RoleGroup(0))
            Call Me.CreateInternalUniquenessConstraint(larRole)
            larRole.Clear()
            larRole.Add(Me.RoleGroup(1))
            Call Me.CreateInternalUniquenessConstraint(larRole)

        End Sub

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
        Public Sub ChangeModel(ByRef arTargetModel As FBM.Model)

            Dim lrRole As FBM.Role

            Me.Model = arTargetModel

            If arTargetModel.FactType.Exists(AddressOf Me.Equals) Then
                '----------------------------------------------
                'The FactType is already in the TargetModel
                '----------------------------------------------
            Else
                arTargetModel.AddFactType(Me)
            End If

            For Each lrRole In Me.RoleGroup
                lrRole.ChangeModel(arTargetModel)
            Next

        End Sub

        Public Sub CreateManyToOneInternalUniquenessConstraint(ByVal arModelObject As FBM.ModelObject)

            Dim larRole As New List(Of FBM.Role)

            Call Me.RemoveInternalUniquenessConstraints()

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

        Public Overridable Sub GetFactsFromDatabase()
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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return Nothing
            End Try

        End Function


        Public Function GetHighestConstraintLevel() As Integer

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
                        lrFactTypeReading.RoleList = New List(Of FBM.Role)
                        liInd2 = 0
                        For Each lsRoleId In laRoleOrders(liInd - 1)
                            lrRole = New FBM.Role(lrFactTypeReading.FactType, lsRoleId)
                            lrRole = Me.RoleGroup.Find(AddressOf lrRole.EqualsBySymbol)
                            lrFactTypeReading.RoleList.Add(lrRole)
                            lrPredicatePart = New FBM.PredicatePart(liInd2, lrRole, "")
                            lrPredicatePart.FactTypeReading = arInitialFactTypeReading
                            lrPredicatePart.Model = arInitialFactTypeReading.Model
                            lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                            liInd2 += 1
                        Next
                        If arInitialFactTypeReading.EqualsByRoleJoinedModelObjectSequence(lrFactTypeReading) Then
                            If Me.FactTypeReading.FindAll(AddressOf lrFactTypeReading.EqualsByRoleSequence).Count = 0 Then
                                lrFactTypeReading.Id = arInitialFactTypeReading.Id
                                lrFactTypeReading.PredicatePart = arInitialFactTypeReading.PredicatePart
                                liInd2 = 0
                                For Each lrRole In lrFactTypeReading.RoleList
                                    lrFactTypeReading.PredicatePart(liInd2).Role = lrRole
                                    liInd2 += 1
                                Next
                                Return lrFactTypeReading
                            End If
                        End If
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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

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
                Dim larPredicatePart = From FactTypeReading In Me.FactTypeReading _
                                      From PredicatePart In FactTypeReading.PredicatePart _
                                      Where FactTypeReading.Id = arFactTypeReading.Id _
                                      And PredicatePart.SequenceNr = aiSequenceNr _
                                      And PredicatePart.RoleId = asRoleId _
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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

            End Try

            Return Nothing

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
                    Me.Model.ThrowErrorMessage("No Role exists for RoleId: " & "'" & asRoleId & "', for FactType with FactTypeId: '" & Me.Id & "'", pcenumErrorType.Critical, Nothing, True)
                    Return Nothing
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

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

            lsSignature = VievLibrary.Strings.RemoveWhiteSpace(Me.Id)

            For Each lrRole In Me.RoleGroup
                lsSignature &= VievLibrary.Strings.RemoveWhiteSpace(lrRole.JoinedORMObject.Id)
            Next

            Return lsSignature

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

        Function HasTotalRoleConstraint() As Boolean

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

        Function IsBinaryFactType() As Boolean

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
        Function IsDoubleMandatory1To1FactType() As Boolean

            If Me.Is1To1BinaryFactType() Then
                If Me.GetMandatoryRoleCount() = 2 Then
                    Return True
                End If
            End If

            Return False

        End Function

        Function IsManyTo1BinaryFactType() As Boolean

            '--------------------------------------------
            'Returns True if 1:1 binary fact type
            'Else returns False
            '--------------------------------------------
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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
                Return False
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

        Function GetOtherRoleOfBinaryFactType(ByVal asRoleId As String) As FBM.Role

            'RETURNS the complimentory RoleId of the FactType in which asRoleId is involved
            'Assumes that the RoleGroup is populated returns an error if called with a l_RoleId that belongs to other
            'than a binary FactType

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
        ''' <remarks></remarks>
        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean

            Dim liInd As Integer = 0
            Dim lrRole As FBM.Role
            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrFactTypeReading As FBM.FactTypeReading

            Try

                If Me.IsPreferredReferenceMode Then
                    Dim larEntityType = From Role In Me.RoleGroup _
                                        From EntityType In Me.Model.EntityType _
                                        Where EntityType.Id = Role.JoinedORMObject.Id _
                                        Select EntityType

                    If IsSomething(larEntityType) Then
                        Dim lrEntityType As FBM.EntityType
                        For Each lrEntityType In larEntityType
                            If lrEntityType.HasSimpleReferenceMode() Then
                                lrEntityType.RemoveSimpleReferenceScheme(abForceRemoval)
                            End If
                        Next
                    End If
                End If

                For Each lrRole In Me.RoleGroup.ToArray
                    lrRole.RemoveFromModel()
                Next

                For liInd = 1 To Me.FactTypeReading.Count
                    lrFactTypeReading = New FBM.FactTypeReading
                    lrFactTypeReading = Me.FactTypeReading(0)
                    lrFactTypeReading.RemoveFromModel()
                Next

                For Each lrRoleConstraint In Me.InternalUniquenessConstraint.ToArray
                    Call lrRoleConstraint.RemoveFromModel(True)
                Next

                Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.FactType)
                lrModelDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrModelDictionaryEntry.Equals)

                Call Me.Delete()
                Me.Model.RemoveFactType(Me)

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
        ''' Removes the InternalUniquenessConstraints from the FactType.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub RemoveInternalUniquenessConstraints()

            Dim lrRoleConstraint As FBM.RoleConstraint

            Try

                For Each lrRoleConstraint In Me.InternalUniquenessConstraint.ToArray

                    Me.Model.RemoveRoleConstraint(lrRoleConstraint)
                    Me.InternalUniquenessConstraint.Remove(lrRoleConstraint)

                    RaiseEvent IUConstraintRemoved(Me, lrRoleConstraint)

                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        ''' <summary>
        ''' Objectifies the FactType
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Objectify()

            Me.IsObjectified = True

            Me.ObjectifyingEntityType = Me.Model.CreateEntityType
            Me.ObjectifyingEntityType.IsObjectifyingEntityType = True

            Me.Model.MakeDirty()
            Me.Model.Save()

            RaiseEvent Objectified()

        End Sub


        ''' <summary>
        ''' Removes a FactTypeReading from the list of FactTypeReadings for the FactType.
        ''' </summary>
        ''' <param name="arFactTypeReading"></param>
        ''' <remarks></remarks>
        Public Sub RemoveFactTypeReading(ByVal arFactTypeReading As FBM.FactTypeReading)

            Try
                Me.FactTypeReading.Remove(arFactTypeReading)
                RaiseEvent FactTypeReadingRemoved(arFactTypeReading)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        ''' <summary>
        ''' Removes the Objectification of the FactType
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveObjectification()

            Dim lrEntityType As FBM.EntityType

            lrEntityType = Me.ObjectifyingEntityType
            lrEntityType.IsObjectifyingEntityType = False

            Me.IsObjectified = False
            Me.ObjectifyingEntityType = Nothing

            '------------------------------------------------------------------------------------------------------------
            'Save the FactType because there's a need to remove the ObjectifyingEntityType reference from the FactType.
            '------------------------------------------------------------------------------------------------------------
            Me.Save()

            RaiseEvent ObjectificationRemoved()

            lrEntityType.RemoveFromModel(True)

        End Sub

        ''' <summary>
        ''' Removes a Role from the RoleGroup of the FactType
        ''' </summary>
        ''' <param name="arRole"></param>
        ''' <param name="abRemoveFromDatabase"></param>
        ''' <remarks></remarks>
        Sub RemoveRole(ByRef arRole As FBM.Role, Optional ByVal abRemoveFromDatabase As Boolean = Nothing)
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

                Me.RoleGroup.Remove(arRole)
                Me.Arity -= 1

                '-----------------------------------------------------------------------
                'Remove the Role from any FactTypeReading associated with the FactType
                '-----------------------------------------------------------------------
                Dim lrFactTypeReading As FBM.FactTypeReading
                For Each lrFactTypeReading In Me.FactTypeReading
                    lrFactTypeReading.RemovePredicatePartForRole(arRole)
                Next

                Dim lrRoleConstraint As FBM.RoleConstraint
                For Each lrRoleConstraint In Me.InternalUniquenessConstraint
                    If lrRoleConstraint.Role.Contains(arRole) Then
                        Call lrRoleConstraint.RemoveRoleConstraintRoleByRole(arRole)
                    End If
                Next

                If Me.Arity = 0 Then
                    Call Me.Model.RemoveFactType(Me)
                End If

                '-------------------
                'Model is now Dirty
                '-------------------
                Me.Model.IsDirty = True

                '------------------------------------------
                'Check to see if the Role is to be removed 
                '  from the database
                '------------------------------------------
                If IsSomething(abRemoveFromDatabase) Then
                    If abRemoveFromDatabase Then
                        Call arRole.Delete()
                    End If
                End If

                RaiseEvent RoleRemoved(arRole)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save()
        End Sub

        Public Sub SetDerivationText(ByVal asDerivationText As String)

            Me.DerivationText = asDerivationText
            RaiseEvent DerivationTextChanged(asDerivationText)

        End Sub

        ''' <summary>
        ''' Resets an already existing FactTypeReading
        ''' </summary>
        ''' <param name="arFactTypeReading"></param>
        ''' <remarks></remarks>
        Public Sub SetFactTypeReading(ByRef arFactTypeReading As FBM.FactTypeReading)

            Try
                Dim liIndex As Integer = -1
                liIndex = Me.FactTypeReading.IndexOf(arFactTypeReading)

                If liIndex >= 0 Then
                    Me.FactTypeReading(liIndex) = arFactTypeReading
                    RaiseEvent FactTypeReadingModified(arFactTypeReading)
                Else
                    Throw New Exception("No FactTypeReading found to change.")
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try


        End Sub

        Public Sub SetIsDerived(ByVal abIsDerived As Boolean)

            Me.IsDerived = abIsDerived
            RaiseEvent IsDerivedChanged(abIsDerived)

        End Sub

        Public Sub SetIsObjectified(ByVal abNewIsObjectified As Boolean)

            Me.IsObjectified = abNewIsObjectified
            RaiseEvent IsObjectifiedChanged(abNewIsObjectified)

        End Sub

        Public Sub SetIsStored(ByVal abIsStored As Boolean)

            Me.IsStored = abIsStored
            RaiseEvent IsStoredChanged(abIsStored)

        End Sub



        Sub SetName(ByVal asNewName As String, Optional ByVal abTriggerEvents As Boolean = False)

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

                '---------------------------------------------------------------------------------------------------------
                'The surrogate key (FactType.Id) for the FactType is about to change (to match the name of the FactType)
                '  so update the ModelDictionary entry for the Concept/Symbol (the nominalistic idenity of the FactType)
                '---------------------------------------------------------------------------------------------------------
                If StrComp(Me.Id, asNewName) <> 0 Then
                    '------------------------------------------------------
                    'The new FactType.Name does not match the FactType.Id
                    '------------------------------------------------------
                    Call Me.SwitchConcept(New FBM.Concept(asNewName))

                    'Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.FactType)

                    'If Me.Model.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsBySymbol) Then
                    '    Call Me.Model.UpdateDictionarySymbol(Me.Id, asNewName, pcenumConceptType.FactType)
                    '    Call TableModelDictionary.ModifySymbol(Me.Model, lrDictionaryEntry, asNewName, pcenumConceptType.FactType)
                    'Else                        
                    '    lsMessage = "Tried to modify the Name of a FactType where no Dictionary Entry exists for that FactType."
                    '    lsMessage &= vbCrLf & "Original DictionaryEntry.Symbol: " & Me.Id.ToString
                    '    lsMessage &= vbCrLf & "New DictionaryEntry.Symbol: " & asNewName
                    '    Throw New System.Exception(lsMessage)
                    'End If

                    '------------------------------------------------------------------------------------------
                    'Update the Model(database) immediately. There is no choice. The reason why
                    '  is because the (in-memory) key is changing, so the database must be updated to 
                    '  reflect the new key, otherwise it will not be possible to Update an existing FactType.
                    '------------------------------------------------------------------------------------------
                    Call Me.ModifyKey(asNewName)
                    Me.Id = asNewName
                    Call Me.Update() 'Sets the new Name in the database, TableFactType.ModifyKey (above) only modifies the key.

                    Me.Model.MakeDirty()
                    RaiseEvent Updated()
                    RaiseEvent IdChanged()

                End If 'Me.Id <> asNewName

                If abTriggerEvents Then
                    RaiseEvent Updated()
                    Call Me.Model.MakeDirty(True)
                End If

                '------------------------------------------------------------------------------------
                'Must save the Model because Roles that reference the EntityType must be saved.
                '  NB If Roles are saved, the FactType must be saved. If the FactType is saved,
                '  the RoleGroup's references (per Role) must be saved. A Role within the RoleGroup 
                '  may reference another FactType, so that FactType must be saved...etc.
                '  i.e. It's easier and safer to simply save the whole model.
                '------------------------------------------------------------------------------------
                Me.Model.Save()

            Catch ex As Exception
                lsMessage = "Error: tFactType.SetName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        ''' <summary>
        ''' Calls TableFactType.ModifyKey  i.e. Updates the PrimaryKey of the FactType in the database.
        ''' Implementation specific.
        ''' </summary>
        ''' <param name="asNewName"></param>
        ''' <remarks></remarks>
        Public Overridable Sub ModifyKey(asNewName As String)
        End Sub

        Public Sub SetShowFactTypeName(ByVal abNewShowFactTypeName As Boolean)

            Me.ShowFactTypeName = abNewShowFactTypeName
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

        ''' <summary>
        ''' For database handling. Implementation specific.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Update()
        End Sub

    End Class
End Namespace