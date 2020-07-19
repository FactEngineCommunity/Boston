Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class ModelObject
        Inherits FBM.Concept
        Implements IEquatable(Of FBM.ModelObject)
        Implements ICloneable

        <XmlIgnore()> _
        <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Model As FBM.Model
        <XmlIgnore()> _
        Public Overridable Property Model() As FBM.Model
            Get
                Return Me._Model
            End Get
            Set(ByVal value As FBM.Model)
                Me._Model = value
            End Set
        End Property

        <XmlIgnore()>
        <Browsable(False),
        [ReadOnly](True),
        BindableAttribute(False)>
        Public WithEvents Concept As FBM.Concept  'New 'The Concept that is related to the ModelObject within the Model/ModelDictionary


        <XmlAttribute()> _
        Public ConceptType As pcenumConceptType

        <XmlAttribute()> _
        <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Name As String = ""
        <XmlIgnore()> _
        <CategoryAttribute("Name"), _
        DefaultValueAttribute(GetType(String), ""), _
        DescriptionAttribute("A unique Name for the model object.")> _
        Public Overridable Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                '------------------------------------------------------
                'See Me.SetName for management of Me.Id and Me.Symbol
                '------------------------------------------------------
                _Name = value
            End Set
        End Property

        <XmlAttribute()> _
        Public Id As String = System.Guid.NewGuid.ToString 'The unique Identifier of the ModelObject within the Model.

        Public GUID As String = System.Guid.NewGuid.ToString

        Public _IsAbsorbed As Boolean = False
        Public Property IsAbsorbed As Boolean
            Get
                Return Me._IsAbsorbed
            End Get
            Set(value As Boolean)
                Me._IsAbsorbed = value
            End Set
        End Property

        <XmlIgnore()> _
        <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ShortDescription As String = ""
        <XmlIgnore()> _
        <CategoryAttribute("Description (Informal)"), _
             Browsable(True), _
             [ReadOnly](False), _
             BindableAttribute(True), _
             DefaultValueAttribute(""), _
             DesignOnly(False), _
             DescriptionAttribute("Enter a description."), _
             Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Shadows Property ShortDescription() As String
            Get
                Return _ShortDescription
            End Get
            Set(ByVal Value As String)
                _ShortDescription = Value
            End Set
        End Property

        <XmlIgnore()> _
        <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _LongDescription As String = ""
        <XmlIgnore()> _
        <CategoryAttribute("Description (Informal)"), _
             Browsable(True), _
             [ReadOnly](False), _
             BindableAttribute(True), _
             DefaultValueAttribute(""), _
             DesignOnly(False), _
             DescriptionAttribute("Enter a description."), _
             Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Shadows Property LongDescription() As String
            Get
                Return _LongDescription
            End Get
            Set(ByVal Value As String)
                _LongDescription = Value
            End Set
        End Property

        ''' <summary>
        ''' Only used for Entity Types, Fact Types....those ModelObject ConceptTypes that can be Subtypes | Supertypes.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _parentModelObjectList As New List(Of FBM.ModelObject) 'String = "" '0 -not sub type, otherwise used to store entity_id of super type, if this entity is a subtype
        <XmlIgnore()> _
        <Browsable(False)> _
        Property parentModelObjectList() As List(Of FBM.ModelObject)
            Get
                Return Me._parentModelObjectList
            End Get
            Set(ByVal value As List(Of FBM.ModelObject))
                Me._parentModelObjectList = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _childModelObjectList As New List(Of FBM.ModelObject) 'String = "" '0 -not sub type, otherwise used to store entity_id of super type, if this entity is a subtype
        <XmlIgnore()> _
        <Browsable(False)> _
        Property childModelObjectList() As List(Of FBM.ModelObject)
            Get
                Return Me._childModelObjectList
            End Get
            Set(ByVal value As List(Of FBM.ModelObject))
                Me._childModelObjectList = value
            End Set
        End Property

        ''' <summary>
        ''' Instances of this EntityType as exist as FactData against Roles within FactTypes where those Roles join this EntityType.
        ''' </summary>
        ''' <remarks></remarks>
        Public Instance As New List(Of String)

        ''' <summary>
        ''' Used for hiding or showing property elements.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        <NonSerialized()> _
        Public m_dctd As DynamicTypeDescriptor.DynamicCustomTypeDescriptor

        ''' <summary>
        ''' Only used (at this stage) for generating CQL. Temporarily populated.
        ''' </summary>
        ''' <remarks></remarks>
        <NonSerialized()> _
        Public PreboundReadingText As String = ""


        '<NonSerialized()> _
        <XmlElement()>
        Public SubtypeRelationship As New List(Of FBM.tSubtypeRelationship)

        ''' <summary>
        ''' Only used (at this stage) for generating CQL Temporarily populated.
        ''' </summary>
        ''' <remarks></remarks>
        <NonSerialized()> _
        Public PostboundReadingText As String = ""

        Public Event ConceptSwitched(ByRef arConcept As FBM.Concept)
        Public Event LongDescriptionChanged(ByVal asLongDescription As String)
        Public Event ShortDescriptionChanged(ByVal asShortDescription As String)
        Public Event NameChanged(ByVal asNewName As String)

        ''' <summary>
        ''' Parameterless Constructor.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        Public Sub New(ByVal asSymbol As String, Optional ByVal aiConceptType As pcenumConceptType = Nothing)

            Me.Symbol = Trim(asSymbol)
            Me.Id = Trim(asSymbol)
            Me.Name = Trim(asSymbol)

            If IsSomething(aiConceptType) Then
                Me.ConceptType = aiConceptType
            End If

        End Sub

        Public Overloads Function Equals(ByVal other As FBM.ModelObject) As Boolean Implements IEquatable(Of FBM.ModelObject).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsByName(ByVal other As FBM.ModelObject) As Boolean

            If Me.Name = other.Name Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Overridden by the ModelObject (e.g. ValueType) represented by the ModelObject.
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function EqualsBySignature(ByVal other As FBM.ModelObject) As Boolean

            Return False

        End Function

        Public Overloads Function EqualsBySymbol(ByVal other As FBM.ModelObject) As Boolean

            If Me.Symbol = other.Symbol Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overridable Overloads Function Clone() As Object Implements System.ICloneable.Clone

            Dim lrModelObject As New FBM.ModelObject

            With Me
                lrModelObject.ConceptType = .ConceptType
                lrModelObject.Id = .Id
                lrModelObject.Name = .Name
                lrModelObject.Symbol = .Symbol
                lrModelObject.Instance = .Instance
                lrModelObject.ShortDescription = .ShortDescription
                lrModelObject.LongDescription = .LongDescription

            End With

            Return lrModelObject

        End Function

        Public Overridable Overloads Function Clone(ByRef arModel As FBM.Model, _
                                                    Optional ByVal abAddToModel As Boolean = False, _
                                                    Optional ByVal abIsMDAModelElement As Boolean = False) As Object
            Return New Object

        End Function

        Public Overridable Overloads Function Clone(ByRef arModel As FBM.Model) As FBM.ModelObject

            Dim lrModelObject As New FBM.ModelObject

            With Me
                lrModelObject.Model = arModel
                lrModelObject.ConceptType = .ConceptType
                lrModelObject.Id = .Id
                lrModelObject.Name = .Name
                lrModelObject.Symbol = .Symbol
                lrModelObject.Instance = .Instance
                lrModelObject.ShortDescription = .ShortDescription
                lrModelObject.LongDescription = .LongDescription
            End With

            Return lrModelObject

        End Function

        Public Overridable Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.ModelObject

            Dim lrPageObject As New FBM.PageObject

            Try

                With Me
                    lrPageObject.ConceptType = .ConceptType
                    lrPageObject.Model = arPage.Model
                    lrPageObject.Page = arPage
                    lrPageObject.Id = .Id
                    lrPageObject.ShortDescription = .ShortDescription
                    lrPageObject.LongDescription = .LongDescription
                End With

                Return lrPageObject

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tModelObject.CloneInstance"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try


        End Function

        Public Overridable Function CloneEntityType(ByRef arModel As FBM.Model) As FBM.ModelObject

            Dim lrEntityType As New FBM.EntityType

            With Me
                lrEntityType.ConceptType = pcenumConceptType.EntityType
                lrEntityType.Id = Me.Id
                lrEntityType.Name = Me.Name
                lrEntityType.Model = arModel
                lrEntityType.ShortDescription = .ShortDescription
                lrEntityType.LongDescription = .LongDescription
            End With

            Return lrEntityType

        End Function

        Public Overridable Function CloneValueType(ByRef arModel As FBM.Model) As FBM.ModelObject

            Dim lrValueType As New FBM.ValueType

            With Me
                lrValueType.ConceptType = pcenumConceptType.ValueType
                lrValueType.Id = Me.Id
                lrValueType.Name = Me.Name
                lrValueType.Model = arModel
                lrValueType.ShortDescription = .ShortDescription
                lrValueType.LongDescription = .LongDescription
            End With

            Return lrValueType

        End Function

        Public Overridable Function CloneFactType(ByRef arModel As FBM.Model) As FBM.ModelObject

            Dim lrFactType As New FBM.FactType

            With Me
                lrFactType.ConceptType = pcenumConceptType.FactType
                lrFactType.Id = Me.Id
                lrFactType.Name = Me.Name
                lrFactType.Model = arModel
                lrFactType.ShortDescription = .ShortDescription
                lrFactType.LongDescription = .LongDescription
            End With

            Return lrFactType

        End Function

        Public Function CloneEntityTypeInstance(ByRef arPage As FBM.Page) As FBM.EntityTypeInstance

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

            Try
                With Me
                    lrEntityTypeInstance.Model = arPage.Model
                    lrEntityTypeInstance.Page = arPage
                    lrEntityTypeInstance.Name = .Name
                    lrEntityTypeInstance.Id = Me.Name
                End With

                Return lrEntityTypeInstance

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrEntityTypeInstance
            End Try


        End Function

        Public Function CloneRoleConstraintInstance(ByRef arPage As FBM.Page) As FBM.RoleConstraintInstance

            Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance

            With Me
                lrRoleConstraintInstance.Name = .Name
                lrRoleConstraintInstance.Id = Me.Name
                lrRoleConstraintInstance.Symbol = Me.Name
                lrRoleConstraintInstance.Page = arPage
                lrRoleConstraintInstance.Model = arPage.Model
            End With

            Return lrRoleConstraintInstance


        End Function

        Public Overridable Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        Public Sub changeModel(ByRef arModel As FBM.Model, ByVal abAddToModel As Boolean)

            Me.Model = arModel

        End Sub

        ''' <summary>
        ''' If the EntityType is a Subtype, then returns the topmost Supertype in the hierarchy,
        '''   ELSE returns the EntityType itself.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTopmostSupertype() As FBM.ModelObject

            If Me.parentModelObjectList.Count = 0 Then
                Return Me
            End If

            Return Me.parentModelObjectList(0).GetTopmostSupertype()

        End Function

        ''' <summary>
        ''' Returns the unique Signature of the ModelObject
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Overrided in FBM.EntityType, FBM.Valuetype, FBM.FactType, FBM.RoleConstraint, FBM.ModelNote</remarks>
        Public Overridable Function GetSignature() As String

            Dim lsSignature As String

            lsSignature = Me.Id

            Return lsSignature

        End Function

        Public Overridable Function getSubtypes() As List(Of FBM.ModelObject)
            Return New List(Of ModelObject)
        End Function

        ''' <summary>
        ''' Used for ValueTypes, EntityTypes, Objectified FactTypes. Returns the set of Roles of FactTypes that reference the ModelObject.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function GetAdjoinedRoles(Optional abIgnoreReferenceModeFactTypes As Boolean = False) As List(Of FBM.Role)

            Try
                Dim lrRole As FBM.Role
                Dim larReturnRoles As New List(Of FBM.Role)

                Dim larRoles = From FactType In Me.Model.FactType.FindAll(Function(x) x.IsPreferredReferenceMode = Not abIgnoreReferenceModeFactTypes) _
                               From Role In FactType.RoleGroup _
                               Where Role.JoinedORMObject.Id = Me.Id _
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try


        End Function

        ''' <summary>
        ''' Overridden at the Instance level to return the Model level ModelObject for the Instance.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function getBaseModelObject() As FBM.ModelObject
            Return New FBM.ModelObject
        End Function

        Public Overridable Function getCorrespondingRDSTable() As RDS.Table
            Return New RDS.Table
        End Function

        ''' <summary>
        ''' Used to make saving to the database much quicker. Only hit the database if it is required.
        '''   NB Initially only implemented on Fact/FactInstances/ConceptInstance, which take up the bulk of a CMML enabled model (i.e. e.g. post v4.0 release of Boston).
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub makeDirty()
            Call Me.Model.MakeDirty(False, False)
            Me.isDirty = True

        End Sub

        ''' <summary>
        ''' Removes an Instance for the ModelElement. For EntityTypes, ValueTypes and FactTypes.
        ''' </summary>
        ''' <param name="asInstance"></param>
        ''' <remarks></remarks>
        Public Overridable Sub removeInstance(ByVal asInstance As String)

        End Sub

        ''' <summary>
        ''' Removes an Instance for the ModelElement. For EntityTypes, ValueTypes and FactTypes.
        ''' </summary>
        ''' <param name="asOriginalInstance">The original Instance being renamed/replaced.</param>
        ''' <param name="asNewInstance">The Instance value to replace the original Instance.</param>
        ''' <remarks></remarks>
        Public Overridable Sub renameInstance(ByVal asOriginalInstance As String, ByVal asNewInstance As String)

        End Sub

        Public Overridable Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                    Optional ByVal abCheckForErrors As Boolean = True,
                                                    Optional ByVal abDoDatabaseProcessing As Boolean = True) As Boolean '(ByRef arError As FBM.ModelError) As Boolean
            '----------------------------------------------------
            'Shadowed in tEntityType, tValueType, FBM.tFactType etc
            '----------------------------------------------------
        End Function

        Public Overridable Sub RemoveSubtypeRelationship(ByRef arSubtypeConstraint As FBM.tSubtypeRelationship)

        End Sub

        Public Sub SetPropertyAttributes(ByRef arObject As Object, ByVal asProperty As String, ByVal abIsBrowsable As Boolean)

            Try

                'Me.m_dctd = DynamicTypeDescriptor.ProviderInstaller.Install(Me)
                'Me.m_dctd.PropertySortOrder = DynamicTypeDescriptor.CustomSortOrder.AscendingByName
                'Me.m_dctd.CategorySortOrder = DynamicTypeDescriptor.CustomSortOrder.DescendingByName


                ' now lets modify some attribute of PropA
                Dim cpd As DynamicTypeDescriptor.CustomPropertyDescriptor = Me.m_dctd.GetProperty(asProperty)
                'cpd.SetDisplayName("New display name of PropA")
                'cpd.SetDescription("New description of PropA")
                'cpd.SetCategory("Fact Type")
                cpd.SetIsBrowsable(abIsBrowsable) ';  // hides the property
                cpd.SetIsReadOnly(False) '; // disables the property

                'cpd.CategoryId = 4
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetLongDescription(ByVal asLongDescription As String)
            Me.LongDescription = asLongDescription
            RaiseEvent LongDescriptionChanged(asLongDescription)

            Me.isDirty = True
            Me.Model.MakeDirty(False, False)
        End Sub

        Public Overridable Sub setName(ByVal asNewName As String, Optional ByVal abBroadcastInterfaceEvent As Boolean = True)
            'See inherited Classes.
        End Sub

        Public Sub SetShortDescription(ByVal asShortDescription As String)
            Me.ShortDescription = asShortDescription
            RaiseEvent ShortDescriptionChanged(asShortDescription)

            Me.isDirty = True
            Me.Model.MakeDirty(False, False)
        End Sub

        Public Sub SwitchConcept(ByVal arNewConcept As Concept, ByVal aiConceptType As pcenumConceptType)

            Dim lsOriginalSymbol As String = ""
            Dim lrOriginalDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Concept.Symbol, pcenumConceptType.Value)
            Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.Model, arNewConcept.Symbol, pcenumConceptType.Value)
            Dim lsDebugMessage As String = ""
            Dim lsMessage As String

            Try

                lsOriginalSymbol = Me.Concept.Symbol

                If (lrOriginalDictionaryEntry.Concept.Symbol = lrNewDictionaryEntry.Concept.Symbol) Then
                    '--------------------
                    'Nothing to do here
                    '--------------------
                Else
                    '--------------------------------------------------------
                    'See if the NewSymbol is already in the ModelDictionary
                    '--------------------------------------------------------                    
                    If Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals) IsNot Nothing Then
                        '----------------------------------------------------------------------------------------------------------
                        'The NewConcept exists in the ModelDictionary
                        '  Substitute the existing Concept for a ModelDictionary entry (Concept) that already exists in the Model
                        '----------------------------------------------------------------------------                     
                        lrOriginalDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)

                        If IsSomething(lrOriginalDictionaryEntry) Then
                            Call Me.Model.DeprecateRealisationsForDictionaryEntry(lrOriginalDictionaryEntry, aiConceptType)
                        Else
                            '----------------------------------------------------------------------------------------------------------------------------------
                            'Throw a warning message but do not interupt programme flow.
                            '  We're going to deprecate Realisations for the DictionaryEntry anyway.
                            '----------------------------------------------------------------------------------------------------------------------------------
                            lsMessage = "Original DictionaryEntry for FactData.Concept not found in the ModelDictionary"
                            Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                        End If

                        Me.Concept = lrNewDictionaryEntry.Concept

                        lrNewDictionaryEntry.AddConceptType(Me.ConceptType)
                    Else
                        '--------------------------------------------------------------------------------------------------
                        'The lrNewDictionaryEntry/arNewConcept does not exist in the ModelDictionary.
                        '  Modify the existing DictionaryEntry in the database, effectively updating the ModelDictionary.
                        '  NB PRECONDITION: The DictionaryEntry/Concept being switched is already in the ModelDictionary.
                        '    This will be True in 99.999% of cases.
                        '-------------------------------------------------------------------------
                        '-------------------------------------------------------------
                        'Make sure that the database reflects the new Concept.Symbol
                        '-------------------------------------------------------------
                        arNewConcept.Save()
                        Call TableModelDictionary.ModifySymbol(Me.Model, lrOriginalDictionaryEntry, arNewConcept.Symbol, Me.ConceptType)

                        '-----------------------------------------------------------------------------------------------
                        'Switch the Symbol of the Concept, which effectively changes the existing ModelDictionaryEntry
                        Me.Concept.Symbol = arNewConcept.Symbol
                    End If

                    If Me.Model.Loaded Then Call Me.makeDirty()


                    RaiseEvent ConceptSwitched(Me.Concept)

                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RaiseEventNameChanged(ByVal asNewName As String)

            RaiseEvent NameChanged(asNewName)

        End Sub


    End Class
End Namespace