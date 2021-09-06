Imports System.Xml.Serialization
Imports System.Reflection

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

        <XmlIgnore()> _
        Public EntityType As New FBM.ModelObject 'The EntityType for which the SubtypeConstraint is applicable

        <XmlIgnore()>
        Public parentEntityType As New FBM.ModelObject 'The Parent EntityType to Me.EntityType

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _IsPrimarySubtypeRelationship As Boolean = False

        <XmlAttribute>
        Public Property IsPrimarySubtypeRelationship As Boolean
            Get
                If Me.EntityType.SubtypeRelationship.Count = 1 Then
                    Return True
                Else
                    Return Me._IsPrimarySubtypeRelationship
                End If

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

        Public Sub New(ByRef arEntityType As FBM.EntityType, ByRef arParentEntityType As FBM.EntityType, ByRef arSubtypingFactType As FBM.FactType)

            Me.Model = arEntityType.Model
            Me.EntityType = arEntityType
            Me.parentEntityType = arParentEntityType
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
                lrSubtypeRelationship.EntityType = .EntityType.Clone(arModel, abAddToModel)
                lrSubtypeRelationship.parentEntityType = .parentEntityType.Clone(arModel, abAddToModel)
                lrSubtypeRelationship.FactType = .FactType.Clone(arModel, abAddToModel)
                lrSubtypeRelationship.isDirty = True

                If abAddToModel Then
                    If lrSubtypeRelationship.EntityType.SubtypeRelationship.Contains(lrSubtypeRelationship) Then
                        '------------------------------------------------------------------------------------------------------------
                        'The SubtypeRelationship already exists within the set of SubtypeRelationships for the EntityType (Subtype)
                        '------------------------------------------------------------------------------------------------------------
                    Else
                        lrSubtypeRelationship.EntityType.SubtypeRelationship.Add(lrSubtypeRelationship)
                    End If
                End If
            End With

            Return lrSubtypeRelationship

        End Function


        Public Overrides Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.ModelObject

            Dim lrSubtypeConstraintInstance As New FBM.SubtypeRelationshipInstance

            Try
                With Me
                    lrSubtypeConstraintInstance.Page = arPage
                    lrSubtypeConstraintInstance.Model = arPage.Model

                    lrSubtypeConstraintInstance.EntityType = arPage.EntityTypeInstance.Find(Function(x) x.Id = .EntityType.Id)

                    lrSubtypeConstraintInstance.parentEntityType = arPage.EntityTypeInstance.Find(Function(x) x.Id = .parentEntityType.Id)

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

            If (Me.EntityType.Id = other.EntityType.Id) And (Me.parentEntityType.Id = other.parentEntityType.Id) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        Public Shadows Sub RemoveFromModel()

            Try
                Call Me.EntityType.RemoveSubtypeRelationship(Me)
                Call Me.FactType.RemoveFromModel(True, False, True, True)
                Call Me.Delete()

                'RDS
                Dim lrTable = CType(Me.EntityType, FBM.EntityType).getCorrespondingRDSTable

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
            Else
                If TableSubtypeRelationship.exists_parentEntityType(Me) Then
                    Call TableSubtypeRelationship.update_parentEntityType(Me)
                Else
                    Call TableSubtypeRelationship.add_parentEntityType(Me)
                End If
            End If

        End Sub

        Public Sub setIsPrimarySubtypeRelationship(ByVal abIsPrimarySubtypeRelationship As Boolean)

            If abIsPrimarySubtypeRelationship And Me.EntityType.SubtypeRelationship.Count > 1 Then
                Dim larSubtypeRelationship = From SubtypeRelationship In Me.EntityType.SubtypeRelationship
                                             Where SubtypeRelationship IsNot Me
                                             Select SubtypeRelationship

                For Each lrSubtypeReltionship In larSubtypeRelationship.ToList
                    Call lrSubtypeReltionship.setIsPrimarySubtypeRelationship(False)
                Next
            End If

            Me.IsPrimarySubtypeRelationship = abIsPrimarySubtypeRelationship

            RaiseEvent IsPrimarySubtypeRelationshipChanged(abIsPrimarySubtypeRelationship)

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