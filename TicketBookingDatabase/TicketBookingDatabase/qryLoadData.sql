ALTER TABLE concerts.tblConcerts NOCHECK CONSTRAINT ALL;
ALTER TABLE sales.tblSales NOCHECK CONSTRAINT ALL;
ALTER TABLE sales.tblCustomerAddresses NOCHECK CONSTRAINT ALL;
ALTER TABLE sales.tblCustomers NOCHECK CONSTRAINT ALL;
ALTER TABLE concerts.tblConcertGenres NOCHECK CONSTRAINT ALL;
ALTER TABLE concerts.tblLocations NOCHECK CONSTRAINT ALL;
ALTER TABLE concerts.tblGenres NOCHECK CONSTRAINT ALL;
ALTER TABLE concerts.tblCities NOCHECK CONSTRAINT ALL;

DELETE FROM sales.tblSales;
DELETE FROM sales.tblCustomerAddresses;
DELETE FROM sales.tblCustomers;
DELETE FROM concerts.tblConcertGenres;
DELETE FROM concerts.tblLocations;
DELETE FROM concerts.tblGenres;
DELETE FROM concerts.tblCities;
DELETE FROM concerts.tblConcerts;

ALTER TABLE concerts.tblConcerts CHECK CONSTRAINT ALL;
ALTER TABLE sales.tblSales CHECK CONSTRAINT ALL;
ALTER TABLE sales.tblCustomerAddresses CHECK CONSTRAINT ALL;
ALTER TABLE sales.tblCustomers CHECK CONSTRAINT ALL;
ALTER TABLE concerts.tblConcertGenres CHECK CONSTRAINT ALL;
ALTER TABLE concerts.tblLocations CHECK CONSTRAINT ALL;
ALTER TABLE concerts.tblGenres CHECK CONSTRAINT ALL;
ALTER TABLE concerts.tblCities CHECK CONSTRAINT ALL;
GO

BEGIN TRANSACTION;

SET IDENTITY_INSERT concerts.tblCities ON;
INSERT  INTO concerts.tblCities (cityId, cityName)
VALUES                
(1, 'Auckland'),
(2, 'Wellington'),
(3, 'Christchurch'),
(4, 'Hamilton'),
(5, 'Tauranga'),
(6, 'Dunedin'),
(7, 'Palmerston North'),
(8, 'Napier'),
(9, 'Hastings'),
(10, 'Nelson'),
(11, 'New Plymouth');
SET IDENTITY_INSERT concerts.tblCities OFF;

SET IDENTITY_INSERT concerts.tblLocations ON;
INSERT  INTO concerts.tblLocations (locationId, locationName, cityId, locationAddress, locationCapacity)
VALUES                   
(1, 'Spark Arena', 1, '42-80 Mahuhu Crescent, Parnell', 12000),
(2, 'The Powerstation', 1, '33 Mount Eden Road, Grafton', 1000),
(3, 'The Civic', 1, 'Corner of Queen Street and Wellesley Street', 2378),
(4, 'The Trusts Arena', 1, '65-67 Central Park Drive, Henderson', 5000),
(5, 'Studio The Venue', 1, '340 Karangahape Road', 800),
(6, 'TSB Bank Arena', 2, '4 Queens Wharf', 5000),
(7, 'Michael Fowler Centre', 2, '111 Wakefield Street', 2209),
(8, 'Opera House', 2, '111-113 Manners Street, Te Aro', 1381),
(9, 'San Fran', 2, '171 Cuba Street, Te Aro', 400),
(10, 'Meow', 2, '9 Edward Street, Te Aro', 250),
(11, 'Christchurch Town Hall', 3, '86 Kilmore Street, Christchurch Central City', 2500),
(12, 'Isaac Theatre Royal', 3, '145 Gloucester Street, Christchurch Central City', 1292),
(13, 'Horncastle Arena', 3, '55 Jack Hinton Drive, Addington', 9000),
(14, 'Blue Smoke', 3, '3 Garlands Road, Woolston', 300),
(15, 'The Piano', 3, '156 Armagh Street, Christchurch Central City', 327),
(16, 'Claudelands Arena', 4, 'Corner of Brooklyn Road and Heaphy Terrace', 5000),
(17, 'The Meteor Theatre', 4, '1 Victoria Street', 250),
(18, 'The Factory', 4, '28 Alexandra Street', 300),
(19, 'Navara Lounge', 4, '266 Victoria Street', 150),
(20, 'The Bank Bar & Brasserie', 4, '117 Victoria Street', 200),
(21, 'Baycourt Community and Arts Centre', 5, '38 Durham Street', 582),
(22, 'Totara Street', 5, '11 Totara Street, Mount Maunganui', 400),
(23, 'The Historic Village', 5, '17th Avenue West', 350),
(24, 'The Barrel Room', 5, '26 Wharf Street', 150),
(25, 'The Hop House', 5, '12 Wharf Street', 120),
(26, 'Forsyth Barr Stadium', 6, '130 Anzac Avenue', 30500),
(27, 'Regent Theatre', 6, '17 The Octagon', 1617),
(28, 'The Crown Hotel', 6, '179 Rattray Street', 250),
(29, 'Dog With Two Tails', 6, '25 Moray Place', 100),
(30, 'The Cook', 6, '354 Great King Street', 300),
(31, 'Regent on Broadway', 7, '53 Broadway Avenue', 1393),
(32, 'The Globe Theatre', 7, '312 Main Street', 200),
(33, 'The Stomach', 7, '84 Lombard Street', 150),
(34, 'Centrepoint Theatre', 7, '280 Church Street', 200),
(35, 'The Royal', 7, '44 Rangitikei Street', 250),
(36, 'Municipal Theatre', 8, '119 Tennyson Street, Napier South', 993),
(37, 'MTG Century Theatre', 8, '9 Herschell Street, Napier South', 320),
(38, 'The Cabana', 8, '11 Shakespeare Road, Bluff Hill', 250),
(39, 'Paisley Stage', 8, '17 Carlyle Street, Napier South', 200),
(40, 'Crab Farm Winery', 8, '511 Main North Road, Bay View', 400),
(41, 'Toitoi – Hawke''s Bay Arts & Events Centre', 9, '101 Hastings Street South', 979),
(42, 'The Common Room', 9, '227 Heretaunga Street East', 150),
(43, 'Spaceship', 9, '114 Karamu Road North', 200),
(44, 'Hastings Sports Centre', 9, '503 Railway Road', 500),
(45, 'The Riverbend Church Auditorium', 9, '354 Te Aute Road', 600),
(46, 'Theatre Royal Nelson', 10, '78 Rutherford Street', 350),
(47, 'Nelson Centre of Musical Arts', 10, '48 Nile Street', 300),
(48, 'Founders Heritage Park', 10, '87 Atawhai Drive, The Wood', 600),
(49, 'The Boathouse', 10, '326 Wakefield Quay', 250),
(50, 'The Free House', 10, '95 Collingwood Street', 150),
(51, 'TSB Showplace', 11, '92-100 Devon Street West', 962),
(52, 'Theatre Royal', 11, '92-100 Devon Street West', 520),
(53, 'Butlers Reef Hotel', 11, '1133 South Road', 400),
(54, 'The Mayfair', 11, '69 Devon Street West', 300),
(55, '4th Wall Theatre', 11, '11 Baring Terrace', 200);
SET IDENTITY_INSERT concerts.tblLocations OFF;

SET IDENTITY_INSERT concerts.tblConcerts ON;
INSERT  INTO concerts.tblConcerts (concertId, concertName, concertDescription, concertDate, concertTime, concertAvailTickets, concertTicketPrice, locationId)
VALUES                  
(1, 'Russell Peters - Relax World Tour', 'Global stand-up sensation with hilarious new material.', '2025-03-18', '20:00:00', 12000, 79.99, 1),
(2, 'Dua Lipa - Radical Optimism Tour', 'Pop star performing her biggest dancefloor hits.', '2025-04-02', '15:00:00', 1000, 99.99, 2),
(3, 'The Killers - Imploding the Mirage Tour', 'Alternative rock legends bringing their anthems to life.', '2025-04-15', '18:00:00', 2378, 129.99, 3),
(4, 'The Weeknd - After Hours Tour', 'R&B superstar with an immersive visual experience.', '2025-05-01', '20:00:00', 5000, 149.99, 4),
(5, 'The Chainsmokers - World War Joy Tour', 'Electro-pop duo delivering high-energy performances.', '2025-05-15', '16:00:00', 800, 69.99, 5),
(6, 'Katy Perry - Smile Tour', 'Chart-topping pop queen with a colorful stage show.', '2025-06-01', '20:00:00', 5000, 99.99, 6),
(7, 'Harry Styles - Love On Tour', 'Former One Direction star with a unique rock-pop sound.', '2025-06-15', '21:00:00', 2209, 119.99, 7),
(8, 'Billie Eilish - Where Do We Go? World Tour', 'Grammy-winning artist performing intimate, haunting tracks.', '2025-07-01', '12:00:00', 1381, 89.99, 8),
(9, 'Ariana Grande - Sweetener World Tour', 'Powerful vocals and breathtaking stage visuals.', '2025-07-15', '20:00:00', 400, 79.99, 9),
(10, 'Ed Sheeran - ÷ Tour', 'Heartfelt ballads and acoustic magic from the global star.', '2025-08-01', '20:00:00', 250, 99.99, 10),
(11, 'Post Malone - Runaway Tour', 'Rap-rock fusion with raw, emotional performances.', '2025-08-15', '20:00:00', 2500, 119.99, 11),
(12, 'Taylor Swift - Lover Fest Tour', 'Pop-country icon with anthems spanning her career.', '2025-09-01', '20:00:00', 1292, 129.99, 12),
(13, 'Lady Gaga - The Chromatica Ball Tour', 'Theatrical pop extravaganza with jaw-dropping visuals.', '2025-09-15', '20:00:00', 9000, 149.99, 13),
(14, 'Justin Bieber - Changes Tour', 'Pop megastar performing his latest R&B-inspired tracks.', '2025-10-01', '20:00:00', 300, 69.99, 14),
(15, 'Shawn Mendes - Wonder Tour', 'Acoustic-driven pop hits from the Canadian singer-songwriter.', '2025-10-15', '20:00:00', 327, 79.99, 15),
(16, 'Camila Cabello - Romance Tour', 'Latin-infused pop from the ex-Fifth Harmony star.', '2025-11-01', '20:00:00', 5000, 99.99, 16),
(17, 'Metallica - M72 World Tour', 'Heavy metal legends performing live', '2025-11-05', '19:30:00', 30000, 189.99, 26),
(18, 'Foo Fighters - Everything or Nothing at All Tour', 'Rock legends with a new album setlist', '2025-11-20', '20:00:00', 12000, 149.99, 1),
(19, 'Beyoncé - Renaissance World Tour', 'Pop and R&B queen in her latest tour', '2025-12-01', '19:00:00', 5000, 199.99, 6),
(20, 'Drake & 21 Savage - Its All A Blur Tour', 'Rap superstars perform their hits', '2025-12-15', '20:00:00', 5000, 179.99, 4),
(21, 'Red Hot Chili Peppers - Global Stadium Tour', 'Funky rock vibes from RHCP', '2026-01-10', '18:30:00', 9000, 159.99, 13),
(22, 'Dream Theater - Top of the World Tour', 'Progressive metal band performing classics', '2026-01-20', '20:00:00', 2500, 129.99, 11),
(23, 'Hans Zimmer Live', 'Legendary composer performing film scores', '2026-02-01', '19:30:00', 2209, 199.99, 7),
(24, 'Slipknot - We Are Not Your Kind Tour', 'Masked metal legends performing live', '2026-02-15', '20:30:00', 12000, 149.99, 1),
(25, 'Eminem - Curtain Call Tour', 'Rap icon performing greatest hits', '2026-03-05', '21:00:00', 12000, 249.99, 1),
(26, 'Arctic Monkeys - The Car Tour', 'Indie rock band with their latest album', '2026-03-18', '20:00:00', 2500, 129.99, 11),
(27, 'Gojira - Fortitude Tour', 'French metal titans on tour', '2026-04-01', '19:00:00', 2378, 99.99, 3),
(28, 'Norah Jones - Pick Me Up Off The Floor Tour', 'Smooth jazz & blues night', '2026-04-15', '20:00:00', 5000, 79.99, 16),
(29, 'Avenged Sevenfold - Life Is But A Dream Tour', 'Metalcore legends performing their best hits', '2026-05-01', '20:30:00', 12000, 139.99, 1),
(30, 'Blackpink - Born Pink Tour', 'K-Pop’s biggest girl group live in concert', '2026-05-10', '19:00:00', 12000, 199.99, 1),
(31, 'The 1975 - Being Funny in a Foreign Language Tour', 'Indie pop band playing new and classic hits', '2026-06-01', '20:00:00', 400, 89.99, 9),
(32, 'Opeth - In Cauda Venenum Tour', 'Progressive death metal legends performing live', '2026-06-15', '19:30:00', 500, 119.99, 44),
(33, 'Paramore - This Is Why Tour', 'Pop-punk icons back on stage', '2026-07-01', '20:00:00', 1292, 109.99, 12),
(34, 'Tool - Fear Inoculum Tour', 'Prog metal giants with an immersive live show', '2026-07-15', '20:00:00', 9000, 159.99, 13),
(35, 'Rammstein - Europe Stadium Tour', 'German industrial metal legends performing live', '2026-08-01', '21:00:00', 30500, 179.99, 26),
(36, 'Martin Garrix - Sentio World Tour', 'Dutch DJ bringing high-energy EDM anthems', '2026-08-15', '22:00:00', 12000, 129.99, 1),
(37, 'Armin van Buuren - A State of Trance 1000', 'Trance legend with an immersive light show', '2026-09-01', '22:30:00', 30500, 149.99, 26),
(38, 'Deadmau5 - We Are Friends Tour', 'Progressive house master with a stunning live set', '2026-09-10', '21:00:00', 9000, 119.99, 13),
(39, 'Subtronics - Cyclops Dome Tour', 'Bass music and dubstep madness', '2026-09-20', '23:00:00', 5000, 99.99, 6),
(40, 'David Guetta - Future Rave Tour', 'French DJ performing his latest club hits', '2026-10-01', '22:30:00', 12000, 149.99, 1),
(41, 'Marshmello - Joytime Tour', 'EDM superstar with a colorful light show', '2026-10-15', '22:00:00', 12000, 129.99, 1),
(42, 'Bill Burr - Slight Return Tour', 'Stand-up comedy legend with his latest material', '2026-10-25', '19:30:00', 2378, 89.99, 3),
(43, 'Kevin Hart - Reality Check Tour', 'One of the biggest names in comedy live on stage', '2026-11-01', '20:00:00', 5000, 159.99, 4),
(44, 'Dave Chappelle - No Phones Allowed Tour', 'Master of comedy performing his latest set', '2026-11-10', '21:00:00', 2209, 199.99, 7),
(45, 'Trevor Noah - Off The Record Tour', 'South African comedian delivering smart humor', '2026-11-15', '20:00:00', 5000, 129.99, 6),
(46, 'Jim Jefferies - Give Em What They Want Tour', 'Dark humor and sharp wit from the Aussie comedian', '2026-12-01', '19:30:00', 400, 79.99, 9),
(47, 'Lindsey Stirling - Artemis Tour', 'Violin meets electronic beats in a stunning live show', '2026-12-10', '19:30:00', 1292, 99.99, 12),
(48, 'John Williams in Concert', 'Legendary film composer conducts live orchestra', '2027-01-05', '19:00:00', 2209, 249.99, 7),
(49, 'Yo-Yo Ma - The Bach Project', 'Classical cello virtuoso performing masterworks', '2027-01-15', '19:30:00', 5000, 159.99, 16),
(50, 'Snarky Puppy - Empire Central Tour', 'Grammy-winning jazz fusion group live in concert', '2027-02-01', '20:00:00', 1381, 89.99, 8),
(51, 'Jacob Collier - Djesse World Tour', 'Musical prodigy with a multi-instrumental live set', '2027-02-10', '20:00:00', 2378, 119.99, 3),
(52, 'Chris Botti - An Evening of Jazz', 'Smooth jazz trumpet performances with a stellar band', '2027-02-20', '19:30:00', 5000, 109.99, 6),
(53, 'Hans Zimmer - Live in Concert', 'Iconic film composer brings Hollywood music to life', '2027-03-01', '19:00:00', 9000, 199.99, 13),
(54, 'Tiësto - Drive Tour', 'EDM icon performing festival anthems', '2027-03-15', '22:30:00', 12000, 139.99, 1),
(55, 'Pendulum - Trinity Live', 'Drum & bass legends with a high-energy performance', '2027-03-25', '21:00:00', 5000, 99.99, 6);
SET IDENTITY_INSERT concerts.tblConcerts OFF;

SET IDENTITY_INSERT concerts.tblGenres ON;
INSERT  INTO concerts.tblGenres (genreId, genreName, genreDescription)
VALUES                
(1, 'Comedy', 'Live stand-up performances by renowned comedians.'),
(2, 'Pop', 'Mainstream popular music from global artists.'),
(3, 'Rock', 'A broad genre encompassing classic rock, alternative, and hard rock bands.'),
(4, 'Alternative Rock', 'A subgenre of rock with a more indie and experimental approach.'),
(5, 'R&B', 'Rhythm and blues music with soulful vocals and grooves.'),
(6, 'Electropop', 'A fusion of electronic music and pop elements.'),
(7, 'Jazz', 'A diverse genre including traditional, smooth, and fusion jazz.'),
(8, 'Hip-Hop/Rap', 'Rhythmic music with rap lyrics and beats.'),
(9, 'Country', 'Music with roots in folk, blues, and Americana.'),
(10, 'EDM', 'Electronic Dance Music, including house, trance, and dubstep.'),
(11, 'Metal', 'Heavy guitar-driven music, including subgenres like thrash and metalcore.'),
(12, 'Progressive Metal', 'A fusion of metal with complex compositions and technical playing.'),
(13, 'K-Pop', 'Korean pop music blending dance, rap, and electronic styles.'),
(14, 'Indie Rock', 'Independent rock music with unique and non-mainstream elements.'),
(15, 'Classical', 'Orchestral and instrumental compositions by legendary composers.'),
(16, 'Film Score', 'Live performances of iconic movie soundtracks.'),
(17, 'Funk', 'Groove-based music with rhythmic basslines and soulful melodies.'),
(18, 'Trance', 'A high-energy electronic music genre with hypnotic beats.'),
(19, 'Industrial Metal', 'A fusion of metal and electronic music with intense, mechanical sounds.'),
(20, 'Drum & Bass', 'Fast-paced electronic music with heavy bass and breakbeats.');
SET IDENTITY_INSERT concerts.tblGenres OFF;

INSERT  INTO concerts.tblConcertGenres (concertId, genreId)
VALUES                       
(1, 1),
(42, 1),
(43, 1),
(44, 1),
(45, 1),
(46, 1),
(2, 2),
(6, 2),
(7, 2),
(9, 2),
(10, 2),
(12, 2),
(14, 2),
(15, 2),
(16, 2),
(19, 2),
(30, 2),
(31, 2),
(3, 3),
(4, 3),
(17, 3),
(18, 3),
(21, 3),
(26, 3),
(33, 3),
(34, 3),
(35, 3),
(4, 5),
(8, 5),
(11, 8),
(20, 8),
(25, 8),
(27, 11),
(17, 11),
(22, 12),
(24, 11),
(29, 11),
(32, 12),
(34, 12),
(35, 19),
(28, 7),
(50, 7),
(51, 7),
(52, 7),
(48, 15),
(49, 15),
(53, 16),
(36, 10),
(37, 18),
(38, 10),
(39, 10),
(40, 10),
(41, 10),
(54, 10),
(55, 20);

COMMIT TRANSACTION;