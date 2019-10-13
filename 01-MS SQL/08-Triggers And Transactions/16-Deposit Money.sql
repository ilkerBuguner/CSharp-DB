CREATE PROC usp_DepositMoney (@AccountId INT, @MoneyAmount DECIMAL(15,4))
AS
BEGIN

	DECLARE @TargetAccountId INT = (SELECT a.Id 
									FROM Accounts AS a 
									WHERE a.Id = @AccountId)

	IF(@MoneyAmount < 0 OR @MoneyAmount IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Cannot enter negative money.', 15, 1)
		RETURN
	END

	IF(@TargetAccountId IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Invalid Id Parameter.', 16, 1)
		RETURN
	END

	UPDATE Accounts
	SET Balance += @MoneyAmount
	WHERE Id = @AccountId
END