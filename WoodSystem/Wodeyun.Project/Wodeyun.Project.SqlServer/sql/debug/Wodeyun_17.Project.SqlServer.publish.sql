/*
Wodeyun.Baise 的部署脚本

此代码由工具生成。
如果重新生成此代码，则对此文件的更改可能导致
不正确的行为并将丢失。
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "Wodeyun.Baise"
:setvar DefaultFilePrefix "Wodeyun.Baise"
:setvar DefaultDataPath "D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
请检测 SQLCMD 模式，如果不支持 SQLCMD 模式，请禁用脚本执行。
要在启用 SQLCMD 模式后重新启用脚本，请执行:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'要成功执行此脚本，必须启用 SQLCMD 模式。';
        SET NOEXEC ON;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
        
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Account'     AND XTYPE = 'U') DROP TABLE [Account]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Function'    AND XTYPE = 'U') DROP TABLE [Function]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Grant'       AND XTYPE = 'U') DROP TABLE [Grant]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'GsmArea'     AND XTYPE = 'U') DROP TABLE [GsmArea]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'GsmItem'     AND XTYPE = 'U') DROP TABLE [GsmItem]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'GsmLine'     AND XTYPE = 'U') DROP TABLE [GsmLine]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'GsmMessage'  AND XTYPE = 'U') DROP TABLE [GsmMessage]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'GsmOrigin'   AND XTYPE = 'U') DROP TABLE [GsmOrigin]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'GsmSupplier' AND XTYPE = 'U') DROP TABLE [GsmSupplier]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'GsmTree'     AND XTYPE = 'U') DROP TABLE [GsmTree]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Link'        AND XTYPE = 'U') DROP TABLE [Link]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Role'        AND XTYPE = 'U') DROP TABLE [Role]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Service'     AND XTYPE = 'U') DROP TABLE [Service]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Token'       AND XTYPE = 'U') DROP TABLE [Token]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Unique'      AND XTYPE = 'U') DROP TABLE [Unique]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Verificate'  AND XTYPE = 'U') DROP TABLE [Verificate]
GO

GO
PRINT N'已跳过具有键 6378befe-b190-44d7-9da2-b1037f76b0c2 的重命名重构操作，不会将元素 [dbo].[Token].[Account] (SqlSimpleColumn) 重命名为 Name';


GO
PRINT N'已跳过具有键 9574face-a278-4efe-b14f-1b23d908986f 的重命名重构操作，不会将元素 [dbo].[Token].[Value] (SqlSimpleColumn) 重命名为 Remark';


GO
PRINT N'已跳过具有键 5271e417-c24b-4320-871c-8ceca56d928f 的重命名重构操作，不会将元素 [dbo].[Function].[Role] (SqlSimpleColumn) 重命名为 Parent';


GO
PRINT N'已跳过具有键 17b7dd80-bcc2-47ea-a3d8-275c229636b9 的重命名重构操作，不会将元素 [dbo].[Function].[Id] (SqlSimpleColumn) 重命名为 Name';


GO
PRINT N'已跳过具有键 9a8aca10-5c5c-4bfa-8484-1072f59cccf9 的重命名重构操作，不会将元素 [dbo].[Account].[Id] (SqlSimpleColumn) 重命名为 Function';


GO
PRINT N'正在创建 [dbo].[Account]...';


GO
CREATE TABLE [dbo].[Account] (
    [Unique]      INT            NOT NULL,
    [Role]        INT            NOT NULL,
    [Id]          NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Remark]      NVARCHAR (MAX) NULL,
    [State]       INT            NOT NULL,
    [Version]     INT            NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[Function]...';


GO
CREATE TABLE [dbo].[Function] (
    [Unique]  INT            NOT NULL,
    [Parent]  INT            NULL,
    [Type]    INT            NOT NULL,
    [Order]   INT            NOT NULL,
    [No]      NVARCHAR (50)  NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Url]     NVARCHAR (255) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_Function] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[Grant]...';


GO
CREATE TABLE [dbo].[Grant] (
    [Unique]   INT NOT NULL,
    [Role]     INT NOT NULL,
    [Function] INT NOT NULL,
    [State]    INT NOT NULL,
    [Version]  INT NOT NULL,
    CONSTRAINT [PK_Grant] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmArea]...';


GO
CREATE TABLE [dbo].[GsmArea] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_GsmArea] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmItem]...';


GO
CREATE TABLE [dbo].[GsmItem] (
    [Unique]   INT            NOT NULL,
    [Message]  INT            NOT NULL,
    [Supplier] NVARCHAR (50)  NOT NULL,
    [Tree]     NVARCHAR (50)  NOT NULL,
    [Make]     DATETIME       NOT NULL,
    [Area]     NVARCHAR (50)  NOT NULL,
    [Origin]   NVARCHAR (50)  NOT NULL,
    [License]  NVARCHAR (50)  NOT NULL,
    [Driver]   NVARCHAR (50)  NOT NULL,
    [Ship]     DATETIME       NOT NULL,
    [Line]     NVARCHAR (50)  NOT NULL,
    [Remark]   NVARCHAR (MAX) NULL,
    [State]    INT            NOT NULL,
    [Version]  INT            NOT NULL,
    CONSTRAINT [PK_GsmItem] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmLine]...';


GO
CREATE TABLE [dbo].[GsmLine] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Alias]   NVARCHAR (255) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_GsmLine] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmMessage]...';


GO
CREATE TABLE [dbo].[GsmMessage] (
    [Unique]  INT            NOT NULL,
    [Parent]  INT            NULL,
    [Mobile]  NVARCHAR (50)  NOT NULL,
    [Date]    DATETIME       NOT NULL,
    [Text]    NVARCHAR (MAX) NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_GsmMessage] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmOrigin]...';


GO
CREATE TABLE [dbo].[GsmOrigin] (
    [Unique]  INT            NOT NULL,
    [Area]    INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Alias]   NVARCHAR (255) NOT NULL,
    [Except]  NVARCHAR (255) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_GsmOrigin] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmSupplier]...';


GO
CREATE TABLE [dbo].[GsmSupplier] (
    [Unique]  INT            NOT NULL,
    [Type]    INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_GsmSupplier] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmTree]...';


GO
CREATE TABLE [dbo].[GsmTree] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Alias]   NVARCHAR (255) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_GsmTree] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[Link]...';


GO
CREATE TABLE [dbo].[Link] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Url]     NVARCHAR (100) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_Link] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[Role]...';


GO
CREATE TABLE [dbo].[Role] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[Service]...';


GO
CREATE TABLE [dbo].[Service] (
    [Unique]   INT            NOT NULL,
    [Name]     NVARCHAR (50)  NOT NULL,
    [Filename] NVARCHAR (255) NOT NULL,
    [Url]      NVARCHAR (255) NOT NULL,
    [Contract] NVARCHAR (100) NOT NULL,
    [Remark]   NVARCHAR (MAX) NULL,
    [State]    INT            NOT NULL,
    [Version]  INT            NOT NULL,
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
PRINT N'正在创建 [dbo].[Token]...';


GO
CREATE TABLE [dbo].[Token] (
    [Unique]  INT           NOT NULL,
    [Account] INT           NOT NULL,
    [Value]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED ([Unique] ASC)
);


GO
PRINT N'正在创建 [dbo].[Token].[IX_Token]...';


GO
CREATE NONCLUSTERED INDEX [IX_Token]
    ON [dbo].[Token]([Value] ASC);


GO
PRINT N'正在创建 [dbo].[Unique]...';


GO
CREATE TABLE [dbo].[Unique] (
    [Name]  NVARCHAR (50) NOT NULL,
    [Value] INT           NOT NULL,
    CONSTRAINT [PK_Unique] PRIMARY KEY CLUSTERED ([Name] ASC)
);


GO
PRINT N'正在创建 [dbo].[Verificate]...';


GO
CREATE TABLE [dbo].[Verificate] (
    [Unique]  INT            NOT NULL,
    [Account] INT            NOT NULL,
    [Link]    INT            NOT NULL,
    [Value]   NVARCHAR (50)  NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [PK_Verificate] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


GO
-- 正在重构步骤以使用已部署的事务日志更新目标服务器

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6378befe-b190-44d7-9da2-b1037f76b0c2')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6378befe-b190-44d7-9da2-b1037f76b0c2')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '9574face-a278-4efe-b14f-1b23d908986f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('9574face-a278-4efe-b14f-1b23d908986f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5271e417-c24b-4320-871c-8ceca56d928f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5271e417-c24b-4320-871c-8ceca56d928f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '17b7dd80-bcc2-47ea-a3d8-275c229636b9')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('17b7dd80-bcc2-47ea-a3d8-275c229636b9')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '9a8aca10-5c5c-4bfa-8484-1072f59cccf9')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('9a8aca10-5c5c-4bfa-8484-1072f59cccf9')

GO

GO
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Account', 1)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Function', 15)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Grant', 15)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmArea', 15)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmLine', 23)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmOrigin', 58)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmSupplier', 17)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmTree', 3)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Link', 1)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Role', 3)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Service', 17)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Verificate', 1)

INSERT INTO [Account]([Unique], [Role], [Id], [Description], [Remark], [State], [Version])
VALUES(1, 1, 'zdq', '{"Department":"慧云公司","Name":"张德强","Mobile":"13570067306","Email":"zdq@gxfl.cn"}', NULL, 0, 1)

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(1, NULL, 0, 1, '短信报备', '短信报备', 'http://localhost/Gsm.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(2, NULL, 0, 2, '系统管理', '系统管理', 'http://localhost/Setting.aspx', NULL, 0, 1)

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(3, 1, 1, 3, 'GsmArea', '属地管理', 'http://localhost:15001/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(4, 1, 1, 5, 'GsmLine', '行驶路线管理', 'http://localhost:15003/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(5, 1, 0, 1, 'GsmMessage', '短信管理', 'http://localhost:15006/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(6, 1, 1, 4, 'GsmOrigin', '产地管理', 'http://localhost:15002/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(7, 1, 0, 2, 'GsmReport', '查看报表', 'http://localhost:15007/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(8, 1, 1, 6, 'GsmSupplier', '供应商管理', 'http://localhost:15004/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(9, 1, 1, 7, 'GsmTree', '树种管理', 'http://localhost:15005/Index.aspx', NULL, 0, 1)

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(10, 2, 2, 1, 'Account', '账户管理', 'http://localhost:13003/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(11, 2, 2, 5, 'Function', '功能管理', 'http://localhost:13005/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(12, 2, 2, 4, 'Grant', '授权管理', 'http://localhost:13006/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(13, 2, 2, 6, 'Link', '链接管理', 'http://localhost:13001/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(14, 2, 2, 3, 'Role', '角色管理', 'http://localhost:13002/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(15, 2, 2, 2, 'Verificate', '认证管理', 'http://localhost:13004/Index.aspx', NULL, 0, 1)

INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(1, 1, 1, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(2, 1, 2, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(3, 1, 3, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(4, 1, 4, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(5, 1, 5, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(6, 1, 6, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(7, 1, 7, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(8, 1, 8, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(9, 1, 9, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(10, 1, 10, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(11, 1, 11, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(12, 1, 12, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(13, 1, 13, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(14, 1, 14, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version]) VALUES(15, 1, 15, 0, 1)

INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (1, '南宁市', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (2, '武鸣县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (3, '田东县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (4, '隆安县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (5, '云南省', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (6, '大化县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (7, '田林县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (8, '平果县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (9, '德保县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (10, '凌云县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (11, '东兰县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (12, '那坡县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (13, '靖西县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (14, '马山县', NULL, 0, 1)
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version]) VALUES (15, '扶绥县', NULL, 0, 1)

INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (1, '者桑', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (2, '博爱', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (3, '阳圩', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (4, '高邦', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (5, '皈朝', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (6, '平年', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (7, '潞城', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (8, '旧州', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (9, '八渡', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (10, '板桃', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (11, '凌云', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (12, '德保', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (13, '祥周', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (14, '田东', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (15, '田阳', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (16, '巴马', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (17, '石埠', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (18, '坛洛', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (19, '隆安', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (20, '平果', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (21, '灵马', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (22, '马山', '', NULL, 0, 1)
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (23, '扶绥', '', NULL, 0, 1)

INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (1, 1, '南宁市金陵木片厂', '金陵', '广丰; 鸿华; 黎森', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (2, 1, '南宁金陵广丰木业', '广丰', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (3, 1, '南宁市金陵镇鸿华木片厂', '鸿华', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (4, 1, '南宁市金陵黎森木业', '黎森', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (5, 1, '南宁扶照兴东木片厂', '扶照; 兴东', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (6, 1, '南宁市金凯木片厂', '金凯', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (7, 1, '南宁市恒桂木材加工厂', '恒桂', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (8, 1, '南宁苏圩巨力木片厂', '巨力', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (9, 1, '南宁苏圩隆达木业', '隆达; 隆德', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (10, 1, '南宁市宏翔木片厂', '宏翔', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (11, 1, '南宁吴圩', '吴圩', '周德', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (12, 1, '吴圩周德木业', '周德', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (13, 1, '南宁那马林进木片厂', '那马; 林进', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (14, 1, '南宁江西镇', '江西', '顾林', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (15, 1, '南宁江西顾林木业', '顾林', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (16, 1, '南宁明阳众森木业', '明阳; 众森', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (17, 1, '南宁市坛洛厂', '坛洛', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (18, 2, '武鸣团结农场', '团结', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (19, 2, '武鸣灵马木片厂', '灵马', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (20, 2, '武鸣陆斡镇', '陆斡', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (21, 2, '武鸣罗圩木片厂', '罗圩; 锣圩', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (22, 2, '武鸣南益木业', '南益', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (23, 3, '田东木片厂', '田东', '怀民; 城南; 金城; 金成; 小龙; 苗圃', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (24, 3, '田东县怀民木片厂', '怀民', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (25, 3, '田东县城南木片厂', '城南', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (26, 3, '田东县金城木片厂', '金城; 金成', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (27, 3, '田东县小龙苗圃木片厂', '小龙; 苗圃', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (28, 3, '田东县', '田东', '木片', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (29, 4, '隆安木片加工厂', '隆安', '那桐; 丰顺; 屏山', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (30, 4, '隆安那桐木片厂', '那桐', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (31, 4, '隆安县丰顺木材厂', '丰顺', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (32, 4, '隆安屏山', '屏山', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (33, 5, '云南', '云南', '富宁; 者桑; 红森', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (34, 5, '云南富宁木片厂', '富宁', '高邦; 皈朝; 海丰; 花果山', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (35, 5, '富宁高邦木片厂', '高邦', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (36, 5, '云南富宁皈朝海丰木业', '皈朝; 海丰', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (37, 5, '富宁县花果山木材加工厂', '花果山', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (38, 5, '云南者桑红森', '者桑; 红森', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (39, 6, '大化都阳木片厂', '都阳、江南', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (40, 6, '大化县乙圩乡木片厂', '乙圩', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (41, 6, '大化县北景乡百益木片厂', '北景; 百益', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (42, 6, '大化县羌圩木片厂', '羌圩、姜圩', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (43, 6, '大化和顺木材加工厂', '和顺', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (44, 6, '大化毅力木片厂', '毅力', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (45, 7, '田林潞城富林木业', '潞城; 富林', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (46, 7, '田林县旧州加工厂', '旧州', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (47, 8, '平果木片厂', '平果', '旧城; 林苑', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (48, 8, '平果旧城', '旧城', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (49, 8, '平果县', '平果', '旧城; 林苑; 木片', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (50, 8, '平果林苑木片厂', '林苑', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (51, 9, '广西德保红泥坡林场', '德保; 红泥', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (52, 10, '凌云友良木材厂', '凌云; 友良', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (53, 11, '东兰县东森木材加工厂', '东兰; 东森', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (54, 12, '那坡县木材公司', '那坡', '德隆', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (55, 12, '那坡县德隆木片厂', '德隆', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (56, 13, '靖西县 ', '靖西', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (57, 14, '马山县', '马山', '', NULL, 0, 1)
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version]) VALUES (58, 15, '扶绥渠黎', '扶绥; 渠黎', '', NULL, 0, 1)

INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (1, 0, 'A2', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (2, 0, 'A7', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (3, 0, 'A8', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (4, 0, 'A9', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (5, 0, 'A11', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (6, 0, 'B9', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (7, 0, 'C3', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (8, 0, 'C5', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (9, 0, 'D1', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (10, 0, 'D2', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (11, 0, 'E3', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (12, 0, 'K8', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (13, 0, 'M1', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (14, 0, 'P9', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (15, 1, '庞坤耀', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (16, 1, '覃良山', NULL, 0, 1)
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version]) VALUES (17, 1, '冉景尤', NULL, 0, 1)

INSERT INTO [GsmTree] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (1, '桉木片', '桉木; 桉片; 桉树', NULL, 0, 1)
INSERT INTO [GsmTree] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (2, '松木片', '松木; 松片; 松树', NULL, 0, 1)
INSERT INTO [GsmTree] ([Unique], [Name], [Alias], [Remark], [State], [Version]) VALUES (3, '杂木片', '杂木; 杂片; 杂树; 桉杂', NULL, 0, 1)

INSERT INTO [Link]([Unique], [Name], [Url], [Remark], [State], [Version])
VALUES(1, '本系统', 'http://localhost:12004/VerificateService.svc/GetEntityByIdAndLink', NULL, 0, 1)

INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version]) VALUES(1, '系统管理员', NULL, 0, 1)
INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version]) VALUES(2, '高级用户', NULL, 0, 1)
INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version]) VALUES(3, '普通用户', NULL, 0, 1)

INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(1, 'Service', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Esb\Wodeyun.Bf.Esb.Wrappers\bin\Debug\Wodeyun.Bf.Esb.Wrappers.dll', 'http://localhost:11001/ServiceService.svc', 'Wodeyun.Bf.Esb.Wrappers.IServiceInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(2, 'Exchange.Single', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Exchange\Wodeyun.Bf.Exchange.Wrappers\bin\Debug\Wodeyun.Bf.Exchange.Wrappers.dll', 'http://localhost:11002/SingleService.svc', 'Wodeyun.Bf.Exchange.Wrappers.SingleService', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(3, 'Exchange.Multi', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Exchange\Wodeyun.Bf.Exchange.Wrappers\bin\Debug\Wodeyun.Bf.Exchange.Wrappers.dll', 'http://localhost:11002/MultiService.svc', 'Wodeyun.Bf.Exchange.Wrappers.MultiService', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(4, 'Link', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Link\Wodeyun.Bf.Link.Wrappers\bin\Debug\Wodeyun.Bf.Link.Wrappers.dll', 'http://localhost:12001/LinkService.svc', 'Wodeyun.Bf.Link.Wrappers.ILinkInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(5, 'Role', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Role\Wodeyun.Bf.Role.Wrappers\bin\Debug\Wodeyun.Bf.Role.Wrappers.dll', 'http://localhost:12002/RoleService.svc', 'Wodeyun.Bf.Role.Wrappers.IRoleInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(6, 'Account', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Account\Wodeyun.Bf.Account.Wrappers\bin\Debug\Wodeyun.Bf.Account.Wrappers.dll', 'http://localhost:12003/AccountService.svc', 'Wodeyun.Bf.Account.Wrappers.IAccountInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(7, 'Verificate', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Verificate\Wodeyun.Bf.Verificate.Wrappers\bin\Debug\Wodeyun.Bf.Verificate.Wrappers.dll', 'http://localhost:12004/VerificateService.svc', 'Wodeyun.Bf.Verificate.Wrappers.IVerificateInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(8, 'Function', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Function\Wodeyun.Bf.Function.Wrappers\bin\Debug\Wodeyun.Bf.Function.Wrappers.dll', 'http://localhost:12005/FunctionService.svc', 'Wodeyun.Bf.Function.Wrappers.IFunctionInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(9, 'Grant', 'D:\Projects\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Grant\Wodeyun.Bf.Grant.Wrappers\bin\Debug\Wodeyun.Bf.Grant.Wrappers.dll', 'http://localhost:12006/GrantService.svc', 'Wodeyun.Bf.Grant.Wrappers.IGrantInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(10, 'GsmArea', 'D:\Projects\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmArea\Wodeyun.Project.GsmArea.Wrappers\bin\Debug\Wodeyun.Project.GsmArea.Wrappers.dll', 'http://localhost:14001/GsmAreaService.svc', 'Wodeyun.Project.GsmArea.Wrappers.IGsmAreaInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(11, 'GsmOrigin', 'D:\Projects\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmOrigin\Wodeyun.Project.GsmOrigin.Wrappers\bin\Debug\Wodeyun.Project.GsmOrigin.Wrappers.dll', 'http://localhost:14002/GsmOriginService.svc', 'Wodeyun.Project.GsmOrigin.Wrappers.IGsmOriginInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(12, 'GsmLine', 'D:\Projects\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmLine\Wodeyun.Project.GsmLine.Wrappers\bin\Debug\Wodeyun.Project.GsmLine.Wrappers.dll', 'http://localhost:14003/GsmLineService.svc', 'Wodeyun.Project.GsmLine.Wrappers.IGsmLineInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(13, 'GsmSupplier', 'D:\Projects\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmSupplier\Wodeyun.Project.GsmSupplier.Wrappers\bin\Debug\Wodeyun.Project.GsmSupplier.Wrappers.dll', 'http://localhost:14004/GsmSupplierService.svc', 'Wodeyun.Project.GsmSupplier.Wrappers.IGsmSupplierInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(14, 'GsmTree', 'D:\Projects\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmTree\Wodeyun.Project.GsmTree.Wrappers\bin\Debug\Wodeyun.Project.GsmTree.Wrappers.dll', 'http://localhost:14005/GsmTreeService.svc', 'Wodeyun.Project.GsmTree.Wrappers.IGsmTreeInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(15, 'GsmMessage', 'D:\Projects\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmMessage\Wodeyun.Project.GsmMessage.Wrappers\bin\Debug\Wodeyun.Project.GsmMessage.Wrappers.dll', 'http://localhost:14006/GsmMessageService.svc', 'Wodeyun.Project.GsmMessage.Wrappers.IGsmMessageInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(16, 'GsmItem', 'D:\Projects\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmMessage\Wodeyun.Project.GsmMessage.Wrappers\bin\Debug\Wodeyun.Project.GsmMessage.Wrappers.dll', 'http://localhost:14006/GsmItemService.svc', 'Wodeyun.Project.GsmMessage.Wrappers.IGsmItemInterface', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(17, 'GsmReport', 'D:\Projects\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmReport\Wodeyun.Project.GsmReport.Wrappers\bin\Debug\Wodeyun.Project.GsmReport.Wrappers.dll', 'http://localhost:14007/GsmReportService.svc', 'Wodeyun.Project.GsmReport.Wrappers.IGsmReportInterface', NULL, 0, 1)

INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version]) VALUES(1, 1, 1, 'zdq', NULL, 0, 1)
GO

GO
PRINT N'更新完成。'
GO
