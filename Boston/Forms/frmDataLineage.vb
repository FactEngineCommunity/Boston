Imports System.ComponentModel
Imports System.Reflection
Imports MindFusion.Diagramming
Imports MindFusion.Drawing

Public Class frmDataLineage

    Public mrModel As FBM.Model
    Public mrModelElement As FBM.ModelObject

    Dim mrDataLineageItem As New DataLineage.DataLineageItem

    Private marLineageCategory As New List(Of DataLineage.DataLineageCategory)

    Private marDataLineageItemProperty As New List(Of DataLineage.DataLineageItemProperty)

    Private _msDataLineageItemName As String = Nothing
    Private Property msDataLineageItemName As String
        Get
            If Me.mrModelElement Is Nothing Then
                Return Me._msDataLineageItemName
            Else
                Select Case Me.mrModelElement.GetType
                    Case Is = GetType(FBM.ValueType),
                                 GetType(FBM.EntityType)
                        Return Me.mrModelElement.Id & " - Object Type"
                    Case Is = GetType(FBM.FactType)
                        If CType(Me.mrModelElement, FBM.FactType).IsObjectified Then
                            Return Me.mrModelElement.Id & " - Object Type"
                        Else
                            Return Me.mrModelElement.Id & " - Fact Type"
                        End If
                    Case Else
                        'General Concept or Failsafe.
                        Return Me.mrModelElement.Id & " - Object Type"
                End Select
            End If
        End Get
        Set(value As String)
            Me._msDataLineageItemName = value
        End Set
    End Property

    Private marLineageSet As New List(Of List(Of DataLineage.DataLineageItemProperty))

    Private mbAddingNewLineageSet As Boolean = False

    ''' <summary>
    ''' User can filter by document name.
    ''' </summary>
    Private msFilterDocumentName As String = Nothing

    Private Sub frmDataLineage_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try
            If Me.mrModelElement IsNot Nothing Then
                Me.mrModel = Me.mrModelElement.Model
                Me.LabelLineageItem.Text = Me.mrModelElement.Id
            Else
                'Code Safe
                Exit Sub
            End If

            Dim liInd = 0

            '==MetaData Lineage Item Name==================================
#Region "Data Lineage Item Name"
            If Me.mrModelElement IsNot Nothing Then
                Select Case Me.mrModelElement.GetType
                    Case Is = GetType(FBM.ValueType),
                                      GetType(FBM.EntityType)
                        Me.msDataLineageItemName = Me.mrModelElement.Id & " - Object Type"
                    Case Is = GetType(FBM.FactType)
                        If CType(Me.mrModelElement, FBM.FactType).IsObjectified Then
                            Me.msDataLineageItemName = Me.mrModelElement.Id & " - Object Type"
                        Else
                            Me.msDataLineageItemName = Me.mrModelElement.Id & " - Fact Type"
                        End If
                End Select
            End If
#End Region
            '==========================================================

            'Documents - Metadata Lineage Documents
            For Each lrDataLineageDocument In tableDataLineageItemProperty.getDataLineageDocumentsByDataLineageItemName(Me.mrModel, Me.msDataLineageItemName, True)

                Dim loDocumentNameStringSize As SizeF = Me.Diagram.MeasureString(Trim(lrDataLineageDocument.Name), Me.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                loDocumentNameStringSize.Width = If(30 > loDocumentNameStringSize.Width, 30, loDocumentNameStringSize.Width)
                Dim lrDataLineageDocumentNode As TableNode = Me.Diagram.Factory.CreateTableNode(5, 8 + liInd * 35, loDocumentNameStringSize.Width + 5, 30, 0, 0)

                lrDataLineageDocumentNode.Style = TableStyle.RoundedRectangle
                lrDataLineageDocumentNode.Caption = lrDataLineageDocument.Name
                lrDataLineageDocumentNode.CustomDraw = CustomDraw.Additional
                lrDataLineageDocumentNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                lrDataLineageDocumentNode.Tag = lrDataLineageDocument.Name
                lrDataLineageDocumentNode.Pen = New Pen(Color.White)
                lrDataLineageDocumentNode.ShadowColor = Color.White

                liInd += 1
            Next
            For Each loNode As TableNode In Me.Diagram.Nodes
                loNode.Move(20 - loNode.Bounds.Width / 2, loNode.Bounds.Y)
            Next
            Me.SplitContainer.SplitterDistance = 80

            Me.mrDataLineageItem.Model = Me.mrModel
            Me.mrDataLineageItem.Name = Me.msDataLineageItemName

            '==Categories==================================================
#Region "Load the Lineage Categories from the Reference/Configuration Table in Boston."
            'Get the set of Lineage Categories.
            'Dim loDataLineageCategory As New Object
            'Dim loDataLineageCategoryPropertyType As New Object

            Dim larDataLineageCategory = TableReferenceFieldValue.GetReferenceFieldValueTuples(37).OrderBy(Function(x) x.SequenceNr) ', loDataLineageCategory
            Dim larDataLineageCategoryPropertyType = TableReferenceFieldValue.GetReferenceFieldValueTuples(38) ', loDataLineageCategoryPropertyType

            For Each loLineageCategoryTuple In larDataLineageCategory

                Dim lrDataLineagaeCategory As New DataLineage.DataLineageCategory(Me.mrModel,
                                                                                  loLineageCategoryTuple.DataLineageCategory,
                                                                                  loLineageCategoryTuple.SequenceNr)

                '==Data Lineage Category Property Type=====================
#Region "Data Lineage Category Property Type"
                For Each loDataLineageCategoryPropertyType In larDataLineageCategoryPropertyType.Where(Function(x) x.DataLineageCategory = lrDataLineagaeCategory.Name And Not CBool(x.Hidden) = True).OrderBy(Function(x) x.SequenceNr)

                    Dim lrDataLineageCategoryPropertyType As New DataLineage.DataLineageCategoryPropertyType(Me.mrModel, lrDataLineagaeCategory.Name, loDataLineageCategoryPropertyType.PropertyType, loDataLineageCategoryPropertyType.SequenceNr)

                    lrDataLineagaeCategory.DataLineagaeCategoryPropertyType.Add(lrDataLineageCategoryPropertyType)
                Next
#End Region
                '==========================================================

                Me.marLineageCategory.Add(lrDataLineagaeCategory)
            Next
#End Region
            '==============================================================

            '==============================================================
            'Load the Properties
            Call Me.LoadProperties(True)

            Me.BindingNavigatorLineageProperty.BindingSource = Me.BindingSourceLineageProperty

            Me.ButtonClose.Focus()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function GenerateDataLineageItemName(ByRef arModelElement As FBM.ModelObject) As String

        Try
            '==Data Lineage Item Name==================================
            Select Case arModelElement.GetType
                Case Is = GetType(FBM.ValueType),
                                 GetType(FBM.EntityType)
                    Return arModelElement.Id & " - Object Type"
                Case Is = GetType(FBM.FactType)
                    If CType(Me.mrModelElement, FBM.FactType).IsObjectified Then
                        Return arModelElement.Id & " - Object Type"
                    Else
                        Return arModelElement.Id & " - Fact Type"
                    End If
            End Select

            Return "Error generating Data Lineage Item Name"

            '==========================================================
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return "Error generating Data Lineage Item Name"
        End Try

    End Function

    Private Sub LoadProperties(ByVal abHideHidden As Boolean)

        Try
            Dim liCategoryGroupboxTop = 35

#Region "Load the actual Properties for the Property Types for the Categories"
            For Each lrDataLineageCategory In Me.marLineageCategory

                Dim loGroupBox As New Windows.Forms.GroupBox

                loGroupBox.Top = liCategoryGroupboxTop
                loGroupBox.Left = 10
                loGroupBox.Text = lrDataLineageCategory.Name & ":"
                loGroupBox.Width = Me.GroupBoxCategories.Width - 10
                loGroupBox.Height = 100
                loGroupBox.ForeColor = System.Drawing.ColorTranslator.FromHtml("#cdb69e")
                loGroupBox.Name = lrDataLineageCategory.Name

                Dim liFieldTop = 25

                Dim liLineageSetNumber = tableDataLineageItemProperty.getHighestLineageSetNrForDataLineageItemCategory(Me.mrModel,
                                                                                                                       Me.msDataLineageItemName,
                                                                                                                       lrDataLineageCategory.Name,
                                                                                                                       Me.msFilterDocumentName)

                If liLineageSetNumber = 0 Then liLineageSetNumber = 1


                Dim laiMetaDataLineageSetNumber As New List(Of Integer)
                If Me.msFilterDocumentName IsNot Nothing Then
                    laiMetaDataLineageSetNumber = tableDataLineageItemProperty.getMetaDataLineageSetNumbersByDocumentFilter(Me.mrModel,
                                                                                                                            Me.msDataLineageItemName,
                                                                                                                            lrDataLineageCategory.Name,
                                                                                                                            Me.msFilterDocumentName)
                Else
                    For liInd = 1 To liLineageSetNumber
                        laiMetaDataLineageSetNumber.Add(liInd)
                    Next
                End If

LoadProperties:
                'For each set of Properties for the Data Lineage Category
                For Each liInd As Integer In laiMetaDataLineageSetNumber

                    Dim larLineagePropertySet As New List(Of DataLineage.DataLineageItemProperty)

                    '-----------------------------------------------------------
                    'Create the DataLineageItem
                    Dim lrDataLineageItemProperty As New DataLineage.DataLineageItemProperty
                    lrDataLineageItemProperty.Model = Me.mrModel
                    lrDataLineageItemProperty.Category = lrDataLineageCategory.Name
                    lrDataLineageItemProperty.PropertyType = "Id"
                    lrDataLineageItemProperty.LineageSetNumber = liInd
                    lrDataLineageItemProperty.Name = Me.msDataLineageItemName

                    'Id first. Id of the MetaData Lineage set. A 'set' is a group of DataLinageProperties.
                    Dim lrDataLineageItem = New DataLineage.DataLineageItem(Me.mrModel,
                                                                            lrDataLineageCategory, Me.msDataLineageItemName,
                                                                            tableDataLineageItemProperty.getDataLineageItemPropertyDetails(lrDataLineageItemProperty, True).Property)

                    'Properties by Property Type
                    For Each lrDataLineageCategoryPropertyType In lrDataLineageCategory.DataLineagaeCategoryPropertyType.FindAll(Function(x) Not x.Hidden = abHideHidden)

#Region "Data Lineage Item Properties for Category Property Type"

#Region "Get the Data Lineage Property value from the Boston database."
                        'Gets the actual data for the Data Lineage Property Type for the Data Lineage Category
                        lrDataLineageItemProperty = New DataLineage.DataLineageItemProperty
                        lrDataLineageItemProperty.Model = Me.mrModel
                        lrDataLineageItemProperty.Category = lrDataLineageCategory.Name
                        lrDataLineageItemProperty.PropertyType = lrDataLineageCategoryPropertyType.PropertyType
                        lrDataLineageItemProperty.LineageSetNumber = liInd 'The set number of the MetaDataLineageSet.
                        lrDataLineageItemProperty.Name = Me.msDataLineageItemName
                        lrDataLineageItemProperty.DataLineageItem = lrDataLineageItem


                        Call tableDataLineageItemProperty.getDataLineageItemPropertyDetails(lrDataLineageItemProperty, True)
#End Region

                        Me.mrDataLineageItem.DataLineageItemProperty.Add(lrDataLineageItemProperty)

                        larLineagePropertySet.Add(lrDataLineageItemProperty)

                        '=========================================================================
                        Dim loTextField As Windows.Forms.TextBox = Nothing
                        Dim loLabelPrompt As Windows.Forms.Label = Nothing
                        '======================================================
                        'Important: Skip this step after the first LineageSet
                        If liInd > 1 Then GoTo EndTextboxSetup 'Skip after the first LineageSet

#Region "Set up the form and populate the first set if there is one."
#Region "Field Prompt - I.e. Create the LabelPrompt for the Property"
                        loLabelPrompt = New Windows.Forms.Label
                        loLabelPrompt.Top = liFieldTop
                        loLabelPrompt.Left = 5
                        loLabelPrompt.Font = SystemFonts.DefaultFont
                        loLabelPrompt.Text = lrDataLineageCategoryPropertyType.PropertyType & ":"
                        loLabelPrompt.AutoSize = True
                        loLabelPrompt.ForeColor = Color.SteelBlue

                        loGroupBox.Controls.Add(loLabelPrompt)
#End Region


#Region "Text Field - I.e. Set the Text field value in the Textbox for the Property"
                        loTextField = New Windows.Forms.TextBox

                        loTextField.Top = loLabelPrompt.Top - 3
                        loTextField.Left = loLabelPrompt.Width + 8
                        loTextField.Width = loGroupBox.Width - loTextField.Left - 10
                        loTextField.BorderStyle = BorderStyle.None
                        loTextField.Name = lrDataLineageCategory.Name & lrDataLineageItemProperty.PropertyType

                        loGroupBox.Controls.Add(loTextField)

                        loTextField.Tag = lrDataLineageItemProperty
                        lrDataLineageItemProperty.Control = loTextField
                        loTextField.Text = lrDataLineageItemProperty.Property
                        loTextField.SelectionLength = 0
#End Region
#End Region

EndTextboxSetup: 'Skip to here for all LineageSets past the first one.

                        Me.marDataLineageItemProperty.Add(lrDataLineageItemProperty)

                        If loTextField IsNot Nothing Then
                            liFieldTop = loTextField.Top + loLabelPrompt.Height + 20
                        End If

                        loGroupBox.Height = liFieldTop + 5
#End Region
                    Next

                    Me.marLineageSet.Add(larLineagePropertySet) 'the Lineage Property data loaded from the database. List(Of DataLineage.DataLineageItemProperty)

                Next 'LineageSetNr

                Me.GroupBoxCategories.Controls.Add(loGroupBox)
                liCategoryGroupboxTop = loGroupBox.Top + loGroupBox.Height + 10

            Next 'Category
#End Region

            Me.BindingSourceLineageProperty.DataSource = Me.marLineageSet
            If Me.BindingSourceLineageProperty.Count > 0 Then
                Me.BindingSourceLineageProperty.Position = 0
            End If
            Call Me.SetProperties()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Private Sub LoadPropertiesFiltered(ByVal abHideHidden As Boolean)

        Try
#Region "Load the actual Properties for the Property Types for the Categories"
            For Each lrDataLineageCategory In Me.marLineageCategory

                Dim liLineageSetNumber = tableDataLineageItemProperty.getHighestLineageSetNrForDataLineageItemCategory(Me.mrModel,
                                                                                                                       Me.msDataLineageItemName,
                                                                                                                       lrDataLineageCategory.Name,
                                                                                                                       Me.msFilterDocumentName)

                If liLineageSetNumber = 0 Then liLineageSetNumber = 1


                Dim laiMetaDataLineageSetNumber As New List(Of Integer)
                If Me.msFilterDocumentName IsNot Nothing Then
                    laiMetaDataLineageSetNumber = tableDataLineageItemProperty.getMetaDataLineageSetNumbersByDocumentFilter(Me.mrModel,
                                                                                                                            Me.msDataLineageItemName,
                                                                                                                            lrDataLineageCategory.Name,
                                                                                                                            Me.msFilterDocumentName)
                Else
                    For liInd = 1 To liLineageSetNumber
                        laiMetaDataLineageSetNumber.Add(liInd)
                    Next
                End If

                'For each set of Properties for the Data Lineage Category
                For Each liInd As Integer In laiMetaDataLineageSetNumber

                    Dim larLineagePropertySet As New List(Of DataLineage.DataLineageItemProperty)

                    '-----------------------------------------------------------
                    'Create the DataLineageItem
                    Dim lrDataLineageItemProperty As New DataLineage.DataLineageItemProperty
                    lrDataLineageItemProperty.Model = Me.mrModel
                    lrDataLineageItemProperty.Category = lrDataLineageCategory.Name
                    lrDataLineageItemProperty.PropertyType = "Id"
                    lrDataLineageItemProperty.LineageSetNumber = liInd
                    lrDataLineageItemProperty.Name = Me.msDataLineageItemName

                    'Id first. Id of the MetaData Lineage set. A 'set' is a group of DataLinageProperties.
                    Dim lrDataLineageItem = New DataLineage.DataLineageItem(Me.mrModel,
                                                                            lrDataLineageCategory, Me.msDataLineageItemName,
                                                                            tableDataLineageItemProperty.getDataLineageItemPropertyDetails(lrDataLineageItemProperty, True).Property)

                    'Properties by Property Type
                    For Each lrDataLineageCategoryPropertyType In lrDataLineageCategory.DataLineagaeCategoryPropertyType.FindAll(Function(x) Not x.Hidden = abHideHidden)

#Region "Data Lineage Item Properties for Category Property Type"

#Region "Get the Data Lineage Property value from the Boston database."
                        'Gets the actual data for the Data Lineage Property Type for the Data Lineage Category
                        lrDataLineageItemProperty = New DataLineage.DataLineageItemProperty
                        lrDataLineageItemProperty.Model = Me.mrModel
                        lrDataLineageItemProperty.Category = lrDataLineageCategory.Name
                        lrDataLineageItemProperty.PropertyType = lrDataLineageCategoryPropertyType.PropertyType
                        lrDataLineageItemProperty.LineageSetNumber = liInd 'The set number of the MetaDataLineageSet.
                        lrDataLineageItemProperty.Name = Me.msDataLineageItemName
                        lrDataLineageItemProperty.DataLineageItem = lrDataLineageItem


                        Call tableDataLineageItemProperty.getDataLineageItemPropertyDetails(lrDataLineageItemProperty, True)
#End Region

                        Me.mrDataLineageItem.DataLineageItemProperty.Add(lrDataLineageItemProperty)

                        larLineagePropertySet.Add(lrDataLineageItemProperty)

                        Me.marDataLineageItemProperty.Add(lrDataLineageItemProperty)
#End Region
                    Next

                    Me.marLineageSet.Add(larLineagePropertySet) 'the Lineage Property data loaded from the database. List(Of DataLineage.DataLineageItemProperty)

                Next 'LineageSetNr

            Next 'Category
#End Region

            Me.BindingSourceLineageProperty.DataSource = Me.marLineageSet
            If Me.BindingSourceLineageProperty.Count > 0 Then
                Me.BindingSourceLineageProperty.Position = 0
            End If

            Me.BindingNavigatorLineageProperty.BindingSource = Me.BindingSourceLineageProperty
            Me.BindingNavigatorLineageProperty.Refresh()
            Me.BindingSourceLineageProperty.ResetBindings(True)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click

        Try
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

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click

        Try
            Call Me.SaveDataLineageItemProperties()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Saves all the Data Lineage Item Properies against the text boxes etc.
    ''' </summary>
    Private Sub SaveDataLineageItemProperties()

        Try
            For Each lrDataLineageItemProperty In Me.BindingSourceLineageProperty.Current

                Call lrDataLineageItemProperty.Save(True)

            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BindingSourceLineageProperty_PositionChanged(sender As Object, e As EventArgs) Handles BindingSourceLineageProperty.PositionChanged

        Try
            'CodeSafe
            If Me.marLineageCategory.Count = 0 Then Exit Sub

            If Me.mbAddingNewLineageSet Then
                For Each lrDataLineageCategory In Me.marLineageCategory
                    For Each lrLineagePropertyType In lrDataLineageCategory.DataLineagaeCategoryPropertyType
                        Dim lrDataLineageItem As New DataLineage.DataLineageItem(Me.mrModel,
                                                                                 New DataLineage.DataLineageCategory(Me.mrModel, "Metadata Lineage", 1),
                                                                                 Me.msDataLineageItemName,
                                                                                 System.Guid.NewGuid.ToString)

                        Dim lrLineageItemProperty = New DataLineage.DataLineageItemProperty(lrDataLineageItem,
                                                                                            Me.mrModel,
                                                                                            Me.msDataLineageItemName,
                                                                                            lrLineagePropertyType.PropertyType,
                                                                                            "",
                                                                                            Me.BindingSourceLineageProperty.Position + 1)
                        lrLineageItemProperty.Category = lrDataLineageCategory.Name
                        Dim loGroupBox As GroupBox = Me.GroupBoxCategories.Controls.Find(lrLineageItemProperty.Category, False)(0)
                        lrLineageItemProperty.Control = loGroupBox.Controls.Find(lrLineageItemProperty.Category & lrLineageItemProperty.PropertyType, False)(0)
                        Call DirectCast(Me.BindingSourceLineageProperty.Current, List(Of DataLineage.DataLineageItemProperty)).Add(lrLineageItemProperty)
                    Next
                Next
            End If

            Me.mbAddingNewLineageSet = False
            Call Me.SetProperties()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveNextItem.Click

        Try
            'If Me.BindingSourceLineageProperty.Current IsNot Nothing Then
            '    Dim currentIndex As Integer = Me.BindingSourceLineageProperty.Position
            '    Dim nextIndex As Integer = currentIndex

            '    If nextIndex < Me.BindingSourceLineageProperty.Count Then
            '        Me.BindingSourceLineageProperty.Position = nextIndex
            '    End If
            'End If

            'Call Me.SetProperties()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BindingNavigatorMovePreviousItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click

        Try
            'If Me.BindingSourceLineageProperty.Current IsNot Nothing Then
            '    Dim currentIndex As Integer = Me.BindingSourceLineageProperty.Position
            '    Dim previousIndex As Integer = currentIndex - 1

            '    If previousIndex >= 0 Then
            '        Me.BindingSourceLineageProperty.Position = previousIndex
            '    End If
            'End If

            'Call Me.SetProperties()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub SetProperties()

        Try
            'Clear TextBoxes
            For Each loControl In Me.SplitContainer.Panel2.Controls(0).Controls
                If loControl.GetType = GetType(GroupBox) Then

                    For Each loSubControl In CType(loControl, GroupBox).Controls

                        If loSubControl.GetType = GetType(TextBox) Then
                            CType(loSubControl, TextBox).Text = ""
                        End If

                    Next

                End If
            Next


            Dim larLinesagePropertySet As List(Of DataLineage.DataLineageItemProperty) = Me.BindingSourceLineageProperty.Current
            Dim liInd = Me.BindingSourceLineageProperty.Position
            'CodeSafe
            If larLinesagePropertySet Is Nothing Then Exit Sub

#Region "Text Field"
            Dim loTextField As Windows.Forms.TextBox

            If larLinesagePropertySet.Count = 0 Then
                Exit Sub

            End If

            Dim loGroupBox As GroupBox = Nothing
            Try
                loGroupBox = Me.GroupBoxCategories.Controls.Find(larLinesagePropertySet(0).Category, False)(0)
            Catch
                GoTo SkipGroup
            End Try
            For Each lrDataLineageItemProperty In larLinesagePropertySet
                Try
                    loTextField = loGroupBox.Controls.Find(lrDataLineageItemProperty.Category & lrDataLineageItemProperty.PropertyType, False)(0)
                Catch
                    GoTo SkipField
                End Try
                lrDataLineageItemProperty.Control = loTextField
                loTextField.Text = lrDataLineageItemProperty.Property
                loTextField.SelectionStart = 0
                loTextField.SelectionLength = 0
SkipField:
            Next
SkipGroup:
#End Region
            Me.ButtonSave.TabStop = 0
            Me.ButtonSave.Focus()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BindingNavigatorMoveFirstItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveFirstItem.Click

        Try
            If Me.BindingSourceLineageProperty.Count > 0 Then
                Me.BindingSourceLineageProperty.Position = 0
            End If

            Call Me.SetProperties()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub BindingNavigatorMoveLastItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveLastItem.Click

        Try
            If Me.BindingSourceLineageProperty.Count > 0 Then
                Me.BindingSourceLineageProperty.Position = Me.BindingSourceLineageProperty.Count - 1
            End If

            Call Me.SetProperties()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonOpenDocument.Click

        Try
            Dim lfrmDocumentViewer As New frmPDFDocumentViewer

            Dim larLineageProperty As List(Of DataLineage.DataLineageItemProperty) = Me.BindingSourceLineageProperty.Current

            lfrmDocumentViewer.msDocumentFilePath = larLineageProperty.Find(Function(x) x.PropertyType = "Document Location").Property

            If Not System.IO.File.Exists(lfrmDocumentViewer.msDocumentFilePath) Then
                MsgBox("There seems to be a problem. No file exists for that file path. Change the document Location and try again.")
                Exit Sub
            End If

            Dim liPageNumber As Integer = 0
            If Integer.TryParse(larLineageProperty.Find(Function(x) x.PropertyType = "Page Number").Property, liPageNumber) Then
                lfrmDocumentViewer.miPageNumber = liPageNumber
            End If

            lfrmDocumentViewer.msObjectTypeName = Me.mrModelElement.Id


            Dim lfrmMain As frmMain = Me.MdiParent

            With New WaitCursor
                lfrmDocumentViewer.Show(lfrmMain.DockPanel)
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BindingSourceLineageProperty_ListChanged(sender As Object, e As ListChangedEventArgs) Handles BindingSourceLineageProperty.ListChanged

        If e.ListChangedType = ListChangedType.ItemAdded Then
            ' Access the newly added item using BindingSource.Current
            Me.mbAddingNewLineageSet = True

            'For Each lrDataLineageItemProperty In newItem
            '    lrDataLineageItemProperty.Property = ""
            '    lrDataLineageItemProperty.LineageSetNumber = Me.BindingSourceLineageProperty.Position + 1
            'Next

            '' Your code to manage the newly added item here
            'Call Me.SetProperties()
        End If

    End Sub

    Private Sub BindingNavigatorAddNewItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorAddNewItem.Click

        Try
            '20231228-Doesn't seem any code is needed.

            'Dim larDataLineageItemProperty As New List(Of DataLineage.DataLineageItemProperty)
            'For Each lrDatLineageCategory In Me.marLineageCategory
            '    For Each lrDataLineagePropertyType In lrDatLineageCategory.DataLineagaeCategoryPropertyType
            '        Dim lrDataLineageItemProperty As New DataLineage.DataLineageItemProperty(Me.mrModel,
            '                                                                                 Me.msDataLineageItemName,
            '                                                                                 lrDataLineagePropertyType.Name,
            '                                                                                 "",
            '                                                                                 Me.marLineageSet.Count + 1)
            '        lrDataLineageItemProperty.Category = lrDatLineageCategory.Name
            '        larDataLineageItemProperty.Add(lrDataLineageItemProperty)
            '    Next
            'Next

            'Me.marLineageSet.Add(larDataLineageItemProperty)


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BindingNavigatorDeleteItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorDeleteItem.Click

        Try
            If MsgBox("Are you sure you want to delete the current set of Lineage Item Properties for each Category in the current set?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                Dim larLinesagePropertySet As List(Of DataLineage.DataLineageItemProperty) = Me.BindingSourceLineageProperty.Current

                'CodeSafe
                If larLinesagePropertySet.Count = 0 Then Exit Sub

                For Each lrLinagePropertyItem In larLinesagePropertySet
                    Call lrLinagePropertyItem.Delete
                Next

                'CodeSafe
                'Safeguard
                For Each lrDataLineageCategory In Me.marLineageCategory
                    tableDataLineageItemProperty.DeleteDataLineageItemPropertisByCategorySetNumber(Me.mrModel, Me.msDataLineageItemName, lrDataLineageCategory, larLinesagePropertySet(0).LineageSetNumber)
                Next

            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripButtonClose_Click(sender As Object, e As EventArgs) Handles ToolStripButtonClose.Click

        Try
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

    Private Sub Diagram_DrawNode(sender As Object, e As DrawNodeEventArgs) Handles Diagram.DrawNode

        If TypeOf e.Node Is TableNode Then
            Dim g As IGraphics = e.Graphics
            Dim image As Image = My.Resources.DocumentVanillaLarge

            ' Set the interpolation mode for smoother image rendering
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

            ' Align the destination, so that image is not blurred
            Dim dest As RectangleF = New RectangleF(New PointF(e.Bounds.X + (e.Bounds.Width / 2 - 12), e.Bounds.Y), New SizeF(e.Bounds.Width, e.Bounds.Height))
            dest = Me.DiagramView.ClientToDoc(Me.DiagramView.DocToClient(dest))
            Dim imagedest = Me.DiagramView.ClientToDoc(Me.DiagramView.DocToClient(New RectangleF(0, 0, image.Width, image.Height)))
            Dim loNode = CType(e.Node, TableNode)

            g.DrawImage(image, dest.X, dest.Y + 5)
        End If

        ' Apply anti-aliasing to the entire view (if desired)
        Me.DiagramView.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality

    End Sub

    Private Sub Diagram_NodeClicked(sender As Object, e As NodeEventArgs) Handles Diagram.NodeClicked

        Try

            If e.MouseButton = MouseButton.Right Then
                Me.DiagramView.ContextMenuStrip = Me.ContextMenuStripDocument
                Me.FilterByThisDocumentToolStripMenuItem.Tag = e.Node.Tag
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ClearFilterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearFilterToolStripMenuItem.Click

        Try
            Me.msFilterDocumentName = Nothing

            Me.marLineageSet.Clear()
            Call Me.LoadPropertiesFiltered(True)
            Call Me.SetProperties()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub FilterByThisDocumentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FilterByThisDocumentToolStripMenuItem.Click

        Try
            Me.msFilterDocumentName = Me.FilterByThisDocumentToolStripMenuItem.Tag
            Me.marLineageSet.Clear()
            Call Me.LoadPropertiesFiltered(True)
            Call Me.SetProperties()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DiagramView_MouseDown(sender As Object, e As MouseEventArgs) Handles DiagramView.MouseDown

        Try
            Dim lo_point = Me.DiagramView.ClientToDoc(e.Location)

            If Diagram.GetNodeAt(lo_point) IsNot Nothing Then
                '----------------------------
                'Mouse is over an ShapeNode
                '----------------------------        
                '----------------------------------------------------
                'Get the Node/Shape under the mouse cursor.
                '----------------------------------------------------
                If TypeOf Diagram.GetNodeAt(lo_point) Is MindFusion.Diagramming.TableNode Then

                    Dim loDocumentNode As TableNode = Diagram.GetNodeAt(lo_point)
                    loDocumentNode.Selected = True
                    Me.ContextMenuStrip = ContextMenuStripDocument

                End If
            Else
                Me.DiagramView.ContextMenuStrip = ContextMenuStripDocuments
            End If



        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Diagram_NodeSelected(sender As Object, e As NodeEventArgs) Handles Diagram.NodeSelected

        Try
            Me.DiagramView.ContextMenuStrip = Me.ContextMenuStripDocument
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Diagram_NodeDeselected(sender As Object, e As NodeEventArgs) Handles Diagram.NodeDeselected

        Try
            Me.DiagramView.ContextMenuStrip = Me.ContextMenuStripDocuments
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class