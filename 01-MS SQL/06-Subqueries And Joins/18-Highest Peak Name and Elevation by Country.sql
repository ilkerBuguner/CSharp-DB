SELECT TOP(5) HighestPeaksPerCountry.CountryName AS 'Country',
       ISNULL(HighestPeaksPerCountry.PeakName, '(no highest peak)') AS [Highest Peak Name],
	   ISNULL(HighestPeaksPerCountry.Elevation, '0') AS [Highest Peak Elevation],
	   ISNULL(HighestPeaksPerCountry.MountainRange, '(no mountain)') AS [Mountain]
FROM 
(
   SELECT c.CountryName,
	      p.PeakName,
	      p.Elevation,
	      m.MountainRange,
		  DENSE_RANK() OVER (PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS 'HighestPeak'
     FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
LEFT JOIN Peaks AS p ON p.MountainId = m.Id
) AS HighestPeaksPerCountry
WHERE HighestPeaksPerCountry.HighestPeak = 1
ORDER BY HighestPeaksPerCountry.CountryName, HighestPeaksPerCountry.HighestPeak



