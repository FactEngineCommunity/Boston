Imports System.Xml.Serialization
Imports System.Reflection

Namespace RDS

    <Serializable()> _
    Public Class Index
        Implements IEquatable(Of RDS.Index)

        Public Model As RDS.Model

        <XmlAttribute()> _
        Public Name As String

        <XmlIgnore()>
        <NonSerialized()>
        Public Table As RDS.Table

        ''' <summary>
        ''' 20200720-VM-In future will need to modify the Core Model to incorporate this, is derived as of today until new Core is created.
        ''' </summary>
        <XmlIgnore()>
        <NonSerialized()>
        Public WithEvents ResponsibleRoleConstraint As FBM.RoleConstraint

        <XmlAttribute()> _
        Public ReadOnly Property TableName As String
            Get
                Return Me.Table.Name
            End Get
        End Property

        <XmlElement()> _
        Public Column As New List(Of RDS.Column)

        <XmlAttribute()> _
        Public IsPrimaryKey As Boolean = False

        <XmlAttribute()> _
        Public NonUnique As Boolean = False

        <XmlAttribute()> _
        Public Unique As Boolean = False

        <XmlAttribute()> _
        Public IndexQualifier As String = ""

        <XmlAttribute()> _
        Public Type As pcenumODBCIndexType

        <XmlAttribute()> _
        Public AscendingOrDescending As pcenumCMMLIndexDirection 'was pcenumODBCAscendingOrDescending

        <XmlAttribute()> _
        Public Cardinality As Integer = 0

        <XmlAttribute()> _
        Public Pages As Integer = 0 'Can be DBNull from ODBC

        <XmlAttribute()> _
        Public FilterCondition As String 'Can be DBNull from ODBC

        <XmlAttribute()> _
        Public IgnoresNulls As Boolean = False

        <NonSerialized()> _
        Public Event ColumnRemoved(ByRef arColumn As RDS.Column)
        Public Event IsPrimaryKeyChanged(ByVal abIsPrimaryKey As Boolean)

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arTable As RDS.Table, ByVal asName As String)

            Me.Model = arTable.Model
            Me.Table = arTable
            Me.Name = asName

        End Sub

        Public Sub New(ByRef arTable As RDS.Table,
                       ByVal asIndexName As String,
                       ByVal asQualifier As String,
                       ByVal aiIndexDirection As pcenumODBCAscendingOrDescending,
                       ByVal abIsPrimaryKey As Boolean,
                       ByVal abRestrainsToUniqueValues As Boolean,
                       ByVal abIndexIgnoresNulls As Boolean,
                       ByVal aarColumn As List(Of RDS.Column),
                       ByVal abAddToTable As Boolean,
                       Optional ByVal abAddIndexToColumns As Boolean = False)

            Me.Model = arTable.Model
            Me.Table = arTable
            Me.Name = asIndexName
            Me.IndexQualifier = asQualifier
            Me.AscendingOrDescending = aiIndexDirection
            Me.IsPrimaryKey = abIsPrimaryKey
            Me.Unique = abRestrainsToUniqueValues
            Me.IgnoresNulls = abIndexIgnoresNulls

            For Each lrColumn In aarColumn
                Me.Column.Add(lrColumn)

                If abAddIndexToColumns Then
                    lrColumn.addIndex(Me)
                End If

                If Me.IsPrimaryKey Then 'Make sure mandatory
                    lrColumn.setMandatory(True)
                End If
            Next

            If abAddToTable Then
                arTable.Index.AddUnique(Me)
            End If

        End Sub

        Public Shadows Function Equals(other As Index) As Boolean Implements IEquatable(Of Index).Equals

            If Me.Column.Count = other.Column.Count Then

                For Each lrColumn In Me.Column
                    If other.Column.Find(Function(x) x.Name = lrColumn.Name And x.ActiveRole.Id = lrColumn.ActiveRole.Id) Is Nothing Then
                        Return False
                    End If
                Next
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsByColumns(other As Index) As Boolean

            If Me.Column.Count = other.Column.Count Then

                For Each lrColumn In Me.Column
                    If other.Column.Find(Function(x) x.Name = lrColumn.Name And x.ActiveRole.Id = lrColumn.ActiveRole.Id) Is Nothing Then
                        Return False
                    End If
                Next
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsByName(ByVal other As RDS.Index) As Boolean
            Return Me.Name = other.Name
        End Function

        Public Sub addColumn(ByRef arColumn As RDS.Column)

            Me.Column.Add(arColumn)
            arColumn.addIndex(Me)

            'CMML
            Call Me.Model.Model.addCMMLColumnToIndex(Me, arColumn)

            'Database synchronisation.
            If Me.Model.Model.IsDatabaseSynchronised Then
                Call Me.Model.Model.connectToDatabase()
                Call Me.Model.Model.DatabaseConnection.IndexAddColumn(Me, arColumn)
            End If

        End Sub

        Public Function getResponsibleRoleConstraintFromORMModel() As FBM.RoleConstraint

            Try
                Dim lrModelObject = Me.Table.FBMModelElement
                Dim lrReferenceModeRoleConstraint As FBM.RoleConstraint

                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        'Responsible Role Constraint is either the PrimaryReferenceScheme Role Constraint
                        '  OR an External Uniqueness Constraint joined to Roles of Fact Types joined to the Entity Type.
                        Dim lrEntityType = CType(lrModelObject, FBM.EntityType)
                        lrReferenceModeRoleConstraint = lrEntityType.ReferenceModeRoleConstraint

                        If lrEntityType.IsSubtype And lrEntityType.HasSimpleReferenceScheme Then
                            Dim lrTopmostSupertype = CType(lrEntityType.GetTopmostSupertype, FBM.EntityType)
                            lrReferenceModeRoleConstraint = lrTopmostSupertype.ReferenceModeRoleConstraint
                        End If
                        If lrEntityType.HasSimpleReferenceScheme Then
                            'PSEUDOCODE
                            ' * Check whether all the ResponsibleRoles of the Columns of the Index are in the FactTypes of the Roles of the RoleConstraint
                            Dim liMatch = (From IdxColumn In Me.Column
                                           From Role In lrReferenceModeRoleConstraint.Role
                                           Where Role.FactType.RoleGroup.Contains(IdxColumn.Role)
                                           Select IdxColumn).Count

                            If liMatch = Me.Column.Count Then
                                Return lrReferenceModeRoleConstraint
                            End If
                        End If
                        If lrEntityType.HasCompoundReferenceMode Then
                            'PSEUDOCODE
                            ' * Check whether all the ResponsibleRoles of the Columns of the Index are in the FactTypes of the Roles of the RoleConstraint
                            Dim liMatch = (From IdxColumn In Me.Column
                                           From Role In lrReferenceModeRoleConstraint.Role
                                           Where Role.FactType.RoleGroup.Contains(IdxColumn.Role)
                                           Where lrReferenceModeRoleConstraint.RoleConstraintRole.Count = Me.Column.Count
                                           Select IdxColumn).Count

                            If liMatch = Me.Column.Count Then
                                Return lrReferenceModeRoleConstraint
                            End If
                        End If

                        '---------------------------------------------------------------
                        'Try and find the RoleConstraint in the FBM Model
                        Dim loRoleConstraint = From RoleConstraint In Me.Model.Model.RoleConstraint
                                               From RoleConstraintRole In RoleConstraint.RoleConstraintRole
                                               From Column In Me.Column
                                               Where RoleConstraintRole.Role.FactType Is Column.Role.FactType
                                               Where RoleConstraint.RoleConstraintRole.Count = Me.Column.Count
                                               Select RoleConstraint Distinct

                        If loRoleConstraint.Count > 0 Then
                            Return loRoleConstraint.First
                        End If

                    'Otherwise, do the same as the above, but for all RoleConstraints in the FBMModel.
                    Case Is = pcenumConceptType.FactType

                End Select

                Return Nothing

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try
        End Function

        Public Sub removeColumn(ByRef arColumn As RDS.Column)

            Try
                Call Me.Column.Remove(arColumn)
                Call arColumn.removeIndex(Me)

                Call Me.Model.Model.removeCMMLPropertyFromIndexByRDSIndexColumn(Me, arColumn)

                RaiseEvent ColumnRemoved(arColumn)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setName(ByVal asNewIndexName As String)

            Try
                Dim lsOriginalName As String = Me.Name

                Me.Name = asNewIndexName

                Call Me.Model.Model.updateCMMLIndexName(lsOriginalName, asNewIndexName)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setIsPrimaryKey(ByVal abIsPrimaryKey As Boolean)

            Try
                Me.IsPrimaryKey = abIsPrimaryKey

                Call Me.Model.Model.updateCMMLIndexIsPrimaryKey(Me, abIsPrimaryKey)

                For Each lrColumn In Me.Column
                    Dim lbPrimaryIndex = (From Index In lrColumn.Index
                                          Where Index.IsPrimaryKey
                                          Select Index).Count > 0

                    'If lbPrimaryIndex Then
                    Call lrColumn.triggerContributesToPrimaryKey(lbPrimaryIndex)
                    'End If
                Next

                RaiseEvent IsPrimaryKeyChanged(abIsPrimaryKey)

                Call Me.Table.RaiseIndexModifiedEvent(Me)

                'Database synchronisation
                If Me.Model.Model.IsDatabaseSynchronised Then
                    Me.Model.Model.connectToDatabase()
                    Call Me.Model.Model.DatabaseConnection.IndexUpdate(Me)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setQualifier(ByVal asNewQualifier As String)

            Try
                Me.IndexQualifier = asNewQualifier

                Call Me.Model.Model.updateCMMLIndexQualifier(Me, asNewQualifier)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub ResponsibleRoleConstraint_RemovedFromModel(abBroadcastInterfaceEvent As Boolean) Handles ResponsibleRoleConstraint.RemovedFromModel

            Call Me.Table.removeIndex(Me)

        End Sub

    End Class

End Namespace
