CREATE TABLE [dbo].[GsmSupplier] (
    [Unique]    INT            NOT NULL,
    [Type]      INT            NOT NULL,
    [Name]      NVARCHAR (50)  NOT NULL,
    [Remark]    NVARCHAR (MAX) NULL,
    [State]     INT            NOT NULL,
    [Version]   INT            NOT NULL,
    [Operator]  INT            NOT NULL,
    [Log]       NVARCHAR (MAX) NOT NULL,
    [LinkMan]   NVARCHAR (30)  NULL,
    [LinkPhone] NVARCHAR (10)  NULL,
    CONSTRAINT [PK_GsmSupplier] PRIMARY KEY CLUSTERED ([Unique] ASC, [Version] ASC)
);

