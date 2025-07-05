DECLARE @CityName AS VARCHAR (50) = 'Auckland';

SELECT   c.concertId AS [Concert ID],
         c.concertName AS [Concert Name],
         l.locationName AS [Venue Name],
         ct.cityName AS [City]
FROM     concerts.tblConcerts AS c
         INNER JOIN
         concerts.tblLocations AS l
         ON c.locationId = l.locationId
         INNER JOIN
         concerts.tblCities AS ct
         ON l.cityId = ct.cityId
WHERE    ct.cityName = @CityName
         OR CAST (ct.cityId AS VARCHAR) = @CityName
ORDER BY l.locationName;