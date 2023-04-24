using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class MarketingManager : User
    {
        public MarketingManager(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType) 
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {

        }

        // should this be an automatic thing or a one by one
        public static void AssignPlaneForFlight(Flight flight)
        {
            double distance = Flight.CalcFlightDistance(flight.DepartureAirportID, flight.ArrivalAirportID);
            int id;
            int cap;

            if (distance < 500)
            {
                // boeing 737
                id = 1;
                cap = Plane.FindPlaneById(id).Capacity;
            }
            else if (distance < 1200)
            {
                // boeing 767
                id = 2;
                cap = Plane.FindPlaneById(id).Capacity;
            }
            else
            {
                // boeing 777
                id = 3;
                cap = Plane.FindPlaneById(id).Capacity;

            }

                Flight.UpdateFlight(flight, planeModelId:id, capacity:cap);
        }

        public static void AssignPlaneForAllFlights()
        {
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            foreach (Flight flight in flights)
            {
                double distance = Flight.CalcFlightDistance(flight.DepartureAirportID, flight.ArrivalAirportID);
                int id;
                int cap;

                if (distance < 500)
                {
                    // boeing 737
                    id = 1;
                    cap = Plane.FindPlaneById(id).Capacity;
                }
                else if (distance < 1200)
                {
                    // boeing 767
                    id = 2;
                    cap = Plane.FindPlaneById(id).Capacity;
                }
                else
                {
                    // boeing 777
                    id = 3;
                    cap = Plane.FindPlaneById(id).Capacity;

                }

                Flight.UpdateFlight(flight, planeModelId: id, capacity: cap);
            }
            CsvDatabase.WriteCsvFile(filePath, flights);
        }
    }
}