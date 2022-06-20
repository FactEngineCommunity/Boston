Imports Viev.FBM.Interface

Public Class FBMInterfaceHost
    Inherits Viev.FBM.Interface.FBMInterface

    Public Sub New()

        Boston.WriteToStatusBar("Initialising Interface Host", True)

    End Sub

    'Public Overrides Sub ChangeWorkingModel(asModelId As String)
    '    MyBase.ChangeWorkingModel(asModelId)

    '    MsgBox("From inside Boston: Changing Working Model to: " & asModelId & "(from the Plugin)")

    '    Dim lrModel As FBM.Model
    '    lrModel = prApplication.Models.Find(Function(x) x.ModelId = asModelId)

    '    If lrModel IsNot Nothing Then
    '        MsgBox(lrModel.Name)
    '        If Not lrModel.Loaded Then
    '            Call lrModel.Load()
    '        End If
    '    End If

    'End Sub

    Public Shadows Sub ShowFeedback(strFeedback As String)

        MsgBox(strFeedback)

    End Sub

    Public Overrides Sub BostonCreateValueType(arModel As Viev.FBM.Interface.Model)

        'Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = arModel.ModelId)
        'Dim lrValueType As New FBM.ValueType
        'Dim lrXMLValueType As [Interface].ValueType = arModel.ValueType(0)

        'lrValueType.Model = lrModel
        'lrValueType.Id = lrXMLValueType.Id
        'lrValueType.Name = lrXMLValueType.Name
        'lrValueType.DataType = lrXMLValueType.DataType
        'lrValueType.DataTypePrecision = lrXMLValueType.DataTypePrecision
        'lrValueType.DataTypeLength = lrXMLValueType.DataTypeLength

        'For Each lsValueTypeConstraintValue In lrXMLValueType.ValueConstraint
        '    lrValueType.ValueConstraint.Add(lsValueTypeConstraintValue)
        '    lrValueType.Instance.Add(lsValueTypeConstraintValue)
        'Next

        ''------------------------------------------------
        ''Link to the Concept within the ModelDictionary
        ''------------------------------------------------
        'Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrValueType.Id, pcenumConceptType.ValueType, lrValueType.ShortDescription, lrValueType.LongDescription)
        'lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry)

        'lrValueType.Concept = lrDictionaryEntry.Concept

        'lrModel.ValueType.Add(lrValueType)
        'Call lrModel.MakeDirty(True, False)

    End Sub

    Public Overrides Sub BostonCreateEntityType(armodel As [Interface].Model)

        'Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = armodel.ModelId)
        'Dim lrEntityType As New FBM.EntityType
        'Dim lrXMLEntityType As [Interface].EntityType = armodel.EntityType(0)

        'lrEntityType.Model = lrModel
        'lrEntityType.Id = lrXMLEntityType.Id
        'lrEntityType.GUID = lrXMLEntityType.GUID
        'lrEntityType.Name = lrXMLEntityType.Name
        'lrEntityType.ReferenceMode = lrXMLEntityType.ReferenceMode
        ''lrEntityType.ShortDescription = 
        ''lrEntityType.LongDescription = 
        'lrEntityType.IsObjectifyingEntityType = lrXMLEntityType.IsObjectifyingEntityType

        'If lrXMLEntityType.ReferenceModeValueTypeId = "" Then
        '    lrEntityType.ReferenceModeValueType = Nothing
        'Else
        '    lrEntityType.ReferenceModeValueType = New FBM.ValueType
        '    lrEntityType.ReferenceModeValueType.Id = lrXMLEntityType.ReferenceModeValueTypeId
        '    lrEntityType.ReferenceModeValueType = lrModel.ValueType.Find(AddressOf lrEntityType.ReferenceModeValueType.Equals)
        'End If

        'lrEntityType.PreferredIdentifierRCId = lrXMLEntityType.ReferenceSchemeRoleConstraintId

        ''------------------------------------------------
        ''Link to the Concept within the ModelDictionary
        ''------------------------------------------------
        'Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrEntityType.Name, pcenumConceptType.EntityType)
        'lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , False)

        'lrEntityType.Concept = lrDictionaryEntry.Concept

        'lrModel.EntityType.Add(lrEntityType)

    End Sub

    Public Overrides Sub BostonCreateFactType(arModel As [Interface].Model)

        'Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = arModel.ModelId)
        'Dim lrFactType As New FBM.FactType
        'Dim lrXMLFactType As [Interface].FactType = arModel.FactType(0)

        'lrFactType = New FBM.FactType(lrModel, _
        '                              lrXMLFactType.Name, _
        '                              lrXMLFactType.Id)

        ''===============================================================================================================
        'Dim lsMessage As String = ""
        'Dim lrXMLRole As [Interface].Role
        'Dim lrRole As FBM.Role


        ''lrFactType.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
        ''lrFactType.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
        'lrFactType.IsObjectified = lrXMLFactType.IsObjectified
        ''lrFactType.IsCoreFactType = CBool(lREcordset("IsCoreFactType").Value)
        'lrFactType.IsPreferredReferenceMode = lrXMLFactType.IsPreferredReferenceSchemeFT
        'lrFactType.IsSubtypeRelationshipFactType = lrXMLFactType.IsSubtypeRelationshipFactType
        'lrFactType.IsDerived = lrXMLFactType.IsDerived
        'lrFactType.IsStored = lrXMLFactType.IsStored
        'lrFactType.DerivationText = lrXMLFactType.DerivationText

        'If lrXMLFactType.ObjectifyingEntityTypeId = "" Then
        '    lrFactType.ObjectifyingEntityType = Nothing
        'Else
        '    Dim lsEntityTypeId As String = ""
        '    lsEntityTypeId = lrXMLFactType.ObjectifyingEntityTypeId
        '    lrFactType.ObjectifyingEntityType = New FBM.EntityType(lrFactType.Model, pcenumLanguage.ORMModel, lsEntityTypeId, Nothing, True)
        '    lrFactType.ObjectifyingEntityType = lrFactType.Model.EntityType.Find(AddressOf lrFactType.ObjectifyingEntityType.Equals)
        '    lrFactType.ObjectifyingEntityType.IsObjectifyingEntityType = True
        '    lrFactType.ObjectifyingEntityType.ObjectifiedFactType = New FBM.FactType
        '    lrFactType.ObjectifyingEntityType.ObjectifiedFactType = lrFactType

        '    If IsSomething(lrFactType.ObjectifyingEntityType) Then
        '        '---------------------------------------------
        '        'Okay, have found the ObjectifyingEntityType
        '        '---------------------------------------------
        '    Else
        '        lsMessage = "No EntityType found in the Model for Objectifying Entity Type of the FactType"
        '        lsMessage &= vbCrLf & "ModelId: " & lrFactType.Model.ModelId
        '        lsMessage &= vbCrLf & "FactTypeId: " & lrFactType.Id
        '        lsMessage &= vbCrLf & "Looking for EntityTypeId: " & lsEntityTypeId
        '        Throw New Exception(lsMessage)
        '    End If
        'End If

        ''-----------------------------------------------------
        ''Get the Roles within the RoleGroup for the FactType
        ''-----------------------------------------------------
        'For Each lrXMLRole In lrXMLFactType.RoleGroup

        '    lrRole = New FBM.Role
        '    lrRole.Model = lrFactType.Model
        '    lrRole.FactType = lrFactType
        '    lrRole.Id = lrXMLRole.Id
        '    lrRole.Name = lrXMLRole.Name
        '    lrRole.SequenceNr = lrXMLRole.SequenceNr
        '    lrRole.Mandatory = lrXMLRole.Mandatory
        '    lrRole.JoinedORMObject = New FBM.ModelObject
        '    lrRole.JoinedORMObject.Id = lrXMLRole.JoinedObjectTypeId

        '    If IsSomething(lrFactType.Model.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)) Then
        '        lrRole.JoinsEntityType = New FBM.EntityType
        '        lrRole.JoinsEntityType = lrFactType.Model.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
        '        lrRole.JoinedORMObject = lrRole.JoinsEntityType
        '        lrRole.TypeOfJoin = pcenumRoleJoinType.EntityType
        '    ElseIf IsSomething(lrFactType.Model.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)) Then
        '        lrRole.JoinsValueType = New FBM.ValueType
        '        lrRole.JoinsValueType = lrFactType.Model.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
        '        lrRole.JoinedORMObject = lrRole.JoinsValueType
        '        lrRole.TypeOfJoin = pcenumRoleJoinType.ValueType
        '    Else
        '        lrRole.JoinsFactType = New FBM.FactType
        '        lrRole.JoinsFactType = lrFactType.Model.FactType.Find(AddressOf lrRole.JoinedORMObject.Equals)
        '        If lrRole.JoinsFactType Is Nothing Then
        '            Throw New Exception("Error: No FactType found for FactTypeId: " & lrXMLRole.JoinedObjectTypeId)
        '        End If
        '        lrRole.JoinedORMObject = lrRole.JoinsFactType
        '        lrRole.TypeOfJoin = pcenumRoleJoinType.FactType
        '    End If

        '    '--------------------------------------------------
        '    'Add the Role to the Model (list of Role) as well
        '    '--------------------------------------------------
        '    lrFactType.Model.Role.Add(lrRole)

        '    lrFactType.RoleGroup.Add(lrRole)
        'Next

        'lrFactType.Arity = lrFactType.RoleGroup.Count


        ''===============================================================================================================

        ''------------------------------------------------
        ''Link to the Concept within the ModelDictionary
        ''------------------------------------------------
        'Dim lrDictionaryEntry As New FBM.DictionaryEntry(lrModel, lrFactType.Id, pcenumConceptType.FactType)
        'lrDictionaryEntry = lrModel.AddModelDictionaryEntry(lrDictionaryEntry, , False)

        'lrFactType.Concept = lrDictionaryEntry.Concept

        'If lrModel.FactType.Find(AddressOf lrFactType.Equals) Is Nothing Then
        '    lrModel.FactType.Add(lrFactType)
        'End If

    End Sub

    Public Overrides Function BostonGetModelIdByModelName(asModelName As String, ByRef asModelId As String) As Boolean

        Dim lrModel As FBM.Model

        lrModel = prApplication.Models.Find(Function(x) x.Name = asModelName)
        If lrModel IsNot Nothing Then
            asModelId = lrModel.ModelId
            Return True
        Else
            Return False
        End If

    End Function

End Class
