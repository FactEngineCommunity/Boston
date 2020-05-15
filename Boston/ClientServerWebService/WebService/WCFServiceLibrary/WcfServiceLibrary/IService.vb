Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports System.ServiceModel.Description

Public Module Module1

    <ServiceContract(CallbackContract:=GetType(IDuplexCallback))>
    Interface IDuplexService

        <OperationContract(IsOneWay:=True)>
        Sub Connect()

        <OperationContract(Isoneway:=True)>
        Sub SendBroadcast(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                          ByVal arObject As Viev.FBM.Interface.Broadcast)

        <OperationContract(IsOneWay:=True)>
        Sub Disconnect()

    End Interface

    Interface IDuplexCallback

        <OperationContract(IsOneWay:=True)>
        Sub ReceiveBroadcast(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                             ByVal arObject As Viev.FBM.Interface.Broadcast)

    End Interface

End Module