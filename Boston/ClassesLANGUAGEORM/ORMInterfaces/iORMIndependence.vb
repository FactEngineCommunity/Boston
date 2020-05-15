Namespace FBM

    Public Interface iFBMIndependence 
        Property IsIndependent() As Boolean
        Event IsIndependentChanged(ByVal abNewIsIndependent As Boolean)
        Sub SetIsIndependent(ByVal abNewIsIndependent As Boolean, ByVal abBroadcastInterfaceEvent As Boolean)
    End Interface

End Namespace
