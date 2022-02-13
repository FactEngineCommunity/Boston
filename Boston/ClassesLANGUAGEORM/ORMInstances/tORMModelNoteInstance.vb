Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class ModelNoteInstance
        Inherits FBM.ModelNote
        Implements FBM.iPageObject

        Public WithEvents ModelNote As FBM.ModelNote

        'Public Shadows JoinedObjectType As FBM.PageObject

        Public Page As FBM.Page

        <NonSerialized(),
        XmlIgnore()>
        Public _Shape As ShapeNode
        <XmlIgnore()>
        Public Property Shape As ShapeNode Implements iPageObject.Shape
            Get
                Return Me._Shape
            End Get
            Set(value As ShapeNode)
                Me._Shape = value
            End Set
        End Property


        Public Property X As Integer Implements FBM.iPageObject.X 'The X coordinate of the PageObject
        Public Property Y As Integer Implements FBM.iPageObject.Y 'The Y coordinate of the PageObject

        <NonSerialized()> _
        <XmlIgnore()> _
        Public Link As DiagramLink

        ''' <summary>
        ''' Parameterless New for cloning
        ''' </summary>
        Public Sub New()
        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page) As Object

            Dim lrModelNoteInstance As New FBM.ModelNoteInstance

            Try
                With Me
                    lrModelNoteInstance.Model = arPage.Model
                    lrModelNoteInstance.Page = arPage
                    lrModelNoteInstance.Symbol = .Symbol
                    lrModelNoteInstance.Id = .Id
                    lrModelNoteInstance.ModelNote = arPage.Model.ModelNote.Find(AddressOf .ModelNote.Equals)
                    lrModelNoteInstance.Name = .Name

                    lrModelNoteInstance.Text = .Text

                    lrModelNoteInstance.X = .X
                    lrModelNoteInstance.Y = .Y
                End With

                Return lrModelNoteInstance

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tModelNoteInstance.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrModelNoteInstance
            End Try

        End Function

        Public Function CloneConceptInstance() As FBM.ConceptInstance

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Id, Me.ConceptType)

            lrConceptInstance.Symbol = Me.ModelNote.Id
            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y

            Return lrConceptInstance

        End Function

        Public Sub MouseDown() Implements FBM.iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements FBM.iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements FBM.iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements FBM.iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements FBM.iPageObject.NodeDeleting

        End Sub

        Public Sub NodeModified() Implements FBM.iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements FBM.iPageObject.NodeSelected

        End Sub

        Public Sub DisplayAndAssociate()

            Try
                Dim loDroppedNode As ShapeNode
                Dim StringSize As SizeF

                StringSize = Me.Page.Diagram.MeasureString(Trim(Me.Text), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                StringSize.Height += 5

                If StringSize.Width > 70 Then
                    StringSize = New SizeF(70, (StringSize.Height + 2) * Me.Text.Length / 100)
                End If

                '--------------------------------------------------------------------
                'Create a Shape for the ValueTypeInstance on the DiagramView object
                '--------------------------------------------------------------------            
                loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, StringSize.Width, StringSize.Height)

                loDroppedNode.Shape = Shapes.Rectangle
                loDroppedNode.HandlesStyle = HandlesStyle.HatchFrame
                loDroppedNode.AllowIncomingLinks = False
                loDroppedNode.AllowOutgoingLinks = True
                loDroppedNode.Text = Trim(Me.Text)
                loDroppedNode.TextFormat.Alignment = StringAlignment.Near
                If Me.Text.Length > 5 Then
                    Call loDroppedNode.ResizeToFitText(FitSize.KeepWidth)
                Else
                    loDroppedNode.Resize(40, 10)
                End If

                loDroppedNode.ShadowOffsetX = 1
                loDroppedNode.ShadowOffsetY = 1
                loDroppedNode.ShadowColor = Color.LightGray
                loDroppedNode.Tag = New FBM.ModelNoteInstance
                loDroppedNode.Tag = Me
                loDroppedNode.Pen.Width = 0.1
                loDroppedNode.Pen.Color = Color.LightGray
                loDroppedNode.Visible = True
                loDroppedNode.ToolTip = "Model Note"

                ReDim loDroppedNode.Pen.DashPattern(3)
                loDroppedNode.Pen.DashPattern(0) = 3
                loDroppedNode.Pen.DashPattern(1) = 2
                loDroppedNode.Pen.DashPattern(2) = 3
                loDroppedNode.Pen.DashPattern(3) = 2

                Me.Shape = loDroppedNode

                Dim lrJoinedORMObject As Object = Me.JoinedObjectType

                If IsSomething(lrJoinedORMObject) Then
                    If lrJoinedORMObject.Name = "" Then
                        '---------------------------------------------------------------------------
                        'The ModelNote is not joined to any ModelElement on the Page.
                    Else
                        Dim loNode As MindFusion.Diagramming.ShapeNode = lrJoinedORMObject.Shape
                        Dim lo_link As New DiagramLink(Me.Page.Diagram, Me.Shape, loNode)
                        lo_link.Locked = True
                        ReDim lo_link.Pen.DashPattern(3)
                        lo_link.Pen.DashPattern(0) = 3
                        lo_link.Pen.DashPattern(1) = 2
                        lo_link.Pen.DashPattern(2) = 3
                        lo_link.Pen.DashPattern(3) = 2
                        lo_link.Pen.Color = Color.LightGray
                        Me.Page.Diagram.Links.Add(lo_link)
                        Me.Link = New DiagramLink(Me.Page.Diagram)
                        Me.Link = lo_link
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

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Try
                '-----------------------------------------
                'Saves the EntityInstance to the database
                '-----------------------------------------
                Dim lrConceptInstance As New FBM.ConceptInstance

                lrConceptInstance.ModelId = Me.Model.ModelId
                lrConceptInstance.PageId = Me.Page.PageId
                lrConceptInstance.Symbol = Me.Id
                lrConceptInstance.X = Me.X
                lrConceptInstance.Y = Me.Y
                lrConceptInstance.ConceptType = pcenumConceptType.ModelNote

                If abRapidSave Then
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                Else
                    If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                        Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                    Else
                        Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
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

        Public Sub RefreshShape()

        End Sub

        Public Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

        End Sub

        Private Sub ModelNote_JoinedObjectTypeChanged(ByVal arJoinedModelObject As ModelObject) Handles ModelNote.JoinedObjectTypeChanged

            Select Case arJoinedModelObject.ConceptType
                Case Is = pcenumConceptType.EntityType
                    Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
                    lrEntityTypeInstance.Id = arJoinedModelObject.Id
                    lrEntityTypeInstance = Me.Page.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)
                    If IsSomething(lrEntityTypeInstance) Then
                        Me.Link.Destination = lrEntityTypeInstance.Shape
                    End If
                Case Is = pcenumConceptType.ValueType
                    Dim lrValueTypeInstance As New FBM.ValueTypeInstance
                    lrValueTypeInstance.Id = arJoinedModelObject.Id
                    lrValueTypeInstance = Me.Page.ValueTypeInstance.Find(AddressOf lrValueTypeInstance.Equals)
                    If IsSomething(lrValueTypeInstance) Then
                        Me.Link.Destination = lrValueTypeInstance.Shape
                    End If
                Case Is = pcenumConceptType.FactType
                    Dim lrFactTypeInstance As New FBM.FactTypeInstance
                    lrFactTypeInstance.Id = arJoinedModelObject.Id
                    lrFactTypeInstance = Me.Page.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                    If IsSomething(lrFactTypeInstance) Then
                        Me.Link.Destination = lrFactTypeInstance.Shape
                    End If
                Case Is = pcenumConceptType.RoleConstraint
                    Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
                    lrRoleConstraintInstance.Id = arJoinedModelObject.Id
                    lrRoleConstraintInstance = Me.Page.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                    If IsSomething(lrRoleConstraintInstance) Then
                        Me.Link.Destination = lrRoleConstraintInstance.Shape
                    End If
            End Select

            Call Me.RefreshShape()

        End Sub

        Private Sub ModelNote_TextChanged(ByVal asNewText As String) Handles ModelNote.TextChanged

            Me.Text = asNewText
            Call Me.RefreshShape()

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

        End Sub

        Public Function RemoveFromPage(ByVal abBroadcastInterfaceEvent As Boolean) As Boolean

            Try
                RemoveFromPage = True

                Call Me.Page.RemoveModelNoteInstance(Me, abBroadcastInterfaceEvent)

                Call TableModelNoteInstance.DeleteModelNoteInstance(Me)
                Call Me.Page.MakeDirty()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try


        End Function

        Private Sub ModelNote_RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean) Handles ModelNote.RemovedFromModel

            Try
                Call Me.RemoveFromPage(abBroadcastInterfaceEvent)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            If Me.Page IsNot Nothing Then
                If Me.Page.Form IsNot Nothing Then
                    Call Me.Page.Form.EnableSaveButton()
                End If
            Else
                Try
                    frmMain.ToolStripButton_Save.Enabled = True
                Catch ex As Exception
                End Try
            End If
        End Sub
    End Class

End Namespace