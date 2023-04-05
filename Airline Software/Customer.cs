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
        public string CreditCardNumber { get; set; }

        public List<Order> OrderHistory;

        public List<Order> ActiveOrders;


        // constructor 
        public Customer(int id, int age, string phoneNumber, string Address, string firstName, string lastName, string password, string creditCardNumber)
        {
            this.Id = id;
            this.Age = age;
            this.PhoneNumber = phoneNumber;
            this.Address = Address;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Password = password;
            this.MilagePoints = 0;
            this.UserType = 0;
            this.CreditCardNumber = creditCardNumber;

            this.OrderHistory = new List<Order>();
            this.ActiveOrders = new List<Order>();
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
