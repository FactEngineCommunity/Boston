Imports System.Windows.Forms

''' <summary>
''' ========================================================================================================================
''' NB This Class holds the methods that are called from Boston to effect changes to the SharedModel, and trigger Events used by the Plugin.
''' The methods to effect changes to the SharedModel are prefixed 'Interface'
''' NB This Class has also a 'Partial Class' that holds the methods that must be implemented in Boston if the Plugin is to effect changes in Boston.
''' The methods to effect changes in Boston are prefixed 'Boston'
''' </summary>
''' <remarks></remarks>
Public Class FBMInterface
    Implements Viev.PluginFramework.IHost

    Public DockPanel As WeifenLuo.WinFormsUI.Docking.DockPanel

    Public WithEvents SharedModel As New Model

    Public Event EntityTypeAdded(ByRef arEntityType As EntityType) 'Triggered when an EntityType is added to the SharedModel by Boston, via the appropriate method in this Interface. Used by the Plugin that also uses this Interface.
    Public Event EntityTypeRemoved(ByRef arEntityType As EntityType)
    Public Event FactTypeAdded(ByRef arFactType As FactType) 'Triggered when a FactType is added to the SharedModel by Boston, via the appropriate method in this Interface. Used by the Plugin that also uses this Interface.
    Public Event FactTypeRemoved(ByRef arFactType As FactType)
    Public Event RoleConstraintAdded(ByRef arRoleConstraint As RoleConstraint) 'Triggered when an EntityType is added to the SharedModel by Boston, via the appropriate method in this Interface. Used by the Plugin that also uses this Interface.
    Public Event RoleConstraintRemoved(ByRef arRoleConstraint As RoleConstraint)
    Public Event ValueTypeAdded(ByRef arValueType As ValueType) 'Triggered when a ValueType is added to the SharedModel by Boston, via the appropriate method in this Interface. Used by the Plugin that also uses this Interface.
    Public Event ValueTypeRemoved(ByRef arValueType As ValueType)

    Public Event RDSEntityAdded(ByRef arEntity As RDS.Entity)
    Public Event RDSEntityRemoved(ByRef arEntity As RDS.Entity)
    Public Event RDSAttributeAdded(ByRef arAttribute As RDS.Attribute)
    Public Event RDSAttributeRemoved(ByRef arAttribute As RDS.Attribute)
    Public Event RDSIndexAdded(ByRef arIndex As RDS.Index)
    Public Event RDSIndexRemoved(ByRef arIndex As RDS.Index)
    Public Event RDSRelationAdded(ByRef arRelation As RDS.Relation)
    Public Event RDSRelationRemoved(ByRef arRelation As RDS.Relation)


    Public Overridable Sub InterfaceAddEntityType(ByVal arEntityType As Viev.FBM.Interface.EntityType)

        '--------------------------------------------------------------------------------------
        'Add the EntityType to the SharedModel
        'Me.SharedModel.EntityType.Add(arEntityType)

        RaiseEvent EntityTypeAdded(arEntityType) 'Used by the Plugin
    End Sub

    Public Overridable Sub InterfaceAddFactType(ByVal arFactType As FactType)

        '--------------------------------------------------------------------------------------
        'Add the FactType to the SharedModel
        'Me.SharedModel.FactType.Add(arFactType)

        RaiseEvent FactTypeAdded(arFactType) 'Used by the Plugin
    End Sub

    Public Overridable Sub InterfaceAddRoleConstraint(ByVal arRoleConstraint As RoleConstraint)

        '--------------------------------------------------------------------------------------
        'Add the RoleConstraint to the SharedModel
        'Me.SharedModel.RoleConstraint.Add(arRoleConstraint)

        RaiseEvent RoleConstraintAdded(arRoleConstraint) 'Used by the Plugin
    End Sub

    Public Overridable Sub InterfaceAddValueType(ByVal arValueType As Viev.FBM.Interface.ValueType)

        '--------------------------------------------------------------------------------------
        'Add the ValueType to the SharedModel
        'Me.SharedModel.ValueType.Add(arValueType)

        RaiseEvent ValueTypeAdded(arValueType) 'Used by the Plugin
    End Sub

End Class
