CREATE PROC usp_GetHoldersWithBalanceHigherThan (@number DECIMAL(15,3))
AS
SELECT ah.FirstName, ah.LastName FROM Accounts AS a
JOIN AccountHolders AS ah ON ah.Id = a.AccountHolderId
GROUP BY ah.FirstName, ah.LastName
HAVING SUM(a.Balance) > @number
ORDER BY ah.FirstName, ah.LastName

