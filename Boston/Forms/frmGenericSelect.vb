Imports System.Linq.Expressions
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SetupForm()

        Dim lsSQLQuery As String
        Dim lREcordset As New RecordsetProxy

        Try
            Select Case zoGenericSelection.Type
                Case Is = pcenumGenericSelectionType.SelectFromList
#Region "Select From List"
                    If Me.zoGenericSelection.TupleList.Count > 0 Then
                        Dim lr_object As tComboboxItem
                        For Each lr_object In Me.zoGenericSelection.TupleList
                            Me.combobox_selection.Items.Insert(0, lr_object)
                        Next
                    End If

                    Me.combobox_selection.DropDownStyle = Me.zoGenericSelection.ComboBoxStyle
                    Select Case Me.combobox_selection.DropDownStyle
                        Case Is = pcenumComboBoxStyle.DropdownList
                            Me.combobox_selection.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                            Me.combobox_selection.AutoCompleteSource = AutoCompleteSource.ListItems
                    End Select
#End Region
                Case Is = pcenumGenericSelectionType.SelectFromDatabase
#Region "Select From Database"

                    If Me.zoGenericSelection.DatabaseConnection Is Nothing Then
                        lREcordset.ActiveConnection = pdbConnection
                    Else
                        lREcordset.ActiveConnection = Me.zoGenericSelection.DatabaseConnection
                    End If
                    lREcordset.CursorType = pcOpenStatic

                    If Me.zoGenericSelection.UseDataStore Then
                        ''JSON In DataStore table in the database
                        'Dim lrDataStore As New DataStore.Store
                        'Dim ltType As Type = Me.zoGenericSelection.DataStoreType
                        '' Using reflection to call the generic Get method with a runtime type
                        ''Dim methodInfo = lrDataStore.GetType().GetMethod("Get", BindingFlags.Public Or BindingFlags.Instance)
                        'Dim methodInfo = lrDataStore.GetType().GetMethod(
                        '                                                "Get",
                        '                                                BindingFlags.Public Or BindingFlags.Instance,
                        '                                                Nothing,
                        '                                                New Type() {GetType(Expression(Of Func(Of Object, Boolean))), GetType(String).MakeByRefType()},
                        '                                                Nothing)
                        'Dim genericMethod = methodInfo.MakeGenericMethod(ltType)

                        '' Prepare the parameters for the Get method
                        '' Assuming you want to pass Nothing for both parameters
                        'Dim whereClause As Expression(Of Func(Of Object, Boolean)) = Nothing ' Adjust the type inside Func as needed
                        'Dim asID As String = Nothing

                        '' The parameters array must match the method signature
                        'Dim parameters() As Object = {whereClause, asID}

                        '' Invoke the method with the parameters
                        'Dim larDataStoreJSONObject = genericMethod.Invoke(lrDataStore, parameters)

                        'JSON In DataStore table in the database
                        Dim lrDataStore As New DataStore.Store
                        Dim ltType As Type = Me.zoGenericSelection.DataStoreType

                        ' Find the 2-parameter generic Get(Of T)(..., ByRef asID)
                        Dim methodInfo As MethodInfo = lrDataStore.GetType().
                                                            GetMethods(BindingFlags.Public Or BindingFlags.Instance).
                                                            Where(Function(m) m.Name = "Get" AndAlso m.IsGenericMethodDefinition).
                                                            First(Function(m) m.GetParameters().Length = 2)

                        ' Close it with the runtime T
                        Dim genericMethod As MethodInfo = methodInfo.MakeGenericMethod(ltType)

                        ' Prepare parameters (Nothing for whereClause; ByRef slot for asID)
                        Dim asID As String = Nothing
                        Dim parameters() As Object = {Nothing, asID}

                        ' Invoke
                        Dim larDataStoreJSONObject As Object = genericMethod.Invoke(lrDataStore, parameters)


                        If larDataStoreJSONObject.Count = 0 Then

                            Boston.ShowFlashCard("There is not instance of " & zoGenericSelection.ObjectName & " to edit.", Color.LightGray)

                            Me.DialogResult = DialogResult.Cancel

                            Me.Hide()
                            Me.Close()
                            Me.Dispose()

                        End If

                        For Each loObject In larDataStoreJSONObject

                            Dim IndexFieldInfo = loObject.GetType().GetField(zoGenericSelection.IndexField)
                            Dim IndexFieldValue As Object
                            Dim SelectFieldValue As Object
                            If IndexFieldInfo IsNot Nothing Then
                                ' If it's a field, get its value
                                IndexFieldValue = IndexFieldInfo.GetValue(loObject)
                                ' Use fieldValue as needed
                            Else
                                ' If getField returns nothing, try getProperty
                                Dim propertyInfo = loObject.GetType().GetProperty(zoGenericSelection.IndexField)
                                If propertyInfo IsNot Nothing Then
                                    ' If it's a property, get its value
                                    IndexFieldValue = propertyInfo.GetValue(loObject)
                                    ' Use propertyValue as needed
                                End If
                            End If
                            Dim SelectFieldInfo = loObject.GetType().GetField(zoGenericSelection.SelectField)
                            If SelectFieldInfo IsNot Nothing Then
                                ' If it's a field, get its value
                                SelectFieldValue = SelectFieldInfo.GetValue(loObject)
                                ' Use fieldValue as needed
                            Else
                                ' If getField returns nothing, try getProperty
                                Dim propertyInfo = loObject.GetType().GetProperty(zoGenericSelection.SelectField)
                                If propertyInfo IsNot Nothing Then
                                    ' If it's a property, get its value
                                    SelectFieldValue = propertyInfo.GetValue(loObject)
                                    ' Use propertyValue as needed
                                End If
                            End If

                            combobox_selection.Items.Add(New tComboboxItem(IndexFieldValue, SelectFieldValue, loObject))
                        Next

                        combobox_selection.Sorted = True
                    Else
#Region "Select from database"
                        'SQL for a Table in the database
                        lsSQLQuery = "SELECT " & zoGenericSelection.SelectField & ", " & zoGenericSelection.IndexField
                        lsSQLQuery &= " FROM " & zoGenericSelection.TableName
                        lsSQLQuery &= " " & zoGenericSelection.WhereClause
                        If zoGenericSelection.OrderByFields IsNot Nothing Then
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

                        combobox_selection.Sorted = True
#End Region
                    End If

                    'TupleList
                    If Me.zoGenericSelection.TupleList.Count > 0 Then
                        Dim lr_object As tComboboxItem
                        For Each lr_object In Me.zoGenericSelection.TupleList
                            Me.combobox_selection.Items.Insert(0, lr_object)
                        Next
                    End If
#End Region
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxSelection_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles combobox_selection.Validating

        Dim cmb = DirectCast(sender, ComboBox)
        If cmb.FindStringExact(cmb.Text) = -1 Then
            MessageBox.Show("Please select a valid option from the list.")
            e.Cancel = True
        End If
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function


    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click

        Try
            If Me.CheckFields() Then
                Select Case Me.combobox_selection.DropDownStyle
                    Case Is = ComboBoxStyle.DropDownList
                        If Me.combobox_selection.SelectedItem IsNot Nothing Then
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

            Me.Hide()
            Me.Close()
            Me.Dispose()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class