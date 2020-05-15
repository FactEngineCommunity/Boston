Public Class frmGenericSelectMultiColumn

    Public zoGenericSelection As tGenericSelection

    Private Sub generic_select_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-------------------------------
        'Centre the form
        '-------------------------------
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Me.Top = (2 * (Screen.PrimaryScreen.WorkingArea.Height / 5)) - (Me.Height / 2)

        Call SetupForm()

    End Sub

    Private Sub SetupForm()

        Dim lsSQLQuery As String
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT " & zoGenericSelection.SelectField & ", " & zoGenericSelection.IndexField
        lsSQLQuery &= " FROM " & zoGenericSelection.TableName
        lsSQLQuery &= " " & zoGenericSelection.WhereClause
        If IsSomething(zoGenericSelection.OrderByFields) Then
            lsSQLQuery &= " ORDER BY " & zoGenericSelection.OrderByFields & " ASC"
        End If

        lREcordset.Open(lsSQLQuery)

        'If Not lREcordset.EOF Then
        '    While Not lREcordset.EOF
        '        comboboxSelection.Items.Add(New tComboboxItem(lREcordset(zoGenericSelection.IndexField).Value, lREcordset(0).Value))
        '        lREcordset.MoveNext()
        '    End While
        'End If

        Dim lasFields() As String

        If zoGenericSelection.FieldList <> "" Then
            lasFields = Viev.Strings.RemoveWhiteSpace(zoGenericSelection.FieldList).Split(",")
        Else
            lasFields = Viev.Strings.RemoveWhiteSpace(zoGenericSelection.SelectField).Split(",")
        End If


        '=================================================================================================================
        Dim dtLoading As New DataTable("UsStates")

        For Each lsField In lasFields
            dtLoading.Columns.Add(lsField, System.Type.GetType("System.String"))
        Next

        While Not lREcordset.EOF
            Dim dr As DataRow
            dr = dtLoading.NewRow

            For Each lsField In lasFields
                dr(lsField) = lREcordset(lsField).Value
            Next

            dtLoading.Rows.Add(dr)
            lREcordset.MoveNext()
        End While

        comboboxSelection.ColumnNum = lasFields.Count
        comboboxSelection.ColumnWidth = Me.zoGenericSelection.ColumnWidthString
        comboboxSelection.SelectedIndex = -1
        comboboxSelection.Items.Clear()
        comboboxSelection.LoadingType = MTGCComboBox.CaricamentoCombo.DataTable
        comboboxSelection.SourceDataString = New String(lasFields.Count - 1) {}
        Dim liInd As Integer = 0
        For Each lsField In lasFields
            comboboxSelection.SourceDataString.SetValue(lsField, liInd)
            liInd += 1
        Next
        comboboxSelection.SourceDataTable = dtLoading
        '=================================================================================================================

        If Me.zoGenericSelection.TupleList.Count > 0 Then
            Dim lr_object As tComboboxItem
            For Each lr_object In Me.zoGenericSelection.TupleList
                Me.comboboxSelection.Items.Insert(0, lr_object)
            Next
        End If

        comboboxSelection.SelectedIndex = 0

        'groupbox_main.Text = "Select " & zoGenericSelection.select_field
        Me.Text = "Select " & zoGenericSelection.FormTitle

        lREcordset.Close()

        Me.comboboxSelection.Select()


    End Sub

    Private Function CheckFields() As Boolean

        Dim lbFieldsOk As Boolean = False

        If Me.comboboxSelection.Text = "" Then
        Else
            lbFieldsOk = True
        End If

        Return lbFieldsOk

    End Function


    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click

        If Me.CheckFields() Then
            Select Case Me.comboboxSelection.DropDownStyle
                Case Is = ComboBoxStyle.DropDownList
                    If IsSomething(Me.comboboxSelection.SelectedItem) Then
                        Me.zoGenericSelection.SelectIndex = comboboxSelection.SelectedIndex
                    Else
                        Throw New Exception("No item selected, frmGenericSelect")
                    End If
                Case Is = ComboBoxStyle.DropDown
                    Me.zoGenericSelection.SelectIndex = Me.comboboxSelection.SelectedIndex
                Case Is = ComboBoxStyle.Simple
                    Me.zoGenericSelection.SelectIndex = Me.comboboxSelection.SelectedIndex
            End Select

            Select Case Me.zoGenericSelection.SelectColumn
                Case Is = 1
                    Me.zoGenericSelection.SelectValue = Trim(comboboxSelection.SelectedItem.Col1)
                Case Is = 2
                    Me.zoGenericSelection.SelectValue = Trim(comboboxSelection.SelectedItem.Col2)
                Case Is = 3
                    Me.zoGenericSelection.SelectValue = Trim(comboboxSelection.SelectedItem.Col3)
                Case Is = 4
                    Me.zoGenericSelection.SelectValue = Trim(comboboxSelection.SelectedItem.Col4)
            End Select

            Me.Hide()
            Me.Close()
            Me.Dispose()
        End If

    End Sub
End Class