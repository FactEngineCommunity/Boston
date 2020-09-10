Imports System.Xml.Serialization
Imports System.Reflection


Namespace XMLModel14

    ''' <summary>
    ''' v1.3 Adds the GUID field to the ValueType, FactType and RoleConstraint classes.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Model

        <XmlAttribute()> _
        Public XSDVersionNr As Double = 1.4
        Public ORMModel As New XMLModel14.ORMModel
        Public ORMDiagram As New List(Of XMLModel14.Page)

        ''' <summary>
        ''' Maps an instance of FBM.Model to this class.
        ''' </summary>
        ''' <param name="arFBMModel">The model being mapped.</param>
        ''' <remarks></remarks>
        Public Sub MapFromFBMModel(ByVal arFBMModel As FBM.Model)

            Me.ORMModel = New XMLModel14.ORMModel
            Me.ORMModel.ModelId = arFBMModel.ModelId
            Me.ORMModel.Name = arFBMModel.Name

            '========================
            'Process the ValueTypes
            '========================
            Dim lrValueType As FBM.ValueType
            Dim lrXMLValueType As XMLModel14.ValueType
            For Each lrValueType In arFBMModel.ValueType

                lrXMLValueType = New XMLModel14.ValueType
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
            Dim lrXMLEntityType As XMLModel14.EntityType

            For Each lrEntityType In arFBMModel.EntityType

                lrXMLEntityType = New XMLModel14.EntityType

                lrXMLEntityType.Id = lrEntityType.Id
                lrXMLEntityType.GUID = lrEntityType.GUID
                lrXMLEntityType.Name = lrEntityType.Name
                lrXMLEntityType.IsObjectifyingEntityType = lrEntityType.IsObjectifyingEntityType
                lrXMLEntityType.Instance = lrEntityType.Instance
                lrXMLEntityType.ReferenceMode = lrEntityType.ReferenceMode
                lrXMLEntityType.IsIndependent = lrEntityType.IsIndependent
                lrXMLEntityType.IsAbsorbed = lrEntityType.IsAbsorbed
                lrXMLEntityType.IsPersonal = lrEntityType.IsPersonal
                lrXMLEntityType.LongDescription = lrEntityType.LongDescription
                lrXMLEntityType.ShortDescription = lrEntityType.ShortDescription

                If IsSomething(lrEntityType.ReferenceModeRoleConstraint) Then
                    lrXMLEntityType.ReferenceSchemeRoleConstraintId = lrEntityType.ReferenceModeRoleConstraint.Id
                End If

                If IsSomething(lrEntityType.ReferenceModeValueType) Then
                    lrXMLEntityType.ReferenceModeValueTypeId = lrEntityType.ReferenceModeValueType.Id
                End If

                Dim lrSubtypeRelationship As FBM.tSubtypeRelationship
                Dim lrXMLSubtypeRelationship As XMLModel14.SubtypeRelationship

                For Each lrSubtypeRelationship In lrEntityType.SubtypeRelationship

                    lrXMLSubtypeRelationship = New XMLModel14.SubtypeRelationship
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
            Dim lrXMLFactType As XMLModel14.FactType

            For Each lrFactType In arFBMModel.FactType

                lrXMLFactType = New XMLModel14.FactType

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

                '---------------
                'Map the Roles
                '---------------
                Dim lrRole As FBM.Role
                Dim lrXMLRole As XMLModel14.Role
                For Each lrRole In lrFactType.RoleGroup

                    lrXMLRole = New XMLModel14.Role

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
                Dim lrXMLFact As XMLModel14.Fact
                Dim lrFactData As FBM.FactData
                Dim lrXMLFactData As XMLModel14.FactData

                For Each lrFact In lrFactType.Fact

                    lrXMLFact = New XMLModel14.Fact

                    lrXMLFact.Id = lrFact.Id

                    '------------------
                    'Map the FactData
                    '------------------
                    For Each lrFactData In lrFact.Data
                        lrXMLFactData = New XMLModel14.FactData

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
                Dim lrXMLFactTypeReading As XMLModel14.FactTypeReading
                Dim lrPredicatePart As FBM.PredicatePart
                Dim lrXMLPredicatePart As XMLModel14.PredicatePart

                For Each lrFactTypeReading In lrFactType.FactTypeReading

                    lrXMLFactTypeReading = New XMLModel14.FactTypeReading

                    lrXMLFactTypeReading.Id = lrFactTypeReading.Id
                    lrXMLFactTypeReading.FrontReadingText = lrFactTypeReading.FrontText
                    lrXMLFactTypeReading.FollowingReadingText = lrFactTypeReading.FollowingText

                    For Each lrPredicatePart In lrFactTypeReading.PredicatePart

                        lrXMLPredicatePart = New XMLModel14.PredicatePart

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

            '=========================
            'Map the RoleConstraints
            '=========================
            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrXMLRoleConstraint As XMLModel14.RoleConstraint
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
            Dim lrXMLRoleConstraintRole As XMLModel14.RoleConstraintRole
            Dim lrXMLRoleConstraintArgument As XMLModel14.RoleConstraintArgument

            For Each lrRoleConstraint In arFBMModel.RoleConstraint

                lrXMLRoleConstraint = New XMLModel14.RoleConstraint

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

                For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole

                    lrXMLRoleConstraintRole = New XMLModel14.RoleConstraintRole

                    lrXMLRoleConstraintRole.RoleId = lrRoleConstraintRole.Role.Id
                    lrXMLRoleConstraintRole.SequenceNr = lrRoleConstraintRole.SequenceNr
                    lrXMLRoleConstraintRole.IsEntry = lrRoleConstraintRole.IsEntry
                    lrXMLRoleConstraintRole.IsExit = lrRoleConstraintRole.IsExit

                    lrXMLRoleConstraint.RoleConstraintRoles.Add(lrXMLRoleConstraintRole)
                Next

                '----------------------------------------
                'Construct the RoleConstraintArguments.
                '----------------------------------------
                Dim lrXMLRoleReference As XMLModel14.RoleReference
                Dim lrRole As FBM.Role
                For Each lrRoleConstraintArgument In lrRoleConstraint.Argument
                    lrXMLRoleConstraintArgument = New XMLModel14.RoleConstraintArgument
                    lrXMLRoleConstraintArgument.SequenceNr = lrRoleConstraintArgument.SequenceNr

                    For Each lrRoleConstraintRole In lrRoleConstraintArgument.RoleConstraintRole
                        lrXMLRoleReference = New XMLModel14.RoleReference
                        lrXMLRoleReference.RoleId = lrRoleConstraintRole.Role.Id
                        lrXMLRoleConstraintArgument.Role.Add(lrXMLRoleReference)
                    Next

                    lrXMLRoleConstraintArgument.JoinPath = New XMLModel14.JoinPath
                    lrXMLRoleConstraintArgument.JoinPath.JoinPathError = lrRoleConstraintArgument.JoinPath.JoinPathError
                    For Each lrRole In lrRoleConstraintArgument.JoinPath.RolePath
                        lrXMLRoleReference = New XMLModel14.RoleReference
                        lrXMLRoleReference.RoleId = lrRole.Id
                        lrXMLRoleConstraintArgument.JoinPath.RolePath.Add(lrXMLRoleReference)
                    Next

                    lrXMLRoleConstraint.Argument.Add(lrXMLRoleConstraintArgument)
                Next

                Me.ORMModel.RoleConstraints.Add(lrXMLRoleConstraint)
            Next

            '================
            'Map ModelNotes
            '================
            Dim lrModelNote As FBM.ModelNote
            Dim lrXMLModelNote As XMLModel14.ModelNote

            For Each lrModelNote In arFBMModel.ModelNote

                lrXMLModelNote = New XMLModel14.ModelNote

                lrXMLModelNote.Id = lrModelNote.Id
                lrXMLModelNote.Note = lrModelNote.LongDescription

                Me.ORMModel.ModelNotes.Add(lrXMLModelNote)

            Next

            '================================
            'Map the Pages
            '================================
            Dim lrPage As FBM.Page

            For Each lrPage In arFBMModel.Page

                Dim lrExportPage As New XMLModel14.Page

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

                Me.ORMDiagram.Add(lrExportPage)
            Next

        End Sub

        ''' <summary>
        ''' Maps an instance of this class to an instance of FBM.Model
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MapToFBMModel() As FBM.Model

            Dim lsMessage As String = ""

            Dim lrModel As New FBM.Model(pcenumLanguage.ORMModel, Me.ORMModel.Name, Me.ORMModel.ModelId)

            '==============================
            'Map the ValueTypes
            '==============================
            Dim lrXMLValueType As XMLModel14.ValueType
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

                For Each lsValueTypeConstraintValue In lrXMLValueType.ValueConstraint
                    lrValueType.ValueConstraint.Add(lsValueTypeConstraintValue)
                    lrValueType.Instance.Add(lsValueTypeConstraintValue)
                Next

                '------------------------------------------------
                'Link to the Concept within the ModelDictionary
                '------------------------------------------------
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrValueType.Id, pcenumConceptType.ValueType, lrValueType.ShortDescription, lrValueType.LongDescription, True)
                lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry)


                lrValueType.Concept = lrDictionaryEntry.Concept

                lrModel.ValueType.Add(lrValueType)
            Next

            '==============================
            'Map the EntityTypes
            '==============================
            Dim lrXMLEntityType As XMLModel14.EntityType
            Dim lrEntityType As FBM.EntityType

            For Each lrXMLEntityType In Me.ORMModel.EntityTypes
                lrEntityType = New FBM.EntityType
                lrEntityType.Model = lrModel
                lrEntityType.Id = lrXMLEntityType.Id
                lrEntityType.GUID = lrXMLEntityType.GUID
                lrEntityType.Name = lrXMLEntityType.Name
                lrEntityType.ReferenceMode = lrXMLEntityType.ReferenceMode
                'lrEntityType.ShortDescription = 
                'lrEntityType.LongDescription = 
                lrEntityType.IsObjectifyingEntityType = lrXMLEntityType.IsObjectifyingEntityType

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
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrEntityType.Name, pcenumConceptType.EntityType, , , True)
                lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , False)

                lrEntityType.Concept = lrDictionaryEntry.Concept

                lrModel.EntityType.Add(lrEntityType)
            Next

            '==============================
            'Map the FactTypes
            '==============================
            Dim lrXMLFactType As XMLModel14.FactType
            Dim lrFactType As FBM.FactType

            For Each lrXMLFactType In Me.ORMModel.FactTypes
                lrFactType = New FBM.FactType(lrModel, _
                                              lrXMLFactType.Name, _
                                              lrXMLFactType.Id)

                Call Me.GetFactTypeDetails(lrFactType)

                '-------------------------------------------
                'Get the FactTypeReadings for the FactType
                '-------------------------------------------
                lrFactType.FactTypeReading = Me.GetFactTypeReadingsForFactType(lrFactType)

                '-------------------------------------------
                'Get the Facts (FactData) for the FactType
                '-------------------------------------------
                Call Me.GetFactsForFactType(lrFactType)

                '------------------------------------------------
                'Link to the Concept within the ModelDictionary
                '------------------------------------------------
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrFactType.Id, pcenumConceptType.FactType, , , True)
                lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , False)

                lrFactType.Concept = lrDictionaryEntry.Concept

                If lrModel.FactType.Find(AddressOf lrFactType.Equals) Is Nothing Then
                    lrModel.FactType.Add(lrFactType)
                End If

            Next

            '===============================================================================================
            'Populate Roles that are (still) joined to Nothing
            '===================================================
            Dim latType = {GetType(FBM.ValueType), _
                           GetType(FBM.EntityType), _
                           GetType(FBM.FactType)}

            Dim larRole = From Role In lrModel.Role _
                           Where Role.JoinedORMObject Is Nothing _
                           Or Not latType.Contains(Role.JoinedORMObject.GetType)
                           Select Role

            For Each lrRole In larRole

                Dim lrXMLRole = From FactType In Me.ORMModel.FactTypes _
                            From Role In FactType.RoleGroup _
                            Where Role.Id = lrRole.Id _
                            Select Role

                lrRole.JoinedORMObject = New FBM.ModelObject
                lrRole.JoinedORMObject.Id = lrXMLRole(0).JoinedObjectTypeId

                If IsSomething(lrModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)) Then
                    'lrRole.JoinsEntityType = New FBM.EntityType
                    'lrRole.JoinsEntityType = arFactType.Model.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    lrRole.JoinedORMObject = lrModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    'lrRole.TypeOfJoin = pcenumRoleJoinType.EntityType
                ElseIf IsSomething(lrModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)) Then
                    'lrRole.JoinsValueType = New FBM.ValueType
                    'lrRole.JoinsValueType = arFactType.Model.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    lrRole.JoinedORMObject = lrModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    'lrRole.TypeOfJoin = pcenumRoleJoinType.ValueType
                Else
                    'lrRole.JoinsFactType = New FBM.FactType
                    lrRole.JoinedORMObject = lrModel.FactType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    If lrRole.JoinedORMObject Is Nothing Then
                        lrRole.JoinedORMObject = New FBM.FactType(lrRole.Model, lrXMLRole(0).JoinedObjectTypeId, True)
                        Me.GetFactTypeDetails(lrRole.JoinsFactType)
                    End If
                    'lrRole.JoinedORMObject = arFactType.Model.FactType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    'lrRole.TypeOfJoin = pcenumRoleJoinType.FactType
                End If

            Next

            '==============================
            'Subtype Relationships
            '==============================
            Dim larSubtypeRelationshipFactTypes = From FactType In Me.ORMModel.FactTypes _
                                                  Where FactType.IsSubtypeRelationshipFactType _
                                                  Select FactType


            For Each lrXMLFactType In larSubtypeRelationshipFactTypes

                lrFactType = New FBM.FactType(lrXMLFactType.Id, True)
                lrFactType = lrModel.FactType.Find(AddressOf lrFactType.Equals)

                Dim lrParentEntityType As FBM.EntityType
                lrEntityType = New FBM.EntityType
                lrEntityType.Model = lrModel
                lrEntityType.Id = lrFactType.RoleGroup(0).JoinedORMObject.Id
                lrEntityType = lrModel.EntityType.Find(AddressOf lrEntityType.Equals)
                lrParentEntityType = New FBM.EntityType
                lrParentEntityType.Id = lrFactType.RoleGroup(1).JoinedORMObject.Id
                lrParentEntityType = lrModel.EntityType.Find(AddressOf lrParentEntityType.Equals)
                lrEntityType.parentModelObjectList.Add(lrParentEntityType)
                lrParentEntityType.childModelObjectList.Add(lrEntityType)

                Dim lrSubtypeConstraint As New FBM.tSubtypeRelationship(lrEntityType, lrParentEntityType, lrFactType)
                lrEntityType.SubtypeRelationship.Add(lrSubtypeConstraint)
            Next


            '==============================
            'Map the RoleConstraints
            '==============================
            Dim lrXMLRoleConstraint As XMLModel14.RoleConstraint
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
                    Case Is = pcenumCardinalityRangeType.LessThanOREqual.ToString
                        lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOREqual
                    Case Is = pcenumCardinalityRangeType.Equal.ToString
                        lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Equal
                    Case Is = pcenumCardinalityRangeType.GreaterThanOREqual.ToString
                        lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.GreaterThanOREqual
                    Case Is = pcenumCardinalityRangeType.Between.ToString
                        lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Between
                End Select

                '------------------------------------------------
                'Link to the Concept within the ModelDictionary
                '------------------------------------------------
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrRoleConstraint.Id, pcenumConceptType.RoleConstraint, lrRoleConstraint.ShortDescription, lrRoleConstraint.LongDescription, True)
                lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , False)

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
                Dim lrXMLRoleConstraintRole As XMLModel14.RoleConstraintRole
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
                Dim lrXMLRoleConstraintArgument As XMLModel14.RoleConstraintArgument
                Dim lrJoinPath As FBM.JoinPath
                Dim lrXMLRoleReference As XMLModel14.RoleReference

                For Each lrXMLRoleConstraintArgument In lrXMLRoleConstraint.Argument
                    lrRoleConstraintArgument = New FBM.RoleConstraintArgument(lrRoleConstraint, _
                                                                              lrXMLRoleConstraintArgument.SequenceNr, _
                                                                              lrXMLRoleConstraintArgument.Id)

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
                    lrRoleConstraint.Argument.Add(lrRoleConstraintArgument)
                Next

                If lrRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    lrRoleConstraint.LevelNr = lrRoleConstraint.Role(0).FactType.InternalUniquenessConstraint.Count
                End If

                lrModel.RoleConstraint.Add(lrRoleConstraint)
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
            Dim lrXMLModelNote As XMLModel14.ModelNote
            Dim lrModelNote As FBM.ModelNote

            For Each lrXMLModelNote In Me.ORMModel.ModelNotes
                lrModelNote = New FBM.ModelNote

                lrModelNote.Model = lrModel
                lrModelNote.Id = lrXMLModelNote.Id
                'lrModelNote.ShortDescription = Trim(Viev.NullVal(lRecordset("ShortDescription").Value, ""))
                'lrModelNote.LongDescription = Trim(Viev.NullVal(lRecordset("LongDescription").Value, ""))
                lrModelNote.ConceptType = pcenumConceptType.ModelNote
                lrModelNote.Text = lrXMLModelNote.Note


                If lrXMLModelNote.JoinedObjectTypeId = "" Then
                    lrModelNote.JoinedObjectType = Nothing
                Else
                    lrModelNote.JoinedObjectType = New FBM.ModelObject
                    lrModelNote.JoinedObjectType.Id = lrXMLModelNote.JoinedObjectTypeId
                    If IsSomething(lrModel.EntityType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)) Then
                        lrModelNote.JoinedObjectType = lrModel.EntityType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)
                    ElseIf IsSomething(lrModel.ValueType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)) Then
                        lrModelNote.JoinedObjectType = lrModel.ValueType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)
                    ElseIf IsSomething(lrModel.FactType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)) Then
                        lrModelNote.JoinedObjectType = lrModel.FactType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)
                    End If
                End If

                lrModel.ModelNote.Add(lrModelNote)
            Next

            '=====================
            'Map the Pages
            '=====================
            Call Me.MapToFBMPages(lrModel)

            lrModel.Loaded = True
            lrModel.LoadedFromXMLFile = True
            Return lrModel

        End Function

        Public Sub MapToFBMPages(ByRef arModel As FBM.Model)

            Dim lrPage As FBM.Page
            Dim lrXMLPage As XMLModel14.Page
            Dim lrConceptInstance As FBM.ConceptInstance

            For Each lrXMLPage In Me.ORMDiagram

                lrPage = New FBM.Page(arModel, _
                                      lrXMLPage.Id, _
                                      lrXMLPage.Name, _
                                      lrXMLPage.Language)

                '=============================
                'Map the ValueTypeInstances
                '=============================
                Dim lrValueTypeInstance As FBM.ValueTypeInstance

                For Each lrConceptInstance In lrXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.ValueType)
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

                For Each lrConceptInstance In lrXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.EntityType)
                    lrEntityTypeInstance = New FBM.EntityTypeInstance
                    lrEntityTypeInstance.Model = arModel
                    lrEntityTypeInstance.Page = lrPage
                    lrEntityTypeInstance.Id = lrConceptInstance.Symbol
                    lrEntityTypeInstance.EntityType.Id = lrEntityTypeInstance.Id
                    lrEntityTypeInstance.EntityType = arModel.EntityType.Find(AddressOf lrEntityTypeInstance.EntityType.Equals)
                    lrEntityTypeInstance._Name = lrEntityTypeInstance.Id
                    lrEntityTypeInstance.ReferenceMode = lrEntityTypeInstance.EntityType.ReferenceMode
                    lrEntityTypeInstance.IsObjectifyingEntityType = lrEntityTypeInstance.EntityType.IsObjectifyingEntityType

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
                Dim lrFact As FBM.Fact

                For Each lrConceptInstance In lrXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.FactType)
                    lrFactTypeInstance = New FBM.FactTypeInstance

                    Dim lrFactType As New FBM.FactType(arModel, _
                                                       lrConceptInstance.Symbol, _
                                                       True)

                    lrFactType = arModel.FactType.Find(AddressOf lrFactType.Equals)

                    lrFactTypeInstance = lrFactType.CloneInstance(lrPage, True)
                    lrFactTypeInstance.X = lrConceptInstance.X
                    lrFactTypeInstance.Y = lrConceptInstance.Y

                    '----------------------------------------
                    'Get the Facts for the FactTypeInstance
                    '----------------------------------------
                    Dim lrFactInstance As FBM.FactInstance
                    Dim lrFactDataInstance As FBM.FactDataInstance
                    For Each lrFact In lrFactType.Fact

                        If IsSomething(lrXMLPage.ConceptInstance.Find(Function(x) x.Symbol = lrFact.Id And x.ConceptType = pcenumConceptType.Fact)) Then
                            '----------------------------------
                            'The Fact is included on the Page
                            '----------------------------------
                            lrFactInstance = lrFact.CloneInstance(lrPage)
                            lrFactTypeInstance.Fact.Add(lrFactInstance)

                            For Each lrFactDataInstance In lrFactInstance.Data

                                Dim lrFactDataConceptInstance As New FBM.ConceptInstance
                                lrFactDataConceptInstance.Symbol = lrFactDataInstance.Data
                                lrFactDataConceptInstance.RoleId = lrFactDataInstance.Role.Id

                                lrFactDataConceptInstance = lrXMLPage.ConceptInstance.Find(AddressOf lrFactDataConceptInstance.EqualsBySymbolRoleId)

                                lrFactDataInstance.X = lrFactDataConceptInstance.X
                                lrFactDataInstance.Y = lrFactDataConceptInstance.Y

                            Next

                        End If

                    Next
                Next

                '===============================================================================================
                'Populate RoleInstances that are (still) joined to Nothing
                '===========================================================
                Dim latType = {GetType(FBM.ValueTypeInstance), _
                               GetType(FBM.EntityTypeInstance), _
                               GetType(FBM.FactTypeInstance)}

                Dim larRole = From Role In lrPage.RoleInstance _
                               Where Role.JoinedORMObject Is Nothing
                               Select Role

                Dim lrRoleInstance As FBM.RoleInstance

                For Each lrRoleInstance In larRole
                    Select Case lrRoleInstance.TypeOfJoin
                        Case Is = pcenumRoleJoinType.FactType
                            lrRoleInstance.JoinedORMObject = lrPage.FactTypeInstance.Find(Function(x) x.Id = lrRoleInstance.JoinsFactType.Id)
                    End Select

                Next

                '=================================================================================
                'Mat the SubtypeRelationships.
                '=================================================================================

                Dim larSubtypeRelationshipFactTypes = From FactType In lrPage.FactTypeInstance _
                                                      Where FactType.IsSubtypeRelationshipFactType _
                                                      Select FactType

                Dim lrParentEntityTypeInstance As FBM.EntityTypeInstance

                For Each lrFactType In larSubtypeRelationshipFactTypes
                    lrEntityTypeInstance = New FBM.EntityTypeInstance
                    lrEntityTypeInstance.Id = lrFactType.RoleGroup(0).JoinedORMObject.Id
                    lrEntityTypeInstance = lrPage.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)

                    lrParentEntityTypeInstance = New FBM.EntityTypeInstance
                    lrParentEntityTypeInstance.Id = lrFactType.RoleGroup(1).JoinedORMObject.Id
                    lrParentEntityTypeInstance = lrPage.EntityTypeInstance.Find(AddressOf lrParentEntityTypeInstance.Equals)

                    Dim lrSubtypeConstraint As New FBM.tSubtypeRelationship
                    lrSubtypeConstraint.EntityType = lrEntityTypeInstance.EntityType
                    lrSubtypeConstraint.parentEntityType = lrParentEntityTypeInstance.EntityType

                    lrSubtypeConstraint = lrSubtypeConstraint.EntityType.SubtypeRelationship.Find(AddressOf lrSubtypeConstraint.Equals)

                    Dim lrSubtypeConstraintInstance As FBM.SubtypeRelationshipInstance

                    lrSubtypeConstraintInstance = lrSubtypeConstraint.CloneInstance(lrPage, True)

                    lrEntityTypeInstance.SubtypeRelationship.Add(lrSubtypeConstraintInstance)
                Next

                '===========================
                'Map the RoleNameInstances
                '===========================
                For Each lrConceptInstance In lrXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleName)

                    lrRoleInstance = New FBM.RoleInstance
                    lrRoleInstance.Id = lrConceptInstance.RoleId

                    lrRoleInstance = lrPage.RoleInstance.Find(AddressOf lrRoleInstance.Equals)

                    lrRoleInstance.RoleName = New FBM.RoleName(lrRoleInstance, lrRoleInstance.Name)
                    lrRoleInstance.RoleName.X = lrConceptInstance.X
                    lrRoleInstance.RoleName.Y = lrConceptInstance.Y
                Next

                '=================================
                'Map the RoleConstraintInstances
                '=================================
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                For Each lrConceptInstance In lrXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleConstraint)
                    lrRoleConstraintInstance = New FBM.RoleConstraintInstance

                    lrRoleConstraintInstance.Id = lrConceptInstance.Symbol

                    If IsSomething(lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)) Then
                        '-------------------------------------------------------------------
                        'The RoleConstraintInstance has already been added to the Page.
                        '  FactType.CloneInstance adds RoleConstraintInstances to the Page
                        '-------------------------------------------------------------------
                    Else
                        Dim lrRoleConstraint As New FBM.RoleConstraint
                        lrRoleConstraint.Id = lrRoleConstraintInstance.Id

                        lrRoleConstraint = arModel.RoleConstraint.Find(AddressOf lrRoleConstraint.Equals)

                        Select Case lrRoleConstraint.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                lrRoleConstraintInstance = lrRoleConstraint.CloneFrequencyConstraintInstance(lrPage)
                            Case Else
                                lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)
                        End Select

                        lrRoleConstraintInstance.X = lrConceptInstance.X
                        lrRoleConstraintInstance.Y = lrConceptInstance.Y

                        lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                    End If
                Next

                '============================
                'Map the ModelNoteInstances
                '============================
                Dim lrModelNoteInstance As FBM.ModelNoteInstance

                For Each lrConceptInstance In lrXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.ModelNote)
                    lrModelNoteInstance = New FBM.ModelNoteInstance

                    Dim lrModelNote As New FBM.ModelNote
                    lrModelNote.Id = lrConceptInstance.Symbol

                    lrModelNote = arModel.ModelNote.Find(AddressOf lrModelNote.Equals)

                    lrModelNoteInstance = lrModelNote.CloneInstance(lrPage, True)
                Next

                lrPage.Loaded = True
                lrPage.IsDirty = True
                arModel.Page.Add(lrPage)

            Next 'XMLModel.Page

        End Sub

        Public Sub GetFactsForFactType(ByRef arFactType As FBM.FactType) 'As List(Of FBM.Fact)

            Dim lrDictionaryEntry As New FBM.DictionaryEntry
            Dim lrFact As New FBM.Fact
            Dim liInd As Integer = 0
            Dim lrRole As FBM.Role
            Dim lsMessage As String = ""

            Try

                Dim lrXMLFact As XMLModel14.Fact
                Dim lrXMLFactData As XMLModel14.FactData
                Dim lrXMLFactType As New XMLModel14.FactType

                lrXMLFactType.Id = arFactType.Id
                lrXMLFactType = Me.ORMModel.FactTypes.Find(AddressOf lrXMLFactType.Equals)

                For Each lrXMLFact In lrXMLFactType.Facts

                    lrFact = New FBM.Fact(lrXMLFact.Id, arFactType)

                    For Each lrXMLFactData In lrXMLFact.Data
                        lrRole = New FBM.Role
                        lrRole.Id = lrXMLFactData.RoleId
                        lrRole = arFactType.RoleGroup.Find(AddressOf lrRole.Equals)

                        '--------------------------------------------------------------------------------------------------
                        'Get the Concept from the ModelDictionary so that FactData objects are linked directly to the Concept/Value in the ModelDictionary
                        '--------------------------------------------------------------------------------------------------
                        lrDictionaryEntry = New FBM.DictionaryEntry(arFactType.Model, _
                                                                    lrXMLFactData.Data, _
                                                                    pcenumConceptType.Value, _
                                                                    , _
                                                                    , True)

                        lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry)

                        Dim lrConcept As FBM.Concept = lrDictionaryEntry.Concept

                        Dim lrFactData As New FBM.FactData(lrRole, lrConcept, lrFact)
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
            Dim lrXMLRole As XMLModel14.Role
            Dim lrRole As FBM.Role
            Dim lrXMLFactType As New XMLModel14.FactType

            lrXMLFactType.Id = arFactType.Id
            lrXMLFactType.GUID = arFactType.GUID
            lrXMLFactType = Me.ORMModel.FactTypes.Find(AddressOf lrXMLFactType.Equals)

            'arFactType.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
            'arFactType.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
            arFactType.IsObjectified = lrXMLFactType.IsObjectified
            'arFactType.IsCoreFactType = CBool(lREcordset("IsCoreFactType").Value)
            arFactType.IsPreferredReferenceMode = lrXMLFactType.IsPreferredReferenceSchemeFT
            arFactType.IsSubtypeRelationshipFactType = lrXMLFactType.IsSubtypeRelationshipFactType
            arFactType.IsDerived = lrXMLFactType.IsDerived
            arFactType.IsStored = lrXMLFactType.IsStored
            arFactType.DerivationText = lrXMLFactType.DerivationText

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

            If Not arFactType.Model.FactType.Exists(AddressOf arFactType.Equals) Then
                arFactType.Model.FactType.Add(arFactType)
            End If


        End Sub

        Public Function GetFactTypeReadingsForFactType(ByRef arFactType As FBM.FactType) As List(Of FBM.FactTypeReading)

            Dim lrXMLFactType As XMLModel14.FactType
            Dim lrXMLFactTypeReading As XMLModel14.FactTypeReading
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lsMessage As String = ""

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetFactTypeReadingsForFactType = New List(Of FBM.FactTypeReading)

            Try

                lrXMLFactType = New XMLModel14.FactType
                lrXMLFactType.Id = arFactType.Id
                lrXMLFactType = Me.ORMModel.FactTypes.Find(AddressOf lrXMLFactType.Equals)

                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart
                Dim lrXMLPredicatePart As XMLModel14.PredicatePart

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
