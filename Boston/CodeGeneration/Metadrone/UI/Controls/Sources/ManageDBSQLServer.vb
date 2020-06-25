Namespace UI

    Friend Class ManageDBSQLServer
        Implements PluginInterface.Sources.IManageSource

        Public Shadows Event ValueChanged(ByVal value As Object) Implements PluginInterface.Sources.IManageSource.ValueChanged
        Public Event Save() Implements PluginInterface.Sources.IManageSource.Save

        Public Sub Setup() Implements PluginInterface.Sources.IManageSource.Setup
            If Me.SchemaQuery.Length = 0 Then
                Me.SchemaQuery = New PluginInterface.Sources.SQLServer("", "").GetQuery(PluginInterface.Sources.SQLServer.QueryEnum.SchemaQuery)
            End If
            If Me.RoutineSchemaQuery.Length = 0 Then
                Me.RoutineSchemaQuery = New PluginInterface.Sources.SQLServer("", "").GetQuery(PluginInterface.Sources.SQLServer.QueryEnum.RoutineSchemaQuery)
            End If
            Me.splitMain.Panel2Collapsed = True
            If Me.txtTransformations.Text.Length = 0 Then
                Me.txtTransformations.Text = New PluginInterface.Sources.SQLServer("", "").GetTransforms()
            End If
        End Sub

        Private Sub TestConn()
            Dim conn As New PluginInterface.Sources.SQLServer("", Me.txtConnectionString.Text)
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
                Return Me.rbApproachSingle.Checked
            End Get
            Set(ByVal value As Boolean)
                Me.rbApproachSingle.Checked = value
                Me.rbApproachTableColumn.Checked = Not value
            End Set
        End Property

        Public Property TableSchemaGeneric() As Boolean Implements PluginInterface.Sources.IManageSource.TableSchemaGeneric
            Get
                Return Me.rbTableDefault.Checked
            End Get
            Set(ByVal value As Boolean)
                Me.rbTableDefault.Checked = value
                Me.rbTableQuery.Checked = Not value
            End Set
        End Property

        Public Property ColumnSchemaGeneric() As Boolean Implements PluginInterface.Sources.IManageSource.ColumnSchemaGeneric
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)

            End Set
        End Property

        Public Property SchemaQuery() As String Implements PluginInterface.Sources.IManageSource.SchemaQuery
            Get
                Return Me.txtSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                Me.txtSchemaQuery.Text = value
            End Set
        End Property

        Public Property TableSchemaQuery() As String Implements PluginInterface.Sources.IManageSource.TableSchemaQuery
            Get
                Return Me.txtTableSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                Me.txtTableSchemaQuery.Text = value
            End Set
        End Property

        Public Property ColumnSchemaQuery() As String Implements PluginInterface.Sources.IManageSource.ColumnSchemaQuery
            Get
                Return Me.txtColumnSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                Me.txtColumnSchemaQuery.Text = value
            End Set
        End Property

        Public Property TableName() As String Implements PluginInterface.Sources.IManageSource.TableName
            Get
                Return Me.txtTableName.Text
            End Get
            Set(ByVal value As String)
                Me.txtTableName.Text = value
            End Set
        End Property

        Public Property RoutineSchemaQuery() As String Implements PluginInterface.Sources.IManageSource.RoutineSchemaQuery
            Get
                Return Me.txtRoutineSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                Me.txtRoutineSchemaQuery.Text = value
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


#Region "Events"

        Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.rbApproachSingle.Checked = True
        End Sub

        Private Sub SavePress() Handles txtSchemaQuery.SavePress, txtTableSchemaQuery.SavePress, txtColumnSchemaQuery.SavePress, txtRoutineSchemaQuery.SavePress, txtTransformations.SavePress
            RaiseEvent Save()
        End Sub

        Private Sub txtSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSchemaQuery.TextChanged
            RaiseEvent ValueChanged(Me.txtSchemaQuery.Text)
        End Sub

        Private Sub txtTableSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTableSchemaQuery.TextChanged
            RaiseEvent ValueChanged(Me.txtTableSchemaQuery.Text)
        End Sub

        Private Sub txtColumnSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtColumnSchemaQuery.TextChanged
            RaiseEvent ValueChanged(Me.txtColumnSchemaQuery.Text)
        End Sub

        Private Sub txtTableName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTableName.TextChanged
            RaiseEvent ValueChanged(Me.txtTableName.Text)
        End Sub

        Private Sub txtRoutineSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRoutineSchemaQuery.TextChanged
            RaiseEvent ValueChanged(Me.txtRoutineSchemaQuery.Text)
        End Sub

        Private Sub txtTransformations_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTransformations.TextChanged
            RaiseEvent ValueChanged(Me.txtTransformations.Text)
        End Sub

        Private Sub rbMeta_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbApproachSingle.CheckedChanged, _
                                                                                                              rbApproachTableColumn.CheckedChanged, _
                                                                                                              rbTableDefault.CheckedChanged, _
                                                                                                              rbTableQuery.CheckedChanged
            Me.splitQuery.Panel1Collapsed = Not Me.rbApproachSingle.Checked
            Me.splitQuery.Panel2Collapsed = Me.rbApproachSingle.Checked
            Me.grpTableSchema.Visible = Not Me.rbApproachSingle.Checked
            Me.grpColumnSchema.Visible = Not Me.rbApproachSingle.Checked

            Me.splitTableColumn.Panel1Collapsed = Not Me.rbTableQuery.Checked

            Dim conn As New PluginInterface.Sources.SQLServer("", "")
            If Me.rbApproachSingle.Checked Then
                If Me.SchemaQuery.Length = 0 Then Me.SchemaQuery = conn.GetQuery(PluginInterface.Sources.SQLServer.QueryEnum.SchemaQuery)
                Me.txtTableSchemaQuery.Text = ""
                Me.txtColumnSchemaQuery.Text = ""
                Me.txtTableName.Text = ""
            Else
                Me.splitMain.Panel2Collapsed = True

                If Me.rbTableQuery.Checked Then
                    If Me.txtTableSchemaQuery.Text.Length = 0 Then Me.txtTableSchemaQuery.Text = conn.GetQuery(PluginInterface.Sources.SQLServer.QueryEnum.TableQuery)
                Else
                    Me.txtTableSchemaQuery.Text = ""
                End If
                If Me.txtColumnSchemaQuery.Text.Length = 0 Then Me.txtColumnSchemaQuery.Text = conn.GetQuery(PluginInterface.Sources.SQLServer.QueryEnum.ColumnQuery)
                If Me.txtTableName.Text.Length = 0 Then
                    Me.txtTableName.Text = Persistence.Source.Default_TableNamePlaceHolder
                End If
                Me.SchemaQuery = ""
            End If
        End Sub

#End Region


#Region "Connection guff"

        Private Sub BuildConnectionString()
            Me.txtConnectionString.Text = "Data Source=" & Me.cboServer.Text & ";Initial Catalog=" & Me.cboDatabase.Text & ";"
            If Me.rbIntegrated.Checked Then
                Me.txtConnectionString.Text &= "Integrated Security=True;"
            Else
                Me.txtConnectionString.Text &= "User Id=" & Me.txtUsername.Text & ";Password=" & Me.txtPassword.Text & ";"
            End If
        End Sub

        Private Sub cboServer_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboServer.DropDown
            If Me.cboServer.Items.Count > 0 Then Exit Sub

            Try
                Me.Cursor = Cursors.WaitCursor
                Me.cboServer.Items.Clear()
                Dim servers As System.Data.Sql.SqlDataSourceEnumerator = System.Data.Sql.SqlDataSourceEnumerator.Instance()
                For Each dr As DataRow In servers.GetDataSources.Rows
                    Dim strServer As String = dr.Item(0).ToString
                    If dr.ItemArray.Count > 1 AndAlso Not dr.Item(1).ToString.Length = 0 Then
                        strServer &= "\" & dr.Item(1).ToString
                    End If
                    Me.cboServer.Items.Add(strServer)
                Next

            Catch ex As Exception

            Finally
                Me.Cursor = Cursors.Default
                Me.Refresh()
                Me.Focus()
                Me.cboServer.Focus()

            End Try
        End Sub

        Private Sub cboSQLServer_Database_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDatabase.DropDown
            If Me.cboDatabase.Items.Count > 0 Then Exit Sub

            Try
                Me.Cursor = Cursors.WaitCursor
                Dim ConnStr As String = "Data Source=" & Me.cboServer.Text & ";Initial Catalog=master;"
                If Me.rbIntegrated.Checked Then
                    ConnStr &= "Integrated Security=True;"
                Else
                    ConnStr &= "User Id=" & Me.txtUsername.Text & ";Password=" & Me.txtPassword.Text & ";"
                End If
                Using conn As New SqlClient.SqlConnection(ConnStr)
                    conn.Open()

                    Using cmd As New SqlClient.SqlCommand()
                        cmd.Connection = conn
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.CommandText = "sp_helpdb"

                        Using da As New SqlClient.SqlDataAdapter(cmd)
                            Dim dt As New DataTable()
                            da.Fill(dt)
                            For Each dr As DataRow In dt.Rows
                                Me.cboDatabase.Items.Add(dr("Name").ToString)
                            Next
                        End Using

                    End Using

                    conn.Close()
                End Using

            Catch ex As Exception

            Finally
                Me.Cursor = Cursors.Default
                Me.Refresh()
                Me.Focus()
                Me.cboDatabase.Focus()

            End Try
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

        Private Sub cboServer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboServer.SelectedIndexChanged
            Me.cboDatabase.Items.Clear()
        End Sub

        Private Sub cbo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboServer.TextChanged, cboDatabase.TextChanged
            Call Me.BuildConnectionString()
        End Sub

        Private Sub rb_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbIntegrated.CheckedChanged, rbSQLAuth.CheckedChanged
            Call Me.BuildConnectionString()
        End Sub

        Private Sub txt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUsername.TextChanged, txtPassword.TextChanged
            Call Me.BuildConnectionString()
        End Sub

        Private Sub txtConnectionString_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConnectionString.TextChanged
            RaiseEvent ValueChanged(CType(sender, TextBox).Text)
        End Sub

#End Region


#Region "Query Testing"

        Private Sub lnkPreview_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkPreviewSchema.LinkClicked
            Call Me.TestQuery()
        End Sub

        Private Sub txtSchemaQuery_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSchemaQuery.KeyDown
            If e.KeyCode = Keys.F5 Then
                Me.TestQuery()
                Me.txtSchemaQuery.Focus()
            End If
        End Sub

        Private Sub TestQuery()
            Me.splitMain.Panel2Collapsed = False

            Me.Cursor = Cursors.WaitCursor

            Try
                Me.QueryResults.PrepareSourceLoad()
                Dim dt As New PluginInterface.Sources.SQLServer("", Me.txtConnectionString.Text)
                Me.QueryResults.SetSource(dt.TestQuery(Me.SchemaQuery))

            Catch ex As Exception
                Me.QueryResults.Messages = ex.Message

            End Try

            Me.Cursor = Cursors.Default
        End Sub

#End Region


#Region "Routine Query Testing"

        Private Sub lnkPreviewRoutineSchema_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPreviewRoutineSchema.Click
            Call Me.TestRoutineQuery()
        End Sub

        Private Sub txtRoutineSchemaQuery_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRoutineSchemaQuery.KeyDown
            If e.KeyCode = Keys.F5 Then
                Me.TestRoutineQuery()
                Me.txtRoutineSchemaQuery.Focus()
            End If
        End Sub

        Private Sub TestRoutineQuery()
            Me.splitRoutine.Panel2Collapsed = False

            Me.Cursor = Cursors.WaitCursor

            Try
                Me.RoutineQueryResults.PrepareSourceLoad()
                Dim dt As New PluginInterface.Sources.SQLServer("", Me.txtConnectionString.Text)
                Me.RoutineQueryResults.SetSource(dt.TestQuery(Me.RoutineSchemaQuery))

            Catch ex As Exception
                Me.RoutineQueryResults.Messages = ex.Message

            End Try

            Me.Cursor = Cursors.Default
        End Sub

#End Region


    End Class

End Namespace