-- Create StudentFinances table
CREATE TABLE StudentFinances (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentID NVARCHAR(MAX) NOT NULL,
    TotalFees DECIMAL(18, 2) NOT NULL,
    AmountPaid DECIMAL(18, 2) NOT NULL,
    LastUpdated DATETIME2(7) NOT NULL
);

-- Create Transactions table
CREATE TABLE Transactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Description NVARCHAR(MAX) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Date DATETIME NOT NULL,
    TransactionType NVARCHAR(50) NOT NULL,
    Category NVARCHAR(100) NULL,
    Notes NVARCHAR(MAX) NULL
);

-- Insert sample data into StudentFinances
INSERT INTO StudentFinances (StudentID, TotalFees, AmountPaid, LastUpdated)
VALUES 
    ('S11111111', 5000.00, 2500.00, GETDATE()),
    ('S22222222', 6000.00, 6000.00, GETDATE()),
    ('S33333333', 4500.00, 1500.00, GETDATE()),
    ('S44444444', 5500.00, 5500.00, GETDATE()),
    ('S55555555', 4800.00, 2400.00, GETDATE());

-- Insert sample data into Transactions
INSERT INTO Transactions (Description, Amount, Date, TransactionType, Category, Notes)
VALUES
    ('Semester 1 Fee Payment - S11111111', 2500.00, GETDATE(), 'Income', 'Tuition', 'First installment'),
    ('Semester 1 Fee Payment - S22222222', 6000.00, DATEADD(day, -5, GETDATE()), 'Income', 'Tuition', 'Full payment'),
    ('Semester 1 Fee Payment - S33333333', 1500.00, DATEADD(day, -2, GETDATE()), 'Income', 'Tuition', 'First installment'),
    ('Semester 1 Fee Payment - S44444444', 5500.00, DATEADD(day, -1, GETDATE()), 'Income', 'Tuition', 'Full payment'),
    ('Semester 1 Fee Payment - S55555555', 2400.00, GETDATE(), 'Income', 'Tuition', 'First installment'),
    ('Library Fine - S11111111', 50.00, GETDATE(), 'Income', 'Fine', 'Late book return'),
    ('Equipment Purchase', 1200.00, DATEADD(day, -10, GETDATE()), 'Expense', 'Equipment', 'New computers for lab'),
    ('Maintenance', 500.00, DATEADD(day, -15, GETDATE()), 'Expense', 'Maintenance', 'Monthly building maintenance');

-- Select data to verify
SELECT * FROM StudentFinances;
SELECT * FROM Transactions; 