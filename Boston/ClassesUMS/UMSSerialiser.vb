Imports System.IO
Imports System.Reflection
Imports AIMLbot.AIMLTagHandlers
Imports Boston.FBM
Imports Boston.RDS
Imports de.jollyday.config
Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

Namespace UMS
    ' ─── Serialiser Factory ───────────────────────────────────────────────────────

    Public Class UmsSchemaSerializer

        Dim Model As FBM.Model


        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model)
            Me.Model = arModel
        End Sub

        Public Function GenerateUMS(Optional ByRef arModelElement As FBM.ModelObject = Nothing) As String


            Try

                Dim lrTypeDefinition As UMS.TypeDefinition 'EntityTypes, UnifiedFactTypes. I.e. E.g. Node Types (graph), Entities (relational).

                Dim lrUMSModel As New UMS.Model

                lrUMSModel.Name = Me.Model.Name

                'Set up the Schema Object/List of UMS.TypeDefinition
                Dim larUnifiedModellingSchema As New List(Of UMS.TypeDefinition)

                If arModelElement Is Nothing Then

                    For Each lrTable In Me.Model.RDS.Table

                        lrTypeDefinition = New UMS.TypeDefinition
                        lrTypeDefinition.Name = lrTable.Name

                        Dim lrModelElement As FBM.ModelObject = lrTable.FBMModelElement

#Region "PrimaryKey"
                        Dim lrPrimaryKey = lrTable.Index.Find(Function(x) x.IsPrimaryKey)

                        If lrPrimaryKey IsNot Nothing Then

                            If lrTypeDefinition.PrimaryKey Is Nothing Then lrTypeDefinition.PrimaryKey = New List(Of String)

                            For Each lrColumn In lrPrimaryKey.Column
                                lrTypeDefinition.PrimaryKey.Add(lrColumn.Name)
                            Next
                        End If
#End Region

                        'Graph Label/s
#Region "Label/s"
                        For Each lrGraphLabel In lrModelElement.GraphLabel

                            If lrTable.isPGSRelation Then
                                lrTypeDefinition.Label = lrGraphLabel.Label

                                Dim lrFactType As FBM.FactType = lrModelElement
                                lrTypeDefinition.Source = lrFactType.Source
                                lrTypeDefinition.Target = lrFactType.Target

                                Exit For
                            End If

                            If lrTypeDefinition.Labels Is Nothing Then lrTypeDefinition.Labels = New List(Of String)

                            lrTypeDefinition.Labels.Add(lrGraphLabel.Label)

                        Next

                        If lrTypeDefinition.Labels Is Nothing And lrTypeDefinition.Label Is Nothing Then
                            'No GraphLabels, and not a PGS Relation, but still requires a Label
                            lrTypeDefinition.Labels = New List(Of String)
                            lrTypeDefinition.Labels.AddUnique(lrTable.Name)
                        End If
#End Region

#Region "RelationshipAnnotation"
                        If lrModelElement.GetType = GetType(FBM.FactType) Then

                            If Not lrTable.isPGSRelation Then
                                lrTypeDefinition.RelationshipAnnotation = Nothing
                            End If

                            Dim lrFactType As FBM.FactType = lrModelElement
                            Dim liInd = 0

                            If lrTypeDefinition.FactTypeReadings Is Nothing And lrFactType.FactTypeReading.Count > 0 Then

                                lrTypeDefinition.FactTypeReadings = New List(Of FactTypeReadings)

                                Dim lrFactTypeReadings = New UMS.FactTypeReadings("Not Defined")

                                For Each lrFactTypeReading In lrFactType.FactTypeReading

                                    lrFactTypeReadings.Readings.Add(lrFactTypeReading.GetReadingText)

                                    If liInd = 0 And lrTable.isPGSRelation Then
                                        lrTypeDefinition.RelationshipAnnotation = lrFactTypeReading.GetReadingText
                                    End If

                                    liInd += 1
                                Next

                                lrTypeDefinition.FactTypeReadings.Add(lrFactTypeReadings)
                            End If

                            '==========================================================================================================================
                            'Facts
#Region "Facts"
                            Dim lrUMSFacts As UMS.Facts

                            If lrFactType.Fact.Count > 0 Then
                                lrUMSFacts = New UMS.Facts("Not Defined")

                                For Each lrFact In lrFactType.Fact

                                    Dim lsFactVerbalisation As String = ""

                                    Dim larRole = (From FactData In lrFact.Data
                                                   Select FactData.Role).ToList

                                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.getFactTypeReadingByRoleSequence(larRole)

                                    If lrFactTypeReading IsNot Nothing Then

                                        liInd = 0
                                        For Each lrFactData In lrFact.Data

                                            lsFactVerbalisation.AppendString(lrFactData.Role.JoinedORMObject.Id)
                                            lsFactVerbalisation.AppendString(",")
                                            If lrFactData.Role.JoinsValueType IsNot Nothing Then
                                                lsFactVerbalisation.AppendString(lrFactData.Data)
                                            End If
                                            lsFactVerbalisation.AppendString(",")
                                            lsFactVerbalisation.AppendString(" " & lrFactTypeReading.PredicatePart(liInd).PredicatePartText)

                                            liInd += 1
                                        Next

                                        lsFactVerbalisation.AppendString(".")

                                        lrUMSFacts.Facts.Add(lsFactVerbalisation)

                                    End If

                                    If lrUMSFacts.Facts.Count > 0 Then

                                        If lrTypeDefinition.Facts Is Nothing Then lrTypeDefinition.Facts = New List(Of Facts)

                                        lrTypeDefinition.Facts.Add(lrUMSFacts)
                                    End If
                                Next
                            End If
#End Region


                        End If
#End Region


                        '==========================================================================================================================
                        'Properties
                        '------------
#Region "Properties"
                        For Each lrColumn In lrTable.Column

                            If lrTypeDefinition.Properties Is Nothing Then lrTypeDefinition.Properties = New List(Of UMS.PropertyDefinition)

                            Dim lrProperty = New UMS.PropertyDefinition

                            'Property.Name. Use lrColumn.DBName as is what is the Column/Attrribute/Property Name in the database.
                            lrProperty.Name = lrColumn.DBName

                            'Fact-Based Modelling Name. Only if is not the same as the lrColumn.DBName
                            If lrColumn.ActiveRole IsNot Nothing AndAlso lrColumn.ActiveRole.JoinsValueType IsNot Nothing Then
                                If lrProperty.Name <> lrColumn.ActiveRole.JoinsValueType.Id Then
                                    lrProperty.FactBasedName = lrColumn.ActiveRole.JoinsValueType.Id
                                End If
                            End If

                            If lrColumn.IsMandatory Then
                                If lrProperty.Constraints Is Nothing Then lrProperty.Constraints = New List(Of UMS.Constraint)
                                lrProperty.Constraints.Add(UMS.Constraint.NotNull)
                            End If

                            If lrColumn.ActiveRole.JoinsValueType IsNot Nothing Then

                                Select Case lrColumn.ActiveRole.JoinsValueType.DataType
                                    Case Is = pcenumORMDataType.NumericAutoCounter
                                        lrProperty.DataType = UMS.DataType.Integer
                                    Case Else
                                        lrProperty.DataType = UMS.DataType.TextVariableLength
                                End Select

                                If lrColumn.ActiveRole.JoinsValueType.DataTypeUsesLength Then
                                    lrProperty.Length = lrColumn.ActiveRole.JoinsValueType.DataTypeLength
                                End If
                            End If

                            '----------------------------------------------
                            'Unique Index
                            Dim larIndex = lrTable.Index.FindAll(Function(x) Not x.IsPrimaryKey)
                            Dim larSingleColumnIndex = larIndex.FindAll(Function(x) x.Column.Count = 1)
                            Dim lbIsSingleColumnUniqueIndexColumn As Boolean = larSingleColumnIndex.FindAll(Function(x) x.Column(0).Id = lrColumn.Id).Count > 0

                            If lbIsSingleColumnUniqueIndexColumn Then

                                If lrProperty.Constraints Is Nothing Then lrProperty.Constraints = New List(Of UMS.Constraint)

                                lrProperty.Constraints.Add(UMS.Constraint.Unique)
                            End If

                            lrTypeDefinition.Properties.Add(lrProperty)

                            'Fact Type Readings
#Region "Fact Type Readings"
                            Select Case lrTable.FBMModelElement.GetType
                                Case Is = GetType(FBM.FactType)
                                Case Else
                                    'Do checking on FactType of the Column
                                    If lrColumn.FactType.Id = lrTable.FBMModelElement.Id Then
                                        Exit Select
                                    ElseIf lrColumn.FactType.Arity <> 2 Then
                                        Exit Select
                                    End If

                                    If lrProperty.FactTypeReadings Is Nothing Then lrProperty.FactTypeReadings = New List(Of FactTypeReadings)

                                    'Get the FactTypeReadings of the FactType for the Column

                                    Dim lrFactTypeReadings As New FactTypeReadings("Not Defined")

                                    For Each lrFactTypeReading In lrColumn.FactType.FactTypeReading

                                        If lrFactTypeReadings.Readings Is Nothing Then lrFactTypeReadings.Readings = New List(Of String)

                                        lrFactTypeReadings.Readings.Add(lrFactTypeReading.GetReadingText)
                                    Next

                                    lrProperty.FactTypeReadings.Add(lrFactTypeReadings)
                            End Select
#End Region

                            '==========================================================================================================================
                            'Facts - Property Facts
#Region "Facts - Property Facts"
                            Dim lrUMSFacts As UMS.Facts

                            If (lrColumn.FactType.IsManyTo1BinaryFactType And Not lrColumn.FactType.isRDSTable) And lrColumn.FactType.Fact.Count > 0 Then

                                lrUMSFacts = New UMS.Facts("Not Defined")

                                For Each lrFact In lrColumn.FactType.Fact

                                    Dim lsFactVerbalisation As String = ""

                                    lsFactVerbalisation = lrFact.GetReading

                                    lrUMSFacts.Facts.Add(lsFactVerbalisation)

                                    'Dim larRole = (From FactData In lrFact.Data
                                    '               Select FactData.Role).ToList

                                    'Dim lrFactTypeReading As FBM.FactTypeReading = lrColumn.FactType.getFactTypeReadingByRoleSequence(larRole)

                                    'If lrFactTypeReading IsNot Nothing Then

                                    '    Dim liInd = 0
                                    '    For Each lrFactData In lrFact.Data

                                    '        lsFactVerbalisation.AppendString(lrFactData.Role.JoinedORMObject.Id)
                                    '        lsFactVerbalisation.AppendString(",")
                                    '        If lrFactData.Role.JoinsValueType IsNot Nothing Then
                                    '            lsFactVerbalisation.AppendString(If(lrColumn.DataTypeIsNumeric, "", "'"))
                                    '            lsFactVerbalisation.AppendString(lrFactData.Data)
                                    '            lsFactVerbalisation.AppendString(If(lrColumn.DataTypeIsNumeric, "", "'"))
                                    '        Else
                                    '            lsFactVerbalisation.AppendString("{")
                                    '            lsFactVerbalisation.AppendString("}")
                                    '        End If
                                    '        lsFactVerbalisation.AppendString(",")
                                    '        lsFactVerbalisation.AppendString(" " & lrFactTypeReading.PredicatePart(liInd).PredicatePartText)

                                    '        liInd += 1
                                    '    Next

                                    '    lsFactVerbalisation.AppendString(".")

                                    '    lrUMSFacts.Facts.Add(lsFactVerbalisation)

                                    'End If

                                    If lrUMSFacts.Facts.Count > 0 Then

                                        If lrProperty.Facts Is Nothing Then lrProperty.Facts = New List(Of Facts)

                                        lrProperty.Facts.Add(lrUMSFacts)
                                    End If
                                Next
                            End If
#End Region 'Facts

                        Next 'Column
#End Region


#Region "Uniqueness Constraints"
                        For Each lrIndex In lrTable.Index.FindAll(Function(x) Not x.IsPrimaryKey And x.Column.Count > 1)

                            Dim lrUniquenessConstraint As New UMS.UniquenessConstraint

                            If lrTypeDefinition.UniquenessConstraints Is Nothing Then lrTypeDefinition.UniquenessConstraints = New List(Of UniquenessConstraint)

                            lrUniquenessConstraint.Name = lrIndex.Name

                            For Each lrColumn In lrIndex.Column
                                lrUniquenessConstraint.Properties.Add(lrColumn.Name)
                            Next

                            lrTypeDefinition.UniquenessConstraints.Add(lrUniquenessConstraint)

                        Next
#End Region

#Region "Foreign Key Relationships"

                        For Each lrRDSRelation In lrTable.ForeignKeyRelationship

                            Dim lrRelationship As New UMS.RelationshipDefinition

                            If lrTypeDefinition.Relationships Is Nothing Then lrTypeDefinition.Relationships = New List(Of RelationshipDefinition)

                            lrRelationship.Name = lrRDSRelation.ResponsibleFactType.Id
                            If lrRDSRelation.ResponsibleFactType.GraphLabel.Count > 0 Then
                                lrRelationship.Name = lrRDSRelation.ResponsibleFactType.GraphLabel(0).Label
                            End If

                            lrRelationship.Source = lrRDSRelation.OriginTable.Name
                            lrRelationship.Target = lrRDSRelation.DestinationTable.Name

                            For Each lrColumn In lrRDSRelation.OriginColumns
                                lrRelationship.From.Add(lrColumn.Name)
                            Next

                            For Each lrColumn In lrRDSRelation.DestinationColumns
                                lrRelationship.To.Add(lrColumn.Name)
                            Next

                            For Each lrFactTypeReading In lrRDSRelation.ResponsibleFactType.FactTypeReading
                                lrRelationship.Readings.Add(lrFactTypeReading.GetReadingText)
                            Next

                            lrTypeDefinition.Relationships.Add(lrRelationship)

                        Next

#End Region


                        larUnifiedModellingSchema.Add(lrTypeDefinition)

                    Next

                Else

                End If

                'Add the range of ModelElements to the UMS Model.
                lrUMSModel.ModelElement.AddRange(larUnifiedModellingSchema)

                Dim serializer = New SerializerBuilder().Build()

                Return serializer.Serialize(lrUMSModel)

                'Return UmsSchemaSerializer.SaveToString(larUnifiedModellingSchema)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
            End Try

        End Function

        Public Shared Function BuildDeserializer() As IDeserializer
            Return New DeserializerBuilder() _
            .WithNamingConvention(PascalCaseNamingConvention.Instance) _
            .WithTypeConverter(New DataTypeConverter()) _
            .WithTypeConverter(New ConstraintConverter()) _
            .IgnoreUnmatchedProperties() _
            .Build()
        End Function

        Public Shared Function BuildSerializer() As ISerializer
            Return New SerializerBuilder() _
            .WithNamingConvention(PascalCaseNamingConvention.Instance) _
            .WithTypeConverter(New DataTypeConverter()) _
            .WithTypeConverter(New ConstraintConverter()) _
            .ConfigureDefaultValuesHandling(
                DefaultValuesHandling.OmitNull Or
                DefaultValuesHandling.OmitEmptyCollections) _
            .Build()
        End Function

        ''' <summary>
        ''' Load a UMS schema from a YAML file.
        ''' </summary>
        Public Shared Function LoadFromFile(filePath As String) As List(Of TypeDefinition)
            Dim yaml As String = File.ReadAllText(filePath)
            Return LoadFromString(yaml)
        End Function

        ''' <summary>
        ''' Load a UMS schema from a YAML string.
        ''' </summary>
        Public Shared Function LoadFromString(yaml As String) As List(Of TypeDefinition)
            Return BuildDeserializer().Deserialize(Of List(Of TypeDefinition))(yaml)
        End Function

        ''' <summary>
        ''' Serialise a UMS schema to a YAML string.
        ''' </summary>
        Public Shared Function SaveToString(schema As List(Of TypeDefinition)) As String
            Return BuildSerializer().Serialize(schema)
        End Function

        ''' <summary>
        ''' Serialise a UMS schema to a YAML file.
        ''' </summary>
        Public Shared Sub SaveToFile(schema As List(Of TypeDefinition), filePath As String)
            File.WriteAllText(filePath, SaveToString(schema))
        End Sub

    End Class

End Namespace