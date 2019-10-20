SELECT r.Description, CONVERT(VARCHAR, r.OpenDate, 105) AS [OpenDate] FROM Reports AS r
LEFT JOIN Employees AS e ON r.EmployeeId = e.Id
WHERE e.Id IS NULL
ORDER BY r.OpenDate, r.Description