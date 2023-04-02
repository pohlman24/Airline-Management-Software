using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class Flight
    {
        //a list of all passengers that have booked a specific flight
        public List<string[]> passengers = new(); //TODO might not be string[] depends on customer class
        public Plane plane;
    }
}
