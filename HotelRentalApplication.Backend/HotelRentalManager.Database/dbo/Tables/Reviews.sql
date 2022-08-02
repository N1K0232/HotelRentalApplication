CREATE TABLE [dbo].[Reviews] (
    [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [UserId]           UNIQUEIDENTIFIER NOT NULL,
    [HotelId]          UNIQUEIDENTIFIER NOT NULL,
    [Value]            INT              NOT NULL,
    [Title]            NVARCHAR (50)    NOT NULL,
    [Text]             NVARCHAR (512)   NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [LastModifiedDate] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([HotelId]) REFERENCES [dbo].[Hotels] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

