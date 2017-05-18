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
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER2012\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER2012\MSSQL\DATA\"

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
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'Act'         AND XTYPE = 'U') DROP TABLE [Act]
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
正在删除列 [dbo].[EmptyPound].[LFDate]，可能会出现数据丢失。

正在删除列 [dbo].[EmptyPound].[LFUnique]，可能会出现数据丢失。
*/

IF EXISTS (select top 1 1 from [dbo].[EmptyPound])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
正在删除列 [dbo].[FullPound].[LFDate]，可能会出现数据丢失。

正在删除列 [dbo].[FullPound].[LFUnique]，可能会出现数据丢失。

正在删除列 [dbo].[FullPound].[Printed]，可能会出现数据丢失。
*/

IF EXISTS (select top 1 1 from [dbo].[FullPound])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
正在删除列 [dbo].[GsmItem].[IsAdd]，可能会出现数据丢失。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmItem])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
正在删除列 [dbo].[GsmSupplier].[LinkMan]，可能会出现数据丢失。

正在删除列 [dbo].[GsmSupplier].[LinkPhone]，可能会出现数据丢失。
*/

IF EXISTS (select top 1 1 from [dbo].[GsmSupplier])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
/*
正在删除列 [dbo].[WoodLaboratory].[RebateGreater]，可能会出现数据丢失。
*/

IF EXISTS (select top 1 1 from [dbo].[WoodLaboratory])
    RAISERROR (N'检测到行。由于可能丢失数据，正在终止架构更新。', 16, 127) WITH NOWAIT

GO
PRINT N'已跳过具有键 585b8a99-0b2a-4dbf-acd1-5e07fc72d767 的重命名重构操作，不会将元素 [dbo].[Wood].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 0ba2448f-1f8a-44c8-8be5-73de3b12ff3c 的重命名重构操作，不会将元素 [dbo].[Wood].[Operator] (SqlSimpleColumn) 重命名为 ArriveDate';


GO
PRINT N'已跳过具有键 b3ee4060-1849-4523-aae6-cb15d35dd284 的重命名重构操作，不会将元素 [dbo].[Wood].[State] (SqlSimpleColumn) 重命名为 Operator';


GO
PRINT N'已跳过具有键 c5c2929c-4660-4bab-9f52-80787e7e11bd 的重命名重构操作，不会将元素 [dbo].[Wood].[Version] (SqlSimpleColumn) 重命名为 State';


GO
PRINT N'已跳过具有键 a3997c11-3625-4bef-b62e-2dfaf8040212 的重命名重构操作，不会将元素 [dbo].[Track].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 e5df9c4f-d607-42fd-9c6d-bd3a30e7b1e3 的重命名重构操作，不会将元素 [dbo].[Card].[Id] (SqlSimpleColumn) 重命名为 Number';


GO
PRINT N'已跳过具有键 1e020b72-92a1-458a-a380-4fd1d8fcae4b 的重命名重构操作，不会将元素 [dbo].[Barriers].[Id] (SqlSimpleColumn) 重命名为 Place';


GO
PRINT N'已跳过具有键 4ef88f70-989a-4b5a-ba7c-e20eafd79b63 的重命名重构操作，不会将元素 [dbo].[Barriers].[Unique] (SqlSimpleColumn) 重命名为 WoodID';


GO
PRINT N'已跳过具有键 96771660-329d-4f60-838b-1778a158068f 的重命名重构操作，不会将元素 [dbo].[Card].[Unique] (SqlSimpleColumn) 重命名为 WoodID';


GO
PRINT N'已跳过具有键 b0a28293-a24f-4868-b937-8268fefc59ff 的重命名重构操作，不会将元素 [dbo].[Track].[Operator] (SqlSimpleColumn) 重命名为 Version';


GO
PRINT N'已跳过具有键 277303f9-212f-4edd-8ee0-72ccb0562eb5 的重命名重构操作，不会将元素 [dbo].[Barrier].[Place] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 c9a2af9c-1086-41cc-863d-3aa7209c9534 的重命名重构操作，不会将元素 [dbo].[Barrier].[Log] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 e3d40d30-1775-4164-98de-e6c7f6389d1d 的重命名重构操作，不会将元素 [dbo].[Check].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 c8b747dd-ffeb-4de9-a322-8c44f10e2177 的重命名重构操作，不会将元素 [dbo].[Barrier].[CardNumer] (SqlSimpleColumn) 重命名为 CardNumber';


GO
PRINT N'已跳过具有键 cb424a93-77b2-4c88-9fcf-32aa06052311 的重命名重构操作，不会将元素 [dbo].[Check].[CardNumer] (SqlSimpleColumn) 重命名为 CardNumber';


GO
PRINT N'已跳过具有键 e47ae239-7847-4de9-9257-1650b02b8e99 的重命名重构操作，不会将元素 [dbo].[Wood].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 97714e85-0afd-43b1-9b6e-748a71329be5 的重命名重构操作，不会将元素 [dbo].[Wood].[Log] (SqlSimpleColumn) 重命名为 Version';


GO
PRINT N'已跳过具有键 5f5e355d-0248-4169-8349-6ff00fc01f9d 的重命名重构操作，不会将元素 [dbo].[FullPound].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 c066e2f2-9a7d-4eb5-95ab-7f730e0e7d51 的重命名重构操作，不会将元素 [dbo].[Recover].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 51d5fdf1-e39c-4783-ac5d-1246f8cb9395 的重命名重构操作，不会将元素 [dbo].[Recover].[CardNumber] (SqlSimpleColumn) 重命名为 RecoverTime';


GO
PRINT N'已跳过具有键 57f5e21d-56c6-4d25-a721-fe03e0de1dec 的重命名重构操作，不会将元素 [dbo].[Wood].[Origin] (SqlSimpleColumn) 重命名为 Area';


GO
PRINT N'已跳过具有键 b0fa3c90-5dc0-41d7-b45b-117b681e75eb 的重命名重构操作，不会将元素 [dbo].[Factory].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 e5215f86-9253-4299-80b5-792b39ce7e17 的重命名重构操作，不会将元素 [dbo].[EmptyPound].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 4a7fcba3-5e14-490b-964f-d49053884cf9 的重命名重构操作，不会将元素 [dbo].[PackBox].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 8ff19951-5658-4f4d-b2df-accd19108760 的重命名重构操作，不会将元素 [dbo].[WoodUnPackBox].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 fe6980a6-3a59-4c8f-be06-e510bea6734b 的重命名重构操作，不会将元素 [dbo].[WoodLaboratory].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 e99ab560-f68c-4f86-bdb5-f14030b22e42 的重命名重构操作，不会将元素 [dbo].[t_bang].[Id] (SqlSimpleColumn) 重命名为 bangid';


GO
PRINT N'已跳过具有键 41a97bfc-fe9d-4d84-820e-00818a76b8b8 的重命名重构操作，不会将元素 [dbo].[WoodJoin].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 f821ee65-8476-4789-9570-460474d12737 的重命名重构操作，不会将元素 [dbo].[WoodMachine].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 52f1f310-4506-4b0e-8123-7e28207a0363 的重命名重构操作，不会将元素 [dbo].[WoodPowerOfReadCard].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 021f9bc9-1f66-4642-a10c-ba0ff11f1cc8 的重命名重构操作，不会将元素 [dbo].[WoodGps].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 4b738c0e-77a6-45ba-95ba-22247b102f99 的重命名重构操作，不会将元素 [dbo].[WoodPowerOfGps].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 6a2e229e-8219-497a-a1a8-1320dc1f1857 的重命名重构操作，不会将元素 [dbo].[WoodCarPhoto].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 20de7da4-6edb-40f2-bead-5318b738ba15 的重命名重构操作，不会将元素 [dbo].[WoodTempPhoto].[Id] (SqlSimpleColumn) 重命名为 Photo';


GO
PRINT N'已跳过具有键 28350435-afd1-43bd-b343-70aa2afb0f0a 的重命名重构操作，不会将元素 [dbo].[WoodCard].[Id] (SqlSimpleColumn) 重命名为 Unique';


GO
PRINT N'已跳过具有键 5cac81a4-3405-4e3d-a1f9-b039009c263f 的重命名重构操作，不会将元素 [dbo].[WoodCard].[Number] (SqlSimpleColumn) 重命名为 CardNumber';


GO
PRINT N'正在删除 DF_FullPound_Printed...';


GO
ALTER TABLE [dbo].[FullPound] DROP CONSTRAINT [DF_FullPound_Printed];


GO
PRINT N'正在删除 DF_GsmItem_IsAdd...';


GO
ALTER TABLE [dbo].[GsmItem] DROP CONSTRAINT [DF_GsmItem_IsAdd];


GO
PRINT N'正在删除 DF__WoodLabor__Rebat__2CBDA3B5...';


GO
ALTER TABLE [dbo].[WoodLaboratory] DROP CONSTRAINT [DF__WoodLabor__Rebat__2CBDA3B5];


GO
PRINT N'正在改变 [dbo].[EmptyPound]...';


GO
ALTER TABLE [dbo].[EmptyPound] DROP COLUMN [LFDate], COLUMN [LFUnique];


GO
PRINT N'正在开始重新生成表 [dbo].[FullPound]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_FullPound] (
    [Unique]     INT             NOT NULL,
    [WoodID]     INT             NOT NULL,
    [FullVolume] DECIMAL (18, 6) NULL,
    [License]    NVARCHAR (50)   NOT NULL,
    [Area]       NVARCHAR (50)   NOT NULL,
    [Tree]       NVARCHAR (50)   NOT NULL,
    [Driver]     NVARCHAR (50)   NULL,
    [Supplier]   NVARCHAR (50)   NOT NULL,
    [CardNumber] NVARCHAR (50)   NOT NULL,
    [WeighTime]  DATETIME        NOT NULL,
    [Operator]   INT             NOT NULL,
    [State]      INT             NOT NULL,
    [Version]    INT             NOT NULL,
    [Log]        NVARCHAR (MAX)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[FullPound])
    BEGIN
        
        INSERT INTO [dbo].[tmp_ms_xx_FullPound] ([Unique], [Version], [WoodID], [FullVolume], [License], [Area], [Tree], [Driver], [Supplier], [CardNumber], [WeighTime], [Operator], [State], [Log])
        SELECT   [Unique],
                 [Version],
                 [WoodID],
                 [FullVolume],
                 [License],
                 [Area],
                 [Tree],
                 [Driver],
                 [Supplier],
                 [CardNumber],
                 [WeighTime],
                 [Operator],
                 [State],
                 [Log]
        FROM     [dbo].[FullPound]
        ORDER BY [Unique] ASC, [Version] ASC;
        
    END

DROP TABLE [dbo].[FullPound];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_FullPound]', N'FullPound';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'正在改变 [dbo].[GsmItem]...';


GO
ALTER TABLE [dbo].[GsmItem] DROP COLUMN [IsAdd];


GO
PRINT N'正在改变 [dbo].[GsmSupplier]...';


GO
ALTER TABLE [dbo].[GsmSupplier] DROP COLUMN [LinkMan], COLUMN [LinkPhone];


GO
PRINT N'正在改变 [dbo].[WoodLaboratory]...';


GO
ALTER TABLE [dbo].[WoodLaboratory] DROP COLUMN [RebateGreater];


GO
-- 正在重构步骤以使用已部署的事务日志更新目标服务器
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'a3997c11-3625-4bef-b62e-2dfaf8040212')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('a3997c11-3625-4bef-b62e-2dfaf8040212')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '585b8a99-0b2a-4dbf-acd1-5e07fc72d767')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('585b8a99-0b2a-4dbf-acd1-5e07fc72d767')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e5df9c4f-d607-42fd-9c6d-bd3a30e7b1e3')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e5df9c4f-d607-42fd-9c6d-bd3a30e7b1e3')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '1e020b72-92a1-458a-a380-4fd1d8fcae4b')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('1e020b72-92a1-458a-a380-4fd1d8fcae4b')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '4ef88f70-989a-4b5a-ba7c-e20eafd79b63')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('4ef88f70-989a-4b5a-ba7c-e20eafd79b63')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '96771660-329d-4f60-838b-1778a158068f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('96771660-329d-4f60-838b-1778a158068f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '7b15b6f5-ea10-49f6-bcd4-db78c5bcb1fd')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('7b15b6f5-ea10-49f6-bcd4-db78c5bcb1fd')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'bd826629-52b8-44f0-9735-338221a33ca0')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('bd826629-52b8-44f0-9735-338221a33ca0')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e5d49f19-374c-4d68-b153-5e9fd3849647')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e5d49f19-374c-4d68-b153-5e9fd3849647')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '66ae38f5-babd-48ec-bc68-1a206191b4fa')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('66ae38f5-babd-48ec-bc68-1a206191b4fa')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'b0a28293-a24f-4868-b937-8268fefc59ff')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('b0a28293-a24f-4868-b937-8268fefc59ff')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '277303f9-212f-4edd-8ee0-72ccb0562eb5')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('277303f9-212f-4edd-8ee0-72ccb0562eb5')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'c9a2af9c-1086-41cc-863d-3aa7209c9534')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('c9a2af9c-1086-41cc-863d-3aa7209c9534')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e3d40d30-1775-4164-98de-e6c7f6389d1d')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e3d40d30-1775-4164-98de-e6c7f6389d1d')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'c8b747dd-ffeb-4de9-a322-8c44f10e2177')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('c8b747dd-ffeb-4de9-a322-8c44f10e2177')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'cb424a93-77b2-4c88-9fcf-32aa06052311')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('cb424a93-77b2-4c88-9fcf-32aa06052311')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e47ae239-7847-4de9-9257-1650b02b8e99')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e47ae239-7847-4de9-9257-1650b02b8e99')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '0ba2448f-1f8a-44c8-8be5-73de3b12ff3c')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('0ba2448f-1f8a-44c8-8be5-73de3b12ff3c')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'b3ee4060-1849-4523-aae6-cb15d35dd284')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('b3ee4060-1849-4523-aae6-cb15d35dd284')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'c5c2929c-4660-4bab-9f52-80787e7e11bd')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('c5c2929c-4660-4bab-9f52-80787e7e11bd')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '97714e85-0afd-43b1-9b6e-748a71329be5')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('97714e85-0afd-43b1-9b6e-748a71329be5')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5f5e355d-0248-4169-8349-6ff00fc01f9d')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5f5e355d-0248-4169-8349-6ff00fc01f9d')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'c066e2f2-9a7d-4eb5-95ab-7f730e0e7d51')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('c066e2f2-9a7d-4eb5-95ab-7f730e0e7d51')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '51d5fdf1-e39c-4783-ac5d-1246f8cb9395')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('51d5fdf1-e39c-4783-ac5d-1246f8cb9395')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '57f5e21d-56c6-4d25-a721-fe03e0de1dec')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('57f5e21d-56c6-4d25-a721-fe03e0de1dec')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'b0fa3c90-5dc0-41d7-b45b-117b681e75eb')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('b0fa3c90-5dc0-41d7-b45b-117b681e75eb')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e5215f86-9253-4299-80b5-792b39ce7e17')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e5215f86-9253-4299-80b5-792b39ce7e17')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '4a7fcba3-5e14-490b-964f-d49053884cf9')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('4a7fcba3-5e14-490b-964f-d49053884cf9')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '8ff19951-5658-4f4d-b2df-accd19108760')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('8ff19951-5658-4f4d-b2df-accd19108760')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'fe6980a6-3a59-4c8f-be06-e510bea6734b')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('fe6980a6-3a59-4c8f-be06-e510bea6734b')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e99ab560-f68c-4f86-bdb5-f14030b22e42')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e99ab560-f68c-4f86-bdb5-f14030b22e42')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '41a97bfc-fe9d-4d84-820e-00818a76b8b8')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('41a97bfc-fe9d-4d84-820e-00818a76b8b8')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'f821ee65-8476-4789-9570-460474d12737')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('f821ee65-8476-4789-9570-460474d12737')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '52f1f310-4506-4b0e-8123-7e28207a0363')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('52f1f310-4506-4b0e-8123-7e28207a0363')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '021f9bc9-1f66-4642-a10c-ba0ff11f1cc8')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('021f9bc9-1f66-4642-a10c-ba0ff11f1cc8')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '4b738c0e-77a6-45ba-95ba-22247b102f99')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('4b738c0e-77a6-45ba-95ba-22247b102f99')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6a2e229e-8219-497a-a1a8-1320dc1f1857')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6a2e229e-8219-497a-a1a8-1320dc1f1857')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '20de7da4-6edb-40f2-bead-5318b738ba15')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('20de7da4-6edb-40f2-bead-5318b738ba15')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '28350435-afd1-43bd-b343-70aa2afb0f0a')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('28350435-afd1-43bd-b343-70aa2afb0f0a')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5cac81a4-3405-4e3d-a1f9-b039009c263f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5cac81a4-3405-4e3d-a1f9-b039009c263f')

GO

GO
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Account', 11)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Act', 16)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Function', 16)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Grant', 18)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmArea', 15)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmLine', 23)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmOrigin', 59)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmSupplier', 19)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('GsmTree', 3)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Link', 3)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Role', 4)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Service', 19)
INSERT INTO [Unique] ([Name], [Value]) VALUES ('Verificate', 13)

INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(1, 'zdq', '{"Department":"慧云公司","Name":"张德强","Mobile":"13570067306","Email":"zdq@tcloudit.com"}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(2, 'sam', '{"Department":"丰林集团/集团领导","Name":"刘念","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(3, 'dyz', '{"Department":"丰林集团/集团领导","Name":"段英志","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(4, 'lgl', '{"Department":"丰林集团/集团领导","Name":"林国利","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(5, 'zwz', '{"Department":"百色丰林/公司领导","Name":"张文治","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(6, 'lhg', '{"Department":"百色丰林/财务部","Name":"李红刚","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(7, 'lmt', '{"Department":"百色丰林/木材采购部","Name":"隆满天","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(8, 'wf', '{"Department":"百色丰林/木材采购部","Name":"伍帆","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(9, 'hxx', '{"Department":"百色丰林/木材采购部","Name":"黄秀香","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(10, 'cxj', '{"Department":"百色丰林/财务部","Name":"陈小娟","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Account]([Unique], [Id], [Description], [Remark], [Status], [State], [Version], [Operator], [Log])
VALUES(11, 'dyc', '{"Department":"丰林集团/物流部","Name":"邓毓超","Mobile":"","Email":""}', NULL, 0, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(1, 1, 1, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(2, 1, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(3, 1, 3, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(4, 1, 4, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(5, 2, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(6, 3, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(7, 4, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(8, 5, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(9, 6, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(10, 7, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(11, 8, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(12, 9, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(13, 10, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(14, 11, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(15, 11, 3, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Act]([Unique], [Account], [Role], [State], [Version], [Operator], [Log]) VALUES(16, 11, 4, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(1, NULL, 0, 1, '短信报备', '短信报备', 'http://{0}/Gsm.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(2, NULL, 0, 2, '系统管理', '系统管理', 'http://{0}/Setting.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(3, 1, 1, 3, 'GsmArea', '属地管理', 'http://{0}:15001/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(4, 1, 1, 5, 'GsmLine', '行驶路线管理', 'http://{0}:15003/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(5, 1, 0, 1, 'GsmMessage', '短信管理', 'http://{0}:15006/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(6, 1, 1, 4, 'GsmOrigin', '产地管理', 'http://{0}:15002/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(7, 1, 0, 2, 'GsmReport', '查看报表', 'http://{0}:15007/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(8, 1, 1, 6, 'GsmSupplier', '供应商管理', 'http://{0}:15004/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(9, 1, 1, 7, 'GsmTree', '树种管理', 'http://{0}:15005/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(10, 2, 2, 1, 'Account', '账户管理', 'http://{0}:13003/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(11, 2, 2, 2, 'Act', '账户角色', 'http://{0}:13007/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(12, 2, 2, 6, 'Function', '功能管理', 'http://{0}:13005/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(13, 2, 2, 5, 'Grant', '角色授权', 'http://{0}:13006/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(14, 2, 2, 7, 'Link', '链接管理', 'http://{0}:13001/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(15, 2, 2, 4, 'Role', '角色管理', 'http://{0}:13002/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Function]([Unique], [Parent], [Type], [Order], [No], [Name], [Url], [Remark], [State], [Version], [Operator], [Log])
VALUES(16, 2, 2, 3, 'Verificate', '认证管理', 'http://{0}:13004/Index.aspx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(1, 1, 2, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(2, 1, 10, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(3, 1, 11, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(4, 1, 12, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(5, 1, 13, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(6, 1, 14, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(7, 1, 15, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(8, 1, 16, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(9, 2, 1, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(10, 2, 7, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(11, 3, 1, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(12, 3, 5, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(13, 4, 1, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(14, 4, 3, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(15, 4, 4, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(16, 4, 6, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(17, 4, 8, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Grant]([Unique], [Role], [Function], [State], [Version], [Operator], [Log]) VALUES(18, 4, 9, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (1, '南宁市', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (2, '武鸣县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (3, '田东县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (4, '隆安县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (5, '云南省', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (6, '河池市', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (7, '田林县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (8, '平果县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (9, '德保县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (10, '凌云县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (11, '东兰县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (12, '那坡县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (13, '靖西县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (14, '马山县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmArea] ([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (15, '扶绥县', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (1, '者桑', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (2, '博爱', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (3, '阳圩', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (4, '高邦', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (5, '皈朝', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (6, '平年', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (7, '潞城', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (8, '旧州', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (9, '八渡', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (10, '板桃', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (11, '凌云', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (12, '德保', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (13, '祥周', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (14, '田东', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (15, '田阳', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (16, '巴马', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (17, '石埠', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (18, '坛洛', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (19, '隆安', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (20, '平果', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (21, '灵马', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (22, '马山', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmLine] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (23, '扶绥', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (1, 1, '南宁市金陵木片厂', '金陵', '广丰; 鸿华; 黎森', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (2, 1, '南宁金陵广丰木业', '广丰', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (3, 1, '南宁市金陵镇鸿华木片厂', '鸿华', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (4, 1, '南宁市金陵黎森木业', '黎森', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (5, 1, '南宁扶照兴东木片厂', '扶照; 兴东', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (6, 1, '南宁市金凯木片厂', '金凯', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (7, 1, '南宁市恒桂木材加工厂', '恒桂', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (8, 1, '南宁苏圩巨力木片厂', '巨力', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (9, 1, '南宁苏圩隆达木业', '隆达; 隆德', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (10, 1, '南宁市宏翔木片厂', '宏翔', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (11, 1, '南宁吴圩', '吴圩', '周德', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (12, 1, '吴圩周德木业', '周德', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (13, 1, '南宁那马林进木片厂', '那马; 林进', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (14, 1, '南宁江西镇', '江西', '顾林', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (15, 1, '南宁江西顾林木业', '顾林', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (16, 1, '南宁明阳众森木业', '明阳; 众森', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (17, 1, '南宁市坛洛厂', '坛洛', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (18, 2, '武鸣团结农场', '团结', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (19, 2, '武鸣灵马木片厂', '灵马', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (20, 2, '武鸣陆斡镇', '陆斡', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (21, 2, '武鸣罗圩木片厂', '罗圩; 锣圩', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (22, 2, '武鸣南益木业', '南益', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (23, 3, '田东木片厂', '田东', '怀民; 城南; 金城; 金成; 小龙; 苗圃', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (24, 3, '田东县怀民木片厂', '怀民', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (25, 3, '田东县城南木片厂', '城南', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (26, 3, '田东县金城木片厂', '金城; 金成', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (27, 3, '田东县小龙苗圃木片厂', '小龙; 苗圃', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (28, 3, '田东县', '田东', '木片', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (29, 4, '隆安木片加工厂', '隆安', '那桐; 丰顺; 屏山', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (30, 4, '隆安那桐木片厂', '那桐', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (31, 4, '隆安县丰顺木材厂', '丰顺', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (32, 4, '隆安屏山', '屏山', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (33, 5, '云南', '云南', '富宁; 者桑; 红森; 文山; 通轩', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (34, 5, '云南富宁木片厂', '富宁', '高邦; 皈朝; 海丰; 花果山', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (35, 5, '富宁高邦木片厂', '高邦', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (36, 5, '云南富宁皈朝海丰木业', '皈朝; 海丰', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (37, 5, '富宁县花果山木材加工厂', '花果山', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (38, 5, '云南者桑红森', '者桑; 红森', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (39, 6, '大化都阳木片厂', '都阳、江南', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (40, 6, '大化县乙圩乡木片厂', '乙圩', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (41, 6, '大化县北景乡百益木片厂', '北景; 百益', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (42, 6, '大化县羌圩木片厂', '羌圩、姜圩', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (43, 6, '大化和顺木材加工厂', '和顺', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (44, 6, '大化毅力木片厂', '毅力', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (45, 7, '田林潞城富林木业', '潞城; 富林', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (46, 7, '田林县旧州加工厂', '旧州', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (47, 8, '平果木片厂', '平果', '旧城; 林苑', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (48, 8, '平果旧城', '旧城', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (49, 8, '平果县', '平果', '旧城; 林苑; 木片', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (50, 8, '平果林苑木片厂', '林苑', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (51, 9, '广西德保红泥坡林场', '德保; 红泥', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (52, 10, '凌云友良木材厂', '凌云; 友良', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (53, 11, '东兰县东森木材加工厂', '东兰; 东森', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (54, 12, '那坡县木材公司', '那坡', '德隆', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (55, 12, '那坡县德隆木片厂', '德隆', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (56, 13, '靖西县 ', '靖西', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (57, 14, '马山县', '马山', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (58, 15, '扶绥渠黎', '扶绥; 渠黎', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmOrigin] ([Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]) VALUES (59, 5, '云南文山通轩木业', '文山; 通轩', '', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (1, 0, 'A2', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (2, 0, 'A7', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (3, 0, 'A8', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (4, 0, 'A9', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (5, 0, 'A11', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (6, 0, 'B9', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (7, 0, 'C3', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (8, 0, 'C5', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (9, 0, 'D1', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (10, 0, 'D2', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (11, 0, 'E3', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (12, 0, 'K8', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (13, 0, 'M1', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (14, 0, 'P9', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (15, 1, '庞坤耀', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (16, 1, '覃良山', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (17, 1, '冉景尤', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (18, 0, 'C2', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmSupplier] ([Unique], [Type], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES (19, 0, 'C9', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [GsmTree] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (1, '桉木片', '桉木; 桉片; 桉树', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmTree] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (2, '松木片', '松木; 松片; 松树', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [GsmTree] ([Unique], [Name], [Alias], [Remark], [State], [Version], [Operator], [Log]) VALUES (3, '杂木片', '杂木; 杂片; 杂树; 桉杂', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Link]([Unique], [Name], [Type], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(1, '本系统', 0, NULL, NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Link]([Unique], [Name], [Type], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(2, '丰林域系统', 2, '172.16.1.250', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Link]([Unique], [Name], [Type], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(3, '慧云域系统', 2, '172.16.200.250', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES(1, '系统管理管理者', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES(2, '短信报备查看者', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES(3, '短信报备操作者', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Role]([Unique], [Name], [Remark], [State], [Version], [Operator], [Log]) VALUES(4, '短信报备管理者', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(1, 'Service', '{0}\Wodeyun.Bf\Wodeyun.Bf.Esb\Wodeyun.Bf.Esb.Wrappers\bin\Debug\Wodeyun.Bf.Esb.Wrappers.dll', 'http://{0}:11001/ServiceService.svc', 'Wodeyun.Bf.Esb.Wrappers.IServiceInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(2, 'Exchange.Single', '{0}\Wodeyun.Bf\Wodeyun.Bf.Exchange\Wodeyun.Bf.Exchange.Wrappers\bin\Debug\Wodeyun.Bf.Exchange.Wrappers.dll', 'http://{0}:11002/SingleService.svc', 'Wodeyun.Bf.Exchange.Wrappers.SingleService', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(3, 'Exchange.Multi', '{0}\Wodeyun.Bf\Wodeyun.Bf.Exchange\Wodeyun.Bf.Exchange.Wrappers\bin\Debug\Wodeyun.Bf.Exchange.Wrappers.dll', 'http://{0}:11002/MultiService.svc', 'Wodeyun.Bf.Exchange.Wrappers.MultiService', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(4, 'Token', '{0}\Wodeyun.Bf\Wodeyun.Bf.Token\Wodeyun.Bf.Token.Wrappers\bin\Debug\Wodeyun.Bf.Token.Wrappers.dll', 'http://{0}:11003/TokenService.svc', 'Wodeyun.Bf.Token.Wrappers.TokenService', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(5, 'Link', '{0}\Wodeyun.Bf\Wodeyun.Bf.Link\Wodeyun.Bf.Link.Wrappers\bin\Debug\Wodeyun.Bf.Link.Wrappers.dll', 'http://{0}:12001/LinkService.svc', 'Wodeyun.Bf.Link.Wrappers.ILinkInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(6, 'Role', '{0}\Wodeyun.Bf\Wodeyun.Bf.Role\Wodeyun.Bf.Role.Wrappers\bin\Debug\Wodeyun.Bf.Role.Wrappers.dll', 'http://{0}:12002/RoleService.svc', 'Wodeyun.Bf.Role.Wrappers.IRoleInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(7, 'Account', '{0}\Wodeyun.Bf\Wodeyun.Bf.Account\Wodeyun.Bf.Account.Wrappers\bin\Debug\Wodeyun.Bf.Account.Wrappers.dll', 'http://{0}:12003/AccountService.svc', 'Wodeyun.Bf.Account.Wrappers.IAccountInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(8, 'Verificate', '{0}\Wodeyun.Bf\Wodeyun.Bf.Verificate\Wodeyun.Bf.Verificate.Wrappers\bin\Debug\Wodeyun.Bf.Verificate.Wrappers.dll', 'http://{0}:12004/VerificateService.svc', 'Wodeyun.Bf.Verificate.Wrappers.IVerificateInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(9, 'Function', '{0}\Wodeyun.Bf\Wodeyun.Bf.Function\Wodeyun.Bf.Function.Wrappers\bin\Debug\Wodeyun.Bf.Function.Wrappers.dll', 'http://{0}:12005/FunctionService.svc', 'Wodeyun.Bf.Function.Wrappers.IFunctionInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(10, 'Grant', '{0}\Wodeyun.Bf\Wodeyun.Bf.Grant\Wodeyun.Bf.Grant.Wrappers\bin\Debug\Wodeyun.Bf.Grant.Wrappers.dll', 'http://{0}:12006/GrantService.svc', 'Wodeyun.Bf.Grant.Wrappers.IGrantInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(11, 'Act', '{0}\Wodeyun.Bf\Wodeyun.Bf.Act\Wodeyun.Bf.Act.Wrappers\bin\Debug\Wodeyun.Bf.Act.Wrappers.dll', 'http://{0}:12007/ActService.svc', 'Wodeyun.Bf.Act.Wrappers.IActInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(12, 'GsmArea', '{0}\Wodeyun.Project\Wodeyun.Project.GsmArea\Wodeyun.Project.GsmArea.Wrappers\bin\Debug\Wodeyun.Project.GsmArea.Wrappers.dll', 'http://{0}:14001/GsmAreaService.svc', 'Wodeyun.Project.GsmArea.Wrappers.IGsmAreaInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(13, 'GsmOrigin', '{0}\Wodeyun.Project\Wodeyun.Project.GsmOrigin\Wodeyun.Project.GsmOrigin.Wrappers\bin\Debug\Wodeyun.Project.GsmOrigin.Wrappers.dll', 'http://{0}:14002/GsmOriginService.svc', 'Wodeyun.Project.GsmOrigin.Wrappers.IGsmOriginInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(14, 'GsmLine', '{0}\Wodeyun.Project\Wodeyun.Project.GsmLine\Wodeyun.Project.GsmLine.Wrappers\bin\Debug\Wodeyun.Project.GsmLine.Wrappers.dll', 'http://{0}:14003/GsmLineService.svc', 'Wodeyun.Project.GsmLine.Wrappers.IGsmLineInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(15, 'GsmSupplier', '{0}\Wodeyun.Project\Wodeyun.Project.GsmSupplier\Wodeyun.Project.GsmSupplier.Wrappers\bin\Debug\Wodeyun.Project.GsmSupplier.Wrappers.dll', 'http://{0}:14004/GsmSupplierService.svc', 'Wodeyun.Project.GsmSupplier.Wrappers.IGsmSupplierInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(16, 'GsmTree', '{0}\Wodeyun.Project\Wodeyun.Project.GsmTree\Wodeyun.Project.GsmTree.Wrappers\bin\Debug\Wodeyun.Project.GsmTree.Wrappers.dll', 'http://{0}:14005/GsmTreeService.svc', 'Wodeyun.Project.GsmTree.Wrappers.IGsmTreeInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(17, 'GsmMessage', '{0}\Wodeyun.Project\Wodeyun.Project.GsmMessage\Wodeyun.Project.GsmMessage.Wrappers\bin\Debug\Wodeyun.Project.GsmMessage.Wrappers.dll', 'http://{0}:14006/GsmMessageService.svc', 'Wodeyun.Project.GsmMessage.Wrappers.IGsmMessageInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(18, 'GsmItem', '{0}\Wodeyun.Project\Wodeyun.Project.GsmMessage\Wodeyun.Project.GsmMessage.Wrappers\bin\Debug\Wodeyun.Project.GsmMessage.Wrappers.dll', 'http://{0}:14006/GsmItemService.svc', 'Wodeyun.Project.GsmMessage.Wrappers.IGsmItemInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Service]([Unique], [Name], [Filename], [Url], [Contract], [Remark], [State], [Version], [Operator], [Log])
VALUES(19, 'GsmReport', '{0}\Wodeyun.Project\Wodeyun.Project.GsmReport\Wodeyun.Project.GsmReport.Wrappers\bin\Debug\Wodeyun.Project.GsmReport.Wrappers.dll', 'http://{0}:14007/GsmReportService.svc', 'Wodeyun.Project.GsmReport.Wrappers.IGsmReportInterface', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(1, 1, 1, 'zdq', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(2, 1, 2, 'zdq', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(3, 1, 3, 'zdq', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')

INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(4, 2, 2, 'sam', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(5, 3, 2, 'dyz', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(6, 4, 2, 'lgl', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(7, 5, 2, 'zwz', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(8, 6, 2, 'lhg', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(9, 7, 2, 'lmt', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(10, 8, 2, 'wf', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(11, 9, 2, 'hxx', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(12, 10, 2, 'cxj', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
INSERT INTO [Verificate]([Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]) VALUES(13, 11, 2, 'dyc', NULL, 0, 1, 1, '{"Date":"2013-04-01 00:00:00"}')
GO

GO
PRINT N'更新完成。'
GO
