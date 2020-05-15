Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.ServiceModel
Imports BostonWCFServiceLibrary.DuplexService

Namespace ExampleDuplexServiceClient

    Class Broadcast

        Public Message As String
        Public [Object] As Object

        Public Sub New(asMessage As String, arObject As Object)
            Me.Message = asMessage
            Me.Object = arObject
        End Sub
    End Class

    <CallbackBehavior(UseSynchronizationContext:=False)> _
    Friend Class GroceryListDuplexCallback
        Implements BostonWCFServiceLibrary.IDuplexCallback
        Private _syncContext As SynchronizationContext = AsyncOperationManager.SynchronizationContext

        Public Event ServiceCallbackEvent As EventHandler(Of UpdatedListEventArgs)
        Public Event BroadcastEventReceived As EventHandler(Of Broadcast)

        Public Sub SendUpdatedList(items As List(Of String)) Implements BostonWCFServiceLibrary.IDuplexCallback.SendUpdatedList
            _syncContext.Post(New SendOrPostCallback(AddressOf OnServiceCallbackEvent), New UpdatedListEventArgs(items))
        End Sub

        Public Sub ReceiveBroadcast(ByVal asMessage As String, ByVal arObject As Viev.FBM.Interface.XMLModel.Model) Implements BostonWCFServiceLibrary.IDuplexCallback.ReceiveBroadcast
            _syncContext.Post(New SendOrPostCallback(AddressOf OnBroadcastEvent), New Broadcast(asMessage, arObject))
        End Sub

        Private Sub OnServiceCallbackEvent(state As Object)
            'Dim handler As EventHandler(Of UpdatedListEventArgs) = ServiceCallbackEvent
            Dim e As UpdatedListEventArgs = TryCast(state, UpdatedListEventArgs)

            RaiseEvent ServiceCallbackEvent(Me, e)
        End Sub

        Private Sub OnBroadcastEvent(state As Object)

            Dim e As Broadcast = TryCast(state, Broadcast)
            RaiseEvent BroadcastEventReceived(Me, e)
        End Sub

    End Class

End Namespace
