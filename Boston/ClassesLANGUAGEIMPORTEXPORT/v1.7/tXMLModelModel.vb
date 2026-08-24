Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks
Imports System.Xml.Serialization
Imports Boston.FBMMetaModel

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

        ' Build a dictionaries for faster search
        <XmlIgnore>
        Public ValueTypeDictionary As Dictionary(Of String, FBM.ValueType)
        <XmlIgnore>
        Public EntityTypeDictionary As Dictionary(Of String, FBM.EntityType)
        <XmlIgnore>
        Public FactTypeDictionary As Dictionary(Of String, FBM.FactType)
        <XmlIgnore>
        Public RoleConstraintDictionary As Dictionary(Of String, FBM.RoleConstraint)

        Private Function FactTypeIsReferenceModeFactType(ByVal asXMLFactTypeId As String) As Boolean

            Try
                Dim larFactType = From EntityType In Me.ORMModel.EntityTypes
                                  From RoleConstraint In Me.ORMModel.RoleConstraints
                                  From FactType In Me.ORMModel.FactTypes
                                  From Role In FactType.RoleGroup
                                  Where FactType.Id = asXMLFactTypeId
                                  Where EntityType.ReferenceSchemeRoleConstraintId IsNot Nothing
                                  Where EntityType.ReferenceSchemeRoleConstraintId = RoleConstraint.Id
                                  Where RoleConstraint.IsPreferredUniqueness
                                  Where RoleConstraint.RoleConstraintRoles(0).RoleId = Role.Id
                                  Where RoleConstraint.RoleConstraintRoles.Count = 1
                                  Where FactType.RoleGroup.Count = 2
                                  Select FactType

                Return larFactType.Count > 0

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function


        ''' <summary>
        ''' Maps an instance of FBM.Model to this class.
        ''' </summary>
        ''' <param name="arFBMModel">The model being mapped.</param>
        ''' <remarks></remarks>
        Public Function MapFromFBMModel(ByVal arFBMModel As FBM.Model, Optional ByVal abExcludedMDAModelElements As Boolean = False) As Boolean

            Try
                Dim lrSubtypeRelationship As FBM.SubtypeRelationship
                Dim lrXMLSubtypeRelationship As XMLModel.SubtypeRelationship

                Me.ORMModel = New XMLModel.ORMModel
                Me.ORMModel.ModelId = arFBMModel.ModelId
                Me.ORMModel.Name = arFBMModel.Name
                Me.ORMModel.CoreVersionNumber = arFBMModel.CoreVersionNumber

                '========================
                'Process the ValueTypes
                '========================
#Region "ValueTypes"
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
                    lrXMLValueType.ObjectifyingFactTypeId = lrValueType.ObjectifyingFactTypeId
                    lrXMLValueType.GUID = lrValueType.GUID
                    lrXMLValueType.IsMDAModelElement = lrValueType.IsMDAModelElement
                    lrXMLValueType.DBName = lrValueType.DBName

                    If lrXMLValueType.IsMDAModelElement And abExcludedMDAModelElements Then GoTo SkipModelLevelValueType

                    Dim lsValueConstraintValue As String

                    For Each lsValueConstraintValue In lrValueType.ValueConstraint
                        lrXMLValueType.ValueConstraint.Add(lsValueConstraintValue)
                    Next

                    For Each lrSubtypeRelationship In lrValueType.SubtypeRelationship

                        lrXMLSubtypeRelationship = New XMLModel.SubtypeRelationship
                        lrXMLSubtypeRelationship.ParentEntityTypeId = lrSubtypeRelationship.parentModelElement.Id
                        lrXMLSubtypeRelationship.SubtypingFactTypeId = lrSubtypeRelationship.FactType.Id
                        lrXMLSubtypeRelationship.IsPrimarySubtypeRelationship = lrSubtypeRelationship.IsPrimarySubtypeRelationship

                        lrXMLValueType.SubtypeRelationships.Add(lrXMLSubtypeRelationship)
                    Next

                    Me.ORMModel.ValueTypes.Add(lrXMLValueType)
SkipModelLevelValueType:
                Next
#End Region
                '========================
                'Process the EntityTypes
                '========================
#Region "EntityTypes"
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
                    lrXMLEntityType.HideReferenceMode = lrEntityType.HideReferenceMode
                    lrXMLEntityType.IsIndependent = lrEntityType.IsIndependent
                    lrXMLEntityType.IsAbsorbed = lrEntityType.IsAbsorbed
                    lrXMLEntityType.IsPersonal = lrEntityType.IsPersonal
                    lrXMLEntityType.IsDerived = lrEntityType.IsDerived
                    lrXMLEntityType.DerivationText = lrEntityType.DerivationText
                    lrXMLEntityType.LongDescription = lrEntityType.LongDescription
                    lrXMLEntityType.ShortDescription = lrEntityType.ShortDescription
                    lrXMLEntityType.IsMDAModelElement = lrEntityType.IsMDAModelElement
                    lrXMLEntityType.DBName = lrEntityType.DBName
                    lrXMLEntityType.ModelElementFlags = lrEntityType.ModelElementFlag
                    lrXMLEntityType.GraphLabel = lrEntityType.GraphLabel.Select(Function(x) x.Label).ToList

                    If lrXMLEntityType.IsMDAModelElement And abExcludedMDAModelElements Then GoTo SkipModelLevelEntityType

                    If lrEntityType.ReferenceModeRoleConstraint IsNot Nothing Then
                        lrXMLEntityType.ReferenceSchemeRoleConstraintId = lrEntityType.ReferenceModeRoleConstraint.Id
                    End If

                    If lrEntityType.ReferenceModeValueType IsNot Nothing Then
                        lrXMLEntityType.ReferenceModeValueTypeId = lrEntityType.ReferenceModeValueType.Id
                    End If

                    For Each lrSubtypeRelationship In lrEntityType.SubtypeRelationship

                        Try
                            lrXMLSubtypeRelationship = New XMLModel.SubtypeRelationship
                            lrXMLSubtypeRelationship.ParentEntityTypeId = lrSubtypeRelationship.parentModelElement.Id
                            lrXMLSubtypeRelationship.SubtypingFactTypeId = lrSubtypeRelationship.FactType.Id
                            lrXMLSubtypeRelationship.IsPrimarySubtypeRelationship = lrSubtypeRelationship.IsPrimarySubtypeRelationship

                            lrXMLEntityType.SubtypeRelationships.Add(lrXMLSubtypeRelationship)
                        Catch ex As Exception
                            prApplication.ThrowMessage("Error exporting Subtype Relationship for Entity Type: " & lrXMLEntityType.Id, pcenumErrorType.Warning, Nothing, False, False, True, MessageBoxButtons.OK, False, Nothing)
                            Throw New Exception("Error exporting Model to FBM format.")
                        End Try
                    Next

                    Me.ORMModel.EntityTypes.Add(lrXMLEntityType)
SkipModelLevelEntityType:
                Next
#End Region

                '=======================
                'Process the FactTypes
                '=======================
#Region "FactTypes"
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
                    If lrFactType.ObjectifyingEntityType IsNot Nothing Then
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
                    lrXMLFactType.ModelElementFlags = lrFactType.ModelElementFlag
#Region "Property Graph Schema specific"
                    lrXMLFactType.GraphLabel = lrFactType.GraphLabel.Select(Function(x) x.Label).ToList
                    lrXMLFactType.Source = lrFactType.Source
                    lrXMLFactType.Target = lrFactType.Target
#End Region

                    If lrXMLFactType.IsMDAModelElement And abExcludedMDAModelElements Then GoTo SkipModelLevelFactType

#Region "Derivation Rule"

                    '--------------------------------------------
                    ' FactType.DerivationRule  -> XMLModel.DerivationRule
                    '--------------------------------------------
                    If lrFactType.DerivationRule IsNot Nothing Then

                        Dim lrDerivationRule As FBM.DerivationRule = lrFactType.DerivationRule
                        Dim lrXMLDerivationRule As New XMLModel.DerivationRule

                        '--------------------------------------------
                        ' DerivationRule.FactTypeDerivationPath
                        '--------------------------------------------
                        If lrDerivationRule.FactTypeDerivationPath IsNot Nothing Then

                            Dim lrDerivationPath As FBM.FactTypeDerivationPath = lrDerivationRule.FactTypeDerivationPath
                            Dim lrXMLDerivationPath As New XMLModel.FactTypeDerivationPath

                            lrXMLDerivationPath.Id = lrDerivationPath.Id
                            lrXMLDerivationPath.Name = lrDerivationPath.Name

                            '--------------------------------------------
                            ' PathComponents.RolePath
                            '--------------------------------------------
                            If lrDerivationPath.PathComponents IsNot Nothing AndAlso lrDerivationPath.PathComponents.RolePaths.Count > 0 Then

                                Dim lrXMLPathComponents As New XMLModel.PathComponents

                                For Each lrRolePath As FBM.RolePath In lrDerivationPath.PathComponents.RolePaths

                                    'Dim lrRolePath As FBM.RolePath = lrDerivationPath.PathComponents.RolePath 'REmoved because we are now storing multiple RolePaths

                                    Dim lrXMLRolePath As New XMLModel.RolePath

                                    lrXMLRolePath.Id = lrRolePath.Id
                                    lrXMLRolePath.SplitCombinationOperator = lrRolePath.SplitCombinationOperator

                                    'RootObjectType is XmlIgnore in XMLModel.RolePath, and we still copy it for runtime usage.
                                    If lrRolePath.RootObjectType IsNot Nothing Then
                                        lrXMLRolePath.RootObjectType = New XMLModel.RootObjectType
                                        lrXMLRolePath.RootObjectType.Id = lrRolePath.RootObjectType.id
                                        lrXMLRolePath.RootObjectType.ModelElementId = lrRolePath.RootObjectType.BostonModelElement.Id
                                        lrXMLRolePath.RootObjectType.IsNegated = lrRolePath.RootObjectType.IsNegated
                                    End If

                                    '--------------------------------------------
                                    ' RolePath.PathedRoles
                                    '--------------------------------------------
                                    If lrRolePath.PathedRole IsNot Nothing Then
                                        For Each lrPathedRole As FBM.PathedRole In lrRolePath.PathedRole
                                            Dim lrXMLPathedRole As New XMLModel.PathedRole
                                            lrXMLPathedRole.id = lrPathedRole.id
                                            lrXMLPathedRole.Ref = lrPathedRole.Ref
                                            lrXMLPathedRole.IsNegated = lrPathedRole.IsNegated
                                            lrXMLPathedRole.Purpose = lrPathedRole.Purpose

                                            lrXMLPathedRole.ValueRestriction = ConvertPathedRoleValueRestrictionToXmlModel(lrPathedRole.ValueRestriction)

                                            lrXMLRolePath.PathedRole.Add(lrXMLPathedRole)
                                        Next
                                    End If

                                    '--------------------------------------------
                                    ' RolePath.SubPaths
                                    '--------------------------------------------
                                    If lrRolePath.SubPath IsNot Nothing Then
                                        For Each lrSubPath As FBM.RoleSubPath In lrRolePath.SubPath

                                            Dim lrXMLSubPath As XMLModel.RoleSubPath = ConvertRoleSubPathToXmlModel(lrSubPath)

                                            If lrXMLSubPath IsNot Nothing Then
                                                lrXMLRolePath.SubPath.Add(lrXMLSubPath)
                                            End If

                                        Next
                                    End If

                                    '--------------------------------------------
                                    ' RolePath.ObjectUnifiers
                                    '--------------------------------------------
                                    If lrRolePath.ObjectUnifier IsNot Nothing Then
                                        For Each lrObjectUnifier As FBM.ObjectUnifier In lrRolePath.ObjectUnifier

                                            Dim lrXMLObjectUnifier As New XMLModel.ObjectUnifier
                                            lrXMLObjectUnifier.id = lrObjectUnifier.id

                                            '-----------------------------------------
                                            ' PathedRole refs
                                            '-----------------------------------------
                                            If lrObjectUnifier.PathedRoles IsNot Nothing Then
                                                For Each lrPathedRoleRef As FBM.PathedRoleReference In lrObjectUnifier.PathedRoles
                                                    Dim lrXMLPathedRoleRef As New XMLModel.PathedRoleReference
                                                    lrXMLPathedRoleRef.Ref = lrPathedRoleRef.Ref
                                                    lrXMLObjectUnifier.PathedRoles.Add(lrXMLPathedRoleRef)
                                                Next
                                            End If

                                            '-----------------------------------------
                                            ' PathRoot refs
                                            '-----------------------------------------
                                            If lrObjectUnifier.PathRoots IsNot Nothing Then
                                                For Each lrPathRootRef As FBM.PathRootReference In lrObjectUnifier.PathRoots
                                                    Dim lrXMLPathRootRef As New XMLModel.PathRootReference
                                                    lrXMLPathRootRef.Ref = lrPathRootRef.Ref
                                                    lrXMLObjectUnifier.PathRoots.Add(lrXMLPathRootRef)
                                                Next
                                            End If

                                            lrXMLRolePath.ObjectUnifier.Add(lrXMLObjectUnifier)

                                        Next
                                    End If


                                    '--------------------------------------------
                                    ' RolePath.CalculatedValues  (typed)
                                    '--------------------------------------------
                                    If lrRolePath.CalculatedValues IsNot Nothing Then

                                        For Each lrCalculatedValue As FBM.CalculatedValue In lrRolePath.CalculatedValues

                                            Dim lrXMLCalculatedValue As New XMLModel.CalculatedValue
                                            lrXMLCalculatedValue.Id = lrCalculatedValue.Id

                                            'Function ref
                                            If lrCalculatedValue.Function IsNot Nothing Then
                                                lrXMLCalculatedValue.Function = New XMLModel.FunctionRef
                                                lrXMLCalculatedValue.Function.Ref = lrCalculatedValue.Function.Ref
                                            End If

                                            'AggregationContext
                                            If lrCalculatedValue.AggregationContext IsNot Nothing Then

                                                Dim lrXMLAggregationContext As New XMLModel.AggregationContext

                                                If lrCalculatedValue.AggregationContext.PathRoot IsNot Nothing Then
                                                    lrXMLAggregationContext.PathRoot = New XMLModel.PathRootReference
                                                    lrXMLAggregationContext.PathRoot.Ref = lrCalculatedValue.AggregationContext.PathRoot.Ref
                                                End If

                                                lrXMLCalculatedValue.AggregationContext = lrXMLAggregationContext

                                            End If

                                            'Inputs
                                            If lrCalculatedValue.Inputs IsNot Nothing AndAlso lrCalculatedValue.Inputs.Input IsNot Nothing Then

                                                lrXMLCalculatedValue.Inputs = New XMLModel.Inputs

                                                For Each lrInput As FBM.Input In lrCalculatedValue.Inputs.Input

                                                    Dim lrXMLInput As New XMLModel.Input
                                                    lrXMLInput.Id = lrInput.Id

                                                    If lrInput.Parameter IsNot Nothing Then
                                                        lrXMLInput.Parameter = New XMLModel.ParameterRef
                                                        lrXMLInput.Parameter.Ref = lrInput.Parameter.Ref
                                                    End If

                                                    If lrInput.Source IsNot Nothing Then
                                                        lrXMLInput.Source = New XMLModel.Source

                                                        If lrInput.Source.Item IsNot Nothing Then

                                                            If TypeOf lrInput.Source.Item Is FBM.PathRootReference Then

                                                                Dim lrInRef As FBM.PathRootReference = CType(lrInput.Source.Item, FBM.PathRootReference)

                                                                Dim lrOutRef As New XMLModel.PathRootReference
                                                                lrOutRef.Ref = lrInRef.Ref
                                                                lrXMLInput.Source.Item = lrOutRef

                                                            ElseIf TypeOf lrInput.Source.Item Is FBM.PathedRoleRef Then
                                                                Dim lrInRef As FBM.PathedRoleRef = CType(lrInput.Source.Item, FBM.PathedRoleRef)
                                                                Dim lrOutRef As New XMLModel.PathedRoleRef
                                                                lrOutRef.Ref = lrInRef.Ref
                                                                lrXMLInput.Source.Item = lrOutRef

                                                            ElseIf TypeOf lrInput.Source.Item Is FBM.CalculatedValueRef Then
                                                                Dim lrInRef As FBM.CalculatedValueRef = CType(lrInput.Source.Item, FBM.CalculatedValueRef)
                                                                Dim lrOutRef As New XMLModel.CalculatedValueRef
                                                                lrOutRef.Ref = lrInRef.Ref
                                                                lrXMLInput.Source.Item = lrOutRef

                                                            ElseIf TypeOf lrInput.Source.Item Is FBM.Constant Then
                                                                Dim lrInConst As FBM.Constant = CType(lrInput.Source.Item, FBM.Constant)
                                                                Dim lrOutConst As New XMLModel.Constant
                                                                lrOutConst.Id = lrInConst.Id
                                                                lrOutConst.Value = lrInConst.Value
                                                                lrXMLInput.Source.Item = lrOutConst

                                                            End If
                                                        End If
                                                    End If

                                                    lrXMLCalculatedValue.Inputs.Input.Add(lrXMLInput)

                                                Next 'Input
                                            End If 'Inputs

                                            lrXMLRolePath.CalculatedValues.Add(lrXMLCalculatedValue)

                                        Next 'CalculatedValue
                                    End If

                                    '--------------------------------------------
                                    ' RolePath.Conditions  (typed)
                                    '--------------------------------------------
                                    If lrRolePath.Conditions IsNot Nothing Then

                                        Dim lrXMLConditions As New XMLModel.Conditions

                                        If lrRolePath.Conditions.Items IsNot Nothing Then
                                            For Each lrNode As FBM.ConditionNode In lrRolePath.Conditions.Items
                                                Dim lrXMLNode As XMLModel.ConditionNode = ConvertConditionNodeToXmlModel(lrNode)
                                                If lrXMLNode IsNot Nothing Then
                                                    lrXMLConditions.Items.Add(lrXMLNode)
                                                End If
                                            Next
                                        End If

                                        lrXMLRolePath.Conditions = lrXMLConditions

                                    End If


                                    lrXMLPathComponents.RolePath.Add(lrXMLRolePath)

                                Next

                                lrXMLDerivationPath.PathComponents = lrXMLPathComponents
                            End If

                            '--------------------------------------------
                            ' DerivationProjections  (NORMA-structured)
                            '--------------------------------------------
                            If lrDerivationPath.Projection IsNot Nothing Then

                                For Each lrProjection As FBM.DerivationProjection In lrDerivationPath.Projection

                                    Dim lrXMLProjection As New XMLModel.DerivationProjection

                                    lrXMLProjection.Id = lrProjection.Id
                                    lrXMLProjection.Ref = lrProjection.Ref

                                    '-----------------------------------------
                                    ' RoleProjection (0..n)
                                    '-----------------------------------------
                                    If lrProjection.RoleProjection IsNot Nothing Then

                                        For Each lrRoleProjection As FBM.RoleProjection In lrProjection.RoleProjection

                                            Dim lrXMLRoleProjection As New XMLModel.RoleProjection

                                            lrXMLRoleProjection.Id = lrRoleProjection.Id
                                            lrXMLRoleProjection.Ref = lrRoleProjection.Ref

                                            '-----------------------------------------
                                            ' DerivationSource
                                            '-----------------------------------------
                                            If lrRoleProjection.DerivationSource IsNot Nothing Then

                                                Dim lrXMLDerivationSource As New XMLModel.DerivationSource

                                                ' PathRoot
                                                If lrRoleProjection.DerivationSource.PathRoot IsNot Nothing Then
                                                    lrXMLDerivationSource.PathRoot = New XMLModel.PathRootReference With {
                                                                        .Ref = lrRoleProjection.DerivationSource.PathRoot.Ref
                                                                    }
                                                End If

                                                ' CalculatedValue
                                                If lrRoleProjection.DerivationSource.CalculatedValue IsNot Nothing Then
                                                    lrXMLDerivationSource.CalculatedValue = New XMLModel.CalculatedValueReference With {
                                                                        .Ref = lrRoleProjection.DerivationSource.CalculatedValue.Ref
                                                                    }
                                                End If

                                                ' PathedRole
                                                If lrRoleProjection.DerivationSource.PathedRole IsNot Nothing Then
                                                    lrXMLDerivationSource.PathedRole = New XMLModel.PathedRoleReference With {
                                                                        .Ref = lrRoleProjection.DerivationSource.PathedRole.Ref
                                                                    }
                                                End If

                                                lrXMLRoleProjection.DerivationSource = lrXMLDerivationSource

                                            End If

                                            lrXMLProjection.RoleProjection.Add(lrXMLRoleProjection)

                                        Next 'RoleProjection

                                    End If

                                    lrXMLDerivationPath.Projection.Add(lrXMLProjection)

                                Next 'DerivationProjection

                            End If

                            lrXMLDerivationRule.FactTypeDerivationPath = lrXMLDerivationPath

                        End If

                        'Attach to FactType
                        lrXMLFactType.DerivationRule = lrXMLDerivationRule

                    End If

#End Region

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

                        If lrRole.JoinedORMObject IsNot Nothing Then
                            lrXMLRole.JoinedObjectTypeId = lrRole.JoinedORMObject.Id
                        End If

                        lrXMLRole.JoinedObjectType = Me.ORMModel.getModelElementById(lrXMLRole.JoinedObjectTypeId)

                        lrXMLFactType.RoleGroup.Add(lrXMLRole)
                    Next

                    '---------------
                    'Map the Facts
                    '---------------
#Region "Facts"
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
#End Region

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
SkipModelLevelFactType:
                Next
#End Region

                '------------------------------------------------------------------------------------
                'IMPORTANT: Make sure the LinkFactTypes are at the end of the list
                Me.ORMModel.FactTypes.Sort(Function(x, y) x.IsLinkFactType.CompareTo(y.IsLinkFactType))

                '=========================
                'Map the RoleConstraints
                '=========================
#Region "RoleConstraints"
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

                    If lrXMLRoleConstraint.IsMDAModelElement And abExcludedMDAModelElements Then GoTo SkipModelLevelRoleConstraint

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
SkipModelLevelRoleConstraint:
                Next
#End Region

                '================
                'Map ModelNotes
                '================
#Region "ModelNotes"
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
#End Region

                '==========================
                'Map Functions (per nORMa)
                '==========================
#Region "Functions"
                Dim lrFunction As FBM.Function
                Dim lrXMLFunction As XMLModel.Function

                For Each lrFunction In arFBMModel.Function

                    lrXMLFunction = New XMLModel.Function

                    lrXMLFunction.Id = lrFunction.id
                    lrXMLFunction.Name = lrFunction.Name
                    lrXMLFunction.OperatorSymbol = lrFunction.OperatorSymbol
                    lrXMLFunction.IsBoolean = lrFunction.IsBoolean

                    For Each lrParameter In lrFunction.Parameters

                        Dim lrXMLParameter = New XMLModel.Parameter

                        lrXMLParameter.Id = lrParameter.id
                        lrXMLParameter.Name = lrParameter.Name
                        lrXMLParameter.BagInput = lrParameter.BagInput

                        lrXMLFunction.Parameters.Add(lrXMLParameter)

                    Next

                    Me.ORMModel.Functions.Add(lrXMLFunction)
                Next

#End Region
                '================
                'Map Synonymns
                '================
#Region "Synonyms"
                For Each lrSynonym In arFBMModel.Synonyms

                    Me.ORMModel.Synonyms.Add(New XMLModel.Synonym(lrSynonym.BaseTermModelElement.Id, lrSynonym.Synonym))

                Next
#End Region

                '================================
                'Map the Pages
                '================================
#Region "Pages"
                Dim lrPage As FBM.Page
                Dim lrExportPage As XMLModel.Page = Nothing

                For Each lrPage In arFBMModel.Page

                    If lrPage.Language <> pcenumLanguage.ORMModel And abExcludedMDAModelElements Then Continue For

                    If lrPage.GetAllPageObjects.Count > 0 Then
                        lrPage.Loaded = True
                    End If

                    lrExportPage = Me.MapToXMLPage(lrPage)

                    Me.ORMDiagram.Add(lrExportPage)
                Next
#End Region

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        Private Function MapToXMLPage(ByRef arPage As FBM.Page) As XMLModel.Page

            Dim lrExportPage As New XMLModel.Page
            Dim lrPage = arPage

            Try
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
                ' Nothing else is required to draw a ConceptualModel. Different types of ConceptualModel require different types of Concepts
                ' as they appear in a Model such that the ConceptualModel can be drawn.
                ' e.g. Within a Use Case Diagram, Actors are Values within a Fact. The Fact has no X/Y co-ordinate, but the Value/Actor does.
                ' NB A Value (e.g. 'Storeman' may well be an EntityType within an ORM-Diagram but within a Use Case Diagram,
                ' drawn from the Core-UseCaseDiagram (metamodel) ORM-Diagram, the Value becomes an Actor(Entity).
                ' Although this is Higher-Order Logic at work, FOL is preserved because
                ' the modeler (or tool. e.g. Richmond) only works with on Page/View (i.e. 'Interpretation')
                ' at a time. i.e. While working with a Use Case Diagram, Richmond works over
                ' the MetaModel of a Use Case Diagram in First-Order Logic.
                '-----------------------------------------------------------------------

                '=============================================================================
                'Overarching
                '  In this strategy, if a User has never opened a Page, then all of the CocneptInstances are within its ConceptInstance list.
                '  I.e. There is no need to laboriously step through ValueTypeInstances, EntityTypeInstance etc.
                '  Just copy the ConceptInstance list over to the Export page and you are done.
                If arPage.Loaded = False And arPage.ConceptInstance.Count > 0 And lrPage.GetAllPageObjects.Count = 0 Then
                    lrExportPage.ConceptInstance = arPage.ConceptInstance
                    Return lrExportPage
                End If


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
#Region "EntityTypeInstances"
                For Each lrEntityTypeInstance In lrPage.EntityTypeInstance

                    lrConceptInstance = lrEntityTypeInstance.CloneConceptInstance
                    '-------------------------------------
                    'Add the ConceptInstance to the Page
                    '-------------------------------------
                    lrExportPage.ConceptInstance.Add(lrConceptInstance)
                Next
#End Region

                '--------------------------------------------------------------
                'Establish the set of ValueTypeInstances that are on the Page
                '  as ConceptInstances.
                '--------------------------------------------------------------
#Region "ValueTypeInstances"
                For Each lrValueTypeInstance In lrPage.ValueTypeInstance
                    lrConceptInstance = lrValueTypeInstance.CloneConceptInstance
                    '-------------------------------------
                    'Add the ConceptInstance to the Page
                    '-------------------------------------
                    lrExportPage.ConceptInstance.Add(lrConceptInstance)
                Next
#End Region

                '--------------------------------------------------------------
                'Establish the set of FactTypeInstances that are on the Page
                '  as ConceptInstances.
                '--------------------------------------------------------------
#Region "FactTypeInstances"
                'NB Facts are below RoleConstraintInstances, below
                For Each lrFactTypeInstance In lrPage.FactTypeInstance

                    lrConceptInstance = lrFactTypeInstance.CloneConceptInstance

                    lrConceptInstance.Visible = lrFactTypeInstance._Visible Or lrFactTypeInstance.IsObjectified Or (lrFactTypeInstance.Shape IsNot Nothing AndAlso lrFactTypeInstance.Shape.Visible)

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
                                                                 pcenumConceptType.FactTypeReading,
                                                                 lrFactTypeInstance.InstanceNumber)

                    lrConceptInstance.X = lrFactTypeInstance.FactTypeReadingPoint.X
                    lrConceptInstance.Y = lrFactTypeInstance.FactTypeReadingPoint.Y
                    lrExportPage.ConceptInstance.Add(lrConceptInstance)

                    'FactTypeName
                    lrConceptInstance = New FBM.ConceptInstance(lrFactTypeInstance.Model,
                                                                 lrFactTypeInstance.Page,
                                                                 lrFactTypeInstance.Id,
                                                                 pcenumConceptType.FactTypeName,
                                                                 lrFactTypeInstance.InstanceNumber)
                    lrConceptInstance.Visible = lrFactTypeInstance.ShowFactTypeName
                    lrConceptInstance.X = lrFactTypeInstance.FactTypeName.X
                    lrConceptInstance.Y = lrFactTypeInstance.FactTypeName.Y
                    lrExportPage.ConceptInstance.Add(lrConceptInstance)

                    'DerivationText
                    If lrFactTypeInstance.FactTypeDerivationText IsNot Nothing Then
                        lrConceptInstance = New FBM.ConceptInstance(lrFactTypeInstance.Model,
                                                                 lrFactTypeInstance.Page,
                                                                 lrFactTypeInstance.Id,
                                                                 pcenumConceptType.DerivationText,
                                                                 lrFactTypeInstance.InstanceNumber)
                        lrConceptInstance.Visible = True
                        lrConceptInstance.X = lrFactTypeInstance.FactTypeDerivationText.X
                        lrConceptInstance.Y = lrFactTypeInstance.FactTypeDerivationText.Y
                        lrExportPage.ConceptInstance.Add(lrConceptInstance)
                    End If
                Next
#End Region

                '--------------------------------------------------------------
                'Establish the set of RoleConstraintInstances that are on the Page
                '  as ConceptInstances.
                '--------------------------------------------------------------
#Region "RoleConstraintInstances"
                For Each lrRoleConstraintInstance In lrPage.RoleConstraintInstance

                    lrConceptInstance = lrRoleConstraintInstance.CloneConceptInstance
                    '-------------------------------------
                    'Add the ConceptInstance to the Page
                    '-------------------------------------
                    lrExportPage.ConceptInstance.Add(lrConceptInstance)
                Next
#End Region

                '--------------------------------------------------------------
                'Establish the set of FactInstances that are on the Page
                '  as ConceptInstances.
                '--------------------------------------------------------------
#Region "FactInstances"
                Dim larFactInstance = From FactTypeInstance In lrPage.FactTypeInstance
                                      From FactInstance In FactTypeInstance.Fact
                                      Select FactInstance

                For Each lrFactInstance In larFactInstance

                    lrConceptInstance = lrFactInstance.CloneConceptInstance
                    lrConceptInstance.X = lrFactInstance.X
                    lrConceptInstance.Y = lrFactInstance.Y
                    '-------------------------------------
                    'Add the ConceptInstance to the Page
                    '-------------------------------------
                    lrExportPage.ConceptInstance.Add(lrConceptInstance)
                Next
#End Region

#Region "FactDataInstances"
                '--------------------------------------------------------------
                'Establish the set of ValueInstances that are on the Page
                '  as ConceptInstances.
                '--------------------------------------------------------------
                Dim larFactData = From FactInstance In larFactInstance
                                  From FactData In FactInstance.Data
                                  Select FactData

                For Each lrFactDataInstance In larFactData
                    lrConceptInstance = lrFactDataInstance.CloneConceptInstance
                    '-------------------------------------
                    'Add the ConceptInstance to the Page
                    '-------------------------------------
                    lrExportPage.ConceptInstance.Add(lrConceptInstance)
                Next
#End Region

                '--------------------------------------------------------------
                'Establish the set of RoleNameInstances that are on the Page
                '  as ConceptInstances.
                '--------------------------------------------------------------
                For Each lrRoleInstance In lrPage.RoleInstance.FindAll(Function(x) x.Name <> "")
                    Try
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
                    Catch ex As Exception
                        'Skip RoleInstance
                    End Try
                Next

                '-----------------------------------------------
                'Model Notes
                '-----------------------------------------------
#Region "Model Notes"
                For Each lrModelNoteInstance In lrPage.ModelNoteInstance
                    lrConceptInstance = lrModelNoteInstance.CloneConceptInstance
                    '-------------------------------------
                    'Add the ConceptInstance to the Page
                    '-------------------------------------
                    lrExportPage.ConceptInstance.Add(lrConceptInstance)
                Next
#End Region

                Return lrExportPage

            Catch ex As Exception
                Return lrExportPage
            End Try

        End Function

        ''' <summary>
        ''' Maps an instance of this class to an instance of FBM.Model
        ''' </summary>
        ''' <param name="arModel">The FBM Model to map to.</param>
        ''' <param name="aoBackgroundWorker">For reporting progress. Start at 60%.</param>
        ''' <param name="abSkipAlreadyLoadedModelElements">True if to test whether ModelElements have already been loaded.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MapToFBMModel(Optional ByRef arModel As FBM.Model = Nothing,
                                      Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing,
                                      Optional ByVal abSkipAlreadyLoadedModelElements As Boolean = False) As FBM.Model

            Try
                Dim larFactType As List(Of FBM.FactType)
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

                lrModel.StoreAsXML = True

                lrModel.CoreVersionNumber = Me.ORMModel.CoreVersionNumber

                '==============================
                'Map the ValueTypes
                '==============================
#Region "ValueTypes"
                Dim lrXMLValueType As XMLModel.ValueType
                Dim lrValueType As FBM.ValueType
                Dim lsValueTypeConstraintValue As String

                For Each lrXMLValueType In Me.ORMModel.ValueTypes

                    If abSkipAlreadyLoadedModelElements Then
                        'Really only ever called when using the UnifiedOntologyBrowser
                        '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
                        If lrModel.ValueType.Find(Function(x) x.Id = lrXMLValueType.Id) IsNot Nothing Then GoTo SkipValueType
                    End If

                    lrValueType = New FBM.ValueType
                    lrValueType.Model = lrModel
                    lrValueType.Id = lrXMLValueType.Id
                    lrValueType.GUID = lrXMLValueType.GUID
                    lrValueType.Name = lrXMLValueType.Name
                    lrValueType.DataType = lrXMLValueType.DataType
                    lrValueType.DataTypePrecision = lrXMLValueType.DataTypePrecision
                    lrValueType.DataTypeLength = lrXMLValueType.DataTypeLength
                    lrValueType.IsIndependent = lrXMLValueType.IsIndependent
                    lrValueType.ObjectifyingFactTypeId = lrXMLValueType.ObjectifyingFactTypeId
                    lrValueType.IsMDAModelElement = lrXMLValueType.IsMDAModelElement
                    lrValueType.DBName = lrXMLValueType.DBName
                    lrValueType.ShortDescription = lrXMLValueType.ShortDescription
                    lrValueType.LongDescription = lrXMLValueType.LongDescription

                    For Each lsValueTypeConstraintValue In lrXMLValueType.ValueConstraint
                        lrValueType.ValueConstraint.Add(lsValueTypeConstraintValue)
                        lrValueType.Instance.Add(lsValueTypeConstraintValue)
                    Next

                    '------------------------------------------------
                    'Link to the Concept within the ModelDictionary
                    '------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrValueType.Id, pcenumConceptType.ValueType, lrValueType.ShortDescription, lrValueType.LongDescription, True, True, lrValueType.DBName)
                    lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , False,, True,, True)


                    lrValueType.Concept = lrDictionaryEntry.Concept

                    lrModel.ValueType.AddUnique(lrValueType)
SkipValueType:
                Next

                ' Build a dictionary for faster search
                Me.ValueTypeDictionary = lrModel.ValueType.ToDictionary(Function(x) x.Id)

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(62)
#End Region

                '==============================
                'Map the EntityTypes
                '==============================
#Region "EntityTypes"
                Dim lrXMLEntityType As XMLModel.EntityType
                Dim lrEntityType As FBM.EntityType

                For Each lrXMLEntityType In Me.ORMModel.EntityTypes

                    If abSkipAlreadyLoadedModelElements Then
                        'Really only ever called when using the UnifiedOntologyBrowser
                        '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
                        If lrModel.EntityType.Find(Function(x) x.Id = lrXMLEntityType.Id) IsNot Nothing Then GoTo SkipEntityType
                    End If

                    lrEntityType = New FBM.EntityType
                    lrEntityType.Model = lrModel
                    lrEntityType.Id = lrXMLEntityType.Id
                    lrEntityType.GUID = lrXMLEntityType.GUID
                    lrEntityType.Name = lrXMLEntityType.Name
                    lrEntityType.ReferenceMode = lrXMLEntityType.ReferenceMode
                    lrEntityType.HideReferenceMode = lrXMLEntityType.HideReferenceMode
                    lrEntityType.IsDerived = lrXMLEntityType.IsDerived
                    lrEntityType.DerivationText = lrXMLEntityType.DerivationText
                    'lrEntityType.ShortDescription = 
                    'lrEntityType.LongDescription = 
                    lrEntityType.IsObjectifyingEntityType = lrXMLEntityType.IsObjectifyingEntityType
                    lrEntityType.IsMDAModelElement = lrXMLEntityType.IsMDAModelElement
                    lrEntityType.IsAbsorbed = lrXMLEntityType.IsAbsorbed
                    lrEntityType.DBName = lrXMLEntityType.DBName
                    lrEntityType.ShortDescription = lrXMLEntityType.ShortDescription
                    lrEntityType.LongDescription = lrXMLEntityType.LongDescription
                    lrEntityType.ModelElementFlag = lrXMLEntityType.ModelElementFlags
                    lrEntityType.GraphLabel = lrXMLEntityType.GraphLabel.Select(Function(x) New RDS.GraphLabel With {.Model = lrModel, .ModelElement = lrEntityType, .ModelElementId = lrEntityType.Id, .ModelId = lrModel.ModelId, .Label = x}).ToList

                    If lrXMLEntityType.ReferenceModeValueTypeId = "" Then
                        lrEntityType.ReferenceModeValueType = Nothing
                    Else
                        lrEntityType.ReferenceModeValueType = New FBM.ValueType
                        lrEntityType.ReferenceModeValueType.Id = lrXMLEntityType.ReferenceModeValueTypeId
                        lrEntityType.ReferenceModeValueType = lrModel.ValueType.Find(Function(x) x.Id = lrXMLEntityType.ReferenceModeValueTypeId)

                        If lrEntityType.ReferenceModeValueType Is Nothing Then
                            lrModel.AddModelError(New FBM.ModelError(pcenumModelErrors.ModelLoadingError, "Model Element Not Found, " & lrXMLEntityType.ReferenceModeValueTypeId & "."))
                        End If
                    End If

                    lrEntityType.PreferredIdentifierRCId = lrXMLEntityType.ReferenceSchemeRoleConstraintId

                    '------------------------------------------------
                    'Link to the Concept within the ModelDictionary
                    '------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrEntityType.Name, pcenumConceptType.EntityType, lrEntityType.ShortDescription, lrEntityType.LongDescription, True, True, lrEntityType.DBName)
                    lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , False,, True,, True)

                    lrEntityType.Concept = lrDictionaryEntry.Concept

                    lrModel.EntityType.AddUnique(lrEntityType)
SkipEntityType:
                Next

                ' Build a dictionary for faster search
                Me.EntityTypeDictionary = lrModel.EntityType.ToDictionary(Function(x) x.Id)

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(64)
#End Region

                '==============================
                'Map the FactTypes
                '==============================
#Region "FactTypes"
                Dim lrXMLFactType As XMLModel.FactType
                Dim lrFactType As FBM.FactType

                For Each lrXMLFactType In Me.ORMModel.FactTypes

                    If abSkipAlreadyLoadedModelElements Then
                        'Really only ever called when using the UnifiedOntologyBrowser
                        '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
                        If lrModel.FactType.Find(Function(x) x.Id = lrXMLFactType.Id) IsNot Nothing Then GoTo SkipFactType
                    End If

                    lrFactType = New FBM.FactType(lrModel,
                                                  lrXMLFactType.Name,
                                                  lrXMLFactType.Id)
                    Call Me.GetFactTypeDetails(lrFactType, lrXMLFactType)
SkipFactType:
                Next

                'All Fact Types now exist, so deferred derivation roots can be resolved.
                Call Me.ResolveDerivationRolePathRoots(lrModel)

                ' Build a dictionary for faster search
                Me.FactTypeDictionary = lrModel.FactType.ToDictionary(Function(x) x.Id)

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(66)
#End Region

                '===============================================================================================
                'Populate Roles that are (still) joined to Nothing
                '===================================================
#Region "Role Cleanup"
                Dim latType = {GetType(FBM.ValueType),
                               GetType(FBM.EntityType),
                               GetType(FBM.FactType)}

                Dim larRole = From Role In lrModel.Role
                              Where Role.JoinedORMObject Is Nothing _
                              OrElse Not latType.Contains(Role.JoinedORMObject.GetType)
                              Select Role

                For Each lrRole In larRole

                    Dim lrXMLRole = From FactType In Me.ORMModel.FactTypes
                                    From Role In FactType.RoleGroup
                                    Where Role.Id = lrRole.Id
                                    Select Role

                    lrRole.JoinedORMObject = New FBM.ModelObject
                    lrRole.JoinedORMObject.Id = lrXMLRole(0).JoinedObjectTypeId

                    If lrModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals) IsNot Nothing Then
                        lrRole.JoinedORMObject = lrModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    ElseIf lrModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals) IsNot Nothing Then
                        lrRole.JoinedORMObject = lrModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    Else
                        lrRole.JoinedORMObject = lrModel.FactType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                    End If
                Next

#End Region
                '==============================
                'Subtype Relationships
                '==============================
#Region "Subtype Relationships"
                Dim larSubtypeRelationshipFactTypes = From FactType In Me.ORMModel.FactTypes
                                                      Where FactType.IsSubtypeRelationshipFactType
                                                      Select FactType

                Dim lrModelElement As FBM.ModelObject
                For Each lrXMLFactType In larSubtypeRelationshipFactTypes

                    Try

                        If abSkipAlreadyLoadedModelElements Then
                            'Really only ever called when using the UnifiedOntologyBrowser
                            '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
                            If lrModel.FactType.Find(Function(x) x.Id = lrXMLFactType.Id) IsNot Nothing Then GoTo SkipSubtypeRelationship
                        End If

                        lrFactType = New FBM.FactType(lrXMLFactType.Id, True)
                        lrFactType = lrModel.FactType.Find(AddressOf lrFactType.Equals)

                        Dim lrParentModelElement As FBM.ModelObject
                        lrModelElement = lrModel.GetModelObjectByName(lrFactType.RoleGroup(0).JoinedORMObject.Id)
                        lrParentModelElement = lrModel.GetModelObjectByName(lrFactType.RoleGroup(1).JoinedORMObject.Id)

                        If lrParentModelElement Is Nothing Then
                            lrModel.AddModelError(New FBM.ModelError(pcenumModelErrors.ModelLoadingError, "Model Element Not Found, " & lrFactType.RoleGroup(1).JoinedORMObject.Id & "."))
                            GoTo SkipSubtypeRelationship
                        End If

                        If lrParentModelElement.GetType = GetType(FBM.FactType) Then
                            lrParentModelElement = CType(lrParentModelElement, FBM.FactType).ObjectifyingEntityType

                            If lrParentModelElement Is Nothing Then
                                lsMessage = "Consider removing the Subtype Relationship: " & lrFactType.Id
                                lsMessage.AppendDoubleLineBreak("There is no Supertype, " & lrFactType.RoleGroup(1).JoinedORMObject.Id & ", in the model.")
                                Throw New Exception(lsMessage)
                            End If
                        End If
                        lrModelElement.parentModelObjectList.Add(lrParentModelElement)
                        lrParentModelElement.childModelObjectList.Add(lrModelElement)

                        Dim lrSubtypeConstraint As New FBM.SubtypeRelationship(lrModelElement, lrParentModelElement, lrFactType)

                        Select Case lrModelElement.GetType
                            Case Is = GetType(FBM.EntityType)
                                Dim larSubtypeRelationship = From EntityType In Me.ORMModel.EntityTypes
                                                             From SubtypeRelationship In EntityType.SubtypeRelationships
                                                             Where SubtypeRelationship.SubtypingFactTypeId = lrFactType.Id
                                                             Select SubtypeRelationship
                                Try
                                    lrSubtypeConstraint.IsPrimarySubtypeRelationship = larSubtypeRelationship.First.IsPrimarySubtypeRelationship
                                Catch ex As Exception
                                    'CodeSafe
                                    'Not a biggie at this stage.
                                End Try

                            Case Is = GetType(FBM.ValueType)
                                Dim larSubtypeRelationship = From ValueType In Me.ORMModel.ValueTypes
                                                             From SubtypeRelationship In ValueType.SubtypeRelationships
                                                             Where SubtypeRelationship.SubtypingFactTypeId = lrFactType.Id
                                                             Select SubtypeRelationship
                                Try
                                    lrSubtypeConstraint.IsPrimarySubtypeRelationship = larSubtypeRelationship.First.IsPrimarySubtypeRelationship
                                Catch ex As Exception
                                    'CodeSafe
                                    'Not a biggie at this stage.
                                End Try

                            Case Is = GetType(FBM.FactType)
                                Dim larSubtypeRelationship = From FactType In Me.ORMModel.FactTypes
                                                             From SubtypeRelationship In FactType.SubtypeRelationships
                                                             Where SubtypeRelationship.SubtypingFactTypeId = lrFactType.Id
                                                             Select SubtypeRelationship
                                Try
                                    lrSubtypeConstraint.IsPrimarySubtypeRelationship = larSubtypeRelationship.First.IsPrimarySubtypeRelationship
                                Catch ex As Exception
                                    'CodeSafe
                                    'Not a biggie at this stage.
                                End Try

                        End Select

                        lrModelElement.SubtypeRelationship.AddUnique(lrSubtypeConstraint)

                    Catch ex As Exception
                        Dim lsMessage1 As String
                        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                        lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                        lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                        prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Warning, , False,,,, True)
                    End Try
SkipSubtypeRelationship:
                Next

#End Region

                '==============================
                'Map the RoleConstraints
                '==============================
#Region "Role Constraints"
                Dim lrXMLRoleConstraint As XMLModel.RoleConstraint
                Dim lrRoleConstraint As FBM.RoleConstraint

                For Each lrXMLRoleConstraint In Me.ORMModel.RoleConstraints

                    Try
                        If abSkipAlreadyLoadedModelElements Then
                            'Really only ever called when using the UnifiedOntologyBrowser
                            '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
                            If lrModel.RoleConstraint.Find(Function(x) x.Id = lrXMLRoleConstraint.Id) IsNot Nothing Then GoTo SkipRoleConstraint
                        End If

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
                        lrRoleConstraint.ShortDescription = lrXMLRoleConstraint.ShortDescription
                        lrRoleConstraint.LongDescription = lrXMLRoleConstraint.LongDescription

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
                            Try
                                lrRoleConstraintRole = New FBM.RoleConstraintRole
                                lrRoleConstraintRole.Model = lrModel
                                lrRoleConstraintRole.RoleConstraint = lrRoleConstraint
                                lrRoleConstraintRole.SequenceNr = lrXMLRoleConstraintRole.SequenceNr
                                lrRoleConstraintRole.IsEntry = lrXMLRoleConstraintRole.IsEntry
                                lrRoleConstraintRole.IsExit = lrXMLRoleConstraintRole.IsExit
                                lrRoleConstraintRole.ArgumentSequenceNr = lrXMLRoleConstraintRole.ArgumentSequenceNr

                                lrRole.Id = lrXMLRoleConstraintRole.RoleId

                                lrRoleConstraintRole.Role = lrModel.Role.Find(AddressOf lrRole.Equals)

                                'CodeSafe
                                If lrRoleConstraintRole.Role Is Nothing Then
                                    'Not a good situation, but must deal with it
                                    lsMessage = "Note: Role Link unidentifiable in Role Constraint: " & lrRoleConstraint.Id
                                    lsMessage.AppendDoubleLineBreak("Removing the Role Link from the Model.")
                                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, , False,,,, True,,)
                                    Continue For
                                Else
                                    '-------------------------------------------------------------------
                                    'lrRoleConstraintRole.RoleConstraintArgument is set further below.
                                    '-------------------------------------------------------------------
                                    lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                                    lrRoleConstraint.Role.Add(lrRoleConstraintRole.Role)
                                    lrRoleConstraintRole.Role.RoleConstraintRole.Add(lrRoleConstraintRole)
                                End If

                            Catch ex As Exception
                                prApplication.ThrowMessage("Error loading RoleConsraintRole for RoleConstraint," & lrXMLRoleConstraint.Id, pcenumErrorType.Warning, ex.StackTrace, True, False, False,, True, ex)
                            End Try
SkipRoleConstraintRole:
                        Next

                        If lrRoleConstraint.RoleConstraintRole.Count = 0 Then
                            prApplication.ThrowMessage("No RoleConstraintRoles found for RoleConstraint.Id: " & lrRoleConstraint.Id, pcenumErrorType.Information)
                        Else
                            Try
                                Select Case lrRoleConstraint.RoleConstraintType
                                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint

                                        larFactType = (From Role In lrRoleConstraint.Role
                                                       Where Role IsNot Nothing
                                                       Select Role.FactType).ToList

                                        If larFactType.Count = 0 Then
                                            Throw New Exception("Error: No Role exists for Role Constraint: " & lrRoleConstraint.Id)
                                        Else
                                            lrFactType = larFactType.First
                                            Call lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)
                                        End If

                                End Select
                            Catch ex As Exception
                                lsMessage = "Error finding FactType for a Role of Role Constraint: " & lrRoleConstraint.Id
                                lsMessage.AppendDoubleLineBreak(ex.Message)
                                Throw New Exception(lsMessage)
                            End Try
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
                            lrModel.RoleConstraint.AddUnique(lrRoleConstraint)
                        End If

                        For Each lsValueTypeConstraintValue In lrXMLRoleConstraint.ValueConstraint
                            lrRoleConstraint.ValueConstraint.Add(lsValueTypeConstraintValue)
                        Next
SkipRoleConstraint:
                    Catch ex As Exception
                        prApplication.ThrowMessage("Error loading Role Constraint: " & lrXMLRoleConstraint.Id, pcenumErrorType.Warning, ex.StackTrace, True, False, False,, True, ex)
                    End Try
            Next 'RoleConstraint

                ' Build a dictionary for faster search
                Me.RoleConstraintDictionary = lrModel.RoleConstraint.ToDictionary(Function(x) x.Id)
#End Region

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(68)

                '-----------------------------------------------------------------------------
                'Set the ReferenceMode ObjectTypes for each of the EntityTypes in the Model
                '-----------------------------------------------------------------------------            
                For Each lrEntityType In lrModel.EntityType

                    If abSkipAlreadyLoadedModelElements And lrEntityType.PreferredIdentifierRCId = "" Then GoTo SkipEntityTypeSetReferenceSchemeObjects

                    Call lrEntityType.SetReferenceModeObjects()

SkipEntityTypeSetReferenceSchemeObjects:
                Next

                '==============================
                'Map the ModelNotes
                '==============================
#Region "Model Notes"
                Dim lrXMLModelNote As XMLModel.ModelNote
                Dim lrModelNote As FBM.ModelNote

                For Each lrXMLModelNote In Me.ORMModel.ModelNotes

                    If abSkipAlreadyLoadedModelElements Then
                        'Really only ever called when using the UnifiedOntologyBrowser
                        '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
                        If lrModel.ModelNote.Find(Function(x) x.Id = lrXMLModelNote.Id) IsNot Nothing Then GoTo SkipModelNote
                    End If

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
SkipModelNote:
                Next
#End Region

                '==========================
                'Map Functions (per nORMa)
                '==========================
#Region "Functions"
                Dim lrFunction As FBM.Function
                Dim lrXMLFunction As XMLModel.Function

                For Each lrXMLFunction In Me.ORMModel.Functions

                    lrFunction = New FBM.Function

                    lrFunction.id = lrXMLFunction.Id
                    lrFunction.Name = lrXMLFunction.Name
                    lrFunction.OperatorSymbol = lrXMLFunction.OperatorSymbol
                    lrFunction.IsBoolean = lrXMLFunction.IsBoolean

                    For Each lrXMLParameter In lrXMLFunction.Parameters

                        Dim lrParameter = New FBM.Parameter

                        lrParameter.id = lrXMLParameter.Id
                        lrParameter.Name = lrXMLParameter.Name
                        lrParameter.BagInput = lrXMLParameter.BagInput

                        lrFunction.Parameters.Add(lrParameter)

                    Next

                    lrModel.Function.Add(lrFunction)
                Next

#End Region

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(70)

                '======================
                'Map the Synonyms
                '======================
#Region "Synonymns"
                For Each lrXMLSynonym In Me.ORMModel.Synonyms

                    lrModelElement = arModel.GetModelObjectByName(lrXMLSynonym.ModelElementId, True)
                    If lrModelElement IsNot Nothing Then
                        Dim lrSynonym As New FBM.Synonym(lrModelElement, lrXMLSynonym.Synonym)
                        arModel.Synonyms.Add(lrSynonym)
                    End If

                Next
#End Region

                '----------------------------------------------------------------------------------------------
                'Reference any FactType.LinkFactTypeRole values that are NOTHING
                larFactType = (From [FactType] In lrModel.FactType
                               Where FactType.LinkFactTypeRole Is Nothing _
                               And FactType.IsLinkFactType = True
                               Select FactType).ToList

                For Each lrFactType In larFactType
                    MsgBox("Error: FactType: '" & lrFactType.Name & "' has no LinkFactTypeRole.")
                Next

#Region "Extra Mappings"

#Region "Objectifying Fact Types for IsIdendependent Value Types"
                Dim larObjectifyingFactType = From [FactType] In lrModel.FactType
                                              From ValueType In lrModel.ValueType.FindAll(Function(x) x.IsIndependent)
                                              Where ValueType.ObjectifyingFactTypeId IsNot Nothing
                                              Where ValueType.ObjectifyingFactTypeId = FactType.Id
                                              Select FactType

                For Each lrFactType In larObjectifyingFactType
                    lrFactType.IsObjectifyingFactType = True
                Next
#End Region

#Region "Derivation Text"

                Dim larDerivedFactType = From FactType In lrModel.FactType
                                         Where FactType.IsDerived
                                         Where FactType.DerivationText.Trim = ""
                                         Select FactType

                For Each lrDerivedFactType In larDerivedFactType

                    lrDerivedFactType.DerivationText = BostonDerivationRenderer.RenderDerivationEnglish(lrModel, lrDerivedFactType)

                Next
#End Region

#End Region

                '=====================
                'Map the Pages
                '=====================
                Call Me.MapToFBMPages(lrModel, aoBackgroundWorker)
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(80)

                lrModel.Loaded = True
                lrModel.LoadedFromXMLFile = True
                Return lrModel

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arModel"></param>
        ''' <param name="aoBackgroundWorker"></param>
        Public Sub MapToFBMPages(ByRef arModel As FBM.Model,
                                 Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Dim lrPage As FBM.Page
            'Dim lrXMLPage As XMLModel.Page
            Dim lrModel As FBM.Model = arModel
            Dim loBackgroundWorker As System.ComponentModel.BackgroundWorker = aoBackgroundWorker
            Try
                If My.Settings.ModelingUseThreadedXMLPageLoading Then
                    Parallel.ForEach(Me.ORMDiagram,
                                     Sub(lrXMLPage As XMLModel.Page)
                                         lrPage = lrModel.Page.Find(Function(x) x.PageId = lrXMLPage.Id)
                                         If lrPage Is Nothing Then
                                             lrPage = Me.MapToFBMPage(lrXMLPage, lrModel,,, True)
                                         Else
                                             If Not lrPage.Loaded Then
                                                 Call Me.MapToFBMPage(lrXMLPage, lrModel, lrPage, loBackgroundWorker, True)
                                             End If
                                         End If

                                         lrPage.Loaded = True
                                         lrPage.IsDirty = True
                                         lrModel.Page.AddUnique(lrPage)

                                     End Sub)

                Else

                    For Each lrXMLPage In Me.ORMDiagram
                        lrPage = lrModel.Page.Find(Function(x) x.PageId = lrXMLPage.Id)
                        If lrPage Is Nothing Then
                            lrPage = Me.MapToFBMPage(lrXMLPage, lrModel,,,, True)
                        Else
                            Call Me.MapToFBMPage(lrXMLPage, lrModel, lrPage, loBackgroundWorker, False, True)
                        End If

                        lrPage.Loaded = False
                        lrPage.IsDirty = True
                        lrModel.Page.AddUnique(lrPage)
                    Next 'XMLModel.Page
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        <MethodImplAttribute(MethodImplOptions.Synchronized)>
        Public Function MapToFBMPage(ByRef arXMLPage As XMLModel.Page,
                                      ByRef arModel As FBM.Model,
                                      Optional ByRef arPage As FBM.Page = Nothing,
                                      Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing,
                                      Optional ByVal abCalledAsThread As Boolean = False,
                                      Optional abCopyConceptInstancesOnly As Boolean = False) As FBM.Page

            Dim lsMessage As String

            Try
                Dim lrConceptInstance As FBM.ConceptInstance
                Dim lrPage As FBM.Page

                If Not abCalledAsThread Then
                    Boston.WriteToStatusBar("Loading Model. Mapping Page: " & arXMLPage.Name & ".", True)
                Else
                    Boston.WriteToStatusBar("Loading Model. Page: " & arXMLPage.Name & ".", True)
                End If

                If arPage Is Nothing Then
                    lrPage = New FBM.Page(arModel,
                                          arXMLPage.Id,
                                          arXMLPage.Name,
                                          arXMLPage.Language)
                Else
                    'Even though passed, was adding instances to another page when using Paralle.ForEach
                    Dim lsPageId = arXMLPage.Id
                    lrPage = arModel.Page.Find(Function(x) x.PageId = lsPageId)
                End If

                'Make sure Language is captured.
                lrPage.Language = arXMLPage.Language

                If abCopyConceptInstancesOnly Then
                    lrPage.ConceptInstance = arXMLPage.ConceptInstance
                    lrPage.Loaded = False
                    Return lrPage
                End If

                '=============================
                'Map the ValueTypeInstances
                '=============================
#Region "ValueTypeInstances"
                Dim lrValueTypeInstance As FBM.ValueTypeInstance
                Dim lrValueType As FBM.ValueType = Nothing

                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.ValueType)
                    Try
                        lrValueTypeInstance = New FBM.ValueTypeInstance
                        lrValueTypeInstance.Model = arModel
                        lrValueTypeInstance.Page = lrPage
                        lrValueTypeInstance.Id = lrConceptInstance.Symbol
                        ' Perform the search using the dictionary
                        Try
                            Me.ValueTypeDictionary.TryGetValue(lrConceptInstance.Symbol, lrValueType) '20230602-CM-Was .ValueType = arModel.ValueType.Find(Function(x) x.Id = lrConceptInstance.Symbol)
                        Catch
                            lrValueType = lrPage.Model.ValueType.Find(Function(x) x.Id = lrConceptInstance.Symbol)
                            If lrValueType Is Nothing Then
                                Throw New Exception("Could not find Value Type, " & lrConceptInstance.Symbol & ", in the Model.")
                            End If
                        End Try
                        lrValueTypeInstance.ValueType = lrValueType
                            lrValueTypeInstance.DataType = lrValueTypeInstance.ValueType.DataType
                            lrValueTypeInstance.DataTypeLength = lrValueTypeInstance.ValueType.DataTypeLength
                            lrValueTypeInstance.DataTypePrecision = lrValueTypeInstance.ValueType.DataTypePrecision
                            lrValueTypeInstance.InstanceNumber = lrConceptInstance.InstanceNumber
                        lrValueTypeInstance.ValueConstraint = lrValueTypeInstance.ValueType.ValueConstraint.Clone
                        lrValueTypeInstance.IsIndependent = lrValueTypeInstance.ValueType.IsIndependent

                        lrValueTypeInstance.Name = lrConceptInstance.Symbol
                            lrValueTypeInstance.DBName = lrValueType.DBName
                            lrValueTypeInstance.X = lrConceptInstance.X
                            lrValueTypeInstance.Y = lrConceptInstance.Y

                            lrPage.ValueTypeInstance.Add(lrValueTypeInstance)
                        Catch ex As Exception
                            Call Me.ReportModelLoadingError(arModel, ex.Message)
                        GoTo SkipValueTypeInstance
                    End Try
SkipValueTypeInstance:
                Next
#End Region

                '=============================
                'Map the EntityTypeInstances
                '=============================
#Region "EntityTypeInstances"
                Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                Dim lrEntityType As FBM.EntityType = Nothing

                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.EntityType)
                    lrEntityTypeInstance = New FBM.EntityTypeInstance
                    lrEntityTypeInstance.Model = arModel
                    lrEntityTypeInstance.Page = lrPage
                    lrEntityTypeInstance.Id = lrConceptInstance.Symbol
                    ' Perform the search using the dictionary                                                    
                    Try
                        Me.EntityTypeDictionary.TryGetValue(lrConceptInstance.Symbol, lrEntityType) '20230602-CM-Was lrEntityType = arModel.EntityType.Find(Function(x) x.Id = lrEntityTypeInstance.Id)
                    Catch
                        lrEntityType = lrPage.Model.EntityType.Find(Function(x) x.Id = lrConceptInstance.Symbol)
                        If lrEntityType Is Nothing Then
                            Throw New Exception("Could not find Entity Type, " & lrConceptInstance.Symbol & ", in the Model.")
                        End If
                    End Try
                    If lrEntityType Is Nothing Then GoTo SkipEntityTypeInstance
                    lrEntityTypeInstance.EntityType = lrEntityType
                    lrEntityTypeInstance._Name = lrEntityTypeInstance.Id
                    lrEntityTypeInstance.ReferenceMode = lrEntityType.ReferenceMode
                    lrEntityTypeInstance.HideReferenceMode = lrConceptInstance.ConceptInstanceFlag.Find(Function(x) x.Flag = pcenumConceptInstanceFlag.EntityTypeHideReferenceMode) IsNot Nothing
                    lrEntityTypeInstance.IsObjectifyingEntityType = lrEntityType.IsObjectifyingEntityType
                    lrEntityTypeInstance.IsAbsorbed = lrEntityType.IsAbsorbed
                    lrEntityTypeInstance.IsDerived = lrEntityType.IsDerived
                    lrEntityTypeInstance.DerivationText = lrEntityType.DerivationText
                    lrEntityTypeInstance.DBName = lrEntityType.DBName
                    lrEntityTypeInstance.InstanceNumber = lrConceptInstance.InstanceNumber

                    If lrEntityTypeInstance.EntityType.ReferenceModeValueType IsNot Nothing Then
                        lrEntityTypeInstance.ReferenceModeValueType = lrPage.ValueTypeInstance.Find(Function(x) x.Id = lrEntityType.ReferenceModeValueType.Id)
                    End If

                    lrEntityTypeInstance.PreferredIdentifierRCId = lrEntityType.PreferredIdentifierRCId

                    lrEntityTypeInstance.Visible = lrConceptInstance.Visible
                    lrEntityTypeInstance.X = lrConceptInstance.X
                    lrEntityTypeInstance.Y = lrConceptInstance.Y

                    lrPage.EntityTypeInstance.Add(lrEntityTypeInstance)
SkipEntityTypeInstance:
                Next
#End Region

                '===========================
                'Map the FactTypeInstances
                '===========================
#Region "FactTypeInstances"
                Dim lrFactTypeInstance As FBM.FactTypeInstance
                Dim lrDerivationTextConceptInstance As FBM.ConceptInstance
                Dim lrFactTypeReadingConceptInstance As FBM.ConceptInstance
                Dim lrFactTypeNameConceptInstance As FBM.ConceptInstance
                Dim lrFactType As FBM.FactType = Nothing

                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.FactType)
                    ' Perform the search using the dictionary                            
                    Try
                        Me.FactTypeDictionary.TryGetValue(lrConceptInstance.Symbol, lrFactType) '20230602-CM-Was lrFactType = arModel.FactType.Find(Function(x) x.Id = lrConceptInstance.Symbol)
                    Catch
                        lrFactType = lrPage.Model.FactType.Find(Function(x) x.Id = lrConceptInstance.Symbol)
                        If lrFactType Is Nothing Then
                            Throw New Exception("Could not find Fact Type, " & lrConceptInstance.Symbol & ", in the Model.")
                        End If
                    End Try
                    If lrFactType Is Nothing Then GoTo SkipFactTypeInstance
                    lrFactTypeInstance = lrFactType.CloneInstance(lrPage, True,, lrConceptInstance.InstanceNumber)
                    lrFactTypeInstance.X = lrConceptInstance.X
                    lrFactTypeInstance.Y = lrConceptInstance.Y
                    lrFactTypeInstance.DBName = lrFactType.DBName
                    lrFactTypeInstance._Visible = lrConceptInstance.Visible

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

                    lrFactTypeNameConceptInstance = arXMLPage.ConceptInstance.Find(Function(x) x.ConceptType = pcenumConceptType.FactTypeName And x.Symbol = lrFactType.Id)
                    If lrFactTypeNameConceptInstance IsNot Nothing Then
                        lrFactTypeInstance.FactTypeName = New FBM.FactTypeName(lrPage.Model, lrPage, lrFactTypeInstance, lrFactTypeInstance.Name)
                        lrFactTypeInstance.FactTypeName.X = lrFactTypeNameConceptInstance.X
                        lrFactTypeInstance.FactTypeName.Y = lrFactTypeNameConceptInstance.Y
                        lrFactTypeInstance.ShowFactTypeName = lrFactTypeNameConceptInstance.Visible
                    End If

                    '----------------------------------------
                    'Get the Facts for the FactTypeInstance
                    '----------------------------------------
                    Dim lrFactInstance As FBM.FactInstance
                    Dim lrFactDataInstance As FBM.FactDataInstance

                    Dim lrXMLPage = arXMLPage
                    Dim larFact = (From Fact In lrFactType.Fact
                                   From ConceptInstance In lrXMLPage.ConceptInstance
                                   Where ConceptInstance.ConceptType = pcenumConceptType.Fact
                                   Where Fact.Id = ConceptInstance.Symbol
                                   Select New With {.Fact = Fact, .X = ConceptInstance.X, .Y = ConceptInstance.Y}).ToList

                    For Each lrFactConceptInstance In larFact

                        'Dim lrFactConceptInstance = arXMLPage.ConceptInstance.Find(Function(x) x.Symbol = lrFact.Id And x.ConceptType = pcenumConceptType.Fact)

                        '----------------------------------
                        'The Fact is included on the Page
                        '----------------------------------
                        lrFactInstance = lrFactConceptInstance.Fact.CloneInstance(lrPage)
                        lrFactInstance.X = lrFactConceptInstance.X
                        lrFactInstance.Y = lrFactConceptInstance.Y
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
                            lrPage.ValueInstance.AddUnique(lrFactDataInstance)
                        Next
                    Next

                    'FactTable
                    Dim lrFactTableInstance = arXMLPage.ConceptInstance.Find(Function(x) x.ConceptType = pcenumConceptType.FactTable And x.Symbol = lrFactTypeInstance.Id)
                    If lrFactTableInstance IsNot Nothing Then
                        lrFactTypeInstance.FactTable.X = lrFactTableInstance.X
                        lrFactTypeInstance.FactTable.Y = lrFactTableInstance.Y
                        lrFactTypeInstance.FactTable.Visible = True
                    End If
SkipFactTypeInstance:
                Next
#End Region

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
                Dim larSubtypeRelationshipFactTypes = (From FactType In lrPage.FactTypeInstance
                                                       Where FactType.IsSubtypeRelationshipFactType
                                                       Select FactType).Distinct

                Dim lrModelElementInstance As FBM.ModelObject
                Dim lrParentModelElementInstance As FBM.ModelObject
                Dim lrSubtypeRelationship As FBM.SubtypeRelationship
                Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance
                For Each lrFactTypeInstance In larSubtypeRelationshipFactTypes.ToArray
                    Try
                        For Each lrModelElementInstance In lrPage.GetAllPageObjects(False, False, lrFactTypeInstance.RoleGroup(0).JoinedORMObject, True)
                            lrParentModelElementInstance = lrPage.getModelElement(lrFactTypeInstance.RoleGroup(1).JoinedORMObject)

                            If lrParentModelElementInstance IsNot Nothing Then
                                If lrParentModelElementInstance.ConceptType = pcenumConceptType.FactType Then
                                    Dim lsParentModelElementInstanceId = CType(lrParentModelElementInstance, FBM.FactTypeInstance).FactType.ObjectifyingEntityType.Id
                                    lrParentModelElementInstance = lrPage.EntityTypeInstance.Find(Function(x) x.Id = lsParentModelElementInstanceId)
                                End If

                                lrSubtypeRelationship = Nothing
                                Select Case lrModelElementInstance.ConceptType
                                    Case Is = pcenumConceptType.EntityType
                                        lrSubtypeRelationship = CType(lrModelElementInstance, FBM.EntityTypeInstance).EntityType.SubtypeRelationship.Find(Function(x) x.ModelElement.Id = lrModelElementInstance.Id And x.parentModelElement.Id = lrParentModelElementInstance.Id)
                                        If lrSubtypeRelationship Is Nothing And CType(lrModelElementInstance, FBM.EntityTypeInstance).EntityType.IsObjectifyingEntityType Then
                                            lrFactType = lrPage.Model.FactType.Find(Function(X) X.Id = lrModelElementInstance.Id)

                                            lrSubtypeRelationship = lrFactType.SubtypeRelationship.Find(Function(x) x.ModelElement.Id = lrModelElementInstance.Id And x.parentModelElement.Id = lrParentModelElementInstance.Id)
                                        End If
                                    Case Is = pcenumConceptType.ValueType
                                        lrSubtypeRelationship = CType(lrModelElementInstance, FBM.ValueTypeInstance).ValueType.SubtypeRelationship.Find(Function(x) x.ModelElement.Id = lrModelElementInstance.Id And x.parentModelElement.Id = lrParentModelElementInstance.Id)
                                    Case Is = pcenumConceptType.FactType
                                        lrSubtypeRelationship = CType(lrModelElementInstance, FBM.FactTypeInstance).FactType.SubtypeRelationship.Find(Function(x) x.ModelElement.Id = lrModelElementInstance.Id And x.parentModelElement.Id = lrParentModelElementInstance.Id)
                                End Select

                                lrSubtypeRelationshipInstance = lrSubtypeRelationship.CloneInstance(lrPage, True)
                                lrSubtypeRelationshipInstance.ModelElement = lrModelElementInstance
                                lrSubtypeRelationshipInstance.ParentModelElement = lrParentModelElementInstance
                                lrSubtypeRelationshipInstance.Visible = lrSubtypeRelationshipInstance.FactType.Visible

                                Select Case lrModelElementInstance.ConceptType
                                    Case Is = pcenumConceptType.EntityType
                                        CType(lrModelElementInstance, FBM.EntityTypeInstance).SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)
                                    Case Is = pcenumConceptType.ValueType
                                        CType(lrModelElementInstance, FBM.ValueTypeInstance).SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)
                                    Case Is = pcenumConceptType.FactType
                                        CType(lrModelElementInstance, FBM.FactTypeInstance).SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)
                                End Select
                            End If
                        Next

                    Catch ex As Exception
                        lsMessage = "Problem loading Subtype Relationship for Subtype Relationship Fact Type with Id: " & lrFactTypeInstance.Id
                        lsMessage.AppendDoubleLineBreak("Page: " & lrPage.Name)
                        If abCalledAsThread Then
                            Dim lrModelError As New FBM.ModelError(pcenumModelErrors.ModelLoadingError,
                                                           lsMessage,
                                                           Nothing,
                                                           Nothing)
                            arModel._ModelError.Add(lrModelError)
                        Else
                            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                        End If

                    End Try
                Next
#End Region

                '===========================
                'Map the RoleNameInstances
                '===========================
                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleName)
                    lrRoleInstance = lrPage.RoleInstance.Find(Function(x) x.Id = lrConceptInstance.RoleId)
                    Try
                        lrRoleInstance.RoleName = New FBM.RoleName(lrRoleInstance, lrRoleInstance.Name)
                        lrRoleInstance.RoleName.X = lrConceptInstance.X
                        lrRoleInstance.RoleName.Y = lrConceptInstance.Y
                    Catch ex As Exception
                        'Not worth crashing over.
                    End Try
                Next

                '=================================
                'Map the RoleConstraintInstances
                '=================================
#Region "Role Constraints"
                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                Dim lrRoleConstraint As FBM.RoleConstraint = Nothing

                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleConstraint)

                    If lrPage.RoleConstraintInstance.Find(Function(x) x.Id = lrConceptInstance.Symbol) IsNot Nothing Then
                        '-------------------------------------------------------------------
                        'The RoleConstraintInstance has already been added to the Page.
                        '  FactType.CloneInstance adds RoleConstraintInstances to the Page
                        '-------------------------------------------------------------------
                    Else
                        ' Perform the search using the dictionary                                
                        Try
                            Me.RoleConstraintDictionary.TryGetValue(lrConceptInstance.Symbol, lrRoleConstraint) '20230602-CM-Was lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.Id = lrConceptInstance.Symbol)
                        Catch
                            lrRoleConstraint = lrPage.Model.RoleConstraint.Find(Function(x) x.Id = lrConceptInstance.Symbol)
                        End Try

                        'CodeSafe
                        If lrRoleConstraint Is Nothing Then
                            lsMessage = "Could not find Role Constraint, " & lrConceptInstance.Symbol & ", in the Model. Page: " & lrPage.Name
                            lsMessage.AppendLine("Removing from Page.")
                            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,,,,,, True,,)
                            Continue For
                        End If

                        Try
                            lrRoleConstraintInstance = Nothing

                            Select Case lrRoleConstraint.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint

                                    '20220716-VM-Can likely remove. All for multiple instances of the FactType on the Page.
                                    For Each lrFactTypeInstance In lrPage.FactTypeInstance.FindAll(Function(x) x.Id = lrRoleConstraint.Role(0).FactType.Id)
                                        lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage, False, lrFactTypeInstance)
                                        lrFactTypeInstance.AddInternalUniquenessConstraint(lrRoleConstraintInstance)
                                    Next

                                Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                    lrRoleConstraintInstance = lrRoleConstraint.CloneFrequencyConstraintInstance(lrPage)
                                    lrRoleConstraintInstance.X = lrConceptInstance.X
                                    lrRoleConstraintInstance.Y = lrConceptInstance.Y

                                Case Is = pcenumRoleConstraintType.RoleValueConstraint
                                    lrRoleConstraintInstance = lrRoleConstraint.CloneRoleValueConstraintInstance(lrPage)
                                    lrRoleConstraintInstance.X = lrConceptInstance.X
                                    lrRoleConstraintInstance.Y = lrConceptInstance.Y

                                Case Else
                                    lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)
                                    lrRoleConstraintInstance.X = lrConceptInstance.X
                                    lrRoleConstraintInstance.Y = lrConceptInstance.Y

                            End Select

                            lrPage.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance)
                        Catch ex As Exception
                            lsMessage = "Error loading Role Constraint with Id: " & lrConceptInstance.Symbol
                            lsMessage.AppendDoubleLineBreak("Page: " & arXMLPage.Name)
                            If abCalledAsThread Then
                                Dim lrModelError As New FBM.ModelError(pcenumModelErrors.ModelLoadingError,
                                                           lsMessage,
                                                           Nothing,
                                                           Nothing)
                                arModel._ModelError.Add(lrModelError)
                            Else
                                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                            End If
                        End Try

                    End If
                Next
#End Region

                '============================
                'Map the ModelNoteInstances
                '============================
#Region "Mode Notes"
                Dim lrModelNoteInstance As FBM.ModelNoteInstance
                Dim lrModelNote As FBM.ModelNote
                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.ModelNote)
                    Try
                        lrModelNote = arModel.ModelNote.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                        lrModelNoteInstance = lrModelNote.CloneInstance(lrPage, True)
                        lrModelNoteInstance.X = lrConceptInstance.X
                        lrModelNoteInstance.Y = lrConceptInstance.Y
                        lrModelNoteInstance.Width = lrConceptInstance.Width
                        lrModelNoteInstance.Height = lrConceptInstance.Height

                    Catch ex As Exception
                        lsMessage = "Error loading Model Note with Id: " & lrConceptInstance.Symbol
                        lsMessage.AppendDoubleLineBreak("Page: " & arXMLPage.Name)
                        If abCalledAsThread Then
                            Dim lrModelError As New FBM.ModelError(pcenumModelErrors.ModelLoadingError,
                                                           lsMessage,
                                                           Nothing,
                                                           Nothing)
                            arModel._ModelError.Add(lrModelError)
                        Else
                            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                        End If
                    End Try

                Next
#End Region

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(70 + CInt(9 * (arModel.Page.FindAll(Function(x) x.Loaded = True).Count / arModel.Page.Count)))

FinishedPage:

                lrPage.Loaded = True
                lrPage.Loaded = False

                Return lrPage

            Catch ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                If abCalledAsThread Then
                    Dim lrModelError As New FBM.ModelError(pcenumModelErrors.ModelLoadingError,
                                                           lsMessage,
                                                           Nothing,
                                                           Nothing)
                    arModel._ModelError.Add(lrModelError)
                Else
                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                End If
                Return Nothing

            End Try
        End Function

#Region "NORMA New 20221128"
        ''' <summary>
        ''' Maps an instance of this class to an instance of FBM.Model
        ''' </summary>
        ''' <param name="arNORMADocument">The FBM Model to map to.</param>
        ''' <param name="aoBackgroundWorker">For reporting progress. Start at 60%.</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MapToNORMAORMModel(Optional ByRef arNORMADocument As NORMA.ORMDocument = Nothing,
                                           Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing
                                           ) As NORMA.ORMDocument

            Try

                Dim lsMessage As String = ""
                Dim lrORMDocument As New NORMA.ORMDocument
                Dim lrNORMAModel As New NORMA.ORMModel

                'Variables for the FBM model element types.
                Dim lrFBMValueType As XMLModel.ValueType = Nothing
                Dim lrFBMEntityType As XMLModel.EntityType
                Dim lrFBMFactType As XMLModel.FactType
                Dim lrFBMRoleConstraint As XMLModel.RoleConstraint
                Dim lrFBMRoleConstraintRole As XMLModel.RoleConstraintRole

                If arNORMADocument IsNot Nothing Then
                    lrORMDocument = arNORMADocument
                    lrNORMAModel = arNORMADocument.ORMModel
                    lrNORMAModel.Id = "_" & Me.ORMModel.ModelId.ToUpper()
                    lrNORMAModel.Name = Me.ORMModel.Name
                Else
                    lrNORMAModel = New NORMA.ORMModel(Me.ORMModel.ModelId.ToUpper(), Me.ORMModel.Name)
                End If

                '==============================
                'Create the DataTypes
                '==============================
#Region "DataTypes"
                lrORMDocument.ORMModel.DataTypes = New NORMA.ORMModelDataTypes

                lrORMDocument.ORMModel.DataTypes.UnspecifiedDataType = New NORMA.Model.UnspecifiedDataType
                lrORMDocument.ORMModel.DataTypes.AutoCounterNumericDataType = New NORMA.Model.AutoCounterNumericDataType()
                lrORMDocument.ORMModel.DataTypes.FixedLengthTextDataType = New NORMA.Model.FixedLengthTextDataType()
                lrORMDocument.ORMModel.DataTypes.DateTemporalDataType = New NORMA.Model.DateTemporalDataType()
                lrORMDocument.ORMModel.DataTypes.VariableLengthTextDataType = New NORMA.Model.VariableLengthTextDataType
                lrORMDocument.ORMModel.DataTypes.LargeLengthTextDataType = New NORMA.Model.LargeLengthTextDataType
                lrORMDocument.ORMModel.DataTypes.SignedIntegerNumericDataType = New NORMA.Model.SignedIntegerNumericDataType
                lrORMDocument.ORMModel.DataTypes.SignedSmallIntegerNumericDataType = New NORMA.Model.SignedSmallIntegerNumericDataType
                lrORMDocument.ORMModel.DataTypes.SignedLargeIntegerNumericDataType = New NORMA.Model.SignedLargeIntegerNumericDataType
                lrORMDocument.ORMModel.DataTypes.UnsignedTinyIntegerNumericDataType = New NORMA.Model.UnsignedTinyIntegerNumericDataType
                lrORMDocument.ORMModel.DataTypes.UnsignedSmallIntegerNumericDataType = New NORMA.Model.UnsignedSmallIntegerNumericDataType
                lrORMDocument.ORMModel.DataTypes.UnsignedIntegerNumericDataType = New NORMA.Model.UnsignedIntegerNumericDataType
                lrORMDocument.ORMModel.DataTypes.AutoCounterNumericDataType = New NORMA.Model.AutoCounterNumericDataType
                lrORMDocument.ORMModel.DataTypes.FloatingPointNumericDataType = New NORMA.Model.FloatingPointNumericDataType
                lrORMDocument.ORMModel.DataTypes.SinglePrecisionFloatingPointNumericDataType = New NORMA.Model.SinglePrecisionFloatingPointNumericDataType
                lrORMDocument.ORMModel.DataTypes.DoublePrecisionFloatingPointNumericDataType = New NORMA.Model.DoublePrecisionFloatingPointNumericDataType
                lrORMDocument.ORMModel.DataTypes.DecimalNumericDataType = New NORMA.Model.DecimalNumericDataType
                lrORMDocument.ORMModel.DataTypes.MoneyNumericDataType = New NORMA.Model.MoneyNumericDataType
                lrORMDocument.ORMModel.DataTypes.FixedLengthRawDataDataType = New NORMA.Model.FixedLengthRawDataDataType
                lrORMDocument.ORMModel.DataTypes.VariableLengthRawDataDataType = New NORMA.Model.VariableLengthRawDataDataType
                lrORMDocument.ORMModel.DataTypes.LargeLengthRawDataDataType = New NORMA.Model.LargeLengthRawDataDataType
                lrORMDocument.ORMModel.DataTypes.PictureRawDataDataType = New NORMA.Model.PictureRawDataDataType
                lrORMDocument.ORMModel.DataTypes.OleObjectRawDataDataType = New NORMA.Model.OleObjectRawDataDataType
                lrORMDocument.ORMModel.DataTypes.AutoTimestampTemporalDataType = New NORMA.Model.AutoTimestampTemporalDataType
                lrORMDocument.ORMModel.DataTypes.TimeTemporalDataType = New NORMA.Model.TimeTemporalDataType
                lrORMDocument.ORMModel.DataTypes.DateTemporalDataType = New NORMA.Model.DateTemporalDataType
                lrORMDocument.ORMModel.DataTypes.DateAndTimeTemporalDataType = New NORMA.Model.DateAndTimeTemporalDataType
                lrORMDocument.ORMModel.DataTypes.TrueOrFalseLogicalDataType = New NORMA.Model.TrueOrFalseLogicalDataType
                lrORMDocument.ORMModel.DataTypes.YesOrNoLogicalDataType = New NORMA.Model.YesOrNoLogicalDataType
                lrORMDocument.ORMModel.DataTypes.RowIdOtherDataType = New NORMA.Model.RowIdOtherDataType
                lrORMDocument.ORMModel.DataTypes.ObjectIdOtherDataType = New NORMA.Model.ObjectIdOtherDataType
#End Region

                '==============================
                'Create ALLCAPS GUIDs
                '==============================
#Region "Create ALLCAPS Ids"
                For Each lrFBMValueType In Me.ORMModel.ValueTypes
                    lrFBMValueType.GUID = System.Guid.NewGuid.ToString.ToUpper
                Next
                For Each lrFBMEntityType In Me.ORMModel.EntityTypes
                    lrFBMEntityType.GUID = System.Guid.NewGuid.ToString.ToUpper
                    'lrFBMEntityType.ReferenceModeValueTypeId = lrFBMEntityType.ReferenceModeValueTypeId.ToUpper
                    'lrFBMEntityType.ReferenceSchemeRoleConstraintId = lrFBMEntityType.ReferenceSchemeRoleConstraintId.ToUpper
                Next
                For Each lrFBMFactType In Me.ORMModel.FactTypes
                    lrFBMFactType.GUID = System.Guid.NewGuid.ToString.ToUpper

                    For Each lrFBMRole In lrFBMFactType.RoleGroup
                        lrFBMRole.Id = lrFBMRole.Id.ToUpper
                    Next

                    For Each lrFBMFactTypeReading In lrFBMFactType.FactTypeReadings
                        lrFBMFactTypeReading.Id = lrFBMFactTypeReading.Id.ToUpper
                        For Each lrFBMPredicatePart In lrFBMFactTypeReading.PredicateParts
                            lrFBMPredicatePart.Role_Id = lrFBMPredicatePart.Role_Id.ToUpper
                        Next
                    Next
                Next
                For Each lrFBMRoleConstraint In Me.ORMModel.RoleConstraints
                    lrFBMRoleConstraint.GUID = System.Guid.NewGuid.ToString.ToUpper

                    For Each lrFBMRoleConstraintRole In lrFBMRoleConstraint.RoleConstraintRoles
                        lrFBMRoleConstraintRole.RoleId = lrFBMRoleConstraintRole.RoleId.ToUpper
                    Next

                    For Each lrFBMArgument In lrFBMRoleConstraint.Argument
                        lrFBMArgument.Id = System.Guid.NewGuid.ToString.ToUpper
                        For Each lrFBMRoleReference In lrFBMArgument.Role
                            lrFBMRoleReference.RoleId = lrFBMRoleReference.RoleId.ToUpper
                        Next
                    Next
                Next
#End Region

                '==============================
                'Map the ValueTypes
                '==============================
#Region "ValueTypes"
                Dim lrValueType As NORMA.Model.ValueType = Nothing
                Dim lsValueTypeConstraintValue As String = ""

                For Each lrFBMValueType In Me.ORMModel.ValueTypes

                    lrValueType = New NORMA.Model.ValueType
                    lrValueType.Id = "_" & lrFBMValueType.GUID
                    lrValueType.Name = lrFBMValueType.Name

                    Dim lsDataTypeRef As String = ""
                    Select Case lrFBMValueType.DataType
                        Case Is = pcenumORMDataType.DataTypeNotSet
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.UnspecifiedDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.NumericAutoCounter
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.AutoCounterNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.TextFixedLength
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.FixedLengthTextDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.NumericFloatCustomPrecision
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.FloatingPointNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                            lrValueType.ConceptualDataType.Length = lrFBMValueType.DataTypePrecision
                        Case Is = pcenumORMDataType.TemporalDate
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.DateTemporalDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.TextVariableLength
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.VariableLengthTextDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, Nothing, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.TextLargeLength
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.LargeLengthTextDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, Nothing, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.NumericSignedInteger
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.SignedIntegerNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.NumericSignedSmallInteger
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.SignedSmallIntegerNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.NumericSignedBigInteger
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.SignedLargeIntegerNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.NumericUnsignedTinyInteger
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.UnsignedTinyIntegerNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.NumericUnsignedSmallInteger
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.UnsignedSmallIntegerNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.NumericUnsignedInteger
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.UnsignedIntegerNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.NumericFloatSinglePrecision
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.SinglePrecisionFloatingPointNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, Nothing)
                        Case Is = pcenumORMDataType.NumericFloatDoublePrecision
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.DoublePrecisionFloatingPointNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, Nothing)
                        Case Is = pcenumORMDataType.NumericDecimal
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.DecimalNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.NumericMoney
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.MoneyNumericDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.RawDataFixedLength
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.FixedLengthRawDataDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, Nothing, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.RawDataVariableLength
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.VariableLengthRawDataDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, Nothing, lrFBMValueType.DataTypeLength)
                        Case Is = pcenumORMDataType.RawDataLargeLength
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.LargeLengthRawDataDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.RawDataPicture
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.PictureRawDataDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.RawDataOLEObject
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.OleObjectRawDataDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.TemporalAutoTimestamp
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.AutoTimestampTemporalDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.TemporalTime
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.TimeTemporalDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.TemporalDateAndTime
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.DateAndTimeTemporalDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.LogicalTrueFalse
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.TrueOrFalseLogicalDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.LogicalYesNo
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.YesOrNoLogicalDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.OtherRowID
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.RowIdOtherDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Is = pcenumORMDataType.OtherObjectID
                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.ObjectIdOtherDataType.Id
                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
                        Case Else
                            Throw New NotImplementedException($"No implementation is found for {lrFBMValueType.DataType} in 'ValueType'")
                    End Select

                    'lrValueType.IsIndependent = lrFBMValueType.IsIndependent

                    ''==============================
                    ''check and apply the ValueConstraint if available
                    ''==============================
                    'If Not IsNothing(lrFBMValueType.ValueConstraint) AndAlso lrFBMValueType.ValueConstraint.Count > 0 Then
                    '    lrValueType.ValueRestriction.ValueConstraint = New NORMA.Model.ValueType.ValueTypeRestrictionValueConstraint()
                    '    For Each lsValueTypeConstraintValue In lrFBMValueType.ValueConstraint
                    '        lrValueType.ValueConstraint.Add(lsValueTypeConstraintValue)
                    '        lrValueType.Instance.Add(lsValueTypeConstraintValue)
                    '    Next
                    'End If

                    lrNORMAModel.Objects.Items.Add(lrValueType)
SkipValueType:
                Next
#End Region

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(20)

                '==============================
                'Map the EntityTypes
                '==============================
#Region "EntityTypes"
                Dim lrEntityType As NORMA.Model.EntityType

                For Each lrFBMEntityType In Me.ORMModel.EntityTypes

                    If lrFBMEntityType.IsObjectifyingEntityType Then

                        '########## Objectified Type ##########
                        Dim lrObjectifiedType As New NORMA.Model.ObjectifiedType()
                        lrObjectifiedType.Id = "_" & lrFBMEntityType.GUID
                        lrObjectifiedType.Name = lrFBMEntityType.Name
                        lrObjectifiedType.IsIndependent = lrFBMEntityType.IsIndependent
                        lrObjectifiedType._ReferenceMode = lrFBMEntityType.ReferenceMode.TrimStart("."c)

                        lrNORMAModel.Objects.Items.Add(lrObjectifiedType)

                    Else

                        '########## Entity Type ##########
                        lrEntityType = New NORMA.Model.EntityType
                        lrEntityType.Id = "_" & lrFBMEntityType.GUID
                        lrEntityType.Name = lrFBMEntityType.Name
                        lrEntityType._ReferenceMode = lrFBMEntityType.ReferenceMode.TrimStart("."c)

                        lrNORMAModel.Objects.Items.Add(lrEntityType)

                    End If

                    'lrEntityType.DerivationText = lrFBMEntityType.DerivationText
                    ''lrEntityType.ShortDescription = 
                    ''lrEntityType.LongDescription = 
                    'lrEntityType.IsObjectifyingEntityType = lrFBMEntityType.IsObjectifyingEntityType
                    'lrEntityType.IsAbsorbed = lrFBMEntityType.IsAbsorbed


                    'If lrFBMEntityType.ReferenceModeValueTypeId = "" Then
                    '    lrEntityType.ReferenceModeValueType = Nothing
                    'Else
                    '    lrEntityType.ReferenceModeValueType = New FBM.ValueType
                    '    lrEntityType.ReferenceModeValueType.Id = lrFBMEntityType.ReferenceModeValueTypeId
                    '    lrEntityType.ReferenceModeValueType = lrNORMAModel.ValueType.Find(AddressOf lrEntityType.ReferenceModeValueType.Equals)
                    'End If

                    'lrEntityType.PreferredIdentifierRCId = lrFBMEntityType.ReferenceSchemeRoleConstraintId

                    ''------------------------------------------------
                    ''Link to the Concept within the ModelDictionary
                    ''------------------------------------------------
                    'Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrNORMAModel, lrEntityType.Name, pcenumConceptType.EntityType, , , True, True, lrEntityType.DBName)
                    'lrDictionaryEntry = lrNORMAModel.AddModelDictionaryEntry(lrDictionaryEntry, , True,, True,, True)

                    'lrEntityType.Concept = lrDictionaryEntry.Concept


SkipEntityType:
                Next
#End Region

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(30)

                '==============================
                'Map the FactTypes
                '==============================
#Region "FactTypes"
                ' define as object because it can be Fact, ImpliedFact, or SubtypeFact
                Dim lrFactType As Object

                For Each lrFBMFactType In Me.ORMModel.FactTypes

                    Dim lrObjectifiedType As NORMA.Model.ObjectifiedType = Nothing
                    If lrFBMFactType.IsObjectified Then
                        lrObjectifiedType = (From lrObject In arNORMADocument.ORMModel.Objects.Items.OfType(Of NORMA.Model.ObjectifiedType)
                                             Where lrObject.Name = lrFBMFactType.ObjectifyingEntityTypeId
                                             Select lrObject).FirstOrDefault()

                        If lrObjectifiedType Is Nothing Then Continue For
                    End If

                    If lrFBMFactType.IsSubtypeRelationshipFactType Then

                        lrFactType = New NORMA.Model.SubtypeFact("_" & lrFBMFactType.GUID, lrFBMFactType.Name) With {
                            .PreferredIdentificationPath = lrFBMFactType.IsSubtypeRelationshipFactType
                        }
                        Call Me.GetNORMASubtypeDetails(lrORMDocument, lrFactType, lrFBMFactType)

                    ElseIf lrFBMFactType.IsLinkFactType Then

                        lrFactType = New NORMA.Model.ImpliedFact("_" & lrFBMFactType.GUID, lrFBMFactType.Name)
                        ' initialize the factroles in for ImpliedFact
                        lrFactType.FactRoles = New NORMA.Model.ImpliedFact.ImpliedFactRoles()
                        Call Me.GetNORMAImpliedFactDetails(lrORMDocument, lrFactType, lrFBMFactType)

                    Else
                        lrFactType = New NORMA.Model.Fact("_" & lrFBMFactType.GUID, lrFBMFactType.Name)
#Region "Objectification"
                        ' check and add the nested predicate if objectified type
                        If lrObjectifiedType IsNot Nothing Then
                            lrObjectifiedType.NestedPredicate = New NORMA.Model.ObjectifiedType.ObjectifiedTypeNestedPredicate()
                            lrObjectifiedType.NestedPredicate.Id = "_" & Guid.NewGuid.ToString.ToUpper()
                            lrObjectifiedType.NestedPredicate.Ref = lrFactType.Id

                            Dim larFBMPreferredIdentifer = (From InternalUniquenssConstraint In Me.ORMModel.RoleConstraints
                                                            Where InternalUniquenssConstraint.RoleConstraintType = "InternalUniquenessConstraint"
                                                            From RoleConstraintRole In InternalUniquenssConstraint.RoleConstraintRoles
                                                            From Role In lrFBMFactType.RoleGroup
                                                            Where Role.Id = RoleConstraintRole.RoleId
                                                            Select InternalUniquenssConstraint).FirstOrDefault()

                            If Not IsNothing(larFBMPreferredIdentifer) Then
                                lrObjectifiedType.PreferredIdentifier = New NORMA.Model.ObjectifiedType.ObjectifiedTypePreferredIdentifier("_" & larFBMPreferredIdentifer.GUID)
                            End If

                        End If
#End Region
                        Call Me.GetNORMAFactTypeDetails(lrORMDocument, lrFactType, lrFBMFactType)
                    End If

                    '20221106-VM-Changed Facts.Items to a List(Of ....)
                    If IsNothing(lrORMDocument.ORMModel.Facts) Then
                        lrORMDocument.ORMModel.Facts = New NORMA.ORMModelFacts()
                        'lrORMDocument.ORMModel.Facts.Items = Array.CreateInstance(GetType(Object), 0)
                    End If
                    lrNORMAModel.Facts.Items.Add(lrFactType)

                Next

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(66)

                '                '==============================
                '                'Subtype Relationships
                '                '==============================
#Region "Subtype Relationships"
                '                Dim larSubtypeRelationshipFactTypes = From FactType In Me.ORMModel.FactTypes
                '                                                      Where FactType.IsSubtypeRelationshipFactType
                '                                                      Select FactType

                '                Dim lrModelElement As FBM.ModelObject
                '                For Each lrFBMFactType In larSubtypeRelationshipFactTypes

                '                    If abSkipAlreadyLoadedModelElements Then
                '                        'Really only ever called when using the UnifiedOntologyBrowser
                '                        '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
                '                        If lrNORMAModel.FactType.Find(Function(x) x.Id = lrFBMFactType.Id) IsNot Nothing Then GoTo SkipSubtypeRelationship
                '                    End If

                '                    lrFactType = New FBM.FactType(lrFBMFactType.Id, True)
                '                    lrFactType = lrNORMAModel.FactType.Find(AddressOf lrFactType.Equals)

                '                    Dim lrParentModelElement As FBM.ModelObject
                '                    lrModelElement = lrNORMAModel.GetModelObjectByName(lrFactType.RoleGroup(0).JoinedORMObject.Id)
                '                    lrParentModelElement = lrNORMAModel.GetModelObjectByName(lrFactType.RoleGroup(1).JoinedORMObject.Id)
                '                    If lrParentModelElement.GetType = GetType(FBM.FactType) Then
                '                        lrParentModelElement = CType(lrParentModelElement, FBM.FactType).ObjectifyingEntityType
                '                    End If
                '                    lrModelElement.parentModelObjectList.Add(lrParentModelElement)
                '                    lrParentModelElement.childModelObjectList.Add(lrModelElement)

                '                    Dim lrSubtypeConstraint As New FBM.SubtypeRelationship(lrModelElement, lrParentModelElement, lrFactType)

                '                    Select Case lrModelElement.GetType
                '                        Case Is = GetType(FBM.EntityType)
                '                            Dim larSubtypeRelationship = From EntityType In Me.ORMModel.EntityTypes
                '                                                         From SubtypeRelationship In EntityType.SubtypeRelationships
                '                                                         Where SubtypeRelationship.SubtypingFactTypeId = lrFactType.Id
                '                                                         Select SubtypeRelationship
                '                            Try
                '                                lrSubtypeConstraint.IsPrimarySubtypeRelationship = larSubtypeRelationship.First.IsPrimarySubtypeRelationship
                '                            Catch ex As Exception
                '                                'CodeSafe
                '                                'Not a biggie at this stage.
                '                            End Try

                '                        Case Is = GetType(FBM.ValueType)
                '                            Dim larSubtypeRelationship = From ValueType In Me.ORMModel.ValueTypes
                '                                                         From SubtypeRelationship In ValueType.SubtypeRelationships
                '                                                         Where SubtypeRelationship.SubtypingFactTypeId = lrFactType.Id
                '                                                         Select SubtypeRelationship
                '                            Try
                '                                lrSubtypeConstraint.IsPrimarySubtypeRelationship = larSubtypeRelationship.First.IsPrimarySubtypeRelationship
                '                            Catch ex As Exception
                '                                'CodeSafe
                '                                'Not a biggie at this stage.
                '                            End Try

                '                    End Select

                '                    lrModelElement.SubtypeRelationship.AddUnique(lrSubtypeConstraint)
                'SkipSubtypeRelationship:
                '                Next
#End Region 'Subtype Relationships

#End Region
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(50)

                '==============================
                'Map the RoleConstraints
                '==============================
#Region "RoleConstraints"
                For Each lrFBMRoleConstraint In Me.ORMModel.RoleConstraints

                    Select Case lrFBMRoleConstraint.RoleConstraintType

                        Case Is = pcenumRoleConstraintType.RingConstraint.ToString
#Region "Ring Constraint"

                            Dim lrNORMARoleConstraint As New NORMA.Model.RingConstraint

                            lrNORMARoleConstraint.Id = "_" & lrFBMRoleConstraint.GUID.ToUpper()
                            lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name
                            lrNORMARoleConstraint.Type = CType([Enum].Parse(GetType(NORMA.Model.RingConstraintTypeValues), lrFBMRoleConstraint.RingConstraintType), NORMA.Model.RingConstraintTypeValues).ToString()

                            '================================================
                            'Map the Ring Constraint
                            '========================================

                            lrNORMARoleConstraint.RoleSequence = New NORMA.Model.RingConstraint.ConstraintRole() {}

                            For Each lrFBMRoleConstraintRole In lrFBMRoleConstraint.RoleConstraintRoles

                                'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
                                Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
                                                            From Role In FactType.RoleGroup
                                                            Where Role.Id = lrFBMRoleConstraintRole.RoleId
                                                            Select New With {.FactType = FactType, .Role = Role}).First

                                'NORMA Model:
                                Dim lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                       From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                       From Role In FactType.FactRoles
                                                       Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                       Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                       Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()

                                If lrRoleAndObject Is Nothing Then
                                    lrRoleAndObject = (From ModelElement In lrNORMAModel.Facts.Items
                                                       From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                       From Role In FactType.FactRoles
                                                       Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                       Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                       Select New With {.FactType = FactType, .Role = Role, .NORMAObject = CType(ModelElement, Object)}).FirstOrDefault()
                                End If

                                lrNORMARoleConstraint.RoleSequence.Add(New NORMA.Model.RingConstraint.ConstraintRole() With {.Id = "_" & System.Guid.NewGuid.ToString, .Ref = lrRoleAndObject.Role.Id})
                            Next

                            If IsNothing(arNORMADocument.ORMModel.Constraints) Then
                                arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
                                arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
                            End If

                            arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)
#End Region 'Ring Constraint
                        Case Is = pcenumRoleConstraintType.EqualityConstraint.ToString
#Region "Equality Constraint"

                            Dim lrNORMARoleConstraint As New NORMA.Model.EqualityConstraint

                            lrNORMARoleConstraint.Id = "_" & lrFBMRoleConstraint.GUID
                            lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name
                            lrNORMARoleConstraint.RoleSequences = New NORMA.Model.EqualityConstraint.RoleSequence() {}

                            '================================================
                            'Map the Equality Constraint
                            '========================================
                            For Each lrArgument In lrFBMRoleConstraint.Argument

                                Dim lrNORMARoleSequence = New NORMA.Model.EqualityConstraint.RoleSequence With {.Id = "_" & lrArgument.Id}
                                lrNORMARoleSequence.Role = New NORMA.Model.EqualityConstraint.Role() {}
                                lrNORMARoleConstraint.RoleSequences.Add(lrNORMARoleSequence)

                                For Each lrFBMRoleReference In lrArgument.Role

                                    'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
                                    Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
                                                                From Role In FactType.RoleGroup
                                                                Where Role.Id = lrFBMRoleReference.RoleId
                                                                Select New With {.FactType = FactType, .Role = Role}).First

                                    'NORMA Model:
                                    Dim lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                           From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                           From Role In FactType.FactRoles
                                                           Where Role.Id = "_" & lrFBMRoleReference.RoleId
                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()

                                    If lrRoleAndObject Is Nothing Then
                                        lrRoleAndObject = (From ModelElement In lrNORMAModel.Facts.Items
                                                           From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                           From Role In FactType.FactRoles
                                                           Where Role.Id = "_" & lrFBMRoleReference.RoleId
                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = CType(ModelElement, Object)}).FirstOrDefault()
                                    End If

                                    lrNORMARoleSequence.Role.Add(
                                        New NORMA.Model.EqualityConstraint.Role() With {
                                        .Id = "_" & Guid.NewGuid.ToString.ToUpper(),
                                        .Ref = lrRoleAndObject.Role.Id})
                                Next
                            Next

                            If IsNothing(arNORMADocument.ORMModel.Constraints) Then
                                arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
                                arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
                            End If

                            arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)
#End Region 'Equality Constraint
                        Case Is = pcenumRoleConstraintType.SubsetConstraint.ToString
#Region "Subset Constraint"
                            Dim lrNORMARoleConstraint As New NORMA.Model.SubsetConstraint()

                            lrNORMARoleConstraint.Id = "_" & lrFBMRoleConstraint.GUID
                            lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name
                            lrNORMARoleConstraint.RoleSequences = New NORMA.Model.SubsetConstraint.ConstraintRoleSequence() {}

                            '================================================
                            'Map the Subset Constraint
                            '========================================
                            For Each lrArgument In lrFBMRoleConstraint.Argument

                                Dim lrNORMARoleSequence = New NORMA.Model.SubsetConstraint.ConstraintRoleSequence With {.Id = "_" & lrArgument.Id}
                                lrNORMARoleSequence.Role = New NORMA.Model.SubsetConstraint.SequenceRole() {}
                                lrNORMARoleConstraint.RoleSequences.Add(lrNORMARoleSequence)

                                For Each lrFBMRoleReference In lrArgument.Role

                                    'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
                                    Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
                                                                From Role In FactType.RoleGroup
                                                                Where Role.Id = lrFBMRoleReference.RoleId
                                                                Select New With {.FactType = FactType, .Role = Role}).First

                                    'NORMA Model:
                                    Dim lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                           From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                           From Role In FactType.FactRoles
                                                           Where Role.Id = "_" & lrFBMRoleReference.RoleId
                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()

                                    If lrRoleAndObject Is Nothing Then
                                        lrRoleAndObject = (From ModelElement In lrNORMAModel.Facts.Items
                                                           From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                           From Role In FactType.FactRoles
                                                           Where Role.Id = "_" & lrFBMRoleReference.RoleId
                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = CType(ModelElement, Object)}).FirstOrDefault()
                                    End If

                                    lrNORMARoleSequence.Role.Add(
                                        New NORMA.Model.SubsetConstraint.SequenceRole() With {
                                            .Id = "_" & Guid.NewGuid.ToString.ToUpper(),
                                            .Ref = lrRoleAndObject.Role.Id})
                                Next
                            Next

                            If IsNothing(arNORMADocument.ORMModel.Constraints) Then
                                arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
                                arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
                            End If

                            arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)
#End Region 'Subset Constraint
                        Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint.ToString
#Region "Internal Uniqueness Constraint"

                            '================================================
                            'Map the InternalConstraints for Fact Type
                            '===========================================

                            Dim latFactTypes = {GetType(NORMA.Model.Fact),
                                GetType(NORMA.Model.SubtypeFact),
                                GetType(NORMA.Model.ImpliedFact)
                            }

                            Try
                                Dim lrNORMARoleConstraint As New NORMA.Model.UniquenessConstraint()

                                lrNORMARoleConstraint = New NORMA.Model.UniquenessConstraint()

                                lrNORMARoleConstraint.Id = "_" & lrFBMRoleConstraint.GUID
                                lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name
                                lrNORMARoleConstraint.RoleSequence = New NORMA.Model.UniquenessConstraint.ConstraintRole() {}
                                lrNORMARoleConstraint.IsInternal = True

                                Dim lrNORMAFactType As Object = Nothing

                                For Each lrFBMRoleConstraintRole In lrFBMRoleConstraint.RoleConstraintRoles

                                    'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
                                    Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
                                                                From Role In FactType.RoleGroup
                                                                Where Role.Id = lrFBMRoleConstraintRole.RoleId
                                                                Select New With {.FactType = FactType, .Role = Role}).First

                                    ' check if the subtypefact or fact type
                                    If lrFBMFactTypeAndRole.FactType.IsSubtypeRelationshipFactType Then
#Region "IUC for SubtypeFact"

                                        Dim lrRoleAndObject As Object
                                        ' check if subtype or supertype role
                                        If lrFBMFactTypeAndRole.Role.Mandatory Then
                                            ' going to check for subtype role
                                            lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                               From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.SubtypeFact)
                                                               Where FactType.FactRoles IsNot Nothing AndAlso
                                                                              FactType.FactRoles.SubtypeMetaRole IsNot Nothing AndAlso
                                                                              FactType.FactRoles.SubtypeMetaRole.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                               Where ModelElement.Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                               Select New With {
                                                                              .FactType = FactType,
                                                                              .Role = FactType.FactRoles.SubtypeMetaRole,
                                                                              .NORMAObject = ModelElement
                                                                        })?.FirstOrDefault()
                                            ' check if subtype is not found
                                            If lrRoleAndObject Is Nothing Then
                                                lrRoleAndObject = (From ModelElement In lrNORMAModel.Facts.Items
                                                                   From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.SubtypeFact)
                                                                   Where FactType.FactRoles IsNot Nothing AndAlso
                                                                              FactType.FactRoles.SubtypeMetaRole IsNot Nothing AndAlso
                                                                              FactType.FactRoles.SubtypeMetaRole.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                                   Where ModelElement._Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                                   Select New With {
                                                                              .FactType = FactType,
                                                                              .Role = FactType.FactRoles.SubtypeMetaRole,
                                                                              .NORMAObject = ModelElement
                                                                            }).FirstOrDefault()
                                            End If
                                        Else
                                            ' going to check for supertype role
                                            lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                               From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.SubtypeFact)
                                                               Where FactType.FactRoles IsNot Nothing AndAlso
                                                                              FactType.FactRoles.SupertypeMetaRole IsNot Nothing AndAlso
                                                                              FactType.FactRoles.SupertypeMetaRole.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                               Where ModelElement.Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                               Select New With {
                                                                              .FactType = FactType,
                                                                              .Role = FactType.FactRoles.SupertypeMetaRole,
                                                                              .NORMAObject = ModelElement
                                                                        })?.FirstOrDefault()
                                            ' check if subtype is not found
                                            If lrRoleAndObject Is Nothing Then
                                                lrRoleAndObject = (From ModelElement In lrNORMAModel.Facts.Items
                                                                   From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.SubtypeFact)
                                                                   Where FactType.FactRoles IsNot Nothing AndAlso
                                                                              FactType.FactRoles.SupertypeMetaRole IsNot Nothing AndAlso
                                                                              FactType.FactRoles.SupertypeMetaRole.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                                   Where ModelElement._Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                                   Select New With {
                                                                              .FactType = FactType,
                                                                              .Role = FactType.FactRoles.SupertypeMetaRole,
                                                                              .NORMAObject = ModelElement
                                                                            }).FirstOrDefault()
                                            End If
                                        End If

                                        lrNORMAFactType = lrRoleAndObject.FactType
                                        lrNORMARoleConstraint.RoleSequence.Add(New NORMA.Model.UniquenessConstraint.ConstraintRole() With {
                                                                               .Ref = lrRoleAndObject.Role.Id})
                                        If lrRoleAndObject.Role._IsMandatory Then
                                            lrRoleAndObject.Role._Multiplicity = "ZeroToOne"
                                        Else
                                            lrRoleAndObject.Role._Multiplicity = "ExactlyOne"
                                        End If

#End Region
                                    ElseIf lrFBMFactTypeAndRole.FactType.IsLinkFactType Then
#Region "IUC for ImpliedFact type"

                                        'NORMA Model:
                                        Dim lrRoleAndObject As Object
                                        Try
                                            lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                               From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.ImpliedFact)
                                                               Where FactType.FactRoles IsNot Nothing AndAlso
                                                                   FactType.FactRoles.Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                               Where ModelElement.Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                               Select New With {
                                                                   .FactType = FactType,
                                                                   .Role = FactType.FactRoles.Role,
                                                                   .NORMAObject = ModelElement
                                                                   })?.FirstOrDefault()
                                        Catch ex As Exception
                                            lrRoleAndObject = Nothing
                                        End Try

                                        If lrRoleAndObject Is Nothing Then
                                            lrRoleAndObject = (From ModelElement In lrNORMAModel.Facts.Items
                                                               From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.ImpliedFact)
                                                               Where FactType.FactRoles IsNot Nothing AndAlso
                                                                   FactType.FactRoles.Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                               Where ModelElement._Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                               Select New With {
                                                                   .FactType = FactType,
                                                                   .Role = FactType.FactRoles.Role,
                                                                   .NORMAObject = ModelElement}).FirstOrDefault()
                                        End If

                                        lrNORMAFactType = lrRoleAndObject.FactType

                                        lrNORMARoleConstraint.RoleSequence.Add(New NORMA.Model.UniquenessConstraint.ConstraintRole() With {.Ref = lrRoleAndObject.Role.Id})

#End Region
                                    Else
#Region "IUC for Fact type"

                                        'NORMA Model:
                                        Dim lrRoleAndObject As Object
                                        Try
                                            lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                               From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                               Where FactType.FactRoles IsNot Nothing
                                                               From Role In FactType.FactRoles
                                                               Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                               Where ModelElement.Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                               Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement})?.FirstOrDefault()
                                        Catch ex As Exception
                                            lrRoleAndObject = Nothing
                                        End Try

                                        If lrRoleAndObject Is Nothing Then
                                            lrRoleAndObject = (From ModelElement In lrNORMAModel.Facts.Items
                                                               From FactType In lrNORMAModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                               From Role In FactType.FactRoles
                                                               Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                               Where ModelElement._Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                               Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()
                                        End If

                                        lrNORMAFactType = lrRoleAndObject.FactType

                                        lrNORMARoleConstraint.RoleSequence.Add(New NORMA.Model.UniquenessConstraint.ConstraintRole() With {.Ref = lrRoleAndObject.Role.Id})

                                        If lrRoleAndObject.Role._IsMandatory Then
                                            lrRoleAndObject.Role._Multiplicity = "ZeroToOne"
                                        Else
                                            lrRoleAndObject.Role._Multiplicity = "ExactlyOne"
                                        End If

                                        If lrFBMRoleConstraint.IsPreferredUniqueness And lrFBMRoleConstraint.RoleConstraintRoles.Count = 1 And CType(lrRoleAndObject.FactType, NORMA.Model.Fact).FactRoles.Count = 2 Then

                                            Dim lrNORMAObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                                 From Role In CType(lrRoleAndObject.FactType, NORMA.Model.Fact).FactRoles
                                                                 Where Role.Id <> lrRoleAndObject.Role.Id
                                                                 Where ModelElement.Id = Role.RolePlayer.Ref
                                                                 Select ModelElement).First

                                            lrNORMARoleConstraint.PreferredIdentifierFor = New NORMA.Model.UniquenessConstraint.ConstraintPreferredIdentifierFor()
                                            lrNORMARoleConstraint.PreferredIdentifierFor.Ref = lrNORMAObject.Id

                                            If lrNORMAObject.GetType() = GetType(NORMA.Model.EntityType) Then
                                                CType(lrNORMAObject, NORMA.Model.EntityType).PreferredIdentifier = New NORMA.Model.EntityType.EntityTypePreferredIdentifier()
                                                CType(lrNORMAObject, NORMA.Model.EntityType).PreferredIdentifier.Ref = lrNORMARoleConstraint.Id
                                            End If

                                        End If

#End Region
                                    End If

                                Next

                                If lrNORMAFactType IsNot Nothing Then
                                    ' set the IC according to Fact type
                                    Select Case lrNORMAFactType.GetType()

                                            ' check if Fact type
                                        Case GetType(NORMA.Model.Fact)
                                            If IsNothing(lrNORMAFactType.InternalConstraints) Then
                                                lrNORMAFactType.InternalConstraints = New NORMA.Model.Fact.FactInternalConstraints()
                                                lrNORMAFactType.InternalConstraints.Items = New List(Of Object)
                                            End If
                                            lrNORMAFactType.InternalConstraints.Items.
                                                Add(New NORMA.Model.Fact.FactInternalConstraints.UniquenessConstraint() With {
                                                    .Ref = lrNORMARoleConstraint.Id
                                                    })

                                            ' check if ImpliedFact type
                                        Case GetType(NORMA.Model.Fact)
                                            ' check if internal constraints initialized
                                            If IsNothing(lrNORMAFactType.InternalConstraints) Then
                                                lrNORMAFactType.InternalConstraints = New NORMA.Model.ImpliedFact.FactInternalConstraints()
                                            End If
                                            ' check if UniquenessConstraint initialized in IC
                                            If IsNothing(lrNORMAFactType.InternalConstraints.UniquenessConstraint) Then
                                                lrNORMAFactType.InternalConstraints.UniquenessConstraint =
                                                    New NORMA.Model.ImpliedFact.FactInternalConstraints.ConstraintsUniquenessConstraint()
                                            End If
                                            ' set value for the UniquenessConstraint
                                            lrNORMAFactType.InternalConstraints.UniquenessConstraint.Ref = lrNORMARoleConstraint.Id

                                            ' check if the Subtype fact
                                        Case GetType(NORMA.Model.SubtypeFact)
                                            ' check if internal constraints initialized
                                            If IsNothing(lrNORMAFactType.InternalConstraints) Then
                                                lrNORMAFactType.InternalConstraints = New NORMA.Model.SubtypeFact.SubtypeFactInternalConstraints()
                                            End If
                                            ' check if UniquenessConstraint initialized in IC
                                            If IsNothing(lrNORMAFactType.InternalConstraints.UniquenessConstraint) Then
                                                lrNORMAFactType.InternalConstraints.UniquenessConstraint =
                                                    New List(Of NORMA.Model.SubtypeFact.InternalConstraintsUniquenessConstraint)
                                            End If
                                            ' add the UniquenessConstraint
                                            lrNORMAFactType.InternalConstraints.UniquenessConstraint.
                                                Add(New NORMA.Model.SubtypeFact.InternalConstraintsUniquenessConstraint() With {
                                                    .Ref = lrNORMARoleConstraint.Id
                                                })

                                    End Select
                                End If

                                If IsNothing(arNORMADocument.ORMModel.Constraints) Then
                                    arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
                                    arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
                                End If

                                arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)

                            Catch ex As Exception
                                prApplication.ThrowMessage("Error loading Internal Uniqueness Constraint: " & lrFBMRoleConstraint.Id, pcenumErrorType.Warning,, False,, True)
                            End Try

#End Region 'Enternal Uniqueness Constraint
                        Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint.ToString
#Region "External Uniqueness Constraint"

                            Try
                                Dim lrNORMARoleConstraint As New NORMA.Model.UniquenessConstraint()

                                lrNORMARoleConstraint = New NORMA.Model.UniquenessConstraint()

                                lrNORMARoleConstraint.Id = "_" & lrFBMRoleConstraint.GUID
                                lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name
                                lrNORMARoleConstraint.RoleSequence = New NORMA.Model.UniquenessConstraint.ConstraintRole() {}

                                lrNORMARoleConstraint.IsInternal = False

                                '================================================
                                'Map the External Uniqueness Constraint
                                '========================================
                                For Each lrFBMRoleConstraintRole In lrFBMRoleConstraint.RoleConstraintRoles

                                    Dim larFactType = From FactType In Me.ORMModel.FactTypes
                                                      Where FactType.Id.StartsWith("Therapy")
                                                      Select FactType

                                    'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
                                    Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
                                                                From Role In FactType.RoleGroup
                                                                Where Role.Id = lrFBMRoleConstraintRole.RoleId
                                                                Select New With {.FactType = FactType, .Role = Role}).First

                                    'NORMA Model:
                                    Dim lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                           From FactType In lrNORMAModel.Facts.Items
                                                           Where CType(FactType, NORMA.Model.Fact).FactRoles IsNot Nothing
                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
                                                           Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()

                                    If lrRoleAndObject Is Nothing Then
                                        lrRoleAndObject = (From ModelElement In lrNORMAModel.Facts.Items
                                                           From FactType In lrNORMAModel.Facts.Items
                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
                                                           Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = CType(ModelElement, Object)}).FirstOrDefault()
                                    End If

                                    lrNORMARoleConstraint.RoleSequence.Add(New NORMA.Model.UniquenessConstraint.ConstraintRole() With {.Ref = lrRoleAndObject.Role.Id})

                                    If lrRoleAndObject.Role._IsMandatory Then
                                        lrRoleAndObject.Role._Multiplicity = "ZeroToOne"
                                    Else
                                        lrRoleAndObject.Role._Multiplicity = "ExactlyOne"
                                    End If

                                    If lrFBMRoleConstraint.IsPreferredUniqueness Then

                                        Dim lrNORMAObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
                                                             From Role In CType(lrRoleAndObject.FactType, NORMA.Model.Fact).FactRoles
                                                             Where Role.Id <> lrRoleAndObject.Role.Id
                                                             Where ModelElement.Id = Role.RolePlayer.Ref
                                                             Select ModelElement).First

                                        If lrNORMARoleConstraint.PreferredIdentifierFor Is Nothing Then
                                            CType(lrNORMAObject, NORMA.Model.EntityType).PreferredIdentifier = New NORMA.Model.EntityType.EntityTypePreferredIdentifier()
                                            CType(lrNORMAObject, NORMA.Model.EntityType).PreferredIdentifier.Ref = lrNORMARoleConstraint.Id

                                            lrNORMARoleConstraint.PreferredIdentifierFor = New NORMA.Model.UniquenessConstraint.ConstraintPreferredIdentifierFor()
                                            lrNORMARoleConstraint.PreferredIdentifierFor.Ref = lrNORMAObject.Id
                                        End If

                                    End If
                                Next

                                If IsNothing(arNORMADocument.ORMModel.Constraints) Then
                                    arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
                                    arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
                                End If

                                arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)

                            Catch ex As Exception
                                prApplication.ThrowMessage("Error exporting External Uniqueness Constraint: " & lrFBMRoleConstraint.Id, pcenumErrorType.Warning,, False,, True)
                            End Try
#End Region 'External Uniqueness Constraint
                        Case Is = pcenumRoleConstraintType.RoleValueConstraint.ToString
#Region "Role Value Constraint"
                            Dim lsFBMRoleId As String = lrFBMRoleConstraint.RoleConstraintRoles(0).RoleId

                            Dim lrFBMRole = (From FactType In Me.ORMModel.FactTypes
                                             From Role In FactType.RoleGroup
                                             Where Role.Id = lsFBMRoleId
                                             Select Role).First

                            Dim larNORMAFacts As New List(Of NORMA.Model.Fact)

                            larNORMAFacts = lrORMDocument.ORMModel.Facts.Items.OfType(Of NORMA.Model.Fact).ToList

                            Dim lrNORMARole As NORMA.Model.Fact.FactRole = (From FactType In larNORMAFacts
                                                                            From Role In FactType.FactRoles
                                                                            Where Role.Id = "_" & lsFBMRoleId
                                                                            Select Role).First

                            ' initialize the RoleValue constraint
                            lrNORMARole.ValueRestriction = New NORMA.Model.Fact.FactRole.FactRoleValueRestriction()
                            lrNORMARole.ValueRestriction.RoleValueConstraint = New NORMA.Model.Fact.FactRole.FactRoleValueRestriction.RestrictionRoleValueConstraint()
                            lrNORMARole.ValueRestriction.RoleValueConstraint.Id = "_" & Guid.NewGuid().ToString().ToUpper()
                            lrNORMARole.ValueRestriction.RoleValueConstraint.Name = "RoleValueConstraint"

                            lrNORMARole.ValueRestriction.RoleValueConstraint.ValueRanges = New NORMA.Model.Fact.FactRole.FactRoleValueRestriction.ConstraintValueRangesValueRange() {}

                            ' loop all the available values from FBM value constraint
                            For Each Value In lrFBMRoleConstraint.ValueConstraint
                                lrNORMARole.ValueRestriction.RoleValueConstraint.ValueRanges.Add(
                                New NORMA.Model.Fact.FactRole.FactRoleValueRestriction.ConstraintValueRangesValueRange() With {
                                    .Id = "_" & Guid.NewGuid().ToString().ToUpper(),
                                    .MinValue = Value,
                                    .MaxValue = Value,
                                    .MinInclusion = "NotSet",
                                    .MaxInclusion = "NotSet"
                                })
                            Next
#End Region

#End Region
                        Case Else
                            Continue For
                    End Select

#Region "Old Boston code"

                    '                    lrRoleConstraint = New FBM.RoleConstraint
                    '                    lrRoleConstraint.Id = lrFBMRoleConstraint.Id
                    '                    lrRoleConstraint.Model = lrModel
                    '                    lrRoleConstraint.Name = lrFBMRoleConstraint.Name
                    '                    'lrRoleConstraint.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                    '                    'lrRoleConstraint.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
                    '                    lrRoleConstraint.ConceptType = pcenumConceptType.RoleConstraint
                    '                    lrRoleConstraint.RoleConstraintType = CType([Enum].Parse(GetType(pcenumRoleConstraintType), lrFBMRoleConstraint.RoleConstraintType), pcenumRoleConstraintType)
                    '                    lrRoleConstraint.RingConstraintType = CType([Enum].Parse(GetType(pcenumRingConstraintType), lrFBMRoleConstraint.RingConstraintType), pcenumRingConstraintType)
                    '                    'lrRoleConstraint.LevelNr = <See right down the bottom of this method>
                    '                    lrRoleConstraint.IsPreferredIdentifier = lrFBMRoleConstraint.IsPreferredUniqueness
                    '                    lrRoleConstraint.IsDeontic = lrFBMRoleConstraint.IsDeontic
                    '                    lrRoleConstraint.Cardinality = lrFBMRoleConstraint.Cardinality
                    '                    lrRoleConstraint.MaximumFrequencyCount = lrFBMRoleConstraint.MaximumFrequencyCount
                    '                    lrRoleConstraint.MinimumFrequencyCount = lrFBMRoleConstraint.MinimumFrequencyCount
                    '                    Select Case lrFBMRoleConstraint.CardinalityRangeType
                    '                        Case Is = pcenumCardinalityRangeType.LessThanOrEqual.ToString
                    '                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOrEqual
                    '                        Case Is = pcenumCardinalityRangeType.Equal.ToString
                    '                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Equal
                    '                        Case Is = pcenumCardinalityRangeType.GreaterThanOrEqual.ToString
                    '                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.GreaterThanOrEqual
                    '                        Case Is = pcenumCardinalityRangeType.Between.ToString
                    '                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Between
                    '                    End Select
                    '
                    '                    '----------------------------------------------------------
                    '                    'Get the RoleConstraintRole list for the RoleConstraint.
                    '                    '  NB the SequenceNr of a RoleConstraintRole is 'not' the same as a SequenceNr on a Role.
                    '                    '  SequenceNr on a RoleConstraintRole is many things, but
                    '                    '  relates particularly to DataIn, DataOut integrity matching.
                    '                    '----------------------------------------------------------

                    '=======================================================================================================
                    '                        lrRoleConstraintRole = New FBM.RoleConstraintRole
                    '                        lrRoleConstraintRole.Model = lrModel
                    '                        lrRoleConstraintRole.RoleConstraint = lrRoleConstraint
                    '                        lrRoleConstraintRole.SequenceNr = lrFBMRoleConstraintRole.SequenceNr
                    '                        lrRoleConstraintRole.IsEntry = lrFBMRoleConstraintRole.IsEntry
                    '                        lrRoleConstraintRole.IsExit = lrFBMRoleConstraintRole.IsExit


                    '                        lrRole.Id = lrFBMRoleConstraintRole.RoleId

                    '                        lrRoleConstraintRole.Role = lrNORMAModel.Role.Find(AddressOf lrRole.Equals)

                    '                        '-------------------------------------------------------------------
                    '                        'lrRoleConstraintRole.RoleConstraintArgument is set further below.
                    '                        '-------------------------------------------------------------------
                    '                        lrRoleConstraintRole.ArgumentSequenceNr = lrFBMRoleConstraintRole.ArgumentSequenceNr

                    '                        lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                    '                        lrRoleConstraint.Role.Add(lrRoleConstraintRole.Role)
                    '                        lrRoleConstraintRole.Role.RoleConstraintRole.Add(lrRoleConstraintRole)
                    '                    Next

                    '                    If lrRoleConstraint.RoleConstraintRole.Count = 0 Then
                    '                        prApplication.ThrowMessage("No RoleConstraintRoles found for RoleConstraint.Id: " & lrRoleConstraint.Id, pcenumErrorType.Information)
                    '                    Else
                    '                        lrFactType = lrRoleConstraint.Role(0).FactType
                    '                        lrFactType = lrNORMAModel.FactType.Find(AddressOf lrFactType.Equals)

                    '                        Select Case lrRoleConstraint.RoleConstraintType
                    '                            Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                    '                                Call lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)
                    '                        End Select
                    '                    End If

                    '                    Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
                    '                    Dim lrFBMRoleConstraintArgument As XMLModel.RoleConstraintArgument
                    '                    Dim lrJoinPath As FBM.JoinPath
                    '                    Dim lrFBMRoleReference As XMLModel.RoleReference

                    '                    For Each lrFBMRoleConstraintArgument In lrFBMRoleConstraint.Argument
                    '                        lrRoleConstraintArgument = New FBM.RoleConstraintArgument(lrRoleConstraint,
                    '                                                                                  lrFBMRoleConstraintArgument.SequenceNr,
                    '                                                                                  lrFBMRoleConstraintArgument.Id)
                    '                        lrRoleConstraintArgument.isDirty = True

                    '                        lrJoinPath = New FBM.JoinPath(lrRoleConstraintArgument)
                    '                        lrJoinPath.JoinPathError = lrFBMRoleConstraintArgument.JoinPath.JoinPathError

                    '                        For Each lrFBMRoleReference In lrFBMRoleConstraintArgument.Role
                    '                            lrRole = New FBM.Role
                    '                            lrRole.Id = lrFBMRoleReference.RoleId
                    '                            lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint)
                    '                            lrRoleConstraintRole = lrRoleConstraint.RoleConstraintRole.Find(AddressOf lrRoleConstraintRole.EqualsByRole)
                    '                            lrRoleConstraintArgument.RoleConstraintRole.Add(lrRoleConstraintRole)
                    '                            lrRoleConstraintRole.RoleConstraintArgument = lrRoleConstraintArgument
                    '                        Next

                    '                        For Each lrFBMRoleReference In lrFBMRoleConstraintArgument.JoinPath.RolePath
                    '                            lrRole = New FBM.Role
                    '                            lrRole.Id = lrFBMRoleReference.RoleId
                    '                            lrRole = lrNORMAModel.Role.Find(AddressOf lrRole.Equals)
                    '                            lrJoinPath.RolePath.Add(lrRole)
                    '                        Next
                    '                        lrRoleConstraintArgument.JoinPath = lrJoinPath
                    '                        Call lrRoleConstraintArgument.JoinPath.ConstructFactTypePath()
                    '                        lrRoleConstraint.Argument.Add(lrRoleConstraintArgument)
                    '                    Next

                    '                    If (lrRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint) And (lrRoleConstraint.Role.Count > 0) Then
                    '                        lrRoleConstraint.LevelNr = lrRoleConstraint.Role(0).FactType.InternalUniquenessConstraint.Count
                    '                    End If

                    '                    'CodeSafe-RoleConstraint must have Roles
                    '                    If lrRoleConstraint.Role.Count > 0 Then
                    '                        lrNORMAModel.RoleConstraint.Add(lrRoleConstraint)
                    '                    End If

                    '                    For Each lsValueTypeConstraintValue In lrFBMRoleConstraint.ValueConstraint
                    '                        lrRoleConstraint.ValueConstraint.Add(lsValueTypeConstraintValue)
                    '                    Next
                    '==============================================================
#End Region

SkipRoleConstraint:
                Next

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(60)

                '==============================
                'Map the ModelNotes
                '==============================
#Region "ModelNotes"
                '                Dim lrFBMModelNote As XMLModel.ModelNote
                '                Dim lrModelNote As FBM.ModelNote

                '                For Each lrFBMModelNote In Me.ORMModel.ModelNotes

                '                    If abSkipAlreadyLoadedModelElements Then
                '                        'Really only ever called when using the UnifiedOntologyBrowser
                '                        '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
                '                        If lrNORMAModel.ModelNote.Find(Function(x) x.Id = lrFBMModelNote.Id) IsNot Nothing Then GoTo SkipModelNote
                '                    End If

                '                    lrModelNote = New FBM.ModelNote(lrModel)

                '                    lrModelNote.Id = lrFBMModelNote.Id
                '                    lrModelNote.Text = lrFBMModelNote.Note

                '                    If lrFBMModelNote.JoinedObjectTypeId = "" Then
                '                        lrModelNote.JoinedObjectType = Nothing
                '                    Else
                '                        lrModelNote.JoinedObjectType = lrNORMAModel.EntityType.Find(Function(x) x.Id = lrFBMModelNote.JoinedObjectTypeId)
                '                        If lrModelNote.JoinedObjectType Is Nothing Then
                '                            lrModelNote.JoinedObjectType = lrNORMAModel.FactType.Find(Function(x) x.Id = lrFBMModelNote.JoinedObjectTypeId)
                '                            If lrModelNote.JoinedObjectType Is Nothing Then
                '                                lrModelNote.JoinedObjectType = lrNORMAModel.ValueType.Find(Function(x) x.Id = lrFBMModelNote.JoinedObjectTypeId)
                '                            End If
                '                        End If
                '                    End If
                '                    lrNORMAModel.AddModelNote(lrModelNote, False)
                'SkipModelNote:
                '                Next

                '                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(70)

                '                '----------------------------------------------------------------------------------------------
                '                'Reference any FactType.LinkFactTypeRole values that are NOTHING
                '                Dim larFactType = From [FactType] In lrNORMAModel.FactType
                '                                  Where FactType.LinkFactTypeRole Is Nothing _
                '                                  And FactType.IsLinkFactType = True
                '                                  Select FactType

                '                For Each lrFactType In larFactType
                '                    MsgBox("Error: FactType: '" & lrFactType.Name & "' has no LinkFactTypeRole.")
                '                Next
#End Region

                '=====================
                'Map the Pages
                '=====================
                Call Me.MapToORMDiagrams(lrORMDocument, aoBackgroundWorker)

                Return lrORMDocument

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

#End Region

#Region "NORMA Old"

        '        ''' <summary>
        '        ''' Maps an instance of this class to an instance of FBM.Model
        '        ''' </summary>
        '        ''' <param name="arNORMADocument">The FBM Model to map to.</param>
        '        ''' <param name="aoBackgroundWorker">For reporting progress. Start at 60%.</param>        
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Function MapToNORMAORMModel(Optional ByRef arNORMADocument As NORMA.ORMDocument = Nothing,
        '                                           Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing
        '                                           ) As NORMA.ORMDocument

        '            Try

        '                Dim lsMessage As String = ""
        '                Dim lrORMDocument As New NORMA.ORMDocument
        '                Dim lrModel As New NORMA.ORMModel

        '                'Variables for the FBM model element types.
        '                Dim lrFBMValueType As XMLModel.ValueType = Nothing
        '                Dim lrFBMEntityType As XMLModel.EntityType
        '                Dim lrFBMFactType As XMLModel.FactType
        '                Dim lrFBMRoleConstraint As XMLModel.RoleConstraint
        '                Dim lrFBMRoleConstraintRole As XMLModel.RoleConstraintRole

        '                If arNORMADocument IsNot Nothing Then
        '                    lrORMDocument = arNORMADocument
        '                    lrModel = arNORMADocument.ORMModel
        '                    lrModel.Id = "_" & Me.ORMModel.ModelId.ToUpper()
        '                    lrModel.Name = Me.ORMModel.Name
        '                Else
        '                    lrModel = New NORMA.ORMModel(Me.ORMModel.ModelId.ToUpper(), Me.ORMModel.Name)
        '                End If

        '                '==============================
        '                'Create the DataTypes
        '                '==============================
        '#Region "DataTypes"
        '                lrORMDocument.ORMModel.DataTypes = New NORMA.ORMModelDataTypes

        '                lrORMDocument.ORMModel.DataTypes.UnspecifiedDataType = New NORMA.Model.UnspecifiedDataType
        '                lrORMDocument.ORMModel.DataTypes.AutoCounterNumericDataType = New NORMA.Model.AutoCounterNumericDataType()
        '                lrORMDocument.ORMModel.DataTypes.FixedLengthTextDataType = New NORMA.Model.FixedLengthTextDataType()
        '                lrORMDocument.ORMModel.DataTypes.DateTemporalDataType = New NORMA.Model.DateTemporalDataType()
        '                lrORMDocument.ORMModel.DataTypes.VariableLengthTextDataType = New NORMA.Model.VariableLengthTextDataType
        '                lrORMDocument.ORMModel.DataTypes.LargeLengthTextDataType = New NORMA.Model.LargeLengthTextDataType
        '                lrORMDocument.ORMModel.DataTypes.SignedIntegerNumericDataType = New NORMA.Model.SignedIntegerNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.SignedSmallIntegerNumericDataType = New NORMA.Model.SignedSmallIntegerNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.SignedLargeIntegerNumericDataType = New NORMA.Model.SignedLargeIntegerNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.UnsignedTinyIntegerNumericDataType = New NORMA.Model.UnsignedTinyIntegerNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.UnsignedSmallIntegerNumericDataType = New NORMA.Model.UnsignedSmallIntegerNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.UnsignedIntegerNumericDataType = New NORMA.Model.UnsignedIntegerNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.AutoCounterNumericDataType = New NORMA.Model.AutoCounterNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.FloatingPointNumericDataType = New NORMA.Model.FloatingPointNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.SinglePrecisionFloatingPointNumericDataType = New NORMA.Model.SinglePrecisionFloatingPointNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.DoublePrecisionFloatingPointNumericDataType = New NORMA.Model.DoublePrecisionFloatingPointNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.DecimalNumericDataType = New NORMA.Model.DecimalNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.MoneyNumericDataType = New NORMA.Model.MoneyNumericDataType
        '                lrORMDocument.ORMModel.DataTypes.FixedLengthRawDataDataType = New NORMA.Model.FixedLengthRawDataDataType
        '                lrORMDocument.ORMModel.DataTypes.VariableLengthRawDataDataType = New NORMA.Model.VariableLengthRawDataDataType
        '                lrORMDocument.ORMModel.DataTypes.LargeLengthRawDataDataType = New NORMA.Model.LargeLengthRawDataDataType
        '                lrORMDocument.ORMModel.DataTypes.PictureRawDataDataType = New NORMA.Model.PictureRawDataDataType
        '                lrORMDocument.ORMModel.DataTypes.OleObjectRawDataDataType = New NORMA.Model.OleObjectRawDataDataType
        '                lrORMDocument.ORMModel.DataTypes.AutoTimestampTemporalDataType = New NORMA.Model.AutoTimestampTemporalDataType
        '                lrORMDocument.ORMModel.DataTypes.TimeTemporalDataType = New NORMA.Model.TimeTemporalDataType
        '                lrORMDocument.ORMModel.DataTypes.DateTemporalDataType = New NORMA.Model.DateTemporalDataType
        '                lrORMDocument.ORMModel.DataTypes.DateAndTimeTemporalDataType = New NORMA.Model.DateAndTimeTemporalDataType
        '                lrORMDocument.ORMModel.DataTypes.TrueOrFalseLogicalDataType = New NORMA.Model.TrueOrFalseLogicalDataType
        '                lrORMDocument.ORMModel.DataTypes.YesOrNoLogicalDataType = New NORMA.Model.YesOrNoLogicalDataType
        '                lrORMDocument.ORMModel.DataTypes.RowIdOtherDataType = New NORMA.Model.RowIdOtherDataType
        '                lrORMDocument.ORMModel.DataTypes.ObjectIdOtherDataType = New NORMA.Model.ObjectIdOtherDataType
        '#End Region

        '                '==============================
        '                'Create ALLCAPS GUIDs
        '                '==============================
        '#Region "Create ALLCAPS Ids"
        '                For Each lrFBMValueType In Me.ORMModel.ValueTypes
        '                    lrFBMValueType.GUID = System.Guid.NewGuid.ToString.ToUpper
        '                Next
        '                For Each lrFBMEntityType In Me.ORMModel.EntityTypes
        '                    lrFBMEntityType.GUID = System.Guid.NewGuid.ToString.ToUpper
        '                    'lrFBMEntityType.ReferenceModeValueTypeId = lrFBMEntityType.ReferenceModeValueTypeId.ToUpper
        '                    'lrFBMEntityType.ReferenceSchemeRoleConstraintId = lrFBMEntityType.ReferenceSchemeRoleConstraintId.ToUpper
        '                Next
        '                For Each lrFBMFactType In Me.ORMModel.FactTypes
        '                    lrFBMFactType.GUID = System.Guid.NewGuid.ToString.ToUpper

        '                    For Each lrFBMRole In lrFBMFactType.RoleGroup
        '                        lrFBMRole.Id = lrFBMRole.Id.ToUpper
        '                    Next

        '                    For Each lrFBMFactTypeReading In lrFBMFactType.FactTypeReadings
        '                        lrFBMFactTypeReading.Id = lrFBMFactTypeReading.Id.ToUpper
        '                        For Each lrFBMPredicatePart In lrFBMFactTypeReading.PredicateParts
        '                            lrFBMPredicatePart.Role_Id = lrFBMPredicatePart.Role_Id.ToUpper
        '                        Next
        '                    Next
        '                Next
        '                For Each lrFBMRoleConstraint In Me.ORMModel.RoleConstraints
        '                    lrFBMRoleConstraint.GUID = System.Guid.NewGuid.ToString.ToUpper

        '                    For Each lrFBMRoleConstraintRole In lrFBMRoleConstraint.RoleConstraintRoles
        '                        lrFBMRoleConstraintRole.RoleId = lrFBMRoleConstraintRole.RoleId.ToUpper
        '                    Next

        '                    For Each lrFBMArgument In lrFBMRoleConstraint.Argument
        '                        lrFBMArgument.Id = System.Guid.NewGuid.ToString.ToUpper
        '                        For Each lrFBMRoleReference In lrFBMArgument.Role
        '                            lrFBMRoleReference.RoleId = lrFBMRoleReference.RoleId.ToUpper
        '                        Next
        '                    Next
        '                Next
        '#End Region

        '                '==============================
        '                'Map the ValueTypes
        '                '==============================
        '#Region "ValueTypes"
        '                Dim lrValueType As NORMA.Model.ValueType = Nothing
        '                Dim lsValueTypeConstraintValue As String = ""

        '                For Each lrFBMValueType In Me.ORMModel.ValueTypes

        '                    lrValueType = New NORMA.Model.ValueType
        '                    lrValueType.Id = "_" & lrFBMValueType.GUID
        '                    lrValueType.Name = lrFBMValueType.Name

        '                    Dim lsDataTypeRef As String = ""
        '                    Select Case lrFBMValueType.DataType
        '                        Case Is = pcenumORMDataType.DataTypeNotSet
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.UnspecifiedDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.NumericAutoCounter
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.AutoCounterNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.TextFixedLength
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.FixedLengthTextDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.NumericFloatCustomPrecision
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.FloatingPointNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                            lrValueType.ConceptualDataType.Length = lrFBMValueType.DataTypePrecision
        '                        Case Is = pcenumORMDataType.TemporalDate
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.DateTemporalDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.TextVariableLength
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.VariableLengthTextDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, Nothing, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.TextLargeLength
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.LargeLengthTextDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, Nothing, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.NumericSignedInteger
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.SignedIntegerNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.NumericSignedSmallInteger
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.SignedSmallIntegerNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.NumericSignedBigInteger
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.SignedLargeIntegerNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.NumericUnsignedTinyInteger
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.UnsignedTinyIntegerNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.NumericUnsignedSmallInteger
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.UnsignedSmallIntegerNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.NumericUnsignedInteger
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.UnsignedIntegerNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.NumericFloatSinglePrecision
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.SinglePrecisionFloatingPointNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, Nothing)
        '                        Case Is = pcenumORMDataType.NumericFloatDoublePrecision
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.DoublePrecisionFloatingPointNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, Nothing)
        '                        Case Is = pcenumORMDataType.NumericDecimal
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.DecimalNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.NumericMoney
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.MoneyNumericDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, lrFBMValueType.DataTypePrecision, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.RawDataFixedLength
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.FixedLengthRawDataDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, Nothing, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.RawDataVariableLength
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.VariableLengthRawDataDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef, Nothing, lrFBMValueType.DataTypeLength)
        '                        Case Is = pcenumORMDataType.RawDataLargeLength
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.LargeLengthRawDataDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.RawDataPicture
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.PictureRawDataDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.RawDataOLEObject
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.OleObjectRawDataDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.TemporalAutoTimestamp
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.AutoTimestampTemporalDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.TemporalTime
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.TimeTemporalDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.TemporalDateAndTime
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.DateAndTimeTemporalDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.LogicalTrueFalse
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.TrueOrFalseLogicalDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.LogicalYesNo
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.YesOrNoLogicalDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.OtherRowID
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.RowIdOtherDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Is = pcenumORMDataType.OtherObjectID
        '                            lsDataTypeRef = lrORMDocument.ORMModel.DataTypes.ObjectIdOtherDataType.Id
        '                            lrValueType.ConceptualDataType = New NORMA.Model.ValueType.ValueTypeConceptualDataType(lsDataTypeRef)
        '                        Case Else
        '                            Throw New NotImplementedException($"No implementation is found for {lrFBMValueType.DataType} in 'ValueType'")
        '                    End Select

        '                    'lrValueType.IsIndependent = lrFBMValueType.IsIndependent

        '                    ''==============================
        '                    ''check and apply the ValueConstraint if available
        '                    ''==============================
        '                    'If Not IsNothing(lrFBMValueType.ValueConstraint) AndAlso lrFBMValueType.ValueConstraint.Count > 0 Then
        '                    '    lrValueType.ValueRestriction.ValueConstraint = New NORMA.Model.ValueType.ValueTypeRestrictionValueConstraint()
        '                    '    For Each lsValueTypeConstraintValue In lrFBMValueType.ValueConstraint
        '                    '        lrValueType.ValueConstraint.Add(lsValueTypeConstraintValue)
        '                    '        lrValueType.Instance.Add(lsValueTypeConstraintValue)
        '                    '    Next
        '                    'End If

        '                    lrModel.Objects.Items.Add(lrValueType)
        'SkipValueType:
        '                Next
        '#End Region

        '                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(20)

        '                '==============================
        '                'Map the EntityTypes
        '                '==============================
        '#Region "EntityTypes"
        '                Dim lrEntityType As NORMA.Model.EntityType

        '                For Each lrFBMEntityType In Me.ORMModel.EntityTypes

        '                    If lrFBMEntityType.IsObjectifyingEntityType Then Continue For

        '                    lrEntityType = New NORMA.Model.EntityType
        '                    lrEntityType.Id = "_" & lrFBMEntityType.GUID
        '                    lrEntityType.Name = lrFBMEntityType.Name

        '                    'lrEntityType.DerivationText = lrFBMEntityType.DerivationText
        '                    ''lrEntityType.ShortDescription = 
        '                    ''lrEntityType.LongDescription = 
        '                    'lrEntityType.IsObjectifyingEntityType = lrFBMEntityType.IsObjectifyingEntityType
        '                    'lrEntityType.IsAbsorbed = lrFBMEntityType.IsAbsorbed

        '                    lrEntityType._ReferenceMode = lrFBMEntityType.ReferenceMode.TrimStart("."c)
        '                    'If lrFBMEntityType.ReferenceModeValueTypeId = "" Then
        '                    '    lrEntityType.ReferenceModeValueType = Nothing
        '                    'Else
        '                    '    lrEntityType.ReferenceModeValueType = New FBM.ValueType
        '                    '    lrEntityType.ReferenceModeValueType.Id = lrFBMEntityType.ReferenceModeValueTypeId
        '                    '    lrEntityType.ReferenceModeValueType = lrModel.ValueType.Find(AddressOf lrEntityType.ReferenceModeValueType.Equals)
        '                    'End If

        '                    'lrEntityType.PreferredIdentifierRCId = lrFBMEntityType.ReferenceSchemeRoleConstraintId

        '                    ''------------------------------------------------
        '                    ''Link to the Concept within the ModelDictionary
        '                    ''------------------------------------------------
        '                    'Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrEntityType.Name, pcenumConceptType.EntityType, , , True, True, lrEntityType.DBName)
        '                    'lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , True,, True,, True)

        '                    'lrEntityType.Concept = lrDictionaryEntry.Concept

        '                    lrModel.Objects.Items.Add(lrEntityType)
        'SkipEntityType:
        '                Next
        '#End Region

        '                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(30)

        '                '==============================
        '                'Map the FactTypes
        '                '==============================
        '#Region "FactTypes"
        '                Dim lrFactType As NORMA.Model.Fact

        '                For Each lrFBMFactType In Me.ORMModel.FactTypes

        '                    If lrFBMFactType.IsObjectified Then
        '                        Dim larExistingFactType = From lrObject In arNORMADocument.ORMModel.Objects.Items
        '                                                  Where GetType(NORMA.Model.ObjectifiedType) = lrObject.GetType AndAlso lrObject.Name = lrFBMFactType.Id
        '                                                  Select lrObject

        '                        If larExistingFactType.Count > 0 Then Continue For
        '                    End If

        '                    lrFactType = New NORMA.Model.Fact("_" & lrFBMFactType.GUID, lrFBMFactType.Name)
        '                    Call Me.GetNORMAFactTypeDetails(lrORMDocument, lrFactType, lrFBMFactType)

        '                    If IsNothing(lrORMDocument.ORMModel.Facts) Then
        '                        lrORMDocument.ORMModel.Facts = New NORMA.ORMModelFacts()
        '                        lrORMDocument.ORMModel.Facts.Items = Array.CreateInstance(GetType(Object), 0)
        '                    End If
        '                    lrModel.Facts.Items.Add(lrFactType)

        '                Next

        '                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(66)

        '                '                '==============================
        '                '                'Subtype Relationships
        '                '                '==============================
        '#Region "Subtype Relationships"
        '                '                Dim larSubtypeRelationshipFactTypes = From FactType In Me.ORMModel.FactTypes
        '                '                                                      Where FactType.IsSubtypeRelationshipFactType
        '                '                                                      Select FactType

        '                '                Dim lrModelElement As FBM.ModelObject
        '                '                For Each lrFBMFactType In larSubtypeRelationshipFactTypes

        '                '                    If abSkipAlreadyLoadedModelElements Then
        '                '                        'Really only ever called when using the UnifiedOntologyBrowser
        '                '                        '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
        '                '                        If lrModel.FactType.Find(Function(x) x.Id = lrFBMFactType.Id) IsNot Nothing Then GoTo SkipSubtypeRelationship
        '                '                    End If

        '                '                    lrFactType = New FBM.FactType(lrFBMFactType.Id, True)
        '                '                    lrFactType = lrModel.FactType.Find(AddressOf lrFactType.Equals)

        '                '                    Dim lrParentModelElement As FBM.ModelObject
        '                '                    lrModelElement = lrModel.GetModelObjectByName(lrFactType.RoleGroup(0).JoinedORMObject.Id)
        '                '                    lrParentModelElement = lrModel.GetModelObjectByName(lrFactType.RoleGroup(1).JoinedORMObject.Id)
        '                '                    If lrParentModelElement.GetType = GetType(FBM.FactType) Then
        '                '                        lrParentModelElement = CType(lrParentModelElement, FBM.FactType).ObjectifyingEntityType
        '                '                    End If
        '                '                    lrModelElement.parentModelObjectList.Add(lrParentModelElement)
        '                '                    lrParentModelElement.childModelObjectList.Add(lrModelElement)

        '                '                    Dim lrSubtypeConstraint As New FBM.SubtypeRelationship(lrModelElement, lrParentModelElement, lrFactType)

        '                '                    Select Case lrModelElement.GetType
        '                '                        Case Is = GetType(FBM.EntityType)
        '                '                            Dim larSubtypeRelationship = From EntityType In Me.ORMModel.EntityTypes
        '                '                                                         From SubtypeRelationship In EntityType.SubtypeRelationships
        '                '                                                         Where SubtypeRelationship.SubtypingFactTypeId = lrFactType.Id
        '                '                                                         Select SubtypeRelationship
        '                '                            Try
        '                '                                lrSubtypeConstraint.IsPrimarySubtypeRelationship = larSubtypeRelationship.First.IsPrimarySubtypeRelationship
        '                '                            Catch ex As Exception
        '                '                                'CodeSafe
        '                '                                'Not a biggie at this stage.
        '                '                            End Try

        '                '                        Case Is = GetType(FBM.ValueType)
        '                '                            Dim larSubtypeRelationship = From ValueType In Me.ORMModel.ValueTypes
        '                '                                                         From SubtypeRelationship In ValueType.SubtypeRelationships
        '                '                                                         Where SubtypeRelationship.SubtypingFactTypeId = lrFactType.Id
        '                '                                                         Select SubtypeRelationship
        '                '                            Try
        '                '                                lrSubtypeConstraint.IsPrimarySubtypeRelationship = larSubtypeRelationship.First.IsPrimarySubtypeRelationship
        '                '                            Catch ex As Exception
        '                '                                'CodeSafe
        '                '                                'Not a biggie at this stage.
        '                '                            End Try

        '                '                    End Select

        '                '                    lrModelElement.SubtypeRelationship.AddUnique(lrSubtypeConstraint)
        '                'SkipSubtypeRelationship:
        '                '                Next
        '#End Region 'Subtype Relationships

        '#End Region
        '                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(50)

        '                '==============================
        '                'Map the RoleConstraints
        '                '==============================
        '#Region "RoleConstraints"
        '                For Each lrFBMRoleConstraint In Me.ORMModel.RoleConstraints

        '                    Select Case lrFBMRoleConstraint.RoleConstraintType
        '#Region "Ring Constraint"
        '                        Case Is = pcenumRoleConstraintType.RingConstraint.ToString

        '                            Dim lrNORMARoleConstraint As New NORMA.Model.RingConstraintType

        '                            lrNORMARoleConstraint.id = "_" & lrFBMRoleConstraint.GUID
        '                            lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name
        '                            lrNORMARoleConstraint.Type = CType([Enum].Parse(GetType(NORMA.Model.RingConstraintTypeValues), lrFBMRoleConstraint.RingConstraintType), NORMA.Model.RingConstraintTypeValues)

        '                            '================================================
        '                            'Map the Ring Constraint
        '                            '========================================


        '                            Dim lrNORMARoleSequence = New NORMA.Model.ConstraintRoleSequenceWithJoinType
        '                            lrNORMARoleConstraint.RoleSequence = lrNORMARoleSequence

        '                            For Each lrFBMRoleConstraintRole In lrFBMRoleConstraint.RoleConstraintRoles

        '                                'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
        '                                Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
        '                                                            From Role In FactType.RoleGroup
        '                                                            Where Role.Id = lrFBMRoleConstraintRole.RoleId
        '                                                            Select New With {.FactType = FactType, .Role = Role}).First

        '                                'NORMA Model:
        '                                Dim lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
        '                                                       From FactType In lrModel.Facts.Items
        '                                                       From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                       Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
        '                                                       Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                       Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()

        '                                If lrRoleAndObject Is Nothing Then
        '                                    lrRoleAndObject = (From ModelElement In lrModel.Facts.Items
        '                                                       From FactType In lrModel.Facts.Items
        '                                                       From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                       Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
        '                                                       Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                       Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()
        '                                End If

        '                                lrNORMARoleSequence.Role.Add(New NORMA.Model.RoleSequenceWithProjectionRoleRef() With {.id = "_" & System.Guid.NewGuid.ToString, .ref = lrRoleAndObject.Role.Id})
        '                            Next

        '                            If IsNothing(arNORMADocument.ORMModel.Constraints) Then
        '                                arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
        '                                arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
        '                            End If

        '                            arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)
        '#End Region 'Ring Constraint
        '#Region "Equality Constraint"
        '                        Case Is = pcenumRoleConstraintType.EqualityConstraint.ToString

        '                            Dim lrNORMARoleConstraint As New NORMA.Model.EqualityConstraintType

        '                            lrNORMARoleConstraint.id = "_" & lrFBMRoleConstraint.GUID
        '                            lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name

        '                            '================================================
        '                            'Map the Equality Constraint
        '                            '========================================
        '                            For Each lrArgument In lrFBMRoleConstraint.Argument

        '                                Dim lrNORMARoleSequence = New NORMA.Model.ConstraintRoleSequenceWithJoinAndIdType With {.id = "_" & lrArgument.Id}
        '                                lrNORMARoleConstraint.RoleSequences.Add(lrNORMARoleSequence)

        '                                For Each lrFBMRoleReference In lrArgument.Role

        '                                    'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
        '                                    Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
        '                                                                From Role In FactType.RoleGroup
        '                                                                Where Role.Id = lrFBMRoleReference.RoleId
        '                                                                Select New With {.FactType = FactType, .Role = Role}).First

        '                                    'NORMA Model:
        '                                    Dim lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
        '                                                           From FactType In lrModel.Facts.Items
        '                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                           Where Role.Id = "_" & lrFBMRoleReference.RoleId
        '                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()

        '                                    If lrRoleAndObject Is Nothing Then
        '                                        lrRoleAndObject = (From ModelElement In lrModel.Facts.Items
        '                                                           From FactType In lrModel.Facts.Items
        '                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                           Where Role.Id = "_" & lrFBMRoleReference.RoleId
        '                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()
        '                                    End If

        '                                    lrNORMARoleSequence.Role.Add(New NORMA.Model.RoleSequenceWithProjectionRoleRef() With {.ref = lrRoleAndObject.Role.Id})
        '                                Next
        '                            Next

        '                            If IsNothing(arNORMADocument.ORMModel.Constraints) Then
        '                                arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
        '                                arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
        '                            End If

        '                            arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)
        '#End Region 'Equality Constraint
        '                        Case Is = "SubsetConstraint"
        '#Region "Subset Constraint"
        '                            Dim lrNORMARoleConstraint As New NORMA.Model.SubsetConstraint()

        '                            lrNORMARoleConstraint.Id = "_" & lrFBMRoleConstraint.GUID
        '                            lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name

        '                            '================================================
        '                            'Map the Subset Constraint
        '                            '========================================
        '                            For Each lrArgument In lrFBMRoleConstraint.Argument

        '                                Dim lrNORMARoleSequence = New NORMA.Model.SubsetConstraint.ConstraintRoleSequence With {.Id = "_" & lrArgument.Id}
        '                                lrNORMARoleConstraint.RoleSequences.Add(lrNORMARoleSequence)

        '                                For Each lrFBMRoleReference In lrArgument.Role

        '                                    'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
        '                                    Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
        '                                                                From Role In FactType.RoleGroup
        '                                                                Where Role.Id = lrFBMRoleReference.RoleId
        '                                                                Select New With {.FactType = FactType, .Role = Role}).First

        '                                    'NORMA Model:
        '                                    Dim lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
        '                                                           From FactType In lrModel.Facts.Items
        '                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                           Where Role.Id = "_" & lrFBMRoleReference.RoleId
        '                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()

        '                                    If lrRoleAndObject Is Nothing Then
        '                                        lrRoleAndObject = (From ModelElement In lrModel.Facts.Items
        '                                                           From FactType In lrModel.Facts.Items
        '                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                           Where Role.Id = "_" & lrFBMRoleReference.RoleId
        '                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()
        '                                    End If

        '                                    lrNORMARoleSequence.Role.Add(New NORMA.Model.SubsetConstraint.SequenceRole() With {.Ref = lrRoleAndObject.Role.Id})
        '                                Next
        '                            Next

        '                            If IsNothing(arNORMADocument.ORMModel.Constraints) Then
        '                                arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
        '                                arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
        '                            End If

        '                            arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)
        '#End Region 'Subset Constraint
        '                        Case Is = "InternalUniquenessConstraint"
        '#Region "Internal Uniqueness Constraint"
        '                            '================================================
        '                            'Map the InternalConstraints for Fact Type
        '                            '===========================================

        '                            Try
        '                                Dim lrNORMARoleConstraint As New NORMA.Model.UniquenessConstraint()

        '                                lrNORMARoleConstraint = New NORMA.Model.UniquenessConstraint()

        '                                lrNORMARoleConstraint.Id = "_" & lrFBMRoleConstraint.GUID
        '                                lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name
        '                                lrNORMARoleConstraint.RoleSequence = New NORMA.Model.UniquenessConstraint.ConstraintRole() {}
        '                                lrNORMARoleConstraint.IsInternal = True

        '                                Dim lrNORMAFactType As NORMA.Model.Fact = Nothing

        '                                For Each lrFBMRoleConstraintRole In lrFBMRoleConstraint.RoleConstraintRoles

        '                                    'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
        '                                    Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
        '                                                                From Role In FactType.RoleGroup
        '                                                                Where Role.Id = lrFBMRoleConstraintRole.RoleId
        '                                                                Select New With {.FactType = FactType, .Role = Role}).First

        '                                    'NORMA Model:
        '                                    Dim lrRoleAndObject As Object
        '                                    Try
        '                                        lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
        '                                                           From FactType In lrModel.Facts.Items
        '                                                           Where CType(FactType, NORMA.Model.Fact).FactRoles IsNot Nothing
        '                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                           Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
        '                                                           Where ModelElement.Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()
        '                                    Catch ex As Exception
        '                                        lrRoleAndObject = Nothing
        '                                    End Try

        '                                    If lrRoleAndObject Is Nothing Then
        '                                        lrRoleAndObject = (From ModelElement In lrModel.Facts.Items
        '                                                           From FactType In lrModel.Facts.Items
        '                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                           Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
        '                                                           Where ModelElement.Name = lrFBMFactTypeAndRole.Role.JoinedObjectType.Id 'Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()
        '                                    End If

        '                                    lrNORMAFactType = lrRoleAndObject.FactType

        '                                    lrNORMARoleConstraint.RoleSequence.Add(New NORMA.Model.UniquenessConstraint.ConstraintRole() With {.Ref = lrRoleAndObject.Role.Id})

        '                                    If lrRoleAndObject.Role._IsMandatory Then
        '                                        lrRoleAndObject.Role._Multiplicity = "ZeroToOne"
        '                                    Else
        '                                        lrRoleAndObject.Role._Multiplicity = "ExactlyOne"
        '                                    End If

        '                                    If lrFBMRoleConstraint.IsPreferredUniqueness And lrFBMRoleConstraint.RoleConstraintRoles.Count = 1 And CType(lrRoleAndObject.FactType, NORMA.Model.Fact).FactRoles.Count = 2 Then

        '                                        Dim lrNORMAObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
        '                                                             From Role In CType(lrRoleAndObject.FactType, NORMA.Model.Fact).FactRoles
        '                                                             Where Role.Id <> lrRoleAndObject.Role.Id
        '                                                             Where ModelElement.Id = Role.RolePlayer.Ref
        '                                                             Select ModelElement).First

        '                                        lrNORMARoleConstraint.PreferredIdentifierFor = New NORMA.Model.UniquenessConstraint.ConstraintPreferredIdentifierFor()
        '                                        lrNORMARoleConstraint.PreferredIdentifierFor.Ref = lrNORMAObject.Id

        '                                        If lrNORMAObject.GetType() = GetType(NORMA.Model.EntityType) Then
        '                                            CType(lrNORMAObject, NORMA.Model.EntityType).PreferredIdentifier = New NORMA.Model.EntityType.EntityTypePreferredIdentifier()
        '                                            CType(lrNORMAObject, NORMA.Model.EntityType).PreferredIdentifier.Ref = lrNORMARoleConstraint.Id
        '                                        End If

        '                                    End If

        '                                Next

        '                                If IsNothing(lrNORMAFactType.InternalConstraints) Then
        '                                    lrNORMAFactType.InternalConstraints = New NORMA.Model.Fact.FactInternalConstraints()
        '                                    lrNORMAFactType.InternalConstraints.Items = New List(Of Object)
        '                                End If
        '                                lrNORMAFactType.InternalConstraints.Items.Add(New NORMA.Model.Fact.FactInternalConstraints.UniquenessConstraint() With {.Ref = lrNORMARoleConstraint.Id})

        '                                If IsNothing(arNORMADocument.ORMModel.Constraints) Then
        '                                    arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
        '                                    arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
        '                                End If

        '                                arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)

        '                            Catch ex As Exception
        '                                prApplication.ThrowMessage("Error loading Internal Uniqueness Constraint: " & lrFBMRoleConstraint.Id, pcenumErrorType.Warning,, False,, True,,, ex)
        '                            End Try

        '#End Region 'Enternal Uniqueness Constraint
        '                        Case "ExternalUniquenessConstraint"
        '#Region "External Uniqueness Constraint"

        '                            Try
        '                                Dim lrNORMARoleConstraint As New NORMA.Model.UniquenessConstraint()

        '                                lrNORMARoleConstraint = New NORMA.Model.UniquenessConstraint()

        '                                lrNORMARoleConstraint.Id = "_" & lrFBMRoleConstraint.GUID
        '                                lrNORMARoleConstraint.Name = lrFBMRoleConstraint.Name
        '                                lrNORMARoleConstraint.RoleSequence = New NORMA.Model.UniquenessConstraint.ConstraintRole() {}

        '                                lrNORMARoleConstraint.IsInternal = False

        '                                '================================================
        '                                'Map the External Uniqueness Constraint
        '                                '========================================
        '                                For Each lrFBMRoleConstraintRole In lrFBMRoleConstraint.RoleConstraintRoles

        '                                    Dim larFactType = From FactType In Me.ORMModel.FactTypes
        '                                                      Where FactType.Id.StartsWith("Therapy")
        '                                                      Select FactType

        '                                    'FactBasedModel: The FactType and Role that the RoleConstraintRole joins to.
        '                                    Dim lrFBMFactTypeAndRole = (From FactType In Me.ORMModel.FactTypes
        '                                                                From Role In FactType.RoleGroup
        '                                                                Where Role.Id = lrFBMRoleConstraintRole.RoleId
        '                                                                Select New With {.FactType = FactType, .Role = Role}).First

        '                                    'NORMA Model:
        '                                    Dim lrRoleAndObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
        '                                                           From FactType In lrModel.Facts.Items
        '                                                           Where CType(FactType, NORMA.Model.Fact).FactRoles IsNot Nothing
        '                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                           Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
        '                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()

        '                                    If lrRoleAndObject Is Nothing Then
        '                                        lrRoleAndObject = (From ModelElement In lrModel.Facts.Items
        '                                                           From FactType In lrModel.Facts.Items
        '                                                           From Role In CType(FactType, NORMA.Model.Fact).FactRoles
        '                                                           Where Role.Id = "_" & lrFBMRoleConstraintRole.RoleId
        '                                                           Where ModelElement.Id = "_" & lrFBMFactTypeAndRole.Role.JoinedObjectType.GUID
        '                                                           Select New With {.FactType = FactType, .Role = Role, .NORMAObject = ModelElement}).FirstOrDefault()
        '                                    End If

        '                                    lrNORMARoleConstraint.RoleSequence.Add(New NORMA.Model.UniquenessConstraint.ConstraintRole() With {.Ref = lrRoleAndObject.Role.Id})

        '                                    If lrRoleAndObject.Role._IsMandatory Then
        '                                        lrRoleAndObject.Role._Multiplicity = "ZeroToOne"
        '                                    Else
        '                                        lrRoleAndObject.Role._Multiplicity = "ExactlyOne"
        '                                    End If

        '                                    If lrFBMRoleConstraint.IsPreferredUniqueness Then

        '                                        Dim lrNORMAObject = (From ModelElement In arNORMADocument.ORMModel.Objects.Items
        '                                                             From Role In CType(lrRoleAndObject.FactType, NORMA.Model.Fact).FactRoles
        '                                                             Where Role.Id <> lrRoleAndObject.Role.Id
        '                                                             Where ModelElement.Id = Role.RolePlayer.Ref
        '                                                             Select ModelElement).First

        '                                        If lrNORMARoleConstraint.PreferredIdentifierFor Is Nothing Then
        '                                            CType(lrNORMAObject, NORMA.Model.EntityType).PreferredIdentifier = New NORMA.Model.EntityType.EntityTypePreferredIdentifier()
        '                                            CType(lrNORMAObject, NORMA.Model.EntityType).PreferredIdentifier.Ref = lrNORMARoleConstraint.Id

        '                                            lrNORMARoleConstraint.PreferredIdentifierFor = New NORMA.Model.UniquenessConstraint.ConstraintPreferredIdentifierFor()
        '                                            lrNORMARoleConstraint.PreferredIdentifierFor.Ref = lrNORMAObject.Id
        '                                        End If

        '                                    End If
        '                                Next

        '                                If IsNothing(arNORMADocument.ORMModel.Constraints) Then
        '                                    arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
        '                                    arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
        '                                End If

        '                                arNORMADocument.ORMModel.Constraints.Items.Add(lrNORMARoleConstraint)

        '                            Catch ex As Exception
        '                                prApplication.ThrowMessage("Error loading External Uniqueness Constraint: " & lrFBMRoleConstraint.Id, pcenumErrorType.Warning,, False,, True,,, ex)
        '                            End Try
        '#End Region 'External Uniqueness Constraint
        '                        Case Else
        '                            Continue For
        '                    End Select

        '#Region "Old Boston code"

        '                    '                    lrRoleConstraint = New FBM.RoleConstraint
        '                    '                    lrRoleConstraint.Id = lrFBMRoleConstraint.Id
        '                    '                    lrRoleConstraint.Model = lrModel
        '                    '                    lrRoleConstraint.Name = lrFBMRoleConstraint.Name
        '                    '                    'lrRoleConstraint.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
        '                    '                    'lrRoleConstraint.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
        '                    '                    lrRoleConstraint.ConceptType = pcenumConceptType.RoleConstraint
        '                    '                    lrRoleConstraint.RoleConstraintType = CType([Enum].Parse(GetType(pcenumRoleConstraintType), lrFBMRoleConstraint.RoleConstraintType), pcenumRoleConstraintType)
        '                    '                    lrRoleConstraint.RingConstraintType = CType([Enum].Parse(GetType(pcenumRingConstraintType), lrFBMRoleConstraint.RingConstraintType), pcenumRingConstraintType)
        '                    '                    'lrRoleConstraint.LevelNr = <See right down the bottom of this method>
        '                    '                    lrRoleConstraint.IsPreferredIdentifier = lrFBMRoleConstraint.IsPreferredUniqueness
        '                    '                    lrRoleConstraint.IsDeontic = lrFBMRoleConstraint.IsDeontic
        '                    '                    lrRoleConstraint.Cardinality = lrFBMRoleConstraint.Cardinality
        '                    '                    lrRoleConstraint.MaximumFrequencyCount = lrFBMRoleConstraint.MaximumFrequencyCount
        '                    '                    lrRoleConstraint.MinimumFrequencyCount = lrFBMRoleConstraint.MinimumFrequencyCount
        '                    '                    Select Case lrFBMRoleConstraint.CardinalityRangeType
        '                    '                        Case Is = pcenumCardinalityRangeType.LessThanOrEqual.ToString
        '                    '                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOrEqual
        '                    '                        Case Is = pcenumCardinalityRangeType.Equal.ToString
        '                    '                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Equal
        '                    '                        Case Is = pcenumCardinalityRangeType.GreaterThanOrEqual.ToString
        '                    '                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.GreaterThanOrEqual
        '                    '                        Case Is = pcenumCardinalityRangeType.Between.ToString
        '                    '                            lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Between
        '                    '                    End Select
        '                    '
        '                    '                    '----------------------------------------------------------
        '                    '                    'Get the RoleConstraintRole list for the RoleConstraint.
        '                    '                    '  NB the SequenceNr of a RoleConstraintRole is 'not' the same as a SequenceNr on a Role.
        '                    '                    '  SequenceNr on a RoleConstraintRole is many things, but
        '                    '                    '  relates particularly to DataIn, DataOut integrity matching.
        '                    '                    '----------------------------------------------------------

        '                    '=======================================================================================================
        '                    '                        lrRoleConstraintRole = New FBM.RoleConstraintRole
        '                    '                        lrRoleConstraintRole.Model = lrModel
        '                    '                        lrRoleConstraintRole.RoleConstraint = lrRoleConstraint
        '                    '                        lrRoleConstraintRole.SequenceNr = lrFBMRoleConstraintRole.SequenceNr
        '                    '                        lrRoleConstraintRole.IsEntry = lrFBMRoleConstraintRole.IsEntry
        '                    '                        lrRoleConstraintRole.IsExit = lrFBMRoleConstraintRole.IsExit


        '                    '                        lrRole.Id = lrFBMRoleConstraintRole.RoleId

        '                    '                        lrRoleConstraintRole.Role = lrModel.Role.Find(AddressOf lrRole.Equals)

        '                    '                        '-------------------------------------------------------------------
        '                    '                        'lrRoleConstraintRole.RoleConstraintArgument is set further below.
        '                    '                        '-------------------------------------------------------------------
        '                    '                        lrRoleConstraintRole.ArgumentSequenceNr = lrFBMRoleConstraintRole.ArgumentSequenceNr

        '                    '                        lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
        '                    '                        lrRoleConstraint.Role.Add(lrRoleConstraintRole.Role)
        '                    '                        lrRoleConstraintRole.Role.RoleConstraintRole.Add(lrRoleConstraintRole)
        '                    '                    Next

        '                    '                    If lrRoleConstraint.RoleConstraintRole.Count = 0 Then
        '                    '                        prApplication.ThrowMessage("No RoleConstraintRoles found for RoleConstraint.Id: " & lrRoleConstraint.Id, pcenumErrorType.Information)
        '                    '                    Else
        '                    '                        lrFactType = lrRoleConstraint.Role(0).FactType
        '                    '                        lrFactType = lrModel.FactType.Find(AddressOf lrFactType.Equals)

        '                    '                        Select Case lrRoleConstraint.RoleConstraintType
        '                    '                            Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
        '                    '                                Call lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)
        '                    '                        End Select
        '                    '                    End If

        '                    '                    Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument
        '                    '                    Dim lrFBMRoleConstraintArgument As XMLModel.RoleConstraintArgument
        '                    '                    Dim lrJoinPath As FBM.JoinPath
        '                    '                    Dim lrFBMRoleReference As XMLModel.RoleReference

        '                    '                    For Each lrFBMRoleConstraintArgument In lrFBMRoleConstraint.Argument
        '                    '                        lrRoleConstraintArgument = New FBM.RoleConstraintArgument(lrRoleConstraint,
        '                    '                                                                                  lrFBMRoleConstraintArgument.SequenceNr,
        '                    '                                                                                  lrFBMRoleConstraintArgument.Id)
        '                    '                        lrRoleConstraintArgument.isDirty = True

        '                    '                        lrJoinPath = New FBM.JoinPath(lrRoleConstraintArgument)
        '                    '                        lrJoinPath.JoinPathError = lrFBMRoleConstraintArgument.JoinPath.JoinPathError

        '                    '                        For Each lrFBMRoleReference In lrFBMRoleConstraintArgument.Role
        '                    '                            lrRole = New FBM.Role
        '                    '                            lrRole.Id = lrFBMRoleReference.RoleId
        '                    '                            lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint)
        '                    '                            lrRoleConstraintRole = lrRoleConstraint.RoleConstraintRole.Find(AddressOf lrRoleConstraintRole.EqualsByRole)
        '                    '                            lrRoleConstraintArgument.RoleConstraintRole.Add(lrRoleConstraintRole)
        '                    '                            lrRoleConstraintRole.RoleConstraintArgument = lrRoleConstraintArgument
        '                    '                        Next

        '                    '                        For Each lrFBMRoleReference In lrFBMRoleConstraintArgument.JoinPath.RolePath
        '                    '                            lrRole = New FBM.Role
        '                    '                            lrRole.Id = lrFBMRoleReference.RoleId
        '                    '                            lrRole = lrModel.Role.Find(AddressOf lrRole.Equals)
        '                    '                            lrJoinPath.RolePath.Add(lrRole)
        '                    '                        Next
        '                    '                        lrRoleConstraintArgument.JoinPath = lrJoinPath
        '                    '                        Call lrRoleConstraintArgument.JoinPath.ConstructFactTypePath()
        '                    '                        lrRoleConstraint.Argument.Add(lrRoleConstraintArgument)
        '                    '                    Next

        '                    '                    If (lrRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint) And (lrRoleConstraint.Role.Count > 0) Then
        '                    '                        lrRoleConstraint.LevelNr = lrRoleConstraint.Role(0).FactType.InternalUniquenessConstraint.Count
        '                    '                    End If

        '                    '                    'CodeSafe-RoleConstraint must have Roles
        '                    '                    If lrRoleConstraint.Role.Count > 0 Then
        '                    '                        lrModel.RoleConstraint.Add(lrRoleConstraint)
        '                    '                    End If

        '                    '                    For Each lsValueTypeConstraintValue In lrFBMRoleConstraint.ValueConstraint
        '                    '                        lrRoleConstraint.ValueConstraint.Add(lsValueTypeConstraintValue)
        '                    '                    Next
        '                    '==============================================================
        '#End Region

        'SkipRoleConstraint:
        '                Next

        '                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(60)

        '#End Region

        '                '==============================
        '                'Map the ModelNotes
        '                '==============================
        '#Region "ModelNotes"
        '                '                Dim lrFBMModelNote As XMLModel.ModelNote
        '                '                Dim lrModelNote As FBM.ModelNote

        '                '                For Each lrFBMModelNote In Me.ORMModel.ModelNotes

        '                '                    If abSkipAlreadyLoadedModelElements Then
        '                '                        'Really only ever called when using the UnifiedOntologyBrowser
        '                '                        '  and when need to load the rest of a Model when part of the Model has already been loaded inside the UnifiedOntologyBrowser.
        '                '                        If lrModel.ModelNote.Find(Function(x) x.Id = lrFBMModelNote.Id) IsNot Nothing Then GoTo SkipModelNote
        '                '                    End If

        '                '                    lrModelNote = New FBM.ModelNote(lrModel)

        '                '                    lrModelNote.Id = lrFBMModelNote.Id
        '                '                    lrModelNote.Text = lrFBMModelNote.Note

        '                '                    If lrFBMModelNote.JoinedObjectTypeId = "" Then
        '                '                        lrModelNote.JoinedObjectType = Nothing
        '                '                    Else
        '                '                        lrModelNote.JoinedObjectType = lrModel.EntityType.Find(Function(x) x.Id = lrFBMModelNote.JoinedObjectTypeId)
        '                '                        If lrModelNote.JoinedObjectType Is Nothing Then
        '                '                            lrModelNote.JoinedObjectType = lrModel.FactType.Find(Function(x) x.Id = lrFBMModelNote.JoinedObjectTypeId)
        '                '                            If lrModelNote.JoinedObjectType Is Nothing Then
        '                '                                lrModelNote.JoinedObjectType = lrModel.ValueType.Find(Function(x) x.Id = lrFBMModelNote.JoinedObjectTypeId)
        '                '                            End If
        '                '                        End If
        '                '                    End If
        '                '                    lrModel.AddModelNote(lrModelNote, False)
        '                'SkipModelNote:
        '                '                Next

        '                '                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(70)

        '                '                '----------------------------------------------------------------------------------------------
        '                '                'Reference any FactType.LinkFactTypeRole values that are NOTHING
        '                '                Dim larFactType = From [FactType] In lrModel.FactType
        '                '                                  Where FactType.LinkFactTypeRole Is Nothing _
        '                '                                  And FactType.IsLinkFactType = True
        '                '                                  Select FactType

        '                '                For Each lrFactType In larFactType
        '                '                    MsgBox("Error: FactType: '" & lrFactType.Name & "' has no LinkFactTypeRole.")
        '                '                Next
        '#End Region

        '                '=====================
        '                'Map the Pages
        '                '=====================
        '                Call Me.MapToORMDiagrams(lrORMDocument, aoBackgroundWorker)

        '                Call Boston.WriteToStatusBar("NORMA model populated", True, 0)

        '                Return lrORMDocument

        '            Catch ex As Exception
        '                Dim lsMessage1 As String
        '                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

        '                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
        '                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
        '                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

        '                Return Nothing
        '            End Try

        '        End Function



#End Region

#Region "NORMA Derivations"

        Private Function ConvertPathedRoleValueRestrictionToXmlModel(
    ByVal arRestriction As FBM.PathedRoleValueRestriction) _
    As XMLModel.PathedRoleValueRestriction

            If arRestriction Is Nothing OrElse
       arRestriction.PathedRoleConditionValueConstraint Is Nothing Then
                Return Nothing
            End If

            Dim lrXMLRestriction As New XMLModel.PathedRoleValueRestriction
            Dim lrConstraint As FBM.PathedRoleConditionValueConstraint =
        arRestriction.PathedRoleConditionValueConstraint
            Dim lrXMLConstraint As New XMLModel.PathedRoleConditionValueConstraint

            lrXMLConstraint.id = lrConstraint.id

            If lrConstraint.ValueRange IsNot Nothing Then
                For Each lrRange As FBM.PathedRoleValueRange In
            lrConstraint.ValueRange

                    Dim lrXMLRange As New XMLModel.PathedRoleValueRange

                    lrXMLRange.id = lrRange.id
                    lrXMLRange.MinValue = lrRange.MinValue
                    lrXMLRange.InvariantMinValue = lrRange.InvariantMinValue
                    lrXMLRange.MaxValue = lrRange.MaxValue
                    lrXMLRange.InvariantMaxValue = lrRange.InvariantMaxValue
                    lrXMLRange.MinInclusion = lrRange.MinInclusion
                    lrXMLRange.MaxInclusion = lrRange.MaxInclusion

                    lrXMLConstraint.ValueRange.Add(lrXMLRange)
                Next
            End If

            lrXMLRestriction.PathedRoleConditionValueConstraint =
        lrXMLConstraint

            Return lrXMLRestriction

        End Function

        Private Function ConvertPathedRoleValueRestrictionFromXmlModel(
    ByVal arRestriction As XMLModel.PathedRoleValueRestriction) _
    As FBM.PathedRoleValueRestriction

            If arRestriction Is Nothing OrElse
       arRestriction.PathedRoleConditionValueConstraint Is Nothing Then
                Return Nothing
            End If

            Dim lrRestriction As New FBM.PathedRoleValueRestriction
            Dim lrXMLConstraint As XMLModel.PathedRoleConditionValueConstraint =
        arRestriction.PathedRoleConditionValueConstraint
            Dim lrConstraint As New FBM.PathedRoleConditionValueConstraint

            lrConstraint.id = lrXMLConstraint.id

            If lrXMLConstraint.ValueRange IsNot Nothing Then
                For Each lrXMLRange As XMLModel.PathedRoleValueRange In
            lrXMLConstraint.ValueRange

                    Dim lrRange As New FBM.PathedRoleValueRange

                    lrRange.id = lrXMLRange.id
                    lrRange.MinValue = lrXMLRange.MinValue
                    lrRange.InvariantMinValue = lrXMLRange.InvariantMinValue
                    lrRange.MaxValue = lrXMLRange.MaxValue
                    lrRange.InvariantMaxValue = lrXMLRange.InvariantMaxValue
                    lrRange.MinInclusion = lrXMLRange.MinInclusion
                    lrRange.MaxInclusion = lrXMLRange.MaxInclusion

                    lrConstraint.ValueRange.Add(lrRange)
                Next
            End If

            lrRestriction.PathedRoleConditionValueConstraint = lrConstraint

            Return lrRestriction

        End Function

        Private Function ConvertRoleSubPathToXmlModel(ByVal arSubPath As FBM.RoleSubPath) As XMLModel.RoleSubPath

            If arSubPath Is Nothing Then Return Nothing

            Dim lrXMLSubPath As New XMLModel.RoleSubPath

            lrXMLSubPath.Id = arSubPath.Id
            lrXMLSubPath.SplitCombinationOperator = arSubPath.SplitCombinationOperator

            If arSubPath.RootObjectType IsNot Nothing Then
                lrXMLSubPath.RootObjectType = New XMLModel.RootObjectType

                lrXMLSubPath.RootObjectType.Id = arSubPath.RootObjectType.id

                If arSubPath.RootObjectType.BostonModelElement IsNot Nothing Then
                    lrXMLSubPath.RootObjectType.ModelElementId = arSubPath.RootObjectType.BostonModelElement.Id
                Else
                    lrXMLSubPath.RootObjectType.ModelElementId = arSubPath.RootObjectType.ref
                End If

                lrXMLSubPath.RootObjectType.IsNegated = arSubPath.RootObjectType.IsNegated
            End If

            If arSubPath.PathedRole IsNot Nothing Then
                For Each lrPathedRole As FBM.PathedRole In arSubPath.PathedRole

                    Dim lrXMLPathedRole As New XMLModel.PathedRole

                    lrXMLPathedRole.id = lrPathedRole.id
                    lrXMLPathedRole.Ref = lrPathedRole.Ref
                    lrXMLPathedRole.IsNegated = lrPathedRole.IsNegated
                    lrXMLPathedRole.Purpose = lrPathedRole.Purpose

                    lrXMLPathedRole.ValueRestriction = ConvertPathedRoleValueRestrictionToXmlModel(lrPathedRole.ValueRestriction)

                    lrXMLSubPath.PathedRole.Add(lrXMLPathedRole)
                Next
            End If

            If arSubPath.SubPath IsNot Nothing Then
                For Each lrChildSubPath As FBM.RoleSubPath In arSubPath.SubPath

                    Dim lrXMLChild As XMLModel.RoleSubPath = ConvertRoleSubPathToXmlModel(lrChildSubPath)

                    If lrXMLChild IsNot Nothing Then
                        lrXMLSubPath.SubPath.Add(lrXMLChild)
                    End If
                Next
            End If

            Return lrXMLSubPath

        End Function

        Private Function ConvertRoleSubPathFromXmlModel(ByVal arXMLSubPath As XMLModel.RoleSubPath,
                                                        ByVal arModel As FBM.Model) As FBM.RoleSubPath

            If arXMLSubPath Is Nothing Then Return Nothing

            Dim lrSubPath As New FBM.RoleSubPath

            lrSubPath.Id = arXMLSubPath.Id
            lrSubPath.SplitCombinationOperator = arXMLSubPath.SplitCombinationOperator

            If arXMLSubPath.RootObjectType IsNot Nothing Then

                Dim lrRoot As New FBM.RootObjectType

                lrRoot.id = arXMLSubPath.RootObjectType.Id
                lrRoot.ref = arXMLSubPath.RootObjectType.ModelElementId
                lrRoot.IsNegated = arXMLSubPath.RootObjectType.IsNegated

                'BostonModelElement is resolved after every Fact Type has loaded.
                lrSubPath.RootObjectType = lrRoot

            End If

            If arXMLSubPath.PathedRole IsNot Nothing Then
                For Each lrXMLPathedRole As XMLModel.PathedRole In arXMLSubPath.PathedRole

                    Dim lrPathedRole As New FBM.PathedRole

                    lrPathedRole.id = lrXMLPathedRole.id
                    lrPathedRole.Ref = lrXMLPathedRole.Ref
                    lrPathedRole.IsNegated = lrXMLPathedRole.IsNegated
                    lrPathedRole.Purpose = lrXMLPathedRole.Purpose

                    lrPathedRole.ValueRestriction = ConvertPathedRoleValueRestrictionFromXmlModel(lrXMLPathedRole.ValueRestriction)

                    lrSubPath.PathedRole.Add(lrPathedRole)
                Next
            End If

            If arXMLSubPath.SubPath IsNot Nothing Then
                For Each lrXMLChild As XMLModel.RoleSubPath In arXMLSubPath.SubPath

                    Dim lrChild As FBM.RoleSubPath = ConvertRoleSubPathFromXmlModel(
                                                                lrXMLChild,
                                                                arModel)

                    If lrChild IsNot Nothing Then
                        lrSubPath.SubPath.Add(lrChild)
                    End If
                Next
            End If

            Return lrSubPath

        End Function

        Private Function ConvertConditionNodeToXmlModel(ByVal arNode As FBM.ConditionNode) As XMLModel.ConditionNode

            If arNode Is Nothing Then Return Nothing

            If TypeOf arNode Is FBM.ConditionAnd Then
                Dim lrIn As FBM.ConditionAnd = CType(arNode, FBM.ConditionAnd)
                Dim lrOut As New XMLModel.ConditionAnd
                For Each lrChild As FBM.ConditionNode In lrIn.Items
                    Dim lrChildOut As XMLModel.ConditionNode = ConvertConditionNodeToXmlModel(lrChild)
                    If lrChildOut IsNot Nothing Then lrOut.Items.Add(lrChildOut)
                Next
                Return lrOut

            ElseIf TypeOf arNode Is FBM.ConditionOr Then
                Dim lrIn As FBM.ConditionOr = CType(arNode, FBM.ConditionOr)
                Dim lrOut As New XMLModel.ConditionOr
                For Each lrChild As FBM.ConditionNode In lrIn.Items
                    Dim lrChildOut As XMLModel.ConditionNode = ConvertConditionNodeToXmlModel(lrChild)
                    If lrChildOut IsNot Nothing Then lrOut.Items.Add(lrChildOut)
                Next
                Return lrOut

            ElseIf TypeOf arNode Is FBM.ConditionNot Then
                Dim lrIn As FBM.ConditionNot = CType(arNode, FBM.ConditionNot)
                Dim lrOut As New XMLModel.ConditionNot
                lrOut.Item = ConvertConditionNodeToXmlModel(lrIn.Item)
                Return lrOut

            ElseIf TypeOf arNode Is FBM.ConditionEquals Then
                Return ConvertBinaryComparisonToXmlModel(Of XMLModel.ConditionEquals)(CType(arNode, FBM.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is FBM.ConditionNotEquals Then
                Return ConvertBinaryComparisonToXmlModel(Of XMLModel.ConditionNotEquals)(CType(arNode, FBM.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is FBM.ConditionGreaterThan Then
                Return ConvertBinaryComparisonToXmlModel(Of XMLModel.ConditionGreaterThan)(CType(arNode, FBM.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is FBM.ConditionGreaterThanOrEqual Then
                Return ConvertBinaryComparisonToXmlModel(Of XMLModel.ConditionGreaterThanOrEqual)(CType(arNode, FBM.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is FBM.ConditionLessThan Then
                Return ConvertBinaryComparisonToXmlModel(Of XMLModel.ConditionLessThan)(CType(arNode, FBM.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is FBM.ConditionLessThanOrEqual Then
                Return ConvertBinaryComparisonToXmlModel(Of XMLModel.ConditionLessThanOrEqual)(CType(arNode, FBM.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is FBM.CalculatedCondition Then
                Dim lrIn As FBM.CalculatedCondition = CType(arNode, FBM.CalculatedCondition)
                Dim lrOut As New XMLModel.CalculatedCondition
                lrOut.Ref = lrIn.Ref
                Return lrOut

            End If

            Return Nothing

        End Function


        Private Function ConvertBinaryComparisonToXmlModel(Of T As {XMLModel.ConditionBinaryComparison, New})(
    ByVal arIn As FBM.ConditionBinaryComparison) As T

            Dim lrOut As New T

            If arIn.Left IsNot Nothing Then lrOut.Left = ConvertConditionValueContainerToXmlModel(arIn.Left)
            If arIn.Right IsNot Nothing Then lrOut.Right = ConvertConditionValueContainerToXmlModel(arIn.Right)

            Return lrOut

        End Function


        Private Function ConvertConditionValueContainerToXmlModel(ByVal arIn As FBM.ConditionValueContainer) As XMLModel.ConditionValueContainer

            Dim lrOut As New XMLModel.ConditionValueContainer
            If arIn Is Nothing OrElse arIn.Item Is Nothing Then Return lrOut

            If TypeOf arIn.Item Is FBM.PathedRoleRef Then
                Dim lrInRef As FBM.PathedRoleRef = CType(arIn.Item, FBM.PathedRoleRef)
                Dim lrOutRef As New XMLModel.PathedRoleRef
                lrOutRef.Ref = lrInRef.Ref
                lrOut.Item = lrOutRef

            ElseIf TypeOf arIn.Item Is FBM.CalculatedValueRef Then
                Dim lrInRef As FBM.CalculatedValueRef = CType(arIn.Item, FBM.CalculatedValueRef)
                Dim lrOutRef As New XMLModel.CalculatedValueRef
                lrOutRef.Ref = lrInRef.Ref
                lrOut.Item = lrOutRef

            ElseIf TypeOf arIn.Item Is FBM.Constant Then
                Dim lrInConst As FBM.Constant = CType(arIn.Item, FBM.Constant)
                Dim lrOutConst As New XMLModel.Constant
                lrOutConst.Id = lrInConst.Id
                lrOutConst.Value = lrInConst.Value
                lrOut.Item = lrOutConst
            End If

            Return lrOut

        End Function

        Private Function ConvertConditionNodeFromXmlModel(ByVal arNode As XMLModel.ConditionNode) As FBM.ConditionNode

            If arNode Is Nothing Then Return Nothing

            If TypeOf arNode Is XMLModel.ConditionAnd Then
                Dim lrIn As XMLModel.ConditionAnd = CType(arNode, XMLModel.ConditionAnd)
                Dim lrOut As New FBM.ConditionAnd
                For Each lrChild As XMLModel.ConditionNode In lrIn.Items
                    Dim lrChildOut As FBM.ConditionNode = ConvertConditionNodeFromXmlModel(lrChild)
                    If lrChildOut IsNot Nothing Then lrOut.Items.Add(lrChildOut)
                Next
                Return lrOut

            ElseIf TypeOf arNode Is XMLModel.ConditionOr Then
                Dim lrIn As XMLModel.ConditionOr = CType(arNode, XMLModel.ConditionOr)
                Dim lrOut As New FBM.ConditionOr
                For Each lrChild As XMLModel.ConditionNode In lrIn.Items
                    Dim lrChildOut As FBM.ConditionNode = ConvertConditionNodeFromXmlModel(lrChild)
                    If lrChildOut IsNot Nothing Then lrOut.Items.Add(lrChildOut)
                Next
                Return lrOut

            ElseIf TypeOf arNode Is XMLModel.ConditionNot Then
                Dim lrIn As XMLModel.ConditionNot = CType(arNode, XMLModel.ConditionNot)
                Dim lrOut As New FBM.ConditionNot
                lrOut.Item = ConvertConditionNodeFromXmlModel(lrIn.Item)
                Return lrOut

            ElseIf TypeOf arNode Is XMLModel.ConditionEquals Then
                Return ConvertBinaryComparisonFromXmlModel(Of FBM.ConditionEquals)(CType(arNode, XMLModel.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is XMLModel.ConditionNotEquals Then
                Return ConvertBinaryComparisonFromXmlModel(Of FBM.ConditionNotEquals)(CType(arNode, XMLModel.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is XMLModel.ConditionGreaterThan Then
                Return ConvertBinaryComparisonFromXmlModel(Of FBM.ConditionGreaterThan)(CType(arNode, XMLModel.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is XMLModel.ConditionGreaterThanOrEqual Then
                Return ConvertBinaryComparisonFromXmlModel(Of FBM.ConditionGreaterThanOrEqual)(CType(arNode, XMLModel.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is XMLModel.ConditionLessThan Then
                Return ConvertBinaryComparisonFromXmlModel(Of FBM.ConditionLessThan)(CType(arNode, XMLModel.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is XMLModel.ConditionLessThanOrEqual Then
                Return ConvertBinaryComparisonFromXmlModel(Of FBM.ConditionLessThanOrEqual)(CType(arNode, XMLModel.ConditionBinaryComparison))

            ElseIf TypeOf arNode Is XMLModel.CalculatedCondition Then

                Dim lrIn As XMLModel.CalculatedCondition = CType(arNode, XMLModel.CalculatedCondition)

                Dim lrOut As New FBM.CalculatedCondition
                lrOut.Ref = lrIn.Ref
                Return lrOut

            End If

            Return Nothing

        End Function


        Private Function ConvertBinaryComparisonFromXmlModel(Of T As {FBM.ConditionBinaryComparison, New})(
    ByVal arIn As XMLModel.ConditionBinaryComparison) As T

            Dim lrOut As New T

            If arIn.Left IsNot Nothing Then lrOut.Left = ConvertValueContainerFromXmlModel(arIn.Left)
            If arIn.Right IsNot Nothing Then lrOut.Right = ConvertValueContainerFromXmlModel(arIn.Right)

            Return lrOut

        End Function


        Private Function ConvertValueContainerFromXmlModel(ByVal arIn As XMLModel.ConditionValueContainer) As FBM.ConditionValueContainer

            Dim lrOut As New FBM.ConditionValueContainer
            If arIn Is Nothing OrElse arIn.Item Is Nothing Then Return lrOut

            If TypeOf arIn.Item Is XMLModel.PathedRoleRef Then
                Dim lrInRef As XMLModel.PathedRoleRef = CType(arIn.Item, XMLModel.PathedRoleRef)
                Dim lrOutRef As New FBM.PathedRoleRef
                lrOutRef.Ref = lrInRef.Ref
                lrOut.Item = lrOutRef

            ElseIf TypeOf arIn.Item Is XMLModel.CalculatedValueRef Then
                Dim lrInRef As XMLModel.CalculatedValueRef = CType(arIn.Item, XMLModel.CalculatedValueRef)
                Dim lrOutRef As New FBM.CalculatedValueRef
                lrOutRef.Ref = lrInRef.Ref
                lrOut.Item = lrOutRef

            ElseIf TypeOf arIn.Item Is XMLModel.Constant Then
                Dim lrInConst As XMLModel.Constant = CType(arIn.Item, XMLModel.Constant)
                Dim lrOutConst As New FBM.Constant
                lrOutConst.Id = lrInConst.Id
                lrOutConst.Value = lrInConst.Value
                lrOut.Item = lrOutConst
            End If

            Return lrOut

        End Function

#End Region

        Public Sub GetNORMASubtypeDetails(ByRef arNORMADocument As NORMA.ORMDocument,
                                      ByRef arNORMAFactType As NORMA.Model.SubtypeFact,
                                      ByVal arFBMFactType As XMLModel.FactType)

            '================================================
            'Map the Role Group for Subtype fact
            '================================================
            Dim latType = {
                GetType(NORMA.Model.ValueType),
                GetType(NORMA.Model.EntityType),
                GetType(NORMA.Model.ObjectifiedType)
            }

            ' loop all the available Roles group in FBM 
            For Each lrFBMRole In arFBMFactType.RoleGroup

                ' get the JoinedObjectTypeId from FBM model
                lrFBMRole.JoinedObjectType = Me.ORMModel.getModelElementById(lrFBMRole.JoinedObjectTypeId)

                Dim lrNORMARole As Object
                ' check if Mandatory then subtype otherwise supertype
                If lrFBMRole.Mandatory Then
                    ' Subtype
                    lrNORMARole = New NORMA.Model.SubtypeFact.RolesSubtypeMetaRole With {
                        ._Multiplicity = "ZeroToOne",
                        .Id = "_" & lrFBMRole.Id,
                        .Name = lrFBMRole.Name,
                        ._IsMandatory = lrFBMRole.Mandatory
                    }

                Else
                    ' Supertype
                    lrNORMARole = New NORMA.Model.SubtypeFact.RolesSupertypeMetaRole With {
                        ._Multiplicity = "ExactlyOne",
                        .Id = "_" & lrFBMRole.Id,
                        .Name = lrFBMRole.Name,
                        ._IsMandatory = lrFBMRole.Mandatory
                    }
                End If

                ' find the JoinedObjectTypeId
                Dim lrJoinedORMObject = (From lrObject In arNORMADocument.ORMModel.Objects.Items
                                         Where latType.Contains(lrObject.GetType) AndAlso
                                             lrObject.Name = lrFBMRole.JoinedObjectTypeId
                                         Select lrObject).FirstOrDefault()
                ' check if JoinedObject found
                If Not IsNothing(lrJoinedORMObject) Then
                    ' set the role player
                    If lrFBMRole.Mandatory Then
                        lrNORMARole.RolePlayer = New NORMA.Model.SubtypeFact.RolesSubtypeMetaRole.MetaRolePlayer()
                    Else
                        lrNORMARole.RolePlayer = New NORMA.Model.SubtypeFact.RolesSupertypeMetaRole.MetaRolePlayer()
                    End If
                    lrNORMARole.RolePlayer.Ref = lrJoinedORMObject.Id

                    Select Case lrJoinedORMObject.GetType()
                        Case GetType(NORMA.Model.EntityType)
                            If IsNothing(lrJoinedORMObject.PlayedRoles) Then
                                lrJoinedORMObject.PlayedRoles = New NORMA.Model.EntityType.EntityTypePlayedRoles()
                                lrJoinedORMObject.PlayedRoles.Items = New Object() {}
                            End If
                            ' check if subtype or supertype
                            If lrFBMRole.Mandatory Then
                                CType(lrJoinedORMObject.PlayedRoles, NORMA.Model.EntityType.EntityTypePlayedRoles).Items.
                                    Add(New NORMA.Model.EntityType.EntityTypePlayedRoles.PlayedRolesSubtypeMetaRole() With {.Ref = lrNORMARole.Id})
                            Else
                                CType(lrJoinedORMObject.PlayedRoles, NORMA.Model.EntityType.EntityTypePlayedRoles).Items.
                                    Add(New NORMA.Model.EntityType.EntityTypePlayedRoles.PlayedRolesSupertypeMetaRole() With {.Ref = lrNORMARole.Id})
                            End If
                    End Select

                End If

                If IsNothing(arNORMAFactType.FactRoles) Then
                    ' initialize the factroles in for SubtypeFact
                    arNORMAFactType.FactRoles = New NORMA.Model.SubtypeFact.SubtypeFactRoles()
                End If
                ' check if subtype or supertype
                If lrFBMRole.Mandatory Then
                    arNORMAFactType.FactRoles.SubtypeMetaRole = lrNORMARole
                Else
                    arNORMAFactType.FactRoles.SupertypeMetaRole = lrNORMARole
                End If

                '======================================================================
                'Add the MandatoryConstraint for the Role of the Subtype Fact if required
                '====================================================================
#Region "Mandatory Role Constraints"

                Dim liMandatorySimpleCounter As Integer = 1
                Dim liMandatoryImpliedCounter As Integer = 1

                If lrFBMRole.Mandatory Then

                    Dim lrMandatoryConstraint As New NORMA.Model.MandatoryConstraint()
                    lrMandatoryConstraint.RoleSequence = New List(Of NORMA.Model.MandatoryConstraint.ConstraintRole)
                    lrMandatoryConstraint.RoleSequence.Add(New NORMA.Model.MandatoryConstraint.ConstraintRole() With {.Ref = "_" & lrFBMRole.Id})

                    lrMandatoryConstraint.Name = $"SimpleMandatoryConstraint{liMandatorySimpleCounter}"
                    lrMandatoryConstraint.IsSimple = True
                    lrMandatoryConstraint.InherentForObjectType = New NORMA.Model.MandatoryConstraint.ConstraintInherentForObjectType With {
                        .Ref = lrJoinedORMObject.Id
                    }

                    If IsNothing(arNORMAFactType.InternalConstraints) Then
                        arNORMAFactType.InternalConstraints = New NORMA.Model.SubtypeFact.SubtypeFactInternalConstraints()
                    End If

                    arNORMAFactType.InternalConstraints.MandatoryConstraint = New NORMA.Model.SubtypeFact.InternalConstraintsMandatoryConstraint()
                    With arNORMAFactType.InternalConstraints.MandatoryConstraint
                        .Ref = lrMandatoryConstraint.Id
                    End With

                    If IsNothing(arNORMADocument.ORMModel.Constraints) Then
                        arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
                        arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
                    End If

                    arNORMADocument.ORMModel.Constraints.Items.Add(lrMandatoryConstraint)
                End If

#End Region 'Mandatory Role Constraints

            Next

        End Sub

        Public Sub GetNORMAImpliedFactDetails(ByRef arNORMADocument As NORMA.ORMDocument,
                                              ByRef arNORMAFactType As NORMA.Model.ImpliedFact,
                                              ByVal arFBMFactType As XMLModel.FactType)

            Try
                '================================================
                'Map the Role Group for Subtype fact
                '================================================
                Dim latType = {
                GetType(NORMA.Model.ValueType),
                GetType(NORMA.Model.EntityType),
                GetType(NORMA.Model.ObjectifiedType)
            }

                ' loop all the available Roles group in FBM 
                For Each lrFBMRole In arFBMFactType.RoleGroup

                    ' get the JoinedObjectTypeId from FBM model
                    lrFBMRole.JoinedObjectType = Me.ORMModel.getModelElementById(lrFBMRole.JoinedObjectTypeId)
                    ' find the JoinedObjectTypeId
                    Dim lrJoinedORMObject As NORMA.Model.ObjectifiedType
                    ' check the JoinedObject is Fact type
                    If TypeOf lrFBMRole.JoinedObjectType Is FactType Then

                        ' find the objectified type from Fact type
                        lrJoinedORMObject = (From lrObject In arNORMADocument.ORMModel.Objects.Items.OfType(Of NORMA.Model.ObjectifiedType)
                                             Where lrObject.Name = CType(lrFBMRole.JoinedObjectType, FactType).ObjectifyingEntityTypeId
                                             Select lrObject).FirstOrDefault()

                    Else

                        ' find the objectifiedtype
                        lrJoinedORMObject = (From lrObject In arNORMADocument.ORMModel.Objects.Items.OfType(Of NORMA.Model.ObjectifiedType)
                                             Where lrObject.Name = lrFBMRole.JoinedObjectTypeId
                                             Select lrObject).FirstOrDefault()

                    End If

                    Dim lrNORMARole As Object
                    ' check if first element in sequence
                    If lrFBMRole.SequenceNr = 1 Then

                        ' set the objectification reference
                        arNORMAFactType.ImpliedByObjectification = New NORMA.Model.ImpliedFact.FactImpliedByObjectification() With {
                            .Ref = lrJoinedORMObject.NestedPredicate.Id
                        }

                        ' add Role in ImpliedFact
                        lrNORMARole = New NORMA.Model.ImpliedFact.ImpliedFactRoles.ImpliedFactRole()
                        lrNORMARole.Id = "_" & lrFBMRole.Id
                        lrNORMARole.Name = lrFBMRole.Name
                        lrNORMARole._IsMandatory = True
                        lrNORMARole._Multiplicity = "ZeroToMany"

                        ' add Role player
                        lrNORMARole.RolePlayer = New NORMA.Model.ImpliedFact.ImpliedFactRoles.ImpliedFactRole.ImpliedFactRolePlayer()
                        lrNORMARole.RolePlayer.Ref = lrJoinedORMObject.Id

                        ' check and add the playedRoles on ObjectifiedType
                        If IsNothing(lrJoinedORMObject.PlayedRoles) Then
                            lrJoinedORMObject.PlayedRoles = New List(Of NORMA.Model.ObjectifiedType.ObjectifiedTypeRole)
                        End If
                        lrJoinedORMObject.PlayedRoles.Add(New NORMA.Model.ObjectifiedType.ObjectifiedTypeRole() With {.Ref = lrNORMARole.Id})

                        arNORMAFactType.FactRoles.Role = lrNORMARole

                    Else

                        ' find type of main role
                        Dim lrFactRoleType = arFBMFactType.RoleGroup.Find(Function(x) x.SequenceNr = 1)?.JoinedObjectType

                        ' check if the type is Fact then add ObjectifiedUnaryRole, Otherwise add RoleProxy
                        If TypeOf lrFactRoleType Is FactType Then

                            ' find the Role in Fact type
                            Dim lrJoinedNORMARole = (From lrObject In arNORMADocument.ORMModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                     From lrRole In lrObject.FactRoles
                                                     Where lrRole.Id = "_" & arFBMFactType.LinkFactTypeRoleId.ToUpper()
                                                     Select lrRole).FirstOrDefault()

                            ' add ObjectifiedUnaryRole in ImpliedFact
                            lrNORMARole = New NORMA.Model.ImpliedFact.ImpliedFactRoles.ImpliedFactObjectifiedUnaryRole()
                            lrNORMARole.Id = "_" & lrFBMRole.Id
                            lrNORMARole.Name = lrFBMRole.Name
                            lrNORMARole._IsMandatory = lrFBMRole.Mandatory
                            lrNORMARole._Multiplicity = "ExactlyOne"

                            ' add references
                            lrNORMARole.UnaryRoleRef.Ref = lrJoinedNORMARole.Id
                            lrNORMARole.RolePlayer.Ref = lrJoinedNORMARole.RolePlayer.Ref

                            arNORMAFactType.FactRoles.ObjectifiedUnaryRole = lrNORMARole

                        Else

                            ' find the role proxy id
                            Dim lrJoinedNORMARole = (From lrObject In arNORMADocument.ORMModel.Facts.Items.OfType(Of NORMA.Model.Fact)
                                                     From lrRole In lrObject.FactRoles
                                                     Where lrRole.Id = "_" & arFBMFactType.LinkFactTypeRoleId.ToUpper()
                                                     Select lrRole).FirstOrDefault()

                            ' add RoleProxy in ImpliedFact
                            lrNORMARole = New NORMA.Model.ImpliedFact.ImpliedFactRoles.ImpliedFactRoleProxy()
                            lrNORMARole.Id = "_" & lrFBMRole.Id

                            ' add RoleProxy reference
                            lrNORMARole.Role = New NORMA.Model.ImpliedFact.ImpliedFactRoles.ImpliedFactRoleProxy.ProxyRole()
                            lrNORMARole.Role.Ref = lrJoinedNORMARole.Id

                            arNORMAFactType.FactRoles.RoleProxy = lrNORMARole

                        End If

                    End If

                    '======================================================================
                    'Add the MandatoryConstraint for the Role of the ImpliedFact if required
                    '====================================================================
#Region "Mandatory Role Constraints"
                    Dim liMandatorySimpleCounter As Integer = 1
                    Dim liMandatoryImpliedCounter As Integer = 1
                    Dim lrMandatoryConstraint As New NORMA.Model.MandatoryConstraint()

                    lrMandatoryConstraint.RoleSequence = New List(Of NORMA.Model.MandatoryConstraint.ConstraintRole)
                    lrMandatoryConstraint.RoleSequence.Add(New NORMA.Model.MandatoryConstraint.ConstraintRole() With {.Ref = "_" & lrFBMRole.Id})

                    If lrFBMRole.Mandatory Then
                        lrMandatoryConstraint.Name = $"SimpleMandatoryConstraint{liMandatorySimpleCounter}"
                        lrMandatoryConstraint.IsSimple = True

                        If IsNothing(arNORMAFactType.InternalConstraints) Then
                            arNORMAFactType.InternalConstraints = New NORMA.Model.ImpliedFact.FactInternalConstraints()
                        End If
                        arNORMAFactType.InternalConstraints.MandatoryConstraint =
                            New NORMA.Model.ImpliedFact.FactInternalConstraints.ConstraintsMandatoryConstraint() With {
                                .Ref = lrMandatoryConstraint.Id
                        }

                        If IsNothing(arNORMADocument.ORMModel.Constraints) Then
                            arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
                            arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
                        End If

                        arNORMADocument.ORMModel.Constraints.Items.Add(lrMandatoryConstraint)
                    End If

#End Region 'Mandatory Role Constraints

                Next

                '================================================
                'Map the Readings For Fact
                '================================================
#Region "Fact Type Readings"
                For Each lrFBMReadings In arFBMFactType.FactTypeReadings

                    Dim lrReadingOrder As New NORMA.Model.ImpliedFact.ReadingOrder()
                    lrReadingOrder.Id = "_" & lrFBMReadings.Id
                    lrReadingOrder.Readings = New NORMA.Model.ImpliedFact.ReadingOrder.OrderReadings()
                    lrReadingOrder.Readings.Reading = New NORMA.Model.ImpliedFact.ReadingOrder.OrderReading()

                    ' START: get and add role sequence
                    lrReadingOrder.RoleSequence = New NORMA.Model.ImpliedFact.ReadingOrder.OrderRole() {}
                    For Each lrFBMSequence In From lrFBMRoleSequence In lrFBMReadings.PredicateParts
                                              From lrFBMRole In arFBMFactType.RoleGroup
                                              Where lrFBMRole.Id = lrFBMRoleSequence.Role_Id
                                              Order By lrFBMRoleSequence.SequenceNr
                                              Select lrFBMRole
                        lrReadingOrder.RoleSequence.Add(New NORMA.Model.ImpliedFact.ReadingOrder.OrderRole() With {.Ref = "_" & lrFBMSequence.Id})
                    Next
                    ' END: get and add role sequence

                    Dim lrReading As New NORMA.Model.ImpliedFact.ReadingOrder.OrderReading()
                    lrReading.ExpandedData = New NORMA.Model.ImpliedFact.ReadingOrder.ExpandedData()
                    lrReading.ExpandedData.RoleText = New NORMA.Model.ImpliedFact.ReadingOrder.RoleText() {}

                    ' trim the front reading text and add a space manually
                    Dim lsPredicate As String = String.Empty

                    ' START: generate reading for PredicateParts
                    Dim liInd As Integer = 1
                    For Each lrFBMPredicatePart In lrFBMReadings.PredicateParts

                        Dim lsFollowingTextExtension As String = ""
                        If liInd < lrFBMReadings.PredicateParts.Count Then
                            lsFollowingTextExtension = lrFBMReadings.PredicateParts(liInd).PreboundReadingText
                        Else
                            If Not String.IsNullOrWhiteSpace(lrFBMPredicatePart.PostboundReadingText) Then
                                lsFollowingTextExtension = lrFBMPredicatePart.PostboundReadingText
                            Else
                                lsFollowingTextExtension = lrFBMReadings.FollowingReadingText
                            End If
                        End If

                        Dim lrRoleText As New NORMA.Model.ImpliedFact.ReadingOrder.RoleText()
                        lrRoleText.RoleIndex = lrFBMPredicatePart.SequenceNr - 1
                        ' check if the last element of predicate
                        If liInd = lrFBMReadings.PredicateParts.Count Then
                            ' trim white space from predicate text
                            lrRoleText.FollowingText = Trim(lrFBMPredicatePart.PredicatePartText.TrimStart() & " " & lsFollowingTextExtension)
                        Else
                            ' trim white space from predicate text and add a space manually
                            lrRoleText.FollowingText = " " & Trim(lrFBMPredicatePart.PredicatePartText.TrimStart() & " " & lsFollowingTextExtension)
                        End If

                        lrReading.ExpandedData.RoleText.Add(lrRoleText)

                        '20220730-VM-Fix this. 
                        '20221102-HA-Improved this
                        If liInd = lrFBMReadings.PredicateParts.Count AndAlso
                            String.IsNullOrEmpty(lrRoleText.FollowingText) Then
                            lsPredicate &= " {" & liInd - 1 & "}"
                        Else
                            lsPredicate &= "{" & liInd - 1 & "}" & lrRoleText.FollowingText
                        End If

                        liInd += 1
                    Next
                    lrReading.Data = lsPredicate
                    lrReadingOrder.Readings.Reading = lrReading
                    ' END: generate reading for PredicateParts

                    If IsNothing(arNORMAFactType.ReadingOrders) Then
                        arNORMAFactType.ReadingOrders = New NORMA.Model.ImpliedFact.ReadingOrder() {}
                    End If
                    arNORMAFactType.ReadingOrders.Add(lrReadingOrder)

                Next
#End Region 'Fact Type Readings

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arORMDocument">The NORMA ORM Document for which the Pages are to be inserted.</param>
        ''' <param name="aoBackgroundWorker"></param>
        Public Sub MapToORMDiagrams(ByRef arORMDocument As NORMA.ORMDocument,
                                    Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Dim lrPage As NORMA.ORMDiagram.ORMDiagram = Nothing
            'Dim lrFBMPage As XMLModel.Page
            Dim lrORMDocument As NORMA.ORMDocument = arORMDocument
            Dim loBackgroundWorker As System.ComponentModel.BackgroundWorker = aoBackgroundWorker
            Try

                For Each lrFBMPage In Me.ORMDiagram
                    lrPage = Nothing
                    lrPage = Me.MapToNORMAPage(lrFBMPage, lrORMDocument, lrPage, loBackgroundWorker, False)

                    lrORMDocument.ORMDiagram.Add(lrPage)
                Next 'XMLModel.Page

#Region "2220721-VM-Use if you decide to do threading"
                '--------------------
                'Parallel Threading
                '--------------------
                'Parallel.ForEach(Me.ORMDiagram,
                '                 Sub(lrFBMpage As XMLModel.Page)
                '                     'lrPage = lrORMDocument.ORMDiagram.Find(Function(x) x.Id = lrFBMpage.Id)
                '                     'If lrPage Is Nothing Then
                '                     'lrPage = Me.MapToFBMPage(lrFBMpage, lrORMDocument,,, True)
                '                     'Else
                '                     'If Not lrPage.loaded Then
                '                     lrPage = Me.MapToFBMPage(lrFBMpage, lrORMDocument, lrPage, loBackgroundWorker, True)
                '                     'End If
                '                     'End If

                '                     'lrPage.loaded = True
                '                     lrORMDocument.ORMDiagram.Add(lrPage)

                '                 End Sub)
#End Region

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        <MethodImplAttribute(MethodImplOptions.Synchronized)>
        Private Function MapToNORMAPage(ByRef arXMLPage As XMLModel.Page,
                                                  ByRef arORMDocument As NORMA.ORMDocument,
                                                  Optional ByRef arPage As NORMA.ORMDiagram.ORMDiagram = Nothing,
                                                  Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing,
                                                  Optional ByVal abCalledAsThread As Boolean = False) As NORMA.ORMDiagram.ORMDiagram

            Dim lsMessage As String
            Dim ldblScalar As Double = 30.5
            Dim ldblWidthScale As Double = 12.1
            Dim ldblFixedHeight As Double = 0.2295
            Dim ldblEntityTypeReferenceModeHeight As Double = 0.359

            Try
                Dim lrConceptInstance As FBM.ConceptInstance = Nothing
                Dim lrPage As NORMA.ORMDiagram.ORMDiagram = Nothing

                If arPage Is Nothing Then
                    lrPage = New NORMA.ORMDiagram.ORMDiagram '("_" & arXMLPage.Id, arXMLPage.Name)
                End If
                lrPage.Id = "_" & arXMLPage.Id
                lrPage.Name = arXMLPage.Name
                lrPage.BaseFontName = "Tahoma"
                lrPage.BaseFontSize = "0.0972"
                lrPage.Subject = New NORMA.ORMDiagram.Subject
                lrPage.Subject.Ref = arORMDocument.ORMModel.Id
                arPage = lrPage

                '=============================
                'Map the ValueTypeInstances
                '=============================
#Region "ValueTypeInstance"
                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.ValueType)
                    Try
                        'Don't include the ValueType if it is a ReferenceModeValueType of an EntityType
                        If Me.ValueTypeIsReferenceModeValueType(lrConceptInstance.Symbol) Then GoTo SkipValueTypeInstance

                        Dim lrValueTypeInstance As New NORMA.ORMDiagram.ObjectTypeShape()
                        lrValueTypeInstance.Subject = New NORMA.ORMDiagram.Subject
                        lrValueTypeInstance.Subject.Ref = arORMDocument.ORMModel.Objects.Items.First(Function(x) x.Name = lrConceptInstance.Symbol).Id
                        lrValueTypeInstance.IsExpanded = True

                        Dim liWidth = Math.Max(8, TextRenderer.MeasureText(lrConceptInstance.Symbol, New Font(lrPage.BaseFontName, lrPage.BaseFontSize)).Width - 5)
                        lrValueTypeInstance.AbsoluteBounds = $"{lrConceptInstance.X / ldblScalar}, {lrConceptInstance.Y / ldblScalar}, {liWidth / ldblWidthScale}, {ldblFixedHeight}"

                        lrPage.Shapes.Items.Add(lrValueTypeInstance)
                    Catch ex As Exception
                        'Call Me.ReportModelLoadingError(arModel, ex.Message)
                        GoTo SkipValueTypeInstance
                    End Try
SkipValueTypeInstance:
                Next
#End Region

                '=============================
                'Map the EntityTypeInstances
                '=============================
#Region "EntityTypeInstances"
                'Dim lrEntityTypeInstance As NORMA.ORMDiagram.ObjectTypeShape = Nothing
                'Dim lrEntityType As NORMA.Model.EntityType = Nothing
                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.EntityType)
                    Try
                        Dim lrFBMEntityType As XMLModel.EntityType = Me.ORMModel.EntityTypes.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                        If Not lrFBMEntityType.IsObjectifyingEntityType Then
                            Dim lrEntityTypeInstance As New NORMA.ORMDiagram.ObjectTypeShape()
                            lrEntityTypeInstance.Subject = New NORMA.ORMDiagram.Subject
                            lrEntityTypeInstance.Subject.Ref = arORMDocument.ORMModel.Objects.Items.First(Function(x) x.Name = lrConceptInstance.Symbol).Id
                            lrEntityTypeInstance.IsExpanded = True

                            'Height of the EntityTypeInstance
                            Dim ldblEntityTypeHeight As Double = Boston.returnIfTrue(lrFBMEntityType.ReferenceMode Is Nothing, ldblFixedHeight, ldblEntityTypeReferenceModeHeight)

                            lrEntityTypeInstance.AbsoluteBounds = $"{lrConceptInstance.X / ldblScalar}, {lrConceptInstance.Y / ldblScalar}, {lrConceptInstance.Symbol.Length / ldblWidthScale}, {ldblEntityTypeHeight}"

                            lrPage.Shapes.Items.Add(lrEntityTypeInstance)
                        End If

                    Catch ex As Exception
                        'Call Me.ReportModelLoadingError(arModel, ex.Message)
                        Continue For
                    End Try
                    'lrEntityTypeInstance = New FBM.EntityTypeInstance
                    'lrEntityTypeInstance.Model = arModel
                    'lrEntityTypeInstance.Page = lrPage
                    'lrEntityTypeInstance.id = lrConceptInstance.Symbol
                    'lrEntityType = arModel.EntityType.Find(Function(x) x.Id = lrEntityTypeInstance.id)
                    'lrEntityTypeInstance.EntityType = lrEntityType
                    'lrEntityTypeInstance._Name = lrEntityTypeInstance.id
                    'lrEntityTypeInstance.ReferenceMode = lrEntityType.ReferenceMode
                    'lrEntityTypeInstance.IsObjectifyingEntityType = lrEntityType.IsObjectifyingEntityType
                    'lrEntityTypeInstance.IsAbsorbed = lrEntityType.IsAbsorbed
                    'lrEntityTypeInstance.IsDerived = lrEntityType.IsDerived
                    'lrEntityTypeInstance.DerivationText = lrEntityType.DerivationText
                    'lrEntityTypeInstance.DBName = lrEntityType.DBName

                    'If lrEntityTypeInstance.EntityType.ReferenceModeValueType IsNot Nothing Then
                    '    lrEntityTypeInstance.ReferenceModeValueType = lrPage.ValueTypeInstance.Find(Function(x) x.Id = lrEntityTypeInstance.EntityType.ReferenceModeValueType.Id)
                    'End If

                    'lrEntityTypeInstance.PreferredIdentifierRCId = lrEntityTypeInstance.EntityType.PreferredIdentifierRCId

                    'lrEntityTypeInstance.X = lrConceptInstance.X
                    'lrEntityTypeInstance.Y = lrConceptInstance.Y

                    'lrPage.EntityTypeInstance.Add(lrEntityTypeInstance)
                Next
#End Region

                '===========================
                'Map the FactTypeInstances
                '===========================
#Region "FactTypeInstances"
                Dim lrFactTypeInstance As NORMA.ORMDiagram.FactTypeShape = Nothing
                Dim lrFactTypeReadingConceptInstance As FBM.ConceptInstance = Nothing
                Dim lrFactTypeNameConceptInstance As FBM.ConceptInstance = Nothing
                '                Dim lrDerivationTextConceptInstance As FBM.ConceptInstance
                '                Dim lrFact As FBM.Fact
                '                Dim lrFactType As FBM.FactType

                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.FactType)

                    'Don't include the FactTypeInstance if it is a ReferenceModeFactType for an EntityType.
                    If Me.FactTypeIsReferenceModeFactType(lrConceptInstance.Symbol) Then GoTo SkipFactTypeInstance

                    Try
                        Dim lrFBMFactType = Me.ORMModel.FactTypes.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                        If Not lrFBMFactType.IsSubtypeRelationshipFactType Then
                            lrFactTypeInstance = New NORMA.ORMDiagram.FactTypeShape
                            lrFactTypeInstance.Subject = New NORMA.ORMDiagram.Subject
                            Dim lrFactType As NORMA.Model.Fact = arORMDocument.ORMModel.Facts.Items.First(Function(x) x._Name = lrConceptInstance.Symbol)
                            lrFactTypeInstance.Subject.Ref = lrFactType.Id
                            lrFactTypeInstance.IsExpanded = True
                            lrFactTypeInstance.AbsoluteBounds = $"{CDbl(lrConceptInstance.X / ldblScalar - (lrConceptInstance.Symbol.Length / ldblWidthScale) / 4)}, {lrConceptInstance.Y / ldblScalar}, {lrConceptInstance.Symbol.Length / ldblWidthScale}, {ldblFixedHeight}"

#Region "Relative Shapes"
                            lrFactTypeInstance.RelativeShapes = New NORMA.ORMDiagram.FactTypeShape.RelativeShape

#Region "Fact Type Reading"
                            'FactTypeReading
                            lrFactTypeReadingConceptInstance = arXMLPage.ConceptInstance.Find(Function(x) x.ConceptType = pcenumConceptType.FactTypeReading And x.Symbol = lrFactType.Id)
                            If lrFactTypeReadingConceptInstance IsNot Nothing Then
                                Dim lrFactTypeReadingInstance As New NORMA.ORMDiagram.FactTypeShape.RelativeShape.FactTypeReadingShape
                                lrFactTypeReadingInstance.AbsoluteBounds = $"{lrFactTypeReadingConceptInstance.X / ldblScalar}, {lrFactTypeReadingConceptInstance.Y / ldblScalar}, {lrConceptInstance.Symbol.Length / ldblWidthScale}, {ldblFixedHeight}"
                                lrFactTypeReadingInstance.Subject.Ref = lrFactType.ReadingOrders.Find(Function(x) x.Id = lrConceptInstance.Symbol).Id
                                lrFactTypeInstance.RelativeShapes.ReadingShape = lrFactTypeReadingInstance
                            End If
#End Region

#Region "Fact Type Name"
                            'FactTypeName
                            lrFactTypeNameConceptInstance = arXMLPage.ConceptInstance.Find(Function(x) x.ConceptType = pcenumConceptType.FactTypeName And x.Symbol = lrFactType.Id)
                            If lrFactTypeNameConceptInstance IsNot Nothing Then
                                Dim lrFactTypeNameInstance As New NORMA.ORMDiagram.FactTypeShape.RelativeShape.FactTypeNameShape
                                If lrFBMFactType.IsObjectified Then
                                    lrFactTypeInstance.RelativeShapes.ObjectifiedFactTypeNameShape = lrFactTypeNameInstance
                                    lrFactTypeNameInstance.AbsoluteBounds = $"{lrFactTypeNameConceptInstance.X / ldblScalar}, {lrFactTypeNameConceptInstance.Y / ldblScalar}, {lrConceptInstance.Symbol.Length / ldblWidthScale}, {ldblFixedHeight}"
                                Else
                                    '20220728-VM-Unknown at this stage.
                                End If
                            End If
#End Region

#Region "Derivation Rules"
                            '20220728-VM-Ignore at this stage. Leave out of transformation to NORMA .orm model at this stage.
                            '                    If lrFactType.IsDerived Then
                            '                        lrDerivationTextConceptInstance = arXMLPage.ConceptInstance.Find(Function(x) x.ConceptType = pcenumConceptType.DerivationText And x.Symbol = lrFactType.Id)
                            '                        If lrDerivationTextConceptInstance IsNot Nothing Then
                            '                            lrFactTypeInstance.FactTypeDerivationText = New FBM.FactTypeDerivationText(lrPage.Model,
                            '                                                                                                           lrPage,
                            '                                                                                                           lrFactTypeInstance)
                            '                            lrFactTypeInstance.FactTypeDerivationText.X = lrDerivationTextConceptInstance.X
                            '                            lrFactTypeInstance.FactTypeDerivationText.Y = lrDerivationTextConceptInstance.Y
                            '                        End If
                            '                    End If
#End Region

#End Region
                            lrPage.Shapes.Items.Add(lrFactTypeInstance)
                        End If
                    Catch ex As Exception
                        'Call Me.ReportModelLoadingError(arModel, ex.Message)
                        Continue For
                    End Try
SkipFactTypeInstance:
                Next


                '----------------------------------------
                'Get the Facts for the FactTypeInstance
                '----------------------------------------
#Region "Facts for FactTypeInstance"
                '20220728-VM-Leave out of transformation to NORMA .orm model at this stage.
                '                    Dim lrFactInstance As FBM.FactInstance
                '                    Dim lrFactDataInstance As FBM.FactDataInstance
                '                    For Each lrFact In lrFactType.Fact

                '                        Dim lrFactConceptInstance = arXMLPage.ConceptInstance.Find(Function(x) x.Symbol = lrFact.Id And x.ConceptType = pcenumConceptType.Fact)
                '                        If lrFactConceptInstance IsNot Nothing Then
                '                            '----------------------------------
                '                            'The Fact is included on the Page
                '                            '----------------------------------
                '                            lrFactInstance = lrFact.CloneInstance(lrPage)
                '                            lrFactInstance.X = lrFactConceptInstance.X
                '                            lrFactInstance.Y = lrFactConceptInstance.Y
                '                            lrFactInstance.isDirty = True
                '                            For Each lrFactDataInstance In lrFactInstance.Data
                '                                lrFactDataInstance.isDirty = True
                '                            Next
                '                            lrFactTypeInstance.Fact.Add(lrFactInstance)
                '                            lrFactTypeInstance.Page.FactInstance.Add(lrFactInstance)
                '                            lrFactTypeInstance.isDirty = True

                '                            For Each lrFactDataInstance In lrFactInstance.Data

                '                                Dim lrFactDataConceptInstance As New FBM.ConceptInstance
                '                                lrFactDataConceptInstance.Symbol = lrFactDataInstance.Data
                '                                lrFactDataConceptInstance.RoleId = lrFactDataInstance.Role.Id

                '                                lrFactDataConceptInstance = arXMLPage.ConceptInstance.Find(AddressOf lrFactDataConceptInstance.EqualsBySymbolRoleId)

                '                                If lrFactDataConceptInstance IsNot Nothing Then
                '                                    lrFactDataInstance.X = lrFactDataConceptInstance.X
                '                                    lrFactDataInstance.Y = lrFactDataConceptInstance.Y
                '                                End If
                '                                lrPage.ValueInstance.AddUnique(lrFactDataInstance)
                '                            Next

                '                        End If

                '                    Next
#End Region

#Region "Populate RoleInstances that are (still) joined to Nothing"
                '===============================================================================================
                'Populate RoleInstances that are (still) joined to Nothing
                '===========================================================
                '20220728-VM-Probably not needed. Leave out of transformation to NORMA .orm model at this stage.
                '                Dim latType = {GetType(FBM.ValueTypeInstance),
                '                                   GetType(FBM.EntityTypeInstance),
                '                                   GetType(FBM.FactTypeInstance)}

                '                Dim larRole = From Role In lrPage.RoleInstance
                '                              Where Role.JoinedORMObject Is Nothing
                '                              Select Role

                '                Dim lrRoleInstance As FBM.RoleInstance

                '                For Each lrRoleInstance In larRole
                '                    Select Case lrRoleInstance.TypeOfJoin
                '                        Case Is = pcenumRoleJoinType.FactType
                '                            lrRoleInstance.JoinedORMObject = lrPage.FactTypeInstance.Find(Function(x) x.Id = lrRoleInstance.JoinsFactType.Id)
                '                    End Select

                '                Next
#End Region

#End Region

                '=================================================================================
                'Map the SubtypeRelationships.
                '=================================================================================
#Region "SubtypeRelationships"
                '                Dim larSubtypeRelationshipFactTypes = From FactType In lrPage.FactTypeInstance
                '                                                      Where FactType.IsSubtypeRelationshipFactType
                '                                                      Select FactType

                '                Dim lrModelElementInstance As FBM.ModelObject
                '                Dim lrParentModelElementInstance As FBM.ModelObject
                '                Dim lrSubtypeRelationship As FBM.SubtypeRelationship
                '                Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance
                '                For Each lrFactTypeInstance In larSubtypeRelationshipFactTypes.ToArray
                '                    Try
                '                        lrModelElementInstance = lrPage.getModelElementById(lrFactTypeInstance.RoleGroup(0).JoinedORMObject.Id)
                '                        lrParentModelElementInstance = lrPage.getModelElementById(lrFactTypeInstance.RoleGroup(1).JoinedORMObject.Id)

                '                        If lrParentModelElementInstance.ConceptType = pcenumConceptType.FactType Then
                '                            Dim lsParentModelElementInstanceId = CType(lrParentModelElementInstance, FBM.FactTypeInstance).FactType.ObjectifyingEntityType.Id
                '                            lrParentModelElementInstance = lrPage.EntityTypeInstance.Find(Function(x) x.Id = lsParentModelElementInstanceId)
                '                        End If

                '                        lrSubtypeRelationship = Nothing
                '                        Select Case lrModelElementInstance.ConceptType
                '                            Case Is = pcenumConceptType.EntityType
                '                                lrSubtypeRelationship = CType(lrModelElementInstance, FBM.EntityTypeInstance).EntityType.SubtypeRelationship.Find(Function(x) x.ModelElement.Id = lrModelElementInstance.Id And x.parentModelElement.Id = lrParentModelElementInstance.Id)
                '                            Case Is = pcenumConceptType.ValueType
                '                                lrSubtypeRelationship = CType(lrModelElementInstance, FBM.ValueTypeInstance).ValueType.SubtypeRelationship.Find(Function(x) x.ModelElement.Id = lrModelElementInstance.Id And x.parentModelElement.Id = lrParentModelElementInstance.Id)
                '                        End Select

                '                        lrSubtypeRelationshipInstance = lrSubtypeRelationship.CloneInstance(lrPage, True)

                '                        Select Case lrModelElementInstance.ConceptType
                '                            Case Is = pcenumConceptType.EntityType
                '                                CType(lrModelElementInstance, FBM.EntityTypeInstance).SubtypeRelationship.Add(lrSubtypeRelationshipInstance)
                '                            Case Is = pcenumConceptType.ValueType
                '                                CType(lrModelElementInstance, FBM.ValueTypeInstance).SubtypeRelationship.Add(lrSubtypeRelationshipInstance)
                '                        End Select



                '                    Catch ex As Exception
                '                        lsMessage = "Problem loading Subtype Relationship for Subtype Relationship Fact Type with Id: " & lrFactTypeInstance.Id
                '                        lsMessage.AppendDoubleLineBreak("Page: " & lrPage.Name)
                '                        If abCalledAsThread Then
                '                            Dim lrModelError As New FBM.ModelError(pcenumModelErrors.ModelLoadingError,
                '                                                           lsMessage,
                '                                                           Nothing,
                '                                                           Nothing)
                '                            arModel.ModelError.Add(lrModelError)
                '                        Else
                '                            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                '                        End If

                '                    End Try
                '                Next
#End Region

                '=============================
                'Map the RoleConstraints
                '=============================
#Region "RoleConstraint"
                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleConstraint)
                    Try
                        Dim lrFBMRoleConstraint = Me.ORMModel.RoleConstraints.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                        Select Case lrFBMRoleConstraint.RoleConstraintType
                            Case Is = "RingConstraint"
                                Dim lrRoleConstraintInstance As New NORMA.ORMDiagram.RingConstraintShape
                                lrRoleConstraintInstance.Subject = New NORMA.ORMDiagram.Subject
                                lrRoleConstraintInstance.Subject.Ref = arORMDocument.ORMModel.Constraints.Items.First(Function(x) x.Name = lrConceptInstance.Symbol).Id
                                lrRoleConstraintInstance.IsExpanded = True

                                Dim liWidth = Math.Max(8, TextRenderer.MeasureText(lrConceptInstance.Symbol, New Font(lrPage.BaseFontName, lrPage.BaseFontSize)).Width - 5)
                                lrRoleConstraintInstance.AbsoluteBounds = $"{lrConceptInstance.X / ldblScalar}, {lrConceptInstance.Y / ldblScalar}, {"0.16"}, {"0.16"}"

                                lrPage.Shapes.Items.Add(lrRoleConstraintInstance)
                            Case Is = "ExternalUniquenessConstraint", "SubsetConstraint", "EqualityConstraint"
                                Dim lrRoleConstraintInstance As New NORMA.ORMDiagram.ExternalConstraintShape
                                lrRoleConstraintInstance.Subject = New NORMA.ORMDiagram.Subject
                                lrRoleConstraintInstance.Subject.Ref = arORMDocument.ORMModel.Constraints.Items.First(Function(x) x.Name = lrConceptInstance.Symbol).Id
                                lrRoleConstraintInstance.IsExpanded = True

                                Dim liWidth = Math.Max(8, TextRenderer.MeasureText(lrConceptInstance.Symbol, New Font(lrPage.BaseFontName, lrPage.BaseFontSize)).Width - 5)
                                lrRoleConstraintInstance.AbsoluteBounds = $"{lrConceptInstance.X / ldblScalar}, {lrConceptInstance.Y / ldblScalar}, {"0.16"}, {"0.16"}"

                                lrPage.Shapes.Items.Add(lrRoleConstraintInstance)
                            Case Else
                                Continue For
                        End Select

                    Catch ex As Exception
                        'Call Me.ReportModelLoadingError(arModel, ex.Message)
                        GoTo SkipRoleConstraintInstance
                    End Try
SkipRoleConstraintInstance:
                Next
#End Region

                '===========================
                'Map the RoleNameInstances
                '===========================
#Region "RoleNameInstances"
                '                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleName)
                '                    lrRoleInstance = lrPage.RoleInstance.Find(Function(x) x.Id = lrConceptInstance.RoleId)
                '                    Try
                '                        lrRoleInstance.RoleName = New FBM.RoleName(lrRoleInstance, lrRoleInstance.Name)
                '                        lrRoleInstance.RoleName.X = lrConceptInstance.X
                '                        lrRoleInstance.RoleName.Y = lrConceptInstance.Y
                '                    Catch ex As Exception
                '                        'Not worth crashing over.
                '                    End Try
                '                Next

                '                '=================================
                '                'Map the RoleConstraintInstances
                '                '=================================
                '                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                '                Dim lrRoleConstraint As FBM.RoleConstraint
                '                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleConstraint)

                '                    If lrPage.RoleConstraintInstance.Find(Function(x) x.Id = lrConceptInstance.Symbol) IsNot Nothing Then
                '                        '-------------------------------------------------------------------
                '                        'The RoleConstraintInstance has already been added to the Page.
                '                        '  FactType.CloneInstance adds RoleConstraintInstances to the Page
                '                        '-------------------------------------------------------------------
                '                    Else
                '                        lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                '                        Try
                '                            Select Case lrRoleConstraint.RoleConstraintType
                '                                Case Is = pcenumRoleConstraintType.FrequencyConstraint
                '                                    lrRoleConstraintInstance = lrRoleConstraint.CloneFrequencyConstraintInstance(lrPage)
                '                                Case Is = pcenumRoleConstraintType.RoleValueConstraint
                '                                    lrRoleConstraintInstance = lrRoleConstraint.CloneRoleValueConstraintInstance(lrPage)
                '                                Case Else
                '                                    lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)
                '                            End Select

                '                            lrRoleConstraintInstance.X = lrConceptInstance.X
                '                            lrRoleConstraintInstance.Y = lrConceptInstance.Y

                '                            lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                '                        Catch ex As Exception
                '                            lsMessage = "Error loading Role Constraint with Id: " & lrConceptInstance.Symbol
                '                            lsMessage.AppendDoubleLineBreak("Page: " & arXMLPage.Name)
                '                            If abCalledAsThread Then
                '                                Dim lrModelError As New FBM.ModelError(pcenumModelErrors.ModelLoadingError,
                '                                                           lsMessage,
                '                                                           Nothing,
                '                                                           Nothing)
                '                                arModel.ModelError.Add(lrModelError)
                '                            Else
                '                                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                '                            End If
                '                        End Try

                '                    End If
                '                Next

                '                '============================
                '                'Map the ModelNoteInstances
                '                '============================
                '                Dim lrModelNoteInstance As FBM.ModelNoteInstance
                '                Dim lrModelNote As FBM.ModelNote
                '                For Each lrConceptInstance In arXMLPage.ConceptInstance.FindAll(Function(x) x.ConceptType = pcenumConceptType.ModelNote)
                '                    Try
                '                        lrModelNote = arModel.ModelNote.Find(Function(x) x.Id = lrConceptInstance.Symbol)

                '                        lrModelNoteInstance = lrModelNote.CloneInstance(lrPage, True)
                '                        lrModelNoteInstance.X = lrConceptInstance.X
                '                        lrModelNoteInstance.Y = lrConceptInstance.Y
                '                    Catch ex As Exception
                '                        lsMessage = "Error loading Model Note with Id: " & lrConceptInstance.Symbol
                '                        lsMessage.AppendDoubleLineBreak("Page: " & arXMLPage.Name)
                '                        If abCalledAsThread Then
                '                            Dim lrModelError As New FBM.ModelError(pcenumModelErrors.ModelLoadingError,
                '                                                           lsMessage,
                '                                                           Nothing,
                '                                                           Nothing)
                '                            arModel.ModelError.Add(lrModelError)
                '                        Else
                '                            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                '                        End If
                '                    End Try

                'Next
#End Region

                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(70 + CInt(29 * (arORMDocument.ORMDiagram.Count / Me.ORMDiagram.Count)))

                Return lrPage

            Catch ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                If abCalledAsThread Then
                    '20220721-VM-Add code here for throwing error messages in a thread.
                Else
                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                End If

                Return Nothing

            End Try

        End Function

        Private Sub ReportModelLoadingError(ByRef arModel As FBM.Model, ByVal asErrorMessage As String)

            Try
                Dim lrModelError As New FBM.ModelError(pcenumModelErrors.ModelLoadingError,
                                       asErrorMessage,
                                       Nothing,
                                       Nothing)
                arModel._ModelError.Add(lrModelError)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub GetFactsForFactType(ByRef arFactType As FBM.FactType,
                                       Optional ByRef arXMLFactType As XMLModel.FactType = Nothing) 'As List(Of FBM.Fact)

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

                If arXMLFactType Is Nothing Then
                    lrXMLFactType = Me.ORMModel.FactTypes.Find(Function(x) x.Id = lsFactTypeId)
                Else
                    lrXMLFactType = arXMLFactType
                End If

                Dim lrConcept As FBM.Concept = Nothing
                Dim lrFactData As New FBM.FactData

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
                    lrDictionaryEntry.Realisations.Add(pcenumConceptType.Fact)
                    arFactType.Model.ModelDictionary.Add(lrDictionaryEntry)


                    For Each lrXMLFactData In lrXMLFact.Data
                        lrRole = arFactType.RoleGroup.Find(Function(x) x.Id = lrXMLFactData.RoleId)

                        '--------------------------------------------------------------------------------------------------
                        'Get the Concept from the ModelDictionary so that FactData objects are linked directly to the Concept/Value in the ModelDictionary
                        '--------------------------------------------------------------------------------------------------
                        lrDictionaryEntry = New FBM.DictionaryEntry(arFactType.Model,
                                                                    lrXMLFactData.Data,
                                                                    pcenumConceptType.Value,
                                                                    ,
                                                                    , True
                                                                    )

                        lrConcept = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry,, False).Concept
                        lrFactData = New FBM.FactData(lrRole, lrConcept, lrFact, True)

                        '-----------------------------
                        'Add the FactData to the Fact
                        '-----------------------------
                        lrFact.Data.Add(lrFactData)
                        '-------------------------------------
                        'Add the FactData to the Role as well
                        '-------------------------------------
                        lrRole.Data.Add(lrFactData)

                        'lrRole.JoinedORMObject.Instance.AddUnique(lrConcept.Symbol)
                    Next

                    '----------------------------------------------------------------------------------------------------------
                    'If the FactType of the Fact is Objectified, add the Fact.Id as an instance of the ObjectifyingEntityType
                    '----------------------------------------------------------------------------------------------------------
                    If arFactType.IsObjectified Then
                        If arFactType.ObjectifyingEntityType IsNot Nothing Then
                            'arFactType.ObjectifyingEntityType.Instance.Add(lrFact.Id)
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
                prApplication.ThrowMessage(lsMessage2, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

#Region "NORMA Derivation Resolvers"

        Private Sub ResolveRootObjectType(ByVal arRoot As FBM.RootObjectType,
                                            ByVal arModel As FBM.Model)

            If arRoot Is Nothing OrElse arRoot.BostonModelElement IsNot Nothing Then
                Exit Sub
            End If

            Dim lsObjectTypeId As String = If(arRoot.ref, "").Trim
            If lsObjectTypeId = "" Then Exit Sub

            Dim lrObjectType As FBM.ModelObject = Nothing

            lrObjectType = arModel.EntityType.Find(Function(x) x.Id = lsObjectTypeId)

            If lrObjectType Is Nothing Then
                lrObjectType = arModel.ValueType.Find(Function(x) x.Id = lsObjectTypeId)
            End If

            If lrObjectType Is Nothing Then
                lrObjectType = arModel.FactType.Find(Function(x) x.Id = lsObjectTypeId)
            End If

            arRoot.BostonModelElement = lrObjectType

        End Sub

        Private Sub ResolveRoleSubPathRoots(ByVal arSubPath As FBM.RoleSubPath,
                                            ByVal arModel As FBM.Model)

            If arSubPath Is Nothing Then Exit Sub

            ResolveRootObjectType(arSubPath.RootObjectType,
                                 arModel)

            If arSubPath.SubPath IsNot Nothing Then
                For Each lrChild As FBM.RoleSubPath In arSubPath.SubPath
                    ResolveRoleSubPathRoots(lrChild, arModel)
                Next
            End If

        End Sub

        Private Sub ResolveDerivationRolePathRoots(ByVal arModel As FBM.Model)

            For Each lrFactType As FBM.FactType In arModel.FactType

                If lrFactType.DerivationRule Is Nothing OrElse
                               lrFactType.DerivationRule.FactTypeDerivationPath Is Nothing OrElse
                               lrFactType.DerivationRule.FactTypeDerivationPath.PathComponents Is Nothing Then
                    Continue For
                End If

                For Each lrRolePath As FBM.RolePath In lrFactType.DerivationRule.FactTypeDerivationPath.PathComponents.RolePaths

                    ResolveRootObjectType(lrRolePath.RootObjectType,
                                          arModel)

                    If lrRolePath.SubPath IsNot Nothing Then
                        For Each lrSubPath As FBM.RoleSubPath In lrRolePath.SubPath
                            ResolveRoleSubPathRoots(lrSubPath, arModel)
                        Next
                    End If

                Next

            Next

        End Sub

#End Region


        ''' <summary>
        ''' Gets the details of a FBM.FactType from the XMLModel.FactType in the Model (me)
        ''' </summary>
        ''' <param name="arFactType">The FactType for which the details are being retrieved</param>
        ''' <remarks></remarks>
        Public Sub GetFactTypeDetails(ByRef arFactType As FBM.FactType,
                                      Optional ByRef arXMlFactType As XMLModel.FactType = Nothing)

            Dim lsMessage As String = ""

            Try
                Dim lsFactTypeId As String = arFactType.Id
                Dim lrXMLRole As XMLModel.Role
                Dim lrRole As FBM.Role
                Dim lrXMLFactType As XMLModel.FactType
                Dim lrFactType = arFactType

                If arXMlFactType Is Nothing Then
                    lrXMLFactType = Me.ORMModel.FactTypes.Find(Function(x) x.Id = lsFactTypeId)

                    If lrXMLFactType Is Nothing Then
                        'Something has gone wrong. Create an EntityType for the FactType
                        arFactType = Nothing
                        Exit Sub
                    End If
                Else
                    lrXMLFactType = arXMlFactType
                End If


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
                If lrXMLFactType.IsLinkFactType Then
                    arFactType.LinkFactTypeRole = arFactType.Model.Role.Find(Function(x) x.Id = lrXMLFactType.LinkFactTypeRoleId)
                    If arFactType.LinkFactTypeRole Is Nothing Then
                        arFactType.IsLinkFactType = False
                    End If
                End If
                arFactType.IsMDAModelElement = lrXMLFactType.IsMDAModelElement
                arFactType.IsSubtypeStateControlling = lrXMLFactType.IsSubtypeStateControlling
                arFactType.StoreFactCoordinates = lrXMLFactType.StoreFactCoordinates
                arFactType.ShortDescription = lrXMLFactType.ShortDescription
                arFactType.LongDescription = lrXMLFactType.LongDescription
                arFactType.DBName = lrXMLFactType.DBName
                arFactType.ModelElementFlag = lrXMLFactType.ModelElementFlags
                arFactType.GraphLabel = lrXMLFactType.GraphLabel.Select(Function(x) New RDS.GraphLabel With {.Model = lrFactType.Model, .ModelElement = lrFactType, .ModelElementId = lrFactType.Id, .ModelId = lrFactType.Model.ModelId, .Label = x}).ToList

                If lrXMLFactType.ObjectifyingEntityTypeId = "" Then
                    arFactType.ObjectifyingEntityType = Nothing
                Else
                    Dim lsEntityTypeId As String = ""
                    lsEntityTypeId = lrXMLFactType.ObjectifyingEntityTypeId
                    arFactType.ObjectifyingEntityType = arFactType.Model.EntityType.Find(Function(x) x.Id = lsEntityTypeId)
                    If arFactType.ObjectifyingEntityType IsNot Nothing Then
                        arFactType.ObjectifyingEntityType.IsObjectifyingEntityType = True
                        '20220530-VM-Commented out. Remove if not needed.
                        'arFactType.ObjectifyingEntityType.ObjectifiedFactType = New FBM.FactType                        
                        arFactType.ObjectifyingEntityType.ObjectifiedFactType = arFactType
                    End If

                    If arFactType.ObjectifyingEntityType IsNot Nothing Then
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

#Region "Derivation Rule"

                '------------------------------------------------------------
                ' XMLModel.FactType.DerivationRule  -> FBM.FactType.DerivationRule
                '------------------------------------------------------------
                If lrXMLFactType.DerivationRule IsNot Nothing Then

                    Dim lrXMLDerivationRule As XMLModel.DerivationRule = lrXMLFactType.DerivationRule
                    Dim lrDerivationRule As New FBM.DerivationRule

                    '------------------------------------------------------------
                    ' FactTypeDerivationPath
                    '------------------------------------------------------------
                    If lrXMLDerivationRule.FactTypeDerivationPath IsNot Nothing Then

                        Dim lrXMLDerivationPath As XMLModel.FactTypeDerivationPath = lrXMLDerivationRule.FactTypeDerivationPath
                        Dim lrDerivationPath As New FBM.FactTypeDerivationPath

                        lrDerivationPath.Id = lrXMLDerivationPath.Id
                        lrDerivationPath.Name = lrXMLDerivationPath.Name

                        '------------------------------------------------------------
                        ' PathComponents.RolePath
                        '------------------------------------------------------------
                        If lrXMLDerivationPath.PathComponents IsNot Nothing AndAlso lrXMLDerivationPath.PathComponents.RolePath.Count > 0 Then

                            'Dim lrXMLRolePath As XMLModel.RolePath = lrXMLDerivationPath.PathComponents.RolePath 'Now multiple (below)
                            Dim lrPathComponents As New FBM.PathComponents

                            For Each lrXMLRolePath As XMLModel.RolePath In lrXMLDerivationPath.PathComponents.RolePath
                                Dim lrRolePath As New FBM.RolePath

                                lrRolePath.Id = lrXMLRolePath.Id
                                lrRolePath.SplitCombinationOperator = lrXMLRolePath.SplitCombinationOperator

                                'RootObjectType might be XmlIgnore in XML; set if available, otherwise leave Nothing.
                                'If your XMLModel.RolePath has RootObjectTypeId, this will pick it up.
                                If lrXMLRolePath.RootObjectType IsNot Nothing Then

                                    lrRolePath.RootObjectType = New FBM.RootObjectType
                                    lrRolePath.RootObjectType.id = lrXMLRolePath.RootObjectType.Id
                                    lrRolePath.RootObjectType.ref = lrXMLRolePath.RootObjectType.ModelElementId
                                    lrRolePath.RootObjectType.IsNegated = lrXMLRolePath.RootObjectType.IsNegated

                                    'BostonModelElement is resolved after every Fact Type has loaded.

                                End If

                                '------------------------------------------------------------
                                ' RolePath.PathedRoles
                                '------------------------------------------------------------
                                If lrXMLRolePath.PathedRole IsNot Nothing Then
                                    For Each lrXMLPathedRole As XMLModel.PathedRole In lrXMLRolePath.PathedRole
                                        Dim lrPathedRole As New FBM.PathedRole
                                        lrPathedRole.id = lrXMLPathedRole.id
                                        lrPathedRole.Ref = lrXMLPathedRole.Ref
                                        lrPathedRole.IsNegated = lrXMLPathedRole.IsNegated
                                        lrPathedRole.Purpose = lrXMLPathedRole.Purpose

                                        lrPathedRole.ValueRestriction = ConvertPathedRoleValueRestrictionFromXmlModel(lrXMLPathedRole.ValueRestriction)

                                        lrRolePath.PathedRole.Add(lrPathedRole)
                                    Next
                                End If

                                '------------------------------------------------------------
                                ' RolePath.SubPaths
                                '------------------------------------------------------------
                                If lrXMLRolePath.SubPath IsNot Nothing Then

                                    For Each lrXMLSubPath As XMLModel.RoleSubPath In lrXMLRolePath.SubPath

                                        Dim lrSubPath As FBM.RoleSubPath = ConvertRoleSubPathFromXmlModel(lrXMLSubPath,
                                                                                                          arFactType.Model)

                                        If lrSubPath IsNot Nothing Then
                                            lrRolePath.SubPath.Add(lrSubPath)
                                        End If

                                    Next

                                End If

                                '------------------------------------------------------------
                                ' RolePath.ObjectUnifiers
                                '------------------------------------------------------------
                                If lrXMLRolePath.ObjectUnifier IsNot Nothing Then

                                    For Each lrXMLObjectUnifier As XMLModel.ObjectUnifier In lrXMLRolePath.ObjectUnifier

                                        Dim lrObjectUnifier As New FBM.ObjectUnifier
                                        lrObjectUnifier.id = lrXMLObjectUnifier.id

                                        '-----------------------------------------
                                        ' PathedRole refs
                                        '-----------------------------------------
                                        If lrXMLObjectUnifier.PathedRoles IsNot Nothing Then
                                            For Each lrXMLPathedRoleRef As XMLModel.PathedRoleReference In lrXMLObjectUnifier.PathedRoles

                                                Dim lrPathedRoleRef As New FBM.PathedRoleReference
                                                lrPathedRoleRef.Ref = lrXMLPathedRoleRef.Ref
                                                lrObjectUnifier.PathedRoles.Add(lrPathedRoleRef)

                                            Next
                                        End If

                                        '-----------------------------------------
                                        ' PathRoot refs
                                        '-----------------------------------------
                                        If lrXMLObjectUnifier.PathRoots IsNot Nothing Then
                                            For Each lrXMLPathRootRef As XMLModel.PathRootReference In lrXMLObjectUnifier.PathRoots

                                                Dim lrPathRootRef As New FBM.PathRootReference
                                                lrPathRootRef.Ref = lrXMLPathRootRef.Ref
                                                lrObjectUnifier.PathRoots.Add(lrPathRootRef)

                                            Next
                                        End If

                                        lrRolePath.ObjectUnifier.Add(lrObjectUnifier)

                                    Next

                                End If

                                '------------------------------------------------------------
                                ' RolePath.CalculatedValues (typed)
                                '------------------------------------------------------------
                                If lrXMLRolePath.CalculatedValues IsNot Nothing Then

                                    For Each lrXMLCalculatedValue As XMLModel.CalculatedValue In lrXMLRolePath.CalculatedValues

                                        Dim lrCalculatedValue As New FBM.CalculatedValue
                                        lrCalculatedValue.Id = lrXMLCalculatedValue.Id

                                        'Function ref
                                        If lrXMLCalculatedValue.Function IsNot Nothing Then
                                            lrCalculatedValue.Function = New FBM.FunctionRef
                                            lrCalculatedValue.Function.Ref = lrXMLCalculatedValue.Function.Ref
                                        End If

                                        'AggregationContext
                                        If lrXMLCalculatedValue.AggregationContext IsNot Nothing Then

                                            Dim lrAggregationContext As New FBM.AggregationContext

                                            If lrXMLCalculatedValue.AggregationContext.PathRoot IsNot Nothing Then
                                                lrAggregationContext.PathRoot = New FBM.PathRootReference With {
                                                    .Ref = lrXMLCalculatedValue.AggregationContext.PathRoot.Ref
                                                }
                                            End If

                                            lrCalculatedValue.AggregationContext = lrAggregationContext

                                        End If

                                        'Inputs
                                        If lrXMLCalculatedValue.Inputs IsNot Nothing AndAlso lrXMLCalculatedValue.Inputs.Input IsNot Nothing Then
                                            lrCalculatedValue.Inputs = New FBM.Inputs

                                            For Each lrXMLInput As XMLModel.Input In lrXMLCalculatedValue.Inputs.Input

                                                Dim lrInput As New FBM.Input
                                                lrInput.Id = lrXMLInput.Id

                                                If lrXMLInput.Parameter IsNot Nothing Then
                                                    lrInput.Parameter = New FBM.ParameterRef
                                                    lrInput.Parameter.Ref = lrXMLInput.Parameter.Ref
                                                End If

                                                If lrXMLInput.Source IsNot Nothing Then
                                                    lrInput.Source = New FBM.Source

                                                    If lrXMLInput.Source.Item IsNot Nothing Then

                                                        If TypeOf lrXMLInput.Source.Item Is XMLModel.PathRootReference Then

                                                            Dim lrInRef As XMLModel.PathRootReference = CType(lrXMLInput.Source.Item, XMLModel.PathRootReference)

                                                            Dim lrOutRef As New FBM.PathRootReference
                                                            lrOutRef.Ref = lrInRef.Ref
                                                            lrInput.Source.Item = lrOutRef

                                                        ElseIf TypeOf lrXMLInput.Source.Item Is XMLModel.PathedRoleRef Then
                                                            Dim lrInRef As XMLModel.PathedRoleRef = CType(lrXMLInput.Source.Item, XMLModel.PathedRoleRef)
                                                            Dim lrOutRef As New FBM.PathedRoleRef
                                                            lrOutRef.Ref = lrInRef.Ref
                                                            lrInput.Source.Item = lrOutRef

                                                        ElseIf TypeOf lrXMLInput.Source.Item Is XMLModel.CalculatedValueRef Then
                                                            Dim lrInRef As XMLModel.CalculatedValueRef = CType(lrXMLInput.Source.Item, XMLModel.CalculatedValueRef)
                                                            Dim lrOutRef As New FBM.CalculatedValueRef
                                                            lrOutRef.Ref = lrInRef.Ref
                                                            lrInput.Source.Item = lrOutRef

                                                        ElseIf TypeOf lrXMLInput.Source.Item Is XMLModel.Constant Then
                                                            Dim lrInConst As XMLModel.Constant = CType(lrXMLInput.Source.Item, XMLModel.Constant)
                                                            Dim lrOutConst As New FBM.Constant
                                                            lrOutConst.Id = lrInConst.Id
                                                            lrOutConst.Value = lrInConst.Value
                                                            lrInput.Source.Item = lrOutConst

                                                        End If
                                                    End If
                                                End If

                                                lrCalculatedValue.Inputs.Input.Add(lrInput)

                                            Next 'Input
                                        End If 'Inputs

                                        lrRolePath.CalculatedValues.Add(lrCalculatedValue)

                                    Next 'CalculatedValue

                                End If

                                '------------------------------------------------------------
                                ' RolePath.Conditions (typed)
                                '------------------------------------------------------------
                                If lrXMLRolePath.Conditions IsNot Nothing Then

                                    Dim lrConditions As New FBM.Conditions

                                    If lrXMLRolePath.Conditions.Items IsNot Nothing Then
                                        For Each lrXMLNode As XMLModel.ConditionNode In lrXMLRolePath.Conditions.Items
                                            Dim lrNode As FBM.ConditionNode = ConvertConditionNodeFromXmlModel(lrXMLNode)
                                            If lrNode IsNot Nothing Then
                                                lrConditions.Items.Add(lrNode)
                                            End If
                                        Next
                                    End If

                                    lrRolePath.Conditions = lrConditions

                                End If

                                lrPathComponents.RolePaths.Add(lrRolePath)
                            Next

                            lrDerivationPath.PathComponents = lrPathComponents

                        End If

                        '------------------------------------------------------------
                        ' DerivationProjections
                        '------------------------------------------------------------
#Region "Derivation Projection/s"
                        If lrXMLDerivationPath.Projection IsNot Nothing Then

                            For Each lrXMLProjection As XMLModel.DerivationProjection In lrXMLDerivationPath.Projection

                                Dim lrProjection As New FBM.DerivationProjection

                                lrProjection.Id = lrXMLProjection.Id
                                lrProjection.Ref = lrXMLProjection.Ref

                                '-----------------------------------------
                                ' RoleProjection (0..n)
                                '-----------------------------------------
                                If lrXMLProjection.RoleProjection IsNot Nothing Then

                                    For Each lrXMLRoleProjection As XMLModel.RoleProjection In lrXMLProjection.RoleProjection

                                        Dim lrRoleProjection As New FBM.RoleProjection

                                        lrRoleProjection.Id = lrXMLRoleProjection.Id
                                        lrRoleProjection.Ref = lrXMLRoleProjection.Ref

                                        '-----------------------------------------
                                        ' DerivationSource
                                        '-----------------------------------------
                                        If lrXMLRoleProjection.DerivationSource IsNot Nothing Then

                                            Dim lrDerivationSource As New FBM.DerivationSource

                                            ' PathRoot
                                            If lrXMLRoleProjection.DerivationSource.PathRoot IsNot Nothing Then
                                                lrDerivationSource.PathRoot = New FBM.PathRootReference With {
                                                                .Ref = lrXMLRoleProjection.DerivationSource.PathRoot.Ref
                                                            }
                                            End If

                                            ' CalculatedValue
                                            If lrXMLRoleProjection.DerivationSource.CalculatedValue IsNot Nothing Then
                                                lrDerivationSource.CalculatedValue = New FBM.CalculatedValueReference With {
                                                                .Ref = lrXMLRoleProjection.DerivationSource.CalculatedValue.Ref
                                                            }
                                            End If

                                            ' PathedRole
                                            If lrXMLRoleProjection.DerivationSource.PathedRole IsNot Nothing Then
                                                lrDerivationSource.PathedRole = New FBM.PathedRoleReference With {
                                                                .Ref = lrXMLRoleProjection.DerivationSource.PathedRole.Ref
                                                            }
                                            End If

                                            lrRoleProjection.DerivationSource = lrDerivationSource

                                        End If

                                        lrProjection.RoleProjection.Add(lrRoleProjection)

                                    Next 'RoleProjection

                                End If

                                lrDerivationPath.Projection.Add(lrProjection)

                            Next 'DerivationProjection

                        End If
#End Region

                        lrDerivationRule.FactTypeDerivationPath = lrDerivationPath

                    End If

                    arFactType.DerivationRule = lrDerivationRule
                    arFactType.IsDerived = True
                    'DerivationText can only really be calculated once all FactTypes have been loaded for the Model, because they may be needed by the derivation.
                    'arFactType.DerivationText = BostonDerivationRenderer.RenderDerivationEnglish(arFactType.Model, arFactType)

                Else
                    arFactType.DerivationRule = Nothing
                End If

#End Region

                '-----------------------------------------------------
                'Get the Roles within the RoleGroup for the FactType
                '-----------------------------------------------------                
                Dim lrModelElement As FBM.ModelObject
                Dim lrDictionaryEntry As FBM.DictionaryEntry
                For Each lrXMLRole In lrXMLFactType.RoleGroup

                    lrRole = New FBM.Role
                    lrRole.Model = arFactType.Model
                    lrRole.FactType = arFactType
                    lrRole.Id = lrXMLRole.Id
                    lrRole.Name = lrXMLRole.Name
                    lrRole.SequenceNr = lrXMLRole.SequenceNr
                    lrRole.Mandatory = lrXMLRole.Mandatory

                    lrModelElement = arFactType.Model.EntityType.Find(Function(x) x.Id = lrXMLRole.JoinedObjectTypeId)

                    If lrModelElement IsNot Nothing Then
                        If CType(lrModelElement, FBM.EntityType).IsObjectifyingEntityType Then
                            lrModelElement = arFactType.Model.FactType.Find(Function(x) x.Id = lrXMLRole.JoinedObjectTypeId)
                            If lrModelElement IsNot Nothing Then
                                lrRole.JoinedORMObject = lrModelElement
                                GoTo FoundModelElement
                            End If
                            GoTo KeepLooking
                        End If
                        lrRole.JoinedORMObject = lrModelElement
                        GoTo FoundModelElement
                    End If
KeepLooking:
                    Dim lrModelElementFound = arFactType.Model.ValueType.Find(Function(x) x.Id = lrXMLRole.JoinedObjectTypeId)
                    If If(lrModelElementFound IsNot Nothing, lrModelElementFound, lrModelElement) IsNot Nothing Then
                        lrRole.JoinedORMObject = lrModelElement
                    Else
                        lrRole.JoinedORMObject = arFactType.Model.FactType.Find(Function(x) x.Id = lrXMLRole.JoinedObjectTypeId)
                        If lrRole.JoinedORMObject Is Nothing Then
                            lrRole.JoinedORMObject = New FBM.FactType(lrRole.Model, lrXMLRole.JoinedObjectTypeId, True)
                            Me.GetFactTypeDetails(lrRole.JoinsFactType)

                            If lrRole.JoinedORMObject Is Nothing Then
                                'Something has gone wrong. Create a DummyEntityType
                                lrRole.JoinedORMObject = New FBM.EntityType(lrRole.Model, pcenumLanguage.ORMModel, lrXMLRole.JoinedObjectTypeId, lrXMLRole.JoinedObjectTypeId)
                                prApplication.ThrowMessage("There was a problem finding the Object Type, " & lrXMLRole.JoinedObjectTypeId & ", so a dummy Entity Type has been created in its place.", pcenumErrorType.Warning, Nothing, False, False, True)
                                lrRole.Model.AddEntityType(lrRole.JoinedORMObject,, False, Nothing, True, True)
                            End If
                        End If
                    End If
FoundModelElement:
                    '--------------------------------------------------
                    'Add the Role to the Model (list of Role) as well
                    '--------------------------------------------------
                    arFactType.Model.Role.Add(lrRole)

                    'RoleNames in ModelDictionary if not "", so can save to database without ForeignKey constraint fail on ModelDictionary.
                    If lrRole.Name.Trim <> "" Then
                        lrDictionaryEntry = New FBM.DictionaryEntry(arFactType.Model, lrRole.Name, pcenumConceptType.Value,,, True, True,)
                        lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry, , False,, True,, True)
                    End If

                    arFactType.RoleGroup.Add(lrRole)
                Next

#Region "Property Graph Schema specific"
                arFactType.Source = lrXMLFactType.Source
                arFactType.Target = lrXMLFactType.Target
#End Region

                '-------------------------------------------
                'Get the FactTypeReadings for the FactType
                '-------------------------------------------
                arFactType.FactTypeReading = Me.GetFactTypeReadingsForFactType(arFactType, lrXMLFactType)

                '-------------------------------------------
                'Get the Facts (FactData) for the FactType
                '-------------------------------------------
                Call Me.GetFactsForFactType(arFactType, lrXMLFactType)

                '------------------------------------------------
                'Link to the Concept within the ModelDictionary
                '------------------------------------------------
                lrDictionaryEntry = New FBM.DictionaryEntry(arFactType.Model,
                                                            arFactType.Id,
                                                            pcenumConceptType.FactType,
                                                            arFactType.ShortDescription,
                                                            arFactType.LongDescription,
                                                            True,
                                                            True,
                                                            arFactType.DBName)

                lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry, , False,, False,, True) '20220117-VM-Was this. DBName wasn't working.

                arFactType.Concept = lrDictionaryEntry.Concept

                Call arFactType.Model.AddFactType(arFactType, False, False, Nothing)

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function GetFactTypeReadingsForFactType(ByRef arFactType As FBM.FactType,
                                                       Optional ByRef arXMLFactType As XMLModel.FactType = Nothing) As List(Of FBM.FactTypeReading)

            Dim lrXMLFactType As XMLModel.FactType
            Dim lrXMLFactTypeReading As XMLModel.FactTypeReading
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lsMessage As String = ""

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetFactTypeReadingsForFactType = New List(Of FBM.FactTypeReading)

            Try
                Dim lsFactTypeId As String = arFactType.Id
                If arXMLFactType Is Nothing Then
                    lrXMLFactType = Me.ORMModel.FactTypes.Find(Function(x) x.Id = lsFactTypeId)
                Else
                    lrXMLFactType = arXMLFactType
                End If

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
                    For Each lrXMLPredicatePart In lrXMLFactTypeReading.PredicateParts

                        lrPredicatePart = New FBM.PredicatePart(arFactType.Model, lrFactTypeReading, Nothing, True)
                        lrPredicatePart.SequenceNr = lrXMLPredicatePart.SequenceNr
                        lrPredicatePart.PreBoundText = lrXMLPredicatePart.PreboundReadingText
                        lrPredicatePart.PostBoundText = lrXMLPredicatePart.PostboundReadingText
                        lrPredicatePart.PredicatePartText = Trim(lrXMLPredicatePart.PredicatePartText)

                        lrPredicatePart.Role = arFactType.RoleGroup.Find(Function(x) x.Id = lrXMLPredicatePart.Role_Id)

                        lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                    Next

                    If Not lrFactTypeReading.PredicatePart IsNot Nothing Then
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
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

#Region "NORM FT Old"
        '        Public Sub GetNORMAFactTypeDetails(ByRef arNORMADocument As NORMA.ORMDocument,
        '                                           ByRef arNORMAFactType As NORMA.Model.Fact,
        '                                           ByVal arFBMFactType As XMLModel.FactType)

        '            '================================================
        '            'Map the PlayedRoles for Type and Roles For Fact
        '            '================================================
        '            Dim latType = {
        '                GetType(NORMA.Model.ValueType),
        '                GetType(NORMA.Model.EntityType),
        '                GetType(NORMA.Model.ObjectifiedType)
        '            }

        '            Try

        '                For Each lrFBMRole In arFBMFactType.RoleGroup.FindAll(Function(x) x.JoinedObjectType IsNot Nothing)

        '                    lrFBMRole.JoinedObjectType = Me.ORMModel.getModelElementById(lrFBMRole.JoinedObjectTypeId)

        '                    Dim lrNORMARole As New NORMA.Model.Fact.FactRole()
        '                    lrNORMARole.Id = "_" & lrFBMRole.Id
        '                    lrNORMARole.Name = lrFBMRole.Name
        '                    lrNORMARole._IsMandatory = lrFBMRole.Mandatory

        '                    Dim larJoinedObject As New List(Of Object)

        '                    latType = {
        '                               GetType(NORMA.Model.ValueType),
        '                               GetType(NORMA.Model.EntityType),
        '                               GetType(NORMA.Model.ObjectifiedType)
        '                              }

        '                    Select Case lrFBMRole.JoinedObjectType.GetType

        '                        Case Is = GetType(XMLModel.EntityType)

        '                            If CType(lrFBMRole.JoinedObjectType, XMLModel.EntityType).IsObjectifyingEntityType Then
        '                                latType = {
        '                                            GetType(NORMA.Model.ObjectifiedType)
        '                                          }
        '                            End If

        '                        Case Is = GetType(XMLModel.FactType)

        '                            latType = {
        '                                       GetType(NORMA.Model.ObjectifiedType)
        '                                      }

        '                    End Select

        '                    larJoinedObject = (From lrObject In arNORMADocument.ORMModel.Objects.Items
        '                                       Where latType.Contains(lrObject.GetType) AndAlso lrObject.Name = lrFBMRole.JoinedObjectTypeId
        '                                       Select lrObject).ToList

        '                    Dim lrJoinedORMObject = larJoinedObject.FirstOrDefault()

        '                    If lrJoinedORMObject Is Nothing Then

        '                        Dim lrFBMFactType As XMLModel.FactType = Me.ORMModel.FactTypes.Find(Function(x) x.Id = lrFBMRole.JoinedObjectTypeId)

        '                        If lrFBMFactType IsNot Nothing Then
        '                            Dim lrNORMAFactType As New NORMA.Model.Fact("_" & lrFBMFactType.GUID, lrFBMFactType.Name)
        '                            Call Me.GetNORMAFactTypeDetails(arNORMADocument, lrNORMAFactType, lrFBMFactType)

        '                            If IsNothing(arNORMADocument.ORMModel.Facts) Then
        '                                arNORMADocument.ORMModel.Facts = New NORMA.ORMModelFacts()
        '                                arNORMADocument.ORMModel.Facts.Items = Array.CreateInstance(GetType(Object), 0)
        '                            End If
        '                            arNORMADocument.ORMModel.Facts.Items.Add(lrNORMAFactType)

        '                            larJoinedObject = (From lrObject In arNORMADocument.ORMModel.Objects.Items
        '                                               Where latType.Contains(lrObject.GetType) AndAlso lrObject.Name = lrFBMRole.JoinedObjectTypeId
        '                                               Select lrObject).ToList

        '                            lrJoinedORMObject = larJoinedObject.FirstOrDefault()
        '                        End If

        '                    End If

        '                    lrNORMARole.RolePlayer = New NORMA.Model.Fact.FactRole.FactRoleRolePlayer()

        '                    If lrJoinedORMObject IsNot Nothing Then
        '                        lrNORMARole.RolePlayer.Ref = lrJoinedORMObject.Id

        '                        Select Case lrJoinedORMObject.GetType()
        '                            Case GetType(NORMA.Model.EntityType)
        '                                If IsNothing(lrJoinedORMObject.PlayedRoles) Then
        '                                    lrJoinedORMObject.PlayedRoles = New NORMA.Model.EntityType.EntityTypePlayedRoles()
        '                                    lrJoinedORMObject.PlayedRoles.Items = New Object() {}
        '                                End If
        '                                CType(lrJoinedORMObject.PlayedRoles, NORMA.Model.EntityType.EntityTypePlayedRoles).Items.
        '                                    Add(New NORMA.Model.EntityType.EntityTypePlayedRoles.PlayedRolesRole() With {.Ref = lrNORMARole.Id})
        '                            Case GetType(NORMA.Model.ValueType)
        '                                If IsNothing(lrJoinedORMObject.PlayedRoles) Then
        '                                    lrJoinedORMObject.PlayedRoles = New List(Of NORMA.Model.ValueType.ValueTypeRole)
        '                                End If
        '                                CType(lrJoinedORMObject.PlayedRoles, List(Of NORMA.Model.ValueType.ValueTypeRole)).
        '                                    Add(New NORMA.Model.ValueType.ValueTypeRole() With {.Ref = lrNORMARole.Id})
        '                            Case GetType(NORMA.Model.ObjectifiedType)
        '                                If IsNothing(lrJoinedORMObject.PlayedRoles) Then
        '                                    lrJoinedORMObject.PlayedRoles = New List(Of NORMA.Model.ObjectifiedType.ObjectifiedTypeRole)
        '                                End If
        '                                CType(lrJoinedORMObject.PlayedRoles, List(Of NORMA.Model.ObjectifiedType.ObjectifiedTypeRole)).
        '                                    Add(New NORMA.Model.ObjectifiedType.ObjectifiedTypeRole() With {.Ref = lrNORMARole.Id})
        '                        End Select
        '                    Else
        '                        prApplication.ThrowMessage("Trouble finding Joined ORM Object, " & lrFBMRole.JoinedObjectTypeId & ", for Fact Type: " & arFBMFactType.Id, pcenumErrorType.Warning,, False,, True)
        '                    End If

        '                    If IsNothing(arNORMAFactType.FactRoles) Then
        '                        arNORMAFactType.FactRoles = New List(Of NORMA.Model.Fact.FactRole)
        '                    End If
        '                    arNORMAFactType.FactRoles.Add(lrNORMARole)

        '                    '======================================================================
        '                    'Add the MandatoryConstraint for the Role of the FactType if required
        '                    '====================================================================
        '#Region "Mandatory Role Constraints"
        '                    Dim liMandatorySimpleCounter As Integer = 1
        '                    Dim liMandatoryImpliedCounter As Integer = 1
        '                    Dim lrMandatoryConstraint As New NORMA.Model.MandatoryConstraint()

        '                    lrMandatoryConstraint.RoleSequence = New List(Of NORMA.Model.MandatoryConstraint.ConstraintRole)
        '                    lrMandatoryConstraint.RoleSequence.Add(New NORMA.Model.MandatoryConstraint.ConstraintRole() With {.Ref = "_" & lrFBMRole.Id})

        '                    If lrFBMRole.Mandatory Then
        '                        lrMandatoryConstraint.Name = $"SimpleMandatoryConstraint{liMandatorySimpleCounter}"
        '                        lrMandatoryConstraint.IsSimple = True

        '                        If IsNothing(arNORMAFactType.InternalConstraints) Then
        '                            arNORMAFactType.InternalConstraints = New NORMA.Model.Fact.FactInternalConstraints()
        '                            arNORMAFactType.InternalConstraints.Items = New List(Of Object)
        '                        End If

        '                        arNORMAFactType.InternalConstraints.Items.Add(New NORMA.Model.Fact.FactInternalConstraints.MandatoryConstraint() With {.Ref = lrMandatoryConstraint.Id})

        '                        If IsNothing(arNORMADocument.ORMModel.Constraints) Then
        '                            arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
        '                            arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
        '                        End If

        '                        arNORMADocument.ORMModel.Constraints.Items.Add(lrMandatoryConstraint)
        '                    End If

        '#End Region 'Mandatory Role Constraints

        '#Region "Implied Mandatory Constraint"
        '                    '20220729-VM-Don't know how to do ImpliedMandatoryConstraints yet.
        '                    'lrMandatoryConstraint.Name = $"ImpliedMandatoryConstraint{liMandatoryImpliedCounter}"
        '                    'lrMandatoryConstraint.IsImplied = True
        '                    'lrMandatoryConstraint.ImpliedByObjectType = New NORMA.Model.MandatoryConstraint.ConstraintImpliedByObjectType()
        '                    'lrMandatoryConstraint.ImpliedByObjectType.Ref = lrRoleAndObject.ORMObject.Id
        '#End Region

        '#Region "Role Value Constraint"
        '                    Dim larRoleValueConstraint = From RoleConstraint In Me.ORMModel.RoleConstraints
        '                                                 Where RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.RoleValueConstraint.ToString
        '                                                 Where RoleConstraint.RoleConstraintRoles.Count = 1
        '                                                 Where RoleConstraint.RoleConstraintRoles(0).RoleId = lrFBMRole.Id
        '                                                 Select RoleConstraint
        '                    If larRoleValueConstraint.Count > 0 Then

        '                        lrNORMARole.ValueRestriction = New NORMA.Model.Fact.FactRole.FactRoleValueRestriction

        '                        Dim lrFBMRoleConstraint As XMLModel.RoleConstraint = larRoleValueConstraint(0)

        '                        Dim lrNORMARoleValueConstraint As New NORMA.Model.Fact.FactRole.FactRoleValueRestriction.RestrictionRoleValueConstraint With {.Name = lrFBMRoleConstraint.Id}

        '                        For Each lsValueRange In lrFBMRoleConstraint.ValueConstraint
        '                            Dim lrNORMAValueRange As New NORMA.Model.Fact.FactRole.FactRoleValueRestriction.ConstraintValueRangesValueRange
        '                            lrNORMAValueRange.MinValue = lsValueRange
        '                            lrNORMAValueRange.MaxValue = lsValueRange
        '                            lrNORMAValueRange.MinInclusion = "NotSet"
        '                            lrNORMAValueRange.MaxInclusion = "NotSet"

        '                            lrNORMARoleValueConstraint.ValueRanges.Add(lrNORMAValueRange)
        '                        Next

        '                        lrNORMARole.ValueRestriction.RoleValueConstraint = lrNORMARoleValueConstraint

        '                    End If
        '#End Region
        '                Next

        '#Region "Objectification"
        '                If arFBMFactType.IsObjectified Then

        '                    Dim lrNORMAObjectifiedType = New NORMA.Model.ObjectifiedType()
        '                    lrNORMAObjectifiedType.Name = arFBMFactType.Name

        '                    Dim larFBMPreferredIdentifer = From InternalUniquenssConstraint In Me.ORMModel.RoleConstraints
        '                                                   Where InternalUniquenssConstraint.RoleConstraintType = "InternalUniquenessConstraint"
        '                                                   From RoleConstraintRole In InternalUniquenssConstraint.RoleConstraintRoles
        '                                                   From Role In arFBMFactType.RoleGroup
        '                                                   Where Role.Id = RoleConstraintRole.RoleId
        '                                                   Where InternalUniquenssConstraint.IsPreferredUniqueness
        '                                                   Select InternalUniquenssConstraint

        '                    If larFBMPreferredIdentifer.Count > 0 Then
        '                        lrNORMAObjectifiedType.PreferredIdentifier = New NORMA.Model.ObjectifiedType.ObjectifiedTypePreferredIdentifier("_" & larFBMPreferredIdentifer.First.GUID)
        '                    End If

        '                    lrNORMAObjectifiedType.NestedPredicate = New NORMA.Model.ObjectifiedType.ObjectifiedTypeNestedPredicate("_" & arFBMFactType.GUID)

        '                    Call arNORMADocument.ORMModel.Objects.Items.Add(lrNORMAObjectifiedType)

        '                End If

        '#End Region

        '                '================================================
        '                'Map the Readings For Fact
        '                '================================================
        '#Region "Fact Type Readings"
        '                For Each lrFBMReadings In arFBMFactType.FactTypeReadings

        '                    Dim lrReadingOrder As New NORMA.Model.Fact.FactReadingOrder()
        '                    lrReadingOrder.Readings = New NORMA.Model.Fact.FactReadingOrder.Reading() {}

        '                    ' START: get and add role sequence
        '                    lrReadingOrder.RoleSequence = New NORMA.Model.Fact.FactReadingOrder.Role() {}
        '                    For Each lrFBMSequence In From lrFBMRoleSequence In lrFBMReadings.PredicateParts
        '                                              From lrFBMRole In arFBMFactType.RoleGroup
        '                                              Where lrFBMRole.Id = lrFBMRoleSequence.Role_Id
        '                                              Order By lrFBMRoleSequence.SequenceNr
        '                                              Select lrFBMRole
        '                        lrReadingOrder.RoleSequence.Add(New NORMA.Model.Fact.FactReadingOrder.Role() With {.Ref = "_" & lrFBMSequence.Id})
        '                    Next
        '                    ' END: get and add role sequence

        '                    Dim lrReading As New NORMA.Model.Fact.FactReadingOrder.Reading()
        '                    lrReading.ExpandedData = New NORMA.Model.Fact.FactReadingOrder.ExpandedData()
        '                    lrReadingOrder.Readings.Add(lrReading)

        '                    Dim lsPredicate As String = lrFBMReadings.FrontReadingText

        '                    ' START: generate reading for PredicateParts
        '                    Dim liInd As Integer = 1
        '                    For Each lrFBMPredicatePart In lrFBMReadings.PredicateParts

        '                        Dim lsFollowingTextExtension As String = ""
        '                        If liInd < lrFBMReadings.PredicateParts.Count Then
        '                            lsFollowingTextExtension = lrFBMReadings.PredicateParts(liInd).PreboundReadingText
        '                        Else
        '                            lsFollowingTextExtension = lrFBMReadings.FollowingReadingText
        '                        End If

        '                        Dim lrRoleText As New NORMA.Model.Fact.FactReadingOrder.RoleText
        '                        lrRoleText.RoleIndex = 0
        '                        lrRoleText.FollowingText = Trim(lrFBMPredicatePart.PredicatePartText & " " & lsFollowingTextExtension)

        '                        lrReading.ExpandedData.RoleText = lrRoleText

        '                        '20220730-VM-Fix this.
        '                        lsPredicate &= "{" & liInd - 1 & "}" & lrRoleText.FollowingText

        '                        liInd += 1
        '                    Next
        '                    lrReading.Data = lsPredicate
        '                    ' END: generate reading for PredicateParts

        '                    If IsNothing(arNORMAFactType.ReadingOrders) Then
        '                        arNORMAFactType.ReadingOrders = New List(Of NORMA.Model.Fact.FactReadingOrder)
        '                    End If
        '                    arNORMAFactType.ReadingOrders.Add(lrReadingOrder)
        '                Next
        '#End Region 'Fact Type Readings

        '            Catch ex As Exception
        '                Dim lsMessage As String
        '                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

        '                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
        '                lsMessage &= vbCrLf & vbCrLf & ex.Message
        '                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        '            End Try
        '        End Sub

#End Region

        Public Sub GetNORMAFactTypeDetails(ByRef arNORMADocument As NORMA.ORMDocument,
                                      ByRef arNORMAFactType As NORMA.Model.Fact,
                                      ByVal arFBMFactType As XMLModel.FactType)

            '================================================
            'Map the PlayedRoles for Type and Roles For Fact
            '================================================
            Dim latType = {
                GetType(NORMA.Model.ValueType),
                GetType(NORMA.Model.EntityType),
                GetType(NORMA.Model.ObjectifiedType)
            }

            For Each lrFBMRole In arFBMFactType.RoleGroup

                lrFBMRole.JoinedObjectType = Me.ORMModel.getModelElementById(lrFBMRole.JoinedObjectTypeId)

                Dim lrNORMARole As New NORMA.Model.Fact.FactRole()
                lrNORMARole.Id = "_" & lrFBMRole.Id
                lrNORMARole.Name = lrFBMRole.Name
                lrNORMARole._IsMandatory = lrFBMRole.Mandatory

                Dim lrJoinedORMObject As Object
                ' check the JoinedObject is Fact type
                If TypeOf lrFBMRole.JoinedObjectType Is FactType Then

                    ' find the ObjectifyingEntityTypeId from Fact type
                    lrJoinedORMObject = (From lrObject In arNORMADocument.ORMModel.Objects.Items
                                         Where latType.Contains(lrObject.GetType) AndAlso lrObject.Name = CType(lrFBMRole.JoinedObjectType, FactType).ObjectifyingEntityTypeId
                                         Select lrObject).FirstOrDefault()

                Else

                    ' find the Entity/Value/Objectified type
                    lrJoinedORMObject = (From lrObject In arNORMADocument.ORMModel.Objects.Items
                                         Where latType.Contains(lrObject.GetType) AndAlso lrObject.Name = lrFBMRole.JoinedObjectTypeId
                                         Select lrObject).FirstOrDefault()

                End If

                lrNORMARole.RolePlayer = New NORMA.Model.Fact.FactRole.FactRoleRolePlayer()
                If Not IsNothing(lrJoinedORMObject) Then
                    lrNORMARole.RolePlayer.Ref = lrJoinedORMObject.Id

                    Select Case lrJoinedORMObject.GetType()
                        Case GetType(NORMA.Model.EntityType)
                            If IsNothing(lrJoinedORMObject.PlayedRoles) Then
                                lrJoinedORMObject.PlayedRoles = New NORMA.Model.EntityType.EntityTypePlayedRoles()
                                lrJoinedORMObject.PlayedRoles.Items = New Object() {}
                            End If
                            CType(lrJoinedORMObject.PlayedRoles, NORMA.Model.EntityType.EntityTypePlayedRoles).Items.
                                Add(New NORMA.Model.EntityType.EntityTypePlayedRoles.PlayedRolesRole() With {.Ref = lrNORMARole.Id})
                        Case GetType(NORMA.Model.ValueType)
                            If IsNothing(lrJoinedORMObject.PlayedRoles) Then
                                lrJoinedORMObject.PlayedRoles = New List(Of NORMA.Model.ValueType.ValueTypeRole)
                            End If
                            CType(lrJoinedORMObject.PlayedRoles, List(Of NORMA.Model.ValueType.ValueTypeRole)).
                                Add(New NORMA.Model.ValueType.ValueTypeRole() With {.Ref = lrNORMARole.Id})
                        Case GetType(NORMA.Model.ObjectifiedType)
                            If IsNothing(lrJoinedORMObject.PlayedRoles) Then
                                lrJoinedORMObject.PlayedRoles = New List(Of NORMA.Model.ObjectifiedType.ObjectifiedTypeRole)
                            End If
                            CType(lrJoinedORMObject.PlayedRoles, List(Of NORMA.Model.ObjectifiedType.ObjectifiedTypeRole)).
                                Add(New NORMA.Model.ObjectifiedType.ObjectifiedTypeRole() With {.Ref = lrNORMARole.Id})
                    End Select

                End If

                If IsNothing(arNORMAFactType.FactRoles) Then
                    arNORMAFactType.FactRoles = New List(Of NORMA.Model.Fact.FactRole)
                End If
                arNORMAFactType.FactRoles.Add(lrNORMARole)

                '======================================================================
                'Add the MandatoryConstraint for the Role of the FactType if required
                '====================================================================
#Region "Mandatory Role Constraints"
                Dim liMandatorySimpleCounter As Integer = 1
                Dim liMandatoryImpliedCounter As Integer = 1
                Dim lrMandatoryConstraint As New NORMA.Model.MandatoryConstraint()

                lrMandatoryConstraint.RoleSequence = New List(Of NORMA.Model.MandatoryConstraint.ConstraintRole)
                lrMandatoryConstraint.RoleSequence.Add(New NORMA.Model.MandatoryConstraint.ConstraintRole() With {.Ref = "_" & lrFBMRole.Id})

                If lrFBMRole.Mandatory Then
                    lrMandatoryConstraint.Name = $"SimpleMandatoryConstraint{liMandatorySimpleCounter}"
                    lrMandatoryConstraint.IsSimple = True

                    If IsNothing(arNORMAFactType.InternalConstraints) Then
                        arNORMAFactType.InternalConstraints = New NORMA.Model.Fact.FactInternalConstraints()
                        arNORMAFactType.InternalConstraints.Items = New List(Of Object)
                    End If

                    arNORMAFactType.InternalConstraints.Items.Add(New NORMA.Model.Fact.FactInternalConstraints.MandatoryConstraint() With {.Ref = lrMandatoryConstraint.Id})

                    If IsNothing(arNORMADocument.ORMModel.Constraints) Then
                        arNORMADocument.ORMModel.Constraints = New NORMA.ORMModelConstraints()
                        arNORMADocument.ORMModel.Constraints.Items = New List(Of Object)
                    End If

                    arNORMADocument.ORMModel.Constraints.Items.Add(lrMandatoryConstraint)
                End If

#End Region 'Mandatory Role Constraints

#Region "Implied Mandatory Constraint"
                '20220729-VM-Don't know how to do ImpliedMandatoryConstraints yet.
                'lrMandatoryConstraint.Name = $"ImpliedMandatoryConstraint{liMandatoryImpliedCounter}"
                'lrMandatoryConstraint.IsImplied = True
                'lrMandatoryConstraint.ImpliedByObjectType = New NORMA.Model.MandatoryConstraint.ConstraintImpliedByObjectType()
                'lrMandatoryConstraint.ImpliedByObjectType.Ref = lrRoleAndObject.ORMObject.Id
#End Region

            Next

            '================================================
            'Map the Readings For Fact
            '================================================
#Region "Fact Type Readings"
            For Each lrFBMReadings In arFBMFactType.FactTypeReadings

                Dim lrReadingOrder As New NORMA.Model.Fact.FactReadingOrder()
                lrReadingOrder.Readings = New NORMA.Model.Fact.FactReadingOrder.Reading() {}

                ' START: get and add role sequence
                lrReadingOrder.RoleSequence = New NORMA.Model.Fact.FactReadingOrder.Role() {}
                For Each lrFBMSequence In From lrFBMRoleSequence In lrFBMReadings.PredicateParts
                                          From lrFBMRole In arFBMFactType.RoleGroup
                                          Where lrFBMRole.Id = lrFBMRoleSequence.Role_Id
                                          Order By lrFBMRoleSequence.SequenceNr
                                          Select lrFBMRole
                    lrReadingOrder.RoleSequence.Add(New NORMA.Model.Fact.FactReadingOrder.Role() With {.Ref = "_" & lrFBMSequence.Id})
                Next
                ' END: get and add role sequence

                Dim lrReading As New NORMA.Model.Fact.FactReadingOrder.Reading()
                lrReading.ExpandedData = New NORMA.Model.Fact.FactReadingOrder.ExpandedData()
                lrReading.ExpandedData.RoleText = New NORMA.Model.Fact.FactReadingOrder.RoleText() {}
                lrReadingOrder.Readings.Add(lrReading)

                ' trim the front reading text and add a space manually
                Dim lsPredicate As String = lrFBMReadings.FrontReadingText.Trim()
                lrReading.ExpandedData.FrontText = lsPredicate

                ' START: generate reading for PredicateParts
                Dim liInd As Integer = 1
                For Each lrFBMPredicatePart In lrFBMReadings.PredicateParts

                    Dim lsFollowingTextExtension As String = ""
                    If liInd < lrFBMReadings.PredicateParts.Count Then
                        lsFollowingTextExtension = lrFBMReadings.PredicateParts(liInd).PreboundReadingText
                    Else
                        If Not String.IsNullOrWhiteSpace(lrFBMPredicatePart.PostboundReadingText) Then
                            lsFollowingTextExtension = lrFBMPredicatePart.PostboundReadingText
                        Else
                            lsFollowingTextExtension = lrFBMReadings.FollowingReadingText
                        End If
                    End If

                    Dim lrRoleText As New NORMA.Model.Fact.FactReadingOrder.RoleText
                    lrRoleText.RoleIndex = lrFBMPredicatePart.SequenceNr - 1
                    ' check if the last element of predicate
                    If liInd = lrFBMReadings.PredicateParts.Count Then
                        ' trim white space from predicate text
                        lrRoleText.FollowingText = Trim(lrFBMPredicatePart.PredicatePartText.TrimStart() & " " & lsFollowingTextExtension)
                    Else
                        ' trim white space from predicate text and add a space manually
                        lrRoleText.FollowingText = " " & Trim(lrFBMPredicatePart.PredicatePartText.TrimStart() & " " & lsFollowingTextExtension)
                    End If

                    lrReading.ExpandedData.RoleText.Add(lrRoleText)

                    '20220730-VM-Fix this. 
                    '20221102-HA-Improved this
                    If liInd = lrFBMReadings.PredicateParts.Count AndAlso
                        String.IsNullOrEmpty(lrRoleText.FollowingText) Then
                        lsPredicate &= " {" & liInd - 1 & "}"
                    Else
                        lsPredicate &= "{" & liInd - 1 & "}" & lrRoleText.FollowingText
                    End If

                    liInd += 1
                Next
                lrReading.Data = lsPredicate
                ' END: generate reading for PredicateParts

                If IsNothing(arNORMAFactType.ReadingOrders) Then
                    arNORMAFactType.ReadingOrders = New List(Of NORMA.Model.Fact.FactReadingOrder)
                End If
                arNORMAFactType.ReadingOrders.Add(lrReadingOrder)
            Next
#End Region 'Fact Type Readings


        End Sub

        Private Function ValueTypeIsReferenceModeValueType(ByVal asXMLValueTypeId As String) As Boolean

            Try
                Dim larValueType = From EntityType In Me.ORMModel.EntityTypes
                                   Where EntityType.ReferenceModeValueTypeId IsNot Nothing
                                   Where EntityType.ReferenceModeValueTypeId = asXMLValueTypeId
                                   Select EntityType.ReferenceModeValueTypeId

                Return larValueType.Count > 0

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function


    End Class

End Namespace
