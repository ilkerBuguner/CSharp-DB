CREATE FUNCTION udf_CalculateTickets(@origin VARCHAR(50), @destination VARCHAR(50), @peopleCount INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
	IF(@peopleCount <= 0)
	BEGIN
		RETURN 'Invalid people count!';
	END

	DECLARE @flightId INT = (SELECT Id 
							 FROM Flights 
							 WHERE Destination = @destination AND Origin = @origin)

	IF(@flightId IS NULL)
	BEGIN
		RETURN 'Invalid flight!'
	END

	DECLARE @flightsPrice DECIMAL(15,2) = (SELECT t.Price FROM Tickets AS t
										   JOIN Flights AS f ON t.FlightId = f.Id
										   WHERE f.Origin = @origin AND f.Destination = @destination)

	RETURN 'Total price ' + CAST((@flightsPrice * @peopleCount) AS VARCHAR(20))
END