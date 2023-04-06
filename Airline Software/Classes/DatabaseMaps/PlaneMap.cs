using CsvHelper.Configuration;
using Airline_Software;

public class PlaneMap : ClassMap<Plane>
{
    public PlaneMap()
    {
        Map(m => m.Model).Name("Model").Index(0);
        Map(m => m.Capacity).Name("Capacity").Index(1);
        Map(m => m.PlaneId).Name("PlaneId").Index(2);
    }
}
