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
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Authorize'  AND XTYPE = 'U') DROP TABLE [Authorize]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Car'        AND XTYPE = 'U') DROP TABLE [Car]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Chek'       AND XTYPE = 'U') DROP TABLE [Check]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Department' AND XTYPE = 'U') DROP TABLE [Department]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Detail'     AND XTYPE = 'U') DROP TABLE [Detail]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Equipment'  AND XTYPE = 'U') DROP TABLE [Equipment]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Function'   AND XTYPE = 'U') DROP TABLE [Function]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Legality'   AND XTYPE = 'U') DROP TABLE [Legality]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Permission' AND XTYPE = 'U') DROP TABLE [Permission]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Role'       AND XTYPE = 'U') DROP TABLE [Role]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Scan'       AND XTYPE = 'U') DROP TABLE [Scan]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Shelf'      AND XTYPE = 'U') DROP TABLE [Shelf]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Species'    AND XTYPE = 'U') DROP TABLE [Species]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Unique'     AND XTYPE = 'U') DROP TABLE [Unique]
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'User'       AND XTYPE = 'U') DROP TABLE [User]
GO

GO
--INSERT INTO [Authorize]([Id],[No],[Name],[Start],[End],[Remark],[State],[User],[Date])
--VALUES(1,'001','演习','2012-05-01 12:00:00','2012-05-01 14:00:00',NULL,1,1,'2012-05-01 00:00:00')

INSERT INTO [Car]([Id],[No],[License],[Charge],[Department],[Remark],[State],[User],[Date])
VALUES(1,'001','广0 00000','莫参谋',1,NULL,1,1,'2012-05-01 00:00:00')

--INSERT INTO [Check]([Id],[No],[Name],[Start],[End],[Remark],[State],[User],[Date])
--VALUES(1,'001','演习','2012-05-01 12:00:00','2012-05-01 14:00:00',NULL,1,1,'2012-05-01 00:00:00')

INSERT INTO [Department]([Id],[No],[Name],[Remark],[State],[User],[Date])
VALUES(1,'001','基本指挥所',NULL,1,1,'2012-05-01 00:00:00')

INSERT INTO [Detail]([Id],[Species],[No],[Name],[Quantity],[Unit],[Remark],[State],[User],[Date])
VALUES(1,1,'001','指南针',1,'个',NULL,1,1,'2012-05-01 00:00:00')
INSERT INTO [Detail]([Id],[Species],[No],[Name],[Quantity],[Unit],[Remark],[State],[User],[Date])
VALUES(2,1,'002','望远镜',1,'个',NULL,1,1,'2012-05-01 00:00:00')

INSERT INTO [Equipment]([Id],[No],[Name],[Rfid],[X],[Y],[Charge],[Species],[Department],[Car],[Shelf],[Remark],[State],[User],[Date])
VALUES(1,'001','背包','01 02 03 04 05 06 07',1,1,'莫参谋',1,1,1,1,NULL,1,1,'2012-05-01 00:00:00')
INSERT INTO [Equipment]([Id],[No],[Name],[Rfid],[X],[Y],[Charge],[Species],[Department],[Car],[Shelf],[Remark],[State],[User],[Date])
VALUES(2,'002','箱子','01 02 03 04 05 06 07',1,1,'莫参谋',1,1,NULL,2,NULL,1,1,'2012-05-01 00:00:00')

--INSERT INTO [Function]([Id],[Name],[Remark])
--VALUES(1,'用户管理',NULL)

--INSERT INTO [Legality]([Id],[Authorize],[Rfid],[State],[User],[Date])
--VALUES(1,1,'01 02 03 04 05 06 07',1,1,'2012-05-01 00:00:00')

--INSERT INTO [Permission]([Id],[Role],[Function],[State],[User],[Date])
--VALUES(1,1,1,1,1,'2012-05-01 00:00:00')

INSERT INTO [Role]([Id],[Name],[Remark],[State],[User],[Date])
VALUES(1,'系统管理员',NULL,1,1,'2012-05-01 00:00:00')
INSERT INTO [Role]([Id],[Name],[Remark],[State],[User],[Date])
VALUES(2,'高级用户',NULL,1,1,'2012-05-01 00:00:00')
INSERT INTO [Role]([Id],[Name],[Remark],[State],[User],[Date])
VALUES(3,'普通用户',NULL,1,1,'2012-05-01 00:00:00')

--INSERT INTO [Scan]([Id],[Type],[Unique],[Scanner],[Rfid],[Date])
--VALUES(1,1,NULL,'01','01 02 03 04 05 06 07','2012-05-01 00:00:00')

INSERT INTO [Shelf]([Id],[No],[Name],[Rfid],[X],[Y],[Charge],[Department],[Car],[Remark],[State],[User],[Date])
VALUES(1,'001','1号架','01 02 03 04 05 06 07',1,1,'莫参谋',1,1,NULL,1,1,'2012-05-01 00:00:00')
INSERT INTO [Shelf]([Id],[No],[Name],[Rfid],[X],[Y],[Charge],[Department],[Car],[Remark],[State],[User],[Date])
VALUES(2,'002','2号架','01 02 03 04 05 06 07',1,1,'莫参谋',1,NULL,NULL,1,1,'2012-05-01 00:00:00')

INSERT INTO [Species]([Id],[No],[Name],[Remark],[State],[User],[Date])
VALUES(1,'001','背包',NULL,1,1,'2012-05-01 00:00:00')
INSERT INTO [Species]([Id],[No],[Name],[Remark],[State],[User],[Date])
VALUES(2,'002','行李箱',NULL,1,1,'2012-05-01 00:00:00')

--INSERT INTO [Unique] ([Name],[Value])
--VALUES ('Bulletin',1)

INSERT INTO [User]([Id],[Role],[Username],[Password],[No],[Name],[Department],[Remark],[State],[Times],[Last],[User],[Date])
VALUES(1,1,'Username','Password','001','莫参谋',1,NULL,1,1,'2012-05-01 00:00:00',1,'2012-05-01 00:00:00')
INSERT INTO [User]([Id],[Role],[Username],[Password],[No],[Name],[Department],[Remark],[State],[Times],[Last],[User],[Date])
VALUES(2,1,'Username1','Password1','002','唐参谋',1,NULL,1,1,'2012-05-01 00:00:00',1,'2012-05-01 00:00:00')
GO

GO
PRINT N'更新完成。'
GO
