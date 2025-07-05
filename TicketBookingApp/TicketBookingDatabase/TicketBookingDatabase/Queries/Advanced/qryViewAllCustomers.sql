SELECT cs.customerId AS [Customer ID],
       cs.customerFirstName AS [First Name],
       cs.customerLastName AS [Last Name],
       cs.customerPhone AS [Phone],
       cs.customerEmail AS [Email],
       ca.streetAddress AS [Street Address],
       ct.cityName AS [City],
       ca.postalCode AS [Postal Code]
FROM   sales.tblCustomers AS cs
       LEFT OUTER JOIN
       sales.tblCustomerAddresses AS ca
       ON cs.customerId = ca.customerId
       LEFT OUTER JOIN
       concerts.tblCities AS ct
       ON ct.cityId = ca.cityId;