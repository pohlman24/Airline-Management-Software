using CsvHelper.Configuration;
using Airline_Software;

public class AirportMap : ClassMap<Airport>
{
    public AirportMap()
    {
        Map(a => a.AirportId).Index(0).Name("AirportId");
        Map(a => a.City).Index(1).Name("City");
        Map(a => a.State).Index(2).Name("State");
        Map(a => a.Code).Index(3).Name("Code");
        Map(a => a.Latitude).Index(4).Name("Latitude");
        Map(a => a.Longitude).Index(5).Name("Longitude");
    }
}
