Public Class cTreeNode
    Inherits TreeNode

    Friend _MyTreeNodeCollection As cTreeNodeCollection

    Public Shadows Nodes As New cTreeNodeCollection(MyBase.Nodes)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(Text As String)
        MyBase.New(Text)
    End Sub

    Public Sub New(Text As String, Children() As cTreeNode)
        MyBase.New(Text)
        For Each N In Children
            Me.Nodes.Add(N)
        Next
    End Sub

    Public Sub New(ByVal asText1 As String,
                   ByVal asText2 As String,
                   ByVal aiInt1 As Integer,
                   ByVal aiInt2 As Integer)
        MyBase.New(asText2, aiInt1, aiInt2)
        Me.Name = asText1
    End Sub

    Public Shadows ReadOnly Property TreeView() As Boston.BostonTreeView
        Get
            Return MyBase.TreeView
        End Get
    End Property


    Private _Hidden As Boolean
    Public Property Hidden(Optional CascadeUp As Boolean = True, Optional CascadeDown As Boolean = False) As Boolean
        Get
            Return _Hidden
        End Get
        Set(ByVal value As Boolean)

            If CascadeUp Then
                Dim P As cTreeNode = MyBase.Parent


                Do While P IsNot Nothing
                    Dim Res = TreeView.CascadeNodeEventRaiser(Me, P)
                    If Res.CancelCascade Then Exit Do
                    If Res.Handled = False Then
                        'we set CascadeUp to false and cycle through all parents manually to be able to pass in CascadeNodeEventRaiser the real originating node of those callings
                        P.Hidden(False) = value
                        P = P.Parent
                    End If
                Loop
                'End If
            End If

            If CascadeDown Then CascadeCollection(Me.Nodes, value)

            'Do nothing if value didn't really changed, to increase performance.
            If _Hidden <> value Then
                _Hidden = value

                If Me.InCollection Then

                    If _Hidden = True Then
                        _MyTreeNodeCollection._VisibleNodes.Remove(Me)
                    Else
                        'if we making the node visible, put it after closer visible node
                        If Me.PreviousUnHidenNode Is Nothing Then
                            _MyTreeNodeCollection._VisibleNodes.Insert(0, Me)
                        Else
                            _MyTreeNodeCollection._VisibleNodes.Insert(Me.PreviousUnHidenNode.VisibilityIndex + 1, Me)
                        End If
                    End If
                End If

            End If

        End Set
    End Property

    'This is a recruceve procedure that will cascade down all nodes
    Private Sub CascadeCollection(NDC As cTreeNodeCollection, IsHidden As Boolean)
        For Each N As cTreeNode In NDC
            Dim Res = TreeView.CascadeNodeEventRaiser(Me, N)
            If Res.CancelCascade Then Exit For
            If Res.Handled = False Then
                N.Hidden(False, False) = IsHidden
                If N.Nodes.Count > 0 Then CascadeCollection(N.Nodes, IsHidden)
            End If
        Next
    End Sub


    'This will return the closest previous unHidden node
    Friend ReadOnly Property PreviousUnHidenNode() As cTreeNode
        Get
            If Me.Index = 0 Then
                Return Nothing
            Else
                If Me.InCollection = False Then
                    Throw New Exception("This node is not assigned to a TreeNodeCollection yet.")
                Else
                    For i = Me.Index - 1 To 0 Step -1
                        If _MyTreeNodeCollection(i).Hidden = False Then Return _MyTreeNodeCollection(i)
                    Next
                End If
            End If
        End Get
    End Property

    'This will return node index as it visible on TreeView
    Public ReadOnly Property VisibilityIndex As Integer
        Get
            Return MyBase.Index
        End Get
    End Property


    'This will return node index as it is in the Actual node list
    Public Shadows ReadOnly Property Index As Integer
        Get
            If Me.InCollection Then
                Return _MyTreeNodeCollection._ActualNodes.IndexOf(Me)
            Else
                Throw New Exception("This node is not assigned to a TreeNodeCollection yet.")
            End If
        End Get
    End Property


    'just a help property to check if tree node assigned to some tree collection
    Friend ReadOnly Property InCollection As Boolean
        Get
            Return _MyTreeNodeCollection IsNot Nothing
        End Get
    End Property

End Class