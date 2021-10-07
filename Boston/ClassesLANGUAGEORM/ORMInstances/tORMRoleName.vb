Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class RoleName
        Inherits FBM.PageObject
        Implements FBM.iPageObject


        Public RoleInstance As FBM.RoleInstance

        Public Shadows Property X As Integer Implements FBM.iPageObject.X
        Public Shadows Property Y As Integer Implements FBM.iPageObject.Y

        Sub New()

            Me.ConceptType = pcenumConceptType.RoleName

        End Sub

        Sub New(ByRef arRoleInstance As FBM.RoleInstance, ByVal asRoleName As String)

            Me.New()
            Me.Name = Trim(asRoleName)
            Me.Id = Me.Name
            Me.Symbol = Me.Name

            Me.Model = arRoleInstance.Model
            Me.Page = arRoleInstance.Page
            Me.RoleInstance = arRoleInstance

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance) As Object

            Dim lrRoleName As New FBM.RoleName

            With Me
                lrRoleName.Page = arPage
                lrRoleName.Model = arPage.Model
                lrRoleName.Id = .Id
                lrRoleName.Name = .Name
                lrRoleName.Symbol = .Symbol
                lrRoleName.RoleInstance = arRoleInstance
                lrRoleName.Shape = Nothing
                lrRoleName.X = .X
                lrRoleName.Y = .Y
            End With

            Return lrRoleName

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return True
        End Function

        Public Sub DisplayAndAssociate(ByRef arRoleInstance As FBM.RoleInstance)

            '----------------------------------------
            'Setup the RoleName
            '----------------------------------------                        
            Dim loRoleName As ShapeNode
            Dim StringSize As New SizeF
            Dim G As Graphics

            G = Me.Page.Form.CreateGraphics
            StringSize = Me.Page.Diagram.MeasureString(Trim("[" & Me.Name & "]"), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

            If Me.Name = "" And (Me.X = 0 And Me.Y = 0) Then
                loRoleName = Me.Page.Diagram.Factory.CreateShapeNode(arRoleInstance.Shape.Bounds.X, arRoleInstance.Shape.Bounds.Y - (StringSize.Height * 2), StringSize.Width, StringSize.Height + 2, MindFusion.Diagramming.Shapes.Rectangle)
            Else
                loRoleName = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, StringSize.Width, StringSize.Height + 2, MindFusion.Diagramming.Shapes.Rectangle)
            End If

            loRoleName.HandlesStyle = HandlesStyle.InvisibleMove
            If Trim(Me.Name) = "" Then
                loRoleName.Text = ""
            Else
                loRoleName.Text = "[" & Trim(Me.Name) & "]"
            End If

            loRoleName.TextColor = Color.Blue
            loRoleName.Transparent = True
            If Me.RoleInstance.FactType.isPreferredReferenceMode Then
                loRoleName.Visible = False
            Else
                loRoleName.Visible = True
            End If

            loRoleName.ZTop()

            Me.Model = arRoleInstance.Model
            Me.Page = arRoleInstance.Page
            Me.RoleInstance = arRoleInstance
            loRoleName.Tag = Me

            Me.Shape = loRoleName
            Me.X = Me.Shape.Bounds.X
            Me.Y = Me.Shape.Bounds.Y

            If Math.Abs(Me.Shape.Bounds.X - arRoleInstance.Shape.Bounds.X) > 35 Or Math.Abs(Me.Shape.Bounds.Y - arRoleInstance.Shape.Bounds.Y) > 35 Then
                Me.Move(arRoleInstance.Shape.Bounds.X - 4, arRoleInstance.Shape.Bounds.Y - 15, True)
            End If

        End Sub

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '-----------------------------------------
            'Saves the EntityInstance to the database
            '-----------------------------------------
            Dim lrConceptInstance As New FBM.ConceptInstance

            lrConceptInstance.ModelId = Me.Model.ModelId
            lrConceptInstance.PageId = Me.Page.PageId
            lrConceptInstance.Symbol = Me.RoleInstance.Name
            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y
            lrConceptInstance.RoleId = Me.RoleInstance.Id
            lrConceptInstance.ConceptType = pcenumConceptType.RoleName

            '--------------------------------------------------
            'Make sure the new Symbol is in the Concept table
            '--------------------------------------------------
            Dim lrConcept As New FBM.Concept(Me.RoleInstance.Name, True)
            lrConcept.Save()

            If abRapidSave Then
                Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
            Else
                If TableConceptInstance.ExistsConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance) Then
                    Call TableConceptInstance.UpdateConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance)
                Else
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                End If
            End If


        End Sub

        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)

            Try
                Dim lo_Diagram As MindFusion.Diagramming.Diagram
                Dim StringSize As New SizeF

                If Me.Page.Diagram Is Nothing Then
                    Exit Sub
                End If

                lo_Diagram = Me.Shape.Parent

                If lo_Diagram Is Nothing Then
                    Exit Sub
                End If

                'Dim myFont As New Font(lo_Diagram.Font.FontFamily, lo_Diagram.Font.Size, FontStyle.Regular, GraphicsUnit.Pixel)

                If Me.RoleInstance.Name = "" Then
                    StringSize = lo_Diagram.MeasureString("", lo_Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                Else
                    StringSize = lo_Diagram.MeasureString("[" & Trim(Me.RoleInstance.Name & "]]"), lo_Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                End If

                '------------------------------------------------------
                'RoleName is already displayed for the RoleInstance
                '------------------------------------------------------
                If Me.RoleInstance.Name = "" Then
                    Me.Shape.Text = ""
                Else
                    Me.Shape.Text = "[" & Trim(Me.RoleInstance.Name) & "]"
                End If
                Me.Shape.Resize(StringSize.Width, StringSize.Height + 2)


                Me.Shape.TextColor = Color.Blue
                Me.Shape.Transparent = True
                Me.Shape.Visible = True
                Me.Shape.ZTop()

                If Me.Y < 0 Then
                    Me.Y = Me.RoleInstance.Shape.Bounds.Y - 12
                End If

                If Math.Abs(Me.X - Me.RoleInstance.X) > (25 + StringSize.Width) Then
                    Me.X = Me.RoleInstance.X - 4
                End If

                If Math.Abs(Me.Y - Me.RoleInstance.Y) > 35 Then
                    Me.Y = Me.RoleInstance.Y - 12
                End If

                Me.Shape.AllowOutgoingLinks = False
                Me.Shape.Move(Me.X, Me.Y)
                Me.Move(Me.X, Me.Y, True)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Overloads Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Overloads Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Overloads Sub Moved() Implements iPageObject.Moved

        End Sub

        Public Overloads Sub NodeDeleting() Implements iPageObject.NodeDeleting

        End Sub

        Public Overloads Sub NodeDeselected() Implements iPageObject.NodeDeselected

        End Sub

        Public Overloads Sub NodeModified() Implements iPageObject.NodeModified

        End Sub

        Public Overloads Sub NodeSelected() Implements iPageObject.NodeSelected

        End Sub

        Public Overloads Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Overloads Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Overrides Sub makeDirty()
            MyBase.makeDirty()

            Me.isDirty = True
            Me.RoleInstance.makeDirty()
        End Sub

        Public Overloads Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

            Try
                Me.X = aiNewX
                Me.Y = aiNewY

                Me.Shape.Move(Me.X, Me.Y)
                Call Me.makeDirty()
            Catch ex As Exception

            End Try

        End Sub

        Public Overloads Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace