﻿CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider]       NVARCHAR (128)   NOT NULL,
    [ProviderKey]         NVARCHAR (128)   NOT NULL,
    [ProviderDisplayName] NVARCHAR (MAX)   NULL,
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC)
);

