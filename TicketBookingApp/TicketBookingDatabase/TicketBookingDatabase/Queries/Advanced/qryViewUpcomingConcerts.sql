SELECT concertName AS [Concert Name],
       concertDate AS [Date],
       locationName AS [Venue Name]
FROM   concerts.tblConcerts
       INNER JOIN
       concerts.tblLocations
       ON concerts.tblConcerts.locationId = concerts.tblLocations.locationId
WHERE  concertDate BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE());