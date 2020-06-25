Namespace UI

    Friend Class LinkButton

        Public Event LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)

        Public Property Image() As Image
            Get
                Return Me.PictureBox1.BackgroundImage
            End Get
            Set(ByVal value As Image)
                Me.PictureBox1.BackgroundImage = value
            End Set
        End Property

        Public Property LinkText() As String
            Get
                Return Me.LinkLabel1.Text
            End Get
            Set(ByVal value As String)
                Me.LinkLabel1.Text = value
            End Set
        End Property

        Public Property LinkFont() As Font
            Get
                Return Me.LinkLabel1.Font
            End Get
            Set(ByVal value As Font)
                Me.LinkLabel1.Font = value
            End Set
        End Property

        Public Property LinkFontColor() As Color
            Get
                Return Me.LinkLabel1.LinkColor
            End Get
            Set(ByVal value As Color)
                Me.LinkLabel1.LinkColor = value
                Me.LinkLabel1.VisitedLinkColor = value
            End Set
        End Property

        Private Sub LinkLabel1_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
            RaiseEvent LinkClicked(sender, e)
        End Sub

    End Class

End Namespace