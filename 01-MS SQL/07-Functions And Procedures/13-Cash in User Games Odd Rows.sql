CREATE FUNCTION ufn_CashInUsersGames (@gameName NVARCHAR(MAX))
RETURNS TABLE AS
RETURN
(
 SELECT SUM(Cash) AS 'SumCash'
  FROM(
	SELECT ug.Cash, ROW_NUMBER() OVER(PARTITION BY g.[Name] ORDER BY ug.Cash DESC) AS [Row]
	FROM UsersGames AS ug
	JOIN Games as g ON g.Id = ug.GameId
	WHERE g.[Name] = @gameName) AS k
	WHERE k.[Row] % 2 = 1
)