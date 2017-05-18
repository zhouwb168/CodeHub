CREATE TABLE [dbo].[GsmJoin] (
    [Unique]   INT            NOT NULL,
    [BangID]   INT            NOT NULL,
    [GsmID]    INT            NULL,
    [WoodID]   INT            NOT NULL,
    [IsAdd]    BIT            DEFAULT ((0)) NOT NULL,
    [IsGsm]    BIT            DEFAULT ((0)) NOT NULL,
    [JoinTime] DATETIME       NOT NULL,
    [Operator] INT            NOT NULL,
    [State]    INT            NOT NULL,
    [Version]  INT            NOT NULL,
    [Log]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_GsmJoin] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

