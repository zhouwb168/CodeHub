CREATE TABLE [dbo].[Service]
(
	[Unique]   INT           NOT NULL,
	[Name]     NVARCHAR(50)  NOT NULL,
	[Filename] NVARCHAR(255) NOT NULL,
	[Url]      NVARCHAR(255) NOT NULL,
	[Contract] NVARCHAR(100) NOT NULL,
	[Remark]   NVARCHAR(MAX) NULL,
	[State]    INT           NOT NULL,
	[Version]  INT           NOT NULL,
	[Operator] INT           NOT NULL,
	[Log]      NVARCHAR(MAX) NOT NULL
)