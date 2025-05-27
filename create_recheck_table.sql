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
    
    PRINT 'RecheckApplications table created successfully';
END
ELSE
BEGIN
    PRINT 'RecheckApplications table already exists';
END
GO 