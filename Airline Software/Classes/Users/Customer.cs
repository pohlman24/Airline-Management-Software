using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    public class Customer : User
    {
        public int MileagePoints { get; set; }
        public string? CreditCardNumber { get; set; }

        public List<Order> AllOrders;

        public List<Order> ActiveOrders;

        public List<Order> OldOrders;

        public List<Order> CanceledOrders;

        // Costructor
        public Customer(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType, string CreditCardNumber, int MileagePoints) 
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {
            this.MileagePoints = MileagePoints;
            this.CreditCardNumber = CreditCardNumber;
            this.AllOrders = new List<Order>();
            this.ActiveOrders = new List<Order>();
            this.OldOrders = new List<Order>();
            this.CanceledOrders = new List<Order>();
            this.UserType = "Customer";
        }

        public static Customer CreateCustomer(string firstName, string lastName, string email, string phoneNumber, int age, string address, string city, string state, string zipCode, string password, string userType, string creditCardNumber)
        {
            int id = User.GenerateId();
            Customer newCustomer = new Customer (id,firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, userType, creditCardNumber, 0);
            string filePath = @"..\..\..\Tables\CustomerDb.csv";
            List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);
            customers.Add(newCustomer);
            CsvDatabase.WriteCsvFile(filePath, customers);
            
            User.CreateUser(id, firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, userType);
            return newCustomer;
        }

        //TODO should u be able to change age, also should we have birthday or just age or do we have birthday i didnt look
        public static void UpdateCustomer(Customer customer, string firstName = "", string lastName = "", string email = "", string phoneNumber = "", int age = -1, string address = "", string city = "", string state = "", string zipCode = "", string userType = "", string creditCardNumber = "")
        {
            //update customer info if changed
            customer.FirstName = string.IsNullOrEmpty(firstName) ? customer.FirstName : firstName;
            customer.LastName = string.IsNullOrEmpty(lastName) ? customer.LastName : lastName;
            customer.Email = string.IsNullOrEmpty(email) ? customer.Email : email;
            customer.PhoneNumber = string.IsNullOrEmpty(phoneNumber) ? customer.PhoneNumber : phoneNumber;
            customer.Age = age == -1 ? customer.Age : age;
            customer.Address = string.IsNullOrEmpty(address) ? customer.Address : address;
            customer.City = string.IsNullOrEmpty(city) ? customer.City : city;
            customer.State = string.IsNullOrEmpty(state) ? customer.State : state;
            customer.ZipCode = string.IsNullOrEmpty(zipCode) ? customer.ZipCode : zipCode;
            customer.UserType = string.IsNullOrEmpty(userType) ? customer.UserType : userType;
            customer.CreditCardNumber = string.IsNullOrEmpty(creditCardNumber) ? customer.CreditCardNumber : creditCardNumber;

            //get csv file to update
            string filePath = @"..\..\..\Tables\CustomerDb.csv";
            List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);

            //update customer info in csv file
            CsvDatabase.UpdateRecord(customers, p => p.Id, customer.Id, (current, updated) =>
            {
                current.FirstName = updated.FirstName;
                current.LastName = updated.LastName;
                current.Email = updated.Email;
                current.PhoneNumber = updated.PhoneNumber;
                current.Age = updated.Age;
                current.Address = updated.Address;
                current.City = updated.City;
                current.State = updated.State;
                current.ZipCode = updated.ZipCode;
                current.UserType = updated.UserType;
                current.CreditCardNumber = updated.CreditCardNumber;
            }, customer);

            CsvDatabase.WriteCsvFile(filePath, customers);

            //update the user csv as well since customer is in both
            UpdateUser(customer, firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, userType);
        }

        public static void DeleteCustomer(Customer customer)
        {
            string filePath = @"..\..\..\Tables\CustomerDb.csv";
            List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);
            CsvDatabase.RemoveRecord(customers, c => c.Id, customer.Id);
            CsvDatabase.WriteCsvFile(filePath, customers);

            DeleteUser(customer);
        }

        public static Customer FindCustomerById(int id)
        {
            string filePath = @"..\..\..\Tables\CustomerDb.csv";
            List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);
            if (CsvDatabase.FindRecord(customers, c => c.Id, id) == null)
            {
                throw (new Exception("Id Not Found"));
                return null;
            }
            Customer customer = CsvDatabase.FindRecord(customers, p => p.Id, id);
            return customer;
        }

        public static void GetOrders(Customer customer)
        {
            int id = customer.Id;
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            // create lists to store various order types
            List<Order> allOrders = new();
            List<Order> activeOrders = new();
            List<Order> oldOrders = new();
            List<Order> canceledOrders = new();

            while (true) //while order csv file has orders for a specific customer id, create separate order lists
            {
                if (CsvDatabase.FindRecord(orders, c => c.CustomerId, id) == null)
                {
                    break;
                }
                Order order = CsvDatabase.FindRecord(orders, p => p.CustomerId, id);
                allOrders.Add(order);

                if (order.OrderStatus == "Active")
                {
                    activeOrders.Add(order);
                }
                else if (order.OrderStatus == "Inactive")
                {
                    oldOrders.Add(order);
                }
                else if (order.OrderStatus == "Canceled")
                {
                    canceledOrders.Add(order);
                }
                else
                {
                    throw new Exception("Invalid Order Status!");
                }

                orders.Remove(order);
            }

            //update customer lists with created lists
            customer.AllOrders = allOrders;
            customer.ActiveOrders = activeOrders;
            customer.OldOrders = oldOrders;
            customer.CanceledOrders = canceledOrders;
        }

        public static void ViewAccountHistory(Customer customer)
        {
            GetOrders(customer); //get all order history

            //display relevant information
            //TODO WE NEED POINTS USED TOO????????
            Console.WriteLine("Points Available:      " + customer.MileagePoints + "\n");

            Console.WriteLine("All Flights History:   ");
            for (int i = 0; i < customer.AllOrders.Count; i++)
            {
                Console.WriteLine("                     Order ID: " + customer.AllOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.AllOrders[i].OrderDate + "\n");
            }

            Console.WriteLine("Booked Flights:        ");
            for (int i = 0; i < customer.ActiveOrders.Count; i++)
            {
                Console.WriteLine("                     Order ID: " + customer.ActiveOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.ActiveOrders[i].OrderDate + "\n");
            }

            Console.WriteLine("Previous Flights:      ");
            for (int i = 0; i < customer.OldOrders.Count; i++)
            {
                Console.WriteLine("                     Order ID: " + customer.OldOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.OldOrders[i].OrderDate + "\n");
            }

            Console.WriteLine("Canceled Flights:      ");
            for (int i = 0; i < customer.CanceledOrders.Count; i++)
            {
                Console.WriteLine("                     Order ID: " + customer.CanceledOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.CanceledOrders[i].OrderDate + "\n");
            }
        }

        public static void ChangeCustomerPassword(Customer customer, string newPassword)
        {
            //set new password for user object
            customer.Password = ChangeUserPassword(customer, newPassword);

            string filePath = @"..\..\..\Tables\CustomerDb.csv";
            List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);

            //update customer info in csv file
            CsvDatabase.UpdateRecord(customers, p => p.Id, customer.Id, (current, updated) =>
            {
                current.Password = updated.Password;
            }, customer);

            CsvDatabase.WriteCsvFile(filePath, customers);
        }

        /// <summary>
        /// User checks out with all items in cart and adds order 
        /// </summary>
        
        public static void UpdatePoints(Customer customer, int points)
        {
            customer.MileagePoints += points;

            string filePath = @"..\..\..\Tables\CustomerDb.csv";
            List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);

            //update customer info in csv file
            CsvDatabase.UpdateRecord(customers, p => p.Id, customer.Id, (current, updated) =>
            {
                current.MileagePoints = updated.MileagePoints;
            }, customer);

            CsvDatabase.WriteCsvFile(filePath, customers);
        }

        public void PrintBoardingPass(int OrderId)
        {
            // print to console all the flight details
            // flight number, first and last name, depature location & time, arrival location & time,
        }

        public static void BookTrip(Customer customer, bool withPoints, int flightId1, int flightId2 = -1)
        {
            if (flightId2 != -1)
            {
                Flight flight1 = Flight.FindFlightById(flightId1);
                Flight flight2 = Flight.FindFlightById(flightId2);

                if (withPoints == true)
                {
                    int pointsCost = 10 * (flight1.PointsEarned + flight2.PointsEarned);

                    if (customer.MileagePoints < pointsCost) //price of flight in points
                    {
                        Console.WriteLine(customer.MileagePoints);
                        throw new Exception("Not Enough Points!");
                    }

                    UpdatePoints(customer, -pointsCost);
                }

                Order.CreateOrder(customer.Id, flightId1, "Active", new DateOnly(), new DateOnly(), true, flightId2); //TODO change these DateOnly things
            }
            else
            {
                Order.CreateOrder(customer.Id, flightId1, "Active", new DateOnly(), new DateOnly(), false); //TODO change these DateOnly things
            }
        }
    }
}
