Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class ManageDBExcel
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ManageDBExcel))
            Me.TabPage1 = New System.Windows.Forms.TabPage
            Me.chkHdr = New System.Windows.Forms.CheckBox
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.lblTitle = New System.Windows.Forms.Label
            Me.picAccess2K = New System.Windows.Forms.PictureBox
            Me.rbExcel2K = New System.Windows.Forms.RadioButton
            Me.picAccess = New System.Windows.Forms.PictureBox
            Me.rbExcel = New System.Windows.Forms.RadioButton
            Me.btnFile = New System.Windows.Forms.Button
            Me.btnTest = New System.Windows.Forms.Button
            Me.lblFile = New System.Windows.Forms.Label
            Me.txtConnectionString = New System.Windows.Forms.TextBox
            Me.txtFile = New System.Windows.Forms.TextBox
            Me.lblConnectionString = New System.Windows.Forms.Label
            Me.tcMain = New System.Windows.Forms.TabControl
            Me.TabPage2 = New System.Windows.Forms.TabPage
            Me.txtTransformations = New Boston.UI.TransformationsEditor
            Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.TabPage1.SuspendLayout()
            CType(Me.picAccess2K, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picAccess, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tcMain.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            Me.SuspendLayout()
            '
            'TabPage1
            '
            Me.TabPage1.BackColor = System.Drawing.Color.Transparent
            Me.TabPage1.Controls.Add(Me.chkHdr)
            Me.TabPage1.Controls.Add(Me.Panel2)
            Me.TabPage1.Controls.Add(Me.lblTitle)
            Me.TabPage1.Controls.Add(Me.picAccess2K)
            Me.TabPage1.Controls.Add(Me.rbExcel2K)
            Me.TabPage1.Controls.Add(Me.picAccess)
            Me.TabPage1.Controls.Add(Me.rbExcel)
            Me.TabPage1.Controls.Add(Me.btnFile)
            Me.TabPage1.Controls.Add(Me.btnTest)
            Me.TabPage1.Controls.Add(Me.lblFile)
            Me.TabPage1.Controls.Add(Me.txtConnectionString)
            Me.TabPage1.Controls.Add(Me.txtFile)
            Me.TabPage1.Controls.Add(Me.lblConnectionString)
            Me.TabPage1.ImageIndex = 0
            Me.TabPage1.Location = New System.Drawing.Point(4, 4)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(830, 580)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Connection"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'chkHdr
            '
            Me.chkHdr.AutoSize = True
            Me.chkHdr.Checked = True
            Me.chkHdr.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkHdr.Location = New System.Drawing.Point(79, 145)
            Me.chkHdr.Name = "chkHdr"
            Me.chkHdr.Size = New System.Drawing.Size(180, 17)
            Me.chkHdr.TabIndex = 7
            Me.chkHdr.Text = "First row identifies column names"
            Me.chkHdr.UseVisualStyleBackColor = True
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.Silver
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel2.Location = New System.Drawing.Point(3, 33)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(824, 1)
            Me.Panel2.TabIndex = 1
            '
            'lblTitle
            '
            Me.lblTitle.BackColor = System.Drawing.Color.White
            Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.DimGray
            Me.lblTitle.Location = New System.Drawing.Point(3, 3)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Padding = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.lblTitle.Size = New System.Drawing.Size(824, 30)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Microsoft Excel"
            '
            'picAccess2K
            '
            Me.picAccess2K.Image = CType(resources.GetObject("picAccess2K.Image"), System.Drawing.Image)
            Me.picAccess2K.Location = New System.Drawing.Point(79, 115)
            Me.picAccess2K.Name = "picAccess2K"
            Me.picAccess2K.Size = New System.Drawing.Size(16, 16)
            Me.picAccess2K.TabIndex = 30
            Me.picAccess2K.TabStop = False
            '
            'rbExcel2K
            '
            Me.rbExcel2K.Location = New System.Drawing.Point(101, 114)
            Me.rbExcel2K.Name = "rbExcel2K"
            Me.rbExcel2K.Size = New System.Drawing.Size(108, 17)
            Me.rbExcel2K.TabIndex = 6
            Me.rbExcel2K.Text = "Excel 97 - 2003"
            Me.rbExcel2K.UseVisualStyleBackColor = True
            '
            'picAccess
            '
            Me.picAccess.Image = CType(resources.GetObject("picAccess.Image"), System.Drawing.Image)
            Me.picAccess.Location = New System.Drawing.Point(79, 92)
            Me.picAccess.Name = "picAccess"
            Me.picAccess.Size = New System.Drawing.Size(16, 16)
            Me.picAccess.TabIndex = 28
            Me.picAccess.TabStop = False
            '
            'rbExcel
            '
            Me.rbExcel.Checked = True
            Me.rbExcel.Location = New System.Drawing.Point(101, 91)
            Me.rbExcel.Name = "rbExcel"
            Me.rbExcel.Size = New System.Drawing.Size(108, 17)
            Me.rbExcel.TabIndex = 5
            Me.rbExcel.TabStop = True
            Me.rbExcel.Text = "Excel"
            Me.rbExcel.UseVisualStyleBackColor = True
            '
            'btnFile
            '
            Me.btnFile.Location = New System.Drawing.Point(396, 57)
            Me.btnFile.Name = "btnFile"
            Me.btnFile.Size = New System.Drawing.Size(25, 22)
            Me.btnFile.TabIndex = 4
            Me.btnFile.Text = "..."
            Me.btnFile.UseVisualStyleBackColor = True
            '
            'btnTest
            '
            Me.btnTest.Image = CType(resources.GetObject("btnTest.Image"), System.Drawing.Image)
            Me.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnTest.Location = New System.Drawing.Point(20, 264)
            Me.btnTest.Name = "btnTest"
            Me.btnTest.Size = New System.Drawing.Size(112, 30)
            Me.btnTest.TabIndex = 10
            Me.btnTest.Text = "Test Connection"
            Me.btnTest.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnTest.UseVisualStyleBackColor = True
            '
            'lblFile
            '
            Me.lblFile.AutoSize = True
            Me.lblFile.Location = New System.Drawing.Point(17, 61)
            Me.lblFile.Name = "lblFile"
            Me.lblFile.Size = New System.Drawing.Size(26, 13)
            Me.lblFile.TabIndex = 2
            Me.lblFile.Text = "File:"
            '
            'txtConnectionString
            '
            Me.txtConnectionString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtConnectionString.Location = New System.Drawing.Point(20, 229)
            Me.txtConnectionString.Name = "txtConnectionString"
            Me.txtConnectionString.Size = New System.Drawing.Size(791, 20)
            Me.txtConnectionString.TabIndex = 9
            '
            'txtFile
            '
            Me.txtFile.Location = New System.Drawing.Point(79, 58)
            Me.txtFile.Name = "txtFile"
            Me.txtFile.Size = New System.Drawing.Size(317, 20)
            Me.txtFile.TabIndex = 3
            '
            'lblConnectionString
            '
            Me.lblConnectionString.AutoSize = True
            Me.lblConnectionString.Location = New System.Drawing.Point(17, 213)
            Me.lblConnectionString.Name = "lblConnectionString"
            Me.lblConnectionString.Size = New System.Drawing.Size(94, 13)
            Me.lblConnectionString.TabIndex = 8
            Me.lblConnectionString.Text = "Connection String:"
            '
            'tcMain
            '
            Me.tcMain.Alignment = System.Windows.Forms.TabAlignment.Bottom
            Me.tcMain.Controls.Add(Me.TabPage1)
            Me.tcMain.Controls.Add(Me.TabPage2)
            Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcMain.ImageList = Me.ImageList1
            Me.tcMain.Location = New System.Drawing.Point(0, 0)
            Me.tcMain.Name = "tcMain"
            Me.tcMain.SelectedIndex = 0
            Me.tcMain.Size = New System.Drawing.Size(838, 607)
            Me.tcMain.TabIndex = 1
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.txtTransformations)
            Me.TabPage2.ImageIndex = 1
            Me.TabPage2.Location = New System.Drawing.Point(4, 4)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Size = New System.Drawing.Size(830, 580)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Transformations"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'txtTransformations
            '
            Me.txtTransformations.BackColor = System.Drawing.SystemColors.Window
            Me.txtTransformations.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtTransformations.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtTransformations.Location = New System.Drawing.Point(0, 0)
            Me.txtTransformations.Name = "txtTransformations"
            Me.txtTransformations.ReadOnly = False
            Me.txtTransformations.SelectedText = ""
            Me.txtTransformations.SelectionLength = 0
            Me.txtTransformations.SelectionStart = 0
            Me.txtTransformations.Size = New System.Drawing.Size(830, 580)
            Me.txtTransformations.TabIndex = 1
            '
            'ImageList1
            '
            Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
            Me.ImageList1.Images.SetKeyName(0, "connection16x16.png")
            Me.ImageList1.Images.SetKeyName(1, "transform16x16.png")
            '
            'ManageDBExcel
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.tcMain)
            Me.Name = "ManageDBExcel"
            Me.Size = New System.Drawing.Size(838, 607)
            Me.TabPage1.ResumeLayout(False)
            Me.TabPage1.PerformLayout()
            CType(Me.picAccess2K, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picAccess, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tcMain.ResumeLayout(False)
            Me.TabPage2.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents picAccess2K As System.Windows.Forms.PictureBox
        Friend WithEvents rbExcel2K As System.Windows.Forms.RadioButton
        Friend WithEvents picAccess As System.Windows.Forms.PictureBox
        Friend WithEvents rbExcel As System.Windows.Forms.RadioButton
        Friend WithEvents btnFile As System.Windows.Forms.Button
        Friend WithEvents btnTest As System.Windows.Forms.Button
        Friend WithEvents lblFile As System.Windows.Forms.Label
        Friend WithEvents txtConnectionString As System.Windows.Forms.TextBox
        Friend WithEvents txtFile As System.Windows.Forms.TextBox
        Friend WithEvents lblConnectionString As System.Windows.Forms.Label
        Friend WithEvents tcMain As System.Windows.Forms.TabControl
        Friend WithEvents chkHdr As System.Windows.Forms.CheckBox
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents txtTransformations As Boston.UI.TransformationsEditor
        Friend WithEvents ImageList1 As System.Windows.Forms.ImageList

    End Class

End Namespace