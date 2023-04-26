using CsvHelper.Configuration;
using Airline_Software;

public class OrderMap : ClassMap<Order>
{
    public OrderMap()
    {
        Map(m => m.OrderId).Name("OrderId").Index(0);
        Map(m => m.CustomerId).Name("CustomerId").Index(1);
        Map(m => m.FlightId1).Name("FlightId1").Index(2);
        Map(m => m.FlightId2).Name("FlightId2").Index(3);
        Map(m => m.OrderStatus).Name("OrderStatus").Index(4);
        Map(m => m.OrderDate).Name("OrderDate").TypeConverterOption.Format("yyyy-MM-dd").Index(5);
        Map(m => m.CancellationDate).Name("CancellationDate").TypeConverterOption.Format("yyyy-MM-dd").Index(6);
        Map(m => m.IsRoundTrip).Name("IsRoundTrip").Index(7);
        Map(m => m.UsedPoints).Name("UsedPoints").Index(8);
        Map(m => m.EarnedPoints).Name("EarnedPoints").Index(9);
        Map(m => m.Layover1).Name("Layover1").Index(10);
        Map(m => m.Layover2).Name("Layover2").Index(11);
        Map(m => m.Layover3).Name("Layover3").Index(12);
        Map(m => m.Layover4).Name("Layover4").Index(13);
    }
}
