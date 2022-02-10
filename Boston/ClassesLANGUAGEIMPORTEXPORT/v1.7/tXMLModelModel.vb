Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Threading.Tasks

Namespace XMLModel

    ''' <summary>
    ''' v1.3 Adds the GUID field to the ValueType, FactType and RoleConstraint classes.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Model

        <XmlAttribute()>
        Public XSDVersionNr As Double = 1.7
        Public ORMModel As New XMLModel.ORMModel
        Public ORMDiagram As New List(Of XMLModel.Page)

        ''' <summary>
        ''' Maps an instance of FBM.Model to this class.
        ''' </summary>
        ''' <param name="arFBMModel">The model being mapped.</param>
        ''' <remarks></remarks>
        Public Sub MapFromFBMModel(ByVal arFBMModel As FBM.Model)

            Try
                Me.ORMModel = New XMLModel.ORMModel
                Me.ORMModel.ModelId = arFBMModel.ModelId
                Me.ORMModel.Name = arFBMModel.Name
                Me.ORMModel.CoreVersionNumber = arFBMModel.CoreVersionNumber

                '========================
                'Process the ValueTypes
                '========================
                Dim lrValueType As FBM.ValueType
                Dim lrXMLValueType As XMLModel.ValueType
                For Each lrValueType In arFBMModel.ValueType

                    lrXMLValueType = New XMLModel.ValueType
                    lrXMLValueType.Id = lrValueType.Id
                    lrXMLValueType.Name = lrValueType.Name
                    lrXMLValueType.DataType = lrValueType.DataType
                    lrXMLValueType.Instance = lrValueType.Instance
                    lrXMLValueType.DataTypePrecision = lrValueType.DataTypePrecision
                    lrXMLValueType.DataTypeLength = lrValueType.DataTypeLength
                    lrXMLValueType.LongDescription = lrValueType.LongDescription
                    lrXMLValueType.ShortDescription = lrValueType.ShortDescription
                    lrXMLValueType.IsIndependent = lrValueType.IsIndependent
                    lrXMLValueType.GUID = lrValueType.GUID
                    lrXMLValueType.IsMDAModelElement = lrValueType.IsMDAModelElement

                    Dim lsValueConstraintValue As String

                    For Each lsValueConstraintValue In lrValueType.ValueConstraint
                        lrXMLValueType.ValueConstraint.Add(lsValueConstraintValue)
                    Next

                    Me.ORMModel.ValueTypes.Add(lrXMLValueType)
                Next

                '========================
                'Process the EntityTypes
                '========================
                Dim lrEntityType As FBM.EntityType
                Dim lrXMLEntityType As XMLModel.EntityType

                For Each lrEntityType In arFBMModel.EntityType

                    lrXMLEntityType = New XMLModel.EntityType

                    lrXMLEntityType.Id = lrEntityType.Id
                    lrXMLEntityType.GUID = lrEntityType.GUID
                    lrXMLEntityType.Name = lrEntityType.Name
                    lrXMLEntityType.IsObjectifyingEntityType = lrEntityType.IsObjectifyingEntityType
                    lrXMLEntityType.Instance = lrEntityType.Instance
                    lrXMLEntityType.ReferenceMode = lrEntityType.ReferenceMode
                    lrXMLEntityType.IsIndependent = lrEntityType.IsIndependent
                    lrXMLEntityType.IsAbsorbed = lrEntityType.IsAbsorbed
                    lrXMLEntityType.IsPersonal = lrEntityType.IsPersonal
                    lrXMLEntityType.IsDerived = lrEntityType.IsDerived
                    lrXMLEntityType.DerivationText = lrEntityType.DerivationText
                    lrXMLEntityType.LongDescription = lrEntityType.LongDescription
                    lrXMLEntityType.ShortDescription = lrEntityType.ShortDescription
                    lrXMLEntityType.IsMDAModelElement = lrEntityType.IsMDAModelElement
                    lrXMLEntityType.DBName = lrEntityType.DBName

                    If IsSomething(lrEntityType.ReferenceModeRoleConstraint) Then
                        lrXMLEntityType.ReferenceSchemeRoleConstraintId = lrEntityType.ReferenceModeRoleConstraint.Id
                    End If

                    If IsSomething(lrEntityType.ReferenceModeValueType) Then
                        lrXMLEntityType.ReferenceModeValueTypeId = lrEntityType.ReferenceModeValueType.Id
                    End If

                    Dim lrSubtypeRelationship As FBM.tSubtypeRelationship
                    Dim lrXMLSubtypeRelationship As XMLModel.SubtypeRelationship

                    For Each lrSubtypeRelationship In lrEntityType.SubtypeRelationship

                        lrXMLSubtypeRelationship = New XMLModel.SubtypeRelationship
                        lrXMLSubtypeRelationship.ParentEntityTypeId = lrSubtypeRelationship.parentEntityType.Id
                        lrXMLSubtypeRelationship.SubtypingFactTypeId = lrSubtypeRelationship.FactType.Id

                        lrXMLEntityType.SubtypeRelationships.Add(lrXMLSubtypeRelationship)

                    Next

                    Me.ORMModel.EntityTypes.Add(lrXMLEntityType)
                Next

                '=======================
                'Process the FactTypes
                '=======================
                Dim lrFactType As FBM.FactType
                Dim lrXMLFactType As XMLModel.FactType

                For Each lrFactType In arFBMModel.FactType

                    lrXMLFactType = New XMLModel.FactType

                    lrXMLFactType.Id = lrFactType.Id
                    lrXMLFactType.GUID = lrFactType.GUID
                    lrXMLFactType.Name = lrFactType.Name
                    lrXMLFactType.IsObjectified = lrFactType.IsObjectified
                    lrXMLFactType.IsPreferredReferenceSchemeFT = lrFactType.IsPreferredReferenceMode
                    lrXMLFactType.IsSubtypeRelationshipFactType = lrFactType.IsSubtypeRelationshipFactType
                    If IsSomething(lrFactType.ObjectifyingEntityType) Then
                        lrXMLFactType.ObjectifyingEntityTypeId = lrFactType.ObjectifyingEntityType.Id
                    End If
                    lrXMLFactType.IsDerived = lrFactType.IsDerived
                    lrXMLFactType.IsStored = lrFactType.IsStored
                    lrXMLFactType.DerivationText = lrFactType.DerivationText
                    lrXMLFactType.LongDescription = lrFactType.LongDescription
                    lrXMLFactType.ShortDescription = lrFactType.ShortDescription
                    lrXMLFactType.IsLinkFactType = lrFactType.IsLinkFactType
                    If lrFactType.LinkFactTypeRole IsNot Nothing Then
                        lrXMLFactType.LinkFactTypeRoleId = lrFactType.LinkFactTypeRole.Id
                    End If
                    lrXMLFactType.IsMDAModelElement = lrFactType.IsMDAModelElement
                    lrXMLFactType.IsSubtypeStateControlling = lrFactType.IsSubtypeStateControlling
                    lrXMLFactType.StoreFactCoordinates = lrFactType.StoreFactCoordinates
                    lrXMLFactType.DBName = lrFactType.DBName

                    '---------------
                    'Map the Roles
                    '---------------
                    Dim lrRole As FBM.Role
                    Dim lrXMLRole As XMLModel.Role
                    For Each lrRole In lrFactType.RoleGroup

                        lrXMLRole = New XMLModel.Role

                        lrXMLRole.Id = lrRole.Id
                        lrXMLRole.Name = lrRole.Name
                        lrXMLRole.SequenceNr = lrRole.SequenceNr
                        lrXMLRole.Mandatory = lrRole.Mandatory

                        If IsSomething(lrRole.JoinedORMObject) Then
                            lrXMLRole.JoinedObjectTypeId = lrRole.JoinedORMObject.Id
                        End If

                        lrXMLFactType.RoleGroup.Add(lrXMLRole)
                    Next

                    '---------------
                    'Map the Facts
                    '---------------
                    Dim lrFact As FBM.Fact
                    Dim lrXMLFact As XMLModel.Fact
                    Dim lrFactData As FBM.FactData
                    Dim lrXMLFactData As XMLModel.FactData

                    For Each lrFact In lrFactType.Fact

                        lrXMLFact = New XMLModel.Fact

                        lrXMLFact.Id = lrFact.Id

                        '------------------
                        'Map the FactData
                        '------------------
                        For Each lrFactData In lrFact.Data
                            lrXMLFactData = New XMLModel.FactData

                            lrXMLFactData.RoleId = lrFactData.Role.Id
                            lrXMLFactData.Data = lrFactData.Data

                            lrXMLFact.Data.Add(lrXMLFactData)
                        Next

                        lrXMLFactType.Facts.Add(lrXMLFact)
                    Next

                    '----------------------
                    'Map FactTypeReadings
                    '----------------------
                    Dim lrFactTypeReading As FBM.FactTypeReading
                    Dim lrXMLFactTypeReading As XMLModel.FactTypeReading
                    Dim lrPredicatePart As FBM.PredicatePart
                    Dim lrXMLPredicatePart As XMLModel.PredicatePart

                    For Each lrFactTypeReading In lrFactType.FactTypeReading

                        lrXMLFactTypeReading = New XMLModel.FactTypeReading

                        lrXMLFactTypeReading.Id = lrFactTypeReading.Id
                        lrXMLFactTypeReading.FrontReadingText = lrFactTypeReading.FrontText
                        lrXMLFactTypeReading.FollowingReadingText = lrFactTypeReading.FollowingText

                        For Each lrPredicatePart In lrFactTypeReading.PredicatePart

                            lrXMLPredicatePart = New XMLModel.PredicatePart

                            lrXMLPredicatePart.SequenceNr = lrPredicatePart.SequenceNr
                            lrXMLPredicatePart.Role_Id = lrPredicatePart.RoleId
                            lrXMLPredicatePart.PreboundReadingText = lrPredicatePart.PreBoundText
                            lrXMLPredicatePart.PostboundReadingText = lrPredicatePart.PostBoundText
                            lrXMLPredicatePart.PredicatePartText = lrPredicatePart.PredicatePartText

                            lrXMLFactTypeReading.PredicateParts.Add(lrXMLPredicatePart)
                        Next

                        lrXMLFactType.FactTypeReadings.Add(lrXMLFactTypeReading)
                    Next


                    Me.ORMModel.FactTypes.Add(lrXMLFactType)
                Next

                '------------------------------------------------------------------------------------
                'IMPORTANT: Make sure the LinkFactTypes are at the end of the list
                Me.ORMModel.FactTypes.Sort(Function(x, y) x.IsLinkFactType.CompareTo(y.IsLinkFactType))

                '=========================
                'Map the RoleConstraints
                '=========================
                Dim lrRoleConstraint As FBM.RoleConstraint
                Dim lrXMLRoleConstraint As XMLModel.RoleConstraint
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
                Dim lrXMLRoleConstraintRole As XMLModel.RoleConstraintRole
                Dim lrXMLRoleConstraintArgument As XMLModel.RoleConstraintArgument

                For Each lrRoleConstraint In arFBMModel.RoleConstraint

                    lrXMLRoleConstraint = New XMLModel.RoleConstraint

                    lrXMLRoleConstraint.Id = lrRoleConstraint.Id
                    lrXMLRoleConstraint.GUID = lrRoleConstraint.GUID
                    lrXMLRoleConstraint.Name = lrRoleConstraint.Name
                    lrXMLRoleConstraint.RoleConstraintType = lrRoleConstraint.RoleConstraintType.ToString
                    lrXMLRoleConstraint.RingConstraintType = lrRoleConstraint.RingConstraintType.ToString
                    'lrXMLRoleConstraint.LevelNr = lrRoleConstraint.LevelNr
                    lrXMLRoleConstraint.IsPreferredUniqueness = lrRoleConstraint.IsPreferredIdentifier
                    lrXMLRoleConstraint.IsDeontic = lrRoleConstraint.IsDeontic
                    lrXMLRoleConstraint.MinimumFrequencyCount = lrRoleConstraint.MinimumFrequencyCount
                    lrXMLRoleConstraint.MaximumFrequencyCount = lrRoleConstraint.MaximumFrequencyCount
                    lrXMLRoleConstraint.Cardinality = lrRoleConstraint.Cardinality
                    lrXMLRoleConstraint.CardinalityRangeType = lrRoleConstraint.CardinalityRangeType.ToString
                    lrXMLRoleConstraint.LongDescription = lrRoleConstraint.LongDescription
                    lrXMLRoleConstraint.ShortDescription = lrRoleConstraint.ShortDescription
                    lrXMLRoleConstraint.IsMDAModelElement = lrRoleConstraint.IsMDAModelElement

                    For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole

                        lrXMLRoleConstraintRole = New XMLModel.RoleConstraintRole

                        lrXMLRoleConstraintRole.RoleId = lrRoleConstraintRole.Role.Id
                        lrXMLRoleConstraintRole.SequenceNr = lrRoleConstraintRole.SequenceNr
                        lrXMLRoleConstraintRole.IsEntry = lrRoleConstraintRole.IsEntry
                        lrXMLRoleConstraintRole.IsExit = lrRoleConstraintRole.IsExit

                        lrXMLRoleConstraint.RoleConstraintRoles.Add(lrXMLRoleConstraintRole)
                    Next

                    '----------------------------------------
                    'Construct the RoleConstraintArguments.
                    '----------------------------------------
                    Dim lrXMLRoleReference As XMLModel.RoleReference
                    Dim lrRole As FBM.Role
                    For Each lrRoleConstraintArgument In lrRoleConstraint.Argument
                        lrXMLRoleConstraintArgument = New XMLModel.RoleConstraintArgument
                        lrXMLRoleConstraintArgument.SequenceNr = lrRoleConstraintArgument.SequenceNr

                        For Each lrRoleConstraintRole In lrRoleConstraintArgument.RoleConstraintRole
                            lrXMLRoleReference = New XMLModel.RoleReference
                            lrXMLRoleReference.RoleId = lrRoleConstraintRole.Role.Id
                            lrXMLRoleConstraintArgument.Role.Add(lrXMLRoleReference)
                        Next

                        lrXMLRoleConstraintArgument.JoinPath = New XMLModel.JoinPath
                        lrXMLRoleConstraintArgument.JoinPath.JoinPathError = lrRoleConstraintArgument.JoinPath.JoinPathError
                        For Each lrRole In lrRoleConstraintArgument.JoinPath.RolePath
                            lrXMLRoleReference = New XMLModel.RoleReference
                            lrXMLRoleReference.RoleId = lrRole.Id
                            lrXMLRoleConstraintArgument.JoinPath.RolePath.Add(lrXMLRoleReference)
                        Next

                        lrXMLRoleConstraint.Argument.Add(lrXMLRoleConstraintArgument)
                    Next


                    For Each lsValueConstraintValue In lrRoleConstraint.ValueConstraint
                        lrXMLRoleConstraint.ValueConstraint.Add(lsValueConstraintValue)
                    Next

                    Me.ORMModel.RoleConstraints.Add(lrXMLRoleConstraint)
                Next

                '================
                'Map ModelNotes
                '================
                Dim lrModelNote As FBM.ModelNote
                Dim lrXMLModelNote As XMLModel.ModelNote

                For Each lrModelNote In arFBMModel.ModelNote

                    lrXMLModelNote = New XMLModel.ModelNote

                    lrXMLModelNote.Id = lrModelNote.Id
                    lrXMLModelNote.Note = lrModelNote.Text
                    lrXMLModelNote.IsMDAModelElement = lrModelNote.IsMDAModelElement
                    If lrModelNote.JoinedObjectType IsNot Nothing Then
                        lrXMLModelNote.JoinedObjectTypeId = lrModelNote.JoinedObjectType.Id
                    End If

                    Me.ORMModel.ModelNotes.Add(lrXMLModelNote)
                Next

                '================================
                'Map the Pages
                '================================
                Dim lrPage As FBM.Page

                For Each lrPage In arFBMModel.Page

                    Dim lrExportPage As New XMLModel.Page

                    lrExportPage.Id = lrPage.PageId
                    lrExportPage.Name = lrPage.Name
                    lrExportPage.Language = lrPage.Language
                    lrExportPage.IsCoreModelPage = lrPage.IsCoreModelPage
                    '--------------------------------------------------------------------
                    'Add all of the ConceptTypes that are in the ModelConceptInstance table.
                    ' These are:
                    '           - EntityType(s)
                    '           - ValueTypes(s)
                    '           - FactType(s)
                    '           - RoleConstraint(s)
                    '           - Fact(s)
                    '           - Value(s)
                    '           - ModelNotes
                    '
                    ' Nothing else is required to draw a ConceptualModel.
                    ' Different types of ConceptualModel require different types of Concepts
                    ' as they appear in a Model such that the ConceptualModel can be drawn.
                    ' e.g. Within a Use Case Diagram, Actors are Values within a Fact.
                    ' The Fact has no X/Y co-ordinate, but the Value/Actor does.
                    ' NB A Value (e.g. 'Storeman' may well be an EntityType within an ORM-Diagram
                    '  but within a Use Case Diagram, drawn from the Core-UseCaseDiagram (metamodel)
                    '  ORM-Diagram, the Value becomes an Actor(Entity).
                    ' Although this is Higher-Order Logic at work, FOL is preserved because
                    ' the modeler (or tool. e.g. Richmond) only works with on Page/View (i.e. 'Interpretation')
                    ' at a time. i.e. While working with a Use Case Diagram, Richmond works over
                    ' the MetaModel of a Use Case Diagram in First-Order Logic.
                    '-----------------------------------------------------------------------

                    '------------------------------------------
                    'Establish ConceptInstances for the Page.
                    '------------------------------------------
                    Dim lrConceptInstance As New FBM.ConceptInstance

                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                    Dim lrValueTypeInstance As FBM.ValueTypeInstance
                    Dim lrFactTypeInstance As FBM.FactTypeInstance
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    Dim lrFactInstance As FBM.FactInstance
                    Dim lrFactDataInstance As FBM.FactDataInstance
                    Dim lrRoleInstance As FBM.RoleInstance
                    Dim lrModelNoteInstance As FBM.ModelNoteInstance

                    '--------------------------------------------------------------
                    'Establish the set of EntityTypeInstances that are on the Page
                    '  as ConceptInstances.
                    '--------------------------------------------------------------
                    For Each lrEntityTypeInstance In lrPage.EntityTypeInstance
                        lrConceptInstance = lrEntityTypeInstance.CloneConceptInstance
                        '-------------------------------------
                        'Add the ConceptInstance to the Page
                        '-------------------------------------
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    Next

                    '--------------------------------------------------------------
                    'Establish the set of ValueTypeInstances that are on the Page
                    '  as ConceptInstances.
                    '--------------------------------------------------------------
                    For Each lrValueTypeInstance In lrPage.ValueTypeInstance
                        lrConceptInstance = lrValueTypeInstance.CloneConceptInstance
                        '-------------------------------------
                        'Add the ConceptInstance to the Page
                        '-------------------------------------
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    Next

                    '--------------------------------------------------------------
                    'Establish the set of FactTypeInstances that are on the Page
                    '  as ConceptInstances.
                    '--------------------------------------------------------------
                    For Each lrFactTypeInstance In lrPage.FactTypeInstance
                        lrConceptInstance = lrFactTypeInstance.CloneConceptInstance
                        '-------------------------------------
                        'Add the ConceptInstance to the Page
                        '-------------------------------------
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)

                        If lrFactTypeInstance.FactType.IsDerived _
                            And (Trim(lrFactTypeInstance.FactType.DerivationText) <> "") _
                            And lrFactTypeInstance.FactTypeDerivationText IsNot Nothing Then

                            lrConceptInstance = New FBM.ConceptInstance(lrFactTypeInstance.Model,
                                                                         lrFactTypeInstance.Page,
                                                                         lrFactTypeInstance.Id,
                                                                         pcenumConceptType.DerivationText)
                            lrConceptInstance.X = lrFactTypeInstance.FactTypeDerivationText.X
                            lrConceptInstance.Y = lrFactTypeInstance.FactTypeDerivationText.Y

                            '-------------------------------------
                            'Add the ConceptInstance to the Page
                            '-------------------------------------
                            lrExportPage.ConceptInstance.Add(lrConceptInstance)

                        End If

                        'FactTypeReading
                        lrConceptInstance = New FBM.ConceptInstance(lrFactTypeInstance.Model,
                                                                     lrFactTypeInstance.Page,
                                                                     lrFactTypeInstance.Id,
                                                                     pcenumConceptType.FactTypeReading)
                        lrConceptInstance.X = lrFactTypeInstance.FactTypeReadingPoint.X
                        lrConceptInstance.Y = lrFactTypeInstance.FactTypeReadingPoint.Y
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    Next

                    '--------------------------------------------------------------
                    'Establish the set of RoleConstraintInstances that are on the Page
                    '  as ConceptInstances.
                    '--------------------------------------------------------------
                    For Each lrRoleConstraintInstance In lrPage.RoleConstraintInstance

                        lrConceptInstance = lrRoleConstraintInstance.CloneConceptInstance
                        '-------------------------------------
                        'Add the ConceptInstance to the Page
                        '-------------------------------------
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    Next

                    '--------------------------------------------------------------
                    'Establish the set of FactInstances that are on the Page
                    '  as ConceptInstances.
                    '--------------------------------------------------------------
                    For Each lrFactInstance In lrPage.FactInstance
                        lrConceptInstance = lrFactInstance.CloneConceptInstance
                        '-------------------------------------
                        'Add the ConceptInstance to the Page
                        '-------------------------------------
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    Next

                    '--------------------------------------------------------------
                    'Establish the set of ValueInstances that are on the Page
                    '  as ConceptInstances.
                    '--------------------------------------------------------------
                    For Each lrFactDataInstance In lrPage.ValueInstance
                        lrConceptInstance = lrFactDataInstance.CloneConceptInstance
                        '-------------------------------------
                        'Add the ConceptInstance to the Page
                        '-------------------------------------
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    Next

                    '--------------------------------------------------------------
                    'Establish the set of RoleNameInstances that are on the Page
                    '  as ConceptInstances.
                    '--------------------------------------------------------------
                    For Each lrRoleInstance In lrPage.RoleInstance.FindAll(Function(x) x.Name <> "")
                        lrConceptInstance = New FBM.ConceptInstance

                        lrConceptInstance.PageId = lrPage.PageId
                        lrConceptInstance.ModelId = lrPage.Model.ModelId
                        lrConceptInstance.ConceptType = pcenumConceptType.RoleName
                        lrConceptInstance.RoleId = lrRoleInstance.Id
                        lrConceptInstance.Symbol = lrRoleInstance.Name
                        lrConceptInstance.X = lrRoleInstance.RoleName.X
                        lrConceptInstance.Y = lrRoleInstance.RoleName.Y
                        lrConceptInstance.Visible = True
                        lrConceptInstance.Orientation = 0
                        '-------------------------------------
                        'Add the ConceptInstance to the Page
                        '-------------------------------------
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    Next

                    '-----------------------------------------------
                    'Model Notes
                    '-----------------------------------------------
                    For Each lrModelNoteInstance In lrPage.ModelNoteInstance
                        lrConceptInstance = lrModelNoteInstance.CloneConceptInstance
                        '-------------------------------------
                        'Add the ConceptInstance to the Page
                        '-------------------------------------
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    Next

                    Me.ORMDiagram.Add(lrExportPage)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Maps an instance of this class to an instance of FBM.Model
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MapToFBMModel(Optional ByRef arModel As FBM.Model = Nothing) As FBM.Model

            Try

                Dim lsMessage As String = ""
                Dim lrModel As New FBM.Model
                If arModel IsNot Nothing Then
                    lrModel = arModel
                    lrModel.ModelId = Me.ORMModel.ModelId
                    lrModel.Name = Me.ORMModel.Name
                Else
                    lrModel = New FBM.Model(pcenumLanguage.ORMModel, Me.ORMModel.Name, Me.ORMModel.ModelId)
                End If

                'So that DictionaryEntries are dirty
                lrModel.Loaded = True

                lrModel.CoreVersionNumber = Me.ORMModel.CoreVersionNumber

                '==============================
                'Map the ValueTypes
                '==============================
                Dim lrXMLValueType As XMLModel.ValueType
                Dim lrValueType As FBM.ValueType
                Dim lsValueTypeConstraintValue As String

                For Each lrXMLValueType In Me.ORMModel.ValueTypes
                    lrValueType = New FBM.ValueType
                    lrValueType.Model = lrModel
                    lrValueType.Id = lrXMLValueType.Id
                    lrValueType.GUID = lrXMLValueType.GUID
                    lrValueType.Name = lrXMLValueType.Name
                    lrValueType.DataType = lrXMLValueType.DataType
                    lrValueType.DataTypePrecision = lrXMLValueType.DataTypePrecision
                    lrValueType.DataTypeLength = lrXMLValueType.DataTypeLength
                    lrValueType.IsMDAModelElement = lrXMLValueType.IsMDAModelElement

                    For Each lsValueTypeConstraintValue In lrXMLValueType.ValueConstraint
                        lrValueType.ValueConstraint.Add(lsValueTypeConstraintValue)
                        lrValueType.Instance.Add(lsValueTypeConstraintValue)
                    Next

                    '------------------------------------------------
                    'Link to the Concept within the ModelDictionary
                    '------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrValueType.Id, pcenumConceptType.ValueType, lrValueType.ShortDescription, lrValueType.LongDescription, True, True)
                    lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, ,,, True,, True)


                    lrValueType.Concept = lrDictionaryEntry.Concept

                    lrModel.ValueType.Add(lrValueType)
                Next

                '==============================
                'Map the EntityTypes
                '==============================
                Dim lrXMLEntityType As XMLModel.EntityType
                Dim lrEntityType As FBM.EntityType

                For Each lrXMLEntityType In Me.ORMModel.EntityTypes
                    lrEntityType = New FBM.EntityType
                    lrEntityType.Model = lrModel
                    lrEntityType.Id = lrXMLEntityType.Id
                    lrEntityType.GUID = lrXMLEntityType.GUID
                    lrEntityType.Name = lrXMLEntityType.Name
                    lrEntityType.ReferenceMode = lrXMLEntityType.ReferenceMode
                    lrEntityType.IsDerived = lrXMLEntityType.IsDerived
                    lrEntityType.DerivationText = lrXMLEntityType.DerivationText
                    'lrEntityType.ShortDescription = 
                    'lrEntityType.LongDescription = 
                    lrEntityType.IsObjectifyingEntityType = lrXMLEntityType.IsObjectifyingEntityType
                    lrEntityType.IsMDAModelElement = lrXMLEntityType.IsMDAModelElement
                    lrEntityType.IsAbsorbed = lrXMLEntityType.IsAbsorbed
                    lrEntityType.DBName = lrXMLEntityType.DBName

                    If lrXMLEntityType.ReferenceModeValueTypeId = "" Then
                        lrEntityType.ReferenceModeValueType = Nothing
                    Else
                        lrEntityType.ReferenceModeValueType = New FBM.ValueType
                        lrEntityType.ReferenceModeValueType.Id = lrXMLEntityType.ReferenceModeValueTypeId
                        lrEntityType.ReferenceModeValueType = lrModel.ValueType.Find(AddressOf lrEntityType.ReferenceModeValueType.Equals)
                    End If

                    lrEntityType.PreferredIdentifierRCId = lrXMLEntityType.ReferenceSchemeRoleConstraintId

                    '------------------------------------------------
                    'Link to the Concept within the ModelDictionary
                    '------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrEntityType.Name, pcenumConceptType.EntityType, , , True, True, lrEntityType.DBName)
                    lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , True,, True,, True)

                    lrEntityType.Concept = lrDictionaryEntry.Concept

                    lrModel.EntityType.Add(lrEntityType)
                Next

                '==============================
                'Map the FactTypes
                '==============================
                Dim lrXMLFactType As XMLModel.FactType
                Dim lrFactType As FBM.FactType

                For Each lrXMLFactType In Me.ORMModel.FactTypes
                    lrFactType = New FBM.FactType(lrModel,
                                                  lrXMLFactType.Name,
                                                  lrXMLFactType.Id)

                    Call Me.GetFactTypeDetails(lrFactType)
                Next

                '===============================================================================================
                'Populate Roles that are (still) joined to Nothing
                '===================================================
                Dim latType = {GetType(FBM.ValueType),
                               GetType(FBM.EntityType),
                               GetType(FBM.FactType)}

                Dim larRole = From Role In lrModel.Role
                              Where Role.JoinedORMObject Is Nothing _
                              Or Not latType.Contains(Role.JoinedORMObject.GetType)
                              Select Role

                For Each lrRole In larRole

                    Dim lrXMLRole = From FactType In Me.ORMModel.FactTypes
                                    From Role In FactType.RoleGroup
                                    Where Role.Id = lrRole.Id
                                    Select Role

                    lrRole.JoinedORMObject = New FBM.ModelObject
                    lrRole.JoinedORMObject.Id = lrXMLRole(0).JoinedObjectTypeId

                    If IsSomething(lrModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)) Then
                        lrRole.JoinedORMObject = lrModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    ElseIf IsSomething(lrModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)) Then
                        lrRole.JoinedORMObject = lrModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    Else
                        lrRole.JoinedORMObject = lrModel.FactType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    End If
                Next

                '==============================
                'Subtype Relationships
                '==============================
                Dim larSubtypeRelationshipFactTypes = From FactType In Me.ORMModel.FactTypes
                                                      Where FactType.IsSubtypeRelationshipFactType
                                                      Select FactType


                For Each lrXMLFactType In larSubtypeRelationshipFactTypes

                    lrFactType = New FBM.FactType(lrXMLFactType.Id, True)
                    lrFactType = lrModel.FactType.Find(AddressOf lrFactType.Equals)

                    Dim lrParentModelElement As FBM.ModelObject
                    lrEntityType = lrModel.EntityType.Find(Function(x) x.Id = lrFactType.RoleGroup(0).JoinedORMObject.Id)
                    lrParentModelElement = lrModel.GetModelObjectByName(lrFactType.RoleGroup(1).JoinedORMObject.Id)
                    If lrParentModelElement.GetType = GetType(FBM.FactType) Then
                        lrParentModelElement = CType(lrParentModelElement, FBM.FactType).ObjectifyingEntityType
                    End If
                    lrEntityType.parentModelObjectList.Add(lrParentModelElement)
                    lrParentModelElement.childModelObjectList.Add(lrEntityType)

                    Dim lrSubtypeConstraint As New FBM.tSubtypeRelationship(lrEntityType, lrParentModelElement, lrFactType)
                    lrEntityType.SubtypeRelationship.AddUnique(lrSubtypeConstraint)
                Next


                '==============================
                'Map the RoleConstraints
                '==============================
                Dim lrXMLRoleConstraint As XMLModel.RoleConstraint
                Dim lrRoleConstraint As FBM.RoleConstraint

                For Each lrXMLRoleConstraint In Me.ORMModel.RoleConstraints
                    lrRoleConstraint = New FBM.RoleConstraint

                    lrRoleConstraint.Id = lrXMLRoleConstraint.Id
                    lrRoleConstraint.GUID = lrXMLRoleConstraint.GUID
                    lrRoleConstraint.Model = lrModel
                    lrRoleConstraint.Name = lrXMLRoleConstraint.Name
                    'lrRoleConstraint.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                    'lrRoleConstraint.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
                    lrRoleConstraint.ConceptType = pcenumConceptType.RoleConstraint
                    lrRoleConstraint.RoleConstraintType = CType([Enum].Parse(GetType(pcenumRoleConstraintType), lrXMLRoleConstraint.RoleConstraintType), pcenumRoleConstraintType)
                    lrRoleConstraint.RingConstraintType = CType([Enum].Parse(GetType(pcenumRingConstraintType), lrXMLRoleConstraint.RingConstraintType), pcenumRingConstraintType)
                    'lrRoleConstraint.LevelNr = <See right down the bottom of this method>
                    lrRoleConstraint.IsPreferredIdentifier = lrXMLRoleConstraint.IsPreferredUniqueness
                    lrRoleConstraint.IsDeontic = lrXMLRoleConstraint.IsDeontic
                    lrRoleConstraint.Cardinality = lrXMLRoleConstraint.Cardinality
                    lrRoleConstraint.MaximumFrequencyCount = lrXMLRoleConstraint.MaximumFrequencyCount
                    lrRoleConstraint.MinimumFrequencyCount = lrXMLRoleConstraint.MinimumFrequencyCount
                    Select Case lrXMLRoleConstraint.CardinalityRangeType
                        Case Is = pcenumCardinalityRangeType.LessThanOrEqual.ToString
                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOrEqual
                        Case Is = pcenumCardinalityRangeType.Equal.ToString
                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Equal
                        Case Is = pcenumCardinalityRangeType.GreaterThanOrEqual.ToString
                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.GreaterThanOrEqual
                        Case Is = pcenumCardinalityRangeType.Between.ToString
                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Between
                    End Select
                    lrRoleConstraint.IsMDAModelElement = lrXMLRoleConstraint.IsMDAModelElement

                    '------------------------------------------------
                    'Link to the Concept within the ModelDictionary
                    '------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrRoleConstraint.Id, pcenumConceptType.RoleConstraint, lrRoleConstraint.ShortDescription, lrRoleConstraint.LongDescription, True, True)
                    lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , False,, True,, True)

                    If lrDictionaryEntry Is Nothing Then
                        lsMessage = "Cannot find DictionaryEntry in the ModelDictionary for RoleConstraint:"
                        lsMessage &= vbCrLf & "Model.Id: " & lrModel.ModelId
                        lsMessage &= vbCrLf & "RoleConstraint.Id: " & lrRoleConstraint.Id
                        Throw New Exception(lsMessage)
                    End If

                    lrRoleConstraint.Concept = lrDictionaryEntry.Concept

                    '----------------------------------------------------------
                    'Get the RoleConstraintRole list for the RoleConstraint.
                    '  NB the SequenceNr of a RoleConstraintRole is 'not' the same as a SequenceNr on a Role.
                    '  SequenceNr on a RoleConstraintRole is many things, but
                    '  relates particularly to DataIn, DataOut integrity matching.
                    '----------------------------------------------------------
                    Dim lrXMLRoleConstraintRole As XMLModel.RoleConstraintRole
                    Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                    Dim lrRole As New FBM.Role

                    For Each lrXMLRoleConstraintRole In lrXMLRoleConstraint.RoleConstraintRoles
                        lrRoleConstraintRole = New FBM.RoleConstraintRole
                        lrRoleConstraintRole.Model = lrModel
                        lrRoleConstraintRole.RoleConstraint = lrRoleConstraint
                        lrRoleConstraintRole.SequenceNr = lrXMLRoleConstraintRole.SequenceNr
                        lrRoleConstraintRole.IsEntry = lrXMLRoleConstraintRole.IsEntry
                        lrRoleConstraintRole.IsExit = lrXMLRoleConstraintRole.IsExit


                        lrRole.Id = lrXMLRoleConstraintRole.RoleId

                        lrRoleConstraintRole.Role = lrModel.Role.Find(AddressOf lrRole.Equals)

                        '-------------------------------------------------------------------
                        'lrRoleConstraintRole.RoleConstraintArgument is set further below.
                        '-------------------------------------------------------------------
                        lrRoleConstraintRole.ArgumentSequenceNr = lrXMLRoleConstraintRole.ArgumentSequenceNr

                        lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                        lrRoleConstraint.Role.Add(lrRoleConstraintRole.Role)
                        lrRoleConstraintRole.Role.RoleConstraintRole.Add(lrRoleConstraintRole)
                    Next

                    If lrRoleConstraint.RoleConstraintRole.Count = 0 Then
                        prApplication.ThrowErrorMessage("No RoleConstraintRoles found for RoleConstraint.Id: " & lrRoleConstraint.Id, pcenumErrorType.Information)
                    Else
                        lrFactType = lrRoleConstraint.Role(0).FactType
                        lrFactType = lrModel.FactType.Find(AddressOf lrFactType.Equals)

                        Select Case lrRoleConstraint.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                Call lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)
                        End Select
                    End If

                    Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
                    Dim lrXMLRoleConstraintArgument As XMLModel.RoleConstraintArgument
                    Dim lrJoinPath As FBM.JoinPath
                    Dim lrXMLRoleReference As XMLModel.RoleReference

                    For Each lrXMLRoleConstraintArgument In lrXMLRoleConstraint.Argument
                        lrRoleConstraintArgument = New FBM.RoleConstraintArgument(lrRoleConstraint,
                                                                                  lrXMLRoleConstraintArgument.SequenceNr,
                                                                                  lrXMLRoleConstraintArgument.Id)
                        lrRoleConstraintArgument.isDirty = True

                        lrJoinPath = New FBM.JoinPath(lrRoleConstraintArgument)
                        lrJoinPath.JoinPathError = lrXMLRoleConstraintArgument.JoinPath.JoinPathError

                        For Each lrXMLRoleReference In lrXMLRoleConstraintArgument.Role
                            lrRole = New FBM.Role
                            lrRole.Id = lrXMLRoleReference.RoleId
                            lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint)
                            lrRoleConstraintRole = lrRoleConstraint.RoleConstraintRole.Find(AddressOf lrRoleConstraintRole.EqualsByRole)
                            lrRoleConstraintArgument.RoleConstraintRole.Add(lrRoleConstraintRole)
                            lrRoleConstraintRole.RoleConstraintArgument = lrRoleConstraintArgument
                        Next

                        For Each lrXMLRoleReference In lrXMLRoleConstraintArgument.JoinPath.RolePath
                            lrRole = New FBM.Role
                            lrRole.Id = lrXMLRoleReference.RoleId
                            lrRole = lrModel.Role.Find(AddressOf lrRole.Equals)
                            lrJoinPath.RolePath.Add(lrRole)
                        Next
                        lrRoleConstraintArgument.JoinPath = lrJoinPath
                        Call lrRoleConstraintArgument.JoinPath.ConstructFactTypePath()
                        lrRoleConstraint.Argument.Add(lrRoleConstraintArgument)
                    Next

                    If (lrRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint) And (lrRoleConstraint.Role.Count > 0) Then
                        lrRoleConstraint.LevelNr = lrRoleConstraint.Role(0).FactType.InternalUniquenessConstraint.Count
                    End If

                    'CodeSafe-RoleConstraint must have Roles
                    If lrRoleConstraint.Role.Count > 0 Then
                        lrModel.RoleConstraint.Add(lrRoleConstraint)
                    End If

                    For Each lsValueTypeConstraintValue In lrXMLRoleConstraint.ValueConstraint
                        lrRoleConstraint.ValueConstraint.Add(lsValueTypeConstraintValue)
                    Next
                Next

                '-----------------------------------------------------------------------------
                'Set the ReferenceMode ObjectTypes for each of the EntityTypes in the Model
                '-----------------------------------------------------------------------------            
                For Each lrEntityType In lrModel.EntityType
                    Call lrEntityType.SetReferenceModeObjects()
                Next

                '==============================
                'Map the ModelNotes
                '==============================
                Dim lrXMLModelNote As XMLModel.ModelNote
                Dim lrModelNote As FBM.ModelNote

                For Each lrXMLModelNote In Me.ORMModel.ModelNotes
                    lrModelNote = New FBM.ModelNote(lrModel)

                    lrModelNote.Id = lrXMLModelNote.Id
                    lrModelNote.Text = lrXMLModelNote.Note
                    lrModelNote.IsMDAModelElement = lrXMLModelNote.IsMDAModelElement

                    If lrXMLModelNote.JoinedObjectTypeId = "" Then
                        lrModelNote.JoinedObjectType = Nothing
                    Else
                        lrModelNote.JoinedObjectType = lrModel.EntityType.Find(Function(x) x.Id = lrXMLModelNote.JoinedObjectTypeId)
                        If lrModelNote.JoinedObjectType Is Nothing Then
                            lrModelNote.JoinedObjectType = lrModel.FactType.Find(Function(x) x.Id = lrXMLModelNote.JoinedObjectTypeId)
                            If lrModelNote.JoinedObjectType Is Nothing Then
                                lrModelNote.JoinedObjectType = lrModel.ValueType.Find(Function(x) x.Id = lrXMLModelNote.JoinedObjectTypeId)
                            End If
                        End If
                    End If
                    lrModel.AddModelNote(lrModelNote, False)
                Next

                '----------------------------------------------------------------------------------------------
                'Reference any FactType.LinkFactTypeRole values that are NOTHING
                Dim larFactType = From [FactType] In lrModel.FactType
                                  Where FactType.LinkFactTypeRole Is Nothing _
                                  And FactType.IsLinkFactType = True
                                  Select FactType

                For Each lrFactType In larFactType
                    MsgBox("Error: FactType: '" & lrFactType.Name & "' has no LinkFactTypeRole.")
                Next

                '=====================
                'Map the Pages
                '=====================
                Call Me.MapToFBMPages(lrModel)

                lrModel.Loaded = True
                lrModel.LoadedFromXMLFile = True
                Return lrModel

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Sub MapToFBMPages(ByRef arModel As FBM.Model)

            Dim lrPage As FBM.Page
            'Dim lrXMLPage As XMLModel.Page
            Dim lrModel As FBM.Model = arModel
            Try
                For Each lrXMLPage In Me.ORMDiagram
                    lrPage = lrModel.Page.Find(Function(x) x.PageId = lrXMLPage.Id)
                    If lrPage Is Nothing Then
                        lrPage = Me.MapToFBMPage(lrXMLPage, lrModel)
                    Else
                        Call Me.MapToFBMPage(lrXMLPage, lrModel, lrPage)
                    End If

                    lrPage.Loaded = True
                    lrPage.IsDirty = True
                    lrModel.Page.AddUnique(lrPage)
                Next 'XMLModel.Page

                'Parallel.ForEach(Me.ORMDiagram,
                '                 Sub(lrXMLPage As XMLModel.Page)
                '                     lrPage = lrModel.Page.Find(Function(x) x.PageId = lrXMLPage.Id)
                '                     If lrPage Is Nothing Then
                '                         lrPage = Me.MapToFBMPage(lrXMLPage, lrModel)
                '                     Else
                '                         Call Me.MapToFBMPage(lrXMLPage, lrModel, lrPage)
                '                     End If

                '                     lrPage.Loaded = True
                '                     lrPage.IsDirty = True
                '                     lrModel.Page.AddUnique(lrPage)

                '                 End Sub)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Function MapToFBMPage(ByRef arXMLPage As XMLModel.Page,
                                      ByRef arModel As FBM.Model,
                                      Optional ByRef arPage As FBM.Page = Nothing) As FBM.Page

            Dim lsMessage As String

            Try
                Dim lrConceptInstance As FBM.ConceptInstance
                Dim lrPage As FBM.Page
                If arPage Is Nothing Then
                    lrPage = New FBM.Page(arModel,
                                          arXMLPage.Id,
                                          arXMLPage.Name,
                                          arXMLPage.Language)
                Else
                    lrPage = arPage
                End If

                '=============================
                'Map the ValueTypeInstances
                '=============================
                Dim lrValueTypeInstance As FBM.ValueTypeInstance

                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.ValueType)
                    lrValueTypeInstance = New FBM.ValueTypeInstance
                    lrValueTypeInstance.Model = arModel
                    lrValueTypeInstance.Page = lrPage
                    lrValueTypeInstance.Id = lrConceptInstance.Symbol
                    lrValueTypeInstance.ValueType.Id = lrValueTypeInstance.Id
                    lrValueTypeInstance.ValueType = arModel.ValueType.Find(AddressOf lrValueTypeInstance.ValueType.Equals)
                    lrValueTypeInstance.DataType = lrValueTypeInstance.ValueType.DataType
                    lrValueTypeInstance.DataTypeLength = lrValueTypeInstance.ValueType.DataTypeLength
                    lrValueTypeInstance.DataTypePrecision = lrValueTypeInstance.ValueType.DataTypePrecision

                    lrValueTypeInstance.ValueConstraint = lrValueTypeInstance.ValueType.ValueConstraint.Clone

                    lrValueTypeInstance.Name = lrConceptInstance.Symbol
                    lrValueTypeInstance.X = lrConceptInstance.X
                    lrValueTypeInstance.Y = lrConceptInstance.Y

                    lrPage.ValueTypeInstance.Add(lrValueTypeInstance)
                Next

                '=============================
                'Map the EntityTypeInstances
                '=============================
                Dim lrEntityTypeInstance As FBM.EntityTypeInstance

                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.EntityType)
                    lrEntityTypeInstance = New FBM.EntityTypeInstance
                    lrEntityTypeInstance.Model = arModel
                    lrEntityTypeInstance.Page = lrPage
                    lrEntityTypeInstance.Id = lrConceptInstance.Symbol
                    lrEntityTypeInstance.EntityType.Id = lrEntityTypeInstance.Id
                    lrEntityTypeInstance.EntityType = arModel.EntityType.Find(AddressOf lrEntityTypeInstance.EntityType.Equals)
                    lrEntityTypeInstance._Name = lrEntityTypeInstance.Id
                    lrEntityTypeInstance.ReferenceMode = lrEntityTypeInstance.EntityType.ReferenceMode
                    lrEntityTypeInstance.IsObjectifyingEntityType = lrEntityTypeInstance.EntityType.IsObjectifyingEntityType
                    lrEntityTypeInstance.IsAbsorbed = lrEntityTypeInstance.EntityType.IsAbsorbed
                    lrEntityTypeInstance.IsDerived = lrEntityTypeInstance.EntityType.IsDerived
                    lrEntityTypeInstance.DerivationText = lrEntityTypeInstance.EntityType.DerivationText
                    lrEntityTypeInstance.DBName = lrEntityTypeInstance.EntityType.DBName

                    If lrEntityTypeInstance.EntityType.ReferenceModeValueType Is Nothing Then
                        lrEntityTypeInstance.ReferenceModeValueType = Nothing
                    Else
                        lrEntityTypeInstance.ReferenceModeValueType = New FBM.ValueTypeInstance
                        lrEntityTypeInstance.ReferenceModeValueType.Id = lrEntityTypeInstance.EntityType.ReferenceModeValueType.Id
                        lrEntityTypeInstance.ReferenceModeValueType = lrPage.ValueTypeInstance.Find(AddressOf lrEntityTypeInstance.ReferenceModeValueType.Equals)
                    End If

                    lrEntityTypeInstance.PreferredIdentifierRCId = lrEntityTypeInstance.EntityType.PreferredIdentifierRCId

                    lrEntityTypeInstance.X = lrConceptInstance.X
                    lrEntityTypeInstance.Y = lrConceptInstance.Y

                    lrPage.EntityTypeInstance.Add(lrEntityTypeInstance)
                Next

                '===========================
                'Map the FactTypeInstances
                '===========================
                Dim lrFactTypeInstance As FBM.FactTypeInstance
                Dim lrDerivationTextConceptInstance As FBM.ConceptInstance
                Dim lrFactTypeReadingConceptInstance As FBM.ConceptInstance
                Dim lrFact As FBM.Fact
                Dim lrFactType As FBM.FactType

                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.FactType)
                    lrFactTypeInstance = New FBM.FactTypeInstance
                    lrFactType = arModel.FactType.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                    lrFactTypeInstance = lrFactType.CloneInstance(lrPage, True)
                    lrFactTypeInstance.X = lrConceptInstance.X
                    lrFactTypeInstance.Y = lrConceptInstance.Y
                    lrFactTypeInstance.DBName = lrFactType.DBName

                    If lrFactType.IsDerived Then
                        lrDerivationTextConceptInstance = arXMLPage.ConceptInstance.Find(Function(x) x.ConceptType = pcenumConceptType.DerivationText And x.Symbol = lrFactType.Id)
                        If lrDerivationTextConceptInstance IsNot Nothing Then
                            lrFactTypeInstance.FactTypeDerivationText = New FBM.FactTypeDerivationText(lrPage.Model,
                                                                                                           lrPage,
                                                                                                           lrFactTypeInstance)
                            lrFactTypeInstance.FactTypeDerivationText.X = lrDerivationTextConceptInstance.X
                            lrFactTypeInstance.FactTypeDerivationText.Y = lrDerivationTextConceptInstance.Y
                        End If
                    End If

                    lrFactTypeReadingConceptInstance = arXMLPage.ConceptInstance.Find(Function(x) x.ConceptType = pcenumConceptType.FactTypeReading And x.Symbol = lrFactType.Id)
                    If lrFactTypeReadingConceptInstance IsNot Nothing Then
                        lrFactTypeInstance.FactTypeReadingPoint = New Point(lrFactTypeReadingConceptInstance.X,
                                                                                lrFactTypeReadingConceptInstance.Y)
                    End If

                    '----------------------------------------
                    'Get the Facts for the FactTypeInstance
                    '----------------------------------------
                    Dim lrFactInstance As FBM.FactInstance
                    Dim lrFactDataInstance As FBM.FactDataInstance
                    For Each lrFact In lrFactType.Fact

                        If IsSomething(arXMLPage.ConceptInstance.Find(Function(x) x.Symbol = lrFact.Id And x.ConceptType = pcenumConceptType.Fact)) Then
                            '----------------------------------
                            'The Fact is included on the Page
                            '----------------------------------
                            lrFactInstance = lrFact.CloneInstance(lrPage)
                            lrFactInstance.isDirty = True
                            For Each lrFactDataInstance In lrFactInstance.Data
                                lrFactDataInstance.isDirty = True
                            Next
                            lrFactTypeInstance.Fact.Add(lrFactInstance)
                            lrFactTypeInstance.Page.FactInstance.Add(lrFactInstance)
                            lrFactTypeInstance.isDirty = True

                            For Each lrFactDataInstance In lrFactInstance.Data

                                Dim lrFactDataConceptInstance As New FBM.ConceptInstance
                                lrFactDataConceptInstance.Symbol = lrFactDataInstance.Data
                                lrFactDataConceptInstance.RoleId = lrFactDataInstance.Role.Id

                                lrFactDataConceptInstance = arXMLPage.ConceptInstance.Find(AddressOf lrFactDataConceptInstance.EqualsBySymbolRoleId)

                                If lrFactDataConceptInstance IsNot Nothing Then
                                    lrFactDataInstance.X = lrFactDataConceptInstance.X
                                    lrFactDataInstance.Y = lrFactDataConceptInstance.Y
                                End If
                                lrFactTypeInstance.Page.ValueInstance.AddUnique(lrFactDataInstance)
                            Next

                        End If

                    Next
                Next

                '===============================================================================================
                'Populate RoleInstances that are (still) joined to Nothing
                '===========================================================
#Region "Populate RoleInstances that are (still) joined to Nothing"
                Dim latType = {GetType(FBM.ValueTypeInstance),
                                   GetType(FBM.EntityTypeInstance),
                                   GetType(FBM.FactTypeInstance)}

                Dim larRole = From Role In lrPage.RoleInstance
                              Where Role.JoinedORMObject Is Nothing
                              Select Role

                Dim lrRoleInstance As FBM.RoleInstance

                For Each lrRoleInstance In larRole
                    Select Case lrRoleInstance.TypeOfJoin
                        Case Is = pcenumRoleJoinType.FactType
                            lrRoleInstance.JoinedORMObject = lrPage.FactTypeInstance.Find(Function(x) x.Id = lrRoleInstance.JoinsFactType.Id)
                    End Select

                Next
#End Region

                '=================================================================================
                'Map the SubtypeRelationships.
                '=================================================================================
#Region "SubtypeRelationships"
                Dim larSubtypeRelationshipFactTypes = From FactType In lrPage.FactTypeInstance
                                                      Where FactType.IsSubtypeRelationshipFactType
                                                      Select FactType

                Dim lrParentModelElementInstance As FBM.ModelObject
                Dim lrSubtypeConstraint As FBM.tSubtypeRelationship
                For Each lrFactTypeInstance In larSubtypeRelationshipFactTypes.ToArray
                    lrEntityTypeInstance = lrPage.EntityTypeInstance.Find(Function(x) x.Id = lrFactTypeInstance.RoleGroup(0).JoinedORMObject.Id)
                    lrParentModelElementInstance = lrPage.getModelElementById(lrFactTypeInstance.RoleGroup(1).JoinedORMObject.Id)

                    If lrParentModelElementInstance.ConceptType = pcenumConceptType.FactType Then
                        Dim lsParentModelElementInstanceId = CType(lrParentModelElementInstance, FBM.FactTypeInstance).FactType.ObjectifyingEntityType.Id
                        lrParentModelElementInstance = lrPage.EntityTypeInstance.Find(Function(x) x.Id = lsParentModelElementInstanceId)
                    End If
                    lrSubtypeConstraint = lrEntityTypeInstance.EntityType.SubtypeRelationship.Find(Function(x) x.EntityType.Id = lrEntityTypeInstance.EntityType.Id And x.parentEntityType.Id = lrParentModelElementInstance.Id)

                    Dim lrSubtypeConstraintInstance As FBM.SubtypeRelationshipInstance

                    lrSubtypeConstraintInstance = lrSubtypeConstraint.CloneInstance(lrPage, True)

                    lrEntityTypeInstance.SubtypeRelationship.Add(lrSubtypeConstraintInstance)
                Next
#End Region

                '===========================
                'Map the RoleNameInstances
                '===========================
                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleName)
                    lrRoleInstance = lrPage.RoleInstance.Find(Function(x) x.Id = lrConceptInstance.RoleId)
                    lrRoleInstance.RoleName = New FBM.RoleName(lrRoleInstance, lrRoleInstance.Name)
                    lrRoleInstance.RoleName.X = lrConceptInstance.X
                    lrRoleInstance.RoleName.Y = lrConceptInstance.Y
                Next

                '=================================
                'Map the RoleConstraintInstances
                '=================================
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                Dim lrRoleConstraint As FBM.RoleConstraint
                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleConstraint)

                    If IsSomething(lrPage.RoleConstraintInstance.Find(Function(x) x.Id = lrConceptInstance.Symbol)) Then
                        '-------------------------------------------------------------------
                        'The RoleConstraintInstance has already been added to the Page.
                        '  FactType.CloneInstance adds RoleConstraintInstances to the Page
                        '-------------------------------------------------------------------
                    Else
                        lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                        Try
                            Select Case lrRoleConstraint.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                    lrRoleConstraintInstance = lrRoleConstraint.CloneFrequencyConstraintInstance(lrPage)
                                Case Is = pcenumRoleConstraintType.RoleValueConstraint
                                    lrRoleConstraintInstance = lrRoleConstraint.CloneRoleValueConstraintInstance(lrPage)
                                Case Else
                                    lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)
                            End Select

                            lrRoleConstraintInstance.X = lrConceptInstance.X
                            lrRoleConstraintInstance.Y = lrConceptInstance.Y

                            lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                        Catch ex As Exception
                            lsMessage = "Error loading Role Constraint with Id: " & lrConceptInstance.Symbol
                            lsMessage.AppendDoubleLineBreak("Page: " & arXMLPage.Name)
                            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                        End Try

                    End If
                Next

                '============================
                'Map the ModelNoteInstances
                '============================
                Dim lrModelNoteInstance As FBM.ModelNoteInstance
                Dim lrModelNote As FBM.ModelNote
                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.ModelNote)
                    Try
                        lrModelNote = arModel.ModelNote.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                        lrModelNoteInstance = lrModelNote.CloneInstance(lrPage, True)
                        lrModelNoteInstance.X = lrConceptInstance.X
                        lrModelNoteInstance.Y = lrConceptInstance.Y
                    Catch ex As Exception
                        lsMessage = "Error loading Model Note with Id: " & lrConceptInstance.Symbol
                        lsMessage.AppendDoubleLineBreak("Page: " & arXMLPage.Name)
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                    End Try

                Next

                Return lrPage

            Catch ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try
        End Function

        Public Sub GetFactsForFactType(ByRef arFactType As FBM.FactType) 'As List(Of FBM.Fact)

            Dim lrDictionaryEntry As New FBM.DictionaryEntry
            Dim lrFact As New FBM.Fact
            Dim liInd As Integer = 0
            Dim lrRole As FBM.Role
            Dim lsMessage As String = ""

            Try

                Dim lsFactTypeId As String = arFactType.Id
                Dim lrXMLFact As XMLModel.Fact
                Dim lrXMLFactData As XMLModel.FactData
                Dim lrXMLFactType As XMLModel.FactType

                lrXMLFactType = Me.ORMModel.FactTypes.Find(Function(x) x.Id = lsFactTypeId)

                For Each lrXMLFact In lrXMLFactType.Facts

                    lrFact = New FBM.Fact(lrXMLFact.Id, arFactType)
                    lrFact.FactType.isDirty = True
                    lrFact.isDirty = True
                    lrDictionaryEntry = New FBM.DictionaryEntry(arFactType.Model,
                                                                lrFact.Id,
                                                                pcenumConceptType.Fact,
                                                                ,
                                                                , True
                                                                )

                    lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry)

                    For Each lrXMLFactData In lrXMLFact.Data
                        lrRole = New FBM.Role
                        lrRole.Id = lrXMLFactData.RoleId
                        lrRole = arFactType.RoleGroup.Find(AddressOf lrRole.Equals)

                        '--------------------------------------------------------------------------------------------------
                        'Get the Concept from the ModelDictionary so that FactData objects are linked directly to the Concept/Value in the ModelDictionary
                        '--------------------------------------------------------------------------------------------------
                        lrDictionaryEntry = New FBM.DictionaryEntry(arFactType.Model,
                                                                    lrXMLFactData.Data,
                                                                    pcenumConceptType.Value,
                                                                    ,
                                                                    , True
                                                                    )

                        lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry)

                        Dim lrConcept As FBM.Concept = lrDictionaryEntry.Concept

                        Dim lrFactData As New FBM.FactData(lrRole, lrConcept, lrFact)
                        lrFactData.isDirty = True
                        lrFactData.Model = arFactType.Model
                        '-----------------------------
                        'Add the FactData to the Fact
                        '-----------------------------
                        lrFact.Data.Add(lrFactData)
                        '-------------------------------------
                        'Add the FactData to the Role as well
                        '-------------------------------------
                        lrRole.Data.Add(lrFactData)

                        If Not lrRole.JoinedORMObject.Instance.Exists(Function(x) x = lrFactData.Data) Then
                            lrRole.JoinedORMObject.Instance.Add(lrFactData.Data)
                        End If
                    Next

                    '----------------------------------------------------------------------------------------------------------
                    'If the FactType of the Fact is Objectified, add the Fact.Id as an instance of the ObjectifyingEntityType
                    '----------------------------------------------------------------------------------------------------------
                    If arFactType.IsObjectified Then
                        If IsSomething(arFactType.ObjectifyingEntityType) Then
                            arFactType.ObjectifyingEntityType.Instance.Add(lrFact.Id)
                        End If
                    End If

                    arFactType.Fact.Add(lrFact)
                Next

            Catch ex As Exception
                Dim lsMessage2 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage2 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage2 &= vbCrLf & vbCrLf & ex.Message
                lsMessage2 &= vbCrLf & vbCrLf & "Loading Facts for FactType: '" & arFactType.Id & "'"
                prApplication.ThrowErrorMessage(lsMessage2, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        ''' <summary>
        ''' Gets the details of a FBM.FactType from the XMLModel.FactType in the Model (me)
        ''' </summary>
        ''' <param name="arFactType">The FactType for which the details are being retrieved</param>
        ''' <remarks></remarks>
        Public Sub GetFactTypeDetails(ByRef arFactType As FBM.FactType)

            Dim lsMessage As String = ""

            Try
                Dim lsFactTypeId As String = arFactType.Id
                Dim lrXMLRole As XMLModel.Role
                Dim lrRole As FBM.Role
                Dim lrXMLFactType As XMLModel.FactType

                lrXMLFactType = Me.ORMModel.FactTypes.Find(Function(x) x.Id = lsFactTypeId)

                'arFactType.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                'arFactType.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
                arFactType.IsObjectified = lrXMLFactType.IsObjectified
                'arFactType.IsCoreFactType = CBool(lREcordset("IsCoreFactType").Value)
                arFactType.IsPreferredReferenceMode = lrXMLFactType.IsPreferredReferenceSchemeFT
                arFactType.IsSubtypeRelationshipFactType = lrXMLFactType.IsSubtypeRelationshipFactType
                arFactType.IsDerived = lrXMLFactType.IsDerived
                arFactType.IsStored = lrXMLFactType.IsStored
                arFactType.DerivationText = lrXMLFactType.DerivationText
                arFactType.IsLinkFactType = lrXMLFactType.IsLinkFactType
                arFactType.LinkFactTypeRole = arFactType.Model.Role.Find(Function(x) x.Id = lrXMLFactType.LinkFactTypeRoleId)
                arFactType.IsMDAModelElement = lrXMLFactType.IsMDAModelElement
                arFactType.IsSubtypeStateControlling = lrXMLFactType.IsSubtypeStateControlling
                arFactType.StoreFactCoordinates = lrXMLFactType.StoreFactCoordinates
                arFactType.DBName = lrXMLFactType.DBName

                If lrXMLFactType.ObjectifyingEntityTypeId = "" Then
                    arFactType.ObjectifyingEntityType = Nothing
                Else
                    Dim lsEntityTypeId As String = ""
                    lsEntityTypeId = lrXMLFactType.ObjectifyingEntityTypeId
                    arFactType.ObjectifyingEntityType = New FBM.EntityType(arFactType.Model, pcenumLanguage.ORMModel, lsEntityTypeId, Nothing, True)
                    arFactType.ObjectifyingEntityType = arFactType.Model.EntityType.Find(AddressOf arFactType.ObjectifyingEntityType.Equals)
                    arFactType.ObjectifyingEntityType.IsObjectifyingEntityType = True
                    arFactType.ObjectifyingEntityType.ObjectifiedFactType = New FBM.FactType
                    arFactType.ObjectifyingEntityType.ObjectifiedFactType = arFactType

                    If IsSomething(arFactType.ObjectifyingEntityType) Then
                        '---------------------------------------------
                        'Okay, have found the ObjectifyingEntityType
                        '---------------------------------------------
                    Else
                        lsMessage = "No EntityType found in the Model for Objectifying Entity Type of the FactType"
                        lsMessage &= vbCrLf & "ModelId: " & arFactType.Model.ModelId
                        lsMessage &= vbCrLf & "FactTypeId: " & arFactType.Id
                        lsMessage &= vbCrLf & "Looking for EntityTypeId: " & lsEntityTypeId
                        Throw New Exception(lsMessage)
                    End If
                End If

                '-----------------------------------------------------
                'Get the Roles within the RoleGroup for the FactType
                '-----------------------------------------------------
                For Each lrXMLRole In lrXMLFactType.RoleGroup

                    lrRole = New FBM.Role
                    lrRole.Model = arFactType.Model
                    lrRole.FactType = arFactType
                    lrRole.Id = lrXMLRole.Id
                    lrRole.Name = lrXMLRole.Name
                    lrRole.SequenceNr = lrXMLRole.SequenceNr
                    lrRole.Mandatory = lrXMLRole.Mandatory
                    lrRole.JoinedORMObject = New FBM.ModelObject
                    lrRole.JoinedORMObject.Id = lrXMLRole.JoinedObjectTypeId

                    If IsSomething(arFactType.Model.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)) Then
                        lrRole.JoinedORMObject = arFactType.Model.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    ElseIf IsSomething(arFactType.Model.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)) Then
                        lrRole.JoinedORMObject = arFactType.Model.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    Else
                        lrRole.JoinedORMObject = arFactType.Model.FactType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                        If lrRole.JoinedORMObject Is Nothing Then
                            lrRole.JoinedORMObject = New FBM.FactType(lrRole.Model, lrXMLRole.JoinedObjectTypeId, True)
                            Me.GetFactTypeDetails(lrRole.JoinsFactType)
                        End If
                    End If

                    '--------------------------------------------------
                    'Add the Role to the Model (list of Role) as well
                    '--------------------------------------------------
                    arFactType.Model.Role.Add(lrRole)

                    arFactType.RoleGroup.Add(lrRole)
                Next

                '-------------------------------------------
                'Get the FactTypeReadings for the FactType
                '-------------------------------------------
                arFactType.FactTypeReading = Me.GetFactTypeReadingsForFactType(arFactType)

                '-------------------------------------------
                'Get the Facts (FactData) for the FactType
                '-------------------------------------------
                Call Me.GetFactsForFactType(arFactType)

                '------------------------------------------------
                'Link to the Concept within the ModelDictionary
                '------------------------------------------------
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(arFactType.Model,
                                                                 arFactType.Id,
                                                                 pcenumConceptType.FactType,
                                                                 arFactType.ShortDescription,
                                                                 arFactType.LongDescription,
                                                                 True,
                                                                 True,
                                                                 arFactType.DBName)

                lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry, ,,, False,, True) '20220117-VM-Was this. DBName wasn't working.

                arFactType.Concept = lrDictionaryEntry.Concept

                Call arFactType.Model.AddFactType(arFactType, False, False, Nothing)

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function GetFactTypeReadingsForFactType(ByRef arFactType As FBM.FactType) As List(Of FBM.FactTypeReading)

            Dim lrXMLFactType As XMLModel.FactType
            Dim lrXMLFactTypeReading As XMLModel.FactTypeReading
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lsMessage As String = ""

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetFactTypeReadingsForFactType = New List(Of FBM.FactTypeReading)

            Try

                lrXMLFactType = New XMLModel.FactType
                lrXMLFactType.Id = arFactType.Id
                lrXMLFactType = Me.ORMModel.FactTypes.Find(AddressOf lrXMLFactType.Equals)

                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart
                Dim lrXMLPredicatePart As XMLModel.PredicatePart

                For Each lrXMLFactTypeReading In lrXMLFactType.FactTypeReadings

                    lrFactTypeReading = New FBM.FactTypeReading
                    lrFactTypeReading.Model = arFactType.Model
                    lrFactTypeReading.FactType = arFactType
                    lrFactTypeReading.Id = lrXMLFactTypeReading.Id
                    lrFactTypeReading.FrontText = lrXMLFactTypeReading.FrontReadingText
                    lrFactTypeReading.FollowingText = lrXMLFactTypeReading.FollowingReadingText

                    '--------------------------------------------------
                    'Get the PredicateParts within the FactTypeReading
                    '--------------------------------------------------
                    Dim lrRole As FBM.Role
                    For Each lrXMLPredicatePart In lrXMLFactTypeReading.PredicateParts

                        lrPredicatePart = New FBM.PredicatePart(arFactType.Model, lrFactTypeReading, Nothing, True)
                        lrPredicatePart.SequenceNr = lrXMLPredicatePart.SequenceNr
                        lrPredicatePart.PreBoundText = lrXMLPredicatePart.PreboundReadingText
                        lrPredicatePart.PostBoundText = lrXMLPredicatePart.PostboundReadingText
                        lrPredicatePart.PredicatePartText = Trim(lrXMLPredicatePart.PredicatePartText)

                        lrRole = New FBM.Role
                        lrRole.Id = lrXMLPredicatePart.Role_Id
                        lrRole = arFactType.Model.Role.Find(AddressOf lrRole.Equals)
                        lrPredicatePart.Role = lrRole

                        lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                    Next

                    If Not IsSomething(lrFactTypeReading.PredicatePart) Then
                        lsMessage = "Error: TableFactTypeReading.GetFactTypeReadingsForFactType: "
                        lsMessage &= vbCrLf & "No PredicateParts found for:"
                        lsMessage &= vbCrLf & "FactType.Id: '" & arFactType.Id & "'"
                        lsMessage &= vbCrLf & "FactTypeReading.Id: '" & lrFactTypeReading.Id & "'"
                        Throw New Exception(lsMessage)
                    End If

                    GetFactTypeReadingsForFactType.Add(lrFactTypeReading)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

    End Class

End Namespace
