CREATE TABLE [dbo].[Reservations] (
    [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [RoomId]           UNIQUEIDENTIFIER NOT NULL,
    [UserId]           UNIQUEIDENTIFIER NOT NULL,
    [Members]          INTEGER          NOT NULL,
    [StartDate]        DATE             NOT NULL,
    [FinishDate]       DATE             NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [LastModifiedDate] DATETIME         NULL,
    [IsDeleted]        BIT              NOT NULL,
    [DeletedDate]      DATETIME         NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Rooms] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);