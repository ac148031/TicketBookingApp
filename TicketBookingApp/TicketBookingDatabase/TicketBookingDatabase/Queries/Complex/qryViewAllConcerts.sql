SELECT   c.concertId,
         c.concertName,
         c.concertDescription,
         c.concertDate,
         c.concertTime, 
         c.concertAvailTickets,
         c.concertTicketPrice,
         l.locationId,
         l.locationName,
         l.locationAddress,
         l.locationCapacity,
         ct.cityId,
         ct.cityName,
         STRING_AGG(g.genreId, ':') AS genreIds,
         STRING_AGG(g.genreName, ':') AS genreNames,
         STRING_AGG(g.genreDescription, ':') AS genreDescriptions
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
GROUP BY c.concertId,
         c.concertName,
         c.concertDescription,
         c.concertDate,
         c.concertTime, 
         c.concertAvailTickets,
         c.concertTicketPrice,
         l.locationId,
         l.locationName,
         l.locationAddress,
         l.locationCapacity,
         ct.cityId,
         ct.cityName;