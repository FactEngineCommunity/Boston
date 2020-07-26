Namespace FactEngine
    Public Class QueryNode

        ''' <summary>
        ''' The FBM ModelObject represented by this QueryNode in the QueryGraph
        ''' </summary>
        Public FBMModelObject As FBM.ModelObject

        Public Edge As New List(Of FactEngine.QueryEdge)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arFBMModelObject"></param>
        Public Sub New(ByRef arFBMModelObject As FBM.ModelObject)

            Me.FBMModelObject = arFBMModelObject

        End Sub


    End Class

End Namespace
