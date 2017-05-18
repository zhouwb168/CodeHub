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
PRINT N'正在改变 [dbo].[Esb]...';


GO
ALTER TABLE [dbo].[Esb] ALTER COLUMN [Filename] NVARCHAR (255) NOT NULL;

ALTER TABLE [dbo].[Esb] ALTER COLUMN [Url] NVARCHAR (255) NOT NULL;


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
