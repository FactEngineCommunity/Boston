Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection
Imports Newtonsoft.Json

Namespace FBM
    <Serializable()> _
    Public Class FactDataInstance
        Inherits FBM.FactData
        Implements IEquatable(Of FBM.FactDataInstance)
        Implements iPageObject

        '--------------------------------------------------------------------------
        'Every cell in a FactTable (SamplePopulationSet)as seen
        '  on an ORM type page, is linked back to the FactData
        '  that the data within that cell represents.
        '  This allows those cells to be immediately updated if
        '  any Value (of an Entity) is changed.
        '  e.g. If a 'Process' EntityType has an Entity called 'Receive Goods'
        '  and the name of that Entity is changed in a UseCaseDiagram,
        '  then the (same) Value as it appears in the FactTable in the 
        '  FactType within the CoreActorToProcessRelation model must change too.
        '--------------------------------------------------------------------------
        <XmlIgnore()>
        Public Shadows Role As FBM.RoleInstance

        ''' <summary>
        ''' References the tFactData instance within the Model layer of the MVC pattern.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        <NonSerialized()>
        Public WithEvents FactData As New FBM.FactData

        ''' <summary>
        ''' The ObjectType joined by the Role to which this FactDataInstance relates.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        Public WithEvents JoinedObjectType As FBM.ModelObject

        <XmlIgnore()>
        <NonSerialized()>
        Public Shadows Fact As FBM.FactInstance

        <XmlIgnore()>
        <NonSerialized()>
        Public FactDataInstance As FBM.FactDataInstance 'Used to refer to original object when cloned, and for convenience.
        'e.g. If working with a tActor or CMML.Process object (as a clone of a tFactDataInstance object), 'FactDataInstance'
        '  refers back to the FactDataInstance from where the clone was made. This is so that any changes made to the tActor/CMML.Process object
        '  can be reflected back against the original tFactDataInstance object. The reason that you would do this is because
        '  tActor objects (for instance) refer to tFactDataInstance objects within the set of FactInstances, in a FactTypeInstance, in a Page.
        '  If the user 'moves' a tActor object on the Page, then the x,y coordinates need to be updated within the FactDataInstance so that the
        '  ConceptInstance can be updated when the Page is saved. Otherwise, x,y coordinates cannot be saved back to the database.
        '  - NB The option to always work with FactDataInstance objects would have negated the need to use this pattern, however it is more
        '  convenient from a programming perspective to work with tActor and CMML.Process objects than with a genereric FactDataInstance object,
        '  which would be confusing over time.
        '  - See CloneActor, CloneEntity, CloneClass etc below.
        '----------------------------------------------------------------------------------------------------------------------------------------

        <XmlIgnore()>
        Public Page As FBM.Page

        <JsonIgnore()>
        <NonSerialized(),
        XmlIgnore()>
        Public WithEvents _Shape As ShapeNode

        <JsonIgnore()>
        <XmlIgnore()>
        Public Property Shape As ShapeNode Implements iPageObject.Shape
            Get
                Return Me._Shape
            End Get
            Set(value As ShapeNode)
                Me._Shape = value
            End Set
        End Property

        <NonSerialized(),
        XmlIgnore()>
        Public TableShape As TableNode
        '20220405-VM-Remove if not missed.
        '<XmlIgnore()> _
        'Public Overridable Property TableShape() As TableNode 'Used in FactTypeInstances, ERDiagrams
        '    Get
        '        Return Me._TableShape
        '    End Get
        '    Set(ByVal value As TableNode)
        '        Me._TableShape = value
        '    End Set
        'End Property

        ''' <summary>
        ''' Refers to the Cell within a Mindfusion TableNode and is only set when the FactDataInstance is displayed on a Diagram.
        ''' </summary>
        ''' <remarks></remarks>
        <NonSerialized()>
        <XmlIgnore()>
        Public Cell As MindFusion.Diagramming.TableNode.Cell 'Used in FactTypeInstances

        ''' <summary>
        ''' The Data stored for the Role for the Fact.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Property Data() As String
            Get
                Return Me.Concept.Symbol
            End Get
            Set(ByVal value As String)
                '--------------------------------------------------------
                'See if the NewSymbol is already in the ModelDictionary
                '--------------------------------------------------------
                Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.Model, value, pcenumConceptType.Value)
                Dim lsDebugMessage As String = ""

                lrNewDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals)

                If IsSomething(lrNewDictionaryEntry) Then
                    '----------------------------------------------------------------------------
                    'The NewConcept exists in the ModelDictionary
                    '  Substitute the existing Concept for a ModelDictionary entry (Concept) that
                    '  already exists in the Model
                    '----------------------------------------------------------------------------
                    Me.Concept = lrNewDictionaryEntry.Concept
                    Me.Symbol = lrNewDictionaryEntry.Symbol
                Else
                    '-------------------------------------------------------
                    'The NewConcept does not exist in the ModelDictionary.
                    '  Modify the existing Concept
                    '-------------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, value, pcenumConceptType.Value, , , True, True)
                    Call Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                    Me.Concept = lrDictionaryEntry.Concept
                    Me.Symbol = value

                    If Me.Model.Loaded And Me.Page.Loaded Then Call Me.makeDirty()

                    'lsDebugMessage = "Setting FactData.Concept.Symbol to new Concep/DictionaryEntry: " & value
                    'Call prApplication.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information)
                End If
            End Set
        End Property


        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public Shadows _X As Integer
        Public Overridable Property X() As Integer
            Get
                Return Me._X
            End Get
            Set(ByVal value As Integer)
                Me._X = value
                If IsSomething(Me.FactDataInstance) Then
                    Me.FactDataInstance._X = value
                End If
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _Y As Integer
        Public Overridable Property Y() As Integer
            Get
                Return Me._Y
            End Get
            Set(ByVal value As Integer)
                Me._Y = value
                If IsSomething(Me.FactDataInstance) Then
                    Me.FactDataInstance._Y = value
                End If
            End Set
        End Property

        Private Property iPageObject_X As Integer Implements iPageObject.X
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Private Property iPageObject_Y As Integer Implements iPageObject.Y
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Sub New()
            Me.ConceptType = pcenumConceptType.RoleData
        End Sub

        Public Sub New(ByRef arPage As FBM.Page)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
            Me.ConceptType = pcenumConceptType.RoleData
            Me.Model = arPage.Model
            Me.Page = arPage

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arFactInstance As FBM.FactInstance, ByRef arRoleInstance As FBM.RoleInstance, ByRef arConcept As FBM.Concept)
            '-----------------------------------------------------------------------------
            'NB Arguments are by Ref, because need to point to actual objects on a Page.
            '-----------------------------------------------------------------------------
            Dim lsMessage As String = ""
            Dim lsFactId As String
            Try
                Me.ConceptType = pcenumConceptType.RoleData
                Me.Model = arPage.Model
                Me.Page = arPage
                Me.FactType = arFactInstance.FactType.FactType
                Me.Fact = arFactInstance
                Me.Role = arRoleInstance
                Me.Concept = arConcept
                Me.Symbol = Me.Concept.Symbol

                '---------------------------------------------------------------------------------
                'Link the FactData back to the Model level 
                ' by seting Me.FactData (at instance level) to the Role.Data at the Model level.
                '---------------------------------------------------------------------------------
                Dim lrRole As FBM.Role = arRoleInstance.Role
                'Dim lrFactData As New FBM.FactData(lrRole, arConcept, arFactInstance.Fact)
                'lrFactData = lrRole.Data.Find(AddressOf lrFactData.Equals)
                'Me.FactData = lrFactData
                lsFactId = arFactInstance.Id
                Me.FactData = lrRole.Data.Find(Function(x) x.Role.Id = lrRole.Id And x.Fact.Id = lsFactId)
                If Me.FactData Is Nothing Then
                    lsMessage = "Error: Cannot find Role.Data (at Model level) to link FactDataInstance.FactData to: "
                    lsMessage &= vbCrLf & vbCrLf & "Expecting to find:"
                    lsMessage &= vbCrLf & "Role.Id: " & lrRole.Id
                    lsMessage &= vbCrLf & "Role.Name: " & lrRole.Name
                    If IsSomething(arFactInstance.Fact) Then
                        lsMessage &= vbCrLf & "Fact.Id: " & Viev.NullVal(arFactInstance.Fact.Id, "")
                    Else
                        lsMessage &= vbCrLf & "Fact.Id: NULL"
                    End If
                    lsMessage &= vbCrLf & "On FactType.Id: " & lrRole.FactType.Id
                    lsMessage &= vbCrLf & "arRoleInstance.Role.Data.Count: " & lrRole.Data.Count.ToString
                    lsMessage &= vbCrLf & "FactInstance.Enumerated = " & arFactInstance.EnumerateAsBracketedFact
                    lsMessage &= vbCrLf & "Role.Data expanded:"
                    '------------------------
                    'Expand the lrRole.Data
                    '------------------------
                    Dim liInd As Integer = 1
                    For Each lrFactData In lrRole.Data
                        lsMessage &= vbCrLf & "FactData" & liInd
                        lsMessage &= vbCrLf & "Role.Id: " & lrFactData.Role.Id
                        lsMessage &= vbCrLf & "Fact.Id: " & lrFactData.Fact.Id
                        liInd += 1
                    Next
                    Throw New ApplicationException(lsMessage)
                End If

                Me.JoinedObjectType = arRoleInstance.JoinedORMObject

            Catch ex As Exception
                lsMessage = "Error: tFactDataInstance.New: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arFactInstance As FBM.FactInstance, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept, ByVal aiX As Integer, ByVal aiY As Integer)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
            Call Me.New(arPage, arFactInstance, arRoleInstance, ar_concept)
            Me.ConceptType = pcenumConceptType.RoleData
            Me.X = aiX
            Me.Y = aiY

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.FactDataInstance) As Boolean Implements System.IEquatable(Of FBM.FactDataInstance).Equals

            If (Me.Role.Id = other.Role.Id) And (Me.Concept.Symbol = other.Concept.Symbol) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByRole(ByVal other As FBM.FactDataInstance) As Boolean

            If (Me.Role.Id = other.Role.Id) Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Clones the FactDataInstance.
        ''' </summary>
        ''' <param name="arPage"></param>
        ''' <returns></returns>
        ''' <remarks>e.g. Used when a complete Clone of a Model (and its set of Pages) is made.</remarks>
        Public Overloads Function Clone(ByRef arPage As FBM.Page, ByRef arFactInstance As FBM.FactInstance) As Object

            Dim lrFactDataInstance As New FBM.FactDataInstance

            Try
                With Me
                    lrFactDataInstance.Model = arPage.Model
                    lrFactDataInstance.Page = arPage
                    lrFactDataInstance.Id = .Id
                    lrFactDataInstance.Name = .Concept.Symbol
                    lrFactDataInstance.Symbol = .Symbol

                    lrFactDataInstance.Role = arPage.RoleInstance.Find(AddressOf .Role.Equals)
                    lrFactDataInstance.Data = .Data

                    lrFactDataInstance.Fact = New FBM.FactInstance
                    lrFactDataInstance.Fact = arFactInstance
                    lrFactDataInstance.FactData = .FactData   '20150113-was commented out with comment 'Need a solution to this. Add arFact as attribute to this function.'
                    lrFactDataInstance.FactDataInstance = Me

                    lrFactDataInstance.JoinedObjectType = lrFactDataInstance.Role.JoinedORMObject

                    lrFactDataInstance.X = .X
                    lrFactDataInstance.Y = .Y
                End With

                Return lrFactDataInstance

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrFactDataInstance
            End Try

        End Function

        Public Overloads Function CloneFactData() As FBM.FactData

            Dim lrFactData As New FBM.FactData

            Try

                With Me
                    lrFactData.Model = .Model
                    lrFactData.Symbol = .Symbol
                    lrFactData.Concept = New FBM.Concept(.Data)
                    lrFactData.FactType = .FactType
                    lrFactData.Name = .Concept.Symbol
                    lrFactData.Role = .Model.Role.Find(Function(x) x.Id = .Role.Id)
                End With

                Return lrFactData

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function CloneActor(ByRef arPage As FBM.Page) As UML.Actor

            Dim lr_actor As New UML.Actor

            With Me
                lr_actor.Model = .Model
                lr_actor.Page = arPage
                lr_actor.ConceptType = pcenumConceptType.Actor 'While this is redundant, it seems that it is required for Polymorphic use under tEntity
                lr_actor.FactData = .FactData
                lr_actor.Name = .Concept.Symbol
                lr_actor.FactDataInstance = New FBM.FactDataInstance
                lr_actor.FactDataInstance = Me
                lr_actor.JoinedObjectType = .Role.JoinedORMObject
                lr_actor.Concept = .Concept
                lr_actor.Role = .Role
                lr_actor.X = .X
                lr_actor.Y = .Y
                lr_actor.Shape = .Shape
                lr_actor.TableShape = .TableShape
            End With

            Return lr_actor

        End Function

        Public Overridable Function CloneBPMNActor(ByRef arPage As FBM.Page) As BPMN.Actor

            Dim lrBPMNActor As New BPMN.Actor

            Try
                With Me
                    lrBPMNActor.Model = .Model
                    lrBPMNActor.Page = arPage
                    lrBPMNActor.ConceptType = pcenumConceptType.Actor 'While this is redundant, it seems that it is required for Polymorphic use under tEntity
                    lrBPMNActor.FactData = .FactData
                    lrBPMNActor.Name = .Concept.Symbol
                    lrBPMNActor.FactDataInstance = New FBM.FactDataInstance
                    lrBPMNActor.FactDataInstance = Me
                    lrBPMNActor.JoinedObjectType = .Role.JoinedORMObject
                    lrBPMNActor.Concept = .Concept
                    lrBPMNActor.Role = .Role
                    lrBPMNActor.X = .X
                    lrBPMNActor.Y = .Y
                    lrBPMNActor.Shape = .Shape
                    lrBPMNActor.TableShape = .TableShape
                    lrBPMNActor.NameShape.Actor = lrBPMNActor
                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrBPMNActor

        End Function



        Public Overridable Function CloneUCDActor(ByRef arPage As FBM.Page) As UCD.Actor

            Dim lrUCDActor As New UCD.Actor

            Try
                With Me
                    lrUCDActor.Model = .Model
                    lrUCDActor.Page = arPage
                    lrUCDActor.ConceptType = pcenumConceptType.Actor 'While this is redundant, it seems that it is required for Polymorphic use under tEntity
                    lrUCDActor.FactData = .FactData
                    lrUCDActor.Name = .Concept.Symbol
                    lrUCDActor.FactDataInstance = New FBM.FactDataInstance
                    lrUCDActor.FactDataInstance = Me
                    lrUCDActor.JoinedObjectType = .Role.JoinedORMObject
                    lrUCDActor.Concept = .Concept
                    lrUCDActor.Role = .Role
                    lrUCDActor.X = .X
                    lrUCDActor.Y = .Y
                    lrUCDActor.Shape = .Shape
                    lrUCDActor.TableShape = .TableShape
                    lrUCDActor.NameShape.Actor = lrUCDActor
                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrUCDActor

        End Function


        Public Function CloneAttribute(ByRef arPage As FBM.Page) As ERD.Attribute

            Dim lrAttribute As New ERD.Attribute

            With Me
                lrAttribute.Model = .Model
                lrAttribute.Page = arPage
                lrAttribute.ConceptType = pcenumConceptType.Actor 'While this is redundant, it seems that it is required for Polymorphic use under tEntity
                lrAttribute.FactData = .FactData
                lrAttribute.Id = .Concept.Symbol
                lrAttribute.Name = .Concept.Symbol
                lrAttribute.FactDataInstance = Me
                lrAttribute.JoinedObjectType = .Role.JoinedORMObject
                lrAttribute.Concept = .Concept
                lrAttribute.Role = .Role
                lrAttribute.X = .X
                lrAttribute.Y = .Y
                lrAttribute.Shape = .Shape
                lrAttribute.TableShape = .TableShape
            End With

            Return lrAttribute

        End Function


        Public Function CloneConceptInstance() As FBM.ConceptInstance

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Id, Me.ConceptType)

            lrConceptInstance.Symbol = Me.Data
            lrConceptInstance.RoleId = Me.Role.Id
            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y

            Return lrConceptInstance

        End Function

        Public Function CloneDataStore(ByRef arPage As FBM.Page) As DFD.DataStore

            Dim lrDataStore As New DFD.DataStore

            With Me
                lrDataStore.Model = .Model
                lrDataStore.Page = arPage
                lrDataStore.ConceptType = pcenumConceptType.DataStore
                lrDataStore.FactData = Me.FactData
                lrDataStore.Name = .Concept.Symbol
                lrDataStore.Symbol = .Data
                lrDataStore.FactDataInstance = Me
                lrDataStore.JoinedObjectType = Me.Role.JoinedORMObject
                lrDataStore.Concept = .Concept
                lrDataStore.Role = .Role
                lrDataStore.X = .X
                lrDataStore.Y = .Y
            End With

            Return lrDataStore

        End Function

        ''' <summary>
        ''' End State Indicators are circles linked from the last State in a State Transition Diagram.
        '''   NB An STD may have more than one End/Finishing State. See also CloneStartStateIndicator.
        ''' </summary>
        ''' <param name="arPage"></param>
        ''' <returns></returns>
        Public Function CloneEndStateIndicator(ByRef arPage As FBM.Page) As STD.EndStateIndicator

            '------------------------------------------------------------------
            'As in 'Start Status Indicator' within a State Transition Diagram
            '------------------------------------------------------------------
            Dim lrEndStateIndicator As New STD.EndStateIndicator

            With Me
                lrEndStateIndicator.EndStateId = .Data
                lrEndStateIndicator.Model = .Model
                lrEndStateIndicator.Page = arPage
                'lrEndStateIndicator.Name = .Concept.Symbol
                lrEndStateIndicator.Fact = Me.Fact
                lrEndStateIndicator.FactDataInstance = Me
                lrEndStateIndicator.Concept = .Concept
                lrEndStateIndicator.X = .X
                lrEndStateIndicator.Y = .Y
            End With

            Return lrEndStateIndicator

        End Function

        Public Overridable Function ClonePageObject() As FBM.PageObject

            Dim lrPageObject As New FBM.PageObject

            lrPageObject.Name = Me.Name
            lrPageObject.Shape = Me.Shape
            lrPageObject.X = Me.X
            lrPageObject.Y = Me.Y

            Return lrPageObject

        End Function


        Public Overridable Function ClonePGSNodeType(ByRef arPage As FBM.Page) As PGS.Node

            '-----------------------------------------------------
            'As in 'Entity' within an EntityRelationshipDiagram
            '-----------------------------------------------------

            Dim lrPGSNode As New PGS.Node

            With Me
                lrPGSNode.Fact = .Fact
                lrPGSNode.Model = .Model
                lrPGSNode.Page = arPage
                lrPGSNode.FactData = Me.FactData
                lrPGSNode.Name = .Concept.Symbol
                lrPGSNode.FactDataInstance = Me
                lrPGSNode.JoinedObjectType = Me.Role.JoinedORMObject
                lrPGSNode.Concept = .Concept
                lrPGSNode.Role = .Role
                lrPGSNode.X = .X
                lrPGSNode.Y = .Y
            End With

            Return lrPGSNode

        End Function

        Public Function CloneEntity(ByRef arPage As FBM.Page) As ERD.Entity

            '----------------------------------------------------
            'As in 'Entity' within an EntityRelationshipDiagram
            '----------------------------------------------------
            Dim lrEntity As New ERD.Entity

            With Me
                lrEntity.Model = .Model
                lrEntity.Page = arPage
                lrEntity.Fact = .Fact
                lrEntity.FactData = Me.FactData
                lrEntity.Name = .Concept.Symbol
                lrEntity.FactDataInstance = Me
                lrEntity.JoinedObjectType = Me.Role.JoinedORMObject
                lrEntity.Concept = .Concept
                lrEntity.Role = .Role
                lrEntity.X = .X
                lrEntity.Y = .Y
            End With

            Return lrEntity

        End Function

        Public Shadows Function CloneEntityTypeInstance(ByRef arPage As FBM.Page) As FBM.EntityTypeInstance

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

            With Me
                lrEntityTypeInstance.Model = .Model
                lrEntityTypeInstance.Page = arPage
                lrEntityTypeInstance.Id = .Concept.Symbol
                lrEntityTypeInstance.Name = .Concept.Symbol
                lrEntityTypeInstance.X = .X
                lrEntityTypeInstance.Y = .Y
            End With

            Return lrEntityTypeInstance

        End Function

        Public Function CloneProcess(ByRef arPage As FBM.Page) As UML.Process

            Dim lrProcess As New UML.Process

            With Me
                lrProcess.Model = .Model
                lrProcess.Page = arPage
                lrProcess.ConceptType = pcenumConceptType.Process 'While this is redundant, it seems that it is required for Polymorphic use under tEntity
                lrProcess.FactData = Me.FactData
                lrProcess.Id = .Concept.Symbol
                lrProcess.Name = .Concept.Symbol
                lrProcess.Symbol = .Data
                lrProcess.FactDataInstance = New FBM.FactDataInstance
                lrProcess.FactDataInstance = Me
                lrProcess.JoinedObjectType = Me.Role.JoinedORMObject
                lrProcess.Concept = .Concept
                lrProcess.Role = .Role
                lrProcess.X = .X
                lrProcess.Y = .Y
            End With

            Return lrProcess

        End Function

        Public Function CloneUCDProcess(ByRef arPage As FBM.Page) As UCD.Process

            Dim lrProcess As New UCD.Process

            Try
                With Me
                    lrProcess.Model = .Model
                    lrProcess.Page = arPage
                    lrProcess.ConceptType = pcenumConceptType.Process 'While this is redundant, it seems that it is required for Polymorphic use under tEntity
                    lrProcess.FactData = Me.FactData
                    lrProcess.Id = .Concept.Symbol
                    lrProcess.Name = .Concept.Symbol
                    lrProcess.Symbol = .Data
                    lrProcess.FactDataInstance = Me
                    lrProcess.JoinedObjectType = Me.Role.JoinedORMObject
                    lrProcess.Concept = .Concept
                    lrProcess.Role = .Role
                    lrProcess.X = .X
                    lrProcess.Y = .Y
                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrProcess

        End Function



        Public Sub ChangeData(ByVal asNewSymbol As String)

            Dim lrConceptInstance As FBM.ConceptInstance
            Dim lrOriginalDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Concept.Symbol, pcenumConceptType.Value)
            Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.Model, asNewSymbol, pcenumConceptType.Value)

            lrNewDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals)

            If IsSomething(lrNewDictionaryEntry) Then
                '--------------------------------------------------------------
                'The NewDictionaryEntry already exists in the ModelDictionary.
                '  The FactDataInstance will point to this DictionaryEntry.
                '--------------------------------------------------------------
            Else
                '--------------------------------------------------------------
                'The NewDictionaryEntry does not exist in the ModelDictionary
                '  so add the NewDictionaryEntry to the ModelDictionary and
                '  save the NewDictionaryEntry into the database.
                '--------------------------------------------------------------
                lrNewDictionaryEntry = New FBM.DictionaryEntry(Me.Model, asNewSymbol, pcenumConceptType.Value)
                Me.Model.AddModelDictionaryEntry(lrNewDictionaryEntry)
                lrNewDictionaryEntry.Save()
            End If

            '---------------------------------------------------------------------------------------------------------------------------
            'If the original ModelDictionary.Entry (Concept.Symbol) is not used by any other RoleData for any other Role
            '  or FactType.Role then the original ModelDictionary.Entry must be removed from the ModelDictionary.
            '---------------------------------------------------------------------------------------------------------------------------
            Dim lrFactType As FBM.FactType
            Dim lrFact As FBM.Fact
            Dim lrRoleData As FBM.FactData
            Dim lbDeleteModelDictionaryEntry As Boolean = True
            For Each lrFactType In Me.Model.FactType
                For Each lrFact In lrFactType.Fact
                    For Each lrRoleData In lrFact.Data
                        If lrRoleData.Role.Id = Me.Role.Id Then
                            '-----------------------------------------------------
                            'Don't check because is the RoleData being modified.
                            '-----------------------------------------------------
                        Else
                            If Not (lrRoleData.Role.JoinedORMObject Is Me.Role.JoinedORMObject) Then
                                If lrRoleData.Data = lrOriginalDictionaryEntry.Symbol Then
                                    lbDeleteModelDictionaryEntry = False
                                End If
                            End If
                        End If
                    Next
                Next
            Next

            Call Me.Model.RemoveDictionaryEntry(lrOriginalDictionaryEntry, True)

            '------------------------------------------------------------------------------------------------------
            'If the new DictionaryEntry already exists as a ConceptInstance on the Page 
            '  then don't modify the original (duplicate primaryKey error) but delete the original
            '------------------------------------------------------------------------------------------------------
            lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me.Page, Me.Data, pcenumConceptType.Value)

            If Not TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                Call TableConceptInstance.ModifySymbol(lrConceptInstance, asNewSymbol)
            Else
                '---------------------------------------------------------------
                'The ConceptInstance already exists in the database, so simply
                '  remove the existing ConceptInstance
                '---------------------------------------------------------------
                Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance)
            End If

            Me.FactData.Data = asNewSymbol

        End Sub

        '-----------------------------------------------------------------------------
        'VM-20150316-Can probably remove this event handler. If some time has passed
        '  and all seems fine with Richmond, remove this commented out code.
        'Private Sub update_from_model_object_type() Handles JoinedObjectType.Updated

        '    '---------------------------------------------------------------------
        '    'Linked by Delegate in New to the 'update' event of the ModelObject 
        '    '  referenced by Objects of this Class
        '    '---------------------------------------------------------------------
        '    Try
        '        'Me.concept = Me.FactData.concept

        '        If IsSomething(Me.Page.Diagram) Then
        '            '------------------
        '            'Diagram is set.
        '            '------------------
        '            If IsSomething(Me.TableShape) Then
        '                If Me.Cell.Text <> "" Then
        '                    '---------------------------------------------------------------------------------
        '                    'Is the type of EntityTypeInstance that 
        '                    '  shows the EntityTypeName within the
        '                    '  ShapeNode itself and not a separate
        '                    '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
        '                    ' 1 for the stickfigure, the other for the name of the Actor.
        '                    '---------------------------------------------------------------------------------
        '                    Me.Cell.Text = Trim(Me.FactData.Concept.Symbol)
        '                End If
        '            ElseIf IsSomething(Me.shape) Then
        '                '---------------------------------------------------------------------------------
        '                'Is the type of EntityTypeInstance that 
        '                '  shows the EntityTypeName within the
        '                '  ShapeNode itself and not a separate
        '                '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
        '                ' 1 for the stickfigure, the other for the name of the Actor.
        '                '---------------------------------------------------------------------------------
        '                Me.shape.Text = Trim(Me.Symbol)
        '            End If

        '            Me.Page.Diagram.Invalidate()
        '        End If
        '    Catch lo_err As Exception
        '        MsgBox("t_ORM_role_data_instance.update_from_model: " & lo_err.Message & "FactTypeId: " & Me.Role.FactType.FactType.Id & ", ValueSymbol:" & Me.Concept.Symbol & ", PageId:" & Me.Page.PageId)
        '    End Try

        'End Sub

        Private Sub FactData_ConceptSwitched(ByRef arConcept As Concept) Handles FactData.ConceptSwitched

            Dim lrNewConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.FactData.Concept.Symbol, pcenumConceptType.Value)
            lrNewConceptInstance.RoleId = Me.Role.Id

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Data, pcenumConceptType.Value)
            lrConceptInstance.RoleId = Me.Role.Id

            If TableConceptInstance.ExistsConceptInstance(lrNewConceptInstance) Then
                Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance)
            Else
                Call Me.FactData.Concept.Save()
                Call TableConceptInstance.ModifySymbol(lrConceptInstance, Me.FactData.Concept.Symbol)
            End If

            Me.Concept = arConcept
            Me.Symbol = Me.Concept.Symbol
            Me.isDirty = True
            Me.Fact.isDirty = True
            Me.Fact.FactType.isDirty = True

            If Me.Page Is Nothing Then
                Exit Sub
            Else
                Me.Page.IsDirty = True
            End If

            If IsSomething(Me.Page.Diagram) Then
                '------------------
                'Diagram is set.
                '------------------
                If IsSomething(Me.Cell) Then
                    If Me.Cell.Text <> "" Then
                        '---------------------------------------------------------------------------------
                        'Is the type of EntityTypeInstance that 
                        '  shows the EntityTypeName within the
                        '  ShapeNode itself and not a separate
                        '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
                        ' 1 for the stickfigure, the other for the name of the Actor.
                        '---------------------------------------------------------------------------------
                        Me.Cell.Text = Trim(Me.Concept.Symbol)
                    End If
                End If
                Me.Page.Diagram.Invalidate()
            End If

        End Sub

        Private Sub UpdateFromFactData() Handles FactData.ConceptSymbolUpdated

            Try
                Dim lrNewConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.FactData.Concept.Symbol, pcenumConceptType.Value)
                lrNewConceptInstance.RoleId = Me.Role.Id

                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Symbol, pcenumConceptType.Value)
                lrConceptInstance.RoleId = Me.Role.Id

                If Not Me.Model.StoreAsXML Then
                    If TableConceptInstance.ExistsConceptInstance(lrNewConceptInstance) Then 'If the NEW ConceptInstance exists
                        Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance) 'We don't need the original Concept Instance for the Model, Page, Symbol, Role                    
                    Else
                        Call TableConceptInstance.ModifySymbol(lrConceptInstance, Me.FactData.Concept.Symbol) 'Update the EXISTING ConceptInstance to the NEW ConceptInstance
                        If Not TableConceptInstance.ExistsConceptInstance(lrNewConceptInstance) Then
                            TableConceptInstance.AddConceptInstance(lrNewConceptInstance)
                        End If
                    End If
                End If

                '20180718-VM-Removed because of the above.
                'Call TableConceptInstance.UpdateConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance, Me.Symbol)

                Me.Concept = Me.FactData.Concept
                Me.Symbol = Me.Concept.Symbol

                Me.Id = Me.FactData.Id
                Me.Name = Me.FactData.Name

                Me.makeDirty()

                'NB Me.Page.MakeDirty() is not required here because the above modifies the Symbol.
                'NB Me.makeDirty() is not required here because the above modifies the Symbol.

                'Call TableConceptInstance.ModifySymbol(lrConceptInstance, Me.FactData.Concept.Symbol)

                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.Cell) Then
                        If Me.Cell.Text <> "" Then
                            '---------------------------------------------------------------------------------
                            'Is the type of EntityTypeInstance that 
                            '  shows the EntityTypeName within the
                            '  ShapeNode itself and not a separate
                            '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
                            ' 1 for the stickfigure, the other for the name of the Actor.
                            '---------------------------------------------------------------------------------
                            Me.Cell.Text = Trim(Me.Concept.Symbol)
                        End If
                    End If
                    Me.Page.Diagram.Invalidate()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: t_ORM_role_data_instance.update_from_model"
                lsMessage &= vbCrLf & vbCrLf & "FactTypeId: " & Me.Role.FactType.FactType.Id & ", ValueSymbol:" & Me.Concept.Symbol & ", PageId:" & Me.Page.PageId
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overridable Function RemoveFromPage() As Boolean

            Try
                Me.Fact.Data.Remove(Me)

                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Data, pcenumConceptType.Value)
                TableConceptInstance.DeleteConceptInstance(lrConceptInstance)

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

        Public Overrides Function setName(ByVal asNewName As String,
                                          Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                          Optional ByVal abSuppressModelSave As Boolean = False) As Boolean

            '----------------------------------------------------------------------------------------------
            'Modify the FactData referenced by the FactData instance.
            '  The FactDataInstance event handler (FactData.Updated) manages(the) changes 
            '  to Me.Id, Me.Name, Me.Data etc
            '----------------------------------------------------------------------------------------------
            Try
                Me.FactData.Id = asNewName
                Me.FactData.Name = asNewName
                Me.FactData.Data = asNewName
                Me.FactData.isDirty = True

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

        Public Overloads Sub SwitchConcept(ByVal asNewInstance As String)

            Dim lsDebugMessage As String = ""
            Dim lrConceptInstance As FBM.ConceptInstance

            Try

                '---------------------------------------------------------------------------------------------------------------------------------
                'Code Safe: If the new Concept (being switched to) already exists in the Model, then switch the Concept to that DictionaryEntry.
                '---------------------------------------------------------------------------------------------------------------------------------
                If Me.Concept.Symbol = asNewInstance Then
                    Exit Sub 'Nothing to do here.
                End If

                Dim lrOriginalDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Concept.Symbol, pcenumConceptType.Value)
                Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.Model, asNewInstance, pcenumConceptType.Value)
                lrNewDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals)

                '---------------------------------------------------------------------------------------------------------------------------
                'If the original ModelDictionary.Entry (Concept.Symbol) is not used by any other RoleData for any other Role
                '  or FactType.Role then the original ModelDictionary.Entry must be removed from the ModelDictionary.
                '---------------------------------------------------------------------------------------------------------------------------
                Dim lrFactType As FBM.FactType
                Dim lrFact As FBM.Fact
                Dim lrRoleData As FBM.FactData
                Dim lbDeleteModelDictionaryEntry As Boolean = True
                For Each lrFactType In Me.Model.FactType
                    For Each lrFact In lrFactType.Fact
                        For Each lrRoleData In lrFact.Data
                            If lrRoleData.Role.Id = Me.Role.Id Then
                                '-----------------------------------------------------
                                'Don't check because is the RoleData being modified.
                                '-----------------------------------------------------
                            Else
                                If Not (lrRoleData.Role.JoinedORMObject Is Me.Role.JoinedORMObject) Then
                                    If lrRoleData.Data = lrOriginalDictionaryEntry.Symbol Then
                                        lbDeleteModelDictionaryEntry = False
                                    End If
                                End If
                            End If
                        Next
                    Next
                Next

                If lrOriginalDictionaryEntry.Realisations.Count = 0 Then
                    Me.Model.RemoveDictionaryEntry(lrOriginalDictionaryEntry, True)
                End If

                '------------------------------------------------------------------------------------------------------
                'If the new DictionaryEntry already exists as a ConceptInstance on the Page 
                '  then don't modify the original (duplicate primaryKey error) but delete the original
                '------------------------------------------------------------------------------------------------------
                lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me.Page, Me.Data, pcenumConceptType.Value)

                If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                    '---------------------------------------------------------------
                    'The ConceptInstance already exists in the database, so simply
                    '  remove the existing ConceptInstance
                    '---------------------------------------------------------------
                    Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance)
                Else
                    Call TableConceptInstance.ModifySymbol(lrConceptInstance, asNewInstance)
                End If

                If IsSomething(lrNewDictionaryEntry) Then
                    '----------------------------------------------------------------------------
                    'The NewConcept exists in the ModelDictionary
                    '  Substitute the existing Concept for a ModelDictionary entry (Concept) that
                    '  already exists in the Model
                    '----------------------------------------------------------------------------
                    Me.Concept = lrNewDictionaryEntry.Concept
                Else
                    '-------------------------------------------------------
                    'The NewConcept does not exist in the ModelDictionary.
                    '  Modify the existing Concept to a new Concept
                    '-------------------------------------------------------                    

                    '-------------------------------------------------
                    'Create a new DictionaryEntry for the NewConcept
                    '-------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, asNewInstance, pcenumConceptType.Value)
                    Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                    lrNewDictionaryEntry.Save()

                    Me.Concept = lrDictionaryEntry.Concept
                End If
                Me.Page.MakeDirty()
                Call Me.Model.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: FBM.tFactData.SwitchConcept"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Sub makeDirty()
            Me.isDirty = True
            Me.Page.IsDirty = True
            If Me.Fact IsNot Nothing Then
                Me.Fact.isDirty = True
                If Me.Fact.FactType IsNot Nothing Then
                    Me.Fact.FactType.isDirty = True
                End If
            End If
        End Sub

        Public Shadows Sub setData(ByVal asData As String, Optional ByRef arConcept As FBM.Concept = Nothing)

            Me.Symbol = asData
            Me.Concept = arConcept
            Me.isDirty = True

        End Sub

        Private Sub FactDataInstance_ConceptSwitched(ByRef arConcept As Concept) Handles Me.ConceptSwitched

            Me.Concept = arConcept
            Me.Symbol = Me.Concept.Symbol

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Data, pcenumConceptType.Value)
            lrConceptInstance.RoleId = Me.Role.Id
            Call TableConceptInstance.ModifySymbol(lrConceptInstance, Me.FactData.Concept.Symbol)

        End Sub

        Private Sub FactDataInstance_ConceptSymbolUpdated() Handles Me.ConceptSymbolUpdated

        End Sub

        Public Function CloneState(ByRef arPage As FBM.Page) As STD.State
            '--------------------------------------------
            'Clones a FactDataInstance as a "State".
            '
            'States are 'Values' of 'ValueTypes'
            '  for models like a StateTransitionDiagram.
            '--------------------------------------------
            Dim lrSTDState As New STD.State(arPage)

            With Me
                lrSTDState.Id = .Data
                lrSTDState.Model = .Model
                lrSTDState.FactData = Me.FactData
                lrSTDState.Fact = Me.Fact
                lrSTDState.Name = .Concept.Symbol
                lrSTDState.FactDataInstance = Me
                lrSTDState.JoinedObjectType = Me.Role.JoinedORMObject
                lrSTDState.Concept = .Concept
                lrSTDState.Role = .Role
                lrSTDState.X = .X
                lrSTDState.Y = .Y
                lrSTDState.StateName = .Concept.Symbol
            End With

            Return lrSTDState

        End Function

        ''' <summary>
        ''' If not DeleteAll, then the ConceptInstance/s for the Fact need to be removed from the database elsewhere.
        '''   The reason for this is that some ConceptInstances are Values for a Role, and may not belong to the Fact being deleted.
        ''' </summary>
        ''' <param name="abDeleteAll"></param>
        Private Sub FactData_RemovedFromModel(ByVal abDeleteAll As Boolean) Handles FactData.RemovedFromModel

            If abDeleteAll Then
                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Data, pcenumConceptType.Value)
                lrConceptInstance.RoleId = Me.Role.Id
                Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance)

                Dim lrModelDictionaryEntry = Me.Model.ModelDictionary.Find(Function(x) x.Symbol = Me.Data)
                If lrModelDictionaryEntry IsNot Nothing Then
                    Call lrModelDictionaryEntry.Realisations.Remove(pcenumConceptType.Value)
                End If
            End If

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected
            Throw New NotImplementedException()
        End Sub

        Public Overridable Sub Move(aiNewX As Integer, aiNewY As Integer, abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move
            Throw New NotImplementedException()
        End Sub

        Public Sub Moved() Implements iPageObject.Moved
            Throw New NotImplementedException()
        End Sub

        Public Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects
            Throw New NotImplementedException()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour
            Throw New NotImplementedException()
        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub

        Private Sub FactData_ModelErrorsRemoved() Handles FactData.ModelErrorsRemoved

            Try
                If Me.Cell IsNot Nothing Then
                    Me.Cell.TextColor = Color.Black
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace