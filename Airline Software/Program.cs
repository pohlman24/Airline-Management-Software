using Airline_Software;
using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
       
        // Testing creating a new customer -- should create the record in the customer table and the user table
        /*Console.WriteLine("Enter First Name");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter Last Name");
        string lastName = Console.ReadLine();
        Console.WriteLine("Enter Email");
        string email = Console.ReadLine();
        Console.WriteLine("Enter Phone Number");
        string phoneNumber = Console.ReadLine();
        Console.WriteLine("Enter Age");
        string age = Console.ReadLine();
        Console.WriteLine("Enter Address");
        string address = Console.ReadLine();
        Console.WriteLine("Enter City");
        string city = Console.ReadLine();
        Console.WriteLine("Enter State");
        string state = Console.ReadLine();
        Console.WriteLine("Enter Zip");
        string zip = Console.ReadLine();
        Console.WriteLine("Enter Password");
        string password = Console.ReadLine();
        Console.WriteLine("Enter Credit Card Number");
        string creditCardNum = Console.ReadLine();

        User newUser = Customer.CreateCustomer(firstName, lastName, email, phoneNumber, int.Parse(age), address, city, state, zip, password, "Customer", creditCardNum);*/

        //Testing FindRecord
        string filePath = @"..\..\..\Tables\CustomerDb.csv";
        List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);
        Customer me = CsvDatabase.FindRecord(customers, p => p.Id, 364608);
        Console.WriteLine(me.FirstName);

        //testing updating the name
        Customer.UpdateCustomer(me, firstName: "Bob", lastName: "Johnson");
        Console.WriteLine(me.FirstName);
        Console.WriteLine(me.LastName);
    }

}