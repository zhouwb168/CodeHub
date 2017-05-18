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
/*
必须添加表 [dbo].[Account] 中的列 [dbo].[Account].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[Account])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[Function] 中的列 [dbo].[Function].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[Function])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[Grant] 中的列 [dbo].[Grant].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[Grant])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[GsmArea] 中的列 [dbo].[GsmArea].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmArea])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[GsmItem] 中的列 [dbo].[GsmItem].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmItem])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[GsmLine] 中的列 [dbo].[GsmLine].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmLine])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[GsmMessage] 中的列 [dbo].[GsmMessage].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmMessage])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[GsmOrigin] 中的列 [dbo].[GsmOrigin].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmOrigin])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[GsmSupplier] 中的列 [dbo].[GsmSupplier].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmSupplier])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[GsmTree] 中的列 [dbo].[GsmTree].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmTree])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[Link] 中的列 [dbo].[Link].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[Link])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[Role] 中的列 [dbo].[Role].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[Role])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[Service] 中的列 [dbo].[Service].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[Service])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
必须添加表 [dbo].[Verificate] 中的列 [dbo].[Verificate].[Version]，但该列无默认值，也不允许使用 NULL 值。如果表中包含数据，ALTER 脚本将不能工作。为避免此问题，必须: 向该列添加默认值，或者将其标记为允许使用 NULL 值，或者启用智能默认值生成功能作为部署选项。
*/

IF EXISTS (select top 1 1 from [dbo].[Verificate])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
PRINT N'正在删除 [dbo].[Account].[IX_Account]...';


GO
DROP INDEX [IX_Account]
    ON [dbo].[Account];


GO
PRINT N'正在删除 [dbo].[Verificate].[IX_Verificate]...';


GO
DROP INDEX [IX_Verificate]
    ON [dbo].[Verificate];


GO
PRINT N'正在开始重新生成表 [dbo].[Account]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Account] (
    [Unique]      INT            NOT NULL,
    [Role]        INT            NOT NULL,
    [Id]          NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Remark]      NVARCHAR (MAX) NULL,
    [State]       INT            NOT NULL,
    [Version]     INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Account] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Account])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_Account] ([Unique], [Role], [Id], [Description], [Remark], [State])
        SELECT   [Unique],
                 [Role],
                 [Id],
                 [Description],
                 [Remark],
                 [State]
        FROM     [dbo].[Account]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[Account];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Account]', N'Account';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Account]', N'PK_Account', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[Function]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Function] (
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
    CONSTRAINT [tmp_ms_xx_constraint_PK_Function] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Function])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_Function] ([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State])
        SELECT   [Unique],
                 [Parent],
                 [Type],
                 [Order],
                 [No],
                 [Name],
                 [Url],
                 [Remark],
                 [State]
        FROM     [dbo].[Function]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[Function];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Function]', N'Function';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Function]', N'PK_Function', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[Grant]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Grant] (
    [Unique]   INT NOT NULL,
    [Role]     INT NOT NULL,
    [Function] INT NOT NULL,
    [State]    INT NOT NULL,
    [Version]  INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Grant] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Grant])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_Grant] ([Unique], [Role], [Function], [State])
        SELECT   [Unique],
                 [Role],
                 [Function],
                 [State]
        FROM     [dbo].[Grant]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[Grant];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Grant]', N'Grant';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Grant]', N'PK_Grant', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[GsmArea]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_GsmArea] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Alias]   NVARCHAR (255) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_GsmArea] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[GsmArea])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_GsmArea] ([Unique], [Name], [Alias], [Remark], [State])
        SELECT   [Unique],
                 [Name],
                 [Alias],
                 [Remark],
                 [State]
        FROM     [dbo].[GsmArea]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[GsmArea];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_GsmArea]', N'GsmArea';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_GsmArea]', N'PK_GsmArea', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[GsmItem]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_GsmItem] (
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
    CONSTRAINT [tmp_ms_xx_constraint_PK_GsmItem] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[GsmItem])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_GsmItem] ([Unique], [Message], [Supplier], [Tree], [Make], [Origin], [License], [Mobile], [Ship], [Line], [Remark], [State])
        SELECT   [Unique],
                 [Message],
                 [Supplier],
                 [Tree],
                 [Make],
                 [Origin],
                 [License],
                 [Mobile],
                 [Ship],
                 [Line],
                 [Remark],
                 [State]
        FROM     [dbo].[GsmItem]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[GsmItem];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_GsmItem]', N'GsmItem';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_GsmItem]', N'PK_GsmItem', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[GsmLine]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_GsmLine] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Alias]   NVARCHAR (255) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_GsmLine] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[GsmLine])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_GsmLine] ([Unique], [Name], [Alias], [Remark], [State])
        SELECT   [Unique],
                 [Name],
                 [Alias],
                 [Remark],
                 [State]
        FROM     [dbo].[GsmLine]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[GsmLine];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_GsmLine]', N'GsmLine';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_GsmLine]', N'PK_GsmLine', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[GsmMessage]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_GsmMessage] (
    [Unique]  INT            NOT NULL,
    [Mobile]  NVARCHAR (50)  NOT NULL,
    [Date]    NVARCHAR (255) NOT NULL,
    [Text]    NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_GsmMessage] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[GsmMessage])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_GsmMessage] ([Unique], [Mobile], [Date], [Text], [State])
        SELECT   [Unique],
                 [Mobile],
                 [Date],
                 [Text],
                 [State]
        FROM     [dbo].[GsmMessage]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[GsmMessage];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_GsmMessage]', N'GsmMessage';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_GsmMessage]', N'PK_GsmMessage', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[GsmOrigin]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_GsmOrigin] (
    [Unique]  INT            NOT NULL,
    [Area]    INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Alias]   NVARCHAR (255) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_GsmOrigin] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[GsmOrigin])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_GsmOrigin] ([Unique], [Area], [Name], [Alias], [Remark], [State])
        SELECT   [Unique],
                 [Area],
                 [Name],
                 [Alias],
                 [Remark],
                 [State]
        FROM     [dbo].[GsmOrigin]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[GsmOrigin];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_GsmOrigin]', N'GsmOrigin';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_GsmOrigin]', N'PK_GsmOrigin', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[GsmSupplier]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_GsmSupplier] (
    [Unique]  INT            NOT NULL,
    [Type]    INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_GsmSupplier] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[GsmSupplier])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_GsmSupplier] ([Unique], [Type], [Name], [Remark], [State])
        SELECT   [Unique],
                 [Type],
                 [Name],
                 [Remark],
                 [State]
        FROM     [dbo].[GsmSupplier]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[GsmSupplier];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_GsmSupplier]', N'GsmSupplier';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_GsmSupplier]', N'PK_GsmSupplier', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[GsmTree]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_GsmTree] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Alias]   NVARCHAR (255) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_GsmTree] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[GsmTree])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_GsmTree] ([Unique], [Name], [Alias], [Remark], [State])
        SELECT   [Unique],
                 [Name],
                 [Alias],
                 [Remark],
                 [State]
        FROM     [dbo].[GsmTree]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[GsmTree];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_GsmTree]', N'GsmTree';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_GsmTree]', N'PK_GsmTree', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[Link]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Link] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Url]     NVARCHAR (100) NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Link] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Link])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_Link] ([Unique], [Name], [Url], [Remark], [State])
        SELECT   [Unique],
                 [Name],
                 [Url],
                 [Remark],
                 [State]
        FROM     [dbo].[Link]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[Link];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Link]', N'Link';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Link]', N'PK_Link', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[Role]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Role] (
    [Unique]  INT            NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Role] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Role])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_Role] ([Unique], [Name], [Remark], [State])
        SELECT   [Unique],
                 [Name],
                 [Remark],
                 [State]
        FROM     [dbo].[Role]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[Role];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Role]', N'Role';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Role]', N'PK_Role', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[Service]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Service] (
    [Unique]   INT            NOT NULL,
    [Name]     NVARCHAR (50)  NOT NULL,
    [Filename] NVARCHAR (255) NOT NULL,
    [Url]      NVARCHAR (255) NOT NULL,
    [Contract] NVARCHAR (100) NOT NULL,
    [Remark]   NVARCHAR (MAX) NULL,
    [State]    INT            NOT NULL,
    [Version]  INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Service] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Service])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_Service] ([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State])
        SELECT   [Unique],
                 [Name],
                 [Filename],
                 [Url],
                 [Contract],
                 [Remark],
                 [State]
        FROM     [dbo].[Service]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[Service];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Service]', N'Service';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Service]', N'PK_Service', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在开始重新生成表 [dbo].[Verificate]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Verificate] (
    [Unique]  INT            NOT NULL,
    [Account] INT            NOT NULL,
    [Link]    INT            NOT NULL,
    [Value]   NVARCHAR (50)  NOT NULL,
    [Remark]  NVARCHAR (MAX) NULL,
    [State]   INT            NOT NULL,
    [Version] INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Verificate] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Verificate])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_Verificate] ([Unique], [Account], [Link], [Value], [Remark], [State])
        SELECT   [Unique],
                 [Account],
                 [Link],
                 [Value],
                 [Remark],
                 [State]
        FROM     [dbo].[Verificate]
        ORDER BY [Unique] ASC;
        
    END

DROP TABLE [dbo].[Verificate];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Verificate]', N'Verificate';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Verificate]', N'PK_Verificate', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


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
