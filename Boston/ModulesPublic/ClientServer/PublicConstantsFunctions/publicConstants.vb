Public Module publicClientServerConstants

    ''' <summary>
    ''' Used for LogIn/LogOut logging.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum pcenumLogType
        LogIn
        LogOut
    End Enum

    Public Enum pcenumPermission
        FullRights
        NoRights
        Create
        Read
        Alter
    End Enum

    Public Enum pcenumPermissionClass
        User
        Group
    End Enum

    Public Enum pcenumFunction
        AddRoleToUser
        AlterEntityType
        AlterERD
        AlterFactType
        AlterModelNote
        AlterNamespace
        AlterProject
        AlterRoleConstraint
        AlterValueType
        CreateEntityType
        CreateERD               'Create Entity Relation Diagram
        CreateFactType
        CreateGroup
        CreateModel
        CreateModelNote
        CreateNamespace
        CreateProject
        CreateRoleConstraint
        CreateUser
        CreateValueType
        DeleteEntityType
        DeleteFactType
        DeleteModel
        DeleteModelNote
        DeleteRoleConstraint
        DeleteValueType
        FullPermission
        ReadERD
        ReadGroup
        ReadOwnProjects
        ReadOwnRoles
        ReadProjectsIncludedIn
        RemoveRoleFromUser
    End Enum

    Public Enum pcenumInvitationType
        UserToJoinProject
        GroupToJoinProject
        UserToJoinGroup
    End Enum

    Public Enum pcenumNotificationType
        GeneralNotification
    End Enum

End Module
