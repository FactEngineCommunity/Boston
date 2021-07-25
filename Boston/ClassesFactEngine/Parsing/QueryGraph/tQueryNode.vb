Imports System.Reflection

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
                Return Me.FBMModelObject.Id  '20200815-Use DatabaseName in the future
            End Get
        End Property

        Public ReadOnly Property Id As String
            Get
                Return Me.FBMModelObject.Id
            End Get
        End Property

        Public IdentifierList As New List(Of String)

        Public ReadOnly Property RDSTable As RDS.Table
            Get
                Return Me.RelativeFBMModelObject.getCorrespondingRDSTable
            End Get
        End Property

        Public HasIdentifier As Boolean = False

        Public [Alias] As String = Nothing

        Public Edge As New List(Of FactEngine.QueryEdge)

        Public IsTargetNode As Boolean = False

        ''' <summary>
        ''' True if is Target node and is on the other side of a Shortest Path query.
        '''   E.g. (Account:1) made [SHORTEST PATH 0..10] WHICH Transaction THAT was made to (Account 2:4) 
        '''   The above would otherwise have Account 2 as a conditional QueryNode/TargetNode, but is taken care of inside the Shortest Path FROM clause processing.
        ''' </summary>
        Public IsExcludedConditional As Boolean = False

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

        Public MathFunction As pcenumMathFunction = pcenumMathFunction.None
        Public MathNumber As Double = 0

        Public ReadOnly Property RelativeFBMModelObject() As FBM.ModelObject
            Get
                If Me.FBMModelObject.IsAbsorbed Then
                    Return Me.FBMModelObject.GetTopmostNonAbsorbedSupertype
                Else
                    Return Me.FBMModelObject
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
                       Optional ByRef arQueryEdge As FactEngine.QueryEdge = Nothing,
                       Optional abIsTargetNode As Boolean = False)
            Me.FBMModelObject = arFBMModelObject
            Me.QueryEdge = arQueryEdge
            Me.IsTargetNode = abIsTargetNode
        End Sub

        Public Shadows Function Equals(other As QueryNode) As Boolean Implements IEquatable(Of QueryNode).Equals

            Return Me.FBMModelObject.Id = other.FBMModelObject.Id And NullVal(Me.Alias, "") = NullVal(other.Alias, "")

        End Function

        Public Function IsPGSRelationByDefacto() As Boolean

            Try
                Dim lrTable As RDS.Table = Me.FBMModelObject.getCorrespondingRDSTable

                If lrTable.isPGSRelation Then
                    Return True
                Else
                    If Me.QueryEdge.BaseNode Is Nothing Then
                        Return False
                    ElseIf Me.IsTargetNode = False Then
                        Return False
                    Else
                        If lrTable.Column.FindAll(Function(x) x.ActiveRole.JoinedORMObject.Id = Me.QueryEdge.BaseNode.Name).Count = 2 Then
                            Return True
                        End If
                    End If
                End If

                Return False

            Catch ex As Exception
                Return ex.Message
            End Try
        End Function

        ''' <summary>
        ''' Used in SubQueries.
        ''' TRUE if the Node is a TargetNode and the QueryEdge for the node has a reading like "...THAT ModelElement" and the ModelElement hasn't previously been referenced.
        ''' E.g. Session in
        ''' WHICH Cinema is showing (Film:'Rocky') at (DateTime:'1/5/2021 10:00') 
        ''' AND contains WHICH Row THAT contains A Seat THAT has NO Booking THAT Is for THAT Session 
        ''' </summary>
        ''' <returns></returns>
        Public Function IsThatReferencedTargetNode() As Boolean

            Try
                If Not Me.IsTargetNode Then Return False

                Select Case Me.QueryEdge.WhichClauseType
                    Case Is = FactEngine.pcenumWhichClauseType.AndThatPredicateThatModelElement,
                              FactEngine.pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement
                        Return True
                End Select

                Return False

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

    End Class

End Namespace
