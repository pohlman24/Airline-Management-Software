using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class LoadEngineer : Admin
    {
        public LoadEngineer()
        {
            //position for load engineer is 2
            setID(2);
        }

        public void scheduleFlight()
        {
            //need to flesh out flight class before this can be done
        }

        public void editFlight(Flight flight) //maybe use int flightID here instead, depends on how it's stored
        {

        }

        public void cancelFlight(Flight flight) //same as edit
        {
            //remove flight from database, refund customers
        }
    }
}
