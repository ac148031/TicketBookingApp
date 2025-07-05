DECLARE @search AS VARCHAR (MAX) = NULL;

SELECT cs.customerId AS [Customer ID],
       CONCAT(cs.customerFirstName, ' ', cs.customerLastName) AS [Full Name],
       cs.customerPhone AS [Phone],
       cs.customerEmail AS [Email],
       CONCAT(ca.streetAddress, ', ', ct.cityName, ', ', ca.postalCode) AS [Address]
FROM   sales.tblCustomers AS cs
       LEFT OUTER JOIN
       sales.tblCustomerAddresses AS ca
       ON cs.customerId = ca.customerId
       LEFT OUTER JOIN
       concerts.tblCities AS ct
       ON ct.cityId = ca.cityId
WHERE (
        CASE WHEN CAST(cs.customerId AS VARCHAR) = @search THEN 1 ELSE 0 END +
        CASE WHEN cs.customerFirstName = @search 
                  OR cs.customerLastName = @search 
                  OR CONCAT(cs.customerFirstName, ' ', cs.customerLastName) = @search THEN 1 ELSE 0 END +
        CASE WHEN cs.customerPhone = @search THEN 1 ELSE 0 END +
        CASE WHEN cs.customerEmail = @search THEN 1 ELSE 0 END
      ) = 1;
