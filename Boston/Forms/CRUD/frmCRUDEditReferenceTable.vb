Imports System.Reflection
Imports System.Xml.Serialization
Imports System.IO

Public Class frmCRUDEditReferenceTable

    Private cManager As CurrencyManager
    Private zo_working_class As Object
    Private zlo_display_list As New List(Of Object)
    Private zb_grid_data_dirty As Boolean = False 'True if a user has modified any cell contents.
    Private mrReferenceTable As New ReferenceTable

    Dim zls_field_list As New List(Of Object)

    Private Sub frm_edit_reference_table_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Dim ls_message As String = ""
        If Me.zb_grid_data_dirty Then
            ls_message = "Do you want to save the changes to the Configuration Data?"
            ls_message &= vbCrLf & vbCrLf
            ls_message &= "Press [Ok] to save changes, or [Cancel] to close this form without saving changes."

            If MsgBox(ls_message, MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Call save_data_grid_items()
            End If
        End If

        Me.ComboBox1.Items.Clear()
        Me.DataGridView1.DataSource = Nothing

    End Sub

    Private Sub frm_edit_reference_table_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetupForm()

    End Sub

    Sub SetupForm()

        Call load_reference_tables()

    End Sub

    Sub load_reference_tables()

        Dim liInd As Integer = 0
        Dim lo_reference_table As ReferenceTable
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= "  FROM ReferenceTable"
            lsSQLQuery &= " WHERE system = " & False
            lsSQLQuery &= " ORDER BY reference_table_name"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    liInd += 1
                    lo_reference_table = New ReferenceTable
                    lo_reference_table.ReferenceTableId = lREcordset("reference_table_id").Value
                    lo_reference_table.name = lREcordset("reference_table_name").Value
                    Me.ComboBox1.Items.Add(New tComboboxItem(liInd, lREcordset("reference_table_name").Value, lo_reference_table))
                    lREcordset.MoveNext()
                End While
            End If
            ComboBox1.SelectedIndex = 0

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Sub populate_data_grid_for_selected_reference_table()

        Try
            '--------------------------------------------
            'Get the list of Tuples as a List(Of Object)
            '--------------------------------------------
            zlo_display_list = TableReferenceFieldValue.GetReferenceFieldValueTuples(ComboBox1.SelectedItem.tag.ReferenceTableId, Me.zo_working_class, mrReferenceTable)

            '---------------------------------
            'Bind the tuples to the DataGrid
            '---------------------------------       
            DataGridView1.DataSource = Nothing
            DataGridView1.DataSource = zlo_display_list 'lrObjectList ' New DefensiveDatasource(zlo_display_list, Nothing)

            cManager = CType(DataGridView1.BindingContext(zlo_display_list), CurrencyManager)

            If zlo_display_list.Count > 0 Then
                Me.DataGridView1.Columns(0).Visible = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub Button_add_new_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_add_new.Click

        Dim loObject As New Object

        loObject = zo_working_class.clone
        loObject.row_id = System.Guid.NewGuid.ToString

        If zlo_display_list.Count = 0 Then
            zlo_display_list = New List(Of Object)
        End If

        zlo_display_list.Add(loObject)

        '---------------------------------
        'Bind the tuples to the DataGrid
        '---------------------------------               
        DataGridView1.DataSource = Nothing
        DataGridView1.DataSource = zlo_display_list
        cManager = CType(DataGridView1.BindingContext(zlo_display_list), CurrencyManager)

        Me.DataGridView1.Columns(0).Visible = False

        '-----------------------------------
        'Clear the values in the new tuple
        '-----------------------------------
        Dim lo_row As DataGridViewRow
        Dim liInd As Integer = 1

        lo_row = Me.DataGridView1.Rows.Item(Me.DataGridView1.Rows.Count - 1)

        For liInd = 1 To lo_row.Cells.Count - 1
            lo_row.Cells.Item(liInd).Value = ""
        Next

    End Sub

    Function TupleToListOfString(ByVal aoTuple As tTuple) As Object

        Dim loObject As New List(Of String)

        loObject = aoTuple.AttributeList

        Return loObject

    End Function

    Private Sub Button_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_delete.Click

        Dim loObject As New Object
        Dim lo_row As DataGridViewRow
        Dim ls_attribute_value As String = ""
        Dim liInd As Integer = 1
        Dim ls_message As String
        Dim li_selected_row_count As Integer = 0

        If IsSomething(DataGridView1.CurrentRow.DataBoundItem) Then

            loObject = DataGridView1.CurrentRow.DataBoundItem.clone

            li_selected_row_count = Me.DataGridView1.SelectedRows.Count
            If li_selected_row_count = 0 Then li_selected_row_count = 1
            '-------------------------------------------------------------
            'Delete the respective ReferenceFieldValue tuples within the 
            '  ReferenceFieldValue table in the database.
            '-------------------------------------------------------------
            If li_selected_row_count > 1 Then
                ls_message = "Please select one row at a time for deletion."
                MsgBox(ls_message)
                Exit Sub
            End If

            ls_message = "You are about to delete " & CStr(li_selected_row_count) & " row/s. This operation is permanent and cannot be undone."
            ls_message &= vbCrLf & vbCrLf
            ls_message &= "Select OK to confirm that you want to delete these rows, or [Cancel] if you do not wish to delete the rows."

            If MsgBox(ls_message, MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                Exit Sub
            End If

            For Each lo_row In Me.DataGridView1.SelectedRows
                loObject = lo_row.DataBoundItem
                prReferenceFieldValue = New tReferenceFieldValue
                prReferenceFieldValue.RowId = loObject.row_id

                For liInd = 1 To lo_row.Cells.Count - 1
                    prReferenceFieldValue.Data = lo_row.Cells.Item(liInd).Value
                    prReferenceFieldValue.ReferenceFieldId = liInd
                    prReferenceFieldValue.ReferenceTableId = ComboBox1.SelectedItem.tag.ReferenceTableId
                    prReferenceFieldValue.delete()
                Next
            Next

            zlo_display_list.Remove(DataGridView1.CurrentRow.DataBoundItem)
            Me.DataGridView1.DataSource = Nothing
            Me.DataGridView1.DataSource = zlo_display_list

            If Me.DataGridView1.RowCount > 0 Then
                Me.DataGridView1.Columns(0).Visible = False
            End If
        End If

    End Sub

    Private Sub button_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_save.Click

        Call Save_data_grid_items()

    End Sub

    Private Sub DataGridView1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit

        Me.zb_grid_data_dirty = True

    End Sub

    Private Sub DataGridView1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEnter

        Button_delete.Enabled = False
    End Sub

    Private Sub DataGridView1_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.RowHeaderMouseClick

        Button_delete.Enabled = True

    End Sub

    Sub Save_data_grid_items()

        Dim lo_row As DataGridViewRow
        Dim loObject As New Object
        Dim ls_attribute_value As String = ""
        Dim liInd As Integer = 1

        For Each lo_row In Me.DataGridView1.Rows

            loObject = lo_row.DataBoundItem
            prReferenceFieldValue = New tReferenceFieldValue
            prReferenceFieldValue.RowId = loObject.row_id

            For liInd = 1 To lo_row.Cells.Count - 1
                prReferenceFieldValue.Data = CStr(lo_row.Cells.Item(liInd).Value).Replace("'", "''")
                prReferenceFieldValue.ReferenceFieldId = liInd
                prReferenceFieldValue.ReferenceTableId = ComboBox1.SelectedItem.tag.ReferenceTableId
                prReferenceFieldValue.save()
            Next
        Next

        Me.zb_grid_data_dirty = False

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        Call Me.Populate_data_grid_for_selected_reference_table()

    End Sub

    Private Sub ButtonExportConfigurationItems_Click(sender As Object, e As EventArgs) Handles ButtonExportConfigurationItems.Click

        Dim lsFileLocationName As String
        Dim loStreamWriter As StreamWriter ' Create file by FileStream class
        Dim loXMLSerialiser As XmlSerializer ' Create binary object

        Try
            'CodeSafe
            If mrReferenceTable Is Nothing Then
                MsgBox("Select a coniguration item before selecting this option.")
                Exit Sub
            End If

            Dim lrSaveFileDialog As New SaveFileDialog()

            Dim lsFileName = mrReferenceTable.Name & ".XML"

            lrSaveFileDialog.Filter = "XML file (*.XML)|*.XML"
            lrSaveFileDialog.FilterIndex = 0
            lrSaveFileDialog.RestoreDirectory = True
            lrSaveFileDialog.FileName = lsFileName

            If lrSaveFileDialog.ShowDialog() = DialogResult.OK Then
                lsFileLocationName = lrSaveFileDialog.FileName
            Else
                Exit Sub
            End If

            loStreamWriter = New StreamWriter(lsFileLocationName) 'lsFolderLocation & "\" & lsFileName)

            'loXMLSerialiser = New XmlSerializer(GetType(FBM.tORMModel))
            loXMLSerialiser = New XmlSerializer(GetType(ReferenceTable))

            'Serialize object to file
            If Boston.IsSerializable(mrReferenceTable) Then

                loXMLSerialiser.Serialize(loStreamWriter, mrReferenceTable)
                loStreamWriter.Close()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonDeleteReferenceTable_Click(sender As Object, e As EventArgs) Handles ButtonDeleteReferenceTable.Click

        Dim lsMessage As String

        Try
            lsMessage = "Are you sure that you want to permanently delete the Reference Table, " & Me.ComboBox1.SelectedItem.Tag.Name

            If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                Call TableReferenceTable.DeleteReferenceTable(Me.ComboBox1.SelectedItem.Tag.ReferenceTableId)
                Call tableReferenceField.DeleteReferenceFieldsForReferenceTableById(Me.ComboBox1.SelectedItem.Tag.ReferenceTableId)
                Call TableReferenceFieldValue.DeleteReferenceFieldValuesForReferenceTableById(Me.ComboBox1.SelectedItem.Tag.ReferenceTableId)

            End If

        Catch ex As Exception

            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class