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
        ''' The Actor at the CMML level of the Model. I.e. As in FBM.Model.UML; not the Page level, but the Model level.
        ''' </summary>
        Public WithEvents CMMLActor As CMML.Actor

        ''' <summary>
        ''' The SequenceNr assigned to the Actor in (say) an EventTraceDiagram.
        ''' </summary>
        ''' <remarks></remarks>
        Public SequenceNr As Single = 1

        Public process As List(Of CMML.Process)

        <NonSerialized(),
        XmlIgnore()>
        Public NameShape As New UML.ActorName(Me)

        Public Sub New()
            '-----------------------------------
            'Default Parameterless constructor
            '-----------------------------------
        End Sub

        Public Sub New(ByRef arPage As FBM.Page, Optional ByVal asName As String = Nothing)

            Me.Model = arPage.Model
            Me.Page = arPage
            Me.FactData.Model = arPage.Model
            Me.Data = "New Actor"
            Me.Id = Me.Data
            Me.Name = Me.Data
            Me.Symbol = Me.Data
            Me.Concept = New FBM.Concept(Me.Data)

            If asName IsNot Nothing Then
                Me.Name = asName
            End If

        End Sub

        Public Overrides Function CloneUCDActor(ByRef arPage As FBM.Page) As UCD.Actor

            Dim lr_actor As New UCD.Actor

            Try
                With Me
                    lr_actor.Model = .Model
                    lr_actor.Page = arPage
                    lr_actor.ConceptType = pcenumConceptType.Actor 'While this is redundant, it seems that it is required for Polymorphic use under tEntity
                    lr_actor.FactData = .FactData
                    lr_actor.Name = .Concept.Symbol
                    lr_actor.FactDataInstance = Me.FactDataInstance
                    lr_actor.JoinedObjectType = .Role.JoinedORMObject
                    lr_actor.Concept = .Concept
                    lr_actor.Role = .Role
                    lr_actor.X = .X
                    lr_actor.Y = .Y
                    lr_actor.Shape = .Shape
                    lr_actor.TableShape = .TableShape
                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lr_actor

        End Function

        Public Shadows Function Equals(ByVal other As UML.Actor) As Boolean Implements System.IEquatable(Of UML.Actor).Equals

            If Me.Name = other.Name Then
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
            Dim lrActorName As New UML.ActorName
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
                    If IsSomething(Me.NameShape.Shape) Then
                        If Me.NameShape.Shape.Text <> "" Then
                            Me.NameShape.Shape.Text = Trim(Me.FactData.Data)

                            '----------------------------------------
                            'Setup the ActorName shape size
                            '----------------------------------------                    
                            Dim StringSize As New SizeF
                            Dim G As Graphics

                            G = Me.Page.Form.CreateGraphics
                            StringSize = Me.Page.Diagram.MeasureString(Trim(Me.FactData.Data), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                            StringSize.Height += 2

                            Me.NameShape.Shape.SetRect(New RectangleF(Me.NameShape.Shape.Bounds.X, Me.NameShape.Shape.Bounds.Y, StringSize.Width, StringSize.Height), True)

                            Me.Page.Diagram.Invalidate()

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

        Public Overrides Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements FBM.iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            Try

                Me.FactDataInstance.X = aiNewX
                Me.FactDataInstance.Y = aiNewY

                Me.FactDataInstance.Fact.FactType.isDirty = True
                Me.FactDataInstance.Fact.isDirty = True
                Me.FactDataInstance.isDirty = True
                Me.isDirty = True
                Me.Model.MakeDirty(False, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Try
                Me.FactDataInstance.Page.MakeDirty()
            Catch ex As Exception
            End Try

        End Sub

        Public Overrides Function setName(asNewName As String, Optional abBroadcastInterfaceEvent As Boolean = True, Optional abSuppressModelSave As Boolean = False) As Boolean

            Try
                Call Me.CMMLActor.FBMModelElement.setName(asNewName, abBroadcastInterfaceEvent, abSuppressModelSave)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

    End Class

End Namespace