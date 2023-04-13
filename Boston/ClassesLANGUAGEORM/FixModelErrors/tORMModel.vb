Imports System.Reflection

Namespace FBM

    Partial Public Class Model

        Public Sub FixErrors(ByVal aaiFixesToApply As List(Of pcenumModelFixType), Optional ByRef arModelElementToFix As Object = Nothing)

            Try

                With New WaitCursor

                    For Each liFixType In aaiFixesToApply

                        Select Case liFixType
                            Case Is = pcenumModelFixType.ObjectifyingEntitTypeIdsNotTheSameAsObjectifiedFactType
                                Call Me.ObjectifyingEntitTypeIdsNotTheSameAsObjectifiedFactType()
                            Case Is = pcenumModelFixType.RolesWithoutJoinedORMObject
                                Call Me.RolesWithoutJoinedORMObject()
                            Case Is = pcenumModelFixType.RelationsInvalidActiveRoleOnOriginColumns
                                Call Me.RelationsInvalidActiveRoleOnOriginColumns()
                            Case Is = pcenumModelFixType.ColumnsWhereActiveRoleIsNothingTryAndFix
                                Call Me.ColumnsWhereActiveRoleIsNothingTryAndFix()
                            Case Is = pcenumModelFixType.ColumnsWhereActiveRoleIsNothingRemoveTheColumn
                                Call Me.ColumnsWhereActiveRoleIsNothingRemoveTheColumn()
                            Case Is = pcenumModelFixType.ColumnsWhereNoLongerPartOfSupertypeHierarchyRemoveColumn
                                Call Me.ColumnsWhereNoLongerPartOfSupertypeHierarchyRemoveColumn()
                            Case Is = pcenumModelFixType.InternalUniquenessConstraintsWhereLevelNumbersAreNotCorrect
                                Call Me.InternalUniquenessConstraintsWhereLevelNumbersAreNotCorrect()
                            Case Is = pcenumModelFixType.ColumnOrdinalPositionsResetWhereOutOfSynchronousOrder
                                Call Me.ColumnOrdinalPositionsResetWhereOutOfSynchronousOrder()
                            Case Is = pcenumModelFixType.RDSColumnsThatShouldBeMandatoryMakeMandatory
                                Call Me.RDSColumnsThatShouldBeMandatoryMakeMandatory()
                            Case Is = pcenumModelFixType.RDSColumnsWithoutActiveRoles
                                Call Me.RDSColumnsWithoutActiveRoles
                            Case Is = pcenumModelFixType.RDSTablesWithNoColumnsRemoveThoseTables
                                Call Me.RDSTablesWithNoColumnsRemoveThoseTables()
                            Case Is = pcenumModelFixType.RemoveFactTypeInstancesFromPageWhereFactTypeIntanceHasRoleInstanceThatJoinsNothing
                                Call Me.RemoveFactTypeInstancesFromPageWhereFactTypeIntanceHasRoleInstanceThatJoinsNothing()
                            Case Is = pcenumModelFixType.RDSTablesWhereColumnAppearsTwiceForSameFactType
                                Call Me.RDSTablesWhereColumnAppearsTwiceForSameFactType
                            Case Is = pcenumModelFixType.RDSTablesWhereTheNumberOfPrimaryKeyColumnsDoesNotMatchTheNumberOfRolesInThePreferredIdentifierFixThat
                                Call Me.RDSTablesWhereTheNumberOfPrimaryKeyColumnsDoesNotMatchTheNumberOfRolesInThePreferredIdentifierFixThat()
                            Case Is = pcenumModelFixType.DuplicateFactsRemoveDuplicates
                                Call Me.DuplicateFactsRemoveDuplicates()
                            Case Is = pcenumModelFixType.RDSTablesAndPGSNodesThatAreMissingRelationsAddTheRelations
                                Call Me.RDSTablesAndPGSNodesThatAreMissingRelationsAddTheRelations()
                            Case Is = pcenumModelFixType.RDSTablesWithMoreThanOneRelationForTheSameFactTypeJoinPruneExtraRelations
                                Call Me.RDSTablesWithMoreThanOneRelationForTheSameFactTypeJoinPruneExtraRelations()
                            Case Is = pcenumModelFixType.RDSRelationsThatHaveNoOriginColumnsRemoveRelation
                                Call Me.RDSRelationsThatHaveNoOriginColumnsRemoveRelation()
                            Case Is = pcenumModelFixType.RDSRelationsThatHaveOriginTableButNoDestinationTableAndViceVersa
                                Call Me.RDSRelationsThatHaveOriginTableButNoDestinationTableAndViceVersa()
                            Case Is = pcenumModelFixType.RDSRelationsWhereOriginColumnCountNotEqualDestinationColumnCount
                                Call Me.RDSRelationsWhereOriginColumnCountNotEqualDestinationColumnCount(arModelElementToFix)
                            Case Is = pcenumModelFixType.SubtypeRelationshipWithNoFactType
                                Call Me.SubtypeRelationshipWithNoFactType()
                            Case Is = pcenumModelFixType.ObjectifiedFactTypesWithNoCorrespondingRDSTable
                                Call Me.ObjectifiedFactTypesWithNoCorrespondingRDSTable
                        End Select

                    Next

                End With


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub ObjectifyingEntitTypeIdsNotTheSameAsObjectifiedFactType()

            Try
                For Each lrFactType In Me.FactType.FindAll(Function(x) x.IsObjectified)
                    If lrFactType.ObjectifyingEntityType.Id <> lrFactType.Id Then
                        Call lrFactType.ObjectifyingEntityType.SetName(lrFactType.Id)
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
        ''' Roles without JoinedORMObject. Remove from Model.
        ''' </summary>
        Private Sub RolesWithoutJoinedORMObject()

            Try
                Dim larFatalRole = From Role In Me.Role
                                   Where Role.JoinedORMObject Is Nothing
                                   Select Role

                For Each lrRole In larFatalRole.ToArray
                    Call lrRole.RemoveFromModel(True, False)
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
        ''' RDS Relations. Invalid ActiveRole on OriginColumns (OriginColumn.ActiveRole is not Destination.ActiveRole).
        ''' </summary>
        Private Sub RelationsInvalidActiveRoleOnOriginColumns()

            Try
                Dim lrIndex As RDS.Index

                Dim larRelation = (From Relation In Me.RDS.Relation
                                   Where Relation.OriginColumns.Count > 0 'Is likely always, but CodeSafe
                                   Where Relation.OriginColumns.Count <> Relation.DestinationColumns.Count
                                   Select Relation).ToList

                For Each lrRelation In larRelation.FindAll(Function(x) x.OriginColumns.Count > 0 And x.DestinationColumns.Count > 0)
                    Dim lrOriginColumn = (From Column In lrRelation.OriginColumns
                                          Select Column
                                          Order By Column.OrdinalPosition).First

                    Dim lrDestinationColumn = (From Column In lrRelation.DestinationColumns
                                               Select Column
                                               Order By Column.OrdinalPosition).First

                    If lrOriginColumn.ActiveRole IsNot lrDestinationColumn.ActiveRole Then
                        'Extra checks
                        If lrRelation.DestinationTable.FBMModelElement.GetType = GetType(FBM.EntityType) And
                                lrRelation.DestinationTable.Index.Find(Function(x) x.IsPrimaryKey) IsNot Nothing Then
                            If lrOriginColumn.Role.FactType Is lrOriginColumn.ActiveRole.FactType Then
                                'The best checks have been done. This is a Relation where the 
                                '  Relation was made before the PrimaryKey was created for the DestinationTable.
                                '  The OriginColumn count <> DestinationColumn count,
                                '  the ActiveRole of the first OriginColumn <> the ActiveRole of the first DestinationColumn,
                                '  and is likely this way for all of the OriginColumns. It needs to be fixed.
                                'Call lrRelation.triggerDestinationColumnsChanged
                                lrIndex = lrRelation.DestinationTable.Index.Find(Function(x) x.IsPrimaryKey)
                                Call lrRelation.DestinationTable_IndexModified(lrIndex)
                            End If
                        End If
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
        ''' Columns where ActiveRole is Nothing. Try and Fix.
        ''' </summary>
        Private Sub ColumnsWhereActiveRoleIsNothingTryAndFix()

            Try
                Dim larFatalColumn = From Table In Me.RDS.Table
                                     From Column In Table.Column
                                     Where Column.ActiveRole Is Nothing
                                     Select Column

                For Each lrColumn In larFatalColumn.ToArray
                    Dim larCoveredRoles As New List(Of FBM.Role)
                    Dim larRole As List(Of FBM.Role) = lrColumn.Role.getDownstreamRoleActiveRoles(larCoveredRoles)
                    If larRole.Count = 1 Then
                        lrColumn.setActiveRole(larRole(0))
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
        ''' Columns where ActiveRole is Nothing. Remove the Column.
        ''' </summary>
        Private Sub ColumnsWhereActiveRoleIsNothingRemoveTheColumn()

            Try


                Dim larFatalColumn = From Table In Me.RDS.Table
                                     From Column In Table.Column
                                     Where Column.ActiveRole Is Nothing
                                     Select Column

                For Each lrColumn In larFatalColumn.ToArray
                    Call lrColumn.Table.removeColumn(lrColumn)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub ColumnsWhereNoLongerPartOfSupertypeHierarchyRemoveColumn()

            Try
                For Each lrTable In Me.RDS.Table.ToArray
                    For Each lrColumn In lrTable.Column.ToArray
                        If Not lrColumn.BelongsToModelElement(lrTable.FBMModelElement) Then
                            If Not lrColumn.Role.JoinedORMObject.isSupertypeOfModelElement(lrTable.FBMModelElement) Then
                                lrTable.removeColumn(lrColumn)
                            End If
                        End If
                    Next
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
        ''' InternalUniquenessConstraints, where LevelNumbers are not correct.
        ''' </summary>
        Private Sub InternalUniquenessConstraintsWhereLevelNumbersAreNotCorrect()

            Try
                For Each lrFactType In Me.FactType
                    Dim liInternalUniquenessConstraintLevelNrSum = (From InternalUniquenessConstraint In lrFactType.InternalUniquenessConstraint
                                                                    Select InternalUniquenessConstraint.LevelNr).Sum
                    Dim liSum As Integer = (lrFactType.InternalUniquenessConstraint.Count / 2) * (1 + lrFactType.InternalUniquenessConstraint.Count)
                    If liInternalUniquenessConstraintLevelNrSum <> liSum Then
                        Call lrFactType.ResetInternalUniquenessConstraintLevelNumbers()
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
        ''' Column Ordinal Positions. Reset ordinal positions.
        ''' </summary>
        Private Sub ColumnOrdinalPositionsResetWhereOutOfSynchronousOrder()

            Try
                For Each lrTable In Me.RDS.Table.FindAll(Function(x) Not x.isSubtype)
                    Dim liColumnOrdinalPositionSum = (From Column In lrTable.Column
                                                      Select Column.OrdinalPosition).Sum
                    Dim liSum As Integer = (lrTable.Column.Count / 2) * (1 + lrTable.Column.Count)
                    If liColumnOrdinalPositionSum <> liSum Then
                        Call lrTable.resetColumnOrdinalPositions()
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
        ''' RDS Tables with no Columns. Remove Table.
        ''' </summary>
        Private Sub RDSTablesWithNoColumnsRemoveThoseTables()

            Try
                For Each lrTable In Me.RDS.Table.FindAll(Function(x) x.Column.Count = 0)
                    Me.RDS.removeTable(lrTable)
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
        ''' RDS Columns that should be Mandatory (i.e. on Objectifed Fact Types).
        ''' </summary>
        Private Sub RDSColumnsThatShouldBeMandatoryMakeMandatory()

            Try
                Dim larColumn = From Table In Me.RDS.Table
                                From Column In Table.Column
                                Where Column.Role.FactType.IsObjectified And Column.Table.Name = Column.Role.FactType.Id And Not Column.IsMandatory
                                Select Column

                For Each lrColumn In larColumn
                    Call Me.createCMMLAttributeIsMandatory(lrColumn)
                    lrColumn.IsMandatory = True
                    Call lrColumn.triggerForceRefreshEvent()
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub RDSColumnsWithoutActiveRoles()

            Try
                Dim larColumn = From Table In Me.RDS.Table
                                From Column In Table.Column
                                Where Column.ActiveRole Is Nothing
                                Select Column

                For Each lrColumn In larColumn
                    If lrColumn.FactType IsNot Nothing And lrColumn.Role IsNot Nothing Then
                        Dim larCoveredRoles As New List(Of FBM.Role)
                        Dim larRole As List(Of FBM.Role) = lrColumn.Role.getDownstreamRoleActiveRoles(larCoveredRoles)
                        If larRole.Count = 1 Then
                            Call lrColumn.setActiveRole(larRole(0))
                        End If
                    End If
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Remove FactTypeInstances from Page where FactTypeIntance has RoleInstance that Joins nothing.
        ''' </summary>
        Private Sub RemoveFactTypeInstancesFromPageWhereFactTypeIntanceHasRoleInstanceThatJoinsNothing()

            Try
                Dim larFactTypeInstance = From Page In Me.Page
                                          From FactTypeInstance In Page.FactTypeInstance
                                          From RoleInstance In FactTypeInstance.RoleGroup
                                          Where RoleInstance.JoinedORMObject Is Nothing
                                          Select FactTypeInstance

                For Each lrFactTypeInstance In larFactTypeInstance.ToArray
                    Call lrFactTypeInstance.RemoveFromPage(True)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub RDSTablesWhereColumnAppearsTwiceForSameFactType()

            Try
                Dim larDuplicateColumn = From Table In Me.RDS.Table
                                         Where Table.FBMModelElement.GetType <> GetType(FBM.FactType)
                                         From Column In Table.Column.GroupBy(Function(x) New With {.TableName = x.Table.Name, .FactTypeId = x.FactType.Id, .RoleId = x.Role.Id, .ActiveRoleId = x.ActiveRole.Id}) _
                                                                    .Where(Function(g) g.Count() > 1) _
                                                                    .Select(Function(g) g)
                                         Select Column


                For Each lrColumnDef In larDuplicateColumn.ToList
                    Debugger.Break()
                    'Dim lrColumn = lrColumnDef.Table.Column.Find(Function(x) x.Role.Id = lrColumnDef.RoleId
                    '                                             And x.ActiveRole.Id = lrColumnDef.ActiveRoleId 
                    '                                             And x.FactType.Id = lrColumnDef.FactTypeId)
                    'Call lrColumn.Table.removeColumn(lrColumn)
                Next



            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' RDSTables where the number of PrimaryKey columns does not match the number of Roles in the PreferredIdentifier
        ''' </summary>
        Private Sub RDSTablesWhereTheNumberOfPrimaryKeyColumnsDoesNotMatchTheNumberOfRolesInThePreferredIdentifierFixThat()

            Try
                Dim lrIndex As RDS.Index

                Dim larTable = From Table In Me.RDS.Table
                               Where Table.FBMModelElement.GetType = GetType(FBM.FactType)
                               Select Table

                For Each lrTable In larTable

                    If lrTable.getPreferredIdentifierRoleConstraint IsNot Nothing Then
                        If lrTable.getPrimaryKeyColumns.Count < lrTable.getPreferredIdentifierRoleConstraint.Role.Count Then
                            'Need to add a Column for the missing Role
                            Dim larPrimaryKeyRole = From lrColumn In lrTable.getPrimaryKeyColumns
                                                    Select lrColumn.Role

                            For Each lrRole In lrTable.getPreferredIdentifierRoleConstraint.Role
                                If Not larPrimaryKeyRole.Contains(lrRole) Then
                                    'Might need to make a Column for that Role.
                                    'Or Add Column to Index.

                                    Dim larCoveredRole As New List(Of FBM.Role)
                                    Dim larActiveRole As List(Of FBM.Role) = lrRole.getDownstreamRoleActiveRoles(larCoveredRole)

                                    Dim lsColumnName As String
                                    For Each lrActiveRole In larActiveRole
                                        lsColumnName = lrTable.createUniqueColumnName(lrActiveRole.JoinedORMObject.Id, Nothing, 0)
                                        Dim lrColumn As New RDS.Column(lrTable, lsColumnName, lrRole, lrActiveRole, True)

                                        Dim lrTempColumn As RDS.Column = lrTable.Column.Find(AddressOf lrColumn.EqualsByRoleActiveRole)
                                        If lrTempColumn IsNot Nothing Then
                                            lrColumn = lrTempColumn
                                        End If

                                        lrTable.addColumn(lrColumn)
                                        lrIndex = lrTable.getPrimaryKeyIndex
                                        If lrIndex IsNot Nothing Then
                                            If Not lrIndex.Column.Contains(lrColumn) Then
                                                lrIndex.addColumn(lrColumn)
                                            End If
                                        End If
                                    Next

                                End If
                            Next
                        End If
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
        ''' Duplicate Facts. Remove Duplicates.
        ''' </summary>
        Private Sub DuplicateFactsRemoveDuplicates()

            Try
                Dim larDuplicateFactFactType = From FactType In Me.FactType
                                               Where FactType.ModelError.FindAll(Function(x) x.ErrorId = pcenumModelErrors.PopulationUniquenessError).Count > 0
                                               Select FactType

                For Each lrFactType In larDuplicateFactFactType
                    For Each lrInternalUniquenessConstraint In lrFactType.InternalUniquenessConstraint
                        '---------------------------------------------------------------------------
                        'Find the Count of Facts that have matching FactData for the span of Roles 
                        '  in the InternalUniquenessConstraint
                        '---------------------------------------------------------------------------
                        For Each lrFact In lrFactType.Fact.ToArray

                            Dim lrFactPredicate = New FBM.FactPredicate

                            For Each lrRole In lrInternalUniquenessConstraint.Role

                                Dim lrFactData = lrFact.GetFactDataByRoleId(lrRole.Id)
                                If lrFactData Is Nothing Then
                                    'No point continuing, abrt
                                    Exit For
                                Else
                                    Call lrFactData.ClearModelErrors()
                                    Dim lsDataValue = lrFact.GetFactDataByRoleId(lrRole.Id).Data
                                    lrFactData = New FBM.FactData(New FBM.Role(lrFactType, lrRole.Id, True), New FBM.Concept(lsDataValue))

                                    lrFactPredicate.data.Add(lrFactData)
                                End If
                            Next

                            '--------------------------------------------------------------------
                            'Retrieve all the Facts from the FactType that match the predicate.
                            '--------------------------------------------------------------------                
                            Dim lrFactList = lrFactType.Fact.FindAll(AddressOf lrFactPredicate.EqualsByRoleIdData)

                            If lrFactList.Count > 1 Then
                                For Each lrDuplicateFact In lrFactList.ToArray
                                    'Remove the first duplicate
                                    Call lrFactType.RemoveFactById(lrDuplicateFact)
                                    Exit For
                                Next
                            End If
                        Next
                    Next
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
        ''' PGSNodes/RDS Tables of ObjectifiedFactTypes that are missing Relations.
        ''' </summary>
        Private Sub RDSTablesAndPGSNodesThatAreMissingRelationsAddTheRelations()

            Try
                'ObjectifiedFactTypes
                For Each lrTable In Me.RDS.Table.FindAll(Function(x) x.FBMModelElement.IsObjectified Or x.isPGSNode)

                    For Each lrColumn In lrTable.Column.FindAll(Function(x) Not x.Role.JoinedORMObject.GetType = GetType(FBM.ValueType) And x.OutgoingRelation.Count = 0)

                        If lrColumn.FactType IsNot Nothing Then
                            If lrColumn.FactType.IsManyTo1BinaryFactType Then
                                GoTo SkipColumn
                            End If
                        End If

                        Dim lrRelation As New RDS.Relation(System.Guid.NewGuid.ToString,
                                                               lrTable,
                                                               pcenumCMMLMultiplicity.Many,
                                                               True,
                                                               lrColumn.Role.HasInternalUniquenessConstraint,
                                                               "is involved in",
                                                               lrColumn.Role.JoinedORMObject.getCorrespondingRDSTable,
                                                               pcenumCMMLMultiplicity.One,
                                                               False,
                                                               "involves",
                                                               lrTable.FBMModelElement
                                                              )

                        Dim lrExistingRelation As RDS.Relation = Me.RDS.Relation.Find(AddressOf lrRelation.EqualsByOriginTableDestinationTable)

                        If lrExistingRelation IsNot Nothing Then
                            MsgBox("Relation already exists between " & lrRelation.OriginTable.Name & " and " & lrRelation.DestinationTable.Name & ". Find some other reason why it isn't being shown in/on the Model.")
                            GoTo SkipColumn
                        End If

                        lrRelation.OriginColumns.Add(lrColumn)
                        For Each lrDestinationPKColumn In lrColumn.Role.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                            lrRelation.DestinationColumns.Add(lrDestinationPKColumn)
                        Next
                        lrColumn.Relation.Add(lrRelation)
                        Call Me.RDS.addRelation(lrRelation)
                    Next
SkipColumn:
                Next

                'EntityTypes with CompoundReferenceScheme
                For Each lrTable In Me.RDS.Table.FindAll(Function(x) x.FBMModelElement.GetType = GetType(FBM.EntityType))

                    For Each lrColumn In lrTable.Column.FindAll(Function(x) x.Role.FactType.Arity = 2)

                        If lrColumn.Role.FactType.IsManyTo1BinaryFactType Then

                            If Not lrColumn.Role.FactType.GetOtherRoleOfBinaryFactType(lrColumn.Role.Id).JoinedORMObject.GetType = GetType(FBM.ValueType) And lrColumn.OutgoingRelation.Count = 0 Then

                                Dim lrRelation As RDS.Relation
                                lrRelation = Me.generateRelationForManyTo1BinaryFactType(lrColumn.Role, False)

                                Dim lrExistingRelation As RDS.Relation = Me.RDS.Relation.Find(AddressOf lrRelation.EqualsByOriginTableDestinationTable)

                                If lrExistingRelation IsNot Nothing Then
                                    MsgBox("Relation already exists between " & lrRelation.OriginTable.Name & " and " & lrRelation.DestinationTable.Name & ". Find some other reason why it isn't being shown in/on the Model.")
                                    lrColumn.Relation.Remove(lrRelation)
                                    GoTo SkipColumn2
                                End If

                                For Each lrOriginColumn In lrRelation.OriginColumns
                                    lrOriginColumn.Relation.AddUnique(lrRelation)
                                Next
                                Call Me.RDS.addRelation(lrRelation)
                            End If
                        End If
SkipColumn2:
                    Next
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
        ''' RDS Tables with more than one relation for the same FactType/Join
        ''' </summary>
        Private Sub RDSTablesWithMoreThanOneRelationForTheSameFactTypeJoinPruneExtraRelations()

            Try
                Dim larRelation = (From Table In Me.RDS.Table
                                   From Column In Table.Column
                                   Where Column.OutgoingRelation.Count > 1
                                   Select Column.OutgoingRelation.First).ToList

                For Each lrRelation In larRelation.ToArray

                    Dim liCount = (From Relation In Me.RDS.Relation
                                   Where Relation.OriginTable Is lrRelation.OriginTable
                                   Where Relation.DestinationTable Is lrRelation.DestinationTable
                                   Select Relation.Id Distinct).Count

                    If liCount > 1 Then
                        Call Me.RDS.removeRelation(lrRelation)
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
        ''' RDS Relations, that have no OriginColumns
        ''' </summary>
        Private Sub RDSRelationsThatHaveNoOriginColumnsRemoveRelation()

            Try
                Dim larRelation = (From Relation In Me.RDS.Relation
                                   Where Relation.OriginColumns.Count = 0
                                   Select Relation).ToList

                For Each lrRelation In larRelation.ToArray
                    Call Me.RDS.removeRelation(lrRelation)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub RDSRelationsWhereOriginColumnCountNotEqualDestinationColumnCount(Optional ByRef arModelElementToFix As Object = Nothing)

            Try
                Dim lrRDSRelation As RDS.Relation = arModelElementToFix

                If arModelElementToFix Is Nothing Then

                    Dim larRDSRelation = From Relation In Me.RDS.Relation
                                         Where Relation.OriginColumns.Count < Relation.DestinationColumns.Count
                                         Select Relation

                    For Each lrRDSRelation In larRDSRelation.ToArray

                        Call Me.FixRelationWhereOriginColumnCountNotEqualDestinationColumnCount(lrRDSRelation)
                    Next

                Else

                    Call Me.FixRelationWhereOriginColumnCountNotEqualDestinationColumnCount(lrRDSRelation)

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSRelationsThatHaveOriginTableButNoDestinationTableAndViceVersa()

            Try

                Dim lsSQLQuery As String
                Dim lrRecordset As ORMQL.Recordset
                Dim lrRecordset2 As ORMQL.Recordset

                lsSQLQuery = "SELECT *"
                lsSQLQuery.AppendLine(" FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString)

                lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrRecordset.EOF

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery.AppendLine(" FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString)
                    lsSQLQuery.AppendLine(" WHERE Relation = '" & lrRecordset("Relation").Data & "'")

                    lrRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset2.EOF Then

                        lsSQLQuery = "REMOVE INSTANCE '" & lrRecordset("Relation").Data & "' FROM CoreElement"
                        Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    lrRecordset.MoveNext()
                End While

                lsSQLQuery = "SELECT *"
                lsSQLQuery.AppendLine(" FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString)

                lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrRecordset.EOF

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery.AppendLine(" FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString)
                    lsSQLQuery.AppendLine(" WHERE Relation = '" & lrRecordset("Relation").Data & "'")

                    lrRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset2.EOF Then

                        lsSQLQuery = "REMOVE INSTANCE '" & lrRecordset("Relation").Data & "' FROM CoreElement"
                        Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    lrRecordset.MoveNext()
                End While

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

#Region "Fixes"

        Private Sub FixRelationWhereOriginColumnCountNotEqualDestinationColumnCount(ByRef arRDSRelation As RDS.Relation)

            Try
                Dim lrRDSRelation As RDS.Relation = arRDSRelation

                'Because are going to replace them with the PrimaryKeyIndex.Columns
                If lrRDSRelation.DestinationTable.HasPrimaryKeyIndex And lrRDSRelation.DestinationTable IsNot Nothing Then

                    For Each lrDestinationColumn In lrRDSRelation.DestinationColumns.ToArray
                        Call lrRDSRelation.RemoveDestinationColumn(lrDestinationColumn)
                    Next


                    'Replacing pointers to PrimaryKey of DestinationTable. I.e. DestinationColumns
                    If lrRDSRelation.DestinationTable.HasPrimaryKeyIndex Then
                        For Each lrDestinationColumn In lrRDSRelation.DestinationTable.getPrimaryKeyColumns
                            lrRDSRelation.AddDestinationColumn(lrDestinationColumn)
                        Next
                    End If

                End If

                Dim lrNewColumn As RDS.Column
                Dim lrRole As FBM.Role = Nothing
                If arRDSRelation.ResponsibleFactType.IsManyTo1BinaryFactType Then
                    lrRole = arRDSRelation.ResponsibleFactType.RoleGroup.Find(Function(x) x.HasInternalUniquenessConstraint)
                End If

                For Each lrDestinationColumn In lrRDSRelation.DestinationColumns
                    If lrRDSRelation.OriginColumns.Find(Function(x) x.ActiveRole.Id = lrDestinationColumn.ActiveRole.Id) Is Nothing Then
                        Try
                            Dim lrActualColumn = (From Column In lrRDSRelation.OriginTable.Column
                                                  Where Not lrRDSRelation.OriginColumns.Contains(Column)
                                                  Where lrRDSRelation.DestinationColumns.Select(Function(x) x.ActiveRole.Id).Contains(Column.ActiveRole.Id)
                                                  Select Column).First
                            'lrRDSRelation.OriginTable.Column.Find(Function(x) x.Name = lrDestinationColumn.Name)
                            If lrActualColumn IsNot Nothing Then
                                Call lrRDSRelation.AddOriginColumn(lrActualColumn)
                            End If

                        Catch ex As Exception
                            'Couldn't fix it that way.
                            '=====================================================
                            'The Column doesn't exist in the Table yet.                                            
                            Dim lsColumnName As String = arRDSRelation.OriginTable.createUniqueColumnName(lrDestinationColumn.ActiveRole.JoinedORMObject.Id)
                            lrNewColumn = New RDS.Column(arRDSRelation.OriginTable, lsColumnName, lrRole, lrDestinationColumn.ActiveRole, lrRole.Mandatory)
                            arRDSRelation.OriginTable.addColumn(lrNewColumn)
                            Call lrRDSRelation.AddOriginColumn(lrNewColumn)
                        End Try
                    End If
                Next

                Dim larExcessOriginColumns = (From OriginColumn In lrRDSRelation.OriginColumns
                                              Select OriginColumn.ActiveRole.Id).ToList.Except(From DestinationColumn In lrRDSRelation.DestinationColumns
                                                                                               Select DestinationColumn.ActiveRole.Id).ToList

                For Each lsActiveRoleId In larExcessOriginColumns
                    Dim lrColumn As RDS.Column = lrRDSRelation.OriginColumns.Find(Function(x) x.ActiveRole.Id = lsActiveRoleId)
                    Call lrRDSRelation.RemoveOriginColumn(lrColumn)
                Next

                'Missing Origin Columns
                Dim larMissingOriginColumns = (From DestinationColumn In lrRDSRelation.DestinationColumns
                                               Select DestinationColumn.ActiveRole.Id).ToList.Except(From OriginColumn In lrRDSRelation.OriginColumns
                                                                                                     Select OriginColumn.ActiveRole.Id).ToList


                For Each lsDestinationColumnId In larMissingOriginColumns

                    Dim lrDestinationColumn As RDS.Column = arRDSRelation.DestinationColumns.Find(Function(x) x.Id = lsDestinationColumnId)
                    lrRole = arRDSRelation.ResponsibleFactType.RoleGroup.Find(Function(x) x.HasInternalUniquenessConstraint)

                    lrNewColumn = arRDSRelation.OriginTable.Column.Find(Function(x) x.Role.Id = lrRole.Id _
                                                                        And x.ActiveRole.Id = lrDestinationColumn.ActiveRole.Id)

                    If arRDSRelation.ResponsibleFactType.IsManyTo1BinaryFactType Then

                        If lrNewColumn Is Nothing Then
                            '=====================================================
                            'The Column doesn't exist in the Table yet.                                            
                            Dim lsColumnName As String = arRDSRelation.OriginTable.createUniqueColumnName(lrDestinationColumn.ActiveRole.JoinedORMObject.Id)
                            lrNewColumn = New RDS.Column(arRDSRelation.OriginTable, lsColumnName, lrRole, lrDestinationColumn.ActiveRole, lrRole.Mandatory)
                            arRDSRelation.OriginTable.addColumn(lrNewColumn)
                        End If
                    End If

                    arRDSRelation.AddOriginColumn(lrNewColumn)
                Next

                '20220726-VM-Doesn't seem to work.
                'If lrRDSRelation.OriginColumns.Count <> lrRDSRelation.DestinationColumns.Count Then

                '    If lrRDSRelation.DestinationTable.HasPrimaryKeyIndex Then
                '        'Replace OriginColumns. Have already fixed DestinationColumns
                '        For Each lrOriginColumn In lrRDSRelation.OriginColumns.ToArray
                '            Call lrRDSRelation.RemoveOriginColumn(lrOriginColumn)
                '            Call lrRDSRelation.OriginTable.removeColumn(lrOriginColumn)
                '        Next

                '        'Replacing pointers to PrimaryKey of DestinationTable. I.e. DestinationColumns
                '        For Each lrDestinationColumn In lrRDSRelation.DestinationTable.getPrimaryKeyColumns
                '            Dim lrNewOriginColumn = lrDestinationColumn.Clone(lrRDSRelation.OriginTable, lrRDSRelation, True, True)
                '            lrNewOriginColumn.Relation.Add(lrRDSRelation)
                '            lrRDSRelation.OriginTable.addColumn(lrNewOriginColumn)
                '            lrRDSRelation.AddOriginColumn(lrDestinationColumn)
                '        Next
                '    End If
                'End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub SubtypeRelationshipWithNoFactType()

            Try
                Dim larSubtypeRelationship = From EntityType In Me.EntityType
                                             From SubtypeRelationship In EntityType.SubtypeRelationship
                                             Where SubtypeRelationship.FactType Is Nothing
                                             Select SubtypeRelationship

                For Each lrSubtypeRelationship In larSubtypeRelationship.ToArray
                    Call lrSubtypeRelationship.RemoveFromModel()
                    Call lrSubtypeRelationship.ModelElement.CreateSubtypeRelationship(lrSubtypeRelationship.parentModelElement, False, Nothing, Nothing, True, Nothing)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub ObjectifiedFactTypesWithNoCorrespondingRDSTable()

            Try
                Dim lrColumn As RDS.Column

                Dim larObjectifiedFactType = From FactType In Me.FactType.FindAll(Function(x) Not x.IsMDAModelElement)
                                             Where FactType.IsObjectified
                                             Where Me.RDS.Table.Find(Function(x) x.Name = FactType.Id) Is Nothing
                                             Select FactType

                For Each lrFactType In larObjectifiedFactType

                    'Create the Table
                    Dim lrTable As New RDS.Table(Me.RDS, lrFactType.Id, lrFactType)
                    Call Me.RDS.addTable(lrTable)

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

                    '---------------------------------------------------------
                    'Columns
                    'Must have a column for all of the Roles of the FactType
                    '--------------------------------------------------------
#Region "Columns for Roles"
                    For Each lrRole In lrFactType.RoleGroup

                        Select Case lrRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.ValueType
#Region "ValueType"
                                If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id) Then
                                    'There is no Column in the Table for the Role.
                                    lrColumn = lrRole.GetCorrespondingFactTypeColumn(lrTable)
                                    lrColumn.IsMandatory = True
                                    'End If
                                    lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                End If
#End Region
                            Case Is = pcenumConceptType.EntityType
#Region "Entity Type"
                                If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id) Then
                                    'There is no Column in the Table for the Role.
                                    Dim lrEntityType As FBM.EntityType = lrRole.JoinedORMObject

                                    If lrEntityType.HasCompoundReferenceMode Then

                                        Dim larColumn As New List(Of RDS.Column)
                                        Call lrEntityType.getCompoundReferenceSchemeColumns(lrTable, lrRole, larColumn)

                                        For Each lrColumn In larColumn
                                            If lrRole.InternalUniquenessConstraint.Count > 0 Then
                                                lrColumn.IsMandatory = True
                                            End If
                                            lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                        Next
                                    Else
                                        lrColumn = lrRole.GetCorrespondingFactTypeColumn(lrTable)

                                        lrColumn.IsMandatory = True
                                        lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)
                                    End If

                                End If
#End Region
                            Case Else 'Joins a FactType.
#Region "FactType"

                                Dim larColumn As New List(Of RDS.Column)

                                larColumn = lrRole.getColumns(lrTable, lrRole)

                                For Each lrColumn In larColumn
                                    Dim lbFailed As Boolean = False
                                    If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id And x.ActiveRole.Id = lrColumn.ActiveRole.Id) Then
                                        'There is no existing Column in the Table for lrColumn.
                                        lrColumn.Name = lrTable.createUniqueColumnName(lrColumn.Name, lrColumn, 0)
                                        If lrRole.InternalUniquenessConstraint.Count > 0 Then
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
                                                    lrColumn.Name = lrTable.createUniqueColumnName(lrColumn.Name, lrColumn, 0)
                                                    If lrRole.InternalUniquenessConstraint.Count > 0 Then
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
#End Region
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
#End Region

                    '-------------------------------------------------
                    'Columns - Adjoined Many-To-One BinaryFactTypes.
                    '--------------------------------------------------
#Region "Columns - Adjoined Many-to-One BinaryFactTypes"

                    Dim larManyToOneFactTypeRoles = From FactType In Me.FactType.FindAll(Function(x) x.IsManyTo1BinaryFactType)
                                                    From Role In FactType.RoleGroup
                                                    Where Role.JoinedORMObject.Id = lrFactType.Id
                                                    Select Role

                    For Each lrManyToOneFactTypeRole In larManyToOneFactTypeRoles

                        If lrManyToOneFactTypeRole.InternalUniquenessConstraint.Count > 0 Then

                            Dim lrModelObject As FBM.ModelObject = Nothing

                            lrColumn = lrManyToOneFactTypeRole.GetCorrespondingUnaryOrBinaryFactTypeColumn(lrTable)
                            lrColumn.FactType = lrManyToOneFactTypeRole.FactType

                            If lrManyToOneFactTypeRole.Mandatory Then
                                lrColumn.IsMandatory = True
                            End If

                            lrTable.addColumn(lrColumn, Me.IsDatabaseSynchronised)

                        End If

                    Next
#End Region


                    For Each lrRoleConstraint In lrFactType.InternalUniquenessConstraint

                        Dim larColumn As List(Of RDS.Column) = Nothing

                        '------------------------
                        'Index 
                        '------------------------
#Region "Index"
                        Dim larIndexColumn As New List(Of RDS.Column)

                        For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole

                            Dim larTableColumn = From Column In lrTable.Column
                                                 Where Column.Role.Id = lrRoleConstraintRole.Role.Id
                                                 Select Column

                            For Each lrColumn In larTableColumn
                                larIndexColumn.Add(lrColumn)
                            Next 'Column

                        Next 'RoleConstraintRole

                        Dim lsQualifier As String
                        Dim lbIsPrimaryKey As Boolean = False
                        If lrFactType.InternalUniquenessConstraint.Count = 1 Or lrRoleConstraint.IsPreferredIdentifier Then
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
#End Region
                        '----------------------------------------
                        'PGS Relation
                        '----------------------------------------
                        If (lrFactType.Arity = 2 Or lrFactType.Arity = 3) _
                               And lrRoleConstraint.RoleConstraintRole.Count = 2 _
                               And Not lrFactType.InternalUniquenessConstraint.Count > 1 _
                               And Not lrRoleConstraint.atLeastOneRoleJoinsAValueType Then
                            lrTable.setIsPGSRelation(True)
                        End If
                    Next

#Region "Flashcard"
                    Dim lfrmFlashCard As New frmFlashCard
                    lfrmFlashCard.ziIntervalMilliseconds = 5600
                    lfrmFlashCard.BackColor = Color.DarkSeaGreen
                    Dim lsMessage As String = ""
                    lsMessage = "Created RDS Table for Fact Type, " & lrFactType.Id & "."
                    lfrmFlashCard.zsText = lsMessage
                    Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(frmMain)
#End Region

                Next


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub
#End Region

    End Class

End Namespace
