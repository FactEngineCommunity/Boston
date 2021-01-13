Public Class cTreeNodeCollection
    'Make class usable with For...Each
    Implements IEnumerable

    Friend _VisibleNodes As TreeNodeCollection
    Friend _ActualNodes As New List(Of cTreeNode)

    Friend Sub New(TreeNodeCollection As TreeNodeCollection)
        _VisibleNodes = TreeNodeCollection
    End Sub

    Public ReadOnly Property Item(Index As Integer) As cTreeNode
        Get
            Return _ActualNodes(Index)
        End Get
    End Property

    'This is to mimic original TreeNodeCollection behavior
    Default Public ReadOnly Property Node(asItemValue As String) As cTreeNode
        Get
            If asItemValue.IsNumeric Then
                Return Me._ActualNodes(CInt(asItemValue))
            Else
                Return Me.Find(asItemValue, False)
            End If
        End Get
    End Property

    Public Sub Add(Node As cTreeNode)
        Node._MyTreeNodeCollection = Me
        _ActualNodes.Add(Node)
        'if the node is Hidden, there is no need to add it to TreeNodeCollection
        If Node.Hidden = False Then _VisibleNodes.Add(Node)
    End Sub

    Public Function Add(ByVal asText1 As String,
                   ByVal asText2 As String,
                   ByVal aiInt1 As Integer,
                   ByVal aiInt2 As Integer)

        Dim lrNode = New cTreeNode(asText1, asText2, aiInt1, aiInt2)

        lrNode._MyTreeNodeCollection = Me

        _ActualNodes.Add(lrNode)

        'if the node is Hidden, there is no need to add it to TreeNodeCollection
        If lrNode.Hidden = False Then _VisibleNodes.Add(lrNode)

        Return lrNode

    End Function

    Public Sub AddRange(Nodes() As cTreeNode)
        For Each N In Nodes
            Add(N)
        Next
    End Sub

    Public Sub Clear()
        Me._ActualNodes.Clear()
    End Sub

    Public Function Find(ByVal asText As String, ByVal abSearchChildren As Boolean)

        For Each lrNode In Me._ActualNodes
            If lrNode.Text = asText Then Return lrNode
            If abSearchChildren Then
                For Each lrChildNode In lrNode.Nodes
                    Return Me.Find(asText, abSearchChildren)
                Next
            End If
        Next

        Return Nothing

    End Function

    Public Sub Remove(Node As cTreeNode)
        Node._MyTreeNodeCollection = Nothing
        _ActualNodes.Remove(Node)
        _VisibleNodes.Remove(Node)
    End Sub

    Public ReadOnly Property Count As Integer
        Get
            Return _ActualNodes.Count
        End Get
    End Property

    Public ReadOnly Property VisibleCount As Integer
        Get
            Return _VisibleNodes.Count
        End Get
    End Property

    Public Sub Insert(Node As cTreeNode, Index As Integer)
        _ActualNodes.Insert(Index, Node)
        If Node.Hidden = False Then
            'If there is no unHidden nodes at same level, add it just at the beginning
            If Node.PreviousUnHidenNode Is Nothing Then
                _VisibleNodes.Insert(0, Node)
            Else
                _VisibleNodes.Insert(Node.PreviousUnHidenNode.VisibilityIndex, Node)
            End If
        End If
    End Sub

#Region "Implementing IEnumerable"

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return New cMyEnumerator(_ActualNodes)
    End Function

    Private Class cMyEnumerator
        Implements IEnumerator

        Public Nodes As List(Of cTreeNode)
        Private position As Integer = -1

        'constructor
        Public Sub New(ByVal ActualNodes As List(Of cTreeNode))
            Nodes = ActualNodes
        End Sub

        Private Function getEnumerator() As IEnumerator
            Return DirectCast(Me, IEnumerator)
        End Function

        'IEnumerator
        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            position += 1
            Return (position < Nodes.Count)
        End Function

        'IEnumerator
        Public Sub Reset() Implements IEnumerator.Reset
            position = -1
        End Sub

        'IEnumerator
        Public ReadOnly Property Current() As Object Implements IEnumerator.Current
            Get
                Try
                    Return Nodes(position)

                Catch e1 As IndexOutOfRangeException
                    Throw New InvalidOperationException()
                End Try
            End Get
        End Property

    End Class 'end nested class

#End Region

End Class