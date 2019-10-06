SELECT TOP(50) e.EmployeeID,
	   (e.FirstName + ' ' + e.LastName) AS 'EmployeeName',
	   (mg.FirstName + ' ' + mg.LastName) AS 'ManagerName',
	   d.[Name] AS 'DepartmentName' 
FROM Employees AS e
JOIN Employees AS mg ON e.ManagerID = mg.EmployeeID
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID
