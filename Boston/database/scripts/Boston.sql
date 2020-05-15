-- DBWScript v4.1
-- Database: C:\ProgramData\Viev Pty Ltd\Boston\2.5.0.0\database\boston.vdb

CREATE TABLE [ClientServerFunction] (
	[FunctionName] TEXT(50) WITH COMPRESSION,
	[FunctionFullText] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([FunctionName])
);
ALTER TABLE [ClientServerFunction] DENY ZERO LENGTH [FunctionName];
ALTER TABLE [ClientServerFunction] ALLOW ZERO LENGTH [FunctionFullText];
CREATE TABLE [ClientServerGroup] (
	[Id] TEXT(40) WITH COMPRESSION,
	[GroupName] TEXT(255) WITH COMPRESSION,
	[CreatedByUserId] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([Id])
);
ALTER TABLE [ClientServerGroup] DENY ZERO LENGTH [Id];
ALTER TABLE [ClientServerGroup] DENY ZERO LENGTH [GroupName];
ALTER TABLE [ClientServerGroup] DENY ZERO LENGTH [CreatedByUserId];
CREATE TABLE [ClientServerGroupRole] (
	[GroupId] TEXT(40) WITH COMPRESSION,
	[RoleId] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([GroupId], [RoleId])
);
ALTER TABLE [ClientServerGroupRole] DENY ZERO LENGTH [GroupId];
ALTER TABLE [ClientServerGroupRole] DENY ZERO LENGTH [RoleId];
CREATE TABLE [ClientServerGroupUser] (
	[GroupId] TEXT(40) WITH COMPRESSION,
	[UserId] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([GroupId], [UserId])
);
ALTER TABLE [ClientServerGroupUser] DENY ZERO LENGTH [GroupId];
ALTER TABLE [ClientServerGroupUser] DENY ZERO LENGTH [UserId];
CREATE TABLE [ClientServerInvitation] (
	[InvitationType] TEXT(40) WITH COMPRESSION,
	[DateTime] DATETIME,
	[InvitingUserId] TEXT(40) WITH COMPRESSION,
	[InvitedUserId] TEXT(40) WITH COMPRESSION,
	[InvitedGroupId] TEXT(40) WITH COMPRESSION,
	[Tag] TEXT(50) WITH COMPRESSION,
	[Accepted] BIT NOT NULL DEFAULT 0,
	[Closed] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([InvitationType], [DateTime], [InvitingUserId])
);
ALTER TABLE [ClientServerInvitation] DENY ZERO LENGTH [InvitationType];
ALTER TABLE [ClientServerInvitation] DENY ZERO LENGTH [InvitingUserId];
ALTER TABLE [ClientServerInvitation] ALLOW ZERO LENGTH [InvitedUserId];
ALTER TABLE [ClientServerInvitation] ALLOW ZERO LENGTH [InvitedGroupId];
ALTER TABLE [ClientServerInvitation] DENY ZERO LENGTH [Tag];
ALTER TABLE [ClientServerInvitation] FORMAT [Accepted] SET "Yes/No";
ALTER TABLE [ClientServerInvitation] FORMAT [Closed] SET "Yes/No";
CREATE TABLE [ClientServerLog] (
	[DateTime] DATETIME,
	[UserId] TEXT(40) WITH COMPRESSION,
	[LogType] TEXT(10) WITH COMPRESSION,
	[IPAddress] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([DateTime], [UserId])
);
ALTER TABLE [ClientServerLog] DENY ZERO LENGTH [UserId];
ALTER TABLE [ClientServerLog] DENY ZERO LENGTH [LogType];
ALTER TABLE [ClientServerLog] ALLOW ZERO LENGTH [IPAddress];
CREATE TABLE [ClientServerNamespace] (
	[Id] TEXT(40) WITH COMPRESSION,
	[Namespace] TEXT(100) WITH COMPRESSION,
	[Number] TEXT(100) WITH COMPRESSION,
	[ProjectId] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([Id])
);
ALTER TABLE [ClientServerNamespace] DENY ZERO LENGTH [Id];
ALTER TABLE [ClientServerNamespace] DENY ZERO LENGTH [Namespace];
ALTER TABLE [ClientServerNamespace] ALLOW ZERO LENGTH [Number];
ALTER TABLE [ClientServerNamespace] DENY ZERO LENGTH [ProjectId];
CREATE TABLE [ClientServerProject] (
	[Id] TEXT(40) WITH COMPRESSION,
	[ProjectName] TEXT(60) WITH COMPRESSION,
	[CreatedByUserId] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([Id])
);
ALTER TABLE [ClientServerProject] DENY ZERO LENGTH [Id];
ALTER TABLE [ClientServerProject] DENY ZERO LENGTH [ProjectName];
ALTER TABLE [ClientServerProject] DENY ZERO LENGTH [CreatedByUserId];
CREATE TABLE [ClientServerProjectGroup] (
	[ProjectId] TEXT(40) WITH COMPRESSION,
	[GroupId] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ProjectId], [GroupId])
);
ALTER TABLE [ClientServerProjectGroup] DENY ZERO LENGTH [ProjectId];
ALTER TABLE [ClientServerProjectGroup] DENY ZERO LENGTH [GroupId];
CREATE TABLE [ClientServerProjectGroupPermission] (
	[ProjectId] TEXT(40) WITH COMPRESSION,
	[GroupId] TEXT(40) WITH COMPRESSION,
	[Permission] TEXT(20) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ProjectId], [GroupId], [Permission])
);
ALTER TABLE [ClientServerProjectGroupPermission] DENY ZERO LENGTH [ProjectId];
ALTER TABLE [ClientServerProjectGroupPermission] DENY ZERO LENGTH [GroupId];
ALTER TABLE [ClientServerProjectGroupPermission] DENY ZERO LENGTH [Permission];
CREATE TABLE [ClientServerProjectModel] (
	[ProjectId] TEXT(255) WITH COMPRESSION,
	[ModelId] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ProjectId], [ModelId])
);
ALTER TABLE [ClientServerProjectModel] ALLOW ZERO LENGTH [ProjectId];
ALTER TABLE [ClientServerProjectModel] ALLOW ZERO LENGTH [ModelId];
CREATE TABLE [ClientServerProjectUser] (
	[ProjectId] TEXT(40) WITH COMPRESSION,
	[UserId] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ProjectId], [UserId])
);
ALTER TABLE [ClientServerProjectUser] DENY ZERO LENGTH [ProjectId];
ALTER TABLE [ClientServerProjectUser] DENY ZERO LENGTH [UserId];
CREATE TABLE [ClientServerProjectUserPermission] (
	[ProjectId] TEXT(40) WITH COMPRESSION,
	[UserId] TEXT(40) WITH COMPRESSION,
	[Permission] TEXT(20) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ProjectId], [UserId], [Permission])
);
ALTER TABLE [ClientServerProjectUserPermission] DENY ZERO LENGTH [ProjectId];
ALTER TABLE [ClientServerProjectUserPermission] DENY ZERO LENGTH [UserId];
ALTER TABLE [ClientServerProjectUserPermission] DENY ZERO LENGTH [Permission];
CREATE TABLE [ClientServerRole] (
	[Id] TEXT(40) WITH COMPRESSION,
	[RoleName] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([Id])
);
ALTER TABLE [ClientServerRole] DENY ZERO LENGTH [Id];
ALTER TABLE [ClientServerRole] DENY ZERO LENGTH [RoleName];
CREATE TABLE [ClientServerRoleFunction] (
	[RoleId] TEXT(40) WITH COMPRESSION,
	[FunctionName] TEXT(50) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([RoleId], [FunctionName])
);
ALTER TABLE [ClientServerRoleFunction] DENY ZERO LENGTH [RoleId];
ALTER TABLE [ClientServerRoleFunction] DENY ZERO LENGTH [FunctionName];
CREATE TABLE [ClientServerUser] (
	[Id] TEXT(40) WITH COMPRESSION,
	[Username] TEXT(60) WITH COMPRESSION,
	[PasswordHash] TEXT(255) WITH COMPRESSION,
	[ResetPassword] BIT NOT NULL DEFAULT 0,
	[IsActive] BIT NOT NULL DEFAULT 0,
	[IsSuperUser] BIT NOT NULL DEFAULT 0,
	[FirstName] TEXT(60) WITH COMPRESSION,
	[LastName] TEXT(60) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([Id])
);
ALTER TABLE [ClientServerUser] DENY ZERO LENGTH [Id];
ALTER TABLE [ClientServerUser] DENY ZERO LENGTH [Username];
ALTER TABLE [ClientServerUser] ALLOW ZERO LENGTH [PasswordHash];
ALTER TABLE [ClientServerUser] ALLOW ZERO LENGTH [FirstName];
ALTER TABLE [ClientServerUser] ALLOW ZERO LENGTH [LastName];
ALTER TABLE [ClientServerUser] FORMAT [ResetPassword] SET "Yes/No";
ALTER TABLE [ClientServerUser] FORMAT [IsActive] SET "Yes/No";
ALTER TABLE [ClientServerUser] FORMAT [IsSuperUser] SET "Yes/No";
CREATE TABLE [ClientServerUserRole] (
	[UserId] TEXT(40) WITH COMPRESSION,
	[RoleId] TEXT(40) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([UserId], [RoleId])
);
ALTER TABLE [ClientServerUserRole] DENY ZERO LENGTH [UserId];
ALTER TABLE [ClientServerUserRole] DENY ZERO LENGTH [RoleId];
CREATE TABLE [DatabaseUpgrade] (
	[UpgradeId] LONG,
	[FromVersion] TEXT(10) WITH COMPRESSION,
	[ToVersion] TEXT(10) WITH COMPRESSION,
	[SuccessfulImplementation] BIT NOT NULL,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([UpgradeId]),
	CONSTRAINT [SecondaryUniquekey] UNIQUE ([FromVersion], [ToVersion])
);
ALTER TABLE [DatabaseUpgrade] DENY ZERO LENGTH [FromVersion];
ALTER TABLE [DatabaseUpgrade] DENY ZERO LENGTH [ToVersion];
CREATE TABLE [DatabaseValidationSQL] (
	[SequenceNr] LONG DEFAULT 0,
	[ShortDescription] TEXT(255) WITH COMPRESSION,
	[ValidationCheckDescription] MEMO WITH COMPRESSION,
	[ValidationSQL] MEMO WITH COMPRESSION,
	[PossibleCauses] MEMO WITH COMPRESSION,
	[PossibleRemedies] MEMO WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([SequenceNr])
);
ALTER TABLE [DatabaseValidationSQL] ALLOW ZERO LENGTH [ShortDescription];
ALTER TABLE [DatabaseValidationSQL] ALLOW ZERO LENGTH [ValidationCheckDescription];
ALTER TABLE [DatabaseValidationSQL] ALLOW ZERO LENGTH [ValidationSQL];
ALTER TABLE [DatabaseValidationSQL] ALLOW ZERO LENGTH [PossibleCauses];
ALTER TABLE [DatabaseValidationSQL] ALLOW ZERO LENGTH [PossibleRemedies];
CREATE TABLE [LanguagePhrase] (
	[PhraseId] LONG,
	[PhraseType] TEXT(255) WITH COMPRESSION,
	[ResolvesToPhraseId] LONG,
	[Example] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([PhraseId])
);
ALTER TABLE [LanguagePhrase] ALLOW ZERO LENGTH [PhraseType];
ALTER TABLE [LanguagePhrase] ALLOW ZERO LENGTH [Example];
CREATE TABLE [LanguagePhraseTokenSequenceWordSense] (
	[PhraseId] LONG,
	[Token] TEXT(255) WITH COMPRESSION,
	[SequenceNr] LONG,
	[WordSense] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([PhraseId], [Token], [SequenceNr], [WordSense])
);
ALTER TABLE [LanguagePhraseTokenSequenceWordSense] ALLOW ZERO LENGTH [Token];
ALTER TABLE [LanguagePhraseTokenSequenceWordSense] ALLOW ZERO LENGTH [WordSense];
CREATE TABLE [MetaModelConcept] (
	[Symbol] TEXT(100) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] UNIQUE ([Symbol])
);
ALTER TABLE [MetaModelConcept] ALLOW ZERO LENGTH [Symbol];
CREATE TABLE [MetaModelEntityType] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(255) WITH COMPRESSION,
	[EntityTypeId] TEXT(100) WITH COMPRESSION,
	[EntityTypeName] TEXT(100) WITH COMPRESSION,
	[ValueTypeId] TEXT(100) WITH COMPRESSION,
	[ReferenceMode] TEXT(255) WITH COMPRESSION,
	[PreferredIdentifierRCId] TEXT(255) WITH COMPRESSION,
	[IsMDAModelElement] BIT NOT NULL DEFAULT 0,
	[IsIndependant] BIT NOT NULL DEFAULT 0,
	[IsPersonal] BIT NOT NULL DEFAULT 0,
	[GUID] TEXT(255) WITH COMPRESSION,
	[IsAbsorbed] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [EntityTypeId])
);
ALTER TABLE [MetaModelEntityType] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelEntityType] ALLOW ZERO LENGTH [EntityTypeId];
ALTER TABLE [MetaModelEntityType] ALLOW ZERO LENGTH [EntityTypeName];
ALTER TABLE [MetaModelEntityType] ALLOW ZERO LENGTH [ValueTypeId];
ALTER TABLE [MetaModelEntityType] ALLOW ZERO LENGTH [ReferenceMode];
ALTER TABLE [MetaModelEntityType] ALLOW ZERO LENGTH [PreferredIdentifierRCId];
ALTER TABLE [MetaModelEntityType] ALLOW ZERO LENGTH [GUID];
ALTER TABLE [MetaModelEntityType] FORMAT [IsMDAModelElement] SET "Yes/No";
ALTER TABLE [MetaModelEntityType] FORMAT [IsIndependant] SET "Yes/No";
ALTER TABLE [MetaModelEntityType] FORMAT [IsPersonal] SET "Yes/No";
ALTER TABLE [MetaModelEntityType] FORMAT [IsAbsorbed] SET "Yes/No";
CREATE TABLE [MetaModelFact] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(255) WITH COMPRESSION,
	[Symbol] TEXT(100) WITH COMPRESSION,
	[FactTypeId] TEXT(100) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [Symbol])
);
ALTER TABLE [MetaModelFact] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelFact] ALLOW ZERO LENGTH [Symbol];
ALTER TABLE [MetaModelFact] ALLOW ZERO LENGTH [FactTypeId];
CREATE TABLE [MetaModelFactData] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[FactTypeId] TEXT(100) WITH COMPRESSION,
	[FactSymbol] TEXT(100) WITH COMPRESSION,
	[RoleId] TEXT(50) WITH COMPRESSION,
	[ValueSymbol] TEXT(100) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [FactSymbol], [RoleId])
);
ALTER TABLE [MetaModelFactData] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelFactData] ALLOW ZERO LENGTH [FactTypeId];
ALTER TABLE [MetaModelFactData] ALLOW ZERO LENGTH [FactSymbol];
ALTER TABLE [MetaModelFactData] ALLOW ZERO LENGTH [RoleId];
ALTER TABLE [MetaModelFactData] ALLOW ZERO LENGTH [ValueSymbol];
CREATE TABLE [MetaModelFactType] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(255) WITH COMPRESSION,
	[FactTypeId] TEXT(100) WITH COMPRESSION,
	[FactTypeName] TEXT(50) WITH COMPRESSION,
	[IsObjectified] BIT NOT NULL,
	[IsCoreFactType] BIT NOT NULL,
	[IsPreferredReferenceMode] BIT NOT NULL,
	[ObjectifyingEntityTypeId] TEXT(255) WITH COMPRESSION,
	[IsSubtypeFactType] BIT NOT NULL DEFAULT 0,
	[IsMDAModelElement] BIT NOT NULL DEFAULT 0,
	[IsDerived] BIT NOT NULL DEFAULT 0,
	[IsStored] BIT NOT NULL DEFAULT 0,
	[DerivationText] MEMO WITH COMPRESSION,
	[IsLinkFactType] BIT NOT NULL DEFAULT 0,
	[GUID] TEXT(255) WITH COMPRESSION,
	[LinkFactTypeRoleId] TEXT(255) WITH COMPRESSION,
	[IsIndependent] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [FactTypeId])
);
ALTER TABLE [MetaModelFactType] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelFactType] ALLOW ZERO LENGTH [FactTypeId];
ALTER TABLE [MetaModelFactType] ALLOW ZERO LENGTH [FactTypeName];
ALTER TABLE [MetaModelFactType] ALLOW ZERO LENGTH [ObjectifyingEntityTypeId];
ALTER TABLE [MetaModelFactType] ALLOW ZERO LENGTH [DerivationText];
ALTER TABLE [MetaModelFactType] ALLOW ZERO LENGTH [GUID];
ALTER TABLE [MetaModelFactType] ALLOW ZERO LENGTH [LinkFactTypeRoleId];
ALTER TABLE [MetaModelFactType] FORMAT [IsObjectified] SET "Yes/No";
ALTER TABLE [MetaModelFactType] FORMAT [IsCoreFactType] SET "Yes/No";
ALTER TABLE [MetaModelFactType] FORMAT [IsPreferredReferenceMode] SET "Yes/No";
ALTER TABLE [MetaModelFactType] FORMAT [IsSubtypeFactType] SET "Yes/No";
ALTER TABLE [MetaModelFactType] FORMAT [IsMDAModelElement] SET "Yes/No";
ALTER TABLE [MetaModelFactType] FORMAT [IsDerived] SET "Yes/No";
ALTER TABLE [MetaModelFactType] FORMAT [IsStored] SET "Yes/No";
ALTER TABLE [MetaModelFactType] FORMAT [IsLinkFactType] SET "Yes/No";
ALTER TABLE [MetaModelFactType] FORMAT [IsIndependent] SET "Yes/No";
CREATE TABLE [MetaModelFactTypeReading] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(50) WITH COMPRESSION,
	[FactTypeReadingId] TEXT(50) WITH COMPRESSION,
	[FactTypeId] TEXT(50) WITH COMPRESSION,
	[FrontText] TEXT(255) WITH COMPRESSION,
	[FollowingText] TEXT(255) WITH COMPRESSION,
	[TypedPredicateId] TEXT(36) WITH COMPRESSION NOT NULL,
	[IsPreferred] BIT NOT NULL,
	[IsPreferredForPredicate] BIT NOT NULL,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [FactTypeReadingId])
);
ALTER TABLE [MetaModelFactTypeReading] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelFactTypeReading] ALLOW ZERO LENGTH [FactTypeReadingId];
ALTER TABLE [MetaModelFactTypeReading] ALLOW ZERO LENGTH [FactTypeId];
ALTER TABLE [MetaModelFactTypeReading] ALLOW ZERO LENGTH [FrontText];
ALTER TABLE [MetaModelFactTypeReading] ALLOW ZERO LENGTH [FollowingText];
ALTER TABLE [MetaModelFactTypeReading] ALLOW ZERO LENGTH [TypedPredicateId];
CREATE TABLE [MetaModelJoinPathRole] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[RoleConstraintArgumentId] TEXT(100) WITH COMPRESSION,
	[RoleId] TEXT(100) WITH COMPRESSION,
	[SequenceNr] LONG,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [RoleConstraintArgumentId], [RoleId], [SequenceNr])
);
ALTER TABLE [MetaModelJoinPathRole] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelJoinPathRole] ALLOW ZERO LENGTH [RoleConstraintArgumentId];
ALTER TABLE [MetaModelJoinPathRole] ALLOW ZERO LENGTH [RoleId];
CREATE TABLE [MetaModelModel] (
	[ModelId] TEXT(100) WITH COMPRESSION,
	[ModelName] TEXT(255) WITH COMPRESSION,
	[ShortDescription] TEXT(255) WITH COMPRESSION,
	[LongDescription] MEMO WITH COMPRESSION,
	[EnterpriseId] TEXT(100) WITH COMPRESSION,
	[SubjectAreaId] TEXT(50) WITH COMPRESSION,
	[ProjectId] TEXT(100) WITH COMPRESSION,
	[ProjectPhaseId] LONG DEFAULT 0,
	[SolutionId] TEXT(100) WITH COMPRESSION,
	[IsConceptualModel] BIT NOT NULL,
	[IsPhysicalModel] BIT NOT NULL,
	[IsNamespace] BIT NOT NULL,
	[IsEnterpriseModel] BIT NOT NULL,
	[TargetDatabaseType] TEXT(50) WITH COMPRESSION,
	[TargetDatabaseConnectionString] MEMO WITH COMPRESSION,
	[NamespaceId] TEXT(255) WITH COMPRESSION,
	[CreatedByUserId] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId])
);
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [ModelName];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [ShortDescription];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [LongDescription];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [EnterpriseId];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [SubjectAreaId];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [ProjectId];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [SolutionId];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [TargetDatabaseType];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [TargetDatabaseConnectionString];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [NamespaceId];
ALTER TABLE [MetaModelModel] ALLOW ZERO LENGTH [CreatedByUserId];
ALTER TABLE [MetaModelModel] FORMAT [IsConceptualModel] SET "Yes/No";
ALTER TABLE [MetaModelModel] FORMAT [IsPhysicalModel] SET "Yes/No";
ALTER TABLE [MetaModelModel] FORMAT [IsNamespace] SET "Yes/No";
ALTER TABLE [MetaModelModel] FORMAT [IsEnterpriseModel] SET "Yes/No";
CREATE TABLE [MetaModelModelDictionary] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[Symbol] TEXT(100) WITH COMPRESSION,
	[ShortDescription] TEXT(255) WITH COMPRESSION,
	[LongDescription] MEMO WITH COMPRESSION,
	[IsEntityType] BIT NOT NULL,
	[IsValueType] BIT NOT NULL,
	[IsFactType] BIT NOT NULL,
	[IsFact] BIT NOT NULL,
	[IsValue] BIT NOT NULL,
	[IsRoleConstraint] BIT NOT NULL,
	[IsModelNote] BIT NOT NULL DEFAULT 0,
	[IsGeneralConcept] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [Symbol])
);
ALTER TABLE [MetaModelModelDictionary] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelModelDictionary] ALLOW ZERO LENGTH [Symbol];
ALTER TABLE [MetaModelModelDictionary] ALLOW ZERO LENGTH [ShortDescription];
ALTER TABLE [MetaModelModelDictionary] ALLOW ZERO LENGTH [LongDescription];
ALTER TABLE [MetaModelModelDictionary] FORMAT [IsEntityType] SET "Yes/No";
ALTER TABLE [MetaModelModelDictionary] FORMAT [IsValueType] SET "Yes/No";
ALTER TABLE [MetaModelModelDictionary] FORMAT [IsFactType] SET "Yes/No";
ALTER TABLE [MetaModelModelDictionary] FORMAT [IsFact] SET "Yes/No";
ALTER TABLE [MetaModelModelDictionary] FORMAT [IsValue] SET "Yes/No";
ALTER TABLE [MetaModelModelDictionary] FORMAT [IsRoleConstraint] SET "Yes/No";
ALTER TABLE [MetaModelModelDictionary] FORMAT [IsModelNote] SET "Yes/No";
ALTER TABLE [MetaModelModelDictionary] FORMAT [IsGeneralConcept] SET "Yes/No";
CREATE TABLE [MetaModelModelNote] (
	[ModelNoteId] TEXT(255) WITH COMPRESSION,
	[Note] MEMO WITH COMPRESSION,
	[JoinedObjectTypeId] TEXT(255) WITH COMPRESSION,
	[IsMDAModelElement] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelNoteId])
);
ALTER TABLE [MetaModelModelNote] ALLOW ZERO LENGTH [ModelNoteId];
ALTER TABLE [MetaModelModelNote] ALLOW ZERO LENGTH [Note];
ALTER TABLE [MetaModelModelNote] ALLOW ZERO LENGTH [JoinedObjectTypeId];
ALTER TABLE [MetaModelModelNote] FORMAT [IsMDAModelElement] SET "Yes/No";
CREATE TABLE [MetaModelPredicatePart] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(50) WITH COMPRESSION,
	[FactTypeReadingId] TEXT(50) WITH COMPRESSION,
	[SequenceNr] LONG DEFAULT 0,
	[Symbol1] TEXT(100) WITH COMPRESSION,
	[Symbol2] TEXT(100) WITH COMPRESSION,
	[PredicatePartText] TEXT(255) WITH COMPRESSION,
	[RoleId] TEXT(50) WITH COMPRESSION,
	[PreboundText] TEXT(255) WITH COMPRESSION,
	[PostboundText] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [FactTypeReadingId], [SequenceNr])
);
ALTER TABLE [MetaModelPredicatePart] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelPredicatePart] ALLOW ZERO LENGTH [FactTypeReadingId];
ALTER TABLE [MetaModelPredicatePart] ALLOW ZERO LENGTH [Symbol1];
ALTER TABLE [MetaModelPredicatePart] ALLOW ZERO LENGTH [Symbol2];
ALTER TABLE [MetaModelPredicatePart] ALLOW ZERO LENGTH [PredicatePartText];
ALTER TABLE [MetaModelPredicatePart] ALLOW ZERO LENGTH [RoleId];
ALTER TABLE [MetaModelPredicatePart] ALLOW ZERO LENGTH [PreboundText];
ALTER TABLE [MetaModelPredicatePart] ALLOW ZERO LENGTH [PostboundText];
CREATE TABLE [MetaModelRole] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[RoleId] TEXT(100) WITH COMPRESSION,
	[RoleName] TEXT(50) WITH COMPRESSION,
	[FactTypeId] TEXT(100) WITH COMPRESSION,
	[PartOfKey] BIT NOT NULL,
	[Cardinality] LONG,
	[SequenceNr] LONG,
	[JoinsEntityTypeId] TEXT(100) WITH COMPRESSION,
	[JoinsValueTypeId] TEXT(100) WITH COMPRESSION,
	[JoinsNestedFactTypeId] TEXT(100) WITH COMPRESSION,
	[TypeOfJoin] LONG,
	[IsMandatory] BIT NOT NULL,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [RoleId])
);
ALTER TABLE [MetaModelRole] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelRole] ALLOW ZERO LENGTH [RoleId];
ALTER TABLE [MetaModelRole] ALLOW ZERO LENGTH [RoleName];
ALTER TABLE [MetaModelRole] ALLOW ZERO LENGTH [FactTypeId];
ALTER TABLE [MetaModelRole] ALLOW ZERO LENGTH [JoinsEntityTypeId];
ALTER TABLE [MetaModelRole] ALLOW ZERO LENGTH [JoinsValueTypeId];
ALTER TABLE [MetaModelRole] ALLOW ZERO LENGTH [JoinsNestedFactTypeId];
CREATE TABLE [MetaModelRoleConstraint] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[RoleConstraintId] TEXT(100) WITH COMPRESSION,
	[RoleConstraintName] TEXT(50) WITH COMPRESSION,
	[RoleConstraintType] TEXT(50) WITH COMPRESSION,
	[RingConstraintType] TEXT(50) WITH COMPRESSION,
	[LevelNr] LONG DEFAULT 0,
	[IsPreferredUniqueness] BIT NOT NULL,
	[IsDeontic] BIT NOT NULL,
	[MinimumFrequencyCount] LONG DEFAULT 0,
	[MaximumFrequencyCount] LONG DEFAULT 0,
	[Cardinality] LONG DEFAULT 0,
	[CardinalityRangeType] TEXT(50) WITH COMPRESSION,
	[ValueRangeType] TEXT(255) WITH COMPRESSION,
	[IsMDAModelElement] BIT NOT NULL DEFAULT 0,
	[GUID] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [RoleConstraintId])
);
ALTER TABLE [MetaModelRoleConstraint] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelRoleConstraint] ALLOW ZERO LENGTH [RoleConstraintId];
ALTER TABLE [MetaModelRoleConstraint] ALLOW ZERO LENGTH [RoleConstraintName];
ALTER TABLE [MetaModelRoleConstraint] ALLOW ZERO LENGTH [RoleConstraintType];
ALTER TABLE [MetaModelRoleConstraint] ALLOW ZERO LENGTH [RingConstraintType];
ALTER TABLE [MetaModelRoleConstraint] ALLOW ZERO LENGTH [CardinalityRangeType];
ALTER TABLE [MetaModelRoleConstraint] ALLOW ZERO LENGTH [ValueRangeType];
ALTER TABLE [MetaModelRoleConstraint] ALLOW ZERO LENGTH [GUID];
ALTER TABLE [MetaModelRoleConstraint] FORMAT [IsPreferredUniqueness] SET "Yes/No";
ALTER TABLE [MetaModelRoleConstraint] FORMAT [IsDeontic] SET "Yes/No";
ALTER TABLE [MetaModelRoleConstraint] FORMAT [IsMDAModelElement] SET "Yes/No";
CREATE TABLE [MetamodelRoleConstraintArgument] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[Id] TEXT(100) WITH COMPRESSION,
	[RoleConstraintId] TEXT(100) WITH COMPRESSION,
	[SequenceNr] LONG,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [Id], [RoleConstraintId], [SequenceNr]),
	CONSTRAINT [SecondaryKey] UNIQUE ([ModelId], [Id])
);
ALTER TABLE [MetamodelRoleConstraintArgument] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetamodelRoleConstraintArgument] ALLOW ZERO LENGTH [Id];
ALTER TABLE [MetamodelRoleConstraintArgument] ALLOW ZERO LENGTH [RoleConstraintId];
CREATE TABLE [MetaModelRoleConstraintRole] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[RoleConstraintId] TEXT(100) WITH COMPRESSION,
	[RoleId] TEXT(100) WITH COMPRESSION,
	[SequenceNr] LONG DEFAULT 0,
	[IsEntry] BIT NOT NULL,
	[IsExit] BIT NOT NULL,
	[ArgumentId] TEXT(100) WITH COMPRESSION,
	[ArgumentSequenceNr] LONG,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [RoleConstraintId], [RoleId])
);
ALTER TABLE [MetaModelRoleConstraintRole] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelRoleConstraintRole] ALLOW ZERO LENGTH [RoleConstraintId];
ALTER TABLE [MetaModelRoleConstraintRole] ALLOW ZERO LENGTH [RoleId];
ALTER TABLE [MetaModelRoleConstraintRole] ALLOW ZERO LENGTH [ArgumentId];
ALTER TABLE [MetaModelRoleConstraintRole] FORMAT [IsEntry] SET "Yes/No";
ALTER TABLE [MetaModelRoleConstraintRole] FORMAT [IsExit] SET "Yes/No";
CREATE TABLE [MetaModelSubtypeRelationship] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[ObjectTypeId] TEXT(100) WITH COMPRESSION,
	[SupertypeObjectTypeId] TEXT(100) WITH COMPRESSION,
	[SubtypingFactTypeId] TEXT(255) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [ObjectTypeId], [SupertypeObjectTypeId])
);
ALTER TABLE [MetaModelSubtypeRelationship] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelSubtypeRelationship] ALLOW ZERO LENGTH [ObjectTypeId];
ALTER TABLE [MetaModelSubtypeRelationship] ALLOW ZERO LENGTH [SupertypeObjectTypeId];
ALTER TABLE [MetaModelSubtypeRelationship] ALLOW ZERO LENGTH [SubtypingFactTypeId];
CREATE TABLE [MetaModelValueType] (
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME,
	[ModelId] TEXT(255) WITH COMPRESSION,
	[ValueTypeId] TEXT(100) WITH COMPRESSION,
	[ValueTypeName] TEXT(100) WITH COMPRESSION,
	[DataType] TEXT(50) WITH COMPRESSION,
	[DataTypeLength] LONG,
	[DataTypePrecision] LONG,
	[IsMDAModelElement] BIT NOT NULL DEFAULT 0,
	[GUID] TEXT(255) WITH COMPRESSION NOT NULL,
	[IsIndependent] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [ValueTypeId])
);
ALTER TABLE [MetaModelValueType] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelValueType] ALLOW ZERO LENGTH [ValueTypeId];
ALTER TABLE [MetaModelValueType] ALLOW ZERO LENGTH [ValueTypeName];
ALTER TABLE [MetaModelValueType] ALLOW ZERO LENGTH [DataType];
ALTER TABLE [MetaModelValueType] ALLOW ZERO LENGTH [GUID];
ALTER TABLE [MetaModelValueType] FORMAT [IsMDAModelElement] SET "Yes/No";
ALTER TABLE [MetaModelValueType] FORMAT [IsIndependent] SET "Yes/No";
CREATE TABLE [MetaModelValueTypeValueConstraint] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(255) WITH COMPRESSION,
	[ValueTypeId] TEXT(100) WITH COMPRESSION,
	[Symbol] TEXT(100) WITH COMPRESSION,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [ValueTypeId], [Symbol])
);
ALTER TABLE [MetaModelValueTypeValueConstraint] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [MetaModelValueTypeValueConstraint] ALLOW ZERO LENGTH [ValueTypeId];
ALTER TABLE [MetaModelValueTypeValueConstraint] ALLOW ZERO LENGTH [Symbol];
CREATE TABLE [ModelConceptInstance] (
	[StartDate] DATETIME,
	[EndDate] DATETIME,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[PageId] TEXT(100) WITH COMPRESSION,
	[Symbol] TEXT(100) WITH COMPRESSION,
	[ConceptType] TEXT(50) WITH COMPRESSION,
	[RoleId] TEXT(255) WITH COMPRESSION,
	[X] LONG DEFAULT 0,
	[Y] LONG DEFAULT 0,
	[Orientation] LONG DEFAULT 0,
	[IsVisible] BIT NOT NULL,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([ModelId], [PageId], [Symbol], [ConceptType], [RoleId])
);
ALTER TABLE [ModelConceptInstance] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [ModelConceptInstance] ALLOW ZERO LENGTH [PageId];
ALTER TABLE [ModelConceptInstance] ALLOW ZERO LENGTH [Symbol];
ALTER TABLE [ModelConceptInstance] ALLOW ZERO LENGTH [ConceptType];
ALTER TABLE [ModelConceptInstance] ALLOW ZERO LENGTH [RoleId];
ALTER TABLE [ModelConceptInstance] FORMAT [IsVisible] SET "Yes/No";
CREATE TABLE [ModelLanguage] (
	[LanguageId] TEXT(50) WITH COMPRESSION,
	[LanguageName] TEXT(100) WITH COMPRESSION,
	[IsNaturalLanguage] BIT NOT NULL,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([LanguageId])
);
ALTER TABLE [ModelLanguage] ALLOW ZERO LENGTH [LanguageId];
ALTER TABLE [ModelLanguage] ALLOW ZERO LENGTH [LanguageName];
ALTER TABLE [ModelLanguage] FORMAT [IsNaturalLanguage] SET "Yes/No";
CREATE TABLE [ModelPage] (
	[PageId] TEXT(100) WITH COMPRESSION,
	[PageName] TEXT(50) WITH COMPRESSION,
	[ModelId] TEXT(100) WITH COMPRESSION,
	[LanguageId] LONG DEFAULT 0,
	[IsCoreModelPage] BIT NOT NULL,
	[ShowFacts] BIT NOT NULL,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([PageId])
);
ALTER TABLE [ModelPage] ALLOW ZERO LENGTH [PageId];
ALTER TABLE [ModelPage] ALLOW ZERO LENGTH [PageName];
ALTER TABLE [ModelPage] ALLOW ZERO LENGTH [ModelId];
ALTER TABLE [ModelPage] FORMAT [IsCoreModelPage] SET "Yes/No";
ALTER TABLE [ModelPage] FORMAT [ShowFacts] SET "Yes/No";
CREATE TABLE [ReferenceDataType] (
	[reference_data_type_id] SMALLINT,
	[Data_Type] TEXT(10) WITH COMPRESSION,
	[null_value] BIT NOT NULL,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([reference_data_type_id])
);
ALTER TABLE [ReferenceDataType] DENY ZERO LENGTH [Data_Type];
CREATE TABLE [ReferenceField] (
	[reference_table_id] SMALLINT,
	[reference_field_Id] SMALLINT,
	[reference_field_label] TEXT(100) WITH COMPRESSION,
	[reference_data_type_id] SMALLINT,
	[cardinality] SMALLINT,
	[required] BIT NOT NULL,
	[system] BIT NOT NULL
);
ALTER TABLE [ReferenceField] DENY ZERO LENGTH [reference_field_label];
CREATE TABLE [ReferenceFieldValue] (
	[reference_table_Id] SMALLINT,
	[row_id] TEXT(50) WITH COMPRESSION,
	[reference_field_id] SMALLINT NOT NULL,
	[Data] TEXT(255) WITH COMPRESSION
);
ALTER TABLE [ReferenceFieldValue] ALLOW ZERO LENGTH [row_id];
ALTER TABLE [ReferenceFieldValue] DENY ZERO LENGTH [Data];
CREATE TABLE [ReferenceTable] (
	[reference_table_id] SMALLINT,
	[reference_table_name] TEXT(30) WITH COMPRESSION,
	[system] BIT NOT NULL,
	CONSTRAINT [PrimaryKey] PRIMARY KEY ([reference_table_id])
);
ALTER TABLE [ReferenceTable] DENY ZERO LENGTH [reference_table_name];
CREATE INDEX [CreatedByUserId]
	ON [ClientServerGroup] ([CreatedByUserId]);
CREATE INDEX [RoleId]
	ON [ClientServerGroupRole] ([RoleId]);
CREATE INDEX [UserId]
	ON [ClientServerGroupUser] ([UserId]);
CREATE INDEX [InvitedGroupId]
	ON [ClientServerInvitation] ([InvitedGroupId]);
CREATE INDEX [InvitedUserId]
	ON [ClientServerInvitation] ([InvitedUserId]);
CREATE INDEX [UserId]
	ON [ClientServerLog] ([UserId]);
CREATE INDEX [Number]
	ON [ClientServerNamespace] ([Number]);
CREATE INDEX [ProjectId]
	ON [ClientServerNamespace] ([ProjectId]);
CREATE INDEX [CreatedByUserId]
	ON [ClientServerProject] ([CreatedByUserId]);
CREATE INDEX [GroupId]
	ON [ClientServerProjectGroup] ([GroupId]);
CREATE INDEX [UserId]
	ON [ClientServerProjectGroupPermission] ([GroupId]);
CREATE INDEX [UserId]
	ON [ClientServerProjectUser] ([UserId]);
CREATE INDEX [UserId]
	ON [ClientServerProjectUserPermission] ([UserId]);
CREATE INDEX [FunctionId]
	ON [ClientServerRoleFunction] ([FunctionName]);
CREATE INDEX [RoleId]
	ON [ClientServerUserRole] ([RoleId]);
CREATE INDEX [ResolvesToPhraseId]
	ON [LanguagePhrase] ([ResolvesToPhraseId]);
CREATE INDEX [GUID]
	ON [MetaModelEntityType] ([GUID]);
CREATE INDEX [ModelId]
	ON [MetaModelEntityType] ([ModelId]);
CREATE INDEX [PreferredIdentifierRCId]
	ON [MetaModelEntityType] ([PreferredIdentifierRCId]);
CREATE INDEX [entity_id]
	ON [MetaModelEntityType] ([EntityTypeId]);
CREATE INDEX [value_type_id]
	ON [MetaModelEntityType] ([ValueTypeId]);
CREATE INDEX [ModelId]
	ON [MetaModelFact] ([ModelId]);
CREATE INDEX [fact_type_id]
	ON [MetaModelFact] ([Symbol]);
CREATE INDEX [fact_type_id1]
	ON [MetaModelFact] ([FactTypeId]);
CREATE INDEX [MetaModelFactDataRoleId]
	ON [MetaModelFactData] ([RoleId]);
CREATE INDEX [fact_type_id]
	ON [MetaModelFactData] ([FactSymbol]);
CREATE INDEX [fact_type_id1]
	ON [MetaModelFactData] ([FactTypeId]);
CREATE INDEX [model_id]
	ON [MetaModelFactData] ([ModelId]);
CREATE INDEX [symbol_id]
	ON [MetaModelFactData] ([ValueSymbol]);
CREATE INDEX [GUID1]
	ON [MetaModelFactType] ([GUID]);
CREATE INDEX [LinkFactTypeRoleId]
	ON [MetaModelFactType] ([LinkFactTypeRoleId]);
CREATE INDEX [ModelId]
	ON [MetaModelFactType] ([ModelId]);
CREATE INDEX [ObjectifiedEntityTypeId]
	ON [MetaModelFactType] ([ObjectifyingEntityTypeId]);
CREATE INDEX [guid]
	ON [MetaModelFactType] ([FactTypeId]);
CREATE INDEX [fact_type_id]
	ON [MetaModelFactTypeReading] ([FactTypeId]);
CREATE INDEX [fact_type_reading_id]
	ON [MetaModelFactTypeReading] ([FactTypeReadingId]);
CREATE INDEX [model_id]
	ON [MetaModelFactTypeReading] ([ModelId]);
CREATE INDEX [ModelId]
	ON [MetaModelJoinPathRole] ([ModelId]);
CREATE INDEX [RoleConstraintArgumentId]
	ON [MetaModelJoinPathRole] ([RoleConstraintArgumentId]);
CREATE INDEX [RoleId]
	ON [MetaModelJoinPathRole] ([RoleId]);
CREATE INDEX [CreatedByUserId]
	ON [MetaModelModel] ([CreatedByUserId]);
CREATE INDEX [NamespaceId]
	ON [MetaModelModel] ([NamespaceId]);
CREATE INDEX [dlc_phase_id]
	ON [MetaModelModel] ([ProjectPhaseId]);
CREATE INDEX [enterprise_id]
	ON [MetaModelModel] ([EnterpriseId]);
CREATE INDEX [model_id]
	ON [MetaModelModel] ([ModelId]);
CREATE INDEX [project_id]
	ON [MetaModelModel] ([ProjectId]);
CREATE INDEX [solution_id]
	ON [MetaModelModel] ([SolutionId]);
CREATE INDEX [subject_area_id]
	ON [MetaModelModel] ([SubjectAreaId]);
CREATE INDEX [MetaModelModelDictionarySymbol]
	ON [MetaModelModelDictionary] ([Symbol]);
CREATE INDEX [StartDate]
	ON [MetaModelModelDictionary] ([StartDate]);
CREATE INDEX [model_id]
	ON [MetaModelModelDictionary] ([ModelId]);
CREATE INDEX [JoinedModelObjectId]
	ON [MetaModelModelNote] ([JoinedObjectTypeId]);
CREATE INDEX [fact_type_reading_id]
	ON [MetaModelPredicatePart] ([FactTypeReadingId]);
CREATE INDEX [model_id]
	ON [MetaModelPredicatePart] ([ModelId]);
CREATE INDEX [MetaModelRoleRoleId]
	ON [MetaModelRole] ([RoleId]);
CREATE INDEX [fact_type_id]
	ON [MetaModelRole] ([FactTypeId]);
CREATE INDEX [joins_domain_id]
	ON [MetaModelRole] ([JoinsValueTypeId]);
CREATE INDEX [joins_entity_id]
	ON [MetaModelRole] ([JoinsEntityTypeId]);
CREATE INDEX [joins_nested_fact_id]
	ON [MetaModelRole] ([JoinsNestedFactTypeId]);
CREATE INDEX [language_id]
	ON [MetaModelRole] ([ModelId]);
CREATE INDEX [GUID]
	ON [MetaModelRoleConstraint] ([GUID]);
CREATE INDEX [paeg_id]
	ON [MetaModelRoleConstraint] ([ModelId]);
CREATE INDEX [role_constraint_id]
	ON [MetaModelRoleConstraint] ([RoleConstraintId]);
CREATE INDEX [ModelId]
	ON [MetamodelRoleConstraintArgument] ([ModelId]);
CREATE INDEX [RoleConstraintId]
	ON [MetamodelRoleConstraintArgument] ([RoleConstraintId]);
CREATE INDEX [ArgumentId]
	ON [MetaModelRoleConstraintRole] ([ArgumentId]);
CREATE INDEX [project_id]
	ON [MetaModelRoleConstraintRole] ([ModelId]);
CREATE INDEX [role_constraint_id]
	ON [MetaModelRoleConstraintRole] ([RoleConstraintId]);
CREATE INDEX [role_id]
	ON [MetaModelRoleConstraintRole] ([RoleId]);
CREATE INDEX [SubtypingFactTypeId]
	ON [MetaModelSubtypeRelationship] ([SubtypingFactTypeId]);
CREATE INDEX [entity_type_id]
	ON [MetaModelSubtypeRelationship] ([ObjectTypeId]);
CREATE INDEX [model_id]
	ON [MetaModelSubtypeRelationship] ([ModelId]);
CREATE INDEX [parent_entity_type_id]
	ON [MetaModelSubtypeRelationship] ([SupertypeObjectTypeId]);
CREATE INDEX [GUID]
	ON [MetaModelValueType] ([GUID]);
CREATE INDEX [ModelId]
	ON [MetaModelValueType] ([ModelId]);
CREATE INDEX [entity_id]
	ON [MetaModelValueType] ([ValueTypeId]);
CREATE INDEX [ModelId]
	ON [MetaModelValueTypeValueConstraint] ([ModelId]);
CREATE INDEX [value_type_id]
	ON [MetaModelValueTypeValueConstraint] ([ValueTypeId]);
CREATE INDEX [RoleId]
	ON [ModelConceptInstance] ([RoleId]);
CREATE INDEX [StartDate]
	ON [ModelConceptInstance] ([StartDate]);
CREATE INDEX [model_id]
	ON [ModelConceptInstance] ([ModelId]);
CREATE INDEX [page_id]
	ON [ModelConceptInstance] ([PageId]);
CREATE INDEX [symbol_id]
	ON [ModelConceptInstance] ([Symbol]);
CREATE INDEX [language_id]
	ON [ModelLanguage] ([LanguageId]);
CREATE INDEX [language_id]
	ON [ModelPage] ([LanguageId]);
CREATE INDEX [model_id]
	ON [ModelPage] ([ModelId]);
CREATE INDEX [page_id]
	ON [ModelPage] ([PageId]);
CREATE INDEX [row_id]
	ON [ReferenceFieldValue] ([row_id]);
ALTER TABLE [ClientServerUserRole]
	ADD CONSTRAINT [ClientServerUserClientServerUserRole]
	FOREIGN KEY ([UserId]) REFERENCES
		[ClientServerUser] ([Id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelEntityType]
	ADD CONSTRAINT [MetaModelModelDictionaryMetaModelEntityType]
	FOREIGN KEY UNIQUE ([ModelId], [EntityTypeId]) REFERENCES
		[MetaModelModelDictionary] ([ModelId], [Symbol])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelEntityType]
	ADD CONSTRAINT [MetaModelRoleConstraintMetaModelEntityType]
	FOREIGN KEY NO INDEX ([ModelId], [PreferredIdentifierRCId]) REFERENCES
		[MetaModelRoleConstraint] ([ModelId], [RoleConstraintId]);
ALTER TABLE [MetaModelFact]
	ADD CONSTRAINT [MetaModelFactTypeMetaModelFact]
	FOREIGN KEY ([ModelId], [FactTypeId]) REFERENCES
		[MetaModelFactType] ([ModelId], [FactTypeId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelFactData]
	ADD CONSTRAINT [MetaModelFactMetaModelFactData]
	FOREIGN KEY ([ModelId], [FactSymbol]) REFERENCES
		[MetaModelFact] ([ModelId], [Symbol])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelFactData]
	ADD CONSTRAINT [MetaModelRoleMetaModelFactData]
	FOREIGN KEY NO INDEX ([ModelId], [RoleId]) REFERENCES
		[MetaModelRole] ([ModelId], [RoleId]);
ALTER TABLE [MetaModelFactType]
	ADD CONSTRAINT [MetaModelModelDictionaryMetaModelFactType]
	FOREIGN KEY UNIQUE ([ModelId], [FactTypeId]) REFERENCES
		[MetaModelModelDictionary] ([ModelId], [Symbol])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelFactTypeReading]
	ADD CONSTRAINT [MetaModelFactTypeMetaModelFactTypeReading]
	FOREIGN KEY ([ModelId], [FactTypeId]) REFERENCES
		[MetaModelFactType] ([ModelId], [FactTypeId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelJoinPathRole]
	ADD CONSTRAINT [MetamodelRoleConstraintArgumeMetaModelJoinPathRole]
	FOREIGN KEY ([ModelId], [RoleConstraintArgumentId]) REFERENCES
		[MetamodelRoleConstraintArgument] ([ModelId], [Id])
	ON DELETE CASCADE;
ALTER TABLE [MetaModelModelDictionary]
	ADD CONSTRAINT [MetaModelConceptMetaModelModelDictionary]
	FOREIGN KEY ([Symbol]) REFERENCES
		[MetaModelConcept] ([Symbol]);
ALTER TABLE [MetaModelModelDictionary]
	ADD CONSTRAINT [MetaModelFactMetaModelModelDictionary]
	FOREIGN KEY NO INDEX ([Symbol]) REFERENCES
		[MetaModelFact] ([Symbol]);
ALTER TABLE [MetaModelModelDictionary]
	ADD CONSTRAINT [MetaModelModelMetaModelModelDictionary]
	FOREIGN KEY ([ModelId]) REFERENCES
		[MetaModelModel] ([ModelId])
	ON DELETE CASCADE;
ALTER TABLE [MetaModelModelDictionary]
	ADD CONSTRAINT [fkConcept]
	FOREIGN KEY ([Symbol]) REFERENCES
		[MetaModelConcept] ([Symbol]);
ALTER TABLE [MetaModelPredicatePart]
	ADD CONSTRAINT [MetaModelFactTypeReadingMetaModelPredicatePart]
	FOREIGN KEY ([ModelId], [FactTypeReadingId]) REFERENCES
		[MetaModelFactTypeReading] ([ModelId], [FactTypeReadingId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelRole]
	ADD CONSTRAINT [MetaModelEntityTypeMetaModelRole]
	FOREIGN KEY NO INDEX ([ModelId], [JoinsEntityTypeId]) REFERENCES
		[MetaModelEntityType] ([ModelId], [EntityTypeId]);
ALTER TABLE [MetaModelRole]
	ADD CONSTRAINT [MetaModelFactTypeMetaModelRole]
	FOREIGN KEY ([ModelId], [FactTypeId]) REFERENCES
		[MetaModelFactType] ([ModelId], [FactTypeId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelRole]
	ADD CONSTRAINT [MetaModelFactTypeMetaModelRole1]
	FOREIGN KEY NO INDEX ([ModelId], [JoinsNestedFactTypeId]) REFERENCES
		[MetaModelFactType] ([ModelId], [FactTypeId]);
ALTER TABLE [MetaModelRole]
	ADD CONSTRAINT [MetaModelValueTypeMetaModelRole]
	FOREIGN KEY NO INDEX ([ModelId], [JoinsValueTypeId]) REFERENCES
		[MetaModelValueType] ([ModelId], [ValueTypeId]);
ALTER TABLE [MetaModelRoleConstraint]
	ADD CONSTRAINT [MetaModelModelDictionaryMetaModelRoleConstraint]
	FOREIGN KEY UNIQUE ([ModelId], [RoleConstraintId]) REFERENCES
		[MetaModelModelDictionary] ([ModelId], [Symbol])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetamodelRoleConstraintArgument]
	ADD CONSTRAINT [MetaModelRoleConstraintMetamodelRoleConstraintArgument]
	FOREIGN KEY ([ModelId], [RoleConstraintId]) REFERENCES
		[MetaModelRoleConstraint] ([ModelId], [RoleConstraintId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelRoleConstraintRole]
	ADD CONSTRAINT [MetaModelRoleConstraintMetaModelRoleConstraintRole]
	FOREIGN KEY ([ModelId], [RoleConstraintId]) REFERENCES
		[MetaModelRoleConstraint] ([ModelId], [RoleConstraintId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelRoleConstraintRole]
	ADD CONSTRAINT [MetaModelRoleMetaModelRoleConstraintRole]
	FOREIGN KEY ([ModelId], [RoleId]) REFERENCES
		[MetaModelRole] ([ModelId], [RoleId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelSubtypeRelationship]
	ADD CONSTRAINT [MetaModelModelMetaModelParentEntityType]
	FOREIGN KEY ([ModelId]) REFERENCES
		[MetaModelModel] ([ModelId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelValueType]
	ADD CONSTRAINT [MetaModelModelDictionaryMetaModelValueType]
	FOREIGN KEY UNIQUE ([ModelId], [ValueTypeId]) REFERENCES
		[MetaModelModelDictionary] ([ModelId], [Symbol])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [MetaModelValueTypeValueConstraint]
	ADD CONSTRAINT [MetaModelValueTypeMetaModelValueTypeValueConstraint]
	FOREIGN KEY ([ModelId], [ValueTypeId]) REFERENCES
		[MetaModelValueType] ([ModelId], [ValueTypeId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [ModelConceptInstance]
	ADD CONSTRAINT [MetaModelConceptModelConceptInstance]
	FOREIGN KEY ([Symbol]) REFERENCES
		[MetaModelConcept] ([Symbol]);
ALTER TABLE [ModelConceptInstance]
	ADD CONSTRAINT [MetaModelModelDictionaryModelConceptInstance]
	FOREIGN KEY NO INDEX ([ModelId], [Symbol]) REFERENCES
		[MetaModelModelDictionary] ([ModelId], [Symbol]);
ALTER TABLE [ModelConceptInstance]
	ADD CONSTRAINT [MetaModelModelModelConceptInstance]
	FOREIGN KEY NO INDEX ([ModelId]) REFERENCES
		[MetaModelModel] ([ModelId]);
ALTER TABLE [ModelConceptInstance]
	ADD CONSTRAINT [ModelPageModelConceptInstance]
	FOREIGN KEY ([PageId]) REFERENCES
		[ModelPage] ([PageId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;
ALTER TABLE [ModelPage]
	ADD CONSTRAINT [MetaModelModelModelPage]
	FOREIGN KEY ([ModelId]) REFERENCES
		[MetaModelModel] ([ModelId])
	ON DELETE CASCADE;
CREATE VIEW [ConceptInstancesNotInConcept] AS SELECT *
FROM ModelConceptInstance
WHERE Symbol NOT IN (SELECT Symbol FROM MetaModelConcept);

CREATE VIEW [ConceptInstancesNotInModelDictionary] AS SELECT *
FROM ModelConceptInstance AS CI
WHERE CI.Symbol NOT IN (SELECT MD.Symbol FROM MetaModelModelDictionary AS MD
                                           WHERE MD.ModelId = CI.ModelId);

CREATE VIEW [DictionaryEntryNotInConceptInstance] AS SELECT *
FROM MetaModelModelDictionary AS MD
WHERE MD.[Symbol] NOT IN (SELECT CI.[Symbol] FROM ModelConceptInstance AS CI
                                           WHERE  CI.ModelId = MD.ModelId);

CREATE VIEW [EntityTypesNotInModelDictionary] AS SELECT *
FROM MetaModelEntityType
WHERE EntityTypeId NOT IN (SELECT Symbol FROM MetaModelModelDictionary);

CREATE VIEW [FactDataWithoutCorrespondingFactType] AS SELECT *
FROM MetaModelFactData AS FD
WHERE FD.FactTypeId NOT IN (SELECT FactTypeId FROM MetaModelFactType);

CREATE VIEW [FactDataWithoutSymbolInModelDictionaryDictionaryEntry] AS SELECT *
FROM MetaModelFactData AS FD
WHERE FD.FactSymbol NOT IN (SELECT Symbol FROM MetaModelModelDictionary);

CREATE VIEW [FactDataWithoutValueInModelDictionaryDictionaryEntry] AS SELECT *
FROM MetaModelFactData AS FD
WHERE FD.ValueSymbol NOT IN (SELECT Symbol FROM MetaModelModelDictionary);

CREATE VIEW [FactsNotInModelDictionary] AS SELECT *
FROM MetaModelFact
WHERE Symbol NOT IN (SELECT Symbol FROM MetaModelModelDictionary);

CREATE VIEW [FactsWithNoReferencedFactType] AS SELECT *
FROM MetaModelFact
WHERE FactTypeId NOT IN (SELECT FactTypeId FROM MetaModelFactType);

CREATE VIEW [FactTypesNotInModelDictionary] AS SELECT *
FROM MetaModelFactType
WHERE FactTypeId NOT IN (SELECT Symbol FROM MetaModelModelDictionary);

CREATE VIEW [FactWithoutCorrespondingFactType] AS SELECT *
FROM MetaModelFact AS F
WHERE F.FactTypeId NOT IN (SELECT FactTypeId FROM MetaModelFactType);

CREATE VIEW [MetaModelRoleConstraintRole Query] AS SELECT MetaModelRoleConstraintRole.RoleConstraintId, MetaModelRoleConstraintRole.RoleId, MetaModelRole.FactTypeId
FROM MetaModelRole INNER JOIN MetaModelRoleConstraintRole ON (MetaModelRole.ModelId=MetaModelRoleConstraintRole.ModelId) AND (MetaModelRole.RoleId=MetaModelRoleConstraintRole.RoleId);

CREATE VIEW [ModelDictionaryNotInRoleConstraint] AS SELECT *
FROM MetaModelModelDictionary AS MD
WHERE IsRoleConstraint = True
   AND Symbol Not In (SELECT RC.[RoleConstraintId]
                                      FROM MetaModelRoleConstraint RC
                                     WHERE RC.ModelId = MD.ModelId);

CREATE VIEW [PredicatePartsNotInFactTypeReading] AS SELECT *
FROM MetaModelPredicatePart
WHERE FactTypeReadingId NOT IN (SELECT FactTypeReadingId FROM MetaModelFactTypeReading);

CREATE VIEW [RoleConstraintRolesWithNoRoleConstraint] AS SELECT *
FROM MetaModelRoleConstraintRole
WHERE MetaModelRoleConstraintRole.[RoleConstraintId] Not In (SELECT RoleConstraintId FROM MetaModelRoleConstraint);

CREATE VIEW [RoleConstraintsModelNotInModelDictionary] AS SELECT *
FROM MetaModelRoleConstraint
WHERE (((MetaModelRoleConstraint.ModelId) Not In (SELECT ModelId FROM MetaModelModelDictionary)));

CREATE VIEW [RoleConstraintsNotInModelDictionary] AS SELECT *
FROM MetaModelRoleConstraint
WHERE (((MetaModelRoleConstraint.[RoleConstraintId]) Not In (SELECT Symbol FROM MetaModelModelDictionary)));

CREATE VIEW [RoleConstraintsWithNoRoleConstraintRoles] AS SELECT *
FROM MetaModelRoleConstraint AS RC
WHERE RC.[RoleConstraintId] Not In (SELECT RoleConstraintId 
                                                                 FROM MetaModelRoleConstraintRole RCR
                                                                WHERE RCR.ModelId = RC.ModelId);

CREATE VIEW [RolesWhereJoinsNestedFactTypeIdNotInMetaModelFactType] AS SELECT *
FROM MetaModelRole
WHERE JoinsNestedFactTypeId <> ''
 AND JoinsNestedFactTypeId NOT IN (SELECT FactTypeId FROM MetaModelFactType);

CREATE VIEW [ValueTypesNotInModelDictionary] AS SELECT *
FROM MetaModelValueType AS VT
WHERE (((VT.[ValueTypeId]) Not In (SELECT Symbol FROM MetaModelModelDictionary MD WHERE MD.ModelId = VT.ModelId)));

CREATE VIEW [AppendModelDictionaryValueType] AS INSERT INTO MetaModelModelDictionary ( model_id, symbol, is_fact_type, FactTypeId )
SELECT [MetaModelFactType].model_id AS Expr1, [MetaModelFactType].[FactTypeId], True AS Expr2, [MetaModelFactType].[FactTypeId]
FROM MetaModelFactType;

CREATE VIEW [CREATE-ModelDictionary] AS INSERT INTO MetaModelModelDictionary ( StartDate, EndDate, ModelId, Symbol, ShortDescription, LongDescription, IsEntityType, IsValueType, IsFactType, IsFact, IsValue, IsRoleConstraint )
SELECT Null AS Expr1, Null AS Expr2, '0843d4ea-eaf2-4d4e-84fa-92ae5a37c68f' AS Expr3, '1b11874b-0531-42d1-8a38-5bbb4ffde657' AS Expr4, '' AS Expr5, '' AS Expr6, False AS Expr7, False AS Expr8, False AS Expr9, True AS Expr10, False AS Expr11, False AS Expr12;

CREATE VIEW [DDLForeignKey-kfConcept-ON-MetaModelModelDictionary] AS ALTER TABLE MetaModelModelDictionary ADD CONSTRAINT fkConcept FOREIGN KEY (Symbol) REFERENCES MetaModelConcept (Symbol);
CREATE VIEW [DeleteConceptInstanceByModelId] AS DELETE *
FROM ModelConceptInstance
WHERE ModelId='_37B11C61-6E47-4D68-B383-71B31696EEAA';

CREATE VIEW [Find duplicates for EnterpriseGoalOutcome] AS SELECT First(EnterpriseGoalOutcome.[GoalId]) AS [GoalId Field], First(EnterpriseGoalOutcome.[OutcomesId]) AS [OutcomesId Field], Count(EnterpriseGoalOutcome.[GoalId]) AS NumberOfDups
FROM EnterpriseGoalOutcome
GROUP BY EnterpriseGoalOutcome.[GoalId], EnterpriseGoalOutcome.[OutcomesId]
HAVING (((Count(EnterpriseGoalOutcome.[GoalId]))>1) AND ((Count(EnterpriseGoalOutcome.[OutcomesId]))>1));

CREATE VIEW [InsertIntoConceptWhereConceptInstanceNotInConcept] AS INSERT INTO MetaModelConcept
SELECT Symbol AS Symbol
FROM ModelConceptInstance
WHERE Symbol NOT IN (SELECT Symbol FROM MetaModelConcept);

CREATE VIEW [InsertIntoConceptWhereModelDictionaryNotInConcept] AS INSERT INTO MetaModelConcept
SELECT Symbol AS Symbol
FROM MetaModelModelDictionary
WHERE Symbol NOT IN (SELECT Symbol FROM MetaModelConcept);

CREATE VIEW [Query1] AS INSERT INTO EnterpriseOrganisation ( OrganisationId, OrganisationName, OrganisationAcronym, ParentOrginisationId, WorkTelephone, FaxNumber )
SELECT KM_tblOrganisation.OrganisaztionPK, KM_tblOrganisation.OrganisationName, KM_tblOrganisation.OrganisationAcronym, KM_tblOrganisation.ParentOrganisationPK, KM_tblOrganisation.WorkTelephone, KM_tblOrganisation.FaxTelephoneNo, *
FROM KM_tblOrganisation;

CREATE VIEW [Query2] AS INSERT INTO EnterpriseOutcome ( OutcomeId, ShortDescription, LongDescription, OrginisationId, PersonId, OutcomeCode )
SELECT KM_TblOutcomes.KM_OutcomePK, KM_TblOutcomes.KM_Outcome_description, KM_TblOutcomes.KM_Extended_Description, KM_TblOutcomes.KM_Organisation_PK, KM_TblOutcomes.KM_tblPersonFK, KM_TblOutcomes.KM_Outcome_ID
FROM KM_TblOutcomes;

CREATE VIEW [UpdateConceptInstanceNotUsed] AS UPDATE ModelConceptInstance SET ModelConceptInstance.RoleId = 'NotUsed'
WHERE (((1)=1));

CREATE VIEW [Update-FactTypeId] AS UPDATE MetaModelFactType SET FactTypeId = 'IsSelfAware'
WHERE FactTypeId='NewEntityType1# 1';

