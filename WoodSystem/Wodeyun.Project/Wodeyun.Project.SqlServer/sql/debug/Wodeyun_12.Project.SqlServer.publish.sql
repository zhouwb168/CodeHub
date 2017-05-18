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
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\"

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
PRINT N'正在创建 [dbo].[Account]...';


GO
CREATE TABLE [dbo].[Account] (
    [Unique]      INT            NOT NULL,
    [Role]        INT            NOT NULL,
    [Id]          NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Remark]      NVARCHAR (MAX) NULL,
    [State]       INT            NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[Account].[IX_Account]...';


GO
CREATE NONCLUSTERED INDEX [IX_Account]
    ON [dbo].[Account]([Id] ASC);


GO
PRINT N'正在创建 [dbo].[Function]...';


GO
CREATE TABLE [dbo].[Function] (
    [Unique] INT            NOT NULL,
    [Parent] INT            NULL,
    [Type]   INT            NOT NULL,
    [Order]  INT            NOT NULL,
    [No]     NVARCHAR (50)  NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Url]    NVARCHAR (255) NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_Function] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[Grant]...';


GO
CREATE TABLE [dbo].[Grant] (
    [Unique]   INT NOT NULL,
    [Role]     INT NOT NULL,
    [Function] INT NOT NULL,
    [State]    INT NOT NULL,
    CONSTRAINT [PK_Grant] PRIMARY KEY CLUSTERED ([Unique] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmArea]...';


GO
CREATE TABLE [dbo].[GsmArea] (
    [Unique] INT            NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Alias]  NVARCHAR (255) NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_GsmArea] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmItem]...';


GO
CREATE TABLE [dbo].[GsmItem] (
    [Unique]   INT            NOT NULL,
    [Message]  INT            NOT NULL,
    [Supplier] INT            NOT NULL,
    [Tree]     INT            NOT NULL,
    [Make]     DATETIME       NOT NULL,
    [Origin]   INT            NOT NULL,
    [License]  NVARCHAR (50)  NOT NULL,
    [Mobile]   NVARCHAR (50)  NOT NULL,
    [Ship]     DATETIME       NOT NULL,
    [Line]     INT            NOT NULL,
    [Remark]   NVARCHAR (MAX) NULL,
    [State]    INT            NOT NULL,
    CONSTRAINT [PK_GsmItem] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmLine]...';


GO
CREATE TABLE [dbo].[GsmLine] (
    [Unique] INT            NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Alias]  NVARCHAR (255) NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_GsmLine] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmMessage]...';


GO
CREATE TABLE [dbo].[GsmMessage] (
    [Unique] INT            NOT NULL,
    [Mobile] NVARCHAR (50)  NOT NULL,
    [Date]   NVARCHAR (255) NOT NULL,
    [Text]   NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_GsmMessage] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmOrigin]...';


GO
CREATE TABLE [dbo].[GsmOrigin] (
    [Unique] INT            NOT NULL,
    [Area]   INT            NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Alias]  NVARCHAR (255) NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_GsmOrigin] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmSupplier]...';


GO
CREATE TABLE [dbo].[GsmSupplier] (
    [Unique] INT            NOT NULL,
    [Type]   INT            NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_GsmSupplier] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[GsmTree]...';


GO
CREATE TABLE [dbo].[GsmTree] (
    [Unique] INT            NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Alias]  NVARCHAR (255) NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_GsmTree] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[Link]...';


GO
CREATE TABLE [dbo].[Link] (
    [Unique] INT            NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Url]    NVARCHAR (100) NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_Link] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[Role]...';


GO
CREATE TABLE [dbo].[Role] (
    [Unique] INT            NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
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
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
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
    CONSTRAINT [PK_Verificate] PRIMARY KEY CLUSTERED ([Unique] ASC, [State] ASC)
);


GO
PRINT N'正在创建 [dbo].[Verificate].[IX_Verificate]...';


GO
CREATE NONCLUSTERED INDEX [IX_Verificate]
    ON [dbo].[Verificate]([Account] ASC);


GO
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Account',1)
--INSERT INTO [Unique] ([Name], [Value])
--VALUES ('Function',18)
--INSERT INTO [Unique] ([Name], [Value])
--VALUES ('Grant',18)
--INSERT INTO [Unique] ([Name], [Value])
--VALUES ('Link',1)
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Role',3)
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Service',4)
--INSERT INTO [Unique] ([Name], [Value])
--VALUES ('Supplier',33)

INSERT INTO [Account]([Unique], [Role], [Id], [Description], [Remark], [State])
VALUES(1, 1, 'zdq', '{"Department":"慧云公司","Name":"张德强","Mobile":"13570067306","Email":"zdq@gxfl.cn"}', NULL, 0)

--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(1, NULL, 0, 1, '短信报备', '短信报备', 'http://localhost:189/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(2, NULL, 0, 2, '供应商管理', '警告', 'http://localhost:189/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(2, NULL, 0, 3, 'Authorize', '授权', 'http://localhost:181/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(3, NULL, 0, 4, 'Change', '变更', 'http://localhost:183/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(4, NULL, 0, 5, 'Check', '盘点', 'http://localhost:185/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(5, NULL, 0, 6, 'Setting', '设置', 'http://localhost:80/Setting.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(6, 5, 1, 1, 'Species', '种类管理', 'http://localhost:161/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(12, 5, 2, 1, 'Function', '功能管理', 'http://localhost:106/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(13, 5, 2, 2, 'Role', '角色管理', 'http://localhost:112/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(14, 5, 2, 3, 'Grant', '授权管理', 'http://localhost:108/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(15, 5, 2, 4, 'Link', '链接管理', 'http://localhost:110/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(16, 5, 2, 5, 'Account', '账户管理', 'http://localhost:102/Index.aspx', NULL, 0)
--INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
--VALUES(17, 5, 2, 6, 'Verificate', '认证管理', 'http://localhost:114/Index.aspx', NULL, 0)

--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(1, 1, 1, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(2, 1, 2, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(3, 1, 3, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(4, 1, 4, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(5, 1, 5, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(6, 1, 6, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(7, 1, 7, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(8, 1, 8, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(9, 1, 9, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(10, 1, 10, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(11, 1, 11, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(12, 1, 12, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(13, 1, 13, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(14, 1, 14, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(15, 1, 15, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(16, 1, 16, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(17, 1, 17, 0)
--INSERT INTO [Grant]([Unique], [Role], [Function], [State])
--VALUES(18, 1, 18, 0)

--INSERT INTO [Link]([Unique], [Name], [Url], [Remark], [State])
--VALUES(1, '本系统', 'http://localhost:113/VerificateService.svc/GetEntityByIdAndLink', NULL, 0)

INSERT INTO [Role]([Unique], [Name], [Remark], [State])
VALUES(1, '系统管理员', NULL, 0)
INSERT INTO [Role]([Unique], [Name], [Remark], [State])
VALUES(2, '高级用户', NULL, 0)
INSERT INTO [Role]([Unique], [Name], [Remark], [State])
VALUES(3, '普通用户', NULL, 0)

INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
VALUES(1, 'Service', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Esb\Wodeyun.Bf.Esb.Wrappers\bin\Debug\Wodeyun.Bf.Esb.Wrappers.dll', 'http://localhost:11001/ServiceService.svc', 'Wodeyun.Bf.Esb.Wrappers.ServiceBll', NULL, 0)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
VALUES(2, 'Exchange.Single', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Exchange\Wodeyun.Bf.Exchange.Wrappers\bin\Debug\Wodeyun.Bf.Exchange.Wrappers.dll', 'http://localhost:11002/SingleService.svc', 'Wodeyun.Bf.Exchange.Wrappers.SingleService', NULL, 0)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
VALUES(3, 'Exchange.Multi', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Exchange\Wodeyun.Bf.Exchange.Wrappers\bin\Debug\Wodeyun.Bf.Exchange.Wrappers.dll', 'http://localhost:11002/MultiService.svc', 'Wodeyun.Bf.Exchange.Wrappers.MultiService', NULL, 0)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
VALUES(4, 'Account', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Account\Wodeyun.Bf.Account.Wrappers\bin\Debug\Wodeyun.Bf.Account.Wrappers.dll', 'http://localhost:12001/AccountService.svc', 'Wodeyun.Bf.Account.Wrappers.AccountBll', NULL, 0)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
VALUES(5, 'GsmArea', 'C:\Private\Project\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmArea\Wodeyun.Project.GsmArea.Wrappers\bin\Debug\Wodeyun.Project.GsmArea.Wrappers.dll', 'http://localhost:14001/GsmAreaService.svc', 'Wodeyun.Project.GsmArea.Wrappers.GsmAreaBll', NULL, 0)

--INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
--VALUES(2, 'Function', 'C:\Private\Project\Warehouse\Codes\Wodeyun.Bf\Wodeyun.Bf.Function\Wodeyun.Bf.Function.Wrappers\bin\Debug\Wodeyun.Bf.Function.Wrappers.dll', 'http://localhost:105/FunctionService.svc', 'Wodeyun.Bf.Function.Wrappers.FunctionBll', NULL, 0)
--INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
--VALUES(3, 'Grant', 'C:\Private\Project\Warehouse\Codes\Wodeyun.Bf\Wodeyun.Bf.Grant\Wodeyun.Bf.Grant.Wrappers\bin\Debug\Wodeyun.Bf.Grant.Wrappers.dll', 'http://localhost:107/GrantService.svc', 'Wodeyun.Bf.Grant.Wrappers.GrantBll', NULL, 0)
--INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
--VALUES(4, 'Link', 'C:\Private\Project\Warehouse\Codes\Wodeyun.Bf\Wodeyun.Bf.Link\Wodeyun.Bf.Link.Wrappers\bin\Debug\Wodeyun.Bf.Link.Wrappers.dll', 'http://localhost:109/LinkService.svc', 'Wodeyun.Bf.Link.Wrappers.LinkBll', NULL, 0)
--INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
--VALUES(5, 'Role', 'C:\Private\Project\Warehouse\Codes\Wodeyun.Bf\Wodeyun.Bf.Role\Wodeyun.Bf.Role.Wrappers\bin\Debug\Wodeyun.Bf.Role.Wrappers.dll', 'http://localhost:111/RoleService.svc', 'Wodeyun.Bf.Role.Wrappers.RoleBll', NULL, 0)
--INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
--VALUES(7, 'Verificate', 'C:\Private\Project\Warehouse\Codes\Wodeyun.Bf\Wodeyun.Bf.Verificate\Wodeyun.Bf.Verificate.Wrappers\bin\Debug\Wodeyun.Bf.Verificate.Wrappers.dll', 'http://localhost:113/VerificateService.svc', 'Wodeyun.Bf.Verificate.Wrappers.VerificateBll', NULL, 0)
GO

GO
PRINT N'更新完成。'
GO
