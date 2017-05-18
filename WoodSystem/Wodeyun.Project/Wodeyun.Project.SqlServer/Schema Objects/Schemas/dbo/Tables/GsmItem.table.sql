CREATE TABLE [dbo].[GsmItem] (
    [Unique]   INT            NOT NULL,
    [Message]  INT            NOT NULL,
    [Supplier] NVARCHAR (50)  NOT NULL,
    [Tree]     NVARCHAR (50)  NOT NULL,
    [Make]     DATETIME       NOT NULL,
    [Area]     NVARCHAR (50)  NOT NULL,
    [Origin]   NVARCHAR (50)  NOT NULL,
    [License]  NVARCHAR (50)  NOT NULL,
    [Driver]   NVARCHAR (50)  NOT NULL,
    [Ship]     DATETIME       NOT NULL,
    [Line]     NVARCHAR (50)  NOT NULL,
    [Remark]   NVARCHAR (MAX) NULL,
    [IsAdd]    BIT            CONSTRAINT [DF_GsmItem_IsAdd] DEFAULT ((0)) NOT NULL,
    [State]    INT            NOT NULL,
    [Version]  INT            NOT NULL,
    [Operator] INT            NOT NULL,
    [Log]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_GsmItem] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

