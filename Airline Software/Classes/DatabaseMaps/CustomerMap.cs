using CsvHelper.Configuration;
using Airline_Software;

public class CustomerMap : ClassMap<Customer>
{
    public CustomerMap()
    {
        Map(m => m.Id).Index(0).Name("Id");
        Map(m => m.FirstName).Index(1).Name("FirstName");
        Map(m => m.LastName).Index(2).Name("LastName");
        Map(m => m.Email).Index(3).Name("Email");
        Map(m => m.PhoneNumber).Index(4).Name("PhoneNumber");
        Map(m => m.Age).Index(5).Name("Age");
        Map(m => m.Password).Index(6).Name("Password");
        Map(m => m.Address).Index(7).Name("Address");
        Map(m => m.City).Index(8).Name("City");
        Map(m => m.State).Index(9).Name("State");
        Map(m => m.ZipCode).Index(10).Name("ZipCode");
        Map(m => m.UserType).Index(11).Name("UserType");
        Map(m => m.MileagePoints).Index(12).Name("MileagePoints");
        Map(m => m.CreditCardNumber).Index(13).Name("CreditCardNumber");
        Map(m => m.PointsSpent).Index(14).Name("PointsSpent");
    }
}
