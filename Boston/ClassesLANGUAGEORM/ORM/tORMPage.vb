Imports System.Xml.Serialization
Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports MindFusion.Drawing
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class Page
        Implements IEquatable(Of FBM.Page)

        <XmlAttribute()> _
        Public PageId As String = ""

        ''' <summary>
        ''' Used when copying the Page to the Clipboard so don't paste a Page back on itself.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public CopiedPageId As String = ""

        ''' <summary>
        ''' Used when copying the Page to the Clipboard. Used to determine if signature checking is required when pasting to a Model.
        ''' i.e. It 'is' required to check signatures of pasted ModelObjects when pasting to a different Model. Not required when pasting to the same Model.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public CopiedModelId As String = ""

        <XmlAttribute()> _
        Public IsCoreModelPage As Boolean = False

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Name As String = ""
        <CategoryAttribute("Page"), _
        DescriptionAttribute("The Name of the Page/Diagram.")> _
        Public Overridable Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        <XmlIgnore()> _
        Public UserRejectedSave As Boolean = False

        <XmlIgnore()> _
        Public IsDirty As Boolean = False

        <XmlIgnore()> _
        Public Loaded As Boolean = False

        <XmlIgnore()> _
        Public Loading As Boolean = False

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public WithEvents _Model As FBM.Model
        <XmlIgnore()> _
        Public Overridable Property Model() As FBM.Model
            Get
                Return Me._Model
            End Get
            Set(ByVal value As FBM.Model)
                Me._Model = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _LanguageId As pcenumLanguage = pcenumLanguage.ORMModel 'Default to ORMModel
        <XmlAttribute()>
        <CategoryAttribute("Page"),
        DisplayNameAttribute("Language"),
        DescriptionAttribute("The Language of the Page/Diagram.")>
        Public Overridable Property Language() As pcenumLanguage
            Get
                Return Me._LanguageId
            End Get
            Set(ByVal value As pcenumLanguage)
                Me._LanguageId = value
            End Set
        End Property

        <XmlIgnore()> _
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Page

        <XmlIgnore()> _
        Public FormLoaded As Boolean = False 'TRUE when the form/diagram of the Page is loaded within Richmond (i.e. displayed on the screen).


        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        <NonSerialized(),
        XmlIgnore()>
        Public _Form As Object
        <XmlIgnore()> _
        Public Overridable Property Form() As Object
            Get
                Return Me._Form
            End Get
            Set(ByVal value As Object)
                Me._Form = value
            End Set
        End Property


        <NonSerialized(),
        XmlIgnore()>
        <CategoryAttribute("What the"),
        Browsable(False),
        [ReadOnly](True),
        BindableAttribute(False),
        DefaultValueAttribute(""),
        DesignOnly(True),
        DescriptionAttribute("The Mindfusion Diagram object for the Page.")>
        Public WithEvents Diagram As MindFusion.Diagramming.Diagram

        <NonSerialized(),
        XmlIgnore()>
        <Browsable(False),
        [ReadOnly](False)>
        Public DiagramView As MindFusion.Diagramming.WinForms.DiagramView

        <NonSerialized(),
        XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _SelectedObject As New List(Of Object) 'NB Each SelectedObject will be an 'Instance' type object with X,Y coordinates

        <XmlIgnore()>
        <Browsable(False), _
        [ReadOnly](False)> _
        Public Property SelectedObject() As List(Of Object)
            Get
                If Me._SelectedObject.Count > 1 Then
                    Me.MultiSelectionPerformed = True
                Else
                    Me.MultiSelectionPerformed = False
                End If
                Return Me._SelectedObject
            End Get
            Set(ByVal value As List(Of Object))
                Me._SelectedObject = value
                If Me._SelectedObject.Count > 1 Then
                    Me.MultiSelectionPerformed = True
                Else
                    Me.MultiSelectionPerformed = False
                End If
            End Set
        End Property

        ''' <summary>
        ''' Set to True by Me.Invalidate method. Set to False by frmDiagramORM (UML etc) on completion of .Diagram.DrawBackground.
        '''   NB Used predominantly to refresh the Verbalisation View.
        ''' </summary>
        ''' <remarks></remarks>
        Public IsInvalidated As Boolean = False

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _MultiSelectionPerformed As Boolean = False
        <Browsable(False), _
        [ReadOnly](False)> _
        Public Property MultiSelectionPerformed() As Boolean
            Get
                If Me.SelectedObject.Count > 1 Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._MultiSelectionPerformed = value
            End Set
        End Property


        <NonSerialized(),
        XmlIgnore()>
        Public ModelObject As New List(Of FBM.ModelObject)

        Public EntityTypeInstance As New List(Of FBM.EntityTypeInstance)
        Public ValueTypeInstance As New List(Of FBM.ValueTypeInstance)

        Public FactTypeInstance As New List(Of FBM.FactTypeInstance)

        <NonSerialized(),
        XmlIgnore()>
        Public RoleInstance As New List(Of FBM.RoleInstance)
        Public RoleConstraintInstance As New List(Of FBM.RoleConstraintInstance)
        <NonSerialized()>
        Public FactInstance As New List(Of FBM.FactInstance)
        <NonSerialized()>
        Public ValueInstance As New List(Of FBM.FactDataInstance)
        Public ModelNoteInstance As New List(Of FBM.ModelNoteInstance)

        Public SubtypeRelationship As New List(Of FBM.SubtypeRelationshipInstance)

        <XmlIgnore()> _
        Public ModelNote As New List(Of FBM.ModelNoteInstance)

        <NonSerialized()> _
        <XmlIgnore()> _
        Public ReferencedForm As WeifenLuo.WinFormsUI.Docking.DockContent

        <XmlIgnore()> _
        Public InternalUniquenessConstraintsExpanded As Boolean = False 'For use with ORM Models only: Toggle indicating whether 
        'the size of the InternalUniquenessConstraints is expanded or contracted so that users can click on them

        <XmlIgnore()> _
        Public ShowFacts As Boolean = False

        <NonSerialized()> _
        Private _ERDiagram As New ERD.Diagram
        Public Overridable Property ERDiagram As ERD.Diagram
            Get
                Return Me._ERDiagram
            End Get
            Set(value As ERD.Diagram)
                Me._ERDiagram = value
            End Set
        End Property

        <NonSerialized()>
        Private _STDiagram As New STD.Diagram(Me)
        Public Overridable Property STDiagram As STD.Diagram
            Get
                Return Me._STDiagram
            End Get
            Set(value As STD.Diagram)
                Me._STDiagram = value
            End Set
        End Property

        '----------------------------------------------------------------------------------


        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, Optional ByVal as_PageId As String = Nothing, Optional ByVal as_page_name As String = Nothing, Optional ByVal aiLanguageId As pcenumLanguage = Nothing)

            Me.Model = arModel
            Me.RDSModel = arModel.RDS
            Me.STModel = arModel.STM

            If IsSomething(as_PageId) Then
                Me.PageId = as_PageId
            Else
                Me.PageId = System.Guid.NewGuid.ToString
            End If

            If IsSomething(as_page_name) Then
                Me.Name = as_page_name
            Else
                Me.Name = "New Model Page"
            End If

            If IsSomething(aiLanguageId) Then
                Me.Language = aiLanguageId
            Else
                '---------------------
                'Default to ORM Model
                '---------------------
                Me.Language = pcenumLanguage.ORMModel
            End If

            Me.IsDirty = True

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.Page) As Boolean Implements System.IEquatable(Of FBM.Page).Equals

            If Me.PageId = other.PageId Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByName(ByVal other As FBM.Page) As Boolean

            If Me.Name = other.Name Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsLanguage(ByVal other As FBM.Page) As Boolean

            If Me.Language = other.Language Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Makes a copy/clone of the Page. Every ModelElement is the same except for the Model and Page that it is related to.
        ''' </summary>
        ''' <param name="arModel">The Model to which the cloned Page is allocated.</param>
        ''' <returns>A new Page object (as clone of the Page being cloned).</returns>
        ''' <remarks></remarks>
        Public Function Clone(ByRef arModel As FBM.Model, _
                              Optional ByVal abAddToModel As Boolean = False, _
                              Optional ByVal abMakeModelObjectsMDAModelElements As Boolean = False,
                              Optional ByVal abSetRDSModel As Boolean = True, _
                              Optional ByVal abMakeDirty As Boolean = False) As FBM.Page

            Dim lrPage As New FBM.Page
            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            Dim lrValueTypeInstance As FBM.ValueTypeInstance
            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
            Dim lrModelNoteInstance As FBM.ModelNoteInstance
            Dim lrDictionaryEntry As FBM.DictionaryEntry

            Try


                With Me
                    lrPage.Model = arModel
                    If abSetRDSModel Then
                        lrPage.RDSModel = arModel.RDS
                    End If
                    lrPage.STModel = arModel.STM
                    lrPage.PageId = System.Guid.NewGuid.ToString
                    lrPage.Name = Me.Name
                    lrPage.ConceptType = .ConceptType
                    lrPage.Language = .Language
                    lrPage.IsCoreModelPage = .IsCoreModelPage
                    lrPage.Diagram = Nothing
                    lrPage.DiagramView = Nothing
                    lrPage.Form = Nothing
                    lrPage.FormLoaded = False
                    lrPage.IsDirty = abMakeDirty

                    '---------------------------------------------------
                    'Clone the DictionaryEntries from the Copied Model
                    '---------------------------------------------------
                    Call Richmond.WriteToStatusBar("Cloning ModelDictionary entries", True, 5)
                    For Each lrDictionaryEntry In Me.Model.ModelDictionary
                        Dim lrClonedDictionaryEntry As New FBM.DictionaryEntry
                        lrClonedDictionaryEntry = lrDictionaryEntry.Clone(arModel)
                        If arModel.ModelDictionary.Exists(AddressOf lrClonedDictionaryEntry.Equals) Then
                            '-----------------------------------------------
                            'No need to add a clone of the DictionaryEntry
                            '-----------------------------------------------
                        Else
                            arModel.ModelDictionary.Add(lrClonedDictionaryEntry)
                        End If
                    Next

                    '----------------------
                    'Clone the ValueTypes
                    '----------------------
                    Call Richmond.WriteToStatusBar("Cloning ValueTypes", True, 32)
                    For Each lrValueTypeInstance In .ValueTypeInstance
                        lrPage.ValueTypeInstance.AddUnique(lrValueTypeInstance.Clone(lrPage, abMakeModelObjectsMDAModelElements))
                    Next

                    '-----------------------
                    'Clone the EntityTypes
                    '-----------------------
                    Call Richmond.WriteToStatusBar("Cloning EntityTypes", True, 16)
                    For Each lrEntityTypeInstance In .EntityTypeInstance
                        lrPage.EntityTypeInstance.AddUnique(lrEntityTypeInstance.Clone(lrPage, abAddToModel, abMakeModelObjectsMDAModelElements))
                    Next

                    '----------------------
                    'Clone the FactTypes
                    '----------------------
                    Call Richmond.WriteToStatusBar("Cloning FactTypes", True, 48)
                    For Each lrFactTypeInstance In .FactTypeInstance
                        If lrPage.FactTypeInstance.Exists(AddressOf lrFactTypeInstance.Equals) Then
                            '--------------------------------------------------------------------------------------------------
                            'The FactTypeInstance was most likely added to the Page as part of recursive Cloning.
                            '  When a RoleInstance, in a FactTypeInstance, refers to a FactTypeInstance that is not already
                            '  added to a Page, RoleInstance.Clone adds a clone of the referred FactTypeInstance to the Page.
                            '--------------------------------------------------------------------------------------------------
                        Else
                            lrFactTypeInstance.Clone(lrPage, True, abMakeModelObjectsMDAModelElements)
                        End If
                    Next

                    '---------------------------
                    'Clone the RoleConstraints
                    '---------------------------
                    Call Richmond.WriteToStatusBar("Cloning RoleConstraints", True, 64)
                    For Each lrRoleConstraintInstance In .RoleConstraintInstance
                        lrPage.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance.Clone(lrPage, abMakeModelObjectsMDAModelElements))
                    Next

                    '----------------------
                    'Clone the ModelNotes
                    '----------------------
                    Call Richmond.WriteToStatusBar("Cloning ModelNotes", True, 80)
                    For Each lrModelNoteInstance In .ModelNote
                        lrPage.ModelNote.Add(lrModelNoteInstance.Clone(lrPage))
                    Next

                    '---------------------------------
                    'Clone the SubtypeRelationships.
                    '---------------------------------
                    Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance
                    For Each lrEntityTypeInstance In .EntityTypeInstance.FindAll(Function(x) x.SubtypeRelationship.Count > 0)
                        For Each lrSubtypeRelationshipInstance In lrEntityTypeInstance.SubtypeRelationship
                            '---------------------------------------------------------------------------------------------
                            'Find the EntityTypeInstance on the Page to add the clone of the SubtypeRelationshipInstance
                            '---------------------------------------------------------------------------------------------
                            Dim lrSubtypeEntityTypeInstance As New FBM.EntityTypeInstance
                            lrSubtypeEntityTypeInstance.Id = lrSubtypeRelationshipInstance.EntityType.Id
                            lrSubtypeEntityTypeInstance = lrPage.EntityTypeInstance.Find(AddressOf lrSubtypeEntityTypeInstance.Equals)
                            lrSubtypeEntityTypeInstance.SubtypeRelationship.Add(lrSubtypeRelationshipInstance.Clone(lrPage, True))
                        Next
                    Next


                    '--------------------------------------
                    'Flush unused ModelDictionary entries
                    '--------------------------------------
                    For Each lrDictionaryEntry In lrPage.Model.ModelDictionary.FindAll(Function(x) (x.Realisations.Count = 0) And Not x.isGeneralConcept)
                        lrPage.Model.RemoveDictionaryEntry(lrDictionaryEntry, True)
                    Next

                End With

                Call Richmond.WriteToStatusBar("Pasted Page: '" & lrPage.Name & "'", True)

                Return lrPage

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tPage.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrPage
            End Try

        End Function

        Public Sub CreateRingConstraint(ByRef arFactTypeInstance As FBM.FactTypeInstance)

            Dim lrRoleConstraint As FBM.RoleConstraint

            Try

                '---------------------------
                'Create the RoleConstraint
                '---------------------------
                lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.RingConstraint, _
                                                                 arFactTypeInstance.FactType.RoleGroup, _
                                                                 "RingConstraint", _
                                                                 0)
                lrRoleConstraint.RingConstraintType = pcenumRingConstraintType.Acyclic
                Dim lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(Me, True)

                lrRoleConstraintInstance.X = arFactTypeInstance.X + 5
                lrRoleConstraintInstance.Y = Viev.Greater(arFactTypeInstance.Y - 10, 0)
                Call lrRoleConstraintInstance.DisplayAndAssociate()

            Catch ex As Exception
                MsgBox("Error: Page.AddRingConstraint: " & ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' Adds a ModelObject to the list of SelectedObjects for the Page
        ''' </summary>
        ''' <param name="arModelObject">The ModelObject to add to the list</param>
        ''' <remarks></remarks>
        Public Sub AddSelectedObject(ByRef arModelObject As FBM.ModelObject)

            Dim lrObject As Object
            Dim lrModelObject As FBM.ModelObject
            Dim lbFound As Boolean = False

            For Each lrObject In Me.SelectedObject.ToArray
                lrModelObject = lrObject
                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType, _
                              pcenumConceptType.ValueType, _
                              pcenumConceptType.FactType, _
                              pcenumConceptType.RoleConstraint
                        If lrModelObject.Id = arModelObject.Id Then
                            lbFound = True
                        End If
                    Case Is = pcenumConceptType.RoleConstraintRole
                        If lrModelObject Is arModelObject Then
                            lbFound = True
                        End If
                End Select
            Next

            If Not lbFound Then
                Me.SelectedObject.Add(arModelObject)
            End If

        End Sub

        Function AreNoSelectedObjectsRoles() As Boolean

            Dim liInd As Integer

            AreNoSelectedObjectsRoles = True

            For liInd = 1 To Me.SelectedObject.Count
                If Me.SelectedObject(liInd - 1).ConceptType = pcenumConceptType.Role Then
                    AreNoSelectedObjectsRoles = False
                End If
            Next liInd

        End Function

        Function are_all_SelectedObjects_entity_types() As Boolean

            Dim lr_SelectedObject As Object

            are_all_SelectedObjects_entity_types = True

            For Each lr_SelectedObject In Me.SelectedObject
                Select Case lr_SelectedObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        '-----------
                        'All Good.
                        '-----------
                    Case Else
                        are_all_SelectedObjects_entity_types = False
                End Select
            Next

        End Function

        Public Function areSelectedObjectsEntityTypeAndFactType() As Boolean

            Dim lrModelObject1 As FBM.ModelObject
            Dim lrModelObject2 As FBM.ModelObject

            If Me.SelectedObject.Count > 1 Then
                lrModelObject1 = Me.SelectedObject(0)
                lrModelObject2 = Me.SelectedObject(1)

                If lrModelObject1.ConceptType = pcenumConceptType.EntityType And _
                   lrModelObject2.ConceptType = pcenumConceptType.FactType Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' RETURNS TRUE if all the SelectedObjects are role type objects otherwise it returns FALSE
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AreAllSelectedObjectsRoles() As Boolean

            '-----------------------------------------------
            'PSEUDOCODE
            '  * Set initial value to TRUE
            '  * For every selected object
            '    * IF the object_type is not a role type then
            '      Set the result to FALSE
            '    * END if
            '  * Loop
            '-----------------------------------------------

            Dim liInd As Integer

            AreAllSelectedObjectsRoles = True

            If (Me.SelectedObject.Count < 2) Or (Me.SelectedObject.Count = 0) Then
                AreAllSelectedObjectsRoles = False
                If (Me.SelectedObject.Count = 1) Then
                    If (Me.SelectedObject(0).ConceptType = pcenumConceptType.Role) Then
                        AreAllSelectedObjectsRoles = True
                    End If
                End If

            Else
                For liInd = 1 To Me.SelectedObject.Count
                    If Not (Me.SelectedObject(liInd - 1).ConceptType = pcenumConceptType.Role) Then
                        AreAllSelectedObjectsRoles = False
                    End If
                Next liInd
            End If

        End Function

        Public Function AreAllSelectedRolesJoinedToTheSameModelObject() As Boolean

            AreAllSelectedRolesJoinedToTheSameModelObject = True

            Dim liInd As Integer
            Dim liFirstModelObjectId As String = ""

            If Me.SelectedObject.Count = 0 Then
                AreAllSelectedRolesJoinedToTheSameModelObject = False
                Exit Function
            ElseIf Me.SelectedObject.Count > 1 Then

                For liInd = 1 To Me.SelectedObject.Count
                    If Me.SelectedObject(liInd - 1).ConceptType = pcenumConceptType.Role Then
                        liFirstModelObjectId = Me.SelectedObject(liInd - 1).JoinedORMObject.Id
                        Exit For
                    End If

                Next liInd



                For liInd = 1 To Me.SelectedObject.Count
                    If Me.SelectedObject(liInd - 1).ConceptType = pcenumConceptType.Role Then
                        If (liFirstModelObjectId = Me.SelectedObject(liInd - 1).JoinedORMObject.Id) Then
                        Else
                            AreAllSelectedRolesJoinedToTheSameModelObject = False
                        End If
                    Else
                        AreAllSelectedRolesJoinedToTheSameModelObject = False
                    End If
                Next liInd
            End If


        End Function

        Function are_all_selected_roles_within_the_same_FactType() As Boolean

            Dim liInd As Integer
            Dim liFirstFactTypeId As String = ""

            If Me.SelectedObject.Count = 0 Then
                are_all_selected_roles_within_the_same_FactType = False
                Exit Function
            Else
                are_all_selected_roles_within_the_same_FactType = True
            End If

            For liInd = 1 To Me.SelectedObject.Count
                If Me.SelectedObject(liInd - 1).ConceptType = pcenumConceptType.Role Then
                    liFirstFactTypeId = Me.SelectedObject(liInd - 1).factType.Id
                    Exit For
                End If
            Next liInd


            For liInd = 1 To Me.SelectedObject.Count
                If Me.SelectedObject(liInd - 1).ConceptType = pcenumConceptType.Role Then
                    If Not (liFirstFactTypeId = Me.SelectedObject(liInd - 1).factType.Id) Then
                        are_all_selected_roles_within_the_same_FactType = False
                    End If
                End If
            Next liInd

        End Function

        Public Function AreSelectedObjectsMultipleObjectTypes() As Boolean

            Dim laiObjectTypes As Integer() = {pcenumConceptType.EntityType, _
                                               pcenumConceptType.ValueType, _
                                               pcenumConceptType.FactType}

            Return Me.SelectedObject.FindAll(Function(x) laiObjectTypes.Contains(x.ConceptType)).Count > 1

        End Function

        Public Sub delete()
            '------------------------------------------------------------
            'Deletes a Page and all the ConceptInstances associated 
            '  with that Page from the database
            '------------------------------------------------------------

            '-------------------------------------------------------------------------
            'Start a database Transaction, so that:
            '  a. The set of transactions that follow are handled as one transaction
            '  b. We can RollBack if necessary
            '-------------------------------------------------------------------------
            pdbConnection.BeginTrans()

            '-----------------------------------
            'Delete ConceptInstances for a Page
            '-----------------------------------
            Call TableConceptInstance.DeleteConceptInstancesForPage(Me)

            Call TablePage.DeletePage(Me)

            '-------------------------------------------------------------------------
            'Commit the database Transaction.
            '-------------------------------------------------------------------------
            pdbConnection.CommitTrans()

        End Sub

        Function DropEntityTypeAtPoint(ByRef arEntityType As FBM.EntityType,
                                       ByVal ao_pt As PointF,
                                       Optional ByVal abBroadcastInterfaceEvent As Boolean = False) As FBM.EntityTypeInstance

            Dim lrEntityType As New FBM.EntityType
            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

            Try

                If Me.Diagram IsNot Nothing Then
                    Me.DiagramView.Cursor = Cursors.WaitCursor
                    Me.Diagram.Invalidate()
                End If


                '----------------------------------------------------------------------
                'Clone the EntityType so that it is established for the target Model.
                '----------------------------------------------------------------------
                If Me.Model.ModelId <> arEntityType.Model.ModelId Then
                    lrEntityType = arEntityType.Clone(Me.Model, False)
                Else
                    lrEntityType = arEntityType
                End If


                '----------------------------------------------------------------------------------------
                'Create a ConceptInstance that can be broadcast to other ClientServer Boston instances.
                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, lrEntityType.Id, pcenumConceptType.EntityType)
                lrConceptInstance.X = ao_pt.X
                lrConceptInstance.Y = ao_pt.Y

                If Me.Model.EntityType.Exists(AddressOf arEntityType.Equals) Then
                    '-----------------------------------------------------------------------------------------------------------------------
                    'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                    '  so we do the Client/Server broadcast processing here.
                    If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                        Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                            lrEntityType,
                                                                            lrConceptInstance)
                    End If
                Else
                    '--------------------------------------------------------------
                    'The EntityType is not already within the ORMModel so add it.
                    '--------------------------------------------------------------
                    Me.Model.AddEntityType(lrEntityType, True, True, lrConceptInstance)
                End If

                lrEntityTypeInstance = New FBM.EntityTypeInstance
                lrEntityTypeInstance = lrEntityType.CloneInstance(Me, True)

                lrEntityTypeInstance.X = ao_pt.X
                lrEntityTypeInstance.Y = ao_pt.Y

                If Me.DiagramView IsNot Nothing Then

                    Call lrEntityTypeInstance.DisplayAndAssociate()

                    If IsSomething(lrEntityTypeInstance.ReferenceModeValueType) Then

                        lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me, lrEntityTypeInstance.ReferenceModeValueType.Id, pcenumConceptType.ValueType)
                        lrConceptInstance.X = lrEntityTypeInstance.ReferenceModeValueType.X
                        lrConceptInstance.Y = lrEntityTypeInstance.ReferenceModeValueType.Y

                        If Me.Model.ValueType.Exists(AddressOf lrEntityTypeInstance.ReferenceModeValueType.ValueType.Equals) Then
                            '-----------------------------------------------------------------------------------------------------------------------
                            'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                            '  so we do the Client/Server broadcast processing here.
                            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                                    lrEntityTypeInstance.ReferenceModeValueType.ValueType,
                                                                                    lrConceptInstance)
                            End If
                        Else
                            '--------------------------------------------------------------
                            'The ValueType is not already within the ORMModel so add it.
                            '--------------------------------------------------------------
                            Me.Model.AddValueType(lrEntityTypeInstance.ReferenceModeValueType.ValueType, True, True, lrConceptInstance)
                        End If

                        Call lrEntityTypeInstance.ReferenceModeValueType.DisplayAndAssociate()
                    End If

                    If IsSomething(lrEntityTypeInstance.ReferenceModeFactType) Then

                        lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me, lrEntityTypeInstance.ReferenceModeFactType.Id, pcenumConceptType.FactType)
                        lrConceptInstance.X = lrEntityTypeInstance.ReferenceModeFactType.X
                        lrConceptInstance.Y = lrEntityTypeInstance.ReferenceModeFactType.Y

                        If Me.Model.FactType.Exists(AddressOf lrEntityTypeInstance.ReferenceModeFactType.FactType.Equals) Then
                            '-----------------------------------------------------------------------------------------------------------------------
                            'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                            '  so we do the Client/Server broadcast processing here.
                            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                                    lrEntityTypeInstance.ReferenceModeFactType.FactType,
                                                                                    lrConceptInstance)
                            End If
                        Else
                            '--------------------------------------------------------------
                            'The FactType is not already within the ORMModel so add it.
                            '--------------------------------------------------------------
                            Me.Model.AddFactType(lrEntityTypeInstance.ReferenceModeFactType.FactType, True, True, lrConceptInstance)
                        End If

                        Call lrEntityTypeInstance.ReferenceModeFactType.DisplayAndAssociate()

                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                        For Each lrRoleConstraintInstance In lrEntityTypeInstance.ReferenceModeFactType.InternalUniquenessConstraint

                            lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me, lrRoleConstraintInstance.Id, pcenumConceptType.RoleConstraint)
                            lrConceptInstance.X = lrEntityTypeInstance.ReferenceModeRoleConstraint.X
                            lrConceptInstance.Y = lrEntityTypeInstance.ReferenceModeRoleConstraint.Y

                            If Me.Model.RoleConstraint.Exists(AddressOf lrRoleConstraintInstance.RoleConstraint.Equals) Then
                                '-----------------------------------------------------------------------------------------------------------------------
                                'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                                '  so we do the Client/Server broadcast processing here.
                                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                                        lrRoleConstraintInstance.RoleConstraint,
                                                                                        lrConceptInstance)
                                End If
                            Else
                                '--------------------------------------------------------------
                                'The RoleConstraint is not already within the ORMModel so add it.
                                '--------------------------------------------------------------
                                Me.Model.AddRoleConstraint(lrRoleConstraintInstance.RoleConstraint, True, True, lrConceptInstance)
                                If lrRoleConstraintInstance.RoleConstraint.IsPreferredIdentifier Then
                                    'Creates the RDS PrimaryKey Index
                                    Call lrRoleConstraintInstance.RoleConstraint.SetIsPreferredIdentifier(True)
                                End If
                            End If

                            lrRoleConstraintInstance.DisplayAndAssociate()
                        Next
                    End If

                    If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                        If arEntityType.HasSimpleReferenceScheme Then
                            Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateEntityType, arEntityType, Nothing)
                        End If
                    End If

                    Me.Diagram.Invalidate()

                    If Me.FormLoaded Then
                        Call Me.Form.EnableSaveButton()
                    End If

                    Me.DiagramView.Cursor = Cursors.Default
                End If

                Call Me.MakeDirty()

                Return lrEntityTypeInstance

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tPage.DropEntityTypeAtPoint: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrEntityTypeInstance
            End Try

        End Function

        Public Function DropFactTypeAtPoint(ByRef arFactType As FBM.FactType,
                                            ByVal ao_pt As PointF,
                                            ByVal abDisplayFactTable As Boolean,
                                            Optional ByVal abCloneFactType As Boolean = True,
                                            Optional ByVal abBroadcastInterfaceEvent As Boolean = False,
                                            Optional ByVal abExpandIfReferenceModeFactType As Boolean = False) As FBM.FactTypeInstance

            Dim lrRoleInstance As New FBM.RoleInstance
            Dim lrFactTypeInstance As New FBM.FactTypeInstance
            Dim lrFactType As New FBM.FactType
            Dim lrDroppedFactTypeInstance As New FBM.FactTypeInstance 'Used if arFactType is actually a FactTypeInstance (see Copy/Paste functions)

            'Return failsafe
            DropFactTypeAtPoint = Nothing

            lrFactType = arFactType
            'CodeSafe: Check to see if the FactType is already on the Page
            If Me.FactTypeInstance.Find(Function(x) x.Id = lrFactType.Id) IsNot Nothing Then Exit Function


            If Me.Diagram IsNot Nothing Then
                Me.DiagramView.Cursor = Cursors.WaitCursor
                Me.Diagram.Invalidate()
            End If

            Try
                '--------------------------------------------------------------------------------
                'Check to see if the ModelObjects joined by the FactType are loaded on the Page
                '--------------------------------------------------------------------------------
                If arFactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count = arFactType.Arity Then
                    '--------------------------------------------------------------------------------
                    'User is dropping a FactType with no joined ModelObjects
                    '  - As in when the User drags a BinaryFactType from the Toolbox
                    '--------------------------------------------------------------------------------
                ElseIf Me.AreObjectTypesLoadedForFactType(arFactType) Then
                    '--------------------------------------------------------------
                    'No need to load the ObjectTypes for the FactType because they
                    '  are already loaded on the Page. Just make sure their shapes are visible.
                    '--------------------------------------------------------------
                Else
                    '--------------------------------------------------------------
                    'Find out which ObjectTypes need loading on the Page, for the
                    '  FactType, and load them.
                    '--------------------------------------------------------------            
                    Dim lrModelObject As Object
                    Dim lrRole As FBM.Role
                    Dim loPoint As New PointF(0, 0)
                    Dim liInd As Integer = 0

                    For Each lrRole In arFactType.RoleGroup

                        liInd += 1

                        lrModelObject = lrRole.JoinedORMObject

                        ' If liInd = 1 Then
                        loPoint.X = CInt(Math.Floor((100 - 10 + 1) * Rnd())) + 10
                        loPoint.Y = CInt(Math.Floor((100 - 10 + 1) * Rnd())) + 10
                        'Else
                        '    Dim abEmptySpaceFound As Boolean = False
                        '    loPoint = Me.FindBlankSpaceInRelationToModelObject(arFactType.RoleGroup(liInd - 2).JoinedORMObject, abEmptySpaceFound)
                        '    If Not abEmptySpaceFound Then
                        '        loPoint.X = CInt(Math.Floor((100 - 10 + 1) * Rnd())) + 10
                        '        loPoint.Y = CInt(Math.Floor((100 - 10 + 1) * Rnd())) + 10
                        '    End If
                        'End If

                        Select Case lrModelObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                Dim lrEntityType As New FBM.EntityType
                                lrEntityType = lrModelObject
                                '----------------------------------------------------------------------
                                'Change the Model of the EntityType to the Model of the current Page.
                                '  The reason for this is that the EntityType may have been dragged
                                '  from the ModelDictionary of another Model.
                                '----------------------------------------------------------------------
                                lrEntityType.Model = Me.Model

                                If Not IsSomething(Me.EntityTypeInstance.Find(AddressOf lrEntityType.Equals)) Then
                                    Dim lrEntityTypeInstance = Me.DropEntityTypeAtPoint(lrEntityType, loPoint)

                                    For Each lrSubtypeModelObject As FBM.ModelObject In lrEntityTypeInstance.EntityType.getSubtypes

                                        If lrSubtypeModelObject.ConceptType = pcenumConceptType.EntityType Then
                                            lrEntityType = CType(lrSubtypeModelObject, FBM.EntityType)
                                            Try
                                                Dim lrSubtypeEntityTypeInstance As FBM.EntityTypeInstance = Me.getModelElementById(lrEntityType.Id)
                                                If lrSubtypeEntityTypeInstance IsNot Nothing Then
                                                    Call lrSubtypeEntityTypeInstance.showSubtypeRelationships()
                                                End If
                                            Catch ex As Exception
                                                Dim lsMessage1 As String
                                                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                                                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                                                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                                                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
                                            End Try
                                        End If
                                    Next

                                End If
                            Case Is = pcenumConceptType.ValueType
                                Dim lrValueType As New FBM.ValueType
                                lrValueType = lrModelObject
                                '----------------------------------------------------------------------
                                'Change the Model of the ValueType to the Model of the current Page.
                                '  The reason for this is that the ValueType may have been dragged
                                '  from the ModelDictionary of another Model.
                                '----------------------------------------------------------------------
                                lrValueType.Model = Me.Model

                                Dim lrValueTypeInstance As FBM.ValueTypeInstance = Me.ValueTypeInstance.Find(AddressOf lrValueType.Equals)
                                If lrValueTypeInstance Is Nothing Then
                                    Call Me.DropValueTypeAtPoint(lrValueType, loPoint)
                                ElseIf arFactType.IsPreferredReferenceMode Then
                                    lrValueTypeInstance.Shape.Visible = False
                                End If
                            Case Is = pcenumConceptType.FactType
                                Dim lrRoleJoinedFactType As New FBM.FactType
                                lrRoleJoinedFactType = lrModelObject
                                '----------------------------------------------------------------------
                                'Change the Model of the FactType to the Model of the current Page.
                                '  The reason for this is that the FactType may have been dragged
                                '  from the ModelDictionary of another Model.
                                '----------------------------------------------------------------------
                                lrRoleJoinedFactType.Model = Me.Model

                                If Me.FactTypeInstance.Find(AddressOf lrRoleJoinedFactType.Equals) Is Nothing Then
                                    Call Me.DropFactTypeAtPoint(lrRoleJoinedFactType, loPoint, False)
                                End If

                        End Select
                    Next
                End If

                '-----------------------------------------------------------------------------------
                'Check/DoubleCheck to see whether the FactType references a ValueType and where the FactType is a ReferenceModeFactType, so that we can hide the ValueType
                If arFactType.IsPreferredReferenceMode Then
                    Dim lrValueType As FBM.ValueType = arFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.ConceptType = pcenumConceptType.ValueType).JoinedORMObject
                    Dim lrValueTypeInstance As FBM.ValueTypeInstance = Me.ValueTypeInstance.Find(Function(x) x.Id = lrValueType.Id)
                    If lrValueTypeInstance.Shape IsNot Nothing Then
                        If lrValueTypeInstance.Shape.OutgoingLinks.Count = 0 Then
                            lrValueTypeInstance.Shape.Visible = False
                        End If
                    End If
                End If

                '------------------------------------------
                'Clone the FactType to a FactTypeInstance 
                '------------------------------------------
                Dim lrConceptInstance As FBM.ConceptInstance
                If TypeOf (arFactType) Is FBM.FactTypeInstance Then
                    lrDroppedFactTypeInstance = arFactType
                    lrFactTypeInstance = lrDroppedFactTypeInstance.Clone(Me, True)
                    lrFactType = lrFactTypeInstance.FactType
                Else

                    If abCloneFactType Then
                        lrFactType = arFactType.Clone(arFactType.Model)
                    Else
                        lrFactType = arFactType
                    End If

                    If lrFactType.IsObjectified Then
                        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                        lrEntityTypeInstance = Me.DropEntityTypeAtPoint(lrFactType.ObjectifyingEntityType, New PointF(10, 10))
                    End If

                    lrFactTypeInstance = arFactType.CloneInstance(Me, True)

                    '----------------------------------------------------------------------------------------
                    'Create a ConceptInstance that can be broadcast to other ClientServer Boston instances.
                    lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me, lrFactType.Id, pcenumConceptType.FactType)
                    lrConceptInstance.X = ao_pt.X
                    lrConceptInstance.Y = ao_pt.Y

                    '------------------------------------------------------------
                    'Check if the FactType is already loaded within the ORMModel 
                    If lrFactType.Model.FactType.Exists(AddressOf arFactType.Equals) Then
                        '---------------------------------------------
                        'The FactType is already within the ORMModel 
                        '---------------------------------------------
                        'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                        '  so we do the Client/Server broadcast processing here.
                        If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                            Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                                lrFactType,
                                                                                lrConceptInstance)
                        End If
                    Else
                        '------------------------------------------------------------
                        'The FactType is not already within the ORMModel so add it.
                        '------------------------------------------------------------
                        Me.Model.AddFactType(lrFactType, True, True, lrConceptInstance)
                    End If
                End If

                '------------------------------------------------------
                'Create and display the shape for the FactTypeInstance
                '------------------------------------------------------
                lrFactTypeInstance.X = ao_pt.X
                lrFactTypeInstance.Y = ao_pt.Y
                lrFactTypeInstance.isDirty = True
                lrFactTypeInstance.Page.IsDirty = True

                If Me.Diagram IsNot Nothing Then
                    Call lrFactTypeInstance.DisplayAndAssociate(abDisplayFactTable, My.Settings.ShowFactTypeNamesOnORMModelLoad)

                    '-------------------------------------------
                    'Display the InternalUniquenessConstraints
                    '-------------------------------------------                    
                    For Each lrRoleConstraintInstance In lrFactTypeInstance.InternalUniquenessConstraint

                        lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me, lrRoleConstraintInstance.Id, pcenumConceptType.RoleConstraint)
                        lrConceptInstance.X = lrRoleConstraintInstance.X
                        lrConceptInstance.Y = lrRoleConstraintInstance.Y

                        If Me.Model.RoleConstraint.Exists(AddressOf lrRoleConstraintInstance.RoleConstraint.Equals) Then

                            'Make sure the RoleConstraint is in the ModelDictionary
                            Call Me.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me.Model,
                                                                                          lrRoleConstraintInstance.RoleConstraint.Id,
                                                                                          pcenumConceptType.RoleConstraint),
                                                                                          ,
                                                                                          True)

                            '-----------------------------------------------------------------------------------------------------------------------
                            'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                            '  so we do the Client/Server broadcast processing here.
                            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                                    lrRoleConstraintInstance.RoleConstraint,
                                                                                    lrConceptInstance)
                            End If
                        Else
                            '--------------------------------------------------------------
                            'The RoleConstraint is not already within the ORMModel so add it.
                            '--------------------------------------------------------------
                            Me.Model.AddRoleConstraint(lrRoleConstraintInstance.RoleConstraint, True, True, lrConceptInstance)
                        End If

                        '-----------------------------------
                        'Load InternalUniquenessConstraint
                        Call lrRoleConstraintInstance.DisplayAndAssociate()
                    Next

                    Me.MakeDirty()
                    If Me.FormLoaded Then
                        Call Me.Form.EnableSaveButton()
                    End If

                    Me.DiagramView.Cursor = Cursors.Default
                End If

                If abExpandIfReferenceModeFactType Then
                    If lrFactTypeInstance.FactType.IsPreferredReferenceMode And lrFactTypeInstance.FactType.Is1To1BinaryFactType Then
                        Dim lrEntityTypeInstance = lrFactTypeInstance.RoleGroup.Find(Function(x) x.JoinsEntityType IsNot Nothing).JoinsEntityType
                        If lrEntityTypeInstance IsNot Nothing Then
                            Call lrEntityTypeInstance.ExpandTheReferenceScheme()
                        End If
                    End If
                End If

                Return lrFactTypeInstance

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function DropFrequencyConstraintAtPoint(ByRef arRoleConstraint As FBM.RoleConstraint,
                                                       ByVal aoPoint As PointF,
                                                       Optional ByVal abBroadcastInterfaceEvent As Boolean = False) As FBM.RoleConstraintInstance

            Dim lrRoleConstraintInstance As FBM.FrequencyConstraint

            '-----------------------------------
            'Create the RoleConstraintInstance
            '-----------------------------------
            lrRoleConstraintInstance = arRoleConstraint.CloneFrequencyConstraintInstance(Me)
            lrRoleConstraintInstance.X = aoPoint.X
            lrRoleConstraintInstance.Y = aoPoint.Y

            lrRoleConstraintInstance.MaximumFrequencyCount = arRoleConstraint.MaximumFrequencyCount
            lrRoleConstraintInstance.MinimumFrequencyCount = arRoleConstraint.MinimumFrequencyCount

            Call lrRoleConstraintInstance.DisplayAndAssociate()

            '----------------------------------------------------------------------------------------
            'Create a ConceptInstance that can be broadcast to other ClientServer Boston instances.
            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, arRoleConstraint.Id, pcenumConceptType.ValueType)
            lrConceptInstance.X = aoPoint.X
            lrConceptInstance.Y = aoPoint.Y

            '----------------------------------------------------------------------------
            'Add the RoleConstraint to the Model if it is not already within the Model.
            '----------------------------------------------------------------------------
            If Me.Model.RoleConstraint.Exists(AddressOf arRoleConstraint.Equals) Then
                '-----------------------------------------------------------------------------------------------------------------------
                'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                '  so we do the Client/Server broadcast processing here.
                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                        arRoleConstraint,
                                                                        lrConceptInstance)
                End If
            Else
                '------------------------------------------------------------------
                'The RoleConstraint is not already within the ORMModel so add it.
                Me.Model.AddRoleConstraint(arRoleConstraint, True, True, lrConceptInstance)
            End If

            '---------------------------------------------
            'Add the RoleConstraintInstance to the Page.
            '---------------------------------------------
            Me.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance)

            Call Me.MakeDirty()

            Return lrRoleConstraintInstance

        End Function

        Public Function DropRoleConstraintAtPoint(ByRef arRoleConstraint As FBM.RoleConstraint,
                                                  ByVal aoPoint As PointF,
                                                  Optional ByVal abBroadcastInterfaceEvent As Boolean = False) As FBM.RoleConstraintInstance


            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

            '-------------------------------------------------------------------------------------------------------
            'Check to see that the relevant FactTypes referenced from the RoleConstraint are loaded onto the Page.
            '-------------------------------------------------------------------------------------------------------
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole
                If Me.FactTypeInstance.FindAll(Function(x) x.Id = lrRoleConstraintRole.Role.FactType.Id).Count = 0 Then
                    Call Me.DropFactTypeAtPoint(lrRoleConstraintRole.Role.FactType, aoPoint, False)
                End If
            Next

            If arRoleConstraint.Argument.Count > 1 Then
                For Each lrArgument In arRoleConstraint.Argument
                    For Each lrFactType In lrArgument.JoinPath.FactTypePath
                        If Me.FactTypeInstance.FindAll(Function(x) x.Id = lrFactType.Id).Count = 0 Then
                            Call Me.DropFactTypeAtPoint(lrFactType, aoPoint, False)
                        End If
                    Next
                Next
            End If

            '----------------------------------------------------------------------------------------
            'Create a ConceptInstance that can be broadcast to other ClientServer Boston instances.
            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, arRoleConstraint.Id, pcenumConceptType.EntityType)
            lrConceptInstance.X = aoPoint.X
            lrConceptInstance.Y = aoPoint.Y

            '----------------------------------------------------------------------------
            'Add the RoleConstraint to the Model if it is not already within the Model.
            '----------------------------------------------------------------------------
            If Me.Model.RoleConstraint.Exists(AddressOf arRoleConstraint.Equals) Then

                'Make sure the RoleConstraint is in the ModelDictionary
                Call Me.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me.Model,
                                                                              arRoleConstraint.Id,
                                                                              pcenumConceptType.RoleConstraint),
                                                                              ,
                                                                              True)

                '-----------------------------------------------------------------------------------------------------------------------
                'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                '  so we do the Client/Server broadcast processing here.
                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                        arRoleConstraint,
                                                                        lrConceptInstance)
                End If
            Else
                Me.Model.AddRoleConstraint(arRoleConstraint, True, True, lrConceptInstance)
            End If

            '-----------------------------------------------------------------------------------
            'Create the RoleConstraintInstance and add the RoleConstraintInstance to the Page.
            '-----------------------------------------------------------------------------------
            lrRoleConstraintInstance = arRoleConstraint.CloneInstance(Me, True)
            lrRoleConstraintInstance.X = aoPoint.X
            lrRoleConstraintInstance.Y = aoPoint.Y

            Call lrRoleConstraintInstance.DisplayAndAssociate()

            Call Me.MakeDirty()

            Return lrRoleConstraintInstance

        End Function

        Public Function DropValueTypeAtPoint(ByRef arValueType As FBM.ValueType,
                                             ByVal ao_pt As PointF,
                                             Optional ByVal abBroadcastInterfaceEvent As Boolean = False) As FBM.ValueTypeInstance

            Dim lrValuetype As New FBM.ValueType
            Dim lrValueTypeInstance As New FBM.ValueTypeInstance

            Try
                lrValuetype = arValueType.Clone(Me.Model, False)

                '----------------------------------------------------------------------------------------
                'Create a ConceptInstance that can be broadcast to other ClientServer Boston instances.
                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, lrValuetype.Id, pcenumConceptType.ValueType)
                lrConceptInstance.X = ao_pt.X
                lrConceptInstance.Y = ao_pt.Y

                If Me.Model.ValueType.Exists(AddressOf lrValuetype.Equals) Then
                    '-----------------------------------------------------------------------------------------------------------------------
                    'Client/Server: Model.Add<ModelElement> would normally drop the ConceptInstance on the Page, but its not being called;
                    '  so we do the Client/Server broadcast processing here.
                    If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                        Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageDropModelElementAtPoint,
                                                                            lrValuetype,
                                                                            lrConceptInstance)
                    End If
                Else
                    '-------------------------------------------------------------
                    'The ValueType is not already within the ORMModel so add it.

                    Me.Model.AddValueType(lrValuetype, True, True, lrConceptInstance)
                End If

                lrValueTypeInstance = lrValuetype.CloneInstance(Me, False)

                lrValueTypeInstance.X = ao_pt.X
                lrValueTypeInstance.Y = ao_pt.Y

                '---------------------------------------
                'Add the ValueTypeInstance to the Page
                '---------------------------------------
                Me.ValueTypeInstance.Add(lrValueTypeInstance)

                If Me.DiagramView IsNot Nothing Then
                    Me.DiagramView.Cursor = Cursors.WaitCursor

                    Call lrValueTypeInstance.DisplayAndAssociate()
                    Me.Diagram.Invalidate()

                    Me.DiagramView.Cursor = Cursors.Default
                    Call Me.MakeDirty()
                End If

                Return lrValueTypeInstance

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Function role_and_object_type_selected() As Boolean

            Dim liInd As Integer
            Dim li_count As Integer = 0
            Dim lb_role_selected As Boolean = False
            Dim lb_object_type_selected As Boolean = False
            Dim liObjectTypeCount As Integer = 0
            Dim lbMoreThanOneObjectTypeSelected As Boolean = False

            role_and_object_type_selected = False

            If Me.SelectedObject.Count > 3 Then
                role_and_object_type_selected = False
            Else
                For liInd = 1 To Me.SelectedObject.Count
                    Select Case Me.SelectedObject(liInd - 1).ConceptType
                        Case Is = pcenumConceptType.EntityType, _
                                  pcenumConceptType.ValueType, _
                                  pcenumConceptType.FactType
                            lb_object_type_selected = True
                            liObjectTypeCount += 1
                            If liObjectTypeCount > 1 Then
                                lbMoreThanOneObjectTypeSelected = True
                            End If
                        Case Is = pcenumConceptType.Role
                            lb_role_selected = True
                    End Select
                Next

                If lb_role_selected And lb_object_type_selected And Not lbMoreThanOneObjectTypeSelected And Not (liObjectTypeCount > 1) Then
                    role_and_object_type_selected = True
                End If
            End If

        End Function

        Function do_selected_roles_span_minimum_for_FactType() As Boolean

            Dim li_FactType_cardinality As Integer = 0

            li_FactType_cardinality = Me.SelectedObject(0).factType.arity

            If Me.SelectedObject.Count >= (li_FactType_cardinality - 1) Then
                do_selected_roles_span_minimum_for_FactType = True
            Else
                do_selected_roles_span_minimum_for_FactType = False
            End If

        End Function

        Public Function hasEntityTypeInstancessWithoutReferenceScheme(ByRef aarEntityTypeInstance As List(Of FBM.EntityTypeInstance)) As Boolean

            Try
                Dim larEntityTypeInstance = From ETInstance In Me.EntityTypeInstance
                                            Where Not ETInstance.EntityType.IsObjectifyingEntityType And Not ETInstance.EntityType.GetTopmostSupertype.HasPrimaryReferenceScheme
                                            Select ETInstance

                aarEntityTypeInstance = larEntityTypeInstance.ToList
                Return larEntityTypeInstance.Count > 0

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try
        End Function

        Public Function GetAllPageObjects(Optional abGetRoleConstraints As Boolean = False, _
                                          Optional abGetModelNotes As Boolean = False) As List(Of Object)

            Dim OutputList As New List(Of Object)
            Try

                For Each lrValueTypeInstance In Me.ValueTypeInstance
                    OutputList.Add(lrValueTypeInstance)
                Next

                For Each lrEntityTypeInstance In Me.EntityTypeInstance
                    OutputList.Add(lrEntityTypeInstance)
                Next

                For Each lrFactTypeInstance In Me.FactTypeInstance
                    OutputList.Add(lrFactTypeInstance)
                Next

                If abGetRoleConstraints Then
                    For Each lrRoleConstraintInstance In Me.RoleConstraintInstance
                        OutputList.Add(lrRoleConstraintInstance)
                    Next
                End If

                If abGetModelNotes Then
                    For Each lrModelNote In Me.ModelNote
                        OutputList.Add(lrModelNote)
                    Next
                End If

                Return OutputList

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return OutputList
            End Try

        End Function

        Public Function getModelElementById(ByVal asModelElementId As String) As FBM.ModelObject

            Try
                If Me.ValueTypeInstance.FindAll(Function(x) x.Id = asModelElementId).Count > 0 Then
                    Return Me.ValueTypeInstance.Find(Function(x) x.Id = asModelElementId)
                End If

                If Me.EntityTypeInstance.FindAll(Function(x) x.Id = asModelElementId).Count > 0 Then
                    Return Me.EntityTypeInstance.Find(Function(x) x.Id = asModelElementId)
                End If

                If Me.FactTypeInstance.FindAll(Function(x) x.Id = asModelElementId).Count > 0 Then
                    Return Me.FactTypeInstance.Find(Function(x) x.Id = asModelElementId)
                End If

                If Me.RoleConstraintInstance.FindAll(Function(x) x.Id = asModelElementId).Count > 0 Then
                    Return Me.RoleConstraintInstance.Find(Function(x) x.Id = asModelElementId)
                End If

                If Me.ModelNote.FindAll(Function(x) x.Id = asModelElementId).Count > 0 Then
                    Return Me.ModelNote.Find(Function(x) x.Id = asModelElementId)
                End If

                'Nothing found, so throw an error
                Throw New Exception("ModelElement with Id:'" & asModelElementId & "' was not found on Page: '" & Me.Name & "' in Model: '" & Me.Model.Name & "' (ModelId: '" & Me.Model.ModelId & "')")

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function GetModelElementsJoinedFactTypes(ByVal arModelElement As FBM.ModelObject) As List(Of FBM.FactTypeInstance)


            Dim larFactType = From Role In Me.RoleInstance _
                              Where Role.JoinedORMObject.Id = arModelElement.Id _
                              Select Role.FactType

            Dim larFactTypeInstance = larFactType.ToList

            larFactTypeInstance.Sort(AddressOf FBM.FactTypeInstance.CompareArity)

            Return larFactTypeInstance

        End Function


        ''' <summary>
        ''' Finds a blank space in relation to a ModelObject on a Page, ostensibly for the placing of another ModelObject in relation to that given ModelObject.
        ''' </summary>
        ''' <param name="arModelObject">The ModelObject around which a blank space on the canvas will be found.</param>
        ''' <param name="abEmptySpaceFound">Set to TRUE if a blank space is found, else set to FALSE</param>
        ''' <returns>A PointF object that represents where a blank space exists on the canvas in relation to the given ModelObject.</returns>
        ''' <remarks></remarks>
        Public Function FindBlankSpaceInRelationToModelObject(ByRef arModelObject As FBM.ModelObject, ByRef abEmptySpaceFound As Boolean) As PointF

            Try
                Dim lrPointF As New PointF(10, 10)
                Dim lrCentrePoint As New PointF(10, 10)
                Dim lrTempCentreModelObject As FBM.ModelObject

                Select Case arModelObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        lrTempCentreModelObject = Me.ValueTypeInstance.Find(AddressOf arModelObject.Equals)
                    Case Is = pcenumConceptType.EntityType
                        lrTempCentreModelObject = Me.EntityTypeInstance.Find(AddressOf arModelObject.Equals)
                    Case Is = pcenumConceptType.FactType
                        lrTempCentreModelObject = Me.FactTypeInstance.Find(AddressOf arModelObject.Equals)
                    Case Else
                        Throw New Exception("This function is not intended for ModelObjects of type, " & arModelObject.ConceptType.ToString)
                End Select

                If lrTempCentreModelObject Is Nothing Then
                    Throw New Exception("ModelObject with Id, " & arModelObject.Id & ", not found on Page, " & Me.Name & ".")
                End If

                Dim laiGrid(50, 50) As Integer 'The grid around the arModelObject in pixels.

                Dim loPageObject As FBM.iPageObject
                loPageObject = lrTempCentreModelObject
                lrCentrePoint.X = loPageObject.X
                lrCentrePoint.Y = loPageObject.Y

                '------------------------------------------------------------------------------------
                'Fill in centre of grid
                Dim liX, liY As Integer
                Dim liPaintX, liPaintY As Integer


                For liX = 1 To 5
                    For liY = 1 To 5
                        laiGrid(25 + liX, 25 + liY) = 1
                    Next
                Next

                For Each loPageObject In Me.ValueTypeInstance
                    If (loPageObject.X >= lrCentrePoint.X - 25) And (loPageObject.X <= lrCentrePoint.X + 25) Then
                        If (loPageObject.Y >= lrCentrePoint.Y - 25) And (loPageObject.Y <= lrCentrePoint.Y + 25) Then
                            laiGrid(Math.Abs(lrCentrePoint.X - loPageObject.X), Math.Abs(lrCentrePoint.Y - loPageObject.Y)) += 1
                            For liX = 1 To 5
                                For liY = 1 To 5
                                    liPaintX = 25 + (loPageObject.X - lrCentrePoint.X) + liX
                                    liPaintY = 25 + (loPageObject.Y - lrCentrePoint.Y) + liY
                                    If liPaintX > 49 Then liPaintX = 49
                                    If liPaintX < 0 Then liPaintX = 0
                                    If liPaintY > 49 Then liPaintY = 49
                                    If liPaintY < 0 Then liPaintY = 0
                                    laiGrid(liPaintX, liPaintY) += 1
                                Next
                            Next
                        End If
                    End If
                Next

                For Each loPageObject In Me.EntityTypeInstance
                    If (loPageObject.X >= lrCentrePoint.X - 25) And (loPageObject.X <= lrCentrePoint.X + 25) Then
                        If (loPageObject.Y >= lrCentrePoint.Y - 25) And (loPageObject.Y <= lrCentrePoint.Y + 25) Then
                            laiGrid(Math.Abs(lrCentrePoint.X - loPageObject.X), Math.Abs(lrCentrePoint.Y - loPageObject.Y)) += 1
                            For liX = 1 To 5
                                For liY = 1 To 5
                                    liPaintX = 25 + (loPageObject.X - lrCentrePoint.X) + liX
                                    liPaintY = 25 + (loPageObject.Y - lrCentrePoint.Y) + liY
                                    If liPaintX > 49 Then liPaintX = 49
                                    If liPaintX < 0 Then liPaintX = 0
                                    If liPaintY > 49 Then liPaintY = 49
                                    If liPaintY < 0 Then liPaintY = 0
                                    laiGrid(liPaintX, liPaintY) += 1
                                Next
                            Next
                        End If
                    End If
                Next

                For Each loPageObject In Me.FactTypeInstance
                    If (loPageObject.X >= lrCentrePoint.X - 25) And (loPageObject.X <= lrCentrePoint.X + 25) Then
                        If (loPageObject.Y >= lrCentrePoint.Y - 25) And (loPageObject.Y <= lrCentrePoint.Y + 25) Then
                            laiGrid(Math.Abs(lrCentrePoint.X - loPageObject.X), Math.Abs(lrCentrePoint.Y - loPageObject.Y)) += 1
                            For liX = 1 To 5
                                For liY = 1 To 5
                                    liPaintX = 25 + (loPageObject.X - lrCentrePoint.X) + liX
                                    liPaintY = 25 + (loPageObject.Y - lrCentrePoint.Y) + liY
                                    If liPaintX > 49 Then liPaintX = 49
                                    If liPaintX < 0 Then liPaintX = 0
                                    If liPaintY > 49 Then liPaintY = 49
                                    If liPaintY < 0 Then liPaintY = 0
                                    laiGrid(liPaintX, liPaintY) += 1
                                Next
                            Next
                        End If
                    End If
                Next

                For liX = 49 To 0 Step -1
                    For liY = 0 To 49
                        If laiGrid(liX, liY) = 0 Then
                            abEmptySpaceFound = True
                            Return New PointF(lrCentrePoint.X + (liX - 25), _
                                               lrCentrePoint.Y + (liY - 25))
                        End If
                    Next
                Next

                abEmptySpaceFound = False
                Return New PointF(10, 10)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try


        End Function



        Function GetFirstSelectedEntityType() As FBM.EntityType

            GetFirstSelectedEntityType = Nothing

            Try
                GetFirstSelectedEntityType = Me.SelectedObject.Find(Function(p) p.ConceptType = pcenumConceptType.EntityType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        ''' <summary>
        ''' Clears the Page of ModelObjects and optionally clears the Page's Diagram.
        ''' </summary>
        ''' <param name="abClearDiagram">True: Clears the Page's Diagram (visible objects). Defaults fo False</param>
        ''' <remarks></remarks>
        Public Sub ClearFast(Optional ByVal abClearDiagram As Boolean = False)

            Me.ModelNote.Clear()
            Me.RoleConstraintInstance.Clear()
            Me.FactTypeInstance.Clear()
            Me.EntityTypeInstance.Clear()
            Me.ValueTypeInstance.Clear()
            Me.RoleInstance.Clear()

            If abClearDiagram And IsSomething(Me.Diagram) Then
                Me.Diagram.Nodes.Clear()
                Me.Diagram.Links.Clear()
            End If

        End Sub



        ''' <summary>
        ''' Clears all (diagram/visible) objects off the Page's Diagram and refreshes the Page's Diagram.
        '''   Used, for instance, when dynamically addings objects to a Page from another Page.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearAndRefresh()

            If IsSomething(Me.Diagram) Then
                Me.Diagram.Nodes.Clear()
                Me.Diagram.Links.Clear()

                Select Case Me.Language
                    Case Is = pcenumLanguage.ORMModel
                        Call Me.Form.DisplayORMModelPage(Me)
                End Select
            End If

        End Sub

        Public Function ContainsModelElement(ByVal arModelElement As FBM.ModelObject) As Boolean

            Return Me.GetAllPageObjects.FindAll(Function(x) x.id = arModelElement.Id).Count > 0

        End Function

        Public Function CreateFactInstance(ByRef arFactTypeInstance As FBM.FactTypeInstance, ByRef arFact As FBM.Fact) As FBM.FactInstance

            Dim liInd As Integer = 0
            Dim lrFactData As FBM.FactData

            Dim lrFactInstance As New FBM.FactInstance

            Try
                '---------------------------------------------
                'Create a new FactInstance for the new Fact
                '---------------------------------------------
                lrFactInstance = New FBM.FactInstance(arFact, arFactTypeInstance)
                lrFactInstance.isDirty = True
                '-------------------------------------------------------------
                'Create the FactDataInstances to attach to the Cells of the
                '  new Fact/row in the FactTable
                '-------------------------------------------------------------
                Dim liRowNr As Integer = Viev.Greater(arFactTypeInstance.FactTable.TableShape.RowCount - 1, 0)


                For liInd = 0 To (arFactTypeInstance.FactType.Arity - 1)
                    Dim lrFactDataInstance As New FBM.FactDataInstance

                    lrFactData = arFact.Data(liInd)
                    lrFactDataInstance = lrFactData.CloneInstance(Me, lrFactInstance)
                    lrFactDataInstance.Fact = lrFactInstance
                    lrFactDataInstance.TableShape = arFactTypeInstance.FactTable.TableShape
                    lrFactDataInstance.Cell = arFactTypeInstance.FactTable.TableShape.Item(liInd, liRowNr)
                    lrFactDataInstance.isDirty = True

                    arFactTypeInstance.FactTable.TableShape.Item(liInd, liRowNr).Text = Trim(lrFactDataInstance.Concept.Symbol)
                    arFactTypeInstance.FactTable.TableShape.Item(liInd, liRowNr).Tag = lrFactDataInstance
                    lrFactDataInstance.Cell.Tag = lrFactDataInstance
                    '--------------------------------------------------
                    'Add the FactDataInstance to the new FactInstance
                    '--------------------------------------------------
                    lrFactInstance.Data.Add(lrFactDataInstance)
                Next liInd

                Return lrFactInstance

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrFactInstance
            End Try



        End Function


        Public Function AreObjectTypesLoadedForFactType(ByVal arFactType As FBM.FactType) As Boolean

            '-----------------------------------------------------------------------------------
            'Returns TRUE if all ObjectTypes linked by Roles for the FactType are loaded
            '  on the Page, ELSE returns FALSE.
            '
            'This Function is used before dropping a FactType onto a Page from the 
            '  Model Dictionary. Richmond will not display a FactTypeInstance on a Page
            '  without associated ObjectTypes to each Role, so each associated ObjectType
            '  needs to be loaded on the Page before dropping a FactType on the Page.
            '  NB The DropFactTypeAtPoint function will load any ObjectTypes to the Page
            '  if they are not loaded, and then add the FactTypeInstance to the Page.
            '-----------------------------------------------------------------------------------
            Dim lrRole As New FBM.Role
            Dim lr_object_type As New FBM.ModelObject
            Dim lrModelObject As New Object

            AreObjectTypesLoadedForFactType = True

            For Each lrRole In arFactType.RoleGroup
                lrModelObject = lrRole.JoinedORMObject

                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        If IsSomething(Me.EntityTypeInstance.Find(Function(x) x.Id = lrRole.JoinedORMObject.Id)) Then
                            '--------------------------------------------
                            'Great the EntityType joined by the Role
                            '  is loaded onto the Page as an EntityTypeInstance
                            '--------------------------------------------
                        Else
                            AreObjectTypesLoadedForFactType = False
                            Exit Function
                        End If
                    Case Is = pcenumConceptType.ValueType
                        If IsSomething(Me.ValueTypeInstance.Find(Function(x) x.Id = lrRole.JoinedORMObject.Id)) Then
                            '--------------------------------------------
                            'Great the ValueType joined by the Role
                            '  is loaded onto the Page as an ValueTypeInstance
                            '--------------------------------------------
                        Else
                            AreObjectTypesLoadedForFactType = False
                            Exit Function
                        End If
                    Case Is = pcenumConceptType.FactType
                        If IsSomething(Me.FactTypeInstance.Find(Function(x) x.Id = lrRole.JoinedORMObject.Id)) Then
                            '--------------------------------------------
                            'Great the FactType joined by the Role
                            '  is loaded onto the Page as an FactTypeInstance
                            '--------------------------------------------
                        Else
                            AreObjectTypesLoadedForFactType = False
                            Exit Function
                        End If
                End Select
            Next

        End Function

        ''' <summary>
        ''' Page loads from database and adds itslef to the Model
        ''' NB Is used in threading load of Model 
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub Load(ByVal abAddToModel As Object)
            '-------------------------------------
            'Loads an ORM model from the database
            '-------------------------------------
            Dim liInd As Integer = 0

            Try
                'CodeSafe
                If Me.Loading Or Me.Loaded Then Exit Sub

                Me.Loading = True
                '------------------------------------
                'Get ValueTypes
                '------------------------------------
                Richmond.WriteToStatusBar("Loading Page: '" & Me.Name & "' Value Types")
                prApplication.ThrowErrorMessage("Loading Page.ValueTypes", pcenumErrorType.Information)
                'If TableValueTypeInstance.getValueTypeInstance_count_by_page(Me) > 0 Then
                '-----------------------------------------------
                'There are EntityTypes within the ORMDiagram
                '-----------------------------------------------
                '--------------------------------------------------
                'Get the list of ValueTypes within the ORMDiagram
                '--------------------------------------------------
                Me.ValueTypeInstance = TableValueTypeInstance.getValueTypeInstances_by_page(Me)
                'End If

                '-----------------
                'Get EntityTypes
                '-----------------
                Richmond.WriteToStatusBar("Loading Page: '" & Me.Name & "' Entity Types")
                prApplication.ThrowErrorMessage("Loading Page.EntityTypes", pcenumErrorType.Information)
                'If TableEntityTypeInstance.getEntityTypeInstance_count_by_page(Me) > 0 Then
                '-----------------------------------------------
                'There are EntityTypes within the ORMDiagram
                '-----------------------------------------------
                '--------------------------------------------------
                'Get the list of EntityTypes within the ORMDiagram
                '--------------------------------------------------
                Me.EntityTypeInstance = TableEntityTypeInstance.GetEntityTypeInstancesByPage(Me)
                'End If

                '---------------
                'Get FactTypes
                '---------------
                Richmond.WriteToStatusBar("Loading Page: '" & Me.Name & "' Fact Types")
                prApplication.ThrowErrorMessage("Loading Page.FactTypes for Page.Name:" & Me.Name, pcenumErrorType.Information)

                '-----------------------------------------------
                'There are FactTypes within the ORMDiagram
                '  Get the list of FactTypes within the ORMDiagram
                '--------------------------------------------------
                Call TableFactTypeInstance.GetFactTypeInstancesByPage(Me)

                '====================================================================================================================
                'Sometimes if a Role within an ObjectifiedFactType references another ObjectifiedFactType the SQL sort order
                ' within TableFactTypeInstance.GetFactTypeInstancesByPage doesn't work. So make sure that all Roles have a JoinedORMObject.
                Dim latType = {GetType(FBM.ValueTypeInstance),
                               GetType(FBM.EntityTypeInstance),
                               GetType(FBM.FactTypeInstance)}

                Dim larRoleInstance = From FactTypeInstance In Me.FactTypeInstance
                                      From RoleInstance In FactTypeInstance.RoleGroup
                                      Where RoleInstance.JoinedORMObject Is Nothing
                                      Select RoleInstance
                'removed Or Not latType.Contains(RoleInstance.JoinedORMObject.GetType)

                For Each lrRoleInstance In larRoleInstance
                    Select Case lrRoleInstance.Role.TypeOfJoin
                        Case Is = pcenumRoleJoinType.ValueType
                            lrRoleInstance.JoinedORMObject = Me.ValueTypeInstance.Find(Function(x) x.Id = lrRoleInstance.Role.JoinedORMObject.Id)
                        Case Is = pcenumRoleJoinType.EntityType
                            lrRoleInstance.JoinedORMObject = Me.EntityTypeInstance.Find(Function(x) x.Id = lrRoleInstance.Role.JoinedORMObject.Id)
                        Case Is = pcenumRoleJoinType.FactType
                            lrRoleInstance.JoinedORMObject = Me.FactTypeInstance.Find(Function(x) x.Id = lrRoleInstance.Role.JoinedORMObject.Id)
                    End Select
                Next
                '====================================================================================================================

                '---------------------
                'Get RoleConstraints
                '---------------------
                Richmond.WriteToStatusBar("Loading Page: '" & Me.Name & "' Role Constraints")
                prApplication.ThrowErrorMessage("Loading Page.RoleConstraints", pcenumErrorType.Information)

                Me.RoleConstraintInstance = TableRoleConstraintInstance.GetRoleConstraintInstancesByPage(Me)


                Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                For Each lrEntityTypeInstance In Me.EntityTypeInstance.FindAll(Function(x) x.PreferredIdentifierRCId <> "")
                    Call lrEntityTypeInstance.SetReferenceModeObjects()
                Next

                '-----------------------------------------
                'Get the SubtypeRelationships
                '-----------------------------------------
                Call TableSubtypeRelationship.GetSubtypeInstancesByPage(Me)

                '----------------
                'Get ModelNotes
                '----------------
                Richmond.WriteToStatusBar("Loading Page: '" & Me.Name & "' Model Notes")
                prApplication.ThrowErrorMessage("Loading Page.ModelNotes", pcenumErrorType.Information)
                If TableModelNoteInstance.getModelNoteInstanceCountByPage(Me) > 0 Then
                    Me.ModelNote = TableModelNoteInstance.getModelNoteInstancesByPage(Me)
                End If

                Richmond.WriteToStatusBar(".")
                Me.Loaded = True
                Me.IsDirty = False

                Richmond.WriteToStatusBar("Loaded Page: '" & Me.Name & "'")

                If abAddToModel Then
                    SyncLock Me.Model.Page
                        Me.Model.Page.AddUnique(Me)
                    End SyncLock
                End If

                Me.Loading = False

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Shadows Sub MakeDirty()

            Me.IsDirty = True
            Me.Model.MakeDirty(False, False)
            RaiseEvent PageUpdated()
            'Call Me.Model.ReviewModelErrors()

        End Sub

        ''' <summary>
        ''' Used when Boston is in ClientServer mode and when one user moves a ModelElement on a Page,
        '''  and the same ModelElement must move within Boston instances of other Users that have the same Model/Page loaded.
        ''' </summary>
        ''' <param name="arConceptInstance"></param>
        ''' <remarks></remarks>
        Public Sub moveModelElement(ByVal arConceptInstance As Viev.FBM.Interface.ConceptInstance)

            Try
                Dim lrModelElement As Object

                lrModelElement = Me.GetAllPageObjects(True, True).Find(Function(x) x.Id = arConceptInstance.ModelElementId)

                If lrModelElement IsNot Nothing Then

                    lrModelElement.X = arConceptInstance.X
                    lrModelElement.Y = arConceptInstance.Y

                    If lrModelElement.Shape IsNot Nothing Then
                        lrModelElement.Shape.Move(arConceptInstance.X, arConceptInstance.Y)
                    End If

                    If Me.Diagram IsNot Nothing Then
                        Me.Diagram.Invalidate()
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

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Try
                '------------------------------------------
                'Refresh the Page (form) hosting the model
                '------------------------------------------
                If Me.ReferencedForm IsNot Nothing Then
                    Me.ReferencedForm.TabText = Me.Name
                    Me.ReferencedForm.Invalidate()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Function GetFirstSelectedModelObject() As FBM.ModelObject

            Dim loObject As Object

            GetFirstSelectedModelObject = Nothing

            Try
                For Each loObject In Me.SelectedObject
                    Select Case loObject.ConceptType
                        Case Is = pcenumConceptType.EntityType, _
                                  pcenumConceptType.ValueType, _
                                  pcenumConceptType.FactType
                            GetFirstSelectedModelObject = loObject
                            Exit For
                    End Select
                Next

            Catch lo_error As Exception
                MsgBox("Error: GetFirstSelectedModelObject: " & lo_error.Message)
            End Try

        End Function

        Function GetMidOfRoleInstances(ByRef aarRoleInstance As List(Of FBM.RoleInstance)) As PointF

            '---------------------------------------------------------------------
            'Calculates the mid-point (coordinate/Point) of the set of objects
            '  (Instances type objects) within the (Me.)SelectedObject struct.
            '  * Effectively gets the AverageX and AverageY of the X,Y coordinates
            '    of the Objects within the (Me.)SelectedObject struct.
            '---------------------------------------------------------------------
            Dim li_average_x As Integer = 0
            Dim li_average_y As Integer = 0
            Dim lrRoleInstance As RoleInstance

            Try
                For Each lrRoleInstance In aarRoleInstance
                    li_average_x += lrRoleInstance.X
                    li_average_y += lrRoleInstance.Y
                Next

                li_average_x = li_average_x / aarRoleInstance.Count
                li_average_y = li_average_y / aarRoleInstance.Count

                Return New PointF(li_average_x, li_average_y)
            Catch lo_error As Exception
                MsgBox("Error: GetMidOfRoleInstances: " & lo_error.Message)
            End Try

        End Function

        Public Function GetMidPointOfModelObjects(ByRef aarModelObject As List(Of FBM.ModelObject)) As PointF

            Dim liXTotal As Integer = 0
            Dim liYTotal As Integer = 0

            For Each lrModelObject In aarModelObject

                Dim lrPageObject = From PageObject In Me.GetAllPageObjects _
                                   Where PageObject.Id = lrModelObject.Id _
                                   Select PageObject

                liXTotal += lrPageObject(0).X
                liYTotal += lrPageObject(0).Y

            Next

            Return New PointF((liXTotal / aarModelObject.Count), (liYTotal / aarModelObject.Count))

        End Function



        Function GetMidOfSelectedObjects() As PointF

            '---------------------------------------------------------------------
            'Calculates the mid-point (coordinate/Point) of the set of objects
            '  (Instances type objects) within the (Me.)SelectedObject struct.
            '  * Effectively gets the AverageX and AverageY of the X,Y coordinates
            '    of the Objects within the (Me.)SelectedObject struct.
            '---------------------------------------------------------------------
            Dim li_average_x As Integer = 0
            Dim li_average_y As Integer = 0
            Dim loObject As Object = Nothing

            Try
                For Each loObject In Me.SelectedObject
                    li_average_x += loObject.x
                    li_average_y += loObject.y
                Next

                li_average_x = li_average_x / Me.SelectedObject.Count
                li_average_y = li_average_y / Me.SelectedObject.Count

                Return New PointF(li_average_x, li_average_y)
            Catch lo_error As Exception
                MsgBox("Error: GetMidOfSelectedObjects: " & lo_error.Message)
            End Try

        End Function

        Function GetFirstSelectedRoleInstance() As FBM.RoleInstance

            GetFirstSelectedRoleInstance = Nothing

            Try
                GetFirstSelectedRoleInstance = Me.SelectedObject.Find(Function(p) p.ConceptType = pcenumConceptType.Role)

            Catch lo_error As Exception
                MsgBox("Error: GetFirstSelectedRoleInstance: " & lo_error.Message)
            End Try

        End Function

        Public Function IsObjectSelected(ByRef arModelObject As FBM.ModelObject) As Boolean

            Dim lrObject As Object

            IsObjectSelected = False

            For Each lrObject In Me.SelectedObject

                If lrObject Is arModelObject Then
                    IsObjectSelected = True
                    Exit For
                End If
            Next

        End Function


        Public Sub RemoveEntityTypeInstance(ByRef arEntityTypeInstance As FBM.EntityTypeInstance, ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                If Me.Diagram IsNot Nothing And arEntityTypeInstance.Shape IsNot Nothing Then
                    Me.Diagram.Nodes.Remove(arEntityTypeInstance.EntityTypeNameShape)
                    Me.Diagram.Nodes.Remove(arEntityTypeInstance.ReferenceModeShape)
                    Me.Diagram.Nodes.Remove(arEntityTypeInstance.Shape)
                End If

                Me.EntityTypeInstance.Remove(arEntityTypeInstance)

                'Do database processing if necessary.
                If abBroadcastInterfaceEvent Then
                    Call TableEntityTypeInstance.delete_entity_type_instance(arEntityTypeInstance)
                End If

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, arEntityTypeInstance.Id, pcenumConceptType.EntityType)
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageRemovePageObject, arEntityTypeInstance, lrConceptInstance)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub RemoveFactTypeInstance(ByRef arFactTypeInstance As FBM.FactTypeInstance, ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                If Me.Diagram IsNot Nothing Then
                    Me.Diagram.Nodes.Remove(arFactTypeInstance.Shape)
                    Me.Diagram.Nodes.Remove(arFactTypeInstance.FactTypeReadingShape.Shape)
                    Me.Diagram.Nodes.Remove(arFactTypeInstance.FactTable.TableShape)
                    Me.Diagram.Nodes.Remove(arFactTypeInstance.FactTypeNameShape)
                End If

                'The FactTypeName is a separate Shape with its own ConceptInstance in the database.
                Call arFactTypeInstance.FactTypeName.RemoveFromPage(abBroadcastInterfaceEvent)

                For Each lrRoleInstance In arFactTypeInstance.RoleGroup.ToArray
                    Call lrRoleInstance.RemoveFromPage()
                Next

                For Each lrRoleConstraintInstance In arFactTypeInstance.InternalUniquenessConstraint.ToArray
                    Call lrRoleConstraintInstance.RemoveFromPage(abBroadcastInterfaceEvent)
                Next

                Me.FactTypeInstance.Remove(arFactTypeInstance)

                'Do database processing if necessary.
                If abBroadcastInterfaceEvent Then
                    Call TableFactTypeInstance.DeleteFactTypeInstance(arFactTypeInstance)
                    If arFactTypeInstance.FactTable IsNot Nothing Then
                        Call TableFactTableInstance.DeleteFactTableInstance(arFactTypeInstance.FactTable)
                    End If
                End If

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, arFactTypeInstance.Id, pcenumConceptType.FactType)
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageRemovePageObject, arFactTypeInstance, lrConceptInstance)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveRoleInstance(ByRef arRoleInstance As FBM.RoleInstance)

            Me.Diagram.Nodes.Remove(arRoleInstance.Shape)

        End Sub

        Public Sub RemoveFromModel()

            Try
                Call TableConceptInstance.DeleteConceptInstancesForPage(Me)
                Me.Model.Page.Remove(Me)

                TablePage.DeletePage(Me)

                '------------------------------------------------------
                'Remove the Page from the EnterpriseExplorer TreeView
                '--------------------------------------
                Dim lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = Me.PageId)
                lrEnterpriseView.TreeNode.Remove()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveModelNoteInstance(ByRef arModelNoteInstance As FBM.ModelNoteInstance, ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                If Me.Diagram IsNot Nothing Then
                    Me.Diagram.Nodes.Remove(arModelNoteInstance.Shape)
                End If

                Me.ModelNoteInstance.Remove(arModelNoteInstance)

                'Do database processing if necessary.
                If abBroadcastInterfaceEvent Then
                    Call TableModelNoteInstance.DeleteModelNoteInstance(arModelNoteInstance)
                End If

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, arModelNoteInstance.Id, pcenumConceptType.ModelNote)
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageRemovePageObject, arModelNoteInstance, lrConceptInstance)
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
        ''' Removes a ModelObject (EntityType, ValueType, FactType) from the Page.
        ''' </summary>
        ''' <param name="arModelObject"></param>
        ''' <remarks></remarks>
        Public Sub RemoveModelObject(ByRef arModelObject As FBM.ModelObject, ByVal abBroadcastInterfaceEvent As Boolean)

            Select Case arModelObject.ConceptType
                Case Is = pcenumConceptType.EntityType
                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                    lrEntityTypeInstance = arModelObject
                    Call Me.RemoveEntityTypeInstance(arModelObject, abBroadcastInterfaceEvent)
                Case Is = pcenumConceptType.ValueType
                    Dim lrValueTypeInstance As FBM.ValueTypeInstance
                    lrValueTypeInstance = arModelObject
                    Call Me.RemoveValueTypeInstance(arModelObject, abBroadcastInterfaceEvent)
                Case Is = pcenumConceptType.FactType
                    Dim lrFactTypeInstance As FBM.FactTypeInstance
                    lrFactTypeInstance = arModelObject
                    Call Me.RemoveFactTypeInstance(arModelObject, abBroadcastInterfaceEvent)
            End Select

        End Sub

        Public Sub RemoveRoleConstraintInstance(ByRef arRoleConstraintInstance As FBM.RoleConstraintInstance,
                                                ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                If Me.Diagram IsNot Nothing Then
                    Me.Diagram.Nodes.Remove(arRoleConstraintInstance.Shape)
                    arRoleConstraintInstance.Shape.Dispose()
                End If

                For Each lrRoleConstraintRoleInstance In arRoleConstraintInstance.RoleConstraintRole
                    If lrRoleConstraintRoleInstance.Role IsNot Nothing Then
                        If lrRoleConstraintRoleInstance.Role.Shape IsNot Nothing Then
                            lrRoleConstraintRoleInstance.Role.Shape.Text = ""
                            lrRoleConstraintRoleInstance.Role.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                        End If
                    End If
                Next

                Me.RoleConstraintInstance.Remove(arRoleConstraintInstance)

                'Do database processing if necessary
                If abBroadcastInterfaceEvent Then
                    Call TableRoleConstraintInstance.DeleteRoleConstraintInstance(arRoleConstraintInstance)
                End If

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, arRoleConstraintInstance.Id, pcenumConceptType.RoleConstraint)
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageRemovePageObject, arRoleConstraintInstance, lrConceptInstance)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveValueTypeInstance(ByRef arValueTypeInstance As FBM.ValueTypeInstance, ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                If Me.Diagram IsNot Nothing Then
                    Me.Diagram.Nodes.Remove(arValueTypeInstance.Shape)
                End If

                Me.ValueTypeInstance.Remove(arValueTypeInstance)

                'Do database processing if necessary.
                If abBroadcastInterfaceEvent Then
                    Call TableValueTypeInstance.DeleteValueTypeInstance(arValueTypeInstance)
                End If

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me, arValueTypeInstance.Id, pcenumConceptType.ValueType)
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.PageRemovePageObject, arValueTypeInstance, lrConceptInstance)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub ResetFactTypeInstancesForLoading()

            Dim lrFactTypeInstance As FBM.FactTypeInstance

            For Each lrFactTypeInstance In Me.FactTypeInstance
                Call lrFactTypeInstance.ResetIsDisplayedAndAssociated()
            Next

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="abRapidSave">True if Page is new to the database, else False</param>
        ''' <remarks></remarks>
        Public Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False, Optional ByVal abSaveModel As Boolean = True)

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            Dim lrValueTypeInstance As FBM.ValueTypeInstance
            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
            Dim lrModelNoteInstance As FBM.ModelNoteInstance

            '-----------------------------------------------------------------
            'Saves the Page back to the database.
            '  NB Also calls the 'Save' for the underlying Model of the Page
            '-----------------------------------------------------------------
            Try
                'CodeSafe:
                If Not Me.Model.Page.Contains(Me) Then
                    Throw New Exception("Page is not in the Model.")
                End If

                '----------------------------------------
                'Save the underlying Model (of the Page)
                '----------------------------------------
                If Me.Model.IsDirty And abSaveModel Then
                    Call Me.Model.Save()
                End If

                If Me.IsDirty = False Then Exit Sub

                '----------------------------------------------
                'First, check to see if the Page itself exists
                '  in the database/Save the Model
                '----------------------------------------------
                If abRapidSave Then
                    Call TablePage.AddPage(Me)
                Else
                    If TablePage.ExistsPageById(Me.PageId) Then
                        Call TablePage.UpdatePage(Me)
                    Else
                        '------------------------------------------------
                        'Create an instance of the Page in the database
                        '------------------------------------------------
                        Call TablePage.AddPage(Me)
                    End If

                End If

                'CMML
                Call Me.performCMMLPreSaveProcessing

                '------------------------------------
                'Save EntityTypeInstance objects
                '------------------------------------
                For Each lrEntityTypeInstance In Me.EntityTypeInstance
                    lrEntityTypeInstance.Save(abRapidSave)
                Next

                '------------------------------------
                'Save ValueTypeInstance objects
                '------------------------------------
                For Each lrValueTypeInstance In Me.ValueTypeInstance
                    lrValueTypeInstance.Save(abRapidSave)
                Next

                '------------------------------------
                'Save the RoleName objects (if any)
                '------------------------------------
                Dim lrRoleInstance As FBM.RoleInstance
                For Each lrFactTypeInstance In Me.FactTypeInstance.FindAll(Function(x) x.isDirty)
                    For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
                        If IsSomething(lrRoleInstance.RoleName) Then
                            If lrRoleInstance.Name <> "" Then
                                lrRoleInstance.RoleName.Save(abRapidSave)
                            End If
                        End If
                    Next
                Next

                '------------------------------------
                'Save the FactTypeInstance objects
                '------------------------------------
                For Each lrFactTypeInstance In Me.FactTypeInstance.FindAll(Function(x) x.isDirty)
                    lrFactTypeInstance.Save(abRapidSave)
                    lrFactTypeInstance.FactTable.Save(abRapidSave)
                Next

                '----------------------------------------
                'Save the RoleConstraintInstance objects
                '----------------------------------------
                For Each lrRoleConstraintInstance In Me.RoleConstraintInstance
                    lrRoleConstraintInstance.Save(abRapidSave)
                Next

                '----------------------------
                'Save the ModelNote objects
                '----------------------------
                For Each lrModelNoteInstance In Me.ModelNoteInstance
                    lrModelNoteInstance.Save(abRapidSave)
                Next

                '------------------------------------
                'Reset the Dirty flag on the 
                '------------------------------------
                Me.IsDirty = False
                Me.UserRejectedSave = False

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub SetName(ByVal asNewName As String)

            Me.Name = asNewName
            Call Me.MakeDirty()
        End Sub

        Public Sub select_object_type(ByVal ao_object_type As FBM.ModelObject)

            Select Case ao_object_type.ConceptType
                Case Is = pcenumConceptType.EntityType
                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                    Dim loObject As Object = ao_object_type
                    lrEntityTypeInstance = Me.EntityTypeInstance.Find(Function(x) x.Id = loObject.Id)
                    If IsSomething(lrEntityTypeInstance) Then
                        lrEntityTypeInstance.Shape.Selected = True
                    End If
                Case Is = pcenumConceptType.ValueType
                    Dim lrValueTypeInstance As FBM.ValueTypeInstance
                    Dim loObject As Object = ao_object_type
                    lrValueTypeInstance = Me.ValueTypeInstance.Find(Function(x) x.Id = loObject.Id)
                    If IsSomething(lrValueTypeInstance) Then
                        lrValueTypeInstance.Shape.Selected = True
                    End If
                Case Is = pcenumConceptType.FactType
                    Dim lrFactTypeInstance As FBM.FactTypeInstance
                    Dim loObject As Object = ao_object_type
                    lrFactTypeInstance = Me.FactTypeInstance.Find(Function(x) x.Id = loObject.Id)
                    If IsSomething(lrFactTypeInstance) Then
                        lrFactTypeInstance.Shape.Selected = True
                    End If
            End Select

        End Sub

        Public Sub ShowFactTypeNames()

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Try
                For Each lrFactTypeInstance In Me.FactTypeInstance
                    lrFactTypeInstance.ShowFactTypeName = True
                    If IsSomething(lrFactTypeInstance.FactTypeNameShape.Shape) Then
                        lrFactTypeInstance.FactTypeNameShape.Visible = True
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

        Public Sub CreateLinkToLink(ByRef aoOriginShapeNode As ShapeNode, ByRef aoTargetLink As DiagramLink)


            Dim n1 As DiagramNode = aoTargetLink.Origin
            Dim n2 As DiagramNode = aoTargetLink.Destination
            Dim n3 As DiagramNode = aoOriginShapeNode

            Try
                '----------------------------------------------------------
                'Create the dummy ShapeNode half way along the TargetLink
                '----------------------------------------------------------
                Dim liX, liY As Integer

                liX = Viev.Lesser(aoTargetLink.ControlPoints(0).X, aoTargetLink.ControlPoints(1).X)
                liX += Math.Abs(aoTargetLink.ControlPoints(0).X - aoTargetLink.ControlPoints(1).X) \ 2

                liY = Viev.Lesser(aoTargetLink.ControlPoints(0).Y, aoTargetLink.ControlPoints(1).Y)
                liY += Math.Abs(aoTargetLink.ControlPoints(0).Y - aoTargetLink.ControlPoints(1).Y) \ 2

                Dim connector As ShapeNode = Me.Diagram.Factory.CreateShapeNode(liX, liY, 2, 2)
                connector.Shape = Shape.FromId("Decision")
                connector.HandlesStyle = HandlesStyle.MoveOnly
                connector.Brush = New SolidBrush(Color.Purple)
                Dim lrPageObject As New FBM.PageObject

                lrPageObject = New FBM.PageObject("Dummy", pcenumConceptType.Decision)
                connector.Tag = lrPageObject

                Dim lo_subtype_link As DiagramLink

                lo_subtype_link = Me.Diagram.Factory.CreateDiagramLink(connector, n1)
                lo_subtype_link.BaseShape = ArrowHead.None
                lo_subtype_link.HeadShape = ArrowHead.None
                lo_subtype_link.Pen.Color = Color.Purple 'RGB(121, 0, 121)
                lo_subtype_link.Pen.Width = 0.5
                lo_subtype_link.Tag = aoTargetLink.Tag

                lo_subtype_link = Me.Diagram.Factory.CreateDiagramLink(connector, n2)
                lo_subtype_link.BaseShape = ArrowHead.None
                lo_subtype_link.HeadShape = ArrowHead.PointerArrow
                lo_subtype_link.HeadShapeSize = 3.0
                lo_subtype_link.HeadPen.Color = Color.Purple
                lo_subtype_link.Pen.Color = Color.Purple 'RGB(121, 0, 121)
                lo_subtype_link.Pen.Width = 0.5
                lo_subtype_link.Tag = aoTargetLink.Tag

                Me.Diagram.Factory.CreateDiagramLink(connector, n3)

                Me.Diagram.Links.Remove(aoTargetLink)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Private Sub TableCellClickedHandler(ByVal sender As Object, ByVal e As MindFusion.Diagramming.CellEventArgs) Handles Diagram.CellClicked
            RaiseEvent TableCellClicked(sender, e)
        End Sub

        Public Event PageUpdated()
        Public Event TableCellClicked(ByVal sender As Object, ByVal e As MindFusion.Diagramming.CellEventArgs)

        ''' <summary>
        ''' Call when it is necessary to reselect a PageObject for Verbalisation.
        '''   NB See frmDiagramORM (or UML etc).Diagram.RedrawBackground, which resets Me.IsInvalidated to False. i.e. After invalidation of the Page, Me.IsInvalidated = False
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Invalidate()

            Me.IsInvalidated = True
        End Sub

    End Class
End Namespace