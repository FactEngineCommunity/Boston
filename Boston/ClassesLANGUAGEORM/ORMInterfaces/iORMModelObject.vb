Namespace FBM

    Public Interface iModelObject
        Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                 Optional ByVal abCheckForErrors As Boolean = True,
                                 Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                 Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = True) As Boolean

        Event RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean)

    End Interface

End Namespace
