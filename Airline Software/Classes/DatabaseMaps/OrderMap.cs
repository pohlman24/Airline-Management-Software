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
    }
}
