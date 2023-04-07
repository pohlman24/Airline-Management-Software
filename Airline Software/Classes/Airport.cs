using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Airport
    {
        public int AirportID { get; set; }
        public string City { get; set; }
        public String State { get; set; }
        public string Code { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Airport(int airportID, string city, string state, string code, double latitude, double longitude)
        {
            this.AirportID = airportID;
            this.City = city;
            this.State = state;
            this.Code = code;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public static Airport CreateAirport(int airportID, string city, string state, string code, double latitude, double longitude)
        {
            Airport newAirport = new Airport(airportID, city, state, code, latitude, longitude);
            string filePath = @"..\..\..\Tables\AirportDb.csv";
            List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(filePath);
            airports.Add(newAirport);
            CsvDatabase.WriteCsvFile<Airport>(filePath, airports);

            return newAirport;
        }

        private static int GenerateAirportID()
        {
            string filePath = @"..\..\..\Tables\AirportDb.csv"; ;
            List<Airport> planes = CsvDatabase.ReadCsvFile<Airport>(filePath);
            int maxID = planes.Count > 0 ? planes.Max(p => p.AirportID) : 0;
            return maxID + 1;
        }

    }
}
