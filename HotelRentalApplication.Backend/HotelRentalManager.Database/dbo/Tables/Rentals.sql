CREATE TABLE [dbo].[Rentals] (
    [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [RoomId]           UNIQUEIDENTIFIER NOT NULL,
    [UserId]           UNIQUEIDENTIFIER NOT NULL,
    [From]             DATE             NOT NULL,
    [To]               DATE             NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [LastModifiedDate] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Rooms] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

