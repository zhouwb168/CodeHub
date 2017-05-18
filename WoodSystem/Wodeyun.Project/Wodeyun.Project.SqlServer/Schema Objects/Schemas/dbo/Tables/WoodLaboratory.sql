CREATE TABLE [dbo].[WoodLaboratory] (
    [Unique]        INT            NOT NULL,
    [WoodID]        INT            NOT NULL,
    [Water]         DECIMAL (5, 2) NOT NULL,
    [RebateWater]   DECIMAL (5, 2) NOT NULL,
    [Skin]          DECIMAL (5, 2) NOT NULL,
    [RebateSkin]    DECIMAL (5, 2) NOT NULL,
    [Scrap]         DECIMAL (5, 2) NOT NULL,
    [RebateScrap]   DECIMAL (5, 2) NOT NULL,
    [Bad]           DECIMAL (5, 2) NULL,
    [Greater]       DECIMAL (5, 2) NULL,
    [Less]          DECIMAL (5, 2) NULL,
    [CheckTime]     DATETIME       NOT NULL,
    [Operator]      INT            NOT NULL,
    [IsConfirmed]   BIT            DEFAULT ((0)) NOT NULL,
    [Confirmor]     INT            NULL,
    [ConfirmTime]   DATETIME       NULL,
    [State]         INT            NOT NULL,
    [Version]       INT            NOT NULL,
    [Log]           NVARCHAR (MAX) NOT NULL,
    [RebateGreater] DECIMAL (5, 2) DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


