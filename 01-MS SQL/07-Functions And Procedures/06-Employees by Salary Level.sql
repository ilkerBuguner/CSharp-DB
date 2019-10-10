CREATE PROC usp_EmployeesBySalaryLevel (@level NVARCHAR(30))
AS
SELECT e.FirstName, e.LastName
FROM Employees AS e
WHERE dbo.ufn_GetSalaryLevel(Salary) = @level
