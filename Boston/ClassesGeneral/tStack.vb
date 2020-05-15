<Serializable()> Public Class tStack

    Public Stack As List(Of Object)
    Public TopOfStack As Integer

    Sub clearStack()
        Me.Stack.Clear()
        TopOfStack = 0
    End Sub

    Function IsItemInStack(ByVal aoItem As Object) As Integer

        IsItemInStack = False

        If Me.Stack.Contains(aoItem) Then
            IsItemInStack = True
        Else
            IsItemInStack = False
        End If

    End Function

    Function IsStackEmpty() As Integer

        If TopOfStack = 0 Then
            IsStackEmpty = True
        Else
            IsStackEmpty = False
        End If

    End Function

    Function Pop() As Object

        Pop = Stack(Me.TopOfStack)

        Me.Stack.RemoveAt(Me.TopOfStack - 1)
        Me.TopOfStack -= 1

    End Function


    Sub Push(ByVal item As Object)

        TopOfStack = TopOfStack + 1
        Stack.Add(item)

    End Sub





End Class
