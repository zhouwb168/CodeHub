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
    [Alias]   NVARCHAR (255) NOT NULL,
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
    [Mobile]  NVARCHAR (50)  NOT NULL,
    [Date]    NVARCHAR (255) NOT NULL,
    [Text]    NVARCHAR (MAX) NULL,
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
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Account', 1)
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Function', 15)
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Grant', 15)
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Link', 1)
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Role', 3)
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Service', 16)
INSERT INTO [Unique] ([Name], [Value])
VALUES ('Verificate', 1)

INSERT INTO [Account]([Unique], [Role], [Id], [Description], [Remark], [State], [Version])
VALUES(1, 1, 'zdq', '{"Department":"慧云公司","Name":"张德强","Mobile":"13570067306","Email":"zdq@gxfl.cn"}', NULL, 0, 1)

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(1, NULL, 0, 1, '短信报备', '短信报备', 'http://localhost/Gsm.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(2, NULL, 0, 2, '系统管理', '系统管理', 'http://localhost/Setting.aspx', NULL, 0, 1)

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(3, 1, 1, 3, 'GsmArea', '属地管理', 'http://localhost:14001/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(4, 1, 1, 5, 'GsmLine', '行驶路线管理', 'http://localhost:14003/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(5, 1, 0, 1, 'GsmMessage', '短信管理', 'http://localhost:14006/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(6, 1, 1, 4, 'GsmOrigin', '产地管理', 'http://localhost:14002/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(7, 1, 0, 2, 'GsmReport', '查看报表', 'http://localhost:14007/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(8, 1, 1, 6, 'GsmSupplier', '供应商管理', 'http://localhost:14004/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(9, 1, 1, 7, 'GsmTree', '树种管理', 'http://localhost:14005/Index.aspx', NULL, 0, 1)

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(10, 2, 2, 1, 'Account', '账户管理', 'http://localhost:12003/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(11, 2, 2, 5, 'Function', '功能管理', 'http://localhost:12005/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(12, 2, 2, 4, 'Grant', '授权管理', 'http://localhost:12006/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(13, 2, 2, 6, 'Link', '链接管理', 'http://localhost:12001/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(14, 2, 2, 3, 'Role', '角色管理', 'http://localhost:12002/Index.aspx', NULL, 0, 1)
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version])
VALUES(15, 2, 2, 2, 'Verificate', '认证管理', 'http://localhost:12004/Index.aspx', NULL, 0, 1)

INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(1, 1, 1, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(2, 1, 2, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(3, 1, 3, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(4, 1, 4, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(5, 1, 5, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(6, 1, 6, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(7, 1, 7, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(8, 1, 8, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(9, 1, 9, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(10, 1, 10, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(11, 1, 11, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(12, 1, 12, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(13, 1, 13, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(14, 1, 14, 0, 1)
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version])
VALUES(15, 1, 15, 0, 1)

INSERT INTO [Link]([Unique], [Name], [Url], [Remark], [State], [Version])
VALUES(1, '本系统', 'http://localhost:12004/VerificateService.svc/GetEntityByIdAndLink', NULL, 0, 1)

INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version])
VALUES(1, '系统管理员', NULL, 0, 1)
INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version])
VALUES(2, '高级用户', NULL, 0, 1)
INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version])
VALUES(3, '普通用户', NULL, 0, 1)

INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(1, 'Service', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Esb\Wodeyun.Bf.Esb.Wrappers\bin\Debug\Wodeyun.Bf.Esb.Wrappers.dll', 'http://localhost:11001/ServiceService.svc', 'Wodeyun.Bf.Esb.Wrappers.ServiceBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(2, 'Exchange.Single', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Exchange\Wodeyun.Bf.Exchange.Wrappers\bin\Debug\Wodeyun.Bf.Exchange.Wrappers.dll', 'http://localhost:11002/SingleService.svc', 'Wodeyun.Bf.Exchange.Wrappers.SingleService', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(3, 'Exchange.Multi', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Exchange\Wodeyun.Bf.Exchange.Wrappers\bin\Debug\Wodeyun.Bf.Exchange.Wrappers.dll', 'http://localhost:11002/MultiService.svc', 'Wodeyun.Bf.Exchange.Wrappers.MultiService', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(4, 'Link', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Link\Wodeyun.Bf.Link.Wrappers\bin\Debug\Wodeyun.Bf.Link.Wrappers.dll', 'http://localhost:12001/LinkService.svc', 'Wodeyun.Bf.Link.Wrappers.LinkBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(5, 'Role', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Role\Wodeyun.Bf.Role.Wrappers\bin\Debug\Wodeyun.Bf.Role.Wrappers.dll', 'http://localhost:12002/RoleService.svc', 'Wodeyun.Bf.Role.Wrappers.RoleBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(6, 'Account', 'C:\Private\Project\Baise\Codes\Wodeyun.Bf\Wodeyun.Bf.Account\Wodeyun.Bf.Account.Wrappers\bin\Debug\Wodeyun.Bf.Account.Wrappers.dll', 'http://localhost:12003/AccountService.svc', 'Wodeyun.Bf.Account.Wrappers.AccountBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(7, 'Verificate', 'C:\Private\Project\Warehouse\Codes\Wodeyun.Bf\Wodeyun.Bf.Verificate\Wodeyun.Bf.Verificate.Wrappers\bin\Debug\Wodeyun.Bf.Verificate.Wrappers.dll', 'http://localhost:12004/VerificateService.svc', 'Wodeyun.Bf.Verificate.Wrappers.VerificateBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(8, 'Function', 'C:\Private\Project\Warehouse\Codes\Wodeyun.Bf\Wodeyun.Bf.Function\Wodeyun.Bf.Function.Wrappers\bin\Debug\Wodeyun.Bf.Function.Wrappers.dll', 'http://localhost:12005/FunctionService.svc', 'Wodeyun.Bf.Function.Wrappers.FunctionBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(9, 'Grant', 'C:\Private\Project\Warehouse\Codes\Wodeyun.Bf\Wodeyun.Bf.Grant\Wodeyun.Bf.Grant.Wrappers\bin\Debug\Wodeyun.Bf.Grant.Wrappers.dll', 'http://localhost:12006/GrantService.svc', 'Wodeyun.Bf.Grant.Wrappers.GrantBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(10, 'GsmArea', 'C:\Private\Project\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmArea\Wodeyun.Project.GsmArea.Wrappers\bin\Debug\Wodeyun.Project.GsmArea.Wrappers.dll', 'http://localhost:14001/GsmAreaService.svc', 'Wodeyun.Project.GsmArea.Wrappers.GsmAreaBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(11, 'GsmOrigin', 'C:\Private\Project\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmOrigin\Wodeyun.Project.GsmOrigin.Wrappers\bin\Debug\Wodeyun.Project.GsmOrigin.Wrappers.dll', 'http://localhost:14002/GsmOriginService.svc', 'Wodeyun.Project.GsmOrigin.Wrappers.GsmOriginBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(12, 'GsmLine', 'C:\Private\Project\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmLine\Wodeyun.Project.GsmLine.Wrappers\bin\Debug\Wodeyun.Project.GsmLine.Wrappers.dll', 'http://localhost:14003/GsmLineService.svc', 'Wodeyun.Project.GsmLine.Wrappers.GsmLineBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(13, 'GsmSupplier', 'C:\Private\Project\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmSupplier\Wodeyun.Project.GsmSupplier.Wrappers\bin\Debug\Wodeyun.Project.GsmSupplier.Wrappers.dll', 'http://localhost:14004/GsmSupplierService.svc', 'Wodeyun.Project.GsmSupplier.Wrappers.GsmSupplierBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(14, 'GsmTree', 'C:\Private\Project\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmTree\Wodeyun.Project.GsmTree.Wrappers\bin\Debug\Wodeyun.Project.GsmTree.Wrappers.dll', 'http://localhost:14005/GsmTreeService.svc', 'Wodeyun.Project.GsmTree.Wrappers.GsmTreeBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(15, 'GsmMessage', 'C:\Private\Project\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmMessage\Wodeyun.Project.GsmMessage.Wrappers\bin\Debug\Wodeyun.Project.GsmMessage.Wrappers.dll', 'http://localhost:14006/GsmMessageService.svc', 'Wodeyun.Project.GsmMessage.Wrappers.GsmMessageBll', NULL, 0, 1)
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version])
VALUES(16, 'GsmReport', 'C:\Private\Project\Baise\Codes\Wodeyun.Project\Wodeyun.Project.GsmReport\Wodeyun.Project.GsmReport.Wrappers\bin\Debug\Wodeyun.Project.GsmReport.Wrappers.dll', 'http://localhost:14007/GsmReportService.svc', 'Wodeyun.Project.GsmReport.Wrappers.GsmReportBll', NULL, 0, 1)

INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version])
VALUES(1, 1, 1, 'zdq', NULL, 0, 1)
GO

GO
PRINT N'更新完成。'
GO
