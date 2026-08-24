Namespace FBM

    Public Class DiagramSpyPage
        Inherits FBM.Page

        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, Optional ByVal as_PageId As String = Nothing, Optional ByVal as_page_name As String = Nothing, Optional ByVal aiLanguageId As pcenumLanguage = pcenumLanguage.ORMModel)

            Me.Model = New FBM.Model
            Me.Model = arModel

            If as_PageId IsNot Nothing Then
                Me.PageId = as_PageId
            Else
                Me.PageId = System.Guid.NewGuid.ToString
            End If

            If as_page_name IsNot Nothing Then
                Me.Name = as_page_name
            Else
                Me.Name = "New Model Page"
            End If

            Me.Language = aiLanguageId

            Me.IsDirty = True

        End Sub

    End Class

End Namespace
