using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class LoadEngineer : Admin
    {
        public LoadEngineer(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType) : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {
        }





        /* public LoadEngineer()
{
    //position for load engineer is 2
    setID(2);
}*/

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
