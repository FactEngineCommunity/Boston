Imports System.Reflection

Public Class frmCRUDAddAttribute

    Public zarAttribute As New List(Of ERD.Attribute) 'The Attribute/s being added to the zrEtntity
    Public zrEntity As ERD.Entity 'The Entity to which the new Attribute will be added.    
    Public zrModel As FBM.Model 'The Model being worked with

    ''' <summary>
    ''' NB 20150319-VM-It is still undecided as to whether to use the ObjectifyingEntityType or FactType for this member
    '''   if the Entity represents a FactType. Likely to use FactType, as the Me.zrFactType has to join to something
    '''   and cannot join to the ObjectifyingEntityType.
    ''' </summary>
    ''' <remarks></remarks>
    Public zrModelObject As FBM.ModelObject 'The EntityType or FactType that the zrEntity represents.
    Public zrValueType As FBM.ValueType 'The ValueType that the Attribute represents (if not an Attribute that references an Entity/EntityType)
    Public zrFactType As FBM.FactType 'The FactType linking the EntityType to the ValueType (if Attribute is not part of a Relation) or EntityType (if is an Attribute that references an Entity/EntityType)
    Public zrReferencesModelObject As Object = Nothing 'Used to return the Entity to which the new (if any) ERD.Relationship will be the DestinationEntity/ModelObject

    '20150319-Delete the following 2, because is now part of zarAttribute.
    Public zbAttributeIsMandatory As Boolean = False 'Used to return whether the Attribute is Mandatory.
    Public zbAttributeIsPartOfPrimaryKey As Boolean = False 'Used to return whether the Attribute is part of the PrimaryKey of the Entity of the Attribute.

    Public zrRelation As New ERD.Relation 'Maintained as the User establishes a Relation, but only used in frmDiagramERD if me.zrReferencesModelObject is not Nothing.

    '---------------------------------------------
    'EnterpriseAware (Intellisense like) code
    '---------------------------------------------
    Public ziCurrentTagStart As Integer = 0
    Public ziIntellisenseBuffer As String = ""
    Private zrHashList As New Hashtable
    Private zrTermList As New List(Of String) 'List of ORMObjectTypes within the FactType for which the reading is being created.
    Public zarEntity As New List(Of FBM.FactDataInstance)

    Private Class tRelationType
        Implements IEquatable(Of tRelationType)

        Public OriginMany As Boolean = False
        Public OriginMandatory As Boolean = False
        Public OriginPartOfPrimaryKey As Boolean = False
        Public DestinationMany As Boolean = False
        Public DestinationMandatory As Boolean = False

        Public Sub New()

        End Sub

        Public Sub New(ByVal abOriginMany As Boolean, ByVal abOriginMandatory As Boolean, ByVal abOriginPartOfPrimaryKey As Boolean, ByVal abDestinationMany As Boolean, ByVal abDestinationMandatory As Boolean)
            Me.OriginMany = abOriginMany
            Me.OriginMandatory = abOriginMandatory
            Me.OriginPartOfPrimaryKey = abOriginPartOfPrimaryKey
            Me.DestinationMany = abDestinationMany
            Me.DestinationMandatory = abDestinationMandatory
        End Sub

        Public Shadows Function Equals(ByVal other As tRelationType) As Boolean Implements System.IEquatable(Of tRelationType).Equals

            If Me.OriginMany = other.OriginMany And _
               Me.OriginMandatory = other.OriginMandatory And _
               Me.OriginPartOfPrimaryKey = other.OriginPartOfPrimaryKey And _
               Me.DestinationMany = other.DestinationMany And _
               Me.DestinationMandatory = other.DestinationMandatory Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function GetHashCode() As Integer

            Return CInt(Me.OriginMany)

        End Function
    End Class

    Private zarRelationTypeDictionary As New Dictionary(Of tRelationType, Integer)

    Private Sub frmAddAttribute_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Dim liInd As Integer
        Dim ls_joined_object_name As String = ""


        Me.BindingSourceAttribute.DataSource = Me.zarAttribute
        Me.zarAttribute.Add(New ERD.Attribute("NewAttribute", Me.zrEntity))
        Me.BindingSourceAttribute.ResetBindings(False)

        '------------------------------
        'Setup the Relationship Types
        '------------------------------
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, True, True, True, True), 1)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, True, True, True, False), 2)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, True, True, False, True), 3)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, True, True, False, False), 4)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, True, False, True, True), 5)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, True, False, True, False), 6)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, True, False, False, True), 7)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, True, False, False, False), 8)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, False, True, True, True), 9)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, False, True, True, False), 10)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, False, True, False, True), 11)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, False, True, False, False), 12)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, False, False, True, True), 13)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, False, False, True, False), 14)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, False, False, False, True), 15)
        Me.zarRelationTypeDictionary.Add(New tRelationType(True, False, False, False, False), 16)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, True, True, True, True), 17)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, True, True, True, False), 18)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, True, True, False, True), 19)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, True, True, False, False), 20)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, True, False, True, True), 21)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, True, False, True, False), 22)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, True, False, False, True), 23)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, True, False, False, False), 24)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, False, True, True, True), 25)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, False, True, True, False), 26)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, False, True, False, True), 27)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, False, True, False, False), 28)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, False, False, True, True), 29)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, False, False, True, False), 30)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, False, False, False, True), 31)
        Me.zarRelationTypeDictionary.Add(New tRelationType(False, False, False, False, False), 32)

        Me.LabelEntityTypeName.Text = Me.zrModelObject.Name

        Dim lsNewUniqueAttributeName As String = ""
        lsNewUniqueAttributeName = Me.zrEntity.CreateUniqueAttributeName("NewAttribute", 0)

        Me.ComboBoxAttribute.Text = lsNewUniqueAttributeName
        Me.ComboBoxAttribute.SelectAll()

        '--------------------------------------------------------------------------------
        'Create a new ValueType (in anticipation of [OK] button press) within the Model
        '--------------------------------------------------------------------------------
        Me.zrValueType = Me.zrModel.CreateValueType(lsNewUniqueAttributeName)        

        '-------------------------------------------------------------------
        'Establish a dummy FactType for the new Attribute.
        '  NB If the User clicks [Cancel] on this form, then the FactType 
        '  and ValueType must be removed from the Model.
        '-------------------------------------------------------------------
        If Me.zrModelObject.ConceptType = pcenumConceptType.EntityType Then
            Dim lrEntityType As FBM.EntityType
            lrEntityType = Me.zrModelObject
            Me.zrFactType = lrEntityType.AddBinaryRelationToValueType(Me.zrValueType, pcenumBinaryRelationMultiplicityType.ManyToOne)
        Else
            Throw New Exception("Not implemented yet")
        End If


        '--------------------------------------------------------------------------------------
        'Populate the AttributeName ComboBox with the list of ValueType/Names for all of the
        '  ValueTypes within the Model, that are not already associated with the Entity.
        '  NB If the User selects one of the ValueTypes, then a FactType will be created
        '  linked to either that ValueType or the Entity to which it is the ReferenceMode.
        '--------------------------------------------------------------------------------------
        Call Me.LoadModelValueTypes(Me.zrModel)

        Call Me.PopulatDataTypes()

        Call Me.PopulateTermList()

        Call Me.PopulateReferences()

        '--------------------------------
        'Setup the FactTypeReading grid
        '--------------------------------
        Dim colRichText As New tRichtextColumn()
        Me.DataGrid_Readings.Columns.Add(colRichText)
        'Me.DataGrid_Readings.Width = Me.Width - (Me.DataGrid_Readings.Left)
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
            ListBox_EnterpriseAware.Items.Add(New tComboboxItem(liInd, ls_joined_object_name))

            If Me.zrHashList.Contains(ls_joined_object_name) Then
                Me.zrHashList(ls_joined_object_name) += 1
            Else
                Me.zrHashList.Add(Trim(ls_joined_object_name), 1)
            End If
        Next

        TextboxReading.DeselectAll()

        Call Me.LoadObjectTypes()

        Me.zrRelation.OriginEntity = Me.zrEntity

        Call Me.SetPictureboxRelation()

        Call Me.UpdateVerbalisationReading()

    End Sub

    Private Sub PopulateReferences()

        Dim lrEntity As ERD.Entity

        For Each lrEntity In Me.zarEntity

            'was os Tag/Object - New FBM.DictionaryEntry(Me.zrFactType.Model, lrEntity.Name, pcenumConceptType.Value)
            Me.ComboBoxReferences.Items.Add(New tComboboxItem(lrEntity.Id, lrEntity.Name, lrEntity))

        Next

    End Sub

    Private Sub PopulatDataTypes()

        ComboBoxDataType.DataSource = [Enum].GetValues(GetType(pcenumDataType))
        ComboBoxDataType.DataSource = [Enum].GetValues(GetType(pcenumDataType))

    End Sub

    Private Sub LoadObjectTypes()

        Me.ComboBoxObjectType.Items.Add(New tComboboxItem("ValueType", "Value", New FBM.DictionaryEntry(Me.zrModel, "ValueType", pcenumConceptType.ValueType)))
        Me.ComboBoxObjectType.Items.Add(New tComboboxItem("EntityType", "Entity Reference", New FBM.DictionaryEntry(Me.zrModel, "EntityType", pcenumConceptType.EntityType)))

        Me.ComboBoxObjectType.SelectedIndex = 0

    End Sub

    Private Sub LoadModelValueTypes(ByVal arModel As FBM.Model)

        Dim lrValueType As FBM.ValueType

        For Each lrValueType In arModel.ValueType
            Me.ComboBoxAttribute.Items.Add(New tComboboxItem(lrValueType.Id, lrValueType.Name, lrValueType))
            Me.ComboBoxAttribute.Items.Add(New tComboboxItem(lrValueType.Id, lrValueType.Name, lrValueType))
        Next

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click


        If Me.CheckFields Then

            Me.zbAttributeIsMandatory = Me.CheckBoxIsMandatory.Checked
            Me.zbAttributeIsPartOfPrimaryKey = Me.CheckBoxIsPrimaryIdentifier.Checked

            If IsSomething(Me.zrReferencesModelObject) Then
                '---------------------------------------------------------------------------------------------
                'The Entity references another Entity.
                '  The associated FactType (i.e. the FBM.FactType associated with the Attribute)
                '  must reference the FBM.Entity represented by the ERD.Entity in the new Relation
                '---------------------------------------------------------------------------------------------
                Dim lrReferencedEntityType As New FBM.EntityType
                lrReferencedEntityType.Id = Me.zrReferencesModelObject.Name
                lrReferencedEntityType = Me.zrModel.EntityType.Find(AddressOf lrReferencedEntityType.Equals)

                Dim lrRole As FBM.Role
                lrRole = Me.zrFactType.FindFirstRoleByModelObject(Me.zrValueType)
                lrRole.ReassignJoinedModelObject(lrReferencedEntityType, True)

                '---------------------------------------------------------------------------------------------
                ' The associated ValueType, created in anticipation of 
                '  the Attribute representing an EntityType referencing that ValueType, must be removed
                '---------------------------------------------------------------------------------------------
                Me.zrValueType.RemoveFromModel()

            Else
                Me.zrValueType.SetName(Me.zarAttribute(0).Name)
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK

            Me.Close()

        End If

    End Sub

    Private Function CheckFields() As Boolean

        CheckFields = True

        ''---------------------------------------------------------------------------------------------------
        ''Check to see whether a ValueType already exists in the Model with the same name as the ValueType
        ''  that will be associated with the Attribute
        ''---------------------------------------------------------------------------------------------------
        'If IsSomething(Me.zrReferencesModelObject) Then

        '    Dim lrValueType As New FBM.ValueType
        '    lrValueType.Id = Me.zarAttribute(0).Name

        '    If Me.zrModel.ValueType.Exists(AddressOf lrValueType.Equals) Then
        '        '------------------------------------------------------------------------------------------
        '        'It is okay that more than one Attribute reference the same ValueType, we simply need to
        '        '  make sure that we are not creating two ValueTypes with the same Id/Name/Symbol
        '        '------------------------------------------------------------------------------------------
        '        Me.zrValueType.RemoveFromModel()
        '        Me.zrValueType = Me.zrModel.ValueType.Find(AddressOf lrValueType.Equals)
        '    End If

        'End If

    End Function

    Private Sub RadioButtonManyToOne_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonManyToOne.CheckedChanged

        If IsSomething(Me.zrModelObject) Then
            Call Me.UpdateVerbalisationReading()
            Call Me.zrFactType.CreateManyToOneInternalUniquenessConstraint(Me.zrModelObject)
        End If

        Me.CheckBoxIsPrimaryIdentifier.Enabled = False
        Me.CheckBoxIsPrimaryIdentifier.Checked = False

        My.Resources.BarkerERImages.ManyToOne.MakeTransparent(Color.White)
        Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages.ManyToOne

        Call Me.SetPictureBoxes()


    End Sub

    Private Sub RadioButtonOneToOne_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonOneToOne.CheckedChanged

        Try
            Call Me.UpdateVerbalisationReading()
            Call Me.zrFactType.CreateOneToOneInternalUniquenessConstraint(True)
            CheckBoxIsPrimaryIdentifier.Enabled = True

            Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages.OneToOne

            Call Me.SetPictureBoxes()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub UpdateVerbalisationReading()

        Dim lsMessage As String = ""

        If RadioButtonManyToOne.Checked Then
            lsMessage = "Values of Attribute, '" & Me.ComboBoxAttribute.Text & "', are not unique to a specific instance of Entity, '" & Me.zrEntity.Name & "'"
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "Each [" & Me.zrModelObject.Name & "]"
            lsMessage &= " associates with at most one "
            lsMessage &= "[" & Me.ComboBoxAttribute.Text & "]"
            lsMessage &= vbCrLf
            lsMessage &= "Each [" & Me.ComboBoxAttribute.Text & "]"
            lsMessage &= " associates with many "
            lsMessage &= "[" & Me.zrModelObject.Name & "/s]"
        Else
            lsMessage = "Values of Attribute, '" & Me.ComboBoxAttribute.Text & "', are unique to a specific instance of Entity, '" & Me.zrEntity.Name & "'"
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "Each [" & Me.zrModelObject.Name & "]"
            lsMessage &= " associates with at most one "
            lsMessage &= "[" & Me.ComboBoxAttribute.Text & "]"
            lsMessage &= vbCrLf
            lsMessage &= "Each [" & Me.ComboBoxAttribute.Text & "]"
            lsMessage &= " associates with at most one "
            lsMessage &= "[" & Me.zrModelObject.Name & "]"
        End If

        Me.LabelMultiplicityVerbalisation.Text = lsMessage

    End Sub

    Private Sub ComboBoxAttribute_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

        'Me.zrValueType(Me.BindingSourceAttribute.IndexOf(Me.BindingSourceAttribute.Current)).Name = Me.ComboBoxAttribute.Text
        'Me.zrFactType(Me.BindingSourceAttribute.IndexOf(Me.BindingSourceAttribute.Current)).Name = Viev.strings.RemoveWhiteSpace(Me.zrModelObject.Name & Me.ComboBoxAttribute.Text)
        Call Me.PopulateTermList()

    End Sub

    Private Sub ComboBoxAttribute_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Call Me.UpdateVerbalisationReading()

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

        Call LoadEnterpriseAwareListbox()

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
            Me.ListBox_EnterpriseAware.Items.Add(New tComboboxItem(liInd, lsJoinedObjectName))
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
        Dim lrPredicatePart As New FBM.PredicatePart(arFactTypeReading.Model, arFactTypeReading)

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
                    'lrPredicatePart.ObjectType1 = New FBM.ModelObject(lsWord)
                ElseIf (liSequenceNr = 2) Then
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
        '---------------------------------------------------------------------------------------
        'Not supported in v1.13 of the database model.
        'arFactTypeReading.ObjectTypeList = lr_orm_ObjectTypeList

    End Sub

    Private Sub TextboxReading_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxReading.GotFocus

        Dim lsMessage As String = ""

        lsMessage = "Enter a Reading for the new Attribute."
        lsMessage &= " e.g. '" & Me.zrModelObject.Name & " has " & Me.ComboBoxAttribute.Text & "'"
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
            Dim liAttributeIndex As Integer = (Me.BindingSourceAttribute.IndexOf(Me.BindingSourceAttribute.Current))
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
            Call LoadEnterpriseAwareListbox()
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

        Dim liAttributeIndex As Integer = (Me.BindingSourceAttribute.IndexOf(Me.BindingSourceAttribute.Current))

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

    Sub LoadEnterpriseAwareListbox()
        '------------------------------------------------------------
        'Loads the list of ORM Object Type names within the FactType
        '  within the EnterpriseAware listbox.
        '------------------------------------------------------------
        Dim ls_term As String
        Dim liInd As Integer = 0

        Me.ListBox_EnterpriseAware.Items.Clear()

        For Each ls_term In Me.zrTermList
            liInd += 1
            Me.ListBox_EnterpriseAware.Items.Add(New tComboboxItem(liInd, ls_term))
            Me.ListBox_EnterpriseAware.Hide()
        Next

    End Sub


    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

        Me.zrFactType.RemoveFromModel()
        Me.zrValueType.RemoveFromModel()

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

    Private Sub ComboBoxObjectType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxObjectType.GotFocus

        Dim lsMessage As String = ""

        lsMessage = "Select [Value Type] for an Attribute that stores a Value and references no Entity (Type)."
        lsMessage &= vbCrLf
        lsMessage &= "Select [Entity Type] for an Attribute that is a Foreign Key Reference to an/other Entity (Type)."

        Me.LabelHelp.Text = lsMessage

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub ComboBoxObjectType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxObjectType.SelectedIndexChanged


        If Me.ComboBoxObjectType.SelectedItem.ItemData = "EntityType" Then
            Me.ComboBoxReferences.Visible = True
            Me.LabelValueTypeDescription.Visible = False
            Me.PictureBoxRelationType.Visible = True
            Me.GroupBoxRelationshipType.Enabled = True
        Else
            Me.ComboBoxReferences.Visible = False
            Me.ComboBoxReferences.SelectedIndex = -1
            Me.LabelValueTypeDescription.Visible = True            
            Me.GroupBoxRelationshipType.Enabled = False

            Me.zarAttribute.Clear()
            Me.zarAttribute.Add(New ERD.Attribute("NewAttribute", Me.zrEntity))
            '-------------------------------------------------------------------------------------------------------------
            'NB A new Attribute has its 'Relation' member set to 'Nothing' and property 'IsPartofRelation' returns False
            '-------------------------------------------------------------------------------------------------------------


            Me.BindingSourceAttribute.ResetBindings(False)
        End If

        Call Me.SetPictureboxRelation()

        Dim lsRelatedEntity As String = "<None selected>"
        Me.LabelPrompRelationDescription.Text = "The relationship between Entity, '" & Me.zrEntity.Name & "', and Entity, '" & lsRelatedEntity & "', has these qualities :"

        Dim hint As ToolTip = New ToolTip()
        hint.IsBalloon = True
        Dim lsMessage As String = "Select the referenced Entity here."
        hint.ToolTipIcon = ToolTipIcon.None
        hint.Show(lsMessage, Me.ComboBoxReferences)
        hint.Show(lsMessage, Me.ComboBoxReferences, 3000)

    End Sub

    Private Sub ComboBoxDataType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.LabelHelp.Text = "Select the Data Type of the Attribute"

    End Sub

    Private Sub ComboBoxDataType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim liAttributeIndex As Integer = (Me.BindingSourceAttribute.IndexOf(Me.BindingSourceAttribute.Current))

        Me.zrValueType.DataType = ComboBoxDataType.Text

    End Sub

    Private Sub CheckBoxIsPrimaryIdentifier_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxIsPrimaryIdentifier.CheckedChanged

        If CheckBoxIsPrimaryIdentifier.Checked Then

            Me.CheckBoxIsMandatory.Checked = True
            Me.CheckBoxIsMandatory.Enabled = False

            If Me.zrModelObject.GetType Is GetType(FBM.EntityType) Then

                If Me.ComboBoxObjectType.SelectedItem.ItemData = "ValueType" Then

                    Dim lrEntityType As FBM.EntityType

                    Dim liAttributeIndex As Integer = (Me.BindingSourceAttribute.IndexOf(Me.BindingSourceAttribute.Current))

                    lrEntityType = Me.zrModelObject
                    lrEntityType.ReferenceMode = Me.ComboBoxAttribute.Text
                    lrEntityType.ReferenceModeValueType = Me.zrValueType
                    lrEntityType.ReferenceModeFactType = Me.zrFactType
                    lrEntityType.ReferenceModeFactType.IsPreferredReferenceMode = True


                    '----------------------------------------------------------------
                    'Create the UniquenessConstraints for the ReferenceModeFactType
                    '----------------------------------------------------------------
                    Call lrEntityType.ReferenceModeFactType.RemoveInternalUniquenessConstraints(True)

                    Dim larRole As New List(Of FBM.Role)
                    larRole.Add(lrEntityType.ReferenceModeFactType.FindFirstRoleByModelObject(lrEntityType))
                    lrEntityType.ReferenceModeFactType.CreateInternalUniquenessConstraint(larRole)

                    Dim lrRoleConstraint As FBM.RoleConstraint
                    larRole.Clear()
                    larRole.Add(lrEntityType.ReferenceModeFactType.FindFirstRoleByModelObject(lrEntityType.ReferenceModeValueType))
                    lrRoleConstraint = lrEntityType.ReferenceModeFactType.CreateInternalUniquenessConstraint(larRole, True)

                    lrEntityType.ReferenceModeRoleConstraint = lrRoleConstraint

                End If

            End If
        Else
            Me.CheckBoxIsMandatory.Enabled = True
        End If

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub ComboBoxReferences_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxReferences.GotFocus

        Dim lsMessage As String = ""

        lsMessage = "Select the Entity (Type) that the Attribute references as a Foreign Key Reference"

        Me.LabelHelp.Text = lsMessage

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub CheckBoxIsMandatory_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub SetPictureboxAttribute()

        If Me.CheckBoxIsMandatory.Checked And Me.CheckBoxIsPrimaryIdentifier.Checked Then
            Me.PictureBoxAttribute.Image = My.Resources.BarkerERImages.AttributePrimaryKey
        ElseIf Me.CheckBoxIsMandatory.Checked Then
            Me.PictureBoxAttribute.Image = My.Resources.BarkerERImages.AttributeMandatory
        Else
            Me.PictureBoxAttribute.Image = My.Resources.BarkerERImages.AttributeOptional
        End If

    End Sub

    Private Sub SetPictureBoxes()

        If Me.ComboBoxObjectType.SelectedItem Is Nothing Then
            '---------------------
            'Setting up the form
            '---------------------
            Exit Sub
        End If

        If Me.ComboBoxObjectType.SelectedItem.ItemData = "EntityType" Then
            Call Me.SetPictureboxRelation()
        Else
            '-----------
            'ValueType
            '-----------            
            Me.PictureBoxRelationType.Enabled = False

        End If

    End Sub

    Private Sub CheckBoxIsPartOfPrimaryKey_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.CheckBoxIsPrimaryIdentifier.Checked = False

        Call Me.SetPictureBoxes()

    End Sub

    Private Sub ComboBoxReferences_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxReferences.SelectedIndexChanged

        Dim lsMessage As String = ""

        If Me.ComboBoxReferences.SelectedIndex >= 0 Then
            Me.zrReferencesModelObject = New Object
            Me.zrReferencesModelObject = Me.ComboBoxReferences.SelectedItem.Tag
            Me.LabelPromptLinksToAttribute.Enabled = True
            Me.LabelPromptOfReferencedEntity.Enabled = True
            Me.ComboBoxKeyAttributes.Enabled = True

            Call LoadKeyAttributes(Me.ComboBoxReferences.SelectedItem.Tag)
            LabelPromptOfReferencedEntity.Text = "...of Entity, '" & Trim(ComboBoxReferences.Text) & "'"

            Dim lsRelatedEntity As String = Me.ComboBoxReferences.Text
            lsMessage = "The relationship between Entity, '" & Me.zrEntity.Name & "', and Entity, '" & lsRelatedEntity & "', has these qualities :"
            Me.LabelPrompRelationDescription.Text = lsMessage

            Dim hint As ToolTip = New ToolTip()
            hint.IsBalloon = True
            lsMessage = "Set the referenced Attribute here."
            hint.ToolTipIcon = ToolTipIcon.None
            hint.Show(lsMessage, Me.ComboBoxKeyAttributes)
            hint.Show(lsMessage, Me.ComboBoxKeyAttributes, 5000)

            Me.zrRelation.DestinationEntity = Me.ComboBoxReferences.SelectedItem.Tag
        Else
            Me.zrReferencesModelObject = Nothing
        End If

        Call Me.SetPictureBoxes()
        Call Me.UpdateVerbalisationReading()

    End Sub

    Private Sub LoadKeyAttributes(ByVal arEntity As ERD.Entity)

        Dim lrKeyAttribute As ERD.Attribute

        Me.ComboBoxKeyAttributes.Items.Clear()
        Me.zarAttribute.Clear()

        For Each lrKeyAttribute In arEntity.PrimaryKey
            Me.ComboBoxKeyAttributes.Items.Add(New tComboboxItem(lrKeyAttribute.Id, lrKeyAttribute.Name, lrKeyAttribute))

            Dim lrClonedKeyAttribute As ERD.Attribute
            lrClonedKeyAttribute = lrKeyAttribute.Clone
            lrClonedKeyAttribute.ReferencesAttribute = lrKeyAttribute
            Me.zarAttribute.Add(lrClonedKeyAttribute)
        Next

        If Me.ComboBoxKeyAttributes.Items.Count = 1 Then
            Me.ComboBoxKeyAttributes.SelectedIndex = 0
        End If

        Me.BindingSourceAttribute.ResetBindings(False)

    End Sub

    Private Sub ComboBoxKeyAttributes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxKeyAttributes.SelectedIndexChanged

        LabelPromptOfReferencedEntity.Text = "...of Entity, '" & Trim(ComboBoxReferences.Text) & "'"
    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRelationContributesToPrimaryKey.CheckedChanged

        If Me.CheckBoxRelationContributesToPrimaryKey.Checked Then
            Me.CheckBoxRelationOriginMandatory.Checked = True
            Me.CheckBoxIsPrimaryIdentifier.Checked = True
        End If

        Me.zrRelation.OriginContributesToPrimaryKey = Me.CheckBoxRelationContributesToPrimaryKey.Checked

        Call Me.SetPictureboxRelation()

    End Sub

    Private Sub SetPictureboxRelation()

        Dim lrRelationType As New tRelationType

        Try

            If Not Me.GroupBoxRelationshipType.Enabled Then
                Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages.DefaultGrayedTransparentAll
                Exit Sub
            End If

            lrRelationType.OriginMandatory = Me.CheckBoxRelationOriginMandatory.Checked
            lrRelationType.OriginMany = Me.CheckBoxRelationOriginMany.Checked
            lrRelationType.OriginPartOfPrimaryKey = Me.CheckBoxRelationContributesToPrimaryKey.Checked
            lrRelationType.DestinationMandatory = Me.CheckBoxRelationDestinationMandatory.Checked
            lrRelationType.DestinationMany = Me.CheckBoxRelationDestinationMany.Checked

            Dim liDrawingNumber As Integer = 0

            liDrawingNumber = Me.zarRelationTypeDictionary(lrRelationType)

            Select Case liDrawingNumber
                Case Is = 1
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._1
                Case Is = 2
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._2
                Case Is = 3
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._3
                Case Is = 4
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._4
                Case Is = 5
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._5
                Case Is = 6
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._6
                Case Is = 7
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._7
                Case Is = 8
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._8
                Case Is = 9
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._9
                Case Is = 10
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._10
                Case Is = 11
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._11
                Case Is = 12
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._12
                Case Is = 13
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._13
                Case Is = 14
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._14
                Case Is = 15
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._15
                Case Is = 16
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._16
                Case Is = 17
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._17
                Case Is = 18
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._18
                Case Is = 19
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._19
                Case Is = 20
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._20
                Case Is = 21
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._21
                Case Is = 22
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._22
                Case Is = 23
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._23
                Case Is = 24
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._24
                Case Is = 25
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._25
                Case Is = 26
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._26
                Case Is = 27
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._27
                Case Is = 28
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._28
                Case Is = 29
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._29
                Case Is = 30
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._30
                Case Is = 31
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._31
                Case Is = 32
                    Me.PictureBoxRelationType.Image = My.Resources.BarkerERImages._32
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub CheckBoxRelationOriginMandatory_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRelationOriginMandatory.CheckedChanged

        Dim lsMessage As String = ""
        Dim lrRelationType As New tRelationType

        lrRelationType.OriginMandatory = Me.CheckBoxRelationOriginMandatory.Checked
        lrRelationType.OriginMany = Me.CheckBoxRelationOriginMany.Checked
        lrRelationType.OriginPartOfPrimaryKey = Me.CheckBoxRelationContributesToPrimaryKey.Checked
        lrRelationType.DestinationMandatory = Me.CheckBoxRelationDestinationMandatory.Checked
        lrRelationType.DestinationMany = Me.CheckBoxRelationDestinationMany.Checked

        If Not Me.CheckRelationValidity(lrRelationType) Then
            Me.CheckBoxRelationOriginMandatory.Checked = False
        End If

        '----------------------------------------------------------
        'Don't allow the User to set the OriginMandatory to False
        ' if the OriginContributesToPrimaryKey is True
        '----------------------------------------------------------
        If Not Me.CheckBoxRelationOriginMandatory.Checked And _
               Me.CheckBoxRelationContributesToPrimaryKey.Checked Then
            Me.CheckBoxRelationOriginMandatory.Checked = True
            Exit Sub
        End If

        If Me.CheckBoxRelationOriginMandatory.Checked Then
            Me.CheckBoxIsMandatory.Checked = True
        End If

        Me.zrRelation.OriginMandatory = Me.CheckBoxRelationOriginMandatory.Checked

        Call Me.SetPictureboxRelation()

    End Sub

    Private Sub CheckBoxRelationOriginMany_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRelationOriginMany.CheckedChanged

        Dim lrRelationType As New tRelationType

        lrRelationType.OriginMandatory = Me.CheckBoxRelationOriginMandatory.Checked
        lrRelationType.OriginMany = Me.CheckBoxRelationOriginMany.Checked
        lrRelationType.OriginPartOfPrimaryKey = Me.CheckBoxRelationContributesToPrimaryKey.Checked
        lrRelationType.DestinationMandatory = Me.CheckBoxRelationDestinationMandatory.Checked
        lrRelationType.DestinationMany = Me.CheckBoxRelationDestinationMany.Checked

        If Not Me.CheckRelationValidity(lrRelationType) Then
            Me.CheckBoxRelationOriginMany.Checked = False
        End If

        If Me.CheckBoxRelationOriginMany.Checked Then
            Me.zrRelation.OriginMultiplicity = pcenumCMMLMultiplicity.Many
        Else
            Me.zrRelation.OriginMultiplicity = pcenumCMMLMultiplicity.One            
        End If

        Call Me.SetPictureboxRelation()

    End Sub

    Private Sub CheckBoxRelationDestinationMandatory_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRelationDestinationMandatory.CheckedChanged

        Dim lrRelationType As New tRelationType

        lrRelationType.OriginMandatory = Me.CheckBoxRelationOriginMandatory.Checked
        lrRelationType.OriginMany = Me.CheckBoxRelationOriginMany.Checked
        lrRelationType.OriginPartOfPrimaryKey = Me.CheckBoxRelationContributesToPrimaryKey.Checked
        lrRelationType.DestinationMandatory = Me.CheckBoxRelationDestinationMandatory.Checked
        lrRelationType.DestinationMany = Me.CheckBoxRelationDestinationMany.Checked

        If Not Me.CheckRelationValidity(lrRelationType) Then
            Me.CheckBoxRelationDestinationMandatory.Checked = False
            Exit Sub
        End If


        Me.zrRelation.DestinationMandatory = Me.CheckBoxRelationDestinationMandatory.Checked

        Call Me.SetPictureboxRelation()
    End Sub

    Private Sub CheckBoxRelationDestinationMany_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRelationDestinationMany.CheckedChanged

        Dim lrRelationType As New tRelationType

        lrRelationType.OriginMandatory = Me.CheckBoxRelationOriginMandatory.Checked
        lrRelationType.OriginMany = Me.CheckBoxRelationOriginMany.Checked
        lrRelationType.OriginPartOfPrimaryKey = Me.CheckBoxRelationContributesToPrimaryKey.Checked
        lrRelationType.DestinationMandatory = Me.CheckBoxRelationDestinationMandatory.Checked
        lrRelationType.DestinationMany = Me.CheckBoxRelationDestinationMany.Checked

        If Not Me.CheckRelationValidity(lrRelationType) Then
            Me.CheckBoxRelationDestinationMany.Checked = False
            Exit Sub
        End If

        If Me.CheckBoxRelationDestinationMany.Checked Then
            Me.zrRelation.DestinationMultiplicity = pcenumCMMLMultiplicity.Many
        Else
            Me.zrRelation.DestinationMultiplicity = pcenumCMMLMultiplicity.One
        End If

        Call Me.SetPictureboxRelation()
    End Sub


    Private Sub CheckBoxIsMandatory_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxIsMandatory.CheckedChanged

        Dim lsMessage As String = ""

        If Me.CheckBoxRelationOriginMandatory.Checked = True And _
           Me.CheckBoxIsMandatory.Checked = False Then

            lsMessage = "If the relation between Entity, '" & Me.zrEntity.Name & "', and Entity, '" & Me.ComboBoxReferences.Text & "'"
            lsMessage &= " is Mandatory for Entity, '" & Me.zrEntity.Name & "', then the Attribute, '" & Me.ComboBoxAttribute.Text & "',"
            lsMessage &= " must be Mandatory."
            MsgBox(lsMessage)

            Me.CheckBoxIsMandatory.Checked = True
        End If

        Call Me.SetPictureboxAttribute()

    End Sub

    Private Sub CheckBoxIsPrimaryIdentifier_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxIsPrimaryIdentifier.CheckedChanged
        Call Me.SetPictureboxAttribute()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerStartTip.Tick

        Dim hint As ToolTip = New ToolTip()
        hint.IsBalloon = True
        Dim lsMessage As String = "Set the Attribute Details here"
        lsMessage &= vbCrLf & "OR"
        lsMessage &= vbCrLf & "Change the 'Attribute Type' to 'Entity Reference' to reference an Attribute of another Entity."
        hint.ToolTipIcon = ToolTipIcon.None
        hint.Show(lsMessage, Me.ComboBoxAttribute)
        hint.Show(lsMessage, Me.ComboBoxAttribute, 5000)

        Me.TimerStartTip.Enabled = False

    End Sub

    Private Sub BindingSourceAttribute_CurrentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BindingSourceAttribute.CurrentChanged

        Dim lrAttribute As ERD.Attribute

        lrAttribute = Me.BindingSourceAttribute.Current

        If IsSomething(Me.BindingSourceAttribute.Current) Then
            If IsSomething(lrAttribute.ReferencesAttribute) Then
                ComboBoxKeyAttributes.SelectedIndex = ComboBoxKeyAttributes.FindString(lrAttribute.ReferencesAttribute.Name)
            End If
        Else
            '--------------------------
            'Refreshing BindingSource
            '--------------------------
        End If
        

    End Sub

    Private Function CheckRelationValidity(ByVal arRelationType As tRelationType) As Boolean

        Dim lsMessage As String = ""

        CheckRelationValidity = True

        Select Case Me.zarRelationTypeDictionary(arRelationType)
            Case Is = 1, 5
                '--------------------------------------------------------------------
                'Mandatory to Mandatory implies the EntityA cannot have an instance
                '  without EntityB having and instance and vice versa
                '--------------------------------------------------------------------
                CheckRelationValidity = False

                lsMessage = "You cannot have a Many to Many, Mandatory to Mandatory relationship."
                lsMessage &= vbCrLf & "This relationship implies that EntityA cannot have an instance without EntityB having and instance and vice versa"
                MsgBox(lsMessage)
            Case Is = 9, 10, 11, 12, 25, 26, 27, 28
                '----------------------------------------------------------------------------------
                'If a Relation contributes to the PrimaryKey, then the Relation must be mandatory
                '----------------------------------------------------------------------------------
                CheckRelationValidity = False

                lsMessage = "If a Relation contributes to the PrimaryKey, then the Relation must be Mandatory."                
                MsgBox(lsMessage)
        End Select

    End Function

    ' ''' <summary>
    ' ''' Original AddAttribute method from frmDiagramERD
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub AddAttributeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAttributeToolStripMenuItem.Click

    '    Dim lrAddAttributeForm As New frmCRUDAddAttribute
    '    Dim lrEntityType As FBM.EntityType
    '    Dim lrTableNode As ERD.TableNode = Me.Diagram.Selection.Items(0)
    '    Dim lrEntity As New ERD.Entity
    '    Dim lrFactInstanceAttribute As FBM.FactInstance
    '    Dim lrFactInstanceRelation As FBM.FactInstance = Nothing
    '    Dim lsSQLQuery As String = ""

    '    '---------------------------------------------------------
    '    'Get the EntityType represented by the (selected) Entity
    '    '---------------------------------------------------------
    '    lrEntity = lrTableNode.Tag '(above) = Me.Diagram.Selection.Items(0)

    '    lrEntityType = New FBM.EntityType(Me.zrPage.Model, pcenumLanguage.ORMModel, lrEntity.Name, Nothing, True)
    '    lrEntityType = Me.zrPage.Model.EntityType.Find(AddressOf lrEntityType.Equals)

    '    lrAddAttributeForm.zrModel = Me.zrPage.Model
    '    lrAddAttributeForm.zrModelObject = lrEntityType
    '    lrAddAttributeForm.zrEntity = lrEntity
    '    lrAddAttributeForm.zarEntity = Me.zrPage.ERDiagram.Entity

    '    lrAddAttributeForm.StartPosition = FormStartPosition.CenterParent

    '    If lrAddAttributeForm.ShowDialog = Windows.Forms.DialogResult.OK Then

    '        Dim lrERAttribute As ERD.Attribute

    '        '----------------------------------------------------------------------------
    '        'Relationship processing.
    '        '  NB Do this first, because we need lrFactInstanceRelation for when 
    '        '  mapping the relationship between an Attribute and its associated Relation.
    '        '----------------------------------------------------------------------------
    '        If IsSomething(lrAddAttributeForm.zrReferencesModelObject) Then

    '            Dim lrLink As ERD.Link

    '            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
    '            lsSQLQuery &= " (Relation, FactType)"
    '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '            lsSQLQuery &= " VALUES ("
    '            lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '            lsSQLQuery &= ",'" & lrAddAttributeForm.zrFactType.Name & "'"
    '            lsSQLQuery &= " )"

    '            '20150615-VM-Must Fix
    '            'lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreERDRelation.ToString
    '            lsSQLQuery &= " (ModelObject, Relation)"
    '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '            lsSQLQuery &= " VALUES ("
    '            lsSQLQuery &= "'" & lrEntity.Name & "'"
    '            lsSQLQuery &= ",'" & lrAddAttributeForm.zrReferencesModelObject.Name & "'"
    '            lsSQLQuery &= " )"

    '            lrFactInstanceRelation = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '            If lrAddAttributeForm.zrRelation.OriginMandatory Then
    '                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
    '                lsSQLQuery &= " (" & pcenumCMMLRelations.CoreOriginIsMandatory.ToString & ")"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                lsSQLQuery &= " )"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '            End If

    '            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString
    '            lsSQLQuery &= " (ERDRelation, Multiplicity)"
    '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '            lsSQLQuery &= " VALUES ("
    '            lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '            lsSQLQuery &= ",'" & lrAddAttributeForm.zrRelation.OriginMultiplicity.ToString & "'"
    '            lsSQLQuery &= " )"

    '            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '            If lrAddAttributeForm.zrRelation.DestinationMandatory Then
    '                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
    '                lsSQLQuery &= " (" & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString & ")"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                lsSQLQuery &= " )"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '            End If

    '            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
    '            lsSQLQuery &= " (ERDRelation, Multiplicity)"
    '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '            lsSQLQuery &= " VALUES ("
    '            lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '            lsSQLQuery &= ",'" & lrAddAttributeForm.zrRelation.DestinationMultiplicity.ToString & "'"
    '            lsSQLQuery &= " )"

    '            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '            If lrAddAttributeForm.zrRelation.OriginContributesToPrimaryKey Then
    '                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString
    '                lsSQLQuery &= " (" & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString & ")"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                lsSQLQuery &= " )"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '            End If

    '            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '            lrLink = New ERD.Link(Me.zrPage, _
    '                                  lrFactInstanceRelation, _
    '                                  lrEntity, _
    '                                  lrAddAttributeForm.zrReferencesModelObject, _
    '                                  Nothing, _
    '                                  Nothing, _
    '                                  lrAddAttributeForm.zrRelation)
    '            lrLink.Relation = lrAddAttributeForm.zrRelation

    '            lrLink.DisplayAndAssociate()

    '        End If

    '        '===================================================
    '        'Process the Attribute/s
    '        '===================================================
    '        For Each lrERAttribute In lrAddAttributeForm.zarAttribute

    '            '---------------------------------------------------
    '            'The String put in the TableNode for the Attribute
    '            '---------------------------------------------------
    '            Dim lsAttribute As String = ""

    '            lsSQLQuery = "INSERT INTO ERDAttribute"
    '            lsSQLQuery &= " (ModelObject, Attribute)"
    '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '            lsSQLQuery &= " VALUES ("
    '            lsSQLQuery &= "'" & lrEntity.Name & "'"
    '            lsSQLQuery &= ",'" & lrERAttribute.Name & "'"
    '            lsSQLQuery &= " )"

    '            lrFactInstanceAttribute = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '            '--------------------------------------------------
    '            'Set the Ordinal Position of the Attribute
    '            '--------------------------------------------------
    '            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
    '            lsSQLQuery &= " (Property, Position)"
    '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '            lsSQLQuery &= " VALUES ("
    '            lsSQLQuery &= "'" & lrFactInstanceAttribute.Id & "'"
    '            lsSQLQuery &= ",'" & (lrEntity.Attribute.Count + 1).ToString & "'"
    '            lsSQLQuery &= " )"

    '            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '            Dim lrFactDataInstance As FBM.FactDataInstance
    '            lrFactDataInstance = lrFactInstanceAttribute.GetFactDataInstanceByRoleName("Attribute")

    '            lrERAttribute = lrFactDataInstance.CloneAttribute(Me.zrPage)

    '            lrERAttribute.OrdinalPosition = lrEntity.Attribute.Count + 1
    '            lrERAttribute.Entity = New ERD.Entity
    '            lrERAttribute.Entity = lrEntity
    '            lrEntity.Attribute.Add(lrERAttribute)

    '            If lrAddAttributeForm.zbAttributeIsMandatory And lrAddAttributeForm.zbAttributeIsPartOfPrimaryKey Then
    '                lsAttribute = "# * " & lrERAttribute.Name
    '                lrERAttribute.Mandatory = True
    '                lrERAttribute.PartOfPrimaryKey = True
    '                lrEntity.PrimaryKey.Add(lrERAttribute)
    '            ElseIf lrAddAttributeForm.zbAttributeIsMandatory Then
    '                lsAttribute = "* " & lrERAttribute.Name
    '                lrERAttribute.Mandatory = True
    '            Else
    '                lsAttribute = "o " & lrERAttribute.Name
    '            End If

    '            lrTableNode.AddRow()
    '            lrTableNode.Item(0, lrTableNode.RowCount - 1).Text = " " & lsAttribute

    '            lrTableNode.Item(0, lrTableNode.RowCount - 1).Tag = lrERAttribute
    '            lrERAttribute.Cell = lrTableNode.Item(0, lrTableNode.RowCount - 1)
    '            lrTableNode.ResizeToFitText(False)

    '            If lrERAttribute.Mandatory Then

    '                lsSQLQuery = "INSERT INTO IsMandatory"
    '                lsSQLQuery &= " (IsMandatory)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceAttribute.Id & "'"
    '                lsSQLQuery &= " )"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '            End If

    '            '==========================================================
    '            'Part of PrimaryKey

    '            lsSQLQuery = "SELECT *"
    '            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
    '            lsSQLQuery &= " WHERE Entity = '" & lrEntity.Id & "'"

    '            Dim lrRecordset As ORMQL.Recordset
    '            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '            Dim lrFactInstance As FBM.FactInstance

    '            Dim lsIndexName As String = ""
    '            lsIndexName = Viev.Strings.RemoveWhiteSpace(lrEntity.Id & "PK")

    '            If lrERAttribute.PartOfPrimaryKey Then

    '                If lrRecordset.Facts.Count = 0 Then
    '                    '-------------------------------------------------
    '                    'Must create a Primary Identifier for the Entity
    '                    '-------------------------------------------------
    '                    lsSQLQuery = "INSERT INTO "
    '                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsForEntity.ToString
    '                    lsSQLQuery &= " (Entity, Index)"
    '                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                    lsSQLQuery &= " VALUES ("
    '                    lsSQLQuery &= "'" & lrEntity.Id & "'"
    '                    lsSQLQuery &= ",'" & lsIndexName & "'"
    '                    lsSQLQuery &= ")"

    '                    lrFactInstance = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '                End If

    '                '-------------------------------------
    '                'Add the Attribute to the PrimaryKey
    '                '-------------------------------------
    '                lsSQLQuery = "INSERT INTO "
    '                lsSQLQuery &= pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
    '                lsSQLQuery &= " (Index, Property)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lsIndexName & "'"
    '                lsSQLQuery &= ",'" & lrERAttribute.Name & "'"
    '                lsSQLQuery &= ")"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '            End If
    '            '==========================================================

    '            If IsSomething(lrAddAttributeForm.zrReferencesModelObject) Then
    '                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelation.ToString
    '                lsSQLQuery &= " (Attribute, Relation)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceAttribute.Id & "'"
    '                lsSQLQuery &= ",'" & lrFactInstanceRelation.Id & "'"
    '                lsSQLQuery &= " )"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '            End If
    '        Next

    '    End If

    '    lrAddAttributeForm.Dispose()

    'End Sub

    ' ''' <summary>
    ' ''' Original AddRelationship code from frmDiagramERD
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub AddRelationshipToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddRelationshipToolStripMenuItem.Click

    '    Dim lrAddAttributeForm As New frmCRUDAddRelationship
    '    Dim lrEntityType As FBM.EntityType
    '    Dim lrTableNode As MindFusion.Diagramming.TableNode = Me.Diagram.Selection.Items(0)
    '    Dim lrEntity As New ERD.Entity
    '    Dim lsSQLQuery As String = ""

    '    '---------------------------------------------------------
    '    'Get the EntityType represented by the (selected) Entity
    '    '---------------------------------------------------------
    '    lrEntity = lrTableNode.Tag

    '    lrEntityType = New FBM.EntityType(Me.zrPage.Model, pcenumLanguage.ORMModel, lrEntity.Name, Nothing, True)
    '    lrEntityType = Me.zrPage.Model.EntityType.Find(AddressOf lrEntityType.Equals)

    '    lrAddAttributeForm.zrModel = Me.zrPage.Model
    '    lrAddAttributeForm.zrModelObject = lrEntityType
    '    lrAddAttributeForm.zrEntity = lrEntity
    '    lrAddAttributeForm.zarEntity = Me.zrPage.ERDiagram.Entity

    '    lrAddAttributeForm.StartPosition = FormStartPosition.CenterParent

    '    If lrAddAttributeForm.ShowDialog = Windows.Forms.DialogResult.OK Then

    '    End If

    'End Sub

End Class