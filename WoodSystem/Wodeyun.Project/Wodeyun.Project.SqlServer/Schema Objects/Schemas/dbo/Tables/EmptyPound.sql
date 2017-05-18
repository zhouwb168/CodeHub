CREATE TABLE [dbo].[EmptyPound] (
    [Unique]        INT             NOT NULL,
    [WoodID]        INT             NOT NULL,
    [EmptyVolume]   DECIMAL (18, 6) NULL,
    [HandVolume]    DECIMAL (18, 6) NULL,
    [BackWeighTime] DATETIME        NOT NULL,
    [Operator]      INT             NOT NULL,
    [State]         INT             NOT NULL,
    [Version]       INT             NOT NULL,
    [Log]           NVARCHAR (MAX)  NOT NULL,
    [LFUnique]      INT             NULL,
    [LFDate]        DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);


