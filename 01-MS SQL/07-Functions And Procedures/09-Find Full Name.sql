CREATE PROC usp_GetHoldersFullName 
AS
	SELECT (a.FirstName + ' ' + a.LastName) AS [Full Name]
	  FROM  AccountHolders AS a
