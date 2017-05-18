CREATE TABLE [dbo].[Wood]
(
	[Unique] INT NOT NULL , 
    [BarrierID] INT NULL, 
    [CardNumber] NVARCHAR(50) NOT NULL, 
    [ArriveDate] DATETIME NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
