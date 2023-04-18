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
            // int departureAirportID, int arrivalAirportID, DateTime departureTime
            // could have all the console output here
            Console.WriteLine("***Enter Flight Info***");

            Console.WriteLine("Departure Airport Code: ");
            string departCityCodeInput = Console.ReadLine();
            Airport departAirport = Airport.FindAirportbyCode(departCityCodeInput);

            Console.WriteLine("Arrival Airport Code: ");
            string arrCityCodeInput = Console.ReadLine();
            Airport arrAirport = Airport.FindAirportbyCode(arrCityCodeInput);

            Console.WriteLine("Departure Time: yyyy-mm-dd hh:mm:ss");
            string departTime = Console.ReadLine();

            Flight.CreateFlight(departAirport.AirportId, arrAirport.AirportId, DateTime.Parse(departTime));
        }

        public static void editFlight(Flight flight) //maybe use int flightID here instead, depends on how it's stored
        {
            Flight.UpdateFlight(flight);
            Console.WriteLine("Flight '" + flight.FlightNumber + "' updated");
        }

        public static void CancelFlight(Flight flight) //same as edit
        {
            //remove flight from database, refund customers
            Flight.DeleteFlight(flight);
            Console.WriteLine("Flight '"+ flight.FlightNumber + "' canceled");
        }
    }
}
