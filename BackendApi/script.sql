IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [Role] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Addresses] (
    [Id] int NOT NULL IDENTITY,
    [Street] nvarchar(200) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [State] nvarchar(50) NOT NULL,
    [ZipCode] nvarchar(20) NOT NULL,
    [Country] nvarchar(50) NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Addresses_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [SessionLogs] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Action] nvarchar(50) NOT NULL,
    [LoginTime] datetime2 NOT NULL,
    [LogoutTime] datetime2 NULL,
    [IpAddress] nvarchar(45) NOT NULL,
    CONSTRAINT [PK_SessionLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SessionLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Studies] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Studies] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Studies_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FirstName', N'LastName', N'PasswordHash', N'Role', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [CreatedAt], [Email], [FirstName], [LastName], [PasswordHash], [Role], [Username])
VALUES (1, '2024-01-01T00:00:00.0000000Z', N'admin@example.com', N'Admin', N'System', N'$2a$11$ssUplNDkYhF8p1BD7HfN8uWe2HNmDsvA1dw9FIv5JedNK32mxOpWC', 0, N'admin'),
(2, '2024-01-01T00:00:00.0000000Z', N'user@example.com', N'Juan', N'Perez', N'$2a$11$gYLjOxzy./0KMeqmvQPWe.LeLvNpwZuojyv3cj/lm4AjDXoiZ3.Ca', 1, N'usuario');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FirstName', N'LastName', N'PasswordHash', N'Role', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;
GO

CREATE UNIQUE INDEX [IX_Addresses_UserId] ON [Addresses] ([UserId]);
GO

CREATE INDEX [IX_SessionLogs_UserId] ON [SessionLogs] ([UserId]);
GO

CREATE INDEX [IX_Studies_UserId] ON [Studies] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260120233327_InitialCreate', N'8.0.11');
GO

COMMIT;
GO

