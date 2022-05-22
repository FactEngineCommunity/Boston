Imports System.Reflection
Imports System.Xml.Serialization
Imports Boston.FBM

Namespace RDS

    <Serializable()>
    Public Class Relation
        Implements IEquatable(Of RDS.Relation)

        Public Model As RDS.Model

        <XmlAttribute()>
        Public Id As String = System.Guid.NewGuid.ToString

        <XmlIgnore()>
        <NonSerialized()>
        Public OriginTable As RDS.Table

        Public OriginColumns As New List(Of RDS.Column)
        Public ReverseOriginColumns As New List(Of RDS.Column) 'For 1:1 Binary Relations

        <XmlAttribute()>
        Public OriginMultiplicity As pcenumCMMLMultiplicity

        <XmlAttribute()>
        Public RelationOriginIsMandatory As Boolean = False

        <XmlAttribute()>
        Public OriginPredicate As String = ""

        <XmlAttribute()>
        Public ContributesToPrimaryKey As Boolean = False

        <XmlIgnore()>
        <NonSerialized()>
        Public WithEvents DestinationTable As RDS.Table

        <XmlElement()>
        Public DestinationColumns As New List(Of RDS.Column)
        Public ReverseDestinationColumns As New List(Of RDS.Column) 'For 1:1 Binary Relations

        <XmlAttribute()>
        Public DestinationMultiplicity As pcenumCMMLMultiplicity

        <XmlAttribute()>
        Public RelationDestinationIsMandatory As Boolean = False

        <XmlAttribute()>
        Public DestinationPredicate As String = ""

        <XmlAttribute()>
        Public EnforceReferentialIntegrity As Boolean = False

        <XmlAttribute()>
        Public CascadingDelete As Boolean = False

        <XmlAttribute()>
        Public CascadingUpdate As Boolean = False

        <XmlIgnore()>
        <NonSerialized()>
        Public WithEvents ResponsibleFactType As FBM.FactType

        ''' <summary>
        ''' Used with PGS Links. A PGS Link may be a LinkFactType but its UltimateFactType is its ObjectifiedFactType
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore()>
        Public ReadOnly Property UltimateFactType As FBM.FactType
            Get
                Dim lrFactType As FBM.FactType = Nothing
                If Me.ResponsibleFactType.IsObjectified Or Me.ResponsibleFactType.IsLinkFactType Then
                    If Me.ResponsibleFactType.IsLinkFactType Then
                        lrFactType = Me.ResponsibleFactType.LinkFactTypeRole.FactType
                    Else
                        lrFactType = Me.ResponsibleFactType
                    End If
                Else
                    lrFactType = Me.ResponsibleFactType
                End If

                Return lrFactType
            End Get
        End Property

        Public Event DestinationMandatoryChanged(ByVal abDestinationIsMandatory As Boolean)
        Public Event DestinationMultiplicityChanged(ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity)
        Public Event ResponsibleFactTypeChanged(ByRef arNewResponsibleFactType As FBM.FactType)
        Public Event ResponsibleFactTypeFactTypeReadingModified()
        Public Event DestinationPredicateChanged(ByVal asPredicate As String)
        Public Event DestinationTableChanged(ByRef arTable As RDS.Table)
        Public Event OriginMandatoryChanged(ByVal abOriginIsMandatory As Boolean)
        Public Event OriginMultiplicityChanged(ByVal aiOriginMultiplicity As pcenumCMMLMultiplicity)
        Public Event OriginPredicateChanged(ByVal asPredicate As String)
        Public Event OriginTableChanged(ByRef arTable As RDS.Table)
        Public Event RemovedFromModel()

        ''' <summary>
        ''' Parameterless New for serialisation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asRelationId As String)
            Me.Id = asRelationId
        End Sub

        Public Sub New(ByVal asRelationId As String,
                       ByRef arOriginTable As RDS.Table,
                       ByVal aiOriginMultiplicity As pcenumCMMLMultiplicity,
                       ByVal abOriginMandatory As Boolean,
                       ByVal abOriginContributesToPrimaryKey As Boolean,
                       ByVal asOriginPredicate As String,
                       ByRef arDestinationTable As RDS.Table,
                       ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity,
                       ByVal abDestinationMandatory As Boolean,
                       ByVal asDestinationPredicate As String,
                       ByRef arResponsibleFactType As FBM.FactType)

            Try
                Me.Id = asRelationId

                Me.OriginTable = arOriginTable
                Me.RelationOriginIsMandatory = abOriginMandatory
                Me.OriginMultiplicity = aiOriginMultiplicity
                Me.OriginPredicate = asOriginPredicate

                Me.DestinationTable = arDestinationTable
                Me.RelationDestinationIsMandatory = abDestinationMandatory
                Me.DestinationMultiplicity = aiDestinationMultiplicity
                Me.DestinationPredicate = asDestinationPredicate

                Me.ResponsibleFactType = arResponsibleFactType

                Me.Model = arOriginTable.Model 'Keep this last. If the arOriginTable does not exist for some reason then most of the Relation is defined.

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function Clone(Optional arOriginTable As RDS.Table = Nothing) As RDS.Relation

            Dim lrRelation As New RDS.Relation

            With Me
                lrRelation.Id = System.Guid.NewGuid.ToString
                lrRelation.CascadingDelete = .CascadingDelete
                lrRelation.CascadingUpdate = .CascadingUpdate
                lrRelation.ContributesToPrimaryKey = .ContributesToPrimaryKey
                lrRelation.DestinationColumns = .DestinationColumns
                lrRelation.DestinationMultiplicity = .DestinationMultiplicity
                lrRelation.DestinationPredicate = .DestinationPredicate
                lrRelation.DestinationTable = .DestinationTable
                lrRelation.EnforceReferentialIntegrity = .EnforceReferentialIntegrity
                lrRelation.Model = .Model
                If arOriginTable IsNot Nothing Then
                    lrRelation.OriginColumns = New List(Of RDS.Column)
                    For Each lrColumn In .OriginColumns
                        Dim lrNewColumn = arOriginTable.Column.Find(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id)
                        If lrNewColumn Is Nothing Then lrNewColumn = lrNewColumn.Clone(arOriginTable, lrRelation) 'Make the Colum for the returned Table/Relation
                        If lrNewColumn.Relation.Find(AddressOf lrRelation.EqualsByOriginColumnsDesinationTable) Is Nothing Then
                            lrNewColumn.Relation.AddUnique(lrRelation)
                        End If
                        lrRelation.OriginColumns.AddUnique(lrNewColumn)
                    Next
                End If
                lrRelation.OriginMultiplicity = .OriginMultiplicity
                lrRelation.OriginPredicate = .OriginPredicate
                If arOriginTable IsNot Nothing Then
                    lrRelation.OriginTable = arOriginTable
                Else
                    lrRelation.OriginTable = .OriginTable
                End If
                lrRelation.RelationDestinationIsMandatory = .RelationDestinationIsMandatory
                lrRelation.RelationOriginIsMandatory = .RelationOriginIsMandatory
                lrRelation.ResponsibleFactType = .ResponsibleFactType
                lrRelation.ReverseDestinationColumns = .ReverseDestinationColumns
                lrRelation.ReverseOriginColumns = .ReverseOriginColumns
            End With

            Return lrRelation
        End Function

        Public Shadows Function Equals(other As Relation) As Boolean Implements IEquatable(Of Relation).Equals

            Return Me.Id = other.Id

        End Function

        Public Function EqualsByOriginColumnsDesinationTable(other As RDS.Relation) As Boolean

            Dim abReturnValue As Boolean = True

            If Me.DestinationTable.Name <> other.DestinationTable.Name Then
                abReturnValue = False
            End If

            For Each lrOriginColumn In Me.OriginColumns
                If other.OriginColumns.Find(Function(x) x.Name = lrOriginColumn.Name And x.ActiveRole.Id = lrOriginColumn.ActiveRole.Id) Is Nothing Then
                    Return False
                End If
            Next

            Return abReturnValue

        End Function

        Public Function EqualsByOriginTableDestinationTable(other As RDS.Relation) As Boolean

            Return Me.OriginTable.Name = other.OriginTable.Name And Me.DestinationTable.Name = other.DestinationTable.Name

        End Function

        Public Function EqualsByOriginColumnsDesinationTableReverseEngineering(other As RDS.Relation) As Boolean

            Dim abReturnValue As Boolean = True

            If Me.DestinationTable.Name <> other.DestinationTable.Name Then
                abReturnValue = False
            End If

            For Each lrOriginColumn In Me.OriginColumns
                If other.OriginColumns.Find(Function(x) x.Name = lrOriginColumn.Name) Is Nothing Then
                    Return False
                End If
            Next

            Return abReturnValue

        End Function


        Public Sub changeResponsibleFactType(ByRef arFactType As FBM.FactType)

            Me.ResponsibleFactType = arFactType

            RaiseEvent ResponsibleFactTypeChanged(arFactType)

        End Sub

        ''' <summary>
        ''' Used for 1:1 Binary Relations. Establishes the Reverse Origin and Destination Columns, based on the PrimaryKey of the OriginTable.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub establishReverseColumns()

            Try
                Dim lrOriginTable, lrDestinationTable As RDS.Table

                If Me.OriginColumns.Count = 0 Or Me.DestinationColumns.Count = 0 Then
                    Exit Sub 'No point in trying to establish the reverse Columns.
                End If

                lrOriginTable = Me.OriginColumns(0).Table
                lrDestinationTable = Me.DestinationColumns(0).Table

                Me.ReverseOriginColumns.Clear()
                Me.ReverseDestinationColumns.Clear()

                For Each lrColumn In lrOriginTable.getPrimaryKeyColumns()
                    Me.ReverseDestinationColumns.Add(lrColumn)
                Next

                For Each lrDestinationColumn In Me.ReverseDestinationColumns

                    Dim lrOriginColumn As RDS.Column

                    lrOriginColumn = lrDestinationTable.Column.Find(Function(x) x.ActiveRole Is lrDestinationColumn.ActiveRole)
                    Me.ReverseOriginColumns.Add(lrOriginColumn)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function is1To1BinaryRelation() As Boolean

            Return (Me.OriginMultiplicity = pcenumCMMLMultiplicity.One) And (Me.DestinationMultiplicity = pcenumCMMLMultiplicity.One)

        End Function

        ''' <summary>
        ''' TRUE if the Relation is for a Column/Columns that are part of the PrimaryKey of the OriginTable.
        ''' </summary>
        ''' <returns></returns>
        Public Function isPrimaryKeyBasedRelation() As Boolean

            Try
                If Me.OriginColumns.Count > 0 Then
                    Return Me.OriginColumns.First.isPartOfPrimaryKey
                Else
                    Return False
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Function


        Public Sub setDestinationMandatory(ByVal abDestinationIsMandatory As Boolean)

            Me.RelationDestinationIsMandatory = abDestinationIsMandatory

            RaiseEvent DestinationMandatoryChanged(abDestinationIsMandatory)

        End Sub

        Public Sub setDestinationMultiplicity(ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity)

            Me.DestinationMultiplicity = aiDestinationMultiplicity

            Call Me.Model.Model.updateCMMLRelationDestinationMultiplicity(Me, aiDestinationMultiplicity)

            RaiseEvent DestinationMultiplicityChanged(aiDestinationMultiplicity)

        End Sub

        Public Sub setDestinationPredicate(ByVal asPredicate As String)

            Try
                Me.DestinationPredicate = asPredicate

                Call Me.Model.Model.updateRelationDestinationPredicate(Me, asPredicate)

                RaiseEvent DestinationPredicateChanged(asPredicate)
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setDestinationTable(ByVal arTable As RDS.Table)

            Try
                Me.DestinationTable = arTable

                'CMML
                Call Me.Model.Model.updateRelationDestinationTable(Me, arTable)

                RaiseEvent DestinationTableChanged(arTable)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setOriginMandatory(ByVal abOriginIsMandatory As Boolean)

            Me.RelationOriginIsMandatory = abOriginIsMandatory

            RaiseEvent OriginMandatoryChanged(abOriginIsMandatory)

        End Sub

        Public Sub setOriginMultiplicity(ByVal aiOriginMultiplicity As pcenumCMMLMultiplicity)

            Me.OriginMultiplicity = aiOriginMultiplicity

            Call Me.Model.Model.updateCMMLRelationOriginMultiplicity(Me, aiOriginMultiplicity)

            RaiseEvent OriginMultiplicityChanged(aiOriginMultiplicity)

        End Sub

        Public Sub setOriginPredicate(ByVal asPredicate As String)

            Me.OriginPredicate = asPredicate

            Call Me.Model.Model.updateRelationOriginPredicate(Me, asPredicate)

            RaiseEvent OriginPredicateChanged(asPredicate)

        End Sub

        Public Sub setOriginTable(ByVal arTable As RDS.Table)

            Try
                Me.OriginTable = arTable

                'CMML
                Call Me.Model.Model.updateRelationOriginTable(Me, arTable)

                RaiseEvent OriginTableChanged(arTable)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub triggerRemovedFromModel()

            RaiseEvent RemovedFromModel()

        End Sub

        Private Sub ResponsibleFactType_RemovedFromModel(abBroadcastInterfaceEvent As Boolean) Handles ResponsibleFactType.RemovedFromModel

            Try

                Me.Model.Relation.Remove(Me)

                RaiseEvent RemovedFromModel()
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub DestinationTable_IndexRemoved(ByRef arIndex As Index) Handles DestinationTable.IndexRemoved

            Try
                If arIndex.IsPrimaryKey Then
                    If Me.OriginColumns.Count <> Me.DestinationColumns.Count Then
                        If Me.DestinationTable.getPrimaryKeyColumns.Count > 0 Then
                            If Me.OriginColumns.Count > 0 Then
                                'Should get this far

                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub AddDestinationColumn(ByRef arColumn As RDS.Column,
                                        Optional ByVal aiOrdinalPosition As Integer = -1)

            Try
                'CodeSafe
                If Me.DestinationColumns.Contains(arColumn) Then Exit Sub

                Me.DestinationColumns.Add(arColumn)

                Dim liOrdinalPosition As Integer = aiOrdinalPosition
                If aiOrdinalPosition = -1 Then
                    liOrdinalPosition = Me.OriginColumns.Count + 1
                End If

                'CMML
                Call Me.Model.Model.addCMMLColumnToRelationDestination(Me, arColumn, liOrdinalPosition)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub AddOriginColumn(ByRef arColumn As RDS.Column,
                                   Optional ByVal aiOrdinalPosition As Integer = -1)

            Try
                'CodeSafe
                If Me.OriginColumns.Contains(arColumn) Then Exit Sub

                Me.OriginColumns.Add(arColumn)

                Dim liOrdinalPosition As Integer = aiOrdinalPosition
                If aiOrdinalPosition = -1 Then
                    liOrdinalPosition = Me.OriginColumns.Count + 1
                End If

                'CMML
                Call Me.Model.Model.addCMMLColumnToRelationOrigin(Me, arColumn, liOrdinalPosition)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub RemoveOriginColumn(ByRef arColumn As RDS.Column)

            Try
                Me.OriginColumns.Remove(arColumn)

                'CMML
                Call Me.Model.Model.removeCMMLRelationOriginColumn(Me, arColumn)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub DestinationTable_IndexModified(ByRef arIndex As Index) Handles DestinationTable.IndexModified
            Try
                If arIndex.IsPrimaryKey Then
                    If Me.OriginColumns.Count <> Me.DestinationColumns.Count Then
                        If Me.OriginColumns.Count > 0 Then
                            'Should get this far
                            If Me.OriginColumns.Count = 1 And Me.OriginColumns(0).ActiveRole.JoinsEntityType IsNot Nothing Then
                                'Column joins, via its ActiveRole, an EntityType rather than the ReferenceScheme ValueTypes of that EntityType.
                                Dim lrOriginalColumn As RDS.Column = Me.OriginColumns(0)
                                Call Me.RemoveOriginColumn(lrOriginalColumn)
                                For Each lrColumn In arIndex.Column
                                    Dim lrNewColumn As New RDS.Column(Me.OriginTable, lrColumn.Name, lrOriginalColumn.Role, lrColumn.ActiveRole)
                                    Call Me.OriginTable.addColumn(lrNewColumn)
                                    lrNewColumn.Relation.Add(Me)
                                    Call Me.AddOriginColumn(lrNewColumn, Me.OriginColumns.Count)
                                    Call Me.AddDestinationColumn(lrColumn, Me.DestinationColumns.Count)
                                Next
                                Call Me.OriginTable.removeColumn(lrOriginalColumn)
                            End If

                            Dim lbColumnsArePartOfPrimaryKey As Boolean = False
                            Dim lrIndex As RDS.Index = arIndex
                            If Me.OriginColumns.FindAll(Function(x) x.ActiveRole.JoinsEntityType IsNot Nothing).Count > 0 Then
                                Dim lrPrimaryKeyIndex As RDS.Index = Nothing
                                lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)
                                For Each lrColumn In Me.OriginColumns.FindAll(Function(x) x.ActiveRole.JoinsEntityType IsNot Nothing).ToArray
                                    If lrColumn.isPartOfPrimaryKey Then lbColumnsArePartOfPrimaryKey = True
                                    Me.OriginColumns.Remove(lrColumn)
                                    Me.OriginTable.removeColumn(lrColumn)
                                    If lrPrimaryKeyIndex IsNot Nothing Then
                                        lrPrimaryKeyIndex.removeColumn(lrColumn)
                                    End If
                                Next

                                '20220226-VM-Found this commented out. If not missed after a while, delete.
                                'Was possibly replaced by Me.DestinationTable.IndexColumnAdded method functionality. Either way, was found like this Feb 2022.
                                'If lbColumnsArePartOfPrimaryKey Then
                                '    lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)
                                'End If

                                'For Each lrColumn In arIndex.Column

                                '    Dim lrNewColumn = lrColumn.Clone(Me.OriginTable, Me)
                                '    lrNewColumn.Relation.AddUnique(Me)

                                '    If Me.OriginTable.addColumn(lrNewColumn) Then
                                '        Me.AddOriginColumn(lrNewColumn, Me.OriginColumns.Count)
                                '        Me.AddDestinationColumn(lrColumn, Me.DestinationColumns.Count)

                                '        If lbColumnsArePartOfPrimaryKey And lrPrimaryKeyIndex IsNot Nothing Then
                                '            lrPrimaryKeyIndex.addColumn(lrNewColumn)
                                '        End If
                                '    End If
                                'Next
                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub DestinationTable_IndexAdded(ByRef arIndex As Index) Handles DestinationTable.IndexAdded

            Try
                If arIndex.IsPrimaryKey Then
                    Dim lbColumnsArePartOfPrimaryKey As Boolean = False
                    Dim lrIndex As RDS.Index = arIndex
                    Dim lrPrimaryKeyIndex As RDS.Index = Nothing
                    Dim lrColumnRole As FBM.Role = Nothing

                    lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)

                    Dim larColumn = From Column In Me.OriginColumns
                                    Where Column.ActiveRole.JoinedORMObject.Id = lrIndex.Table.Name And Column.ActiveRole.JoinsEntityType IsNot Nothing
                                    Select Column

                    For Each lrColumn In larColumn.ToArray
                        lrColumnRole = lrColumn.Role
                        Dim lrActualColumn As RDS.Column = lrColumn.Clone(Nothing, Nothing)
                        lrActualColumn.Table = Me.OriginTable 'CodeSafe: Was returning Columns on different Tables. Relevant but wrong Table.

                        If lrActualColumn.isPartOfPrimaryKey Then lbColumnsArePartOfPrimaryKey = True

                        If lrPrimaryKeyIndex IsNot Nothing Then
                            lrPrimaryKeyIndex.removeColumn(lrActualColumn)
                        End If
                        Me.RemoveOriginColumn(lrColumn)
                        Me.OriginTable.removeColumn(lrActualColumn)
                    Next

                    If lbColumnsArePartOfPrimaryKey Then
                        lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)
                    End If

                    For Each lrColumn In arIndex.Column

                        Dim lrNewColumn = lrColumn.Clone(Me.OriginTable, Me,, True)
                        If lrColumnRole IsNot Nothing Then
                            lrNewColumn.Role = lrColumnRole
                            lrNewColumn.FactType = lrColumnRole.FactType
                        End If
                        lrNewColumn.Name = lrNewColumn.Table.createUniqueColumnName(lrNewColumn.Name, lrNewColumn, 0)
                        lrNewColumn.Relation.AddUnique(Me)

                        Me.OriginTable.addColumn(lrNewColumn)

                        Me.AddOriginColumn(lrNewColumn)
                        Me.AddDestinationColumn(lrColumn)

                        If lbColumnsArePartOfPrimaryKey And lrPrimaryKeyIndex IsNot Nothing Then
                            lrPrimaryKeyIndex.addColumn(lrNewColumn)
                        End If
                    Next
                End If

            Catch ex As Exception

            End Try

        End Sub

        Private Sub DestinationTable_IndexColumnAdded(ByRef arIndex As Index, ByRef arColumn As Column) Handles DestinationTable.IndexColumnAdded

            Try
                If arIndex.IsPrimaryKey Then
                    Dim lrNewColumn = arColumn.Clone(Me.OriginTable, Me)
                    lrNewColumn.Relation.AddUnique(Me)

                    Dim lbColumnsArePartOfPrimaryKey As Boolean = False
                    Try
                        lbColumnsArePartOfPrimaryKey = Me.OriginColumns(0).isPartOfPrimaryKey
                    Catch ex As Exception
                        'Not a biggie. lbColumnsArePartOfPrimaryKey set to false when declared.
                    End Try
                    Dim lrPrimaryKeyIndex As RDS.Index = Nothing
                    lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)

                    If Me.OriginTable.addColumn(lrNewColumn) Then
                        Me.AddOriginColumn(lrNewColumn, Me.OriginColumns.Count)
                        Me.AddDestinationColumn(arColumn, Me.DestinationColumns.Count)

                        If lbColumnsArePartOfPrimaryKey And lrPrimaryKeyIndex IsNot Nothing Then
                            lrPrimaryKeyIndex.addColumn(lrNewColumn)
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub ResponsibleFactType_FactTypeReadingModified(ByRef arFactTypeReading As FactTypeReading) Handles ResponsibleFactType.FactTypeReadingModified

            RaiseEvent ResponsibleFactTypeFactTypeReadingModified()

        End Sub

    End Class

End Namespace
