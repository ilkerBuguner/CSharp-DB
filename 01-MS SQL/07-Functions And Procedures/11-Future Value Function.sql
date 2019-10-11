CREATE FUNCTION ufn_CalculateFutureValue (@sum DECIMAL(15,3), @YNR FLOAT, @numOfYears INT)
RETURNS DECIMAL(15,4)
AS
BEGIN
	DECLARE @futureValue DECIMAL(15,4)

		SET @futureValue = @sum * POWER((1 + @YNR),  @numOfYears)

	RETURN @futureValue	
END