SELECT   COALESCE(c.concertName, '***Overall Total***') AS [Concert Name],
         SUM(s.saleQuantity * c.concertTicketPrice) AS [Total Revenue]
FROM     sales.tblSales AS s
         INNER JOIN concerts.tblConcerts AS c
         ON s.concertId = c.concertId
GROUP BY ROLLUP(c.concertName)
ORDER BY [Total Revenue] DESC;
