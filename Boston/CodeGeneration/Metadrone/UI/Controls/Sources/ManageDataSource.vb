Imports Boston.Tools

Namespace UI

    Friend Class ManageSource
        Private SysIgnoreEvent As Boolean = True

        Private linkMessage As String = ""

        Public Shadows Event ValueChanged(ByVal value As Object)
        Public Event Save()

        Public Sub Initialise(ByVal Source As Persistence.Source)
            'Set up default managers
            Me.ctlSQLServer.Dock = DockStyle.Fill
            Me.ctlOracle.Dock = DockStyle.Fill
            Me.ctlAccess.Dock = DockStyle.Fill
            Me.ctlExcel.Dock = DockStyle.Fill
            Me.ctlOLEDB.Dock = DockStyle.Fill
            Me.ctlODBC.Dock = DockStyle.Fill

            'Setup plugins
            For Each plugin In Globals.SourcePlugins.Plugins
                'Add manager
                With CType(plugin.Manager, UserControl)
                    .Dock = DockStyle.Fill
                    .Visible = False
                    .Tag = plugin.SourceDescription.ProviderName
                End With
                AddHandler plugin.Manager.ValueChanged, AddressOf ctl_ValueChanged
                AddHandler plugin.Manager.Save, AddressOf ctl_Save
                Me.pnlManagers.Controls.Add(CType(plugin.Manager, UserControl))

                'Add logo and description
                Dim pic As New PictureBox()
                pic.Image = plugin.SourceDescription.LogoImage
                pic.Size = New System.Drawing.Size(32, 32)
                Dim rb As New RadioButton()
                rb.Text = plugin.SourceDescription.Description
                rb.AutoSize = True
                rb.Tag = plugin.SourceDescription.ProviderName
                AddHandler rb.CheckedChanged, AddressOf rbS_CheckedChanged

                'Find where to put it
                Dim maxTop As Integer = 1
                For i As Integer = 0 To Me.pnlProviders.Controls.Count - 1
                    If Me.pnlProviders.Controls.Item(i).Top > maxTop Then maxTop = Me.pnlProviders.Controls.Item(i).Top
                Next
                pic.Left = 13
                pic.Top = maxTop + 32 + 2
                rb.Left = 51
                rb.Top = pic.Top + 7

                Me.pnlProviders.Controls.Add(pic)
                Me.pnlProviders.Controls.Add(rb)
            Next

            'Set up which manager to use
            If Source.Provider.Equals(PluginInterface.Sources.Descriptions.SQLSERVER.ProviderName, StringComparison.CurrentCultureIgnoreCase) Then
                Me.rbSQLServer.Checked = True
                Me.ctlSQLServer.ConnectionString = Source.ConnectionString
                Me.ctlSQLServer.TableSchemaGeneric = Conv.ToString(Source.TableSchemaQuery, True).Length = 0
                Me.ctlSQLServer.SingleResultApproach = Not Conv.ToString(Source.SchemaQuery, True).Length = 0
                Me.ctlSQLServer.SchemaQuery = Source.SchemaQuery
                Me.ctlSQLServer.TableSchemaQuery = Source.TableSchemaQuery
                Me.ctlSQLServer.ColumnSchemaQuery = Source.ColumnSchemaQuery
                Me.ctlSQLServer.TableName = Source.TableNamePlaceHolder
                Me.ctlSQLServer.RoutineSchemaQuery = Source.RoutineSchemaQuery
                Me.ctlSQLServer.Transformations = Source.Transformations

            ElseIf Source.Provider.Equals(PluginInterface.Sources.Descriptions.ORACLE.ProviderName, StringComparison.CurrentCultureIgnoreCase) Then
                Me.rbOracle.Checked = True
                Me.ctlOracle.ConnectionString = Source.ConnectionString
                Me.ctlOracle.TableSchemaGeneric = Conv.ToString(Source.TableSchemaQuery, True).Length = 0
                Me.ctlOracle.SingleResultApproach = Not Conv.ToString(Source.SchemaQuery, True).Length = 0
                Me.ctlOracle.SchemaQuery = Source.SchemaQuery
                Me.ctlOracle.TableSchemaQuery = Source.TableSchemaQuery
                Me.ctlOracle.ColumnSchemaQuery = Source.ColumnSchemaQuery
                Me.ctlOracle.TableName = Source.TableNamePlaceHolder
                Me.ctlOracle.RoutineSchemaQuery = Source.RoutineSchemaQuery
                Me.ctlOracle.Transformations = Source.Transformations

            ElseIf Source.Provider.Equals(PluginInterface.Sources.Descriptions.OLEDB.ProviderName, StringComparison.CurrentCultureIgnoreCase) Then
                Me.rbOLEDB.Checked = True
                Me.ctlOLEDB.ConnectionString = Source.ConnectionString
                Me.ctlOLEDB.TableSchemaGeneric = Conv.ToString(Source.TableSchemaQuery, True).Length = 0
                Me.ctlOLEDB.ColumnSchemaGeneric = Conv.ToString(Source.ColumnSchemaQuery, True).Length = 0
                Me.ctlOLEDB.SingleResultApproach = Not Conv.ToString(Source.SchemaQuery, True).Length = 0
                Me.ctlOLEDB.SchemaQuery = Source.SchemaQuery
                Me.ctlOLEDB.TableSchemaQuery = Source.TableSchemaQuery
                Me.ctlOLEDB.ColumnSchemaQuery = Source.ColumnSchemaQuery
                Me.ctlOLEDB.TableName = Source.TableNamePlaceHolder
                Me.ctlOLEDB.RoutineSchemaQuery = Source.RoutineSchemaQuery
                Me.ctlOLEDB.Transformations = Source.Transformations

            ElseIf Source.Provider.Equals(PluginInterface.Sources.Descriptions.ODBC.ProviderName, StringComparison.CurrentCultureIgnoreCase) Then
                Me.rbODBC.Checked = True
                Me.ctlODBC.ConnectionString = Source.ConnectionString
                Me.ctlODBC.TableSchemaGeneric = Conv.ToString(Source.TableSchemaQuery, True).Length = 0
                Me.ctlODBC.SingleResultApproach = Not Conv.ToString(Source.SchemaQuery, True).Length = 0
                Me.ctlODBC.SchemaQuery = Source.SchemaQuery
                Me.ctlODBC.TableSchemaQuery = Source.TableSchemaQuery
                Me.ctlODBC.ColumnSchemaQuery = Source.ColumnSchemaQuery
                Me.ctlODBC.TableName = Source.TableNamePlaceHolder
                Me.ctlODBC.RoutineSchemaQuery = Source.RoutineSchemaQuery
                Me.ctlODBC.Transformations = Source.Transformations

            ElseIf Source.Provider.Equals(PluginInterface.Sources.Descriptions.ACCESS.ProviderName, StringComparison.CurrentCultureIgnoreCase) Or
                   Source.Provider.Equals(PluginInterface.Sources.Descriptions.ACCESS2K.ProviderName, StringComparison.CurrentCultureIgnoreCase) Then
                Me.rbAccess.Checked = True
                Me.ctlAccess.Access2K = (Source.Provider = PluginInterface.Sources.Descriptions.ACCESS2K.ProviderName)
                Me.ctlAccess.ConnectionString = Source.ConnectionString
                Me.ctlAccess.SchemaQuery = Source.SchemaQuery
                Me.ctlAccess.TableSchemaQuery = Source.TableSchemaQuery
                Me.ctlAccess.ColumnSchemaQuery = Source.ColumnSchemaQuery
                Me.ctlAccess.TableName = Source.TableNamePlaceHolder
                Me.ctlAccess.RoutineSchemaQuery = Source.RoutineSchemaQuery
                Me.ctlAccess.Transformations = Source.Transformations

            ElseIf Source.Provider.Equals(PluginInterface.Sources.Descriptions.EXCEL.ProviderName, StringComparison.CurrentCultureIgnoreCase) Or
                   Source.Provider.Equals(PluginInterface.Sources.Descriptions.EXCEL2K.ProviderName, StringComparison.CurrentCultureIgnoreCase) Then
                Me.rbExcel.Checked = True
                Me.ctlExcel.Excel2K = (Source.Provider.Equals(PluginInterface.Sources.Descriptions.EXCEL2K.ProviderName, StringComparison.CurrentCultureIgnoreCase))
                Me.ctlExcel.ConnectionString = Source.ConnectionString
                Me.ctlExcel.Transformations = Source.Transformations

            Else
                'Find plugin matching provider
                Dim pi As PluginInterface.Sources.Loader.Plugin = Globals.SourcePlugins.GetPlugin(Source.Provider)
                If pi Is Nothing Then
                    'Indicate
                    Dim sb As New System.Text.StringBuilder("No plug-in for provider '" & Source.Provider & "' was loaded. ")
                    sb.Append("The default provider is now selected.")
                    Me.linkMessage = sb.ToString
                    Me.lnkMessage.Text = "Provider could not be identified"
                    Me.pnlMessage.Visible = True

                    Call Me.ShowManager()
                    Me.SysIgnoreEvent = False
                    Me.Refresh()

                    Exit Sub
                End If

                'Check the radio button
                For Each ctl In Me.pnlProviders.Controls
                    If Not TypeOf ctl Is RadioButton Then Continue For
                    If CType(ctl, RadioButton).Tag Is Nothing Then Continue For
                    If Not Source.Provider.Equals(CType(ctl, RadioButton).Tag.ToString, StringComparison.CurrentCultureIgnoreCase) Then Continue For

                    CType(ctl, RadioButton).Checked = True
                Next

                'Set up manager control
                For Each ctl In Me.pnlManagers.Controls
                    If CType(ctl, UserControl).Tag Is Nothing Then Continue For
                    MsgBox(ctl.GetType.ToString)
                    If Not TypeOf ctl Is PluginInterface.Sources.IManageSource Then Continue For

                    If Source.Provider.Equals(CType(ctl, UserControl).Tag.ToString, StringComparison.CurrentCultureIgnoreCase) Then
                        'Now set
                        With CType(ctl, PluginInterface.Sources.IManageSource)
                            .ConnectionString = Source.ConnectionString
                            .TableSchemaGeneric = Conv.ToString(Source.TableSchemaQuery, True).Length = 0
                            .ColumnSchemaGeneric = Conv.ToString(Source.ColumnSchemaQuery, True).Length = 0
                            .SingleResultApproach = Not Conv.ToString(Source.SchemaQuery, True).Length = 0
                            .SchemaQuery = Source.SchemaQuery
                            .TableSchemaQuery = Source.TableSchemaQuery
                            .ColumnSchemaQuery = Source.ColumnSchemaQuery
                            .TableName = Source.TableNamePlaceHolder
                            .RoutineSchemaQuery = Source.RoutineSchemaQuery
                            .Transformations = Source.Transformations
                            .BostonModel = prApplication.getBostonApplicationModelById(Source.BostonModelId)
                        End With
                    End If
                Next

                pi.Connection.BostonModel = prApplication.getBostonApplicationModelById(Source.BostonModelId)
            End If

            Call Me.ShowManager()

            Me.SysIgnoreEvent = False

            Me.Refresh()
        End Sub

        Public Sub UpdateTag()
            With CType(Me.Tag, Persistence.Source)
                Select Case True
                    Case Me.rbSQLServer.Checked
                        .Provider = PluginInterface.Sources.Descriptions.SQLSERVER.ProviderName
                        .ConnectionString = Me.ctlSQLServer.ConnectionString
                        .SchemaQuery = Me.ctlSQLServer.SchemaQuery
                        .TableSchemaQuery = Me.ctlSQLServer.TableSchemaQuery
                        .ColumnSchemaQuery = Me.ctlSQLServer.ColumnSchemaQuery
                        .TableNamePlaceHolder = Me.ctlSQLServer.TableName
                        .RoutineSchemaQuery = Me.ctlSQLServer.RoutineSchemaQuery
                        .Transformations = Me.ctlSQLServer.Transformations

                    Case Me.rbOracle.Checked
                        .Provider = PluginInterface.Sources.Descriptions.ORACLE.ProviderName
                        .ConnectionString = Me.ctlOracle.ConnectionString
                        .SchemaQuery = Me.ctlOracle.SchemaQuery
                        .TableSchemaQuery = Me.ctlOracle.TableSchemaQuery
                        .ColumnSchemaQuery = Me.ctlOracle.ColumnSchemaQuery
                        .TableNamePlaceHolder = Me.ctlOracle.TableName
                        .RoutineSchemaQuery = Me.ctlOracle.RoutineSchemaQuery
                        .Transformations = Me.ctlOracle.Transformations

                    Case Me.rbAccess.Checked
                        If Me.ctlAccess.Access2K Then
                            .Provider = PluginInterface.Sources.Descriptions.ACCESS2K.ProviderName
                        Else
                            .Provider = PluginInterface.Sources.Descriptions.ACCESS.ProviderName
                        End If
                        .ConnectionString = Me.ctlAccess.ConnectionString
                        .SchemaQuery = Me.ctlAccess.SchemaQuery
                        .TableSchemaQuery = Me.ctlAccess.TableSchemaQuery
                        .ColumnSchemaQuery = Me.ctlAccess.ColumnSchemaQuery
                        .TableNamePlaceHolder = Me.ctlAccess.TableName
                        .RoutineSchemaQuery = Me.ctlAccess.RoutineSchemaQuery
                        .Transformations = Me.ctlAccess.Transformations

                    Case Me.rbExcel.Checked
                        If Me.ctlExcel.Excel2K Then
                            .Provider = PluginInterface.Sources.Descriptions.EXCEL2K.ProviderName
                        Else
                            .Provider = PluginInterface.Sources.Descriptions.EXCEL.ProviderName
                        End If
                        .ConnectionString = Me.ctlExcel.ConnectionString
                        .SchemaQuery = Nothing
                        .TableSchemaQuery = Nothing
                        .ColumnSchemaQuery = Nothing
                        .TableNamePlaceHolder = Nothing
                        .RoutineSchemaQuery = Nothing
                        .Transformations = Me.ctlExcel.Transformations

                    Case Me.rbOLEDB.Checked
                        .Provider = PluginInterface.Sources.Descriptions.OLEDB.ProviderName
                        .ConnectionString = Me.ctlOLEDB.ConnectionString
                        .SchemaQuery = Me.ctlOLEDB.SchemaQuery
                        .TableSchemaQuery = Me.ctlOLEDB.TableSchemaQuery
                        .ColumnSchemaQuery = Me.ctlOLEDB.ColumnSchemaQuery
                        .TableNamePlaceHolder = Me.ctlOLEDB.TableName
                        .RoutineSchemaQuery = Me.ctlOLEDB.RoutineSchemaQuery
                        .Transformations = Me.ctlOLEDB.Transformations

                    Case Me.rbODBC.Checked
                        .Provider = PluginInterface.Sources.Descriptions.ODBC.ProviderName
                        .ConnectionString = Me.ctlODBC.ConnectionString
                        .SchemaQuery = Me.ctlODBC.SchemaQuery
                        .TableSchemaQuery = Me.ctlODBC.TableSchemaQuery
                        .ColumnSchemaQuery = Me.ctlODBC.ColumnSchemaQuery
                        .TableNamePlaceHolder = Me.ctlODBC.TableName
                        .RoutineSchemaQuery = Me.ctlODBC.RoutineSchemaQuery
                        .Transformations = Me.ctlODBC.Transformations

                    Case Else
                        'Check plugins
                        Dim pi As PluginInterface.Sources.Loader.Plugin = Nothing
                        'Find plugin for the checked radio button
                        For Each ctl In Me.pnlProviders.Controls
                            'Skip to the checked radio button
                            If Not TypeOf ctl Is RadioButton Then Continue For
                            If CType(ctl, RadioButton).Tag Is Nothing Then Continue For
                            If Not CType(ctl, RadioButton).Checked Then Continue For

                            'Get plugin
                            pi = Globals.SourcePlugins.GetPlugin(CType(ctl, RadioButton).Tag.ToString)
                            Exit For
                        Next
                        If pi IsNot Nothing Then
                            'Find manager control
                            For Each ctl In Me.pnlManagers.Controls
                                If CType(ctl, UserControl).Tag Is Nothing Then Continue For
                                If Not TypeOf ctl Is PluginInterface.Sources.IManageSource Then Continue For

                                If CType(ctl, UserControl).Tag.ToString.Equals(pi.SourceDescription.ProviderName,
                                                                               StringComparison.CurrentCultureIgnoreCase) Then
                                    'Now set
                                    With CType(Me.Tag, Persistence.Source)
                                        .Provider = pi.SourceDescription.ProviderName
                                        .ConnectionString = CType(ctl, PluginInterface.Sources.IManageSource).ConnectionString
                                        .SchemaQuery = CType(ctl, PluginInterface.Sources.IManageSource).SchemaQuery
                                        .TableSchemaQuery = CType(ctl, PluginInterface.Sources.IManageSource).TableSchemaQuery
                                        .ColumnSchemaQuery = CType(ctl, PluginInterface.Sources.IManageSource).ColumnSchemaQuery
                                        .TableNamePlaceHolder = CType(ctl, PluginInterface.Sources.IManageSource).TableName
                                        .RoutineSchemaQuery = CType(ctl, PluginInterface.Sources.IManageSource).RoutineSchemaQuery
                                        .Transformations = CType(ctl, PluginInterface.Sources.IManageSource).Transformations
                                        If CType(ctl, PluginInterface.Sources.IManageSource).BostonModel IsNot Nothing Then
                                            .BostonModelId = CType(ctl, PluginInterface.Sources.IManageSource).BostonModel.ModelId
                                        End If
                                    End With
                                End If
                            Next
                        End If

                End Select
            End With
        End Sub

        Private Sub ShowManager()
            'Visibility of controls

            'Hide first
            For Each ctl In Me.pnlManagers.Controls
                CType(ctl, UserControl).Visible = False
            Next

            'Check for plugins
            For Each ctl In Me.pnlProviders.Controls
                If Not TypeOf ctl Is RadioButton Then Continue For
                If Not CType(ctl, RadioButton).Checked Then Continue For
                If CType(ctl, RadioButton).Tag Is Nothing Then Continue For

                Dim pi As PluginInterface.Sources.Loader.Plugin = Globals.SourcePlugins.GetPlugin(CType(ctl, RadioButton).Tag.ToString)
                If pi Is Nothing Then Continue For

                CType(pi.Manager, UserControl).Visible = True
                pi.Manager.Setup()
                Exit Sub
            Next

            'Otherwise the defaults
            Select Case True
                Case Me.rbSQLServer.Checked
                    Me.ctlSQLServer.Visible = True
                    Me.ctlSQLServer.Setup()

                Case Me.rbOracle.Checked
                    Me.ctlOracle.Visible = True
                    Me.ctlOracle.Setup()

                Case Me.rbAccess.Checked
                    Me.ctlAccess.Visible = True
                    Me.ctlAccess.Setup()

                Case Me.rbExcel.Checked
                    Me.ctlExcel.Visible = True
                    Me.ctlExcel.Setup()

                Case Me.rbOLEDB.Checked
                    Me.ctlOLEDB.Visible = True
                    Me.ctlOLEDB.Setup()

                Case Me.rbODBC.Checked
                    Me.ctlODBC.Visible = True
                    Me.ctlODBC.Setup()

            End Select
        End Sub

        Private Sub rbS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbSQLServer.CheckedChanged,
                                                                                                    rbOracle.CheckedChanged,
                                                                                                    rbAccess.CheckedChanged,
                                                                                                    rbExcel.CheckedChanged,
                                                                                                    rbOLEDB.CheckedChanged,
                                                                                                    rbODBC.CheckedChanged
            If Me.SysIgnoreEvent Then Exit Sub

            Me.SysIgnoreEvent = True

            Call Me.ShowManager()
            RaiseEvent ValueChanged(CType(sender, RadioButton).Checked)

            Me.SysIgnoreEvent = False

        End Sub

        Private Sub ctl_Save() Handles ctlSQLServer.Save, _
                                       ctlOracle.Save, _
                                       ctlAccess.Save, _
                                       ctlExcel.Save, _
                                       ctlOLEDB.Save, _
                                       ctlODBC.Save
            If Me.SysIgnoreEvent Then Exit Sub

            RaiseEvent Save()
        End Sub

        Private Sub ctl_ValueChanged(ByVal value As Object) Handles ctlSQLServer.ValueChanged,
                                                                    ctlOracle.ValueChanged,
                                                                    ctlAccess.ValueChanged,
                                                                    ctlExcel.ValueChanged,
                                                                    ctlOLEDB.ValueChanged,
                                                                    ctlODBC.ValueChanged
            If Me.SysIgnoreEvent Then Exit Sub

            RaiseEvent ValueChanged(value)
        End Sub

        Private Sub lnkMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMessage.Click
            MessageBox.Show(Me.linkMessage, "Provider Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Sub
    End Class

End Namespace