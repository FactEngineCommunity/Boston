Imports System.Reflection

Namespace FactEngine
    Public Class QueryNode
        Inherits tQueryFormulaToken 'Because a QueryNode may be part of the formula for a QueryEdge. QueryNode is used as a FormulaToken because it contains a RelativeFBMModelObject to get the Table/NodeType of a Column/Property.
        Implements IEquatable(Of FactEngine.QueryNode)
        Implements ICloneable

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

        Public ReadOnly Property RDSTable As RDS.Table
            Get
                Return Me.RelativeFBMModelObject.getCorrespondingRDSTable
            End Get
        End Property

        Private _IdentifierList As New List(Of String)
        Public Property IdentifierList As List(Of String)
            Get
                If Me.QueryEdge IsNot Nothing Then
                    If Me.IsTargetNode Then
                        Return Me.QueryEdge.TargetNodeIdentifierList
                    Else
                        Return Me.QueryEdge.BaseNodeIdentifierList
                    End If

                Else
                    Return Me._IdentifierList
                End If
            End Get
            Set(value As List(Of String))
                If Me.QueryEdge IsNot Nothing Then
                    If Me.IsTargetNode Then
                        Me.QueryEdge.TargetNodeIdentifierList = value
                    Else
                        Me.QueryEdge.BaseNodeIdentifierList = value
                    End If
                Else
                    Me._IdentifierList = value
                End If
            End Set
        End Property



        Public ReadOnly Property DBVariableName As String
            Get
                If Me.FBMModelObject.GetType = GetType(FBM.ValueType) Then
                    Return Me.FBMModelObject.Id
                ElseIf Me.RDSTable IsNot Nothing Then
                    Return Me.RDSTable.DBVariableName
                Else
                    Return Me.FBMModelObject.DBName
                End If
            End Get
        End Property

        Private _HasIdentifier As Boolean = False
        Public Property HasIdentifier As Boolean
            Get
                If Not _HasIdentifier Then
                    Return Me.IdentifierList.Count > 0
                Else
                    Return _HasIdentifier
                End If
            End Get
            Set(value As Boolean)
                Me._HasIdentifier = value
            End Set
        End Property

        Private _BETWEENCLAUSE As FEQL.BETWEENClause = Nothing
        Public Property BETWEENCLAUSE As FEQL.BETWEENClause
            Get
                Return Me._BETWEENCLAUSE
            End Get
            Set(value As FEQL.BETWEENClause)
                Me._BETWEENCLAUSE = value
            End Set
        End Property

        ''' <summary>
        ''' E.g. WHICH DateTime.NEXT('Wednesday') has (Film:'Rocky') THAT showing at (Cinema:'Rialto') 
        ''' </summary>
        Private _NEXTCLAUSE As FEQL.NEXTClause = Nothing
        Public Property NEXTCLAUSE As FEQL.NEXTClause
            Get
                Return Me._NEXTCLAUSE
            End Get
            Set(value As FEQL.NEXTClause)
                Me._NEXTCLAUSE = value
            End Set
        End Property

        ''' <summary>
        ''' E.g. WHICH DateTime.THIS('Wednesday') has (Film:'Rocky') THAT showing at (Cinema:'Rialto') 
        ''' </summary>
        Private _THISCLAUSE As FEQL.THISClause = Nothing
        Public Property THISCLAUSE As FEQL.THISClause
            Get
                Return Me._THISCLAUSE
            End Get
            Set(value As FEQL.THISClause)
                Me._THISCLAUSE = value
            End Set
        End Property

        Public [Alias] As String = Nothing

        Public Edge As New List(Of FactEngine.QueryEdge)

        Public IsTargetNode As Boolean = False

        Public ModifierFunction As FEQL.pcenumFEQLNodeModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None

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

        '20230130-VM-Moved to new regime. QueryEdge.Formula
        'Public MathFunction As pcenumMathFunction = pcenumMathFunction.None
        'Public MathNumber As Double = 0

        Public _RelativeFBMModelObject As FBM.ModelObject = Nothing
        ''' <summary>
        ''' A QueryNode may represent a ValueType for a Column, and so QueryNode.RDSTable will return the Column for the relative Table for the RelativeFBMModelObject
        ''' </summary>
        ''' <returns></returns>
        Public Property RelativeFBMModelObject() As FBM.ModelObject
            Get
                If Me._RelativeFBMModelObject IsNot Nothing Then
                    Return Me._RelativeFBMModelObject
                ElseIf Me.QueryEdge IsNot Nothing AndAlso Me.QueryEdge.FBMFactType.getCorrespondingRDSTable(Nothing, True) IsNot Nothing And Me.FBMModelObject.GetType = GetType(FBM.ValueType) Then
                    Return Me.QueryEdge.FBMFactType
                ElseIf Me.FBMModelObject.IsAbsorbed Then
                    Return Me.FBMModelObject.GetTopmostNonAbsorbedSupertype
                Else
                    Return Me.FBMModelObject
                End If
            End Get
            Set(value As FBM.ModelObject)
                Me._RelativeFBMModelObject = value
            End Set
        End Property

        ''' <summary>
        ''' Used for TargetNodes. Used for when generating SQL for Conditional Where Clauses.
        '''   Bang is for != (not equals)
        '''   Colon is for = (equals)
        '''   Carret is for when to use the PK of the Table rather than the first Unique Identifier.
        ''' </summary>
        Public Comparitor As FEQL.pcenumFEQLComparitor = FEQL.pcenumFEQLComparitor.Colon

        Public ReadOnly Property ComparitorSymbol As String
            Get
                Select Case Me.Comparitor
                    Case Is = FEQL.pcenumFEQLComparitor.Colon
                        Return ":"
                    Case Is = FEQL.pcenumFEQLComparitor.Bang
                        Return "!"
                    Case Is = FEQL.pcenumFEQLComparitor.LikeComparitor
                        Return "~"
                    Case Is = FEQL.pcenumFEQLComparitor.Carret
                        Return "^"
                    Case Else
                        Return ":"
                End Select
            End Get
        End Property

        Public IsDistinct As Boolean = False 'As in WHICH Order was placed on WHICH DISTINCT OrderDate.YEAR AND is for (Product:'Steeleye Stout')

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
                       Optional abIsTargetNode As Boolean = False,
                       Optional aiComparitor As FEQL.pcenumFEQLComparitor = FEQL.pcenumFEQLComparitor.Colon)

            Me.FBMModelObject = arFBMModelObject
            Me.QueryEdge = arQueryEdge
            Me.IsTargetNode = abIsTargetNode
            Me.Comparitor = aiComparitor
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone

            Dim lrQueryNode As New FactEngine.QueryNode(Nothing)
            Try
                With Me
                    lrQueryNode.FBMModelObject = .FBMModelObject
                End With

                Return lrQueryNode

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Public Shadows Function Equals(other As QueryNode) As Boolean Implements IEquatable(Of QueryNode).Equals

            Return Me.FBMModelObject.Id = other.FBMModelObject.Id And NullVal(Me.Alias, "") = NullVal(other.Alias, "")

        End Function

        Public Function getTargetSQLComparator() As String

            Try

                Select Case Me.Comparitor
                    Case Is = FEQL.pcenumFEQLComparitor.Bang
                        Return " <> "
                    Case Is = FEQL.pcenumFEQLComparitor.Colon,
                              FEQL.pcenumFEQLComparitor.Carret
                        Return " = "
                    Case Is = FEQL.pcenumFEQLComparitor.LikeComparitor

                        If Me.FBMModelObject.Model.DatabaseConnection IsNot Nothing Then

                            Select Case Me.FBMModelObject.Model.TargetDatabaseType
                                Case Is = pcenumDatabaseType.Neo4j,
                                          pcenumDatabaseType.KuzuDB
                                    Return " =~ "
                                Case Else
                                    Return " LIKE "
                            End Select

                        Else
                            Return " LIKE "
                        End If

                End Select


            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            Return " = "

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

    End Class

End Namespace
