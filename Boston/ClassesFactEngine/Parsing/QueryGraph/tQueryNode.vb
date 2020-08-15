Namespace FactEngine
    Public Class QueryNode
        Implements IEquatable(Of FactEngine.QueryNode)

        Public PreboundText As String = Nothing
        Public PostboundText As String = Nothing

        ''' <summary>
        ''' The FBM ModelObject represented by this QueryNode in the QueryGraph
        ''' </summary>
        Public FBMModelObject As FBM.ModelObject

        Public ReadOnly Property Name As String
            Get
                Return Me.FBMModelObject.Id '20200815-Use DatabaseName in the future
            End Get
        End Property

        Public ReadOnly Property Id As String
            Get
                Return Me.FBMModelObject.Id
            End Get
        End Property

        Public ReadOnly Property RDSTable As RDS.Table
            Get
                Return Me.FBMModelObject.getCorrespondingRDSTable
            End Get
        End Property

        Public [Alias] As String = Nothing

        Public Edge As New List(Of FactEngine.QueryEdge)

        ''' <summary>
        ''' The QueryEdge that resulted in this Node being added to the QueryGraph.Nodes collection
        ''' </summary>
        Public QueryEdge As FactEngine.QueryEdge = Nothing

        Public ReadOnly Property QueryEdgeAlias As String
            Get
                If Me.QueryEdge Is Nothing Then
                    Return Nothing
                Else
                    Return Me.QueryEdge.Alias
                End If
            End Get
        End Property

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arFBMModelObject"></param>
        Public Sub New(ByRef arFBMModelObject As FBM.ModelObject,
                       Optional ByRef arQueryEdge As FactEngine.QueryEdge = Nothing)
            Me.FBMModelObject = arFBMModelObject
            Me.QueryEdge = arQueryEdge
        End Sub

        Public Shadows Function Equals(other As QueryNode) As Boolean Implements IEquatable(Of QueryNode).Equals

            Return Me.FBMModelObject.Id = other.FBMModelObject.Id And Me.Alias = other.Alias
        End Function
    End Class

End Namespace
