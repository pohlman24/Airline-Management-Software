using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class FlightManager : User
    {
        public FlightManager(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType) 
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {

        }

        // make csv file with list of every customer on flight
        public static void PrintFlightManifest(Flight flight)
        {
            // read over every Order and check if flightId is same as given flight
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            //create .csv file to store list of all passengers that can be displayed
            string outputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "flight-manifest.csv");
            using (StreamWriter writer = new(outputFilePath))
            {
                // write header row with column names
                writer.WriteLine("First Name,Last Name");
                foreach (Order order in orders)
                {
                    if (order.FlightId == flight.FlightId)
                    {
                        Customer customer = Customer.FindCustomerById(order.CustomerId);
                        string customerName = string.Join(",", customer.FirstName, customer.LastName);
                        writer.WriteLine(customerName);
                    }
                }
            }
            Console.WriteLine("flight-manifest.csv File saved in your documents folder");
        }
    }
}
