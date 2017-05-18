CREATE TABLE [dbo].[FullPound] (
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
    [Printed]    BIT             CONSTRAINT [DF_FullPound_Printed] DEFAULT ((0)) NOT NULL,
    [Operator]   INT             NOT NULL,
    [State]      INT             NOT NULL,
    [Version]    INT             NOT NULL,
    [Log]        NVARCHAR (MAX)  NOT NULL,
    [LFUnique]   INT             NULL,
    [LFDate]     DATETIME        NULL,
    CONSTRAINT [PK__FullPoun__04CEE9DD876608F2] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


