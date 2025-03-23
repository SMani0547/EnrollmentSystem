-- First ensure the SubjectAreas exist
IF NOT EXISTS (SELECT 1 FROM SubjectAreas WHERE Code = 'CS')
BEGIN
    INSERT INTO SubjectAreas (Code, Name, Description, IsActive, CanBeMajor, CanBeMinor) VALUES
    ('CS', 'Computer Science', 'Computer Science courses', 1, 1, 1),
    ('MA', 'Mathematics', 'Mathematics courses', 1, 1, 1),
    ('AC', 'Accounting', 'Accounting courses', 1, 1, 1),
    ('FI', 'Finance', 'Finance courses', 1, 1, 1),
    ('MG', 'Management', 'Management courses', 1, 1, 1),
    ('EC', 'Economics', 'Economics courses', 1, 1, 1),
    ('BI', 'Biology', 'Biology courses', 1, 1, 1),
    ('CH', 'Chemistry', 'Chemistry courses', 1, 1, 1),
    ('PH', 'Physics', 'Physics courses', 1, 1, 1);
END

-- Get the SubjectArea IDs
DECLARE @CSId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'CS');
DECLARE @MathId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'MA');
DECLARE @AccId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'AC');
DECLARE @FinId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'FI');
DECLARE @MgtId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'MG');
DECLARE @EcoId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'EC');
DECLARE @BioId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'BI');
DECLARE @CheId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'CH');
DECLARE @PhyId INT = (SELECT Id FROM SubjectAreas WHERE Code = 'PH');

-- Insert courses if they don't exist
IF NOT EXISTS (SELECT 1 FROM Courses WHERE Code = 'CS101')
BEGIN
    -- First ensure we have valid SubjectArea IDs
    IF @CSId IS NOT NULL AND @MathId IS NOT NULL AND @AccId IS NOT NULL AND @FinId IS NOT NULL 
       AND @MgtId IS NOT NULL AND @EcoId IS NOT NULL AND @BioId IS NOT NULL AND @CheId IS NOT NULL 
       AND @PhyId IS NOT NULL
    BEGIN
        INSERT INTO Courses (Code, Name, Description, CreditPoints, Level, Semester, SubjectAreaId, IsActive) VALUES
        ('CS101', 'Introduction to Computer Science', 'Basic concepts of computer science', 3, 0, 0, @CSId, 1),
        ('CS102', 'Programming Fundamentals', 'Introduction to programming concepts', 3, 0, 1, @CSId, 1),
        ('MA101', 'Calculus I', 'Introduction to calculus', 4, 0, 0, @MathId, 1),
        ('AC101', 'Financial Accounting', 'Basic accounting principles', 3, 0, 0, @AccId, 1),
        ('FI101', 'Introduction to Finance', 'Basic financial concepts', 3, 0, 1, @FinId, 1),
        ('MG101', 'Principles of Management', 'Basic management concepts', 3, 0, 0, @MgtId, 1),
        ('EC101', 'Microeconomics', 'Introduction to microeconomics', 3, 0, 0, @EcoId, 1),
        ('BI101', 'General Biology', 'Introduction to biology', 4, 0, 0, @BioId, 1),
        ('CH101', 'General Chemistry', 'Introduction to chemistry', 4, 0, 0, @CheId, 1),
        ('PH101', 'General Physics', 'Introduction to physics', 4, 0, 0, @PhyId, 1);
    END
END 