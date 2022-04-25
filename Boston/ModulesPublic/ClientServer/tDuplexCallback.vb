Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.ServiceModel
Imports BostonWCFServiceLibrary.DuplexService


Namespace DuplexServiceClient

    Public Class Broadcast

        Public BroadcastType As Viev.FBM.Interface.pcenumBroadcastType
        Public Broadcast As Viev.FBM.Interface.Broadcast  'Object

        Public Sub New(aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType, arBroadcast As Object)
            Me.BroadcastType = aiBroadcastType
            Me.Broadcast = arBroadcast
        End Sub
    End Class

    <CallbackBehavior(UseSynchronizationContext:=False)>
    Friend Class DuplexCallback
        Implements BostonWCFServiceLibrary.IDuplexCallback

        Private _syncContext As SynchronizationContext = AsyncOperationManager.SynchronizationContext

        ''' <summary>
        ''' See prDuplexServiceClient.HandleBroadcastReceived
        ''' See also frmMain where: AddHandler DuplexCallback.BroadcastEventReceived, AddressOf prDuplexServiceClient.HandleBroadcastReceived
        ''' </summary>
        Public Event BroadcastEventReceived As EventHandler(Of Broadcast)

        Public Sub ReceiveBroadcast(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                                    ByRef arObject As Viev.FBM.Interface.Broadcast) Implements BostonWCFServiceLibrary.IDuplexCallback.ReceiveBroadcast

            Dim lrBroadcast = New Broadcast(aiBroadcastType, arObject)
            '20220327-VM-Was _syncContext.Post(New SendOrPostCallback(AddressOf OnBroadcastEvent), lrBroadcast)
            'To send data back to client, must use Send.
            _syncContext.Send(New SendOrPostCallback(AddressOf OnBroadcastEvent), lrBroadcast)
            arObject.Model.Namespace = lrBroadcast.Broadcast.Model.Namespace
        End Sub

        Private Sub OnBroadcastEvent(ByVal State As Broadcast)

            Dim e As Broadcast = TryCast(State, Broadcast)
            RaiseEvent BroadcastEventReceived(Me, e)
        End Sub

    End Class

End Namespace
