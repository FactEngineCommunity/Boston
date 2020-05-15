
'====================================================================================================================
'NB These are the functions to implement in Boston. The Plugin can call these methods to effect changes to a Model within Boston.
'NB This is a 'Partial' Class. For the main class, see Class FBMInterface
Partial Public Class FBMInterface

    Public Overridable Sub BostonChangeWorkingModel(ByVal asModelId As String)
    End Sub

    Public Overridable Sub BostonCreateValueType(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Function BostonGetModelIdByModelName(ByVal asModelName As String, ByRef asModelId As String) As Boolean
        Return True
    End Function

    Public Overridable Sub BostonReadValueType(ByRef arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonUpdateValueType(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonDeleteValueType(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonCreateEntityType(ByVal armodel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonReadEntityType(ByRef arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonUpdateEntityType(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonDeleteEntityType(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonCreateFactType(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonReadFactType(ByRef arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonUpdateFactType(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonDeleteFactType(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonCreateRoleConstraint(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonReadRoleConstraint(ByRef arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonUpdateRoleConstraint(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

    Public Overridable Sub BostonDeleteRoleConstraint(ByVal arModel As Viev.FBM.Interface.Model)
    End Sub

End Class
