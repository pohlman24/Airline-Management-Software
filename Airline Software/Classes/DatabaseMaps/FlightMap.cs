using CsvHelper.Configuration;
using Airline_Software;

public class FlightMap : ClassMap<Flight>
{
    public FlightMap()
    {
        Map(m => m.FlightId).Index(0).Name("FlightId");
        Map(m => m.FlightNumber).Index(1).Name("FlightNumber");
        Map(m => m.DepartureAirportID).Index(2).Name("DepartureAirportID");
        Map(m => m.ArrivalAirportID).Index(3).Name("ArrivalAirportID");
        Map(m => m.DepartureTime).Index(4).Name("DepartureTime").TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
        Map(m => m.ArrivalTime).Index(5).Name("ArrivalTime").TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
        Map(m => m.PlaneModelId).Index(6).Name("PlaneModelId");
        Map(m => m.Price).Index(7).Name("Price");
        Map(m => m.PointsEarned).Index(8).Name("PointsEarned");
        Map(m => m.Capacity).Index(9).Name("Capacity");
        Map(m => m.SeatsSold).Index(10).Name("SeatsSold");
    }
}
