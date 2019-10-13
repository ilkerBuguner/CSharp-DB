CREATE TABLE Logs(
LogId INT PRIMARY KEY IDENTITY,
AccountId INT,
OldSum DECIMAL(15,2) NOT NULL,
NewSum DECIMAL(15,2) NOT NULL
)

ALTER TRIGGER tr_Logger ON Accounts FOR UPDATE
AS
BEGIN

	DECLARE @newSum DECIMAL(15,2) = (SELECT i.Balance FROM inserted AS i)
	DECLARE @oldSum DECIMAL(15,2) = (SELECT d.Balance FROM deleted AS d)
	DECLARE @accountId INT = (SELECT i.Id FROM inserted AS i)

	INSERT INTO Logs (AccountId, OldSum, NewSum) VALUES
	(@accountId, @oldSum, @newSum)

END

