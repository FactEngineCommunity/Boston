Imports openCypherTranspiler
Imports openCypherTranspiler.SQLRenderer
Imports openCypherTranspiler.Common

Namespace RDS
    Class GraphProvider
        Implements openCypherTranspiler.SQLRenderer.ISQLDBSchemaProvider

        Private Nodes As New Dictionary(Of String, GraphSchema.NodeSchema)
        Private Edges As New Dictionary(Of String, GraphSchema.EdgeSchema)
        Private Tables As New Dictionary(Of String, SQLTableDescriptor)

        Shared Sub New()
        End Sub

        Sub New(graphSchema As GraphStandard.GraphSchemaRepresentation)

            Me.PopulateNodes(graphSchema.GraphSchema.NodeObjectTypes)
            Me.PopulateEdges(graphSchema.GraphSchema.RelationshipObjectTypes)
            Me.PopulateTables(graphSchema.GraphSchema.NodeObjectTypes, graphSchema.GraphSchema.RelationshipObjectTypes)
        End Sub

        Sub New(ByRef lrRDSModel As RDS.Model)

            Me.PopulateNodesFromRDSTables(lrRDSModel.Table)
            Me.PopulateEdgesFromRDSModel(lrRDSModel)
            Me.PopulateTablesFromRDSTables(lrRDSModel)

        End Sub

        Private Function GetSQLTableDescriptors(entityId As String) As SQLTableDescriptor Implements ISQLDBSchemaProvider.GetSQLTableDescriptors
            Try
                Return Tables(entityId)
            Catch ex As Exception
                Try
                    Dim lsParts = entityId.Split("@")
                    Return Tables(lsParts(2) & "@" & lsParts(1) & "@" & lsParts(0))
                Catch ex1 As Exception
                    Return New SQLTableDescriptor With {
                            .EntityId = entityId.Substring(entityId.LastIndexOf("@") + 1),
                            .TableOrViewName = entityId.Substring(entityId.LastIndexOf("@") + 1)
                        }
                End Try

            End Try

        End Function

        Private Function GetNodeDefinition(nodeName As String) As GraphSchema.NodeSchema Implements ISQLDBSchemaProvider.GetNodeDefinition
            Return Nodes(nodeName)
        End Function

        Private Function GetEdgeDefinition(edgeVerb As String, fromNodeName As String, toNodeName As String) As GraphSchema.EdgeSchema Implements ISQLDBSchemaProvider.GetEdgeDefinition

            Dim lsTarget As String = $"{fromNodeName}@{edgeVerb}@{toNodeName}"
            Try
                Return Edges(lsTarget)
            Catch ex As Exception
                Try
                    Dim lsReverseTarget As String = $"{toNodeName}@{edgeVerb}@{fromNodeName}"

                    Return Edges(lsReverseTarget)
                Catch ex1 As Exception
                    Throw New Exception(ex1.Message & " " & lsTarget)
            End Try
            End Try

        End Function

        Private Sub PopulateNodes(nodeObjects As IList(Of GraphStandard.NodeObjectType))

            For Each nodeObject In nodeObjects
                Dim lsName = nodeObject.Labels(0).ref.Substring(4, nodeObject.Labels(0).ref.Length - 4)

                Dim nodeSchema As New GraphSchema.NodeSchema With {
                    .Name = lsName,
                    .NodeIdProperties = New List(Of GraphSchema.EntityProperty) From {
                        New GraphSchema.EntityProperty With {
                            .PropertyName = "id",
                            .DataType = GetType(String),
                            .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.NodeJoinKey
                        }
                    },
                    .Properties = New List(Of GraphSchema.EntityProperty)
                }

                'Dim nodeSchema As New GraphSchema.NodeSchema With {
                '.Name = lsName,
                '.NodeIdProperty = New GraphSchema.EntityProperty With {
                '    .PropertyName = "id",
                '    .DataType = GetType(String),
                '    .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.NodeJoinKey
                '    },
                '.Properties = New List(Of GraphSchema.EntityProperty)
                '}


                For Each lrProperty In nodeObject.Properties

                    nodeSchema.Properties.Add(New GraphSchema.EntityProperty With {
                                    .PropertyName = lrProperty.Token,
                                    .DataType = GetType(String),
                                    .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.RegularProperty
                                }
                            )
                Next
                Nodes.Add(nodeSchema.Name, nodeSchema)
            Next

        End Sub

        Private Sub PopulateNodesFromRDSTables(ByRef aarTable As List(Of RDS.Table))

            For Each lrTable In aarTable
                Dim lsName = lrTable.Name
                Dim lsPKColumnName = ""
                Try
                    lsPKColumnName = lrTable.getPrimaryKeyColumns.First.Name
                Catch ex As Exception
                    lsPKColumnName = "Id"
                End Try
                Dim nodeSchema As New GraphSchema.NodeSchema With {
                    .Name = lsName,
                    .NodeIdProperties = New List(Of GraphSchema.EntityProperty),
                    .Properties = New List(Of GraphSchema.EntityProperty)
                }

                For Each lrColumn In lrTable.getPrimaryKeyColumns

                    nodeSchema.NodeIdProperties.Add(New GraphSchema.EntityProperty With {
                                            .PropertyName = lrColumn.Name,
                                            .DataType = GetType(String),
                                            .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.NodeJoinKey
                                        })
                Next


                For Each lrProperty In lrTable.Column

                    nodeSchema.Properties.Add(New GraphSchema.EntityProperty With {
                                    .PropertyName = lrProperty.Name,
                                    .DataType = GetType(String),
                                    .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.RegularProperty
                                }
                            )
                Next
                Nodes.Add(nodeSchema.Name, nodeSchema)
            Next

        End Sub


        Private Sub PopulateEdges(relationshipObjectTypes As IList(Of GraphStandard.RelationshipObjectType))

            For Each lrType In relationshipObjectTypes
                Dim edgeSchema As New GraphSchema.EdgeSchema With {
                .Name = lrType.Properties(0).Token,
                .SourceNodeId = lrType.From.ref.Substring(3, lrType.From.ref.Length - 3),
                .SourceProperties = New List(Of GraphSchema.EntityProperty) From {
                    New GraphSchema.EntityProperty With {
                        .PropertyName = "srcid",
                        .DataType = GetType(String),
                        .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SourceNodeJoinKey
                    }
                },
                .SinkNodeId = lrType.To.ref.Substring(3, lrType.To.ref.Length - 3),
                .SinkProperties = New List(Of GraphSchema.EntityProperty) From {
                    New GraphSchema.EntityProperty With {
                        .PropertyName = "destid",
                        .DataType = GetType(String),
                        .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SinkNodeJoinKey
                    }
                },
                .Properties = New List(Of GraphSchema.EntityProperty)
            }
                Dim edgeKey As String = $"{edgeSchema.SourceNodeId}@{edgeSchema.Name}@{edgeSchema.SinkNodeId}"
                Edges.Add(edgeKey, edgeSchema)
            Next
        End Sub

        Private Sub PopulateEdgesFromRDSModel(ByRef arRDSModel As RDS.Model)

            For Each lrRelation In arRDSModel.Relation

                'Dim lsOriginPKColumnName = ""
                'Try
                '    lsOriginPKColumnName = lrRelation.OriginColumns.First.Name
                'Catch ex As Exception
                '    lsOriginPKColumnName = "Id"
                'End Try

                'Dim lsDestinationPKColumnName = ""
                'Try
                '    lsDestinationPKColumnName = lrRelation.DestinationColumns.First.Name
                'Catch ex As Exception
                '    lsDestinationPKColumnName = "Id"
                'End Try

                'Dim edgeSchema As New GraphSchema.EdgeSchema With {
                '.Name = lrRelation.ResponsibleFactType.DBName,
                '.SourceNodeId = lrRelation.OriginTable.Name,
                '.SourceProperties = New List(Of GraphSchema.EntityProperty) From {
                '        New GraphSchema.EntityProperty With {
                '            .PropertyName = lsOriginPKColumnName,
                '            .DataType = GetType(String),
                '            .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SourceNodeJoinKey
                '        }
                '    },
                '.SinkNodeId = lrRelation.DestinationTable.Name,
                '.SinkProperties = New List(Of GraphSchema.EntityProperty) From {
                '        New GraphSchema.EntityProperty With {
                '            .PropertyName = lsDestinationPKColumnName,
                '            .DataType = GetType(String),
                '            .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SinkNodeJoinKey
                '        }
                '    },
                '.Properties = New List(Of GraphSchema.EntityProperty)
                '}

                '============New====20230612
                Dim edgeSchema As New GraphSchema.EdgeSchema With {
                    .Name = lrRelation.ResponsibleFactType.DBName,
                    .SourceNodeId = lrRelation.OriginTable.Name,
                    .SourceProperties = New List(Of GraphSchema.EntityProperty),
                    .SinkNodeId = lrRelation.DestinationTable.Name,
                    .SinkProperties = New List(Of GraphSchema.EntityProperty),
                    .Properties = New List(Of GraphSchema.EntityProperty)
                }

                If lrRelation.OriginColumns.Count = 0 Then

                    edgeSchema.SourceProperties.Add(New GraphSchema.EntityProperty With {
                                .PropertyName = "Id",
                                .DataType = GetType(String),
                                .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SourceNodeJoinKey
                            })

                    edgeSchema.SinkProperties.Add(New GraphSchema.EntityProperty With {
                                .PropertyName = "Id",
                                .DataType = GetType(String),
                                .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SinkNodeJoinKey
                            })
                End If

                For Each originColumn In lrRelation.OriginColumns
                    Dim originProperty As New GraphSchema.EntityProperty With {
                        .PropertyName = originColumn.Name,
                        .DataType = GetType(String),
                        .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SourceNodeJoinKey
                    }
                    edgeSchema.SourceProperties.Add(originProperty)
                Next

                For Each destinationColumn In lrRelation.DestinationColumns
                    Dim destinationProperty As New GraphSchema.EntityProperty With {
                        .PropertyName = destinationColumn.Name,
                        .DataType = GetType(String),
                        .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SinkNodeJoinKey
                    }
                    edgeSchema.SinkProperties.Add(destinationProperty)
                Next
                '===========================


                Dim edgeKey As String = $"{edgeSchema.SourceNodeId}@{edgeSchema.Name}@{edgeSchema.SinkNodeId}"
                Edges.Add(edgeKey, edgeSchema)
            Next

            For Each lrTable In arRDSModel.Table.FindAll(Function(x) x.isPGSRelation)

                Dim lrFactType As FBM.FactType = CType(lrTable.FBMModelElement, FBM.FactType)

                Dim lsOriginPKColumnName = ""
                Dim lsOriginTableName = ""
                Try
                    lsOriginPKColumnName = lrFactType.RoleGroup(0).JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns.First.Name
                    lsOriginTableName = lrFactType.RoleGroup(0).JoinedORMObject.Id
                Catch ex As Exception
                    lsOriginPKColumnName = "Id"
                End Try

                Dim lsDestinationPKColumnName = ""
                Dim lsDestinationTableName = ""
                Try
                    lsDestinationPKColumnName = lrFactType.RoleGroup(1).JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns.First.Name
                    lsDestinationTableName = lrFactType.RoleGroup(1).JoinedORMObject.Id
                Catch ex As Exception
                    lsDestinationPKColumnName = "Id"
                End Try

                Dim edgeSchema As New GraphSchema.EdgeSchema With {
                .Name = lrTable.FBMModelElement.Name,
                .SourceNodeId = lsOriginTableName,
                .SourceProperties = New List(Of GraphSchema.EntityProperty),
                .SinkNodeId = lsDestinationTableName,
                .SinkProperties = New List(Of GraphSchema.EntityProperty),
                .Properties = New List(Of GraphSchema.EntityProperty)
                }

                For Each lrColumn In lrFactType.RoleGroup(0).JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns

                    edgeSchema.SourceProperties.Add(New GraphSchema.EntityProperty With {
                            .PropertyName = lrColumn.Name,
                            .DataType = GetType(String),
                            .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SourceNodeJoinKey
                            }
                        )

                Next

                For Each lrColumn In lrFactType.RoleGroup(1).JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns

                    edgeSchema.SinkProperties.Add(New GraphSchema.EntityProperty With {
                            .PropertyName = lrColumn.Name,
                            .DataType = GetType(String),
                            .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SinkNodeJoinKey
                            }
                        )

                Next

                '20230612-VM-Was
                'Dim edgeSchema As New GraphSchema.EdgeSchema With {
                '.Name = lrTable.FBMModelElement.Name,
                '.SourceNodeId = lsOriginTableName,
                '.SourceProperties = New List(Of GraphSchema.EntityProperty) From {
                '    New GraphSchema.EntityProperty With {
                '        .PropertyName = lsOriginPKColumnName,
                '        .DataType = GetType(String),
                '        .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SourceNodeJoinKey
                '      }
                '    },
                '.SinkNodeId = lsDestinationTableName,
                '.SinkProperties = New List(Of GraphSchema.EntityProperty) From {
                '    New GraphSchema.EntityProperty With {
                '        .PropertyName = lsDestinationPKColumnName,
                '        .DataType = GetType(String),
                '        .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SinkNodeJoinKey
                '      }
                '    },
                '.Properties = New List(Of GraphSchema.EntityProperty)
                '}
                Dim edgeKey As String = $"{edgeSchema.SourceNodeId}@{lrTable.FBMModelElement.DBName}@{edgeSchema.SinkNodeId}"
                Edges.Add(edgeKey, edgeSchema)
            Next

        End Sub


        Private Sub PopulateTables(nodeObjectTypes As IList(Of GraphStandard.NodeObjectType), relationshipObjectTypes As IList(Of GraphStandard.RelationshipObjectType))

            For Each nodeObjectType In nodeObjectTypes
                Dim tableDescriptor As New SQLTableDescriptor With {
                .EntityId = nodeObjectType.id.Substring(2, nodeObjectType.id.Length - 2),
                .TableOrViewName = .EntityId
            }
                Tables.Add(tableDescriptor.EntityId, tableDescriptor)
            Next

            For Each relationshipObjectType In relationshipObjectTypes
                Dim tableDescriptor As New SQLTableDescriptor With {
                .EntityId = $"{relationshipObjectType.From.ref}@HAS@{relationshipObjectType.To.ref}",
                .TableOrViewName = $"tbl{relationshipObjectType.From.ref}_HAS_{relationshipObjectType.To.ref}"
            }
                Tables.Add(tableDescriptor.EntityId, tableDescriptor)
            Next
        End Sub

        Private Sub PopulateTablesFromRDSTables(ByRef arRDSModel As RDS.Model)

            For Each lrTable In arRDSModel.Table

                Dim lsEntityId As String = lrTable.Name
                If lrTable.isPGSRelation Then
                    Dim lrFactType As FBM.FactType = CType(lrTable.FBMModelElement, FBM.FactType)
                    lsEntityId = lrFactType.RoleGroup(0).JoinedORMObject.Id & "@" & lrTable.Name & "@" & lrFactType.RoleGroup(1).JoinedORMObject.Id
                End If

                Dim tableDescriptor As New SQLTableDescriptor With {
                        .EntityId = lsEntityId,
                        .TableOrViewName = lrTable.Name
                    }
                Tables.Add(tableDescriptor.EntityId, tableDescriptor)
            Next

            For Each lrRelation In arRDSModel.Relation

                Dim lsEntityId As String = lrRelation.OriginTable.Name & "@" & lrRelation.ResponsibleFactType.DBName & "@" & lrRelation.DestinationTable.Name


                Dim tableDescriptor As New SQLTableDescriptor With {
                        .EntityId = lsEntityId,
                        .TableOrViewName = lrRelation.OriginTable.Name
                    }

                'Tables.Add(tableDescriptor.EntityId, tableDescriptor)
            Next


        End Sub

    End Class

End Namespace