DECLARE @date AS DATE = '2025-06-15';

SELECT concertName AS [Concert Name],
       concertDate AS [Date],
       CONVERT (VARCHAR (5), concertTime, 108) AS [Time]
FROM   concerts.tblConcerts
WHERE  concertDate = @date;