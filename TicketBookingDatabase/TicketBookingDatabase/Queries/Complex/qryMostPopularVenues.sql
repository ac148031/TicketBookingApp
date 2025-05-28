SELECT   l.locationName AS [Venue Name],
         SUM(s.saleQuantity) AS [Total Tickets Sold]
FROM     sales.tblSales AS s
         INNER JOIN
         concerts.tblConcerts AS c
         ON s.concertId = c.concertId
         INNER JOIN
         concerts.tblLocations AS l
         ON c.locationId = l.locationId
GROUP BY l.locationName
ORDER BY [Total Tickets Sold] DESC;