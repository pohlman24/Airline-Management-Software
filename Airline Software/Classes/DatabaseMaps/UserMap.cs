using CsvHelper.Configuration;
using Airline_Software;

public class UserMap : ClassMap<User>
{
    public UserMap()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.FirstName).Index(1);
        Map(m => m.LastName).Index(2);
        Map(m => m.Email).Index(3);
        Map(m => m.PhoneNumber).Index(4);
        Map(m => m.Age).Index(5);
        Map(m => m.Password).Index(6);
        Map(m => m.Address).Index(7);
        Map(m => m.City).Index(8);
        Map(m => m.State).Index(9);
        Map(m => m.ZipCode).Index(10);
        Map(m => m.UserType).Index(11);
    }
}
