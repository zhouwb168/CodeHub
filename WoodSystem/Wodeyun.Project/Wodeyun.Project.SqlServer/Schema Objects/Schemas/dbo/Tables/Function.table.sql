CREATE TABLE [dbo].[Function]
(
	[Unique]   INT           NOT NULL,
	[Parent]   INT           NULL,
	[Type]     INT           NOT NULL,
	[Order]    INT           NOT NULL,
	[No]       NVARCHAR(50)  NOT NULL,
	[Name]     NVARCHAR(50)  NOT NULL,
	[Url]      NVARCHAR(255) NOT NULL,
	[Remark]   NVARCHAR(MAX) NULL,
	[State]    INT           NOT NULL,
	[Version]  INT           NOT NULL,
	[Operator] INT           NOT NULL,
	[Log]      NVARCHAR(MAX) NOT NULL
)