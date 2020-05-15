Public Class frmCRUDAddRelationship

    Public zrModel As FBM.Model 'The Model being worked with
    Public zrEntity As ERD.Entity 'The entity to which the new Attribute will be added.
    Public zrModelObject As Object
    Public zrReferencesModelObject As Object 'Used to return the Entity to which the new (if any) ERD.Relationship will be made.
    Public zbAttributeIsMandatory As Boolean = False 'Used to return whether the Attribute is Mandatory.
    Public zbAttributeIsPartOfPrimaryKey As Boolean = False 'Used to return whether the Attribute is part of the PrimaryKey of the Entity of the Attribute.

    '---------------------------------------------
    'EnterpriseAware (Intellisense like) code
    '---------------------------------------------
    Public ziCurrentTagStart As Integer = 0
    Public ziIntellisenseBuffer As String = ""
    Private zrHashList As New Hashtable
    Private zrTermList As New List(Of String) 'List of ORMObjectTypes within the FactType for which the reading is being created.
    Public zarEntity As New List(Of FBM.FactDataInstance)

    Public zrFactType As FBM.FactType
    Private zrValueType As FBM.ValueType

    Private Sub frmAddAttribute_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Dim liInd As Integer
        Dim ls_joined_object_name As String = ""

        Me.LabelEntityTypeName.Text = Me.zrModelObject.Name

        Dim lsNewUniqueAttributeName As String = ""
        lsNewUniqueAttributeName = Me.zrEntity.CreateUniqueAttributeName("NewAttribute", 0)

        Me.zrFactType = New FBM.FactType(Me.zrModel, Me.zrModelObject.Name & lsNewUniqueAttributeName, True)

        '--------------------------------------------------------------------------------
        'Create a new ValueType (in anticipation of [OK] button press) within the Model
        '--------------------------------------------------------------------------------
        Me.zrValueType = Me.zrModel.CreateValueType
        Me.zrValueType.SetName(lsNewUniqueAttributeName)

        '-------------------------------------------------------------------
        'Establish a dummy FactType for the new Attribute.
        '  NB If the User clicks [Cancel] on this form, then the FactType 
        '  and ValueType must be removed from the Model.
        '-------------------------------------------------------------------
        Me.zrFactType = Me.zrModelObject.AddBinaryRelationToValueType(Me.zrValueType, pcenumBinaryRelationMultiplicityType.ManyToOne)

        '--------------------------------------------------------------------------------------
        'Populate the AttributeName ComboBox with the list of ValueType/Names for all of the
        '  ValueTypes within the Model, that are not already associated with the Entity.
        '  NB If the User selects one of the ValueTypes, then a FactType will be created
        '  linked to either that ValueType or the Entity to which it is the ReferenceMode.
        '--------------------------------------------------------------------------------------
        Call Me.PopulateTermList()

        Call Me.PopulateReferences()

        '--------------------------------
        'Setup the FactTypeReading grid
        '--------------------------------
        Dim colRichText As New tRichtextColumn()
        Me.DataGrid_Readings.Columns.Add(colRichText)
        Me.DataGrid_Readings.Width = Me.Width - (Me.DataGrid_Readings.Left)
        Me.DataGrid_Readings.Columns(0).Width = Me.Width

        For liInd = 1 To Me.zrFactType.Arity
            Select Case Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.ConceptType
                Case Is = pcenumConceptType.EntityType
                    ls_joined_object_name = Trim(Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.Name)
                Case Is = pcenumConceptType.FactType
                    ls_joined_object_name = Trim(Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.Name)
                Case Is = pcenumConceptType.ValueType
                    ls_joined_object_name = Trim(Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.Name)
            End Select

            '--------------------------------------------------------
            'Add to the EnterpriseAware (IntellisenseLike) listbox
            '--------------------------------------------------------
            ListBox_EnterpriseAware.Items.Add(New tcomboboxitem(liInd, ls_joined_object_name))

            If Me.zrHashList.Contains(ls_joined_object_name) Then
                Me.zrHashList(ls_joined_object_name) += 1
            Else
                Me.zrHashList.Add(Trim(ls_joined_object_name), 1)
            End If
        Next

        TextboxReading.DeselectAll()

    End Sub

    Private Sub PopulateReferences()

        Dim lrEntity As ERD.Entity

        For Each lrEntity In Me.zarEntity

            'was os Tag/Object - New FBM.DictionaryEntry(Me.zrFactType.Model, lrEntity.Name, pcenumConceptType.Value)
            Me.ComboBoxReferences.Items.Add(New tComboboxItem(lrEntity.Id, lrEntity.Name, lrEntity))

        Next

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click

        Me.zbAttributeIsMandatory = Me.CheckBoxIsMandatory.Checked
        Me.Tag = Me.zrValueType
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub RadioButtonManyToOne_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonManyToOne.CheckedChanged

        If IsSomething(Me.zrModelObject) Then
            Call Me.UpdateVerbalisationReading()
            Call Me.zrFactType.CreateManyToOneInternalUniquenessConstraint(Me.zrModelObject)
        End If

        Me.PictureBoxMultiplicity.Image = My.Resources.BarkerERImages.ManyToOne

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub RadioButtonOneToOne_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonOneToOne.CheckedChanged

        Call Me.UpdateVerbalisationReading()
        Call Me.zrFactType.CreateOneToOneInternalUniquenessConstraint(True)

        Me.PictureBoxMultiplicity.Image = My.Resources.BarkerERImages.OneToOne

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub UpdateVerbalisationReading()

        Dim lsMessage As String = ""

        'If RadioButtonManyToOne.Checked Then
        '    lsMessage = "Each [" & Me.zrModelObject.Name & "]"
        '    lsMessage &= " associates with at most one "
        '    lsMessage &= "[" & Me.ComboBoxAttribute.Text & "]"
        '    lsMessage &= vbCrLf
        '    lsMessage &= "Each [" & Me.ComboBoxAttribute.Text & "]"
        '    lsMessage &= " associates with many "
        '    lsMessage &= "[" & Me.zrModelObject.Name & "/s]"
        'Else
        '    lsMessage = "Each [" & Me.zrModelObject.Name & "]"
        '    lsMessage &= " associates with at most one "
        '    lsMessage &= "[" & Me.ComboBoxAttribute.Text & "]"
        '    lsMessage &= vbCrLf
        '    lsMessage &= "Each [" & Me.ComboBoxAttribute.Text & "]"
        '    lsMessage &= " associates with at most one "
        '    lsMessage &= "[" & Me.zrModelObject.Name & "]"
        'End If

        Me.LabelMultiplicityVerbalisation.Text = lsMessage

    End Sub

    Private Sub frm_orm_reading_editor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        If IsSomething(frmMain) Then
            frmMain.zfrm_orm_reading_editor = Nothing
        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '------------------------------
        'Clears the Reading TextBox
        '------------------------------
        Dim lo_sentence As Language.Sentence
        Dim lsWord As String = ""

        lo_sentence = New Language.Sentence(Me.TextboxReading.Text)

        TextboxReading.SelectAll()
        TextboxReading.SelectionProtected = False
        TextboxReading.Text = ""

        Call load_enterprise_aware_listbox()

    End Sub

    Sub PopulateTermList()

        Dim liInd As Integer
        Dim lsJoinedObjectName As String = ""

        Me.ListBox_EnterpriseAware.Items.Clear()

        Me.zrTermList.Clear()
        Me.zrHashList.Clear()

        For liInd = 1 To Me.zrFactType.Arity
            Select Case Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.ConceptType
                Case Is = pcenumConceptType.EntityType
                    lsJoinedObjectName = Trim(Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.Name)
                Case Is = pcenumConceptType.FactType
                    lsJoinedObjectName = Trim(Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.Name)
                Case Is = pcenumConceptType.ValueType
                    lsJoinedObjectName = Trim(Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.Name)
            End Select

            '----------------------------------------------------------------
            'Add to the list of ORMObjectTypes within the selected FactType
            '----------------------------------------------------------------
            Me.zrTermList.Add(lsJoinedObjectName)

            If Me.zrHashList.Contains(lsJoinedObjectName) Then
                Me.zrHashList(lsJoinedObjectName) += 1
            Else
                Me.zrHashList.Add(Trim(lsJoinedObjectName), 1)
            End If

            '--------------------------------------------------------
            'Add to the EnterpriseAware (IntellisenseLike) listbox
            '--------------------------------------------------------
            Me.ListBox_EnterpriseAware.Items.Add(New tcomboboxitem(liInd, lsJoinedObjectName))
        Next

    End Sub


    Private Sub get_PredicateParts_from_reading(ByVal as_reading As String, ByRef arFactTypeReading As FBM.FactTypeReading)

        Dim li_PredicatePart_SequenceNr As Integer = 0
        Dim liSequenceNr As Integer = 0
        Dim lb_found_first_object_type As Boolean = False
        Dim lsWord As String = ""
        Dim las_word_list() As String
        Dim ls_prefix As String = ""
        Dim lsSuffix As String = ""
        Dim lrPredicatePart As New FBM.PredicatePart(arFactTypeReading.model, arFactTypeReading)

        Dim lr_orm_ObjectTypeList As New List(Of FBM.ModelObject)
        Dim lrPredicateParts = New List(Of FBM.PredicatePart)

        las_word_list = as_reading.Split

        '--------------------------------------------------------
        'Perform Left-2-Right parsing to get the PredicateParts
        '--------------------------------------------------------
        For Each lsWord In las_word_list
            '----------------------------------------
            'Check to see if the word is one of the
            '  ORM Object Types within the reading
            '----------------------------------------            

            If Me.zrHashList.Contains(lsWord) Then
                '----------------------------------------
                'The word is one of the ORM Object Types
                '----------------------------------------
                lr_orm_ObjectTypeList.Add(New FBM.ModelObject(lsWord))

                liSequenceNr += 1
                If liSequenceNr = 1 Then
                    '------------------------------------------------------------------------------
                    'No Longer supported (v1.13 of the database Model).
                    'lrPredicatePart.ObjectType1 = New FBM.ModelObject(lsWord)
                ElseIf (liSequenceNr = 2) Then
                    '------------------------------------------------------------------------------
                    'No Longer supported (v1.13 of the database Model).
                    'lrPredicatePart.ObjectType2 = New FBM.ModelObject(lsWord)
                    lrPredicatePart.PredicatePartText = Trim(lsSuffix)
                    ls_prefix = ""
                    lsSuffix = ""
                    li_PredicatePart_SequenceNr += 1
                    lrPredicatePart.SequenceNr = li_PredicatePart_SequenceNr
                    lrPredicateParts.Add(lrPredicatePart)
                    '----------------------------
                    'Create a new PredicatePart
                    '----------------------------
                    lrPredicatePart = New FBM.PredicatePart(arFactTypeReading.Model, arFactTypeReading)
                    '------------------------------------------------------------------------------
                    'No Longer supported (v1.13 of the database Model).
                    'lrPredicatePart.ObjectType1 = New FBM.ModelObject(lsWord)
                    liSequenceNr = 1
                End If
            Else
                If liSequenceNr = 0 Then
                    ls_prefix &= " " & lsWord
                Else
                    lsSuffix &= " " & lsWord
                End If
            End If
        Next

        arFactTypeReading.PredicatePart = lrPredicateParts
        ' '------------------------------------------------------------------------------
        'No Longer supported (v1.13 of the database Model).
        'arFactTypeReading.ObjectTypeList = lr_orm_ObjectTypeList

    End Sub

    Private Sub TextboxReading_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxReading.GotFocus

        Dim lsMessage As String = ""

        lsMessage = "Enter a Reading for the new Attribute."
        'lsMessage &= " e.g. '" & Me.zrModelObject.Name & " has " & Me.ComboBoxAttribute.Text & "'"
        lsMessage &= vbCrLf & "Press [Enter] to save the Reading."

        Me.LabelHelp.Text = lsMessage

    End Sub

    Private Sub TextboxReading_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextboxReading.KeyDown

        If e.KeyCode = Keys.OemPeriod Then '(e.KeyChar = ".") Then
            '-------------------------------------------------------------------------------------
            'User wants to view the EnterpriseAware listbox. Has hit the '.' key on their keypad
            '-------------------------------------------------------------------------------------

            '------------------------------------------------------------
            'Check if there are any more EnterpiseAware items left in the 
            '  ListboxEnterpriseAware
            '------------------------------------------------------------
            If ListBox_EnterpriseAware.Items.Count = 0 Then
                Exit Sub
            Else
                '------------------------------------------------------
                'The user hasn't used the quota of Terms so popup the
                '------------------------------------------------------
                ziCurrentTagStart = TextboxReading.SelectionStart - 1

                Dim lo_point As New Point(TextboxReading.GetPositionFromCharIndex(TextboxReading.SelectionStart))
                lo_point.Y += CInt(TextboxReading.Font.GetHeight() * 2)
                ListBox_EnterpriseAware.Location = lo_point
                ListBox_EnterpriseAware.Show()

                Me.ActiveControl = Me.ListBox_EnterpriseAware

                e.Handled = True
                e.SuppressKeyPress = True
            End If
        End If

        If (e.KeyCode = Keys.Enter) Then
            '----------------------------------------------------------------------------------
            'User has finished editing the FactTypeReading and is ready to commit the changes
            '----------------------------------------------------------------------------------

            '-------------------------------------------------------------------------
            'Create a FactTypeReading object to accompany the reading in the DataGrid
            '-------------------------------------------------------------------------
            Dim lrFactTypeReading As New FBM.FactTypeReading(Me.zrFactType)
            Call Me.get_PredicateParts_from_reading(Trim(Me.TextboxReading.Text), lrFactTypeReading)

            '-------------------------------------------------
            'Add the FactTypeReading to the underlying Model
            '-------------------------------------------------
            Me.zrFactType.FactTypeReading.Add(lrFactTypeReading)

            '----------------------------------
            'Create a new row in the DataGrid
            '----------------------------------            
            Me.DataGrid_Readings.Rows.Add()
            Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(0).Value = Trim(Me.TextboxReading.Text)
            Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Tag = lrFactTypeReading
            Dim loObject As Object = Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(0)
            loObject.term_list = Me.zrTermList

            TextboxReading.Text = ""

            '------------------------------------
            'Reload the EnterpriseAware ListBox
            '------------------------------------
            Call load_enterprise_aware_listbox()
        ElseIf e.KeyCode = Keys.Space Then
            If Me.TextboxReading.SelectionStart >= Me.TextboxReading.TextLength Then
                Call Me.ProtectFactTypeTerms(Me.TextboxReading)
            End If
        End If

    End Sub


    Sub ProtectFactTypeTerms(ByVal aoRichTextBox As Object)
        '------------------------------------------------------------------------
        'Protects the names of ORM Object Types within the Reading so that
        '  the User doesn't accidentally delete or change them.
        '  Makes it easier to structure the Fact Type Reading as a Predicate.
        '------------------------------------------------------------------------

        Dim liInd As Integer
        Dim liInd2 As Integer = 1
        Dim ls_joined_object_name As String = ""


        aoRichTextBox.SelectAll()
        aoRichTextBox.SelectionProtected = False
        aoRichTextBox.SelectionColor = Color.Black
        aoRichTextBox.DeselectAll()
        aoRichTextBox.SelectionStart = aoRichTextBox.TextLength

        For liInd = 1 To Me.zrFactType.Arity
            '------------------------------------------------------
            'Get the Name of the ModelObject within the FactType
            '------------------------------------------------------
            Select Case Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.ConceptType
                Case Is = pcenumConceptType.EntityType, pcenumConceptType.ValueType
                    ls_joined_object_name = Trim(Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.Name)
                Case Is = pcenumConceptType.FactType
                    ls_joined_object_name = Trim(Me.zrFactType.RoleGroup(liInd - 1).JoinedORMObject.Name)
            End Select

            While liInd2 < aoRichTextBox.TextLength
                If liInd2 <= 0 Then liInd2 = 1

                liInd2 = aoRichTextBox.Find(ls_joined_object_name, (liInd2 - 1), RichTextBoxFinds.WholeWord)
                If liInd2 < 0 Then
                    liInd2 = 1
                    Exit While
                End If

                aoRichTextBox.SelectionColor = Color.Blue
                aoRichTextBox.SelectionProtected = True

                If (aoRichTextBox.SelectionStart + Len(ls_joined_object_name)) >= aoRichTextBox.TextLength Then
                    Exit While
                Else
                    liInd2 = aoRichTextBox.SelectionStart + Len(ls_joined_object_name)
                End If
            End While
        Next

        aoRichTextBox.SelectionStart = aoRichTextBox.TextLength

    End Sub

    Sub load_enterprise_aware_listbox()
        '------------------------------------------------------------
        'Loads the list of ORM Object Type names within the FactType
        '  within the EnterpriseAware listbox.
        '------------------------------------------------------------
        Dim ls_term As String
        Dim liInd As Integer = 0

        Me.ListBox_EnterpriseAware.Items.Clear()

        For Each ls_term In Me.zrTermList
            liInd += 1
            Me.ListBox_EnterpriseAware.Items.Add(New tcomboboxitem(liInd, ls_term))
            Me.ListBox_EnterpriseAware.Hide()
        Next

    End Sub


    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

        Call Me.zrFactType.RemoveFromModel()
        Call Me.zrValueType.RemoveFromModel()

    End Sub

    Private Sub frmAddAttribute_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.Escape Then
            e.Handled = True
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub ListBox_EnterpriseAware_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox_EnterpriseAware.KeyUp

        Dim ObjToSelect As New Object

        If Not (e.KeyCode = Keys.OemPeriod) Then


            Select Case e.KeyCode
                Case Is = Keys.Escape
                    Me.ListBox_EnterpriseAware.Hide()
                    Me.TextboxReading.Focus()
                Case Is = Keys.Back, Keys.Up, Keys.Down
                    '------------
                    'Do nothing
                    '------------
                    Exit Sub
                Case Is = Keys.Return, Keys.Space
                    '------------------------------------------
                    'User is selecting an item from the list.
                    '------------------------------------------
                    Dim lsModelObjectName As String = ListBox_EnterpriseAware.SelectedItem.ToString()

                    '-------------------------------
                    'Remove the item from the list
                    '-------------------------------
                    Me.ListBox_EnterpriseAware.Items.Remove(ListBox_EnterpriseAware.SelectedItem)
                    Me.ListBox_EnterpriseAware.Hide()

                    Me.TextboxReading.Text &= lsModelObjectName & " "

                    Call ProtectFactTypeTerms(Me.TextboxReading)
                    Me.TextboxReading.Focus()
                Case Else
                    Dim lbStartsWith As Boolean = False
                    Dim lrModelObjectName As Object

                    ziIntellisenseBuffer &= e.KeyData.ToString

                    For Each lrModelObjectName In ListBox_EnterpriseAware.Items
                        Dim str As String = lrModelObjectName.ToString()
                        If Not (str = "") Then
                            lbStartsWith = str.StartsWith(ziIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture)
                            If lbStartsWith Then
                                ObjToSelect = lrModelObjectName
                                Exit For
                            End If
                        End If
                    Next

                    If (lbStartsWith = False) Then
                        ziIntellisenseBuffer = ""
                        ListBox_EnterpriseAware.Hide()
                    End If
                    ListBox_EnterpriseAware.SelectedItem = ObjToSelect
            End Select
        End If

    End Sub

    Private Sub ComboBoxObjectType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim lsMessage As String = ""

        lsMessage = "Select [Value Type] for an Attribute that stores a Value and references no Entity (Type)."
        lsMessage &= vbCrLf
        lsMessage &= "Select [Entity Type] for an Attribute that is a Foreign Key Reference to an/other Entity (Type)."

        Me.LabelHelp.Text = lsMessage

        Call Me.SetPictureBoxes()

    End Sub


    Private Sub ComboBoxReferences_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxReferences.GotFocus

        Dim lsMessage As String = ""

        lsMessage = "Select the Entity (Type) that the Attribute references as a Foreign Key Reference"

        Me.LabelHelp.Text = lsMessage

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub CheckBoxIsMandatory_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxIsMandatory.CheckedChanged

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub SetPictureBoxes()



        If Me.ComboBoxReferences.SelectedIndex >= 0 Then
            If ComboBoxReferences.SelectedItem.Text = Me.zrModelObject.Name Then
                Me.PictureBoxValueTypeAttribute.Visible = True
                Me.PictureBoxMultiplicity.Visible = False
            Else
                Me.PictureBoxValueTypeAttribute.Visible = False
                Me.PictureBoxMultiplicity.Visible = True
            End If

            If Me.RadioButtonManyToOne.Checked And ComboBoxReferences.SelectedItem.Text = Me.zrModelObject.Name Then
                Me.PictureBoxValueTypeAttribute.Image = My.Resources.BarkerERImages.AttributeSelfReferenceManyToOne
            ElseIf Me.RadioButtonOneToOne.Checked And ComboBoxReferences.SelectedItem.Text = Me.zrModelObject.Name Then
                Me.PictureBoxValueTypeAttribute.Image = My.Resources.BarkerERImages.AttributeSelfReferenceOneToOne
            ElseIf Me.RadioButtonManyToOne.Checked Then
                Me.PictureBoxMultiplicity.Image = My.Resources.BarkerERImages.ManyToOne
            Else
                Me.PictureBoxMultiplicity.Image = My.Resources.BarkerERImages.OneToOne
            End If

            'If Me.RadioButtonOneToOne.Checked And ComboBoxReferences.SelectedItem.Text = Me.zrModelObject.Name Then
            '    Me.PictureBoxValueTypeAttribute.Image = My.Resources.BarkerERImages.AttributeSelfReferenceOneToOne
            'Else
            '    Me.PictureBoxMultiplicity.Image = My.Resources.BarkerERImages.OneToOne
            'End If
        Else
            Me.PictureBoxValueTypeAttribute.Visible = True
            Me.PictureBoxMultiplicity.Visible = False
        End If

    End Sub

    Private Sub ComboBoxReferences_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxReferences.SelectedIndexChanged

        If Me.ComboBoxReferences.SelectedIndex >= 0 Then
            Me.zrReferencesModelObject = Me.ComboBoxReferences.SelectedItem.Tag
        End If

        Call Me.SetPictureBoxes()

    End Sub

End Class