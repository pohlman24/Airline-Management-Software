using CsvHelper.Configuration;
using Airline_Software;

public class BoardingPassMap : ClassMap<BoardingPass>
{
    public BoardingPassMap()
    {
        Map(m => m.BoardingPassId).Index(0).Name("BoardingPassId");
        Map(m => m.OrderId).Index(1).Name("OrderId");
        Map(m => m.CustomerId).Index(2).Name("CustomerId");
        Map(m => m.FlightId).Index(3).Name("FlightId");
        Map(m => m.FirstName).Index(4).Name("FirstName");
        Map(m => m.LastName).Index(5).Name("LastName");
        Map(m => m.DepartureTime).Index(6).Name("DepartureTime").TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
        Map(m => m.ArrivalTime).Index(7).Name("ArrivalTime").TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
        Map(m => m.DepartureAirportId).Index(8).Name("DepartureAirportId");
        Map(m => m.ArrivalAirportId).Index(9).Name("ArrivalAirportId");
    }
}
