CREATE TABLE [dbo].[Grant]
(
	[Unique]   INT           NOT NULL,
	[Role]     INT           NOT NULL,
	[Function] INT           NOT NULL,
	[State]    INT           NOT NULL,
	[Version]  INT           NOT NULL,
	[Operator] INT           NOT NULL,
	[Log]      NVARCHAR(MAX) NOT NULL
)