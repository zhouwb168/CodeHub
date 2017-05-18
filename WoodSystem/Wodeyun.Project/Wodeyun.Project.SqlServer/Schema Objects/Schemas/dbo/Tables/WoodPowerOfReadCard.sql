CREATE TABLE [dbo].[WoodPowerOfReadCard]
(
	[Unique] INT NOT NULL , 
    [AccountID] INT NOT NULL, 
    [MachineID] INT NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
