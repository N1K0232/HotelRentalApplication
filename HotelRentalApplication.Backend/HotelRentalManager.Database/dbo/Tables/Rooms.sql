CREATE TABLE [dbo].[Rooms] (
    [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [HotelId]          UNIQUEIDENTIFIER NOT NULL,
    [Number]           INT              NOT NULL,
    [MaxMembersNumber] INT              NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [LastModifiedDate] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([HotelId]) REFERENCES [dbo].[Hotels] ([Id])
);

