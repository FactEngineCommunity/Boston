Namespace ExampleDuplexServiceClient
	Partial Class MainForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		'''// <summary>
		'''// Clean up any resources being used.
		'''// </summary>
		'''// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		'protected override void Dispose(bool disposing)
		'{
		'    if (disposing && (components != null))
		'    {
		'        components.Dispose();
		'    }
		'    base.Dispose(disposing);
		'}

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
            Me.itemTextBox = New System.Windows.Forms.TextBox()
            Me.addItemButton = New System.Windows.Forms.Button()
            Me.addItemLabel = New System.Windows.Forms.Label()
            Me.groceryListLabel = New System.Windows.Forms.Label()
            Me.groceryListBox = New System.Windows.Forms.ListBox()
            Me.Button1 = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'itemTextBox
            '
            Me.itemTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.itemTextBox.Location = New System.Drawing.Point(83, 11)
            Me.itemTextBox.Name = "itemTextBox"
            Me.itemTextBox.Size = New System.Drawing.Size(223, 20)
            Me.itemTextBox.TabIndex = 0
            '
            'addItemButton
            '
            Me.addItemButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.addItemButton.Location = New System.Drawing.Point(312, 9)
            Me.addItemButton.Name = "addItemButton"
            Me.addItemButton.Size = New System.Drawing.Size(75, 23)
            Me.addItemButton.TabIndex = 1
            Me.addItemButton.Text = "Add Item"
            Me.addItemButton.UseVisualStyleBackColor = True
            '
            'addItemLabel
            '
            Me.addItemLabel.AutoSize = True
            Me.addItemLabel.Location = New System.Drawing.Point(13, 14)
            Me.addItemLabel.Name = "addItemLabel"
            Me.addItemLabel.Size = New System.Drawing.Size(64, 13)
            Me.addItemLabel.TabIndex = 2
            Me.addItemLabel.Text = "Item to Add:"
            '
            'groceryListLabel
            '
            Me.groceryListLabel.AutoSize = True
            Me.groceryListLabel.Location = New System.Drawing.Point(11, 47)
            Me.groceryListLabel.Name = "groceryListLabel"
            Me.groceryListLabel.Size = New System.Drawing.Size(66, 13)
            Me.groceryListLabel.TabIndex = 3
            Me.groceryListLabel.Text = "Grocery List:"
            '
            'groceryListBox
            '
            Me.groceryListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.groceryListBox.FormattingEnabled = True
            Me.groceryListBox.Location = New System.Drawing.Point(12, 63)
            Me.groceryListBox.Name = "groceryListBox"
            Me.groceryListBox.Size = New System.Drawing.Size(375, 186)
            Me.groceryListBox.TabIndex = 4
            '
            'Button1
            '
            Me.Button1.Location = New System.Drawing.Point(284, 34)
            Me.Button1.Name = "Button1"
            Me.Button1.Size = New System.Drawing.Size(103, 23)
            Me.Button1.TabIndex = 5
            Me.Button1.Text = "Test Broadcast"
            Me.Button1.UseVisualStyleBackColor = True
            '
            'MainForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(399, 262)
            Me.Controls.Add(Me.Button1)
            Me.Controls.Add(Me.groceryListBox)
            Me.Controls.Add(Me.groceryListLabel)
            Me.Controls.Add(Me.addItemLabel)
            Me.Controls.Add(Me.addItemButton)
            Me.Controls.Add(Me.itemTextBox)
            Me.MinimumSize = New System.Drawing.Size(250, 300)
            Me.Name = "MainForm"
            Me.Text = "Client - Main Form"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

		#End Region

		Private itemTextBox As System.Windows.Forms.TextBox
        Private WithEvents addItemButton As System.Windows.Forms.Button
		Private addItemLabel As System.Windows.Forms.Label
		Private groceryListLabel As System.Windows.Forms.Label
        Private groceryListBox As System.Windows.Forms.ListBox
        Friend WithEvents Button1 As System.Windows.Forms.Button
	End Class
End Namespace

