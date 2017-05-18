/*
Wodeyun.Project 的部署脚本

此代码由工具生成。
如果重新生成此代码，则对此文件的更改可能导致
不正确的行为并将丢失。
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "Wodeyun.Project"
:setvar DefaultFilePrefix "Wodeyun.Project"
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
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Account'    AND XTYPE = 'U') DROP TABLE [Account]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Link'       AND XTYPE = 'U') DROP TABLE [Link]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Token'      AND XTYPE = 'U') DROP TABLE [Token]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Unique'     AND XTYPE = 'U') DROP TABLE [Unique]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Verificate' AND XTYPE = 'U') DROP TABLE [Verificate]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Equipment'  AND XTYPE = 'U') DROP TABLE [Equipment]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Function'   AND XTYPE = 'U') DROP TABLE [Function]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Legality'   AND XTYPE = 'U') DROP TABLE [Legality]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Permission' AND XTYPE = 'U') DROP TABLE [Permission]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Role'       AND XTYPE = 'U') DROP TABLE [Role]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Scan'       AND XTYPE = 'U') DROP TABLE [Scan]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Shelf'      AND XTYPE = 'U') DROP TABLE [Shelf]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Species'    AND XTYPE = 'U') DROP TABLE [Species]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Unique'     AND XTYPE = 'U') DROP TABLE [Unique]
--IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'User'       AND XTYPE = 'U') DROP TABLE [User]
GO

GO
PRINT N'正在创建 [dbo].[Account]...';


GO
CREATE TABLE [dbo].[Account] (
    [Unique]      INT            NOT NULL,
    [Role]        INT            NOT NULL,
    [Id]          NVARCHAR (50)  NOT NULL,
    [Description] XML            NULL,
    [Remark]      NVARCHAR (MAX) NULL,
    [State]       INT            NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Unique] ASC)
);


GO
PRINT N'正在创建 [dbo].[Account].[IX_Account]...';


GO
CREATE NONCLUSTERED INDEX [IX_Account]
    ON [dbo].[Account]([Id] ASC);


GO
PRINT N'正在创建 [dbo].[Link]...';


GO
CREATE TABLE [dbo].[Link] (
    [Unique] INT            NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,
    [Url]    NVARCHAR (100) NOT NULL,
    [Remark] NVARCHAR (MAX) NULL,
    [State]  INT            NOT NULL,
    CONSTRAINT [PK_Link] PRIMARY KEY CLUSTERED ([Unique] ASC)
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
    [Link]    INT            NULL,
    [Id]      NVARCHAR (50)  NULL,
    [Value]   NVARCHAR (100) NOT NULL,
    [State]   INT            NOT NULL,
    CONSTRAINT [PK_Verificate] PRIMARY KEY CLUSTERED ([Unique] ASC)
);


GO
PRINT N'正在创建 [dbo].[Verificate].[IX_Verificate]...';


GO
CREATE NONCLUSTERED INDEX [IX_Verificate]
    ON [dbo].[Verificate]([Account] ASC);


GO
--INSERT INTO [Unique] ([Name],[Value])
--VALUES ('Bulletin',1)


INSERT INTO [Esb]([Unique], [Service], [Filename], [Url], [Contract], [Remark], [State])
VALUES(1, 'Link', 'C:\Private\Project\1.0\Codes\Wodeyun.Bf\Wodeyun.Bf.Link.Wrappers\bin\Debug\Wodeyun.Bf.Link.Wrappers.dll', 'http://localhost:83/LinkService.svc', 'Wodeyun.Bf.Link.Blls.LinkBll', NULL, 0)

INSERT INTO [Link]([Unique], [Name], [Url], [Remark], [State])
VALUES(1, '本地', 'http://localhost', NULL, 0)
GO

GO
PRINT N'更新完成。'
GO
