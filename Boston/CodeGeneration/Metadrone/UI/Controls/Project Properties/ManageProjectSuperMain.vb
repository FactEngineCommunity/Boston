Namespace UI

    Friend Class ManageProjectSuperMain
        Private Default_Font As New Font("Lucida Console", 9)

        Public Shadows Event TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Shadows Event KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Public Shadows Event KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Public Event SavePress()
        Public Event Run()

        Private WithEvents txtBox As txtBox
        Private WithEvents txtRich As New RichTextBox()

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Try
                Me.txtBox = New txtBox(Boston.UI.txtBox.HighlightModes.MetadroneSuperMain)
                Me.txtBox.Dock = DockStyle.Fill
                Me.txtBox.ShowLineNumbers = True
                Me.pnlMain.Controls.Add(Me.txtBox)

            Catch ex As Exception
                'If just stand-alone exe, use the old crap one I made.
                Me.txtBox = Nothing
                Me.txtRich.Dock = DockStyle.Fill
                Me.txtRich.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.pnlMain.Controls.Add(Me.txtRich)

            End Try
        End Sub

        Private Sub ManageProjectExecPlan_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Set focus, etc
            Me.Font = Me.Default_Font
            Me.FocusText()
        End Sub

        Public Shadows Property Font() As Font
            Get
                Try
                    If Me.txtBox IsNot Nothing Then
                        Return Me.txtBox.Font
                    Else
                        Return Me.txtRich.Font
                    End If
                Catch ex As Exception
                    Return Me.txtRich.Font
                End Try
            End Get
            Set(ByVal value As Font)
                Try
                    If Me.txtBox IsNot Nothing Then
                        Me.txtBox.Font = value
                    Else
                        Me.txtRich.Font = value
                    End If
                Catch ex As Exception
                    Me.txtRich.Font = value
                End Try
            End Set
        End Property

        Public Property [ReadOnly]() As Boolean
            Get
                Try
                    If Me.txtBox IsNot Nothing Then
                        Return Me.txtBox.ReadOnly
                    Else
                        Return Me.txtRich.ReadOnly
                    End If
                Catch ex As Exception
                    Return Me.txtRich.ReadOnly
                End Try
            End Get
            Set(ByVal value As Boolean)
                Try
                    If Me.txtBox IsNot Nothing Then
                        Me.txtBox.ReadOnly = value
                    Else
                        Me.txtRich.ReadOnly = value
                    End If
                Catch ex As Exception
                    Me.txtRich.ReadOnly = value
                End Try
            End Set
        End Property

        Public Sub FocusText()
            Try
                If Me.txtBox IsNot Nothing Then
                    Me.txtBox.Focus()
                Else
                    Me.txtRich.Focus()
                End If
            Catch ex As Exception
                Me.txtRich.Focus()
            End Try
        End Sub

        Public Shadows Property Text() As String
            Get
                Try
                    If Me.txtBox IsNot Nothing Then
                        Return Me.txtBox.Text
                    Else
                        Return Me.txtRich.Text
                    End If
                Catch ex As Exception
                    Return Me.txtRich.Text
                End Try
            End Get
            Set(ByVal value As String)
                Try
                    If Me.txtBox IsNot Nothing Then
                        Me.txtBox.Text = value
                    Else
                        Me.txtRich.Text = value
                    End If
                Catch ex As Exception
                    Me.txtRich.Text = value
                End Try
            End Set
        End Property

        Public Shadows Property Enabled() As Boolean
            Get
                Try
                    If Me.txtBox IsNot Nothing Then
                        Return Me.txtBox.Enabled
                    Else
                        Return Me.txtRich.Enabled
                    End If
                Catch ex As Exception
                    Return Me.txtRich.Enabled
                End Try
            End Get
            Set(ByVal value As Boolean)
                Try
                    If Me.txtBox IsNot Nothing Then
                        Me.txtBox.Enabled = value
                    Else
                        Me.txtRich.Enabled = value
                    End If
                Catch ex As Exception
                    Me.txtRich.Enabled = value
                End Try
            End Set
        End Property

        Public Property SelectedText() As String
            Get
                Try
                    If Me.txtBox IsNot Nothing Then
                        Return Me.txtBox.SelectedText
                    Else
                        Return Me.txtRich.SelectedText
                    End If
                Catch ex As Exception
                    Return Me.txtRich.SelectedText
                End Try
            End Get
            Set(ByVal value As String)
                Try
                    If Me.txtBox IsNot Nothing Then
                        Me.txtBox.SelectedText = value
                    Else
                        Me.txtRich.SelectedText = value
                    End If
                Catch ex As Exception
                    Me.txtRich.SelectedText = value
                End Try
            End Set
        End Property

        Public Property SelectionStart() As Integer
            Get
                Try
                    If Me.txtRich IsNot Nothing Then
                        Return Me.txtRich.SelectionStart
                    End If
                Catch ex As Exception
                    Return Me.txtRich.SelectionStart
                End Try
            End Get
            Set(ByVal value As Integer)
                Try
                    If Me.txtRich IsNot Nothing Then
                        Me.txtRich.SelectionStart = value
                    End If
                Catch ex As Exception
                    Me.txtRich.SelectionStart = value
                End Try
            End Set
        End Property

        Public Property SelectionLength() As Integer
            Get
                Try
                    If Me.txtRich IsNot Nothing Then
                        Return Me.txtRich.SelectionLength
                    End If
                Catch ex As Exception
                    Return Me.txtRich.SelectionLength
                End Try
            End Get
            Set(ByVal value As Integer)
                Try
                    If Me.txtRich IsNot Nothing Then
                        Me.txtRich.SelectionLength = value
                    End If
                Catch ex As Exception
                    Me.txtRich.SelectionLength = value
                End Try
            End Set
        End Property

        Public Sub SetSelection(ByVal offsetStartChars As Integer, ByVal offsetCharLength As Integer)
            If Me.txtBox IsNot Nothing Then Me.txtBox.SetSelection(offsetStartChars, offsetCharLength)
        End Sub

        Private Sub txtBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBox.KeyDown
            Try
                Select Case e.KeyCode
                    Case Keys.S
                        'CRTL-S save
                        If e.Modifiers = Keys.Control Then
                            e.Handled = True
                            e.SuppressKeyPress = True
                            RaiseEvent SavePress()
                        End If

                    Case Keys.F5
                        'F5 - run
                        e.Handled = True
                        e.SuppressKeyPress = True
                        RaiseEvent Run()

                End Select

                RaiseEvent KeyDown(sender, e)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub txtBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBox.KeyPress
            Try
                RaiseEvent KeyPress(sender, e)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub txtBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBox.TextChanged
            Try
                RaiseEvent TextChanged(sender, e)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub txtRich_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRich.KeyDown
            Try
                Select Case e.KeyCode
                    Case Keys.S
                        'CRTL-S save
                        If e.Modifiers = Keys.Control Then
                            e.Handled = True
                            e.SuppressKeyPress = True
                            RaiseEvent SavePress()
                        End If
                End Select

                RaiseEvent KeyDown(sender, e)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub txtRich_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRich.KeyPress
            Try
                RaiseEvent KeyPress(sender, e)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub txtRich_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRich.TextChanged
            Try
                RaiseEvent TextChanged(sender, e)
            Catch ex As Exception
            End Try
        End Sub

    End Class

End Namespace