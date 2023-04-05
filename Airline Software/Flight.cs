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
        public string FlightNumber { get; set; }
        public int DepartureAirportID { get; set; }
        public int ArrivalAirportID { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int PlaneModelId { get; set; }
    }
}
