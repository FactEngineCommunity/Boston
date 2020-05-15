Imports System.Reflection

Public Class frmCRUDImportFact

    Public zrFactTypeInstance As New FBM.FactTypeInstance

    Private Sub frmImportFact_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.SetupPage()

    End Sub

    Private Sub LoadFacts()

        Try
            Dim lrFact As FBM.Fact
            Dim lrComboboxItem As tComboboxItem

            For Each lrFact In Me.zrFactTypeInstance.FactType.Fact

                lrComboboxItem = New tComboboxItem(lrFact.Id, lrFact.EnumerateAsBracketedFact, lrFact)

                Me.ComboBox1.Items.Add(lrComboboxItem)
            Next

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub SetupPage()

        Call Me.LoadFacts()

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click

        Dim lrFact As FBM.Fact
        Dim lrFactInstance As New FBM.FactInstance

        lrFact = ComboBox1.SelectedItem.Tag

        lrFactInstance = lrFact.CloneInstance(Me.zrFactTypeInstance.Page)

        Me.zrFactTypeInstance.Fact.Add(lrFactInstance)

        Call Me.zrFactTypeInstance.FactTable.ResortFactTable()

        Call Me.zrFactTypeInstance.Page.MakeDirty() 'Leave this as last call, because triggers events

        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

    End Sub
End Class