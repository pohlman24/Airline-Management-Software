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
        public static void SaveFlightManifest(Flight flight)
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
                // find everyone on a given flight by searching the orders table and matching flight ID
                foreach (Order order in orders)
                {
                    if (order.FlightId1 == flight.FlightId || order.FlightId2 == flight.FlightId)
                    {
                        Customer customer = Customer.FindCustomerById(order.CustomerId);
                        string customerName = string.Join(",", customer.FirstName, customer.LastName);
                        writer.WriteLine(customerName);
                    }
                }
            }
            Console.WriteLine("flight-manifest.csv File saved in your documents folder");
        }

        public static void PrintFlightManifest(Flight flight)
        {
            // if the flight hasnt left yet, than output list of every customer who has bought a ticket for given flight
            if (DateTime.Now < flight.DepartureTime)
            {
                Console.WriteLine("The flight has not departed yet. It's departure time is: " + flight.DepartureTime);
                Console.WriteLine("A list of passengers with tickets will be shown instead");
                if (flight.SeatsSold == 0)
                {
                    Console.WriteLine("\nFlight has no passengers");
                    return;
                }
                // read over every Order and check if flightId is same as given flight
                string filePath = @"..\..\..\Tables\OrderDb.csv";
                List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
                Console.WriteLine("\nFirst Name Last Name");
                // find everyone on a given flight by searching the orders table and matching flight ID
                foreach (Order order in orders)
                {
                    if (order.FlightId1 == flight.FlightId || order.FlightId2 == flight.FlightId)
                    {
                        Customer customer = Customer.FindCustomerById(order.CustomerId);
                        string customerName = string.Join(",", customer.FirstName, customer.LastName);
                        Console.WriteLine(customerName);
                    }
                }
            }
            // else generate list of all customers with boardingpass printed for given flight (i.e they actually boarded the flight)
            else
            {
                if (flight.SeatsSold == 0)
                {
                    Console.WriteLine("\nFlight has no passengers");
                    return;
                }
                // read over every boardingpass and check if flightId is same as given flight
                string filePath = @"..\..\..\Tables\BoardingPass.csv";
                List<BoardingPass> boardingPasses = CsvDatabase.ReadCsvFile<BoardingPass>(filePath);
                Console.WriteLine("\nFirst Name Last Name");
                // find everyone on a given flight by searching the orders table and matching flight ID
                foreach(BoardingPass bp in boardingPasses)
                {
                    if (bp.FlightId == flight.FlightId)
                    {
                        Customer customer = Customer.FindCustomerById(bp.CustomerId);
                        string customerName = string.Join(",", customer.FirstName, customer.LastName);
                        Console.WriteLine(customerName);
                    }
                }
            }
            
        }

    }
}
