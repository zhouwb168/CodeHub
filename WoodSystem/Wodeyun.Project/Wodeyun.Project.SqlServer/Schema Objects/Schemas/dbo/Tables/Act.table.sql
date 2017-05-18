CREATE TABLE [dbo].[Act]
(
	[Unique]   INT           NOT NULL,
	[Account]  INT           NOT NULL,
	[Role]     INT           NOT NULL,
	[State]    INT           NOT NULL,
	[Version]  INT           NOT NULL,
	[Operator] INT           NOT NULL,
	[Log]      NVARCHAR(MAX) NOT NULL
)