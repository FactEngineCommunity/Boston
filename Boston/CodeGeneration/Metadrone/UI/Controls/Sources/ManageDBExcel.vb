Namespace UI

    Friend Class ManageDBExcel
        Implements PluginInterface.Sources.IManageSource

        Public Shadows Event ValueChanged(ByVal value As Object) Implements PluginInterface.Sources.IManageSource.ValueChanged
        Public Event Save() Implements PluginInterface.Sources.IManageSource.Save

        Public Sub Setup() Implements PluginInterface.Sources.IManageSource.Setup

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

        Public Property UseOleDBSchemaGet() As Boolean
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)

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
                Return Nothing
            End Get
            Set(ByVal value As String)

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


        Public Property Excel2K() As Boolean
            Get
                Return Me.rbExcel2K.Checked
            End Get
            Set(ByVal value As Boolean)
                Me.rbExcel2K.Checked = value
                Me.rbExcel.Checked = Not value
            End Set
        End Property

        Private Sub BuildConnectionString()
            If Me.Excel2K Then
                Me.txtConnectionString.Text = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Me.txtFile.Text & ";" & _
                                              "Extended Properties=""Excel 8.0;HDR=" & IIf(Me.chkHdr.Checked, "Yes", "No").ToString & ";IMEX=1"";"
            Else
                Me.txtConnectionString.Text = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Me.txtFile.Text & ";" & _
                                              "Extended Properties=""Excel 12.0 Xml;HDR=" & IIf(Me.chkHdr.Checked, "Yes", "No").ToString & """;"
            End If
        End Sub

        Private Sub btnTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTest.Click
            Try
                Me.Cursor = Cursors.WaitCursor

                Using conn As New Data.OleDb.OleDbConnection(Me.txtConnectionString.Text)
                    conn.Open()
                    conn.Close()
                End Using

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

        Private Sub rbExcel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbExcel.CheckedChanged, rbExcel2K.CheckedChanged
            Call Me.BuildConnectionString()
        End Sub

        Private Sub chkHdr_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHdr.CheckedChanged
            Call Me.BuildConnectionString()
        End Sub

        Private Sub btnFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFile.Click
            Dim dlg As New System.Windows.Forms.OpenFileDialog()
            dlg.Filter = "Excel Files (*.xls, *.xlsx, *.xl*)|*.xls;*.xlsx;*.xl*|All Files|*.*"
            If dlg.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                Me.txtFile.Text = dlg.FileName
            End If
        End Sub

        Private Sub txtTransformations_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTransformations.TextChanged
            RaiseEvent ValueChanged(Me.txtTransformations.Text)
        End Sub

    End Class

End Namespace