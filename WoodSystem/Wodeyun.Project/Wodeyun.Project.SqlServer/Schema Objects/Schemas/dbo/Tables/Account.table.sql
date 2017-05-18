CREATE TABLE [dbo].[Account]
(
	[Unique]      INT           NOT NULL,
	[Id]          NVARCHAR(50)  NOT NULL,
	[Description] NVARCHAR(MAX) NULL,
	[Remark]      NVARCHAR(MAX) NULL,
	[Status]      INT           NOT NULL,
	[State]       INT           NOT NULL,
	[Version]     INT           NOT NULL,
	[Operator]    INT           NOT NULL,
	[Log]         NVARCHAR(MAX) NOT NULL
)