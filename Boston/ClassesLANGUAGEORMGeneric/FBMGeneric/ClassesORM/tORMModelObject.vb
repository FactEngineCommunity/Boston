Imports System.ComponentModel
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

        <XmlIgnore()> _
        <Browsable(False), _
        [ReadOnly](True), _
        BindableAttribute(False)> _
        Public WithEvents Concept As New FBM.Concept   'The Concept that is related to the ModelObject within the Model/ModelDictionary


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
        <NonSerialized()> _
        Public m_dctd As DynamicTypeDescriptor.DynamicCustomTypeDescriptor

        Public Event LongDescriptionChanged(ByVal asLongDescription As String)
        Public Event ShortDescriptionChanged(ByVal asShortDescription As String)

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


        Public Overridable Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

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

        ''' <summary>
        ''' Used for ValueTypes, EntityTypes, Objectified FactTypes. Returns the set of Roles of FactTypes that reference the ModelObject.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function GetAdjoinedRoles() As List(Of FBM.Role)

            Return Nothing

        End Function

        Public Overridable Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                    Optional ByVal abCheckForErrors As Boolean = True) As Boolean '(ByRef arError As FBM.ModelError) As Boolean
            '----------------------------------------------------
            'Shadowed in tEntityType, tValueType, FBM.tFactType etc
            '----------------------------------------------------
        End Function

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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Sub SetLongDescription(ByVal asLongDescription As String)
            Me.LongDescription = asLongDescription
            RaiseEvent LongDescriptionChanged(asLongDescription)
        End Sub

        Public Sub SetShortDescription(ByVal asShortDescription As String)
            Me.ShortDescription = asShortDescription
            RaiseEvent ShortDescriptionChanged(asShortDescription)
        End Sub


        Public Overridable Sub SwitchConcept(ByVal arNewConcept As Concept)

            '--------------------------------------------------------
            'See if the NewSymbol is already in the ModelDictionary
            '--------------------------------------------------------
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
                    lrNewDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals)

                    If IsSomething(lrNewDictionaryEntry) Then
                        '----------------------------------------------------------------------------
                        'The NewConcept exists in the ModelDictionary
                        '  Substitute the existing Concept for a ModelDictionary entry (Concept) that
                        '  already exists in the Model
                        '----------------------------------------------------------------------------                     
                        ''-----------------------------------------------------------------------------------------------------------
                        ''Set the Symbol/Concept.Symbol values of the NewDictionaryEntry (existing entry), because
                        ''  .Equals may have matched a DictionaryEntry with the same Lowercase string value, but not the same value
                        ''-----------------------------------------------------------------------------------------------------------
                        'lrNewDictionaryEntry.Symbol = arNewConcept.Symbol
                        'lrNewDictionaryEntry.Concept.Symbol = arNewConcept.Symbol

                        lrOriginalDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)

                        If IsSomething(lrOriginalDictionaryEntry) Then
                            Call Me.Model.DeprecateRealisationsForDictionaryEntry(lrOriginalDictionaryEntry)
                        Else
                            '----------------------------------------------------------------------------------------------------------------------------------
                            'Throw a warning message but do not interupt programme flow.
                            '  We're going to deprecate Realisations for the DictionaryEntry anyway.
                            '----------------------------------------------------------------------------------------------------------------------------------
                            lsMessage = "Original DictionaryEntry for FactData.Concept not found in the ModelDictionary"
                            Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, "", False)
                        End If

                        Me.Concept = lrNewDictionaryEntry.Concept
                        lrNewDictionaryEntry.AddConceptType(Me.ConceptType)
                    Else
                        '-------------------------------------------------------------------------
                        'The NewConcept does not exist in the ModelDictionary.
                        '  Modify the existing Concept, effectively updating the ModelDictionary.
                        '-------------------------------------------------------------------------
                        '20150504-VM-Remove this commented out code if all is working well.
                        'Quite obviously causes error on Save if OriginalDictionaryEntry doesn't exist in the ModelDictionary
                        'Seems to serve no purpose...because OriginalDictionaryEntry was not in the dictionary
                        'So why Save it?
                        'lrOriginalDictionaryEntry = New FBM.DictionaryEntry(Me.Model, lsOriginalSymbol, pcenumConceptType.Value)
                        'lrOriginalDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)
                        'lrOriginalDictionaryEntry.Save()
                        '---------------------------------------------------
                        'Make sure that the new Concept is in the database
                        '---------------------------------------------------
                        arNewConcept.Save()
                        Me.Concept.Symbol = arNewConcept.Symbol
                    End If

                    Call Me.Model.MakeDirty()
                End If

            Catch ex As Exception
                lsMessage = "Error: tFactData.Data.Set"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

    End Class
End Namespace