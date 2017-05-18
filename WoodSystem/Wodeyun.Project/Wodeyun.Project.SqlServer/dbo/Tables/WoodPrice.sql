CREATE TABLE [dbo].[WoodPrice] (
    [Unique]    INT             NOT NULL,
    [AreaID]    INT             NOT NULL,
    [TreeID]    INT             NOT NULL,
    [Price]     DECIMAL (10, 2) NOT NULL,
    [Unit]      NVARCHAR (10)   NOT NULL,
    [ExeDate]   DATETIME        NOT NULL,
    [State]     SMALLINT        NOT NULL,
    [Version]   SMALLINT        NOT NULL,
    [Operator]  INT             NULL,
    [Remark]    NVARCHAR (500)  NULL,
    [Log]       NVARCHAR (MAX)  NULL,
    [WetPrice]  DECIMAL (10, 2) NULL,
    [CubePrice] DECIMAL (10, 2) NULL,
    CONSTRAINT [PK_WoodPrice] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);





