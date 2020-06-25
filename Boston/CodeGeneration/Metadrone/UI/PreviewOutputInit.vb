Imports System.Windows.Forms

Namespace UI

    Friend Class PreviewOutputInit

        Public ParamNames As New List(Of String)
        Public ParamVals As New List(Of Object)

        Private Sub PreviewOutputInit_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Clear values first
            Me.ParamVals.Clear()
            For Each p In Me.ParamNames
                Me.ParamVals.Add(Nothing)
            Next

            'Set rows (+1 spacer)
            Me.tlpMain.RowCount = Me.ParamNames.Count + 1
            Me.tlpMain.RowStyles(0).Height = 26

            'Add controls
            For i As Integer = 0 To Me.ParamNames.Count - 1
                Dim lbl As New Label()
                lbl.Name = "lbl" & i.ToString
                lbl.Text = Me.ParamNames(i)
                lbl.AutoSize = True
                lbl.TextAlign = ContentAlignment.MiddleRight
                lbl.Dock = DockStyle.Fill
                Me.tlpMain.Controls.Add(lbl, 0, i)

                Dim txt As New TextBox()
                txt.Name = "txt" & i.ToString
                txt.Dock = DockStyle.Fill
                Me.tlpMain.Controls.Add(txt, 1, i)
            Next

            Me.Height = 250 + Me.tlpMain.RowCount * 20
        End Sub

        Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
            'Evaluate expressions
            'Don't do last spacer row
            For i As Integer = 0 To Me.tlpMain.RowCount - 2
                Try
                    Dim toks As Parser.Syntax.SyntaxTokenCollection = Parser.Syntax.Strings.Tokenise(Me.tlpMain.GetControlFromPosition(1, i).Text, 0)
                    Me.ParamVals(i) = Parser.Syntax.Exec_Expr.EvalExpression(toks, Nothing, 0)

                Catch ex As Exception
                    MessageBox.Show("Failed to evaluate '" & Me.ParamNames(i) & "'." & System.Environment.NewLine & ex.Message, _
                                    "Evaluation Error", _
                                    MessageBoxButtons.OK, _
                                    MessageBoxIcon.Exclamation)
                    Exit Sub

                End Try
            Next

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End Sub

        Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End Sub

    End Class

End Namespace