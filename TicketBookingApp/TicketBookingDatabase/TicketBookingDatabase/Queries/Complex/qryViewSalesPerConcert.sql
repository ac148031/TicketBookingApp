DECLARE @concertId AS INT = 2;

SELECT   s.saleId AS [Sale ID],
         CONCAT(cs.customerFirstName, ' ', cs.customerLastName) AS [Full Name],
         c.concertName AS [Concert Name],
         s.saleQuantity AS [Sale Quantity],
         c.concertTicketPrice AS [Ticket Price],
         (s.saleQuantity * c.concertTicketPrice) AS [Total Sale Price]
FROM     sales.tblSales AS s
         INNER JOIN
         sales.tblCustomers AS cs
         ON s.customerId = cs.customerId
         INNER JOIN
         concerts.tblConcerts AS c
         ON s.concertId = c.concertId
WHERE    c.concertId = @concertId
ORDER BY [Sale ID] ASC;