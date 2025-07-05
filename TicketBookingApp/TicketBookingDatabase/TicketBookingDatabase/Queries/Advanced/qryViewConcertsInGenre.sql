DECLARE @genre AS VARCHAR (MAX) = 'Rock';

SELECT c.concertId AS [Concert ID],
       c.concertName AS [Concert Name],
       g.genreName AS [Genre]
FROM   concerts.tblConcerts AS c
       INNER JOIN
       concerts.tblConcertGenres AS cg
       ON c.concertId = cg.concertId
       INNER JOIN
       concerts.tblGenres AS g
       ON cg.genreId = g.genreId
WHERE  g.genreName = @genre;