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

        // add list of Customers on a flight? 


        public Flight(int flightID, string flightNumber, int departureAirportID, int arrivalAirportID,
                      DateTime departureTime, DateTime arrivalTime, int planeModelId, int pointsEarned, int price)
        {
            FlightID = flightID;
            FlightNumber = flightNumber;
            DepartureAirportID = departureAirportID;
            ArrivalAirportID = arrivalAirportID;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            PlaneModelId = planeModelId;
            PointsEarned = pointsEarned;
            Price = price;
        }

        public static Flight CreateFlight(string flightNumber, int departureAirportID, int arrivalAirportID,
                                         DateTime departureTime, DateTime arrivalTime, int planeModelId, int pointsEarned, int price)
        {
            int flightID = GenerateFlightID();
            string filePath = "C:/Users/Reece/Code/Airline-Management-Software/Airline Software/Tables/FlightDb.csv";

            Flight newFlight = new Flight(flightID, flightNumber, departureAirportID, arrivalAirportID, departureTime, arrivalTime, planeModelId, pointsEarned, price);
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            flights.Add(newFlight);
            CsvDatabase.WriteCsvFile<Flight>(filePath, flights);
            return newFlight;
        }


        // function for auto generating the ID's 
        private static int GenerateFlightID()
        {
            string filePath = "C:/Users/Reece/Code/Airline-Management-Software/Airline Software/Tables/FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            int maxID = flights.Count > 0 ? flights.Max(f => f.FlightID) : 0;
            return maxID + 1;
        }

    }
}
