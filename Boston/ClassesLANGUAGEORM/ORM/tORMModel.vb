Imports System.IO
Imports DynamicClassLibrary.Factory
Imports System.Reflection
Imports System.Xml.Serialization
Imports System.Linq
Imports System.Xml.Linq
Imports <xmlns:ns="http://www.w3.org/2001/XMLSchema">
Imports System.Text.RegularExpressions

Namespace FBM
    <Serializable()>
    Public Class Model
        Implements IEquatable(Of FBM.Model)

        <XmlAttribute()>
        Public ConceptType As pcenumConceptType = pcenumConceptType.Model

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ModelId As String = System.Guid.NewGuid.ToString
        <XmlAttribute()>
        Public Overridable Property ModelId() As String 'GUID
            Get
                Return Me._ModelId
            End Get
            Set(ByVal value As String)
                Me._ModelId = value
            End Set
        End Property

        <NonSerialized()>
        <XmlIgnore()>
        Public SharedModel As New Viev.FBM.Interface.Model

        ''' <summary>
        ''' A list of Languages that the ORM Model represents. Defaults to including ORM, but may include EntityRelationshipDiagrams, PropertyGraphSchemas etc
        ''' </summary>
        ''' <remarks></remarks>
        Public ContainsLanguage As New List(Of pcenumLanguage)

        Public Name As String = ""

        <XmlIgnore()>
        Public AllowCheckForErrors As Boolean = False

        ''' <summary>
        ''' True if the Model is saved to XML rather than to the database.
        ''' </summary>
        <XmlAttribute>
        Public StoreAsXML As Boolean = False

        <XmlIgnore()>
        <NonSerialized()>
        Public TreeNode As TreeNode

        Public Loading As Boolean = False

        <XmlIgnore()>
        Public Loaded As Boolean = False 'Used to stop reloading every time the User selects a Model in the navigation tree.

        <XmlIgnore()>
        Public RDSLoading As Boolean = False 'Used to stop loading of Pages when the RDS (Relational Data Structure/Schema) has not finished loading under threading. See also STMLoading below.

        <XmlIgnore()>
        Public STMLoading As Boolean = False 'Used to stop loading of Pages when the STM (State Transition Model) has not finished loading under threading. See also RDSLoading above.

        <XmlIgnore()>
        Public LoadedFromXMLFile As Boolean = False

        '<XmlIgnore()> _
        '<DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        <XmlIgnore()>
        Public IsDirty As Boolean = False 'True if the Model needs to be saved back to the database, else False

        Public ShortDescription As String = ""
        Public LongDescription As String = ""

        <XmlAttribute()>
        Public EnterpriseId As String = ""
        <XmlAttribute()>
        Public SubjectAreaId As String = ""
        <XmlAttribute()>
        Public ProjectId As String = ""
        <XmlAttribute()>
        Public ProjectPhaseId As Integer
        <XmlAttribute()>
        Public SolutionId As String = ""

        <XmlAttribute()>
        Public IsEnterpriseModel As Boolean = False 'True - If the Model represents that one allowable EnterpriseModel for the Enterprise, ELSE False
        <XmlAttribute()>
        Public IsConceptualModel As Boolean = True 'Default
        <XmlAttribute()>
        Public IsPhysicalModel As Boolean = False
        <XmlAttribute()>
        Public IsNamespace As Boolean = False

        <XmlIgnore()>
        Public [Namespace] As ClientServer.Namespace

        <XmlAttribute()>
        Public CreatedByUserId As String = "" 'The User who created the Model when in Client/Server mode.


        <XmlIgnore()>
        Public [Dictionary] As New Dictionary(Of String, Integer)

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ModelDictionary As New List(Of FBM.DictionaryEntry)
        Public Overridable Property ModelDictionary() As List(Of FBM.DictionaryEntry)
            Get
                Return Me._ModelDictionary
            End Get
            Set(ByVal value As List(Of FBM.DictionaryEntry))
                Me._ModelDictionary = value
                RaiseEvent ModelUpdated()
            End Set
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _EntityType As New List(Of FBM.EntityType)
        Public Overridable Property EntityType() As List(Of FBM.EntityType)
            Get
                Return Me._EntityType
            End Get
            Set(ByVal value As List(Of FBM.EntityType))
                Me._EntityType = value
            End Set
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ValueType As New List(Of FBM.ValueType)
        Public Overridable Property ValueType() As List(Of FBM.ValueType)
            Get
                Return Me._ValueType
            End Get
            Set(ByVal value As List(Of FBM.ValueType))
                Me._ValueType = value
            End Set
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _FactType As New List(Of FBM.FactType)
        Public Overridable Property FactType() As List(Of FBM.FactType)
            Get
                Return Me._FactType
            End Get
            Set(ByVal value As List(Of FBM.FactType))
                Me._FactType = value
            End Set
        End Property

        Public ReadOnly Property FactTypeReading As List(Of FBM.FactTypeReading)
            Get
                Dim larFactTypeReading = From FactType In Me.FactType.FindAll(Function(x) Not x.IsMDAModelElement)
                                         From lrFactTypeReading In FactType.FactTypeReading
                                         Select lrFactTypeReading

                Return larFactTypeReading.ToList

            End Get
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _Role As New List(Of FBM.Role)
        <XmlIgnore()>
        Public Overridable Property Role() As List(Of FBM.Role)
            Get
                Return Me._Role
            End Get
            Set(ByVal value As List(Of FBM.Role))
                Me._Role = value
            End Set
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _RoleConstraint As New List(Of FBM.RoleConstraint)
        Public Overridable Property RoleConstraint() As List(Of FBM.RoleConstraint)
            Get
                Return Me._RoleConstraint
            End Get
            Set(ByVal value As List(Of FBM.RoleConstraint))
                Me._RoleConstraint = value
            End Set
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ModelNote As New List(Of FBM.ModelNote)
        Public Overridable Property ModelNote() As List(Of FBM.ModelNote)
            Get
                Return Me._ModelNote
            End Get
            Set(ByVal value As List(Of FBM.ModelNote))
                Me._ModelNote = value
            End Set
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ModelError As New List(Of FBM.ModelError)
        Public Property ModelError() As List(Of FBM.ModelError)
            Get
                Return Me._ModelError
            End Get
            Set(ByVal value As List(Of FBM.ModelError))
                Me._ModelError = value
            End Set
        End Property

        ''' <summary>
        ''' The list of Pages within the Model.
        ''' NB LinFu requires Overridable Properties to work.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public WithEvents _Page As New List(Of FBM.Page)
        Public Overridable Property Page() As List(Of FBM.Page)
            Get
                Return Me._Page
            End Get
            Set(ByVal value As List(Of FBM.Page))
                Me._Page = value
            End Set
        End Property

        ''' <summary>
        ''' The Relational Data Schema for the Model.
        ''' </summary>
        ''' <remarks></remarks>
        <NonSerialized()>
        <XmlIgnore()>
        Public RDS As New RDS.Model(Me)

        ''' <summary>
        ''' The State Transition Model for Value Type / Value Type Constraints for the Model.
        ''' </summary>
        <NonSerialized()>
        <XmlIgnore()>
        Public STM As New FBM.STM.Model(Me)

        Public CoreVersionNumber As String 'the version number of the Core Model injected into the Model, or the version number of the Core Model itself if Me is Core.

        <XmlIgnore()>
        Public TargetDatabaseType As pcenumDatabaseType = pcenumDatabaseType.None 'e.g. MSAccess, ORACLE, SQL Server, MySQL etc.
        <XmlIgnore()>
        Public TargetDatabaseConnectionString As String = "" 'The ConnectionString used to connect to the Target Database.

        <NonSerialized()>
        Public DatabaseManager As FactEngine.DatabaseManager = New FactEngine.DatabaseManager(Me)

        <XmlIgnore()>
        <NonSerialized()>
        Public DatabaseConnection As FactEngine.DatabaseConnection = Nothing

        ''' <summary>
        ''' True if modifying the model modifies the database schema of the connected database.
        ''' </summary>
        <XmlAttribute>
        Public IsDatabaseSynchronised As Boolean = False

        Public ReadOnly Property RequiresDatabaseConnectionString As Boolean
            Get
                Select Case Me.TargetDatabaseType
                    Case Is = pcenumDatabaseType.TypeDB
                        Return False
                    Case Else
                        Return True
                End Select
            End Get
        End Property

        '-------------------------------------------------------
        'The Parser and ParseTree are built into the Model
        '  such that ORMQL statements can be passed to 
        '  a Model for processing (on itself), in the same
        '  way that an SQL relational database has its own Parser
        ' for SQL statements.
        '-------------------------------------------------------
        <XmlIgnore()>
        <NonSerialized()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ORMQL As New ORMQL.Processor(Me)
        <XmlIgnore()>
        Public Overridable Property ORMQL() As ORMQL.Processor
            Get
                Return Me._ORMQL
            End Get
            Set(ByVal value As ORMQL.Processor)
                Me._ORMQL = value
            End Set
        End Property

        <NonSerialized()>
        <XmlIgnore()>
        Private Parser As New TinyPG.Parser(New TinyPG.Scanner) 'Used to parse Text input into the Brain; especially for ORMQL.
        <NonSerialized()>
        <XmlIgnore()>
        Private Parsetree As New TinyPG.ParseTree 'Used with the Parser, is populated during the parsing of text input into the Brain; especially ORMQL

        <NonSerialized()>
        <XmlIgnore()>
        Public Server As String 'E.g. As needed by Snowflake for ODBC. See also the physical model.

        <NonSerialized()>
        <XmlIgnore()>
        Public Database As String 'E.g. As needed by Snowflake for ODBC. See also the physical model.

        <NonSerialized()>
        <XmlIgnore()>
        Public Schema As String 'E.g. As needed by Snowflake for ODBC. See also the physical model.

        <NonSerialized()>
        <XmlIgnore()>
        Public Warehouse As String 'E.g. As needed by Snowflake for ODBC. See also the physical model.

        <NonSerialized()>
        <XmlIgnore()>
        Public DatabaseRole As String 'E.g. As needed by Snowflake for ODBC. See also the physical model.

        <NonSerialized()>
        <XmlIgnore()>
        Public Port As String 'E.g. As needed by TypeDB for a Session/Connection. See also the physical model.

        Public ReadOnly Property RequiresConnectionString As Boolean
            Get
                Select Case Me.TargetDatabaseType
                    Case Is = pcenumDatabaseType.None,
                              pcenumDatabaseType.TypeDB
                        Return False
                    Case Else
                        Return True
                End Select
            End Get
        End Property

        <NonSerialized()>
        Public Event Deleting()
        <NonSerialized()>
        Public Event FinishedErrorChecking()
        <NonSerialized()>
        Public Event MadeDirty(ByVal abGlobalBroadcast As Boolean)
        <NonSerialized()>
        Public Event ModelErrorAdded()
        <NonSerialized()>
        Public Event ModelErrorRemoved(ByVal arModelError As FBM.ModelError)
        <NonSerialized()>
        Public Event ModelErrorsCleared()
        <NonSerialized()>
        Public Event StructureModified() 'Used, for example, to refresh the ModelDictionary toolbox. Used in leiu of making the model dirty and saving the entire model.
        <NonSerialized()>
        Public Event ModelUpdated()
        <NonSerialized()>
        Public Event ModelErrorsUpdated()
        <NonSerialized()>
        Public Event RDSColumnAdded(ByRef arColumn As RDS.Column)
        <NonSerialized()>
        Public Event Saved()

        '--------------------------
        'Parameterless Constructor
        '--------------------------
        Sub New()

            Me.ModelId = System.Guid.NewGuid.ToString
            Me.ContainsLanguage.Add(pcenumLanguage.ORMModel)

        End Sub

        Sub New(ByVal aiLanguageId As pcenumLanguage, ByVal as_model_name As String, ByVal abUseModelNameAsModelId As Boolean)

            MyBase.New()

            If abUseModelNameAsModelId Then
                Me.ModelId = Trim(as_model_name)
            End If
            Me.Name = as_model_name

        End Sub

        Sub New(ByVal aiLanguageId As pcenumLanguage,
                ByVal as_model_name As String,
                ByVal as_ModelId As String,
                Optional ByVal arNamespace As ClientServer.Namespace = Nothing)

            MyBase.New()

            Me.ModelId = Trim(as_ModelId)
            Me.Name = Trim(as_model_name)

            If arNamespace IsNot Nothing Then
                Me.Namespace = arNamespace
            End If

        End Sub

        Sub New(ByVal asModelName As String, ByVal aiModelId As String)

            Me.Name = asModelName
            Me.ModelId = aiModelId

            Me.ContainsLanguage.Add(pcenumLanguage.ORMModel)

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aiEnterprise_id"></param>
        ''' <param name="aiSubject_area_id"></param>
        ''' <param name="aiProject_id"></param>
        ''' <param name="aiModelId"></param>
        ''' <param name="aiLanguageId"></param>
        ''' <param name="as_ORMModel_name"></param>
        ''' <remarks></remarks>
        Sub New(ByVal aiEnterprise_id As String, ByVal aiSubject_area_id As String, ByVal aiProject_id As String, ByVal aiModelId As String, ByVal aiLanguageId As pcenumLanguage, Optional ByVal as_ORMModel_name As String = Nothing)

            If IsNothing(aiModelId) Then
                Me.ModelId = System.Guid.NewGuid.ToString
            Else
                Me.ModelId = aiModelId
            End If

            Me.ContainsLanguage.Add(pcenumLanguage.ORMModel)

            Me.EnterpriseId = aiEnterprise_id
            Me.SubjectAreaId = aiSubject_area_id
            Me.ProjectId = aiProject_id

            Me.ConceptType = pcenumConceptType.Model

            If IsSomething(as_ORMModel_name) Then
                Me.Name = as_ORMModel_name
            End If

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.Model) As Boolean Implements System.IEquatable(Of FBM.Model).Equals

            If Me.ModelId = other.ModelId Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function Clone() As FBM.Model

            Dim lrORMModel As New FBM.Model(pcenumLanguage.ORMModel, Me.Name, True)
            Dim lrEntityType As FBM.EntityType
            Dim lrValueType As FBM.ValueType
            Dim lrFactType As FBM.FactType
            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrPage As FBM.Page
            Dim lrModelNote As FBM.ModelNote
            Dim lrModelError As FBM.ModelError

            With Me
                lrORMModel.EnterpriseId = .EnterpriseId
                lrORMModel.SubjectAreaId = .SubjectAreaId
                lrORMModel.ProjectId = .ProjectId
                lrORMModel.ProjectPhaseId = .ProjectPhaseId
                lrORMModel.SolutionId = .SolutionId
                lrORMModel.ShortDescription = .ShortDescription
                lrORMModel.LongDescription = .LongDescription

                lrORMModel.IsConceptualModel = .IsConceptualModel
                lrORMModel.IsEnterpriseModel = .IsEnterpriseModel
                lrORMModel.IsPhysicalModel = .IsPhysicalModel
                lrORMModel.IsNamespace = .IsNamespace

                lrORMModel.Loaded = .Loaded
                lrORMModel.IsDirty = .IsDirty

                For Each lrEntityType In .EntityType
                    lrORMModel.AddEntityType(lrEntityType.Clone(lrORMModel))
                Next

                For Each lrValueType In .ValueType
                    lrORMModel.AddValueType(lrValueType.Clone(lrORMModel))
                Next

                For Each lrFactType In .FactType
                    lrORMModel.AddFactType(lrFactType.Clone(lrORMModel))
                Next

                For Each lrRoleConstraint In .RoleConstraint
                    lrORMModel.AddRoleConstraint(lrRoleConstraint.Clone(lrORMModel))
                Next

                For Each lrModelNote In .ModelNote
                    lrORMModel.ModelNote.Add(lrModelNote.Clone(lrORMModel))
                Next

                For Each lrModelError In .ModelError
                    lrORMModel.ModelError.Add(lrModelError.Clone())
                Next

                For Each lrPage In .Page
                    lrORMModel.Page.Add(lrPage.Clone(lrORMModel))
                Next

            End With

            Return lrORMModel

        End Function

        Public Sub AddEntityType(ByRef arEntityType As FBM.EntityType,
                                 Optional ByVal abMakeModelDirty As Boolean = False,
                                 Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                 Optional arConceptInstance As FBM.ConceptInstance = Nothing,
                                 Optional abMakeDictionaryEntryDirty As Boolean = False)

            Dim lrDictionaryEntry As FBM.DictionaryEntry

            '------------------------------------------------------------------------------------------------
            'Add a new DictionaryEntry to the ModelDictionary if the DictionaryEntry doesn't already exist.
            '------------------------------------------------------------------------------------------------
            Dim asSymbol As String = arEntityType.Id

            lrDictionaryEntry = Me.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me, arEntityType.Id, pcenumConceptType.EntityType,,, abMakeModelDirty, abMakeModelDirty, arEntityType.DBName))

            If abMakeDictionaryEntryDirty Then
                lrDictionaryEntry.isDirty = True
            End If

            arEntityType.Concept = lrDictionaryEntry.Concept
            arEntityType.ShortDescription = lrDictionaryEntry.ShortDescription
            arEntityType.LongDescription = lrDictionaryEntry.LongDescription

            '---------------------------------------------------------------------------------------------------------
            'Add the EntityType into the list of EntityTypes for the Model if it does not already exist in the list.
            '---------------------------------------------------------------------------------------------------------
            If Me.EntityType.Exists(AddressOf arEntityType.Equals) Then
                '-------------------------------------------------------------------------
                'The EntityType already exists in the list of EntityTypes for the Model.
                '-------------------------------------------------------------------------
            Else
                lrDictionaryEntry.AddConceptType(pcenumConceptType.EntityType)

                Me.EntityType.Add(arEntityType)
                If abMakeModelDirty Then
                    Me.MakeDirty(False, True)
                End If

                ''=====================================================================================
                ''Add the EntityType to the SharedModel
                Dim lrInterfaceEntityType As New Viev.FBM.Interface.EntityType
                lrInterfaceEntityType.Id = arEntityType.Id
                lrInterfaceEntityType.Name = arEntityType.Name
                lrInterfaceEntityType.ReferenceMode = arEntityType.ReferenceMode
                lrInterfaceEntityType.Instance = arEntityType.Instance 'Not needed at this stage
                lrInterfaceEntityType.ShortDescription = arEntityType.ShortDescription
                lrInterfaceEntityType.LongDescription = arEntityType.LongDescription

                ''-------------------------------------------------------------------------------------
                ''Call the Interface method that adds the EntityType to the SharedModel.
                ''  We use a method because it triggers an Event that the Plugin may use.
                prApplication.PluginInterface.InterfaceAddEntityType(lrInterfaceEntityType)

                '======================================================================================
                'RDS
                If Not arEntityType.IsMDAModelElement And Not arEntityType.IsObjectifyingEntityType Then
                    Dim lrTable As New RDS.Table(Me.RDS, arEntityType.Id, arEntityType)
                    Me.RDS.Table.AddUnique(lrTable)
                End If

                ''=====================================================================================
                ''Broadcast the addition to the DuplexServer
                If My.Settings.UseClientServer _
                    And My.Settings.InitialiseClient _
                    And abBroadcastInterfaceEvent Then

                    Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                    lrInterfaceModel.ModelId = Me.ModelId
                    lrInterfaceModel.Name = Me.Name
                    lrInterfaceModel.Namespace = prApplication.WorkingNamespace.Id
                    lrInterfaceModel.ProjectId = prApplication.WorkingProject.Id
                    lrInterfaceModel.EntityType.Add(lrInterfaceEntityType)

                    If arConceptInstance IsNot Nothing Then
                        Dim lrInterfacePage As New Viev.FBM.Interface.Page
                        lrInterfacePage.Id = arConceptInstance.PageId

                        Dim lrInterfaceConceptInstance As New Viev.FBM.Interface.ConceptInstance
                        lrInterfaceConceptInstance.ModelElementId = arEntityType.Id
                        lrInterfaceConceptInstance.X = arConceptInstance.X
                        lrInterfaceConceptInstance.Y = arConceptInstance.Y

                        lrInterfacePage.ConceptInstance = lrInterfaceConceptInstance
                        lrInterfaceModel.Page = lrInterfacePage
                    End If

                    Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                    lrBroadcast.Model = lrInterfaceModel
                    Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.ModelAddEntityType, lrBroadcast)

                End If
                ''=====================================================================================
            End If

        End Sub

        Public Sub AddModelError(ByRef arModelError As FBM.ModelError)

            If Not Me.ModelError.Exists(AddressOf arModelError.Equals) Then
                Me.ModelError.Add(arModelError)
            End If

            RaiseEvent ModelErrorAdded()

        End Sub

        Public Sub ClearModelErrors()

            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrFactType As FBM.FactType
            Dim lrFact As FBM.Fact
            Dim lrEntityType As FBM.EntityType
            Dim lrValueType As FBM.ValueType

            For Each lrValueType In Me.ValueType
                lrValueType.ModelError.Clear()
            Next

            For Each lrEntityType In Me.EntityType
                lrEntityType.ModelError.Clear()
            Next

            For Each lrFactType In Me.FactType
                lrFactType.ModelError.Clear()
                For Each lrFact In lrFactType.Fact
                    lrFact.ModelError.Clear()
                Next
            Next

            For Each lrRoleConstraint In Me.RoleConstraint
                lrRoleConstraint.ModelError.Clear()
                For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole
                    'lrRoleConstraintRole.ModelError.Clear()
                Next
            Next

            Me.ModelError.Clear()
            RaiseEvent ModelErrorsCleared()
        End Sub

        Public Sub AddModelNote(ByRef arModelNote As FBM.ModelNote, Optional ByVal abMakeModelDirty As Boolean = False)

            Dim lrDictionaryEntry As FBM.DictionaryEntry

            '------------------------------------------------------------------------------------------------
            'Add a new DictionaryEntry to the ModelDictionary if the DictionaryEntry doesn't already exist.
            '------------------------------------------------------------------------------------------------
            lrDictionaryEntry = New FBM.DictionaryEntry(Me, arModelNote.Id, pcenumConceptType.ModelNote)
            If Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.Equals) Then
                '-----------------------------------------------------------
                'The DictionaryEntry already exists in the ModelDictionary
                '-----------------------------------------------------------
                lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                lrDictionaryEntry.AddConceptType(pcenumConceptType.ModelNote)
            Else
                lrDictionaryEntry = Me.AddModelDictionaryEntry(lrDictionaryEntry)
                Me.MakeDirty()
            End If

            arModelNote.Concept = lrDictionaryEntry.Concept

            '---------------------------------------------------------------------------------------------------------
            'Add the ModelNote into the list of ModelNotes for the Model if it does not already exist in the list.
            '---------------------------------------------------------------------------------------------------------
            Me.ModelNote.AddUnique(arModelNote)
            If abMakeModelDirty Then
                Me.MakeDirty()
            End If

        End Sub

        ''' <summary>
        ''' Adds a Role Constraint to the list of Role Constraints in the Model.
        '''   NB Adds a Dictionary Entry for the Role Constraint to the Model's Model Dictionary.
        ''' </summary>
        ''' <param name="arRoleConstraint">The Role Constraint to be added to the Model.</param>
        ''' <remarks>Use this method if it known that the Role Constraint is unique to the model, otherwise use tORMModel.CreateRoleConstraint.</remarks>
        Public Sub AddRoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint,
                                     Optional ByVal abMakeModelDirty As Boolean = False,
                                     Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                     Optional arConceptInstance As FBM.ConceptInstance = Nothing,
                                     Optional ByVal abIsSubtypeRelationshipSubtypeRole As Boolean = False,
                                     Optional ByRef arTopmostSupertypeModelObject As FBM.ModelObject = Nothing)

            Dim lrDictionaryEntry As FBM.DictionaryEntry

            Try

                '------------------------------------------------------------------------------------------------
                'Add a new DictionaryEntry to the ModelDictionary if the DictionaryEntry doesn't already exist.
                '------------------------------------------------------------------------------------------------                
                Dim asSymbol As String = arRoleConstraint.Id
                lrDictionaryEntry = Me.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me,
                                                                                       arRoleConstraint.Id,
                                                                                       pcenumConceptType.RoleConstraint),
                                                               , abMakeModelDirty,,,, True,)
                arRoleConstraint.Concept = lrDictionaryEntry.Concept
                arRoleConstraint.ShortDescription = lrDictionaryEntry.ShortDescription
                arRoleConstraint.LongDescription = lrDictionaryEntry.LongDescription

                '---------------------------------------------------------------------------------------------------------
                'Add the RoleConstraint into the list of RoleConstraints for the Model if it does not already exist in the list.
                '---------------------------------------------------------------------------------------------------------
                If Me.RoleConstraint.Exists(AddressOf arRoleConstraint.Equals) Then
                    '-------------------------------------------------------------------------
                    'The RoleConstraint already exists in the list of RoleConstraints for the Model.
                    '-------------------------------------------------------------------------
                Else
                    lrDictionaryEntry.AddConceptType(pcenumConceptType.RoleConstraint)
                    'lrDictionaryEntry.isRoleConstraint = True
                    'lrDictionaryEntry.Realisations.AddUnique(pcenumConceptType.RoleConstraint)

                    Me.RoleConstraint.Add(arRoleConstraint)

                    '=====================================================================================
                    'Add the RoleConstraint to the SharedModel
                    Dim lrInterfaceRoleConstraint As New Viev.FBM.Interface.RoleConstraint
                    lrInterfaceRoleConstraint.Id = arRoleConstraint.Id
                    lrInterfaceRoleConstraint.Name = arRoleConstraint.Name
                    lrInterfaceRoleConstraint.IsMDAModelElement = arRoleConstraint.IsMDAModelElement
                    lrInterfaceRoleConstraint.IsPreferredIdentifier = arRoleConstraint.IsPreferredIdentifier
                    lrInterfaceRoleConstraint.ShortDescription = arRoleConstraint.ShortDescription
                    lrInterfaceRoleConstraint.LongDescription = arRoleConstraint.LongDescription
                    lrInterfaceRoleConstraint.MaximumFrequencyCount = arRoleConstraint.MaximumFrequencyCount
                    lrInterfaceRoleConstraint.MinimumFrequencyCount = arRoleConstraint.MinimumFrequencyCount
                    lrInterfaceRoleConstraint.RingConstraintType.GetByDescription(arRoleConstraint.RingConstraintType.ToString)
                    lrInterfaceRoleConstraint.RoleConstraintType.GetByDescription(arRoleConstraint.RoleConstraintType.ToString)
                    lrInterfaceRoleConstraint.IsDeontic = arRoleConstraint.IsDeontic
                    lrInterfaceRoleConstraint.Cardinality = arRoleConstraint.Cardinality
                    lrInterfaceRoleConstraint.CardinalityRangeType.GetByDescription(arRoleConstraint.CardinalityRangeType.ToString)
                    lrInterfaceRoleConstraint.ValueRangeType.GetByDescription(arRoleConstraint.ValueRangeType.ToString)

                    For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole

                        Dim lrInterfaceRoleConstraintRole As New Viev.FBM.Interface.RoleConstraintRole

                        If lrRoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                            lrInterfaceRoleConstraintRole.ArgumentId = lrRoleConstraintRole.RoleConstraintArgument.Id
                            lrInterfaceRoleConstraintRole.ArgumentSequenceNr = lrRoleConstraintRole.ArgumentSequenceNr
                        End If

                        lrInterfaceRoleConstraintRole.RoleId = lrRoleConstraintRole.Role.Id
                        lrInterfaceRoleConstraintRole.SequenceNr = lrRoleConstraintRole.SequenceNr

                        lrInterfaceRoleConstraint.RoleConstraintRoles.Add(lrInterfaceRoleConstraintRole)
                    Next

                    '-------------------------------------------------------------------------------------
                    'Call the Interface method that adds the ValueType to the SharedModel.
                    '  We use a method because it triggers an Event that the Plugin may use.
                    'prApplication.PluginInterface.InterfaceAddRoleConstraint(lrInterfaceRoleConstraint)

                    '=====================================================================================
                    'RDS

                    '20210804-Removed below, because Boston needs RDS Tables for Derived Fact Types to make life easy in FactEngine query generation.
                    'Dim lbRoleConstraintFactTypeIsDerived As Boolean = False
                    'If arRoleConstraint.Role.Count > 0 Then
                    '    If arRoleConstraint.Role(0).FactType.IsDerived Then lbRoleConstraintFactTypeIsDerived = True
                    'End If
                    'And Not (arRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint And lbRoleConstraintFactTypeIsDerived)

                    If (Not arRoleConstraint.IsMDAModelElement) _
                        And Me.RDSCreated Then 'For now, check this...because otherwise RDS may have no Tables.

                        If arRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint _
                            And arRoleConstraint.isSubtypeRelationshipFactTypeIUConstraint Then
                            'PSEUDOCODE
                            '* We are making a SubtypeRelationship, so if the ModelObject is not absorbed by the Supertype,
                            '    the ModelObject/Table must have the same Columns as the PrimaryKey of the Supertypes, table.
                            If abIsSubtypeRelationshipSubtypeRole Then
                                Dim lrModelObject = arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject
                                'Only for EntityTypes for now
                                If lrModelObject.ConceptType = pcenumConceptType.EntityType Then
                                    Dim lrEntityType = CType(lrModelObject, FBM.EntityType)
                                    If lrEntityType.IsAbsorbed Then
                                        'Nothing to do here, because is absorbed by the Supertype
                                        '20200713-VM-The Supertype must absorb all of the Columns of the Subtype
                                    ElseIf arRoleConstraint.IsPreferredIdentifier Then
                                        'Create the Table for the Subtype if it does not exist.
                                        Dim lrTable = Me.RDS.getTableByName(lrEntityType.Id)

                                        If lrTable Is Nothing Then
                                            'Table not created yet                                            
                                            lrTable = New RDS.Table(Me.RDS, lrEntityType.Id, lrEntityType)
                                            Me.RDS.Table.AddUnique(lrTable)
                                        End If

                                        Dim larIndexColumn As New List(Of RDS.Column)
                                        For Each lrColumn In arTopmostSupertypeModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns

                                            Dim lrNewColumn = lrColumn.Clone
                                            lrNewColumn.Table = lrTable
                                            lrNewColumn.Id = System.Guid.NewGuid.ToString
                                            lrNewColumn.IsMandatory = True
                                            '20200719-VM-ToDo lrColumn.Index Index for the PrimaryKey
                                            '20200719-VM-ToDo lrColumn.Relation Relation for any Column in the PK that has a Relation
                                            Call lrTable.addColumn(lrNewColumn, Me.IsDatabaseSynchronised)

                                            larIndexColumn.Add(lrNewColumn)
                                        Next

                                        '------------------------
                                        'Index 
                                        'Commented out below, because should be the new set of Columns added to the Table.
                                        'larIndexColumn = arTopmostSupertypeModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns

                                        Dim lsQualifier = lrTable.generateUniqueQualifier("PK")
                                        Dim lbIsPrimaryKey As Boolean = True
                                        Dim lsIndexName As String = lrTable.Name & "_" & Trim(lsQualifier)
                                        lsIndexName = Me.RDS.createUniqueIndexName(lsIndexName, 0)

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
                                    End If
                                End If
                            End If

                        ElseIf arRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint _
                        And Not arRoleConstraint.isSubtypeRelationshipFactTypeIUConstraint Then

                            'Only interested in InternalUniquenessConstraints for forming Columns.

                            Dim lrTable As RDS.Table
                            Dim lrColumn As RDS.Column = Nothing
                            Dim lsTableName As String

                            If arRoleConstraint.impliesSingleColumnForRDSTable Then
                                Dim lrRole As FBM.Role

                                If arRoleConstraint.RoleConstraintRole.Count = 1 Then

                                    lrRole = arRoleConstraint.RoleConstraintRole(0).Role

                                    lrTable = Nothing

                                    If lrRole.IsERDPropertyRole Then

                                        lsTableName = lrRole.BelongsToTable

                                        Dim lrModelObject As FBM.ModelObject = Nothing

                                        Try
                                            lrModelObject = Me.GetModelObjectByName(lsTableName).GetTopmostNonAbsorbedSupertype
                                            lrTable = Me.RDS.getTableByName(lrModelObject.Id)
                                        Catch
                                            Throw New Exception("No Model Element found for (possibly new) Table Name:" & lsTableName)
                                        End Try

                                        If lrTable Is Nothing Then
                                            'Table not created yet
                                            If lsTableName = lrModelObject.Id Then
                                                Dim lrModelElement As FBM.ModelObject = Me.GetModelObjectByName(lsTableName)
                                                lrTable = New RDS.Table(Me.RDS, lsTableName, lrModelElement)
                                                Me.RDS.Table.AddUnique(lrTable)

                                            Else
                                                lrTable = New RDS.Table(Me.RDS, lrModelObject.Id, lrModelObject)
                                                Me.RDS.Table.AddUnique(lrTable)
                                            End If
                                        End If

                                        lrColumn = lrRole.GetCorrespondingUnaryOrBinaryFactTypeColumn(lrTable)
                                        lrColumn.FactType = lrRole.FactType

                                        If lrRole.Mandatory Then
                                            lrColumn.IsMandatory = True
                                        End If

                                        lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                    End If

                                    'There may be a case where an Index should have already contained the columns.
#Region "Check...An Index should have already contained the New Columns."

                                    'Restrict to unary FactTypes at this stage.
                                    If lrRole.FactType.IsUnaryFactType Then
                                        Dim larExistingIndexRoleConstraint = From RoleConstraint In Me.RoleConstraint
                                                                             From RoleConsraintRole In RoleConstraint.RoleConstraintRole
                                                                             Where lrRole.FactType.RoleGroup.Contains(RoleConsraintRole.Role)
                                                                             Select RoleConstraint

                                        If larExistingIndexRoleConstraint.Count > 0 Then
                                            Dim lrIndex As RDS.Index = Nothing
                                            Dim lrResponsibleRoleConstraint = larExistingIndexRoleConstraint.First

                                            Dim larExistingIndex = From Index In Me.RDS.Index
                                                                   From Column In Index.Column
                                                                   Where Index.ResponsibleRoleConstraint Is lrResponsibleRoleConstraint
                                                                   Select Index

                                            'ResponsibleRoleConstraint not implemented at CMML level, so need other way of finding Index.
                                            If larExistingIndex.Count = 0 Then
                                                Dim larRCFactTypes = (From Role In lrResponsibleRoleConstraint.Role
                                                                      Select Role.FactType).ToList

                                                larExistingIndex = From Index In Me.RDS.Index
                                                                   From Column In Index.Column
                                                                   Where larRCFactTypes.Contains(Column.Role.FactType)
                                                                   Select Index

                                                If larExistingIndex.Count > 0 Then
                                                    lrIndex = larExistingIndex.First
                                                Else
                                                    lrIndex = lrTable.Index.Find(Function(x) x.IsPrimaryKey)

                                                    If lrIndex Is Nothing Then

                                                        'Not a biggie at this stage, but do need to fix this.
                                                        'CodeSafe: Create the index.
                                                        Dim larIndexColumn As New List(Of RDS.Column)
                                                        larIndexColumn.Add(lrColumn)

                                                        Dim lsQualifier As String = lrTable.generateUniqueQualifier("PK")
                                                        Dim lbIsPrimaryKey As Boolean = True
                                                        Dim lsIndexName As String = lrTable.Name & "_" & Trim(lsQualifier)
                                                        lsIndexName = Me.RDS.createUniqueIndexName(lsIndexName, 0)

                                                        'Add the new Index
                                                        lrIndex = New RDS.Index(lrTable,
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
                                                    End If
                                                End If
                                            Else
                                                lrIndex = larExistingIndex.First
                                            End If

                                            If lrIndex IsNot Nothing Then
                                                Call lrIndex.addColumn(lrColumn)
                                            End If
                                        End If
                                    End If
#End Region
                                    'Relation
                                    If lrRole.FactType.Arity = 1 Then
                                        Call Me.generateRelationManyTo1ForUnaryFactType(lrRole)

                                    ElseIf lrRole.FactType.Arity = 2 _
                                        And lrRole.FactType.InternalUniquenessConstraint.Count = 1 _
                                        And Not (lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).ConceptType = pcenumConceptType.ValueType) Then

                                        Call Me.generateRelationForManyTo1BinaryFactType(lrRole)

                                    ElseIf lrRole.FactType.Arity = 2 _
                                     And lrRole.FactType.InternalUniquenessConstraint.Count = 2 _
                                     And Not lrRole.FactType.IsPreferredReferenceMode Then

                                        Dim larRelation = From Relation In Me.RDS.Relation
                                                          Where Relation.ResponsibleFactType.Id = lrRole.FactType.Id
                                                          Select Relation

                                        If larRelation.Count = 0 Then
                                            'No Relation exists for the 1:1 FactType.
                                            Call Me.generateRelationFor1To1BinaryFactType(lrRole)
                                        Else
                                            Dim lrRelation As RDS.Relation = larRelation.First

                                            Call lrRelation.setOriginMultiplicity(pcenumCMMLMultiplicity.One)
                                            Call lrRelation.setDestinationMultiplicity(pcenumCMMLMultiplicity.One)
                                            Call lrRelation.establishReverseColumns()

                                            Dim larDestinationColumn As New List(Of RDS.Column)
                                            larDestinationColumn = lrRelation.OriginTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey = True) '20210505-VM-Was ContributesToPrimaryKey

                                            For Each lrOriginColumn In larDestinationColumn
                                                For Each lrColumn In lrRelation.DestinationTable.Column.FindAll(Function(x) x.ActiveRole.Id = lrOriginColumn.ActiveRole.Id)
                                                    lrColumn.Relation.AddUnique(lrRelation)
                                                Next
                                            Next

                                        End If

                                    End If 'Relation

                                    'Index 
                                    If arRoleConstraint.FirstRoleConstraintRoleFactType.Is1To1BinaryFactType Then
                                        Dim larIndexColumn As New List(Of RDS.Column)
                                        Dim lrFirstRole = arRoleConstraint.FirstRoleConstraintRole

                                        Dim larColumn = From Column In lrTable.Column
                                                        Where Column.Role.Id = lrFirstRole.Id
                                                        Select Column

                                        larIndexColumn.Add(larColumn.First)

                                        Dim lsQualifier As String = lrTable.generateUniqueQualifier("UC")
                                        Dim lbIsPrimaryKey As Boolean = False
                                        Dim lsIndexName As String = lrTable.Name & "_" & Trim(lsQualifier)
                                        lsIndexName = Me.RDS.createUniqueIndexName(lsIndexName, 0)

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
                                    End If

                                End If

                            ElseIf arRoleConstraint.Role(0).FactType.IsManyTo1BinaryFactType And
                               arRoleConstraint.Role(0).HasInternalUniquenessConstraint And
                               Not arRoleConstraint.Role(0).FactType.IsLinkFactType And
                               Not arRoleConstraint.Role(0).TypeOfJoin = pcenumRoleJoinType.ValueType Then

                                Dim lrRoleConstraintRole As FBM.Role = arRoleConstraint.RoleConstraintRole(0).Role


                                lsTableName = lrRoleConstraintRole.JoinedORMObject.Id
                                lrTable = Me.RDS.getTableByName(lsTableName)

                                If lrTable Is Nothing Then
                                    'CodeSafe: Table not created yet, but should already exist.
                                    lrTable = New RDS.Table(Me.RDS, lsTableName, lrRoleConstraintRole.JoinedORMObject)
                                    Me.RDS.addTable(lrTable)
                                End If

                                Dim larCoveredRoles As New List(Of FBM.Role)
                                Dim larDownstreamActiveRoles = lrRoleConstraintRole.getDownstreamRoleActiveRoles(larCoveredRoles) 'Returns all Roles joined ObjectifiedFactTypes and their Roles' JoinedORMObjects (recursively).

                                Dim larAddedColumns As New List(Of RDS.Column)

                                'Create the new Column/s in the newly joined Table
                                For Each lrActiveRole In larDownstreamActiveRoles
                                    Dim lrNewColumn As New RDS.Column(lrTable,
                                                                      lrActiveRole.JoinedORMObject.Id,
                                                                      lrRoleConstraintRole,
                                                                      lrActiveRole,
                                                                      lrRoleConstraintRole.Mandatory)
                                    lrTable.addColumn(lrNewColumn, Me.IsDatabaseSynchronised)
                                    larAddedColumns.Add(lrNewColumn)
                                Next

                                'There may be a case where an Index should have already contained the columns.
#Region "Check...An Index should have already contained the New Columns"
                                Dim larExistingIndexRoleConstraint = From RoleConstraint In Me.RoleConstraint
                                                                     From RoleConsraintRole In RoleConstraint.RoleConstraintRole
                                                                     Where lrRoleConstraintRole.FactType.RoleGroup.Contains(RoleConsraintRole.Role)
                                                                     Select RoleConstraint

                                If larExistingIndexRoleConstraint.Count > 0 Then
                                    Dim lrResponsibleRoleConstraint = larExistingIndexRoleConstraint.First
                                    Dim lrIndex As RDS.Index = Nothing
                                    Dim larExistingIndex = From Index In Me.RDS.Index
                                                           From Column In Index.Column
                                                           Where Index.ResponsibleRoleConstraint Is lrResponsibleRoleConstraint
                                                           Select Index

                                    'ResponsibleRoleConstraint not implemented at CMML level, so need other way of finding Index.
                                    If larExistingIndex.Count = 0 Then
                                        Dim larRCFactTypes = (From Role In lrResponsibleRoleConstraint.Role
                                                              Select Role.FactType).ToList

                                        larExistingIndex = From Index In Me.RDS.Index
                                                           From Column In Index.Column
                                                           Where larRCFactTypes.Contains(Column.Role.FactType)
                                                           Select Index

                                        If larExistingIndex.Count > 0 Then
                                            lrIndex = larExistingIndex.First
                                        Else
                                            'Not a biggie at this stage, but do need to fix this.
                                        End If
                                    Else
                                        lrIndex = larExistingIndex.First
                                    End If

                                    If lrIndex IsNot Nothing Then
                                        For Each lrColumn In larAddedColumns
                                            Call lrIndex.addColumn(lrColumn)
                                        Next
                                    End If
                                End If
#End Region

                                'Relation
                                Call Me.generateRelationForManyTo1BinaryFactType(lrRoleConstraintRole)

                            ElseIf arRoleConstraint.RoleConstraintRole.Count = 1 _
                                   And arRoleConstraint.RoleConstraintRole(0).Role.FactType.Is1To1BinaryFactType Then
                                'Need to create an Index on the corresponding Table

                                If Not arRoleConstraint.impliesSingleColumnForRDSTable And
                                       arRoleConstraint.RoleConstraintRole(0).Role.TypeOfJoin = pcenumRoleJoinType.ValueType Then

                                    Dim lrFactType = arRoleConstraint.FirstRoleConstraintRoleFactType
                                    Dim lrOtherRole = lrFactType.GetOtherRoleOfBinaryFactType(arRoleConstraint.FirstRoleConstraintRole.Id)

                                    lrTable = lrOtherRole.JoinedORMObject.getCorrespondingRDSTable

                                    If lrOtherRole.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                                        If lrOtherRole.JoinsEntityType.IsObjectifyingEntityType Then
                                            lrTable = lrOtherRole.JoinsEntityType.ObjectifiedFactType.getCorrespondingRDSTable
                                        End If
                                    End If

                                    'Index 
                                    Dim larIndexColumn As New List(Of RDS.Column)


                                    Dim larColumn = From Column In lrTable.Column
                                                    Where Column.Role.Id = lrOtherRole.Id
                                                    Select Column


                                    larIndexColumn.Add(larColumn.First)

                                    Dim lsQualifier As String
                                    Dim lbIsPrimaryKey As Boolean
                                    If arRoleConstraint.IsPreferredIdentifier Then
                                        lsQualifier = "PK"
                                        lbIsPrimaryKey = True
                                    Else
                                        lsQualifier = lrTable.generateUniqueQualifier("UC")
                                        lbIsPrimaryKey = False
                                    End If

                                    Dim lsIndexName As String = lrTable.Name & "_" & Trim(lsQualifier)
                                    lsIndexName = Me.RDS.createUniqueIndexName(lsIndexName, 0)

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


                                End If

                            ElseIf (arRoleConstraint.RoleConstraintRole.Count <> 1) _
                                Or (arRoleConstraint.RoleConstraintRole.Count = 1 And arRoleConstraint.RoleConstraintRole(0).Role.FactType.IsObjectified) Then 'And (arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject.ConceptType <> pcenumConceptType.ValueType)

                                'Make Columns for the FactType of the RoleConstraint (InternalUniquenessConstraint)

                                Dim lrFactType As FBM.FactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType

                                lsTableName = lrFactType.Name
                                lrTable = Me.RDS.getTableByName(lsTableName)

                                If lrTable Is Nothing Then
                                    'Table not created yet
                                    lrTable = New RDS.Table(Me.RDS, lsTableName, lrFactType)

                                    Me.RDS.addTable(lrTable)

                                End If

                                If (lrFactType.Arity = 2 Or lrFactType.Arity = 3) _
                                        And arRoleConstraint.RoleConstraintRole.Count = 2 _
                                        And Not lrFactType.InternalUniquenessConstraint.Count > 1 _
                                        And Not arRoleConstraint.atLeastOneRoleJoinsAValueType Then
                                    lrTable.setIsPGSRelation(True)
                                End If


                                'If the FactType for the RoleConstraint  (InternalUniquenessConstraint) is Objectified
                                ' AND the ObjectifiedFactType has no outgoing FactTypes 
                                ' then the Table must be a PGS Relation
                                If lrFactType.IsObjectified Then
                                    If Not lrFactType.JoinedFactTypes.Count = 0 Then
                                        lrTable.setIsPGSRelation(True)
                                    End If
                                ElseIf lrFactType.JoinedFactTypes.Count = 0 Then
                                    lrTable.setIsPGSRelation(True)
                                End If

                                'Must have a column for all of the Roles of the FactType
                                For Each lrRole In lrFactType.RoleGroup

                                    Select Case lrRole.JoinedORMObject.ConceptType
                                        Case Is = pcenumConceptType.ValueType

                                            If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id) Then
                                                'There is no Column in the Table for the Role.
                                                lrColumn = lrRole.GetCorrespondingFactTypeColumn(lrTable)
                                                '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                                                'If arRoleConstraint.Role.Contains(lrRole) And lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                                '    lrColumn.ContributesToPrimaryKey = True
                                                'End If
                                                'If arRoleConstraint.Role.Contains(lrRole) Then  20210523-VM-Removed, because e.g. if a FT is ternary, and a two role IUC/RC is being made, the third role/Column is none the less mandatory.
                                                lrColumn.IsMandatory = True
                                                'End If
                                                lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                            End If

                                        Case Is = pcenumConceptType.EntityType

                                            If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id) Then
                                                'There is no Column in the Table for the Role.
                                                Dim lrEntityType As FBM.EntityType = lrRole.JoinedORMObject

                                                If lrEntityType.HasCompoundReferenceMode Then

                                                    Dim larColumn As New List(Of RDS.Column)
                                                    Call lrEntityType.getCompoundReferenceSchemeColumns(lrTable, lrRole, larColumn)

                                                    For Each lrColumn In larColumn
                                                        If arRoleConstraint.Role.Contains(lrRole) Then
                                                            lrColumn.IsMandatory = True
                                                            '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                                                            'If lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                                            '    lrColumn.ContributesToPrimaryKey = True
                                                            'End If
                                                        End If
                                                        lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                                    Next
                                                Else
                                                    lrColumn = lrRole.GetCorrespondingFactTypeColumn(lrTable)
                                                    'If arRoleConstraint.Role.Contains(lrRole) Then '20210523-VM-Removed, because e.g. if a FT is ternary, and a two role IUC/RC is being made, the third role/Column is none the less mandatory.
                                                    lrColumn.IsMandatory = True
                                                    '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                                                    'If lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                                    '    lrColumn.ContributesToPrimaryKey = True
                                                    'End If
                                                    'End If
                                                    lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                                End If

                                            End If

                                        Case Else 'Joins a FactType.

                                            Dim larColumn As New List(Of RDS.Column)

                                            larColumn = lrRole.getColumns(lrTable, lrRole)

                                            For Each lrColumn In larColumn
                                                Dim lbFailed As Boolean = False
                                                If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id And x.ActiveRole.Id = lrColumn.ActiveRole.Id) Then
                                                    'There is no existing Column in the Table for lrColumn.
                                                    lrColumn.Name = lrTable.createUniqueColumnName(lrColumn, lrColumn.Name, 0)
                                                    If arRoleConstraint.Role.Contains(lrRole) Then
                                                        lrColumn.IsMandatory = True
                                                    End If
                                                    lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                                Else
                                                    lbFailed = True
                                                End If

                                                If lbFailed Then
                                                    'Could be a good reason why it Failed but shouldn't have.
                                                    'E.g.The joined FactType has more than on Role in PK pointing to the same EntityType
                                                    Try
                                                        If lrRole.JoinedORMObject.getCorrespondingRDSTable IsNot Nothing Then

                                                            Dim larDownstreamColumn = From Column In lrRole.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                                                                                      Where Column.ActiveRole Is lrColumn.ActiveRole
                                                                                      Select Column

                                                            If larDownstreamColumn.Count > 1 Then
                                                                'Add the Column anyway.
                                                                lrColumn.Name = lrTable.createUniqueColumnName(lrColumn, lrColumn.Name, 0)
                                                                If arRoleConstraint.Role.Contains(lrRole) Then
                                                                    lrColumn.IsMandatory = True
                                                                End If
                                                                lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                                            End If
                                                        End If
                                                    Catch ex As Exception
                                                        'Not a biggie, but will be problematic.
                                                    End Try
                                                End If
                                            Next
                                    End Select

                                    'Relation  
                                    If lrFactType.IsObjectified Then
                                        Dim larLinkFactTypeRole = From FactType In Me.FactType
                                                                  Where FactType.IsLinkFactType = True _
                                                                        And FactType.LinkFactTypeRole Is lrRole
                                                                  Select FactType.RoleGroup(0)

                                        If larLinkFactTypeRole.Count > 0 Then
                                            For Each lrLinkFactTypeRole In larLinkFactTypeRole
                                                Call Me.generateRelationForManyTo1BinaryFactType(lrLinkFactTypeRole)
                                            Next
                                        Else
                                            Select Case lrRole.JoinedORMObject.ConceptType
                                                Case Is = pcenumConceptType.EntityType, pcenumConceptType.FactType
                                                    Call Me.generateRelationForManyToManyFactTypeRole(lrRole)
                                            End Select
                                        End If
                                    Else
                                        Select Case lrRole.JoinedORMObject.ConceptType
                                            Case Is = pcenumConceptType.EntityType, pcenumConceptType.FactType
                                                Call Me.generateRelationForManyToManyFactTypeRole(lrRole)
                                        End Select

                                    End If

                                    Dim lrModelElement As FBM.ModelObject = lrRole.JoinedORMObject

                                Next 'Role in the FactType's RoleGroup

                                '------------------------
                                'Index 
                                Dim larIndexColumn As New List(Of RDS.Column)

                                For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole

                                    Dim larColumn = From Column In lrTable.Column
                                                    Where Column.Role.Id = lrRoleConstraintRole.Role.Id
                                                    Select Column

                                    For Each lrColumn In larColumn
                                        larIndexColumn.Add(lrColumn)
                                    Next 'Column

                                Next 'RoleConstraintRole

                                Dim lsQualifier As String
                                Dim lbIsPrimaryKey As Boolean = False
                                If lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                    lsQualifier = lrTable.generateUniqueQualifier("PK")
                                    lrFactType.InternalUniquenessConstraint(0).SetIsPreferredIdentifier(True, False)
                                    lbIsPrimaryKey = True
                                Else
                                    lsQualifier = lrTable.generateUniqueQualifier("UC")
                                End If


                                Dim lsIndexName As String = lrTable.Name & "_" & Trim(lsQualifier)
                                lsIndexName = Me.RDS.createUniqueIndexName(lsIndexName, 0)

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

                            ElseIf arRoleConstraint.RoleConstraintRole.Count = 1 _
                                And arRoleConstraint.RoleConstraintRole(0).Role.FactType.IsBinaryFactType _
                                And Not arRoleConstraint.RoleConstraintRole(0).Role.FactType.IsLinkFactType _
                                And Not arRoleConstraint.Role(0).JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then

                                'BinaryFactType that joins either:
                                '1. An EntityType with a CompoundReferenceScheme; or
                                '2. An ObjectifiedFactType.

                                Dim lrFactType As FBM.FactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
                                Dim lrRole As FBM.Role = arRoleConstraint.RoleConstraintRole(0).Role

                                Dim larColumn As New List(Of RDS.Column)

                                Select Case lrFactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject.ConceptType
                                    Case Is = pcenumConceptType.FactType

                                        lsTableName = arRoleConstraint.RoleConstraintRole(0).Role.BelongsToTable
                                        lrTable = Me.RDS.getTableByName(lsTableName)

                                        If lrTable Is Nothing Then
                                            'Table not created yet
                                            Dim lrModelElement As FBM.ModelObject = Me.GetModelObjectByName(lsTableName)
                                            lrTable = New RDS.Table(Me.RDS, lsTableName, lrModelElement)
                                            Me.RDS.Table.AddUnique(lrTable)
                                        End If

                                        larColumn = lrFactType.GetOtherRoleOfBinaryFactType(lrRole.Id).getColumns(lrTable, lrRole)

                                        For Each lrColumn In larColumn
                                            'There is no Column in the Table for the Role.
                                            lrColumn.Name = lrTable.createUniqueColumnName(lrColumn, lrColumn.Name, 0)
                                            lrColumn.IsMandatory = lrRole.Mandatory
                                            lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                        Next

                                    Case Is = pcenumConceptType.EntityType

                                        Dim lrEntityType As FBM.EntityType = lrFactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject

                                        If lrEntityType.HasCompoundReferenceMode Then

                                            lsTableName = arRoleConstraint.RoleConstraintRole(0).Role.BelongsToTable
                                            lrTable = Me.RDS.getTableByName(lsTableName)

                                            If lrTable Is Nothing Then
                                                'Table not created yet
                                                Dim lrModelElement As FBM.ModelObject = Me.GetModelObjectByName(lsTableName)
                                                lrTable = New RDS.Table(Me.RDS, lsTableName, lrModelElement)
                                                Me.RDS.Table.AddUnique(lrTable)
                                            End If

                                            Call lrEntityType.getCompoundReferenceSchemeColumns(lrTable, lrRole, larColumn)

                                            For Each lrColumn In larColumn
                                                lrColumn.IsMandatory = lrRole.Mandatory
                                                lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                            Next
                                        End If

                                End Select

                                'Relations
                                If lrRole.FactType.Arity = 2 _
                                    And lrRole.FactType.InternalUniquenessConstraint.Count = 1 _
                                    And Not (lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).ConceptType = pcenumConceptType.ValueType) Then

                                    Call Me.generateRelationForManyTo1BinaryFactType(lrRole)

                                ElseIf lrRole.FactType.Arity = 2 _
                                     And lrRole.FactType.InternalUniquenessConstraint.Count = 2 _
                                     And Not lrRole.FactType.IsPreferredReferenceMode Then

                                    Dim larRelation = From Relation In Me.RDS.Relation
                                                      Where Relation.ResponsibleFactType.Id = lrRole.FactType.Id
                                                      Select Relation

                                    If larRelation.Count = 0 Then
                                        'No Relation exists for the 1:1 FactType.
                                        Call Me.generateRelationForManyTo1BinaryFactType(lrRole)
                                    Else
                                        Dim lrRelation As RDS.Relation = larRelation.First

                                        Call lrRelation.setOriginMultiplicity(pcenumCMMLMultiplicity.One)
                                        Call lrRelation.setDestinationMultiplicity(pcenumCMMLMultiplicity.One)
                                        Call lrRelation.establishReverseColumns()

                                        Dim larDestinationColumn As New List(Of RDS.Column)
                                        larDestinationColumn = lrRelation.OriginTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey) '20210505-VM-Was ContributesToPrimaryKey

                                        For Each lrOriginColumn In larDestinationColumn
                                            For Each lrColumn In lrRelation.DestinationTable.Column.FindAll(Function(x) x.ActiveRole.Id = lrOriginColumn.ActiveRole.Id)
                                                lrColumn.Relation.AddUnique(lrRelation)
                                            Next
                                        Next

                                    End If

                                End If

                            End If 'RoleConstraintRole.Count > 1

                        End If 'Is Internal Uniqueness Constraint
                    End If

                    '=====================================================================================
                    'Broadcast the addition to the DuplexServer
                    If My.Settings.UseClientServer _
                        And My.Settings.InitialiseClient _
                        And abBroadcastInterfaceEvent Then

                        Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                        lrInterfaceModel.ModelId = Me.ModelId
                        lrInterfaceModel.Name = Me.Name
                        lrInterfaceModel.Namespace = prApplication.WorkingNamespace.Id
                        lrInterfaceModel.ProjectId = prApplication.WorkingProject.Id

                        lrInterfaceModel.RoleConstraint.Add(lrInterfaceRoleConstraint)

                        If arConceptInstance IsNot Nothing Then
                            Dim lrInterfacePage As New Viev.FBM.Interface.Page
                            lrInterfacePage.Id = arConceptInstance.PageId

                            Dim lrInterfaceConceptInstance As New Viev.FBM.Interface.ConceptInstance
                            lrInterfaceConceptInstance.ModelElementId = arRoleConstraint.Id
                            lrInterfaceConceptInstance.X = arConceptInstance.X
                            lrInterfaceConceptInstance.Y = arConceptInstance.Y

                            lrInterfacePage.ConceptInstance = lrInterfaceConceptInstance
                            lrInterfaceModel.Page = lrInterfacePage
                        End If

                        Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                        lrBroadcast.Model = lrInterfaceModel
                        Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.ModelAddRoleConstraint, lrBroadcast)

                    End If
                    '=====================================================================================

                    If abMakeModelDirty Then
                        Me.MakeDirty()
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

        Public Sub AddRole(ByRef arRole As FBM.Role)

            Dim lrDictionaryEntry As FBM.DictionaryEntry

            '------------------------------------------------------------------------------------------------
            'Add a new DictionaryEntry to the ModelDictionary if the DictionaryEntry doesn't already exist.
            '------------------------------------------------------------------------------------------------
            lrDictionaryEntry = New FBM.DictionaryEntry(Me, arRole.Id, pcenumConceptType.Role)
            If Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.Equals) Then
                '-----------------------------------------------------------
                'The DictionaryEntry already exists in the ModelDictionary
                '-----------------------------------------------------------
            Else
                Call Me.AddModelDictionaryEntry(lrDictionaryEntry)
                Me.MakeDirty()
            End If

            '---------------------------------------------------------------------------------------------------------
            'Add the Role into the list of Roles for the Model if it does not already exist in the list.
            '---------------------------------------------------------------------------------------------------------
            Me.Role.AddUnique(arRole)
            Me.MakeDirty()

        End Sub

        Public Sub AddValueType(ByRef arValueType As FBM.ValueType,
                                Optional ByVal abMakeModelDirty As Boolean = False,
                                Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                Optional arConceptInstance As FBM.ConceptInstance = Nothing,
                                Optional abMakeModelDictionaryEntryDirty As Boolean = False)

            Try
                Dim lrDictionaryEntry As FBM.DictionaryEntry

                '------------------------------------------------------------------------------------------------
                'Add a new DictionaryEntry to the ModelDictionary if the DictionaryEntry doesn't already exist.
                '------------------------------------------------------------------------------------------------
                lrDictionaryEntry = New FBM.DictionaryEntry(Me, arValueType.Id, pcenumConceptType.ValueType)
                lrDictionaryEntry = Me.AddModelDictionaryEntry(lrDictionaryEntry, True, abMakeModelDirty)

                If abMakeModelDictionaryEntryDirty Then
                    lrDictionaryEntry.isDirty = True
                End If

                arValueType.Concept = lrDictionaryEntry.Concept
                arValueType.ShortDescription = lrDictionaryEntry.ShortDescription
                arValueType.LongDescription = lrDictionaryEntry.LongDescription

                '---------------------------------------------------------------------------------------------------------
                'Add the ValueType into the list of ValueTypes for the Model if it does not already exist in the list.
                '---------------------------------------------------------------------------------------------------------
                If Me.ValueType.Exists(AddressOf arValueType.Equals) Then
                    '-------------------------------------------------------------------------
                    'The ValueType already exists in the list of ValueTypes for the Model.
                    '-------------------------------------------------------------------------
                Else
                    lrDictionaryEntry.AddConceptType(pcenumConceptType.ValueType)
                    'lrDictionaryEntry.isValueType = True
                    'lrDictionaryEntry.Realisations.AddUnique(pcenumConceptType.ValueType)

                    Me.ValueType.Add(arValueType)

                    If abMakeModelDirty Then
                        Me.MakeDirty(False, False)
                    End If

                    '=====================================================================================
                    'Add the ValueType to the SharedModel
                    Dim lrXMLValueType As New Viev.FBM.Interface.ValueType
                    lrXMLValueType.Id = arValueType.Id
                    lrXMLValueType.Name = arValueType.Name
                    lrXMLValueType.DataType.GetByDescription(arValueType.DataType.ToString)
                    lrXMLValueType.DataTypeLength = arValueType.DataTypeLength
                    lrXMLValueType.DataTypePrecision = arValueType.DataTypePrecision
                    'lrXMLValueType.ShortDescriptio = arValueType.ShortDescription
                    'lrXMLValueType.LongDescription = arValueType.LongDescription
                    'lrXMLValueType.ValueConstraint 'Not needed at this stage

                    '-------------------------------------------------------------------------------------
                    'Call the Interface method that adds the ValueType to the SharedModel.
                    '  We use a method because it triggers an Event that the Plugin may use.
                    prApplication.PluginInterface.InterfaceAddValueType(lrXMLValueType)
                    '=====================================================================================
                    'Broadcast the addition to the DuplexServer
                    If My.Settings.UseClientServer _
                        And My.Settings.InitialiseClient _
                        And abBroadcastInterfaceEvent Then

                        Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                        lrInterfaceModel.ModelId = Me.ModelId
                        lrInterfaceModel.Name = Me.Name
                        lrInterfaceModel.Namespace = prApplication.WorkingNamespace.Id
                        lrInterfaceModel.ProjectId = prApplication.WorkingProject.Id
                        lrInterfaceModel.ValueType.Add(lrXMLValueType)

                        If arConceptInstance IsNot Nothing Then
                            Dim lrInterfacePage As New Viev.FBM.Interface.Page
                            lrInterfacePage.Id = arConceptInstance.PageId

                            Dim lrInterfaceConceptInstance As New Viev.FBM.Interface.ConceptInstance
                            lrInterfaceConceptInstance.ModelElementId = arValueType.Id
                            lrInterfaceConceptInstance.X = arConceptInstance.X
                            lrInterfaceConceptInstance.Y = arConceptInstance.Y

                            lrInterfacePage.ConceptInstance = lrInterfaceConceptInstance
                            lrInterfaceModel.Page = lrInterfacePage
                        End If

                        Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                        lrBroadcast.Model = lrInterfaceModel
                        Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.ModelAddValueType, lrBroadcast)

                    End If
                    '=====================================================================================

                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Function AreObjectTypesSupertypesSubtypesOfEachOther(ByRef arFirstObjectType As FBM.ModelObject, ByVal arSecondObjectType As FBM.ModelObject) As Boolean

            Dim lbAreSupertypeSubtype As Boolean = False

            Return lbAreSupertypeSubtype

        End Function

        ''' <summary>
        ''' Checks to see whether two Roles are compatible. i.e. Same JoinedObjectType, or subtype/supertype thereof.
        '''   * See also Me.AreObjectTypesSupertypesSubtypesOfEachOther
        ''' </summary>
        ''' <param name="arFirstRole">The first Role to be checked for compatibility.</param>
        ''' <param name="arSecondRole">The second Role to be checked for compatibility.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function AreRolesCompatible(ByRef arFirstRole As FBM.Role, ByRef arSecondRole As FBM.Role) As Boolean

            Try
                Dim lbAreCompatible As Boolean = False
                Dim larSubtypes As New List(Of FBM.ModelObject)
                Dim larSupertypes As New List(Of FBM.ModelObject)
                Dim larAllSubtypesSupertypes As New List(Of FBM.ModelObject)
                Dim lrRole As FBM.Role

                If arFirstRole.Id = arSecondRole.Id Then
                    Return True
                ElseIf arFirstRole.JoinedORMObject.Id = arSecondRole.JoinedORMObject.Id Then
                    Return True
                Else
                    larSubtypes = Me.GetSubtypesOfModelObject(arFirstRole.JoinedORMObject)
                    larSupertypes = Me.GetSupertypesOfModelObject(arFirstRole.JoinedORMObject)

                    Call larAllSubtypesSupertypes.AddRange(larSubtypes)
                    Call larAllSubtypesSupertypes.AddRange(larSupertypes)

                    lrRole = arSecondRole
                    Return larAllSubtypesSupertypes.FindAll(Function(x) x.Id = lrRole.JoinedORMObject.Id).Count > 0
                End If

                Return lbAreCompatible

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Function AreRolesWithinTheSameFactType(ByVal aarRoleList As List(Of FBM.Role)) As Boolean

            Dim liInd As Integer
            Dim liFirstFactTypeId As String = ""

            If aarRoleList.Count = 0 Then
                AreRolesWithinTheSameFactType = False
                Exit Function
            Else
                AreRolesWithinTheSameFactType = True
            End If

            For liInd = 1 To aarRoleList.Count
                If aarRoleList(liInd - 1).ConceptType = pcenumConceptType.Role Then
                    liFirstFactTypeId = aarRoleList(liInd - 1).FactType.Id
                    Exit For
                End If
            Next liInd


            For liInd = 1 To aarRoleList.Count
                If aarRoleList(liInd - 1).ConceptType = pcenumConceptType.Role Then
                    If Not (liFirstFactTypeId = aarRoleList(liInd - 1).FactType.Id) Then
                        AreRolesWithinTheSameFactType = False
                    End If
                End If
            Next liInd

        End Function

        Public Sub AddFactType(ByRef arFactType As FBM.FactType,
                               Optional ByVal abMakeModelDirty As Boolean = False,
                               Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                               Optional arConceptInstance As FBM.ConceptInstance = Nothing)

            Dim lrDictionaryEntry As FBM.DictionaryEntry

            '------------------------------------------------------------------------------------------------
            'Add a new DictionaryEntry to the ModelDictionary if the DictionaryEntry doesn't already exist.
            '------------------------------------------------------------------------------------------------
            Dim asSymbol As String = arFactType.Id
            lrDictionaryEntry = Me.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me, arFactType.Id, pcenumConceptType.FactType,,,,, arFactType.DBName), , abMakeModelDirty,,,, True,)

            arFactType.Concept = lrDictionaryEntry.Concept
            arFactType.ShortDescription = lrDictionaryEntry.ShortDescription
            arFactType.LongDescription = lrDictionaryEntry.LongDescription

            '---------------------------------------------------------------------------------------------------------
            'Add the FactType into the list of FactTypes for the Model if it does not already exist in the list.
            '---------------------------------------------------------------------------------------------------------
            If Me.FactType.Exists(AddressOf arFactType.Equals) Then
                '-------------------------------------------------------------------------
                'The FactType already exists in the list of FactTypes for the Model.
                '-------------------------------------------------------------------------
            Else
                lrDictionaryEntry.AddConceptType(pcenumConceptType.FactType)
                'lrDictionaryEntry.isFactType = True
                'lrDictionaryEntry.Realisations.AddUnique(pcenumConceptType.FactType)

                Me.FactType.Add(arFactType)

                Dim lrRole As FBM.Role
                For Each lrRole In arFactType.RoleGroup
                    Me.Role.AddUnique(lrRole)
                Next

                If abMakeModelDirty Then
                    Me.MakeDirty()
                End If

                '=====================================================================================
                'Add the FactType to the SharedModel
                Dim lrInterfaceFactType As New Viev.FBM.Interface.FactType
                lrInterfaceFactType.Id = arFactType.Id
                lrInterfaceFactType.Name = arFactType.Name
                lrInterfaceFactType.IsPreferredReferenceSchemeFT = arFactType.IsPreferredReferenceMode
                lrInterfaceFactType.IsSubtypeRelationshipFactType = arFactType.IsSubtypeRelationshipFactType

                lrInterfaceFactType.ShortDescription = arFactType.ShortDescription
                lrInterfaceFactType.LongDescription = arFactType.LongDescription
                lrInterfaceFactType.IsIndependent = arFactType.IsIndependent
                lrInterfaceFactType.IsObjectified = arFactType.IsObjectified
                lrInterfaceFactType.IsStored = arFactType.IsStored
                lrInterfaceFactType.IsSubtypeRelationshipFactType = arFactType.IsSubtypeRelationshipFactType
                lrInterfaceFactType.DerivationText = arFactType.DerivationText
                lrInterfaceFactType.IsDerived = arFactType.IsDerived
                lrInterfaceFactType.IsLinkFactType = arFactType.IsLinkFactType
                lrInterfaceFactType.IsMDAModelElement = arFactType.IsMDAModelElement
                lrInterfaceFactType.ShowFactTypeName = arFactType.ShowFactTypeName

                If arFactType.LinkFactTypeRole IsNot Nothing Then
                    lrInterfaceFactType.LinkFactTypeRoleId = arFactType.LinkFactTypeRole.Id
                End If

                Dim lrInterfaceRole As Viev.FBM.Interface.Role
                For Each lrRole In arFactType.RoleGroup
                    lrInterfaceRole = New Viev.FBM.Interface.Role
                    lrInterfaceRole.Id = lrRole.Id
                    lrInterfaceRole.Name = lrRole.Name
                    If lrRole.JoinedORMObject IsNot Nothing Then
                        lrInterfaceRole.JoinedObjectTypeId = lrRole.JoinedORMObject.Id
                    Else
                        lrInterfaceRole.JoinedObjectTypeId = Nothing
                    End If
                    lrInterfaceRole.SequenceNr = lrRole.SequenceNr
                    lrInterfaceRole.Mandatory = lrRole.Mandatory

                    lrInterfaceFactType.RoleGroup.Add(lrInterfaceRole)
                Next
                '-------------------------------------------------------------------------------------
                'Call the Interface method that adds the FactType to the SharedModel.
                '  We use a method because it triggers an Event that the Plugin may use.
                prApplication.PluginInterface.InterfaceAddFactType(lrInterfaceFactType)
                '=====================================================================================
                'Broadcast the addition to the DuplexServer
                If My.Settings.UseClientServer _
                    And My.Settings.InitialiseClient _
                    And abBroadcastInterfaceEvent Then

                    Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                    lrInterfaceModel.ModelId = Me.ModelId
                    lrInterfaceModel.Name = Me.Name
                    lrInterfaceModel.Namespace = prApplication.WorkingNamespace.Id
                    lrInterfaceModel.ProjectId = prApplication.WorkingProject.Id
                    lrInterfaceModel.FactType.Add(lrInterfaceFactType)

                    If arConceptInstance IsNot Nothing Then
                        Dim lrInterfacePage As New Viev.FBM.Interface.Page
                        lrInterfacePage.Id = arConceptInstance.PageId

                        Dim lrInterfaceConceptInstance As New Viev.FBM.Interface.ConceptInstance
                        lrInterfaceConceptInstance.ModelElementId = arFactType.Id
                        lrInterfaceConceptInstance.X = arConceptInstance.X
                        lrInterfaceConceptInstance.Y = arConceptInstance.Y

                        lrInterfacePage.ConceptInstance = lrInterfaceConceptInstance
                        lrInterfaceModel.Page = lrInterfacePage
                    End If

                    Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                    lrBroadcast.Model = lrInterfaceModel
                    Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.ModelAddFactType, lrBroadcast)

                    '------------------------------------------------------------------------------------------------------------------
                    'If the FactType already has FactTypeReadings, broadcast those as well.
                    For Each lrFactTypeReading In arFactType.FactTypeReading
                        Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelAddFactTypeReading,
                                                                            lrFactTypeReading,
                                                                            Nothing)
                    Next

                End If
                '=====================================================================================

            End If
        End Sub

        ''' <summary>
        ''' Adds an FBM.DictionaryEntry to the Model's 'ModelDictionary' (if it does not already exist).
        '''   Returns the new or existing DictionaryEntry
        ''' </summary>
        ''' <param name="arDictionaryEntry">The DictionaryEntry being added to the ModelDictionary.</param>
        ''' <param name="abAppendRealisations">Defaults to True. If True the Realisations for the DictionaryEntry is appended with the Concept of the DictionaryEntry</param>
        ''' <param name="abStraightSave">Adds the DictionaryEntry without checking whether it already exists. Use when have already checked</param>
        ''' <remarks>This function can be used to check if a DictionaryEntry already exists in the ModelDictionary. Set abAppendRealisations to False if you don't wish for the Realisations of the DictionaryEntry to be appended with the Concept of the DictionaryEntry.</remarks>
        Public Function AddModelDictionaryEntry(ByRef arDictionaryEntry As FBM.DictionaryEntry,
                                                Optional ByVal abAppendRealisations As Boolean = True,
                                                Optional ByVal abMakeModelDirty As Boolean = True,
                                                Optional ByVal abCheckForErrors As Boolean = False,
                                                Optional ByVal abStraightSave As Boolean = False,
                                                Optional ByVal abMatchCase As Boolean = False,
                                                Optional ByVal abMakeDirtyIfNotExists As Boolean = False,
                                                Optional ByVal abCaseInsensitive As Boolean = False) As FBM.DictionaryEntry

            Dim lrDictionaryEntry As FBM.DictionaryEntry = Nothing
            Dim liInd As Integer

            Try
                If abStraightSave Then
                    lrDictionaryEntry = arDictionaryEntry
                    Me.ModelDictionary.Add(arDictionaryEntry)
                    If abMakeModelDirty Then
                        Me.MakeDirty(False, abCheckForErrors)
                    End If
                    Try
                        Me.Dictionary.Add(lrDictionaryEntry.Symbol, Me.ModelDictionary.Count - 1)
                    Catch ex As Exception
                    End Try
                ElseIf abMatchCase Then
                    If Me.Dictionary.ContainsKey(arDictionaryEntry.Symbol) Then
                        lrDictionaryEntry = Me.ModelDictionary(Me.Dictionary(arDictionaryEntry.Symbol))
                    End If
                ElseIf abCaseInsensitive Then
                    lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf arDictionaryEntry.EqualsLowercase)
                Else
                    If Me.Dictionary.TryGetValue(arDictionaryEntry.Symbol, liInd) Then
                        Try
                            lrDictionaryEntry = Me.ModelDictionary(liInd)
                        Catch ex As Exception
                            lrDictionaryEntry = Nothing
                        End Try
                    Else
                        lrDictionaryEntry = Nothing
                    End If

                    If lrDictionaryEntry IsNot Nothing Then
                        If LCase(lrDictionaryEntry.Symbol) <> LCase(arDictionaryEntry.Symbol) Then
                            'Will sometimes get here because removing dictionary entries does not change their value (in Key/Value pair).
                            '  The bonus of this strategy is that when initially loading a model, cuts load time by 2/3rds.
                            lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf arDictionaryEntry.Equals)
                        End If
                    End If
                End If

                If lrDictionaryEntry IsNot Nothing Then
                    '-------------------------------------------------------------------------------------------------------
                    'Concept already exists in the ModelDictionary. Effectively we are updating the DictionaryEntry.
                    ' Make sure the DictionaryEntry contains the ConceptType of the DictionaryEntry attempted to be added.
                    '-------------------------------------------------------------------------------------------------------                    
                    lrDictionaryEntry.AddConceptType(arDictionaryEntry.GetConceptType)
                    '20220129-VM-Commented out below. AddConceptType above adds a realisation.
                    'If abAppendRealisations Then
                    '    'CodeSafe - Only allow multiple Value realisations.
                    '    lrDictionaryEntry.AddRealisation(arDictionaryEntry.ConceptType, arDictionaryEntry.ConceptType <> pcenumConceptType.Value)
                    'End If
                    If lrDictionaryEntry.isFactType And arDictionaryEntry.DBName <> "" Then
                        lrDictionaryEntry.DBName = arDictionaryEntry.DBName
                    End If
                Else
                    '----------------------------------------------
                    'Add a the new Concept to the ModelDictionary
                    '----------------------------------------------    
                    If abAppendRealisations Then
                        'CodeSafe - Only allow multiple Value realisations.
                        arDictionaryEntry.AddRealisation(arDictionaryEntry.ConceptType, arDictionaryEntry.ConceptType <> pcenumConceptType.Value)
                    End If
                    arDictionaryEntry.isDirty = abMakeDirtyIfNotExists
                    If Me.Loaded Then
                        If Me.Page.FindAll(Function(x) x.Loaded = False).Count > 0 Then arDictionaryEntry.isDirty = True
                    End If
                    Me.ModelDictionary.Add(arDictionaryEntry)
                    If abMakeModelDirty Then
                        Me.MakeDirty(False, abCheckForErrors)
                    End If
                    lrDictionaryEntry = arDictionaryEntry
                    Try
                        Me.Dictionary.Add(lrDictionaryEntry.Symbol, Me.ModelDictionary.Count - 1)
                    Catch ex As Exception
                        'Can ignore at this stage
                        'Debugger.Break()
                    End Try

                End If

                Return lrDictionaryEntry

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return lrDictionaryEntry 'Which may be Nothing.
            End Try

        End Function

        Public Sub checkForErrors()

            Try
                Dim lrValidator = New Validation.ModelValidator(Me)
                Call lrValidator.CheckForErrors()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub checkIfCanCheckForErrors()

            If Me.Loading Or Me.RDSLoading Or Me.STMLoading Or Me.Page.FindAll(Function(x) x.Loading).Count > 0 Then
            Else
                Me.AllowCheckForErrors = True
                Dim lrValidator As New Validation.ModelValidator(Me)
                Call lrValidator.CheckForErrors()
            End If
        End Sub


        ''' <summary>
        ''' Creates a new EntityType for the Model (including a ValueType and FactType for the ReferenceMode of the EntityType)
        ''' PRECONDITION: Have already checked to see that the asEntityTypeName does not clash with another ModelObject.
        ''' </summary>
        ''' <returns>tEntityType</returns>
        ''' <remarks></remarks>
        Public Function CreateEntityType(Optional ByVal asEntityTypeName As String = Nothing,
                                         Optional ByVal abAddToModel As Boolean = True) As FBM.EntityType

            Dim lrEntityType As FBM.EntityType

            Try
                Dim lsNewUniqueName As String = ""

                If IsSomething(asEntityTypeName) Then
                    lsNewUniqueName = asEntityTypeName
                Else
                    lsNewUniqueName = "NewEntityType"
                End If

                If Me.ExistsModelElement(lsNewUniqueName) Then
                    lsNewUniqueName = Me.CreateUniqueEntityTypeName(lsNewUniqueName, 0)
                End If

                '--------------------------------------------------------------------
                'Create the FBM.tEntityType to return (with the lsNewUniqueName)
                '--------------------------------------------------------------------
                lrEntityType = New FBM.EntityType(Me, pcenumLanguage.ORMModel, lsNewUniqueName, lsNewUniqueName, Nothing)

                Dim lrDictionaryEntry = New FBM.DictionaryEntry(Me, lsNewUniqueName, pcenumConceptType.EntityType, , , True, True)
                lrDictionaryEntry = Me.AddModelDictionaryEntry(lrDictionaryEntry)
                lrDictionaryEntry.isDirty = True
                lrEntityType.Concept = lrDictionaryEntry.Concept

                '-----------------------------------------
                'Add the new EntityType to the Model
                '-----------------------------------------
                If abAddToModel Then
                    Me.AddEntityType(lrEntityType, False)
                    Call Me.MakeDirty()
                End If

                Return lrEntityType

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tORMModel.CreateEntityType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Function GetFactTypesByEntityType(ByVal ar_entity_type As FBM.EntityType) As List(Of FBM.FactType)
            '------------------------------------------------------------------
            'Returns a list of all the FactTypes associated with an EntityType
            '------------------------------------------------------------------
            Dim lrFactType As FBM.FactType
            Dim lrRole As FBM.Role

            GetFactTypesByEntityType = New List(Of FBM.FactType)

            For Each lrFactType In Me.FactType
                For Each lrRole In lrFactType.RoleGroup
                    If lrRole.JoinedORMObject Is ar_entity_type Then
                        GetFactTypesByEntityType.Add(lrFactType)
                    End If
                Next
            Next
        End Function

        Public Function getFactTypeByModelObjects(ByVal aarModelObject As List(Of FBM.ModelObject)) As List(Of FBM.FactType)

            Try
                Dim [set] = New HashSet(Of FBM.ModelObject)(aarModelObject)

                Dim larFactType = From FactType In Me.FactType
                                  Where FactType.RoleGroup.Count = aarModelObject.Count
                                  Where [set].SetEquals(FactType.ModelObjects)
                                  Select FactType

                Return larFactType.ToList

            Catch ex As Exception
                Return New List(Of FactType)
            End Try

        End Function

        ''' <summary>
        ''' 202007-VM-Created
        ''' Returns a FactType, else Nothing, if the list of ModelObjects and FactTypeReading match.
        '''   * Used to thwart attempts to create more than one FactType with the same reading.
        ''' </summary>
        ''' <param name="aarModelObject"></param>
        ''' <param name="arFactTypeReading"></param>
        ''' <returns></returns>
        Public Function getFactTypeByModelObjectsFactTypeReading(ByVal aarModelObject As List(Of FBM.ModelObject),
                                                                 ByVal arFactTypeReading As FBM.FactTypeReading,
                                                                 Optional ByVal abUseFastenshtein As Boolean = False,
                                                                 Optional ByRef arReturnFactTypeReading As FBM.FactTypeReading = Nothing,
                                                                 Optional ByRef arReturnPredicatePart As FBM.PredicatePart = Nothing) As FBM.FactType

            Try
                '------------------------------------------------------
                'Check to see if the ModelObjects are in the FactType
                Dim larFactType = From FactType In Me.FactType
                                  Where FactType.RoleGroup.Count = aarModelObject.Count
                                  From Role In FactType.RoleGroup
                                  From ModelObject In aarModelObject
                                  Where Role.JoinedORMObject.Id = ModelObject.Id
                                  Select FactType Distinct

                If larFactType.Count = 0 Then
                    Return Nothing
                Else
                    Dim larFTRFactTypeReading = From FactType In larFactType
                                                From FactTypeReading In FactType.FactTypeReading
                                                Where arFactTypeReading.EqualsByRoleJoinedModelObjectSequence(FactTypeReading)
                                                Where arFactTypeReading.EqualsByPredicatePartText(FactTypeReading, abUseFastenshtein)
                                                Select FactTypeReading

                    If larFTRFactTypeReading.Count = 0 Then
                        Return Nothing
                    Else
                        arReturnFactTypeReading = larFTRFactTypeReading.First
                        If aarModelObject.Count = 2 Then
                            arReturnPredicatePart = arReturnFactTypeReading.PredicatePart(0)
                        End If
                        Return larFTRFactTypeReading.First.FactType
                    End If
                End If

                Return Nothing

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function getFactTypeByPartialMatchModelObjectsFactTypeReading(ByVal aarModelObject As List(Of FBM.ModelObject),
                                                                             ByVal arFactTypeReading As FBM.FactTypeReading,
                                                                             Optional ByVal abReturnPartialMatchIfFound As Boolean = False) As List(Of FBM.FactType)

            Try
                '------------------------------------------------------
                'Check to see if the ModelObjects are in the FactType
                Dim larFactType = From FactType In Me.FactType
                                  Where FactType.RoleGroup.FindAll(Function(x) aarModelObject.Contains(x.JoinedORMObject)).Count >= aarModelObject.Count
                                  Select FactType Distinct

                '20210803-VM-Removed Where FactType.RoleGroup.Count = aarModelObject.Count

                If larFactType.Count = 0 Then
                    Return New List(Of FBM.FactType)
                Else
                    Dim larFTRFactType = From FactType In larFactType
                                         From FactTypeReading In FactType.FactTypeReading
                                         Where arFactTypeReading.EqualsByRoleJoinedModelObjectSequence(FactTypeReading)
                                         Where arFactTypeReading.EqualsByPredicatePartText(FactTypeReading)
                                         Select FactType


                    If larFTRFactType.Count = 0 Then

                        'Try and find partial match. E.g. TernaryFactType where FactTypeReading matches 1 of the PredicateParts
                        'Used for QueryEdges in the FactEngine and only where arFactTypeReading is for BinaryFactType
                        If arFactTypeReading.PredicatePart.Count = 2 Then

                            larFactType = From FactType In Me.FactType
                                          Where FactType.RoleGroup.Count > aarModelObject.Count
                                          Where FactType.RoleGroup.FindAll(Function(x) aarModelObject.Contains(x.JoinedORMObject)).Count >= aarModelObject.Count
                                          Select FactType Distinct


                            'larFactType = From FactType In Me.FactType
                            '              Where FactType.RoleGroup.Count > aarModelObject.Count
                            '              From Role In FactType.RoleGroup
                            '              Where aarModelObject.Contains(Role.JoinedORMObject)
                            '              Where FactType.RoleGroup.FindAll(Function(x) aarModelObject.Contains(Role.JoinedORMObject)).Count = aarModelObject.Count
                            '              Select FactType Distinct

                            larFTRFactType = From FactType In larFactType
                                             From FactTypeReading In FactType.FactTypeReading
                                             Where arFactTypeReading.EqualsPartiallyByRoleJoinedModelObjectSequence(FactTypeReading)
                                             Where arFactTypeReading.EqualsPartiallyByPredicatePartText(FactTypeReading)
                                             Select FactType Distinct

                            If larFTRFactType.Count = 0 Then
                                Return New List(Of FBM.FactType)
                            Else
                                Return larFTRFactType.ToList
                            End If

                        Else
                            Return New List(Of FBM.FactType)
                        End If
                    Else
                        Return larFTRFactType.ToList
                    End If
                End If

                Return Nothing

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.FactType)
            End Try

        End Function

        Public Function getFactTypeReadingByPartialPredicateReading(ByVal asPredicatePart As String,
                                                                    ByVal asModelElementName As String,
                                                                    ByRef aarPredicatePart As List(Of FBM.PredicatePart)) As FBM.FactTypeReading

            Try
                Dim larPredicatePartReturn As New List(Of FBM.PredicatePart)

                Dim larPredicatePart = From FactType In Me.FactType.FindAll(Function(x) x.Arity > 2)
                                       From FactTypeReading In FactType.FactTypeReading
                                       Where FactTypeReading.PredicatePart(0).PredicatePartText = asPredicatePart
                                       Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id = asModelElementName
                                       Select FactTypeReading.PredicatePart(0)


                If larPredicatePart.Count = 1 Then
                    larPredicatePartReturn.Add(larPredicatePart.First)
                    aarPredicatePart = larPredicatePartReturn
                    Return larPredicatePart.First.FactTypeReading
                Else
                    aarPredicatePart = larPredicatePart.ToList
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
        ''' Creates a random fact for the specified FactType.
        ''' </summary>
        ''' <param name="arFactType">The FactType for which a random Fact will be created.</param>
        ''' <returns>A Fact with random FactData.</returns>
        ''' <remarks></remarks>
        Public Function CreateFact(ByRef arFactType As FBM.FactType, Optional ByVal aasData As List(Of String) = Nothing, Optional ByVal abMakeDirty As Boolean = False) As FBM.Fact

            Dim lsMessage As String = ""
            Dim lrRole As FBM.Role
            Dim lrConcept As FBM.Concept
            Dim lrFactData As FBM.FactData
            Dim lrFact As FBM.Fact

            Try

                lrFact = New FBM.Fact(arFactType)
                lrFact.isDirty = abMakeDirty

                Call Me.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me, lrFact.Id, pcenumConceptType.Fact))


                Dim liInd As Integer = 0

                For Each lrRole In arFactType.RoleGroup

                    '-----------------------------------------------------------------------------------------
                    'If the Role references a FactType, the referenced FactType must have at least one Fact.
                    '-----------------------------------------------------------------------------------------
                    If lrRole.TypeOfJoin = pcenumRoleJoinType.FactType Then
                        '---------------------------------------------------------------
                        'The new FactData must reference a Fact in the joined FactType
                        '---------------------------------------------------------------
                        Dim lrJoinedFactType As New FBM.FactType
                        lrJoinedFactType = lrRole.JoinedORMObject
                        If lrJoinedFactType.Fact.Count = 0 Then
                            lsMessage = "Cannot create new Fact" & vbCrLf & vbCrLf & " The Fact Type, '" & arFactType.Name & "', references Fact Type, '" & lrJoinedFactType.Name & "', but no Facts have been created for Fact Type, '" & lrJoinedFactType.Name & "'"
                            Throw New Exception(lsMessage)
                        Else
                            lrConcept = New FBM.Concept(lrJoinedFactType.Fact(0).Symbol)
                        End If
                    Else
                        lrConcept = New FBM.Concept()
                    End If

                    If aasData Is Nothing Then
                        '==========================================================================================================================
                        'Create the Symbol of the Concept based on the DataType of the ObjectType referenced by the Role.
                        Dim liORMDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
                        Dim liDataTypeLength As Integer = 10 '10 to be on the Safe side. Someone may have put the DataTypeLength as 0. Code Safe.
                        Dim lrValueType As FBM.ValueType
                        Dim lrEntityType As FBM.EntityType
                        Select Case lrRole.TypeOfJoin
                            Case Is = pcenumRoleJoinType.ValueType
                                lrValueType = lrRole.JoinedORMObject
                                liORMDataType = lrValueType.DataType
                                liDataTypeLength = lrValueType.DataTypeLength
                            Case Is = pcenumRoleJoinType.EntityType
                                lrEntityType = lrRole.JoinedORMObject
                                If lrEntityType.HasSimpleReferenceScheme Then
                                    liORMDataType = lrEntityType.getDataType() 'lrEntityType.ReferenceModeValueType.DataType
                                    liDataTypeLength = lrEntityType.getDataTypeLength
                                End If
                        End Select

                        Select Case liORMDataType
                            Case Is = pcenumORMDataType.DataTypeNotSet
                                '---------------------------------------------------------------
                                'Do Nothing, just use the random GUID created for the Concept.
                                '---------------------------------------------------------------
                            Case Is = pcenumORMDataType.NumericAutoCounter
                                Dim liHighestInteger As Integer
                                Dim liDummyInteger As Integer
                                Select Case lrRole.TypeOfJoin
                                    Case Is = pcenumRoleJoinType.ValueType
                                        lrValueType = lrRole.JoinedORMObject
                                        If (lrValueType.Instance.Count = 0) Or
                                            lrValueType.Instance.FindAll(Function(x) Integer.TryParse(Regex.Replace(x, "[^\d]", ""), liDummyInteger)).Count = 0 Then
                                            liHighestInteger = 1
                                        Else
                                            liHighestInteger = CInt(lrValueType.Instance.FindAll(Function(x) Integer.TryParse(Regex.Replace(x, "[^\d]", ""), liDummyInteger)).Max) + 1
                                        End If
                                    Case Is = pcenumRoleJoinType.EntityType
                                        lrEntityType = lrRole.JoinedORMObject
                                        If (lrEntityType.Instance.Count = 0) Or
                                        lrEntityType.Instance.FindAll(Function(x) Integer.TryParse(Regex.Replace(x, "[^\d]", ""), liDummyInteger)).Count = 0 Then
                                            liHighestInteger = 1
                                        Else
                                            liHighestInteger = CInt(lrEntityType.Instance.FindAll(Function(x) Integer.TryParse(Regex.Replace(x, "[^\d]", ""), liDummyInteger)).Max) + 1
                                        End If
                                End Select
                                lrConcept.Symbol = liHighestInteger.ToString
                            Case Else
                                lrConcept.Symbol = publicORMFunctions.CreateRandomFactData(liORMDataType, liDataTypeLength)
                        End Select
                    Else
                        lrConcept.Symbol = aasData(liInd)
                    End If

                    '==========================================================================================================================
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry

                    lrDictionaryEntry = Me.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me, lrConcept.Symbol, pcenumConceptType.Value))

                    lrFactData = New FBM.FactData(lrRole, lrDictionaryEntry.Concept, lrFact)
                    lrFactData.isDirty = abMakeDirty
                    lrFact.Data.Add(lrFactData)

                    If Not lrRole.JoinedORMObject.Instance.Exists(Function(x) x = lrFactData.Data) Then
                        Select Case lrRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                '-------------------------------------------------------------------------------------------------
                                'Special processing for EntityTypes, because data Instances are recursively added to all Parents
                                '-------------------------------------------------------------------------------------------------
                                Dim lrEnityType As New FBM.EntityType
                                lrEnityType = lrRole.JoinedORMObject
                                lrEnityType.AddDataInstance(lrFactData.Data)

                            Case Is = pcenumConceptType.ValueType,
                                      pcenumConceptType.FactType
                                lrRole.JoinedORMObject.Instance.Add(lrFactData.Data)
                        End Select

                    End If

                    liInd += 1
                Next

                Return lrFact

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
        ''' Creates a new FactType and adds it to the Model if required.
        ''' Client/Server: Doesn't broadcast any event within this method.
        ''' </summary>
        ''' <param name="asFactTypeName"></param>
        ''' <param name="aarReferencedObject"></param>
        ''' <param name="abIsReferenceModeFactType"></param>
        ''' <param name="abMakeModelDirty"></param>
        ''' <param name="abIsLinkFactType"></param>
        ''' <param name="arLinkFactTypeRole"></param>
        ''' <param name="abAddtoModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateFactType(ByVal asFactTypeName As String,
                                       Optional ByRef aarReferencedObject As List(Of FBM.ModelObject) = Nothing,
                                       Optional ByVal abIsReferenceModeFactType As Boolean = False,
                                       Optional ByVal abMakeModelDirty As Boolean = True,
                                       Optional ByVal abIsLinkFactType As Boolean = False,
                                       Optional ByVal arLinkFactTypeRole As FBM.Role = Nothing,
                                       Optional ByVal abAddtoModel As Boolean = True,
                                       Optional ByRef arPage As FBM.Page = Nothing) As FBM.FactType

            Try
                Dim lrModelObject As FBM.ModelObject
                Dim lrFactType As New FBM.FactType(Me, asFactTypeName, True)
                Dim lsNewUniqueName As String = ""

                lsNewUniqueName = Me.CreateUniqueFactTypeName(lrFactType.Name, 0)

                '--------------------------------------------------------------------
                'Create the FBM.FactType to return (with the lsNewUniqueName)
                '--------------------------------------------------------------------
                lrFactType = New FBM.FactType(Me, lsNewUniqueName, True)
                lrFactType.isDirty = True

                If abIsReferenceModeFactType Then
                    lrFactType.IsPreferredReferenceMode = abIsReferenceModeFactType
                End If

                If abIsLinkFactType And (arLinkFactTypeRole Is Nothing) Then
                    Throw New Exception("Can't be both a LinkFactType and have no LinkFactTypeRole.")
                End If

                lrFactType.IsLinkFactType = abIsLinkFactType
                lrFactType.LinkFactTypeRole = arLinkFactTypeRole

                '----------------------------------------------------------------------
                'Crate the Roles within the FactType for the SelectedObjects
                '----------------------------------------------------------------------
                If IsSomething(aarReferencedObject) Then
                    For Each lrModelObject In aarReferencedObject

                        '---------------------------------------------------
                        'Create a new Role for the RoleInstance
                        '  and put it within the RoleGroup of the FactType
                        '---------------------------------------------------
                        Call lrFactType.CreateRole(lrModelObject, False, False)

                    Next 'For each Object in the SelectedObject list
                End If

                lrFactType.makeDirty()

                '----------------------------------
                'Add the new FactType to the Model
                '----------------------------------
                If abAddtoModel Then
                    Dim lrConceptInstance As FBM.ConceptInstance
                    If arPage IsNot Nothing Then
                        lrConceptInstance = New FBM.ConceptInstance(Me, arPage, pcenumConceptType.FactType)
                        lrConceptInstance.X = 50
                        lrConceptInstance.Y = 50
                    Else
                        lrConceptInstance = Nothing
                    End If

                    Me.AddFactType(lrFactType, abMakeModelDirty, True, lrConceptInstance)
                End If

                '-----------------------
                'Return the new FactType
                '-----------------------
                Return lrFactType

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function CreateModelNote() As FBM.ModelNote

            Dim lrModelNote As New FBM.ModelNote(Me)

            Me.AddModelNote(lrModelNote)

            Return lrModelNote

        End Function

        ''' <summary>
        ''' Creates a unique EntityType.Id/Name/Symbol
        ''' </summary>
        ''' <param name="asRootEntityTypeName">The root name to start with.</param>
        ''' <param name="aiCounter">Start at 0.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateUniqueEntityTypeName(ByVal asRootEntityTypeName As String, ByVal aiCounter As Integer) As String

            Dim lsTrialEntityTypeName As String

            If aiCounter = 0 Then
                lsTrialEntityTypeName = asRootEntityTypeName
            Else
                lsTrialEntityTypeName = asRootEntityTypeName & CStr(aiCounter)
            End If

            Dim lrEntityType As New FBM.EntityType(Me, pcenumLanguage.ORMModel, lsTrialEntityTypeName, lsTrialEntityTypeName)
            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, lsTrialEntityTypeName, pcenumConceptType.EntityType)

            CreateUniqueEntityTypeName = lsTrialEntityTypeName

            If Me.EntityType.Exists(AddressOf lrEntityType.EqualsByName) Or
               TableEntityType.ExistsEntityType(lrEntityType) Or
                Me.ExistsModelElement(lsTrialEntityTypeName) Or
                Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsBySymbol) Then
                CreateUniqueEntityTypeName = Me.CreateUniqueEntityTypeName(asRootEntityTypeName, aiCounter + 1)
            Else
                Return lsTrialEntityTypeName
            End If

        End Function

        Public Function CreateUniqueFactTypeName(ByVal asRootFactTypeName As String,
                                                 ByVal aiCounter As Integer,
                                                 Optional ByVal abIncludeDatabaseLookup As Boolean = True) As String

            Dim lsTrialFactTypeName As String

            If aiCounter = 0 Then
                lsTrialFactTypeName = asRootFactTypeName
            Else
                lsTrialFactTypeName = asRootFactTypeName & CStr(aiCounter)
            End If

            Dim lrFactType As New FBM.FactType(Me, lsTrialFactTypeName, True)
            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, lsTrialFactTypeName, pcenumConceptType.FactType)

            CreateUniqueFactTypeName = lsTrialFactTypeName

            If Me.FactType.Exists(AddressOf lrFactType.EqualsByName) Or
               Me.ExistsModelElement(lsTrialFactTypeName) Or
               Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsBySymbol) Then
                CreateUniqueFactTypeName = Me.CreateUniqueFactTypeName(asRootFactTypeName, aiCounter + 1)
            ElseIf abIncludeDatabaseLookup Then
                If TableFactType.ExistsFactType(lrFactType) Then
                    CreateUniqueFactTypeName = Me.CreateUniqueFactTypeName(asRootFactTypeName, aiCounter + 1)
                End If
            Else
                Return lsTrialFactTypeName
            End If

        End Function

        ''' <summary>
        ''' Queries the database and creates a unique Model Name based on the given asModelName and liSequenceNr arguments.
        ''' </summary>
        ''' <param name="asModelName">The base Model Name from which a unique Model Name will be derived.</param>
        ''' <param name="liSequenceNr">The starting Sequence Number to be extended to the Model Name to create a new and unique Model Name. Increments by 1 for each failed test of uniqueness. NB Start at 0.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateUniqueModelName(ByVal asModelName As String, ByVal liSequenceNr As Integer) As String

            Dim lsModelId As String
            Dim lbModelNameIsUnique As Boolean = True

            Try
                If liSequenceNr = 0 Then
                    lsModelId = asModelName
                Else
                    lsModelId = asModelName & liSequenceNr.ToString
                End If

                If TableModel.ExistsModelByName(lsModelId) Then
                    lbModelNameIsUnique = False
                Else
                    lbModelNameIsUnique = True
                End If

                If lbModelNameIsUnique Then
                    Return lsModelId
                Else
                    Return Me.CreateUniqueModelName(asModelName, liSequenceNr + 1)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return "NewTempModel"
            End Try

        End Function

        Public Function CreateUniquePageName(ByVal asRootPageName As String, ByVal aiCounter As Integer) As String

            Dim lsTrialPageName As String = ""

            If aiCounter = 0 Then
                lsTrialPageName = asRootPageName
            Else
                lsTrialPageName = asRootPageName & " " & CStr(aiCounter)
            End If

            Dim lrPage As New FBM.Page(Me, pcenumLanguage.ORMModel, lsTrialPageName, pcenumLanguage.ORMModel)

            CreateUniquePageName = lsTrialPageName

            If Me.Page.Exists(AddressOf lrPage.EqualsByName) Or TablePage.ExistsPageById(lrPage.PageId) Then
                CreateUniquePageName = Me.CreateUniquePageName(asRootPageName, aiCounter + 1)
            Else
                Return lsTrialPageName
            End If

        End Function

        Public Function CreateUniqueRoleConstraintName(ByVal asRootRoleConstraintName As String, ByVal aiCounter As Integer) As String

            Dim lsTrialRoleConstraintName As String
            If aiCounter = 0 Then
                lsTrialRoleConstraintName = asRootRoleConstraintName
            Else
                lsTrialRoleConstraintName = asRootRoleConstraintName & CStr(aiCounter)
            End If

            Dim lrRoleConstraint As New FBM.RoleConstraint(Me, lsTrialRoleConstraintName, True)
            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, lsTrialRoleConstraintName, pcenumConceptType.RoleConstraint)

            CreateUniqueRoleConstraintName = lsTrialRoleConstraintName

            If Me.RoleConstraint.Exists(AddressOf lrRoleConstraint.EqualsByName) Or
               TableRoleConstraint.ExistsRoleConstraint(lrRoleConstraint) Or
               Me.ExistsModelElement(lsTrialRoleConstraintName) Or
               Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsBySymbol) Then

                'Shortcut
                Dim liCounter = Me.RoleConstraint.FindAll(Function(x) x.Id.StartsWith(asRootRoleConstraintName)).Count
                If liCounter >= aiCounter Then
                    aiCounter = liCounter + 1
                End If

                CreateUniqueRoleConstraintName = Me.CreateUniqueRoleConstraintName(asRootRoleConstraintName, aiCounter + 1)
            Else
                Return lsTrialRoleConstraintName
            End If

        End Function

        Public Function CreateUniqueValueTypeName(ByVal asRootValueTypeName As String, ByVal aiCounter As Integer) As String

            Dim lsTrialValueTypeName As String
            If aiCounter = 0 Then
                lsTrialValueTypeName = asRootValueTypeName
            Else
                lsTrialValueTypeName = asRootValueTypeName & CStr(aiCounter)
            End If

            Dim lrValueType As New FBM.ValueType(Me, pcenumLanguage.ORMModel, lsTrialValueTypeName, True)
            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, lsTrialValueTypeName, pcenumConceptType.ValueType)

            CreateUniqueValueTypeName = lsTrialValueTypeName


            If Me.ValueType.Exists(AddressOf lrValueType.EqualsByName) Or
               TableValueType.ExistsValueType(lrValueType) Or
               Me.ExistsModelElement(lsTrialValueTypeName) Then
                CreateUniqueValueTypeName = Me.CreateUniqueValueTypeName(asRootValueTypeName, aiCounter + 1)
            ElseIf Me.ModelDictionary.Find(Function(x) x.Symbol = lrDictionaryEntry.Symbol And x.isValue) IsNot Nothing Then
                Return lsTrialValueTypeName
            Else
                Return lsTrialValueTypeName
            End If

        End Function

        ''' <summary>
        ''' Creates a ValueType and adds it to the model.
        ''' </summary>
        ''' <param name="asValueTypeName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateValueType(Optional ByVal asValueTypeName As String = Nothing,
                                        Optional ByVal abAddtoModel As Boolean = True,
                                        Optional ByVal aiDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet,
                                        Optional ByVal aiDataTypeLength As Integer = 0,
                                        Optional ByVal aiDataTypePrecision As Integer = 0) As FBM.ValueType

            Dim lsNewUniqueName As String = ""
            Dim lrValueType As FBM.ValueType

            If IsSomething(asValueTypeName) Then
                lrValueType = New FBM.ValueType(Me, pcenumLanguage.ORMModel, asValueTypeName, True)
            Else
                lrValueType = New FBM.ValueType(Me, pcenumLanguage.ORMModel, "NewValueType", True)
            End If

            lsNewUniqueName = Me.CreateUniqueValueTypeName(lrValueType.Name, 0)

            '--------------------------------------------------------------------
            'Create the FBM.tValueType to return (with the lsNewUniqueName)
            '--------------------------------------------------------------------
            lrValueType = New FBM.ValueType(Me, pcenumLanguage.ORMModel, lsNewUniqueName, True)

            lrValueType.DataType = aiDataType
            lrValueType.DataTypeLength = aiDataTypeLength
            lrValueType.DataTypePrecision = aiDataTypePrecision

            '-----------------------------------------
            'Add the new ValueType to the Model
            '-----------------------------------------
            If abAddtoModel Then
                Me.AddValueType(lrValueType)
                Call Me.MakeDirty()
            End If

            Return lrValueType

        End Function

        ''' <summary>
        ''' Creates a new Role Constraint for a Model.
        '''   NB Adds a Dictionary Entry for the Role Constraint in the Model's Model Dictionary.
        ''' </summary>
        ''' <param name="aiRoleConstraintType"></param>
        ''' <param name="aarRole"></param>
        ''' <returns>A new unique Role Constraint, as created for the Model.</returns>
        ''' <remarks></remarks>
        Public Function CreateRoleConstraint(ByVal aiRoleConstraintType As pcenumRoleConstraintType,
                                             Optional ByVal aarRole As List(Of FBM.Role) = Nothing,
                                             Optional ByVal asRoleConstraintName As String = Nothing,
                                             Optional ByVal aiLevelNr As Integer = Nothing,
                                             Optional ByVal abMakeModelDirty As Boolean = False,
                                             Optional ByVal abAddToModel As Boolean = True
                                             ) As FBM.RoleConstraint

            Try
                Dim lrRoleConstraint As FBM.RoleConstraint

                If IsSomething(asRoleConstraintName) Then
                    lrRoleConstraint = New FBM.RoleConstraint(Me, asRoleConstraintName, True, aiRoleConstraintType, aarRole, True)
                Else
                    lrRoleConstraint = New FBM.RoleConstraint(Me, aiRoleConstraintType.ToString, True, aiRoleConstraintType, aarRole, True)
                End If

                If IsSomething(aiLevelNr) Then
                    lrRoleConstraint.LevelNr = aiLevelNr
                End If

                Dim lsNewUniqueName As String = ""

                lsNewUniqueName = Me.CreateUniqueRoleConstraintName(lrRoleConstraint.Name, 1)

                '--------------------------------------------------------------------
                'Create the FBM.RoleConstraint to return (with the lsNewUniqueName)
                '--------------------------------------------------------------------
                lrRoleConstraint = New FBM.RoleConstraint(Me, lsNewUniqueName, True, aiRoleConstraintType, aarRole, True)
                If aiRoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    lrRoleConstraint.LevelNr = aiLevelNr
                End If

                '-----------------------------------------
                'Add the new RoleConstraint to the Model
                '-----------------------------------------
                If abAddToModel Then
                    Me.AddRoleConstraint(lrRoleConstraint, abMakeModelDirty, True)
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

        Public Sub GenerateTestCases()
            '-------------------------------------------------
            'Automatically generates TestCases for the Model
            '-------------------------------------------------

            '------------------------------------------------------------------------------
            'PSEUDOCODE
            '--------------
            '
            ' * FOR EACH EntityType within the Model
            '     * Create Test Cases for
            '        * Manditory Roles
            '        * 1:1 BinaryRoles stemming from the EntityType
            ' * LOOP
            '
            ' * FOR EACH ValueType within the Model
            '     * Create Test Cases for
            '        * StateTransitions for the ValueType if it has a ValueConstraint with an associated StateTransitionDiagram
            ' * LOOP
            '
            ' * FOR EACH ObjectifiedFactType within the Model
            '     * Create Test Cases for
            '        * 
            ' * LOOP
            '
            '------------------------------------------------------------------------------

        End Sub

        ''' <summary>
        ''' Deprecates the Realisations for a DictionaryEntry in the ModelDictionary
        ''' </summary>
        ''' <param name="arDictionaryEntry"></param>
        ''' <remarks></remarks>
        Public Sub DeprecateRealisationsForDictionaryEntry(ByRef arDictionaryEntry As FBM.DictionaryEntry, ByVal aiConceptType As pcenumConceptType)

            Try
                Dim lrDictionarEntry As FBM.DictionaryEntry

                lrDictionarEntry = Me.ModelDictionary.Find(AddressOf arDictionaryEntry.Equals)

                If IsSomething(lrDictionarEntry) Then

                    Dim liConceptType As pcenumConceptType = aiConceptType 'arDictionaryEntry.ConceptType
                    Dim liInd As Integer = lrDictionarEntry.Realisations.FindLastIndex(Function(x) x = liConceptType)
                    If liInd >= 0 Then
                        lrDictionarEntry.Realisations.RemoveAt(liInd)
                    Else
                        lrDictionarEntry.removeConceptType(liConceptType)
                    End If

                    '-----------------------------------------------------------------------------------------
                    'CodeSafe: If the Realisations.Count is less than 0, then definitely want to remove the
                    '  DictionaryEntry from the ModelDiction and the database.
                    '-----------------------------------------------------------------------------------------
                    If (lrDictionarEntry.Realisations.Count <= 0) And Not lrDictionarEntry.isGeneralConcept Then
                        Me.RemoveDictionaryEntry(lrDictionarEntry, True)
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

        ''' <summary>
        ''' Empties all Model Objects from the Model.
        '''   NB Does not delete the Model from the database.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EmptyModel()

            Dim liInd As Integer = 0
            Dim lrPage As FBM.Page
            Dim lrEntityType As FBM.EntityType
            Dim lrValueType As FBM.ValueType
            Dim lrFactType As FBM.FactType
            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrModelNote As FBM.ModelNote
            Dim lrDictionaryEntry As FBM.DictionaryEntry

            '=====================================
            'Remove all the Pages from the Model
            '=====================================
            For Each lrPage In Me.Page.ToArray
                Call lrPage.RemoveFromModel()
            Next

            '---------------------------------------------------------
            'Last resort, Delete all the Pages for the Model via SQL
            '---------------------------------------------------------
            TablePage.DeletePagesByModel(Me)

            '==========================================
            'Remove all the ModelNotes from the Model
            '==========================================
            For Each lrModelNote In Me.ModelNote.ToArray
                Call lrModelNote.RemoveFromModel()
            Next

            '----------------------------------------------------------------------------------------------------------
            'Remove all SimpleReferenceModes from EntityTypes that have them....so that the respective
            '  RoleConstraints can be removed from the Model.
            '----------------------------------------------------------------------------------------------------------
            Dim larEntityType = From EntityType In Me.EntityType
                                Where EntityType.HasSimpleReferenceScheme
                                Where EntityType.ReferenceMode <> ""
                                Select EntityType

            For Each lrEntityType In larEntityType
                Call lrEntityType.RemoveSimpleReferenceScheme(False)
            Next

            '===============================================
            'Remove all the RoleConstraints from the Model
            '===============================================
            For liInd = 1 To Me.RoleConstraint.Count
                lrRoleConstraint = New FBM.RoleConstraint
                lrRoleConstraint = Me.RoleConstraint(0)
                Call lrRoleConstraint.RemoveFromModel(False, False)
            Next

            '=========================================
            'Remove all the FactTypes from the Model
            '=========================================
            For Each lrFactType In Me.FactType.ToArray
                Call lrFactType.RemoveFromModel()
            Next

            '-----------------------------------------------------------------------
            'CodeSafe: Last resort, delete all the FactTypes for the Model via SQL
            '-----------------------------------------------------------------------
            Call TableFactType.DeleteFactTypesByModel(Me)

            '===========================================
            'Remove all the EntityTypes from the Model
            '===========================================
            For liInd = 1 To Me.EntityType.Count
                lrEntityType = New FBM.EntityType
                lrEntityType = Me.EntityType(0)
                lrEntityType.PreferredIdentifierRCId = Nothing
                Call lrEntityType.RemoveFromModel(True)
            Next

            '==========================================
            'Remove all the ValueTypes from the Model
            '==========================================
            For liInd = 1 To Me.ValueType.Count
                lrValueType = New FBM.ValueType
                lrValueType = Me.ValueType(0)
                Call lrValueType.RemoveFromModel()
            Next

            '========================================================================
            'Remove all the DictionaryEntries from the ModelDictionary of the Model
            '========================================================================
            For Each lrDictionaryEntry In Me.ModelDictionary
                TableModelDictionary.DeleteModelDictionaryEntry(lrDictionaryEntry)
            Next

            '-----------------------------------------------------------------------
            'CodeSafe: Last resort, delete all the DictionaryEntries for the Model
            '-----------------------------------------------------------------------
            Call TableModelDictionary.DeleteModelDictionaryEntriesByModel(Me)

            Me.ModelDictionary.Clear()

            '----------------------------------------------------------------------
            'Reinstate the Core Model
            '----------------------------------------------------------------------
            Me.CoreVersionNumber = ""
            Call Me.performCoreManagement()

        End Sub

        ''' <summary>
        ''' Removes all Model Objects from the Model and deletes the Model from the database.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveFromDatabase()

            'Dim liInd As Integer = 0
            'Dim lrEntityType As FBM.EntityType
            'Dim lrValueType As FBM.ValueType
            'Dim lrFactType As FBM.FactType
            'Dim lrRoleConstraint As FBM.RoleConstraint
            'Dim lrDictionaryEntry As FBM.DictionaryEntry
            'Dim lrModelNote As FBM.ModelNote

            'For Each lrModelNote In Me.ModelNote.ToArray
            '    Call lrModelNote.RemoveFromModel()
            'Next

            'For Each lrRoleConstraint In Me.RoleConstraint.ToArray
            '    Call lrRoleConstraint.RemoveFromModel()
            'Next

            'For liInd = 1 To Me.FactType.Count
            '    lrFactType = New FBM.FactType
            '    lrFactType = Me.FactType(0)
            '    Call lrFactType.RemoveFromModel()
            'Next

            'For liInd = 1 To Me.EntityType.Count
            '    lrEntityType = New FBM.EntityType
            '    lrEntityType = Me.EntityType(0)
            '    Call lrEntityType.RemoveFromModel(True)
            'Next

            'For liInd = 1 To Me.ValueType.Count
            '    lrValueType = New FBM.ValueType
            '    lrValueType = Me.ValueType(0)
            '    Call lrValueType.RemoveFromModel()
            'Next

            'For Each lrDictionaryEntry In Me.ModelDictionary
            '    TableModelDictionary.DeleteModelDictionaryEntry(lrDictionaryEntry)
            'Next
            'Me.ModelDictionary.Clear()
            RaiseEvent Deleting()

            Call TableModel.DeleteModel(Me)

        End Sub

        Public Function PagesLoading() As Boolean

            Return Me.Page.Find(Function(x) x.Loaded = False) IsNot Nothing
        End Function

        Public Sub RemoveDictionaryEntry(ByRef arDictionaryEntry As FBM.DictionaryEntry,
                                         ByVal abDoDatabaseProcessing As Boolean)

            Dim lsMessage As String

            Try
                If arDictionaryEntry.Realisations.Count <= 1 Then

                    Me.ModelDictionary.Remove(arDictionaryEntry)
                    Me.Dictionary.Remove(arDictionaryEntry.Symbol)

                    If abDoDatabaseProcessing Then
                        TableModelDictionary.DeleteModelDictionaryEntry(arDictionaryEntry)
                    End If
                Else
                    lsMessage = "Tried to remove DictionaryEntry from the ModelDictionary where the DictionaryEntry has more than one Realisation"
                    lsMessage &= vbCrLf & "DictionaryEntry.Symbol: " & arDictionaryEntry.Symbol
                    lsMessage &= vbCrLf & vbCrLf & "Deprecating the Realisations for the DictionaryEntry for this realisation"

                    Dim liConceptType As pcenumConceptType = arDictionaryEntry.ConceptType
                    Dim aiInd As Integer = arDictionaryEntry.Realisations.FindLastIndex(Function(x) x = liConceptType)
                    If aiInd >= 0 Then
                        arDictionaryEntry.Realisations.RemoveAt(aiInd)
                    End If

                    Throw New Exception(lsMessage)
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveEntityType(ByRef arEntityType As FBM.EntityType, ByVal abDoDatabaseProcessing As Boolean)

            Try
                If Me.EntityType.Contains(arEntityType) Then
                    Me.EntityType.Remove(arEntityType)
                End If

                Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, arEntityType.Id, pcenumConceptType.EntityType)

                If Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsByOtherConceptType) Then
                    '----------------------------------------------------------------------------------------------
                    'Don't remove the DictionaryEntry because another Concept (of ConceptType other than EntityType)
                    '  uses the Dictionary entry
                    '----------------------------------------------------------------------------------------------
                    lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                    lrDictionaryEntry.removeConceptType(pcenumConceptType.EntityType)
                    Call TableModelDictionary.UpdateModelDictionaryEntry(lrDictionaryEntry)
                Else
                    lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                    If lrDictionaryEntry.Realisations.Count <= 1 Then
                        Me.RemoveDictionaryEntry(lrDictionaryEntry, abDoDatabaseProcessing)
                    Else
                        Dim lrConcept As New FBM.Concept(arEntityType.Id)
                        lrDictionaryEntry.Realisations.Remove(pcenumConceptType.EntityType)
                    End If
                End If

                '==========================================================================================================
                'Client/Server
                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abDoDatabaseProcessing Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelDeleteEntityType, arEntityType, Nothing)
                End If
                '==========================================================================================================

                '==========================================================================================================
                'RDS
                Dim lrEntityType As FBM.EntityType = arEntityType
                Dim lrTable As RDS.Table = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.Name)

                If lrTable IsNot Nothing Then
                    Call Me.RDS.removeTable(lrTable)
                End If

                Call Me.RemoveModelErrorsForModelObject(arEntityType)

                RaiseEvent StructureModified()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveValueType(ByRef arValueType As FBM.ValueType, ByVal abBroadcastInterfaceEvent As Boolean)


            Try
                If Me.ValueType.Contains(arValueType) Then
                    Me.ValueType.Remove(arValueType)
                End If

                Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, arValueType.Id, pcenumConceptType.ValueType)

                If Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsByOtherConceptType) Then
                    '----------------------------------------------------------------------------------------------
                    'Don't remove the DictionaryEntry because another Concept (of ConceptType other than ValueType)
                    '  uses the Dictionary entry
                    '----------------------------------------------------------------------------------------------
                    lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                    lrDictionaryEntry.removeConceptType(pcenumConceptType.ValueType, True)
                    Call TableModelDictionary.UpdateModelDictionaryEntry(lrDictionaryEntry)
                Else
                    lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                    If lrDictionaryEntry.Realisations.Count <= 1 Then
                        Me.RemoveDictionaryEntry(lrDictionaryEntry, abBroadcastInterfaceEvent)
                    Else
                        Dim lrConcept As New FBM.Concept(arValueType.Id)
                        lrDictionaryEntry.Realisations.Remove(pcenumConceptType.ValueType)
                    End If
                End If

                '==========================================================================================================
                'Client/Server
                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelDeleteValueType, arValueType, Nothing)
                End If
                '==========================================================================================================

                Call Me.RemoveModelErrorsForModelObject(arValueType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveFactType(ByRef arFactType As FBM.FactType, ByVal abDoDatabaseProcessing As Boolean)

            Try
                If Me.FactType.Contains(arFactType) Then
                    Me.FactType.Remove(arFactType)
                End If

                Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, arFactType.Id, pcenumConceptType.FactType)

                If Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsByOtherConceptType) Then
                    '----------------------------------------------------------------------------------------------
                    'Don't remove the DictionaryEntry because another Concept (of ConceptType other than FactType)
                    '  uses the Dictionary entry
                    '----------------------------------------------------------------------------------------------
                    lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                    lrDictionaryEntry.removeConceptType(pcenumConceptType.FactType)
                    Call TableModelDictionary.UpdateModelDictionaryEntry(lrDictionaryEntry)
                Else
                    lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

                    If lrDictionaryEntry Is Nothing Then
                        'Throw New Exception("No dictionary entry exists for the FactType with FactType.Id: '" & arFactType.Id & "'")
                        'Not a biggie, were possibly going to remove it anyway.
                    Else
                        If lrDictionaryEntry.Realisations.Count <= 1 Then
                            Me.RemoveDictionaryEntry(lrDictionaryEntry, abDoDatabaseProcessing)
                        Else
                            lrDictionaryEntry.removeConceptType(pcenumConceptType.FactType)
                        End If
                    End If

                End If

                '==========================================================================================================
                'CMML Processing
                If Me.HasCoreModel Then
                    '----------------------------------------------------------------------------------------
                    'Need to remove the relevant aspects of related Entity/Attributes within ERDiagram Pages within the Model,
                    ' and within the CMML Core within this Model.
                    Call Me.RemoveFactTypeReferencesFromCore(arFactType)
                End If
                '==========================================================================================================

                '==========================================================================================================
                'Client/Server
                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abDoDatabaseProcessing Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelDeleteFactType, arFactType, Nothing)
                End If
                '==========================================================================================================

                Call Me.RemoveModelErrorsForModelObject(arFactType)

                '==========================================================================================================
                'RDS                
                For Each lrRole In arFactType.RoleGroup
                    Call Me.removeColumnsIndexColumnsForRole(lrRole)
                Next

                Dim lrFactType As FBM.FactType = arFactType
                Dim lrTable As RDS.Table = Me.RDS.Table.Find(Function(x) x.Name = lrFactType.Name)

                If lrTable IsNot Nothing Then
                    Call Me.RDS.removeTable(lrTable)
                End If

                RaiseEvent StructureModified()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arRoleConstraint"></param>
        ''' <param name="abCheckForErrors"></param>
        ''' <param name="abBroadcastInterfaceEvent"></param>
        ''' <param name="abReplacingRoleConstraint">If replacing existing RoleConstraint, then don't remove RDSRelation table for the right circumstances.</param>
        Public Sub RemoveRoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint,
                                        ByVal abCheckForErrors As Boolean,
                                        ByVal abBroadcastInterfaceEvent As Boolean,
                                        Optional ByVal abReplacingRoleConstraint As Boolean = False)

            Try
                If Me.RoleConstraint.Contains(arRoleConstraint) Then
                    Me.RoleConstraint.Remove(arRoleConstraint)
                    If arRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                        If arRoleConstraint.RoleConstraintRole.Count > 0 Then
                            arRoleConstraint.RoleConstraintRole(0).Role.FactType.InternalUniquenessConstraint.Remove(arRoleConstraint)
                        End If
                    End If
                End If

                Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, arRoleConstraint.Id, pcenumConceptType.RoleConstraint)

                If Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsByOtherConceptType) Then
                    '----------------------------------------------------------------------------------------------
                    'Don't remove the DictionaryEntry because another Concept (of ConceptType other than RoleConstraint)
                    '  uses the Dictionary entry
                    '----------------------------------------------------------------------------------------------
                    lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                    lrDictionaryEntry.removeConceptType(pcenumConceptType.RoleConstraint)
                    Call TableModelDictionary.UpdateModelDictionaryEntry(lrDictionaryEntry)
                Else
                    lrDictionaryEntry = Me.AddModelDictionaryEntry(lrDictionaryEntry, False, False)
                    If lrDictionaryEntry.Realisations.Count <= 1 Then
                        Me.RemoveDictionaryEntry(lrDictionaryEntry, abBroadcastInterfaceEvent)
                    Else
                        Dim lrConcept As New FBM.Concept(arRoleConstraint.Id)
                        lrDictionaryEntry.Realisations.Remove(pcenumConceptType.RoleConstraint)
                    End If
                End If

                Me.MakeDirty(False, abCheckForErrors)

                '-------------------------------------------------------------------------------------------------
                'Client/Server -Do this before the RDS processing for this method.
                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelDeleteRoleConstraint, arRoleConstraint, Nothing)
                End If

                Call Me.RemoveModelErrorsForModelObject(arRoleConstraint)

                '-------------------------------------------------------------------------------------------------
                'RDS
                '=====================================================================================
                'RDS
                If (Not arRoleConstraint.IsMDAModelElement) And Me.RDSCreated Then ' ContainsLanguage.Contains(pcenumLanguage.EntityRelationshipDiagram) Then 'For now, check this...because otherwise RDS may have no Tables.

                    If arRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                        'Only interested in InternalUniquenessConstraints for forming Columns.

                        Dim lrTable As RDS.Table
                        Dim lrColumn As RDS.Column

                        If arRoleConstraint.impliesSingleColumnForRDSTable Then
                            Dim lrRole As FBM.Role

                            If arRoleConstraint.RoleConstraintRole.Count = 1 Then

                                lrRole = arRoleConstraint.RoleConstraintRole(0).Role

                                Dim larTable = From Table In Me.RDS.Table
                                               From Column In Table.Column
                                               Where Column.Role.Id = lrRole.Id
                                               Select Table Distinct

                                For Each lrTable In larTable.ToList

                                    '---------------------------------------------------------------
                                    'Remove the Index first, because finds the Index by the Column
                                    Call lrTable.removeIndexByRoleConstraint(arRoleConstraint)

                                    '-----------------------------------------
                                    'Remove the Columns
                                    Dim larColumn As New List(Of RDS.Column)
                                    larColumn = lrTable.Column.FindAll(Function(x) x.Role.Id = lrRole.Id)

                                    For Each lrColumn In larColumn.ToList
                                        Call lrTable.removeColumn(lrColumn, Me.IsDatabaseSynchronised)

                                        Dim larRelation = lrTable.getOutgoingRelations.FindAll(Function(x) x.OriginColumns.Contains(lrColumn)) 'Origin
                                        larRelation.AddRange(lrTable.getOutgoingRelations.FindAll(Function(x) x.ReverseDestinationColumns.Contains(lrColumn)))

                                        larRelation.AddRange(lrTable.getIncomingRelations.FindAll(Function(x) x.DestinationColumns.Contains(lrColumn))) 'Destination
                                        larRelation.AddRange(lrTable.getIncomingRelations.FindAll(Function(x) x.ReverseOriginColumns.Contains(lrColumn)))

                                        For Each lrRelation In larRelation.ToList
                                            If lrRelation.is1To1BinaryRelation Then
                                                If lrRelation.OriginColumns.Contains(lrColumn) Then
                                                    'Remove the whole relation and create a ManyToOneRelation. This is much easier than trying to manage reverse relations.
                                                    Call Me.RDS.removeRelation(lrRelation)
                                                    Dim lrOtherRole As FBM.Role = lrColumn.Role.FactType.GetOtherRoleOfBinaryFactType(lrColumn.Role.Id)
                                                    Call Me.generateRelationForManyTo1BinaryFactType(lrOtherRole)
                                                Else
                                                    lrRelation.setOriginMultiplicity(pcenumCMMLMultiplicity.Many)
                                                End If
                                            Else
                                                Call Me.RDS.removeRelation(lrRelation)
                                            End If
                                        Next
                                    Next

                                    '---------------------------------------------------------------------------------------------------
                                    'Decide whether the joined ModelElement is a PGSRelation. Needs first to be an ObjectifiedFactType
                                    If lrRole.JoinedORMObject.GetType Is GetType(FBM.FactType) Then
                                        If lrRole.JoinsFactType.IsObjectified Then
                                            'Should be Objectified if the Role of the RoleConstraint (with one Role) joins the FactType
                                            If (lrRole.JoinsFactType.Arity = 2) Then
                                                If lrRole.JoinsFactType.JoinedFactTypes.Count = 0 Then
                                                    '------------------------------------------------------------------------------------------------------------------
                                                    'The ObjectifiedFactType no longer has any joined FactTypes, so must be a PGSRelation
                                                    Call lrTable.setIsPGSRelation(True)
                                                Else
                                                    If lrRole.JoinsFactType.JoinedRoles.FindAll(Function(x) x.HasInternalUniquenessConstraint).Count = 0 Then
                                                        'No Role joined to the ObjectifiedFactType could possibly be the ResponsibleRole for a Column
                                                        Call lrTable.setIsPGSRelation(True)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                        Else 'RoleConstraint spans multiple Roles. i.e. The FactType is likely Objectified.
                            Dim lrFactType As FBM.FactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType

                            lrTable = Me.RDS.getTableByName(arRoleConstraint.RoleConstraintRole(0).Role.FactType.Id)

                            '==================================================
                            'Remove any Indexes related to the RoleConstraint
                            Dim lrResponsibleRoleConstraint = arRoleConstraint
                            Dim larTable = From Index In Me.RDS.Index
                                           Where Index.ResponsibleRoleConstraint Is lrResponsibleRoleConstraint
                                           Select Index.Table

                            For Each lrTable In larTable
                                '---------------------------------------------------------------
                                'Remove the Index first, because finds the Index by the Column
                                Call lrTable.removeIndexByRoleConstraint(arRoleConstraint)
                            Next

                            '==============================
                            'Remove Columns and Relations
                            If (lrFactType.isRDSTable Or (lrTable IsNot Nothing)) And
                                lrFactType.InternalUniquenessConstraint.Count = 0 And
                                Not abReplacingRoleConstraint Then

                                '--------------------------------------------------------------------------------------------
                                'NB If replacing the RoleConstraint (see frmDiagramORM event for adding new RoleConstraint)
                                '  then we don't want to remove the Table or its Columns or Relations, especially if is an RDSRelation table

                                'Remove the Table from the RDSModel/Database                                

                                '--------------------------------------------------------------------
                                'CodeSafe: Don't progress if there is no table.
                                If lrTable Is Nothing Then Exit Sub

                                For Each lrRole In arRoleConstraint.RoleConstraintRole(0).Role.FactType.RoleGroup
                                    lrColumn = lrTable.Column.Find(Function(x) x.Role.Id = lrRole.Id)
                                    If lrColumn IsNot Nothing Then
                                        Call lrTable.removeColumn(lrColumn, Me.IsDatabaseSynchronised)
                                    End If
                                Next

                                Dim larIncomingRelations As New List(Of RDS.Relation)
                                larIncomingRelations = lrTable.getIncomingRelations()

                                For Each lrRelation In larIncomingRelations
                                    Call Me.RDS.removeRelation(lrRelation)
                                Next

                                Dim larOutgoingRelations As New List(Of RDS.Relation)
                                larOutgoingRelations = lrTable.getOutgoingRelations()

                                For Each lrRelation In larOutgoingRelations
                                    Call Me.RDS.removeRelation(lrRelation)
                                Next

                                Call Me.RDS.removeTable(lrTable)

                            ElseIf lrFactType.Arity = 2 And Not abReplacingRoleConstraint Then

                                Dim lrResponsibleRole As FBM.Role = arRoleConstraint.RoleConstraintRole(0).Role

                                Dim larColumn = From Table In Me.RDS.Table
                                                From Column In Table.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Select Column

                                For Each lrColumn In larColumn.ToList
                                    Call lrColumn.Table.removeColumn(lrColumn, Me.IsDatabaseSynchronised)
                                Next

                                'Relations: Already removed within FactType.RemoveInternalUniquenessConstraint.
                            End If
                        End If
                    ElseIf arRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.ExternalUniquenessConstraint Then

                        Dim lrTable As RDS.Table = Nothing

                        If arRoleConstraint.DoesEachRoleReferenceTheSameModelObject Then
                            If arRoleConstraint.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then
                                'All good. Can look for an Index to remove
                            Else
                                Exit Try
                            End If
                        Else
                            If arRoleConstraint.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then
                                'All good. Can look for an Index to remove
                            Else
                                Exit Try
                            End If
                        End If

                        Dim lrOtherRoleOfFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(arRoleConstraint.RoleConstraintRole(0).Role.Id)

                        If lrOtherRoleOfFactType.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                            '------------------------------------------------------------
                            'Unique identifier for an Entity Type
                            '--------------------------------------
                            Dim lrEntityType As FBM.EntityType
                            lrEntityType = lrOtherRoleOfFactType.JoinedORMObject

                            lrTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.Name)
                        End If

                        If lrTable Is Nothing Then
                            Exit Try
                        End If

                        Call lrTable.removeIndexByRoleConstraint(arRoleConstraint)

                    End If 'Is InternalUniquenessConstraint
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
        ''' Removes Roles that reference Nothing. i.e. e.g. When the User doesn't assign a ModelObject to a Role.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveRolesThatReferenceNothing(ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                Dim larRole = From Role In Me.Role
                              Where Role.JoinedORMObject Is Nothing
                              Select Role

                Dim lrFactType As FBM.FactType
                For Each lrRole In larRole.ToArray
                    If lrRole.FactType IsNot Nothing Then
                        lrFactType = Me.FactType.Find(Function(x) x.Id = lrRole.FactType.Id)
                        If lrFactType IsNot Nothing Then
                            lrFactType.RemoveRole(lrRole, True, abBroadcastInterfaceEvent)
                        End If
                    End If
                    Me.Role.Remove(lrRole)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveModelError(ByVal arModelError As FBM.ModelError)

            If arModelError IsNot Nothing Then
                Call Me.ModelError.Remove(Me.ModelError.Find(Function(x) x.ErrorId = arModelError.ErrorId And x.ModelObject.Id = arModelError.ModelObject.Id))
            End If

            RaiseEvent ModelErrorRemoved(arModelError)

        End Sub

        Public Sub RemoveModelErrorsForModelObject(ByVal arModeObject As FBM.ModelObject)

            For Each lrModelError In Me.ModelError.FindAll(Function(x) x.ModelObject.Id = arModeObject.Id)
                Call Me.RemoveModelError(lrModelError)
            Next

        End Sub

        Public Sub RemoveModelNote(ByRef arModelNote As FBM.ModelNote, ByVal abDoDatabaseProcessing As Boolean)

            If Me.ModelNote.Contains(arModelNote) Then
                Me.ModelNote.Remove(arModelNote)
            End If

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, arModelNote.Id, pcenumConceptType.ModelNote)

            If Me.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsByOtherConceptType) Then
                '----------------------------------------------------------------------------------------------
                'Don't remove the DictionaryEntry because another Concept (of ConceptType other than ModelNote)
                '  uses the Dictionary entry
                '----------------------------------------------------------------------------------------------
                lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                lrDictionaryEntry.removeConceptType(pcenumConceptType.ModelNote)
                Call TableModelDictionary.UpdateModelDictionaryEntry(lrDictionaryEntry)
            Else
                lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                If lrDictionaryEntry.Realisations.Count <= 1 Then
                    Me.RemoveDictionaryEntry(lrDictionaryEntry, abDoDatabaseProcessing)
                Else
                    Dim lrConcept As New FBM.Concept(arModelNote.Id)
                    lrDictionaryEntry.Realisations.Remove(pcenumConceptType.ModelNote)
                End If
            End If

        End Sub

        ''' <summary>
        ''' Detects ModelErrors and adds/removes them to/from the Model.ModelError list.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReviewModelErrors()

            '--------------------------------------------------------------------
            'Look for Fact ModelErrors. i.e. Fact that do not fit
            '  with the RoleConstraints over the FactType to which they belong.
            '--------------------------------------------------------------------            
            Dim lrModelErrors As New List(Of FBM.ModelError)

            Try

                Me.ModelError.Clear()

                'lrModelErrors = New List(Of FBM.ModelError)
                'If lrFact.DoesFactMeetRoleConstraintsOfFactType(lrModelErrors) Then

                'Else
                '    '--------------------------------------------------------------
                '    'Add the ModelError/s to the list of ModelErrors in the Model
                '    '--------------------------------------------------------------
                '    Me.ModelError.AddRange(lrModelErrors)
                'End If

                'For Each lrDictionaryEntry In Me.ModelDictionary

                '    Select Case lrDictionaryEntry.ConceptType
                '        Case Is = pcenumConceptType.RoleConstraint

                '            'Select CountStringInString(*)
                '            '    FROM Page,
                '            '         RoleConstraintInstance
                '            '      WHERE RoleConstraintInstance.PageId = Page.PageId
                '            '       AND RoleConstraintInstance.Id = lrDictionaryEntry.Symbol
                '            Dim liCount = From pg In Me.Page, _
                '                               rci In pg.RoleConstraintInstance _
                '                         Where rci.Id = lrDictionaryEntry.Symbol _
                '                         Select rci Distinct.Count

                '            If liCount = 0 Then
                '                lrModelError = New FBM.ModelError("333", "Role Constraint, '" & lrDictionaryEntry.Symbol & "', is not used on any Page but exists in the Model.")
                '                If Not Me.ModelError.Exists(AddressOf lrModelError.Equals) Then
                '                    Me.ModelError.Add(lrModelError)
                '                End If
                '            End If
                '    End Select
                'Next

                RaiseEvent ModelErrorsUpdated()
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: FBM.tORMModel.ReviewModelErrors: " & vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="abRapidSave"></param>        
        ''' <remarks></remarks>
        Sub Save(Optional ByVal abRapidSave As Boolean = False, Optional abModelDictionaryRapidSave As Boolean = False)

            '-------------------------------------------------------
            'Saves the currently loaded ORM model to the database
            '-------------------------------------------------------
            Dim lrEntityType As FBM.EntityType = Nothing
            Dim lrValueType As FBM.ValueType = Nothing
            Dim lrFactType As FBM.FactType = Nothing
            Dim lrFact As FBM.Concept = Nothing
            Dim lrRole As FBM.Role = Nothing
            Dim lrRoleConstraint As FBM.RoleConstraint = Nothing
            Dim lrModelNote As FBM.ModelNote = Nothing

            Try

                '------------------------------------------------------------------------------------
                'OrganicComputing:SafeCode: Remove all Roles that reference Nothing
                '  - Users may create FactTypes/Roles through the GUI and forget to assign 
                '  a ModelObject (as reference) to one or more Roles within the FactType.
                Call Me.RemoveRolesThatReferenceNothing(False)

                '----------------------------------------------
                'First, check to see if the Model itself exists
                '  in the database/Save the Model
                '----------------------------------------------
                If abRapidSave Then
                    '------------------------------------------------
                    'Create an instance of the Model in the database
                    '------------------------------------------------
                    Call TableModel.add_model(Me)
                Else
                    If TableModel.ExistsModelById(Me.ModelId) Then
                        Call TableModel.update_model(Me)
                    Else
                        '------------------------------------------------
                        'Create an instance of the Model in the database
                        '------------------------------------------------
                        Call TableModel.add_model(Me)
                    End If
                End If

                If Me.StoreAsXML Then
                    Call Me.SaveToXMLDocument()
                Else
                    Call Me.SaveToDatabase(abRapidSave, abModelDictionaryRapidSave)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SaveToDatabase(Optional ByVal abRapidSave As Boolean = False, Optional abModelDictionaryRapidSave As Boolean = False)

            Try
                '--------------------------------------------------
                'Save the DictionaryEntries within the ORM Model.
                '  NB Also maintains the MetaModelConcept table.
                '--------------------------------------------------
                Richmond.WriteToStatusBar("Saving Model Dictionary",, 0)
                Call Me.SaveModelDictionary(abModelDictionaryRapidSave)

                '----------------------------------------------
                ' Save the set of Entities within the ORM Model
                '----------------------------------------------
                Richmond.WriteToStatusBar("Saving Entity Types",, 10)
                For Each lrEntityType In Me.EntityType.OrderBy(Function(x) x.SubtypeRelationship.Count)
                    Call lrEntityType.Save(abRapidSave)
                Next

                '-------------------------------------------------
                ' Save the set of Value Types within the ORM Model
                '-------------------------------------------------
                Richmond.WriteToStatusBar("Saving Value Types",, 20)
                For Each lrValueType In Me.ValueType.FindAll(Function(x) x.isDirty)
                    Call lrValueType.Save(abRapidSave)
                Next

                '-------------------------------------------------
                ' Save the set of Fact Types within the ORM Model
                '-------------------------------------------------
                Richmond.WriteToStatusBar("Saving Fact Types",, 30)
                For Each lrFactType In Me.FactType
                    Call lrFactType.Save(abRapidSave)
                Next

                Richmond.WriteToStatusBar("Saving Role Constraints",, 40)
                For Each lrRoleConstraint In Me.RoleConstraint
                    Call lrRoleConstraint.Save(abRapidSave)
                Next

                '----------------------------
                'Save the ModelNote objects
                '----------------------------
                Richmond.WriteToStatusBar("Saving Model Notes",, 50)
                For Each lrModelNote In Me.ModelNote
                    lrModelNote.Save(abRapidSave)
                Next

                '------------------------------------------------------------------
                'NB Procedural: Keep before saving the Pages.
                Me.IsDirty = False 'So that the Pages don't save the Model again.

                Dim lrPage As FBM.Page

                Richmond.WriteToStatusBar("Saving Pages",, 60)
                For Each lrPage In Me.Page
                    If lrPage.IsDirty Then
                        lrPage.Save(abRapidSave)
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

        Public Sub SaveToXMLDocument()

            Dim lsFolderLocation As String = ""
            Dim lsFileName As String = ""
            Dim loStreamWriter As StreamWriter ' Create file by FileStream class
            Dim loXMLSerialiser As XmlSerializer ' Create binary object
            Dim lrExportModel As New XMLModel.Model

            Try
                '-----------------------------------------
                'Get the Model from the selected TreeNode
                '-----------------------------------------
                lrExportModel.ORMModel.ModelId = Me.ModelId
                lrExportModel.ORMModel.Name = Me.Name

                Call lrExportModel.MapFromFBMModel(Me)

                Dim lsFileLocationName As String = ""
                If Richmond.IsSerializable(lrExportModel) Then

                    Dim lsConnectionString As String = Trim(My.Settings.DatabaseConnectionString)

                    If My.Settings.DatabaseType = pcenumDatabaseType.MSJet.ToString Then

                        Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                        lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString

                        lsFolderLocation = Path.GetDirectoryName(lrSQLConnectionStringBuilder("Data Source")) & "\XML"
                    Else
                        lsFolderLocation = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\XML"
                    End If
                    System.IO.Directory.CreateDirectory(lsFolderLocation)
                    lsFileName = Me.ModelId & "-" & Me.Name & ".fbm"
                    lsFileLocationName = lsFolderLocation & "\" & lsFileName

                    loStreamWriter = New StreamWriter(lsFileLocationName)

                    loXMLSerialiser = New XmlSerializer(GetType(XMLModel.Model))

                    ' Serialize object to file
                    loXMLSerialiser.Serialize(loStreamWriter, lrExportModel)
                    loStreamWriter.Close()

                End If 'IsSerialisable
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub SaveModelDictionary(Optional ByVal abRapidSave As Boolean = False)

            Dim lrConcept As New FBM.Concept
            Dim lrModelDictionaryEntry As FBM.DictionaryEntry
            Dim larDictionaryEntriesToRemove As New List(Of FBM.DictionaryEntry)

            '---------------------------------------------------
            'Save the DictionaryEntries in the ModelDictionary
            '---------------------------------------------------
            For Each lrModelDictionaryEntry In Me.ModelDictionary.FindAll(Function(x) x.isDirty Or abRapidSave)
                '---------------------------------------------------------
                'CodeSafe: Only save DictionaryEntries with Realisations
                '---------------------------------------------------------
                If (lrModelDictionaryEntry.Realisations.Count >= 1) Or lrModelDictionaryEntry.isGeneralConcept Then
                    '-------------------------------------------------------------------------
                    'CodeSafe:If Value only, make sure is limited to 100 characters
                    If lrModelDictionaryEntry.Realisations.FindAll(Function(x) x <> pcenumConceptType.Value).Count = 0 Then
                        lrModelDictionaryEntry.Symbol = Strings.Left(lrModelDictionaryEntry.Symbol, 100)
                    End If

                    lrConcept = New FBM.Concept(lrModelDictionaryEntry.Symbol, True)
                    lrConcept.Save(abRapidSave)
                    Call lrModelDictionaryEntry.Save(abRapidSave)
                Else
                    '---------------------------------------------------------------------
                    'Remove unnused DictionaryEntries from the ModelDictionary/database.
                    '---------------------------------------------------------------------
                    larDictionaryEntriesToRemove.Add(lrModelDictionaryEntry)
                End If
            Next

            For Each lrModelDictionaryEntry In larDictionaryEntriesToRemove
                Me.RemoveDictionaryEntry(lrModelDictionaryEntry, True)
            Next

        End Sub

        Public Sub SetName(ByVal asNewName As String)

            Me.Name = asNewName
            Call Me.MakeDirty()
        End Sub

        ''' <summary>
        ''' Makes all Page.IsDirty flags of each of the Pages within the Model = False.
        ''' Generally called in ClientServer mode and where another instance of Boston has saved the Model.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub makeAllPagesClean()

            For Each lrPage In Me.Page
                lrPage.IsDirty = False
            Next

        End Sub

        Public Sub MakeDirty(Optional ByVal abGlobalBroadcast As Boolean = False, Optional abCheckForErrors As Boolean = True)

            Me.IsDirty = True

            If abGlobalBroadcast Then
                RaiseEvent ModelUpdated()
            Else
                '----------------------------
                'Default is GlobalBroadcast
                '----------------------------
                'RaiseEvent ModelUpdated()
            End If

            If abCheckForErrors And Me.AllowCheckForErrors Then
                Dim lrValidator = New Validation.ModelValidator(Me)
                Call lrValidator.CheckForErrors()
            End If

            RaiseEvent MadeDirty(abGlobalBroadcast)

        End Sub

        ''' <summary>
        ''' Merges Page2 into Page1.
        ''' </summary>
        ''' <param name="arOriginPage"></param>
        ''' <param name="arTargetPage"></param>
        ''' <remarks></remarks>
        Public Sub MergePages(ByRef arOriginPage As FBM.Page, ByRef arTargetPage As FBM.Page)

            '---------------------------------------------------------------------------------------------
            'PSEUDOCODE
            ' * FOR EACH EntityTypeInstance in Page2
            '   * IF EntityTypeInstance.EntityType is in Page1.Model.EntityType list
            '     * Do Nothing
            '   * ELSE
            '     * Add EntityTypeInstance.EntityType to Page1.Model.EntityType list
            '   * END IF
            '   * IF EntityTypeInstance is in Page1.EntityTypeInstance list
            '     * Do Nothing
            '   * ELSE
            '     * Add EntityTypeInstance to Page1.EntityTypeInstance list
            '   * END IF
            ' * LOOP
            '---------------------------------------------------------------------------------------------

        End Sub

        Public Function ModelObjectIsSubtypeOfModelObject(ByRef arCandidateSubtypeModelObject As FBM.ModelObject,
                                                          ByRef arCandidateSupertypeModelObject As FBM.ModelObject) As Boolean

            Try
                Dim laiValidConceptTypes() As pcenumConceptType = {pcenumConceptType.ValueType,
                                                                   pcenumConceptType.EntityType,
                                                                   pcenumConceptType.FactType}

                If Array.IndexOf(laiValidConceptTypes, arCandidateSubtypeModelObject.ConceptType) = -1 Then
                    Throw New Exception("Error: Candidate Subtype must either be a Value Type, an Entity Type or a Fact Type.")
                ElseIf Array.IndexOf(laiValidConceptTypes, arCandidateSupertypeModelObject.ConceptType) = -1 Then
                    Throw New Exception("Error: Candidate Supertype must either be a Value Type, an Entity Type or a Fact Type.")
                End If

                If arCandidateSubtypeModelObject.parentModelObjectList.Contains(arCandidateSupertypeModelObject) Then
                    Return True
                Else
                    Dim lbInterimResult As Boolean
                    Dim lrModelObject As FBM.ModelObject

                    For Each lrModelObject In arCandidateSubtypeModelObject.parentModelObjectList
                        lbInterimResult = Me.ModelObjectIsSubtypeOfModelObject(lrModelObject, arCandidateSupertypeModelObject)
                        If lbInterimResult = True Then
                            Return True
                        End If
                    Next
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
        ''' Matches a ModelElement.Id/Name even if the case of the letters is incorrect
        ''' </summary>
        ''' <param name="asModelElementName">The Id/Name of the ModelElement</param>
        ''' <param name="asActualModelElementName">The actual Id/Name of the ModelElement, with correct Case</param>
        ''' <returns>The ConceptType of the ModelElement</returns>
        ''' <remarks></remarks>
        Public Function GetConceptTypeByNameFuzzy(ByVal asModelElementName As String, ByRef asActualModelElementName As String) As pcenumConceptType

            Dim lrValueType As FBM.ValueType
            Dim lrEntityType As FBM.EntityType
            Dim lrFactType As FBM.FactType
            Dim lrRoleConstraint As FBM.RoleConstraint

            lrValueType = Me.ValueType.Find(Function(x) LCase(x.Id) = LCase(asModelElementName))
            lrEntityType = Me.EntityType.Find(Function(x) LCase(x.Id) = LCase(asModelElementName))
            lrFactType = Me.FactType.Find(Function(x) LCase(x.Id) = LCase(asModelElementName))
            lrRoleConstraint = Me.RoleConstraint.Find(Function(x) LCase(x.Id) = LCase(asModelElementName))

            If IsSomething(lrValueType) Then
                asActualModelElementName = lrValueType.Id
                Return pcenumConceptType.ValueType
            ElseIf IsSomething(lrEntityType) Then
                asActualModelElementName = lrEntityType.Id
                Return pcenumConceptType.EntityType
            ElseIf IsSomething(lrFactType) Then
                asActualModelElementName = lrFactType.Id
                Return pcenumConceptType.FactType
            ElseIf IsSomething(lrRoleConstraint) Then
                asActualModelElementName = lrRoleConstraint.Id
                Return pcenumConceptType.RoleConstraint
            ElseIf Me.ModelDictionary.Find(Function(x) lcase(x.Symbol) = lcase(asModelElementName) And x.isGeneralConcept) IsNot Nothing Then
                Return pcenumConceptType.GeneralConcept
            Else
                Return Nothing
            End If

        End Function

        Public Function GetJoinPathBetweenRoles(ByVal arFirstRole As FBM.Role,
                                                ByVal arSecondRole As FBM.Role,
                                                ByRef abSuccessfull As Boolean,
                                                ByRef aiJoinPathError As pcenumJoinPathError,
                                                ByRef aarPathCovered As List(Of FBM.Role),
                                                ByVal arRoleConstraint As FBM.RoleConstraint,
                                                ByRef aarUniqueRolesCovered As List(Of FBM.Role)) As FBM.JoinPath


            Try
                Dim lrJoinPath As New FBM.JoinPath
                Dim lrRole As FBM.Role

                lrJoinPath.RolePath.AddUnique(arFirstRole)
                abSuccessfull = False

                'Dim larFactType = From Role In aarPathCovered
                '                  Select Role.FactType Distinct

                'Dim lsFactTypeTraversal As String = ""
                'Dim liInd As Integer = 1
                'For Each lrFactType In larFactType
                '    lsFactTypeTraversal &= lrFactType.Id
                '    If liInd <> larFactType.Count Then
                '        lsFactTypeTraversal &= "->"
                '    End If
                '    liInd += 1
                'Next
                'MsgBox(lsFactTypeTraversal)

                If aarPathCovered.Count > 15 Then
                    aiJoinPathError = pcenumJoinPathError.RidiculousDepth
                    abSuccessfull = False
                    Return lrJoinPath
                ElseIf aarUniqueRolesCovered.Contains(arFirstRole) Then
                    aiJoinPathError = pcenumJoinPathError.DoubledBackToCoveredRole
                    abSuccessfull = False
                    Return lrJoinPath
                ElseIf aarPathCovered.Contains(arFirstRole) Then
                    '-----------------------------------------------------------------------
                    'Have gone around in circles on some sort of circular JoinPath. 
                    '  Must stop otherwise this recurrent procedure will continue forever.
                    '-----------------------------------------------------------------------
                    aiJoinPathError = pcenumJoinPathError.CircularPathFound
                    abSuccessfull = False
                End If

                If arRoleConstraint.ExistingArgumentsContainsMemberOfRoleList(aarPathCovered) Then
                    'Have doubled back to a Role already within an existing Argument of the RoleConstraint.
                    aiJoinPathError = pcenumJoinPathError.DoubledBackToExistingArgumentRole
                    abSuccessfull = False
                ElseIf arFirstRole.FactType.RoleGroup.FindAll(Function(x) x.Id = arSecondRole.Id).Count > 0 Then
                    '--------------------------------------------------------------------------------
                    'Have found a Path to the lrSecondRole within the same FactType as arFirstRole.
                    '--------------------------------------------------------------------------------
                    lrJoinPath.RolePath.AddUnique(arSecondRole)
                    abSuccessfull = True
                Else

                    For Each lrRole In arFirstRole.FactType.RoleGroup.FindAll(Function(x) x.Id <> arFirstRole.Id)
                        '--------------------------------------------------------------------------------------------------------
                        'Next Role in the FactType isn't the sought Role, so must scan/transverse from/past the JoinedORMObject
                        '  of the Role.
                        '--------------------------------------------------------------------------------------------------------
                        Dim lrTraversedRole As FBM.Role
                        lrJoinPath.RolePath.AddUnique(lrRole)
                        aarPathCovered.Add(lrRole)
                        Dim larSuccessfulJoinPathContinuation As New List(Of FBM.JoinPath)

                        If lrRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                            abSuccessfull = False
                        Else


                            If Not lrRole.JoinedORMObject.GetAdjoinedRoles(True) Is Nothing Then
                                If lrRole.JoinedORMObject.GetAdjoinedRoles(True).Count > 0 Then

                                    Dim larRole As New List(Of FBM.Role)
                                    larRole = lrRole.JoinedORMObject.GetAdjoinedRoles(True).FindAll(Function(x) x.Id <> lrRole.Id)

                                    For Each lrAdjoinedRole In larRole
                                        If lrAdjoinedRole.FactType.RoleGroup.FindAll(Function(x) x.Id = arSecondRole.Id).Count > 0 Then
                                            '-----------------------------------------------------------------------------------------------------------
                                            'Have found a Path to the lrSecondRole
                                            If lrAdjoinedRole.Id <> arSecondRole.Id Then
                                                lrJoinPath.RolePath.AddUnique(lrAdjoinedRole)
                                                aarUniqueRolesCovered.AddUnique(lrAdjoinedRole)
                                            End If
                                            lrJoinPath.RolePath.AddUnique(arSecondRole)
                                            aarUniqueRolesCovered.AddUnique(arSecondRole)
                                            abSuccessfull = True
                                            Return lrJoinPath
                                        End If
                                    Next

                                    For Each lrTraversedRole In lrRole.JoinedORMObject.GetAdjoinedRoles(True).FindAll(Function(x) x.Id <> lrRole.Id)

                                        If lrTraversedRole.FactType.RoleGroup.FindAll(Function(x) x.Id = arSecondRole.Id).Count > 0 Then
                                            '-------------------------------------------------------------------------------------------
                                            'Have found the JoinPath, so return.
                                            '-------------------------------------------------------------------------------------------
                                            If arSecondRole.Id <> lrTraversedRole.Id Then
                                                lrJoinPath.RolePath.AddUnique(lrTraversedRole)
                                                aarUniqueRolesCovered.AddUnique(lrTraversedRole)
                                            End If
                                            Dim lrSuccessfulJoinPath As New FBM.JoinPath
                                            lrSuccessfulJoinPath.RolePath.AddUnique(arSecondRole)
                                            larSuccessfulJoinPathContinuation.Add(lrSuccessfulJoinPath)
                                            aarUniqueRolesCovered.AddUnique(arSecondRole)
                                            Exit For
                                        End If

                                        If arRoleConstraint.ExistingArgumentsContainsMemberOfRoleList(aarPathCovered) Then
                                            'Have doubled back to a Role already within an existing Argument of the RoleConstraint.
                                            aiJoinPathError = pcenumJoinPathError.DoubledBackToExistingArgumentRole
                                            abSuccessfull = False
                                            aarUniqueRolesCovered.AddUnique(lrTraversedRole)
                                        Else
                                            Dim lrContinuingJoinPath As New FBM.JoinPath

                                            Dim larTempPathCovered As New List(Of FBM.Role)
                                            larTempPathCovered.AddRange(aarPathCovered)
                                            larTempPathCovered.Add(lrTraversedRole)

                                            lrContinuingJoinPath.AppendJoinPath(Me.GetJoinPathBetweenRoles(lrTraversedRole,
                                                                                                 arSecondRole,
                                                                                                 abSuccessfull,
                                                                                                 aiJoinPathError,
                                                                                                 larTempPathCovered,
                                                                                                 arRoleConstraint,
                                                                                                 aarUniqueRolesCovered))
                                            If abSuccessfull Then
                                                larSuccessfulJoinPathContinuation.Add(lrContinuingJoinPath)
                                                'aarPathCovered.AddRange(larTempPathCovered)
                                                'Exit For
                                            Else
                                                'aarPathCovered.Add(arFirstRole)
                                            End If
                                        End If
                                        aarUniqueRolesCovered.AddUnique(lrTraversedRole)
                                    Next 'Transvered Role

                                End If
                            End If

                            Select Case larSuccessfulJoinPathContinuation.Count
                                Case Is = 0
                                    '-------------------------------------------------------------------------------------
                                    'No transversal path was successful.
                                    '-------------------------------------------------------------------------------------
                                    abSuccessfull = False
                                Case Is = 1
                                    '-------------------------------------------------------------------------------------
                                    'Successful, non-ambiguous JoinPath found.
                                    '-------------------------------------------------------------------------------------
                                    abSuccessfull = True
                                    lrJoinPath.AppendJoinPath(larSuccessfulJoinPathContinuation(0))
                                    Return lrJoinPath
                                Case Else
                                    '-------------------------------------------------------------------------------------------
                                    'Ambiguous Join Path.
                                    '-------------------------------------------------------------------------------------------
                                    Dim liTemp = From JoinPath In larSuccessfulJoinPathContinuation
                                                 Select JoinPath.RolePath.Count
                                                 Order By Count

                                    If liTemp(0) = liTemp(1) Then

                                        aiJoinPathError = pcenumJoinPathError.AmbiguousJoinPath
                                        abSuccessfull = False
                                    Else
                                        abSuccessfull = True
                                        Dim lrMinJoinPath As JoinPath
                                        lrMinJoinPath = larSuccessfulJoinPathContinuation.Find(Function(x) x.RolePath.Count = liTemp(0))

                                        lrJoinPath.AppendJoinPath(lrMinJoinPath)
                                        Return lrJoinPath
                                    End If
                            End Select

                            lrJoinPath.RolePath.AddUnique(arSecondRole)

                        End If
                        aarUniqueRolesCovered.AddUnique(lrRole)
                    Next 'Role in the FactType


                End If

                Return lrJoinPath

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        Public Function GetModelObjectByName(ByVal asModelObjectName As String) As FBM.ModelObject

            Dim lrValueType As FBM.ValueType
            Dim lrEntityType As FBM.EntityType
            Dim lrFactType As FBM.FactType
            Dim lrRoleConstraint As FBM.RoleConstraint

            Dim lsModelObjectName = Trim(asModelObjectName)

            Try
                If Me.ExistsModelElement(lsModelObjectName) Then

                    Dim lrDictionaryEntry As FBM.DictionaryEntry
                    lrDictionaryEntry = Me.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(lsModelObjectName))

                    If lrDictionaryEntry IsNot Nothing Then
                        If lrDictionaryEntry.isValueType Then
                            Return Me.ValueType.Find(Function(x) x.Id = lsModelObjectName)
                        ElseIf lrDictionaryEntry.isEntityType Then
                            Return Me.EntityType.Find(Function(x) x.Id = lsModelObjectName)
                        ElseIf lrDictionaryEntry.isFactType Then
                            Return Me.FactType.Find(Function(x) x.Id = lsModelObjectName)
                        ElseIf lrDictionaryEntry.isRoleConstraint Then
                            Return Me.RoleConstraint.Find(Function(x) x.Id = lsModelObjectName)
                        Else
                            Dim lsMessage As String = ""
                            lsMessage = "Model Element doesn't actually exist in the Model. Only in the ModelDictionary: '" & lsModelObjectName & "'"
                            lsMessage.AppendString(vbCrLf & vbCrLf & "Boston will now remove the ModelElement from the ModelDictionary.")
                            Throw New Exception(lsMessage)
                        End If
                    Else
                        lrValueType = New FBM.ValueType(Me, pcenumLanguage.ORMModel, lsModelObjectName, True)
                        lrEntityType = New FBM.EntityType(Me, pcenumLanguage.ORMModel, lsModelObjectName, Nothing, True)
                        lrFactType = New FBM.FactType(Me, lsModelObjectName, True)
                        lrRoleConstraint = New FBM.RoleConstraint(Me, lsModelObjectName, True)

                        If Me.ValueType.Exists(AddressOf lrValueType.Equals) Then
                            Return Me.ValueType.Find(AddressOf lrValueType.Equals)
                        ElseIf Me.EntityType.Exists(AddressOf lrEntityType.Equals) Then
                            Return Me.EntityType.Find(AddressOf lrEntityType.Equals)
                        ElseIf Me.FactType.Exists(AddressOf lrFactType.Equals) Then
                            Return Me.FactType.Find(AddressOf lrFactType.Equals)
                        ElseIf Me.RoleConstraint.Exists(AddressOf lrRoleConstraint.Equals) Then
                            Return Me.RoleConstraint.Find(AddressOf lrRoleConstraint.Equals)
                        Else
                            Dim lsMessage As String = ""
                            lsMessage = "Model Element doesn't actually exist in the Model. Only in the ModelDictionary: '" & lsModelObjectName & "'"
                            lsMessage.AppendString(vbCrLf & vbCrLf & "Boston will now remove the ModelElement from the ModelDictionary.")
                            Throw New Exception(lsMessage)
                            Me.ModelDictionary.Remove(New FBM.DictionaryEntry(Me, lsModelObjectName, pcenumConceptType.Actor))
                            Me.Dictionary.Remove(lsModelObjectName)
                        End If
                    End If

                Else
                    '-------------------------------------------------
                    'The ModelObject does not exist within the Model
                    '-------------------------------------------------
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
        ''' Returns a list of all the ValueTypes, EntitTypes and FactTypes in the Model
        ''' </summary>
        ''' <returns></returns>
        Public Function getModelObjects() As List(Of FBM.ModelObject)

            Dim larModelObject As New List(Of FBM.ModelObject)

            larModelObject.AddRange(Me.ValueType.FindAll(Function(x) Not x.IsMDAModelElement))
            larModelObject.AddRange(Me.EntityType.FindAll(Function(x) Not x.IsMDAModelElement And Not x.IsObjectifyingEntityType))
            larModelObject.AddRange(Me.FactType.FindAll(Function(x) (Not x.IsMDAModelElement) And x.IsObjectified))

            larModelObject.OrderBy(Function(x) x.Id)

            Return larModelObject

        End Function

        ''' <summary>        
        '''ORMQL Mode. Gets the tokens from the Parse Tree.
        '''Walks the ParseTree and finds the tokens as per the Properties/Fields of the ao_object passed to the procedure.
        '''  i.e. Based on the type of token at the Root of the ParseTree, the software dynamically creates ao_object such that 
        '''  it contains the tokens that it wants returned.
        ''' </summary>
        ''' <param name="ao_object">is of runtime generated type DynamicCollection.Entity</param>
        ''' <param name="aoParseTreeNode">ParseNode as from TinyPG Parser</param>
        ''' <remarks></remarks>
        Public Sub GetParseTreeTokens(ByRef ao_object As Object, ByRef aoParseTreeNode As TinyPG.ParseNode)

            '-------------------------------
            'NB IMPORTANT: The TokenTypes in the ParseTree must be the same as established in aoObject (as hardcoded here in Richmond).
            'i.e. Richmond (the software) and the Parser (to the outside word) are linked.
            'If the Tokens in the Parser do not match the HardCoded tokens then Richmond will not be able to retrieve the tokens
            ' from the ParseText input from the user.
            'This isn't the falt of the user, this is a fault of using the ParserGenerator (TinyPG in Richmond's case) to set-up the Tokens.
            'i.e. The person setting up the Parser in TinyPG need be aware that 'Tokens' in TinyPG (when defining the ORMQL) need be the same
            ' as the Tokens in Richmond and as Richmond expects.
            'i.e. Establishing a Parser in TinyPG is actually a form of 'coding' (hard-coding), and where Richmond ostensibly has 2 parsers,
            '  VB.Net and TinyPG.
            '---------------------
            'Parameters
            'ao_object is of runtime generated type DynamicCollection.Entity
            '----------------------------------------------------------------------

            Dim loPropertyInfo() As System.Reflection.PropertyInfo = ao_object.GetType().GetProperties
            Dim loProperty As PropertyInfo
            Dim lr_bag As New Stack
            Dim loParseTreeNode As TinyPG.ParseNode

            Try
                '--------------------------------------
                'Retrieve the list of required Tokens
                '--------------------------------------
                For Each loProperty In loPropertyInfo
                    lr_bag.Push(loProperty.Name)
                Next

                loParseTreeNode = aoParseTreeNode

                Dim lasListOfString As New List(Of String)
                If lr_bag.Contains(aoParseTreeNode.Token.Type.ToString) Then
                    Dim lrType As Type
                    lrType = ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).GetType
                    If lrType Is lasListOfString.GetType Then
                        ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode.Token.Text)
                    ElseIf lrType Is GetType(List(Of Object)) Then
                        ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode)
                    ElseIf lrType Is GetType(Object) Then
                        ao_object.SetAttributeMember(aoParseTreeNode.Token.Type.ToString, aoParseTreeNode)
                    ElseIf lrType Is GetType(String) Then
                        ao_object.SetAttributeMember(aoParseTreeNode.Token.Type.ToString, aoParseTreeNode.Token.Text)
                    End If
                End If

                For Each loParseTreeNode In aoParseTreeNode.Nodes
                    Call GetParseTreeTokens(ao_object, loParseTreeNode)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Function GetPagesContainingModelObject(ByVal arModelObject As FBM.ModelObject) As List(Of FBM.Page)

            Dim larPage As New List(Of FBM.Page)
            Select Case arModelObject.ConceptType
                Case Is = pcenumConceptType.EntityType,
                          pcenumConceptType.ValueType,
                          pcenumConceptType.FactType

                    Dim larPageList = From Page In Me.Page
                                      From EntityTypeInstance In Page.EntityTypeInstance
                                      Where EntityTypeInstance.Id = arModelObject.Id _
                                      And EntityTypeInstance.Page.PageId = Page.PageId
                                      Select Page

                    larPage = larPageList.ToList
            End Select

            Return larPage

        End Function

        Public Function getEntityTypeByReferenceModeValueType(ByVal arModelElement As FBM.ModelObject) As FBM.EntityType

            Try

                If arModelElement.GetType <> GetType(FBM.ValueType) Then
                    Return Nothing
                Else
                    Dim larEntityType = From EntityType In Me.EntityType
                                        Where EntityType.ReferenceModeValueType IsNot Nothing
                                        Where EntityType.ReferenceModeValueType.Id = arModelElement.Id
                                        Select EntityType

                    If larEntityType.Count > 0 Then
                        Return larEntityType.First
                    Else
                        Return Nothing
                    End If
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

        Public Function GetSubtypesOfModelObject(ByRef arModelObject As FBM.ModelObject, Optional ByRef aarCheckedList As List(Of FBM.ModelObject) = Nothing) As List(Of FBM.ModelObject)

            Try
                Dim larSubtypes As New List(Of FBM.ModelObject)
                Dim lrModelObject As FBM.ModelObject
                Dim lrEntityType As FBM.EntityType
                Dim aiConceptType() As pcenumConceptType = {pcenumConceptType.EntityType, pcenumConceptType.FactType}

                GetSubtypesOfModelObject = New List(Of FBM.ModelObject)

                If Array.IndexOf(aiConceptType, arModelObject.ConceptType) = -1 Then
                    Throw New Exception("ModelObject must either be an Entity Type or a Fact Type")
                End If

                Select Case arModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        lrEntityType = arModelObject

                        For Each lrModelObject In lrEntityType.childModelObjectList
                            GetSubtypesOfModelObject.Add(lrModelObject)
                            Call Me.GetSubtypesOfModelObject(lrModelObject, GetSubtypesOfModelObject)
                        Next

                    Case Is = pcenumConceptType.FactType
                End Select

                Return larSubtypes

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function GetSupertypesOfModelObject(ByRef arModelObject As FBM.ModelObject, Optional ByRef aarCheckedList As List(Of FBM.ModelObject) = Nothing) As List(Of FBM.ModelObject)

            Try
                Dim larSupertypes As New List(Of FBM.ModelObject)
                Dim lrModelObject As FBM.ModelObject
                Dim lrEntityType As FBM.EntityType
                Dim aiConceptType() As pcenumConceptType = {pcenumConceptType.EntityType, pcenumConceptType.FactType}

                GetSupertypesOfModelObject = New List(Of FBM.ModelObject)

                If Array.IndexOf(aiConceptType, arModelObject.ConceptType) = -1 Then
                    Throw New Exception("ModelObject must either be an Entity Type or a Fact Type")
                End If

                Select Case arModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        lrEntityType = arModelObject

                        For Each lrModelObject In lrEntityType.parentModelObjectList
                            larSupertypes.Add(lrModelObject)
                            larSupertypes.AddRange(Me.GetSupertypesOfModelObject(lrModelObject, GetSupertypesOfModelObject))
                        Next

                    Case Is = pcenumConceptType.FactType
                End Select

                Return larSupertypes

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function ExistsFactTypeForModelElements(ByVal aarModelElemenets As List(Of FBM.ModelObject)) As Boolean

            Dim lrModelObject As FBM.ModelObject
            Dim lrDummyFactType As FBM.FactType
            Dim lrRole As FBM.Role

            ExistsFactTypeForModelElements = False

            lrDummyFactType = New FBM.FactType(Me, "DummyFactType", True)

            For Each lrModelObject In aarModelElemenets
                lrRole = New FBM.Role(lrDummyFactType, New FBM.ValueType(Me, pcenumLanguage.ORMModel, lrModelObject.Id, True))
                lrDummyFactType.RoleGroup.Add(lrRole)
            Next

            If Me.FactType.FindAll(Function(x) x.EqualsByModelElements(lrDummyFactType)).Count > 0 Then
                ExistsFactTypeForModelElements = True
            End If

        End Function

        ''' <summary>
        ''' Returns TRUE if there is a JoinPath for all of the Roles associated with the RoleConstraintArgument, else returns FALSE.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function ExistsJoinPathForRoles(ByRef aarRole As List(Of FBM.Role),
                                        ByRef aiJoinPathError As pcenumJoinPathError,
                                        ByVal arRoleConstraint As FBM.RoleConstraint) As Boolean

            '----------------------------------------------------------------------------------------------------------
            'First check to see whether the JoinPath is relatively straight forward and is for Roles of one FactType.
            '----------------------------------------------------------------------------------------------------------
            If aarRole.Count = 1 Then
                '------------------------------------------------------------------
                'Simplest. Of course a JoinPath exists between a Role and itself.
                '------------------------------------------------------------------
                Return True
            ElseIf Me.AreRolesWithinTheSameFactType(aarRole) Then
                '-------------------------------------------
                'Simple and straight forward, return True.
                '-------------------------------------------
                Return True
            Else
                Dim lrFirstRole As FBM.Role
                Dim lrRole As FBM.Role
                Dim lrJoinPath As New FBM.JoinPath

                lrFirstRole = aarRole(0)

                Dim lbSuccessful As Boolean = False
                Dim larJoinPathCovered As New List(Of FBM.Role)
                Dim larAllRolesCovered As New List(Of FBM.Role)
                For Each lrRole In aarRole.FindAll(Function(x) x.Id <> lrFirstRole.Id)
                    lrJoinPath.AppendJoinPath(Me.GetJoinPathBetweenRoles(lrFirstRole,
                                                                       lrRole,
                                                                       lbSuccessful,
                                                                       aiJoinPathError,
                                                                       larJoinPathCovered,
                                                                       arRoleConstraint,
                                                                       larAllRolesCovered))
                Next
                Return lbSuccessful
            End If

        End Function

        Public Function ExistsModelElement(ByVal asModelElementName As String) As Boolean

            Try
                ExistsModelElement = False

                Dim lrDictionaryEntry = Me.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Trim(asModelElementName)))

                If IsSomething(lrDictionaryEntry) Then
                    If lrDictionaryEntry.isValueType Or
                       lrDictionaryEntry.isEntityType Or
                       lrDictionaryEntry.isFactType Or
                       lrDictionaryEntry.isRoleConstraint Or
                       lrDictionaryEntry.isModelNote Then

                        ExistsModelElement = True
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return ExistsModelElement

        End Function

        Public Function existsPredicatePart(ByVal asPredicatePartText) As Boolean

            Dim larPredicatePart = From FactType In Me.FactType
                                   From FactTypeReading In FactType.FactTypeReading
                                   From PredicatePart In FactTypeReading.PredicatePart
                                   Where PredicatePart.PredicatePartText = asPredicatePartText
                                   Select PredicatePart

            Return larPredicatePart.Count > 0

        End Function

        Public Function FactIsOnAnotherPage(ByRef arPage As FBM.Page, ByRef arFact As FBM.Fact) As Boolean

            Dim lsPageId As String = arPage.PageId
            Dim lsFactId As String = arFact.Id

            Dim liReturnCount = (From pg In Me.Page,
                                     fti In pg.FactTypeInstance,
                                       f In fti.Fact
                                 Where f.Id = lsFactId _
                                 And pg.PageId <> lsPageId).Count

            If liReturnCount > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function FindEntityTypeByValueTypeId(ByVal asValueTypeId As String) As FBM.EntityType

            FindEntityTypeByValueTypeId = Nothing

            Dim lrEntityType As FBM.EntityType
            For Each lrEntityType In Me.EntityType.FindAll(Function(x) x.ReferenceModeValueType IsNot Nothing)
                If lrEntityType.ReferenceModeValueType.Id = asValueTypeId Then
                    Return lrEntityType
                End If
            Next

            Return Nothing

        End Function

        Public Function hasADirtyPage() As Boolean

            Return Me.Page.FindAll(Function(x) x.IsDirty).Count > 0

        End Function

        ''' <summary>
        ''' Gets the count of FactTypes linking two ModelElements
        ''' </summary>
        ''' <param name="arModelElement1"></param>
        ''' <param name="arModelElement2"></param>
        ''' <returns></returns>
        Public Function hasCountFactTypesBetweenModelElements(ByRef arModelElement1 As FBM.ModelObject,
                                                             ByRef arModelElement2 As FBM.ModelObject) As Integer

            Try
                Dim larModelElement As New List(Of FBM.ModelObject) From {arModelElement1, arModelElement2}

                Dim liCount = (From FactType In Me.FactType
                               Where FactType.Arity = 2
                               Where FactType.RoleGroup.All(Function(x) larModelElement.Contains(x.JoinedORMObject))
                               Select FactType).Count

                Return liCount

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return 0
            End Try

        End Function

        Public Function getFactTypeByPredicateFarSideModelElement(ByVal asPredicate As String,
                                                                  ByVal arModelElement As FBM.ModelObject,
                                                                  Optional ByVal abUseFastenshtein As Boolean = False,
                                                                  Optional ByRef aarModelElement As List(Of FBM.ModelObject) = Nothing) As FBM.FactType

            Try
                Dim larFactType As List(Of FBM.FactType)

                larFactType = (From FactType In Me.FactType
                               From FactTypeReading In FactType.FactTypeReading
                               Where FactType.Arity = 2
                               Where FactTypeReading.PredicatePart.Last.Role.JoinedORMObject.Id = arModelElement.Id
                               Where FactTypeReading.PredicatePart(0).PredicatePartText = asPredicate
                               Select FactType).ToList

                If larFactType.Count = 0 And abUseFastenshtein Then


                    larFactType = (From FactType In Me.FactType
                                   From FactTypeReading In FactType.FactTypeReading
                                   Where FactType.Arity = 2
                                   Where FactTypeReading.PredicatePart.Last.Role.JoinedORMObject.Id = arModelElement.Id
                                   Where Fastenshtein.Levenshtein.Distance(FactTypeReading.PredicatePart(0).PredicatePartText, asPredicate) < 4
                                   Select FactType).ToList

                End If

                If aarModelElement IsNot Nothing Then
                    aarModelElement.AddUnique(arModelElement)
                    'Used by FactEngine to make sure the ModelElement is in the QueryGraph
                    For Each lrFactType In larFactType.ToArray
                        If aarModelElement.Contains(lrFactType.FactTypeReading(0).RoleList(0).JoinedORMObject) And
                           aarModelElement.Contains(lrFactType.FactTypeReading(0).RoleList(1).JoinedORMObject) Then
                            'Nothing to do here.
                        Else
                            larFactType.Remove(lrFactType)
                        End If
                    Next
                End If

                If larFactType.Count = 0 Then
                    Return Nothing
                Else
                    Return larFactType.First
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

        Public Function getFactTypeByPredicateNearSideModelElement(ByVal asPredicate As String,
                                                                  ByVal arModelElement As FBM.ModelObject,
                                                                  Optional ByVal abUseFastenshtein As Boolean = False,
                                                                  Optional ByRef aarModelElement As List(Of FBM.ModelObject) = Nothing) As FBM.FactType

            Try
                Dim larFactType As List(Of FBM.FactType)

                larFactType = (From FactType In Me.FactType
                               From FactTypeReading In FactType.FactTypeReading
                               Where FactType.Arity = 2
                               Where FactTypeReading.PredicatePart.First.Role.JoinedORMObject.Id = arModelElement.Id
                               Where FactTypeReading.PredicatePart(0).PredicatePartText = asPredicate
                               Select FactType).ToList

                If larFactType.Count = 0 And abUseFastenshtein Then


                    larFactType = (From FactType In Me.FactType
                                   From FactTypeReading In FactType.FactTypeReading
                                   Where FactType.Arity = 2
                                   Where FactTypeReading.PredicatePart.First.Role.JoinedORMObject.Id = arModelElement.Id
                                   Where Fastenshtein.Levenshtein.Distance(FactTypeReading.PredicatePart(0).PredicatePartText, asPredicate) < 4
                                   Select FactType).ToList

                End If

                If aarModelElement IsNot Nothing Then
                    aarModelElement.AddUnique(arModelElement)
                    'Used by FactEngine to make sure the ModelElement is in the QueryGraph
                    For Each lrFactType In larFactType.ToArray
                        If aarModelElement.Contains(lrFactType.FactTypeReading(0).RoleList(0).JoinedORMObject) And
                           aarModelElement.Contains(lrFactType.FactTypeReading(0).RoleList(1).JoinedORMObject) Then
                            'Nothing to do here.
                        Else
                            larFactType.Remove(lrFactType)
                        End If
                    Next
                End If

                If larFactType.Count = 0 Then
                    Return Nothing
                Else
                    Return larFactType.First
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
        ''' Deprecates the Realisations for a DictionaryEntry in the ModelDictionary
        ''' </summary>
        ''' <param name="arDictionaryEntry"></param>
        ''' <remarks></remarks>
        Public Sub IncrementRealisationsForDictionaryEntry(ByRef arDictionaryEntry As FBM.DictionaryEntry)

            Dim lrDictionarEntry As New FBM.DictionaryEntry

            lrDictionarEntry = Me.ModelDictionary.Find(AddressOf arDictionaryEntry.Equals)

            lrDictionarEntry.AddRealisation(arDictionaryEntry.ConceptType)

        End Sub

        Public Function IsEmpty() As Boolean

            Dim lbEmpty As Boolean

            lbEmpty = True

            If Me.EntityType.FindAll(Function(x) x.IsMDAModelElement = False).Count > 0 Then
                lbEmpty = False
            End If

            If Me.ValueType.FindAll(Function(x) x.IsMDAModelElement = False).Count > 0 Then
                lbEmpty = False
            End If

            If Me.FactType.FindAll(Function(x) x.IsMDAModelElement = False).Count > 0 Then
                lbEmpty = False
            End If

            If Me.RoleConstraint.FindAll(Function(x) x.IsMDAModelElement = False).Count > 0 Then
                lbEmpty = False
            End If

            If Me.ModelNote.FindAll(Function(x) x.IsMDAModelElement = False).Count > 0 Then
                lbEmpty = False
            End If

            Return lbEmpty

        End Function

        Function IsObjectAnAttribute(ByVal aiConceptType As pcenumConceptType, ByVal asObjectId As String, ByRef asTableName As String) As Boolean

            Dim ind As Integer
            Dim lrRole As FBM.Role

            IsObjectAnAttribute = True

            For Each lrRole In Me.Role
                Select Case aiConceptType
                    Case Is = pcenumConceptType.EntityType 'EntityType
                        If Me.Role(ind - 1).JoinsEntityType.Id = asObjectId Then
                            If Me.Role(ind).HasInternalUniquenessConstraint() And Not (Me.Role(ind).FactType.Is1To1BinaryFactType()) Then
                                IsObjectAnAttribute = False
                            End If
                            asTableName = Me.Role(ind).FactType.GetTableName()
                        End If
                    Case Is = pcenumConceptType.ValueType  'ValueType
                        If Me.Role(ind).JoinsValueType.Id = asObjectId Then
                            If Me.Role(ind).HasInternalUniquenessConstraint() And Not (Me.Role(ind).FactType.Is1To1BinaryFactType()) Then
                                IsObjectAnAttribute = False
                            End If
                            asTableName = Me.Role(ind).FactType.GetTableName()
                        End If
                End Select
            Next

        End Function

        ''' <summary>
        ''' Loads the Model from the database.
        ''' </summary>
        ''' <param name="abLoadPages"></param>
        ''' <param name="abUseThreading"></param>
        ''' <param name="aoBackgroundWorker">Used for Prgress reporting.</param>
        ''' <remarks></remarks>
        Public Sub Load(Optional ByVal abLoadPages As Boolean = False,
                        Optional ByVal abUseThreading As Boolean = True,
                        Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            'CodeSafe
            If Me.Loading Or Me.Loaded Then Exit Sub

            Me.Loading = True

            If Me.StoreAsXML Then
                Call Me.LoadFromXML(aoBackgroundWorker)
            Else
                Call Me.LoadFromDatabase(abLoadPages, abUseThreading, aoBackgroundWorker)
            End If

            Me.Loaded = True

            '20180410-VM-ToDo-Test to see if the RDF has been created for the Model.
            Me.RDSCreated = True 'For now for testing. 

            Richmond.WriteToStatusBar(".")

            Me.Loading = False


        End Sub

        Public Sub LoadFromDatabase(Optional ByVal abLoadPages As Boolean = False,
                                    Optional ByVal abUseThreading As Boolean = True,
                                    Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Try
                '-------------------------------------------------------
                'Loads an ORM model from the database
                '-------------------------------------------------------
                Dim liInd As Integer = 0

                Richmond.WriteToStatusBar("Loading the Model level Model Elements.", True)

                '-------------------------------------------------------
                'Load the ModelDictionary (Concepts for the Model)
                '-------------------------------------------------------
                If TableModelDictionary.GetModelDictionaryCountByModel(Me) > 0 Then
                    Call TableModelDictionary.GetDictionaryEntriesByModel(Me)
                End If

                '------------------------------------
                'Get ValueTypes
                '------------------------------------
                'Richmond.WriteToStatusBar("Loading the Value Types")            
                If TableValueType.GetValueTypeCountByModel(Me.ModelId) > 0 Then
                    '-----------------------------------------------
                    'There are EntityTypes within the ORMDiagram
                    '-----------------------------------------------
                    '--------------------------------------------------
                    'Get the list of EntityTypes within the ORMDiagram
                    '--------------------------------------------------            
                    Me.ValueType = TableValueType.GetValueTypesByModel(Me)
                End If

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(10)

                '------------------------------------
                'Get EntityTypes
                '------------------------------------
                'Richmond.WriteToStatusBar("Loading the Entity Types")
                prApplication.ThrowErrorMessage("Loading EntityTypes", pcenumErrorType.Information)
                If TableEntityType.GetEntityTypeCountByModel(Me.ModelId) > 0 Then
                    '-----------------------------------------------
                    'There are EntityTypes within the ORMDiagram
                    '-----------------------------------------------
                    '--------------------------------------------------
                    'Get the list of EntityTypes within the ORMDiagram
                    '--------------------------------------------------            
                    Me.EntityType = TableEntityType.getEntityTypesByModel(Me)
                End If

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(20)

                '------------------------------------
                'Get FactTypes
                '------------------------------------
                Richmond.WriteToStatusBar("Loading the Fact Types")
                TableFactType.GetFactTypesByModel(Me, True)

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(30)

                Call TableSubtypeRelationship.GetSubtypeRelationshipsByModel(Me)

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(40)

                '---------------------------------------------------
                'Get RoleConstraints 
                '---------------------------------------------------
                'Richmond.WriteToStatusBar("Loading the Role Constraints")
                prApplication.ThrowErrorMessage("Loading RoleConstraints", pcenumErrorType.Information)
                If TableRoleConstraint.getRoleConstraintCountByModel(Me) > 0 Then
                    '-----------------------------------------------
                    'There are RoleConstraints within the ORMModel
                    '-----------------------------------------------
                    Me.RoleConstraint = TableRoleConstraint.GetRoleConstraintsByModel(Me)
                End If

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(45)

                '-----------------------------------------------------------------------------
                'Set the ReferenceMode ObjectTypes for each of the EntityTypes in the Model
                '-----------------------------------------------------------------------------
                Dim lrEntityType As FBM.EntityType
                For Each lrEntityType In Me.EntityType.FindAll(Function(x) x.PreferredIdentifierRCId <> "")
                    Call lrEntityType.SetReferenceModeObjects()
                Next

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(50)

                '-----------------------------------
                'Load the ModelNotes for the Model
                '-----------------------------------
                If TableModelNote.getModelNoteCountByModel(Me) > 0 Then
                    '-----------------------------------------------
                    'There are RoleConstraints within the ORMModel
                    '-----------------------------------------------
                    Me.ModelNote = TableModelNote.getModelNotesByModel(Me)
                End If

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(55)

                '------------------------------------
                'Load the Pages for the Model
                '------------------------------------
                Richmond.WriteToStatusBar("Loading the Pages", True)
                prApplication.ThrowErrorMessage("Loading Pages", pcenumErrorType.Information)

                If abLoadPages Then
                    If abUseThreading And TablePage.GetPageCountByModel(Me.ModelId) < 30 Then
                        Call TablePage.GetPagesByModel(Me, True, False, aoBackgroundWorker, False)
                    Else
                        Call TablePage.GetPagesByModel(Me, True, False, aoBackgroundWorker, False)
                    End If
                Else
                    Call TablePage.GetPagesByModel(Me, False)
                End If

                '--------------------------------------
                'Flush unused ModelDictionary entries
                '--------------------------------------
                For Each lrDictionaryEntry In Me.ModelDictionary.FindAll(Function(x) (x.Realisations.Count = 0) And Not x.isGeneralConcept)

                    'Dim larDictionaryEntry = From Entry In Me.ModelDictionary
                    '                         Where Entry.Symbol = lrDictionaryEntry.Symbol
                    '                         Select Entry

                    Me.RemoveDictionaryEntry(lrDictionaryEntry, True)
                Next

                'Dim liModelPageCount As Integer = 0

                'liModelPageCount = TablePage.GetPageCountByModel(Me.ModelId, False)

                '-----------------------------------------
                'Wait for MultiThreaded loading of Pages
                '-----------------------------------------
                'While abLoadPages And Me.HasPagesUnloaded
                '-------------------------------------
                'Loop until all the Pages are loaded
                '-------------------------------------
                'End While

                'Me.AllowCheckForErrors = True
                'Dim lrValidator As New Validation.ModelValidator(Me)
                'Call lrValidator.CheckForErrors()


                If Not {"English", "Core"}.Contains(Me.ModelId) Then 'No need to modify the English or Core models
                    If Not Me.HasCoreModel Then

                        Call Me.AddCoreERDPGSAndSTDModelElements(aoBackgroundWorker)

                    ElseIf Me.CoreVersionNumber = "1.0" Then
                        '---------------------------------------------------------------------------------------
                        'Only contains ERD and PGS Model Elements. Need to add State Transition Model Elements
                        Me.ContainsLanguage.AddUnique(pcenumLanguage.EntityRelationshipDiagram)
                        Me.ContainsLanguage.AddUnique(pcenumLanguage.PropertyGraphSchema)

                        Call Me.AddCoreSTDModelElements()
                        Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)

                        Me.CoreVersionNumber = "2.0"

                        Call Me.Save()

                    ElseIf Me.CoreVersionNumber = "" Then 'Makes up for anomaly where CoreVersionNumber wasn't added when creating a new model in earlier versions.
                        'The CMML metamodels should be in all customer's models.
                        Me.CoreVersionNumber = "2.0"
                        Me.ContainsLanguage.AddUnique(pcenumLanguage.EntityRelationshipDiagram)
                        Me.ContainsLanguage.AddUnique(pcenumLanguage.PropertyGraphSchema)


                    ElseIf CDbl(Me.CoreVersionNumber) >= 2.0 Then
                        'Nothing to do (at this point), because is the latest version of the Core @ 04/11/2021. See also performCoreManagement (below).
                        Me.ContainsLanguage.AddUnique(pcenumLanguage.EntityRelationshipDiagram)
                        Me.ContainsLanguage.AddUnique(pcenumLanguage.PropertyGraphSchema)
                        Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)
                    End If

                    '============================================================================================
                    'Core Management. E.g. If the Core needs modification for a new release.
                    Call Me.performCoreManagement()

                    '==============================================
                    'Populate the RDS,STM data structure.
                    'Dim loRDSThread As System.Threading.Thread
                    'loRDSThread = New System.Threading.Thread(AddressOf Me.PopulateRDSStructureFromCoreMDAElements)
                    'loRDSThread.Start()
                    Me.PopulateRDSStructureFromCoreMDAElements()

                    '20200113-VM-Can't have more han one thread on the ORMQL parser, call from within PopulateRDSStructureFromCoreMDAElements
                    'Dim loSTMThread As System.Threading.Thread
                    'loSTMThread = New System.Threading.Thread(AddressOf Me.PopulateSTMStructureFromCoreMDAElements)
                    'loSTMThread.Start()

                    'Call Me.PopulateRDSStructureFromCoreMDAElements()

                End If

                Me.AllowCheckForErrors = True
                Call Me.checkIfCanCheckForErrors()
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub LoadFromXML(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Dim lsFolderLocation As String
            Dim lsFileName As String
            Dim lsFileLocationName As String

            Try
                If My.Settings.DatabaseType = pcenumDatabaseType.MSJet.ToString Then

                    Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                    lrSQLConnectionStringBuilder.ConnectionString = My.Settings.DatabaseConnectionString

                    lsFolderLocation = Path.GetDirectoryName(lrSQLConnectionStringBuilder("Data Source")) & "\XML"
                Else
                    lsFolderLocation = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\XML"
                End If

                lsFileName = Me.ModelId & "-" & Me.Name & ".fbm"
                lsFileLocationName = lsFolderLocation & "\" & lsFileName

                '==================================================================================================
                Dim xml As XDocument = Nothing
                Dim lsXSDVersionNr As String = ""

                'Deserialize text file to a new object.
                Dim objStreamReader As New StreamReader(lsFileLocationName)

                xml = XDocument.Load(lsFileLocationName)

                Richmond.WriteToStatusBar("Loading model.", True)
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(50)

                lsXSDVersionNr = xml.<Model>.@XSDVersionNr
                '=====================================================================================================
                Dim lrSerializer As XmlSerializer = Nothing
                Select Case lsXSDVersionNr
                    Case Is = "0.81"
                        lrSerializer = New XmlSerializer(GetType(XMLModelv081.Model))
                        Dim lrXMLModel As New XMLModelv081.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrXMLModel.MapToFBMModel(Me)
                    Case Is = "1"
                        lrSerializer = New XmlSerializer(GetType(XMLModel1.Model))
                        Dim lrXMLModel As New XMLModel1.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrXMLModel.MapToFBMModel(Me)
                    Case Is = "1.1"
                        lrSerializer = New XmlSerializer(GetType(XMLModel11.Model))
                        Dim lrXMLModel As New XMLModel11.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrXMLModel.MapToFBMModel(Me)
                    Case Is = "1.2"
                        lrSerializer = New XmlSerializer(GetType(XMLModel12.Model))
                        Dim lrXMLModel As New XMLModel12.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrXMLModel.MapToFBMModel(Me)
                    Case Is = "1.3"
                        lrSerializer = New XmlSerializer(GetType(XMLModel13.Model))
                        Dim lrXMLModel As New XMLModel13.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()

                        lrXMLModel.MapToFBMModel(Me)
                    Case Is = "1.4"
                        lrSerializer = New XmlSerializer(GetType(XMLModel14.Model))
                        Dim lrXMLModel As New XMLModel14.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrXMLModel.MapToFBMModel(Me)
                    Case Is = "1.5"
                        lrSerializer = New XmlSerializer(GetType(XMLModel15.Model))
                        Dim lrXMLModel As New XMLModel15.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrXMLModel.MapToFBMModel(Me)
                    Case Is = "1.6"
                        lrSerializer = New XmlSerializer(GetType(XMLModel16.Model))
                        Dim lrXMLModel As New XMLModel16.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrXMLModel.MapToFBMModel(Me)
                    Case Is = "1.7"
                        lrSerializer = New XmlSerializer(GetType(XMLModel.Model))
                        Dim lrXMLModel As New XMLModel.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrXMLModel.MapToFBMModel(Me)
                End Select

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(70)

                '================================================================================================================
                'RDS
                If (Me.ModelId <> "Core") And Me.HasCoreModel Then
                    Call Me.performCoreManagement()
                    Call Me.PopulateRDSStructureFromCoreMDAElements()
                    Me.RDSCreated = True
                ElseIf (Me.ModelId <> "Core") Then
                    '==================================================
                    'RDS - Create a CMML Page and then dispose of it.
                    Dim lrPage As FBM.Page '(lrModel)
                    Dim lrCorePage As FBM.Page

                    lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                    If lrCorePage Is Nothing Then
                        Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                    End If

                    lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page Model Elements for the EntityRelationshipDiagram into the metamodel

                    'StateTransitionDiagrams
                    lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                    If lrCorePage Is Nothing Then
                        Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                    End If

                    lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page Model Elements for the StateTransitionDiagram into the metamodel

                    'Derivations
                    lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)

                    If lrCorePage Is Nothing Then
                        Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
                    End If

                    lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page Model Elements for the CoreDerivations into the metamodel
                    '==================================================

                    Call Me.createEntityRelationshipArtifacts()
                    Call Me.PopulateRDSStructureFromCoreMDAElements()
                    Me.RDSCreated = True
                End If
                '==================================================================================================
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(100)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub LoadPagesFromXML()

            Dim lsFolderLocation As String
            Dim lsFileName As String
            Dim lsFileLocationName As String

            Try
                If My.Settings.DatabaseType = pcenumDatabaseType.MSJet.ToString Then
                    Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                    lrSQLConnectionStringBuilder.ConnectionString = My.Settings.DatabaseConnectionString

                    lsFolderLocation = Path.GetDirectoryName(lrSQLConnectionStringBuilder("Data Source")) & "\XML"
                Else
                    lsFolderLocation = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\XML"
                End If


                lsFileName = Me.ModelId & "-" & Me.Name & ".fbm"
                lsFileLocationName = lsFolderLocation & "\" & lsFileName

                '==================================================================================================
                Dim xml As XDocument = Nothing
                Dim lsXSDVersionNr As String = ""

                'Deserialize text file to a new object.
                Dim objStreamReader As New StreamReader(lsFileLocationName)

                xml = XDocument.Load(lsFileLocationName)

                For Each loPage In xml.<Model>.<ORMDiagram>.<Page>
                    Dim lrPage As New FBM.Page(Me,
                                               loPage.Attribute("Id").Value,
                                               loPage.Attribute("Name").Value,
                                               Richmond.GetEnumFromDescriptionAttribute(Of pcenumLanguage)(loPage.Attribute("Language").Value)
                                               )
                    Me.Page.Add(lrPage)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub performCoreManagement()

            Dim lsSQLQuery As String

            If Me.CoreVersionNumber = "" Then

                'Entity Relationship Diagrams / Property Graph Schema
                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                End If

                Dim lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page Model Elements for the EntityRelationshipDiagram into the metamodel

                'StateTransitions
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.
                Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)

                'CoreDerivations
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.                

                Me.CoreVersionNumber = "2.3"
                Me.MakeDirty(False, False)
                Call Me.Save()

            ElseIf Me.CoreVersionNumber = "2.0" Then
                'NB Tightly coupled to the v5.5 release of Boston.
                'NB Must upgrade to v2.1 Core, at least nominally, because new model elements are created/copied when the user creates a STD Page.
                '  No need to create/modify the model elements here...just need to remove the old ones.
                'STM (State Transition Model) totally changed.

                If Me.GetModelObjectByName("CoreValueTypeHasFinishCoreElementState") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreValueTypeHasFinishCoreElementState"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If Me.GetModelObjectByName("CoreValueTypeHasStartCoreElementState") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreValueTypeHasStartCoreElementState"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If Me.GetModelObjectByName("CoreValueTypeIsSubtypeStateControlled") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreValueTypeIsSubtypeStateControlled"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If Me.GetModelObjectByName("CoreStateTransitionIsForValueType") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreStateTransitionIsForValueType"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If Me.GetModelObjectByName("CoreStateTransition") IsNot Nothing Then
                    lsSQLQuery = "REMOVE MODELELEMENT CoreStateTransition"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                'Upgrade to CoreVersionNumber, v2.1
                Me.CoreVersionNumber = "2.1"

                Call TableModel.update_model(Me)

                '==================================================
                'CMML. STM - (State Transition Model). Create a CMML Page and then dispose of it.                
                'For StateTransitionDiagrams
                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                End If

                Dim lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.
                Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)

            ElseIf Me.CoreVersionNumber = "2.1" Then
                'CodeSafe
                If Me.GetModelObjectByName(pcenumCMMLRelations.CoreValueTypeHasState.ToString) Is Nothing Then
                    Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                    If lrCorePage Is Nothing Then
                        Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                    End If

                    Dim lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.
                    Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)
                End If
            End If

            If Me.CoreVersionNumber = "2.1" Then
                Dim lrFactType As FBM.FactType = Me.GetModelObjectByName("CoreERDAttribute")
                If lrFactType.InternalUniquenessConstraint(0).Role.Count = 1 Then

                    Dim lsSQLCommand = "EXTEND ROLECONSTRAINT CoreInternalUniquenessConstraint16 WITH ROLE JOINING CoreEntity IN FACTTYPE CoreERDAttribute"
                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLCommand)
                End If
                Me.CoreVersionNumber = "2.2"
                Call Me.Save()
            End If

            If Me.CoreVersionNumber = "2.2" Then

                Dim lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
                End If

                Dim lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Element for the State Transition Diagrams into the core metamodel.                

                Me.CoreVersionNumber = "2.3"
                Me.MakeDirty(False, False)
                Call Me.Save()
            End If

        End Sub

        Public Sub AddCoreERDPGSAndSTDModelElements(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Dim lfrmFlashCard As New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = 2600
            lfrmFlashCard.BackColor = Color.LightGray
            Dim lsMessage As String = ""
            lsMessage = "Adding core data structure."
            lfrmFlashCard.zsText = lsMessage
            Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(frmMain)

            '==================================================
            'RDS - Create a CMML Page and then dispose of it.
            Dim lrPage As FBM.Page
            Dim lrCorePage As FBM.Page

            'Now for ERDs/PGSs which have the same basic metamodel
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Element for the EntityRelationshipDiagram into the core metamodel.

            'Now for CoreDerivations
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Elements into the core metamodel.

            'Now for StateTransitionDiagrams
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)
            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
            End If
            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Elements into the core metamodel.
            '==================================================

            '----------------------------------------------------------------------------------------
            'Populate the Facts/FactData within the ERD/PGS Model Element metamodel Model Elements.
            Call Me.createEntityRelationshipArtifacts(aoBackgroundWorker)

            Me.ContainsLanguage.AddUnique(pcenumLanguage.EntityRelationshipDiagram)
            Me.ContainsLanguage.AddUnique(pcenumLanguage.PropertyGraphSchema)
            Me.ContainsLanguage.AddUnique(pcenumLanguage.StateTransitionDiagram)

            '------------------------------------------------------------------
            'Set the CoreVersionNumber
            Me.CoreVersionNumber = prApplication.CMML.Core.CoreVersionNumber
            Call TableModel.update_model(Me)


            'lfrmFlashCard = New frmFlashCard
            'lfrmFlashCard.ziIntervalMilliseconds = 3000
            'lfrmFlashCard.BackColor = Color.LightGray
            'lfrmFlashCard.zsText = "Your model is ready for Entity Relationship Diagrams and Property Graph Schemas."
            'liDialogResult = lfrmFlashCard.ShowDialog(frmMain)

        End Sub

        ''' <summary>
        ''' Injects v2.0 of the Core into the Model.
        ''' IMPORTANT: Assumes that V1.0  (ERD/PGS) model elements already in the Model.
        ''' </summary>
        ''' <param name="aoBackgroundWorker"></param>
        Public Sub AddCoreSTDModelElements(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Dim lfrmFlashCard As New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = 5600
            lfrmFlashCard.BackColor = Color.LightGray
            Dim lsMessage As String = ""
            lsMessage = "Creating the Relational Data Structure for State Transition Diagrams."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "This is a one-off process and may take a minute."
            lfrmFlashCard.zsText = lsMessage
            Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(frmMain)

            '==================================================
            'RDS - Create a CMML Page and then dispose of it.
            Dim lrPage As FBM.Page
            Dim lrCorePage As FBM.Page

            'Now for StateTransitionDiagrams
            lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

            If lrCorePage Is Nothing Then
                Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
            End If

            lrPage = lrCorePage.Clone(Me, False, True, False) 'Clone the Page's Model Element for the EntityRelationshipDiagram into the core metamodel.
            '==================================================

            Call Me.createEntityRelationshipArtifacts(aoBackgroundWorker)

            lfrmFlashCard = New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = 3000
            lfrmFlashCard.BackColor = Color.LightGray
            lfrmFlashCard.zsText = "Your model is ready for State Transition Diagrams."
            liDialogResult = lfrmFlashCard.ShowDialog(frmMain)

        End Sub

        Public Sub LoadPages()

            '-------------------------------------------------------------------------------------------------------
            'Load the Pages for the Model, but do not load the Pages (i.e. Do not get the Page data for the Page).
            '  This function is used to rapidly load Pages during the establishment of the EnterpriseTree within 
            '  the Enterprise Tree Viewer form.
            '-------------------------------------------------------------------------------------------------------
            If Me.StoreAsXML Then
                Call Me.LoadPagesFromXML()
            Else
                If TablePage.GetPageCountByModel(Me.ModelId) > 0 Then
                    Call TablePage.GetPagesByModel(Me, False)
                End If
            End If


        End Sub


        Public Sub UpdateDictionarySymbol(ByVal asOldSymbol As String, ByVal asNewSymbol As String, ByVal aiConceptType As pcenumConceptType)

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me, asNewSymbol, aiConceptType)

            Try
                '-----------------------------------
                'Modify the Symbol in the database
                '-----------------------------------
                Dim lrConcept As New FBM.Concept(asNewSymbol)

                '----------------------------------------------------------------------------------
                'Check to see whether the new Concept exists in the Concept table in the database
                '----------------------------------------------------------------------------------
                If TableConcept.ExistsConcept(lrConcept) Then
                    '----------------------------------------------------------
                    'No action required, the Concept already exists/is known.
                    '----------------------------------------------------------
                Else
                    TableConcept.AddConcept(lrConcept)
                End If

                '-----------------------------------------------------------------------------------------------------------
                'Check to see whether the new DictionaryEntry already exists in the ModelDictionary table in the database.
                '  NB This should already be done before calling this function.
                '-----------------------------------------------------------------------------------------------------------
                'If TableModelDictionary.ExistsModelDictionaryEntry(lrDictionaryEntry) Then
                '    Throw New System.Exception("Trying to create a new Dictionary Entry in the Model Dictionary for a Dictionary Entry that already exists.")
                'Else
                Call TableModelDictionary.ModifySymbol(Me, lrDictionaryEntry, asNewSymbol, aiConceptType)
                'End If

                '-------------------------------------------------------------------
                'Find and modify the Symbol within the (in-memory) ModelDictionary
                '-------------------------------------------------------------------
                lrDictionaryEntry.Symbol = asOldSymbol
                lrDictionaryEntry = Me.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

                If IsSomething(lrDictionaryEntry) Then
                    lrDictionaryEntry.Symbol = asNewSymbol
                Else
                    '----------------------------------------------
                    'Concepts can be updated/modified directly,
                    '  so move on (don't throw an error)
                    '----------------------------------------------
                End If
            Catch ex As Exception

                Dim lsMessage As String
                lsMessage = "Error: tORMModel.UpdateDictionarySymbol"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Original Symbol: '" & asOldSymbol & "'"
                lsMessage &= vbCrLf & "New Symbol: '" & asNewSymbol & "'"
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            End Try

        End Sub

        Private Sub tORMModel_ModelErrorsUpdated() Handles Me.ModelErrorsUpdated

        End Sub

        Private Sub ModelUpdatedHandler() Handles Me.ModelUpdated
            'Call Me.ReviewModelErrors()
        End Sub

        Public Sub TriggerFinishedErrorChecking()
            RaiseEvent FinishedErrorChecking()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

    End Class

End Namespace