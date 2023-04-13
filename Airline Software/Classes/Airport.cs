using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Airport
    {
        public int AirportId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Airport(int AirportId, string City, string State, string Code, double Latitude, double Longitude)
        {
            this.AirportId = AirportId;
            this.City = City;
            this.State = State;
            this.Code = Code;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }

        public static Airport CreateAirport(string city, string state, string code, double latitude, double longitude)
        {
            int airportId = GenerateAirportID();
            Airport newAirport = new Airport(airportId, city, state, code, latitude, longitude);
            string filePath = @"..\..\..\Tables\AirportDb.csv";
            List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(filePath);
            airports.Add(newAirport);
            CsvDatabase.WriteCsvFile<Airport>(filePath, airports);

            return newAirport;
        }

        public static void UpdateAirport(Airport airport, string city = "", string state = "", string code = "", double latitude = -1, double longitude = -1)
        {
            // update the object instance if there is a change if value
            airport.City = string.IsNullOrEmpty(city) ? airport.City : city;
            airport.State = string.IsNullOrEmpty(state) ? airport.State: state;
            airport.Code = string.IsNullOrEmpty(code) ? airport.Code : code;
            airport.Latitude = latitude == -1 ? airport.Latitude : latitude;
            airport.Longitude = longitude == -1 ? airport.Longitude : longitude;

            // open the csv table
            string filePath = @"..\..\..\Tables\AirportDb.csv";
            List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(filePath);

            // update the record on the table
            CsvDatabase.UpdateRecord(airports, p => p.AirportId, airport.AirportId, (current, updated) =>
            {
                current.City = updated.City;
                current.State = updated.State;
                current.Code = updated.Code;
                current.Latitude = updated.Latitude;
                current.Longitude = updated.Longitude;
            }, airport);

            CsvDatabase.WriteCsvFile(filePath, airports);
        }

        public static void DeleteAirport(Airport airport)
        {
            string filePath = @"..\..\..\Tables\AirportDb.csv";
            List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(filePath);
            CsvDatabase.RemoveRecord(airports, a => a.AirportId, airport.AirportId);
            CsvDatabase.WriteCsvFile(filePath, airports);
        }

        public static Airport FindAirportbyId(int id)
        {
            string filePath = @"..\..\..\Tables\AirportDb.csv";
            List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(filePath);
            if(CsvDatabase.FindRecord(airports, a => a.AirportId, id) == null)
            {
                throw (new Exception("Id Not Found"));
                return null;
            }
            Airport airport = CsvDatabase.FindRecord(airports, a => a.AirportId, id);
            return airport;
        }
        public static Airport FindAirportbyCode(string code)
        {
            string filePath = @"..\..\..\Tables\AirportDb.csv";
            List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(filePath);
            if (CsvDatabase.FindRecordByString(airports, a => a.Code, code) == null)
            {
                throw (new Exception("Record Not Found"));
                return null;
            }
            Airport airport = CsvDatabase.FindRecordByString(airports, a => a.Code, code);
            return airport;
        }

        private static int GenerateAirportID()
        {
            string filePath = @"..\..\..\Tables\AirportDb.csv";
            List<Airport> airports = CsvDatabase.ReadCsvFile<Airport>(filePath);
            int maxID = airports.Count > 0 ? airports.Max(p => p.AirportId) : 0;
            return maxID + 1;
        }

    }
}
