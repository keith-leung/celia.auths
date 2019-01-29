-- ----------------------------
-- Table structure for auths_role_claims
-- ----------------------------
CREATE TABLE auths_role_claims (
  [Id] nvarchar(255) NOT NULL,
  [RoleId] nvarchar(255) NULL,
  [ClaimType] nvarchar(1024) NULL,
  [ClaimValue] nvarchar(1024) NULL
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId]
ON auths_role_claims (
  [RoleId]
)
GO


-- ----------------------------
-- Table structure for auths_roles
-- ----------------------------
CREATE TABLE auths_roles (
  [Id] nvarchar(255) NOT NULL,
  [Name] nvarchar(255) NULL,
  [NormalizedName] nvarchar(255) NOT NULL,
  [ConcurrentStamp] nvarchar(1024) NULL,
  PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetRoles_RoleName]
ON auths_roles (
  [NormalizedName]
)
GO
 

-- ----------------------------
-- Table structure for auths_user_claims
-- ----------------------------
CREATE TABLE auths_user_claims (
  [Id] nvarchar(255) NOT NULL,
  [UserId] nvarchar(255) NOT NULL,
  [ClaimType] nvarchar(1024) NULL,
  [ClaimValue] nvarchar(1024) NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserClaimss_UserId]
ON auths_user_claims (
  [UserId]
)
GO
 

-- ----------------------------
-- Table structure for auths_user_logins
-- ----------------------------
CREATE TABLE auths_user_logins (
  [Id] nvarchar(255) NOT NULL,
  [LoginProvider] nvarchar(255) NOT NULL,
  [ProviderKey] nvarchar(1024) NOT NULL,
  [ProviderDisplayName] nvarchar(1024) NULL,
  [UserId] nvarchar(255) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id]) 
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
ON auths_user_logins (
  [UserId]
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_ProviderUserId]
ON auths_user_logins (
  [UserId],
  [LoginProvider]
)
GO

CREATE UNIQUE INDEX IX_AspNetUserLogins_LoginProviderKey
    on auths_user_logins (ProviderKey, LoginProvider)
GO
 

-- ----------------------------
-- Table structure for auths_user_roles
-- ----------------------------
CREATE TABLE auths_user_roles (
  [Id] nvarchar(255) NOT NULL,
  [UserId] nvarchar(255) NOT NULL,
  [RoleId] nvarchar(255) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
ON auths_user_roles (
  [RoleId]
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_UserIdRoleId]
ON auths_user_roles (
  [UserId],
  [RoleId]
)
GO

 
-- ----------------------------
-- Table structure for auths_user_tokens
-- ----------------------------
CREATE TABLE auths_user_tokens (
  [Id] nvarchar(255) NOT NULL,
  [UserId] nvarchar(255) NOT NULL,
  [LoginProvider] nvarchar(255) NOT NULL,
  [Name] nvarchar(255) NOT NULL,
  [Value] nvarchar(1024) NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserTokens]
ON auths_user_tokens (
  [UserId],
  [LoginProvider],
  [Name]
)
GO

 
-- ----------------------------
-- Table structure for auths_users
-- ----------------------------
CREATE TABLE auths_users (
  [Id] nvarchar(255) NOT NULL,
  [UserName] nvarchar(255) NULL,
  [NormalizedUserName] nvarchar(255) NOT NULL,
  [Email] nvarchar(255) NULL,
  [NormalizedEmail] nvarchar(255) NULL,
  [EmailConfirmed] tinyint DEFAULT 0 NOT NULL,
  [PasswordHash] nvarchar(1024) NULL,
  [SecurityStamp] nvarchar(1024) NULL,
  [ConcurrencyStamp] nvarchar(1024) NULL,
  [PhoneNumber] nvarchar(50) NULL,
  [PhoneNumberConfirmed] tinyint DEFAULT 0 NOT NULL,
  [TwoFactorEnabled] tinyint DEFAULT 0 NOT NULL,
  [LockoutEnd] datetime2 NULL,
  [LockoutEnabled] tinyint DEFAULT 0 NOT NULL,
  [AccessFailedCount] tinyint DEFAULT 0 NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUser_Email]
ON auths_users (
  [NormalizedEmail]
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AspNetUser_UserName]
ON auths_users (
  [NormalizedUserName]
)
GO

 
-- ----------------------------
-- Table structure for auths_service_apps
-- ----------------------------
create table auths_service_apps
(
 appid nvarchar(100) not null,
 appsecret nvarchar(100) null,
 lockoutenabled tinyint null,
 accessfailedcount int null,
 lockoutendtimestamp bigint null,
	PRIMARY KEY CLUSTERED (appid)
)
GO
 

-- ----------------------------
-- Table structure for __migrationversions
-- ----------------------------
CREATE TABLE __migrationversions (
  [VersionCode] varchar(255) NOT NULL,
  [Version] varchar(255) NOT NULL,
  [ctime] datetime2(7)  NULL,
  PRIMARY KEY CLUSTERED ([VersionCode])
)
GO 


INSERT INTO __migrationversions(VersionCode, [Version]) VALUES ('20190129001', 'V1.0.0');