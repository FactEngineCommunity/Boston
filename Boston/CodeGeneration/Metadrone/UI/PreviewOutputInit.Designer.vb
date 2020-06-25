Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class PreviewOutputInit
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
            Me.OK_Button = New System.Windows.Forms.Button
            Me.Cancel_Button = New System.Windows.Forms.Button
            Me.pnlTop = New System.Windows.Forms.Panel
            Me.tlpHeader = New System.Windows.Forms.TableLayoutPanel
            Me.lblHeader2 = New System.Windows.Forms.Label
            Me.lblHeader1 = New System.Windows.Forms.Label
            Me.pnlLine = New System.Windows.Forms.Panel
            Me.Label1 = New System.Windows.Forms.Label
            Me.tlpMain = New System.Windows.Forms.TableLayoutPanel
            Me.TableLayoutPanel1.SuspendLayout()
            Me.pnlTop.SuspendLayout()
            Me.tlpHeader.SuspendLayout()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.TableLayoutPanel1.ColumnCount = 2
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(429, 163)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 1
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
            Me.TableLayoutPanel1.TabIndex = 2
            '
            'OK_Button
            '
            Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.OK_Button.Location = New System.Drawing.Point(3, 3)
            Me.OK_Button.Name = "OK_Button"
            Me.OK_Button.Size = New System.Drawing.Size(67, 23)
            Me.OK_Button.TabIndex = 0
            Me.OK_Button.Text = "OK"
            '
            'Cancel_Button
            '
            Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
            Me.Cancel_Button.Name = "Cancel_Button"
            Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
            Me.Cancel_Button.TabIndex = 1
            Me.Cancel_Button.Text = "Cancel"
            '
            'pnlTop
            '
            Me.pnlTop.BackgroundImage = Global.Boston.My.Resources.CodeGeneration.DialogTop
            Me.pnlTop.Controls.Add(Me.tlpHeader)
            Me.pnlTop.Controls.Add(Me.pnlLine)
            Me.pnlTop.Controls.Add(Me.Label1)
            Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTop.Location = New System.Drawing.Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New System.Drawing.Size(587, 118)
            Me.pnlTop.TabIndex = 0
            '
            'tlpHeader
            '
            Me.tlpHeader.BackColor = System.Drawing.Color.Transparent
            Me.tlpHeader.ColumnCount = 2
            Me.tlpHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpHeader.Controls.Add(Me.lblHeader2, 0, 0)
            Me.tlpHeader.Controls.Add(Me.lblHeader1, 0, 0)
            Me.tlpHeader.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.tlpHeader.Location = New System.Drawing.Point(0, 86)
            Me.tlpHeader.Name = "tlpHeader"
            Me.tlpHeader.RowCount = 1
            Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpHeader.Size = New System.Drawing.Size(587, 31)
            Me.tlpHeader.TabIndex = 1
            '
            'lblHeader2
            '
            Me.lblHeader2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblHeader2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblHeader2.Location = New System.Drawing.Point(296, 0)
            Me.lblHeader2.Name = "lblHeader2"
            Me.lblHeader2.Size = New System.Drawing.Size(288, 31)
            Me.lblHeader2.TabIndex = 2
            Me.lblHeader2.Text = "Expression"
            Me.lblHeader2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblHeader1
            '
            Me.lblHeader1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblHeader1.Location = New System.Drawing.Point(3, 0)
            Me.lblHeader1.Name = "lblHeader1"
            Me.lblHeader1.Size = New System.Drawing.Size(287, 31)
            Me.lblHeader1.TabIndex = 1
            Me.lblHeader1.Text = "Parameter"
            Me.lblHeader1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlLine
            '
            Me.pnlLine.BackColor = System.Drawing.Color.LightGray
            Me.pnlLine.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlLine.Location = New System.Drawing.Point(0, 117)
            Me.pnlLine.Name = "pnlLine"
            Me.pnlLine.Size = New System.Drawing.Size(587, 1)
            Me.pnlLine.TabIndex = 2
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.BackColor = System.Drawing.Color.Transparent
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.Location = New System.Drawing.Point(21, 32)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(249, 20)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Assign template parameter values"
            '
            'tlpMain
            '
            Me.tlpMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tlpMain.ColumnCount = 2
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpMain.Location = New System.Drawing.Point(0, 123)
            Me.tlpMain.Name = "tlpMain"
            Me.tlpMain.RowCount = 1
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
            Me.tlpMain.Size = New System.Drawing.Size(587, 33)
            Me.tlpMain.TabIndex = 1
            '
            'PreviewOutputInit
            '
            Me.AcceptButton = Me.OK_Button
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.CancelButton = Me.Cancel_Button
            Me.ClientSize = New System.Drawing.Size(587, 204)
            Me.Controls.Add(Me.tlpMain)
            Me.Controls.Add(Me.pnlTop)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "PreviewOutputInit"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Preview Output Initialisation"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.pnlTop.ResumeLayout(False)
            Me.pnlTop.PerformLayout()
            Me.tlpHeader.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents OK_Button As System.Windows.Forms.Button
        Friend WithEvents Cancel_Button As System.Windows.Forms.Button
        Friend WithEvents pnlTop As System.Windows.Forms.Panel
        Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents tlpHeader As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents lblHeader2 As System.Windows.Forms.Label
        Friend WithEvents lblHeader1 As System.Windows.Forms.Label
        Friend WithEvents pnlLine As System.Windows.Forms.Panel

    End Class

End Namespace