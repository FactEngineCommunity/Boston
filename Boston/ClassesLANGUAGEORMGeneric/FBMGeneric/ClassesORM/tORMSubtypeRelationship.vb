Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class tSubtypeRelationship
        Inherits FBM.ModelObject
        Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship)
        Implements IEquatable(Of FBM.tSubtypeRelationship)

        <XmlAttribute()> _
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.SubtypeConstraint

        <XmlIgnore()> _
        Public EntityType As New FBM.EntityType 'The EntityType for which the SubtypeConstraint is applicable

        <XmlIgnore()> _
        Public parentEntityType As New FBM.EntityType 'The Parent EntityType to Me.EntityType

        ''' <summary>
        ''' The corresponding FactType that represents this SubtypeConstraint.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public FactType As New FBM.FactType

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

                If abAddToModel Then
                    If lrSubtypeRelationship.EntityType.SubtypeConstraint.Contains(lrSubtypeRelationship) Then
                        '------------------------------------------------------------------------------------------------------------
                        'The SubtypeRelationship already exists within the set of SubtypeRelationships for the EntityType (Subtype)
                        '------------------------------------------------------------------------------------------------------------
                    Else
                        lrSubtypeRelationship.EntityType.SubtypeConstraint.Add(lrSubtypeRelationship)
                    End If
                End If
            End With

            Return lrSubtypeRelationship

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

            Call Me.EntityType.RemoveSubtypeRelationship(Me)
            Call Me.FactType.RemoveFromModel(True)
            Call Me.Delete()

        End Sub

        ''' <summary>
        ''' Saves the SubtypeRelationship to the database. Override to implement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Shadows Sub Save() Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Save

        End Sub

        ''' <summary>
        ''' Creates an instance of the SubtypeRelationship in the database. Override to implement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Create() Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Create

        End Sub

        ''' <summary>
        ''' Deletes the SubtypeRelationship from the database. Override to implement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Delete() Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Delete

        End Sub

        Public Function Load() As FBM.tSubtypeRelationship Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Load

            Return Me
        End Function

    End Class
End Namespace