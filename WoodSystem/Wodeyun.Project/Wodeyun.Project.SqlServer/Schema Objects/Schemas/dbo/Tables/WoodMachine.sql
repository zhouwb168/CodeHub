CREATE TABLE [dbo].[WoodMachine]
(
	[Unique] INT NOT NULL , 
    [Name] NVARCHAR(50) NOT NULL, 
    [MachineNumber] NVARCHAR(500) NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
