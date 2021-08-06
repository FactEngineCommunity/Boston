Namespace FBM

    Interface iPageObject
        Sub MouseDown()
        Sub MouseMove()
        Sub MouseUp()
        Sub NodeDeleting()
        Sub NodeDeselected()
        Sub NodeModified()
        Sub NodeSelected()
        Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean)
        Sub Moved()
        Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer)
        Sub SetAppropriateColour()

        Sub EnableSaveButton()

        Property X As Integer
        Property Y As Integer
    End Interface

    Interface iTableNodePageObject
        Inherits FBM.iPageObject

        Sub CellClicked()        
        Sub NodeDoubleClicked()

    End Interface

    Interface iFactDataInstance

        Sub CellTextEdited()
    End Interface

    Interface iRolePageObject
        Inherits FBM.iPageObject

        Sub ValidateAnchorPoint()
    End Interface

End Namespace
