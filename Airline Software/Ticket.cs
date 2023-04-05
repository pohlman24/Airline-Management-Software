using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Ticket
    {
        public Customer Customer { get; set; }
        public Flight Flight { get; set; }
        public float Price { get; set; }
        public bool IsRoundTrip { get; set; }
        public DateTime DepartureDate { get; set; }
        public int PointsEarned { get; set; }
        public string TicketStatus { get; set; }

    }
}
