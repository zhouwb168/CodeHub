CREATE TABLE [dbo].[Check]
(
	[Unique] INT NOT NULL , 
    [BarrierID] INT NOT NULL, 
    [CheckDate] DATETIME NOT NULL , 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
