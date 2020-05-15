Imports System.ServiceModel

<ServiceBehavior(InstanceContextMode:=InstanceContextMode.Single, ConcurrencyMode:=ConcurrencyMode.Multiple)>
Public Class DuplexService
    Implements IDuplexService

    Private Shared _callbackChannels As List(Of IDuplexCallback) = New List(Of IDuplexCallback)()

    Private Shared ReadOnly _sycnRoot As Object = New Object()

    Public Sub Connect() Implements IDuplexService.Connect
        Try
            Dim callbackChannel As IDuplexCallback = OperationContext.Current.GetCallbackChannel(Of IDuplexCallback)()
            SyncLock _sycnRoot
                If Not _callbackChannels.Contains(callbackChannel) Then
                    _callbackChannels.Add(callbackChannel)
                    Console.WriteLine("Added Callback Channel: {0}", callbackChannel.GetHashCode())
                End If

            End SyncLock
        Catch
        End Try
    End Sub


    Public Sub SendBroadcast(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                             ByVal arObject As Viev.FBM.Interface.Broadcast) Implements BostonWCFServiceLibrary.IDuplexService.SendBroadcast

        SyncLock _sycnRoot

            Console.WriteLine("Broadcasting Message: " & aiBroadcastType.ToString)

            For i As Integer = _callbackChannels.Count - 1 To 0 Step -1
                If (CType(_callbackChannels(i), ICommunicationObject)).State <> CommunicationState.Opened Then
                    Console.WriteLine("Detected Non-Open Callback Channel: {0}. Closing", _callbackChannels(i).GetHashCode())
                    _callbackChannels.RemoveAt(i)
                    Continue For
                End If

                Try

                    Console.WriteLine("Attempting Broadcasts")

                    Dim callbackChannel As IDuplexCallback = OperationContext.Current.GetCallbackChannel(Of IDuplexCallback)()
                    If Not (callbackChannel Is _callbackChannels(i)) Then
                        _callbackChannels(i).ReceiveBroadcast(aiBroadcastType, arObject)
                        Console.WriteLine("Pushed Broadcast on Callback Channel: {0}", _callbackChannels(i).GetHashCode())
                    End If

                Catch ex As Exception
                    Console.WriteLine("Service threw exception while communicating on Callback Channel: {0}", _callbackChannels(i).GetHashCode())
                    Console.WriteLine("Exception Type: {0} on Callback Chanel {1}. Description: {2}", ex.[GetType](), _callbackChannels(i).GetHashCode, ex.Message)
                    Console.WriteLine("Removing Callback Channel: {0}", _callbackChannels(i).GetHashCode)
                    _callbackChannels.RemoveAt(i)
                End Try
            Next

        End SyncLock

    End Sub


    Public Sub Disconnect() Implements IDuplexService.Disconnect
        Dim callbackChannel As IDuplexCallback = OperationContext.Current.GetCallbackChannel(Of IDuplexCallback)()
        Try
            SyncLock _sycnRoot
                If _callbackChannels.Remove(callbackChannel) Then
                    Console.WriteLine("Removed Callback Channel: {0}", callbackChannel.GetHashCode())
                End If

            End SyncLock
        Catch
        End Try
    End Sub
End Class

