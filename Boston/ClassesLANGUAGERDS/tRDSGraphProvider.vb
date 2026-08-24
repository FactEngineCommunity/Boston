Imports openCypherTranspiler
Imports openCypherTranspiler.SQLRenderer
Imports openCypherTranspiler.Common
Imports System.Reflection

Namespace RDS

    ''' <summary>
    ''' Used to set up the openCypher to SQL Transpiler.
    ''' </summary>
    Class GraphProvider
        Implements openCypherTranspiler.SQLRenderer.ISQLDBSchemaProvider

        Private Nodes As New Dictionary(Of String, GraphSchema.NodeSchema)
        Private Edges As New Dictionary(Of String, GraphSchema.EdgeSchema)
        Private Tables As New Dictionary(Of String, SQLTableDescriptor)

        Shared Sub New()
        End Sub

        Sub New(graphSchema As GraphStandard.GraphSchemaRepresentation)

            Try
                Me.PopulateNodes(graphSchema.GraphSchema.NodeObjectTypes)
                Me.PopulateEdges(graphSchema.GraphSchema.RelationshipObjectTypes)
                Me.PopulateTables(graphSchema.GraphSchema.NodeObjectTypes, graphSchema.GraphSchema.RelationshipObjectTypes)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Sub New(ByRef lrRDSModel As RDS.Model)

            Try

                Me.PopulateNodesFromRDSTables(lrRDSModel.Table)
                Me.PopulateEdgesFromRDSModel(lrRDSModel)
                Me.PopulateTablesFromRDSTables(lrRDSModel)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

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

            Try

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

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub PopulateNodesFromRDSTables(ByRef aarTable As List(Of RDS.Table))

            Try

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


                    For Each lrProperty In lrTable.Column.FindAll(Function(x) Not x.FactType.IsDerived)

                        nodeSchema.Properties.Add(New GraphSchema.EntityProperty With {
                                        .PropertyName = lrProperty.Name,
                                        .DataType = GetType(String),
                                        .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.RegularProperty
                                    }
                                )
                    Next

                    For Each lrProperty In lrTable.Column.FindAll(Function(x) Not x.FactType.IsDerived And x.Relation.Count > 0)

                        nodeSchema.Properties.Add(New GraphSchema.EntityProperty With {
                                        .PropertyName = lrProperty.Name,
                                        .DataType = GetType(String),
                                        .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SourceNodeJoinKey
                                    }
                                )
                    Next


                    Nodes.Add(nodeSchema.Name, nodeSchema)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Private Sub PopulateEdges(relationshipObjectTypes As IList(Of GraphStandard.RelationshipObjectType))

            Try

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

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub PopulateEdgesFromRDSModel(ByRef arRDSModel As RDS.Model)

            Try
                Dim lrRDSModel = arRDSModel
                Dim larRDSRelation = From Relation In lrRDSModel.Relation
                                     Where Not Relation.ResponsibleFactType.IsCandidatePGSRelationshipNode
                                     Select Relation

                For Each lrRelation In larRDSRelation

#Region "Old Code-Delete After 2024-12-31 if not missed."
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
#End Region

                    '============New====20230612
                    Dim lsGraphLabel = ""
                    Try
                        lsGraphLabel = lrRelation.ResponsibleFactType.PropertyGraphLabel
                    Catch ex As Exception
                        'No loss. Not set
                    End Try

                    Dim edgeSchema As New GraphSchema.EdgeSchema With {
                        .Name = lsGraphLabel, 'lrRelation.ResponsibleFactType.DBName
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

                For Each lrTable In arRDSModel.Table.FindAll(Function(x) x.isPGSRelation Or x.FBMModelElement.IsCandidatePGSRelationshipNode)

                    Dim lrFactType As FBM.FactType = CType(lrTable.FBMModelElement, FBM.FactType)

                    'Get the appropriate Roles.
                    Dim lrFirstRole = lrFactType.RoleGroup.Find(Function(x) x.JoinsValueType Is Nothing)
                    Dim lrSecondRole As FBM.Role = Nothing
                    lrSecondRole = lrFactType.RoleGroup.FindAll(Function(x) x.JoinsValueType Is Nothing)(1)

                    Dim lsOriginPKColumnName = ""
                    Dim lsOriginTableName = ""
                    Try
                        lsOriginPKColumnName = lrFirstRole.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns.First.Name
                        lsOriginTableName = lrFirstRole.JoinedORMObject.Id
                    Catch ex As Exception
                        lsOriginPKColumnName = "Id"
                    End Try

                    Dim lsDestinationPKColumnName = ""
                    Dim lsDestinationTableName = ""
                    Try
                        lsDestinationPKColumnName = lrSecondRole.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns.First.Name
                        lsDestinationTableName = lrSecondRole.JoinedORMObject.Id
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


                    For Each lrColumn In lrFirstRole.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns

                        edgeSchema.SourceProperties.Add(New GraphSchema.EntityProperty With {
                                .PropertyName = lrColumn.Name,
                                .DataType = GetType(String),
                                .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SourceNodeJoinKey
                                }
                            )

                    Next

                    For Each lrColumn In lrSecondRole.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns

                        edgeSchema.SinkProperties.Add(New GraphSchema.EntityProperty With {
                                .PropertyName = lrColumn.Name,
                                .DataType = GetType(String),
                                .PropertyType = GraphSchema.EntityProperty.PropertyDefinitionType.SinkNodeJoinKey
                                }
                            )

                    Next
#Region "Old Code"
                    '20230612-VM-Was-Delete Post 2025-06-01
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
#End Region
                    Dim edgeKey As String = $"{edgeSchema.SourceNodeId}@{lrTable.FBMModelElement.PropertyGraphLabel}@{edgeSchema.SinkNodeId}" 'PropertyGraphLabel
                    If Not Edges.ContainsKey(edgeKey) Then
                        Edges.Add(edgeKey, edgeSchema)
                    End If
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Private Sub PopulateTables(nodeObjectTypes As IList(Of GraphStandard.NodeObjectType), relationshipObjectTypes As IList(Of GraphStandard.RelationshipObjectType))

            Try

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

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub PopulateTablesFromRDSTables(ByRef arRDSModel As RDS.Model)

            Try

                For Each lrTable In arRDSModel.Table

                    Dim lsEntityId As String = lrTable.Name
                    If lrTable.FBMModelElement.IsCandidatePGSNode AndAlso lrTable.FBMModelElement.GetType = GetType(FBM.FactType) Then ' isPGSRelation Then '2024-09-24
                        Dim lrFactType As FBM.FactType = CType(lrTable.FBMModelElement, FBM.FactType)
                        Dim lrFirstRole = lrFactType.RoleGroup.Find(Function(x) x.JoinsValueType Is Nothing)
                        Dim lrSecondRole As FBM.Role = Nothing
                        lrSecondRole = lrFactType.RoleGroup.FindAll(Function(x) x.JoinsValueType Is Nothing)(1)
                        lsEntityId = lrFirstRole.JoinedORMObject.Id & "@" & lrTable.Name & "@" & lrSecondRole.JoinedORMObject.Id
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

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace