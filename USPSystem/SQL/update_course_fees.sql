-- Update all course fees to $200
UPDATE Courses
SET Fees = 200.00
WHERE 1=1;

-- Verify the update
SELECT Code, Name, Fees
FROM Courses
ORDER BY Code; 