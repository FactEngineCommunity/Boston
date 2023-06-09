Imports System.Xml.Serialization
Imports System.Reflection

Namespace RDS


    <Serializable()> _
    Public Class Model

        <XmlIgnore()> _
        <NonSerialized()> _
        Public Model As FBM.Model

        <XmlElement()> _
        Public Table As New List(Of RDS.Table)

        <XmlElement()> _
        Public DataType As New List(Of RDS.DataType)

        <XmlElement()> _
        Public Index As New List(Of RDS.Index)

        <XmlElement()> _
        Public Relation As New List(Of RDS.Relation)

        <XmlIgnore()>
        <NonSerialized()>
        Public TargetDatabaseType As pcenumDatabaseType = Nothing

        <XmlIgnore()>
        <NonSerialized()>
        Public DatabaseDataType As New List(Of DatabaseDataType)

        <NonSerialized()>
        Public Event IndexAdded(ByRef arIndex As RDS.Index)
        <NonSerialized()>
        Public Event IndexRemoved(ByRef arIndex As RDS.Index)
        <NonSerialized()>
        Public Event RelationAdded(ByRef arRelation As RDS.Relation)
        <NonSerialized()>
        Public Event RelationRemoved(ByVal arRelation As RDS.Relation)
        <NonSerialized()>
        Public Event TableAdded(ByRef arTable As RDS.Table)
        <NonSerialized()>
        Public Event TableRemoved(ByRef arTable As RDS.Table)

        ''' <summary>
        ''' Parameterless New for serialisation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model)
            Me.Model = arModel
        End Sub

        Public Sub addRelation(ByRef arRelation As RDS.Relation)

            Try
                Me.Relation.Add(arRelation)

                Call Me.Model.createCMMLRelation(arRelation)

                RaiseEvent RelationAdded(arRelation)

                'Database synchronisation
                If Me.Model.IsDatabaseSynchronised Then
                    Call Me.Model.connectToDatabase()
                    Call Me.Model.DatabaseConnection.AddForeignKey(arRelation)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub addTable(ByRef arTable As RDS.Table)

            Try
                Dim lrTable As RDS.Table = arTable
                'CodeSafe - Check that the Table doesn't already exist
                If Me.Table.Find(Function(x) x.Name = lrTable.Name) IsNot Nothing Then
                    'CodeSafe: Set arTable to the existing Table.
                    '20200725-VM-This might not be the right strategy. Need to revisit. Why is a Table being created that already exists.
                    'See FBM.FactType.Objectify for ManyToOne FactType.
                    arTable = Me.Table.Find(Function(x) x.Name = lrTable.Name)
                    Exit Sub
                    'Throw New Exception("Table with name, " & arTable.Name & ", already exists in the Relational Data Structure.")
                End If

                Me.Table.AddUnique(arTable)

                Call Me.Model.createCMMLTable(arTable)

                RaiseEvent TableAdded(arTable)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub columnSetDataType(ByRef arColumn As RDS.Column,
                                     ByVal aiDataType As pcenumORMDataType,
                                     ByVal aiLength As Integer,
                                     ByVal aiPrecision As Integer)

            Try
                'Database synchronisation.
                Call Me.Model.connectToDatabase()
                Call Me.Model.DatabaseConnection.columnChangeDatatype(arColumn, aiDataType, aiLength, aiPrecision)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function createUniqueIndexName(ByRef asTrialName As String, ByVal aiSuffix As Integer) As String

            Dim lsTrialName = asTrialName

            If Me.Index.Find(Function(x) x.Name = lsTrialName) Is Nothing Then
                Return asTrialName
            Else
                aiSuffix += 1
                asTrialName &= aiSuffix.ToString
                Call Me.createUniqueIndexName(asTrialName, aiSuffix)
            End If

            Return asTrialName

        End Function

        Public Function getColumnsThatReferenceValueType(ByVal arValueType As FBM.ValueType) As List(Of Column)

            Try
                'Direct Columns
                Dim larNonNullActiveColumns = From Table In Me.Table
                                              From Column In Table.Column
                                              Where Column.ActiveRole IsNot Nothing
                                              Select Column

                Dim larDirectColumn = From Column In larNonNullActiveColumns
                                      Where Column.ActiveRole.JoinedORMObject.Id = arValueType.Id
                                      Select Column Distinct

                'Indirect Columns
                Dim larSimpleReferenceSchemeEntityTypes = From EntityType In Me.Model.EntityType
                                                          Where EntityType.ReferenceModeValueType IsNot Nothing
                                                          Select EntityType

                Dim larIndirectColumns = From EntityType In larSimpleReferenceSchemeEntityTypes
                                         From Column In larNonNullActiveColumns
                                         Where Column.ActiveRole.JoinedORMObject.Id = EntityType.Id _
                                         And EntityType.ReferenceModeValueType.Id = arValueType.Id
                                         Select Column Distinct

                Dim larReturnColumn As List(Of RDS.Column)

                larReturnColumn = larDirectColumn.ToList
                larReturnColumn.AddRange(larIndirectColumns.ToList)

                Return larReturnColumn

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return New List(Of RDS.Column)
            End Try

        End Function

        ''' <summary>
        ''' Gets a Relation by its ResponsibleFactType. NB Returns an error if there is no Relation or more than one Relation is returned.
        ''' </summary>
        ''' <param name="arFactType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getRelationByResponsibleFactType(ByVal arFactType As FBM.FactType) As RDS.Relation

            Try

                Dim larRelation = From Relation In Me.Relation
                                  Where Relation.ResponsibleFactType Is arFactType
                                  Select Relation

                If larRelation.Count > 1 Then
                    Throw New Exception("More than one Relation returned.")
                ElseIf larRelation.Count = 0 Then
                    Throw New Exception("Returned no Relations at all.")
                Else
                    Return larRelation.First
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return New RDS.Relation

            End Try
        End Function

        Public Function getRelationsByOriginTableOriginColumns(ByVal arOriginTable As RDS.Table,
                                                               ByVal aarOriginColumn As List(Of RDS.Column)
                                                               ) As List(Of RDS.Relation)

            Try
                Dim larRelation = From Relation In Me.Relation
                                  Where Relation.OriginTable.Name = arOriginTable.Name _
                                  And Relation.OriginColumns.CompareWith(aarOriginColumn) = 0
                                  Select Relation Distinct


                Return larRelation.ToList

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return New List(Of RDS.Relation)
            End Try

        End Function


        Public Function getTableByName(ByVal asTableName As String) As RDS.Table

            Return Me.Table.Find(Function(x) x.Name = asTableName)

        End Function

        Public Function getTableByRoleNamePlayedInRelation(ByVal asRoleName As String,
                                                           ByVal asRelationName As String,
                                                           Optional ByVal abThrowErrorMessage As Boolean = True) As RDS.Table

            Try
                Dim larTable = From Table In Me.Table
                               From Plays In Table.RolesPlayed
                               Where Plays.RoleName = asRoleName
                               Where Plays.RelationName = asRelationName
                               Select Table

                If larTable.Count > 0 Then
                    Return larTable.First
                Else
                    Throw New Exception("Method called where there is no Table that plays the Role named:" & asRoleName)
                End If

                Return Nothing

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                If abThrowErrorMessage Then
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                End If

                Return Nothing
            End Try

        End Function

        Public Function getTableByRoleNamePlayed(ByVal asRoleName As String,
                                                 Optional ByVal abThrowErrorMessage As Boolean = True) As RDS.Table

            Try
                Dim larTable = From Table In Me.Table
                               From Plays In Table.RolesPlayed
                               Where Plays.RoleName = asRoleName
                               Select Table

                If larTable.Count > 0 Then
                    Return larTable.First
                Else
                    Throw New Exception("Method called where there is no Table that plays the Role named:" & asRoleName)
                End If

                Return Nothing

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                If abThrowErrorMessage Then
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                End If

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Puts the tables in Relationship Order.
        ''' </summary>
        Public Sub orderTablesByRelations(Optional ByVal abOrderByFactTypesLast As Boolean = False)

            Dim lrTable As RDS.Table
            Dim liInd2 As Integer
            Dim lbTripped As Boolean = False
            Try
                'Put the Tables that are for FactTypes last
                If abOrderByFactTypesLast Then
                    For liInd = 0 To Me.Table.Count - 1
                        lrTable = Me.Table(liInd)
                        If lrTable.FBMModelElement.GetType = GetType(FBM.FactType) Then
                            Me.Table.Remove(lrTable)
                            Me.Table.Insert(Me.Table.Count - 1, lrTable)
                        End If
                    Next
                End If

                'Order by number of outgoing relations
                Me.Table.OrderBy(Function(x) x.getOutgoingRelations.Count)

                For liInd = 0 To Me.Table.Count - 1

                    lrTable = Me.Table(liInd)

                    liInd2 = liInd
                    lbTripped = False
                    While lrTable.hasHigherReferencedTable

                        Dim lrTempTable As RDS.Table = Me.Table(liInd2) 'Create a temporary Table

                        'Swap the Table with the next Table in the list of tables in Me.Tables
                        Me.Table(liInd2) = Me.Table(liInd2 + 1)
                        Me.Table(liInd2 + 1) = lrTempTable

                        liInd2 += 1
                        lbTripped = True
                    End While
                    If lbTripped Then
                        If Not liInd = 0 Then
                            liInd -= 1
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


        Public Sub removeIndex(ByRef arIndex As RDS.Index)

            Dim lrIndex As RDS.Index = arIndex

            '-------------------------------------------------------------------
            'Columns
            'Remove the Index from all Columns that are involved with the Index
            Dim larColumn = From Table In Me.Table
                            From Column In Table.Column
                            Where Column.Index.Contains(lrIndex)
                            Select Column

            For Each lrColumn In larColumn
                Call lrColumn.removeIndex(arIndex)
            Next

            'Remove the Index from the Table.
            arIndex.Table.Index.Remove(arIndex)

            '-------------------------------------------------------------------
            'Remove the Index from the RDS Model level.
            Me.Index.Remove(arIndex)

            '-------------------------------------------------------------------
            'Remove the Index from the CMML level.
            Call Me.Model.removeCMMLIndex(arIndex)

            RaiseEvent IndexRemoved(arIndex)

        End Sub

        Public Sub removeRelation(ByRef arRelation As RDS.Relation)

            Try
                Me.Relation.Remove(arRelation)

                Dim lsRelationId = arRelation.Id

                'Remove the Relation from the associated Columns
                Dim larColumn = From Table In Me.Table
                                From Column In Table.Column
                                From Relation In Column.Relation
                                Where Relation.Id = lsRelationId
                                Select Column

                For Each lrColumn In larColumn.ToArray
                    Call lrColumn.Relation.Remove(arRelation)
                Next

                Call Me.Model.removeCMMLRelation(arRelation)

                RaiseEvent RelationRemoved(arRelation)
                Call arRelation.triggerRemovedFromModel()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeTable(ByRef arTable As RDS.Table)

            Try
                'CodeSafe
                If arTable Is Nothing Then Exit Sub

                Dim lrTable As RDS.Table = arTable
                '-----------------------------------------------------------------------------------------------------
                'Indexes
                For Each lrIndex In arTable.Index.ToList
                    Call Me.removeIndex(lrIndex)
                Next

                '-----------------------------------------------------------------------------------------------------
                'Relations
                '  NB All relations should have been removed before removing the Table. But just for completeness, remove the Relations if there are any.
                Dim larRelation = From Relation In Me.Relation
                                  From OriginColumn In Relation.OriginColumns
                                  Where lrTable.Column.Contains(OriginColumn)
                                  Select Relation

                For Each lrRelation In larRelation.ToArray
                    Call Me.removeRelation(lrRelation)
                Next

                larRelation = From Relation In Me.Relation
                              From DestinationColumn In Relation.DestinationColumns
                              Where lrTable.Column.Contains(DestinationColumn)
                              Select Relation

                For Each lrRelation In larRelation.ToArray
                    Call Me.removeRelation(lrRelation)
                Next

                For Each lrIndex In Me.Index.FindAll(Function(x) x.Table.Name = lrTable.Name).ToArray
                    Call Me.removeIndex(lrIndex)
                Next

                'Remove the Table fromm the RDS Model.
                Call Me.Table.Remove(arTable)

                'Remove at the CMML Model level
                Call Me.Model.removeCMMLEntityByRDSTable(arTable)

                RaiseEvent TableRemoved(arTable)

                'Database synchronisation
                If Me.Model.IsDatabaseSynchronised Then
                    Me.Model.connectToDatabase()
                    Call Me.Model.DatabaseConnection.removeTable(arTable)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub RemoveTablesWithNoColumns()

            Try
                Dim larTable = From Table In Me.Table
                               Where Table.Column.Count = 0
                               Select Table

                For Each lrTable In larTable.ToArray
                    Call Me.removeTable(lrTable)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Function BuildGraphSchema(tables As IEnumerable(Of RDS.Table)) As GraphStandard.GraphSchemaRepresentation

            Dim graphSchemaRepresentation As New GraphStandard.GraphSchemaRepresentation()
            Dim graphSchema As New GraphStandard.GraphSchema()
            Dim nodeLabels As New List(Of GraphStandard.NodeLabel)()
            Dim relationshipTypes As New List(Of GraphStandard.RelationshipType)()
            Dim nodeObjectTypes As New List(Of GraphStandard.NodeObjectType)()
            Dim relationshipObjectTypes As New List(Of GraphStandard.RelationshipObjectType)()

            For Each lrTable In Me.Table
                ' NodeLabel
                Dim nodeLabel As New GraphStandard.NodeLabel() With {
                    .id = "nl:" & lrTable.Name,
                    .Token = lrTable.Name
                }
                nodeLabels.Add(nodeLabel)

                ' NodeObjectType
                Dim nodeObjectType As New GraphStandard.NodeObjectType() With {
                    .id = "n:" & lrTable.Name,
                    .Labels = New List(Of GraphStandard.NodeLabelRef)() From {
                        New GraphStandard.NodeLabelRef() With {.ref = "#nl:" & lrTable.Name}
                    },
                    .Properties = New List(Of GraphStandard.NodeProperty)()
                }

                For Each lrColumn In lrTable.Column
                    ' NodeProperty
                    Dim nodeProperty As New GraphStandard.NodeProperty() With {
                        .Token = lrColumn.Name,
                        .Type = New GraphStandard.NodeType() With {.Type = lrColumn.getMetamodelDataType.ToString},
                        .Nullable = Not lrColumn.isPartOfPrimaryKey
                    }
                    nodeObjectType.Properties.Add(nodeProperty)
                Next

                nodeObjectTypes.Add(nodeObjectType)

            Next

            ' RelationshipObjectType
            For Each lrRelation In Me.Relation.FindAll(Function(x) x.ResponsibleFactType.DBName <> "")
                Dim relationshipObjectType As New GraphStandard.RelationshipObjectType() With {
                    .id = "r:" & lrRelation.ResponsibleFactType.DBName,
                    .Type = New GraphStandard.RelationshipTypeRef() With {.ref = "#rt:" & lrRelation.ResponsibleFactType.DBName},
                    .From = New GraphStandard.RelationshipNodeRef() With {.ref = "#n:" & lrRelation.OriginTable.Name},
                    .To = New GraphStandard.RelationshipNodeRef() With {.ref = "#n:" & lrRelation.DestinationTable.Name},
                    .Properties = New List(Of GraphStandard.RelationshipProperty)() From {
                        New GraphStandard.RelationshipProperty() With {
                                    .Token = lrRelation.ResponsibleFactType.DBName,
                                    .Type = New GraphStandard.RelationshipPropertyType() With {.Type = "string"},
                                    .Nullable = False
                                }
                            }
                        }

                relationshipObjectTypes.Add(relationshipObjectType)
            Next

            graphSchema.NodeLabels = nodeLabels
            graphSchema.RelationshipTypes = relationshipTypes
            graphSchema.NodeObjectTypes = nodeObjectTypes
            graphSchema.RelationshipObjectTypes = relationshipObjectTypes

            graphSchemaRepresentation.GraphSchema = graphSchema

            Return graphSchemaRepresentation
        End Function


    End Class

End Namespace
