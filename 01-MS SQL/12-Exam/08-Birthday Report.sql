SELECT u.Username, c.Name FROM Users AS u
JOIN Reports AS r ON r.UserId = u.Id
JOIN Categories AS c ON r.CategoryId = c.Id
WHERE DAY(u.Birthdate) = DAY(r.OpenDate) AND MONTH(u.Birthdate) = MONTH(r.OpenDate)
ORDER BY u.Username, c.Name