using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Flight
    {
        public int FlightID { get; set; }
        public string FlightNumber { get; set; } // should be a combination of the departure/arrival city and number 
        public int DepartureAirportID { get; set; }
        public int ArrivalAirportID { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int PlaneModelId { get; set; }
        public int PointsEarned { get; set; }
        public int Price { get; set; }

        // we may want to include the non-id forms of these too for easier access when needed -- but for right now this is good
    }
}
