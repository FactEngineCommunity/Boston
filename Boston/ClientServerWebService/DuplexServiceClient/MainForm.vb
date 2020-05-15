Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Diagnostics

Imports System.ServiceModel
Imports BostonWCFServiceLibrary.DuplexService

Namespace ExampleDuplexServiceClient

    Partial Public Class MainForm
        Inherits Form

        Private Const ServiceEndpointUri As String = "http://localhost:9001/WCFServices/DuplexService"
        Private _proxy As GroceryListDuplexServiceClient

        Public Sub New()
            InitializeComponent()
            InitializeClient()
        End Sub

        Private Sub HandleServiceCallbackEvent(sender As Object, e As UpdatedListEventArgs)
            Dim groceryList As List(Of String) = e.ItemList
            If groceryList IsNot Nothing AndAlso groceryList.Count > 0 Then
                groceryListBox.DataSource = groceryList
            End If
        End Sub

        Private Sub InitializeClient()

            Try
                If _proxy IsNot Nothing Then
                    Try
                        _proxy.Close()
                    Catch
                        _proxy.Abort()
                    Finally
                        _proxy = Nothing
                    End Try
                End If

                Dim groceryListDuplexCallback As New GroceryListDuplexCallback()
                AddHandler groceryListDuplexCallback.ServiceCallbackEvent, AddressOf HandleServiceCallbackEvent

                Dim instanceContext As New InstanceContext(groceryListDuplexCallback)
                Dim dualHttpBinding As New WSDualHttpBinding(WSDualHttpSecurityMode.None)
                dualHttpBinding.OpenTimeout = New TimeSpan(0, 0, 5)
                Dim endpointAddress As New EndpointAddress(ServiceEndpointUri)
                _proxy = New GroceryListDuplexServiceClient(instanceContext, dualHttpBinding, endpointAddress)
                _proxy.Open()
                _proxy.Connect()

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub addItemButton_Click(sender As Object, e As EventArgs) Handles addItemButton.Click
            _proxy.AddItem(itemTextBox.Text.Trim())
            itemTextBox.Clear()
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If

                If _proxy IsNot Nothing Then
                    Try
                        _proxy.Disconnect()
                        _proxy.Close()
                    Catch
                        _proxy.Abort()
                    End Try
                End If
            End If

            MyBase.Dispose(disposing)
        End Sub


        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

            Dim lrModel As New Viev.FBM.Interface.XMLModel.Model
            Dim lrPage As New Viev.FBM.Interface.XMLModel.Page()

            lrModel.ORMModel.Name = "HelloWorld"
            lrPage.Name = "HelloWorld"
            'lrModel.ORMDiagram.Add(lrPage)

            Call Me._proxy.SendBroadcast("MovePageObject", lrModel)

        End Sub

    End Class

End Namespace
