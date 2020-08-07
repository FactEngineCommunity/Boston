Imports MindFusion.Diagramming
Imports System.Xml.Serialization

Namespace FBM

    <Serializable()> _
    Public Class PageObject
        Inherits FBM.ModelObject

        <NonSerialized>
        <XmlIgnore()>
        Public Page As FBM.Page

        <NonSerialized>
        <XmlIgnore()>
        Public Shape As New ShapeNode

        Public X As Integer
        Public Y As Integer

        Public Sub New()

        End Sub

        Public Sub New(ByVal as_Symbol As String, Optional ByVal aiConceptType As pcenumConceptType = Nothing)


            Me.Symbol = Trim(as_Symbol)

            If IsSomething(aiConceptType) Then
                Me.ConceptType = aiConceptType
            End If

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page) As Object

            Dim lrPageObject As New FBM.PageObject

            With Me

                lrPageObject.ConceptType = .ConceptType
                lrPageObject.Symbol = .Symbol
                lrPageObject.X = .X
                lrPageObject.Y = .Y

            End With

            Return lrPageObject

        End Function

        ''' <summary>
        ''' Prototype method, RefreshShape.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RefreshShape()
            '-------------------------------------------------
            'Is empty Prototype Method for Class tPageObject
            '-------------------------------------------------
        End Sub

        Friend Sub update_GUI_from_model()

            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try
                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.Shape) Then
                        If Me.Shape.Text <> "" Then
                            '---------------------------------------------------------------------------------
                            'Is the type of EntityTypeInstance that 
                            '  shows the EntityTypeName within the
                            '  ShapeNode itself and not a separate
                            '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
                            ' 1 for the stickfigure, the other for the name of the Actor.
                            '---------------------------------------------------------------------------------
                            Me.Shape.Text = Trim(Me.Name)
                            Call Me.Page.Form.EnableSaveButton()
                            Me.Page.Diagram.Invalidate()
                        End If
                    End If
                End If
            Catch lo_err As Exception
                MsgBox("t_Entity.update_from_model: " & lo_err.Message)
            End Try
        End Sub

    End Class

End Namespace