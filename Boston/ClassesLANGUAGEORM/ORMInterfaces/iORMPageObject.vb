Imports Newtonsoft.Json

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

        <JsonIgnore()>
        Property Shape As MindFusion.Diagramming.ShapeNode

        Property X As Integer
        Property Y As Integer
    End Interface

    Interface iPageModelObject
        Inherits iPageObject

        Property SubtypeRelationship As FBM.SubtypeRelationshipInstance
    End Interface

    Interface iObjectTypePageObject 'for EntityType,ValueType,FactType Intances
        Inherits iPageObject

        Shadows Property Shape As tORMDiagrammingShapeNode
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

    Interface iRoleConstraintObject
        Inherits FBM.iPageObject

        Shadows Property Shape As FBM.RoleConstraintShape

    End Interface

End Namespace
