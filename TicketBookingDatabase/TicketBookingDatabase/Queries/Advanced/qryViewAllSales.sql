SELECT   s.saleId AS [Sale ID],
         cust.customerFirstName AS [First Name],
         cust.customerLastName AS [Last Name],
         c.concertName AS [Concert],
         s.saleQuantity AS [Tickets Sold]
FROM     sales.tblSales AS s
         INNER JOIN
         sales.tblCustomers AS cust
         ON s.customerId = cust.customerId
         INNER JOIN
         concerts.tblConcerts AS c
         ON s.concertId = c.concertId
ORDER BY s.saleId;