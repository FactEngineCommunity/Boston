Namespace publicPermutations
    Public Module publicPermutations

        ''' <summary>
        '''  
        ''' </summary>    
        ''' <param name="aaoElements">Elements() is the array to permutate (remember, this will grow shorter as we work, so the ArrayCount parameter cannot be deduced from the length of Elements).</param>
        ''' <param name="aaoOrder">Order is the temporary array where we store one permutation</param>
        ''' <param name="aaoOrders">Orders is the Collection where we store all permutations found.</param>
        ''' <remarks></remarks>
        Public Sub Permutate(ByVal aiDepth As Integer, _
                             ByRef aaoElements As List(Of Object), _
                             ByRef aaoOrder As List(Of Object), _
                             ByRef aaoOrders As List(Of List(Of Object)))

            Dim loElement As Object
            Dim liInd As Integer

            ' Position in the Order array of the first element in
            ' the permutated arrays.
            '
            ' Example: Given the array(a,b,c,d), where we want to permutate
            ' (b,c,d), the position in the new array for the first element
            ' will be 2 (since (a) will take up the first position).
            ' Likewise, when we permutate (c,d), the position of the first
            ' element will be 3, since the first two spots are taken by
            ' (a,b).

            If aaoElements.Count = 1 Then
                ' The most primitive array we will permutate.
                ' The result is the array itself, and the result
                ' is inserted in the last position of the Order array.
                aaoOrder.Add(aaoElements(0))

                ' This Order is now complete, since the final element has
                ' been filled in.
                Dim List2 As New List(Of Object)(aaoOrder)
                aaoOrders.Add(List2)

                aaoOrder.RemoveRange(aaoOrder.Count - 2, 2)
            Else
                ' The permutation of Elements is each distinct Element
                ' + all permutations of the remaining elements.
                For liInd = 1 To aaoElements.Count
                    loElement = aaoElements(liInd - 1)
                    aaoOrder.Add(loElement)
                    Call Permutate(aiDepth + 1, RemoveFromArray(aaoElements, loElement), aaoOrder, aaoOrders)
                Next liInd
                If aaoOrder.Count > 0 Then
                    aaoOrder.RemoveRange(aiDepth - 2, (aaoOrder.Count - (aiDepth - 2)))
                End If
            End If

        End Sub

        Public Function RemoveFromArray(ByRef Elements As List(Of Object), ByVal Element As Object) As List(Of Object)

            Dim LiInd As Integer

            'Will create a new array where Element has been left out.

            Dim laoNewList As New List(Of Object)

            For LiInd = 0 To (Elements.Count - 1)
                If Elements(LiInd) <> Element Then
                    laoNewList.Add(Elements(LiInd))
                End If
            Next

            Return laoNewList

        End Function

    End Module
End Namespace