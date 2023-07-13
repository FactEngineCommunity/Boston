Imports System.Reflection

Public Class frmGenericSelect

    Public zoGenericSelection As New tGenericSelection

    Private Sub generic_select_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            '-------------------------------
            'Centre the form
            '-------------------------------
            Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
            Me.Top = (2 * (Screen.PrimaryScreen.WorkingArea.Height / 5)) - (Me.Height / 2)

            Call SetupForm()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SetupForm()

        Dim lsSQLQuery As String
        Dim lREcordset As New RecordsetProxy

        Try
            Select Case zoGenericSelection.Type
                Case Is = pcenumGenericSelectionType.SelectFromList

                    If Me.zoGenericSelection.TupleList.Count > 0 Then
                        Dim lr_object As tComboboxItem
                        For Each lr_object In Me.zoGenericSelection.TupleList
                            Me.combobox_selection.Items.Insert(0, lr_object)
                        Next
                    End If

                Case Is = pcenumGenericSelectionType.SelectFromDatabase


                    lREcordset.ActiveConnection = pdbConnection
                    lREcordset.CursorType = pcOpenStatic

                    lsSQLQuery = "SELECT " & zoGenericSelection.SelectField & ", " & zoGenericSelection.IndexField
                    lsSQLQuery &= " FROM " & zoGenericSelection.TableName
                    lsSQLQuery &= " " & zoGenericSelection.WhereClause
                    If IsSomething(zoGenericSelection.OrderByFields) Then
                        lsSQLQuery &= " ORDER BY " & zoGenericSelection.OrderByFields & " ASC"
                    End If

                    lREcordset.Open(lsSQLQuery)

                    If lREcordset.EOF Then '20230713-VM-Was RecordCount = 0
                        MsgBox("There is/are no " & Me.zoGenericSelection.FormTitle)
                        Me.Hide()
                        Me.Close()
                        Me.Dispose()
                    End If

                    If Not lREcordset.EOF Then
                        While Not lREcordset.EOF
                            combobox_selection.Items.Add(New tComboboxItem(lREcordset(zoGenericSelection.IndexField).Value, lREcordset(0).Value))
                            lREcordset.MoveNext()
                        End While
                    End If

                    lREcordset.Close()

                    'TupleList
                    If Me.zoGenericSelection.TupleList.Count > 0 Then
                        Dim lr_object As tComboboxItem
                        For Each lr_object In Me.zoGenericSelection.TupleList
                            Me.combobox_selection.Items.Insert(0, lr_object)
                        Next
                    End If

            End Select

            If combobox_selection.Items.Count > 0 Then
                combobox_selection.SelectedIndex = 0
            End If


            'groupbox_main.Text = "Select " & zoGenericSelection.select_field
            Me.Text = "Select " & zoGenericSelection.FormTitle

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function CheckFields() As Boolean

        Dim lbFieldsOk As Boolean = False

        Try

            If Me.combobox_selection.Text = "" Then
            Else
                lbFieldsOk = True
            End If

            Return lbFieldsOk

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function


    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click

        Try
            If Me.CheckFields() Then
                Select Case Me.combobox_selection.DropDownStyle
                    Case Is = ComboBoxStyle.DropDownList
                        If IsSomething(Me.combobox_selection.SelectedItem) Then
                            Me.zoGenericSelection.SelectIndex = combobox_selection.SelectedItem.itemdata
                            Me.zoGenericSelection.SelectValue = Trim(combobox_selection.SelectedItem.text)
                            Me.zoGenericSelection.SelectedTag = combobox_selection.SelectedItem.Tag
                        Else
                            Throw New Exception("No item selected, frmGenericSelect")
                        End If
                    Case Is = ComboBoxStyle.DropDown
                        Me.zoGenericSelection.SelectIndex = Me.combobox_selection.SelectedItem.itemdata() 'was .Text
                        Me.zoGenericSelection.SelectValue = Me.combobox_selection.Text
                        Me.zoGenericSelection.SelectedTag = combobox_selection.SelectedItem.Tag
                    Case Is = ComboBoxStyle.Simple
                        Me.zoGenericSelection.SelectIndex = Me.combobox_selection.Text
                        Me.zoGenericSelection.SelectValue = Trim(combobox_selection.SelectedItem.text)
                        Me.zoGenericSelection.SelectedTag = combobox_selection.SelectedItem.Tag
                End Select

                Me.DialogResult = DialogResult.OK

                Me.Hide()
                Me.Close()
                Me.Dispose()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub button_cancel_Click(sender As Object, e As EventArgs) Handles button_cancel.Click

        Try
            Me.DialogResult = DialogResult.Cancel

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class