Namespace UI

    Friend Class ManageDBODBC
        Implements PluginInterface.Sources.IManageSource

        Public Shadows Event ValueChanged(ByVal value As Object) Implements PluginInterface.Sources.IManageSource.ValueChanged
        Public Event Save() Implements PluginInterface.Sources.IManageSource.Save

        Public Sub Setup() Implements PluginInterface.Sources.IManageSource.Setup
            If Me.txtSchemaQuery.Text.Length = 0 Then
                Me.SingleResultApproach = False
            End If
            Me.splitMain.Panel2Collapsed = True
        End Sub

        Private Sub TestConn()
            Dim conn As New PluginInterface.Sources.ODBC("", Me.txtConnectionString.Text)
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
            If Me.rbApproachSingle.Checked Then
                Me.txtTableSchemaQuery.Text = ""
                Me.txtColumnSchemaQuery.Text = ""
                Me.txtTableName.Text = ""
            Else
                Me.splitMain.Panel2Collapsed = True

                If Not Me.rbTableQuery.Checked Then
                    Me.txtTableSchemaQuery.Text = ""
                End If
                If Me.txtTableName.Text.Length = 0 Then
                    Me.txtTableName.Text = Persistence.Source.Default_TableNamePlaceHolder
                End If
                Me.txtSchemaQuery.Text = ""
            End If
        End Sub

#End Region


#Region "Connection guff"

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
                Dim dt As New PluginInterface.Sources.ODBC("", Me.txtConnectionString.Text)
                Me.QueryResults.SetSource(dt.TestQuery(Me.SchemaQuery))

            Catch ex As Exception
                Me.QueryResults.Messages = ex.Message

            End Try

            Me.Cursor = Cursors.Default
        End Sub

#End Region


    End Class

End Namespace