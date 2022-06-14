Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace UML
    <Serializable()>
    Public Class Actor
        Inherits FBM.FactDataInstance
        Implements IEquatable(Of UML.Actor)
        Implements FBM.iPageObject

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Actor

        <CategoryAttribute("Actor"),
             DefaultValueAttribute(GetType(String), ""),
             DescriptionAttribute("Name of the Actor.")>
        Public Shadows _name As String = ""
        Public Overrides Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal Value As String)
                Me._name = Value
            End Set
        End Property

        ''' <summary>
        ''' The SequenceNr assigned to the Actor in (say) an EventTraceDiagram.
        ''' </summary>
        ''' <remarks></remarks>
        Public SequenceNr As Single = 1

        Public process As List(Of CMML.Process)

        <NonSerialized(),
        XmlIgnore()>
        Public NameShape As ShapeNode

        Public Sub New()
            '-----------------------------------
            'Default Parameterless constructor
            '-----------------------------------
        End Sub

        Public Sub New(ByRef arPage As FBM.Page)

            Me.Model = arPage.Model
            Me.Page = arPage
            Me.FactData.Model = arPage.Model
            Me.Data = "New Actor"
            Me.Id = Me.Data
            Me.Name = Me.Data
            Me.Symbol = Me.Data
            Me.Concept = New FBM.Concept(Me.Data)

        End Sub

        Public Shadows Function Equals(ByVal other As UML.Actor) As Boolean Implements System.IEquatable(Of UML.Actor).Equals

            If (Me.Role.Id = other.Role.Id) And (Me.Concept.Symbol = other.Concept.Symbol) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByName(ByVal other As UML.Actor) As Boolean

            If other.Name Like (Me.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByNameLike(ByVal other As FBM.EntityType) As Boolean

            If other.Name Like (Me.Name & "*") Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shared Function CompareSequenceNrs(ByVal aoA As CMML.Actor, ByVal aoB As CMML.Actor) As Integer

            '------------------------------------------------------
            'Used as a delegate within 'SortRoleGroup'
            '------------------------------------------------------
            Dim loa As New Object
            Dim lob As New Object

            Try
                Return aoA.SequenceNr - aoB.SequenceNr

            Catch ex As Exception
                prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Overridable Sub DisplayAndAssociate()

            Dim loDroppedNode As ShapeNode

            loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
            loDroppedNode.Shape = Shapes.RoundRect
            loDroppedNode.HandlesStyle = HandlesStyle.Invisible
            loDroppedNode.ToolTip = "Actor"
            loDroppedNode.Visible = True
            loDroppedNode.Image = My.Resources.CMML.actor
            loDroppedNode.Pen.Color = Color.White
            loDroppedNode.ShadowColor = Color.White

            loDroppedNode.Tag = New CMML.Actor
            loDroppedNode.Resize(10, 15)

            loDroppedNode.Tag = Me

            Me.Shape = loDroppedNode

            '-----------------------------------------
            'Establish the Name caption for the Actor
            '-----------------------------------------
            Dim StringSize As New SizeF
            Dim loActorNameShape As New ShapeNode

            StringSize = Me.Page.Diagram.MeasureString("[" & Trim(Me.Name) & "]", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            Dim lr_rectanglef As New RectangleF(loDroppedNode.Bounds.X, loDroppedNode.Bounds.Bottom, StringSize.Width, StringSize.Height)
            loActorNameShape = Me.Page.Diagram.Factory.CreateShapeNode(lr_rectanglef, MindFusion.Diagramming.Shapes.Rectangle)
            loActorNameShape.HandlesStyle = HandlesStyle.Invisible
            loActorNameShape.TextColor = Color.Black
            loActorNameShape.Transparent = True
            loActorNameShape.Visible = True
            loActorNameShape.Text = Me.Name
            loActorNameShape.ZTop()
            Dim lrActorName As New UCD.ActorName
            loActorNameShape.Tag = lrActorName
            lrActorName.Shape = loActorNameShape

            '-----------------------------------------------------------
            'Attach the Actor.Name ShapeNode to the Actor Shape
            '-----------------------------------------------------------
            loActorNameShape.AttachTo(loDroppedNode, AttachToNode.BottomCenter)


        End Sub

        Public Shadows Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)

            Try
                '--------------------------------------------------------
                'Update the Model
                '  NB GUI is updated via role_data.updated event below.
                '--------------------------------------------------------
                'Me.FactData.Data = Me.Name

            Catch lo_err As Exception
                MsgBox("class_UML_actor.RefreshShape: " & lo_err.Message & ". Symbol: " & Me.Symbol & ", PageId:" & Me.Page.PageId)
            End Try

        End Sub

        Private Sub UpdateFromModel() Handles FactData.ConceptSymbolUpdated
            Try
                Me.Data = MyBase.FactData.Data
                Call Me.UpdateGUIFromModel()
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Friend Shadows Sub UpdateGUIFromModel()

            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try

                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.NameShape) Then
                        If Me.NameShape.Text <> "" Then
                            Me.NameShape.Text = Trim(Me.FactData.Data)

                            '----------------------------------------
                            'Setup the ActorName shape size
                            '----------------------------------------                    
                            Dim StringSize As New SizeF
                            Dim G As Graphics

                            G = Me.Page.Form.CreateGraphics
                            StringSize = Me.Page.Diagram.MeasureString(Trim(Me.FactData.Data), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                            StringSize.Height += 2

                            Me.NameShape.SetRect(New RectangleF(Me.NameShape.Bounds.X, Me.NameShape.Bounds.Y, StringSize.Width, StringSize.Height), True)

                            Me.Page.Diagram.Invalidate()

                            Call Me.EnableSaveButton()
                        End If
                    End If
                End If
            Catch lo_err As Exception
                MsgBox("tActor.UpdateGUIFromModel: " & lo_err.Message & "FactTypeId: " & Me.Role.FactType.Id & ", ValueSymbol:" & Me.Concept.Symbol & ", PageId:" & Me.Page.PageId)
            End Try

        End Sub

        Public Overloads Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            Call Me.SetAppropriateColour()
        End Sub

        Public Overloads Sub SetAppropriateColour() Implements FBM.iPageObject.SetAppropriateColour

            Try
                If IsSomething(Me.Shape) Then
                    If Me.Shape.Selected Then
                        Me.Shape.Pen.Color = Color.Blue
                    Else
                        Me.Shape.Pen.Color = Color.White
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace