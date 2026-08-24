Imports System.ComponentModel
Imports System.Reflection
Imports System.Xml.Serialization
Imports Newtonsoft.Json
Imports Boston.FBM

Namespace RDS

    <Serializable()>
    Public Class Relation
        Implements IEquatable(Of RDS.Relation)
        Implements IDisposable

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public Model As RDS.Model

        <XmlAttribute()>
        Public Id As String = System.Guid.NewGuid.ToString

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public OriginTable As RDS.Table

        <XmlArray("Origin Columns")>
        <XmlArrayItem("Column")>
        Public OriginColumns As New List(Of RDS.Column)

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public ReverseOriginColumns As New List(Of RDS.Column) 'For 1:1 Binary Relations


        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public OriginMultiplicity As pcenumCMMLMultiplicity

        <XmlAttribute>
        Public Property RelationOriginMultiplicity As String
            Get
                Return Me.OriginMultiplicity.ToString
            End Get
            Set(value As String)
                Throw New NotImplementedException("Not implemented. See Relation.OriginMultiplicity")
            End Set
        End Property

        <XmlAttribute()>
        Public RelationOriginIsMandatory As Boolean = False

        <XmlAttribute()>
        Public OriginPredicate As String = ""

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public ContributesToPrimaryKey As Boolean = False

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Private WithEvents _DestinationTable As RDS.Table
        <XmlIgnore>
        <JsonIgnore()>
        Public Property DestinationTable As RDS.Table
            Get
                Return Me._DestinationTable
            End Get
            Set(value As RDS.Table)
                Me._DestinationTable = value
            End Set
        End Property

        <XmlAttribute>
        Public Property DestinationTableName As String
            Get
                Return Me.DestinationTable.Name
            End Get
            Set(value As String)
                Throw New NotImplementedException("Not implemented. See Relation.DestinationTable")
            End Set
        End Property

        <XmlArray("Destination Columns")>
        <XmlArrayItem("Column")>
        Public DestinationColumns As New List(Of RDS.Column)

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public ReverseDestinationColumns As New List(Of RDS.Column) 'For 1:1 Binary Relations

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Public DestinationMultiplicity As pcenumCMMLMultiplicity

        <XmlAttribute>
        Public Property RelationDestinationMultiplicity As String
            Get
                Return Me.DestinationMultiplicity.ToString
            End Get
            Set(value As String)
                Throw New NotImplementedException("Not implemented. See Relation.DestinationMultiplicity")
            End Set
        End Property

        <XmlAttribute()>
        Public RelationDestinationIsMandatory As Boolean = False

        <XmlAttribute()>
        Public DestinationPredicate As String = ""

        <XmlAttribute()>
        Public EnforcesOnCascadeUpdate As Boolean = False

        <XmlAttribute()>
        Public EnforcesOnCascadeDelete As Boolean = False

        <XmlAttribute()>
        Public EnforcesReferentialIntegrity As Boolean = False

        <XmlIgnore>
        <JsonIgnore()>
        Public CascadingDelete As Boolean = False

        <XmlIgnore>
        <JsonIgnore()>
        Public CascadingUpdate As Boolean = False

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Private WithEvents _ResponsibleFactType As FBM.FactType
        <XmlIgnore>
        <JsonIgnore()>
        Public Property ResponsibleFactType As FBM.FactType
            Get
                Return Me._ResponsibleFactType
            End Get
            Set(value As FBM.FactType)
                Me._ResponsibleFactType = value
            End Set
        End Property

        <XmlIgnore()>
        <JsonIgnore()>
        <NonSerialized()>
        Private disposedValue As Boolean

        ''' <summary>
        ''' Used with PGS Links. A PGS Link may be a LinkFactType but its UltimateFactType is its ObjectifiedFactType
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore()>
        <JsonIgnore()>
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

        <NonSerialized()>
        Public Event DestinationMandatoryChanged(ByVal abDestinationIsMandatory As Boolean)
        <NonSerialized()>
        Public Event DestinationMultiplicityChanged(ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity)
        <NonSerialized()>
        Public Event EnforcesOnCascadeUpdateChanged(ByVal abNewEnforcesOnCascadeUpdate As Boolean)
        <NonSerialized()>
        Public Event EnforcesOnCascadeDeleteChanged(ByVal abNewEnforcesOnCascadeDelete As Boolean)
        <NonSerialized()>
        Public Event EnforcesReferentialIntegrityChanged(ByVal abNewEnforcesReferentialIntegrity As Boolean)
        <NonSerialized()>
        Public Event GraphLabelAdded(ByVal asNewGraphLabel As String)
        <NonSerialized()>
        Public Event ResponsibleFactTypeChanged(ByRef arNewResponsibleFactType As FBM.FactType)
        <NonSerialized()>
        Public Event ResponsibleFactTypeFactTypeReadingModified()
        <NonSerialized()>
        Public Event DestinationPredicateChanged(ByVal asPredicate As String)
        <NonSerialized()>
        Public Event DestinationTableChanged(ByRef arTable As RDS.Table)
        <NonSerialized()>
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
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                lrRelation.EnforcesOnCascadeUpdate = .EnforcesOnCascadeUpdate
                lrRelation.EnforcesOnCascadeDelete = .EnforcesOnCascadeDelete
                lrRelation.EnforcesReferentialIntegrity = .EnforcesReferentialIntegrity
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
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function GetGraphPredicate() As String

            Try
                If Me.ResponsibleFactType.GraphLabel.Count > 0 Then
                    Return Me.ResponsibleFactType.PropertyGraphLabel
                Else
                    If Me.ResponsibleFactType.IsManyTo1BinaryFactType Then
                        If Me.ResponsibleFactType.getOutgoingFactTypeReadingPredicates.Count = 0 Then
                            Return "<Error. Add Graph Label>"
                        Else
                            Return Me.ResponsibleFactType.getOutgoingFactTypeReadingPredicates(0)
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, abUseFlashCard:=True)

                Return "<Error>"
            End Try

        End Function

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
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
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub SetEnforcesOnCascadeUpdate(ByVal abEnforcesOnCascadeUpdate As Boolean)

            Try
                Me.EnforcesOnCascadeUpdate = abEnforcesOnCascadeUpdate

                Call Me.Model.Model.setCMMLCoreRelationEnforcesOnCascadeUpdate(Me, abEnforcesOnCascadeUpdate)

                RaiseEvent EnforcesOnCascadeUpdateChanged(abEnforcesOnCascadeUpdate)

                Call Me.Model.Model.MakeDirty(, False,)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub SetEnforcesOnCascadeDelete(ByVal abEnforcesOnCascadeDelete As Boolean)

            Try
                Me.EnforcesOnCascadeDelete = abEnforcesOnCascadeDelete

                Call Me.Model.Model.setCMMLCoreRelationEnforcesOnCascadeDelete(Me, abEnforcesOnCascadeDelete)

                RaiseEvent EnforcesOnCascadeDeleteChanged(abEnforcesOnCascadeDelete)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub SetEnforcesReferentialIntegrity(ByVal abEnforcesReferentialIntegrity As Boolean)

            Try
                Me.EnforcesReferentialIntegrity = abEnforcesReferentialIntegrity

                Call Me.Model.Model.setCMMLCoreRelationEnforcesReferentialIntegrity(Me, abEnforcesReferentialIntegrity)

                RaiseEvent EnforcesReferentialIntegrityChanged(abEnforcesReferentialIntegrity)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Sub setOriginMandatory(ByVal abOriginIsMandatory As Boolean)

            Try
                Me.RelationOriginIsMandatory = abOriginIsMandatory

                RaiseEvent OriginMandatoryChanged(abOriginIsMandatory)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub SetResponsibleFactType(arFactType As FBM.FactType)

            Try
                Me.ResponsibleFactType = arFactType

                Dim lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString & " WHERE Relation = '" & Me.Id & "'"
                Dim lrRecordset As ORMQL.Recordset = Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrRecordset.EOF Then
                    Call Me.Model.Model.addCMMLRelationIsForFactType(Me, arFactType)
                Else
                    lsSQLQuery = $"UPDATE {pcenumCMMLRelations.CoreRelationIsForFactType.ToString}"
                    lsSQLQuery.AppendLine($" SET FactType = '{arFactType.Id}' WHERE Relation = '{Me.Id}'")
                    Me.Model.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub triggerRemovedFromModel()

            Try
                'CodeSafe
                'Remove the Relation from all associated Columns
                Dim larColumn = From Table In Me.Model.Table
                                From Column In Table.Column
                                Where Column.Relation.Contains(Me)
                                Select Column

                For Each lrColumn In larColumn.ToArray
                    Call lrColumn.Relation.Remove(Me)
                Next

                RaiseEvent RemovedFromModel()

                Me.Dispose()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub ResponsibleFactType_RemovedFromModel(abBroadcastInterfaceEvent As Boolean) Handles _ResponsibleFactType.RemovedFromModel

            Try
                'CodeSafe
                'Remove the Relation from all associated Columns
                Dim larColumn = From Table In Me.Model.Table
                                From Column In Table.Column
                                Where Column.Relation.Contains(Me)
                                Select Column

                For Each lrColumn In larColumn.ToArray
                    Call lrColumn.Relation.Remove(Me)
                Next

                Me.Model.Relation.Remove(Me)

                RaiseEvent RemovedFromModel()

                Me.Dispose()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub DestinationTable_IndexRemoved(ByRef arIndex As Index) Handles _DestinationTable.IndexRemoved

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                    liOrdinalPosition = Me.DestinationColumns.Count + 1
                End If

                'CMML
                Call Me.Model.Model.addCMMLColumnToRelationDestination(Me, arColumn, liOrdinalPosition)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub AddOriginColumn(ByRef arColumn As RDS.Column,
                                   Optional ByVal aiOrdinalPosition As Integer = -1)

            Try
                'CodeSafe
                If Me.OriginColumns.Contains(arColumn) Then Exit Sub
                If arColumn Is Nothing Then Exit Sub

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub RemoveDestinationColumn(ByRef arColumn As RDS.Column)

            Try
                Dim lrColumn = arColumn
                Me.DestinationColumns.RemoveAll(Function(x) x.Id = lrColumn.Id)

                'CMML
                Call Me.Model.Model.removeCMMLRelationDestinationColumn(Me, arColumn)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub ReplaceOriginColumnsTable()

            Try
                Dim lsOriginalColumnName As String = "Error-Contact FactEngine"
                Dim lrColumn As RDS.Column = Nothing

                For Each lrDestinationColumn In Me.DestinationTable.getPrimaryKeyColumns

                    Dim larOriginColumn = From Column In Me.OriginColumns
                                          Where Column.ActiveRole.JoinedORMObject.Id = lrDestinationColumn.ActiveRole.JoinedORMObject.Id
                                          Select Column

                    If larOriginColumn.Count = 0 Then
                        'Need to create a new Origin Column or replace one, perhaps.
                        For Each lrColumn In Me.OriginColumns.ToArray
                            Call Me.RemoveOriginColumn(lrColumn)
                        Next

                        larOriginColumn = From Column In Me.OriginTable.Column
                                          Where Column.ActiveRole.JoinedORMObject.Id = lrDestinationColumn.ActiveRole.JoinedORMObject.Id
                                          Select Column

                        If larOriginColumn.Count > 0 Then
                            Call Me.AddOriginColumn(larOriginColumn.First)
                        End If

                    Else
                        larOriginColumn.First.setTable(Me.OriginTable, True)

                        'Check the Role (ResponsibleRole)
                        lrColumn = larOriginColumn.First

                        If Me.ResponsibleFactType.IsCandidatePGSRelationshipNode Then
                        Else
                            If lrColumn.Role.JoinedORMObject.Id <> Me.OriginTable.FBMModelElement.Id Then

                                Select Case Me.ResponsibleFactType.Arity
                                    Case Is = 2
                                        Dim lrRole = Me.ResponsibleFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = Me.OriginTable.FBMModelElement.Id)

                                        If lrRole IsNot Nothing Then
                                            Call lrColumn.setRole(lrRole)
                                        End If
                                End Select
                            End If
                        End If

                    End If
                Next

#Region "Example Code: From RDSRelation Builder: FBM.Model.generateRelationForManyTo1BinaryFactType "
                'Select Case Me.DestinationTable.FBMModelElement.ConceptType
                '    Case Is = pcenumConceptType.EntityType
                '        For Each lrColumn In Me.DestinationColumns
                '            If arRole.FactType.IsLinkFactType Then
                '                lrColumn = Me.OriginTable.Column.Find(Function(x) x.Role.Id = lrRole.FactType.LinkFactTypeRole.Id _
                '                                                                  And x.ActiveRole.Id = lrColumn.ActiveRole.Id)
                '                lsOriginalColumnName = lrColumn.ActiveRole.JoinedORMObject.DBName
                '                larOriginColumn.Add(lrColumn)
                '            Else
                '                If lrOriginTable.Column.Find(Function(x) x.Role.Id = lrRole.Id _
                '                                             And x.ActiveRole.Id = lrColumn.ActiveRole.Id) Is Nothing Then
                '                    '=====================================================
                '                    'The Column doesn't exist in the Table yet.
                '                    lsOriginalColumnName = lrColumn.ActiveRole.JoinedORMObject.Id
                '                    lsColumnName = Me.OriginTable.createUniqueColumnName(lsOriginalColumnName)
                '                    Dim lrNewColumn = New RDS.Column(Me.OriginTable, lsColumnName, lrRole, lrColumn.ActiveRole, lrRole.Mandatory)
                '                    Me.OriginTable.addColumn(lrNewColumn)
                '                End If
                '                lrColumn = Me.OriginTable.Column.Find(Function(x) x.Role.Id = lrRole.Id _
                '                                                                 And x.ActiveRole.Id = lrColumn.ActiveRole.Id)
                '                lsOriginalColumnName = lrColumn.ActiveRole.JoinedORMObject.DBName
                '                larOriginColumn.Add(lrColumn)

                '            End If
                '        Next
                '    Case Is = pcenumConceptType.FactType
                'For Each lrColumn In Me.DestinationTable.Column
                '    lrColumn = Me.OriginTable.Column.Find(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id _
                '                                                             And Not x.Role.FactType.IsPreferredReferenceMode _
                '                                                             And x.Relation.Count = 0
                '                                                             )
                '    lsOriginalColumnName = lrColumn.ActiveRole.JoinedORMObject.DBName
                '    larOriginColumn.Add(lrColumn)
                'Next
                'End Select
#End Region

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub DestinationTable_IndexModified(ByRef arIndex As Index) Handles _DestinationTable.IndexModified

            Try
                'CodeSafe
                If Not Me.Model.Relation.Contains(Me) Then Exit Sub

                Dim lrOriginalColumn As RDS.Column = Nothing
                Dim lrActualTable As RDS.Table = Nothing

                'If arIndex.IsPrimaryKey Then
                Dim lbColumnsArePartOfPrimaryKey As Boolean = False
                Dim lrIndex As RDS.Index = arIndex
                Dim lrPrimaryKeyIndex As RDS.Index = Nothing
                Dim lrColumnRole As FBM.Role = Nothing

                lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)

                'CodeSafe
                If lrPrimaryKeyIndex IsNot Nothing AndAlso lrPrimaryKeyIndex.IndexQualifier <> arIndex.IndexQualifier Then Exit Sub

                lrOriginalColumn = Me.OriginColumns(0)
                lrActualTable = lrOriginalColumn.Table

                lrColumnRole = lrOriginalColumn.Role

#Region "EntityType joined by ActiveRole in OriginColumn"
                Dim larOriginTableColumnsToRemove As New List(Of RDS.Column)

                Dim larColumn = From Column In Me.OriginColumns
                                Where Column.ActiveRole IsNot Nothing
                                Where Column.ActiveRole.JoinedORMObject.Id = lrIndex.Table.Name And Column.ActiveRole.JoinsEntityType IsNot Nothing
                                Select Column

                For Each lrColumn In larColumn.ToArray
                    lrColumnRole = lrColumn.Role
                    Dim lrActualColumn As RDS.Column = lrColumn.Clone(Nothing, Nothing)
                    lrActualColumn.Table = Me.OriginTable 'CodeSafe: Was returning Columns on different Tables. Relevant but wrong Table.

                    If lrActualColumn.isPartOfPrimaryKey Then lbColumnsArePartOfPrimaryKey = True

                    If lrPrimaryKeyIndex IsNot Nothing And lbColumnsArePartOfPrimaryKey Then
                        lrPrimaryKeyIndex.removeColumn(lrActualColumn)
                    End If
                    Me.RemoveOriginColumn(lrColumn)
                    larOriginTableColumnsToRemove.Add(lrActualColumn)
                Next

                larColumn = From Column In Me.OriginTable.Column
                            From IndexColumn In lrIndex.Column
                            Where Column.ActiveRole IsNot Nothing
                            Where Column.ActiveRole.JoinsEntityType IsNot Nothing
                            Where IndexColumn.ActiveRole IsNot Nothing
                            Where IndexColumn.ActiveRole.JoinsEntityType IsNot Nothing
                            Where Column.ActiveRole.JoinedORMObject.Id = IndexColumn.ActiveRole.JoinedORMObject.Id
                            Select Column

                For Each lrColumn In larColumn
                    larOriginTableColumnsToRemove.AddUnique(lrColumn)
                Next

                larColumn = From Column In Me.OriginTable.Column
                            From IndexColumn In lrIndex.Column
                            Where Column.FactType IsNot Nothing
                            Where Column.ActiveRole IsNot Nothing
                            Where Column.ActiveRole.JoinsEntityType IsNot Nothing
                            Where Column.FactType.Id = lrColumnRole.FactType.Id
                            Select Column

                For Each lrColumn In larColumn
                    larOriginTableColumnsToRemove.AddUnique(lrColumn)
                Next

                For Each lrActualTableColumn In larOriginTableColumnsToRemove
                    Me.OriginTable.removeColumn(lrActualTableColumn)
                Next
#End Region

                Dim larOriginColumnsToRemove = Me.OriginColumns.ToList

                For Each lrDestinationColumn In Me.DestinationColumns.ToArray
                    Call Me.RemoveDestinationColumn(lrDestinationColumn)
                Next

                For Each lrOriginColumn In larOriginColumnsToRemove
                    Call Me.RemoveOriginColumn(lrOriginColumn)
                Next

                For Each lrColumn In arIndex.Column

                    Dim lrNewColumn = lrColumn.Clone(Me.OriginTable, Me,, True)
                    If lrColumnRole IsNot Nothing Then
                        lrNewColumn.Role = lrColumnRole
                        lrNewColumn.FactType = lrColumnRole.FactType
                    End If
                    'lrNewColumn.Name = lrNewColumn.Table.createUniqueColumnName(lrNewColumn.Name, lrNewColumn, 0)
                    lrNewColumn.Relation.AddUnique(Me)

                    If Not Me.OriginTable.Column.Contains(lrNewColumn) And lrNewColumn.isPartOfPrimaryKey Then
                        Me.OriginTable.addColumn(lrNewColumn)
                    Else
                        lrNewColumn = Me.OriginTable.Column.Find(AddressOf lrNewColumn.Equals)
                        lrNewColumn.setRole(lrColumnRole)
                        lrNewColumn.FactType = lrColumnRole.FactType
                    End If

                    Me.AddOriginColumn(lrNewColumn)
                    Me.AddDestinationColumn(lrColumn)

                    If lbColumnsArePartOfPrimaryKey And lrPrimaryKeyIndex IsNot Nothing Then
                        lrPrimaryKeyIndex.addColumn(lrNewColumn, True)
                    End If
                Next

                If lbColumnsArePartOfPrimaryKey And lrPrimaryKeyIndex IsNot Nothing Then
                    Call lrPrimaryKeyIndex.Table.triggerIndexModified(lrPrimaryKeyIndex)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub DestinationTable_IndexAdded(ByRef arIndex As Index) Handles _DestinationTable.IndexAdded

            Try
                If arIndex.IsPrimaryKey Then
                    Dim lbColumnsArePartOfPrimaryKey As Boolean = False
                    Dim lrIndex As RDS.Index = arIndex
                    Dim lrPrimaryKeyIndex As RDS.Index = Nothing
                    Dim lrColumnRole As FBM.Role = Nothing

                    lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)

#Region "EntityType joined by ActiveRole in OriginColumn"
                    Dim larOriginTableColumnsToRemove As New List(Of RDS.Column)

                    Dim larColumn = From Column In Me.OriginColumns
                                    Where Column.ActiveRole.JoinedORMObject.Id = lrIndex.Table.Name And Column.ActiveRole.JoinsEntityType IsNot Nothing
                                    Select Column

                    For Each lrColumn In larColumn.ToArray
                        lrColumnRole = lrColumn.Role
                        Dim lrActualColumn As RDS.Column = lrColumn.Clone(Nothing, Nothing)
                        lrActualColumn.Table = Me.OriginTable 'CodeSafe: Was returning Columns on different Tables. Relevant but wrong Table.

                        If lrActualColumn.isPartOfPrimaryKey Then lbColumnsArePartOfPrimaryKey = True

                        If lrPrimaryKeyIndex IsNot Nothing And lbColumnsArePartOfPrimaryKey Then
                            lrPrimaryKeyIndex.removeColumn(lrActualColumn)
                        End If
                        Me.RemoveOriginColumn(lrColumn)
                        larOriginTableColumnsToRemove.Add(lrActualColumn)
                    Next

                    For Each lrActualTableColumn In larOriginTableColumnsToRemove
                        Me.OriginTable.removeColumn(lrActualTableColumn)
                    Next
#End Region

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
                            lrPrimaryKeyIndex.addColumn(lrNewColumn, True)
                        End If
                    Next

                    If lbColumnsArePartOfPrimaryKey And lrPrimaryKeyIndex IsNot Nothing Then
                        Call lrPrimaryKeyIndex.Table.triggerIndexModified(lrPrimaryKeyIndex)
                    End If

                End If

            Catch ex As Exception

            End Try

        End Sub

        Private Sub DestinationTable_IndexColumnAdded(ByRef arIndex As Index, ByRef arColumn As Column) Handles _DestinationTable.IndexColumnAdded

            Try
                If arIndex.IsPrimaryKey Then
                    Dim lrNewColumn = arColumn.Clone(Me.OriginTable, Me,, True)
                    lrNewColumn.Relation.AddUnique(Me)

                    Dim lbColumnsArePartOfPrimaryKey As Boolean = False
                    Try
                        lbColumnsArePartOfPrimaryKey = Me.OriginColumns(0).isPartOfPrimaryKey
                    Catch ex As Exception
                        'Not a biggie. lbColumnsArePartOfPrimaryKey set to false when declared.
                    End Try
                    Dim lrPrimaryKeyIndex As RDS.Index = Nothing
                    lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)

                    If Me.ResponsibleFactType.IsManyTo1BinaryFactType Then
                        lrNewColumn.Role = Me.ResponsibleFactType.RoleGroup.Find(Function(x) x.InternalUniquenessConstraint.Count > 0)
                        lrNewColumn.FactType = lrNewColumn.Role.FactType
                    End If

                    If Not Me.OriginTable.Column.Contains(lrNewColumn) Then

                        If Me.OriginTable.addColumn(lrNewColumn) Then
                            Me.AddOriginColumn(lrNewColumn, Me.OriginColumns.Count)
                            Me.AddDestinationColumn(arColumn, Me.DestinationColumns.Count)

                            If lbColumnsArePartOfPrimaryKey And lrPrimaryKeyIndex IsNot Nothing Then
                                lrPrimaryKeyIndex.addColumn(lrNewColumn)
                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub ResponsibleFactType_FactTypeReadingModified(ByRef arFactTypeReading As FactTypeReading) Handles _ResponsibleFactType.FactTypeReadingModified

            Call Me.setDestinationPredicate(arFactTypeReading.PredicatePart(0).PredicatePartText)

            RaiseEvent ResponsibleFactTypeFactTypeReadingModified()

        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects)
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null
                disposedValue = True
            End If
        End Sub

        ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
        ' Protected Overrides Sub Finalize()
        '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Not often used. Comes into effect when Entity Type is subtype of FactType.
        ''' </summary>
        ''' <param name="arColumn"></param>
        Private Sub DestinationTable_ColumnAdded(ByRef arColumn As Column) Handles _DestinationTable.ColumnAdded

            Try
                If Not arColumn.isPartOfPrimaryKey Then Exit Sub

                Dim lrModelElement As FBM.ModelObject = arColumn.Table.FBMModelElement
                If lrModelElement.GetType = GetType(FBM.EntityType) Then
                    Dim lrEntityType As FBM.EntityType = lrModelElement
                    Dim lrTopmostEntityType As FBM.EntityType = lrEntityType.GetTopmostNonAbsorbedSupertype(True)
                    If lrTopmostEntityType.IsObjectifyingEntityType Then

                        If arColumn.isPartOfPrimaryKey Then

                            'Tidy up
#Region "Tidy up OriginColumns that have an ActiveRole joined to an Entity Type."
                            Dim larColumn = From Column In Me.OriginColumns
                                            Where Column.ActiveRole.JoinsEntityType IsNot Nothing
                                            Select Column

                            For Each lrColumn In larColumn.ToList
                                Call Me.OriginTable.removeColumn(lrColumn)
                                Call Me.OriginColumns.Remove(lrColumn)
                                lrColumn.Relation.Remove(Me)
                                lrColumn.Dispose()
                            Next
                        End If
                    End If
#End Region

                    Dim lrNewColumn = arColumn.Clone(Me.OriginTable, Me,, True)
                    lrNewColumn.Relation.AddUnique(Me)

                    Dim lbColumnsArePartOfPrimaryKey As Boolean = False
                    Try
                        lbColumnsArePartOfPrimaryKey = Me.OriginColumns(0).isPartOfPrimaryKey
                    Catch ex As Exception
                        'Not a biggie. lbColumnsArePartOfPrimaryKey set to false when declared.
                    End Try
                    Dim lrPrimaryKeyIndex As RDS.Index = Nothing
                    lrPrimaryKeyIndex = Me.OriginTable.Index.Find(Function(x) x.IsPrimaryKey)

                    If Me.ResponsibleFactType.IsManyTo1BinaryFactType Then
                        lrNewColumn.Role = Me.ResponsibleFactType.RoleGroup.Find(Function(x) x.InternalUniquenessConstraint.Count > 0)
                        lrNewColumn.FactType = lrNewColumn.Role.FactType
                    End If

                    If Not Me.OriginTable.Column.Contains(lrNewColumn) Then

                        'MsgBox(Me.OriginTable.Name)

                        If Me.OriginTable.addColumn(lrNewColumn) Then
                            Me.AddOriginColumn(lrNewColumn, Me.OriginColumns.Count)
                            Me.AddDestinationColumn(arColumn, Me.DestinationColumns.Count)

                            If lbColumnsArePartOfPrimaryKey And lrPrimaryKeyIndex IsNot Nothing Then
                                lrPrimaryKeyIndex.addColumn(lrNewColumn)
                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub DestinationTable_ColumnRemoved(ByRef arColumn As Column) Handles _DestinationTable.ColumnRemoved

            Try
                'CodeSafe
                Dim lrColumn = arColumn
                If Me.DestinationColumns.FindAll(Function(x) x.Id = lrColumn.Id).Count > 0 Then
                    Call Me.RemoveDestinationColumn(arColumn)
                    If Me.ResponsibleFactType.IsManyTo1BinaryFactType Then
                        Dim lrRole = Me.ResponsibleFactType.RoleGroup.Find(Function(x) x.InternalUniquenessConstraint.Count > 0)
                        Dim larOriginTableColumn = From Column In Me.OriginTable.Column
                                                   Where Column.Role.Id = lrRole.Id
                                                   Where Column.ActiveRole.Id = lrColumn.ActiveRole.Id
                                                   Select Column

                        For Each lrColumn In larOriginTableColumn.ToList
                            Call Me.OriginTable.removeColumn(lrColumn,, Me.OriginTable.FBMModelElement.HasSubTypes)
                        Next
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub _ResponsibleFactType_GraphLabelAdded(asNewGraphLabel As String) Handles _ResponsibleFactType.GraphLabelAdded
            RaiseEvent GraphLabelAdded(asNewGraphLabel)
        End Sub

    End Class

End Namespace
