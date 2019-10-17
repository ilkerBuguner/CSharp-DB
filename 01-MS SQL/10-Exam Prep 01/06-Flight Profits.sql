SELECT t.FlightId, SUM(t.Price) FROM Tickets AS t
JOIN Flights AS f ON t.FlightId = f.Id
GROUP BY t.FlightId
ORDER BY SUM(t.Price) DESC, t.FlightId
