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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    CREATE TABLE [Authors] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Biography] nvarchar(200) NULL,
        [Country] nvarchar(100) NULL,
        CONSTRAINT [PK_Authors] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    CREATE TABLE [Genres] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [Description] nvarchar(200) NULL,
        CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    CREATE TABLE [Books] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [AuthorId] int NOT NULL,
        [ISBN] nvarchar(20) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Year] int NOT NULL,
        CONSTRAINT [PK_Books] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Books_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    CREATE TABLE [BookGenres] (
        [BooksId] int NOT NULL,
        [GenresId] int NOT NULL,
        CONSTRAINT [PK_BookGenres] PRIMARY KEY ([BooksId], [GenresId]),
        CONSTRAINT [FK_BookGenres_Books_BooksId] FOREIGN KEY ([BooksId]) REFERENCES [Books] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BookGenres_Genres_GenresId] FOREIGN KEY ([GenresId]) REFERENCES [Genres] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Biography', N'Country', N'Name') AND [object_id] = OBJECT_ID(N'[Authors]'))
        SET IDENTITY_INSERT [Authors] ON;
    EXEC(N'INSERT INTO [Authors] ([Id], [Biography], [Country], [Name])
    VALUES (1, NULL, N''United States'', N''F. Scott Fitzgerald''),
    (2, NULL, N''United States'', N''Harper Lee''),
    (3, NULL, N''United Kingdom'', N''George Orwell'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Biography', N'Country', N'Name') AND [object_id] = OBJECT_ID(N'[Authors]'))
        SET IDENTITY_INSERT [Authors] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Genres]'))
        SET IDENTITY_INSERT [Genres] ON;
    EXEC(N'INSERT INTO [Genres] ([Id], [Description], [Name])
    VALUES (1, N''Books that have stood the test of time'', N''Classic''),
    (2, N''Literary works created from imagination'', N''Fiction''),
    (3, N''Fiction with magical or supernatural elements'', N''Fantasy''),
    (4, N''Stories focused on romantic relationships'', N''Romance''),
    (5, N''Fiction based on scientific discoveries or advanced technology'', N''Science Fiction'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Genres]'))
        SET IDENTITY_INSERT [Genres] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AuthorId', N'ISBN', N'Price', N'Title', N'Year') AND [object_id] = OBJECT_ID(N'[Books]'))
        SET IDENTITY_INSERT [Books] ON;
    EXEC(N'INSERT INTO [Books] ([Id], [AuthorId], [ISBN], [Price], [Title], [Year])
    VALUES (1, 1, N''9780743273565'', 12.99, N''The Great Gatsby'', 1925),
    (2, 2, N''9780061120084'', 14.99, N''To Kill a Mockingbird'', 1960),
    (3, 3, N''9780451524935'', 11.99, N''1984'', 1949)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AuthorId', N'ISBN', N'Price', N'Title', N'Year') AND [object_id] = OBJECT_ID(N'[Books]'))
        SET IDENTITY_INSERT [Books] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'BooksId', N'GenresId') AND [object_id] = OBJECT_ID(N'[BookGenres]'))
        SET IDENTITY_INSERT [BookGenres] ON;
    EXEC(N'INSERT INTO [BookGenres] ([BooksId], [GenresId])
    VALUES (1, 1),
    (2, 2),
    (3, 3)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'BooksId', N'GenresId') AND [object_id] = OBJECT_ID(N'[BookGenres]'))
        SET IDENTITY_INSERT [BookGenres] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_BookGenres_GenresId] ON [BookGenres] ([GenresId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Books_AuthorId] ON [Books] ([AuthorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513121150_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250513121150_InitialCreate', N'9.0.4');
END;

COMMIT;
GO

