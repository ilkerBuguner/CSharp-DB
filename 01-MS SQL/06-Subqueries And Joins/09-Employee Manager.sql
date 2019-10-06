SELECT e.EmployeeID, e.FirstName, e.ManagerID, mg.FirstName AS [ManagerName]
FROM Employees AS e
JOIN Employees AS mg ON mg.EmployeeID = e.ManagerID
WHERE mg.EmployeeID IN(3, 7)
ORDER BY e.EmployeeID

