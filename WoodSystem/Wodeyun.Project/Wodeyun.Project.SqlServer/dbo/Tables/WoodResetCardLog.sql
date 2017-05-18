CREATE TABLE [dbo].[WoodResetCardLog] (
    [Unique]     INT            NOT NULL,
    [CardNumber] NVARCHAR (50)  NOT NULL,
    [CardType]   INT            NOT NULL,
    [CardState]  INT            NOT NULL,
    [RecordId]   INT            NOT NULL,
    [ComeFrom]   INT            NOT NULL,
    [AccountID]  INT            NOT NULL,
    [ResetTime]  DATETIME       NOT NULL,
    [Remark]     NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Unique] ASC)
);

