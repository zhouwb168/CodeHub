CREATE TABLE [dbo].[WoodCardState] (
    [CardNumber] NVARCHAR (50) NOT NULL,
    [CardType]   INT           NOT NULL,
    [CardState]  INT           NOT NULL,
    [RecordId]   INT           NOT NULL,
    [ComeFrom]   INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([CardNumber] ASC, [CardType] ASC)
);

