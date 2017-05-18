CREATE TABLE [dbo].[Factory]
(
	[Unique] INT NOT NULL , 
    [WoodID] INT NOT NULL, 
    [UnLoadPalce] NVARCHAR(50) NOT NULL, 
    [UnLoadPeople] NVARCHAR(50) NOT NULL, 
    [Key] NVARCHAR(50) NOT NULL, 
    [Sampler] NVARCHAR(50) NOT NULL, 
    [Water] DECIMAL(5, 2) NOT NULL, 
    [Skin] DECIMAL(5, 2) NOT NULL, 
    [Scrap] DECIMAL(5, 2) NOT NULL, 
    [Deduct] NVARCHAR(MAX) NULL, 
    [Remark] NVARCHAR(MAX) NULL, 
    [SampleTime] DATETIME NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Version], [Unique])
)
