CREATE TABLE [dbo].[WoodPackBox]
(
	[Unique] INT NOT NULL , 
    [WoodID] INT NOT NULL, 
    [Box] INT NOT NULL, 
    [PackTime] DATETIME NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Version], [Unique])
)
