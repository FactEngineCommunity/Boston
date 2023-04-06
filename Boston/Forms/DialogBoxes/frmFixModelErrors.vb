Imports System.Reflection

Public Class frmFixModelErrors

    Public mrModel As FBM.Model
    Dim marFixItem As New List(Of tComboboxItem)

    Private Sub frmFixModelErrors_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Public Sub SetupForm()

        Try

            Me.LabelModelName.Text = Me.mrModel.Name

            Me.CheckedListBoxFixTypes.Items.Clear()

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.DuplicateFactsRemoveDuplicates,
                                                                  "Duplicate Facts. Remove Duplicates.",
                                                                  pcenumModelFixType.DuplicateFactsRemoveDuplicates))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.InternalUniquenessConstraintsWhereLevelNumbersAreNotCorrect,
                                                                  "Internal Uniqueness Constraints Where Level Numbers Are Not Correct. Fix.",
                                                                  pcenumModelFixType.InternalUniquenessConstraintsWhereLevelNumbersAreNotCorrect))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.ObjectifyingEntitTypeIdsNotTheSameAsObjectifiedFactType,
                                                                  "Objectifying Entity Type Ids Not the same As Objectified Fact Type Id.",
                                                                  pcenumModelFixType.ObjectifyingEntitTypeIdsNotTheSameAsObjectifiedFactType))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.ObjectifiedFactTypesWithNoCorrespondingRDSTable,
                                                                  "Objectied Fact Types with no corresponding RDS Table.",
                                                                  pcenumModelFixType.ObjectifiedFactTypesWithNoCorrespondingRDSTable))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RemoveFactTypeInstancesFromPageWhereFactTypeIntanceHasRoleInstanceThatJoinsNothing,
                                                                  "Remove FactTypeInstances From Page Where FactTypeIntance Has RoleInstance That JoinsNothing.",
                                                                  pcenumModelFixType.RemoveFactTypeInstancesFromPageWhereFactTypeIntanceHasRoleInstanceThatJoinsNothing))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RelationsInvalidActiveRoleOnOriginColumns,
                                                                  "RDS Relations. Invalid ActiveRole On OriginColumns.",
                                                                  pcenumModelFixType.RelationsInvalidActiveRoleOnOriginColumns))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.ColumnsWhereActiveRoleIsNothingTryAndFix,
                                                                  "RDS Columns Where ActiveRole Is Nothing. TryAndFix.",
                                                                  pcenumModelFixType.ColumnsWhereActiveRoleIsNothingTryAndFix))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.ColumnsWhereActiveRoleIsNothingRemoveTheColumn,
                                                                  "RDS Columns Where ActiveRole Is Nothing. Remove The Column.",
                                                                  pcenumModelFixType.ColumnsWhereActiveRoleIsNothingRemoveTheColumn))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.ColumnsWhereNoLongerPartOfSupertypeHierarchyRemoveColumn,
                                                                  "RDS Columns Where Column is no longer part of Subtype Relationship Hierarchy. Remove The Column.",
                                                                  pcenumModelFixType.ColumnsWhereNoLongerPartOfSupertypeHierarchyRemoveColumn))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.ColumnOrdinalPositionsResetWhereOutOfSynchronousOrder,
                                                                  "RDS Column. Ordinal Positions. Reset Where Out Of Synchronous Order.",
                                                                  pcenumModelFixType.ColumnOrdinalPositionsResetWhereOutOfSynchronousOrder))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSTablesWithNoColumnsRemoveThoseTables,
                                                                  "RDS Tables With No Columns. Remove Those Tables.",
                                                                  pcenumModelFixType.RDSTablesWithNoColumnsRemoveThoseTables))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSColumnsThatShouldBeMandatoryMakeMandatory,
                                                                  "RDS Columns That Should Be Mandatory. Make Mandatory.",
                                                                  pcenumModelFixType.RDSColumnsThatShouldBeMandatoryMakeMandatory))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSTablesWhereColumnAppearsTwiceForSameFactType,
                                                                  "RDS Tables Where The Same Column Appears Twice For The Same Fact Type, Role, and Active Role. FixThat.",
                                                                  pcenumModelFixType.RDSTablesWhereColumnAppearsTwiceForSameFactType))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSTablesWhereTheNumberOfPrimaryKeyColumnsDoesNotMatchTheNumberOfRolesInThePreferredIdentifierFixThat,
                                                                  "RDS Tables Where The Number Of PrimaryKey Columns Does Not Match The Number Of Roles In The PreferredIdentifier. FixThat.",
                                                                  pcenumModelFixType.RDSTablesWhereTheNumberOfPrimaryKeyColumnsDoesNotMatchTheNumberOfRolesInThePreferredIdentifierFixThat))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSTablesAndPGSNodesThatAreMissingRelationsAddTheRelations,
                                                                  "RDS Tables And PGS Nodes That Are Missing Relations. Add The Relations.",
                                                                  pcenumModelFixType.RDSTablesAndPGSNodesThatAreMissingRelationsAddTheRelations))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSTablesWithMoreThanOneRelationForTheSameFactTypeJoinPruneExtraRelations,
                                                                  "RDS Tables With More Than One Relation For The Same Fact Type Join. Prune Extra Relations.",
                                                                  pcenumModelFixType.RDSTablesWithMoreThanOneRelationForTheSameFactTypeJoinPruneExtraRelations))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSRelationsThatHaveNoOriginColumnsRemoveRelation,
                                                                  "RDS Relations That Have No Origin Columns. Remove Relation.",
                                                                  pcenumModelFixType.RDSRelationsThatHaveNoOriginColumnsRemoveRelation))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSRelationsThatHaveOriginTableButNoDestinationTableAndViceVersa,
                                                                  "RDS Relations that have an origin table but no destination table, and vice versa. Remove from Model.",
                                                                  pcenumModelFixType.RDSRelationsThatHaveOriginTableButNoDestinationTableAndViceVersa))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RDSRelationsWhereOriginColumnCountNotEqualDestinationColumnCount,
                                                                  "RDS Relations, where Origin Column count <> Destination Column count.",
                                                                  pcenumModelFixType.RDSRelationsWhereOriginColumnCountNotEqualDestinationColumnCount))

            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.RolesWithoutJoinedORMObject,
                                                                  "Roles Without JoinedORMObject. Remove from Model.",
                                                                  pcenumModelFixType.RolesWithoutJoinedORMObject))


            Me.CheckedListBoxFixTypes.Items.Add(New tComboboxItem(pcenumModelFixType.SubtypeRelationshipWithNoFactType,
                                                                  "Subtype Relationships with no Fact Type, fix.",
                                                                  pcenumModelFixType.SubtypeRelationshipWithNoFactType))
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ButtonFixModelErrors_Click(sender As Object, e As EventArgs) Handles ButtonFixModelErrors.Click

        Try
            If Me.CheckedListBoxFixTypes.CheckedItems.Count = 0 Then
                MsgBox("Select at least one type of model fix that you would like to execute.")
                Exit Sub
            End If

            If MsgBox("Have you backed up the Boston database?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                Dim laiFixType As New List(Of pcenumModelFixType)

                For Each liItemIndex In Me.CheckedListBoxFixTypes.CheckedIndices
                    Dim lrItem As tComboboxItem = Me.CheckedListBoxFixTypes.Items(liItemIndex)
                    laiFixType.Add(lrItem.Tag)
                Next

                Call Me.mrModel.FixErrors(laiFixType)

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