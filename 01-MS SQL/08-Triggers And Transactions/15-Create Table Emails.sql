CREATE TABLE NotificationEmails(
Id INT PRIMARY KEY IDENTITY,
Recipient INT,
[Subject] NVARCHAR(MAX),
Body NVARCHAR(MAX)
)

CREATE TRIGGER tr_notificationEmails ON Logs FOR INSERT
AS
BEGIN
	DECLARE @recipient INT = (SELECT i.AccountId FROM inserted AS i)
	DECLARE @oldSum DECIMAL(15,2) = (SELECT i.OldSum FROM inserted AS i)
	DECLARE @newSum DECIMAL(15,2) = (SELECT i.NewSum FROM inserted AS i)

	INSERT INTO NotificationEmails (Recipient, [Subject], Body) VALUES
	(
		@recipient,
		'Balance change for account: ' + CAST(@recipient AS NVARCHAR(50)),
		'On ' + CAST(GETDATE() AS NVARCHAR(50)) + ' your balance was changed from ' + CAST(@oldSum AS NVARCHAR(50)) +' to ' + CAST(@newSum AS NVARCHAR(50)) + '.'
	)
END