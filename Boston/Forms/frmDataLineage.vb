Imports System.Reflection

Public Class frmDataLineage

    Public mrModel As FBM.Model
    Public mrModelElement As FBM.ModelObject

    Private marLineageCategory As New List(Of DataLineage.DataLineageCategory)

    Private marDataLineageItemProperty As New List(Of DataLineage.DataLineageItemProperty)

    Private Sub frmDataLineage_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try
            If Me.mrModelElement IsNot Nothing Then
                Me.mrModel = Me.mrModelElement.Model
                Me.LabelLineageItem.Text = Me.mrModelElement.Id
            End If

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
                For Each loDataLineageCategoryPropertyType In larDataLineageCategoryPropertyType.Where(Function(x) x.DataLineageCategory = lrDataLineagaeCategory.Name).OrderBy(Function(x) x.SequenceNr)

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

            For Each lrDataLineageCategory In Me.marLineageCategory

                Dim loGroupBox As New Windows.Forms.GroupBox

                loGroupBox.Top = liCategoryGroupboxTop
                loGroupBox.Left = 10
                loGroupBox.Text = lrDataLineageCategory.Name & ":"
                loGroupBox.Width = Me.GroupBoxCategories.Width - 10
                loGroupBox.Height = 100
                loGroupBox.ForeColor = Color.SaddleBrown

                Dim liFieldTop = 25

                For Each lrDataLineageCategoryPropertyType In lrDataLineageCategory.DataLineagaeCategoryPropertyType

                    Dim loLabelPrompt As New Windows.Forms.Label

                    loLabelPrompt.Top = liFieldTop
                    loLabelPrompt.Left = 5
                    loLabelPrompt.Font = SystemFonts.DefaultFont
                    loLabelPrompt.Text = lrDataLineageCategoryPropertyType.PropertyType & ":"
                    loLabelPrompt.AutoSize = True
                    loLabelPrompt.ForeColor = Color.SteelBlue

                    loGroupBox.Controls.Add(loLabelPrompt)

                    Dim loTextField As New Windows.Forms.TextBox

                    loTextField.Top = loLabelPrompt.Top - 3
                    loTextField.Left = loLabelPrompt.Width + 8
                    loTextField.Width = loGroupBox.Width - loTextField.Left - 10

                    loGroupBox.Controls.Add(loTextField)

                    Dim lrDataLineageItemProperty As New DataLineage.DataLineageItemProperty

                    loTextField.Tag = lrDataLineageItemProperty

                    Me.marDataLineageItemProperty.Add(lrDataLineageItemProperty)

                    liFieldTop = loTextField.Top + loLabelPrompt.Height + 20

                    loGroupBox.Height = liFieldTop + 5
                Next

                Me.GroupBoxCategories.Controls.Add(loGroupBox)

                liCategoryGroupboxTop = loGroupBox.Top + loGroupBox.Height + 10
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


End Class