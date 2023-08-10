<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPDFDocumentViewer
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

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
        Me.components = New System.ComponentModel.Container()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabelPage = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripTextBoxPage = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripButtonGetPageText = New System.Windows.Forms.ToolStripButton()
        Me.PdfViewer = New PdfiumViewer.PdfViewer()
        Me.TimerGotoPage = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStrip1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.PdfViewer, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(800, 450)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabelPage, Me.ToolStripTextBoxPage, Me.ToolStripButtonGetPageText})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(800, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripLabelPage
        '
        Me.ToolStripLabelPage.Name = "ToolStripLabelPage"
        Me.ToolStripLabelPage.Size = New System.Drawing.Size(36, 22)
        Me.ToolStripLabelPage.Text = "Page:"
        '
        'ToolStripTextBoxPage
        '
        Me.ToolStripTextBoxPage.BackColor = System.Drawing.Color.Linen
        Me.ToolStripTextBoxPage.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ToolStripTextBoxPage.Name = "ToolStripTextBoxPage"
        Me.ToolStripTextBoxPage.Size = New System.Drawing.Size(25, 25)
        '
        'ToolStripButtonGetPageText
        '
        Me.ToolStripButtonGetPageText.Checked = True
        Me.ToolStripButtonGetPageText.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.ToolStripButtonGetPageText.Image = Global.Boston.My.Resources.Resources.Copy16x16
        Me.ToolStripButtonGetPageText.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonGetPageText.Name = "ToolStripButtonGetPageText"
        Me.ToolStripButtonGetPageText.Size = New System.Drawing.Size(98, 22)
        Me.ToolStripButtonGetPageText.Text = "Get Page Text"
        '
        'PdfViewer
        '
        Me.PdfViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PdfViewer.Location = New System.Drawing.Point(3, 28)
        Me.PdfViewer.Name = "PdfViewer"
        Me.PdfViewer.Size = New System.Drawing.Size(794, 419)
        Me.PdfViewer.TabIndex = 1
        '
        'TimerGotoPage
        '
        Me.TimerGotoPage.Interval = 2500
        '
        'frmPDFDocumentViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "frmPDFDocumentViewer"
        Me.Text = "PDF Viewer"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripLabelPage As ToolStripLabel
    Friend WithEvents ToolStripTextBoxPage As ToolStripTextBox
    Friend WithEvents TimerGotoPage As Timer
    Friend WithEvents ToolStripButtonGetPageText As ToolStripButton
    Friend WithEvents PdfViewer As PdfiumViewer.PdfViewer
End Class
