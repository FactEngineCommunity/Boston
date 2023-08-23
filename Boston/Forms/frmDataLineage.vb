Imports System.Reflection

Public Class frmDataLineage

    Public mrModel As FBM.Model
    Public mrModelElement As FBM.ModelObject

    Dim mrDataLineageItem As New DataLineage.DataLineageItem

    Private marLineageCategory As New List(Of DataLineage.DataLineageCategory)

    Private marDataLineageItemProperty As New List(Of DataLineage.DataLineageItemProperty)

    Private msDataLineageItemName As String

    Private marLineageSet As New List(Of List(Of DataLineage.DataLineageItemProperty))

    Private Sub frmDataLineage_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try

            If Me.mrModelElement IsNot Nothing Then
                Me.mrModel = Me.mrModelElement.Model
                Me.LabelLineageItem.Text = Me.mrModelElement.Id
            End If

            '==Data Lineage Item Name==================================
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

            Me.mrDataLineageItem.Model = Me.mrModel
            Me.mrDataLineageItem.Name = Me.msDataLineageItemName


            '==========================================================
            'Load the Properties
            Call Me.LoadProperties(True)

            Me.ButtonClose.Focus()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadProperties(ByVal abHideHidden As Boolean)

        Try

            '==Categories==============================================
#Region "Lineage Categories"
            'Get the set of Lineage Categories.
            Dim loDataLineageCategory As New Object
            Dim loDataLineageCategoryPropertyType As New Object

            Dim larDataLineageCategory = TableReferenceFieldValue.GetReferenceFieldValueTuples(37, loDataLineageCategory).OrderBy(Function(x) x.SequenceNr)
            Dim larDataLineageCategoryPropertyType = TableReferenceFieldValue.GetReferenceFieldValueTuples(38, loDataLineageCategoryPropertyType)

            For Each loLineageCategoryTuple In larDataLineageCategory

                Dim lrDataLineagaeCategory As New DataLineage.DataLineageCategory(Me.mrModel,
                                                                                  loLineageCategoryTuple.DataLineageCategory,
                                                                                  loLineageCategoryTuple.SequenceNr)

                '==Data Lineage Category Property Type=====================
#Region "Data Lineage Category Property Type"
                For Each loDataLineageCategoryPropertyType In larDataLineageCategoryPropertyType.Where(Function(x) x.DataLineageCategory = lrDataLineagaeCategory.Name And Not CBool(x.Hidden) = abHideHidden).OrderBy(Function(x) x.SequenceNr)

                    Dim lrDataLineageCategoryPropertyType As New DataLineage.DataLineageCategoryPropertyType(Me.mrModel, lrDataLineagaeCategory.Name, loDataLineageCategoryPropertyType.PropertyType, loDataLineageCategoryPropertyType.SequenceNr)

                    lrDataLineagaeCategory.DataLineagaeCategoryPropertyType.Add(lrDataLineageCategoryPropertyType)
                Next
#End Region
                '==========================================================

                Me.marLineageCategory.Add(lrDataLineagaeCategory)
            Next
#End Region
            '==========================================================

            Dim liCategoryGroupboxTop = 35

#Region "Load the Properties"
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
                                                                                                                       lrDataLineageCategory.Name)

                'For each set of Properties for the Data Lineage Category
                For liInd = 1 To liLineageSetNumber

                    Dim larLineagePropertySet As New List(Of DataLineage.DataLineageItemProperty)

                    'Properties by Property Type
                    For Each lrDataLineageCategoryPropertyType In lrDataLineageCategory.DataLineagaeCategoryPropertyType.FindAll(Function(x) Not x.Hidden = abHideHidden)

#Region "Data Lineage Category Property Type"

#Region "Get the Data Lineage Property"
                        'Gets the actual data for the Data Lineage Property Type for the Data Lineage Category
                        Dim lrDataLineageItemProperty As New DataLineage.DataLineageItemProperty
                        lrDataLineageItemProperty.Model = Me.mrModel
                        lrDataLineageItemProperty.Category = lrDataLineageCategory.Name
                        lrDataLineageItemProperty.PropertyType = lrDataLineageCategoryPropertyType.PropertyType
                        lrDataLineageItemProperty.LineageSetNumber = liInd
                        lrDataLineageItemProperty.Name = Me.msDataLineageItemName


                        Call tableDataLineageItemProperty.getDataLineageItemPropertyDetails(lrDataLineageItemProperty, True)
#End Region

                        Me.mrDataLineageItem.DataLineageItemProperty.Add(lrDataLineageItemProperty)

                        larLineagePropertySet.Add(lrDataLineageItemProperty)

                        If liInd > 1 Then GoTo EndTextboxSetup
#Region "Field Prompt"
                        Dim loLabelPrompt As New Windows.Forms.Label
                        loLabelPrompt.Top = liFieldTop
                        loLabelPrompt.Left = 5
                        loLabelPrompt.Font = SystemFonts.DefaultFont
                        loLabelPrompt.Text = lrDataLineageCategoryPropertyType.PropertyType & ":"
                        loLabelPrompt.AutoSize = True
                        loLabelPrompt.ForeColor = Color.SteelBlue

                        loGroupBox.Controls.Add(loLabelPrompt)
#End Region


#Region "Text Field"
                        Dim loTextField As New Windows.Forms.TextBox

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
EndTextboxSetup:

                        Me.marDataLineageItemProperty.Add(lrDataLineageItemProperty)

                        liFieldTop = loTextField.Top + loLabelPrompt.Height + 20

                        loGroupBox.Height = liFieldTop + 5
#End Region
                    Next

                    Me.marLineageSet.Add(larLineagePropertySet)
                Next 'LineageSetNr

                Me.GroupBoxCategories.Controls.Add(loGroupBox)

                liCategoryGroupboxTop = loGroupBox.Top + loGroupBox.Height + 10
            Next
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click

        Try
            Call Me.SaveDataLineageItemProperties

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Saves all the Data Lineage Item Properies against the text boxes etc.
    ''' </summary>
    Private Sub SaveDataLineageItemProperties()

        Try
            For Each lrDataLineageItemProperty In Me.marDataLineageItemProperty

                Call lrDataLineageItemProperty.Save(True)

            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveNextItem.Click

        Try
            If Me.BindingSourceLineageProperty.Current IsNot Nothing Then
                Dim currentIndex As Integer = Me.BindingSourceLineageProperty.Position
                Dim nextIndex As Integer = currentIndex + 1

                If nextIndex < Me.BindingSourceLineageProperty.Count Then
                    Me.BindingSourceLineageProperty.Position = nextIndex
                End If
            End If

            Call Me.SetProperties()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SetProperties()

        Try
            Dim larLinesagePropertySet As List(Of DataLineage.DataLineageItemProperty) = Me.BindingSourceLineageProperty.Current

            'CodeSafe
            If larLinesagePropertySet Is Nothing Then Exit Sub

#Region "Text Field"
            Dim loTextField As Windows.Forms.TextBox

            If larLinesagePropertySet.Count = 0 Then
                Exit Sub

            End If

            Dim loGroupBox = Me.GroupBoxCategories.Controls.Find(larLinesagePropertySet(0).Category, False)(0)

            For Each lrDataLineageItemProperty In larLinesagePropertySet
                loTextField = loGroupBox.Controls.Find(lrDataLineageItemProperty.Category & lrDataLineageItemProperty.PropertyType, False)(0)
                lrDataLineageItemProperty.Control = loTextField
                loTextField.Text = lrDataLineageItemProperty.Property
                loTextField.SelectionStart = 0
                loTextField.SelectionLength = 0
            Next
#End Region
            Me.ButtonSave.TabStop = 0
            Me.ButtonSave.Focus()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BindingNavigatorMovePreviousItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click

        Try
            If Me.BindingSourceLineageProperty.Current IsNot Nothing Then
                Dim currentIndex As Integer = Me.BindingSourceLineageProperty.Position
                Dim previousIndex As Integer = currentIndex - 1

                If previousIndex >= 0 Then
                    Me.BindingSourceLineageProperty.Position = previousIndex
                End If
            End If

            Call Me.SetProperties()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try
            Dim lfrmDocumentViewer As New frmPDFDocumentViewer

            Dim larLineageProperty As List(Of DataLineage.DataLineageItemProperty) = Me.BindingSourceLineageProperty.Current

            lfrmDocumentViewer.msDocumentFilePath = larLineageProperty.Find(Function(x) x.PropertyType = "Document Location").Property

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class