Namespace Tools

    Friend Class ThreadedControlUpdates
        'Delegates to enable asynchronous calls for setting form control properties
        Private Delegate Sub SetLabelTextCallback(ByRef Form As Form, ByRef Label As Label, ByVal TextValue As String)
        Private Delegate Sub SetTextBoxTextCallback(ByRef Form As Form, ByRef TextBox As TextBox, ByVal TextValue As String)
        Private Delegate Sub SetRichTextBoxTextCallback(ByRef Form As Form, ByRef RichTextBox As RichTextBox, ByVal TextValue As String)
        Private Delegate Sub SetButtonTextCallback(ByRef Form As Form, ByRef Button As Button, ByVal TextValue As String)
        Private Delegate Sub SetButtonVisibleCallback(ByRef Form As Form, ByRef Button As Button, ByVal Visible As Boolean)
        Private Delegate Sub SetProgressCallback(ByRef Form As Form, ByRef Bar As ProgressBar, ByVal Percentage As Double)
        Private Delegate Sub SetProgressVisibleCallback(ByRef Form As Form, ByRef Bar As ProgressBar, ByVal Visible As Boolean)
        Private Delegate Sub AddListitemCallback(ByRef Form As Form, ByRef Listbox As ListBox, ByVal Item As String)
        Private Delegate Sub SetCursorCallback(ByRef Form As Form, ByVal Cursor As System.Windows.Forms.Cursor)
        Private Delegate Sub CloseFormCallback(ByRef Form As Form)
        Private Delegate Sub SetPanelVisibleCallback(ByRef Panel As Panel, ByVal Visible As Boolean)
        Private Delegate Sub AddControlToContainerCallback(ByRef Container As Control, ByVal Control As Control)


        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        Public Shared Sub SetLabelText(ByRef Form As Form, ByRef Label As Label, ByVal TextValue As String)
            If Label.InvokeRequired Then
                Dim d As New SetLabelTextCallback(AddressOf SetLabelText)
                Form.Invoke(d, New Object() {Form, Label, TextValue})
            Else
                Label.Text = TextValue
                Label.Refresh()
                'Label.ForeColor = Color.Black
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub SetTextBoxText(ByRef Form As Form, ByRef TextBox As TextBox, ByVal TextValue As String)
            If TextBox.InvokeRequired Then
                Dim d As New SetTextBoxTextCallback(AddressOf SetTextBoxText)
                Form.Invoke(d, New Object() {Form, TextBox, TextValue})
            Else
                TextBox.Text = TextValue
                TextBox.Refresh()
                'Label.ForeColor = Color.Black
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub SetRichTextBoxText(ByRef Form As Form, ByRef RichTextBox As RichTextBox, ByVal TextValue As String)
            If RichTextBox.InvokeRequired Then
                Dim d As New SetRichTextBoxTextCallback(AddressOf SetRichTextBoxText)
                Form.Invoke(d, New Object() {Form, RichTextBox, TextValue})
            Else
                RichTextBox.Text = TextValue
                RichTextBox.Refresh()
                'Label.ForeColor = Color.Black
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub SetButtonText(ByRef Form As Form, ByRef Button As Button, ByVal TextValue As String)
            If Button.InvokeRequired Then
                Dim d As New SetButtonTextCallback(AddressOf SetButtonText)
                Form.Invoke(d, New Object() {Form, Button, TextValue})
            Else
                Button.Text = TextValue
                Button.Refresh()
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub SetButtonVisible(ByRef Form As Form, ByRef Button As Button, ByVal Visible As Boolean)
            If Button.InvokeRequired Then
                Dim d As New SetButtonVisibleCallback(AddressOf SetButtonVisible)
                Form.Invoke(d, New Object() {Form, Button, Visible})
            Else
                Button.Visible = Visible
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub SetProgress(ByRef Form As Form, ByRef Bar As ProgressBar, ByVal Percentage As Double)
            If Bar.InvokeRequired Then
                Dim d As New SetProgressCallback(AddressOf SetProgress)
                Form.Invoke(d, New Object() {Form, Bar, Percentage})
            Else
                If Convert.ToInt32(Bar.Maximum * (Percentage / 100)) < Bar.Maximum Then
                    Bar.Value = Convert.ToInt32(Bar.Maximum * (Percentage / 100))
                Else
                    Bar.Value = Bar.Maximum
                End If
                Bar.Refresh()
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub SetProgressVisible(ByRef Form As Form, ByRef Bar As ProgressBar, ByVal Visible As Boolean)
            If Bar.InvokeRequired Then
                Dim d As New SetProgressVisibleCallback(AddressOf SetProgressVisible)
                Form.Invoke(d, New Object() {Form, Bar, Visible})
            Else
                Bar.Visible = Visible
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub AddListitem(ByRef Form As Form, ByRef Listbox As ListBox, ByVal Item As String)
            If Listbox.InvokeRequired Then
                Dim d As New AddListitemCallback(AddressOf AddListitem)
                Form.Invoke(d, New Object() {Form, Listbox, Item})
            Else
                Listbox.Items.Add(Item)
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub SetCursor(ByRef Form As Form, ByVal Cursor As System.Windows.Forms.Cursor)
            If Form.InvokeRequired Then
                Dim d As New SetCursorCallback(AddressOf SetCursor)
                Form.Invoke(d, New Object() {Form, Cursor})
            Else
                Form.Cursor = Cursor
                Form.Refresh()
            End If
        End Sub

        Public Shared Sub CloseForm(ByRef Form As Form)
            If Form.InvokeRequired Then
                Dim d As New CloseFormCallback(AddressOf CloseForm)
                Form.Invoke(d, New Object() {Form})
            Else
                Form.Close()
            End If
        End Sub

        Public Shared Sub SetPanelVisible(ByRef Panel As Panel, ByVal Visible As Boolean)
            If Panel.InvokeRequired Then
                Dim d As New SetPanelVisibleCallback(AddressOf SetPanelVisible)
                Panel.Invoke(d, New Object() {Panel, Visible})
            Else
                Panel.Visible = Visible
                Panel.Refresh()
            End If
        End Sub

        Public Shared Sub AddControlToContainer(ByRef Container As Control, ByVal Control As Control)
            If Container.InvokeRequired Then
                Dim d As New AddControlToContainerCallback(AddressOf AddControlToContainer)
                Container.Invoke(d, New Object() {Container, Control})
            Else
                Container.Controls.Add(Control)
                Container.Refresh()
            End If
        End Sub

    End Class

End Namespace