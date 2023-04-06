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
        public int MilagePoints { get; set; }
        public string? CreditCardNumber { get; set; }

        public List<Order> OrderHistory;

        public List<Order> ActiveOrders;

        // Costructor
        public Customer(int Id, string FirstName, string LastName, string Email, string PhoneNumber, int Age, string Address, string City, string State, string ZipCode, string Password, string UserType, string CreditCardNumber) 
            : base(Id, FirstName, LastName, Email, PhoneNumber, Age, Address, City, State, ZipCode, Password, UserType)
        {
            this.MilagePoints = MilagePoints;
            this.CreditCardNumber = CreditCardNumber;
            this.OrderHistory = new List<Order>();
            this.ActiveOrders = new List<Order>();
            this.UserType = "Customer";
        }

        public static Customer CreateCustomer(string firstName, string lastName, string email, string phoneNumber, int age, string address, string city, string state, string zipCode, string password, string userType, string creditCardNum)
        {
            int id = User.GenerateId();
            Customer newCustomer = new Customer (id,firstName, lastName, email,phoneNumber, age, address, city, state, zipCode, password, userType, creditCardNum);
            string filePath = "C:/Users/Reece/Code/Airline-Management-Software/Airline Software/Tables/CustomerDb.csv";
            List<Customer> customers = CsvDatabase.ReadCsvFile<Customer>(filePath);
            customers.Add(newCustomer);
            CsvDatabase.WriteCsvFile(filePath, customers);

            User.CreateUser(id, firstName, lastName, email, phoneNumber, age, address, city, state, zipCode, password, userType);
            return newCustomer;
        }

        /// <summary>
        /// User checks out with all items in cart and adds order 
        /// </summary>
        public void AddOrder()
        {
            OrderHistory = new List<Order>();
        }
        
        public void CancelOrder()
        {
    
        }

        public void PrintBoardingPass(int OrderId)
        {
            // print to console all the flight details
            // flight number, first and last name, depature location & time, arrival location & time,
        }
    }
}
