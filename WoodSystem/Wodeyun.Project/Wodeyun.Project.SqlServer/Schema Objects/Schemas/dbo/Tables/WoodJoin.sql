CREATE TABLE [dbo].[WoodJoin]
(
	[Unique] INT NOT NULL , 
    [BangID] INT NOT NULL, 
    [GsmID] INT NULL, 
    [WoodID] INT NOT NULL, 
    [IsRebate] BIT NOT NULL DEFAULT 0, 
    [IsGsm] BIT NOT NULL DEFAULT 0, 
    [JoinTime] DATETIME NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_WoodJoin] PRIMARY KEY ([Unique], [Version]) 
)
