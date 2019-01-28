CREATE TABLE __migrationversions (
  [VersionCode] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Version] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ctime] datetime2(7)  NULL,
  CONSTRAINT [PK____migrat__8C291688BC1BA6BA] PRIMARY KEY CLUSTERED ([VersionCode])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO 
 
-- ----------------------------
-- Table structure for auths_role_claims
-- ----------------------------
DROP TABLE IF EXISTS `auths_role_claims`;
CREATE TABLE `auths_role_claims`  (
  `Id` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `RoleId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ClaimType` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ClaimValue` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AspNetRoleClaims_RoleId`(`RoleId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;
 
-- ----------------------------
-- Table structure for auths_roles
-- ----------------------------
DROP TABLE IF EXISTS `auths_roles`;
CREATE TABLE `auths_roles`  (
  `Id` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ConcurrencyStamp` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `RoleNameIndex`(`NormalizedName`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;
 
DROP TABLE IF EXISTS `auths_user_claims`;
CREATE TABLE `auths_user_claims`  (
  `Id` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ClaimType` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ClaimValue` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AspNetUserClaims_UserId`(`UserId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;
 
DROP TABLE IF EXISTS `auths_user_logins`;
CREATE TABLE `auths_user_logins`  (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `LoginProvider` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProviderKey` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProviderDisplayName` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AspNetUserLogins_UserId`(`UserId`) USING BTREE,
  INDEX `IX_AspNetUserLogins_ProviderUserId`(`LoginProvider`, `UserId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;
 
-- ----------------------------
-- Table structure for auths_user_roles
-- ----------------------------
DROP TABLE IF EXISTS `auths_user_roles`;
CREATE TABLE `auths_user_roles`  (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `RoleId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AspNetUserRoles_RoleId`(`RoleId`) USING BTREE,
  INDEX `IX_AspNetUserRoles_UserIdRoleId`(`UserId`, `RoleId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;
 
-- ----------------------------
-- Table structure for auths_user_tokens
-- ----------------------------
DROP TABLE IF EXISTS `auths_user_tokens`;
CREATE TABLE `auths_user_tokens`  (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `LoginProvider` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AspNetUserTokens`(`UserId`, `LoginProvider`, `Name`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;
 
-- ----------------------------
-- Table structure for auths_users
-- ----------------------------
DROP TABLE IF EXISTS `auths_users`;
CREATE TABLE `auths_users`  (
  `Id` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Email` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `EmailConfirmed` tinyint(4) NOT NULL,
  `PasswordHash` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `SecurityStamp` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ConcurrencyStamp` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PhoneNumber` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(4) NOT NULL DEFAULT 0,
  `TwoFactorEnabled` tinyint(4) NOT NULL,
  `LockoutEnd` datetime(0) NULL DEFAULT NULL,
  `LockoutEnabled` tinyint(4) NOT NULL DEFAULT 0,
  `AccessFailedCount` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `UserNameIndex`(`NormalizedUserName`) USING BTREE,
  INDEX `EmailIndex`(`NormalizedEmail`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;
 
INSERT INTO `__migrationversions`(`VersionCode`, `Version`) VALUES ('20180809001', 'V1.0.0');