Imports System.Reflection

Public Class frmCRUDRoleInstance

    Public zrRole As FBM.Role
    Public zrPage As FBM.Page

    ''' <summary>
    ''' The FactType being extended if the Role shape was dropped on a Role of an existing FactType.
    ''' </summary>
    ''' <remarks></remarks>
    Public zrBaseFactType As FBM.FactType

    ''' <summary>
    ''' Set to True if extending an existing FactType.
    ''' </summary>
    ''' <remarks></remarks>
    Public zbExtendingExistingFactType As Boolean = False


    Public Overloads Function showdialog(ByRef arRole As FBM.Role, ByRef arPage As FBM.Page) As Windows.Forms.DialogResult

        Me.zrRole = arRole
        Me.zrPage = arPage

        Return MyBase.ShowDialog()

    End Function

    Private Sub role_instance_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        SetupForm()

    End Sub

    Sub SetupForm()

        Dim lsFactTypeName As String = ""

        If Me.zbExtendingExistingFactType Then
            Me.LabelCreatingNewOrExtending.Text = "Extending existing Fact Type:"
        Else
            Me.LabelCreatingNewOrExtending.Text = "Creating a new Fact Type:"
        End If

        If Me.zrPage.EntityTypeInstance.Count > 0 Then
            Call load_entity_types()
        ElseIf Me.zrPage.ValueTypeInstance.Count > 0 Then
            Call Me.load_value_types()
        Else
            Call Me.load_nested_FactTypes()
        End If

        If Me.zbExtendingExistingFactType Then
            lsFactTypeName = Me.zrBaseFactType.Name
            lsFactTypeName &= ComboBox_join_object.SelectedItem.text
            Me.TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
        Else
            lsFactTypeName = ComboBox_join_object.SelectedItem.text
            Me.TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
        End If

        TextBox_RoleName.Text = Trim(Me.zrRole.Name)
        TextBox_RoleName.Select()

        If Me.zrPage.ValueTypeInstance.Count = 0 Then
            Me.RadioButton_value_type.Enabled = False
        End If

        '---------------------------------------------------
        'Get the count of ObjectifiedFactTypes on the Page
        '---------------------------------------------------
        Dim orderCounts = (From FactTypeInstance In Me.zrPage.FactTypeInstance _
                          Where FactTypeInstance.IsObjectified = True _
                          Select FactTypeInstance).Count()

        If orderCounts = 0 Then
            Me.RadioButton_nested_FactType.Enabled = False
        End If

    End Sub

    Sub load_entity_types()

        Dim liInd As Integer = 0
        Dim liInd2 As Integer = 0
        Dim lrEntityType As FBM.EntityType

        Me.ComboBox_join_object.Items.Clear()

        For Each lrEntityType In prApplication.WorkingPage.EntityTypeInstance.FindAll(Function(x) x.EntityType.IsObjectifyingEntityType = False)

            Dim lo_combobox_item As tComboboxItem = New tComboboxItem(lrEntityType.Id, lrEntityType.Name, lrEntityType)
            If Not ComboBox_join_object.Items.Contains(lo_combobox_item) Then
                ComboBox_join_object.Items.Add(lo_combobox_item)
            End If
        Next

        ComboBox_join_object.SelectedIndex = 0

    End Sub

    Sub load_value_types()

        Dim lrValueTypeInstance As FBM.ValueTypeInstance

        Try
            Me.ComboBox_join_object.Items.Clear()

            For Each lrValueTypeInstance In prApplication.WorkingPage.ValueTypeInstance.FindAll(Function(x) x.ValueType.IsReferenceMode = False)

                Dim lo_combobox_item As tComboboxItem = New tComboboxItem(lrValueTypeInstance.Id, lrValueTypeInstance.Name, lrValueTypeInstance)

                If Not ComboBox_join_object.Items.Contains(lo_combobox_item) Then
                    ComboBox_join_object.Items.Add(lo_combobox_item)
                    ComboBox_join_object.SelectedIndex = 0
                End If
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Sub load_nested_FactTypes()

        Dim liInd As Integer = 0
        Dim lo_combobox_item As tComboboxItem
        Dim lrFactTypeInstance As FBM.FactTypeInstance

        Try
            Me.ComboBox_join_object.Items.Clear()

            For Each lrFactTypeInstance In prApplication.WorkingPage.FactTypeInstance.FindAll(Function(x) x.IsObjectified = True)
                lo_combobox_item = New tComboboxItem(Nothing, lrFactTypeInstance.Name, lrFactTypeInstance)
                ComboBox_join_object.Items.Add(lo_combobox_item)
                ComboBox_join_object.SelectedIndex = 0
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Function check_fields()

        check_fields = True

    End Function

    Sub GetFields(ByRef arRole As FBM.Role)

        Dim lsFactTypeName As String = ""

        Try
            lsFactTypeName = Viev.Strings.RemoveWhiteSpace(Viev.Strings.MakeCapCamelCase(Trim(TextBox_FactTypeName.Text)))
            lsFactTypeName = arRole.Model.CreateUniqueFactTypeName(lsFactTypeName, 0)

            arRole.Name = Trim(TextBox_RoleName.Text)                        
            arRole.Mandatory = checkbox_mandatory.Checked

            If RadioButton_entity_type.Checked Then
                arRole.ReassignJoinedModelObject(ComboBox_join_object.SelectedItem.tag.EntityType, False)

            ElseIf RadioButton_value_type.Checked Then
                arRole.ReassignJoinedModelObject(ComboBox_join_object.SelectedItem.tag.ValueType, False)

            ElseIf RadioButton_nested_FactType.Checked Then
                arRole.ReassignJoinedModelObject(ComboBox_join_object.SelectedItem.tag.FactType, False)

            End If

            arRole.FactType.SetName(lsFactTypeName)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try



    End Sub



    Private Sub RadioButton_entity_type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton_entity_type.GotFocus

        Dim lsFactTypeName As String = ""

        Call load_entity_types()

        ComboBox_join_object.Select()

        If Me.zbExtendingExistingFactType Then
            lsFactTypeName = Me.zrBaseFactType.Name
            lsFactTypeName &= ComboBox_join_object.SelectedItem.text
            TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
        Else
            lsFactTypeName = ComboBox_join_object.SelectedItem.text
            TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
        End If

    End Sub

    Private Sub RadioButton_value_type_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_value_type.GotFocus

        Dim lsFactTypeName As String = ""

        Try
            Call load_value_types()
            ComboBox_join_object.Select()

            If Me.zbExtendingExistingFactType Then
                lsFactTypeName = Me.zrBaseFactType.Name
                lsFactTypeName &= ComboBox_join_object.SelectedItem.text
                TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
            Else
                lsFactTypeName = ComboBox_join_object.SelectedItem.text
                TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Private Sub RadioButton_nested_FactType_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_nested_FactType.GotFocus

        Dim lsFactTypeName As String = ""

        Try
            Call load_nested_FactTypes()
            ComboBox_join_object.Select()

            If Me.zbExtendingExistingFactType Then
                lsFactTypeName = Me.zrBaseFactType.Name
                lsFactTypeName &= ComboBox_join_object.SelectedItem.text
                TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
            Else
                lsFactTypeName = ComboBox_join_object.SelectedItem.text
                TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        If check_fields() Then

            Call Me.GetFields(Me.zrRole)

            Me.DialogResult = Windows.Forms.DialogResult.OK

            Me.Hide()
            Me.Close()
            Me.Dispose()

        End If

    End Sub

    Private Sub labelpromptRoleName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles labelpromptRoleName.GotFocus

        TextBox_RoleName.Select()

    End Sub

    Private Sub ComboBox_join_object_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_join_object.SelectedIndexChanged

        Dim lsFactTypeName As String = ""

        If Me.zbExtendingExistingFactType Then
            lsFactTypeName = Me.zrBaseFactType.Name
            lsFactTypeName &= ComboBox_join_object.SelectedItem.text
            TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
        Else
            lsFactTypeName = ComboBox_join_object.SelectedItem.text
            TextBox_FactTypeName.Text = Viev.Strings.RemoveWhiteSpace(lsFactTypeName)
        End If

    End Sub

End Class