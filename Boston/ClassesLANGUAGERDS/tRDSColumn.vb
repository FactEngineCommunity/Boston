Imports System.Reflection
Imports System.Xml.Serialization

Namespace RDS

    <Serializable()> _
    Public Class Column
        Implements IEquatable(Of RDS.Column)

        <XmlIgnore()> _
        <NonSerialized()> _
        Public Model As RDS.Model

        <XmlIgnore()> _
        <NonSerialized()> _
        Public Table As RDS.Table

        <XmlIgnore()> _
        <NonSerialized()> _
        Public Index As New List(Of RDS.Index)

        Public Relation As New List(Of RDS.Relation)

        <XmlAttribute()> _
        Public Id As String = System.Guid.NewGuid.ToString

        <XmlAttribute()> _
        Public Name As String = ""

        <XmlAttribute()> _
        Public OrdinalPosition As Integer = 1

        <XmlElement()> _
        Public DataType As RDS.DataType

        <XmlAttribute()> _
        Public DataTypeName As String

        <XmlAttribute()> _
        Public ODBCDataType As Integer = 0

        <XmlAttribute()> _
        Public Nullable As Boolean = False

        Public IsMandatory As Boolean = False

        <XmlAttribute()> _
        Public IsNullable As Boolean = False

        <XmlAttribute()> _
        Public TableCategory As String

        <XmlAttribute()> _
        Public TableSchema As String

        <XmlAttribute()> _
        Public ColumnSize As Integer

        <XmlAttribute()> _
        Public BufferLength As Integer

        <XmlAttribute()> _
        Public DecimalDigits As Integer

        <XmlAttribute()> _
        Public NumPrecRadix As Integer

        <XmlAttribute()> _
        Public Remarks As String = ""

        <XmlAttribute()> _
        Public ColumnDef As String = ""

        <XmlAttribute()> _
        Public SQLDataType As Integer = 0

        <XmlAttribute()> _
        Public SQLDateTimeSub As String = ""

        <XmlAttribute()> _
        Public CharOctetLength As Integer = 0

        <XmlAttribute()> _
        Public SSDataType As Integer = 0

        <XmlAttribute()> _
        Public ContributesToPrimaryKey As Boolean = False

        'CrossLayer Members
        ''' <summary>
        ''' The FactType (usually Binary) that is responsible for this Column within the Model from which this 
        '''   Column is derived.
        ''' </summary>
        ''' <remarks></remarks>        
        <XmlIgnore()> _
        <NonSerialized()> _
        Public FactType As FBM.FactType

        ''' <summary>
        ''' The Role that was responsible for the derivation of the Column.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        <NonSerialized()> _
        Public WithEvents Role As FBM.Role

        ''' <summary>
        ''' The ultimate Role that resulted in the Column. Where ObjectifiedFactTypes are refererced by other Roles, the first Role is the responsible Role...the nested/absorbed Role is the ActiveRole
        '''   e.g. See the TimetableBookings Page of the University Model.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        <NonSerialized()> _
        Public WithEvents ActiveRole As FBM.Role

        Public Event ActiveRoleChanged()
        Public Event AddedToPrimaryKey()
        Public Event IndexAdded(ByRef arIndex As RDS.Index)
        Public Event IndexRemoved(ByRef arIndex As RDS.Index)
        Public Event IsMandatoryChanged(ByRef abIsMandatory As Boolean)
        Public Event NameChanged(ByVal asNewName As String)
        Public Event ContributesToPrimaryKeyChanged(ByVal abContributesToPrimaryKey As Boolean)
        Public Event forceRefresh()

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arTable As RDS.Table, asName As String,
                       ByRef arResponsibleRole As FBM.Role,
                       ByRef arActiveRole As FBM.Role,
                       Optional ByVal abIsMandatory As Boolean = False)

            Me.Model = arTable.Model
            Me.Table = arTable
            Me.Name = asName
            Me.Role = arResponsibleRole
            Me.ActiveRole = arActiveRole
            Me.Nullable = Not abIsMandatory

        End Sub

        Public Shadows Function Equals(other As Column) As Boolean Implements IEquatable(Of Column).Equals

            Return ((Me.Table.Name = other.Table.Name) And (Me.Name = other.Name)) And (Me.ActiveRole.Id = other.ActiveRole.Id)

        End Function

        Public Sub addIndex(ByRef arIndex As RDS.Index)

            Me.Index.AddUnique(arIndex)

            RaiseEvent IndexAdded(arIndex)

        End Sub

        Public Sub triggerForceRefreshEvent()

            RaiseEvent forceRefresh()

        End Sub

        Public Function getAttributeName() As String

            Try
                Dim lsAttributeName As String = ""

                'CodeSafe
                If Me.ActiveRole Is Nothing Then Throw New Exception("Tried to get an AttributeName for a Column with no Active Role")

                Select Case Me.ActiveRole.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        lsAttributeName = Me.ActiveRole.JoinedORMObject.Name
                    Case Is = pcenumConceptType.EntityType
                        Dim lrEntityType As FBM.EntityType
                        lrEntityType = Me.ActiveRole.JoinedORMObject
                        If lrEntityType.HasSimpleReferenceScheme Then
                            lsAttributeName &= lrEntityType.ReferenceModeValueType.Id
                        Else
                            lsAttributeName = Me.ActiveRole.JoinedORMObject.Name
                        End If
                    Case Else
                        lsAttributeName = Me.ActiveRole.JoinedORMObject.Name
                End Select

                lsAttributeName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsAttributeName))
                lsAttributeName = Me.Table.createUniqueColumnName(Me, lsAttributeName, 0)

                Return lsAttributeName

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return "Error"
            End Try

        End Function

        Public Function getMetamodelDataType() As pcenumORMDataType

            Try
                'Get it using the FactType of the Function.
                If Me.ActiveRole.FactType.IsUnaryFactType Then
                    Return pcenumORMDataType.Boolean
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then 'FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then
                    Return Me.ActiveRole.JoinsValueType.DataType
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    If Me.ActiveRole.JoinsEntityType.HasSimpleReferenceScheme Then
                        Return Me.ActiveRole.JoinsEntityType.getDataType
                    Else
                        Return pcenumORMDataType.DataTypeNotSet 'Throw New Exception("Column references an Entity Type, but the Entity Type does not have a Simple Reference Scheme.")
                    End If
                Else
                    Return pcenumORMDataType.DataTypeNotSet
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function getMetamodelDataTypeLength() As Integer

            Try
                'Get it using the FactType of the Function.
                If Me.ActiveRole.FactType.IsUnaryFactType Then
                    Return 0
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then 'FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then
                    Return Me.ActiveRole.JoinsValueType.DataTypeLength
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    If Me.ActiveRole.JoinsEntityType.HasSimpleReferenceScheme Then
                        Return Me.ActiveRole.JoinsEntityType.ReferenceModeValueType.DataTypeLength
                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function getMetamodelDataTypePrecision() As Integer

            Try
                'Get it using the FactType of the Function.
                If Me.ActiveRole.FactType.IsUnaryFactType Then
                    Return 0
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then 'FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then
                    Return Me.ActiveRole.JoinsValueType.DataTypePrecision
                ElseIf Me.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                    If Me.ActiveRole.JoinsEntityType.HasSimpleReferenceScheme Then
                        Return Me.ActiveRole.JoinsEntityType.ReferenceModeValueType.DataTypePrecision
                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function hasNonPrimaryKeyColumnsAboveIt() As Boolean

            Dim liNonPrimaryKeyCount = Aggregate Column In Me.Table.Column
                                       Where Column.ContributesToPrimaryKey = False _
                                       And Column.OrdinalPosition < Me.OrdinalPosition
                                       Into Count()

            Return liNonPrimaryKeyCount > 0

        End Function

        ''' <summary>
        ''' The Column is part of a Relation mapping the Column's Table to another Table, and may be one of many Columns in the associated Relation.
        ''' </summary>
        ''' <returns></returns>
        Public Function isForeignKey() As Boolean

            Return Me.Relation.Count > 0

        End Function

        ''' <summary>
        ''' Used to check if the Column is part of the PK Index for the Table. Especially when adding a new Unique Index to a Table,
        '''   such that the ERD.Attribute.PartOfPrimaryKey is not set to False.
        ''' </summary>
        ''' <returns></returns>
        Public Function isPartOfPrimaryKey() As Boolean

            Return Me.Model.Index.Find(Function(x) x.IsPrimaryKey And (x.Column.Find(Function(y) y.Id = Me.Id) IsNot Nothing)) IsNot Nothing

        End Function

        Public Sub moveToOrdinalPosition(ByVal aiOrdinalPosition As Integer)

            Try
                If aiOrdinalPosition > Me.Table.Column.Count Then
                    Throw New Exception("Tried to move the Column to an Ordinal Position outside the range of Columns within the Table.")
                End If

                Dim larColumn = From Column In Me.Table.Column _
                                Where Column.OrdinalPosition >= aiOrdinalPosition _
                                Select Column

                For Each lrColumn In larColumn
                    lrColumn.OrdinalPosition += 1
                    Call Me.Model.Model.setCMMLAttributeOrdinalPosition(lrColumn.Id, lrColumn.OrdinalPosition)
                Next

                Me.OrdinalPosition = aiOrdinalPosition
                Call Me.Model.Model.setCMMLAttributeOrdinalPosition(Me.Id, Me.OrdinalPosition)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeIndex(ByRef arIndex As RDS.Index)

            Call Me.Index.Remove(arIndex)

            RaiseEvent IndexRemoved(arIndex)

        End Sub

        Public Sub setActiveRole(ByRef arActiveRole As FBM.Role)

            Me.ActiveRole = arActiveRole

            Call Me.Model.Model.updateORSetCMMLPropertyActiveRole(Me)

            RaiseEvent ActiveRoleChanged()

        End Sub

        Public Sub setContributesToPrimaryKey(ByVal abContributesToPrimaryKey As Boolean)

            Me.ContributesToPrimaryKey = abContributesToPrimaryKey

            RaiseEvent ContributesToPrimaryKeyChanged(abContributesToPrimaryKey)

        End Sub

        Public Sub setMandatory(ByVal abIsMandatory As Boolean)

            Me.IsMandatory = abIsMandatory
            Me.IsNullable = Not IsMandatory

            If abIsMandatory Then
                Call Me.Model.Model.createCMMLAttributeIsMandatory(Me)
            Else
                Call Me.Model.Model.removeCMMLAttributeIsMandatory(Me)
            End If

            RaiseEvent IsMandatoryChanged(abIsMandatory)

        End Sub

        Public Sub setName(ByVal asNewName As String)

            Me.Name = asNewName

            RaiseEvent NameChanged(asNewName)

        End Sub

    End Class

End Namespace
