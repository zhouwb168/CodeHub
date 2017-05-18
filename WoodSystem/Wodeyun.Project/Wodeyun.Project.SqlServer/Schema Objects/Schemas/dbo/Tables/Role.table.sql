CREATE TABLE [dbo].[Role]
(
	[Unique]   INT           NOT NULL,
	[Name]     NVARCHAR(50)  NOT NULL,
	[Remark]   NVARCHAR(MAX) NULL,
	[State]    INT           NOT NULL,
	[Version]  INT           NOT NULL,
	[Operator] INT           NOT NULL,
	[Log]      NVARCHAR(MAX) NOT NULL
)