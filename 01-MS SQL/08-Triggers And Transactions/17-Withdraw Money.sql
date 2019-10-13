CREATE PROC usp_WithdrawMoney (@AccountId INT, @MoneyAmount DECIMAL(18,4))
AS
BEGIN
	
	IF(@MoneyAmount < 0 OR @MoneyAmount IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Withdraw money cannot be null or negative', 15, 1)
		RETURN
	END

	UPDATE Accounts
	SET Balance -= @MoneyAmount
	WHERE Id = @AccountId
END