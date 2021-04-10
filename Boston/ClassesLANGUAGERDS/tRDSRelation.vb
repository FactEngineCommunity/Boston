Imports System.Reflection
Imports System.Xml.Serialization

Namespace RDS

    <Serializable()> _
    Public Class Relation
        Implements IEquatable(Of RDS.Relation)

        Public Model As RDS.Model

        <XmlAttribute()> _
        Public Id As String = System.Guid.NewGuid.ToString

        <XmlIgnore()> _
        <NonSerialized()> _
        Public OriginTable As RDS.Table

        Public OriginColumns As New List(Of RDS.Column)
        Public ReverseOriginColumns As New List(Of RDS.Column) 'For 1:1 Binary Relations

        <XmlAttribute()> _
        Public OriginMultiplicity As pcenumCMMLMultiplicity

        <XmlAttribute()> _
        Public RelationOriginIsMandatory As Boolean = False

        <XmlAttribute()> _
        Public OriginPredicate As String = ""

        <XmlAttribute()> _
        Public ContributesToPrimaryKey As Boolean = False

        <XmlIgnore()>
        <NonSerialized()>
        Public WithEvents DestinationTable As RDS.Table

        <XmlElement()> _
        Public DestinationColumns As New List(Of RDS.Column)
        Public ReverseDestinationColumns As New List(Of RDS.Column) 'For 1:1 Binary Relations

        <XmlAttribute()> _
        Public DestinationMultiplicity As pcenumCMMLMultiplicity

        <XmlAttribute()> _
        Public RelationDestinationIsMandatory As Boolean = False

        <XmlAttribute()> _
        Public DestinationPredicate As String = ""

        <XmlAttribute()> _
        Public EnforceReferentialIntegrity As Boolean = False

        <XmlAttribute()> _
        Public CascadingDelete As Boolean = False

        <XmlAttribute()> _
        Public CascadingUpdate As Boolean = False

        <XmlIgnore()> _
        <NonSerialized()> _
        Public WithEvents ResponsibleFactType As FBM.FactType

        Public Event DestinationMandatoryChanged(ByVal abDestinationIsMandatory As Boolean)
        Public Event DestinationMultiplicityChanged(ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity)
        Public Event ResponsibleFactTypeChanged(ByRef arNewResponsibleFactType As FBM.FactType)
        Public Event DestinationPredicateChanged(ByVal asPredicate As String)
        Public Event OriginMandatoryChanged(ByVal abOriginIsMandatory As Boolean)
        Public Event OriginMultiplicityChanged(ByVal aiOriginMultiplicity As pcenumCMMLMultiplicity)
        Public Event OriginPredicateChanged(ByVal asPredicate As String)
        Public Event RemovedFromModel()

        ''' <summary>
        ''' Parameterless New for serialisation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
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

        Public Sub triggerRemovedFromModel()

            RaiseEvent RemovedFromModel()

        End Sub

        Private Sub ResponsibleFactType_RemovedFromModel(abBroadcastInterfaceEvent As Boolean) Handles ResponsibleFactType.RemovedFromModel

            Me.Model.Relation.Remove(Me)

            RaiseEvent RemovedFromModel()

        End Sub

        Private Sub DestinationTable_IndexRemoved(ByRef arIndex As Index) Handles DestinationTable.IndexRemoved

            Try
                If arIndex.IsPrimaryKey Then
                    If Me.OriginColumns.Count <> Me.DestinationColumns.Count Then
                        Debugger.Break()
                        If Me.DestinationTable.getPrimaryKeyColumns.Count > 0 Then
                            If Me.OriginColumns.Count > 0 Then
                                'Should get this far

                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                Debugger.Break()
            End Try


        End Sub

        Public Sub AddDestinationColumn(ByRef arColumn As RDS.Column, ByVal aiOrdinalPosition As Integer)

            Try
                Me.DestinationColumns.Add(arColumn)

                'CMML
                Call Me.Model.Model.addCMMLColumnToRelationDestination(Me, arColumn, aiOrdinalPosition)

            Catch ex As Exception
                Debugger.Break()
            End Try
        End Sub

        Public Sub AddOriginColumn(ByRef arColumn As RDS.Column, ByVal aiOrdinalPosition As Integer)

            Try
                Me.OriginColumns.Add(arColumn)

                'CMML
                Call Me.Model.Model.addCMMLColumnToRelationOrigin(Me, arColumn, aiOrdinalPosition)

            Catch ex As Exception
                Debugger.Break()
            End Try
        End Sub

        Public Sub RemoveOriginColumn(ByRef arColumn As RDS.Column)

            Try
                Me.OriginColumns.Remove(arColumn)

                'CMML
                Call Me.Model.Model.removeCMMLRelationOriginColumn(Me, arColumn)

            Catch ex As Exception
                Debugger.Break()
            End Try
        End Sub

        Private Sub DestinationTable_IndexModified(ByRef arIndex As Index) Handles DestinationTable.IndexModified
            Try
                If arIndex.IsPrimaryKey Then
                    If Me.OriginColumns.Count <> Me.DestinationColumns.Count Then
                        Debugger.Break()
                        If Me.OriginColumns.Count > 0 Then
                            'Should get this far
                            If Me.OriginColumns.Count = 1 And Me.OriginColumns(0).ActiveRole.JoinsEntityType IsNot Nothing Then
                                Debugger.Break()
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
                        End If
                    End If
                End If

            Catch ex As Exception
                Debugger.Break()
            End Try

        End Sub
    End Class

End Namespace
