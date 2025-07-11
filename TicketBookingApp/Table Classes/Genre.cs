using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBookingApp.Table_Classes;

namespace TicketBookingApp
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public string GenreDescription { get; set; }

        public Genre(int genreId, string genreName, string genreDescription)
        {
            GenreId = genreId;
            GenreName = genreName;
            GenreDescription = genreDescription;
        }

        public ConcertGenre ToConcertGenre(Concert concert)
        {
            return new ConcertGenre(concert.ConcertId, GenreId);
        }

        public ConcertGenre ToConcertGenre(int concertId)
        {
            return new ConcertGenre(concertId, GenreId);
        }
    }
}
