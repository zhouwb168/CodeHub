CREATE TABLE [dbo].[CostStatement] (
    [OrderNo]      NVARCHAR (30)   NOT NULL,
    [Unique]       INT             NULL,
    [License]      NVARCHAR (50)   NOT NULL,
    [LinkMan]      NVARCHAR (30)   NOT NULL,
    [JWeight]      DECIMAL (18, 6) NULL,
    [Area]         NVARCHAR (50)   NULL,
    [Tree]         NVARCHAR (50)   NULL,
    [GWeight]      DECIMAL (18, 6) NOT NULL,
    [GPrice]       DECIMAL (18, 6) NOT NULL,
    [Amount]       DECIMAL (18, 6) NULL,
    [State]        SMALLINT        NOT NULL,
    [Operator]     INT             NULL,
    [OperatorDate] DATETIME        NULL,
    [Auditor]      INT             NULL,
    [IsPrint]      SMALLINT        NULL,
    [GroupID]      NVARCHAR (30)   NULL,
    [Bang_Time]    DATETIME        NULL,
    [Log]          NVARCHAR (500)  NULL,
    [IsConfirmed]  BIT             CONSTRAINT [DF_CostStatement_IsConfirmed] DEFAULT ((0)) NOT NULL,
    [Confirmor]    INT             NULL,
    [ConfirmTime]  DATETIME        NULL,
    [JVolume]      DECIMAL (18, 6) NULL,
    [FullVolume]   DECIMAL (18, 6) NULL,
    [CubePrice]    DECIMAL (10, 2) NULL,
    CONSTRAINT [PK_CostStatement_1] PRIMARY KEY CLUSTERED ([OrderNo] ASC)
);



