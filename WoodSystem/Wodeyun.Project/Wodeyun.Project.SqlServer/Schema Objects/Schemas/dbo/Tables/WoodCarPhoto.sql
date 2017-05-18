CREATE TABLE [dbo].[WoodCarPhoto]
(
	[Unique] INT NOT NULL , 
    [BarrierID] INT NOT NULL, 
    [GPS] DECIMAL(18, 6) NOT NULL, 
    [Photo] VARBINARY(MAX) NOT NULL, 
    [PhotoTime] DATETIME NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
