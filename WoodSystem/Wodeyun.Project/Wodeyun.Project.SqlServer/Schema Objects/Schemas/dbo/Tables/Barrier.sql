CREATE TABLE [dbo].[Barrier]
(
    [Unique] INT NOT NULL, 
    [Place] NVARCHAR(50) NOT NULL, 
    [CardNumber] NVARCHAR(50) NOT NULL, 
    [License] NVARCHAR(50) NOT NULL, 
    [Area] NVARCHAR(50) NOT NULL, 
    [TimeOfStation] DATETIME NOT NULL , 
    [GPS] DECIMAL(18, 6) NOT NULL, 
    [Operator] INT NOT NULL, 
    [State] INT NOT NULL, 
    [Version] INT NOT NULL, 
    [Log] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_Barriers] PRIMARY KEY ([Version], [Unique]) 
)
