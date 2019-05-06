SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for __migrationversions
-- ----------------------------
DROP TABLE IF EXISTS `__migrationversions`;
CREATE TABLE `__migrationversions`  (
  `VersionCode` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键，人工设置一个值，例如：20180809001，代表2018年8月9日第一次更新',
  `Version` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '代码版本，表示这个更新后的数据库版本是跟哪个代码版本的',
  `ctime` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '执行时间戳',
  PRIMARY KEY (`VersionCode`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auths_role_claims
-- ----------------------------
DROP TABLE IF EXISTS `auths_role_claims`;
CREATE TABLE `auths_role_claims`  (
  `Id` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `RoleId` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
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
  `Id` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `NormalizedName` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ConcurrencyStamp` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `RoleNameIndex`(`NormalizedName`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auths_service_apps
-- ----------------------------
DROP TABLE IF EXISTS `auths_service_apps`;
CREATE TABLE `auths_service_apps`  (
  `appid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `appsecret` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `lockoutenabled` tinyint(4) NULL DEFAULT NULL,
  `accessfailedcount` int(11) NULL DEFAULT NULL,
  `lockoutendtimestamp` bigint(20) NULL DEFAULT NULL,
  PRIMARY KEY (`appid`) USING BTREE,
  UNIQUE INDEX `auths_service_apps_appid_uindex`(`appid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auths_user_claims
-- ----------------------------
DROP TABLE IF EXISTS `auths_user_claims`;
CREATE TABLE `auths_user_claims`  (
  `Id` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserId` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ClaimType` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ClaimValue` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AspNetUserClaims_UserId`(`UserId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auths_user_logins
-- ----------------------------
DROP TABLE IF EXISTS `auths_user_logins`;
CREATE TABLE `auths_user_logins`  (
  `Id` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `LoginProvider` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProviderKey` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProviderDisplayName` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UserId` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AspNetUserLogins_LoginProviderKey`(`ProviderKey`, `LoginProvider`) USING BTREE,
  INDEX `IX_AspNetUserLogins_ProviderUserId`(`LoginProvider`, `UserId`) USING BTREE,
  INDEX `IX_AspNetUserLogins_UserId`(`UserId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auths_user_roles
-- ----------------------------
DROP TABLE IF EXISTS `auths_user_roles`;
CREATE TABLE `auths_user_roles`  (
  `Id` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `UserId` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `RoleId` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AspNetUserRoles_RoleId`(`RoleId`) USING BTREE,
  INDEX `IX_AspNetUserRoles_UserIdRoleId`(`UserId`, `RoleId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auths_user_tokens
-- ----------------------------
DROP TABLE IF EXISTS `auths_user_tokens`;
CREATE TABLE `auths_user_tokens`  (
  `Id` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `UserId` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `LoginProvider` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AspNetUserTokens`(`UserId`, `LoginProvider`, `Name`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for auths_users
-- ----------------------------
DROP TABLE IF EXISTS `auths_users`;
CREATE TABLE `auths_users`  (
  `Id` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `NormalizedUserName` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Email` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `NormalizedEmail` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
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

SET FOREIGN_KEY_CHECKS = 1;

INSERT INTO `__migrationversions`(`VersionCode`, `Version`, `ctime`) VALUES ('20181209001', 'V1.1.0', '2019-02-20 16:08:07');
