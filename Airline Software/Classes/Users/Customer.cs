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
        public int PointsSpent { get; set; }
        public string? CreditCardNumber { get; set; }

        public List<Order> AllOrders;

        public List<Order> ActiveOrders;

        public List<Order> OldOrders;

        public List<Order> CanceledOrders;

        // Costructor
        public Customer(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType, string CreditCardNumber, int MileagePoints, int PointsSpent)
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {
            this.MileagePoints = MileagePoints;
            this.PointsSpent = PointsSpent;
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
            Customer newCustomer = new Customer(id, firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, userType, creditCardNumber, 0, 0);
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
                    Flight endFlight = Flight.FindFlightById(order.FlightId1); //used to decide when to disallow cancellation 1 hour before takeoff
                    TimeSpan timeDiff = endFlight.DepartureTime - DateTime.Now;
                    if (timeDiff.TotalMinutes >= 60) //if at least an hour before takeoff
                    {
                        activeOrders.Add(order);
                    }
                    else
                    {
                        order.OrderStatus = "Inactive";
                        Order.UpdateOrder(order, orderStatus: "Inactive");
                        oldOrders.Add(order);
                    }
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
            Flight flight1 = null;
            Flight flight2 = null; //return flight
            Airport airport1 = null; //origin airport
            Airport airport2 = null; //destination airport

            //display relevant information
            //TODO WE NEED POINTS USED TOO????????
            Console.WriteLine("\n\n**** ACCOUNT HISTORY ****");
            Console.WriteLine("\nPoints Available: " + customer.MileagePoints);
            Console.WriteLine("Points Spent: " + customer.PointsSpent);
            Console.WriteLine("ID Number: " + customer.Id);
            Console.WriteLine("\nAll Flights History:");
            for (int i = 0; i < customer.AllOrders.Count; i++)
            {
                flight1 = Flight.FindFlightById(customer.AllOrders[i].FlightId1);
                flight2 = null;
                airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
                airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
                Console.WriteLine("  Order ID: " + customer.AllOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.AllOrders[i].OrderDate);
                Console.WriteLine("    Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                if (customer.AllOrders[i].FlightId2 != -1) //print flight return info if roundtrip
                {
                    flight2 = Flight.FindFlightById(customer.AllOrders[i].FlightId2);
                    Console.WriteLine("    Returning: " + flight2.FlightNumber + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
                }
            }

            Console.WriteLine("\nBooked Flights:");
            for (int i = 0; i < customer.ActiveOrders.Count; i++)
            {
                flight1 = Flight.FindFlightById(customer.ActiveOrders[i].FlightId1);
                flight2 = null;
                airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
                airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
                Console.WriteLine("  Order ID: " + customer.ActiveOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.ActiveOrders[i].OrderDate);
                Console.WriteLine("    Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                if (customer.ActiveOrders[i].FlightId2 != -1) //print flight return info if roundtrip
                {
                    flight2 = Flight.FindFlightById(customer.ActiveOrders[i].FlightId2);
                    Console.WriteLine("    Returning: " + flight2.FlightNumber + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
                }
            }

            Console.WriteLine("\nPrevious Flights:");
            for (int i = 0; i < customer.OldOrders.Count; i++)
            {
                flight1 = Flight.FindFlightById(customer.OldOrders[i].FlightId1);
                flight2 = null;
                airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
                airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
                Console.WriteLine("  Order ID: " + customer.OldOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.OldOrders[i].OrderDate);
                Console.WriteLine("    Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                if (customer.OldOrders[i].FlightId2 != -1) //print flight return info if roundtrip
                {
                    flight2 = Flight.FindFlightById(customer.OldOrders[i].FlightId2);
                    Console.WriteLine("    Returning: " + flight2.FlightNumber + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
                }
            }

            Console.WriteLine("\nCanceled Flights:");
            for (int i = 0; i < customer.CanceledOrders.Count; i++)
            {
                flight1 = Flight.FindFlightById(customer.CanceledOrders[i].FlightId1);
                flight2 = null;
                airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
                airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
                Console.WriteLine("  Order ID: " + customer.CanceledOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.CanceledOrders[i].OrderDate);
                Console.WriteLine("    Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                if (customer.CanceledOrders[i].FlightId2 != -1) //print flight return info if roundtrip
                {
                    flight2 = Flight.FindFlightById(customer.CanceledOrders[i].FlightId2);
                    Console.WriteLine("    Returning: " + flight2.FlightNumber + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
                }
            }
        }

        public static void PrintFlightInfo(Customer customer, int orderNumber)
        {
            Flight flight1 = null;
            Flight flight2 = null; //return flight
            Airport airport1 = null; //origin airport
            Airport airport2 = null; //destination airport
            flight1 = Flight.FindFlightById(customer.ActiveOrders[orderNumber].FlightId1);
            airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
            airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
            //print depart flight info
            Console.WriteLine(" Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
            if (customer.ActiveOrders[orderNumber].FlightId2 != -1) //print flight return info if roundtrip
            {
                flight2 = Flight.FindFlightById(customer.ActiveOrders[orderNumber].FlightId2);
                Console.WriteLine("      Returning: " + flight2.FlightNumber + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
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

        public static void UpdatePointsSpent(Customer customer, int points)
        {
            customer.PointsSpent += points;

            string filePath = @"..\..\..\Tables\CustomerDb.csv";
            List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);

            //update customer info in csv file
            CsvDatabase.UpdateRecord(customers, p => p.Id, customer.Id, (current, updated) =>
            {
                current.PointsSpent = updated.PointsSpent;
            }, customer);

            CsvDatabase.WriteCsvFile(filePath, customers);
        }

        public static void BookTrip(Customer customer, bool withPoints, int flightId1, int flightId2 = -1)
        {
            Flight flight1 = Flight.FindFlightById(flightId1);
            int pointsCost = 10 * flight1.PointsEarned;

            if (flightId2 != -1)
            {
                Flight flight2 = Flight.FindFlightById(flightId2);

                if (withPoints == true)
                {
                    pointsCost += 10 * flight2.PointsEarned;

                    if (customer.MileagePoints < pointsCost) //price of flight in points
                    {
                        throw new Exception("Not Enough Points!");
                    }

                    UpdatePoints(customer, -pointsCost);
                    UpdatePointsSpent(customer, pointsCost);
                    Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight2.ArrivalTime), true, true, flightId2);
                    Flight.UpdateFlight(flight1, seatsSold: flight1.SeatsSold + 1);
                    Flight.UpdateFlight(flight2, seatsSold: flight2.SeatsSold + 1);
                    return;
                }

                Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight2.ArrivalTime), true, false, flightId2);
                Flight.UpdateFlight(flight1, seatsSold: flight1.SeatsSold + 1);
                Flight.UpdateFlight(flight2, seatsSold: flight2.SeatsSold + 1);
            }
            else
            {
                if (withPoints == true)
                {
                    if (customer.MileagePoints < pointsCost) //price of flight in points
                    {
                        throw new Exception("Not Enough Points!");
                    }

                    UpdatePoints(customer, -pointsCost);
                    UpdatePointsSpent(customer, pointsCost);
                    Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight1.ArrivalTime), false, true);
                    Flight.UpdateFlight(flight1, seatsSold: flight1.SeatsSold + 1);
                    return;
                }

                Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight1.ArrivalTime), false, false);
                Flight.UpdateFlight(flight1, seatsSold: flight1.SeatsSold + 1);
            }
        }
    }
}
