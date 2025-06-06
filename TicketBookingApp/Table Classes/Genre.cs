using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
