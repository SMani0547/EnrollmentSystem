-- First, check if migration history table exists and create it if needed
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

-- Add the initial migration as if it was already applied
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250323101547_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250323101547_InitialCreate', N'8.0.3');
END
GO

-- Create the RecheckApplications table if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'RecheckApplications')
BEGIN
    CREATE TABLE [RecheckApplications] (
        [Id] int NOT NULL IDENTITY,
        [StudentId] nvarchar(450) NOT NULL,
        [CourseCode] nvarchar(max) NOT NULL,
        [CourseName] nvarchar(max) NOT NULL,
        [CurrentGrade] nvarchar(max) NOT NULL,
        [Year] int NOT NULL,
        [Semester] int NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PaymentReceiptNumber] nvarchar(max) NOT NULL,
        [Reason] nvarchar(max) NOT NULL,
        [AdditionalComments] nvarchar(max) NULL,
        [ApplicationDate] datetime2 NOT NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_RecheckApplications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RecheckApplications_AspNetUsers_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_RecheckApplications_StudentId] ON [RecheckApplications] ([StudentId]);
END
GO

-- Add the RecheckApplications migration entry
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250526010655_AddRecheckApplicationsTable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250526010655_AddRecheckApplicationsTable', N'8.0.3');
END
GO 