Imports System.Runtime.InteropServices
Namespace Boston
    Public Class BostonTreeView
        Inherits TreeView

        Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
            SendMessage(Me.Handle, TVM_SETEXTENDEDSTYLE, CType(TVS_EX_DOUBLEBUFFER, IntPtr), CType(TVS_EX_DOUBLEBUFFER, IntPtr))
            MyBase.OnHandleCreated(e)
        End Sub

        Private Const TVM_SETEXTENDEDSTYLE As Integer = &H1100 + 44
        Private Const TVM_GETEXTENDEDSTYLE As Integer = &H1100 + 45
        Private Const TVS_EX_DOUBLEBUFFER As Integer = &H4
        <DllImport("user32.dll")>
        Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wp As IntPtr, ByVal lp As IntPtr) As IntPtr
        End Function


#Region "Cascade Event"

        Public Event CascadeNode(sender As Object, e As EventArgs)

        Friend Function CascadeNodeEventRaiser(NDSource As cTreeNode, NDCurrent As cTreeNode) As CascadeNodeEventArgs
            Dim EA = New CascadeNodeEventArgs(NDSource, NDCurrent)
            RaiseEvent CascadeNode(Me, EA)
            Return EA
        End Function

        Public Class CascadeNodeEventArgs
            Inherits EventArgs

            Public Sub New(NDSource As cTreeNode, NDCurrent As cTreeNode)
                _CascadeNode = NDCurrent
                _TrigerNode = NDSource
            End Sub

            'If this set to True, the whole cascading operation will be canceled.
            Private _CancelCascade As Boolean
            Public Property CancelCascade() As Boolean
                Get
                    Return _CancelCascade
                End Get
                Set(ByVal value As Boolean)
                    _CancelCascade = value
                End Set
            End Property

            Private _CascadeNode As cTreeNode
            Public ReadOnly Property CascadeNode() As cTreeNode
                Get
                    Return _CascadeNode
                End Get
            End Property

            Private _TrigerNode As cTreeNode
            Public ReadOnly Property TrigerNode() As cTreeNode
                Get
                    Return _TrigerNode
                End Get
            End Property

            Private _Handled As Boolean
            Public Property Handled() As Boolean
                Get
                    Return _Handled
                End Get
                Set(ByVal value As Boolean)
                    _Handled = value
                End Set
            End Property


        End Class

#End Region

        'Redefining native Node collection
        Public Shadows _Nodes As New cTreeNodeCollection(MyBase.Nodes, Me.ImageList)

        Public Shadows Property Nodes As cTreeNodeCollection
            Get
                Return Me._Nodes
            End Get
            Set(value As cTreeNodeCollection)
                Me._Nodes = value
            End Set
        End Property

#Region "Filtering"

        'delegate that will decide what to keep visible
        Public Delegate Function Selector(Node As cTreeNode) As Boolean

        'Was not here before cTreeNode
        Public Shadows _SelectedNode As cTreeNode = Nothing

        Public Shadows Property SelectedNode() As cTreeNode
            Get
                Return Me._SelectedNode
            End Get
            Set(ByVal value As cTreeNode)
                Me._SelectedNode = value
            End Set
        End Property


        Public Sub Filter(Filter As Selector)
            _Filter(Filter, Me.Nodes)
        End Sub


        'The recursive Filtering procedure
        Private Sub _Filter(Filter As Selector, NDC As cTreeNodeCollection)
            For Each ND In NDC._ActualNodes
                'Before anything else, if we have subNodes, go and filter them first
                If ND.Nodes._ActualNodes.Count > 0 Then _Filter(Filter, ND.Nodes)

                If Filter(ND) Then
                    ND.Hidden(False) = False
                Else
                    'if current node has at least one visible node after filtering, make the parent visible too
                    ND.Hidden(False) = ND.Nodes._VisibleNodes.Count = 0
                End If

            Next
        End Sub

        Public Sub ForceSelectedNode(ByRef arNode As cTreeNode)
            MyBase.SelectedNode = arNode
        End Sub

#End Region


    End Class
End Namespace