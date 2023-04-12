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

        public BoardingPass(int BoardingPassId, int OrderId, int CustomerId, int FlightId,
                            string FirstName, string LastName, DateTime DepartureTime, DateTime ArrivalTime,
                            int DepartureAirportId, int ArrivalAirportId)
        {
            this.BoardingPassId = BoardingPassId;
            this.OrderId = OrderId;
            this.CustomerId = CustomerId;
            this.FlightId = FlightId;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.DepartureTime = DepartureTime;
            this.ArrivalTime = ArrivalTime;
            this.DepartureAirportId = DepartureAirportId;
            this.ArrivalAirportId = ArrivalAirportId;
        }

        public static BoardingPass CreateBoardingPass(int orderId, int customerId, int flightId,
                                                      string firstName, string lastName, DateTime departureTime, DateTime arrivalTime,
                                                      int departureAirportId, int arrivalAirportId)
        {
            int boardingPassId = GenerateBoardingPassID();
            BoardingPass newBoardingPass = new BoardingPass(boardingPassId, orderId, customerId, flightId, firstName, lastName, departureTime, arrivalTime, departureAirportId, arrivalAirportId);

            string filePath = @"..\..\..\Tables\BoardingPassDb.csv";
            List<BoardingPass> boardingPasses = CsvDatabase.ReadCsvFile<BoardingPass>(filePath);
            boardingPasses.Add(newBoardingPass);
            CsvDatabase.WriteCsvFile<BoardingPass>(filePath, boardingPasses);

            return newBoardingPass;
        }

        public static void UpdateBoardingPass(BoardingPass boardingPass, int orderId = -1, int customerId = -1, int flightId = -1,
                                      string firstName = "", string lastName = "", DateTime? departureTime = null, DateTime? arrivalTime = null,
                                      int departureAirportId = -1, int arrivalAirportId = -1)
        {
            boardingPass.OrderId = orderId == -1 ? boardingPass.OrderId : orderId;
            boardingPass.CustomerId = customerId == -1 ? boardingPass.CustomerId : customerId;
            boardingPass.FlightId = flightId == -1 ? boardingPass.FlightId : flightId;
            boardingPass.FirstName = string.IsNullOrEmpty(firstName) ? boardingPass.FirstName : firstName;
            boardingPass.LastName = string.IsNullOrEmpty(lastName) ? boardingPass.LastName : lastName;
            boardingPass.DepartureTime = departureTime == null ? boardingPass.DepartureTime : departureTime.Value;
            boardingPass.ArrivalTime = arrivalTime == null ? boardingPass.ArrivalTime : arrivalTime.Value;
            boardingPass.DepartureAirportId = departureAirportId == -1 ? boardingPass.DepartureAirportId : departureAirportId;
            boardingPass.ArrivalAirportId = arrivalAirportId == -1 ? boardingPass.ArrivalAirportId : arrivalAirportId;

            string filePath = @"..\..\..\Tables\BoardingPassDb.csv";
            List<BoardingPass> boardingPasses = CsvDatabase.ReadCsvFile<BoardingPass>(filePath);

            CsvDatabase.UpdateRecord(boardingPasses, b => b.BoardingPassId, boardingPass.BoardingPassId, (current, updated) =>
            {
                current.OrderId = updated.OrderId;
                current.CustomerId = updated.CustomerId;
                current.FlightId = updated.FlightId;
                current.FirstName = updated.FirstName;
                current.LastName = updated.LastName;
                current.DepartureTime = updated.DepartureTime;
                current.ArrivalTime = updated.ArrivalTime;
                current.DepartureAirportId = updated.DepartureAirportId;
                current.ArrivalAirportId = updated.ArrivalAirportId;
            }, boardingPass);

            CsvDatabase.WriteCsvFile(filePath, boardingPasses);
        }

        public static void DeleteBoardingPass(BoardingPass boardingPass)
        {
            string filePath = @"..\..\..\Tables\BoardingPassDb.csv";
            List<BoardingPass> boardingPasses = CsvDatabase.ReadCsvFile<BoardingPass>(filePath);
            CsvDatabase.RemoveRecord(boardingPasses, b => b.BoardingPassId, boardingPass.BoardingPassId);
            CsvDatabase.WriteCsvFile(filePath, boardingPasses);
        }

        public static BoardingPass FindBoardingPassById(int id)
        {
            string filePath = @"..\..\..\Tables\BoardingPassDb.csv";
            List<BoardingPass> boardingPasses = CsvDatabase.ReadCsvFile<BoardingPass>(filePath);
            if (CsvDatabase.FindRecord(boardingPasses, b => b.BoardingPassId, id) == null)
            {
                throw (new Exception("Id Not Found"));
                return null;
            }
            BoardingPass boardingPass = CsvDatabase.FindRecord(boardingPasses, b => b.BoardingPassId, id);
            return boardingPass;
        }


        private static int GenerateBoardingPassID()
        {
            string filePath = @"..\..\..\Tables\BoardingPassDb.csv"; ;
            List<BoardingPass> planes = CsvDatabase.ReadCsvFile<BoardingPass>(filePath);
            int maxID = planes.Count > 0 ? planes.Max(p => p.BoardingPassId) : 0;
            return maxID + 1;
        }
    }
}
