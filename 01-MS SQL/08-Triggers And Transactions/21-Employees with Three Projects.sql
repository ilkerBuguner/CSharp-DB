CREATE PROC usp_AssignProject(@emloyeeId INT, @projectID INT)
AS
BEGIN TRANSACTION
	
	IF((SELECT COUNT(ep.ProjectID)
		FROM Employees AS e
		JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
		WHERE e.EmployeeID = @emloyeeId) >= 3)
	BEGIN
		ROLLBACK
		RAISERROR('The employee has too many projects!',16,1)
		RETURN
	END

	INSERT INTO EmployeesProjects (EmployeeID, ProjectID) VALUES
	(@emloyeeId, @projectID)

COMMIT
