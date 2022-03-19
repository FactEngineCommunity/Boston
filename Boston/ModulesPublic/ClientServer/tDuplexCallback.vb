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

    <CallbackBehavior(UseSynchronizationContext:=False)> _
    Friend Class DuplexCallback
        Implements BostonWCFServiceLibrary.IDuplexCallback

        Private _syncContext As SynchronizationContext = AsyncOperationManager.SynchronizationContext

        ''' <summary>
        ''' See prDuplexServiceClient.HandleBroadcastReceived
        ''' </summary>
        Public Event BroadcastEventReceived As EventHandler(Of Broadcast)

        Public Sub ReceiveBroadcast(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                                    ByVal arObject As Viev.FBM.Interface.Broadcast) Implements BostonWCFServiceLibrary.IDuplexCallback.ReceiveBroadcast
            _syncContext.Post(New SendOrPostCallback(AddressOf OnBroadcastEvent), New Broadcast(aiBroadcastType, arObject))
        End Sub

        Private Sub OnBroadcastEvent(state As Object)

            Dim e As Broadcast = TryCast(state, Broadcast)
            RaiseEvent BroadcastEventReceived(Me, e)
        End Sub


    End Class

End Namespace
