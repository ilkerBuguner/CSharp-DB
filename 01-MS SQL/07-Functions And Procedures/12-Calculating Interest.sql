CREATE PROC usp_CalculateFutureValueForAccount(@AccountId INT, @Rate FLOAT)
AS
BEGIN
	SELECT a.Id,
		   ah.FirstName,
		   ah.LastName, 
		   a.Balance, 
		   dbo.ufn_CalculateFutureValue(a.Balance, @Rate , 5) AS [Balance in 5 years]
	 FROM Accounts AS a 
	JOIN AccountHolders AS ah ON a.AccountHolderId = ah.Id
	WHERE a.Id = @AccountId
END