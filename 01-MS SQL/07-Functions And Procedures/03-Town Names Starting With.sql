CREATE PROC usp_GetTownsStartingWith (@symbol NVARCHAR(30))
AS
SELECT t.[Name] 
FROM Towns AS t
WHERE t.[Name] LIKE @symbol + '%'
