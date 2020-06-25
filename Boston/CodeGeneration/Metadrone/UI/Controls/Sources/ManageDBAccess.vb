Imports Boston.Tools

Namespace UI

    Friend Class ManageDBAccess
        Implements PluginInterface.Sources.IManageSource

        Public Shadows Event ValueChanged(ByVal value As Object) Implements PluginInterface.Sources.IManageSource.ValueChanged
        Public Event Save() Implements PluginInterface.Sources.IManageSource.Save

        Public Sub Setup() Implements PluginInterface.Sources.IManageSource.Setup
            If Not Me.UseOleDBSchemaGet And Me.txtTableSchemaQuery.Text.Length = 0 Then
                Me.txtTableSchemaQuery.Text = New PluginInterface.Sources.Access("", "").GetQuery(PluginInterface.Sources.Access.QueryEnum.TableQuery)
            End If
            Me.splitMain.Panel2Collapsed = True
        End Sub

        Private Sub TestConn()
            Dim conn As New PluginInterface.Sources.Access("", Me.txtConnectionString.Text)
            conn.TestConnection()
        End Sub


#Region "Properties"

        Public Property ConnectionString() As String Implements PluginInterface.Sources.IManageSource.ConnectionString
            Get
                Return Me.txtConnectionString.Text
            End Get
            Set(ByVal value As String)
                Me.txtConnectionString.Text = value
            End Set
        End Property

        Public Property SingleResultApproach() As Boolean Implements PluginInterface.Sources.IManageSource.SingleResultApproach
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)

            End Set
        End Property

        Public Property TableSchemaGeneric() As Boolean Implements PluginInterface.Sources.IManageSource.TableSchemaGeneric
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)

            End Set
        End Property

        Public Property ColumnSchemaGeneric() As Boolean Implements PluginInterface.Sources.IManageSource.ColumnSchemaGeneric
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)

            End Set
        End Property

        Public Property Access2K() As Boolean
            Get
                Return Me.rbAccess2K.Checked
            End Get
            Set(ByVal value As Boolean)
                Me.rbAccess2K.Checked = value
                Me.rbAccess.Checked = Not value
            End Set
        End Property

        Public Property UseOleDBSchemaGet() As Boolean
            Get
                Return Me.rbGeneric.Checked
            End Get
            Set(ByVal value As Boolean)
                Me.rbGeneric.Checked = value
                Me.rbQuery.Checked = Not value
            End Set
        End Property

        Public Property SchemaQuery() As String Implements PluginInterface.Sources.IManageSource.SchemaQuery
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property TableSchemaQuery() As String Implements PluginInterface.Sources.IManageSource.TableSchemaQuery
            Get
                If Me.UseOleDBSchemaGet Then
                    Return Nothing
                Else
                    Return Me.txtTableSchemaQuery.Text
                End If
            End Get
            Set(ByVal value As String)
                Me.txtTableSchemaQuery.Text = value
                Me.UseOleDBSchemaGet = Conv.ToString(value, True).Length = 0
            End Set
        End Property

        Public Property ColumnSchemaQuery() As String Implements PluginInterface.Sources.IManageSource.ColumnSchemaQuery
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property TableName() As String Implements PluginInterface.Sources.IManageSource.TableName
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property RoutineSchemaQuery() As String Implements PluginInterface.Sources.IManageSource.RoutineSchemaQuery
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property Transformations() As String Implements PluginInterface.Sources.IManageSource.Transformations
            Get
                Return Me.txtTransformations.Text
            End Get
            Set(ByVal value As String)
                Me.txtTransformations.Text = value
            End Set
        End Property

#End Region

        Private Sub BuildConnectionString()
            If Me.Access2K Then
                Me.txtConnectionString.Text = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Me.txtFile.Text & ";"
            Else
                Me.txtConnectionString.Text = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Me.txtFile.Text & ";Persist Security Info=True;"
            End If
        End Sub

        Private Sub btnTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTest.Click
            Try
                Me.Cursor = Cursors.WaitCursor

                Call Me.TestConn()

                Me.Cursor = Cursors.Default
                Call MessageBox.Show("Test connection successful.", "Metadrone", _
                                     MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

            Catch ex As Exception
                Me.Cursor = Cursors.Default
                Call MessageBox.Show(ex.Message, "Metadrone", _
                                     MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)

            End Try
        End Sub

        Private Sub txtConnectionString_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConnectionString.TextChanged
            RaiseEvent ValueChanged(CType(sender, TextBox).Text)
        End Sub

        Private Sub txtFile_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFile.TextChanged
            Call Me.BuildConnectionString()
        End Sub

        Private Sub SavePress() Handles txtTableSchemaQuery.SavePress, txtTransformations.SavePress
            RaiseEvent Save()
        End Sub

        Private Sub txtTableSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTableSchemaQuery.TextChanged
            RaiseEvent ValueChanged(txtTableSchemaQuery.Text)
        End Sub

        Private Sub rbAccess_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAccess.CheckedChanged, rbAccess2K.CheckedChanged
            Call Me.BuildConnectionString()
        End Sub

        Private Sub txtTransformations_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTransformations.TextChanged
            RaiseEvent ValueChanged(Me.txtTransformations.Text)
        End Sub

        Private Sub btnFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFile.Click
            Dim dlg As New System.Windows.Forms.OpenFileDialog()
            dlg.Filter = "Access Files (*.mdb, *.accdb)|*.mdb;*.accdb|All Files|*.*"
            If dlg.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                Me.txtFile.Text = dlg.FileName
            End If
        End Sub

        Private Sub rbGeneric_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbGeneric.CheckedChanged, rbQuery.CheckedChanged
            Me.pnlQuery.Visible = Me.rbQuery.Checked
            If Me.rbQuery.Checked Then
                If Me.txtTableSchemaQuery.Text.Length = 0 Then
                    Me.txtTableSchemaQuery.Text = New PluginInterface.Sources.Access("", "").GetQuery(PluginInterface.Sources.Access.QueryEnum.TableQuery)
                End If
            Else
                Me.splitMain.Panel2Collapsed = True
                Me.txtTableSchemaQuery.Text = ""
            End If
        End Sub


#Region "Query Testing"

        Private Sub lnkPreview_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkPreviewSchema.LinkClicked
            Call Me.TestQuery()
        End Sub

        Private Sub txtTableSchemaQuery_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTableSchemaQuery.KeyDown
            If e.KeyCode = Keys.F5 Then
                Me.TestQuery()
                Me.txtTableSchemaQuery.Focus()
            End If
        End Sub

        Private Sub TestQuery()
            Me.splitMain.Panel2Collapsed = False

            Me.Cursor = Cursors.WaitCursor

            Try
                Me.QueryResults.PrepareSourceLoad()
                Dim dt As New PluginInterface.Sources.Access("", Me.txtConnectionString.Text)
                Me.QueryResults.SetSource(dt.TestQuery(Me.TableSchemaQuery))

            Catch ex As Exception
                Me.QueryResults.Messages = ex.Message

            End Try

            Me.Cursor = Cursors.Default
        End Sub

#End Region


    End Class

End Namespace