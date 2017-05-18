CREATE TABLE [dbo].[Verificate]
(
	[Unique]   INT           NOT NULL,
	[Account]  INT           NOT NULL,
	[Link]     INT           NOT NULL,
	[Value]    NVARCHAR(50)  NOT NULL,
	[Remark]   NVARCHAR(MAX) NULL,
	[State]    INT           NOT NULL,
	[Version]  INT           NOT NULL,
	[Operator] INT           NOT NULL,
	[Log]      NVARCHAR(MAX) NOT NULL
)