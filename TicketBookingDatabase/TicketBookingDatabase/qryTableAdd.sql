DROP TABLE IF EXISTS sales.tblSales;
DROP TABLE IF EXISTS sales.tblCustomerAddresses;
DROP TABLE IF EXISTS sales.tblCustomers;
DROP TABLE IF EXISTS concerts.tblConcertGenres;
DROP TABLE IF EXISTS concerts.tblConcerts;
DROP TABLE IF EXISTS concerts.tblLocations;
DROP TABLE IF EXISTS concerts.tblGenres;
DROP TABLE IF EXISTS concerts.tblCities;

DROP SCHEMA IF EXISTS concerts;
DROP SCHEMA IF EXISTS sales;
GO

CREATE SCHEMA concerts;
GO
CREATE SCHEMA sales;
GO

-- Has Data
CREATE TABLE concerts.tblCities (
    cityId   INT          IDENTITY (1, 1) PRIMARY KEY,
    cityName VARCHAR (50) NOT NULL
);

-- Has Data
CREATE TABLE concerts.tblLocations (
    locationId       INT           IDENTITY (1, 1) PRIMARY KEY,
    locationName     VARCHAR (255) NOT NULL,
    cityId           INT           NOT NULL,
    locationAddress  VARCHAR (MAX) NOT NULL,
    locationCapacity INT           NOT NULL,
    FOREIGN KEY (cityId) REFERENCES concerts.tblCities (cityId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Has Data
CREATE TABLE concerts.tblGenres (
    genreId          INT           IDENTITY (1, 1) PRIMARY KEY,
    genreName        VARCHAR (255) NOT NULL,
    genreDescription VARCHAR (MAX) NOT NULL
);

-- Has Data
CREATE TABLE concerts.tblConcerts (
    concertId           INT           IDENTITY (1, 1) PRIMARY KEY,
    concertName         VARCHAR (255) NOT NULL,
    concertDescription  VARCHAR (MAX) NOT NULL,
    concertDate         DATE          NOT NULL,
    concertTime         TIME          NOT NULL,
    concertAvailTickets INT           NOT NULL,
    concertTicketPrice  SMALLMONEY    NOT NULL,
    locationId          INT           NOT NULL,
    FOREIGN KEY (locationId) REFERENCES concerts.tblLocations (locationId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Has Data
CREATE TABLE concerts.tblConcertGenres (
    concertId INT NOT NULL,
    genreId   INT NOT NULL,
    PRIMARY KEY (concertId, genreId),
    FOREIGN KEY (concertId) REFERENCES concerts.tblConcerts (concertId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (genreId) REFERENCES concerts.tblGenres (genreId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Has Data
CREATE TABLE sales.tblCustomers (
    customerId        INT           IDENTITY (1, 1) PRIMARY KEY,
    customerFirstName VARCHAR (50)  NOT NULL,
    customerLastName  VARCHAR (50)  NOT NULL,
    customerPhone     VARCHAR (15)  NOT NULL,
    customerEmail     VARCHAR (255) NOT NULL,
    customerUsername  VARCHAR (255) NOT NULL,
    customerPassword  VARCHAR (255) NOT NULL
);

-- Has Data
CREATE TABLE sales.tblCustomerAddresses (
    addressId     INT           IDENTITY (1, 1) PRIMARY KEY,
    customerId    INT           NOT NULL,
    streetAddress VARCHAR (255) NOT NULL,
    cityId        INT           NOT NULL,
    postalCode    VARCHAR (10)  NOT NULL,
    FOREIGN KEY (customerId) REFERENCES sales.tblCustomers (customerId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (cityId) REFERENCES concerts.tblCities (cityId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Has Data
CREATE TABLE sales.tblSales (
    saleId       INT IDENTITY (1, 1) PRIMARY KEY,
    customerId   INT NOT NULL,
    concertId    INT NOT NULL,
    saleQuantity INT NOT NULL,
    FOREIGN KEY (customerId) REFERENCES sales.tblCustomers (customerId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (concertId) REFERENCES concerts.tblConcerts (concertId) ON DELETE CASCADE ON UPDATE CASCADE
);