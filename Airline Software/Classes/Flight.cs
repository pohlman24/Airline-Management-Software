﻿using System;
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
        public int? PlaneModelId { get; set; }
        public double Price { get; set; }
        public int PointsEarned { get; set; }
        public int Capacity { get; set; }
        public int SeatsSold { get; set; }
        public List<Flight> LayoverFlights { get; set; }
        public int ParentConnectionFlight { get; set; }
        public string FlightInfo { get; set; } 

        private const double MaxDirectFlightDistance = 1250; // in miles


        public Flight(int FlightId, string FlightNumber, int DepartureAirportID, int ArrivalAirportID,
                      DateTime DepartureTime, DateTime ArrivalTime, double Price, int PlaneModelId, int PointsEarned, int Capacity, int SeatsSold, int ParentConnectionFlight = -1, string FlightInfo = "Direct")
        {
            this.FlightId = FlightId;
            this.FlightNumber = FlightNumber;
            this.DepartureAirportID = DepartureAirportID;
            this.ArrivalAirportID = ArrivalAirportID;
            this.DepartureTime = DepartureTime;
            this.ArrivalTime = ArrivalTime;
            this.PlaneModelId = PlaneModelId;
            this.Price = Price;
            this.PointsEarned = PointsEarned;
            this.Capacity = Capacity;
            this.SeatsSold = SeatsSold;
            this.LayoverFlights = new List<Flight>();
            this.ParentConnectionFlight = ParentConnectionFlight;
            this.FlightInfo = FlightInfo;
        }

        // outputs list of flights without assigned plane
        public static void FlightWithNoPlane()
        {
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            Console.WriteLine("{0, -18} {1, -18} {2, -18} {3, -25} {4, -25}", "Flight Number", "Departure City",
                "Arrival City", "Departure Time", "Est Arrival Time");
            foreach (Flight flight in flights)
            {
                if (flight.PlaneModelId == -1)
                {
                    Console.WriteLine("{0, -18} {1, -18} {2, -18} {3, -25} {4, -25}",
                    flight.FlightNumber, Airport.FindAirportbyId(flight.DepartureAirportID).City,
                    Airport.FindAirportbyId(flight.ArrivalAirportID).City, flight.DepartureTime.ToString("MM/dd/yyyy HH:mm:ss"), flight.ArrivalTime.ToString("MM/dd/yyyy HH:mm:ss"));
                }
            }
        }

        public static Flight CreateFlight(int departureAirportID, int arrivalAirportID,
                                         DateTime departureTime, int parentConnectionFlight = -1, string flightInfo = "direct")
        {
            double distance = CalcFlightDistance(departureAirportID, arrivalAirportID);

            int flightID = GenerateFlightID();
            string flightNumber = GenerateFlightNumber(flightID, departureAirportID, arrivalAirportID);
            DateTime arrivalTime = CalculateArrivalTime(departureTime, departureAirportID, arrivalAirportID);
            double price = CalculateFlightPrice(departureTime, arrivalTime, departureAirportID, arrivalAirportID);
            int pointsEarned = CalculateFlightPoints(price);

            string filePath = @"..\..\..\Tables\FlightDb.csv";
            Flight newFlight = new Flight(flightID, flightNumber, departureAirportID, arrivalAirportID, departureTime, arrivalTime, price,-1, pointsEarned, 0, 0, ParentConnectionFlight: parentConnectionFlight, FlightInfo:flightInfo);

            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            flights.Add(newFlight);
            CsvDatabase.WriteCsvFile<Flight>(filePath, flights);

            Flight currentFlight = FindFlightByFlightNumber(flightNumber);
            if (distance > MaxDirectFlightDistance && flightInfo != "connection")
            {
                // layover stuff 
                GenerateLayovers(currentFlight, departureAirportID, arrivalAirportID, departureTime);
                //DeleteFlight(newFlight);
                UpdateFlight(newFlight, flightInfo: "parent");
            }

            
            return newFlight;
        }

        public static List<Flight> getLayoverFlights(Flight Parentflight)
        {
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            List<Flight> layovers = new List<Flight>();
            foreach (Flight flight in flights)
            {
                if (flight.ParentConnectionFlight == Parentflight.FlightId)
                {
                    layovers.Add(flight);
                }
            }
            return layovers;
        }

        public void PopulateLayovers()
        {
            // if the flight has already had its flight populated
            if (this.LayoverFlights.Count >= 2)
            {
                return;
            }

            List<Flight> layoverFlights = getLayoverFlights(this);
            // populate layovers
            foreach (Flight flight in layoverFlights)
            {
                this.LayoverFlights.Add(flight);
            }
        }


        public static void UpdateFlight(Flight flight, string flightNumber = "", int departureAirportID = -1, int arrivalAirportID = -1,
                                DateTime? departureTime = null, int planeModelId = -1, int capacity = -1, int seatsSold = -1, string flightInfo ="")
        {
            
            flight.DepartureAirportID = departureAirportID == -1 ? flight.DepartureAirportID : departureAirportID;
            flight.ArrivalAirportID = arrivalAirportID == -1 ? flight.ArrivalAirportID : arrivalAirportID;
            flight.DepartureTime = departureTime == null ? flight.DepartureTime : departureTime.Value;
            flight.ArrivalTime = (departureTime == null && departureAirportID == -1 && arrivalAirportID == -1) ? flight.ArrivalTime : CalculateArrivalTime(flight.DepartureTime, flight.DepartureAirportID, flight.ArrivalAirportID); //if changing depart time or either airport, must change arrival time
            flight.PlaneModelId = planeModelId == -1 ? flight.PlaneModelId : planeModelId;
            flight.Price = (departureTime == null && departureAirportID == -1 && arrivalAirportID == -1) ? flight.Price : CalculateFlightPrice(flight.DepartureTime, flight.ArrivalTime, flight.DepartureAirportID, flight.ArrivalAirportID);
            flight.PointsEarned = (departureTime == null && departureAirportID == -1 && arrivalAirportID == -1) ? flight.PointsEarned : CalculateFlightPoints(flight.Price);
            flight.Capacity = capacity == -1 ? flight.Capacity : capacity;
            flight.SeatsSold = seatsSold == -1 ? flight.SeatsSold : seatsSold;
            flight.FlightInfo = string.IsNullOrEmpty(flightInfo) ? flight.FlightInfo : flightInfo;
            // flight number is going to by default re-generate the flight number so that if the user changes the depart/arr city it will be the correct ID
            flight.FlightNumber = string.IsNullOrEmpty(flightNumber) ? GenerateFlightNumber(flight.FlightId, flight.DepartureAirportID, flight.ArrivalAirportID) : flightNumber;
            if (flightInfo != null)
            {
                flight.PopulateLayovers();
                foreach (Flight layover in flight.LayoverFlights)
                {
                    flight.Price += 8;
                }
            }

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
                current.Price = updated.Price;
                current.PointsEarned = updated.PointsEarned;
                current.Capacity = updated.Capacity;
                current.SeatsSold = updated.SeatsSold;
                current.FlightInfo = updated.FlightInfo;
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

        private static List<Flight> GenerateLayovers(Flight flight, int departureAirportID, int arrivalAirportID, DateTime departureTime)
        {
            Airport layoverAirport = FindLayoverAirport(departureAirportID, arrivalAirportID);
            DateTime layoverDepartureTime = departureTime.Add(CalculateFlightTime(departureAirportID, layoverAirport.AirportId)).AddMinutes(25);
            
            Flight firstLeg = CreateFlight(departureAirportID, layoverAirport.AirportId, departureTime, parentConnectionFlight:flight.FlightId, flightInfo:"connection");
            flight.LayoverFlights.Add(firstLeg);
            Flight secondLeg = CreateFlight(layoverAirport.AirportId, arrivalAirportID, layoverDepartureTime, parentConnectionFlight: flight.FlightId, flightInfo:"connection");
            flight.LayoverFlights.Add(secondLeg);
            return new List<Flight> { firstLeg, secondLeg };
        }


        public static Airport FindLayoverAirport(int departureAirportID, int arrivalAirportID)
        {
            string filePath = @"..\..\..\Tables\AirportDb.csv";
            List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(filePath);

            Airport departureAirport = Airport.FindAirportbyId(departureAirportID);
            Airport arrivalAirport = Airport.FindAirportbyId(arrivalAirportID);

            double midLatitude = (departureAirport.Latitude + arrivalAirport.Latitude) / 2;
            double midLongitude = (departureAirport.Longitude + arrivalAirport.Longitude) / 2;

            Airport bestLayoverAirport = null;
            double minDistance = double.MaxValue;

            foreach (Airport airport in airports)
            {
                if (airport.AirportId == departureAirportID || airport.AirportId == arrivalAirportID)
                {
                    continue; // Skip departure and arrival airports
                }

                double distance = Math.Sqrt(Math.Pow(airport.Latitude - midLatitude, 2) + Math.Pow(airport.Longitude - midLongitude, 2));

                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestLayoverAirport = airport;
                }
            }

            return bestLayoverAirport;
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
        public static Flight FindFlightByFlightNumber(string num)
        {
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);
            if (CsvDatabase.FindRecordByString(flights, f => f.FlightNumber, num) == null)
            {
                throw (new Exception("Record Not Found"));
                return null;
            }
            Flight flight = CsvDatabase.FindRecordByString(flights, f => f.FlightNumber, num);
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

        // this will be the number visible to the customer
        private static string GenerateFlightNumber(int flightId, int departId, int arrId)
        {
            Airport departLocation = Airport.FindAirportbyId(departId);
            Airport arrivialLocation = Airport.FindAirportbyId(arrId);
            string departCode = departLocation.Code;
            string arrCode = arrivialLocation.Code;
            string flightNum = departCode + arrCode + flightId;
            return flightNum;
        }

        // generate a list of all flights 
        public static void ShowAllFlights()
        {
            string filePath = @"..\..\..\Tables\FlightDb.csv";
            List<Flight> flights = CsvDatabase.ReadCsvFile<Flight>(filePath);

            Console.WriteLine("\n{0, -18} {1, -18} {2, -18} {3, -25} {4, -25} {5, -18} {6, -18} {7}", "Flight Number","Departure City",
                "Arrival City", "Departure Time", "Est Arrival Time", "Price", "Points Value", "Seats Sold/Capacity");
            foreach (Flight flight in flights)
            {
                string flightNumber = flight.FlightNumber;
                string departAirport = Airport.FindAirportbyId(flight.DepartureAirportID).City;
                string arrivalAirport = Airport.FindAirportbyId(flight.ArrivalAirportID).City;
                DateTime departTime = flight.DepartureTime;
                DateTime arrivalTime = flight.ArrivalTime;
                double price = flight.Price;
                int points = flight.PointsEarned;
                int capacity = flight.Capacity;
                int seatsSold = flight.SeatsSold;
                Console.WriteLine("{0, -18} {1, -18} {2, -18} {3, -25} {4, -25} ${5, -17} {6, -18} {7}/{8}",
                    flightNumber, departAirport, arrivalAirport, departTime.ToString("MM/dd/yyyy HH:mm:ss"), arrivalTime.ToString("MM/dd/yyyy HH:mm:ss"), price, points, seatsSold, capacity);
            }
        }

        // generate summary for a selected flight
        public void FlightSummary()
        {
            if(this.FlightInfo == "connection")
            {
                Console.WriteLine("\n{0, -18} {1, -18} {2, -18} {3, -25} {4, -25}",
                                "Flight Number", "Departure City", "Arrival City", "Departure Time", "Est Arrival Time");

                Console.WriteLine("{0, -18} {1, -18} {2, -18} {3, -25} {4, -25}",
                        this.FlightNumber, Airport.FindAirportbyId(this.DepartureAirportID).City, Airport.FindAirportbyId(this.ArrivalAirportID).City,
                        this.DepartureTime, this.ArrivalTime);
            }
            else if(this.FlightInfo == "parent")
            {
                this.PopulateLayovers();
                Console.WriteLine("\n{0, -18} {1, -18} {2, -18} {3, -25} {4, -25} {5, -12} {6}",
                "Flight Number", "Departure City", "Arrival City", "Departure Time", "Est Arrival Time", "Price", "Points Value");

                Console.WriteLine("{0, -18} {1, -18} {2, -18} {3, -25} {4, -25} ${5, -11} {6}",
                        this.FlightNumber, Airport.FindAirportbyId(this.DepartureAirportID).City, Airport.FindAirportbyId(this.ArrivalAirportID).City,
                        this.DepartureTime, this.LayoverFlights[1].ArrivalTime, this.Price, this.PointsEarned);
            }
            else
            {
                Console.WriteLine("\n{0, -18} {1, -18} {2, -18} {3, -25} {4, -25} {5, -12} {6}",
                "Flight Number", "Departure City", "Arrival City", "Departure Time", "Est Arrival Time", "Price", "Points Value");

                Console.WriteLine("{0, -18} {1, -18} {2, -18} {3, -25} {4, -25} ${5, -11} {6}",
                        this.FlightNumber, Airport.FindAirportbyId(this.DepartureAirportID).City, Airport.FindAirportbyId(this.ArrivalAirportID).City,
                        this.DepartureTime, this.ArrivalTime, this.Price, this.PointsEarned);
            }
        }

        public static double CalcFlightDistance(int departureAirportID, int arrivalAirportID)
        {
            Airport departLocation = Airport.FindAirportbyId(departureAirportID);
            Airport arrivalLocation = Airport.FindAirportbyId(arrivalAirportID);
            double departLat = departLocation.Latitude;
            double departLong = departLocation.Longitude;
            double attLat = arrivalLocation.Latitude;
            double attLong = arrivalLocation.Longitude;
            return CalculateDistance(departLat, departLong, attLat, attLong);
        }

        // helper function to calcualte flight distance
        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusMiles = 3958.8; // Earth's radius in miles

            // Convert latitude and longitude from degrees to radians
            double lat1Rad = DegreesToRadians(lat1);
            double lon1Rad = DegreesToRadians(lon1);
            double lat2Rad = DegreesToRadians(lat2);
            double lon2Rad = DegreesToRadians(lon2);

            // Apply the Haversine formula
            double latDiff = lat2Rad - lat1Rad;
            double lonDiff = lon2Rad - lon1Rad;

            double a = Math.Pow(Math.Sin(latDiff / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(lonDiff / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance in miles
            return EarthRadiusMiles * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static TimeSpan CalculateFlightTime(int departureAirportID, int arrivalAirportID)
        {
            const double milesPerMinute = 500.0 / 60.0; //flight speed
            double distance = CalcFlightDistance(departureAirportID, arrivalAirportID);
            int minutes = 30; //gate to runway and vice versa
            minutes += (int)(distance / (milesPerMinute));
            return new TimeSpan(0, minutes, 0);
        }

        public static DateTime CalculateArrivalTime(DateTime departureTime, int departureAirportID, int arrivalAirportID)
        {
            return departureTime.Add(CalculateFlightTime(departureAirportID, arrivalAirportID));
        }

        public static double CalculateFlightPrice(DateTime departureTime, DateTime arrivalTime, int departureAirportID, int arrivalAirportID)
        {
            TimeSpan start10Discount = new(19, 0, 0);
            TimeSpan end10Discount = new(8, 0, 0);
            TimeSpan end20Discount = new(5, 0, 0);
            TimeSpan dayEnd = TimeSpan.FromHours(24);
            TimeSpan dayStart = TimeSpan.Zero;
            double price = 50.0; //fixed costs plus takeoff
            price += 0.12 * CalcFlightDistance(departureAirportID, arrivalAirportID); //12 cents per mile

            if ((departureTime.TimeOfDay >= dayStart && departureTime.TimeOfDay <= end20Discount) || (arrivalTime.TimeOfDay >= dayStart && arrivalTime.TimeOfDay <= end20Discount))
            {
                price *= 0.8; //20 percent discount
            }
            else if (((departureTime.TimeOfDay >= start10Discount && departureTime.TimeOfDay <= dayEnd) || (arrivalTime.TimeOfDay >= start10Discount && arrivalTime.TimeOfDay <= dayEnd)) || 
                ((departureTime.TimeOfDay >= dayStart && departureTime.TimeOfDay <= end10Discount) || (arrivalTime.TimeOfDay >= dayStart && arrivalTime.TimeOfDay <= end10Discount)))
            {
                price *= 0.9; //10 percent discount
            }

            return Math.Round(price,2);
        }

        public static int CalculateFlightPoints(double price)
        {
            return (int)(10 * price);
        }
    }
}
