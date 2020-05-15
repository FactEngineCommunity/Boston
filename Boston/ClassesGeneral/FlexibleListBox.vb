Public Class FlexibleListBox

    Public Sub Add(c As Control)
        flpListBox.Controls.Add(c)
    End Sub

    Public Sub Remove(name As String)
        Dim c As Control = flpListBox.Controls(name)
        flpListBox.Controls.Remove(c)
        c.Dispose()
    End Sub

    Public Sub Clear()

        Do
            If flpListBox.Controls.Count = 0 Then Exit Do
            Dim c As Control = flpListBox.Controls(0)
            flpListBox.Controls.Remove(c)
            c.Dispose()
        Loop

    End Sub

    Public ReadOnly Property Count() As Integer
        Get
            Return flpListBox.Controls.Count
        End Get
    End Property

End Class
