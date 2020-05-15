Imports System.Windows.Forms.Design
Imports System.Collections.Specialized
Imports System.Collections.Generic
Imports System.Text
Imports System.ComponentModel.Design


Public Class tStringCollectionEditor
    Inherits CollectionEditor

    Public Delegate Sub myFormClosedEventHandler(ByVal sender As Object, ByVal e As EventArgs)
    Public Shared Event MyFormClosed As myFormClosedEventHandler

    Public Delegate Sub myFormItemChangedHandler(ByVal sender As Object, ByVal e As PropertyValueChangedEventArgs)
    Public Shared Event MyItemChanged As myFormItemChangedHandler

    Public Sub New()
        MyBase.new(GetType(Viev.Strings.StringCollection))
    End Sub

    Protected Overrides Function CreateInstance(ByVal itemType As System.Type) As Object                
        Return ""
    End Function


    Protected Overrides Function CreateCollectionForm() As System.ComponentModel.Design.CollectionEditor.CollectionForm

        Dim lrCollectionForm As CollectionForm = MyBase.CreateCollectionForm()
        Dim tlpLayout As TableLayoutPanel

        AddHandler lrCollectionForm.FormClosed, AddressOf collection_FormClosed

        lrCollectionForm.Text = "Value Constraint Editor"
        '----------------------
        tlpLayout = lrCollectionForm.Controls(0)

        If IsSomething(tlpLayout) Then
            'Get a reference to the inner PropertyGrid and hook 
            '  an event handler to it.
            If TypeOf tlpLayout.Controls(5) Is PropertyGrid Then
                Dim loPropertyGrid As PropertyGrid = tlpLayout.Controls(5) '(5) ' as PropertyGrid;                
                AddHandler loPropertyGrid.PropertyValueChanged, AddressOf Me.propertyGrid_PropertyValueChanged
            End If

        End If

        '----------------------
        Return lrCollectionForm
    End Function


    Public Sub collection_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        '-----------------------------------------------------------------------------------------
        'Called when 'FormClosed' triggered and associated event handler also triggered as above
        '-----------------------------------------------------------------------------------------
        RaiseEvent MyFormClosed(Me, e)        

    End Sub

    Public Sub propertyGrid_PropertyValueChanged(ByVal sender As Object, ByVal e As PropertyValueChangedEventArgs)
        '-----------------------------------------
        'Fire the customized collection event...
        '-----------------------------------------
        RaiseEvent MyItemChanged(sender, e)

    End Sub

End Class
