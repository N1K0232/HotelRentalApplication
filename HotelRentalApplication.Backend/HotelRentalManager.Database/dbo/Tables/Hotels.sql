CREATE TABLE [dbo].[Hotels] (
    [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]             NVARCHAR (100)   NOT NULL,
    [Stars]            INT              NOT NULL,
    [City]             NVARCHAR (100)   NOT NULL,
    [Country]          NVARCHAR (100)   NOT NULL,
    [Address]          NVARCHAR (100)   NOT NULL,
    [PricePerDay]      DECIMAL (5, 2)   NOT NULL,
    [HasSpa]           BIT              NOT NULL,
    [HasPool]          BIT              NOT NULL,
    [EmailAddress]     NVARCHAR (100)   NULL,
    [CellphoneNumber]  NVARCHAR (20)    NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [LastModifiedDate] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

