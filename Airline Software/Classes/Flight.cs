using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Flight
    {
        public int FlightId { get; set; } // not visable to customer
        public string FlightNumber { get; set; } // should be a combination of the departure/arrival city and number -- visable to customer
        public int DepartureAirportID { get; set; }
        public int ArrivalAirportID { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int PlaneModelId { get; set; }
        public int PointsEarned { get; set; }
        public int Price { get; set; }
        public int Capacity { get; set; }
        public int SeatsSold { get; set; }


        // add list of Customers on a flight? 


        public Flight(int FlightId, string FlightNumber, int DepartureAirportID, int ArrivalAirportID,
                      DateTime DepartureTime, DateTime ArrivalTime, int PlaneModelId, int PointsEarned, int Price, int Capacity, int SeatsSold)
        {
            this.FlightId = FlightId;
            this.FlightNumber = FlightNumber;
            this.DepartureAirportID = DepartureAirportID;
            this.ArrivalAirportID = ArrivalAirportID;
            this.DepartureTime = DepartureTime;
            this.ArrivalTime = ArrivalTime;
            this.PlaneModelId = PlaneModelId;
            this.PointsEarned = PointsEarned;
            this.Price = Price;
            this.Capacity = Capacity;
            this.SeatsSold = SeatsSold;
        }

        public static Flight CreateFlight(int departureAirportID, int arrivalAirportID,
                                         DateTime departureTime, DateTime arrivalTime, int planeModelId, int pointsEarned, int price)
        {
            int flightID = GenerateFlightID();
            string flightNumber = GenerateFlightNumber(flightID, departureAirportID, arrivalAirportID);
            int capacity = Plane.FindPlaneById(planeModelId).Capacity;

            string filePath = @"..\..\..\Tables\FlightDb.csv";

            Flight newFlight = new Flight(flightID, flightNumber, departureAirportID, arrivalAirportID, departureTime, arrivalTime, planeModelId, pointsEarned, price, capacity, 0);
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            flights.Add(newFlight);
            CsvDatabase.WriteCsvFile<Flight>(filePath, flights);
            return newFlight;
        }

        public static void UpdateFlight(Flight flight, string flightNumber = "", int departureAirportID = -1, int arrivalAirportID = -1,
                                DateTime? departureTime = null, DateTime? arrivalTime = null, int planeModelId = -1,
                                int pointsEarned = -1, int price = -1, int capacity = -1, int seatsSold = -1)
        {
            flight.FlightNumber = string.IsNullOrEmpty(flightNumber) ? flight.FlightNumber : flightNumber;
            flight.DepartureAirportID = departureAirportID == -1 ? flight.DepartureAirportID : departureAirportID;
            flight.ArrivalAirportID = arrivalAirportID == -1 ? flight.ArrivalAirportID : arrivalAirportID;
            flight.DepartureTime = departureTime == null ? flight.DepartureTime : departureTime.Value;
            flight.ArrivalTime = arrivalTime == null ? flight.ArrivalTime : arrivalTime.Value;
            flight.PlaneModelId = planeModelId == -1 ? flight.PlaneModelId : planeModelId;
            flight.PointsEarned = pointsEarned == -1 ? flight.PointsEarned : pointsEarned;
            flight.Price = price == -1 ? flight.Price : price;
            flight.Capacity = capacity == -1 ? (int)capacity : capacity;
            flight.SeatsSold = seatsSold == -1 ? flight.SeatsSold : seatsSold;

            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);

            CsvDatabase.UpdateRecord(flights, f => f.FlightId, flight.FlightId, (current, updated) =>
            {
                current.FlightNumber = updated.FlightNumber;
                current.DepartureAirportID = updated.DepartureAirportID;
                current.ArrivalAirportID = updated.ArrivalAirportID;
                current.DepartureTime = updated.DepartureTime;
                current.ArrivalTime = updated.ArrivalTime;
                current.PlaneModelId = updated.PlaneModelId;
                current.PointsEarned = updated.PointsEarned;
                current.Price = updated.Price;
                current.Capacity = updated.Capacity;
                current.SeatsSold = updated.SeatsSold;
            }, flight);

            CsvDatabase.WriteCsvFile(filePath, flights);
        }

        public static void DeleteFlight(Flight flight)
        {
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            CsvDatabase.RemoveRecord(flights, f => f.FlightId, flight.FlightId);
            CsvDatabase.WriteCsvFile(filePath, flights);
        }

        public static Flight FindFlightById(int id)
        {
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            if (CsvDatabase.FindRecord(flights, f => f.FlightId, id) == null)
            {
                throw (new Exception("Id Not Found"));
                return null;
            }
            Flight flight = CsvDatabase.FindRecord(flights, f => f.FlightId, id);
            return flight;
        }



        // function for auto generating the ID's 
        private static int GenerateFlightID()
        {
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            int maxID = flights.Count > 0 ? flights.Max(f => f.FlightId) : 0;
            return maxID + 1;
        }

        // this will be the number visable to the customer
        private static string GenerateFlightNumber(int flightId, int departId, int arrId)
        {
            Airport departLocation = Airport.FindAirportbyId(departId);
            Airport arrivialLocation = Airport.FindAirportbyId(arrId);
            string departCode = departLocation.Code;
            string arrCode = arrivialLocation.Code;
            string flightNum = departCode + arrCode + flightId;
            return flightNum;

        }

    }
}
