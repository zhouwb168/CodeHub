CREATE TABLE [dbo].[WoodGps]
(
	[Unique] INT NOT NULL , 
    [StationName] NVARCHAR(50) NOT NULL, 
    [Lat] DECIMAL(18, 8) NOT NULL, 
    [Lng] DECIMAL(18, 8) NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Unique], [Version])
)
