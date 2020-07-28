Namespace FactEngine
    Public Class QueryNode
        Implements IEquatable(Of FactEngine.QueryNode)

        ''' <summary>
        ''' The FBM ModelObject represented by this QueryNode in the QueryGraph
        ''' </summary>
        Public FBMModelObject As FBM.ModelObject

        Public Edge As New List(Of FactEngine.QueryEdge)

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arFBMModelObject"></param>
        Public Sub New(ByRef arFBMModelObject As FBM.ModelObject)
            Me.FBMModelObject = arFBMModelObject
        End Sub

        Public Function Equals(other As QueryNode) As Boolean Implements IEquatable(Of QueryNode).Equals
            Return Me.FBMModelObject.Id = other.FBMModelObject.Id
        End Function
    End Class

End Namespace
