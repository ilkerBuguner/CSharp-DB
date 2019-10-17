SELECT CONCAT(p.FirstName, ' ', p.LastName) AS [Full Name],
	   pl.[Name] AS [Plane Name],
	   CONCAT(f.Origin, ' - ', f.Destination) AS [Trip],
	   lt.[Type] AS [Luggage Type]
FROM Passengers AS p
JOIN Tickets AS t ON t.PassengerId = p.Id
JOIN Flights AS f ON t.FlightId = f.Id
JOIN Planes AS pl ON f.PlaneId = pl.Id
JOIN Luggages AS l ON t.LuggageId = l.Id
JOIN LuggageTypes AS lt ON lt.Id = l.LuggageTypeId
ORDER BY [Full Name], [Name],f.Origin, f.Destination, [Luggage Type]