Imports System.Reflection

Public Class frmCRUDAddAttributeNew

    ''' <summary>
    ''' True if this form used to Edit/Modify a Column/Attribute/Property.
    ''' </summary>
    Public lbEditMode As Boolean = False

    Public mbUseDatabasesDataTypes As Boolean = False

    ''' <summary>
    ''' NB 20150319-VM-It is still undecided as to whether to use the ObjectifyingEntityType or FactType for this member
    '''   if the Entity represents a FactType. Likely to use FactType, as the Me.zrFactType has to join to something
    '''   and cannot join to the ObjectifyingEntityType.
    ''' </summary>
    ''' <remarks></remarks>
    Public zrModel As FBM.Model 'The Model being worked with

    Public zrModelObject As FBM.ModelObject 'The EntityType or FactType that the zrEntity represents.
    Public zrValueType As FBM.ValueType 'The ValueType that the Attribute represents (if not an Attribute that references an Entity/EntityType)    
    Public zrFactType As FBM.FactType 'The FactType linking the EntityType to the ValueType (if Attribute is not part of a Relation) or EntityType (if is an Attribute that references an Entity/EntityType)
    Public zrRDSTable As RDS.Table 'The Table of the Attribute/Column.
    Public zrRDSColumn As RDS.Column 'The RDS Column of the Attribute.

    ''' <summary>
    ''' If the user elects to make a Foreign Key Relationship, this member is populated.
    ''' </summary>
    Public mrForeignKeyModelObject As FBM.ModelObject = Nothing

    Public zsValueTypeName As String


    Public zrAttribute As ERD.Attribute 'The Attribute/s being added to the zrEtntity
    Public zrEntity As Object 'The Entity to which the new Attribute will be added.    

    Public miDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
    Public mbIsMandatory As Boolean = False 'Used to return whether the Attribute is Mandatory.
    Public miDataTypeLength As Integer = 0
    Public miDataTypePrecision As Integer = 0

    '---------------------------------------------
    'Just for form setup
    '---------------------------------------------
    Private msUniqueAttributeName

    Public Overloads Function ShowDialog(ByVal asUniqueAttributeName As String)

        Try
            Me.msUniqueAttributeName = asUniqueAttributeName
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

        Return MyBase.ShowDialog()

    End Function

    Private Sub frmAddAttribute_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try

            Me.LabelPromptAttributeOk.Text = ""

            Me.zsValueTypeName = Me.zrValueType.Id

            Me.LabelEntityTypeName.Text = Me.zrModelObject.Name
            Me.TextBoxDataTypeLength.Text = Me.zrValueType.DataTypeLength
            Me.TextBoxDataTypePrecision.Text = Me.zrValueType.DataTypePrecision
            If Me.zrRDSTable IsNot Nothing AndAlso Me.zrRDSColumn IsNot Nothing Then
                'When this form used for Editing, rather than Adding.
                Me.CheckBoxIsMandatory.Checked = Me.zrRDSColumn.IsMandatory
            End If

            'Length and Precision (DataType based)
#Region "Length and Precision"
            If Not Me.zrValueType.DataTypeUsesLength Then
                Me.LabelPromptLength.Visible = False
                Me.TextBoxDataTypeLength.Visible = False
            End If

            If Not Me.zrValueType.DataTypeUsesPrecision Then
                Me.LabelPromptLength.Visible = False
                Me.TextBoxDataTypePrecision.Visible = False
            End If
#End Region



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

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub PopulatDataTypes()

        Try
            If Me.mbUseDatabasesDataTypes And Me.zrModel.TargetDatabaseType <> pcenumDatabaseType.None Then

                Me.ComboBoxDataType.DataSource = Me.zrModel.DatabaseDataTypes

                ' Find the index of the value that matches zrValueType.DataType
                Dim selectedIndex As Integer
                If zrValueType.DataType <> pcenumORMDataType.DataTypeNotSet Then
                    selectedIndex = ComboBoxDataType.Items.IndexOf(Me.zrModel.getDatabaseDataTypeFromORMDataType(Me.zrModel.TargetDatabaseType, zrValueType.DataType))
                End If

                ' Set the ComboBox's selected index
                If selectedIndex >= 0 Then
                    ComboBoxDataType.SelectedIndex = selectedIndex
                Else
                    ComboBoxDataType.SelectedIndex = 0 ' Default to the first item if not found
                End If

            Else
                    'Use ORM/Boston Datatypes.
                    ComboBoxDataType.DataSource = [Enum].GetValues(GetType(pcenumORMDataType))

                ' Find the index of the value that matches zrValueType.DataType
                Dim selectedIndex As Integer = ComboBoxDataType.Items.IndexOf(Me.zrValueType.DataType)

                ' Set the ComboBox's selected index
                If selectedIndex >= 0 Then
                    ComboBoxDataType.SelectedIndex = selectedIndex
                Else
                    ComboBoxDataType.SelectedIndex = 0 ' Default to the first item if not found
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub LoadModelValueTypes(ByVal arModel As FBM.Model)

        Dim lrValueType As FBM.ValueType

        For Each lrValueType In arModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            Me.ComboBoxAttribute.Items.Add(New tComboboxItem(lrValueType.Id, lrValueType.Name, lrValueType))
            Me.ComboBoxAttribute.Items.Add(New tComboboxItem(lrValueType.Id, lrValueType.Name, lrValueType))
        Next

    End Sub

    Private Function CheckFields() As Boolean

        Dim lsAttributeName As String = FEStrings.MakeCapCamelCase(Trim(Me.ComboBoxAttribute.Text), True)
        Dim lrTable As RDS.Table = If(Me.zrEntity Is Nothing, Me.zrModelObject.getCorrespondingRDSTable, Me.zrEntity.RDSTable)

        If lsAttributeName.Length = 0 Then
            Return False
        ElseIf lbEditMode And lrTable.Column.Find(Function(x) x.Name.Trim.ToLower = lsAttributeName.Trim.ToLower) Is Nothing Then
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub ComboBoxAttribute_KeyUp(sender As Object, e As KeyEventArgs) Handles ComboBoxAttribute.KeyUp

        Try
            Me.LabelPromptAttributeOk.Text = ""

            Dim lsAttributeName = FEStrings.MakeCapCamelCase(Trim(Me.ComboBoxAttribute.Text), True)
            Dim lrTable As RDS.Table = If(Me.zrEntity IsNot Nothing, Me.zrEntity.RDSTable, Me.zrRDSTable)
            Dim lrModelElement As FBM.ModelObject = Me.zrModel.GetModelObjectByName(lsAttributeName)

            If lsAttributeName.Length > 0 Then

                Me.TableLayoutPanelMain.Visible = True

                If lrTable.Column.Find(Function(x) LCase(x.Name) = LCase(Trim(Me.ComboBoxAttribute.Text)) And x IsNot Me.zrRDSColumn) IsNot Nothing Then

                    Me.LabelPromptAttributeOk.Font = New Font("Arial Unicode MS", 8)
                    Me.LabelPromptAttributeOk.Text = "A Property/Column for the Node Type/Entity " & If(Me.zrEntity IsNot Nothing, Me.zrEntity.RDSTable.Name, Me.zrRDSTable.Name) & " already exists with this Attribute Name."
                    Me.LabelPromptAttributeOk.ForeColor = Color.Red
                    Me.TableLayoutPanelFKReference.Visible = False

                ElseIf lrModelElement IsNot Nothing Then
                    Me.TableLayoutPanelFKReference.Visible = False
                    Me.LabelPromptAttributeOk.Font = New Font("Arial Unicode MS", 8)
                    Select Case lrModelElement.GetType
                        Case Is = GetType(FBM.EntityType)
                            'Might be making a Foreign Key Reference.
#Region "Load Tables"
                            Me.TableLayoutPanelFKReference.Visible = True

                            Dim larTable = (From Table In Me.zrModel.RDS.Table
                                            From Column In Table.Column
                                            Where Column.ActiveRole IsNot Nothing
                                            Where Column.ActiveRole.JoinedORMObject.Id = Me.ComboBoxAttribute.Text.Trim
                                            Select Table).Distinct

                            Me.ListBoxReferencedTable.SelectedIndex = -1
                            Me.ListBoxReferencedTable.Items.Clear()
                            For Each lrTable In larTable
                                Dim lrComboboxItem = New tComboboxItem(lrTable, lrTable.Name, lrTable)
                                Me.ListBoxReferencedTable.Items.Add(lrComboboxItem)
                            Next
#End Region
                        Case Is = GetType(FBM.FactType)
                            If CType(lrModelElement, FBM.FactType).IsObjectified Then
                                'Might be making a Foreign Key Reference.
#Region "Load Tables"
                                Me.TableLayoutPanelFKReference.Visible = True

                                Dim larTable = (From Table In Me.zrModel.RDS.Table
                                                From Column In Table.Column
                                                Where Column.ActiveRole IsNot Nothing
                                                Where Column.ActiveRole.JoinedORMObject.Id = Me.ComboBoxAttribute.Text.Trim
                                                Select Table).Distinct

                                Me.ListBoxReferencedTable.SelectedIndex = -1
                                Me.ListBoxReferencedTable.Items.Clear()
                                For Each lrTable In larTable
                                    Dim lrComboboxItem = New tComboboxItem(lrTable, lrTable.Name, lrTable)
                                    Me.ListBoxReferencedTable.Items.Add(lrComboboxItem)
                                Next
#End Region
                            End If
                        Case Is = GetType(FBM.ValueType)
                            'Might be making a Foreign Key Reference.
#Region "Load Tables"
                            Me.TableLayoutPanelFKReference.Visible = True

                            Dim larTable = (From Table In Me.zrModel.RDS.Table
                                            From Column In Table.Column
                                            Where Column.ActiveRole IsNot Nothing
                                            Where Column.ActiveRole.JoinedORMObject.Id = Me.ComboBoxAttribute.Text.Trim
                                            Select Table).Distinct

                            Me.ListBoxReferencedTable.SelectedIndex = -1
                            Me.ListBoxReferencedTable.Items.Clear()
                            For Each lrTable In larTable
                                Dim lrComboboxItem = New tComboboxItem(lrTable, lrTable.Name, lrTable)
                                Me.ListBoxReferencedTable.Items.Add(lrComboboxItem)
                            Next
#End Region

                            'Me.LabelPromptAttributeOk.Text = "A Value Type already exists in the Model for with this Name. Only proceed if you are okay to reuse the Value Type for this Property/Column."
                            'Me.LabelPromptAttributeOk.ForeColor = Color.Orange
                        Case Else
                            Me.LabelPromptAttributeOk.Text = "A Model Element already exists in the Model for with this Name. You can't use that name."
                            Me.LabelPromptAttributeOk.ForeColor = Color.Red
                    End Select
                Else
                    Me.LabelPromptAttributeOk.Font = New Font("Arial Unicode MS", 10)
                    Me.LabelPromptAttributeOk.Text = (ChrW(&H2714)).ToString() '(ChrW(&H221A)).ToString() 'Tick
                    Me.LabelPromptAttributeOk.ForeColor = Color.Green
                    Me.TableLayoutPanelFKReference.Visible = False
                End If
            Else
                Me.TableLayoutPanelMain.Visible = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        If Me.CheckFields Then

            Me.zsValueTypeName = FEStrings.MakeCapCamelCase(Trim(Me.ComboBoxAttribute.Text))
            Me.mbIsMandatory = Me.CheckBoxIsMandatory.Checked
            If Me.mbUseDatabasesDataTypes And Me.zrModel.IsConnectedToDatabase Then
                Me.miDataType = Me.zrModel.DatabaseConnection.getBostonDataTypeByDatabaseDataType(Me.ComboBoxDataType.SelectedItem)
            Else
                Me.miDataType = Me.ComboBoxDataType.SelectedItem
            End If
            Me.mbIsMandatory = Me.CheckBoxIsMandatory.Checked
            Me.miDataTypeLength = CInt(If(Me.TextBoxDataTypeLength.Text.Trim = "", "0", Me.TextBoxDataTypeLength.Text.Trim))
            Me.miDataTypePrecision = CInt(If(Me.TextBoxDataTypePrecision.Text.Trim = "", "0", Me.TextBoxDataTypePrecision.Text.Trim))

            If Me.ListBoxReferencedTable.SelectedIndex >= 0 Then
                Me.mrForeignKeyModelObject = Me.ListBoxReferencedTable.SelectedItem.ItemData.FBMModelElement

                Dim lrTable = Me.mrForeignKeyModelObject.getCorrespondingRDSTable

                Dim lsMessage As String = "The referenced Entity/Table/Node Type has no Primary Key."
                lsMessage.AppendDoubleLineBreak("Please select another reference or add a Primary Key to the Entity/Table/Node Type.")

                If lrTable.getPrimaryKeyColumns.Count = 0 Then
                    Boston.ShowFlashCard(lsMessage, Color.Salmon)
                    Exit Sub
                End If
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK

            Me.Close()

        Else
            Exit Sub
            'Nothing to do here
        End If

    End Sub

    Private Sub ListBoxReferencedTable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBoxReferencedTable.SelectedIndexChanged

        Try
            'CodeSafe
            If Me.ListBoxReferencedTable.SelectedIndex < 0 Then Exit Sub

            'Only really want to lock in the data type for the Attribute if the MakeForeignKeyReference Checkbox is checked.
            If Not Me.CheckBoxMakeForeignKeyReference.Checked Then Exit Sub

            Me.CheckBoxIsMandatory.Enabled = True
            Me.ComboBoxDataType.Enabled = True
            Me.TextBoxDataTypeLength.Enabled = True
            Me.TextBoxDataTypePrecision.Enabled = True

            Dim lrModelElement = Me.ListBoxReferencedTable.SelectedItem.ItemData.FBMModelElement

            Select Case lrModelElement.GetType
                Case Is = GetType(FBM.EntityType)

                    Dim lrEntityType = CType(lrModelElement, FBM.EntityType)

                    Dim lsDataType As String

                    Dim lrTable = lrEntityType.getCorrespondingRDSTable(False)

                    If lrTable.getPrimaryKeyColumns.Count = 0 Then
                        Boston.ShowFlashCard("This Entity/Table/Node Type has no Primary Key", Color.Salmon)
                        Exit Sub
                    End If

                    If lrEntityType.HasSimpleReferenceScheme Then

                        lsDataType = lrEntityType.ReferenceModeValueType.DBDataType

                        Dim selectedItem = ComboBoxDataType.Items.Cast(Of Object)().FirstOrDefault(Function(item) item = lsDataType)
                        ComboBoxDataType.SelectedItem = If(selectedItem, ComboBoxDataType.Items(0))

                        Me.CheckBoxIsMandatory.Checked = True

                        Dim lrColum = lrTable.getPrimaryKeyColumns(0)
                        Me.TextBoxDataTypeLength.Text = lrColum.getMetamodelDataTypeLength
                        Me.TextBoxDataTypePrecision.Text = lrColum.getMetamodelDataTypePrecision

                        'Disable the appropriate fields                        
                        Me.ComboBoxDataType.Enabled = False
                        Me.TextBoxDataTypeLength.Enabled = False
                        Me.TextBoxDataTypePrecision.Enabled = False

                    Else
                        Dim lrColumn = lrTable.Column.Find(Function(x) x.Name = Me.ComboBoxAttribute.Text.Trim)

                        If lrColumn IsNot Nothing Then
                            Me.TextBoxDataTypeLength.Text = lrColumn.getMetamodelDataTypeLength
                            Me.TextBoxDataTypePrecision.Text = lrColumn.getMetamodelDataTypePrecision

                            lsDataType = lrColumn.DBDataType
                            Dim selectedItem = ComboBoxDataType.Items.Cast(Of Object)().FirstOrDefault(Function(item) item = lsDataType)
                            ComboBoxDataType.SelectedItem = If(selectedItem, ComboBoxDataType.Items(0))

                            'Disable the appropriate fields                            
                            Me.ComboBoxDataType.Enabled = False
                            Me.TextBoxDataTypeLength.Enabled = False
                            Me.TextBoxDataTypePrecision.Enabled = False
                        End If

                    End If

            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CheckBoxMakeForeignKeyReference_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxMakeForeignKeyReference.CheckedChanged

        Try
            If Me.CheckBoxMakeForeignKeyReference.Checked Then

                'CodeSafe
                If Me.ListBoxReferencedTable.SelectedIndex < 0 Then Exit Sub

                Me.CheckBoxIsMandatory.Enabled = True
                Me.ComboBoxDataType.Enabled = True
                Me.TextBoxDataTypeLength.Enabled = True
                Me.TextBoxDataTypePrecision.Enabled = True

                Dim lrModelElement = Me.ListBoxReferencedTable.SelectedItem.ItemData.FBMModelElement

                Select Case lrModelElement.GetType
                    Case Is = GetType(FBM.EntityType)

                        Dim lrEntityType = CType(lrModelElement, FBM.EntityType)

                        Dim lsDataType As String

                        If lrEntityType.HasSimpleReferenceScheme Then

                            lsDataType = lrEntityType.ReferenceModeValueType.DBDataType

                            Dim selectedItem = ComboBoxDataType.Items.Cast(Of Object)().FirstOrDefault(Function(item) item = lsDataType)
                            ComboBoxDataType.SelectedItem = If(selectedItem, ComboBoxDataType.Items(0))

                            Me.CheckBoxIsMandatory.Checked = True

                            Dim lrTable = lrEntityType.getCorrespondingRDSTable(False)

                            Dim lrColum = lrTable.getPrimaryKeyColumns(0)
                            Me.TextBoxDataTypeLength.Text = lrColum.getMetamodelDataTypeLength
                            Me.TextBoxDataTypePrecision.Text = lrColum.getMetamodelDataTypePrecision

                            'Disable the appropriate fields
                            Me.ComboBoxDataType.Enabled = False
                            Me.TextBoxDataTypeLength.Enabled = False
                            Me.TextBoxDataTypePrecision.Enabled = False

                        End If

                End Select

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxDataType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxDataType.SelectedIndexChanged

        Try
            Dim liORMDataType As pcenumORMDataType = pcenumORMDataType.TextFixedLength

            If Me.mbUseDatabasesDataTypes And Me.zrModel.IsConnectedToDatabase Then
                liORMDataType = Me.zrModel.DatabaseConnection.getBostonDataTypeByDatabaseDataType(Me.ComboBoxDataType.SelectedItem)
            Else
                liORMDataType = Me.ComboBoxDataType.SelectedItem
            End If

            Dim lrValueType As New FBM.ValueType(Me.zrModel, pcenumLanguage.ORMModel, "DummyValueType", "DummyValueType")
            lrValueType.DataType = liORMDataType

            'Length and Precision (DataType based)
#Region "Length and Precision"
            If Not lrValueType.DataTypeUsesLength Then
                Me.TextBoxDataTypeLength.Enabled = True
                Me.TextBoxDataTypeLength.Text = "0"
                Me.LabelPromptLength.Visible = False
                Me.TextBoxDataTypeLength.Visible = False

            Else
                Me.LabelPromptLength.Visible = True
                Me.TextBoxDataTypeLength.Visible = True
            End If

            If Not lrValueType.DataTypeUsesPrecision Then
                Me.TextBoxDataTypePrecision.Enabled = True
                Me.TextBoxDataTypePrecision.Text = "0"
                Me.LabelPromptPrecision.Visible = False
                Me.TextBoxDataTypePrecision.Visible = False
            Else
                Me.LabelPromptPrecision.Visible = True
                Me.TextBoxDataTypePrecision.Visible = True
            End If
#End Region

        Catch ex As Exception

        End Try
    End Sub
End Class