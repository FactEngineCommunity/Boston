Public Class frmToolboxCypher

    Public zrModel As New FBM.Model
    Public zrPage As New FBM.Page

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frmToolboxCypher_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Public Sub SetupForm()

    End Sub

    Public Sub DevelopCypherText()


        If Me.zrPage.SelectedObject.Count > 0 Then

            Select Case Me.zrPage.SelectedObject(0).ConceptType
                Case Is = pcenumConceptType.Entity


                    Me.RichTextBox1.Clear()

                    '===================================
                    'DDL (Database Definition Language)
                    '===================================
                    Me.RichTextBox1.AppendText("Database Definition Language (DDL)")
                    Me.RichTextBox1.AppendText(vbCrLf & vbCrLf)
                    'CREATE (matrix1:Movie { title : 'The Matrix', year : '1999-03-31' })

                    Dim lrEntity As ERD.Entity
                    Dim lrAttribute As ERD.Attribute
                    lrEntity = Me.zrPage.SelectedObject(0)

                    Dim lsCREATEStatement As String = ""

                    lsCREATEStatement &= "CREATE "
                    lsCREATEStatement &= "("
                    lsCREATEStatement &= LCase(lrEntity.Name) & "1" & ":"
                    lsCREATEStatement &= Viev.Strings.MakeCapCamelCase(lrEntity.Name)
                    lsCREATEStatement &= ")"
                    Me.RichTextBox1.AppendText(lsCREATEStatement)
                    Me.RichTextBox1.AppendText(vbCrLf & vbCrLf)
                    Me.RichTextBox1.AppendText(vbCrLf & vbCrLf)

                    lsCREATEStatement &= "CREATE "
                    lsCREATEStatement &= "("
                    lsCREATEStatement &= LCase(lrEntity.Name) & "1" & ":"
                    lsCREATEStatement &= Viev.Strings.MakeCapCamelCase(lrEntity.Name)
                    lsCREATEStatement &= "{"
                    For Each lrAttribute In lrEntity.Attribute
                        lsCREATEStatement &= LCase(lrAttribute.Name) & ":''"
                    Next
                    lsCREATEStatement &= "}"
                    lsCREATEStatement &= ")"
                    Me.RichTextBox1.AppendText(lsCREATEStatement)
                    Me.RichTextBox1.AppendText(vbCrLf & vbCrLf)
                    Me.RichTextBox1.AppendText(vbCrLf & vbCrLf)




                Case Else
                    Me.RichTextBox1.Clear()
            End Select


        End If



    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged

    End Sub
End Class