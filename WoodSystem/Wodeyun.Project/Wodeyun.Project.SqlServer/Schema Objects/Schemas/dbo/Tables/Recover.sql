CREATE TABLE [dbo].[Recover]
(
	[Unique] INT NOT NULL , 
    [WoodID] INT NOT NULL, 
    [RecoverTime] DATETIME NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
