Imports System.Text.RegularExpressions
Imports System.Xml.Serialization
Imports System.Xml.Linq
Imports System.Reflection
Imports Humanizer
Imports <xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore">
Imports <xmlns:ormDiagram="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram">

Namespace NORMA

    Public Class NORMAXMLFileLoader

        Private FBMModel As FBM.Model = Nothing

        'FactTypeReading Processing
        Private FTRScanner As FTR.Scanner
        Private FTRProcessor As New FTR.Processor 'Used for parsing FTR texts as input by the user. 
        Private FTRParser As New FTR.Parser(New FTR.Scanner)
        Private FTRParseTree As New FTR.ParseTree

        ''' <summary>
        ''' Key is NORMA reference, Value is data type name.
        ''' </summary>
        Private DataTypeDictionary As New Dictionary(Of String, String) 'Key is NORMA reference, Value is data type name.

        ''' <summary>
        ''' Parameterless constructor.
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model)
            Me.FBMModel = arModel
        End Sub

        Public Function getBostonDataTypeByNORMADataType(ByVal asNORMADataType As String) As pcenumORMDataType

            Try
                asNORMADataType = Trim(asNORMADataType)

                Dim larDBDataType = From DatabaseDataType In Me.FBMModel.RDS.DatabaseDataType
                                    Where UCase(DatabaseDataType.DataType) = UCase(asNORMADataType)
                                    Where "NORMA" = DatabaseDataType.Database.ToString
                                    Select DatabaseDataType.BostonDataType

                If larDBDataType.Count > 0 Then
                    Return larDBDataType.First
                Else
                    Return pcenumORMDataType.TextVariableLength
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return pcenumORMDataType.TextVariableLength
            End Try

        End Function

        Public Sub getNORMADataTypes(ByRef arModel As FBM.Model)

            Try
                Dim lsPath = Boston.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
                Dim reader As System.IO.TextReader = New System.IO.StreamReader(lsPath)

                Dim csvReader = New CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture)
                arModel.RDS.DatabaseDataType = csvReader.GetRecords(Of DatabaseDataType).ToList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function getModelErrorCount(ByRef arNORMAXMLDOC As XDocument) As Integer

            Try
                Dim loEnumElementQueryResult = From ModelError In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:ModelErrors>
                                               Select ModelError

                Return loEnumElementQueryResult.Count

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                Return 0
            End Try

        End Function

        Public Sub SetSimpleReferenceSchemes(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Try

                Dim lrEntityType As New FBM.EntityType
                Dim loEnumElementQueryResult As IEnumerable(Of XElement)
                Dim loEnumElementQueryResult2 As IEnumerable(Of XElement)
                Dim loElement As XElement

                Dim lrModel As FBM.Model = arModel

                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:EntityType>
                                           Select ModelInformation
                                           Order By ModelInformation.Attribute("Name").Value

                '---------------------------------
                'Step through the EntityTypes
                '---------------------------------
                For Each loElement In loEnumElementQueryResult
                    lrEntityType = arModel.EntityType.Find(Function(x) x.Id = loElement.Attribute("Name").Value)

                    'Simple Reference Scheme
                    Dim lsReferenceMode As String = loElement.Attribute("_ReferenceMode").Value

                    loEnumElementQueryResult2 = From PreferredIdentifier In loElement.<orm:PreferredIdentifier>
                                                Select PreferredIdentifier

                    If loEnumElementQueryResult2.Count > 0 Then
                        Dim lsPreferredRoleConstraintRoleId As String = loEnumElementQueryResult2.First.Attribute("ref").Value

                        Dim larRoleConstraint = From RoleConstraint In lrModel.RoleConstraint
                                                Where RoleConstraint.NORMAReferenceId = lsPreferredRoleConstraintRoleId
                                                Select RoleConstraint

                        If larRoleConstraint.Count > 0 Then
                            lrEntityType.ReferenceModeRoleConstraint = larRoleConstraint.First
                            lrEntityType.ReferenceModeRoleConstraint.SetIsPreferredIdentifier(True)
                        End If
                    End If

                    If lsReferenceMode <> "" Then
                        If arModel.GetModelObjectByName(lrEntityType.Id & "_" & lsReferenceMode) IsNot Nothing Then
                            lrEntityType.SetReferenceMode("." & lsReferenceMode, True, Nothing, False,, True)
                            lrEntityType.ReferenceModeValueType = arModel.ValueType.Find(Function(x) x.Id = lrEntityType.Id & "_" & lsReferenceMode)
                        Else
                            lrEntityType.SetReferenceMode(lsReferenceMode, True, Nothing, False,, True)
                            lrEntityType.ReferenceModeValueType = arModel.ValueType.Find(Function(x) x.Id = lsReferenceMode)
                            If lrEntityType.ReferenceModeValueType Is Nothing Then
                                If lrEntityType.ReferenceModeRoleConstraint IsNot Nothing Then
                                    lrEntityType.ReferenceModeValueType = lrEntityType.ReferenceModeRoleConstraint.Role(0).JoinsValueType
                                End If
                            End If
                        End If
                    End If

                    If lrEntityType.ReferenceModeRoleConstraint IsNot Nothing Then
                        lrEntityType.ReferenceModeFactType = lrEntityType.ReferenceModeRoleConstraint.Role(0).FactType
                        lrEntityType.ReferenceModeFactType.IsPreferredReferenceMode = True
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

        Public Sub SetSimpleReferenceSchemesObjectifyingEntityTypes(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lsMessage As String
            Try

                Dim lrEntityType As New FBM.EntityType
                Dim loEnumElementQueryResult As IEnumerable(Of XElement)
                Dim loEnumElementQueryResult2 As IEnumerable(Of XElement)
                Dim loElement As XElement

                Dim lrModel As FBM.Model = arModel

                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ObjectifiedType>
                                           Select ModelInformation
                                           Where ModelInformation.Attribute("_ReferenceMode").Value <> ""
                                           Order By ModelInformation.Attribute("Name").Value

                '---------------------------------
                'Step through the EntityTypes
                '---------------------------------
                Dim lsReferenceMode As String
                Dim lrFactType As FBM.FactType = Nothing
                For Each loElement In loEnumElementQueryResult
                    'Simple Reference Scheme
                    lsReferenceMode = loElement.Attribute("_ReferenceMode").Value

                    '20220210-Can probably dump this IFThen...see XML LINQ query above.
                    If Trim(lsReferenceMode) <> "" Then

                        lrEntityType = arModel.EntityType.Find(Function(x) x.NORMAReferenceId = loElement.Attribute("id").Value)
                        If lrEntityType Is Nothing Then
                            lrFactType = arModel.FactType.Find(Function(x) x.Id = loElement.Attribute("Name").Value)
                            lrEntityType = lrFactType.ObjectifyingEntityType
                        End If

                        loEnumElementQueryResult2 = From PreferredIdentifier In loElement.<orm:PreferredIdentifier>
                                                    Select PreferredIdentifier

                        If loEnumElementQueryResult2.Count > 0 Then
                            Dim lsPreferredRoleConstraintRoleId As String = loEnumElementQueryResult2.First.Attribute("ref").Value

                            Dim larRoleConstraint = From RoleConstraint In lrModel.RoleConstraint
                                                    Where RoleConstraint.NORMAReferenceId = lsPreferredRoleConstraintRoleId
                                                    Select RoleConstraint

                            If larRoleConstraint.Count > 0 Then
                                Try
                                    lrEntityType.ReferenceModeRoleConstraint = larRoleConstraint.First
                                    lrEntityType.ReferenceModeRoleConstraint.SetIsPreferredIdentifier(True)
                                Catch ex As Exception
                                    lsMessage = "Problem setting Simple Reference Scheme fo Objectifying Entity Type."
                                    Try
                                        lsMessage.AppendDoubleLineBreak("Entity Type: " & lrEntityType.Id)
                                        lsMessage.AppendLine("Fact Type: " & lrFactType.Id)
                                    Catch
                                        'Not a biggie.
                                    End Try
                                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False, False, True)
                                End Try
                            End If
                        End If

                        lrEntityType.ReferenceMode = lsReferenceMode

                        lrEntityType.ReferenceModeValueType = lrEntityType.ReferenceModeRoleConstraint.Role(0).JoinsValueType
                        lrEntityType.ObjectifiedFactType.InternalUniquenessConstraint(0).IsPreferredIdentifier = False

                        If lrEntityType.ReferenceModeRoleConstraint IsNot Nothing Then
                            lrEntityType.ReferenceModeFactType = lrEntityType.ReferenceModeRoleConstraint.Role(0).FactType
                            lrEntityType.ReferenceModeFactType.IsPreferredReferenceMode = True
                        End If


                    End If
                Next

            Catch ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub LoadDataTypes(ByRef arNORMAXMLDOC As XDocument)

            Try
                Dim loEnumElementQueryResult As IEnumerable(Of XElement)
                Dim loElement As XElement

                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:DataTypes>
                                           Select ModelInformation

                For Each loElement In loEnumElementQueryResult.Elements

                    Me.DataTypeDictionary.Add(loElement.Attribute("id").Value, loElement.Name.LocalName)

                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Gets the concept type of an Object in a NORMA .orm file.
        ''' </summary>
        ''' <param name="arNORMAXMLDOC">The NORMA .orm file.</param>
        ''' <param name="asNORMAReferenceId">The NORMA ReferenceId to check for.</param>
        ''' <param name="asUltimateNORMAReferenceId">Either the asNORMAReferenceId or the FactType ReferenceId if asNORMAReferenceId is for an ObjectifyingEntityType.</param>
        ''' <returns></returns>
        Public Function GetTypeOfObjectFromNORMAFile(ByRef arNORMAXMLDOC As XDocument,
                                                     ByVal asNORMAReferenceId As String,
                                                     ByRef asUltimateNORMAReferenceId As String) As pcenumConceptType

            Dim loElement As XElement

            Try
                loElement = (From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.Elements
                             Select ModelInformation
                             Where ModelInformation.Attribute("id").Value = asNORMAReferenceId).First

                Select Case loElement.Name.LocalName
                    Case Is = "EntityType"
                        asUltimateNORMAReferenceId = asNORMAReferenceId
                        Return pcenumConceptType.EntityType
                    Case Is = "ValueType"
                        asUltimateNORMAReferenceId = asNORMAReferenceId
                        Return pcenumConceptType.ValueType
                    Case Is = "ObjectifiedType"
                        asUltimateNORMAReferenceId = loElement.<orm:NestedPredicate>(0).Attribute("ref").Value
                        Return pcenumConceptType.FactType
                    Case Else
                        Return pcenumConceptType.Unknown
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return pcenumConceptType.Unknown
            End Try
        End Function

        Public Function LoadEntityType(ByRef arModel As FBM.Model,
                                       ByRef arNORMAXMLDOC As XDocument,
                                       ByVal asNORMAReferenceId As String) As FBM.EntityType

            Dim lrEntityType As New FBM.EntityType
            Dim loElement As XElement

            Try
                loElement = (From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:EntityType>
                             Select ModelInformation
                             Where ModelInformation.Attribute("id").Value = asNORMAReferenceId).First

                lrEntityType = New FBM.EntityType(arModel, pcenumLanguage.ORMModel, loElement.Attribute("Name").Value, loElement.Attribute("Name").Value, Nothing)
                lrEntityType.NORMAReferenceId = loElement.Attribute("id").Value

                arModel.AddEntityType(lrEntityType, False, False, Nothing, True)

                Dim lsReferenceMode As String = loElement.Attribute("_ReferenceMode").Value

                If lsReferenceMode <> "" Then
                    Dim lsPreferredIdentifierId = loElement.<orm:PreferredIdentifier>.AsEnumerable.First.Attribute("ref").Value

                    Dim larUniquenessConstraintRole = From Constraint In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:UniquenessConstraint>
                                                      From RoleSequence In Constraint.<orm:RoleSequence>
                                                      From Role In RoleSequence.<orm:Role>
                                                      Where Constraint.Attribute("id") = lsPreferredIdentifierId
                                                      Select Role

                    Dim lsRoleId = larUniquenessConstraintRole.First.Attribute("ref").Value

                    Dim larFactType = From FactType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                      From Role In FactType.<orm:FactRoles>.<orm:Role>
                                      Where Role.Attribute("id") = lsRoleId
                                      Select FactType


                    Dim lrFactType As New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                    lrFactType.NORMAReferenceId = larFactType.First.Attribute("id").Value

                    Call Me.LoadFactType(lrFactType, arNORMAXMLDOC)


                End If

                Return lrEntityType

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Sub LoadEntityTypes(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrEntityType As New FBM.EntityType
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement

            Try
                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:EntityType>
                                           Select ModelInformation
                                           Order By ModelInformation.Attribute("Name").Value

                '---------------------------------
                'Add the EntiyTypes to the Model
                '---------------------------------
                For Each loElement In loEnumElementQueryResult
                    lrEntityType = New FBM.EntityType(arModel, pcenumLanguage.ORMModel, loElement.Attribute("Name").Value, loElement.Attribute("Name").Value, Nothing)
                    lrEntityType.NORMAReferenceId = loElement.Attribute("id").Value

                    arModel.AddEntityType(lrEntityType, False, False, Nothing, True)

                    Dim lsReferenceMode As String = loElement.Attribute("_ReferenceMode").Value

                    If lsReferenceMode <> "" Then
                        Dim lsPreferredIdentifierId = loElement.<orm:PreferredIdentifier>.AsEnumerable.First.Attribute("ref").Value

                        Dim larUniquenessConstraintRole = From Constraint In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:UniquenessConstraint>
                                                          From RoleSequence In Constraint.<orm:RoleSequence>
                                                          From Role In RoleSequence.<orm:Role>
                                                          Where Constraint.Attribute("id") = lsPreferredIdentifierId
                                                          Select Role

                        Dim lsRoleId = larUniquenessConstraintRole.First.Attribute("ref").Value

                        Dim larFactType = From FactType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                          From Role In FactType.<orm:FactRoles>.<orm:Role>
                                          Where Role.Attribute("id") = lsRoleId
                                          Select FactType


                        Dim lrFactType As New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                        lrFactType.NORMAReferenceId = larFactType.First.Attribute("id").Value

                        Call Me.LoadFactType(lrFactType, arNORMAXMLDOC)

                        '20220209-VM-Remove if not missed over time.
                        'Dim loXMLFactType As XElement = larFactType.First

                        'Dim lsFirstRoleId As String = (From Role In loXMLFactType.<orm:FactRoles>.<orm:Role>
                        '                               Where Role.Attribute("id").Value <> lsRoleId
                        '                               Select Role).First.Attribute("id").Value

                        'Dim lsFirstIUCId As String = (From Constraint In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:UniquenessConstraint>
                        '                              From RoleSequence In Constraint.<orm:RoleSequence>
                        '                              From Role In RoleSequence.<orm:Role>
                        '                              Where Role.Attribute("ref") = lsFirstRoleId
                        '                              Select Constraint).First.Attribute("id").Value

                        'Call Me.LoadRoleConstraintInternalUniquenessConstraint(arModel, arNORMAXMLDOC, lsFirstIUCId)

                        'Call Me.LoadRoleConstraintInternalUniquenessConstraint(arModel, arNORMAXMLDOC, lsPreferredIdentifierId)

                    End If

                Next

                '-----------------------------------------------------
                'Check for Subtype relationships for each EntityType
                '-----------------------------------------------------
                For Each lrEntityType In arModel.EntityType.ToArray
                    loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:SubtypeFact>
                                               From FactRoles In ModelInformation.<orm:FactRoles>
                                               From SubtypeMetaRole In FactRoles.<orm:SubtypeMetaRole>
                                               From RolePlayer In SubtypeMetaRole.<orm:RolePlayer>
                                               Where RolePlayer.Attribute("ref") = lrEntityType.NORMAReferenceId
                                               Select ModelInformation

                    If loEnumElementQueryResult.Count > 0 Then

                        Dim lrSupertypeEntityType As FBM.EntityType

                        For Each loElement In loEnumElementQueryResult

                            Dim loSuprtypeElement As IEnumerable(Of XElement)
                            loSuprtypeElement = From FactRoles In loElement.<orm:FactRoles>
                                                From SupertypeMetaRole In FactRoles.<orm:SupertypeMetaRole>
                                                From RolePlayer In SupertypeMetaRole.<orm:RolePlayer>
                                                Select RolePlayer

                            lrSupertypeEntityType = arModel.EntityType.Find(Function(x) x.NORMAReferenceId = loSuprtypeElement(0).Attribute("ref").Value)

                            If lrSupertypeEntityType Is Nothing Then
                                Dim lsUltimateNORMAReferenceId As String = Nothing
                                Select Case Me.GetTypeOfObjectFromNORMAFile(arNORMAXMLDOC,
                                                                            loSuprtypeElement(0).Attribute("ref").Value,
                                                                            lsUltimateNORMAReferenceId)
                                    Case Is = pcenumConceptType.EntityType
                                        lrSupertypeEntityType = Me.LoadEntityType(arModel, arNORMAXMLDOC, lsUltimateNORMAReferenceId)
                                    Case Is = pcenumConceptType.FactType
                                        Dim lrFactType As New FBM.FactType(arModel, "DummyFactType", True)
                                        lrFactType.NORMAReferenceId = lsUltimateNORMAReferenceId
                                        lrFactType = Me.LoadFactType(lrFactType, arNORMAXMLDOC)
                                        If lrFactType.IsObjectified Then
                                            lrSupertypeEntityType = lrFactType.ObjectifyingEntityType
                                        Else
                                            prApplication.ThrowErrorMessage("Tried to load a Supertype that is a Fact Type that is not Objectified", pcenumErrorType.Warning, Nothing, False, False, True)
                                            GoTo SkippedSubtypeRelationship
                                        End If
                                End Select

                            End If

                            Dim lsSubtypeRoleId As String = loElement.<orm:FactRoles>.<orm:SubtypeMetaRole>.AsEnumerable.First.Attribute("id").Value
                            Dim lsSupertypeRoleId As String = loElement.<orm:FactRoles>.<orm:SupertypeMetaRole>.AsEnumerable.First.Attribute("id").Value

                            Dim lrSubtypeRelationship As FBM.tSubtypeRelationship = Nothing
                            lrSubtypeRelationship = lrEntityType.CreateSubtypeRelationship(lrSupertypeEntityType,
                                                                                           False,
                                                                                           lsSubtypeRoleId,
                                                                                           lsSupertypeRoleId)
                            Try
                                lrSubtypeRelationship.IsPrimarySubtypeRelationship = CBool(loElement.Attribute("PreferredIdentificationPath").Value)
                            Catch ex As Exception
                                lrSubtypeRelationship.IsPrimarySubtypeRelationship = False
                            End Try
SkippedSubtypeRelationship:
                        Next
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

        Public Sub LoadValueTypes(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrValueType As New FBM.ValueType
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement

            Try

                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ValueType>
                                           Select ModelInformation
                                           Order By ModelInformation.Attribute("Name").Value

                '---------------------------------
                'Add the ValueTypes to the Model
                '---------------------------------            
                For Each loElement In loEnumElementQueryResult
                    lrValueType = New FBM.ValueType(arModel, pcenumLanguage.ORMModel, loElement.Attribute("Name").Value, loElement.Attribute("Name").Value)
                    lrValueType.NORMAReferenceId = loElement.Attribute("id").Value

                    If Not arModel.ValueType.Exists(AddressOf lrValueType.EqualsByName) Then
                        arModel.AddValueType(lrValueType, False, False, Nothing, True)
                    End If

                    Dim loIsNORMAUnaryFactTypeValueType As IEnumerable(Of XElement)
                    loIsNORMAUnaryFactTypeValueType = From ValueRange In loElement.<orm:ValueRestriction>.<orm:ValueConstraint>.<orm:ValueRanges>.<orm:ValueRange>
                                                      Select ValueRange

                    Dim loValueRange As XElement

                    For Each loValueRange In loIsNORMAUnaryFactTypeValueType
                        If loValueRange.Attribute("MinValue").Value = "True" And
                           loValueRange.Attribute("MaxValue").Value = "True" Then
                            lrValueType.NORMAIsUnaryFactTypeValueType = True
                        End If
                    Next

                    Dim loDataTypeElement As XElement = loElement.<orm:ConceptualDataType>.AsEnumerable.First
                    Dim lsNORMADataTypeName As String = ""
                    If Me.DataTypeDictionary.TryGetValue(loDataTypeElement.Attribute("ref").Value, lsNORMADataTypeName) Then
                        lrValueType.DataType = Me.getBostonDataTypeByNORMADataType(lsNORMADataTypeName)
                        lrValueType.DataTypeLength = loDataTypeElement.Attribute("Length").Value
                        lrValueType.DataTypePrecision = loDataTypeElement.Attribute("Scale").Value
                    End If

                    '-------------------------------------------------------------------------
                    'ValueConstraints
                    If loElement.<orm:ValueRestriction>.Count > 0 Then
                        Dim lsValue As String

                        For Each loValueTypeValueConstraint In loElement.<orm:ValueRestriction>.<orm:ValueConstraint>.<orm:ValueRanges>.<orm:ValueRange>

                            lsValue = loValueTypeValueConstraint.Attribute("MinValue").Value

                            lrValueType.ValueConstraint.Add(lsValue)
                            lrValueType.Instance.Add(lsValue)
                        Next
                    End If

                Next

                '-----------------------------------------------------
                'Check for Subtype relationships for each ValueType
                '-----------------------------------------------------
                For Each lrValueType In arModel.ValueType.ToArray
                    loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:SubtypeFact>
                                               From FactRoles In ModelInformation.<orm:FactRoles>
                                               From SubtypeMetaRole In FactRoles.<orm:SubtypeMetaRole>
                                               From RolePlayer In SubtypeMetaRole.<orm:RolePlayer>
                                               Where RolePlayer.Attribute("ref") = lrValueType.NORMAReferenceId
                                               Select ModelInformation

                    If loEnumElementQueryResult.Count > 0 Then

                        Dim lrSupertypeModelElement As FBM.ModelObject

                        For Each loElement In loEnumElementQueryResult

                            Dim loSuprtypeElement As IEnumerable(Of XElement)
                            loSuprtypeElement = From FactRoles In loElement.<orm:FactRoles>
                                                From SupertypeMetaRole In FactRoles.<orm:SupertypeMetaRole>
                                                From RolePlayer In SupertypeMetaRole.<orm:RolePlayer>
                                                Select RolePlayer

                            lrSupertypeModelElement = arModel.ValueType.Find(Function(x) x.NORMAReferenceId = loSuprtypeElement(0).Attribute("ref").Value)

                            If lrSupertypeModelElement Is Nothing Then
                                Dim lsUltimateNORMAReferenceId As String = Nothing
                                Select Case Me.GetTypeOfObjectFromNORMAFile(arNORMAXMLDOC,
                                                                            loSuprtypeElement(0).Attribute("ref").Value,
                                                                            lsUltimateNORMAReferenceId)
                                    Case Is = pcenumConceptType.EntityType
                                        lrSupertypeModelElement = Me.LoadEntityType(arModel, arNORMAXMLDOC, lsUltimateNORMAReferenceId)
                                    Case Is = pcenumConceptType.FactType
                                        Dim lrFactType As New FBM.FactType(arModel, "DummyFactType", True)
                                        lrFactType.NORMAReferenceId = lsUltimateNORMAReferenceId
                                        lrFactType = Me.LoadFactType(lrFactType, arNORMAXMLDOC)
                                        If lrFactType.IsObjectified Then
                                            lrSupertypeModelElement = lrFactType.ObjectifyingEntityType
                                        Else
                                            prApplication.ThrowErrorMessage("Tried to load a Supertype that is a Fact Type that is not Objectified", pcenumErrorType.Warning, Nothing, False, False, True)
                                            GoTo SkippedSubtypeRelationship
                                        End If
                                End Select
                            End If

                            Dim lsSubtypeRoleId As String = loElement.<orm:FactRoles>.<orm:SubtypeMetaRole>.AsEnumerable.First.Attribute("id").Value
                            Dim lsSupertypeRoleId As String = loElement.<orm:FactRoles>.<orm:SupertypeMetaRole>.AsEnumerable.First.Attribute("id").Value

                            Dim lrSubtypeRelationship As FBM.tSubtypeRelationship = Nothing
                            lrSubtypeRelationship = lrValueType.CreateSubtypeRelationship(lrSupertypeModelElement,
                                                                                          False,
                                                                                          lsSubtypeRoleId,
                                                                                          lsSupertypeRoleId)
                            Try
                                lrSubtypeRelationship.IsPrimarySubtypeRelationship = CBool(loElement.Attribute("PreferredIdentificationPath").Value)
                            Catch ex As Exception
                                lrSubtypeRelationship.IsPrimarySubtypeRelationship = False
                            End Try
SkippedSubtypeRelationship:
                        Next
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

        ''' <summary>
        ''' Loads a specific FactType from the NORMAXMLDOC
        ''' </summary>
        ''' <param name="arFactType">The FactType that is to be loaded. Must be prepopulated with the .Model and .Id of the FactType that is to be loaded.</param>
        ''' <param name="arNORMAXMLDOC">The opened XDocument reference to the NORMA XML document that is being imported into Boston.</param>
        ''' <remarks></remarks>
        Public Function LoadFactType(ByRef arFactType As FBM.FactType, ByRef arNORMAXMLDOC As XDocument) As FBM.FactType

            Dim lrJoinedEntityType As New FBM.EntityType
            Dim lrJoinedFactType As New FBM.FactType
            Dim lrValueType As New FBM.ValueType
            Dim lrFactType As New FBM.FactType
            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loXMLElementQueryResult As IEnumerable(Of XElement)
            Dim lrRoleXElement As XElement
            Dim loElement As XElement

            lrFactType = arFactType

            Try

                loEnumElementQueryResult = From FactTypeInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                           Where FactTypeInformation.Attribute("id").Value = lrFactType.NORMAReferenceId
                                           Select FactTypeInformation

                For Each loElement In loEnumElementQueryResult

                    lrFactType = New FBM.FactType(lrFactType.Model, loElement.Attribute("_Name").Value, loElement.Attribute("_Name").Value)
                    lrFactType.Name = Strings.Left(lrFactType.Name, 99)
                    lrFactType.Id = lrFactType.Model.CreateUniqueFactTypeName(lrFactType.Name, 0)
                    lrFactType.Name = lrFactType.Id
                    lrFactType.NORMAReferenceId = arFactType.NORMAReferenceId

                    If lrFactType.Model.FactType.Find(Function(x) x.NORMAReferenceId = lrFactType.NORMAReferenceId) IsNot Nothing Then
                        lrFactType = lrFactType.Model.FactType.Find(Function(x) x.NORMAReferenceId = lrFactType.NORMAReferenceId)
                    Else
                        lrFactType.Model.AddFactType(lrFactType, True) 'Don't use Model.CreateFactType because need to keep NORMA's FactType.Id for now.

                        '---------------------------
                        'Add the Roles to the Model
                        '---------------------------
                        For Each lrRoleXElement In loElement.<orm:FactRoles>.<orm:Role>

                            Dim lsNORMARoleId As String = lrRoleXElement.Attribute("id").Value

                            lrRole = New FBM.Role(lrFactType, lsNORMARoleId, True)
                            lrRole.Name = lrRoleXElement.Attribute("Name").Value

                            '----------------------------------------------
                            'Find the ModelObject within the NORMA Model
                            '----------------------------------------------
                            Dim lrModelObject As New FBM.ModelObject

                            lrModelObject.NORMAReferenceId = lrRoleXElement.<orm:RolePlayer>(0).Attribute("ref").Value

                            '-------------------------------------
                            'Check to see if it is an EntityType
                            '-------------------------------------
                            lrJoinedEntityType = New FBM.EntityType
                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:EntityType>
                                                      Where ModelInformation.Attribute("id") = lrModelObject.NORMAReferenceId

                            If IsSomething(loXMLElementQueryResult(0)) Then
                                lrModelObject.Name = loXMLElementQueryResult(0).Attribute("Name")
                                lrJoinedEntityType = lrFactType.Model.EntityType.Find(AddressOf lrModelObject.EqualsByName)
                            Else
                                lrJoinedEntityType = Nothing
                            End If

                            '-------------------------------------
                            'Check to see if it is an ValueType
                            '-------------------------------------
                            lrValueType = New FBM.ValueType
                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ValueType>
                                                      Where ModelInformation.Attribute("id") = lrModelObject.NORMAReferenceId

                            If IsSomething(loXMLElementQueryResult(0)) Then
                                lrModelObject.Name = loXMLElementQueryResult(0).Attribute("Name")
                                lrValueType = lrFactType.Model.ValueType.Find(AddressOf lrModelObject.EqualsByName)
                            Else
                                lrValueType = Nothing
                            End If
                            '-------------------------------------
                            'Check to see if it is an FactType
                            '-------------------------------------
                            lrJoinedFactType = New FBM.FactType
                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                                      Where ModelInformation.Attribute("id") = lrModelObject.NORMAReferenceId
                                                      Select ModelInformation

                            If IsSomething(loXMLElementQueryResult(0)) Then
                                lrModelObject.Name = loXMLElementQueryResult(0).Attribute("Name").Value
                                lrJoinedFactType = lrFactType.Model.FactType.Find(AddressOf lrModelObject.EqualsByName)
                            Else
                                lrJoinedFactType = Nothing
                            End If

                            '-----------------------------------------------
                            'Check to see if it is an Objectified FactType
                            '-----------------------------------------------
                            lrJoinedFactType = New FBM.FactType
                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ObjectifiedType>
                                                      Where ModelInformation.Attribute("id") = lrModelObject.NORMAReferenceId
                                                      Select ModelInformation

                            If IsSomething(loXMLElementQueryResult(0)) Then
                                lrModelObject.Name = loXMLElementQueryResult(0).Attribute("Name").Value

                                Dim loXMLNestedPredicateElement As IEnumerable(Of XElement)
                                loXMLNestedPredicateElement = From NestedPredicateElement In loXMLElementQueryResult(0).<orm:NestedPredicate>
                                                              Select NestedPredicateElement

                                lrModelObject.NORMAReferenceId = loXMLNestedPredicateElement(0).Attribute("ref").Value

                                lrJoinedFactType.Model = lrFactType.Model
                                lrJoinedFactType.Id = lrModelObject.Name
                                lrJoinedFactType.Name = lrModelObject.Name
                                lrJoinedEntityType.NORMAReferenceId = lrModelObject.NORMAReferenceId

                                If lrFactType.Model.FactType.Exists(AddressOf lrModelObject.Equals) Then
                                    '------------------------------
                                    'All okay, found the FactType
                                    '------------------------------
                                    lrJoinedFactType = lrFactType.Model.FactType.Find(AddressOf lrModelObject.Equals)
                                Else
                                    Call Me.LoadFactType(lrJoinedFactType, arNORMAXMLDOC)
                                End If
                            Else
                                lrJoinedFactType = Nothing
                            End If

                            If IsSomething(lrJoinedEntityType) Then
                                '---------------------------------------------------------
                                'Check to see if the Role already exists in the FactType
                                '---------------------------------------------------------
                                If lrFactType.RoleGroup.Exists(AddressOf lrRole.Equals) Then
                                    '--------------------------------
                                    'Update the JoinedORMObject etc
                                    '--------------------------------
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.RoleGroup.Find(AddressOf lrRole.Equals)
                                    lrRole.JoinedORMObject = lrJoinedEntityType
                                Else
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.CreateRole(lrJoinedEntityType)
                                    lrRole.Id = lsNORMARoleId
                                    lrRole.Name = lrRoleXElement.Attribute("Name").Value
                                    lrRole.Mandatory = Convert.ToBoolean(lrRoleXElement.Attribute("_IsMandatory").Value)
                                End If
                            ElseIf IsSomething(lrValueType) Then
                                '---------------------------------------------------------
                                'Check to see if the Role already exists in the FactType
                                '---------------------------------------------------------
                                If lrFactType.RoleGroup.Exists(AddressOf lrRole.Equals) Then
                                    '--------------------------------
                                    'Update the JoinedORMObject etc
                                    '--------------------------------
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.RoleGroup.Find(AddressOf lrRole.Equals)
                                    lrRole.JoinedORMObject = lrValueType
                                Else
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.CreateRole(lrValueType)
                                    lrRole.Id = lsNORMARoleId
                                    lrRole.Name = lrRoleXElement.Attribute("Name").Value
                                    lrRole.Mandatory = Convert.ToBoolean(lrRoleXElement.Attribute("_IsMandatory").Value)
                                    If lrValueType.NORMAIsUnaryFactTypeValueType Then
                                        lrRole.NORMALinksToUnaryFactTypeValueType = True
                                    End If
                                End If
                            ElseIf IsSomething(lrJoinedFactType) Then
                                '---------------------------------------------------------
                                'Check to see if the Role already exists in the FactType
                                '---------------------------------------------------------
                                If lrFactType.RoleGroup.Exists(AddressOf lrRole.Equals) Then
                                    '--------------------------------
                                    'Update the JoinedORMObject etc
                                    '--------------------------------
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.RoleGroup.Find(AddressOf lrRole.Equals)
                                    lrRole.JoinedORMObject = lrJoinedFactType
                                Else
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.CreateRole(lrJoinedFactType)
                                    lrRole.Id = lsNORMARoleId
                                    lrRole.Name = lrRoleXElement.Attribute("Name").Value
                                    lrRole.Mandatory = Convert.ToBoolean(lrRoleXElement.Attribute("_IsMandatory").Value)
                                End If
                            End If

                            'If lrRole.JoinedORMObject Is Nothing Then
                            '    '----------------------------------------------------------------------------
                            '    'Don't add the Role to the FactType.RoleGroup because it references nothing
                            '    '----------------------------------------------------------------------------
                            '    lrFactType.RoleGroup.Remove(lrRole)
                            '    lrFactType.Model.Role.Remove(lrRole)
                            '    Dim lsMessage As String = ""
                            '    lsMessage = "Warning: Error loading NORMA XML (.orm) file"
                            '    lsMessage &= vbCrLf & "NORMA Role.Id: " & lrRole.Id
                            '    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                            'End If

                            If lrRoleXElement.<orm:ValueRestriction>.Count > 0 Then

                                Dim loValueConstraintXElement As XElement = lrRoleXElement.<orm:ValueRestriction>.<orm:RoleValueConstraint>.First

                                Dim lsMinimumValue, lsMaximumValue As String
                                lsMinimumValue = loValueConstraintXElement.<orm:ValueRanges>.First.<orm:ValueRange>.First.Attribute("MinValue").Value
                                lsMaximumValue = loValueConstraintXElement.<orm:ValueRanges>.First.<orm:ValueRange>.First.Attribute("MaxValue").Value

                                Dim larRole As New List(Of FBM.Role)
                                larRole.Add(lrRole)

                                Dim lrRoleConstraint As FBM.RoleConstraint
                                If loValueConstraintXElement.<orm:ValueRanges>.<orm:ValueRange>.Count = 1 And lsMinimumValue.IsNumeric And lsMaximumValue.IsNumeric Then
                                    lrRoleConstraint = New FBM.RoleConstraint(Me.FBMModel, loValueConstraintXElement.Attribute("Name").Value, True, pcenumRoleConstraintType.ValueConstraint, larRole, True)
                                    lrRoleConstraint.MinimumValue = lsMinimumValue
                                    lrRoleConstraint.MaximumValue = lsMaximumValue
                                    lrRoleConstraint.NORMAReferenceId = loValueConstraintXElement.Attribute("id").Value
                                Else
                                    lrRoleConstraint = New FBM.RoleConstraint(Me.FBMModel, loValueConstraintXElement.Attribute("Name").Value, True, pcenumRoleConstraintType.RoleValueConstraint, larRole, True)
                                    lrRoleConstraint.NORMAReferenceId = loValueConstraintXElement.Attribute("id").Value

                                    For Each loValueRangeXElement In loValueConstraintXElement.<orm:ValueRanges>.<orm:ValueRange>
                                        lrRoleConstraint.AddValueConstraint(loValueRangeXElement.Attribute("MinValue").Value)
                                    Next
                                End If


                                Call Me.FBMModel.AddRoleConstraint(lrRoleConstraint, True)
                            End If
                        Next 'Role

                        '---------------------------------------------
                        'Load the Internal Uniqueness Constraints
                        '---------------------------------------------
                        Dim laoXMLUniquenessConstraints = From UniquenessConstraint In loElement.<orm:InternalConstraints>.<orm:UniquenessConstraint>
                                                          Select UniquenessConstraint

                        For Each loXMLUniquenessConstraint In laoXMLUniquenessConstraints
                            Call Me.LoadRoleConstraintInternalUniquenessConstraint(Me.FBMModel, arNORMAXMLDOC, loXMLUniquenessConstraint.Attribute("ref").Value)
                        Next

                        '---------------------------------------------
                        'Check to see if the FactType is Objectified
                        '---------------------------------------------
                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ObjectifiedType>
                                                  Where ModelInformation.Attribute("Name").Value = lrFactType.Name
                                                  Where ModelInformation.Attribute("IsIndependant") Is Nothing
                                                  Select ModelInformation

                        '--------------------------------------------
                        'Load the FactTypeReadings for the FactType
                        '--------------------------------------------
                        Call Me.LoadFactTypeReadings(lrFactType.Model, lrFactType, loElement)

                        '---------------------------------
                        'Load the Facts for the FactType
                        '---------------------------------
                        Call Me.LoadFactTypeFacts(lrFactType, arNORMAXMLDOC, loElement)

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            'Need to remove NORMAUnaryFactTypeValueTypes before Objectifying.
                            For Each lrRole In lrFactType.RoleGroup.ToArray
                                If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                    Call lrRole.FactType.RemoveRole(lrRole, False, True)
                                End If
                            Next

                            lrFactType.Objectify(True)
                        End If

                    End If 'Adding new FactType to the Model.
                Next

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

        Public Sub LoadFactTypes(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrJoinedEntityType As New FBM.EntityType
            Dim lrJoinedFactType As New FBM.FactType
            Dim lrValueType As New FBM.ValueType
            Dim lrFactType As New FBM.FactType
            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement
            Dim loXMLElementQueryResult As IEnumerable(Of XElement)
            Dim lsMessage As String
            '--------------------------------------------------------------------------------------------
            '  PSEUDOCODE
            '  * Load all of the FactTypes regardless of whether the FactType has a Role referencing
            '      a(FactType) not yet loaded.
            '  * Update all of those FactTypes loaded that have a Role of TypeOfJoin=FactType and where
            '      JoinedORMObject = Nothing to FactTypes already loaded in the previous step.
            '      Update the JoinedORMObject to the FactType that was missing on the initial load.
            '--------------------------------------------------------------------------------------------

            Try
                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                           Select ModelInformation
                                           Order By ModelInformation.Attribute("_Name").Value

                '---------------------------------
                'Add the FactTypes to the Model
                '---------------------------------     

                For Each loElement In loEnumElementQueryResult

                    lrFactType = New FBM.FactType(arModel, loElement.Attribute("_Name").Value, loElement.Attribute("_Name").Value)
                    lrFactType.NORMAName = lrFactType.Name
                    lrFactType.Name = Strings.Left(lrFactType.Name, 99).Replace(Chr(34), "")
                    lrFactType.NORMAReferenceId = loElement.Attribute("id").Value
                    lrFactType.Id = arModel.CreateUniqueFactTypeName(lrFactType.Name, 0)
                    lrFactType.Name = lrFactType.Id

                    If arModel.FactType.Find(Function(x) x.NORMAReferenceId = lrFactType.NORMAReferenceId) IsNot Nothing Then
                        lrFactType = arModel.FactType.Find(Function(x) x.NORMAReferenceId = lrFactType.NORMAReferenceId)
                    Else
                        arModel.AddFactType(lrFactType, True, False, Nothing) 'Don't use Model.CreateFactType because need to keep NORMA's FactType.Id for now.

                        '---------------------------
                        'Add the Roles to the Model
                        '---------------------------
                        For Each lrRoleXElement In loElement.<orm:FactRoles>.<orm:Role>

                            Dim lsNORMARoleId As String = lrRoleXElement.Attribute("id").Value

                            lrRole = New FBM.Role(lrFactType, lsNORMARoleId, True)
                            lrRole.Name = lrRoleXElement.Attribute("Name").Value

                            '----------------------------------------------
                            'Find the ModelObject within the NORMA Model
                            '----------------------------------------------
                            Dim lrModelObject As New FBM.ModelObject

                            Try
                                lrModelObject.NORMAReferenceId = lrRoleXElement.<orm:RolePlayer>(0).Attribute("ref").Value
                            Catch ex As Exception
                                'Can't guarantee that NORMA Roles actually join something and are not in error.
                                'I.e. e.g. NORMA can have a Role that is in error, and the joinedModelElement is 'Missing'.
                                'In this case we can't create the Role for the FactType
                                lsMessage = "The Fact Type, " & lrFactType.Id & ", in the NORMA .orm file is in error and has a Role that is joined to nothing."
                                lsMessage.AppendDoubleLineBreak("Boston will load the Fact Type without the Role that joins nothing.")
                                lsMessage.AppendDoubleLineBreak("Recommendation: Fix the errors in NORMA .orm files before loading into Boston.")
                                MsgBox(lsMessage, MsgBoxStyle.Exclamation)
                                GoTo SkipRole
                            End Try

                            '-------------------------------------
                            'Check to see if it is an EntityType
                            '-------------------------------------
                            lrJoinedEntityType = New FBM.EntityType
                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:EntityType>
                                                      Where ModelInformation.Attribute("id") = lrModelObject.NORMAReferenceId

                            If IsSomething(loXMLElementQueryResult(0)) Then
                                lrModelObject.Name = loXMLElementQueryResult(0).Attribute("Name")
                                lrJoinedEntityType = arModel.EntityType.Find(AddressOf lrModelObject.EqualsByName)
                            Else
                                lrJoinedEntityType = Nothing
                            End If

                            '-------------------------------------
                            'Check to see if it is a ValueType
                            '-------------------------------------
                            lrValueType = New FBM.ValueType
                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ValueType>
                                                      Where ModelInformation.Attribute("id") = lrModelObject.NORMAReferenceId

                            If IsSomething(loXMLElementQueryResult(0)) Then
                                lrModelObject.Name = loXMLElementQueryResult(0).Attribute("Name")
                                lrValueType = arModel.ValueType.Find(AddressOf lrModelObject.EqualsByName)
                            Else
                                lrValueType = Nothing
                            End If

                            '-------------------------------------
                            'Check to see if it is an FactType
                            '-------------------------------------
                            lrJoinedFactType = New FBM.FactType
                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                                      Where ModelInformation.Attribute("id") = lrModelObject.NORMAReferenceId
                                                      Select ModelInformation

                            If IsSomething(loXMLElementQueryResult(0)) Then
                                lrModelObject.Name = loXMLElementQueryResult(0).Attribute("Name")
                                lrJoinedFactType = arModel.FactType.Find(Function(x) x.NORMAReferenceId = lrModelObject.NORMAReferenceId) '20220127-VM-was AddressOf lrModelObject.EqualsByName)
                            Else
                                lrJoinedFactType = Nothing
                            End If

                            '-----------------------------------------------
                            'Check to see if it is an Objectified FactType
                            '-----------------------------------------------
                            lrJoinedFactType = New FBM.FactType
                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ObjectifiedType>
                                                      Where ModelInformation.Attribute("id") = lrModelObject.NORMAReferenceId
                                                      Select ModelInformation

                            If IsSomething(loXMLElementQueryResult(0)) Then

                                Dim lrModelObject2 As New FBM.ModelObject
                                lrModelObject2.Id = loXMLElementQueryResult(0).Attribute("Name").Value
                                lrModelObject2.Name = lrModelObject.Id

                                Dim loXMLNestedPredicateElement As IEnumerable(Of XElement)
                                loXMLNestedPredicateElement = From NestedPredicateElement In loXMLElementQueryResult(0).<orm:NestedPredicate>
                                                              Select NestedPredicateElement

                                lrModelObject2.NORMAReferenceId = loXMLNestedPredicateElement(0).Attribute("ref").Value

                                lrJoinedFactType.Model = arModel
                                lrJoinedFactType.Id = lrModelObject2.Id
                                lrJoinedFactType.Name = lrModelObject2.Name
                                lrJoinedFactType.NORMAReferenceId = lrModelObject2.NORMAReferenceId

                                If arModel.FactType.Find(Function(x) x.NORMAReferenceId = lrModelObject2.NORMAReferenceId) IsNot Nothing Then '20220127-VM-Was 'Exists(AddressOf lrModelObject2.Equals) Then
                                    '------------------------------
                                    'All okay, found the FactType
                                    '------------------------------
                                    lrJoinedFactType = arModel.FactType.Find(Function(x) x.NORMAReferenceId = lrModelObject2.NORMAReferenceId) 'AddressOf lrModelObject2.Equals)
                                Else
                                    lrJoinedFactType = Me.LoadFactType(lrJoinedFactType, arNORMAXMLDOC)
                                End If
                            Else
                                lrJoinedFactType = Nothing
                            End If

                            If IsSomething(lrJoinedEntityType) Then
                                '---------------------------------------------------------
                                'Check to see if the Role already exists in the FactType
                                '---------------------------------------------------------
                                If lrFactType.RoleGroup.Exists(AddressOf lrRole.Equals) Then
                                    '--------------------------------
                                    'Update the JoinedORMObject etc
                                    '--------------------------------
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.RoleGroup.Find(AddressOf lrRole.Equals)
                                    lrRole.JoinedORMObject = lrJoinedEntityType
                                Else
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.CreateRole(lrJoinedEntityType)
                                    lrRole.Id = lsNORMARoleId
                                    lrRole.Name = lrRoleXElement.Attribute("Name").Value
                                    lrRole.Mandatory = Convert.ToBoolean(lrRoleXElement.Attribute("_IsMandatory").Value)
                                End If
                            ElseIf IsSomething(lrValueType) Then
                                '---------------------------------------------------------
                                'Check to see if the Role already exists in the FactType
                                '---------------------------------------------------------
                                If lrFactType.RoleGroup.Exists(AddressOf lrRole.Equals) Then
                                    '--------------------------------
                                    'Update the JoinedORMObject etc
                                    '--------------------------------
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.RoleGroup.Find(AddressOf lrRole.Equals)
                                    lrRole.JoinedORMObject = lrValueType
                                Else
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.CreateRole(lrValueType)
                                    lrRole.Id = lsNORMARoleId
                                    lrRole.Name = lrRoleXElement.Attribute("Name").Value
                                    lrRole.Mandatory = Convert.ToBoolean(lrRoleXElement.Attribute("_IsMandatory").Value)
                                    If lrValueType.NORMAIsUnaryFactTypeValueType Then
                                        lrRole.NORMALinksToUnaryFactTypeValueType = True
                                    End If
                                End If
                            ElseIf IsSomething(lrJoinedFactType) Then
                                '---------------------------------------------------------
                                'Check to see if the Role already exists in the FactType
                                '---------------------------------------------------------
                                If lrFactType.RoleGroup.Exists(AddressOf lrRole.Equals) Then
                                    '--------------------------------
                                    'Update the JoinedORMObject etc
                                    '--------------------------------
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.RoleGroup.Find(AddressOf lrRole.Equals)
                                    lrRole.JoinedORMObject = lrJoinedFactType
                                Else
                                    lrRole = New FBM.Role
                                    lrRole = lrFactType.CreateRole(lrJoinedFactType)
                                    lrRole.Id = lsNORMARoleId
                                    lrRole.Name = lrRoleXElement.Attribute("Name").Value
                                    lrRole.Mandatory = Convert.ToBoolean(lrRoleXElement.Attribute("_IsMandatory").Value)
                                End If
                            End If

                            'If lrRole.JoinedORMObject Is Nothing Then
                            '    '----------------------------------------------------------------------------
                            '    'Don't add the Role to the FactType.RoleGroup because it references nothing
                            '    '----------------------------------------------------------------------------
                            '    lrFactType.RoleGroup.Remove(lrRole)
                            '    arModel.Role.Remove(lrRole)
                            '    Dim lsMessage As String = ""
                            '    lsMessage = "Warning: Error loading NORMA XML (.orm) file"
                            '    lsMessage &= vbCrLf & "NORMA Role.Id: " & lrRole.Id
                            '    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                            'End If

SkipRole: 'Used when NORMA file has a 'Missing' Role.

                            If lrRoleXElement.<orm:ValueRestriction>.Count > 0 Then

                                Dim loValueConstraintXElement As XElement = lrRoleXElement.<orm:ValueRestriction>.<orm:RoleValueConstraint>.First

                                Dim lsMinimumValue, lsMaximumValue As String
                                lsMinimumValue = loValueConstraintXElement.<orm:ValueRanges>.First.<orm:ValueRange>.First.Attribute("MinValue").Value
                                lsMaximumValue = loValueConstraintXElement.<orm:ValueRanges>.First.<orm:ValueRange>.First.Attribute("MaxValue").Value

                                Dim larRole As New List(Of FBM.Role)
                                larRole.Add(lrRole)

                                Dim lrRoleConstraint As FBM.RoleConstraint
                                If loValueConstraintXElement.<orm:ValueRanges>.<orm:ValueRange>.Count = 1 And lsMinimumValue.IsNumeric And lsMaximumValue.IsNumeric Then
                                    lrRoleConstraint = New FBM.RoleConstraint(arModel, loValueConstraintXElement.Attribute("Name").Value, True, pcenumRoleConstraintType.ValueConstraint, larRole, True)
                                    lrRoleConstraint.MinimumValue = lsMinimumValue
                                    lrRoleConstraint.MaximumValue = lsMaximumValue
                                    lrRoleConstraint.NORMAReferenceId = loValueConstraintXElement.Attribute("id").Value
                                Else
                                    lrRoleConstraint = New FBM.RoleConstraint(arModel, loValueConstraintXElement.Attribute("Name").Value, True, pcenumRoleConstraintType.RoleValueConstraint, larRole, True)
                                    lrRoleConstraint.NORMAReferenceId = loValueConstraintXElement.Attribute("id").Value

                                    For Each loValueRangeXElement In loValueConstraintXElement.<orm:ValueRanges>.<orm:ValueRange>
                                        lrRoleConstraint.AddValueConstraint(loValueRangeXElement.Attribute("MinValue").Value)
                                    Next
                                End If


                                Call arModel.AddRoleConstraint(lrRoleConstraint, True)
                            End If
                        Next 'Role

                        '---------------------------------------------
                        'Load the Internal Uniqueness Constraints
                        '---------------------------------------------
                        Dim laoXMLUniquenessConstraints = From UniquenessConstraint In loElement.<orm:InternalConstraints>.<orm:UniquenessConstraint>
                                                          Select UniquenessConstraint

                        For Each loXMLUniquenessConstraint In laoXMLUniquenessConstraints

                            Dim lrRoleConstraint As FBM.RoleConstraint
                            lrRoleConstraint = Me.LoadRoleConstraintInternalUniquenessConstraint(Me.FBMModel, arNORMAXMLDOC, loXMLUniquenessConstraint.Attribute("ref").Value)

                            Dim laoXMLIsPreferredUniqueness = From PreferredIdentifier In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ObjectifiedType>.<orm:PreferredIdentifier>
                                                              Where PreferredIdentifier.Attribute("ref").Value = loXMLUniquenessConstraint.Attribute("ref").Value
                                                              Select PreferredIdentifier

                            If laoXMLIsPreferredUniqueness.Count > 0 Then
                                Call lrRoleConstraint.SetIsPreferredIdentifier(True, True, Nothing)
                            End If
                        Next

                        '---------------------------------------------
                        'Check to see if the FactType is Objectified
                        '---------------------------------------------
                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ObjectifiedType>
                                                  Where ModelInformation.Attribute("Name").Value = lrFactType.NORMAName
                                                  Where ModelInformation.Attribute("IsIndependent") Is Nothing
                                                  Select ModelInformation

                        '--------------------------------------------
                        'Load the FactTypeReadings for the FactType
                        '--------------------------------------------
                        Call Me.LoadFactTypeReadings(arModel, lrFactType, loElement)

                        '---------------------------------
                        'Load the Facts for the FactType
                        '---------------------------------
                        Call Me.LoadFactTypeFacts(lrFactType, arNORMAXMLDOC, loElement)

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            'Need to remove NORMAUnaryFactTypeValueTypes before Objectifying.
                            For Each lrRole In lrFactType.RoleGroup.ToArray
                                If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                    Call lrRole.FactType.RemoveRole(lrRole, False, True)
                                End If
                            Next

                            lrFactType.Objectify(True)
                            Dim lsReferenceMode As String = ""
                            If loXMLElementQueryResult(0).Attribute("_ReferenceMode").Value <> "" Then
                                lsReferenceMode = "." & loXMLElementQueryResult(0).Attribute("_ReferenceMode").Value
                            End If
                            lrFactType.ObjectifyingEntityType.SetReferenceMode(lsReferenceMode, True,, False,, True, True)
                            lrFactType.ObjectifyingEntityType.NORMAReferenceId = loXMLElementQueryResult(0).Attribute("id").Value
                        End If

                    End If 'FactType exists in Model
                Next 'FactType

                Dim larFaultyFactTypes = From FactType In arModel.FactType
                                         From Role In FactType.RoleGroup
                                         Where Role.JoinedORMObject Is Nothing
                                         Select FactType

                For Each lrFactType In larFaultyFactTypes

                    Dim larRole = From Role In lrFactType.RoleGroup
                                  Where Role.JoinedORMObject Is Nothing
                                  Select Role

                    For Each lrRole In larRole

                        loEnumElementQueryResult = From FactType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                                   From Role In FactType.<orm:FactRoles>.<orm:Role>
                                                   Where Role.Attribute("id").Value = lrRole.Id
                                                   Select Role

                        For Each lrRoleXElement In loEnumElementQueryResult.<orm:RolePlayer>
                            '------------------------------------------------
                            'Find the ModelObject within the Richmond Model
                            '------------------------------------------------
                            lrJoinedFactType = New FBM.FactType
                            lrJoinedFactType.NORMAReferenceId = lrRoleXElement.<orm:RolePlayer>(0).Attribute("ref").Value
                            lrJoinedFactType = arModel.FactType.Find(Function(x) x.NORMAReferenceId = lrJoinedFactType.NORMAReferenceId)

                            lrRole.JoinedORMObject = lrJoinedFactType

                        Next 'Seach in NORMAXMLDOC
                    Next 'Role
                Next 'FaultyFactType

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub LoadModelNotes(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrModelNote As New FBM.ModelNote
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement

            Try

                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:ModelNotes>.<orm:ModelNote>
                                           Select ModelInformation

                '---------------------------------
                'Add the ValueTypes to the Model
                '---------------------------------            
                For Each loElement In loEnumElementQueryResult
                    lrModelNote = New FBM.ModelNote(arModel)
                    lrModelNote.NORMAReferenceId = loElement.Attribute("id").Value
                    lrModelNote.Id = lrModelNote.NORMAReferenceId

                    If Not arModel.ModelNote.Exists(AddressOf lrModelNote.Equals) Then
                        arModel.AddModelNote(lrModelNote, True)
                    End If

                    lrModelNote.Text = loElement.<orm:Text>(0).Value

                    'Referenced ModelElement
                    Try
                        Dim lsModelElementNORMAReference As String
                        Try
                            lsModelElementNORMAReference = loElement.<orm:ReferencedBy>(0).<orm:ObjectType>(0).Attribute("ref").Value
                        Catch
                            lsModelElementNORMAReference = loElement.<orm:ReferencedBy>(0).<orm:FactType>(0).Attribute("ref").Value
                        End Try

                        Dim lrModelElement As FBM.ModelObject = arModel.getModelObjects.Find(Function(x) x.NORMAReferenceId = lsModelElementNORMAReference)
                        lrModelNote.JoinedObjectType = lrModelElement
                    Catch ex As Exception
                        'Model Notes do not need to reference a Model Element.
                    End Try
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function LoadRoleConstraintInternalUniquenessConstraint(ByRef arModel As FBM.Model,
                                                                  ByRef arNORMAXMLDOC As XDocument,
                                                                  ByVal asNORMARCID As String) As FBM.RoleConstraint

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim lrRoleXElement As XElement
            Dim lrRoleConstraint As FBM.RoleConstraint = Nothing

            Try
                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:UniquenessConstraint>
                                           Where ModelInformation.Attribute("id") = asNORMARCID
                                           Select ModelInformation
                                           Order By ModelInformation.Attribute("Name").Value

                Dim loXMLRoleConstraint As XElement
                Try
                    loXMLRoleConstraint = loEnumElementQueryResult.First
                Catch ex As Exception
                    Throw New Exception("Can't find RoleConstraint with Id: " & asNORMARCID)
                End Try

                '-------------------------------------------------------------------------
                'Create the list of Roles that are to be added to the new RoleConstraint
                '-------------------------------------------------------------------------
                Dim larRoleList As New List(Of FBM.Role)
                Dim lbRolesFound As Boolean = True

                For Each lrRoleXElement In loXMLRoleConstraint.<orm:RoleSequence>.<orm:Role>
                    lrRole = New FBM.Role
                    '--------------------------------
                    'Find the Role within the Model
                    '--------------------------------
                    lrRole.Id = lrRoleXElement.Attribute("ref").Value
                    lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                    If IsSomething(lrRole) Then
                        If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                            '------------------------------------------
                            'Don't add the Role to the RoleConstraint
                            '------------------------------------------
                        Else
                            larRoleList.Add(lrRole)
                        End If
                    Else
                        lbRolesFound = False

                        Dim lsMessage As String = ""
                        lsMessage = "Warning: Loading NORMA XML (.orm) file"
                        lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                        lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                    End If
                Next

                If lbRolesFound Then
                    If arModel.AreRolesWithinTheSameFactType(larRoleList) Then
                        '---------------------------
                        'Create the RoleConstraint
                        '---------------------------                            
                        lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                                                                            larRoleList,
                                                                            loXMLRoleConstraint.Attribute("Name").Value,,, False)
                        lrRoleConstraint.Role(0).FactType.InternalUniquenessConstraint.AddUnique(lrRoleConstraint)
                        lrRoleConstraint.LevelNr = lrRoleConstraint.Role(0).FactType.InternalUniquenessConstraint.Count
                        lrRoleConstraint.isDirty = True
                        lrRoleConstraint.NORMAReferenceId = loXMLRoleConstraint.Attribute("id").Value
                        arModel.AddRoleConstraint(lrRoleConstraint, False, False, Nothing, False, Nothing)
                    End If
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

        Public Sub LoadRoleConstraintInternalUniquenessConstraints(ByRef arModel As FBM.Model,
                                                                   ByRef arNORMAXMLDOC As XDocument,
                                                                   Optional ByVal abSuppressLinkFactTypeIUCs As Boolean = False)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:UniquenessConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult
                '-------------------------------------------------------------------------
                'Create the list of Roles that are to be added to the new RoleConstraint
                '-------------------------------------------------------------------------
                If arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = loElement.Attribute("id").Value) Is Nothing Then


                    Dim larRoleList As New List(Of FBM.Role)
                    Dim lbRolesFound As Boolean = True
                    For Each lrRoleXElement In loElement.<orm:RoleSequence>.<orm:Role>
                        lrRole = New FBM.Role
                        '--------------------------------
                        'Find the Role within the Model
                        '--------------------------------
                        lrRole.Id = lrRoleXElement.Attribute("ref").Value
                        lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                        If IsSomething(lrRole) Then
                            If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                '------------------------------------------
                                'Don't add the Role to the RoleConstraint
                                '------------------------------------------
                            Else
                                larRoleList.Add(lrRole)
                            End If
                        Else
                            lbRolesFound = False

                            If My.Settings.NORMAImportingThrowRoleConstraintRoleWarnings Then
                                Dim lsMessage As String = ""
                                lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                lsMessage &= vbCrLf & " No Role found for RoleConstraint.Name: " & loElement.Attribute("Name").Value
                                lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                            End If
                        End If
                    Next

                    If lbRolesFound Then
                        If arModel.AreRolesWithinTheSameFactType(larRoleList) Then
                            '---------------------------
                            'Create the RoleConstraint
                            '---------------------------
                            Dim lrRoleConstraint As FBM.RoleConstraint
                            If abSuppressLinkFactTypeIUCs And larRoleList(0).FactType.IsLinkFactType Then
                                'Ignore
                            Else
                                lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                                                                            larRoleList,
                                                                            loElement.Attribute("Name").Value,,, False)
                                lrRoleConstraint.Role(0).FactType.InternalUniquenessConstraint.AddUnique(lrRoleConstraint)
                                lrRoleConstraint.LevelNr = lrRoleConstraint.Role(0).FactType.InternalUniquenessConstraint.Count
                                lrRoleConstraint.isDirty = True
                                lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value

                                arModel.AddRoleConstraint(lrRoleConstraint, False, False, Nothing, False, Nothing)
                            End If
                        End If
                        End If
                End If
            Next

        End Sub

        Public Sub LoadRoleConstraintExternalUniquenessConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement
            Dim lsMessage As String = ""

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:UniquenessConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult

                Try
                    '-------------------------------------------------------------------------
                    'Create the list of Roles that are to be added to the new RoleConstraint
                    '-------------------------------------------------------------------------
                    Dim larRoleList As New List(Of FBM.Role)
                    Dim lbRolesFound As Boolean = True

                    If arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = loElement.Attribute("id").Value) IsNot Nothing Then
                        'RoleConstraint has already been added to the Model.
                        GoTo SkipRoleConstraint
                    End If

                    For Each lrRoleXElement In loElement.<orm:RoleSequence>.<orm:Role>
                        lrRole = New FBM.Role
                        '--------------------------------
                        'Find the Role within the Model
                        '--------------------------------
                        lrRole.Id = lrRoleXElement.Attribute("ref").Value
                        lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                        If IsSomething(lrRole) Then
                            If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                '------------------------------------------
                                'Don't add the Role to the RoleConstraint
                                '------------------------------------------
                            Else
                                larRoleList.Add(lrRole)
                            End If
                        Else
                            lbRolesFound = False

                            If My.Settings.NORMAImportingThrowRoleConstraintRoleWarnings Then
                                lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: " & loElement.Attribute("Name").Value
                                lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                            End If
                        End If
                    Next

                    Dim lrRoleConstraint As FBM.RoleConstraint
                    If lbRolesFound Then
                        If arModel.AreRolesWithinTheSameFactType(larRoleList) Then
                            '----------------------------------------------------------
                            'Represents an Internal Uniqueness Constraint, do nothing
                            '----------------------------------------------------------
                        Else
                            '---------------------------
                            'Create the RoleConstraint
                            '---------------------------
                            lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.ExternalUniquenessConstraint, larRoleList, loElement.Attribute("Name").Value)
                            lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id")
                            arModel.AddRoleConstraint(lrRoleConstraint)

                            Try
                                Dim lsPreferredIdentifierForModelElementId = (From PreferredIdentiferFor In loElement.<orm:PreferredIdentifierFor>
                                                                              Select PreferredIdentiferFor).First.Attribute("ref").Value

                                Dim lrEntityType As FBM.EntityType = arModel.EntityType.Find(Function(x) x.NORMAReferenceId = lsPreferredIdentifierForModelElementId)
                                lrEntityType.SetCompoundReferenceSchemeRoleConstraint(lrRoleConstraint)
                                Call lrRoleConstraint.SetIsPreferredIdentifier(True, True, Nothing)

                            Catch ex As Exception
                                'Not a concern if it doesn't exist.
                            End Try

                        End If
                    End If
SkipRoleConstraint:
                Catch ex As Exception
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace, False, False, True)
                End Try
            Next

        End Sub

        Public Sub LoadRoleConstraintEqualityConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:EqualityConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult
                '-------------------------------------------------------------------------
                'Create the list of Roles that are to be added to the new RoleConstraint
                '-------------------------------------------------------------------------

                '---------------------------
                'Create the RoleConstraint
                '---------------------------
                Dim lrRoleConstraint As FBM.RoleConstraint
                lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.EqualityConstraint, Nothing, loElement.Attribute("Name").Value)
                lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value

                Dim lrRoleSequencesXElement As XElement
                Dim lrRoleSequenceXElement As XElement

                For Each lrRoleSequencesXElement In loElement.<orm:RoleSequences>
                    Dim liRoleSequenceNr As Integer = 1
                    For Each lrRoleSequenceXElement In lrRoleSequencesXElement.<orm:RoleSequence>

                        Dim lbRolesFound As Boolean = True
                        Dim larRoleList As New List(Of FBM.Role)

                        lrRoleConstraint.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraint, lrRoleConstraint.GetNextArgumentSequenceNr)

                        For Each lrRoleXElement In lrRoleSequenceXElement.<orm:Role>
                            lrRole = New FBM.Role
                            '--------------------------------
                            'Find the Role within the Model
                            '--------------------------------
                            lrRole.Id = lrRoleXElement.Attribute("ref").Value
                            lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                            If IsSomething(lrRole) Then
                                If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                    '------------------------------------------
                                    'Don't add the Role to the RoleConstraint
                                    '------------------------------------------
                                Else
                                    larRoleList.Add(lrRole)
                                End If
                            Else
                                lbRolesFound = False

                                Dim lsMessage As String = ""
                                lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                                lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                            End If

                        Next

                        If lbRolesFound Then
                            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                            For Each lrRole In larRoleList

                                lrRoleConstraint.CurrentArgument.ManuallyCreatedJoinPath = True
                                lrRoleConstraintRole = lrRoleConstraint.CreateRoleConstraintRole(lrRole,
                                                                                                 lrRoleConstraint.CurrentArgument,
                                                                                                 Nothing)

                                'If liRoleSequenceNr = 1 Then
                                '    '----------
                                '    'Is Entry
                                '    '----------
                                '    '------------------------------------------------------------------------
                                '    'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                '    '------------------------------------------------------------------------
                                '    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, True, False, liRoleSequenceNr)
                                'Else
                                '    '--------
                                '    'Is Exit
                                '    '--------
                                '    '------------------------------------------------------------------------
                                '    'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                '    '------------------------------------------------------------------------
                                '    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, False, True, liRoleSequenceNr)
                                'End If

                                ''------------------------------------
                                ''Add the Role to the RoleConstraint
                                ''------------------------------------
                                'lrRoleConstraint.Role.Add(lrRole)

                                ''------------------------------------------
                                ''Attach the RoleConstraintRole to the Role
                                ''------------------------------------------
                                'lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)

                                ''----------------------------------------------------
                                ''Attach the RoleConstraintRole to the RoleConstraint
                                ''----------------------------------------------------
                                'lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                            Next 'Role
                        End If
                        liRoleSequenceNr += 1
                        lrRoleConstraint.Argument.Add(lrRoleConstraint.CurrentArgument)
                    Next 'Role Sequence                    
                Next 'Role Sequences
                arModel.AddRoleConstraint(lrRoleConstraint)
            Next 'Equality Constraint

        End Sub

        Public Sub LoadRoleConstraintExclusiveOrConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:ExclusionConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult
                If loElement.<orm:ExclusiveOrMandatoryConstraint>.Count = 1 Then
                    '-------------------------------------------------------------------------
                    'Create the list of Roles that are to be added to the new RoleConstraint
                    '-------------------------------------------------------------------------

                    '---------------------------
                    'Create the RoleConstraint
                    '---------------------------
                    Dim lrRoleConstraint As FBM.RoleConstraint
                    lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.ExclusiveORConstraint, Nothing, loElement.Attribute("Name").Value)
                    lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value

                    Dim lrRoleSequencesXElement As XElement
                    Dim lrRoleSequenceXElement As XElement


                    lrRoleConstraint.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraint, lrRoleConstraint.GetNextArgumentSequenceNr)

                    For Each lrRoleSequencesXElement In loElement.<orm:RoleSequences>
                        Dim liRoleSequenceNr As Integer = 1
                        For Each lrRoleSequenceXElement In lrRoleSequencesXElement.<orm:RoleSequence>

                            Dim lbRolesFound As Boolean = True
                            Dim larRoleList As New List(Of FBM.Role)

                            For Each lrRoleXElement In lrRoleSequenceXElement.<orm:Role>
                                lrRole = New FBM.Role
                                '--------------------------------
                                'Find the Role within the Model
                                '--------------------------------
                                lrRole.Id = lrRoleXElement.Attribute("ref").Value
                                lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                                If IsSomething(lrRole) Then
                                    If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                        '------------------------------------------
                                        'Don't add the Role to the RoleConstraint
                                        '------------------------------------------
                                    Else
                                        larRoleList.Add(lrRole)
                                    End If
                                Else
                                    lbRolesFound = False

                                    Dim lsMessage As String = ""
                                    lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                    lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                                    lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                                End If

                            Next

                            If lbRolesFound Then
                                Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                                For Each lrRole In larRoleList

                                    lrRoleConstraintRole = lrRoleConstraint.CreateRoleConstraintRole(lrRole,
                                                                                                     lrRoleConstraint.CurrentArgument,
                                                                                                     Nothing)
                                    '    If liRoleSequenceNr = 1 Then
                                    '        '----------
                                    '        'Is Entry
                                    '        '----------
                                    '        '------------------------------------------------------------------------
                                    '        'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                    '        '------------------------------------------------------------------------
                                    '        lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, True, False, liRoleSequenceNr)
                                    '    Else
                                    '        '--------
                                    '        'Is Exit
                                    '        '--------
                                    '        '------------------------------------------------------------------------
                                    '        'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                    '        '------------------------------------------------------------------------
                                    '        lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, False, True, liRoleSequenceNr)
                                    '    End If

                                    '    '------------------------------------
                                    '    'Add the Role to the RoleConstraint
                                    '    '------------------------------------
                                    '    lrRoleConstraint.Role.Add(lrRole)

                                    '    '------------------------------------------
                                    '    'Attach the RoleConstraintRole to the Role
                                    '    '------------------------------------------
                                    '    lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)

                                    '    '----------------------------------------------------
                                    '    'Attach the RoleConstraintRole to the RoleConstraint
                                    '    '----------------------------------------------------
                                    '    lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                                Next 'Role
                            End If
                            liRoleSequenceNr += 1
                        Next 'Role Sequence                    
                    Next 'Role Sequences

                    lrRoleConstraint.Argument.Add(lrRoleConstraint.CurrentArgument)

                    arModel.AddRoleConstraint(lrRoleConstraint)
                End If
            Next 'Exclusive OR Constraint

        End Sub


        Public Sub LoadRoleConstraintValueComparisonConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:ValueComparisonConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult

                '-------------------------------------------------------------------------
                'Create the list of Roles that are to be added to the new RoleConstraint
                '-------------------------------------------------------------------------

                '---------------------------
                'Create the RoleConstraint
                '---------------------------
                Dim lrRoleConstraint As FBM.RoleConstraint
                lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.ValueComparisonConstraint, Nothing, loElement.Attribute("Name").Value)
                lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value

                Dim lrRoleSequenceXElement As XElement

                For Each lrRoleSequenceXElement In loElement.<orm:RoleSequence>

                    Dim lbRolesFound As Boolean = True
                    Dim larRoleList As New List(Of FBM.Role)

                    lrRoleConstraint.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraint, lrRoleConstraint.GetNextArgumentSequenceNr)

                    lrRoleConstraint.ValueRangeType = CType([Enum].Parse(GetType(pcenumValueRangeType), loElement.Attribute("Operator").Value), pcenumValueRangeType)

                    For Each lrRoleXElement In lrRoleSequenceXElement.<orm:Role>
                        lrRole = New FBM.Role
                        '--------------------------------
                        'Find the Role within the Model
                        '--------------------------------
                        lrRole.Id = lrRoleXElement.Attribute("ref").Value
                        lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                        If IsSomething(lrRole) Then
                            If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                '------------------------------------------
                                'Don't add the Role to the RoleConstraint
                                '------------------------------------------
                            Else
                                larRoleList.Add(lrRole)
                            End If
                        Else
                            lbRolesFound = False

                            Dim lsMessage As String = ""
                            lsMessage = "Warning: Loading NORMA XML (.orm) file"
                            lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                            lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                        End If

                    Next

                    If lbRolesFound Then
                        Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                        For Each lrRole In larRoleList

                            lrRoleConstraintRole = lrRoleConstraint.CreateRoleConstraintRole(lrRole,
                                                                                             lrRoleConstraint.CurrentArgument,
                                                                                             Nothing)
                            '    If liRoleSequenceNr = 1 Then
                            '        '----------
                            '        'Is Entry
                            '        '----------
                            '        '------------------------------------------------------------------------
                            '        'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                            '        '------------------------------------------------------------------------
                            '        lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, True, False, liRoleSequenceNr)
                            '    Else
                            '        '--------
                            '        'Is Exit
                            '        '--------
                            '        '------------------------------------------------------------------------
                            '        'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                            '        '------------------------------------------------------------------------
                            '        lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, False, True, liRoleSequenceNr)
                            '    End If

                            '    '------------------------------------
                            '    'Add the Role to the RoleConstraint
                            '    '------------------------------------
                            '    lrRoleConstraint.Role.Add(lrRole)

                            '    '------------------------------------------
                            '    'Attach the RoleConstraintRole to the Role
                            '    '------------------------------------------
                            '    lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)

                            '    '----------------------------------------------------
                            '    'Attach the RoleConstraintRole to the RoleConstraint
                            '    '----------------------------------------------------
                            '    lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                        Next 'Role
                    End If
                    lrRoleConstraint.Argument.Add(lrRoleConstraint.CurrentArgument)
                Next 'Role Sequence                    

                arModel.AddRoleConstraint(lrRoleConstraint)

            Next 'Value Comparison Constraint

        End Sub

        Public Sub LoadRoleConstraintExclusionConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:ExclusionConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult
                If loElement.<orm:ExclusiveOrMandatoryConstraint>.Count = 0 Then
                    '-------------------------------------------------------------------------
                    'Create the list of Roles that are to be added to the new RoleConstraint
                    '-------------------------------------------------------------------------

                    '---------------------------
                    'Create the RoleConstraint
                    '---------------------------
                    Dim lrRoleConstraint As FBM.RoleConstraint
                    lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.ExclusionConstraint, Nothing, loElement.Attribute("Name").Value)
                    lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value

                    Dim lrRoleSequencesXElement As XElement
                    Dim lrRoleSequenceXElement As XElement

                    For Each lrRoleSequencesXElement In loElement.<orm:RoleSequences>
                        Dim liRoleSequenceNr As Integer = 1
                        For Each lrRoleSequenceXElement In lrRoleSequencesXElement.<orm:RoleSequence>

                            Dim lbRolesFound As Boolean = True
                            Dim larRoleList As New List(Of FBM.Role)

                            For Each lrRoleXElement In lrRoleSequenceXElement.<orm:Role>
                                lrRole = New FBM.Role
                                '--------------------------------
                                'Find the Role within the Model
                                '--------------------------------
                                lrRole.Id = lrRoleXElement.Attribute("ref").Value
                                lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                                If IsSomething(lrRole) Then
                                    If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                        '------------------------------------------
                                        'Don't add the Role to the RoleConstraint
                                        '------------------------------------------
                                    Else
                                        larRoleList.Add(lrRole)
                                    End If
                                Else
                                    lbRolesFound = False

                                    Dim lsMessage As String = ""
                                    lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                    lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                                    lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                                End If

                            Next

                            If lbRolesFound Then
                                Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                                lrRoleConstraint.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraint,
                                                                                                  lrRoleConstraint.GetNextArgumentSequenceNr)


                                For Each lrRole In larRoleList

                                    lrRoleConstraintRole = lrRoleConstraint.CreateRoleConstraintRole(lrRole,
                                                                                                     lrRoleConstraint.CurrentArgument,
                                                                                                     Nothing)

                                    'If liRoleSequenceNr = 1 Then
                                    '    '----------
                                    '    'Is Entry
                                    '    '----------
                                    '    '------------------------------------------------------------------------
                                    '    'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                    '    '------------------------------------------------------------------------
                                    '    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, True, False, liRoleSequenceNr)
                                    'Else
                                    '    '--------
                                    '    'Is Exit
                                    '    '--------
                                    '    '------------------------------------------------------------------------
                                    '    'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                    '    '------------------------------------------------------------------------
                                    '    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, False, True, liRoleSequenceNr)
                                    'End If

                                    '------------------------------------
                                    'Add the Role to the RoleConstraint
                                    '------------------------------------
                                    'lrRoleConstraint.Role.Add(lrRole)

                                    '------------------------------------------
                                    'Attach the RoleConstraintRole to the Role
                                    '------------------------------------------
                                    'lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)

                                    ''----------------------------------------------------
                                    ''Attach the RoleConstraintRole to the RoleConstraint
                                    ''----------------------------------------------------
                                    'lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                                Next 'Role
                            End If
                            liRoleSequenceNr += 1
                            lrRoleConstraint.Argument.Add(lrRoleConstraint.CurrentArgument)
                        Next 'Role Sequence                    
                    Next 'Role Sequences



                    arModel.AddRoleConstraint(lrRoleConstraint)

                End If
            Next 'Subset Constraint


        End Sub

        Public Sub LoadRoleConstraintFrequencyConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:FrequencyConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult
                '-------------------------------------------------------------------------
                'Create the list of Roles that are to be added to the new RoleConstraint
                '-------------------------------------------------------------------------

                '---------------------------
                'Create the RoleConstraint
                '---------------------------
                Dim lrRoleConstraint As FBM.RoleConstraint
                lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.FrequencyConstraint, Nothing, loElement.Attribute("Name").Value)
                lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value
                lrRoleConstraint.MaximumFrequencyCount = loElement.Attribute("MaxFrequency").Value
                lrRoleConstraint.MinimumFrequencyCount = loElement.Attribute("MinFrequency").Value

                If (lrRoleConstraint.MinimumFrequencyCount = 0) And (lrRoleConstraint.MaximumFrequencyCount > 0) Then
                    lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOREqual
                ElseIf (lrRoleConstraint.MinimumFrequencyCount > 0) And (lrRoleConstraint.MaximumFrequencyCount = 0) Then
                    lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.GreaterThanOREqual
                ElseIf lrRoleConstraint.MinimumFrequencyCount = lrRoleConstraint.MaximumFrequencyCount Then
                    lrRoleConstraint.CardinalityRangeType = pcenumCardinalityRangeType.Equal
                End If

                lrRoleConstraint.Cardinality = lrRoleConstraint.MinimumFrequencyCount
                If lrRoleConstraint.MaximumFrequencyCount > lrRoleConstraint.MinimumFrequencyCount Then
                    lrRoleConstraint.Cardinality = lrRoleConstraint.MaximumFrequencyCount
                End If

                Dim lrRoleSequenceXElement As XElement
                Dim liRoleSequenceNr As Integer = 1

                For Each lrRoleSequenceXElement In loElement.<orm:RoleSequence>

                    Dim lbRolesFound As Boolean = True
                    Dim larRoleList As New List(Of FBM.Role)

                    For Each lrRoleXElement In lrRoleSequenceXElement.<orm:Role>
                        lrRole = New FBM.Role
                        '--------------------------------
                        'Find the Role within the Model
                        '--------------------------------
                        lrRole.Id = lrRoleXElement.Attribute("ref").Value
                        lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                        If IsSomething(lrRole) Then
                            If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                '------------------------------------------
                                'Don't add the Role to the RoleConstraint
                                '------------------------------------------
                            Else
                                larRoleList.Add(lrRole)
                            End If
                        Else
                            lbRolesFound = False

                            Dim lsMessage As String = ""
                            lsMessage = "Warning: Loading NORMA XML (.orm) file"
                            lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                            lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                        End If

                    Next

                    If lbRolesFound Then
                        Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                        For Each lrRole In larRoleList
                            If liRoleSequenceNr = 1 Then
                                '----------
                                'Is Entry
                                '----------
                                '------------------------------------------------------------------------
                                'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                '------------------------------------------------------------------------
                                lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, True, False, liRoleSequenceNr, True)
                            Else
                                '--------
                                'Is Exit
                                '--------
                                '------------------------------------------------------------------------
                                'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                '------------------------------------------------------------------------
                                lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, False, True, liRoleSequenceNr, True)
                            End If

                            '------------------------------------
                            'Add the Role to the RoleConstraint
                            '------------------------------------
                            lrRoleConstraint.Role.Add(lrRole)

                            '------------------------------------------
                            'Attach the RoleConstraintRole to the Role
                            '------------------------------------------
                            lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)

                            '----------------------------------------------------
                            'Attach the RoleConstraintRole to the RoleConstraint
                            '----------------------------------------------------
                            lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                        Next 'Role
                    End If
                    liRoleSequenceNr += 1
                Next 'Role Sequence                    

                arModel.AddRoleConstraint(lrRoleConstraint)
            Next 'Subset Constraint


        End Sub

        Public Sub SetRoleIdsForLinkFactTypes(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Try
                Dim lrRole As FBM.Role
                Dim loElement As XElement


                Dim loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:ImpliedFact>
                                               Select ModelInformation

                For Each loElement In loEnumElementQueryResult

                    Try
                        Dim loRoleProxyXElement As XElement

                        Try
                            loRoleProxyXElement = loElement.<orm:FactRoles>.<orm:RoleProxy>.<orm:Role>.First
                        Catch ex As Exception
                            loRoleProxyXElement = loElement.<orm:FactRoles>.<orm:ObjectifiedUnaryRole>.<orm:UnaryRole>.First
                        End Try


                        lrRole = New FBM.Role
                        lrRole.Id = loRoleProxyXElement.Attribute("ref").Value
                        lrRole = arModel.Role.Find(Function(x) x.Id = lrRole.Id)

                        If Not lrRole.FactType.IsObjectified Then GoTo SkippedRole

                        Dim loLinkFTRoleXElement As XElement = loElement.<orm:FactRoles>.<orm:Role>.First

                        Dim lrLinkFTRole As FBM.Role = (From FactType In Me.FBMModel.FactType
                                                        Where FactType.IsLinkFactType
                                                        Where FactType.LinkFactTypeRole.Id = lrRole.Id
                                                        Select FactType.RoleGroup(0)).First

                        lrLinkFTRole.Id = loLinkFTRoleXElement.Attribute("id").Value
                        lrLinkFTRole.FactType.NORMAReferenceId = loElement.Attribute("id").Value
SkippedRole:

                    Catch ex As Exception
                        Dim lsMessage As String
                        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                        lsMessage &= vbCrLf & vbCrLf & "Error trying to set RoleId for Link Fact Type for Fact Type: " & loElement.Attribute("_Name").Value
                        lsMessage &= vbCrLf & vbCrLf & ex.Message
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                    End Try
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub




        Public Sub LoadRoleConstraintInclusiveOrConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:MandatoryConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult
                If loElement.<orm:ExclusiveOrExclusionConstraint>.Count = 0 Then
                    '-------------------------------------------------------------------------
                    'Create the list of Roles that are to be added to the new RoleConstraint
                    '-------------------------------------------------------------------------
                    If (loElement.<orm:RoleSequence>.<orm:Role>.Count > 1) And (loElement.<orm:ImpliedByObjectType>.Count = 0) Then
                        '---------------------------
                        'Create the RoleConstraint
                        '---------------------------
                        Dim lrRoleConstraint As FBM.RoleConstraint
                        lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.InclusiveORConstraint, Nothing, loElement.Attribute("Name").Value)
                        lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value


                        Dim lrRoleSequenceXElement As XElement


                        Dim liRoleSequenceNr As Integer = 1
                        For Each lrRoleSequenceXElement In loElement.<orm:RoleSequence>

                            Dim lbRolesFound As Boolean = True
                            Dim larRoleList As New List(Of FBM.Role)

                            For Each lrRoleXElement In lrRoleSequenceXElement.<orm:Role>
                                lrRole = New FBM.Role
                                '--------------------------------
                                'Find the Role within the Model
                                '--------------------------------
                                lrRole.Id = lrRoleXElement.Attribute("ref").Value
                                lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                                If IsSomething(lrRole) Then
                                    If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                        '------------------------------------------
                                        'Don't add the Role to the RoleConstraint
                                        '------------------------------------------
                                    Else
                                        larRoleList.Add(lrRole)
                                    End If
                                Else
                                    lbRolesFound = False

                                    Dim lsMessage As String = ""
                                    lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                    lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                                    lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                                End If

                            Next

                            If lbRolesFound Then
                                Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                                For Each lrRole In larRoleList
                                    If liRoleSequenceNr = 1 Then
                                        '----------
                                        'Is Entry
                                        '----------
                                        '------------------------------------------------------------------------
                                        'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                        '------------------------------------------------------------------------
                                        lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, True, False, liRoleSequenceNr)
                                    Else
                                        '--------
                                        'Is Exit
                                        '--------
                                        '------------------------------------------------------------------------
                                        'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                        '------------------------------------------------------------------------
                                        lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, False, True, liRoleSequenceNr)
                                    End If

                                    '------------------------------------
                                    'Add the Role to the RoleConstraint
                                    '------------------------------------
                                    lrRoleConstraint.Role.Add(lrRole)

                                    '------------------------------------------
                                    'Attach the RoleConstraintRole to the Role
                                    '------------------------------------------
                                    lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)

                                    '----------------------------------------------------
                                    'Attach the RoleConstraintRole to the RoleConstraint
                                    '----------------------------------------------------
                                    lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                                Next 'Role
                            End If
                            liRoleSequenceNr += 1
                        Next 'Role Sequence                    
                        arModel.AddRoleConstraint(lrRoleConstraint)
                    End If 'RoleSequence Count > 1
                End If
            Next 'Subset Constraint


        End Sub

        Public Sub LoadRoleConstraintRingConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:RingConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult
                Try

                    '---------------------------
                    'Create the RoleConstraint
                    '---------------------------
                    Dim lrRoleConstraint As FBM.RoleConstraint
                    lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.RingConstraint, Nothing, loElement.Attribute("Name").Value)
                    lrRoleConstraint.Id = loElement.Attribute("Name").Value
                    lrRoleConstraint.Name = lrRoleConstraint.Id
                    lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value
                    lrRoleConstraint.RingConstraintType = CType([Enum].Parse(GetType(pcenumRingConstraintType), loElement.Attribute("Type").Value), pcenumRingConstraintType)


                    Dim lrRoleSequenceXElement As XElement

                    lrRoleConstraint.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraint, lrRoleConstraint.GetNextArgumentSequenceNr)

                    Dim liRoleSequenceNr As Integer = 1
                    For Each lrRoleSequenceXElement In loElement.<orm:RoleSequence>

                        Dim lbRolesFound As Boolean = True
                        Dim larRoleList As New List(Of FBM.Role)

                        For Each lrRoleXElement In lrRoleSequenceXElement.<orm:Role>
                            lrRole = New FBM.Role
                            '--------------------------------
                            'Find the Role within the Model
                            '--------------------------------
                            lrRole.Id = lrRoleXElement.Attribute("ref").Value
                            lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                            If IsSomething(lrRole) Then
                                If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                    '------------------------------------------
                                    'Don't add the Role to the RoleConstraint
                                    '------------------------------------------
                                Else
                                    larRoleList.Add(lrRole)
                                End If
                            Else
                                lbRolesFound = False

                                Dim lsMessage As String = ""
                                lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                                lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                            End If

                        Next

                        If lbRolesFound Then
                            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                            For Each lrRole In larRoleList
                                'If liRoleSequenceNr = 1 Then
                                '    '----------
                                '    'Is Entry
                                '    '----------
                                '    '------------------------------------------------------------------------
                                '    'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                '    '------------------------------------------------------------------------
                                '    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, True, False, liRoleSequenceNr)
                                'Else
                                '    '--------
                                '    'Is Exit
                                '    '--------
                                '    '------------------------------------------------------------------------
                                '    'Create a new RoleConstraintRole for the RoleConstraint/Role combination
                                '    '------------------------------------------------------------------------
                                '    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint, False, True, liRoleSequenceNr)
                                'End If

                                ''------------------------------------
                                ''Add the Role to the RoleConstraint
                                ''------------------------------------
                                'lrRoleConstraint.Role.Add(lrRole)

                                ''------------------------------------------
                                ''Attach the RoleConstraintRole to the Role
                                ''------------------------------------------
                                'lrRole.RoleConstraintRole.Add(lrRoleConstraintRole)

                                ''----------------------------------------------------
                                ''Attach the RoleConstraintRole to the RoleConstraint
                                ''----------------------------------------------------
                                'lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)

                                '==================================================================================
                                lrRoleConstraintRole = lrRoleConstraint.CreateRoleConstraintRole(lrRole,
                                                                                                  lrRoleConstraint.CurrentArgument,
                                                                                                  Nothing)

                            Next 'Role
                        End If
                        liRoleSequenceNr += 1
                    Next 'Role Sequence                    

                    lrRoleConstraint.Argument.Add(lrRoleConstraint.CurrentArgument)
                    '==================================================================================              

                    arModel.AddRoleConstraint(lrRoleConstraint)

                Catch ex As Exception
                    Dim lsMessage As String
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                End Try
            Next 'Ring Constraint


        End Sub

        Public Sub LoadRoleConstraintSubsetConstraints(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrRole As New FBM.Role
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim loElement As XElement
            Dim lrRoleXElement As XElement

            loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:SubsetConstraint>
                                       Select ModelInformation
                                       Order By ModelInformation.Attribute("Name").Value

            For Each loElement In loEnumElementQueryResult
                '-------------------------------------------------------------------------
                'Create the list of Roles that are to be added to the new RoleConstraint
                '-------------------------------------------------------------------------

                '---------------------------
                'Create the RoleConstraint
                '---------------------------
                Dim lrRoleConstraint As FBM.RoleConstraint
                lrRoleConstraint = arModel.CreateRoleConstraint(pcenumRoleConstraintType.SubsetConstraint, Nothing, loElement.Attribute("Name").Value)
                lrRoleConstraint.NORMAReferenceId = loElement.Attribute("id").Value

                Dim lrRoleSequencesXElement As XElement
                Dim lrRoleSequenceXElement As XElement

                For Each lrRoleSequencesXElement In loElement.<orm:RoleSequences>

                    For Each lrRoleSequenceXElement In lrRoleSequencesXElement.<orm:RoleSequence>

                        lrRoleConstraint.CurrentArgument = New FBM.RoleConstraintArgument(lrRoleConstraint, lrRoleConstraint.GetNextArgumentSequenceNr)
                        Dim liRoleSequenceNr As Integer = 1

                        Dim lbRolesFound As Boolean = True
                        Dim larRoleList As New List(Of FBM.Role)

                        For Each lrRoleXElement In lrRoleSequenceXElement.<orm:Role>
                            lrRole = New FBM.Role
                            '--------------------------------
                            'Find the Role within the Model
                            '--------------------------------
                            lrRole.Id = lrRoleXElement.Attribute("ref").Value
                            lrRole = arModel.Role.Find(AddressOf lrRole.Equals)
                            If IsSomething(lrRole) Then
                                If lrRole.NORMALinksToUnaryFactTypeValueType = True Then
                                    '------------------------------------------
                                    'Don't add the Role to the RoleConstraint
                                    '------------------------------------------
                                Else
                                    larRoleList.Add(lrRole)
                                End If
                            Else
                                lbRolesFound = False

                                Dim lsMessage As String = ""
                                lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                                lsMessage &= vbCrLf & " Role.Id: " & lrRoleXElement.Attribute("ref").Value
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                            End If

                        Next

                        If lbRolesFound Then
                            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                            For Each lrRole In larRoleList
                                lrRoleConstraint.CurrentArgument.ManuallyCreatedJoinPath = True
                                lrRoleConstraintRole = lrRoleConstraint.CreateRoleConstraintRole(lrRole,
                                                                                                 lrRoleConstraint.CurrentArgument,
                                                                                                 Nothing)
                            Next 'Role
                        End If


                        If lrRoleSequenceXElement.<orm:JoinRule>.Count > 0 Then

                            lrRoleConstraint.CurrentArgument.JoinPath.RolePath.Clear()

                            For Each loPathedRoleXElement In lrRoleSequenceXElement.<orm:JoinRule>.<orm:JoinPath>.<orm:PathComponents>.<orm:RolePath>.<orm:PathedRoles>.<orm:PathedRole>

                                lrRole = New FBM.Role
                                lrRole.Id = loPathedRoleXElement.Attribute("ref").Value
                                lrRole = arModel.Role.Find(AddressOf lrRole.Equals)

                                If lrRole IsNot Nothing Then
                                    If Not lrRole.FactType.IsObjectified Then
                                        lrRoleConstraint.CurrentArgument.JoinPath.RolePath.AddUnique(lrRole)

                                        If lrRole.FactType.IsBinaryFactType Then
                                            Dim lrOtherRole As FBM.Role = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id)
                                            lrRoleConstraint.CurrentArgument.JoinPath.RolePath.AddUnique(lrOtherRole)
                                        End If
                                    End If
                                Else
                                    Dim lsMessage As String = ""
                                    lsMessage = "Warning: Loading NORMA XML (.orm) file"
                                    lsMessage &= vbCrLf & " No Role found for RoleConstraint.Id: "
                                    lsMessage &= vbCrLf & " Role.Id: " & loPathedRoleXElement.Attribute("ref").Value
                                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                                End If
                            Next

                        End If

                        Call lrRoleConstraint.CurrentArgument.JoinPath.ConstructFactTypePath()
                        liRoleSequenceNr += 1
                        lrRoleConstraint.Argument.Add(lrRoleConstraint.CurrentArgument)
                    Next 'Role Sequence                    
                Next 'Role Sequences
                arModel.AddRoleConstraint(lrRoleConstraint)
            Next 'Subset Constraint


        End Sub

        Public Sub GetRidOfRolesInFactTypesThatReferToUnaryFactTypeValueTypes(ByRef arModel As FBM.Model)

            Dim liInd As Integer = 0
            Dim lrFactType As FBM.FactType = Nothing
            Dim lrRole As FBM.Role

            Dim liFactTypeInd As Integer = 0

            Dim larRole = From FactType In arModel.FactType
                          From Role In FactType.RoleGroup
                          Where Role.NORMALinksToUnaryFactTypeValueType
                          Select Role

            For Each lrRole In larRole.ToArray
                Dim larJoinPath = From RoleConstraint In arModel.RoleConstraint
                                  From Argument In RoleConstraint.Argument
                                  From Role In Argument.JoinPath.RolePath
                                  Where Role.Id = lrRole.Id
                                  Select Argument.JoinPath

                Call lrRole.FactType.RemoveRole(lrRole, False, True)

                'RDS - Need to create a Column in the RDS for the Unary Fact Type.
                Try
                    lrFactType = lrRole.FactType
                    Dim lsPredicate As String = lrFactType.FactTypeReading(0).PredicatePart(0).PredicatePartText
                    lsPredicate = MakeCapCamelCase(lsPredicate, True)
                    Dim lrTable = lrFactType.RoleGroup(0).JoinedORMObject.getCorrespondingRDSTable
                    Dim lrColumn As New RDS.Column(lrTable, lsPredicate, lrFactType.RoleGroup(0), lrFactType.RoleGroup(0), False)
                    Call lrTable.addColumn(lrColumn)
                Catch ex As Exception
                    MsgBox("Trouble creating Column for Unary Fact Type: " & lrFactType.Id)
                End Try


                For Each lrJoinPath In larJoinPath.ToArray
                    Dim liIndex As Integer = lrJoinPath.RolePath.IndexOf(lrRole)
                    lrJoinPath.RolePath.RemoveAt(liIndex)
                    lrJoinPath.RolePath.Insert(liIndex, lrRole.FactType.RoleGroup(0))
                Next

                Call arModel.RemoveValueType(lrRole.JoinsValueType, False)
            Next

        End Sub

        Public Sub LoadPageModelInstances(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument)

            Dim lrPage As FBM.Page
            Dim lrEntityType As FBM.EntityType
            Dim lrValueType As FBM.ValueType
            Dim lrFactType As FBM.FactType
            Dim lrFact As FBM.Fact
            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            Dim lrRoleInstance As FBM.RoleInstance
            Dim loEnumElementQueryResult As IEnumerable(Of XElement)
            Dim lrFactTypeInstance2 As FBM.FactTypeInstance
            Dim lrModelNote As FBM.ModelNote
            Dim lsMessage As String

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim ldblScalar As Double = 30.5

            Try
                loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<ormDiagram:ORMDiagram>
                                           Select ModelInformation

                '---------------------------------------------------------------------------
                'Add the ModelObjects to the Pages.
                '  i.e. ModelObjects that are either EntityTypes, ValueTypes or FactTypes
                '---------------------------------------------------------------------------
                Dim lrObjectTypeShapeXElement As XElement
                Dim lrObjectTypeXElement As XElement
                Dim lrPageXElement As XElement
                '.<ormDiagram:Shapes>.<ormDiagram:ObjectTypeShape>
                For Each lrPageXElement In loEnumElementQueryResult
                    '-------------
                    'Get the Page
                    '-------------
                    lrPage = New FBM.Page(arModel, lrPageXElement.Attribute("id").Value, lrPageXElement.Attribute("Name").Value, pcenumLanguage.ORMModel)
                    lrPage = arModel.Page.Find(AddressOf lrPage.Equals) 'Because already loaded as empty shells.
                    If TablePage.ExistsPageById(lrPage.PageId) Then lrPage.PageId = System.Guid.NewGuid.ToString

                    '---------------------------------------------
                    'Load the ModelObjectInstances onto the Page
                    '---------------------------------------------
                    Dim lsBounds() As String
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ObjectTypeShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        Dim loXMLElementQueryResult As IEnumerable(Of XElement)
                        '-----------------------
                        'EntityType
                        '-----------------------
#Region "Entity Type"
                        Dim lbExpandReferenceScheme As Boolean = False
                        lrEntityType = New FBM.EntityType
                        lrEntityType.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value
                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:EntityType>
                                                  Where ModelInformation.Attribute("id") = lrEntityType.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            lrEntityType.Name = loXMLElementQueryResult(0).Attribute("Name")
                            lrEntityType = arModel.EntityType.Find(Function(x) x.NORMAReferenceId = lrEntityType.NORMAReferenceId)

                            Try
                                If My.Settings.NORMAImportingAlwaysCollapseReferenceMode Then
                                    lbExpandReferenceScheme = false
                                Else
                                    lbExpandReferenceScheme = CBool(lrObjectTypeShapeXElement.Attribute("ExpandRefMode").Value)
                                End If
                                
                            Catch ex As Exception
                                'Not a Biggie
                            End Try
                        Else
                            lrEntityType = Nothing
                        End If
#End Region
                        '-----------------------
                        'ValueType
                        '-----------------------
#Region "ValueType"
                        lrValueType = New FBM.ValueType
                        lrValueType.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value
                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ValueType>
                                                  Where ModelInformation.Attribute("id") = lrValueType.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            lrValueType.Name = loXMLElementQueryResult(0).Attribute("Name")
                            lrValueType = arModel.ValueType.Find(Function(x) x.NORMAReferenceId = lrValueType.NORMAReferenceId)
                        Else
                            lrValueType = Nothing
                        End If
#End Region

                        '---------------------------------------------------------------------------------------------------------------------
                        'FactType
                        '  NB Normally, should not find a FactType in <ormDiagram:ObjectTypeShape> because is in <ormDiagram:FactTypeShape>.
                        '  See further below where FactTypeInstance are loaded by searching <ormDiagram:FactTypeShape>
                        '---------------------------------------------------------------------------------------------------------------------
#Region "FactType"
                        lrFactType = New FBM.FactType
                        lrFactType.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value
                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                                  Where ModelInformation.Attribute("id") = lrFactType.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            lrFactType = arModel.FactType.Find(Function(x) x.NORMAReferenceId = lrFactType.NORMAReferenceId)
                        Else
                            lrFactType = Nothing
                        End If
#End Region

                        If IsSomething(lrEntityType) Then
#Region "EntityType"
                            lrEntityTypeInstance = New FBM.EntityTypeInstance
                            lrEntityTypeInstance = lrEntityType.CloneInstance(lrPage, False, True, False)
                            lrEntityTypeInstance.InstanceNumber = lrPage.EntityTypeInstance.FindAll(Function(x) x.Id = lrEntityType.Id).Count + 1

                            Boston.WriteToStatusBar("Loading Entity Type Instance")
                            lrPage.EntityTypeInstance.Add(lrEntityTypeInstance)

                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            lrEntityTypeInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            lrEntityTypeInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)

                            lrEntityTypeInstance.ExpandReferenceMode = lbExpandReferenceScheme
#End Region
                        ElseIf IsSomething(lrValueType) Then
#Region "ValueTypes"
                            Dim lrValueTypeInstance As New FBM.ValueTypeInstance
                            lrValueTypeInstance = lrValueType.CloneInstance(lrPage, False, True)
                            lrValueTypeInstance.InstanceNumber = lrPage.ValueTypeInstance.FindAll(Function(x) x.Id = lrValueType.Id).Count + 1
                            'If lrPage.ValueTypeInstance.Exists(AddressOf lrValueTypeInstance.Equals) Then
                            '    lrValueTypeInstance = lrPage.ValueTypeInstance.Find(AddressOf lrValueTypeInstance.Equals)
                            'Else
                            Boston.WriteToStatusBar("Loading Value Type Instance")
                            lrPage.ValueTypeInstance.Add(lrValueTypeInstance)
                            'End If
                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            lrValueTypeInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            lrValueTypeInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
#End Region
                        ElseIf IsSomething(lrFactType) Then
#Region "FactTypes"
                            lrFactTypeInstance = New FBM.FactTypeInstance
                            lrFactTypeInstance = lrFactType.CloneInstance(lrPage, False)
                            lrFactTypeInstance.InstanceNumber = lrPage.FactTypeInstance.FindAll(Function(x) x.Id = lrFactType.Id).Count + 1
                            'If lrPage.FactTypeInstance.Exists(AddressOf lrFactTypeInstance.Equals) Then
                            '    lrFactTypeInstance = lrPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                            'Else
                            Boston.WriteToStatusBar("Loading Fact Type Instance")
                            lrPage.DropFactTypeAtPoint(lrFactTypeInstance.FactType, New PointF(lrFactTypeInstance.X, lrFactTypeInstance.Y), False, False, False, False)
                            'End If
                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            lrFactTypeInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            lrFactTypeInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)

                            'FactTypeReadingPosition
                            Try
                                Dim lrFTRXElement As XElement = (lrObjectTypeShapeXElement.<ormDiagram:RelativeShapes>.<ormDiagram:ReadingShape>)(0)
                                If lrFTRXElement IsNot Nothing Then
                                    lsBounds = lrFTRXElement.Attribute("AbsoluteBounds").Value.Split(",")
                                    lrFactTypeInstance.FactTypeReadingPoint = New Point(Int(CSng(Trim(lsBounds(0))) * ldblScalar),
                                                                                        Int(CSng(Trim(lsBounds(1))) * ldblScalar))
                                End If
                            Catch ex As Exception
                                'Not a biggie
                            End Try
                        End If
#End Region
                    Next 'lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ObjectTypeShape>

                    '-----------------------------------
                    'Add FactTypeInstances to the Page
                    '-----------------------------------
#Region "FactTypes other"
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:FactTypeShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        '--------------------------------
                        'Find the FactType in the Model
                        '--------------------------------
                        lrFactType = New FBM.FactType
                        lrFactType.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        loEnumElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>
                                                   Where ModelInformation.Attribute("id") = lrFactType.NORMAReferenceId

                        lrFactType = arModel.FactType.Find(Function(x) x.NORMAReferenceId = lrFactType.NORMAReferenceId)

                        If IsSomething(lrFactType) Then
                            '------------------------------------
                            'Clone an Instance of the FactType
                            '------------------------------------
                            '20220127-VM-Commented out below. Remove if all okay.
                            'lrFactTypeInstance = New FBM.FactTypeInstance(arModel, lrPage, pcenumLanguage.ORMModel)                            
                            lrFactTypeInstance = New FBM.FactTypeInstance(arModel, lrPage, pcenumLanguage.ORMModel, lrFactType.Id, True) ' lrFactType.CloneInstance(lrPage, False)
                            lrFactTypeInstance.FactType = lrFactType
                            Boston.WriteToStatusBar("Loading Fact Type Instance: '" & lrFactTypeInstance.Name & "'")
                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            lrFactTypeInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            lrFactTypeInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)

                            'FactTypeReadingPosition
                            Try
                                Dim lrFTRXElement As XElement = (lrObjectTypeShapeXElement.<ormDiagram:RelativeShapes>.<ormDiagram:ReadingShape>)(0)
                                If lrFTRXElement IsNot Nothing Then
                                    lsBounds = lrFTRXElement.Attribute("AbsoluteBounds").Value.Split(",")
                                    lrFactTypeInstance.FactTypeReadingPoint = New Point(Int(CSng(Trim(lsBounds(0))) * ldblScalar),
                                                                                        Int(CSng(Trim(lsBounds(1))) * ldblScalar))
                                End If
                            Catch ex As Exception
                                'Not a biggie
                            End Try


                            '-----------------------------------------------------------------------
                            'If the FactTypeInstance doesn't exist on the Page, add it to the Page
                            '-----------------------------------------------------------------------
                            '20220715-VM-Commented out check to see if already on the Page. New InstanceNumber facility now in place.
                            'If Not lrPage.FactTypeInstance.Exists(AddressOf lrFactTypeInstance.Equals) Then

                            '-----------------------------------------------------------------------------
                            'Add the FactTypeInstance to the Page, because Role.CloneInstance
                            '  automatically looks for and adds the ROleInstance to the FactTypeInstance
                            '-----------------------------------------------------------------------------
                            lrFactTypeInstance2 = lrPage.DropFactTypeAtPoint(lrFactTypeInstance.FactType, New PointF(lrFactTypeInstance.X, lrFactTypeInstance.Y), False, False, False, False)
                            lrFactTypeInstance2.FactTypeReadingPoint = lrFactTypeInstance.FactTypeReadingPoint

                            '------------------------------------
                            'Load the FactInstances to the Page
                            '------------------------------------
                            For Each lrFact In lrFactType.Fact
                                lrFactTypeInstance.AddFact(lrFact, False)
                            Next
                            'Else
                            'lrPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals).Move(lrFactTypeInstance.X, lrFactTypeInstance.Y, False)
                            'End If
                        End If 'IsSomething(lrFactType)
                    Next 'FactTypeShape in NORMA XML 
#End Region

                    Dim larFaultyFactTypeInstances = From Page In arModel.Page
                                                     From FactTypeInstance In Page.FactTypeInstance
                                                     From Role In FactTypeInstance.RoleGroup
                                                     Where Page.PageId = lrPage.PageId _
                                                       And Role.JoinedORMObject Is Nothing
                                                     Select FactTypeInstance

                    For Each lrFactTypeInstance In larFaultyFactTypeInstances

                        Dim larRoleInstance = From RoleInstance In lrFactTypeInstance.RoleGroup
                                              Where RoleInstance.JoinedORMObject Is Nothing
                                              Select RoleInstance

                        For Each lrRoleInstance In larRoleInstance

                            '------------------------------------------------
                            'Find the ModelObject within the Richmond Model
                            '------------------------------------------------
                            Dim lrJoinedFactTypeInstance As FBM.FactTypeInstance
                            lrJoinedFactTypeInstance = New FBM.FactTypeInstance
                            lrJoinedFactTypeInstance.Id = lrRoleInstance.JoinsFactType.Id
                            lrJoinedFactTypeInstance = lrPage.FactTypeInstance.Find(AddressOf lrJoinedFactTypeInstance.Equals)

                            lrRoleInstance.JoinedORMObject = lrJoinedFactTypeInstance

                            'MsgBox("Aha found one")


                        Next 'Role
                    Next 'FaultyFactTypeInstance

                    '-----------------------
                    'Subtype Relationships
                    '-----------------------
#Region "Subtype Relationships"
                    Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance = Nothing
                    For Each lrEntityTypeInstance In lrPage.EntityTypeInstance.FindAll(Function(x) x.EntityType.SubtypeRelationship.Count > 0).ToArray
                        For Each lrSubtypeRelationship In lrEntityTypeInstance.EntityType.SubtypeRelationship
                            Dim lrParentModelElement As FBM.ModelObject
                            lrParentModelElement = lrPage.EntityTypeInstance.Find(Function(x) x.Id = lrSubtypeRelationship.parentModelElement.Id)
                            If lrParentModelElement IsNot Nothing Then
                                If lrPage.FactTypeInstance.Find(Function(x) x.Id = lrSubtypeRelationship.FactType.Id) Is Nothing Then
                                    Call lrPage.DropFactTypeAtPoint(lrSubtypeRelationship.FactType, New PointF(10, 10), False, False, False, False)
                                End If
                                lrSubtypeRelationshipInstance = lrSubtypeRelationship.CloneInstance(lrPage, False)
                                'New FBM.SubtypeRelationshipInstance(lrPage, lrEntityTypeInstance, lrPage.EntityTypeInstance.Find(AddressOf lrParentEntityTypeInstance.Equals))
                                lrEntityTypeInstance.SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)
                                lrPage.SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)
                            End If
                        Next
                    Next

                    For Each lrValueTypeInstance In lrPage.ValueTypeInstance.FindAll(Function(x) x.ValueType.SubtypeRelationship.Count > 0).ToArray
                        For Each lrSubtypeRelationship In lrValueTypeInstance.ValueType.SubtypeRelationship
                            Try
                                Dim lrParentModelElement As FBM.ModelObject
                                lrParentModelElement = lrPage.getModelElementById(lrSubtypeRelationship.parentModelElement.Id)
                                If lrParentModelElement IsNot Nothing Then
                                    If lrPage.FactTypeInstance.Find(Function(x) x.Id = lrSubtypeRelationship.FactType.Id) Is Nothing Then
                                        Call lrPage.DropFactTypeAtPoint(lrSubtypeRelationship.FactType, New PointF(10, 10), False, False, False, False)
                                    End If
                                    lrSubtypeRelationshipInstance = lrSubtypeRelationship.CloneInstance(lrPage, False)
                                    'New FBM.SubtypeRelationshipInstance(lrPage, lrEntityTypeInstance, lrPage.EntityTypeInstance.Find(AddressOf lrParentEntityTypeInstance.Equals))
                                    lrValueTypeInstance.SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)
                                    lrPage.SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)
                                End If
                            Catch ex As Exception
                                lsMessage = "Trouble loading Subtype Relationship Instance."
                                lsMessage.AppendDoubleLineBreak("Page: " & lrPage.Name)
                                Try
                                    lsMessage.AppendLine("Subtype: " & lrSubtypeRelationshipInstance.ModelElement.Id)
                                    lsMessage.AppendLine("Supertype: " & lrSubtypeRelationshipInstance.parentModelElement.Id)
                                Catch ex1 As Exception
                                    'Not a biggie.
                                End Try
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False,, True)
                            End Try
                        Next
                    Next
#End Region

                    '-----------------------------------------------------------
                    'Internal Uniqueness Constraints (RoleConstraintInstances)
                    '-----------------------------------------------------------
#Region "Internal Uniqueness Constraints"
                    '20220127-VM-Commented out. Remove if all okay.

                    'Dim lbCanAddRoleConstraintToPage As Boolean = True

                    'For Each lrRoleConstraint In arModel.RoleConstraint
                    '    If lrRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    '        '--------------------------------------------------------------------------------------------
                    '        'If all of the Roles in the RoleConstraintRole group for the RoleConstraint
                    '        '  are on the Page, then add the RoleConstraint to the Page as a new RoleConstraintInstance
                    '        '--------------------------------------------------------------------------------------------
                    '        lbCanAddRoleConstraintToPage = True
                    '        For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole
                    '            lrRoleInstance = New FBM.RoleInstance(arModel, lrPage, lrRoleConstraintRole.Role)
                    '            lrRoleInstance = lrPage.RoleInstance.Find(AddressOf lrRoleInstance.Equals)
                    '            If IsSomething(lrRoleInstance) Then
                    '                '---------------------------------------
                    '                'Okay, the RoleInstance is on the Page
                    '                '---------------------------------------
                    '                'MsgBox("Found something at least")
                    '            Else
                    '                lbCanAddRoleConstraintToPage = False
                    '            End If
                    '        Next
                    '        If lbCanAddRoleConstraintToPage Then
                    '            lrPage.RoleConstraintInstance.Add(lrRoleConstraint.CloneInstance(lrPage))
                    '            'MsgBox("Adding")
                    '        End If
                    '    End If
                    'Next
#End Region

                    '------------------
                    'Model Notes
                    '------------------
#Region "ModelNote"
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ModelNoteShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        lrModelNote = New FBM.ModelNote
                        lrModelNote.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        lrModelNote = arModel.ModelNote.Find(Function(x) x.NORMAReferenceId = lrModelNote.NORMAReferenceId)

                        lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                        Dim loPointF As New PointF(Int(CSng(Trim(lsBounds(0))) * ldblScalar), Int(CSng(Trim(lsBounds(1))) * ldblScalar))
                        Call lrPage.DropModelNoteAtPoint(lrModelNote, loPointF)
                    Next
#End Region
                    '------------------
                    'Ring Constraints
                    '------------------
#Region "RingConstraints"
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:RingConstraintShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        Dim loXMLElementQueryResult As IEnumerable(Of XElement)
                        '-----------------------
                        'Ring Constraint
                        '-----------------------
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value
                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:RingConstraint>
                                                  Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            lrRoleConstraint.Name = loXMLElementQueryResult(0).Attribute("Name")
                            lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                            'Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.RingConstraint)
                            'lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage, False)
                            'lrRoleConstraintInstance = lrRoleConstraintInstance.CloneRingConstraintInstance(lrPage)

                            'If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                            '    lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                            'Else
                            '    Boston.WriteToStatusBar("Loading Ring Constraint Instance")
                            '    lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                            'End If
                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            'lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            'lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)

                            Dim loPointF As New PointF(Int(CSng(Trim(lsBounds(0))) * ldblScalar), Int(CSng(Trim(lsBounds(1))) * ldblScalar))
                            Call lrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPointF, False)

                        Else
                            lrRoleConstraint = Nothing
                        End If
                    Next
#End Region

                    '------------------
                    'Frequency Constraints
                    '------------------
#Region "Frequency Constraints"
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:FrequencyConstraintShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        Dim loXMLElementQueryResult As IEnumerable(Of XElement)
                        '-----------------------
                        'Frequency Constraint
                        '-----------------------
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value
                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:FrequencyConstraint>
                                                  Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            lrRoleConstraint.Name = loXMLElementQueryResult(0).Attribute("Name")
                            lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                            Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
                            lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)
                            lrRoleConstraintInstance = lrRoleConstraintInstance.CloneFrequencyConstraintInstance(lrPage)
                            'lrRoleConstraintInstance.MinimumFrequencyCount = lrRoleConstraint.MinimumFrequencyCount
                            'lrRoleConstraintInstance.MaximumFrequencyCount = lrRoleConstraint.MaximumFrequencyCount

                            If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                                lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                            Else
                                Boston.WriteToStatusBar("Loading Ring Constraint Instance")
                                lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                            End If
                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                        Else
                            lrRoleConstraint = Nothing
                        End If

                    Next
#End Region

                    '------------------
                    'Value Constraints
                    '------------------
#Region "ValueConstraints"
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:FactTypeShape>.<ormDiagram:RelativeShapes>.<ormDiagram:ValueConstraintShape>

                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        '-----------------------
                        'Frequency Constraint
                        '-----------------------
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                        Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
                        Select Case lrRoleConstraint.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.ValueConstraint
                                lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)
                                lrRoleConstraintInstance = lrRoleConstraintInstance.CloneValueConstraintInstance(lrPage)
                            Case Is = pcenumRoleConstraintType.RoleValueConstraint
                                lrRoleConstraintInstance = lrRoleConstraint.CloneRoleValueConstraintInstance(lrPage)
                        End Select

                        If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                            lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                        Else
                            Boston.WriteToStatusBar("Loading Ring Constraint Instance")
                            lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                        End If
                        lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                        lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                        lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                    Next
#End Region

                    '--------------------------
                    'Role Value Constraints 1
                    '--------------------------
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ObjectTypeShape>.<ormDiagram:RelativeShapes>.<ormDiagram:ValueConstraintShape>

                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        '----------------------
                        'Role Value Constraint
                        '----------------------
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                        If lrRoleConstraint IsNot Nothing Then
                            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                            lrRoleConstraintInstance = lrRoleConstraint.CloneRoleValueConstraintInstance(lrPage)

                            If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                                lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                            Else
                                Boston.WriteToStatusBar("Loading Role Value Constraint Instance")
                                lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                            End If
                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                        End If
                    Next

                    '--------------------------
                    'Role Value Constraints 2
                    '--------------------------
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:FactTypeShape>.<ormDiagram:RelativeShapes>.<ormDiagram:ValueConstraintShape>

                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                        If lrRoleConstraint IsNot Nothing Then
                            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                            lrRoleConstraintInstance = lrRoleConstraint.CloneRoleValueConstraintInstance(lrPage)

                            If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                                lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                            Else
                                Boston.WriteToStatusBar("Loading Role Value Constraint Instance")
                                lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                            End If
                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                        End If
                    Next

                    '---------------------------------
                    'External Uniqueness Constraints
                    '---------------------------------
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ExternalConstraintShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        Dim loXMLElementQueryResult As IEnumerable(Of XElement)
                        '--------------------------------
                        'External Uniqueness Constraint
                        '--------------------------------
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value
                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:UniquenessConstraint>
                                                  Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            lrRoleConstraint.Name = loXMLElementQueryResult(0).Attribute("Name")
                            lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                            'Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.ExternalUniquenessConstraint)
                            'lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)

                            'If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                            '    lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                            'Else
                            '    Boston.WriteToStatusBar("Loading Subset Constraint Instance")
                            '    lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                            'End If
                            'lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            'lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            'lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)

                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")

                            Dim loPointF As New PointF(Int(CSng(Trim(lsBounds(0))) * ldblScalar), Int(CSng(Trim(lsBounds(1))) * ldblScalar))
                            Call lrPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPointF, False)
                        Else
                            lrRoleConstraint = Nothing
                        End If
                    Next

                    '------------------
                    'Subset Constraints
                    '------------------
                    Try

                        For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ExternalConstraintShape>
                            lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                            Dim loXMLElementQueryResult As IEnumerable(Of XElement)
                            '-----------------------
                            'Subset Constraint
                            '-----------------------
                            lrRoleConstraint = New FBM.RoleConstraint
                            lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                            loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:SubsetConstraint>
                                                      Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                            If IsSomething(loXMLElementQueryResult(0)) Then

                                lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                                Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.SubsetConstraint)
                                lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)

                                If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                                    lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                                Else
                                    Boston.WriteToStatusBar("Loading Subset Constraint Instance")
                                    lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                                End If
                                lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                                lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                                lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                            Else
                                lrRoleConstraint = Nothing
                            End If
                        Next
                    Catch ex As Exception
                        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                        lsMessage &= vbCrLf & vbCrLf & ex.Message
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                    End Try

                    '------------------
                    'InclusiveOR Constraints
                    '------------------
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ExternalConstraintShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        Dim loXMLElementQueryResult As IEnumerable(Of XElement)
                        '-----------------------
                        'EntityType
                        '-----------------------
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:MandatoryConstraint>
                                                  Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            If loXMLElementQueryResult.<orm:ExclusiveOrExclusionConstraint>.Count = 0 Then
                                lrRoleConstraint.Name = loXMLElementQueryResult(0).Attribute("Name")
                                lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                                Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.InclusiveORConstraint)
                                lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)

                                If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                                    lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                                Else
                                    Boston.WriteToStatusBar("Loading Subset Constraint Instance")
                                    lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                                End If
                                lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                                lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                                lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                            Else
                                lrRoleConstraint = Nothing
                            End If
                        Else
                            lrRoleConstraint = Nothing
                        End If
                    Next

                    '-------------------------
                    'ExclusiveOR Constraints
                    '-------------------------
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ExternalConstraintShape>

                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        Dim loXMLElementQueryResult As IEnumerable(Of XElement)
                        '-----------------------
                        'ExclusiveOR
                        '-----------------------
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:MandatoryConstraint>
                                                  Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                        If loXMLElementQueryResult.<orm:ExclusiveOrExclusionConstraint>.Count = 1 Then
                            If IsSomething(loXMLElementQueryResult(0)) Then

                                lrRoleConstraint.NORMAReferenceId = loXMLElementQueryResult(0).<orm:ExclusiveOrExclusionConstraint>(0).Attribute("ref").Value

                                loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:ExclusionConstraint>
                                                          Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                                lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                                '------------------------------------------------------------------------------------------------------------
                                'CodeSafe
                                'Add FactTypes to Page for RoleConstraintRoles where FactType is not already on the Page.
                                For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole
                                    lrFactType = lrRoleConstraintRole.Role.FactType
                                    If lrPage.FactTypeInstance.Find(Function(x) x.Id = lrFactType.Id) Is Nothing Then
                                        Call lrPage.DropFactTypeAtPoint(lrFactType, New PointF(10, 10), False, False, False, False)
                                    End If
                                Next

                                Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.ExclusiveORConstraint)
                                lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)

                                If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                                    lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                                Else
                                    Boston.WriteToStatusBar("Loading Subset Constraint Instance")
                                    lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                                End If
                                lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                                lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                                lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                            Else
                                lrRoleConstraint = Nothing
                            End If
                        Else
                            lrRoleConstraint = Nothing
                        End If
                    Next

                    '------------------
                    'Exclusion Constraints
                    '------------------
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ExternalConstraintShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        Dim loXMLElementQueryResult As IEnumerable(Of XElement)

                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:ExclusionConstraint>
                                                  Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then

                            Try
                                lrRoleConstraint.Name = loXMLElementQueryResult(0).Attribute("Name")
                                lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                                Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.ExclusionConstraint)
                                lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)

                                If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                                    lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                                Else
                                    Boston.WriteToStatusBar("Loading Subset Constraint Instance")
                                    lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                                End If
                                lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                                lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                                lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                            Catch ex As Exception
                                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                                lsMessage = "Error loading Exclusion Constraint instance."
                                lsMessage.AppendLine("Role Constraint Name:" & lrRoleConstraint.Name)
                                lsMessage.AppendLine("NORMA Reference Id: " & lrRoleConstraint.NORMAReferenceId)
                                lsMessage.AppendLine("Page Name: " & lrPageXElement.Attribute("Name").Value)
                                lsMessage.AppendLine("")
                                lsMessage &= "Error: " & mb.ReflectedType.Name & "." & mb.Name
                                lsMessage &= vbCrLf & vbCrLf & ex.Message
                                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace, True, False, True)
                            End Try
                        Else
                            lrRoleConstraint = Nothing
                        End If
                    Next

                    '------------------
                    'Equality Constraints
                    '------------------
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ExternalConstraintShape>
                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        Dim loXMLElementQueryResult As IEnumerable(Of XElement)
                        '-----------------------
                        'EntityType
                        '-----------------------
                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        loXMLElementQueryResult = From ModelInformation In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Constraints>.<orm:EqualityConstraint>
                                                  Where ModelInformation.Attribute("id") = lrRoleConstraint.NORMAReferenceId

                        If IsSomething(loXMLElementQueryResult(0)) Then
                            lrRoleConstraint.Name = loXMLElementQueryResult(0).Attribute("Name")
                            lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                            Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.EqualityConstraint)
                            lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)

                            If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                                lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                            Else
                                Boston.WriteToStatusBar("Loading Subset Constraint Instance")
                                lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                            End If
                            lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                            lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                            lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)

                        Else
                            lrRoleConstraint = Nothing
                        End If
                    Next

                    '------------------------------
                    'Value Comparison Constraints
                    '------------------------------
                    For Each lrObjectTypeShapeXElement In lrPageXElement.<ormDiagram:Shapes>.<ormDiagram:ValueComparisonConstraintShape>

                        lrObjectTypeXElement = lrObjectTypeShapeXElement.<ormDiagram:Subject>(0)

                        lrRoleConstraint = New FBM.RoleConstraint
                        lrRoleConstraint.NORMAReferenceId = lrObjectTypeXElement.Attribute("ref").Value

                        lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.NORMAReferenceId = lrRoleConstraint.NORMAReferenceId)

                        '------------------------------------------------------------------------------------------------------------
                        'CodeSafe
                        'Add FactTypes to Page for RoleConstraintRoles where FactType is not already on the Page.
                        For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole
                            lrFactType = lrRoleConstraintRole.Role.FactType
                            If lrPage.FactTypeInstance.Find(Function(x) x.Id = lrFactType.Id) Is Nothing Then
                                Call lrPage.DropFactTypeAtPoint(lrFactType, New PointF(10, 10), False, False, False, False)
                            End If
                        Next

                        Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance(pcenumRoleConstraintType.ValueComparisonConstraint)
                        lrRoleConstraintInstance = lrRoleConstraint.CloneInstance(lrPage)

                        If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                            lrRoleConstraintInstance = lrPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                        Else
                            Boston.WriteToStatusBar("Loading Subset Constraint Instance")
                            lrPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)
                        End If
                        lsBounds = lrObjectTypeShapeXElement.Attribute("AbsoluteBounds").Value.Split(",")
                        lrRoleConstraintInstance.X = Int(CSng(Trim(lsBounds(0))) * ldblScalar)
                        lrRoleConstraintInstance.Y = Int(CSng(Trim(lsBounds(1))) * ldblScalar)
                    Next

                    'GoTo SkipToArea
                    'SkipToArea:

                    lrPage.IsDirty = True
                Next 'Page in NORMA XML

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub LoadFactTypeReadings(ByRef arModel As FBM.Model, ByRef arFactType As FBM.FactType, ByRef arElement As XElement)

            '--------------------------
            'Get the FactTypeReadings
            '--------------------------
            Dim lrFactTypeReadingXElement As XElement
            Dim lrPredicatePartXElement As XElement
            Dim lrPredicatePartRoleXElement As XElement
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lasPredicateParts() As String
            Dim lrPredicatePart As FBM.PredicatePart = Nothing
            Dim lrRoleHashTable As New Hashtable()

            Try
                For Each lrFactTypeReadingXElement In arElement.<orm:ReadingOrders>.<orm:ReadingOrder>

                    For Each lrPredicatePartXElement In lrFactTypeReadingXElement.<orm:Readings>.<orm:Reading>.<orm:Data>

                        '20220126-VM-Was above the 'For each' above. Next two lines.
                        lrFactTypeReading = New FBM.FactTypeReading(arFactType)
                        lrFactTypeReading.isDirty = True

                        'MsgBox(lrPredicatePartXElement.Value)
                        Dim lsNORMAPredicateList As New Regex("(\{.*?\})|([a-z][A-Z]\s)")
                        lasPredicateParts = lsNORMAPredicateList.Split(lrPredicatePartXElement.Value).Where(Function(s) Not String.IsNullOrEmpty(s)).ToArray()

                        Dim liRoleSequenceNr As Integer = 0
                        lrRoleHashTable.Clear()
                        For Each lrPredicatePartRoleXElement In lrFactTypeReadingXElement.<orm:RoleSequence>.<orm:Role>
                            lrRoleHashTable.Add("{" & liRoleSequenceNr.ToString & "}", lrPredicatePartRoleXElement.Attribute("ref").Value)
                            liRoleSequenceNr += 1
                        Next

                        '-------------------------------------------------------------------------------------------
                        'Perform Left-2-Right parsing to get the PredicateParts
                        '--------------------------------------------------------
                        Dim liSequenceNr As Integer = 0
                        Dim liPredicatePartSequenceNr As Integer = 1
                        Dim lsPredicatePartText As String = ""
                        Dim lsPrefix As String = ""
                        Dim lsSuffix As String = ""
                        Dim lrPredicateRole As FBM.Role

                        For Each lsNORMAPredicatePart In lasPredicateParts
                            '----------------------------------------
                            'Check to see if the word is one of the
                            '  ORM Object Types within the reading
                            '----------------------------------------      
                            Dim liCharPosition As Integer = 0
                            Dim lsNORMAPredicatePart2 As String = ""

                            'Get rid of - characters inside the predicate
                            For Each lsChar In Trim(lsNORMAPredicatePart)
                                If liCharPosition > 0 And liCharPosition < Trim(lsNORMAPredicatePart).Length - 1 Then
                                    If lsChar <> "-" Then
                                        lsNORMAPredicatePart2 &= lsChar
                                    End If
                                Else
                                    lsNORMAPredicatePart2 &= lsChar
                                End If
                                liCharPosition += 1
                            Next

                            lsNORMAPredicatePart2 = LCase(lsNORMAPredicatePart2)

                            'If lrHashList.Contains(lsPredicatePart) Then
                            If lsNORMAPredicatePart2 Like "{#}" Then
                                '----------------------------
                                'Create a new PredicatePart
                                '----------------------------
                                lrPredicatePart = New FBM.PredicatePart(lrFactTypeReading.Model, lrFactTypeReading)
                                lrPredicatePart.isDirty = True

                                liPredicatePartSequenceNr += 1
                                lrPredicatePart.SequenceNr = liPredicatePartSequenceNr

                                lrPredicateRole = New FBM.Role(arFactType, lrRoleHashTable(lsNORMAPredicatePart2), True)
                                lrPredicateRole = arModel.Role.Find(AddressOf lrPredicateRole.Equals)
                                lrPredicatePart.Role = lrPredicateRole

                                lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                            Else
                                If lrPredicatePart Is Nothing Then
                                    If liPredicatePartSequenceNr = 1 Then
                                        lrFactTypeReading.FrontText = Trim(lsNORMAPredicatePart2)
                                    Else
                                        lrFactTypeReading.FollowingText = Trim(lsNORMAPredicatePart2)
                                    End If
                                Else
                                    lsPredicatePartText = lsNORMAPredicatePart2
                                    lrPredicatePart.PredicatePartText = Trim(lsPredicatePartText)
                                End If
                            End If
                            liPredicatePartSequenceNr += 1
                        Next

                        '----------------------------------------------------------------------
                        '20220126-VM-This section was below the 'Next' below (for <orm:Data>)
                        'Get PreBoundReadingTexts etc
                        Dim larRoleOrder As New List(Of FBM.Role)
                        For Each lrPredicatePart In lrFactTypeReading.PredicatePart
                            larRoleOrder.Add(lrPredicatePart.Role)
                        Next

                        Dim lsFactTypeReading = lrFactTypeReading.GetReadingText
                        lrFactTypeReading.PredicatePart.Clear()
                        Call Me.GetPredicatePartsFromReadingUsingParser(lsFactTypeReading, lrFactTypeReading, larRoleOrder)

                        arFactType.FactTypeReading.Add(lrFactTypeReading)
                        '----------------------------------------------------------------------

                    Next 'FactTypeReading.PredicatePart (<orm:Data>)

                Next 'FactTypeReading

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub LoadFactTypeFacts(ByRef arFactType As FBM.FactType, ByRef arNORMAXMLDOC As XDocument, ByRef arElement As XElement)

            '---------------------------------
            'Load the Facts for the FactType
            '---------------------------------
            Dim lrRole As FBM.Role
            Dim lrFact As FBM.Fact
            Dim lrFactData As FBM.FactData
            Dim lrFactXElement As XElement
            Dim lrFactDataXElement As XElement
            Dim lrFactTypeRoleInstance As XElement
            Dim lrFactDataData As IEnumerable(Of XElement)
            Dim lrEntityTypeRoleInstance As XElement

            Try
                For Each lrFactXElement In arElement.<orm:Instances>.<orm:FactTypeInstance>
                    lrFact = New FBM.Fact(arFactType)

                    For Each lrFactDataXElement In lrFactXElement.<orm:RoleInstances>.<orm:FactTypeRoleInstance>

                        Dim lrNORMAFactTypeRoleInstance = From RoleInstance In arElement.<orm:FactRoles>.<orm:Role>.<orm:RoleInstances>.<orm:FactTypeRoleInstance>
                                                          Where RoleInstance.Attribute("id").Value = lrFactDataXElement.Attribute("ref").Value

                        For Each lrFactTypeRoleInstance In lrNORMAFactTypeRoleInstance
                            lrRole = New FBM.Role
                            lrRole.Id = lrFactTypeRoleInstance.Parent.Parent.Attribute("id").Value
                            lrRole = arFactType.RoleGroup.Find(AddressOf lrRole.Equals)

                            '-----------------------------------------------------------------------------
                            'Set the initial value of the FactData to the NORMA FactTypeRoleInstance.ref
                            '  NB This isn't the actual Data (in NORMA) but a reference to the Data
                            '-----------------------------------------------------------------------------
                            lrFactData = New FBM.FactData(lrRole, New FBM.Concept(lrFactTypeRoleInstance.Attribute("ref").Value), lrFact)
                            lrFactData.Id = lrFactDataXElement.Attribute("ref").Value

                            Select Case lrRole.TypeOfJoin
                                Case Is = pcenumRoleJoinType.ValueType
                                    lrFactDataData = From ValueType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ValueType>.<orm:Instances>.<orm:ValueTypeInstance>
                                                     Where ValueType.Attribute("id").Value = lrFactData.Data

                                    If lrFactDataData.<orm:Value>.Value = "" Then
                                        lrFactData.Data = "a"
                                    Else
                                        lrFactData.Data = lrFactDataData.<orm:Value>.Value
                                    End If

                                    lrFact.Data.Add(lrFactData)
                                Case Is = pcenumRoleJoinType.EntityType

                                    lrFactDataData = From ValueType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:EntityType>.<orm:Instances>.<orm:EntityTypeInstance>
                                                     Where ValueType.Attribute("id").Value = lrFactData.Data

                                    If lrFactDataData.Any Then
                                        Dim lrEntityTypeRoleInstanceXAttributeList As IEnumerable(Of XElement)
                                        Dim lrEntityTypeRoleInstanceXAttribute As XElement
                                        lrEntityTypeRoleInstanceXAttributeList = From EntityTypeRoleInstance In lrFactDataData.<orm:RoleInstances>.<orm:EntityTypeRoleInstance>
                                                                                 Select EntityTypeRoleInstance

                                        lrEntityTypeRoleInstanceXAttribute = lrEntityTypeRoleInstanceXAttributeList(0)
                                        lrFactData.Data = lrEntityTypeRoleInstanceXAttribute.Attribute("ref").Value

                                        lrFactDataData = From RoleInstance In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>.<orm:FactRoles>.<orm:Role>.<orm:RoleInstances>.<orm:EntityTypeRoleInstance>
                                                         Where RoleInstance.Attribute("id").Value = lrFactData.Data
                                                         Select RoleInstance

                                        lrEntityTypeRoleInstanceXAttribute = lrFactDataData(0)
                                        lrFactData.Data = lrEntityTypeRoleInstanceXAttribute.Attribute("ref").Value

                                        lrFactDataData = From ValueType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ValueType>.<orm:Instances>.<orm:ValueTypeInstance>
                                                         Where ValueType.Attribute("id").Value = lrFactData.Data

                                        lrFactData.Data = lrFactDataData.<orm:Value>.Value
                                    Else
                                        lrFactData.Data = "a"
                                    End If

                                    lrFact.Data.Add(lrFactData)

                                Case Is = pcenumRoleJoinType.FactType

                                    lrFactDataData = From ValueType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ObjectifiedType>.<orm:Instances>.<orm:EntityTypeInstance>
                                                     Where ValueType.Attribute("id").Value = lrFactData.Data

                                    Dim lrEntityTypeRoleInstanceXAttributeList As IEnumerable(Of XElement)
                                    Dim lrEntityTypeRoleInstanceXAttribute As XElement
                                    lrEntityTypeRoleInstanceXAttributeList = From EntityTypeRoleInstance In lrFactDataData.<orm:RoleInstances>.<orm:EntityTypeRoleInstance>
                                                                             Select EntityTypeRoleInstance

                                    Dim lrObjectifiedObjectTypeFact As New FBM.Fact(lrRole.JoinedORMObject)
                                    Dim lrObjectifiedObjectTypeFactData As FBM.FactData

                                    For Each lrEntityTypeRoleInstance In lrEntityTypeRoleInstanceXAttributeList

                                        lrObjectifiedObjectTypeFactData = New FBM.FactData(lrRole, New FBM.Concept(""), lrObjectifiedObjectTypeFact)

                                        lrObjectifiedObjectTypeFactData.Data = lrEntityTypeRoleInstance.Attribute("ref").Value

                                        lrFactDataData = From RoleInstance In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>.<orm:FactRoles>.<orm:Role>.<orm:RoleInstances>.<orm:EntityTypeRoleInstance>
                                                         Where RoleInstance.Attribute("id").Value = lrObjectifiedObjectTypeFactData.Data
                                                         Select RoleInstance

                                        lrEntityTypeRoleInstanceXAttribute = lrFactDataData(0)
                                        lrObjectifiedObjectTypeFactData.Data = lrEntityTypeRoleInstanceXAttribute.Attribute("ref").Value

                                        lrFactDataData = From ValueType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:EntityType>.<orm:Instances>.<orm:EntityTypeInstance>
                                                         Where ValueType.Attribute("id").Value = lrObjectifiedObjectTypeFactData.Data

                                        lrEntityTypeRoleInstanceXAttributeList = From EntityTypeRoleInstance In lrFactDataData.<orm:RoleInstances>.<orm:EntityTypeRoleInstance>
                                                                                 Select EntityTypeRoleInstance

                                        lrEntityTypeRoleInstanceXAttribute = lrEntityTypeRoleInstanceXAttributeList(0)
                                        lrObjectifiedObjectTypeFactData.Data = lrEntityTypeRoleInstanceXAttribute.Attribute("ref").Value

                                        lrFactDataData = From RoleInstance In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Facts>.<orm:Fact>.<orm:FactRoles>.<orm:Role>.<orm:RoleInstances>.<orm:EntityTypeRoleInstance>
                                                         Where RoleInstance.Attribute("id").Value = lrObjectifiedObjectTypeFactData.Data
                                                         Select RoleInstance

                                        lrEntityTypeRoleInstanceXAttribute = lrFactDataData(0)
                                        lrObjectifiedObjectTypeFactData.Data = lrEntityTypeRoleInstanceXAttribute.Attribute("ref").Value

                                        lrFactDataData = From ValueType In arNORMAXMLDOC.Elements.<orm:ORMModel>.<orm:Objects>.<orm:ValueType>.<orm:Instances>.<orm:ValueTypeInstance>
                                                         Where ValueType.Attribute("id").Value = lrObjectifiedObjectTypeFactData.Data

                                        lrObjectifiedObjectTypeFactData.Data = lrFactDataData.<orm:Value>.Value

                                        lrObjectifiedObjectTypeFact.Data.Add(lrObjectifiedObjectTypeFactData)
                                    Next

                                    lrObjectifiedObjectTypeFact = lrRole.JoinsFactType.Fact.Find(AddressOf lrObjectifiedObjectTypeFact.EqualsByData)

                                    If IsSomething(lrObjectifiedObjectTypeFact) Then
                                        lrFactData.Data = lrObjectifiedObjectTypeFact.Id
                                    Else
                                        If lrRole.JoinsFactType.Fact.Count > 0 Then
                                            lrFactData.Data = lrRole.JoinsFactType.Fact(0).Id
                                        Else
                                            '-----------------------------------
                                            'Need to abort loading of the Fact
                                            '-----------------------------------                                            
                                        End If
                                    End If
                                    lrFact.Data.Add(lrFactData)
                                    '---------------------------------
                                    'End - Select Case Is = FactType
                                    '---------------------------------
                            End Select
                            lrRole.Data.Add(lrFactData)
                        Next 'Role
                    Next 'FactData

                    arFactType.Fact.Add(lrFact)
                    arFactType.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(arFactType.Model, lrFact.Id, pcenumConceptType.Fact))

                Next 'Fact

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Function GetPredicatePartsFromReadingUsingParser(ByVal asReading As String,
                                                                 ByRef arFactTypeReading As FBM.FactTypeReading,
                                                                 Optional ByRef aarRoleOrder As List(Of FBM.Role) = Nothing) As Boolean

            Dim lsMessage As String
            Dim lrPredicatePart As FBM.PredicatePart

            Try
                ''Testing
                'Me.FTRProcessor.ProcessFTR(asReading, Me.FTRParseTree)

                'OR
                asReading = Database.MakeStringSafe(asReading)
                Me.FTRParseTree = Me.FTRParser.Parse(asReading)

                If Me.FTRParseTree.Errors.Count > 0 Then
                    '---------------------------------------------------------------------------------------------------
                    'Is either an incorrectly formatted FactTypeReading, or is not a FactTypeReading Statement at all.
                    '---------------------------------------------------------------------------------------------------
                    lsMessage = "That's not a well formatted Fact Type Reading: '" & asReading & "'"
                    lsMessage &= vbCrLf
                    lsMessage.AppendLine("The correct format to use is:")
                    lsMessage.AppendLine("Object Types, words start with a capital. E.g. Person")
                    lsMessage.AppendLine("Predicates are all lowercase. E.g. is married")
                    lsMessage.AppendDoubleLineBreak("Fact Type: " & arFactTypeReading.FactType.Id)
                    MsgBox(lsMessage)
                    Return False
                Else
                    Me.FTRProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT = New List(Of String)
                    Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT = New List(Of Object)
                    Me.FTRProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE = New List(Of Object)
                    Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = ""
                    Me.FTRProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT = ""
                    Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.FACTTYPEREADINGStatement, Me.FTRParseTree.Nodes(0))

                    Dim lsFrontReadingTextWord As String = ""
                    arFactTypeReading.FrontText = ""
                    For Each lsFrontReadingTextWord In Me.FTRProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT
                        arFactTypeReading.FrontText &= lsFrontReadingTextWord
                    Next
                    arFactTypeReading.FrontText = Trim(arFactTypeReading.FrontText)

                    Dim lrModelElementNode As FTR.ParseNode
                    Dim lrPredicateClauseNode As FTR.ParseNode
                    Dim liInd As Integer = 0
                    Dim lasModelObjectId As New List(Of String)

                    For liInd = 1 To Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count

                        lrPredicatePart = New FBM.PredicatePart(arFactTypeReading.Model, arFactTypeReading)
                        lrPredicatePart.SequenceNr = liInd

                        lrModelElementNode = Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT(liInd - 1)
                        Me.FTRProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                        Me.FTRProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                        Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                        Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.MODELELEMENTClause, lrModelElementNode)

                        '------------------------------------------------------------------------------------------------------
                        'Check to see whether the MODELELEMENTNAME is an Object Type that is actually linked by the FactType.
                        '------------------------------------------------------------------------------------------------------
                        Dim lsModelElementName As String = Trim(Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME)
                        If arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(lsModelElementName) Is Nothing Then
                            '------------------------------------------------------------------------
                            'Fixes a problem with NORMA files where predicates can contain numbers in NORMA.
                            '  E.g. "ModelElement1 has ModelElement2 in position 2" and where '2' is not a ModelElementName in the Model.
                            '  The below will change that to "ModelElement1 has ModelElement2 in position two", which works for Boston.
                            If lsModelElementName.IsNumeric Then
                                Try
                                    Dim lsPredicatePart = CInt(lsModelElementName).ToWords.ToLower
                                    arFactTypeReading.PredicatePart.Last.PredicatePartText &= " " & lsPredicatePart
                                    GoTo SkippedModelElement
                                Catch ex As Exception
                                    'Let Boston report the error below.
                                End Try
                            End If

                            lsMessage = "Having trouble getting the Predicate Parts for a Fact Type Reading." & vbCrLf & vbCrLf
                            lsMessage &= lsModelElementName & " is not the name of an Object Type linked by the Fact Type." & vbCrLf & vbCrLf
                            lsMessage &= "Reading: " & asReading & vbCrLf
                            lsMessage &= "FactTypeId: " & arFactTypeReading.FactType.Id
                            MsgBox(lsMessage)
                            Return False
                        End If

                        lrPredicatePart.PreBoundText = Trim(Me.FTRProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT)
                        lrPredicatePart.PostBoundText = Trim(Me.FTRProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT)

                        Dim lrRole As New FBM.Role(arFactTypeReading.FactType, "TEMP", False, New FBM.ModelObject(lsModelElementName))
                        If arFactTypeReading.FactType.RoleGroup.FindAll(AddressOf lrRole.EqualsByJoinedModelObjectId).Count = 0 Then
                            Return False
                        End If

                        lasModelObjectId.Add(lsModelElementName)
                        Dim lsModelObjectId As String = lsModelElementName
                        Dim lsTempInd As Integer = lasModelObjectId.FindAll(AddressOf lsModelObjectId.Equals).Count

                        If aarRoleOrder Is Nothing Then
                            lrPredicatePart.Role = arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(lsModelElementName,
                                                                                                          lsTempInd)
                        ElseIf aarRoleOrder(liInd - 1).JoinedORMObject.Id = arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(lsModelElementName,
                                                                                                                                   lsTempInd).JoinedORMObject.Id Then
                            lrPredicatePart.Role = aarRoleOrder(liInd - 1)
                        Else
                            lrPredicatePart.Role = arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(lsModelElementName,
                                                                                                          lsTempInd)
                        End If

                        arFactTypeReading.RoleList.Add(lrPredicatePart.Role)
                        Dim lsPredicatePartText As String = ""

                        If Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = "" Then
                            '----------------------------------------
                            'FactType is binary or greater in arity
                            '----------------------------------------
                            If liInd < Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count Then
                                lrPredicateClauseNode = Me.FTRProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE(liInd - 1)
                                Me.FTRProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)
                                Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.PREDICATEPARTClause, lrPredicateClauseNode)

                                For Each lsPredicatePartText In Me.FTRProcessor.PREDICATEPARTClause.PREDICATEPART
                                    lrPredicatePart.PredicatePartText &= lsPredicatePartText
                                Next
                            End If

                            lrPredicatePart.PredicatePartText = Trim(lrPredicatePart.PredicatePartText)
                        Else
                            '------------------------------
                            'FactType is a unary FactType
                            '------------------------------
                            lrPredicatePart.PredicatePartText = Trim(Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART)
                        End If

                        lrPredicatePart.makeDirty()
                        arFactTypeReading.PredicatePart.Add(lrPredicatePart)
SkippedModelElement:
                    Next

                    '-----------------------------------------------
                    'Get the FollowingReadingText if there is one.
                    '-----------------------------------------------
                    arFactTypeReading.FollowingText = Trim(Me.FTRProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT)

                    Return True

                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

    End Class

End Namespace
