Public Class tShapeNodeDragItem

    Private _Index As Integer

    Public ReadOnly Property Index() As Integer
        Get
            Return _Index
        End Get
    End Property

    Public Tag As Object 'Set to the Object being dragged.

    ''' <summary>
    ''' Parameterless New
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
    End Sub


    Public Sub New(ByVal aiIndex As Integer)

        Me._index = aiIndex

    End Sub

    Public Sub New(ByVal aiIndex As Integer, ByVal aoTag As Object)

        Me._index = aiIndex
        Me.Tag = aoTag

    End Sub

    Public Sub New(ByVal arObject As Object)

        Me._index = -1
        Me.Tag = arObject

    End Sub

    Public Shared Function DraggedItemObjectType() As System.Type

        '---------------------------------------------------------------------------------------
        'Basically returns the Object Type. GetDataReference (in DragOver) requires an instance
        ' of an object (if the function isn't 'Shared') to validate the 'type' of the object being
        ' dragged. 
        '---------------------------------------------------------------------------------------
        Dim lrObject As New tShapeNodeDragItem()
        Return lrObject.GetType

    End Function

End Class
