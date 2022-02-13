Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace FBM
    <Serializable()> _
    Public Class tSubtypeRelationship
        Inherits FBM.ModelObject
        Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship)
        Implements IEquatable(Of FBM.tSubtypeRelationship)

        <XmlIgnore()>
        Public Overrides Property ConceptType As pcenumConceptType
            Get
                Return pcenumConceptType.SubtypeRelationship
            End Get
            Set(value As pcenumConceptType)
                'Nothing to do here.
            End Set
        End Property

        <XmlIgnore()>
        Public ModelElement As New FBM.ModelObject 'The EntityType for which the SubtypeConstraint is applicable

        <XmlIgnore()>
        Public parentModelElement As New FBM.ModelObject 'The Parent EntityType to Me.EntityType

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _IsPrimarySubtypeRelationship As Boolean = False

        <XmlAttribute>
        Public Property IsPrimarySubtypeRelationship As Boolean
            Get
                Return Me._IsPrimarySubtypeRelationship
            End Get
            Set(value As Boolean)
                Me._IsPrimarySubtypeRelationship = value
            End Set
        End Property

        ''' <summary>
        ''' The corresponding FactType that represents this SubtypeConstraint.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        Public FactType As New FBM.FactType

        Public Event IsPrimarySubtypeRelationshipChanged(ByVal abIsPrimarySubtypeRelationship As Boolean)

        Public Sub New()

        End Sub

        Public Sub New(ByRef arEntityType As FBM.ModelObject,
                       ByRef arParentEntityType As FBM.ModelObject,
                       ByRef arSubtypingFactType As FBM.FactType)

            Me.Model = arEntityType.Model
            Me.ModelElement = arEntityType
            Me.parentModelElement = arParentEntityType
            Me.FactType = arSubtypingFactType

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arModel">The target Model that the SubtypeRelationship is being cloned to.</param>
        ''' <param name="abAddToModel">TRUE if all relevant attributes are also cloned to arModel, else FALSE</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Clone(ByRef arModel As FBM.Model, Optional abAddToModel As Boolean = False) As Object

            Dim lrSubtypeRelationship As New FBM.tSubtypeRelationship

            With Me
                lrSubtypeRelationship.Model = arModel
                lrSubtypeRelationship.ModelElement = .ModelElement.Clone(arModel, abAddToModel)
                lrSubtypeRelationship.parentModelElement = .parentModelElement.Clone(arModel, abAddToModel)
                lrSubtypeRelationship.FactType = .FactType.Clone(arModel, abAddToModel)
                lrSubtypeRelationship.IsPrimarySubtypeRelationship = .IsPrimarySubtypeRelationship
                lrSubtypeRelationship.isDirty = True

                If abAddToModel Then
                    If lrSubtypeRelationship.ModelElement.SubtypeRelationship.Contains(lrSubtypeRelationship) Then
                        '------------------------------------------------------------------------------------------------------------
                        'The SubtypeRelationship already exists within the set of SubtypeRelationships for the EntityType (Subtype)
                        '------------------------------------------------------------------------------------------------------------
                    Else
                        lrSubtypeRelationship.ModelElement.SubtypeRelationship.Add(lrSubtypeRelationship)
                    End If
                End If
            End With

            Return lrSubtypeRelationship

        End Function

        <MethodImplAttribute(MethodImplOptions.Synchronized)>
        Public Overrides Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.ModelObject

            Dim lrSubtypeConstraintInstance As New FBM.SubtypeRelationshipInstance

            Try
                With Me
                    lrSubtypeConstraintInstance.Page = arPage
                    lrSubtypeConstraintInstance.Model = arPage.Model

                    lrSubtypeConstraintInstance.ModelElement = arPage.getModelElementById(.ModelElement.Id)

                    lrSubtypeConstraintInstance.parentModelElement = arPage.getModelElementById(.parentModelElement.Id)

                    Dim lrFactTypeInstance As New FBM.FactTypeInstance
                    lrFactTypeInstance.Id = .FactType.Id
                    lrFactTypeInstance = arPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                    If lrFactTypeInstance Is Nothing Then
                        lrFactTypeInstance = .FactType.CloneInstance(arPage, abAddToPage)
                    End If
                    lrSubtypeConstraintInstance.FactType = lrFactTypeInstance
                    lrSubtypeConstraintInstance.FactType.SubtypeConstraintInstance = lrSubtypeConstraintInstance

                    lrSubtypeConstraintInstance.IsPrimarySubtypeRelationship = .IsPrimarySubtypeRelationship

                    lrSubtypeConstraintInstance.SubtypeRelationship = Me

                End With

                Return lrSubtypeConstraintInstance

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Shadows Function Equals(ByVal other As FBM.tSubtypeRelationship) As Boolean Implements System.IEquatable(Of FBM.tSubtypeRelationship).Equals

            Return (Me.ModelElement.Id = other.ModelElement.Id) And (Me.parentModelElement.Id = other.parentModelElement.Id)

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        Public Shadows Sub RemoveFromModel()

            Try
                Call Me.ModelElement.RemoveSubtypeRelationship(Me)
                Call Me.FactType.RemoveFromModel(True, False, True, True)
                Call Me.Delete()

                'RDS
                Dim lrTable = CType(Me.ModelElement, FBM.EntityType).getCorrespondingRDSTable

                If lrTable IsNot Nothing Then
                    If lrTable.getPrimaryKeyColumns.Count > 0 Then
                        If lrTable.getPrimaryKeyColumns(0).Role.JoinedORMObject IsNot Me Then
                            'Must have got the Primary Key from a Supertype.
                            Call lrTable.removeExistingPrimaryKeyColumnsAndIndex(True)
                        End If
                    End If

                    Call lrTable.removeSupertypeColumns(Me)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Shadows Sub Save(Optional ByRef abRapidSave As Boolean = False) Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Save

            If abRapidSave Then
                Call TableSubtypeRelationship.add_parentEntityType(Me)
                Me.isDirty = False
            Else
                If TableSubtypeRelationship.exists_parentEntityType(Me) Then
                    Call TableSubtypeRelationship.update_parentEntityType(Me)
                Else
                    Call TableSubtypeRelationship.add_parentEntityType(Me)
                End If
                Me.isDirty = False
            End If

        End Sub

        Public Sub setIsPrimarySubtypeRelationship(ByVal abIsPrimarySubtypeRelationship As Boolean)

            Try

                If abIsPrimarySubtypeRelationship And Me.ModelElement.SubtypeRelationship.Count > 1 Then
                    Dim larSubtypeRelationship = From SubtypeRelationship In Me.ModelElement.SubtypeRelationship
                                                 Where SubtypeRelationship IsNot Me
                                                 Select SubtypeRelationship

                    For Each lrSubtypeReltionship In larSubtypeRelationship.ToList
                        Call lrSubtypeReltionship.setIsPrimarySubtypeRelationship(False)
                    Next
                End If

                '====================================================================================
                'RDS
                If Not abIsPrimarySubtypeRelationship And Me.IsPrimarySubtypeRelationship Then
                    'Remove the PrimaryKey from the supertype
                    Try
                        Dim larSupertypePKColumn = Me.parentModelElement.getCorrespondingRDSTable.getPrimaryKeyColumns

                        For Each lrColumn In larSupertypePKColumn
                            Dim lrSubtypeTable = Me.ModelElement.getCorrespondingRDSTable
                            Dim lrSubtypeColumn As RDS.Column = lrSubtypeTable.Column.Find(Function(x) x.Role Is lrColumn.Role)
                            Me.ModelElement.getCorrespondingRDSTable.removeColumn(lrSubtypeColumn)
                        Next

                    Catch ex As Exception
                        'Not a biggie if Fails.
                    End Try
                End If

                Me.IsPrimarySubtypeRelationship = abIsPrimarySubtypeRelationship

                RaiseEvent IsPrimarySubtypeRelationshipChanged(abIsPrimarySubtypeRelationship)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub Create() Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Create

            Call TableSubtypeRelationship.add_parentEntityType(Me)
        End Sub

        Public Sub Delete() Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Delete
            Call TableSubtypeRelationship.DeleteParentEntityType(Me)
        End Sub

        Public Function Load() As FBM.tSubtypeRelationship Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Load

            Return Me
        End Function

    End Class
End Namespace