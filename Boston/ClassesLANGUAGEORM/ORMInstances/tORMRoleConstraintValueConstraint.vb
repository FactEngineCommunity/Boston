Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports MindFusion.Drawing
Imports System.Reflection
Imports System.Xml.Serialization

Namespace FBM
    <Serializable()>
    Public Class ValueConstraint
        Inherits FBM.RoleConstraintInstance

        Private WithEvents _RoleConstraint As FBM.RoleConstraint
        Public Overrides Property RoleConstraint As FBM.RoleConstraint
            Get
                Return Me._RoleConstraint
            End Get
            Set(value As FBM.RoleConstraint)
                Me._RoleConstraint = value
            End Set
        End Property

        '<DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        <CategoryAttribute("Role Constraint Detail"),
             Browsable(True),
             [ReadOnly](False),
             BindableAttribute(True),
             DefaultValueAttribute(""),
             DesignOnly(False),
             DescriptionAttribute("Enter the Minimum value for the Value Constraint.")>
        Public Overrides Property MinimumValue As String
            Get
                Return _MinimumValue
            End Get
            Set(ByVal value As String)
                _MinimumValue = value
            End Set
        End Property

        '<DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        <CategoryAttribute("Role Constraint Detail"),
             Browsable(True),
             [ReadOnly](False),
             BindableAttribute(True),
             DefaultValueAttribute(""),
             DesignOnly(False),
             DescriptionAttribute("Enter the Maximum value for the Value Constraint.")>
        Public Overrides Property MaximumValue As String
            Get
                Return _MaximumValue
            End Get
            Set(ByVal value As String)
                _MaximumValue = value
            End Set
        End Property


        Sub New()

            MyBase.ConceptType = pcenumConceptType.RoleConstraint
            Me.RoleConstraintType = pcenumRoleConstraintType.ValueConstraint

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByRef arPage As FBM.Page, ByVal asId As String)

            Call Me.New()

            Me.Model = arModel
            Me.Page = arPage
            Me.Id = asId

        End Sub

        Public Overrides Sub DisplayAndAssociate()

            Dim loDroppedNode As FBM.RoleConstraintShape ' ShapeNode
            Dim StringSize As New SizeF
            Dim G As Graphics

            Try
                Dim lrRoleInstance As New FBM.RoleInstance
                Dim lsValueConstraintText As String = ""
                Dim lo_link As DiagramLink

                lsValueConstraintText = "{" & Me.MinimumValue & "..." & Me.MaximumValue & "}"

                '------------------------------------------
                'Must be associated with one or more Role
                '------------------------------------------
                If Me.RoleConstraintRole.Count = 0 Then
                    Throw New Exception("Cannot display and associate a FrequencyConstraint without first populating the RoleConstraintRole group.")
                End If

                lrRoleInstance = Me.RoleConstraintRole(0).Role

                Dim myFont As New Font(lrRoleInstance.Shape.Parent.Font.FontFamily, lrRoleInstance.Shape.Parent.Font.Size, FontStyle.Regular, GraphicsUnit.Pixel)
                G = frmDiagramORM.CreateGraphics
                StringSize = lrRoleInstance.Shape.Parent.MeasureString(lsValueConstraintText, lrRoleInstance.Shape.Parent.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                loDroppedNode = New FBM.RoleConstraintShape(Me.Page.Diagram, Me)
                loDroppedNode.Move(lrRoleInstance.Shape.Bounds.X, lrRoleInstance.Shape.Bounds.Y - (StringSize.Height * 3))
                Me.Page.Diagram.Nodes.Add(loDroppedNode)
                loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.White)
                'loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDroppedNode.HandlesStyle = HandlesStyle.HatchHandles
                loDroppedNode.Text = lsValueConstraintText

                'loDroppedNode.ResizeToFitText(FitSize.KeepHeight)
                loDroppedNode.TextColor = Color.Black
                loDroppedNode.Transparent = True
                loDroppedNode.Visible = True
                loDroppedNode.ZTop()

                '--------------------------------------------------------------------------------------
                'Attach the FrequencyConstraint.Shape to the RoleInstance.Shape to which it joins.
                '  This is so that if an 'AutoArrange' of the Shapes on the Page is called by
                '  the user, then the FrequencyConstraint will stay next to its associated Role.
                '--------------------------------------------------------------------------------------
                loDroppedNode.AttachTo(lrRoleInstance.Shape, AttachToNode.TopCenter)

                '---------------------------------------------------------------------------
                'Attach the Role.FrequencyConstraint ShapeNode to the Role ShapeGroup,                                    
                '---------------------------------------------------------------------------
                'loDroppedNode.AttachTo(lrRoleInstance.shape, AttachToNode.TopLeft)


                '--------------------------------------------------------------------------------
                'Create the link between the FrequencyConstraint and the Role that it joins to.
                '--------------------------------------------------------------------------------
                lo_link = New DiagramLink(lrRoleInstance.Shape.Parent, lrRoleInstance.Shape, loDroppedNode)
                lo_link.BaseShape = ArrowHead.None
                lo_link.HeadShape = ArrowHead.None
                lo_link.Pen.DashStyle = Drawing2D.DashStyle.Custom
                lo_link.Pen.DashPattern = New Single() {2, 1, 2, 1}
                'ReDim lo_link.Pen.DashPattern(3)
                'lo_link.Pen.DashPattern(0) = 1
                'lo_link.Pen.DashPattern(1) = 2
                'lo_link.Pen.DashPattern(2) = 1
                'lo_link.Pen.DashPattern(3) = 2
                Dim lrRoleConstraintRoleInstance As New FBM.RoleConstraintRoleInstance
                lrRoleConstraintRoleInstance.RoleConstraint = Me
                lo_link.Tag = lrRoleConstraintRoleInstance


                Me.Page.Diagram.Links.Add(lo_link)
                Me.Page.Diagram.Invalidate()

                Using graphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(New Bitmap(1, 1))
                    Dim lrTextSize As SizeF = graphics.MeasureString(loDroppedNode.Text, New Font(loDroppedNode.Font.ToString, 11, FontStyle.Regular, GraphicsUnit.Point))
                    'loDroppedNode.Resize(lrTextSize.Width, 5)
                    'loDroppedNode.Resize(TextRenderer.MeasureText(loDroppedNode.Text, loDroppedNode.Font).Width, 5)
                    loDroppedNode.Resize(loDroppedNode.Text.Length + 5, 5)
                End Using

                loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.White)
                loDroppedNode.Visible = True
                loDroppedNode.ImageAlign = ImageAlign.Stretch
                loDroppedNode.AllowOutgoingLinks = True

                loDroppedNode.Tag = New Object
                loDroppedNode.Tag = Me

                Me.Shape = loDroppedNode

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: FBM.RoleConstraintInstance.DisplayAndAssociate: " & ex.Message
                lsMessage &= vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            End Try

        End Sub

        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)


            Try

                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"
                            '------------------------------------------------
                            'Set the values in the underlying Model.EntityType
                            '------------------------------------------------
                            If Me.RoleConstraint.Name = Me.Name Then
                                '------------------------------------------------------------
                                'Nothing to do. Name of the RoleConstraint has not been changed.
                                '------------------------------------------------------------
                            Else
                                Me.RoleConstraint.SetName(Me.Name)
                                Me.Id = Me.Name
                                Me.Symbol = Me.Name
                            End If
                        Case Is = "MinimumValue"
                            Call Me.RoleConstraint.SetMinimumValue(Me.MinimumValue, True)
                            Call Me.EnableSaveButton()
                        Case Is = "MaximumValue"
                            Call Me.RoleConstraint.SetMaximumValue(Me.MaximumValue, True)
                            Call Me.EnableSaveButton()
                    End Select
                End If

                Me.Shape.Text = "{" & Me.MinimumValue & "..." & Me.MaximumValue & "}"

                Dim StringSize As New SizeF

                StringSize = Me.Page.Diagram.MeasureString(Me.Shape.Text, Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                Me.Shape.Resize(StringSize.Width, StringSize.Height)

                If IsSomething(Me.Shape) Then
                    If Me.RoleConstraint.HasModelError Then
                        Me.Shape.HandlesStyle = HandlesStyle.InvisibleMove
                        Me.Shape.Transparent = False
                        Me.Shape.Pen.Color = Color.Red
                    Else
                        Me.Shape.Transparent = True
                        Me.Shape.Pen.Color = Color.Navy
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try


        End Sub

        Private Sub RoleConstraint_MaximumValueChanged() Handles _RoleConstraint.MaximumValueChanged

            Try
                Dim lsValueConstraintText As String = ""

                Me.MinimumValue = Me.RoleConstraint.MinimumValue
                Me.MaximumValue = Me.RoleConstraint.MaximumValue

                lsValueConstraintText = "{" & Me.MinimumValue & "..." & Me.MaximumValue & "}"

                Me.Shape.Text = lsValueConstraintText

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub RoleConstraint_MinimumValueChanged() Handles _RoleConstraint.MinimumValueChanged

            Try
                Dim lsValueConstraintText As String = ""

                Me.MinimumValue = Me.RoleConstraint.MinimumValue
                Me.MaximumValue = Me.RoleConstraint.MaximumValue

                lsValueConstraintText = "{" & Me.MinimumValue & "..." & Me.MaximumValue & "}"

                Me.Shape.Text = lsValueConstraintText

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Overloads Sub SetAppropriateColour()

            If IsSomething(Me.Shape) Then
                If Me.Shape.Selected Then
                    Me.Shape.Pen.Color = Color.Blue
                Else
                    If Me.RoleConstraint.HasModelError Then
                        Me.Shape.HandlesStyle = HandlesStyle.InvisibleMove
                        Me.Shape.Transparent = False
                        Me.Shape.Visible = True
                        Me.Shape.Pen.Color = Color.Red
                    Else
                        Me.Shape.Transparent = True
                        Me.Shape.Pen.Color = Color.White
                    End If
                End If
            End If

        End Sub

        Private Sub _RoleConstraint_ValueConstraintCleared() Handles _RoleConstraint.ValueConstraintCleared

            Try
                Me._ValueConstraintList.Clear()
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace