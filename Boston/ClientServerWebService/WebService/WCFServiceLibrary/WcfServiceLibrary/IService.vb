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

        <OperationContract(IsOneWay:=False)>  '20220327-VM-Was True
        Sub SendBroadcast(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                          ByRef arObject As Viev.FBM.Interface.Broadcast) '20220327-VM-Was ByVal

        <OperationContract(IsOneWay:=True)>
        Sub Disconnect()

    End Interface

    Interface IDuplexCallback

        <OperationContract(IsOneWay:=False)>  '20220327-VM-Was False
        Sub ReceiveBroadcast(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                             ByRef arObject As Viev.FBM.Interface.Broadcast) '20220327-VM-Was ByVal

    End Interface

End Module