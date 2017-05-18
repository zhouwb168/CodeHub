CREATE TABLE [dbo].[WoodCard]
(
	[Unique] INT NOT NULL , 
    [CID] NVARCHAR(50) NOT NULL, 
    [CardNumber] NVARCHAR(50) NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
