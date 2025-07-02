using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingApp.Table_Classes
{
    public class ConcertGenre
    {
        public int ConcertId { get; set; }
        public int GenreId { get; set; }

        public ConcertGenre(int concertId, int genreId)
        {
            ConcertId = concertId;
            GenreId = genreId;
        }
    }
}
