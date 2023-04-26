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
                    if (timeDiff.TotalMinutes >= 60) //if flight has at lest one hour until departure
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
                else if(order.OrderStatus == "Printed")
                {
                    oldOrders.Add(order);
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

        public static List<Order> GetOrdersForBoardingPass(Customer customer)
        {
            int id = customer.Id;
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            // create lists to store various order types
            List<Order> currentOrders = new();

            while (true) //while order csv file has orders for a specific customer id, create separate order lists
            {
                if (CsvDatabase.FindRecord(orders, c => c.CustomerId, id) == null)
                {
                    break;
                }
                Order order = CsvDatabase.FindRecord(orders, p => p.CustomerId, id);

                if (order.OrderStatus != "Canceled")
                {
                    Flight endFlight = Flight.FindFlightById(order.FlightId1); //used to decide when to disallow cancellation 1 hour before takeoff
                    if (order.FlightId2 != -1)
                    {
                        endFlight = Flight.FindFlightById(order.FlightId2);
                    }
                    TimeSpan timeDiff = endFlight.DepartureTime - DateTime.Now;
                    if (timeDiff.TotalMinutes > 0) //if flight not departed
                    {
                        currentOrders.Add(order);
                    }
                }
                orders.Remove(order);
            }
            return currentOrders;
        }

        public static void ApplyPurchasePoints(Customer customer)
        {
            int id = customer.Id;
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);

            while (true)
            {
                if (CsvDatabase.FindRecord(orders, c => c.CustomerId, id) == null)
                {
                    break;
                }
                Order order = CsvDatabase.FindRecord(orders, p => p.CustomerId, id);
                Flight flight = Flight.FindFlightById(order.FlightId1); //credit points if not canceled
                TimeSpan timeDiff = flight.DepartureTime - DateTime.Now;
                if (order.OrderStatus != "Canceled" && timeDiff.TotalMinutes < 60 && order.EarnedPoints == false)
                {
                    Order.UpdateOrder(order, earnedPoints: true);
                    UpdatePoints(customer, flight.PointsEarned);
                    if (order.FlightId2 != -1)
                    {
                        Flight flight2 = Flight.FindFlightById(order.FlightId2);
                        UpdatePoints(customer, flight2.PointsEarned);
                    }
                }
                orders.Remove(order);
            }
        }

        public static List<Order> GetOrdersForBoardingPass(Customer customer)
        {
            int id = customer.Id;
            string filePath = @"..\..\..\Tables\OrderDb.csv";
            List<Order> orders = CsvDatabase.ReadCsvFile<Order>(filePath);
            // create lists to store various order types
            List<Order> currentOrders = new();

            while (true) //while order csv file has orders for a specific customer id, create separate order lists
            {
                if (CsvDatabase.FindRecord(orders, c => c.CustomerId, id) == null)
                {
                    break;
                }
                Order order = CsvDatabase.FindRecord(orders, p => p.CustomerId, id);

                if (order.OrderStatus != "Canceled" || order.OrderStatus != "Printed")
                {
                    Flight endFlight = Flight.FindFlightById(order.FlightId1); //used to decide when to disallow cancellation 1 hour before takeoff
                    if (order.FlightId2 != -1)
                    {
                        endFlight = Flight.FindFlightById(order.FlightId2);
                    }
                    TimeSpan timeDiff = endFlight.DepartureTime - DateTime.Now;
                    if (timeDiff.TotalMinutes > 0) //if flight not departed
                    {
                        currentOrders.Add(order);
                    }
                }
                orders.Remove(order);
            }
            return currentOrders;
        }

        public static void PrintFlightInfoForBoardingPass(int orderNumber, List<Order> orders)
        {
            Flight flight1 = null;
            Flight flight2 = null; //return flight
            Airport airport1 = null; //origin airport
            Airport airport2 = null; //destination airport
            flight1 = Flight.FindFlightById(orders[orderNumber].FlightId1);
            airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
            airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
            //print depart flight info
            if(flight1.FlightInfo == "direct")
            {
                if(orders[orderNumber].IsRoundTrip == true)
                {
                    Console.WriteLine(" Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ") (Round trip)");
                }
                else
                {
                    Console.WriteLine(" Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                }
                
            }
            else if(flight1.FlightInfo == "parent")
            {
                if (orders[orderNumber].IsRoundTrip == true)
                {
                    Console.WriteLine(" Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ") (Connection Flights) (Round Trip)");
                }
                else
                {
                    Console.WriteLine(" Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ") (Connection Flights)");
                }
            }
            /*if (orders[orderNumber].FlightId2 != -1) //print flight return info if roundtrip
            {

                flight2 = Flight.FindFlightById(orders[orderNumber].FlightId2);

                if (flight2.FlightInfo == "direct")
                {
                    Console.WriteLine(" Departing: " + flight2.FlightNumber + " flying from " + airport2.City + ", " + airport2.State + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                }
                else if (flight2.FlightInfo == "parent")
                {
                    Console.WriteLine(" Departing: " + flight2.FlightNumber + " flying from " + airport2.City + ", " + airport2.State + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ") (Will print all connection flight passes)");
                }
            }*/
        }

        public static void ViewAccountHistory(Customer customer)
        {
            GetOrders(customer); //get all order history
            Flight flight1 = null;
            Flight flight2 = null; //return flight
            Airport airport1 = null; //origin airport
            Airport airport2 = null; //destination airport

            //display relevant information
            Console.WriteLine("\n\n**** ACCOUNT HISTORY ****");
            Console.WriteLine("\nPoints Available: " + customer.MileagePoints);
            Console.WriteLine("Points Spent: " + customer.PointsSpent);
            Console.WriteLine("ID Number: " + customer.Id);
            Console.WriteLine("\nAll Flights History:");
            for (int i = 0; i < customer.AllOrders.Count; i++)
            {
                flight1 = Flight.FindFlightById(customer.AllOrders[i].FlightId1);
                flight1.PopulateLayovers();
                flight2 = null;
                airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
                airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
                // if connections 
                if(customer.AllOrders[i].Layover1 != -1)
                {
                    Console.WriteLine("\n\tOrder ID: " + customer.AllOrders[i].OrderId.ToString().PadLeft(7) + " (" + airport1.City + ", " + airport1.State + " to " + airport2.City + ", " + airport2.State + ")   Order Date: " + customer.AllOrders[i].OrderDate);
                    foreach (Flight layover in flight1.LayoverFlights)
                    {
                        Airport layoverDepart = Airport.FindAirportbyId(layover.DepartureAirportID);
                        Airport layoverArr = Airport.FindAirportbyId(layover.ArrivalAirportID);
                        Console.WriteLine("\tDeparting: " +layover.FlightNumber +" flying from "+ layoverDepart.City + ", " + layoverDepart.State + " flying to " + layoverArr.City + ", " + layoverArr.State + " (Departure: " + layover.DepartureTime + " - Arrival: " + layover.ArrivalTime + ")");
                    }
                }
                else
                {
                    Console.WriteLine("\n\tOrder ID: " + customer.AllOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.AllOrders[i].OrderDate);
                    Console.WriteLine("\tDeparting: " + flight1.FlightNumber + " flying from " + airport1.City + ", " + airport1.State + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                }
                // return flight info
                if (customer.AllOrders[i].FlightId2 != -1) //print flight return info if roundtrip
                {
                    flight2 = Flight.FindFlightById(customer.AllOrders[i].FlightId2);
                    flight2.PopulateLayovers();
                    if (customer.AllOrders[i].Layover1 != -1)
                    {
                        foreach (Flight layover in flight2.LayoverFlights)
                        {
                            Airport layoverDepart = Airport.FindAirportbyId(layover.DepartureAirportID);
                            Airport layoverArr = Airport.FindAirportbyId(layover.ArrivalAirportID);
                            Console.WriteLine("\tReturning: " + layover.FlightNumber + " flying from " + layoverDepart.City + ", " + layoverDepart.State + " flying to " + layoverArr.City + ", " + layoverArr.State + " (Departure: " + layover.DepartureTime + " - Arrival: " + layover.ArrivalTime + ")");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tReturning: " + flight2.FlightNumber + " flying from " + airport2.City + ", " + airport2.State + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
                    }
                }

            }

            // ACTIVE FLIGHTS
            Console.WriteLine("\nBooked Flights:");
            for (int i = 0; i < customer.ActiveOrders.Count; i++)
            {
                flight1 = Flight.FindFlightById(customer.ActiveOrders[i].FlightId1);
                flight1.PopulateLayovers();
                flight2 = null;
                airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
                airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
                
                //layovers
                if (customer.ActiveOrders[i].Layover1 != -1)
                {
                    Console.WriteLine("\n\tOrder ID:  " + customer.ActiveOrders[i].OrderId.ToString().PadLeft(7) + " (" + airport1.City + ", " + airport1.State + " to " + airport2.City + ", " + airport2.State + ")   Order Date: " + customer.ActiveOrders[i].OrderDate);
                    foreach (Flight layover in flight1.LayoverFlights)
                    {
                        Airport layoverDepart = Airport.FindAirportbyId(layover.DepartureAirportID);
                        Airport layoverArr = Airport.FindAirportbyId(layover.ArrivalAirportID);
                        Console.WriteLine("\tDeparting: " + flight1.FlightNumber + " flying from "+ layoverDepart.City + ", " + layoverDepart.State + " flying to " + layoverArr.City + ", " + layoverArr.State + " (Departure: " + layover.DepartureTime + " - Arrival: " + layover.ArrivalTime + ")");
                    }
                }
                //directs
                else
                {
                    Console.WriteLine("\n\tOrder ID:  " + customer.ActiveOrders[i].OrderId.ToString().PadLeft(7) + "  Order Date: " + customer.ActiveOrders[i].OrderDate);
                    Console.WriteLine("\tDeparting: " + flight1.FlightNumber + " flying from " + airport1.City + ", " + airport1.State + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                }
                // return flight info
                if (customer.ActiveOrders[i].FlightId2 != -1) //print flight return info if roundtrip
                {
                    flight2 = Flight.FindFlightById(customer.ActiveOrders[i].FlightId2);
                    flight2.PopulateLayovers();
                    if (customer.ActiveOrders[i].Layover1 != -1)
                    {
                        foreach (Flight layover in flight2.LayoverFlights)
                        {
                            Airport layoverDepart = Airport.FindAirportbyId(layover.DepartureAirportID);
                            Airport layoverArr = Airport.FindAirportbyId(layover.ArrivalAirportID);
                            Console.WriteLine("\tReturning: " + layover.FlightNumber + " flying from " + layoverDepart.City + ", " + layoverDepart.State + " flying to " + layoverArr.City + ", " + layoverArr.State + " (Departure: " + layover.DepartureTime + " - Arrival: " + layover.ArrivalTime + ")");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tReturning: " + flight2.FlightNumber + " flying from " + airport2.City + ", " + airport2.State + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
                    }
                }
            }
            //PREVIOUS FLIGHTS
            Console.WriteLine("\nPrevious Flights:");
            for (int i = 0; i < customer.OldOrders.Count; i++)
            {
                flight1 = Flight.FindFlightById(customer.OldOrders[i].FlightId1);
                flight1.PopulateLayovers();
                flight2 = null;
                airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
                airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);

                // layover output
                if (customer.OldOrders[i].Layover1 != -1)
                {
                    Console.WriteLine("\n\tOrder ID: " + customer.OldOrders[i].OrderId.ToString().PadLeft(7) + " (" + airport1.City + ", " + airport1.State + " to " + airport2.City + ", " + airport2.State + ")   Order Date: " + customer.OldOrders[i].OrderDate);
                    foreach(Flight layoverFlight in flight1.LayoverFlights)
                    {
                        Airport layoverDepart = Airport.FindAirportbyId(layoverFlight.DepartureAirportID);
                        Airport layoverArr = Airport.FindAirportbyId(layoverFlight.ArrivalAirportID);
                        Console.WriteLine("\tDeparting: " + layoverFlight.FlightNumber + " flying from " + layoverDepart.City + ", " + layoverDepart.State + " flying to " + layoverArr.City + ", " + layoverArr.State + " (Departure: " + layoverFlight.DepartureTime + " - Arrival: " + layoverFlight.ArrivalTime + ")");
                    }
                }
                // direct output
                else
                {
                    Console.WriteLine("\n\tOrder ID: " + customer.OldOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.OldOrders[i].OrderDate);
                    Console.WriteLine("\tDeparting: " + flight1.FlightNumber + " flying from " + airport1.City + ", " + airport1.State + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                }

                // return flight info
                if (customer.OldOrders[i].FlightId2 != -1) //print flight return info if roundtrip
                {
                    flight2 = Flight.FindFlightById(customer.OldOrders[i].FlightId2);
                    flight2.PopulateLayovers();
                    if (customer.OldOrders[i].Layover1 != -1)
                    {
                        foreach (Flight layover in flight2.LayoverFlights)
                        {
                            Airport layoverDepart = Airport.FindAirportbyId(layover.DepartureAirportID);
                            Airport layoverArr = Airport.FindAirportbyId(layover.ArrivalAirportID);
                            Console.WriteLine("\tReturning: " + layover.FlightNumber + " flying from " + layoverDepart.City + ", " + layoverDepart.State + " flying to " + layoverArr.City + ", " + layoverArr.State + " (Departure: " + layover.DepartureTime + " - Arrival: " + layover.ArrivalTime + ")");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tReturning: " + flight2.FlightNumber + " flying from " + airport2.City + ", " + airport2.State + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
                    }
                }
            }
            // CANCELLED FLIGHTS
            Console.WriteLine("\nCanceled Flights:");
            for (int i = 0; i < customer.CanceledOrders.Count; i++)
            {
                flight1 = Flight.FindFlightById(customer.CanceledOrders[i].FlightId1);
                flight1.PopulateLayovers();
                flight2 = null;
                airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
                airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);

                //lay over flights
                if (customer.CanceledOrders[i].Layover1 != -1)
                {
                    Console.WriteLine("\n\tOrder ID: " + customer.CanceledOrders[i].OrderId.ToString().PadLeft(7) + " (" + airport1.City + ", " + airport1.State + " to " + airport2.City + ", " + airport2.State + ")   Order Date: " + customer.CanceledOrders[i].OrderDate);
                    foreach (Flight layoverFlight in flight1.LayoverFlights)
                    {
                        Airport layoverDepart = Airport.FindAirportbyId(layoverFlight.DepartureAirportID);
                        Airport layoverArr = Airport.FindAirportbyId(layoverFlight.ArrivalAirportID);
                        Console.WriteLine("\tDeparting: " + layoverFlight.FlightNumber + " flying from " + layoverDepart.City + ", " + layoverDepart.State + " flying to " + layoverArr.City + ", " + layoverArr.State + " (Departure: " + layoverFlight.DepartureTime + " - Arrival: " + layoverFlight.ArrivalTime + ")");
                    }
                }
                //direct flights 
                else
                {
                    Console.WriteLine("\n\tOrder ID: " + customer.CanceledOrders[i].OrderId.ToString().PadLeft(7) + "   Order Date: " + customer.CanceledOrders[i].OrderDate);
                    Console.WriteLine("\tDeparting: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                }
                // return flight info
                if (customer.CanceledOrders[i].FlightId2 != -1) //print flight return info if roundtrip
                {
                    flight2 = Flight.FindFlightById(customer.CanceledOrders[i].FlightId2);
                    flight2.PopulateLayovers();
                    // layover flights
                    if (customer.CanceledOrders[i].Layover1 != -1)
                    {
                        foreach (Flight layoverFlight in flight2.LayoverFlights)
                        {
                            Airport layoverDepart = Airport.FindAirportbyId(layoverFlight.DepartureAirportID);
                            Airport layoverArr = Airport.FindAirportbyId(layoverFlight.ArrivalAirportID);
                            Console.WriteLine("\tReturning: " + layoverFlight.FlightNumber + " flying from " + layoverDepart.City + ", " + layoverDepart.State + " flying to " + layoverArr.City + ", " + layoverArr.State + " (Departure: " + layoverFlight.DepartureTime + " - Arrival: " + layoverFlight.ArrivalTime + ")");
                        }
                    }
                    // direct flight
                    else
                    {
                        Console.WriteLine("\tReturning: " + flight2.FlightNumber + " flying from " + airport2.City + ", " + airport2.State + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
                    }
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
            if(flight1.FlightInfo == "parent")
            {
                Console.WriteLine(" Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ") (includes all connections)");
            }
            else
            {
                Console.WriteLine(" Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
            }
            if (customer.ActiveOrders[orderNumber].FlightId2 != -1) //print flight return info if roundtrip
            {
                flight2 = Flight.FindFlightById(customer.ActiveOrders[orderNumber].FlightId2);
                Console.WriteLine("      Returning: " + flight2.FlightNumber + " flying to " + airport1.City + ", " + airport1.State + " (Departure: " + flight2.DepartureTime + " - Arrival: " + flight2.ArrivalTime + ")");
            }
        }

        public static void PrintFlightInfoForBoardingPass(int orderNumber, List<Order> orders)
        {
            Flight flight1 = null;
            Flight flight2 = null; //return flight
            Airport airport1 = null; //origin airport
            Airport airport2 = null; //destination airport
            flight1 = Flight.FindFlightById(orders[orderNumber].FlightId1);
            airport1 = Airport.FindAirportbyId(flight1.DepartureAirportID);
            airport2 = Airport.FindAirportbyId(flight1.ArrivalAirportID);
            //print depart flight info
            Console.WriteLine(" Departing: " + flight1.FlightNumber + " flying to " + airport2.City + ", " + airport2.State + " (Departure: " + flight1.DepartureTime + " - Arrival: " + flight1.ArrivalTime + ")");
            if (orders[orderNumber].FlightId2 != -1) //print flight return info if roundtrip
            {
                flight2 = Flight.FindFlightById(orders[orderNumber].FlightId2);
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
            flight1.PopulateLayovers();
            int pointsCost = 10 * flight1.PointsEarned;
            // with return flights
            if (flightId2 != -1)
            {
                Flight flight2 = Flight.FindFlightById(flightId2);
                flight2.PopulateLayovers();
                // bought with points
                if (withPoints == true)
                {
                    pointsCost += 10 * flight2.PointsEarned;

                    if (customer.MileagePoints < pointsCost) //price of flight in points
                    {
                        throw new Exception("Not Enough Points!");
                    }

                    UpdatePoints(customer, -pointsCost);
                    UpdatePointsSpent(customer, pointsCost);
                    // with layovers -- if the flight1 has a layover than flight 2 will as well 
                    if(flight1.LayoverFlights.Count != 0)
                    {
                        List<Flight> flight1Layovers = Flight.getLayoverFlights(flight1);
                        List<Flight> flight2Layovers = Flight.getLayoverFlights(flight2);
                        Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight2.ArrivalTime), true, true, false, flightId2, layover1: flight1Layovers[0].FlightId, layover2: flight1Layovers[1].FlightId, layover3: flight2Layovers[0].FlightId, layover4: flight2Layovers[1].FlightId);
                        foreach(Flight flight in flight1.LayoverFlights)
                        {
                            Flight.UpdateFlight(flight, seatsSold: flight1.SeatsSold+1);
                        }
                        foreach(Flight flight in flight2.LayoverFlights)
                        {
                            Flight.UpdateFlight(flight, seatsSold: flight2.SeatsSold+1);
                        }
                        return;
                    }
                    // direct flight
                    else
                    {
                        Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight2.ArrivalTime), true, true, false, flightId2);
                        Flight.UpdateFlight(flight1, seatsSold: flight1.SeatsSold + 1);
                        Flight.UpdateFlight(flight2, seatsSold: flight2.SeatsSold + 1);
                        return;
                    }
                    
                }
                // bought with money
                else
                {
                    // if layovers
                    if (flight1.LayoverFlights.Count != 0)
                    {
                        List<Flight> flight1Layovers = Flight.getLayoverFlights(flight1);
                        List<Flight> flight2Layovers = Flight.getLayoverFlights(flight2);
                        Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight2.ArrivalTime), true, false, false, flightId2, layover1: flight1Layovers[0].FlightId, layover2: flight1Layovers[1].FlightId, layover3: flight2Layovers[0].FlightId, layover4: flight2Layovers[1].FlightId);
                        foreach (Flight flight in flight1.LayoverFlights)
                        {
                            Flight.UpdateFlight(flight, seatsSold: flight1.SeatsSold + 1);
                        }
                        foreach (Flight flight in flight2.LayoverFlights)
                        {
                            Flight.UpdateFlight(flight, seatsSold: flight2.SeatsSold + 1);
                        }
                        return;
                    }
                    // direct flight
                    else
                    {
                        Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight2.ArrivalTime), true, false, false, flightId2);
                        Flight.UpdateFlight(flight1, seatsSold: flight1.SeatsSold + 1);
                        Flight.UpdateFlight(flight2, seatsSold: flight2.SeatsSold + 1);
                        return;
                    }
                }
            }
            // one way trip
            else
            {
                // bought with points
                if (withPoints == true)
                {
                    if (customer.MileagePoints < pointsCost) //price of flight in points
                    {
                        throw new Exception("Not Enough Points!");
                    }

                    UpdatePoints(customer, -pointsCost);
                    UpdatePointsSpent(customer, pointsCost);

                    // if layovers
                    if (flight1.LayoverFlights.Count != 0)
                    {
                        Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight1.ArrivalTime), false, true, false, layover1: (flightId1 + 1), layover2: (flightId1 + 2));
                        foreach(Flight flight in flight1.LayoverFlights)
                        {
                            Flight.UpdateFlight(flight, seatsSold: flight.SeatsSold + 1);
                        }
                        return;
                    }
                    // direct flight 
                    else
                    {
                        Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight1.ArrivalTime), false, true, false);
                        Flight.UpdateFlight(flight1, seatsSold: flight1.SeatsSold + 1);
                        return;
                    }
                }
                // bought with money
                else
                {
                    // if layovers
                    if (flight1.LayoverFlights.Count != 0)
                    {
                        Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight1.ArrivalTime), false, false, false, layover1: (flightId1 + 1), layover2: (flightId1 + 2));
                        foreach (Flight flight in flight1.LayoverFlights)
                        {
                            Flight.UpdateFlight(flight, seatsSold: flight.SeatsSold + 1);
                        }
                        return;
                    }
                    // if direct 
                    else
                    {
                        Order.CreateOrder(customer.Id, flightId1, "Active", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(flight1.ArrivalTime), false, false, false);
                        Flight.UpdateFlight(flight1, seatsSold: flight1.SeatsSold + 1);
                        return;
                    }
                }
            }
        }
    }
}
