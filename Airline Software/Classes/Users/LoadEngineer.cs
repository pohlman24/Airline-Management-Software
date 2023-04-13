using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class LoadEngineer : User
    {
        public LoadEngineer(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType) : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {
        }

        public static void scheduleFlight()
        {
            // (int departureAirportID, int arrivalAirportID, DateTime departureTime, DateTime arrivalTime, double price, int pointsEarned)
            // could have all the console output here
            Console.WriteLine("***Enter Flight Info***");

            Console.WriteLine("Departure Airport Code: ");
            string departCityCode = Console.ReadLine();
            Airport departAirport = Airport.FindAirportbyCode(departCityCode);

            Console.WriteLine("Arrival Airport Code: ");
            string arrCityCode = Console.ReadLine();
            Airport arrAirport = Airport.FindAirportbyCode(arrCityCode);

            Console.WriteLine("Departure Time: ");
            string departTime = Console.ReadLine();

            Console.WriteLine("Arrival Time: ");
            string arrTime = Console.ReadLine();

            
            // need to auto calc price and points earned and arrivalTime 
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
