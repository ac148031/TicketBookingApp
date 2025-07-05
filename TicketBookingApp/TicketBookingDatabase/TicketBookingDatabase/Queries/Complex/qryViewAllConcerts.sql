SELECT   c.concertId AS [Concert ID],
         c.concertName AS [Concert Name],
         c.concertDescription AS [Description],
         STRING_AGG(g.genreName, ', ') AS [Genres],
         c.concertDate AS [Date],
         CONVERT (VARCHAR (5), c.concertTime, 108) AS [Time],
         c.concertAvailTickets AS [Available Tickets],
         c.concertTicketPrice AS [Ticket Price],
         l.locationName AS [Venue Name],
         CONCAT(l.locationAddress, ', ', ct.cityName) AS [Venue]
FROM     concerts.tblConcerts AS c
         LEFT OUTER JOIN
         concerts.tblConcertGenres AS cg
         ON c.concertId = cg.concertId
         LEFT OUTER JOIN
         concerts.tblGenres AS g
         ON cg.genreId = g.genreId
         LEFT OUTER JOIN
         concerts.tblLocations AS l
         ON c.locationId = l.locationId
         LEFT OUTER JOIN
         concerts.tblCities AS ct
         ON l.cityId = ct.cityId
GROUP BY c.concertId, c.concertName, c.concertDescription, 
         c.concertDate, c.concertTime, c.concertAvailTickets, 
         c.concertTicketPrice, l.locationName, l.locationAddress, 
         ct.cityName;