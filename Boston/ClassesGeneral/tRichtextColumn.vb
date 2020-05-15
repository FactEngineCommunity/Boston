Public Class tRichtextColumn
    Inherits DataGridViewColumn

    Public Sub New()
        MyBase.New(New class_richtext_cell())
    End Sub

    Public Overrides Property CellTemplate() As DataGridViewCell

        Get
            Return MyBase.CellTemplate
        End Get

        Set(ByVal value As DataGridViewCell)

            ' Ensure that the cell used for the template is a RichTextCell.
            If (value IsNot Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(class_richtext_cell)) Then
                Throw New InvalidCastException("Must be a class_richtext_cell")
            End If
            MyBase.CellTemplate = value

        End Set

    End Property

    Private Sub tRichtextColumn_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

        Call Me.CellTemplate.Dispose()

    End Sub

End Class
