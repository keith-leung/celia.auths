-- ----------------------------
-- Table structure for __migrationversions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[__migrationversions]') AND type IN ('U'))
	DROP TABLE [dbo].[__migrationversions]
GO

CREATE TABLE [dbo].[__migrationversions] (
  [VersionCode] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Version] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ctime] datetime2(7)  NULL
)
GO


-- ----------------------------
-- Table structure for auths_role_claims
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[auths_role_claims]') AND type IN ('U'))
	DROP TABLE [dbo].[auths_role_claims]
GO

CREATE TABLE [dbo].[auths_role_claims] (
  [Id] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [RoleId] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ClaimType] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ClaimValue] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO


-- ----------------------------
-- Table structure for auths_roles
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[auths_roles]') AND type IN ('U'))
	DROP TABLE [dbo].[auths_roles]
GO

CREATE TABLE [dbo].[auths_roles] (
  [Id] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Name] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [NormalizedName] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ConcurrentStamp] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO


-- ----------------------------
-- Table structure for auths_service_apps
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[auths_service_apps]') AND type IN ('U'))
	DROP TABLE [dbo].[auths_service_apps]
GO

CREATE TABLE [dbo].[auths_service_apps] (
  [appid] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [appsecret] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [lockoutenabled] tinyint  NULL,
  [accessfailedcount] int  NULL,
  [lockoutendtimestamp] bigint  NULL
)
GO


-- ----------------------------
-- Table structure for auths_user_claims
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[auths_user_claims]') AND type IN ('U'))
	DROP TABLE [dbo].[auths_user_claims]
GO

CREATE TABLE [dbo].[auths_user_claims] (
  [Id] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [UserId] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ClaimType] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ClaimValue] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO


-- ----------------------------
-- Table structure for auths_user_logins
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[auths_user_logins]') AND type IN ('U'))
	DROP TABLE [dbo].[auths_user_logins]
GO

CREATE TABLE [dbo].[auths_user_logins] (
  [Id] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [LoginProvider] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ProviderKey] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ProviderDisplayName] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [UserId] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO


-- ----------------------------
-- Table structure for auths_user_roles
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[auths_user_roles]') AND type IN ('U'))
	DROP TABLE [dbo].[auths_user_roles]
GO

CREATE TABLE [dbo].[auths_user_roles] (
  [Id] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [UserId] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [RoleId] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO


-- ----------------------------
-- Table structure for auths_user_tokens
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[auths_user_tokens]') AND type IN ('U'))
	DROP TABLE [dbo].[auths_user_tokens]
GO

CREATE TABLE [dbo].[auths_user_tokens] (
  [Id] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [UserId] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [LoginProvider] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Name] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Value] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO


-- ----------------------------
-- Table structure for auths_users
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[auths_users]') AND type IN ('U'))
	DROP TABLE [dbo].[auths_users]
GO

CREATE TABLE [dbo].[auths_users] (
  [Id] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [UserName] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [NormalizedUserName] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Email] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [NormalizedEmail] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [EmailConfirmed] bit  NOT NULL,
  [PasswordHash] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [SecurityStamp] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ConcurrencyStamp] nvarchar(1024) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [PhoneNumber] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [PhoneNumberConfirmed] bit  NOT NULL,
  [TwoFactorEnabled] bit  NOT NULL,
  [LockoutEnd] datetime2(7)  NULL,
  [LockoutEnabled] bit  NOT NULL,
  [AccessFailedCount] int DEFAULT ((0)) NOT NULL
)
GO


-- ----------------------------
-- Primary Key structure for table __migrationversions
-- ----------------------------
ALTER TABLE [dbo].[__migrationversions] ADD CONSTRAINT [PK____migrat__8C291688CAA15BED] PRIMARY KEY CLUSTERED ([VersionCode])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table auths_role_claims
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId]
ON [dbo].[auths_role_claims] (
  [RoleId] ASC
)
GO


-- ----------------------------
-- Indexes structure for table auths_roles
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AspNetRoles_RoleName]
ON [dbo].[auths_roles] (
  [NormalizedName] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table auths_roles
-- ----------------------------
ALTER TABLE [dbo].[auths_roles] ADD CONSTRAINT [PK__auths_ro__3214EC07A7B58C21] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Primary Key structure for table auths_service_apps
-- ----------------------------
ALTER TABLE [dbo].[auths_service_apps] ADD CONSTRAINT [PK__auths_se__C00F024DB7E64591] PRIMARY KEY CLUSTERED ([appid])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table auths_user_claims
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaimss_UserId]
ON [dbo].[auths_user_claims] (
  [UserId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table auths_user_claims
-- ----------------------------
ALTER TABLE [dbo].[auths_user_claims] ADD CONSTRAINT [PK__auths_us__3214EC0730562163] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table auths_user_logins
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
ON [dbo].[auths_user_logins] (
  [UserId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_ProviderUserId]
ON [dbo].[auths_user_logins] (
  [UserId] ASC,
  [LoginProvider] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_ProviderKey]
ON [dbo].[auths_user_logins] (
  [LoginProvider] ASC,
  [ProviderKey] ASC
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AspNetUserLogins_LoginProviderKey]
ON [dbo].[auths_user_logins] (
  [ProviderKey] ASC,
  [LoginProvider] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table auths_user_logins
-- ----------------------------
ALTER TABLE [dbo].[auths_user_logins] ADD CONSTRAINT [PK__auths_us__3214EC073ED0EAAF] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table auths_user_roles
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
ON [dbo].[auths_user_roles] (
  [RoleId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_UserIdRoleId]
ON [dbo].[auths_user_roles] (
  [UserId] ASC,
  [RoleId] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table auths_user_roles
-- ----------------------------
ALTER TABLE [dbo].[auths_user_roles] ADD CONSTRAINT [PK__auths_us__3214EC07B629A25C] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table auths_user_tokens
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AspNetUserTokens]
ON [dbo].[auths_user_tokens] (
  [UserId] ASC,
  [LoginProvider] ASC,
  [Name] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table auths_user_tokens
-- ----------------------------
ALTER TABLE [dbo].[auths_user_tokens] ADD CONSTRAINT [PK__auths_us__3214EC07286349FE] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table auths_users
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AspNetUser_Email]
ON [dbo].[auths_users] (
  [NormalizedEmail] ASC
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AspNetUser_UserName]
ON [dbo].[auths_users] (
  [NormalizedUserName] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table auths_users
-- ----------------------------
ALTER TABLE [dbo].[auths_users] ADD CONSTRAINT [PK__auths_us__3214EC072F95DE0C] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


INSERT INTO __migrationversions(VersionCode, [Version]) VALUES ('20190129001', 'V1.0.0');