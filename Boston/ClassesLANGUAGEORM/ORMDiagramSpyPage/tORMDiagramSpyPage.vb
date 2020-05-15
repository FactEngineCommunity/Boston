Namespace FBM

    Public Class DiagramSpyPage
        Inherits FBM.Page

        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, Optional ByVal as_PageId As String = Nothing, Optional ByVal as_page_name As String = Nothing, Optional ByVal aiLanguageId As pcenumLanguage = Nothing)

            Me.Model = New FBM.Model
            Me.Model = arModel

            If IsSomething(as_PageId) Then
                Me.PageId = as_PageId
            Else
                Me.PageId = System.Guid.NewGuid.ToString
            End If

            If IsSomething(as_page_name) Then
                Me.Name = as_page_name
            Else
                Me.Name = "New Model Page"
            End If

            If IsSomething(aiLanguageId) Then
                Me.Language = aiLanguageId
            Else
                '---------------------
                'Default to ORM Model
                '---------------------
                Me.Language = pcenumLanguage.ORMModel
            End If

            Me.IsDirty = True

        End Sub

    End Class

End Namespace
