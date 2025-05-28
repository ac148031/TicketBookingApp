SELECT l.locationId AS [Location ID],
       l.locationName AS [Venue Name],
       l.locationCapacity AS [Venue Capacity],
       l.locationAddress AS [Venue Address],
       ct.cityName AS [Venue City]
FROM   concerts.tblLocations AS l
       LEFT OUTER JOIN
       concerts.tblCities AS ct
       ON l.cityId = ct.cityId;