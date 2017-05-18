CREATE TABLE [dbo].[WoodBang] (
    [bangid]        INT             NOT NULL,
    [bangCid]       NVARCHAR (50)   NULL,
    [jWeight]       DECIMAL (18, 6) NULL,
    [Bang_Time]     DATETIME        NOT NULL,
    [carCID]        NVARCHAR (50)   NULL,
    [carUser]       NVARCHAR (50)   NULL,
    [firstBangUser] NVARCHAR (50)   NULL,
    [breedName]     NVARCHAR (50)   NULL,
    [userXHName]    NVARCHAR (50)   NULL,
    [IsFiltered]    BIT             DEFAULT ((0)) NOT NULL,
    [IsJoined]      BIT             DEFAULT ((0)) NOT NULL,
    [IsJoinGsmed]   BIT             DEFAULT ((0)) NOT NULL,
    [IsFilterGsmed] BIT             DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([bangid] ASC)
);





GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Bang_Time]
    ON [dbo].[WoodBang]([Bang_Time] ASC);

