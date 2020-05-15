Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class SubtypeRelationship
        Inherits Viev.FBM.tSubtypeRelationship


        ''' <summary>
        ''' Creates an instance of the SubtypeRelationship in the database. Override to implement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub Create() Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Create

            Call TableSubtypeRelationship.add_parentEntityType(Me)
        End Sub


        ''' <summary>
        ''' Deletes the SubtypeRelationship from the database. Override to implement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub Delete() Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Delete
            Call TableSubtypeRelationship.DeleteParentEntityType(Me)
        End Sub


        ''' <summary>
        ''' Saves the SubtypeRelationship to the database. Override to implement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Shadows Sub Save() Implements iObjectRelationalMap(Of FBM.tSubtypeRelationship).Save

            If TableSubtypeRelationship.exists_parentEntityType(Me) Then
                Call TableSubtypeRelationship.update_parentEntityType(Me)
            Else
                Call TableSubtypeRelationship.add_parentEntityType(Me)
            End If

        End Sub

    End Class

End Namespace
