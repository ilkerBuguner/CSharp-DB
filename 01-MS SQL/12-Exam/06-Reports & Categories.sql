SELECT r.Description, c.Name AS [CategoryName] FROM Reports AS r
LEFT JOIN Categories AS c ON r.CategoryId = c.Id
WHERE c.Id IS NOT NULL
ORDER BY r.Description, c.Name