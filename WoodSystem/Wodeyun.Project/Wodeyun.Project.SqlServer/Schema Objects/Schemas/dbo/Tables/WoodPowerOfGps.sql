CREATE TABLE [dbo].[WoodPowerOfGps]
(
	[Unique] INT NOT NULL , 
    [AccountID] INT NOT NULL, 
    [GpsID] INT NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
