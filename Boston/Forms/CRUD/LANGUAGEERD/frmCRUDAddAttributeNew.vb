Imports System.Reflection

Public Class frmCRUDAddAttributeNew

    Public zrAttribute As ERD.Attribute 'The Attribute/s being added to the zrEtntity
    Public zrEntity As Object 'The Entity to which the new Attribute will be added.    
    Public zrModel As FBM.Model 'The Model being worked with

    Public zbAttributeIsMandatory As Boolean = False 'Used to return whether the Attribute is Mandatory.
    Public zbDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
    Public ziDataTypeLength As Integer = 0
    Public ziDataTypePrecision As Integer = 0

    '---------------------------------------------
    'Just for form setup
    '---------------------------------------------
    Private msUniqueAttributeName

    ''' <summary>
    ''' NB 20150319-VM-It is still undecided as to whether to use the ObjectifyingEntityType or FactType for this member
    '''   if the Entity represents a FactType. Likely to use FactType, as the Me.zrFactType has to join to something
    '''   and cannot join to the ObjectifyingEntityType.
    ''' </summary>
    ''' <remarks></remarks>
    Public zrModelObject As FBM.ModelObject 'The EntityType or FactType that the zrEntity represents.
    Public zrValueType As FBM.ValueType 'The ValueType that the Attribute represents (if not an Attribute that references an Entity/EntityType)
    Public zsValueTypeName As String
    Public zrFactType As FBM.FactType 'The FactType linking the EntityType to the ValueType (if Attribute is not part of a Relation) or EntityType (if is an Attribute that references an Entity/EntityType)

    Public Overloads Function ShowDialog(ByVal asUniqueAttributeName As String)

        Try
            Me.msUniqueAttributeName = asUniqueAttributeName
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

        Return MyBase.ShowDialog()

    End Function

    Private Sub frmAddAttribute_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Me.LabelPromptAttributeOk.Text = ""

        Me.zsValueTypeName = Me.zrValueType.Id

        Me.LabelEntityTypeName.Text = Me.zrModelObject.Name

        RemoveHandler Me.ComboBoxAttribute.TextChanged, AddressOf Me.ComboBoxAttribute_TextChanged
        Me.ComboBoxAttribute.Text = Me.msUniqueAttributeName
        AddHandler Me.ComboBoxAttribute.TextChanged, AddressOf Me.ComboBoxAttribute_TextChanged

        Me.ComboBoxAttribute.SelectAll()

        '--------------------------------------------------------------------------------------
        'Populate the AttributeName ComboBox with the list of ValueType/Names for all of the
        '  ValueTypes within the Model, that are not already associated with the Entity.
        '  NB If the User selects one of the ValueTypes, then a FactType will be created
        '  linked to either that ValueType or the Entity to which it is the ReferenceMode.
        '--------------------------------------------------------------------------------------
        Call Me.LoadModelValueTypes(Me.zrModel)

        Call Me.PopulatDataTypes()

    End Sub

    Private Sub PopulatDataTypes()

        ComboBoxDataType.DataSource = [Enum].GetValues(GetType(pcenumORMDataType))
        Me.ComboBoxDataType.SelectedIndex = 0

    End Sub

    Private Sub LoadModelValueTypes(ByVal arModel As FBM.Model)

        Dim lrValueType As FBM.ValueType

        For Each lrValueType In arModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            Me.ComboBoxAttribute.Items.Add(New tComboboxItem(lrValueType.Id, lrValueType.Name, lrValueType))
            Me.ComboBoxAttribute.Items.Add(New tComboboxItem(lrValueType.Id, lrValueType.Name, lrValueType))
        Next

    End Sub

    Private Function CheckFields() As Boolean

        Dim lsAttributeName = Viev.Strings.MakeCapCamelCase(Trim(Me.ComboBoxAttribute.Text))
        Dim lrTable As RDS.Table = Me.zrEntity.RDSTable

        If lsAttributeName.Length = 0 Then
            Return False
        ElseIf lrTable.Column.Find(Function(x) lcase(x.Name) = lcase(lsAttributeName)) IsNot Nothing Then
            Return False
        End If

        Dim lrModelElement As FBM.ModelObject = Me.zrModel.GetModelObjectByName(lsAttributeName)

        If lrModelElement IsNot Nothing Then
            Select Case lrModelElement.GetType
                Case Is = GetType(FBM.EntityType)
                    Me.LabelPromptAttributeOk.Text = "An Entity Type exists in the Model for with this Name. You can't use that name."
                    Me.LabelPromptAttributeOk.ForeColor = Color.Red
                    Return False
                Case Is = GetType(FBM.FactType)
                    Me.LabelPromptAttributeOk.Text = "A Fact Type exists in the Model for with this Name. You can't use that name."
                    Me.LabelPromptAttributeOk.ForeColor = Color.Red
                    Return False
                Case Is = GetType(FBM.ValueType)
                    Me.LabelPromptAttributeOk.Text = "A Value Type already exists in the Model for with this Name. Only proceed if you are okay to reuse the Value Type for this Property/Column."
                    Me.LabelPromptAttributeOk.ForeColor = Color.Orange
                    Return True
                Case Else
                    Me.LabelPromptAttributeOk.Text = "A Model Element already exists in the Model for with this Name. You can't use that name."
                    Me.LabelPromptAttributeOk.ForeColor = Color.Red
                    Return False
            End Select
        End If

OkayToProceed:

        CheckFields = True

    End Function

    Private Sub frmAddAttribute_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.Escape Then
            e.Handled = True
            e.SuppressKeyPress = True
        End If

    End Sub


    Private Sub ComboBoxAttribute_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxAttribute.TextChanged

        If Trim(Me.ComboBoxAttribute.Text) = "" Then Exit Sub

        Me.zsValueTypeName = Trim(Me.ComboBoxAttribute.Text)

    End Sub

    Private Sub TextBoxDataTypeLength_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxDataTypeLength.KeyPress

        Try
            '97 - 122 = Ascii codes for simple letters
            '65 - 90  = Ascii codes for capital letters
            '48 - 57  = Ascii codes for numbers

            If Asc(e.KeyChar) <> 8 Then
                If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                    e.Handled = True
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxDataTypePrecision_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxDataTypePrecision.KeyPress

        Try

            '97 - 122 = Ascii codes for simple letters
            '65 - 90  = Ascii codes for capital letters
            '48 - 57  = Ascii codes for numbers

            If Asc(e.KeyChar) <> 8 Then
                If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                    e.Handled = True
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub ComboBoxAttribute_KeyUp(sender As Object, e As KeyEventArgs) Handles ComboBoxAttribute.KeyUp

        Try
            Me.LabelPromptAttributeOk.Text = ""

            Dim lsAttributeName = Viev.Strings.MakeCapCamelCase(Trim(Me.ComboBoxAttribute.Text))
            Dim lrTable As RDS.Table = Me.zrEntity.RDSTable
            Dim lrModelElement As FBM.ModelObject = Me.zrModel.GetModelObjectByName(lsAttributeName)

            If lsAttributeName.Length > 0 Then
                If lrTable.Column.Find(Function(x) LCase(x.Name) = LCase(Trim(Me.ComboBoxAttribute.Text))) IsNot Nothing Then
                    Me.LabelPromptAttributeOk.Font = New Font("Arial Unicode MS", 8)
                    Me.LabelPromptAttributeOk.Text = "A Property/Column for the Node Type/Entity " & Me.zrEntity.Name & " already exists with this Attribute Name."
                    Me.LabelPromptAttributeOk.ForeColor = Color.Red
                ElseIf lrModelElement IsNot Nothing Then
                    Me.LabelPromptAttributeOk.Font = New Font("Arial Unicode MS", 8)
                    Select Case lrModelElement.GetType
                        Case Is = GetType(FBM.EntityType)
                            Me.LabelPromptAttributeOk.Text = "An Entity Type exists in the Model for with this Name. You can't use that name."
                            Me.LabelPromptAttributeOk.ForeColor = Color.Red
                        Case Is = GetType(FBM.FactType)
                            Me.LabelPromptAttributeOk.Text = "A Fact Type exists in the Model for with this Name. You can't use that name."
                            Me.LabelPromptAttributeOk.ForeColor = Color.Red
                        Case Is = GetType(FBM.ValueType)
                            Me.LabelPromptAttributeOk.Text = "A Value Type already exists in the Model for with this Name. Only proceed if you are okay to reuse the Value Type for this Property/Column."
                            Me.LabelPromptAttributeOk.ForeColor = Color.Orange
                        Case Else
                            Me.LabelPromptAttributeOk.Text = "A Model Element already exists in the Model for with this Name. You can't use that name."
                            Me.LabelPromptAttributeOk.ForeColor = Color.Red
                    End Select
                Else
                    Me.LabelPromptAttributeOk.Font = New Font("Arial Unicode MS", 10)
                    Me.LabelPromptAttributeOk.Text = (ChrW(&H2714)).ToString() '(ChrW(&H221A)).ToString() 'Tick
                    Me.LabelPromptAttributeOk.ForeColor = Color.Green
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        If Me.CheckFields Then

            Me.zsValueTypeName = Viev.Strings.MakeCapCamelCase(Trim(Me.ComboBoxAttribute.Text))
            Me.zbAttributeIsMandatory = Me.CheckBoxIsMandatory.Checked
            Me.zbDataType = Me.ComboBoxDataType.SelectedItem
            Me.ziDataTypeLength = CInt(If(Me.TextBoxDataTypeLength.Text.Trim = "", "0", Me.TextBoxDataTypeLength.Text.Trim))
            Me.ziDataTypePrecision = CInt(If(Me.TextBoxDataTypePrecision.Text.Trim = "", "0", Me.TextBoxDataTypePrecision.Text.Trim))
            Me.DialogResult = Windows.Forms.DialogResult.OK

            Me.Close()

        Else
            Exit Sub
            'Nothing to do here
        End If

    End Sub

End Class