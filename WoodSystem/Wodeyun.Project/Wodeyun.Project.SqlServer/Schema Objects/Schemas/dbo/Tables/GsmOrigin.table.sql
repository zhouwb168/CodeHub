CREATE TABLE [dbo].[GsmOrigin]
(
	[Unique]   INT           NOT NULL,
	[Area]     INT           NOT NULL,
	[Name]     NVARCHAR(50)  NOT NULL,
	[Alias]    NVARCHAR(255) NOT NULL,
	[Except]   NVARCHAR(255) NOT NULL,
	[Remark]   NVARCHAR(MAX) NULL,
	[State]    INT           NOT NULL,
	[Version]  INT           NOT NULL,
	[Operator] INT           NOT NULL,
	[Log]      NVARCHAR(MAX) NOT NULL
)