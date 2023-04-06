using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class BoardingPass
    {
        // was called ticket previsouly 
        public int BoardingPassId { get; set; }
        public int OrderId { get; set; } // remove?? 
        public int CustomerId { get; set; }
        public int FlightId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }

        public BoardingPass(int boardingPassId, int orderId, int customerId, int flightId,
                            string firstName, string lastName, DateTime departureTime, DateTime arrivalTime,
                            int departureAirportId, int arrivalAirportId)
        {
            BoardingPassId = boardingPassId;
            OrderId = orderId;
            CustomerId = customerId;
            FlightId = flightId;
            FirstName = firstName;
            LastName = lastName;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            DepartureAirportId = departureAirportId;
            ArrivalAirportId = arrivalAirportId;
        }

        public static BoardingPass CreateBoardingPass(int boardingPassId, int orderId, int customerId, int flightId,
                                                      string firstName, string lastName, DateTime departureTime, DateTime arrivalTime,
                                                      int departureAirportId, int arrivalAirportId)
        {
            BoardingPass newBoardingPass = new BoardingPass(boardingPassId, orderId, customerId, flightId, firstName, lastName, departureTime, arrivalTime, departureAirportId, arrivalAirportId);

            string filePath = "C:/Users/Reece/Code/Airline-Management-Software/Airline Software/Tables/BoardingPassDb.csv";
            List<BoardingPass> boardingPasses = CsvDatabase.ReadCsvFile<BoardingPass>(filePath);
            boardingPasses.Add(newBoardingPass);
            CsvDatabase.WriteCsvFile<BoardingPass>(filePath, boardingPasses);

            return newBoardingPass;
        }
    }
}
