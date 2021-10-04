Imports System.Reflection

Namespace FBM

    Partial Public Class Model

        Public Sub FixErrors(ByVal aaiFixesToApply As List(Of pcenumModelFixType))

            Try

                With New WaitCursor

                    For Each liFixType In aaiFixesToApply

                        Select Case liFixType
                            Case Is = pcenumModelFixType.RolesWithoutJoinedORMObject
                                Call Me.RolesWithoutJoinedORMObject()
                            Case Is = pcenumModelFixType.RelationsInvalidActiveRoleOnOriginColumns
                                Call Me.RelationsInvalidActiveRoleOnOriginColumns()
                            Case Is = pcenumModelFixType.ColumnsWhereActiveRoleIsNothingTryAndFix
                                Call Me.ColumnsWhereActiveRoleIsNothingTryAndFix()
                            Case Is = pcenumModelFixType.ColumnsWhereActiveRoleIsNothingRemoveTheColumn
                                Call Me.ColumnsWhereActiveRoleIsNothingRemoveTheColumn()
                            Case Is = pcenumModelFixType.InternalUniquenessConstraintsWhereLevelNumbersAreNotCorrect
                                Call Me.InternalUniquenessConstraintsWhereLevelNumbersAreNotCorrect()
                            Case Is = pcenumModelFixType.ColumnOrdinalPositionsResetWhereOutOfSynchronousOrder
                                Call Me.ColumnOrdinalPositionsResetWhereOutOfSynchronousOrder()
                            Case Is = pcenumModelFixType.RDSTablesWithNoColumnsRemoveThoseTables
                                Call Me.RDSTablesWithNoColumnsRemoveThoseTables()
                            Case Is = pcenumModelFixType.RDSColumnsThatShouldBeMandatoryMakeMandatory
                                Call Me.RDSColumnsThatShouldBeMandatoryMakeMandatory()
                            Case Is = pcenumModelFixType.RemoveFactTypeInstancesFromPageWhereFactTypeIntanceHasRoleInstanceThatJoinsNothing
                                Call Me.RemoveFactTypeInstancesFromPageWhereFactTypeIntanceHasRoleInstanceThatJoinsNothing()
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
                                        lsColumnName = lrTable.createUniqueColumnName(Nothing, lrActiveRole.JoinedORMObject.Id, 0)
                                        Dim lrColumn As New RDS.Column(lrTable, lsColumnName, lrRole, lrActiveRole, True)
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
                                               Where FactType.ModelError.FindAll(Function(x) x.ErrorId = 100).Count > 0
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
                For Each lrTable In Me.RDS.Table.FindAll(Function(x) x.FBMModelElement.IsObjectified Or x.isPGSNode)

                    For Each lrColumn In lrTable.Column.FindAll(Function(x) Not x.Role.JoinedORMObject.GetType = GetType(FBM.ValueType) And x.OutgoingRelation.Count = 0)

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

                        lrRelation.OriginColumns.Add(lrColumn)
                        For Each lrDestinationPKColumn In lrColumn.Role.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                            lrRelation.DestinationColumns.Add(lrDestinationPKColumn)
                        Next
                        lrColumn.Relation.Add(lrRelation)
                        Call Me.RDS.addRelation(lrRelation)
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

        Private Sub RDSRelationsThatHaveOriginTableButNoDestinationTableAndViceVersa()

            Try

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
