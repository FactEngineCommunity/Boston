Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports Newtonsoft.Json

Namespace FBM

    <Serializable()>
    Public Class PageObject
        Inherits FBM.ModelObject
        Implements iPageObject

        <XmlIgnore()>
        Public Page As FBM.Page

        <JsonIgnore()>
        <NonSerialized(),
        XmlIgnore()>
        Public _Shape As ShapeNode

        <JsonIgnore()>
        <XmlIgnore()>
        Public Property Shape As ShapeNode Implements iPageObject.Shape
            Get
                Return Me._Shape
            End Get
            Set(value As ShapeNode)
                Me._Shape = value
            End Set
        End Property

        <NonSerialized>
        <XmlIgnore()>
        Public TableShape As New MindFusion.Diagramming.TableNode

        Public X As Integer
        Public Y As Integer

        Private Property iPageObject_X As Integer Implements iPageObject.X
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Private Property iPageObject_Y As Integer Implements iPageObject.Y
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property InstanceNumber As Integer Implements iPageObject.InstanceNumber
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

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
                            Call Me.EnableSaveButton()
                            Me.Page.Diagram.Invalidate()
                        End If
                    End If
                End If
            Catch lo_err As Exception
                MsgBox("t_Entity.update_from_model: " & lo_err.Message)
            End Try
        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected
            Throw New NotImplementedException()
        End Sub

        Public Sub Move(aiNewX As Integer, aiNewY As Integer, abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move
            Throw New NotImplementedException()
        End Sub

        Public Sub Moved() Implements iPageObject.Moved
            Throw New NotImplementedException()
        End Sub

        Public Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects
            Throw New NotImplementedException()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour
            Throw New NotImplementedException()
        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace