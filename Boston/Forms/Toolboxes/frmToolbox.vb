Imports MindFusion.Diagramming
Imports System.Reflection
Imports System.IO

Public Class frmToolbox

    Dim zsl_shape_library As ShapeLibrary

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frmToolbox_Enter(sender As Object, e As EventArgs) Handles Me.Enter

        If My.Settings.UseClientServer Then
            If prApplication.User.CanAlterOnProject(prApplication.WorkingProject) = False Then
                '-------------------------------------
                'Display the FlashCard to show that the User has logged in
                Me.ShapeListBox.Enabled = False
            Else
                Me.ShapeListBox.Enabled = True
            End If
        Else
            Me.ShapeListBox.Enabled = True
        End If

    End Sub


    Private Sub frm_toolbox_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        'If IsSomething(frmMain.zfrm_toolbox) Then
        '    frmMain.zfrm_toolbox = Nothing
        'End If
        prApplication.RightToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub frm_toolbox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Call SetupForm()
        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub SetupForm()

        Try
            '-----------------------------------------------------------------------------
            'NB Each Diagram/Page form (e.g. frm_ORMModel_page) loads it's own toolbox.
            '-----------------------------------------------------------------------------
            'Me.ToolBox.AddTab("Toolbox", -1)
            'Me.ToolBox.AddTab("Tab 2 (TreeView)", -1)
            'Me.ToolBox.AddTab("Tab 3", -1)
            'Me.ToolBox.AddTab("Tab 4", -1)
            'Me.ToolBox.AddTab("Tab 5 (TreeView)", -1)

            'Dim ToolShapeListBox As New MindFusion.Diagramming.WinForms.ShapeListBox

            'ToolShapeListBox.BorderStyle = BorderStyle.None
            'ToolShapeListBox.Dock = DockStyle.Fill
            'Me.ToolBox(1).Control = ToolShapeListBox

            'Me.ToolBox(0).Selected = True
            'Me.ToolBox(0).Control = Me.ShapeListBox

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ShapeListBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ShapeListBox.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Left Then
            DoDragDrop(New tShapeNodeDragItem(ShapeListBox.SelectedIndex, ShapeListBox.SelectedItem.Shape), DragDropEffects.Copy)
        End If

    End Sub

    Private Sub PictureBox1_GiveFeedback(ByVal sender As Object, ByVal e As System.Windows.Forms.GiveFeedbackEventArgs) Handles ShapeListBox.GiveFeedback

        'e.UseDefaultCursors = True

        'If ((e.Effect And DragDropEffects.Copy) = DragDropEffects.Copy) Then
        'Dim ms As New System.IO.MemoryStream(1 )
        'Me.Cursor = Cursors.Default
        'Else
        'Cursor.Current = System.Windows.Forms.Cursors.Default
        'End If
    End Sub

    Sub SetToolbox(ByVal aiLanguage As pcenumLanguage)

        Try
            Dim loShapeLibrary As ShapeLibrary
            Dim loShape As Shape

            Call Directory.SetCurrentDirectory(Boston.MyPath)

            Select Case aiLanguage
                Case Is = pcenumLanguage.ORMModel
                    loShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.ORMShapeLibrary)

                    Me.ShapeListBox.Shapes = loShapeLibrary.Shapes

                    For Each loShape In Me.ShapeListBox.Shapes
                        Select Case loShape.Id
                            Case Is = "Entity Type"
                                loShape.Image = My.Resources.ORMShapes.EntityType
                            Case Is = "Value Type"
                                loShape.Image = My.Resources.ORMShapes.ValueType
                            Case Is = "Subtype Connector"
                                loShape.Image = My.Resources.ORMShapes.SubtypeConnector
                            Case Is = "Ring Constraint"
                                loShape.Image = My.Resources.ORMShapes.acyclic
                            Case Is = "External Uniqueness Constraint"
                                loShape.Image = My.Resources.ORMShapes.externalUniqueness
                            Case Is = "Equality Constraint"
                                loShape.Image = My.Resources.ORMShapes.equality
                            Case Is = "Exclusion Constraint"
                                loShape.Image = My.Resources.ORMShapes.exclusion
                            Case Is = "Inclusive-OR Constraint"
                                loShape.Image = My.Resources.ORMShapes.inclusive_or
                            Case Is = "Exclusive-OR Constraint"
                                loShape.Image = My.Resources.ORMShapes.exclusiveOr
                            Case Is = "Subset Constraint"
                                loShape.Image = My.Resources.ORMShapes.subset
                            Case Is = "Frequency Constraint"
                                loShape.Image = My.Resources.ORMShapes.frequency_ge
                            Case Is = "Value Comparison Constraint"
                                loShape.Image = My.Resources.ORMShapes.value_comparison
                            Case Is = "Model Note"
                                loShape.ImageRectangle = New RectangleF(0, 0, 75, 120)
                                loShape.Image = My.Resources.ORMShapes.ModelNote
                        End Select
                    Next
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class