Public Class frmToolboxDescriptions

    Public mrModelElement As FBM.ModelObject

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub setDescriptions(ByRef arModelElement As FBM.ModelObject)

        Me.mrModelElement = arModelElement

        If arModelElement Is Nothing Then
            Me.LabelModelElementName.Text = "<No Model Element Selected>"

            Me.TextBoxShortDescription.Text = ""
            Me.TextBoxLongDescription.Text = ""
        Else
            Me.LabelModelElementName.Text = arModelElement.Id

            Me.TextBoxShortDescription.Text = arModelElement.ShortDescription
            Me.TextBoxLongDescription.Text = arModelElement.LongDescription
        End If

    End Sub

    Private Sub frmToolboxDescriptions_Leave(sender As Object, e As EventArgs) Handles Me.Leave

        If Me.mrModelElement IsNot Nothing Then

            Call Me.mrModelElement.SetShortDescription(Trim(Me.TextBoxShortDescription.Text))
            Call Me.mrModelElement.SetLongDescription(Trim(Me.TextBoxLongDescription.Text))

        End If

    End Sub

    Private Sub frmToolboxDescriptions_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

End Class