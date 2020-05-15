Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports BostonWCFServiceLibrary.DuplexService

Namespace ExampleDuplexServiceClient

    Friend Class GroceryListDuplexServiceClient
        Inherits DuplexClientBase(Of BostonWCFServiceLibrary.IDuplexService)
        Implements BostonWCFServiceLibrary.IDuplexService

        Public Sub New(callbackInstance As InstanceContext, binding As WSDualHttpBinding, endpointAddress As EndpointAddress)
            MyBase.New(callbackInstance, binding, endpointAddress)
        End Sub

        Public Sub Connect() Implements BostonWCFServiceLibrary.IDuplexService.Connect
            Channel.Connect()
        End Sub

        Public Sub AddItem(item As String) Implements BostonWCFServiceLibrary.IDuplexService.AddItem
            Channel.AddItem(item)
        End Sub

        Public Sub Disconnect() Implements BostonWCFServiceLibrary.IDuplexService.Disconnect
            Channel.Disconnect()
        End Sub

        Public Sub SendBroadcast(ByVal asMessage As String, ByVal arObject As Viev.FBM.Interface.XMLModel.Model) Implements BostonWCFServiceLibrary.IDuplexService.SendBroadcast
            Channel.SendBroadcast(asMessage, arObject)
        End Sub

    End Class

End Namespace
