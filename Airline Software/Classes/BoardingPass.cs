using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class BoardingPass
    {
        // AKA Boarding pass
        public int BoardingPassId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int FlightId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }

    }
}
