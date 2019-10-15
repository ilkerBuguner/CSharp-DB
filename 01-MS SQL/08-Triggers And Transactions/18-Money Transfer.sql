CREATE PROC usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount DECIMAL(18, 4))
AS
BEGIN
	
	DECLARE @targetSender INT = (SELECT Id FROM Accounts WHERE Id = @SenderId)
	DECLARE @targetReceiver INT = (SELECT Id FROM Accounts WHERE Id = @ReceiverId)

	IF(@targetSender IS NULL OR @targetReceiver IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Sender or Receiver cannot be null.', 16,2)
		RETURN
	END

	IF(@Amount < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Money ammont cannot be negative number.', 16,1)
		RETURN
	END

	EXEC dbo.usp_DepositMoney @targetReceiver, @Amount
	EXEC dbo.usp_WithdrawMoney @targetSender, @Amount

END