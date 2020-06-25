Namespace UI

    Friend Class StartPageRecent

        Public Event OpenRecent(ByVal path As String)

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

        End Sub

        Public Sub New(ByVal linkText As String, ByVal linkPath As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.lnkRecent.Text = linkText
            Me.lblPath.Text = linkPath
        End Sub

        Private Sub lnkRecent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRecent.Click
            RaiseEvent OpenRecent(Me.lblPath.Text)
        End Sub

    End Class

End Namespace
