CREATE TABLE Deleted_Employees(
EmployeeId INT PRIMARY KEY,
FirstName NVARCHAR(30),
MiddleName NVARCHAR(30),
LastName NVARCHAR(30),
JobTitle NVARCHAR(50),
DepartmentId INT,
Salary DECIMAL(15,2)
)

CREATE TRIGGER tr_DeletedEmployes ON Employees FOR DELETE
AS
BEGIN
	INSERT INTO Deleted_Employees 
			(
			FirstName,
			MiddleName,
			LastName,
			JobTitle,
			DepartmentId,
			Salary) 

	 SELECT		   d.FirstName,
				   d.MiddleName,
				   d.LastName,
				   d.JobTitle,
				   d.DepartmentID,
				   d.Salary 
	 FROM deleted AS d
END