CREATE PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN
	
	DECLARE @employeesDepartmentId INT = (SELECT e.DepartmentId FROM Employees AS e
									  WHERE e.Id = @EmployeeId)

	DECLARE @reportsCategoryDepartmentId INT = (SELECT c.DepartmentId FROM Reports AS r
												JOIN Categories AS c ON c.Id = r.CategoryId
												WHERE r.Id = @ReportId)

	IF(@employeesDepartmentId = @reportsCategoryDepartmentId)
		BEGIN
			UPDATE Reports
			SET EmployeeId = @EmployeeId
			WHERE Id = @ReportId
		END
	ELSE
		BEGIN
			RAISERROR('Employee doesn''t belong to the appropriate department!', 16, 2)
			RETURN
		END
	
END
