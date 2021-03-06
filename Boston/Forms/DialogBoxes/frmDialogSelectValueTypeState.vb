Public Class frmDialogSelectValueTypeState

    Public mrModel As FBM.Model
    Public mrValueType As FBM.ValueType
    Public msState As String

    Public Sub New(ByRef arModel As FBM.Model, ByRef arSTDiagram As STD.Diagram)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mrModel = arModel
        Me.mrValueType = arSTDiagram.ValueType

        'Dim larValueTypeList = Me.mrModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False And
        '                                                                pcenumORMDataType.NumericAutoCounter <> x.DataType And
        '                                                                Not x.IsReferenceMode)

        'For Each lrValueType In larValueTypeList
        '    Me.ComboBoxState.Items.Add(New tComboboxItem(lrValueType, lrValueType.Name, lrValueType))
        'Next

        For Each lsValueConstraint In Me.mrValueType.ValueConstraint
            If arSTDiagram.State.Find(Function(x) x.StateName = lsValueConstraint) Is Nothing Then
                Me.ComboBoxState.Items.Add(New tComboboxItem(lsValueConstraint, lsValueConstraint, Nothing))
            End If
        Next

    End Sub

    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click

        If Me.ComboBoxState.SelectedIndex = -1 Then
            MsgBox("Select a Value Type State or click [I'll create my own State Name].")
        Else
            Me.msState = Me.ComboBoxState.SelectedItem.ItemData
            Me.DialogResult = DialogResult.OK
        End If

    End Sub

    Private Sub ButtonIllCreateMyOwn_Click(sender As Object, e As EventArgs) Handles ButtonIllCreateMyOwn.Click

        Me.DialogResult = DialogResult.Yes
        Me.Close()

    End Sub

End Class